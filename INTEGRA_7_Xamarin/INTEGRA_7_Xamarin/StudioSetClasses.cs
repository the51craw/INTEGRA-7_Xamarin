using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
//using Windows.Storage;
//using Windows.Storage.Pickers;
using System.Runtime.Serialization;
using System.Xml;
using System.Threading.Tasks;

namespace Integra_7_Xamarin
{
    [DataContract]
    public class StudioSet
    {
        //[IgnoreDataMember]
        //HBTrace t = new HBTrace("class StudioSet");
        //[DataMember]
        //public StudioSet_Setup Setup { get; set; }
        [DataMember]
        public StudioSet_SystemCommon SystemCommon { get; set; }
        [DataMember]
        public StudioSet_Common Common { get; set; }
        [DataMember]
        public StudioSet_CommonChorus CommonChorus { get; set; }
        [DataMember]
        public StudioSet_CommonReverb CommonReverb { get; set; }
        [DataMember]
        public StudioSet_MotionalSurround MotionalSurround { get; set; }
        [DataMember]
        public StudioSet_MasterEQ MasterEQ { get; set; }
        [DataMember]
        public StudioSet_PartMainSettings[] PartMainSettings { get; set; }
        [DataMember]
        public StudioSet_PartKeyboard[] PartKeyboard { get; set; }
        [DataMember]
        public StudioSet_PartScaleTune[] PartScaleTune { get; set; }
        [DataMember]
        public StudioSet_PartMidi[] PartMidi { get; set; }
        [DataMember]
        public StudioSet_PartMotionalSurround[] PartMotionalSurround { get; set; }
        [DataMember]
        public StudioSet_PartEQ[] PartEQ { get; set; }

        public StudioSet()
        {
            //t.Trace("public StudioSet()");
            // This will populate chorus type classes with initial data:
            //CommonChorus = new StudioSet_CommonChorus();
            PartMainSettings = new StudioSet_PartMainSettings[16];
            PartKeyboard = new StudioSet_PartKeyboard[16];
            PartScaleTune = new StudioSet_PartScaleTune[16];
            PartMidi = new StudioSet_PartMidi[16];
            PartMotionalSurround = new StudioSet_PartMotionalSurround[16];
            PartEQ = new StudioSet_PartEQ[16];
        }

        public static String[] NoteString = new String[] {
            "1/64T", "1/64", "1/32T", "1/32", "1/16T", "1/32.", "1/16", "1/8T", "1/16.",
            "1/8T", "1/16.", "1/8", "1/4T", "1/8.", "1/4", "1/2T", "1/4.", "1/2", "1/1T",
            "1/2.", "1/1", "2/1T", "1/1.", "2/1" };

        public static async Task<StudioSet> Deserialize<StudioSet>(StudioSet studioSet)
        {
            //try
            //{
            //    FileOpenPicker openPicker = new FileOpenPicker();
            //    openPicker.ViewMode = PickerViewMode.Thumbnail;
            //    openPicker.FileTypeFilter.Add(".xml");
            //    StorageFile openStudioSetFile = await openPicker.PickSingleFileAsync();

            //    if (openStudioSetFile != null)
            //    {
            //        Stream stream = await openStudioSetFile.OpenStreamForReadAsync();
            //        await Task.Run(() =>
            //        {
            //            MemoryStream memoryStream = new MemoryStream();
            //            stream.CopyTo(memoryStream);
            //            memoryStream.Position = 0;
            //            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(StudioSet));
            //            XmlReader xmlReader = XmlReader.Create(memoryStream);
            //            XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(xmlReader);
            //            studioSet = (StudioSet)dataContractSerializer.ReadObject(xmlDictionaryReader);
            //            xmlReader.Dispose();
            //        });
            //    }
            //}
            //catch (Exception e) { }
            return studioSet;
        }

        public static async Task Serialize<StudioSet>(StudioSet studioSet)
        {
            //try
            //{
            //    FileSavePicker savePicker = new FileSavePicker();
            //    savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".xml" });
            //    StorageFile saveStudioSetFile = await savePicker.PickSaveFileAsync();
            //    if (saveStudioSetFile != null)
            //    {
            //        //await saveStudioSetFile. DeleteAsync(StorageDeleteOption.Default);
            //        CachedFileManager.DeferUpdates(saveStudioSetFile);
            //        Stream fileStream = await saveStudioSetFile.OpenStreamForWriteAsync();
            //        fileStream.SetLength(0);
            //        await Task.Run(() =>
            //        {
            //            MemoryStream memoryStream = new MemoryStream();
            //            XmlWriter xmlWriter = XmlWriter.Create(memoryStream);
            //            using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(xmlWriter))
            //            {
            //                var dataContractSerializer = new DataContractSerializer(typeof(StudioSet));
            //                dataContractSerializer.WriteObject(memoryStream, studioSet);
            //                fileStream.Seek(0, SeekOrigin.Begin);
            //                ArraySegment<byte> data = new ArraySegment<byte>();
            //                memoryStream.TryGetBuffer(out data);
            //                xmlWriter.Flush();
            //                fileStream.Write(data.ToArray(), 0, (Int32)data.Count);
            //                fileStream.Flush();
            //                xmlDictionaryWriter.Dispose();
            //            }
            //            xmlWriter.Dispose();
            //            memoryStream.Dispose();
            //        });
            //        fileStream.Dispose();
            //        String xmlText = await FileIO.ReadTextAsync(saveStudioSetFile);
            //        xmlText = xmlText.Replace("><", ">\n<");
            //        String[] lines = xmlText.Split('\n');
            //        String indent = "";
            //        xmlText = "";
            //        Boolean firstLine = true;
            //        foreach (String line in lines)
            //        {
            //            if (firstLine)
            //            {
            //                xmlText = line + "\r\n";
            //                firstLine = false;
            //            }
            //            else if (line.StartsWith("</"))
            //            {
            //                if (indent.Length > 0)
            //                {
            //                    indent = indent.Remove(0, 1);
            //                }
            //                xmlText += indent + line + "\r\n";
            //            }
            //            else if (line.Contains("</"))
            //            {
            //                xmlText += indent + line + "\r\n";
            //            }
            //            else
            //            {
            //                xmlText += indent + line + "\r\n";
            //                indent += "\t";
            //            }
            //        }
            //        await FileIO.WriteTextAsync(saveStudioSetFile, xmlText);
            //    }
            //}
            //catch (Exception e) { }
        }
    }

    //[DataContract]
    //class StudioSet_Setup
    //{
    //    [IgnoreDataMember]
    //    HBTrace t = new HBTrace("class StudioSet_Setup");
    //    [DataMember]
    //    public byte SoundMode { get; set; }
    //    [DataMember]
    //    public Int32 Program { get; set; } // 3-byte, MSB, LSB and PC, but PC does not seem to be in use? Selecting a studio set works vith MSB and LSB.
    //}

