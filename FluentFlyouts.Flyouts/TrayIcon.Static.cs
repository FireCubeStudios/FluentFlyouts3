using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.Windows.WM;
using static TerraFX.Interop.Windows.Windows;
using FluentFlyouts.Flyouts.Helpers;

namespace FluentFlyouts.Flyouts
{
	public partial class TrayIcon
	{
		private static Dictionary<uint, TrayIcon> Icons = new();
		private static HashSet<uint> IconId = new();

		public static event EventHandler<bool> SystemThemeChanged;

		private static void AddIcon(uint Id, TrayIcon Icon)
		{
			IconId.Add(0x8000 + Id);
			Icons.Add(0x8000 + Id, Icon);
		}

		private static void RemoveIcon(uint Id)
		{
			IconId.Remove(0x8000 + Id);
			Icons.Remove(0x8000 + Id);
		}

		[UnmanagedCallersOnly]
		public static unsafe LRESULT WndProc(HWND hWnd, uint message, WPARAM wParam, LPARAM lParam)
		{
			if (IconId.Contains(message))
			{
				if (lParam == WM_LBUTTONDOWN)
				{
					TrayIcon Icon;
					Icons.TryGetValue(message, out Icon);
					Icon?.LeftClicked?.Invoke(null, EventArgs.Empty);
				}
				else if (lParam == WM_RBUTTONDOWN)
				{
					TrayIcon Icon;
					Icons.TryGetValue(message, out Icon);
					Icon?.RightClicked?.Invoke(null, EventArgs.Empty);
				}
			}
			else if (message == WM_SETTINGCHANGE)
			{
				if (Marshal.PtrToStringUni(lParam)! == "ImmersiveColorSet")
					SystemThemeChanged?.Invoke(null, ThemeHelper.IsSystemThemeDark());
			}

			return DefWindowProc(hWnd, message, wParam, lParam);
		}
	}
}
