using FluentFlyouts.Calendar.Classes;
using FluentFlyouts.Core.Interfaces;
using FluentFlyouts.Flyouts;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Timers;
using TerraFX.Interop.Windows;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using static System.Net.Mime.MediaTypeNames;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.Calendar.Flyouts
{
	public sealed partial class CalendarFlyout : UserControl, IFlyoutContent
	{
		private DispatcherTimer timer;
		private TrayIcon? trayIcon;
		public CalendarFlyout(TrayIcon trayIcon)
		{
			this.InitializeComponent();
			this.trayIcon = trayIcon;
			timer = new DispatcherTimer();
			Timer_Tick(null, null);
			timer.Interval = TimeSpan.FromSeconds(1.0);
			timer.Tick += Timer_Tick;
			timer.Start();

			var uiSettings = new UISettings();
			trayIcon?.UpdateIcon($"{(uiSettings.GetColorValue(UIColorType.Background) == Colors.Black ? "Calendar/Assets/CalendarDark.ico" : "Calendar/Assets/CalendarLight.ico")}");
			uiSettings.ColorValuesChanged += (sender, e) => {
				trayIcon?.UpdateIcon($"{(uiSettings.GetColorValue(UIColorType.Background) == Colors.Black ? "Calendar/Assets/CalendarDark.ico" : "Clock/Assets/CalendarLight.ico")}");
			};
		}
		public CalendarFlyout()
		{
			this.InitializeComponent();
			timer = new DispatcherTimer();
			Timer_Tick(null, null);
			timer.Interval = TimeSpan.FromSeconds(1.0);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object sender, object e)
		{
			var now = DateTime.Now;

			bool is24HourClock = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Contains("H");

			Time.Text = is24HourClock
				? now.ToString("HH:mm:ss", CultureInfo.InvariantCulture) // 24-hour format
				: now.ToString("h:mm:ss", CultureInfo.InvariantCulture); // 12-hour format

			AMPM.Text = is24HourClock
				? "" // Empty string for 24-hour format
				: now.ToString("tt", CultureInfo.InvariantCulture); // AM/PM for 12-hour format

			trayIcon?.UpdateTooltip($"{now.ToString("dddd, MMMM d yyyy")}\n{now.ToString((is24HourClock ? "HH:mm" : "h:mm tt"))}");
			CalendarTime.Content = DateTime.Now.ToString("dddd") + ", " + DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture) + " " + DateTime.Today.Date.ToString("dd") + ", " + DateTime.Now.Year.ToString();
		}

		private async void CalendarBox_GotFocus(object sender, RoutedEventArgs e)
		{
			//  Events.Visibility = Visibility.Collapsed;
			//  NewTime.Visibility = Visibility.Visible;
		}

		private void CalendarBox_LostFocus(object sender, RoutedEventArgs e)
		{
			// NewTime.Visibility = Visibility.Collapsed;
			//Events.Visibility = Visibility.Visible;
		}

		private async void AddButton_Click(object sender, RoutedEventArgs e)
		{
			var appointment = new Windows.ApplicationModel.Appointments.Appointment();

			// Get the selection rect of the button pressed to add this appointment
			Rect r = new Rect();
			r.Height = 30;
			r.Width = 50;
			r.X = 0;
			r.Y = 0;
			// ShowAddAppointmentAsync returns an appointment id if the appointment given was added to the user's calendar.
			// This value should be stored in app data and roamed so that the appointment can be replaced or removed in the future.
			// An empty string return value indicates that the user canceled the operation before the appointment was added.
			String appointmentId = await Windows.ApplicationModel.Appointments.AppointmentManager.ShowAddAppointmentAsync(
								   appointment, r, Windows.UI.Popups.Placement.Default);
		}

		private async void EventsListView_Loaded(object sender, RoutedEventArgs e)
		{
			AppointmentStore appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);
			ObservableCollection<CalendarItem> EventList = new ObservableCollection<CalendarItem>();
			var dateToShow = DateTime.Now.AddDays(0);
			var duration = TimeSpan.FromHours(24);

			var appCalendars = await appointmentStore.FindAppointmentsAsync(dateToShow, duration);
			foreach (var calendar in appCalendars)
			{
				Visibility Location = Visibility.Visible;
				Visibility Details = Visibility.Visible;
				if (String.IsNullOrEmpty(calendar.Location) == true)
				{
					Location = Visibility.Collapsed;
				}
				if (String.IsNullOrEmpty(calendar.Details) == true)
				{
					Details = Visibility.Collapsed;
				}
				Color color;
				if (calendar.IsCanceledMeeting == true)
				{
					color = Color.FromArgb(255, 233, 110, 96);
				}
				else if (calendar.HasInvitees == true)
				{
					color = Color.FromArgb(255, 233, 211, 120);
				}
				else if (calendar.AllDay == true)
				{
					color = Colors.HotPink;
				}
				else
				{
					var rnd = new Random();
					color = Color.FromArgb(255, (byte)rnd.Next(255), (byte)rnd.Next(255), (byte)255);
				}
				EventList.Add(new CalendarItem
				{
					Appointment = calendar,
					DetailsVisibility = Details,
					LocationVisibility = Location,
					StartTime = calendar.StartTime.ToString("HH:mm"),
					EndTime = calendar.StartTime.Add(calendar.Duration).ToString("HH:mm"),
					GlowBrush = new SolidColorBrush(color),
					GlowColor = color,
				});
			}
			EventsListView.ItemsSource = EventList;
		}

		private async void EventsTomorowListView_Loaded(object sender, RoutedEventArgs e)
		{
			AppointmentStore appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);

			var dateToShow = DateTime.Today.AddDays(1);
			var duration = TimeSpan.FromHours(24);

			ObservableCollection<CalendarItem> EventTomorowList = new ObservableCollection<CalendarItem>();
			var appCalendars = await appointmentStore.FindAppointmentsAsync(dateToShow, duration);
			foreach (var calendar in appCalendars)
			{
				Visibility Location = Visibility.Visible;
				Visibility Details = Visibility.Visible;
				if (String.IsNullOrEmpty(calendar.Location) == true)
				{
					Location = Visibility.Collapsed;
				}
				if (String.IsNullOrEmpty(calendar.Details) == true)
				{
					Details = Visibility.Collapsed;
				}

				Color color;
				if (calendar.IsCanceledMeeting == true)
				{
					color = Color.FromArgb(255, 233, 110, 96);
				}
				else if (calendar.HasInvitees == true)
				{
					color = Color.FromArgb(255, 233, 211, 120);
				}
				else if (calendar.AllDay == true)
				{
					color = Colors.HotPink;
				}
				else
				{
					var rnd = new Random();
					color = Color.FromArgb(255, (byte)rnd.Next(255), (byte)rnd.Next(255), (byte)255);
				}
				EventTomorowList.Add(new CalendarItem
				{
					Appointment = calendar,
					DetailsVisibility = Details,
					LocationVisibility = Location,
					StartTime = calendar.StartTime.ToString("HH:mm"),
					EndTime = calendar.StartTime.Add(calendar.Duration).ToString("HH:mm"),
					GlowBrush = new SolidColorBrush(color),
					GlowColor = color,
				});
			}
			EventsTomorowListView.ItemsSource = EventTomorowList;
		}

		private async void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
		{
			var date = args.AddedDates[0];
			AppointmentStore appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);
			ObservableCollection<CalendarItem> EventList = new ObservableCollection<CalendarItem>();
			var dateToShow = date.DateTime;
			if (date.DateTime.Date.ToShortDateString() == DateTime.Now.Date.ToShortDateString())
			{
				ListDate.Text = "Today";
				ListDateNext.Text = "Tomorrow";
			}
			else
			{
				ListDate.Text = date.DateTime.Date.ToShortDateString();
				ListDateNext.Text = date.DateTime.Date.AddDays(1).ToShortDateString();
			}
			var duration = TimeSpan.FromHours(24);
			var appCalendars = await appointmentStore.FindAppointmentsAsync(dateToShow, duration);
			foreach (var calendar in appCalendars)
			{
				Visibility Location = Visibility.Visible;
				Visibility Details = Visibility.Visible;
				if (String.IsNullOrEmpty(calendar.Location) == true)
				{
					Location = Visibility.Collapsed;
				}
				if (String.IsNullOrEmpty(calendar.Details) == true)
				{
					Details = Visibility.Collapsed;
				}
				Color color;
				if (calendar.IsCanceledMeeting == true)
				{
					color = Color.FromArgb(255, 233, 110, 96);
				}
				else if (calendar.HasInvitees == true)
				{
					color = Color.FromArgb(255, 233, 211, 120);
				}
				else if (calendar.AllDay == true)
				{
					color = Colors.HotPink;
				}
				else
				{
					var rnd = new Random();
					color = Color.FromArgb(255, (byte)rnd.Next(255), (byte)rnd.Next(255), (byte)255);
				}
				EventList.Add(new CalendarItem
				{
					Appointment = calendar,
					DetailsVisibility = Details,
					LocationVisibility = Location,
					StartTime = calendar.StartTime.ToString("HH:mm"),
					EndTime = calendar.StartTime.Add(calendar.Duration).ToString("HH:mm"),
					GlowBrush = new SolidColorBrush(color),
					GlowColor = color,
				});
			}
			EventsListView.ItemsSource = EventList;
			UpdateTomotor(date.DateTime);
		}
		public async void UpdateTomotor(DateTime date)
		{
			AppointmentStore appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);

			var dateToShow = date.AddDays(1);
			var duration = TimeSpan.FromHours(24);

			ObservableCollection<CalendarItem> EventTomorowList = new ObservableCollection<CalendarItem>();
			var appCalendars = await appointmentStore.FindAppointmentsAsync(dateToShow, duration);
			foreach (var calendar in appCalendars)
			{
				Visibility Location = Visibility.Visible;
				Visibility Details = Visibility.Visible;
				if (String.IsNullOrEmpty(calendar.Location) == true)
				{
					Location = Visibility.Collapsed;
				}
				if (String.IsNullOrEmpty(calendar.Details) == true)
				{
					Details = Visibility.Collapsed;
				}

				Color color;
				if (calendar.IsCanceledMeeting == true)
				{
					color = Color.FromArgb(255, 233, 110, 96);
				}
				else if (calendar.HasInvitees == true)
				{
					color = Color.FromArgb(255, 233, 211, 120);
				}
				else if (calendar.AllDay == true)
				{
					color = Colors.HotPink;
				}
				else
				{
					var rnd = new Random();
					color = Color.FromArgb(255, (byte)rnd.Next(255), (byte)rnd.Next(255), (byte)255);
				}
				EventTomorowList.Add(new CalendarItem
				{
					Appointment = calendar,
					DetailsVisibility = Details,
					LocationVisibility = Location,
					StartTime = calendar.StartTime.ToString("HH:mm"),
					EndTime = calendar.StartTime.Add(calendar.Duration).ToString("HH:mm"),
					GlowBrush = new SolidColorBrush(color),
					GlowColor = color,
				});
			}
			EventsTomorowListView.ItemsSource = EventTomorowList;
		}

		private async void CalendarBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
		{
			await new MessageDialog("This flyout is still in preview beta and this feature hasnt been implemented.").ShowAsync();
		}

		private void Calendar_Loaded(object sender, RoutedEventArgs e)
		{
			DateTime now = DateTime.Now;
			Calendar.SetDisplayDate(now);
		}

		private async void WindowsSettingsButton_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("ms-settings:dateandtime"));

		public void Dispose() => timer?.Stop();
	}
}
