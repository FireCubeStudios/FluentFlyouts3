using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CubeKit.UI.Icons;
using FluentFlyouts3.Helpers;
using Windows.ApplicationModel.Core;
using Windows.Devices.Power;
using Windows.UI.Core;

namespace FluentFlyouts3.Services
{
    public partial class BatteryService
    {
        private BatteryReport Info;

        public string Percentage { get => Info.GetPercentageText(); }

        public string Status { get => Info.GetStatusLabel(); }

        private FluentSymbol icon;

        private string timeRemaining;

        private string powerusage;



        public BatteryService()
        {
            Refresh();
            Battery.AggregateBattery.ReportUpdated += AggregateBattery_ReportUpdated;
        }

        private void AggregateBattery_ReportUpdated(Battery sender, object args) => Refresh();

        public void Refresh()
        {
            Info = Battery.AggregateBattery.GetReport();    
        }
    }
}
