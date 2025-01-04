using FluentFlyouts.Core.Battery.EFCore.Repositories;
using FluentFlyouts.Core.Battery.Services;
using FluentFlyouts.Core.Battery.EFCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentFlyouts.Core.Battery.ViewModels;

namespace FluentFlyouts.Core
{
	public class ServiceContainer
	{
		public static IServiceProvider? Services { get; set; }

		public static IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IBatteryRepository, BatteryRepository>();
			services.AddSingleton<BatteryService>();
			services.AddSingleton<BatteryFlyoutViewModel>();

			Services = services.BuildServiceProvider();
			return Services;
		}
	}
}
