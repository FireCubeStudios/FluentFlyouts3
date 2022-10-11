using CubeKit.Flyouts.Helpers;
using Microsoft.UI;
using System.IO;
using System;
using WinUIEx;
using H.NotifyIcon;
using Microsoft.UI.Xaml.Input;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CubeKit.Flyouts.Interfaces;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CubeKit.Flyouts
{
    /// <summary>
    /// A flyout window
    /// </summary>
    public partial class FlyoutWindow : BaseWindow, IFlyoutWindow
    {
        public IPositionHelper FlyoutPositionHelper = new PositionHelper();
        public IIconManager FlyoutIconManager;

        public FlyoutWindow()
        {
            FlyoutPositionHelper.Positionflyout(this);
            this.InitializeComponent();
            this.SetTitleBarBackgroundColors(Colors.Transparent);
            this.BringToFront();
            this.Activated += Flyout_Activated;
        }
        private void Flyout_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == WindowActivationState.Deactivated)
                this.Hide();
        }
    }
}
