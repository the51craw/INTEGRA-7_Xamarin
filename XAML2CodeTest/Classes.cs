using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
//using Xamarin.Forms;
using System.Runtime.Serialization;
using Integra_7_Xamarin;

namespace XAML2CodeTest
{
    public class HBTrace
    {
        Int32 debugLevel = -1; // 0 or greater enables if not given in call
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

    public class Player
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
            //*** byte[] data = new byte[] { (byte)(commonState.midi.MidiOutPortChannel + 1) };
            //*** byte[] package = commonState.midi.SystemExclusiveDT1Message(address, data);
            //*** commonState.midi.SendSystemExclusive(package);
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
    public class CommonState
    {
        public enum ReactToMidiIn
        {
            MAIN,
            EDIT,
            EDIT_STUDIO_SET,
            SURROUND,
        }

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
        public IMidi midi { get; set; }
        public Tone currentTone { get; set; }
        //public Int32 currentToneIndex { get; set; }
        public ToneList toneList { get; set; }
        public List<List<String>> toneNames { get; set; } // Will hold names of user tones.
        public List<String> keyNames { get; set; }  // Will hold names of keys for drum sets for edit.
        public DrumKeyAssignLists drumKeyAssignLists { get; set; }
        public Int32 PresetDrumKeyAssignListsCount { get; set; }
        public FavoritesList favoritesList { get; set; }
        public Player player { get; set; }
        public List<String> studioSetNames { get; set; }
        //*** public StudioSet studioSet { get; set; }
        public ToneTypes ToneType { get; set; }
        public SimpleToneTypes SimpleToneType { get; set; }
        public String ToneSource {get;set;}
        public byte CurrentStudioSet { get; set; }
        public byte CurrentPart { get; set; }
        public byte[] PartChannels { get; set; }
        public ReactToMidiIn reactToMidiIn { get; set; }
        public Boolean VenderDriverIsInstalled { get; set; }
        //public PickerLocationId pickerLocationId { get; set; }

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
            //*** studioSet = null;
            CurrentPart = 0;
            PartChannels = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11,
                0x12, 0x13, 0x14, 0x15, 0xff }; // Last one is for EXT, which has no channel, but may be selected.
            keyNames = new List<String>();
            drumKeyAssignLists = new DrumKeyAssignLists();
            PresetDrumKeyAssignListsCount = drumKeyAssignLists.ToneNames.Count();
            reactToMidiIn = ReactToMidiIn.MAIN;
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
    [DataContract]
    public class Tone
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Group { get; set; }
        [DataMember]
        public String Category { get; set; }
        [DataMember]
        public Int32 GroupIndex { get; set; }
        [DataMember]
        public Int32 CategoryIndex { get; set; }
        [DataMember]
        public Int32 ToneIndex { get; set; }
        [DataMember]
        public Int32 Index { get; set; }
        [DataMember]
        public Int32 VariationIndex { get; set; }
        [DataMember]
        public byte BankMSB { get; set; }
        [DataMember]
        public byte BankLSB { get; set; }
        [DataMember]
        public byte Program { get; set; }
        [DataMember]
        public ProgramType ProgramType { get; }
        [DataMember]
        public String ProgramBank { get; }
        [DataMember]
        public UInt32 Id { get; }

