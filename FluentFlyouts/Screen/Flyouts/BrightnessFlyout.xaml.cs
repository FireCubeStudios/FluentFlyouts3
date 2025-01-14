using CommunityToolkit.Mvvm.ComponentModel;
using FluentFlyouts.Core.Interfaces;
using FluentFlyouts.Flyouts;
using FluentFlyouts.Flyouts.Helpers;
using FluentFlyouts.Screen.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TerraFX.Interop.Windows;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.Screen.Flyouts
{
	public sealed partial class BrightnessFlyout : UserControl, IFlyoutContent
	{
		public event EventHandler? ShowFlyoutRequested;

		private ScreenViewModel ViewModel = App.Current.Services.GetService<ScreenViewModel>()!;
		private const string Location = "Screen/Assets/";
		private TrayIcon? trayIcon;
		public BrightnessFlyout(TrayIcon trayIcon)
		{
			this.InitializeComponent();
			this.trayIcon = trayIcon;

			UpdateIcon();
			TrayIcon.SystemThemeChanged += (sender, IsDark) => {
				UpdateIcon();
			};

			ViewModel.ScreenService.BrightnessChanged += async (sender, e) => {
			/*	if (App.Settings.ShowBrightnessFlyoutWhenChanged)
					ShowFlyoutRequested?.Invoke(this, EventArgs.Empty);*/
				await Task.Run(() => UpdateIcon(e));
			};

			ViewModel.UIThread = action => DispatcherQueue.TryEnqueue(() => action());
		}

		public BrightnessFlyout()
		{
			this.InitializeComponent();
			ViewModel.UIThread = action => DispatcherQueue.TryEnqueue(() => action());
		}

		private void UpdateIcon(double brightness = -1)
		{
			try
			{
				if (brightness == -1)
					brightness = ViewModel.ScreenService.GetBrightness();
				var theme = ThemeHelper.IsSystemThemeDark() ? "Dark" : "Light";
				var level = brightness < 50 ? "Low" : "";
				trayIcon?.UpdateIcon($"{Location}Brightness{level}{theme}.ico");
			}
			catch { }
		}

		private FluentIcons.Common.Symbol BrightnessToIcon(double value) => value < 50 ? FluentIcons.Common.Symbol.BrightnessLow : FluentIcons.Common.Symbol.BrightnessHigh;

		private string BrightnessToString(double value) => $"{((int)value)}%";

		public void Dispose()
		{
			// fixes singleton bug by commenting out
			// ViewModel.ScreenService.Dispose();
		}

		private async void SettingsButton_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("ms-settings:display"));
	}
}
