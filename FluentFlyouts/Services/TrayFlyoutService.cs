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
		public Dictionary<int, IFlyoutWindow> Flyouts = new();

		public void AddFlyout(int Id, Func<TrayIcon, IFlyoutContent> flyoutFactory)
		{
			if (!HasFlyout(Id))
			{
				var tray = new TrayIcon((uint)Id, "", "");
				var flyoutContent = flyoutFactory(tray);
				Flyouts.Add(Id, new TrayFlyoutWindow(tray, flyoutContent, GetContextFlyout()));
			}
		}


		public bool HasFlyout(int Id) => Flyouts.ContainsKey(Id);

		public void RemoveFlyout(int Id)
		{
			IFlyoutWindow? Flyout;
			Flyouts.TryGetValue(Id, out Flyout);
			Flyout?.Dispose();
			Flyouts.Remove(Id);
		}

		public void ClearAllFlyouts()
		{
			foreach(var flyout in Flyouts.Values)
				flyout.Dispose();
			
			Flyouts.Clear();
		}

		private MenuFlyout GetContextFlyout()
		{
			MenuFlyout contextFlyout = new MenuFlyout();
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
				App.OpenSettings();
			};

			return contextFlyout;
		}
	}
}