        public Tone(Int32 GroupIndex = -1, Int32 CategoryIndex = -1, Int32 ToneIndex = -1, String Group = "", String Category = "", String Name = "", Int32 Index = -1, Int32 VariationIndex = -1)
        {
            this.GroupIndex = GroupIndex;
            this.CategoryIndex = CategoryIndex;
            this.ToneIndex = ToneIndex;
            this.Group = Group;
            this.Category = Category;
            this.Name = Name;
            this.Index = Index;
            this.VariationIndex = VariationIndex;
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
            this.VariationIndex = Int32.Parse(tone[10]);
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

    class SRXWaves
    {
        public List<String[]> srxWaveNames { get; }

        public SRXWaves()
        {
            srxWaveNames = new List<String[]>();
            srxWaveNames.Add(new String[] { // SRX-01
                "Off", "Kick 1 Menu", "Kick 2 MenuL", "Kick 2 MenuR", "Kick 3 MenuL", "Kick 3 MenuR",
                "Kick 4 Menu", "Latin K sft", "Latin K med", "Latin K hrd", "ColDwnK sft", "ColDwnK med",
                "ColDwnK hrd", "70NtrlK sft", "70NtrlK hrd", "70PhnqK sft", "70PhnqK hrd", "Disco K sft",
                "Disco K hrd", "OldFnkK sft", "OldFnkK hrd", "StudioK sftL", "StudioK sftR",
                "StudioK medL", "StudioK medR", "StudioK hrdL", "StudioK hrdR", "StdRckK sftL",
                "StdRckK sftR", "StdRckK hrdL", "StdRckK hrdR", "StreetK sftL", "StreetK sftR",
                "StreetK hrdL", "StreetK hrdR", "OhBabyK sftL", "OhBabyK sftR", "OhBabyK hrdL",
                "OhBabyK hrdR", "PwrBldK sftL", "PwrBldK sftR", "PwrBldK medL", "PwrBldK medR",
                "PwrBldK hrdL", "PwrBldK hrdR", "B.DeepK sftL", "B.DeepK sftR", "B.DeepK hrdL",
                "B.DeepK hrdR", "BigRckK sftL", "BigRckK sftR", "BigRckK hrdL", "BigRckK hrdR",
                "WhamBmK sftL", "WhamBmK sftR", "WhamBmK hrdL", "WhamBmK hrdR", "StdJazK sft",
                "StdJazK hrd", "OldJazK sft", "OldJazK hrd", "RoomJzK sft", "RoomJzK hrd", "JzSoulK sft",
                "JzSoulK hrd", "OldFnkSMenu", "Latin SMenu", "HiPiccSMenuL", "HiPiccSMenuR", "Tight SMenuL",
                "Tight SMenuR", "StdRk1SMenuL", "StdRk1SMenuR", "StdRk2SMenuL", "StdRk2SMenuR",
                "OhBabySMenuL", "OhBabySMenuR", "PoppinSMenuL", "PoppinSMenuR", "StdFatSMenuL",
                "StdFatSMenuR", "JazStkSMenuL", "JazStkSMenuR", "StdJazSMenuL", "StdJazSMenuR",
                "BalladSMenuL", "BalladSMenuR", "PwrBldSMenuL", "PwrBldSMenuR", "AlBeefSMenuL",
                "AlBeefSMenuR", "BigRckSMenuL", "BigRckSMenuR", "70BrshSMenu", "JzBrshSMenuL",
                "JzBrshSMenuR", "CrsStk MenuL", "CrsStk MenuR", "OldFnkS sft", "OldFnkS hrd",
                "OldFnkS rim1", "OldFnkS rim2", "OldFnkS flm", "OldFnkS buz", "OldFnkS stk1",
                "OldFnkS stk2", "Latin S sft", "Latin S med", "Latin S hrd1", "Latin S hrd2",
                "Latin S rim1", "Latin S rim2", "Latin S flm", "Latin S buz", "Latin S stk1",
                "Latin S stk2", "HiPiccSsft1L", "HiPiccSsft1R", "HiPiccSsft2L", "HiPiccSsft2R",
                "HiPiccShrd1L", "HiPiccShrd1R", "HiPiccShrd2L", "HiPiccShrd2R", "HiPiccSrim1L",
                "HiPiccSrim1R", "HiPiccSrim2L", "HiPiccSrim2R", "HiPiccSrim3L", "HiPiccSrim3R",
                "HiPiccSrim4L", "HiPiccSrim4R", "HiPiccSflm L", "HiPiccSflm R", "HiPiccSbuz1L",
                "HiPiccSbuz1R", "HiPiccSbuz2L", "HiPiccSbuz2R", "HiPiccSstk1L", "HiPiccSstk1R",
                "HiPiccSstk2L", "HiPiccSstk2R", "Tight Ssft L", "Tight Ssft R", "Tight Smed L",
                "Tight Smed R", "Tight Shrd1L", "Tight Shrd1R", "Tight Shrd2L", "Tight Shrd2R",
                "Tight Srim1L", "Tight Srim1R", "Tight Srim2L", "Tight Srim2R", "Tight Sflm1L",
                "Tight Sflm1R", "Tight Sflm2L", "Tight Sflm2R", "StdRk1Ssft L", "StdRk1Ssft R",
                "StdRk1Smed L", "StdRk1Smed R", "StdRk1Shrd1L", "StdRk1Shrd1R", "StdRk1Shrd2L",
                "StdRk1Shrd2R", "StdRk1Srim L", "StdRk1Srim R", "StdRk1Sflm L", "StdRk1Sflm R",
                "StdRk1Sbuz1L", "StdRk1Sbuz1R", "StdRk1Sbuz2L", "StdRk1Sbuz2R", "StdRk2Ssft L",
                "StdRk2Ssft R", "StdRk2Smed L", "StdRk2Smed R", "StdRk2Shrd L", "StdRk2Shrd R",
                "StdRk2Srim1L", "StdRk2Srim1R", "StdRk2Srim2L", "StdRk2Srim2R", "StdRk2Sflm L",
                "StdRk2Sflm R", "StdRk2Sbuz1L", "StdRk2Sbuz1R", "StdRk2Sbuz2L", "StdRk2Sbuz2R",
                "StdRk2Sstk L", "StdRk2Sstk R", "OhBabySsft L", "OhBabySsft R", "OhBabySmed1L",
                "OhBabySmed1R", "OhBabySmed2L", "OhBabySmed2R", "OhBabyShrd1L", "OhBabyShrd1R",
                "OhBabyShrd2L", "OhBabyShrd2R", "OhBabySrim L", "OhBabySrim R", "OhBabySflm L",
                "OhBabySflm R", "OhBabySbuz1L", "OhBabySbuz1R", "OhBabySbuz2L", "OhBabySbuz2R",
                "OhBabySstk1L", "OhBabySstk1R", "OhBabySstk2L", "OhBabySstk2R", "PoppinSsft L",
                "PoppinSsft R", "PoppinSmed L", "PoppinSmed R", "PoppinShrd L", "PoppinShrd R",
                "PoppinSrim1L", "PoppinSrim1R", "PoppinSrim2L", "PoppinSrim2R", "PoppinSflm L",
                "PoppinSflm R", "PoppinSbuz1L", "PoppinSbuz1R", "PoppinSbuz2L", "PoppinSbuz2R",
                "PoppinSstk1L", "PoppinSstk1R", "PoppinSstk2L", "PoppinSstk2R", "StdFatSsft L",
                "StdFatSsft R", "StdFatSmed1L", "StdFatSmed1R", "StdFatSmed2L", "StdFatSmed2R",
                "StdFatShrd1L", "StdFatShrd1R", "StdFatShrd2L", "StdFatShrd2R", "StdFatSrim1L",
                "StdFatSrim1R", "StdFatSrim2L", "StdFatSrim2R", "StdFatSflm L", "StdFatSflm R",
                "StdFatSbuz1L", "StdFatSbuz1R", "StdFatSbuz2L", "StdFatSbuz2R", "StdFatSstk1L",
                "StdFatSstk1R", "StdFatSstk2L", "StdFatSstk2R", "JazStkSsft L", "JazStkSsft R",
                "JazStkSmed1L", "JazStkSmed1R", "JazStkSmed2L", "JazStkSmed2R", "JazStkShrd1L",
                "JazStkShrd1R", "JazStkShrd2L", "JazStkShrd2R", "JazStkSrim L", "JazStkSrim R",
                "JazStkSflm L", "JazStkSflm R", "JazStkSbuz1L", "JazStkSbuz1R", "JazStkSbuz2L",
                "JazStkSbuz2R", "JazStkSstk1L", "JazStkSstk1R", "JazStkSstk2L", "JazStkSstk2R",
                "StdJazSsft L", "StdJazSsft R", "StdJazSmed1L", "StdJazSmed1R", "StdJazSmed2L",
                "StdJazSmed2R", "StdJazShrd L", "StdJazShrd R", "StdJazSrim L", "StdJazSrim R",
                "StdJazSflm L", "StdJazSflm R", "StdJazSbuz1L", "StdJazSbuz1R", "StdJazSbuz2L",
                "StdJazSbuz2R", "StdJazSstk1L", "StdJazSstk1R", "StdJazSstk2L", "StdJazSstk2R",
                "BalladSsft L", "BalladSsft R", "BalladSmed1L", "BalladSmed1R", "BalladSmed2L",
                "BalladSmed2R", "BalladShrd L", "BalladShrd R", "BalladSrim1L", "BalladSrim1R",
                "BalladSrim2L", "BalladSrim2R", "BalladSflm L", "BalladSflm R", "BalladSbuz1L",
                "BalladSbuz1R", "BalladSbuz2L", "BalladSbuz2R", "BalladSstk1L", "BalladSstk1R",
                "BalladSstk2L", "BalladSstk2R", "PwrBldSsft1L", "PwrBldSsft1R", "PwrBldSsft2L",
                "PwrBldSsft2R", "PwrBldSmed1L", "PwrBldSmed1R", "PwrBldSmed2L", "PwrBldSmed2R",
                "PwrBldShrd1L", "PwrBldShrd1R", "PwrBldShrd2L", "PwrBldShrd2R", "PwrBldSflm L",
                "PwrBldSflm R", "PwrBldSbuz1L", "PwrBldSbuz1R", "PwrBldSbuz2L", "PwrBldSbuz2R",
                "PwrBldSstk1L", "PwrBldSstk1R", "PwrBldSstk2L", "PwrBldSstk2R", "AlBeefSsft L",
                "AlBeefSsft R", "AlBeefShrd L", "AlBeefShrd R", "AlBeefSrim1L", "AlBeefSrim1R",
                "AlBeefSrim2L", "AlBeefSrim2R", "AlBeefSflm L", "AlBeefSflm R", "AlBeefSbuz1L",
                "AlBeefSbuz1R", "AlBeefSbuz2L", "AlBeefSbuz2R", "AlBeefSstk1L", "AlBeefSstk1R",
                "AlBeefSstk2L", "AlBeefSstk2R", "BigRckSsft L", "BigRckSsft R", "BigRckSmed L",
                "BigRckSmed R", "BigRckShrd L", "BigRckShrd R", "BigRckSflm1L", "BigRckSflm1R",
                "BigRckSflm2L", "BigRckSflm2R", "BigRckSbuz L", "BigRckSbuz R", "BigRckSstk L",
                "BigRckSstk R", "70BrshS tap1", "70BrshS tap2", "70BrshS tap3", "70BrshS tap4",
                "70BrshSswsh1", "70BrshSswsh2", "70BrshSswsh3", "70BrshSsweep", "JzBrshStap1L",
                "JzBrshStap1R", "JzBrshStap2L", "JzBrshStap2R", "JzBrshStap3L", "JzBrshStap3R",
                "JzBrshStap4L", "JzBrshStap4R", "JzBrshSsws1L", "JzBrshSsws1R", "JzBrshSsws2L",
                "JzBrshSsws2R", "JzBrshSsws3L", "JzBrshSsws3R", "JzBrshSswepL", "JzBrshSswepR",
                "70PhunqStk L", "70PhunqStk R", "PwrAmb Stk1L", "PwrAmb Stk1R", "PwrAmb Stk2L",
                "PwrAmb Stk2R", "Latin TMenuL", "Latin TMenuR", "Tight TMenuL", "Tight TMenuR",
                "JazStkTMenuL", "JazStkTMenuR", "OldFnkTMenu", "OhBabyTMenuL", "OhBabyTMenuR",
                "PwrBldTMenuL", "PwrBldTMenuR", "BigRckTMenuL", "BigRckTMenuR", "OldBrsTMenu",
                "Latin T1sftL", "Latin T1sftR", "Latin T1medL", "Latin T1medR", "Latin T1hrdL",
                "Latin T1hrdR", "Latin T1flmL", "Latin T1flmR", "Latin T2sftL", "Latin T2sftR",
                "Latin T2medL", "Latin T2medR", "Latin T2hrdL", "Latin T2hrdR", "Latin T2flmL",
                "Latin T2flmR", "Latin T3sftL", "Latin T3sftR", "Latin T3medL", "Latin T3medR",
                "Latin T3hrdL", "Latin T3hrdR", "Latin T3flmL", "Latin T3flmR", "Latin T4sftL",
                "Latin T4sftR", "Latin T4medL", "Latin T4medR", "Latin T4hrdL", "Latin T4hrdR",
                "Latin T4flmL", "Latin T4flmR", "Tight T1sftL", "Tight T1sftR", "Tight T1hrdL",
                "Tight T1hrdR", "Tight T1flmL", "Tight T1flmR", "Tight T2sftL", "Tight T2sftR",
                "Tight T2hrdL", "Tight T2hrdR", "Tight T2flmL", "Tight T2flmR", "Tight T3sftL",
                "Tight T3sftR", "Tight T3hrdL", "Tight T3hrdR", "Tight T3flmL", "Tight T3flmR",
                "Tight T4sftL", "Tight T4sftR", "Tight T4hrdL", "Tight T4hrdR", "Tight T4flmL",
                "Tight T4flmR", "JazStkT1sftL", "JazStkT1sftR", "JazStkT1medL", "JazStkT1medR",
                "JazStkT1hrdL", "JazStkT1hrdR", "JazStkT1flmL", "JazStkT1flmR", "JazStkT2sftL",
                "JazStkT2sftR", "JazStkT2medL", "JazStkT2medR", "JazStkT2hrdL", "JazStkT2hrdR",
                "JazStkT2flmL", "JazStkT2flmR", "JazStkT3sftL", "JazStkT3sftR", "JazStkT3medL",
                "JazStkT3medR", "JazStkT3hrdL", "JazStkT3hrdR", "JazStkT3flmL", "JazStkT3flmR",
                "OldFnkT1sft", "OldFnkT1hrd", "OldFnkT2sft", "OldFnkT2hrd", "OldFnkT3sft",
                "OldFnkT3hrd", "OhBabyT1sftL", "OhBabyT1sftR", "OhBabyT1hrdL", "OhBabyT1hrdR",
                "OhBabyT1flmL", "OhBabyT1flmR", "OhBabyT2sftL", "OhBabyT2sftR", "OhBabyT2hrdL",
                "OhBabyT2hrdR", "OhBabyT2flmL", "OhBabyT2flmR", "OhBabyT3sftL", "OhBabyT3sftR",
                "OhBabyT3hrdL", "OhBabyT3hrdR", "OhBabyT3flmL", "OhBabyT3flmR", "OhBabyT4sftL",
                "OhBabyT4sftR", "OhBabyT4hrdL", "OhBabyT4hrdR", "OhBabyT4flmL", "OhBabyT4flmR",
                "PwrBldT1sftL", "PwrBldT1sftR", "PwrBldT1hrdL", "PwrBldT1hrdR", "PwrBldT1flmL",
                "PwrBldT1flmR", "PwrBldT2sftL", "PwrBldT2sftR", "PwrBldT2hrdL", "PwrBldT2hrdR",
                "PwrBldT2flmL", "PwrBldT2flmR", "PwrBldT3sftL", "PwrBldT3sftR", "PwrBldT3hrdL",
                "PwrBldT3hrdR", "PwrBldT3flmL", "PwrBldT3flmR", "PwrBldT4sftL", "PwrBldT4sftR",
                "PwrBldT4hrdL", "PwrBldT4hrdR", "PwrBldT4flmL", "PwrBldT4flmR", "BigRckT1sftL",
                "BigRckT1sftR", "BigRckT1medL", "BigRckT1medR", "BigRckT1hrdL", "BigRckT1hrdR",
                "BigRckT1flmL", "BigRckT1flmR", "BigRckT2sftL", "BigRckT2sftR", "BigRckT2medL",
                "BigRckT2medR", "BigRckT2hrdL", "BigRckT2hrdR", "BigRckT2flmL", "BigRckT2flmR",
                "BigRckT3sftL", "BigRckT3sftR", "BigRckT3medL", "BigRckT3medR", "BigRckT3hrdL",
                "BigRckT3hrdR", "BigRckT3flmL", "BigRckT3flmR", "OldBrsT1sft", "OldBrsT1med",
                "OldBrsT1hrd", "OldBrsT2sft", "OldBrsT2med", "OldBrsT2hrd", "OhBabyHMenu",
                "JazStkHMenu", "Latin HMenu", "Disco HMenu", "OldFnkHMenu", "StreetHMenuL",
                "StreetHMenuR", "BigRckHMenuL", "BigRckHMenuR", "OldBrsHMenu", "OhBabyH cl1",
                "OhBabyH cl2", "OhBabyH cl3", "OhBabyH cl4", "OhBabyH cl5", "OhBabyH cl6",
                "OhBabyHhcl1", "OhBabyHhcl2", "OhBabyHhcl3", "OhBabyHhop", "OhBabyH op",
                "OhBabyHpdl1", "OhBabyHpdl2", "JazStkH cl1", "JazStkH cl2", "JazStkHhcl1",
                "JazStkHhcl2", "JazStkHhop", "JazStkH op", "JazStkHpdl", "Latin H cl1",
                "Latin H cl2", "Latin Hhcl", "Latin Hhop", "Latin H op", "Latin Hpdl",
                "Disco H cl1", "Disco H cl2", "Disco Hhcl", "Disco H op", "Disco Hpdl",
                "OldFnkH cl1", "OldFnkH cl2", "OldFnkHhcl", "OldFnkHhop", "OldFnkH op",
                "OldFnkHpdl", "StreetH cl1L", "StreetH cl1R", "StreetH cl2L", "StreetH cl2R",
                "StreetHhcl L", "StreetHhcl R", "StreetHhop L", "StreetHhop R", "StreetH op L",
                "StreetH op R", "StreetHpdl L", "StreetHpdl R", "BigRckH cl1L", "BigRckH cl1R",
                "BigRckH cl2L", "BigRckH cl2R", "BigRckHhcl L", "BigRckHhcl R", "BigRckHhop L",
                "BigRckHhop R", "BigRckH op L", "BigRckH op R", "OldBrsH cl1", "OldBrsH cl2",
                "OldBrsH cl3", "OldBrsH cl4", "OldBrsHhop", "OldBrsH op", "OldBrsHpdl", "Crash MenuL",
                "Crash MenuR", "Ride MenuL", "Ride MenuR", "OthrCymMenuL", "OthrCymMenuR", "StreetCr L",
                "StreetCr R", "BalladCrSftL", "BalladCrSftR", "BalladCrHrdL", "BalladCrHrdR", "BigRckCrSftL",
                "BigRckCrSftR", "BigRckCrHrdL", "BigRckCrHrdR", "JazStkCrSftL", "JazStkCrSftR", "JazStkCrHrdL",
                "JazStkCrHrdR", "OldBrsh Cr L", "OldBrsh Cr R", "Latin RdSftL", "Latin RdSftR", "Latin RdMedL",
                "Latin RdMedR", "Latin RdHrdL", "Latin RdHrdR", "Latin RdBelL", "Latin RdBelR", "BalladRdSftL",
                "BalladRdSftR", "BalladRdHrdL", "BalladRdHrdR", "BalladRdBelL", "BalladRdBelR", "BigRckRdSftL",
                "BigRckRdSftR", "BigRckRdHrdL", "BigRckRdHrdR", "BigRckRdBelL", "BigRckRdBelR", "JazStkRd L",
                "JazStkRd R", "JazStkRdBelL", "JazStkRdBelR", "SizzleRd L", "SizzleRd R", "SizzleRdBelL",
                "SizzleRdBelR", "OldBrsRd L", "OldBrsRd R", "OldBrsRdBelL", "OldBrsRdBelR", "Light SplshL",
                "Light SplshR", "BalladSplshL", "BalladSplshR", "YoMamaChinaL", "YoMamaChinaR", });
            srxWaveNames.Add(new String[] { // SRX-02
                "Off",  "Grand1 L p A", "Grand1 R p A", "Grand1 L p B", "Grand1 R p B", "Grand1 L p C",
                "Grand1 R p C", "Grand2 L p A", "Grand2 R p A", "Grand2 L p B", "Grand2 R p B",
                "Grand2 L p C", "Grand2 R p C", "Grand1 L mpA", "Grand1 R mpA", "Grand1 L mpB",
                "Grand1 R mpB", "Grand1 L mpC", "Grand1 R mpC", "Grand2 L mpA", "Grand2 R mpA",
                "Grand2 L mpB", "Grand2 R mpB", "Grand2 L mpC", "Grand2 R mpC", "Grand1 L f A",
                "Grand1 R f A", "Grand1 L f B", "Grand1 R f B", "Grand1 L f C", "Grand1 R f C",
                "Grand2 L f A", "Grand2 R f A", "Grand2 L f B", "Grand2 R f B", "Grand2 L f C",
                "Grand2 R f C", "Grand1 L ffA", "Grand1 R ffA", "Grand1 L ffB", "Grand1 R ffB",
                "Grand1 L ffC", "Grand1 R ffC", "Grand2 L ffA", "Grand2 R ffA", "Grand2 L ffB",
                "Grand2 R ffB", "Grand2 L ffC", "Grand2 R ffC",  });
            srxWaveNames.Add(new String[] { // SRX-03
                "Off",  "AcPiano3pA L", "AcPiano3pA R", "AcPiano3pB L", "AcPiano3pB R", "AcPiano3pC L", "AcPiano3pC R", "AcPiano3fA L", "AcPiano3fA R", "AcPiano3fB L", "AcPiano3fB R",
                "AcPiano3fC L", "AcPiano3fC R", "Dyn EP 2 p A", "Dyn EP 2 p B", "Dyn EP 2 p C", "Dyn EP 2 f A", "Dyn EP 2 f B", "Dyn EP 2 f C", "BrightB3 A L", "BrightB3 A R",
                "BrightB3 B L", "BrightB3 B R", "BrightB3 C L", "BrightB3 C R", "NylonGt4 p A", "NylonGt4 p B", "NylonGt4 p C", "NylonGt4 f A", "NylonGt4 f B", "NylonGt4 f C",
                "NylnGt4Sld A", "NylnGt4Sld B", "NylnGt4Sld C", "NylnGt4Noiz1", "NylnGt4Noiz2", "FlamncGt p A", "FlamncGt p B", "FlamncGt p C", "FlamncGt f A",
                "FlamncGt f B", "FlamncGt f C", "FlamncGtRoll", "12StrGt A L", "12StrGt A R", "12StrGt B L", "12StrGt B R", "12StrGt C L", "12StrGt C R", "335Gt p A", "335Gt p B",
                "335Gt p C", "335Gt f A", "335Gt f B", "335Gt f C", "JazzGt2 p A", "JazzGt2 p B", "JazzGt2 p C", "JazzGt2 f A",
                "JazzGt2 f B", "JazzGt2 f C", "JzGt3HrmncsA", "JzGt3HrmncsB", "JzGt3HrmncsC", "JzGt3OctaveA", "JzGt3OctaveB", "JzGt3OctaveC", "JzGt3OctSldA", "JzGt3OctSldB",
                "JzGt3OctSldC", "CleanGt2 A", "CleanGt2 B", "CleanGt2 C", "CleanGt3 A", "CleanGt3 B", "CleanGt3 C", "CrunchGt A", "CrunchGt B",
                "CrunchGt C", "6StrBs2 p A", "6StrBs2 p B", "6StrBs2 p C", "6StrBs2 f A", "6StrBs2 f B", "6StrBs2 f C", "6StrBs2ThumA", "6StrBs2ThumB", "6StrBs2ThumC",
                "6StrBs2PullA", "6StrBs2PullB", "6StrBs2PullC", "JazzBs3 p A", "JazzBs3 p B", "JazzBs3 p C", "JazzBs3 f A", "JazzBs3 f B", "JazzBs3 f C",
                "JzBs3PckMt A", "JzBs3PckMt B", "JzBs3PckMt C", "JzBs3Pops A", "JzBs3Pops B", "JzBs3Pops C", "JzBs3Noise1", "JzBs3Noise2", "AcBass2 p A", "AcBass2 p B",
                "AcBass2 p C", "AcBass2 f A", "AcBass2 f B", "AcBass2 f C", "AcBass2Noiz1", "AcBass2Noiz2", "AcBass2Noiz3", "Gt/BsNz Menu", "Flute Vib3 A",
                "Flute Vib3 B", "Flute Vib3 C", "Alto Flute A", "Alto Flute B", "Alto Flute C", "Afro Flute A", "Afro Flute B", "Afro Flute C", "Vox Flute A", "Vox Flute B",
                "Vox Flute C", "Flute Atk A", "Flute Atk B", "Flute Atk C", "Pan Pipe 2 A", "Pan Pipe 2 B", "Pan Pipe 2 C", "PanPipeBrthA", "PanPipeBrthB",
                "PanPipeBrthC", "Moceno A", "Moceno B", "Moceno C", "AltoSax2 p A", "AltoSax2 p B", "AltoSax2 p C", "AltoSax2 f A", "AltoSax2 f B", "AltoSax2 f C", "TenrSax2 p A",
                "TenrSax2 p B", "TenrSax2 p C", "TenrSax2 f A", "TenrSax2 f B", "TenrSax2 f C", "BariSax2 p A", "BariSax2 p B", "BariSax2 p C",
                "BariSax2 f A", "BariSax2 f B", "BariSax2 f C", "WindsFX Menu", "Perc.Flute 1", "Perc.Flute 2", "Perc.Flute 3", "Perc.Flute 4", "Perc.Flute 5", "Perc.Flute 6",
                "Perc.Flute 7", "AltFlOvrTone", "AltoSax EFX", "StudioTp p A", "StudioTp p B", "StudioTp p C", "StudioTp f A", "StudioTp f B", "StudioTp f C",
                "TpHrmnMt p A", "TpHrmnMt p B", "TpHrmnMt p C", "TpHrmnMt f A", "TpHrmnMt f B", "TpHrmnMt f C", "S-BoreTb p A", "S-BoreTb p B", "S-BoreTb p C", "S-BoreTb f A",
                "S-BoreTb f B", "S-BoreTb f C", "Brs2K mf A L", "Brs2K mf A R", "Brs2K mf B L", "Brs2K mf B R", "Brs2K mf C L", "Brs2K mf C R", "Brs2K ff A L",
                "Brs2K ff A R", "Brs2K ff B L", "Brs2K ff B R", "Brs2K ff C L", "Brs2K ff C R", "SoftPad2 A L", "SoftPad2 A R", "SoftPad2 B L", "SoftPad2 B R", "SoftPad2 C L",
                "SoftPad2 C R", "MG Stringz A", "MG Stringz B", "MG Stringz C", "OB Stringz A", "OB Stringz B", "OB Stringz C", "KidsChoirA L", "KidsChoirA R",
                "KidsChoirB L", "KidsChoirB R", "KidsChoirC L", "KidsChoirC R", "M+F Ahs A L", "M+F Ahs A R", "M+F Ahs B L", "M+F Ahs B R", "M+F Ahs C L", "M+F Ahs C R",
                "JazzOoo A", "JazzOoo B", "JazzOoo C", "SteelDrm3p A", "SteelDrm3p B", "SteelDrm3p C", "SteelDrm3f A", "SteelDrm3f B", "SteelDrm3f C",
                "Kick Menu L", "Kick Menu R", "Stdio1K sftL", "Stdio1K sftR", "Stdio1K medL", "Stdio1K medR", "Stdio1K hrdL", "Stdio1K hrdR", "Stdio2K sftL", "Stdio2K sftR",
                "Stdio2K medL", "Stdio2K medR", "Stdio2K hrdL", "Stdio2K hrdR", "Stdio1SMenuL", "Stdio1SMenuR", "Stdio2SMenuL", "Stdio2SMenuR", "Stdio3SMenuL",
                "Stdio3SMenuR", "Stdio4SMenuL", "Stdio4SMenuR", "CymbalSMenuL", "CymbalSMenuR", "Stdio1Ssft1L", "Stdio1Ssft1R", "Stdio1Ssft2L", "Stdio1Ssft2R", "Stdio1Smed1L",
                "Stdio1Smed1R", "Stdio1Smed2L", "Stdio1Smed2R", "Stdio1Shrd1L", "Stdio1Shrd1R", "Stdio1Shrd2L", "Stdio1Shrd2R", "Stdio1Srim1L", "Stdio1Srim1R",
                "Stdio1Srim2L", "Stdio1Srim2R", "Stdio1Sbuz1L", "Stdio1Sbuz1R", "Stdio1Sbuz2L", "Stdio1Sbuz2R", "Stdio1Sflm L", "Stdio1Sflm R", "Stdio1Sstk1L", "Stdio1Sstk1R",
                "Stdio1Sstk2L", "Stdio1Sstk2R", "Stdio2Ssft1L", "Stdio2Ssft1R", "Stdio2Ssft2L", "Stdio2Ssft2R", "Stdio2Smed1L", "Stdio2Smed1R", "Stdio2Smed2L",
                "Stdio2Smed2R", "Stdio2Shrd1L", "Stdio2Shrd1R", "Stdio2Shrd2L", "Stdio2Shrd2R", "Stdio2Srim1L", "Stdio2Srim1R", "Stdio2Srim2L", "Stdio2Srim2R", "Stdio2Sbuz1L",
                "Stdio2Sbuz1R", "Stdio2Sbuz2L", "Stdio2Sbuz2R", "Stdio2Sflm L", "Stdio2Sflm R", "Stdio2Sstk1L", "Stdio2Sstk1R", "Stdio2Sstk2L", "Stdio2Sstk2R",
                "Stdio3Ssft1L", "Stdio3Ssft1R", "Stdio3Ssft2L", "Stdio3Ssft2R", "Stdio3Smed1L", "Stdio3Smed1R", "Stdio3Smed2L", "Stdio3Smed2R", "Stdio3Shrd1L", "Stdio3Shrd1R",
                "Stdio3Shrd2L", "Stdio3Shrd2R", "Stdio3Srim1L", "Stdio3Srim1R", "Stdio3Srim2L", "Stdio3Srim2R", "Stdio3Sbuz1L", "Stdio3Sbuz1R", "Stdio3Sbuz2L",
                "Stdio3Sbuz2R", "Stdio3Sflm L", "Stdio3Sflm R", "Stdio3Sstk1L", "Stdio3Sstk1R", "Stdio3Sstk2L", "Stdio3Sstk2R", "Stdio4Ssft1L", "Stdio4Ssft1R", "Stdio4Ssft2L",
                "Stdio4Ssft2R", "Stdio4Smed1L", "Stdio4Smed1R", "Stdio4Smed2L", "Stdio4Smed2R", "Stdio4Shrd1L", "Stdio4Shrd1R", "Stdio4Shrd2L", "Stdio4Shrd2R",
                "Stdio4Srim1L", "Stdio4Srim1R", "Stdio4Srim2L", "Stdio4Srim2R", "Stdio4Sbuz1L", "Stdio4Sbuz1R", "Stdio4Sbuz2L", "Stdio4Sbuz2R", "Stdio4Sflm L", "Stdio4Sflm R",
                "Sn w/Cym 1 L", "Sn w/Cym 1 R", "Sn w/Cym 2 L", "Sn w/Cym 2 R", "Sn w/Cym 3 L", "Sn w/Cym 3 R", "Sn w/Cym 4 L", "Sn w/Cym 4 R", "StdioT MenuL",
                "StdioT MenuR", "StdioT1sft L", "StdioT1sft R", "StdioT1med L", "StdioT1med R", "StdioT1hrd L", "StdioT1hrd R", "StdioT1flm1L", "StdioT1flm1R", "StdioT1flm2L",
                "StdioT1flm2R", "StdioT2sft L", "StdioT2sft R", "StdioT2med L", "StdioT2med R", "StdioT2hrd L", "StdioT2hrd R", "StdioT2flm1L", "StdioT2flm1R",
                "StdioT2flm2L", "StdioT2flm2R", "StdioT3sft L", "StdioT3sft R", "StdioT3med L", "StdioT3med R", "StdioT3hrd L", "StdioT3hrd R", "StdioT3flm1L", "StdioT3flm1R",
                "StdioT3flm2L", "StdioT3flm2R", "StdioT4sft L", "StdioT4sft R", "StdioT4med L", "StdioT4med R", "StdioT4hrd L", "StdioT4hrd R", "StdioT4flm1L",
                "StdioT4flm1R", "StdioT4flm2L", "StdioT4flm2R", "StdioH MenuL", "StdioH MenuR", "StdioH cl1 L", "StdioH cl1 R", "StdioH cl2 L", "StdioH cl2 R", "StdioH cl3 L",
                "StdioH cl3 R", "StdioH cl4 L", "StdioH cl4 R", "StdioH cl5 L", "StdioH cl5 R", "StdioH hcl L", "StdioH hcl R", "StdioH op1 L", "StdioH op1 R",
                "StdioH op2 L", "StdioH op2 R", "StdioH op3 L", "StdioH op3 R", "StdioH pdl1L", "StdioH pdl1R", "StdioH pdl2L", "StdioH pdl2R", "Conga2 MenuL", "Conga2 MenuR",
                "Cng2LoMtSftL", "Cng2LoMtSftR", "Cng2LoMtHrdL", "Cng2LoMtHrdR", "Cng2LoOpSftL", "Cng2LoOpSftR", "Cng2LoOpHrdL", "Cng2LoOpHrdR", "Cng2LoRoll L",
                "Cng2LoRoll R", "Cng2HiMtSftL", "Cng2HiMtSftR", "Cng2HiMtHrdL", "Cng2HiMtHrdR", "Cng2HiOpSftL", "Cng2HiOpSftR", "Cng2HiOpHrdL", "Cng2HiOpHrdR", "Cng2SlClSftL",
                "Cng2SlClSftR", "Cng2SlClHrdL", "Cng2SlClHrdR", "Cng2SlOpSftL", "Cng2SlOpSftR", "Cng2SlOpHrdL", "Cng2SlOpHrdR", "Cng2HiFlam L", "Cng2HiFlam R",
                "Cng2HiRoll L", "Cng2HiRoll R", "Bngo4LoMenuL", "Bngo4LoMenuR", "Bg4LFngrSftL", "Bg4LFngrSftR", "Bg4LFngrHrdL", "Bg4LFngrHrdR", "Bg4LFngsSftL", "Bg4LFngsSftR",
                "Bg4LFngsHrdL", "Bg4LFngsHrdR", "Bg4LMuteSftL", "Bg4LMuteSftR", "Bg4LMuteHrdL", "Bg4LMuteHrdR", "Bg4LSlapSftL", "Bg4LSlapSftR", "Bg4LSlapHrdL",
                "Bg4LSlapHrdR", "Bg4LOpenSftL", "Bg4LOpenSftR", "Bg4LOpenHrdL", "Bg4LOpenHrdR", "Bngo4HiMenuL", "Bngo4HiMenuR", "Bg4HFgMtSftL", "Bg4HFgMtSftR", "Bg4HFgMtHrdL",
                "Bg4HFgMtHrdR", "Bg4HFgOpSftL", "Bg4HFgOpSftR", "Bg4HFgOpHrdL", "Bg4HFgOpHrdR", "Bg4HFngsSftL", "Bg4HFngsSftR", "Bg4HFngsHrdL", "Bg4HFngsHrdR",
                "Bg4H Heel L", "Bg4H Heel R", "Bg4HSlapSftL", "Bg4HSlapSftR", "Bg4HSlapHrdL", "Bg4HSlapHrdR", "Bg4HOpenSftL", "Bg4HOpenSftR", "Bg4HOpenHrdL", "Bg4HOpenHrdR",
                "Bg4H Flam L", "Bg4H Flam R", "Bg4H Roll L", "Bg4H Roll R", "Timbals3Menu", "Timbl3HiHead", "Timbl3HiRim", "Timbl3LoHead", "Timbl3LoRim",
                "Timbl3HPaila", "Timb3lLPaila", "Synth Saw 2", "Syn Saw 2inv", "Synth Square", "Synth Pulse1", "Triangle", "Sine" });
            srxWaveNames.Add(new String[] { // SRX-04
                "Off", "F.Str mp A L", "F.Str mp A R", "F.Str mp B L", "F.Str mp B R", "F.Str mp C L", "F.Str mp C R",
                "F.Str mp + L", "F.Str mp + R", "Mrct A L", "Mrct A R", "Mrct B L", "Mrct B R", "Mrct C L",
                "Mrct C R", "Mrct + L", "Mrct + R", "Spcct A L", "Spcct A R", "Spcct B L", "Spcct B R",
                "Spcct C L", "Spcct C R", "Trem mp A L", "Trem mp A R", "Trem mp B L", "Trem mp B R", "Trem mp C L",
                "Trem mp C R", "Trem mp + L", "Trem mp + R", "Vn mp A L", "Vn mp A R", "Vn mp B L", "Vn mp B R",
                "Vn mp C L", "Vn mp C R", "Vn mp + L", "Vn mp + R", "Va mp A L", "Va mp A R", "Va mp B L",
                "Va mp B R", "Va mp C L", "Va mp C R", "Va mp + L", "Va mp + R", "Vc mp A L", "Vc mp A R",
                "Vc mp B L", "Vc mp B R", "Vc mp C L", "Vc mp C R", "Vc mp + L", "Vc mp + R", "Cb mp A L",
                "Cb mp A R", "Cb mp B L", "Cb mp B R", "Cb mp C L", "Cb mp C R", "Cb mp + L", "Cb mp + R",
                "Vn mltV A L", "Vn mltV A R", "Vn mltV B L", "Vn mltV B R", "Vn mltV C L", "Vn mltV C R", "Vn mltV + L",
                "Vn mltV + R", "Va mltV A L", "Va mltV A R", "Va mltV B L", "Va mltV B R", "Va mltV C L", "Va mltV C R",
                "Va mltV + L", "Va mltV + R", "Vc mltV A L", "Vc mltV A R", "Vc mltV B L", "Vc mltV B R", "Vc mltV C L",
                "Vc mltV C R", "Vc mltV + L", "Vc mltV + R", "Cb f A L", "Cb f A R", "Cb f B L", "Cb f B R",
                "Cb f C L", "Cb f C R", "Cb f + L", "Cb f + R", "Vn mltV^ A L", "Vn mltV^ A R", "Vn mltV^ B L",
                "Vn mltV^ B R", "Vn mltV^ C L", "Vn mltV^ C R", "Vn mltV^ + L", "Vn mltV^ + R", "Va mltV^ A L", "Va mltV^ A R",
                "Va mltV^ B L", "Va mltV^ B R", "Va mltV^ C L", "Va mltV^ C R", "Va mltV^ + L", "Va mltV^ + R", "Vc mltV^ A L",
                "Vc mltV^ A R", "Vc mltV^ B L", "Vc mltV^ B R", "Vc mltV^ C L", "Vc mltV^ C R", "Vc mltV^ + L", "Vc mltV^ + R",
                "Vn Stc A L", "Vn Stc A R", "Vn Stc B L", "Vn Stc B R", "Vn Stc C L", "Vn Stc C R", "Va Stc A L",
                "Va Stc A R", "Va Stc B L", "Va Stc B R", "Va Stc C L", "Va Stc C R", "Vc Stc A L", "Vc Stc A R",
                "Vc Stc B L", "Vc Stc B R", "Vc Stc C L", "Vc Stc C R", "Cb Stc A L", "Cb Stc A R", "Cb Stc B L",
                "Cb Stc B R", "Cb Stc C L", "Cb Stc C R", "All Stc L", "All Stc R", "Vn Pz mf A L", "Vn Pz mf A R",
                "Vn Pz mf B L", "Vn Pz mf B R", "Vn Pz mf C L", "Vn Pz mf C R", "Va Pz mf A L", "Va Pz mf A R", "Va Pz mf B L",
                "Va Pz mf B R", "Va Pz mf C L", "Va Pz mf C R", "Vc Pz mf A L", "Vc Pz mf A R", "Vc Pz mf B L", "Vc Pz mf B R",
                "Vc Pz mf C L", "Vc Pz mf C R", "Cb Pz mf A L", "Cb Pz mf A R", "Cb Pz mf B L", "Cb Pz mf B R", "Cb Pz mf C L",
                "Cb Pz mf C R", "All Pz mf L", "All Pz mf R", "Vn Pz ff A L", "Vn Pz ff A R", "Vn Pz ff B L", "Vn Pz ff B R",
                "Vn Pz ff C L", "Vn Pz ff C R", "Va Pz ff A L", "Va Pz ff A R", "Va Pz ff B L", "Va Pz ff B R", "Va Pz ff C L",
                "Va Pz ff C R", "Vc Pz ff A L", "Vc Pz ff A R", "Vc Pz ff B L", "Vc Pz ff B R", "Vc Pz ff C L", "Vc Pz ff C R",
                "Cb Pz ff A L", "Cb Pz ff A R", "Cb Pz ff B L", "Cb Pz ff B R", "Cb Pz ff C L", "Cb Pz ff C R", "All Pz ff L",
                "All Pz ff R",  });
            srxWaveNames.Add(new String[] { // SRX-05
                "Off", "PhraseMenuL", "PhraseMenuR", "080:BladeBtL", "080:BladeBtR", "093:R&Bees", "096:RugBurnL",
                "096:RugBurnR", "120:BossClbL", "120:BossClbR", "120:ClubbinL", "120:ClubbinR", "135:XRacer L", "135:XRacer R",
                "Drain-O L", "Drain-O R", "RocketBrs L", "RocketBrs R", "Resound L", "Resound R", "StrikeWhstle",
                "Blowout L", "Blowout R", "MGSAW HQ", "P5SAW HQ", "106 Saw HQ", "SH101SAW HQ", "TB303SAW HQ",
                "CustomSawA2", "JU2SpSaw2", "JU2SpSaw3", "JU2SpSaw4", "OB2 Pat7", "JPSuperSaw", "JP8SQU HQ",
                "MGSQU HQ", "P5SQU HQ", "CustomSquA2", "TB303SQU HQ", "106 SubOsc", "JU2SpSub1", "JU2SpSub2",
                "JP8PLS05 HQ", "JP8PLS15 HQ", "JP8PLS20 HQ", "JP8PLS30 HQ", "JP8PLS40 HQ", "JP8PLS45 HQ", "SyncPlsInv",
                "JUNO Pls Bd", "JUNO Pls Rls", "JP6 DynPLS P", "JP6 DynPLS N", "JUNO106 Rave", "MGTRI HQ", "JP8TRI HQ",
                "Variant Sin", "ARP SIN", "CalcSIN2", "106 Bs56", "Spct11", "Sprk vx2", "Spct2 20",
                "MG WhteNz HQ", "MG PinkNz HQ", "Spect Noise", "JD PnoEQ", "EP Chd Menu", "EP Maj 9th", "EP Maj 11th",
                "EP Min 11th", "FM Clavi mf", "Old Hous Org", "FM Prc Organ", "Fing E.Bs mf", "Fing E.Bs ff", "Baby Bass f2",
                "Baby Bassff2", "GuitarrnWarm", "GuitarrnOct2", "HouseBass mf", "HouseBass ff", "Brs Stab L", "Brs Stab R",
                "Brs Fall L", "Brs Fall R", "WindsFX Menu", "Flute Fx 1", "Flute Fx 2", "Flute Fx 3", "Sax Fx 1",
                "Sax Fx 2", "Sax Fx 3", "Gregorian2 L", "Gregorian2 R", "Dance Str L", "Dance Str R", "D50Pizzagogo",
                "Str Fall L", "Str Fall R", "StrScaleMajL", "StrScaleMajR", "StrChd MenuL", "StrChd MenuR", "StrChdMin9L",
                "StrChdMin9R", "StrChdMn11L", "StrChdMn11R", "StrSprdMaj L", "StrSprdMaj R", "StrSprdMin L", "StrSprdMin R",
                "E.Gtr Menu L", "E.Gtr Menu R", "ShredFuzzGtr", "ClnGtrRif1 L", "ClnGtrRif1 R", "ClnGtrRif2 L", "ClnGtrRif2 R",
                "TrmDwnGtr1 L", "TrmDwnGtr1 R", "TrmDwnGtr2 L", "TrmDwnGtr2 R", "WahGtrChord", "WahGtrCt1 L", "WahGtrCt1 R",
                "WahGtrCt2 L", "WahGtrCt2 R", "WahGtrCt3 L", "WahGtrCt3 R", "WahGtrCt4 L", "WahGtrCt4 R", "WahGtrCt5 L",
                "WahGtrCt5 R", "WahGtrCt6 L", "WahGtrCt6 R", "WahGtrRif1 L", "WahGtrRif1 R", "WahGtrRif2 L", "WahGtrRif2 R",
                "Hit SpecialL", "Hit SpecialR", "Hit OneShotL", "Hit OneShotR", "BrassHit 1", "BrassHit 2", "BrassHit 3",
                "BrassHit 4", "BrassHit 5", "BrassHit 6 L", "BrassHit 6 R", "BrassHit 7 L", "BrassHit 7 R", "BrassHit 8",
                "BrassHit 9 L", "BrassHit 9 R", "BrassHit 10L", "BrassHit 10R", "BrassHit 11L", "BrassHit 11R", "BrassHit 12L",
                "BrassHit 12R", "BrassHit 13L", "BrassHit 13R", "BrassHit 14L", "BrassHit 14R", "BrassHit 15", "BrassHit 16L",
                "BrassHit 16R", "BrassHit 17L", "BrassHit 17R", "BrassHit 18", "BrassHit 19L", "BrassHit 19R", "BrassHit 20",
                "BrassHit 21L", "BrassHit 21R", "BrassHit 22L", "BrassHit 22R", "BrassHit 23L", "BrassHit 23R", "BrassHit 24",
                "SymphHit 1 L", "SymphHit 1 R", "SymphHit 2 L", "SymphHit 2 R", "SymphHit 3 L", "SymphHit 3 R", "SymphHit 4 L",
                "SymphHit 4 R", "SymphHit 5 L", "SymphHit 5 R", "SymphHit 6 L", "SymphHit 6 R", "SymphHit 7 L", "SymphHit 7 R",
                "Organ Hit 1", "OrgTrnsplntL", "OrgTrnsplntR", "Organ Hit 2L", "Organ Hit 2R", "Chord Hit 1", "Chord Hit 2L",
                "Chord Hit 2R", "Chord Hit 3L", "Chord Hit 3R", "Chord Hit 4", "Altr Hit L", "Altr Hit R", "Crystal HitL",
                "Crystal HitR", "Dirty Hit 2", "Exhaust HitL", "Exhaust HitR", "Scoop Hit L", "Scoop Hit R", "Stamp Hit L",
                "Stamp Hit R", "ThousandHitL", "ThousandHitR", "9th Hit L", "9th Hit R", "Slap Hit 1 L", "Slap Hit 1 R",
                "Slap Hit 2 L", "Slap Hit 2 R", "Slap Hit 3 L", "Slap Hit 3 R", "Dust Hit L", "Dust Hit R", "Dist Hit 1 L",
                "Dist Hit 1 R", "Dist Hit 2 L", "Dist Hit 2 R", "Slide Hit L", "Slide Hit R", "Wah Hit", "Slide Hit 2L",
                "Slide Hit 2R", "Tremble Hit", "Sweep Hit", "ParabolaHitL", "ParabolaHitR", "Daw Hit L", "Daw Hit R",
                "Dah Hit L", "Dah Hit R", "Aah Hit", "Mixed Hit L", "Mixed Hit R", "Showy Hit", "Pretty Hit L",
                "Pretty Hit R", "Smoky Hit L", "Smoky Hit R", "Fist Hit", "Iron Hit 2", "Clavy Hit L", "Clavy Hit R",
                "Warp Hit 1L", "Warp Hit 1R", "Warp Hit 2L", "Warp Hit 2R", "Warp Hit 3L", "Warp Hit 3R", "Warp Hit 4",
                "Tread Hit", "Old Org Hit", "Sand Hit", "Bounce Hit1", "Bounce Hit2L", "Bounce Hit2R", "Bounce Hit3",
                "Warp Hit 5L", "Warp Hit 5R", "Bounce Hit4", "Scratch Menu", "Scratch 4", "Scratch 5", "Scratch 6",
                "Scratch 7", "Scratch 8", "Scratch 9", "Scratch 10", "Scratch 11", "Scratch 12", "Scratch 13",
                "Scratch 14", "Scratch 15", "Vinyl NzLp 1", "Vinyl NzLp 2", "Vox Menu", "Whoo! M", "You’re DJ M",
                "Groove M", "Com’on M", "Yeah M", "Wow Yeah F", "Yeah F", "Ho F", "Aow F",
                "Oh F", "R&B Kick 1", "LoBit Kick 1", "LoBit Kick 2", "Old Kick 1", "Old Kick 2", "Metal Kick",
                "Old Kick 3", "Rebo Kick 1", "Abuz Kick", "Cool Kick 1", "Dyna Kick", "Floor Kick", "Gan Kick",
                "GoGo Kick", "Livn Kick", "Old Kick 4", "Mellow Kick", "Two Kick", "Gummo Kick 1", "Gummo Kick 2",
                "R&B Kick 2", "Narrow Kick", "Alley Kick 1", "Alley Kick 2", "FunkB Kick", "Light Kick 1", "Light Kick 2",
                "Light Kick 3", "L-RiderKick1", "Light Kick 4", "Light Kick 5", "Light Kick 6", "H-Rider Kick", "Light Kick 7",
                "L-RiderKick2", "Light Kick 8", "ChickenKick1", "Edge Kick", "Buck Kick", "Layer Kick 1", "Layer Kick 2",
                "Layer Kick 3", "Layer Kick 4", "Retro Kick 1", "Ambi Kick", "Cbell Kick", "Tamb Kick", "Retro Kick 2",
                "Pump Kick", "Heavy Kick 1", "Heavy Kick 2", "Heavy Kick 3", "Heavy Kick 4", "Layer Kick 5", "Shake Kick",
                "Rebo Kick 2", "Chop Kick", "Cool Kick 2", "Light Kick 9", "Seiz Kick", "Soul Kick", "Trip Kick",
                "Lo-Tek Kick", "Filter Kick", "Vinyl Kick", "Blade Kick", "Club Kick", "Dinky Kick 1", "Dinky Kick 2",
                "Dinky Kick 3", "Dinky Kick 4", "Old Kick 5", "Hybrid Kick3", "Slopey Kick1", "Junk Kick", "707 DryKick1",
                "707 DryKick2", "808 DryKick1", "808 DryKick2", "808 DryKick3", "808 DryKick4", "808 DryKick5", "Analog Kick1",
                "Analog Kick2", "909 DryKick1", "909 DryKick2", "909 DryKick3", "Craze Kick 1", "Craze Kick 2", "106 Kick 1",
                "106 Kick 2", "MG Kick 1", "MG Kick 2", "Fat Kick", "Croak Kick 1", "Fuzz Kick", "ChickenKick2",
                "Motor Kick", "Craze Kick 3", "Craze Kick 4", "Layer Kick 6", "Plump Kick", "Deviant Kick", "Dance Kick 5",
                "Dance Kick 6", "Dance Kick 7", "Dance Kick 8", "BigDrds Kick", "Slopey Kick2", "Swanky Kick", "SnugBugaKick",
                "Synth Kick", "DeeBee Kick", "HipHop SD 1", "R&B SD 1", "Lo-Bit SD 1", "Blade SD", "Basis SD 1",
                "Gruntoid SD", "L-Rider SD", "Craze SD 1", "R&B SD 2", "Lo-Bit SD 2", "Metalic SD", "Noisy SD",
                "Pump SD", "Dinky SD 1", "Dinky SD 2", "Dinky SD 3", "Basis SD 2", "Basis SD 3", "Basis SD 4",
                "Retro SD 1", "HiTune SD", "Snapy SD", "Thin SD", "Thin SD mb", "Slim SD", "Rocky SD1",
                "Rocky SD1 mb", "Rocky SD2 mb", "Rocky SD3 mb", "Club SD1", "Club SD2 mb", "Club SD3", "Club SD3 mb",
                "Comp SD1 mb", "Comp SD2 mb", "Rev SD mb", "LoBtHf SD1mb", "LoBtHf SD2mb", "LoBtHf SD3mb", "LoBtHf SD4mb",
                "LoBtHf SD5mb", "LoBtHf SD6mb", "Light SD1", "Light SD1 mb", "Light SD2", "Light SD2 mb", "Alley SD1",
                "Alley SD1mb", "BigDreds SD", "H-Rider SD", "PFunkSD mb", "Slopey SD", "Gan SD mb", "GoGo SD mb",
                "MellowSD mb", "Metal SD mb", "Old SD1 mb", "Old SD2 mb", "Old SD3 mb", "Old SD4 mb", "Old SD5 mb",
                "Attack SD1", "Retro SD1", "Retro SD2 mb", "Retro SD3", "Retro SD4", "Retro SD5", "Retro SD6",
                "Hybrid SD mb", "DeeBee SD", "Old SD6 mb", "Layer SD1 mb", "Layer SD2 mb", "Filter SD1", "Filter SD2",
                "Attack SD2", "Attack SD3", "Attack SD4", "Lo-Tek SD", "Chicken SD", "Filter SD3", "Filter SD4",
                "Retro SD7", "Retro SD8", "Ultra SD 1", "Ultra SD 2", "Funky SD", "Layer SD3", "Layer SD4 mb",
                "Alley SD2", "Alley SD2 mb", "Fella SD1 mb", "Fella SD2", "Fella SD2 mb", "Rebo SD1", "Rebo SD1 mb",
                "Rebo SD2", "Swush SD mb", "Heat SD mb", "Layer SD5 mb", "Cool SD", "GoGo SD", "Soul SD mb",
                "Layer SD6", "Funk SD1", "Funk SD2", "Dinky SD4 mb", "Layer SD7 mb", "Layer SD8 mb", "Layer SD9 mb",
                "Vynil SD mb", "FilterSD5 mb", "Junk SD mb", "707 Dry SD1", "707 Dry SD2", "808 Dry SD1", "808 Dry SD2",
                "808 Dry SD3", "909 Dry SD1", "909 Dry SD2", "909 Dry SD3", "909 Dry SD4", "909 Dry SD5", "909 SD&CLP",
                "106 SD1", "106 SD2", "Clappy SD", "Cranky SD", "Grim SD", "Sparkle SD", "SparkleSD mb",
                "Hush SD mb", "R&B Crs Stk1", "R&B Crs Stk2", "Lo-Bit CStk", "707 Dry RSht", "808 Dry RSht", "909 Dry RSht",
                "Basis Rim", "Basis Rim mb", "Deviant RM1", "Devi RM1mb", "Deviant RM2", "Deviant RM3", "Devi RM3mb",
                "Hop RimShot1", "Hop RimShot2", "Hop RimShot3", "Old HTom A1", "Old HTom A2", "Old HTom A3", "Old LTom A1",
                "Old LTom A2", "Old HTom B1", "Old HTom B2", "Old HTom B3", "Old LTom B1", "Old LTom B2", "Retro Tom1",
                "Retro Tom2", "Sim5 Tom1S", "Sim5 Tom1F", "Sim5 Tom2S", "Sim5 Tom2F", "Sim5 Tom3S", "Sim5 Tom3F",
                "Sim5 Tom4S", "Sim5 Tom4F", "707 Dry HTom", "707 Dry MTom", "707 Dry LTom", "808 Dry HTom", "808 Dry MTom",
                "808 Dry LTom", "909 Dry HTom", "909 Dry MTom", "909 Dry LTom", "Lo-Tek HH1", "Lo-Tek HH2", "Lo-Tek HH3",
                "Lo-Tek HH4", "Lo-Bit CHH", "Dry CHH 1", "Dry CHH 2", "Dry CHH 3", "Comp CHH 1", "Comp CHH 2",
                "Comp CHH 3", "Lo-Fi CHH 1", "Lo-Fi CHH 2", "Lo-Fi CHH 3", "Timba CHH", "Chill CHH", "Mosaic CHH 1",
                "Mosaic CHH 2", "Mosaic CHH 3", "UltrafunkCH1", "UltrafunkCH2", "Miami CHH", "Lulu CHH", "707 Dry CHH",
                "808 Dry CHH", "808 Cl&OpHH", "909 Dry CHH1", "909 Dry CHH2", "909 Dry CHH3", "Miami OHH", "Time OHH",
                "UltrafunkOHH", "Jiggle OHH", "Lo-Bit OHH", "Dry OHH", "Comp OHH", "Lo-Fi OHH", "707 Dry OHH",
                "808 Dry OHH1", "808 Dry OHH2", "808 Dry OHH3", "909 Dry OHH1", "909 Dry OHH2", "Single HClp1", "Single HClp2",
                "Single HClp3", "Real Clap L", "Real Clap R", "Lofi Clap1mb", "Lofi Clap2mb", "Lofi Clap3mb", "Buck Clap",
                "Head Knd Clp", "Royal Clap", "Clap Tail 2", "GospelClap L", "GospelClap R", "707 Dry Clap", "808 Dry Clap",
                "909 Dry Clap", "Pop Crash1", "Pop Crash2", "Pop SprshCym", "Pop ChinaCym", "Pop Cym Roll", "707 Dry CR",
                "909 Dry CR 1", "909 Dry CR 2", "909 Dry CR 3", "PopRideCym 1", "PopRideCym 2", "PopRideCym 3", "PopRideCym 4",
                "707 Dry RD", "808 Dry Cym1", "808 Dry Cym2", "808 Dry Cym3", "808 Dry Cym4", "909 Dry RD1", "909 Dry RD2",
                "909 Dry RD3", "MG Nz Cym", "US Nz Cym", "WindChime Up", "WindChime Dn", "Cwbl Lo", "Cwbl Hi",
                "Cwbl Mute", "707 Dry Cwbl", "808 Dry Cwbl", "808 Dry Clvs", "Clvs Hi", "Clvs Lo", "HiBongo Opn",
                "HiBongo Mfld", "HiBongo 2", "HiBongo Slp1", "HiBongo Slp2", "HiBongo Mute", "LoBongo Opn", "LoBongo Slap",
                "808 Dry HCng", "808 Dry MCng", "808 Dry LCng", "LTBit CgH L", "LTBit CgH R", "LTBit CgL L", "LTBit CgL R",
                "HiConga Opn", "HiConga Slap", "HiConga Cls", "LoConga Opn", "Tmbl Hi p", "Tmbl Hi f", "Tmbl Hi Rim",
                "Tmbl Hi Flm", "Tmbl Hi Roll", "Tmbl HiPaila", "Tmbl Lo p", "Tmbl Lo Flm", "Latin Hit", "Chekele 1",
                "Chekele 2", "Real Shaker1", "Real Shaker2", "Real Shaker3", "Real Cabasa3", "808 Dry Mrcs", "Short Guiro2",
                "Long Guiro2", "Long Tamb", "707 Dry Tamb", "Long Triangl", "Short FgSnap", "St. FgSnap L", "St. FgSnap R",
                "Bobs Slide", "Cave Hit L", "Cave Hit R", "Warp Hit 6", "Thru Hit 1", "Thru Hit 2", "Mull Hit",
                "Warp Hit 7", "Warp Hit 8", "Electronica", "Ripper L", "Ripper R", "Warp Hit 9", "Tickle Hit",
                "PercolateHit", "Tramp Hit L", "Tramp Hit R", "LoFi Moan", "Sonar Hit", "Electric Saw", "Cartn Fall L",
                "Cartn Fall R", "Cartn Boing1", "Cartn Boing2", "LoTek Bit L", "LoTek Bit R", "LoFi Hole", "LoFi Coo",
                "LoFi Bit 1", "LoFi Bit 2", "Nz Pass L", "Nz Pass R", "Thumpkin L", "Thumpkin R", "Dist Slide L",
                "Dist Slide R", "Howl Sync", "Slipper Hit", "Spoon Hit", "Pod Hit", "LoFi Shaker", "Box Hit 1",
                "Hard Shaker", "LoFi Pod Hit", "Press Hit", "Swish Hit", "Rumble Hit", "Cym Slide", "Knock Hit 1",
                "Knock Hit 2", "Swish Rope 1", "Swish Rope 2", "Swish Rope 3", "Swish Rope 4", "Metal Atk Lo", "Metal Atk Hi",
                "Fl Press Hit", "Dump Hit", "Vice Hit 1", "Vice Hit 2", "Vice Hit 3", "Box Hit 2", "US Metal 1",
                "US Metal 2", "MG NoiseHit", "SH2 S Zap 1", "SH2 S Zap 2", "SH2 S Zap 3", "SH2 S Zap 4", "SH2 S Zap 5",
                "MG S Zap 1", "MG S Zap 2", "MG S Zap 3", "SH2 U Zap 1", "SH2 U Zap 2", "SH2 U Zap 3", "SH2 U Zap 4",
                "SH2 U Zap 5", "SH2 U Zap 6", "Ring FX 1", "Ring FX 2", "OSC Perc 1", "OSC Perc 2", "US SweepD L1",
                "US SweepD S1", "US SweepD L2", "US SweepD S2", "US SweepD L3", "MG SweepDn", "US SweepU L1", "US SweepU L2", });
            srxWaveNames.Add(new String[] { // SRX-06
                "Off", "Strings L pA", "Strings R pA", "Strings L pB", "Strings R pB", "Strings L pC", "Strings R pC",
                "Strs L pA Lp", "Strs R pA Lp", "Strs L pB Lp", "Strs R pB Lp", "Strs L pC Lp", "Strs R pC Lp", "Strings L fA",
                "Strings R fA", "Strings L fB", "Strings R fB", "Strings L fC", "Strings R fC", "Strs L fA Lp", "Strs R fA Lp",
                "Strs L fB Lp", "Strs R fB Lp", "Strs L fC Lp", "Strs R fC Lp", "Multi STR A", "Multi STR B", "Multi STR C",
                "Multi STR Lp", "Vl Sect A", "Vl Sect B", "Vl Sect C", "Vl Sect Lp", "Va Sect A", "Va Sect B",
                "Va Sect C", "Va Sect Lp", "Vc Sect A", "Vc Sect B", "Vc Sect C", "Vc Sect Lp", "Cb Sect",
                "Cb Sect Lp", "Vl Solo A", "Vl Solo B", "Vl Solo C", "Va Solo A", "Va Solo B", "Va Solo C",
                "Vc Solo A", "Vc Solo B", "Vc Solo C", "Cb Solo", "Violin&Cello", "Multi Solo 1", "Multi Solo 2",
                "VlSolo Vib A", "VlSolo Vib B", "VlSolo Vib C", "VcSolo Vib A", "VcSolo Vib B", "VcSolo Vib C", "Vls Spicc A",
                "Vls Spicc B", "Vls Spicc C", "Vas Spicc A", "Vas Spicc B", "Vas Spicc C", "Vcs Spicc A", "Vcs Spicc B",
                "Vcs Spicc C", "Cbs Spicc A", "Cbs Spicc B", "Cbs Spicc C", "Multi Spicc", "VlSolo Spicc", "VlSlSpicc2 A",
                "VlSlSpicc2 B", "VlSlSpicc2 C", "VcSolo Spicc", "MultSl Spicc", "Pizzicato 1", "Pizzicato 2", "STR Attack",
                "StrAttack 2A", "StrAttack 2B", "StrAttack 2C", "OrchUnisonLA", "OrchUnisonRA", "OrchUnisonLB", "OrchUnisonRB",
                "OrchUnisonLC", "OrchUnisonRC", "Full Orch.", "Full Orch Lp", "JV Strings L", "JV Strings R", "JV Strings A",
                "JP Strings1A", "JP Strings2A", "Soft Pad A", "Piccolo", "Piccolo 2 A", "Piccolo 2 B", "Piccolo 2 C",
                "Flute 1A", "FluteVib2 A", "FluteVib2 B", "FluteVib2 C", "Celtic Flt A", "Celtic Flt B", "Celtic Flt C",
                "ORG_Flute A", "ORG_Flute B", "ORG_Flute C", "Clarinet", "Clarinet 2 A", "Clarinet 2 B", "Clarinet 2 C",
                "Multi Cla", "Bs Clarinet", "Oboe 1", "Oboe 1A", "Oboe 1B", "Oboe 1C", "Oboe 2A",
                "Oboe 2B", "Oboe 2C", "Oboe 3 A", "Oboe 3 B", "Oboe 3 C", "Eng.Horn A", "Eng.Horn B",
                "Eng.Horn C", "Eng.Horn 2 A", "Eng.Horn 2 B", "Eng.Horn 2 C", "Bassoon", "Bassoon 1", "Bassoon 2 A",
                "Bassoon 2 B", "Bassoon 2 C", "Bassoon&Oboe", "S_Recorder A", "S_Recorder B", "S_Recorder C", "T_Recorder A",
                "T_Recorder B", "T_Recorder C", "Tnr.Recorder", "TinWhistle A", "TinWhistle B", "TinWhistle C", "TinWhisOrn A",
                "TinWhisOrn B", "TinWhisOrn C", "Whistle 1", "Multi Sax 1", "Multi Sax 2", "BRS Ensemble", "BRS Ens Lp",
                "Brass ff", "ff Brass Lp", "Trumpet 1A", "Trumpet 2", "Trumpet 3 A", "Trumpet 3 B", "Trumpet 3 C",
                "Cornet", "Tpt Sect. A", "Flugelhorn", "HarmonMute2A", "HarmonMute2B", "HarmonMute2C", "Solo Tb A",
                "Solo Tb B", "Solo Tb C", "Trombone 1", "Trombone 2 A", "Trombone 2 B", "Trombone 2 C", "Trombone mfA",
                "Trombone mfB", "Trombone mfC", "Trombone f A", "Trombone f B", "Trombone f C", "Bass Tb", "Tb Sect",
                "Tb Sect Lp", "Multi Reed", "Uill.Pipe1 A", "Uill.Pipe1 B", "Uill.Pipe1 C", "Uill.Pipe2 A", "Uill.Pipe2 B",
                "Uill.Pipe2 C", "U.Pipe Orn A", "U.Pipe Orn B", "U.Pipe Orn C", "F.Horn Solo", "F.HornSolo2A", "F.HornSolo2B",
                "F.HornSolo2C", "F.Horn Sect1", "F.Horn Sect2", "F.Hrn Sc1 Lp", "F.Hrn Sc2 Lp", "French 1A", "F.Horns2 pA",
                "F.Horns2 pB", "F.Horns2 pC", "F.Horns2 pA+", "F.Horns2 pB+", "F.Horns2 pC+", "F.Horns2 fA", "F.Horns2 fB",
                "F.Horns2 fC", "F.Horns2 fA+", "F.Horns2 fB+", "F.Horns2 fC+", "F.Horn Mute", "F.Hrn MuteLp", "Tuba",
                "Tuba 1", "Harp A", "Harp B", "Harp C", "Harp 2 A", "Harp 2 B", "Harp 2 C",
                "ClarsahHarpA", "ClarsahHarpB", "ClarsahHarpC", "Harp Maj Up", "Harp Maj Dn", "Harp m7 Up", "Harp m7 Dwn",
                "Harp 9th Up", "Harp 9th Dwn", "Harp +7 Up", "Harp +7 Dwn", "Harp b9 Up", "Harp b9 Dwn", "Harp m7 UpLp",
                "Harp m7 DnLp", "Harp 9thUpLp", "Harp 9thDnLp", "Harp +7 UpLp", "Harp +7 DnLp", "Harp b9 UpLp", "Harp b9 DnLp",
                "EuroPiano pA", "EuroPiano pB", "EuroPiano pC", "EuroPiano fA", "EuroPiano fB", "EuroPiano fC", "Ac Piano1 A",
                "Piano Thump", "Piano Up TH", "Harpsichord", "HPS_Front A", "HPS_Front B", "HPS_Front C", "HPS_Back A",
                "HPS_Back B", "HPS_Back C", "HPS_Lute A", "HPS_Lute B", "HPS_Lute C", "HPS_Click", "Celesta A",
                "Celesta B", "Celesta C", "MusicBox 2 A", "MusicBox 2 B", "MusicBox 2 C", "Bass Marimba", "Xylophone",
                "Glockenspiel", "Glocken 2 A", "Glocken 2 B", "Glocken 2 C", "TubularBells", "Tubular 2 A", "Tubular 2 B",
                "Tubular 2 C", "Wind Chime 2", "Wind Chime 3", "Church Bells", "ChurchBell 2", "JingleBell 2", "Sleigh Bell",
                "Bousouki A", "Bousouki B", "Bousouki C", "OrchPrc Hit", "OrchestraHit", "Orch Hit f", "Orch Hit Maj",
                "Orch Hit Min", "Orch Hit Dim", "OrchHit2 Maj", "OrchHit2 Min", "OrchHit2 Dim", "Gliss Maj", "Gliss Min",
                "Gliss Maj Lp", "Gliss Min Lp", "Tremolo p", "Tremolo f", "Tremolo sfz", "Tremolo p Lp", "Tremolo f Lp",
                "Tremolo sfLp", "Cluster", "Cluster Lp", "Pentatonic", "PentatonicLp", "Staccato p", "Staccato f",
                "Brass Stacc", "Brass Fall", "Tps Fall", "F.Horn Rip", "Choir A", "Choir B", "Choir C",
                "F.Chor Aah A", "F.Chor Aah B", "F.Chor Aah C", "F.Chor Mmh A", "F.Chor Mmh B", "F.Chor Mmh C", "LargeChrL pA",
                "LargeChrR pA", "LargeChrL pB", "LargeChrR pB", "LargeChrL pC", "LargeChrR pC", "LargeChrL fA", "LargeChrR fA",
                "LargeChrL fB", "LargeChrR fB", "LargeChrL fC", "LargeChrR fC", "Boys Choir A", "Boys Choir B", "Boys Choir C",
                "Gregorian LA", "Gregorian RA", "Gregorian LB", "Gregorian RB", "Gregorian LC", "Gregorian RC", "Sop Chr Mono",
                "SopranoChr L", "SopranoChr R", "Tenor Solo", "Jazz Doo L A", "Jazz Doo R A", "Jazz Doo L B", "Jazz Doo R B",
                "Jazz Doo L C", "Jazz Doo R C", "Jazz Doot LA", "Jazz Doot RA", "Jazz Doot LB", "Jazz Doot RB", "Jazz Doot LC",
                "Jazz Doot RC", "Jazz Dat L A", "Jazz Dat R A", "Jazz Dat L B", "Jazz Dat R B", "Jazz Dat L C", "Jazz Dat R C",
                "Jazz Bap L A", "Jazz Bap R A", "Jazz Bap L B", "Jazz Bap R B", "Jazz Bap L C", "Jazz Bap R C", "Dow fall L A",
                "Dow fall R A", "Dow fall L B", "Dow fall R B", "Dow fall L C", "Dow fall R C", "Bass Thum A", "Bass Thum B",
                "Bass Thum C", "Vocal Menu L", "Vocal Menu R", "BoysAleluiaL", "BoysAleluiaR", "Boys Amen1 L", "Boys Amen1 R",
                "Boys Amen2 L", "Boys Amen2 R", "Amen L", "Amen R", "Aleluia L", "Aleluia R", "Breath Wind",
                "Breath Atack", "Breath Whisl", "Breath Hrmon", "Whistle Nz", "Flute Breath", "Vocal Breath", "Timpani",
                "Timpani p", "Timpani f", "Timpani 2 p", "Timpani 2 f", "Timpani Roll", "Timp Roll p", "Timp Roll f",
                "Concert SNR1", "Concert SNR2", "Concert SNR3", "Orch SNR", "OrchSNR Roll", "SNR Roll", "SNR Set",
                "Concert BD 1", "Concert BD 2", "Concert BD 3", "Orch BD", "BD Roll", "Tam Tam", "Crash Cymbal",
                "Crash Cym Lp", "Cymbal Hit", "Finger Cym 2", "MalletCymbal", "CymbalScrape", "Gong", "Grongkas",
                "Castanets 1", "Castanets 2", "Tambourine", "Triangle", "Slapstick", "Ratchet", "TunedBlock A",
                "TunedBlock B", "TunedBlock C", "Bodhran MENU", "Bodhran 1", "Bodhran 2", "Bodhran 3", "Bodhran 4",
                "Bodhran 5", "Bodhran 6", "Bodhran 7", "Perc Hit 1", "Perc Hit 2", "Perc Set 1", "Perc Set 2",
                "Perc Set 3", "REV Hrp m7Up", "REV Hrp m7Dn", "REV Hrp9thUp", "REV Hrp9thDn", "REV Hrp +7Up", "REV Hrp +7Dn",
                "REV Hrp b9Up", "REV Hrp b9Dn", "REV Hit f", "REV Hit Maj", "REV Hit Min", "REV Hit Dim", "REV Hit2 Maj",
                "REV Hit2 Min", "REV Hit2 Dim", "REV Stacc p", "REV Stacc f", "REV Bell", "REV Cym Hit", "REV PercHit1",
                "REV PercHit2", "REV BD 1", "REV BD 2", "REV BD 3", "REV BD Roll", "REV SNR 1", "REV SNR 2",
                "REV SNR 3", "REV Tam Tam", "REV Crash", "REV Gong", "REV Tamb", "REV Timp 1", "REV Timp 2",
                "REV Casta 1", "REV Casta 2", "REV S.Stick", "REV Sleigh", "REV VoMenu L", "REV VoMenu R", "REV ByAleluL",
                "REV ByAleluR", "REV ByAmen1L", "REV ByAmen1R", "REV ByAmen2L", "REV ByAmen2R", "REV Amen L", "REV Amen R",
                "REV AleluiaL", "REV AleluiaR", });
            srxWaveNames.Add(new String[] { // SRX-07
                "Off", "StGrand L pA", "StGrand R pA", "StGrand L pB", "StGrand R pB", "StGrand L pC", "StGrand R pC",
                "StGrand L fA", "StGrand R fA", "StGrand L fB", "StGrand R fB", "StGrand L fC", "StGrand R fC", "SApiano 3",
                "E.Grand BdyA", "E.Grand BdyB", "E.Grand BdyC", "E.Grand 1A", "E.Grand 1B", "E.Grand 1C", "Suitcase p A",
                "Suitcase p B", "Suitcase p C", "Suitcase mfA", "Suitcase mfB", "Suitcase mfC", "Suitcase f A", "Suitcase f B",
                "Suitcase f C", "Stage p A", "Stage p B", "Stage p C", "Stage f A", "Stage f B", "Stage f C",
                "70'EP 5 A p", "70'EP 5 B p", "70'EP 5 C p", "70'EP 5 A f", "70'EP 5 B f", "70'EP 5 C f", "70'EP Bs p A",
                "70'EP Bs p B", "70'EP Bs p C", "70'EP Bs f A", "70'EP Bs f B", "70'EP Bs f C", "Wurly 1 p A", "Wurly 1 p B",
                "Wurly 1 p C", "Wurly 1 mf A", "Wurly 1 mf B", "Wurly 1 mf C", "Wurly 1 f A", "Wurly 1 f B", "Wurly 1 f C",
                "Wurly 2 p A", "Wurly 2 p B", "Wurly 2 p C", "Wurly 2 mf A", "Wurly 2 mf B", "Wurly 2 mf C", "Wurly 2 f A",
                "Wurly 2 f B", "Wurly 2 f C", "Pnet p A", "Pnet p B", "Pnet p C", "Pnet f A", "Pnet f B",
                "Pnet f C", "R.Chord A", "R.Chord B", "R.Chord C", "'70s Piano A", "'70s Piano B", "'70s PianoLp",
                "'70s Lute A", "'70s Lute B", "'70s Lute Lp", "Vibraphone A", "Vibraphone B", "Vibraphone C", "Clav Pluck A",
                "Clav Pluck B", "Clav Pluck C", "Ch.Clav A", "Ch.Clav B", "Ch.Clav C", "Clav 1 A", "Clav 1 B",
                "Clav 1 C", "Clav 2 A", "Clav 2 B", "Clav 2 C", "Clav 3 p A", "Clav 3 p B", "Clav 3 p C",
                "Clav 3 f A", "Clav 3 f B", "Clav 3 f C", "Clav 4 p A", "Clav 4 p B", "Clav 4 p C", "Clav 4 mf A",
                "Clav 4 mf B", "Clav 4 mf C", "Clav 4 f A", "Clav 4 f B", "Clav 4 f C", "Clav 5 p A", "Clav 5 p B",
                "Clav 5 p C", "Clav 5 f A", "Clav 5 f B", "Clav 5 f C", "Clav 6 p A", "Clav 6 p B", "Clav 6 p C",
                "Clav 6 f A", "Clav 6 f B", "Clav 6 f C", "Clav 7 A", "Clav 7 B", "Clav 7 C", "Clav Mute 1A",
                "Clav Mute 1B", "Clav Mute 1C", "Clav Mute 2A", "Clav Mute 2B", "Clav Mute 2C", "Clav Attack", "B3 1 A",
                "B3 1 B", "B3 1 C", "B3 2 A", "B3 2 B", "B3 2 C", "B3 2 Ch A", "B3 2 Ch B",
                "B3 2 Ch C", "B3 2 FL A", "B3 2 FL B", "B3 2 FL C", "B3 3 A", "B3 3 B", "B3 3 C",
                "B3 3 FL A", "B3 3 FL B", "B3 3 FL C", "B3 4", "B3 5 A", "B3 5 B", "B3 5 C",
                "B3 6 A", "B3 6 B", "B3 6 C", "B3 7 A", "B3 7 B", "B3 7 C", "B3 7 FL A",
                "B3 7 FL B", "B3 7 FL C", "B3 8 A", "B3 8 B", "B3 8 C", "B3 8 Ch A", "B3 8 Ch B",
                "B3 8 Ch C", "B3 8 FL A", "B3 8 FL B", "B3 8 FL C", "B3 9", "B3 10 A", "B3 10 B",
                "B3 10 C", "B3 10 Ch A", "B3 10 Ch B", "B3 10 Ch C", "B3 10 FL A", "B3 10 FL B", "B3 10 FL C",
                "B3 11", "B3 12", "B3 13", "B3 14 A", "B3 14 B", "B3 14 C", "B3 14 Ch A",
                "B3 14 Ch B", "B3 14 Ch C", "B3 14 FL A", "B3 14 FL B", "B3 14 FL C", "B3 15 A", "B3 15 B",
                "B3 15 C", "B3 15 Ch A", "B3 15 Ch B", "B3 15 Ch C", "B3 15 FL A", "B3 15 FL B", "B3 15 FL C",
                "B3 16", "B3 17", "B3 18", "B3 19 A", "B3 19 B", "B3 19 C", "B3 Perc 1 A",
                "B3 Perc 1 B", "B3 Perc 1 C", "B3 Perc 2 A", "B3 Perc 2 B", "B3 Perc 2 C", "B3 Perc 3 A", "B3 Perc 3 B",
                "B3 Perc 3 C", "B3 Perc3FL A", "B3 Perc3FL B", "B3 Perc3FL C", "B3 Harm 1", "B3 Harm 2 A", "B3 Harm 2 B",
                "B3 Harm 2 C", "B3 Harm 3 A", "B3 Harm 3 B", "B3 Harm 3 C", "B3 Harm3FL A", "B3 Harm3FL B", "B3 Harm3FL C",
                "Power B slwA", "Power B slwB", "Power B slwC", "Power B fstA", "Power B fstB", "Power B fstC", "B3 Click",
                "VX Organ 1 A", "VX Organ 1 B", "VX Organ 1 C", "VX Organ 2 A", "VX Organ 2 B", "VX Organ 2 C", "VX Organ 3 A",
                "VX Organ 3 B", "VX Organ 3 C", "VX Organ 4 A", "VX Organ 4 B", "VX Organ 4 C", "VX Harpsi A", "VX Harpsi B",
                "VX Harpsi C", "VX Org Bs A", "VX Org Bs B", "VX Org Bs C", "Farf Organ 1", "Farf Organ 2", "Farf Organ 3",
                "Farf Organ 4", "Farf Organ 5", "Farf Organ 6", "Farf Organ 7", "Farf Organ 8", "FarfClar16'A", "FarfClar16'B",
                "FarfClar16'C", "FarfFlut8' A", "FarfFlut8' B", "FarfFlut8' C", "Farf 8'+2'", "Farf 8'+4'", "Farf 8'+5th",
                "Farf Noise", "Tron Str A", "Tron Str B", "Tron Str C", "Tron Str2 A", "Tron Str2 B", "Tron Str2 C",
                "Tron Vlns A", "Tron Vlns B", "Tron Vlns C", "Tron Cello A", "Tron Cello B", "Tron Cello C", "Tron Flute A",
                "Tron Flute B", "Tron Flute C", "Tron Choir A", "Tron Choir B", "Tron Choir C", "Tron Cho.1 A", "Tron Cho.1 B",
                "Tron Cho.1 C", "Tron Cho.2 A", "Tron Cho.2 B", "Tron Cho.2 C", "Blow Sax A", "Blow Sax B", "Blow Sax C",
                "T.Sax hrd A", "T.Sax hrd B", "T.Sax hrd C", "Solo Tpt. A", "Solo Tpt. B", "Solo Tpt. C", "R&R Horns A",
                "R&R Horns B", "R&R Horns C", "Flute Vib A", "Flute Vib B", "Flute Vib C", "Clean TC2 pA", "Clean TC2 pB",
                "Clean TC2 pC", "Clean TC2 fA", "Clean TC2 fB", "Clean TC2 fC", "NylonGt2 p A", "NylonGt2 p B", "NylonGt2 p C",
                "NylonGt2 mfA", "NylonGt2 mfB", "NylonGt2 mfC", "NylonGt2 f A", "NylonGt2 f B", "NylonGt2 f C", "D.MuteGt p A",
                "D.MuteGt p B", "D.MuteGt p C", "D.MuteGt mpA", "D.MuteGt mpB", "D.MuteGt mpC", "D.MuteGt mfA", "D.MuteGt mfB",
                "D.MuteGt mfC", "Wah Gtr MENU", "Wah Down 1", "Wah Up 1", "Wah Down 2", "Wah Up 2", "Gtr FX MENU",
                "Gtr Feedback", "Gtr Scrap", "Gtr Slid Nz", "Gtr Cut Nz", "Gtr Slap", "MM Slap Bs A", "MM Slap Bs B",
                "MM Slap Bs C", "MM Pop Bs A", "MM Pop Bs B", "MM Pop Bs C", "MM Frls Bs A", "MM Frls Bs B", "MM Frls Bs C",
                "MM Jazz Bs A", "MM Jazz Bs B", "MM Jazz Bs C", "MM Pick Bs A", "MM Pick Bs B", "MM Pick Bs C", "MM Harm Bs A",
                "MM Harm Bs B", "MM Harm Bs C", "AL Funk Bs A", "AL Funk Bs B", "AL Funk Bs C", "AL Pop Bs A", "AL Pop Bs B",
                "AL Pop Bs C", "AL Frls Bs A", "AL Frls Bs B", "AL Frls Bs C", "AL LatinBs A", "AL LatinBs B", "AL LatinBs C",
                "AL Old Bs A", "AL Old Bs B", "AL Old Bs C", "AL P.Bass A", "AL P.Bass B", "AL P.Bass C", "JP Funk Bs A",
                "JP Funk Bs B", "JP Funk Bs C", "JP Pop Bs A", "JP Pop Bs B", "JP Pop Bs C", "JP Frls Bs A", "JP Frls Bs B",
                "JP Frls Bs C", "JP Pick Bs A", "JP Pick Bs B", "JP Pick Bs C", "JP Rock Bs A", "JP Rock Bs B", "JP Rock Bs C",
                "JP 6StrBs pA", "JP 6StrBs pB", "JP 6StrBs pC", "JP 6StrBs fA", "JP 6StrBs fB", "JP 6StrBs fC", "JP Ac.Bs p A",
                "JP Ac.Bs p B", "JP Ac.Bs p C", "JP Ac.Bs f A", "JP Ac.Bs f B", "JP Ac.Bs f C", "Sld&Nz MENU", "Slides MENU",
                "E.Bs Nz MENU", "A.Bs Nz MENU", "MM Slide 1", "MM Slide 2", "AL Slide", "JP Slide 1", "JP Slide 2",
                "JP Slide 3", "AL Nz 1", "AL Nz 2", "AL Nz 3", "AL Nz 4", "AL Nz 5", "AL Nz 6",
                "AL Nz 7", "AL Nz 8", "AL Nz 9", "JP Frls Nz 1", "JP Frls Nz 2", "JP Frls Nz 3", "JP Frls Nz 4",
                "JP Ac.Nz 1", "JP Ac.Nz 2", "JP Ac.Nz 3", "JP Ac.Nz 4", "OB Bass", "OB Bass Lp A", "OB Bass Lp B",
                "OB Bass Lp C", "MG BsPedal", "MG BsPdl LpA", "MG BsPdl LpC", "MG Fat Bs", "MG Sharp Bs1", "MG Big Bs",
                "MG ClassicBs", "MG Sharp Bs2", "TB-303 Bass", "JP-4 Bass 1", "JP-4 Bass 2", "SH-101 Bs 1", "SH-101 Bs 2",
                "SH-101 Bs 3", "SH-101 Bs 4", "SH-101 Bs 5", "Sys700 Bs 1", "Sys700 Bs 2", "FM Super Bs", "KG Poly Bs",
                "KG Poly BsLp", "Muted Bass A", "Muted Bass B", "Muted Bass C", "JP-8 Saw A", "JP-8 Saw C", "Sys700 Saw",
                "JX-10 Saw", "D-50 Saw 1", "D-50 Saw 2", "SH-5 Saw", "SH-2 Saw", "SH-101 Saw", "SH-1000 Saw",
                "GR-300 Saw 1", "GR-300 Saw 2", "JU-2 Saw", "MG Saw 1A", "MG Saw 1C", "MG Saw 2", "OB Saw 1A",
                "OB Saw 1C", "OB Saw 2", "P5 Saw 1A", "P5 Saw 1C", "2600 Saw", "AP Saw", "OSC Saw",
                "OSC Reso Saw", "KG700 Saw", "KG800 Saw 1", "KG800 Saw 2", "KG MS Saw", "CS Saw 1A", "CS Saw 1C",
                "CS Saw 2", "JP-8 SquareA", "JP-8 SquareC", "JX-10 Square", "SH-5 Square", "SH-2 Square", "MG Square A",
                "MG Square C", "OB Square A", "OB Square C", "P5 Square A", "P5 Square C", "2600 Square", "OSC Square",
                "KG800 Square", "KG MS Square", "CS Square", "JP-8 Pulse 1", "JP-8 Pulse 2", "JP-8 Pulse 3", "JP-8 Pulse 4",
                "JP-8 Pulse 5", "SH-1000 Puls", "MG Pulse 1A", "MG Pulse 1C", "MG Pulse 2A", "MG Pulse 2C", "OB Pulse 1",
                "OB Pulse 2", "OB Pulse 3", "2600 Pulse 1", "2600 Pulse 2", "EM Pulse", "CS Pulse 1", "CS Pulse 2",
                "JU-2 Sub OSC", "MG Ramp", "MG Triangle", "2600Triangle", "2600 Sine", "JP-8 PWM A", "JP-8 PWM B",
                "JP-8 PWM C", "MG Dt.Saw A", "MG Dt.Saw B", "MG Dt.Saw C", "P5 Dt.Saw A", "P5 Dt.Saw B", "P5 Dt.Saw C",
                "MG Dt.Squ A", "MG Dt.Squ B", "MG Dt.Squ C", "Juno Rave A", "Juno Rave B", "Juno Rave C", "Blaster A",
                "Blaster B", "Blaster C", "JP Hollo A", "JP Hollo B", "JP Hollo C", "JP-8 Str A", "JP-8 Str B",
                "JP-8 Str C", "OB Str 1A", "OB Str 1B", "OB Str 1C", "OB Str 2A", "OB Str 2B", "OB Str 2C",
                "AP Str Ens A", "AP Str Ens B", "AP Str Ens C", "Hard 5ths A", "Hard 5ths B", "Hard 5ths C", "OBXP Str A",
                "OBXP Str B", "OBXP Str C", "OBXP Str Lp", "MG Oct A", "MG Oct B", "MG Oct C", "MG Dt.Oct A",
                "MG Dt.Oct B", "MG Dt.Oct C", "OBXP Brass A", "OBXP Brass B", "OBXP Brass C", "OBXP BrassLp", "FM Brass",
                "Waspy", "Waspy Lp", "OB Lead", "OB Lead Lp", "JP-6 SqLead", "JP-6 SqLd Lp", "Blown 1",
                "Blown 2", "PG Sweep 1A", "PG Sweep 1C", "PG Sweep 2A", "PG Sweep 2C", "D-50 HeavenA", "D-50 HeavenB",
                "D-50 HeavenC", "JX-8P Vox", "JX-8P Vox Lp", "VP-330ChoirA", "VP-330ChoirB", "VP-330ChoirC", "P5 Unisync",
                "P5 UnisyncLp", "P5 Dipthong", "P5 DipthngLp", "FM Lead", "KG800 Lead", "MG Lead", "MG Lead Lp",
                "JP-8 Lead", "Digiwave 1", "Digiwave 2", "Digiwave 3", "Frog wave", "SRG FM", "Shimmer wave",
                "VS Organ A", "VS Organ C", "Juno Organ", "Juno OrganLp", "FM Punch", "Mondigital", "MondigitalLp",
                "JP-8 Clavi A", "JP-8 Clavi C", "JP-8 ClaviLp", "Juno Clavi", "P5 X-mod", "Steam Drum", "Kalimba Atk",
                "Additive", "MG Blip", "MG Blip Lp", "MG Thump", "MG Thump Lp", "MG Attack", "MG Attack Lp",
                "VS Bell 1", "VS Bell 2", "JP-6 Bell", "MKS-80 Xmod1", "MKS-80 Xmod2", "MKS-80 Xmod3", "MKS-80 Xmod4",
                "MKS-80 Xmod5", "Record Noise", "MG White Nz", "MG Pink Nz", "SH-5 Pink Nz", "JP-8 X-mod 1", "JP-8 X-mod 2",
                "P5 Noise 1", "P5 Noise 2", "ZZZ loop", "Atmosphere", "FX1A-L(RSS)", "FX1A-R(RSS)", "FX1B-L(RSS)",
                "FX1B-R(RSS)", "FX1C-L(RSS)", "FX1C-R(RSS)", "FX2A-L(RSS)", "FX2A-R(RSS)", "FX2B-L(RSS)", "FX2B-R(RSS)",
                "FX2C-L(RSS)", "FX2C-R(RSS)", "FX3A-L(RSS)", "FX3A-R(RSS)", "FX3B-L(RSS)", "FX3B-R(RSS)", "FX3C-L(RSS)",
                "FX3C-R(RSS)", "DrumGrv MENU 1", "BW Jz 6/8 52", "BW Brush 66", "AL Slam 66", "AL Whack 76", "AL Grunge 84",
                "BW Swamp 90", "BW Latin 102", "BW Fusion112", "AL Funk 120", "AL Shuff.126", "Kik&CymSET 1", "Kik&CymSET 2",
                "Kik&CymSET 3", "Kik&CymSET 4", "Kik&CymSET 5", "Kik&CymSET 6", "Kik&CymSET 7", "Kik&CymSET 8", "Kik&CymSET 9",
                "Kik&CymSET10", "Kik&CymSET11", "Kik&CymSET12", "Kik&CymSET13", "SNR SET 1p", "SNR SET 1f", "SNR SET 2p",
                "SNR SET 2f", "SNR SET 3p", "SNR SET 3f", "SNR SET 4p", "SNR SET 4f", "SNR SET 5p", "SNR SET 5f",
                "SNR SET 6p", "SNR SET 6f", "SNR SET 7p", "SNR SET 7f", "SNR SET 8p", "SNR SET 8f", "SNR SET 9p",
                "SNR SET 9f", "SNR SET 10p", "SNR SET 10f", "SNR SET 11p", "SNR SET 11f", "HAT SET 1p", "HAT SET 1f",
                "HAT SET 1pdl", "HAT SET 2p", "HAT SET 2f", "HAT SET 2pdl", "HAT SET 3", "TOM SET 1p", "TOM SET 1f",
                "TOM SET 2", "TOM SET 3p", "TOM SET 3f", "KIK MENU", "Meat K", "Boomer K1", "Boomer K2",
                "Medium K", "Deep Dry K", "Real Dry K1", "Real Dry K2", "Real Dry K3", "Stomp K", "Comp K1",
                "Comp K2", "Comp K3", "Lo-Fi K", "SNR MENU 1", "SNR MENU 2", "SNR MENU 3", "Ring sft Sn",
                "Ring hrd Sn", "Ring rol Sn", "Ring flm Sn", "SlamRm sftSn", "SlamRm hrdSn", "SlamRm rolSn", "SlamRm flmSn",
                "SlmDry sftSn", "SlmDry hrdSn", "Dry sft Sn", "Dry hrd Sn", "Dry rol Sn", "Dry flm Sn", "RockOn Sn1",
                "RockOn Sn2", "Verb sft Sn", "Verb hrd Sn", "Warm sft Sn", "Warm hrd Sn", "Warm flm Sn", "DryFat sftSn",
                "DryFat hrdSn", "DryFat rolSn", "DryFat flmSn", "Funk Sn1", "Funk Sn2", "Picc. sft Sn", "Picc. mid Sn",
                "Picc. hrd Sn", "Picc. rol Sn", "Picc. flm Sn", "Choke Sn", "LiteVerb Sn", "Live Stick", "NaturalStick",
                "Click Stick", "HAT MENU", "18 Hat pdl", "18 Hat cls 1", "18 Hat cls 2", "18 Hat hlf 1", "18 Hat hlf 2",
                "18 Hat opn 1", "18 Hat opn 2", "16 Hat pdl", "16 Hat cls 1", "16 Hat cls 2", "16 Hat hlf 1", "16 Hat hlf 2",
                "16 Hat opn 1", "16 Hat opn 2", "CR78 Hat cls", "CR78 Hat opn", "TOM MENU", "16 MPL sftTm", "16 MPL hrdTm",
                "13 MPL sftTm", "13 MPL hrdTm", "12 MPL sftTm", "12 MPL hrdTm", "10 MPL sftTm", "10 MPL hrdTm", "Soft Low Tom",
                "Soft Mid Tom", "Soft Hi Tom", "LoVerb sftTm", "LoVerb hrdTm", "HiVerb sftTm", "HiVerb hrdTm", "CYM MENU",
                "Long Crash 1", "Long Crash 2", "Long Ride 1", "Long Ride 2", "LngRide Bell", "Shaker MENU2", "626 Shaker",
                "Shaker 3", "Shaker 4", "Shaker 5", "Sm.Club", "Sm.Club fw", "Sm.Club lp", "REV Waspy",
                "REV P5 X-mod", "REV SteamDrm", "REV Kalimba", "REV Additive", "REV Blip", "REV Thump", "REV Attack",
                "REV Gt Scrap", "REV Gt SldNz", "REV Gt CutNz", "REV Gt Slap", "REV 626Shakr", "REV Shaker 3", "REV Shaker 4",
                "REV Shaker 5", "REV FX1L RSS", "REV FX1R RSS", "REV FX2L RSS", "REV FX2R RSS", "REV FX3L RSS", "REV FX3R RSS", });
            srxWaveNames.Add(new String[] { // SRX-08
                "Off", "Phrase MENU1", "Phrase MENU2", "Phrase MENU3", "61:Slow Grv", "75:BoomRvBel", "76:TrpHpKikn",
                "78:NinjaBrek", "80:Thump Grv", "80:MetalHead", "81:Ringn Kit", "83:Big & Bad", "83:HmBeatBx1", "83:Poing Snr",
                "85:Homie", "85:Snr Wall", "86:Street 16", "86:Bubbles", "88:TrenchTwn", "88:Cut Hats", "88:Yo!",
                "88:Crackl'n", "88:HmBeatBx2", "89:BuzyFunky", "89:Slamn'Grv", "90:Swingn", "90:NoizyBoys", "90:Swingdirt",
                "91:NotopHpHp", "91:Cruis'n", "91:Uptown", "91:BigBeauty", "92:Jazzman", "92:Overring", "92:Boom Town",
                "92:Too Big", "94:Ya Mon", "95:Head Bobn", "95:Pure Phat", "96:PlainJane", "96:Jane 2", "98:Funkster1",
                "98:Funkster2", "104:BigBottm", "104:Respect", "104:Riddim", "108:Gargle", "Phrase MENU4", "120:House 1",
                "120:House 2", "120:House 3", "120:TeknoBNG", "144:Tekno BD", "144:TeknoHAT", "160:Drum'nBs", "184:Gabba",
                "132:Detroit", "132:Agogo", "116:Elect'80", "120:Electro1", "138:Electro2", "DrumLp MENU1", "DrumLp MENU2",
                "125:Basic 1", "125:Basic 2", "125:ClpMania", "125:LowToEnd", "125:LivZone1", "125:LivZone2", "125:LivZone3",
                "125:LivZone4", "125:LivZone5", "125:LpFabric", "125:PuristcL", "125:PuristcR", "125:FullHous", "125:4thFloor",
                "125:Front", "125:Fatbrett", "125:FatTrck1", "125:FatTrck2", "125:BsWorx 1", "125:BsWorx 2", "125:Mid Beat",
                "125:Offbt HH", "Perc.Lp MENU 1", "125:Tamb Man", "125:AccntShk", "125:DoublShk", "125:Ganza", "125:Triangle",
                "125:Conga L", "125:Conga R", "125:ToneCong", "125:StpBongo", "125:FullBrzl", "Pf/Key MENU", "116:HousePf1",
                "116:HousePf2", "116:HousePf3", "116:HousePf4", "116:Pf&Str 1", "116:Pf&Str 2", "116:Pf&Str 3", "125:E-Groove",
                "125:SlowWah1", "125:SlowWah2", "Dry Chord D#", "Dry Chord C#", "Dry Chord F", "House Organ", "SubmarineSin",
                "ShortStrings", "Gtr/Bs MENU", "125:OpenStr1", "125:OpenStr2", "125:WahMute1", "125:WahMute2", "125:BendLick",
                "125:Dry Funk", "JUNO Bass 1", "JUNO Bass Lp", "Moogish", "JUNO Bass 2", "JUNO Bs 2 Lp", "Big Saw Bass",
                "Big SawBs 2", "JUNO Sine Bs", "Octa Bass", "Rave Bass", "FM Pluck Bs", "FM Slide Bs", "Solid Bass",
                "JUNO-60 Bass", "SH-5 Bass", "Dirty Bass", "Sub Bass", "Jungle Bass", "Bass Gliss", "Bass Sustain",
                "Bass Short", "Bass Slide", "Bass Down", "Bass Slap", "Hard Attack", "Square Low", "JUNO 60 Buzz",
                "JUNO Pulse 5", "Funky JUNO", "Mellow JUNO", "JUNO Strings", "JUNO Str Lp", "Lo-Fi RoomTp", "JP8000 Saw 1",
                "JP8000 Saw 2", "JP-6 Saw", "Techno Saw", "TB Saw", "TB Dst Saw", "TB Solid Saw", "TB Reso Saw",
                "SH-1 Square", "TB Square 1", "TB Square 2", "TB Square 3", "Dist Square", "TB Dst Sqr 1", "TB Dst Sqr 2",
                "TB Reso Sqr1", "TB Reso Sqr2", "TB Reso Sqr3", "SH-1 Pulse", "JP8000 PWM", "JP8000 FBK", "260 Sub OSC",
                "Dist Synth", "P5 Pipe", "FM Garage", "JUNO Pluck", "Funky Synth", "JUNO Bowing", "JUNO Synth",
                "JX Synth", "Alpha Wave", "Killer", "Detuned Saw", "Fat JP-6", "Euro Dance", "Noisy 101",
                "Daft Wave", "JP Siren", "Cold Dress", "Pizzy Techno", "Organ Pizz", "Garage Org", "FM Club Org",
                "Org Chord", "Org EP", "Lo-Fi Wurly", "Pure EP", "Ac Piano1 A", "Ac Piano1 B", "Ac Piano1 C",
                "Dist TekGtr1", "Dist TekGtr2", "Dist Gtr Chd", "GTR FX MENU", "Gtr FX MENU", "Wah Gtr Lp 1", "Wah Gtr Lp 2",
                "Wah Gtr A", "Wah Gtr B", "D.GtrUpwhine", "Gtr Thumper", "Hit MENU 1", "Hit MENU 2", "Brass Fall 2",
                "Smear Hit 1", "Smear Hit 2", "LoFi MinorHt", "BrassVox Hit", "Brass Hit Lp", "Mean Brs Hit", "Sax Lick 1",
                "Sax Lick 2", "Sax Cry", "Sax Blast", "Sax Blast Lp", "HipHop Alarm", "Cuica Hit", "Breath Hit",
                "Bauw Hit", "ClassicHseHt", "JUNO Blip 1", "JUNO Blip 2", "Sci-Fi 2", "Slice of Trp", "Rattle Hit",
                "P.Phatt Hit", "BD Scratch", "SD Scratch", "Tape Stop 2", "BasicScratch", "Scratch Loop", "Record Noiz",
                "Hit MENU 3", "Hit MENU 4", "Beam HiQ", "Analog Bird", "ElectronFall", "Retro UFO", "Jungle Beep",
                "PC-2 Machine", "Dr.Beat", "Mental Perc", "May Day Perc", "PC-2 Spacers", "Techno Scene", "Pure Psycho",
                "TAO Hit", "Thin Beef", "Organ Hit 2", "Hit MENU 5", "Hit & Drum", "Stop Hit", "Horny-Hit",
                "Mute Trumpet", "Panic", "INDUST. MENU", "PCM Press", "ElectricDunk", "Thrill", "Drill Hit",
                "MachineShout", "Air Gun", "Emergency", "Buzzer", "Tonality", "EFX Tom Lp", "Hip Hop Ride",
                "P5 Noise 3", "Toy Gun 1", "Toy Gun 2", "GeigerCountr", "GlassBreakLp", "Voice MENU 1", "Voice MENU 2",
                "Voice MENU 3", "Voice MENU 4", "Voice MENU 5", "1,2,3,4", "Kick it!", "Come on!", "Funky!",
                "Crazy!", "DJ!", "Go!", "Stop!", "Good Eve", "Yeah!", "Dirty Laugh",
                "Female Laugh", "Ow!", "Pretty Aou!", "Singing Ahh", "Oh Yeah!", "Hoo!", "Say Yeah!",
                "Scream!", "Baby!", "Get on Up", "MC!", "Give it Up", "Canvas Laugh", "Uuha",
                "Whouw", "Eh", "Oh !", "Ahhhhhh 1", "Ahhhhhh 2", "Breath", "Get Off",
                "Get On 1", "Get Up", "Get On 2", "Dance", "Do It !", "Jam !", "House Music",
                "Come On", "Oh No !", "Oh Yes !", "Alright", "It's MyHouse", "Higher", "Everybody",
                "Ouhouhooohou", "Tic", "Tac", "Tigidi", "Tac Tac", "Auu", "VOCODER MENU",
                "Aah Formant", "Eeh Formant", "Iih Formant", "Ooh Formant", "Uuh Formant", "Dist Ooh Vox", "Talkbox",
                "CricketChoir", "Wild Ways", "Psychedelics", "Taj Mahal", "Ghandi Drone", "Enigmatic", "Cleansing",
                "Frost Byte", "Iceland", "Columns", "Effervescent", "Transolia", "Voyage", "Frontiers",
                "Cruising", "Tunnel Rain", "Rumble Rain", "Bombay", "Black Den", "Husky Chord", "Phantasm",
                "Fumes", "Temptation", "Creepy 1", "Creepy 2", "Elves", "Fireflies", "CrystalWaves",
                "Electroshock", "Snowflakes", "Steel Spires", "Rise Up!", "Sea Whistle", "Lava Flow", "Sparkling",
                "Fuse", "Turntable", "UK Station", "ChinaStation", "Crowding", "Nasalnoise", "Gnashing",
                "Construction", "Construct. 2", "Turbine", "Scraping", "Vacuum Pump", "Sawing", "Air Pump",
                "Soda Water", "CompresStart", "CompresStop", "Jack Hammer", "Propeller", "Neon Hum", "Buzz Hum",
                "Tcashe Hum", "Oil Can Bow", "Crank Shaft", "Snarls", "Feed Me!", "Fuzzy Love", "Bad Dreams",
                "Dark Buzzord", "SqualorDrone", "Air Raid!", "180:CyberToy", "150:PhoneFrq", "160:Clockers", "Phlange Hit",
                "Ring Bass", "Ring Bass Lp", "Orc.Cluster", "Harpin'Piano", "RollingChime", "Thin Bells", "FX Bell 3",
                "Rave Stopper", "MachineNoise", "Gate Keeper", "FootStepping", "Droppin'", "P-Zing", "Rubberized",
                "Pipe", "Boing", "Metal Bang", "MetallicShot", "Doorsqweek", "Tomnoise", "Steam Blow",
                "Let Air Go!", "Screetchy 1", "Screetchy 2", "Tubuloid", "Slicer", "Fuzzclapz", "Brakes!",
                "Fire Snare", "Back Snatch", "Phone", "Sh.Burst", "Blasto Kick", "O.D.Kick", "Bowl Drop",
                "Cavern Tube", "Fireball", "Firebomb", "Firework", "Hothead", "Metalicous", "Cluster Slam",
                "Pit Hit", "Big Stinger", "Mondo Blast", "Ignite!", "Heat Seeker", "Missile", "Fly-By",
                "Psycho Alert", "Distressed", "Afro Cheer", "Kick MENU 1", "Kick MENU 2", "BeatBox Kick", "BeatBox Hat",
                "BeatBox Snr1", "BeatBox Snr2", "BeatBox Rim", "BeatBox Kik2", "BeatBox Perc", "B.Box Shaker", "Chiki!",
                "Ahhhhhh", "B.Box OpHat", "Jungle Kick", "Hip Kick", "Ninja Kick", "Break Kick", "Funky Kick",
                "Slamn'Kick 1", "Too Big Kick", "Boom Kick", "FarNear Kick", "Slamn'Kick 2", "Hall Kick 1", "Dance Kick 4",
                "Thump Kick", "PurePhatKick", "Hall Kick 2", "Plastic Kick", "ElectricKick", "TR808 KickLp", "TR707 Kick 3",
                "Lo-Fi Kick", "Kick MENU 3", "Kick MENU 4", "TR909 Kick 1", "TR909 Kick 2", "Plastic BD 1", "Plastic BD 2",
                "TR808 Kick 1", "TR808 Kick 2", "TR808 Kick 3", "TR606 Kick 1", "TR606 Kick 2", "TR707 Kick 1", "TR707 Kick 2",
                "Culture Kick", "Optic Kick", "Lo-Fi BD", "Wet Kick", "Video Kick", "JungleKick 1", "Street Kick",
                "Turbo Kick", "JungleKick 2", "Tekno Kick", "Sim Kick", "Amsterdam BD", "TR909 Dst BD", "Roll Kick",
                "Kick MENU 5", "Kick MENU 6", "Punchy K 1", "Punchy K 2", "Direct K", "Tone K", "Dynamic K",
                "Clone K", "Noizzy 909 K", "Bottom 909 K", "Sub & Noiz K", "Open Cut K", "Short Dry K", "Medium K2",
                "Hicut K", "Full Range K", "1ToTheFloorK", "DrctRehrsalK", "Fat Clap K", "Bottom K", "50Hz K",
                "Full House K", "Snare MENU 1", "Snare MENU 2", "Snare MENU 3", "HpHpJazzySnr", "DR Snare 1", "DR Snare 2",
                "RealJazz Snr", "Ragga Snr 3", "HipHop Snr 3", "TightBoomSnr", "Slamn' Snr", "PurePhat Snr", "R&B Snare",
                "Hi Snare", "DR Disco Snr", "Cruis'n Snr", "Overring Snr", "Cut Hats Snr", "Scrap Snr", "Machine Snr",
                "DanceHallSnr", "Dance Snr 1", "Big Snare", "Flat Snare", "HipHop Snr 4", "DistortedSnr", "Dance Snr 2",
                "Sizzle Snr", "Break Snr", "Whack Snr 2", "Real Snare", "TR808 Snr 5", "TR808 Snr 6", "TR808 Snr 7",
                "TR606 Snr 4", "Electro Snr", "Echo Snare", "TooBig Snr", "Snare MENU 4", "Snare MENU 5", "Snare MENU 6",
                "Snare MENU 7", "Snare MENU 8", "TR909 Snr 1", "TR909 Snr 2", "TR909 Snr 3", "TR909 Snr 4", "TR909 Snr 5",
                "TR909 Snr 6", "TR909 Snr 7", "TR808 Snr 1", "TR808 Snr 2", "TR808 Snr 3", "TR808 Snr 4", "TR606 Snr 1",
                "TR606 Snr 2", "TR606 Snr 3", "TR707 Snr 1", "TR707 Snr 2", "CR78 Snare", "Headz Snare", "Deep Snare",
                "Fat Snare", "Antigua Snr", "MC Snare", "DJ Snare", "Macho Snare", "Clap Snare", "Rage Snare",
                "Indus Snare", "TekRok Snare", "Jungle Snr 1", "Jungle Snr 2", "Jungle Snr 3", "Jungle Snr 4", "SideStiker",
                "Ragga Snr 2", "Lo-Fi Snare", "Jungle Snr 5", "Urban Snare", "Sim Snare", "Roll Snare", "Snare MENU 9",
                "Snare MENU1", "Dry Comp SN", "ShrtDirectSN", "Tone Comp SN", "Wooden SN", "High Rim SN", "Mid Rim SN",
                "Tone&SynthSN", "Tiny SN", "Little SN", "Clap SN", "Stick SN", "Vintage SN", "Cutted SN",
                "Rim and SN", "AllInOne SN", "Syn-SN", "Live SN", "Back Rev SN", "Heavy SN", "Rim MENU",
                "Bright Rim", "Ragga Rim 1", "TR808 Rim 2", "Ragga Rim 2", "Lo-Fi Rim", "Sizzle Rim", "Real Rim",
                "Tom MENU 1", "TR606 Tom 2", "Electro Tom", "Garbage Tom", "Tom MENU 2", "TR909 Tom", "TR909 DstTom",
                "TR808 Tom", "TR606 Tom", "TR606 CmpTom", "TR707 Tom", "TR707 CmpTom", "Deep Tom", "Kick Tom",
                "Natural Tom", "Can Tom", "HH/Cym MENU", "Overlord", "Real Hat", "Lil' Hat", "Garbage Hat",
                "Ninja Hat", "Junk Hat", "Ragga ClHat", "Rock OpHat", "HipHopMedHat", "CR78 Hat", "TR909 OpHat",
                "TR909 Cymbal", "TR808 Cymbal", "Ragga Crash", "Lo-Fi Ride", "CHH MENU 1", "CHH MENU 2", "TR909 CHH 1",
                "TR909 CHH 2", "TR909 CHH 3", "TR808 CHH 1", "TR808 CHH 2", "TR606 CHH", "TR707 CHH", "CR78 CHH",
                "Pop CHH", "Bristol CHH", "PHH MENU", "TR909 PHH 1", "TR909 PHH 2", "TR808 PHH", "TR606 PHH",
                "TR707 PHH", "OHH MENU", "TR909 OHH 1", "TR909 OHH 2", "TR909 OHH 3", "TR909 DstOHH", "TR808 OHH",
                "TR606 DstOHH", "TR707 OHH", "CR78 OHH", "R8 OHH", "Cym OHH", "HH MENU 1", "HH MENU 2",
                "Old Style HH", "ShortOpen HH", "Closed8bitHH", "ShortNzMidHH", "Mid Hat", "Hi Freq HH", "808 HH",
                "OpenDitherHH", "Open Soft HH", "Noizzz HH", "JazzClosedHH", "Full Ambi HH", "Open Hard HH", "FastOpnSynHH",
                "Hi Open HH", "VinylOpen HH", "Openopen HH", "Vintage Hat", "Let It Go HH", "808 Open HH", "Cymbal MENU1",
                "TR606 Cym 1", "TR606 Cym 2", "TR606 DstCym", "TR909 Ride", "TR909DstRide", "TR707 Ride", "TR909 Crash",
                "TR909DsCrash", "Cymbal MENU2", "Crash Zero", "Crash 1 Long", "Crash 1 Mute", "Crash 2 Long", "Crash 2 Mute",
                "Hi Crash", "Ride 3", "Ride 3 Bell", "Ride 4", "Stick Cymbal", "High Cymbal", "Clap MENU 1",
                "Funk Clap", "HipHop Clap", "Crackhed", "MC Clap", "TR909 Clap 2", "Clap MENU 2", "TR909 Clap",
                "TS Clap", "Clap Stop", "TR707 Clap", "HC2 Dry Clap", "Scratch Clap", "Comp Clap", "Claptail",
                "Clap MENU 3", "Shot Clap", "Room Clap", "Real Clap 1", "Real Clap 2", "Poly Clap", "TMB&SKR MENU",
                "Tambourine 2", "Rattle Tamb", "TechnoShaker", "Dance Shaker", "COW&RIM MENU", "TR808Cowbell", "TR707Cowbell",
                "CR78 Cowbell", "TR727 Agogo", "TR909 Rim", "TR808 Rim", "TR808 RimLng", "TR808 Claves", "Perc. MENU 1",
                "TR808Claves2", "TR808 Cowbel", "Tamb.Short 2", "Tamb. Long 2", "CR78 Tamb 2", "Jingle Bell", "Belltree",
                "Funky Tri", "TR727Quijada", "PERCUSS MENU", "TR808 Conga1", "TR808 Conga2", "Surdo mute", "Surdo open",
                "Perc. MENU 2", "Perc. MENU 3", "Perc. MENU 4", "Perc. MENU 5", "Perc. MENU 6", "ShortGuira 1", "ShortGuira 2",
                "Guira 1", "Guira 2", "Schelle", "Tin Cowbell", "ReverbCowbel", "Real Bell Hi", "AnalogCwblLo",
                "AnalogCwblHi", "SynthCowbell", "Block", "High Claves", "Timbale 2 Hi", "Timbale 2 Lo", "LowRealConga",
                "MidRealConga", "Conga Slap 2", "FstAtkCngMid", "FstAtkCngHi", "FstAtkCngLow", "Da Conga Mid", "Da Conga Hi",
                "AnalgCongaHi", "AnalgCongaLo", "SynthCongaHi", "SynthCongaLo", "TalkingConga", "TambourinHit", "Tamb Hit&Shk",
                "NoizzyTambrn", "Light Tambrn", "SuprHighTamb", "HardTambourn", "EffectTambrn", "Small Shake", "HighShakeUp",
                "HighShakeDwn", "Snake Shake", "Shortie", "Klick Shake", "Open Shake", "Egg Shake", "Dirty Shake",
                "Sand Shake", "Hit Shake", "Analog Shake", "Noiz Perc.", "Tick", "Ping Pong", "Tack+Tambrn",
                "Shot Perc.", "High Cut", "Wood Plopp", "Metal", "Tuned 4 bit", "Toy Perc.", "FX Triangle",
                "Flex", "Metal Box", "VibratingMtl", "Plock Drum", "Sub BD D#", "30Hz Sub C", "REV WildWays",
                "REV FrostByt", "REV Elves", "REV CrystlWv", "REV Rise Up!", "REV Cnstrct2", "REV Turbine", "REV Sawing",
                "REV Air Pump", "REV SodaWatr", "REV CompStrt", "REV CompStop", "REV OilCanBw", "REV Feed Me!", "REV FuzyLove",
                "REV BadDream", "REV DarkBuzz", "REV SqualDrn", "REV CyberToy", "REV PhoneFrq", "REV Clockers", "REV PhlngHit",
                "REV HrpPiano", "REV RollChme", "REV ThinBels", "REV RaveStop", "REV MachinNz", "REV GateKeep", "REV FootStep",
                "REV Droppin'", "REV P-Zing", "REV Rubberiz", "REV Pipe", "REV Boing", "REV Mtl Bang", "REV MtlcShot",
                "REV Doorsqwk", "REV Tomnoise", "REV SteamBlw", "REV Scrtchy1", "REV Fire Snr", "REV BkSnatch", "REV Phone",
                "REV Sh.Burst", "REV BlastKik", "REV O.D.Kick", "REV BowlDrop", "REV CaveTube", "REV Fireball", "REV Firebomb",
                "REV Firework", "REV Hothead", "REV Mtlicous", "REV ClustSlm", "REV Pit Hit", "REV BigSting", "REV MondBlst",
                "REV Ignite!", "REV HeatSeek", "REV Missile", "REV Fly-By", "REV PsycAlrt", "REV Distresd", });
            srxWaveNames.Add(new String[] { // SRX-09
                "Off", "World Tour", "Korean Ens", "Morocco Ens", "African Ens", "Asia Phrase", "122:GamelnP1",
                "78:GamelnP2", "78:Korea Ph", "118:ChinaPh1", "132:ChinaPh2", "118:TablaByP", "92:DholakPh", "120:Dhol Ph",
                "96:Salsa", "116:Mariachi", "96:Cumbia", "192:LatinPt1", "180:LatinPt2", "96:Cng6/8 1", "96:Cng6/8 2",
                "144:CgBolero", "168:CgChacha", "88:CgCumbia", "160:CgMambo", "104:CgMriach", "138:CgMarinr", "126:CgWonwn",
                "132:TmblPtn1", "132:TmblPtn2", "100:TmblPtn3", "120:TmblPtn4", "104:Shakin'", "80:PndrPtn1", "108:PndrPtn2",
                "138:PndrPtn3", "132:AgogoPtn", "132:BotlePtn", "104:BomboPtn", "126:SurdoPt1", "108:SurdoPt2", "138:CaixaPtn",
                "132:TambrmPt", "108:RepnqPt1", "144:RepnqPt2", "Samba MENU", "BnjoPhs Menu", "120:Bnjo G", "120:Bnjo A",
                "120:Bnjo B", "120:Bnjo Cm", "120:Bnjo Cm+", "120:Bnjo G-C", "120:BnjoG-C+", "EG Phs Menu1", "95:EGtr1 G",
                "95:EGtr1 C", "95:EGtr1 D", "95:EGtr2 G", "95:EGtr2 D", "EG Phs Menu2", "140:EGtr3 G+", "140:EGtr3 G",
                "140:EGtr3 C+", "140:EGtr3 C", "140:EGtr3 D+", "140:EGtr3 D", "PdlSteelMenu", "120:PdSt1 C", "120:PdSt1 D",
                "120:PdSt1 G", "140:PdSt2 D", "140:PdSt2 D+", "140:PdSt2 G", "140:PdSt2 G+", "E.Gtr Sus A", "E.Gtr Sus B",
                "E.Gtr Sus C", "Strat Rear A", "Strat Rear B", "Strat Rear C", "LP Clean A", "LP Clean B", "LP Clean C",
                "Tele Front A", "Tele Front B", "Tele Front C", "Tele Rear A", "Tele Rear B", "Tele Rear C", "335Pick A",
                "335Pick B", "335Pick C", "E.Gtr Mute A", "E.Gtr Mute B", "E.Gtr Mute C", "335Mute A", "335Mute B",
                "335Mute C", "PdlSteel 1 A", "PdlSteel 1 B", "PdlSteel 1 C", "PdlSteel 2 A", "PdlSteel 2 B", "PdlSteel 2 C",
                "PdlSteel 3 A", "PdlSteel 3 B", "PdlSteel 3 C", "RequintoGtrA", "RequintoGtrB", "RequintoGtrC", "Nylon Gtr 3A",
                "Nylon Gtr 3B", "Nylon Gtr 3C", "AcGtr Pick A", "AcGtr Pick B", "AcGtr Pick C", "AcGtr Fngr A", "AcGtr Fngr B",
                "AcGtr Fngr C", "Ac.Guitar A", "Ac.Guitar B", "Ac.Guitar C", "Reso.Gtr 2 A", "Reso.Gtr 2 B", "Reso.Gtr 2 C",
                "Reso.Gtr 3pA", "Reso.Gtr 3pB", "Reso.Gtr 3pC", "Reso.Gtr 3fA", "Reso.Gtr 3fB", "Reso.Gtr 3fC", "MandolinDn A",
                "MandolinDn B", "MandolinDn C", "MandolinUp A", "MandolinUp B", "MandolinUp C", "MandlnTrem A", "MandlnTrem B",
                "MandlnTrem C", "Banjo 2 A", "Banjo 2 B", "Banjo 2 C", "Banjo 3 A", "Banjo 3 B", "Banjo 3 C",
                "Bouzouki 2fA", "Bouzouki 2fB", "Bouzouki 2fC", "Bandolim", "Cavaquinho", "Oud A", "Oud B",
                "Oud C", "Sitar/Drone", "Sitar A", "Sitar B", "Sitar C", "Sitar 2 A", "Sitar 2 B",
                "Sitar 2 C", "SitarDrone", "Tambura A", "Tambura B", "Tambura C", "TamburaDrone", "Zither A",
                "Zither B", "Zither C", "Santur 2 A", "Santur 2 B", "Santur 2 C", "Santur Trm A", "Santur Trm B",
                "Santur Trm C", "HmrDulcimer", "Afro Zither", "Yuehchin", "Yangchin", "Yang Qin 2 A", "Yang Qin 2 B",
                "Yang Qin 2 C", "YangQinTrm A", "YangQinTrm B", "YangQinTrm C", "Chung Ruan A", "Chung Ruan B", "Chung Ruan C",
                "Gu Zheng A", "Gu Zheng B", "Gu Zheng C", "Kanoun", "Koto", "Kayakeum", "Sanshin A",
                "Sanshin B", "Sanshin C", "Pi Pa A", "Pi Pa B", "Pi Pa C", "Biwa MENU", "Biwa 1",
                "Biwa 2", "Biwa 3", "Shamisen 2", "Shami Attack", "BerimbauMENU", "Berimbau Opn", "Berimbau Up",
                "Berimbau Dn", "Berimbau Mut", "Oct Harp", "Afro Harp", "JawHarp MENU", "Jaw Harp Opn", "Jaw Harp Wow",
                "Gtr Strum 1", "Gtr Strum 2", "Nyln GtrNz 1", "Nyln GtrNz 2", "Guitarron Nz", "EG Bend Menu", "EG C Bend 1",
                "EG C Bend 2", "EG D Bend 1", "EG D Bend 2", "EG D Bend 3", "EG E Bend 1", "EG E Bend 2", "EG E Bend 3",
                "EG SlideMenu", "EG Slide 1", "EG Slide 2", "EG Slide 3", "EG Slide 4", "EG Slide 5", "EG Slide 6",
                "EG Slide 7", "EG Fx Menu", "EG Rake 1", "EG Rake 2", "EG Mute", "BzkiChd Menu", "Bouzki Chd 1",
                "Bouzki Chd 2", "Bouzki Chd 3", "Sitar Gliss", "GuZhengArpUp", "GuZhengArpDn", "Baby Bass ff", "6Str Bass A",
                "6Str Bass B", "6Str Bass C", "FingerdBs 2A", "FingerdBs 2B", "FingerdBs 2C", "Pick Bass 2", "6StBs Mute A",
                "6StBs Mute B", "6StBs Mute C", "Tub Bass", "Guitarron", "GuitarronOct", "Baby Bass Nz", "Violin 2 A",
                "Violin 2 B", "Violin 2 C", "Fiddle 1 A", "Fiddle 1 B", "Fiddle 1 C", "Fiddle 2 A", "Fiddle 2 B",
                "Fiddle 2 C", "Fdl Pizz 1 A", "Fdl Pizz 1 B", "Fdl Pizz 1 C", "Fdl Stac 1 A", "Fdl Stac 1 B", "Fdl Stac 1 C",
                "Erhu", "Er Hu 2 A", "Er Hu 2 B", "Er Hu 2 C", "Esraj 1", "Esraj 2", "Kemanche",
                "FiddleFxMenu", "Fdl 5th 1", "Fdl 5th 2", "Fdl Riff 6", "Fdl Riff 8", "Er Hu Phrase", "Trumpets f A",
                "Trumpets f B", "Trumpets f C", "Tp Ambient A", "Tp Ambient B", "Tp Ambient C", "SoloTp Vib1A", "SoloTp Vib1B",
                "SoloTp Vib1C", "SoloTp Vib2A", "SoloTp Vib2B", "SoloTp Vib2C", "Tp RomanticA", "Tp RomanticB", "Tp RomanticC",
                "Tp Vib MariA", "Tp Vib MariB", "Tp Vib MariC", "Tp Shake A", "Tp Shake B", "Tp Shake C", "Tp Grrwah A",
                "Tp Grrwah B", "Tp Grrwah C", "MuteTp 2 A", "MuteTp 2 B", "MuteTp 2 C", "Tbns ff A", "Tbns ff B",
                "Tbns ff C", "Fat Tbns A", "Fat Tbns B", "Fat Tbns C", "Tbn Accent A", "Tbn Accent B", "Tbn Accent C",
                "Trombone3mfA", "Trombone3mfB", "Trombone3mfC", "Trombone3 fA", "Trombone3 fB", "Trombone3 fC", "Tbn Alto A",
                "Tbn Alto B", "Tbn Alto C", "Tuba f A", "Tuba f B", "Tuba f C", "2Tp+Tbn A", "2Tp+Tbn B",
                "2Tp+Tbn C", "2Tp+TbAltTnA", "2Tp+TbAltTnB", "2Tp+TbAltTnC", "Sect Fall A", "Sect Fall B", "Sect Fall C",
                "Sect Stact A", "Sect Stact B", "Sect Stact C", "SectChd 13th", "SectChd m9", "SectChd Mj9", "IndFlute L A",
                "IndFlute L B", "IndFlute L C", "IndFlute H A", "IndFlute H B", "IndFlute H C", "Bang Di A", "Bang Di B",
                "Bang Di C", "BangDi Vib A", "BangDi Vib B", "BangDi Vib C", "Qu Di A", "Qu Di B", "Qu Di C",
                "Qu Di Vib A", "Qu Di Vib B", "Qu Di Vib C", "Zampona 1", "Zampo Attack", "Zampo Trem A", "Zampo Trem B",
                "Zampo Trem C", "Sicu Pipe", "Quena", "Ocarina", "Kawala A", "Kawala B", "Kawala C",
                "Shakuhachi 2", "Shaku Attack", "Shaku Ornam", "Hunt Pipe", "Hunt Noise", "Bagpipes 1", "Bagpipes 2",
                "Bagpipes 3", "Bagpipes 4", "Hichiriki", "Hichiriki Lp", "Sheng A", "Sheng B", "Sheng C",
                "Suona A", "Suona B", "Suona C", "Shahnai", "Mizmar", "Mizmar Lp", "Piri",
                "Piri Lp", "Conch Shell1", "Conch Shell2", "Didge MENU", "Didgeridoo 1", "Didgeridoo 2", "Didgeridoo 3",
                "Accrd ff OpA", "Accrd ff OpB", "Accrd ff OpC", "Accrd Cls A", "Accrd Cls B", "Accrd Cls C", "Musette 4 A",
                "Musette 4 B", "Musette 4 C", "D.AccordionA", "D.AccordionB", "D.AccordionC", "Harmonica2 A", "Harmonica2 B",
                "Harmonica2 C", "Harmonica3 A", "Harmonica3 B", "Harmonica3 C", "Harmonica4 A", "Harmonica4 B", "Harmonica4 C",
                "Voice MENU 1", "Yoh Tribe", "How Tribe", "Hey Brazil", "Yyoo Dude", "ZaghrutaLoop", "ZaghrutaStop",
                "Bull Scream", "Voice MENU 2", "Sabor!", "Arriba!", "Ole!", "Uno!", "Dos!",
                "Tres!", "Quatro!", "Grito-Hahaha", "Grito-Ahaha!", "Grito-Haahai", "Grito-Rrrrr!", "Voice MENU 3",
                "Tiquitito!", "Grito-Oa Oa!", "Grito-Eh Eh!", "Ama ya ahi!", "Fuego!", "SambaBateria", "AsiaGng MENU",
                "Asian Gong 1", "Asian Gong 2", "Asian Gong 3", "Asian Gong 4", "Asian Gong 5", "Asian Gong 6", "Asian Gong 7",
                "REV Gong 5", "REV Gong 7", "Gong MENU", "Gamelan Gong", "Kempur", "Kajar", "Kelontuk",
                "Kelontuk Mt", "Kelontuk Sid", "Kemong", "Ceng Ceng", "Kopyak Op", "Kopyak Mt", "Kane",
                "Kane Side", "Korea MENU", "Ching Open", "Ching Mute", "Kwaengwari p", "Kwaengwari f", "KwaengwariMt",
                "Bonang", "Spokes", "Satellite Dr", "Atarigane", "China MENU", "Xiao Bo", "Nao Bo",
                "Shou Luo 1", "Shou Luo 2", "HuYinLuoL Op", "HuYinLuoL Mt", "HuYinLuoH Op", "HuYinLuoH Mt", "Ban Gu 1",
                "Ban Gu 2", "Ban Gu 3", "Gender", "Saron", "Jegogan A", "Jegogan B", "Jegogan C",
                "Jublag A", "Jublag B", "Jublag C", "Pemade A", "Pemade B", "Pemade C", "Reyong A",
                "Reyong B", "Reyong C", "Reyong Sid A", "Reyong Sid B", "Reyong Sid C", "Steel Dr 2", "Kalimba 1",
                "Kalimba 2", "Kalimba 3", "Kalimba Glis", "Kalim Gls Lp", "Balaphone 1", "Balaphone 2", "AsiaCym MENU",
                "Chenchen Ptn", "Chenchen Opn", "Chenchen Cls", "BaliCym Opn", "BaliCym Cls", "Finger Cym", "Ramacymbal",
                "Sagat Open", "Sagat Close", "Sarna Bell", "Wind Bell", "Blossom Bell", "Agogo MENU", "Agogo 2 Lo",
                "Agogo 2 Hi", "Agogo 3 Lo", "Agogo 3 Hi", "AggoCwblMenu", "BongoBell Op", "BongoBell Mt", "MamboBell Op",
                "MamboBell Mt", "Cowbell 1", "Cowbell 2", "Cowbell 3", "VDrm Cowbell", "Cowbell Op 1", "Cowbell Mt 1",
                "Cowbell Op 2", "Cowbell Mt 2", "TablaByMENU1", "TablaBaya 1", "TablaBaya 2", "TablaBaya 3", "TablaBaya 4",
                "TablaBaya 5", "TablaBaya 6", "TablaBaya 7", "TablaBaya 8", "TablaByMENU2", "TablaBayaSld", "TablaBayaGin",
                "TablaBaya Ge", "TablaBaya Ka", "TablaBaya Na", "TablaBayaTin", "TablaBayaTun", "TablaBaya Te", "TablaBaya Ti",
                "Dholak MENU1", "Dholak 1", "Dholak 2", "Dholak 3", "Dholak 4", "Dholak 5", "Dholak 6",
                "Dholak 7", "Dholak 8", "Dholak 9", "Dholak MENU2", "Dholak Ga", "Dholak Ta", "Dholak Tun",
                "Dholak Na", "Madal MENU", "Madal Da", "Madal Din", "Madal Ta", "Dhol MENU", "Dhol 1",
                "Dhol 2", "Dhol 3", "Dhol 4", "Dhol 5", "Udu Pot MENU", "Udu Pot1 Lo", "Udu Pot1 Hi",
                "Udu Pot1 Slp", "Udu Pot1 Acc", "Udu Pot2 Lng", "Udu Pot2 Sht", "Udu Pot2 Mut", "Kendang MENU", "Wadon 1",
                "Wadon 2", "Wadon 3", "Wadon 4", "Wadon 5", "Wadon 6", "Wadon 7", "Bebarongan 1",
                "Bebarongan 2", "Bebarongan 3", "Pelegongan 1", "Pelegongan 2", "Pelegongan 3", "TalkingDr Up", "TalkingDr Dn",
                "AfroDrm MENU", "AfroDrum Op1", "AfroDrum Op2", "AfroDrum Flm", "AfroDrum Rat", "Tablah MENU", "Tablah Bend",
                "Tablah Dom", "Tablah Tak", "Tablah Rim", "Tablah Roll", "Doira Dun", "Doira Tik", "Doholla MENU",
                "Doholla Dom", "Doholla Sak", "Doholla Tak", "Doholla Roll", "Doholla Stop", "Rek MENU", "Rek Dom",
                "Rek Tek", "Rek Open", "Rek Trill", "Bendir 1", "Bendir 2", "Dawul", "JapanPrcMENU",
                "Japan MENU", "Ho", "Yoh", "iYooh", "Taiko", "Wadaiko", "Wadaiko Rim",
                "Sime Taiko", "Shimedaiko 2", "Buk", "Buk Rim", "Tsuzumi Lo", "Tsuzumi Hi", "Tsuzumi 2 p",
                "Tsuzumi 2 mf", "Tsuzumi 2 Hi", "Mokugyo 1", "Mokugyo 2", "Ohkawa", "Ohkawa 2", "Hyoshigi",
                "Hyoshigi 2", "Clapstick", "VDrm Woodblk", "Boomerang", "Changgo", "Tang Gu Op", "Tang Gu Mt",
                "Shu Gu", "Shu Gu Rim", "Shu Ban 1", "Shu Ban 2", "Ban Gu 4", "ChinaPrcMENU", "Gu Roll",
                "Gu Hi", "Rot Drum", "Log Drum 2", "Slit Drum", "Bongo MENU 1", "Bongo 1 Lo", "Bongo 1 Hi",
                "Bongo 2 Lo", "Bongo 2 Hi", "Bongo MENU 2", "Bongo LoOp f", "Bongo Lo Hrd", "Bongo Lo Sft", "Bongo LoOpmf",
                "Bongo LoSlap", "Bongo Hi Hrd", "Bongo HiOp f", "BongoHiSlap1", "BongoHiSlap2", "Conga Menu", "CongaLoOp f",
                "CongaLoOp mf", "Conga Lo Mt", "Conga Slap", "Conga Hi Op", "Conga Hi Mt", "Conga Thumb", "Conga Link",
                "Conga Roll", "Timbale MENU", "Timbale Lo", "Timbale Hi", "Timbale Side", "TimbalesMenu", "Timbles LoOp",
                "Timbles LoMt", "Timbles HiOp", "Timbles HiMt", "TimbalesHand", "Timbales Rim", "TmbSideStick", "TimbalesFil1",
                "TimbalesFil2", "TimbalesFil3", "TimbalesFil4", "Surdo MENU 1", "Surdo Open L", "Surdo Open H", "Surdo Mute",
                "Surdo Rim", "Surdo MENU 2", "Surdo Hard", "Surdo Open1", "Surdo Open2", "Surdo Mute2", "Surdo Rim 2",
                "Surdo Soft", "Caixa MENU 1", "Caixa Open1", "Caixa Open2", "Caixa Roll", "Caixa Mute", "Caixa MENU 2",
                "Caixa Open3", "Caixa Mute2", "Caixa Roll2", "Caixa Rim", "Rpnq&RpqMenu", "RepiniqueHrd", "RepiniqueSft",
                "Repinique1", "Repinique2", "Repique Open", "Repique Rim", "Repique Roll", "Clv&CucaMenu", "Clave!",
                "Claves Lo 2", "VDrm Claves", "Cuica 2", "Cuica 3", "Cuica Lo 1", "Cuica Lo 2", "Cuica Hi 1",
                "Cuica Hi 2", "MrcsShkrMenu", "Maracas 2", "MaracasUpDwn", "VDrm Maracas", "Shaker MENU", "Shaker Ptn",
                "Shaker 1", "Shaker 2", "Shaker Long", "Shaker Short", "Angklung 2", "Rainstick", "Guiro Long",
                "Guiro Short", "Guiro Menu", "Quide Long", "Quide Short", "Guiro Long2", "Guiro Short2", "RecoRecoLng",
                "RecoRecoSht", "MtlGuiroLng", "MtlGuiroSht", "CbsCxGnzMenu", "Real Cabasa1", "Real Cabasa2", "Cabasa Roll",
                "VDrmCabasaUp", "VDrmCabasDwn", "Caxixi", "Ganza Soft", "Ganza Hard", "Chekere 1", "Chekere 2",
                "PandeiroMENU", "PandeiroL Lo", "PandeiroL Hi", "PandeiroL Sp", "PandeiroL Rm", "PandeiroS Op", "PandeiroS Sp",
                "PandeiroS Rm", "PandTambMenu", "PandeiroOpen", "PandeiroMute", "PandeiroHit", "PandeiroRim", "PandeiroCrsh",
                "PandeiroRoll", "TamborimMENU", "Tamborim Opn", "Tamborim Mut", "Tamborim Slp", "TamborimOpen", "TamborimRim",
                "TamborimMute", "TamborimRoll", "Tambrin Hit", "TambrinShake", "R8 Tamb", "Others Menu", "ApitoHiShort",
                "ApitoLoShort", "SambaWhistle", "Quijada", "Chekere 3", "CajonClpMenu", "Cajon Lo", "Cajon Hi",
                "Cajon Rol Hi", "Cajon Rol Lo", "Afro Feet 1", "Afro Feet 2", "Afro Clap", "Hand Clap 2", "Claps Real",
                "FingerSnaps4", "BomboBbnMenu", "SoftSet Menu", "HardSet Menu", "Fat BD", "Room Kick 2", "Maple Kick",
                "Warm Kick", "Maple Lo Snr", "MapleSoft SN", "NaturlHardSN", "NaturlRimSht", "Cross Stick2", "StudioLo Tom",
                "StudioMidTom", "StudioHi Tom", "Dixie HH Pdl", "Dixie HH Cls", "Dixie HH Hlf", "Dixie HH Opn", });
            srxWaveNames.Add(new String[] { // SRX-10
                "Off", "PopBrs mfA L", "PopBrs mfA R", "PopBrs mfB L", "PopBrs mfB R", "PopBrs mfC L", "PopBrs mfC R",
                "PopBrs f A L", "PopBrs f A R", "PopBrs f B L", "PopBrs f B R", "PopBrs f C L", "PopBrs f C R", "PopStc mfA L",
                "PopStc mfA R", "PopStc mfB L", "PopStc mfB R", "PopStc mfC L", "PopStc mfC R", "PopStc f A L", "PopStc f A R",
                "PopStc f B L", "PopStc f B R", "PopStc f C L", "PopStc f C R", "PopBrsRipA L", "PopBrsRipA R", "PopBrsRipB L",
                "PopBrsRipB R", "PopBrsRipC L", "PopBrsRipC R", "PopFlSht A L", "PopFlSht A R", "PopFlSht B L", "PopFlSht B R",
                "PopFlSht C L", "PopFlSht C R", "PopFlLng A L", "PopFlLng A R", "PopFlLng B L", "PopFlLng B R", "PopFlLng C L",
                "PopFlLng C R", "Tps mf A L", "Tps mf A R", "Tps mf B L", "Tps mf B R", "Tps mf C L", "Tps mf C R",
                "Tps f A L", "Tps f A R", "Tps f B L", "Tps f B R", "Tps f C L", "Tps f C R", "Tps Stc mfAL",
                "Tps Stc mfAR", "Tps Stc mfBL", "Tps Stc mfBR", "Tps Stc mfCL", "Tps Stc mfCR", "Tps Stc f AL", "Tps Stc f AR",
                "Tps Stc f BL", "Tps Stc f BR", "Tps Stc f CL", "Tps Stc f CR", "Tps Mute A L", "Tps Mute A R", "Tps Mute B L",
                "Tps Mute B R", "Tps Mute C L", "Tps Mute C R", "Tps MtStc AL", "Tps MtStc AR", "Tps MtStc BL", "Tps MtStc BR",
                "Tps MtStc CL", "Tps MtStc CR", "Tps RipFl AL", "Tps RipFl AR", "Tps RipFl BL", "Tps RipFl BR", "Tps RipFl CL",
                "Tps RipFl CR", "Tps RipCp AL", "Tps RipCp AR", "Tps RipCp BL", "Tps RipCp BR", "Tps RipCp CL", "Tps RipCp CR",
                "Tps Fall A L", "Tps Fall A R", "Tps Fall B L", "Tps Fall B R", "Tps Fall C L", "Tps Fall C R", "Tps Shake AL",
                "Tps Shake AR", "Tps Shake BL", "Tps Shake BR", "Tps Shake CL", "Tps Shake CR", "Tps Doit A L", "Tps Doit A R",
                "Tps Doit B L", "Tps Doit B R", "Tps Doit C L", "Tps Doit C R", "C Tps mp A L", "C Tps mp A R", "C Tps mp B L",
                "C Tps mp B R", "C Tps mp C L", "C Tps mp C R", "C Tps mf A L", "C Tps mf A R", "C Tps mf B L", "C Tps mf B R",
                "C Tps mf C L", "C Tps mf C R", "C Tps f A L", "C Tps f A R", "C Tps f B L", "C Tps f B R", "C Tps f C L",
                "C Tps f C R", "CTpsStc mpAL", "CTpsStc mpAR", "CTpsStc mpBL", "CTpsStc mpBR", "CTpsStc mpCL", "CTpsStc mpCR",
                "CTpsStc mfAL", "CTpsStc mfAR", "CTpsStc mfBL", "CTpsStc mfBR", "CTpsStc mfCL", "CTpsStc mfCR", "CTpsStc f AL",
                "CTpsStc f AR", "CTpsStc f BL", "CTpsStc f BR", "CTpsStc f CL", "CTpsStc f CR", "CTps Trit AL", "CTps Trit AR",
                "CTps Trit BL", "CTps Trit BR", "CTps Trit CL", "CTps Trit CR", "Tbs mp A L", "Tbs mp A R", "Tbs mp B L",
                "Tbs mp B R", "Tbs mp C L", "Tbs mp C R", "Tbs mf A L", "Tbs mf A R", "Tbs mf B L", "Tbs mf B R",
                "Tbs mf C L", "Tbs mf C R", "Tbs f A L", "Tbs f A R", "Tbs f B L", "Tbs f B R", "Tbs f C L",
                "Tbs f C R", "Tbs Stc mfAL", "Tbs Stc mfAR", "Tbs Stc mfBL", "Tbs Stc mfBR", "Tbs Stc mfCL", "Tbs Stc mfCR",
                "Tbs Stc f AL", "Tbs Stc f AR", "Tbs Stc f BL", "Tbs Stc f BR", "Tbs Stc f CL", "Tbs Stc f CR", "Tbs Fall A L",
                "Tbs Fall A R", "Tbs Fall B L", "Tbs Fall B R", "Tbs Fall C L", "Tbs Fall C R", "Tbs Rip A L", "Tbs Rip A R",
                "Tbs Rip B L", "Tbs Rip B R", "Tbs Rip C L", "Tbs Rip C R", "Horns mp A L", "Horns mp A R", "Horns mp B L",
                "Horns mp B R", "Horns mp C L", "Horns mp C R", "Horns mf A L", "Horns mf A R", "Horns mf B L", "Horns mf B R",
                "Horns mf C L", "Horns mf C R", "Horns f A L", "Horns f A R", "Horns f B L", "Horns f B R", "Horns f C L",
                "Horns f C R", "HrnsStc mpAL", "HrnsStc mpAR", "HrnsStc mpBL", "HrnsStc mpBR", "HrnsStc mpCL", "HrnsStc mpCR",
                "HrnsStc mfAL", "HrnsStc mfAR", "HrnsStc mfBL", "HrnsStc mfBR", "HrnsStc mfCL", "HrnsStc mfCR", "HrnsStc f AL",
                "HrnsStc f AR", "HrnsStc f BL", "HrnsStc f BR", "HrnsStc f CL", "HrnsStc f CR", "Hrns Rip A L", "Hrns Rip A R",
                "Hrns Rip B L", "Hrns Rip B R", "Hrns Rip C L", "Hrns Rip C R", "Tuba mf A", "Tuba mf B", "Tuba mf C",
                "Tuba f A", "Tuba f B", "Tuba f C", "Tuba Stc mpA", "Tuba Stc mpB", "Tuba Stc mpC", "Tuba Stc mfA",
                "Tuba Stc mfB", "Tuba Stc mfC", "Tuba Stc f A", "Tuba Stc f B", "Tuba Stc f C", "Brs LipNz1 L", "Brs LipNz1 R",
                "Brs LipNz2 L", "Brs LipNz2 R", "Brs LipNz3 L", "Brs LipNz3 R", "Tp LipNz 1 L", "Tp LipNz 1 R", "Tp LipNz 2 L",
                "Tp LipNz 2 R", "CTp Lip Nz L", "CTp Lip Nz R", });
            srxWaveNames.Add(new String[] { // SRX-11
                "Off", "CmpPno ppA L","CmpPno ppA R","CmpPno ppB L","CmpPno ppB R","CmpPno ppB’L","CmpPno ppB’R",
                "CmpPno ppC L","CmpPno ppC R","CmpPno ppC’L","CmpPno ppC’R","CmpPno mpA L","CmpPno mpA R","CmpPno mpB L",
                "CmpPno mpB R","CmpPno mpB’L","CmpPno mpB’R","CmpPno mpC L","CmpPno mpC R","CmpPno mpC’L","CmpPno mpC’R",
                "CmpPno f A L","CmpPno f A R","CmpPno f B L","CmpPno f B R","CmpPno f B’L","CmpPno f B’R","CmpPno f C L",
                "CmpPno f C R","CmpPno f C’L","CmpPno f C’R","CmpPno ffA L","CmpPno ffA R","CmpPno ffB L","CmpPno ffB R",
                "CmpPno ffB’L","CmpPno ffB’R","CmpPno ffC L","CmpPno ffC R","CmpPno ffC’L","CmpPno ffC’R",});
            srxWaveNames.Add(new String[] { // SRX-12
                "Off", "EPtype1 pp A", "EPtype1 pp B", "EPtype1 pp C", "EPtype1 ppV1", "EPtype1 ppV2", "EPtype1 ppV3",
                "EPtype1 ppV4", "EPtype1 mp A", "EPtype1 mp B", "EPtype1 mp C", "EPtype1 mpV1", "EPtype1 mpV2", "EPtype1 mpV3",
                "EPtype1 mpV4", "EPtype1 f A", "EPtype1 f B", "EPtype1 f C", "EPtype1 f V1", "EPtype1 f V2", "EPtype1 f V3",
                "EPtype1 f V4", "EPtype1 ff A", "EPtype1 ff B", "EPtype1 ff C", "EPtype1 ffV1", "EPtype1 ffV2", "EPtype1 ffV3",
                "EPtype1 ffV4", "EPtype2 pp A", "EPtype2 pp B", "EPtype2 pp C", "EPtype2 ppV1", "EPtype2 ppV2", "EPtype2 ppV3",
                "EPtype2 ppV4", "EPtype2 mp A", "EPtype2 mp B", "EPtype2 mp C", "EPtype2 mpV1", "EPtype2 mpV2", "EPtype2 mpV3",
                "EPtype2 mpV4", "EPtype2 f A", "EPtype2 f B", "EPtype2 f C", "EPtype2 f V1", "EPtype2 f V2", "EPtype2 f V3",
                "EPtype2 f V4", "EPtype2 ff A", "EPtype2 ff B", "EPtype2 ff C", "EPtype2 ffV1", "EPtype2 ffV2", "EPtype2 ffV3",
                "EPtype2 ffV4", "EPtype3 pp A", "EPtype3 pp B", "EPtype3 pp C", "EPtype3 ppV1", "EPtype3 ppV2", "EPtype3 ppV3",
                "EPtype3 ppV4", "EPtype3 mp A", "EPtype3 mp B", "EPtype3 mp C", "EPtype3 mpV1", "EPtype3 mpV2", "EPtype3 mpV3",
                "EPtype3 mpV4", "EPtype3 f A", "EPtype3 f B", "EPtype3 f C", "EPtype3 f V1", "EPtype3 f V2", "EPtype3 f V3",
                "EPtype3 f V4", "EPtype3 ff A", "EPtype3 ff B", "EPtype3 ff C", "EPtype3 ffV1", "EPtype3 ffV2", "EPtype3 ffV3",
                "EPtype3 ffV4", "Clav CA mp A", "Clav CA mp B", "Clav CA mp C", "Clav CA f A", "Clav CA f B", "Clav CA f C",
                "Clav CA ff A", "Clav CA ff B", "Clav CA ff C", "Clav CB mp A", "Clav CB mp B", "Clav CB mp C", "Clav CB f A",
                "Clav CB f B", "Clav CB f C", "Clav CB ff A", "Clav CB ff B", "Clav CB ff C", "Clav DA mp A", "Clav DA mp B",
                "Clav DA mp C", "Clav DA f A", "Clav DA f B", "Clav DA f C", "Clav DA ff A", "Clav DA ff B", "Clav DA ff C",
                "Clav DB mp A", "Clav DB mp B", "Clav DB mp C", "Clav DB f A", "Clav DB f B", "Clav DB f C", "Clav DB ff A",
                "Clav DB ff B", "Clav DB ff C", "ClvMtCA mp A", "ClvMtCA mp B", "ClvMtCA mp C", "ClvMt CA f A", "ClvMt CA f B",
                "ClvMt CA f C", "ClvMtCB mp A", "ClvMtCB mp B", "ClvMtCB mp C", "ClvMt CB f A", "ClvMt CB f B", "ClvMt CB f C",
                "ClvMtDA mp A", "ClvMtDA mp B", "ClvMtDA mp C", "ClvMt DA f A", "ClvMt DA f B", "ClvMt DA f C", "ClvMtDB mp A",
                "ClvMtDB mp B", "ClvMtDB mp C", "ClvMt DB f A", "ClvMt DB f B", "ClvMt DB f C", "ClvMtRsCA pA", "ClvMtRsCA pB",
                "ClvMtRsCA pC", "ClvMtRsCA fA", "ClvMtRsCA fB", "ClvMtRsCA fC", "ClvMtRsCB pA", "ClvMtRsCB pB", "ClvMtRsCB pC",
                "ClvMtRsCB fA", "ClvMtRsCB fB", "ClvMtRsCB fC", "ClvMtRsDA pA", "ClvMtRsDA pB", "ClvMtRsDA pC", "ClvMtRsDA fA",
                "ClvMtRsDA fB", "ClvMtRsDA fC", "ClvMtRsDB pA", "ClvMtRsDB pB", "ClvMtRsDB pC", "ClvMtRsDB fA", "ClvMtRsDB fB",
                "ClvMtRsDB fC", });
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
            pcmToneCategoryNames = new String[] { "No Assign", "Ac.Piano", "", "", "E.Piano", "", "Organ", "", "", "Other Keyboards",
                "", "", "Accordeon/Harmonica", "", "Bell/Mallet", "", "Ac.Guitar", "E.Guitar", "Dist.Guitar", "Ac.Bass", "E.Bass",
                "Synth Bass", "Plucked/Stroke", "Strings", "", "", "Brass", "", "Wind", "Flute", "Sax", "Recorder", "Vox/Choir", "",
                "Synth Lead", "Synth Brass", "Synth Pad/Strings", "Synth Bellpad", "Synth PolyKey", "FX", "Synth Seq/Pop", "Phrase",
                "Pulsating", "Beat&Groove", "Hit", "Sound FX", "Drums", "Percussion", "Combination" };
            snsToneCategoryNames = new String[] { "No assign", "Ac.Piano", "", "", "E.Piano", "", "Organ", "", "", "Other Keyboards",
                "", "", "Accordion/Harmonica", "", "Bell/Mallet", "Ac.Guitar", "E.Guitar", "Dist.Guitar", "Ac.Bass", "E.Bass",
                "Synth Bass", "Plucked/Stroke", "Strings", "Brass", "", "Wind", "Flute", "Sax", "Recorder", "Vox/Choir", "",
                "Synth Lead", "Synth Brass", "Synth Pad/Strings", "Synth Bellpad", "Synth PolyKey", "FX", "Synth Seq/Pop", "Phrase",
                "Pulsating", "Beat&Groove", "Hit", "Sound FX", "Drums", "Percussion", "Combination" };
            snaToneCategoryNames = new String[] { "No assign", "Ac.Piano", "", "", "E.Piano", "", "Organ", "", "", "Other Keyboards",
                "", "", "Accordion/Harmonica", "", "Bell/Mallet", "Ac.Guitar", "E.Guitar", "Dist.Guitar", "Ac.Bass", "E.Bass",
                "Synth Bass", "Plucked/Stroke", "Strings", "", "", "Brass", "", "Wind", "Flute", "Sax", "Recorder", "Vox/Choir", "",
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

    [DataContract]
    public class FavoritesFolder
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
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

    [DataContract]
    public class FavoritesList
    {
        [DataMember]
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

    public enum ProgramType
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

    public enum PARAMETER_TYPE
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

    [DataContract]
    public class ParameterSets
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class ParameterSets");
        /// <summary>
        /// These are the specifics for SET_OF_NAMES type numbered parameters for PCM Synth Tone MFX
        /// </summary>
        /// <param name="MFXType"></param>
        /// <param name="i"></param>
        
        [DataMember]
        public String[] NumberedParameter { get; set; }

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
            NumberedParameter = result;
            return result;
        }
    }

    [DataContract]
    public class NumberedParameters
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class NumberedParameters");
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public byte Offset { get; set; }
        [DataMember]
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
    [DataContract]
    public class NumberedParameter
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class NumberedParameter");
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public PARAMETER_TYPE Type { get; set; }
        [DataMember]
        public String ControlName { get; set; }
        [DataMember]
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
    [DataContract]
    public class NumberedParameterValue
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class NumberedParameterValue");
        [DataMember]
        public String[] Text { get; set; } // These are texts for type SET_OF_NAMES
        [DataMember]
        public UInt16 Value { get; set; }    // This is for numerical values
        [DataMember]
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

    [DataContract]
    public class MFXNumberedParameters
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class MFXNumberedParameters");
        [DataMember]
        public UInt16 Offset { get; set; }
        [DataMember]
        public byte MFXType { get; set; }
        [DataMember]
        public byte MFXLength { get; set; }
        [DataMember]
        public NumberedParameters Parameters { get; set; }
        [DataMember]
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

    /*
     * When changing instrument number in I-7 front panel, it sends the following data.
     * Since parameters are just numbered, where each parameter have different meaning
     * for different instrument, we better just do that as well. Send from the table below.
     * Address is 0x19 0x02 0x00 0x20, and each line is an instrument.
     * This applies only to SuperNATURAL Acoustic tones.
     * Here we also have a list of modules and synth types, with and without descriptions.
     * First list is a list of booleans telling wether the type is editable or not.
     */
    class InstrumentSettings
    {
        public static Boolean[] Editable = new Boolean[] { false, true, true, true, true, true, true, false, false,
            true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };

        public static String[] ModulesAndDescriptions = new string[]
        {
            "ExPCM: HQ GM2 + HQ PCM Sound Col",
            "ExSN1: Ethnic",
            "ExSN2: Wood Winds",
            "ExSN3: Session",
            "ExSN4: A.Guitar",
            "ExSN5: Brass",
            "ExSN6: SFX",
            "GM2 Drum Kit (PCM Drum Kit)",
            "GM2 Tone (PCM Synth Tone)",
            "PCM Drum Kit",
            "PCM Synth Tone",
            "SRX-01: Dynamic Drum Kits",
            "SRX-02: Concert Piano",
            "SRX-03: Studio SRX",
            "SRX-04: Symphonique Strings",
            "SRX-05: Supreme Dance",
            "SRX-06: Complete Orchestra",
            "SRX-07: Ultimate Keys",
            "SRX-08: Platinum Trax",
            "SRX-09: World Collection",
            "SRX-10: Big Brass Ensemble",
            "SRX-11: Complete Piano",
            "SRX-12: Classic EPs",
            "SuperNATURAL Acoustic Tone",
            "SuperNATURAL Synth Tone",
            "SuperNATURAL Drum Kit",
        };

        public static String[] ModuleNames = new string[]
        {
            "ExPCM",
            "ExSN1",
            "ExSN2",
            "ExSN3",
            "ExSN4",
            "ExSN5",
            "ExSN6",
            "GM2-D",
            "GM2_T",
            "PCM-D",
            "PCM-S",
            "SRX-01",
            "SRX-02",
            "SRX-03",
            "SRX-04",
            "SRX-05",
            "SRX-06",
            "SRX-07",
            "SRX-08",
            "SRX-09",
            "SRX-10",
            "SRX-11",
            "SRX-12",
            "SN-A",
            "SN-S",
            "SN-D",
        };

        /*
         */
        public static byte[][][] Tone = new byte[][][]
        {
            //"ExPCM: HQ GM2 + HQ PCM Sound Col",
            new byte[][]
            {
                // This one can not be edited!
            },
            //"ExSN1: Ethnic",
            new byte[][]
            {
                new byte[] { 0x20, 0x00, 0x0f, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x20, 0x01, 0x2e, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x20, 0x01, 0x4b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x20, 0x01, 0x4d, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x20, 0x00, 0x6a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x20, 0x01, 0x6a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x20, 0x00, 0x6b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x20, 0x01, 0x6b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x20, 0x00, 0x6c, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x20, 0x02, 0x6e, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
            },
            //"ExSN2: Wood Winds",
            new byte[][]
            {
                new byte[] { 0x00, 0x40, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x41, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x42, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x43, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x45, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x47, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x49, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x4a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x4a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x02, 0x4a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x03, 0x4a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x4f, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x4f, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x02, 0x4f, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x03, 0x4f, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
            },
            //"ExSN3: Session",
            new byte[][]
            {
                new byte[] { 0x01, 0x1a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x02, 0x1a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x03, 0x1b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x04, 0x1b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x05, 0x1b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x20, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x21, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x22, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
            },
            //"ExSN4: A.Guitar",
            new byte[][]
            {
                new byte[] { 0x02, 0x18, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x03, 0x18, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x19, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x02, 0x19, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x03, 0x19, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x04, 0x19, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
            },
            //"ExSN5: Brass",
            new byte[][]
            {
                new byte[] { 0x01, 0x38, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x02, 0x38, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x03, 0x38, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x04, 0x38, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x39, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x02, 0x39, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x3a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x3b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x02, 0x3b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x3c, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x02, 0x3c, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
            },
            //"ExSN6: SFX",
            new byte[][]
            {
                // Drum kits are handled quite different since they have one sound per key.
            },
            //"GM2 Drum Kit (PCM Drum Kit)",
            new byte[][]
            {
                // GM2 tones cannot be edited.
            },
            //"GM2 Tone (PCM Synth Tone)",
            new byte[][]
            {
                // GM2 tones cannot be edited.
            },
            //"PCM Drum Kit",
            new byte[][]
            {
                // Drum kits are handled quite different since they have one sound per key.
            },
            //"PCM Synth Tone",
            new byte[][]
            {
                // PCM Synth tones does not have this need since they do not have tone variations.
            },
            //"SRX-01: Dynamic Drum Kits",
            new byte[][]
            {
                // Drum kits are handled quite different since they have one sound per key.
            },
            //"SRX-02: Concert Piano",
            new byte[][]
            {
                // PCM Synth tones does not have this need since they do not have tone variations.
            },
            //"SRX-03: Studio SRX",
            new byte[][]
            {
                // PCM Synth tones does not have this need since they do not have tone variations.
            },
            //"SRX-04: Symphonique Strings",
            new byte[][]
            {
                // PCM Synth tones does not have this need since they do not have tone variations.
            },
            //"SRX-05: Supreme Dance",
            new byte[][]
            {
                // PCM Synth tones does not have this need since they do not have tone variations.
            },
            //"SRX-06: Complete Orchestra",
            new byte[][]
            {
                // PCM Synth tones does not have this need since they do not have tone variations.
            },
            //"SRX-07: Ultimate Keys",
            new byte[][]
            {
                // PCM Synth tones does not have this need since they do not have tone variations.
            },
            //"SRX-08: Platinum Trax",
            new byte[][]
            {
                // PCM Synth tones does not have this need since they do not have tone variations.
            },
            //"SRX-09: World Collection",
            new byte[][]
            {
                // PCM Synth tones does not have this need since they do not have tone variations.
            },
            //"SRX-10: Big Brass Ensemble",
            new byte[][]
            {
                // PCM Synth tones does not have this need since they do not have tone variations.
            },
            //"SRX-11: Complete Piano",
            new byte[][]
            {
                // PCM Synth tones does not have this need since they do not have tone variations.
            },
            //"SRX-12: Classic EPs",
            new byte[][]
            {
                // PCM Synth tones does not have this need since they do not have tone variations.
            },
            //"SuperNATURAL Acoustic Tone",
            new byte[][]
            {
                new byte[] { 0x40, 0x00, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 },
                new byte[] { 0x40, 0x01, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 },
                new byte[] { 0x40, 0x02, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 },
                new byte[] { 0x40, 0x03, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 },
                new byte[] { 0x40, 0x04, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 },
                new byte[] { 0x40, 0x05, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 },
                new byte[] { 0x40, 0x06, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 },
                new byte[] { 0x40, 0x07, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 },
                new byte[] { 0x40, 0x08, 0x40, 0x40, 0x40, 0x3f, 0x00, 0x40 },
                new byte[] { 0x00, 0x04, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x04, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x02, 0x04, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x03, 0x04, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x06, 0x04, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x07, 0x04, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x02, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x03, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x04, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x05, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x06, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x07, 0x07, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x09, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x0b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x0c, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x0d, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x0e, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x41, 0x00, 0x08, 0x08, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x05, 0x05, 0x03, 0x0a, 0x68, 0x2b, 0x05, 0x40, 0x00, 0x14 },
                new byte[] { 0x00, 0x15, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x15, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x16, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x17, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x18, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x18, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x19, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x1a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x1b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x1b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x02, 0x1b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x20, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x21, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x22, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x23, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x28, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x28, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x29, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x2a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x2a, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x2b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x2e, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x2f, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x30, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x30, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x34, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x34, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x38, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x39, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x03, 0x39, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x3b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x3c, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x40, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x41, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x02, 0x42, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x43, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x44, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x46, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x47, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x48, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x49, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x4b, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x4d, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x68, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x6d, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x6d, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x01, 0x6e, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x72, 0x40, 0x40, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00 },
            },
            //"SuperNATURAL Synth Tone",
            new byte[][]
            {
                // SuperNATURAL Synth tones does not have this need since they do not have tone variations.
            },
            //"SuperNATURAL Drum Kit",
            new byte[][]
            {
                // Drum kits are handled quite different since they have one sound per key.
            },
        };
    }

    class SuperNATURALAcousticToneVariation
    {
        public String Bank { get; set; }
        public byte VariationIndex { get; set; }
        //public byte ComboBoxOffset { get; set; }
        public String InstrumentName { get; set; }
        public List<String> Variations { get; set; }

        public SuperNATURALAcousticToneVariation(String Bank, byte VariationIndex, byte ComboBoxOffset, String InstrumentName, String Variation1, String Variation2, String Variation3, String Variation4)
        {
            this.Bank = Bank;
            this.VariationIndex = VariationIndex;
            //this.ComboBoxOffset = (byte)(ComboBoxOffset + 1); // Because ComBox creation code will add an "Off" entry first.
            this.InstrumentName = InstrumentName;
            Variations = new List<String>();
            if (!String.IsNullOrEmpty(Variation1)) Variations.Add(Variation1);
            if (!String.IsNullOrEmpty(Variation2)) Variations.Add(Variation2);
            if (!String.IsNullOrEmpty(Variation3)) Variations.Add(Variation3);
            if (!String.IsNullOrEmpty(Variation4)) Variations.Add(Variation4);
        }
    }

    class SuperNATURALAcousticToneVariations
    {
        List<SuperNATURALAcousticToneVariation> SuperNATURALAcousticToneVariation;

        public SuperNATURALAcousticToneVariations()
        {
            SuperNATURALAcousticToneVariation = new List<Integra_7_Xamarin.SuperNATURALAcousticToneVariation>();
            // Bank, Instrument number (VariationIndex), ?, Name, Variation 1, Variation 2, Variation 3, Variation 4
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 24, 0, "Glockenspiel", "Dead Stroke", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 25, 0, "Vibraphone", "Dead Stroke", "Tremolo Sw", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 25, 0, "Vibes Hard", "Dead Stroke", "Tremolo Sw", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 25, 0, "Vibes Soft", "Dead Stroke", "Tremolo Sw", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 25, 0, "Vibes Trem", "Dead Stroke", "Tremolo Sw", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 26, 0, "Marimba", "Dead Stroke", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 26, 0, "Marimba Hard", "Dead Stroke", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 26, 0, "Marimba Soft", "Dead Stroke", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 27, 0, "Xylophone", "Dead Stroke", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 27, 0, "Hard Xylo", "Dead Stroke", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 28, 0, "Tubular Bells", "Dead Stroke", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 28, 0, "TubulrBells1", "Dead Stroke", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 28, 0, "TubulrBells2", "Dead Stroke", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 34, 0, "Nylon Guitar", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 34, 0, "Classic Gtr", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 34, 0, "Gut Guitar", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 34, 0, "Solid GutGt", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 35, 0, "Flamenco Guitar", "Rasugueado", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 35, 0, "Flamenco Gtr", "Rasugueado", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 35, 0, "Warm Spanish", "Rasugueado", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 35, 0, "Rasugueado", "Rasugueado", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 36, 0, "SteelStr Guitar", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 36, 0, "StrumSteelGt", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 36, 0, "ArpegSteelGt", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 37, 0, "Jazz Guitar", "FingerPicking", "Octave Tone", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 38, 0, "ST Guitar Half", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 39, 0, "ST Guitar Front", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 40, 0, "TC Guitar Rear", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 41, 0, "Acoustic Bass", "Staccato", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 42, 0, "Fingered Bass", "Slap", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 43, 0, "Picked Bass ", "Bridge Mute", "Harmonics", " - ", " - "));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 44, 0, "Fretless Bass", "Staccato", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 45, 0, "Violin", "Staccato", "Pizzicato", "Tremolo", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 46, 0, "Violin 2", "Staccato", "Pizzicato", "Tremolo", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 47, 0, "Viola", "Staccato", "Pizzicato", "Tremolo", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 48, 0, "Cello", "Staccato", "Pizzicato", "Tremolo", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 49, 0, "Cello 2", "Staccato", "Pizzicato", "Tremolo", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 50, 0, "Contrabass", "Staccato", "Pizzicato", "Tremolo", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 51, 0, "Harp", "Nail", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 52, 0, "Timpani", "Flam", "Accent Roll", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 53, 0, "Strings", "Staccato", "Pizzicato", "Tremolo", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 54, 0, "Marcato Strings", "Staccato", "Pizzicato", "Tremolo", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 55, 0, "London Choir", "Voice Woo", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 56, 0, "Boys Choir", "Voice Woo", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 57, 0, "Trumpet", "Staccato", "Fall", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 58, 0, "Trombone", "Staccato", "Fall", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 59, 0, "Tb2 CupMute", "Staccato", "Fall", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 60, 0, "Mute Trumpet", "Staccato", "Fall", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 61, 0, "French Horn", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 62, 0, "Sop Sax 2", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 63, 0, "Alto Sax 2", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 64, 0, "T.Sax 2", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 65, 0, "Bari Sax 2", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 66, 0, "Oboe", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 67, 0, "Bassoon", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 68, 0, "Clarinet", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 69, 0, "Piccolo", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 70, 0, "Flute", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 71, 0, "Pan Flute", "Staccato", "Flutter", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 72, 0, "Shakuhachi", "Staccato", "Ornament", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 74, 1, "Uilleann Pipes", "", "Ornament", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 75, 1, "Bag Pipes", "", "Ornament", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 76, 0, "Erhu", "Staccato", "Ornament", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("INT", 77, 0, "Steel Drums", "Mute", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 1, 0, "Santoor", "Mute", "Tremolo", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 1, 0, "Santoor 1", "Mute", "Tremolo", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 1, 0, "Santoor 2", "Mute", "Tremolo", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 2, 0, "Yang Chin", "Mute", "Tremolo", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 2, 0, "Yang Chin 1", "Mute", "Tremolo", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 2, 0, "Yang Chin 2", "Mute", "Tremolo", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 2, 0, "Yang Chin 3", "Mute", "Tremolo", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 3, 0, "Tin Whistle", "Cut", "Ornament", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 4, 0, "Ryuteki", "Staccato", "Ornament", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 5, 0, "Tsugaru", "Strum", "Up Picking", "Auto Bend", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 6, 0, "Sansin", "Strum", "Up Picking", "Auto Bend", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 7, 0, "Koto", "Tremolo", "Ornament", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN1", 9, 0, "Kalimba", "Buzz", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 1, 0, "Sop Sax 1", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 1, 0, "SopSax1 Soft", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 2, 0, "A.Sax 1 Soft", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 2, 0, "Alto Sax 1", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 3, 0, "T.Sax 1 Soft", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 3, 0, "T.Sax Growl", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 3, 0, "TenorSax 1", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 4, 0, "B.Sax 1 Soft", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 4, 0, "Bari Sax 1", "Staccato", "Fall", "SubTone", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 5, 0, "English Horn", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 6, 0, "Bass Clarinet", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 7, 0, "Flute2", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 8, 0, "Soprano Recorder", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 9, 0, "Alto Recorder", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 10, 0, "Tenor Recorder", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 11, 0, "Bass Recorder", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 12, 0, "Ocarina SopC", "Staccato", "Ornament", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 13, 0, "Ocarina SopF", "Staccato", "Ornament", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 14, 0, "Ocarina Alto", "Staccato", "Ornament", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN2", 15, 0, "Ocarina Bass", "Staccato", "Ornament", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 1, 0, "TC Guitar w/Fing", "FingerPicking", "Octave Tone", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 2, 0, "335Guitar w/Fing", "FingerPicking", "Octave Tone", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 3, 0, "LP Guitar Rear", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 4, 0, "LP Guitar Front", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 5, 0, "335 Guitar Half", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 6, 0, "Acoustic Bass 2", "Staccato", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 7, 0, "Fingered Bass 2", "Slap", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN3", 8, 0, "Picked Bass 2 ", "Bridge Mute", "Harmonics", " - ", " - "));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 2, 0, "Nylon Guitar 2", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 3, 0, "12th Steel Gtr", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 4, 0, "Mandolin", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 4, 0, "MandolinGt", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 4, 0, "MandolinStum", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 5, 0, "SteelFing Guitar", "FingerPicking", "Octave Tone", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN4", 6, 0, "SteelStr Guitar2", "Mute", "Harmonics", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 1, 0, "Classical Trumpet", "Staccato", "Fall", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 2, 0, "Frugal Horn", "Staccato", "Fall", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 3, 0, "Trumpet 2", "Staccato", "Fall", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 4, 0, "Mariachi Tp", "Staccato", "Fall", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 5, 0, "Trombone 2", "Staccato", "Fall", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 6, 0, "Bass Trombone", "Staccato", "Fall", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 7, 0, "Tuba", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 8, 0, "Straight Mute Tp", "Staccato", "Fall", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 9, 0, "Cup Mute Trumpet", "Staccato", "Fall", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 10, 0, "French Horn 2", "Staccato", "", "", ""));
            SuperNATURALAcousticToneVariation.Add(new SuperNATURALAcousticToneVariation("ExSN5", 11, 0, "Mute French Horn", "Staccato", "", "", ""));
        }

        public SuperNATURALAcousticToneVariation Get(String Bank, Int32 VariationIndex)
        {
            UInt16 i = 0;
            if (Bank == "Preset SN-A")
            {
                Bank = "INT";
            }
            while (i < SuperNATURALAcousticToneVariation.Count())
            {
                if (Bank.StartsWith(SuperNATURALAcousticToneVariation[i].Bank) && SuperNATURALAcousticToneVariation[i].VariationIndex == VariationIndex)
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
            Tones.Add(new Instrument("INT", 35, "Flamenco Gtr", "Ac.Guitar", 5));
            Tones.Add(new Instrument("INT", 36, "SteelStr Gtr", "Ac.Guitar", 5));
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
                for (byte j = 0; j < 51; j++)
                {
                    ParameterMask[i][j] = 0; 
                }
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
    [DataContract]
    public class PCMSynthTone
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class PCMSynthTone");
        [DataMember]
        public PCMSynthToneCommon pCMSynthToneCommon { get; set; }
        //public PCMSynthToneCommonMFX PCMSynthToneCommonMFX { get; set; }
        [DataMember]
        public PCMSynthTonePMT pCMSynthTonePMT { get; set; }
        [DataMember]
        public PCMSynthTonePartial[] pCMSynthTonePartial { get; set; }
        [DataMember]
        public PCMSynthToneCommon2 pCMSynthToneCommon2 { get; set; }

        public PCMSynthTone(ReceivedData Data)
        {
            t.Trace("public PCMSynthTone (" + "ReceivedData" + Data + ", " + ")");
            pCMSynthTonePartial = new PCMSynthTonePartial[4];
            pCMSynthToneCommon = new PCMSynthToneCommon(Data);
        }
    }

    /// <summary>
    /// Same as above for all main classes
    /// </summary>

    [DataContract]
    public class PCMDrumKit
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class PCMDrumKit");
        [DataMember]
        public PCMDrumKitCommon pCMDrumKitCommon { get; set; }
        //public PCMDrumKitCommonMFX PCMDrumKitCommonMFX { get; set; }
        [DataMember]
        public PCMDrumKitCommonCompEQ pCMDrumKitCommonCompEQ { get; set; }
        [DataMember]
        public PCMDrumKitPartial[] pCMDrumKitPartial { get; set; } // [88]
        [DataMember]
        public PCMDrumKitCommon2 pCMDrumKitCommon2 { get; set; }

        public PCMDrumKit(ReceivedData Data)
        {
            t.Trace("public PCMDrumKit (" + "ReceivedData" + Data + ", " + ")");
            pCMDrumKitPartial = new PCMDrumKitPartial[88];
            pCMDrumKitCommon = new PCMDrumKitCommon(Data);
        }
    }

    [DataContract]
    public class SuperNATURALAcousticTone
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class SuperNATURALAcousticTone");
        [DataMember]
        public SuperNATURALAcousticToneCommon superNATURALAcousticToneCommon { get; set; }
        //public SuperNATURALAcousticToneMFX SuperNATURALAcousticToneMFX { get; set; }

        public SuperNATURALAcousticTone(ReceivedData Data)
        {
            t.Trace("public SuperNATURALAcousticTone (" + "ReceivedData" + Data + ", " + ")");
            superNATURALAcousticToneCommon = new SuperNATURALAcousticToneCommon(Data);
        }
    }

    [DataContract]
    public class SuperNATURALSynthTone
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class SuperNATURALSynthTone");
        [DataMember]
        public SuperNATURALSynthToneCommon superNATURALSynthToneCommon { get; set; }
        //public SuperNATURALSynthToneCommonMFX SuperNATURALSynthToneCommonMFX { get; set; }
        [DataMember]
        public SuperNATURALSynthTonePartial[] superNATURALSynthTonePartial { get; set; }
        [DataMember]
        public SuperNATURALSynthToneMisc superNATURALSynthToneMisc { get; set; }

        public SuperNATURALSynthTone(ReceivedData Data)
        {
            t.Trace("public SuperNATURALSynthTone (" + "ReceivedData" + Data + ", " + ")");
            superNATURALSynthToneCommon = new SuperNATURALSynthToneCommon(Data);
            superNATURALSynthTonePartial = new SuperNATURALSynthTonePartial[3];
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

    [DataContract]
    public class SuperNATURALDrumKit
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class SuperNATURALDrumKit");
        [DataMember]
        public SuperNATURALDrumKitCommon superNATURALDrumKitCommon { get; set; }
        [DataMember]
        public SuperNATURALDrumKitCommonCompEQ superNATURALDrumKitCommonCompEQ { get; set; }
        [DataMember]
        public SuperNATURALDrumKitKey[] superNATURALDrumKitKey { get; set; }

        public SuperNATURALDrumKit(ReceivedData Data)
        {
            t.Trace("public SuperNATURALDrumKit (" + "ReceivedData" + Data + ", " + ")");
            superNATURALDrumKitCommon = new SuperNATURALDrumKitCommon(Data);
            superNATURALDrumKitKey = new SuperNATURALDrumKitKey[62];
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

    [DataContract]
    public class CommonMFX
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class CommonMFX");
        [DataMember]
        public byte MFXType { get; set; }
        [DataMember]
        public byte Reserve1 { get; set; }
        [DataMember]
        public byte MFXChorusSendLevel { get; set; }
        [DataMember]
        public byte MFXReverbSendLevel { get; set; }
        [DataMember]
        public byte Reserve2 { get; set; }
        [DataMember]
        public byte[] MFXControlSource { get; set; } // [4]
        [DataMember]
        public byte[] MFXControlSens { get; set; }   // [4]
        [DataMember]
        public byte[] MFXControlAssign { get; set; } // [4]
        [DataMember]
        public MFXNumberedParameters MFXNumberedParameters { get; set; }
        //[DataMember]
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

    [DataContract]
    public class PCMSynthToneCommon
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class PCMSynthToneCommon");
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public byte Level { get; set; }
        [DataMember]
        public byte Pan { get; set; }
        [DataMember]
        public byte Priority { get; set; }
        [DataMember]
        public byte CoarseTune { get; set; }
        [DataMember]
        public byte FineTune { get; set; }
        [DataMember]
        public byte OctaveShift { get; set; }
        [DataMember]
        public byte TuneDepth { get; set; }
        [DataMember]
        public byte AnalogFeel { get; set; }
        [DataMember]
        public byte MonoPoly { get; set; }
        [DataMember]
        public Boolean LegatoSwitch { get; set; }
        [DataMember]
        public Boolean LegatoRetrigger { get; set; }
        [DataMember]
        public Boolean PortamentoSwitch { get; set; }
        [DataMember]
        public byte PortamentoMode { get; set; }
        [DataMember]
        public byte PortamentoType { get; set; }
        [DataMember]
        public byte PortamentoStart { get; set; }
        [DataMember]
        public byte PortamentoTime { get; set; }
        [DataMember]
        public byte CutoffOffset { get; set; }
        [DataMember]
        public byte ResonanceOffset { get; set; }
        [DataMember]
        public byte AttackTimeOffset { get; set; }
        [DataMember]
        public byte ReleaseTimeOffset { get; set; }
        [DataMember]
        public byte VelocitySenseOffset { get; set; }
        [DataMember]
        public Boolean PMTControlSwitch { get; set; }
        [DataMember]
        public byte PitchBendRangeUp { get; set; }
        [DataMember]
        public byte PitchBendRangeDown { get; set; }
        [DataMember]
        public byte[] MatrixControlSource { get; set; }        // [4]
        [DataMember]
        public byte[][] MatrixControlDestination { get; set; } // [4][4]
        [DataMember]
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
    [DataContract]
    public class PCMSynthTonePMT // Partial Mapping Table
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class PCMSynthTonePMT // Partial Mapping Table");
        [DataMember]
        public byte StructureType1_2 { get; set; }
        [DataMember]
        public byte Booster1_2 { get; set; }
        [DataMember]
        public byte StructureType3_4 { get; set; }
        [DataMember]
        public byte Booster3_4 { get; set; }
        [DataMember]
        public byte PMTVelocityControl { get; set; }
        [DataMember]
        public Boolean[] PMTPartialSwitch { get; set; }       // [4]
        [DataMember]
        public byte[] PMTKeyboardRangeLower { get; set; }     // [4]
        [DataMember]
        public byte[] PMTKeyboardRangeUpper { get; set; }     // [4]
        [DataMember]
        public byte[] PMTKeyboardFadeWidthLower { get; set; } // [4]
        [DataMember]
        public byte[] PMTKeyboardFadeWidthUpper { get; set; } // [4]
        [DataMember]
        public byte[] PMTVelocityRangeLower { get; set; }     // [4]
        [DataMember]
        public byte[] PMTVelocityRangeUpper { get; set; }     // [4]
        [DataMember]
        public byte[] PMTVelocityFadeWidthLower { get; set; } // [4]
        [DataMember]
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

    [DataContract]
    public class LFO
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class LFO");
        [DataMember]
        public byte LFOWaveform { get; set; }
        [DataMember]
        public byte LFORate { get; set; }
        [DataMember]
        public byte LFOOffset { get; set; }
        [DataMember]
        public byte LFORateDetune { get; set; }
        [DataMember]
        public byte LFODelayTime { get; set; }
        [DataMember]
        public byte LFODelayTimeKeyfollow { get; set; }
        [DataMember]
        public byte LFOFadeMode { get; set; }
        [DataMember]
        public byte LFOFadeTime { get; set; }
        [DataMember]
        public Boolean LFOKeyTrigger { get; set; }
        [DataMember]
        public byte LFOPitchDepth { get; set; }
        [DataMember]
        public byte LFOTVFDepth { get; set; }
        [DataMember]
        public byte LFOTVADepth { get; set; }
        [DataMember]
        public byte LFOPanDepth { get; set; }

        public LFO(ReceivedData Data, byte msb, byte lsb)
        {
            t.Trace("*** " + DateTime.Now.TimeOfDay.Milliseconds.ToString());
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

    [DataContract]
    public class TVA
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class TVA");
        [DataMember]
        public byte TVALevelVelocityCurve { get; set; }
        [DataMember]
        public byte TVALevelVelocitySens { get; set; }
        [DataMember]
        public byte TVAEnvTime1VelocitySens { get; set; }
        [DataMember]
        public byte TVAEnvTime4VelocitySens { get; set; }
        [DataMember]
        public byte TVAEnvTimeKeyfollow { get; set; }
        [DataMember]
        public byte[] TVAEnvTime { get; set; } // [4]
        [DataMember]
        public byte[] TVAEnvLevel { get; set; } // [3]

        public TVA(ReceivedData Data, byte msb, byte lsb, Boolean keyFollow)
        {
            t.Trace("*** " + DateTime.Now.TimeOfDay.Milliseconds.ToString());
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

    [DataContract]
    public class TVF
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class TVF");
        [DataMember]
        public byte TVFFilterType { get; set; }
        [DataMember]
        public byte TVFCutoffFrequency { get; set; }
        [DataMember]
        public byte TVFCutoffKeyfollow { get; set; }
        [DataMember]
        public byte TVFCutoffVelocityCurve { get; set; }
        [DataMember]
        public byte TVFCutoffVelocitySens { get; set; }
        [DataMember]
        public byte TVFResonance { get; set; }
        [DataMember]
        public byte TVFResonanceVelocitySens { get; set; }
        [DataMember]
        public byte TVFEnvDepth { get; set; }
        [DataMember]
        public byte TVFEnvVelocityCurve { get; set; }
        [DataMember]
        public byte TVFEnvVelocitySens { get; set; }
        [DataMember]
        public byte TVFEnvTime1VelocitySens { get; set; }
        [DataMember]
        public byte TVFEnvTime4VelocitySens { get; set; }
        [DataMember]
        public byte TVFEnvTimeKeyfollow { get; set; }
        [DataMember]
        public byte[] TVFEnvTime { get; set; } // [4]
        [DataMember]
        public byte[] TVFEnvLevel { get; set; } // [5]

        public TVF(ReceivedData Data, byte msb, byte lsb, Boolean keyFollow)
        {
            t.Trace("*** " + DateTime.Now.TimeOfDay.Milliseconds.ToString());
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

    [DataContract]
    public class WMT
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class WMT");
        [DataMember]
        public Boolean WMTWaveSwitch { get; set; }
        [DataMember]
        public byte WMTWaveGroupType { get; set; }
        [DataMember]
        public UInt16 WMTWaveGroupID { get; set; }
        [DataMember]
        public UInt16 WMTWaveNumberL { get; set; }
        [DataMember]
        public UInt16 WMTWaveNumberR { get; set; }
        [DataMember]
        public byte WMTWaveGain { get; set; }
        [DataMember]
        public Boolean WMTWaveFXMSwitch { get; set; }
        [DataMember]
        public byte WMTWaveFXMColor { get; set; }
        [DataMember]
        public byte WMTWaveFXMDepth { get; set; }
        [DataMember]
        public Boolean WMTWaveTempoSync { get; set; }
        [DataMember]
        public byte WMTWaveCoarseTune { get; set; }
        [DataMember]
        public byte WMTWaveFineTune { get; set; }
        [DataMember]
        public byte WMTWavePan { get; set; }
        [DataMember]
        public Boolean WMTWaveRandomPanSwitch { get; set; }
        [DataMember]
        public byte WMTWaveAlternatePanSwitch { get; set; }
        [DataMember]
        public byte WMTWaveLevel { get; set; }
        [DataMember]
        public byte WMTVelocityRangeLower { get; set; }
        [DataMember]
        public byte WMTVelocityRangeUpper { get; set; }
        [DataMember]
        public byte WMTVelocityFadeWidthLower { get; set; }
        [DataMember]
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

    [DataContract]
    public class PitchEnv
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class PitchEnv");
        [DataMember]
        public byte PitchEnvDepth { get; set; }
        [DataMember]
        public byte PitchEnvVelocitySens { get; set; }
        [DataMember]
        public byte PitchEnvTime1VelocitySens { get; set; }
        [DataMember]
        public byte PitchEnvTime4VelocitySens { get; set; }
        [DataMember]
        public byte PitchEnvTimeKeyfollow { get; set; }
        [DataMember]
        public byte[] PitchEnvTime { get; set; }  // [4]
        [DataMember]
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

    [DataContract]
    public class PCMSynthTonePartial
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class PCMSynthTonePartial");
        [DataMember]
        public TVA TVA { get; set; }
        [DataMember]
        public TVF TVF { get; set; }
        [DataMember]
        public LFO LFO1 { get; set; }
        [DataMember]
        public LFO LFO2 { get; set; }
        [DataMember]
        public PitchEnv PitchEnv { get; set; }
        [DataMember]
        public byte PartialLevel { get; set; }
        [DataMember]
        public byte PartialCoarseTune { get; set; }
        [DataMember]
        public byte PartialFineTune { get; set; }
        [DataMember]
        public byte PartialRandomPitchDepth { get; set; }
        [DataMember]
        public byte PartialPan { get; set; }
        [DataMember]
        public byte PartialPanKeyfollow { get; set; }
        [DataMember]
        public byte PartialRandomPanDepth { get; set; }
        [DataMember]
        public byte PartialAlternatePanDepth { get; set; }
        [DataMember]
        public byte PartialEnvMode { get; set; }
        [DataMember]
        public byte PartialDelayMode { get; set; }
        [DataMember]
        public byte PartialDelayTime { get; set; }
        [DataMember]
        public byte PartialOutputLevel { get; set; }
        [DataMember]
        public byte PartialChorusSendLevel { get; set; }
        [DataMember]
        public byte PartialReverbSendLevel { get; set; }
        [DataMember]
        public Boolean PartialReceiveBender { get; set; }
        [DataMember]
        public Boolean PartialReceiveExpression { get; set; }
        [DataMember]
        public Boolean PartialReceiveHold_1 { get; set; }
        [DataMember]
        public Boolean PartialRedamperSwitch { get; set; }
        [DataMember]
        public byte[][] PartialControlSwitch { get; set; } // [4][4]
        [DataMember]
        public byte WaveGroupType { get; set; }
        [DataMember]
        public UInt16 WaveGroupID { get; set; }
        [DataMember]
        public UInt16 WaveNumberL { get; set; }
        [DataMember]
        public UInt16 WaveNumberR { get; set; }
        [DataMember]
        public byte WaveGain { get; set; }
        [DataMember]
        public Boolean WaveFXMSwitch { get; set; }
        [DataMember]
        public byte WaveFXMColor { get; set; }
        [DataMember]
        public byte WaveFXMDepth { get; set; }
        [DataMember]
        public Boolean WaveTempoSync { get; set; }
        [DataMember]
        public byte WavePitchKeyfollow { get; set; }
        [DataMember]
        public byte BiasLevel { get; set; }
        [DataMember]
        public byte BiasPosition { get; set; }
        [DataMember]
        public byte BiasDirection { get; set; }
        [DataMember]
        public byte LFOStepType { get; set; }
        [DataMember]
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

    [DataContract]
    public class PCMSynthToneCommon2
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class PCMSynthToneCommon2");
        [DataMember]
        public byte ToneCategory { get; set; }
        [DataMember]
        public byte MissingInDocs { get; set; }
        [DataMember]
        public byte PhraseOctaveShift { get; set; }
        [DataMember]
        public Boolean TFXSwitch { get; set; }
        [DataMember]
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

    [DataContract]
    public class PCMDrumKitCommon
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class PCMDrumKitCommon");
        [DataMember]
        public String Name { get; set; }
        [DataMember]
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

    [DataContract]
    public class CompEq
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class CompEq");
        [DataMember]
        public Boolean CompSwitch { get; set; }
        [DataMember]
        public byte CompAttackTime { get; set; }
        [DataMember]
        public byte CompReleaseTime { get; set; }
        [DataMember]
        public byte CompThreshold { get; set; }
        [DataMember]
        public byte CompRatio { get; set; }
        [DataMember]
        public byte CompOutputGain { get; set; }
        [DataMember]
        public Boolean EQSwitch { get; set; }
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

    [DataContract]
    public class PCMDrumKitCommonCompEQ
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class PCMDrumKitCommonCompEQ");
        [DataMember]
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

    [DataContract]
    public class PCMDrumKitPartial
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class PCMDrumKitPartial");
        [DataMember]
        public TVF TVF { get; set; }
        [DataMember]
        public TVA TVA { get; set; }
        [DataMember]
        public WMT[] WMT { get; set; } // [4]
        [DataMember]
        public PitchEnv PitchEnv { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public byte AssignType { get; set; }
        [DataMember]
        public byte MuteGroup { get; set; }
        [DataMember]
        public byte PartialLevel { get; set; }
        [DataMember]
        public byte PartialCoarseTune { get; set; }
        [DataMember]
        public byte PartialFineTune { get; set; }
        [DataMember]
        public byte PartialRandomPitchDepth { get; set; }
        [DataMember]
        public byte PartialPan { get; set; }
        [DataMember]
        public byte PartialRandomPanDepth { get; set; }
        [DataMember]
        public byte PartialAlternatePanDepth { get; set; }
        [DataMember]
        public byte PartialEnvMode { get; set; }
        [DataMember]
        public byte PartialOutputLevel { get; set; }
        [DataMember]
        public byte PartialChorusSendLevel { get; set; }
        [DataMember]
        public byte PartialReverbSendLevel { get; set; }
        [DataMember]
        public byte PartialOutputAssign { get; set; }
        [DataMember]
        public byte PartialPitchBendRange { get; set; }
        [DataMember]
        public Boolean PartialReceiveExpression { get; set; }
        [DataMember]
        public Boolean PartialReceiveHold_1 { get; set; }
        [DataMember]
        public byte WMTVelocityControl { get; set; }
        [DataMember]
        public Boolean OneShotMode { get; set; }
        [DataMember]
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

    [DataContract]
    public class PCMDrumKitCommon2
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class PCMDrumKitCommon2");
        [DataMember]
        public byte PhraseNumber { get; set; }
        [DataMember]
        public byte TFXSwitch { get; set; }

        public PCMDrumKitCommon2(ReceivedData Data)
        {
            t.Trace("public PCMDrumKitCommon2 (" + "ReceivedData" + Data + ", " + ")");
            PhraseNumber = Data.Get2Byte(16);
            TFXSwitch = Data.GetByte(18);
        }
    }

    [DataContract]
    public class SuperNATURALSynthToneCommon
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class SuperNATURALSynthToneCommon");
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public byte ToneLevel { get; set; }
        [DataMember]
        public Boolean PortamentoSwitch { get; set; }
        [DataMember]
        public byte PortamentoTime { get; set; }
        [DataMember]
        public byte MonoPoly { get; set; }
        [DataMember]
        public byte OctaveShift { get; set; }
        [DataMember]
        public byte PitchBendRangeUp { get; set; }
        [DataMember]
        public byte PitchBendRangeDown { get; set; }
        [DataMember]
        public Boolean Partial1Switch { get; set; }
        [DataMember]
        public byte Partial1Select { get; set; }
        [DataMember]
        public Boolean Partial2Switch { get; set; }
        [DataMember]
        public byte Partial2Select { get; set; }
        [DataMember]
        public Boolean Partial3Switch { get; set; }
        [DataMember]
        public byte Partial3Select { get; set; }
        [DataMember]
        public Boolean RINGSwitch { get; set; } // 0 - 2!
        [DataMember]
        public Boolean TFXSwitch { get; set; }
        [DataMember]
        public Boolean UnisonSwitch { get; set; }
        [DataMember]
        public byte PortamentoMode { get; set; }
        [DataMember]
        public Boolean LegatoSwitch { get; set; }
        [DataMember]
        public byte AnalogFeel { get; set; }
        [DataMember]
        public byte WaveShape { get; set; }
        [DataMember]
        public byte Category { get; set; }
        [DataMember]
        public UInt16 PhraseNumber { get; set; }
        [DataMember]
        public byte PhraseOctaveShift { get; set; }
        [DataMember]
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

    [DataContract]
    public class SuperNATURALSynthTonePartial
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class SuperNATURALSynthTonePartial");
        [DataMember]
        public byte OSCWave { get; set; }
        [DataMember]
        public byte OSCWaveVariation { get; set; }
        [DataMember]
        public byte OSCPitch { get; set; }
        [DataMember]
        public byte OSCDetune { get; set; }
        [DataMember]
        public byte OSCPulseWidthModDepth { get; set; }
        [DataMember]
        public byte OSCPulseWidth { get; set; }
        [DataMember]
        public byte OSCPitchEnvAttackTime { get; set; }
        [DataMember]
        public byte OSCPitchEnvDecay { get; set; }
        [DataMember]
        public byte OSCPitchEnvDepth { get; set; }
        [DataMember]
        public byte FILTERMode { get; set; }
        [DataMember]
        public byte FILTERSlope { get; set; }
        [DataMember]
        public byte FILTERCutoff { get; set; }
        [DataMember]
        public byte FILTERCutoffKeyfollow { get; set; }
        [DataMember]
        public byte FILTEREnvVelocitySens { get; set; }
        [DataMember]
        public byte FILTERResonance { get; set; }
        [DataMember]
        public byte FILTEREnvAttackTime { get; set; }
        [DataMember]
        public byte FILTEREnvDecayTime { get; set; }
        [DataMember]
        public byte FILTEREnvSustainLevel { get; set; }
        [DataMember]
        public byte FILTEREnvReleaseTime { get; set; }
        [DataMember]
        public byte FILTEREnvDepth { get; set; }
        [DataMember]
        public byte AMPLevel { get; set; }
        [DataMember]
        public byte AMPLevelVelocitySens { get; set; }
        [DataMember]
        public byte AMPEnvAttackTime { get; set; }
        [DataMember]
        public byte AMPEnvDecayTime { get; set; }
        [DataMember]
        public byte AMPEnvSustainLevel { get; set; }
        [DataMember]
        public byte AMPEnvReleaseTime { get; set; }
        [DataMember]
        public byte AMPPan { get; set; }
        [DataMember]
        public byte LFOShape { get; set; }
        [DataMember]
        public byte LFORate { get; set; }
        [DataMember]
        public Boolean LFOTempoSyncSwitch { get; set; }
        [DataMember]
        public byte LFOTempoSyncNote { get; set; }
        [DataMember]
        public byte LFOFadeTime { get; set; }
        [DataMember]
        public Boolean LFOKeyTrigger { get; set; }
        [DataMember]
        public byte LFOPitchDepth { get; set; }
        [DataMember]
        public byte LFOFilterDepth { get; set; }
        [DataMember]
        public byte LFOAmpDepth { get; set; }
        [DataMember]
        public byte LFOPanDepth { get; set; }
        [DataMember]
        public byte ModulationLFOShape { get; set; }
        [DataMember]
        public byte ModulationLFORate { get; set; }
        [DataMember]
        public Boolean ModulationLFOTempoSyncSwitch { get; set; }
        [DataMember]
        public byte ModulationLFOTempoSyncNote { get; set; }
        [DataMember]
        public byte OSCPulseWidthShift { get; set; }
        [DataMember]
        public byte ModulationLFOPitchDepth { get; set; }
        [DataMember]
        public byte ModulationLFOFilterDepth { get; set; }
        [DataMember]
        public byte ModulationLFOAmpDepth { get; set; }
        [DataMember]
        public byte ModulationLFOPanDepth { get; set; }
        [DataMember]
        public byte CutoffAftertouchSens { get; set; }
        [DataMember]
        public byte LevelAftertouchSens { get; set; }
        [DataMember]
        public byte WaveGain { get; set; }
        [DataMember]
        public UInt16 WaveNumber { get; set; }
        [DataMember]
        public byte HPFCutoff { get; set; }
        [DataMember]
        public byte SuperSawDetune { get; set; }
        [DataMember]
        public byte ModulationLFORateControl { get; set; }
        [DataMember]
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

    [DataContract]
    public class SuperNATURALSynthToneMisc
    {
        [DataMember]
        public byte AttackTimeIntervalSens { get; set; }
        [DataMember]
        public byte ReleaseTimeIntervalSens { get; set; }
        [DataMember]
        public byte PortamentoTimeIntervalSens { get; set; }
        [DataMember]
        public byte EnvelopeLoopMode { get; set; }
        [DataMember]
        public byte EnvelopeLoopSyncNote { get; set; }
        [DataMember]
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

    [DataContract]
    public class SuperNATURALAcousticToneCommon
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class SuperNATURALAcousticToneCommon");
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public byte ToneLevel { get; set; }
        [DataMember]
        public byte MonoPoly { get; set; }
        [DataMember]
        public byte PortamentoTimeOffset { get; set; }
        [DataMember]
        public byte CutoffOffset { get; set; }
        [DataMember]
        public byte ResonanceOffset { get; set; }
        [DataMember]
        public byte AttackTimeOffset { get; set; }
        [DataMember]
        public byte ReleaseTimeOffset { get; set; }
        [DataMember]
        public byte VibratoRate { get; set; }
        [DataMember]
        public byte VibratoDepth { get; set; }
        [DataMember]
        public byte VibratoDelay { get; set; }
        [DataMember]
        public byte OctaveShift { get; set; }
        [DataMember]
        public byte Category { get; set; }
        [DataMember]
        public byte PhraseNumber { get; set; }
        [DataMember]
        public byte PhraseOctaveShift { get; set; }
        [DataMember]
        public Boolean TFXSwitch { get; set; }
        [DataMember]
        public byte InstVariation { get; set; }
        [DataMember]
        public byte InstNumber { get; set; }
        [DataMember]
        public byte ModifyParameter1 { get; set; }
        [DataMember]
        public byte ModifyParameter2 { get; set; }
        [DataMember]
        public byte ModifyParameter3 { get; set; }
        [DataMember]
        public byte ModifyParameter4 { get; set; }
        [DataMember]
        public byte ModifyParameter5 { get; set; }
        [DataMember]
        public byte ModifyParameter6 { get; set; }
        [DataMember]
        public byte ModifyParameter7 { get; set; }
        [DataMember]
        public byte ModifyParameter8 { get; set; }
        [DataMember]
        public byte ModifyParameter9 { get; set; }
        [DataMember]
        public byte ModifyParameter10 { get; set; }
        [DataMember]
        public byte ModifyParameter11 { get; set; }
        [DataMember]
        public byte ModifyParameter12 { get; set; }
        [DataMember]
        public byte ModifyParameter13 { get; set; }
        [DataMember]
        public byte ModifyParameter14 { get; set; }
        [DataMember]
        public byte ModifyParameter15 { get; set; }
        [DataMember]
        public byte ModifyParameter16 { get; set; }
        [DataMember]
        public byte ModifyParameter17 { get; set; }
        [DataMember]
        public byte ModifyParameter18 { get; set; }
        [DataMember]
        public byte ModifyParameter19 { get; set; }
        [DataMember]
        public byte ModifyParameter20 { get; set; }
        [DataMember]
        public byte ModifyParameter21 { get; set; }
        [DataMember]
        public byte ModifyParameter22 { get; set; }
        [DataMember]
        public byte ModifyParameter23 { get; set; }
        [DataMember]
        public byte ModifyParameter24 { get; set; }
        [DataMember]
        public byte ModifyParameter25 { get; set; }
        [DataMember]
        public byte ModifyParameter26 { get; set; }
        [DataMember]
        public byte ModifyParameter27 { get; set; }
        [DataMember]
        public byte ModifyParameter28 { get; set; }
        [DataMember]
        public byte ModifyParameter29 { get; set; }
        [DataMember]
        public byte ModifyParameter30 { get; set; }
        [DataMember]
        public byte ModifyParameter31 { get; set; }
        [DataMember]
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

    [DataContract]
    public class SuperNATURALDrumKitCommon
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class SuperNATURALDrumKitCommon");
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public byte KitLevel { get; set; }
        [DataMember]
        public byte AmbienceLevel { get; set; }
        [DataMember]
        public byte PhraseNumber { get; set; }
        [DataMember]
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

    //class SuperNATURALDrumKitMFX
    //{
    //    HBTrace t = new HBTrace("class SuperNATURALDrumKitMFX");
    //    public byte MFXType { get; set; }
    //    public byte MFXChorusSendLevel { get; set; }
    //    public byte MFXReverbSendLevel { get; set; }
    //    public byte[] MFXControlSource { get; set; } // [4]
    //    public byte[] MFXControlSens { get; set; }   // [4]
    //    public byte[] MFXControlAssign { get; set; } // [4]
    //    public byte[] MFXParameter { get; set; } // [32] Kolla Ã¤ven dessa!
    //    public MFXNumberedParameters MFXNumberedParameters { get; set; }

    //    public SuperNATURALDrumKitMFX(ReceivedData Data)
    //    {
    //        t.Trace("public SuperNATURALDrumKitMFX (" + "ReceivedData" + Data + ", " + ")");
    //        MFXControlSource = new byte[4];
    //        MFXControlSens = new byte[4];
    //        MFXControlAssign = new byte[4];

    //        MFXType = Data.GetByte(0x00);
    //        MFXChorusSendLevel = Data.GetByte(0x02);
    //        MFXReverbSendLevel = Data.GetByte(0x03);
    //        for (byte i = 0; i < 4; i++)
    //        {
    //            MFXControlSource[i] = Data.GetByte(0x05 + 2 * i);
    //            MFXControlSens[i] = Data.GetByte(0x06 + 2 * i);
    //            MFXControlAssign[i] = Data.GetByte(0x0d + i);
    //        }
    //        MFXNumberedParameters = new MFXNumberedParameters(Data, 0x11);
    //    }
    //}

    [DataContract]
    public class SuperNATURALDrumKitCommonCompEQ
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class SuperNATURALDrumKitCommonCompEQ");
        [DataMember]
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

    [DataContract]
    public class SuperNATURALDrumKitKey
    {
        [IgnoreDataMember]
        HBTrace t = new HBTrace("class SuperNATURALDrumKitKey");
        [DataMember]
        public byte BankNumber { get; set; } // This is 0 for Internal and 1 for ExSN6. Read more in MakeDynamicControls.cs AddSupernaturalDrumKitDruminstrumentControls()
        [DataMember]
        public UInt16 InstNumber { get; set; } // 0 - 168 iternal sound, > 168 = ExSN6 sound
        [DataMember]
        public byte Level { get; set; }
        [DataMember]
        public byte Pan { get; set; }
        [DataMember]
        public byte ChorusSendLevel { get; set; }
        [DataMember]
        public byte ReverbSendLevel { get; set; }
        [DataMember]
        public byte Tune { get; set; }
        [DataMember]
        public byte Attack { get; set; }
        [DataMember]
        public byte Decay { get; set; }
        [DataMember]
        public byte Brilliance { get; set; }
        [DataMember]
        public byte Variation { get; set; }
        [DataMember]
        public byte DynamicRange { get; set; }
        [DataMember]
        public byte StereoWidth { get; set; }
        [DataMember]
        public byte OutputAssign { get; set; }

        public SuperNATURALDrumKitKey(ReceivedData Data)
        {
            t.Trace("public SuperNATURALDrumKitNote (" + "ReceivedData" + Data + ", " + ")");
            //InstNumber = new byte[2];
            InstNumber = Data.Get3Of4Byte(0);
            if (InstNumber > 168)
            {
                BankNumber = 0x01; // This is 0 for Internal and 1 for ExSN6. Read more in MakeDynamicControls.cs AddSupernaturalDrumKitDruminstrumentControls()
                //InstNumber[0] = 0; // Because we do not know yet
                //InstNumber[1] = (byte)(temp - 169);
            }
            else
            {
                BankNumber = 0x00; // Because we do not know yet
                //InstNumber[0] = (byte)temp;
                //InstNumber[1] = 0;
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

        public GridRow(byte row, View[] controls = null, byte[] columnWiths = null, Boolean KeepAlignment = false, Boolean AddMargins = true, Int32 rowspan = 1)
        {
            t.Trace("public GridRow ()");
            try
            {
                Row = new Grid();
                Grid.SetRow(Row, row);
                //Row.MinimumHeightRequest = 40;
                Row.SetValue(Grid.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                Row.SetValue(Grid.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                Row.SetValue(Grid.ColumnSpacingProperty, 0);
                Row.SetValue(Grid.RowSpacingProperty, 0);
                Row.SetValue(Grid.PaddingProperty, new Thickness(0, 0, 0, 0));
                Row.SetValue(Grid.MarginProperty, new Thickness(0, 0, 0, 0));
                Row.SetValue(Grid.MinimumHeightRequestProperty, 1);
                Row.SetValue(Grid.MinimumWidthRequestProperty, 1);
                Grid.SetRowSpan(Row, rowspan);
                ColumnDefinition[] columnDefinitions = new ColumnDefinition[controls.Length];

                if (controls != null)
                {
                    Columns = new Grid[controls.Length];
                    for (byte i = 0; i < controls.Length; i++)
                    {
                        try
                        {
                            Row.Children.Add(controls[i]);
                        }
                        catch (Exception e)
                        {
                            GC.Collect(10, GCCollectionMode.Forced);
                            Row.Children.Add(Columns[i]);
                        }
                        columnDefinitions[i] = new ColumnDefinition();
                        if (columnWiths == null || columnWiths.Length < i - 1)
                        {
                            columnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
                        }
                        else
                        {
                            columnDefinitions[i].Width = new GridLength(columnWiths[i], GridUnitType.Star);
                        }
                        controls[i].SetValue(Grid.MinimumHeightRequestProperty, 1);
                        controls[i].SetValue(Grid.MinimumWidthRequestProperty, 1);
                        if (!KeepAlignment)
                        {
                            if (controls[i].GetType() == typeof(Button))
                            {
                                controls[i].SetValue(Button.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Button.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Button.BorderWidthProperty, new Thickness(0, 0, 0, 0));
                                controls[i].SetValue(Button.BackgroundColorProperty, UIHandler.colorSettings.Background);
                                controls[i].SetValue(Button.BorderColorProperty, UIHandler.colorSettings.Background);
                                controls[i].SetValue(Button.MarginProperty, new Thickness(0, 0, 0, 0));
                                controls[i].Parent.SetValue(Grid.VerticalOptionsProperty, controls[i].VerticalOptions);
                            }
                            else if (controls[i].GetType() == typeof(Switch))
                            {
                                controls[i].SetValue(Switch.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Switch.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Switch.BackgroundColorProperty, UIHandler.colorSettings.Background);
                                controls[i].SetValue(Switch.MarginProperty, new Thickness(0, 0, 0, 0));
                            }
                            else if (controls[i].GetType() == typeof(LabeledSwitch))
                            {
                                controls[i].SetValue(LabeledSwitch.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(LabeledSwitch.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(LabeledSwitch.BackgroundColorProperty, UIHandler.colorSettings.Background);
                                controls[i].SetValue(LabeledSwitch.MarginProperty, new Thickness(0, 0, 0, 0));
                                controls[i].SetValue(LabeledSwitch.PaddingProperty, new Thickness(0, 0, 0, 0));
                            }
                            else if (controls[i].GetType() == typeof(ListView))
                            {
                                controls[i].SetValue(ListView.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(ListView.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(ListView.BackgroundColorProperty, UIHandler.colorSettings.Background);
                                controls[i].SetValue(ListView.MarginProperty, new Thickness(0, 0, 0, 0));
                                controls[i].Parent.SetValue(Grid.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                                Row.SetValue(Grid.BackgroundColorProperty, UIHandler.colorSettings.Frame);
                            }
                            else if (controls[i].GetType() == typeof(Picker))
                            {
                                controls[i].SetValue(Picker.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Picker.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Picker.BackgroundColorProperty, UIHandler.colorSettings.Background);
                                controls[i].SetValue(Picker.MarginProperty, new Thickness(0, 0, 0, 0));
                            }
                            else if (controls[i].GetType() == typeof(LabeledPicker))
                            {
                                controls[i].SetValue(LabeledPicker.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(LabeledPicker.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(LabeledPicker.BackgroundColorProperty, UIHandler.colorSettings.Background);
                                controls[i].SetValue(LabeledPicker.MarginProperty, new Thickness(0, 0, 0, 0));
                            }
                            else if (controls[i].GetType() == typeof(Label))
                            {
                                controls[i].SetValue(Label.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Label.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Label.HorizontalTextAlignmentProperty, LayoutAlignment.Center);
                                controls[i].SetValue(Label.VerticalTextAlignmentProperty, LayoutAlignment.Center);
                                controls[i].SetValue(Label.BackgroundColorProperty, UIHandler.colorSettings.Background);
                                controls[i].SetValue(Label.MarginProperty, new Thickness(0, 0, 0, 0));
                            }
                            else if (controls[i].GetType() == typeof(Editor))
                            {
                                controls[i].SetValue(Editor.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Editor.VerticalOptionsProperty, LayoutOptions.Start);
                                controls[i].SetValue(Editor.MarginProperty, new Thickness(0, 0, 0, 0));
                            }
                            else if (controls[i].GetType() == typeof(Image))
                            {
                                controls[i].SetValue(Image.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Image.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Image.MarginProperty, new Thickness(0, 0, 0, 0));
                            }
                            else if (controls[i].GetType() == typeof(LabeledText))
                            {
                                controls[i].SetValue(LabeledText.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(LabeledText.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(LabeledText.BackgroundColorProperty, UIHandler.colorSettings.Background);
                                controls[i].SetValue(LabeledText.MarginProperty, new Thickness(0, 0, 0, 0));
                                controls[i].SetValue(LabeledText.PaddingProperty, new Thickness(0, 0, 0, 0));
                            }
                            else if (controls[i].GetType() == typeof(LabeledTextInput))
                            {
                                controls[i].SetValue(LabeledTextInput.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(LabeledTextInput.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(LabeledTextInput.BackgroundColorProperty, UIHandler.colorSettings.Background);
                                controls[i].SetValue(LabeledTextInput.MarginProperty, new Thickness(0, 0, 0, 0));
                                controls[i].SetValue(LabeledTextInput.PaddingProperty, new Thickness(0, 0, 0, 0));
                            }
                            else if (controls[i].GetType() == typeof(Grid))
                            {
                                controls[i].SetValue(Grid.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Grid.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Grid.MarginProperty, new Thickness(0, 0, 0, 0));
                                controls[i].SetValue(Grid.PaddingProperty, new Thickness(0, 0, 0, 0));
                                controls[i].SetValue(Grid.RowSpacingProperty, 0);
                                controls[i].SetValue(Grid.ColumnSpacingProperty, 0);
                            }

                            // Set margins on all controls. Then the form background color will be seen as frames around controls.
                            //if (/*AddMargins && */controls[i].GetType() != typeof(Grid))
                            if (AddMargins)
                            {
                                if (row == 0)
                                {
                                    if (i == 0)
                                    {
                                        // Top left control supplies all margins:
                                        controls[i].SetValue(View.MarginProperty, new Thickness(
                                            UIHandler.borderThicknesSettings.Size,      // Left
                                            UIHandler.borderThicknesSettings.Size,      // Top
                                            UIHandler.borderThicknesSettings.Size,      // Right
                                            UIHandler.borderThicknesSettings.Size));    // Bottom
                                    }
                                    else
                                    {
                                        // Other top controls supplies all margins but left the one:
                                        controls[i].SetValue(View.MarginProperty, new Thickness(
                                            0,                                          // Left
                                            UIHandler.borderThicknesSettings.Size,      // Top
                                            UIHandler.borderThicknesSettings.Size,      // Right
                                            UIHandler.borderThicknesSettings.Size));    // Bottom
                                    }
                                }
                                else
                                {
                                    if (i == 0)
                                    {
                                        // Non-top left controls supplies all margins but the top one:
                                        controls[i].SetValue(View.MarginProperty, new Thickness(
                                            UIHandler.borderThicknesSettings.Size,      // Left
                                            UIHandler.borderThicknesSettings.Size,      // Top
                                            0,                                          // Right
                                            UIHandler.borderThicknesSettings.Size));    // Bottom
                                    }
                                    else
                                    {
                                        // Non-top non-left controls supplies only right and bottom borders:
                                        controls[i].SetValue(View.MarginProperty, new Thickness(
                                            0,                                          // Left
                                            UIHandler.borderThicknesSettings.Size,      // Top
                                            0,                                          // Right
                                            UIHandler.borderThicknesSettings.Size));    // Bottom
                                    }
                                }
                            }
                        }
                        controls[i].SetValue(Grid.ColumnProperty, i);
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

        /// <summary>
        /// MyltiplyBytes multiplies 2 bytes with values from 0x00 to 0xff.
        /// The result is returned as an array of four MIDI data bytes, which
        /// excludes values that would need more than 4 bytes. That never occurs
        /// so this is ok.
        /// The bytes will be shifted to the left as given in offset (0 - 3),
        /// leftmost discarded and rightmost will be fillec with zeroes.
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public byte[] MultiplyBytes(byte arg1, byte arg2, byte offset = 0)
        {
            UInt32 value = (UInt32)arg1 * (UInt32)arg2;
            byte[] result = new byte[] { (byte)((value & 0x0fe00000) >> 21), (byte)((value & 0x001fc000) >> 14), (byte)((value & 0x003f80) >> 7), (byte)(value & 0x00007f) };
            for (byte i = 0; i < offset; i++)
            {
                result[0] = result[1];
                result[1] = result[2];
                result[2] = result[3];
                result[3] = 0x00;
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
    public class ReceivedData
    {
        HBTrace t = new HBTrace("class ReceivedData");
        public byte[] RawData { get; set; }

        public ReceivedData(byte[] RawData)
        {
            //t.Trace("public ReceivedData (" + "byte[]" + RawData + ", " + ")");
            this.RawData = RawData;
        }

        // Use when you have 2 bytes for where class object resides in parent class, and you have an offset into the current class:
        public byte GetByte(byte msb, byte lsb, byte offset)
        {
            //t.Trace("public byte GetByte (" + "byte" + msb + ", " + "byte" + lsb + ", " + "byte" + offset + ", " + ")");
            UInt16 Index = (UInt16)(128 * msb + lsb + offset);
            if (Index < RawData.Length - 11)
            {
                return RawData[Index + 11];
            }
            else
            {
                return 0xff;
            }
        }

        public byte GetByte(Int32 Index)
        {
            //t.Trace("public byte GetByte (" + "Int32" + Index + ", " + ")");
            // NOTE!
            // Addressing does NOT use msb. Index larger than 0x7f will start on new page, thus 0x00, 0x7f + 1 = 0x01, 0x00
            // Msb is never larger than 0x01, so math can be simplified.
            if (Index / 0x100 > 0)
            {
                Index -= 0x80;
            }

            if (Index < RawData.Length - 11 && Index > -1)
            {
                return RawData[Index + 11];
            }
            else
            {
                return 0xff;
            }
        }

        // Use for non-numbered parameters where two address bytes (nibbles actually) are given _in sequence_ (not as msb + lsb).
        // Those addresses are marked with a # in the MIDI implementation chart.
        public byte Get2Byte(Int32 Index)
        {
            //t.Trace("public byte Get2Byte (" + "Int32" + Index + ", " + ")");
            if (Index < RawData.Length - 12 && Index > -1)
            {
                return (byte)(16 * RawData[Index + 11] + RawData[Index + 12]);
            }
            else
            {
                return 0xff;
            }
        }

        // Use for numbered parameters. Actual address always points to the first of the four bytes,
        // but the functions gets bytes 3-4, 2-4 and 1-4 respectively.
        public byte Get2Of4Byte(Int32 Index)
        {
            //t.Trace("public byte Get2Of4Byte (" + "Int32" + Index + ", " + ")");
            if (Index < RawData.Length - 14 && Index > -1)
            {
                return (byte)(16 * RawData[Index + 13] + RawData[Index + 14]);
            }
            else
            {
                return 0xff;
            }
        }

        public UInt16 Get3Of4Byte(Int32 Index)
        {
            //t.Trace("public byte Get3Of4Byte (" + "Int32" + Index + ", " + ")");
            if (Index < RawData.Length - 14 && Index > -1)
            {
                return (UInt16)(16 * 16 * RawData[Index + 12] + 16 * RawData[Index + 13] + RawData[Index + 14]);
            }
            else
            {
                return 0xffff;
            }
        }

        public UInt16 Get4Byte(Int32 Index)
        {
            //t.Trace("public UInt16 Get4Byte (" + "Int32" + Index + ", " + ")");
            if (Index < RawData.Length - 14 && Index > -1)
            {
                return (UInt16)(16 * 16 * 16 * RawData[Index + 11] + 16 * 16 * RawData[Index + 12] + 16 * RawData[Index + 13] + RawData[Index + 14]);
            }
            else
            {
                return 0xffff;
            }
        }
    }
}