    [DataContract]
    public class StudioSet_SystemCommon
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class StudioSet_SystemCommon");
        [DataMember]
        public Int32 MasterTune { get; set; } // 4 byte nibble coded in raw data
        [DataMember]
        public byte MasterKeyShift { get; set; }
        [DataMember]
        public byte MasterLevel { get; set; }
        [DataMember]
        public Boolean ScaleTuneSwitch { get; set; }
        [DataMember]
        public byte StudioSetControlChannel { get; set; }
        [DataMember]
        public byte SystemControl1Source { get; set; }
        [DataMember]
        public byte SystemControl2Source { get; set; }
        [DataMember]
        public byte SystemControl3Source { get; set; }
        [DataMember]
        public byte SystemControl4Source { get; set; }
        [DataMember]
        public byte ControlSource { get; set; }
        [DataMember]
        public byte SystemClockSource { get; set; }
        [DataMember]
        public Int16 SystemTempo { get; set; } // 2 byte nibble coded in raw data
        [DataMember]
        public byte TempoAssignSource { get; set; }
        [DataMember]
        public Boolean ReceiveProgramChange { get; set; }
        [DataMember]
        public Boolean ReceiveBankSelect { get; set; }
        [DataMember]
        public Boolean SurroundCenterSpeakerSwitch { get; set; }
        [DataMember]
        public Boolean SurroundSubWooferSwitch { get; set; }
        [DataMember]
        public byte StereoOutputMode { get; set; }

