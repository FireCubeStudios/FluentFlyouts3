using FluentFlyouts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FluentFlyouts.Tests.UnitTests
{
	public class SQLiteFileServiceTests
	{
		[Fact]
		public void GetConnectionTest()
		{
			SQLiteFileService service = new SQLiteFileService();
			var connection = service.GetLocalSQLiteDB();

			Assert.NotNull(connection);
			Assert.StartsWith(ApplicationData.Current.LocalFolder.Path, connection);
			Assert.EndsWith(".db", connection);
		}
	}
}
