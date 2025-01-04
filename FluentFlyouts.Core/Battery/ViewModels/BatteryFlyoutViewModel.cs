using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Core.Battery.ViewModels
{
	public partial class BatteryFlyoutViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool isSettingsShown = false;

		[ObservableProperty]
		private bool isPowerSliderEnabled = true;

		[ObservableProperty]
		private bool isEnergySaverEnabled = false;

		[ObservableProperty]
		private bool isRemainingCapacityEnabled = false;

		[ObservableProperty]
		private bool isDischargeRateEnabled = false;

		[ObservableProperty]
		private bool isBatteryHealthEnabled = false;
	}
}
