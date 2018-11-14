//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

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
//using Windows.Devices.Enumeration;
//using Windows.UI.Core;

namespace Integra_7_Xamarin.UWP
{
    /// <summary>
    /// DeviceWatcher class to monitor adding/removing MIDI devices on the fly
    /// </summary>
    public class MidiDeviceWatcher
    {
        DeviceWatcher deviceWatcher;
        string deviceSelectorString;
        Picker deviceComboBox = null;
        CoreDispatcher coreDispatcher;
        public DeviceInformationCollection DeviceInformationCollection { get; set; }

        /// <summary>
        /// Constructor: Initialize and hook up Device Watcher events
        /// </summary>
        /// <param name="midiSelectorString">MIDI Device Selector</param>
        /// <param name="dispatcher">CoreDispatcher instance, to update UI thread</param>
        /// <param name="portListBox">The UI element to update with list of devices</param>
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

        /// <summary>
        /// Destructor: Remove Device Watcher events
        /// </summary>
        ~MidiDeviceWatcher()
        {
            deviceWatcher.Added -= DeviceWatcher_Added;
            deviceWatcher.Removed -= DeviceWatcher_Removed;
            deviceWatcher.Updated -= DeviceWatcher_Updated;
            deviceWatcher.EnumerationCompleted -= DeviceWatcher_EnumerationCompleted;
            deviceWatcher = null;
        }

        public void SelectDevice(String DeviceName)
        {

        }

        /// <summary>
        /// Update UI on device removed
        /// </summary>
        /// <param name="sender">The active DeviceWatcher instance</param>
        /// <param name="args">Event arguments</param>
        private async void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            await coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Update the device list
                UpdateDevices();
            });
        }

        /// <summary>
        /// Update UI on device added
        /// </summary>
        /// <param name="sender">The active DeviceWatcher instance</param>
        /// <param name="args">Event arguments</param>
        private async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            await coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Update the device list
                UpdateDevices();
            });
        }

        /// <summary>
        /// Update UI on device enumeration completed.
        /// </summary>
        /// <param name="sender">The active DeviceWatcher instance</param>
        /// <param name="args">Event arguments</param>
        private async void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            await coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Update the device list
                UpdateDevices();
            });
        }

        /// <summary>
        /// Update UI on device updated
        /// </summary>
        /// <param name="sender">The active DeviceWatcher instance</param>
        /// <param name="args">Event arguments</param>
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

        /// <summary>
        /// Add any connected MIDI devices to the list
        /// </summary>
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

        /// <summary>
        /// Start the Device Watcher
        /// </summary>
        public void StartWatcher()
        {
            deviceWatcher.Start();
        }

        /// <summary>
        /// Stop the Device Watcher
        /// </summary>
        public void StopWatcher()
        {
            deviceWatcher.Stop();
        }
    }
}
