using CommunityToolkit.Mvvm.ComponentModel;
using FluentFlyouts.Screen.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Screen.ViewModels
{
	public partial class ScreenViewModel : ObservableObject
	{
		[ObservableProperty]
		private double screenBrightness;

		[ObservableProperty]
		private bool isBrightnessControlEnabled = false;

		public Action<Action>? UIThread;
		public ScreenService ScreenService;
		public ScreenViewModel(ScreenService ScreenService)
		{
			this.ScreenService = ScreenService;

			try
			{
				if (ScreenService.IsBrightnessControlEnabled)
				{
					screenBrightness = ScreenService.GetBrightness();
					ScreenService.BrightnessChanged += (object? sender, double e) =>
					{
						if (UIThread is not null)
							UIThread(() => ScreenBrightness = e);
						else
							ScreenBrightness = e;
					};
					IsBrightnessControlEnabled = true;
				}
			}
			catch
            {
				IsBrightnessControlEnabled = false;
            }
		}

		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			try
			{
				if (e.PropertyName == "ScreenBrightness")
				{
					ScreenService.SetBrightness(ScreenBrightness);
					return;
				}
			}
			catch
            {
                IsBrightnessControlEnabled = false;
            }
		}
	}
}
