using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FluentFlyouts.Services
{
	public class SettingsService : ObservableObject
	{
		private static ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;

		private bool isClockFlyoutEnabled = (bool)(Settings.Values["ClockFlyoutEnabled_Preview"] ?? true);
		public bool IsClockFlyoutEnabled
		{
			get => isClockFlyoutEnabled;
			set
			{
				Settings.Values["ClockFlyoutEnabled_Preview"] = value;
				SetProperty(ref isClockFlyoutEnabled, value);
			}
		}

		private bool isCalendarFlyoutEnabled = (bool)(Settings.Values["CalendarFlyoutEnabled_Preview"] ?? true);
		public bool IsCalendarFlyoutEnabled
		{
			get => isCalendarFlyoutEnabled;
			set
			{
				Settings.Values["CalendarFlyoutEnabled_Preview"] = value;
				SetProperty(ref isCalendarFlyoutEnabled, value);
			}
		}

		private bool isBrightnessFlyoutEnabled = (bool)(Settings.Values["BrightnessFlyoutEnabled_Preview"] ?? true);
		public bool IsBrightnessFlyoutEnabled
		{
			get => isBrightnessFlyoutEnabled;
			set
			{
				Settings.Values["BrightnessFlyoutEnabled_Preview"] = value;
				SetProperty(ref isBrightnessFlyoutEnabled, value);
			}
		}

		// Show brightness flyout if brightness changes
		private bool showBrightnessFlyoutWhenChanged = (bool)(Settings.Values["ShowBrightnessFlyoutWhenChanged_Preview"] ?? true);
		public bool ShowBrightnessFlyoutWhenChanged
		{
			get => showBrightnessFlyoutWhenChanged;
			set
			{
				Settings.Values["ShowBrightnessFlyoutWhenChanged_Preview"] = value;
				SetProperty(ref showBrightnessFlyoutWhenChanged, value);
			}
		}

		private bool isFirstRun = (bool)(Settings.Values["FirstRun_Preview"] ?? true);
		public bool IsFirstRun
		{
			get => isFirstRun;
			set
			{
				Settings.Values["FirstRun_Preview"] = value;
				SetProperty(ref isFirstRun, value);
			}
		}
	}
}
