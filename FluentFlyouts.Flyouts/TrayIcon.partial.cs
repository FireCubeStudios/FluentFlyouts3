using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.Windows.Windows;

namespace FluentFlyouts.Flyouts
{
	public partial class TrayIcon
	{
		private unsafe HWND CreateWindow(string Icon)
		{
			fixed (char* lpszClassName = "SystemTrayIconWindowClass" + Id.ToString())
			fixed (char* lpWindowName = Id.ToString())
			{
				WNDCLASSEXW wndClass;

				wndClass.cbSize = (uint)sizeof(WNDCLASSEXW);
				wndClass.style = 0;
				wndClass.lpfnWndProc = &TrayIcon.WndProc;
				wndClass.hInstance = GetModuleHandle(null);
				wndClass.hIcon = LoadIcon(Icon);
				wndClass.hCursor = GetCursor();
				wndClass.lpszClassName = lpszClassName;

				RegisterClassEx(&wndClass);
				return CreateWindowEx(0, lpszClassName, lpWindowName, 0, 0, 0, 0, 0, HWND.NULL, HMENU.NULL, wndClass.hInstance, null);
			}
		}

		private unsafe HCURSOR LoadIcon(string Icon)
		{
			fixed (char* iconPath = Path.Combine(AppContext.BaseDirectory, Icon))
			{
				return (HCURSOR)LoadImage(HINSTANCE.NULL, iconPath, 1, 0, 0, 0x00000010 | 0x00000020);
			}
		}

		private unsafe HCURSOR GetCursor()
		{
			fixed (char* cursor = "#32512")
			{
				return LoadCursor(HINSTANCE.NULL, cursor);
			}
		}

		private unsafe NOTIFYICONDATAW._szTip_e__FixedBuffer GetSzTip(string toolTip)
		{
			// Create a char array of length 128, padding with '\0' if necessary
			char[] szTip = toolTip.PadRight(128, '\0').ToCharArray();

			// Create the fixed buffer and copy the values from the char array
			NOTIFYICONDATAW._szTip_e__FixedBuffer result = new NOTIFYICONDATAW._szTip_e__FixedBuffer();

			for (int i = 0; i < 128; i++)
				result[i] = szTip[i];

			return result;
		}
	}
}
