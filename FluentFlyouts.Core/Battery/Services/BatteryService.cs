using FluentFlyouts.Core.Battery.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Core.Battery.Services
{
	public class BatteryService : IBatteryService
	{
		private IBatteryRepository batteryRepository;
		public BatteryService(IBatteryRepository batteryRepository)
		{
			this.batteryRepository = batteryRepository;
			/*var statusProvider = new PowerStatusProvider();
			var status = statusProvider.GetStatus();*/
		}
	}
}
