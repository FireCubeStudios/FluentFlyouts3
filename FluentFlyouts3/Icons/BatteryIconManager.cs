using CubeKit.Flyouts;
using CubeKit.Flyouts.Interfaces;
using H.NotifyIcon;
using H.NotifyIcon.Core;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Power;
using FluentFlyouts3.Helpers;
using Windows.System.Power;
using FluentFlyouts3.Services;
using Microsoft.Extensions.DependencyInjection;
using FluentFlyouts3.Classes;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using CommunityToolkit.WinUI.UI.Helpers;

namespace FluentFlyouts3.Icons
{
    public class BatteryIconManager : IIconManager
    {
        public TaskbarIcon FlyoutIcon { get; set; }
        public BaseWindow FlyoutWindow { get; set; }

        private PowerService Power = App.Current.Services.GetService<PowerService>();
        private DispatcherTimer Timer;
        private ThemeListener Theme;

        public bool Initialize()
        {
            Timer = new DispatcherTimer();
            Timer.Tick += Refresh_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
            Theme = new ThemeListener();
            Theme.ThemeChanged += Listener_ThemeChanged;
            Debug.WriteLine(Theme.CurrentThemeName);
            return true;
        }

        private void Update()
        {
            BatteryReport Info = Battery.AggregateBattery.GetReport();
            FlyoutIcon.ToolTipText = $"{Info.GetPercentageText()} - {Info.GetRemaningTimeText()}";
            string Status = "";
            string value = Info.GetAbsolutePercentage().ToString();
            if (Info.Status == BatteryStatus.Charging)
                Status = "Charging";
            else if (Power.CurrentPowerPlan == PowerMode.PowerSaver)
                Status = "PowerSaver";
            Timer.Interval = new TimeSpan(0, 0, 30);
            Uri Icon = new Uri($"ms-appx:///Assets/BatteryIcons/Battery{Status}{Theme.CurrentThemeName}{value}.ico", UriKind.Absolute);
            BitmapImage bitmap = new BitmapImage(Icon);
            FlyoutIcon.ForceCreate();
            FlyoutIcon.IconSource = bitmap;
        }

        private void Refresh_Tick(object sender, object e) => Update();

        private void Listener_ThemeChanged(ThemeListener sender) => Update();

        public void Dispose()
        {
            FlyoutIcon?.Dispose();
        }
    }
}
