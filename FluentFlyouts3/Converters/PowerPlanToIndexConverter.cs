using FluentFlyouts3.Classes;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts3.Converters
{
    public class PowerPlanToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => (double)(((PowerPlan)value).Plan);

        public object ConvertBack(object value, Type targetType, object parameter, string language) => (double)value switch
        {
            0 => PowerMode.PowerSaver, 
            1 => PowerMode.Recommended,
            2 => PowerMode.BetterPerformance,
            3 => PowerMode.BestPerformance,
            _ => PowerMode.Recommended
        };
    }
}
