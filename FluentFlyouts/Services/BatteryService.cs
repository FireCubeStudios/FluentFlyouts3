using FluentFlyouts.Core.EFCore.Repositories;
using PowerStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Core.Services
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
