using FluentFlyouts.Core.EFCore;
using FluentFlyouts.Core.EFCore.Repositories;
using FluentFlyouts.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Core
{
	public class ServiceContainer
	{
		public static IServiceProvider? Services { get; set; }

		public static IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IBatteryRepository, BatteryRepository>();
			services.AddSingleton<BatteryService>();

			Services = services.BuildServiceProvider();
			return Services;
		}
	}
}
