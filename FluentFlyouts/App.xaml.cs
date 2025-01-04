using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentFlyouts.Calendar.Flyouts;
using FluentFlyouts.Flyouts;
using FluentFlyouts.Services;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
		{
			App.m_window ??= new MainWindow();
			App.m_window.Activate();

			var tray = new TrayIcon(2, "", "");
			App.flyoutService.AddFlyout(2, tray, new ClockFlyout(tray));
		}

        public static TrayFlyoutService flyoutService = new();
		public static Window? m_window;
	}
}
