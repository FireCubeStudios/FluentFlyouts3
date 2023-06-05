using CubeKit.Flyouts;
using CubeKit.Flyouts.Interfaces;
using FluentFlyouts3.Helpers;
using FluentFlyouts3.Icons;
using FluentFlyouts3.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts3.Flyouts
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BatteryFlyout : FlyoutWindow
    {
        private SettingsService Settings = App.Current.Services.GetService<SettingsService>();
        new IPositionHelper FlyoutPositionHelper = new BatteryPositionHelper();
        new IIconManager FlyoutIconManager = new BatteryIconManager();
        public BatteryFlyout()
        {
            this.InitializeComponent();
            FlyoutPositionHelper.Positionflyout(this);
        }

        private void Flyout_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var appX = this.AppWindow.Position.X;
            var appY = this.AppWindow.Position.Y;

            var height = this.Height;
            var width = this.Width;

            Settings.XB = (int)(appX + e.Position.X);
            Settings.YB = (int)(appY + e.Position.Y);

            FlyoutPositionHelper.Positionflyout(this);
        }
    }
}
