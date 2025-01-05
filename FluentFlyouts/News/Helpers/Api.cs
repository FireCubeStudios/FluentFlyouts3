using FluentFlyouts.News.Models;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraFX.Interop.Windows;

namespace FluentFlyouts.News.Helpers
{
	public static class Api
	{
		public const string MSN_ASSETS_BASE_URL = "https://assets.msn.com";
		public const string MSN_ASSETS_WINSHELL_LATEST = MSN_ASSETS_BASE_URL + "/bundles/v1/windowsShell/latest";

		private const string MSFT_API_KEY = "pWw5OmQehOA0XNfgcgrTrwEJZJJJzE83ovtTQx6JRG";
		private const string MSFT_USER = "m-30F2319692C16C9719E53E21933D6DD4";

		public static async Task<Feed> GetFeed()
		{
			return await MSN_ASSETS_BASE_URL.AppendPathSegments("service", "news", "feed", "windows")
				.SetQueryParam("apikey", MSFT_API_KEY)
				.SetQueryParam("user", MSFT_USER)
				.SetQueryParam("ocid", "windows-windowsshellhp-feeds")
				.SetQueryParam("market", CultureInfo.CurrentCulture.Name)
			.GetJsonAsync<Feed>();
		}
	}
}
