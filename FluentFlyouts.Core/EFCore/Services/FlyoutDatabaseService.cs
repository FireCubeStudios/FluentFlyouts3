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
			throw new NotImplementedException();
		}

		public IList<BatteryHistory> GetBatteryHistory() => Context.BatteryHistory.ToList();
	}
}
