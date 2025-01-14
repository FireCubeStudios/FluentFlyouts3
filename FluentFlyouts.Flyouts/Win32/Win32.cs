using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace FluentFlyouts.Flyouts.Win32
{
	public partial class Win32
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;

			public static implicit operator Point(POINT point)
			{
				return new Point(point.X, point.Y);
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct WndClassEx
		{
			public uint cbSize;
			public uint style;
			public IntPtr lpfnWndProc;
			public int cbClsExtra;
			public int cbWndExtra;
			public IntPtr hInstance;
			public IntPtr hIcon;
			public IntPtr hCursor;
			public IntPtr hbrBackground;
			public string lpszMenuName;
			public string lpszClassName;
			public IntPtr hIconSm;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NotifyIconData
		{
			public uint cbSize;
			public IntPtr hWnd;
			public uint uID;
			public uint uFlags;
			public uint uCallbackMessage;
			public IntPtr hIcon;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string szTip;
			public uint dwState;
			public uint dwStateMask;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string szInfo;
			public uint uTimeoutOrVersion;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string szInfoTitle;
			public uint dwInfoFlags;
			public Guid guidItem;
			public IntPtr hBalloonIcon;
		}

		public delegate IntPtr WndProcDelegate(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		public static RECT GetScreenBounds(int x, int y)
		{
			POINT pt = new POINT { X = x, Y = y };
			IntPtr hMonitor = MonitorFromPoint(pt, MONITOR_DEFAULTTONEAREST);

			MONITORINFO monitorInfo = new MONITORINFO();
			monitorInfo.cbSize = (uint)Marshal.SizeOf(monitorInfo);

			if (GetMonitorInfo(hMonitor, ref monitorInfo))
			{
				return monitorInfo.rcWork; // rcWork excludes taskbar; rcMonitor includes it
			}

			return new RECT { Left = 0, Top = 0, Right = 1920, Bottom = 1080 }; // Default fallback
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MONITORINFO
		{
			public uint cbSize;
			public RECT rcMonitor;
			public RECT rcWork;
			public uint dwFlags;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
	}
}
