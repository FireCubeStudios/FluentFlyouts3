using FluentFlyouts.Core.Interfaces;
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
using TerraFX.Interop.Windows;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using WinUIEx;
using System.Runtime.InteropServices;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Interop.Windows.GWLP;
using static FluentFlyouts.Flyouts.Win32.Win32;
using FluentFlyouts.Flyouts.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.Flyouts
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class TrayFlyoutWindow : WindowEx, IFlyoutWindow
	{
		// Right click flyout menu
		public MenuFlyout? ContextFlyout;

		// If true then the flyout uses PlacementMode TopEdgeAlignedLeft otherwise it uses Top
		public bool IsAlignedLeft = false;

		private TrayIcon TrayIcon;
		private IFlyoutContent FlyoutContent;
		public TrayFlyoutWindow(TrayIcon TrayIcon, IFlyoutContent FlyoutContent, MenuFlyout contextFlyout = null, bool IsAlignedLeft = false)
		{
			this.InitializeComponent();
			this.SetExtendedWindowStyle(ExtendedWindowStyle.Transparent);
			this.SetWindowOpacity(0);
			this.ExtendsContentIntoTitleBar = true;

			this.IsAlignedLeft = IsAlignedLeft;
			this.FlyoutContent = FlyoutContent;
			this.TrayIcon = TrayIcon;
			TrayIcon.LeftClicked += (sender, e) => ShowFlyout();

			this.Flyout.Content = FlyoutContent as UIElement;
			FlyoutContent.ShowFlyoutRequested += (sender, e) => ShowFlyout();

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
			DispatcherQueue.TryEnqueue(() =>
			{
				if (this.Flyout.IsOpen || this.Visible) return;
				this.Activate();
				var cursor = GetCursorPosition();
				this.MoveAndResize((double)cursor.X, (double)cursor.Y, 0, 0);
				FlyoutShowOptions options = new FlyoutShowOptions();
				options.Placement = IsAlignedLeft ? FlyoutPlacementMode.TopEdgeAlignedLeft : FlyoutPlacementMode.Top;
				options.Position = new Point(cursor.X, cursor.Y);
				SetForegroundWindow(this.GetWindowHandle());
				this.Flyout.ShowAt(this.FlyoutContainer, options);
				//FlyoutBase.ShowAttachedFlyout(Container);
			});
		}

		public void ShowMenuFlyout()
		{
			if (ContextFlyout is not null)
			{
				this.Activate();
				this.MoveAndResize((double)GetCursorPosition().X, (double)GetCursorPosition().Y, 0, 0);
				FlyoutShowOptions options = new FlyoutShowOptions();
				options.Position = new Point(0, 0);
				this.ContextFlyout.ShowAt(this.ContextFlyoutContainer, options);
				//FlyoutBase.ShowAttachedFlyout(Container);
				SetForegroundWindow(this.GetWindowHandle());
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
