using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using FluentFlyouts.Calendar.Pages;
using FluentFlyouts.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentFlyouts
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitleBar);
            SettingsNavigation.SelectedItem = SettingsNavigation.MenuItems[0];
        }

		private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Exit();

		private void SettingsNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
		{
            if (args.SelectedItem is null) return;
          
            if(args.SelectedItem == sender.MenuItems[0]) SettingsFrame.Navigate(typeof(HomeSettingsPage));
			else if(args.SelectedItem == sender.MenuItems[2]) SettingsFrame.Navigate(typeof(ClockSettingsPage));
		}
	}
}
