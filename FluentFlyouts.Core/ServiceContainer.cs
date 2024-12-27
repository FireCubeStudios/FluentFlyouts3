using FluentFlyouts.Core.EFCore;
using FluentFlyouts.Core.EFCore.Services;
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

		public static IServiceProvider ConfigureServices()
		{
			ServiceCollection services = new ServiceCollection();

			services.AddSingleton<IFlyoutDatabaseService, FlyoutDatabaseService>();
			services.AddDbContext<FlyoutDBContext>();

			Services = services.BuildServiceProvider();
			return Services;
		}
	}
}
