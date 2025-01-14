using FluentFlyouts.Core.Interfaces;
using FluentFlyouts.Flyouts;
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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.Devices.Flyouts
{
	public sealed partial class DeviceFlyout : UserControl, IFlyoutContent
	{
		public event EventHandler? ShowFlyoutRequested;

		private TrayIcon trayIcon;
		public DeviceFlyout(TrayIcon trayIcon)
		{
			this.InitializeComponent();
			this.trayIcon = trayIcon;
		}

		public void Dispose()
		{
		}

		private async void Grid_Loaded(object sender, RoutedEventArgs e)
		{
			ObservableCollection<DeviceInformation> USBItems = new ObservableCollection<DeviceInformation>();
			var USBDevinfo = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(Windows.Devices.Enumeration.DeviceClass.PortableStorageDevice);
			foreach (var i in USBDevinfo)
			{
				USBItems.Add(i);

			}
			USBListView.ItemsSource = USBItems;
			ObservableCollection<DeviceInformation> SoundItems = new ObservableCollection<DeviceInformation>();
			var SoundDevinfo = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(Windows.Devices.Enumeration.DeviceClass.AudioRender);
			foreach (var i in SoundDevinfo)
			{
				SoundItems.Add(i);

			}
			SoundListView.ItemsSource = SoundItems;
		}
	}
}
