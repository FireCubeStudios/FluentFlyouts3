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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.Calendar.Controls
{
	public sealed partial class ClockControl : UserControl
	{
		private DispatcherTimer timer;

		public ClockControl()
		{
			this.InitializeComponent();
			timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(1) // Update every second
			};
			timer.Tick += (sender, e) => UpdateClockHands();
			timer.Start();

			UpdateClockHands();
		}

		private void UpdateClockHands()
		{
			DateTime now = DateTime.Now;

			// Calculate angles for each clock hand
			///double secondsAngle = (now.Second / 60.0) * 360.0;
			double minutesAngle = ((now.Minute + now.Second / 60.0) / 60.0) * 360.0;
			double hoursAngle = ((now.Hour % 12 + now.Minute / 60.0) / 12.0) * 360.0;

			// Update RotateTransform angles
			///SecondsTransform.Angle = secondsAngle;
			MinutesTransform.Angle = minutesAngle;
			HoursTransform.Angle = hoursAngle;
		}
	}
}
