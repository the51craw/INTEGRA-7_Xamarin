using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;

namespace Integra7Random_Xamarin.UWP
{
    public class MidiDeviceWatcher
    {
        DeviceWatcher deviceWatcher;
        string deviceSelectorString;
        Picker deviceComboBox = null;
        CoreDispatcher coreDispatcher;
        public DeviceInformationCollection DeviceInformationCollection { get; set; }

        public MidiDeviceWatcher(string midiDeviceSelectorString, Picker midiDeviceComboBox, CoreDispatcher dispatcher)
        {
            deviceComboBox = midiDeviceComboBox;
            coreDispatcher = dispatcher;

            deviceSelectorString = midiDeviceSelectorString;

            deviceWatcher = DeviceInformation.CreateWatcher(deviceSelectorString);
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Removed += DeviceWatcher_Removed;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
        }

        ~MidiDeviceWatcher()
        {
            deviceWatcher.Added -= DeviceWatcher_Added;
            deviceWatcher.Removed -= DeviceWatcher_Removed;
            deviceWatcher.Updated -= DeviceWatcher_Updated;

            deviceWatcher = null;
        }

        public void SelectDevice(String DeviceName)
        {

        }

        private async void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            await coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Update the device list
                UpdateDevices();
            });
        }

        private async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            await coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Update the device list
                UpdateDevices();
            });
        }

        private async void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            await coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Update the device list
                UpdateDevices();
            });
        }

        private async void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            await coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Update the device list
                UpdateDevices();
            });
        }

        private async void UpdateDevices()
        {
            // Get a list of all MIDI devices
            this.DeviceInformationCollection = await DeviceInformation.FindAllAsync(deviceSelectorString);

            if (deviceComboBox != null)
            {
                deviceComboBox.Items.Clear();

                if (!this.DeviceInformationCollection.Any())
                {
                    deviceComboBox.Items.Add("No MIDI devices found!");
                }

                foreach (var deviceInformation in this.DeviceInformationCollection)
                {
                    deviceComboBox.Items.Add(deviceInformation.Name);
                }

                for (Int32 i = 0; i < deviceComboBox.Items.Count(); i++)
                {
                    if (((String)deviceComboBox.Items[i]).Contains("INTEGRA-7"))
                    {
                        deviceComboBox.SelectedIndex = i;
                    }
                }
                if (deviceComboBox.SelectedIndex < 0 && deviceComboBox.Items.Count() > 0)
                {
                    deviceComboBox.SelectedIndex = 0;
                }
            }
        }

        public void UpdateComboBox(Picker comboBox, Int32 selectedIndex)
        {
            try
            {
                deviceComboBox = comboBox;
                foreach (var deviceInformation in this.DeviceInformationCollection)
                {
                    deviceComboBox.Items.Add(deviceInformation.Name);
                }
                deviceComboBox.SelectedIndex = selectedIndex;
            }
            catch { }
        }

        public void StartWatcher()
        {
            deviceWatcher.Start();
        }

        public void StopWatcher()
        {
            deviceWatcher.Stop();
        }
    }
}
