using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integra_7_Xamarin
{
    public partial class UIHandler
    {
        enum Edit_CurrentMidiRequest
        {
            NONE,
            QUERYING_TONE_TYPE,
            QUERYING_SELECTED_SOUND,
            QUERYING_PCM_SYNTH_TONE_COMMON,             // length 01 11 start 19 00 00 00 (for part 1)
            QUERYING_PCM_SYNTH_TONE_COMMON_MFX,             // length 01 11 start 19 00 02 00 (for part 1)
            QUERYING_PCM_SYNTH_TONE_PMT,
            QUERYING_PCM_SYNTH_TONE_PARTIAL,
            QUERYING_PCM_SYNTH_TONE_COMMON2,
            QUERYING_PCM_DRUM_KIT_COMMON,               // length 01 11 start 19 10 00 00 (for part 1)
            QUERYING_PCM_DRUM_KIT_COMMON_MFX,
            QUERYING_PCM_DRUM_KIT_COMMON_COMP_EQ,
            QUERYING_PCM_DRUM_KIT_PARTIAL,
            QUERYING_PCM_DRUM_KIT_COMMON2,
            QUERYING_SUPERNATURAL_SYNTH_TONE_UNDOCUMENTED_PARAMETERS,
            QUERYING_SUPERNATURAL_SYNTH_TONE_COMMON,
            QUERYING_SUPERNATURAL_SYNTH_TONE_COMMON_MFX,
            QUERYING_SUPERNATURAL_SYNTH_TONE_PMT,
            QUERYING_SUPERNATURAL_SYNTH_TONE_PARTIAL,
            QUERYING_SUPERNATURAL_SYNTH_TONE_MISC,
            QUERYING_SUPERNATURAL_ACOUSTIC_TONE_COMMON,
            QUERYING_SUPERNATURAL_ACOUSTIC_TONE_COMMON_MFX,
            QUERYING_SUPERNATURAL_ACOUSTIC_TONE_WAVE,
            QUERYING_SUPERNATURAL_DRUM_KIT_COMMON,
            QUERYING_SUPERNATURAL_DRUM_KIT_COMMON_MFX,
            QUERYING_SUPERNATURAL_DRUM_KIT_COMMON_COMP_EQ,
            QUERYING_SUPERNATURAL_DRUM_KIT_PARTIAL,
            QUERYING_STUDIO_SET_SEND_PARAMETERS,
            GET_CURRENT_STUDIO_SET_NUMBER,
            STUDIO_SET_TITLES,
            STUDIO_SET_COMMON,
            SYSTEM_COMMON,
            STUDIO_SET_CHORUS,
            STUDIO_SET_CHORUS_OFF,
            STUDIO_SET_CHORUS_CHORUS,
            STUDIO_SET_CHORUS_DELAY,
            STUDIO_SET_CHORUS_GM2_CHORUS,
            STUDIO_SET_REVERB,
            STUDIO_SET_REVERB_OFF,
            STUDIO_SET_REVERB_ROOM_HALL_PLATE,
            STUDIO_SET_REVERB_GM2_REVERB,
            STUDIO_SET_MOTIONAL_SURROUND,
            STUDIO_SET_MASTER_EQ,
            STUDIO_SET_PART,
            STUDIO_SET_PART_MIDI_PHASELOCK,
            STUDIO_SET_PART_EQ,
            EDIT_TONE_SAVE,
        }

        enum Edit_State
        {
            INIT,
            INIT_DONE,
            WAITING,
            NONE,
            QUERYING_CURRENT_STUDIO_SET_NUMBER,
            QUERYING_STUDIO_SET_NAMES,
            QUERYING_SYSTEM_COMMON,
            QUERYING_STUDIO_SET_COMMON,
            QUERYING_STUDIO_SET_CHORUS,
            QUERYING_STUDIO_SET_CHORUS_OFF,
            QUERYING_STUDIO_SET_CHORUS_CHORUS,
            QUERYING_STUDIO_SET_CHORUS_DELAY,
            QUERYING_STUDIO_SET_CHORUS_GM2_CHORUS,
            QUERYING_STUDIO_SET_REVERB,
            QUERYING_STUDIO_SET_REVERB_OFF,
            QUERYING_STUDIO_SET_REVERB_ROOM_HALL_PLATE,
            QUERYING_STUDIO_SET_REVERB_GM2_REVERB,
            QUERYING_STUDIO_SET_MOTIONAL_SURROUND,
            QUERYING_STUDIO_SET_MASTER_EQ,
            QUERYING_STUDIO_SET_PART,
            QUERYING_STUDIO_SET_PART_MIDI_PHASELOCK,
            QUERYING_STUDIO_SET_PART_EQ,
            UPDATE_CONTROLS,
            UPDATE_PCMDRUMKIT_PARTIAL,
            DONE,
        }

        //Boolean showTestControls = false;

        SelectedTone selectedSound;
        Edit_CurrentMidiRequest currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
        Edit_State edit_State;
        //Boolean initDone = false;
        //Boolean handleControlEvents = false;            // Some control events re-creates the control, and that will cause a loop. Use handleControlEvents to prevent that.
        Boolean currentHandleControlEvents = false;     // Use when temporarioy disabling control events.
        //DispatcherTimer timer = null;
        //byte[] currentStudioSetNumberAsBytes = new byte[3];
        //byte[] studioSetNumberIndexAsBytes = new byte[3];
        public ProgramType currentProgramType = ProgramType.PCM_SYNTH_TONE;
        Instrument currentInstrument;                   // For retaining more info on current SuperNATURAL Acoustic tone instrument.
        byte currentProgramIndex = 0;                   // 0 = PCM Synth Tone, 1 = PCM Drum Kit, 2 = SuperNATURAL Acoustic Tone, 3 = SuperNATURAL Synth Tone, 4 = SuperNATURAL Drum Kit
        byte currentParameterPageIndex = 0;             // 0 = Common, 1 = Wave etc, depending on program type
        String currentParameterPage = "";               // The text from the selector.
        byte controlsIndex = 0;                         // Index of a control. Used to mark controls for help context.
        byte currentPartial = 0;                        // 0 - 3 = partial 1 - 4
        byte currentPMT = 0;                            // Current Partial Mapping Table index
        byte currentKey = 0;                        // Current key for drum kit type tones. Each key has its own settings in its own partial.
                                                    // Starts with 21 for PCM Drum Kit (keys 21 - 108) and 27 for SuperNATURAL Drum Kit (keys 27 - 88).
        byte currentMatrixControlPage = 0;              // 0 - 3 = Matrix control page 1 - 4
        byte currentMFXType = 0;                        // 0 = None, 1 = Equalizer etc.
        byte currentMFXTypeOffset = 0;                  // Difference between MFX type combobox selected item due to splitted pages.
        byte currentMFXTypePageAddressOffset = 0;       // Offset to variables when a page i splitted. Set in SetMFXTypeAndOffset().
        byte currentMFXTypePageCount = 1;               // This is used to know when to read more than one page for a specific MFX type.
        byte currentMFXTypePageParameterOffset = 0;     // This is the offset into the parameters due to page split.
        byte mfxPageReadFromIntegra7 = 0xff;            // When entering Edit Tone, one MFX type is currently active in Integra-7,
                                                        // and that is the only one we can trust. When changing MFX type, Integra-7 does NOT
                                                        // fetch internally stored values (like it does if MFX type is changed ON the Integra-7)
                                                        // and thus we have to use default values from the table NumberedParametersContent.
                                                        // So, when entering Edit, save current MFX type here.
        UInt32 waitingForResponseFromIntegra7 = 0;      // A counter for detecting non-responsiveness from I-7. Used in Timer_Tick.
        //***SolidColorBrush blackBorder;
        //Thickness borderThickness;
        //Thickness margin2222;
        //Thickness margin2022;
        SRXWaves srxWaves = new SRXWaves();
        Int32 KeySamplePlayingL = -1;
        Int32 KeySamplePlayingR = -1;
        byte chorusSendLevel;
        byte reverbSendLevel;
        Boolean stopEditTimer;

        public Grid EditTonesGrid = null;
        public Grid EditTonesLeftColumnGrid = null;
        public Grid EditTonesRightColumnGrid = null;
        public Grid gridSecondRow = new Grid();
        public ColumnDefinition cdEditTone_ParameterPages;
        public ColumnDefinition cdEditTone_PartialSelector;
        public ColumnDefinition cdEditTone_InstrumentCategorySelector;
        public ColumnDefinition cdEditTone_KeySelector;
        public Grid ControlsGrid = null;
        public Grid HelpGrid = null;
        public RowDefinition rdHelpHeader = null;
        public RowDefinition rdHelpHeaderImage = null;
        public RowDefinition rdHelpText = null;
        public RowDefinition rdHelpImage = null;
        //public RowDefinition rdHelpMain = null;

        // Help objects:
        Help Help;
		
        // Dynamically created controls buddy textboxes.
        // PCMSynthTone Common:
        private ParameterSets parameterSets = new ParameterSets();
        private NumberedParametersContent numberedParametersContent = new NumberedParametersContent();
        String[] toneLengths;
        String[] keyNames;
        private TextBox tbEditTone_PCMSynthTone_ToneLevel = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TonePan = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_CoarseTune = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_FineTune = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_VibratoRate = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_VibratoDepth = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_VibratoDelay = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_AnalogFeel = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_CutoffOffset = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_ResonanceOffset = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_AttackTimeOffset = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_ReleaseTimeOffset = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_VelocitySenseOffset = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_PortamentoTime = new TextBox();
        // Wave
        private TextBox tbEditTone_PCMSynthTone_Wave_PartialDelayTime = new TextBox();
        private TextBox tbEditTone_pCMSynthTone_Wave_WaveFXMDepth = new TextBox();
        // PMT
        private TextBox tbEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper = new TextBox();
        private Slider slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper = new Slider();
        private Slider slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper = new Slider();
        private Slider slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower = new Slider();
        private Slider slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower = new Slider();
        private Slider slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper = new Slider();
        private Slider slEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower = new Slider();
        private Slider slEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper = new Slider();
        private Slider slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower = new Slider();
        // Pitch
        private TextBox tbEditTone_PCMSynthTone_Pitch_PartialCoarseTune = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_Pitch_PartialFineTune = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_Pitch_PitchBendRangeUp = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_Pitch_PitchBendRangeDown = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow = new TextBox();
        private TextBox[] tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime = new TextBox[4];
        private TextBox[] tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel = new TextBox[5];
        private TextBox tbEditTone_PCMSynthTone_TVF_TVFCutoffFrequency = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVF_TVFResonance = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow = new TextBox();
        private TextBox[] tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime = new TextBox[4];
        private TextBox[] tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel = new TextBox[5];
        private TextBox tbEditTone_PCMSynthTone_TVA_PartialOutputLevel = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVA_TVALevelVelocitySens = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVA_BiasLevel = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVA_BiasPosition = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVA_PartialPan = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVA_PartialPanKeyfollow = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVA_PartialRandomPanDepth = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow = new TextBox();
        private TextBox[] tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime = new TextBox[4];
        private TextBox[] tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel = new TextBox[3];
        private TextBox tbEditTone_PCMSynthTone_Output_PartialOutputLevel = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_Output_PartialChorusSendLevel = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_Output_PartialReverbSendLevel = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO1_LFORate = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO1_LFORateDetune = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO1_LFODelayTime = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO1_LFOFadeTime = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO1_LFOTVFDepth = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO1_LFOTVADepth = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO1_LFOPanDepth = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO1_LFOPitchDepth = new TextBox();

        private TextBox tbEditTone_PCMSynthTone_LFO2_LFORate = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO2_LFORateDetune = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO2_LFODelayTime = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO2_LFOFadeTime = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO2_LFOTVFDepth = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO2_LFOTVADepth = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO2_LFOPanDepth = new TextBox();
        private TextBox tbEditTone_PCMSynthTone_LFO2_LFOPitchDepth = new TextBox();
        private TextBox[] tbEditTone_PCMSynthTone_StepLFO_LFOStep = new TextBox[16];
        private ComboBox[] cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource = new ComboBox[4];
        ComboBox[][] cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination = new ComboBox[4][];
        public ComboBox[][] cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch = new ComboBox[4][];
        private TextBox[][] tbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens = new TextBox[4][];
        private Slider[][] slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens = new Slider[4][];
        private TextBox[] tbEditTone_PCMSynthTone_MFXControl_MFXControlSens = new TextBox[4];

        // Save controls
        private TextBox tbEditTone_SaveTone_Title = new TextBox();
        private TextBox tbEditTone_SaveTone_TitleText = new TextBox();
        private ComboBox cbEditTone_SaveTone_SlotNumber = new ComboBox();
        private Button btnEditTone_SaveTone = new Button();
        private Button btnEditTone_DeleteTone = new Button();

        // PCM Drum Kit
        private TextBox tbEditTone_PCMDrumKit_Common_DrumKitLevel = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_Common_PartialPitchBendRange = new TextBox();
        private TextBox[] tbEditTone_PCMDrumKit_Compressor_CompThreshold = new TextBox[6];
        private TextBox[] tbEditTone_PCMDrumKit_Compressor_CompOutputGain = new TextBox[6];
        private List<TextBox> dynamicTextboxes = null;
        private List<Slider> dynamicSliders = null;
        private List<ComboBox> dynamicComboboxes = null;
        private List<CheckBox> dynamicCheckboxes = null;
        // PCMSynthTone Wave:
        PCMWaveNames waveNames = new PCMWaveNames();
        ComboBox cbEditTone_PCMSynthTone_WaveGroupType = new ComboBox();
        TextBox tbEditTone_Wave_WaveNumberL = new TextBox();
        TextBox tbEditTone_Wave_WaveNumberR = new TextBox();
        ComboBox cbEditTone_PCMSynthTone_Wave_WaveNumberL = new ComboBox();
        ComboBox cbEditTone_PCMSynthTone_Wave_WaveNumberR = new ComboBox();
        private TextBox tbEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_Wave_WMTWaveFineTune = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_Wave_WMTWaveLevel = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_Wave_WMTWavePan = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower = new TextBox();
        // PCM Drum Kit Pitch tab
        private TextBox tbEditTone_PCMDrumKit_Pitch_PartialCoarseTune = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_Pitch_PartialFineTune = new TextBox();
        // PCM Drum Kit Pitch Env:
        private TextBox tbEditTone_PCMDrumKit_Pitch_PitchEnvDepth = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens = new TextBox();
        private TextBox[] tbEditTone_PCMDrumKit_Pitch_PitchEnvTime = new TextBox[4];
        private TextBox[] tbEditTone_PCMDrumKit_Pitch_PitchEnvLevel = new TextBox[5];
        // PCM Drum Kit TVF controls
        private TextBox tbEditTone_PCMDrumKit_TVF_TVFCutoffFrequency = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVF_TVFResonance = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens = new TextBox();
        // PCM Drum Kit TVF ENV tab
        private TextBox tbEditTone_PCMDrumKit_TVFenv_TVFEnvDepth = new TextBox();
        private TextBox[] tbEditTone_PCMDrumKit_TVFenv_TVFEnvTime = new TextBox[4];
        private TextBox[] tbEditTone_PCMDrumKit_TVFenv_TVFEnvLevel = new TextBox[5];
        // PCM Drum Kit TVA tab
        private TextBox tbEditTone_PCMDrumKit_TVA_PartialLevel = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVA_TVALevelVelocitySens = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVA_PartialPan = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVA_PartialRandomPanDepth = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVA_RelativeLevel = new TextBox();
        // PCM Drum Kit TVA ENV tab
        private TextBox tbEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens = new TextBox();
        private TextBox[] tbEditTone_PCMDrumKit_TVA_TVAEnvTime = new TextBox[4];
        private TextBox[] tbEditTone_PCMDrumKit_TVA_TVAEnvLevel = new TextBox[5];
        // PCM Drum Kit Output tab
        private TextBox tbEditTone_PCMDrumKit_TVA_PartialOutputLevel = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVA_PartialChorusSendLevel = new TextBox();
        private TextBox tbEditTone_PCMDrumKit_TVA_PartialReverbSendLevel = new TextBox();
        // PCM Drum Kit Eq tab
        private TextBox[] tbEditTone_PCMDrumKit_Equalizer_EQLowGain = new TextBox[6];
        private TextBox[] tbEditTone_PCMDrumKit_Equalizer_EQMidGain = new TextBox[6];
        private TextBox[] tbEditTone_PCMDrumKit_Equalizer_EQHighGain = new TextBox[6];
        // PCM Drum Kit MFX control
        private TextBox[] tbEditTone_PCMDrumKit_MFXcontrol_MFXControlSens = new TextBox[4];
        // SuperNATURAL Acoustic Tone Common tab
        private TextBox tbEditTone_SuperNATURALAcousticTone_Common_ToneLevel = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Common_CutoffOffset = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Common_VibratoRate = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Common_VibratoDepth = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Common_VibratoDelay = new TextBox();
        // SuperNATURAL Acoustic Tone Instrument tab
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument = new TextBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_Bank = new ComboBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber = new ComboBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_StringResonance = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16 = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel = new TextBox();
        private CheckBox cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSwitch = new CheckBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel = new TextBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16 = new ComboBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513 = new ComboBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8 = new ComboBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4 = new ComboBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223 = new ComboBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2 = new ComboBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135 = new ComboBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113 = new ComboBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1 = new ComboBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoft = new ComboBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlow = new ComboBox();
        private ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonic = new ComboBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime = new TextBox();
        private Slider slEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime = new Slider();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18 = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16 = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17 = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17 = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17 = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17 = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16 = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_BendDepth = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel = new TextBox();
        private TextBox tbEditTone_SuperNATURALAcousticTone_Instrument_DronePitch = new TextBox();
        // SuperNaturalAcousticTone MFXControl controls
        private TextBox[] tbEditTone_CommonMFX_Control_MFXControlSens = new TextBox[4];
        private TextBox[] tbEditTone_SuperNaturalAcousticTone_MFXControl_MFXControlSens = new TextBox[4];
        // SuperNATURALSynthTone common controls
        SuperNaturalSynthToneWaveNames superNaturalSynthToneWaveNames = new SuperNaturalSynthToneWaveNames();
        private TextBox tbEditTone_superNATURALSynthTone_Common_ToneLevel = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Common_WaveShape = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Common_AnalogFeel = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Common_PortamentoTime = new TextBox();
        // SuperNATURALSynthTone osc controls
        private TextBox tbEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Osc_PulseWidthShift = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Osc_SuperSawDetune = new TextBox();
        // SuperNATURALSynthTone pitch controls
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_OSCPitch = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_OSCDetune = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown = new TextBox();
        // SuperNATURALSynthTone filter controls
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_FILTERCutoff = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_FILTERResonance = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Pitch_HPFCutoff = new TextBox();
        // SuperNATURALSynthTone AMP controls
        private TextBox tbEditTone_superNATURALSynthTone_Amp_AMPLevel = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Amp_AMPPan = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime = new TextBox();
        // SuperNATURALSynthTone LFO controls
        private TextBox tbEditTone_superNATURALSynthTone_LFO_LFORate = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_LFO_LFOFadeTime = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_LFO_LFOPitchDepth = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_LFO_LFOFilterDepth = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_LFO_LFOAMPDepth = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_LFO_LFOPanDepth = new TextBox();
        // SuperNATURALSynthTone Modulation LFO controls
        private TextBox tbEditTone_superNATURALSynthTone_LFO_ModulationLFORate = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl = new TextBox();
        // SuperNATURALSynthTone Aftertouch controls
        private TextBox tbEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens = new TextBox();
        // SuperNATURALSynthTone Misc controls
        private TextBox tbEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens = new TextBox();
        private TextBox tbEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens = new TextBox();
        private TextBox[] tbEditTone_superNATURALSynthTone_MFXControl_MFXControlSens = new TextBox[4];
        // SuperNATURALSynthTone MFX Control controls
        private TextBox[] tbEditTone_CommonMFX_MFXControl_MFXControlSens = new TextBox[4];
        // SuperNATURAL Drum Kit Common controls
        private TextBox tbEditTone_superNATURALDrumKit_Common_KitLevel = new TextBox();
        private TextBox tbEditTone_superNATURALDrumKit_Common_AmbienceLevel = new TextBox();
        // SuperNATURAL Drum Kit Instrument controls
        private ComboBox cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber = new ComboBox();
        private ComboBox cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT = new ComboBox();
        private ComboBox cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6 = new ComboBox();
        private TextBox tbEditTone_superNATURALDrumKit_Druminstrument_Level = new TextBox();
        private TextBox tbEditTone_superNATURALDrumKit_Druminstrument_Pan = new TextBox();
        private TextBox tbEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel = new TextBox();
        private TextBox tbEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel = new TextBox();
        private TextBox tbEditTone_superNATURALDrumKit_Druminstrument_Tune = new TextBox();
        private TextBox tbEditTone_superNATURALDrumKit_Druminstrument_Attack = new TextBox();
        private TextBox tbEditTone_superNATURALDrumKit_Druminstrument_Decay = new TextBox();
        private TextBox tbEditTone_superNATURALDrumKit_Druminstrument_Brilliance = new TextBox();
        private ComboBox cbEditTone_superNATURALDrumKit_Druminstrument_Variation = new ComboBox();
        private TextBox tbEditTone_superNATURALDrumKit_Druminstrument_DynamicRange = new TextBox();
        private TextBox tbEditTone_superNATURALDrumKit_Druminstrument_StereoWidth = new TextBox();
        // SuperNATURAL Drum Kit Compressor controls
        private TextBox[] tbEditTone_superNATURALDrumKit_Compressor_CompThreshold = new TextBox[6];
        private TextBox[] tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain = new TextBox[6];
        private TextBox[] tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain = new TextBox[6];
        // SuperNATURAL Drum Kit Equalizer controls
        private TextBox[] tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain = new TextBox[6];
        private TextBox[] tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain = new TextBox[6];
        // SuperNATURAL Drum Kit MFX Control controls
        private TextBox[] tbEditTone_SuperNaturalDrumKit_MFXControl_MFXControlSens = new TextBox[4];
        // MFX type control:
        ComboBox cbEditTone_MFXType = null;

        // Data:
        NumberedParameterValues numberedParameterValues = new NumberedParameterValues();
        Boolean useMFXParameterDataFromIntegra_7 = true; // When selecting another MFX type, do not ask for parameter data from Integra-7 since it is not updated.
                                                         // Use NumberedParameterValues instead when changing MFXType. Otherwise asking Integra-7 is ok.
        SuperNATURALAcousticToneVariations superNATURALAcousticToneVariations = new SuperNATURALAcousticToneVariations();
        SuperNATURALAcousticToneVariation superNATURALAcousticToneVariation;
        SuperNaturalAcousticInstrumentList superNaturalAcousticInstrumentList = new SuperNaturalAcousticInstrumentList();
        //SuperNATURALDrumKitInstrumentList superNATURALDrumKitInstrumentList = new SuperNATURALDrumKitInstrumentList(); // Moved to UIHandler.cs
        PCMSynthTone pCMSynthTone;
        PCMDrumKit pCMDrumKit;
        SuperNATURALSynthTone superNATURALSynthTone;
        SuperNATURALAcousticTone superNATURALAcousticTone;
        SuperNATURALDrumKit superNATURALDrumKit;
        Undocumented_Commands unDocumented_Commands;
        Undocumented_Parameters undocumented_Parameters;
        CommonMFX commonMFX;
        Phrases phrases = new Phrases();
        //Boolean firstTime = true;
        public static String saveAndLoadPath = "";
        //*** public ApplicationDataContainer localSettings = null;

        // Controls that are always present (not in parameter pages)
        public ComboBox cbEditTone_PartSelector = null;
        public ComboBox cbEditTone_SynthesizerType = null;
        public Button tbEditTone_Instrument = null;
        public ComboBox cbEditTone_KeySelector = null;
        public ComboBox cbEditTone_ParameterPages = null;
        public ComboBox cbEditTone_PartialSelector = null;
        public ComboBox cbEditTone_InstrumentCategorySelector = null;
        public Label tbEditToneChorusSendLevel = null;
        public Slider slEditToneChorusSendLevel = null;
        public Label tbEditToneReverbSendLevel = null;
        public Slider slEditToneReverbSendLevel = null;
        public Label tbEditToneHelpsHeading = null; // " Text="Help heading texts goes here..." VerticalAlignment="Center" HorizontalAlignment="Stretch"
        public Image imgEditToneHeadingImage = null; //" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" Tapped="Help_GotFocus" />
        public Label tbEditToneHelpsText = null; // " Text="Help texts goes here..." VerticalAlignment="Center" HorizontalAlignment="Stretch"
        public Image imgEditToneImage = null; // " VerticalAlignment="Stretch" HorizontalAlignment="Stretch" Tapped="Help_GotFocus"/>
        public Button btnEditToneMainHelp = null; // " Content="Show main help info for selected synthesizer type" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" Tapped="Help_GotFocus" />
        public Button btnEditTone_Play = null; // " Margin="2,0,1,2" Content="Play" HorizontalAlignment="Stretch"
        public Button btnEditTone_Reset = null; // " x:Uid="btnStudioSetReturn" Margin="1,0,1,2" Content="Reset" HorizontalAlignment="Stretch"
        public Button btnEditTone_Return = null; // " x:Uid="btnStudioSetReturn" Margin="1,0,2,2" Content="Return" HorizontalAlignment="Stretch"

        /* The tone editor page has a common layout into which 5 different parts implemented in left column,
         * one for each type of synthesizer: SN-A, SN-S, SN-D, PCM-C and PCM-D.
         * The common part varies a little depending of synthesizer type, e.g. SN-A has no partials, thus partial selector.
         * Each synth specific part also has different sub-pages.
         * All parts has subpages for MFX and save/delete tone, where MFX is a common page.
         * 
         * This is th actual common layout content we must generate:
         * 
            <Grid x:Name="EditTonesGrid" Visibility="Visible">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="3*" />
                    <ColumnDefinition Width="2*" />
                </Grid.ColumnDefinitions>
                <Grid Grid.Column="0" Margin="2,2,2,2" BorderBrush="black" BorderThickness="1">
                    <Grid.RowDefinitions>
                        <RowDefinition Height="*" />
                        <RowDefinition Height="*" />
                        <RowDefinition Height="14*" />
                    </Grid.RowDefinitions>
                    <Grid Grid.Row="0">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="*" />
                            <ColumnDefinition Width="*" />
                            <ColumnDefinition Width="*" />
                        </Grid.ColumnDefinitions>
                        <ComboBox Grid.Column="0" x:Name="cbEditTone_PartSelector" HorizontalAlignment="Stretch" VerticalAlignment="Stretch" SelectedIndex="0"
                                  SelectionChanged="cbEditTone_PartSelector_SelectionChanged" GotFocus="cbEditTone_PartSelector_GotFocus" >
                            <x:String>Part 1</x:String>
                            <x:String>Part 2</x:String>
                            <x:String>Part 3</x:String>
                            <x:String>Part 4</x:String>
                            <x:String>Part 5</x:String>
                            <x:String>Part 6</x:String>
                            <x:String>Part 7</x:String>
                            <x:String>Part 8</x:String>
                            <x:String>Part 9</x:String>
                            <x:String>Part 10</x:String>
                            <x:String>Part 11</x:String>
                            <x:String>Part 12</x:String>
                            <x:String>Part 13</x:String>
                            <x:String>Part 14</x:String>
                            <x:String>Part 15</x:String>
                            <x:String>Part 16</x:String>
                        </ComboBox>
                        <ComboBox Grid.Column="1" x:Name="cbEditTone_SynthesizerType" HorizontalAlignment="Stretch" VerticalAlignment="Stretch" SelectedIndex="0"
                                  SelectionChanged="cbEditTone_SynthesizerType_SelectionChanged" GotFocus="cbEditTone_SynthesizerType_GotFocus"  >
                            <x:String>PCM synth tone</x:String>
                            <x:String>PCM drum kit</x:String>
                            <x:String>SuperNATURAL acoustic tone</x:String>
                            <x:String>SuperNATURAL synth tone</x:String>
                            <x:String>SuperNATURAL drum kit</x:String>
                        </ComboBox>
                        <Grid Grid.Column="2" HorizontalAlignment="Stretch" VerticalAlignment="Stretch">
                            <Button x:Name="tbEditTone_Instrument" HorizontalAlignment="Stretch" VerticalAlignment="Stretch" IsEnabled="False" Background="White"/>
                        </Grid>
                    </Grid>
                    <Grid Grid.Row="1">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="1*" />
                            <ColumnDefinition Width="1*" x:Name="cdEditTone_PartialSelector" />
                            <ColumnDefinition Width="1*" />
                        </Grid.ColumnDefinitions>
                        <ComboBox Grid.Column="0" x:Name="cbEditTone_ParameterPages" HorizontalAlignment="Stretch" VerticalAlignment="Stretch"
                                  SelectionChanged="cbEditTone_ParameterPages_SelectionChanged" GotFocus="cbEditTone_ParameterPages_GotFocus" >
                        </ComboBox>
                        <ComboBox Grid.Column="1" x:Name="cbEditTone_PartialSelector" HorizontalAlignment="Stretch" VerticalAlignment="Stretch"
                                  SelectionChanged="cbEditTone_PartialSelector_SelectionChanged" GotFocus="cbEditTone_PartialSelector_GotFocus" >
                        </ComboBox>
                        <TextBox Grid.Column="1" x:Name="tbEditTone_KeyName" Margin="2,2,2,2" HorizontalAlignment="Stretch" VerticalAlignment="Center"
                                 BorderBrush="White"
                                 TextChanged="tbEditTone_KeyName_TextChanged" GotFocus="tbEditTone_KeyName_GotFocus" Visibility="Collapsed">
                        </TextBox>
                        <ComboBox Grid.Column="2" x:Name="cbEditTone_KeySelector" HorizontalAlignment="Stretch" VerticalAlignment="Stretch"
                                  SelectionChanged="cbEditTone_KeySelector_SelectionChanged" GotFocus="cbEditTone_KeySelector_GotFocus" Visibility="Collapsed" >
                        </ComboBox>
                        <ComboBox Grid.Column="2" x:Name="cbEditTone_InstrumentCategorySelector" HorizontalAlignment="Stretch" VerticalAlignment="Stretch"
                                  SelectionChanged="cbEditTone_InstrumentCategorySelector_SelectionChanged" GotFocus="cbEditTone_InstrumentCategorySelector_GotFocus"  >
                        </ComboBox>
                    </Grid>
                    <Grid Grid.Row="2">
                        <Grid x:Name="ControlsGrid" Margin="2,2,2,2" BorderBrush="Black" BorderThickness="1" >
                            <Grid.RowDefinitions>
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                                <RowDefinition Height="*" />
                            </Grid.RowDefinitions>
                        </Grid>
                    </Grid>
                </Grid>
                <Grid Grid.Column="1" x:Name="EditToneColumn1" Margin="0,2,2,2" BorderBrush="black" BorderThickness="1">
                    <Grid.RowDefinitions>
                        <RowDefinition Height="*" />
                        <RowDefinition Height="*" />
                        <RowDefinition Height="14*" />
                        <RowDefinition Height="*" />
                    </Grid.RowDefinitions>
                    <Grid Grid.Row="0">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="*" />
                            <ColumnDefinition Width="2*" />
                        </Grid.ColumnDefinitions>
                        <Grid Grid.Column="0" Margin="4,4,4,4" VerticalAlignment="Stretch" HorizontalAlignment="Stretch">
                            <TextBlock x:Name="tbEditToneChorusSendLevel" Text="Chorus send level" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" />
                        </Grid>
                        <Grid Grid.Column="1" Margin="4,4,4,4" VerticalAlignment="Stretch" HorizontalAlignment="Stretch">
                            <Slider x:Name="slEditToneChorusSendLevel" Minimum="0" Maximum="127" VerticalAlignment="Stretch" HorizontalAlignment="Stretch"
                                    ValueChanged="slEditToneChorusSendLevel_ValueChanged" GotFocus="slEditToneChorusSendLevel_GotFocus" />
                        </Grid>
                    </Grid>
                    <Grid Grid.Row="1">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="*" />
                            <ColumnDefinition Width="2*" />
                        </Grid.ColumnDefinitions>
                        <Grid Grid.Column="0" Margin="4,4,4,4" VerticalAlignment="Stretch" HorizontalAlignment="Stretch">
                            <TextBlock x:Name="tbEditToneReverbSendLevel" Text="Reverb send level" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" />
                        </Grid>
                        <Grid Grid.Column="1" Margin="4,4,4,4" VerticalAlignment="Stretch" HorizontalAlignment="Stretch">
                            <Slider x:Name="slEditToneReverbSendLevel" Minimum="0" Maximum="127" VerticalAlignment="Stretch" HorizontalAlignment="Stretch"
                                    ValueChanged="slEditToneReverbSendLevel_ValueChanged" GotFocus="slEditToneReverbSendLevel_GotFocus" />
                        </Grid>
                    </Grid>
                    <Grid Grid.Row="2" x:Name="HelpGrid" Tapped="Help_GotFocus" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" >
                        <Grid.RowDefinitions>
                            <RowDefinition Height="*" x:Name="rdHelpHeader"/>
                            <RowDefinition Height="*" x:Name="rdHelpHeaderImage" />
                            <RowDefinition Height="6*" x:Name="rdHelpText"/>
                            <RowDefinition Height="6*" x:Name="rdHelpImage" />
                            <RowDefinition Height="*" x:Name="rdHelpMain" />
                        </Grid.RowDefinitions>
                        <Grid Grid.Row="0" x:Name="EditToneHelpHeading" Margin="4,4,4,4" BorderBrush="Black" BorderThickness="0,1,0,0" Tapped="Help_GotFocus" >
                         <TextBlock x:Name="tbEditToneHelpsHeading" Text="Help heading texts goes here..." VerticalAlignment="Center" HorizontalAlignment="Stretch"
                                    TextAlignment="Left" TextWrapping="WrapWholeWords" Tapped="Help_GotFocus" GotFocus="Help_GotFocus" /> 
                        </Grid>
                        <Grid Grid.Row="1" x:Name="EditSoundHelpsHeadingImage" Margin="4,0,4,4" Tapped="Help_GotFocus">
                            <Image x:Name="imgEditToneHeadingImage" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" Tapped="Help_GotFocus" />
                        </Grid>
                        <Grid Grid.Row="2" Margin="4,0,4,4" Tapped="Help_GotFocus">
                            <TextBlock x:Name="tbEditToneHelpsText" Text="Help texts goes here..." VerticalAlignment="Center" HorizontalAlignment="Stretch"
                                       TextAlignment="Left" TextWrapping="WrapWholeWords" Tapped="Help_GotFocus" />
                        </Grid>
                        <Grid Grid.Row="3" x:Name="EditSoundHelpsImage" Margin="4,0,4,4" Tapped="Help_GotFocus">
                            <Image x:Name="imgEditToneImage" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" Tapped="Help_GotFocus"/>
                        </Grid>
                        <Grid Grid.Row="4" x:Name="EditToneMainHelp" Margin="4,0,4,4" Tapped="Help_GotFocus">
                            <Button x:Name="btnEditToneMainHelp" Content="Show main help info for selected synthesizer type" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" Tapped="Help_GotFocus" />
                        </Grid>
                    </Grid>
                    <Grid Grid.Row="3" x:Name="ButtonsRow">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="*" />
                            <ColumnDefinition Width="*" />
                            <ColumnDefinition Width="*" />
                        </Grid.ColumnDefinitions>
                        <Button Grid.Column="0" x:Name="btnEditTone_Play" Margin="2,0,1,2" Content="Play" HorizontalAlignment="Stretch"
                                VerticalAlignment="Stretch" Click="btnEditTone_Play_Click" GotFocus="btnEditTone_Play_GotFocus" />
                        <Button Grid.Column="1" x:Name="btnEditTone_Reset" x:Uid="btnStudioSetReturn" Margin="1,0,1,2" Content="Reset" HorizontalAlignment="Stretch"
                                VerticalAlignment="Stretch" Click="btnEditTone_Reset_Click" GotFocus="btnEditTone_Reset_GotFocus" />
                        <Button Grid.Column="2" x:Name="btnEditTone_Return" x:Uid="btnStudioSetReturn" Margin="1,0,2,2" Content="Return" HorizontalAlignment="Stretch"
                                VerticalAlignment="Stretch" GotFocus="btnEditTone_Return_GotFocus" Click="btnEditTone_Return_Click" />
                    </Grid>
                </Grid>
            </Grid>
         */
        public void DrawToneEditorPage()
        {

            // Create all controls ------------------------------------------------------------------------

            // Main layout:
            EditTonesGrid = new Grid();
            EditTonesGrid.VerticalOptions = LayoutOptions.FillAndExpand;
            EditTonesLeftColumnGrid = new Grid();
            EditTonesLeftColumnGrid.VerticalOptions = LayoutOptions.FillAndExpand;
            for (Int32 i = 0; i < 17; i++)
            {
                EditTonesLeftColumnGrid.RowDefinitions.Add(new RowDefinition());
            }
            EditTonesRightColumnGrid = new Grid();
            EditTonesRightColumnGrid.VerticalOptions = LayoutOptions.FillAndExpand;
            for (Int32 i = 0; i < 17; i++)
            {
                EditTonesRightColumnGrid.RowDefinitions.Add(new RowDefinition());
            }

            ControlsGrid = new Grid();
            ControlsGrid.MinimumHeightRequest = UIHandler.minimumHeightRequest;
            ControlsGrid.MinimumWidthRequest = 1;
            ControlsGrid.VerticalOptions = LayoutOptions.FillAndExpand;
            HelpGrid = new Grid();
            HelpGrid.VerticalOptions = LayoutOptions.FillAndExpand;

            // Row 0 controls:
            cbEditTone_PartSelector = new ComboBox();
            cbEditTone_SynthesizerType = new ComboBox();
            tbEditTone_Instrument = new Button();

            // Row 1 controls:
            cbEditTone_ParameterPages = new ComboBox();
            cbEditTone_PartialSelector = new ComboBox();
            cbEditTone_InstrumentCategorySelector = new ComboBox();
            cbEditTone_KeySelector = new ComboBox();
            // We need columndefinitions i order to hide/show the controls above!
            gridSecondRow = new Grid();
            gridSecondRow.ColumnDefinitions = new ColumnDefinitionCollection();
            cdEditTone_ParameterPages = new ColumnDefinition();
            cdEditTone_PartialSelector = new ColumnDefinition();
            cdEditTone_InstrumentCategorySelector = new ColumnDefinition();
            cdEditTone_KeySelector = new ColumnDefinition();

            tbEditToneChorusSendLevel = new Label();
            slEditToneChorusSendLevel = new Slider();
            tbEditToneReverbSendLevel = new Label();
            slEditToneReverbSendLevel = new Slider();
            tbEditToneHelpsHeading = new Label(); // " Text="Help heading texts goes here..." VerticalAlignment="Center" HorizontalAlignment="Stretch"
            imgEditToneHeadingImage = new Image(); //" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" Tapped="Help_GotFocus" />
            //imgEditToneHeadingImage.VerticalOptions = LayoutOptions.FillAndExpand;
            tbEditToneHelpsText = new Label(); // " Text="Help texts goes here..." VerticalAlignment="Center" HorizontalAlignment="Stretch"
            //tbEditToneHelpsText.VerticalOptions = LayoutOptions.FillAndExpand;
            imgEditToneImage = new Image(); // " VerticalAlignment="Stretch" HorizontalAlignment="Stretch" Tapped="Help_GotFocus"/>
            //imgEditToneImage.VerticalOptions = LayoutOptions.FillAndExpand;
            btnEditToneMainHelp = new Button(); // " Content="Show main help info for selected synthesizer type" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" Tapped="Help_GotFocus" />
            //btnEditToneMainHelp.VerticalOptions = LayoutOptions.FillAndExpand;
            btnEditTone_Play = new Button(); // " Margin="2,0,1,2" Content="Play" HorizontalAlignment="Stretch"
            btnEditTone_Reset = new Button(); // " x:Uid="btnStudioSetReturn" Margin="1,0,1,2" Content="Reset" HorizontalAlignment="Stretch"
            btnEditTone_Return = new Button(); // " x:Uid="btnStudioSetReturn" Margin="1,0,2,2" Content="Return" HorizontalAlignment="Stretch"

            // Controls:
            for (byte i = 0; i < 16; i++)
            {
                cbEditTone_PartSelector.Items.Add("Part " + (i + 1).ToString());
            }
            cbEditTone_SynthesizerType.Items.Add("PCM synth tone");
            cbEditTone_SynthesizerType.Items.Add("PCM drum kit");
            cbEditTone_SynthesizerType.Items.Add("SuperNATURAL acoustic tone");
            cbEditTone_SynthesizerType.Items.Add("SuperNATURAL synth tone");
            cbEditTone_SynthesizerType.Items.Add("SuperNATURAL drum kit");
            tbEditTone_Instrument.IsEnabled = false;
            tbEditToneChorusSendLevel.Text = "Chorus send level";
            slEditToneChorusSendLevel.Minimum = 0;
            slEditToneChorusSendLevel.Maximum = 127;
            tbEditToneReverbSendLevel.Text = "Reverb send level";
            slEditToneReverbSendLevel.Minimum = 0;
            slEditToneReverbSendLevel.Maximum = 127;
            tbEditToneHelpsHeading.Text = "Help heading texts goes here...";
            tbEditToneHelpsText.Text = "Help texts goes here...\r\nWith multiple\r\nlines!";
            imgEditToneImage.Focused += Help_GotFocus;
            btnEditToneMainHelp.Text = "Show help for selected synthesizer type";
            btnEditTone_Play.Text = "Play";
            btnEditTone_Play.Clicked += Librarian_btnPlay_Clicked; // btnEditTone_Play_Click; It's all in the UIHandler class now, use the same handler.
            btnEditTone_Play.Focused += btnEditTone_Play_GotFocus;
            btnEditTone_Reset.Text = "Reset";
            btnEditTone_Reset.Clicked += btnEditTone_Reset_Click;
            btnEditTone_Reset.Focused += btnEditTone_Reset_GotFocus;
            btnEditTone_Return.Text = "Return";
            btnEditTone_Return.Clicked += btnEditTone_Return_Click;
            btnEditTone_Return.Focused += btnEditTone_Return_GotFocus;

            // Add handlers -------------------------------------------------------------------------------

            cbEditTone_PartSelector.SelectedIndexChanged += cbEditTone_PartSelector_SelectionChanged;
            cbEditTone_PartSelector.Focused += cbEditTone_PartSelector_GotFocus;
            cbEditTone_SynthesizerType.SelectedIndexChanged += cbEditTone_SynthesizerType_SelectionChanged;
            cbEditTone_SynthesizerType.Focused += cbEditTone_SynthesizerType_GotFocus;
            cbEditTone_ParameterPages.SelectedIndexChanged += cbEditTone_ParameterPages_SelectionChanged;
            cbEditTone_ParameterPages.Focused += cbEditTone_ParameterPages_GotFocus;
            cbEditTone_PartialSelector.SelectedIndexChanged += cbEditTone_PartialSelector_SelectionChanged;
            cbEditTone_PartialSelector.Focused += cbEditTone_PartialSelector_GotFocus;
            //tbEditTone_KeyName.TextChanged += tbEditTone_KeyName_TextChanged;
            //tbEditTone_KeyName.Focused += tbEditTone_KeyName_GotFocus;
            cbEditTone_KeySelector.SelectedIndexChanged += cbEditTone_KeySelector_SelectionChanged;
            cbEditTone_KeySelector.Focused += cbEditTone_KeySelector_GotFocus;
            cbEditTone_InstrumentCategorySelector.SelectedIndexChanged += cbEditTone_InstrumentCategorySelector_SelectionChanged;
            cbEditTone_InstrumentCategorySelector.Focused += cbEditTone_InstrumentCategorySelector_GotFocus;
            slEditToneChorusSendLevel.ValueChanged += slEditToneChorusSendLevel_ValueChanged;
            slEditToneChorusSendLevel.Focused += slEditToneChorusSendLevel_GotFocus;
            slEditToneReverbSendLevel.ValueChanged += slEditToneReverbSendLevel_ValueChanged;
            slEditToneReverbSendLevel.Focused += slEditToneReverbSendLevel_GotFocus;

            // Assemle main layout ------------------------------------------------------------------------
            // Assemle second row:
            gridSecondRow.Children.Clear();
            gridSecondRow.Children.Add((new GridRow(0, new View[] {cbEditTone_ParameterPages, cbEditTone_PartialSelector,
                cbEditTone_InstrumentCategorySelector, cbEditTone_KeySelector }, new byte[] { 127, 127, 127, 127 }, false, true, 1)).Row);

            // Assemle left column ------------------------------------------------------------------------

            EditTonesLeftColumnGrid.Children.Clear();
            EditTonesLeftColumnGrid.Children.Add((new GridRow(0, new View[] { cbEditTone_PartSelector, cbEditTone_SynthesizerType, tbEditTone_Instrument })).Row);
            EditTonesLeftColumnGrid.Children.Add((new GridRow(1, new View[] { gridSecondRow })).Row);
            EditTonesLeftColumnGrid.Children.Add((new GridRow(2, new View[] { ControlsGrid }, null, false, true, 15)).Row);

            // Assemle right column -----------------------------------------------------------------------

            Edit_RenderRightColumn(null);

            //EditTonesRightColumnGrid.Children.Clear();
            //EditTonesRightColumnGrid.Children.Add((new GridRow(0, new View[] { tbEditToneChorusSendLevel, slEditToneChorusSendLevel }, new byte[] { 2, 3 }, false, false, 1)).Row);
            //EditTonesRightColumnGrid.Children.Add((new GridRow(1, new View[] { tbEditToneReverbSendLevel, slEditToneReverbSendLevel }, new byte[] { 2, 3 }, false, false, 1)).Row);
            //EditTonesRightColumnGrid.Children.Add((new GridRow(16, new View[] { btnEditTone_Play, btnEditTone_Reset, btnEditToneMainHelp, btnEditTone_Return })).Row);

            // Assemle main layout ------------------------------------------------------------------------

            EditTonesGrid.Children.Clear();
            EditTonesGrid.Children.Add((new GridRow(0, new View[] { EditTonesLeftColumnGrid, EditTonesRightColumnGrid }, new byte[] { 3, 2 } )).Row);
            //EditTonesGrid.Children.Add((new GridRow(0, new View[] { EditTonesLeftColumnGrid })).Row);
            //EditTonesGrid.Children.Add((new GridRow(0, new View[] { EditTonesRightColumnGrid })).Row);

            // Create and asemble EditorStackLayout ------------------------------------------------------

            //Edit_StackLayout.MinimumHeightRequest = 1;
            //Edit_StackLayout.MinimumWidthRequest = 1;
            Edit_StackLayout = new StackLayout();
            Edit_StackLayout.BackgroundColor = colorSettings.Background;
            Edit_StackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            Edit_StackLayout.Children.Add((new GridRow(0, new View[] { EditTonesGrid })).Row);
        }


        private void Edit_RenderRightColumn(HelpItem helpItem)
        {
            // Get Helpitem values:
            byte row = 0;
            EditTonesRightColumnGrid.Children.Clear();
            //if (EditTonesRightColumnGrid.Children.Count > 3)
            //{
            //    EditTonesRightColumnGrid.Children.RemoveAt(2);
            //    EditTonesRightColumnGrid.Children.RemoveAt(2);
            //    EditTonesRightColumnGrid.Children.RemoveAt(2);
            //    EditTonesRightColumnGrid.Children.RemoveAt(2);
            //}
            EditTonesRightColumnGrid.Children.Add((new GridRow(row++, new View[] { tbEditToneChorusSendLevel, slEditToneChorusSendLevel }, new byte[] { 2, 3 }, false, true, 1)).Row);
            EditTonesRightColumnGrid.Children.Add((new GridRow(row++, new View[] { tbEditToneReverbSendLevel, slEditToneReverbSendLevel }, new byte[] { 2, 3 }, false, true, 1)).Row);
            if (helpItem != null)
            {
                tbEditToneHelpsHeading.Text = helpItem.Heading;
                imgEditToneHeadingImage.Source = helpItem.HeadingImage;
                tbEditToneHelpsText.Text = helpItem.Text;
                imgEditToneImage.Source = helpItem.Image;
                EditTonesRightColumnGrid.Children.Add((new GridRow(row++, new View[] { tbEditToneHelpsHeading }, null, false, false, helpItem.spaceForHeading)).Row);
                EditTonesRightColumnGrid.Children.Add((new GridRow(row += helpItem.spaceForHeading, new View[] { imgEditToneHeadingImage }, null, false, true, helpItem.spaceForHeadingImage)).Row);
                EditTonesRightColumnGrid.Children.Add((new GridRow(row += helpItem.spaceForHeadingImage, new View[] { tbEditToneHelpsText }, null, false, true, helpItem.spaceForText)).Row);
                EditTonesRightColumnGrid.Children.Add((new GridRow(row += helpItem.spaceForText, new View[] { imgEditToneImage }, null, false, true, helpItem.spaceForImage)).Row);
            }
            else
            {
                tbEditToneHelpsHeading.Text = "";
                imgEditToneHeadingImage.Source = null;
                tbEditToneHelpsText.Text = "Sorry, no help available";
                imgEditToneImage.Source = null;
                EditTonesRightColumnGrid.Children.Add((new GridRow(row++, new View[] { tbEditToneHelpsHeading }, null, false, true, 2)).Row);
                EditTonesRightColumnGrid.Children.Add((new GridRow(row += 2, new View[] { imgEditToneHeadingImage }, null, false, true, 6)).Row);
                EditTonesRightColumnGrid.Children.Add((new GridRow(row += 6, new View[] { tbEditToneHelpsText }, null, false, true, 2)).Row);
                EditTonesRightColumnGrid.Children.Add((new GridRow(row += 2, new View[] { imgEditToneImage }, null, false, true, 3)).Row);
            }

            while (row < MAX_ROWS - 1)
            {
                Grid dummy = new Grid();
                dummy.BackgroundColor = colorSettings.ControlBackground;
                ControlsGrid.Children.Add((new GridRow(row++, new View[] { dummy })).Row);
            }

            EditTonesRightColumnGrid.Children.Add((new GridRow(row++, new View[] { btnEditTone_Play, btnEditTone_Reset, btnEditToneMainHelp, btnEditTone_Return }, new byte[] { 1, 1, 5, 1 })).Row);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Tone editor handlers
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////private void btnEditTone_Return_GotFocus(object sender, FocusEventArgs e)
        ////{
        ////    //throw new NotImplementedException();
        ////}

        private void btnEditTone_Return_Click(object sender, EventArgs e)
        {
            Edit_StackLayout.IsVisible = false;
            ShowLibrarianPage();
        }

        ////private void btnEditTone_Reset_GotFocus(object sender, FocusEventArgs e)
        ////{
        ////    //throw new NotImplementedException();
        ////}

        //private void btnEditTone_Reset_Click(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        ////private void btnEditTone_Play_GotFocus(object sender, FocusEventArgs e)
        ////{
        ////    //throw new NotImplementedException();
        ////}

        //private void btnEditTone_Play_Click(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        ////private void Help_GotFocus(object sender, FocusEventArgs e)
        ////{
        ////    //throw new NotImplementedException();
        ////}

        //private void HelpGrid_Focused(object sender, FocusEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        ////private void slEditToneReverbSendLevel_GotFocus(object sender, FocusEventArgs e)
        ////{
        ////    //throw new NotImplementedException();
        ////}

        //private void slEditToneReverbSendLevel_ValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        ////private void slEditToneChorusSendLevel_GotFocus(object sender, FocusEventArgs e)
        ////{
        ////    //throw new NotImplementedException();
        ////}

        //private void slEditToneChorusSendLevel_ValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        ////private void cbEditTone_InstrumentCategorySelector_GotFocus(object sender, FocusEventArgs e)
        ////{
        ////    //throw new NotImplementedException();
        ////}

        //private void cbEditTone_InstrumentCategorySelector_SelectionChanged(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        ////private void cbEditTone_KeySelector_GotFocus(object sender, FocusEventArgs e)
        ////{
        ////    //throw new NotImplementedException();
        ////}

        //private void cbEditTone_KeySelector_SelectionChanged(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        ////private void tbEditTone_KeyName_GotFocus(object sender, FocusEventArgs e)
        ////{
        ////    //throw new NotImplementedException();
        ////}

        ////private void tbEditTone_KeyName_TextChanged(object sender, TextChangedEventArgs e)
        ////{
        ////    //throw new NotImplementedException();
        ////}

        ////private void cbEditTone_PartialSelector_GotFocus(object sender, FocusEventArgs e)
        ////{
        ////    //throw new NotImplementedException();
        ////}

        //private void cbEditTone_PartialSelector_SelectionChanged(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        ////private void cbEditTone_ParameterPages_GotFocus(object sender, FocusEventArgs e)
        ////{
        ////    //throw new NotImplementedException();
        ////}

        //private void cbEditTone_ParameterPages_SelectionChanged(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        ////private void cbEditTone_SynthesizerType_GotFocus(object sender, FocusEventArgs e)
        ////{
        ////    if (handleControlEvents)
        ////    {
        ////    }
        ////}

        //private async void cbEditTone_SynthesizerType_SelectionChangedAsync(object sender, EventArgs e)
        //{
        //    //t.Trace("private void cbEditTone_SynthesizerType_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
        //    if (initDone && handleControlEvents)
        //    {
        //        if (cbEditTone_SynthesizerType.SelectedIndex != currentProgramIndex)
        //        {
        //            // Changing program type means that we have incorrect data read into the controls, and that some classes are not filled out.
        //            // Doing this will mean that we will have to fetch default data for all classes used by the new program type.
        //            //MessageDialog warning = new MessageDialog("Changing Tone Type results in all settings to be reset. Are you sure this is what you want to do?");
        //            //warning.Title = "Warning!";
        //            //warning.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
        //            //warning.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
        //            String response = await mainPage.DisplayActionSheet("Changing Tone Type results in all settings to be reset. Are you sure this is what you want to do?", null, null, new String[] { "No", "Yes" });
        //            //var response = await warning.ShowAsync();
        //            //if ((Int32)response.Id == 0)
        //            if (response == "Yes")
        //            {
        //                Reset();
        //            }
        //            else
        //            {
        //                cbEditTone_SynthesizerType.SelectedIndex = currentProgramIndex;
        //            }
        //        }
        //    }
        //}

        //private void cbEditTone_PartSelector_GotFocus(object sender, FocusEventArgs e)
        //{
        //    if (handleControlEvents)
        //    {
        //    }
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Tone editor functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ShowToneEditorPage()
        {
            Page = _page.EDIT_TONE;
            if (!Edit_IsCreated)
            {
                PushHandleControlEvents();
                DrawToneEditorPage();
                mainStackLayout.Children.Add(Edit_StackLayout);
                Edit_IsCreated = true;
                PopHandleControlEvents();
                Edit_Init();
                //Edit_StartTimer();
            }
            cbEditTone_PartSelector.SelectedIndex = commonState.CurrentPart;
            // Current tone is in commonState.currentTone but we also need all parameters, 
            // so use QueryToneType which in turn will read all parameters:
            QueryToneType();
            ShowToneTypeDependentControls();
            Edit_StackLayout.IsVisible = true;
        }

        //private void ToneEdit_NotYetImplemented_Clicked(object sender, EventArgs e)
        //{
        //    EditorStackLayout.IsVisible = false;
        //    ShowLibrarianPage();
        //}

        //private void cbEditTone_PartSelector_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (handleControlEvents)
        //    {
        //    }
        //}

        private void ShowToneTypeDependentControls()
        {
            //commonState.SimpleToneType = CommonState.SimpleToneTypes.PCM_SYNTH_TONE;

            switch (commonState.SimpleToneType)
            {
                case CommonState.SimpleToneTypes.PCM_SYNTH_TONE:
                    cbEditTone_InstrumentCategorySelector.IsVisible = true;
                    cdEditTone_InstrumentCategorySelector.Width = new GridLength(255, GridUnitType.Star);
                    cbEditTone_PartialSelector.IsVisible = true;
                    cdEditTone_PartialSelector.Width = new GridLength(255, GridUnitType.Star);
                    cbEditTone_KeySelector.IsVisible = false;
                    cdEditTone_KeySelector.Width = new GridLength(1, GridUnitType.Star);
                    cbEditTone_PartialSelector.Items.Clear();
                    cbEditTone_PartialSelector.Items.Add("Partial 1");
                    cbEditTone_PartialSelector.Items.Add("Partial 2");
                    cbEditTone_PartialSelector.Items.Add("Partial 3");
                    cbEditTone_PartialSelector.Items.Add("Partial 4");
                    break;
                case CommonState.SimpleToneTypes.SUPERNATURAL_SYNTH_TONE:
                    cbEditTone_InstrumentCategorySelector.IsVisible = true;
                    cdEditTone_InstrumentCategorySelector.Width = new GridLength(255, GridUnitType.Star);
                    cbEditTone_PartialSelector.IsVisible = true;
                    cdEditTone_PartialSelector.Width = new GridLength(255, GridUnitType.Star);
                    cbEditTone_KeySelector.IsVisible = false;
                    cdEditTone_KeySelector.Width = new GridLength(1, GridUnitType.Star);
                    cbEditTone_PartialSelector.Items.Clear();
                    cbEditTone_PartialSelector.Items.Add("Partial 1");
                    cbEditTone_PartialSelector.Items.Add("Partial 2");
                    cbEditTone_PartialSelector.Items.Add("Partial 3");
                    break;
                case CommonState.SimpleToneTypes.SUPERNATURAL_ACOUSTIC_TONE:
                    cbEditTone_InstrumentCategorySelector.IsVisible = true;
                    cdEditTone_InstrumentCategorySelector.Width = new GridLength(255, GridUnitType.Star);
                    cbEditTone_PartialSelector.IsVisible = false;
                    cdEditTone_PartialSelector.Width = new GridLength(1, GridUnitType.Star);
                    cbEditTone_KeySelector.IsVisible = false;
                    cdEditTone_KeySelector.Width = new GridLength(1, GridUnitType.Star);
                    break;
                case CommonState.SimpleToneTypes.PCM_DRUM_KIT:
                case CommonState.SimpleToneTypes.SUPERNATURAL_DRUM_KIT:
                    cbEditTone_InstrumentCategorySelector.IsVisible = false;
                    cdEditTone_InstrumentCategorySelector.Width = new GridLength(1, GridUnitType.Star);
                    cbEditTone_PartialSelector.IsVisible = true;
                    cdEditTone_PartialSelector.Width = new GridLength(255, GridUnitType.Star);
                    cbEditTone_KeySelector.IsVisible = true;
                    cdEditTone_KeySelector.Width = new GridLength(255, GridUnitType.Star);
                    break;
            }
        }

        //private void Waiting(Boolean on)
        //{
        //    t.Trace("private void Waiting(" + on.ToString() + ")");
        //    if (on)
        //    {
        //        Window.Current.CoreWindow.PointerCursor =
        //            new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 1);
        //    }
        //    else
        //    {
        //        Window.Current.CoreWindow.PointerCursor =
        //            new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        //    }
        //}
        
//        byte[] rawData;

        //public Edit()
        //{
        //    t.Trace("public Edit()");
        //    this.InitializeComponent();
        //    edit_State = Edit_State.INIT;
        //    timer = new DispatcherTimer();
        //    timer.Interval = TimeSpan.FromMilliseconds(100);
        //    timer.Tick += Timer_Tick;
        //    timer.Start();
        //}

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    try
        //    {
        //        commonState = (CommonState)e.Parameter;
        //        if (firstTime)
        //        {
        //            commonState.midi.midiInPort.MessageReceived += Edit_MidiInPort_MessageReceived;
        //            firstTime = false;
        //        }
        //        commonState.player.btnPlayStop = btnEditTone_Play;
        //        if (commonState.player.Playing)
        //        {
        //            btnEditTone_Play.Content = "Stop";
        //        }
        //        commonState.reactToMidiIn = CommonState.ReactToMidiIn.EDIT;
        //        Edit_Init();
        //    }
        //    catch { }
        //}

        private void Edit_Init()
        {
            t.Trace("private void Edit_Init()");
            try
            {
                //commonState = (CommonState)e.Parameter;
                //if (firstTime)
                //{
                //    //commonState.midi.midiInPort.MessageReceived += Edit_MidiInPort_MessageReceived;
                //    firstTime = false;
                //}
                commonState.Player.btnPlayStop = btnEditTone_Play;
                if (commonState.Player.Playing)
                {
                    btnEditTone_Play.Content = "Stop";
                }
                //commonState.reactToMidiInAndTimerTick = CommonState.ReactToMidiInAndTimerTick.EDIT;
            }
            catch { }
            Waiting(true, "Working...", Edit_StackLayout);
            edit_State = Edit_State.NONE;
            //blackBorder = new SolidColorBrush(Windows.UI.Color.FromArgb(255,0,0,0));
            //borderThickness = new Thickness(1);
            //margin2022 = new Thickness(2, 2, 2, 2);
            //margin2222 = new Thickness(2);
            toneLengths = parameterSets.GetNumberedParameter(PARAMETER_TYPE.COMBOBOX_NOTE_LENGTH);
            keyNames = parameterSets.GetNumberedParameter(PARAMETER_TYPE.COMBOBOX_TONE_NAMES);
            Help = new Help(tbEditToneHelpsHeading, imgEditToneHeadingImage, tbEditToneHelpsText, imgEditToneImage);
            btnEditTone_SaveTone.IsEnabled = !commonState.VenderDriverIsInstalled;
            btnEditTone_DeleteTone.IsEnabled = !commonState.VenderDriverIsInstalled;
            btnEditTone_Play.IsEnabled = !commonState.VenderDriverIsInstalled;
            //localSettings = ApplicationData.Current.LocalSettings;
            //cbEditTone_PartSelector.SelectedIndex = commonState.CurrentPart;
            //QueryToneType();
        }

        #region MIDI communication handlers
        /// <summary>
        /// The midi class is marhalled in a different thread, so Edit_MidiInPort_MessageReceived belongs
        /// to another thread than the xaml controls, and cannot update those controls.
        /// Instead, an Edit_State.DONE set in Edit_MidiInPort_MessageReceived when read data has been
        /// inserted into a class (one that holds the data for the controls in question) will allow
        /// Timer_Tick to update the xaml controls according to the data in the class.
        /// Enumeration currentMidiRequest tells Timer_Tick what class and controls to use.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void Timer_Tick(object sender, object e)
        //private void Edit_StartTimer()
        //{
        //    stopEditTimer = false;
        //    Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
        //    {
        //        //if (commonState.reactToMidiIn != CommonState.ReactToMidiIn.EDIT)
        //        //{
        //        //    return;
        //        //}
        //        //t.Trace("private void Timer_Tick (" + "object" + sender + ", " + "object" + e + ", " + ")");
        //        if (stopEditTimer)
        //        {
        //            return false;
        //        }
        private void Edit_Timer_Tick()
        {
            if (!initDone)
            {
                if (edit_State == Edit_State.UPDATE_CONTROLS)
                {
                    //switch (currentProgramType)
                    //{
                    //    case ProgramType.PCM_SYNTH_TONE:
                    //        try { cdEditTone_PartialSelector.Width = new GridLength(1, GridUnitType.Star); } catch (Exception e2) { }
                    //        break;
                    //    case ProgramType.PCM_DRUM_KIT:
                    //        try { cdEditTone_PartialSelector.Width = new GridLength(1, GridUnitType.Star); } catch { }
                    //        break;
                    //    case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                    //        try { cdEditTone_PartialSelector.Width = new GridLength(1, GridUnitType.Star); } catch { }
                    //        break;
                    //    case ProgramType.SUPERNATURAL_SYNTH_TONE:
                    //        try { cdEditTone_PartialSelector.Width = new GridLength(1, GridUnitType.Star); } catch { }
                    //        break;
                    //    case ProgramType.SUPERNATURAL_DRUM_KIT:
                    //        try { cdEditTone_PartialSelector.Width = new GridLength(0, GridUnitType.Star); } catch { }
                    //        break;
                    //}
                    slEditToneChorusSendLevel.Value = chorusSendLevel;
                    slEditToneReverbSendLevel.Value = reverbSendLevel;
                    tbEditToneChorusSendLevel.Text = "Chorus send level: " + chorusSendLevel.ToString();
                    tbEditToneReverbSendLevel.Text = "Reverb send level: " + reverbSendLevel.ToString();
                    EditTone_UpdateControls();
                    edit_State = Edit_State.DONE;
                    initDone = true;
                    Waiting(false, "", Edit_StackLayout);
                }
                // If the user tries to enter Edit Tone when selected a voice in an expansion
                // that has not been loaded, the I-7 will not respond (it probably did when
                // user selected the sound, but that was in the Main Page, and the Main Page
                // does not listen for responses.
                // So, we end up here, but the current edit_State is WAITING and the currentMidiRequest
                // is QUERYING_(type)_COMMON and it has been so for loong. Catch it here:
                if (edit_State == Edit_State.WAITING)
                {
                    waitingForResponseFromIntegra7++;
                    if (waitingForResponseFromIntegra7 > 150)
                    {
                        //timer.Stop();
                        Waiting(false, "", Edit_StackLayout);
                        IsModuleLoaded();
                    }
                }
                else
                {
                    waitingForResponseFromIntegra7 = 0;
                }
            }
            else
            {
                if (edit_State == Edit_State.DONE && currentEditMidiRequest == Edit_CurrentMidiRequest.NONE)
                {
                    edit_State = Edit_State.NONE;
                }
                else if (edit_State == Edit_State.NONE && currentEditMidiRequest == Edit_CurrentMidiRequest.NONE)
                {
                    PopHandleControlEvents();
                }
                else
                {
                    switch (currentProgramType)
                    {
                        case ProgramType.PCM_SYNTH_TONE:
                            if (edit_State == Edit_State.UPDATE_CONTROLS && currentEditMidiRequest == Edit_CurrentMidiRequest.NONE)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                EditTone_UpdateControls();
                            }
                            if (edit_State == Edit_State.DONE && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_COMMON_MFX)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                EditTone_UpdateControls();
                            }
                            else if (edit_State == Edit_State.DONE && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_PARTIAL)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                EditTone_UpdateControls();
                            }
                            break;
                        case ProgramType.PCM_DRUM_KIT:
                            if (edit_State == Edit_State.UPDATE_CONTROLS && currentEditMidiRequest == Edit_CurrentMidiRequest.NONE)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                MakePCMDrumKitControls();// (byte)(currentKey - 21));
                            }
                            else if (edit_State == Edit_State.DONE && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_PARTIAL)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                UpdatePCMDrumKitControls();
                            }
                            else if (edit_State == Edit_State.DONE && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_COMMON_MFX)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                UpdatePCMDrumKitControls();
                            }
                            break;
                        case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                            if (edit_State == Edit_State.UPDATE_CONTROLS && currentEditMidiRequest == Edit_CurrentMidiRequest.NONE)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                UpdateSuperNATURALAcousticToneControls();// (byte)(currentKey - 21));
                            }
                            else if (edit_State == Edit_State.DONE && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_ACOUSTIC_TONE_COMMON_MFX)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                UpdateSuperNATURALAcousticToneControls();
                            }
                            break;
                        case ProgramType.SUPERNATURAL_SYNTH_TONE:
                            if (edit_State == Edit_State.UPDATE_CONTROLS && currentEditMidiRequest == Edit_CurrentMidiRequest.NONE)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                UpdateSuperNATURALSynthToneControls();// (byte)(currentKey - 21));
                            }
                            else if (edit_State == Edit_State.DONE && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_ACOUSTIC_TONE_WAVE)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                UpdateSuperNATURALSynthToneControls();
                            }
                            else if (edit_State == Edit_State.DONE && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_PARTIAL)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                UpdateSuperNATURALSynthToneControls();
                            }
                            else if (edit_State == Edit_State.DONE && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_COMMON_MFX)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                UpdateSuperNATURALSynthToneControls();
                            }
                            break;
                        case ProgramType.SUPERNATURAL_DRUM_KIT:
                            if (edit_State == Edit_State.UPDATE_CONTROLS && currentEditMidiRequest == Edit_CurrentMidiRequest.NONE)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                UpdateSuperNATURALDrumKitControls();// (byte)(currentKey - 27));
                            }
                            else if (edit_State == Edit_State.DONE && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_DRUM_KIT_PARTIAL)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                UpdateSuperNATURALDrumKitControls();
                            }
                            else if (edit_State == Edit_State.DONE && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_DRUM_KIT_COMMON_MFX)
                            {
                                edit_State = Edit_State.NONE;
                                currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                                UpdateSuperNATURALDrumKitControls();
                            }
                            break;
                    }
                }
            }
        }
        //        return true;
        //    });
        //}

        private async void IsModuleLoaded()
        {
            if (currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_ACOUSTIC_TONE_COMMON)
            {
                //MessageDialog warning = new MessageDialog("There is no response from your INTEGRA-7. Is the selected module loaded?");
                //warning.Title = "Problem!";
                //warning.Commands.Add(new UICommand { Label = "Back to Librarian", Id = 0 });
                //var response = await warning.ShowAsync();

                String response = await mainPage.DisplayActionSheet("There is no response from your INTEGRA-7. Is the selected module loaded?",
                    null, null, new String[] { "Back to Librarian" });

                commonState.CurrentTone = null; // To force re-read of data.
                //commonState.midi.midiInPort.MessageReceived -= Edit_MidiInPort_MessageReceived;
                //this.Frame.Navigate(typeof(MainPage), commonState);
                Edit_StackLayout.IsVisible = false;
                ShowLibrarianPage();
            }
            else
            {
                //commonState.midi.midiInPort.MessageReceived -= Edit_MidiInPort_MessageReceived;
                //this.Frame.Navigate(typeof(CommunicationError), commonState);
                Edit_StackLayout.IsVisible = false;
                ShowLibrarianPage(); // TODO: Add an error page and show
            }
        }

        private void QueryIntegra7()
        {
            t.Trace("private void QueryIntegra7()");
            switch (currentProgramType)
            {
                case ProgramType.PCM_SYNTH_TONE:
                    QueryPCMSynthToneCommonMFX();
                    break;
                case ProgramType.PCM_DRUM_KIT:
                    QueryPCMDrumKitCommonMFX();
                    break;
                case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                    QuerySuperNATURALAcousticToneCommonMFX();
                    break;
                case ProgramType.SUPERNATURAL_SYNTH_TONE:
                    QuerySuperNATURALSynthToneCommonMFX();
                    break;
                case ProgramType.SUPERNATURAL_DRUM_KIT:
                    QuerySuperNATURALDrumKitCommonMFX();
                    break;
            }
        }

        private void EditTone_UpdateControls()
        {
            t.Trace("private void EditTone_UpdateControls()");
            //currentHandleControlEvents = handleControlEvents;
            //PushHandleControlEvents();
            if (!initDone)
            {
                UpdateParameterPagesSelector();
                Update_PartialSelector();
                UpdateInstrumentSelector();
            }
            cbEditTone_InstrumentCategorySelector.SelectedItem = commonState.CurrentTone.Category;
            switch (currentProgramType)
            {
                case ProgramType.PCM_SYNTH_TONE:
                    tbEditTone_Instrument.Content =
                        selectedSound.ProgramBank + ": " + pCMSynthTone.pCMSynthToneCommon.Name;
                    cbEditTone_SynthesizerType.SelectedIndex = 0;
                    cbEditTone_InstrumentCategorySelector.Visibility = Visibility.Visible;
                    cbEditTone_PartialSelector.Visibility = Visibility.Visible;
                    //cdEditTone_PartialSelector.Width = new GridLength(1, GridUnitType.Star);
                    MakePCMSynthToneControls();
                    break;
                case ProgramType.PCM_DRUM_KIT:
                    tbEditTone_Instrument.Content =
                        selectedSound.ProgramBank + ": " + pCMDrumKit.pCMDrumKitCommon.Name;
                    //currentKey = 21;
                    cbEditTone_SynthesizerType.SelectedIndex = 1;
                    cbEditTone_InstrumentCategorySelector.Visibility = Visibility.Collapsed;
                    cbEditTone_PartialSelector.Visibility = Visibility.Visible;
                    //cdEditTone_PartialSelector.Width = new GridLength(1, GridUnitType.Star);
                    MakePCMDrumKitControls();
                    break;
                case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                    tbEditTone_Instrument.Content =
                        selectedSound.ProgramBank + ": " + superNATURALAcousticTone.superNATURALAcousticToneCommon.Name;
                    cbEditTone_SynthesizerType.SelectedIndex = 2;
                    cbEditTone_InstrumentCategorySelector.Visibility = Visibility.Visible;
                    cbEditTone_PartialSelector.Visibility = Visibility.Collapsed;
                    //cdEditTone_PartialSelector.Width = new GridLength(0, GridUnitType.Star);
                    UpdateSuperNATURALAcousticToneControls();
                    break;
                case ProgramType.SUPERNATURAL_SYNTH_TONE:
                    tbEditTone_Instrument.Content =
                        selectedSound.ProgramBank + ": " + superNATURALSynthTone.superNATURALSynthToneCommon.Name;
                    cbEditTone_SynthesizerType.SelectedIndex = 3;
                    cbEditTone_InstrumentCategorySelector.Visibility = Visibility.Visible;
                    cbEditTone_PartialSelector.Visibility = Visibility.Visible;
                    //cdEditTone_PartialSelector.Width = new GridLength(1, GridUnitType.Star);
                    UpdateSuperNATURALSynthToneControls();
                    break;
                case ProgramType.SUPERNATURAL_DRUM_KIT:
                    tbEditTone_Instrument.Content =
                        selectedSound.ProgramBank + ": " + superNATURALDrumKit.superNATURALDrumKitCommon.Name;
                    //currentKey = 27;
                    cbEditTone_SynthesizerType.SelectedIndex = 4;
                    cbEditTone_InstrumentCategorySelector.Visibility = Visibility.Collapsed;
                    cbEditTone_PartialSelector.Visibility = Visibility.Collapsed;
                    //cdEditTone_PartialSelector.Width = new GridLength(0, GridUnitType.Star);
                    UpdateSuperNATURALDrumKitControls();
                    break;
            }
            Edit_RenderRightColumn(Help.Show(currentProgramIndex, 0, 0, 0));
            try
            {
                ((Button)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
            }
            catch
            {
                try
                {
                    ((ComboBox)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                }
                catch
                {
                    try
                    {
                        ((Slider)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                    }
                    catch
                    {
                        try
                        {
                            ((Switch)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                        }
                        catch
                        {
                            try
                            {
                                ((LabeledSwitch)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                            }
                            catch
                            {
                                try
                                {
                                    ((ListView)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                                }
                                catch
                                {
                                    try
                                    {
                                        ((Picker)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            ((LabeledPicker)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                ((Label)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    ((Editor)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                                                }
                                                catch
                                                {
                                                    try
                                                    {
                                                        ((Image)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                                                    }
                                                    catch
                                                    {
                                                        try
                                                        {
                                                            ((LabeledText)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                                                        }
                                                        catch
                                                        {
                                                            try
                                                            {
                                                                ((TextBox)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                                                            }
                                                            catch
                                                            {
                                                                try
                                                                {
                                                                    ((LabeledTextInput)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                                                                }
                                                                catch
                                                                {
                                                                    try
                                                                    {
                                                                        ((Grid)((Grid)((Grid)ControlsGrid.Children[0]).Children[0]).Children[0]).Focus();
                                                                    }
                                                                    catch
                                                                    {
                                                                        t.Trace("Could not find type on line 1629");
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //handleControlEvents = currentHandleControlEvents;
        }

        // TODO: Compare to earlier solutions for different MIDI reception using a variabla and a timer
        private void Edit_MidiInPort_MessageReceived()
        {
            //if (commonState.reactToMidiInAndTimerTick != CommonState.ReactToMidiInAndTimerTick.EDIT)
            //{
            //    return;
            //}
            t.Trace("private void Edit_MidiInPort_MessageReceived ()");
            byte temp = currentKey;
            //IMidiMessage receivedMidiMessage = args.Message;
            //rawData = receivedMidiMessage.RawData.ToArray();
            //if (receivedMidiMessage.Type == MidiMessageType.SystemExclusive)
            {
                currentKey = temp;
                if (!initDone)
                {
                    if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_TONE_TYPE)
                    {
                        selectedSound = new SelectedTone(rawData[17], rawData[18], rawData[19]);
                        //commonState.currentTone = commonState.toneList.Get(rawData[17], rawData[18], rawData[19]);
                        //commonState.currentTone = new Tone(rawData[17], rawData[18], rawData[19]);
                        currentProgramType = selectedSound.ProgramType;
                        switch (currentProgramType)
                        {
                            case ProgramType.PCM_SYNTH_TONE:
                                currentProgramIndex = 0;
                                currentParameterPageIndex = 0;
                                currentPartial = 0;
                                QueryPCMSynthToneCommon();
                                break;
                            case ProgramType.PCM_DRUM_KIT:
                                currentKey = 17;
                                currentProgramIndex = 1;
                                currentParameterPageIndex = 0;
                                currentPartial = 0;
                                QueryPCMDrumKitCommon();
                                break;
                            case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                                currentProgramIndex = 2;
                                currentParameterPageIndex = 0;
                                currentPartial = 0;
                                QuerySuperNATURALAcousticTone();
                                break;
                            case ProgramType.SUPERNATURAL_SYNTH_TONE:
                                currentProgramIndex = 3;
                                currentParameterPageIndex = 0;
                                currentPartial = 0;
                                QuerySuperNATURALSynthToneUndocumentedParameters();
                                break;
                            case ProgramType.SUPERNATURAL_DRUM_KIT:
                                currentKey = 9;
                                currentProgramIndex = 4;
                                currentParameterPageIndex = 0;
                                currentPartial = 0;
                                QuerySuperNATURALDrumKit();
                                break;
                        }
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_COMMON)
                    {
                        pCMSynthTone = new PCMSynthTone(new ReceivedData(rawData));
                        QueryPCMSynthToneCommonMFX();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_COMMON_MFX)
                    {
                        commonMFX = new CommonMFX(new ReceivedData(rawData));
                        SetMFXTypeAndOffset(commonMFX.MFXType);
                        if (mfxPageReadFromIntegra7 == 0xff)
                        {
                            mfxPageReadFromIntegra7 = commonMFX.MFXType;
                        }
                        QueryPCMSynthTonePMT();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_PMT)
                    {
                        pCMSynthTone.pCMSynthTonePMT = new PCMSynthTonePMT(new ReceivedData(rawData));
                        currentPartial = 0;
                        QueryPCMSynthTonePartial();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_PARTIAL)
                    {
                        pCMSynthTone.pCMSynthTonePartial[currentPartial] = new PCMSynthTonePartial(new ReceivedData(rawData));
                        if (++currentPartial < 4)
                        {
                            QueryPCMSynthTonePartial();
                        }
                        else
                        {
                            currentPartial = 0;
                            QueryPCMSynthToneCommon2();
                        }
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_COMMON2)
                    {
                        pCMSynthTone.pCMSynthToneCommon2 = new PCMSynthToneCommon2(new ReceivedData(rawData));
                        QueryStudioSetSendParameters();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_COMMON)
                    {
                        pCMDrumKit = new PCMDrumKit(new ReceivedData(rawData));
                        QueryPCMDrumKitCommonMFX();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_COMMON_MFX)
                    {
                        commonMFX = new CommonMFX(new ReceivedData(rawData));
                        if (mfxPageReadFromIntegra7 == 0xff)
                        {
                            mfxPageReadFromIntegra7 = commonMFX.MFXType;
                        }
                        QueryPCMDrumKitCommonCompEq();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_COMMON_COMP_EQ)
                    {
                        pCMDrumKit.pCMDrumKitCommonCompEQ = new PCMDrumKitCommonCompEQ(new ReceivedData(rawData));
                        currentKey = 0;
                        QueryPCMDrumKitPartial();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_PARTIAL)
                    {
                        pCMDrumKit.pCMDrumKitPartial[currentKey] = new PCMDrumKitPartial(new ReceivedData(rawData));
                        currentKey++;
                        if (currentKey < 88)
                        {
                            waitingForResponseFromIntegra7 = 0;
                            t.Trace("*** " + DateTime.Now.TimeOfDay.Milliseconds.ToString());
                            QueryPCMDrumKitPartial();
                            t.Trace("*** " + DateTime.Now.TimeOfDay.Milliseconds.ToString());
                        }
                        else
                        {
                            currentKey = 0;
                            QueryPCMDrumKitCommon2();
                        }
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_COMMON2)
                    {
                        pCMDrumKit.pCMDrumKitCommon2 = new PCMDrumKitCommon2(new ReceivedData(rawData));
                        QueryStudioSetSendParameters();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_ACOUSTIC_TONE_COMMON)
                    {
                        superNATURALAcousticTone = new SuperNATURALAcousticTone(new ReceivedData(rawData));
                        QuerySuperNATURALAcousticToneCommonMFX();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_ACOUSTIC_TONE_COMMON_MFX)
                    {
                        commonMFX = new CommonMFX(new ReceivedData(rawData));
                        if (mfxPageReadFromIntegra7 == 0xff)
                        {
                            mfxPageReadFromIntegra7 = commonMFX.MFXType;
                        }
                        currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                        edit_State = Edit_State.UPDATE_CONTROLS;
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_UNDOCUMENTED_PARAMETERS)
                    {
                        undocumented_Parameters = new Undocumented_Parameters(new ReceivedData(rawData));
                        QuerySuperNATURALSynthToneCommon();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_COMMON)
                    {
                        superNATURALSynthTone = new SuperNATURALSynthTone(new ReceivedData(rawData));
                        QuerySuperNATURALSynthToneCommonMFX();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_COMMON_MFX)
                    {
                        commonMFX = new CommonMFX(new ReceivedData(rawData));
                        if (mfxPageReadFromIntegra7 == 0xff)
                        {
                            mfxPageReadFromIntegra7 = commonMFX.MFXType;
                        }
                        currentPartial = 0;
                        QuerySuperNATURALSynthTonePartial();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_PARTIAL)
                    {
                        superNATURALSynthTone.superNATURALSynthTonePartial[currentKey] = new SuperNATURALSynthTonePartial(new ReceivedData(rawData));
                        currentKey++;
                        if (currentKey < 3)
                        {
                            QuerySuperNATURALSynthTonePartial();
                        }
                        else
                        {
                            currentKey = 0;
                            QuerySuperNATURALSynthToneMisc();
                        }
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_MISC)
                    {
                        superNATURALSynthTone.superNATURALSynthToneMisc = new SuperNATURALSynthToneMisc(new ReceivedData(rawData));
                        // Envelope Loop Sync Note is in Undocumented_Parameters.Data_05:
                        superNATURALSynthTone.superNATURALSynthToneMisc.EnvelopeLoopSyncNote = undocumented_Parameters.Data_00;
                        QueryStudioSetSendParameters();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_DRUM_KIT_COMMON)
                    {
                        superNATURALDrumKit = new SuperNATURALDrumKit(new ReceivedData(rawData));
                        QuerySuperNATURALDrumKitCommonMFX();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_DRUM_KIT_COMMON_MFX)
                    {
                        commonMFX = new CommonMFX(new ReceivedData(rawData));
                        if (mfxPageReadFromIntegra7 == 0xff)
                        {
                            mfxPageReadFromIntegra7 = commonMFX.MFXType;
                        }
                        QuerySuperNATURALDrumKitCommonCompEQ();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_DRUM_KIT_COMMON_COMP_EQ)
                    {
                        superNATURALDrumKit.superNATURALDrumKitCommonCompEQ = new SuperNATURALDrumKitCommonCompEQ(new ReceivedData(rawData));
                        currentKey = 0;
                        QuerySuperNATURALDrumKitPartial();
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_DRUM_KIT_PARTIAL)
                    {
                        superNATURALDrumKit.superNATURALDrumKitKey[currentKey] = new SuperNATURALDrumKitKey(new ReceivedData(rawData));
                        currentKey++;
                        if (currentKey < 62)
                        {
                            QuerySuperNATURALDrumKitPartial();
                        }
                        else
                        {
                            currentKey = 0;
                            QueryStudioSetSendParameters();
                        }
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_STUDIO_SET_SEND_PARAMETERS)
                    {
                        chorusSendLevel = rawData[11];
                        reverbSendLevel = rawData[12];
                        currentEditMidiRequest = Edit_CurrentMidiRequest.NONE;
                        edit_State = Edit_State.UPDATE_CONTROLS;
                    }
                }
                else
                {
                    // Temporary code to catch Inst Variation, Inst Number and Moddify Parameters from I-7
                    //result += hexString(rawData) + "\r\n";
                    //return;
                    currentKey = temp;
                    if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_PARTIAL)
                    {
                        //pCMDrumKit.pCMDrumKitPartial = new PCMDrumKitPartial(new ReceivedData(rawData));
                        edit_State = Edit_State.DONE;
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_ACOUSTIC_TONE_COMMON_MFX)
                    {
                        commonMFX = new CommonMFX(new ReceivedData(rawData));
                        edit_State = Edit_State.DONE;
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_PARTIAL)
                    {
                        pCMSynthTone.pCMSynthTonePartial[currentPartial] = new PCMSynthTonePartial(new ReceivedData(rawData));
                        edit_State = Edit_State.DONE;
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_COMMON_MFX)
                    {
                        commonMFX = new CommonMFX(new ReceivedData(rawData));
                        edit_State = Edit_State.DONE;
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_COMMON_MFX)
                    {
                        commonMFX = new CommonMFX(new ReceivedData(rawData));
                        edit_State = Edit_State.DONE;
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_PARTIAL)
                    {
                        superNATURALSynthTone.superNATURALSynthTonePartial[currentKey] = new SuperNATURALSynthTonePartial(new ReceivedData(rawData));
                        edit_State = Edit_State.DONE;
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_COMMON_MFX)
                    {
                        commonMFX = new CommonMFX(new ReceivedData(rawData));
                        edit_State = Edit_State.DONE;
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_DRUM_KIT_PARTIAL)
                    {
                        //superNATURALDrumKit.superNATURALDrumKitKey = new SuperNATURALDrumKitKey(new ReceivedData(rawData));
                        edit_State = Edit_State.DONE;
                    }
                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_DRUM_KIT_COMMON_MFX)
                    {
                        commonMFX = new CommonMFX(new ReceivedData(rawData));
                        edit_State = Edit_State.DONE;
                    }

                    else if (edit_State == Edit_State.WAITING && currentEditMidiRequest == Edit_CurrentMidiRequest.EDIT_TONE_SAVE)
                    {
                        //if (tbEditToneToneName.Text.Length > 12)
                        //{
                        //    tbEditToneToneName.Text.Remove(12);
                        //}
                        //while (tbEditToneToneName.Text.Length < 12)
                        //{
                        //    tbEditToneToneName.Text += " ";
                        //}
                        //byte[] address = new byte[] { 0x19, 0x00, 0x00, 0x00 };
                        //byte[] value = Encoding.ASCII.GetBytes(tbEditToneToneName.Text); //new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c };
                        //byte[] data = midi.SystemExclusiveDT1Message(address, value);
                        //midi.SendSystemExclusive(data);                    }
                        //byte[] address = new byte[] { 0x18, 0x00, 0x20, 0x06 };
                        //byte[] value = new byte[] { 0x57, 0x00, 0xff };
                        //byte[] data = midi.SystemExclusiveDT1Message(address, value);
                        //midi.SendSystemExclusive(data);
                    }
                }
            }
        }
        #endregion

        #region queryIntegra7
        /// <summary>
        /// The INTEGRA-7 does not answer to sound settings requests unless that type of sound is selected.
        /// We therefore need to read what type of sound is selected on current part (selectable, but starts with part1).
        /// Studio set parts contains selected tone by bank and number. Different type of tones are sorted in different
        /// banks, and bank msb will tell us what type of tone to expect. Address for studio set part 1 is 18 00 20 00
        /// and bank is at offset 00 06. Length is 40.
        /// </summary>
        private void QueryToneType()
        {
            t.Trace("private void QueryToneType()");
            initDone = false;
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_TONE_TYPE; 
            byte[] address = { 0x18, 0x00, (byte)(0x20 + commonState.CurrentPart), 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x09 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        // Common query MFX entry:
        private void QueryCommonMFX()
        {
            switch (currentProgramType)
            {
                case ProgramType.PCM_SYNTH_TONE:
                    QueryPCMSynthToneCommonMFX();
                    break;
                case ProgramType.PCM_DRUM_KIT:
                    QueryPCMDrumKitCommonMFX();
                    break;
                case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                    QuerySuperNATURALAcousticToneCommonMFX();
                    break;
                case ProgramType.SUPERNATURAL_SYNTH_TONE:
                    QuerySuperNATURALSynthToneCommonMFX();
                    break;
                case ProgramType.SUPERNATURAL_DRUM_KIT:
                    QuerySuperNATURALDrumKitCommonMFX();
                    break;
            }
        }

        private void QueryPCMSynthToneCommon()          // length 01 11 start 19 00 00 00 (for part 1)
        {
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_COMMON;
            byte[] address = { 0x19, 0x00, 0x00, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x01, 0x11 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryPCMSynthToneCommonMFX()          // length 01 11 start 19 00 02 00 (for part 1)
        {
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_COMMON_MFX;
            byte[] address = { 0x19, 0x00, 0x02, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x01, 0x11 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryPCMSynthTonePMT()
        {
            t.Trace("private void QueryPCMSynthTonePMT()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_PMT;
            byte[] address = { 0x19, 0x00, 0x10, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x00, 0x29 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryPCMSynthTonePartial()
        {
            t.Trace("private void QueryPCMSynthTonePartial()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_PARTIAL;
            byte[] address = { 0x19, 0x00, 0x00, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] partialOffset = new byte[] { 0x00, 0x00, (byte)(0x20 + 0x02 * currentPartial), 0x00 };
            address = AddBytes128(address, partialOffset);
            byte[] length = { 0x00, 0x00, 0x01, 0x1a };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryPCMSynthToneCommon2()
        {
            t.Trace("private void QueryPCMSynthToneCommon2()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_PCM_SYNTH_TONE_COMMON2;
            byte[] address = { 0x19, 0x00, 0x30, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x00, 0x3c };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryPCMDrumKitCommon()
        {
            t.Trace("private void QueryPCMDrumKitCommon()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_COMMON;
            byte[] address = { 0x19, 0x10, 0x00, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x00, 0x12 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryPCMDrumKitCommonMFX()
        {
            t.Trace("private void QueryPCMDrumKitCommonMFX()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_COMMON_MFX;
            byte[] address = { 0x19, 0x10, 0x02, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x01, 0x11 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryPCMDrumKitCommonCompEq()
        {
            t.Trace("private void QueryPCMDrumKitCommonCompEq()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_COMMON_COMP_EQ;
            byte[] address = { 0x19, 0x10, 0x08, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x00, 0x54 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryPCMDrumKitPartial() // Read for a current key, 21 - 108!
        {
            t.Trace("private void QueryPCMDrumKitPartial (" + ")");
            byte[] address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, 0x00);
            byte[] length = { 0x00, 0x00, 0x01, 0x43 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_PARTIAL;
        }

        private void QueryPCMDrumKitCommon2()
        {
            t.Trace("private void QueryPCMDrumKitCommon2()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_PCM_DRUM_KIT_COMMON2;
            byte[] address = { 0x19, 0x12, 0x00, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x00, 0x32 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QuerySuperNATURALAcousticTone()
        {
            t.Trace("private void QuerySuperNATURALAcousticTone()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_ACOUSTIC_TONE_COMMON;
            byte[] address = { 0x19, 0x02, 0x00, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x00, 0x46 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QuerySuperNATURALAcousticToneCommonMFX()
        {
            t.Trace("private void QuerySuperNATURALAcousticToneMFX()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_ACOUSTIC_TONE_COMMON_MFX;
            byte[] address = { 0x19, 0x02, 0x02, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x01, 0x11 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QuerySuperNATURALSynthToneUndocumentedParameters()
        {
            t.Trace("private void QuerySuperNATURALSynthToneUndocumentedParameters()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_UNDOCUMENTED_PARAMETERS;
            byte[] address = { 0x19, 0x01, 0x50, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x25 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QuerySuperNATURALSynthToneCommon()
        {
            t.Trace("private void QuerySuperNATURALSynthToneCommon()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_COMMON;
            byte[] address = { 0x19, 0x01, 0x00, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x00, 0x40 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QuerySuperNATURALSynthToneCommonMFX()
        {
            t.Trace("private void QuerySuperNATURALSynthToneCommonMFX()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_COMMON_MFX;
            byte[] address = { 0x19, 0x01, 0x02, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x01, 0x11 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QuerySuperNATURALSynthTonePartial()
        {
            t.Trace("private void QuerySuperNATURALSynthTonePartial()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_PARTIAL;
            byte[] address = { 0x19, 0x01, 0x20, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] partialOffset = new byte[] { 0x00, 0x00, (byte)currentPartial, 0x00 };
            address = AddBytes128(address, partialOffset);
            byte[] length = { 0x00, 0x00, 0x00, 0x3d };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QuerySuperNATURALSynthToneMisc()
        {
            t.Trace("private void QuerySuperNATURALSynthToneMisc()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_SYNTH_TONE_MISC;
            byte[] address = { 0x19, 0x01, 0x50, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x06 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QuerySuperNATURALDrumKit()
        {
            t.Trace("private void QuerySuperNATURALDrumKit()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_DRUM_KIT_COMMON;
            byte[] address = { 0x19, 0x03, 0x00, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x00, 0x14 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QuerySuperNATURALDrumKitCommonMFX()
        {
            t.Trace("private void QuerySuperNATURALDrumKitMFX()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_DRUM_KIT_COMMON_MFX;
            byte[] address = { 0x19, 0x03, 0x02, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x01, 0x11 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QuerySuperNATURALDrumKitCommonCompEQ()
        {
            t.Trace("private void QuerySuperNATURALDrumKitCommonCompEQ()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_DRUM_KIT_COMMON_COMP_EQ;
            byte[] address = { 0x19, 0x03, 0x08, 0x00 };
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);
            byte[] length = { 0x00, 0x00, 0x00, 0x54 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QuerySuperNATURALDrumKitPartial()
        {
            t.Trace("private void QuerySuperNATURALDrumKitNote()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_SUPERNATURAL_DRUM_KIT_PARTIAL;
            byte[] address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x00 });
            byte[] length = { 0x00, 0x00, 0x00, 0x13 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetSendParameters()
        {
            t.Trace("private void QueryStudioSetSendParameters()");
            edit_State = Edit_State.WAITING;
            currentEditMidiRequest = Edit_CurrentMidiRequest.QUERYING_STUDIO_SET_SEND_PARAMETERS;
            byte[] address = new byte[] { 0x18, 0x00, (byte)(0x20 + commonState.CurrentPart), 0x27 };
            byte[] length = { 0x00, 0x00, 0x00, 0x02 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        #endregion

        #region build controls
        private void BuildComboBox(String Name, Object Parent)
        {
            t.Trace("private void BuildComboBox (" + "String" + Name + ", " + "Control" + Parent + ", " + ")");

        }

        private void BuildSlider(String Name, Object Parent)
        {
            t.Trace("private void BuildSlider (" + "String" + Name + ", " + "Control" + Parent + ", " + ")");

        }

        private void BuildCheckBox(String Name, Object Parent)
        {
            t.Trace("private void BuildCheckBox (" + "String" + Name + ", " + "Control" + Parent + ", " + ")");

        }

        private void BuildPCMSynthToneCommonControls()
        {
            t.Trace("private void BuildPCMSynthToneCommonControls()");

        }
        #endregion

        #region handlers

        private void Generic_GotFocus(object sender, FocusEventArgs e)
        {
            if (initDone && handleControlEvents)
            {
                HelpTag tag;
                HelpItem helpItem = null;
                try
                {
                    if (sender.GetType() == typeof(ComboBox))
                    {
                        ComboBox control = (ComboBox)sender;
                        if (control.Tag.GetType() == typeof(HelpTag))
                        {
                            tag = (HelpTag)control.Tag;
                            helpItem = Help.Show(currentProgramIndex, currentParameterPageIndex, (byte)(((HelpTag)tag).ItemIndex + Help.Skip), (byte)(control.SelectedIndex));
                        }
                        else
                        {
                            Buddy buddyTag = (Buddy)control.Tag;
                            tag = (HelpTag)buddyTag.Tag;
                            helpItem = Help.Show(5, (byte)(currentMFXType + currentMFXTypeOffset), (byte)(((HelpTag)tag).ItemIndex), (byte)(((HelpTag)tag).SubItemIndex + control.SelectedIndex));
                        }
                    }
                    else if (sender.GetType() == typeof(Slider))
                    {
                        Slider control = (Slider)sender;
                        if (control.Tag.GetType() == typeof(HelpTag))
                        {
                            tag = (HelpTag)control.Tag;
                            helpItem = Help.Show(currentProgramIndex, currentParameterPageIndex, (byte)(((HelpTag)tag).ItemIndex + Help.Skip), (byte)(((HelpTag)tag).SubItemIndex));
                        }
                        else
                        {
                            Buddy buddyTag = (Buddy)control.Tag;
                            tag = (HelpTag)buddyTag.Tag;
                            helpItem = Help.Show(5, (byte)(currentMFXType + currentMFXTypeOffset), (byte)(((HelpTag)tag).ItemIndex), (byte)(((HelpTag)tag).SubItemIndex));
                        }
                    }
                    else if (sender.GetType() == typeof(CheckBox))
                    {
                        CheckBox control = (CheckBox)sender;
                        if (control.Tag.GetType() == typeof(HelpTag))
                        {
                            tag = (HelpTag)control.Tag;
                            helpItem = Help.Show(currentProgramIndex, currentParameterPageIndex, (byte)(((HelpTag)tag).ItemIndex + Help.Skip), (byte)(((HelpTag)tag).SubItemIndex));
                        }
                        else
                        {
                                
                            Buddy buddyTag = (Buddy)control.Tag;
                            tag = (HelpTag)buddyTag.Tag;
                            helpItem = Help.Show(5, (byte)(currentMFXType + currentMFXTypeOffset), (byte)(((HelpTag)tag).ItemIndex), (byte)(((HelpTag)tag).SubItemIndex));
                        }
                    }
                    else if (sender.GetType() == typeof(TextBox))
                    {
                        TextBox control = (TextBox)sender;
                        if (control.Tag.GetType() == typeof(HelpTag))
                        {
                            tag = (HelpTag)control.Tag;
                            helpItem = Help.Show(currentProgramIndex, currentParameterPageIndex, (byte)(((HelpTag)tag).ItemIndex + Help.Skip), (byte)(((HelpTag)tag).SubItemIndex));
                        }
                        else
                        {

                            Buddy buddyTag = (Buddy)control.Tag;
                            tag = (HelpTag)buddyTag.Tag;
                            helpItem = Help.Show(5, (byte)(currentMFXType + currentMFXTypeOffset), (byte)(((HelpTag)tag).ItemIndex), (byte)(((HelpTag)tag).SubItemIndex));
                        }
                    }
                    else if (sender.GetType() == typeof(Button))
                    {
                        Button control = (Button)sender;
                        if (control.Tag.GetType() == typeof(HelpTag))
                        {
                            tag = (HelpTag)control.Tag;
                            helpItem = Help.Show(currentProgramIndex, currentParameterPageIndex, (byte)(((HelpTag)tag).ItemIndex + Help.Skip), (byte)(((HelpTag)tag).SubItemIndex));
                        }
                        else
                        {

                            Buddy buddyTag = (Buddy)control.Tag;
                            tag = (HelpTag)buddyTag.Tag;
                            helpItem = Help.Show(5, (byte)(currentMFXType + currentMFXTypeOffset), (byte)(((HelpTag)tag).ItemIndex), (byte)(((HelpTag)tag).SubItemIndex));
                        }
                    }
                    Edit_RenderRightColumn(helpItem);
                }
                catch (Exception e2)
                {
                    t.Trace("Error in Generic_GotFocus: " + e2.Message);
                    if (e2.InnerException != null)
                    {
                        t.Trace("Inner exception: " + e2.InnerException.Message);
                    }
                }
            }
        }

        private void Help_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(currentProgramIndex, 0, 0, 0);
        }

        private void cbEditTone_PartSelector_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(currentProgramIndex, 0, 1, 0);
        }

        private void cbEditTone_SynthesizerType_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(currentProgramIndex, 0, 2, 0);
        }

        private void cbEditTone_ParameterPages_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(currentProgramIndex, 0, 3, 0);
        }

        private void cbEditTone_PartialSelector_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(currentProgramIndex, 0, 4, 0);
        }

        private void cbEditTone_KeySelector_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(currentProgramIndex, 0, 6, 0);
        }

        //private void tbEditTone_KeyName_GotFocus(object sender, FocusEventArgs e)
        //{
        //    Help.Show(currentProgramIndex, 0, 5, 0);
        //}

        private void cbEditTone_InstrumentCategorySelector_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(currentProgramIndex, 0, 7, 0);
        }

        private void cbEditTone_MFXType_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(5, 0, 0, 0);
        }

        private void slEditToneChorusSendLevel_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(currentProgramIndex, 0, 8, 0);
        }

        private void slEditToneReverbSendLevel_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(currentProgramIndex, 0, 9, 0);
        }

        private void btnEditTone_Play_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(currentProgramIndex, 0, 10, 0);
        }

        private void btnEditTone_Reset_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(currentProgramIndex, 0, 11, 0);
        }

        private void btnEditTone_Return_GotFocus(object sender, FocusEventArgs e)
        {
            Help.Show(currentProgramIndex, 0, 12, 0);
        }
        #endregion

        #region helpers

        /// <summary>
        /// Given an MFX type some variable will be set to defeat the problem of splitted parameter pages
        /// i.e. the selectedIndex is not the same as MFXType once a splitter page has been reached.
        /// We need address offset to use in Integra-7, parameter offset to use in numbered parameters etc.
        /// A call PCMSynthTone.PCMSynthToneCommonMFX.MFXNumberedParameters.SetMFXTypeAndOffset(MFXType) gives us an array of bytes containig
        /// values needed to update current mfx type status:
        /// currentMFXType = result[0]:                    This is the selected index converted to the actual MFX type by using the type offset.
        /// currentMFXTypeOffset = result[1];              This is the offset between actual MFX type and selected index.
        /// currentMFXTypePageAddressOffset = result[2];   This is the address offset in Integra-7 that is used to get to parameters in splitted page.
        /// currentMFXTypePageCount = result[3];           This is the total page count for current MFX type.
        /// currentMFXTypePageParameterOffset = result[4]; This is the parameter offset for the current page in a splitted page.
        /// </summary>
        /// <param name="ComboBoxIndex">The selected index of the combobox.</param>
        /// <returns>A byte containing the decucted MFX type.</returns>
        public byte SetMFXTypeAndOffset(byte ComboBoxIndex)
        {
            t.Trace("public byte SetMFXTypeAndOffset (" + "byte" + ComboBoxIndex + ", " + ")");
            currentMFXTypeOffset = numberedParametersContent.MFXTypeOffset[ComboBoxIndex];
            currentMFXTypePageParameterOffset = numberedParametersContent.MFXPageParameterOffset[ComboBoxIndex];
            currentMFXTypePageAddressOffset = (byte)(4 * currentMFXTypePageParameterOffset);
            currentMFXTypePageCount = numberedParametersContent.MFXPageCount[ComboBoxIndex];
            currentMFXType = (byte)(ComboBoxIndex - currentMFXTypeOffset);
            return currentMFXType;
        }

        public void SetMFXOffsetValues(byte MFXType)
        {
            byte i = 0;
            while (MFXType != SetMFXTypeAndOffset(i)) i++;
        }

        //private Double AdjustSliderStep(Double value, Double step, Double lastValue)
        //{
        //    t.Trace("private Double AdjustSliderStep (" + "Double" + value + ", " + "Double" + step + ", " + "Double" + lastValue + ", " + ")");
        //    if (value > 0)
        //    {
        //        if (value - lastValue > 0) // Increasing
        //        {
        //            if (value - lastValue < 1) // Small step up, use 1 step
        //            {
        //                value = lastValue + step;
        //            }
        //            else if (lastValue - value < 1) // Small setp down, use 1 step
        //            {
        //                value = lastValue - step;
        //            }   // Else, stay with given value
        //        }
        //    }
        //    else if (value < 0) // Negative values
        //    {

        //    }
        //    return value;
        //}

        private Double AdjustSliderStep(Double value, Double step)
        {
            t.Trace("private Double AdjustSliderStep (" + "Double" + value + ", " + "Double" + step + ", " + ")");
            if (value > 0)
            {
                if (value % step == 1)
                {
                    value = value + step - 1;
                }
                else if (value % step == step - 1)
                {
                    value = value - (step - 1);
                }
                else if (value % step < step / 2)
                {
                    value = value - value % step;
                }
                else
                {
                    value = value - value % step + step;
                }
            }
            else
            {
                if (value % step == -1)
                {
                    value = value - (step - 1);
                }
                else if (value % step == -(step - 1))
                {
                    value = value + (step - 1);
                }
                else if (value % step < step / 2)
                {
                    value = value + value % step;
                }
                else
                {
                    value = value - value % step - step;
                }
            }
            return value;
        }

        /// <summary>
        /// Makes a system exclusive to Integra-7 to alter a parameter.
        /// Very important! Make sure commonState.CurrentPart and currentPMT are set properly before using this function!
        /// </summary>
        /// <param name="Address">Base address of parameter</param>
        /// <param name="Data">Data to send (one byte, an array of bytes or a text string)</param>
        /// <param name="Index">Index if parameter is an array of values</param>
        /// <param name="ObjectIndex">Some parameters are arrays of objects, give 4 bytes indicating what byte to step and how much</param>
        /// <param name="FirstAddressByteIs0x08">Some 4-byte addressed parameters are sent as msb, ... , lsb (4 bytes) but some as 0x08, 0x00, msb/0x00, lsb</param>
        private void SendParameter(byte[] Address, UInt16 Data, Boolean UsePartialOffset = false, byte[] Index = null, byte[] ObjectIndex = null, Boolean FirstAddressByteIs0x08 = false)
        {
            t.Trace("private void SendParameter (" + "byte[]" + Address + ", " + "UInt16" + Data + ", " + "Boolean" + UsePartialOffset + ", " + 
                "byte[]" + Index + ", " + "byte[]" + ObjectIndex + ", " + "Boolean" + FirstAddressByteIs0x08 + ", " + ")");
            byte[] data = new byte[4];
            data[0] = (byte)((Data & 0xf000) >> 12);
            data[1] = (byte)((Data & 0x0f00) >> 8);
            data[2] = (byte)((Data & 0x00f0) >> 4);
            data[3] = (byte)(Data & 0x000f);
            SendParameter(Address, data, UsePartialOffset, Index, ObjectIndex, FirstAddressByteIs0x08);
        }

        private void SendParameter(byte[] Address, byte Data, Boolean UsePartialOffset = false, byte[] Index = null, byte[] ObjectIndex = null, Boolean FirstAddressByteIs0x08 = false)
        {
            t.Trace("private void SendParameter (" + "byte[]" + Address + ", " + "byte" + Data + ", " + "Boolean" + UsePartialOffset + ", " + 
                "byte[]" + Index + ", " + "byte[]" + ObjectIndex + ", " + "Boolean" + FirstAddressByteIs0x08 + ", " + ")");
            byte[] data = new byte[1];
            data[0] = Data;
            SendParameter(Address, data, UsePartialOffset, Index, ObjectIndex, FirstAddressByteIs0x08);
        }

        private void SendParameter(byte[] Address, String Data, Boolean UsePartialOffset = false, byte[] Index = null, byte[] ObjectIndex = null, Boolean FirstAddressByteIs0x08 = false)
        {
            t.Trace("private void SendParameter (" + "byte[]" + Address + ", " + "byte" + Data + ", " + "Boolean" + UsePartialOffset + ", " +
                "byte[]" + Index + ", " + "byte[]" + ObjectIndex + ", " + "Boolean" + FirstAddressByteIs0x08 + ", " + ")");
            byte[] data = new byte[Data.Length];
            byte i = 0;
            foreach (char c in Data)
            {
                data[i] = (byte)Data[i++];
            }
            SendParameter(Address, data, UsePartialOffset, Index, ObjectIndex, FirstAddressByteIs0x08);
        }

        // NOTE! UsePartialOffset is no longer used. MakeAddress takes care of both partial offset and key offset.
        private void SendParameter(byte[] Address, byte[] Data, Boolean UsePartialOffset = false, byte[] Index = null, byte[] ObjectIndex = null, Boolean FirstAddressByteIs0x08 = false)
        {
            t.Trace("private void SendParameter (" + "byte[]" + Address + ", " + "byte[]" + Data + ", " + "Boolean" + UsePartialOffset + ", " + 
                "byte[]" + Index + ", " + "byte[]" + ObjectIndex + ", " + "Boolean" + FirstAddressByteIs0x08 + ", " + ")");
            if (ObjectIndex != null)
            {
                Address = AddBytes128(Address, ObjectIndex);
            }
            if (Index != null)
            {
                Address = AddBytes128(Address, Index);
            }
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(Address, Data);
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private byte[] MakeAddress(ProgramType Type, ParameterPage Page, UInt16 ParameterAddressByte, Boolean PartialOrKeyDependent = false)
        {
            return MakeAddress(Type, Page, new byte[] { (byte)((ParameterAddressByte & 0xf000) >> 12),
                (byte)((ParameterAddressByte & 0x0f00) >> 8), (byte)((ParameterAddressByte & 0x00f0) >> 4),
                (byte)(ParameterAddressByte & 0x000f) }, PartialOrKeyDependent);
        }


        private byte[] MakeAddress(ProgramType Type, ParameterPage Page, byte ParameterAddressByte, Boolean PartialOrKeyDependent = false)
        {
            t.Trace("private byte[] MakeAddress (" + "ProgramType" + Type + ", " + "ParameterPage" + Page + ", " + "byte" + ParameterAddressByte + ", " + ")");
            byte[] bytes = new byte[1];
            bytes[0] = ParameterAddressByte;
            return MakeAddress(Type, Page, bytes, PartialOrKeyDependent);
        }

        private byte[] MakeAddress(ProgramType Type, ParameterPage Page, byte[] ParameterAddressBytes, Boolean PartialOrKeyDependent = false)
        {
            t.Trace("private byte[] MakeAddress (" + "ProgramType" + Type + ", " + "ParameterPage" + Page + ", " + "byte[]" + ParameterAddressBytes);
            t.Trace("currentProgramType: " + currentProgramType.ToString());
            t.Trace("commonState.CurrentPart.......: " + commonState.CurrentPart.ToString());
            t.Trace("currentPartial....: " + currentPartial.ToString());
            //t.Trace("currentKey........: " + currentKey.ToString());
            //try
            //{ 
            //    tbEditToneHelpsText.Text =
            //        "currentProgramType: " + currentProgramType.ToString() + "\r\n" +
            //        "commonState.CurrentPart.......: " + commonState.CurrentPart.ToString() + "\r\n" +
            //        "currentPartial....: " + currentPartial.ToString() + "\r\n" +
            //        "currentKey........: " + currentKey.ToString() + "\r\n";
            //} catch { }

            // Main address:
            byte[] address = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            switch (Type)
            {
                case ProgramType.PCM_SYNTH_TONE:
                    switch (Page)
                    {
                        case ParameterPage.COMMON:
                            address = new byte[4] { 0x19, 0x00, 0x00, 0x00 };
                            break;
                        case ParameterPage.COMMONMFX:
                            address = new byte[4] { 0x19, 0x00, 0x02, 0x00 };
                            break;
                        case ParameterPage.PMT:
                            address = new byte[4] { 0x19, 0x00, 0x10, 0x00 };
                            break;
                        case ParameterPage.PARTIAL:
                            address = new byte[4] { 0x19, 0x00, 0x20, 0x00 };
                            break;
                        case ParameterPage.COMMON2:
                            address = new byte[4] { 0x19, 0x00, 0x30, 0x00 };
                            break;
                    }
                    break;
                case ProgramType.PCM_DRUM_KIT:
                    switch (Page)
                    {
                        case ParameterPage.COMMON:
                            address = new byte[4] { 0x19, 0x10, 0x00, 0x00 };
                            break;
                        case ParameterPage.COMMONMFX:
                            address = new byte[4] { 0x19, 0x10, 0x02, 0x00 };
                            break;
                        case ParameterPage.PARTIAL:
                            address = new byte[4] { 0x19, 0x10, 0x10, 0x00 };
                            break;
                        case ParameterPage.COMMONCOMPEQ:
                            address = new byte[4] { 0x19, 0x10, 0x08, 0x00 };
                            break;
                        case ParameterPage.COMMON2:
                            address = new byte[4] { 0x19, 0x12, 0x00, 0x00 };
                            break;
                    }
                    break;
                case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                    switch (Page)
                    {
                        case ParameterPage.COMMON:
                            address = new byte[4] { 0x19, 0x02, 0x00, 0x00 };
                            break;
                        case ParameterPage.COMMONMFX:
                            address = new byte[4] { 0x19, 0x02, 0x02, 0x00 };
                            break;
                    }
                    break;
                case ProgramType.SUPERNATURAL_SYNTH_TONE:
                    switch (Page)
                    {
                        case ParameterPage.COMMON:
                            address = new byte[4] { 0x19, 0x01, 0x00, 0x00 };
                            break;
                        case ParameterPage.MISC:
                            address = new byte[4] { 0x19, 0x01, 0x50, 0x00 };
                            break;
                        case ParameterPage.COMMONMFX:
                            address = new byte[4] { 0x19, 0x01, 0x02, 0x00 };
                            break;
                        case ParameterPage.PARTIAL:
                            address = new byte[4] { 0x19, 0x01, 0x20, 0x00 };
                            break;
                    }
                    break;
                case ProgramType.SUPERNATURAL_DRUM_KIT:
                    switch (Page)
                    {
                        case ParameterPage.COMMON:
                            address = new byte[4] { 0x19, 0x03, 0x00, 0x00 };
                            break;
                        case ParameterPage.COMMONMFX:
                            address = new byte[4] { 0x19, 0x03, 0x02, 0x00 };
                            break;
                        case ParameterPage.PARTIAL: // For drum kits, this is actually the lowest key address
                            address = new byte[4] { 0x19, 0x03, 0x10, 0x00 };
                            break;
                        case ParameterPage.COMMONCOMPEQ:
                            address = new byte[4] { 0x19, 0x03, 0x08, 0x00 };
                            break;
                    }
                    break;
            }

            // Part offset:
            byte[] partOffset = new byte[] { (byte)((0x20 * commonState.CurrentPart) / 128), (byte)((0x20 * commonState.CurrentPart) % 128), 0x00, 0x00 };
            address = AddBytes128(address, partOffset);

            // Key and WMT offset:
            byte[] partialOffset = null;
            if (Page == ParameterPage.PARTIAL)
            {
                switch (currentProgramType)
                {
                    case ProgramType.PCM_SYNTH_TONE:
                        if (currentPartial >= 0 && currentPartial < 4)
                        {
                            partialOffset = new byte[] { 0x00, 0x00, (byte)(2 * currentPartial), 0x00 };
                        }
                        break;
                    case ProgramType.PCM_DRUM_KIT:
                        // What looks like partials in PCM drum kits are actually 4 sets of Wave Mix Tables inside current partial.
                        // What really are partials in PCM drum kits are the different keys.
                        partialOffset = new byte[] { 0x00, 0x00, (byte)(2 * (currentKey)), 0x00 };
                        break;
                    case ProgramType.SUPERNATURAL_SYNTH_TONE:
                        if (currentPartial >= 0 && currentPartial < 3)
                        {
                            partialOffset = new byte[] { 0x00, 0x00, (byte)(currentPartial), 0x00 };
                        }
                        break;
                    case ProgramType.SUPERNATURAL_DRUM_KIT:
                        // What really are partials in SuperNATURAL drum kits are the different keys.
                        // There are no further 'partial' per key like in PCM Drum kits (those use 4 datasets for waves in each key WMT's, but not in SN-D!
                        if (currentKey >= 0 && currentKey <= 61)
                        {
                            partialOffset = new byte[] { 0x00, 0x00, (byte)((currentKey)), 0x00 };
                        }
                        break;
                }
            }

            byte[] waveMixTableOffset = null;
            if (Page == ParameterPage.PARTIAL && PartialOrKeyDependent)
            {
                switch (currentProgramType)
                {
                    case ProgramType.PCM_DRUM_KIT:
                        // What looks like partials in PCM drum kits are actually 4 sets of Wave Mix Tables inside current partial.
                        // What really are partials in PCM drum kits are the different keys.
                        partialOffset = new byte[] { 0x00, 0x00, (byte)(2 * (currentKey)), 0x00 };
                        if (currentKey >= 0 && currentKey <= 87)
                        {
                            if (currentPartial != 0xff)
                            {
                                waveMixTableOffset = new byte[] { 0x00, 0x00, 0x00, (byte)(0x1d * currentPartial) };
                            }
                        }
                        break;
                    //case ProgramType.SUPERNATURAL_DRUM_KIT:
                    //    // What really are partials in SuperNATURAL drum kits are the different keys.
                    //    // There are no further 'partial' per key like in PCM Drum kits (those use 4 datasets for waves in each key WMT's, but not in SN-D!
                    //    if (currentKey >= 0 && currentKey <= 61)
                    //    {
                    //        partialOffset = new byte[] { 0x00, 0x00, (byte)((currentKey)), 0x00 };
                    //    }
                    //    break;
                }
            }

            if (partialOffset != null)
            {
                address = AddBytes128(address, partialOffset);
            }
            if (waveMixTableOffset != null)
            {
                address = AddBytes128(address, waveMixTableOffset);
            }

            byte[] log = AddBytes128(address, ParameterAddressBytes);
            //try
            //{
            //    tbEditToneHelpsText.Text += "Address...........: " + hexString(log);
            //}
            //catch { }
            return AddBytes128(address, ParameterAddressBytes);
        }

        // For logging only, no functionality
        private String hexString(byte[] b)
        {
            String s = "";
            String hexChars = "0123456789abcdef";
            for (byte i = 0; i < b.Length; i++)
            {
                byte b1 = (byte)((b[i] & 0xf0) >> 4);
                byte b2 = (byte)(b[i] & 0x0f);
                s += "0x" + hexChars.ToCharArray()[b1] + hexChars.ToCharArray()[b2] + " ";
            }
            return s;
        }

        public void SetLabelProperties(ref TextBox tb) // TODO: Take a look at this!
        {
            t.Trace("public void SetLabelProperties (" + "ref TextBox" + tb.Text + ", " + ")");
            //tb.TextAlignment = TextAlignment.Right;
            //tb.VerticalContentAlignment = VerticalAlignment.Bottom;
            //tb.BorderThickness = new Thickness(0);
            tb.IsEnabled = false;
        }

        public void SetFirstSoundOfCurrentType()
        {
            t.Trace("public void SetFirstSoundOfCurrentType()");
            byte msb = 0;
            byte lsb = 0;
            byte pc = 0;
            switch (currentParameterPageIndex)
            {
                case 0:
                    // Set first sound of PCM synth tone
                    msb = 87;
                    break;
                case 1:
                    // Set first sound of PCM drum kit
                    msb = 86;
                    break;
                case 2:
                    // Set first sound of SuperNatural scoustic tone 
                    msb = 89;
                    break;
                case 3:
                    // Set first sound of SuperNatural synth tone
                    msb = 95;
                    break;
                case 4:
                    // Set first sound of SuperNatural drum kit
                    msb = 88;
                    break;
            }
            commonState.Midi.ProgramChange(commonState.CurrentPart, msb, lsb, pc);
        }

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
            t.Trace("public byte[] AddBytes128 (" + "byte[]" + arg1 + ", " + "byte[]" + arg2 + ", " + ")");
            if (arg1.Length < arg2.Length)
            {
                return null;
            }
            if (arg1.Length > arg2.Length)
            {
                byte diff = (byte)(arg1.Length - arg2.Length);
                byte[] b = new byte[arg1.Length];
                for (byte i = diff; i <  (byte)arg1.Length; i++)
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

        public byte[] AddBytes16(byte[] arg1, byte[] arg2)
        {
            if (arg1.Length != arg2.Length)
            {
                return null;
            }
            byte[] result = new byte[arg1.Length];
            for (Int32 i = 0; i < result.Length; i++)
            {
                result[i] = arg1[i];
            }
            for (Int32 i = arg1.Length - 1; i >= 0;  i--)
            {
                if (arg1[i] + arg2[i] > 0x0f)
                {
                    result[i] = (byte)(result[i] + arg2[i] - 0x0f);
                    if (i > 0)
                    {
                        result[i - 1]++;
                    }
                }
                else
                {
                    result[i] = (byte)(result[i] + arg2[i]);
                }
            }
            return result;
        }

        public byte[] SplitToBytes128(UInt16 value)
        {
            byte[] result = new byte[4];
            result[0] = 0x00;                           // 0000 0000 0000 0000
            result[1] = (byte)((value & 0x7c00) >> 14); // 1100 0000 0000 0000
            result[2] = (byte)((value & 0x03e0) >> 7);  // 0011 1111 1000 0000
            result[3] = (byte)(value & 0x007f);         // 0000 0000 0111 1111
            return result;
        }

        public byte[] SplitToBytes16(UInt16 value)
        {
            byte[] result = new byte[4];
            result[0] = (byte)((value & 0xf000) >> 12);
            result[1] = (byte)((value & 0x0f00) >> 8);
            result[2] = (byte)((value & 0x00f0) >> 4);
            result[3] = (byte)(value & 0x000f);
            return result;
        }
    }
    #endregion
}