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
using TerraFX.Interop.Windows;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class HomeSettingsPage : Page
	{
		public HomeSettingsPage()
		{
			this.InitializeComponent();
		}

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			var startup = await StartupTask.GetAsync("FluentFlyoutsStartupTaskId");
			UpdateToggleState(startup.State);
		}

		private void UpdateToggleState(StartupTaskState state)
		{
			StartupToggle.IsEnabled = true;
			switch (state)
			{
				case StartupTaskState.Enabled:
					StartupToggle.IsOn = true;
					break;
				case StartupTaskState.Disabled:
					break;
				case StartupTaskState.DisabledByUser:
					StartupToggle.IsOn = false;
					StartupErrorText.Text = "Unable to change state of startup task via the application - enable via Startup page in Windows Settings";
					break;
				default:
					StartupToggle.IsEnabled = false;
					break;
			}
		}

		private async void StartupToggle_Toggled(object sender, RoutedEventArgs e)
		{
			bool enable = StartupToggle.IsOn;
			var startup = await StartupTask.GetAsync("FluentFlyoutsStartupTaskId");
			switch (startup.State)
			{
				case StartupTaskState.Enabled when !enable:
					startup.Disable();
					break;
				case StartupTaskState.Disabled when enable:
					var updatedState = await startup.RequestEnableAsync();
					UpdateToggleState(updatedState);
					break;
				case StartupTaskState.DisabledByUser when enable:
					StartupToggle.IsOn = false;
					StartupErrorText.Text = "Unable to change state of startup task via the application - enable via Startup page in Windows Settings";
					break;
				default:
					break;
			}
		}
	}
}
