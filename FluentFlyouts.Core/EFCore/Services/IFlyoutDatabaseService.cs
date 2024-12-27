using FluentFlyouts.Core.EFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Core.EFCore.Services
{
	public interface IFlyoutDatabaseService
	{
		IList<BatteryHistory> GetBatteryHistory();

		void AddBatteryHistory(BatteryHistory batteryHistory);
	}
}
