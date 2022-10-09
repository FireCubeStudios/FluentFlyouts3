using CommunityToolkit.WinUI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FluentFlyouts3.Services
{
    public partial class SettingsService
    {
        private bool isHealthEnabled = (bool)Settings.Values["IsHealthEnabled"];
        public bool IsHealthEnabled
        {
            get => isHealthEnabled; 
            set { 
                Settings.Values["IsHealthEnabled"] = value;
                SetProperty(ref isHealthEnabled, value);
            }
        }

        private bool isPowerSliderEnabled = (bool)Settings.Values["IsPowerSliderEnabled"];
        public bool IsPowerSliderEnabled
        {
            get => isPowerSliderEnabled;
            set {
                Settings.Values["IsPowerSliderEnabled"] = value;
                SetProperty(ref isPowerSliderEnabled, value);
            }
        }

        private bool isAdditionalInformationEnabled = (bool)Settings.Values["IsAdditionalInformationEnabled"];
        public bool IsAdditionalInformationEnabled
        {
            get => isAdditionalInformationEnabled;
            set {
                Settings.Values["IsAdditionalInformationEnabled"] = value;
                SetProperty(ref isAdditionalInformationEnabled, value);
            }
        }
    }
}
