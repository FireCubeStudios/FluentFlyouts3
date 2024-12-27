using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Core.EFCore.Models
{
	public class BatteryHistory
	{
		public int Percentage { get; set; }
		public DateTime Time { get; set; }
	}
}
