using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static FluentFlyouts.Flyouts.Helpers.Win32;

namespace FluentFlyouts.Flyouts
{
	public partial class TrayIcon : IDisposable
	{
		private IntPtr windowHandle;
		private IntPtr notifyIconHandle;
		private NotifyIconData notifyIconData;
		private WndProcDelegate wndProcDelegate;

		public event EventHandler LeftClicked;
		public event EventHandler RightClicked;

		private uint Id; // Used for callback to recieve clicked messages

		public TrayIcon(uint Id, string Icon, string ToolTip)
		{
			this.Id = Id;
			wndProcDelegate = new WndProcDelegate(WndProc);
			windowHandle = CreateWindow(Icon);
			SetWndProc();
			notifyIconHandle = LoadIcon(Icon);
			notifyIconData = new NotifyIconData
			{
				cbSize = (uint)Marshal.SizeOf<NotifyIconData>(),
				hWnd = windowHandle,
				uID = Id,
				uFlags = NIF_ICON | NIF_MESSAGE | NIF_TIP | NIF_SHOWTIP,
				uCallbackMessage = WM_APP + Id,
				hIcon = notifyIconHandle,
				szTip = ToolTip.PadRight(128, '\0')
			};

			// Add the icon
			Shell_NotifyIcon(NIM_ADD, ref notifyIconData);
		}

		private void SetWndProc()
		{
			GCHandle.Alloc(wndProcDelegate);
			SetWindowLong(windowHandle, 0, Marshal.GetFunctionPointerForDelegate(wndProcDelegate));
		}

		public void Dispose()
		{
			Shell_NotifyIcon(NIM_DELETE, ref notifyIconData);
			DestroyIcon(notifyIconHandle);
			DestroyWindow(windowHandle);
		}
	}
}
