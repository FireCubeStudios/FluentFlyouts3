using FluentFlyouts.Core.Interfaces;
using FluentFlyouts.Flyouts.Helpers;
using FluentFlyouts.Flyouts.Interfaces;
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
	public sealed partial class TrayFlyoutWindow : WindowEx, IFlyoutWindow
	{
		private TrayIcon TrayIcon;
		private IFlyoutContent FlyoutContent;

		public MenuFlyout? ContextFlyout;
		public TrayFlyoutWindow(TrayIcon TrayIcon, IFlyoutContent FlyoutContent, MenuFlyout contextFlyout = null)
		{
			this.InitializeComponent();
			this.SetExtendedWindowStyle(ExtendedWindowStyle.Transparent);
			this.SetWindowOpacity(0);
			this.ExtendsContentIntoTitleBar = true;

			this.FlyoutContent = FlyoutContent;
			this.TrayIcon = TrayIcon;
			TrayIcon.LeftClicked += (sender, e) => ShowFlyout();

			this.Flyout.Content = FlyoutContent as UIElement;

			if (contextFlyout is not null)
			{
				this.ContextFlyout = contextFlyout;
				TrayIcon.RightClicked += (sender, e) => ShowMenuFlyout();
				ContextFlyoutContainer.Content = ContextFlyout;
				contextFlyout.SystemBackdrop = new MicaBackdrop();
				contextFlyout.Placement = FlyoutPlacementMode.Top;
				contextFlyout.ShouldConstrainToRootBounds = false;
			}
		}

		public void ShowFlyout()
		{
			this.Activate();
			var cursor = Win32.GetCursorPosition();
			this.MoveAndResize((double)cursor.X, (double)cursor.Y, 0, 0);
			FlyoutShowOptions options = new FlyoutShowOptions();
			options.Placement = FlyoutPlacementMode.TopEdgeAlignedLeft;
			options.Position = new Point(cursor.X, cursor.Y);
			Win32.SetForegroundWindow(this.GetWindowHandle());


			this.Flyout.ShowAt(this.FlyoutContainer, options);
			//FlyoutBase.ShowAttachedFlyout(Container);
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

		public void Dispose()
		{
			FlyoutContent.Dispose();
			TrayIcon.Dispose();
			this.Close();
		}
	}
}
