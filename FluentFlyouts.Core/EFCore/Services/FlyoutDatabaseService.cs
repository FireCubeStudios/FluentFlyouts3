using FluentFlyouts.Core.EFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Core.EFCore.Services
{
	public class FlyoutDatabaseService : IFlyoutDatabaseService
	{
		private FlyoutDBContext Context;
		public FlyoutDatabaseService(FlyoutDBContext context) 
		{
			Context = context;
		}

		public void AddBatteryHistory(BatteryHistory batteryHistory)
		{
			if (batteryHistory.Percentage < 0 || batteryHistory.Time > DateTime.Now) throw new Exception("Invalid BatteryHistory item");
			Context.BatteryHistory.Add(batteryHistory);
			Context.SaveChanges();
		}

		public IList<BatteryHistory> GetBatteryHistory() => Context.BatteryHistory.OrderByDescending(x => x.Time).ToList();
	}
}
