﻿using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Network.Converters
{
	public class WifiGlyphConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (!(value is byte))
			{
				return null;
			}

			var strength = (byte)value;

			switch (strength)
			{
				case 0:
					return "\xE904";
				case 1:
					return "\xE905";
				case 2:
				case 3:
					return "\xE906";
				case 4:
					return "\xE907";
				default:
					return "\xE908";
			}

		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
