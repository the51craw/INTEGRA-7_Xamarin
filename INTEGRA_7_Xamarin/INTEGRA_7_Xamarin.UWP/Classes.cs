using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Integra_7_Xamarin.UWP
{
    public class HBTrace
    {
        Int32 debugLevel = 5;
        //StorageFolder localFolder = null;
        //StorageFile sampleFile = null;

        public HBTrace(String s, Int32 DebugLevel = 0)
        {
            //localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Debug.WriteLine(s);
        }

        public void Trace(String s, Int32 DebugLevel = 0, Double ticks = 0)
        {
            if (DebugLevel <= debugLevel)
            {
                //if (sampleFile == null)
                //{
                //    sampleFile = await localFolder.CreateFileAsync("Roland INTEGRA_7.log",
                //           CreationCollisionOption.OpenIfExists);
                //}
                //await FileIO.AppendTextAsync(sampleFile, ticks > 0 ? ticks.ToString() + " " : "" + s + "\r\n");
                Debug.WriteLine(s);
            }
        }
    }

    class Player
    {
        public Boolean Playing { get; set; }
        public Boolean WasPlaying { get; set; }
        public Button btnPlayStop { get; set; }
        private CommonState commonState;

        public Player(CommonState commonState, ref Button btnPlayStop)
        {
            this.commonState = commonState;
            this.btnPlayStop = btnPlayStop;
            Playing = false;
            WasPlaying = false;
        }

        public Boolean ToneIsExpansion(String group)
        {
            return group != "PCM Synth Tone"
            && group != "PCM Drum Kit"
            && group != "SuperNATURAL Acoustic Tone"
            && group != "SuperNATURAL Synth Tone"
            && group != "SuperNATURAL Drum Kit";

        }

        public Boolean ToneIsPlayableExpansion(String group)
        {
            return group == "GM2 Tone (PCM Synth Tone)";
        }

        public void AllowPlay(Boolean allow = true)
        {
            if (allow)
            {
                btnPlayStop.IsEnabled = true;
            }
            else
            {
                btnPlayStop.IsEnabled = false;
                StopPlaying();
            }
        }

        public void Play()
        {
            if (Playing)
            {
                StopPlaying();
            }
            else
            {
                StartPlaying();
            }
        }

        public void StartPlaying()
        {
            byte[] address = new byte[] { 0x0f, 0x00, 0x20, 0x00 };
            byte[] data = new byte[] { (byte)(commonState.midi.MidiOutPortChannel + 1) };
            byte[] package = commonState.midi.SystemExclusiveDT1Message(address, data);
            commonState.midi.SendSystemExclusive(package);
            btnPlayStop.Content = "Stop";
            Playing = true;
        }

        public void StopPlaying()
        {
            byte[] address = new byte[] { 0x0f, 0x00, 0x20, 0x00 };
            byte[] data = new byte[] { 0x00 };
            byte[] package = commonState.midi.SystemExclusiveDT1Message(address, data);
            commonState.midi.SendSystemExclusive(package);
            btnPlayStop.Content = "Play";
            Playing = false;
        }
    }

    /// <summary>
    /// Use this class to preserve the application state over page navigations.
    /// Include it in all navigations.
    /// </summary>
    class CommonState
    {
        public enum SimpleToneTypes
        {
            UNKNOWN = 0xff,
            PCM_SYNTH_TONE = 0x00,
            PCM_DRUM_KIT = 0x10,
            SUPERNATURAL_ACOUSTIC_TONE = 0x02,
            SUPERNATURAL_SYNTH_TONE = 0x01,
            SUPERNATURAL_DRUM_KIT = 0x03,
        }

        public enum ToneTypes
        {
            UNKNOWN,
            USER_PCM_SYNTH_TONE,
            PRESET_PCM_SYNTH_TONE,
            GM2_TONE,
            USER_PCM_DRUM_KIT,
            PRESET_PCM_DRUM_KIT,
            GM2_DRUM_KIT,
            USER_SN_A_TONE,
            PRESET_SN_A_TONE,
            USER_SN_S_TONE,
            PRESET_SN_S_TONE,
            USER_SN_DRUM_KIT,
            PRESET_SN_DRUM_KIT,
            SRX0_PCM_TONE,
            SRX0_PCM_DRUM_KIT,
            SRX0_SN_TONE,
            SRX0_SN_DRUM_KIT,
            SRX0_GM2_TONE,
            SRX0_GM2_DRUM_KIT,
            SRX01_PCM_TONE,
            SRX01_PCM_DRUM_KIT,
            SRX01_SN_TONE,
            SRX01_SN_DRUM_KIT,
            SRX01_GM2_TONE,
            SRX01_GM2_DRUM_KIT,
            SRX02_PCM_TONE,
            SRX02_PCM_DRUM_KIT,
            SRX02_SN_TONE,
            SRX02_SN_DRUM_KIT,
            SRX02_GM2_TONE,
            SRX02_GM2_DRUM_KIT,
            SRX03_PCM_TONE,
            SRX03_PCM_DRUM_KIT,
            SRX03_SN_TONE,
            SRX03_SN_DRUM_KIT,
            SRX03_GM2_TONE,
            SRX03_GM2_DRUM_KIT,
            SRX04_PCM_TONE,
            SRX04_PCM_DRUM_KIT,
            SRX04_SN_TONE,
            SRX04_SN_DRUM_KIT,
            SRX04_GM2_TONE,
            SRX04_GM2_DRUM_KIT,
            SRX05_PCM_TONE,
            SRX05_PCM_DRUM_KIT,
            SRX05_SN_TONE,
            SRX05_SN_DRUM_KIT,
            SRX05_GM2_TONE,
            SRX05_GM2_DRUM_KIT,
            SRX06_PCM_TONE,
            SRX06_PCM_DRUM_KIT,
            SRX06_SN_TONE,
            SRX06_SN_DRUM_KIT,
            SRX06_GM2_TONE,
            SRX06_GM2_DRUM_KIT,
            SRX07_PCM_TONE,
            SRX07_PCM_DRUM_KIT,
            SRX07_SN_TONE,
            SRX07_SN_DRUM_KIT,
            SRX07_GM2_TONE,
            SRX07_GM2_DRUM_KIT,
            SRX08_PCM_TONE,
            SRX08_PCM_DRUM_KIT,
            SRX08_SN_TONE,
            SRX08_SN_DRUM_KIT,
            SRX08_GM2_TONE,
            SRX08_GM2_DRUM_KIT,
            SRX09_PCM_TONE,
            SRX09_PCM_DRUM_KIT,
            SRX09_SN_TONE,
            SRX09_SN_DRUM_KIT,
            SRX09_GM2_TONE,
            SRX09_GM2_DRUM_KIT,
            SRX10_PCM_TONE,
            SRX10_PCM_DRUM_KIT,
            SRX10_SN_TONE,
            SRX10_SN_DRUM_KIT,
            SRX10_GM2_TONE,
            SRX10_GM2_DRUM_KIT,
            SRX11_PCM_TONE,
            SRX11_PCM_DRUM_KIT,
            SRX11_SN_TONE,
            SRX11_SN_DRUM_KIT,
            SRX11_GM2_TONE,
            SRX11_GM2_DRUM_KIT,
            SRX12_PCM_TONE,
            SRX12_PCM_DRUM_KIT,
            SRX12_SN_TONE,
            SRX12_SN_DRUM_KIT,
            SRX12_GM2_TONE,
            SRX12_GM2_DRUM_KIT,
            EXSN1_PCM_TONE,
            EXSN1_PCM_DRUM_KIT,
            EXSN1_SN_TONE,
            EXSN1_SN_DRUM_KIT,
            EXSN1_GM2_TONE,
            EXSN1_GM2_DRUM_KIT,
            EXSN2_PCM_TONE,
            EXSN2_PCM_DRUM_KIT,
            EXSN2_SN_TONE,
            EXSN2_SN_DRUM_KIT,
            EXSN2_GM2_TONE,
            EXSN2_GM2_DRUM_KIT,
            EXSN3_PCM_TONE,
            EXSN3_PCM_DRUM_KIT,
            EXSN3_SN_TONE,
            EXSN3_SN_DRUM_KIT,
            EXSN3_GM2_TONE,
            EXSN3_GM2_DRUM_KIT,
            EXSN4_PCM_TONE,
            EXSN4_PCM_DRUM_KIT,
            EXSN4_SN_TONE,
            EXSN4_SN_DRUM_KIT,
            EXSN4_GM2_TONE,
            EXSN4_GM2_DRUM_KIT,
            EXSN5_PCM_TONE,
            EXSN5_PCM_DRUM_KIT,
            EXSN5_SN_TONE,
            EXSN5_SN_DRUM_KIT,
            EXSN5_GM2_TONE,
            EXSN5_GM2_DRUM_KIT,
            EXSN6_PCM_TONE,
            EXSN6_PCM_DRUM_KIT,
            EXSN6_SN_TONE,
            EXSN6_SN_DRUM_KIT,
            EXSN6_GM2_TONE,
            EXSN6_GM2_DRUM_KIT,
            EXPXM_PCM_TONE,
            EXPXM_PCM_DRUM_KIT,
            EXPXM_SN_TONE,
            EXPXM_SN_DRUM_KIT,
            EXPXM_GM2_TONE,
            EXPXM_GM2_DRUM_KIT,
        }

        public String command { get; set; }
        public MIDI midi { get; set; }
        public Tone currentTone { get; set; }
        public ToneList toneList { get; set; }
        public List<List<String>> toneNames { get; set; } // Will hold names of user tones.
        public List<String> keyNames { get; set; }  // Will hold names of keys for drum sets for edit.
        public DrumKeyAssignLists drumKeyAssignLists { get; set; }
        public Int32 PresetDrumKeyAssignListsCount { get; set; }
        public FavoritesList favoritesList { get; set; }
        public Player player { get; set; }
        public List<String> studioSetNames { get; set; }
        public ToneTypes ToneType { get; set; }
        public SimpleToneTypes SimpleToneType { get; set; }
        public String ToneSource {get;set;}
        public byte CurrentStudioSet { get; set; }
        public byte CurrentPart { get; set; }
        public byte[] PartChannels { get; set; }

        public CommonState(ref Button btnPlayStop)
        {
            command = "";
            midi = null;
            currentTone = null;
            toneList = new ToneList();
            toneNames = new List<List<string>>();
            player = new Player(this, ref btnPlayStop);
            for (byte i = 0; i < 5; i++)
            {
                toneNames.Add(new List<String>());
            }
            favoritesList = null;
            studioSetNames = null;
            CurrentPart = 0;
            PartChannels = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11,
                0x12, 0x13, 0x14, 0x15, 0xff }; // Last one is for EXT, which has no channel, but may be selected.
            keyNames = new List<String>();
            drumKeyAssignLists = new DrumKeyAssignLists();
            PresetDrumKeyAssignListsCount = drumKeyAssignLists.ToneNames.Count();
        }

        public void GetToneType(byte msb = 0xff, byte lsb = 0xff, byte pc = 0xff)
        {
            ToneType = ToneTypes.UNKNOWN;
            SimpleToneType = SimpleToneTypes.UNKNOWN;
            ToneSource = "";
            if (msb > 127 || lsb > 127 || pc > 127)
            {
                if (currentTone.Index > -1 && currentTone.Index < toneNames.Count())
                {
                    try
                    {
                        msb = (byte)UInt16.Parse(toneNames[currentTone.Index][4]);
                        lsb = (byte)UInt16.Parse(toneNames[currentTone.Index][5]);
                        pc = (byte)UInt16.Parse(toneNames[currentTone.Index][7]);
                    }
                    catch { }
                }
            }
            if (msb < 128 || lsb < 128 || pc < 128)
            {
                switch (msb)
                {
                    case 86:
                        if (lsb == 0)
                        {
                            ToneType = ToneTypes.USER_PCM_DRUM_KIT;
                            SimpleToneType = SimpleToneTypes.PCM_DRUM_KIT;
                            ToneSource = "User";
                        }
                        else
                        {
                            ToneType = ToneTypes.PRESET_PCM_DRUM_KIT;
                            SimpleToneType = SimpleToneTypes.PCM_DRUM_KIT;
                            ToneSource = "Int";
                        }
                        break;
                    case 87:
                        if (lsb < 2)
                        {
                            ToneType = ToneTypes.USER_PCM_SYNTH_TONE;
                            SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                            ToneSource = "User";
                        }
                        else
                        {
                            ToneType = ToneTypes.PRESET_PCM_SYNTH_TONE;
                            SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                            ToneSource = "Int";
                        }
                        break;
                    case 88:
                        switch (lsb)
                        {
                            case 0:
                                ToneType = ToneTypes.USER_SN_DRUM_KIT;
                                SimpleToneType = SimpleToneTypes.SUPERNATURAL_DRUM_KIT;
                                ToneSource = "User";
                                break;
                            case 64:
                                ToneType = ToneTypes.PRESET_SN_DRUM_KIT;
                                SimpleToneType = SimpleToneTypes.SUPERNATURAL_DRUM_KIT;
                                ToneSource = "";
                                break;
                            case 101:
                                ToneType = ToneTypes.EXSN6_SN_DRUM_KIT;
                                SimpleToneType = SimpleToneTypes.SUPERNATURAL_DRUM_KIT;
                                ToneSource = "Int";
                                break;
                        }
                        break;
                    case 89:
                        switch (lsb)
                        {
                            case 0:
                            case 1:
                                ToneType = ToneTypes.USER_SN_A_TONE;
                                SimpleToneType = SimpleToneTypes.SUPERNATURAL_ACOUSTIC_TONE;
                                ToneSource = "User";
                                break;
                            case 64:
                            case 65:
                                ToneType = ToneTypes.PRESET_SN_A_TONE;
                                SimpleToneType = SimpleToneTypes.SUPERNATURAL_ACOUSTIC_TONE;
                                ToneSource = "Int";
                                break;
                            case 96:
                                ToneType = ToneTypes.EXSN1_SN_TONE;
                                SimpleToneType = SimpleToneTypes.SUPERNATURAL_SYNTH_TONE;
                                ToneSource = "ExSN1";
                                break;
                            case 97:
                                ToneType = ToneTypes.EXSN2_SN_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "ExSN2";
                                break;
                            case 98:
                                ToneType = ToneTypes.EXSN3_SN_TONE;
                                SimpleToneType = SimpleToneTypes.SUPERNATURAL_SYNTH_TONE;
                                ToneSource = "ExSN3";
                                break;
                            case 99:
                                ToneType = ToneTypes.EXSN4_SN_TONE;
                                SimpleToneType = SimpleToneTypes.SUPERNATURAL_SYNTH_TONE;
                                ToneSource = "ExSN4";
                                break;
                            case 100:
                                ToneType = ToneTypes.EXSN5_SN_TONE;
                                SimpleToneType = SimpleToneTypes.SUPERNATURAL_SYNTH_TONE;
                                ToneSource = "ExSN5";
                                break;
                        }
                        break;
                    case 92:
                        switch (lsb)
                        {
                            case 0:
                                ToneType = ToneTypes.SRX01_PCM_DRUM_KIT;
                                SimpleToneType = SimpleToneTypes.PCM_DRUM_KIT;
                                ToneSource = "SRX01";
                                break;
                            case 2:
                                ToneType = ToneTypes.SRX03_PCM_DRUM_KIT;
                                SimpleToneType = SimpleToneTypes.PCM_DRUM_KIT;
                                ToneSource = "SRX03";
                                break;
                            case 4:
                                ToneType = ToneTypes.SRX05_PCM_DRUM_KIT;
                                SimpleToneType = SimpleToneTypes.PCM_DRUM_KIT;
                                ToneSource = "SRX05";
                                break;
                            case 7:
                                ToneType = ToneTypes.SRX06_PCM_DRUM_KIT;
                                SimpleToneType = SimpleToneTypes.PCM_DRUM_KIT;
                                ToneSource = "SRX06";
                                break;
                            case 11:
                                ToneType = ToneTypes.SRX07_PCM_DRUM_KIT;
                                SimpleToneType = SimpleToneTypes.PCM_DRUM_KIT;
                                ToneSource = "SRX07";
                                break;
                            case 15:
                                ToneType = ToneTypes.SRX08_PCM_DRUM_KIT;
                                SimpleToneType = SimpleToneTypes.PCM_DRUM_KIT;
                                ToneSource = "SRX08";
                                break;
                            case 19:
                                ToneType = ToneTypes.SRX09_PCM_DRUM_KIT;
                                SimpleToneType = SimpleToneTypes.PCM_DRUM_KIT;
                                ToneSource = "SRX09";
                                break;
                        }
                        break;
                    case 93:
                        switch (lsb)
                        {
                            case 0:
                                ToneType = ToneTypes.SRX01_PCM_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "SRX01";
                                break;
                            case 1:
                                ToneType = ToneTypes.SRX02_PCM_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "SRX02";
                                break;
                            case 2:
                                ToneType = ToneTypes.SRX03_PCM_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "SRX03";
                                break;
                            case 3:
                                ToneType = ToneTypes.SRX04_PCM_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "SRX04";
                                break;
                            case 4:
                            case 5:
                            case 6:
                                ToneType = ToneTypes.SRX05_PCM_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "SRX05";
                                break;
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                                ToneType = ToneTypes.SRX06_PCM_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "SRX06";
                                break;
                            case 11:
                            case 12:
                            case 13:
                            case 14:
                                ToneType = ToneTypes.SRX07_PCM_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "SRX07";
                                break;
                            case 15:
                            case 16:
                            case 17:
                            case 18:
                                ToneType = ToneTypes.SRX08_PCM_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "SRX08";
                                break;
                            case 19:
                            case 20:
                            case 21:
                            case 22:
                                ToneType = ToneTypes.SRX09_PCM_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "SRX09";
                                break;
                            case 23:
                                ToneType = ToneTypes.SRX10_PCM_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "SRX10";
                                break;
                            case 24:
                                ToneType = ToneTypes.SRX11_PCM_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "SRX11";
                                break;
                            case 26:
                                ToneType = ToneTypes.SRX12_PCM_TONE;
                                SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                                ToneSource = "SRX12";
                                break;
                        }
                        break;
                    case 95:
                        if (lsb < 4)
                        {
                            ToneType = ToneTypes.USER_SN_S_TONE;
                            SimpleToneType = SimpleToneTypes.SUPERNATURAL_SYNTH_TONE;
                            ToneSource = "User";
                        }
                        else
                        {
                            ToneType = ToneTypes.PRESET_SN_S_TONE;
                            SimpleToneType = SimpleToneTypes.SUPERNATURAL_SYNTH_TONE;
                            ToneSource = "Int";
                        }
                        break;
                    case 96:
                        ToneType = ToneTypes.EXPXM_PCM_DRUM_KIT;
                        SimpleToneType = SimpleToneTypes.PCM_DRUM_KIT;
                        ToneSource = "ExPCM";
                        break;
                    case 97:
                        ToneType = ToneTypes.EXPXM_PCM_TONE;
                        SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                        ToneSource = "ExPCM";
                        break;
                    case 120:
                        ToneType = ToneTypes.GM2_DRUM_KIT;
                        SimpleToneType = SimpleToneTypes.PCM_DRUM_KIT;
                        ToneSource = "GM2";
                        break;
                    case 121:
                        ToneType = ToneTypes.GM2_TONE;
                        SimpleToneType = SimpleToneTypes.PCM_SYNTH_TONE;
                        ToneSource = "GM2";
                        break;
                }
            }
        }
    }

    /// <summary>
    /// This class is _not_ full tone data.
    /// It holds indexes (optionally) and texts for the listviews
    /// and the index into ToneList, where full data can be fetched.
    /// </summary>
    public class Tone
    {
        public String Name { get; set; }
        public String Group { get; set; }
        public String Category { get; set; }
        public Int32 GroupIndex { get; set; }
        public Int32 CategoryIndex { get; set; }
        public Int32 ToneIndex { get; set; }
        public Int32 Index { get; set; }

        public Tone(Int32 GroupIndex = -1, Int32 CategoryIndex = -1, Int32 ToneIndex = -1, String Group = "", String Category = "", String Name = "", Int32 Index = -1)
        {
            this.GroupIndex = GroupIndex;
            this.CategoryIndex = CategoryIndex;
            this.ToneIndex = ToneIndex;
            this.Group = Group;
            this.Category = Category;
            this.Name = Name;
            this.Index = Index;
        }

        public Tone(List<String> tone)
        {
            this.GroupIndex = -1;
            this.CategoryIndex = -1;
            this.ToneIndex = -1;
            this.Group = tone[0];
            this.Category = tone[1];
            this.Name = tone[3];
            this.Index = Int32.Parse(tone[9]);
        }

        public Tone(Tone tone)
        {
            this.GroupIndex = tone.GroupIndex;
            this.CategoryIndex = tone.CategoryIndex;
            this.ToneIndex = tone.ToneIndex;
            this.Group = tone.Group;
            this.Category = tone.Category;
            this.Name = tone.Name;
            this.Index = tone.Index;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    class ToneCategories
    {
        public String[] pcmToneCategoryNames { get; }
        public String[] snsToneCategoryNames { get; }
        public String[] snaToneCategoryNames { get; }
        public byte[] pcmToneCategoryNameIndex { get; set; }
        public byte[] pcmToneCategoryIndex { get; set; }
        public byte[] snsToneCategoryNameIndex { get; set; }
        public byte[] snsToneCategoryIndex { get; set; }
        public byte[] snaToneCategoryNameIndex { get; set; }
        public byte[] snaToneCategoryIndex { get; set; }

        public ToneCategories()
        {
            pcmToneCategoryNames = new String[] { "No Assign", "AC.Piano", "", "", "E.Piano", "", "Organ", "", "", "Other Keyboards",
                "", "", "Accordeon/Harmonica", "", "Bell/Mallet", "", "Ac.Guitar", "E.Guitar", "Dist.Guitar", "Ac.Bass", "E.Bass",
                "Synth Bass", "Plucked/Stroke", "Strings", "", "", "Brass", "", "Wind", "Flute", "Sax", "Recorder", "Vox/Choir",
                "Synth Lead", "Synth Brass", "Synth Pad/Strings", "Synth Bellpad", "Synth PolyKey", "FX", "Synth Seq/Pop", "Phrase",
                "Pulsating", "Beat&Groove", "Hit", "SoundFx", "Drums", "Percussion", "Combination" };
            snsToneCategoryNames = new String[] { "No assign", "Ac.Piano", "", "", "", "E.Piano", "", "Organ", "", "", "", "Other Keyboards",
                "", "", "Accordion/Harmonica", "", "Bell/Mallet", "Ac.Guitar", "E.Guitar", "Dist.Guitar", "Ac.Bass", "E.Bass",
                "Synth Bass", "Plucked/Stroke", "Strings", "Brass", "", "Wind", "Flute", "Sax", "Recorder", "Vox/Choir", "",
                "Synth Lead", "Synth Brass", "Synth Pad/Strings", "Synth Bellpad", "Synth PolyKey", "FX", "Synth Seq/Pop", "Phrase",
                "Pulsating", "Beat&Groove", "Hit", "Sound FX", "Drums", "Percussion", "Combination" };
            snaToneCategoryNames = new String[] { "No assign", "Ac.Piano", "", "", "E.Piano", "", "Organ", "", "", "", "Other Keyboards",
                "", "", "Accordion/Harmonica", "", "Bell/Mallet", "Ac.Guitar", "E.Guitar", "Dist.Guitar", "Ac.Bass", "E.Bass",
                "Synth Bass", "Plucked/Stroke", "Strings", "", "", "Brass", "", "Wind", "Flute", "Sax", "Recorder", "Vox/Choir",
                "Synth Lead", "Synth Brass", "Synth Pad/Strings", "Synth Bellpad", "Synth PolyKey", "FX", "Synth Seq/Pop", "Phrase",
                "Pulsating", "Beat&Groove", "Hit", "Sound FX", "Drums", "Percussion", "Combination" };
            // When populating selector, empty strings will not be included. Thus the selected index does
            // not correctly map to a selected name in Integra-7. We need a translation table.
            // This will make a translation table that gets correct index for category names:
            byte count = 0;
            for (byte i = 0; i < pcmToneCategoryNames.Length; i++)
            {
                if (!String.IsNullOrEmpty(pcmToneCategoryNames[i]))
                {
                    count++;
                }
            }
            pcmToneCategoryIndex = new byte[count];
            pcmToneCategoryNameIndex = new byte[pcmToneCategoryNames.Length];
            count = 0;
            for (byte i = 0; i < pcmToneCategoryNames.Length; i++)
            {
                pcmToneCategoryNameIndex[i] = count;
                if (!String.IsNullOrEmpty(pcmToneCategoryNames[i]))
                {
                    pcmToneCategoryIndex[count++] = i;
                }
            }
            count = 0;
            for (byte i = 0; i < snsToneCategoryNames.Length; i++)
            {
                if (!String.IsNullOrEmpty(snsToneCategoryNames[i]))
                {
                    count++;
                }
            }
            snsToneCategoryIndex = new byte[count];
            snsToneCategoryNameIndex = new byte[snsToneCategoryNames.Length];
            count = 0;
            for (byte i = 0; i < snsToneCategoryNames.Length; i++)
            {
                snsToneCategoryNameIndex[i] = count;
                if (!String.IsNullOrEmpty(snsToneCategoryNames[i]))
                {
                    snsToneCategoryIndex[count++] = i;
                }
            }
            count = 0;
            for (byte i = 0; i < snaToneCategoryNames.Length; i++)
            {
                if (!String.IsNullOrEmpty(snaToneCategoryNames[i]))
                {
                    count++;
                }
            }
            snaToneCategoryIndex = new byte[count];
            snaToneCategoryNameIndex = new byte[snaToneCategoryNames.Length];
            count = 0;
            for (byte i = 0; i < snaToneCategoryNames.Length; i++)
            {
                snaToneCategoryNameIndex[i] = count;
                if (!String.IsNullOrEmpty(snaToneCategoryNames[i]))
                {
                    snaToneCategoryIndex[count++] = i;
                }
            }
       }
    }

    class FavoritesFolder
    {
        public String Name { get; set; }
        public List<Tone> FavoritesTones { get; set; }

        public FavoritesFolder(String Name = "")
        {
            this.Name = Name;
            FavoritesTones = new List<Tone>();
        }

        public Tone ByToneName(String Name)
        {
            foreach (Tone tone in FavoritesTones)
            {
                if (tone.Name == Name)
                {
                    return tone;
                }
            }
            return null;
        }
    }

    class FavoritesList
    {
        public List<FavoritesFolder> folders = null;
    }

    class Buddy
    {
        HBTrace t = new HBTrace("class Buddy");
        public byte Offset { get; set; }
        public byte ParameterNumber { get; set; }
        public Int16 ValueOffset { get; set; }
        public TextBox TextBox { get; set; }
        public CheckBox CheckBox { get; set; }
        public Double ValueMultiplier { get; set; }
        public HelpTag Tag { get; set; }

        public Buddy(byte Offset, Byte ParameterNumber, Int16 ValueOffset, TextBox TextBox, HelpTag Tag = null, Double ValueMultiplier = 1)
        {
            if (TextBox != null)
            {
                t.Trace("public Buddy (" + "byte" + Offset + ", " + "Byte" + ParameterNumber + ", " + "Int16" + ValueOffset + ", " + "TextBox" + TextBox.Text + ", " + "Double" + ValueMultiplier + ", " + ")");
            }
            else
            {
                t.Trace("public Buddy (" + "byte" + Offset + ", " + "Byte" + ParameterNumber + ", " + "Int16" + ValueOffset + ", " + "TextBox" + TextBox + ", " + "Double" + ValueMultiplier + ", " + ")");
            }
            this.Offset = Offset;
            this.ParameterNumber = ParameterNumber;
            this.ValueOffset = ValueOffset;
            this.TextBox = TextBox;
            this.CheckBox = null;
            this.ValueMultiplier = ValueMultiplier;
            this.Tag = Tag;
        }

        public Buddy(byte Offset, Byte ParameterNumber, Int16 ValueOffset, CheckBox CheckBox, HelpTag Tag = null, Double ValueMultiplier = 1)
        {
            if (CheckBox != null)
            {
                t.Trace("public Buddy (" + "byte" + Offset + ", " + "Byte" + ParameterNumber + ", " + "Int16" + ValueOffset + ", " + "CheckBox" + CheckBox.IsChecked.ToString() + ")");
            }
            else
            {
                t.Trace("public Buddy (" + "byte" + Offset + ", " + "Byte" + ParameterNumber + ", " + "Int16" + ValueOffset + ", " + "CheckBox" + CheckBox + ")");
            }
            this.Offset = Offset;
            this.ParameterNumber = ParameterNumber;
            this.ValueOffset = ValueOffset;
            this.TextBox = null;
            this.CheckBox = CheckBox;
            this.ValueMultiplier = ValueMultiplier;
            this.Tag = Tag;
        }
    }

    enum ProgramType
    {
        PCM_SYNTH_TONE,
        PCM_DRUM_KIT,
        SUPERNATURAL_ACOUSTIC_TONE,
        SUPERNATURAL_SYNTH_TONE,
        SUPERNATURAL_DRUM_KIT,
    }

    enum ParameterPage
    {
        COMMON,
        COMMONMFX,
        PMT,
        COMMONCOMPEQ,
        PARTIAL,
        MISC,
        COMMON2,
    }

    class SelectedTone
    {
        HBTrace t = new HBTrace("class SelectedTone");
        public byte BankMSB { get; set; }
        public byte BankLSB { get; set; }
        public byte Program { get; set; }
        public ProgramType ProgramType { get; }
        public String ProgramBank { get; }
        public UInt32 Id { get; }

        public SelectedTone(byte MSB, byte LSB, byte Program)
        {
            t.Trace("public SelectedTone (" + "byte" + MSB + ", " + "byte" + LSB + ", " + "byte" + Program + ", " + ")");
            this.BankMSB = MSB;
            this.BankLSB = LSB;
            this.Program = Program;
            Id = (UInt32)(BankMSB * 128 * 128 + BankLSB * 128 + Program);
            switch (MSB)
            {
                case 86:
                    ProgramType = ProgramType.PCM_DRUM_KIT;
                    switch (BankLSB)
                    {
                        case 0:
                            ProgramBank = "User PCM-D";
                            break;
                        case 64:
                            ProgramBank = "Preset PCM-D";
                            break;
                        default:
                            ProgramBank = "";
                            break;
                    }
                    break;
                case 87:
                    ProgramType = ProgramType.PCM_SYNTH_TONE;
                    if (BankLSB < 3)
                    {
                        ProgramBank = "User PCM-S";
                    }
                    else
                    { 
                        ProgramBank = "Preset  PCM-S";
                    }
                    break;
                case 88:
                    ProgramType = ProgramType.SUPERNATURAL_DRUM_KIT;
                    switch (BankLSB)
                    {
                        case 0:
                            ProgramBank = "User SN-D";
                            break;
                        case 64:
                            ProgramBank = "Preset SN-D";
                            break;
                        case 101:
                            ProgramBank = "ExSN6";
                            break;
                        default:
                            ProgramBank = "";
                            break;
                    }
                    break;
                case 89:
                    ProgramType = ProgramType.SUPERNATURAL_ACOUSTIC_TONE;
                    if (BankLSB < 2)
                    {
                        ProgramBank = "User SN-A";
                    }
                    else if (BankLSB == 64 || BankLSB == 65)
                    {
                        ProgramBank = "Preset SN-A";
                    }
                    else
                    {
                        switch (BankLSB)
                        {
                            case 96:
                                ProgramBank = "ExSN1 SN-T";
                                break;
                            case 97:
                                ProgramBank = "ExSN2 SN-T";
                                break;
                            case 98:
                                ProgramBank = "ExSN3 SN-T";
                                break;
                            case 99:
                                ProgramBank = "ExSN4 SN-T";
                                break;
                            case 100:
                                ProgramBank = "ExSN5 SN-T";
                                break;
                            case 101:
                                ProgramBank = "ExSN6 SN-T";
                                break;
                            default:
                                ProgramBank = "";
                                break;
                        }
                    }
                    break;
                case 92:
                    ProgramType = ProgramType.PCM_DRUM_KIT;
                    switch (BankLSB)
                    {
                        case 0:
                            ProgramBank = "SRX01 PCM-D";
                            break;
                        case 2:
                            ProgramBank = "SRX03 PCM-D";
                            break;
                        case 4:
                            ProgramBank = "SRX05 PCM-D";
                            break;
                        case 7:
                            ProgramBank = "SRX06 PCM-D";
                            break;
                        case 15:
                            ProgramBank = "SRX08 PCM-D";
                            break;
                        case 19:
                            ProgramBank = "SRX09 PCM-D";
                            break;
                        default:
                            ProgramBank = "";
                            break;
                    }
                    break;
                case 93:
                    ProgramType = ProgramType.PCM_SYNTH_TONE;
                    switch (BankLSB)
                    {
                        case 0:
                            ProgramBank = "SRX01 PCM-T";
                            break;
                        case 1:
                            ProgramBank = "SRX02 PCM-T";
                            break;
                        case 2:
                            ProgramBank = "SRX03 PCM-T";
                            break;
                        case 3:
                            ProgramBank = "SRX04 PCM-T";
                            break;
                        case 4:
                        case 5:
                        case 6:
                            ProgramBank = "SRX05 PCM-T";
                            break;
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            ProgramBank = "SRX06 PCM-T";
                            break;
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                            ProgramBank = "SRX07 PCM-T";
                            break;
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                            ProgramBank = "SRX08 PCM-T";
                            break;
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                            ProgramBank = "SRX09 PCM-T";
                            break;
                        case 23:
                            ProgramBank = "SRX10 PCM-T";
                            break;
                        case 24:
                            ProgramBank = "SRX11 PCM-T";
                            break;
                        case 26:
                            ProgramBank = "SRX12 PCM-T";
                            break;
                        default:
                            ProgramBank = "";
                            break;
                    }
                    break;
                case 95:
                    ProgramType = ProgramType.SUPERNATURAL_SYNTH_TONE;
                    if (BankLSB < 4)
                    {
                        ProgramBank = "User SN-S";
                    }
                    else
                    {
                        ProgramBank = "Preset SN-S";
                    }
                    break;
                case 120:
                    ProgramType = ProgramType.PCM_DRUM_KIT;
                    ProgramBank = "GM2 Drum Kit";
                    break;
                case 121:
                    ProgramType = ProgramType.PCM_SYNTH_TONE;
                    ProgramBank = "GM2 Tone";
                    break;
                default:
                    ProgramType = ProgramType.PCM_SYNTH_TONE;
                    ProgramBank = "";
                    break;
            }
        }
    }

    class PCMWaveNames
    {
        HBTrace t = new HBTrace("class PCMWaveNames");
        public String[] Names { get; set; }

        public PCMWaveNames()
        {
            t.Trace("public PCMWaveNames()");
            Names = new String[] { "Off", "StGrand pA L", "StGrand pA R", "StGrand pB L", "StGrand pB R", "StGrand pC L", "StGrand pC R", "StGrand fA L",
                "StGrand fA R", "StGrand fB L", "StGrand fB R", "StGrand fC L", "StGrand fC R", "Ac Piano2 pA", "Ac Piano2 pB", "Ac Piano2 pC",
                "Ac Piano2 fA", "Ac Piano2 fB", "Ac Piano2 fC", "Ac Piano1 A", "Ac Piano1 B", "Ac Piano1 C", "Piano Thump", "Piano Up TH", "Piano Atk",
                "MKS-20 P3 A", "MKS-20 P3 B", "MKS-20 P3 C", "SA Rhodes 1A", "SA Rhodes 1B", "SA Rhodes 1C", "SA Rhodes 2A", "SA Rhodes 2B",
                "SA Rhodes 2C", "Dyn Rhd mp A", "Dyn Rhd mp B", "Dyn Rhd mp C", "Dyn Rhd mf A", "Dyn Rhd mf B", "Dyn Rhd mf C", "Dyn Rhd ff A",
                "Dyn Rhd ff B", "Dyn Rhd ff C", "Wurly soft A", "Wurly soft B", "Wurly soft C", "Wurly hard A", "Wurly hard B", "Wurly hard C",
                "E.Piano 1A", "E.Piano 1B", "E.Piano 1C", "E.Piano 2A", "E.Piano 2B", "E.Piano 2C", "E.Piano 3A", "E.Piano 3B", "E.Piano 3C",
                "MK-80 EP A", "MK-80 EP B", "MK-80 EP C", "EP Hard", "EP Distone", "Clear Keys", "D-50 EP A", "D-50 EP B", "D-50 EP C", "Celesta",
                "Music Box", "Music Box 2", "Clav 1A", "Clav 1B", "Clav 1C", "Clav 2A", "Clav 2B", "Clav 2C", "Clav 3A", "Clav 3B", "Clav 3C",
                "Clav 4A", "Clav 4B", "Clav 4C", "Clav Wave", "MIDI Clav", "HarpsiWave A", "HarpsiWave B", "HarpsiWave C", "Jazz Organ 1",
                "Jazz Organ 2", "Organ 1", "Organ 2", "Organ 3", "Organ 4", "60's Organ1", "60's Organ2", "60's Organ3", "60's Organ4", "Full Organ",
                "Full Draw", "Rock Organ", "RockOrg1 A L", "RockOrg1 A R", "RockOrg1 B L", "RockOrg1 B R", "RockOrg1 C L", "RockOrg1 C R",
                "RockOrg2 A L", "RockOrg2 A R", "RockOrg2 B L", "RockOrg2 B R", "RockOrg2 C L", "RockOrg2 C R", "RockOrg3 A L", "RockOrg3 A R",
                "RockOrg3 B L", "RockOrg3 B R", "RockOrg3 C L", "RockOrg3 C R", "Dist. Organ", "Rot.Org Slw", "Rot.Org Fst", "Pipe Organ",
                "Soft Nylon A", "Soft Nylon B", "Soft Nylon C", "Nylon Gtr A", "Nylon Gtr B", "Nylon Gtr C", "Nylon Str", "6-Str Gtr A", "6-Str Gtr B",
                "6-Str Gtr C", "StlGtr mp A", "StlGtr mp B", "StlGtr mp C", "StlGtr mf A", "StlGtr mf B", "StlGtr mf C", "StlGtr ff A", "StlGtr ff B",
                "StlGtr ff C", "StlGtr sld A", "StlGtr sld B", "StlGtr sld C", "StlGtr Hrm A", "StlGtr Hrm B", "StlGtr Hrm C", "Gtr Harm A",
                "Gtr Harm B", "Gtr Harm C", "Jazz Gtr A", "Jazz Gtr B", "Jazz Gtr C", "LP Rear A", "LP Rear B", "LP Rear C", "Rock lead 1",
                "Rock lead 2", "Comp Gtr A", "Comp Gtr B", "Comp Gtr C", "Comp Gtr A+", "Mute Gtr 1", "Mute Gtr 2A", "Mute Gtr 2B", "Mute Gtr 2C",
                "Muters", "Pop Strat A", "Pop Strat B", "Pop Strat C", "JC Strat A", "JC Strat B", "JC Strat C", "JC Strat A+", "JC Strat B+",
                "JC Strat C+", "Clean Gtr A", "Clean Gtr B", "Clean Gtr C", "Stratus A", "Stratus B", "Stratus C", "Scrape Gut", "Strat Sust",
                "Strat Atk", "OD Gtr A", "OD Gtr B", "OD Gtr C", "OD Gtr A+", "Heavy Gtr A", "Heavy Gtr B", "Heavy Gtr C", "Heavy Gtr A+",
                "Heavy Gtr B+", "Heavy Gtr C+", "PowerChord A", "PowerChord B", "PowerChord C", "EG Harm", "Gt.FretNoise", "Syn Gtr A", "Syn Gtr B",
                "Syn Gtr C", "Harp 1A", "Harp 1B", "Harp 1C", "Harp Harm", "Pluck Harp", "Banjo A", "Banjo B", "Banjo C", "Sitar A", "Sitar B",
                "Sitar C", "E.Sitar A", "E.Sitar B", "E.Sitar C", "Santur A", "Santur B", "Santur C", "Dulcimer A", "Dulcimer B", "Dulcimer C",
                "Shamisen A", "Shamisen B", "Shamisen C", "Koto A", "Koto B", "Koto C", "Taishokoto A", "Taishokoto B", "Taishokoto C", "Pick Bass A",
                "Pick Bass B", "Pick Bass C", "Fingerd Bs A", "Fingerd Bs B", "Fingerd Bs C", "E.Bass", "P.Bass 1", "P.Bass 2", "Stick", "Fretless A",
                "Fretless B", "Fretless C", "Fretless 2A", "Fretless 2B", "Fretless 2C", "UprightBs 1", "UprightBs 2A", "UprightBs 2B", "UprightBs 2C",
                "Ac.Bass A", "Ac.Bass B", "Ac.Bass C", "Slap Bass 1", "Slap & Pop", "Slap Bass 2", "Slap Bass 3", "Jz.Bs Thumb", "Jz.Bs Slap 1",
                "Jz.Bs Slap 2", "Jz.Bs Slap 3", "Jz.Bs Pop", "Funk Bass1", "Funk Bass2", "Syn Bass A", "Syn Bass C", "Syn Bass", "Syn Bass 2 A",
                "Syn Bass 2 B", "Syn Bass 2 C", "Mini Bs 1A", "Mini Bs 1B", "Mini Bs 1C", "Mini Bs 2", "Mini Bs 2+", "MC-202 Bs A", "MC-202 Bs B",
                "MC-202 Bs C", "Hollow Bs", "Flute 1A", "Flute 1B", "Flute 1C", "Jazz Flute A", "Jazz Flute B", "Jazz Flute C", "Flute Tone",
                "Piccolo A", "Piccolo B", "Piccolo C", "Blow Pipe", "Pan Pipe", "BottleBlow", "Rad Hose", "Shakuhachi", "Shaku Atk", "Flute Push",
                "Clarinet A", "Clarinet B", "Clarinet C", "Oboe mf A", "Oboe mf B", "Oboe mf C", "Oboe f A", "Oboe f B", "Oboe f C", "E.Horn A",
                "E.Horn B", "E.Horn C", "Bassoon A", "Bassoon B", "Bassoon C", "T_Recorder A", "T_Recorder B", "T_Recorder C", "Sop.Sax A", "Sop.Sax B",
                "Sop.Sax C", "Sop.Sax mf A", "Sop.Sax mf B", "Sop.Sax mf C", "Alto mp A", "Alto mp B", "Alto mp C", "Alto Sax 1A", "Alto Sax 1B",
                "Alto Sax 1C", "T.Breathy A", "T.Breathy B", "T.Breathy C", "SoloSax A", "SoloSax B", "SoloSax C", "Tenor Sax A", "Tenor Sax B",
                "Tenor Sax C", "T.Sax mf A", "T.Sax mf B", "T.Sax mf C", "Bari.Sax f A", "Bari.Sax f B", "Bari.Sax f C", "Bari.Sax A", "Bari.Sax B",
                "Bari.Sax C", "Syn Sax", "Chanter", "Harmonica A", "Harmonica B", "Harmonica C", "OrcUnisonA L", "OrcUnisonA R", "OrcUnisonB L",
                "OrcUnisonB R", "OrcUnisonC L", "OrcUnisonC R", "BrassSectA L", "BrassSectA R", "BrassSectB L", "BrassSectB R", "BrassSectC L",
                "BrassSectC R", "Tpt Sect. A", "Tpt Sect. B", "Tpt Sect. C", "Tb Sect A", "Tb Sect B", "Tb Sect C", "T.Sax Sect A", "T.Sax Sect B",
                "T.Sax Sect C", "Flugel A", "Flugel B", "Flugel C", "FlugelWave", "Trumpet 1A", "Trumpet 1B", "Trumpet 1C", "Trumpet 2A", "Trumpet 2B",
                "Trumpet 2C", "HarmonMute1A", "HarmonMute1B", "HarmonMute1C", "Trombone 1", "Trombone 2 A", "Trombone 2 B", "Trombone 2 C",
                "Tuba A", "Tuba B", "Tuba C", "French 1A", "French 1C", "F.Horns A", "F.Horns B", "F.Horns C", "Violin A", "Violin B", "Violin C",
                "Violin 2 A", "Violin 2 B", "Violin 2 C", "Cello A", "Cello B", "Cello C", "Cello 2 A", "Cello 2 B", "Cello 2 C", "Cello Wave", "Pizz",
                "STR Attack A", "STR Attack B", "STR Attack C", "DolceStr.A L", "DolceStr.A R", "DolceStr.B L", "DolceStr.B R", "DolceStr.C L",
                "DolceStr.C R", "JV Strings L", "JV Strings R", "JV Strings A", "JV Strings C", "JP Strings1A", "JP Strings1B", "JP Strings1C",
                "JP Strings2A", "JP Strings2B", "JP Strings2C", "PWM", "Pulse Mod", "Soft Pad A", "Soft Pad B", "Soft Pad C", "Fantasynth A",
                "Fantasynth B", "Fantasynth C", "D-50 HeavenA", "D-50 HeavenB", "D-50 HeavenC", "Fine Wine", "D-50 Brass A", "D-50 Brass B",
                "D-50 Brass C", "D-50 BrassA+", "Doo", "Pop Voice", "Syn Vox 1", "Syn Vox 2", "Voice Aahs A", "Voice Aahs B", "Voice Aahs C",
                "Voice Oohs1A", "Voice Oohs1B", "Voice Oohs1C", "Voice Oohs2A", "Voice Oohs2B", "Voice Oohs2C", "Choir 1A", "Choir 1B", "Choir 1C",
                "Oohs Chord L", "Oohs Chord R", "Male Ooh A", "Male Ooh B", "Male Ooh C", "Org Vox A", "Org Vox B", "Org Vox C", "Org Vox", "ZZZ Vox",
                "Bell VOX", "Kalimba", "JD Kalimba", "Klmba Atk", "Wood Crak", "Block", "Gamelan 1", "Gamelan 2", "Gamelan 3", "Log Drum", "Hooky",
                "Tabla", "Marimba Wave", "Xylo", "Xylophone", "Vibes", "Bottle Hit", "Glockenspiel", "Tubular", "Steel Drums", "Pole lp",
                "Fanta Bell A", "Fanta Bell B", "Fanta Bell C", "FantaBell A+", "Org Bell", "AgogoBells", "FingerBell", "DIGI Bell 1", "DIGI Bell 1+",
                "JD Cowbell", "Bell Wave", "Chime", "Crystal", "2.2 Bellwave", "2.2 Vibwave", "Digiwave", "DIGI Chime", "JD DIGIChime", "BrightDigi",
                "Can Wave 1", "Can Wave 2", "Vocal Wave", "Wally Wave", "Brusky lp", "Wave Scan", "Wire String", "Nasty", "Wave Table", "Klack Wave",
                "Spark VOX", "JD Spark VOX", "Cutters", "EML 5th", "MMM VOX", "Lead Wave", "Synth Reed", "Synth Saw 1", "Synth Saw 2", "Syn Saw 2inv",
                "Synth Saw 3", "JD Syn Saw 2", "FAT Saw", "JP-8 Saw A", "JP-8 Saw B", "JP-8 Saw C", "P5 Saw A", "P5 Saw B", "P5 Saw C", "P5 Saw2 A",
                "P5 Saw2 B", "P5 Saw2 C", "D-50 Saw A", "D-50 Saw B", "D-50 Saw C", "Synth Square", "JP-8 SquareA", "JP-8 SquareB", "JP-8 SquareC",
                "DualSquare A", "DualSquare C", "DualSquareA+", "JD SynPulse1", "JD SynPulse2", "JD SynPulse3", "JD SynPulse4", "Synth Pulse1",
                "Synth Pulse2", "JD SynPulse5", "Sync Sweep", "Triangle", "JD Triangle", "Sine", "Metal Wind", "Wind Agogo", "Feedbackwave",
                "Spectrum", "CrunchWind", "ThroatWind", "Pitch Wind", "JD Vox Noise", "Vox Noise", "BreathNoise", "Voice Breath", "White Noise",
                "Pink Noise", "Rattles", "Ice Rain", "Tin Wave", "Anklungs", "Wind Chimes", "Orch. Hit", "Tekno Hit", "Back Hit", "Philly Hit",
                "Scratch 1", "Scratch 2", "Scratch 3", "Shami", "Org Atk 1", "Org Atk 2", "Sm Metal", "StrikePole", "Thrill", "Switch", "Tuba Slap",
                "Plink", "Plunk", "EP Atk", "TVF_Trig", "Org Click", "Cut Noiz", "Bass Body", "Flute Click", "Gt&BsNz MENU", "Ac.BassNz 1",
                "Ac.BassNz 2", "El.BassNz 1", "El.BassNz 2", "DistGtrNz 1", "DistGtrNz 2", "DistGtrNz 3", "DistGtrNz 4", "SteelGtrNz 1", "SteelGtrNz 2",
                "SteelGtrNz 3", "SteelGtrNz 4", "SteelGtrNz 5", "SteelGtrNz 6", "SteelGtrNz 7", "Sea", "Thunder", "Windy", "Stream", "Bubble", "Bird",
                "Dog Bark", "Horse", "Telephone 1", "Telephone 2", "Creak", "Door Slam", "Engine", "Car Stop", "Car Pass", "Crash", "Gun Shot", "Siren",
                "Train", "Jetplane", "Starship", "Breath", "Laugh", "Scream", "Punch", "Heart", "Steps", "Machine Gun", "Laser", "Thunder 2",
                "AmbientSN pL", "AmbientSN pR", "AmbientSN fL", "AmbientSN fR", "Wet SN p L", "Wet SN p R", "Wet SN f L", "Wet SN f R", "Dry SN p",
                "Dry SN f", "Sharp SN", "Piccolo SN", "Maple SN", "Old Fill SN", "70s SN", "SN Roll", "Natural SN1", "Natural SN2", "Ballad SN",
                "Rock SN p L", "Rock SN p R", "Rock SN mf L", "Rock SN mf R", "Rock SN f L", "Rock SN f R", "Rock Rim p L", "Rock Rim p R",
                "Rock Rim mfL", "Rock Rim mfR", "Rock Rim f L", "Rock Rim f R", "Rock Gst L", "Rock Gst R", "Snare Ghost", "Jazz SN p L",
                "Jazz SN p R", "Jazz SN mf L", "Jazz SN mf R", "Jazz SN f L", "Jazz SN f R", "Jazz SN ff L", "Jazz SN ff R", "Jazz Rim p L",
                "Jazz Rim p R", "Jazz Rim mfL", "Jazz Rim mfR", "Jazz Rim f L", "Jazz Rim f R", "Jazz Rim ffL", "Jazz Rim ffR", "Brush Slap",
                "Brush Swish", "Jazz Swish p", "Jazz Swish f", "909 SN 1", "909 SN 2", "808 SN", "Rock Roll L", "Rock Roll R", "Jazz Roll",
                "Brush Roll", "Dry Stick", "Dry Stick 2", "Side Stick", "Woody Stick", "RockStick pL", "RockStick pR", "RockStick fL", "RockStick fR",
                "Dry Kick", "Maple Kick", "Rock Kick p", "Rock Kick mf", "Rock Kick f", "Jazz Kick p", "Jazz Kick mf", "Jazz Kick f", "Jazz Kick",
                "Pillow Kick", "JazzDry Kick", "Lite Kick", "Old Kick", "Hybrid Kick", "Hybrid Kick2", "Verb Kick", "Round Kick", "MplLmtr Kick",
                "70s Kick 1", "70s Kick 2", "Dance Kick", "808 Kick", "909 Kick 1", "909 Kick 2", "Rock TomL1 p", "Rock TomL2 p", "Rock Tom M p",
                "Rock Tom H p", "Rock TomL1 f", "Rock TomL2 f", "Rock Tom M f", "Rock Tom H f", "Rock Flm L1", "Rock Flm L2", "Rock Flm M",
                "Rock Flm H", "Jazz Tom L p", "Jazz Tom M p", "Jazz Tom H p", "Jazz Tom L f", "Jazz Tom M f", "Jazz Tom H f", "Jazz Flm L",
                "Jazz Flm M", "Jazz Flm H", "Maple Tom 1", "Maple Tom 2", "Maple Tom 3", "Maple Tom 4", "808 Tom", "Verb Tom Hi", "Verb Tom Lo",
                "Dry Tom Hi", "Dry Tom Lo", "Rock ClHH1 p", "Rock ClHH1mf", "Rock ClHH1 f", "Rock ClHH2 p", "Rock ClHH2mf", "Rock ClHH2 f",
                "Jazz ClHH1 p", "Jazz ClHH1mf", "Jazz ClHH1 f", "Jazz ClHH2 p", "Jazz ClHH2mf", "Jazz ClHH2 f", "Cl HiHat 1", "Cl HiHat 2",
                "Cl HiHat 3", "Cl HiHat 4", "Cl HiHat 5", "Rock OpHH p", "Rock OpHH f", "Jazz OpHH p", "Jazz OpHH mf", "Jazz OpHH f", "Op HiHat",
                "Op HiHat 2", "Rock PdHH p", "Rock PdHH f", "Jazz PdHH p", "Jazz PdHH f", "Pedal HiHat", "Pedal HiHat2", "Dance Cl HH", "909 NZ HiHat",
                "70s Cl HiHat", "70s Op HiHat", "606 Cl HiHat", "606 Op HiHat", "909 Cl HiHat", "909 Op HiHat", "808 Claps", "HumanClapsEQ",
                "Tight Claps", "Hand Claps", "Finger Snaps", "Rock RdCym1p", "Rock RdCym1f", "Rock RdCym2p", "Rock RdCym2f", "Jazz RdCym p",
                "Jazz RdCymmf", "Jazz RdCym f", "Ride 1", "Ride 2", "Ride Bell", "Rock CrCym1p", "Rock CrCym1f", "Rock CrCym2p", "Rock CrCym2f",
                "Rock Splash", "Jazz CrCym p", "Jazz CrCym f", "Crash Cymbal", "Crash 1", "Rock China", "China Cym", "Cowbell", "Wood Block", "Claves",
                "Bongo Hi", "Bongo Lo", "Cga Open Hi", "Cga Open Lo", "Cga Mute Hi", "Cga Mute Lo", "Cga Slap", "Timbale", "Cabasa Up", "Cabasa Down",
                "Cabasa Cut", "Maracas", "Long Guiro", "Tambourine 1", "Tambourine 2", "Open Triangl", "Cuica", "Vibraslap", "Timpani", "Timp3 pp",
                "Timp3 mp", "Applause", "Syn FX Loop", "Loop 1", "Loop 2", "Loop 3", "Loop 4", "Loop 5", "Loop 6", "Loop 7", "R8 Click", "Metronome 1",
                "Metronome 2", "MC500 Beep 1", "MC500 Beep 2", "Low Saw", "Low Saw inv", "Low P5 Saw", "Low Pulse 1", "Low Pulse 2", "Low Square",
                "Low Sine", "Low Triangle", "Low White NZ", "Low Pink NZ", "DC", "REV Orch.Hit", "REV TeknoHit", "REV Back Hit", "REV PhillHit",
                "REV Steel DR", "REV Tin Wave", "REV AmbiSNpL", "REV AmbiSNpR", "REV AmbiSNfL", "REV AmbiSNfR", "REV Wet SNpL", "REV Wet SNpR",
                "REV Wet SNfL", "REV Wet SNfR", "REV Dry SN", "REV PiccloSN", "REV Maple SN", "REV OldFilSN", "REV 70s SN", "REV SN Roll",
                "REV NatrlSN1", "REV NatrlSN2", "REV BalladSN", "REV RkSNpL", "REV RkSNpR", "REV RkSNmfL", "REV RkSNmfR", "REV RkSNfL", "REV RkSNfR",
                "REV RkRimpL", "REV RkRimpR", "REV RkRimmfL", "REV RkRimmfR", "REV RkRimfL", "REV RkRimfR", "REV RkGstL", "REV RkGstR", "REV SnareGst",
                "REV JzSNpL", "REV JzSNpR", "REV JzSNmfL", "REV JzSNmfR", "REV JzSNfL", "REV JzSNfR", "REV JzSNffL", "REV JzSNffR", "REV JzRimpL",
                "REV JzRimpR", "REV JzRimmfL", "REV JzRimmfR", "REV JzRimfL", "REV JzRimfR", "REV JzRimffL", "REV JzRimffR", "REV Brush 1",
                "REV Brush 2", "REV Brush 3", "REV JzSwish1", "REV JzSwish2", "REV 909 SN 1", "REV 909 SN 2", "REV RkRoll L", "REV RkRoll R",
                "REV JzRoll", "REV Dry Stk", "REV DrySick", "REV Side Stk", "REV Wdy Stk", "REV RkStk1L", "REV RkStk1R", "REV RkStk2L", "REV RkStk2R",
                "REV Thrill", "REV Dry Kick", "REV Mpl Kick", "REV RkKik p", "REV RkKik mf", "REV RkKik f", "REV JzKik p", "REV JzKik mf",
                "REV JzKik f", "REV Jaz Kick", "REV Pillow K", "REV Jz Dry K", "REV LiteKick", "REV Old Kick", "REV Hybrid K", "REV HybridK2",
                "REV 70s K 1", "REV 70s K 2", "REV Dance K", "REV 909 K 2", "REV RkTomL1p", "REV RkTomL2p", "REV RkTomM p", "REV RkTomH p",
                "REV RkTomL1f", "REV RkTomL2f", "REV RkTomM f", "REV RkTomH f", "REV RkFlmL1", "REV RkFlmL2", "REV RkFlm M", "REV RkFlm H",
                "REV JzTomL p", "REV JzTomM p", "REV JzTomH p", "REV JzTomL f", "REV JzTomM f", "REV JzTomH f", "REV JzFlm L", "REV JzFlm M",
                "REV JzFlm H", "REV MplTom2", "REV MplTom4", "REV 808Tom", "REV VerbTomH", "REV VerbTomL", "REV DryTom H", "REV DryTom M",
                "REV RkClH1 p", "REV RkClH1mf", "REV RkClH1 f", "REV RkClH2 p", "REV RkClH2mf", "REV RkClH2 f", "REV JzClH1 p", "REV JzClH1mf",
                "REV JzClH1 f", "REV JzClH2 p", "REV JzClH2mf", "REV JzClH2 f", "REV Cl HH 1", "REV Cl HH 2", "REV Cl HH 3", "REV Cl HH 4",
                "REV Cl HH 5", "REV RkOpHH p", "REV RkOpHH f", "REV JzOpHH p", "REV JzOpHHmf", "REV JzOpHH f", "REV Op HiHat", "REV OpHiHat2",
                "REV RkPdHH p", "REV RkPdHH f", "REV JzPdHH p", "REV JzPdHH f", "REV PedalHH", "REV PedalHH2", "REV Dance HH", "REV 70s ClHH",
                "REV 70s OpHH", "REV 606 ClHH", "REV 606 OpHH", "REV 909 NZHH", "REV 909 OpHH", "REV HClapsEQ", "REV TghtClps", "REV FingSnap",
                "REV RealCLP", "REV RkRCym1p", "REV RkRCym1f", "REV RkRCym2p", "REV RkRCym2f", "REV JzRCym p", "REV JzRCymmf", "REV JzRCym f",
                "REV Ride 1", "REV Ride 2", "REV RideBell", "REV RkCCym1p", "REV RkCCym1f", "REV RkCCym2p", "REV RkCCym2f", "REV RkSplash",
                "REV JzCCym p", "REV JzCCym f", "REV CrashCym", "REV Crash 1", "REV RkChina", "REV China", "REV Cowbell", "REV WoodBlck",
                "REV Claves", "REV Conga", "REV Timbale", "REV Maracas", "REV Guiro", "REV Tamb 1", "REV Tamb 2", "REV Cuica", "REV Timpani",
                "REV Timp3 pp", "REV Timp3 mp", "REV Metro" };

            for (UInt16 i = 0; i < Names.Length; i++)
            {
                Names[i] = i.ToString() + ": " + Names[i];
            }
        }
    }

    class SuperNaturalSynthToneWaveNames
    {
        HBTrace t = new HBTrace("class PCMWaveNames");
        public String[] Names { get; set; }

        public SuperNaturalSynthToneWaveNames()
        {
            Names = new String[] { "JP-8 Saw", "Syn Saw", "WaveMG Saw", "1GR-300 Saw",
            "P5 Saw", "MG Saw 2", "Calc.Saw", "Calc.Saw inv", "Digital Saw", "JD Fat Saw",
            "Unison Saw", "DistSaw Wave", "JP-8 Pls 05", "Pulse Wave", "Ramp Wave 1",
            "Ramp Wave 2", "Sine", "PWM Wave 1", "PWM Wave 2", "PWM Wave 3", "PWM Wave 4",
            "Hollo Wave1", "Hollo Wave2", "Hollo Wave2+", "SynStrings 1", "SynStrings 2",
            "SynStrings 3", "SynStrings 4", "SynStrings 5", "SynStrings5+", "SynStrings 6",
            "SynStrings 7", "SynStrings 8", "SynStrings 9", "FM Brass", "Lead Wave 1",
            "Lead Wave 2", "Lead Wave 3", "Lead Wave 4", "Lead Wave 5", "SqrLeadWave",
            "SqrLeadWave+", "SBF Lead 1", "SBF Lead 2", "Sync Sweep", "Saw Sync",
            "Unison Sync", "Unison Sync+", "Sync Wave", "X-Mod Wave 1", "X-Mod Wave 2",
            "X-Mod Wave 3", "X-Mod Wave 4", "X-Mod Wave 5", "X-Mod Wave 6", "X-Mod Wave 7",
            "FeedbackWave", "SubOSC Wave1", "SubOSC Wave2", "SubOSC Wave3", "Saw+Sub Wave",
            "DipthongWave", "DipthongWv +", "Heaven Wave", "Fanta Synth", "Syn Vox 1",
            "Syn Vox 2", "Org Vox", "ZZZ Vox", "Male Ooh", "Doo", "MMM Vox", "Digital Vox",
            "Spark Vox 1", "Spark Vox 2", "Aah Formant", "Eeh Formant", "Iih Formant",
            "Ooh Formant", "UUh Formant", "SBF Vox", "SBF Digi Vox", "VP-330 Choir",
            "FM Syn Vox", "Fine Wine", "Digi Loop", "Vib Wave", "Bell Wave 1", "Bell Wave 1+",
            "Bell Wave 2", "Bell Wave 3", "Bell Wave 4", "Digi Wave 1", "Digi Wave 2",
            "Digi Wave 3", "DIGI Bell", "DIGI Bell +", "Digi Chime", "Org Bell", "FM Bell",
            "Hooky", "Klack Wave", "Syn Sax", "Can Wave 1", "Can Wave 2", "MIDI Clav",
            "Huge MIDI", "Huge MIDI +", "Pulse Clav", "Pulse Clav+", "Cello Wave", "Cutters",
            "5th Wave", "Nasty", "Wave Table", "Bagpipe Wave", "Wally Wave", "Brusky Wave",
            "Wave Scan", "Wire String", "Synth Piano", "EP Hard", "Vint. EP mp", "Vint. EP f",
            "Vint. EP ff", "Stage EP p", "Stage EP f", "SA EP 1", "SA EP 2", "Wurly mp",
            "Wurly mf", "FM EP 1", "FM EP 2", "FM EP 3", "FM EP 4", "FM EP 5", "EP Distone",
            "OrganWave 1", "OrganWave 2", "OrganWave 3", "OrganWave 4", "OrganWave 5",
            "OrganWave 5+", "OrganWave 6", "PercOrgan 1", "PercOrgan 1+", "PercOrgan 2",
            "PercOrgan 2+", "OrganWave 7", "OrganWave 8", "Org Basic 1", "Org Basic 2",
            "Perc Org", "Vint.Organ", "Chorus Organ", "Org Perc", "Org Perc 2nd",
            "JLOrg1 Slw L", "JLOrg1 Slw R", "JLOrg1 Fst L", "JLOrg1 Fst R", "JLOrg2 Slw L",
            "JLOrg2 Slw R", "JLOrg2 Fst L", "JLOrg2 Fst R", "TheaterOrg1L", "TheaterOrg1R",
            "TheaterOrg2L", "TheaterOrg2R", "TheaterOrg3L", "TheaterOrg3R", "Positive \'8",
            "Pipe Organ", "CathedralOrg", "Clav Wave 1", "Clav Wave 2", "Clav Wave 3",
            "Reg.Clav", "Harpsi Wave1", "Harpsi Wave2", "Harpsi Wave3", "Marimba Wave",
            "Marimba Atk", "Vibe Wave", "Xylo Wave 1", "Xylo Wave 2", "FM Mallet",
            "Tubular Bell", "Celesta", "Music Box 1", "Music Box 2", "Nylon Gtr",
            "Brite Nylon", "Ac Gtr ff", "Strat Sust", "Strat Wave 1", "Jazz Gtr",
            "Strat Wave 2", "FstPick70s", "Funk Gtr", "Muters", "Mute Gtr 1", "Mute Gtr 2",
            "Mute Gtr 3", "Harm Gtr", "Nasty Gr", "E.Gtr Loop", "Overdrive 1", "Overdrive 2",
            "Dist Gtr 1", "Dist Gtr 2", "Mute Dis", "Fretless", "SlapBs Wave1", "SlapBs Wave2",
            "Hollow Bass", "Solid Bass", "FM Super Bs", "SyntBs Wave", "SyntBs Wave +",
            "Banjo Wave", "Pluck Harp", "Harp Harm", "Harp Wave", "E.Sitar", "Sitar Wave",
            "Sitar Drone", "Yangqin", "KalimbaWave1", "KalimbaWave2", "Gamelan 1", "Gamelan 2",
            "Gamelan 3", "Steel Drums", "Log Drum", "Bottle Hit", "Agogo", "Agogo Bell",
            "Crystal", "Finger Bell", "Church Bell", "LargeChrF 1", "LargeChrF 2",
            "Female Aahs1", "Female Oohs", "Female Aahs2", "Male Aahs", "Gospel Hum 1",
            "Gospel Hum 2", "Pop Voice", "Jazz Doo 1", "Jazz Doo 2", "Jazz Doo 1+",
            "Jazz Doo 2+", "Jazz Doot 1", "Jazz Doot 2", "Jazz Dat 1", "Jazz Dat 2",
            "Jazz Bap 1", "Jazz Bap 2", "Dow fall 1", "Dow fall 2", "Bass Thum", "Strings 1",
            "Strings 2", "Strings 3", "Strings 4", "Strings 5 L", "Strings 5 R", "Marcato1 L",
            "Marcato1 R", "Marcato2", "F.StrStac1", "F.StrStac2 L", "F.StrStac2 R", "Pizz 1",
            "Pizz 2", "Pizzagogo", "Flute Wave", "Flute Push", "PanPipe Wave", "Bottle Blow",
            "Rad Hose", "Shaku Atk 1", "Shaku Atk 2", "OrchUnison L", "OrchUnison R",
            "Tp Section", "Flugel Wave", "Fr.Horn Wave", "Harmonica", "Harmonica +",
            "Cowbell", "Tabla", "O\'Skool Hit", "Orch Hit", "Punch Hit", "Philly Hit",
            "ClassicHseHt", "Tao Hit", "Anklungs", "Rattles", "Xylo Seq. 1", "Wind Chimes",
            "Bubble", "Xylo Seq. 2", "Siren Wave", "Schratch 1", "Schratch 2", "Schratch 3",
            "Schratch 4", "Schratch 5", "Schratch 6", "Schratch Push", "Schratch Pull",
            "Metal Vox 1", "Metal Vox 1+", "Metal Vox 2", "Metal Vox 2+", "Metal Vox 3",
            "Metal Vox 3+", "Scrape Gut", "Strat Atk", "EP Atk", "Org Atk 1", "Org Atk 2",
            "Org Click", "Harpsi Thmp1", "Harpsi Thmp2", "Shaku Noise", "Klmba Atk",
            "Shami Attack", "Block", "Wood Crak", "AnalogAttack", "Metal Attack", "Pole Loop",
            "Strike Pole", "Switch", "Tuba Slap", "Plink", "Plunk", "Tin Wave", "Vinyl Noise",
            "Pitch Wind", "Vox Noice 1", "Vox Noice 2", "SynVox Noise", "Digi Breath",
            "Agogo Noice", "Wind Agogo", "Polishing Nz", "Dentist Nz", "CrunchWind",
            "ThroatWind", "MetalWind", "Atmosphere", "DigiSpectrum", "SBF Cym", "SBF Bell",
            "SBF Nz", "White Noise", "Pink Noise", "Thickness Bs", "Plastic Bass",
            "Breakdown Bs", "Dist TB", "Pulse Bass", "Hip Lead", "VintageStack", "Tekno Ld 1",
            "Icy Keys", "JP-8StringsL", "JP-8StringsR", "Revalation", "Boreal Pad L",
            "Boreal Pad R", "Sea Waves L", "Sea Waves R", "Sweep Pad 1", "Sweep Pad 2",
            "Sweep Pad 3", "Particles L", "Particles R", "3Delay Poly", "Poly Fat 1",
            "Poly Fat 2", "Poly Fat 3", "Alan\'s Pad L", "Alan\'s Pad R", "DlyReso Saw1",
            "DlyReso Saw2", "DlyReso Saw3", "TranceSaws 1", "TranceSaws 2", "TranceSaws 3",
            "Tekno Ld 2", "NuWave", "EQ Lead 1", "EQ Lead 2", "EQ Lead 3", "80sBrsSect L",
            "80sBrsSect R", "LoveBrsLiveL", "LoveBrsLiveR", "ScoopSynBrsL", "ScoopSynBrsR",
            "Power JP L", "Power JP R", "ChasingBells", "Bad Axe L", "Bad Axe R",
            "Cutting Lead", "Poly Key", "Buzz Cut", "DsturbedSync", "LFO Poly", "HPF Pad L",
            "HPF Pad R", "Chubby Ld", "FantaClaus", "FantasyPad 1", "FantasyPad 2",
            "FantasyPad 3", "Legend Pad", "D-50 Stack", "Digi Crystal", "PipeChatter1",
            "PipeChatter2", "PipeChatter3", "JP Hollow L", "JP Hollow R", "VoiceHeavenL",
            "VoiceHeavenR", "Atmospheric", "Air Pad 1", "Air Pad 2", "Air Pad 3",
            "ChrdOfCnadaL", "ChrdOfCnadaR", "Fireflies", "NewJupiter 1", "NewJupiter 2",
            "NewJupiter 3", "NewJupiter 4", "NewJupiter 5", "Pulsatron", "JazzBubbles",
            "SynthFx 1", "SynthFx 2" };

            for (UInt16 i = 0; i < Names.Length; i++)
            {
                Names[i] = i.ToString() + ": " + Names[i];
            }
        }
    }

    /// <summary>
    /// Different tone types has different set of parameters. The parameter
    /// page selector must be changed to reflect correct pages. The texts
    /// for the selector is listed here.
    /// </summary>
    class EditToneParameterPageItems
    {
        HBTrace t = new HBTrace("class EditToneParameterPageItems");
        public String[][] Items { get; set; }

        public EditToneParameterPageItems()
        {
            Items = new String[5][];
            for (byte i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        Items[i] = new String[] { "Common", "Wave", "PMT (Partial Mapping Table)", "Pitch", "Pitch envelope", "TVF (Time Variant Filter)", "TVF Envelope",
                                            "TVA (Time Variant Amplitude)", "TVA Envelope", "Output", "LFO1", "LFO2", "Step LFO", "Control", "Matrix Control", "MFX (Multi effects)", "MFX Control" };
                        break;
                    case 1:
                        Items[i] = new String[] { "Common", "Wave", "WMT (Wave Mix Table)", "Pitch", "Pitch env", "TVF (Time Variant Filter)", "TVF env", "TVA (Time Variant Amplitude)", "TVA env", "Output", "Compressor", "Equalizer", "MFX (Multi effects)", "MFX control" };
                        break;
                    case 2:
                        Items[i] = new String[] { "Common", "Instrument", "MFX (Multi effects)", "MFX control" };
                        break;
                    case 3:
                        Items[i] = new String[] { "Common", "Osc", "Pitch", "Filter", "AMP", "LFO", "Modulate LFO", "Aftertouch", "Misc", "MFX (Multi effects)", "MFX control" };
                        break;
                    case 4:
                        Items[i] = new String[] { "Common", "Drum instrument", "Compressor", "Equalizer", "MFX (Multi effects)", "MFX control" };
                        break;
                }
            }
        }

        public String[] ParameterPageItems(ProgramType ProgramType)
        {
            t.Trace("public ParameterPageItems (" + "ProgramType." + ProgramType.ToString() + ", " + ")");
            switch (ProgramType)
            {
                case ProgramType.PCM_SYNTH_TONE:
                    return Items[0];
                case ProgramType.PCM_DRUM_KIT:
                    return Items[1];
                case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                    return Items[2];
                case ProgramType.SUPERNATURAL_SYNTH_TONE:
                    return Items[3];
                case ProgramType.SUPERNATURAL_DRUM_KIT:
                    return Items[4];
            }
            return null;
        }
    }

    /// <summary>
    /// Representation of the numbered parameters that can have totally different usage.
    /// E.g. PCM Synth Tone MFX, type 1, Equalizer:
    /// NumberedParameters.Name = Equalizer
    /// NumberedParameters.Parameters:
    ///     NumberedParameters.Parameters[0].Name = Low freq
    ///     NumberedParameters.Parameters[0].Type = SET_OF_NAMES
    ///     NumberedParameters.Parameters[0].Value[0].Name = 200
    ///     NumberedParameters.Parameters[0].Value[0].Value = 0 (Actually, we use index in this case.)
    ///     NumberedParameters.Parameters[0].Value[1].Name = 400
    ///     NumberedParameters.Parameters[0].Value[1].Value = 1 (Actually, we use index in this case.)
    /// </summary>

    enum PARAMETER_TYPE
    {
        CHECKBOX,                        // Single checkbox on its own line
        CHECKBOX_1,                      // Checkbox to be to the left. Next control on the same line.
        CHECKBOX_2,                      // Checkbox to be second from left. Next control on the same line if it is a CHECKBOX or COMBOBOX.
        CHECKBOX_3,                      // Checkbox to be to the right of previous checkbox. Next control on the same line if it is a CHECKBOX.
        CHECKBOX_4,                      // Checkbox to be to the right of previous checkbox. Last on line.
        COMBOBOX_0_TO_100_STEP_0_1_TO_2,
        COMBOBOX_AMPLIFIER_GAIN,
        COMBOBOX_AMPLIFIER_TYPE_3,
        COMBOBOX_AMPLIFIER_TYPE_4,
        COMBOBOX_AMPLIFIER_TYPE_14,
        COMBOBOX_BEND_AFT_SYS1_TO_SYS4,
        COMBOBOX_DRIVE_TYPE,
        COMBOBOX_FILTER_SLOPE,
        COMBOBOX_FILTER_TYPE_2,
        COMBOBOX_FILTER_TYPE_4,
        COMBOBOX_FILTER_TYPE_OFF_2,
        COMBOBOX_GATE_MODE,
        COMBOBOX_HIGH_FREQ,
        COMBOBOX_HZ_AND_NOTE_LENGTHS,
        COMBOBOX_MS_AND_NOTE_LENGTHS,
        COMBOBOX_LEGATO_SLASH,
        COMBOBOX_LOW_BOOST_FREQUENCY,
        COMBOBOX_LOW_BOOST_WIDTH,
        COMBOBOX_LOW_FREQ,
        COMBOBOX_HF_DAMP,
        COMBOBOX_MICROPHONE_DISTANCE,
        COMBOBOX_MID_FREQ,
        COMBOBOX_NORMAL_CROSS,
        COMBOBOX_NORMAL_INVERSE,
        COMBOBOX_PHASER_COLOR,
        COMBOBOX_PHASER_MODE_3,
        COMBOBOX_PHASER_MODE_4,
        COMBOBOX_PHASER_MODE_6,
        COMBOBOX_PHASER_POLARITY,
        COMBOBOX_POLARITY,
        COMBOBOX_POSTFILTER_TYPE,
        COMBOBOX_PREFILTER_TYPE,
        COMBOBOX_LOFI_TYPE,
        COMBOBOX_Q,
        COMBOBOX_RATIO,
        COMBOBOX_ROTARY_SPEED,
        COMBOBOX_SPEAKER_TYPES,
        COMBOBOX_SPEAKER_TYPES_5,
        COMBOBOX_NOTE_LENGTH,
        COMBOBOX_TONE_NAMES,
        COMBOBOX_VOWELS,
        COMBOBOX_WAVE_SHAPE,
        NONE,
        SLIDER_0_05_TO_10_00_STEP_0_05,
        SLIDER_0_10_TO_20_00_STEP_0_10,
        SLIDER_0_TO_10,
        SLIDER_0_TO_100,
        SLIDER_0_TO_12,
        SLIDER_0_TO_18_DB,
        SLIDER_0_TO_127,
        SLIDER_0_TO_127_R,                 // Use this one when putting to the right of a CheckBox!
        SLIDER_0_TO_15,
        SLIDER_0_TO_180_STEP_2,
        SLIDER_0_TO_1300_MS,
        SLIDER_0_TO_100_HZ,
        SLIDER_0_TO_100_MS,
        SLIDER_0_TO_20,
        SLIDER_0_TO_2600_MS,
        SLIDER_MINUS_100_TO_100_STEP_2,
        SLIDER_MINUS_10_TO_10,
        SLIDER_MINUS_15_TO_15,
        SLIDER_MINUS_20_TO_20,
        SLIDER_MINUS_24_TO_24,
        SLIDER_MINUS_50_TO_50,
        SLIDER_MINUS_63_TO_63,
        SLIDER_MINUS_64_TO_64,
        SLIDER_MINUS_98_TO_98_STEP_2,
        SLIDER_MINUS_L64_TO_R63,
        SLIDER_MINUS_W100_TO_D100_STEP_2,
    }

    class ParameterSets
    {
        HBTrace t = new HBTrace("class ParameterSets");
        /// <summary>
        /// These are the specifics for SET_OF_NAMES type numbered parameters for PCM Synth Tone MFX
        /// </summary>
        /// <param name="MFXType"></param>
        /// <param name="i"></param>
        public String[] GetNumberedParameter(PARAMETER_TYPE ParameterType)
        {
            t.Trace("public String[] GetNumberedParameter (" + "PARAMETER_TYPE." + ParameterType + ", " + ")");
            String[] result = new String[] { };
            switch (ParameterType)
            {
                case PARAMETER_TYPE.COMBOBOX_0_TO_100_STEP_0_1_TO_2:
                    result = new String[] { "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9",
                        "1.0", "1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9", "2.0", "2.1",
                        "2.2", "2.3", "2.4", "2.5", "2.6", "2.7", "2.8", "2.9", "3.0", "3.1", "3.2", "3.3",
                        "3.4", "3.5", "3.6", "3.7", "3.8", "3.9", "4.0", "4.1", "4.2", "4.3", "4.4", "4.5",
                        "4.6", "4.7", "4.8", "4.9", "5.0", "5.5", "6.0", "6.5", "7.0", "7.5", "8.0", "8.5",
                        "9.0", "9.5", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21",
                        "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35",
                        "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
                        "50", "52", "54", "56", "58", "60", "62", "64", "66", "68", "70", "72", "74", "76",
                        "78", "80", "82", "84", "86", "88", "90", "92", "94", "96", "98", "100" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_AMPLIFIER_GAIN:
                    result = new String[] { "Low", "Middle", "High" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_3:
                    result = new String[] { "Oldcase", "Newcase", "Wurly" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_4:
                    result = new String[] { "Small", "Built-in", "2-Stack", "3-Stack" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_14:
                    result = new String[] { "JC-120", "Clean Twin", "Match Drive", "Bg Lead", "Ms1959I",
                        "Ms1959Ii", "Ms1959I+Ii", "Sldn Lead", "Metal5150", "Metal Lead", "OD-1", "OD-2 Turbo",
                        "Distortion", "Fuzz" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_DRIVE_TYPE:
                    result = new String[] { "Overdrive", "Distortion" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_GATE_MODE:
                    result = new String[] { "Gate", "Duck" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_HF_DAMP:
                    result = new String[] { "200", "250", "315", "400", "500", "630", "800", "1000", "1250",
                        "1600", "2000", "2500", "3150", "4000", "5000", "6300", "8000", "ByPass" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS:
                    result = new String[] { "Hz", "Note" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_LOW_FREQ:
                    result = new String[] { "200", "400" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_MID_FREQ:
                    result = new String[] { "200", "250", "315", "400", "500", "630", "800", "1000", "1250",
                        "1600", "2000", "2500", "3150", "4000", "5000", "6300", "8000" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS:
                    result = new String[] { "Ms", "Note" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_NORMAL_CROSS:
                    result = new String[] { "Normal", "Cross" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_NORMAL_INVERSE:
                    result = new String[] { "Normal", "Inverse" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_HIGH_FREQ:
                    result = new String[] { "2000", "4000", "8000" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_POSTFILTER_TYPE:
                    result = new String[] { "Post-filter Off", "Post-filter LPF", "Post-filter HPF" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_PREFILTER_TYPE:
                    result = new String[] { "Pre-filter type 1", "Pre-filter type 2", "Pre-filter type 3",
                        "Pre-filter type 4", "Pre-filter type 5", "Pre-filter type 6" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_LOFI_TYPE:
                    result = new String[] { "Lo-Fi type 1", "Lo-Fi type 2", "Lo-Fi type 3",
                        "Lo-Fi type 4", "Lo-Fi type 5", "Lo-Fi type 6", "Lo-Fi type 7", "Lo-Fi type 8", "Lo-Fi type 9" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_Q:
                    result = new String[] { "0.5", "1.0", "2.0", "4.0", "8.0" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_RATIO:
                    result = new String[] { "1.5:1", "2:1", "4:1", "100:1" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH:
                    result = new String[] { "1/64T", "1/64", "1/32T", "1/32", "1/16T", "1/32.",
                                            "1/16", "1/8T", "1/16.", "1/8", "1/4T", "1/8.",
                                            "1/4", "1/2T", "1/4.", "1/2", "1/1T", "1/2.",
                                            "1/1", "2/1T", "1/1.", "2/1" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_TONE_NAMES: // Translates 0 - 127 into tone names
                    result = new String[] { "C-1", "C#-1", "D-1", "D#-1", "E-1", "F-1", "F#-1", "G-1", "G#-1", "A-1", "A#-1", "B-1",
                                            "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B",
                                            "C1", "C#1", "D1", "D#1", "E1", "F1", "F#1", "G1", "G#1", "A1", "A#1", "B1",
                                            "C2", "C#2", "D2", "D#2", "E2", "F2", "F#2", "G2", "G#2", "A2", "A#2", "B2",
                                            "C3", "C#3", "D3", "D#3", "E3", "F3", "F#3", "G3", "G#3", "A3", "A#3", "B3",
                                            "C4", "C#4", "D4", "D#4", "E4", "F4", "F#4", "G4", "G#4", "A4", "A#4", "B4",
                                            "C5", "C#5", "D5", "D#5", "E5", "F5", "F#5", "G5", "G#5", "A5", "A#5", "B5",
                                            "C6", "C#6", "D6", "D#6", "E6", "F6", "F#6", "G6", "G#6", "A6", "A#6", "B6",
                                            "C7", "C#7", "D7", "D#7", "E7", "F7", "F#7", "G7", "G#7", "A7", "A#7", "B7",
                                            "C8", "C#8", "D8", "D#8", "E8", "F8", "F#8", "G8", "G#8", "A8", "A#8", "B8",
                                            "C9", "C#9", "D9", "D#9", "E9", "F9", "F#9", "G9" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_LOW_BOOST_FREQUENCY:
                    result = new String[] { "Boost frequency 50 Hz", "Boost frequency 56 Hz", "Boost frequency 63 Hz", "Boost frequency 71 Hz",
                        "Boost frequency 80 Hz", "Boost frequency 90 Hz", "Boost frequency 100 Hz", "Boost frequency 112 Hz", "Boost frequency 125 Hz" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_LOW_BOOST_WIDTH:
                    result = new String[] { "Wide", "Mid", "Narrow" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_2:
                    result = new String[] { "LPF (Low Pass Filter)", "BPF (Band Pass Filter)" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_4:
                    result = new String[] { "LPF (Low Pass Filter)", "BPF (Band Pass Filter)", "HPF (High Pass Filter)", "Notch filter" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_OFF_2:
                    result = new String[] { "Off", "LPF (Low Pass Filter)", "HPF (High Pass Filter)" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_FILTER_SLOPE:
                    result = new String[] { "-12 Db", "-24 Db", "-36 Db" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_POLARITY:
                    result = new String[] { "Up", "Down" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_VOWELS:
                    result = new String[] { "a", "e", "i", "o", "u" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES:
                    result = new String[]
                    {
                        "SMALL 1 open back 10 dynamic",
                        "SMALL 2 open back 10 dynamic",
                        "MIDDLE open back 12 x 1 dynamic",
                        "JC-120 open back 12 x 2 dynamic",
                        "BUILT-IN 1 open back 12 x 2 dynamic",
                        "BUILT-IN 2 open back 12 x 2 condenser",
                        "BUILT-IN 3 open back 12 x 2 condenser",
                        "BUILT-IN 4 open back 12 x 2 condenser",
                        "BUILT-IN 5 open back 12 x 2 condenser",
                        "BG STACK 1 sealed 12 x 2 condenser",
                        "BG STACK 2 large sealed 12 x 2 condenser",
                        "MS STACK 1 large sealed 12 x 4 condenser",
                        "MS STACK 2 large sealed 12 x 4 condenser",
                        "METAL STACK large double stack 12 x 4 condenser",
                        "2-STACK large double stack 12 x 4 condenser",
                        "3-STACK large triple stack 12 x 4 condenser",
                    };
                    break;
                case PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES_5:
                    result = new String[] { "Line", "Old", "New", "Wurly", "Twin" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_MICROPHONE_DISTANCE:
                    result = new String[] { "Near speaker", "Medium", "Far from speaker" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_PHASER_MODE_3:
                    result = new String[] { "4-Stage", "8-Stage", "12-Stage" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_PHASER_MODE_4:
                    result = new String[] { "Effect depth 1", "Effect depth 2", "Effect depth 3", "Effect depth 4", };
                    break;
                case PARAMETER_TYPE.COMBOBOX_PHASER_MODE_6:
                    result = new String[] { "4-Stage", "8.Stage", "12-Stage", "16-Stage", "20-Stage", "24-Stage" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_PHASER_POLARITY:
                    result = new String[] { "Inverse", "Synchro" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_PHASER_COLOR:
                    result = new String[] { "Modulation character type 1", "Modulation character type 2" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_WAVE_SHAPE:
                    result = new String[] { "Triangle", "Square", "Sinus", "Saw up", "Saw down" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_LEGATO_SLASH:
                    result = new String[] { "Legato", "Slash" };
                    break;
                case PARAMETER_TYPE.COMBOBOX_ROTARY_SPEED:
                    result = new String[] { "Slow", "Fast" };
                    break;
            }
            return result;
        }
    }

    class NumberedParameters
    {
        HBTrace t = new HBTrace("class NumberedParameters");
        public String Name { get; set; }
        public byte Offset { get; set; }
        public NumberedParameter[] Parameters { get; set; }

        public NumberedParameters(byte Offset = 0)
        {
            t.Trace("public NumberedParameters (" + "byte " + Offset + ", " + ")");
            Name = "";
            this.Offset = Offset;
            Parameters = new NumberedParameter[0];
        }
    }

    /// <summary>
    /// Representation of one numbered parameter that can have some specific usage
    /// </summary>
    class NumberedParameter
    {
        HBTrace t = new HBTrace("class NumberedParameter");
        public String Name { get; set; }
        public PARAMETER_TYPE Type { get; set; }
        public String ControlName { get; set; }
        public NumberedParameterValue Value { get; set; }

        public NumberedParameter()
        {
            t.Trace("public NumberedParameter()");
            Name = "";
            Type = PARAMETER_TYPE.SLIDER_0_TO_127;
            ControlName = "";
            Value = new NumberedParameterValue();
        }
    }

    /// <summary>
    /// Representation of a numbered parameter's value.
    /// Text is only used for texts in combobaxes.
    /// Value is only needed when index of the parameter value differs from actual numeric value.
    /// On is only needed for checkboxes (On-Off parameters).
    /// </summary>
    class NumberedParameterValue
    {
        HBTrace t = new HBTrace("class NumberedParameterValue");
        public String[] Text { get; set; } // These are texts for type SET_OF_NAMES
        public UInt16 Value { get; set; }    // This is for numerical values
        public Boolean On { get; set; }    // This is for boolean values

        public NumberedParameterValue()
        {
            t.Trace("public NumberedParameterValue()");
            Text = null;
            Value = 0xff;
            On = false;
        }
    }

    class NumberedParametersContent
    {
        HBTrace t = new HBTrace("class NumberedParametersContent");
        public String[] TypeNames;
        public String[][] ParameterNames;
        public PARAMETER_TYPE[][] ParameterTypes;
        public byte[] MFXPageCount;                 // An MFX type may have too many parameters for one page, and is then splitted into more than one page. 
        public byte[] MFXTypeOffset;                // When a page is splitted, the MFX type is only valid for the first page.
                                                    // Following pages, also for other MFX types, are offset as indicated here.
        public byte[] MFXPageParameterOffset;       // Parameter offset within a splitted page.
        public byte[] MFXIndexFromType;

        public NumberedParametersContent()
        {
            t.Trace("public NumberedParametersContent()");
            // All type names (same for all 5 tone types [PCM tone, PCM drum kit, SuperNatural tone etc] of MFX)
            TypeNames = new String[] {"00:Thru","01:Equalizer","02:Spectrum","03:Low boost",
                "04:Step filter band 1 - 8","        04:Step filter band 9 - 16","        04:Step filter settings","05:Enhancer",
                "06:Auto wah","07:Humanizer","08:Speaker simulator","09:Phaser 1","10:Phaser 2","11:Phaser 3","12:Step phaser",
                "13:Multi stage phaser","14:Infinite phaser","15:Ring modulator","16:Tremolo","17:Auto pan",
                "18:Slicer band 1 - 8","        18:Slicer band 9 - 16","        18:Slicer settings",
                "19:Rotary 1","20:Rotary 2, Speed to Woofer","        20:Rotary 2, Tweeter to Level","21:Rotary 3, Speed to Woofer",
                "        21:Rotary 3, Tweeter to Level","22:Chorus","23:Flanger","24:Step flanger","25:Hexa-chorus","26:Tremolo chorus",
                "27:Space-D","28:Overdrive","29:Distorsion","30:Guitar amp simulator, Amplifier","        30:Guitar amp simulator, Speaker and Mic",
                "31:Compressor","32:Limiter","33:Gate","34:Delay",
                "35:Modulation delay","36:3Tap pan delay","37:4Tap pan delay, delays","        37:4Tap pan delay, levels","38:Multi tap delay, delays",
                "        38:Multi tap delay, levels","39:Reverse delay, reverse","        39:Reverse delay, delays","        39:Reverse delay, levels","40:Time control delay",
                "41:Lo-Fi compress","42:Bit crasher","43:Pitch shifter","44:2Voice shift pitcher","        44:2Voice shift pitcher, output","45:Overdrive->chorus","46:Overdrive->Flanger",
                "47:Overdirve->delay","48:Distorsion->chorus","49:Distorsion->Flanger","50:Distorsion->delay","51:OD/DS->TouchWah, Drive, Amp and TouchWah","        51:OD/DS->TouchWah, TouchWah and Levels",
                "52:DS/OD->AutoWah, amplifier","        52:DS/OD->AutoWah, AutoWah and levels","53:GuitarAmpSim->Chorus, Amplifier","        53:GuitarAmpSim->Chorus, Chorus",
                "54:GuitarAmpSim->Flanger, Amplifier","        54:GuitarAmpSim->Flanger, Flanger, speaker and level","55:GuitarAmpSim->Phaser, Amplifier",
                "        55:GuitarAmpSim->Phaser, Phaser, speaker and level","56:GuitarAmpSim->Delay, Amplifier","        56:GuitarAmpSim->Delay, Delay, speaker and level",
                "57:EP AmpSim->Tremolo","58:EP AmpSim->Chorus","59:EP AmpSim->Flanger","60:EP AmpSim->Phaser","61:EP AmpSim->Delay",
                "62:Enhancer->Chorus","63:Enhancer->Flanger","64:Enhancer->Delay","65:Chorus->Delay","66:Flanger->Delay","67:Chorus->Flanger"};
            ParameterNames = new String[TypeNames.Length][];
            ParameterTypes = new PARAMETER_TYPE[TypeNames.Length][];
            //NonMFXParameters = new byte[TypeNames.Length][];

            byte i = 0;

            // Parameter 00:Thru
            ParameterNames[i] = new String[0];
            ParameterTypes[i++] = new PARAMETER_TYPE[0];
            // Parameter 01:Equalizer
            ParameterNames[i] = new String[] { "Low freq", "Low gain", "Mid1 freq", "Mid1 gain", "Mid1 Q",
                "Mid2 freq", "Mid2 gain", "Mid2 Q", "High freq", "High gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_LOW_FREQ,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.COMBOBOX_MID_FREQ,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.COMBOBOX_Q,
                PARAMETER_TYPE.COMBOBOX_MID_FREQ,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.COMBOBOX_Q,
                PARAMETER_TYPE.COMBOBOX_HIGH_FREQ,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 02:Spectrum
            ParameterNames[i] = new String[] { "Band 1", "Band 2", "Band 3", "Band 4", "Band 5", "Band 6",
                "Band 7", "Band 8", "Q", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.COMBOBOX_Q,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 03:Low boost
            ParameterNames[i] = new String[] { "Freq", "Gain", "Width", "Low gain", "High gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_LOW_BOOST_FREQUENCY,
                PARAMETER_TYPE.SLIDER_0_TO_12,
                PARAMETER_TYPE.COMBOBOX_LOW_BOOST_WIDTH,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 04:Step filter Steps 01 - 08
            ParameterNames[i] = new String[] { "Step 01", "Step 02", "Step 03", "Step 04", "Step 05", "Step 06",
                "Step 07", "Step 08" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 04:Step filter Steps 09 - 16
            ParameterNames[i] = new String[] { "Step 09", "Step 10", "Step 11", "Step 12", "Step 13",
                "Step 14", "Step 15", "Step 16" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 04:Step filter Parameters
            // The selector Hz/Note will cause the next parameter to be double, one for Hz and 
            // one for Note, and they are different.
            // They also occupy 2 memory positions in the Integra-7, so we must make two controls.
            // One slider for Hz and one combobox for Note.
            ParameterNames[i] = new String[] { "Rate (Hz/Note)", "Rate (Hz)", "Note length", "Attack", "Filter type",
                "Filter slope", "Filter resonance", "Filter gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH, 
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_4,
                PARAMETER_TYPE.COMBOBOX_FILTER_SLOPE,
                PARAMETER_TYPE.SLIDER_0_TO_127 ,
                PARAMETER_TYPE.SLIDER_0_TO_12,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 05:Enhancer
            ParameterNames[i] = new String[] { "Sens", "Mix", "Low gain", "High gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 06:Auto wah
            ParameterNames[i] = new String[] { "Filter type", "Manual", "Peak", "Sens", "Polarity",
                "Rate (Hz/Note)", "Rate (Hz)", "Note length", "Depth", "Phase", "Low gain", "High gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_2,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_POLARITY,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH, 
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_180_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 07:Humanizer
            ParameterNames[i] = new String[] { "Overdrive switch", "Overdrive", "Vowel 1", "Vowel 2",
                "Rate (Hz/Note)", "Rate (Hz)", "Note length", "Depth", "Input sync",
                "Input sync threshold: ", "Manual", "Low gain", "High gain",
                "Pan", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_VOWELS,
                PARAMETER_TYPE.COMBOBOX_VOWELS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_100,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 08:Speaker simulator
            ParameterNames[i] = new String[] { "Speaker type", "Mic setting", "Mic level",
                "Direct sound level", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES,
                PARAMETER_TYPE.COMBOBOX_MICROPHONE_DISTANCE,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 09:Phaser 1
            ParameterNames[i] = new String[] { "Mode", "Manual", "Rate (Hz/Note)", "Rate (Hz)",
                "Note length", "Depth", "Polarity", "Resonance", "Cross feedback", "Mix",
                "Low gain", "High gain", "Output level"};
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_PHASER_MODE_3,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_PHASER_POLARITY,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 10Phaser 2
            ParameterNames[i] = new String[] { "Rate", "Color", "Low gain", "High gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_100,
                PARAMETER_TYPE.COMBOBOX_PHASER_COLOR,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 11:Phaser 3
            ParameterNames[i] = new String[] { "Speed", "Low gain", "High gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_100,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 12:Step phaser
            ParameterNames[i] = new String[] { "Mode", "Manual", "Rate(Hz/Note)", "Rate(Hz)", "Note length",
                "Depth", "Polarity", "Resonance", "Cross Feedback", "Step Rate(Hz/Note)", "Rate(Hz)",
                "Note length", "Mix", "Low gain", "High gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_PHASER_MODE_3,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_PHASER_POLARITY,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_10_TO_20_00_STEP_0_10,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 13:Multi stage phaser
            ParameterNames[i] = new String[] { "Mode", "Manual", "Rate(Hz/Note)", "Rate(Hz)",
                "Note length", "Depth", "Resonance", "Mix", "Pan", "Low gain", "High gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_PHASER_MODE_6,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 14:Infinite phaser
            ParameterNames[i] = new String[] { "Mode", "Speed", "Resonance", "Mix", "Pan", "Low gain",
                "High gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_PHASER_MODE_4,
                PARAMETER_TYPE.SLIDER_MINUS_100_TO_100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 15:Ring modulator
            ParameterNames[i] = new String[] { "Frequency", "Sens", "Polarity", "Low gain", "High gain",
                "FX/Direct sound balance", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_POLARITY,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 16:Tremolo
            ParameterNames[i] = new String[] { "Modulation wave", "Rate(Hz/Note)", "Rate(Hz)", "Note length",
                "Depth", "Low gain", "High gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_WAVE_SHAPE,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 17:Auto pan
            ParameterNames[i] = new String[] { "Modulation wave", "Rate(Hz/Note)", "Rate(Hz)", "Note length",
                "Depth", "Low gain", "High gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_WAVE_SHAPE,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 18:Slicer step 1 - 8
            ParameterNames[i] = new String[] { "Band 01", "Band 02", "Band 03", "Band 04", "Band 05",
                "Band 06", "Band 07", "Band 08" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63 };
            // Parameter 18:Slicer step 9 - 16
            ParameterNames[i] = new String[] { "Band 09", "Band 10", "Band 11", "Band 12", "Band 13",
                "Band 14", "Band 15", "Band 16" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63 };
            // Parameter 18:Slicer parameters
            ParameterNames[i] = new String[] { "Rate(Hz/Note)", "Rate(Hz)", "Note length", "Attack",
                "Input sync", "Input sync threshold",
                "Mode", "Shuffle", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.CHECKBOX_1,
                PARAMETER_TYPE.SLIDER_0_TO_127_R,
                PARAMETER_TYPE.COMBOBOX_LEGATO_SLASH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 19:Rotary 1
            ParameterNames[i] = new String[] { "Speed", "Woofer slow speed", "Woofer fast speed",
                "Woofer acceleration", "Woofer level", "Tweeter slow speed", "Tweeter fast speed",
                "Tweeter acceleration", "Tweeter level", "Separation", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_ROTARY_SPEED,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 20:Rotary 2, Speed - Woofer
            ParameterNames[i] = new String[] { "Speed", "Brake", "Woofer slow speed", "Woofer fast speed",
                "Woofer trans up", "Woofer trans down", "Woofer level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_ROTARY_SPEED,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 20:Rotary 2, Tweeter - Level
            ParameterNames[i] = new String[] {  "Tweeter slow speed", "Tweeter fast speed", "Tweeter trans up",
                "Tweeter trans down", "Tweeter level", "Spread", "Low gain", "High gain", "Output level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_10,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 21:Rotary3, Speed - Overdrive
            ParameterNames[i] = new String[] { "Speed", "Brake", "Woofer slow speed", "Woofer fast speed",
                "Woofer trans up", "Woofer trans down", "Woofer level"};
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_ROTARY_SPEED,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 21:Rotary3, Tweeter - Level
            ParameterNames[i] = new String[] { "Tweeter slow speed", "Tweeter fast speed", "Tweeter trans up",
                "Tweeter trans down", "Tweeter level", "Spread", "Low gain", "High gain", "Level", "Overdrive",
                "Overdrive gain", "Overdrive drive", "Overdrive level" }; 
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_10,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127 }; //,
            // Parameter 22:Chorus
            ParameterNames[i] = new String[] { "Filter Type", "Cutoff Freq", "Pre Delay", "Rate(Hz/Note)",
                "Rate", "Note length", "Depth", "Phase", "Low Gain", "High Gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_OFF_2,
                PARAMETER_TYPE.COMBOBOX_MID_FREQ,
                PARAMETER_TYPE.COMBOBOX_0_TO_100_STEP_0_1_TO_2,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_180_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_100_TO_100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 23:Flanger
            ParameterNames[i] = new String[] { "Filter type", "Cutoff frequency", "Pre Delay", "Rate(Hz/Note)",
                "Rate", "Note length", "Depth", "Phase", "Feedback",
                "Low Gain", "High Gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_OFF_2,
                PARAMETER_TYPE.COMBOBOX_MID_FREQ,
                PARAMETER_TYPE.COMBOBOX_0_TO_100_STEP_0_1_TO_2,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_180_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_100_TO_100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 24:Step flanger
            ParameterNames[i] = new String[] { "Filter type", "Cutoff frequency", "Pre Delay", "Rate(Hz/Note)",
                "Rate", "Note length", "Depth", "Phase", "Feedback", "Rate(Hz/Note)", "Step Rate", "Note length",
                "Low Gain", "High Gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_OFF_2,
                PARAMETER_TYPE.COMBOBOX_MID_FREQ,
                PARAMETER_TYPE.COMBOBOX_0_TO_100_STEP_0_1_TO_2,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_180_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_10_TO_20_00_STEP_0_10,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_100_TO_100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 25:Hexa-chorus
            ParameterNames[i] = new String[] { "Pre Delay", "Rate(Hz/Note)", "Rate", "Note length", "Depth",
                "Pre Delay Deviation", "Depth Deviation", "Pan Deviation", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_0_TO_100_STEP_0_1_TO_2,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_20,
                PARAMETER_TYPE.SLIDER_MINUS_20_TO_20,
                PARAMETER_TYPE.SLIDER_0_TO_20,
                PARAMETER_TYPE.SLIDER_MINUS_100_TO_100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127 };
            // Parameter 26:Tremolo chorus
            ParameterNames[i] = new String[] { "Pre Delay", "Rate(Hz/Note)", "Chorus Rate", "Note length",
                "Chorus Depth", "Rate(Hz/Note)", "Tremolo Rate", "Note length", "Tremolo separation",
                "Tremolo phase", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_0_TO_100_STEP_0_1_TO_2,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_180_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_100_TO_100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 27:Space-D
            ParameterNames[i] = new String[] { "Pre Delay", "Rate(Hz/Note)", "Rate", "Note length", "Depth",
                "Phase", "Low Gain", "High Gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_0_TO_100_STEP_0_1_TO_2,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_180_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_100_TO_100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 28:Overdrive
            ParameterNames[i] = new String[] { "Drive", "Tone", "Amplifier switch", "Amplifier type",
                "Low gain", "High gain", "Pan", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_4,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 29:Distorsion
            ParameterNames[i] = new String[] { "Drive", "Tone", "Amplifier switch", "Amplifier type",
                "Low gain", "High gain", "Pan", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_4,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 30:Guitar amp simulator Amp
            ParameterNames[i] = new String[] { "Amplifier switch", "Type", "Volume", "Master", "Gain",
                "Bass", "Middle", "Treble", "Presence", "Bright" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_14,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_GAIN,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.CHECKBOX};
            // Parameter 30:Guitar amp simulator Speaker and Mic
            ParameterNames[i] = new String[] { "Speaker switch", "Speaker type", "Mic setting", "Mic level", "Direct level", "Pan", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES,
                PARAMETER_TYPE.COMBOBOX_MICROPHONE_DISTANCE,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 31:Compressor
            ParameterNames[i] = new String[] { "Attack", "Threshold", "Post gain", "Low gain", "High gain", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_18_DB,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 32:Limiter
            ParameterNames[i] = new String[] { "Release", "Threshold", "Ratio", "Post gain", "Low gain", "High gain", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_RATIO,
                PARAMETER_TYPE.SLIDER_0_TO_18_DB,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 33:Gate
            ParameterNames[i] = new String[] { "Threshold", "Mode", "Attack", "Hold", "Release", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_GATE_MODE,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 34:Delay
            ParameterNames[i] = new String[] { "Delay left(Ms/Note)", "Delay left", "Note", "Delay left(Ms/Note)",
                "Delay right", "Note", "Phase left", "Phase right", "Feedback mode", "Feedback", "HF damp", "Low gain",
                "High gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_NORMAL_INVERSE,
                PARAMETER_TYPE.COMBOBOX_NORMAL_INVERSE,
                PARAMETER_TYPE.COMBOBOX_NORMAL_CROSS,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 35:Modulation delay
            ParameterNames[i] = new String[] { "Delay left(Ms/Note)", "Delay left", "Note", "Delay right(Ms/Note)",
                "Delay right", "Note", "Feedback mode", "Feedback", "HF damp", "Rate(Hz/Note)", "Rate", "Note",
                "Depth", "Phase", "Low gain", "High gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_NORMAL_CROSS,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_180_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 36:3Tap pan delay
            ParameterNames[i] = new String[] { "Delay left(Ms/Note)", "Delay left", "Note", "Delay right(Ms/Note)",
                "Delay right", "Note", "Delay center(Ms/Note)", "Delay center", "Note", "Center feedback", "HF damp",
                "Left level", "Right level", "Center level", "Low gain", "High gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 37:4Tap pan delay, delays
            ParameterNames[i] = new String[] { "Delay 1(Ms/Note)", "Delay 1", "Note", "Delay 2(Ms/Note)", "Delay 2",
                "Note", "Delay 3(Ms/Note)", "Delay 3", "Note", "Delay 4(Ms/Note)", "Delay 4", "Note",
                "Delay 1 feedback" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2};
            // Parameter 37:4Tap pan delay, levels
            ParameterNames[i] = new String[] { "HF Damp", "Delay 1 level", "Delay 2 level", "Delay 3 level",
                "Delay 4 level", "Low gain", "High gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 38:Multi tap delay
            ParameterNames[i] = new String[] { "Delay 1(Ms/Note)", "Delay 1", "Note", "Delay 2(Ms/Note)", "Delay 2",
                "Note", "Delay 3(Ms/Note)", "Delay 3", "Note", "Delay 4(Ms/Note)", "Delay 4", "Note",
                "Delay 1 feedback" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2};
            // Parameter 38:Multi tap delay
            ParameterNames[i] = new String[] { "HF Damp", "Delay 1 pan", "Delay 2 pan", "Delay 3 pan", "Delay 4 pan",
                "Delay 1 level", "Delay 2 level", "Delay 3 level", "Delay 4 level", "Low gain", "High gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 39:Reverse delay, reverse
            ParameterNames[i] = new String[] { "Threshold", "Reverse delay time(Ms/Note)", "Reverse delay time",
                "Note", "Reverse delay feedback", "Reverse delay HF damp", "Reverse delay pan", "Reverse delay level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 39:Reverse delay, delays
            ParameterNames[i] = new String[] { "Delay 1(Ms/Note)", "Delay 1", "Note", "Delay 2(Ms/Note)", "Delay 2",
                "Note", "Delay 3(Ms/Note)", "Delay 3", "Note", "Delay 3 feedback"};
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2};
            // Parameter 39:Reverse delay, levels
            ParameterNames[i] = new String[] { "Delay HF damp", "Delay 1 pan", "Delay 2 pan", "Delay 1 level",
                "Delay 2 level", "Low gain", "High gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 40:Time control delay
            ParameterNames[i] = new String[] { "Delay time(Ms/Note)", "Delay time", "Tone", "Acceleration",
                "Feedback", "HF damp", "Low gain", "High gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 41:LOFI compress
            ParameterNames[i] = new String[] { "Pre-filter type", "Lo-Fi type", "Post-filter type",
                "Post-filter Cof", "Low gain", "High gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_PREFILTER_TYPE,
                PARAMETER_TYPE.COMBOBOX_LOFI_TYPE,
                PARAMETER_TYPE.COMBOBOX_POSTFILTER_TYPE,
                PARAMETER_TYPE.COMBOBOX_MID_FREQ,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 42:Bit crasher
            ParameterNames[i] = new String[] { "Sample rate", "Bit down depth", "Filter depth",
                "Low gain", "High gain", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_20,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 43:Pitch shifter
            ParameterNames[i] = new String[] { "Coarse", "Fine", "Delay time(Ms/Tone)", "Delay time",
                "Tone", "Feedback", "Low gain", "High gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_MINUS_24_TO_24,
                PARAMETER_TYPE.SLIDER_MINUS_100_TO_100_STEP_2,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 44:2Voice shift pitcher
            ParameterNames[i] = new String[] { "Coarse 1", "Fine 1", "Delay time 1(Ms/Tone)", "Delay time 1",
                "Tone 1", "Feedback 1", "Pan 1", "Level 1", "Coarse 2", "Fine 2", "Delay time 2(Ms/Tone)",
                "Delay time 2", "Tone 2", "Feedback 2", "Pan 2", "Level 2" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_MINUS_24_TO_24,
                PARAMETER_TYPE.SLIDER_MINUS_100_TO_100_STEP_2,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_24_TO_24,
                PARAMETER_TYPE.SLIDER_MINUS_100_TO_100_STEP_2,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 44:2Voice shift pitcher, output
            ParameterNames[i] = new String[] { "Low gain", "High gain", "Balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 45:Overdrive->chorus
            ParameterNames[i] = new String[] { "Overdrive drive", "Overdrive pan", "Chorus pre-delay",
                "Chorus rate(Hz/Note)", "Chorus rate", "Note", "Chorus depth", "Chorus balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 46:Overdrive->Flanger
            ParameterNames[i] = new String[] { "Overdrive drive", "Overdrive pan", "Flanger pre-delay",
                "Flanger rate(Hz/Note)", "Flanger rate", "Note", "Flanger depth", "Flanger feedback",
                "Flanger balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 47:Overdirve->delay
            ParameterNames[i] = new String[] { "Overdrive drive", "Overdrive pan", "Delay time(Ms/Note)",
                "Delay time", "Note", "Delay feedback", "Delay HF damp", "Delay balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_2600_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 48:Distorsion->chorus
            ParameterNames[i] = new String[] { "Distortion drive", "Distortion pan", "Chorus pre-delay",
                "Chorus rate(Hz/Note)", "Chorus rate", "Note", "Chorus depth", "Chorus balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 49:Distorsion->Flanger
            ParameterNames[i] = new String[] { "Distortion drive", "Distortion pan", "Flanger pre-delay",
                "Flanger rate(Hz/Note)", "Flanger rate", "Note", "Modulation depth", "Flanger feedback",
                "Flanger balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 50:Distorsion->delay
            ParameterNames[i] = new String[] { "Distortion drive", "Distortion pan", "Delay time(Ms/Note)",
                "Delay time rate", "Note", "Delay feedback", "Delay HF damp", "Delay balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 51:OD/DS->TouchWah, Drive, Amp and TouchWah
            ParameterNames[i] = new String[] { "Drive switch", "Drive type", "Drive", "Tone",
                "Amplifier switch", "Ampifier type" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_DRIVE_TYPE,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_4};
            // Parameter 51:OD/DS->TouchWah, TouchWah and Levels
            ParameterNames[i] = new String[] { "Touch wah switch", "Touch wah filter type", "Touch wah polarity",
                "Touch wah Sens", "Touch wah manual", "Touch wah peak", "Touch wah balance", "Low gain",
                "High gain", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_2,
                PARAMETER_TYPE.COMBOBOX_POLARITY,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 52:DS/OD->AutoWah, amplifier
            ParameterNames[i] = new String[] { "Drive switch", "Drive type", "Drive", "Tone",
                "Amplifier switch", "Ampifier type" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_DRIVE_TYPE,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_4};
            // Parameter 52:DS/OD->AutoWah, amplifier, AutoWah and levels
            ParameterNames[i] = new String[] { "Auto wah switch", "Auto wah filter type", "Auto wah manual",
                "Auto wah peak", "Auto wah rate(Hz/Note)", "Auto wah rate", "Tone", "Auto wah depth",
                "Auto wah balance", "Low gain", "High gain", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_2,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_15_TO_15,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 53:GuitarAmpSim->Chorus, Amplifier
            ParameterNames[i] = new String[] { "Amplifier switch", "Amplifier type", "Amplifier volume",
                "Amplifier master", "Amplifier gain", "Amplifier bass", "Amplifier middle",
                "Amplifier treble" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_14,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_GAIN,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 53:GuitarAmpSim->Chorus, Chorus
            ParameterNames[i] = new String[] { "Chorus switch", "Chorus pre-delay", "Chorus rate",
                "Chorus depth", "Chorus balance", "Speaker switch", "Speaker type", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 54:GuitarAmpSim->Flanger, Amplifier
            ParameterNames[i] = new String[] { "Amplifier switch", "Amplifier type", "Amplifier volume",
                "Amplifier master", "Amplifier gain", "Amplifier bass", "Amplifier middle",
                "Amplifier treble" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_14,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_GAIN,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 54:GuitarAmpSim->Flanger, Flanger, speaker and level
            ParameterNames[i] = new String[] { "Flanger switch", "Flanger pre-delay", "Flanger Rate",
                "Flanger Depth", "Flanger feedback", "Flanger balance", "Speaker switch",
                "Speaker type", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 55:GuitarAmpSim->Phaser, Amplifier
            ParameterNames[i] = new String[] { "Amplifier switch", "Amplifier type", "Amplifier volume",
                "Amplifier master", "Amplifier gain", "Amplifier bass", "Amplifier middle",
                "Amplifier treble" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_14,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_GAIN,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 55:GuitarAmpSim->Phaser, Phaser, speaker and level
            ParameterNames[i] = new String[] { "Phaser switch", "Phaser Manual", "Phaser resonance", "Phaser mix", "Phaser rate", "Phaser depth", "Speaker switch", "Speaker type", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 56:GuitarAmpSim->Delay, Amplifier
            ParameterNames[i] = new String[] { "Amplifier switch", "Amplifier type", "Amplifier volume",
                "Amplifier master", "Amplifier gain", "Amplifier bass", "Amplifier middle",
                "Amplifier treble" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_14,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_GAIN,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 56:GuitarAmpSim->Delay, Delay, speaker and level
            ParameterNames[i] = new String[] { "Delay switch", "Delay Manual", "Delay resonance",
                "Delay mix", "Delay rate", "Delay depth", "Speaker switch", "Speaker type", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 57:EP AmpSim->Tremolo
            ParameterNames[i] = new String[] { "Type", "Bass", "Treble", "Tremolo switch", "Tremolo rate(Hz/Note)",
                "Tremolo rate", "Note", "Tremolo depth", "Tremolo duty", "Speaker type", "Overdrive switch",
                "Overdrive gain", "Overdrive drive", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_3,
                PARAMETER_TYPE.SLIDER_MINUS_50_TO_50,
                PARAMETER_TYPE.SLIDER_MINUS_50_TO_50,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_10_TO_10,
                PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES_5,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 58:EP AmpSim->Chorus
            ParameterNames[i] = new String[] { "Type", "Bass", "Treble", "Chorus switch", "Chorus pre-delay",
                "Chorus rate(Hz/Note)", "Chorus rate", "Note", "Chorus depth", "Chorus balance", "Speaker type",
                "Overdrive switch", "Overdrive gain", "Overdrive drive", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_3,
                PARAMETER_TYPE.SLIDER_MINUS_50_TO_50,
                PARAMETER_TYPE.SLIDER_MINUS_50_TO_50,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES_5,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 59:EP AmpSim->Flanger
            ParameterNames[i] = new String[] { "Type", "Bass", "Treble", "Flanger switch", "Flanger pre-delay", "Flanger rate(Hz/Note)",
                "Flanger rate", "Note", "Flanger depth", "Flanger feedback", "Flanger balance", "Speaker type",
                "Overdrive switch", "Overdrive gain", "Overdrive drive", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_3,
                PARAMETER_TYPE.SLIDER_MINUS_50_TO_50,
                PARAMETER_TYPE.SLIDER_MINUS_50_TO_50,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES_5,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 60:EP AmpSim->Phaser
            ParameterNames[i] = new String[] { "Type", "Bass", "Treble", "Phaser switch", "Phaser manual",
                "Phaser resonance", "Phaser mix", "Phaser rate(Hz/Note)", "Phaser rate", "Note", "Phaser depth",
                "Speaker type", "Overdrive switch", "Overdrive gain", "Overdrive drive", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_3,
                PARAMETER_TYPE.SLIDER_MINUS_50_TO_50,
                PARAMETER_TYPE.SLIDER_MINUS_50_TO_50,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES_5,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 61:EP AmpSim->Delay
            ParameterNames[i] = new String[] { "Type", "Bass", "Treble", "Delay switch", "Delay time(Ms/Note)",
                "Delay time", "Note", "Delay acceleration", "Delay feedback", "Delay HF damp", "Delay balance",
                "Speaker type", "Overdrive switch", "Overdrive gain", "Overdrive drive", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_3,
                PARAMETER_TYPE.SLIDER_MINUS_50_TO_50,
                PARAMETER_TYPE.SLIDER_MINUS_50_TO_50,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_1300_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_15,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES_5,
                PARAMETER_TYPE.CHECKBOX,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 62:Enhancer->Chorus
            ParameterNames[i] = new String[] { "Enhancer sens", "Enhancer mix", "Chorus pre-delay",
                "Chorus rate(Hz/Note)", "Chorus rate", "Note", "Chorus depth", "Chorus balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 63:Enhancer->Flanger
            ParameterNames[i] = new String[] { "Enhancer sens", "Enhancer mix", "Flanger pre-delay",
                "Flanger rate(Hz/Note)", "Flanger rate", "Note", "Flanger depth", "Flanger feedback",
                "Flanger balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 64Enhancer->Delay
            ParameterNames[i] = new String[] { "Enhancer sens", "Enhancer mix",
                "Delay time(Ms/Note)", "Delay time", "Note", "Delay feedback",
                "Delay HF damp", "Delay balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_2600_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 65:Chorus->Delay
            ParameterNames[i] = new String[] { "Chorus pre-delay", "Chorus rate(Hz/Note)", "Chorus rate",
                "Note", "Chorus depth", "Chorus balance", "Delay time(Ms/Note)", "Delay time", "Note",
                "Delay feedback", "Delay HF damp", "Delay balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_2600_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 66:Flanger->Delay
            ParameterNames[i] = new String[] { "Flanger pre-delay", "Flanger rate(Hz/Note)", "Flanger rate",
                "Note", "Flanger depth", "Flanger feedback", "Flanger balance", "Delay time(Ms/Note)",
                "Delay time", "Note", "Delay feedback", "Delay HF damp", "Delay balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_TO_2600_MS,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.COMBOBOX_HF_DAMP,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};
            // Parameter 67:Chorus->Flanger
            ParameterNames[i] = new String[] { "Chorus pre-delay", "Chorus rate(Hz/Note)", "Chorus rate",
                "Note", "Chorus depth", "Chorus balance", "Flanger pre-delay", "Flanger rate(Hz/Note)",
                "Flanger rate", "Note", "Flanger depth", "Flanger feedback", "Flanger balance", "Level" };
            ParameterTypes[i++] = new PARAMETER_TYPE[] {
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_100_MS,
                PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS,
                    PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05,
                    PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH,
                PARAMETER_TYPE.SLIDER_0_TO_127,
                PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2,
                PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2,
                PARAMETER_TYPE.SLIDER_0_TO_127};

            // MFX offsets depending on splitted pages:
            byte mfxCount = i;
            MFXPageCount = new byte[mfxCount];            // Number of pages current MFX type occupies (normally 1)
            MFXTypeOffset = new byte[mfxCount];           // Offset in parameter pages that differs from MFXType due to preceeding multiple page types
            MFXPageParameterOffset = new byte[mfxCount];  // Offset to first parameter in a page. Always 0 for first page, number of preceeding controls for following pages
            MFXIndexFromType = new byte[mfxCount];        // Index to a ParameterType entry from a given MFXType. To get correct parameters use e.g. ParameterNames[MFXIndexFromType[MFXType](+ parameter number)]
            byte mfxPages = 1;
            byte mfxOffset = 0;
            byte mfxParameterOffset = 0;
            Int16 indexFromTypeOffset = -1;
            for (i = 0; i < mfxCount; i++)
            {
                switch (i)
                {
                    // 04:Step filter is splitted into 3 pages and occupies indexes 4 through 6:
                    case 4:
                        mfxPages = 3;
                        mfxOffset = 0;
                        mfxParameterOffset = 0;
                        break;
                    case 5:
                        mfxPages = 3;
                        mfxOffset = 1;
                        mfxParameterOffset = 8;
                        break;
                    case 6:
                        mfxPages = 3;
                        mfxOffset = 2;
                        mfxParameterOffset = 16;
                        break;
                    // 18: Slicer is splitted into 3 pages and occupies indexes 20 through 22:
                    case 20:
                        mfxPages = 3;
                        mfxOffset = 2;
                        mfxParameterOffset = 0;
                        break;
                    case 21:
                        mfxPages = 3;
                        mfxOffset = 3;
                        mfxParameterOffset = 8;
                        break;
                    case 22:
                        mfxPages = 3;
                        mfxOffset = 4;
                        mfxParameterOffset = 16;
                        break;
                    // 20: Rotary 2 occupies 2 indexes, 24 and 25:
                    case 24:
                        mfxPages = 2;
                        mfxOffset = 4;
                        mfxParameterOffset = 0;
                        break;
                    case 25:
                        mfxPages = 2;
                        mfxOffset = 5;
                        mfxParameterOffset = 7;
                        break;
                    // 21: Rotary 3 occupies 2 indexes, 26 and 27:
                    case 26:
                        mfxPages = 2;
                        mfxOffset = 5;
                        mfxParameterOffset = 0;
                        break;
                    case 27:
                        mfxPages = 2;
                        mfxOffset = 6;
                        mfxParameterOffset = 7;
                        break;
                    // 30: Guitar Amp Simulator occupies 2 indexes, 36 and 37:
                    case 36:
                        mfxPages = 2;
                        mfxOffset = 6;
                        mfxParameterOffset = 0;
                        break;
                    case 37:
                        mfxPages = 2;
                        mfxOffset = 7;
                        mfxParameterOffset = 10;
                        break;
                    // 37: 4Tap Pan Delay occupies 2 indexes, 44 and 45:
                    case 44:
                        mfxPages = 2;
                        mfxOffset = 7;
                        mfxParameterOffset = 0;
                        break;
                    case 45:
                        mfxPages = 2;
                        mfxOffset = 8;
                        mfxParameterOffset = 13;
                        break;
                    // 38: Multi Tap Delay occupies 2 indexes, 46 and 47:
                    case 46:
                        mfxPages = 2;
                        mfxOffset = 8;
                        mfxParameterOffset = 0;
                        break;
                    case 47:
                        mfxPages = 2;
                        mfxOffset = 9;
                        mfxParameterOffset = 13;
                        break;
                    // 39: Reverse Delay occupies 3 indexes, 48 - 50:
                    case 48:
                        mfxPages = 3;
                        mfxOffset = 9;
                        mfxParameterOffset = 0;
                        break;
                    case 49:
                        mfxPages = 3;
                        mfxOffset = 10;
                        mfxParameterOffset = 8;
                        break;
                    case 50:
                        mfxPages = 3;
                        mfxOffset = 11;
                        mfxParameterOffset = 18;
                        break;
                    // 44:2Voice shift pitcher occupies 2 indexes, 55 - 56:
                    case 55:
                        mfxPages = 2;
                        mfxOffset = 11;
                        mfxParameterOffset = 0;
                        break;
                    case 56:
                        mfxPages = 2;
                        mfxOffset = 12;
                        mfxParameterOffset = 14;
                        break;
                    // 51:OD/DS->TouchWah occupies 2 indexes, 63 - 64:
                    case 63:
                        mfxPages = 2;
                        mfxOffset = 12;
                        mfxParameterOffset = 0;
                        break;
                    case 64:
                        mfxPages = 2;
                        mfxOffset = 13;
                        mfxParameterOffset = 6;
                        break;
                    // 52:DS/OD->AutoWah occupies 2 indexes, 65 - 66:
                    case 65:
                        mfxPages = 2;
                        mfxOffset = 13;
                        mfxParameterOffset = 0;
                        break;
                    case 66:
                        mfxPages = 2;
                        mfxOffset = 14;
                        mfxParameterOffset = 6;
                        break;
                    // 53:GuitarAmpSim->Chorus occupies 2 indexes, 67 and 68:
                    case 67:
                        mfxPages = 2;
                        mfxOffset = 14;
                        mfxParameterOffset = 0;
                        break;
                    case 68:
                        mfxPages = 2;
                        mfxOffset = 15;
                        mfxParameterOffset = 8;
                        break;
                    // 54:GuitarAmpSim->Flanger occupies 2 indexes, 69 and 70:
                    case 69:
                        mfxPages = 2;
                        mfxOffset = 15;
                        mfxParameterOffset = 0;
                        break;
                    case 70:
                        mfxPages = 2;
                        mfxOffset = 16;
                        mfxParameterOffset = 8;
                        break;
                    // 55:GuitarAmpSim->Phaser occupies 2 indexes, 71 and 72:
                    case 71:
                        mfxPages = 2;
                        mfxOffset = 16;
                        mfxParameterOffset = 0;
                        break;
                    case 72:
                        mfxPages = 2;
                        mfxOffset = 17;
                        mfxParameterOffset = 8;
                        break;
                    // 56:GuitarAmpSim->Delay occupies 2 indexes, 73 and 74:
                    case 73:
                        mfxPages = 2;
                        mfxOffset = 17;
                        mfxParameterOffset = 0;
                        break;
                    case 74:
                        mfxPages = 2;
                        mfxOffset = 18;
                        mfxParameterOffset = 8;
                        break;

                    // Add cases above when more pages are splitted!
                    // Case numbers are the actual ComboBox selected indexes (MFX type + MFX type offset)
                    // mfxPages = number of pages.
                    // mfxOffset = first is same as last in previous splitted page, then increment by one at each page.
                    // mfxParameterOffset = number of parameters to skip before fist parameter on current page, 
                    // always 0 on forst page. Hz/Note and similar parameters counts as 3 parameters!
                    default:
                        mfxPages = 1;
                        mfxParameterOffset = 0;
                        break;
                }
                //MFXIndexFromType[i] = (byte)(indexFromTypeOffset);
                MFXPageCount[i] = mfxPages;
                MFXTypeOffset[i] = mfxOffset;
                MFXPageParameterOffset[i] = mfxParameterOffset;
            }
            for (i = 0; i < mfxCount; i++)
            {
                switch (i)
                {
                    // 04:Step filter is splitted into 3 pages and occupies indexes 4 through 6:
                    case 5:
                        indexFromTypeOffset += 3;
                        break;
                    // 18: Slicer is splitted into 3 pages and occupies indexes 20 through 22:
                    case 19:
                        indexFromTypeOffset += 3;
                        break;
                    // 20: Rotary 2 occupies 2 indexes, 24 and 25:
                    case 21:
                        indexFromTypeOffset += 2;
                        break;
                    // 21: Rotary 3 occupies 2 indexes, 26 and 27:
                    case 22:
                        indexFromTypeOffset += 2;
                        break;
                    // 30: Guitar Amp Simulator occupiew 2 indexes, 36 and 37:
                    case 31:
                        indexFromTypeOffset += 2;
                        break;
                    // 37: 4Tap Pan Delay occupies 2 indexes, 44 and 45:
                    case 38:
                        indexFromTypeOffset += 2;
                        break;
                    // 38: Multi Tap Delay occupies 2 indexes, 46 and 47:
                    case 39:
                        indexFromTypeOffset += 2;
                        break;
                    // 39: Reverse Delay occupies 3 indexes, 48 - 50:
                    case 40:
                        indexFromTypeOffset += 3;
                        break;
                    // 44:2Voice shift pitcher occupies 2 indexes, 55 - 56:
                    case 45:
                        indexFromTypeOffset += 2;
                        break;
                    // 51:OD/DS->TouchWah occupies 2 indexes, 63 - 64:
                    case 52:
                        indexFromTypeOffset += 2;
                        break;
                    // 52:DS/OD->AutoWah occupies 2 indexes, 65 - 66:
                    case 53:
                        indexFromTypeOffset += 2;
                        break;
                    // 53:GuitarAmpSim->Chorus occupies 2 indexes, 67 and 68:
                    case 54:
                        indexFromTypeOffset += 2;
                        break;
                    // 54:GuitarAmpSim->Flanger occupies 2 indexes, 69 and 70:
                    case 55:
                        indexFromTypeOffset += 2;
                        break;
                    // 55:GuitarAmpSim->Phaser occupies 2 indexes, 71 and 72:
                    case 56:
                        indexFromTypeOffset += 2;
                        break;
                    // 56:GuitarAmpSim->Delay occupies 2 indexes, 73 and 74:
                    case 57:
                        indexFromTypeOffset += 2;
                        break;

                    // Add cases here when more pages are splitted!
                    // Case number is always the following MFX type (which will be affected by the extra pages)
                    // indexFromTypeOffset is incremented by the number of pages for the current MFX type
                    default:
                        indexFromTypeOffset++;
                        break;
                }
                MFXIndexFromType[i] = (byte)(indexFromTypeOffset);
            }
        }
    }

    class MFXNumberedParameters
    {
        HBTrace t = new HBTrace("class MFXNumberedParameters");
        public UInt16 Offset { get; set; }
        public byte MFXType { get; set; }
        public byte MFXLength { get; set; }
        public NumberedParameters Parameters { get; set; }

        private ParameterSets sets;

        public MFXNumberedParameters(ReceivedData Data, UInt16 Offset)
        {
            t.Trace("public MFXNumberedParameters (" + "ReceivedData" + Data + ", " + "UInt16" + Offset + ", " + ")");
            this.Offset = Offset;
            sets = new ParameterSets();
            Parameters = new NumberedParameters(0x11);
            NumberedParametersContent content = new NumberedParametersContent();

            MFXType = Data.GetByte(0);
            //byte[]offsets = SetMFXTypeAndOffset(MFXType);

            try
            {
                Parameters.Parameters = new NumberedParameter[content.ParameterTypes.Length];
                MFXLength = (byte)content.ParameterNames[content.MFXIndexFromType[MFXType]].Length;
                for (byte i = 0; i < content.ParameterNames[content.MFXIndexFromType[MFXType]].Length; i++)
                {
                    Parameters.Name = content.ParameterNames[content.MFXIndexFromType[MFXType]][i];
                    Parameters.Parameters[i] = new NumberedParameter();
                    Parameters.Parameters[i].Type = content.ParameterTypes[content.MFXIndexFromType[MFXType]][i];
                    Parameters.Parameters[i].Name = content.ParameterNames[content.MFXIndexFromType[MFXType]][i];
                    if (i < content.ParameterTypes[content.MFXIndexFromType[MFXType]].Length)// && content.ParameterTypes[MFXType][i] != SETS.NOT_A_SET)
                    {
                        Parameters.Parameters[i].Value.Text = sets.GetNumberedParameter(content.ParameterTypes[content.MFXIndexFromType[MFXType]][i]);
                    }
                    Parameters.Parameters[i].Value.Value = Data.Get2Of4Byte(Parameters.Offset + 4 * i); // This gets the value to set selected index.
                }

                // Now, handle any pages that belongs to the same MFXType (splitted pages)
                if (content.MFXPageCount[content.MFXIndexFromType[MFXType]] > 1)
                {
                    byte offset = (byte)(content.ParameterNames[content.MFXIndexFromType[MFXType]].Length);
                    for (byte page = 1; page < content.MFXPageCount[content.MFXIndexFromType[MFXType]]; page++)
                    {
                        for (byte i = 0; i < content.ParameterNames[content.MFXIndexFromType[MFXType] + page].Length; i++)
                        {
                            Parameters.Name = content.ParameterNames[content.MFXIndexFromType[MFXType] + page][i];
                            Parameters.Parameters[i + offset] = new NumberedParameter();
                            Parameters.Parameters[i + offset].Type = content.ParameterTypes[content.MFXIndexFromType[MFXType] + page][i];
                            Parameters.Parameters[i + offset].Name = content.ParameterNames[content.MFXIndexFromType[MFXType] + page][i];
                            if (i < content.ParameterTypes[content.MFXIndexFromType[MFXType] + page].Length)// && content.ParameterSets[MFXType + page][i] != SETS.NOT_A_SET)
                            {
                                Parameters.Parameters[i + offset].Value.Text = sets.GetNumberedParameter(content.ParameterTypes[content.MFXIndexFromType[MFXType] + page][i]);
                            }
                            Parameters.Parameters[i + offset].Value.Value = Data.Get2Of4Byte(Parameters.Offset + 4 * (i + offset)); // This gets the value to set selected index.
                        }
                        offset += (byte)(content.ParameterNames[content.MFXIndexFromType[MFXType] + page].Length);
                    }
                }
            }
            catch (Exception e)
            {
                String message = "Error in MFXNumberedParameters(): " + e.Message;
                if (e.InnerException != null && e.InnerException.Message != null)
                {
                    message += " InnerException: " + e.InnerException.Message;
                }
                t.Trace(message);
            }
        }
    }

    class Instrument
    {
        public String InstrumentBank { get; set; }
        public byte InstrumentNumber { get; set; }
        public String InstrumentName { get; set; }
        public String InstrumentGroup { get; set; }
        public byte MaskIndex { get; set; }

        public Instrument(String InstrumentBank, byte InstrumentNumber, String InstrumentName, String InstrumentGroup, byte MaskIndex)
        {
            this.InstrumentBank = InstrumentBank;
            this.InstrumentNumber = InstrumentNumber;
            this.InstrumentName = InstrumentName;
            this.InstrumentGroup = InstrumentGroup;
            this.MaskIndex = MaskIndex;
        }
    }

    class SuperNATURALAcousticToneVariation
    {
        public String Bank { get; set; }
        public byte Number { get; set; }
        public byte ComboBoxOffset { get; set; }
        public String InstrumentName { get; set; }
        public List<String> Variations { get; set; }

        public SuperNATURALAcousticToneVariation(String Bank, byte Number, byte ComboBoxOffset, String InstrumentName, String Variation1, String Variation2, String Variation3, String Variation4)
        {
            this.Bank = Bank;
            this.Number = Number;
            this.ComboBoxOffset = (byte)(ComboBoxOffset + 1); // Because ComBox creation code will add an "Off" entry first.
            this.InstrumentName = InstrumentName;
            Variations = new List<String>();
            if (Variation1 != "-") Variations.Add(Variation1);
            if (Variation2 != "-") Variations.Add(Variation2);
            if (Variation3 != "-") Variations.Add(Variation3);
            if (Variation4 != "-") Variations.Add(Variation4);
        }
    }

    class SuperNATURALAcousticToneVariations
    {
        List<SuperNATURALAcousticToneVariation> SuperNATURALAcousticToneVariation;

        public SuperNATURALAcousticToneVariations()
        {
            SuperNATURALAcousticToneVariation = new List<Integra_7_uwp.SuperNATURALAcousticToneVariation>();

            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 24, 0, "Glockenspiel", "Dead Stroke", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 25, 0, "Vibraphone", "Dead Stroke", "Tremolo Sw", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 25, 0, "Vibes Hard", "Dead Stroke", "Tremolo Sw", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 25, 0, "Vibes Soft", "Dead Stroke", "Tremolo Sw", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 25, 0, "Vibes Trem", "Dead Stroke", "Tremolo Sw", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 26, 0, "Marimba", "Dead Stroke", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 26, 0, "Marimba Hard", "Dead Stroke", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 26, 0, "Marimba Soft", "Dead Stroke", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 27, 0, "Xylophone", "Dead Stroke", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 27, 0, "Hard Xylo", "Dead Stroke", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 28, 0, "Tubular Bells", "Dead Stroke", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 28, 0, "TubulrBells1", "Dead Stroke", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 28, 0, "TubulrBells2", "Dead Stroke", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 34, 0, "Nylon Guitar", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 34, 0, "Classic Gtr", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 34, 0, "Gut Guitar", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 34, 0, "Solid GutGt", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 35, 0, "Flamenco Guitar", "Rasugueado", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 35, 0, "Flamenco Gtr", "Rasugueado", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 35, 0, "Warm Spanish", "Rasugueado", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 35, 0, "Rasugueado", "Rasugueado", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 36, 0, "SteelStr Guitar", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 36, 0, "StrumSteelGt", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 36, 0, "ArpegSteelGt", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 37, 0, "Jazz Guitar", "FingerPicking", "Octave Tone", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 38, 0, "ST Guitar Half", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 39, 0, "ST Guitar Front", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 40, 0, "TC Guitar Rear", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 41, 0, "Acoustic Bass", "Staccato", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 42, 0, "Fingered Bass", "Slap", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 43, 0, "Picked Bass ", "Bridge Mute", "Harmonics", " - ", " - "));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 44, 0, "Fretless Bass", "Staccato", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 45, 0, "Violin", "Staccato", "Pizzicato", "Tremolo", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 46, 0, "Violin 2", "Staccato", "Pizzicato", "Tremolo", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 47, 0, "Viola", "Staccato", "Pizzicato", "Tremolo", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 48, 0, "Cello", "Staccato", "Pizzicato", "Tremolo", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 49, 0, "Cello 2", "Staccato", "Pizzicato", "Tremolo", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 50, 0, "Contrabass", "Staccato", "Pizzicato", "Tremolo", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 51, 0, "Harp", "Nail", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 52, 0, "Timpani", "Flam", "Accent Roll", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 53, 0, "Strings", "Staccato", "Pizzicato", "Tremolo", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 54, 0, "Marcato Strings", "Staccato", "Pizzicato", "Tremolo", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 55, 0, "London Choir", "Voice Woo", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 56, 0, "Boys Choir", "Voice Woo", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 57, 0, "Trumpet", "Staccato", "Fall", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 58, 0, "Trombone", "Staccato", "Fall", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 59, 0, "Tb2 CupMute", "Staccato", "Fall", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 60, 0, "Mute Trumpet", "Staccato", "Fall", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 61, 0, "French Horn", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 62, 0, "Sop Sax 2", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 63, 0, "Alto Sax 2", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 64, 0, "T.Sax 2", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 65, 0, "Bari Sax 2", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 66, 0, "Oboe", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 67, 0, "Bassoon", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 68, 0, "Clarinet", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 69, 0, "Piccolo", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 70, 0, "Flute", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 71, 0, "Pan Flute", "Staccato", "Flutter", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 72, 0, "Shakuhachi", "Staccato", "Ornament", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 74, 1, "Uilleann Pipes", "-", "Ornament", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 75, 1, "Bag Pipes", "-", "Ornament", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 76, 0, "Erhu", "Staccato", "Ornament", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 77, 0, "Steel Drums", "Mute", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 1, 0, "Santoor", "Mute", "Tremolo", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 1, 0, "Santoor 1", "Mute", "Tremolo", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 1, 0, "Santoor 2", "Mute", "Tremolo", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 2, 0, "Yang Chin", "Mute", "Tremolo", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 2, 0, "Yang Chin 1", "Mute", "Tremolo", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 2, 0, "Yang Chin 2", "Mute", "Tremolo", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 2, 0, "Yang Chin 3", "Mute", "Tremolo", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 3, 0, "Tin Whistle", "Cut", "Ornament", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 4, 0, "Ryuteki", "Staccato", "Ornament", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 5, 0, "Tsugaru", "Strum", "Up Picking", "Auto Bend", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 6, 0, "Sansin", "Strum", "Up Picking", "Auto Bend", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 7, 0, "Koto", "Tremolo", "Ornament", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 9, 0, "Kalimba", "Buzz", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 1, 0, "Sop Sax 1", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 1, 0, "SopSax1 Soft", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 2, 0, "A.Sax 1 Soft", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 2, 0, "Alto Sax 1", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 3, 0, "T.Sax 1 Soft", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 3, 0, "T.Sax Growl", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 3, 0, "TenorSax 1", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 4, 0, "B.Sax 1 Soft", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 4, 0, "Bari Sax 1", "Staccato", "Fall", "SubTone", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 5, 0, "English Horn", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 6, 0, "Bass Clarinet", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 7, 0, "Flute2", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 8, 0, "Soprano Recorder", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 9, 0, "Alto Recorder", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 10, 0, "Tenor Recorder", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 11, 0, "Bass Recorder", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 12, 0, "Ocarina SopC", "Staccato", "Ornament", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 13, 0, "Ocarina SopF", "Staccato", "Ornament", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 14, 0, "Ocarina Alto", "Staccato", "Ornament", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 15, 0, "Ocarina Bass", "Staccato", "Ornament", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 1, 0, "TC Guitar w/Fing", "FingerPicking", "Octave Tone", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 2, 0, "335Guitar w/Fing", "FingerPicking", "Octave Tone", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 3, 0, "LP Guitar Rear", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 4, 0, "LP Guitar Front", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 5, 0, "335 Guitar Half", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 6, 0, "Acoustic Bass 2", "Staccato", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 7, 0, "Fingered Bass 2", "Slap", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 8, 0, "Picked Bass 2 ", "Bridge Mute", "Harmonics", " - ", " - "));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 2, 0, "Nylon Guitar 2", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 3, 0, "12th Steel Gtr", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 4, 0, "Mandolin", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 4, 0, "MandolinGt", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 4, 0, "MandolinStum", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 5, 0, "SteelFing Guitar", "FingerPicking", "Octave Tone", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 6, 0, "SteelStr Guitar2", "Mute", "Harmonics", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 1, 0, "Classical Trumpet", "Staccato", "Fall", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 2, 0, "Frugal Horn", "Staccato", "Fall", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 3, 0, "Trumpet 2", "Staccato", "Fall", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 4, 0, "Mariachi Tp", "Staccato", "Fall", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 5, 0, "Trombone 2", "Staccato", "Fall", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 6, 0, "Bass Trombone", "Staccato", "Fall", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 7, 0, "Tuba", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 8, 0, "Straight Mute Tp", "Staccato", "Fall", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 9, 0, "Cup Mute Trumpet", "Staccato", "Fall", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 10, 0, "French Horn 2", "Staccato", "-", "-", "-"));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 11, 0, "Mute French Horn", "Staccato", "-", "-", "-"));
        }

        public SuperNATURALAcousticToneVariation Get(String Bank, String Instrument)
        {
            UInt16 i = 0;
            while (i < SuperNATURALAcousticToneVariation.Count())
            {
                if (Bank.StartsWith(SuperNATURALAcousticToneVariation[i].Bank) && Instrument.StartsWith(SuperNATURALAcousticToneVariation[i].InstrumentName.Trim()))
                {
                    return SuperNATURALAcousticToneVariation[i];
                }
                i++;
            }
            return null;
        }
    }

    class SuperNaturalAcousticInstrumentList
    {
        public List<Instrument> Tones { get; set; }
        public List<String> ToneGroups { get; set; }
        public List<Instrument> Instruments { get; set; }
        public List<String> Banks { get; set; }
        public List<String> Groups { get; set; }
        public List<List<byte[]>> Parameterlist1 { get; set; }
        public List<List<byte[]>> Parameterlist2 { get; set; }
        public byte[][] ParameterMask { get; set; }

        public SuperNaturalAcousticInstrumentList()
        {
            Tones = new List<Instrument>();
            ToneGroups = new List<String>();
            Instruments = new List<Instrument>();
            Banks = new List<String>();
            Groups = new List<String>();
            Parameterlist1 = new List<List<byte[]>>();
            Parameterlist2 = new List<List<byte[]>>();
            for (byte i = 0; i < 6; i++)
            {
                Parameterlist1.Add(new List<byte[]>());
                Parameterlist2.Add(new List<byte[]>());
            }

            ToneList toneList = new ToneList();
            foreach (List<String> strings in toneList.Tones)
            {
                if (strings[0] == "SuperNATURAL Acoustic Tone" && strings[1] != "Drums")
                {
                    Instruments.Add(new Instrument("INT", (byte)(Int32.Parse(strings[2])), strings[3], strings[1], 0));
                    if (!Groups.Contains(strings[1]))
                    {
                        Groups.Add(strings[1]);
                    }
                }
            }
            foreach (List<String> strings in toneList.Tones)
            {
                if (strings[0].StartsWith("ExSN") && strings[1] != "Drums")
                {
                    String[] parts = strings[0].Split(':');
                    Instruments.Add(new Instrument(parts[0], (byte)(Int32.Parse(strings[2])), strings[3], strings[1], 0));
                    if (!Groups.Contains(strings[1]))
                    {
                        Groups.Add(strings[1]);
                    }
                }
            }

            Banks.Add("INT");
            Banks.Add("ExSN1");
            Banks.Add("ExSN2");
            Banks.Add("ExSN3");
            Banks.Add("ExSN4");
            Banks.Add("ExSN5");

            ToneGroups.Add("Ac.Piano");
            ToneGroups.Add("Ac.Guitar");
            ToneGroups.Add("Ac.Bass");
            ToneGroups.Add("Accordion/Harmonica");
            ToneGroups.Add("Bell/Mallet");
            ToneGroups.Add("Brass");
            ToneGroups.Add("E.Bass");
            ToneGroups.Add("E.Piano");
            ToneGroups.Add("E.Guitar");
            ToneGroups.Add("Flute");
            ToneGroups.Add("Other Keyboards");
            ToneGroups.Add("Organ");
            ToneGroups.Add("Plucked/Stroke");
            ToneGroups.Add("Recorder");
            ToneGroups.Add("Sax");
            ToneGroups.Add("Strings");
            ToneGroups.Add("Vox/Choir");
            ToneGroups.Add("Wind");

            Tones.Add(new Instrument("INT", 1, "Concert Grand", "Ac.Piano", 0));
            Tones.Add(new Instrument("INT", 2, "Grand Piano1", "Ac.Piano", 0));
            Tones.Add(new Instrument("INT", 3, "Grand Piano2", "Ac.Piano", 0));
            Tones.Add(new Instrument("INT", 4, "Grand Piano3", "Ac.Piano", 0));
            Tones.Add(new Instrument("INT", 5, "Mellow Piano", "Ac.Piano", 0));
            Tones.Add(new Instrument("INT", 6, "Bright Piano", "Ac.Piano", 0));
            Tones.Add(new Instrument("INT", 7, "Upright Piano", "Ac.Piano", 0));
            Tones.Add(new Instrument("INT", 8, "Concert Mono", "Ac.Piano", 0));
            Tones.Add(new Instrument("INT", 9, "Honky-tonk", "Ac.Piano", 0));
            Tones.Add(new Instrument("INT", 10, "Pure Vintage EP1", "E.Piano", 1));
            Tones.Add(new Instrument("INT", 11, "Pure Vintage EP2", "E.Piano", 1));
            Tones.Add(new Instrument("INT", 12, "Pure Wurly", "E.Piano", 1));
            Tones.Add(new Instrument("INT", 13, "Pure Vintage EP3", "E.Piano", 1));
            Tones.Add(new Instrument("INT", 14, "Old Hammer EP", "E.Piano", 1));
            Tones.Add(new Instrument("INT", 15, "Dyno Piano", "E.Piano", 1));
            Tones.Add(new Instrument("INT", 16, "Clav CB Flat", "Other Keyboards", 1));
            Tones.Add(new Instrument("INT", 17, "Clav CA Flat", "Other Keyboards", 1));
            Tones.Add(new Instrument("INT", 18, "Clav CB Medium", "Other Keyboards", 1));
            Tones.Add(new Instrument("INT", 19, "Clav CA Medium", "Other Keyboards", 1));
            Tones.Add(new Instrument("INT", 20, "Clav CB Brillia", "Other Keyboards", 1));
            Tones.Add(new Instrument("INT", 21, "Clav CA Brillia", "Other Keyboards", 1));
            Tones.Add(new Instrument("INT", 22, "Clav CB Combo", "Other Keyboards", 1));
            Tones.Add(new Instrument("INT", 23, "Clav CA Combo", "Other Keyboards", 1));
            Tones.Add(new Instrument("INT", 24, "Glockenspiel", "Bell/Mallet", 2));
            Tones.Add(new Instrument("INT", 25, "Vibraphone", "Bell/Mallet", 2));
            Tones.Add(new Instrument("INT", 26, "Marimba", "Bell/Mallet", 2));
            Tones.Add(new Instrument("INT", 27, "Xylophone", "Bell/Mallet", 2));
            Tones.Add(new Instrument("INT", 28, "Tubular Bells", "Bell/Mallet", 2));
            Tones.Add(new Instrument("INT", 29, "TW Organ", "Organ", 3));
            Tones.Add(new Instrument("INT", 30, "French Accordion", "Accordion/Harmonica", 1));
            Tones.Add(new Instrument("INT", 31, "Italian Accordion", "Accordion/Harmonica", 1));
            Tones.Add(new Instrument("INT", 32, "Harmonica", "Accordion/Harmonica", 4));
            Tones.Add(new Instrument("INT", 33, "Bandoneon", "Accordion/Harmonica", 1));
            Tones.Add(new Instrument("INT", 34, "Nylon Guitar", "Ac.Guitar", 5));
            Tones.Add(new Instrument("INT", 35, "Flamenco Guitar", "Ac.Guitar", 5));
            Tones.Add(new Instrument("INT", 36, "SteelStr Guitar", "Ac.Guitar", 5));
            Tones.Add(new Instrument("INT", 37, "Jazz Guitar", "E.Guitar", 7));
            Tones.Add(new Instrument("INT", 38, "ST Guitar Half", "E.Guitar", 7));
            Tones.Add(new Instrument("INT", 39, "ST Guitar Front", "E.Guitar", 7));
            Tones.Add(new Instrument("INT", 40, "TC Guitar Rear", "E.Guitar", 7));
            Tones.Add(new Instrument("INT", 41, "Acoustic Bass", "Ac.Bass", 14));
            Tones.Add(new Instrument("INT", 42, "Fingered Bass", "E.Bass", 14));
            Tones.Add(new Instrument("INT", 43, "Picked Bass", "E.Bass", 14));
            Tones.Add(new Instrument("INT", 44, "Fretless Bass", "E.Bass", 14));
            Tones.Add(new Instrument("INT", 45, "Violin", "Strings", 14));
            Tones.Add(new Instrument("INT", 46, "Violin 2", "Strings", 14));
            Tones.Add(new Instrument("INT", 47, "Viola", "Strings", 14));
            Tones.Add(new Instrument("INT", 48, "Cello", "Strings", 14));
            Tones.Add(new Instrument("INT", 49, "Cello 2", "Strings", 14));
            Tones.Add(new Instrument("INT", 50, "Contrabass", "Strings", 14));
            Tones.Add(new Instrument("INT", 51, "Harp", "Plucked/Stroke", 8));
            Tones.Add(new Instrument("INT", 52, "Timpani", "Percussion", 24));
            Tones.Add(new Instrument("INT", 53, "Strings", "Strings", 15));
            Tones.Add(new Instrument("INT", 54, "Marcato Strings", "Strings", 15));
            Tones.Add(new Instrument("INT", 55, "London Choir", "Vox/Choir", 23));
            Tones.Add(new Instrument("INT", 56, "Boys Choir", "Vox/Choir", 23));
            Tones.Add(new Instrument("INT", 57, "Trumpet", "Brass", 17));
            Tones.Add(new Instrument("INT", 58, "Trombone", "Brass", 17));
            Tones.Add(new Instrument("INT", 59, "Tb2 CupMute", "Brass", 17));
            Tones.Add(new Instrument("INT", 60, "Mute Trumpet", "Brass", 17));
            Tones.Add(new Instrument("INT", 61, "French Horn", "Brass", 17));
            Tones.Add(new Instrument("INT", 62, "Soprano Sax 2", "Sax", 22));
            Tones.Add(new Instrument("INT", 63, "Alto Sax 2", "Sax", 22));
            Tones.Add(new Instrument("INT", 64, "Tenor Sax 2", "Sax", 22));
            Tones.Add(new Instrument("INT", 65, "Baritone Sax 2", "Sax", 22));
            Tones.Add(new Instrument("INT", 66, "Oboe", "Wind", 18));
            Tones.Add(new Instrument("INT", 67, "Bassoon", "Wind", 18));
            Tones.Add(new Instrument("INT", 68, "Clarinet", "Wind", 18));
            Tones.Add(new Instrument("INT", 69, "Piccolo", "Flute", 20));
            Tones.Add(new Instrument("INT", 70, "Flute", "Flute", 20));
            Tones.Add(new Instrument("INT", 71, "Pan Flute", "Flute", 20));
            Tones.Add(new Instrument("INT", 72, "Shakuhachi", "Flute", 21));
            Tones.Add(new Instrument("INT", 73, "Sitar", "Plucked/Stroke", 9));
            Tones.Add(new Instrument("INT", 74, "Uilleann Pipes", "Wind", 19));
            Tones.Add(new Instrument("INT", 75, "Bag Pipes", "Wind", 19));
            Tones.Add(new Instrument("INT", 76, "Erhu", "Strings", 14));
            Tones.Add(new Instrument("INT", 77, "Steel Drums", "Percussion", 25));
            Tones.Add(new Instrument("ExSN1", 1, "Santoor", "Bell/Mallet", 2));
            Tones.Add(new Instrument("ExSN1", 2, "Yang Chin", "Bell/Mallet", 2));
            Tones.Add(new Instrument("ExSN1", 3, "Tin Whistle", "Flute", 21));
            Tones.Add(new Instrument("ExSN1", 4, "Ryuteki", "Flute", 21));
            Tones.Add(new Instrument("ExSN1", 5, "Tsugaru", "Plucked/Stroke", 10));
            Tones.Add(new Instrument("ExSN1", 6, "Sansin", "Plucked/Stroke", 10));
            Tones.Add(new Instrument("ExSN1", 7, "Koto", "Plucked/Stroke", 11));
            Tones.Add(new Instrument("ExSN1", 8, "Taishou Koto", "Plucked/Stroke", 12));
            Tones.Add(new Instrument("ExSN1", 9, "Kalimba", "Plucked/Stroke", 13));
            Tones.Add(new Instrument("ExSN1", 10, "Sarangi", "Strings", 16));
            Tones.Add(new Instrument("ExSN2", 1, "Soprano Sax", "Sax", 22));
            Tones.Add(new Instrument("ExSN2", 2, "Alto Sax", "Sax", 22));
            Tones.Add(new Instrument("ExSN2", 3, "Tenor Sax", "Sax", 22));
            Tones.Add(new Instrument("ExSN2", 4, "Baritone Sax", "Sax", 22));
            Tones.Add(new Instrument("ExSN2", 5, "English Horn", "Wind", 18));
            Tones.Add(new Instrument("ExSN2", 6, "Bass Clarinet", "Wind", 18));
            Tones.Add(new Instrument("ExSN2", 7, "Flute2", "Flute", 20));
            Tones.Add(new Instrument("ExSN2", 8, "Soprano Recorder", "Recorder", 21));
            Tones.Add(new Instrument("ExSN2", 9, "Alto Recorder", "Recorder", 21));
            Tones.Add(new Instrument("ExSN2", 10, "Tenor Recorder", "Recorder", 21));
            Tones.Add(new Instrument("ExSN2", 11, "Bass Recorder", "Recorder", 21));
            Tones.Add(new Instrument("ExSN2", 12, "Ocarina SopC", "Recorder", 21));
            Tones.Add(new Instrument("ExSN2", 13, "Ocarina SopF", "Recorder", 21));
            Tones.Add(new Instrument("ExSN2", 14, "Ocarina Alto", "Recorder", 21));
            Tones.Add(new Instrument("ExSN2", 15, "Ocarina Bass", "Recorder", 21));
            Tones.Add(new Instrument("ExSN3", 1, "TC Guitar w/Fing", "Ac.Guitar", 5));
            Tones.Add(new Instrument("ExSN3", 2, "335Guitar w/Fing", "Ac.Guitar", 5));
            Tones.Add(new Instrument("ExSN3", 3, "LP Guitar Rear", "E.Guitar", 7));
            Tones.Add(new Instrument("ExSN3", 4, "LP Guitar Front", "E.Guitar", 7));
            Tones.Add(new Instrument("ExSN3", 5, "335 Guitar Half", "E.Guitar", 7));
            Tones.Add(new Instrument("ExSN3", 6, "Acoustic Bass 2", "Ac.Bass", 14));
            Tones.Add(new Instrument("ExSN3", 7, "Fingered Bass 2", "E.Bass", 14));
            Tones.Add(new Instrument("ExSN3", 8, "Picked Bass 2", "E.Bass", 14));
            Tones.Add(new Instrument("ExSN4", 1, "Ukulele", "Ac.Guitar", 5));
            Tones.Add(new Instrument("ExSN4", 2, "Nylon Guitar 2", "Ac.Guitar", 5));
            Tones.Add(new Instrument("ExSN4", 3, "12th Steel Gtr", "Ac.Guitar", 5));
            Tones.Add(new Instrument("ExSN4", 4, "MandolinGt", "Ac.Guitar", 6));
            Tones.Add(new Instrument("ExSN4", 5, "MandolinStum", "Ac.Guitar", 6));
            Tones.Add(new Instrument("ExSN4", 6, "SteelFing Guitar", "Ac.Guitar", 5));
            Tones.Add(new Instrument("ExSN4", 7, "SteelStr Guitar2", "Ac.Guitar", 5));
            Tones.Add(new Instrument("ExSN5", 1, "Classical Trumpet", "Brass", 17));
            Tones.Add(new Instrument("ExSN5", 2, "Frugal Horn", "Brass", 17));
            Tones.Add(new Instrument("ExSN5", 3, "Trumpet 2", "Brass", 17));
            Tones.Add(new Instrument("ExSN5", 4, "Mariachi Tp", "Brass", 17));
            Tones.Add(new Instrument("ExSN5", 5, "Trombone 2", "Brass", 17));
            Tones.Add(new Instrument("ExSN5", 6, "Bass Trombone", "Brass", 17));
            Tones.Add(new Instrument("ExSN5", 7, "Tuba", "Brass", 17));
            Tones.Add(new Instrument("ExSN5", 8, "Straight Mute Tp", "Brass", 17));
            Tones.Add(new Instrument("ExSN5", 9, "Cup Mute Trumpet", "Brass", 17));
            Tones.Add(new Instrument("ExSN5", 10, "French Horn 2", "Brass", 17));
            Tones.Add(new Instrument("ExSN5", 11, "Mute French Horn", "Brass", 17));

            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });

            Parameterlist2[0].Add(new byte[] { 0x40, 0x00, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 });
            Parameterlist2[0].Add(new byte[] { 0x40, 0x01, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 });
            Parameterlist2[0].Add(new byte[] { 0x40, 0x02, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 });
            Parameterlist2[0].Add(new byte[] { 0x40, 0x03, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 });
            Parameterlist2[0].Add(new byte[] { 0x40, 0x04, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 });
            Parameterlist2[0].Add(new byte[] { 0x40, 0x05, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 });
            Parameterlist2[0].Add(new byte[] { 0x40, 0x06, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 });
            Parameterlist2[0].Add(new byte[] { 0x40, 0x07, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 });
            Parameterlist2[0].Add(new byte[] { 0x40, 0x08, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x04, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x04, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x02, 0x04, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x03, 0x04, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x06, 0x04, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x07, 0x04, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x02, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x03, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x04, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x05, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x06, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x07, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x09, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x0b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x0c, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x0d, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x0e, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x41, 0x00, 0x08, 0x08, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x05, 0x05, 0x03, 0x0a, 0x68, 0x2b, 0x05, 0x40, 0x00, 0x14 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x15, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x15, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x16, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x17, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x18, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x18, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x19, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x1a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x1b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x1b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x02, 0x1b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x20, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x21, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x22, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x23, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x28, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x28, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x29, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x2a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x2a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x2b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x2e, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x2f, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x30, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x30, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x34, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x34, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x38, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x39, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x03, 0x39, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x3b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x3c, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x41, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x02, 0x42, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x43, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x44, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x46, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x47, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x48, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x49, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x4b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x4d, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x68, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x6d, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x6d, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x01, 0x6e, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[0].Add(new byte[] { 0x00, 0x72, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });

            Parameterlist1[1].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[1].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[1].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[1].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[1].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[1].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[1].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[1].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[1].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[1].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });

            Parameterlist2[1].Add(new byte[] { 0x00, 0x0f, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[1].Add(new byte[] { 0x01, 0x2e, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[1].Add(new byte[] { 0x01, 0x4b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[1].Add(new byte[] { 0x01, 0x4d, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[1].Add(new byte[] { 0x00, 0x6a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[1].Add(new byte[] { 0x01, 0x6a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[1].Add(new byte[] { 0x00, 0x6b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[1].Add(new byte[] { 0x01, 0x6b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[1].Add(new byte[] { 0x00, 0x6c, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[1].Add(new byte[] { 0x02, 0x6e, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });

            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });

            Parameterlist2[2].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x00, 0x41, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x00, 0x42, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x00, 0x43, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x00, 0x45, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x01, 0x47, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x01, 0x49, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x00, 0x4a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x01, 0x4a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x02, 0x4a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x03, 0x4a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x00, 0x4f, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x01, 0x4f, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x02, 0x4f, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[2].Add(new byte[] { 0x03, 0x4f, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });

            Parameterlist1[3].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[3].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[3].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[3].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[3].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[3].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[3].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[3].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });

            Parameterlist2[3].Add(new byte[] { 0x01, 0x1a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[3].Add(new byte[] { 0x02, 0x1a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[3].Add(new byte[] { 0x03, 0x1b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[3].Add(new byte[] { 0x04, 0x1b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[3].Add(new byte[] { 0x05, 0x1b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[3].Add(new byte[] { 0x01, 0x20, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[3].Add(new byte[] { 0x01, 0x21, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[3].Add(new byte[] { 0x01, 0x22, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });

            Parameterlist1[4].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[4].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[4].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[4].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[4].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[4].Add(new byte[] { 0x01, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });

            Parameterlist2[4].Add(new byte[] { 0x02, 0x18, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[4].Add(new byte[] { 0x03, 0x18, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[4].Add(new byte[] { 0x01, 0x19, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[4].Add(new byte[] { 0x02, 0x19, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[4].Add(new byte[] { 0x03, 0x19, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[4].Add(new byte[] { 0x04, 0x19, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });

            Parameterlist1[5].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[5].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[5].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[5].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[5].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[5].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[5].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[5].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[5].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[5].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });
            Parameterlist1[5].Add(new byte[] { 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 });

            Parameterlist2[5].Add(new byte[] { 0x01, 0x38, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[5].Add(new byte[] { 0x02, 0x38, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[5].Add(new byte[] { 0x03, 0x38, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[5].Add(new byte[] { 0x04, 0x38, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[5].Add(new byte[] { 0x01, 0x39, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[5].Add(new byte[] { 0x02, 0x39, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[5].Add(new byte[] { 0x00, 0x3a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[5].Add(new byte[] { 0x01, 0x3b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[5].Add(new byte[] { 0x02, 0x3b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[5].Add(new byte[] { 0x01, 0x3c, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });
            Parameterlist2[5].Add(new byte[] { 0x02, 0x3c, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 });


            byte[] CategoryToMaskMap = new byte[30];
            CategoryToMaskMap[0] = 0;

            ParameterMask = new byte[49][];

            for (byte i = 0; i < 49; i++)
            {
                ParameterMask[i] = new byte[51];
            }

            // First index is instrument settings group (see comments above each group).
            // Second index is the parameter:
            // 0 Slider for String Resonance
            // 1 Key Off Resonance 0–127
            // 2 Hammer Noise -2, -1, 0, +1, +2
            // 3 StereoWidth 0–63
            // 4 Nuance Type1, Type2, Type3
            // 5 Tone Character -5, -4, -3, -2, -1, 0, +1, +2, +3, +4, +5
            // 6 Noise Level (CC16) - 64–+63
            // 7 Crescendo Depth(CC17) - 64–+63 (This applies only for ExSN5 004: Mariachi Tp)
            // 8 Tremolo Speed(CC17) - 64–+63
            // 9 Strum Speed(CC17) - 64–+63
            // 10 Strum Mode(CC19) OFF, ON
            // 11 Picking Harmonics OFF, ON
            // 12 Sub String Tune - 64–+63 (This is valid only for ExSN4 003: 12th Steel Gtr.)
            // 13 Growl Sens(CC18) 0–127
            // 14 Harmonic Bar 16' 0–8
            // 15 Harmonic Bar 5 - 1 / 3' 0–8
            // 16 Harmonic Bar 8' 0–8
            // 17 Harmonic Bar 4' 0–8
            // 18 Harmonic Bar 2 - 2 / 3' 0–8
            // 19 Harmonic Bar 2' 0–8
            // 20 Harmonic Bar 1 - 3 / 5' 0–8
            // 21 Harmonic Bar 1 - 1 / 3' 0–8
            // 22 Harmonic Bar 1' 0–8
            // 23 Leakage Level 0–127
            // 24 Percussion Switch OFF, ON
            // 25 Percussion Soft NORM, SOFT
            // 26 Percussion Soft Level 0–15
            // 27 Percussion Normal Level 0–15
            // 28 Percussion Slow FAST, SLOW
            // 29 Percussion Slow Time 0–127
            // 30 Percussion Fast Time 0–127
            // 31 Percussion Harmonic 2ND, 3RD
            // 32 Percussion Recharge Time 0–15
            // 33 Percussion Harmonic Bar Level 0–127
            // 34 Key On Click Level 0–31
            // 35 Key Off Click Level 0–31
            // 36 Mallet Hardness(CC16) - 64–+63
            // 37 Resonance Level(CC16) - 64–+63
            // 38 Roll Speed(CC17) - 64–+63
            // 39 Glissando Mode(CC19) OFF, ON
            // 40 Play Scale
            // 41 Scale Key C, Db, D, Eb, E, F, Gb, G, Ab, A, Bb, B
            // 42 Bend Depth(CC17) - 64–+63
            // 43 Buzz Key Switch OFF, ON
            // 44 Tambura Level - 64–+63
            // 45 Tambura Pitch - 12–+12
            // 46 Hold Legato Mode(CC19) OFF, ON
            // 47 Drone Level - 64–+63
            // 48 Drone Pitch - 12–+12
            // 49 Glide
            // 50 Variation Refer to p. 28.

            // A.Piano
            ParameterMask[1][0] = 1; // Slider for String Resonance:
            ParameterMask[1][1] = 1; // Key Off Resonance 0–127
            ParameterMask[1][2] = 1; // Hammer Noise -2, -1, 0, +1, +2
            ParameterMask[1][3] = 1; // StereoWidth 0–63
            ParameterMask[1][4] = 1; // Nuance Type1, Type2, Type3
            ParameterMask[1][5] = 1; // Tone Character -5, -4, -3, -2, -1, 0, +1, +2, +3, +4, +5

            // E.Piano
            ParameterMask[2][6] = 1; // Noise Level (CC16) - 64–+63

            // Organ
            ParameterMask[3][14] = 1;
            ParameterMask[3][15] = 1;
            ParameterMask[3][16] = 1;
            ParameterMask[3][17] = 1;
            ParameterMask[3][18] = 1;
            ParameterMask[3][19] = 1;
            ParameterMask[3][20] = 1;
            ParameterMask[3][21] = 1;
            ParameterMask[3][22] = 1;
            ParameterMask[3][23] = 1;
            ParameterMask[3][24] = 1;
            ParameterMask[3][25] = 1;
            ParameterMask[3][26] = 1;
            ParameterMask[3][27] = 1;
            ParameterMask[3][28] = 1;
            ParameterMask[3][29] = 1;
            ParameterMask[3][30] = 1;
            ParameterMask[3][31] = 1;
            ParameterMask[3][32] = 1;
            ParameterMask[3][33] = 1;
            ParameterMask[3][34] = 1;
            ParameterMask[3][35] = 1;

            // Other keyboards + Accordion
            ParameterMask[4][6] = 1;

            // Accordion
            ParameterMask[5][6] = 1;

            // Bell/Mallet
            ParameterMask[6][36] = 1;
            ParameterMask[6][38] = 1;
            ParameterMask[6][50] = 1;

            // Ac.Guitar
            ParameterMask[7][6] = 1;
            ParameterMask[7][9] = 1;
            ParameterMask[7][10] = 1;
            ParameterMask[7][12] = 1;
            ParameterMask[7][50] = 1;

            // E.Guitar
            ParameterMask[8][6] = 1;
            ParameterMask[8][9] = 1;
            ParameterMask[8][10] = 1;
            ParameterMask[8][11] = 1;
            ParameterMask[8][50] = 1;

            // Dist.Guitar
            ParameterMask[9][6] = 1;
            ParameterMask[9][9] = 1;
            ParameterMask[9][10] = 1;
            ParameterMask[9][11] = 1;
            ParameterMask[9][50] = 1;

            // Ac.Bass
            ParameterMask[10][6] = 1;
            ParameterMask[10][50] = 1;

            // E.Bass
            ParameterMask[11][6] = 1;
            ParameterMask[11][50] = 1;

            // Synth Bass
            ParameterMask[12][6] = 1;
            ParameterMask[12][50] = 1;

            // Plucked/Stroke
            ParameterMask[13][39] = 1;
            ParameterMask[13][40] = 1;
            ParameterMask[13][41] = 1;
            ParameterMask[13][50] = 1;

            // Strings
            ParameterMask[14][6] = 1;
            ParameterMask[14][50] = 1;

            // Brass
            ParameterMask[15][6] = 1;
            ParameterMask[15][7] = 1;
            ParameterMask[15][13] = 1;
            ParameterMask[15][50] = 1;

            // Wind
            ParameterMask[16][6] = 1;
            ParameterMask[16][13] = 1;
            ParameterMask[16][40] = 1;
            ParameterMask[16][41] = 1;
            ParameterMask[16][50] = 1;

            // Flute
            ParameterMask[17][6] = 1;
            ParameterMask[17][13] = 1;
            ParameterMask[17][40] = 1;
            ParameterMask[17][41] = 1;
            ParameterMask[17][50] = 1;

            // Sax
            ParameterMask[18][6] = 1;  // Noise Level (CC16) - 64–+63
            ParameterMask[18][13] = 1; // 13 Growl Sens(CC18) 0–127
            ParameterMask[18][40] = 1; // 40 Play Scale
            ParameterMask[18][41] = 1; // 41 Scale Key C, Db, D, Eb, E, F, Gb, G, Ab, A, Bb, B
            ParameterMask[18][49] = 1; // 49 Glide
            ParameterMask[18][50] = 1; // 50 Variation Refer to p. 28.

            // Recorder
            ParameterMask[19][6] = 1;
            ParameterMask[19][13] = 1;
            ParameterMask[19][50] = 1;

            // Vox/Choir
            ParameterMask[20][46] = 1;
            ParameterMask[20][50] = 1;

            // Synth Lead [21]

            // Synth Brass [22]

            // Synth Pad/Strings [23]

            // Synth Bellpad [24]
            // Synth PolyKey [25]
            // FX [26]
            // Synth Seq/Pop [27]
            // Phrase [28]
            // Pulsating [29]
            // Beat &Groove [30]
            // Hit [31]
            // Sound FX [32]
            // Drums [33]



            // Percussion
            ParameterMask[34][38] = 1;
            ParameterMask[34][50] = 1;

            // Combination [35]

            // From here on we have exceptions

            // Mandolin
            ParameterMask[36][6] = 1;
            ParameterMask[36][8] = 1;
            ParameterMask[36][10] = 1;
            ParameterMask[36][50] = 1;

            // Sitar
            ParameterMask[37][37] = 1;
            ParameterMask[37][44] = 1;
            ParameterMask[37][45] = 1;

            // Tsugaru/Sansin
            ParameterMask[38][37] = 1;
            ParameterMask[38][42] = 1;
            ParameterMask[38][43] = 1;
            ParameterMask[38][50] = 1;

            // Koto
            ParameterMask[39][8] = 1;
            ParameterMask[39][39] = 1;
            ParameterMask[39][40] = 1;
            ParameterMask[39][41] = 1;
            ParameterMask[39][43] = 1;
            ParameterMask[39][50] = 1;

            // Taishou Koto
            ParameterMask[40][6] = 1;
            ParameterMask[40][8] = 1;

            // Kalimba
            ParameterMask[41][37] = 1;
            ParameterMask[41][50] = 1;

            // Erhu
            ParameterMask[42][6] = 1;
            ParameterMask[42][50] = 1;

            // Marcato Strings
            ParameterMask[43][46] = 1;
            ParameterMask[43][50] = 1;

            // Sarangi
            ParameterMask[44][37] = 1;
            ParameterMask[44][44] = 1;
            ParameterMask[44][45] = 1;

            // Pipes
            ParameterMask[45][47] = 1;
            ParameterMask[45][48] = 1;
            ParameterMask[45][50] = 1;

            // Shakuhachi + Recorder
            ParameterMask[46][6] = 1;
            ParameterMask[46][13] = 1;
            ParameterMask[46][50] = 1;

            // Steel Drums
            ParameterMask[47][37] = 1;
            ParameterMask[47][38] = 1;
            ParameterMask[47][50] = 1;

            // Harmonica
            ParameterMask[48][6] = 1;
            ParameterMask[48][13] = 1;
        }

        public List<Instrument> ListInstruments(String Bank)
        {
            List<Instrument> Result = new List<Instrument>();
            foreach (Instrument instrument in Instruments)
            {
                if (instrument.InstrumentBank == Bank)
                {
                    Result.Add(instrument);
                }
            }
            return Result;
        }

        public Int16 GetInstrument(String InstrumentBank, Int32 InstrumentNumber)
        {
            Int16 result = -1;
            Int16 i = 0;
            while (result == -1 && i < Instruments.Count())
            {
                if (Instruments[i].InstrumentBank == InstrumentBank && Instruments[i].InstrumentNumber == InstrumentNumber)
                {
                    result = i;
                    break;
                }
                i++;
            }
            return result;
        }

        public Instrument GetInstrument(String InstrumentBank, String Instrument)
        {
            Instrument result = null;
            Int16 i = 0;
            while (result == null && i < Instruments.Count())
            {
                if (Instruments[i].InstrumentBank == InstrumentBank && Instrument.EndsWith(Instruments[i].InstrumentName))
                {
                    result = Instruments[i];
                    break;
                }
                i++;
            }
            return result;
        }

        public Instrument GetTone(String InstrumentBank, String Instrument)
        {
            Instrument result = null;
            Int16 i = 0;
            while (result == null && i < Tones.Count())
            {
                if (Tones[i].InstrumentBank == InstrumentBank && Instrument.EndsWith(Tones[i].InstrumentName))
                {
                    result = Tones[i];
                    break;
                }
                i++;
            }
            return result;
        }
    }

    /// <summary>
    /// PCM Synth Tone
    /// Read PCM Synth Tone Common from MIDI and create PCMSynthTone. PCMSynthTone and subclass PCMSynthToneCommon will be created and populated.
    /// Read subclasses one by one and create using read data.
    /// Note that PCMSynthTonePartial are read each from different addresses!
    /// </summary>
    class PCMSynthTone
    {
        HBTrace t = new HBTrace("class PCMSynthTone");
        public PCMSynthToneCommon pCMSynthToneCommon { get; set; }
        //public PCMSynthToneCommonMFX PCMSynthToneCommonMFX { get; set; }
        public PCMSynthTonePMT pCMSynthTonePMT { get; set; }
        public PCMSynthTonePartial pCMSynthTonePartial { get; set; }
        public PCMSynthToneCommon2 pCMSynthToneCommon2 { get; set; }

        public PCMSynthTone(ReceivedData Data)
        {
            t.Trace("public PCMSynthTone (" + "ReceivedData" + Data + ", " + ")");
            pCMSynthTonePartial = new PCMSynthTonePartial(Data);
            pCMSynthToneCommon = new PCMSynthToneCommon(Data);
        }
    }

    /// <summary>
    /// Same as above for all main classes
    /// </summary>
    class PCMDrumKit
    {
        HBTrace t = new HBTrace("class PCMDrumKit");
        public PCMDrumKitCommon pCMDrumKitCommon { get; set; }
        //public PCMDrumKitCommonMFX PCMDrumKitCommonMFX { get; set; }
        public PCMDrumKitCommonCompEQ pCMDrumKitCommonCompEQ { get; set; }
        public PCMDrumKitPartial pCMDrumKitPartial { get; set; } // [88]
        public PCMDrumKitCommon2 pCMDrumKitCommon2 { get; set; }

        public PCMDrumKit(ReceivedData Data)
        {
            t.Trace("public PCMDrumKit (" + "ReceivedData" + Data + ", " + ")");
            pCMDrumKitPartial = new PCMDrumKitPartial(Data);
            pCMDrumKitCommon = new PCMDrumKitCommon(Data);
        }
    }

    class SuperNATURALAcousticTone
    {
        HBTrace t = new HBTrace("class SuperNATURALAcousticTone");
        public SuperNATURALAcousticToneCommon superNATURALAcousticToneCommon { get; set; }
        //public SuperNATURALAcousticToneMFX SuperNATURALAcousticToneMFX { get; set; }

        public SuperNATURALAcousticTone(ReceivedData Data)
        {
            t.Trace("public SuperNATURALAcousticTone (" + "ReceivedData" + Data + ", " + ")");
            superNATURALAcousticToneCommon = new SuperNATURALAcousticToneCommon(Data);
        }
    }

    class SuperNATURALSynthTone
    {
        HBTrace t = new HBTrace("class SuperNATURALSynthTone");
        public SuperNATURALSynthToneCommon superNATURALSynthToneCommon { get; set; }
        //public SuperNATURALSynthToneCommonMFX SuperNATURALSynthToneCommonMFX { get; set; }
        public SuperNATURALSynthTonePartial superNATURALSynthTonePartial { get; set; }
        public SuperNATURALSynthToneMisc superNATURALSynthToneMisc { get; set; }

        public SuperNATURALSynthTone(ReceivedData Data)
        {
            t.Trace("public SuperNATURALSynthTone (" + "ReceivedData" + Data + ", " + ")");
            superNATURALSynthToneCommon = new SuperNATURALSynthToneCommon(Data);
        }
    }

    class DrumInstrument
    {
        public String Bank { get; set; }
        public byte Number { get; set; }
        public String Name { get; set; }
        public String Group { get; set; }
        public Boolean StereoWidth { get; set; }
        public Boolean AmbienceLevel { get; set; }
        public String Variation { get; set; }

        public DrumInstrument(String Bank, byte Number, String Name, String Group, Boolean StereoWidth, Boolean AmbienceLevel, String Variation)
        {
            this.Bank = Bank;
            this.Number = Number;
            this.Name = Name;
            this.Group = Group;
            this.StereoWidth = StereoWidth;
            this.AmbienceLevel = AmbienceLevel;
            this.Variation = Variation;
        }

        public List<String> Variations()
        {
            List<String> variations = new List<String>();

            variations.Add("Off");
            if (!String.IsNullOrEmpty(Variation))
            {
                if (Variation.Contains("Flam"))
                {
                    variations.Add("Flam 1");
                    variations.Add("Flam 2");
                    variations.Add("Flam 3");
                }
                else
                {
                    variations.Add("---");
                    variations.Add("---");
                    variations.Add("---");
                }
                if (Variation.Contains("Buzz"))
                {
                    variations.Add("Buzz 1");
                    variations.Add("Buzz 2");
                    variations.Add("Buzz 3");
                }
                else
                {
                    variations.Add("---");
                    variations.Add("---");
                    variations.Add("---");
                }
                if (Variation.Contains("Roll"))
                {
                    variations.Add("Roll");
                }
                else
                {
                    variations.Add("---");
                }
            }

            return variations;
        }
    }

    class SuperNATURALDrumKitInstrumentList
    {
        public List<DrumInstrument> DrumInstruments = new List<DrumInstrument>();

        public SuperNATURALDrumKitInstrumentList()
        {
            DrumInstruments.Add(new DrumInstrument("INT", 0, "Off", "", false, false, ""));
            DrumInstruments.Add(new DrumInstrument("INT", 1, "Studio Kick", "Kick", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 2, "Pop Kick", "Kick", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 3, "Jazz Kick", "Kick", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 4, "Rock Kick", "Kick", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 5, "Studio Kick 2", "Kick", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 6, "Rock Kick 2", "Kick", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 7, "Orch Bass Drum", "Kick", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 8, "Studio Sn", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 9, "Studio Sn Rim", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 10, "Studio Sn XStk", "Snare", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 11, "Pop Sn", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 12, "Pop Sn Rim", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 13, "Pop Sn XStk", "Snare", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 14, "Jazz Sn", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 15, "Jazz Sn Rim", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 16, "Jazz Sn XStk", "Snare", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 17, "Rock Sn", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 18, "Rock Sn Rim", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 19, "Rock Sn XStk", "Snare", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 20, "Tight Sn", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 21, "Tight Sn Rim", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 22, "Tight Sn XStk", "Snare", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 23, "Studio Sn 2", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 24, "Studio Sn 2 Rim", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 25, "Studio Sn 2 XStk", "Snare", true, true, "FlamBuzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 26, "Rock Sn 2", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 27, "Rock Sn 2 Rim", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 28, "Rock Sn 2 XStk", "Snare", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 29, "Brush Sn Slap", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 30, "Brush Sn Tap", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 31, "Brush Sn Slide", "Snare", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 32, "Brush Sn Swirl 1", "Snare", true, true, ""));
            DrumInstruments.Add(new DrumInstrument("INT", 33, "Brush Sn Swirl 2", "Snare", true, true, ""));
            DrumInstruments.Add(new DrumInstrument("INT", 34, "Snare CrossStk", "Snare", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 35, "Orch Snare", "Snare", true, true, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 36, "Orch Snare XStk", "Snare", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 37, "Pop Tom Hi", "Tom", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 38, "Pop Tom Mid", "Tom", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 39, "Pop Tom Flr", "Tom", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 40, "Rock Tom Hi", "Tom", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 41, "Rock Tom Mid", "Tom", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 42, "Rock Tom Floor", "Tom", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 43, "Jazz Tom Hi", "Tom", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 44, "Jazz Tom Mid", "Tom", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 45, "Jazz Tom Floor", "Tom", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 46, "Brush Tom Hi", "Tom", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 47, "Brush Tom Mid", "Tom", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 48, "Brush Tom Floor", "Tom", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 49, "Med HH Close", "Hi-Hat", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 50, "Med HH Open", "Hi-Hat", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 51, "Med HH Pedal", "Hi-Hat", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 52, "Standard HH Cl", "Hi-Hat", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 53, "Standard HH Op", "Hi-Hat", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 54, "Standard HH Pdl", "Hi-Hat", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 55, "Jazz HH Close", "Hi-Hat", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 56, "Jazz HH Open", "Hi-Hat", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 57, "Jazz HH Pedal", "Hi-Hat", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 58, "Brush HH Close", "Hi-Hat", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 59, "Brush HH Open", "Hi-Hat", true, true, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 60, "Standard Rd Edge", "Ride", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 61, "Standard Rd Bell", "Ride", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 62, "Std Rd Edge/Bell", "Ride", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 63, "Medium Ride Edge", "Ride", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 64, "Medium Ride Bell", "Ride", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 65, "Med Rd Edge/Bell", "Ride", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 66, "Flat 18\"Ride", "Ride", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 67, "Brush 18\"Ride", "Ride", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 68, "Brush 20\"Ride", "Ride", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 69, "Standard 16\"Cr R", "Crash", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 70, "Standard 16\"Cr L", "Crash", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 71, "Standard 18\"Cr R", "Crash", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 72, "Standard 18\"Cr L", "Crash", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 73, "Jazz 16\"Cr R", "Crash", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 74, "Jazz 16\"Cr L", "Crash", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 75, "Heavy 18\"Cr R", "Crash", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 76, "Heavy 18\"Cr L", "Crash", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 77, "Brush 16\"Cr R", "Crash", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 78, "Brush 16\"Cr L", "Crash", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 79, "Brush 18\"Cr R", "Crash", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 80, "Brush 18\"Cr L", "Crash", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 81, "Splash Cymbal 1", "Crash", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 82, "Splash Cymbal 2", "Crash", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 83, "Brush Splash Cym", "Crash", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 84, "China Cymbal", "Crash", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 85, "Orch Cymbal", "Crash", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 86, "Orch Mallet Cym", "Crash", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 87, "Gong", "Crash", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 88, "Timpani F2", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 89, "Timpani F#2", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 90, "Timpani G2", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 91, "Timpani G#2", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 92, "Timpani A2", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 93, "Timpani A#2", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 94, "Timpani B2", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 95, "Timpani C3", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 96, "Timpani C#3", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 97, "Timpani D3", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 98, "Timpani D#3", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 99, "Timpani E3", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 100, "Timpani F3", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 101, "Tambourine 1", "Percussion", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 102, "Tambourine 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 103, "Cowbell 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 104, "Cowbell 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 105, "Vibra-slap", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 106, "High Bongo 1", "Percussion", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 107, "Low Bongo 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 108, "High Bongo 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 109, "Low Bongo 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 110, "MuteHi Conga 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 111, "OpenHi Conga 1", "Percussion", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 112, "Low Conga 1", "Percussion", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 113, "MuteHi Conga 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 114, "OpenHi Conga 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 115, "Low Conga 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 116, "High Timbale", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 117, "Low Timbale", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 118, "High Agogo 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 119, "Low Agogo 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 120, "High Agogo 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 121, "Low Agogo 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 122, "Cabasa 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 123, "Cabasa 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 124, "Maracas 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 125, "Maracas 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 126, "Short Whistle", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 127, "Long Whistle", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 128, "Short Guiro", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 129, "Long Guiro", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 130, "Claves 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 131, "Claves 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 132, "Hi WoodBlock 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 133, "Low WoodBlock 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 134, "Hi WoodBlock 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 135, "Low WoodBlock 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 136, "Mute Cuica 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 137, "Open Cuica 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 138, "Mute Cuica 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 139, "Open Cuica 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 140, "Mute Triangle 1", "Percussion", false, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 141, "Open Triangle 1", "Percussion", false, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 142, "Mute Triangle 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 143, "Open Triangle 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 144, "Shaker", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 145, "Sleigh Bell 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 146, "Sleigh Bell 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 147, "Wind Chimes", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 148, "Castanets 1", "Percussion", true, false, "Flam/Buzz/Roll"));
            DrumInstruments.Add(new DrumInstrument("INT", 149, "Castanets 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 150, "Mute Surdo 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 151, "Open Surdo 1", "Percussion", true, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 152, "Mute Surdo 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 153, "Open Surdo 2", "Percussion", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 154, "Sticks", "Other", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 155, "Square Click", "Other", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 156, "Metro Click", "Other", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 157, "Metro Bell", "Other", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 158, "Hand Clap", "Other", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 159, "HighQ", "SFX", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 160, "Slap", "SFX", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 161, "Scratch Push", "SFX", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 162, "Scratch Pull", "SFX", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 163, "Gt Fret Noise", "SFX", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 164, "Gt Cutting Up Nz", "SFX", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 165, "Gt Cutting Dw Nz", "SFX", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 166, "AcBass Noise", "SFX", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 167, "Flute Key Click", "SFX", false, false, "Flam/Buzz"));
            DrumInstruments.Add(new DrumInstrument("INT", 168, "Applause", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 1, "Laughing 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 2, "Laughing 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 3, "Laughing 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 4, "Scream 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 5, "Scream 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 6, "Scream 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 7, "Punch 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 8, "Punch 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 9, "Punch 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 10, "Heart Beat 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 11, "Heart Beat 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 12, "Heart Beat 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 13, "Foot Steps 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 14, "Foot Steps 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 15, "Foot Steps 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 16, "Foot Step 1 A", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 17, "Foot Step 1 B", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 18, "Foot Step 2 A", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 19, "Foot Step 2 B", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 20, "Foot Step 3 A", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 21, "Foot Step 3 B", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 22, "Door Creaking 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 23, "Door Creaking 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 24, "Door Creaking 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 25, "Door Slam 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 26, "Door Slam 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 27, "Door Slam 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 28, "Scratch", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 29, "MetalScratch", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 30, "Matches", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 31, "Car Engine 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 32, "Car Engine 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 33, "Car Engine 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 34, "Car Stop 1 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 35, "Car Stop 1 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 36, "Car Stop 2 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 37, "Car Stop 2 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 38, "Car Stop 3 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 39, "Car Stop 3 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 40, "CarPassing 1 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 41, "CarPassing 1 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 42, "CarPassing 2 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 43, "CarPassing 2 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 44, "CarPassing 3 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 45, "CarPassing 3 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 46, "CarPassing 4", "SFX", false, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 47, "CarPassing 5", "SFX", false, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 48, "CarPassing 6", "SFX", false, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 49, "Car Crash 1 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 50, "Car Crash 1 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 51, "Car Crash 2 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 52, "Car Crash 2 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 53, "Car Crash 3 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 54, "Car Crash 3 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 55, "Crash 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 56, "Crash 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 57, "Crash 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 58, "Siren 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 59, "Siren 2 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 60, "Siren 2 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 61, "Siren 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 62, "Train 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 63, "Train 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 64, "Jetplane 1 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 65, "Jetplane 1 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 66, "Jetplane 2 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 67, "Jetplane 2 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 68, "Jetplane 3 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 69, "Jetplane 3 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 70, "Helicopter 1 L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 71, "Helicopter 1 R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 72, "Helicopter 2 L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 73, "Helicopter 2 R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 74, "Helicopter 3 L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 75, "Helicopter 3 R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 76, "Starship 1 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 77, "Starship 1 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 78, "Starship 2 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 79, "Starsmhip 2 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 80, "Starship 3 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 81, "Starship 3 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 82, "Gun Shot 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 83, "Gun Shot 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 84, "Gun Shot 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 85, "Machine Gun 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 86, "Machine Gun 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 87, "Machine Gun 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 88, "Laser Gun 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 89, "Laser Gun 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 90, "Laser Gun 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 91, "Explosion 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 92, "Explosion 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 93, "Explosion 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 94, "Dog 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 95, "Dog 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 96, "Dog 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 97, "Dog 4", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 98, "Horse 1 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 99, "Horse 1 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 100, "Horse 2 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 101, "Horse 2 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 102, "Horse 3 L>R", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 103, "Horse 3 R>L", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 104, "Birds 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 105, "Birds 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 106, "Rain 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 107, "Rain 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 108, "Thunder 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 109, "Thunder 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 110, "Thunder 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 111, "Wind", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 112, "Seashore", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 113, "Stream 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 114, "Stream 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 115, "Bubbles 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 116, "Bubbles 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 117, "Burst 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 118, "Burst 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 119, "Burst 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 120, "Burst 4", "SFX", false, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 121, "Glass Burst 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 122, "Glassm Burst 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 123, "Glass Burst 3", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 124, "Telephone 1", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 125, "Telephone 2", "SFX", true, false, ""));
            DrumInstruments.Add(new DrumInstrument("ExSN6", 126, "Telephone 3", "SFX", true, false, ""));
        }

        public DrumInstrument Get(String Bank, String Instrument)
        {
            UInt16 i = 0;
            while (i < DrumInstruments.Count())
            {
                if (Bank.StartsWith(DrumInstruments[i].Bank) && Instrument.StartsWith(DrumInstruments[i].Name.Trim()))
                {
                    return DrumInstruments[i];
                }
                i++;
            }
            return null;
        }
    }

    class SuperNATURALDrumKit
    {
        HBTrace t = new HBTrace("class SuperNATURALDrumKit");
        public SuperNATURALDrumKitCommon superNATURALDrumKitCommon { get; set; }
        public SuperNATURALDrumKitCommonCompEQ superNATURALDrumKitCommonCompEQ { get; set; }
        public SuperNATURALDrumKitKey superNATURALDrumKitKey { get; set; }

        public SuperNATURALDrumKit(ReceivedData Data)
        {
            t.Trace("public SuperNATURALDrumKit (" + "ReceivedData" + Data + ", " + ")");
            superNATURALDrumKitCommon = new SuperNATURALDrumKitCommon(Data);
        }
    }

    /// <summary>
    /// Subclasses
    /// </summary>

    class Phrases
    {
        public String[] Names;
        public Phrases()
        {
            Names = new String[] { "No Assign", "Piano 01", "Piano 02", "Piano 03", "Piano 04", "Piano 05", "Piano 06", "Piano 07", "Piano 08", "Piano 09", "Piano 10",
                "E.Piano 01", "E.Piano 02", "E.Piano 03", "E.Piano 04", "E.Piano 05", "E.Piano 06", "E.Organ 01", "E.Organ 02", "E.Organ 03", "E.Organ 04",
                "E.Organ 05", "E.Organ 06", "E.Organ 07", "E.Organ 08", "E.Organ 09", "E.Organ 10", "Pipe Organ 01", "Pipe Organ 02", "Reed Organ", "Harpsicord 01",
                "Harpsicord 02", "Clav 01", "Clav 02", "Celesta", "Accordion 01", "Accordion 02", "Harmonica", "Bell 01", "Music Box", "Vibraphone 01", "Vibraphone 02", "Vibraphone 03", "Vibraphone 04",
                "Marimba 01", "Marimba 02", "Glockenspiel", "Xylophon 01", "Xylophone 02", "Xylophone 03", "YangQin", "Santur 01", "Santur 02", "SteelDrums",
                "Ac.Guitar 01", "Ac.Guitar 02", "Ac.Guitar 03", "Ac.Guitar 04", "Ac.Guitar 05", "Mandolin 01", "Mandolin 02", "Ukulele", "Jazz Guitar 01", "Jazz Guitar 02", "Jazz Guitar 03",
                "E.Guitar", "Muted Guitar", "Pedal Steel", "Dist.Guitar 01", "Ac.Bass 01", "Ac.Bass 02", "E.Bass 01", "E.Bass 02", "Fretless Bass 01", "Fretless Bass 02", "Fretless Bass 03",
                "Slap Bass 01", "Slap Bass 02", "Synth Bass 01", "Synth Bass 02", "Synth Bass 03", "Synth Bass 04", "Synth Bass 05", "Synth Bass 06",
                "Plucked/Stroke", "Banjo", "Harp", "Koto", "Shamisen", "Sitar", "Violin 01", "Violin 02", "Fiddle", "Cello 01", " Cello 02", "Contrabass 01", "Contrabass 02",
                "Enssemble Strings 01", "Enssemble Strings 02", "Enssemble Strings 03", "Tremolo Strings", "Pizzicato Strings 01", "Pizzicato Strings 02",
                "Orchestra 01", "Orchestra 02", "Solo Brass", "Trumpet 01", "Trumpet 02", "Mute Trumpet", "Trombone", "French Horn", "Tuba", "Ensemble Brass 01",
                "French Horn Section", "Wind", "Oboe", "Clarinett", "Bassoon", "Bagpipe 01", "Bagpipe 02", "Shanai", "Shakuhachi", "Flute", "Soprano Sax 01", "Soprano Sax 02",
                "Alto Sax 01", "Alto Sax 02", "Tenor Sax 01", "Baritone Sax", "Recorder", "Vox/Choirs 01", "Vox/Choirs 02", "Scat 01", "Scat 02",
                "Synth Lead 01", "Synth Lead 02", "Synth Lead 03", "Synth Lead 04", "Synth Lead 05", "Synth Lead 06", "Synth Lead 07", "Synth Brass 01", "Synth Brass 02", "Synth Brass 03",
                "Synth Brass 04", "Synth Pad/Strings 01", "Synth Pad/Strings 02", "Synth Pad/Strings 03", "Synth BellPad 01", "Synth BellPad 02", "Synth BellPad 03", "Synth PolyKey 01",
                "Synth PolyKey 02", "Synth PolyKey 03", "Synth SeqPop 01", "Synth SeqPop 02", "Timpani 01", "Timpani 02", "Percussion", "Sound FX 01", "Sound FX 02", "Sound FX 03",
                "Bibraphone 05", "Dist.Guitar 02", "Dist.guitar 03", "E.Bass 03", "E.Bass 04", "Synth Bass 07", "Synth Bass 08", "Synth Bass 09", "Synth Bass 10", "Synth Bass 11", "Synth Bass 12",
                "Santur 03", "Ensemble Brass 02", "Tenor Sax 02", "Tenor Sax 03", "Pan Pipe", "Vox/Choirs 03", "Vox/Choirs 04", "Vox/Choirs 05", "Vox/Choirs 06", "Vox/Choirs 07", "Vox/Choirs 08",
                "Sunth Pad/Strings 04", "Synth Pad/Strings 05", "Synth Bell 01", "Synth Bell 02", "Synth Bell 03", "Synth Bell 04", "Synth Bell 05", "Synth PolyKey 04", "Synth PolyKey 05",
                "Synth PolyKey 06", "Synth PolyKey 07", "Synth PolyKey 08", "Synth PolyKey 09", "Synth PolyKey 10", "Bell 02", "Bell 03", "Synth PolyKey 11", "Synth Pad/Strings 06",
                "Synth Pad/Strings 07", "Synth Pad/Strings 08", "Sound FX 04", "Sound FX 05", "XV/Ac.Piano", "XV/El.Piano", "XV/Keyboards", "XV/Bell", "XV/Mallet", "XV/Organ",
                "XV/Accordion", "XV/Harmonica", "XV/Ac.Guitar", "XV/ElGuitar", "XV/Dist.Guitar", "XV/Bass", "XV/Synth Bass", "XV/Strings", "XV/Orchestra", "XV/Hit&Stab", "XV/Wind", "XV/Flute",
                "XV/Ac.Brass", "XV/Synth Brass", "XV/Sax", "XV/Hard Lead", "XV/Soft Lead", "XV/Techno Synth", "XV/Pulsating", "XV/Synth FX", "XV/Other Synth", "XV/Bright Pad", "Soft Pad", "XV/Vox",
                "XV/Plucked", "XV/Ethnic", "XV/Fretted", "XV/Percussion", "XV/Sound FX", "XV/Beat&Groove", "XV/Drums", "XV/Combination" };
        }
    }

    class CommonMFX
    {
        HBTrace t = new HBTrace("class CommonMFX");
        public byte MFXType { get; set; }
        public byte Reserve1 { get; set; }
        public byte MFXChorusSendLevel { get; set; }
        public byte MFXReverbSendLevel { get; set; }
        public byte Reserve2 { get; set; }
        public byte[] MFXControlSource { get; set; } // [4]
        public byte[] MFXControlSens { get; set; }   // [4]
        public byte[] MFXControlAssign { get; set; } // [4]
        public MFXNumberedParameters MFXNumberedParameters { get; set; }

        private ParameterSets sets;

        public CommonMFX(ReceivedData Data)
        {
            t.Trace("public CommonMFX (" + "ReceivedData" + Data + ", " + ")");
            sets = new ParameterSets();
            MFXControlSource = new byte[4];
            MFXControlSens = new byte[4];
            MFXControlAssign = new byte[4];
            MFXNumberedParameters = new MFXNumberedParameters(Data, 0x11);

            MFXType = Data.GetByte(0);
            Reserve1 = Data.GetByte(1);
            MFXChorusSendLevel = Data.GetByte(2);
            MFXReverbSendLevel = Data.GetByte(3);
            Reserve1 = Data.GetByte(4);

            for (byte i = 0; i < 4; i++)
            {
                MFXControlSource[i] = Data.GetByte((byte)(5 + 2 * i));
                MFXControlSens[i] = Data.GetByte((byte)(6 + 2 * i));
                MFXControlAssign[i] = Data.GetByte((byte)(13 + i));
            }
        }
    }

    /// <summary>
    /// Some parameters are not documented
    /// Address is 0x19, 0x01, 0x50, 0x00
    /// Length is 0x25 (37) bytes
    /// Given the address, this probably belongs to SuperNATURAL Synth Tone.
    /// Read it only for SuperNATURAL Synth Tone since it does not answer otherwise.
    /// Fill out info on any other parameters found, if ever.
    /// </summary>
    class Undocumented_Parameters
    {
        public byte Data_00 { get; set; }
        public byte Data_01 { get; set; }
        public byte Data_02 { get; set; }
        public byte Data_03 { get; set; }
        public byte Data_04 { get; set; }
        public byte Data_05 { get; set; } // Envelope Loop Sync Note
        public byte Data_06 { get; set; }
        public byte Data_07 { get; set; }
        public byte Data_08 { get; set; }
        public byte Data_09 { get; set; }
        public byte Data_10 { get; set; }
        public byte Data_11 { get; set; }
        public byte Data_12 { get; set; }
        public byte Data_13 { get; set; }
        public byte Data_14 { get; set; }
        public byte Data_15 { get; set; }
        public byte Data_16 { get; set; }
        public byte Data_17 { get; set; }
        public byte Data_18 { get; set; }
        public byte Data_19 { get; set; }
        public byte Data_20 { get; set; }
        public byte Data_21 { get; set; }
        public byte Data_22 { get; set; }
        public byte Data_23 { get; set; }
        public byte Data_24 { get; set; }
        public byte Data_25 { get; set; }
        public byte Data_26 { get; set; }
        public byte Data_27 { get; set; }
        public byte Data_28 { get; set; }
        public byte Data_29 { get; set; }
        public byte Data_30 { get; set; }
        public byte Data_31 { get; set; }
        public byte Data_32 { get; set; }
        public byte Data_33 { get; set; }
        public byte Data_34 { get; set; }
        public byte Data_35 { get; set; }
        public byte Data_36 { get; set; }

        public Undocumented_Parameters(ReceivedData Data)
        {
            Data_00 = Data.GetByte(00);
            Data_01 = Data.GetByte(01);
            Data_02 = Data.GetByte(02);
            Data_03 = Data.GetByte(03);
            Data_04 = Data.GetByte(04);
            Data_05 = Data.GetByte(05);
            Data_06 = Data.GetByte(06);
            Data_07 = Data.GetByte(07);
            Data_08 = Data.GetByte(08);
            Data_09 = Data.GetByte(09);
            Data_10 = Data.GetByte(10);
            Data_11 = Data.GetByte(11);
            Data_12 = Data.GetByte(12);
            Data_13 = Data.GetByte(13);
            Data_14 = Data.GetByte(14);
            Data_15 = Data.GetByte(15);
            Data_16 = Data.GetByte(16);
            Data_17 = Data.GetByte(17);
            Data_18 = Data.GetByte(18);
            Data_19 = Data.GetByte(19);
            Data_20 = Data.GetByte(20);
            Data_21 = Data.GetByte(21);
            Data_22 = Data.GetByte(22);
            Data_23 = Data.GetByte(23);
            Data_24 = Data.GetByte(24);
            Data_25 = Data.GetByte(25);
            Data_26 = Data.GetByte(26);
            Data_27 = Data.GetByte(27);
            Data_28 = Data.GetByte(28);
            Data_29 = Data.GetByte(29);
            Data_30 = Data.GetByte(30);
            Data_31 = Data.GetByte(31);
            Data_32 = Data.GetByte(32);
            Data_33 = Data.GetByte(33);
            Data_34 = Data.GetByte(34);
            Data_35 = Data.GetByte(35);
            Data_36 = Data.GetByte(36);
        }
    }

    /// <summary>
    /// Some commands are not documented
    /// </summary>
    class Undocumented_Commands
    {
        // Addresses:
        public byte[] Play { get; set; } // 0 = Stop Current part number (1-based!) = Play part.

        public Undocumented_Commands()
        {
            Play = new byte[] { 0x0f, 0x00, 0x20, 0x00 };
        }

    }

    class PCMSynthToneCommon
    {
        HBTrace t = new HBTrace("class PCMSynthToneCommon");
        public String Name { get; set; }
        public byte Level { get; set; }
        public byte Pan { get; set; }
        public byte Priority { get; set; }
        public byte CoarseTune { get; set; }
        public byte FineTune { get; set; }
        public byte OctaveShift { get; set; }
        public byte TuneDepth { get; set; }
        public byte AnalogFeel { get; set; }
        public byte MonoPoly { get; set; }
        public Boolean LegatoSwitch { get; set; }
        public Boolean LegatoRetrigger { get; set; }
        public Boolean PortamentoSwitch { get; set; }
        public byte PortamentoMode { get; set; }
        public byte PortamentoType { get; set; }
        public byte PortamentoStart { get; set; }
        public byte PortamentoTime { get; set; }
        public byte CutoffOffset { get; set; }
        public byte ResonanceOffset { get; set; }
        public byte AttackTimeOffset { get; set; }
        public byte ReleaseTimeOffset { get; set; }
        public byte VelocitySenseOffset { get; set; }
        public Boolean PMTControlSwitch { get; set; }
        public byte PitchBendRangeUp { get; set; }
        public byte PitchBendRangeDown { get; set; }
        public byte[] MatrixControlSource { get; set; }        // [4]
        public byte[][] MatrixControlDestination { get; set; } // [4][4]
        public byte[][] MatrixControlSens { get; set; }       // [4][4]

        public PCMSynthToneCommon(ReceivedData Data)
        {
            t.Trace("public PCMSynthToneCommon (" + "ReceivedData" + Data + ", " + ")");
            MatrixControlSource = new byte[4];
            MatrixControlDestination = new byte[4][];
            MatrixControlSens = new byte[4][];
            for (byte i = 0; i < 4; i++)
            {
                MatrixControlDestination[i] = new byte[4];
                MatrixControlSens[i] = new byte[4];
            }

            Name = "";
            for (byte i = 0x00; i < 0x0c; i++)
            {
                Name += (char)Data.GetByte(i);
            }
            Level = Data.GetByte(0x0e);
            Pan = Data.GetByte(0x0f);
            Priority = Data.GetByte(0x10);
            CoarseTune = Data.GetByte(0x11);
            FineTune = Data.GetByte(0x12);
            OctaveShift = Data.GetByte(0x13);
            TuneDepth = Data.GetByte(0x14);
            AnalogFeel = Data.GetByte(0x15);
            MonoPoly = Data.GetByte(0x16);
            LegatoSwitch = Data.GetByte(0x17) > 0;
            LegatoRetrigger = Data.GetByte(0x18) > 0;
            PortamentoSwitch = Data.GetByte(0x19) > 0;
            PortamentoMode = Data.GetByte(0x1a);
            PortamentoType = Data.GetByte(0x1b);
            PortamentoStart = Data.GetByte(0x1c);
            PortamentoTime = Data.GetByte(0x1d);
            CutoffOffset = Data.GetByte(0x22);
            ResonanceOffset = Data.GetByte(0x23);
            AttackTimeOffset = Data.GetByte(0x24);
            ReleaseTimeOffset = Data.GetByte(0x25);
            VelocitySenseOffset = Data.GetByte(0x26);
            PMTControlSwitch = Data.GetByte(0x28) > 0;
            PitchBendRangeUp = Data.GetByte(0x29);
            PitchBendRangeDown = Data.GetByte(0x2a);
            for (byte i = 0; i < 4; i++)
            {
                MatrixControlSource[i] = Data.GetByte(0x2b + (9 * i));
                for (byte j = 0; j < 4; j++)
                {
                    MatrixControlDestination[i][j] = Data.GetByte(0x2c + (9 * i) + (j * 2));
                    MatrixControlSens[i][j] = Data.GetByte(0x2d + (9 * i) + (j * 2));
                }
            }
        }

    }

    class PCMSynthTonePMT // Partial Mapping Table
    {
        HBTrace t = new HBTrace("class PCMSynthTonePMT // Partial Mapping Table");
        public byte StructureType1_2 { get; set; }
        public byte Booster1_2 { get; set; }
        public byte StructureType3_4 { get; set; }
        public byte Booster3_4 { get; set; }
        public byte PMTVelocityControl { get; set; }
        public Boolean[] PMTPartialSwitch { get; set; }       // [4]
        public byte[] PMTKeyboardRangeLower { get; set; }     // [4]
        public byte[] PMTKeyboardRangeUpper { get; set; }     // [4]
        public byte[] PMTKeyboardFadeWidthLower { get; set; } // [4]
        public byte[] PMTKeyboardFadeWidthUpper { get; set; } // [4]
        public byte[] PMTVelocityRangeLower { get; set; }     // [4]
        public byte[] PMTVelocityRangeUpper { get; set; }     // [4]
        public byte[] PMTVelocityFadeWidthLower { get; set; } // [4]
        public byte[] PMTVelocityFadeWidthUpper { get; set; } // [4]

        public PCMSynthTonePMT(ReceivedData Data)
        {
            t.Trace("public PCMSynthTonePMT (" + "ReceivedData" + Data + ", " + ")");
            PMTPartialSwitch = new Boolean[4];
            PMTKeyboardRangeLower = new byte[4];
            PMTKeyboardRangeUpper = new byte[4];
            PMTKeyboardFadeWidthLower = new byte[4];
            PMTKeyboardFadeWidthUpper = new byte[4];
            PMTVelocityRangeLower = new byte[4];
            PMTVelocityRangeUpper = new byte[4];
            PMTVelocityFadeWidthLower = new byte[4];
            PMTVelocityFadeWidthUpper = new byte[4];

            StructureType1_2 = Data.GetByte(0x00);
            Booster1_2 = Data.GetByte(0x01);
            StructureType3_4 = Data.GetByte(0x02);
            Booster3_4 = Data.GetByte(0x03);
            PMTVelocityControl = Data.GetByte(0x04);
            for (byte i = 0; i < 4; i++)
            {
                PMTPartialSwitch[i] = Data.GetByte(0x05 + 9 * i) > 0;
                PMTKeyboardRangeLower[i] = Data.GetByte(0x06 + 9 * i);
                PMTKeyboardRangeUpper[i] = Data.GetByte(0x07 + 9 * i);
                PMTKeyboardFadeWidthLower[i] = Data.GetByte(0x08 + 9 * i);
                PMTKeyboardFadeWidthUpper[i] = Data.GetByte(0x09 + 9 * i);
                PMTVelocityRangeLower[i] = Data.GetByte(0x0a + 9 * i);
                PMTVelocityRangeUpper[i] = Data.GetByte(0x0b + 9 * i);
                PMTVelocityFadeWidthLower[i] = Data.GetByte(0x0c + 9 * i);
                PMTVelocityFadeWidthUpper[i] = Data.GetByte(0x0d + 9 * i);
            }
        }
    }

    class LFO
    {
        HBTrace t = new HBTrace("class LFO");
        public byte LFOWaveform { get; set; }
        public byte LFORate { get; set; }
        public byte LFOOffset { get; set; }
        public byte LFORateDetune { get; set; }
        public byte LFODelayTime { get; set; }
        public byte LFODelayTimeKeyfollow { get; set; }
        public byte LFOFadeMode { get; set; }
        public byte LFOFadeTime { get; set; }
        public Boolean LFOKeyTrigger { get; set; }
        public byte LFOPitchDepth { get; set; }
        public byte LFOTVFDepth { get; set; }
        public byte LFOTVADepth { get; set; }
        public byte LFOPanDepth { get; set; }

        public LFO(ReceivedData Data, byte msb, byte lsb)
        {
            t.Trace("public LFO (" + "ReceivedData" + Data + ", " + "byte" + msb + ", " + "byte" + lsb + ", " + ")");
            LFOWaveform = Data.GetByte(256 * msb + lsb + 0x00);
            LFORate = (byte)(16 * Data.GetByte(256 * msb + lsb + 0x01) + Data.GetByte(256 * msb + lsb + 0x02));
            LFOOffset = Data.GetByte(256 * msb + lsb + 0x03);
            LFORateDetune = Data.GetByte(256 * msb + lsb + 0x04);
            LFODelayTime = Data.GetByte(256 * msb + lsb + 0x05);
            LFODelayTimeKeyfollow = Data.GetByte(256 * msb + lsb + 0x06);
            LFOFadeMode = Data.GetByte(256 * msb + lsb + 0x07);
            LFOFadeTime = Data.GetByte(256 * msb + lsb + 0x08);
            LFOKeyTrigger = Data.GetByte(256 * msb + lsb + 0x09) > 0;
            LFOPitchDepth = Data.GetByte(256 * msb + lsb + 0x0a);
            LFOTVFDepth = Data.GetByte(256 * msb + lsb + 0x0b);
            LFOTVADepth = Data.GetByte(256 * msb + lsb + 0x0c);
            LFOPanDepth = Data.GetByte(256 * msb + lsb + 0x0d);
        }
    }

    class TVA
    {
        HBTrace t = new HBTrace("class TVA");
        public byte TVALevelVelocityCurve { get; set; }
        public byte TVALevelVelocitySens { get; set; }
        public byte TVAEnvTime1VelocitySens { get; set; }
        public byte TVAEnvTime4VelocitySens { get; set; }
        public byte TVAEnvTimeKeyfollow { get; set; }
        public byte[] TVAEnvTime { get; set; } // [4]
        public byte[] TVAEnvLevel { get; set; } // [3]

        public TVA(ReceivedData Data, byte msb, byte lsb, Boolean keyFollow)
        {
            t.Trace("public TVA (" + "ReceivedData" + Data + ", " + "byte" + msb + ", " + "byte" + lsb + ", " + ")");
            TVAEnvTime = new byte[4];
            TVAEnvLevel = new byte[3];

            TVALevelVelocityCurve = Data.GetByte(msb, lsb, 0x00);
            TVALevelVelocitySens = Data.GetByte(msb, lsb, 0x01);
            TVAEnvTime1VelocitySens = Data.GetByte(msb, lsb, 0x02);
            TVAEnvTime4VelocitySens = Data.GetByte(msb, lsb, 0x03);
            byte offset = 0;
            if (keyFollow)
            {
                TVAEnvTimeKeyfollow = Data.GetByte(msb, lsb, 0x04);
                offset = 1;
            }
            for (byte i = 0; i < 4; i++)
            {
                TVAEnvTime[i] = Data.GetByte(msb, lsb, (byte)(0x04 + i + offset));
            }
            for (byte i = 0; i < 3; i++)
            {
                TVAEnvLevel[i] = Data.GetByte(msb, lsb, (byte)(0x08 + i + offset));
            }
        }
    }

    class TVF
    {
        HBTrace t = new HBTrace("class TVF");
        public byte TVFFilterType { get; set; }
        public byte TVFCutoffFrequency { get; set; }
        public byte TVFCutoffKeyfollow { get; set; }
        public byte TVFCutoffVelocityCurve { get; set; }
        public byte TVFCutoffVelocitySens { get; set; }
        public byte TVFResonance { get; set; }
        public byte TVFResonanceVelocitySens { get; set; }
        public byte TVFEnvDepth { get; set; }
        public byte TVFEnvVelocityCurve { get; set; }
        public byte TVFEnvVelocitySens { get; set; }
        public byte TVFEnvTime1VelocitySens { get; set; }
        public byte TVFEnvTime4VelocitySens { get; set; }
        public byte TVFEnvTimeKeyfollow { get; set; }
        public byte[] TVFEnvTime { get; set; } // [4]
        public byte[] TVFEnvLevel { get; set; } // [5]

        public TVF(ReceivedData Data, byte msb, byte lsb, Boolean keyFollow)
        {
            t.Trace("public TVF (" + "ReceivedData" + Data + ", " + "byte" + msb + ", " + "byte" + lsb + ", " + ")");
            TVFEnvTime = new byte[4];
            TVFEnvLevel = new byte[5];

            TVFFilterType = Data.GetByte(msb, lsb, 0x00);
            TVFCutoffFrequency = Data.GetByte(msb, lsb, 0x01);
            byte offset = 0;
            if (keyFollow)
            {
                TVFCutoffKeyfollow = Data.GetByte(msb, lsb, 0x02);
                TVFEnvTimeKeyfollow = Data.GetByte(msb, lsb, 0x0c);
                offset = 1;
            }
            TVFCutoffVelocityCurve = Data.GetByte(msb, lsb, (byte)(0x02 + offset));
            TVFCutoffVelocitySens = Data.GetByte(msb, lsb, (byte)(0x03 + offset));
            TVFResonance = Data.GetByte(msb, lsb, (byte)(0x04 + offset));
            TVFResonanceVelocitySens = Data.GetByte(msb, lsb, (byte)(0x05 + offset));
            TVFEnvDepth = Data.GetByte(msb, lsb, (byte)(0x06 + offset));
            TVFEnvVelocityCurve = Data.GetByte(msb, lsb, (byte)(0x07 + offset));
            TVFEnvVelocitySens = Data.GetByte(msb, lsb, (byte)(0x08 + offset));
            TVFEnvTime1VelocitySens = Data.GetByte(msb, lsb, (byte)(0x09 + offset));
            TVFEnvTime4VelocitySens = Data.GetByte(msb, lsb, (byte)(0x0a + offset));
            if (keyFollow)
            {
                offset = 2;
            }
            for (byte i = 0; i < 4; i++)
            {
                TVFEnvTime[i] = Data.GetByte(msb, lsb, (byte)(0x0b + i + offset));
            }
            for (byte i = 0; i < 5; i++)
            {
                TVFEnvLevel[i] = Data.GetByte(msb, lsb, (byte)(0x0f + i + offset));
            }
        }
    }

    class WMT
    {
        HBTrace t = new HBTrace("class WMT");
        public Boolean WMTWaveSwitch { get; set; }
        public byte WMTWaveGroupType { get; set; }
        public UInt16 WMTWaveGroupID { get; set; }
        public UInt16 WMTWaveNumberL { get; set; }
        public UInt16 WMTWaveNumberR { get; set; }
        public byte WMTWaveGain { get; set; }
        public Boolean WMTWaveFXMSwitch { get; set; }
        public byte WMTWaveFXMColor { get; set; }
        public byte WMTWaveFXMDepth { get; set; }
        public Boolean WMTWaveTempoSync { get; set; }
        public byte WMTWaveCoarseTune { get; set; }
        public byte WMTWaveFineTune { get; set; }
        public byte WMTWavePan { get; set; }
        public Boolean WMTWaveRandomPanSwitch { get; set; }
        public byte WMTWaveAlternatePanSwitch { get; set; }
        public byte WMTWaveLevel { get; set; }
        public byte WMTVelocityRangeLower { get; set; }
        public byte WMTVelocityRangeUpper { get; set; }
        public byte WMTVelocityFadeWidthLower { get; set; }
        public byte WMTVelocityFadeWidthUpper { get; set; }

        public WMT(ReceivedData Data, byte msb, byte lsb, byte index)
        {
            t.Trace("public WMT (" + "ReceivedData" + Data + ", " + "byte" + msb + ", " + "byte" + lsb + ", " + "byte" + index + ", " + ")");
            UInt16 offset = (UInt16)(msb * 16 + lsb + 29 * index);
            WMTWaveSwitch = Data.GetByte(offset + 0x00) > 0;
            WMTWaveGroupType = Data.GetByte(offset + 0x01);
            WMTWaveGroupID = Data.Get4Byte(offset + 0x02);
            WMTWaveNumberL = Data.Get4Byte(offset + 0x06);
            WMTWaveNumberR = Data.Get4Byte(offset + 0x0a);
            WMTWaveGain = Data.GetByte(offset + 0x0e);
            WMTWaveFXMSwitch = Data.GetByte(offset + 0x0f) > 0;
            WMTWaveFXMColor = Data.GetByte(offset + 0x10);
            WMTWaveFXMDepth = Data.GetByte(offset + 0x11);
            WMTWaveTempoSync = Data.GetByte(offset + 0x12) > 0;
            WMTWaveCoarseTune = Data.GetByte(offset + 0x13);
            WMTWaveFineTune = Data.GetByte(offset + 0x14);
            WMTWavePan = Data.GetByte(offset + 0x15);
            WMTWaveRandomPanSwitch = Data.GetByte(offset + 0x16) > 0;
            WMTWaveAlternatePanSwitch = Data.GetByte(offset + 0x17);
            WMTWaveLevel = Data.GetByte(offset + 0x18);
            WMTVelocityRangeLower = Data.GetByte(offset + 0x19);
            WMTVelocityRangeUpper = Data.GetByte(offset + 0x1a);
            WMTVelocityFadeWidthLower = Data.GetByte(offset + 0x1b);
            WMTVelocityFadeWidthUpper = Data.GetByte(offset + 0x1c);
        }
    }

    class PitchEnv
    {
        HBTrace t = new HBTrace("class PitchEnv");
        public byte PitchEnvDepth { get; set; }
        public byte PitchEnvVelocitySens { get; set; }
        public byte PitchEnvTime1VelocitySens { get; set; }
        public byte PitchEnvTime4VelocitySens { get; set; }
        public byte PitchEnvTimeKeyfollow { get; set; }
        public byte[] PitchEnvTime { get; set; }  // [4]
        public byte[] PitchEnvLevel { get; set; } // [5]

        public PitchEnv(ReceivedData Data, byte msb, byte lsb, Boolean keyFollow)
        {
            t.Trace("public PitchEnv (" + "ReceivedData" + Data + ", " + "byte" + msb + ", " + "byte" + lsb + ", " + ")");
            PitchEnvTime = new byte[4];
            PitchEnvLevel = new byte[5];

            PitchEnvDepth = Data.GetByte(msb, lsb, 0);
            PitchEnvVelocitySens = Data.GetByte(msb, lsb, 1);
            PitchEnvTime1VelocitySens = Data.GetByte(msb, lsb, 2);
            PitchEnvTime4VelocitySens = Data.GetByte(msb, lsb, 3);
            byte offset = 0;
            if (keyFollow)
            {
                PitchEnvTimeKeyfollow = Data.GetByte(msb, lsb, 4);
                offset = 1;
            }
            for (byte i = 0; i < 4; i++)
            {
                PitchEnvTime[i] = Data.GetByte(msb, lsb, (byte)(4 + i + offset));
            }
            for (byte i = 0; i < 5; i++)
            {
                PitchEnvLevel[i] = Data.GetByte(msb, lsb, (byte)(8 + i + offset));
            }
        }
    }

    class PCMSynthTonePartial
    {
        HBTrace t = new HBTrace("class PCMSynthTonePartial");
        public TVA TVA { get; set; }
        public TVF TVF { get; set; }
        public LFO LFO1 { get; set; }
        public LFO LFO2 { get; set; }
        public PitchEnv PitchEnv { get; set; }

        public byte PartialLevel { get; set; }
        public byte PartialCoarseTune { get; set; }
        public byte PartialFineTune { get; set; }
        public byte PartialRandomPitchDepth { get; set; }
        public byte PartialPan { get; set; }
        public byte PartialPanKeyfollow { get; set; }
        public byte PartialRandomPanDepth { get; set; }
        public byte PartialAlternatePanDepth { get; set; }
        public byte PartialEnvMode { get; set; }
        public byte PartialDelayMode { get; set; }
        public byte PartialDelayTime { get; set; }
        public byte PartialOutputLevel { get; set; }
        public byte PartialChorusSendLevel { get; set; }
        public byte PartialReverbSendLevel { get; set; }
        public Boolean PartialReceiveBender { get; set; }
        public Boolean PartialReceiveExpression { get; set; }
        public Boolean PartialReceiveHold_1 { get; set; }
        public Boolean PartialRedamperSwitch { get; set; }
        public byte[][] PartialControlSwitch { get; set; } // [4][4]
        public byte WaveGroupType { get; set; }
        public UInt16 WaveGroupID { get; set; }
        public UInt16 WaveNumberL { get; set; }
        public UInt16 WaveNumberR { get; set; }
        public byte WaveGain { get; set; }
        public Boolean WaveFXMSwitch { get; set; }
        public byte WaveFXMColor { get; set; }
        public byte WaveFXMDepth { get; set; }
        public Boolean WaveTempoSync { get; set; }
        public byte WavePitchKeyfollow { get; set; }
        public byte BiasLevel { get; set; }
        public byte BiasPosition { get; set; }
        public byte BiasDirection { get; set; }
        public byte LFOStepType { get; set; }
        public byte[] LFOStep { get; set; }       // [16]

        public PCMSynthTonePartial(ReceivedData Data)
        {
            t.Trace("public PCMSynthTonePartial (" + "ReceivedData" + Data + ", " + ")");

            TVA = new TVA(Data, 0x00, 0x61, true);
            TVF = new TVF(Data, 0x00, 0x48, true);
            LFO1 = new LFO(Data, 0x00, 0x6d);
            LFO2 = new LFO(Data, 0x00, 0x7b);
            PitchEnv = new PitchEnv(Data, 0x00, 0x3a, true);
            PartialControlSwitch = new byte[4][];
            LFOStep = new byte[16];

            PartialLevel = Data.GetByte(0x00);
            PartialCoarseTune = Data.GetByte(0x01);
            PartialFineTune = Data.GetByte(0x02);
            PartialRandomPitchDepth = Data.GetByte(0x03);
            PartialPan = Data.GetByte(0x04);
            PartialPanKeyfollow = Data.GetByte(0x05);
            PartialRandomPanDepth = Data.GetByte(0x06);
            PartialAlternatePanDepth = Data.GetByte(0x07);
            PartialEnvMode = Data.GetByte(0x08);
            PartialDelayMode = Data.GetByte(0x09);
            PartialDelayTime = (byte)(16 * Data.GetByte(0x0a) + Data.GetByte(0x0b));
            PartialOutputLevel = Data.GetByte(0x0c);
            PartialChorusSendLevel = Data.GetByte(0x0f);
            PartialReverbSendLevel = Data.GetByte(0x10);
            PartialReceiveBender = Data.GetByte(0x12) > 0;
            PartialReceiveExpression = Data.GetByte(0x13) > 0;
            PartialReceiveHold_1 = Data.GetByte(0x14) > 0;
            PartialRedamperSwitch = Data.GetByte(0x16) > 0;
            for (byte i = 0; i < 4; i++)
            {
                PartialControlSwitch[i] = new byte[4];
                for (byte j = 0; j < 4; j++)
                {
                    PartialControlSwitch[i][j] = Data.GetByte(0x17 + 4 * i + j);
                }
            }
            WaveGroupType = Data.GetByte(0x27);
            WaveGroupID = (UInt16)(16 * 16 * 16 * Data.GetByte(0x28) + 16 * 16 * Data.GetByte(0x29) + 16 * Data.GetByte(0x2a) + Data.GetByte(0x2b));
            WaveNumberL = (UInt16)(16 * 16 * 16 * Data.GetByte(0x2c) + 16 * 16 * Data.GetByte(0x2d) + 16 * Data.GetByte(0x2e) + Data.GetByte(0x2f));
            WaveNumberR = (UInt16)(16 * 16 * 16 * Data.GetByte(0x30) + 16 * 16 * Data.GetByte(0x31) + 16 * Data.GetByte(0x32) + Data.GetByte(0x33));
            WaveGain = Data.GetByte(0x34);
            WaveFXMSwitch = Data.GetByte(0x35) > 0;
            WaveFXMColor = Data.GetByte(0x36);
            WaveFXMDepth = Data.GetByte(0x37);
            WaveTempoSync = Data.GetByte(0x38) > 0;
            WavePitchKeyfollow = Data.GetByte(0x39);
            BiasLevel = Data.GetByte(0x5e);
            BiasPosition = Data.GetByte(0x5f);
            BiasDirection = Data.GetByte(0x60);
            LFOStepType = Data.GetByte(0x89);
            for (byte i = 0; i < 16; i++)
            {
                LFOStep[i] = Data.GetByte(0x8a + i);
            }
        }
    }

    class PCMSynthToneCommon2
    {
        HBTrace t = new HBTrace("class PCMSynthToneCommon2");
        public byte ToneCategory { get; set; }
        public byte MissingInDocs { get; set; }
        public byte PhraseOctaveShift { get; set; }
        public Boolean TFXSwitch { get; set; }
        public UInt16 PhraseNumber { get; set; }

        public PCMSynthToneCommon2(ReceivedData Data)
        {
            t.Trace("public PCMSynthToneCommon2 (" + "ReceivedData" + Data + ", " + ")");
            ToneCategory = Data.GetByte(0x10);
            MissingInDocs = (byte)(16 * Data.GetByte(0x11) + Data.GetByte(0x12));
            PhraseOctaveShift = Data.GetByte(0x13);
            TFXSwitch = Data.GetByte(0x33) > 0;
            PhraseNumber = (UInt16)(16 * Data.GetByte(0x3a) + Data.GetByte(0x3b));
        }
    }

    class PCMDrumKitCommon
    {
        HBTrace t = new HBTrace("class PCMDrumKitCommon");
        public String Name { get; set; }
        public byte DrumKitLevel { get; set; }
        public PCMDrumKitCommon(ReceivedData Data)
        {
            t.Trace("public PCMDrumKitCommon (" + "ReceivedData" + Data + ", " + ")");
            Name = "";
            for (Int32 i = 0x00; i < 0x0c; i++)
            {
                Name += (char)Data.GetByte(i);
            }
            DrumKitLevel = Data.GetByte(0x0c);
        }
    }

    class CompEq
    {
        HBTrace t = new HBTrace("class CompEq");
        public Boolean CompSwitch { get; set; }
        public byte CompAttackTime { get; set; }
        public byte CompReleaseTime { get; set; }
        public byte CompThreshold { get; set; }
        public byte CompRatio { get; set; }
        public byte CompOutputGain { get; set; }
        public Boolean EQSwitch { get; set; }
        public byte EQLowFreq { get; set; }
        public byte EQLowGain { get; set; }
        public byte EQMidFreq { get; set; }
        public byte EQMidGain { get; set; }
        public byte EQMidQ { get; set; }
        public byte EQHighFreq { get; set; }
        public byte EQHighGain { get; set; }

        public void SetContent(byte i, ReceivedData Data)
        {
            t.Trace("public void SetContent (" + "byte" + i + ", " + "ReceivedData" + Data + ", " + ")");
            byte address = (byte)(i * 0x0e);
            CompSwitch = Data.GetByte(address + 0) > 0;
            CompAttackTime = Data.GetByte(address + 1);
            CompReleaseTime = Data.GetByte(address + 2);
            CompThreshold = Data.GetByte(address + 3);
            CompRatio = Data.GetByte(address + 4);
            CompOutputGain = Data.GetByte(address + 5);
            EQSwitch = Data.GetByte(address + 6) > 0;
            EQLowFreq = Data.GetByte(address + 7);
            EQLowGain = Data.GetByte(address + 8);
            EQMidFreq = Data.GetByte(address + 9);
            EQMidGain = Data.GetByte(address + 10);
            EQMidQ = Data.GetByte(address + 11);
            EQHighFreq = Data.GetByte(address + 12);
            EQHighGain = Data.GetByte(address + 13);
        }
    }

    class PCMDrumKitCommonCompEQ
    {
        HBTrace t = new HBTrace("class PCMDrumKitCommonCompEQ");
        public CompEq[] CompEq { get; set; } // [6]

        public PCMDrumKitCommonCompEQ(ReceivedData Data)
        {
            t.Trace("public PCMDrumKitCommonCompEQ (" + "ReceivedData" + Data + ", " + ")");
            CompEq = new CompEq[6];
            for (byte i = 0; i < 6; i++)
            {
                CompEq[i] = new CompEq();
                CompEq[i].SetContent(i, Data);
            }
        }
    }

    class PCMDrumKitPartial
    {
        HBTrace t = new HBTrace("class PCMDrumKitPartial");
        public TVF TVF { get; set; }
        public TVA TVA { get; set; }
        public WMT[] WMT { get; set; } // [4]
        public PitchEnv PitchEnv { get; set; }

        public String Name { get; set; }
        public byte AssignType { get; set; }
        public byte MuteGroup { get; set; }
        public byte PartialLevel { get; set; }
        public byte PartialCoarseTune { get; set; }
        public byte PartialFineTune { get; set; }
        public byte PartialRandomPitchDepth { get; set; }
        public byte PartialPan { get; set; }
        public byte PartialRandomPanDepth { get; set; }
        public byte PartialAlternatePanDepth { get; set; }
        public byte PartialEnvMode { get; set; }
        public byte PartialOutputLevel { get; set; }
        public byte PartialChorusSendLevel { get; set; }
        public byte PartialReverbSendLevel { get; set; }
        public byte PartialOutputAssign { get; set; }
        public byte PartialPitchBendRange { get; set; }
        public Boolean PartialReceiveExpression { get; set; }
        public Boolean PartialReceiveHold_1 { get; set; }
        public byte WMTVelocityControl { get; set; }
        public Boolean OneShotMode { get; set; }
        public byte RelativeLevel { get; set; }

        public PCMDrumKitPartial(ReceivedData Data)
        {
            t.Trace("public PCMDrumKitPartial (" + "ReceivedData" + Data + ", " + ")");
            TVF = new TVF(Data, 0x01, 0x22, false);
            TVA = new TVA(Data, 0x01, 0x36, false);
            WMT = new WMT[4];
            for (byte i = 0; i < 4; i++)
            {
                WMT[i] = new WMT(Data, 0x00, 0x21, i);
            }
            PitchEnv = new PitchEnv(Data, 0x01, 0x15, false);

            Name = "";
            for (byte i = 0; i < 0x0c; i++)
            {
                Name += (char)Data.GetByte(i);
            }
            AssignType = Data.GetByte(0x0c);
            MuteGroup = Data.GetByte(0x0d);
            PartialLevel = Data.GetByte(0x0e);
            PartialCoarseTune = Data.GetByte(0x0f);
            PartialFineTune = Data.GetByte(0x10);
            PartialRandomPitchDepth = Data.GetByte(0x11);
            PartialPan = Data.GetByte(0x12);
            PartialRandomPanDepth = Data.GetByte(0x13);
            PartialAlternatePanDepth = Data.GetByte(0x14);
            PartialEnvMode = Data.GetByte(0x15);
            PartialOutputLevel = Data.GetByte(0x16);
            PartialChorusSendLevel = Data.GetByte(0x19);
            PartialReverbSendLevel = Data.GetByte(0x1a);
            PartialOutputAssign = Data.GetByte(0x1b);
            PartialPitchBendRange = Data.GetByte(0x1c);
            PartialReceiveExpression = Data.GetByte(0x1d) > 0;
            PartialReceiveHold_1 = Data.GetByte(0x1e) > 0;
            WMTVelocityControl = Data.GetByte(0x20);
            OneShotMode = Data.GetByte(0x141) > 1;
            RelativeLevel = Data.GetByte(0x142);
        }
    }

    class PCMDrumKitCommon2
    {
        HBTrace t = new HBTrace("class PCMDrumKitCommon2");
        public byte PhraseNumber { get; set; }
        public byte TFXSwitch { get; set; }

        public PCMDrumKitCommon2(ReceivedData Data)
        {
            t.Trace("public PCMDrumKitCommon2 (" + "ReceivedData" + Data + ", " + ")");
            PhraseNumber = Data.Get2Byte(16);
            TFXSwitch = Data.GetByte(18);
        }
    }

    class SuperNATURALSynthToneCommon
    {
        HBTrace t = new HBTrace("class SuperNATURALSynthToneCommon");
        public String Name { get; set; }
        public byte ToneLevel { get; set; }
        public Boolean PortamentoSwitch { get; set; }
        public byte PortamentoTime { get; set; }
        public byte MonoPoly { get; set; }
        public byte OctaveShift { get; set; }
        public byte PitchBendRangeUp { get; set; }
        public byte PitchBendRangeDown { get; set; }
        public Boolean Partial1Switch { get; set; }
        public byte Partial1Select { get; set; }
        public Boolean Partial2Switch { get; set; }
        public byte Partial2Select { get; set; }
        public Boolean Partial3Switch { get; set; }
        public byte Partial3Select { get; set; }
        public Boolean RINGSwitch { get; set; } // 0 - 2!
        public Boolean TFXSwitch { get; set; }
        public Boolean UnisonSwitch { get; set; }
        public byte PortamentoMode { get; set; }
        public Boolean LegatoSwitch { get; set; }
        public byte AnalogFeel { get; set; }
        public byte WaveShape { get; set; }
        public byte Category { get; set; }
        public UInt16 PhraseNumber { get; set; }
        public byte PhraseOctaveShift { get; set; }
        public byte UnisonSize { get; set; }

        public SuperNATURALSynthToneCommon(ReceivedData Data)
        {
            t.Trace("public SuperNATURALSynthToneCommon (" + "ReceivedData" + Data + ", " + ")");
            Name = "";
            for (byte i = 0; i < 0x0c; i++)
            {
                Name += (char)Data.GetByte(i);
            }
            ToneLevel = Data.GetByte(0x0c);
            PortamentoSwitch = Data.GetByte(0x12) > 0;
            PortamentoTime = Data.GetByte(0x13);
            MonoPoly = Data.GetByte(0x14);
            OctaveShift = Data.GetByte(0x15);
            PitchBendRangeUp = Data.GetByte(0x16);
            PitchBendRangeDown = Data.GetByte(0x17);
            Partial1Switch = Data.GetByte(0x19) > 0;
            Partial1Select = Data.GetByte(0x1a);
            Partial2Switch = Data.GetByte(0x1b) > 0;
            Partial2Select = Data.GetByte(0x1c);
            Partial3Switch = Data.GetByte(0x1d) > 0;
            Partial3Select = Data.GetByte(0x1e);
            RINGSwitch = Data.GetByte(0x1f) > 0;
            TFXSwitch = Data.GetByte(0x20) > 0;
            UnisonSwitch = Data.GetByte(0x2e) > 0;
            PortamentoMode = Data.GetByte(0x31);
            LegatoSwitch = Data.GetByte(0x32) > 0;
            AnalogFeel = Data.GetByte(0x34);
            WaveShape = Data.GetByte(0x35);
            Category = Data.GetByte(0x36);
            PhraseNumber = Data.Get4Byte(0x37);
            PhraseOctaveShift = Data.GetByte(0x3b);
            UnisonSize = Data.GetByte(0x3c);
        }
    }

    class SuperNATURALSynthTonePartial
    {
        HBTrace t = new HBTrace("class SuperNATURALSynthTonePartial");
        public byte OSCWave { get; set; }
        public byte OSCWaveVariation { get; set; }
        public byte OSCPitch { get; set; }
        public byte OSCDetune { get; set; }
        public byte OSCPulseWidthModDepth { get; set; }
        public byte OSCPulseWidth { get; set; }
        public byte OSCPitchEnvAttackTime { get; set; }
        public byte OSCPitchEnvDecay { get; set; }
        public byte OSCPitchEnvDepth { get; set; }
        public byte FILTERMode { get; set; }
        public byte FILTERSlope { get; set; }
        public byte FILTERCutoff { get; set; }
        public byte FILTERCutoffKeyfollow { get; set; }
        public byte FILTEREnvVelocitySens { get; set; }
        public byte FILTERResonance { get; set; }
        public byte FILTEREnvAttackTime { get; set; }
        public byte FILTEREnvDecayTime { get; set; }
        public byte FILTEREnvSustainLevel { get; set; }
        public byte FILTEREnvReleaseTime { get; set; }
        public byte FILTEREnvDepth { get; set; }
        public byte AMPLevel { get; set; }
        public byte AMPLevelVelocitySens { get; set; }
        public byte AMPEnvAttackTime { get; set; }
        public byte AMPEnvDecayTime { get; set; }
        public byte AMPEnvSustainLevel { get; set; }
        public byte AMPEnvReleaseTime { get; set; }
        public byte AMPPan { get; set; }
        public byte LFOShape { get; set; }
        public byte LFORate { get; set; }
        public Boolean LFOTempoSyncSwitch { get; set; }
        public byte LFOTempoSyncNote { get; set; }
        public byte LFOFadeTime { get; set; }
        public Boolean LFOKeyTrigger { get; set; }
        public byte LFOPitchDepth { get; set; }
        public byte LFOFilterDepth { get; set; }
        public byte LFOAmpDepth { get; set; }
        public byte LFOPanDepth { get; set; }
        public byte ModulationLFOShape { get; set; }
        public byte ModulationLFORate { get; set; }
        public Boolean ModulationLFOTempoSyncSwitch { get; set; }
        public byte ModulationLFOTempoSyncNote { get; set; }
        public byte OSCPulseWidthShift { get; set; }
        public byte ModulationLFOPitchDepth { get; set; }
        public byte ModulationLFOFilterDepth { get; set; }
        public byte ModulationLFOAmpDepth { get; set; }
        public byte ModulationLFOPanDepth { get; set; }
        public byte CutoffAftertouchSens { get; set; }
        public byte LevelAftertouchSens { get; set; }
        public byte WaveGain { get; set; }
        public UInt16 WaveNumber { get; set; }
        public byte HPFCutoff { get; set; }
        public byte SuperSawDetune { get; set; }
        public byte ModulationLFORateControl { get; set; }
        public byte AMPLevelKeyfollow { get; set; }

        public SuperNATURALSynthTonePartial(ReceivedData Data)
        {
            t.Trace("public SuperNATURALSynthTonePartial (" + "ReceivedData" + Data + ", " + ")");
            OSCWave = Data.GetByte(0x00);
            OSCWaveVariation = Data.GetByte(0x01);
            OSCPitch = Data.GetByte(0x03);
            OSCDetune = Data.GetByte(0x04);
            OSCPulseWidthModDepth = Data.GetByte(0x05);
            OSCPulseWidth = Data.GetByte(0x06);
            OSCPitchEnvAttackTime = Data.GetByte(0x07);
            OSCPitchEnvDecay = Data.GetByte(0x08);
            OSCPitchEnvDepth = Data.GetByte(0x09);
            FILTERMode = Data.GetByte(0x0a);
            FILTERSlope = Data.GetByte(0x0b);
            FILTERCutoff = Data.GetByte(0x0c);
            FILTERCutoffKeyfollow = Data.GetByte(0x0d);
            FILTEREnvVelocitySens = Data.GetByte(0x0e);
            FILTERResonance = Data.GetByte(0x0f);
            FILTEREnvAttackTime = Data.GetByte(0x10);
            FILTEREnvDecayTime = Data.GetByte(0x11);
            FILTEREnvSustainLevel = Data.GetByte(0x12);
            FILTEREnvReleaseTime = Data.GetByte(0x13);
            FILTEREnvDepth = Data.GetByte(0x14);
            AMPLevel = Data.GetByte(0x15);
            AMPLevelVelocitySens = Data.GetByte(0x16);
            AMPEnvAttackTime = Data.GetByte(0x17);
            AMPEnvDecayTime = Data.GetByte(0x18);
            AMPEnvSustainLevel = Data.GetByte(0x19);
            AMPEnvReleaseTime = Data.GetByte(0x1a);
            AMPPan = Data.GetByte(0x1b);
            LFOShape = Data.GetByte(0x1c);
            LFORate = Data.GetByte(0x1d);
            LFOTempoSyncSwitch = Data.GetByte(0x1e) > 0;
            LFOTempoSyncNote = Data.GetByte(0x1f);
            LFOFadeTime = Data.GetByte(0x20);
            LFOKeyTrigger = Data.GetByte(0x21) > 0;
            LFOPitchDepth = Data.GetByte(0x22);
            LFOFilterDepth = Data.GetByte(0x23);
            LFOAmpDepth = Data.GetByte(0x24);
            LFOPanDepth = Data.GetByte(0x25);
            ModulationLFOShape = Data.GetByte(0x26);
            ModulationLFORate = Data.GetByte(0x27);
            ModulationLFOTempoSyncSwitch = Data.GetByte(0x28) > 0;
            ModulationLFOTempoSyncNote = Data.GetByte(0x29);
            OSCPulseWidthShift = Data.GetByte(0x2a);
            ModulationLFOPitchDepth = Data.GetByte(0x2c);
            ModulationLFOFilterDepth = Data.GetByte(0x2d);
            ModulationLFOAmpDepth = Data.GetByte(0x2e);
            ModulationLFOPanDepth = Data.GetByte(0x2f);
            CutoffAftertouchSens = Data.GetByte(0x30);
            LevelAftertouchSens = Data.GetByte(0x31);
            WaveGain = Data.GetByte(0x34);
            WaveNumber = Data.Get4Byte(0x35);
            HPFCutoff = Data.GetByte(0x39);
            SuperSawDetune = Data.GetByte(0x3a);
            ModulationLFORateControl = Data.GetByte(0x3b);
            AMPLevelKeyfollow = Data.GetByte(0x3c);
        }
    }

    class SuperNATURALSynthToneMisc
    {
        public byte AttackTimeIntervalSens { get; set; }
        public byte ReleaseTimeIntervalSens { get; set; }
        public byte PortamentoTimeIntervalSens { get; set; }
        public byte EnvelopeLoopMode { get; set; }
        public byte EnvelopeLoopSyncNote { get; set; }
        public Boolean ChromaticPortamento { get; set; }

        public SuperNATURALSynthToneMisc(ReceivedData Data)
        {
            AttackTimeIntervalSens = Data.GetByte(0x01);
            ReleaseTimeIntervalSens = Data.GetByte(0x02);
            PortamentoTimeIntervalSens = Data.GetByte(0x03);
            EnvelopeLoopMode = Data.GetByte(0x04);
            EnvelopeLoopSyncNote = Data.GetByte(0x05);
            ChromaticPortamento = Data.GetByte(0x06) > 0;
        }
    }

    class SuperNATURALAcousticToneCommon
    {
        HBTrace t = new HBTrace("class SuperNATURALAcousticToneCommon");
        public String Name { get; set; }
        public byte ToneLevel { get; set; }
        public byte MonoPoly { get; set; }
        public byte PortamentoTimeOffset { get; set; }
        public byte CutoffOffset { get; set; }
        public byte ResonanceOffset { get; set; }
        public byte AttackTimeOffset { get; set; }
        public byte ReleaseTimeOffset { get; set; }
        public byte VibratoRate { get; set; }
        public byte VibratoDepth { get; set; }
        public byte VibratoDelay { get; set; }
        public byte OctaveShift { get; set; }
        public byte Category { get; set; }
        public byte PhraseNumber { get; set; }
        public byte PhraseOctaveShift { get; set; }
        public Boolean TFXSwitch { get; set; }
        public byte InstVariation { get; set; }
        public byte InstNumber { get; set; }
        public byte ModifyParameter1 { get; set; }
        public byte ModifyParameter2 { get; set; }
        public byte ModifyParameter3 { get; set; }
        public byte ModifyParameter4 { get; set; }
        public byte ModifyParameter5 { get; set; }
        public byte ModifyParameter6 { get; set; }
        public byte ModifyParameter7 { get; set; }
        public byte ModifyParameter8 { get; set; }
        public byte ModifyParameter9 { get; set; }
        public byte ModifyParameter10 { get; set; }
        public byte ModifyParameter11 { get; set; }
        public byte ModifyParameter12 { get; set; }
        public byte ModifyParameter13 { get; set; }
        public byte ModifyParameter14 { get; set; }
        public byte ModifyParameter15 { get; set; }
        public byte ModifyParameter16 { get; set; }
        public byte ModifyParameter17 { get; set; }
        public byte ModifyParameter18 { get; set; }
        public byte ModifyParameter19 { get; set; }
        public byte ModifyParameter20 { get; set; }
        public byte ModifyParameter21 { get; set; }
        public byte ModifyParameter22 { get; set; }
        public byte ModifyParameter23 { get; set; }
        public byte ModifyParameter24 { get; set; }
        public byte ModifyParameter25 { get; set; }
        public byte ModifyParameter26 { get; set; }
        public byte ModifyParameter27 { get; set; }
        public byte ModifyParameter28 { get; set; }
        public byte ModifyParameter29 { get; set; }
        public byte ModifyParameter30 { get; set; }
        public byte ModifyParameter31 { get; set; }
        public byte ModifyParameter32 { get; set; }

        public SuperNATURALAcousticToneCommon(ReceivedData Data)
        {
            t.Trace("public SuperNATURALAcousticToneCommon (" + "ReceivedData" + Data + ", " + ")");
            Name = "";
            for (byte i = 0; i < 0x0c; i++)
            {
                Name += (char)Data.GetByte(i);
            }
            ToneLevel = Data.GetByte(0x10);
            MonoPoly = Data.GetByte(0x11);
            PortamentoTimeOffset = Data.GetByte(0x12);
            CutoffOffset = Data.GetByte(0x13);
            ResonanceOffset = Data.GetByte(0x14);
            AttackTimeOffset = Data.GetByte(0x15);
            ReleaseTimeOffset = Data.GetByte(0x16);
            VibratoRate = Data.GetByte(0x17);
            VibratoDepth = Data.GetByte(0x18);
            VibratoDelay = Data.GetByte(0x19);
            OctaveShift = Data.GetByte(0x1a);
            Category = Data.GetByte(0x1b);
            PhraseNumber = Data.Get2Byte(0x1c);
            PhraseOctaveShift = Data.GetByte(0x1e);
            TFXSwitch = Data.GetByte(0x1f) > 0;
            InstVariation = Data.GetByte(0x20);
            InstNumber = Data.GetByte(0x21);
            ModifyParameter1 = Data.GetByte(0x22);
            ModifyParameter2 = Data.GetByte(0x23);
            ModifyParameter3 = Data.GetByte(0x24);
            ModifyParameter4 = Data.GetByte(0x25);
            ModifyParameter5 = Data.GetByte(0x26);
            ModifyParameter6 = Data.GetByte(0x27);
            ModifyParameter7 = Data.GetByte(0x28);
            ModifyParameter8 = Data.GetByte(0x29);
            ModifyParameter9 = Data.GetByte(0x2a);
            ModifyParameter10 = Data.GetByte(0x2b);
            ModifyParameter11 = Data.GetByte(0x2c);
            ModifyParameter12 = Data.GetByte(0x2d);
            ModifyParameter13 = Data.GetByte(0x2e);
            ModifyParameter14 = Data.GetByte(0x2f);
            ModifyParameter15 = Data.GetByte(0x30);
            ModifyParameter16 = Data.GetByte(0x31);
            ModifyParameter17 = Data.GetByte(0x32);
            ModifyParameter18 = Data.GetByte(0x33);
            ModifyParameter19 = Data.GetByte(0x34);
            ModifyParameter20 = Data.GetByte(0x35);
            ModifyParameter21 = Data.GetByte(0x36);
            ModifyParameter22 = Data.GetByte(0x37);
            ModifyParameter23 = Data.GetByte(0x38);
            ModifyParameter24 = Data.GetByte(0x39);
            ModifyParameter25 = Data.GetByte(0x3a);
            ModifyParameter26 = Data.GetByte(0x3b);
            ModifyParameter27 = Data.GetByte(0x3c);
            ModifyParameter28 = Data.GetByte(0x3d);
            ModifyParameter29 = Data.GetByte(0x3e);
            ModifyParameter30 = Data.GetByte(0x3f);
            ModifyParameter31 = Data.GetByte(0x40);
            ModifyParameter32 = Data.GetByte(0x41);
        }
    }

    class SuperNATURALDrumKitCommon
    {
        HBTrace t = new HBTrace("class SuperNATURALDrumKitCommon");
        public String Name { get; set; }
        public byte KitLevel { get; set; }
        public byte AmbienceLevel { get; set; }
        public byte PhraseNumber { get; set; }
        public Boolean TFXSwitch { get; set; }

        public SuperNATURALDrumKitCommon(ReceivedData Data)
        {
            t.Trace("public SuperNATURALDrumKitCommon (" + "ReceivedData" + Data + ", " + ")");
            Name = "";
            for (byte i = 0; i < 0x0c; i++)
            {
                Name += (char)Data.GetByte(i);
            }
            KitLevel = Data.GetByte(0x10);
            AmbienceLevel = Data.GetByte(0x11);
            PhraseNumber = Data.GetByte(0x12);
            TFXSwitch = Data.GetByte(0x13) > 0;
        }
    }

    class SuperNATURALDrumKitMFX
    {
        HBTrace t = new HBTrace("class SuperNATURALDrumKitMFX");
        public byte MFXType { get; set; }
        public byte MFXChorusSendLevel { get; set; }
        public byte MFXReverbSendLevel { get; set; }
        public byte[] MFXControlSource { get; set; } // [4]
        public byte[] MFXControlSens { get; set; }   // [4]
        public byte[] MFXControlAssign { get; set; } // [4]
        public byte[] MFXParameter { get; set; } // [32] Kolla Ã¤ven dessa!
        public MFXNumberedParameters MFXNumberedParameters { get; set; }

        public SuperNATURALDrumKitMFX(ReceivedData Data)
        {
            t.Trace("public SuperNATURALDrumKitMFX (" + "ReceivedData" + Data + ", " + ")");
            MFXControlSource = new byte[4];
            MFXControlSens = new byte[4];
            MFXControlAssign = new byte[4];

            MFXType = Data.GetByte(0x00);
            MFXChorusSendLevel = Data.GetByte(0x02);
            MFXReverbSendLevel = Data.GetByte(0x03);
            for (byte i = 0; i < 4; i++)
            {
                MFXControlSource[i] = Data.GetByte(0x05 + 2 * i);
                MFXControlSens[i] = Data.GetByte(0x06 + 2 * i);
                MFXControlAssign[i] = Data.GetByte(0x0d + i);
            }
            MFXNumberedParameters = new MFXNumberedParameters(Data, 0x11);
        }
    }

    class SuperNATURALDrumKitCommonCompEQ
    {
        HBTrace t = new HBTrace("class SuperNATURALDrumKitCommonCompEQ");
        public CompEq[] CompEQ; // [6]

        public SuperNATURALDrumKitCommonCompEQ(ReceivedData Data)
        {
            t.Trace("public SuperNATURALDrumKitCommonCompEQ (" + "ReceivedData" + Data + ", " + ")");
            CompEQ = new CompEq[6];
            for (byte i = 0; i < 6; i++)
            {
                CompEQ[i] = new CompEq();
                CompEQ[i].SetContent(i, Data);
            }
        }
    }

    class SuperNATURALDrumKitKey
    {
        HBTrace t = new HBTrace("class SuperNATURALDrumKitKey");
        public byte BankNumber { get; set; } // This is 0 for Internal and 1 for ExSN6. Read more in MakeDynamicControls.cs AddSupernaturalDrumKitDruminstrumentControls()
        public byte[] InstNumber { get; set; } // [2] 0 = iternal sound, 1 = ExSN6 sound
        public byte Level { get; set; }
        public byte Pan { get; set; }
        public byte ChorusSendLevel { get; set; }
        public byte ReverbSendLevel { get; set; }
        public byte Tune { get; set; }
        public byte Attack { get; set; }
        public byte Decay { get; set; }
        public byte Brilliance { get; set; }
        public byte Variation { get; set; }
        public byte DynamicRange { get; set; }
        public byte StereoWidth { get; set; }
        public byte OutputAssign { get; set; }

        public SuperNATURALDrumKitKey(ReceivedData Data)
        {
            t.Trace("public SuperNATURALDrumKitNote (" + "ReceivedData" + Data + ", " + ")");
            InstNumber = new byte[2];
            UInt16 temp = Data.Get3Of4Byte(0);
            if (temp > 168)
            {
                BankNumber = 0x01; // This is 0 for Internal and 1 for ExSN6. Read more in MakeDynamicControls.cs AddSupernaturalDrumKitDruminstrumentControls()
                InstNumber[0] = 0; // Because we do not know yet
                InstNumber[1] = (byte)(temp - 169);
            }
            else
            {
                BankNumber = 0x00; // Because we do not know yet
                InstNumber[0] = (byte)temp;
                InstNumber[1] = 0;
            }
            Level = Data.GetByte(4);
            Pan = Data.GetByte(5);
            ChorusSendLevel = Data.GetByte(6);
            ReverbSendLevel = Data.GetByte(7);
            Tune = Data.Get2Of4Byte(0x08);
            Attack = Data.GetByte(0x0c);
            Decay = Data.GetByte(0x0d);
            Brilliance = Data.GetByte(0x0e);
            Variation = Data.GetByte(0x0f);
            DynamicRange = Data.GetByte(0x10);
            StereoWidth = Data.GetByte(0x11);
            OutputAssign = Data.GetByte(0x12);
        }
    }

    /// <summary>
    /// XAML layout classes
    /// </summary>
    class GridRow
    {
        HBTrace t = new HBTrace("class GridRow");
        public Grid Row { get; set; }
        public Grid[] Columns { get; set; }

        public GridRow(byte row, Control[] controls = null, byte[] columnWiths = null)
        {
            t.Trace("public GridRow ()");
            try
            {
                Row = new Grid();
                Grid.SetRow(Row, row);
                Row.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                Row.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Stretch);
                ColumnDefinition[] columnDefinitions = new ColumnDefinition[controls.Length];

                if (controls != null)
                {
                    Columns = new Grid[controls.Length];
                    for (byte i = 0; i < controls.Length; i++)
                    {
                        columnDefinitions[i] = new ColumnDefinition();
                        if (columnWiths == null || columnWiths.Length < i - 1)
                        {
                            columnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
                        }
                        else
                        {
                            columnDefinitions[i].Width = new GridLength(columnWiths[i], GridUnitType.Star);
                        }
                        Row.ColumnDefinitions.Add(columnDefinitions[i]);
                        controls[i].SetValue(Control.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                        controls[i].SetValue(Control.VerticalAlignmentProperty, VerticalAlignment.Stretch);
                        if (i == 0)
                        {
                            controls[i].SetValue(Control.MarginProperty, new Thickness(2));
                        }
                        else
                        {
                            controls[i].SetValue(Control.MarginProperty, new Thickness(0, 2, 2, 2));
                        }
                        Columns[i] = new Grid();
                        Columns[i].HorizontalAlignment = HorizontalAlignment.Stretch;
                        Columns[i].SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                        Columns[i].SetValue(Control.VerticalAlignmentProperty, VerticalAlignment.Stretch);
                        Columns[i].SetValue(Grid.ColumnProperty, i);
                        try
                        {
                            Columns[i].Children.Add(controls[i]);
                        }
                        catch (Exception e)
                        {
                            t.Trace("Catch in public GridRow (Columns[i].Children.Add(controls[i]);): " + e.Message);
                            GC.Collect(10, GCCollectionMode.Forced);
                            Columns[i].Children.Add(controls[i]);
                        }
                        try
                        {
                            Row.Children.Add(Columns[i]);
                        }
                        catch (Exception e)
                        {
                            t.Trace("Catch in public GridRow (Row.Children.Add(Columns[i]);): " + e.Message);
                            GC.Collect(10, GCCollectionMode.Forced);
                            Row.Children.Add(Columns[i]);
                        }
                    }
                }
                t.Trace("Exit public GridRow ()");
            }
            catch (Exception e)
            {
                t.Trace("Catch in public GridRow: " + e.Message);
            }
        }
    }

    public class Hex2Midi
    {
        /// <summary>
        ///  In MIDI msb is not allowed for data, and addresses are sent as data.
        ///  This function helps adding two addresses with arbitrary number of bytes
        ///  taking into consideration that the values may only be 0 - 0x7f (0 - 127).
        ///  However, max number of bytes are 4, and the second argument must contain 
        ///  the same byte-count as the first argument.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="addition"></param>
        /// <returns></returns>
        public byte[] AddBytes128(byte[] arg1, byte[] arg2)
        {
            if (arg1.Length < arg2.Length)
            {
                return null;
            }
            if (arg1.Length > arg2.Length)
            {
                byte diff = (byte)(arg1.Length - arg2.Length);
                byte[] b = new byte[arg1.Length];
                for (byte i = diff; i < (byte)arg1.Length; i++)
                {
                    b[i] = arg2[i - diff];
                    //if (!(arg2.Length < 4 - i))
                    //{
                    //    b[i] = arg2[arg2.Length - i];
                    //}
                }
                arg2 = b;
                //arg2 = (byte[])b.Concat(arg2.AsEnumerable());
            }
            byte[] result = new byte[arg1.Length];
            UInt16[] temp = new UInt16[arg1.Length];
            for (byte i = 0; i < arg1.Length; i++)
            {
                temp[i] = (UInt16)(arg1[i] + arg2[i]);
            }

            for (byte i = (byte)(temp.Length - 1); i > 0; i--)
            {
                if (temp[i] > 127)
                {
                    if (i > 0)
                    {
                        temp[i - 1] += (UInt16)(temp[i] / 128);
                    }
                    temp[i] = (UInt16)(temp[i] % 128);
                }
            }

            for (byte i = 0; i < arg1.Length; i++)
            {
                result[i] = (byte)(temp[i]);
            }
            return result;
        }
    }

    class NumberedParameterValues
    {
        HBTrace t = new HBTrace("class NumberedParameterValues");
        public UInt16[][] Parameters { get; set; }

        public NumberedParameterValues()
        {
            t.Trace("public NumberedParameterValues()");
            Parameters = new UInt16[68][];
            for (byte i = 0; i < 68; i++)
            {
                Parameters[i] = new UInt16[32];
                switch (i)
                {
                    case 0:
                        Parameters[0][0] = 0;
                        Parameters[0][1] = 0;
                        Parameters[0][2] = 0;
                        Parameters[0][3] = 0;
                        Parameters[0][4] = 0;
                        Parameters[0][5] = 0;
                        Parameters[0][6] = 0;
                        Parameters[0][7] = 0;
                        Parameters[0][8] = 0;
                        Parameters[0][9] = 0;
                        Parameters[0][10] = 0;
                        Parameters[0][11] = 0;
                        Parameters[0][12] = 0;
                        Parameters[0][13] = 0;
                        Parameters[0][14] = 0;
                        Parameters[0][15] = 0;
                        Parameters[0][16] = 0;
                        Parameters[0][17] = 0;
                        Parameters[0][18] = 0;
                        Parameters[0][19] = 0;
                        Parameters[0][20] = 0;
                        Parameters[0][21] = 0;
                        Parameters[0][22] = 0;
                        Parameters[0][23] = 0;
                        Parameters[0][24] = 0;
                        Parameters[0][25] = 0;
                        Parameters[0][26] = 0;
                        Parameters[0][27] = 0;
                        Parameters[0][28] = 0;
                        Parameters[0][29] = 0;
                        Parameters[0][30] = 0;
                        Parameters[0][31] = 0;
                        break;
                    case 1:
                        Parameters[1][0] = 1;
                        Parameters[1][1] = 17;
                        Parameters[1][2] = 7;
                        Parameters[1][3] = 15;
                        Parameters[1][4] = 3;
                        Parameters[1][5] = 11;
                        Parameters[1][6] = 15;
                        Parameters[1][7] = 3;
                        Parameters[1][8] = 0;
                        Parameters[1][9] = 15;
                        Parameters[1][10] = 127;
                        Parameters[1][11] = 0;
                        Parameters[1][12] = 0;
                        Parameters[1][13] = 0;
                        Parameters[1][14] = 0;
                        Parameters[1][15] = 0;
                        Parameters[1][16] = 0;
                        Parameters[1][17] = 0;
                        Parameters[1][18] = 0;
                        Parameters[1][19] = 0;
                        Parameters[1][20] = 0;
                        Parameters[1][21] = 0;
                        Parameters[1][22] = 0;
                        Parameters[1][23] = 0;
                        Parameters[1][24] = 0;
                        Parameters[1][25] = 0;
                        Parameters[1][26] = 0;
                        Parameters[1][27] = 0;
                        Parameters[1][28] = 0;
                        Parameters[1][29] = 0;
                        Parameters[1][30] = 0;
                        Parameters[1][31] = 0;
                        break;
                    case 2:
                        Parameters[2][0] = 15;
                        Parameters[2][1] = 15;
                        Parameters[2][2] = 15;
                        Parameters[2][3] = 15;
                        Parameters[2][4] = 15;
                        Parameters[2][5] = 15;
                        Parameters[2][6] = 15;
                        Parameters[2][7] = 15;
                        Parameters[2][8] = 0;
                        Parameters[2][9] = 127;
                        Parameters[2][10] = 0;
                        Parameters[2][11] = 0;
                        Parameters[2][12] = 0;
                        Parameters[2][13] = 0;
                        Parameters[2][14] = 0;
                        Parameters[2][15] = 0;
                        Parameters[2][16] = 0;
                        Parameters[2][17] = 0;
                        Parameters[2][18] = 0;
                        Parameters[2][19] = 0;
                        Parameters[2][20] = 0;
                        Parameters[2][21] = 0;
                        Parameters[2][22] = 0;
                        Parameters[2][23] = 0;
                        Parameters[2][24] = 0;
                        Parameters[2][25] = 0;
                        Parameters[2][26] = 0;
                        Parameters[2][27] = 0;
                        Parameters[2][28] = 0;
                        Parameters[2][29] = 0;
                        Parameters[2][30] = 0;
                        Parameters[2][31] = 0;
                        break;
                    case 3:
                        Parameters[3][0] = 4;
                        Parameters[3][1] = 6;
                        Parameters[3][2] = 0;
                        Parameters[3][3] = 15;
                        Parameters[3][4] = 15;
                        Parameters[3][5] = 127;
                        Parameters[3][6] = 0;
                        Parameters[3][7] = 0;
                        Parameters[3][8] = 0;
                        Parameters[3][9] = 0;
                        Parameters[3][10] = 0;
                        Parameters[3][11] = 0;
                        Parameters[3][12] = 0;
                        Parameters[3][13] = 0;
                        Parameters[3][14] = 0;
                        Parameters[3][15] = 0;
                        Parameters[3][16] = 0;
                        Parameters[3][17] = 0;
                        Parameters[3][18] = 0;
                        Parameters[3][19] = 0;
                        Parameters[3][20] = 0;
                        Parameters[3][21] = 0;
                        Parameters[3][22] = 0;
                        Parameters[3][23] = 0;
                        Parameters[3][24] = 0;
                        Parameters[3][25] = 0;
                        Parameters[3][26] = 0;
                        Parameters[3][27] = 0;
                        Parameters[3][28] = 0;
                        Parameters[3][29] = 0;
                        Parameters[3][30] = 0;
                        Parameters[3][31] = 0;
                        break;
                    case 4:
                        Parameters[4][0] = 60;
                        Parameters[4][1] = 30;
                        Parameters[4][2] = 60;
                        Parameters[4][3] = 30;
                        Parameters[4][4] = 60;
                        Parameters[4][5] = 30;
                        Parameters[4][6] = 60;
                        Parameters[4][7] = 30;
                        Parameters[4][8] = 60;
                        Parameters[4][9] = 60;
                        Parameters[4][10] = 30;
                        Parameters[4][11] = 60;
                        Parameters[4][12] = 60;
                        Parameters[4][13] = 30;
                        Parameters[4][14] = 60;
                        Parameters[4][15] = 30;
                        Parameters[4][16] = 1;
                        Parameters[4][17] = 10;
                        Parameters[4][18] = 18;
                        Parameters[4][19] = 50;
                        Parameters[4][20] = 2;
                        Parameters[4][21] = 2;
                        Parameters[4][22] = 40;
                        Parameters[4][23] = 0;
                        Parameters[4][24] = 127;
                        Parameters[4][25] = 0;
                        Parameters[4][26] = 0;
                        Parameters[4][27] = 0;
                        Parameters[4][28] = 0;
                        Parameters[4][29] = 0;
                        Parameters[4][30] = 0;
                        Parameters[4][31] = 0;
                        break;
                    case 5:
                        Parameters[5][0] = 64;
                        Parameters[5][1] = 64;
                        Parameters[5][2] = 15;
                        Parameters[5][3] = 15;
                        Parameters[5][4] = 127;
                        Parameters[5][5] = 0;
                        Parameters[5][6] = 0;
                        Parameters[5][7] = 0;
                        Parameters[5][8] = 0;
                        Parameters[5][9] = 0;
                        Parameters[5][10] = 0;
                        Parameters[5][11] = 0;
                        Parameters[5][12] = 0;
                        Parameters[5][13] = 0;
                        Parameters[5][14] = 0;
                        Parameters[5][15] = 0;
                        Parameters[5][16] = 0;
                        Parameters[5][17] = 0;
                        Parameters[5][18] = 0;
                        Parameters[5][19] = 0;
                        Parameters[5][20] = 0;
                        Parameters[5][21] = 0;
                        Parameters[5][22] = 0;
                        Parameters[5][23] = 0;
                        Parameters[5][24] = 0;
                        Parameters[5][25] = 0;
                        Parameters[5][26] = 0;
                        Parameters[5][27] = 0;
                        Parameters[5][28] = 0;
                        Parameters[5][29] = 0;
                        Parameters[5][30] = 0;
                        Parameters[5][31] = 0;
                        break;
                    case 6:
                        Parameters[6][0] = 1;
                        Parameters[6][1] = 60;
                        Parameters[6][2] = 40;
                        Parameters[6][3] = 0;
                        Parameters[6][4] = 0;
                        Parameters[6][5] = 1;
                        Parameters[6][6] = 40;
                        Parameters[6][7] = 12;
                        Parameters[6][8] = 60;
                        Parameters[6][9] = 0;
                        Parameters[6][10] = 15;
                        Parameters[6][11] = 15;
                        Parameters[6][12] = 127;
                        Parameters[6][13] = 0;
                        Parameters[6][14] = 0;
                        Parameters[6][15] = 0;
                        Parameters[6][16] = 0;
                        Parameters[6][17] = 0;
                        Parameters[6][18] = 0;
                        Parameters[6][19] = 0;
                        Parameters[6][20] = 0;
                        Parameters[6][21] = 0;
                        Parameters[6][22] = 0;
                        Parameters[6][23] = 0;
                        Parameters[6][24] = 0;
                        Parameters[6][25] = 0;
                        Parameters[6][26] = 0;
                        Parameters[6][27] = 0;
                        Parameters[6][28] = 0;
                        Parameters[6][29] = 0;
                        Parameters[6][30] = 0;
                        Parameters[6][31] = 0;
                        break;
                    case 7:
                        Parameters[7][0] = 1;
                        Parameters[7][1] = 127;
                        Parameters[7][2] = 4;
                        Parameters[7][3] = 0;
                        Parameters[7][4] = 1;
                        Parameters[7][5] = 10;
                        Parameters[7][6] = 18;
                        Parameters[7][7] = 127;
                        Parameters[7][8] = 0;
                        Parameters[7][9] = 60;
                        Parameters[7][10] = 50;
                        Parameters[7][11] = 15;
                        Parameters[7][12] = 15;
                        Parameters[7][13] = 64;
                        Parameters[7][14] = 127;
                        Parameters[7][15] = 0;
                        Parameters[7][16] = 0;
                        Parameters[7][17] = 0;
                        Parameters[7][18] = 0;
                        Parameters[7][19] = 0;
                        Parameters[7][20] = 0;
                        Parameters[7][21] = 0;
                        Parameters[7][22] = 0;
                        Parameters[7][23] = 0;
                        Parameters[7][24] = 0;
                        Parameters[7][25] = 0;
                        Parameters[7][26] = 0;
                        Parameters[7][27] = 0;
                        Parameters[7][28] = 0;
                        Parameters[7][29] = 0;
                        Parameters[7][30] = 0;
                        Parameters[7][31] = 0;
                        break;
                    case 8:
                        Parameters[8][0] = 6;
                        Parameters[8][1] = 1;
                        Parameters[8][2] = 127;
                        Parameters[8][3] = 0;
                        Parameters[8][4] = 127;
                        Parameters[8][5] = 0;
                        Parameters[8][6] = 0;
                        Parameters[8][7] = 0;
                        Parameters[8][8] = 0;
                        Parameters[8][9] = 0;
                        Parameters[8][10] = 0;
                        Parameters[8][11] = 0;
                        Parameters[8][12] = 0;
                        Parameters[8][13] = 0;
                        Parameters[8][14] = 0;
                        Parameters[8][15] = 0;
                        Parameters[8][16] = 0;
                        Parameters[8][17] = 0;
                        Parameters[8][18] = 0;
                        Parameters[8][19] = 0;
                        Parameters[8][20] = 0;
                        Parameters[8][21] = 0;
                        Parameters[8][22] = 0;
                        Parameters[8][23] = 0;
                        Parameters[8][24] = 0;
                        Parameters[8][25] = 0;
                        Parameters[8][26] = 0;
                        Parameters[8][27] = 0;
                        Parameters[8][28] = 0;
                        Parameters[8][29] = 0;
                        Parameters[8][30] = 0;
                        Parameters[8][31] = 0;
                        break;
                    case 9:
                        Parameters[9][0] = 2;
                        Parameters[9][1] = 64;
                        Parameters[9][2] = 1;
                        Parameters[9][3] = 10;
                        Parameters[9][4] = 18;
                        Parameters[9][5] = 40;
                        Parameters[9][6] = 1;
                        Parameters[9][7] = 40;
                        Parameters[9][8] = 49;
                        Parameters[9][9] = 127;
                        Parameters[9][10] = 15;
                        Parameters[9][11] = 15;
                        Parameters[9][12] = 127;
                        Parameters[9][13] = 0;
                        Parameters[9][14] = 0;
                        Parameters[9][15] = 0;
                        Parameters[9][16] = 0;
                        Parameters[9][17] = 0;
                        Parameters[9][18] = 0;
                        Parameters[9][19] = 0;
                        Parameters[9][20] = 0;
                        Parameters[9][21] = 0;
                        Parameters[9][22] = 0;
                        Parameters[9][23] = 0;
                        Parameters[9][24] = 0;
                        Parameters[9][25] = 0;
                        Parameters[9][26] = 0;
                        Parameters[9][27] = 0;
                        Parameters[9][28] = 0;
                        Parameters[9][29] = 0;
                        Parameters[9][30] = 0;
                        Parameters[9][31] = 0;
                        break;
                    case 10:
                        Parameters[10][0] = 50;
                        Parameters[10][1] = 0;
                        Parameters[10][2] = 15;
                        Parameters[10][3] = 15;
                        Parameters[10][4] = 127;
                        Parameters[10][5] = 0;
                        Parameters[10][6] = 0;
                        Parameters[10][7] = 0;
                        Parameters[10][8] = 0;
                        Parameters[10][9] = 0;
                        Parameters[10][10] = 0;
                        Parameters[10][11] = 0;
                        Parameters[10][12] = 0;
                        Parameters[10][13] = 0;
                        Parameters[10][14] = 0;
                        Parameters[10][15] = 0;
                        Parameters[10][16] = 0;
                        Parameters[10][17] = 0;
                        Parameters[10][18] = 0;
                        Parameters[10][19] = 0;
                        Parameters[10][20] = 0;
                        Parameters[10][21] = 0;
                        Parameters[10][22] = 0;
                        Parameters[10][23] = 0;
                        Parameters[10][24] = 0;
                        Parameters[10][25] = 0;
                        Parameters[10][26] = 0;
                        Parameters[10][27] = 0;
                        Parameters[10][28] = 0;
                        Parameters[10][29] = 0;
                        Parameters[10][30] = 0;
                        Parameters[10][31] = 0;
                        break;
                    case 11:
                        Parameters[11][0] = 50;
                        Parameters[11][1] = 15;
                        Parameters[11][2] = 15;
                        Parameters[11][3] = 127;
                        Parameters[11][4] = 0;
                        Parameters[11][5] = 0;
                        Parameters[11][6] = 0;
                        Parameters[11][7] = 0;
                        Parameters[11][8] = 0;
                        Parameters[11][9] = 0;
                        Parameters[11][10] = 0;
                        Parameters[11][11] = 0;
                        Parameters[11][12] = 0;
                        Parameters[11][13] = 0;
                        Parameters[11][14] = 0;
                        Parameters[11][15] = 0;
                        Parameters[11][16] = 0;
                        Parameters[11][17] = 0;
                        Parameters[11][18] = 0;
                        Parameters[11][19] = 0;
                        Parameters[11][20] = 0;
                        Parameters[11][21] = 0;
                        Parameters[11][22] = 0;
                        Parameters[11][23] = 0;
                        Parameters[11][24] = 0;
                        Parameters[11][25] = 0;
                        Parameters[11][26] = 0;
                        Parameters[11][27] = 0;
                        Parameters[11][28] = 0;
                        Parameters[11][29] = 0;
                        Parameters[11][30] = 0;
                        Parameters[11][31] = 0;
                        break;
                    case 12:
                        Parameters[12][0] = 2;
                        Parameters[12][1] = 64;
                        Parameters[12][2] = 1;
                        Parameters[12][3] = 30;
                        Parameters[12][4] = 13;
                        Parameters[12][5] = 40;
                        Parameters[12][6] = 1;
                        Parameters[12][7] = 40;
                        Parameters[12][8] = 49;
                        Parameters[12][9] = 1;
                        Parameters[12][10] = 80;
                        Parameters[12][11] = 6;
                        Parameters[12][12] = 127;
                        Parameters[12][13] = 15;
                        Parameters[12][14] = 15;
                        Parameters[12][15] = 127;
                        Parameters[12][16] = 0;
                        Parameters[12][17] = 0;
                        Parameters[12][18] = 0;
                        Parameters[12][19] = 0;
                        Parameters[12][20] = 0;
                        Parameters[12][21] = 0;
                        Parameters[12][22] = 0;
                        Parameters[12][23] = 0;
                        Parameters[12][24] = 0;
                        Parameters[12][25] = 0;
                        Parameters[12][26] = 0;
                        Parameters[12][27] = 0;
                        Parameters[12][28] = 0;
                        Parameters[12][29] = 0;
                        Parameters[12][30] = 0;
                        Parameters[12][31] = 0;
                        break;
                    case 13:
                        Parameters[13][0] = 5;
                        Parameters[13][1] = 60;
                        Parameters[13][2] = 1;
                        Parameters[13][3] = 10;
                        Parameters[13][4] = 18;
                        Parameters[13][5] = 40;
                        Parameters[13][6] = 40;
                        Parameters[13][7] = 127;
                        Parameters[13][8] = 64;
                        Parameters[13][9] = 15;
                        Parameters[13][10] = 15;
                        Parameters[13][11] = 127;
                        Parameters[13][12] = 0;
                        Parameters[13][13] = 0;
                        Parameters[13][14] = 0;
                        Parameters[13][15] = 0;
                        Parameters[13][16] = 0;
                        Parameters[13][17] = 0;
                        Parameters[13][18] = 0;
                        Parameters[13][19] = 0;
                        Parameters[13][20] = 0;
                        Parameters[13][21] = 0;
                        Parameters[13][22] = 0;
                        Parameters[13][23] = 0;
                        Parameters[13][24] = 0;
                        Parameters[13][25] = 0;
                        Parameters[13][26] = 0;
                        Parameters[13][27] = 0;
                        Parameters[13][28] = 0;
                        Parameters[13][29] = 0;
                        Parameters[13][30] = 0;
                        Parameters[13][31] = 0;
                        break;
                    case 14:
                        Parameters[14][0] = 3;
                        Parameters[14][1] = 140;
                        Parameters[14][2] = 80;
                        Parameters[14][3] = 127;
                        Parameters[14][4] = 64;
                        Parameters[14][5] = 15;
                        Parameters[14][6] = 15;
                        Parameters[14][7] = 127;
                        Parameters[14][8] = 0;
                        Parameters[14][9] = 0;
                        Parameters[14][10] = 0;
                        Parameters[14][11] = 0;
                        Parameters[14][12] = 0;
                        Parameters[14][13] = 0;
                        Parameters[14][14] = 0;
                        Parameters[14][15] = 0;
                        Parameters[14][16] = 0;
                        Parameters[14][17] = 0;
                        Parameters[14][18] = 0;
                        Parameters[14][19] = 0;
                        Parameters[14][20] = 0;
                        Parameters[14][21] = 0;
                        Parameters[14][22] = 0;
                        Parameters[14][23] = 0;
                        Parameters[14][24] = 0;
                        Parameters[14][25] = 0;
                        Parameters[14][26] = 0;
                        Parameters[14][27] = 0;
                        Parameters[14][28] = 0;
                        Parameters[14][29] = 0;
                        Parameters[14][30] = 0;
                        Parameters[14][31] = 0;
                        break;
                    case 15:
                        Parameters[15][0] = 60;
                        Parameters[15][1] = 0;
                        Parameters[15][2] = 0;
                        Parameters[15][3] = 15;
                        Parameters[15][4] = 15;
                        Parameters[15][5] = 50;
                        Parameters[15][6] = 127;
                        Parameters[15][7] = 0;
                        Parameters[15][8] = 0;
                        Parameters[15][9] = 0;
                        Parameters[15][10] = 0;
                        Parameters[15][11] = 0;
                        Parameters[15][12] = 0;
                        Parameters[15][13] = 0;
                        Parameters[15][14] = 0;
                        Parameters[15][15] = 0;
                        Parameters[15][16] = 0;
                        Parameters[15][17] = 0;
                        Parameters[15][18] = 0;
                        Parameters[15][19] = 0;
                        Parameters[15][20] = 0;
                        Parameters[15][21] = 0;
                        Parameters[15][22] = 0;
                        Parameters[15][23] = 0;
                        Parameters[15][24] = 0;
                        Parameters[15][25] = 0;
                        Parameters[15][26] = 0;
                        Parameters[15][27] = 0;
                        Parameters[15][28] = 0;
                        Parameters[15][29] = 0;
                        Parameters[15][30] = 0;
                        Parameters[15][31] = 0;
                        break;
                    case 16:
                        Parameters[16][0] = 0;
                        Parameters[16][1] = 1;
                        Parameters[16][2] = 80;
                        Parameters[16][3] = 9;
                        Parameters[16][4] = 96;
                        Parameters[16][5] = 15;
                        Parameters[16][6] = 15;
                        Parameters[16][7] = 127;
                        Parameters[16][8] = 0;
                        Parameters[16][9] = 0;
                        Parameters[16][10] = 0;
                        Parameters[16][11] = 0;
                        Parameters[16][12] = 0;
                        Parameters[16][13] = 0;
                        Parameters[16][14] = 0;
                        Parameters[16][15] = 0;
                        Parameters[16][16] = 0;
                        Parameters[16][17] = 0;
                        Parameters[16][18] = 0;
                        Parameters[16][19] = 0;
                        Parameters[16][20] = 0;
                        Parameters[16][21] = 0;
                        Parameters[16][22] = 0;
                        Parameters[16][23] = 0;
                        Parameters[16][24] = 0;
                        Parameters[16][25] = 0;
                        Parameters[16][26] = 0;
                        Parameters[16][27] = 0;
                        Parameters[16][28] = 0;
                        Parameters[16][29] = 0;
                        Parameters[16][30] = 0;
                        Parameters[16][31] = 0;
                        break;
                    case 17:
                        Parameters[17][0] = 0;
                        Parameters[17][1] = 1;
                        Parameters[17][2] = 80;
                        Parameters[17][3] = 9;
                        Parameters[17][4] = 96;
                        Parameters[17][5] = 15;
                        Parameters[17][6] = 15;
                        Parameters[17][7] = 127;
                        Parameters[17][8] = 0;
                        Parameters[17][9] = 0;
                        Parameters[17][10] = 0;
                        Parameters[17][11] = 0;
                        Parameters[17][12] = 0;
                        Parameters[17][13] = 0;
                        Parameters[17][14] = 0;
                        Parameters[17][15] = 0;
                        Parameters[17][16] = 0;
                        Parameters[17][17] = 0;
                        Parameters[17][18] = 0;
                        Parameters[17][19] = 0;
                        Parameters[17][20] = 0;
                        Parameters[17][21] = 0;
                        Parameters[17][22] = 0;
                        Parameters[17][23] = 0;
                        Parameters[17][24] = 0;
                        Parameters[17][25] = 0;
                        Parameters[17][26] = 0;
                        Parameters[17][27] = 0;
                        Parameters[17][28] = 0;
                        Parameters[17][29] = 0;
                        Parameters[17][30] = 0;
                        Parameters[17][31] = 0;
                        break;
                    case 18:
                        Parameters[18][0] = 127;
                        Parameters[18][1] = 0;
                        Parameters[18][2] = 30;
                        Parameters[18][3] = 127;
                        Parameters[18][4] = 127;
                        Parameters[18][5] = 0;
                        Parameters[18][6] = 30;
                        Parameters[18][7] = 0;
                        Parameters[18][8] = 127;
                        Parameters[18][9] = 0;
                        Parameters[18][10] = 30;
                        Parameters[18][11] = 127;
                        Parameters[18][12] = 0;
                        Parameters[18][13] = 0;
                        Parameters[18][14] = 30;
                        Parameters[18][15] = 0;
                        Parameters[18][16] = 1;
                        Parameters[18][17] = 10;
                        Parameters[18][18] = 18;
                        Parameters[18][19] = 50;
                        Parameters[18][20] = 0;
                        Parameters[18][21] = 60;
                        Parameters[18][22] = 0;
                        Parameters[18][23] = 0;
                        Parameters[18][24] = 127;
                        Parameters[18][25] = 0;
                        Parameters[18][26] = 0;
                        Parameters[18][27] = 0;
                        Parameters[18][28] = 0;
                        Parameters[18][29] = 0;
                        Parameters[18][30] = 0;
                        Parameters[18][31] = 0;
                        break;
                    case 19:
                        Parameters[19][0] = 0;
                        Parameters[19][1] = 40;
                        Parameters[19][2] = 160;
                        Parameters[19][3] = 10;
                        Parameters[19][4] = 127;
                        Parameters[19][5] = 40;
                        Parameters[19][6] = 160;
                        Parameters[19][7] = 10;
                        Parameters[19][8] = 127;
                        Parameters[19][9] = 127;
                        Parameters[19][10] = 127;
                        Parameters[19][11] = 0;
                        Parameters[19][12] = 0;
                        Parameters[19][13] = 0;
                        Parameters[19][14] = 0;
                        Parameters[19][15] = 0;
                        Parameters[19][16] = 0;
                        Parameters[19][17] = 0;
                        Parameters[19][18] = 0;
                        Parameters[19][19] = 0;
                        Parameters[19][20] = 0;
                        Parameters[19][21] = 0;
                        Parameters[19][22] = 0;
                        Parameters[19][23] = 0;
                        Parameters[19][24] = 0;
                        Parameters[19][25] = 0;
                        Parameters[19][26] = 0;
                        Parameters[19][27] = 0;
                        Parameters[19][28] = 0;
                        Parameters[19][29] = 0;
                        Parameters[19][30] = 0;
                        Parameters[19][31] = 0;
                        break;
                    case 20:
                        Parameters[20][0] = 0;
                        Parameters[20][1] = 0;
                        Parameters[20][2] = 40;
                        Parameters[20][3] = 160;
                        Parameters[20][4] = 64;
                        Parameters[20][5] = 64;
                        Parameters[20][6] = 127;
                        Parameters[20][7] = 40;
                        Parameters[20][8] = 160;
                        Parameters[20][9] = 64;
                        Parameters[20][10] = 64;
                        Parameters[20][11] = 127;
                        Parameters[20][12] = 10;
                        Parameters[20][13] = 15;
                        Parameters[20][14] = 15;
                        Parameters[20][15] = 127;
                        Parameters[20][16] = 0;
                        Parameters[20][17] = 0;
                        Parameters[20][18] = 0;
                        Parameters[20][19] = 0;
                        Parameters[20][20] = 0;
                        Parameters[20][21] = 0;
                        Parameters[20][22] = 0;
                        Parameters[20][23] = 0;
                        Parameters[20][24] = 0;
                        Parameters[20][25] = 0;
                        Parameters[20][26] = 0;
                        Parameters[20][27] = 0;
                        Parameters[20][28] = 0;
                        Parameters[20][29] = 0;
                        Parameters[20][30] = 0;
                        Parameters[20][31] = 0;
                        break;
                    case 21:
                        Parameters[21][0] = 0;
                        Parameters[21][1] = 0;
                        Parameters[21][2] = 40;
                        Parameters[21][3] = 160;
                        Parameters[21][4] = 64;
                        Parameters[21][5] = 64;
                        Parameters[21][6] = 127;
                        Parameters[21][7] = 40;
                        Parameters[21][8] = 160;
                        Parameters[21][9] = 64;
                        Parameters[21][10] = 64;
                        Parameters[21][11] = 127;
                        Parameters[21][12] = 10;
                        Parameters[21][13] = 15;
                        Parameters[21][14] = 15;
                        Parameters[21][15] = 127;
                        Parameters[21][16] = 1;
                        Parameters[21][17] = 80;
                        Parameters[21][18] = 100;
                        Parameters[21][19] = 127;
                        Parameters[21][20] = 0;
                        Parameters[21][21] = 0;
                        Parameters[21][22] = 0;
                        Parameters[21][23] = 0;
                        Parameters[21][24] = 0;
                        Parameters[21][25] = 0;
                        Parameters[21][26] = 0;
                        Parameters[21][27] = 0;
                        Parameters[21][28] = 0;
                        Parameters[21][29] = 0;
                        Parameters[21][30] = 0;
                        Parameters[21][31] = 0;
                        break;
                    case 22:
                        Parameters[22][0] = 2;
                        Parameters[22][1] = 6;
                        Parameters[22][2] = 20;
                        Parameters[22][3] = 0;
                        Parameters[22][4] = 10;
                        Parameters[22][5] = 18;
                        Parameters[22][6] = 30;
                        Parameters[22][7] = 90;
                        Parameters[22][8] = 15;
                        Parameters[22][9] = 15;
                        Parameters[22][10] = 50;
                        Parameters[22][11] = 127;
                        Parameters[22][12] = 0;
                        Parameters[22][13] = 0;
                        Parameters[22][14] = 0;
                        Parameters[22][15] = 0;
                        Parameters[22][16] = 0;
                        Parameters[22][17] = 0;
                        Parameters[22][18] = 0;
                        Parameters[22][19] = 0;
                        Parameters[22][20] = 0;
                        Parameters[22][21] = 0;
                        Parameters[22][22] = 0;
                        Parameters[22][23] = 0;
                        Parameters[22][24] = 0;
                        Parameters[22][25] = 0;
                        Parameters[22][26] = 0;
                        Parameters[22][27] = 0;
                        Parameters[22][28] = 0;
                        Parameters[22][29] = 0;
                        Parameters[22][30] = 0;
                        Parameters[22][31] = 0;
                        break;
                    case 23:
                        Parameters[23][0] = 2;
                        Parameters[23][1] = 6;
                        Parameters[23][2] = 20;
                        Parameters[23][3] = 1;
                        Parameters[23][4] = 10;
                        Parameters[23][5] = 18;
                        Parameters[23][6] = 40;
                        Parameters[23][7] = 90;
                        Parameters[23][8] = 79;
                        Parameters[23][9] = 15;
                        Parameters[23][10] = 15;
                        Parameters[23][11] = 50;
                        Parameters[23][12] = 127;
                        Parameters[23][13] = 0;
                        Parameters[23][14] = 0;
                        Parameters[23][15] = 0;
                        Parameters[23][16] = 0;
                        Parameters[23][17] = 0;
                        Parameters[23][18] = 0;
                        Parameters[23][19] = 0;
                        Parameters[23][20] = 0;
                        Parameters[23][21] = 0;
                        Parameters[23][22] = 0;
                        Parameters[23][23] = 0;
                        Parameters[23][24] = 0;
                        Parameters[23][25] = 0;
                        Parameters[23][26] = 0;
                        Parameters[23][27] = 0;
                        Parameters[23][28] = 0;
                        Parameters[23][29] = 0;
                        Parameters[23][30] = 0;
                        Parameters[23][31] = 0;
                        break;
                    case 24:
                        Parameters[24][0] = 2;
                        Parameters[24][1] = 6;
                        Parameters[24][2] = 20;
                        Parameters[24][3] = 1;
                        Parameters[24][4] = 30;
                        Parameters[24][5] = 13;
                        Parameters[24][6] = 40;
                        Parameters[24][7] = 90;
                        Parameters[24][8] = 79;
                        Parameters[24][9] = 1;
                        Parameters[24][10] = 80;
                        Parameters[24][11] = 6;
                        Parameters[24][12] = 15;
                        Parameters[24][13] = 15;
                        Parameters[24][14] = 50;
                        Parameters[24][15] = 127;
                        Parameters[24][16] = 0;
                        Parameters[24][17] = 0;
                        Parameters[24][18] = 0;
                        Parameters[24][19] = 0;
                        Parameters[24][20] = 0;
                        Parameters[24][21] = 0;
                        Parameters[24][22] = 0;
                        Parameters[24][23] = 0;
                        Parameters[24][24] = 0;
                        Parameters[24][25] = 0;
                        Parameters[24][26] = 0;
                        Parameters[24][27] = 0;
                        Parameters[24][28] = 0;
                        Parameters[24][29] = 0;
                        Parameters[24][30] = 0;
                        Parameters[24][31] = 0;
                        break;
                    case 25:
                        Parameters[25][0] = 20;
                        Parameters[25][1] = 0;
                        Parameters[25][2] = 10;
                        Parameters[25][3] = 18;
                        Parameters[25][4] = 30;
                        Parameters[25][5] = 0;
                        Parameters[25][6] = 20;
                        Parameters[25][7] = 20;
                        Parameters[25][8] = 50;
                        Parameters[25][9] = 127;
                        Parameters[25][10] = 0;
                        Parameters[25][11] = 0;
                        Parameters[25][12] = 0;
                        Parameters[25][13] = 0;
                        Parameters[25][14] = 0;
                        Parameters[25][15] = 0;
                        Parameters[25][16] = 0;
                        Parameters[25][17] = 0;
                        Parameters[25][18] = 0;
                        Parameters[25][19] = 0;
                        Parameters[25][20] = 0;
                        Parameters[25][21] = 0;
                        Parameters[25][22] = 0;
                        Parameters[25][23] = 0;
                        Parameters[25][24] = 0;
                        Parameters[25][25] = 0;
                        Parameters[25][26] = 0;
                        Parameters[25][27] = 0;
                        Parameters[25][28] = 0;
                        Parameters[25][29] = 0;
                        Parameters[25][30] = 0;
                        Parameters[25][31] = 0;
                        break;
                    case 26:
                        Parameters[26][0] = 20;
                        Parameters[26][1] = 0;
                        Parameters[26][2] = 10;
                        Parameters[26][3] = 18;
                        Parameters[26][4] = 50;
                        Parameters[26][5] = 0;
                        Parameters[26][6] = 40;
                        Parameters[26][7] = 12;
                        Parameters[26][8] = 127;
                        Parameters[26][9] = 90;
                        Parameters[26][10] = 50;
                        Parameters[26][11] = 127;
                        Parameters[26][12] = 0;
                        Parameters[26][13] = 0;
                        Parameters[26][14] = 0;
                        Parameters[26][15] = 0;
                        Parameters[26][16] = 0;
                        Parameters[26][17] = 0;
                        Parameters[26][18] = 0;
                        Parameters[26][19] = 0;
                        Parameters[26][20] = 0;
                        Parameters[26][21] = 0;
                        Parameters[26][22] = 0;
                        Parameters[26][23] = 0;
                        Parameters[26][24] = 0;
                        Parameters[26][25] = 0;
                        Parameters[26][26] = 0;
                        Parameters[26][27] = 0;
                        Parameters[26][28] = 0;
                        Parameters[26][29] = 0;
                        Parameters[26][30] = 0;
                        Parameters[26][31] = 0;
                        break;
                    case 27:
                        Parameters[27][0] = 20;
                        Parameters[27][1] = 0;
                        Parameters[27][2] = 10;
                        Parameters[27][3] = 18;
                        Parameters[27][4] = 30;
                        Parameters[27][5] = 90;
                        Parameters[27][6] = 15;
                        Parameters[27][7] = 15;
                        Parameters[27][8] = 50;
                        Parameters[27][9] = 127;
                        Parameters[27][10] = 0;
                        Parameters[27][11] = 0;
                        Parameters[27][12] = 0;
                        Parameters[27][13] = 0;
                        Parameters[27][14] = 0;
                        Parameters[27][15] = 0;
                        Parameters[27][16] = 0;
                        Parameters[27][17] = 0;
                        Parameters[27][18] = 0;
                        Parameters[27][19] = 0;
                        Parameters[27][20] = 0;
                        Parameters[27][21] = 0;
                        Parameters[27][22] = 0;
                        Parameters[27][23] = 0;
                        Parameters[27][24] = 0;
                        Parameters[27][25] = 0;
                        Parameters[27][26] = 0;
                        Parameters[27][27] = 0;
                        Parameters[27][28] = 0;
                        Parameters[27][29] = 0;
                        Parameters[27][30] = 0;
                        Parameters[27][31] = 0;
                        break;
                    case 28:
                        Parameters[28][0] = 127;
                        Parameters[28][1] = 50;
                        Parameters[28][2] = 1;
                        Parameters[28][3] = 0;
                        Parameters[28][4] = 15;
                        Parameters[28][5] = 15;
                        Parameters[28][6] = 64;
                        Parameters[28][7] = 127;
                        Parameters[28][8] = 0;
                        Parameters[28][9] = 0;
                        Parameters[28][10] = 0;
                        Parameters[28][11] = 0;
                        Parameters[28][12] = 0;
                        Parameters[28][13] = 0;
                        Parameters[28][14] = 0;
                        Parameters[28][15] = 0;
                        Parameters[28][16] = 0;
                        Parameters[28][17] = 0;
                        Parameters[28][18] = 0;
                        Parameters[28][19] = 0;
                        Parameters[28][20] = 0;
                        Parameters[28][21] = 0;
                        Parameters[28][22] = 0;
                        Parameters[28][23] = 0;
                        Parameters[28][24] = 0;
                        Parameters[28][25] = 0;
                        Parameters[28][26] = 0;
                        Parameters[28][27] = 0;
                        Parameters[28][28] = 0;
                        Parameters[28][29] = 0;
                        Parameters[28][30] = 0;
                        Parameters[28][31] = 0;
                        break;
                    case 29:
                        Parameters[29][0] = 127;
                        Parameters[29][1] = 50;
                        Parameters[29][2] = 1;
                        Parameters[29][3] = 3;
                        Parameters[29][4] = 15;
                        Parameters[29][5] = 15;
                        Parameters[29][6] = 64;
                        Parameters[29][7] = 127;
                        Parameters[29][8] = 0;
                        Parameters[29][9] = 0;
                        Parameters[29][10] = 0;
                        Parameters[29][11] = 0;
                        Parameters[29][12] = 0;
                        Parameters[29][13] = 0;
                        Parameters[29][14] = 0;
                        Parameters[29][15] = 0;
                        Parameters[29][16] = 0;
                        Parameters[29][17] = 0;
                        Parameters[29][18] = 0;
                        Parameters[29][19] = 0;
                        Parameters[29][20] = 0;
                        Parameters[29][21] = 0;
                        Parameters[29][22] = 0;
                        Parameters[29][23] = 0;
                        Parameters[29][24] = 0;
                        Parameters[29][25] = 0;
                        Parameters[29][26] = 0;
                        Parameters[29][27] = 0;
                        Parameters[29][28] = 0;
                        Parameters[29][29] = 0;
                        Parameters[29][30] = 0;
                        Parameters[29][31] = 0;
                        break;
                    case 30:
                        Parameters[30][0] = 1;
                        Parameters[30][1] = 1;
                        Parameters[30][2] = 80;
                        Parameters[30][3] = 100;
                        Parameters[30][4] = 1;
                        Parameters[30][5] = 64;
                        Parameters[30][6] = 64;
                        Parameters[30][7] = 64;
                        Parameters[30][8] = 0;
                        Parameters[30][9] = 0;
                        Parameters[30][10] = 1;
                        Parameters[30][11] = 4;
                        Parameters[30][12] = 1;
                        Parameters[30][13] = 127;
                        Parameters[30][14] = 0;
                        Parameters[30][15] = 64;
                        Parameters[30][16] = 127;
                        Parameters[30][17] = 0;
                        Parameters[30][18] = 0;
                        Parameters[30][19] = 0;
                        Parameters[30][20] = 0;
                        Parameters[30][21] = 0;
                        Parameters[30][22] = 0;
                        Parameters[30][23] = 0;
                        Parameters[30][24] = 0;
                        Parameters[30][25] = 0;
                        Parameters[30][26] = 0;
                        Parameters[30][27] = 0;
                        Parameters[30][28] = 0;
                        Parameters[30][29] = 0;
                        Parameters[30][30] = 0;
                        Parameters[30][31] = 0;
                        break;
                    case 31:
                        Parameters[31][0] = 20;
                        Parameters[31][1] = 64;
                        Parameters[31][2] = 6;
                        Parameters[31][3] = 15;
                        Parameters[31][4] = 15;
                        Parameters[31][5] = 127;
                        Parameters[31][6] = 0;
                        Parameters[31][7] = 0;
                        Parameters[31][8] = 0;
                        Parameters[31][9] = 0;
                        Parameters[31][10] = 0;
                        Parameters[31][11] = 0;
                        Parameters[31][12] = 0;
                        Parameters[31][13] = 0;
                        Parameters[31][14] = 0;
                        Parameters[31][15] = 0;
                        Parameters[31][16] = 0;
                        Parameters[31][17] = 0;
                        Parameters[31][18] = 0;
                        Parameters[31][19] = 0;
                        Parameters[31][20] = 0;
                        Parameters[31][21] = 0;
                        Parameters[31][22] = 0;
                        Parameters[31][23] = 0;
                        Parameters[31][24] = 0;
                        Parameters[31][25] = 0;
                        Parameters[31][26] = 0;
                        Parameters[31][27] = 0;
                        Parameters[31][28] = 0;
                        Parameters[31][29] = 0;
                        Parameters[31][30] = 0;
                        Parameters[31][31] = 0;
                        break;
                    case 32:
                        Parameters[32][0] = 32;
                        Parameters[32][1] = 64;
                        Parameters[32][2] = 2;
                        Parameters[32][3] = 6;
                        Parameters[32][4] = 15;
                        Parameters[32][5] = 15;
                        Parameters[32][6] = 127;
                        Parameters[32][7] = 0;
                        Parameters[32][8] = 0;
                        Parameters[32][9] = 0;
                        Parameters[32][10] = 0;
                        Parameters[32][11] = 0;
                        Parameters[32][12] = 0;
                        Parameters[32][13] = 0;
                        Parameters[32][14] = 0;
                        Parameters[32][15] = 0;
                        Parameters[32][16] = 0;
                        Parameters[32][17] = 0;
                        Parameters[32][18] = 0;
                        Parameters[32][19] = 0;
                        Parameters[32][20] = 0;
                        Parameters[32][21] = 0;
                        Parameters[32][22] = 0;
                        Parameters[32][23] = 0;
                        Parameters[32][24] = 0;
                        Parameters[32][25] = 0;
                        Parameters[32][26] = 0;
                        Parameters[32][27] = 0;
                        Parameters[32][28] = 0;
                        Parameters[32][29] = 0;
                        Parameters[32][30] = 0;
                        Parameters[32][31] = 0;
                        break;
                    case 33:
                        Parameters[33][0] = 70;
                        Parameters[33][1] = 0;
                        Parameters[33][2] = 8;
                        Parameters[33][3] = 0;
                        Parameters[33][4] = 16;
                        Parameters[33][5] = 100;
                        Parameters[33][6] = 127;
                        Parameters[33][7] = 0;
                        Parameters[33][8] = 0;
                        Parameters[33][9] = 0;
                        Parameters[33][10] = 0;
                        Parameters[33][11] = 0;
                        Parameters[33][12] = 0;
                        Parameters[33][13] = 0;
                        Parameters[33][14] = 0;
                        Parameters[33][15] = 0;
                        Parameters[33][16] = 0;
                        Parameters[33][17] = 0;
                        Parameters[33][18] = 0;
                        Parameters[33][19] = 0;
                        Parameters[33][20] = 0;
                        Parameters[33][21] = 0;
                        Parameters[33][22] = 0;
                        Parameters[33][23] = 0;
                        Parameters[33][24] = 0;
                        Parameters[33][25] = 0;
                        Parameters[33][26] = 0;
                        Parameters[33][27] = 0;
                        Parameters[33][28] = 0;
                        Parameters[33][29] = 0;
                        Parameters[33][30] = 0;
                        Parameters[33][31] = 0;
                        break;
                    case 34:
                        Parameters[34][0] = 1;
                        Parameters[34][1] = 600;
                        Parameters[34][2] = 12;
                        Parameters[34][3] = 1;
                        Parameters[34][4] = 600;
                        Parameters[34][5] = 12;
                        Parameters[34][6] = 0;
                        Parameters[34][7] = 0;
                        Parameters[34][8] = 0;
                        Parameters[34][9] = 59;
                        Parameters[34][10] = 17;
                        Parameters[34][11] = 15;
                        Parameters[34][12] = 15;
                        Parameters[34][13] = 50;
                        Parameters[34][14] = 127;
                        Parameters[34][15] = 0;
                        Parameters[34][16] = 0;
                        Parameters[34][17] = 0;
                        Parameters[34][18] = 0;
                        Parameters[34][19] = 0;
                        Parameters[34][20] = 0;
                        Parameters[34][21] = 0;
                        Parameters[34][22] = 0;
                        Parameters[34][23] = 0;
                        Parameters[34][24] = 0;
                        Parameters[34][25] = 0;
                        Parameters[34][26] = 0;
                        Parameters[34][27] = 0;
                        Parameters[34][28] = 0;
                        Parameters[34][29] = 0;
                        Parameters[34][30] = 0;
                        Parameters[34][31] = 0;
                        break;
                    case 35:
                        Parameters[35][0] = 1;
                        Parameters[35][1] = 600;
                        Parameters[35][2] = 12;
                        Parameters[35][3] = 1;
                        Parameters[35][4] = 600;
                        Parameters[35][5] = 12;
                        Parameters[35][6] = 0;
                        Parameters[35][7] = 59;
                        Parameters[35][8] = 17;
                        Parameters[35][9] = 0;
                        Parameters[35][10] = 10;
                        Parameters[35][11] = 18;
                        Parameters[35][12] = 20;
                        Parameters[35][13] = 90;
                        Parameters[35][14] = 15;
                        Parameters[35][15] = 15;
                        Parameters[35][16] = 50;
                        Parameters[35][17] = 127;
                        Parameters[35][18] = 0;
                        Parameters[35][19] = 0;
                        Parameters[35][20] = 0;
                        Parameters[35][21] = 0;
                        Parameters[35][22] = 0;
                        Parameters[35][23] = 0;
                        Parameters[35][24] = 0;
                        Parameters[35][25] = 0;
                        Parameters[35][26] = 0;
                        Parameters[35][27] = 0;
                        Parameters[35][28] = 0;
                        Parameters[35][29] = 0;
                        Parameters[35][30] = 0;
                        Parameters[35][31] = 0;
                        break;
                    case 36:
                        Parameters[36][0] = 1;
                        Parameters[36][1] = 400;
                        Parameters[36][2] = 10;
                        Parameters[36][3] = 1;
                        Parameters[36][4] = 800;
                        Parameters[36][5] = 13;
                        Parameters[36][6] = 1;
                        Parameters[36][7] = 1200;
                        Parameters[36][8] = 15;
                        Parameters[36][9] = 59;
                        Parameters[36][10] = 17;
                        Parameters[36][11] = 127;
                        Parameters[36][12] = 127;
                        Parameters[36][13] = 127;
                        Parameters[36][14] = 15;
                        Parameters[36][15] = 15;
                        Parameters[36][16] = 50;
                        Parameters[36][17] = 127;
                        Parameters[36][18] = 0;
                        Parameters[36][19] = 0;
                        Parameters[36][20] = 0;
                        Parameters[36][21] = 0;
                        Parameters[36][22] = 0;
                        Parameters[36][23] = 0;
                        Parameters[36][24] = 0;
                        Parameters[36][25] = 0;
                        Parameters[36][26] = 0;
                        Parameters[36][27] = 0;
                        Parameters[36][28] = 0;
                        Parameters[36][29] = 0;
                        Parameters[36][30] = 0;
                        Parameters[36][31] = 0;
                        break;
                    case 37:
                        Parameters[37][0] = 1;
                        Parameters[37][1] = 1200;
                        Parameters[37][2] = 15;
                        Parameters[37][3] = 1;
                        Parameters[37][4] = 900;
                        Parameters[37][5] = 14;
                        Parameters[37][6] = 1;
                        Parameters[37][7] = 600;
                        Parameters[37][8] = 12;
                        Parameters[37][9] = 1;
                        Parameters[37][10] = 300;
                        Parameters[37][11] = 9;
                        Parameters[37][12] = 59;
                        Parameters[37][13] = 17;
                        Parameters[37][14] = 127;
                        Parameters[37][15] = 127;
                        Parameters[37][16] = 127;
                        Parameters[37][17] = 127;
                        Parameters[37][18] = 15;
                        Parameters[37][19] = 15;
                        Parameters[37][20] = 50;
                        Parameters[37][21] = 127;
                        Parameters[37][22] = 0;
                        Parameters[37][23] = 0;
                        Parameters[37][24] = 0;
                        Parameters[37][25] = 0;
                        Parameters[37][26] = 0;
                        Parameters[37][27] = 0;
                        Parameters[37][28] = 0;
                        Parameters[37][29] = 0;
                        Parameters[37][30] = 0;
                        Parameters[37][31] = 0;
                        break;
                    case 38:
                        Parameters[38][0] = 1;
                        Parameters[38][1] = 1200;
                        Parameters[38][2] = 15;
                        Parameters[38][3] = 1;
                        Parameters[38][4] = 900;
                        Parameters[38][5] = 14;
                        Parameters[38][6] = 1;
                        Parameters[38][7] = 600;
                        Parameters[38][8] = 12;
                        Parameters[38][9] = 1;
                        Parameters[38][10] = 300;
                        Parameters[38][11] = 9;
                        Parameters[38][12] = 59;
                        Parameters[38][13] = 17;
                        Parameters[38][14] = 0;
                        Parameters[38][15] = 127;
                        Parameters[38][16] = 32;
                        Parameters[38][17] = 96;
                        Parameters[38][18] = 127;
                        Parameters[38][19] = 127;
                        Parameters[38][20] = 127;
                        Parameters[38][21] = 127;
                        Parameters[38][22] = 15;
                        Parameters[38][23] = 15;
                        Parameters[38][24] = 50;
                        Parameters[38][25] = 127;
                        Parameters[38][26] = 0;
                        Parameters[38][27] = 0;
                        Parameters[38][28] = 0;
                        Parameters[38][29] = 0;
                        Parameters[38][30] = 0;
                        Parameters[38][31] = 0;
                        break;
                    case 39:
                        Parameters[39][0] = 30;
                        Parameters[39][1] = 1;
                        Parameters[39][2] = 600;
                        Parameters[39][3] = 12;
                        Parameters[39][4] = 49;
                        Parameters[39][5] = 17;
                        Parameters[39][6] = 64;
                        Parameters[39][7] = 127;
                        Parameters[39][8] = 1;
                        Parameters[39][9] = 300;
                        Parameters[39][10] = 9;
                        Parameters[39][11] = 1;
                        Parameters[39][12] = 600;
                        Parameters[39][13] = 12;
                        Parameters[39][14] = 1;
                        Parameters[39][15] = 600;
                        Parameters[39][16] = 12;
                        Parameters[39][17] = 49;
                        Parameters[39][18] = 17;
                        Parameters[39][19] = 0;
                        Parameters[39][20] = 127;
                        Parameters[39][21] = 0;
                        Parameters[39][22] = 0;
                        Parameters[39][23] = 15;
                        Parameters[39][24] = 15;
                        Parameters[39][25] = 50;
                        Parameters[39][26] = 127;
                        Parameters[39][27] = 0;
                        Parameters[39][28] = 0;
                        Parameters[39][29] = 0;
                        Parameters[39][30] = 0;
                        Parameters[39][31] = 0;
                        break;
                    case 40:
                        Parameters[40][0] = 0;
                        Parameters[40][1] = 600;
                        Parameters[40][2] = 12;
                        Parameters[40][3] = 10;
                        Parameters[40][4] = 59;
                        Parameters[40][5] = 17;
                        Parameters[40][6] = 15;
                        Parameters[40][7] = 15;
                        Parameters[40][8] = 50;
                        Parameters[40][9] = 127;
                        Parameters[40][10] = 0;
                        Parameters[40][11] = 0;
                        Parameters[40][12] = 0;
                        Parameters[40][13] = 0;
                        Parameters[40][14] = 0;
                        Parameters[40][15] = 0;
                        Parameters[40][16] = 0;
                        Parameters[40][17] = 0;
                        Parameters[40][18] = 0;
                        Parameters[40][19] = 0;
                        Parameters[40][20] = 0;
                        Parameters[40][21] = 0;
                        Parameters[40][22] = 0;
                        Parameters[40][23] = 0;
                        Parameters[40][24] = 0;
                        Parameters[40][25] = 0;
                        Parameters[40][26] = 0;
                        Parameters[40][27] = 0;
                        Parameters[40][28] = 0;
                        Parameters[40][29] = 0;
                        Parameters[40][30] = 0;
                        Parameters[40][31] = 0;
                        break;
                    case 41:
                        Parameters[41][0] = 1;
                        Parameters[41][1] = 4;
                        Parameters[41][2] = 1;
                        Parameters[41][3] = 13;
                        Parameters[41][4] = 15;
                        Parameters[41][5] = 15;
                        Parameters[41][6] = 100;
                        Parameters[41][7] = 127;
                        Parameters[41][8] = 0;
                        Parameters[41][9] = 0;
                        Parameters[41][10] = 0;
                        Parameters[41][11] = 0;
                        Parameters[41][12] = 0;
                        Parameters[41][13] = 0;
                        Parameters[41][14] = 0;
                        Parameters[41][15] = 0;
                        Parameters[41][16] = 0;
                        Parameters[41][17] = 0;
                        Parameters[41][18] = 0;
                        Parameters[41][19] = 0;
                        Parameters[41][20] = 0;
                        Parameters[41][21] = 0;
                        Parameters[41][22] = 0;
                        Parameters[41][23] = 0;
                        Parameters[41][24] = 0;
                        Parameters[41][25] = 0;
                        Parameters[41][26] = 0;
                        Parameters[41][27] = 0;
                        Parameters[41][28] = 0;
                        Parameters[41][29] = 0;
                        Parameters[41][30] = 0;
                        Parameters[41][31] = 0;
                        break;
                    case 42:
                        Parameters[42][0] = 80;
                        Parameters[42][1] = 16;
                        Parameters[42][2] = 127;
                        Parameters[42][3] = 15;
                        Parameters[42][4] = 15;
                        Parameters[42][5] = 127;
                        Parameters[42][6] = 0;
                        Parameters[42][7] = 0;
                        Parameters[42][8] = 0;
                        Parameters[42][9] = 0;
                        Parameters[42][10] = 0;
                        Parameters[42][11] = 0;
                        Parameters[42][12] = 0;
                        Parameters[42][13] = 0;
                        Parameters[42][14] = 0;
                        Parameters[42][15] = 0;
                        Parameters[42][16] = 0;
                        Parameters[42][17] = 0;
                        Parameters[42][18] = 0;
                        Parameters[42][19] = 0;
                        Parameters[42][20] = 0;
                        Parameters[42][21] = 0;
                        Parameters[42][22] = 0;
                        Parameters[42][23] = 0;
                        Parameters[42][24] = 0;
                        Parameters[42][25] = 0;
                        Parameters[42][26] = 0;
                        Parameters[42][27] = 0;
                        Parameters[42][28] = 0;
                        Parameters[42][29] = 0;
                        Parameters[42][30] = 0;
                        Parameters[42][31] = 0;
                        break;
                    case 43:
                        Parameters[43][0] = 24;
                        Parameters[43][1] = 50;
                        Parameters[43][2] = 0;
                        Parameters[43][3] = 0;
                        Parameters[43][4] = 12;
                        Parameters[43][5] = 49;
                        Parameters[43][6] = 15;
                        Parameters[43][7] = 15;
                        Parameters[43][8] = 100;
                        Parameters[43][9] = 127;
                        Parameters[43][10] = 0;
                        Parameters[43][11] = 0;
                        Parameters[43][12] = 0;
                        Parameters[43][13] = 0;
                        Parameters[43][14] = 0;
                        Parameters[43][15] = 0;
                        Parameters[43][16] = 0;
                        Parameters[43][17] = 0;
                        Parameters[43][18] = 0;
                        Parameters[43][19] = 0;
                        Parameters[43][20] = 0;
                        Parameters[43][21] = 0;
                        Parameters[43][22] = 0;
                        Parameters[43][23] = 0;
                        Parameters[43][24] = 0;
                        Parameters[43][25] = 0;
                        Parameters[43][26] = 0;
                        Parameters[43][27] = 0;
                        Parameters[43][28] = 0;
                        Parameters[43][29] = 0;
                        Parameters[43][30] = 0;
                        Parameters[43][31] = 0;
                        break;
                    case 44:
                        Parameters[44][0] = 28;
                        Parameters[44][1] = 50;
                        Parameters[44][2] = 1;
                        Parameters[44][3] = 300;
                        Parameters[44][4] = 9;
                        Parameters[44][5] = 49;
                        Parameters[44][6] = 64;
                        Parameters[44][7] = 127;
                        Parameters[44][8] = 31;
                        Parameters[44][9] = 50;
                        Parameters[44][10] = 1;
                        Parameters[44][11] = 600;
                        Parameters[44][12] = 12;
                        Parameters[44][13] = 49;
                        Parameters[44][14] = 64;
                        Parameters[44][15] = 127;
                        Parameters[44][16] = 15;
                        Parameters[44][17] = 15;
                        Parameters[44][18] = 50;
                        Parameters[44][19] = 127;
                        Parameters[44][20] = 0;
                        Parameters[44][21] = 0;
                        Parameters[44][22] = 0;
                        Parameters[44][23] = 0;
                        Parameters[44][24] = 0;
                        Parameters[44][25] = 0;
                        Parameters[44][26] = 0;
                        Parameters[44][27] = 0;
                        Parameters[44][28] = 0;
                        Parameters[44][29] = 0;
                        Parameters[44][30] = 0;
                        Parameters[44][31] = 0;
                        break;
                    case 45:
                        Parameters[45][0] = 64;
                        Parameters[45][1] = 64;
                        Parameters[45][2] = 20;
                        Parameters[45][3] = 0;
                        Parameters[45][4] = 10;
                        Parameters[45][5] = 18;
                        Parameters[45][6] = 30;
                        Parameters[45][7] = 50;
                        Parameters[45][8] = 127;
                        Parameters[45][9] = 0;
                        Parameters[45][10] = 0;
                        Parameters[45][11] = 0;
                        Parameters[45][12] = 0;
                        Parameters[45][13] = 0;
                        Parameters[45][14] = 0;
                        Parameters[45][15] = 0;
                        Parameters[45][16] = 0;
                        Parameters[45][17] = 0;
                        Parameters[45][18] = 0;
                        Parameters[45][19] = 0;
                        Parameters[45][20] = 0;
                        Parameters[45][21] = 0;
                        Parameters[45][22] = 0;
                        Parameters[45][23] = 0;
                        Parameters[45][24] = 0;
                        Parameters[45][25] = 0;
                        Parameters[45][26] = 0;
                        Parameters[45][27] = 0;
                        Parameters[45][28] = 0;
                        Parameters[45][29] = 0;
                        Parameters[45][30] = 0;
                        Parameters[45][31] = 0;
                        break;
                    case 46:
                        Parameters[46][0] = 64;
                        Parameters[46][1] = 64;
                        Parameters[46][2] = 20;
                        Parameters[46][3] = 1;
                        Parameters[46][4] = 10;
                        Parameters[46][5] = 18;
                        Parameters[46][6] = 40;
                        Parameters[46][7] = 79;
                        Parameters[46][8] = 50;
                        Parameters[46][9] = 127;
                        Parameters[46][10] = 0;
                        Parameters[46][11] = 0;
                        Parameters[46][12] = 0;
                        Parameters[46][13] = 0;
                        Parameters[46][14] = 0;
                        Parameters[46][15] = 0;
                        Parameters[46][16] = 0;
                        Parameters[46][17] = 0;
                        Parameters[46][18] = 0;
                        Parameters[46][19] = 0;
                        Parameters[46][20] = 0;
                        Parameters[46][21] = 0;
                        Parameters[46][22] = 0;
                        Parameters[46][23] = 0;
                        Parameters[46][24] = 0;
                        Parameters[46][25] = 0;
                        Parameters[46][26] = 0;
                        Parameters[46][27] = 0;
                        Parameters[46][28] = 0;
                        Parameters[46][29] = 0;
                        Parameters[46][30] = 0;
                        Parameters[46][31] = 0;
                        break;
                    case 47:
                        Parameters[47][0] = 64;
                        Parameters[47][1] = 64;
                        Parameters[47][2] = 1;
                        Parameters[47][3] = 600;
                        Parameters[47][4] = 12;
                        Parameters[47][5] = 59;
                        Parameters[47][6] = 17;
                        Parameters[47][7] = 50;
                        Parameters[47][8] = 127;
                        Parameters[47][9] = 0;
                        Parameters[47][10] = 0;
                        Parameters[47][11] = 0;
                        Parameters[47][12] = 0;
                        Parameters[47][13] = 0;
                        Parameters[47][14] = 0;
                        Parameters[47][15] = 0;
                        Parameters[47][16] = 0;
                        Parameters[47][17] = 0;
                        Parameters[47][18] = 0;
                        Parameters[47][19] = 0;
                        Parameters[47][20] = 0;
                        Parameters[47][21] = 0;
                        Parameters[47][22] = 0;
                        Parameters[47][23] = 0;
                        Parameters[47][24] = 0;
                        Parameters[47][25] = 0;
                        Parameters[47][26] = 0;
                        Parameters[47][27] = 0;
                        Parameters[47][28] = 0;
                        Parameters[47][29] = 0;
                        Parameters[47][30] = 0;
                        Parameters[47][31] = 0;
                        break;
                    case 48:
                        Parameters[48][0] = 127;
                        Parameters[48][1] = 64;
                        Parameters[48][2] = 20;
                        Parameters[48][3] = 0;
                        Parameters[48][4] = 10;
                        Parameters[48][5] = 18;
                        Parameters[48][6] = 30;
                        Parameters[48][7] = 50;
                        Parameters[48][8] = 127;
                        Parameters[48][9] = 0;
                        Parameters[48][10] = 0;
                        Parameters[48][11] = 0;
                        Parameters[48][12] = 0;
                        Parameters[48][13] = 0;
                        Parameters[48][14] = 0;
                        Parameters[48][15] = 0;
                        Parameters[48][16] = 0;
                        Parameters[48][17] = 0;
                        Parameters[48][18] = 0;
                        Parameters[48][19] = 0;
                        Parameters[48][20] = 0;
                        Parameters[48][21] = 0;
                        Parameters[48][22] = 0;
                        Parameters[48][23] = 0;
                        Parameters[48][24] = 0;
                        Parameters[48][25] = 0;
                        Parameters[48][26] = 0;
                        Parameters[48][27] = 0;
                        Parameters[48][28] = 0;
                        Parameters[48][29] = 0;
                        Parameters[48][30] = 0;
                        Parameters[48][31] = 0;
                        break;
                    case 49:
                        Parameters[49][0] = 127;
                        Parameters[49][1] = 64;
                        Parameters[49][2] = 20;
                        Parameters[49][3] = 1;
                        Parameters[49][4] = 10;
                        Parameters[49][5] = 18;
                        Parameters[49][6] = 40;
                        Parameters[49][7] = 79;
                        Parameters[49][8] = 50;
                        Parameters[49][9] = 127;
                        Parameters[49][10] = 0;
                        Parameters[49][11] = 0;
                        Parameters[49][12] = 0;
                        Parameters[49][13] = 0;
                        Parameters[49][14] = 0;
                        Parameters[49][15] = 0;
                        Parameters[49][16] = 0;
                        Parameters[49][17] = 0;
                        Parameters[49][18] = 0;
                        Parameters[49][19] = 0;
                        Parameters[49][20] = 0;
                        Parameters[49][21] = 0;
                        Parameters[49][22] = 0;
                        Parameters[49][23] = 0;
                        Parameters[49][24] = 0;
                        Parameters[49][25] = 0;
                        Parameters[49][26] = 0;
                        Parameters[49][27] = 0;
                        Parameters[49][28] = 0;
                        Parameters[49][29] = 0;
                        Parameters[49][30] = 0;
                        Parameters[49][31] = 0;
                        break;
                    case 50:
                        Parameters[50][0] = 127;
                        Parameters[50][1] = 64;
                        Parameters[50][2] = 1;
                        Parameters[50][3] = 600;
                        Parameters[50][4] = 12;
                        Parameters[50][5] = 59;
                        Parameters[50][6] = 17;
                        Parameters[50][7] = 50;
                        Parameters[50][8] = 127;
                        Parameters[50][9] = 0;
                        Parameters[50][10] = 0;
                        Parameters[50][11] = 0;
                        Parameters[50][12] = 0;
                        Parameters[50][13] = 0;
                        Parameters[50][14] = 0;
                        Parameters[50][15] = 0;
                        Parameters[50][16] = 0;
                        Parameters[50][17] = 0;
                        Parameters[50][18] = 0;
                        Parameters[50][19] = 0;
                        Parameters[50][20] = 0;
                        Parameters[50][21] = 0;
                        Parameters[50][22] = 0;
                        Parameters[50][23] = 0;
                        Parameters[50][24] = 0;
                        Parameters[50][25] = 0;
                        Parameters[50][26] = 0;
                        Parameters[50][27] = 0;
                        Parameters[50][28] = 0;
                        Parameters[50][29] = 0;
                        Parameters[50][30] = 0;
                        Parameters[50][31] = 0;
                        break;
                    case 51:
                        Parameters[51][0] = 1;
                        Parameters[51][1] = 0;
                        Parameters[51][2] = 50;
                        Parameters[51][3] = 50;
                        Parameters[51][4] = 1;
                        Parameters[51][5] = 0;
                        Parameters[51][6] = 1;
                        Parameters[51][7] = 1;
                        Parameters[51][8] = 0;
                        Parameters[51][9] = 50;
                        Parameters[51][10] = 30;
                        Parameters[51][11] = 50;
                        Parameters[51][12] = 100;
                        Parameters[51][13] = 15;
                        Parameters[51][14] = 15;
                        Parameters[51][15] = 127;
                        Parameters[51][16] = 0;
                        Parameters[51][17] = 0;
                        Parameters[51][18] = 0;
                        Parameters[51][19] = 0;
                        Parameters[51][20] = 0;
                        Parameters[51][21] = 0;
                        Parameters[51][22] = 0;
                        Parameters[51][23] = 0;
                        Parameters[51][24] = 0;
                        Parameters[51][25] = 0;
                        Parameters[51][26] = 0;
                        Parameters[51][27] = 0;
                        Parameters[51][28] = 0;
                        Parameters[51][29] = 0;
                        Parameters[51][30] = 0;
                        Parameters[51][31] = 0;
                        break;
                    case 52:
                        Parameters[52][0] = 1;
                        Parameters[52][1] = 0;
                        Parameters[52][2] = 50;
                        Parameters[52][3] = 50;
                        Parameters[52][4] = 1;
                        Parameters[52][5] = 0;
                        Parameters[52][6] = 1;
                        Parameters[52][7] = 1;
                        Parameters[52][8] = 60;
                        Parameters[52][9] = 50;
                        Parameters[52][10] = 0;
                        Parameters[52][11] = 10;
                        Parameters[52][12] = 11;
                        Parameters[52][13] = 30;
                        Parameters[52][14] = 100;
                        Parameters[52][15] = 15;
                        Parameters[52][16] = 15;
                        Parameters[52][17] = 127;
                        Parameters[52][18] = 0;
                        Parameters[52][19] = 0;
                        Parameters[52][20] = 0;
                        Parameters[52][21] = 0;
                        Parameters[52][22] = 0;
                        Parameters[52][23] = 0;
                        Parameters[52][24] = 0;
                        Parameters[52][25] = 0;
                        Parameters[52][26] = 0;
                        Parameters[52][27] = 0;
                        Parameters[52][28] = 0;
                        Parameters[52][29] = 0;
                        Parameters[52][30] = 0;
                        Parameters[52][31] = 0;
                        break;
                    case 53:
                        Parameters[53][0] = 1;
                        Parameters[53][1] = 1;
                        Parameters[53][2] = 80;
                        Parameters[53][3] = 100;
                        Parameters[53][4] = 1;
                        Parameters[53][5] = 64;
                        Parameters[53][6] = 64;
                        Parameters[53][7] = 64;
                        Parameters[53][8] = 1;
                        Parameters[53][9] = 4;
                        Parameters[53][10] = 1;
                        Parameters[53][11] = 20;
                        Parameters[53][12] = 10;
                        Parameters[53][13] = 30;
                        Parameters[53][14] = 50;
                        Parameters[53][15] = 127;
                        Parameters[53][16] = 0;
                        Parameters[53][17] = 0;
                        Parameters[53][18] = 0;
                        Parameters[53][19] = 0;
                        Parameters[53][20] = 0;
                        Parameters[53][21] = 0;
                        Parameters[53][22] = 0;
                        Parameters[53][23] = 0;
                        Parameters[53][24] = 0;
                        Parameters[53][25] = 0;
                        Parameters[53][26] = 0;
                        Parameters[53][27] = 0;
                        Parameters[53][28] = 0;
                        Parameters[53][29] = 0;
                        Parameters[53][30] = 0;
                        Parameters[53][31] = 0;
                        break;
                    case 54:
                        Parameters[54][0] = 1;
                        Parameters[54][1] = 1;
                        Parameters[54][2] = 80;
                        Parameters[54][3] = 100;
                        Parameters[54][4] = 1;
                        Parameters[54][5] = 64;
                        Parameters[54][6] = 64;
                        Parameters[54][7] = 64;
                        Parameters[54][8] = 1;
                        Parameters[54][9] = 4;
                        Parameters[54][10] = 1;
                        Parameters[54][11] = 20;
                        Parameters[54][12] = 10;
                        Parameters[54][13] = 40;
                        Parameters[54][14] = 79;
                        Parameters[54][15] = 50;
                        Parameters[54][16] = 127;
                        Parameters[54][17] = 0;
                        Parameters[54][18] = 0;
                        Parameters[54][19] = 0;
                        Parameters[54][20] = 0;
                        Parameters[54][21] = 0;
                        Parameters[54][22] = 0;
                        Parameters[54][23] = 0;
                        Parameters[54][24] = 0;
                        Parameters[54][25] = 0;
                        Parameters[54][26] = 0;
                        Parameters[54][27] = 0;
                        Parameters[54][28] = 0;
                        Parameters[54][29] = 0;
                        Parameters[54][30] = 0;
                        Parameters[54][31] = 0;
                        break;
                    case 55:
                        Parameters[55][0] = 1;
                        Parameters[55][1] = 1;
                        Parameters[55][2] = 80;
                        Parameters[55][3] = 100;
                        Parameters[55][4] = 1;
                        Parameters[55][5] = 64;
                        Parameters[55][6] = 64;
                        Parameters[55][7] = 64;
                        Parameters[55][8] = 1;
                        Parameters[55][9] = 4;
                        Parameters[55][10] = 1;
                        Parameters[55][11] = 10;
                        Parameters[55][12] = 60;
                        Parameters[55][13] = 40;
                        Parameters[55][14] = 80;
                        Parameters[55][15] = 127;
                        Parameters[55][16] = 127;
                        Parameters[55][17] = 0;
                        Parameters[55][18] = 0;
                        Parameters[55][19] = 0;
                        Parameters[55][20] = 0;
                        Parameters[55][21] = 0;
                        Parameters[55][22] = 0;
                        Parameters[55][23] = 0;
                        Parameters[55][24] = 0;
                        Parameters[55][25] = 0;
                        Parameters[55][26] = 0;
                        Parameters[55][27] = 0;
                        Parameters[55][28] = 0;
                        Parameters[55][29] = 0;
                        Parameters[55][30] = 0;
                        Parameters[55][31] = 0;
                        break;
                    case 56:
                        Parameters[56][0] = 1;
                        Parameters[56][1] = 1;
                        Parameters[56][2] = 80;
                        Parameters[56][3] = 100;
                        Parameters[56][4] = 1;
                        Parameters[56][5] = 64;
                        Parameters[56][6] = 64;
                        Parameters[56][7] = 64;
                        Parameters[56][8] = 1;
                        Parameters[56][9] = 4;
                        Parameters[56][10] = 1;
                        Parameters[56][11] = 600;
                        Parameters[56][12] = 59;
                        Parameters[56][13] = 17;
                        Parameters[56][14] = 50;
                        Parameters[56][15] = 127;
                        Parameters[56][16] = 0;
                        Parameters[56][17] = 0;
                        Parameters[56][18] = 0;
                        Parameters[56][19] = 0;
                        Parameters[56][20] = 0;
                        Parameters[56][21] = 0;
                        Parameters[56][22] = 0;
                        Parameters[56][23] = 0;
                        Parameters[56][24] = 0;
                        Parameters[56][25] = 0;
                        Parameters[56][26] = 0;
                        Parameters[56][27] = 0;
                        Parameters[56][28] = 0;
                        Parameters[56][29] = 0;
                        Parameters[56][30] = 0;
                        Parameters[56][31] = 0;
                        break;
                    case 57:
                        Parameters[57][0] = 0;
                        Parameters[57][1] = 50;
                        Parameters[57][2] = 50;
                        Parameters[57][3] = 1;
                        Parameters[57][4] = 0;
                        Parameters[57][5] = 25;
                        Parameters[57][6] = 11;
                        Parameters[57][7] = 50;
                        Parameters[57][8] = 8;
                        Parameters[57][9] = 1;
                        Parameters[57][10] = 1;
                        Parameters[57][11] = 32;
                        Parameters[57][12] = 0;
                        Parameters[57][13] = 127;
                        Parameters[57][14] = 0;
                        Parameters[57][15] = 0;
                        Parameters[57][16] = 0;
                        Parameters[57][17] = 0;
                        Parameters[57][18] = 0;
                        Parameters[57][19] = 0;
                        Parameters[57][20] = 0;
                        Parameters[57][21] = 0;
                        Parameters[57][22] = 0;
                        Parameters[57][23] = 0;
                        Parameters[57][24] = 0;
                        Parameters[57][25] = 0;
                        Parameters[57][26] = 0;
                        Parameters[57][27] = 0;
                        Parameters[57][28] = 0;
                        Parameters[57][29] = 0;
                        Parameters[57][30] = 0;
                        Parameters[57][31] = 0;
                        break;
                    case 58:
                        Parameters[58][0] = 0;
                        Parameters[58][1] = 50;
                        Parameters[58][2] = 50;
                        Parameters[58][3] = 1;
                        Parameters[58][4] = 20;
                        Parameters[58][5] = 0;
                        Parameters[58][6] = 10;
                        Parameters[58][7] = 11;
                        Parameters[58][8] = 30;
                        Parameters[58][9] = 50;
                        Parameters[58][10] = 1;
                        Parameters[58][11] = 1;
                        Parameters[58][12] = 32;
                        Parameters[58][13] = 0;
                        Parameters[58][14] = 127;
                        Parameters[58][15] = 0;
                        Parameters[58][16] = 0;
                        Parameters[58][17] = 0;
                        Parameters[58][18] = 0;
                        Parameters[58][19] = 0;
                        Parameters[58][20] = 0;
                        Parameters[58][21] = 0;
                        Parameters[58][22] = 0;
                        Parameters[58][23] = 0;
                        Parameters[58][24] = 0;
                        Parameters[58][25] = 0;
                        Parameters[58][26] = 0;
                        Parameters[58][27] = 0;
                        Parameters[58][28] = 0;
                        Parameters[58][29] = 0;
                        Parameters[58][30] = 0;
                        Parameters[58][31] = 0;
                        break;
                    case 59:
                        Parameters[59][0] = 0;
                        Parameters[59][1] = 50;
                        Parameters[59][2] = 50;
                        Parameters[59][3] = 1;
                        Parameters[59][4] = 20;
                        Parameters[59][5] = 0;
                        Parameters[59][6] = 10;
                        Parameters[59][7] = 11;
                        Parameters[59][8] = 30;
                        Parameters[59][9] = 79;
                        Parameters[59][10] = 50;
                        Parameters[59][11] = 1;
                        Parameters[59][12] = 1;
                        Parameters[59][13] = 32;
                        Parameters[59][14] = 0;
                        Parameters[59][15] = 127;
                        Parameters[59][16] = 0;
                        Parameters[59][17] = 0;
                        Parameters[59][18] = 0;
                        Parameters[59][19] = 0;
                        Parameters[59][20] = 0;
                        Parameters[59][21] = 0;
                        Parameters[59][22] = 0;
                        Parameters[59][23] = 0;
                        Parameters[59][24] = 0;
                        Parameters[59][25] = 0;
                        Parameters[59][26] = 0;
                        Parameters[59][27] = 0;
                        Parameters[59][28] = 0;
                        Parameters[59][29] = 0;
                        Parameters[59][30] = 0;
                        Parameters[59][31] = 0;
                        break;
                    case 60:
                        Parameters[60][0] = 0;
                        Parameters[60][1] = 50;
                        Parameters[60][2] = 50;
                        Parameters[60][3] = 1;
                        Parameters[60][4] = 0;
                        Parameters[60][5] = 10;
                        Parameters[60][6] = 11;
                        Parameters[60][7] = 60;
                        Parameters[60][8] = 40;
                        Parameters[60][9] = 80;
                        Parameters[60][10] = 127;
                        Parameters[60][11] = 1;
                        Parameters[60][12] = 1;
                        Parameters[60][13] = 32;
                        Parameters[60][14] = 0;
                        Parameters[60][15] = 127;
                        Parameters[60][16] = 0;
                        Parameters[60][17] = 0;
                        Parameters[60][18] = 0;
                        Parameters[60][19] = 0;
                        Parameters[60][20] = 0;
                        Parameters[60][21] = 0;
                        Parameters[60][22] = 0;
                        Parameters[60][23] = 0;
                        Parameters[60][24] = 0;
                        Parameters[60][25] = 0;
                        Parameters[60][26] = 0;
                        Parameters[60][27] = 0;
                        Parameters[60][28] = 0;
                        Parameters[60][29] = 0;
                        Parameters[60][30] = 0;
                        Parameters[60][31] = 0;
                        break;
                    case 61:
                        Parameters[61][0] = 0;
                        Parameters[61][1] = 50;
                        Parameters[61][2] = 50;
                        Parameters[61][3] = 1;
                        Parameters[61][4] = 0;
                        Parameters[61][5] = 600;
                        Parameters[61][6] = 11;
                        Parameters[61][7] = 10;
                        Parameters[61][8] = 59;
                        Parameters[61][9] = 17;
                        Parameters[61][10] = 50;
                        Parameters[61][11] = 1;
                        Parameters[61][12] = 1;
                        Parameters[61][13] = 32;
                        Parameters[61][14] = 0;
                        Parameters[61][15] = 127;
                        Parameters[61][16] = 0;
                        Parameters[61][17] = 0;
                        Parameters[61][18] = 0;
                        Parameters[61][19] = 0;
                        Parameters[61][20] = 0;
                        Parameters[61][21] = 0;
                        Parameters[61][22] = 0;
                        Parameters[61][23] = 0;
                        Parameters[61][24] = 0;
                        Parameters[61][25] = 0;
                        Parameters[61][26] = 0;
                        Parameters[61][27] = 0;
                        Parameters[61][28] = 0;
                        Parameters[61][29] = 0;
                        Parameters[61][30] = 0;
                        Parameters[61][31] = 0;
                        break;
                    case 62:
                        Parameters[62][0] = 64;
                        Parameters[62][1] = 64;
                        Parameters[62][2] = 20;
                        Parameters[62][3] = 0;
                        Parameters[62][4] = 10;
                        Parameters[62][5] = 18;
                        Parameters[62][6] = 30;
                        Parameters[62][7] = 50;
                        Parameters[62][8] = 127;
                        Parameters[62][9] = 0;
                        Parameters[62][10] = 0;
                        Parameters[62][11] = 0;
                        Parameters[62][12] = 0;
                        Parameters[62][13] = 0;
                        Parameters[62][14] = 0;
                        Parameters[62][15] = 0;
                        Parameters[62][16] = 0;
                        Parameters[62][17] = 0;
                        Parameters[62][18] = 0;
                        Parameters[62][19] = 0;
                        Parameters[62][20] = 0;
                        Parameters[62][21] = 0;
                        Parameters[62][22] = 0;
                        Parameters[62][23] = 0;
                        Parameters[62][24] = 0;
                        Parameters[62][25] = 0;
                        Parameters[62][26] = 0;
                        Parameters[62][27] = 0;
                        Parameters[62][28] = 0;
                        Parameters[62][29] = 0;
                        Parameters[62][30] = 0;
                        Parameters[62][31] = 0;
                        break;
                    case 63:
                        Parameters[63][0] = 64;
                        Parameters[63][1] = 64;
                        Parameters[63][2] = 20;
                        Parameters[63][3] = 1;
                        Parameters[63][4] = 10;
                        Parameters[63][5] = 18;
                        Parameters[63][6] = 40;
                        Parameters[63][7] = 79;
                        Parameters[63][8] = 50;
                        Parameters[63][9] = 127;
                        Parameters[63][10] = 0;
                        Parameters[63][11] = 0;
                        Parameters[63][12] = 0;
                        Parameters[63][13] = 0;
                        Parameters[63][14] = 0;
                        Parameters[63][15] = 0;
                        Parameters[63][16] = 0;
                        Parameters[63][17] = 0;
                        Parameters[63][18] = 0;
                        Parameters[63][19] = 0;
                        Parameters[63][20] = 0;
                        Parameters[63][21] = 0;
                        Parameters[63][22] = 0;
                        Parameters[63][23] = 0;
                        Parameters[63][24] = 0;
                        Parameters[63][25] = 0;
                        Parameters[63][26] = 0;
                        Parameters[63][27] = 0;
                        Parameters[63][28] = 0;
                        Parameters[63][29] = 0;
                        Parameters[63][30] = 0;
                        Parameters[63][31] = 0;
                        break;
                    case 64:
                        Parameters[64][0] = 64;
                        Parameters[64][1] = 64;
                        Parameters[64][2] = 1;
                        Parameters[64][3] = 600;
                        Parameters[64][4] = 12;
                        Parameters[64][5] = 59;
                        Parameters[64][6] = 17;
                        Parameters[64][7] = 50;
                        Parameters[64][8] = 127;
                        Parameters[64][9] = 0;
                        Parameters[64][10] = 0;
                        Parameters[64][11] = 0;
                        Parameters[64][12] = 0;
                        Parameters[64][13] = 0;
                        Parameters[64][14] = 0;
                        Parameters[64][15] = 0;
                        Parameters[64][16] = 0;
                        Parameters[64][17] = 0;
                        Parameters[64][18] = 0;
                        Parameters[64][19] = 0;
                        Parameters[64][20] = 0;
                        Parameters[64][21] = 0;
                        Parameters[64][22] = 0;
                        Parameters[64][23] = 0;
                        Parameters[64][24] = 0;
                        Parameters[64][25] = 0;
                        Parameters[64][26] = 0;
                        Parameters[64][27] = 0;
                        Parameters[64][28] = 0;
                        Parameters[64][29] = 0;
                        Parameters[64][30] = 0;
                        Parameters[64][31] = 0;
                        break;
                    case 65:
                        Parameters[65][0] = 20;
                        Parameters[65][1] = 0;
                        Parameters[65][2] = 10;
                        Parameters[65][3] = 18;
                        Parameters[65][4] = 30;
                        Parameters[65][5] = 50;
                        Parameters[65][6] = 1;
                        Parameters[65][7] = 600;
                        Parameters[65][8] = 12;
                        Parameters[65][9] = 59;
                        Parameters[65][10] = 17;
                        Parameters[65][11] = 50;
                        Parameters[65][12] = 127;
                        Parameters[65][13] = 0;
                        Parameters[65][14] = 0;
                        Parameters[65][15] = 0;
                        Parameters[65][16] = 0;
                        Parameters[65][17] = 0;
                        Parameters[65][18] = 0;
                        Parameters[65][19] = 0;
                        Parameters[65][20] = 0;
                        Parameters[65][21] = 0;
                        Parameters[65][22] = 0;
                        Parameters[65][23] = 0;
                        Parameters[65][24] = 0;
                        Parameters[65][25] = 0;
                        Parameters[65][26] = 0;
                        Parameters[65][27] = 0;
                        Parameters[65][28] = 0;
                        Parameters[65][29] = 0;
                        Parameters[65][30] = 0;
                        Parameters[65][31] = 0;
                        break;
                    case 66:
                        Parameters[66][0] = 20;
                        Parameters[66][1] = 1;
                        Parameters[66][2] = 10;
                        Parameters[66][3] = 18;
                        Parameters[66][4] = 40;
                        Parameters[66][5] = 79;
                        Parameters[66][6] = 50;
                        Parameters[66][7] = 1;
                        Parameters[66][8] = 600;
                        Parameters[66][9] = 12;
                        Parameters[66][10] = 59;
                        Parameters[66][11] = 17;
                        Parameters[66][12] = 50;
                        Parameters[66][13] = 127;
                        Parameters[66][14] = 0;
                        Parameters[66][15] = 0;
                        Parameters[66][16] = 0;
                        Parameters[66][17] = 0;
                        Parameters[66][18] = 0;
                        Parameters[66][19] = 0;
                        Parameters[66][20] = 0;
                        Parameters[66][21] = 0;
                        Parameters[66][22] = 0;
                        Parameters[66][23] = 0;
                        Parameters[66][24] = 0;
                        Parameters[66][25] = 0;
                        Parameters[66][26] = 0;
                        Parameters[66][27] = 0;
                        Parameters[66][28] = 0;
                        Parameters[66][29] = 0;
                        Parameters[66][30] = 0;
                        Parameters[66][31] = 0;
                        break;
                    case 67:
                        Parameters[67][0] = 20;
                        Parameters[67][1] = 0;
                        Parameters[67][2] = 10;
                        Parameters[67][3] = 18;
                        Parameters[67][4] = 30;
                        Parameters[67][5] = 50;
                        Parameters[67][6] = 20;
                        Parameters[67][7] = 1;
                        Parameters[67][8] = 10;
                        Parameters[67][9] = 18;
                        Parameters[67][10] = 40;
                        Parameters[67][11] = 79;
                        Parameters[67][12] = 50;
                        Parameters[67][13] = 127;
                        Parameters[67][14] = 0;
                        Parameters[67][15] = 0;
                        Parameters[67][16] = 0;
                        Parameters[67][17] = 0;
                        Parameters[67][18] = 0;
                        Parameters[67][19] = 0;
                        Parameters[67][20] = 0;
                        Parameters[67][21] = 0;
                        Parameters[67][22] = 0;
                        Parameters[67][23] = 0;
                        Parameters[67][24] = 0;
                        Parameters[67][25] = 0;
                        Parameters[67][26] = 0;
                        Parameters[67][27] = 0;
                        Parameters[67][28] = 0;
                        Parameters[67][29] = 0;
                        Parameters[67][30] = 0;
                        Parameters[67][31] = 0;
                        break;
                }
            }
        }
    }
}
