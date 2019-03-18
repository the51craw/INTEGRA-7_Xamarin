using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integra_7_Xamarin
{
    #region help

    /// <summary>
    /// Help texts should be available for most controls. Inserting them should not upset indexing and cause a shift among help texts.
    /// Each tone type has a default description help item that is shown in case a control does not have a help item, and when edit
    /// page is navigated to.
    /// At top level there is a list of lists. It has 6 entries, 5 for the different tone types and one for MFX help items.
    /// The five entries for the different tone types has one list for each control page (Common, Wave etc).
    /// Each of those entries has a list with one or more entries.
    /// Those entries are the actual help items.
    /// ComboBox controls can have more than one such entry, each corresponding to a selected item in the ComboBox, giving help on each selected item.
    /// Other types of controls can only have one such entry.
    /// 
    /// All pages in Edit has a set of common controls that are always shown. Those are declaired in Edit.xaml and has dedicated GotFocus handlers to show their help items.
    /// The Grid ControlsGrid in Edit.xaml is populated dynamically for each Tone Type and selected parameter page.
    /// Help items must occur in the same order in the lists as they are created.
    /// All those controls GotFocus use the same handler, Generic_GotFocus.
    /// They all carry a Tag with the same controlsIndex as the corresponding help item.
    /// Therefore, all controls in a page start controlsIndex from zero and count up per control,
    /// and all help items are created the same way.
    /// So, controlsIndex is the third list index.
    /// First tone type (PCM Synth Tone) has list number zero.
    /// First control page has the first list number in its list number.
    /// First control in a control page has the first control number in its page number in its list number.
    /// BUT! Since the default page description and the common controls take up space, all dynamic controls are shifted by means of an offset in Skip.
    /// Skip is calculated to allow for changes regarding the common controls.
    /// 
    /// MFX control help items resides in the last top-level list.
    /// MFX help items does not need to use the offset in Skip since the main help texts and common controls help text is not in this list.
    /// 
    /// Examples:
    /// 0,0,0,0  = default text for PCM Synth Tone
    /// 0,0,1,0  = text for the Part selector
    /// 0,0,2,0  = text for the Tone category selector
    /// 0,0,9,0  = text for the Return button
    /// 0,0,10,0 = text for the first dynamically added control on the Common control page of PCM Synth Tone
    /// 0,1,10,0 = text for the first dynamically added control on the Wave control page of PCM Synth Tone
    /// 1,0,11,0 = text for the second dynamically added control on the Wave control page of SuperNATURAL Acoustic Tone
    /// 5,0,0,0  = text for the first MFX parameter on the first MFX Type
    /// </summary>
    class Help
    {
        public List<List<List<List<HelpItem>>>> HelpItems;
        public RowDefinition rdHelpHeader;
        public RowDefinition rdHelpHeaderImage;
        public RowDefinition rdHelpText;
        public RowDefinition rdHelpImage;
        public Grid HelpGrid;
        public Label tbEditToneHelpsHeading;
        public Image imgEditToneHeadingImage;
        public Label tbEditToneHelpsText;
        public Image imgEditToneImage;
        public UInt16 ItemIndex { get; set; }
        public UInt16 SubItemIndex { get; set; }
        //public byte ParameterPagesCount { get; set; }
        private EditToneParameterPageItems editToneParameterPageItems;
        public byte Skip { get; set; } // Static controls occupy Skip number of help texts. When doing the dynamic controls (not the MFX however), skip those.

        public Help(Label tbEditToneHelpsHeading, Image imgEditToneHeadingImage, Label tbEditToneHelpsText, Image imgEditToneImage) //, Int32 ParameterPagesCount)
        {
            HelpItems = new List<List<List<List<HelpItem>>>>();
            // Create top level menuitems for the 5 tone types and one for MFX controls:
            for (byte i = 0; i < 6; i++)
            {
                HelpItems.Add(new List<List<List<HelpItem>>>());
            }
            this.tbEditToneHelpsHeading = tbEditToneHelpsHeading;
            this.imgEditToneHeadingImage = imgEditToneHeadingImage;
            this.tbEditToneHelpsText = tbEditToneHelpsText;
            this.imgEditToneImage = imgEditToneImage;
            editToneParameterPageItems = new EditToneParameterPageItems();
            ItemIndex = 0;
            SubItemIndex = 0;
            Init();
        }

        public void Add(byte ToneType, byte Page, byte Item, byte SubItem, String Heading, String HeadingImage = null, String Text = null, String Image = null, UInt16 SpaceAssign = 0x10d0)
        {
            if (HelpItems.Count <= ToneType)
            {
                for (Int32 i = HelpItems.Count(); i < ToneType ; i++)
                {
                    HelpItems.Add(new List<List<List<HelpItem>>>());
                }
            }
            if (HelpItems[ToneType].Count() <= Page)
            {
                for (Int32 i = HelpItems[ToneType].Count(); i <= Page; i++)
                {
                    HelpItems[ToneType].Add(new List<List<HelpItem>>());
                }
            }
            if (HelpItems[ToneType][Page].Count() <= Item)
            {
                for (Int32 i = HelpItems[ToneType][Page].Count(); i <= Item; i++)
                {
                    HelpItems[ToneType][Page].Add(new List<HelpItem>());
                }
            }
            HelpItems[ToneType][Page][Item].Add(new HelpItem(this, Heading, HeadingImage, Text, Image, SpaceAssign));
        }

        public HelpItem Show(byte ToneType, byte Page, byte Item, byte SubItem)
        {
            try
            {
                if (HelpItems != null)
                {
                    if (HelpItems.Count() > ToneType && HelpItems[ToneType] != null)// && HelpItems[ToneType].Count() > Page)
                    {
                        if (HelpItems[ToneType].Count() > Page && HelpItems[ToneType][Page] != null)// && HelpItems[ToneType][Page].Count() > Item)
                        {
                            if (HelpItems[ToneType][Page].Count() > Item && HelpItems[ToneType][Page][Item] != null)// && HelpItems[ToneType][Page][Item].Count() > SubItem)
                            {
                                if (HelpItems[ToneType][Page][Item].Count() > SubItem && HelpItems[ToneType][Page][Item][SubItem] != null)
                                {
                                    return HelpItems[ToneType][Page][Item][SubItem].Show();
                                }
                                else
                                {
                                    if (HelpItems[ToneType][Page][Item][0] != null)
                                    {
                                        return HelpItems[ToneType][Page][Item][0].Show();
                                    }
                                }
                            }
                            else
                            {
                                if (HelpItems[ToneType][Page][0][0] != null)
                                {
                                    return HelpItems[ToneType][Page][0][0].Show();
                                }
                            }
                        }
                        else
                        {
                            if (HelpItems[ToneType][0][0][0] != null)
                            {
                                return HelpItems[ToneType][0][0][0].Show();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            { }
            return null;
        }

        String Warning = "\r\n\r\nNOTE! The INTEGRA-7 will not report current settings if you change part," +
                " tone type or MFX type. This app will use default settings instead.\r\n" +
            "If you want to use current settings rather than default settings, go back to main page (tap" +
                " return button), change part and/or tone and/or MFX type on the INTEGRA-7 " +
            "front panel, and then go to Edit Sound again.";
        List<String> commonStrings;
        List<String> mainPageDescriptions;

        private void Init()
        {
            // Strings for controls that are always shown (not depending on parameter page selection)
            commonStrings = new List<String>();
            commonStrings.Add("PCM synth tone");
            commonStrings.Add("PCM drum kit");
            commonStrings.Add("SuperNATURAL acoustic tone");
            commonStrings.Add("SuperNATURAL synth tone");
            commonStrings.Add("SuperNATURAL drum kit");
            commonStrings.Add("The part selector selects what part you want to work in. You will edit" +
                " the sound currently selected for that part." + Warning);
            commonStrings.Add("The tone type selector selects what type of tone you want to use: " +
                "PCM synth tone/PCM drum kit/SuperNATURAL acoustic tone/SuperNATURAL synth tone/SuperNATURAL drum kit. " +
                "You will edit the sound currently selected for that part." + Warning);
            commonStrings.Add("The page selector selects what parameter page you want to work with. They correspond" +
                " to the tabs in INTEGRA-7 edit screen.");
            commonStrings.Add("Tones can combine up to 4 different sounds, called partials. The partial selector" +
                " selects what partial you are currently working on.");
            commonStrings.Add("This is the name of the sound played for the selected key.");
            commonStrings.Add("Drum kits can have one tone per keyboard key. The key selector selects what key" +
                " (note number) you are currently working on.");
            commonStrings.Add("The Tone Category selector selects the type of instrument, like a sound group." +
                " This is mostly informative but will change the setting in INTEGRA-7.\r\n\r\n" +
                "NOTE\r\nThe sound listings on the main page is based on the official sound list. If you change" +
                " Tone Category for a preset sound, it will " +
                "not reflect in the main page.");
            commonStrings.Add("Sets chorus send level for all MFX types.");
            commonStrings.Add("Sets reverb send level for all MFX types.");
            commonStrings.Add("Starts or stops sample music (phrase).");
            commonStrings.Add("Reset INTEGRA-7 and the editor to the initial state for the current tone type." + 
                "\r\n\r\nThis is the same as \'PART INIT\' in the INTEGRA-7 menu.");
            commonStrings.Add("Returns you to the librarian (the main page).");

            // Strings describing tone types. These are shown when Edit opens and when clicking on help.
            mainPageDescriptions = new List<String>();
            mainPageDescriptions.Add("These are sounds that were called \'patches\' on" +
                " previous Roland synthesizers.\r\n\r\n" +
                "They have been tuned specifically for the INTEGRA-7. One PCM synth tone can consist of up to" +
                " four partials(waves).\r\n\r\n" +
                "Each tone has settings for four sets (Partial 1–4) of WAVE, TVF, TVA, and LFO×2," +
                " in addition to multi-effect (MFX) settings.\r\n" +
                "\r\n" +
                "You can create sounds by combining four partials.\r\n" +
                "\r\n" +
                "Each partial can be turned on/off, allowing you to specify which partial (s) wil" +
                "l be heard.\r\n");
            mainPageDescriptions.Add("These were called \'rhythm sets\' on previous Roland" +
                " synthesizers.\r\n\r\n" +
                "They have been tuned specifically for the INTEGRA-7. A drum kit is a group of multiple percussion" +
                " instrument sounds assigned so that a different " +
                "percussion instrument will be heard depending on the key (note number) you play." +
                "\r\n\r\n" +
                "Each kit has 88 sets (Partial 1–88) of WAVE, TVF, and TVA settings, in addition " +
                "to multi-effect (MFX) settings.\r\n" +
                "\r\n" +
                "Each partial has four wave generators. You can assign a different note number th" +
                "at will sound each of the 88 partials.\r\n\r\n" +
                "For the one part specified by the Drum Comp+EQ Assign setting, you’ll be able to" +
                " use six sets of compressor + equalizer units to make the sound more c" +
                "onsistent or to adjust the tonal character.");
            String superNaturalDescription = "The SuperNATURAL sounds has special enhancements to make you play" +
                " like you play on the actual instrument. " +
                "Read more in the manual or see demonstrations on youtube.com.";
            mainPageDescriptions.Add(superNaturalDescription + "\r\n\r\n" +
                "The acoustic tones are based on recorded samples of actual instruments." +
                "\r\n" +
                "\r\n" +
                "For each tone, there are instrument settings (INST) and multi-effect settings (M" +
                "FX).\r\n" +
                "\r\n" +
                "The instrument settings let you make settings for the tone and its parameters.\r\n");
            mainPageDescriptions.Add(superNaturalDescription + "\r\n\r\n" +
                "The synth tones are based on recorded samples of syntesized instruments" +
                "\r\n" +
                "\r\n" +
                "Each tone has three sets (Partial 1–3) of OSC, FILTER, AMP, and LFO settings, in" +
                " addition to multi-effect (MFX) settings.\r\n");
            mainPageDescriptions.Add(superNaturalDescription + "\r\n\r\n" +
                "The drum kits are based on recorded drums, sounds and synthetsized sound effects." +
                "\r\n" +
                "\r\n" +
                "Each kit has 62 sets (Partial 1–62) of Drum instrument, Compressor and Equalizer, in addition " +
                "to multi-effect (MFX) settings.\r\n" +
                "\r\n" +
                "Each partial has one wave generator.\r\n" +
                "\r\n" +
                "For the one part specified by the Drum Comp+EQ Assign setting, you’ll be able to" +
                " use six sets of compressor + equalizer units to make the sound more c" +
                "onsistent or to adjust the tonal character\r\n");

            // Images shown when edit opens:
            List<String> mainPageImages = new List<String>();
            mainPageImages.Add("_PCM_01.png");
            mainPageImages.Add("_PCM-D_01.png");
            mainPageImages.Add("_SN-A_01.png");
            mainPageImages.Add("_SNS_01.png");
            mainPageImages.Add("_SN-D_01.png");

            // Layouts used when showing first help page:
            List<UInt16> mainPageLayouts = new List<UInt16>();
            mainPageLayouts.Add(0x1670);
            mainPageLayouts.Add(0x1670);
            mainPageLayouts.Add(0x1670);
            mainPageLayouts.Add(0x1670);
            mainPageLayouts.Add(0x1670);

            String matrixOption = "\r\nThis parameter can be controlled using the Matrix Control.";

            // Main pages default descriptions. Displayed when entering page and when no description is available for a focused control:
            for (byte i = 0; i < 5; i++)
            {
                Add(i, 0, 0, 0, commonStrings[i], mainPageImages[i], mainPageDescriptions[i], "", mainPageLayouts[i]);
            }

            // Main page controls (those always displayed for all tone types, declaired in Edit.xaml):
            byte[] controlsCount = new byte[5];
            controlsCount[0] = (byte)editToneParameterPageItems.ParameterPageItems(ProgramType.PCM_SYNTH_TONE).Count();
            controlsCount[1] = (byte)editToneParameterPageItems.ParameterPageItems(ProgramType.PCM_DRUM_KIT).Count();
            controlsCount[2] = (byte)editToneParameterPageItems.ParameterPageItems(ProgramType.SUPERNATURAL_ACOUSTIC_TONE).Count();
            controlsCount[3] = (byte)editToneParameterPageItems.ParameterPageItems(ProgramType.SUPERNATURAL_SYNTH_TONE).Count();
            controlsCount[4] = (byte)editToneParameterPageItems.ParameterPageItems(ProgramType.SUPERNATURAL_DRUM_KIT).Count();
            for (byte i = 0; i < 5; i++) // For each tone type (PCM Synth Tone, PCM Drum Set, SuperNATURAL...) May be that all will end up the same, but anyway...
            {
                for (byte j = 0; j < controlsCount[i]; j++) // For each control page
                {
                    for (byte k = 0; k < commonStrings.Count - 5; k++) // For each help item 
                    {
                        //if (i == 0 && )
                        Add(i, j, (byte)(k + 1), 0, commonStrings[i], "", commonStrings[k + 5]); // Last index is 0 because sub-items are not available among main page controls.
                    }
                }
            }

            // All dynamically generated controls:
            Skip = (byte)this.HelpItems[0][0].Count();

            // PCM Synth Tone Common tab
            ItemIndex = Skip;
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Pressing and holding the volume control on the" +
                " INTEGRA-7 will make it play a phrase. You can select what phrase will be played here. " +
                "Default is a phrase specifically created for the current tone.\r\n\r\nYou can use the \'Play\'" +
                " button below to play/stop the selected phrase.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Pressing and holding the volume control on the" +
                " INTEGRA-7 will make it play a phrase. Here you may transpose the phrase to another octave." +
                "\r\n\r\nYou can use the \'Play\' button below to play/stop the selected phrase.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "This slider lets you set a default tone level" +
                " for the current tone you are editing.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "This slider lets you pan the current tone.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Tone priority determines how notes will be managed" +
                " when the maximum polyphony is exceeded (128 tones).\r\n\r\nLAST: The last-played tones will be given priority, " +
                "and currently sounding notes will be turned off in order, beginning with the first-played note. " +
                "\r\n\r\nLOUDEST: The tones with the loudest volume will be given priority, and currently sounding notes " + 
                "will be turned off, beginning with the lowest-volume tone.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Transpose current tone up or down 1 to 3 octaves.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "This setting allows you to apply \'stretched tuning\'" +
                " to the patch. (Stretched tuning is a system by which acoustic pianos are normally tuned, " +
                "causing the lower range to be lower and the higher range to be higher than the mathematical tuning" +
                " ratios would otherwise dictate.)\r\n\r\n" +
                "With a setting of \'OFF,\' the patch\'s tuning will be equal temperament. A setting of \'3\' will" +
                " produce the greatest difference in the pitch of the low and high ranges.\r\n\r\n" +
                "The diagram shows the pitch change relative to equal temperament that will occur in the low and high" +
                " ranges.This setting will have a subtle effect on the way in which chords resonate.", "_PCM_Common_01.png", 0x1075);
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Here you can fine tune the pitch within +/- 1/12 of" +
                " an octave (half tone step).");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Here you can coarse tune the pinch in half tone steps.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Specifies the depth of 1/f modulation that is to be" +
                " applied to the tone. (1/f modulation is a pleasant and naturally - occurring ratio of modulation " +
                "that occurs in a babbling brook or rustling wind.)\r\n\r\nBy adding this \'1 / f modulation,\' you can" +
                " simulate the natural instability characteristic of an analog synthesizer.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Cutoff Frequency Offset alters the cutoff frequency" +
                " of the overall tone, while preserving the relative differences between the cutoff " +
                "frequency values set for each partial in the Cutoff Frequency parameters.\r\n\r\nNOTE:\r\nThis value" +
                " is added to the cutoff frequency value of a partial, so if the cutoff " +
                "frequency value of any partial is already set to 127 (maximum), positive settings here will not produce" +
                " any change.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Resonance Offset alters the resonance of the overall" +
                " tone, while preserving the relative differences between the resonance values set " +
                "for each partial in the Resonance parameter.\r\n\r\nResonance: emphasizes the overtones in the region of" +
                " the cutoff frequency, adding character to the sound. " + matrixOption +
                "\r\n\r\nNOTE\r\nThis value is added to the resonance value of a partial, so if the resonance value of any" +
                " partial is already set to 127 (maximum), positive settings here will not produce any change.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Attack Time Offset alters the attack time of the overall" +
                " tone, while preserving the relative differences between the attack time values " +
                "set for each partial in the TVA Env Time 1 parameters and TVF Env Time 1 parameters. \r\n\r\nAttack Time:" +
                " The time it takes for a sound to reach maximum volume after the key is pressed " +
                "and sound begun." + matrixOption + "\r\n\r\nNOTE\r\nThis value is added to the attack time value of a partial," +
                " so if the attack time value of any partial is already set to 127 (maximum), " +
                "positive settings here will not produce any change.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Release Time Offset alters the release time of the overall" +
                " tone, while preserving the relative differences between the release time values set " +
                "for each partial in the TVA Env Time 4 parameters and TVF Env Time 4 parameters. Release Time: The time from" +
                " when you take your finger off the key until the sound disappears.\r\n\r\n" + matrixOption +
                "\r\n\r\nNOTE\r\nThis value is added to the release time value of a partial, so if the release time value of" +
                " any partial is already set to 127 (maximum), positive settings here will not produce any change.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Velocity Sensitivity Offset alters the Velocity Sensitivity" +
                " of the overall tone while preserving the relative differences between the Velocity " +
                "Sensitivity values set for each partial in the parameters Cutoff Velocity Sens and Level Velocity Sens.\r\n" +
                "Velocity: Pressure with which the key is pressed. " + matrixOption +
                "\r\n\r\nNOTE\r\n This value is added to the velocity sensitivity value of a partial, so if the velocity" +
                " sensitivity value of any partial is already set to +63 (maximum), positive settings here will not produce any change.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Specifies whether the tone will play polyphonically" +
                " (POLY) or monophonically (MONO). The \'MONO\' setting is effective when playing a solo instrument patch such as sax or flute, " +
                "but may be changed here to simulate playing more than one solo instrument." + 
                "\r\n\r\nMONO: Only the last - played note will sound.\r\n\r\nPOLY: Two or more notes can be played simultaneously.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Use Legato in combination with Mono. If you play" +
                " legato, i.e. do not release previous key before playing next key, " +
                "Threr will be no attack sound, but rather sounds like real legato play. If you release previous key" +
                " before playing the next one, an attack sound will be produced as normal.\r\n\r\n" +
                "Note that this only works in Mono mode!\r\n\r\nAlso note that for some instruments, like a guitarr," +
                " it is not possible to do legato to a lower tone, like you can with e.g. a flute.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Legato play uses pitch shift, not a new key number." +
                " Thus there is always a chanse that the requested legato shift is greater " +
                "than the available pitch shift, and the legato will stop at a lower pitch than intended. If you rather" +
                " like to have a new attack (new key down) in this situation, use Legato Retrigger.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Portamento makes the tone slide from one key to" +
                " the next pressed key. This works best in Mono mode.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "Portamento may be applied always, or only when" +
                " playing legato.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "The time it takes to slide from one tone to next" +
                " tone may be either constant or depending on the distance between the keys.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "If a portamento slide is on-going when you press" +
                " a new key, you can here decide whether to start the new portamento slide " +
                "from current pitch or from the pitch current portamento should have ended with.");
            Add(0, 0, (byte)ItemIndex++, 0, "PCM synth tone", "", "This slider sets how long time a portamento slide takes.");
            // PCM Synth Tone Wave tab
            ItemIndex = Skip;
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "Selects the group for the waveform that is to be the basis of the partial.\r\n" +
                "\r\n" +
                "INT: Waveforms stored in internal bank.\r\n" +
                "SRX01–SRX12: Expansion sound banks.\r\n" +
                "\r\n" +
                "Do not forget to load any SRXnn expansion module you use.\r\n", "", 0x00e0);
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "Sets the gain (amplification) of the waveform.\r\n" +
                "\r\n" +
                "The value changes in 6 dB (decibel) steps—an increase of 6 dB doubles the wavefo" +
                "rm’s gain.\r\n" +
                "\r\n" +
                "If you intend to use the Booster to distort the waveform’s sound, set this param" +
                "eter to its maximum value (p. 53).\r\n", "", 0x00e0);
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "Selects the basic waveform for a tone, left side or mono.\r\n" +
                "(Along with the Wave number, the Wave name appears at the lower part of the disp" +
                "lay on your INTEGRA-7).\r\n" +
                "\r\n" +
                "When in monaural mode, only the left side (L) is specified. When in stereo, the " +
                "right side (R) is also specified.\r\n" +
                "\r\n" +
                "NOTE:\r\n" +
                "If you specify only the right side (R), there will be no sound!\r\n", "", 0x00e0);
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "Optionally selects the basic waveform for a tone, right side. Use only in stereo" +
                " mode.\r\n" +
                "(Along with the Wave number, the Wave name appears at the lower part of the disp" +
                "lay on your INTEGRA-7).\r\n" +
                "\r\n" +
                "When in monaural mode, only the left side (L) is specified. When in stereo, the " +
                "right side (R) is also specified.\r\n" +
                "\r\n" +
                "NOTE:\r\n" +
                "If you specify only the right side (R), there will be no sound!\r\n", "", 0x00e0);
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "When you wish to synchronize a Phrase Loop to the clock (tempo), set this to “ON" +
                ".”\r\n" +
                "\r\n" +
                "This is valid only when an SRX waveform which indicates a tempo (BPM) is selecte" +
                "d.\r\n" +
                "(Example: SRX05 3:080:BladeBtL, SRX08 5:75:BoomRvBel, etc.)\r\n" +
                "\r\n" +
                "If a waveform from an SRX is selected for the partial, turning the Wave Tempo Sy" +
                "nc parameter “ON” will cause pitch-related settings and FXM-related se" +
                "ttings to be ignored.\r\n" +
                "\r\n" +
                "When the Wave Tempo Sync is set to “ON,” set the Partial Delay Time (p. 51) to “" +
                "0.”\r\n" +
                "\r\n" +
                "With other settings, a delay effect will be applied, and you will be not be able" +
                " to play as you expect.\r\n" +
                "\r\n" +
                "Phrase Loop:\r\n" +
                "“Phrase” loop refers to the repeated playback of a phrase that’s been pulled out" +
                " of a song (e.g., by using a sampler). One technique involving the use" +
                " of Phrase Loops is the excerpting of a Phrase from a pre-existing son" +
                "g in a certain genre, for example dance music, and then creating a new" +
                " song with that Phrase used as the basic motif. This is referred to as" +
                " “Break Beats.”\r\n", "", 0x00e0);
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "This sets whether FXM will be used (ON) or not (OFF).\r\n" +
                "\r\n" +
                "FXM (Frequency Cross Modulation) uses a specified waveform to apply frequency mo" +
                "dulation to the currently selected waveform, creating complex overtone" +
                "s. This is useful for creating dramatic sounds or sound effects.\r\n", "", 0x00e0);
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies how FXM will perform frequency modulation.\r\n" +
                "\r\n" +
                "Higher settings result in a grainier sound, while lower settings result in a mor" +
                "e metallic sound.\r\n", "", 0x00e0);
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies the depth of the modulation produced by FXM.\r\n" +
                "\r\n" +
                "* You can use matrix control to modify this.\r\n", "", 0x00e0);
            Add(0, 1, (byte)ItemIndex, 0, "Partial Delay.\r\n" +
                "This produces a time delay between the moment a key is pressed (or released), an" +
                "d the moment the partial actually begins to sound. You can also make s" +
                "ettings that shift the timing at which each partial is sounded.\r\n" +
                "\r\n" +
                "This differs from the Delay in the internal effects, in that by changing the sou" +
                "nd qualities of the delayed partials and changing the pitch for each p" +
                "artial, you can also perform arpeggio-like passages just by pressing o" +
                "ne key.\r\n" +
                "\r\n" +
                "You can also synchronize the partial delay time to the tempo of the external MID" +
                "I sequencer.\r\n" +
                "\r\n" +
                "NOTE:\r\n" +
                "If you don’t wish to use Partial Delay, set Partial Delay Mode to “NORM” and Par" +
                "tial Delay Time to “0.”\r\n" +
                "\r\n" +
                "• If the Structure Type set in the range of “2”–”10,” the output of partial 1 an" +
                "d 2 will be combined into partial 2, and the output of partial 3 and 4" +
                " will be combined into partial 4. For this reason, partial 1 will foll" +
                "ow the settings of partial 2, and partial 3 will follow the settings o" +
                "f partial 4 (p. 52).", "_PCM_Wave_01.png",
                "The partial begins to play after the time specified in the Partial Delay Time pa" +
                "rameter has elapsed.", "", 0x9320);
            Add(0, 1, (byte)ItemIndex, 1, "Partial Delay.\r\n" +
                "This produces a time delay between the moment a key is pressed (or released), an" +
                "d the moment the partial actually begins to sound. You can also make s" +
                "ettings that shift the timing at which each partial is sounded.\r\n" +
                "\r\n" +
                "This differs from the Delay in the internal effects, in that by changing the sou" +
                "nd qualities of the delayed partials and changing the pitch for each p" +
                "artial, you can also perform arpeggio-like passages just by pressing o" +
                "ne key.\r\n" +
                "\r\n" +
                "You can also synchronize the partial delay time to the tempo of the external MID" +
                "I sequencer.\r\n" +
                "\r\n" +
                "NOTE:\r\n" +
                "If you don’t wish to use Partial Delay, set Partial Delay Mode to “NORM” and Par" +
                "tial Delay Time to “0.”\r\n" +
                "\r\n" +
                "• If the Structure Type set in the range of “2”–”10,” the output of partial 1 an" +
                "d 2 will be combined into partial 2, and the output of partial 3 and 4" +
                " will be combined into partial 4. For this reason, partial 1 will foll" +
                "ow the settings of partial 2, and partial 3 will follow the settings o" +
                "f partial 4 (p. 52).", "_PCM_Wave_02.png",
                "Although the partial begins to play after the time specified in the Partial Dela" +
                "y Time parameter " +
                "has elapsed, if the key is released before the time specified in the Partial Del" +
                "ay Time parameter has " +
                "elapsed, the partial is not played.", "", 0x9320);
            Add(0, 1, (byte)ItemIndex, 2, "Partial Delay.\r\n" +
                "This produces a time delay between the moment a key is pressed (or released), an" +
                "d the moment the partial actually begins to sound. You can also make s" +
                "ettings that shift the timing at which each partial is sounded.\r\n" +
                "\r\n" +
                "This differs from the Delay in the internal effects, in that by changing the sou" +
                "nd qualities of the delayed partials and changing the pitch for each p" +
                "artial, you can also perform arpeggio-like passages just by pressing o" +
                "ne key.\r\n" +
                "\r\n" +
                "You can also synchronize the partial delay time to the tempo of the external MID" +
                "I sequencer.\r\n" +
                "\r\n" +
                "NOTE:\r\n" +
                "If you don’t wish to use Partial Delay, set Partial Delay Mode to “NORM” and Par" +
                "tial Delay Time to “0”.\r\n" +
                "\r\n" +
                "• If the Structure Type set in the range of “2”–”10,” the output of partial 1 an" +
                "d 2 will be combined into partial 2, and the output of partial 3 and 4" +
                " will be combined into partial 4. For this reason, partial 1 will foll" +
                "ow the settings of partial 2, and partial 3 will follow the settings o" +
                "f partial 4 (p. 52).", "_PCM_Wave_03.png",
                "Rather than being played while the key is pressed, the partial begins to play on" +
                "ce the period of" +
                "time specified in the Partial Delay Time parameter has elapsed after release of " +
                "the key. This is" +
                "effective in situations such as when simulating noises from guitars and other in" +
                "struments.", "", 0x9320);
            Add(0, 1, (byte)ItemIndex++, 3, "Partial Delay.\r\n" +
                "This produces a time delay between the moment a key is pressed (or released), an" +
                "d the moment the partial actually begins to sound. You can also make s" +
                "ettings that shift the timing at which each partial is sounded.\r\n" +
                "\r\n" +
                "This differs from the Delay in the internal effects, in that by changing the sou" +
                "nd qualities of the delayed partials and changing the pitch for each p" +
                "artial, you can also perform arpeggio-like passages just by pressing o" +
                "ne key.\r\n" +
                "\r\n" +
                "You can also synchronize the partial delay time to the tempo of the external MID" +
                "I sequencer.\r\n" +
                "\r\n" +
                "NOTE:\r\n" +
                "If you don’t wish to use Partial Delay, set Partial Delay Mode to “NORM” and Par" +
                "tial Delay Time to “0.”\r\n" +
                "\r\n" +
                "• If the Structure Type set in the range of “2”–”10,” the output of partial 1 an" +
                "d 2 will be combined into partial 2, and the output of partial 3 and 4" +
                " will be combined into partial 4. For this reason, partial 1 will foll" +
                "ow the settings of partial 2, and partial 3 will follow the settings o" +
                "f partial 4 (p. 52).", "_PCM_Wave_04.png",
                "Rather than being played while the key is pressed, the partial begins to play on" +
                "ce the period of time specified in the Partial Delay Time parameter ha" +
                "s elapsed after release of the key. Here, however, changes in the TVA " +
                "Envelope begin while the key is pressed, which in many cases means tha" +
                "t only the sound from the release portion of the envelope is heard.", "", 0x9230);
            Add(0, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies the time from when the key is pressed (or if the Partial Delay Mode pa" +
                "rameter is set to “OFF-N” or “OFF-D,” the time from when the key is re" +
                "leased) until when the partial will sound.\r\n" +
                "\r\n" +
                "If you want the time until the partial sounds to be synchronized with the tempo," +
                " specify the time as a note value relative to the synchronization temp" +
                "o.\r\n" +
                "\r\n" +
                "(Example) For a tempo of 120 (120 quarter notes occur in 1 minute (60 seconds)) " +
                "a quarter not will take half a second.\r\n" +
                "\r\n" +
                "(Example) For a tempo of 60 (60 quarter notes occur in 1 minute (60 seconds)) a " +
                "quarter not will take one second.\r\n" +
                "\r\n" +
                "Formula: 60 * 4 * Note length / Tempo seconds\r\n" +
                "or: 240 * Note length / Tempo seconds\r\n", "", 0x00e0);
            // PCM Synth Tone PMT tab
            ItemIndex = Skip;
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "Selects what partial you are working on.\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "PMT Velocity Control determines whether a different partial is played (ON) or no" +
                "t (OFF) depending on the force with which the key is played (velocity)" +
                ".\r\n" +
                "\r\n" +
                "When set to “RANDOM,” the tone’s constituent partials will sound randomly, regar" +
                "dless of any Velocity messages.\r\n" +
                "When set to “CYCLE,” the tone’s constituent partials will sound consecutively, r" +
                "egardless of any Velocity messages.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "Use “Velo Range Lower” (p. 54) and “Velo Range Upper” (p. 54) to specify the ran" +
                "ge of keyboard dynamics.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "• If Velocity Range Lower and Velocity Range Upper are set to the same values, y" +
                "ou won’t be able to obtain any effect by setting PMT Velocity Control " +
                "to “RANDOM” or “CYCLE.”\r\n" +
                "* Instead of using Velocity, you can also have partials substituted using the Ma" +
                "trix Control (p. 63). \r\n" +
                "However, the keyboard velocity and the Matrix Control cannot be used simultaneou" +
                "sly to make different partials to sound. When using the Matrix Control" +
                " to switch partials, set the Velocity Control parameter to “OFF.”\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "", "Use the Matrix Control (p. 63) to enable (ON), or disable(OFF) sounding of different partials.\r\n\r\n" +
                "NOTE\r\n" +
                "You can also cause different partials to sound in response to notes played at different strengths" +
                "(velocity) on the keyboard(p. 53). However, the Matrix Control and the keyboard velocity cannot" +
                " be used simultaneously to make different partials to sound.\r\n\r\n" +
                "When using the Matrix Control to have different partials to sound, set the Velocity Control" +
                "parameter(p. 53) to “OFF.”", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex, 0, "Determines how partial 1 and 2 are connected.\r\n", "_PMT_PMT_01.png",
                "With this type, partial 1 and 2 are independent. Use this type when" +
                " you want to preserve PCM sounds or create and combine sounds for each" +
                " partial.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 1, "Determines how partial 1 and 2 are connected.\r\n", "_PMT_PMT_02.png",
                "This type stacks the two filters together to intensify the characteristics of th" +
                "e filters. The TVA for partial 1 controls the volume balance be" +
                "tween the two partials.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 2, "Determines how partial 1 and 2 are connected.\r\n", "_PMT_PMT_03.png",
                "This type mixes the sound of partial 1 and partial 2, applies a filter, " +
                "and then applies a booster to distort the waveform.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 3, "Determines how partial 1 and 2 are connected.\r\n", "_PMT_PMT_04.png",
                "This type applies a booster to distort the waveform, and then combines the two f" +
                "ilters. The TVA for partial 1 controls the volume balance betwe" +
                "en the two partials and adjusts booster level.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 4, "Determines how partial 1 and 2 are connected.\r\n", "_PMT_PMT_05.png",
                "This type uses a ring modulator to create new overtones, and combines the two fi" +
                "lters. The partial 1 TVA will control the volume balance of the tw" +
                "o partials, adjusting the depth of ring modulator.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 5, "Determines how partial 1 and 2 are connected.\r\n", "_PMT_PMT_06.png",
                "This type uses a ring modulator to create new overtones, and in addition mixes i" +
                "n the sound of partial 2 and stacks the two filters. Since the rin" +
                "g-modulated sound can be mixed with partial 2, partial 1 TVA c" +
                "an adjust the amount of the ring-modulated sound.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 6, "Determines how partial 1 and 2 are connected.\r\n", "_PMT_PMT_07.png",
                "This type applies a filter to partial 1 and ring-modulates it with partial 2" +
                " to create new overtones.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 7, "Determines how partial 1 and 2 are connected.\r\n", "_PMT_PMT_08.png",
                "This type sends the filtered partial 1 and partial 2 through a ring modu" +
                "lator, and then mixes in the sound of partial 2 and applies a filt" +
                "er to the result.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 8, "Determines how partial 1 and 2 are connected.\r\n", "_PMT_PMT_09.png",
                "This type passes the filtered sound of each partial through a ring modulator to " +
                "create new overtones. The partial 1 TVA will control the volume ba" +
                "lance of the two partials, adjusting the depth of ring modulator.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex++, 9, "Determines how partial 1 and 2 are connected.\r\n", "_PMT_PMT_10.png",
                "This type passes the filtered sound of each partial through a ring modulator to " +
                "create new overtones, and also mixes in the sound of partial 2. Si" +
                "nce the ring-modulated sound can be mixed with partial 2, partial " +
                "1 TVA can adjust the amount of the ring-modulated sound.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex++, 0, "When a Structure Type of TYPE 3 or TYPE 4 is selected, you can adjust the depth " +
                "of the booster. The booster increases the input signal in order to dis" +
                "tort the sound. This creates the distortion effect frequently used wit" +
                "h electric guitars. Higher settings will produce more distortion.\r\n", "_PMT_PMT_11.png",
                "Booster\r\n" +
                "The Booster is used to distort the incoming signal. In addition to using this to" +
                " create distortion, you can use the waveform (WG1) of one of the parti" +
                "als as an LFO which shifts the other waveform (WG2) upward or downward" +
                " to create modulation similar to PWM (pulse width modulation). This pa" +
                "rameter works best when you use it in conjunction with the Wave Gain p" +
                "arameter (p. 50).\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 0, "Determines how partial partial 3 and 4 are connected.\r\n", "_PMT_PMT_01.png",
                "With this type, partial 3 and 4 are independent. Use this type when" +
                " you want to preserve PCM sounds or create and combine sounds for each" +
                " partial.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 1, "Determines how partial partial 3 and 4 are connected.\r\n", "_PMT_PMT_02.png",
                "This type stacks the two filters together to intensify the characteristics of th" +
                "e filters. The TVA for partial 3 controls the volume balance be" +
                "tween the two partials.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 2, "Determines how partial partial 3 and 4 are connected.\r\n", "_PMT_PMT_03.png",
                "This type mixes the sound of partial 3 and partial 4, applies a filter, " +
                "and then applies a booster to distort the waveform.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 3, "Determines how partial partial 3 and 4 are connected.\r\n", "_PMT_PMT_04.png",
                "This type applies a booster to distort the waveform, and then combines the two f" +
                "ilters. The TVA for partial 3 controls the volume balance betwe" +
                "en the two partials and adjusts booster level.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 4, "Determines how partial partial 3 and 4 are connected.\r\n", "_PMT_PMT_05.png",
                "This type uses a ring modulator to create new overtones, and combines the two fi" +
                "lters. The partial 3 TVA will control the volume balance of the tw" +
                "o partials, adjusting the depth of ring modulator.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 5, "Determines how partial partial 3 and 4 are connected.\r\n", "_PMT_PMT_06.png",
                "This type uses a ring modulator to create new overtones, and in addition mixes i" +
                "n the sound of partial 4 and stacks the two filters. Since the rin" +
                "g-modulated sound can be mixed with partial 4, partial 3 TVA c" +
                "an adjust the amount of the ring-modulated sound.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 6, "Determines how partial partial 3 and 4 are connected.\r\n", "_PMT_PMT_07.png",
                "This type applies a filter to partial 3 and ring-modulates it with partial 2" +
                " (4) to create new overtones.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 7, "Determines how partial partial 3 and 4 are connected.\r\n", "_PMT_PMT_08.png",
                "This type sends the filtered partial 3 and partial 4 through a ring modu" +
                "lator, and then mixes in the sound of partial 4 and applies a filt" +
                "er to the result.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex, 8, "Determines how partial partial 3 and 4 are connected.\r\n", "_PMT_PMT_09.png",
                "This type passes the filtered sound of each partial through a ring modulator to " +
                "create new overtones. The partial 3 TVA will control the volume ba" +
                "lance of the two partials, adjusting the depth of ring modulator.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex++, 9, "Determines how partial partial 3 and 4 are connected.\r\n", "_PMT_PMT_10.png",
                "This type passes the filtered sound of each partial through a ring modulator to " +
                "create new overtones, and also mixes in the sound of partial 4. Si" +
                "nce the ring-modulated sound can be mixed with partial 4, partial " +
                "3 TVA can adjust the amount of the ring-modulated sound.\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex++, 0, "When a Structure Type of TYPE 3 or TYPE 4 is selected, you can adjust the depth " +
                "of the booster. The booster increases the input signal in order to dis" +
                "tort the sound. This creates the distortion effect frequently used wit" +
                "h electric guitars. Higher settings will produce more distortion.\r\n", "_PMT_PMT_11.png",
                "Booster\r\n" +
                "The Booster is used to distort the incoming signal. In addition to using this to" +
                " create distortion, you can use the waveform (WG1) of one of the parti" +
                "als as an LFO which shifts the other waveform (WG2) upward or downward" +
                " to create modulation similar to PWM (pulse width modulation). This pa" +
                "rameter works best when you use it in conjunction with the Wave Gain p" +
                "arameter (p. 50).\r\n", "", 0x2840);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "This determines what will happen to the tone’s level when a note that’s higher t" +
                "han the partial’s specified keyboard range is played. Higher settings " +
                "produce a more gradual change in volume. If you don’t want the tone to" +
                " sound at all when a note above the keyboard range is played, set this" +
                " parameter to “0.”\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "Specifies the highest note that the tone will sound for each partial.\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "Specifies the lowest note that the tone will sound for each partial.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "If you attempt to raise the lower key higher than the upper key, or to lower the" +
                " upper key\r\n" +
                "below the lower key, the other value will be automatically modified to the same " +
                "setting.\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "This determines what will happen to the tone’s level when a note that’s lower th" +
                "an the partial’s specified keyboard range is played. Higher settings p" +
                "roduce a more gradual change in volume.\r\n" +
                "\r\n" +
                "If you don’t want the tone to sound at all when a note below the keyboard range " +
                "is played, set this parameter to “0.”\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "This determines what will happen to the tone’s level when the tone is played at " +
                "a velocity greater than its specified velocity range.\r\n" +
                "\r\n" +
                "Higher settings produce a more gradual change in volume.\r\n" +
                "\r\n" +
                "If you want notes played outside the specified key velocity range to not be soun" +
                "ded at all, set this to “0.”\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "This sets the highest velocity at which the partial will sound. Make these setti" +
                "ngs when you want different partials to sound in response to notes pla" +
                "yed at different strengths.\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "This sets the lowest velocity at which the partial will sound. Make these settin" +
                "gs when you want different partial to sound in response to notes playe" +
                "d at different strengths.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "If you attempt to set the Lower velocity limit above the Upper, or the Upper bel" +
                "ow the Lower, the other value will automatically be adjusted to the sa" +
                "me setting.\r\n" +
                "\r\n" +
                "When using the Matrix Control (p. 63) to have different partials played, set the" +
                " lowest value (Lower) and highest value (Upper) of the value of the MI" +
                "DI message used.\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "This determines what will happen to the tone’s level when the tone is played at " +
                "a velocity lower than its specified velocity range.\r\n" +
                "\r\n" +
                "Higher settings produce a more gradual change in volume.\r\n" +
                "\r\n" +
                "If you want notes played outside the specified key velocity range to not be soun" +
                "ded at all, set this to “0.”\r\n", "", 0x00e0);
            Add(0, 2, (byte)ItemIndex++, 0, "", "",
                "Use the Matrix Control (p. 63) to enable (ON), or disable (OFF) sounding of diff" +
                "erent partials.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "You can also cause different partials to sound in response to notes played at di" +
                "fferent strengths (velocity) on the keyboard (p. 53). However, the Mat" +
                "rix Control and the keyboard velocity cannot be used simultaneously to" +
                " make different partials to sound.\r\n" +
                "\r\n" +
                "When using the Matrix Control to have different partials to sound, set the Veloc" +
                "ity Control parameter (p. 53) to “OFF.”\r\n", "", 0x00e0);
            // PCM Synth Tone Pitch tab
            ItemIndex = Skip;
            Add(0, 3, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 3, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 3, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 3, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 3, (byte)ItemIndex++, 0, "", "",
                "Adjusts the pitch of the partial’s sound up or down in semitone steps (+/-4 octa" +
                "ves).\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "", 0x00e0);
            Add(0, 3, (byte)ItemIndex++, 0, "", "",
                "Adjusts the pitch of the partial’s sound up or down in 1-cent steps (+/-50 cents" +
                ").\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n" +
                "\r\n" +
                "* One cent is 1/100th of a semitone.\r\n", "", 0x00e0);
            Add(0, 3, (byte)ItemIndex++, 0, "", "",
                "This specifies the width of random pitch deviation that will occur each time a k" +
                "ey is pressed. If you do not want the pitch to change randomly, set th" +
                "is to “0.” These values are in units of cents (1/100th of a semitone).\r\n", "", 0x00e0);
            Add(0, 3, (byte)ItemIndex++, 0, "", "",
                "This specifies the amount of pitch change that will occur when you play a key on" +
                "e octave higher (i.e., 12 keys upward on the keyboard).\r\n" +
                "\r\n" +
                "If you want the pitch to rise one octave as on a conventional keyboard, set this" +
                " to “+100.” If you want the pitch to rise two octaves, set this to “+2" +
                "00.” Conversely, set this to a negative value if you want the pitch to" +
                " fall. With a setting of “0,” all keys will produce the same pitch.\r\n", "_PCM_Pitch_01.png", 0x0068);
            Add(0, 3, (byte)ItemIndex++, 0, "", "",
                "Specifies the degree of pitch change in semitones when the Pitch Bend lever is a" +
                "ll the way right.\r\n" +
                "\r\n" +
                "For example, if this parameter is set to “12,” the pitch will rise one octave wh" +
                "en the pitch bend lever is moved to the right-most position.\r\n", "", 0x00e0);
            Add(0, 3, (byte)ItemIndex++, 0, "", "",
                "Specifies the degree of pitch change in semitones when the Pitch Bend lever is a" +
                "ll the way left.\r\n" +
                "For example if this is set to “-48” and you move the pitch bend lever all the wa" +
                "y to the left, the pitch will fall 4 octaves.\r\n", "", 0x00e0);
            // PCM Synth Tone Pitch Env tab
            ItemIndex = Skip;
            Add(0, 4, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 4, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 4, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 4, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 4, (byte)ItemIndex++, 0, "", "",
                "Adjusts the effect of the Pitch Envelope. Higher settings will cause the pitch e" +
                "nvelope to produce greater change. Negative “-” settings will invert t" +
                "he shape of the envelope.\r\n", "", 0x00e0);
            Add(0, 4, (byte)ItemIndex++, 0, "", "",
                "Keyboard playing dynamics can be used to control the depth of the pitch envelope" +
                ".\r\n" +
                "\r\n" +
                "If you want the pitch envelope to have more effect for strongly played notes, se" +
                "t this parameter to a positive “+” value.\r\n" +
                "\r\n" +
                "If you want the pitch envelope to have less effect for strongly played notes, se" +
                "t this to a negative “-” value.\r\n", "", 0x00e0);
            Add(0, 4, (byte)ItemIndex++, 0, "", "",
                "This allows keyboard dynamics to affect the Time 1 of the Pitch envelope.\r\n" +
                "\r\n" +
                "If you want Time 1 to be speeded up for strongly played notes, set this paramete" +
                "r to a positive “+” value.\r\n" +
                "\r\n" +
                "If you want it to be slowed down, set this to a negative “-” value.\r\n", "", 0x00e0);
            Add(0, 4, (byte)ItemIndex++, 0, "", "",
                "Use this parameter when you want key release speed to affect the Time 4 value of" +
                " the pitch envelope.\r\n" +
                "\r\n" +
                "If you want Time 4 to be speeded up for quickly released notes, set this paramet" +
                "er to a positive “+” value.\r\n" +
                "\r\n" +
                "If you want it to be slowed down, set this to a negative “-” value.\r\n", "", 0x00e0);
            Add(0, 4, (byte)ItemIndex++, 0, "", "",
                "Use this setting if you want the pitch envelope times (Time 2–Time 4) to be affe" +
                "cted by the keyboard location. \r\n" +
                "Based on the pitch envelope times for the C4 key, positive “+” settings will cau" +
                "se notes higher than C4 to have increasingly shorter times, and negati" +
                "ve “-” settings will cause them to have increasingly longer times. \r\n" +
                "Larger settings will produce greater change.\r\n", "_PCM_PitchEnv_01.png", 0x0068);
            Add(0, 4, (byte)ItemIndex++, 0, "", "",
                "Specify the pitch envelope times (Time 1–Time 4).\r\n" +
                "\r\n" +
                "Higher settings will result in a longer time until the next pitch is reached. (F" +
                "or example,\r\n" +
                "\r\n" +
                "Time 2 is the time over which the pitch changes from Level 1 to Level 2.)\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "_PCM_PitchEnv_02.png", 0x0068);
            Add(0, 4, (byte)ItemIndex++, 0, "", "",
                "Specify the pitch envelope levels (Level 0–Level 4).\r\n" +
                "\r\n" +
                "It determines how much the pitch changes from the reference pitch (the value set" +
                " with Coarse Tune or Fine Tune on the Pitch screen) at each point.\r\n" +
                "\r\n" +
                "Positive “+” settings will cause the pitch to be higher than the standard pitch," +
                " and negative “-” settings will cause it to be lower.\r\n", "", 0x00e0);
            // PCM Synth Tone TVF tab
            ItemIndex = Skip;
            Add(0, 5, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 5, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 5, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 5, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 5, (byte)ItemIndex, 0, "Selects the type of filter.\r\n" +
                "A filter cuts or boosts a specific frequency region to change a sound’s brightne" +
                "ss, thickness, or other qualities.\r\n", "",
                "No filter is used.\r\n", "", 0x30b0);
            Add(0, 5, (byte)ItemIndex, 1, "Selects the type of filter.\r\n" +
                "A filter cuts or boosts a specific frequency region to change a sound’s brightne" +
                "ss, thickness, or other qualities.\r\n", "_PCM_TVF_01.png",
                "Low Pass Filter. This reduces the volume of all frequencies above the cutoff fre" +
                "quency (Cutoff Freq) in order to round off, or un-brighten the sound.\r\n" +
                "\r\n" +
                "This is the most common filter used in synthesizers.\r\n", "", 0x2750);
            Add(0, 5, (byte)ItemIndex, 2, "Selects the type of filter.\r\n" +
                "A filter cuts or boosts a specific frequency region to change a sound’s brightne" +
                "ss, thickness, or other qualities.\r\n", "_PCM_TVF_02.png",
                "Band Pass Filter. This leaves only the frequencies in the region of the cutoff f" +
                "requency (Cutoff Freq), and cuts the rest.\r\n" +
                "\r\n" +
                "This can be useful when creating distinctive sounds.\r\n", "", 0x2750);
            Add(0, 5, (byte)ItemIndex, 3, "Selects the type of filter.\r\n" +
                "A filter cuts or boosts a specific frequency region to change a sound’s brightne" +
                "ss, thickness, or other qualities.\r\n", "_PCM_TVF_03.png",
                "High Pass Filter. This cuts the frequencies in the region below the cutoff frequ" +
                "ency (Cutoff Freq).\r\n" +
                "\r\n" +
                "This is suitable for creating percussive sounds emphasizing their higher tones.\r\n", "", 0x2750);
            Add(0, 5, (byte)ItemIndex, 4, "Selects the type of filter.\r\n" +
                "A filter cuts or boosts a specific frequency region to change a sound’s brightne" +
                "ss, thickness, or other qualities.\r\n", "_PCM_TVF_04.png",
                "Peaking Filter. This emphasizes the frequencies in the region of the cutoff freq" +
                "uency (Cutoff Freq).\r\n" +
                "\r\n" +
                "You can use this to create wah-wah effects by employing an LFO to change the cut" +
                "off frequency cyclically.\r\n", "", 0x2750);
            Add(0, 5, (byte)ItemIndex, 5, "Selects the type of filter.\r\n" +
                "A filter cuts or boosts a specific frequency region to change a sound’s brightne" +
                "ss, thickness, or other qualities.\r\n", "_PCM_TVF_01.png",
                "Low Pass Filter 2. Although frequency components above the Cutoff frequency (Cut" +
                "off Freq) are cut, the sensitivity of this filter is half that of the " +
                "LPF.\r\n" +
                "\r\n" +
                "This makes it a comparatively warmer low pass filter.\r\n" +
                "\r\n" +
                "This filter is good for use with simulated instrument sounds such as the acousti" +
                "c piano.\r\n" +
                "\r\n" +
                "* If you set “LPF2,” the setting for the Resonance parameter will be ignored (p." +
                " 56).\r\n", "", 0x2750);
            Add(0, 5, (byte)ItemIndex++, 6, "Selects the type of filter.\r\n" +
                "A filter cuts or boosts a specific frequency region to change a sound’s brightne" +
                "ss, thickness, or other qualities.\r\n", "_PCM_TVF_01.png",
                "Low Pass Filter 3. Although frequency components above the Cutoff frequency (Cut" +
                "off Freq) are cut, the sensitivity of this filter changes according to" +
                " the Cutoff frequency.\r\n" +
                "\r\n" +
                "While this filter is also good for use with simulated acoustic instrument sounds" +
                ", the nuance it exhibits differs from that of the LPF2, even with the " +
                "same TVF Envelope settings.\r\n" +
                "\r\n" +
                "* If you set “LPF3,” the setting for the Resonance parameter will be ignored (p." +
                " 56).\r\n", "", 0x2750);
            Add(0, 5, (byte)ItemIndex++, 0, "", "",
                "Selects the frequency at which the filter begins to have an effect on the wavefo" +
                "rm’s frequency components.\r\n" +
                "\r\n" +
                "With “LPF/LPF2/LPF3” selected for the Filter Type parameter, lower cutoff freque" +
                "ncy settings reduce a tone’s upper harmonics for a more rounded, warme" +
                "r sound. Higher settings make it sound brighter.\r\n" +
                "\r\n" +
                "If “BPF” is selected, harmonic components will change depending on the TVF Cutof" +
                "f Frequency setting. This can be useful when creating distinctive soun" +
                "ds.\r\n" +
                "\r\n" +
                "With “HPF” selected, higher Cutoff Frequency settings will reduce lower harmonic" +
                "s to emphasize just the brighter components of the sound.\r\n" +
                "\r\n" +
                "With “PKG” selected, the harmonics to be emphasized will vary depending on Cutof" +
                "f Frequency setting.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "To edit the overall tone while preserving the relative differences in the Cutoff" +
                " Frequency values set for each partial, set the Cutoff Offset paramete" +
                "r (p. 48).\r\n", "", 0x00e0);
            Add(0, 5, (byte)ItemIndex++, 0, "", "",
                "Emphasizes the portion of the sound in the region of the cutoff frequency, addin" +
                "g character to the sound. \r\n" +
                "\r\n" +
                "Excessively high settings can produce oscillation, causing the sound to distort." +
                "\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "To edit the overall tone while preserving the relative differences in the Resona" +
                "nce values set for each partial, set the Resonance Offset parameter (p" +
                ". 48).\r\n", "_PCM_TVF_05.png", 0x0059);
            Add(0, 5, (byte)ItemIndex++, 0, "", "",
                "Use this parameter if you want the cutoff frequency to change according to the k" +
                "ey that is pressed.\r\n" +
                "\r\n" +
                "Relative to the cutoff frequency at the C4 key (center C), positive “+” settings" +
                " will cause the cutoff frequency to rise for notes higher than C4, and" +
                " negative “-” settings will cause the cutoff frequency to fall for not" +
                "es higher than C4.\r\n" +
                "\r\n" +
                "Larger settings will produce greater change.\r\n" +
                "\r\n", "_PCM_TVF_06.png", 0x0059);
            Add(0, 5, (byte)ItemIndex, 0, "", "",
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the Cutoff frequency to be affected by the" +
                " keyboard velocity.\r\n" +
                "\r\n" +
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_10.png", 0x0068);
            Add(0, 5, (byte)ItemIndex, 1, "", "",
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the Cutoff frequency to be affected by the" +
                " keyboard velocity.\r\n" +
                "\r\n" +
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_11.png", 0x0068);
            Add(0, 5, (byte)ItemIndex, 2, "", "",
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the Cutoff frequency to be affected by the" +
                " keyboard velocity.\r\n" +
                "\r\n" +
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_12.png", 0x0068);
            Add(0, 5, (byte)ItemIndex, 3, "", "",
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the Cutoff frequency to be affected by the" +
                " keyboard velocity.\r\n" +
                "\r\n" +
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_13.png", 0x0068);
            Add(0, 5, (byte)ItemIndex, 4, "", "",
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the Cutoff frequency to be affected by the" +
                " keyboard velocity.\r\n" +
                "\r\n" +
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_14.png", 0x0068);
            Add(0, 5, (byte)ItemIndex, 5, "", "",
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the Cutoff frequency to be affected by the" +
                " keyboard velocity.\r\n" +
                "\r\n" +
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_15.png", 0x0068);
            Add(0, 5, (byte)ItemIndex, 6, "", "",
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the Cutoff frequency to be affected by the" +
                " keyboard velocity.\r\n" +
                "\r\n" +
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_16.png", 0x0068);
            Add(0, 5, (byte)ItemIndex++, 7, "", "",
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the Cutoff frequency to be affected by the" +
                " keyboard velocity.\r\n" +
                "\r\n" +
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_17.png", 0x0068);
            Add(0, 5, (byte)ItemIndex++, 0, "", "",
                "Use this parameter when changing the cutoff frequency to be applied as a result " +
                "of changes in playing velocity.\r\n" +
                "\r\n" +
                "If you want strongly played notes to raise the cutoff frequency, set this parame" +
                "ter to positive “+” settings.\r\n" +
                "\r\n" +
                "If you want strongly played notes to lower the cutoff frequency, use negative “-" +
                "” settings.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "To edit the overall tone while preserving the relative differences in the Cutoff" +
                " V-Sens values set for each partial, set the Velocity Sens Offset para" +
                "meter (p. 48). However, this setting is shared by the Level V-Sens par" +
                "ameter (p. 58).\r\n", "", 0x00e0);
            Add(0, 5, (byte)ItemIndex++, 0, "", "",
                "This allows keyboard velocity to modify the amount of Resonance.\r\n" +
                "\r\n" +
                "If you want strongly played notes to have a greater Resonance effect, set this p" +
                "arameter to positive “+” settings.\r\n" +
                "\r\n" +
                "If you want strongly played notes to have less Resonance, use negative “-” setti" +
                "ngs.\r\n", "", 0x00e0);
            // PCM Synth Tone TVF Env tab
            ItemIndex = Skip;
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Specifies the depth of the TVF envelope.\r\n" +
                "\r\n" +
                "Higher settings will cause the TVF envelope to produce greater change. Negative " +
                "“-” settings will invert the shape of the envelope.\r\n", "", 0x00e0);
            Add(0, 6, (byte)ItemIndex, 0, "", "",
                "Selects one of the following 7 curves that will determine how keyboard playing d" +
                "ynamics will affect the TVF envelope.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the TVF Envelope to be affected by the key" +
                "board velocity.\r\n", "_PCM_TVF_10.png", 0x0068);
            Add(0, 6, (byte)ItemIndex, 1, "", "",
                "Selects one of the following 7 curves that will determine how keyboard playing d" +
                "ynamics will affect the TVF envelope.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the TVF Envelope to be affected by the key" +
                "board velocity.\r\n", "_PCM_TVF_11.png", 0x0068);
            Add(0, 6, (byte)ItemIndex, 2, "", "",
                "Selects one of the following 7 curves that will determine how keyboard playing d" +
                "ynamics will affect the TVF envelope.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the TVF Envelope to be affected by the key" +
                "board velocity.\r\n", "_PCM_TVF_12.png", 0x0068);
            Add(0, 6, (byte)ItemIndex, 3, "", "",
                "Selects one of the following 7 curves that will determine how keyboard playing d" +
                "ynamics will affect the TVF envelope.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the TVF Envelope to be affected by the key" +
                "board velocity.\r\n", "_PCM_TVF_13.png", 0x0068);
            Add(0, 6, (byte)ItemIndex, 4, "", "",
                "Selects one of the following 7 curves that will determine how keyboard playing d" +
                "ynamics will affect the TVF envelope.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the TVF Envelope to be affected by the key" +
                "board velocity.\r\n", "_PCM_TVF_14.png", 0x0068);
            Add(0, 6, (byte)ItemIndex, 5, "", "",
                "Selects one of the following 7 curves that will determine how keyboard playing d" +
                "ynamics will affect the TVF envelope.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the TVF Envelope to be affected by the key" +
                "board velocity.\r\n", "_PCM_TVF_15.png", 0x0068);
            Add(0, 6, (byte)ItemIndex, 6, "", "",
                "Selects one of the following 7 curves that will determine how keyboard playing d" +
                "ynamics will affect the TVF envelope.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the TVF Envelope to be affected by the key" +
                "board velocity.\r\n", "_PCM_TVF_16.png", 0x0068);
            Add(0, 6, (byte)ItemIndex++, 7, "", "",
                "Selects one of the following 7 curves that will determine how keyboard playing d" +
                "ynamics will affect the TVF envelope.\r\n" +
                "\r\n" +
                "Set this to “FIXED” if you don’t want the TVF Envelope to be affected by the key" +
                "board velocity.\r\n", "_PCM_TVF_17.png", 0x0068);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Specifies how keyboard playing dynamics will affect the depth of the TVF envelop" +
                "e.\r\n" +
                "\r\n" +
                "Positive “+” settings will cause the TVF envelope to have a greater effect for s" +
                "trongly played notes, and negative “-” settings will cause the effect " +
                "to be less.\r\n", "", 0x00e0);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "This allows keyboard dynamics to affect the Time 1 of the TVF envelope.\r\n" +
                "\r\n" +
                "If you want Time 1 to be speeded up for strongly played notes, set this paramete" +
                "r to a positive “+” value.\r\n" +
                "If you want it to be slowed down, set this to a negative “-” value.\r\n", "", 0x00e0);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "The parameter to use when you want key release speed to control the Time 4 value" +
                " of the TVF envelope.\r\n" +
                "\r\n" +
                "If you want Time 4 to be speeded up for quickly released notes, set this paramet" +
                "er to a positive “+” value.\r\n" +
                "If you want it to be slowed down, set this to a negative “-” value.\r\n", "", 0x00e0);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Use this setting if you want the TVF envelope times (Time 2–Time 4) to be affect" +
                "ed by the keyboard location. \r\n" +
                "\r\n" +
                "Based on the TVF envelope times for the C4 key (center C), positive “+” settings" +
                " will cause notes higher than C4 to have increasingly shorter times, a" +
                "nd negative “-” settings will cause them to have increasingly longer t" +
                "imes. Larger settings will produce greater change.\r\n", "_PCM_PitchEnv_01.png", 0x0068);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope time 1.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until cutoff frequency L1 is " +
                "reached. \r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope time 2.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until cutoff frequency L2 is " +
                "reached. \r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope time 3.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until cutoff frequency L3 is " +
                "reached. \r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope time 4.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until cutoff frequency L4 is " +
                "reached. \r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope level 0.\r\n" +
                "\r\n" +
                "This settings specify how the cutoff frequency will change at point L0, relat" +
                "ive to the standard cutoff frequency (the cutoff frequency value speci" +
                "fied in the TVF screen).\r\n", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope level 1.\r\n" +
                "\r\n" +
                "This settings specify how the cutoff frequency will change at point L1, relat" +
                "ive to the standard cutoff frequency (the cutoff frequency value speci" +
                "fied in the TVF screen).\r\n", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope level 2.\r\n" +
                "\r\n" +
                "This settings specify how the cutoff frequency will change at point L2, relat" +
                "ive to the standard cutoff frequency (the cutoff frequency value speci" +
                "fied in the TVF screen).\r\n", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope level 3.\r\n" +
                "\r\n" +
                "This settings specify how the cutoff frequency will change at point L3, relat" +
                "ive to the standard cutoff frequency (the cutoff frequency value speci" +
                "fied in the TVF screen).\r\n", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(0, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope level 4.\r\n" +
                "\r\n" +
                "This settings specify how the cutoff frequency will change at point L4, relat" +
                "ive to the standard cutoff frequency (the cutoff frequency value speci" +
                "fied in the TVF screen).\r\n", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            // PCM Synth Tone TVA tab
            ItemIndex = Skip;
            Add(0, 7, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 7, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 7, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 7, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 7, (byte)ItemIndex++, 0, "", "",
                "Sets the volume of the partial. This setting is useful primarily for adjusting t" +
                "he volume balance between partials.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "", 0x00e0);
            Add(0, 7, (byte)ItemIndex, 0, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the partial to be affected by the force with wh" +
                "ich you play the key, set this to “FIXED.”\r\n", "_PCM_TVF_10.png", 0x0068);
            Add(0, 7, (byte)ItemIndex, 1, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the partial to be affected by the force with wh" +
                "ich you play the key, set this to “FIXED.”\r\n", "_PCM_TVF_11.png", 0x0068);
            Add(0, 7, (byte)ItemIndex, 2, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the partial to be affected by the force with wh" +
                "ich you play the key, set this to “FIXED.”\r\n", "_PCM_TVF_12.png", 0x0068);
            Add(0, 7, (byte)ItemIndex, 3, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the partial to be affected by the force with wh" +
                "ich you play the key, set this to “FIXED.”\r\n", "_PCM_TVF_13.png", 0x0068);
            Add(0, 7, (byte)ItemIndex, 4, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the partial to be affected by the force with wh" +
                "ich you play the key, set this to “FIXED.”\r\n", "_PCM_TVF_14.png", 0x0068);
            Add(0, 7, (byte)ItemIndex, 5, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the partial to be affected by the force with wh" +
                "ich you play the key, set this to “FIXED.”\r\n", "_PCM_TVF_15.png", 0x0068);
            Add(0, 7, (byte)ItemIndex, 6, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the partial to be affected by the force with wh" +
                "ich you play the key, set this to “FIXED.”\r\n", "_PCM_TVF_16.png", 0x0068);
            Add(0, 7, (byte)ItemIndex++, 7, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the partial to be affected by the force with wh" +
                "ich you play the key, set this to “FIXED.”\r\n", "_PCM_TVF_17.png", 0x0068);
            Add(0, 7, (byte)ItemIndex++, 0, "", "",
                "Set this when you want the volume of the partial to change depending on the forc" +
                "e with which you press the keys.\r\n" +
                "\r\n" +
                "Set this to a positive “+” value to have the changes in partial volume increase " +
                "the more forcefully the keys are played; to make the partial play more" +
                " softly as you play harder, set this to a negative “-” value.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "If you wish to make adjustments to the entire tone while maintaining the relativ" +
                "e values of TVA Level V-Sens among partials, adjust the Velocity Sens " +
                "Offset parameter (p. 48). However, this setting is shared by the Cutof" +
                "f V-Sens parameter (p. 57).\r\n", "", 0x00e0);
            Add(0, 7, (byte)ItemIndex++, 0, "", "",
                "Adjusts the angle of the volume change that will occur in the selected Bias Dire" +
                "ction.\r\n" +
                "\r\n" +
                "Larger settings will produce greater change.\r\n" +
                "Negative “-” values will invert the change direction.\r\n", "", 0x00e0);
            Add(0, 7, (byte)ItemIndex++, 0, "", "",
                "Specifies the key relative to which the volume will be modified.\r\n", "", 0x00e0);
            Add(0, 7, (byte)ItemIndex, 0, "Selects the direction in which change will occur starting from the Bias Position" +
                ".\r\n", "",
                "The volume will be modified for the keyboard area below the Bias Point.\r\n", "", 0x7070);
            Add(0, 7, (byte)ItemIndex, 1, "Selects the direction in which change will occur starting from the Bias Position" +
                ".\r\n", "",
                "The volume will be modified for the keyboard area above the Bias Point.\r\n", "", 0x7070);
            Add(0, 7, (byte)ItemIndex, 2, "Selects the direction in which change will occur starting from the Bias Position" +
                ".\r\n", "",
                "The volume will be modified symmetrically toward the left and right of the Bias " +
                "Point.\r\n", "", 0x7070);
            Add(0, 7, (byte)ItemIndex++, 3, "Selects the direction in which change will occur starting from the Bias Position" +
                ".\r\n", "",
                "The volume changes linearly with the bias point at the center.\r\n", "", 0x7070);
            Add(0, 7, (byte)ItemIndex++, 0, "", "",
                "Sets the pan of the partial. “L64” is far left, “0” is center, and “63R” is far " +
                "right.\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "", 0x00e0);
            Add(0, 7, (byte)ItemIndex++, 0, "", "",
                "Use this parameter if you want key position to affect panning. Positive “+” sett" +
                "ings will cause notes\r\n" +
                "higher than C4 key (center C) to be panned increasingly further toward the right" +
                ", and negative\r\n" +
                "“-” settings will cause notes higher than C4 key (center C) to be panned toward " +
                "the left. Larger\r\n" +
                "settings will produce greater change.\r\n", "_PCM_Pitch_Pan_01.png", 0x0068);
            Add(0, 7, (byte)ItemIndex++, 0, "", "",
                "Use this parameter when you want the stereo location to change randomly each tim" +
                "e you press a key.\r\n" +
                "\r\n" +
                "Higher settings will produce a greater amount of change.\r\n", "", 0x00e0);
            Add(0, 7, (byte)ItemIndex++, 0, "", "",
                "This setting causes panning to be alternated between left and right each time a " +
                "key is pressed.\r\n" +
                "\r\n" +
                "Higher settings will produce a greater amount of change.\r\n" +
                "\r\n" +
                "“L” or “R” settings will reverse the order in which the pan will alternate betwe" +
                "en left and right. For example if two tones are set to “L” and “R” res" +
                "pectively, the panning of the two tones will alternate each time they " +
                "are played.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "In the Pan Key Follow, Random Pan Depth, Alternate Pan Depth parameter settings," +
                " if the Structure Type set in the range of “2”–”10,” the output of par" +
                "tial 1 and 2 will be combined into partial 2, and the output of partia" +
                "l 3 and 4 will be combined into partial 4.\r\n" +
                "For this reason, partial 1 will follow the settings of partial 2, and partial 3 " +
                "will follow the settings" +
                "of partial 4 (p. 52).\r\n", "", 0x00e0);
            // PCM Synth Tone TVA Env tab
            ItemIndex = Skip;
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "This allows keyboard dynamics to affect the Time 1 of the TVA envelope.\r\n" +
                "\r\n" +
                "If you want Time 1 to be speeded up for strongly played notes, set this paramete" +
                "r to a positive “+” value.\r\n" +
                "\r\n" +
                "If you want it to be slowed down, set this to a negative “-” value.\r\n", "", 0x00e0);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "The parameter to use when you want key release speed to control the Time 4 value" +
                " of the TVA envelope.\r\n" +
                "\r\n" +
                "If you want Time 4 to be speeded up for quickly released notes, set this paramet" +
                "er to a positive “+” value.\r\n" +
                "\r\n" +
                "If you want it to be slowed down, set this to a negative “-” value.\r\n", "", 0x00e0);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "Use this setting if you want the TVA envelope times (Time 2–Time 4) to be affect" +
                "ed by the keyboard location. \r\n" +
                "Based on the TVA envelope times for the C4 key (center C), positive “+” settings" +
                " will cause notes higher than C4 to have increasingly shorter times, a" +
                "nd negative “-” settings will cause them to have increasingly longer t" +
                "imes.\r\n" +
                "\r\n" +
                "Larger settings will produce greater change.\r\n", "_PCM_Pitch_Time_01.png", 0x0068);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope time 1.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until volume level 1 is reached." +
                "\r\n" +
                "\r\n" +
                "This is the time over which level raises to Level 1 after note on.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "_PCM_TVALevelEnv_02.png", 0x0068);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope time 2.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until the volume level 2 is reached." +
                "\r\n" +
                "\r\n" +
                "This is the time over which level changes from Level 1 to Level 2.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "_PCM_TVALevelEnv_02.png", 0x0068);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope time 3.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until the volume level 3 is reached." +
                "\r\n" +
                "\r\n" +
                "This is the time over which level changes from Level 2 to Level 3.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "_PCM_TVALevelEnv_02.png", 0x0068);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope time 4.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until volume level 4 is reached." +
                "\r\n" +
                "\r\n" +
                "This is the time over which level changes to 0 after note off.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "_PCM_TVALevelEnv_02.png", 0x0068);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope level 1.\r\n" +
                "\r\n" +
                "These settings specify how the volume will change at each point, relative to the" +
                " standard volume (the Partial Level value specified in the TVA screen)" +
                ".\r\n", "_PCM_TVALevelEnv_02.png", 0x0068);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope level 2.\r\n" +
                "\r\n" +
                "These settings specify how the volume will change at each point, relative to the" +
                " standard volume (the Partial Level value specified in the TVA screen)" +
                ".\r\n", "_PCM_TVALevelEnv_02.png", 0x0068);
            Add(0, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope level 3.\r\n" +
                "\r\n" +
                "These settings specify how the volume will change at each point, relative to the" +
                " standard volume (the Partial Level value specified in the TVA screen)" +
                ".\r\n", "_PCM_TVALevelEnv_02.png", 0x0068);
            // PCM Synth Tone Output tab
            ItemIndex = Skip;
            Add(0, 9, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 9, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 9, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 9, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 9, (byte)ItemIndex++, 0, "", "",
                "Specifies the signal level of currently selected partial.\r\n", "", 0x0068);
            Add(0, 9, (byte)ItemIndex++, 0, "", "",
                "Specifies the level of the signal sent to the chorus for currently selected partial.\r\n", "", 0x0068);
            Add(0, 9, (byte)ItemIndex++, 0, "", "",
                "Specifies the level of the signal sent to the reverb for currently selected partial.\r\n", "", 0x0068);
            // PCM Synth Tone LFO1 tab
            ItemIndex = Skip;
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex, 0, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_05.png",
                "Sine wave.\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex, 1, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_04.png",
                "Triangle wave.\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex, 2, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_01.png",
                "Sawtooth wave.\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex, 3, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_09.png",
                "Sawtooth wave (negative polarity).\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex, 4, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_02.png",
                "Square wave.\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex, 5, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_12.png",
                "Random wave.\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex, 6, "Selects the waveform of the LFO.\r\n", "",
                "Once the attack of the waveform output by the LFO is allowed to develop in stand" +
                "ard fashion, the waveform then continues without further change.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "You must turn the Key Trigger parameter to “ON.” If this is “OFF,” it will have " +
                "no effect.\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex, 7, "Selects the waveform of the LFO.\r\n", "",
                "Once the decay of the waveform output by the LFO is allowed to develop in standa" +
                "rd fashion, the waveform then continues without further change.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "You must turn the Key Trigger parameter to “ON.” If this is “OFF,” it will have " +
                "no effect.\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex, 8, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_10.png",
                "Trapezoidal wave.\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex, 9, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_07.png",
                "Sample & Hold wave (one time per cycle, LFO value is changed).\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex, 10, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_06.png",
                "Chaos wave.\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex, 11, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_11.png",
                "Modified sine wave. The amplitude of a sine wave is randomly varied once each cy" +
                "cle.\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex++, 12, "Selects the waveform of the LFO.\r\n", "",
                "A waveform generated by the data specified by LFO Step 1–64.\r\n" +
                "\r\n" +
                "This produces stepped change with a fixed pattern similar to a step modulator.\r\n", "", 0x2840);
            Add(0, 10, (byte)ItemIndex++, 0, "Adjusts the modulation rate, or speed, of the LFO.\r\n" +
                "\r\n" +
                "If you want the LFO rate to be synchronized with the tempo, specify the setting " +
                "as a note value relative to the synchronization tempo.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n" +
                "\r\n" +
                "(Example) For a tempo of 120 (120 quarter notes occur in 1 minute (60 seconds))\r\n", "_PCM_LFO_01.png",
                "NOTE\r\n" +
                "This setting will be ignored if the Waveform parameter is set to “CHAOS.”\r\n", "", 0x5540);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "LFO Rate Detune makes subtle changes in the LFO cycle rate (Rate parameter) each" +
                " time a key is pressed.\r\n" +
                "\r\n" +
                "Higher settings will cause greater change. This parameter is invalid when Rate i" +
                "s set to “note.”\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Raises or lowers the LFO waveform relative to the central value (pitch or cutoff" +
                " frequency).\r\n" +
                "\r\n" +
                "Positive “+” settings will move the waveform so that modulation will occur from " +
                "the central value upward. \r\n" +
                "\r\n" +
                "Negative “-” settings will move the waveform so that modulation will occur from " +
                "the central value downward.\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Delay Time (LFO Delay Time) specifies the time elapsed before the LFO effect is " +
                "applied (the effect continues) after the key is pressed (or released)." +
                "\r\n" +
                "\r\n" +
                "* After referring to “How to Apply the LFO” (p. 62), change the setting until th" +
                "e desired effect is achieved.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "When using violin, wind, or certain other instrument sounds in a performance, ra" +
                "ther than having vibrato added immediately after the sounds are played" +
                ", it can be effective to add the vibrato after the note is drawn out s" +
                "omewhat. If you set the Delay Time in conjunction with the Pitch Depth" +
                " parameter and Rate parameter, the vibrato will be applied automatical" +
                "ly following a certain interval after the key is pressed.\r\n" +
                "\r\n" +
                "This effect is called “Delay Vibrato.”\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Adjusts the value for the Delay Time parameter depending on the key position, re" +
                "lative to the C4 key (center C).\r\n" +
                "\r\n" +
                "To decrease the time that elapses before the LFO effect is applied (the effect i" +
                "s continuous) with each higher key that is pressed in the upper regist" +
                "ers, select a positive value. \r\n" +
                "\r\n" +
                "To increase the elapsed time, select a negative value. Larger settings will prod" +
                "uce greater change.\r\n" +
                "\r\n" +
                "If you do not want the elapsed time before the LFO effect is applied (the effect" +
                " is continuous) to change according to the key pressed, set this to “0" +
                ".”\r\n", "_PCM_Pitch_Time_01.png", 0x0068);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Specifies how the LFO will be applied.\r\n" +
                "\r\n" +
                "* After referring to “How to Apply the LFO” (p. 62), change the setting until th" +
                "e desired effect is achieved.\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Specifies the time over which the LFO amplitude will reach the maximum (minimum)" +
                ".\r\n" +
                "\r\n" +
                "* After referring to “How to Apply the LFO” (p. 62), change the setting until th" +
                "e desired effect is achieved.\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Specifies whether the LFO cycle will be synchronized to begin when the key is pr" +
                "essed (ON) or not (OFF).\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Specifies how deeply the LFO will affect pitch.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Specifies how deeply the LFO will affect the cutoff frequency.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Specifies how deeply the LFO will affect the volume.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "", 0x00e0);
            Add(0, 10, (byte)ItemIndex++, 0, "", "",
                "Specifies how deeply the LFO will affect the pan.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "Positive “+” and negative “-” settings for the Depth parameter result in differi" +
                "ng kinds of change in pitch and volume. For example, if you set the De" +
                "pth parameter to a positive “+” value for one partial, and set another" +
                " partial to the same numerical value, but make it negative “-”, the mo" +
                "dulation phase for the two partials will be the reverse of each other." +
                " This allows you to shift back and forth between two different partial" +
                "s, or combine it with the Pan setting to cyclically change the locatio" +
                "n of the sound image.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "In the Pan Depth parameter settings, if the Structure Type parameter is set to a" +
                "ny value from “2” through “10,” the output of partial 1 and 2 will be " +
                "combined into partial 2, and the output of partial 3 and 4 will be com" +
                "bined into partial 4. For this reason, partial 1 will follow the setti" +
                "ngs of partial 2, and partial 3 will follow the settings of partial 4 " +
                "(p. 52).\r\n", "", 0x00e0);
            // PCM Synth Tone LFO2 tab
            ItemIndex = Skip;
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex, 0, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_05.png",
                "Sine wave.\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex, 1, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_04.png",
                "Triangle wave.\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex, 2, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_01.png",
                "Sawtooth wave.\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex, 3, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_09.png",
                "Sawtooth wave (negative polarity).\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex, 4, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_02.png",
                "Square wave.\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex, 5, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_12.png",
                "Random wave.\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex, 6, "Selects the waveform of the LFO.\r\n", "",
                "Once the attack of the waveform output by the LFO is allowed to develop in stand" +
                "ard fashion, the waveform then continues without further change.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "You must turn the Key Trigger parameter to “ON.” If this is “OFF,” it will have " +
                "no effect.\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex, 7, "Selects the waveform of the LFO.\r\n", "",
                "Once the decay of the waveform output by the LFO is allowed to develop in standa" +
                "rd fashion, the waveform then continues without further change.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "You must turn the Key Trigger parameter to “ON.” If this is “OFF,” it will have " +
                "no effect.\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex, 8, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_10.png",
                "Trapezoidal wave.\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex, 9, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_07.png",
                "Sample & Hold wave (one time per cycle, LFO value is changed).\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex, 10, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_06.png",
                "Chaos wave.\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex, 11, "Selects the waveform of the LFO.\r\n", "_SNS_Osc_11.png",
                "Modified sine wave. The amplitude of a sine wave is randomly varied once each cy" +
                "cle.\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex++, 12, "Selects the waveform of the LFO.\r\n", "",
                "A waveform generated by the data specified by LFO Step 1–64.\r\n" +
                "\r\n" +
                "This produces stepped change with a fixed pattern similar to a step modulator.\r\n", "", 0x2840);
            Add(0, 11, (byte)ItemIndex++, 0, "Adjusts the modulation rate, or speed, of the LFO.\r\n" +
                "\r\n" +
                "If you want the LFO rate to be synchronized with the tempo, specify the setting " +
                "as a note value relative to the synchronization tempo.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n" +
                "\r\n" +
                "(Example) For a tempo of 120 (120 quarter notes occur in 1 minute (60 seconds))\r\n", "_PCM_LFO_01.png",
                "NOTE\r\n" +
                "This setting will be ignored if the Waveform parameter is set to “CHAOS.”\r\n", "", 0x5540);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "LFO Rate Detune makes subtle changes in the LFO cycle rate (Rate parameter) each" +
                " time a key is pressed.\r\n" +
                "\r\n" +
                "Higher settings will cause greater change. This parameter is invalid when Rate i" +
                "s set to “note.”\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Raises or lowers the LFO waveform relative to the central value (pitch or cutoff" +
                " frequency).\r\n" +
                "\r\n" +
                "Positive “+” settings will move the waveform so that modulation will occur from " +
                "the central value upward. \r\n" +
                "\r\n" +
                "Negative “-” settings will move the waveform so that modulation will occur from " +
                "the central value downward.\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Delay Time (LFO Delay Time) specifies the time elapsed before the LFO effect is " +
                "applied (the effect continues) after the key is pressed (or released)." +
                "\r\n" +
                "\r\n" +
                "* After referring to “How to Apply the LFO” (p. 62), change the setting until th" +
                "e desired effect is achieved.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "When using violin, wind, or certain other instrument sounds in a performance, ra" +
                "ther than having vibrato added immediately after the sounds are played" +
                ", it can be effective to add the vibrato after the note is drawn out s" +
                "omewhat. If you set the Delay Time in conjunction with the Pitch Depth" +
                " parameter and Rate parameter, the vibrato will be applied automatical" +
                "ly following a certain interval after the key is pressed.\r\n" +
                "\r\n" +
                "This effect is called “Delay Vibrato.”\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Adjusts the value for the Delay Time parameter depending on the key position, re" +
                "lative to the C4 key (center C).\r\n" +
                "\r\n" +
                "To decrease the time that elapses before the LFO effect is applied (the effect i" +
                "s continuous) with each higher key that is pressed in the upper regist" +
                "ers, select a positive value. \r\n" +
                "\r\n" +
                "To increase the elapsed time, select a negative value. Larger settings will prod" +
                "uce greater change.\r\n" +
                "\r\n" +
                "If you do not want the elapsed time before the LFO effect is applied (the effect" +
                " is continuous) to change according to the key pressed, set this to “0" +
                ".”\r\n", "_PCM_Pitch_Time_01.png", 0x0068);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Specifies how the LFO will be applied.\r\n" +
                "\r\n" +
                "* After referring to “How to Apply the LFO” (p. 62), change the setting until th" +
                "e desired effect is achieved.\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Specifies the time over which the LFO amplitude will reach the maximum (minimum)" +
                ".\r\n" +
                "\r\n" +
                "* After referring to “How to Apply the LFO” (p. 62), change the setting until th" +
                "e desired effect is achieved.\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Specifies whether the LFO cycle will be synchronized to begin when the key is pr" +
                "essed (ON) or not (OFF).\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Specifies how deeply the LFO will affect pitch.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Specifies how deeply the LFO will affect the cutoff frequency.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Specifies how deeply the LFO will affect the volume.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n", "", 0x00e0);
            Add(0, 11, (byte)ItemIndex++, 0, "", "",
                "Specifies how deeply the LFO will affect the pan.\r\n" +
                "\r\n" +
                "* You can control this parameter using the Matrix Control.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "Positive “+” and negative “-” settings for the Depth parameter result in differi" +
                "ng kinds of change in pitch and volume. For example, if you set the De" +
                "pth parameter to a positive “+” value for one partial, and set another" +
                " partial to the same numerical value, but make it negative “-”, the mo" +
                "dulation phase for the two partials will be the reverse of each other." +
                " This allows you to shift back and forth between two different partial" +
                "s, or combine it with the Pan setting to cyclically change the locatio" +
                "n of the sound image.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "In the Pan Depth parameter settings, if the Structure Type parameter is set to a" +
                "ny value from “2” through “10,” the output of partial 1 and 2 will be " +
                "combined into partial 2, and the output of partial 3 and 4 will be com" +
                "bined into partial 4. For this reason, partial 1 will follow the setti" +
                "ngs of partial 2, and partial 3 will follow the settings of partial 4 " +
                "(p. 52).\r\n", "", 0x00e0);
            // PCM Synth Tone Step LFO tab
            ItemIndex = Skip;
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "When generating an LFO waveform from the data specified in LFO Step1–16, specify" +
                " whether the level will change abruptly at each step (TYP1) or will be" +
                " connected linearly (TYP2).\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 1.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 2.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 3.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 4.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 5.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 6.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 7.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 8.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 9.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 10.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 11.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 12.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 13.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 14.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 15.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            Add(0, 12, (byte)ItemIndex++, 0, "", "",
                "Specifies the data for the Step LFO step 16.\r\n" +
                "\r\n" +
                "If the LFO Pitch Depth is +63, each +1 unit of the step data corresponds to a pi" +
                "tch of +50 cents.\r\n", "", 0x00e0);
            // PCM Synth Tone Control tab
            ItemIndex = Skip;
            Add(0, 13, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 13, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 13, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 13, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 13, (byte)ItemIndex++, 0, "", "",
                "When a loop waveform is selected, the sound will normally continue as long as th" +
                "e key is pressed.\r\n" +
                "\r\n" +
                "If you want the sound to decay naturally even if the key remains pressed, set th" +
                "is to “NO SUS.”\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "If a one-shot type Wave is selected, it will not sustain even if this parameter " +
                "is set to “SUST.”\r\n", "", 0x00e0);
            Add(0, 13, (byte)ItemIndex++, 0, "", "",
                "For each partial, specify whether MIDI Pitch Bend messages will be received (ON)" +
                ", or not (OFF).\r\n", "", 0x00e0);
            Add(0, 13, (byte)ItemIndex++, 0, "", "",
                "For each partial, specify whether MIDI Expression messages will be received (ON)" +
                ", or not (OFF).\r\n", "", 0x00e0);
            Add(0, 13, (byte)ItemIndex++, 0, "", "",
                "For each partial, specify whether MIDI Hold-1 messages will be received (ON), or" +
                " not (OFF).\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "If “NO SUS” is selected for Env Mode parameter, this setting will have no effect" +
                ".\r\n", "", 0x00e0);
            Add(0, 13, (byte)ItemIndex++, 0, "", "",
                "You can specify, on an individual partial basis, whether or not the sound will b" +
                "e held when a Hold\r\n" +
                "1 message is received after a key is released, but before the sound has decayed " +
                "to silence.\r\n" +
                "\r\n" +
                "If you want to sustain the sound, set this “ON.”\r\n" +
                "\r\n" +
                "When using this function, also set the Rx Hold-1 parameter “ON.”\r\n" +
                "\r\n" +
                "This function is effective for piano sounds.\r\n", "", 0x00e0);
            // PCM Synth Tone MTRX Control tab
            ItemIndex = Skip;
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Selects the Matrix Control you wish to work on (1 - 4).", "", 0x00e0);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the partial parameter with the Matrix Contr" +
                "ol.\r\n" +
                "\r\n" +
                "OFF: Matrix control will not be used.\r\n" +
                "CC01–31, 33–95: Controller numbers 1–31, 33–95\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "SYS CTRL1–4: MIDI messages used as common matrix controls.\r\n" +
                "VELOCITY: Velocity (pressure you press a key with)\r\n" +
                "KEYFOLLOW: Keyfollow (keyboard position with C4 as 0)\r\n" +
                "TEMPO: Tempo specified by the tempo assign source, or the tempo of an external M" +
                "IDI sequencer\r\n" +
                "LFO1: LFO 1\r\n" +
                "LFO2: LFO 2\r\n" +
                "PITCH ENV: Pitch envelope\r\n" +
                "TVF ENV: TVF envelope\r\n" +
                "TVA ENV: TVA envelope\r\n" +
                "\r\n" +
                "* Velocity and Keyfollow correspond to Note messages.\r\n" +
                "* Although there are no MIDI messages for LFO 1 through TVA Envelope, they can b" +
                "e used as Matrix Control. In this case, you can change the partial set" +
                "tings in realtime by playing patches.\r\n" +
                "* If you want to use common controllers for the entire INTEGRA-7, select “SYS CT" +
                "RL1”–”SYS CTRL4.” MIDI messages used as System Control 1–4 are set wit" +
                "h the Tone Control 1–4 Src (p. 5).\r\n" +
                "\r\n" +
                "Reference\r\n" +
                "For more information about Control Change messages, please refer to “MIDI Implem" +
                "entation” and “Parameter Guide” (PDF).\r\n", "", 0x00e0);
            // ...control 1
            Add(0, 14, (byte)ItemIndex, 0, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "OFF: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 1, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Changing the Pitch.\r\n" +
                "\r\n" +
                "PITCH: Changes the pitch.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 2, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Opening and Closing the Filter.\r\n" +
                "\r\n" +
                "CUTOFF: Changes the cutoff frequency.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 3, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "RESONANCE: Emphasizes the overtones in the region of the cutoff frequency, addin" +
                "g character to the sound.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 4, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Changing the Volume.\r\n" +
                "\r\n" +
                "LEVEL: Changes the volume level.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 5, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Changing the Pan.\r\n" +
                "\r\n" +
                "PAN: Changes the pan.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 6, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.” Up t" +
                "o four parameters can be specified for each Matrix Control, and contro" +
                "lled simultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In this manual, Parameters that can be controlled using the Matrix Control (p. 6" +
                "3) are marked with a “*”.\r\n", "",
                "OUTPUT LEVEL: Changes the volume of the original sound.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 7, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.” Up t" +
                "o four parameters can be specified for each Matrix Control, and contro" +
                "lled simultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In this manual, Parameters that can be controlled using the Matrix Control (p. 6" +
                "3) are marked with a “*”.\r\n", "",
                "CHORUS SEND: Changes the amount of chorus.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 8, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.” Up t" +
                "o four parameters can be specified for each Matrix Control, and contro" +
                "lled simultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In this manual, Parameters that can be controlled using the Matrix Control (p. 6" +
                "3) are marked with a “*”.\r\n", "",
                "REVERB SEND: Changes the amount of reverb.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 9, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 PCH DEPTH: Changes the vibrato depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 10, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 PCH DEPTH: Changes the vibrato depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 11, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 TVF DEPTH: Changes the wah depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 12, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 TVF DEPTH: Changes the wah depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 13, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 TVA DEPTH: Changes the tremolo depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 14, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 TVA DEPTH: Changes the tremolo depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 15, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 PAN DEPTH: Changes the effect that the LFO will have on pan.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 16, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 PAN DEPTH: Changes the effect that the LFO will have on pan.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 17, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 RATE: Changes the speed of the LFO cycles. The speed will not change i" +
                "f LFO Rate is set to “note.”\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 18, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 RATE: Changes the speed of the LFO cycles. The speed will not change i" +
                "f LFO Rate is set to “note.”\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 19, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PIT ENV A-TIME: Changes the Env Time 1 of the pitch envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 20, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PIT ENV D-TIME: Changes the Env Time 2 and Env Time 3 of the pitch envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 21, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PIT ENV R-TIME: Changes the Env Time 4 of the pitch envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 22, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVF ENV A-TIME: Changes the Env Time 1 of the TVF envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 23, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVF ENV D-TIME: Changes the Env Time 2 and Env Time 3 of the TVF envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 24, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVF ENV R-TIME: Changes the Env Time 4 of the TVF envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 25, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVA ENV A-TIME: Changes the Env Time 1 of the TVA envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 26, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVA ENV D-TIME: Changes the Env Time 2 and Env Time 3 of the TVA envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 27, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVA ENV R-TIME: Changes the Env Time 4 of the TVA envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 28, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PMT\r\n" +
                "If the Matrix Control is used to split partials, set the PMT Velocity Control (p" +
                ". 53) to “OFF,” and the PMT Control Switch (p. 54) to “ON.”\r\n" +
                "\r\n" +
                "• If the Matrix Control is used to split partials, we recommend setting the Sens" +
                " (p. 64) to “+63.”\r\n" +
                "Selecting a lower value may prevent switching of the partials. Furthermore, if y" +
                "ou want to reverse the effect, set the value to “-63.”\r\n" +
                "\r\n" +
                "• If you want to use matrix control to switch smoothly between partials, use the" +
                " Velo Fade Lower and\r\n" +
                "Velo Fade Upper (p. 54). The higher the values set, the smoother the switch is b" +
                "etween the partials.", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 29, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "FXM DEPTH: Changing the Depth of Frequency Modulation Produced by FXM.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 30, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n1", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 31, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n2", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 32, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n3", "", 0x7070);
            Add(0, 14, (byte)ItemIndex++, 33, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n4", "", 0x7070);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Selects the partial to which the effect is applied when using the Matrix Control" +
                ".\r\n" +
                "\r\n" +
                "OFF: The effect will not be applied.\r\n" +
                "\r\n" +
                "ON: The effect will be applied.\r\n" +
                "\r\n" +
                "REVS: The effect will be applied in reverse.\r\n", "", 0x00e0);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the Matrix Control’s effect that is applied.\r\n" +
                "\r\n" +
                "If you wish to modify the selected parameter in a positive “+” direction – i.e.," +
                " a higher value, toward the right, or faster etc. – from its current s" +
                "etting, select a positive “+” value.\r\n" +
                "\r\n" +
                "If you wish to modify the selected parameter in a negative “-” direction – i.e.," +
                " a lower value, toward the left, or slower etc. – from its current set" +
                "ting, select a negative “-” value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change.\r\n" +
                "\r\n" +
                "Set this to “0” if you don’t want to apply the effect.\r\n", "", 0x00e0);
            // ...control 2
            Add(0, 14, (byte)ItemIndex, 0, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "OFF: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 1, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Changing the Pitch.\r\n" +
                "\r\n" +
                "PITCH: Changes the pitch.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 2, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Opening and Closing the Filter.\r\n" +
                "\r\n" +
                "CUTOFF: Changes the cutoff frequency.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 3, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "RESONANCE: Emphasizes the overtones in the region of the cutoff frequency, addin" +
                "g character to the sound.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 4, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Changing the Volume.\r\n" +
                "\r\n" +
                "LEVEL: Changes the volume level.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 5, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Changing the Pan.\r\n" +
                "\r\n" +
                "PAN: Changes the pan.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 6, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.” Up t" +
                "o four parameters can be specified for each Matrix Control, and contro" +
                "lled simultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In this manual, Parameters that can be controlled using the Matrix Control (p. 6" +
                "3) are marked with a “*”.\r\n", "",
                "OUTPUT LEVEL: Changes the volume of the original sound.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 7, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.” Up t" +
                "o four parameters can be specified for each Matrix Control, and contro" +
                "lled simultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In this manual, Parameters that can be controlled using the Matrix Control (p. 6" +
                "3) are marked with a “*”.\r\n", "",
                "CHORUS SEND: Changes the amount of chorus.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 8, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.” Up t" +
                "o four parameters can be specified for each Matrix Control, and contro" +
                "lled simultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In this manual, Parameters that can be controlled using the Matrix Control (p. 6" +
                "3) are marked with a “*”.\r\n", "",
                "REVERB SEND: Changes the amount of reverb.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 9, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 PCH DEPTH: Changes the vibrato depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 10, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 PCH DEPTH: Changes the vibrato depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 11, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 TVF DEPTH: Changes the wah depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 12, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 TVF DEPTH: Changes the wah depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 13, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 TVA DEPTH: Changes the tremolo depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 14, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 TVA DEPTH: Changes the tremolo depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 15, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 PAN DEPTH: Changes the effect that the LFO will have on pan.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 16, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 PAN DEPTH: Changes the effect that the LFO will have on pan.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 17, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 RATE: Changes the speed of the LFO cycles. The speed will not change i" +
                "f LFO Rate is set to “note.”\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 18, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 RATE: Changes the speed of the LFO cycles. The speed will not change i" +
                "f LFO Rate is set to “note.”\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 19, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PIT ENV A-TIME: Changes the Env Time 1 of the pitch envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 20, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PIT ENV D-TIME: Changes the Env Time 2 and Env Time 3 of the pitch envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 21, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PIT ENV R-TIME: Changes the Env Time 4 of the pitch envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 22, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVF ENV A-TIME: Changes the Env Time 1 of the TVF envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 23, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVF ENV D-TIME: Changes the Env Time 2 and Env Time 3 of the TVF envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 24, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVF ENV R-TIME: Changes the Env Time 4 of the TVF envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 25, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVA ENV A-TIME: Changes the Env Time 1 of the TVA envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 26, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVA ENV D-TIME: Changes the Env Time 2 and Env Time 3 of the TVA envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 27, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVA ENV R-TIME: Changes the Env Time 4 of the TVA envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 28, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PMT\r\n" +
                "If the Matrix Control is used to split partials, set the PMT Velocity Control (p" +
                ". 53) to “OFF,” and the PMT Control Switch (p. 54) to “ON.”\r\n" +
                "\r\n" +
                "• If the Matrix Control is used to split partials, we recommend setting the Sens" +
                " (p. 64) to “+63.”\r\n" +
                "Selecting a lower value may prevent switching of the partials. Furthermore, if y" +
                "ou want to reverse the effect, set the value to “-63.”\r\n" +
                "\r\n" +
                "• If you want to use matrix control to switch smoothly between partials, use the" +
                " Velo Fade Lower and\r\n" +
                "Velo Fade Upper (p. 54). The higher the values set, the smoother the switch is b" +
                "etween the partials.", "", 0x7070);
            Add(0, 14, (byte)ItemIndex++, 29, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "FXM DEPTH: Changing the Depth of Frequency Modulation Produced by FXM.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 30, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 31, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 32, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex++, 33, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the Matrix Control’s effect that is applied.\r\n" +
                "\r\n" +
                "If you wish to modify the selected parameter in a positive “+” direction – i.e.," +
                " a higher value, toward the right, or faster etc. – from its current s" +
                "etting, select a positive “+” value.\r\n" +
                "\r\n" +
                "If you wish to modify the selected parameter in a negative “-” direction – i.e.," +
                " a lower value, toward the left, or slower etc. – from its current set" +
                "ting, select a negative “-” value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change.\r\n" +
                "\r\n" +
                "Set this to “0” if you don’t want to apply the effect.\r\n", "", 0x00e0);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Selects the partial to which the effect is applied when using the Matrix Control" +
                ".\r\n" +
                "\r\n" +
                "OFF: The effect will not be applied.\r\n" +
                "\r\n" +
                "ON: The effect will be applied.\r\n" +
                "\r\n" +
                "REVS: The effect will be applied in reverse.\r\n", "", 0x00e0);
            // ...control 3
            Add(0, 14, (byte)ItemIndex, 0, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "OFF: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 1, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Changing the Pitch.\r\n" +
                "\r\n" +
                "PITCH: Changes the pitch.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 2, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Opening and Closing the Filter.\r\n" +
                "\r\n" +
                "CUTOFF: Changes the cutoff frequency.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 3, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "RESONANCE: Emphasizes the overtones in the region of the cutoff frequency, addin" +
                "g character to the sound.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 4, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Changing the Volume.\r\n" +
                "\r\n" +
                "LEVEL: Changes the volume level.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 5, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Changing the Pan.\r\n" +
                "\r\n" +
                "PAN: Changes the pan.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 6, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.” Up t" +
                "o four parameters can be specified for each Matrix Control, and contro" +
                "lled simultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In this manual, Parameters that can be controlled using the Matrix Control (p. 6" +
                "3) are marked with a “*”.\r\n", "",
                "OUTPUT LEVEL: Changes the volume of the original sound.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 7, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.” Up t" +
                "o four parameters can be specified for each Matrix Control, and contro" +
                "lled simultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In this manual, Parameters that can be controlled using the Matrix Control (p. 6" +
                "3) are marked with a “*”.\r\n", "",
                "CHORUS SEND: Changes the amount of chorus.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 8, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.” Up t" +
                "o four parameters can be specified for each Matrix Control, and contro" +
                "lled simultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In this manual, Parameters that can be controlled using the Matrix Control (p. 6" +
                "3) are marked with a “*”.\r\n", "",
                "REVERB SEND: Changes the amount of reverb.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 9, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 PCH DEPTH: Changes the vibrato depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 10, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 PCH DEPTH: Changes the vibrato depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 11, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 TVF DEPTH: Changes the wah depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 12, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 TVF DEPTH: Changes the wah depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 13, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 TVA DEPTH: Changes the tremolo depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 14, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 TVA DEPTH: Changes the tremolo depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 15, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 PAN DEPTH: Changes the effect that the LFO will have on pan.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 16, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 PAN DEPTH: Changes the effect that the LFO will have on pan.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 17, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 RATE: Changes the speed of the LFO cycles. The speed will not change i" +
                "f LFO Rate is set to “note.”\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 18, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 RATE: Changes the speed of the LFO cycles. The speed will not change i" +
                "f LFO Rate is set to “note.”\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 19, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PIT ENV A-TIME: Changes the Env Time 1 of the pitch envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 20, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PIT ENV D-TIME: Changes the Env Time 2 and Env Time 3 of the pitch envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 21, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PIT ENV R-TIME: Changes the Env Time 4 of the pitch envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 22, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVF ENV A-TIME: Changes the Env Time 1 of the TVF envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 23, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVF ENV D-TIME: Changes the Env Time 2 and Env Time 3 of the TVF envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 24, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVF ENV R-TIME: Changes the Env Time 4 of the TVF envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 25, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVA ENV A-TIME: Changes the Env Time 1 of the TVA envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 26, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVA ENV D-TIME: Changes the Env Time 2 and Env Time 3 of the TVA envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 27, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVA ENV R-TIME: Changes the Env Time 4 of the TVA envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 28, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PMT\r\n" +
                "If the Matrix Control is used to split partials, set the PMT Velocity Control (p" +
                ". 53) to “OFF,” and the PMT Control Switch (p. 54) to “ON.”\r\n" +
                "\r\n" +
                "• If the Matrix Control is used to split partials, we recommend setting the Sens" +
                " (p. 64) to “+63.”\r\n" +
                "Selecting a lower value may prevent switching of the partials. Furthermore, if y" +
                "ou want to reverse the effect, set the value to “-63.”\r\n" +
                "\r\n" +
                "• If you want to use matrix control to switch smoothly between partials, use the" +
                " Velo Fade Lower and\r\n" +
                "Velo Fade Upper (p. 54). The higher the values set, the smoother the switch is b" +
                "etween the partials.", "", 0x7070);
            Add(0, 14, (byte)ItemIndex++, 29, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "FXM DEPTH: Changing the Depth of Frequency Modulation Produced by FXM.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 30, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 31, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 32, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex++, 33, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the Matrix Control’s effect that is applied.\r\n" +
                "\r\n" +
                "If you wish to modify the selected parameter in a positive “+” direction – i.e.," +
                " a higher value, toward the right, or faster etc. – from its current s" +
                "etting, select a positive “+” value.\r\n" +
                "\r\n" +
                "If you wish to modify the selected parameter in a negative “-” direction – i.e.," +
                " a lower value, toward the left, or slower etc. – from its current set" +
                "ting, select a negative “-” value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change.\r\n" +
                "\r\n" +
                "Set this to “0” if you don’t want to apply the effect.\r\n", "", 0x00e0);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Selects the partial to which the effect is applied when using the Matrix Control" +
                ".\r\n" +
                "\r\n" +
                "OFF: The effect will not be applied.\r\n" +
                "\r\n" +
                "ON: The effect will be applied.\r\n" +
                "\r\n" +
                "REVS: The effect will be applied in reverse.\r\n", "", 0x00e0);
            // ...control 4
            Add(0, 14, (byte)ItemIndex, 0, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "OFF: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 1, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Changing the Pitch.\r\n" +
                "\r\n" +
                "PITCH: Changes the pitch.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 2, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Opening and Closing the Filter.\r\n" +
                "\r\n" +
                "CUTOFF: Changes the cutoff frequency.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 3, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "RESONANCE: Emphasizes the overtones in the region of the cutoff frequency, addin" +
                "g character to the sound.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 4, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Changing the Volume.\r\n" +
                "\r\n" +
                "LEVEL: Changes the volume level.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 5, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "Changing the Pan.\r\n" +
                "\r\n" +
                "PAN: Changes the pan.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 6, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.” Up t" +
                "o four parameters can be specified for each Matrix Control, and contro" +
                "lled simultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In this manual, Parameters that can be controlled using the Matrix Control (p. 6" +
                "3) are marked with a “*”.\r\n", "",
                "OUTPUT LEVEL: Changes the volume of the original sound.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 7, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.” Up t" +
                "o four parameters can be specified for each Matrix Control, and contro" +
                "lled simultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In this manual, Parameters that can be controlled using the Matrix Control (p. 6" +
                "3) are marked with a “*”.\r\n", "",
                "CHORUS SEND: Changes the amount of chorus.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 8, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.” Up t" +
                "o four parameters can be specified for each Matrix Control, and contro" +
                "lled simultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In this manual, Parameters that can be controlled using the Matrix Control (p. 6" +
                "3) are marked with a “*”.\r\n", "",
                "REVERB SEND: Changes the amount of reverb.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 9, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 PCH DEPTH: Changes the vibrato depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 10, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 PCH DEPTH: Changes the vibrato depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 11, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 TVF DEPTH: Changes the wah depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 12, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 TVF DEPTH: Changes the wah depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 13, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 TVA DEPTH: Changes the tremolo depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 14, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 TVA DEPTH: Changes the tremolo depth.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 15, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 PAN DEPTH: Changes the effect that the LFO will have on pan.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 16, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 PAN DEPTH: Changes the effect that the LFO will have on pan.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 17, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO1 RATE: Changes the speed of the LFO cycles. The speed will not change i" +
                "f LFO Rate is set to “note.”\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 18, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "LFO2 RATE: Changes the speed of the LFO cycles. The speed will not change i" +
                "f LFO Rate is set to “note.”\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 19, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PIT ENV A-TIME: Changes the Env Time 1 of the pitch envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 20, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PIT ENV D-TIME: Changes the Env Time 2 and Env Time 3 of the pitch envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 21, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PIT ENV R-TIME: Changes the Env Time 4 of the pitch envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 22, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVF ENV A-TIME: Changes the Env Time 1 of the TVF envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 23, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVF ENV D-TIME: Changes the Env Time 2 and Env Time 3 of the TVF envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 24, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVF ENV R-TIME: Changes the Env Time 4 of the TVF envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 25, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVA ENV A-TIME: Changes the Env Time 1 of the TVA envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 26, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVA ENV D-TIME: Changes the Env Time 2 and Env Time 3 of the TVA envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 27, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "TVA ENV R-TIME: Changes the Env Time 4 of the TVA envelope.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 28, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "PMT\r\n" +
                "If the Matrix Control is used to split partials, set the PMT Velocity Control (p" +
                ". 53) to “OFF,” and the PMT Control Switch (p. 54) to “ON.”\r\n" +
                "\r\n" +
                "• If the Matrix Control is used to split partials, we recommend setting the Sens" +
                " (p. 64) to “+63.”\r\n" +
                "Selecting a lower value may prevent switching of the partials. Furthermore, if y" +
                "ou want to reverse the effect, set the value to “-63.”\r\n" +
                "\r\n" +
                "• If you want to use matrix control to switch smoothly between partials, use the" +
                " Velo Fade Lower and\r\n" +
                "Velo Fade Upper (p. 54). The higher the values set, the smoother the switch is b" +
                "etween the partials.", "", 0x7070);
            Add(0, 14, (byte)ItemIndex++, 29, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the “Parameter Guide” manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "FXM DEPTH: Changing the Depth of Frequency Modulation Produced by FXM.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 30, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 31, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex, 32, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex++, 33, "Selects the partial parameter that is to be controlled when using the Matrix Con" +
                "trol.\r\n" +
                "When not controlling parameters with the Matrix Control, set this to “OFF.”\r\n" +
                "Up to four parameters can be specified for each Matrix Control, and controlled s" +
                "imultaneously.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "In the \"Parameter Guide\" manual, Parameters that can be controlled using the Mat" +
                "rix Control (p. 63) are marked with a “*”.\r\n", "",
                "If you’re not using Matrix Control.\r\n" +
                "\r\n" +
                "---: Matrix Control will not be used.\r\n", "", 0x7070);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the Matrix Control’s effect that is applied.\r\n" +
                "\r\n" +
                "If you wish to modify the selected parameter in a positive “+” direction – i.e.," +
                " a higher value, toward the right, or faster etc. – from its current s" +
                "etting, select a positive “+” value.\r\n" +
                "\r\n" +
                "If you wish to modify the selected parameter in a negative “-” direction – i.e.," +
                " a lower value, toward the left, or slower etc. – from its current set" +
                "ting, select a negative “-” value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change.\r\n" +
                "\r\n" +
                "Set this to “0” if you don’t want to apply the effect.\r\n", "", 0x00e0);
            Add(0, 14, (byte)ItemIndex++, 0, "", "",
                "Selects the partial to which the effect is applied when using the Matrix Control" +
                ".\r\n" +
                "\r\n" +
                "OFF: The effect will not be applied.\r\n" +
                "\r\n" +
                "ON: The effect will be applied.\r\n" +
                "\r\n" +
                "REVS: The effect will be applied in reverse.\r\n", "", 0x00e0);
            // PCM Synth Tone MFX tab: (Handled under MFX, last type below)

            // PCM Synth Tone MFX Control tab:
            ItemIndex = Skip;
            Add(0, 16, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(0, 16, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(0, 16, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(0, 16, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(0, 16, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(0, 16, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(0, 16, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(0, 16, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(0, 16, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(0, 16, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(0, 16, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(0, 16, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            // PCM Synth Tone Save tab:
            ItemIndex = Skip;
            Add(0, 17, (byte)ItemIndex++, 0, "", "",
                "Type in a name for the tone.\r\n" +
                "\r\n" +
                "Names can be up to 12 characters and will be truncated if you enter more than 12.\r\n" +
                "\r\n" +
                "This name will be displayed on the INTEGRA-7.\r\n", "", 0x00e0);
            Add(0, 17, (byte)ItemIndex++, 0, "", "",
                "Select a slot to save the tone in.\r\n", "", 0x00e0);
            Add(0, 17, (byte)ItemIndex++, 0, "", "",
                "Saves the tone in the selected slot.\r\n", "", 0x00e0);
            Add(0, 17, (byte)ItemIndex++, 0, "", "",
                "Deletes the tone in the selected slot.\r\n\r\n" + 
                "This has no effect on the tone you are editing, you can still save it in any slot.", "", 0x00e0);

            // PCM Drum Kit Common tab:
            ItemIndex = Skip;
            Add(1, 0, (byte)ItemIndex++, 0, "", "", "Pressing and holding the volume control on the" +
                " INTEGRA-7 will make it play a phrase. You can select what phrase will be played here. " +
                "Default is a phrase specifically created for the current drum kit.\r\n\r\nYou can use the \'Play\'" +
                " button below to play/stop the selected phrase.");
            Add(1, 0, (byte)ItemIndex++, 0, "", "",
                "Assign Type sets the way sounds are played when the same key is pressed a number" +
                " of times.\r\n" +
                "\r\n" +
                "MULTI: Layer the sound of the same keys. Even with continuous sounds where the s" +
                "ound plays for an extended time, such as with crash cymbals, the sound" +
                "s are layered, without previously played sounds being eliminated.\r\n" +
                "\r\n" +
                "SINGLE: Only one sound can be played at a time when the same key is pressed. Wit" +
                "h continuous sounds where the sound plays for an extended time, the pr" +
                "evious sound is stopped when the following sound is played.\r\n", "", 0x00e0);
            Add(1, 0, (byte)ItemIndex++, 0, "", "",
                "Sets the volume of the drum kit.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "The volume levels of the partials from which the drum kit is composed is set wit" +
                "h the Partial Level parameter (p. 70). The volume levels of the Waves " +
                "from which the drum partial is composed is set with the Wave Level par" +
                "ameter (p. 67).\r\n", "", 0x00e0);
            Add(1, 0, (byte)ItemIndex++, 0, "", "",
                "On an actual acoustic drum set, an open hi-hat and a closed hi-hat sound can nev" +
                "er occur simultaneously. To reproduce the reality of this situation, y" +
                "ou can set up a Mute Group.\r\n" +
                "\r\n" +
                "The Mute Group function allows you to designate two or more drum partials that a" +
                "re not allowed to sound simultaneously. Up to 31 Mute Groups can be us" +
                "ed.\r\n" +
                "\r\n" +
                "Drum partials that do not belong to any such group should be set to “OFF.”\r\n", "", 0x00e0);
            Add(1, 0, (byte)ItemIndex++, 0, "", "",
                "When a loop waveform is selected, the sound will normally continue as long as th" +
                "e key is pressed.\r\n" +
                "\r\n" +
                "If you want the sound to decay naturally even if the key remains pressed, set th" +
                "is to “NO-SUS”.\r\n" +
                "\r\n" +
                "* If a one-shot type Wave is selected, it will not sustain even if this paramete" +
                "r is set to “SUSTAIN”.\r\n", "", 0x00e0);
            Add(1, 0, (byte)ItemIndex++, 0, "", "",
                "Specifies the amount of pitch change in semitones (4 octaves) that will occur wh" +
                "en the Pitch Bend Lever is moved.\r\n" +
                "\r\n" +
                "The amount of change when the lever is tilted is set to the same value for both " +
                "left and right sides.\r\n", "", 0x00e0);
            Add(1, 0, (byte)ItemIndex++, 0, "", "",
                "For each drum partial, specify whether MIDI Expression messages will be received" +
                " (ON), or not (OFF).\r\n", "", 0x00e0);
            Add(1, 0, (byte)ItemIndex++, 0, "", "",
                "For each drum partial, specify whether MIDI Hold-1 messages will be received (ON" +
                "), or not (OFF).\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "If “NO-SUS” is selected for Partial Env Mode parameter (p. 66), this setting wil" +
                "l have no effect.\r\n", "", 0x00e0);
            Add(1, 0, (byte)ItemIndex++, 0, "", "",
                "The sound will play back until the end of the waveform (or the end of the envelo" +
                "pe, whichever comes first).\r\n" +
                "\r\n" +
                "The result will be the same as when the envelope’s Partial Env Mode parameter is" +
                " set to “NO-SUS”.\r\n", "", 0x00e0);
            // PCM Drum Kit Wave tab:
            ItemIndex = Skip;
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "Select the groups containing the Waves comprising the drum partial.\r\n" +
                "\r\n" +
                "INT: Waveforms stored in internal.\r\n" +
                "\r\n" +
                "SRX: Expansion sound banks. Don't forget to load any expansion sound" +
                " bank you intend to use!", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "This selects the left of the one or two Waves comprising the drum partial. " +
                "(Along with the Wave number, " +
                "the Wave name appears at the lower part of the INTEGRA-7 display.)\r\n" +
                "\r\n" +
                "When in monaural mode, only the left side (L) is specified. When in stereo, the " +
                "right side (R) is also specified.\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "This selects the right wave comprising the drum partial. \r\n" +
                "\r\n" +
                "When in monaural mode, only the left side (L) is specified. When in stereo, this " +
                "right side (R) is also specified.\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "Sets the gain (amplification) of the waveform.\r\n" +
                "\r\n" +
                "The value changes in 6 dB (decibel) steps—an increase of 6 dB doubles the wavefo" +
                "rm’s gain.\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "When you wish to synchronize a Phrase Loop to the clock (tempo), set this to “ON" +
                ".”\r\n" +
                "\r\n" +
                "This is valid only when an SRX waveform which indicates a tempo (BPM) is selecte" +
                "d. (Example: SRX05 3:080:BladeBtL, SRX08 5:75:BoomRvBel, etc.)\r\n" +
                "\r\n" +
                "If a waveform from an SRX is selected for the partial, turning the Wave Tempo Sy" +
                "nc parameter “ON” will cause pitch-related settings and FXM-related se" +
                "ttings to be ignored.\r\n" +
                "\r\n" +
                "Phrase Loop\r\n" +
                "“Phrase Loop” refers to the repeated playback of a phrase that’s been pulled out" +
                " of a song (e.g., by using a sampler). One technique involving the use" +
                " of Phrase Loops is the excerpting of a Phrase from a pre-existing son" +
                "g in a certain genre, for example dance music, and then creating a new" +
                " song with that Phrase used as the basic motif.\r\n" +
                "\r\n" +
                "This is referred to as “Break Beats.”\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "This sets whether FXM will be used (ON) or not (OFF).\r\n" +
                "\r\n" +
                "FXM\r\n" +
                "FXM (Frequency Cross Modulation) uses a specified waveform to apply frequency mo" +
                "dulation to the currently selected waveform, creating complex overtone" +
                "s. This is useful for creating dramatic sounds or sound effects.\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies how FXM will perform frequency modulation.\r\n" +
                "\r\n" +
                "Higher settings result in a grainier sound, while lower settings result in a mor" +
                "e metallic sound.\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies the depth of the modulation produced by FXM.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "When the Tempo Sync is set to “ON,” settings related to Pitch (p. 68) and FXM ar" +
                "e disabled.\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "Adjusts the pitch of the waveform’s sound up or down in semitone steps (+/-4 oct" +
                "aves).\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "The Coarse Tune of the entire drum partial is set by the Partial Coarse Tune (p." +
                " 68).\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "Adjusts the pitch of the waveform’s sound up or down in 1-cent steps (+/-50 cent" +
                "s).\r\n" +
                "\r\n" +
                "* One cent is 1/100th of a semitone.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "The Fine Tune of the entire drum partial is set by the Partial Fine Tune (p. 68)" +
                ".\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "You can set the volume of the waveform.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "The volume level of each drum partial is set with the Partial Level; the volume " +
                "levels of the entire drum kit is set with the Drum Kit Level (p. 66*).\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "This specifies the pan of the waveform.\r\n" +
                "\r\n" +
                "“L64” is far left, “0” is center, and “63R” is far right.\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "Use this setting to cause the waveform’s panning to change randomly each time a " +
                "key is pressed (ON) or not (OFF).\r\n" +
                "\r\n" +
                "* The range of the panning change is set by the Random Pan Depth (p. 71).\r\n", "", 0x00e0);
            Add(1, 1, (byte)ItemIndex++, 0, "", "",
                "This setting causes panning of the waveform to be alternated between left and ri" +
                "ght each time a key is pressed.\r\n" +
                "\r\n" +
                "Set Alter Pan Sw to “ON” to pan the Wave according to the Alter Pan Depth settin" +
                "gs, or to “REVS” when you want the panning reversed.\r\n" +
                "\r\n" +
                "If you do not want the panning to change each time a key is pressed, set this to" +
                " “OFF.”\r\n", "", 0x00e0);
            // PCM Drum Kit WMT tab:
            ItemIndex = Skip;
            Add(1, 2, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 1 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(1, 2, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 2 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(1, 2, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 3 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(1, 2, (byte)ItemIndex++, 0, "", "",
                "Switch for partial 4 sound. Turn on if you wish to use and hear it.\r\n", "", 0x00e0);
            Add(1, 2, (byte)ItemIndex++, 0, "", "",
                "WMT Velocity Control determines whether a different drum partial is played (ON) " +
                "or not (OFF) depending on the force with which the key is played (velo" +
                "city).\r\n" +
                "\r\n" +
                "When set to “RANDOM,” the drum kit’s constituent drum partials will sound random" +
                "ly, regardless of any Velocity messages.\r\n", "", 0x00e0);
            Add(1, 2, (byte)ItemIndex++, 0, "", "",
                "This determines what will happen to the tone’s level when the tone is played at " +
                "a velocity greater than its specified velocity range. Higher settings " +
                "produce a more gradual change in volume. If you want notes played outs" +
                "ide the specified key velocity range to not be sounded at all, set thi" +
                "s to “0.”\r\n", "_PCM-D_WMT_04.png", 0x0068);
            Add(1, 2, (byte)ItemIndex++, 0, "", "",
                "This sets the highest velocity at which the waveform will sound. Make these sett" +
                "ings when you want different waveforms to sound in response to notes p" +
                "layed at different strengths.\r\n", "_PCM-D_WMT_03.png", 0x0068);
            Add(1, 2, (byte)ItemIndex++, 0, "", "",
                "This sets the lowest velocity at which the waveform will sound. Make these setti" +
                "ngs when you want different waveforms to sound in response to notes pl" +
                "ayed at different strengths.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "If you attempt to set the Lower velocity limit above the Upper, or the Upper bel" +
                "ow the Lower, the other value will automatically be adjusted to the sa" +
                "me setting.\r\n", "_PCM-D_WMT_02.png", 0x0068);
            Add(1, 2, (byte)ItemIndex++, 0, "", "",
                "This determines what will happen to the tone’s level when the tone is played at " +
                "a velocity lower than its specified velocity range. Higher settings pr" +
                "oduce a more gradual change in volume.\r\n" +
                "\r\n" +
                "If you want notes played outside the specified key velocity range to not be soun" +
                "ded at all, set this to “0.”\r\n", "_PCM-D_WMT_01.png", 0x0068);
            // PCM Drum Kit Pitch tab:
            ItemIndex = Skip;
            Add(1, 3, (byte)ItemIndex++, 0, "", "",
                "Selects the pitch at which a drum partial sounds.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "Set the coarse tuning for Waves comprising the drum partials with the Wave Coars" +
                "e Tune parameter (p. 67).\r\n", "", 0x00e0);
            Add(1, 3, (byte)ItemIndex++, 0, "", "",
                "Adjusts the pitch of the drum partial’s sound up or down in 1-cent steps (+/-50 " +
                "cents).\r\n" +
                "\r\n" +
                "* One cent is 1/100th of a semitone.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "Set the fine tuning for Waves comprising the drum partials with the Wave Fine Tu" +
                "ne parameter (p. 67).\r\n", "", 0x00e0);
            Add(1, 3, (byte)ItemIndex++, 0, "", "",
                "This specifies the width of random pitch deviation that will occur each time a k" +
                "ey is pressed.\r\n" +
                "\r\n" +
                "If you do not want the pitch to change randomly, set this to “0.”\r\n" +
                "\r\n" +
                "These values are in units of cents (1/100th of a semitone).\r\n", "", 0x00e0);
            // PCM Drum Kit Pitch Env tab:
            ItemIndex = Skip;
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "Adjusts the effect of the Pitch Envelope.\r\n" +
                "\r\n" +
                "Higher settings will cause the pitch envelope to produce greater change.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the shape of the envelope.\r\n", "_PCM_PitchEnv_02.png", 0x0068);
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "Keyboard playing dynamics can be used to control the depth of the pitch envelope" +
                ".\r\n" +
                "\r\n" +
                "If you want the pitch envelope to have more effect for strongly played notes, se" +
                "t this parameter to a positive “+” value.\r\n" +
                "\r\n" +
                "If you want the pitch envelope to have less effect for strongly played notes, se" +
                "t this to a negative “-” value.\r\n", "_PCM_PitchEnv_02.png", 0x0068);
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "This allows keyboard dynamics to affect the Time 1 of the Pitch envelope.\r\n" +
                "\r\n" +
                "If you want Time 1 to be speeded up for strongly played notes, set this paramete" +
                "r to a positive “+” value.\r\n" +
                "\r\n" +
                "If you want it to be slowed down, set this to a negative “-” value.\r\n", "_PCM_PitchEnv_02.png", 0x0068);
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "Use this parameter when you want key release speed to affect the Time 4 value of" +
                " the pitch envelope.\r\n" +
                "\r\n" +
                "If you want Time 4 to be speeded up for quickly released notes, set this paramet" +
                "er to a positive “+” value.\r\n" +
                "\r\n" +
                "If you want it to be slowed down, set this to a negative “-” value.\r\n", "_PCM_PitchEnv_02.png", 0x0068);
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "Specify the pitch envelope time 1.\r\n" +
                "\r\n" +
                "Higher settings will result in a longer time until pitch level 1 is reached.", "_PCM_PitchEnv_02.png", 0x0068);
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "Specify the pitch envelope time 2.\r\n" +
                "\r\n" +
                "Higher settings will result in a longer time until pitch level 2 is reached.", "_PCM_PitchEnv_02.png", 0x0068);
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "Specify the pitch envelope time 3.\r\n" +
                "\r\n" +
                "Higher settings will result in a longer time until pitch level 3 is reached.", "_PCM_PitchEnv_02.png", 0x0068);
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "Specify the pitch envelope time 4.\r\n" +
                "\r\n" +
                "Higher settings will result in a longer time until pitch level 4 is reached.", "_PCM_PitchEnv_02.png", 0x0068);
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "Specify the pitch envelope level 0.\r\n" +
                "\r\n" +
                "This is the initial pitch at Note on.\r\n" +
                "\r\n" +
                "Positive “+” settings will cause the pitch to be higher than the standard pitch," +
                " and negative\r\n" +
                "“-” settings will cause it to be lower.\r\n", "_PCM_PitchEnv_02.png", 0x0068);
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "Specify the pitch envelope level 1.\r\n" +
                "\r\n" +
                "This is the pitch reached after time 1.\r\n" +
                "\r\n" +
                "Positive “+” settings will cause the pitch to be higher than the standard pitch," +
                " and negative\r\n" +
                "“-” settings will cause it to be lower.\r\n", "_PCM_PitchEnv_02.png", 0x0068);
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "Specify the pitch envelope level 2.\r\n" +
                "\r\n" +
                "This is the pitch reached after time 2.\r\n" +
                "\r\n" +
                "Positive “+” settings will cause the pitch to be higher than the standard pitch," +
                " and negative\r\n" +
                "“-” settings will cause it to be lower.\r\n", "_PCM_PitchEnv_02.png", 0x0068);
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "Specify the pitch envelope level 3.\r\n" +
                "\r\n" +
                "This is the pitch reached after time 3.\r\n" +
                "\r\n" +
                "Positive “+” settings will cause the pitch to be higher than the standard pitch," +
                " and negative\r\n" +
                "“-” settings will cause it to be lower.\r\n", "_PCM_PitchEnv_02.png", 0x0068);
            Add(1, 4, (byte)ItemIndex++, 0, "", "",
                "Specify the pitch envelope level 4.\r\n" +
                "\r\n" +
                "This is the pitch reached after time 4, which starts at note off.\r\n" +
                "\r\n" +
                "Positive “+” settings will cause the pitch to be higher than the standard pitch," +
                " and negative\r\n" +
                "“-” settings will cause it to be lower.\r\n", "_PCM_PitchEnv_02.png", 0x0068);
            // PCM Drum Kit TVF tab:
            ItemIndex = Skip;
            Add(1, 5, (byte)ItemIndex, 0, "Selects the type of filter. A filter cuts or boosts a specific frequency region " +
                "to change a sound’s brightness, thickness, or other qualities.\r\n", "_PCM_TVF_00.png",
                "No filter selected.\r\n", "", 0x2660);
            Add(1, 5, (byte)ItemIndex, 1, "Selects the type of filter. A filter cuts or boosts a specific frequency region " +
                "to change a sound’s brightness, thickness, or other qualities.\r\n", "_PCM_TVF_01.png",
                "LPF: Low Pass Filter. \r\n" +
                "\r\n" +
                "This reduces the volume of all frequencies above the cutof" +
                "f frequency in order to round off, or un-brighten the so" +
                "und. This is the most common filter used in synthesizers.\r\n", "", 0x2660);
            Add(1, 5, (byte)ItemIndex, 2, "Selects the type of filter. A filter cuts or boosts a specific frequency region " +
                "to change a sound’s brightness, thickness, or other qualities.\r\n", "_PCM_TVF_02.png",
                "BPF: Band Pass Filter. \r\n" +
                "\r\n" +
                "This leaves only the frequencies in the region of the cut" +
                "off frequency, and cuts the rest. This can be usefu" +
                "l when creating distinctive sounds.\r\n", "", 0x2660);
            Add(1, 5, (byte)ItemIndex, 3, "Selects the type of filter. A filter cuts or boosts a specific frequency region " +
                "to change a sound’s brightness, thickness, or other qualities.\r\n", "_PCM_TVF_03.png",
                "HPF: High Pass Filter.\r\n" +
                "\r\n" +
                "This cuts the frequencies in the region below the cutoff frequency" +
                ". This is suitable for creating percussive sounds emphasizing the" +
                "ir higher ones.\r\n", "", 0x2660);
            Add(1, 5, (byte)ItemIndex, 4, "Selects the type of filter. A filter cuts or boosts a specific frequency region " +
                "to change a sound’s brightness, thickness, or other qualities.\r\n", "_PCM_TVF_04.png",
                "PKG: Peaking Filter. \r\n" +
                "\r\n" +
                "This emphasizes the frequencies in the region of the cutoff frequency." +
                "You can use this to create wah-wah effects by employing an L" +
                "FO to change the cutoff frequency cyclically.\r\n", "", 0x2660);
            Add(1, 5, (byte)ItemIndex, 5, "Selects the type of filter. A filter cuts or boosts a specific frequency region " +
                "to change a sound’s brightness, thickness, or other qualities.\r\n", "_PCM_TVF_01.png",
                "LPF2: Low Pass Filter 2. \r\n" +
                "\r\n" +
                "Although frequency components above the Cutoff frequency are " +
                "cut, the sensitivity of this filter is half that of the LPF. This make" +
                "s it a comparatively warmer low pass filter. .\r\n" +
                "\r\n" +
                "This filter is good for u" +
                "se with simulated instrument sounds such as the acoustic piano.\r\n" +
                "NOTE\r\n\r\nIf you set “LPF2” or “LPF3,” the setting for the Resonance parameter " +
                "will be ignored.", "", 0x2660);
            Add(1, 5, (byte)ItemIndex++, 6, "Selects the type of filter. A filter cuts or boosts a specific frequency region " +
                "to change a sound’s brightness, thickness, or other qualities.\r\n", "_PCM_TVF_01.png",
                "LPF3: Low Pass Filter 3. \r\n" +
                "\r\n" +
                "Although frequency components above the Cutoff frequency are " +
                "cut, the sensitivity of this filter changes according to the Cutoff fr" +
                "equency.\r\n" +
                "\r\n" +
                "While this filter is also good for use with simulated acoustic instrument sounds" +
                ", the nuance it exhibits differs from that of the LPF2, even with the " +
                "same TVF Envelope settings.\r\n\r\n" +
                "NOTE\r\n\r\nIf you set “LPF2” or “LPF3,” the setting for the Resonance parameter " +
                "will be ignored.", "", 0x2660);
            Add(1, 5, (byte)ItemIndex++, 0, "", "",
                "Selects the frequency at which the filter begins to have an effect on the wavefo" +
                "rm’s frequency components.\r\n" +
                "\r\n" +
                "With “LPF/LPF2/LPF3” selected for the Filter Type parameter, lower cutoff freque" +
                "ncy settings reduce a tone’s upper harmonics for a more rounded, warme" +
                "r sound. Higher settings make it sound brighter.\r\n" +
                "\r\n" +
                "If “BPF” is selected, harmonic components will change depending on the TVF Cutof" +
                "f Frequency setting. This can be useful when creating distinctive soun" +
                "ds.\r\n" +
                "\r\n" +
                "With “HPF” selected, higher Cutoff Frequency settings will reduce lower harmonic" +
                "s to emphasize just the brighter components of the sound.\r\n" +
                "\r\n" +
                "With “PKG” selected, the harmonics to be emphasized will vary depending on Cutof" +
                "f Frequency\r\n" +
                "setting.\r\n", "", 0x00e0);
            Add(1, 5, (byte)ItemIndex++, 0, "", "",
                "Emphasizes the portion of the sound in the region of the cutoff frequency, addin" +
                "g character to the sound. Excessively high settings can produce oscill" +
                "ation, causing the sound to distort.\r\n", "_PCM_TVF_05.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 0, "", "",
                "Selects one of the following seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_10.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 1, "", "",
                "Selects one of the seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_11.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 2, "", "",
                "Selects one of the seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_12.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 3, "", "",
                "Selects one of the seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_13.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 4, "", "",
                "Selects one of the seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_14.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 5, "", "",
                "Selects one of the seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_15.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 6, "", "",
                "Selects one of the seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_16.png", 0x0068);
            Add(1, 5, (byte)ItemIndex++, 7, "", "",
                "Selects one of the seven curves that determine how keyboard playing dy" +
                "namics (velocity) influence the cutoff frequency. Set this to “FIXED” " +
                "if you don’t want the Cutoff frequency to be affected by the keyboard " +
                "velocity.\r\n", "_PCM_TVF_17.png", 0x0068);
            Add(1, 5, (byte)ItemIndex++, 0, "", "",
                "Use this parameter when changing the cutoff frequency to be applied as a result " +
                "of changes in playing velocity.\r\n" +
                "\r\n" +
                "If you want strongly played notes to raise the cutoff frequency, set this parame" +
                "ter to positive “+” settings.\r\n" +
                "\r\n" +
                "If you want strongly played notes to lower the cutoff frequency, use negative “-" +
                "” settings.\r\n", "", 0x00e0);
            Add(1, 5, (byte)ItemIndex++, 0, "", "",
                "This allows keyboard velocity to modify the amount of Resonance.\r\n" +
                "\r\n" +
                "If you want strongly played notes to have a greater Resonance effect, set this p" +
                "arameter to positive “+” settings.\r\n" +
                "\r\n" +
                "If you want strongly played notes to have less Resonance, use negative “-” setti" +
                "ngs.\r\n", "", 0x00e0);
            Add(1, 5, (byte)ItemIndex, 0, "", "",
                "Selects one of the 7 curves that will determine how keyboard playing dynamics wi" +
                "ll affect the TVF envelope. Set this to “FIXED” if you don’t want the " +
                "TVF Envelope to be affected by the keyboard velocity.\r\n", "_PCM_TVF_10.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 1, "", "",
                "Selects one of the 7 curves that will determine how keyboard playing dynamics wi" +
                "ll affect the TVF envelope. Set this to “FIXED” if you don’t want the " +
                "TVF Envelope to be affected by the keyboard velocity.\r\n", "_PCM_TVF_11.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 2, "", "",
                "Selects one of the 7 curves that will determine how keyboard playing dynamics wi" +
                "ll affect the TVF envelope. Set this to “FIXED” if you don’t want the " +
                "TVF Envelope to be affected by the keyboard velocity.\r\n", "_PCM_TVF_12.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 3, "", "",
                "Selects one of the 7 curves that will determine how keyboard playing dynamics wi" +
                "ll affect the TVF envelope. Set this to “FIXED” if you don’t want the " +
                "TVF Envelope to be affected by the keyboard velocity.\r\n", "_PCM_TVF_13.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 4, "", "",
                "Selects one of the 7 curves that will determine how keyboard playing dynamics wi" +
                "ll affect the TVF envelope. Set this to “FIXED” if you don’t want the " +
                "TVF Envelope to be affected by the keyboard velocity.\r\n", "_PCM_TVF_14.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 5, "", "",
                "Selects one of the 7 curves that will determine how keyboard playing dynamics wi" +
                "ll affect the TVF envelope. Set this to “FIXED” if you don’t want the " +
                "TVF Envelope to be affected by the keyboard velocity.\r\n", "_PCM_TVF_15.png", 0x0068);
            Add(1, 5, (byte)ItemIndex, 6, "", "",
                "Selects one of the 7 curves that will determine how keyboard playing dynamics wi" +
                "ll affect the TVF envelope. Set this to “FIXED” if you don’t want the " +
                "TVF Envelope to be affected by the keyboard velocity.\r\n", "_PCM_TVF_16.png", 0x0068);
            Add(1, 5, (byte)ItemIndex++, 7, "", "",
                "Selects one of the 7 curves that will determine how keyboard playing dynamics wi" +
                "ll affect the TVF envelope. Set this to “FIXED” if you don’t want the " +
                "TVF Envelope to be affected by the keyboard velocity.\r\n", "_PCM_TVF_17.png", 0x0068);
            Add(1, 5, (byte)ItemIndex++, 0, "", "",
                "Specifies how keyboard playing dynamics will affect the depth of the TVF envelop" +
                "e.\r\n" +
                "\r\n" +
                "Positive “+” settings will cause the TVF envelope to have a greater effect for s" +
                "trongly played notes, and negative “-“ settings will cause the effect " +
                "to be less.\r\n", "", 0x00e0);
            Add(1, 5, (byte)ItemIndex++, 0, "", "",
                "This allows keyboard dynamics to affect the Time 1 of the TVF envelope.\r\n" +
                "\r\n" +
                "If you want Time 1 to be speeded up for strongly played notes, set this paramete" +
                "r to a positive “+” value.\r\n" +
                "\r\n" +
                "If you want it to be slowed down, set this to a negative “-” value.\r\n", "", 0x00e0);
            Add(1, 5, (byte)ItemIndex++, 0, "", "",
                "The parameter to use when you want key release speed to control the Time 4 value" +
                " of the TVF envelope.\r\n" +
                "\r\n" +
                "If you want Time 4 to be speeded up for quickly released notes, set this paramet" +
                "er to a positive “+“ value.\r\n" +
                "\r\n" +
                "If you want it to be slowed down, set this to a negative “-” value.\r\n", "", 0x00e0);
            // PCM Drum Kit TVF Env tab:
            ItemIndex = Skip;
            Add(1, 6, (byte)ItemIndex++, 0, "", "",
                "Specifies the depth of the TVF envelope.\r\n" +
                "\r\n" +
                "Higher settings will cause the TVF envelope to produce greater change.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the shape of the envelope.\r\n", "", 0x00e0);
            Add(1, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope time 1.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until cutoff frequency level 1 is " +
                "reached after note on.", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(1, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope time 2.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until cutoff frequency level 2 is " +
                "reached.", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(1, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope time 3.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until cutoff frequency level 3 is " +
                "reached.", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(1, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope time 4.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until cutoff frequency level 4 is " +
                "reached after note off.", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(1, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope level 0.\r\n" +
                "\r\n" +
                "This settings specify how the cutoff frequency will change relat" +
                "ive to the standard cutoff frequency (the cutoff frequency value speci" +
                "fied in the TVF screen).\r\n\r\n" +
                "This is the initial level.", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(1, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope level 1.\r\n" +
                "\r\n" +
                "This settings specify how the cutoff frequency will change relat" +
                "ive to the standard cutoff frequency (the cutoff frequency value speci" +
                "fied in the TVF screen).\r\n\r\n" +
                "This is the level reached after time T1 following note on.", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(1, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope level 2.\r\n" +
                "\r\n" +
                "This settings specify how the cutoff frequency will change relat" +
                "ive to the standard cutoff frequency (the cutoff frequency value speci" +
                "fied in the TVF screen).\r\n\r\n" +
                "This is the level reached after time T2.", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(1, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope level 3.\r\n" +
                "\r\n" +
                "This settings specify how the cutoff frequency will change relat" +
                "ive to the standard cutoff frequency (the cutoff frequency value speci" +
                "fied in the TVF screen).\r\n\r\n" +
                "This is the level reached after time T3.", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            Add(1, 6, (byte)ItemIndex++, 0, "", "",
                "Specify the TVF envelope level 4.\r\n" +
                "\r\n" +
                "This settings specify how the cutoff frequency will change relat" +
                "ive to the standard cutoff frequency (the cutoff frequency value speci" +
                "fied in the TVF screen).\r\n\r\n" +
                "This is the level reached after time T4 following note off.", "_PCM_CutOffFreqEnv_02.png", 0x0068);
            // PCM Drum Kit TVA tab:
            ItemIndex = Skip;
            Add(1, 7, (byte)ItemIndex++, 0, "", "",
                "Sets the volume of the drum partial. Use this parameter to adjust the volume bal" +
                "ance between drum partials.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "The volume levels of the Waves from which the drum partial is composed is set wi" +
                "th the Wave Level parameter (p. 67).\r\n", "", 0x00e0);
            Add(1, 7, (byte)ItemIndex, 0, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the drum partial to be affected by the force wi" +
                "th which you press the key, select “FIXED.”\r\n", "_PCM_TVF_10.png", 0x0068);
            Add(1, 7, (byte)ItemIndex, 1, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the drum partial to be affected by the force wi" +
                "th which you press the key, select “FIXED.”\r\n", "_PCM_TVF_11.png", 0x0068);
            Add(1, 7, (byte)ItemIndex, 2, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the drum partial to be affected by the force wi" +
                "th which you press the key, select “FIXED.”\r\n", "_PCM_TVF_12.png", 0x0068);
            Add(1, 7, (byte)ItemIndex, 3, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the drum partial to be affected by the force wi" +
                "th which you press the key, select “FIXED.”\r\n", "_PCM_TVF_13.png", 0x0068);
            Add(1, 7, (byte)ItemIndex, 4, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the drum partial to be affected by the force wi" +
                "th which you press the key, select “FIXED.”\r\n", "_PCM_TVF_14.png", 0x0068);
            Add(1, 7, (byte)ItemIndex, 5, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the drum partial to be affected by the force wi" +
                "th which you press the key, select “FIXED.”\r\n", "_PCM_TVF_15.png", 0x0068);
            Add(1, 7, (byte)ItemIndex, 6, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the drum partial to be affected by the force wi" +
                "th which you press the key, select “FIXED.”\r\n", "_PCM_TVF_16.png", 0x0068);
            Add(1, 7, (byte)ItemIndex++, 7, "", "",
                "You can select from seven curves that determine how keyboard playing strength wi" +
                "ll affect the volume.\r\n" +
                "\r\n" +
                "If you do not want the volume of the drum partial to be affected by the force wi" +
                "th which you press the key, select “FIXED.”\r\n", "_PCM_TVF_17.png", 0x0068);
            Add(1, 7, (byte)ItemIndex++, 0, "", "",
                "Set this when you want the volume of the drum partial to change depending on the" +
                " force with which you press the keys.\r\n" +
                "\r\n" +
                "Set this to a positive “+” value to have the changes in drum partial volume incr" +
                "ease the more forcefully the keys are played; to make the tone play mo" +
                "re softly as you play harder, set this to a negative “-” value.\r\n", "", 0x00e0);
            Add(1, 7, (byte)ItemIndex++, 0, "", "",
                "Sets the pan for the drum partial. “L64” is far left, “0” is center, and “63R” i" +
                "s far right.\r\n" +
                "\r\n" +
                "MEMO\r\n" +
                "Set the Pan for Waves comprising the drum partials with the Wave Pan parameter (" +
                "p. 67).\r\n", "", 0x00e0);
            Add(1, 7, (byte)ItemIndex++, 0, "", "",
                "Use this parameter when you want the stereo location to change randomly each tim" +
                "e you press a key. Higher settings will produce a greater amount of ch" +
                "ange.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "This will affect only waves whose Wave Random Pan Sw parameter (p. 67) is ON.\r\n", "", 0x00e0);
            Add(1, 7, (byte)ItemIndex++, 0, "", "",
                "This setting causes panning to be alternated between left and right each time a " +
                "key is pressed.\r\n" +
                "\r\n" +
                "Higher settings will produce a greater amount of change. “L” or “R” settings wil" +
                "l reverse the order in which the pan will alternate between left and r" +
                "ight. For example if two drum partials are set to “L” and “R” respecti" +
                "vely, the panning of the two drum partials will alternate each time th" +
                "ey are played.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "This will affect only waves whose Wave Alter Pan Sw parameter (p. 67) is ON or R" +
                "EVS.\r\n", "", 0x00e0);
            Add(1, 7, (byte)ItemIndex++, 0, "", "",
                "Corrects for the volume of the drum partial.\r\n" +
                "\r\n" +
                "This parameter is set by the key-based controller system exclusive message. Norm" +
                "ally, you should leave it set to 0.\r\n" +
                "\r\n" +
                "NOTE\r\n" +
                "If the drum partial level is set to 127, the volume will not increase beyond tha" +
                "t point.\r\n", "", 0x00e0);
            // PCM Drum Kit TVA Env tab:
            ItemIndex = Skip;
            Add(1, 8, (byte)ItemIndex++, 0, "", "",
                "This allows keyboard dynamics to affect the Time 1 of the TVA envelope.\r\n" +
                "\r\n" +
                "If you want Time 1 to be speeded up for strongly played notes, set this paramete" +
                "r to a positive “+” value.\r\n" +
                "\r\n" +
                "If you want it to be slowed down, set this to a negative “-” value.\r\n", "", 0x00e0);
            Add(1, 8, (byte)ItemIndex++, 0, "", "",
                "The parameter to use when you want key release speed to control the Time 4 value" +
                " of the TVA envelope.\r\n" +
                "\r\n" +
                "If you want Time 4 to be speeded up for quickly released notes, set this paramet" +
                "er to a positive “+” value.\r\n" +
                "\r\n" +
                "If you want it to be slowed down, set this to a negative “-” value.\r\n", "", 0x00e0);
            Add(1, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope time 1.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until volume level L1 is reached after note on.",
                "_PCM_TVALevelEnv_02.png", 0x0068);
            Add(1, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope time 2.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until volume level L2 is reached.",
                "_PCM_TVALevelEnv_02.png", 0x0068);
            Add(1, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope time 3.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until volume level L3 is reached.",
                "_PCM_TVALevelEnv_02.png", 0x0068);
            Add(1, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope time 4.\r\n" +
                "\r\n" +
                "Higher settings will lengthen the time until volume zero is reached after note off.",
                "_PCM_TVALevelEnv_02.png", 0x0068);
            Add(1, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope level 1.\r\n" +
                "\r\n" +
                "This settings specify how the volume will change between zero at note on and le" +
                "vel L1.\r\n", "_PCM_TVALevelEnv_02.png", 0x0068);
            Add(1, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope level 2.\r\n" +
                "\r\n" +
                "This settings specify how the volume will change between level L1 and level L2.\r\n",
                "_PCM_TVALevelEnv_02.png", 0x0068);
            Add(1, 8, (byte)ItemIndex++, 0, "", "",
                "Specify the TVA envelope level 3.\r\n" +
                "\r\n" +
                "This settings specify how the volume will change between level L2 and level L3.\r\n",
                "_PCM_TVALevelEnv_02.png", 0x0068);
            // PCM Drum Kit Output tab:
            ItemIndex = Skip;
            Add(1, 9, (byte)ItemIndex++, 0, "", "",
                "Specifies how the sound of selected partial will be output.\r\n", "", 0x00e0);
            Add(1, 9, (byte)ItemIndex++, 0, "", "",
                "Specifies the signal level of selected partial.\r\n", "", 0x00e0);
            Add(1, 9, (byte)ItemIndex++, 0, "", "",
                "Specifies the level of the signal sent to the chorus for selected partial.\r\n", "", 0x00e0);
            Add(1, 9, (byte)ItemIndex++, 0, "", "",
                "Specifies the level of the signal sent to the reverb for selected partial.\r\n", "", 0x00e0);
            // PCM Drum Kit Comp tab:
            ItemIndex = Skip;
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Compressor 1 on/off.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Compressor 2 on/off.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Compressor 3 on/off.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Compressor 4 on/off.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Compressor 5 on/off.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Compressor 6 on/off.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Time from when the input 1 exceeds the threshold until compression 1 begins.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Time from when the input 2 exceeds the threshold until compression 2 begins.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Time from when the input 3 exceeds the threshold until compression 3 begins.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Time from when the input 4 exceeds the threshold until compression 4 begins.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Time from when the input 5 exceeds the threshold until compression 5 begins.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Time from when the input 6 exceeds the threshold until compression 6 begins.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Time from when the input 1 falls below the threshold 1 until compression 1 is turned o" +
                "ff.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Time from when the input 2 falls below the threshold 2 until compression 2 is turned o" +
                "ff.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Time from when the input 3 falls below the threshold 3 until compression 3 is turned o" +
                "ff.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Time from when the input 4 falls below the threshold 4 until compression 4 is turned o" +
                "ff.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Time from when the input 5 falls below the threshold 5 until compression 5 is turned o" +
                "ff.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Time from when the input 6 falls below the threshold 6 until compression 6 is turned o" +
                "ff.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Level above which compression 1 is applied.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Level above which compression 2 is applied.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Level above which compression 3 is applied.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Level above which compression 4 is applied.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Level above which compression 5 is applied.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Level above which compression 6 is applied.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Compressor 1 compression ratio.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Compressor 2 compression ratio.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Compressor 3 compression ratio.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Compressor 4 compression ratio.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Compressor 5 compression ratio.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Compressor 6 compression ratio.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Level of compressor 1 output sound.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Level of compressor 2 output sound.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Level of compressor 3 output sound.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Level of compressor 4 output sound.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Level of compressor 5 output sound.\r\n", "", 0x00e0);
            Add(1, 10, (byte)ItemIndex++, 0, "", "",
                "Level of compressor 6 output sound.\r\n", "", 0x00e0);
            // PCM Drum Kit Eq tab:
            ItemIndex = Skip;
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Equalizer 1 on/off.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Equalizer 2 on/off.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Equalizer 3 on/off.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Equalizer 4 on/off.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Equalizer 5 on/off.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Equalizer 6 on/off.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the low range for equalizer 1.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the low range for equalizer 2.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the low range for equalizer 3.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the low range for equalizer 4.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the low range for equalizer 5.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the low range for equalizer 6.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the middle range for equalizer 1.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the middle range for equalizer 2.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the middle range for equalizer 3.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the middle range for equalizer 4.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the middle range for equalizer 5.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the middle range for equalizer 6.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Width of the middle range for equalizer 1.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Width of the middle range for equalizer 2.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Width of the middle range for equalizer 3.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Width of the middle range for equalizer 4.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Width of the middle range for equalizer 5.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Width of the middle range for equalizer 6.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the high range for equalizer 1.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the high range for equalizer 2.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the high range for equalizer 3.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the high range for equalizer 4.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the high range for equalizer 5.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Frequency of the high range for equalizer 6.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the low range for equalizer 1.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the low range for equalizer 2.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the low range for equalizer 3.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the low range for equalizer 4.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the low range for equalizer 5.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the low range for equalizer 6.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the middle range for equalizer 1.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the middle range for equalizer 2.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the middle range for equalizer 3.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the middle range for equalizer 4.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the middle range for equalizer 5.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the middle range for equalizer 6.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the high range for equalizer 1.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the high range for equalizer 2.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the high range for equalizer 3.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the high range for equalizer 4.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the high range for equalizer 5.\r\n", "", 0x00e0);
            Add(1, 11, (byte)ItemIndex++, 0, "", "",
                "Gain of the high range for equalizer 6.\r\n", "", 0x00e0);
            // PCM Drum Kit MFX control tab:
            ItemIndex = Skip;
            Add(1, 13, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(1, 13, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(1, 13, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(1, 13, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(1, 13, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(1, 13, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(1, 13, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(1, 13, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(1, 13, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(1, 13, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(1, 13, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(1, 13, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            // PCM Drum Kit Save tab:
            ItemIndex = Skip;
            Add(1, 14, (byte)ItemIndex++, 0, "", "",
                "Type in a name for the tone.\r\n" +
                "\r\n" +
                "Names can be up to 12 characters and will be truncated if you enter more than 12.\r\n" +
                "\r\n" +
                "This name will be displayed on the INTEGRA-7.\r\n", "", 0x00e0);
            Add(1, 14, (byte)ItemIndex++, 0, "", "",
                "Select a slot to save the tone in.\r\n", "", 0x00e0);
            Add(1, 14, (byte)ItemIndex++, 0, "", "",
                "Saves the tone in the selected slot.\r\n", "", 0x00e0);
            Add(1, 14, (byte)ItemIndex++, 0, "", "",
                "Deletes the tone in the selected slot.\r\n\r\n" +
                "This has no effect on the tone you are editing, you can still save it in any slot.", "", 0x00e0);
            // SuperNATURAL Acoustic tone common tab:
            ItemIndex = Skip;
            Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic tone", "", "Pressing and holding the volume control on the" +
                " INTEGRA-7 will make it play a phrase. You can select what phrase will be played here. " +
                "Default is a phrase specifically created for the current tone.\r\n\r\nYou can use the \'Play\'" +
                " button below to play/stop the selected phrase.");
            Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic tone", "", "Pressing and holding the volume control on the" +
                " INTEGRA-7 will make it play a phrase. Here you may transpose the phrase to another octave." +
                "\r\n\r\nYou can use the \'Play\' button below to play/stop the selected phrase.");
            Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the volume of the tone.", "", 0x10d0);
                        Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "", 
				"Specifies whether the patch will play polyphonically (POLY) or monophonically (MONO).\r\n" +
                "\r\n" +
                "MONO: Only the last-played note will sound.\r\n" +
                "POLY: Two or more notes can be played simultaneously.\r\n" +
                "\r\n" +
				"* This parameter does not apply when INT 029: TW Organ is selected." , "", 0x10d0);
            Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the pitch of the patch’s sound up or down in units of an octave (+/-3 octaves).", "", 0x10d0);
            Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the cutoff frequency Offset for the instrument assigned to a tone.\r\n\r\n" +
                "* This parameter does not apply when any of INT 001: Concert Grand, INT 009: Honky-tonk, or INT " +
                "029: TW Organ \r\nis selected.", "", 0x10d0);
            Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the Resonance Offset for the instrument assigned to a tone.\r\n\r\n" +
                "* This parameter does not apply when any of INT 001: Concert Grand, INT 009: Honky-tonk, or INT" +
                "029: TW Organ \r\nis selected.", "", 0x10d0);
            Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the TVA Envelope Attack Time Offset for the instrument assigned to a tone.\r\n" +
                "\r\n" +
                "* This parameter does not apply when any of INT 001:\r\n" +
                "Concert Grand, INT 009: Honky-tonk, or INT 029: TW Organ\r\n" +
                "is selected.", "", 0x10d0);
            Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the TVA Envelope Release Time Offset for the instrument assigned to a tone.\r\n" +
                "\r\n" +
                "* This parameter does not apply when any of INT 001:\r\n" +
                " Concert Grand, INT 009: Honky-tonk, or INT 029: TW Organ\r\n" +
                "is selected.", "", 0x10d0);
            Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "When portamento is used, this specifies the time over which the pitch will change. Higher settings will cause the pitch change to the next note to take more time.\r\n" +
                "\r\n" +
                "* This parameter does not apply when any of INT 001:\r\n" +
                " INT 029: TW Organ is selected.", "", 0x10d0);
            Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjust the vibrato speed (the rate at which the pitch is modulated). The pitch will be modulated more rapidly for higher settings, and more slowly with lower settings.\r\n" +
                "\r\n" +
                "* This effect does not apply to instruments of the Organ, Bell/Mallet, or Percussion categories.", "", 0x10d0);
            Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "This adjusts the depth of the vibrato effect (the depth at which the pitch is modulated). The pitch" +
                "will be modulated more greatly for higher settings, and less with lower settings.\r\n\r\n" +
                "* This effect does not apply to instruments of the Organ, Bell/Mallet, or Percussion categories.", "", 0x10d0);
            Add(2, 0, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "This adjusts the time delay until the vibrato (pitch modulation) effect begins. Higher settings will" +
                "produce a longer delay time before vibrato begins, while lower settings produce a shorter time.\r\n\r\n" +
                "* This effect does not apply to instruments of the Organ, Bell/Mallet, or Percussion categories.", "", 0x10d0);
            // SuperNATURAL Acoustic tone instrument tab:
            ItemIndex = Skip;
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Select the instrument bank of the tone.\r\n" +
                "\r\n" +
                "INT: Internal sound bank\r\n" +
                "ExSN1–ExSN5: Expanded sound bank\r\n" +
                "\r\n" +
                "(Remember to load the ExSN# bank into the Integra-7 memory!)\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Select the instrument number of the tone.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "", 
				"When the keys are pressed on an acoustic piano, the strings for keys that are already pressed also vibrate sympathetically. The function used to reproduce is called “String Resonance.”\r\n" +
				"\r\n" +
				"Increasing the value will increase the amount of effect.\r\n" , "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "This adjusts resonances such as the key-off sound of an acoustic piano (subtle sounds that are heard when you release a key).\r\n" +
                "\r\n" +
                "Higher values will increase the volume of the resonances.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "This adjusts the sound of the hammer striking the string of an acoustic piano.\r\n" +
                "\r\n" +
                "Higher values will increase the sound of the hammer striking the string.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "The higher the value set, the wider the sound is spread out.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "This changes the Tone’s subtle nuances by altering the phase of the left and right sounds.\r\n" +
                "\r\n" +
                "This effect is difficult to hear when headphones are used.\r\n" +
                "\r\n" +
                "* This has no effect for 008:Concert Mono.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Higher values produce a harder sound; lower values produce a more mellow sound.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the amount of hum noise and key-off noise. Higher settings will raise the volume.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the amount of automatically produced crescendo. The effect is most noticeable when you play softly.\r\n" +
                "\r\n" +
                "* This applies only for ExSN5 004: Mariachi Tp.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the speed of the tremolo effect.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the deviation in the timing of sound production by the strings when strumming with Strum Mode turned on.\r\n" +
                "\r\n" +
                "Higher values produce a greater time deviation.\r\n" +
                "\r\n" +
                "The effect will be more significant for lower velocities.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "If Strum Mode is turned on, strumming will be produced when you play multiple keys simultaneously. This also reproduces the difference in time at which each string of a guitar is sounded. The guitar’s up strokes and down strokes will alternately be produced when chords are played in succession.\r\n" +
                "\r\n" +
                "It is effective to play while holding down the Hold pedal.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "If this is on, strongly played notes will have a picking harmonic effect added to them.\r\n" +
                "\r\n" +
                "* This has no effect on the INT 037: Jazz Guitar.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the pitch of the sympathetic strings.\r\n" +
                "\r\n" +
                "* This is valid only for ExSN4 003: 12th Steel Gtr.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the distinctive nuance (growl) that occurs when a brass instrument is blown.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjust the level of Harmonic Bar 16'.\r\n" +
                "\r\n" +
                "A different harmonic component is assigned to each footage. The sound of the organ is created by mixing these components.\r\n" +
                "\r\n" +
                "The 8’ footage is the core of the sound, the basic pitch around which the sound is created.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjust the level of Harmonic Bar 5-1/3'.\r\n" +
                "\r\n" +
                "A different harmonic component is assigned to each footage. The sound of the organ is created by mixing these components.\r\n" +
                "\r\n" +
                "The 8’ footage is the core of the sound, the basic pitch around which the sound is created.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjust the level of Harmonic Bar 8'\r\n" +
                ".\r\n" +
                "A different harmonic component is assigned to each footage. The sound of the organ is created by mixing these components.\r\n" +
                "\r\n" +
                "This footage is the core of the sound, the basic pitch around which the sound is created.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjust the level of Harmonic Bar 4'.\r\n" +
                "\r\n" +
                "A different harmonic component is assigned to each footage. The sound of the organ is created by mixing these components.\r\n" +
                "\r\n" +
                "The 8' footage is the core of the sound, the basic pitch around which the sound is created.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjust the level of Harmonic Bar 2-2/3'.\r\n" +
                "\r\n" +
                "A different harmonic component is assigned to each footage. The sound of the organ is created by mixing these components.\r\n" +
                "\r\n" +
                "The 8' footage is the core of the sound, the basic pitch around which the sound is created.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjust the level of Harmonic Bar 2'.\r\n" +
                "\r\n" +
                "A different harmonic component is assigned to each footage. The sound of the organ is created by mixing these components.\r\n" +
                "\r\n" +
                "The 8' footage is the core of the sound, the basic pitch around which the sound is created.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjust the level of Harmonic Bar 1-3/5'.\r\n" +
                "\r\n" +
                "A different harmonic component is assigned to each footage. The sound of the organ is created by mixing these components.\r\n" +
                "\r\n" +
                "The 8' footage is the core of the sound, the basic pitch around which the sound is created.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjust the level of Harmonic Bar 1-1/3'.\r\n" +
                "\r\n" +
                "A different harmonic component is assigned to each footage. The sound of the organ is created by mixing these components.\r\n" +
                "\r\n" +
                "The 8' footage is the core of the sound, the basic pitch around which the sound is created.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjust the level of Harmonic Bar 1'.\r\n" +
                "\r\n" +
                "A different harmonic component is assigned to each footage. The sound of the organ is created by mixing these components.\r\n" +
                "\r\n" +
                "The 8' footage is the core of the sound, the basic pitch around which the sound is created.\r\n" +
                "\r\n" +
                "* Harmonic Bar 1’ is unavailable if Percussion Switch is on.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Noise Level at which the signal of tone wheels unrelated to the pressed keys is mixed into the input.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "If this is on, a crisp attack will be added to the beginning of the notes.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "NORM: The percussion sound will be at the normal volume, and the sound of the harmonic bars will be reduced.\r\n" +
                "\r\n" +
                "SOFT: The percussion sound will be reduced, and the harmonic bars will be at the normal volume.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Volume of the percussion sound when Percussion Soft is set to SOFT.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Volume of the percussion sound when Percussion Soft is set to NORM.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "FAST: The percussion sound will disappear immediately, producing a sharp attack.\r\n" +
                "\r\n" +
                "SLOW: The percussion sound will disappear slowly, producing a more gentle attack.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Decay time of the percussion sound when Percussion Slow is set to SLOW.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Decay time of the percussion sound when Percussion Slow is set to FAST.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "2ND: The percussion sound will be the same pitch as the 4’ harmonic bar.\r\n" +
                "\r\n" +
                "3RD: The percussion sound will be the same pitch as the 2-2/3’ harmonic bar.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Normally, the percussion sound will be added only to the first note of successive notes played legato.\r\n" +
                "\r\n" +
                "This reproduces the characteristics of the analog circuitry that produced the percussion sound in tone wheel" +
                " organs, which caused the percussion sound to be softer when keys were pressed in quick succession.\r\n" +
                "\r\n" +
                "This specifies the characteristics of this analog circuit.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "The volume of the organ will be reduced if Percussion Soft is set to NORM.\r\n" +
                "\r\n" +
                "This specifies how much the volume will be reduced.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Level of the key-click when a key is pressed.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Level of the key-click when a key is released.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the hardness of the mallet. Higher settings produce the sound of a harder mallet.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the sympathetic resonance. Higher settings will increase the sympathetic resonance.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the speed of the roll effect.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "By turning Glissando mode (CC19) on, you can cause only the notes included in a specific scale to be sounded. This lets you easily produce an idiomatic harp glissando simply by playing a glissando on the white keys.\r\n" +
                "\r\n" +
                "* It is effective to play this while holding down the HOLD pedal.\r\n" +
                "\r\n" +
                "* By using CC18 you can simulate the technique of using your hand to stop the vibration of the strings.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Specifies the scale used when Glissando Mode is on.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Specifies the key of the scale produced when you play a glissando with Glissando Mode turned on.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the amount of pitch change that occurs at the attack when you play strongly.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "If this is on, keys of note number 42 and lower will sound vocal interjections or other sound effects.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the volume of the tambura sound effect sounded by CC80.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the pitch of the tambura sound effect sounded by CC80.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Specifies how notes are sounded when Hold (CC64) is on.\r\n" +
                "\r\n" +
                "If Hold Legato Mode is on, notes that were being held will go silent when you play a key.\r\n" +
                "\r\n" +
                "For example if you play and release C major with Hold (CC64) on, the C major notes will be held. When you then play E major, the C major notes will go silent, and the E major notes will be heard.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the volume of the drone sound effect sounded by CC80.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Adjusts the pitch of the drone sound effect sounded by CC80.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Specifies whether portamento or glissando will be applied when the portamento switch is on.\r\n", "", 0x10d0);
            Add(2, 1, (byte)ItemIndex++, 0, "SuperNATURAL Acoustic Tone", "",
                "Some instruments come in different variations.\r\n" +
                "\r\n" +
                "Select variation here.\r\n", "", 0x10d1);
            // SuperNATURAL Acoustic Tone MFX control tab:
            ItemIndex = Skip;
            Add(2, 3, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(2, 3, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(2, 3, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(2, 3, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(2, 3, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(2, 3, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(2, 3, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(2, 3, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(2, 3, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(2, 3, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(2, 3, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(2, 3, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            // SuperNATURAL Acoustic Tone Save tab:
            ItemIndex = Skip;
            Add(2, 4, (byte)ItemIndex++, 0, "", "",
                "Type in a name for the tone.\r\n" +
                "\r\n" +
                "Names can be up to 12 characters and will be truncated if you enter more than 12.\r\n" +
                "\r\n" +
                "This name will be displayed on the INTEGRA-7.\r\n", "", 0x00e0);
            Add(2, 4, (byte)ItemIndex++, 0, "", "",
                "Select a slot to save the tone in.\r\n", "", 0x00e0);
            Add(2, 4, (byte)ItemIndex++, 0, "", "",
                "Saves the tone in the selected slot.\r\n", "", 0x00e0);
            Add(2, 4, (byte)ItemIndex++, 0, "", "",
                "Deletes the tone in the selected slot.\r\n\r\n" +
                "This has no effect on the tone you are editing, you can still save it in any slot.", "", 0x00e0);
            // SuperNATURAL Synth Tone Common tab
            ItemIndex = Skip;
            Add(3, 0, (byte)ItemIndex++, 0, "SuperNATURAL Synth Tone", "", "Pressing and holding the volume control on the" +
                " INTEGRA-7 will make it play a phrase. You can select what phrase will be played here. " +
                "Default is a phrase specifically created for the current tone.\r\n\r\nYou can use the \'Play\'" +
                " button below to play/stop the selected phrase.");
            Add(3, 0, (byte)ItemIndex++, 0, "SuperNATURAL Synth Tone", "", "Pressing and holding the volume control on the" +
                " INTEGRA-7 will make it play a phrase. Here you may transpose the phrase to another octave." +
                "\r\n\r\nYou can use the \'Play\' button below to play/stop the selected phrase.");
            Add(3, 0, (byte)ItemIndex++, 0, "SuperNATURAL Synth Tone", "",
                "Adjusts the overall volume of the tone.\r\n", "", 0x10d0);
            Add(3, 0, (byte)ItemIndex++, 0,
                "Turns ring modulator on / off.\r\n" +
                "\r\n" +
                "By multiplying partial 1’s OSC and partial 2’s OSC, this creates a complex, metallic-sounding waveform like" +
                " that of a bell.\r\n" +
                "The partial 1’s OSC waveform will change as shown in the illustration, and partial 2’s OSC will be output" +
                " with its original waveform.\r\n" +
                "Setting the partial 1 OSC and the partial 2 OSC to different pitches will make the ring modulator effect more" +
                " apparent.\r\n", "_MFX_Common_01.png",
                "If Ring Switch is turned on, the OSC Pulse Width Mod Depth, OSC Pulse Width, and SUPER SAW Detune of partial 1" +
                " and partial 2 cannot be used.\r\n" +
                "\r\n" +
                "In addition, if an asymmetrical square wave is selected as the OSC waveform, the OSC variation will be ignored," +
                " and there will be a slight difference in sound compared to the originally selected waveform.\r\n", "", 0x5450);
            Add(3, 0, (byte)ItemIndex++, 0, "", "",
                "Partial 1 will be modulated by the pitch of partial 2. Higher values produce a greater effect.\r\n" +
                "\r\n" +
                "This has no effect if the partial 1 waveform is PW-SQR or SP-SAW.\r\n", "", 0x00e0);
            Add(3, 0, (byte)ItemIndex++, 0, "", "",
                "Use this to apply “1/f fluctuation,” a type of randomness or instability that is" +
                " present in many natural systems (such as a babbling brook or whisperi" +
                "ng breeze) and is perceived as pleasant by many people.\r\n" +
                "\r\n" +
                "By applying “1/f fluctuation” you can create the natural-sounding instability th" +
                "at is characteristic of an analog synthesizer.\r\n", "", 0x00e0);
            Add(3, 0, (byte)ItemIndex++, 0, "", "",
                "This layers a single sound.\r\n" +
                "\r\n" +
                "If the Unison Switch is on, the number of notes layered on one key will change a" +
                "ccording to the number of keys you play.\r\n", "", 0x00e0);
            Add(3, 0, (byte)ItemIndex++, 0, "Number of notes assigned to each key when the Unison Switch is on.\r\n" +
                "\r\n" +
                "Note that the number of notes sound is limited by the Unison Size. The sum of so" +
                "unds for all payed keys do not exceed the Unison Size.\r\n" +
                "\r\n" +
                "Example: If Unison Size is 8\r\n", "_MFX_Common_02.png",
                "Example: If Unison Size is 6\r\n", "_MFX_Common_03.png", 0x7323);
            Add(3, 0, (byte)ItemIndex++, 0, "", "",
                "Specifies whether notes will sound polyphonically (POLY) or monophonically (MONO" +
                ").\r\n", "", 0x00e0);
            Add(3, 0, (byte)ItemIndex++, 0, "", "",
                "This is valid only if the Mono/Poly parameter is set to “MONO.” If this is on, p" +
                "ressing a key while the previous key remains held down will cause the " +
                "pitch to change to that of the newly pressed key while maintaining the" +
                " state in which the previous note was being sounded.\r\n" +
                "\r\n" +
                "This produces an effect similar to hammering-on or pulling-off when playing a gu" +
                "itar.\r\n", "", 0x00e0);
            Add(3, 0, (byte)ItemIndex++, 0, "", "",
                "Specifies whether the portamento effect will be applied (ON) or not applied (OFF" +
                ").\r\n", "", 0x00e0);
            Add(3, 0, (byte)ItemIndex++, 0, "", "",
                "Specifies the time taken for the pitch to change when playing portamento.\r\n" +
                "\r\n" +
                "Higher values lengthen the time over which the pitch will change to the next not" +
                "e.\r\n", "", 0x00e0);
            Add(3, 0, (byte)ItemIndex++, 0, "", "",
                "NORMAL: Portamento will always be applied.\r\n" +
                "\r\n" +
                "LEGATO: Portamento will be applied only when you play legato (i.e., when you pre" +
                "ss the next key before releasing the previous key).\r\n", "", 0x00e0);
            // SuperNATURAL Synth Tone Osc tab
            ItemIndex = Skip;
            Add(3, 1, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 1 to be heard.\r\n", "", 0x00e0);
            Add(3, 1, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 2 to be heard.\r\n", "", 0x00e0);
            Add(3, 1, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 3 to be heard.\r\n", "", 0x00e0);
            Add(3, 1, (byte)ItemIndex, 0, "", "",
                "This waveform contains a sine wave fundamental plus a fixed proportion of sine w" +
                "ave harmonics at\r\n" +
                "all integer multiples of that fundamental.\r\n", "_SNS_Osc_01.png", 0x0095);
            Add(3, 1, (byte)ItemIndex, 1, "", "",
                "This waveform contains a sine wave fundamental plus a fixed proportion of sine w" +
                "ave harmonics at\r\n" +
                "odd-numbered multiples of that fundamental.\r\n", "_SNS_Osc_02.png", 0x0095);
            Add(3, 1, (byte)ItemIndex, 2, "", "",
                "The overtone structure of this waveform will vary significantly depending on the" +
                " width of the rate between the upper portion and the lower form of the" +
                " waveform (Pulse Width).\r\n", "_SNS_Osc_03.png", 0x0095);
            Add(3, 1, (byte)ItemIndex, 3, "", "",
                "This waveform contains a sine wave fundamental plus a fixed proportion of sine w" +
                "ave harmonics at\r\n" +
                "even-numbered multiples of that fundamental.\r\n", "_SNS_Osc_04.png", 0x0095);
            Add(3, 1, (byte)ItemIndex, 4, "", "",
                "This is a sine wave. This is a waveform that produces just a single frequency; i" +
                "t is the basis of all sound.\r\n", "_SNS_Osc_05.png", 0x0095);
            Add(3, 1, (byte)ItemIndex, 5, "", "",
                "This waveform contains all frequencies. It is suitable for percussion instrument" +
                " sounds or sound effects.\r\n", "_SNS_Osc_06.png", 0x0095);
            Add(3, 1, (byte)ItemIndex, 6, "", "",
                "This produces a tone similar to seven sawtooth waves heard simultaneously. Pitch" +
                "-shifted sounds are added to the center sound. It is suitable for stri" +
                "ngs sounds, and for creating thick sounds.\r\n", "", 0x00e0);
            Add(3, 1, (byte)ItemIndex++, 7, "", "",
                "This is a PCM waveform.\r\n" +
                "\r\n" +
                "Select the actual waveform under \"Wave Number\" below.\r\n", "", 0x00e0);
            Add(3, 1, (byte)ItemIndex++, 0, "", "",
                "You can select variations of the currently selected WAVE.\r\n" +
                "\r\n" +
                "* This has no effect for SP-SAW or PCM.\r\n", "", 0x00e0);
            Add(3, 1, (byte)ItemIndex++, 0, "", "",
                "Selects the PCM waveform.\r\n" +
                "\r\n" +
                "* This is valid only if PCM is selected for OSC Wave.\r\n", "", 0x00e0);
            Add(3, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies the gain (amplitude) of the waveform.\r\n" +
                "\r\n" +
                "The value will change in 6 dB (decibel) steps. Each 6 dB increase doubles the ga" +
                "in.\r\n" +
                "\r\n" +
                "* This is valid only if PCM is selected for OSC Wave.\r\n", "", 0x00e0);
            Add(3, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies the amount (depth) of LFO applied to PW (Pulse Width).\r\n" +
                "\r\n" +
                "If the OSC Wave has selected (PW-SQR), you can use this slider to specify the am" +
                "ount of LFO modulation applied to PW (pulse width).\r\n" +
                "\r\n" +
                "* If the Ring Switch is on, this has no effect on partials 1 and 2.\r\n", "", 0x00e0);
            Add(3, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies the pulse width.\r\n" +
                "\r\n" +
                "If the OSC Wave has selected (PW-SQR), you can use this slider to specify the wi" +
                "dth of the upper portion of the square wave (the pulse width) as a per" +
                "centage of the entire cycle.\r\n" +
                "\r\n" +
                "Decreasing the value will decrease the width, approaching a square wave (pulse w" +
                "idth = 50%).\r\n" +
                "\r\n" +
                "Increasing the value will increase the width, producing a distinctive sound.\r\n" +
                "\r\n" +
                "* If the Ring Switch is on, this has no effect on partials 1 and 2.\r\n", "", 0x00e0);
            Add(3, 1, (byte)ItemIndex++, 0, "", "",
                "Shifts the range of change. Normally, you can leave this at 127.\r\n" +
                "\r\n" +
                "* If the Ring Switch is on, this has no effect on partials 1 and 2.\r\n", "", 0x00e0);
            Add(3, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies the amount of pitch difference between the seven sawtooth waves layere" +
                "d within a single oscillator.\r\n" +
                "\r\n" +
                "* Higher values will increase the pitch difference. (OSC Detune applies an equal" +
                " amount of pitch difference between each of the seven sawtooth waves.)" +
                "\r\n" +
                "\r\n" +
                "* If the Ring Switch is on, this has no effect on partials 1 and 2.\r\n" +
                "\r\n" +
                "* This is valid only if SP-SAW is selected for OSC Wave.\r\n", "", 0x00e0);
            // SuperNATURAL Synth Tone Pitch tab
            ItemIndex = Skip;
            Add(3, 2, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 1 to be heard.\r\n", "", 0x00e0);
            Add(3, 2, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 2 to be heard.\r\n", "", 0x00e0);
            Add(3, 2, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 3 to be heard.\r\n", "", 0x00e0);
            Add(3, 2, (byte)ItemIndex++, 0, "", "",
                "Adjusts the pitch in semitone steps.\r\n", "", 0x00e0);
            Add(3, 2, (byte)ItemIndex++, 0, "", "",
                "Adjusts the pitch in steps of one cent.\r\n", "", 0x00e0);
            Add(3, 2, (byte)ItemIndex++, 0, "", "",
                "Specifies the attack time of the pitch envelope.\r\n" +
                "\r\n" +
                "This specifies the time from the moment you press the key until the pitch reache" +
                "s its highest (or lowest) point.\r\n", "", 0x00e0);
            Add(3, 2, (byte)ItemIndex++, 0, "", "",
                "Specifies the decay time of the pitch envelope.\r\n" +
                "\r\n" +
                "This specifies the time from the moment the pitch reaches its highest (or lowest" +
                ") point until it returns to the pitch of the key you pressed.\r\n", "", 0x00e0);
            Add(3, 2, (byte)ItemIndex++, 0, "", "",
                "This specifies how much the pitch envelope will affect the pitch.\r\n", "", 0x00e0);
            Add(3, 2, (byte)ItemIndex++, 0, "", "",
                "Specifies the octave of the tone.\r\n", "", 0x00e0);
            Add(3, 2, (byte)ItemIndex++, 0, "", "",
                "Specifies the amount of pitch change that occurs when the pitch bend/modulation " +
                "lever is moved all the way to the right.\r\n", "", 0x00e0);
            Add(3, 2, (byte)ItemIndex++, 0, "", "",
                "Specifies the amount of pitch change that occurs when the pitch bend/modulation " +
                "lever is moved all the way to the left.\r\n", "", 0x00e0);
            // SuperNATURAL Synth Tone Filter tab
            ItemIndex = Skip;
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 1 to be heard.\r\n", "", 0x00e0);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 2 to be heard.\r\n", "", 0x00e0);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 3 to be heard.\r\n", "", 0x00e0);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "Selects the type of filter.\r\n", "", 0x00e0);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "This button selects the slope (steepness) of the filter.\r\n", "", 0x00e0);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "Specifies the cutoff frequency.\r\n", "_SNS_Filter_00.png", 0x0059);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "Here’s how you can make the filter cutoff frequency to vary according to the key" +
                " you play.\r\n", "_SNS_Filter_01.png", 0x0059);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "Here’s how you can make the filter envelope depth vary according to the strength" +
                " with which you play the key.\r\n", "", 0x00e0);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "Resonance emphasizes the sound in the region of the filter cutoff frequency.\r\n", "", 0x00e0);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "This specifies the time from the moment you press the key until the cutoff frequ" +
                "ency reaches its highest (or lowest) point.\r\n", "_SNS_Filter_02.png", 0x0059);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "This specifies the time from when the cutoff frequency reaches its highest (or l" +
                "owest) point, until it decays to the sustain level.\r\n", "_SNS_Filter_02.png", 0x0059);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "This specifies the cutoff frequency that will be maintained from when the decay " +
                "time has elapsed until you release the key.\r\n", "_SNS_Filter_02.png", 0x0059);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "This specifies the time from when you release the key until the cutoff frequency" +
                " reaches its minimum value.\r\n", "_SNS_Filter_02.png", 0x0059);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "This specifies the direction and depth to which the cutoff frequency will change" +
                ".\r\n", "_SNS_Filter_02.png", 0x0059);
            Add(3, 3, (byte)ItemIndex++, 0, "", "",
                "Specifies the cutoff frequency of an independent -6 dB high-pass filter.\r\n", "", 0x00e0);
            // SuperNATURAL Synth Tone Amp tab
            ItemIndex = Skip;
            Add(3, 4, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 1 to be heard.\r\n", "", 0x00e0);
            Add(3, 4, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 2 to be heard.\r\n", "", 0x00e0);
            Add(3, 4, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 3 to be heard.\r\n", "", 0x00e0);
            Add(3, 4, (byte)ItemIndex++, 0, "", "",
                "Partial volume.\r\n", "", 0x00e0);
            Add(3, 4, (byte)ItemIndex++, 0, "", "",
                "Here’s how you can make the volume vary according to the strength with which you" +
                " play the keyboard.\r\n", "", 0x00e0);
            Add(3, 4, (byte)ItemIndex++, 0, "", "",
                "Here’s how to change the stereo position of the partial.\r\n", "", 0x00e0);
            Add(3, 4, (byte)ItemIndex++, 0, "", "",
                "Specify this if you want to vary the volume according to the position of the key" +
                " that you play.\r\n" +
                "\r\n" +
                "With the C4 key (middle C) as the base volume, “+” values will make the volume i" +
                "ncrease as you play above C4; “-” values will make the volume decrease" +
                ". Higher values will produce greater change.\r\n", "", 0x00e0);
            Add(3, 4, (byte)ItemIndex++, 0, "", "",
                "Specifies the attack time of the amp envelope.\r\n" +
                "\r\n" +
                "This specifies the time from the moment you press the key until the maximum volu" +
                "me is reached.\r\n", "_SNS_Filter_02.png", 0x0059);
            Add(3, 4, (byte)ItemIndex++, 0, "", "",
                "Specifies the decay time of the amp envelope.\r\n" +
                "\r\n" +
                "This specifies the time from when the maximum volume is reached, until it decays" +
                " to the sustain level.\r\n", "_SNS_Filter_02.png", 0x0059);
            Add(3, 4, (byte)ItemIndex++, 0, "", "",
                "Specifies the sustain level of the amp envelope.\r\n" +
                "\r\n" +
                "This specifies the volume level that will be maintained from when the attack and" +
                " decay times have elapsed until you release the key.\r\n", "_SNS_Filter_02.png", 0x0059);
            Add(3, 4, (byte)ItemIndex++, 0, "", "",
                "Specifies the release time of the amp envelope.\r\n" +
                "\r\n" +
                "This specifies the time from when you release the key until the volume reaches i" +
                "ts minimum value.\r\n", "_SNS_Filter_02.png", 0x0059);
            // SuperNATURAL Synth Tone LFO tab
            ItemIndex = Skip;
            Add(3, 5, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 1 to be heard.\r\n", "", 0x00e0);
            Add(3, 5, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 2 to be heard.\r\n", "", 0x00e0);
            Add(3, 5, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 3 to be heard.\r\n", "", 0x00e0);
            Add(3, 5, (byte)ItemIndex, 0, "", "",
                "Selects the LFO waveform.\r\n\r\n" +
                "Triangle wave.\r\n", "_SNS_Osc_04.png", 0x0059);
            Add(3, 5, (byte)ItemIndex, 1, "", "",
                "Selects the LFO waveform.\r\n\r\n" +
                "Sine wave.\r\n", "_SNS_Osc_05.png", 0x0059);
            Add(3, 5, (byte)ItemIndex, 2, "", "",
                "Selects the LFO waveform.\r\n\r\n" +
                "Sawtooth wave.\r\n", "_SNS_Osc_01.png", 0x0059);
            Add(3, 5, (byte)ItemIndex, 3, "", "",
                "Selects the LFO waveform.\r\n\r\n" +
                "Square wave.\r\n", "_SNS_Osc_02.png", 0x0059);
            Add(3, 5, (byte)ItemIndex, 4, "", "",
                "Selects the LFO waveform.\r\n\r\n" +
                "Sample and Hold (The LFO value will change once each cycle.)\r\n", "_SNS_Osc_07.png", 0x0059);
            Add(3, 5, (byte)ItemIndex++, 4, "", "",
                "Selects the LFO waveform.\r\n\r\n" +
                "Random wave\r\n", "_SNS_Osc_12.png", 0x0059);
            Add(3, 5, (byte)ItemIndex++, 0, "", "",
                "Specifies the LFO rate when Modulation LFO Tempo Sync Switch is OFF.\r\n", "", 0x00e0);
            Add(3, 5, (byte)ItemIndex++, 0, "", "",
                "If this is ON, the LFO rate can be specified as a note value relative to the tem" +
                "po.\r\n", "", 0x00e0);
            Add(3, 5, (byte)ItemIndex++, 0, "", "",
                "Specifies the LFO rate when Modulation LFO Tempo Sync Switch is ON.\r\n", "", 0x00e0);
            Add(3, 5, (byte)ItemIndex++, 0, "", "",
                "This specifies the time from when the partial sounds until the LFO reaches its m" +
                "aximum amplitude.\r\n", "_SNS_Osc_08.png", 0x0059);
            Add(3, 5, (byte)ItemIndex++, 0, "", "",
                "If this is on, the LFO cycle will be restarted when you press a key.\r\n", "", 0x00e0);
            Add(3, 5, (byte)ItemIndex++, 0, "", "",
                "This allows the LFO to modulate the pitch, producing a vibrato effect.\r\n", "", 0x00e0);
            Add(3, 5, (byte)ItemIndex++, 0, "", "",
                "This allows the LFO to modulate the FILTER CUTOFF (cutoff frequency), producing " +
                "a wah effect.\r\n", "", 0x00e0);
            Add(3, 5, (byte)ItemIndex++, 0, "", "",
                "This allows the LFO to modulate the AMP LEVEL (volume), producing a tremolo effe" +
                "ct.\r\n", "", 0x00e0);
            Add(3, 5, (byte)ItemIndex++, 0, "", "",
                "Here\'s how to make the PAN (stereo position) vary (Auto Panning).\r\n", "", 0x00e0);
            // SuperNATURAL Synth Tone Mod LFO tab
            ItemIndex = Skip;
            Add(3, 6, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 1 to be heard.\r\n", "", 0x00e0);
            Add(3, 6, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 2 to be heard.\r\n", "", 0x00e0);
            Add(3, 6, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 3 to be heard.\r\n", "", 0x00e0);
            Add(3, 6, (byte)ItemIndex, 0, "Selects the MODULATION LFO waveform.\r\n" +
                "There is an LFO that is always applied to the partial, and a MODULATION LFO for " +
                "applying modulation when the pitch bend/modulation lever is moved away" +
                " from yourself.\r\n", "_SNS_Osc_04.png",
                "Triangle wave.\r\n", "", 0x5810);
            Add(3, 6, (byte)ItemIndex, 1, "Selects the MODULATION LFO waveform.\r\n" +
                "There is an LFO that is always applied to the partial, and a MODULATION LFO for " +
                "applying modulation when the pitch bend/modulation lever is moved away" +
                " from yourself.\r\n", "_SNS_Osc_05.png",
                "Sine wave.\r\n", "", 0x5810);
            Add(3, 6, (byte)ItemIndex, 2, "Selects the MODULATION LFO waveform.\r\n" +
                "There is an LFO that is always applied to the partial, and a MODULATION LFO for " +
                "applying modulation when the pitch bend/modulation lever is moved away" +
                " from yourself.\r\n", "_SNS_Osc_01.png",
                "Sawtooth wave.\r\n", "", 0x5810);
            Add(3, 6, (byte)ItemIndex, 3, "Selects the MODULATION LFO waveform.\r\n" +
                "There is an LFO that is always applied to the partial, and a MODULATION LFO for " +
                "applying modulation when the pitch bend/modulation lever is moved away" +
                " from yourself.\r\n", "_SNS_Osc_02.png",
                "Square wave.\r\n", "", 0x5810);
            Add(3, 6, (byte)ItemIndex, 4, "Selects the MODULATION LFO waveform.\r\n" +
                "There is an LFO that is always applied to the partial, and a MODULATION LFO for " +
                "applying modulation when the pitch bend/modulation lever is moved away" +
                " from yourself.\r\n", "",
                "Sample and Hold (The LFO value will change once each cycle.)\r\n", "", 0x5810);
            Add(3, 6, (byte)ItemIndex++, 5, "Selects the MODULATION LFO waveform.\r\n" +
                "There is an LFO that is always applied to the partial, and a MODULATION LFO for " +
                "applying modulation when the pitch bend/modulation lever is moved away" +
                " from yourself.\r\n", "_SNS_Osc_12.png",
                "Random wave.\r\n", "", 0x5810);
            Add(3, 6, (byte)ItemIndex++, 0, "", "",
                "Specifies the LFO rate when Modulation LFO Tempo Sync Switch is OFF.\r\n", "", 0x00e0);
            Add(3, 6, (byte)ItemIndex++, 0, "", "",
                "If this is ON, the LFO rate can be specified as a note value relative to the tem" +
                "po.\r\n", "", 0x00e0);
            Add(3, 6, (byte)ItemIndex++, 0, "", "",
                "Specifies the LFO rate when Modulation LFO Tempo Sync Switch is ON.\r\n", "", 0x00e0);
            Add(3, 6, (byte)ItemIndex++, 0, "", "",
                "This allows the LFO to modulate the pitch, producing a vibrato effect.\r\n", "", 0x00e0);
            Add(3, 6, (byte)ItemIndex++, 0, "", "",
                "This allows the LFO to modulate the FILTER CUTOFF (cutoff frequency), producing " +
                "a wah effect.\r\n", "", 0x00e0);
            Add(3, 6, (byte)ItemIndex++, 0, "", "",
                "This allows the LFO to modulate the AMP LEVEL (volume), producing a tremolo effe" +
                "ct.\r\n", "", 0x00e0);
            Add(3, 6, (byte)ItemIndex++, 0, "", "",
                "Here\'s how to make the PAN (stereo position) vary (Auto Panning).\r\n", "", 0x00e0);
            Add(3, 6, (byte)ItemIndex++, 0, "", "",
                "Make these settings if you want to change the Modulation LFO Rate when the modul" +
                "ation lever is operated.\r\n" +
                "\r\n" +
                "Specify a positive “+” value if you want the Modulation LFO Rate to speed up whe" +
                "n you move the modulation lever; specify a negative “-” value if you w" +
                "ant it to slow down.\r\n", "", 0x00e0);
            // SuperNATURAL Synth Tone Aftertouch tab
            ItemIndex = Skip;
            Add(3, 7, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 1 to be heard.\r\n", "", 0x00e0);
            Add(3, 7, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 2 to be heard.\r\n", "", 0x00e0);
            Add(3, 7, (byte)ItemIndex++, 0, "", "",
                "Use this button to turn on partial 3 to be heard.\r\n", "", 0x00e0);
            Add(3, 7, (byte)ItemIndex++, 0, "", "",
                "Specifies how aftertouch pressure will affect the cutoff frequency.\r\n" +
                "\r\n" +
                "Specify a positive “+” value if you want aftertouch to raise the cutoff frequenc" +
                "y; specify a negative “-” value if you want aftertouch to lower the cu" +
                "toff frequency\r\n", "", 0x00e0);
            Add(3, 7, (byte)ItemIndex++, 0, "", "",
                "Specifies how aftertouch pressure will affect the volume.\r\n" +
                "\r\n" +
                "Specify a positive “+” value if you want aftertouch to increase the volume; spec" +
                "ify a negative “-” value if you want aftertouch to decrease the volume" +
                ".\r\n", "", 0x00e0);
            // SuperNATURAL Synth Tone Misc tab
            ItemIndex = Skip;
            Add(3, 8, (byte)ItemIndex++, 0, "", "",
                "Shortens the FILTER and AMP Attack Time according to the spacing between note-on" +
                " events.\r\n" +
                "\r\n" +
                "Higher values produce a greater effect. With a setting of 0, there will be no ef" +
                "fect. This is effective" +
                "when you want to play rapid notes using a sound that has a slow attack (Attack T" +
                "ime).\r\n", "", 0x00e0);
            Add(3, 8, (byte)ItemIndex++, 0, "", "",
                "Shortens the FILTER and AMP Release Time if the interval between one note-on and" +
                " the next note-off is brief. Higher values produce a greater effect. W" +
                "ith a setting of 0, there will be no effect.\r\n" +
                "\r\n" +
                "This is effective when you want to play staccato notes using a sound that has a " +
                "slow release.\r\n", "", 0x00e0);
            Add(3, 8, (byte)ItemIndex++, 0, "", "",
                "Shortens the Portamento Time according to the spacing between note-on events. Hi" +
                "gher values produce a greater effect. With a setting of 0, there will " +
                "be no effect.\r\n", "", 0x00e0);
            Add(3, 8, (byte)ItemIndex, 0, "Use this to loop the envelope between certain regions during a note-on.\r\n", "",
                "The envelope will operate normally.\r\n", "", 0x3650);
            Add(3, 8, (byte)ItemIndex, 1, "Use this to loop the envelope between certain regions during a note-on.\r\n", "_SNS_Misc/01.png",
                "When the Decay segment has ended, the envelope will return to the Attack. The At" +
                "tack through Decay segments will repeat until note-off occurs.\r\n", "", 0x3650);
            Add(3, 8, (byte)ItemIndex++, 2, "Use this to loop the envelope between certain regions during a note-on.\r\n", "_SNS_Misc/01.png",
                "Specifies the loop rate as a note value (Sync Note parameter).\r\n", "", 0x3650);
            Add(3, 8, (byte)ItemIndex++, 0, "", "",
                "Returns to the Attack at the specified rate. If the Attack+Decay time is shorter" +
                " than the specified rate, the Sustain Level will be maintained. If the" +
                " Attack+Decay time is longer than the specified rate, the envelope wil" +
                "l return to the Attack even though the Decay has not been completed.\r\n" +
                "\r\n" +
                "This will continue repeating until note-off occurs.\r\n", "", 0x00e0);
            Add(3, 8, (byte)ItemIndex++, 0, "", "",
                "If this is turned on, portamento will operate in semitone steps.\r\n", "", 0x00e0);
            // SuperNATURAL Synth Tone MFX control tab:
            ItemIndex = Skip;
            Add(3, 10, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(3, 10, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(3, 10, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(3, 10, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(3, 10, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(3, 10, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(3, 10, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(3, 10, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(3, 10, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(3, 10, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(3, 10, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(3, 10, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            // SuperNATURAL Synth Tone Save tab:
            ItemIndex = Skip;
            Add(3, 11, (byte)ItemIndex++, 0, "", "",
                "Type in a name for the tone.\r\n" +
                "\r\n" +
                "Names can be up to 12 characters and will be truncated if you enter more than 12.\r\n" +
                "\r\n" +
                "This name will be displayed on the INTEGRA-7.\r\n", "", 0x00e0);
            Add(3, 11, (byte)ItemIndex++, 0, "", "",
                "Select a slot to save the tone in.\r\n", "", 0x00e0);
            Add(3, 11, (byte)ItemIndex++, 0, "", "",
                "Saves the tone in the selected slot.\r\n", "", 0x00e0);
            Add(3, 11, (byte)ItemIndex++, 0, "", "",
                "Deletes the tone in the selected slot.\r\n\r\n" +
                "This has no effect on the tone you are editing, you can still save it in any slot.", "", 0x00e0);
            // SuperNATURAL Drum Kit Tone Common tab
            ItemIndex = Skip;
            Add(4, 0, (byte)ItemIndex++, 0, "SuperNATURAL Drum Kit", "", "Pressing and holding the volume control on the" +
                " INTEGRA-7 will make it play a phrase. You can select what phrase will be played here. " +
                "Default is a phrase specifically created for the current tone.\r\n\r\nYou can use the \'Play\'" +
                " button below to play/stop the selected phrase.");
            Add(4, 0, (byte)ItemIndex++, 0, "", "",
                "Sets the volume of the entire drum kit.\r\n", "", 0x00e0);
            Add(4, 0, (byte)ItemIndex++, 0, "", "",
                "Specifies the volume of the drum kit resonances and the resonances of the room. " +
                "This applies only for sounds whose type is Kick, Snare, Tom, and Hi-Ha" +
                "t.\r\n" +
                "\r\n" +
                "* For some drum instruments, this will have no effect. Refer to “SuperNATURAL Dr" +
                "um Inst List” (p. 41).\r\n", "", 0x00e0);
            // SuperNATURAL Drum Kit Drum Instrument tab
            ItemIndex = Skip;
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Selects the drum inst bank assigned to partial.\r\n" +
                "\r\n" +
                "INT: Internal inst bank\r\n" +
                "ExSN6: Expanded inst bank.\r\n\r\nDo not forget to load the ExSN6 expansion module!\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Selects the drum inst number assigned to partial.\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Sets the volume of the drum instrument.\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Sets the pan of the drum instrument.\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies the level of the signal sent to the chorus for each drum inst.\r\n" +
                "\r\n" +
                "* This has no effect if motional surround is on.\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies the level of the signal sent to the reverb for each drum inst.\r\n" +
                "\r\n" +
                "* This has no effect if motional surround is on.\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Adjusts the pitch of the drum inst.\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Adjusts the level and time of the attack. A setting of 100% produces the fastest" +
                " attack.\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Adjusts the decay time. Negative “-” settings will produce a muting effect.\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Adjusts the brilliance of the sound. Positive “+” settings make the sound bright" +
                "er, and negative “-” settings make the sound darker.\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies performance variations such as flam, buzz, or roll.\r\n" +
                "\r\n" +
                "* The parameters available for editing will depend on the drum instrument. Refer" +
                " to “SuperNATURAL Drum Inst List” (p. 41).\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies the curve by which velocity will affect the volume. With a setting of " +
                "0, any velocity will produce the maximum volume.\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Adjusts the stereo width of the sound. A setting of 0 is monaural.\r\n" +
                "\r\n" +
                "* For some drum instruments, this will have no effect. Refer to “SuperNATURAL Dr" +
                "um Inst List” (p. 41).\r\n", "", 0x00e0);
            Add(4, 1, (byte)ItemIndex++, 0, "", "",
                "Specifies for each drum inst how the sound will be output.\r\n", "", 0x00e0);
            // SuperNATURAL Drum Kit Tone Comp tab
            ItemIndex = Skip;
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Compressor 1 on/off setting.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Compressor 2 on/off setting.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Compressor 3 on/off setting.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Compressor 4 on/off setting.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Compressor 5 on/off setting.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Compressor 6 on/off setting.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Time from when the input exceeds the threshold until compression 1 begins.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Time from when the input exceeds the threshold until compression 2 begins.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Time from when the input exceeds the threshold until compression 3 begins.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Time from when the input exceeds the threshold until compression 4 begins.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Time from when the input exceeds the threshold until compression 5 begins.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Time from when the input exceeds the threshold until compression 6 begins.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Time from when the input falls below the threshold until compression 1 is turned o" +
                "ff.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Time from when the input falls below the threshold until compression 2 is turned o" +
                "ff.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Time from when the input falls below the threshold until compression 3 is turned o" +
                "ff.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Time from when the input falls below the threshold until compression 4 is turned o" +
                "ff.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Time from when the input falls below the threshold until compression 5 is turned o" +
                "ff.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Time from when the input falls below the threshold until compression 6 is turned o" +
                "ff.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Level above which compression 1 is applied.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Level above which compression 2 is applied.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Level above which compression 3 is applied.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Level above which compression 4 is applied.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Level above which compression 5 is applied.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Level above which compression 6 is applied.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Compression 1 ratio.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Compression 2 ratio.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Compression 3 ratio.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Compression 4 ratio.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Compression 5 ratio.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Compression 6 ratio.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Level of the compressor 1 output sound.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Level of the compressor 2 output sound.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Level of the compressor 3 output sound.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Level of the compressor 4 output sound.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Level of the compressor 5 output sound.\r\n", "", 0x00e0);
            Add(4, 2, (byte)ItemIndex++, 0, "", "",
                "Level of the compressor 6 output sound.\r\n", "", 0x00e0);
            // SuperNATURAL Drum Kit Tone Eq tab
            ItemIndex = Skip;
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Equalizer 1 on/off setting.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Equalizer 2 on/off setting.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Equalizer 3 on/off setting.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Equalizer 4 on/off setting.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Equalizer 5 on/off setting.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Equalizer 6 on/off setting.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 1 low range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 2 low range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 3 low range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 4 low range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 5 low range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 6 low range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of the eqalizer 1 low frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of the eqalizer 2 low frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of the eqalizer 3 low frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of the eqalizer 4 low frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of the eqalizer 5 low frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of the eqalizer 6 low frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 1 middle range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 2 middle range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 3 middle range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 4 middle range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 5 middle range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 6 middle range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of eqalizer 1 middle frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of eqalizer 2 middle frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of eqalizer 3 middle frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of eqalizer 4 middle frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of eqalizer 5 middle frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of eqalizer 6 middle frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Width of eqalizer 1 middle frequency range.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Width of eqalizer 2 middle frequency range.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Width of eqalizer 3 middle frequency range.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Width of eqalizer 4 middle frequency range.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Width of eqalizer 5 middle frequency range.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Width of eqalizer 6 middle frequency range.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 1 high range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 2 high range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 3 high range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 4 high range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 5 high range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Frequency of eqalizer 6 high range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of eqalizer 1 high frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of eqalizer 2 high frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of eqalizer 3 high frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of eqalizer 4 high frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of eqalizer 5 high frequency range.\r\n", "", 0x00e0);
            Add(4, 3, (byte)ItemIndex++, 0, "", "",
                "Gain of eqalizer 6 high frequency range.\r\n", "", 0x00e0);
            // SuperNATURAL Drum Kit MFX control tab:
            ItemIndex = Skip;
            Add(4, 5, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(4, 5, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(4, 5, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(4, 5, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(4, 5, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(4, 5, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(4, 5, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(4, 5, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(4, 5, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            Add(4, 5, (byte)ItemIndex++, 0, "", "",
                "Sets the MIDI message used to change the multi-effects parameter with the multi-" +
                "effects control.\r\n" +
                "\r\n" +
                "OFF: Multi-effects control will not be used.\r\n" +
                "\r\n" +
                "CC01–31, 33–95: Control Change\r\n" +
                "\r\n" +
                "PITCH BEND: Pitch Bend\r\n" +
                "\r\n" +
                "AFTERTOUCH: Aftertouch\r\n" +
                "\r\n" +
                "SYS CTRL1–SYS CTRL4: MIDI messages used as common multi-effects controls.\r\n", "", 0x00e0);
            Add(4, 5, (byte)ItemIndex++, 0, "", "",
                "Sets the multi-effects parameters to be controlled with the multi-effects contro" +
                "l.\r\n" +
                "\r\n" +
                "The multi-effects parameters available for control will depend on the multi-effe" +
                "cts type. For details, refer to “MFX Parameters” (p. 97).\r\n", "", 0x00e0);
            Add(4, 5, (byte)ItemIndex++, 0, "", "",
                "Sets the amount of the multi-effects control’s effect that is applied.\r\n" +
                "\r\n" +
                "To make an increase in the currently selected value (to get higher values, move " +
                "to the right, increase rates, and so on), select a positive value.\r\n" +
                "\r\n" +
                "To make a decrease in the currently selected value (to get lower values, move to" +
                " the left, decrease rates, and so on), select a negative value.\r\n" +
                "\r\n" +
                "For either positive or negative settings, greater absolute values will allow gre" +
                "ater amounts of change. Set this to “0” if you don’t want to apply the" +
                " effect.\r\n", "", 0x00e0);
            // SuperNATURAL Drum Kit Save tab:
            ItemIndex = Skip;
            Add(4, 6, (byte)ItemIndex++, 0, "", "",
                "Type in a name for the tone.\r\n" +
                "\r\n" +
                "Names can be up to 12 characters and will be truncated if you enter more than 12.\r\n" +
                "\r\n" +
                "This name will be displayed on the INTEGRA-7.\r\n", "", 0x00e0);
            Add(4, 6, (byte)ItemIndex++, 0, "", "",
                "Select a slot to save the tone in.\r\n", "", 0x00e0);
            Add(4, 6, (byte)ItemIndex++, 0, "", "",
                "Saves the tone in the selected slot.\r\n", "", 0x00e0);
            Add(4, 6, (byte)ItemIndex++, 0, "", "",
                "Deletes the tone in the selected slot.\r\n\r\n" +
                "This has no effect on the tone you are editing, you can still save it in any slot.", "", 0x00e0);
            // MFX (first index is 5 to indicate MFX type, Tone type has to be added separatly):
            ItemIndex = 0;
            byte PageIndex = 0;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "MFX type selector.", "", "The MFX type selector selects one of the 67 available Multi Effects available on the INTEGRA-7", "");
            // 01: Equalizer
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "01: Equalizer", "", "Frequency of the low range", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "01: Equalizer", "", "Gain of the low range", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "01: Equalizer", "", "Frequency of the middle range 1", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "01: Equalizer", "", "Gain of the middle range 1", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "01: Equalizer", "", "Width of the middle range 1. \r\n\r\nSet a higher value for Q to narrow the range to be affected.", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "01: Equalizer", "", "Frequency of the middle range 2, 200", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "01: Equalizer", "", "Gain of the middle range 2", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "01: Equalizer", "", "Width of the middle range 2. \r\n\r\nSet a higher value for Q to narrow the range to be affected", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "01: Equalizer", "", "Frequency of the high range", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "01: Equalizer", "", "Gain of the high range", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "01: Equalizer", "", "Output level", "");
            // 02: Spectrum
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "02: Spectrum", "", "Band 1 (250 Hz) gain", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "02: Spectrum", "", "Band 2 (500 Hz) gain", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "02: Spectrum", "", "Band 3 (1000 Hz) gain", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "02: Spectrum", "", "Band 4 (1250 Hz) gain", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "02: Spectrum", "", "Band 5 (2000 Hz) gain", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "02: Spectrum", "", "Band 6 (3150 Hz) gain", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "02: Spectrum", "", "Band 7 (4000 Hz) gain", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "02: Spectrum", "", "Band 8 (8000 Hz) gain", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "02: Spectrum", "", "Ranges width for all the frequency bands.", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "02: Spectrum", "", "Output level", "");
            //03: Low Boost
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "03: Low Boost", "", "Center frequency at which the lower range will be boosted", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "03: Low Boost", "", "Amount by which the lower range will be boosted", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "03: Low Boost", "", "Width of the lower range that will be boosted", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "03: Low Boost", "", "Gain of the low frequency range", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "03: Low Boost", "", "Gain of the high frequency range", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "03: Low Boost", "", "Output level", "");
            // 04: Step Filter step 1 - 8
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 1 - 8", "", "Cutoff frequency at step 1", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 1 - 8", "", "Cutoff frequency at step 2", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 1 - 8", "", "Cutoff frequency at step 3", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 1 - 8", "", "Cutoff frequency at step 4", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 1 - 8", "", "Cutoff frequency at step 5", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 1 - 8", "", "Cutoff frequency at step 6", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 1 - 8", "", "Cutoff frequency at step 7", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 1 - 8", "", "Cutoff frequency at step 8", "");
            // 04: Step Filter step 9 - 16
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 9 - 16", "", "Cutoff frequency at step 9", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 9 - 16", "", "Cutoff frequency at step 10", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 9 - 16", "", "Cutoff frequency at step 11", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 9 - 16", "", "Cutoff frequency at step 12", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 9 - 16", "", "Cutoff frequency at step 13", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 9 - 16", "", "Cutoff frequency at step 14", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 9 - 16", "", "Cutoff frequency at step 15", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter step 9 - 16", "", "Cutoff frequency at step 16", "");
            // 04: Step Filter settings
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter settings", "", "Select method for setting modulation time, Hz or note lengt", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter settings", "", "Modulation measured in Hz", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter settings", "", "Modulation measured in note length", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter settings", "", "Speed at which the cutoff frequency changes between steps", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter settings", "", "Filter type Frequency range that will pass through each filter.\r\n\r\n" +
                "LPF: frequencies below the cutoff\r\n\r\nBPF: frequencies in the region of the cutoff\r\n\r\nHPF: frequencies above the cutoff\r\n\r\n" +
                "NOTCH: frequencies other than the region of the cutoff", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter settings", "", "Amount of attenuation per octave\r\n\r\n- 12 dB: gentle\r\n\r\n- 24 dB: steep\r\n\r\n- 36 dB: extremely steep", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter settings", "", "Filter resonance level\r\n\r\nIncreasing this value will emphasize the region near the cutoff frequency.", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter settings", "", "Amount of boost for the filter output", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "04: Step Filter settings", "", "Output level", "");
            // 05: Enhancer
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "05: Enhancer", "", "Sensitivity of the enhancer", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "05: Enhancer", "", "Level of the overtones generated by the enhancer", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "05: Enhancer", "", "Gain of the low range", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "05: Enhancer", "", "Gain of the high range", "");
            Add(5, PageIndex, (byte)ItemIndex++, 0, "05: Enhancer", "", "Output Level", "");
            // 06: Auto Wah
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "06: Auto Wah\r\n" +
                "\r\n" +
                "Cyclically controls a filter to create cyclic change in timbre.\r\n", "_MFX_OdDsToAutoWah.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "LPF: The wah effect will be applied over a wide frequency range.\r\n" +
                "\r\n" +
                "BPF: The wah effect will be applied over a narrow frequency range.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "06: Auto Wah\r\n" +
                "\r\n" +
                "Cyclically controls a filter to create cyclic change in timbre.\r\n", "_MFX_AutoWah.png",
                "Adjusts the center frequency at which the effect is applied.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "06: Auto Wah\r\n" +
                "\r\n" +
                "Cyclically controls a filter to create cyclic change in timbre.\r\n", "_MFX_AutoWah.png",
                "Adjusts the amount of the wah effect that will occur in the range of the center " +
                "frequency.\r\n" +
                "\r\n" +
                "Set a higher value for Q to narrow the range to be affected.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "06: Auto Wah\r\n" +
                "\r\n" +
                "Cyclically controls a filter to create cyclic change in timbre.\r\n", "_MFX_AutoWah.png",
                "Adjusts the sensitivity with which the filter is controlled.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "06: Auto Wah\r\n" +
                "\r\n" +
                "Cyclically controls a filter to create cyclic change in timbre.\r\n", "_MFX_AutoWah.png",
                "Sets the direction in which the frequency will change when the auto-wah filter i" +
                "s modulated.\r\n" +
                "\r\n" +
                "UP: The filter will change toward a higher frequency.\r\n" +
                "\r\n" +
                "DOWN: The filter will change toward a lower frequency.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "06: Auto Wah\r\n" +
                "\r\n" +
                "Cyclically controls a filter to create cyclic change in timbre.\r\n", "_MFX_AutoWah.png",
                "Frequency of modulation.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "06: Auto Wah\r\n" +
                "\r\n" +
                "Cyclically controls a filter to create cyclic change in timbre.\r\n", "_MFX_AutoWah.png",
                "Depth of modulation.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "06: Auto Wah\r\n" +
                "\r\n" +
                "Cyclically controls a filter to create cyclic change in timbre.\r\n", "_MFX_AutoWah.png",
                "Adjusts the degree of phase shift of the left and right sounds when the wah effe" +
                "ct is applied.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "06: Auto Wah\r\n" +
                "\r\n" +
                "Cyclically controls a filter to create cyclic change in timbre.\r\n", "_MFX_AutoWah.png",
                "Gain of the low range.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "06: Auto Wah\r\n" +
                "\r\n" +
                "Cyclically controls a filter to create cyclic change in timbre.\r\n", "_MFX_AutoWah.png",
                "Gain of the high range.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "06: Auto Wah\r\n" +
                "\r\n" +
                "Cyclically controls a filter to create cyclic change in timbre.\r\n", "_MFX_AutoWah.png",
                "Output Level.\r\n", "", 0x3580);
            // 07: Humanizer
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Turns Drive on/off.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Degree of distortion.\r\n" +
                "\r\n" +
                "Also changes the volume.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Selects the vowel for vowel 1.\r\n", "", 0x3580);

            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Selects the vowel for vowel 2.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Select to use frequency or note lenght for timing regarding when the vowels switches from vowel 1 to vowel 2.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Frequency at which the vowels switches from vowel 1 to vowel 2.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Note length to decide when the vowels switches from vowel 1 to vowel 2.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Effect depth.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "LFO reset on/off.\r\n" +
                "\r\n" +
                "Determines whether the LFO for switching the vowels is reset by the input signal" +
                " (ON) or not (OFF).\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Volume level at which reset is applied.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Point at which Vowel 1/2 switch.\r\n\r\n49 or less: Vowel 1 will have a longer duration." +
                "\r\n" +
                "\r\n" +
                "50: Vowel 1 and 2 will be of equal duration.\r\n" +
                "\r\n" +
                "51 or more: Vowel 2 will have a longer duration.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Gain of the low frequency range.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Gain of the high frequency range.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Stereo location of the output.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "07: Humanizer\r\n" +
                "\r\n" +
                "Adds a vowel character to the sound, making it similar to a human voice.\r\n", "_MFX_Humanizer.png",
                "Output level.\r\n", "", 0x3580);
            // 08: Speaker Simulator
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "08: Speaker Simulator\r\n" +
                "\r\n" +
                "Simulates the speaker type and mic settings used to record the speaker sound.\r\n", "_MFX_SpeakerSimulator.png",
                "Type of speaker.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "08: Speaker Simulator\r\n" +
                "\r\n" +
                "Simulates the speaker type and mic settings used to record the speaker sound.\r\n", "_MFX_SpeakerSimulator.png",
                "Adjusts the location of the mic that is recording the sound of the speaker.\r\n" +
                "\r\n" +
                "This can be adjusted in three steps, with the mic becoming more distant in the o" +
                "rder of 1, 2, and 3.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "08: Speaker Simulator\r\n" +
                "\r\n" +
                "Simulates the speaker type and mic settings used to record the speaker sound.\r\n", "_MFX_SpeakerSimulator.png",
                "Volume of the microphone.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "08: Speaker Simulator\r\n" +
                "\r\n" +
                "Simulates the speaker type and mic settings used to record the speaker sound.\r\n", "_MFX_SpeakerSimulator.png",
                "Volume of the direct sound.\r\n", "", 0x3580);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "08: Speaker Simulator\r\n" +
                "\r\n" +
                "Simulates the speaker type and mic settings used to record the speaker sound.\r\n", "_MFX_SpeakerSimulator.png",
                "Output Level.\r\n", "", 0x3580);
            // 09: Phaser 1
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Number of stages in the phaser.\r\n", "", 0x2680);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Adjusts the basic frequency from which the sound will be modulated.\r\n", "", 0x2680);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Select to use Hertz or note length to assing modulation frequency.\r\n", "", 0x2680);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x2680);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Frequency of modulation as note lenght.\r\n", "", 0x2680);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Depth of modulation.\r\n", "", 0x2680);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Selects whether the left and right phase of the modulation will be the same or t" +
                "he opposite.\r\n" +
                "\r\n" +
                "INVERSE: The left and right phase will be opposite. When using a mono source, th" +
                "is spreads the sound.\r\n" +
                "\r\n" +
                "SYNCHRO: The left and right phase will be the same. Select this when inputting a" +
                " stereo source.\r\n", "", 0x2680);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Amount of feedback.\r\n", "", 0x2680);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Adjusts the proportion of the phaser sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x2680);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Level of the phase-shifted sound.\r\n", "", 0x2680);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Gain of the low range.\r\n", "", 0x2680);

            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Gain of the high range.\r\n", "", 0x2680);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "09: Phaser 1\r\n" +
                "A phase-shifted sound is added to the original sound and modulated.\r\n", "_MFX_Phaser1.png",
                "Output Level.\r\n", "", 0x2680);
            // 10: Phaser 2
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "10: Phaser 2\r\n" +
                "\r\n" +
                "This simulates an analog phaser of the past.\r\n" +
                "\r\n" +
                "It is particularly suitable for electric piano.\r\n", "_MFX_Phaser2And3.png",
                "Frequency of modulation.\r\n", "", 0x4640);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "10: Phaser 2\r\n" +
                "\r\n" +
                "This simulates an analog phaser of the past.\r\n" +
                "\r\n" +
                "It is particularly suitable for electric piano.\r\n", "_MFX_Phaser2And3.png",
                "Modulation character.\r\n", "", 0x4640);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "10: Phaser 2\r\n" +
                "\r\n" +
                "This simulates an analog phaser of the past.\r\n" +
                "\r\n" +
                "It is particularly suitable for electric piano.\r\n", "_MFX_Phaser2And3.png",
                "Gain of the low range.\r\n", "", 0x4640);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "10: Phaser 2\r\n" +
                "\r\n" +
                "This simulates an analog phaser of the past.\r\n" +
                "\r\n" +
                "It is particularly suitable for electric piano.\r\n", "_MFX_Phaser2And3.png",
                "Gain of the high range.\r\n", "", 0x4640);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "10: Phaser 2\r\n" +
                "\r\n" +
                "This simulates an analog phaser of the past.\r\n" +
                "\r\n" +
                "It is particularly suitable for electric piano.\r\n", "_MFX_Phaser2And3.png",
                "Output Level.\r\n", "", 0x4640);
            // 11: Phaser 3
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "11: Phaser 3\r\n" +
                "\r\n" +
                "This simulates a different analog phaser than Phaser 2.\r\n" +
                "\r\n" +
                "It is particularly suitable for electric piano.\r\n", "_MFX_Phaser2And3.png",
                "Frequency of modulation.\r\n", "", 0x4640);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "11: Phaser 3\r\n" +
                "\r\n" +
                "This simulates a different analog phaser than Phaser 2.\r\n" +
                "\r\n" +
                "It is particularly suitable for electric piano.\r\n", "_MFX_Phaser2And3.png",
                "Gain of the low range.\r\n", "", 0x4640);

            Add(5, PageIndex, (byte)ItemIndex++, 0, "11: Phaser 3\r\n" +
                "\r\n" +
                "This simulates a different analog phaser than Phaser 2.\r\n" +
                "\r\n" +
                "It is particularly suitable for electric piano.\r\n", "_MFX_Phaser2And3.png",
                "Gain of the high range.\r\n", "", 0x4640);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "11: Phaser 3\r\n" +
                "\r\n" +
                "This simulates a different analog phaser than Phaser 2.\r\n" +
                "\r\n" +
                "It is particularly suitable for electric piano.\r\n", "_MFX_Phaser2And3.png",
                "Output Level.\r\n", "", 0x4640);
            // 12: Step Phaser
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Number of stages in the phaser.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Adjusts the basic frequency from which the sound will be modulated.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Select to use Hertz or note lenght to set modulation frequency.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Frequency of modulation in note length.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Depth of modulation.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Selects whether the left and right phase of the modulation will be the same or t" +
                "he opposite.\r\n" +
                "\r\n" +
                "INVERSE: The left and right phase will be opposite. When using a mono source, th" +
                "is spreads the sound.\r\n" +
                "\r\n" +
                "SYNCHRO: The left and right phase will be the same. Select this when inputting a" +
                " stereo source.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Amount of feedback.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Adjusts the proportion of the phaser sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Select to use Hertz or note length to set the rate of the step-wise change in the phaser effect.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Rate of the step-wise change in the phaser effect in Hertz.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Rate of the step-wise change in the phaser effect as note length.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Level of the phase-shifted sound.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Gain of the low range.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Gain of the high range.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "12: Step Phaser\r\n" +
                "\r\n" +
                "The phaser effect will be varied gradually.\r\n", "_MFX_StepPhaser.png",
                "Output Level.\r\n", "", 0x3560);
            // 13: Multi Stage Phaser
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "13: Multi Stage Phaser\r\n" +
                "\r\n" +
                "Extremely high settings of the phase difference produce a deep phaser effect.\r\n", "_MFX_MultiStagePhaser.png",
                "Number of phaser stages.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "13: Multi Stage Phaser\r\n" +
                "\r\n" +
                "Extremely high settings of the phase difference produce a deep phaser effect.\r\n", "_MFX_MultiStagePhaser.png",
                "Adjusts the basic frequency from which the sound will be modulated.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "13: Multi Stage Phaser\r\n" +
                "\r\n" +
                "Extremely high settings of the phase difference produce a deep phaser effect.\r\n", "_MFX_MultiStagePhaser.png",
                "Select Hertz or note length to set modulation frequency.\r\n", "", 0x3560);

            Add(5, PageIndex, (byte)ItemIndex++, 0, "13: Multi Stage Phaser\r\n" +
                "\r\n" +
                "Extremely high settings of the phase difference produce a deep phaser effect.\r\n", "_MFX_MultiStagePhaser.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x3560);

            Add(5, PageIndex, (byte)ItemIndex++, 0, "13: Multi Stage Phaser\r\n" +
                "\r\n" +
                "Extremely high settings of the phase difference produce a deep phaser effect.\r\n", "_MFX_MultiStagePhaser.png",
                "Frequency of modulation as note length.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "13: Multi Stage Phaser\r\n" +
                "\r\n" +
                "Extremely high settings of the phase difference produce a deep phaser effect.\r\n", "_MFX_MultiStagePhaser.png",
                "Depth of modulation.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "13: Multi Stage Phaser\r\n" +
                "\r\n" +
                "Extremely high settings of the phase difference produce a deep phaser effect.\r\n", "_MFX_MultiStagePhaser.png",
                "Amount of feedback.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "13: Multi Stage Phaser\r\n" +
                "\r\n" +
                "Extremely high settings of the phase difference produce a deep phaser effect.\r\n", "_MFX_MultiStagePhaser.png",
                "Level of the phase-shifted sound.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "13: Multi Stage Phaser\r\n" +
                "\r\n" +
                "Extremely high settings of the phase difference produce a deep phaser effect.\r\n", "_MFX_MultiStagePhaser.png",
                "Stereo location of the output sound.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "13: Multi Stage Phaser\r\n" +
                "\r\n" +
                "Extremely high settings of the phase difference produce a deep phaser effect.\r\n", "_MFX_MultiStagePhaser.png",
                "Gain of the low range.\r\n", "", 0x3560);

            Add(5, PageIndex, (byte)ItemIndex++, 0, "13: Multi Stage Phaser\r\n" +
                "\r\n" +
                "Extremely high settings of the phase difference produce a deep phaser effect.\r\n", "_MFX_MultiStagePhaser.png",
                "Gain of the high range.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "13: Multi Stage Phaser\r\n" +
                "\r\n" +
                "Extremely high settings of the phase difference produce a deep phaser effect.\r\n", "_MFX_MultiStagePhaser.png",
                "Output Level.\r\n", "", 0x3560);
            // 14: Infinite Phaser
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "14: Infinite Phaser\r\n" +
                "\r\n" +
                "A phaser that continues raising/lowering the frequency at which the sound is mod" +
                "ulated.\r\n", "_MFX_InfinitePhaser.png",
                "Higher values will produce a deeper phaser effect.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "14: Infinite Phaser\r\n" +
                "\r\n" +
                "A phaser that continues raising/lowering the frequency at which the sound is mod" +
                "ulated.\r\n", "_MFX_InfinitePhaser.png",
                "Speed at which to raise or lower the frequency at which the sound is modulated (" +
                "+: upward / -: downward).\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "14: Infinite Phaser\r\n" +
                "\r\n" +
                "A phaser that continues raising/lowering the frequency at which the sound is mod" +
                "ulated.\r\n", "_MFX_InfinitePhaser.png",
                "Amount of feedback.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "14: Infinite Phaser\r\n" +
                "\r\n" +
                "A phaser that continues raising/lowering the frequency at which the sound is mod" +
                "ulated.\r\n", "_MFX_InfinitePhaser.png",
                "Volume of the phase-shifted sound.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "14: Infinite Phaser\r\n" +
                "\r\n" +
                "A phaser that continues raising/lowering the frequency at which the sound is mod" +
                "ulated.\r\n", "_MFX_InfinitePhaser.png",
                "Panning of the output sound.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "14: Infinite Phaser\r\n" +
                "\r\n" +
                "A phaser that continues raising/lowering the frequency at which the sound is mod" +
                "ulated.\r\n", "_MFX_InfinitePhaser.png",
                "Amount of boost/cut for the low-frequency range.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "14: Infinite Phaser\r\n" +
                "\r\n" +
                "A phaser that continues raising/lowering the frequency at which the sound is mod" +
                "ulated.\r\n", "_MFX_InfinitePhaser.png",
                "Amount of boost/cut for the high-frequency range.\r\n", "", 0x3560);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "14: Infinite Phaser\r\n" +
                "\r\n" +
                "A phaser that continues raising/lowering the frequency at which the sound is mod" +
                "ulated.\r\n", "_MFX_InfinitePhaser.png",
                "Output volume.\r\n", "", 0x3560);
            // 15: Ring Modulator
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "15: Ring Modulator\r\n" +
                "\r\n" +
                "This is an effect that applies amplitude modulation (AM) to the input signal, pr" +
                "oducing bell-like sounds.\r\n" +
                "\r\n" +
                "You can also change the modulation frequency in response to changes in the volum" +
                "e of the sound sent into the effect.\r\n", "_MFX_RingModulator.png",
                "Adjusts the frequency at which modulation is applied.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "15: Ring Modulator\r\n" +
                "\r\n" +
                "This is an effect that applies amplitude modulation (AM) to the input signal, pr" +
                "oducing bell-like sounds.\r\n" +
                "\r\n" +
                "You can also change the modulation frequency in response to changes in the volum" +
                "e of the sound sent into the effect.\r\n", "_MFX_RingModulator.png",
                "Adjusts the amount of frequency modulation applied.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "15: Ring Modulator\r\n" +
                "\r\n" +
                "This is an effect that applies amplitude modulation (AM) to the input signal, pr" +
                "oducing bell-like sounds.\r\n" +
                "\r\n" +
                "You can also change the modulation frequency in response to changes in the volum" +
                "e of the sound sent into the effect.\r\n", "_MFX_RingModulator.png",
                "Determines whether the frequency modulation moves towards higher frequencies (UP" +
                ") or lower frequencies (DOWN).\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "15: Ring Modulator\r\n" +
                "\r\n" +
                "This is an effect that applies amplitude modulation (AM) to the input signal, pr" +
                "oducing bell-like sounds.\r\n" +
                "\r\n" +
                "You can also change the modulation frequency in response to changes in the volum" +
                "e of the sound sent into the effect.\r\n", "_MFX_RingModulator.png",
                "Gain of the low frequency range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "15: Ring Modulator\r\n" +
                "\r\n" +
                "This is an effect that applies amplitude modulation (AM) to the input signal, pr" +
                "oducing bell-like sounds.\r\n" +
                "\r\n" +
                "You can also change the modulation frequency in response to changes in the volum" +
                "e of the sound sent into the effect.\r\n", "_MFX_RingModulator.png",
                "Gain of the high frequency range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "15: Ring Modulator\r\n" +
                "\r\n" +
                "This is an effect that applies amplitude modulation (AM) to the input signal, pr" +
                "oducing bell-like sounds.\r\n" +
                "\r\n" +
                "You can also change the modulation frequency in response to changes in the volum" +
                "e of the sound sent into the effect.\r\n", "_MFX_RingModulator.png",
                "Volume balance between the direct sound (D) and the effect sound (W).\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "15: Ring Modulator\r\n" +
                "\r\n" +
                "This is an effect that applies amplitude modulation (AM) to the input signal, pr" +
                "oducing bell-like sounds.\r\n" +
                "\r\n" +
                "You can also change the modulation frequency in response to changes in the volum" +
                "e of the sound sent into the effect.\r\n", "_MFX_RingModulator.png",
                "Output level.\r\n", "", 0x4550);
            // 16: Tremolo
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex, 0, "16: Tremolo\r\n" +
                "\r\n" +
                "Cyclically modulates the volume to add tremolo effect to the sound.\r\n", "_MFX_Tremolo.png",
                "Modulation Wave.\r\n" +
                "\r\n" +
                "TRI: triangle wave.\r\n" +
                "\r\n", "_SNS_Osc_04.png", 0x3434);
            Add(5, PageIndex, (byte)ItemIndex, 1, "16: Tremolo\r\n" +
                "\r\n" +
                "Cyclically modulates the volume to add tremolo effect to the sound.\r\n", "_MFX_Tremolo.png",
                "Modulation Wave.\r\n" +
                "\r\n" +
                "SQR: square wave.\r\n" +
                "\r\n", "_SNS_Osc_02.png", 0x3434);
            Add(5, PageIndex, (byte)ItemIndex, 2, "16: Tremolo\r\n" +
                "\r\n" +
                "Cyclically modulates the volume to add tremolo effect to the sound.\r\n", "_MFX_Tremolo.png",
                "Modulation Wave.\r\n" +
                "\r\n" +
                "SIN: sine wave.\r\n" +
                "\r\n", "_SNS_Osc_05.png", 0x3434);
            Add(5, PageIndex, (byte)ItemIndex, 3, "16: Tremolo\r\n" +
                "\r\n" +
                "Cyclically modulates the volume to add tremolo effect to the sound.\r\n", "_MFX_Tremolo.png",
                "Modulation Wave.\r\n" +
                "\r\n" +
                "SAW: sawtooth wave.\r\n" +
                "\r\n", "_SNS_Osc_01.png", 0x3434);
            Add(5, PageIndex, (byte)ItemIndex++, 4, "16: Tremolo\r\n" +
                "\r\n" +
                "Cyclically modulates the volume to add tremolo effect to the sound.\r\n", "_MFX_Tremolo.png",
                "Modulation Wave.\r\n" +
                "\r\n" +
                "SAW: reverse sawtooth wave.\r\n" +
                "\r\n", "_SNS_Osc_09.png", 0x3434);
            Add(5, PageIndex, (byte)ItemIndex++, 5, "16: Tremolo\r\n" +
                "\r\n" +
                "Cyclically modulates the volume to add tremolo effect to the sound.\r\n", "_MFX_Tremolo.png",
                "Select to change frequency in Herts or as note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "16: Tremolo\r\n" +
                "\r\n" +
                "Cyclically modulates the volume to add tremolo effect to the sound.\r\n", "_MFX_Tremolo.png",
                "Frequency of the change in Hertz.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "16: Tremolo\r\n" +
                "\r\n" +
                "Cyclically modulates the volume to add tremolo effect to the sound.\r\n", "_MFX_Tremolo.png",
                "Frequency of the change as note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "16: Tremolo\r\n" +
                "\r\n" +
                "Cyclically modulates the volume to add tremolo effect to the sound.\r\n", "_MFX_Tremolo.png",
                "Depth to which the effect is applied.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "16: Tremolo\r\n" +
                "\r\n" +
                "Cyclically modulates the volume to add tremolo effect to the sound.\r\n", "_MFX_Tremolo.png",
                "Gain of the low range.\r\n", "", 0x4550);

            Add(5, PageIndex, (byte)ItemIndex++, 0, "16: Tremolo\r\n" +
                "\r\n" +
                "Cyclically modulates the volume to add tremolo effect to the sound.\r\n", "_MFX_Tremolo.png",
                "Gain of the high range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "16: Tremolo\r\n" +
                "\r\n" +
                "Cyclically modulates the volume to add tremolo effect to the sound.\r\n", "_MFX_Tremolo.png",
                "Output level.\r\n", "", 0x4550);
            // 17: Auto Pan
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex, 0, "17: Auto Pan\r\n" +
                "\r\n" +
                "Cyclically modulates the stereo location of the sound.\r\n", "_MFX_AutoPan.png",
                "Modulation Wave.\r\n" +
                "\r\n" +
                "TRI: triangle wave.\r\n" +
                "\r\n", "_SNS_Osc_04.png", 0x3434);
            Add(5, PageIndex, (byte)ItemIndex, 1, "17: Auto Pan\r\n" +
                "\r\n" +
                "Cyclically modulates the stereo location of the sound.\r\n", "_MFX_AutoPan.png",
                "Modulation Wave.\r\n" +
                "\r\n" +
                "SQR: square wave.\r\n" +
                "\r\n", "_SNS_Osc_02.png", 0x3434);
            Add(5, PageIndex, (byte)ItemIndex, 2, "17: Auto Pan\r\n" +
                "\r\n" +
                "Cyclically modulates the stereo location of the sound.\r\n", "_MFX_AutoPan.png",
                "Modulation Wave.\r\n" +
                "\r\n" +
                "SIN: sine wave.\r\n" +
                "\r\n", "_SNS_Osc_05.png", 0x3434);
            Add(5, PageIndex, (byte)ItemIndex, 3, "17: Auto Pan\r\n" +
                "\r\n" +
                "Cyclically modulates the stereo location of the sound.\r\n", "_MFX_AutoPan.png",
                "Modulation Wave.\r\n" +
                "\r\n" +
                "SAW: sawtooth wave.\r\n" +
                "\r\n", "_SNS_Osc_01.png", 0x3434);
            Add(5, PageIndex, (byte)ItemIndex++, 4, "17: Auto Pan\r\n" +
                "\r\n" +
                "Cyclically modulates the stereo location of the sound.\r\n", "_MFX_AutoPan.png",
                "Modulation Wave.\r\n" +
                "\r\n" +
                "SAW: reverse sawtooth wave.\r\n" +
                "\r\n", "_SNS_Osc_09.png", 0x3434);
            Add(5, PageIndex, (byte)ItemIndex++, 5, "17: Auto Pan\r\n" +
                "\r\n" +
                "Cyclically modulates the stereo location of the sound.\r\n", "_MFX_AutoPan.png",
                "Select to change frequency in Herts or as note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "17: Auto Pan\r\n" +
                "\r\n" +
                "Cyclically modulates the stereo location of the sound.\r\n", "_MFX_AutoPan.png",
                "Frequency of the change in Hertz.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "17: Auto Pan\r\n" +
                "\r\n" +
                "Cyclically modulates the stereo location of the sound.\r\n", "_MFX_AutoPan.png",
                "Frequency of the change as note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "17: Auto Pan\r\n" +
                "\r\n" +
                "Cyclically modulates the stereo location of the sound.\r\n", "_MFX_AutoPan.png",
                "Depth to which the effect is applied.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "17: Auto Pan\r\n" +
                "\r\n" +
                "Cyclically modulates the stereo location of the sound.\r\n", "_MFX_AutoPan.png",
                "Gain of the low range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "17: Auto Pan\r\n" +
                "\r\n" +
                "Cyclically modulates the stereo location of the sound.\r\n", "_MFX_AutoPan.png",
                "Gain of the high range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "17: Auto Pan\r\n" +
                "\r\n" +
                "Cyclically modulates the stereo location of the sound.\r\n", "_MFX_AutoPan.png",
                "Output level.\r\n", "", 0x3434);
            // 18: Slicer step 1 - 8
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 1.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 2.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 3.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 4.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 5.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 6.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 7.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 8.\r\n", "", 0x4550);
            // 18: Slicer step 9 - 16
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 9.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 10.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 11.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 12.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 13.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 14.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 15.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Level at step 16.\r\n", "", 0x4550);
            // 18: Slicer settings
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Select Hertz or note length for setting rate at which the 16-step sequence will cycle.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Rate at which the 16-step sequence will cycle in Hertz.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Rate at which the 16-step sequence will cycle as note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Speed at which the level changes between steps.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Specifies whether an input note will cause the sequence to resume from the first" +
                " step of the sequence (ON) or not (OFF).\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Volume at which an input note will be detected.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Sets the manner in which the volume changes as one step progresses to the next.\r\n" +
                "\r\n" +
                "LEGATO: The change in volume from one step’s level to the next remains unaltered" +
                ". If the level of a following step is the same as the one preceding it" +
                ", there is no change in volume.\r\n" +
                "\r\n" +
                "SLASH: The level is momentarily set to 0 before progressing to the level of the " +
                "next step. This change in volume occurs even if the level of the follo" +
                "wing step is the same as the preceding step.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Timing of volume changes in levels for even-numbered steps (step 2, step 4, step" +
                " 6...).\r\n" +
                "\r\n" +
                "The higher the value, the later the beat progresses.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "18: Slicer\r\n" +
                "\r\n" +
                "By applying successive cuts to the sound, this effect turns a conventional sound" +
                " into a sound that appears to be played as a backing phrase. This is e" +
                "specially effective when applied to sustaintype sounds.\r\n" +
                "\r\n" +
                "You can use MFX CONTROL to restart the step sequence from the\r\n" +
                "beginning (p. 97).\r\n", "_MFX_Slicer.png",
                "Output level.\r\n", "", 0x4550);
            // 19: Rotary 1
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "19: Rotary 1\r\n" +
                "\r\n" +
                "This simulates a classic rotary speaker of the past.\r\n" +
                "\r\n" +
                "Since the operation of the high-frequency and low-frequency rotors can be specif" +
                "ied independently, the distinctive modulation can be reproduced realis" +
                "tically.\r\n" +
                "\r\n" +
                "This is most effective on organ patches.\r\n", "_MFX_Rotary1.png",
                "Simultaneously switch the rotational speed of the low frequency rotor and high f" +
                "requency rotor.\r\n" +
                "\r\n" +
                "SLOW: Slows down the rotation to the Slow Rate.\r\n" +
                "\r\n" +
                "FAST: Speeds up the rotation to the Fast Rate.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "19: Rotary 1\r\n" +
                "\r\n" +
                "This simulates a classic rotary speaker of the past.\r\n" +
                "\r\n" +
                "Since the operation of the high-frequency and low-frequency rotors can be specif" +
                "ied independently, the distinctive modulation can be reproduced realis" +
                "tically.\r\n" +
                "\r\n" +
                "This is most effective on organ patches.\r\n", "_MFX_Rotary1.png",
                "Slow speed of the low frequency rotor.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "19: Rotary 1\r\n" +
                "\r\n" +
                "This simulates a classic rotary speaker of the past.\r\n" +
                "\r\n" +
                "Since the operation of the high-frequency and low-frequency rotors can be specif" +
                "ied independently, the distinctive modulation can be reproduced realis" +
                "tically.\r\n" +
                "\r\n" +
                "This is most effective on organ patches.\r\n", "_MFX_Rotary1.png",
                "Fast speed of the low frequency rotor.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "19: Rotary 1\r\n" +
                "\r\n" +
                "This simulates a classic rotary speaker of the past.\r\n" +
                "\r\n" +
                "Since the operation of the high-frequency and low-frequency rotors can be specif" +
                "ied independently, the distinctive modulation can be reproduced realis" +
                "tically.\r\n" +
                "\r\n" +
                "This is most effective on organ patches.\r\n", "_MFX_Rotary1.png",
                "Adjusts the time it takes the low frequency rotor to reach the newly selected sp" +
                "eed when switching from fast to slow (or slow to fast) speed.\r\n" +
                "\r\n" +
                "Lower values will require longer times.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "19: Rotary 1\r\n" +
                "\r\n" +
                "This simulates a classic rotary speaker of the past.\r\n" +
                "\r\n" +
                "Since the operation of the high-frequency and low-frequency rotors can be specif" +
                "ied independently, the distinctive modulation can be reproduced realis" +
                "tically.\r\n" +
                "\r\n" +
                "This is most effective on organ patches.\r\n", "_MFX_Rotary1.png",
                "Volume of the low frequency rotor.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "19: Rotary 1\r\n" +
                "\r\n" +
                "This simulates a classic rotary speaker of the past.\r\n" +
                "\r\n" +
                "Since the operation of the high-frequency and low-frequency rotors can be specif" +
                "ied independently, the distinctive modulation can be reproduced realis" +
                "tically.\r\n" +
                "\r\n" +
                "This is most effective on organ patches.\r\n", "_MFX_Rotary1.png",
                "Slow speed of the low frequency rotor.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "19: Rotary 1\r\n" +
                "\r\n" +
                "This simulates a classic rotary speaker of the past.\r\n" +
                "\r\n" +
                "Since the operation of the high-frequency and high-frequency rotors can be specif" +
                "ied independently, the distinctive modulation can be reproduced realis" +
                "tically.\r\n" +
                "\r\n" +
                "This is most effective on organ patches.\r\n", "_MFX_Rotary1.png",
                "Fast speed of the high frequency rotor.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "19: Rotary 1\r\n" +
                "\r\n" +
                "This simulates a classic rotary speaker of the past.\r\n" +
                "\r\n" +
                "Since the operation of the high-frequency and high-frequency rotors can be specif" +
                "ied independently, the distinctive modulation can be reproduced realis" +
                "tically.\r\n" +
                "\r\n" +
                "This is most effective on organ patches.\r\n", "_MFX_Rotary1.png",
                "Adjusts the time it takes the high frequency rotor to reach the newly selected sp" +
                "eed when switching from fast to slow (or slow to fast) speed.\r\n" +
                "\r\n" +
                "Lower values will require longer times.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "19: Rotary 1\r\n" +
                "\r\n" +
                "This simulates a classic rotary speaker of the past.\r\n" +
                "\r\n" +
                "Since the operation of the high-frequency and high-frequency rotors can be specif" +
                "ied independently, the distinctive modulation can be reproduced realis" +
                "tically.\r\n" +
                "\r\n" +
                "This is most effective on organ patches.\r\n", "_MFX_Rotary1.png",
                "Volume of the high frequency rotor.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "19: Rotary 1\r\n" +
                "\r\n" +
                "This simulates a classic rotary speaker of the past.\r\n" +
                "\r\n" +
                "Since the operation of the high-frequency and low-frequency rotors can be specif" +
                "ied independently, the distinctive modulation can be reproduced realis" +
                "tically.\r\n" +
                "\r\n" +
                "This is most effective on organ patches.\r\n", "_MFX_Rotary1.png",
                "Spatial dispersion of the sound.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "19: Rotary 1\r\n" +
                "\r\n" +
                "This simulates a classic rotary speaker of the past.\r\n" +
                "\r\n" +
                "Since the operation of the high-frequency and low-frequency rotors can be specif" +
                "ied independently, the distinctive modulation can be reproduced realis" +
                "tically.\r\n" +
                "\r\n" +
                "This is most effective on organ patches.\r\n", "_MFX_Rotary1.png",
                "Output Level.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "19: Rotary 1", "", "", "");
            // 20: Rotary 2 Speed - woofer
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Rotational speed of the rotating speaker.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Switches the rotation of the rotary speaker.\r\n" +
                "\r\n" +
                "When this is turned on, the rotation will gradually stop.\r\n" +
                "\r\n" +
                "When it is turned off, the rotation will gradually resume.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Low-speed rotation speed of the woofer.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "High-speed rotation speed of the woofer.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Adjusts the rate at which the woofer rotation speeds up when the rotation is swi" +
                "tched from Slow to Fast.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Adjusts the rate at which the woofer rotation speeds up when the rotation is swi" +
                "tched from Fast to Slow.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Volume of the woofer.\r\n", "", 0x4550);
            // 20: Rotary 2 tweeter - level
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Low-speed rotation speed of the tweeter.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "High-speed rotation speed of the tweeter.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Adjusts the rate at which the tweeter rotation speeds up when the rotation is swi" +
                "tched from Slow to Fast.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Adjusts the rate at which the tweeter rotation speeds up when the rotation is swi" +
                "tched from Fast to Slow.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Volume of the tweeter.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Sets the rotary speaker stereo image.\r\n" +
                "\r\n" +
                "The higher the value set, the wider the sound is spread out.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Gain of the low range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Gain of the high range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "20: Rotary 2\r\n" +
                "\r\n" +
                "This type provides modified response for the rotary speaker, with the low end bo" +
                "osted further.\r\n" +
                "\r\n" +
                "This effect features the same specifications as the VK-7’s built-in rotary speak" +
                "er.\r\n", "_MFX_Rotary2.png",
                "Output level.\r\n", "", 0x4550);
            // 21: Rotary 3 speed - woofer
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Rotational speed of the rotating speaker.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Switches the rotation of the rotary speaker.\r\n" +
                "\r\n" +
                "When this is turned on, the rotation will gradually stop.\r\n" +
                "\r\n" +
                "When it is turned off, the rotation will gradually resume.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Overdrive on/off.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Overdrive input level.\r\n" +
                "\r\n" +
                "Higher values will increase the distortion.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Degree of distortion.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Volume of the overdrive.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Low-speed rotation speed of the woofer.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "High-speed rotation speed of the woofer.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Adjusts the rate at which the woofer rotation speeds up when the rotation is swi" +
                "tched from Slow to Fast.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Adjusts the rate at which the woofer rotation speeds up when the rotation is swi" +
                "tched from Fast to Slow.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Volume of the woofer.\r\n", "", 0x4550);
            // 21: Rotary 3 tweeter - level
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Low-speed rotation speed of the tweeter.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "High-speed rotation speed of the tweeter.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Adjusts the rate at which the tweeter rotation speeds up when the rotation is swi" +
                "tched from Slow to Fast.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Adjusts the rate at which the tweeter rotation speeds up when the rotation is swi" +
                "tched from Fast to Slow.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Volume of the tweeter.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Sets the rotary speaker stereo image.\r\n" +
                "\r\n" +
                "The higher the value set, the wider the sound is spread out.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Gain of the low range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Gain of the high range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "21: Rotary 3\r\n" +
                "\r\n" +
                "This type includes an overdrive. By distorting the sound you can produce the int" +
                "ense organ sound used in hard rock.\r\n", "_MFX_Rotary3.png",
                "Output volume.\r\n", "", 0x4550);
            // 22: Chorus
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex, 0, "22: Chorus\r\n" +
               "\r\n" +
               "This is a stereo chorus.\r\n" +
               "\r\n" +
               "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
               "Type of filter.\r\n" +
               "\r\n" +
               "OFF: no filter is used.\r\n", "_PCM_TVF_00.png", 0x4433);
            Add(5, PageIndex, (byte)ItemIndex, 1, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "LPF: cuts the frequency range above the Cutoff Freq.\r\n", "_PCM_TVF_01.png", 0x4433);
            Add(5, PageIndex, (byte)ItemIndex++, 2, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "HPF: cuts the frequency range below the Cutoff Freq.\r\n", "_PCM_TVF_03.png", 0x4433);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Basic frequency of the filter.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Adjusts the delay time from the direct sound until the chorus sound is heard.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Select Hertz or note length for setting frequency of modulation.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Frequency of modulation as note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Depth of modulation.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Spatial spread of the sound.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Gain of the low range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Gain of the high range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Volume balance between the direct sound (D) and the chorus sound (W).\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "22: Chorus\r\n" +
                "\r\n" +
                "This is a stereo chorus.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the chorus sound.\r\n", "_MFX_Chorus.png",
                "Output level.\r\n", "", 0x4550);
            // 23: Flanger
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "OFF: no filter is used.\r\n", "_PCM_TVF_00.png", 0x4433);

            Add(5, PageIndex, (byte)ItemIndex, 1, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "LPF: cuts the frequency range above the Cutoff Freq.\r\n", "_PCM_TVF_01.png", 0x4433);

            Add(5, PageIndex, (byte)ItemIndex++, 2, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "HPF: cuts the frequency range below the Cutoff Freq.\r\n", "_PCM_TVF_03.png", 0x4433);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Basic frequency of the filter.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Adjusts the delay time from when the direct sound begins until the flanger sound" +
                " is heard.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Select Hertz or note length for setting frequency of modulation.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Frequency of modulation as note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Depth of modulation.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Spatial spread of the sound.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Adjusts the proportion of the flanger sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Gain of the low range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Gain of the high range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Volume balance between the direct sound (D) and the flanger sound (W).\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "23: Flanger\r\n" +
                "\r\n" +
                "This is a stereo flanger. (The LFO has the same phase for left and right.) It pr" +
                "oduces a metallic resonance that rises and falls like a jet airplane t" +
                "aking off or landing.\r\n" +
                "\r\n" +
                "A filter is provided so that you can adjust the timbre of the flanged sound.\r\n", "_MFX_Flanger.png",
                "Output level.\r\n", "", 0x4550);
            // 24: Step Flanger
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "OFF: no filter is used.\r\n", "_PCM_TVF_00.png", 0x4433);
            Add(5, PageIndex, (byte)ItemIndex, 1, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "LPF: cuts the frequency range above the Cutoff Freq.\r\n", "_PCM_TVF_01.png", 0x4433);
            Add(5, PageIndex, (byte)ItemIndex++, 2, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "HPF: cuts the frequency range below the Cutoff Freq.\r\n", "_PCM_TVF_03.png", 0x4433);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Basic frequency of the filter.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Adjusts the delay time from when the direct sound begins until the flanger sound" +
                " is heard.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Select Hertz or note length for setting frequency of modulation.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Frequency of modulation as note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Depth of modulation.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Spatial spread of the sound.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Adjusts the proportion of the flanger sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Select Hertz or note length for setting rate (period) of pitch change.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Rate (period) of pitch change as Hertz.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Rate (period) of pitch change in note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Gain of the low range\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Gain of the high range\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Volume balance between the direct sound (D) and the flanger sound (W).\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "24: Step Flanger\r\n" +
                "\r\n" +
                "This is a flanger in which the flanger pitch changes in steps.\r\n" +
                "\r\n" +
                "The speed at which the pitch changes can also be specified in terms of a note-va" +
                "lue of a specified tempo.\r\n", "_MFX_StepFlanger.png",
                "Ouptut level\r\n", "", 0x4550);
            // 25: Hexa-Chorus
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "25: Hexa-Chorus\r\n" +
                "\r\n" +
                "Uses a six-phase chorus (six layers of chorused sound) to give richness and spat" +
                "ial spread to the sound.\r\n", "_MFX_HexaChorus.png",
                "Adjusts the delay time from the direct sound until the chorus sound is heard.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "25: Hexa-Chorus\r\n" +
                "\r\n" +
                "Uses a six-phase chorus (six layers of chorused sound) to give richness and spat" +
                "ial spread to the sound.\r\n", "_MFX_HexaChorus.png",
                "Select Hertz or note length to set frequency of modulation.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "25: Hexa-Chorus\r\n" +
                "\r\n" +
                "Uses a six-phase chorus (six layers of chorused sound) to give richness and spat" +
                "ial spread to the sound.\r\n", "_MFX_HexaChorus.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "25: Hexa-Chorus\r\n" +
                "\r\n" +
                "Uses a six-phase chorus (six layers of chorused sound) to give richness and spat" +
                "ial spread to the sound.\r\n", "_MFX_HexaChorus.png",
                "Frequency of modulation as note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "25: Hexa-Chorus\r\n" +
                "\r\n" +
                "Uses a six-phase chorus (six layers of chorused sound) to give richness and spat" +
                "ial spread to the sound.\r\n", "_MFX_HexaChorus.png",
                "Depth of modulation.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "25: Hexa-Chorus\r\n" +
                "\r\n" +
                "Uses a six-phase chorus (six layers of chorused sound) to give richness and spat" +
                "ial spread to the sound.\r\n", "_MFX_HexaChorus.png",
                "Adjusts the differences in Pre Delay between each chorus sound.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "25: Hexa-Chorus\r\n" +
                "\r\n" +
                "Uses a six-phase chorus (six layers of chorused sound) to give richness and spat" +
                "ial spread to the sound.\r\n", "_MFX_HexaChorus.png",
                "Adjusts the difference in modulation depth between each chorus sound.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "25: Hexa-Chorus\r\n" +
                "\r\n" +
                "Uses a six-phase chorus (six layers of chorused sound) to give richness and spat" +
                "ial spread to the sound.\r\n", "_MFX_HexaChorus.png",
                "Adjusts the difference in stereo location between each chorus sound.\r\n" +
                "\r\n" +
                "0: All chorus sounds will be in the center.\r\n" +
                "\r\n" +
                "20: Each chorus sound will be spaced at 60 degree intervals relative to the cent" +
                "er.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "25: Hexa-Chorus\r\n" +
                "\r\n" +
                "Uses a six-phase chorus (six layers of chorused sound) to give richness and spat" +
                "ial spread to the sound.\r\n", "_MFX_HexaChorus.png",
                "Volume balance between the direct sound (D) and the chorus sound (W).\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "25: Hexa-Chorus\r\n" +
                "\r\n" +
                "Uses a six-phase chorus (six layers of chorused sound) to give richness and spat" +
                "ial spread to the sound.\r\n", "_MFX_HexaChorus.png",
                "Output Level.\r\n", "", 0x4550);
            // 26: Tremolo Chorus
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "26: Tremolo Chorus\r\n" +
                "\r\n" +
                "This is a chorus effect with added Tremolo (cyclic modulation of volume).\r\n", "_MFX_TremoloChorus.png",
                "Adjusts the delay time from the direct sound until the chorus sound is heard.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "26: Tremolo Chorus\r\n" +
                "\r\n" +
                "This is a chorus effect with added Tremolo (cyclic modulation of volume).\r\n", "_MFX_TremoloChorus.png",
                "Select Hertz or note length to set modulation frequency of the chorus effect.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "26: Tremolo Chorus\r\n" +
                "\r\n" +
                "This is a chorus effect with added Tremolo (cyclic modulation of volume).\r\n", "_MFX_TremoloChorus.png",
                "Modulation frequency of the chorus effect in Hertz.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "26: Tremolo Chorus\r\n" +
                "\r\n" +
                "This is a chorus effect with added Tremolo (cyclic modulation of volume).\r\n", "_MFX_TremoloChorus.png",
                "Modulation frequency of the chorus effect as note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "26: Tremolo Chorus\r\n" +
                "\r\n" +
                "This is a chorus effect with added Tremolo (cyclic modulation of volume).\r\n", "_MFX_TremoloChorus.png",
                "Modulation depth of the chorus effect.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "26: Tremolo Chorus\r\n" +
                "\r\n" +
                "This is a chorus effect with added Tremolo (cyclic modulation of volume).\r\n", "_MFX_TremoloChorus.png",
                "Select Hertz or note length to set modulation frequency of the tremolo effect.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "26: Tremolo Chorus\r\n" +
                "\r\n" +
                "This is a chorus effect with added Tremolo (cyclic modulation of volume).\r\n", "_MFX_TremoloChorus.png",
                "Modulation frequency of the tremolo effect in Hertz.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "26: Tremolo Chorus\r\n" +
                "\r\n" +
                "This is a chorus effect with added Tremolo (cyclic modulation of volume).\r\n", "_MFX_TremoloChorus.png",
                "Modulation frequency of the tremolo effect as note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "26: Tremolo Chorus\r\n" +
                "\r\n" +
                "This is a chorus effect with added Tremolo (cyclic modulation of volume).\r\n", "_MFX_TremoloChorus.png",
                "Spread of the tremolo effect.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "26: Tremolo Chorus\r\n" +
                "\r\n" +
                "This is a chorus effect with added Tremolo (cyclic modulation of volume).\r\n", "_MFX_TremoloChorus.png",
                "Phase of the tremolo effect.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "26: Tremolo Chorus\r\n" +
                "\r\n" +
                "This is a chorus effect with added Tremolo (cyclic modulation of volume).\r\n", "_MFX_TremoloChorus.png",
                "Volume balance between the direct sound (D) and the tremolo chorus sound (W).\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "26: Tremolo Chorus\r\n" +
                "\r\n" +
                "This is a chorus effect with added Tremolo (cyclic modulation of volume).\r\n", "_MFX_TremoloChorus.png",
                "Output Level.\r\n", "", 0x4550);
            // 27: Space-D
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "27: Space-D\r\n" +
                "\r\n" +
                "This is a multiple chorus that applies two-phase modulation in stereo.\r\n" +
                "\r\n" +
                "It gives no impression of modulation, but produces a transparent chorus effect.\r\n", "_MFX_SpaceD.png",
                "Adjusts the delay time from the direct sound until the chorus sound is heard.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "27: Space-D\r\n" +
                "\r\n" +
                "This is a multiple chorus that applies two-phase modulation in stereo.\r\n" +
                "\r\n" +
                "It gives no impression of modulation, but produces a transparent chorus effect.\r\n", "_MFX_SpaceD.png",
                "Select Hertz or note length to set frequency of modulation.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "27: Space-D\r\n" +
                "\r\n" +
                "This is a multiple chorus that applies two-phase modulation in stereo.\r\n" +
                "\r\n" +
                "It gives no impression of modulation, but produces a transparent chorus effect.\r\n", "_MFX_SpaceD.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "27: Space-D\r\n" +
                "\r\n" +
                "This is a multiple chorus that applies two-phase modulation in stereo.\r\n" +
                "\r\n" +
                "It gives no impression of modulation, but produces a transparent chorus effect.\r\n", "_MFX_SpaceD.png",
                "Frequency of modulation as note length.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "27: Space-D\r\n" +
                "\r\n" +
                "This is a multiple chorus that applies two-phase modulation in stereo.\r\n" +
                "\r\n" +
                "It gives no impression of modulation, but produces a transparent chorus effect.\r\n", "_MFX_SpaceD.png",
                "Depth of modulation.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "27: Space-D\r\n" +
                "\r\n" +
                "This is a multiple chorus that applies two-phase modulation in stereo.\r\n" +
                "\r\n" +
                "It gives no impression of modulation, but produces a transparent chorus effect.\r\n", "_MFX_SpaceD.png",
                "Spatial spread of the sound.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "27: Space-D\r\n" +
                "\r\n" +
                "This is a multiple chorus that applies two-phase modulation in stereo.\r\n" +
                "\r\n" +
                "It gives no impression of modulation, but produces a transparent chorus effect.\r\n", "_MFX_SpaceD.png",
                "Gain of the low range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "27: Space-D\r\n" +
                "\r\n" +
                "This is a multiple chorus that applies two-phase modulation in stereo.\r\n" +
                "\r\n" +
                "It gives no impression of modulation, but produces a transparent chorus effect.\r\n", "_MFX_SpaceD.png",
                "Gain of the high range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "27: Space-D\r\n" +
                "\r\n" +
                "This is a multiple chorus that applies two-phase modulation in stereo.\r\n" +
                "\r\n" +
                "It gives no impression of modulation, but produces a transparent chorus effect.\r\n", "_MFX_SpaceD.png",
                "Volume balance between the direct sound (D) and the chorus sound (W).\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "27: Space-D\r\n" +
                "\r\n" +
                "This is a multiple chorus that applies two-phase modulation in stereo.\r\n" +
                "\r\n" +
                "It gives no impression of modulation, but produces a transparent chorus effect.\r\n", "_MFX_SpaceD.png",
                "Output level.\r\n", "", 0x4550);
            // 28: Overdrive
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "28: Overdrive\r\n" +
                "\r\n" +
                "This is an overdrive that provides heavy distortion.\r\n", "_MFX_Overdrive.png",
                "Degree of distortion.\r\n" +
                "\r\n" +
                "Also changes the volume.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "28: Overdrive\r\n" +
                "\r\n" +
                "This is an overdrive that provides heavy distortion.\r\n", "_MFX_Overdrive.png",
                "Sound quality of the Overdrive effect.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "28: Overdrive\r\n" +
                "\r\n" +
                "This is an overdrive that provides heavy distortion.\r\n", "_MFX_Overdrive.png",
                "Turns the Amp Simulator on/off.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "28: Overdrive\r\n" +
                "\r\n" +
                "This is an overdrive that provides heavy distortion.\r\n", "_MFX_Overdrive.png",
                "Type of guitar amp.\r\n" +
                "\r\n" +
                "SMALL: small amp.\r\n" +
                "BUILT-IN: single-unit type amp.\r\n" +
                "2-STACK: large double stack amp.\r\n" +
                "3-STACK: large triple stack amp.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "28: Overdrive\r\n" +
                "\r\n" +
                "This is an overdrive that provides heavy distortion.\r\n", "_MFX_Overdrive.png",
                "Gain of the low range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "28: Overdrive\r\n" +
                "\r\n" +
                "This is an overdrive that provides heavy distortion.\r\n", "_MFX_Overdrive.png",
                "Gain of the high range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "28: Overdrive\r\n" +
               "\r\n" +
               "This is an overdrive that provides heavy distortion.\r\n", "_MFX_Overdrive.png",
               "Stereo location of the output sound.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "28: Overdrive\r\n" +
                "\r\n" +
                "This is an overdrive that provides heavy distortion.\r\n", "_MFX_Overdrive.png",
                "Output level.\r\n", "", 0x4550);
            // 29: Distortion
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "29: Distortion\r\n" +
                "\r\n" +
                "This is a distortion effect that provides heavy distortion.\r\n", "_MFX_Distorsion.png",
                "Degree of distortion.\r\n" +
                "\r\n" +
                "Also changes the volume.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "29: Distortion\r\n" +
                "\r\n" +
                "This is a distortion effect that provides heavy distortion.\r\n", "_MFX_Distorsion.png",
                "Sound quality of the Overdrive effect.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "29: Distortion\r\n" +
                "\r\n" +
                "This is a distortion effect that provides heavy distortion.\r\n", "_MFX_Distorsion.png",
                "Turns the Amp Simulator on/off.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "29: Distortion\r\n" +
                "\r\n" +
                "This is a distortion effect that provides heavy distortion.\r\n", "_MFX_Distorsion.png",
                "Type of guitar amp.\r\n" +
                "\r\n" +
                "SMALL: small amp.\r\n" +
                "BUILT-IN: single-unit type amp.\r\n" +
                "2-STACK: large double stack amp.\r\n" +
                "3-STACK: large triple stack amp.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "29: Distortion\r\n" +
                "\r\n" +
                "This is a distortion effect that provides heavy distortion.\r\n", "_MFX_Distorsion.png",
                "Gain of the low range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "29: Distortion\r\n" +
                "\r\n" +
                "This is a distortion effect that provides heavy distortion.\r\n", "_MFX_Distorsion.png",
                "Gain of the high range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "29: Distortion\r\n" +
                "\r\n" +
                "This is a distortion effect that provides heavy distortion.\r\n", "_MFX_Distorsion.png",
               "Stereo location of the output sound.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "29: Distortion\r\n" +
                "\r\n" +
                "This is a distortion effect that provides heavy distortion.\r\n", "_MFX_Distorsion.png",
                "Output level.\r\n", "", 0x4550);
            // 30: Guitar Amp Simulator amp
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Turns the amp switch on/off.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Type of guitar amp.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Volume and amount of distortion of the amp.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Volume of the entire pre-amp.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Amount of pre-amp distortion.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Tone of the bass frequency range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Tone of the middle frequency range.\r\n" +
                "\r\n" +
                "* Middle has no effect if “Match Drive” is selected as the Pre Amp Type.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Tone of the treble frequency range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Tone for the ultra-high frequency range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Turning this “On” produces a sharper and brighter sound.\r\n" +
                "\r\n" +
                "* This parameter applies to the “JC-120,” “Clean Twin,” and “BG Lead” Pre Amp Ty" +
                "pes.\r\n", "", 0x4550);
            // 30: Guitar Amp Simulator mic and speakers
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Determines whether the signal passes through the speaker (ON), or not (OFF).\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Type of speaker.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Adjusts the location of the microphone that’s capturing the sound of the speaker" +
                ".\r\n" +
                "\r\n" +
                "This can be adjusted in three steps, from 1 to 3, with the microphone becoming m" +
                "ore distant as the value increases.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Volume of the microphone.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Volume of the direct sound.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Stereo location of the output.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "30: Guitar Amp Simulator\r\n" +
                "\r\n" +
                "This is an effect that simulates the sound of a guitar amplifier.\r\n", "_MFX_GuitarAmpSimulator.png",
                "Output level.\r\n", "", 0x4550);
            // 31: Compressor
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "31: Compressor\r\n" +
               "\r\n" +
               "Flattens out high levels and boosts low levels, smoothing out fluctuations in vo" +
               "lume.\r\n", "_MFX_Compressor.png",
               "Sets the speed at which compression starts.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "31: Compressor\r\n" +
                "\r\n" +
                "Flattens out high levels and boosts low levels, smoothing out fluctuations in vo" +
                "lume.\r\n", "_MFX_Compressor.png",
                "Adjusts the volume at which compression begins.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "31: Compressor\r\n" +
                "\r\n" +
                "Flattens out high levels and boosts low levels, smoothing out fluctuations in vo" +
                "lume.\r\n", "_MFX_Compressor.png",
                "Adjusts the output gain.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "31: Compressor\r\n" +
                "\r\n" +
                "Flattens out high levels and boosts low levels, smoothing out fluctuations in vo" +
                "lume.\r\n", "_MFX_Compressor.png",
                "Gain of the low frequency range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "31: Compressor\r\n" +
                "\r\n" +
                "Flattens out high levels and boosts low levels, smoothing out fluctuations in vo" +
                "lume.\r\n", "_MFX_Compressor.png",
                "Gain of the high frequency range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "31: Compressor\r\n" +
                "\r\n" +
                "Flattens out high levels and boosts low levels, smoothing out fluctuations in vo" +
                "lume.\r\n", "_MFX_Compressor.png",
                "Output level.\r\n", "", 0x4550);
            // 32: Limiter
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "32: Limiter\r\n" +
                "\r\n" +
                "Compresses signals that exceed a specified volume level, preventing distortion f" +
                "rom occurring.\r\n", "_MFX_Limiter.png",
                "Adjusts the time after the signal volume falls below the Threshold Level until c" +
                "ompression is no longer applied.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "32: Limiter\r\n" +
                "\r\n" +
                "Compresses signals that exceed a specified volume level, preventing distortion f" +
                "rom occurring.\r\n", "_MFX_Limiter.png",
                "Adjusts the volume at which compression begins.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "32: Limiter\r\n" +
                "\r\n" +
                "Compresses signals that exceed a specified volume level, preventing distortion f" +
                "rom occurring.\r\n", "_MFX_Limiter.png",
                "Compression ratio.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "32: Limiter\r\n" +
                "\r\n" +
                "Compresses signals that exceed a specified volume level, preventing distortion f" +
                "rom occurring.\r\n", "_MFX_Limiter.png",
                "Adjusts the output gain.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "32: Limiter\r\n" +
                "\r\n" +
                "Compresses signals that exceed a specified volume level, preventing distortion f" +
                "rom occurring.\r\n", "_MFX_Limiter.png",
                "Gain of the low frequency range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "32: Limiter\r\n" +
                "\r\n" +
                "Compresses signals that exceed a specified volume level, preventing distortion f" +
                "rom occurring.\r\n", "_MFX_Limiter.png",
                "Gain of the high frequency range.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "32: Limiter\r\n" +
                "\r\n" +
                "Compresses signals that exceed a specified volume level, preventing distortion f" +
                "rom occurring.\r\n", "_MFX_Limiter.png",
                "Output level.\r\n", "", 0x4550);
            // 33: Gate
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "33: Gate\r\n" +
                "\r\n" +
                "Cuts the reverb’s delay according to the volume of the sound sent into the effec" +
                "t.\r\n" +
                "\r\n" +
                "Use this when you want to create an artificialsounding decrease in the reverb’s " +
                "decay.\r\n", "_MFX_Gate.png",
                "Volume level at which the gate begins to close.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "33: Gate\r\n" +
                "\r\n" +
                "Cuts the reverb’s delay according to the volume of the sound sent into the effec" +
                "t.\r\n" +
                "\r\n" +
                "Use this when you want to create an artificialsounding decrease in the reverb’s " +
                "decay.\r\n", "_MFX_Gate.png",
                "Type of gate.\r\n" +
                "\r\n" +
                "GATE: The gate will close when the volume of the original sound decreases, cutti" +
                "ng the original sound.\r\n" +
                "\r\n" +
                "DUCK (Ducking): The gate will close when the volume of the original sound increa" +
                "ses, cutting the original sound.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "33: Gate\r\n" +
                "\r\n" +
                "Cuts the reverb’s delay according to the volume of the sound sent into the effec" +
                "t.\r\n" +
                "\r\n" +
                "Use this when you want to create an artificialsounding decrease in the reverb’s " +
                "decay.\r\n", "_MFX_Gate.png",
                "Adjusts the time it takes for the gate to fully open after being triggered.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "33: Gate\r\n" +
                "\r\n" +
                "Cuts the reverb’s delay according to the volume of the sound sent into the effec" +
                "t.\r\n" +
                "\r\n" +
                "Use this when you want to create an artificialsounding decrease in the reverb’s " +
                "decay.\r\n", "_MFX_Gate.png",
                "Adjusts the time it takes for the gate to start closing after the source sound f" +
                "alls beneath the Threshold.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "33: Gate\r\n" +
                "\r\n" +
                "Cuts the reverb’s delay according to the volume of the sound sent into the effec" +
                "t.\r\n" +
                "\r\n" +
                "Use this when you want to create an artificialsounding decrease in the reverb’s " +
                "decay.\r\n", "_MFX_Gate.png",
                "Adjusts the time it takes the gate to fully close after the hold time.\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "33: Gate\r\n" +
                "\r\n" +
                "Cuts the reverb’s delay according to the volume of the sound sent into the effec" +
                "t.\r\n" +
                "\r\n" +
                "Use this when you want to create an artificialsounding decrease in the reverb’s " +
                "decay.\r\n", "_MFX_Gate.png",
                "Volume balance between the direct sound (D) and the effect sound (W).\r\n", "", 0x4550);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "33: Gate\r\n" +
                "\r\n" +
                "Cuts the reverb’s delay according to the volume of the sound sent into the effec" +
                "t.\r\n" +
                "\r\n" +
                "Use this when you want to create an artificialsounding decrease in the reverb’s " +
                "decay.\r\n", "_MFX_Gate.png",
                "Output Level.\r\n", "", 0x4550);
            // 34: Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Select milliseconds or note length to adjust the time until the left side delay sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Adjusts the time until the left side delay sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Adjusts the time until the left side delay sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Select milliseconds or note length to adjust the time until the right side delay sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Adjusts the time until the right side delay sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Adjusts the time until the right side delay sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Phase of the left side delay sound.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Phase of the right side delay sound.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Selects the way in which delay sound is fed back into the effect. (See the schem" +
                "atics above.)\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Adjusts the amount of the delay sound that’s fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings invert the phase.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Adjusts the frequency above which sound fed back to the effect is filtered out.\r\n" +
                "\r\n" +
                "If you don’t want to filter out any high frequencies, set this parameter to BYPA" +
                "SS.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Gain of the low frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Gain of the high frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Volume balance between the direct sound (D) and the delay sound (W).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "34: Delay\r\n" +
                "\r\n" +
                "This is a stereo delay.\r\n", "_MFX_Delay.png",
                "Output level.\r\n", "", 0x4830);
            // 35: Modulation Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Select milliseconds or note length for adjusting the time until the delay left sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Adjusts the time until the delay left sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Adjusts the time until the delay left sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Select milliseconds or note length for adjusting the time until the delay right sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Adjusts the time until the delay right sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Adjusts the time until the delay right sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Selects the way in which delay sound is fed back into the effect (See the image" +
                "s above.)\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Adjusts the amount of the delay sound that’s fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings invert the phase.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Adjusts the frequency above which sound fed back to the effect is filtered out.\r\n" +
                "\r\n" +
                "If you don’t want to filter out any high frequencies, set this parameter to BYPA" +
                "SS.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Select Hertz or note length for setting frequency of modulation.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Frequency of modulation as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Depth of modulation.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Spatial spread of the sound.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Gain of the low frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Gain of the high frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Volume balance between the direct sound (D) and the delay sound (W).\r\n", "", 0x4830);

            Add(5, PageIndex, (byte)ItemIndex++, 0, "35: Modulation Delay\r\n" +
                "\r\n" +
                "Adds modulation to the delayed sound.\r\n", "_MFX_ModulationDelay.png",
                "Output level.\r\n", "", 0x4830);
            // 36: 3Tap Pan Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Select milliseconds or note length for adjusting the time until the left side delay sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Adjusts the time until the left side delay sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Adjusts the time until the left side delay sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Select milliseconds or note length for adjusting the time until the right side delay sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Adjusts the time until the right side delay sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Adjusts the time until the right side delay sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Select milliseconds or note length for adjusting the time until the center delay sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Adjusts the time until the center delay sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Adjusts the time until the center delay sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Adjusts the amount of the delay sound that’s fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings invert the phase.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Adjusts the frequency above which sound fed back to the effect is filtered out.\r\n" +
                "\r\n" +
                "If you do not want to filter out any high frequencies, set this parameter to BYP" +
                "ASS.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Volume of left side delay.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Volume of right side delay.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Volume of center delay.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Gain of the low frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Gain of the high frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Volume balance between the direct sound (D) and the delay sound (W).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "36: 3Tap Pan Delay\r\n" +
                "\r\n" +
                "Produces three delay sounds; center, left and right.\r\n", "_MFX_3TapPanDelay.png",
                "Output level.\r\n", "", 0x4830);
            // 37: 4Tap Pan Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Select milliseconds or note length for adjusting the time until the delay 1 sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Adjusts the time until the delay 1 sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Adjusts the time until the delay 1 sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Select milliseconds or note length for adjusting the time until the delay 2 sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Adjusts the time until the delay 2 sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Adjusts the time until the delay 2 sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Select milliseconds or note length for adjusting the time until the delay 3 sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Adjusts the time until the delay 3 sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Adjusts the time until the delay 3 sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Select milliseconds or note length for adjusting the time until the delay 4 sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Adjusts the time until the delay 4 sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Adjusts the time until the delay 4 sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Adjusts the amount of the delay sound that’s fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings invert the phase.\r\n", "", 0x4830);
            // 37: 4Tap Pan Delay levels
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Adjusts the frequency above which sound fed back to the effect is filtered out.\r\n" +
                "\r\n" +
                "If you do not want to filter out any high frequencies, set this parameter to BYP" +
                "ASS.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Volume of delay 1.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Volume of delay 2.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Volume of delay 3.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Volume of delay 4.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Gain of the low frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Gain of the high frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Volume balance between the direct sound (D) and the delay sound (W).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "37: 4Tap Pan Delay\r\n" +
                "\r\n" +
                "This effect has four delays.\r\n", "_MFX_4TapPanDelay.png",
                "Output level.\r\n", "", 0x4830);
            // 38: Multi Tap Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Select milliseconds or note length for adjusting the time until delay 1 is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Adjusts the time until delay 1 is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Adjusts the time until delay 1 is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Select milliseconds or note length for adjusting the time until delay 2 is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Adjusts the time until delay 2 is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Adjusts the time until delay 2 is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Select milliseconds or note length for adjusting the time until delay 3 is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Adjusts the time until delay 3 is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Adjusts the time until delay 3 is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Select milliseconds or note length for adjusting the time until delay 4 is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Adjusts the time until delay 4 is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Adjusts the time until delay 4 is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Adjusts the amount of the delay sound that’s fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings invert the phase.\r\n", "", 0x4830);
            // 38: Multi Tap Delay levels
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Adjusts the frequency above which sound fed back to the effect is filtered out.\r\n" +
                "\r\n" +
                "If you don’t want to filter out any the high frequencies, set this parameter to " +
                "BYPASS.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Stereo location of delay 1.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Stereo location of delay 2.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Stereo location of delay 3.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Stereo location of delay 4.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Output level of delay 1.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Output level of delay 2.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Output level of delay 3.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Output level of delay 4.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Gain of the low frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Gain of the high frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Volume balance between the direct sound (D) and the effect sound (W).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "38: Multi Tap Delay\r\n" +
                "\r\n" +
                "This effect provides four delays. Each of the Delay Time parameters can be set t" +
                "o a note length based on the selected tempo.\r\n" +
                "\r\n" +
                "You can also set the panning and level of each delay sound.\r\n", "_MFX_MultiTapDelay.png",
                "Output level.\r\n", "", 0x4830);
            // 39: Reverse Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Volume at which the reverse delay will begin to be applied.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Select milliseconds or note length for setting delay time from when sound is input into the reverse delay until the delay sound" +
                " is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Delay time from when sound is input into the reverse delay until the delay sound" +
                " is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Delay time from when sound is input into the reverse delay until the delay sound" +
                " is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Proportion of the delay sound that is to be returned to the input of the reverse" +
                " delay (negative values invert the phase).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Frequency at which the high-frequency content of the reverse-delayed sound will " +
                "be cut (BYPASS: no cut).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Panning of the reverse delay sound.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Volume of the reverse delay sound.\r\n", "", 0x4830);
            // 39: Reverse Delay delays
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Select milliseconds or note length for setting delay time from when sound is input into the tap delay until the delay 1 sound is" +
                " heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Delay time from when sound is input into the tap delay until the delay 1 sound is" +
                " heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Delay time from when sound is input into the tap delay until the delay 1 sound is" +
                " heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Select milliseconds or note length for setting delay time from when sound is input into the tap delay until the delay 2 sound is" +
                " heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Delay time from when sound is input into the tap delay until the delay 2 sound is" +
                " heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Delay time from when sound is input into the tap delay until the delay 2 sound is" +
                " heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Select milliseconds or note length for setting delay time from when sound is input into the tap delay until the delay 3 sound is" +
                " heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Delay time from when sound is input into the tap delay until the delay 3 sound is" +
                " heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Delay time from when sound is input into the tap delay until the delay 3 sound is" +
                " heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Proportion of the delay sound that is to be returned to the input of the tap del" +
                "ay (negative values invert the phase).\r\n", "", 0x4830);
            // 39: Reverse Delay delays
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Frequency at which the lowfrequency content of the tap delay sound will be cut (" +
                "BYPASS: no cut).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Panning of the tap delay sound 1.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Panning of the tap delay sound 2.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Volume of the tap delay sound 1.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Volume of the tap delay sound 2.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Amount of boost/cut for the low-frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Amount of boost/cut for the high-frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Volume balance of the original sound (D) and delay sound (W).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "39: Reverse Delay\r\n" +
                "\r\n" +
                "This is a reverse delay that adds a reversed and delayed sound to the input soun" +
                "d.\r\n" +
                "\r\n" +
                "A tap delay is connected immediately after the reverse delay.\r\n", "_MFX_ReverseDelay.png",
                "Output level.\r\n", "", 0x4830);
            // 40: Time Ctrl Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "40: Time Ctrl Delay\r\n" +
                "\r\n" +
                "A stereo delay in which the delay time can be varied smoothly.\r\n", "_MFX_TimeCtrlDelay.png",
                "Select milliseconds of note lenght for adjusting the time until the delay is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "40: Time Ctrl Delay\r\n" +
                "\r\n" +
                "A stereo delay in which the delay time can be varied smoothly.\r\n", "_MFX_TimeCtrlDelay.png",
                "Adjusts the time until the delay is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "40: Time Ctrl Delay\r\n" +
                "\r\n" +
                "A stereo delay in which the delay time can be varied smoothly.\r\n", "_MFX_TimeCtrlDelay.png",
                "Adjusts the time until the delay is heard in note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "40: Time Ctrl Delay\r\n" +
                "\r\n" +
                "A stereo delay in which the delay time can be varied smoothly.\r\n", "_MFX_TimeCtrlDelay.png",
                "Adjusts the speed which the Delay Time changes from the current setting to a spe" +
                "cified new setting.\r\n" +
                "\r\n" +
                "The rate of change for the Delay Time directly affects the rate of pitch change.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "40: Time Ctrl Delay\r\n" +
                "\r\n" +
                "A stereo delay in which the delay time can be varied smoothly.\r\n", "_MFX_TimeCtrlDelay.png",
                "Adjusts the amount of the delay that’s fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings invert the phase.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "40: Time Ctrl Delay\r\n" +
                "\r\n" +
                "A stereo delay in which the delay time can be varied smoothly.\r\n", "_MFX_TimeCtrlDelay.png",
                "Adjusts the frequency above which sound fed back to the effect is filtered out.\r\n" +
                "\r\n" +
                "If you do not want to filter out any high frequencies, set this parameter to BYP" +
                "ASS.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "40: Time Ctrl Delay\r\n" +
                "\r\n" +
                "A stereo delay in which the delay time can be varied smoothly.\r\n", "_MFX_TimeCtrlDelay.png",
                "Gain of the low frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "40: Time Ctrl Delay\r\n" +
                "\r\n" +
                "A stereo delay in which the delay time can be varied smoothly.\r\n", "_MFX_TimeCtrlDelay.png",
                "Gain of the high frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "40: Time Ctrl Delay\r\n" +
                "\r\n" +
                "A stereo delay in which the delay time can be varied smoothly.\r\n", "_MFX_TimeCtrlDelay.png",
                "Volume balance between the direct sound (D) and the delay sound (W).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "40: Time Ctrl Delay\r\n" +
                "\r\n" +
                "A stereo delay in which the delay time can be varied smoothly.\r\n", "_MFX_TimeCtrlDelay.png",
                "Output level.\r\n", "", 0x4830);
            // 41: LOFI Compress
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "41: LOFI Compress\r\n" +
                "\r\n" +
                "This is an effect that intentionally degrades the sound quality for creative pur" +
                "poses.\r\n", "_MFX_LoFiCompress.png",
                "Selects the type of filter applied to the sound before it passes through the Lo-" +
                "Fi effect.\r\n" +
                "\r\n" +
                "1: Compressor off.\r\n" +
                "2–6: Compressor on.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "41: LOFI Compress\r\n" +
                "\r\n" +
                "This is an effect that intentionally degrades the sound quality for creative pur" +
                "poses.\r\n", "_MFX_LoFiCompress.png",
                "Degrades the sound quality.\r\n" +
                "\r\n" +
                "The sound quality grows poorer as this value is increased.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex, 0, "41: LOFI Compress\r\n" +
                "\r\n" +
                "This is an effect that intentionally degrades the sound quality for creative pur" +
                "poses.\r\n", "_MFX_LoFiCompress.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "OFF: no filter is used.\r\n", "_PCM_TVF_00.png", 0x4545);
            Add(5, PageIndex, (byte)ItemIndex, 0, "41: LOFI Compress\r\n" +
                "\r\n" +
                "This is an effect that intentionally degrades the sound quality for creative pur" +
                "poses.\r\n", "_MFX_LoFiCompress.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "LPF: cuts the frequency range above the Cutoff.\r\n", "_PCM_TVF_01.png", 0x4545);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "41: LOFI Compress\r\n" +
                "\r\n" +
                "This is an effect that intentionally degrades the sound quality for creative pur" +
                "poses.\r\n", "_MFX_LoFiCompress.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "HPF: cuts the frequency range below the Cutoff.\r\n", "_PCM_TVF_03.png", 0x4545);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "41: LOFI Compress\r\n" +
                "\r\n" +
                "This is an effect that intentionally degrades the sound quality for creative pur" +
                "poses.\r\n", "_MFX_LoFiCompress.png",
                "Basic frequency of the Post Filter.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "41: LOFI Compress\r\n" +
                "\r\n" +
                "This is an effect that intentionally degrades the sound quality for creative pur" +
                "poses.\r\n", "_MFX_LoFiCompress.png",
                "Gain of the low range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "41: LOFI Compress\r\n" +
                "\r\n" +
                "This is an effect that intentionally degrades the sound quality for creative pur" +
                "poses.\r\n", "_MFX_LoFiCompress.png",
                "Gain of the high range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "41: LOFI Compress\r\n" +
                "\r\n" +
                "This is an effect that intentionally degrades the sound quality for creative pur" +
                "poses.\r\n", "_MFX_LoFiCompress.png",
                "Volume balance between the direct sound (D) and the effect sound (W).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "41: LOFI Compress\r\n" +
                "\r\n" +
                "This is an effect that intentionally degrades the sound quality for creative pur" +
                "poses.\r\n", "_MFX_LoFiCompress.png",
                "Output level.\r\n", "", 0x4830);
            // 42: Bit Crasher
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "42: Bit Crasher\r\n" +
                "\r\n" +
                "This creates a lo-fi sound.\r\n", "_MFX_BitCrasher.png",
                "Adjusts the sample rate.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "42: Bit Crasher\r\n" +
                "\r\n" +
                "This creates a lo-fi sound.\r\n", "_MFX_BitCrasher.png",
                "Adjusts the bit depth.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "42: Bit Crasher\r\n" +
                "\r\n" +
                "This creates a lo-fi sound.\r\n", "_MFX_BitCrasher.png",
                "Adjusts the filter depth.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "42: Bit Crasher\r\n" +
                "\r\n" +
                "This creates a lo-fi sound.\r\n", "_MFX_BitCrasher.png",
                "Gain of the low frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "42: Bit Crasher\r\n" +
                "\r\n" +
                "This creates a lo-fi sound.\r\n", "_MFX_BitCrasher.png",
                "Gain of the high frequency range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "42: Bit Crasher\r\n" +
                "\r\n" +
                "This creates a lo-fi sound.\r\n", "_MFX_BitCrasher.png",
                "Output level.\r\n", "", 0x4830);
            // 43: Pitch Shifter
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "43: Pitch Shifter\r\n" +
                "\r\n" +
                "A stereo pitch shifter.\r\n", "_MFX_PitchShifter.png",
                "Adjusts the pitch of the pitch shifted sound in semitone steps.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "43: Pitch Shifter\r\n" +
                "\r\n" +
                "A stereo pitch shifter.\r\n", "_MFX_PitchShifter.png",
                "Adjusts the pitch of the pitch shifted sound in 2-cent steps.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "43: Pitch Shifter\r\n" +
                "\r\n" +
                "A stereo pitch shifter.\r\n", "_MFX_PitchShifter.png",
                "Select milliseconds or note length for adjusting the delay time from the direct sound until the pitch shifted sound is he" +
                "ard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "43: Pitch Shifter\r\n" +
                "\r\n" +
                "A stereo pitch shifter.\r\n", "_MFX_PitchShifter.png",
                "Adjusts the delay time from the direct sound until the pitch shifted sound is he" +
                "ard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "43: Pitch Shifter\r\n" +
                "\r\n" +
                "A stereo pitch shifter.\r\n", "_MFX_PitchShifter.png",
                "Adjusts the delay time from the direct sound until the pitch shifted sound is he" +
                "ard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "43: Pitch Shifter\r\n" +
                "\r\n" +
                "A stereo pitch shifter.\r\n", "_MFX_PitchShifter.png",
                "Adjusts the proportion of the pitch shifted sound that is fed back into the effe" +
                "ct.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "43: Pitch Shifter\r\n" +
                "\r\n" +
                "A stereo pitch shifter.\r\n", "_MFX_PitchShifter.png",
                "Gain of the low range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "43: Pitch Shifter\r\n" +
                "\r\n" +
                "A stereo pitch shifter.\r\n", "_MFX_PitchShifter.png",
                "Gain of the high range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "43: Pitch Shifter\r\n" +
               "\r\n" +
               "A stereo pitch shifter.\r\n", "_MFX_PitchShifter.png",
               "Volume balance between the direct sound (D) and the pitch shifted sound (W).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "43: Pitch Shifter\r\n" +
                "\r\n" +
                "A stereo pitch shifter.\r\n", "_MFX_PitchShifter.png",
                "Outpu level.\r\n", "", 0x4830);
            // 44: 2Voice Pitch Shifter
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Adjusts the pitch of Pitch Shift 1 in semitone steps.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Adjusts the pitch of Pitch Shift Pitch 1 in 2-cent steps.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Select milliseconds or note length for adjusting the delay time from the direct sound until the Pitch Shift 1 sound is he" +
                "ard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Adjusts the delay time from the direct sound until the Pitch Shift 1 sound is he" +
                "ard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Adjusts the delay time from the direct sound until the Pitch Shift 1 sound is he" +
                "ard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Adjusts the proportion of the pitch shifted sound 1 that is fed back into the effe" +
                "ct.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Stereo location of the Pitch Shift 1 sound.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Volume of the Pitch Shift 1 sound.\r\n", "", 0x4830);

            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Adjusts the pitch of Pitch Shift 2 in semitone steps.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Adjusts the pitch of Pitch Shift Pitch 2 in 2-cent steps.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Select milliseconds or note length for adjusting the delay time from the direct sound until the Pitch Shift 2 sound is he" +
                "ard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Adjusts the delay time from the direct sound until the Pitch Shift 2 sound is he" +
                "ard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Adjusts the delay time from the direct sound until the Pitch Shift 2 sound is he" +
                "ard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Adjusts the proportion of the pitch shifted sound 2 that is fed back into the effe" +
                "ct.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Stereo location of the Pitch Shift 2 sound.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Volume of the Pitch Shift 2 sound.\r\n", "", 0x4830);
            // 44: 2Voice Pitch Shifter levels
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Gain of the low range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Gain of the high range.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Volume balance between the direct sound (D) and the pitch shifted sound (W).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "44: 2Voice Pitch Shifter\r\n" +
                "\r\n" +
                "Shifts the pitch of the original sound.\r\n" +
                "\r\n" +
                "This 2-voice pitch shifter has two pitch shifters, and can add two pitch shifted" +
                " sounds to the original sound.\r\n", "_MFX_2VoicePitchShifter.png",
                "Output level.\r\n", "", 0x4830);
            // 45: Overdrive -> Chorus
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "45: Overdrive -> Chorus\r\n", "_MFX_OverdriveToChorus.png",
                "Degree of distortion.\r\n" +
                "\r\n" +
                "Also changes the volume.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "45: Overdrive -> Chorus\r\n", "_MFX_OverdriveToChorus.png",
                "Stereo location of the overdrive sound.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "45: Overdrive -> Chorus\r\n", "_MFX_OverdriveToChorus.png",
                "Adjusts the delay time from the direct sound until the chorus sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "45: Overdrive -> Chorus\r\n", "_MFX_OverdriveToChorus.png",
                "Select Hertz or note lenght to adjust frequency of modulation.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "45: Overdrive -> Chorus\r\n", "_MFX_OverdriveToChorus.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "45: Overdrive -> Chorus\r\n", "_MFX_OverdriveToChorus.png",
                "Frequency of modulation as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "45: Overdrive -> Chorus\r\n", "_MFX_OverdriveToChorus.png",
                "Depth of modulation.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "45: Overdrive -> Chorus\r\n", "_MFX_OverdriveToChorus.png",
                "Adjusts the volume balance between the sound that is sent through the chorus (W)" +
                " and the sound that is not sent through the chorus (D).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "45: Overdrive -> Chorus\r\n", "_MFX_OverdriveToChorus.png",
                "Output Level.\r\n", "", 0x4830);
            // 46: Overdrive -> Flanger
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "46: Overdrive -> Flanger\r\n", "_MFX_OverdriveToFlanger.png",
                "Degree of distortion.\r\n" +
                "\r\n" +
                "Also changes the volume.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "46: Overdrive -> Flanger\r\n", "_MFX_OverdriveToFlanger.png",
                "Stereo location of the overdrive sound.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "46: Overdrive -> Flanger\r\n", "_MFX_OverdriveToFlanger.png",
                "Adjusts the delay time from when the direct sound begins until the flanger sound" +
                " is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "46: Overdrive -> Flanger\r\n", "_MFX_OverdriveToFlanger.png",
                "Select Hertz of note length to set frequency of modulation.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "46: Overdrive -> Flanger\r\n", "_MFX_OverdriveToFlanger.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "46: Overdrive -> Flanger\r\n", "_MFX_OverdriveToFlanger.png",
                "Frequency of modulation as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "46: Overdrive -> Flanger\r\n", "_MFX_OverdriveToFlanger.png",
                "Depth of modulation.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "46: Overdrive -> Flanger\r\n", "_MFX_OverdriveToFlanger.png",
                "Adjusts the proportion of the flanger sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "46: Overdrive -> Flanger\r\n", "_MFX_OverdriveToFlanger.png",
                "Adjusts the volume balance between the sound that is sent through the flanger (W" +
                ") and the sound that is not sent through the flanger (D).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "46: Overdrive -> Flanger\r\n", "_MFX_OverdriveToFlanger.png",
                "Output Level.\r\n", "", 0x4830);
            // 47: Overdrive -> Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Overdrive -> Delay\r\n", "_MFX_OverdriveToDelay.png",
                "Degree of distortion.\r\n" +
                "\r\n" +
                "Also changes the volume.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Overdrive -> Delay\r\n", "_MFX_OverdriveToDelay.png",
                "Stereo location of the overdrive sound.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Overdrive -> Delay\r\n", "_MFX_OverdriveToDelay.png",
                "Select milliseconds or note length for adjusting the delay time from the direct sound until the delay sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Overdrive -> Delay\r\n", "_MFX_OverdriveToDelay.png",
                "Adjusts the delay time from the direct sound until the delay sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Overdrive -> Delay\r\n", "_MFX_OverdriveToDelay.png",
                "Adjusts the delay time from the direct sound until the delay sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Overdrive -> Delay\r\n", "_MFX_OverdriveToDelay.png",
                "Adjusts the proportion of the delay sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Overdrive -> Delay\r\n", "_MFX_OverdriveToDelay.png",
                "Adjusts the frequency above which sound fed back to the effect will be cut.\r\n" +
                "\r\n" +
                "If you do not want to cut the high frequencies, set this parameter to BYPASS.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Overdrive -> Delay\r\n", "_MFX_OverdriveToDelay.png",
                "Adjusts the volume balance between the sound that is sent through the delay (W) " +
                "and the sound that is not  sent through the delay (D).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Overdrive -> Delay\r\n", "_MFX_OverdriveToDelay.png",
                "Output Level.\r\n", "", 0x4830);
            // 48: Distortion -> Chorus
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Distortion -> Chorus\r\n", "_MFX_DistorsionToChorus.png",
                "Degree of distortion.\r\n" +
                "\r\n" +
                "Also changes the volume.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Distortion -> Chorus\r\n", "_MFX_DistorsionToChorus.png",
                "Stereo location of the overdrive sound.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Distortion -> Chorus\r\n", "_MFX_DistorsionToChorus.png",
                "Adjusts the delay time from when the direct sound begins until the chorus sound" +
                " is heard.", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "48: Distortion -> Chorus\r\n", "_MFX_DistorsionToChorus.png",
                "Select Hertz or note length for adjusting the frequency of modulation.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "48: Distortion -> Chorus\r\n", "_MFX_DistorsionToChorus.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "48: Distortion -> Chorus\r\n", "_MFX_DistorsionToChorus.png",
                "Frequency of modulation as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "48: Distortion -> Chorus\r\n", "_MFX_DistorsionToChorus.png",
                "Depth of modulation.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "47: Distortion -> Chorus\r\n", "_MFX_DistorsionToChorus.png",
                "Adjusts the volume balance between the sound that is sent through the delay (W) " +
                "and the sound that is not  sent through the delay (D).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "48: Distortion -> Chorus\r\n", "_MFX_DistorsionToChorus.png",
                "Output level.\r\n", "", 0x4830);
            // 49: Distortion -> Flanger
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "49: Distortion -> Flanger\r\n", "_MFX_DistortionToFlanger.png",
                "Degree of distortion.\r\n" +
                "\r\n" +
                "Also changes the volume.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "49: Distortion -> Flanger\r\n", "_MFX_DistortionToFlanger.png",
                "Stereo location of the distortion sound.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "49: Distortion -> Flanger\r\n", "_MFX_DistortionToFlanger.png",
                "Adjusts the delay time from when the direct sound begins until the flanger sound" +
                " is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "49: Distortion -> Flanger\r\n", "_MFX_DistortionToFlanger.png",
                "Select Hertz of note length to set frequency of modulation.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "49: Distortion -> Flanger\r\n", "_MFX_DistortionToFlanger.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "49: Distortion -> Flanger\r\n", "_MFX_DistortionToFlanger.png",
                "Frequency of modulation as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "49: Distortion -> Flanger\r\n", "_MFX_DistortionToFlanger.png",
                "Depth of modulation.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "49: Distortion -> Flanger\r\n", "_MFX_DistortionToFlanger.png",
                "Adjusts the proportion of the flanger sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "49: Distortion -> Flanger\r\n", "_MFX_DistortionToFlanger.png",
                "Adjusts the volume balance between the sound that is sent through the flanger (W" +
                ") and the sound that is not sent through the flanger (D).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "49: Distortion -> Flanger\r\n", "_MFX_DistortionToFlanger.png",
                "Output level.\r\n", "", 0x4830);
            // 50: Distortion -> Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "50: Distortion -> Delay\r\n", "_MFX_DistorsionToDelay.png",
                "Degree of distortion.\r\n" +
                "\r\n" +
                "Also changes the volume.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "50: Distortion -> Delay\r\n", "_MFX_DistorsionToDelay.png",
                "Stereo location of the distortion sound.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "50: Distortion -> Delay\r\n", "_MFX_DistorsionToDelay.png",
                "Select milliseconds or note length for adjusting the delay time from the direct sound until the delay sound is heard.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "50: Distortion -> Delay\r\n", "_MFX_DistorsionToDelay.png",
                "Adjusts the delay time from the direct sound until the delay sound is heard in milliseconds.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "50: Distortion -> Delay\r\n", "_MFX_DistorsionToDelay.png",
                "Adjusts the delay time from the direct sound until the delay sound is heard as note length.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "50: Distortion -> Delay\r\n", "_MFX_DistorsionToDelay.png",
                "Adjusts the proportion of the delay sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "50: Distortion -> Delay\r\n", "_MFX_DistorsionToDelay.png",
                "Adjusts the frequency above which sound fed back to the effect will be cut.\r\n" +
                "\r\n" +
                "If you do not want to cut the high frequencies, set this parameter to BYPASS.\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "50: Distortion -> Delay\r\n", "_MFX_DistorsionToDelay.png",
                "Adjusts the volume balance between the sound that is sent through the delay (W) " +
                "and the sound that is not  sent through the delay (D).\r\n", "", 0x4830);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "50: Distortion -> Delay\r\n", "_MFX_DistorsionToDelay.png",
                "Output level.\r\n", "", 0x4830);
            // 51: OD/DS -> TouchWah
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Turns overdrive/distortion on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Type of distortion.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Degree of distortion.\r\n" +
                "\r\n" +
                "Also changes the volume.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Sound quality of the Overdrive effect.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Turns the Amp Simulator on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Type of guitar amp.\r\n" +
                "\r\n" +
                "SMALL: small amp.\r\n" +
                "BUILT-IN: single-unit type amp.\r\n" +
                "2-STACK: large double stack amp.\r\n" +
                "3-STACK: large triple stack amp.\r\n", "", 0x2940);
            // 51: OD/DS -> TouchWah
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Touch Wah on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "LPF: Produces a wah effect in a broad frequency range.\r\n", "_PCM_TVF_01.png", 0x2544);
            Add(5, PageIndex, (byte)ItemIndex++, 1, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "BPF: Produces a wah effect in a narrow frequency range.\r\n", "_PCM_TVF_02.png", 0x2544);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Direction in which the filter will move.\r\n" +
                "\r\n" +
                "UP: Move toward a higher frequency.\r\n" +
                "DOWN: Move toward a lower frequency.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Sensitivity with which the filter is modified.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Center frequency at which the wah effect is applied.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Width of the frequency region at which the wah effect is applied Increasing this" +
                " value will make the frequency region narrower.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Volume balance of the sound that passes through the wah (W) and the unprocessed " +
                "sound (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Gain of the low range.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Gain of the high range.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "51: OD/DS -> TouchWah\r\n", "_MFX_OdDsToTouchWah.png",
                "Output level.\r\n", "", 0x2940);
            // 52: OD/DS -> AutoWah Amplifier
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Overdrive/distortion on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Type of distortion.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Degree of distortion.\r\n" +
                "\r\n" +
                "Also changes the volume.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Sound quality of the Overdrive effect.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Turns the Amp Simulator on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Type of guitar amp.\r\n" +
                "\r\n" +
                "SMALL: small amp.\r\n" +
                "BUILT-IN: single-unit type amp.\r\n" +
                "2-STACK: large double stack amp.\r\n" +
                "3-STACK: large triple stack amp.\r\n", "", 0x2940);
            // 52: OD/DS -> AutoWah Filter and autowah
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Auto Wah on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "LPF: Produces a wah effect in a broad frequency range.\r\n", "_PCM_TVF_01.png", 0x2544);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Type of filter.\r\n" +
                "\r\n" +
                "BPF: Produces a wah effect in a narrow frequency range.\r\n", "_PCM_TVF_02.png", 0x2544);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Center frequency at which the wah effect is applied.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Width of the frequency region at which the wah effect is applied.\r\n" +
                "\r\n" +
                "Increasing this value will make the frequency region narrower.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Select Hertz or note length to set rate at which the wah effect is modulated.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Rate at which the wah effect is modulated in Hertz.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Rate at which the wah effect is modulated as note length.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Depth at which the wah effect is modulated.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Volume balance of the sound that passes through the wah (W) and the unprocessed " +
                "sound (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Gain of the low range.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Gain of the high range.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "52: OD/DS -> AutoWah\r\n", "_MFX_OdDsToAutoWah.png",
                "Output level.\r\n", "", 0x2940);
            // 53: GuitarAmpSim -> Chorus
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Turns the amp switch on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Type of guitar amp.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Volume and amount of distortion of the amp.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Volume of the entire pre-amp.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Amount of pre-amp distortion.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Tone of the bass frequency range.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Tone of the middle frequency range.\r\n" +
                "\r\n" +
                "* Middle has no effect if “Match Drive” is selected as the Pre Amp Type.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Tone of the treble frequency range.\r\n", "", 0x2940);
            // 53: GuitarAmpSim -> Chorus
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Chorus on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Adjusts the delay time from the direct sound until the chorus sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Frequency of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Depth of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Volume balance of the sound that passes through the chorus (W) and the unprocess" +
                "ed sound (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Selects whether the sound will be sent through the speaker simulation (ON) or no" +
                "t (OFF).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Type of speaker.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "53: GuitarAmpSim -> Chorus\r\n", "_MFX_GuitarAmpSimToChorus.png",
                "Output Level.\r\n", "", 0x2940);
            // 54: GuitarAmpSim -> Flanger, Amp
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Turns the amp switch on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Type of guitar amp.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Volume and amount of distortion of the amp.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Volume of the entire pre-amp.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Amount of pre-amp distortion.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Tone of the bass frequency range.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Tone of the middle frequency range.\r\n" +
                "\r\n" +
                "* Middle has no effect if “Match Drive” is selected as the Pre Amp Type.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Tone of the treble frequency range.\r\n", "", 0x2940);
            // 54: GuitarAmpSim -> Flanger, Flanger 
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Flanger on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Adjusts the delay time from when the direct sound begins until the flanger sound" +
                " is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Frequency of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Depth of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Adjusts the proportion of the flanger sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Adjusts the volume balance between the sound that is sent through the flanger (W" +
                ") and the sound that is not  ent through the flanger (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Selects whether the sound will be sent through the speaker simulation (ON) or no" +
                "t (OFF).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Type of speaker.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "54: GuitarAmpSim -> Flanger\r\n", "_MFX_GuitarAmpSimToFlanger.png",
                "Output Level.\r\n", "", 0x2940);
            // 55: GuitarAmpSim -> Phaser, Amp
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Turns the amp switch on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Type of guitar amp.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Volume and amount of distortion of the amp.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Volume of the entire pre-amp.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Amount of pre-amp distortion.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Tone of the bass frequency range.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Tone of the middle frequency range.\r\n" +
                "\r\n" +
                "* Middle has no effect if “Match Drive” is selected as the Pre Amp Type.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Tone of the treble frequency range.\r\n", "", 0x2940);
            // 55: GuitarAmpSim -> Phaser, Phaser
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Phaser on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Center frequency at which the sound is modulated.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Amount of feedback.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Volume of phase-shifted sound.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Modulation rate.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Modulation depth.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Selects whether the sound will be sent through the speaker simulation (ON) or no" +
                "t (OFF).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Type of speaker.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "55: GuitarAmpSim -> Phaser\r\n", "_MFX_GuitarAmpSimToFhaser.png",
                "Output Level.\r\n", "", 0x2940);
            // 56: GuitarAmpSim -> Delay, Amp
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
               "Turns the amp switch on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Type of guitar amp.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Volume and amount of distortion of the amp.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Volume of the entire pre-amp.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Amount of pre-amp distortion.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Tone of the bass frequency range.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Tone of the middle frequency range.\r\n" +
                "\r\n" +
                "* Middle has no effect if “Match Drive” is selected as the Pre Amp Type.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Tone of the treble frequency range.\r\n", "", 0x2940);
            // 56: GuitarAmpSim -> Delay, Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Delay on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Adjusts the delay time from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Adjusts the proportion of the delay sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Frequency at which the highfrequency portion of the delay sound will be cut (BYP" +
                "ASS: no cut).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Adjusts the volume balance between the sound that is sent through the delay (W) " +
                "and the sound that is not sent through the delay (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Selects whether the sound will be sent through the speaker simulation (ON) or no" +
                "t (OFF).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Type of speaker.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "56: GuitarAmpSim -> Delay\r\n", "_MFX_GuitarAmpSimToDelay.png",
                "Output Level.\r\n", "", 0x2940);
            // 57: EP AmpSim -> Tremolo
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
               "Type of amp.\r\n" +
               "\r\n" +
               "OLDCASE: a standard electric piano sound of the early 70s.\r\n" +
               "NEWCASE: a standard electric piano sound of the late 70s and early 80s.\r\n" +
               "WURLY: a standard electric piano sound of the 60s.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Amount of low-frequency boost/cut.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Amount of high-frequency boost/cut.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Tremolo on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Select Hertz or note length to set rate of the tremolo effect.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Rate of the tremolo effect in Hertz.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Rate of the tremolo effect as note length.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Depth of the tremolo effect.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Adjusts the duty cycle of the LFO waveform used to apply tremolo.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Type of speaker.\r\n" +
                "\r\n" +
                "* If LINE is selected, the sound will not be sent through the speaker simulation" +
                ".\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Overdrive on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Overdrive input level.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Degree of distortion.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "57: EP AmpSim -> Tremolo\r\n", "_MFX_EpAmpSimToTremolo.png",
                "Output Level.\r\n", "", 0x2940);
            // 58: EP AmpSim -> Chorus
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Type of amp.\r\n" +
                "\r\n" +
                "OLDCASE: a standard electric piano sound of the early 70s.\r\n" +
                "NEWCASE: a standard electric piano sound of the late 70s and early 80s.\r\n" +
                "WURLY: a standard electric piano sound of the 60s.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Amount of low-frequency boost/cut.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Amount of high-frequency boost/cut.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Chorus on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Adjusts the delay time from the direct sound until the chorus sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Select Hertz or note length to set frequency of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Frequency of modulation as note length.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Depth of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Volume balance of the sound that passes through the chorus (W) and the unprocess" +
                "ed sound (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Type of speaker.\r\n" +
                "\r\n" +
                "* If LINE is selected, the sound will not be sent through the speaker simulation" +
                ".\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Overdrive on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Overdrive input level.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Degree of distortion.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "58: EP AmpSim -> Chorus\r\n", "_MFX_EpAmpSimToChorus.png",
                "Output Level.\r\n", "", 0x2940);
            // 59: EP AmpSim -> Flanger
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Type of amp.\r\n" +
                "\r\n" +
                "OLDCASE: a standard electric piano sound of the early 70s.\r\n" +
                "NEWCASE: a standard electric piano sound of the late 70s and early 80s.\r\n" +
                "WURLY: a standard electric piano sound of the 60s.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Amount of low-frequency boost/cut.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Amount of high-frequency boost/cut.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Flanger on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Adjusts the delay time from when the direct sound begins until the flanger sound" +
                " is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Select Hertz or note length to set frequency of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Frequency of modulation as note length.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Depth of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Adjusts the proportion of the flanger sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Adjusts the volume balance between the sound that is sent through the flanger (W" +
                ") and the sound that is not sent through the flanger (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Type of speaker.\r\n" +
                "\r\n" +
                "* If LINE is selected, the sound will not be sent through the speaker simulation" +
                ".\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Overdrive on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Overdrive input level.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Degree of distortion.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "59: EP AmpSim -> Flanger\r\n", "_MFX_EpAmpSimToFlanger.png",
                "Output Level.\r\n", "", 0x2940);
            // 60: EP AmpSim -> Phaser
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
               "Type of amp.\r\n" +
               "\r\n" +
               "OLDCASE: a standard electric piano sound of the early 70s.\r\n" +
               "NEWCASE: a standard electric piano sound of the late 70s and early 80s.\r\n" +
               "WURLY: a standard electric piano sound of the 60s.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Amount of low-frequency boost/cut.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Amount of high-frequency boost/cut.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Phaser on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Center frequency at which the sound is modulated.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Amount of feedback.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Volume of phase-shifted sound.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Select Hertz or note length to set modulation rate.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Modulation rate in Hertz.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Modulation rate as note length.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Modulation depth.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Type of speaker.\r\n" +
                "\r\n" +
                "* If LINE is selected, the sound will not be sent through the speaker simulation" +
                ".\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Overdrive on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Overdrive input level.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Degree of distortion.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "60: EP AmpSim -> Phaser\r\n", "_MFX_EpAmpSimToPhaser.png",
                "Output Level.\r\n", "", 0x2940);
            // 61: EP AmpSim -> Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Type of amp.\r\n" +
                "\r\n" +
                "OLDCASE: a standard electric piano sound of the early 70s.\r\n" +
                "NEWCASE: a standard electric piano sound of the late 70s and early 80s.\r\n" +
                "WURLY: a standard electric piano sound of the 60s.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Amount of low-frequency boost/cut.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Amount of high-frequency boost/cut.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Delay on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Select milliseconds or note legth for adjusting the delay time from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Adjusts the delay time in milliseconds from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Adjusts the delay time as note length from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Speed at which the current delay time changes to the specified delay time when y" +
                "ou change the delay time.\r\n" +
                "\r\n" +
                "The speed of the pitch change will change simultaneously with the delay time.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Adjusts the proportion of the delay sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Frequency at which the highfrequency portion of the delay sound will be cut (BYP" +
                "ASS: no cut).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Adjusts the volume balance between the sound that is sent through the delay (W) " +
                "and the sound that is not sent through the delay (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Type of speaker.\r\n" +
                "\r\n" +
                "* If LINE is selected, the sound will not be sent through the speaker simulation" +
                ".\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Overdrive on/off.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Overdrive input level.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Degree of distortion.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "61: EP AmpSim -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Output level.\r\n", "", 0x2940);
            // 62: Enhancer -> Chorus
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "62: Enhancer -> Chorus\r\n", "_MFX_EnhancerToChorus.png",
                "Sensitivity of the enhancer.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "62: Enhancer -> Chorus\r\n", "_MFX_EnhancerToChorus.png",
                "Level of the overtones generated by the enhancer.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "62: Enhancer -> Chorus\r\n", "_MFX_EnhancerToChorus.png",
                "Adjusts the delay time from the direct sound until the chorus sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "62: Enhancer -> Chorus\r\n", "_MFX_EnhancerToChorus.png",
                "Select Hertz or note length for setting frequency of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "62: Enhancer -> Chorus\r\n", "_MFX_EnhancerToChorus.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "62: Enhancer -> Chorus\r\n", "_MFX_EnhancerToChorus.png",
                "Frequency of modulation as note length.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "62: Enhancer -> Chorus\r\n", "_MFX_EnhancerToChorus.png",
                "Depth of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "62: Enhancer -> Chorus\r\n", "_MFX_EnhancerToChorus.png",
                "Adjusts the volume balance between the sound that is sent through the chorus (W)" +
                " and the sound that is not sent through the chorus (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "62: Enhancer -> Chorus\r\n", "_MFX_EnhancerToChorus.png",
                "Output Level.\r\n", "", 0x2940);
            // 63: Enhancer -> Flanger
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "63: Enhancer -> Flanger\r\n", "_MFX_EnhancerToFlanger.png",
                "Sensitivity of the enhancer.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "63: Enhancer -> Flanger\r\n", "_MFX_EnhancerToFlanger.png",
                "Level of the overtones generated by the enhancer.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "63: Enhancer -> Flanger\r\n", "_MFX_EnhancerToFlanger.png",
                "Adjusts the delay time from when the direct sound begins until the flanger sound" +
                " is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "63: Enhancer -> Flanger\r\n", "_MFX_EnhancerToFlanger.png",
                "Select Hertz or note length to set frequency of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "63: Enhancer -> Flanger\r\n", "_MFX_EnhancerToFlanger.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "63: Enhancer -> Flanger\r\n", "_MFX_EnhancerToFlanger.png",
                "Frequency of modulation as note length.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "63: Enhancer -> Flanger\r\n", "_MFX_EnhancerToFlanger.png",
                "Depth of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "63: Enhancer -> Flanger\r\n", "_MFX_EnhancerToFlanger.png",
                "Adjusts the proportion of the flanger sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "63: Enhancer -> Flanger\r\n", "_MFX_EnhancerToFlanger.png",
                "Adjusts the volume balance between the sound that is sent through the flanger (W" +
                ") and the sound that is not sent through the flanger (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "63: Enhancer -> Flanger\r\n", "_MFX_EnhancerToFlanger.png",
                "Output Level.\r\n", "", 0x2940);
            // 64: Enhancer -> Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "64: Enhancer -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Sensitivity of the enhancer.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "64: Enhancer -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Level of the overtones generated by the enhancer.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "64: Enhancer -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Select milliseconds or note length for adjusting the delay time from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "64: Enhancer -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Adjusts the delay time in milliseconds from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "64: Enhancer -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Adjusts the delay time as note length from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "64: Enhancer -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Adjusts the proportion of the delay sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "64: Enhancer -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Adjusts the frequency above which sound fed back to the effect will be cut.\r\n" +
                "\r\n" +
                "If you do not want to cut the high frequencies, set this parameter to BYPASS.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "64: Enhancer -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Adjusts the volume balance between the sound that is sent through the delay (W) " +
                "and the sound that is not sent through the  delay (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "64: Enhancer -> Delay\r\n", "_MFX_EpAmpSimToDelay.png",
                "Output Level.\r\n", "", 0x2940);
            // 65: Chorus -> Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Adjusts the delay time from the direct sound until the chorus sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Select Hertz or note length to set frequency of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Frequency of modulation as note length.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Depth of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Volume balance between the direct sound (D) and the chorus sound (W).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Select milliseconds or note length to adjust the delay time from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Adjusts the delay time in milliseconds from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Adjusts the delay time as note length from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Adjusts the proportion of the delay sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Adjusts the frequency above which sound fed back to the effect will be cut.\r\n" +
                "\r\n" +
                "If you do not want to cut the high frequencies, set this parameter to BYPASS.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Adjusts the volume balance between the sound that is sent through the delay (W) " +
                "and the sound that is not sent through the delay (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "65: Chorus -> Delay\r\n", "_MFX_ChorusToDelay.png",
                "Output Level.\r\n", "", 0x2940);
            // 66: Flanger -> Delay
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Adjusts the delay time from when the direct sound begins until the flanger sound" +
                " is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Select Hertz or note length to set frequency of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Frequency of modulation in Hertz.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Frequency of modulation as note length.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Depth of modulation.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Adjusts the proportion of the flanger sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Volume balance between the direct sound (D) and the flanger sound (W).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Select milliseconds or note length to adjust the delay time from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Adjusts the delay time in milliseconds from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Adjusts the delay time as note length from the direct sound until the delay sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Adjusts the proportion of the delay sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Adjusts the frequency above which sound fed back to the effect will be cut.\r\n" +
                "\r\n" +
                "If you do not want to cut the high frequencies, set this parameter to BYPASS.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Adjusts the volume balance between the sound that is sent through the delay (W) " +
                "and the sound that is not sent through the delay (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "66: Flanger -> Delay\r\n", "_MFX_FlangerToDelay.png",
                "Output Level.\r\n", "", 0x2940);
            // 67: Chorus -> Flanger
            ItemIndex = 0;
            PageIndex++;
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
               "Adjusts the delay time from the direct sound until the chorus sound is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Select Hertz or note length to set modulation frequency of the chorus effect.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Modulation frequency of the chorus effect, in Hertz.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Modulation frequency of the chorus effect, as note length.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Modulation depth of the chorus effect.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Volume balance between the direct sound (D) and the chorus sound (W).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Adjusts the delay time from when the direct sound begins until the flanger sound" +
                " is heard.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Select Hertz or note length to set modulation frequency of the flanger effect.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Modulation frequency of the flanger effect, in Hertz.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Modulation frequency of the flanger effect, as note length.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Modulation depth of the flanger effect.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Adjusts the proportion of the flanger sound that is fed back into the effect.\r\n" +
                "\r\n" +
                "Negative “-” settings will invert the phase.\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Adjusts the volume balance between the sound that is sent through the flanger (W" +
                ") and the sound that is not sent through the flanger (D).\r\n", "", 0x2940);
            Add(5, PageIndex, (byte)ItemIndex++, 0, "67: Chorus -> Flanger\r\n", "_MFX_ChorusToFlanger.png",
                "Output Level.\r\n", "", 0x2940);












        }
    }

    class HelpItem
    {
        public String Heading { get; set; }
        public String HeadingImage { get; set; }
        public String Text { get; set; }
        public String Image { get; set; }
        public UInt16 SpaceAssign { get; set; }
        private Help Help;
        public byte spaceForHeading;
        public byte spaceForHeadingImage;
        public byte spaceForText;
        public byte spaceForImage;

        public HelpItem(Help Help, String Heading, String HeadingImage, String Text, String Image, UInt16 SpaceAssign = 0x10c0)
        {
            this.Help = Help;
            this.Heading = Heading;
            this.HeadingImage = HeadingImage;
            this.Text = Text;
            this.Image = Image;
            this.spaceForHeading = (byte)((SpaceAssign & 0xf000) >> 12);
            this.spaceForHeadingImage = (byte)((SpaceAssign & 0x0f00) >> 8);
            this.spaceForText = (byte)((SpaceAssign & 0x00f0) >> 4);
            this.spaceForImage = (byte)(SpaceAssign & 0x000f);
        }

        public HelpItem Show()
        {
            spaceForHeading = spaceForHeading == (byte)0 ? (byte)1 : spaceForHeading;
            spaceForHeadingImage = spaceForHeadingImage == (byte)0 ? (byte)1 : spaceForHeadingImage;
            spaceForText = spaceForText == (byte)0 ? (byte)1 : spaceForText;
            spaceForImage = spaceForImage == (byte)0 ? (byte)1 : spaceForImage;

            Help.tbEditToneHelpsHeading.Text = "";
            Help.tbEditToneHelpsText.Text = "";

            if (!String.IsNullOrEmpty(Heading))
            {
                Help.tbEditToneHelpsHeading.Text = Heading;
            }
            if (!String.IsNullOrEmpty(HeadingImage))
            {
                try
                {
                    Help.imgEditToneHeadingImage.Source = ImageSource.FromFile(HeadingImage);
                    if (Help.imgEditToneHeadingImage.Source == null)
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    Help.imgEditToneImage.Source = ImageSource.FromResource("MissingImage.jpg");
                }
            }
            else
            {
                Help.imgEditToneHeadingImage.Source = "";
            }
            if (!String.IsNullOrEmpty(Text))
            {
                Help.tbEditToneHelpsText.Text = Text;
            }
            if (!String.IsNullOrEmpty(Image))
            {
                try
                {
                    Help.imgEditToneImage.Source = ImageSource.FromFile(Image);
                    if (Help.imgEditToneImage.Source == null)
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    Help.imgEditToneImage.Source = ImageSource.FromResource("MissingImage.jpg");
                }
            }
            else
            {
                Help.imgEditToneImage.Source = "";
            }
            return this;
        }
    }

    class HelpTag
    {
        public UInt16 ItemIndex { get; set; }
        public UInt16 SubItemIndex { get; set; }

        public HelpTag(UInt16 ItemIndex, UInt16 SubItemIndex)
        {
            this.ItemIndex = ItemIndex;
            this.SubItemIndex = SubItemIndex;
        }
    }
    #endregion
}
