using CubeKit.UI.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Power;
using Windows.System.Power;

namespace FluentFlyouts3.Helpers
{
    // Code from Fluent Flyouts xaml islands
    public static partial class BatteryExtensions
    {
        /// <summary>
        /// Converts BatterReport info into a percentage value of battery remaining.
        /// </summary>
        /// <param name="report">A BatteryReport object.</param>
        /// <returns>Returns a percentage value of the battery as a string with %.</returns>
        public static string GetPercentageText(this BatteryReport report) => GetPercentage(report).ToString("F0") + "%";

        /// <summary>
        /// Gets a generic power status label.
        /// </summary>
        /// <param name="report">A BatteryReport object.</param>
        /// <returns>Returns a string regarding charge or discharge status.</returns>
        public static string GetStatusLabel(this BatteryReport report) => report.Status == BatteryStatus.Charging ? "Charge Rate:" : "Discharge Rate:";

        /// <summary>
        /// Gets remaining capacity in Wh.
        /// </summary>
        /// <param name="report">A BatteryReport object.</param>
        /// <returns>Returns a string with remaining capacity in Wh</returns>
        public static string GetRemainingCapacity(this BatteryReport report) => mWhToWh((double)report.RemainingCapacityInMilliwattHours) + " Wh";

        /// <summary>
        /// Gets discharge/charge rate in Wh.
        /// </summary>
        /// <param name="report">A BatteryReport object.</param>
        /// <returns>Returns a string with discharge/charge in Wh</returns>
        public static string GetPowerRate(this BatteryReport report) => mWhToWh((double)report.ChargeRateInMilliwatts) + " Wh";

        /// <summary>
        /// Gets battery health information
        /// </summary>
        /// <param name="report">A BatteryReport object.</param>
        /// <returns>Returns a string with battery health information/returns>
        public static string GetBatteryHealth(this BatteryReport report)
        {
            string DesignCapacity = mWhToWh((double)report.DesignCapacityInMilliwattHours).ToString();
            string FullCapacity = mWhToWh((double)report.FullChargeCapacityInMilliwattHours).ToString();
            string Percent = (Math.Round(mWhToWh((double)report.FullChargeCapacityInMilliwattHours) / mWhToWh((double)report.DesignCapacityInMilliwattHours) * 100)).ToString();
            return $"{Percent} % ({FullCapacity} Wh / {DesignCapacity} Wh)";
        }

        /// <summary>
        /// Gets an icon that represents current battery level.
        /// </summary>
        /// <param name="report">A BatteryReport object.</param>
        /// <returns>Returns a FluentSymbol representing battery level./returns>
        public static FluentSymbol GetPercentageIcon(this BatteryReport report)
        {
            return ((int)GetPercentage(report)) switch
            {
                int i when i > 0 && i <= 10 => FluentSymbol.BatteryWarning24,
                int i when i > 10 && i <= 20 => FluentSymbol.Battery124,
                int i when i > 20 && i <= 30 => FluentSymbol.Battery224,
                int i when i > 30 && i <= 40 => FluentSymbol.Battery324,
                int i when i > 40 && i <= 50 => FluentSymbol.Battery424,
                int i when i > 50 && i <= 60 => FluentSymbol.Battery524,
                int i when i > 60 && i <= 70 => FluentSymbol.Battery624,
                int i when i > 70 && i <= 80 => FluentSymbol.Battery724,
                int i when i > 80 && i <= 90 => FluentSymbol.Battery824,
                int i when i > 90 && i < 99 => FluentSymbol.Battery924,
                _ => FluentSymbol.BatteryFull24
            };
        }

        /// <summary>
        /// Converts BatterReport info into a value of battery remaining.
        /// </summary>
        /// <param name="report">A BatteryReport object.</param>
        /// <returns>Returns a  value of the battery as a int./returns>
        public static int GetAbsolutePercentage(this BatteryReport report) => (int)GetPercentage(report) / 10;

        /// <summary>
        /// Converts BatterReport info into a percentage value of battery remaining.
        /// </summary>
        /// <param name="report">A BatteryReport object.</param>
        /// <returns>Returns a percentage value of the battery as a double./returns>
        private static double GetPercentage(this BatteryReport report) => (((double)report.RemainingCapacityInMilliwattHours / (double)report.FullChargeCapacityInMilliwattHours) * 100);

        /// <summary>
        /// Converts mWh to Wh.
        /// </summary>
        /// <param name="mWh">A mWh value.</param>
        /// <returns>Returns a wH value./returns>
        private static double mWhToWh(double mWh) => Math.Round(Math.Abs(mWh) * 0.001);
    }
}
