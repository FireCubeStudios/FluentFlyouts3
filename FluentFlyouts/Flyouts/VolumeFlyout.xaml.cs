using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.Flyouts
{
	public sealed partial class VolumeFlyout : UserControl
	{
		private TrayIcon trayIcon;
		public VolumeFlyout(TrayIcon trayIcon)
		{
			this.InitializeComponent();
			this.trayIcon = trayIcon;
		}

		private string VolumeToString(double value) => $"{((int)value)}";

		private async void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
		{

		}

		private void MuteButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
