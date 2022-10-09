using CommunityToolkit.Mvvm.ComponentModel;
using FluentFlyouts3.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts3.Services
{
    [INotifyPropertyChanged]
    public partial class PowerService
    {

        private PowerPlan currentPowerPlan = GetCurrentPlan();
        public PowerPlan CurrentPowerPlan
        {
            get { return currentPowerPlan; }
            set
            {
                SetPowerPlan(((PowerPlan)value).Id);
                SetProperty(ref currentPowerPlan, value);
            }
        }

        private static PowerPlan GetCurrentPlan() => GetCurrentPlanGUID() switch
        {
            PowerPlanIds.PowerSaver => PowerMode.PowerSaver,
            PowerPlanIds.Recommended => PowerMode.Recommended,
            PowerPlanIds.Recommended11 => PowerMode.Recommended,
            PowerPlanIds.BetterPerformance => PowerMode.BetterPerformance,
            PowerPlanIds.BestPerformance => PowerMode.BestPerformance,
            PowerPlanIds.BestPerformance11 => PowerMode.BestPerformance,
            _ => PowerMode.Recommended
        };

        /// <summary>
        /// Retrieves the active overlay power scheme and returns a GUID that identifies the scheme.
        /// </summary>
        /// <returns>Returns a GUID of the active power scheme</returns>
        private static string GetCurrentPlanGUID()
        {
            PowerGetEffectiveOverlayScheme(out Guid currentMode);
            return currentMode.ToString();
        }

        /// <summary>
        /// Sets a power scheme in the system from the GUID provided
        /// </summary>
        /// <param name="guid">A PowerPlan guid</param>
        private void SetPowerPlan(Guid guid)
        {
            var process = new Process { StartInfo = _startInfo };
            process.Start();
            process.StandardInput.WriteLine("powercfg /SETACTIVE " + guid);
            process.StandardInput.WriteLine("exit");
            process.StandardOutput.ReadToEnd();
            process.Dispose();
        }

        /// <summary>
        /// Common StartInfo properties for a Process to hide the window
        /// </summary>
        private static readonly ProcessStartInfo _startInfo = new ProcessStartInfo("cmd")
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        /// <summary>
        /// Retrieves the active overlay power scheme and returns a GUID that identifies the scheme.
        /// </summary>
        /// <param name="EffectiveOverlayPolicyGuid">A pointer to a GUID structure.</param>
        /// <returns>Returns zero if the call was successful, and a nonzero value if the call failed.</returns>
        [DllImportAttribute("powrprof.dll", EntryPoint = "PowerGetEffectiveOverlayScheme")]
        public static extern uint PowerGetEffectiveOverlayScheme(out Guid EffectiveOverlayPolicyGuid);
    }
}
