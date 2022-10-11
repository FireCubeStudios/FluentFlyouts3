using CubeKit.Flyouts.Interfaces;
using Microsoft.UI.Windowing;
using System;
using System.Diagnostics;
using Windows.Graphics;
using WinUIEx;

namespace CubeKit.Flyouts.Helpers
{
    public class PositionHelper : IPositionHelper
    {
        public void Positionflyout(BaseWindow Flyout)
        {
            double Scale = GetScale();
            double DisplayHeight = DisplayArea.Primary.OuterBounds.Height;
            double DisplayWidth = DisplayArea.Primary.OuterBounds.Width;

            double W = Flyout.Width;
            double H = Flyout.Height;

            Flyout.MoveAndResize((DisplayWidth / 1.17 ) - (W / 2), (DisplayHeight / 1.17) - (H / 2), W, H);
        //    Flyout.MoveAndResize(DisplayWidth - W , DisplayHeight - H , Flyout.Width, Flyout.Height);
        }

        private double GetScale()
        {
            var progmanWindow = NativeHelper.FindWindow("Shell_TrayWnd", null);
            var monitor = NativeHelper.MonitorFromWindow(progmanWindow, NativeHelper.MONITOR_DEFAULTTOPRIMARY);

            NativeHelper.DeviceScaleFactor scale;
            NativeHelper.GetScaleFactorForMonitor(monitor, out scale);

            if (scale == NativeHelper.DeviceScaleFactor.DEVICE_SCALE_FACTOR_INVALID)
                scale = NativeHelper.DeviceScaleFactor.SCALE_100_PERCENT;

            return Convert.ToDouble(scale) / 100;
        }
    }
}
