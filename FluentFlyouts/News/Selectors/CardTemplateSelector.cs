using FluentFlyouts.News.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.News.Selectors
{
	public class CardTemplateSelector : DataTemplateSelector
	{
		public DataTemplate Default { get; set; }
		public DataTemplate Article { get; set; }
		public DataTemplate StockQuote { get; set; }
		public DataTemplate WeatherSummary { get; set; }

		protected override DataTemplate SelectTemplateCore(object item)
		{
			switch (item)
			{
				  case ArticleCard _:
					  return Article;

				  case StockQuoteCard _:
					  return StockQuote;

				case WeatherSummaryCard _:
					return WeatherSummary;

				 default:
					 case Card _:
						 return Default;
			}
		}

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			return SelectTemplateCore(item);
		}
	}
}
