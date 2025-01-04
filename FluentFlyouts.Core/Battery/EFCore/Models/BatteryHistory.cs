using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Core.Battery.EFCore.Models
{
	public class BatteryHistory
	{
		public double Percentage { get; set; }
		public DateTime Time { get; set; }

		public BatteryHistory(double percentage, DateTime time)
		{
			Percentage = percentage;
			Time = time;
		}
	}
}
