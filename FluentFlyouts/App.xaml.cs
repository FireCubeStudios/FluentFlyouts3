using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using CommunityToolkit.WinUI.Helpers;
using FluentFlyouts.Battery.Flyouts;
using FluentFlyouts.Calendar.Flyouts;
using FluentFlyouts.Copilot.Flyouts;
using FluentFlyouts.Devices.Flyouts;
using FluentFlyouts.Flyouts;
using FluentFlyouts.Network.Flyouts;
using FluentFlyouts.Notifications.Flyouts;
using FluentFlyouts.Screen.Flyouts;
using FluentFlyouts.Screen.Services;
using FluentFlyouts.Screen.ViewModels;
using FluentFlyouts.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
		private const string MutexID = "FluentFlyoutsMutex";
		private static Mutex? SingleInstanceMutex;

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
        {
			Services = ConfigureServices();
			this.InitializeComponent();
			CheckSingleInstance();
		}

		private void CheckSingleInstance()
		{
			bool isNewInstance;
			SingleInstanceMutex = new Mutex(true, MutexID, out isNewInstance);
			if (!isNewInstance)
				System.Environment.Exit(0);
		}

		/// <summary>
		/// Gets the current <see cref="App"/> instance in use
		/// </summary>
		public new static App Current => (App)Application.Current;

		/// <summary>
		/// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
		/// </summary>
		public IServiceProvider Services { get; }

		/// <summary>
		/// Setup the Depdency Injection <see cref="IServiceProvider"/> Services.
		/// </summary>
		private static IServiceProvider ConfigureServices()
		{
			var services = new ServiceCollection();

			//Add Services here
			services.AddSingleton<ScreenService>();

			// Add ViewModels here
			services.AddSingleton<ScreenViewModel>();

			return services.BuildServiceProvider();
		}

		/// <summary>
		/// Invoked when the application is launched.
		/// </summary>
		/// <param name="args">Details about the launch request and process.</param>
		protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
		{
			// App was not launched automatically from a StartupTask
			if (AppInstance.GetActivatedEventArgs().Kind != ActivationKind.StartupTask)
			{
				if (Settings.IsFirstRun)
					OpenSettings();

				Settings.IsFirstRun = true;
			}

			if (Settings.IsClockFlyoutEnabled)
			{
				App.flyoutService.AddFlyout(2, tray => new ClockFlyout(tray), true);
			}
			if (Settings.IsCalendarFlyoutEnabled)
			{
				App.flyoutService.AddFlyout(3, tray => new CalendarFlyout(tray), true);
			}
			if (Settings.IsBrightnessFlyoutEnabled)
			{
				App.flyoutService.AddFlyout(4, tray => new BrightnessFlyout(tray));
			}

			//App.flyoutService.AddFlyout(1, tray => new BatteryFlyout(tray));
			App.flyoutService.AddFlyout(4, tray => new BrightnessFlyout(tray));
		}

        public static void OpenSettings()
        {
			App.m_window ??= new MainWindow();
			App.m_window.Activate();
			App.m_window.Closed += (sender, e) => App.m_window = new MainWindow();
		}

		public static void Close()
		{
			flyoutService.ClearAllFlyouts();
			m_window?.Close();
			CoreApplication.Exit();
			Environment.Exit(0);
		}

		public static SettingsService Settings = new();
        public static TrayFlyoutService flyoutService = new();
		public static Window? m_window;
	}
}
