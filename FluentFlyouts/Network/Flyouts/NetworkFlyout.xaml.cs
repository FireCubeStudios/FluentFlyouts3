using FluentFlyouts.Core.Interfaces;
using FluentFlyouts.Flyouts;
using FluentFlyouts.Network.Helpers;
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
using Windows.Devices.WiFi;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.Network.Flyouts
{
	public sealed partial class NetworkFlyout : UserControl, IFlyoutContent
	{
		public event EventHandler? ShowFlyoutRequested;

		private TrayIcon trayIcon;
		private NetworkPresenter networkPresenter = new NetworkPresenter();
		private bool ConnectAutomatically = true;
		private string CurrentPassword = string.Empty;

		public NetworkFlyout(TrayIcon trayIcon)
		{
			this.InitializeComponent();
			this.trayIcon = trayIcon;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		private async void Grid_Loaded(object sender, RoutedEventArgs e)
		{
			var WifiList = await networkPresenter.GetAvailableNetworks();
			ObservableCollection<WiFiAvailableNetwork> WifiItems = new ObservableCollection<WiFiAvailableNetwork>();
			var CurrentWifi = networkPresenter.GetCurrentWifiNetwork();
			foreach (var i in WifiList)
			{
				WifiItems.Add(i);
			}
			WifiListView.ItemsSource = WifiItems;
			CurrentWifiID.Text = CurrentWifi.Ssid;
		}
	}
}
