using FluentFlyouts.Core.EFCore;
using FluentFlyouts.Core.EFCore.Models;
using FluentFlyouts.Core.EFCore.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FluentFlyouts.Core.Tests.UnitTests
{
	public class FlyoutDatabaseServiceTests
	{
		private IFlyoutDatabaseService databaseService;

		public FlyoutDatabaseServiceTests() 
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
		}

		[Fact]
		public void AddInvalidBatteryHistory()
		{
			// Ensure it fails if null is added or if invalid BatteryHistory is added
			Assert.ThrowsAny<Exception>(() => databaseService.AddBatteryHistory(null));
			Assert.ThrowsAny<Exception>(() => databaseService.AddBatteryHistory(new BatteryHistory(-1, DateTime.Now.AddDays(1))));

			var list = databaseService.GetBatteryHistory();
			Assert.Empty(list); // List should be empty as none of the items should have been added
		}

		[Fact]
		public void GetBatteryHistory()
		{
			var list = databaseService.GetBatteryHistory();
			Assert.Empty(list); // List should be empty first

			// Add items in random order and check if they come back in descending order
			databaseService.AddBatteryHistory(new BatteryHistory(70, DateTime.Now.AddDays(-2)));
			databaseService.AddBatteryHistory(new BatteryHistory(70, DateTime.Now.AddDays(-1)));
			databaseService.AddBatteryHistory(new BatteryHistory(70, DateTime.Now.AddDays(-3)));

			list = databaseService.GetBatteryHistory();
			Assert.NotEmpty(list); // List should not be empty
			Assert.Equal(3, list.Count); // List should have 3 items

			// Check if list items are in descending order
			for(int i = 0; i < list.Count - 1; i++)
			{
				if(list[i] == null) Assert.Fail();

				if (list[i].Time < list[i + 1].Time) Assert.Fail();
			}
		}
	}
}
