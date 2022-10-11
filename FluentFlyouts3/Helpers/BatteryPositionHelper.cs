using CommunityToolkit.WinUI.Helpers;
using CubeKit.Flyouts;
using CubeKit.Flyouts.Interfaces;
using FluentFlyouts3.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIEx;

namespace FluentFlyouts3.Helpers
{
    public class BatteryPositionHelper : IPositionHelper
    {
        private SettingsService Settings = App.Current.Services.GetService<SettingsService>();

        public void Positionflyout(BaseWindow Flyout)
        {
            var DisplayBounds = DisplayArea.Primary.OuterBounds;
            int DisplayHeight = DisplayBounds.Height;
            int DisplayWidth = DisplayBounds.Width;

            double W = Flyout.Width;
            double H = Flyout.Height;
            if ((SystemInformation.Instance.IsFirstRun || SystemInformation.Instance.IsAppUpdated) && (Settings.XB == 100 || Settings.YB == 100))
            {
                Settings.XB = (int)((DisplayWidth / 1.17) - (W / 2));
                Settings.YB = (int)((DisplayHeight / 1.17) - (H / 2));
            }

            Flyout.MoveAndResize(Settings.XB, Settings.YB, W, H);
        }
    }
}
