using FluentFlyouts.Core.EFCore;
using FluentFlyouts.Core.EFCore.Models;
using FluentFlyouts.Core.EFCore.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FluentFlyouts.Core.Tests.UnitTests
{
	public class FlyoutDatabaseTests
	{
		private IFlyoutDatabaseService databaseService;

		public FlyoutDatabaseTests() 
		{
			var options = new DbContextOptionsBuilder<FlyoutDBContext>().UseSqlite(new SqliteConnection("Filename=:memory:")).Options;
			
			var context = new FlyoutDBContext(options);
			context.Database.OpenConnection();
			context.Database.EnsureCreated();

			databaseService = new FlyoutDatabaseService(context);
		}

		[Xunit.Theory]
		[InlineData(70)]
		public void AddBatteryHistory(double percentage)
		{
			var list = databaseService.GetBatteryHistory();
			Assert.Empty(list); // List should be empty first

			var history = new BatteryHistory(percentage, DateTime.Now);
			databaseService.AddBatteryHistory(history);
			list = databaseService.GetBatteryHistory();

			Assert.NotEmpty(list); // List should not be empty
			Assert.Equal(history.Percentage, list[0].Percentage); // Item should be the same
			Assert.Equal(history.Time, list[0].Time); // Item should be the same

			// Ensure it fails if null is added or if invalid BatteryHistory is added
			Assert.ThrowsAny<Exception>(() => databaseService.AddBatteryHistory(null));
			Assert.ThrowsAny<Exception>(() => databaseService.AddBatteryHistory(new BatteryHistory(-1, DateTime.Now.AddDays(1))));
		}

		[Fact]
		public void GetBatteryHistory()
		{
			var list = databaseService.GetBatteryHistory();
			Assert.Empty(list); // List should be empty first
			Assert.IsAssignableFrom<IList<BatteryHistory>>(list);
		}
	}
}
