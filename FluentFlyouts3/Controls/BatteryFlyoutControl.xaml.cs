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
using FluentFlyouts3.Services;
using Microsoft.Extensions.DependencyInjection;
using Windows.Devices.Power;
using Windows.UI.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using Windows.ApplicationModel.Core;
using FluentFlyouts3.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts3.Controls
{
    public partial class BatteryFlyoutControl : UserControl
    {
        private SettingsService Settings = App.Current.Services.GetService<SettingsService>();
        private PowerService Power = App.Current.Services.GetService<PowerService>();
        private BatteryReport Info = Battery.AggregateBattery.GetReport();

        private DispatcherTimer Timer;

        public BatteryFlyoutControl()
        {
            this.InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Tick += Refresh_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        // Cursed but regular binding was crashing eventhough it works in xaml islands, wpf versions
        private void Refresh_Tick(object sender, object e)
        {
            Info = Battery.AggregateBattery.GetReport();
            Percentage.Text = Info.GetPercentageText();
            PowerUsageText.Text = Info.GetPowerUsageText();
            RemainingTime.Text = Info.GetRemaningTimeText();
            Icon.Symbol = Info.GetPercentageIcon();
            if (Settings.IsHealthEnabled)
                BatteryHealthText.Text = Info.GetBatteryHealth();
            if (Settings.IsAdditionalInformationEnabled)
            {
                PowerRate.Text = Info.GetPowerRate();
                RemaniningCapacity.Text = Info.GetRemainingCapacity();
                Status.Text = Info.GetStatusLabel();
            }
            Timer.Interval = new TimeSpan(0, 0, 30);
        }
    }
}
