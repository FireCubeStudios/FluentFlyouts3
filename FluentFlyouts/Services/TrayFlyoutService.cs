using FluentFlyouts.Calendar.Flyouts;
using FluentFlyouts.Core.Interfaces;
using FluentFlyouts.Flyouts;
using FluentFlyouts.Flyouts.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using WinUIEx;

namespace FluentFlyouts.Services
{
	/*
	 * Manages active flyouts
	 */
	public class TrayFlyoutService
	{
		private MenuFlyout contextFlyout;
		private Dictionary<int, IFlyoutWindow> Flyouts = new();
		public TrayFlyoutService() 
		{
			contextFlyout = new MenuFlyout();
			contextFlyout.Items.Add(new MenuFlyoutItem()
			{
				Icon = new FluentIcons.WinUI.SymbolIcon() { Symbol = FluentIcons.Common.Symbol.PersonFeedback },
				Text = "Send feedback on GitHub"
			});
			(contextFlyout.Items[0] as MenuFlyoutItem)!.Click += async (sender, e) => await Launcher.LaunchUriAsync(new Uri("https://github.com/FireCubeStudios/FluentFlyouts"));
			contextFlyout.Items.Add(new MenuFlyoutItem()
			{
				Icon = new FluentIcons.WinUI.SymbolIcon() { Symbol = FluentIcons.Common.Symbol.ContactCard },
				Text = "Contact Developer on Discord"
			});
			(contextFlyout.Items[1] as MenuFlyoutItem)!.Click += async (sender, e) => await Launcher.LaunchUriAsync(new Uri("https://discord.gg/3WYcKat"));
			contextFlyout.Items.Add(new MenuFlyoutSeparator());
			contextFlyout.Items.Add(new MenuFlyoutItem()
			{
				Icon = new FluentIcons.WinUI.SymbolIcon() { Symbol = FluentIcons.Common.Symbol.Settings },
				Text = "Settings"
			});
			(contextFlyout.Items[3] as MenuFlyoutItem)!.Click += (sender, e) =>
			{
				App.m_window ??= new MainWindow();
				App.m_window.Activate();
			};
		}

		public void AddFlyout(int Id, TrayIcon Icon, IFlyoutContent FlyoutContent)
		{
			if(!HasFlyout(Id))
				Flyouts.Add(Id, new TrayFlyoutWindow(Icon, FlyoutContent, contextFlyout));
		}

		public bool HasFlyout(int Id) => Flyouts.ContainsKey(Id);

		public void RemoveFlyout(int Id)
		{
			IFlyoutWindow? Flyout;
			Flyouts.TryGetValue(Id, out Flyout);
			Flyout?.Dispose();
			Flyouts.Remove(Id);
		}
	}
}
