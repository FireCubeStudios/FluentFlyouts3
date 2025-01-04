using FluentFlyouts.Core.Battery.EFCore.Repositories;
using FluentFlyouts.Core.Battery.Services;
using PowerStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Battery.Services
{
	public class BatteryService : IBatteryService
	{
		private IBatteryRepository batteryRepository;
		public BatteryService(IBatteryRepository batteryRepository)
		{
			this.batteryRepository = batteryRepository;
		}
	}
}
