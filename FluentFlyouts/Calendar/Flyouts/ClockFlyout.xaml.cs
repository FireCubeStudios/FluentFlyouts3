using CommunityToolkit.WinUI.UI.Helpers;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.Calendar.Flyouts
{
	public sealed partial class ClockFlyout : UserControl, IFlyoutContent
	{
		private DispatcherTimer? timer;

		private TrayIcon? trayIcon;
		public ClockFlyout(TrayIcon trayIcon)
		{
			this.InitializeComponent();
			this.trayIcon = trayIcon;

			StartTimer();

			var uiSettings = new UISettings();
			trayIcon?.UpdateIcon($"{(uiSettings.GetColorValue(UIColorType.Background) == Colors.Black ? "Calendar/Assets/ClockDark.ico" : "Calendar/Assets/ClockLight.ico")}");
			uiSettings.ColorValuesChanged += (sender, e) =>{
				trayIcon?.UpdateIcon($"{(uiSettings.GetColorValue(UIColorType.Background) == Colors.Black ? "Calendar/Assets/ClockDark.ico" : "Calendar/Assets/ClockLight.ico")}");
			};
		}

		public ClockFlyout()
		{
			this.InitializeComponent();
			StartTimer();
		}

		private void StartTimer()
		{
			timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(1) // Update every second
			};
			timer.Tick += (sender, e) => UpdateTime();
			timer.Start();

			UpdateTime();
		}

		private void UpdateTime()
		{
			// Get current date and time
			DateTime now = DateTime.Now;

			Calendar.SetDisplayDate(now);

			bool is24HourClock = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Contains("H");

			// Update DatesText to "4 January 2025" format
			DatesText.Text = now.ToString("d MMMM yyyy", CultureInfo.InvariantCulture);

			// Update HourMinuteText based on the system clock format
			HourMinuteText.Text = is24HourClock
				? now.ToString("HH:mm", CultureInfo.InvariantCulture) // 24-hour format
				: now.ToString("h:mm", CultureInfo.InvariantCulture); // 12-hour format

			// Update SecondsText to show seconds only
			SecondsText.Text = is24HourClock
						  ? $":{now.ToString("ss", CultureInfo.InvariantCulture)}" // Only seconds
						  : $":{now.ToString("ss tt", CultureInfo.InvariantCulture)}"; // Seconds with AM/PM

			// Update DayText to "Saturday" format (day of the week)
			DayText.Text = now.ToString("dddd", CultureInfo.InvariantCulture);

			trayIcon?.UpdateTooltip($"{now.ToString("dddd, MMMM d yyyy")}\n{now.ToString((is24HourClock ? "HH:mm" : "h:mm tt"))}");
		}

		private async void WindowsSettingsButton_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("ms-settings:dateandtime"));

		public void Dispose() => timer?.Stop();
	}
}
