using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace FluentFlyouts3.Services
{
    public partial class SettingsService : ObservableObject
    {
        private static ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;

        public SettingsService()
        {
            if (SystemInformation.Instance.IsAppUpdated)
            {
                // Place new 
                IsPowerSliderEnabled = true;
                IsAdditionalInformationEnabled = true;
                IsHealthEnabled = true;
                XB = 100;
                YB = 100;
            }
        }

        [RelayCommand]
        public void LaunchSettings()
        {
            SettingsWindow s_window = new SettingsWindow();
            s_window.Activate();
        }
    }
}
