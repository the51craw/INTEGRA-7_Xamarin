using System;
using System.Linq;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Xamarin.Forms;
using INTEGRA_7_Xamarin.UWP;
using System.Collections.Generic;

[assembly: Dependency(typeof(MIDI))]

namespace INTEGRA_7_Xamarin.UWP
{
    public class MIDI : IMidi
    {
        public MidiOutPort midiOutPort;
        public MidiInPort midiInPort;
        public byte MidiOutPortChannel { get; set; }
        public byte MidiInPortChannel { get; set; }
        public Int32 MidiOutPortSelectedIndex { get; set; }
        public Int32 MidiInPortSelectedIndex { get; set; }
        public INTEGRA_7_Xamarin.MainPage mainPage { get; set; }
        public INTEGRA_7_Xamarin.UWP.MainPage MainPage_UWP { get; set; }
        public byte[] rawData;
        //public DispatcherTimer timer;
        public Boolean MessageReceived = false;
        public Boolean VenderDriverPresent = false;
        Picker OutputDeviceSelector { get; set; }
        Picker InputDeviceSelector { get; set; }
        public List<String> MidiDevices { get; set; }

        public MIDI()
        {
            MidiDevices = new List<String>();
        }

        //private void Timer_Tick(object sender, object e)
        //{
        //    if (MessageReceived)
        //    {
        //        // Alert mainPage
        //        MessageReceived = false;
        //        mainPage.uIHandler.rawData = rawData;
        //        mainPage.uIHandler.MidiInPort_MessageRecceived();
        //    }
        //}

        private void StartTimer()
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
                Device.StartTimer(TimeSpan.FromMilliseconds(1), () =>
                {
                    if (UIHandler.StopTimer)
                    {
                        return false;
                    }
                    else
                    {
                        if (MessageReceived)
                        {
                            // Alert mainPage
                            MessageReceived = false;
                            mainPage.uIHandler.rawData = rawData;
                            mainPage.uIHandler.MidiInPort_MessageRecceived();
                        }
                        return true;
                    }
                });
            //});
        }

