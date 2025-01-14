using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.Screen.Services
{
	public class ScreenService : IDisposable
	{
		public event EventHandler<double>? BrightnessChanged;
		public bool IsBrightnessControlEnabled { get; } = false; // If brightness changing is possible via WMI

		private ManagementEventWatcher? eventWatcher;
		private bool IsInternalChange = false; // If brightness is changing due to ScreenService
		private ManagementObjectCollection? WMICollection;
		public ScreenService()
		{
			try
			{
				string query = "SELECT * FROM WmiMonitorBrightnessEvent";
				eventWatcher = new ManagementEventWatcher(new ManagementScope("root\\WMI"), new EventQuery(query));
				eventWatcher.EventArrived += OnBrightnessChanged;
				eventWatcher.Start();
				IsBrightnessControlEnabled = true;


				ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI", "SELECT InstanceName FROM WmiMonitorBrightnessMethods");
				WMICollection = searcher.Get();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Failed to start listening for brightness changes: {ex.Message}");
				IsBrightnessControlEnabled = false;
			}
		}

		private void OnBrightnessChanged(object sender, EventArrivedEventArgs e)
		{
			try
			{
				if (e.NewEvent["Brightness"] is byte newBrightness && !IsInternalChange)
					BrightnessChanged?.Invoke(this, newBrightness);

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error handling brightness change event: {ex.Message}");
			}
		}

		public double GetBrightness()
		{
			try
			{
				using (var searcher = new ManagementObjectSearcher("root\\WMI", "SELECT CurrentBrightness FROM WmiMonitorBrightness"))
				{
					foreach (ManagementObject obj in searcher.Get())
					{
						byte currentBrightness = (byte)obj["CurrentBrightness"];
						return currentBrightness;
					}
				}

				throw new InvalidOperationException("Unable to retrieve monitor brightness using WMI.");
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Error accessing brightness through WMI.", ex);
			}
		}

		public void SetBrightness(double brightness)
		{
			IsInternalChange = true;
			try
			{
				if (WMICollection is null) return;
				foreach (ManagementObject obj in WMICollection)
				{
					obj.InvokeMethod("WmiSetBrightness", new object[] { uint.MaxValue, (byte)(brightness) });
					IsInternalChange = false;
					return;
				}
				IsInternalChange = false;
				throw new InvalidOperationException("Unable to set monitor brightness using WMI.");
			}
			catch (Exception ex)
			{
				IsInternalChange = false;
				throw new InvalidOperationException("Error setting brightness through WMI.", ex);
			}
		}

		public void Dispose()
		{
			if (eventWatcher is not null)
			{
				eventWatcher.Stop();
				eventWatcher.EventArrived -= OnBrightnessChanged;
				eventWatcher.Dispose();
				eventWatcher = null;
			}
		}

		~ScreenService() => Dispose();
	}
}
