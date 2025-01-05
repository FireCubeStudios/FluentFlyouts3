using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Notifications.Management;
using Windows.UI.Notifications;
using FluentFlyouts.Core.Interfaces;
using FluentFlyouts.Flyouts;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.Notifications.Flyouts
{
	public sealed partial class NotificationFlyout : UserControl, IFlyoutContent
	{
		private UserNotificationListener _listener;
		private TrayIcon trayIcon;
		public NotificationFlyout(TrayIcon trayIcon)
		{
			this.InitializeComponent();
			this.trayIcon = trayIcon;
		}
		public ObservableCollection<NotificationItems> NotificationItemsList;
		public class NotificationItems
		{
			public string Time { get; set; }
			public string App { get; set; }
			public string Text { get; set; }
			public string Header { get; set; }
			public UserNotification Notification { get; set; }
			public BitmapImage Icon { get; set; }
		}
		private async void Notifications_Loaded(object sender, RoutedEventArgs e)
		{
			_listener = UserNotificationListener.Current;
			NotificationItemsList = new ObservableCollection<NotificationItems>();
			var notifications = await _listener.GetNotificationsAsync(NotificationKinds.Toast);
			foreach (var i in notifications)
			{
				try
				{
					NotificationItems Notif = new NotificationItems();
					Notif.App = i.AppInfo.DisplayInfo.DisplayName.ToString();
					Notif.Notification = i;
					/*  new ToastContentBuilder()
         .AddText("Adaptive Tiles Meeting", hintMaxLines: 1)
         .AddText("Conf Room 2001 / Building 135")

         .AddText("10:00 AM - 10:30 AM");*/

					foreach (var ii in i.Notification.Visual.Bindings)
					{
						NotificationBinding toastBinding = i.Notification.Visual.GetBinding(KnownNotificationBindings.ToastGeneric);
						IReadOnlyList<AdaptiveNotificationText> textElements = ii.GetTextElements();
						Notif.Header = textElements.FirstOrDefault()?.Text;
						Notif.Text = string.Join("\n", textElements.Skip(1).Select(t => t.Text));
					}
					RandomAccessStreamReference streamReference = i.AppInfo.DisplayInfo.GetLogo(new Size(30, 30));
					// In an actual app, we would probably load these images before the notification is displayed
					// so that the images don't pop in
					BitmapImage appLogo = new BitmapImage();
					await appLogo.SetSourceAsync(await streamReference.OpenReadAsync());
					Notif.Icon = appLogo;
					if (i.CreationTime.Date == DateTime.Today)
					{
						Notif.Time = "Today at " + i.CreationTime.DateTime.ToString("h:mm tt");
					}
					else if (i.CreationTime.DateTime == DateTime.Today.AddDays(-1))
					{
						Notif.Time = "Yesterday at " + i.CreationTime.DateTime.ToString("h:mm tt");
					}
					else
					{
						Notif.Time = i.CreationTime.DateTime.ToString();
					}
					NotificationItemsList.Add(Notif);
				}
				catch
				{

				}
			}
			NotificationItemsList.Reverse().ToList();
			Notifications.ItemsSource = NotificationItemsList;
		}
		private void ButtonClearAll_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_listener = UserNotificationListener.Current;
				foreach (var item in NotificationItemsList)
				{
					_listener.RemoveNotification(item.Notification.Id);
				}
			}

			catch (Exception ex)
			{
			}

			UpdateNotifications();
		}

		private async void UpdateNotifications()
		{
			_listener = UserNotificationListener.Current;
			NotificationItemsList = new ObservableCollection<NotificationItems>();
			var notifications = await _listener.GetNotificationsAsync(NotificationKinds.Toast);
			foreach (var i in notifications)
			{
				try
				{
					NotificationItems Notif = new NotificationItems();
					Notif.App = i.AppInfo.DisplayInfo.DisplayName.ToString();
					Notif.Notification = i;
					/*  new ToastContentBuilder()
         .AddText("Adaptive Tiles Meeting", hintMaxLines: 1)
         .AddText("Conf Room 2001 / Building 135")

         .AddText("10:00 AM - 10:30 AM");*/

					foreach (var ii in i.Notification.Visual.Bindings)
					{
						NotificationBinding toastBinding = i.Notification.Visual.GetBinding(KnownNotificationBindings.ToastGeneric);
						IReadOnlyList<AdaptiveNotificationText> textElements = ii.GetTextElements();
						Notif.Header = textElements.FirstOrDefault()?.Text;
						Notif.Text = string.Join("\n", textElements.Skip(1).Select(t => t.Text));
					}
					RandomAccessStreamReference streamReference = i.AppInfo.DisplayInfo.GetLogo(new Size(30, 30));
					// In an actual app, we would probably load these images before the notification is displayed
					// so that the images don't pop in
					BitmapImage appLogo = new BitmapImage();
					await appLogo.SetSourceAsync(await streamReference.OpenReadAsync());
					Notif.Icon = appLogo;
					if (i.CreationTime.Date == DateTime.Today)
					{
						Notif.Time = "Today at " + i.CreationTime.DateTime.ToString("h:mm tt");
					}
					else if (i.CreationTime.DateTime == DateTime.Today.AddDays(-1))
					{
						Notif.Time = "Yesterday at " + i.CreationTime.DateTime.ToString("h:mm tt");
					}
					else
					{
						Notif.Time = i.CreationTime.DateTime.ToString();
					}
					NotificationItemsList.Add(Notif);
				}
				catch
				{

				}
			}
			NotificationItemsList.Reverse().ToList();
			Notifications.ItemsSource = NotificationItemsList;
			/* try
             {
                 IReadOnlyList<UserNotification> notifsInPlatform = await _listener.GetNotificationsAsync(NotificationKinds.Toast);
                 // Reverse their order since the platform returns them with oldest first, we want newest first
                 notifsInPlatform = notifsInPlatform.Reverse().ToList();

                 // First remove any notifications that no longer exist
                 for (int i = 0; i < this.Notifications.Count; i++)
                 {
                     UserNotification existingNotif = this.Notifications[i];

                     // If not in platform anymore, remove from our list
                     if (!notifsInPlatform.Any(n => n.Id == existingNotif.Id))
                     {
                         this.Notifications.RemoveAt(i);
                         i--;
                     }
                 }

                 // Now our list only contains notifications that exist,
                 // but it might be missing new notifications.

                 for (int i = 0; i < notifsInPlatform.Count; i++)
                 {
                     UserNotification platNotif = notifsInPlatform[i];

                     int indexOfExisting = FindIndexOfNotification(platNotif.Id);

                     // If we have an existing
                     if (indexOfExisting != -1)
                     {
                         // And if it's in the wrong position
                         if (i != indexOfExisting)
                         {
                             // Move it to the right position
                             this.Notifications.Move(indexOfExisting, i);
                         }

                         // Otherwise, leave it in its place
                     }

                     // Otherwise, notification is new
                     else
                     {
                         // Insert at that position
                         this.Notifications.Insert(i, platNotif);
                     }
                 }
             }

             catch (Exception ex)
             {
               //  Error = "Error updating notifications: " + ex.ToString();
             }*/
		}

		public void Dispose()
		{

		}
	}
}
