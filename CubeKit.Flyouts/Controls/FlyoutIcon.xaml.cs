using CubeKit.Flyouts.Interfaces;
using H.NotifyIcon;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
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

namespace CubeKit.Flyouts.Controls
{
    public sealed partial class FlyoutIcon : UserControl
    {
        public BaseWindow Window
        {
            get { return (BaseWindow)GetValue(WindowProperty); }
            set
            {
                SetValue(WindowProperty, value);
            }
        }
        public static readonly DependencyProperty WindowProperty =
                   DependencyProperty.Register("Window", typeof(BaseWindow), typeof(FlyoutIcon), null);

        public IIconManager IconManager
        {
            get { return (IIconManager)GetValue(IconManagerProperty); }
            set
            {
                SetValue(IconManagerProperty, value);
                IconManager.FlyoutIcon = (TaskbarIcon)Resources["TrayIcon"]; // setup taskbar icon
                IconManager.Initialize();
            }
        }
        public static readonly DependencyProperty IconManagerProperty =
                   DependencyProperty.Register("IconManager", typeof(IIconManager), typeof(FlyoutIcon), null);

        public FlyoutIcon()
        {
            this.InitializeComponent();
            ((XamlUICommand)Resources["ToggleWindowCommand"]).ExecuteRequested += ToggleWindow;

            ((XamlUICommand)Resources["ExitCommand"]).ExecuteRequested += Exit;
        }

        private void ToggleWindow(object? _, ExecuteRequestedEventArgs args)
        {
            if (Window.Visible)
                Window.Hide();
            else
            {
                Window.Show();
                Window.BringToFront();
            }
        }

        private void Exit(object? _, ExecuteRequestedEventArgs args)
        {
            IconManager.Dispose();
            Window?.Close();
        }
    }
}
