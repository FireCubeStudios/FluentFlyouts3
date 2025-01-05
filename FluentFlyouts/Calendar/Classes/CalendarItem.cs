using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Windows.UI;

namespace FluentFlyouts.Calendar.Classes
{
	public class CalendarItem
	{
		public Visibility DetailsVisibility { get; set; }
		public Visibility LocationVisibility { get; set; }
		public Appointment Appointment { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public SolidColorBrush GlowBrush { get; set; }
		public Color GlowColor { get; set; }
	}
}
