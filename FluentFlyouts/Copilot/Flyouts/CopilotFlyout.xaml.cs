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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Web.UI.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.Copilot.Flyouts
{
	public class Consts
	{
		public const string CopilotMode = "CopilotMode";
		public const string WindowsCopilot = "WindowsCopilot";
		public const string BingCopilot = "BingCopilot";
	}

	public sealed partial class CopilotFlyout : UserControl, IFlyoutContent
	{
		private readonly Dictionary<string, string> Copilots = new Dictionary<string, string>
		{
			{ Consts.WindowsCopilot, "https://edgeservices.bing.com/edgesvc/chat?&darkschemeovr=1&FORM=SHORUN&udscs=1&udsnav=1&setlang=en-US&udsedgeshop=1&clientscopes=noheader,coauthor,chat,visibilitypm,udsedgeshop,wincopilot,docvisibility,channelstable,udsinwin11,&copilotsupported=1,&browserversion=119.0.2151.72,&udsframed=1" },
			{ Consts.BingCopilot, "https://edgeservices.bing.com/edgesvc/chat?&darkschemeovr=1&FORM=SHORUN&udscs=1&udsnav=1&setlang=en-US&udsedgeshop=1&clientscopes=noheader,coauthor,chat,visibilitypm,udsedgeshop,docvisibility,channelstable,udsinwin11,&copilotsupported=1,&browserversion=119.0.2151.72,&udsframed=1" },
		};

		private TrayIcon trayIcon;

		public CopilotFlyout(TrayIcon trayIcon)
		{
			this.InitializeComponent();
			this.trayIcon = trayIcon;
			Initialize();
		}

		private async void Initialize()
		{
			await WebViewControl.EnsureCoreWebView2Async();

			WebViewControl.CoreWebView2.Settings.IsStatusBarEnabled = false;
			WebViewControl.CoreWebView2.Settings.IsSwipeNavigationEnabled = false;

			LoadPage();
		}

		private void UseWindowsCopilot(object sender, RoutedEventArgs e)
		{
			//SettingsHelper.SetValue(Consts.CopilotMode, Consts.WindowsCopilot);
			LoadPage();
		}

		private void UseBingCopilot(object sender, RoutedEventArgs e)
		{
			//SettingsHelper.SetValue(Consts.CopilotMode, Consts.BingCopilot);
			LoadPage();
		}

		private void Refresh(object sender, RoutedEventArgs e)
		{
			LoadPage();
		}

		private void LoadPage()
		{
			var copilotMode = "BingCopilot"; // (string)SettingsHelper.GetValue(Consts.CopilotMode);
			var url = Copilots[copilotMode];
			WebViewControl.Source = new Uri(url);
		}

		public void Dispose() => WebViewControl.Close();
	}
}