        public void MidiInPort_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args)
        {
            IMidiMessage receivedMidiMessage = args.Message;
            byte[] temp = receivedMidiMessage.RawData.ToArray();

            // Just skip the keep-alive messages:
            if (temp.Length == 1 && temp[0] == 0xfe)
            {
                return;
            }

            rawData = receivedMidiMessage.RawData.ToArray();
            MessageReceived = true;
        }

        public async Task Init(INTEGRA_7_Xamarin.MainPage mainPage, string deviceName, object DeviceSpecificObject, byte MidiOutPortChannel, byte MidiInPortChannel)
        {
            MidiDevices = new List<String>();
            await Init(mainPage, "INTEGRA-7", MidiOutPortChannel, MidiInPortChannel);
        }

        public MIDI(INTEGRA_7_Xamarin.MainPage mainPage, 
            byte MidiOutPortChannel, byte MidiInPortChannel)
        {
            MidiDevices = new List<String>();
            //Init(mainPage, "INTEGRA-7", MidiOutPortChannel, MidiInPortChannel);
        }

        ~MIDI()
        {
            try
            {
                midiOutPort.Dispose();
                midiInPort.MessageReceived -= MidiInPort_MessageReceived;
                midiInPort.Dispose();
                midiOutPort = null;
                midiInPort = null;
            } catch { }
        }

        public async Task Init(INTEGRA_7_Xamarin.MainPage mainPage, String deviceName, byte MidiOutPortChannel, byte MidiInPortChannel)
        {
            if (deviceName == null)
            {
                return;
            }

            this.mainPage = mainPage;
            this.MidiOutPortChannel = MidiOutPortChannel;
            this.MidiInPortChannel = MidiInPortChannel;

            // If the user has connected the I-7 via 5-pin connections, we cannot easily identify it. Therefore, we fill out
            // the selectors and let the user selects the correct one. We set the selector to first line.
            // If the user has connected the I-7 via USB, we will still populate the selectors, and we will
            // have the name in there, so we can select it.
            if (midiOutPort == null)
            {
                DeviceInformationCollection midiOutputDevices = await DeviceInformation.FindAllAsync(MidiOutPort.GetDeviceSelector());
                DeviceInformation midiOutDevInfo = null;

                foreach (DeviceInformation device in midiOutputDevices)
                {
                    if (device.Name.Contains(deviceName))
                    {
                        midiOutDevInfo = device;
                        break;
                    }
                }

                if (midiOutDevInfo != null)
                {
                    midiOutPort = (MidiOutPort)await MidiOutPort.FromIdAsync(midiOutDevInfo.Id);
                }
            }

            if (midiInPort == null)
            {
                DeviceInformationCollection midiInputDevices = await DeviceInformation.FindAllAsync(MidiInPort.GetDeviceSelector());
                DeviceInformation midiInDevInfo = null;

                foreach (DeviceInformation device in midiInputDevices)
                {
                    if (device.Name.Contains(deviceName))
                    {
                        midiInDevInfo = device;
                        break;
                    }
                }

                if (midiInDevInfo != null)
                {
                    midiInPort = (MidiInPort)await MidiInPort.FromIdAsync(midiInDevInfo.Id);
                }
            }

            if (midiOutPort == null)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create MidiOutPort from output device");
            }

            if (midiInPort == null)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create MidiInPort from input device");
            }
            else
            {
                midiInPort.MessageReceived += MidiInPort_MessageReceived;
                StartTimer();
            }
        }

        public void ResetMidi()
        {
            try
            {
                if (midiInPort != null)
                {
                    midiInPort.MessageReceived -= MidiInPort_MessageReceived;
                }
            } catch { }
            try
            {
                if (midiOutPort != null)
                {
                    midiOutPort.Dispose();
                }
            } catch { }
            try
            {
                if (midiInPort != null)
                {
                    midiInPort.Dispose();
                }
            }
            catch { }
            try
            {
                if (midiOutPort != null)
                {
                    midiOutPort = null;
                }
            } catch { }
            try
            {
                if (midiInPort != null)
                {
                    midiInPort = null;
                }
            } catch { }
        }


        public Boolean MidiIsReady()
        {
            return midiInPort != null && midiOutPort != null;
        }

        public async Task CheckForVenderDriver()
        {
            DeviceInformationCollection collection = await DeviceInformation.FindAllAsync();
            foreach (DeviceInformation dev in collection)
            {
                if (dev.Name.ToLower().Contains("integra")
                    && dev.Id.ToLower().Contains("wave")
                    && dev.IsEnabled)
                {
                    VenderDriverPresent = true;
                    break;
                }
            }
        }

        public Boolean VenderDriverDetected()
        {
            return VenderDriverPresent;
        }

        public void UpdateMidiComboBoxes(Picker midiOutputComboBox, Picker midiInputComboBox)
        {
        }

        //public async void OutputDeviceChanged(Picker DeviceSelector)
        //{
        //    try
        //    {
        //        if (!String.IsNullOrEmpty((String)DeviceSelector.SelectedItem))
        //        {
        //            var midiOutDeviceInformationCollection = await DeviceInformation.FindAllAsync(MidiOutPort.GetDeviceSelector());

        //            if (midiOutDeviceInformationCollection == null)
        //            {
        //                return;
        //            }

        //            DeviceInformation midiOutDevInfo = midiOutDeviceInformationCollection[DeviceSelector.SelectedIndex];

        //            if (midiOutDevInfo == null)
        //            {
        //                return;
        //            }

        //            midiOutPort = (MidiOutPort)await MidiOutPort.FromIdAsync(midiOutDevInfo.Id);

        //            if (midiOutPort == null)
        //            {
        //                System.Diagnostics.Debug.WriteLine("Unable to create MidiOutPort from output device");
        //                return;
        //            }
        //        }
        //    }
        //    catch { }
        //}

        //public async void InputDeviceChanged(Picker DeviceSelector)
        //{
        //    try
        //    {
        //        if (!String.IsNullOrEmpty((String)DeviceSelector.SelectedItem))
        //        {
        //            var midiInDeviceInformationCollection = await DeviceInformation.FindAllAsync(MidiInPort.GetDeviceSelector());

        //            if (midiInDeviceInformationCollection == null)
        //            {
        //                return;
        //            }

        //            DeviceInformation midiInDevInfo = midiInDeviceInformationCollection[DeviceSelector.SelectedIndex];

        //            if (midiInDevInfo == null)
        //            {
        //                return;
        //            }

        //            midiInPort = await MidiInPort.FromIdAsync(midiInDevInfo.Id);

        //            if (midiInPort == null)
        //            {
        //                System.Diagnostics.Debug.WriteLine("Unable to create MidiInPort from input device");
        //                return;
        //            }
        //            midiInPort.MessageReceived += MidiInPort_MessageReceived;
        //        }
        //    }
        //    catch { }
        //}

        public byte GetMidiOutPortChannel()
        {
            return MidiOutPortChannel;
        }

        public void SetMidiOutPortChannel(byte OutPortChannel)
        {
            MidiOutPortChannel = OutPortChannel;
        }

        public byte GetMidiInPortChannel()
        {
            return MidiInPortChannel;
        }

        public void SetMidiInPortChannel(byte InPortChannel)
        {
            MidiInPortChannel = InPortChannel;
        }

        public List<String> GetMidiDeviceList()
        {
            return MidiDevices;
        }

        public async Task MakeMidiDeviceList()
        {
            DeviceInformationCollection midiInputDevices = await DeviceInformation.FindAllAsync(MidiInPort.GetDeviceSelector());
            MidiDevices.Clear();
            foreach (DeviceInformation device in midiInputDevices)
            {
                if (device.Name.Contains("["))
                {
                    MidiDevices.Add(device.Name.Remove(device.Name.IndexOf('[')));
                }
                else
                {
                    MidiDevices.Add(device.Name);
                }
            }
        }

        public void NoteOn(byte currentChannel, byte noteNumber, byte velocity)
        {
            if (midiOutPort != null)
            {
                IMidiMessage midiMessageToSend = new MidiNoteOnMessage(currentChannel, noteNumber, velocity);
                midiOutPort.SendMessage(midiMessageToSend);
            }
        }

        public void NoteOff(byte currentChannel, byte noteNumber)
        {
            if (midiOutPort != null)
            {
                IMidiMessage midiMessageToSend = new MidiNoteOnMessage(currentChannel, noteNumber, 0);
                midiOutPort.SendMessage(midiMessageToSend);
            }
        }

        public void SendControlChange(byte channel, byte controller, byte value)
        {
            if (midiOutPort != null)
            {
                IMidiMessage midiMessageToSend = new MidiControlChangeMessage(channel, controller, value);
                midiOutPort.SendMessage(midiMessageToSend);
            }
        }

        public void SetVolume(byte currentChannel, byte volume)
        {
            if (midiOutPort != null)
            {
                IMidiMessage midiMessageToSend = new MidiControlChangeMessage(currentChannel, 0x07, volume);
                midiOutPort.SendMessage(midiMessageToSend);
            }
        }

        public void AllNotesOff(byte currentChannel)
        {
            if (midiOutPort != null)
            {
                IMidiMessage midiMessageToSend = new MidiControlChangeMessage(currentChannel, 0x78, 0);
                midiOutPort.SendMessage(midiMessageToSend);
            }
        }

        public void ProgramChange(byte currentChannel, String smsb, String slsb, String spc)
        {
            try
            {
                MidiControlChangeMessage controlChangeMsb = new MidiControlChangeMessage(currentChannel, 0x00, (byte)(UInt16.Parse(smsb)));
                MidiControlChangeMessage controlChangeLsb = new MidiControlChangeMessage(currentChannel, 0x20, (byte)(UInt16.Parse(slsb)));
                MidiProgramChangeMessage programChange = new MidiProgramChangeMessage(currentChannel, (byte)(UInt16.Parse(spc) - 1));
                midiOutPort.SendMessage(controlChangeMsb);
                midiOutPort.SendMessage(controlChangeLsb);
                midiOutPort.SendMessage(programChange);
            }
            catch { }
        }

        public void ProgramChange(byte currentChannel, byte msb, byte lsb, byte pc)
        {
            try
            {
                MidiControlChangeMessage controlChangeMsb = new MidiControlChangeMessage(currentChannel, 0x00, msb);
                MidiControlChangeMessage controlChangeLsb = new MidiControlChangeMessage(currentChannel, 0x20, lsb);
                MidiProgramChangeMessage programChange = new MidiProgramChangeMessage(currentChannel, (byte)(pc - 1));
                midiOutPort.SendMessage(controlChangeMsb);
                midiOutPort.SendMessage(controlChangeLsb);
                midiOutPort.SendMessage(programChange);
            }
            catch { }
        }

        public void SendSystemExclusive(byte[] bytes)
        {
            try
            {
                IBuffer buffer = bytes.AsBuffer();
                MidiSystemExclusiveMessage midiMessageToSend = new MidiSystemExclusiveMessage(buffer);
                midiOutPort.SendMessage(midiMessageToSend);
            }
            catch (Exception e)
            {
                // TODO: Add trace reporting!!!
            }
        }

        public byte[] SystemExclusiveRQ1Message(byte[] Address, byte[] Length)
        {
            byte[] result = new byte[17];
            result[0] = 0xf0; // Start of exclusive message
            result[1] = 0x41; // Roland
            result[2] = 0x10; // Device Id is 17 according to settings in INTEGRA-7 (Menu -> System -> MIDI, 1 = 0x00 ... 17 = 0x10)
            result[3] = 0x00;
            result[4] = 0x00;
            result[5] = 0x64; // INTEGRA-7
            result[6] = 0x11; // Command (DT1)
            result[7] = Address[0];
            result[8] = Address[1];
            result[9] = Address[2];
            result[10] = Address[3];
            result[11] = Length[0];
            result[12] = Length[1];
            result[13] = Length[2];
            result[14] = Length[3];
            result[15] = 0x00; // Filled out by CheckSum but present here to avoid confusion about index 15 missing.
            result[16] = 0xf7; // End of sysex
            CheckSum(ref result);
            return (result);
        }

        public byte[] SystemExclusiveDT1Message(byte[] Address, byte[] DataToTransmit)
        {
            Int32 length = 13 + DataToTransmit.Length;
            byte[] result = new byte[length]; 
            result[0] = 0xf0; // Start of exclusive message
            result[1] = 0x41; // Roland
            result[2] = 0x10; // Device Id is 17 according to settings in INTEGRA-7 (Menu -> System -> MIDI, 1 = 0x00 ... 17 = 0x10)
            result[3] = 0x00;
            result[4] = 0x00;
            result[5] = 0x64; // INTEGRA-7
            result[6] = 0x12; // Command (DT1)
            result[7] = Address[0];
            result[8] = Address[1];
            result[9] = Address[2];
            result[10] = Address[3];
            for (Int32 i = 0; i < DataToTransmit.Length; i++)
            {
                result[i + 11] = DataToTransmit[i];
            }
            result[12 + DataToTransmit.Length] = 0xf7; // End of sysex
            CheckSum(ref result);
            return (result);
        }

        public void CheckSum(ref byte[] bytes)
        {
            byte chksum = 0;
            for (Int32 i = 7; i < bytes.Length - 2; i++)
            {
                chksum += bytes[i];
            }
            bytes[bytes.Length - 2] = (byte)((0x80 - (chksum & 0x7f)) & 0x7f);
        }
        private String ToHex(byte data)
        {
            String[] chars = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
            byte msb = (byte)((data & 0xf0) >> 4);
            byte lsb = (byte)(data & 0x0f);
            return "0x" + chars[msb] + chars[lsb] + " ";
        }

        private byte StringToHex(String s)
        {
            String chars = "0123456789abcdef";
            if (s.Length != 2)
            {
                return 0xff;
            }
            else
            {
                s = s.ToLower();
                String s1 = s.Remove(1);
                String s2 = s.Remove(0, 1);
                if (!chars.Contains(s1) || !chars.Contains(s2))
                {
                    return 0xff;
                }
                return (byte)(chars.IndexOf(s1) * 16 + chars.IndexOf(s2));
            }

        }
    }
}
