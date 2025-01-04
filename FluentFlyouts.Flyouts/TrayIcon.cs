using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.Windows.WM;
using static TerraFX.Interop.Windows.Windows;

namespace FluentFlyouts.Flyouts
{
	/*
	 * TO-DO: Comment how the code works
	 */
	public unsafe partial class TrayIcon : IDisposable
	{
		private HWND windowHandle;
		private HICON notifyIconHandle;
		private NOTIFYICONDATAW notifyIconData;

		public event EventHandler LeftClicked;
		public event EventHandler RightClicked;

		public uint Id; // Used for callback to get clicked messages

		public TrayIcon(uint Id, string Icon, string ToolTip)
		{
			unsafe
			{
				this.Id = Id;
				windowHandle = CreateWindow(Icon);
				notifyIconHandle = LoadIcon(Icon);
				notifyIconData = new NOTIFYICONDATAW
				{
					cbSize = (uint)sizeof(NOTIFYICONDATAW),
					hWnd = windowHandle,
					uID = Id,
					uFlags = NIF_ICON | NIF_MESSAGE | NIF_TIP | NIF_SHOWTIP,
					uCallbackMessage = WM_APP + Id,
					hIcon = notifyIconHandle,
					szTip = GetSzTip(ToolTip)
				};

				// Add the icon
				fixed (NOTIFYICONDATAW* pNotifyIconData = &notifyIconData)
					Shell_NotifyIcon(NIM_ADD, pNotifyIconData);

				TrayIcon.AddIcon(Id, this);
			}
		}

		public void UpdateIcon(string Icon)
		{
			notifyIconHandle = LoadIcon(Icon);
			notifyIconData.hIcon = notifyIconHandle;
			fixed (NOTIFYICONDATAW* pNotifyIconData = &notifyIconData)
				Shell_NotifyIcon(NIM_MODIFY, pNotifyIconData);
		}

		public void UpdateTooltip(string ToolTip)
		{
			notifyIconData.szTip = GetSzTip(ToolTip);
			fixed (NOTIFYICONDATAW* pNotifyIconData = &notifyIconData)
				Shell_NotifyIcon(NIM_MODIFY, pNotifyIconData);
		}

		~TrayIcon() => Dispose();
		public unsafe void Dispose()
		{
			fixed (NOTIFYICONDATAW* pNotifyIconData = &notifyIconData)
			{
				TrayIcon.RemoveIcon(Id);
				Shell_NotifyIcon(NIM_DELETE, pNotifyIconData);
				DestroyIcon(notifyIconHandle);
				DestroyWindow(windowHandle);
			}
		}
	}
}
