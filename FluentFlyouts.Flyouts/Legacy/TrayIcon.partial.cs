using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static FluentFlyouts.Flyouts.Helpers.Win32;

namespace FluentFlyouts.Flyouts
{
	public partial class OldTrayIcon
	{
		private IntPtr CreateWindow(string Icon)
		{
			var wndClass = new WndClassEx
			{
				cbSize = (uint)Marshal.SizeOf<WndClassEx>(),
				style = 0,
				lpfnWndProc = Marshal.GetFunctionPointerForDelegate(wndProcDelegate),
				hInstance = GetModuleHandle(null),
				hIcon = LoadIcon(Icon),
				hCursor = LoadCursor(),
				lpszClassName = "SystemOldTrayIconWindowClass" + Id.ToString(),
			};
			RegisterClassEx(ref wndClass);
			return CreateWindowEx(0, "SystemOldTrayIconWindowClass" + Id.ToString(), Id.ToString(), 0, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, wndClass.hInstance, IntPtr.Zero);
		}

		private IntPtr LoadIcon(string Icon)
		{
			string iconPath = Path.Combine(AppContext.BaseDirectory, Icon);
			return LoadImage(IntPtr.Zero, iconPath, 1, 0, 0, 0x00000010 | 0x00000020);
		}

		private IntPtr LoadCursor() => Helpers.Win32.LoadCursor(IntPtr.Zero, "#32512");

		private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
		{
			if (msg == 0x8000 + Id)
			{
				if (lParam.ToInt32() == WM_LBUTTONDOWN)
				{
					LeftClicked?.Invoke(this, EventArgs.Empty);
				}
				else if (lParam.ToInt32() == WM_RBUTTONDOWN)
				{
					RightClicked?.Invoke(this, EventArgs.Empty);
				}
			}

			return DefWindowProc(hWnd, msg, wParam, lParam);
		}
	}
}
