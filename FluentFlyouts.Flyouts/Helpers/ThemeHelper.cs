using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Flyouts.Helpers
{
	public class ThemeHelper
	{
		[DllImport("uxtheme.dll", EntryPoint = "#138", CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool ShouldAppsUseDarkMode();

		public static bool IsSystemThemeDark() => ShouldAppsUseDarkMode();
	}
}
