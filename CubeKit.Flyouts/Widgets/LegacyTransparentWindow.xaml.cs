using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using System.Runtime.InteropServices;
using DisplayInformation = Windows.Graphics.Display.DisplayInformation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CubeKit.Flyouts
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LegacyTransparentWindow : Window
    {
        public const int WM_CREATE = 0x0001;
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_COMMAND = 0x0111;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int WM_NCRBUTTONDOWN = 0x00A4;
        public const int WM_NCRBUTTONUP = 0x00A5;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;
        public const int WM_RBUTTONDBLCLK = 0x0206;
        public const int WM_MOUSEFIRST = 0x0200;
        public const int WM_MOUSELAST = 0x020E;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_MOVE = 0x0003;
        public const int WM_NCPOINTERDOWN = 0x0242;
        public const int WM_NCPOINTERUP = 0x0243;
        public const int WM_POINTERUPDATE = 0x0245;
        public const int WM_POINTERDOWN = 0x0246;
        public const int WM_POINTERUP = 0x0247;
        public const int WM_DESTROY = 2;
        public const int WM_PAINT = 0x0f;

        public const int WS_THICKFRAME = 0x00040000;
        public const int WS_CHILD = 0x40000000;
        public const int WS_POPUP = unchecked((int)0x80000000);

        public const int SC_MOVE = 0xF010;
        public const int SC_MOUSEMOVE = SC_MOVE + 0x02;

        public const int HTERROR = (-2);
        public const int HTTRANSPARENT = (-1);
        public const int HTNOWHERE = 0;
        public const int HTCLIENT = 1;
        public const int HTCAPTION = 2;
        public const int HTSYSMENU = 3;
        public const int HTGROWBOX = 4;
        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 16;
        public const int HTBOTTOMRIGHT = 17;

        public const int WS_EX_DLGMODALFRAME = 0x00000001;
        public const int WS_EX_NOPARENTNOTIFY = 0x00000004;
        public const int WS_EX_TOPMOST = 0x00000008;
        public const int WS_EX_ACCEPTFILES = 0x00000010;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_MDICHILD = 0x00000040;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_WINDOWEDGE = 0x00000100;
        public const int WS_EX_CLIENTEDGE = 0x00000200;
        public const int WS_EX_CONTEXTHELP = 0x00000400;
        public const int WS_EX_RIGHT = 0x00001000;
        public const int WS_EX_LEFT = 0x00000000;
        public const int WS_EX_RTLREADING = 0x00002000;
        public const int WS_EX_LTRREADING = 0x00000000;
        public const int WS_EX_LEFTSCROLLBAR = 0x00004000;
        public const int WS_EX_RIGHTSCROLLBAR = 0x00000000;
        public const int WS_EX_CONTROLPARENT = 0x00010000;
        public const int WS_EX_STATICEDGE = 0x00020000;
        public const int WS_EX_APPWINDOW = 0x00040000;
        public const int WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        public const int WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children
        public const int WS_EX_NOREDIRECTIONBITMAP = 0x00200000;
        public const int WS_EX_LAYOUTRTL = 0x00400000; // Right to left mirroring
        public const int WS_EX_COMPOSITED = 0x02000000;
        public const int WS_EX_NOACTIVATE = 0x08000000;

        public const uint LWA_COLORKEY = 0x00000001;
        public const uint LWA_ALPHA = 0x00000002;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        private static long MakeArgb(byte alpha, byte red, byte green, byte blue)
        {
            return (long)(((ulong)((((red << 0x10) | (green << 8)) | blue) | (alpha << 0x18))) & 0xffffffffL);
        }

        const int GWL_STYLE = (-16);
        const int GWL_EXSTYLE = (-20);
        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 4)
            {
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
        public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        // public static IntPtr GetWindowLong(HandleRef hWnd, int nIndex)
        public static long GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return GetWindowLong32(hWnd, nIndex);
            }
            return GetWindowLongPtr64(hWnd, nIndex);
        }

        [DllImport("User32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        public static extern long GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("User32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern long GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool ShowWindow(IntPtr hWnd, int nShowCmd);

        public const int SW_HIDE = 0;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;

        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int PostMessage(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int DefWindowProc(IntPtr hWnd, uint uMsg, int wParam, IntPtr lParam);

        [DllImport("Gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);

        [DllImport("Gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, int iMode);

        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("Gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

        public const int RGN_AND = 1;
        public const int RGN_OR = 2;
        public const int RGN_XOR = 3;
        public const int RGN_DIFF = 4;
        public const int RGN_COPY = 5;
        public const int RGN_MIN = RGN_AND;
        public const int RGN_MAX = RGN_COPY;

        public const int ERROR = 0;
        public const int NULLREGION = 1;
        public const int SIMPLEREGION = 2;
        public const int COMPLEXREGION = 3;

        [DllImport("Gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        public delegate int SUBCLASSPROC(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData);

        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern bool SetWindowSubclass(IntPtr hWnd, SUBCLASSPROC pfnSubclass, uint uIdSubclass, uint dwRefData);

        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern int DefSubclassProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public RECT(int Left, int Top, int Right, int Bottom)
            {
                left = Left;
                top = Top;
                right = Right;
                bottom = Bottom;
            }
        }

        public static int GET_X_LPARAM(IntPtr lParam)
        {
            return LOWORD(lParam.ToInt32());
        }

        public static int GET_Y_LPARAM(IntPtr lParam)
        {
            return HIWORD(lParam.ToInt32());
        }
        public static int HIWORD(int i)
        {
            return (short)(i >> 16);
        }
        public static int LOWORD(int i)
        {
            return (short)(i & 0xFFFF);
        }

        private static int MakeLParam(int LoWord, int HiWord)
        {
            int res = (int)((HiWord << 16) | (LoWord & 0xffff));
            return res;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public POINT(int X, int Y)
            {
                this.x = X;
                this.y = Y;
            }
        }

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool PtInRect(ref RECT lprc, POINT pt);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);

        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOREDRAW = 0x0008;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
        public const int SWP_SHOWWINDOW = 0x0040;
        public const int SWP_HIDEWINDOW = 0x0080;
        public const int SWP_NOCOPYBITS = 0x0100;
        public const int SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
        public const int SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */
        public const int SWP_DRAWFRAME = SWP_FRAMECHANGED;
        public const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;
        public const int SWP_DEFERERASE = 0x2000;
        public const int SWP_ASYNCWINDOWPOS = 0x4000;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

        public const int RDW_INVALIDATE = 0x0001;
        public const int RDW_INTERNALPAINT = 0x0002;
        public const int RDW_ERASE = 0x0004;

        public const int RDW_VALIDATE = 0x0008;
        public const int RDW_NOINTERNALPAINT = 0x0010;
        public const int RDW_NOERASE = 0x0020;

        public const int RDW_NOCHILDREN = 0x0040;
        public const int RDW_ALLCHILDREN = 0x0080;

        public const int RDW_UPDATENOW = 0x0100;
        public const int RDW_ERASENOW = 0x0200;

        public const int RDW_FRAME = 0x0400;
        public const int RDW_NOFRAME = 0x0800;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short GetAsyncKeyState(int nVirtKey);

        public const int VK_LBUTTON = 0x01;
        public const int VK_RBUTTON = 0x02;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        public const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        public const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
        public const int MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; /* middle button down */
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040; /* middle button up */
        public const int MOUSEEVENTF_XDOWN = 0x0080; /* x button down */
        public const int MOUSEEVENTF_XUP = 0x0100; /* x button down */
        public const int MOUSEEVENTF_WHEEL = 0x0800; /* wheel button rolled */
        public const int MOUSEEVENTF_HWHEEL = 0x01000; /* hwheel button rolled */
        public const int MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000; /* do not coalesce mouse moves */
        public const int MOUSEEVENTF_VIRTUALDESK = 0x4000; /* map to entire virtual desktop */
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000; /* absolute move */

        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SendInput(int nInputs, [MarshalAs(UnmanagedType.LPArray)] INPUT[] pInput, int cbSize);

        public const int INPUT_MOUSE = 0;
        public const int INPUT_KEYBOARD = 1;
        public const int INPUT_HARDWARE = 2;

        public const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const int KEYEVENTF_KEYUP = 0x0002;
        public const int KEYEVENTF_UNICODE = 0x0004;

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public int type;
            public INPUTUNION inputUnion;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUTUNION
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr GetCapture();

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CallWindowProc(IntPtr wndProc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [DllImport("User32.dll", SetLastError = true, EntryPoint = "RegisterClassW")]
        public static extern short RegisterClass(ref WNDCLASS wc);

        [DllImport("User32.dll", SetLastError = true, EntryPoint = "RegisterClassExW")]
        public static extern short RegisterClassEx(ref WNDCLASSEX lpwcx);

        public delegate int WNDPROC(IntPtr hwnd, uint uMsg, int wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WNDCLASS
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint style;
            public WNDPROC lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string lpszClassName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WNDCLASSEX
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public int style;
            public WNDPROC lpfnWndProc;
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

        public const int WS_OVERLAPPEDWINDOW = 0xcf0000;
        public const int WS_VISIBLE = 0x10000000;
        public const int CS_USEDEFAULT = unchecked((int)0x80000000);
        public const int CS_DBLCLKS = 8;
        public const int CS_VREDRAW = 1;
        public const int CS_HREDRAW = 2;
        public const int COLOR_BACKGROUND = 1;
        public const int COLOR_WINDOW = 5;
        public const int IDC_ARROW = 32512;
        public const int IDC_IBEAM = 32513;
        public const int IDC_WAIT = 32514;
        public const int IDC_CROSS = 32515;
        public const int IDC_UPARROW = 32516;

        public const int BS_PUSHLIKE = 0x00001000;
        public const int BN_CLICKED = 0;

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string modName);


        public LegacyTransparentWindow()
        {
            this.InitializeComponent();

            // WinUIDesktopWin32WindowClass
            //     Microsoft.UI.Content.ContentWindowSiteBridge
            //     DRAG_BAR_WINDOW_CLASS   
            hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            _apw = AppWindow.GetFromWindowId(myWndId);

            hWndChild = FindWindowEx(hWnd, IntPtr.Zero, "Microsoft.UI.Content.ContentWindowSiteBridge", null);

            //SetWindowPos(hWndChild, new IntPtr(-1), 0, 0, 800, 800, SWP_NOMOVE | SWP_NOACTIVATE | SWP_NOZORDER | SWP_NOREDRAW);

            //ShowWindow(hWndChild, SW_HIDE);

            _apw.Title = "Test WindowsAppSDK";
            _apw.Resize(new Windows.Graphics.SizeInt32(320, 200));

            Grid grid1 = (Grid)this.Content;
            grid1.PointerPressed += Grid1_PointerPressed;
            //grid1.PointerReleased += Grid1_PointerReleased;

            _presenter = _apw.Presenter as OverlappedPresenter;
            _presenter.IsResizable = false;
            _presenter.IsMinimizable = false;
            _presenter.IsAlwaysOnTop = true;
            _presenter.SetBorderAndTitleBar(false, false);

            SubClassDelegate = new SUBCLASSPROC(WindowSubClass);
            bool bRet = SetWindowSubclass(hWnd, SubClassDelegate, 0, 0);

            //SetParent(hWndChild, IntPtr.Zero);
            //long nStyleChild = GetWindowLong(hWndChild, GWL_STYLE);
            ////nStyleChild = (nStyleChild | WS_POPUP) & (~WS_CHILD);
            //nStyleChild = (nStyleChild) & (~WS_CHILD);
            //SetWindowLong(hWndChild, GWL_STYLE, (IntPtr)(nStyleChild));

            ////SetWindowPos(hWnd, (IntPtr)(-1), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            //SetWindowPos(hWndChild, (IntPtr)(-1), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);

            //long nExStyleChild = GetWindowLong(hWndChild, GWL_EXSTYLE);
            //SetWindowLong(hWndChild, GWL_EXSTYLE, (IntPtr)(nExStyleChild | WS_EX_TOPMOST));


            long nExStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            if ((nExStyle & WS_EX_LAYERED) == 0)
            {
                SetWindowLong(hWnd, GWL_EXSTYLE, (IntPtr)(nExStyle | WS_EX_LAYERED));
                //nExStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
                //SetWindowLong(hWnd, GWL_EXSTYLE, (IntPtr)(nExStyle | WS_EX_TRANSPARENT));
                SetLayeredWindowAttributes(hWnd, (uint)0, (byte)(255 * nOpacity / 100), LWA_ALPHA);

                //SetLayeredWindowAttributes(hWnd, (uint)MakeArgb(0, 0, 0, 0), (byte)(255 * nOpacity / 100), LWA_COLORKEY);
                //SetLayeredWindowAttributes(hWnd, (uint)MakeArgb(0, 0, 0, 0), 255, LWA_ALPHA | LWA_COLORKEY);

            }
            nExStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            SetWindowLong(hWnd, GWL_EXSTYLE, (IntPtr)(nExStyle | WS_EX_NOACTIVATE));

            //nExStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            //SetWindowLong(hWnd, GWL_EXSTYLE, (IntPtr)(nExStyle | WS_EX_NOREDIRECTIONBITMAP));

            int nX = (int)(_apw.Size.Width - myButton.ActualOffset.X - myButton.Width);
            int nY = (int)myButton.ActualOffset.Y;
            ////IntPtr HoleRgn = CreateRectRgn((int)myButton.ActualOffset.X, (int)myButton.ActualOffset.Y, (int)(myButton.ActualOffset.X + myButton.Width), (int)(myButton.ActualOffset.Y + myButton.Height));
            //IntPtr HoleRgn = CreateRectRgn(nX, nY, (int)(nX + myButton.Width), (int)(nY + myButton.Height));
            //CombineRgn(WindowRgn, WindowRgn, HoleRgn, RGN_DIFF);
            //SetWindowRgn(hWndChild, WindowRgn, true);
            //DeleteObject(HoleRgn);

            IntPtr WindowRgn = CreateRectRgn(0, 0, _apw.Size.Width, _apw.Size.Height);
            IntPtr HoleRgn = CreateRectRgn(nX, nY, (int)(nX + myButton.Width), (int)(nY + myButton.Height));

            IntPtr HoleRgn2 = CreateRectRgn(0, 0, _apw.Size.Width, 20);
            IntPtr WindowRgn2 = CreateRectRgn(0, 0, 0, 0);
            int nRet = CombineRgn(WindowRgn2, HoleRgn2, HoleRgn, RGN_OR);


            //CombineRgn(WindowRgn, WindowRgn, HoleRgn, RGN_DIFF);
            CombineRgn(WindowRgn, HoleRgn, WindowRgn, RGN_MIN);
            //CombineRgn(WindowRgn, WindowRgn2, WindowRgn, RGN_MIN); 

            //SetWindowRgn(hWndChild, WindowRgn, true);
            DeleteObject(HoleRgn);

            //nExStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            //SetWindowLong(hWnd, GWL_EXSTYLE, (IntPtr)(nExStyle | WS_EX_TRANSPARENT));
            //SetWindowRgn(hWnd, IntPtr.Zero, true);

            return;
        }

        private AppWindow _apw;
        private OverlappedPresenter _presenter;
        private CompactOverlayPresenter _compact;

        private SUBCLASSPROC SubClassDelegate;

        private IntPtr hWnd = IntPtr.Zero;
        private IntPtr hWndChild = IntPtr.Zero;


        private void Grid1_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Console.Beep(6000, 10);
            //SetWindowPos(hWnd, hWndChild, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_HIDEWINDOW);
            RECT rc;
            GetWindowRect(hWnd, out rc);
            //SetWindowPos(hWnd, hWndChild, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_HIDEWINDOW);
            SetWindowPos(hWnd, (IntPtr)(-1), 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
            SetWindowPos(hWndChild, (IntPtr)(-2), rc.left, rc.top, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW | SWP_FRAMECHANGED | SWP_ASYNCWINDOWPOS);

        }

        private void Grid1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var properties = e.GetCurrentPoint((Grid)sender).Properties;
            if (properties.IsLeftButtonPressed)
            {
                long nExStyle = GetWindowLong(hWndChild, GWL_EXSTYLE);
                if ((nExStyle & WS_EX_LAYERED) == 0)
                {
                    SetWindowLong(hWndChild, GWL_EXSTYLE, (IntPtr)(nExStyle | WS_EX_LAYERED));
                    nExStyle = GetWindowLong(hWndChild, GWL_EXSTYLE);
                    // break events/UI
                    SetWindowLong(hWndChild, GWL_EXSTYLE, (IntPtr)(nExStyle | WS_EX_TRANSPARENT));
                    //Console.Beep(6000, 10);

                    //SetLayeredWindowAttributes(hWndChild, (uint)MakeArgb(0, 0, 0, 0), 100, LWA_ALPHA);
                }
                INPUT[] mi = new INPUT[2];
                mi[0].type = INPUT_MOUSE;
                mi[0].inputUnion.mi.dwFlags = MOUSEEVENTF_LEFTUP;
                mi[1].inputUnion.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
                SendInput(2, mi, Marshal.SizeOf(mi[0]));
            }
            else if (properties.IsRightButtonPressed)
            {
                System.Threading.Thread.Sleep(200);
                Application.Current.Exit();
            }
            e.Handled = true;
        }

        int nOpacity = 80;
        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            {
                delegateWndProc = Win32WndProc;
                WNDCLASSEX wcex = new WNDCLASSEX();
                wcex.cbSize = Marshal.SizeOf(typeof(WNDCLASSEX));
                wcex.style = (CS_HREDRAW | CS_VREDRAW | CS_DBLCLKS);
                wcex.hbrBackground = (IntPtr)COLOR_BACKGROUND + 1;
                wcex.cbClsExtra = 0;
                wcex.cbWndExtra = 0;
                wcex.hInstance = Marshal.GetHINSTANCE(this.GetType().Module); // Process.GetCurrentProcess().Handle;
                wcex.hIcon = IntPtr.Zero;
                wcex.hCursor = LoadCursor(IntPtr.Zero, (int)IDC_ARROW);
                wcex.lpszMenuName = null;
                wcex.lpszClassName = "Win32Class";
                //wind_class.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(delegWndProc);
                wcex.lpfnWndProc = delegateWndProc;
                wcex.hIconSm = IntPtr.Zero;
                short nRet = RegisterClassEx(ref wcex);
                if (nRet == 0)
                {
                    int nError = Marshal.GetLastWin32Error();
                    if (nError != 1410) //0x582 ERROR_CLASS_ALREADY_EXISTS
                        return;
                }
                string sClassName = wcex.lpszClassName;
                IntPtr hWnd2 = CreateWindowEx(0, sClassName, "Win32 window", WS_OVERLAPPEDWINDOW | WS_VISIBLE, _apw.Position.X, _apw.Position.Y, 270, 270, hWnd, IntPtr.Zero, wcex.hInstance, IntPtr.Zero);
            }
            return;

            // 0x8001010E (RPC_E_WRONG_THREAD))'
            //var thread = new System.Threading.Thread(() =>
            //{
            //    Window window = new Window();
            //    window.Content = new TextBlock() { Text = "Hello" };
            //    window.Activate();
            //    //window.Closed += (_, __) => w.DispatcherQueue.InvokeShutdown();

            //    var syncContext = new Microsoft.UI.Dispatching.DispatcherQueueSynchronizationContext(Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread());
            //    System.Threading.SynchronizationContext.SetSynchronizationContext(syncContext);

            //    //this.ProcessEvents(); // Message pump
            //});

            //thread.SetApartmentState(System.Threading.ApartmentState.STA);
            //thread.Start();
            //return;

            // MessageBox(hWnd, "Button clicked !", "Information", 0);

            // 0xc000027b
            //Window window = new Window();
            //window.Content = new TextBlock() { Text = "Hello" };
            //window.Activate();
            //return;

            //IntPtr WindowRgn = CreateRectRgn(0, 0, _apw.Size.Width / 2, _apw.Size.Height);
            //SetWindowRgn(hWnd, WindowRgn, true);

            TeachingTip.IsOpen = true;
            TeachingTip.XamlRoot = this.Content.XamlRoot;
            return;

            myButton.Content = "Clicked";
            var cd = new ContentDialog
            {
                Title = "Information",
                Content = "You clicked the button !",
                CloseButtonText = "Ok"
            };

            cd.XamlRoot = this.Content.XamlRoot;
            var result = await cd.ShowAsync();
        }

        int n = 0;
        private int WindowSubClass(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("uMsg({0}) = {1:x}", n, uMsg));
            n++;
            switch (uMsg)
            {
                //case WM_MOVE:
                //    {
                //        RECT rc;
                //        GetWindowRect(hWnd, out rc);
                //        //SetWindowPos(hWndChild, hWnd, rc.left, rc.top, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW | SWP_FRAMECHANGED );
                //        //SetWindowPos(hWnd, hWndChild, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOREDRAW);
                //        SetWindowPos(hWndChild, (IntPtr)(-1), rc.left, rc.top, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW | SWP_FRAMECHANGED);
                //        return 0;
                //    }
                //    break;

                // case WM_POINTERDOWN:
                // case WM_NCLBUTTONDOWN:
                //    {
                //        Console.Beep(1000, 10);
                //        //SetWindowPos(hWnd, hWndChild, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_HIDEWINDOW);

                //        //SetWindowPos(hWnd, hWndChild, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_HIDEWINDOW);
                //        SetWindowPos(hWnd, (IntPtr)(-2), 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
                //        SetWindowPos(hWndChild, (IntPtr)(-1), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW | SWP_FRAMECHANGED);
                //        //return 0;
                //    }
                //    break;
                //case WM_NCPOINTERUP:
                //case WM_LBUTTONUP:
                //case WM_POINTERUP:
                ////case WM_POINTERUPDATE:
                //    {
                //        Console.Beep(6000, 10);
                //        //SetWindowPos(hWnd, hWndChild, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_HIDEWINDOW);
                //        RECT rc;
                //        GetWindowRect(hWnd, out rc);
                //        //SetWindowPos(hWnd, hWndChild, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_HIDEWINDOW);
                //        SetWindowPos(hWnd, (IntPtr)(-1), 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
                //        SetWindowPos(hWndChild, (IntPtr)(-2), rc.left, rc.top, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW | SWP_FRAMECHANGED | SWP_ASYNCWINDOWPOS);
                //        //return 0;
                //    }
                //    break;
                //case WM_NCHITTEST:
                //    {
                //        //SendMessage(hWnd, WM_NCLBUTTONDOWN, HTCAPTION, lParam);
                //        //bool bLeftButton = (GetAsyncKeyState((int)VK_LBUTTON) & 0x8000) == 0x8000;
                //        //if (GetCapture() == IntPtr.Zero)
                //        //{
                //        //    Console.Beep(6000, 10);
                //        //    return HTCAPTION;
                //        //}
                //        //if (bLeftButton)
                //        {
                //            return HTCAPTION;
                //        }
                //    }
                //    break;
                case WM_NCHITTEST:
                    {
                        POINT pt = new POINT(GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam));
                        ScreenToClient(hWnd, ref pt);
                        int nWidth = 0;
                        int nCaptionSize = 0;
                        RECT rc = new RECT((int)myButton.ActualOffset.X + nWidth, (int)myButton.ActualOffset.Y + nCaptionSize, (int)(myButton.ActualOffset.X + nWidth + myButton.Width), (int)(myButton.ActualOffset.Y + nCaptionSize + myButton.Height));
                        bool bRet = PtInRect(ref rc, pt);
                        bool bRightButton = (GetAsyncKeyState((int)VK_RBUTTON) & 0x8000) == 0x8000;
                        bool bLeftButton = (GetAsyncKeyState((int)VK_LBUTTON) & 0x8000) == 0x8000;
                        if (!bLeftButton)
                        {
                            long nExStyle = GetWindowLong(hWndChild, GWL_EXSTYLE);
                            if ((nExStyle & WS_EX_LAYERED) == WS_EX_LAYERED)
                            {
                                SetWindowLong(hWndChild, GWL_EXSTYLE, (IntPtr)(nExStyle & ~(WS_EX_LAYERED | WS_EX_TRANSPARENT)));
                            }
                            break;
                        }

                        int nRet = DefWindowProc(hWnd, WM_NCHITTEST, 0, lParam);
                        //System.Diagnostics.Debug.WriteLine(String.Format("WM_NCHITTEST = {0}", nRet));
                        switch (nRet)
                        {
                            case HTLEFT:
                            case HTTOP:
                            case HTBOTTOM:
                            case HTRIGHT:
                            case HTBOTTOMLEFT:
                            case HTBOTTOMRIGHT:
                            case HTTOPLEFT:
                            case HTTOPRIGHT:
                                return nRet;

                        }
                        return HTCAPTION;
                    }
                    break;
            }
            return DefSubclassProc(hWnd, uMsg, wParam, lParam);
        }
        private const int IDC_BUTTON = 10;
        private WNDPROC delegateWndProc;
        private int Win32WndProc(IntPtr hwnd, uint msg, int wParam, IntPtr lParam)
        {
            int wmId, wmEvent;
            switch (msg)
            {
                case WM_CREATE:
                    {
                        IntPtr hWndButton = CreateWindowEx(0, "Button", "Click", WS_CHILD | WS_VISIBLE | BS_PUSHLIKE, 100, 92, 60, 32, hwnd, (IntPtr)IDC_BUTTON, GetModuleHandle(null), IntPtr.Zero);
                    }
                    break;
                case WM_COMMAND:
                    {
                        wmId = LOWORD(wParam);
                        wmEvent = HIWORD(wParam);
                        switch (wmId)
                        {
                            case IDC_BUTTON:
                                {
                                    if (wmEvent == BN_CLICKED)
                                    {
                                        nOpacity -= 10;
                                        SetLayeredWindowAttributes(hWnd, 0, (byte)(255 * nOpacity / 100), LWA_ALPHA);
                                    }
                                }
                                break;
                            default:
                                return DefWindowProc(hwnd, msg, wParam, lParam);
                        }
                    }
                    break;
                //case WM_PAINT:
                //    break;

                //case WM_LBUTTONDBLCLK:
                //    Console.Beep(5000, 10);
                //    //System.Windows.Forms.MessageBox.Show("Doubleclick");
                //    break;

                //case WM_DESTROY:
                //    DestroyWindow(hWnd);
                //    //PostQuitMessage(0);
                //    break;

                default:
                    break;
            }
            return DefWindowProc(hwnd, msg, wParam, lParam);
        }
    }
}
