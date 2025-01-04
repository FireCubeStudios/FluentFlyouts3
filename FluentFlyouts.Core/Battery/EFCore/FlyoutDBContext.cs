using FluentFlyouts.Core.Battery.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Core.Battery.EFCore
{
	public class FlyoutDBContext : DbContext
	{
		public DbSet<BatteryHistory> BatteryHistory { get; set; }

		public FlyoutDBContext(DbContextOptions<FlyoutDBContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Set a composite key
			modelBuilder.Entity<BatteryHistory>().HasKey(x => new { x.Percentage, x.Time });
		}
	}
}