        public StudioSet_SystemCommon(ReceivedData Data)
        {
            //t.Trace("public StudioSet_SystemCommon (" + "ReceivedData" + Data + ", " + ")");
            MasterTune = (256 * Data.GetByte(0x01) + 16 * Data.GetByte(0x02) + Data.GetByte(0x03)) - 1024;
            MasterKeyShift = (byte)(Data.GetByte(0x04) - 64);
            MasterLevel = Data.GetByte(0x05);
            ScaleTuneSwitch = !(Data.GetByte(0x06) == 0x00);
            StudioSetControlChannel = Data.GetByte(0x11);
            SystemControl1Source = Data.GetByte(0x20);
            if (SystemControl1Source == 32) SystemControl1Source--; // CC32 is not allowed!
            SystemControl2Source = Data.GetByte(0x21);
            if (SystemControl2Source == 32) SystemControl2Source--; // CC32 is not allowed!
            SystemControl3Source = Data.GetByte(0x22);
            if (SystemControl3Source == 32) SystemControl3Source--; // CC32 is not allowed!
            SystemControl4Source = Data.GetByte(0x23);
            if (SystemControl4Source == 32) SystemControl4Source--; // CC32 is not allowed!
            ControlSource = Data.GetByte(0x24);
            SystemClockSource = Data.GetByte(0x25);
            SystemTempo = (Int16)(16 * Data.GetByte(0x26) + Data.GetByte(0x27));
            TempoAssignSource = Data.GetByte(0x28);
            ReceiveProgramChange = !(Data.GetByte(0x29) == 0x00);
            ReceiveBankSelect = !(Data.GetByte(0x2a) == 0x00);
            SurroundCenterSpeakerSwitch = !(Data.GetByte(0x2b) == 0x00);
            SurroundSubWooferSwitch = !(Data.GetByte(0x2c) == 0x00);
            StereoOutputMode = Data.GetByte(0x2d);
        }
    }

    [DataContract]
    public class StudioSet_Common
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class StudioSet_Common");
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public byte[] VoiceReserve { get; set; }
        [DataMember]
        public byte[] ToneControlSource { get; set; }
        [DataMember]
        public byte Tempo { get; set; }
        [DataMember]
        public byte SoloPart { get; set; }
        [DataMember]
        public Boolean Reverb { get; set; }
        [DataMember]
        public Boolean Chorus { get; set; }
        [DataMember]
        public Boolean MasterEqualizer { get; set; }
        [DataMember]
        public Boolean DrumCompressorAndEqualizer { get; set; }
        [DataMember]
        public byte DrumCompressorAndEqualizerPart { get; set; }
        [DataMember]
        public byte[] DrumCompressorAndEqualizerOutputAssign { get; set; }
        [DataMember]
        public byte ExternalPartLevel { get; set; }
        [DataMember]
        public byte ExternalPartChorusSendLevel { get; set; }
        [DataMember]
        public byte ExternalPartReverbSendLevel { get; set; }
        [DataMember]
        public Boolean ExternalPartMute { get; set; }

        public StudioSet_Common(ReceivedData Data)
        {
            //t.Trace("public StudioSet_Common (" + "ReceivedData" + Data + ", " + ")");
            Name = "";
            for (Int32 i = 0x00; i < 0x10; i++)
            {
                Name += (char)Data.GetByte(i);
            }
            VoiceReserve = new byte[16];
            for (Int32 i = 0; i < 16; i++)
            {
                VoiceReserve[i] = Data.GetByte(i + 0x18);
            }
            ToneControlSource = new byte[4];
            for (Int32 i = 0; i < 4; i++)
            {
                ToneControlSource[i] = Data.GetByte(i + 0x39);
            }
            Tempo = (byte)(16 * Data.GetByte(0x3d) + Data.GetByte(0x3e));
            SoloPart = Data.GetByte(0x3f);
            Reverb = !(Data.GetByte(0x40) == 0x00);
            Chorus = !(Data.GetByte(0x41) == 0x00);
            MasterEqualizer = !(Data.GetByte(0x42) == 0x00);
            DrumCompressorAndEqualizer = !(Data.GetByte(0x43) == 0x00);
            DrumCompressorAndEqualizerPart = Data.GetByte(0x44);
            DrumCompressorAndEqualizerOutputAssign = new byte[6];
            for (Int32 i = 0; i < 6; i++)
            {
                DrumCompressorAndEqualizerOutputAssign[i] = Data.GetByte(i + 0x45);
            }
            ExternalPartLevel = Data.GetByte(0x4c);
            ExternalPartChorusSendLevel = Data.GetByte(0x4d);
            ExternalPartReverbSendLevel = Data.GetByte(0x4e);
            ExternalPartMute = !(Data.GetByte(0x4f) == 0x00);
        }
    }

    [DataContract]
    public class StudioSet_CommonChorus
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class StudioSet_CommonChorus");
        [DataMember]
        public byte Type { get; set; }
        [DataMember]
        public byte Level { get; set; }
        [DataMember]
        public byte OutputAssign { get; set; }
        [DataMember]
        public byte OutputSelect { get; set; }
        [DataMember]
        public StudioSet_CommonChorusOff Off = new StudioSet_CommonChorusOff(null);
        [DataMember]
        public StudioSet_CommonChorusChorus Chorus = new StudioSet_CommonChorusChorus(null);
        [DataMember]
        public StudioSet_CommonChorusDelay Delay = new StudioSet_CommonChorusDelay(null);
        [DataMember]
        public StudioSet_CommonChorusGM2Chorus Gm2Chorus = new StudioSet_CommonChorusGM2Chorus(null);

        public StudioSet_CommonChorus(ReceivedData Data)
        {
            //t.Trace("public StudioSet_CommonChorus(ReceivedData Data)");
            Type = Data.GetByte(0);
            Level = Data.GetByte(1);
            OutputAssign = Data.GetByte(2);
            OutputSelect = Data.GetByte(3);

            // Data for the different types are stored in the same MIDI memory space!
            // Changing type by sending type byte only will not make I-7 update MIDI data.
            // I-7 will update MIDI data from some internal storage only when changed from the front panel.
            // Below are default settigns for each type that I have read out from I-7 Integra Preview studio set.
            // Create all type classes with receivedata = null and initialize with this data:
            byte[] defaultChorusOff = { 0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x04, 0x00, 0x00, 0x00, 0x7f, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x06, 0x08, 0x00, 0x01, 0x04, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x0a, 0x08, 0x00, 0x01, 0x02, 0x08, 0x00, 0x01, 0x0e, 0x08, 0x00, 0x05, 0x0a, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x03, 0x0b, 0x08, 0x00, 0x01, 0x01, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 };
            byte[] defaultChorusChorus = { 0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x04, 0x00, 0x00, 0x01, 0x7f, 0x00, 0x00, 0x08, 0x00, 0x00, 0x02, 0x08, 0x00, 0x00, 0x06, 0x08, 0x00, 0x01, 0x04, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x0a, 0x08, 0x00, 0x01, 0x02, 0x08, 0x00, 0x01, 0x0e, 0x08, 0x00, 0x05, 0x0a, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 };
            byte[] defaultChorusDelay = { 0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x04, 0x00, 0x00, 0x02, 0x7f, 0x00, 0x00, 0x08, 0x00, 0x00, 0x01, 0x08, 0x00, 0x0c, 0x08, 0x08, 0x00, 0x00, 0x07, 0x08, 0x00, 0x00, 0x01, 0x08, 0x01, 0x09, 0x00, 0x08, 0x00, 0x00, 0x0a, 0x08, 0x00, 0x00, 0x01, 0x08, 0x02, 0x05, 0x08, 0x08, 0x00, 0x00, 0x0c, 0x08, 0x00, 0x03, 0x0b, 0x08, 0x00, 0x01, 0x01, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 };
            byte[] defaultChorusGM2Chorus = { 0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x04, 0x00, 0x00, 0x03, 0x7f, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x04, 0x00, 0x08, 0x00, 0x00, 0x08, 0x08, 0x00, 0x05, 0x00, 0x08, 0x00, 0x00, 0x03, 0x08, 0x00, 0x01, 0x03, 0x08, 0x00, 0x00, 0x00, 0x08, 0x02, 0x05, 0x08, 0x08, 0x00, 0x00, 0x0c, 0x08, 0x00, 0x03, 0x0b, 0x08, 0x00, 0x01, 0x01, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 };
            if (Type == 0)
            {
                Off = new StudioSet_CommonChorusOff(Data);
            }
            //else
            //{
            //    Off = new StudioSet_CommonChorusOff(new ReceivedData(defaultChorusOff));
            //}
            if (Type == 1)
            {
                Chorus = new StudioSet_CommonChorusChorus(Data);
            }
            //else
            //{
            //    Chorus = new StudioSet_CommonChorusChorus(new ReceivedData(defaultChorusChorus));
            //}
            if (Type == 2)
            {
                Delay = new StudioSet_CommonChorusDelay(Data);
            }
            //else
            //{
            //    Delay = new StudioSet_CommonChorusDelay(new ReceivedData(defaultChorusDelay));
            //}
            if (Type == 3)
            {
                Gm2Chorus = new StudioSet_CommonChorusGM2Chorus(Data);
            }
            //else
            //{
            //    Gm2Chorus = new StudioSet_CommonChorusGM2Chorus(new ReceivedData(defaultChorusGM2Chorus));
            //}
        }

        //public StudioSet_CommonChorus(ReceivedData Data)
        //{
        //    //t.Trace("public StudioSet_CommonChorus (" + "ReceivedData" + Data + ", " + ")");
        //    byte[] defaultChorusOff = { 0x00, 0x7f, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x06, 0x08, 0x00, 0x01, 0x04, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x0a, 0x08, 0x00, 0x01, 0x02, 0x08, 0x00, 0x01, 0x0e, 0x08, 0x00, 0x05, 0x0a, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x03, 0x0b, 0x08, 0x00, 0x01, 0x01, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 };
        //    byte[] defaultChorusChorus = { 0x01, 0x7f, 0x00, 0x00, 0x08, 0x00, 0x00, 0x02, 0x08, 0x00, 0x00, 0x06, 0x08, 0x00, 0x01, 0x04, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x0a, 0x08, 0x00, 0x01, 0x02, 0x08, 0x00, 0x01, 0x0e, 0x08, 0x00, 0x05, 0x0a, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 };
        //    byte[] defaultChorusDelay = { 0x02, 0x7f, 0x00, 0x00, 0x08, 0x00, 0x00, 0x01, 0x08, 0x00, 0x0c, 0x08, 0x08, 0x00, 0x00, 0x07, 0x08, 0x00, 0x00, 0x01, 0x08, 0x01, 0x09, 0x00, 0x08, 0x00, 0x00, 0x0a, 0x08, 0x00, 0x00, 0x01, 0x08, 0x02, 0x05, 0x08, 0x08, 0x00, 0x00, 0x0c, 0x08, 0x00, 0x03, 0x0b, 0x08, 0x00, 0x01, 0x01, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 };
        //    byte[] defaultChorusGM2Chorus = { 0x03, 0x7f, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x04, 0x00, 0x08, 0x00, 0x00, 0x08, 0x08, 0x00, 0x05, 0x00, 0x08, 0x00, 0x00, 0x03, 0x08, 0x00, 0x01, 0x03, 0x08, 0x00, 0x00, 0x00, 0x08, 0x02, 0x05, 0x08, 0x08, 0x00, 0x00, 0x0c, 0x08, 0x00, 0x03, 0x0b, 0x08, 0x00, 0x01, 0x01, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 };
        //    Off = new StudioSet_CommonChorusOff(new ReceivedData(defaultChorusOff));
        //    Chorus = new StudioSet_CommonChorusChorus(new ReceivedData(defaultChorusChorus));
        //    Delay = new StudioSet_CommonChorusDelay(new ReceivedData(defaultChorusDelay));
        //    Gm2Chorus = new StudioSet_CommonChorusGM2Chorus(new ReceivedData(defaultChorusGM2Chorus));
        //    Type = Data.GetByte(0);
        //    Level = Data.GetByte(1);
        //    OutputAssign = Data.GetByte(2);
        //    OutputSelect = Data.GetByte(3);
        //    switch (Type)
        //    {
        //        case 0:
        //            Off = new StudioSet_CommonChorusOff(Data);
        //            break;
        //        case 1:
        //            Chorus = new StudioSet_CommonChorusChorus(Data);
        //            break;
        //        case 2:
        //            Delay = new StudioSet_CommonChorusDelay(Data);
        //            break;
        //        case 3:
        //            Gm2Chorus = new StudioSet_CommonChorusGM2Chorus(Data);
        //            break;
        //    }
        //}

        public byte[] GetSwitchMessage(byte Type)
        {
            ////t.Trace("public byte[] GetSwitchMessage (" + "byte" + Type + ", " + ")");
            switch (Type)
            {
                case 0:
                    return Off.RawData;
                case 1:
                    return Chorus.RawData;
                case 2:
                    return Delay.RawData;
                case 3:
                    return Gm2Chorus.RawData;
            }
            return null;
        }
    }

    [DataContract]
    public class StudioSet_CommonChorusOff
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class StudioSet_CommonChorusOff");
        [DataMember]
        public byte[] RawData { get; set; }

        public StudioSet_CommonChorusOff(ReceivedData Data)
        {
            //t.Trace("public StudioSet_CommonChorusOff (" + "ReceivedData" + Data + ", " + ")");
            if (Data == null)
            {
                Data = new ReceivedData(new byte[] {
                    0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x00, 0x04, 0x00, 0x00, 0x7f, 0x00, 0x00, 0x08,
                    0x00, 0x00, 0x00, 0x5d, 0xf7 });
            }
            RawData = Data.RawData;
        }
    }

    [DataContract]
    public class StudioSet_CommonChorusChorus
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class StudioSet_CommonChorusChorus");
        [DataMember]
        public byte[] RawData { get; set; }
        [DataMember]
        public byte FilterType { get; set; }
        [DataMember]
        public byte FilterCutoffFrequency { get; set; }
        [DataMember]
        public byte PreDelay { get; set; }
        [DataMember]
        public byte RateHzNote { get; set; }
        [DataMember]
        public byte RateHz { get; set; }
        [DataMember]
        public byte RateNote { get; set; }
        [DataMember]
        public byte Depth { get; set; }
        [DataMember]
        public byte Phase { get; set; }
        [DataMember]
        public byte Feedback { get; set; }

        public StudioSet_CommonChorusChorus(ReceivedData Data)
        {
            //t.Trace("public StudioSet_CommonChorusChorus (" + "ReceivedData" + Data + ", " + ")");
            if (Data == null)
            {
                Data = new ReceivedData(new byte[] {
                0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x00, 0x04, 0x00, 0x01, 0x7f, 0x00, 0x00, 0x08,
                0x00, 0x00, 0x02, 0x08, 0x00, 0x00, 0x06, 0x08, 0x00, 0x01, 0x04, 0x08, 0x00, 0x00, 0x00, 0x08,
                0x00, 0x00, 0x0a, 0x08, 0x00, 0x01, 0x02, 0x08, 0x00, 0x01, 0x0e, 0x08, 0x00, 0x05, 0x0a, 0x08,
                0x00, 0x00, 0x00, 0x64, 0xf7 });
            }
            RawData = Data.RawData;

            FilterType = Data.GetByte(7);
            FilterCutoffFrequency = Data.GetByte(11);
            PreDelay = (byte)(16 * Data.GetByte(14) + Data.GetByte(15));
            RateHzNote = Data.GetByte(19);
            RateHz = (byte)(16 * Data.GetByte(22) + Data.GetByte(23)); // 0x08 0x00 0x00 0x01 - 0x08 0x00 0x0c 0x08  => 0.05 - 10 step 0.05, 199 values
            RateNote = (byte)(16 * Data.GetByte(26) + Data.GetByte(27));
            Depth = (byte)(16 * Data.GetByte(30) + Data.GetByte(31));
            Phase = (byte)(16 * Data.GetByte(34) + Data.GetByte(35));
            Feedback = (byte)(16 * Data.GetByte(38) + Data.GetByte(39));
        }

        public byte[] GetData()
        {
            List<byte> data = new List<byte>();
            data.Add(0x08);
            data.Add(0x00);
            data.Add(0x00);
            data.Add(FilterType);
            data.Add(0x08);
            data.Add(0x00);
            data.Add(0x00);
            data.Add(FilterCutoffFrequency);
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(PreDelay / 16));
            data.Add((byte)(PreDelay % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add(0x00);
            data.Add(RateHzNote);
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(RateHz / 16));
            data.Add((byte)(RateHz % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(RateNote / 16));
            data.Add((byte)(RateNote % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(Depth / 16));
            data.Add((byte)(Depth % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(Phase / 16));
            data.Add((byte)(Phase % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(Feedback / 16));
            data.Add((byte)(Feedback % 16));
            return data.ToArray();
        }
    }

    [DataContract]
    public class StudioSet_CommonChorusDelay
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class StudioSet_CommonChorusDelay");
        [DataMember]
        public byte[] RawData { get; set; }
        [DataMember]
        public byte LeftMsNote { get; set; }
        [DataMember]
        public byte LeftMs { get; set; }
        [DataMember]
        public byte LeftNote { get; set; }
        [DataMember]
        public byte RightMsNote { get; set; }
        [DataMember]
        public byte RightMs { get; set; }
        [DataMember]
        public byte RightNote { get; set; }
        [DataMember]
        public byte CenterMsNote { get; set; }
        [DataMember]
        public byte CenterMs { get; set; }
        [DataMember]
        public byte CenterNote { get; set; }
        [DataMember]
        public byte CenterFeedback { get; set; }
        [DataMember]
        public byte HFDamp { get; set; }
        [DataMember]
        public byte LeftLevel { get; set; }
        [DataMember]
        public byte RightLevel { get; set; }
        [DataMember]
        public byte CenterLevel { get; set; }

        public StudioSet_CommonChorusDelay()
        {
            //t.Trace("public StudioSet_CommonChorusDelay()");
            LeftMsNote = 0;
            //Left = (byte)(16 * Data.GetByte(10) + Data.GetByte(11));
            RightMsNote = 0;
            //Right = (byte)(16 * Data.GetByte(22) + Data.GetByte(23));
            CenterMsNote = 0;
            //Center = (byte)(16 * Data.GetByte(34) + Data.GetByte(35));
            //CenterFeedback = (byte)(16 * Data.GetByte(42) + Data.GetByte(43));
            //HFDamp = Data.GetByte(47);
            //LeftLevel = (byte)(16 * Data.GetByte(50) + Data.GetByte(51));
            //RightLevel = (byte)(16 * Data.GetByte(54) + Data.GetByte(55));
            //CenterLevel = (byte)(16 * Data.GetByte(58) + Data.GetByte(59));
        }

        public StudioSet_CommonChorusDelay(ReceivedData Data)
        {
            //t.Trace("public StudioSet_CommonChorusDelay (" + "ReceivedData" + Data + ", " + ")");
            if (Data == null)
            {
                Data = new ReceivedData(new byte[] {
                    0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x00, 0x04, 0x00, 0x02, 0x7f, 0x00, 0x00, 0x08,
                    0x00, 0x00, 0x01, 0x08, 0x00, 0x0c, 0x08, 0x08, 0x00, 0x00, 0x06, 0x08, 0x00, 0x00, 0x01, 0x08,
                    0x01, 0x09, 0x00, 0x08, 0x00, 0x00, 0x09, 0x08, 0x00, 0x00, 0x01, 0x08, 0x02, 0x05, 0x08, 0x08,
                    0x00, 0x00, 0x0c, 0x08, 0x00, 0x03, 0x0b, 0x08, 0x00, 0x01, 0x01, 0x08, 0x00, 0x07, 0x0f, 0x08,
                    0x00, 0x07, 0x0f, 0x08, 0x00, 0x07, 0x0f, 0x56, 0xf7 });
            }
            RawData = Data.RawData;

            LeftMsNote = Data.GetByte(7);
            LeftMs = (byte)(16 * Data.GetByte(10) + Data.GetByte(11));
            LeftNote = (byte)(16 * Data.GetByte(14) + Data.GetByte(15));
            RightMsNote = Data.GetByte(19);
            RightMs = (byte)(16 * Data.GetByte(22) + Data.GetByte(23));
            RightNote = (byte)(16 * Data.GetByte(26) + Data.GetByte(27));
            CenterMsNote = Data.GetByte(31);
            CenterMs = (byte)(16 * Data.GetByte(34) + Data.GetByte(35));
            CenterNote = (byte)(16 * Data.GetByte(38) + Data.GetByte(39));
            CenterFeedback = (byte)(16 * Data.GetByte(42) + Data.GetByte(43));
            HFDamp = Data.GetByte(47);
            LeftLevel = (byte)(16 * Data.GetByte(50) + Data.GetByte(51));
            RightLevel = (byte)(16 * Data.GetByte(54) + Data.GetByte(55));
            CenterLevel = (byte)(16 * Data.GetByte(58) + Data.GetByte(59));
        }

        public byte[] GetData()
        {
            List<byte> data = new List<byte>();
            data.Add(0x08);
            data.Add(0x00);
            data.Add(0x00);
            data.Add(LeftMsNote);
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(LeftMs / 16));
            data.Add((byte)(LeftMs % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(LeftNote / 16));
            data.Add((byte)(LeftNote % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add(0x00);
            data.Add(RightMsNote);
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(RightMs / 16));
            data.Add((byte)(RightMs % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(RightNote / 16));
            data.Add((byte)(RightNote % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add(0x00);
            data.Add(CenterMsNote);
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(CenterMs / 16));
            data.Add((byte)(CenterMs % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(CenterNote / 16));
            data.Add((byte)(CenterNote % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)((CenterFeedback + 49) / 16));
            data.Add((byte)((CenterFeedback + 49) % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add(0x00);
            data.Add(HFDamp);
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(LeftLevel / 16));
            data.Add((byte)(LeftLevel % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(RightLevel / 16));
            data.Add((byte)(RightLevel % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(CenterLevel / 16));
            data.Add((byte)(CenterLevel % 16));
            return data.ToArray();
        }
    }

    [DataContract]
    public class StudioSet_CommonChorusGM2Chorus
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class StudioSet_CommonChorusGM2Chorus");
        [DataMember]
        public byte[] RawData { get; set; }
        [DataMember]
        public byte PreLPF { get; set; }
        [DataMember]
        public byte Level { get; set; }
        [DataMember]
        public byte Feedback { get; set; }
        [DataMember]
        public byte Delay { get; set; }
        [DataMember]
        public byte Rate { get; set; }
        [DataMember]
        public byte Depth { get; set; }
        [DataMember]
        public byte SendLevelToReverb { get; set; }

        public StudioSet_CommonChorusGM2Chorus(ReceivedData Data)
        {
            //t.Trace("public StudioSet_CommonChorusGM2Chorus (" + "ReceivedData" + Data + ", " + ")");
            if (Data == null)
            {
                Data = new ReceivedData(new byte[] {
                    0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x00, 0x04, 0x00, 0x03, 0x7f, 0x00, 0x00, 0x08,
                    0x00, 0x00, 0x00, 0x08, 0x00, 0x04, 0x00, 0x08, 0x00, 0x00, 0x08, 0x08, 0x00, 0x05, 0x00, 0x08,
                    0x00, 0x00, 0x03, 0x08, 0x00, 0x01, 0x03, 0x08, 0x00, 0x00, 0x00, 0x12, 0xf7 });
            }
            RawData = Data.RawData;

            PreLPF = Data.GetByte(7);
            Level = (byte)(16 * Data.GetByte(10) + Data.GetByte(11));
            Feedback = (byte)(16 * Data.GetByte(14) + Data.GetByte(15));
            Delay = (byte)(16 * Data.GetByte(18) + Data.GetByte(19));
            Rate = (byte)(16 * Data.GetByte(22) + Data.GetByte(23));
            Depth = (byte)(16 * Data.GetByte(26) + Data.GetByte(27));
            SendLevelToReverb = (byte)(16 * Data.GetByte(30) + Data.GetByte(31));
        }

        public byte[] GetData()
        {
            List<byte> data = new List<byte>();
            data.Add(0x08);
            data.Add(0x00);
            data.Add(0x00);
            data.Add(PreLPF);
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(Level / 16));
            data.Add((byte)(Level % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(Feedback / 16));
            data.Add((byte)(Feedback % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(Delay / 16));
            data.Add((byte)(Delay % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(Rate / 16));
            data.Add((byte)(Rate % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(Depth / 16));
            data.Add((byte)(Depth % 16));
            data.Add(0x08);
            data.Add(0x00);
            data.Add((byte)(SendLevelToReverb / 16));
            data.Add((byte)(SendLevelToReverb % 16));
            return data.ToArray();
        }
    }

    [DataContract]
    public class StudioSet_CommonReverb
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class StudioSet_CommonReverb");
        [DataMember]
        public byte Type { get; set; }
        [DataMember]
        public byte Level { get; set; }
        [DataMember]
        public byte OutputAssign { get; set; }
        [DataMember]
        public StudioSet_CommonReverbReverb ReverbRoom1 { get; set; }
        [DataMember]
        public StudioSet_CommonReverbReverb ReverbRoom2 { get; set; }
        [DataMember]
        public StudioSet_CommonReverbReverb ReverbHall1 { get; set; }
        [DataMember]
        public StudioSet_CommonReverbReverb ReverbHall2 { get; set; }
        [DataMember]
        public StudioSet_CommonReverbReverb ReverbPlate { get; set; }
        [DataMember]
        public StudioSet_CommonReverbGM2Reverb GM2Reverb { get; set; }

        public StudioSet_CommonReverb(ReceivedData Data)
        {
            //t.Trace("public StudioSet_CommonReverb (" + "ReceivedData" + Data + ", " + ")");

            Type = Data.GetByte(0);
            Level = Data.GetByte(1);
            OutputAssign = Data.GetByte(2);
            if (Type == 1)
            {
                ReverbRoom1 = new StudioSet_CommonReverbReverb(Data);
            }
            else
            {
                ReverbRoom1 = new StudioSet_CommonReverbReverb(new ReceivedData(new byte[] {
                    0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x00, 0x06, 0x00, 0x01, 0x5f, 0x00, 0x08, 0x00,
                    0x05, 0x0f, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x09, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00,
                    0x07, 0x0f, 0x08, 0x00, 0x03, 0x02, 0x08, 0x00, 0x03, 0x02, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00,
                    0x04, 0x00, 0x4d, 0xf7 }));
            }
            if (Type == 2)
            {
                ReverbRoom2 = new StudioSet_CommonReverbReverb(Data);
            }
            else
            {
                ReverbRoom2 = new StudioSet_CommonReverbReverb(new ReceivedData(new byte[] {
                    0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x00, 0x06, 0x00, 0x02, 0x5f, 0x00, 0x08, 0x00,
                    0x05, 0x0f, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x09, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00,
                    0x07, 0x0f, 0x08, 0x00, 0x03, 0x02, 0x08, 0x00, 0x03, 0x02, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00,
                    0x04, 0x00, 0x4c, 0xf7 }));
            }
            if (Type == 3)
            {
                ReverbHall1 = new StudioSet_CommonReverbReverb(Data);
            }
            else
            {
                ReverbHall1 = new StudioSet_CommonReverbReverb(new ReceivedData(new byte[] {
                    0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x00, 0x06, 0x00, 0x03, 0x5f, 0x00, 0x08, 0x00,
                    0x05, 0x0f, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x01, 0x05, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00,
                    0x07, 0x0f, 0x08, 0x00, 0x03, 0x02, 0x08, 0x00, 0x03, 0x02, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00,
                    0x04, 0x00, 0x4e, 0xf7 }));
            }
            if (Type == 4)
            {
                ReverbHall2 = new StudioSet_CommonReverbReverb(Data);
            }
            else
            {
                ReverbHall2 = new StudioSet_CommonReverbReverb(new ReceivedData(new byte[] {
                    0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x00, 0x06, 0x00, 0x04, 0x5f, 0x00, 0x08, 0x00,
                    0x05, 0x0f, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x01, 0x0a, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00,
                    0x07, 0x0f, 0x08, 0x00, 0x03, 0x02, 0x08, 0x00, 0x03, 0x02, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00,
                    0x04, 0x00, 0x48, 0xf7 }));
            }
            if (Type == 5)
            {
                ReverbPlate = new StudioSet_CommonReverbReverb(Data);
            }
            else
            {
                ReverbPlate = new StudioSet_CommonReverbReverb(new ReceivedData(new byte[] {
                    0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x00, 0x06, 0x00, 0x05, 0x5f, 0x00, 0x08, 0x00,
                    0x05, 0x0f, 0x08, 0x00, 0x01, 0x04, 0x08, 0x00, 0x01, 0x0a, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00,
                    0x07, 0x0f, 0x08, 0x00, 0x03, 0x02, 0x08, 0x00, 0x03, 0x02, 0x08, 0x00, 0x07, 0x0f, 0x08, 0x00,
                    0x04, 0x00, 0x42, 0xf7 }));
            }
            if (Type == 6)
            {
                GM2Reverb = new StudioSet_CommonReverbGM2Reverb(Data);
            }
            else
            {
                GM2Reverb = new StudioSet_CommonReverbGM2Reverb(new ReceivedData(new byte[] {
                    0xf0, 0x41, 0x10, 0x00, 0x00, 0x64, 0x12, 0x18, 0x00, 0x06, 0x00, 0x06, 0x5f, 0x00, 0x08, 0x00,
                    0x00, 0x04, 0x08, 0x00, 0x04, 0x00, 0x08, 0x00, 0x00, 0x00, 0x08, 0x00, 0x04, 0x00, 0x51, 0xf7 }));
            }
        }
    }

    [DataContract]
    public class StudioSet_CommonReverbReverb
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class StudioSet_CommonReverbReverb");
        [DataMember]
        public byte PreDelay { get; set; }
        [DataMember]
        public byte Time { get; set; }
        [DataMember]
        public byte Density { get; set; }
        [DataMember]
        public byte Diffusion { get; set; }
        [DataMember]
        public byte LFDamp { get; set; }
        [DataMember]
        public byte HFDamp { get; set; }
        [DataMember]
        public byte Spread { get; set; }
        [DataMember]
        public byte Tone { get; set; }

        public StudioSet_CommonReverbReverb(ReceivedData Data)
        {
            //t.Trace("public StudioSet_CommonReverbReverb (" + "ReceivedData" + Data + ", " + ")");
            PreDelay = (byte)(16 * Data.GetByte(9) + Data.GetByte(10));
            Time = (byte)(16 * Data.GetByte(13) + Data.GetByte(14));
            Density = (byte)(16 * Data.GetByte(17) + Data.GetByte(18));
            Diffusion = (byte)(16 * Data.GetByte(21) + Data.GetByte(22));
            LFDamp = (byte)(16 * Data.GetByte(25) + Data.GetByte(26));
            HFDamp = (byte)(16 * Data.GetByte(29) + Data.GetByte(30));
            Spread = (byte)(16 * Data.GetByte(33) + Data.GetByte(34));
            Tone = (byte)(16 * Data.GetByte(37) + Data.GetByte(38));
        }

        // Use this to quickly create a parameter set to send to I-7 at address 18 00 06 03
        public byte[] GetData()
        {
            return new byte[]
            {
                0x08, 0x00, 0x00, 0x00, // GM2Character, not used in this class
                0x08, 0x00, (byte)(PreDelay / 16), (byte)(PreDelay % 16),
                0x08, 0x00, (byte)(Time / 16), (byte)(Time % 16),
                0x08, 0x00, (byte)(Density / 16), (byte)(Density % 16),
                0x08, 0x00, (byte)(Diffusion / 16), (byte)(Diffusion % 16),
                0x08, 0x00, (byte)(LFDamp / 16), (byte)(LFDamp % 16),
                0x08, 0x00, (byte)(HFDamp / 16), (byte)(HFDamp % 16),
                0x08, 0x00, (byte)(Spread / 16), (byte)(Spread % 16),
                0x08, 0x00, (byte)(Tone / 16), (byte)(Tone % 16)
            };
        }
    }

    [DataContract]
    public class StudioSet_CommonReverbGM2Reverb
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class StudioSet_CommonReverbGM2Reverb");
        [DataMember]
        public byte Character { get; set; }
        [DataMember]
        public byte Time { get; set; }

        public StudioSet_CommonReverbGM2Reverb(ReceivedData Data)
        {
            //t.Trace("public StudioSet_CommonReverbGM2Reverb (" + "ReceivedData" + Data + ", " + ")");
            Character = (byte)(16 * Data.GetByte(5) + Data.GetByte(6));
            Time = (byte)(16 * Data.GetByte(17) + Data.GetByte(18));
        }

        // Use this to quickly create a parameter set to send to I-7 at address 18 00 06 03
        public byte[] GetData()
        {
            return new byte[]
            {
                0x08, 0x00, (byte)(Character / 16), (byte)(Character % 16),
                0x08, 0x00, 0x00, 0x00, // Not used in GM2Reverb
                0x08, 0x00, 0x00, 0x00,
                0x08, 0x00, (byte)(Time / 16), (byte)(Time % 16)
            };
        }
    }

    [DataContract]
    public class StudioSet_MotionalSurround
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class StudioSet_MotionalSurround");
        public Boolean MotionalSurroundSwitch { get; set; }
        [DataMember]
        public byte RoomType { get; set; }
        [DataMember]
        public byte AmbienceLevel { get; set; }
        [DataMember]
        public byte RoomSize { get; set; }
        [DataMember]
        public byte AmbienceTime { get; set; }
        [DataMember]
        public byte AmbienceDensity { get; set; }
        [DataMember]
        public byte AmbienceHFDamp { get; set; }
        [DataMember]
        public byte ExtPartLR { get; set; }
        [DataMember]
        public byte ExtPartFB { get; set; }
        [DataMember]
        public byte ExtPartWidth { get; set; }
        [DataMember]
        public byte ExtPartAmbienceSendLevel { get; set; }
        [DataMember]
        public byte ExtPartControlChannel { get; set; }
        [DataMember]
        public byte MotionalSurroundDepth { get; set; }

        public StudioSet_MotionalSurround(ReceivedData Data)
        {
            //t.Trace("public StudioSet_MotionalSurround (" + "ReceivedData" + Data + ", " + ")");
            MotionalSurroundSwitch = Data.GetByte(0) > 0 ? true : false;
            RoomType = Data.GetByte(1);
            AmbienceLevel = Data.GetByte(2);
            RoomSize = Data.GetByte(3);
            AmbienceTime = Data.GetByte(4);
            AmbienceDensity = Data.GetByte(5);
            AmbienceHFDamp = Data.GetByte(6);
            ExtPartLR = Data.GetByte(7);
            ExtPartFB = Data.GetByte(8);
            ExtPartWidth = Data.GetByte(9);
            ExtPartAmbienceSendLevel = Data.GetByte(10);
            ExtPartControlChannel = Data.GetByte(11);
            MotionalSurroundDepth = Data.GetByte(12);
        }
    }

    [DataContract]
    public class StudioSet_MasterEQ
    {
        public Boolean MotionalSurroundSwitch { get; set; }
        HBTrace t = new HBTrace("class StudioSet_MasterEQ");
        [DataMember]
        public byte EQLowFreq { get; set; }
        [DataMember]
        public byte EQLowGain { get; set; }
        [DataMember]
        public byte EQMidFreq { get; set; }
        [DataMember]
        public byte EQMidGain { get; set; }
        [DataMember]
        public byte EQMidQ { get; set; }
        [DataMember]
        public byte EQHighFreq { get; set; }
        [DataMember]
        public byte EQHighGain { get; set; }

        public StudioSet_MasterEQ(ReceivedData Data)
        {
            //t.Trace("public StudioSet_MasterEQ (" + "ReceivedData" + Data + ", " + ")");
            EQLowFreq = Data.GetByte(0);
            EQLowGain = Data.GetByte(1);
            EQMidFreq = Data.GetByte(2);
            EQMidGain = Data.GetByte(3);
            EQMidQ = Data.GetByte(4);
            EQHighFreq = Data.GetByte(5);
            EQHighGain = Data.GetByte(6);
        }
    }

    [DataContract]
    public class StudioSet_PartMainSettings
    {
        public Boolean MotionalSurroundSwitch { get; set; }
        HBTrace t = new HBTrace("class StudioSet_PartMainSettings");
        [DataMember]
        public byte ReceiveChannel { get; set; }
        [DataMember]
        public Boolean ReceiveSwitch { get; set; }
        [DataMember]
        public byte ToneBankSelectMSB { get; set; }
        [DataMember]
        public byte ToneBankSelectLSB { get; set; }
        [DataMember]
        public byte ToneProgramNumber { get; set; }
        [DataMember]
        public byte Level { get; set; }
        [DataMember]
        public byte Pan { get; set; }
        [DataMember]
        public byte CoarseTune { get; set; }
        [DataMember]
        public byte FineTune { get; set; }
        [DataMember]
        public byte MonoPoly { get; set; }
        [DataMember]
        public byte Legato { get; set; }
        [DataMember]
        public byte PitchBendRange { get; set; }
        [DataMember]
        public byte PortamentoSwitch { get; set; }
        [DataMember]
        public byte PortamentoTime { get; set; }
        [DataMember]
        public byte CutoffOffset { get; set; }
        [DataMember]
        public byte ResonanceOffset { get; set; }
        [DataMember]
        public byte AttackTimeOffset { get; set; }
        [DataMember]
        public byte DecayTimeOffset { get; set; }
        [DataMember]
        public byte ReleaseTimeOffset { get; set; }
        [DataMember]
        public byte VibratoRate { get; set; }
        [DataMember]
        public byte VibratoDepth { get; set; }
        [DataMember]
        public byte VibratoDelay { get; set; }
        [DataMember]
        public byte ChorusSendLevel { get; set; }
        [DataMember]
        public byte ReverbSendLevel { get; set; }
        [DataMember]
        public byte OutputAssign { get; set; }

        public StudioSet_PartMainSettings(ReceivedData Data)
        {
            //t.Trace("public StudioSet_PartMainSettings (" + "ReceivedData" + Data + ", " + ")");
            ReceiveChannel = Data.GetByte(0x00);
            ReceiveSwitch = Data.GetByte(0x01) > 0;
            ToneBankSelectMSB = Data.GetByte(0x06);
            ToneBankSelectLSB = Data.GetByte(0x07);
            ToneProgramNumber = Data.GetByte(0x08);
            Level = Data.GetByte(0x09);
            Pan = Data.GetByte(0x0a);
            CoarseTune = Data.GetByte(0x0b);
            FineTune = Data.GetByte(0x0c);
            MonoPoly = Data.GetByte(0x0d);
            Legato = Data.GetByte(0x0e);
            PitchBendRange = Data.GetByte(0x0f);
            PortamentoSwitch = Data.GetByte(0x10);
            PortamentoTime = (byte)(16 * Data.GetByte(0x11) + Data.GetByte(0x11));
            CutoffOffset = Data.GetByte(0x13);
            ResonanceOffset = Data.GetByte(0x14);
            AttackTimeOffset = Data.GetByte(0x15);
            DecayTimeOffset = Data.GetByte(0x16);
            ReleaseTimeOffset = Data.GetByte(0x17);
            VibratoRate = Data.GetByte(0x18);
            VibratoDepth = Data.GetByte(0x19);
            VibratoDelay = Data.GetByte(0x1a);
            ChorusSendLevel = Data.GetByte(0x27);
            ReverbSendLevel = Data.GetByte(0x28);
            OutputAssign = Data.GetByte(0x29);
        }
    }

    [DataContract]
    public class StudioSet_PartKeyboard
    {
        public Boolean MotionalSurroundSwitch { get; set; }
        HBTrace t = new HBTrace("class StudioSet_PartKeyboard");
        [DataMember]
        public byte OctaveShift { get; set; }
        [DataMember]
        public byte VelocitySenseOffset { get; set; }
        [DataMember]
        public byte RangeLower { get; set; }
        [DataMember]
        public byte RangeUpper { get; set; }
        [DataMember]
        public byte FadeWidthLower { get; set; }
        [DataMember]
        public byte FadeWidthUpper { get; set; }
        [DataMember]
        public byte VelocityRangeLower { get; set; }
        [DataMember]
        public byte VelocityRangeUpper { get; set; }
        [DataMember]
        public byte VelocityFadeWidthLower { get; set; }
        [DataMember]
        public byte VelocityFadeWidthUpper { get; set; }
        [DataMember]
        public Boolean Mute { get; set; }
        [DataMember]
        public byte VelocityCurveType { get; set; }

        public StudioSet_PartKeyboard(ReceivedData Data)
        {
            //t.Trace("public StudioSet_PartKeyboard (" + "ReceivedData" + Data + ", " + ")");
            OctaveShift = Data.GetByte(0x1b);
            VelocitySenseOffset = Data.GetByte(0x1c);
            RangeLower = Data.GetByte(0x1d);
            RangeUpper = Data.GetByte(0x1e);
            FadeWidthLower = Data.GetByte(0x1f);
            FadeWidthUpper = Data.GetByte(0x20);
            VelocityRangeLower = Data.GetByte(0x21);
            VelocityRangeUpper = Data.GetByte(0x22);
            VelocityFadeWidthLower = Data.GetByte(0x23);
            VelocityFadeWidthUpper = Data.GetByte(0x24);
            Mute = Data.GetByte(0x25) > 0;
            VelocityCurveType = Data.GetByte(0x43);
        }
    }

    [DataContract]
    public class StudioSet_PartScaleTune
    {
        public Boolean MotionalSurroundSwitch { get; set; }
        HBTrace t = new HBTrace("class StudioSet_PartScaleTune");
        [DataMember]
        public byte Type { get; set; }
        [DataMember]
        public byte Key { get; set; }
        [DataMember]
        public byte C { get; set; }
        [DataMember]
        public byte Ci { get; set; }
        [DataMember]
        public byte D { get; set; }
        [DataMember]
        public byte Di { get; set; }
        [DataMember]
        public byte E { get; set; }
        [DataMember]
        public byte F { get; set; }
        [DataMember]
        public byte Fi { get; set; }
        [DataMember]
        public byte G { get; set; }
        [DataMember]
        public byte Gi { get; set; }
        [DataMember]
        public byte A { get; set; }
        [DataMember]
        public byte Ai { get; set; }
        [DataMember]
        public byte B { get; set; }

        public StudioSet_PartScaleTune(ReceivedData Data)
        {
            //t.Trace("public StudioSet_PartScaleTune (" + "ReceivedData" + Data + ", " + ")");
            Type = Data.GetByte(0x2b);
            Key = Data.GetByte(0x2c);
            C = (byte)(Data.GetByte(0x2d));
            Ci = (byte)(Data.GetByte(0x2e));
            D = (byte)(Data.GetByte(0x2f));
            Di = (byte)(Data.GetByte(0x30));
            E = (byte)(Data.GetByte(0x31));
            F = (byte)(Data.GetByte(0x32));
            Fi = (byte)(Data.GetByte(0x33));
            G = (byte)(Data.GetByte(0x34));
            Gi = (byte)(Data.GetByte(0x35));
            A = (byte)(Data.GetByte(0x36));
            Ai = (byte)(Data.GetByte(0x37));
            B = (byte)(Data.GetByte(0x38));
        }
    }

    [DataContract]
    public class StudioSet_PartMidi
    {
        public Boolean MotionalSurroundSwitch { get; set; }
        HBTrace t = new HBTrace("class StudioSet_PartMidi");
        [DataMember]
        public Boolean PhaseLock { get; set; }
        [DataMember]
        public Boolean ReceiveProgramChange { get; set; }
        [DataMember]
        public Boolean ReceiveBankSelect { get; set; }
        [DataMember]
        public Boolean ReceivePitchBend { get; set; }
        [DataMember]
        public Boolean ReceivePolyphonicKeyPressure { get; set; }
        [DataMember]
        public Boolean ReceiveChannelPressure { get; set; }
        [DataMember]
        public Boolean ReceiveModulation { get; set; }
        [DataMember]
        public Boolean ReceiveVolume { get; set; }
        [DataMember]
        public Boolean ReceivePan { get; set; }
        [DataMember]
        public Boolean ReceiveExpression { get; set; }
        [DataMember]
        public Boolean ReceiveHold1 { get; set; }

        public StudioSet_PartMidi(ReceivedData Data)
        {
            //t.Trace("public StudioSet_PartMidi (" + "ReceivedData" + Data + ", " + ")");
            PhaseLock = false; // This has to be read separately!
            ReceiveProgramChange = Data.GetByte(0x39) > 0;
            ReceiveBankSelect = Data.GetByte(0x3a) > 0;
            ReceivePitchBend = Data.GetByte(0x3b) > 0;
            ReceivePolyphonicKeyPressure = Data.GetByte(0x3c) > 0;
            ReceiveChannelPressure = Data.GetByte(0x3d) > 0;
            ReceiveModulation = Data.GetByte(0x3e) > 0;
            ReceiveVolume = Data.GetByte(0x3f) > 0;
            ReceivePan = Data.GetByte(0x40) > 0;
            ReceiveExpression = Data.GetByte(0x41) > 0;
            ReceiveHold1 = Data.GetByte(0x42) > 0;
        }
    }

    [DataContract]
    public class StudioSet_PartMotionalSurround
    {
        public Boolean MotionalSurroundSwitch { get; set; }
        HBTrace t = new HBTrace("class StudioSet_PartMotionalSurround");
        [DataMember]
        public byte LR { get; set; }
        [DataMember]
        public byte FB { get; set; }
        [DataMember]
        public byte Width { get; set; }
        [DataMember]
        public byte AmbienceSendLevel { get; set; }

        public StudioSet_PartMotionalSurround(ReceivedData Data)
        {
            //t.Trace("public StudioSet_PartMotionalSurround (" + "ReceivedData" + Data + ", " + ")");
            LR = (byte)(Data.GetByte(0x44));
            FB = (byte)(Data.GetByte(0x46));
            Width = Data.GetByte(0x48);
            AmbienceSendLevel = Data.GetByte(0x49);
        }
    }

    [DataContract]
    public class StudioSet_PartEQ
    {
        public Boolean MotionalSurroundSwitch { get; set; }
        HBTrace t = new HBTrace("class StudioSet_PartEQ");
        [DataMember]
        public Boolean EqSwitch { get; set; }
        [DataMember]
        public byte EqLowFreq { get; set; }
        [DataMember]
        public byte EqLowGain { get; set; }
        [DataMember]
        public byte EqMidFreq { get; set; }
        [DataMember]
        public byte EqMidGain { get; set; }
        [DataMember]
        public byte EqMidQ { get; set; }
        [DataMember]
        public byte EqHighFreq { get; set; }
        [DataMember]
        public byte EqHighGain { get; set; }

        public StudioSet_PartEQ(ReceivedData Data)
        {
            //t.Trace("public StudioSet_PartEQ (" + "ReceivedData" + Data + ", " + ")");
            EqSwitch = Data.GetByte(0x00) > 0;
            EqLowFreq = Data.GetByte(0x01);
            EqLowGain = (byte)(Data.GetByte(0x02) - 15);
            EqMidFreq = Data.GetByte(0x03);
            EqMidGain = (byte)(Data.GetByte(0x04) - 15);
            EqMidQ = Data.GetByte(0x05);
            EqHighFreq = Data.GetByte(0x06);
            EqHighGain = (byte)(Data.GetByte(0x07) - 15);
        }
    }
}
