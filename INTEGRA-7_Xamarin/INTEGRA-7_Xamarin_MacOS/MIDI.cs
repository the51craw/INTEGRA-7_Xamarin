using System;
using System.Collections.Generic;
//using System.Timers;
using Xamarin.Forms;
using CoreMidi;
using Integra7Random_Xamarin;
using Foundation;
using System.Threading;

namespace Integra7Random_Xamarin_MacOS
{
    public class MIDI : IMidi
    {
        public MidiPort midiOutPort;
        public MidiPort midiInPort;
        public MidiDevice midiDevice;
        public MidiEntity midiEntity;
        public MidiEndpoint midiOutEndpoint;
        MidiPacket midiPacket;
        unsafe byte *indataPointer;
        public byte MidiOutPortChannel { get; set; }
        public byte MidiInPortChannel { get; set; }
        public Int32 MidiOutPortSelectedIndex { get; set; }
        public Int32 MidiInPortSelectedIndex { get; set; }
        public Integra7Random_Xamarin.MainPage mainPage;
        public Integra7Random_Xamarin_MacOS.AppDelegate mainPage_MacOS;
        public byte[][] rawData;  /* Packets are coming in in small chunks, so
                                     I need to have two buffers for incoming
                                     hex data, one under filling, and one
                                     possibly filled with data to handle. */
        public Int32 validIndata; /* validIndata will point to the filled
                                     buffer if one exists, else -1 */
        public Int32 readIndex;   /* readindex is for reading the buffer
                                     currently holding a complete message */
        public Int32 incoming;    /* incoming will point to the current
                                     buffer ready to be filled or is currently
                                     being filled */
        public Int32 writeIndex;  /* writeindex is for writing to the buffer
                                     currently being filled */
        public Timer timer;
        public Boolean MessageReceived = false;
        CoreMidi.MidiClient midiClient = null;

        //public void IMidi()
        //{
        //    if (midiOutPort == null)
        //    {
        //        mainPage_MacOS = DependencyService.Get<Integra7Random_Xamarin_MacOS.AppDelegate>();
        //    }
        //}

        public MIDI()
        {
            if (midiOutPort == null)
            {
                mainPage_MacOS = DependencyService.Get<Integra7Random_Xamarin_MacOS.AppDelegate>();
            }
        }

        private void Timer_Tick(object sender)
        {
            if (MessageReceived)
            {       
                mainPage.uIHandler.rawData = rawData[validIndata];
                mainPage.uIHandler.MidiInPort_MessageRecceived();
                MessageReceived = false;
            }
        }

