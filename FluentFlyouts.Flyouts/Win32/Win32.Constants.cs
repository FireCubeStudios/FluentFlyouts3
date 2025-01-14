using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Flyouts.Win32
{
	public partial class Win32
	{
		public const int HWND_TOPMOST = -1;
		public const int WM_APP = 0x8000;
		public const int WM_LBUTTONDOWN = 0x0201;
		public const int WM_RBUTTONDOWN = 0x0204;
		public const int NIM_ADD = 0x00000000;
		public const int NIM_MODIFY = 0x00000001;
		public const int NIM_DELETE = 0x00000002;
		public const int NIF_MESSAGE = 0x00000001;
		public const int NIF_ICON = 0x00000002;
		public const int NIF_TIP = 0x00000004;
		public const int NIF_SHOWTIP = 0x00000010;

		public const int GWL_STYLE = -16;
		public static int WS_BORDER = 0x00800000;
		public static int WS_DLGFRAME = 0x00400000;
		public static int WS_CAPTION = WS_BORDER | WS_DLGFRAME;
		public const int WS_SYSMENU = 0x00080000; 
		public const int WS_MAXIMIZEBOX = 0x00010000;
		public const int WS_MINIMIZEBOX = 0x00020000;
		public const uint WS_POPUP = 0x80000000;
		public const uint WS_SIZEBOX = 0x00040000;
		public const uint WS_THICKFRAME = 0x00040000;
		public const int GWL_EXSTYLE = -20;
		public const uint WS_EX_DLGMODALFRAME = 0x00000001;
		public const uint WS_EX_CLIENTEDGE = 0x00000200;
		public const uint WS_EX_STATICEDGE = 0x00020000;
		public const int SWP_FRAMECHANGED = 0x0020;
		public const int SWP_NOZORDER = 0x0004;
		public const int SWP_NOOWNERZORDER = 0x0200;
		public const int SWP_NOACTIVATE = 0x0010;
		public const int SWP_NOMOVE = 0x0002;
		public const int SWP_NOSIZE = 0x0001;

		private const uint MONITOR_DEFAULTTONEAREST = 2;
	}
}
