using FluentFlyouts.Core.Battery.EFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Core.Battery.EFCore.Repositories
{
	public interface IBatteryRepository
	{
		IList<BatteryHistory> GetBatteryHistory();

		void AddBatteryHistory(BatteryHistory batteryHistory);
	}
}
