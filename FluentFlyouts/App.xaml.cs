using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CommunityToolkit.WinUI.UI.Helpers;
using FluentFlyouts.Battery.Flyouts;
using FluentFlyouts.Calendar.Flyouts;
using FluentFlyouts.Flyouts;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
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
			MenuFlyout contextFlyout = new MenuFlyout();
			contextFlyout.Items.Add(new MenuFlyoutItem()
			{
				Icon = new FluentIcons.WinUI.SymbolIcon() { Symbol = FluentIcons.Common.Symbol.PersonFeedback },
				Text = "Send feedback on GitHub"
			});
			(contextFlyout.Items[0] as MenuFlyoutItem)!.Click += async (sender, e) => await Launcher.LaunchUriAsync(new Uri("https://github.com/FireCubeStudios/FluentFlyouts"));
			contextFlyout.Items.Add(new MenuFlyoutItem()
			{
				Icon = new FluentIcons.WinUI.SymbolIcon() { Symbol = FluentIcons.Common.Symbol.ContactCard },
				Text = "Contact Developer on Discord"
			});
			(contextFlyout.Items[1] as MenuFlyoutItem)!.Click += async (sender, e) => await Launcher.LaunchUriAsync(new Uri("https://discord.gg/3WYcKat"));
			contextFlyout.Items.Add(new MenuFlyoutSeparator());
			contextFlyout.Items.Add(new MenuFlyoutItem()
            {
                Icon = new FluentIcons.WinUI.SymbolIcon() { Symbol = FluentIcons.Common.Symbol.Settings },
                Text = "Settings"
            });
			(contextFlyout.Items[3] as MenuFlyoutItem)!.Click += (sender, e) =>
			{
				m_window ??= new MainWindow();
				m_window.Activate();
			};

			// var tray = new TrayIcon(1, "", "Battery");
			// f_window = new TrayFlyoutWindow(tray, new BatteryFlyout(tray));

			var tray2 = new TrayIcon(2, "Calendar/Assets/ClockDark.ico", "");
			c_window = new TrayFlyoutWindow(tray2, new ClockFlyout(tray2), contextFlyout);

			/*var tray3 = new TrayIcon(3, "", "Volume");
			v_window = new TrayFlyoutWindow(tray3, new VolumeFlyout(tray3));*/
		}

		private Window? m_window;
		public static Window? f_window;
		private Window? c_window;
		private Window? v_window;
	}
}
