using FluentFlyouts.News.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts.News.Controls
{
	public sealed partial class NewsControl : UserControl
	{
		public ObservableCollection<object> Cards { get; } = new ObservableCollection<object>();
		public NewsControl()
		{
			this.InitializeComponent();
		}

		private async void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			foreach (var card in Card.ProcessCards((await FluentFlyouts.News.Helpers.Api.GetFeed()).Cards))
			{
				Cards.Add(card);
			}
		}
	}
}
