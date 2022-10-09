using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace FluentFlyouts3.Services
{
    public partial class SettingsService
    {
        private bool isStartupEnabled = GetStartupAsync().Result;
        public bool IsStartupEnabled
        {
            get => isStartupEnabled;
            set => SetProperty(ref isStartupEnabled, EnableStartupAsync().Result);
        }

        private static async Task<bool> GetStartupAsync()
        {
            StartupTask startupTask = await StartupTask.GetAsync("FF3");
            return (startupTask.State == StartupTaskState.Enabled || startupTask.State == StartupTaskState.EnabledByPolicy);
        }

        public async Task<bool> EnableStartupAsync()
        {
            StartupTask startupTask = await StartupTask.GetAsync("FF3");
          //  if(!startupTask.Equals(StartupTaskState.Enabled) || startupTask.State != StartupTaskState.EnabledByPolicy)
           //     await startupTask.RequestEnableAsync();
            return await GetStartupAsync();
        }
    }
}