        /**
         * This sucker gets called for every few bytes received, not just once per message.
         * Thus it is neccesary not only to keep track of message starts and message endings,
         * but also to keep two buffers in order to let Timer_Tick consume one ready
         * buffer while the other might be filled out.
         * The received package in e is actually a list of packages, which seems odd 
         * when only a few bytes normally comes through, but that too must be handled.
         */
        internal unsafe void MidiInPort_MessageReceived(object sender, MidiPacketsEventArgs e)
        {
            // Loop incoming packages:
            for (Int32 packetsIndex = 0; packetsIndex < e.Packets.Length; packetsIndex++)
            {
                // Obtain an unsafe pointer for fetching the bytes:
                midiPacket = (MidiPacket)e.Packets.GetValue(packetsIndex);
                IntPtr ip = midiPacket.Bytes;
                indataPointer = (byte*)ip.ToPointer();

                // Set currentReceiveBuffer to point to the one in use:
                Int32 currentReceiveBuffer = 0;
                if (validIndata == 0)
                {
                    currentReceiveBuffer = 1;
                }

                // If the first byte is a command byte (MSB = 1) this can be:
                // A system real-time (single byte) message
                // Start of a channel voice or mode message (no end command!)
                // A system common message
                // Start of a system exclusive message
                // End of a system exclusive message
                // Thus causing for starting or ending a message or 
                // system common message with data or making a single byte message:
                byte commandByte = indataPointer[0];
                byte commandNibble = (byte)(commandByte & 0xf0);
                if (commandByte == 0xf7)
                {
                    // This is an end of system exclusive that happened to 
                    // come in as a last byte. It must be added to the
                    // current message, so do nothing yet.
                }
                else if ((commandNibble >= 0x80 && commandNibble <= 0xe0) ||
                    (commandByte >= 0xf1 && commandByte <= 0xfe))
                {
                    // This is a channel voice or mode message (no end command!)
                    // If there is a current message in currentReceiveBuffer it
                    // must be handled in Timer_Tick:
                    if (rawData[currentReceiveBuffer].Length > 0)
                    {
                        validIndata = currentReceiveBuffer;
                        currentReceiveBuffer++;
                        currentReceiveBuffer = currentReceiveBuffer % 2;
                        rawData[currentReceiveBuffer] = new byte[0];
                        MessageReceived = true;
                    }
                }


                // Back up whatever might have been read in earlier call:
                byte[] tempBuffer = new byte[rawData[currentReceiveBuffer].Length];
                for (Int32 i = 0; i < rawData[currentReceiveBuffer].Length; i++)
                {
                    tempBuffer[i] = rawData[currentReceiveBuffer][i];
                }

                // Remember how long that was in order to know where to continue:
                Int32 previousLength = rawData[currentReceiveBuffer].Length;

                //Re-create the current buffer with extended length:
                rawData[currentReceiveBuffer] = new byte[tempBuffer.Length + midiPacket.Length];

                // Copy back what was stored in backup:
                //tempBuffer.CopyTo()
                for (Int32 i = 0; i < tempBuffer.Length; i++)
                {
                    rawData[currentReceiveBuffer][i] = tempBuffer[i];
                }

                // Concatenate the new bytes:
                for (Int32 i = 0; i < e.Packets[packetsIndex].Length; i++)
                {
                    rawData[currentReceiveBuffer][i + previousLength] = indataPointer[i];
                }

                // See if we now have a complete message:
                commandByte = rawData[currentReceiveBuffer][0];
                commandNibble = (byte)(commandByte & 0xf0);
                if ((rawData[currentReceiveBuffer][rawData[currentReceiveBuffer].
                       Length - 1] == 0xf7) // End of system exclusive
                    || (commandNibble >= 0x80 && commandNibble <= 0xb0)
                        && (rawData[currentReceiveBuffer].
                            Length - 1 == 3) // 3-byte message
                    || (commandNibble >= 0xc0 && commandNibble <= 0xe0
                        && rawData[currentReceiveBuffer].
                            Length - 1 == 2) // 2-byte message
                    || ((commandByte == 0xf1 || commandByte == 0xf3)
                        && rawData[currentReceiveBuffer].
                            Length - 1 == 2) // 2-byte message
                    || ((commandByte == 0xf4)
                        && rawData[currentReceiveBuffer].
                        Length - 1 == 1) // 1-byte message
                   )
                {
                    validIndata = currentReceiveBuffer;
                    currentReceiveBuffer++;
                    currentReceiveBuffer = currentReceiveBuffer % 2;
                    rawData[currentReceiveBuffer] = new byte[0];
                    MessageReceived = true;
                }
            }
        }

        // Simpleconstructor that takes the name of the device:
        public MIDI(String deviceName)
        {
            Init(deviceName);
        }

        public void Init(Integra7Random_Xamarin.MainPage mainPage, String deviceName, Picker OutputDeviceSelector, Picker InputDeviceSelector, object Dispatcher, byte MidiOutPortChannel, byte MidiInPortChannel)
        {
            this.mainPage = mainPage;
            Init(deviceName);
        }

        public void Init(String deviceName)
        {
            MidiDevice md;
            Midi.Restart();
            if (midiClient == null)
            {
                midiClient = new MidiClient("The MidiClient");
                midiClient.ObjectAdded += delegate (object sender, ObjectAddedOrRemovedEventArgs e)
                {
                    Int32 handle = ((MidiClient)sender).Handle;
                    IntPtr ip = new IntPtr(handle); 
                };
                midiClient.ObjectRemoved += delegate (object sender, ObjectAddedOrRemovedEventArgs e)
                {
                    //Console.WriteLine("Object {0} removed to {1}", e.Child, e.Parent);
                };
                midiClient.PropertyChanged += delegate (object sender, ObjectPropertyChangedEventArgs e)
                {
                    //Console.WriteLine("Property {0} changed on {1}", e.PropertyName, e.MidiObject);
                };
                midiClient.ThruConnectionsChanged += delegate
                {
                    //Console.WriteLine("Thru connections changed");
                };
                midiClient.SerialPortOwnerChanged += delegate
                {
                    //Console.WriteLine("Serial port changed");
                };
            }
            midiOutPort = midiClient.CreateOutputPort("MIDI Out Port");
            midiInPort = midiClient.CreateInputPort("MIDI In Port");
            midiInPort.MessageReceived += MidiInPort_MessageReceived;

            for (Int32 deviceIndex = 0; deviceIndex < Midi.DeviceCount; deviceIndex++)
            {
                md = Midi.GetDevice(deviceIndex);
                if (md.EntityCount > 0)
                {
                    for (Int32 e = 0; e < md.EntityCount; e++)
                    {
                        MidiEntity me = md.GetEntity(e);
                        if (me.Sources > 0)
                        {
                            mainPage.uIHandler.Librarian_midiInputDevice.Items.Add(md.Name);
                        }
                        if (me.Destinations > 0)
                        {
                            mainPage.uIHandler.Librarian_midiOutputDevice.Items.Add(md.Name);
                        }
                        if (md.Name != null && md.Name.Contains("INTEGRA-7"))
                        {
                            midiDevice = md;
                            midiEntity = md.GetEntity(e);
                            midiInPort.ConnectSource(midiEntity.GetSource(0));
                            midiOutPort.ConnectSource(midiEntity.GetDestination(0));
                            midiOutEndpoint = me.GetDestination(0);
                            mainPage.uIHandler.Librarian_midiOutputDevice.SelectedIndex = deviceIndex;
                            mainPage.uIHandler.Librarian_midiInputDevice.SelectedIndex = deviceIndex;
                        }
                    }
                }
            }
            rawData = new byte[2][];
            rawData[0] = new byte[0];
            rawData[1] = new byte[0];
            validIndata = -1;
            readIndex = 0;
            writeIndex = 0;
            timer = new Timer(Timer_Tick, MessageReceived, 1, 1);
        }

