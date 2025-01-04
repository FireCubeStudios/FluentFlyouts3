using FluentFlyouts.Flyouts.Helpers;
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
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.Flyouts
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class TrayFlyoutWindow : WindowEx
	{
		private TrayIcon TrayIcon;

		public MenuFlyout? ContextFlyout;
		public TrayFlyoutWindow(TrayIcon trayIcon, UIElement flyoutContent, MenuFlyout contextFlyout = null)
		{
			this.InitializeComponent();
			this.SetExtendedWindowStyle(ExtendedWindowStyle.Transparent);
			this.SetWindowOpacity(0);
			this.ExtendsContentIntoTitleBar = true;

			this.TrayIcon = trayIcon;
			TrayIcon.LeftClicked += (sender, e) => ShowFlyout();

			this.Flyout.Content = flyoutContent;

			if (contextFlyout is not null)
			{
				this.ContextFlyout = contextFlyout;
				TrayIcon.RightClicked += (sender, e) => ShowMenuFlyout();
				ContextFlyoutContainer.Content = ContextFlyout;
				contextFlyout.SystemBackdrop = new MicaBackdrop();
				contextFlyout.Placement = FlyoutPlacementMode.Top;
				contextFlyout.ShouldConstrainToRootBounds = false;
			}

		//	this.Content.LostFocus += (sender, e) => this.Hide();
		}

		public void ShowFlyout()
		{
			this.Activate();
			this.MoveAndResize((double)Win32.GetCursorPosition().X, (double)Win32.GetCursorPosition().Y, 0, 0);
			FlyoutShowOptions options = new FlyoutShowOptions();
			options.Position = new Point(0, 0);
			this.Flyout.ShowAt(this.FlyoutContainer, options);
			//FlyoutBase.ShowAttachedFlyout(Container);
			Win32.SetForegroundWindow(this.GetWindowHandle());
		}

		public void ShowMenuFlyout()
		{
			if (ContextFlyout is not null)
			{
				this.Activate();
				this.MoveAndResize((double)Win32.GetCursorPosition().X, (double)Win32.GetCursorPosition().Y, 0, 0);
				FlyoutShowOptions options = new FlyoutShowOptions();
				options.Position = new Point(0, 0);
				this.ContextFlyout.ShowAt(this.ContextFlyoutContainer, options);
				//FlyoutBase.ShowAttachedFlyout(Container);
				Win32.SetForegroundWindow(this.GetWindowHandle());
			}
		}

		private void Flyout_Closed(object sender, object e) => this.Hide();
	}
}