        public Boolean MidiIsReady()
        {
            return true; // Until we know how to verify it.
        }

        public void OutputDeviceChanged(Picker DeviceSelector)
        {
            // In MacOS this is handled above, in midiClient.ObjectRemoved and ObjectAdded.
            // The windows version needs this function, so it is in the interface, thus
            // must be present even if not used.
        }

        public void InputDeviceChanged(Picker DeviceSelector)
        {
            // In MacOS this is handled above, in midiClient.ObjectRemoved and ObjectAdded
            // The windows version needs this function, so it is in the interface, thus
            // must be present even if not used.
        }

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

        public void NoteOn(byte currentChannel, byte noteNumber, byte velocity)
        {
            Byte[] bytes = new Byte[] { (byte)(0x90 | currentChannel), noteNumber, velocity };
            SendPacket(bytes);
        }

        public void NoteOff(byte currentChannel, byte noteNumber)
        {
            Byte[] bytes = new Byte[] { (byte)(0x80 | currentChannel), noteNumber, 0x00 };
            SendPacket(bytes);
        }

        public void SendControlChange(byte channel, byte controller, byte value)
        {
            Byte[] bytes = new Byte[] { (byte)(0xb0 | channel), controller, value };
            SendPacket(bytes);
        }

        public void SetVolume(byte channel, byte volume)
        {
            SendControlChange(channel, 0x07, volume);
        }

        public void ProgramChange(byte channel, String smsb, String slsb, String spc)
        {
            Byte[] bytes = new Byte[] { (byte)(0xb0 | channel), 0x00, (byte)(UInt16.Parse(smsb))};
            SendPacket(bytes);
            bytes = new Byte[] { (byte)(0xb0 | channel), 0x20, (byte)(UInt16.Parse(slsb))};
            SendPacket(bytes);
            bytes = new Byte[] { (byte)(0xc0 | channel), (byte)(UInt16.Parse(spc) - 1) };
            SendPacket(bytes);
        }

        public void AllNotesOff(byte channel)
        {
            SendControlChange(channel, 0x78, 0);
        }

        public void ProgramChange(byte channel, byte msb, byte lsb, byte pc)
        {
            Byte[] bytes = new Byte[] { (byte)(0xb0 | channel), 0x00, msb };
            SendPacket(bytes);
            bytes = new Byte[] { (byte)(0xb0 | channel), 0x20, lsb };
            SendPacket(bytes);
            bytes = new Byte[] { (byte)(0xc0 | channel), (byte)(pc - 1) };
            SendPacket(bytes);
        }

        private void SendPacket(byte[] bytes)
        {
            if (midiOutPort != null)
            {
                MidiPacket[] mp = new MidiPacket[1];
                mp[0] = new MidiPacket(0, bytes);
                midiOutPort.Send(midiOutEndpoint, mp);
                mp[0].Dispose();
            }
        }

        public void SendSystemExclusive(byte[] bytes)
        {
                SendPacket(bytes);
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
                    return (byte)(chars.IndexOf(s1, 
                        StringComparison.CurrentCulture) * 16 +
                        chars.IndexOf(s2, StringComparison.CurrentCulture));
            }

        }
    }
}
