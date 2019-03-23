using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integra_7_Xamarin
{
    public partial class UIHandler
    {
        enum StudioSetEditor_currentStudioSetEditorMidiRequest
        {
            NONE,
            GET_CURRENT_STUDIO_SET_NUMBER,
            GET_CURRENT_STUDIO_SET_NUMBER_AND_SCAN,
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
            STUDIO_SET_REVERB_ROOM1,
            STUDIO_SET_REVERB_ROOM2,
            STUDIO_SET_REVERB_HALL1,
            STUDIO_SET_REVERB_HALL2,
            STUDIO_SET_REVERB_PLATE,
            STUDIO_SET_REVERB_GM2_REVERB,
            STUDIO_SET_MOTIONAL_SURROUND,
            STUDIO_SET_MASTER_EQ,
            STUDIO_SET_PART,
            STUDIO_SET_PART_TONE_NAME,
            STUDIO_SET_PART_MIDI_PHASELOCK,
            STUDIO_SET_PART_EQ,
        }

        enum StudioSetEditor_State
        {
            INIT,
            INIT_DONE,
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
            QUERYING_STUDIO_SET_REVERB_ROOM1,
            QUERYING_STUDIO_SET_REVERB_ROOM2,
            QUERYING_STUDIO_SET_REVERB_HALL1,
            QUERYING_STUDIO_SET_REVERB_HALL2,
            QUERYING_STUDIO_SET_REVERB_PLATE,
            QUERYING_STUDIO_SET_REVERB_GM2_REVERB,
            QUERYING_STUDIO_SET_MOTIONAL_SURROUND,
            QUERYING_STUDIO_SET_MASTER_EQ,
            QUERYING_STUDIO_SET_PART,
            QUERYING_STUDIO_SET_PART_TONE_NAME,
            QUERYING_STUDIO_SET_PART_MIDI_PHASELOCK,
            QUERYING_STUDIO_SET_PART_EQ,
            DONE,
        }
        StudioSetEditor_currentStudioSetEditorMidiRequest currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.NONE;
        StudioSetEditor_State studioSetEditor_State;
        private Int32 studioSetEditor_PartToRead;
        byte[] currentToneNumberAsBytes;
        Boolean initialGuiDone = false;
        ObservableCollection<String> StudioSetEditor_SearchResult;
        private byte studioSetNumberTemp;

        //---------------------------------------------------------------------------------------
        // Layout objects declaration
        //---------------------------------------------------------------------------------------

        Grid grid_StudioSet_Column0;
        Grid grid_StudioSet_Column1;   // This will be used in column 1 and contain
                                       // two subgrids, gStudioSet_Column1 and
                                       // gEditStudioSetSearchResult, one visible
                                       // at each time depending on search mode
        Grid grid_StudioSet_Column2;

        //---------------------------------------------------------------------------------------
        // Column 0 
        //---------------------------------------------------------------------------------------

        //Grid grid_StudioSet_Column0_Container;
        //Grid grid_StudioSet_Column1_Container;
        //Grid grid_StudioSet_Column2_Container;
        ComboBox cbStudioSetSelector;
        TextBlock tbToneControl1;
        ComboBox cbToneControl1;
        TextBlock tbToneControl2;
        ComboBox cbToneControl2;
        TextBlock tbToneControl3;
        ComboBox cbToneControl3;
        TextBlock tbToneControl4;
        ComboBox cbToneControl4;
        TextBlock tbTempo;
        Slider slTempo;
        TextBlock tbSoloPart;
        ComboBox cbSoloPart;
        LabeledSwitch cbReverb;
        LabeledSwitch cbChorus;
        LabeledSwitch cbMasterEQ;
        TextBlock tbDrumCompEQPart;
        ComboBox cbDrumCompEQPart;
        TextBlock tbDrumCompEQ1OutputAssign;
        ComboBox cbDrumCompEQ1OutputAssign;
        TextBlock tbDrumCompEQ2OutputAssign;
        ComboBox cbDrumCompEQ2OutputAssign;
        TextBlock tbDrumCompEQ3OutputAssign;
        ComboBox cbDrumCompEQ3OutputAssign;
        TextBlock tbDrumCompEQ4OutputAssign;
        ComboBox cbDrumCompEQ4OutputAssign;
        TextBlock tbDrumCompEQ5OutputAssign;
        ComboBox cbDrumCompEQ5OutputAssign;
        TextBlock tbDrumCompEQ6OutputAssign;
        ComboBox cbDrumCompEQ6OutputAssign;
        LabeledSwitch cbDrumCompEQ;
        LabeledSwitch cbExtPartMute;
        TextBlock tbExtPartLevel;
        Slider slExtPartLevel;
        TextBlock tbExtPartChorusSend;
        Slider slExtPartChorusSend;
        TextBlock tbExtPartReverbSend;
        Slider slExtPartReverbSend;

        //---------------------------------------------------------------------------------------
        // Column 1 
        //---------------------------------------------------------------------------------------

        Grid gStudioSet_Column1;
        Grid gEditStudioSetSearchResult;

        ComboBox cbColumn1Selector;
        ListView lvSearchResults;  // The listview in grid gEditStudioSetSearchResult

        //---------------------------------------------------------------------------------------
        // System common settings 
        Grid SystemCommonSettings;
        TextBlock tbSystemCommonMasterTune;
        Slider slSystemCommonMasterTune;
        TextBlock tbSystemCommonMasterKeyShift;
        Slider slSystemCommonMasterKeyShift;
        TextBlock tbSystemCommonMasterLevel;
        Slider slSystemCommonMasterLevel;
        LabeledSwitch cbSystemCommonScaleTuneSwitch;
        TextBlock tbSystemCommonStudioSetControlChannel;
        ComboBox cbSystemCommonStudioSetControlChannel;
        TextBlock tbSystemCommonSystemControlSource1;
        ComboBox cbSystemCommonSystemControlSource1;
        TextBlock tbSystemCommonSystemControlSource2;
        ComboBox cbSystemCommonSystemControlSource2;
        TextBlock tbSystemCommonSystemControlSource3;
        ComboBox cbSystemCommonSystemControlSource3;
        TextBlock tbSystemCommonSystemControlSource4;
        ComboBox cbSystemCommonSystemControlSource4;
        TextBlock tbSystemCommonControlSource;
        ComboBox cbSystemCommonControlSource;
        TextBlock tbSystemCommonSystemClockSource;
        ComboBox cbSystemCommonSystemClockSource;
        TextBlock tbSystemCommonSystemTempo;
        Slider slSystemCommonSystemTempo;
        TextBlock tbTempoAssignSource;
        ComboBox cbSystemCommonTempoAssignSource;
        LabeledSwitch cbSystemCommonReceiveProgramChange;
        LabeledSwitch cbSystemCommonReceiveBankSelect;
        LabeledSwitch cbSystemCommonSurroundCenterSpeakerSwitch;
        LabeledSwitch cbSystemCommonSurroundSubWooferSwitch;
        TextBlock tbSystemCommonStereoOutputMode;
        ComboBox cbSystemCommonStereoOutputMode;

        //---------------------------------------------------------------------------------------
        // Voice reserve settings 
        Grid VoiceReserve;
        TextBlock tbVoiceReserve01;
        Slider slVoiceReserve01;
        TextBlock tbVoiceReserve02;
        Slider slVoiceReserve02;
        TextBlock tbVoiceReserve03;
        Slider slVoiceReserve03;
        TextBlock tbVoiceReserve04;
        Slider slVoiceReserve04;
        TextBlock tbVoiceReserve05;
        Slider slVoiceReserve05;
        TextBlock tbVoiceReserve06;
        Slider slVoiceReserve06;
        TextBlock tbVoiceReserve07;
        Slider slVoiceReserve07;
        TextBlock tbVoiceReserve08;
        Slider slVoiceReserve08;
        TextBlock tbVoiceReserve09;
        Slider slVoiceReserve09;
        TextBlock tbVoiceReserve10;
        Slider slVoiceReserve10;
        TextBlock tbVoiceReserve11;
        Slider slVoiceReserve11;
        TextBlock tbVoiceReserve12;
        Slider slVoiceReserve12;
        TextBlock tbVoiceReserve13;
        Slider slVoiceReserve13;
        TextBlock tbVoiceReserve14;
        Slider slVoiceReserve14;
        TextBlock tbVoiceReserve15;
        Slider slVoiceReserve15;
        TextBlock tbVoiceReserve16;
        Slider slVoiceReserve16;

        //---------------------------------------------------------------------------------------
        // Chorus settings
        Grid Chorus;
        TextBlock tbStudioSetChorusType;
        ComboBox cbStudioSetChorusType;
        TextBlock tbChorusLevel;
        Slider slChorusLevel;
        TextBlock tbStudioSetChorusOutputAssign;
        ComboBox cbChorusOutputAssign;
        TextBlock tbChorusOutputSelect;
        ComboBox cbChorusOutputSelect;
        // Chorus chorus settings 
        Grid ChorusChorus;
        TextBlock tbChorusChorusFilterType;
        ComboBox cbChorusChorusFilterType;
        TextBlock tbChorusChorusFilterCutoffFrequency;
        ComboBox cbChorusChorusFilterCutoffFrequency;
        TextBlock tbChorusChorusPreDelay;
        Slider slChorusChorusPreDelay;
        TextBlock tbChorusChorusRateHzNote;
        ComboBox cbChorusChorusRateHzNote;
        TextBlock tbChorusChorusRateHz;
        Slider slChorusChorusRateHz;
        TextBlock tbChorusChorusRateNote;
        Slider slChorusChorusRateNote;
        TextBlock tbChorusChorusDepth;
        Slider slChorusChorusDepth;
        TextBlock tbChorusChorusPhase;
        Slider slChorusChorusPhase;
        TextBlock tbChorusChorusFeedback;
        Slider slChorusChorusFeedback;
        // Chorus delay settings 
        Grid ChorusDelay;
        TextBlock tbChorusDelayLeftMsNote;
        ComboBox cbChorusDelayLeftMsNote;
        TextBlock tbChorusDelayLeftHz;
        Slider slChorusDelayLeftHz;
        TextBlock tbChorusDelayLeftNote;
        Slider slChorusDelayLeftNote;
        TextBlock tbChorusDelayRightMsNote;
        ComboBox cbChorusDelayRightMsNote;
        TextBlock tbChorusDelayRightHz;
        Slider slChorusDelayRightHz;
        TextBlock tbChorusDelayRightNote;
        Slider slChorusDelayRightNote;
        TextBlock tbChorusDelayCenterMsNote;
        ComboBox cbChorusDelayCenterMsNote;
        TextBlock tbChorusDelayCenterHz;
        Slider slChorusDelayCenterHz;
        TextBlock tbChorusDelayCenterNote;
        Slider slChorusDelayCenterNote;
        TextBlock tbChorusDelayCenterFeedback;
        Slider slChorusDelayCenterFeedback;
        TextBlock tbChorusDelayHFDamp;
        ComboBox cbChorusDelayHFDamp;
        TextBlock tbChorusDelayLeftLevel;
        Slider slChorusDelayLeftLevel;
        TextBlock tbChorusDelayRightLevel;
        Slider slChorusDelayRightLevel;
        TextBlock tbChorusDelayCenterLevel;
        Slider slChorusDelayCenterLevel;
        // Chorus GM2 settings
        Grid ChorusGM2Chorus;
        Grid ChorusGM2ChorusPreLPF;
        TextBlock tbChorusGM2ChorusPreLPF;
        Slider slChorusGM2ChorusPreLPF;
        Grid ChorusGM2ChorusLevel;
        TextBlock tbChorusGM2ChorusLevel;
        Slider slChorusGM2ChorusLevel;
        Grid ChorusGM2ChorusFeedback;
        TextBlock tbChorusGM2ChorusFeedback;
        Slider slChorusGM2ChorusFeedback;
        Grid ChorusGM2ChorusDelay;
        TextBlock tbChorusGM2ChorusDelay;
        Slider slChorusGM2ChorusDelay;
        Grid ChorusGM2ChorusRate;
        TextBlock tbChorusGM2ChorusRate;
        Slider slChorusGM2ChorusRate;
        Grid ChorusGM2ChorusDepth;
        TextBlock tbChorusGM2ChorusDepth;
        Slider slChorusGM2ChorusDepth;
        Grid ChorusGM2ChorusSendLevelToReverb;
        TextBlock tbChorusGM2ChorusSendLevelToReverb;
        Slider slChorusGM2ChorusSendLevelToReverb;

        //---------------------------------------------------------------------------------------
        // Reverb
        Grid Reverb;
        TextBlock tbStudioSetReverbType;
        ComboBox cbStudioSetReverbType;
        TextBlock tbStudioSetReverbLevel;
        Slider slStudioSetReverbLevel;
        TextBlock tbStudioSetReverbOutputAssign;
        ComboBox cbStudioSetReverbOutputAssign;
        // Reverb reverb
        Grid StudioSetReverbReverb;
        TextBlock tbStudioSetReverbPreDelay;
        Slider slStudioSetReverbPreDelay;
        TextBlock tbStudioSetReverbTime;
        Slider slStudioSetReverbTime;
        TextBlock tbStudioSetReverbDensity;
        Slider slStudioSetReverbDensity;
        TextBlock tbStudioSetReverbDiffusion;
        Slider slStudioSetReverbDiffusion;
        TextBlock tbStudioSetReverbLFDamp;
        Slider slStudioSetReverbLFDamp;
        TextBlock tbStudioSetReverbHFDamp;
        Slider slStudioSetReverbHFDamp;
        TextBlock tbStudioSetReverbSpread;
        Slider slStudioSetReverbSpread;
        TextBlock tbStudioSetReverbTone;
        Slider slStudioSetReverbTone;
        // Reverb GM2
        Grid StudioSetReverbGM2;
        TextBlock tbStudioSetReverbGM2Character;
        Slider slStudioSetReverbGM2Character;
        TextBlock tbStudioSetReverbGM2Time;
        Slider slStudioSetReverbGM2Time;

        //---------------------------------------------------------------------------------------
        // Motional surround
        Grid StudioSetMotionalSurround;
        TextBlock tbStudioSetMotionalSurroundCommon;
        TextBlock tbStudioSetMotionalSurroundExt;
        TextBlock tbStudioSetMotionalSurroundControl;
        CheckBox cbStudioSetMotionalSurround;
        TextBlock tbStudioSetMotionalSurroundRoomType;
        ComboBox cbStudioSetMotionalSurroundRoomType;
        TextBlock tbStudioSetMotionalSurroundAmbienceLevel;
        Slider slStudioSetMotionalSurroundAmbienceLevel;
        TextBlock tbStudioSetMotionalSurroundRoomSize;
        ComboBox cbStudioSetMotionalSurroundRoomSize;
        TextBlock tbStudioSetMotionalSurroundAmbienceTime;
        Slider slStudioSetMotionalSurroundAmbienceTime;
        TextBlock tbStudioSetMotionalSurroundAmbienceDensity;
        Slider slStudioSetMotionalSurroundAmbienceDensity;
        TextBlock tbStudioSetMotionalSurroundAmbienceHFDamp;
        Slider slStudioSetMotionalSurroundAmbienceHFDamp;
        TextBlock tbStudioSetMotionalSurroundExternalPartLR;
        Slider slStudioSetMotionalSurroundExternalPartLR;
        TextBlock tbStudioSetMotionalSurroundExternalPartFB;
        Slider slStudioSetMotionalSurroundExternalPartFB;
        TextBlock tbStudioSetMotionalSurroundExtPartWidth;
        Slider slStudioSetMotionalSurroundExtPartWidth;
        TextBlock tbStudioSetMotionalSurroundExtpartAmbienceSend;
        Slider slStudioSetMotionalSurroundExtpartAmbienceSend;
        TextBlock tbStudioSetMotionalSurroundExtPartControl;
        ComboBox cbStudioSetMotionalSurroundExtPartControl;
        TextBlock tbStudioSetMotionalSurroundDepth;
        Slider slStudioSetMotionalSurroundDepth;

        //---------------------------------------------------------------------------------------
        // Master EQ
        Grid StudioSetMasterEQ;
        TextBlock tbStudioSetMasterEqLowFreq;
        ComboBox cbStudioSetMasterEqLowFreq;
        TextBlock tbStudioSetMasterEqLowGain;
        Slider slStudioSetMasterEqLowGain;
        TextBlock tbStudioSetMasterEqMidFreq;
        ComboBox cbStudioSetMasterEqMidFreq;
        TextBlock tbStudioSetMasterEqMidGain;
        Slider slStudioSetMasterEqMidGain;
        TextBlock tbStudioSetMasterEqMidQ;
        ComboBox cbStudioSetMasterEqMidQ;
        TextBlock tbStudioSetMasterEqHighFreq;
        ComboBox cbStudioSetMasterEqHighFreq;
        TextBlock tbStudioSetMasterEqHighGain;
        Slider slStudioSetMasterEqHighGain;

        //---------------------------------------------------------------------------------------
        // Column 2
        //---------------------------------------------------------------------------------------

        //Grid grid_StudioSet_Column2;
        Grid grid_PartSelector;
        ComboBox cbStudioSetPartSelector;          // For selectin part to work on
        Grid grid_PartSettings;
        ComboBox cbStudioSetPartSubSelector;       // For selecting parameter page for selected part
        Grid grid_StudioSetPartSubSelector;
        TextBlock StudioSetCurrentToneName;
        Grid StudioSetPartSettings1;
        LabeledSwitch cbStudioSetPartSettings1Receive;
        ComboBox cbStudioSetPartSettings1ReceiveChannel;
        ColumnDefinition columndefinition_0701;
        ColumnDefinition columndefinition_0702;
        TextBlock tbStudioSetPartSettings1Group;
        ComboBox cbStudioSetPartSettings1Group;
        ColumnDefinition columndefinition_0705;
        ColumnDefinition columndefinition_0706;
        ColumnDefinition columndefinition_0707;
        TextBlock tbStudioSetPartSettings1Category;
        ComboBox cbStudioSetPartSettings1Category;
        ColumnDefinition columndefinition_0710;
        ColumnDefinition columndefinition_0711;
        TextBlock tbStudioSetPartSettings1Program;
        ComboBox cbStudioSetPartSettings1Program;
        ColumnDefinition columndefinition_0714;
        ColumnDefinition columndefinition_0715;
        TextBlock tbStudioSetPartSettings1Search;
        TextBox cbStudioSetPartSettings1Search;
        ColumnDefinition columndefinition_0718;
        ColumnDefinition columndefinition_0719;
        TextBlock tbStudioSetPartSettings1Level;
        Slider slStudioSetPartSettings1Level;
        ColumnDefinition columndefinition_0722;
        ColumnDefinition columndefinition_0723;
        TextBlock tbStudioSetPartSettings1Pan;
        Slider slStudioSetPartSettings1Pan;
        ColumnDefinition columndefinition_0726;
        ColumnDefinition columndefinition_0727;
        TextBlock tbStudioSetPartSettings1CoarseTune;
        Slider slStudioSetPartSettings1CoarseTune;
        ColumnDefinition columndefinition_0730;
        ColumnDefinition columndefinition_0731;
        TextBlock tbStudioSetPartSettings1FineTune;
        Slider slStudioSetPartSettings1FineTune;
        ColumnDefinition columndefinition_0734;
        ColumnDefinition columndefinition_0735;
        TextBlock tbStudioSetPartSettings1MonoPoly;
        ComboBox cbStudioSetPartSettings1MonoPoly;
        ColumnDefinition columndefinition_0739;
        ColumnDefinition columndefinition_0740;
        TextBlock tbStudioSetPartSettings1Legato;
        ComboBox cbStudioSetPartSettings1Legato;
        ColumnDefinition columndefinition_0744;
        ColumnDefinition columndefinition_0745;
        TextBlock tbStudioSetPartSettings1PitchBendRange;
        Slider slStudioSetPartSettings1PitchBendRange;
        ColumnDefinition columndefinition_0748;
        ColumnDefinition columndefinition_0749;
        TextBlock tbStudioSetPartSettings1Portamento;
        ComboBox cbStudioSetPartSettings1Portamento;
        ColumnDefinition columndefinition_0753;
        ColumnDefinition columndefinition_0754;
        TextBlock tbStudioSetPartSettings1PortamentoTime;
        Slider slStudioSetPartSettings1PortamentoTime;
        Grid StudioSetPartSettings2;
        TextBlock tbStudioSetPartSettings2CutoffOffset;
        Slider slStudioSetPartSettings2CutoffOffset;
        TextBlock tbStudioSetPartSettings2ResonanceOffset;
        Slider slStudioSetPartSettings2ResonanceOffset;
        TextBlock tbStudioSetPartSettings2AttackTimeOffset;
        Slider slStudioSetPartSettings2AttackTimeOffset;
        TextBlock tbStudioSetPartSettings2DecayTimeOffset;
        Slider slStudioSetPartSettings2DecayTimeOffset;
        TextBlock tbStudioSetPartSettings2ReleaseTimeOffset;
        Slider slStudioSetPartSettings2ReleaseTimeOffset;

        TextBlock tbStudioSetPartSettings2VibratoRate;
        Slider slStudioSetPartSettings2VibratoRate;
        TextBlock tbStudioSetPartSettings2VibratoDepth;
        Slider slStudioSetPartSettings2VibratoDepth;
        TextBlock tbStudioSetPartSettings2VibratoDelay;
        Slider slStudioSetPartSettings2VibratoDelay;
        Grid StudioSetPartEffects;
        TextBlock tbStudioSetPartEffectsChorusSendLevel;
        Slider slStudioSetPartEffectsChorusSendLevel;
        TextBlock tbStudioSetPartEffectsReverbSendLevel;
        Slider slStudioSetPartEffectsReverbSendLevel;
        TextBlock tbStudioSetPartEffectsOutputAssign;
        ComboBox cbStudioSetPartEffectsOutputAssign;
        Grid StudioSetPartKeyboard;
        TextBlock tbStudioSetPartKeyboardOctaveShift;
        Slider slStudioSetPartKeyboardOctaveShift;
        TextBlock tbStudioSetPartKeyboardVelocitySenseOffset;
        Slider slStudioSetPartKeyboardVelocitySenseOffset;
        TextBlock tbStudioSetPartKeyboardRangeLower;
        Slider slStudioSetPartKeyboardRangeLower;
        TextBlock tbStudioSetPartKeyboardRangeUpper;
        Slider slStudioSetPartKeyboardRangeUpper;
        TextBlock tbStudioSetPartKeyboardFadeWidthLower;
        Slider slStudioSetPartKeyboardFadeWidthLower;
        TextBlock tbStudioSetPartKeyboardFadeWidthUpper;
        Slider slStudioSetPartKeyboardFadeWidthUpper;
        TextBlock tbStudioSetPartKeyboardVelocityRangeLower;
        Slider slStudioSetPartKeyboardVelocityRangeLower;
        TextBlock tbStudioSetPartKeyboardVelocityRangeUpper;
        Slider slStudioSetPartKeyboardVelocityRangeUpper;
        TextBlock tbStudioSetPartKeyboardVelocityFadeWidthLower;
        Slider slStudioSetPartKeyboardVelocityFadeWidthLower;
        TextBlock tbStudioSetPartKeyboardVelocityFadeWidthUpper;
        Slider slStudioSetPartKeyboardVelocityFadeWidthUpper;
        CheckBox cbStudioSetPartKeyboardMute;
        TextBlock tbStudioSetPartKeyboardVelocityCurveType;
        ComboBox cbStudioSetPartKeyboardVelocityCurveType;
        Grid StudioSetPartScaleTune;
        TextBlock tbStudioSetPartScaleTuneType;
        ComboBox cbStudioSetPartScaleTuneType;
        TextBlock tbStudioSetPartScaleTuneKey;
        ComboBox cbStudioSetPartScaleTuneKey;
        TextBlock tbStudioSetPartScaleTuneC;
        Slider slStudioSetPartScaleTuneC;
        TextBlock tbStudioSetPartScaleTuneCi;
        Slider slStudioSetPartScaleTuneCi;
        TextBlock tbStudioSetPartScaleTuneD;
        Slider slStudioSetPartScaleTuneD;
        TextBlock tbStudioSetPartScaleTuneDi;
        Slider slStudioSetPartScaleTuneDi;
        TextBlock tbStudioSetPartScaleTuneE;
        Slider slStudioSetPartScaleTuneE;
        TextBlock tbStudioSetPartScaleTuneF;
        Slider slStudioSetPartScaleTuneF;
        TextBlock tbStudioSetPartScaleTuneFi;
        Slider slStudioSetPartScaleTuneFi;
        TextBlock tbStudioSetPartScaleTuneG;
        Slider slStudioSetPartScaleTuneG;
        TextBlock tbStudioSetPartScaleTuneGi;
        Slider slStudioSetPartScaleTuneGi;
        TextBlock tbStudioSetPartScaleTuneA;
        Slider slStudioSetPartScaleTuneA;
        TextBlock tbStudioSetPartScaleTuneAi;
        Slider slStudioSetPartScaleTuneAi;
        TextBlock tbStudioSetPartScaleTuneB;
        Slider slStudioSetPartScaleTuneB;
        Grid StudioSetPartMidi;
        LabeledSwitch cbStudioSetPartMidiPhaseLock;
        LabeledSwitch cbStudioSetPartMidiReceiveProgramChange;
        LabeledSwitch cbStudioSetPartMidiReceiveBankSelect;
        LabeledSwitch cbStudioSetPartMidiReceivePitchBend;
        LabeledSwitch cbStudioSetPartMidiReceivePolyphonicKeyPressure;
        LabeledSwitch cbStudioSetPartMidiReceiveChannelPressure;
        LabeledSwitch cbStudioSetPartMidiReceiveModulation;
        LabeledSwitch cbStudioSetPartMidiReceiveVolume;
        LabeledSwitch cbStudioSetPartMidiReceivePan;
        LabeledSwitch cbStudioSetPartMidiReceiveExpression;
        LabeledSwitch cbStudioSetPartMidiReceiveHold1;
        Grid gStudioSetPartVelocityCurve;
        Image imgVelocityCurve1On;
        Image imgVelocityCurve1Off;
        Image imgVelocityCurve2On;
        Image imgVelocityCurve2Off;
        Image imgVelocityCurve3On;
        Image imgVelocityCurve3Off;
        Image imgVelocityCurve4On;
        Image imgVelocityCurve4Off;
        Button btnVelocityCurve1;
        Button btnVelocityCurve2;
        Button btnVelocityCurve3;
        Button btnVelocityCurve4;
        Grid StudioSetPartMotionalSurround;
        TextBlock tbStudioSetPartMotionalSurroundLR;
        Slider slStudioSetPartMotionalSurroundLR;
        TextBlock tbStudioSetPartMotionalSurroundFB;
        Slider slStudioSetPartMotionalSurroundFB;
        TextBlock tbStudioSetPartMotionalSurroundWidth;
        Slider slStudioSetPartMotionalSurroundWidth;
        TextBlock tbStudioSetPartMotionalSurroundAmbienceSendLevel;
        Slider slStudioSetPartMotionalSurroundAmbienceSendLevel;
        Grid StudioSetPartEQ;
        CheckBox cbStudioSetPartEQSwitch;
        TextBlock tbStudioSetPartEQLowFreq;
        ComboBox cbStudioSetPartEQLowFreq;
        TextBlock tbStudioSetPartEQLowGain;
        Slider slStudioSetPartEQLowGain;
        TextBlock tbStudioSetPartEQMidFreq;
        ComboBox cbStudioSetPartEQMidFreq;
        TextBlock tbStudioSetPartEQMidGain;
        Slider slStudioSetPartEQMidGain;
        TextBlock tbStudioSetPartEQMidQ;
        ComboBox cbStudioSetPartEQMidQ;
        TextBlock tbStudioSetPartEQHighFreq;
        ComboBox cbStudioSetPartEQHighFreq;
        TextBlock tbStudioSetPartEQHighGain;
        Slider slStudioSetPartEQHighGain;
        ComboBox cbStudioSetSlot;
        TextBlock lblStudioSetName;
        TextBox tbStudioSetName;
        Grid grid_Buttons;
        Button btnFileSave;
        Button btnFileLoad;
        Button btnStudioSetSave;
        Button btnStudioSetDelete;
        Button btnStudioSetReturn;

        Boolean setVisibility = false;
        public void ShowStudioSetEditorPage()
        {
            //currentPage = CurrentPage.EDIT_STUDIO_SET;
            if (!EditStudioSet_IsCreated)
            {
                initDone = false;
                initialGuiDone = false;
                studioSetNumberTemp = 0;
                PushHandleControlEvents();
                DrawStudioSetEditorPage();
                StudioSetEditor_StackLayout.MinimumWidthRequest = 1;
                mainStackLayout.Children.Add(StudioSetEditor_StackLayout);
                StudioSetEditor_StackLayout.IsVisible = false;
                EditStudioSet_IsCreated = true;
                PopHandleControlEvents();
                commonState.StudioSet = new StudioSet();
                StudioSetEditor_Init();
                PopulateComboBoxes();
                setVisibility = true;
            }
            StudioSetEditor_StackLayout.IsVisible = false;
        }

        /*
        StudioSetEditor_StackLayout grid hierarchy (controls omitted for clarity):
            grid_StudioSet_Column0

            grid_StudioSet_Column1
                gStudioSet_Column1
                    SystemCommonSettings
                    VoiceReserve
                    Chorus
                        ChorusChorus
                        ChorusDelay
                        ChorusGM2Chorus
                    Reverb
                        StudioSetReverbReverb
                        StudioSetReverbGM2
                    StudioSetMotionalSurround
                    StudioSetMasterEQ
                    gEditStudioSetSearchResult

            grid_StudioSet_Column2
                grid_PartSettings
                    StudioSetPartSettings1
                    StudioSetPartSettings2
                    StudioSetPartEffects
                    StudioSetPartKeyboard
                    StudioSetPartScaleTune
                    StudioSetPartMidi
                        gStudioSetPartVelocityCurve
                    StudioSetPartMotionalSurround
                    StudioSetPartEQ
                grid_Buttons
        */

        /// </summary>
        public void DrawStudioSetEditorPage()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Edit studio set
            // ____________________________________________________________________________________________
            // |                                                                                          |
            // |__________________________________________________________________________________________|


            //---------------------------------------------------------------------------------------
            // Create layout objects 
            //---------------------------------------------------------------------------------------

            try
            {
                grid_StudioSet_Column0 = new Grid();
                grid_StudioSet_Column1 = new Grid();   // This will be used in column 1 and contain
                                                       // two subgrids, gStudioSet_Column1 and
                                                       // gEditStudioSetSearchResult, one visible
                                                       // at each time depending on search mode
                grid_StudioSet_Column2 = new Grid();
                for (Int32 i = 0; i < 19; i++)
                {
                    grid_StudioSet_Column0.RowDefinitions.Add(new RowDefinition());
                    grid_StudioSet_Column1.RowDefinitions.Add(new RowDefinition());
                    grid_StudioSet_Column2.RowDefinitions.Add(new RowDefinition());
                }

                //---------------------------------------------------------------------------------------
                // Create all controls 
                //---------------------------------------------------------------------------------------

                //---------------------------------------------------------------------------------------
                // Column 0 
                //---------------------------------------------------------------------------------------

                cbStudioSetSelector = new ComboBox();
                tbToneControl1 = new TextBlock();
                cbToneControl1 = new ComboBox();
                tbToneControl2 = new TextBlock();
                cbToneControl2 = new ComboBox();
                tbToneControl3 = new TextBlock();
                cbToneControl3 = new ComboBox();
                tbToneControl4 = new TextBlock();
                cbToneControl4 = new ComboBox();
                tbTempo = new TextBlock();
                slTempo = new Slider();
                tbSoloPart = new TextBlock();
                cbSoloPart = new ComboBox();
                cbReverb = new LabeledSwitch("Reverb");
                cbChorus = new LabeledSwitch("Chorus");
                cbMasterEQ = new LabeledSwitch("Master EQ");
                tbDrumCompEQPart = new TextBlock();
                cbDrumCompEQPart = new ComboBox();
                tbDrumCompEQ1OutputAssign = new TextBlock();
                cbDrumCompEQ1OutputAssign = new ComboBox();
                tbDrumCompEQ2OutputAssign = new TextBlock();
                cbDrumCompEQ2OutputAssign = new ComboBox();
                tbDrumCompEQ3OutputAssign = new TextBlock();
                cbDrumCompEQ3OutputAssign = new ComboBox();
                tbDrumCompEQ4OutputAssign = new TextBlock();
                cbDrumCompEQ4OutputAssign = new ComboBox();
                tbDrumCompEQ5OutputAssign = new TextBlock();
                cbDrumCompEQ5OutputAssign = new ComboBox();
                tbDrumCompEQ6OutputAssign = new TextBlock();
                cbDrumCompEQ6OutputAssign = new ComboBox();
                cbDrumCompEQ = new LabeledSwitch("Drum Comp/Eq");
                cbExtPartMute = new LabeledSwitch("Ext mute");
                tbExtPartLevel = new TextBlock();
                slExtPartLevel = new Slider();
                tbExtPartChorusSend = new TextBlock();
                slExtPartChorusSend = new Slider();
                tbExtPartReverbSend = new TextBlock();
                slExtPartReverbSend = new Slider();

                //---------------------------------------------------------------------------------------
                // Column 1 
                //---------------------------------------------------------------------------------------

                gStudioSet_Column1 = new Grid();
                gEditStudioSetSearchResult = new Grid();

                cbColumn1Selector = new ComboBox();
                lvSearchResults = new ListView();  // The listview in grid gEditStudioSetSearchResult

                //---------------------------------------------------------------------------------------
                // System common settings 
                SystemCommonSettings = new Grid();
                tbSystemCommonMasterTune = new TextBlock();
                slSystemCommonMasterTune = new Slider();
                tbSystemCommonMasterKeyShift = new TextBlock();
                slSystemCommonMasterKeyShift = new Slider();
                tbSystemCommonMasterLevel = new TextBlock();
                slSystemCommonMasterLevel = new Slider();
                cbSystemCommonScaleTuneSwitch = new LabeledSwitch("Use scale tune (as set in parts)", null, new byte[] { 3, 1 });
                tbSystemCommonStudioSetControlChannel = new TextBlock();
                cbSystemCommonStudioSetControlChannel = new ComboBox();
                tbSystemCommonSystemControlSource1 = new TextBlock();
                cbSystemCommonSystemControlSource1 = new ComboBox();
                tbSystemCommonSystemControlSource2 = new TextBlock();
                cbSystemCommonSystemControlSource2 = new ComboBox();
                tbSystemCommonSystemControlSource3 = new TextBlock();
                cbSystemCommonSystemControlSource3 = new ComboBox();
                tbSystemCommonSystemControlSource4 = new TextBlock();
                cbSystemCommonSystemControlSource4 = new ComboBox();
                tbSystemCommonControlSource = new TextBlock();
                cbSystemCommonControlSource = new ComboBox();
                tbSystemCommonSystemClockSource = new TextBlock();
                cbSystemCommonSystemClockSource = new ComboBox();
                tbSystemCommonSystemTempo = new TextBlock();
                slSystemCommonSystemTempo = new Slider();
                tbTempoAssignSource = new TextBlock();
                cbSystemCommonTempoAssignSource = new ComboBox();
                cbSystemCommonReceiveProgramChange = new LabeledSwitch("Recieve program change", null, new byte[] { 3, 1 });
                cbSystemCommonReceiveBankSelect = new LabeledSwitch("Receive bank select", null, new byte[] { 3, 1 });
                cbSystemCommonSurroundCenterSpeakerSwitch = new LabeledSwitch("5.1Ch center speaker", null, new byte[] { 3, 1 });
                cbSystemCommonSurroundSubWooferSwitch = new LabeledSwitch("5.1Ch sub woofer", null, new byte[] { 3, 1 });
                tbSystemCommonStereoOutputMode = new TextBlock();
                cbSystemCommonStereoOutputMode = new ComboBox();

                //---------------------------------------------------------------------------------------
                // Voice reserve settings 
                VoiceReserve = new Grid();
                tbVoiceReserve01 = new TextBlock();
                slVoiceReserve01 = new Slider();
                tbVoiceReserve02 = new TextBlock();
                slVoiceReserve02 = new Slider();
                tbVoiceReserve03 = new TextBlock();
                slVoiceReserve03 = new Slider();
                tbVoiceReserve04 = new TextBlock();
                slVoiceReserve04 = new Slider();
                tbVoiceReserve05 = new TextBlock();
                slVoiceReserve05 = new Slider();
                tbVoiceReserve06 = new TextBlock();
                slVoiceReserve06 = new Slider();
                tbVoiceReserve07 = new TextBlock();
                slVoiceReserve07 = new Slider();
                tbVoiceReserve08 = new TextBlock();
                slVoiceReserve08 = new Slider();
                tbVoiceReserve09 = new TextBlock();
                slVoiceReserve09 = new Slider();
                tbVoiceReserve10 = new TextBlock();
                slVoiceReserve10 = new Slider();
                tbVoiceReserve11 = new TextBlock();
                slVoiceReserve11 = new Slider();
                tbVoiceReserve12 = new TextBlock();
                slVoiceReserve12 = new Slider();
                tbVoiceReserve13 = new TextBlock();
                slVoiceReserve13 = new Slider();
                tbVoiceReserve14 = new TextBlock();
                slVoiceReserve14 = new Slider();
                tbVoiceReserve15 = new TextBlock();
                slVoiceReserve15 = new Slider();
                tbVoiceReserve16 = new TextBlock();
                slVoiceReserve16 = new Slider();

                //---------------------------------------------------------------------------------------
                // Chorus settings
                Chorus = new Grid();
                tbStudioSetChorusType = new TextBlock();
                cbStudioSetChorusType = new ComboBox();
                tbChorusLevel = new TextBlock();
                slChorusLevel = new Slider();
                tbStudioSetChorusOutputAssign = new TextBlock();
                cbChorusOutputAssign = new ComboBox();
                tbChorusOutputSelect = new TextBlock();
                cbChorusOutputSelect = new ComboBox();
                // Chorus chorus settings 
                ChorusChorus = new Grid();
                tbChorusChorusFilterType = new TextBlock();
                cbChorusChorusFilterType = new ComboBox();
                tbChorusChorusFilterCutoffFrequency = new TextBlock();
                cbChorusChorusFilterCutoffFrequency = new ComboBox();
                tbChorusChorusPreDelay = new TextBlock();
                slChorusChorusPreDelay = new Slider();
                tbChorusChorusRateHzNote = new TextBlock();
                cbChorusChorusRateHzNote = new ComboBox();
                tbChorusChorusRateHz = new TextBlock();
                slChorusChorusRateHz = new Slider();
                tbChorusChorusRateNote = new TextBlock();
                slChorusChorusRateNote = new Slider();
                tbChorusChorusDepth = new TextBlock();
                slChorusChorusDepth = new Slider();
                tbChorusChorusPhase = new TextBlock();
                slChorusChorusPhase = new Slider();
                tbChorusChorusFeedback = new TextBlock();
                slChorusChorusFeedback = new Slider();
                // Chorus delay settings 
                ChorusDelay = new Grid();
                tbChorusDelayLeftMsNote = new TextBlock();
                cbChorusDelayLeftMsNote = new ComboBox();
                tbChorusDelayLeftHz = new TextBlock();
                slChorusDelayLeftHz = new Slider();
                tbChorusDelayLeftNote = new TextBlock();
                slChorusDelayLeftNote = new Slider();
                tbChorusDelayRightMsNote = new TextBlock();
                cbChorusDelayRightMsNote = new ComboBox();
                tbChorusDelayRightHz = new TextBlock();
                slChorusDelayRightHz = new Slider();
                tbChorusDelayRightNote = new TextBlock();
                slChorusDelayRightNote = new Slider();
                tbChorusDelayCenterMsNote = new TextBlock();
                cbChorusDelayCenterMsNote = new ComboBox();
                tbChorusDelayCenterHz = new TextBlock();
                slChorusDelayCenterHz = new Slider();
                tbChorusDelayCenterNote = new TextBlock();
                slChorusDelayCenterNote = new Slider();
                tbChorusDelayCenterFeedback = new TextBlock();
                slChorusDelayCenterFeedback = new Slider();
                tbChorusDelayHFDamp = new TextBlock();
                cbChorusDelayHFDamp = new ComboBox();
                tbChorusDelayLeftLevel = new TextBlock();
                slChorusDelayLeftLevel = new Slider();
                tbChorusDelayRightLevel = new TextBlock();
                slChorusDelayRightLevel = new Slider();
                tbChorusDelayCenterLevel = new TextBlock();
                slChorusDelayCenterLevel = new Slider();
                // Chorus GM2 settings
                ChorusGM2Chorus = new Grid();
                ChorusGM2ChorusPreLPF = new Grid();
                tbChorusGM2ChorusPreLPF = new TextBlock();
                slChorusGM2ChorusPreLPF = new Slider();
                ChorusGM2ChorusLevel = new Grid();
                tbChorusGM2ChorusLevel = new TextBlock();
                slChorusGM2ChorusLevel = new Slider();
                ChorusGM2ChorusFeedback = new Grid();
                tbChorusGM2ChorusFeedback = new TextBlock();
                slChorusGM2ChorusFeedback = new Slider();
                ChorusGM2ChorusDelay = new Grid();
                tbChorusGM2ChorusDelay = new TextBlock();
                slChorusGM2ChorusDelay = new Slider();
                ChorusGM2ChorusRate = new Grid();
                tbChorusGM2ChorusRate = new TextBlock();
                slChorusGM2ChorusRate = new Slider();
                ChorusGM2ChorusDepth = new Grid();
                tbChorusGM2ChorusDepth = new TextBlock();
                slChorusGM2ChorusDepth = new Slider();
                ChorusGM2ChorusSendLevelToReverb = new Grid();
                tbChorusGM2ChorusSendLevelToReverb = new TextBlock();
                slChorusGM2ChorusSendLevelToReverb = new Slider();

                //---------------------------------------------------------------------------------------
                // Reverb
                Reverb = new Grid();
                tbStudioSetReverbType = new TextBlock();
                cbStudioSetReverbType = new ComboBox();
                tbStudioSetReverbLevel = new TextBlock();
                slStudioSetReverbLevel = new Slider();
                tbStudioSetReverbOutputAssign = new TextBlock();
                cbStudioSetReverbOutputAssign = new ComboBox();
                // Reverb reverb
                StudioSetReverbReverb = new Grid();
                tbStudioSetReverbPreDelay = new TextBlock();
                slStudioSetReverbPreDelay = new Slider();
                tbStudioSetReverbTime = new TextBlock();
                slStudioSetReverbTime = new Slider();
                tbStudioSetReverbDensity = new TextBlock();
                slStudioSetReverbDensity = new Slider();
                tbStudioSetReverbDiffusion = new TextBlock();
                slStudioSetReverbDiffusion = new Slider();
                tbStudioSetReverbLFDamp = new TextBlock();
                slStudioSetReverbLFDamp = new Slider();
                tbStudioSetReverbHFDamp = new TextBlock();
                slStudioSetReverbHFDamp = new Slider();
                tbStudioSetReverbSpread = new TextBlock();
                slStudioSetReverbSpread = new Slider();
                tbStudioSetReverbTone = new TextBlock();
                slStudioSetReverbTone = new Slider();
                // Reverb GM2
                StudioSetReverbGM2 = new Grid();
                tbStudioSetReverbGM2Character = new TextBlock();
                slStudioSetReverbGM2Character = new Slider();
                tbStudioSetReverbGM2Time = new TextBlock();
                slStudioSetReverbGM2Time = new Slider();

                //---------------------------------------------------------------------------------------
                // Motional surround
                StudioSetMotionalSurround = new Grid();
                tbStudioSetMotionalSurroundCommon = new TextBlock();
                tbStudioSetMotionalSurroundExt = new TextBlock();
                tbStudioSetMotionalSurroundControl = new TextBlock();
                cbStudioSetMotionalSurround = new CheckBox();
                tbStudioSetMotionalSurroundRoomType = new TextBlock();
                cbStudioSetMotionalSurroundRoomType = new ComboBox();
                tbStudioSetMotionalSurroundAmbienceLevel = new TextBlock();
                slStudioSetMotionalSurroundAmbienceLevel = new Slider();
                tbStudioSetMotionalSurroundRoomSize = new TextBlock();
                cbStudioSetMotionalSurroundRoomSize = new ComboBox();
                tbStudioSetMotionalSurroundAmbienceTime = new TextBlock();
                slStudioSetMotionalSurroundAmbienceTime = new Slider();
                tbStudioSetMotionalSurroundAmbienceDensity = new TextBlock();
                slStudioSetMotionalSurroundAmbienceDensity = new Slider();
                tbStudioSetMotionalSurroundAmbienceHFDamp = new TextBlock();
                slStudioSetMotionalSurroundAmbienceHFDamp = new Slider();
                tbStudioSetMotionalSurroundExternalPartLR = new TextBlock();
                slStudioSetMotionalSurroundExternalPartLR = new Slider();
                tbStudioSetMotionalSurroundExternalPartFB = new TextBlock();
                slStudioSetMotionalSurroundExternalPartFB = new Slider();
                tbStudioSetMotionalSurroundExtPartWidth = new TextBlock();
                slStudioSetMotionalSurroundExtPartWidth = new Slider();
                tbStudioSetMotionalSurroundExtpartAmbienceSend = new TextBlock();
                slStudioSetMotionalSurroundExtpartAmbienceSend = new Slider();
                tbStudioSetMotionalSurroundExtPartControl = new TextBlock();
                cbStudioSetMotionalSurroundExtPartControl = new ComboBox();
                tbStudioSetMotionalSurroundDepth = new TextBlock();
                slStudioSetMotionalSurroundDepth = new Slider();

                //---------------------------------------------------------------------------------------
                // Master EQ
                StudioSetMasterEQ = new Grid();
                tbStudioSetMasterEqLowFreq = new TextBlock();
                cbStudioSetMasterEqLowFreq = new ComboBox();
                tbStudioSetMasterEqLowGain = new TextBlock();
                slStudioSetMasterEqLowGain = new Slider();
                tbStudioSetMasterEqMidFreq = new TextBlock();
                cbStudioSetMasterEqMidFreq = new ComboBox();
                tbStudioSetMasterEqMidGain = new TextBlock();
                slStudioSetMasterEqMidGain = new Slider();
                tbStudioSetMasterEqMidQ = new TextBlock();
                cbStudioSetMasterEqMidQ = new ComboBox();
                tbStudioSetMasterEqHighFreq = new TextBlock();
                cbStudioSetMasterEqHighFreq = new ComboBox();
                tbStudioSetMasterEqHighGain = new TextBlock();
                slStudioSetMasterEqHighGain = new Slider();

                //---------------------------------------------------------------------------------------
                // Column 2
                //---------------------------------------------------------------------------------------

                grid_PartSelector = new Grid();
                cbStudioSetPartSelector = new ComboBox();          // For selectin part to work on
                grid_PartSettings = new Grid();
                cbStudioSetPartSubSelector = new ComboBox();       // For selecting parameter page for selected part
                grid_StudioSetPartSubSelector = new Grid();
                StudioSetCurrentToneName = new TextBlock();
                StudioSetPartSettings1 = new Grid();
                cbStudioSetPartSettings1Receive = new LabeledSwitch("Receive");
                cbStudioSetPartSettings1ReceiveChannel = new ComboBox();
                columndefinition_0701 = new ColumnDefinition();
                columndefinition_0702 = new ColumnDefinition();
                tbStudioSetPartSettings1Group = new TextBlock();
                cbStudioSetPartSettings1Group = new ComboBox();
                columndefinition_0705 = new ColumnDefinition();
                columndefinition_0706 = new ColumnDefinition();
                columndefinition_0707 = new ColumnDefinition();
                tbStudioSetPartSettings1Category = new TextBlock();
                cbStudioSetPartSettings1Category = new ComboBox();
                columndefinition_0710 = new ColumnDefinition();
                columndefinition_0711 = new ColumnDefinition();
                tbStudioSetPartSettings1Program = new TextBlock();
                cbStudioSetPartSettings1Program = new ComboBox();
                columndefinition_0714 = new ColumnDefinition();
                columndefinition_0715 = new ColumnDefinition();
                tbStudioSetPartSettings1Search = new TextBlock();
                cbStudioSetPartSettings1Search = new TextBox();
                columndefinition_0718 = new ColumnDefinition();
                columndefinition_0719 = new ColumnDefinition();
                tbStudioSetPartSettings1Level = new TextBlock();
                slStudioSetPartSettings1Level = new Slider();
                columndefinition_0722 = new ColumnDefinition();
                columndefinition_0723 = new ColumnDefinition();
                tbStudioSetPartSettings1Pan = new TextBlock();
                slStudioSetPartSettings1Pan = new Slider();
                columndefinition_0726 = new ColumnDefinition();
                columndefinition_0727 = new ColumnDefinition();
                tbStudioSetPartSettings1CoarseTune = new TextBlock();
                slStudioSetPartSettings1CoarseTune = new Slider();
                columndefinition_0730 = new ColumnDefinition();
                columndefinition_0731 = new ColumnDefinition();
                tbStudioSetPartSettings1FineTune = new TextBlock();
                slStudioSetPartSettings1FineTune = new Slider();
                columndefinition_0734 = new ColumnDefinition();
                columndefinition_0735 = new ColumnDefinition();
                tbStudioSetPartSettings1MonoPoly = new TextBlock();
                cbStudioSetPartSettings1MonoPoly = new ComboBox();
                columndefinition_0739 = new ColumnDefinition();
                columndefinition_0740 = new ColumnDefinition();
                tbStudioSetPartSettings1Legato = new TextBlock();
                cbStudioSetPartSettings1Legato = new ComboBox();
                columndefinition_0744 = new ColumnDefinition();
                columndefinition_0745 = new ColumnDefinition();
                tbStudioSetPartSettings1PitchBendRange = new TextBlock();
                slStudioSetPartSettings1PitchBendRange = new Slider();
                columndefinition_0748 = new ColumnDefinition();
                columndefinition_0749 = new ColumnDefinition();
                tbStudioSetPartSettings1Portamento = new TextBlock();
                cbStudioSetPartSettings1Portamento = new ComboBox();
                columndefinition_0753 = new ColumnDefinition();
                columndefinition_0754 = new ColumnDefinition();
                tbStudioSetPartSettings1PortamentoTime = new TextBlock();
                slStudioSetPartSettings1PortamentoTime = new Slider();
                StudioSetPartSettings2 = new Grid();
                tbStudioSetPartSettings2CutoffOffset = new TextBlock();
                slStudioSetPartSettings2CutoffOffset = new Slider();
                tbStudioSetPartSettings2ResonanceOffset = new TextBlock();
                slStudioSetPartSettings2ResonanceOffset = new Slider();
                tbStudioSetPartSettings2AttackTimeOffset = new TextBlock();
                slStudioSetPartSettings2AttackTimeOffset = new Slider();
                tbStudioSetPartSettings2DecayTimeOffset = new TextBlock();
                slStudioSetPartSettings2DecayTimeOffset = new Slider();
                tbStudioSetPartSettings2ReleaseTimeOffset = new TextBlock();
                slStudioSetPartSettings2ReleaseTimeOffset = new Slider();

                tbStudioSetPartSettings2VibratoRate = new TextBlock();
                slStudioSetPartSettings2VibratoRate = new Slider();
                tbStudioSetPartSettings2VibratoDepth = new TextBlock();
                slStudioSetPartSettings2VibratoDepth = new Slider();
                tbStudioSetPartSettings2VibratoDelay = new TextBlock();
                slStudioSetPartSettings2VibratoDelay = new Slider();
                StudioSetPartEffects = new Grid();
                tbStudioSetPartEffectsChorusSendLevel = new TextBlock();
                slStudioSetPartEffectsChorusSendLevel = new Slider();
                tbStudioSetPartEffectsReverbSendLevel = new TextBlock();
                slStudioSetPartEffectsReverbSendLevel = new Slider();
                tbStudioSetPartEffectsOutputAssign = new TextBlock();
                cbStudioSetPartEffectsOutputAssign = new ComboBox();
                StudioSetPartKeyboard = new Grid();
                tbStudioSetPartKeyboardOctaveShift = new TextBlock();
                slStudioSetPartKeyboardOctaveShift = new Slider();
                tbStudioSetPartKeyboardVelocitySenseOffset = new TextBlock();
                slStudioSetPartKeyboardVelocitySenseOffset = new Slider();
                tbStudioSetPartKeyboardRangeLower = new TextBlock();
                slStudioSetPartKeyboardRangeLower = new Slider();
                tbStudioSetPartKeyboardRangeUpper = new TextBlock();
                slStudioSetPartKeyboardRangeUpper = new Slider();
                tbStudioSetPartKeyboardFadeWidthLower = new TextBlock();
                slStudioSetPartKeyboardFadeWidthLower = new Slider();
                tbStudioSetPartKeyboardFadeWidthUpper = new TextBlock();
                slStudioSetPartKeyboardFadeWidthUpper = new Slider();
                tbStudioSetPartKeyboardVelocityRangeLower = new TextBlock();
                slStudioSetPartKeyboardVelocityRangeLower = new Slider();
                tbStudioSetPartKeyboardVelocityRangeUpper = new TextBlock();
                slStudioSetPartKeyboardVelocityRangeUpper = new Slider();
                tbStudioSetPartKeyboardVelocityFadeWidthLower = new TextBlock();
                slStudioSetPartKeyboardVelocityFadeWidthLower = new Slider();
                tbStudioSetPartKeyboardVelocityFadeWidthUpper = new TextBlock();
                slStudioSetPartKeyboardVelocityFadeWidthUpper = new Slider();
                cbStudioSetPartKeyboardMute = new CheckBox();
                tbStudioSetPartKeyboardVelocityCurveType = new TextBlock();
                cbStudioSetPartKeyboardVelocityCurveType = new ComboBox();
                StudioSetPartScaleTune = new Grid();
                tbStudioSetPartScaleTuneType = new TextBlock();
                cbStudioSetPartScaleTuneType = new ComboBox();
                tbStudioSetPartScaleTuneKey = new TextBlock();
                cbStudioSetPartScaleTuneKey = new ComboBox();
                tbStudioSetPartScaleTuneC = new TextBlock();
                slStudioSetPartScaleTuneC = new Slider();
                tbStudioSetPartScaleTuneCi = new TextBlock();
                slStudioSetPartScaleTuneCi = new Slider();
                tbStudioSetPartScaleTuneD = new TextBlock();
                slStudioSetPartScaleTuneD = new Slider();
                tbStudioSetPartScaleTuneDi = new TextBlock();
                slStudioSetPartScaleTuneDi = new Slider();
                tbStudioSetPartScaleTuneE = new TextBlock();
                slStudioSetPartScaleTuneE = new Slider();
                tbStudioSetPartScaleTuneF = new TextBlock();
                slStudioSetPartScaleTuneF = new Slider();
                tbStudioSetPartScaleTuneFi = new TextBlock();
                slStudioSetPartScaleTuneFi = new Slider();
                tbStudioSetPartScaleTuneG = new TextBlock();
                slStudioSetPartScaleTuneG = new Slider();
                tbStudioSetPartScaleTuneGi = new TextBlock();
                slStudioSetPartScaleTuneGi = new Slider();
                tbStudioSetPartScaleTuneA = new TextBlock();
                slStudioSetPartScaleTuneA = new Slider();
                tbStudioSetPartScaleTuneAi = new TextBlock();
                slStudioSetPartScaleTuneAi = new Slider();
                tbStudioSetPartScaleTuneB = new TextBlock();
                slStudioSetPartScaleTuneB = new Slider();
                StudioSetPartMidi = new Grid();
                cbStudioSetPartMidiPhaseLock = new LabeledSwitch("Phase lock");
                cbStudioSetPartMidiReceiveProgramChange = new LabeledSwitch("Receive program change");
                cbStudioSetPartMidiReceiveBankSelect = new LabeledSwitch("Receive bank select");
                cbStudioSetPartMidiReceivePitchBend = new LabeledSwitch("Receive pitch bend");
                cbStudioSetPartMidiReceivePolyphonicKeyPressure = new LabeledSwitch("Receive polyphonic key pressure");
                cbStudioSetPartMidiReceiveChannelPressure = new LabeledSwitch("Receive channel pressure");
                cbStudioSetPartMidiReceiveModulation = new LabeledSwitch("Receive modulation");
                cbStudioSetPartMidiReceiveVolume = new LabeledSwitch("Receive volume");
                cbStudioSetPartMidiReceivePan = new LabeledSwitch("Receive pan");
                cbStudioSetPartMidiReceiveExpression = new LabeledSwitch("Receive expression");
                cbStudioSetPartMidiReceiveHold1 = new LabeledSwitch("Receive hold-1");
                gStudioSetPartVelocityCurve = new Grid();
                imgVelocityCurve1On = new Image();
                imgVelocityCurve1Off = new Image();
                imgVelocityCurve2On = new Image();
                imgVelocityCurve2Off = new Image();
                imgVelocityCurve3On = new Image();
                imgVelocityCurve3Off = new Image();
                imgVelocityCurve4On = new Image();
                imgVelocityCurve4Off = new Image();
                btnVelocityCurve1 = new Button();
                btnVelocityCurve2 = new Button();
                btnVelocityCurve3 = new Button();
                btnVelocityCurve4 = new Button();
                StudioSetPartMotionalSurround = new Grid();
                tbStudioSetPartMotionalSurroundLR = new TextBlock();
                slStudioSetPartMotionalSurroundLR = new Slider();
                tbStudioSetPartMotionalSurroundFB = new TextBlock();
                slStudioSetPartMotionalSurroundFB = new Slider();
                tbStudioSetPartMotionalSurroundWidth = new TextBlock();
                slStudioSetPartMotionalSurroundWidth = new Slider();
                tbStudioSetPartMotionalSurroundAmbienceSendLevel = new TextBlock();
                slStudioSetPartMotionalSurroundAmbienceSendLevel = new Slider();
                StudioSetPartEQ = new Grid();
                cbStudioSetPartEQSwitch = new CheckBox();
                tbStudioSetPartEQLowFreq = new TextBlock();
                cbStudioSetPartEQLowFreq = new ComboBox();
                tbStudioSetPartEQLowGain = new TextBlock();
                slStudioSetPartEQLowGain = new Slider();
                tbStudioSetPartEQMidFreq = new TextBlock();
                cbStudioSetPartEQMidFreq = new ComboBox();
                tbStudioSetPartEQMidGain = new TextBlock();
                slStudioSetPartEQMidGain = new Slider();
                tbStudioSetPartEQMidQ = new TextBlock();
                cbStudioSetPartEQMidQ = new ComboBox();
                tbStudioSetPartEQHighFreq = new TextBlock();
                cbStudioSetPartEQHighFreq = new ComboBox();
                tbStudioSetPartEQHighGain = new TextBlock();
                slStudioSetPartEQHighGain = new Slider();
                cbStudioSetSlot = new ComboBox();
                lblStudioSetName = new TextBlock();
                tbStudioSetName = new TextBox();
                grid_Buttons = new Grid();
                btnFileSave = new Button();
                btnFileLoad = new Button();
                btnStudioSetSave = new Button();
                btnStudioSetDelete = new Button();
                btnStudioSetReturn = new Button();

                for (Int32 i = 0; i < 18; i++)
                {
                    SystemCommonSettings.RowDefinitions.Add(new RowDefinition());
                }
            }
            catch { }

            //---------------------------------------------------------------------------------------
            // Set properties 
            //---------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------
            // Column 0
            tbToneControl1.Text = "Tone control 1 source";
            tbToneControl2.Text = "Tone control 2 source";
            tbToneControl3.Text = "Tone control 3 source";
            tbToneControl4.Text = "Tone control 4 source";

            //grid_PleaseWaitWhileScanning.IsVisible = false;
            ////Progress.Name = "Progress";
            //Progress.Margin = new Thickness(10, 25, 10, 0);
            ////Progress.IsIndeterminate = true;
            //textblock_0002.Text = "Please wait while reading Studio Set names and settings from your INTEGRA-7...";
            //grid_MainStudioSet.IsVisible = true;
            cbStudioSetSelector.SelectedIndexChanged += cbStudioSetSelector_SelectionChanged;
            cbToneControl1.SelectedIndex = 0;
            cbToneControl1.SelectedIndexChanged += cbToneControl1_SelectionChanged;
            cbToneControl2.SelectedIndex = 0;
            cbToneControl2.SelectedIndexChanged += cbToneControl2_SelectionChanged;
            cbToneControl3.SelectedIndex = 0;
            cbToneControl3.SelectedIndexChanged += cbToneControl3_SelectionChanged;
            cbToneControl4.SelectedIndex = 0;
            cbToneControl4.SelectedIndexChanged += cbToneControl4_SelectionChanged;
            tbTempo.Text = "Tempo";
            slTempo.Maximum = 250;
            slTempo.Minimum = 20;
            slTempo.ValueChanged += slTempo_ValueChanged;
            tbSoloPart.Text = "Solo part";
            cbSoloPart.SelectedIndex = 0;
            cbSoloPart.SelectedIndexChanged += cbSoloPart_SelectionChanged;
            cbReverb.LSSwitch.Toggled += cbReverb_Click;
            cbChorus.LSSwitch.Toggled += cbChorus_Click;
            cbMasterEQ.LSSwitch.Toggled += cbMasterEQ_Click;
            tbDrumCompEQPart.Text = "Drum Compression/Equalizer";
            cbDrumCompEQPart.SelectedIndex = 0;
            cbDrumCompEQPart.SelectedIndexChanged += cbDrumCompEQPart_SelectionChanged;
            tbDrumCompEQ1OutputAssign.Text = "Drum Comp/EQ 1 Output assign";
            cbDrumCompEQ1OutputAssign.SelectedIndex = 0;
            cbDrumCompEQ1OutputAssign.SelectedIndexChanged += cbDrumCompEQ1OutputAssign_SelectionChanged;
            tbDrumCompEQ2OutputAssign.Text = "Drum Comp/EQ 2 Output assign";
            cbDrumCompEQ2OutputAssign.SelectedIndex = 0;
            cbDrumCompEQ2OutputAssign.SelectedIndexChanged += cbDrumCompEQ2OutputAssign_SelectionChanged;
            tbDrumCompEQ3OutputAssign.Text = "Drum Comp/EQ 3 Output assign";
            cbDrumCompEQ3OutputAssign.SelectedIndex = 0;
            cbDrumCompEQ3OutputAssign.SelectedIndexChanged += cbDrumCompEQ3OutputAssign_SelectionChanged;
            tbDrumCompEQ4OutputAssign.Text = "Drum Comp/EQ 4 Output assign";
            cbDrumCompEQ4OutputAssign.SelectedIndex = 0;
            cbDrumCompEQ4OutputAssign.SelectedIndexChanged += cbDrumCompEQ4OutputAssign_SelectionChanged;
            tbDrumCompEQ5OutputAssign.Text = "Drum Comp/EQ 5 Output assign";
            cbDrumCompEQ5OutputAssign.SelectedIndex = 0;
            cbDrumCompEQ5OutputAssign.SelectedIndexChanged += cbDrumCompEQ5OutputAssign_SelectionChanged;
            tbDrumCompEQ6OutputAssign.Text = "Drum Comp/EQ 6 Output assign";
            cbDrumCompEQ6OutputAssign.SelectedIndex = 0;
            cbDrumCompEQ6OutputAssign.SelectedIndexChanged += cbDrumCompEQ6OutputAssign_SelectionChanged;
            cbDrumCompEQ.LSSwitch.Toggled += cbDrumCompEQ_Click;
            cbExtPartMute.LSSwitch.Toggled += cbExtPartMute_Click;
            tbExtPartLevel.Text = "Ext Part Level";
            slExtPartLevel.Minimum = 0;
            slExtPartLevel.Maximum = 127;
            slExtPartLevel.ValueChanged += slExtPartLevel_ValueChanged;
            tbExtPartChorusSend.Text = "Ext part chorus level";
            slExtPartChorusSend.Minimum = 0;
            slExtPartChorusSend.Maximum = 127;
            slExtPartChorusSend.ValueChanged += slExtPartChorusSend_ValueChanged;
            tbExtPartReverbSend.Text = "Ext part reverb level";
            slExtPartReverbSend.Minimum = 0;
            slExtPartReverbSend.Maximum = 127;
            slExtPartReverbSend.ValueChanged += slExtPartReverbSend_ValueChanged;

            //---------------------------------------------------------------------------------------
            // Column 1
            cbColumn1Selector.SelectedIndexChanged += cbColumn1Selector_SelectionChanged;
            cbColumn1Selector.SelectedIndex = 0;
            tbSystemCommonMasterTune.Text = "Master tune 0 cent";
            slSystemCommonMasterTune.Minimum = -1000;
            slSystemCommonMasterTune.Maximum = 1000;
            slSystemCommonMasterTune.ValueChanged += slSystemCommonMasterTune_ValueChanged;
            tbSystemCommonMasterKeyShift.Text = "Master Key Shift";
            slSystemCommonMasterKeyShift.Minimum = -24;
            slSystemCommonMasterKeyShift.Maximum = 24;
            slSystemCommonMasterKeyShift.ValueChanged += slSystemCommonMasterKeyShift_ValueChanged;
            tbSystemCommonMasterLevel.Text = "Master level";
            slSystemCommonMasterLevel.Minimum = 0;
            slSystemCommonMasterLevel.Maximum = 127;
            slSystemCommonMasterLevel.ValueChanged += slSystemCommonMasterLevel_ValueChanged;
            cbSystemCommonScaleTuneSwitch.LSSwitch.Toggled += cbSystemCommonScaleTuneSwitch_Click;
            tbSystemCommonStudioSetControlChannel.Text = "Studio set control channel";
            cbSystemCommonStudioSetControlChannel.SelectedIndexChanged += cbSystemCommonStudioSetControlChannel_SelectionChanged;
            tbSystemCommonSystemControlSource1.Text = "System control source 1";
            cbSystemCommonSystemControlSource1.SelectedIndexChanged += cbSystemCommonSystemControlSource1_SelectedIndexChanged;
            tbSystemCommonSystemControlSource2.Text = "System control source 2";
            cbSystemCommonSystemControlSource2.SelectedIndexChanged += cbSystemCommonSystemControlSource2_SelectedIndexChanged;
            tbSystemCommonSystemControlSource3.Text = "System control source 3";
            cbSystemCommonSystemControlSource3.SelectedIndexChanged += cbSystemCommonSystemControlSource3_SelectedIndexChanged;
            tbSystemCommonSystemControlSource4.Text = "System control source 4";
            cbSystemCommonSystemControlSource4.SelectedIndexChanged += cbSystemCommonSystemControlSource4_SelectedIndexChanged;
            tbSystemCommonControlSource.Text = "Control source";
            cbSystemCommonControlSource.SelectedIndexChanged += cbSystemCommonControlSource_SelectionChanged;
            tbSystemCommonSystemClockSource.Text = "System clock source";
            cbSystemCommonSystemClockSource.SelectedIndexChanged += cbSystemCommonSystemClockSource_SelectionChanged;
            tbSystemCommonSystemTempo.Text = "System tempo";
            slSystemCommonSystemTempo.Maximum = 250;
            slSystemCommonSystemTempo.Minimum = 20;
            slSystemCommonSystemTempo.ValueChanged += slSystemCommonSystemTempo_ValueChanged;
            tbTempoAssignSource.Text = "Tempo assign source";
            cbSystemCommonTempoAssignSource.SelectedIndexChanged += cbSystemCommonTempoAssignSource_SelectionChanged;
            cbSystemCommonReceiveProgramChange.LSSwitch.Toggled += cbSystemCommonReceiveProgramChange_Click;
            cbSystemCommonReceiveBankSelect.LSSwitch.Toggled += cbSystemCommonReceiveBankSelect_Click;
            cbSystemCommonSurroundCenterSpeakerSwitch.LSSwitch.Toggled += cbSystemCommonSurroundCenterSpeakerSwitch_Click;
            cbSystemCommonSurroundSubWooferSwitch.LSSwitch.Toggled += cbSystemCommonSurroundSubWooferSwitch_Click;
            tbSystemCommonStereoOutputMode.Text = "Output mode";
            cbSystemCommonStereoOutputMode.SelectedIndexChanged += cbSystemCommonStereoOutputMode_SelectionChanged;

            tbVoiceReserve01.Text = "Voice reserve 1";
            tbVoiceReserve02.Text = "Voice reserve 2";
            tbVoiceReserve03.Text = "Voice reserve 3";
            tbVoiceReserve04.Text = "Voice reserve 4";
            tbVoiceReserve05.Text = "Voice reserve 5";
            tbVoiceReserve06.Text = "Voice reserve 6";
            tbVoiceReserve07.Text = "Voice reserve 7";
            tbVoiceReserve08.Text = "Voice reserve 8";
            tbVoiceReserve09.Text = "Voice reserve 9";
            tbVoiceReserve10.Text = "Voice reserve 10";
            tbVoiceReserve11.Text = "Voice reserve 11";
            tbVoiceReserve12.Text = "Voice reserve 12";
            tbVoiceReserve13.Text = "Voice reserve 13";
            tbVoiceReserve14.Text = "Voice reserve 14";
            tbVoiceReserve15.Text = "Voice reserve 15";
            tbVoiceReserve16.Text = "Voice reserve 16";
            slVoiceReserve01.Maximum = 64;
            slVoiceReserve01.Minimum = 0;
            slVoiceReserve01.ValueChanged += slVoiceReserve01_ValueChanged;
            slVoiceReserve02.Maximum = 64;
            slVoiceReserve02.Minimum = 0;
            slVoiceReserve02.ValueChanged += slVoiceReserve02_ValueChanged;
            slVoiceReserve03.Maximum = 64;
            slVoiceReserve03.Minimum = 0;
            slVoiceReserve03.ValueChanged += slVoiceReserve03_ValueChanged;
            slVoiceReserve04.Maximum = 64;
            slVoiceReserve04.Minimum = 0;
            slVoiceReserve04.ValueChanged += slVoiceReserve04_ValueChanged;
            slVoiceReserve05.Minimum = 0;
            slVoiceReserve05.Maximum = 64;
            slVoiceReserve05.ValueChanged += slVoiceReserve05_ValueChanged;
            slVoiceReserve06.Minimum = 0;
            slVoiceReserve06.Maximum = 64;
            slVoiceReserve06.ValueChanged += slVoiceReserve06_ValueChanged;
            slVoiceReserve07.Minimum = 0;
            slVoiceReserve07.Maximum = 64;
            slVoiceReserve07.ValueChanged += slVoiceReserve07_ValueChanged;
            slVoiceReserve08.Minimum = 0;
            slVoiceReserve08.Maximum = 64;
            slVoiceReserve08.ValueChanged += slVoiceReserve08_ValueChanged;
            slVoiceReserve09.Minimum = 0;
            slVoiceReserve09.Maximum = 64;
            slVoiceReserve09.ValueChanged += slVoiceReserve09_ValueChanged;
            slVoiceReserve10.Minimum = 0;
            slVoiceReserve10.Maximum = 64;
            slVoiceReserve10.ValueChanged += slVoiceReserve10_ValueChanged;
            slVoiceReserve11.Minimum = 0;
            slVoiceReserve11.Maximum = 64;
            slVoiceReserve11.ValueChanged += slVoiceReserve11_ValueChanged;
            slVoiceReserve12.Minimum = 0;
            slVoiceReserve12.Maximum = 64;
            slVoiceReserve12.ValueChanged += slVoiceReserve12_ValueChanged;
            slVoiceReserve13.Minimum = 0;
            slVoiceReserve13.Maximum = 64;
            slVoiceReserve13.ValueChanged += slVoiceReserve13_ValueChanged;
            slVoiceReserve14.Minimum = 0;
            slVoiceReserve14.Maximum = 64;
            slVoiceReserve14.ValueChanged += slVoiceReserve14_ValueChanged;
            slVoiceReserve15.Minimum = 0;
            slVoiceReserve15.Maximum = 64;
            slVoiceReserve15.ValueChanged += slVoiceReserve15_ValueChanged;
            slVoiceReserve16.Minimum = 0;
            slVoiceReserve16.Maximum = 64;
            slVoiceReserve16.ValueChanged += slVoiceReserve16_ValueChanged;

            //Chorus
            tbStudioSetChorusType.Text = "Type";
            cbStudioSetChorusType.SelectedIndex = 0;
            cbStudioSetChorusType.SelectedIndexChanged += cbStudioSetChorusType_SelectionChanged;
            tbChorusLevel.Text = "Level";
            slChorusLevel.Minimum = 0;
            slChorusLevel.Maximum = 127;
            slChorusLevel.ValueChanged += slChorusLevel_ValueChanged;
            tbStudioSetChorusOutputAssign.Text = "Output assign";
            cbChorusOutputAssign.SelectedIndexChanged += cbChorusOutputAssign_SelectionChanged;
            tbChorusOutputSelect.Text = "Output select";
            cbChorusOutputSelect.SelectedIndexChanged += CbChorusOutputSelect_SelectedIndexChanged;
            // Chorus chorus
            tbChorusChorusFilterType.Text = "Filter type";
            cbChorusChorusFilterType.SelectedIndexChanged += cbChorusChorusFilterType_SelectionChanged;
            tbChorusChorusFilterCutoffFrequency.Text = "Filter cutoff frequency";
            cbChorusChorusFilterCutoffFrequency.SelectedIndexChanged += cbChorusChorusFilterCutoffFrequency_SelectionChanged;
            tbChorusChorusPreDelay.Text = "Pre delay";
            slChorusChorusPreDelay.Minimum = 0;
            slChorusChorusPreDelay.Maximum = 125;
            slChorusChorusPreDelay.ValueChanged += slChorusChorusPreDelay_ValueChanged;
            tbChorusChorusRateHzNote.Text = "Rate (num/note sw)";
            cbChorusChorusRateHzNote.SelectedIndexChanged += cbChorusChorusRateHzNote_SelectionChanged;
            tbChorusChorusRateHz.Text = "Rate";
            slChorusChorusRateHz.Minimum = 0.05;
            slChorusChorusRateHz.Maximum = 10;
            slChorusChorusRateHz.StepFrequency = 0.05;
            slChorusChorusRateHz.ValueChanged += slChorusChorusRateHz_ValueChanged;
            tbChorusChorusRateNote.Text = "Rate";
            slChorusChorusRateNote.Minimum = 0;
            slChorusChorusRateNote.Maximum = 21;
            slChorusChorusRateNote.StepFrequency = 1;
            slChorusChorusRateNote.ValueChanged += slChorusChorusRateNote_ValueChanged;
            tbChorusChorusDepth.Text = "Depth";
            slChorusChorusDepth.Minimum = 0;
            slChorusChorusDepth.Maximum = 127;
            slChorusChorusDepth.ValueChanged += slChorusChorusDepth_ValueChanged;
            tbChorusChorusPhase.Text = "Phase";
            slChorusChorusPhase.Minimum = 0;
            slChorusChorusPhase.Maximum = 90; // prepared for fix below, must show as 0 - 180 but sent as 0 - 90
            slChorusChorusPhase.StepFrequency = 2;
            slChorusChorusPhase.ValueChanged += slChorusChorusPhase_ValueChanged;
            tbChorusChorusFeedback.Text = "Feedback";
            slChorusChorusFeedback.Minimum = 0;
            slChorusChorusFeedback.Maximum = 127;
            slChorusChorusFeedback.ValueChanged += slChorusChorusFeedback_ValueChanged;
            // Chorus delay
            tbChorusDelayLeftMsNote.Text = "Delay left (num/note sw)";
            cbChorusDelayLeftMsNote.SelectedIndexChanged += cbChorusDelayLeftMsNote_SelectionChanged;
            tbChorusDelayLeftHz.Text = "Delay left";
            slChorusDelayLeftHz.Minimum = 0;
            slChorusDelayLeftHz.Maximum = 1000;
            slChorusDelayLeftHz.ValueChanged += slChorusDelayLeftHz_ValueChanged;
            tbChorusDelayLeftNote.Text = "Delay left";
            slChorusDelayLeftNote.Minimum = 0;
            slChorusDelayLeftNote.Maximum = 21;
            slChorusDelayLeftNote.ValueChanged += slChorusDelayLeftNote_ValueChanged;
            tbChorusDelayRightMsNote.Text = "Delay right (num/note sw)";
            cbChorusDelayRightMsNote.SelectedIndexChanged += cbChorusDelayRightMsNote_SelectionChanged;
            tbChorusDelayRightHz.Text = "Delay right";
            slChorusDelayRightHz.Minimum = 0;
            slChorusDelayRightHz.Maximum = 1000;
            slChorusDelayRightHz.ValueChanged += slChorusDelayRightHz_ValueChanged;
            tbChorusDelayRightNote.Text = "Delay right";
            slChorusDelayRightNote.Minimum = 0;
            slChorusDelayRightNote.Maximum = 21;
            slChorusDelayRightNote.ValueChanged += slChorusDelayRightNote_ValueChanged;
            tbChorusDelayCenterMsNote.Text = "Delay center (num/note sw)";
            cbChorusDelayCenterMsNote.SelectedIndexChanged += cbChorusDelayCenterMsNote_SelectionChanged;
            tbChorusDelayCenterHz.Text = "Delay center";
            slChorusDelayCenterHz.Minimum = 0;
            slChorusDelayCenterHz.Maximum = 1000;
            slChorusDelayCenterHz.ValueChanged += slChorusDelayCenterHz_ValueChanged;
            tbChorusDelayCenterNote.Text = "Delay center";
            slChorusDelayCenterNote.Minimum = 0;
            slChorusDelayCenterNote.Maximum = 21;
            slChorusDelayCenterNote.ValueChanged += slChorusDelayCenterNote_ValueChanged;
            tbChorusDelayCenterFeedback.Text = "Center feedback";
            slChorusDelayCenterFeedback.Minimum = -98; // change to 49, send -49 to 49 but display -98 to 98
            slChorusDelayCenterFeedback.Maximum = 98;
            slChorusDelayCenterFeedback.StepFrequency = 2;
            slChorusDelayCenterFeedback.ValueChanged += slChorusDelayCenterFeedback_ValueChanged;
            tbChorusDelayHFDamp.Text = "HF damp";
            cbChorusDelayHFDamp.SelectedIndexChanged += cbChorusDelayHFDamp_SelectionChanged;
            tbChorusDelayLeftLevel.Text = "Left level";
            slChorusDelayLeftLevel.Minimum = 0;
            slChorusDelayLeftLevel.Maximum = 127;
            slChorusDelayLeftLevel.ValueChanged += slChorusDelayLeftLevel_ValueChanged;
            tbChorusDelayRightLevel.Text = "Right level";
            slChorusDelayRightLevel.Minimum = 0;
            slChorusDelayRightLevel.Maximum = 127;
            slChorusDelayRightLevel.ValueChanged += slChorusDelayRightLevel_ValueChanged;
            tbChorusDelayCenterLevel.Text = "Center level";
            slChorusDelayCenterLevel.Minimum = 0;
            slChorusDelayCenterLevel.Maximum = 127;
            slChorusDelayCenterLevel.ValueChanged += slChorusDelayCenterLevel_ValueChanged;
            // Chorus GM2
            tbChorusGM2ChorusPreLPF.Text = "Pre-LPF";
            slChorusGM2ChorusPreLPF.Minimum = 0;
            slChorusGM2ChorusPreLPF.Maximum = 7;
            slChorusGM2ChorusPreLPF.ValueChanged += slChorusGM2ChorusPreLPF_ValueChanged;
            tbChorusGM2ChorusLevel.Text = "Level";
            slChorusGM2ChorusLevel.Minimum = 0;
            slChorusGM2ChorusLevel.Maximum = 127;
            slChorusGM2ChorusLevel.ValueChanged += slChorusGM2ChorusLevel_ValueChanged;
            tbChorusGM2ChorusFeedback.Text = "Feedback";
            slChorusGM2ChorusFeedback.Minimum = 0;
            slChorusGM2ChorusFeedback.Maximum = 127;
            slChorusGM2ChorusFeedback.ValueChanged += slChorusGM2ChorusFeedback_ValueChanged;
            tbChorusGM2ChorusDelay.Text = "Delay";
            slChorusGM2ChorusDelay.Minimum = 0;
            slChorusGM2ChorusDelay.Maximum = 127;
            slChorusGM2ChorusDelay.ValueChanged += slChorusGM2ChorusDelay_ValueChanged;
            tbChorusGM2ChorusRate.Text = "Rate";
            slChorusGM2ChorusRate.Minimum = 0;
            slChorusGM2ChorusRate.Maximum = 127;
            slChorusGM2ChorusRate.ValueChanged += slChorusGM2ChorusRate_ValueChanged;
            tbChorusGM2ChorusDepth.Text = "Depth";
            slChorusGM2ChorusDepth.Minimum = 0;
            slChorusGM2ChorusDepth.Maximum = 127;
            slChorusGM2ChorusDepth.ValueChanged += slChorusGM2ChorusDepth_ValueChanged;
            tbChorusGM2ChorusSendLevelToReverb.Text = "Send lvl to rev.";
            slChorusGM2ChorusSendLevelToReverb.Minimum = 0;
            slChorusGM2ChorusSendLevelToReverb.Maximum = 127;
            slChorusGM2ChorusSendLevelToReverb.ValueChanged += slChorusGM2ChorusSendLevelToReverb_ValueChanged;
            //ChorusReverb.IsVisible = false; Here? Why?

            // Reverb
            tbStudioSetReverbType.Text = "Type";
            cbStudioSetReverbType.SelectedIndexChanged += cbStudioSetReverbType_SelectionChanged;
            tbStudioSetReverbLevel.Text = "Level";
            slStudioSetReverbLevel.Minimum = 0;
            slStudioSetReverbLevel.Maximum = 127;
            slStudioSetReverbLevel.ValueChanged += slReverbLevel_ValueChanged;
            tbStudioSetReverbOutputAssign.Text = "Output assign";
            cbStudioSetReverbOutputAssign.SelectedIndexChanged += cbStudioSetReverbOutputAssign_SelectionChanged;
            tbStudioSetReverbPreDelay.Text = "Pre delay";
            slStudioSetReverbPreDelay.Minimum = 0;
            slStudioSetReverbPreDelay.Maximum = 100;
            slStudioSetReverbPreDelay.ValueChanged += slStudioSetReverbPreDelay_ValueChanged;
            tbStudioSetReverbTime.Text = "Time";
            slStudioSetReverbTime.Maximum = 100;
            slStudioSetReverbTime.Minimum = 1;
            slStudioSetReverbTime.ValueChanged += slStudioSetReverbTime_ValueChanged;
            tbStudioSetReverbDensity.Text = "Density";
            slStudioSetReverbDensity.Minimum = 0;
            slStudioSetReverbDensity.Maximum = 127;
            slStudioSetReverbDensity.ValueChanged += slStudioSetReverbDensity_ValueChanged;
            tbStudioSetReverbDiffusion.Text = "Diffusion";
            slStudioSetReverbDiffusion.Minimum = 0;
            slStudioSetReverbDiffusion.Maximum = 127;
            slStudioSetReverbDiffusion.ValueChanged += slStudioSetReverbDiffusion_ValueChanged;
            tbStudioSetReverbLFDamp.Text = "LF damp";
            slStudioSetReverbLFDamp.Minimum = 0;
            slStudioSetReverbLFDamp.Maximum = 100;
            slStudioSetReverbLFDamp.ValueChanged += slStudioSetReverbLFDamp_ValueChanged;
            tbStudioSetReverbHFDamp.Text = "HF damp";
            slStudioSetReverbHFDamp.Minimum = 0;
            slStudioSetReverbHFDamp.Maximum = 100;
            slStudioSetReverbHFDamp.ValueChanged += slStudioSetReverbHFDamp_ValueChanged;
            tbStudioSetReverbSpread.Text = "Spread";
            slStudioSetReverbSpread.Minimum = 0;
            slStudioSetReverbSpread.Maximum = 127;
            slStudioSetReverbSpread.ValueChanged += slStudioSetReverbSpread_ValueChanged;
            tbStudioSetReverbTone.Text = "Tone";
            slStudioSetReverbTone.Minimum = 0;
            slStudioSetReverbTone.Maximum = 127;
            slStudioSetReverbTone.ValueChanged += slStudioSetReverbTone_ValueChanged;
            tbStudioSetReverbGM2Character.Text = "Character";
            slStudioSetReverbGM2Character.Minimum = 0;
            slStudioSetReverbGM2Character.Maximum = 5;
            slStudioSetReverbGM2Character.ValueChanged += slStudioSetReverbGM2Character_ValueChanged;
            tbStudioSetReverbGM2Time.Text = "Time";
            slStudioSetReverbGM2Time.Minimum = 0;
            slStudioSetReverbGM2Time.Maximum = 127;
            slStudioSetReverbGM2Time.ValueChanged += slStudioSetReverbGM2Time_ValueChanged;

            // Motional surround
            cbStudioSetMotionalSurround.Content = "Motional surround";
            tbStudioSetMotionalSurroundCommon.Text = "Common";
            tbStudioSetMotionalSurroundExt.Text = "External part (See right column for part 1-16)";
            tbStudioSetMotionalSurroundControl.Text = "----------------------";
            cbStudioSetMotionalSurround.CBSwitch.Toggled += cbStudioSetMotionalSurround_Click;
            tbStudioSetMotionalSurroundRoomType.Text = "Room type";
            cbStudioSetMotionalSurroundRoomType.SelectedIndexChanged += cbStudioSetMotionalSurroundRoomType_SelectionChanged;
            tbStudioSetMotionalSurroundAmbienceLevel.Text = "Ambience level";
            slStudioSetMotionalSurroundAmbienceLevel.Minimum = 0;
            slStudioSetMotionalSurroundAmbienceLevel.Maximum = 127;
            slStudioSetMotionalSurroundAmbienceLevel.ValueChanged += slStudioSetMotionalSurroundAmbienceLevel_ValueChanged;
            tbStudioSetMotionalSurroundRoomSize.Text = "Room size";
            cbStudioSetMotionalSurroundRoomSize.SelectedIndexChanged += cbStudioSetMotionalSurroundRoomSize_SelectionChanged;
            tbStudioSetMotionalSurroundAmbienceTime.Text = "Ambience time";
            slStudioSetMotionalSurroundAmbienceTime.Minimum = 0;
            slStudioSetMotionalSurroundAmbienceTime.Maximum = 100;
            slStudioSetMotionalSurroundAmbienceTime.ValueChanged += slStudioSetMotionalSurroundAmbienceTime_ValueChanged;
            tbStudioSetMotionalSurroundAmbienceDensity.Text = "Ambience density";
            slStudioSetMotionalSurroundAmbienceDensity.Minimum = 0;
            slStudioSetMotionalSurroundAmbienceDensity.Maximum = 100;
            slStudioSetMotionalSurroundAmbienceDensity.ValueChanged += slStudioSetMotionalSurroundAmbienceDensity_ValueChanged;
            tbStudioSetMotionalSurroundAmbienceHFDamp.Text = "Ambience HF damp";
            slStudioSetMotionalSurroundAmbienceHFDamp.Minimum = 0;
            slStudioSetMotionalSurroundAmbienceHFDamp.Maximum = 100;
            slStudioSetMotionalSurroundAmbienceHFDamp.ValueChanged += slStudioSetMotionalSurroundAmbienceHFDamp_ValueChanged;
            tbStudioSetMotionalSurroundExternalPartLR.Text = "External part L-R";
            slStudioSetMotionalSurroundExternalPartLR.Minimum = -64;
            slStudioSetMotionalSurroundExternalPartLR.Maximum = 63;
            slStudioSetMotionalSurroundExternalPartLR.ValueChanged += slStudioSetMotionalSurroundExternalPartLR_ValueChanged;
            tbStudioSetMotionalSurroundExternalPartFB.Text = "External part F-B";
            slStudioSetMotionalSurroundExternalPartFB.Minimum = -64;
            slStudioSetMotionalSurroundExternalPartFB.Maximum = 63;
            slStudioSetMotionalSurroundExternalPartFB.ValueChanged += slStudioSetMotionalSurroundExternalPartFB_ValueChanged;
            tbStudioSetMotionalSurroundExtPartWidth.Text = "Ext part width";
            slStudioSetMotionalSurroundExtPartWidth.Minimum = 0;
            slStudioSetMotionalSurroundExtPartWidth.Maximum = 32;
            slStudioSetMotionalSurroundExtPartWidth.ValueChanged += slStudioSetMotionalSurroundExtPartWidth_ValueChanged;
            tbStudioSetMotionalSurroundExtpartAmbienceSend.Text = "Ext part ambience send";
            slStudioSetMotionalSurroundExtpartAmbienceSend.Minimum = 0;
            slStudioSetMotionalSurroundExtpartAmbienceSend.Maximum = 127;
            slStudioSetMotionalSurroundExtpartAmbienceSend.ValueChanged += slStudioSetMotionalSurroundExtpartAmbienceSend_ValueChanged;
            tbStudioSetMotionalSurroundExtPartControl.Text = "Ext part control";
            cbStudioSetMotionalSurroundExtPartControl.SelectedIndexChanged += cbStudioSetMotionalSurroundExtPartControl_SelectionChanged;
            tbStudioSetMotionalSurroundDepth.Text = "Motional surround depth";
            slStudioSetMotionalSurroundDepth.Minimum = 0;
            slStudioSetMotionalSurroundDepth.Maximum = 100;
            slStudioSetMotionalSurroundDepth.ValueChanged += slStudioSetMotionalSurroundDepth_ValueChanged;

            // Master EQ
            tbStudioSetMasterEqLowFreq.Text = "EQ low freq";
            cbStudioSetMasterEqLowFreq.SelectedIndexChanged += cbStudioSetMasterEqLowFreq_SelectionChanged;
            slStudioSetMasterEqLowGain.Minimum = -15;
            slStudioSetMasterEqLowGain.Maximum = 15;
            slStudioSetMasterEqLowGain.ValueChanged += slStudioSetMasterEqLowGain_ValueChanged;
            tbStudioSetMasterEqMidFreq.Text = "EQ mid freq";
            cbStudioSetMasterEqMidFreq.SelectedIndexChanged += cbStudioSetMasterEqMidFreq_SelectionChanged;
            tbStudioSetMasterEqMidGain.Text = "Parameter 2";
            slStudioSetMasterEqMidGain.Minimum = -15;
            slStudioSetMasterEqMidGain.Maximum = 15;
            slStudioSetMasterEqMidGain.ValueChanged += slStudioSetMasterEqMidGain_ValueChanged;
            tbStudioSetMasterEqMidQ.Text = "EQ mid Q";
            cbStudioSetMasterEqMidQ.SelectedIndexChanged += cbStudioSetMasterEqMidQ_SelectionChanged;
            tbStudioSetMasterEqHighFreq.Text = "EQ high freq";
            cbStudioSetMasterEqHighFreq.SelectedIndexChanged += cbStudioSetMasterEqHighFreq_SelectionChanged;
            tbStudioSetMasterEqHighGain.Text = "Parameter 2";
            slStudioSetMasterEqHighGain.Minimum = -15;
            slStudioSetMasterEqHighGain.Maximum = 15;
            slStudioSetMasterEqHighGain.ValueChanged += slStudioSetMasterEqHighGain_ValueChanged;
            lvSearchResults.ItemSelected += lvSearchResults_SelectionChanged;

            //---------------------------------------------------------------------------------------
            // Column 2

            // Part settings 1
            //cbStudioSetPartSelector.SelectedIndex = 0;
            cbStudioSetPartSelector.SelectedIndexChanged += cbStudioSetPartSelector_SelectionChanged;
            //cbStudioSetPartSubSelector.SelectedIndex = 0;
            cbStudioSetPartSubSelector.SelectedIndexChanged += cbStudioSetPartSubSelector_SelectionChanged;
            cbStudioSetPartSettings1Receive.LSSwitch.Toggled += cbStudioSetPartSettings1Receive_Click;
            cbStudioSetPartSettings1ReceiveChannel.SelectedIndex = 0;
            cbStudioSetPartSettings1ReceiveChannel.SelectedIndexChanged += cbStudioSetPartSettings1ReceiveChannel_SelectionChanged;
            tbStudioSetPartSettings1Group.Text = "Group ";
            cbStudioSetPartSettings1Group.SelectedIndexChanged += cbStudioSetPartSettings1Group_ValueChanged;
            tbStudioSetPartSettings1Category.Text = "Category ";
            cbStudioSetPartSettings1Category.SelectedIndexChanged += cbStudioSetPartSettings1Category_ValueChanged;
            tbStudioSetPartSettings1Program.Text = "Tone";
            cbStudioSetPartSettings1Program.SelectedIndexChanged += cbStudioSetPartSettings1Program_SelectionChanged;
            tbStudioSetPartSettings1Search.Text = "Search";
            cbStudioSetPartSettings1Search.TextChanged += cbStudioSetPartSettings1Search_TextChanged;
            tbStudioSetPartSettings1Level.Text = "Part level";
            slStudioSetPartSettings1Level.Minimum = 0;
            slStudioSetPartSettings1Level.Maximum = 127;
            slStudioSetPartSettings1Level.ValueChanged += slStudioSetPartSettings1Level_ValueChanged;
            tbStudioSetPartSettings1Pan.Text = "Part pan";
            slStudioSetPartSettings1Pan.Minimum = -64;
            slStudioSetPartSettings1Pan.Maximum = 63;
            slStudioSetPartSettings1Pan.ValueChanged += slStudioSetPartSettings1Pan_ValueChanged;
            tbStudioSetPartSettings1CoarseTune.Text = "Coarse tune";
            slStudioSetPartSettings1CoarseTune.Minimum = -48;
            slStudioSetPartSettings1CoarseTune.Maximum = 48;
            slStudioSetPartSettings1CoarseTune.ValueChanged += slStudioSetPartSettings1CoarseTune_ValueChanged;
            tbStudioSetPartSettings1FineTune.Text = "Fine tune";
            slStudioSetPartSettings1FineTune.Minimum = -50;
            slStudioSetPartSettings1FineTune.Maximum = 50;
            slStudioSetPartSettings1FineTune.ValueChanged += slStudioSetPartSettings1FineTune_ValueChanged;
            tbStudioSetPartSettings1MonoPoly.Text = "Mono/Poly";
            cbStudioSetPartSettings1MonoPoly.SelectedIndex = 0;
            cbStudioSetPartSettings1MonoPoly.SelectedIndexChanged += cbStudioSetPartSettings1Poly_SelectionChanged;
            tbStudioSetPartSettings1Legato.Text = "Legato";
            cbStudioSetPartSettings1Legato.SelectedIndex = 0;
            cbStudioSetPartSettings1Legato.SelectedIndexChanged += cbStudioSetPartSettings1Legato_SelectionChanged;
            tbStudioSetPartSettings1PitchBendRange.Text = "Bend range";
            slStudioSetPartSettings1PitchBendRange.Minimum = 0;
            slStudioSetPartSettings1PitchBendRange.Maximum = 25;
            slStudioSetPartSettings1PitchBendRange.ValueChanged += slStudioSetPartSettings1BendRange_ValueChanged;
            tbStudioSetPartSettings1Portamento.Text = "Portamento";
            cbStudioSetPartSettings1Portamento.SelectedIndex = 0;
            cbStudioSetPartSettings1Portamento.SelectedIndexChanged += cbStudioSetPartSettings1Portamento_SelectionChanged;
            tbStudioSetPartSettings1PortamentoTime.Text = "Portamento time";
            slStudioSetPartSettings1PortamentoTime.Minimum = 0;
            slStudioSetPartSettings1PortamentoTime.Maximum = 128;
            slStudioSetPartSettings1PortamentoTime.ValueChanged += slStudioSetPartSettings1PortamentoTime_ValueChanged;

            // Part settings 2
            tbStudioSetPartSettings2CutoffOffset.Text = "Cutoff offset";
            slStudioSetPartSettings2CutoffOffset.Minimum = -64;
            slStudioSetPartSettings2CutoffOffset.Maximum = 63;
            slStudioSetPartSettings2CutoffOffset.ValueChanged += slStudioSetPartSettings2CutoffOffset_ValueChanged;
            tbStudioSetPartSettings2ResonanceOffset.Text = "Resonance offset";
            slStudioSetPartSettings2ResonanceOffset.Minimum = -64;
            slStudioSetPartSettings2ResonanceOffset.Maximum = 63;
            slStudioSetPartSettings2ResonanceOffset.ValueChanged += slStudioSetPartSettings2ResonanceOffset_ValueChanged;
            tbStudioSetPartSettings2AttackTimeOffset.Text = "Attack time offset";
            slStudioSetPartSettings2AttackTimeOffset.Minimum = -64;
            slStudioSetPartSettings2AttackTimeOffset.Maximum = 63;
            slStudioSetPartSettings2AttackTimeOffset.ValueChanged += slStudioSetPartSettings2AttackTimeOffset_ValueChanged;
            tbStudioSetPartSettings2DecayTimeOffset.Text = "Decay time offset";
            slStudioSetPartSettings2DecayTimeOffset.Minimum = -64;
            slStudioSetPartSettings2DecayTimeOffset.Maximum = 63;
            slStudioSetPartSettings2DecayTimeOffset.ValueChanged += slStudioSetPartSettings2DecayTimeOffset_ValueChanged;
            tbStudioSetPartSettings2ReleaseTimeOffset.Text = "Release time offset";
            slStudioSetPartSettings2ReleaseTimeOffset.Minimum = -64;
            slStudioSetPartSettings2ReleaseTimeOffset.Maximum = 63;
            slStudioSetPartSettings2ReleaseTimeOffset.ValueChanged += slStudioSetPartSettings2ReleaseTimeOffset_ValueChanged;
            tbStudioSetPartSettings2VibratoRate.Text = "Vibrato rate";
            slStudioSetPartSettings2VibratoRate.Minimum = -64;
            slStudioSetPartSettings2VibratoRate.Maximum = 63;
            slStudioSetPartSettings2VibratoRate.ValueChanged += slStudioSetPartSettings2VibratoRate_ValueChanged;
            tbStudioSetPartSettings2VibratoDepth.Text = "Vibrato depth";
            slStudioSetPartSettings2VibratoDepth.Minimum = -64;
            slStudioSetPartSettings2VibratoDepth.Maximum = 63;
            slStudioSetPartSettings2VibratoDepth.ValueChanged += slStudioSetPartSettings2VibratoDepth_ValueChanged;
            tbStudioSetPartSettings2VibratoDelay.Text = "Vibrato delay";
            slStudioSetPartSettings2VibratoDelay.Minimum = -64;
            slStudioSetPartSettings2VibratoDelay.Maximum = 63;
            slStudioSetPartSettings2VibratoDelay.ValueChanged += slStudioSetPartSettings2VibratoDelay_ValueChanged;

            // Chorus settings
            tbStudioSetPartEffectsChorusSendLevel.Text = "Chorus send level";
            slStudioSetPartEffectsChorusSendLevel.Minimum = 0;
            slStudioSetPartEffectsChorusSendLevel.Maximum = 127;
            slStudioSetPartEffectsChorusSendLevel.ValueChanged += slStudioSetPartEffectsChorusSendLevel_ValueChanged;
            tbStudioSetPartEffectsReverbSendLevel.Text = "Reverb send level";
            slStudioSetPartEffectsReverbSendLevel.Minimum = 0;
            slStudioSetPartEffectsReverbSendLevel.Maximum = 127;
            slStudioSetPartEffectsReverbSendLevel.ValueChanged += slStudioSetPartEffectsReverbSendLevel_ValueChanged;
            tbStudioSetPartEffectsOutputAssign.Text = "Output assign";
            cbStudioSetPartEffectsOutputAssign.SelectedIndex = 0;
            cbStudioSetPartEffectsOutputAssign.SelectedIndexChanged += cbStudioSetPartEffectsOutputAssign_SelectionChanged;

            // Keyboard settings
            tbStudioSetPartKeyboardOctaveShift.Text = "Octave shift";
            slStudioSetPartKeyboardOctaveShift.Minimum = -3;
            slStudioSetPartKeyboardOctaveShift.Maximum = 3;
            slStudioSetPartKeyboardOctaveShift.ValueChanged += slStudioSetPartKeyboardOctaveShift_ValueChanged;
            tbStudioSetPartKeyboardVelocitySenseOffset.Text = "Velocity sense offset";
            slStudioSetPartKeyboardVelocitySenseOffset.Minimum = -63;
            slStudioSetPartKeyboardVelocitySenseOffset.Maximum = 63;
            slStudioSetPartKeyboardVelocitySenseOffset.ValueChanged += slStudioSetPartKeyboardVelocitySenseOffset_ValueChanged;
            tbStudioSetPartKeyboardRangeLower.Text = "Range lower";
            slStudioSetPartKeyboardRangeLower.Minimum = 0;
            slStudioSetPartKeyboardRangeLower.Maximum = 127;
            slStudioSetPartKeyboardRangeLower.ValueChanged += slStudioSetPartKeyboardRangeLower_ValueChanged;
            tbStudioSetPartKeyboardRangeUpper.Text = "Range upper";
            slStudioSetPartKeyboardRangeUpper.Minimum = 0;
            slStudioSetPartKeyboardRangeUpper.Maximum = 127;
            slStudioSetPartKeyboardRangeUpper.ValueChanged += slStudioSetPartKeyboardRangeUpper_ValueChanged;
            tbStudioSetPartKeyboardFadeWidthLower.Text = "Fade width lower";
            slStudioSetPartKeyboardFadeWidthLower.Minimum = 0;
            slStudioSetPartKeyboardFadeWidthLower.Maximum = 127;
            slStudioSetPartKeyboardFadeWidthLower.ValueChanged += slStudioSetPartKeyboardFadeWidthLower_ValueChanged;
            tbStudioSetPartKeyboardFadeWidthUpper.Text = "Fade width upper";
            slStudioSetPartKeyboardFadeWidthUpper.Minimum = 0;
            slStudioSetPartKeyboardFadeWidthUpper.Maximum = 127;
            slStudioSetPartKeyboardFadeWidthUpper.ValueChanged += slStudioSetPartKeyboardFadeWidthUpper_ValueChanged;
            tbStudioSetPartKeyboardVelocityRangeLower.Text = "Velocity range lower";
            slStudioSetPartKeyboardVelocityRangeLower.Maximum = 127;
            slStudioSetPartKeyboardVelocityRangeLower.Minimum = 1;
            slStudioSetPartKeyboardVelocityRangeLower.ValueChanged += slStudioSetPartKeyboardVelocityRangeLower_ValueChanged;
            tbStudioSetPartKeyboardVelocityRangeUpper.Text = "Velocity range upper";
            slStudioSetPartKeyboardVelocityRangeUpper.Maximum = 127;
            slStudioSetPartKeyboardVelocityRangeUpper.Minimum = 1;
            slStudioSetPartKeyboardVelocityRangeUpper.ValueChanged += slStudioSetPartKeyboardVelocityRangeUpper_ValueChanged;
            tbStudioSetPartKeyboardVelocityFadeWidthLower.Text = "Velocity fade width lower";
            slStudioSetPartKeyboardVelocityFadeWidthLower.Minimum = 0;
            slStudioSetPartKeyboardVelocityFadeWidthLower.Maximum = 127;
            slStudioSetPartKeyboardVelocityFadeWidthLower.ValueChanged += slStudioSetPartKeyboardVelocityFadeWidthLower_ValueChanged;
            tbStudioSetPartKeyboardVelocityFadeWidthUpper.Text = "Velocity fade width upper";
            slStudioSetPartKeyboardVelocityFadeWidthUpper.Minimum = 0;
            slStudioSetPartKeyboardVelocityFadeWidthUpper.Maximum = 127;
            slStudioSetPartKeyboardVelocityFadeWidthUpper.ValueChanged += slStudioSetPartKeyboardVelocityFadeWidthUpper_ValueChanged;
            cbStudioSetPartKeyboardMute.Content = "Mute";
            cbStudioSetPartKeyboardMute.CBSwitch.Toggled += cbStudioSetPartKeyboard_Click;
            tbStudioSetPartKeyboardVelocityCurveType.Text = "Velocity Curve Type";
            cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = 0;
            cbStudioSetPartKeyboardVelocityCurveType.SelectedIndexChanged += cbStudioSetPartKeyboardVelocityCurveType_SelectionChanged;

            // Part scale tune settings
            tbStudioSetPartScaleTuneType.Text = "Type";
            cbStudioSetPartScaleTuneType.SelectedIndex = 0;
            cbStudioSetPartScaleTuneType.SelectedIndexChanged += cbStudioSetPartScaleTuneType_SelectionChanged;
            tbStudioSetPartScaleTuneKey.Text = "Key";
            cbStudioSetPartScaleTuneKey.SelectedIndex = 0;
            cbStudioSetPartScaleTuneKey.SelectedIndexChanged += cbStudioSetPartScaleTune_SelectionChanged;
            tbStudioSetPartScaleTuneC.Text = "Tune for C";
            slStudioSetPartScaleTuneC.Minimum = -64;
            slStudioSetPartScaleTuneC.Maximum = 63;
            slStudioSetPartScaleTuneC.ValueChanged += slStudioSetPartScaleTuneC_ValueChanged;
            tbStudioSetPartScaleTuneCi.Text = "Tune for C#";
            slStudioSetPartScaleTuneCi.Minimum = -64;
            slStudioSetPartScaleTuneCi.Maximum = 63;
            slStudioSetPartScaleTuneCi.ValueChanged += slStudioSetPartScaleTuneCi_ValueChanged;
            tbStudioSetPartScaleTuneD.Text = "Tune for D";
            slStudioSetPartScaleTuneD.Minimum = -64;
            slStudioSetPartScaleTuneD.Maximum = 63;
            slStudioSetPartScaleTuneD.ValueChanged += slStudioSetPartScaleTuneD_ValueChanged;
            tbStudioSetPartScaleTuneDi.Text = "Tune for D#";
            slStudioSetPartScaleTuneDi.Minimum = -64;
            slStudioSetPartScaleTuneDi.Maximum = 63;
            slStudioSetPartScaleTuneDi.ValueChanged += slStudioSetPartScaleTuneDi_ValueChanged;
            tbStudioSetPartScaleTuneE.Text = "Tune for E";
            slStudioSetPartScaleTuneE.Minimum = -64;
            slStudioSetPartScaleTuneE.Maximum = 63;
            slStudioSetPartScaleTuneE.ValueChanged += slStudioSetPartScaleTuneE_ValueChanged;
            tbStudioSetPartScaleTuneF.Text = "Tune for F";
            slStudioSetPartScaleTuneF.Minimum = -64;
            slStudioSetPartScaleTuneF.Maximum = 63;
            slStudioSetPartScaleTuneF.ValueChanged += slStudioSetPartScaleTuneF_ValueChanged;
            tbStudioSetPartScaleTuneFi.Text = "Tune for F#";
            slStudioSetPartScaleTuneFi.Minimum = -64;
            slStudioSetPartScaleTuneFi.Maximum = 63;
            slStudioSetPartScaleTuneFi.ValueChanged += slStudioSetPartScaleTuneFi_ValueChanged;
            tbStudioSetPartScaleTuneG.Text = "Tune for G";
            slStudioSetPartScaleTuneG.Minimum = -64;
            slStudioSetPartScaleTuneG.Maximum = 63;
            slStudioSetPartScaleTuneG.ValueChanged += slStudioSetPartScaleTuneG_ValueChanged;
            tbStudioSetPartScaleTuneGi.Text = "Tune for G#";
            slStudioSetPartScaleTuneGi.Minimum = -64;
            slStudioSetPartScaleTuneGi.Maximum = 63;
            slStudioSetPartScaleTuneGi.ValueChanged += slStudioSetPartScaleTuneGi_ValueChanged;
            tbStudioSetPartScaleTuneA.Text = "Tune for A";
            slStudioSetPartScaleTuneA.Minimum = -64;
            slStudioSetPartScaleTuneA.Maximum = 63;
            slStudioSetPartScaleTuneA.ValueChanged += slStudioSetPartScaleTuneA_ValueChanged;
            tbStudioSetPartScaleTuneAi.Text = "Tune for A#";
            slStudioSetPartScaleTuneAi.Minimum = -64;
            slStudioSetPartScaleTuneAi.Maximum = 63;
            slStudioSetPartScaleTuneAi.ValueChanged += slStudioSetPartScaleTuneAi_ValueChanged;
            tbStudioSetPartScaleTuneB.Text = "Tune for B";
            slStudioSetPartScaleTuneB.Minimum = -64;
            slStudioSetPartScaleTuneB.Maximum = 63;
            slStudioSetPartScaleTuneB.ValueChanged += slStudioSetPartScaleTuneB_ValueChanged;

            // Part MIDI settings
            cbStudioSetPartMidiPhaseLock.LSSwitch.Toggled += cbStudioSetPartMidiPhaseLock_Click;
            cbStudioSetPartMidiReceiveProgramChange.LSSwitch.Toggled += cbStudioSetPartMidiReceiveProgramChange_Click;
            cbStudioSetPartMidiReceiveBankSelect.LSSwitch.Toggled += cbStudioSetPartMidiReceiveBankSelect_Click;
            cbStudioSetPartMidiReceivePitchBend.LSSwitch.Toggled += cbStudioSetPartMidiReceivePitchBend_Click;
            cbStudioSetPartMidiReceivePolyphonicKeyPressure.LSSwitch.Toggled += cbStudioSetPartMidiReceivePolyphonicKeyPressure_Click;
            cbStudioSetPartMidiReceiveChannelPressure.LSSwitch.Toggled += cbStudioSetPartMidiReceiveChannelPressure_Click;
            cbStudioSetPartMidiReceiveModulation.LSSwitch.Toggled += cbStudioSetPartMidiReceiveModulation_Click;
            cbStudioSetPartMidiReceiveVolume.LSSwitch.Toggled += cbStudioSetPartMidiReceiveVolume_Click;
            cbStudioSetPartMidiReceivePan.LSSwitch.Toggled += cbStudioSetPartMidiReceivePan_Click;
            cbStudioSetPartMidiReceiveExpression.LSSwitch.Toggled += cbStudioSetPartMidiReceiveExpression_Click;
            cbStudioSetPartMidiReceiveHold1.LSSwitch.Toggled += cbStudioSetPartMidiReceiveHold1_Click;

            gStudioSetPartVelocityCurve.ColumnDefinitions.Add(new ColumnDefinition());
            gStudioSetPartVelocityCurve.ColumnDefinitions.Add(new ColumnDefinition());
            gStudioSetPartVelocityCurve.ColumnDefinitions.Add(new ColumnDefinition());
            gStudioSetPartVelocityCurve.ColumnDefinitions.Add(new ColumnDefinition());
            Grid.SetColumn(imgVelocityCurve1On, 0);
            Grid.SetRow(imgVelocityCurve1Off, 0);
            Grid.SetColumn(imgVelocityCurve1Off, 0);
            Grid.SetRow(imgVelocityCurve2On, 0);
            Grid.SetColumn(imgVelocityCurve2On, 1);
            Grid.SetRow(imgVelocityCurve2Off, 0);
            Grid.SetColumn(imgVelocityCurve2Off, 1);
            Grid.SetRow(imgVelocityCurve3On, 0);
            Grid.SetColumn(imgVelocityCurve3On, 2);
            Grid.SetRow(imgVelocityCurve3Off, 0);
            Grid.SetColumn(imgVelocityCurve3Off, 2);
            Grid.SetRow(imgVelocityCurve4On, 0);
            Grid.SetColumn(imgVelocityCurve4On, 3);
            Grid.SetRow(imgVelocityCurve4Off, 0);
            Grid.SetColumn(imgVelocityCurve4Off, 3);
            Grid.SetRow(btnVelocityCurve1, 0);
            Grid.SetColumn(btnVelocityCurve1, 0);
            Grid.SetRow(btnVelocityCurve2, 0);
            Grid.SetColumn(btnVelocityCurve2, 1);
            Grid.SetRow(btnVelocityCurve3, 0);
            Grid.SetColumn(btnVelocityCurve3, 2);
            Grid.SetRow(btnVelocityCurve4, 0);
            Grid.SetColumn(btnVelocityCurve4, 3);

            imgVelocityCurve1On.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve1Off.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve2On.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve2Off.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve3On.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve3Off.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve4On.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve4Off.HorizontalOptions = LayoutOptions.FillAndExpand;
            btnVelocityCurve1.HorizontalOptions = LayoutOptions.FillAndExpand;
            btnVelocityCurve2.HorizontalOptions = LayoutOptions.FillAndExpand;
            btnVelocityCurve3.HorizontalOptions = LayoutOptions.FillAndExpand;
            btnVelocityCurve4.HorizontalOptions = LayoutOptions.FillAndExpand;

            imgVelocityCurve1On.VerticalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve1Off.VerticalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve2On.VerticalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve2Off.VerticalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve3On.VerticalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve3Off.VerticalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve4On.VerticalOptions = LayoutOptions.FillAndExpand;
            imgVelocityCurve4Off.VerticalOptions = LayoutOptions.FillAndExpand;
            btnVelocityCurve1.VerticalOptions = LayoutOptions.FillAndExpand;
            btnVelocityCurve2.VerticalOptions = LayoutOptions.FillAndExpand;
            btnVelocityCurve3.VerticalOptions = LayoutOptions.FillAndExpand;
            btnVelocityCurve4.VerticalOptions = LayoutOptions.FillAndExpand;

            imgVelocityCurve1On.Source = ImageSource.FromFile("VelocityCurve1On.png");
            imgVelocityCurve1Off.Source = ImageSource.FromFile("VelocityCurve1Off.png");
            imgVelocityCurve2On.Source = ImageSource.FromFile("VelocityCurve2On.png");
            imgVelocityCurve2Off.Source = ImageSource.FromFile("VelocityCurve2Off.png");
            imgVelocityCurve3On.Source = ImageSource.FromFile("VelocityCurve3On.png");
            imgVelocityCurve3Off.Source = ImageSource.FromFile("VelocityCurve3Off.png");
            imgVelocityCurve4On.Source = ImageSource.FromFile("VelocityCurve4On.png");
            imgVelocityCurve4Off.Source = ImageSource.FromFile("VelocityCurve4Off.png");

            btnVelocityCurve1.Clicked += BtnVelocity1Curve_Clicked;
            btnVelocityCurve2.Clicked += BtnVelocity2Curve_Clicked;
            btnVelocityCurve3.Clicked += BtnVelocity3Curve_Clicked;
            btnVelocityCurve4.Clicked += BtnVelocity4Curve_Clicked;

            gStudioSetPartVelocityCurve.Children.Add(imgVelocityCurve1On);
            gStudioSetPartVelocityCurve.Children.Add(imgVelocityCurve1Off);
            gStudioSetPartVelocityCurve.Children.Add(imgVelocityCurve2On);
            gStudioSetPartVelocityCurve.Children.Add(imgVelocityCurve2Off);
            gStudioSetPartVelocityCurve.Children.Add(imgVelocityCurve3On);
            gStudioSetPartVelocityCurve.Children.Add(imgVelocityCurve3Off);
            gStudioSetPartVelocityCurve.Children.Add(imgVelocityCurve4On);
            gStudioSetPartVelocityCurve.Children.Add(imgVelocityCurve4Off);
            gStudioSetPartVelocityCurve.Children.Add(btnVelocityCurve1);
            gStudioSetPartVelocityCurve.Children.Add(btnVelocityCurve2);
            gStudioSetPartVelocityCurve.Children.Add(btnVelocityCurve3);
            gStudioSetPartVelocityCurve.Children.Add(btnVelocityCurve4);

            // Part motional surround
            tbStudioSetPartMotionalSurroundLR.Text = "Motional surround L-R";
            slStudioSetPartMotionalSurroundLR.Minimum = -64;
            slStudioSetPartMotionalSurroundLR.Maximum = 63;
            slStudioSetPartMotionalSurroundLR.ValueChanged += slStudioSetPartMotionalSurroundLR_ValueChanged;
            tbStudioSetPartMotionalSurroundFB.Text = "Motional surround F-B";
            slStudioSetPartMotionalSurroundFB.Minimum = -64;
            slStudioSetPartMotionalSurroundFB.Maximum = 63;
            slStudioSetPartMotionalSurroundFB.ValueChanged += slStudioSetPartMotionalSurroundFB_ValueChanged;
            tbStudioSetPartMotionalSurroundWidth.Text = "Width";
            slStudioSetPartMotionalSurroundWidth.Minimum = 0;
            slStudioSetPartMotionalSurroundWidth.Maximum = 32;
            slStudioSetPartMotionalSurroundWidth.ValueChanged += slStudioSetPartMotionalSurroundWidth_ValueChanged;
            tbStudioSetPartMotionalSurroundAmbienceSendLevel.Text = "Ambience send level";
            slStudioSetPartMotionalSurroundAmbienceSendLevel.Minimum = 0;
            slStudioSetPartMotionalSurroundAmbienceSendLevel.Maximum = 127;
            slStudioSetPartMotionalSurroundAmbienceSendLevel.ValueChanged += slStudioSetPartMotionalSurroundAmbienceSendLevel_ValueChanged;

            // Part EQ settings
            cbStudioSetPartEQSwitch.Content = "Part equalizer on/off";
            cbStudioSetPartEQSwitch.CBSwitch.Toggled += cbStudioSetPartEQ_Click;
            tbStudioSetPartEQLowFreq.Text = "EQ low freq";
            cbStudioSetPartEQLowFreq.SelectedIndex = 0;
            cbStudioSetPartEQLowFreq.SelectedIndexChanged += cbStudioSetPartEQLoqFreq_SelectionChanged;
            tbStudioSetPartEQLowGain.Text = "EQ low gain";
            slStudioSetPartEQLowGain.Minimum = -15;
            slStudioSetPartEQLowGain.Maximum = 15;
            slStudioSetPartEQLowGain.ValueChanged += slStudioSetPartEQLowGain_ValueChanged;
            tbStudioSetPartEQMidFreq.Text = "EQ mid freq";
            cbStudioSetPartEQMidFreq.SelectedIndexChanged += cbStudioSetPartEQMidFreq_SelectionChanged;
            tbStudioSetPartEQMidGain.Text = "EQ mid gain";
            slStudioSetPartEQMidGain.Minimum = -15;
            slStudioSetPartEQMidGain.Maximum = 15;
            slStudioSetPartEQMidGain.ValueChanged += slStudioSetPartEQMidGain_ValueChanged;
            tbStudioSetPartEQMidQ.Text = "EQ mid Q";
            cbStudioSetPartEQMidQ.SelectedIndexChanged += cbStudioSetPartEQMidQ_SelectionChanged;
            tbStudioSetPartEQHighFreq.Text = "EQ high freq";
            cbStudioSetPartEQHighFreq.SelectedIndexChanged += cbStudioSetPartEQHighFreq_SelectionChanged;
            tbStudioSetPartEQHighGain.Text = "EQ high gain";
            slStudioSetPartEQHighGain.Minimum = -15;
            slStudioSetPartEQHighGain.Maximum = 15;
            slStudioSetPartEQHighGain.ValueChanged += slStudioSetPartEQHighGain_ValueChanged;
            cbStudioSetSlot.SelectedIndexChanged += cbStudioSetSlot_SelectionChanged;
            lblStudioSetName.Text = "Name:";
            tbStudioSetName.TextChanged += tbStudioSetName_KeyUp;
            btnFileSave.Content = "Save file";
            btnFileSave.Clicked += btnFileSave_Click;
            btnFileLoad.Content = "Load file";
            btnFileLoad.Clicked += btnFileLoad_Click;
            btnStudioSetSave.Content = "Save";
            btnStudioSetSave.Clicked += btnStudioSetSave_Click;
            btnStudioSetDelete.Content = "Delete";
            btnStudioSetDelete.Clicked += btnStudioSetDelete_Click;
            btnStudioSetReturn.Content = "Return";
            btnStudioSetReturn.Clicked += btnStudioSetReturn_Click;

            //---------------------------------------------------------------------------------------
            // Assemble system common grids with controls 
            //---------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------
            // Column 0 has no paging, so everything is assembled later when the other columns get
            // their pages put into the column.

            //---------------------------------------------------------------------------------------
            // Column 1

            // Common settings
            SystemCommonSettings.Children.Add((new GridRow(0, new View[] { tbSystemCommonMasterTune, slSystemCommonMasterTune })).Row);
            SystemCommonSettings.Children.Add((new GridRow(1, new View[] { tbSystemCommonMasterKeyShift, slSystemCommonMasterKeyShift })).Row);
            SystemCommonSettings.Children.Add((new GridRow(2, new View[] { tbSystemCommonMasterLevel, slSystemCommonMasterLevel })).Row);
            SystemCommonSettings.Children.Add((new GridRow(3, new View[] { cbSystemCommonScaleTuneSwitch })).Row);
            SystemCommonSettings.Children.Add((new GridRow(4, new View[] { tbSystemCommonStudioSetControlChannel, cbSystemCommonStudioSetControlChannel })).Row);
            SystemCommonSettings.Children.Add((new GridRow(5, new View[] { tbSystemCommonSystemControlSource1, cbSystemCommonSystemControlSource1 })).Row);
            SystemCommonSettings.Children.Add((new GridRow(6, new View[] { tbSystemCommonSystemControlSource2, cbSystemCommonSystemControlSource2 })).Row);
            SystemCommonSettings.Children.Add((new GridRow(7, new View[] { tbSystemCommonSystemControlSource3, cbSystemCommonSystemControlSource3 })).Row);
            SystemCommonSettings.Children.Add((new GridRow(8, new View[] { tbSystemCommonSystemControlSource4, cbSystemCommonSystemControlSource4 })).Row);
            SystemCommonSettings.Children.Add((new GridRow(9, new View[] { tbSystemCommonControlSource, cbSystemCommonControlSource })).Row);
            SystemCommonSettings.Children.Add((new GridRow(10, new View[] { tbSystemCommonSystemClockSource, cbSystemCommonSystemClockSource })).Row);
            SystemCommonSettings.Children.Add((new GridRow(11, new View[] { tbSystemCommonSystemTempo, slSystemCommonSystemTempo })).Row);
            SystemCommonSettings.Children.Add((new GridRow(12, new View[] { tbTempoAssignSource, cbSystemCommonTempoAssignSource })).Row);
            SystemCommonSettings.Children.Add((new GridRow(13, new View[] { cbSystemCommonReceiveProgramChange })).Row);
            SystemCommonSettings.Children.Add((new GridRow(14, new View[] { cbSystemCommonReceiveBankSelect })).Row);
            SystemCommonSettings.Children.Add((new GridRow(15, new View[] { cbSystemCommonSurroundCenterSpeakerSwitch })).Row);
            SystemCommonSettings.Children.Add((new GridRow(16, new View[] { cbSystemCommonSurroundSubWooferSwitch })).Row);
            SystemCommonSettings.Children.Add((new GridRow(17, new View[] { tbSystemCommonStereoOutputMode, cbSystemCommonStereoOutputMode })).Row);

            // Voice reserve
            VoiceReserve.Children.Add((new GridRow(0, new View[] { tbVoiceReserve01, slVoiceReserve01 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(1, new View[] { tbVoiceReserve02, slVoiceReserve02 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(2, new View[] { tbVoiceReserve03, slVoiceReserve03 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(3, new View[] { tbVoiceReserve04, slVoiceReserve04 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(4, new View[] { tbVoiceReserve05, slVoiceReserve05 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(5, new View[] { tbVoiceReserve06, slVoiceReserve06 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(6, new View[] { tbVoiceReserve07, slVoiceReserve07 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(7, new View[] { tbVoiceReserve08, slVoiceReserve08 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(8, new View[] { tbVoiceReserve09, slVoiceReserve09 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(9, new View[] { tbVoiceReserve10, slVoiceReserve10 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(10, new View[] { tbVoiceReserve11, slVoiceReserve11 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(11, new View[] { tbVoiceReserve12, slVoiceReserve12 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(12, new View[] { tbVoiceReserve13, slVoiceReserve13 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(13, new View[] { tbVoiceReserve14, slVoiceReserve14 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(14, new View[] { tbVoiceReserve15, slVoiceReserve15 }, new byte[] { 1, 2 })).Row);
            VoiceReserve.Children.Add((new GridRow(15, new View[] { tbVoiceReserve16, slVoiceReserve16 }, new byte[] { 1, 2 })).Row);

            // Chorus
            Chorus.Children.Add((new GridRow(0, new View[] { tbStudioSetChorusType, cbStudioSetChorusType }, new byte[] { 2, 1 })).Row);
            Chorus.Children.Add((new GridRow(1, new View[] { tbChorusLevel, slChorusLevel })).Row);
            Chorus.Children.Add((new GridRow(2, new View[] { tbStudioSetChorusOutputAssign, cbChorusOutputAssign }, new byte[] { 2, 1 })).Row);
            Chorus.Children.Add((new GridRow(3, new View[] { tbChorusOutputSelect, cbChorusOutputSelect }, new byte[] { 2, 1 })).Row);
            // Chorus chorus
            ChorusChorus.Children.Add((new GridRow(0, new View[] { tbChorusChorusFilterType, cbChorusChorusFilterType }, new byte[] { 2, 1 })).Row);
            ChorusChorus.Children.Add((new GridRow(1, new View[] { tbChorusChorusFilterCutoffFrequency, cbChorusChorusFilterCutoffFrequency }, new byte[] { 2, 1 })).Row);
            ChorusChorus.Children.Add((new GridRow(2, new View[] { tbChorusChorusPreDelay, slChorusChorusPreDelay })).Row);
            ChorusChorus.Children.Add((new GridRow(3, new View[] { tbChorusChorusRateHzNote, cbChorusChorusRateHzNote }, new byte[] { 2, 1 })).Row);
            ChorusChorus.Children.Add((new GridRow(4, new View[] { tbChorusChorusRateHz, slChorusChorusRateHz })).Row);
            ChorusChorus.Children.Add((new GridRow(5, new View[] { tbChorusChorusRateNote, slChorusChorusRateNote })).Row);
            ChorusChorus.Children.Add((new GridRow(6, new View[] { tbChorusChorusDepth, slChorusChorusDepth })).Row);
            ChorusChorus.Children.Add((new GridRow(7, new View[] { tbChorusChorusPhase, slChorusChorusPhase })).Row);
            ChorusChorus.Children.Add((new GridRow(8, new View[] { tbChorusChorusFeedback, slChorusChorusFeedback })).Row);
            // Chorus delay
            ChorusDelay.Children.Add((new GridRow(0, new View[] { tbChorusDelayLeftMsNote, cbChorusDelayLeftMsNote }, new byte[] { 2, 1 })).Row);
            ChorusDelay.Children.Add((new GridRow(1, new View[] { tbChorusDelayLeftHz, slChorusDelayLeftHz }, new byte[] { 1, 2 })).Row);
            ChorusDelay.Children.Add((new GridRow(2, new View[] { tbChorusDelayLeftNote, slChorusDelayLeftNote }, new byte[] { 1, 2 })).Row);
            ChorusDelay.Children.Add((new GridRow(3, new View[] { tbChorusDelayRightMsNote, cbChorusDelayRightMsNote }, new byte[] { 2, 1 })).Row);
            ChorusDelay.Children.Add((new GridRow(4, new View[] { tbChorusDelayRightHz, slChorusDelayRightHz }, new byte[] { 1, 2 })).Row);
            ChorusDelay.Children.Add((new GridRow(5, new View[] { tbChorusDelayRightNote, slChorusDelayRightNote }, new byte[] { 1, 2 })).Row);
            ChorusDelay.Children.Add((new GridRow(6, new View[] { tbChorusDelayCenterMsNote, cbChorusDelayCenterMsNote }, new byte[] { 2, 1 })).Row);
            ChorusDelay.Children.Add((new GridRow(7, new View[] { tbChorusDelayCenterHz, slChorusDelayCenterHz }, new byte[] { 1, 2 })).Row);
            ChorusDelay.Children.Add((new GridRow(8, new View[] { tbChorusDelayCenterNote, slChorusDelayCenterNote }, new byte[] { 1, 2 })).Row);
            ChorusDelay.Children.Add((new GridRow(9, new View[] { tbChorusDelayCenterFeedback, slChorusDelayCenterFeedback }, new byte[] { 1, 2 })).Row);
            ChorusDelay.Children.Add((new GridRow(10, new View[] { tbChorusDelayHFDamp, cbChorusDelayHFDamp }, new byte[] { 2, 1 })).Row);
            ChorusDelay.Children.Add((new GridRow(11, new View[] { tbChorusDelayLeftLevel, slChorusDelayLeftLevel }, new byte[] { 1, 2 })).Row);
            ChorusDelay.Children.Add((new GridRow(12, new View[] { tbChorusDelayRightLevel, slChorusDelayRightLevel }, new byte[] { 1, 2 })).Row);
            ChorusDelay.Children.Add((new GridRow(13, new View[] { tbChorusDelayCenterLevel, slChorusDelayCenterLevel }, new byte[] { 1, 2 })).Row);
            // Chorus GM2 chorus
            ChorusGM2Chorus.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusPreLPF, slChorusGM2ChorusPreLPF })).Row);
            ChorusGM2Chorus.Children.Add((new GridRow(1, new View[] { tbChorusGM2ChorusLevel, slChorusGM2ChorusLevel })).Row);
            ChorusGM2Chorus.Children.Add((new GridRow(2, new View[] { tbChorusGM2ChorusFeedback, slChorusGM2ChorusFeedback })).Row);
            ChorusGM2Chorus.Children.Add((new GridRow(3, new View[] { tbChorusGM2ChorusDelay, slChorusGM2ChorusDelay })).Row);
            ChorusGM2Chorus.Children.Add((new GridRow(4, new View[] { tbChorusGM2ChorusRate, slChorusGM2ChorusRate })).Row);
            ChorusGM2Chorus.Children.Add((new GridRow(5, new View[] { tbChorusGM2ChorusDepth, slChorusGM2ChorusDepth })).Row);
            ChorusGM2Chorus.Children.Add((new GridRow(6, new View[] { tbChorusGM2ChorusSendLevelToReverb, slChorusGM2ChorusSendLevelToReverb })).Row);
            // Assemble chorus subpages
            Chorus.Children.Add((new GridRow(4, new View[] { ChorusChorus }, null, false, true, 9)).Row);
            Chorus.Children.Add((new GridRow(4, new View[] { ChorusDelay }, null, false, true, 14)).Row);
            Chorus.Children.Add((new GridRow(4, new View[] { ChorusGM2Chorus }, null, false, true, 7)).Row);

            // Reverb
            Reverb.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbType, cbStudioSetReverbType }, new byte[] { 2, 3 })).Row);
            Reverb.Children.Add((new GridRow(1, new View[] { tbStudioSetReverbLevel, slStudioSetReverbLevel }, new byte[] { 2, 3 })).Row);
            Reverb.Children.Add((new GridRow(2, new View[] { tbStudioSetReverbOutputAssign, cbStudioSetReverbOutputAssign }, new byte[] { 2, 3 })).Row);
            // Reverb all except GM2
            StudioSetReverbReverb.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbPreDelay, slStudioSetReverbPreDelay }, new byte[] { 1, 2 })).Row);
            StudioSetReverbReverb.Children.Add((new GridRow(1, new View[] { tbStudioSetReverbTime, slStudioSetReverbTime }, new byte[] { 1, 2 })).Row);
            StudioSetReverbReverb.Children.Add((new GridRow(2, new View[] { tbStudioSetReverbDensity, slStudioSetReverbDensity }, new byte[] { 1, 2 })).Row);
            StudioSetReverbReverb.Children.Add((new GridRow(3, new View[] { tbStudioSetReverbDiffusion, slStudioSetReverbDiffusion }, new byte[] { 1, 2 })).Row);
            StudioSetReverbReverb.Children.Add((new GridRow(4, new View[] { tbStudioSetReverbLFDamp, slStudioSetReverbLFDamp }, new byte[] { 1, 2 })).Row);
            StudioSetReverbReverb.Children.Add((new GridRow(5, new View[] { tbStudioSetReverbHFDamp, slStudioSetReverbHFDamp }, new byte[] { 1, 2 })).Row);
            StudioSetReverbReverb.Children.Add((new GridRow(6, new View[] { tbStudioSetReverbSpread, slStudioSetReverbSpread }, new byte[] { 1, 2 })).Row);
            StudioSetReverbReverb.Children.Add((new GridRow(7, new View[] { tbStudioSetReverbTone, slStudioSetReverbTone }, new byte[] { 1, 2 })).Row);
            // Reverb GM2
            StudioSetReverbGM2.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbGM2Character, slStudioSetReverbGM2Character }, new byte[] { 1, 2 })).Row);
            StudioSetReverbGM2.Children.Add((new GridRow(1, new View[] { tbStudioSetReverbGM2Time, slStudioSetReverbGM2Time }, new byte[] { 1, 2 })).Row);
            // Add the sub-pages
            Reverb.Children.Add((new GridRow(3, new View[] { StudioSetReverbReverb }, null, false, true, 8)).Row);
            Reverb.Children.Add((new GridRow(3, new View[] { StudioSetReverbGM2 }, null, false, true, 2)).Row);

            // Motional surround

            StudioSetMotionalSurround.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundCommon, cbStudioSetMotionalSurround })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(1, new View[] { tbStudioSetMotionalSurroundRoomType, cbStudioSetMotionalSurroundRoomType })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(2, new View[] { tbStudioSetMotionalSurroundRoomSize, cbStudioSetMotionalSurroundRoomSize })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(3, new View[] { tbStudioSetMotionalSurroundDepth, slStudioSetMotionalSurroundDepth })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(4, new View[] { tbStudioSetMotionalSurroundAmbienceLevel, slStudioSetMotionalSurroundAmbienceLevel })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(5, new View[] { tbStudioSetMotionalSurroundAmbienceTime, slStudioSetMotionalSurroundAmbienceTime })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(6, new View[] { tbStudioSetMotionalSurroundAmbienceDensity, slStudioSetMotionalSurroundAmbienceDensity })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(7, new View[] { tbStudioSetMotionalSurroundAmbienceHFDamp, slStudioSetMotionalSurroundAmbienceHFDamp })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(8, new View[] { tbStudioSetMotionalSurroundExt })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(9, new View[] { tbStudioSetMotionalSurroundExternalPartLR, slStudioSetMotionalSurroundExternalPartLR })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(10, new View[] { tbStudioSetMotionalSurroundExternalPartFB, slStudioSetMotionalSurroundExternalPartFB })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(11, new View[] { tbStudioSetMotionalSurroundExtPartWidth, slStudioSetMotionalSurroundExtPartWidth })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(12, new View[] { tbStudioSetMotionalSurroundExtpartAmbienceSend, slStudioSetMotionalSurroundExtpartAmbienceSend })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(13, new View[] { tbStudioSetMotionalSurroundControl })).Row);
            StudioSetMotionalSurround.Children.Add((new GridRow(14, new View[] { tbStudioSetMotionalSurroundExtPartControl, cbStudioSetMotionalSurroundExtPartControl })).Row);

            // Master EQ
            StudioSetMasterEQ.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqLowFreq, cbStudioSetMasterEqLowFreq })).Row);
            StudioSetMasterEQ.Children.Add((new GridRow(1, new View[] { tbStudioSetMasterEqLowGain, slStudioSetMasterEqLowGain })).Row);
            StudioSetMasterEQ.Children.Add((new GridRow(2, new View[] { tbStudioSetMasterEqMidFreq, cbStudioSetMasterEqMidFreq })).Row);
            StudioSetMasterEQ.Children.Add((new GridRow(3, new View[] { tbStudioSetMasterEqMidGain, slStudioSetMasterEqMidGain })).Row);
            StudioSetMasterEQ.Children.Add((new GridRow(4, new View[] { tbStudioSetMasterEqMidQ, cbStudioSetMasterEqMidQ })).Row);
            StudioSetMasterEQ.Children.Add((new GridRow(5, new View[] { tbStudioSetMasterEqHighFreq, cbStudioSetMasterEqHighFreq })).Row);
            StudioSetMasterEQ.Children.Add((new GridRow(6, new View[] { tbStudioSetMasterEqHighGain, slStudioSetMasterEqHighGain })).Row);

            //---------------------------------------------------------------------------------------
            // Column 2

            grid_StudioSet_Column2.Children.Add((new GridRow(0, new View[] { cbStudioSetPartSelector })).Row);
            grid_StudioSet_Column2.Children.Add((new GridRow(1, new View[] { cbStudioSetPartSubSelector })).Row);
            grid_StudioSet_Column2.Children.Add((new GridRow(2, new View[] { StudioSetCurrentToneName })).Row);
            grid_StudioSet_Column2.Children.Add((new GridRow(3, new View[] { grid_PartSettings }, null, false, true, 14)).Row);
            grid_StudioSet_Column2.Children.Add((new GridRow(17, new View[] { grid_Buttons }, null, false, true, 2)).Row);

            grid_PartSettings.Children.Add((new GridRow(0, new View[] { StudioSetPartSettings1 }, null, false, true, 14)).Row);
            grid_PartSettings.Children.Add((new GridRow(0, new View[] { StudioSetPartSettings2 }, null, false, true, 8)).Row);
            grid_PartSettings.Children.Add((new GridRow(0, new View[] { StudioSetPartEffects }, null, false, true, 3)).Row);
            grid_PartSettings.Children.Add((new GridRow(0, new View[] { StudioSetPartKeyboard }, null, false, true, 11)).Row);
            grid_PartSettings.Children.Add((new GridRow(0, new View[] { StudioSetPartScaleTune }, null, false, true, 14)).Row);
            grid_PartSettings.Children.Add((new GridRow(0, new View[] { StudioSetPartMidi }, null, false, true, 14)).Row);
            grid_PartSettings.Children.Add((new GridRow(0, new View[] { StudioSetPartMotionalSurround }, null, false, true, 4)).Row);
            grid_PartSettings.Children.Add((new GridRow(0, new View[] { StudioSetPartEQ }, null, false, true, 8)).Row);

            StudioSetPartSettings1.Children.Add((new GridRow(0, new View[] { cbStudioSetPartSettings1Receive, cbStudioSetPartSettings1ReceiveChannel })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(1, new View[] { tbStudioSetPartSettings1Group, cbStudioSetPartSettings1Group })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(2, new View[] { tbStudioSetPartSettings1Category, cbStudioSetPartSettings1Category }, new byte[] { 96, 71 })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(3, new View[] { tbStudioSetPartSettings1Program, cbStudioSetPartSettings1Program })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(4, new View[] { tbStudioSetPartSettings1Search, cbStudioSetPartSettings1Search }, new byte[] { 1, 4 })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(5, new View[] { tbStudioSetPartSettings1Level, slStudioSetPartSettings1Level })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(6, new View[] { tbStudioSetPartSettings1Pan, slStudioSetPartSettings1Pan })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(7, new View[] { tbStudioSetPartSettings1CoarseTune, slStudioSetPartSettings1CoarseTune })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(8, new View[] { tbStudioSetPartSettings1FineTune, slStudioSetPartSettings1FineTune })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(9, new View[] { tbStudioSetPartSettings1MonoPoly, cbStudioSetPartSettings1MonoPoly })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(10, new View[] { tbStudioSetPartSettings1Legato, cbStudioSetPartSettings1Legato })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(11, new View[] { tbStudioSetPartSettings1PitchBendRange, slStudioSetPartSettings1PitchBendRange })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(12, new View[] { tbStudioSetPartSettings1Portamento, cbStudioSetPartSettings1Portamento })).Row);
            StudioSetPartSettings1.Children.Add((new GridRow(13, new View[] { tbStudioSetPartSettings1PortamentoTime, slStudioSetPartSettings1PortamentoTime })).Row);

            StudioSetPartSettings2.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2CutoffOffset, slStudioSetPartSettings2CutoffOffset })).Row);
            StudioSetPartSettings2.Children.Add((new GridRow(1, new View[] { tbStudioSetPartSettings2ResonanceOffset, slStudioSetPartSettings2ResonanceOffset })).Row);
            StudioSetPartSettings2.Children.Add((new GridRow(2, new View[] { tbStudioSetPartSettings2AttackTimeOffset, slStudioSetPartSettings2AttackTimeOffset })).Row);
            StudioSetPartSettings2.Children.Add((new GridRow(3, new View[] { tbStudioSetPartSettings2DecayTimeOffset, slStudioSetPartSettings2DecayTimeOffset })).Row);
            StudioSetPartSettings2.Children.Add((new GridRow(4, new View[] { tbStudioSetPartSettings2ReleaseTimeOffset, slStudioSetPartSettings2ReleaseTimeOffset })).Row);
            StudioSetPartSettings2.Children.Add((new GridRow(5, new View[] { tbStudioSetPartSettings2VibratoRate, slStudioSetPartSettings2VibratoRate })).Row);
            StudioSetPartSettings2.Children.Add((new GridRow(6, new View[] { tbStudioSetPartSettings2VibratoDepth, slStudioSetPartSettings2VibratoDepth })).Row);
            StudioSetPartSettings2.Children.Add((new GridRow(7, new View[] { tbStudioSetPartSettings2VibratoDelay, slStudioSetPartSettings2VibratoDelay })).Row);

            StudioSetPartEffects.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEffectsChorusSendLevel, slStudioSetPartEffectsChorusSendLevel })).Row);
            StudioSetPartEffects.Children.Add((new GridRow(1, new View[] { tbStudioSetPartEffectsReverbSendLevel, slStudioSetPartEffectsReverbSendLevel })).Row);
            StudioSetPartEffects.Children.Add((new GridRow(2, new View[] { tbStudioSetPartEffectsOutputAssign, cbStudioSetPartEffectsOutputAssign })).Row);

            StudioSetPartKeyboard.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardOctaveShift, slStudioSetPartKeyboardOctaveShift })).Row);
            StudioSetPartKeyboard.Children.Add((new GridRow(1, new View[] { tbStudioSetPartKeyboardVelocitySenseOffset, slStudioSetPartKeyboardVelocitySenseOffset })).Row);
            StudioSetPartKeyboard.Children.Add((new GridRow(2, new View[] { tbStudioSetPartKeyboardRangeLower, slStudioSetPartKeyboardRangeLower })).Row);
            StudioSetPartKeyboard.Children.Add((new GridRow(3, new View[] { tbStudioSetPartKeyboardRangeUpper, slStudioSetPartKeyboardRangeUpper })).Row);
            StudioSetPartKeyboard.Children.Add((new GridRow(4, new View[] { tbStudioSetPartKeyboardFadeWidthLower, slStudioSetPartKeyboardFadeWidthLower })).Row);
            StudioSetPartKeyboard.Children.Add((new GridRow(5, new View[] { tbStudioSetPartKeyboardFadeWidthUpper, slStudioSetPartKeyboardFadeWidthUpper })).Row);
            StudioSetPartKeyboard.Children.Add((new GridRow(6, new View[] { tbStudioSetPartKeyboardVelocityRangeLower, slStudioSetPartKeyboardVelocityRangeLower })).Row);
            StudioSetPartKeyboard.Children.Add((new GridRow(7, new View[] { tbStudioSetPartKeyboardVelocityRangeUpper, slStudioSetPartKeyboardVelocityRangeUpper })).Row);
            StudioSetPartKeyboard.Children.Add((new GridRow(8, new View[] { tbStudioSetPartKeyboardVelocityFadeWidthLower, slStudioSetPartKeyboardVelocityFadeWidthLower })).Row);
            StudioSetPartKeyboard.Children.Add((new GridRow(9, new View[] { tbStudioSetPartKeyboardVelocityFadeWidthUpper, slStudioSetPartKeyboardVelocityFadeWidthUpper })).Row);
            StudioSetPartKeyboard.Children.Add((new GridRow(10, new View[] { tbStudioSetPartKeyboardVelocityCurveType, cbStudioSetPartKeyboardVelocityCurveType })).Row);

            StudioSetPartScaleTune.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneType, cbStudioSetPartScaleTuneType })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(1, new View[] { tbStudioSetPartScaleTuneKey, cbStudioSetPartScaleTuneKey })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(2, new View[] { tbStudioSetPartScaleTuneC, slStudioSetPartScaleTuneC })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(3, new View[] { tbStudioSetPartScaleTuneCi, slStudioSetPartScaleTuneCi })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(4, new View[] { tbStudioSetPartScaleTuneD, slStudioSetPartScaleTuneD })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(5, new View[] { tbStudioSetPartScaleTuneDi, slStudioSetPartScaleTuneDi })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(6, new View[] { tbStudioSetPartScaleTuneE, slStudioSetPartScaleTuneE })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(7, new View[] { tbStudioSetPartScaleTuneF, slStudioSetPartScaleTuneF })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(8, new View[] { tbStudioSetPartScaleTuneFi, slStudioSetPartScaleTuneFi })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(9, new View[] { tbStudioSetPartScaleTuneG, slStudioSetPartScaleTuneG })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(10, new View[] { tbStudioSetPartScaleTuneGi, slStudioSetPartScaleTuneGi })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(11, new View[] { tbStudioSetPartScaleTuneA, slStudioSetPartScaleTuneA })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(12, new View[] { tbStudioSetPartScaleTuneAi, slStudioSetPartScaleTuneAi })).Row);
            StudioSetPartScaleTune.Children.Add((new GridRow(13, new View[] { tbStudioSetPartScaleTuneB, slStudioSetPartScaleTuneB })).Row);

            StudioSetPartMidi.Children.Add((new GridRow(0, new View[] { cbStudioSetPartMidiPhaseLock })).Row);
            StudioSetPartMidi.Children.Add((new GridRow(1, new View[] { cbStudioSetPartMidiReceiveProgramChange })).Row);
            StudioSetPartMidi.Children.Add((new GridRow(2, new View[] { cbStudioSetPartMidiReceiveBankSelect })).Row);
            StudioSetPartMidi.Children.Add((new GridRow(3, new View[] { cbStudioSetPartMidiReceivePitchBend })).Row);
            StudioSetPartMidi.Children.Add((new GridRow(4, new View[] { cbStudioSetPartMidiReceivePolyphonicKeyPressure })).Row);
            StudioSetPartMidi.Children.Add((new GridRow(5, new View[] { cbStudioSetPartMidiReceiveChannelPressure })).Row);
            StudioSetPartMidi.Children.Add((new GridRow(6, new View[] { cbStudioSetPartMidiReceiveModulation })).Row);
            StudioSetPartMidi.Children.Add((new GridRow(7, new View[] { cbStudioSetPartMidiReceiveVolume })).Row);
            StudioSetPartMidi.Children.Add((new GridRow(8, new View[] { cbStudioSetPartMidiReceivePan })).Row);
            StudioSetPartMidi.Children.Add((new GridRow(9, new View[] { cbStudioSetPartMidiReceiveExpression })).Row);
            StudioSetPartMidi.Children.Add((new GridRow(10, new View[] { cbStudioSetPartMidiReceiveHold1 })).Row);
            StudioSetPartMidi.Children.Add((new GridRow(11, new View[] { cbStudioSetPartKeyboardVelocityCurveType })).Row);
            StudioSetPartMidi.Children.Add((new GridRow(12, new View[] { gStudioSetPartVelocityCurve }, null, false, false, 2)).Row);

            StudioSetPartMotionalSurround.Children.Add((new GridRow(0, new View[] { tbStudioSetPartMotionalSurroundLR, slStudioSetPartMotionalSurroundLR })).Row);
            StudioSetPartMotionalSurround.Children.Add((new GridRow(1, new View[] { tbStudioSetPartMotionalSurroundFB, slStudioSetPartMotionalSurroundFB })).Row);
            StudioSetPartMotionalSurround.Children.Add((new GridRow(2, new View[] { tbStudioSetPartMotionalSurroundWidth, slStudioSetPartMotionalSurroundWidth })).Row);
            StudioSetPartMotionalSurround.Children.Add((new GridRow(3, new View[] { tbStudioSetPartMotionalSurroundAmbienceSendLevel, slStudioSetPartMotionalSurroundAmbienceSendLevel })).Row);

            StudioSetPartEQ.Children.Add((new GridRow(0, new View[] { cbStudioSetPartEQSwitch })).Row);
            StudioSetPartEQ.Children.Add((new GridRow(1, new View[] { tbStudioSetPartEQLowFreq, cbStudioSetPartEQLowFreq })).Row);
            StudioSetPartEQ.Children.Add((new GridRow(2, new View[] { tbStudioSetPartEQLowGain, slStudioSetPartEQLowGain })).Row);
            StudioSetPartEQ.Children.Add((new GridRow(3, new View[] { tbStudioSetPartEQMidFreq, cbStudioSetPartEQMidFreq })).Row);
            StudioSetPartEQ.Children.Add((new GridRow(4, new View[] { tbStudioSetPartEQMidGain, slStudioSetPartEQMidGain })).Row);
            StudioSetPartEQ.Children.Add((new GridRow(5, new View[] { tbStudioSetPartEQMidQ, cbStudioSetPartEQMidQ })).Row);
            StudioSetPartEQ.Children.Add((new GridRow(6, new View[] { tbStudioSetPartEQHighFreq, cbStudioSetPartEQHighFreq })).Row);
            StudioSetPartEQ.Children.Add((new GridRow(7, new View[] { tbStudioSetPartEQHighGain, slStudioSetPartEQHighGain })).Row);

            grid_Buttons.Children.Add((new GridRow(0, new View[] { cbStudioSetSlot, lblStudioSetName, tbStudioSetName }, new byte[] { 5, 3, 8 })).Row);
            grid_Buttons.Children.Add((new GridRow(1, new View[] { btnFileSave, btnFileLoad, btnStudioSetSave, btnStudioSetDelete, btnStudioSetReturn }, new byte[] { 3, 3, 2, 2, 2 })).Row);

            //---------------------------------------------------------------------------------------
            // Assemble column 0 
            //---------------------------------------------------------------------------------------

            grid_StudioSet_Column0.Children.Add((new GridRow(0, new View[] { cbStudioSetSelector })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(1, new View[] { tbToneControl1, cbToneControl1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(2, new View[] { tbToneControl2, cbToneControl2 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(3, new View[] { tbToneControl3, cbToneControl3 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(4, new View[] { tbToneControl4, cbToneControl4 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(5, new View[] { tbTempo, slTempo })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(6, new View[] { tbSoloPart, cbSoloPart })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(7, new View[] { cbReverb, cbChorus, cbMasterEQ }, new byte[] { 1, 1, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(8, new View[] { tbDrumCompEQPart, cbDrumCompEQPart }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(9, new View[] { tbDrumCompEQ1OutputAssign, cbDrumCompEQ1OutputAssign }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(10, new View[] { tbDrumCompEQ2OutputAssign, cbDrumCompEQ2OutputAssign }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(11, new View[] { tbDrumCompEQ3OutputAssign, cbDrumCompEQ3OutputAssign }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(12, new View[] { tbDrumCompEQ4OutputAssign, cbDrumCompEQ4OutputAssign }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(13, new View[] { tbDrumCompEQ5OutputAssign, cbDrumCompEQ5OutputAssign }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(14, new View[] { tbDrumCompEQ6OutputAssign, cbDrumCompEQ6OutputAssign }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(15, new View[] { cbDrumCompEQ, cbExtPartMute }, new byte[] { 7, 6 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(16, new View[] { tbExtPartLevel, slExtPartLevel })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(17, new View[] { tbExtPartChorusSend, slExtPartChorusSend })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(18, new View[] { tbExtPartReverbSend, slExtPartReverbSend })).Row);

            //---------------------------------------------------------------------------------------
            // Assemble column 1 
            //---------------------------------------------------------------------------------------

            gStudioSet_Column1.Children.Add((new GridRow(0, new View[] { cbColumn1Selector })).Row);
            gStudioSet_Column1.Children.Add((new GridRow(1, new View[] { SystemCommonSettings }, null, false, true, 18)).Row);
            gStudioSet_Column1.Children.Add((new GridRow(1, new View[] { VoiceReserve }, null, false, true, 18)).Row);
            gStudioSet_Column1.Children.Add((new GridRow(1, new View[] { Chorus }, null, false, true, 18)).Row);
            gStudioSet_Column1.Children.Add((new GridRow(1, new View[] { Reverb }, null, false, true, 18)).Row);
            gStudioSet_Column1.Children.Add((new GridRow(1, new View[] { StudioSetMotionalSurround }, null, false, true, 15)).Row);
            gStudioSet_Column1.Children.Add((new GridRow(1, new View[] { StudioSetMasterEQ }, null, false, true, 7)).Row);

            grid_StudioSet_Column1.Children.Add((new GridRow(0, new View[] { gStudioSet_Column1 }, null, false, true, 19)).Row);
            grid_StudioSet_Column1.Children.Add((new GridRow(0, new View[] { gEditStudioSetSearchResult }, null, false, true, 19)).Row);

            //---------------------------------------------------------------------------------------
            // Assemble column 2 
            //---------------------------------------------------------------------------------------

            gEditStudioSetSearchResult.Children.Add((new GridRow(0, new View[] { lvSearchResults })).Row);

            //grid_StudioSet_Column2.Children.Add((new GridRow(1, new View[] { grid_0712 })).Row);
            //gEditStudioSetColumn2.Children.Add((new GridRow(1, new View[] { grid_PartSelector, grid_PartSettings, grid_StudioSetPartSubSelector, StudioSetPartSettings1, StudioSetPartSettings2,
            //    StudioSetPartKeyboard, StudioSetPartScaleTune, StudioSetPartMidi, StudioSetPartMotionalSurround, StudioSetPartEQ, Buttons })).Row);

            //---------------------------------------------------------------------------------------
            // Assemble StudioSetEditorStackLayout 
            //---------------------------------------------------------------------------------------

            StudioSetEditor_StackLayout = new StackLayout();
            // put the 3 columns together. The column gEditStudioSetSearchResult will replace gEditStudioSetColumn1 in StudioSetEditor_StackLayout.Children[1]
            // when searching is active, and gEditStudioSetColumn1 will be put back when seach ends.
            Grid grid_StudioSet_Column0_Container = new Grid();
            Grid grid_StudioSet_Column1_Container = new Grid();
            Grid grid_StudioSet_Column2_Container = new Grid();
            grid_StudioSet_Column0_Container.BackgroundColor = colorSettings.Border;
            grid_StudioSet_Column1_Container.BackgroundColor = colorSettings.Border;
            grid_StudioSet_Column2_Container.BackgroundColor = colorSettings.Border;
            grid_StudioSet_Column0.BackgroundColor = colorSettings.Background;
            grid_StudioSet_Column1.BackgroundColor = colorSettings.Background;
            grid_StudioSet_Column2.BackgroundColor = colorSettings.Background;
            grid_StudioSet_Column0.Margin = new Thickness(2, 2, 2, 2);
            grid_StudioSet_Column1.Margin = new Thickness(2, 2, 2, 2);
            grid_StudioSet_Column2.Margin = new Thickness(2, 2, 2, 2);
            grid_StudioSet_Column0_Container.Children.Add(grid_StudioSet_Column0);
            grid_StudioSet_Column1_Container.Children.Add(grid_StudioSet_Column1);
            grid_StudioSet_Column2_Container.Children.Add(grid_StudioSet_Column2);
            StudioSetEditor_StackLayout.Children.Add((new GridRow(0, new View[] { grid_StudioSet_Column0_Container, grid_StudioSet_Column1_Container, grid_StudioSet_Column2_Container }, null, true, false)).Row);

            //StudioSetEditor_StackLayout.BackgroundColor = Color.Black;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Studio set editor functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void StudioSetEditor_Init()
        {
            t.Trace("private void StudioSetEditor_Init()");
            studioSetEditor_State = StudioSetEditor_State.INIT;
            try
            {
                //commonState = (CommonState)e.Parameter;
                //if (firstTime)
                //{
                //    //commonState.midi.midiInPort.MessageReceived += Edit_MidiInPort_MessageReceived;
                //    firstTime = false;
                //}
                if (btnEditTone_Play != null)
                {
                    commonState.Player.btnPlayStop = btnEditTone_Play;
                    if (commonState.Player.Playing)
                    {
                        btnEditTone_Play.Content = "Stop";
                    }
                }
                //commonState.reactToMidiInAndTimerTick = CommonState.ReactToMidiInAndTimerTick.EDIT_STUDIO_SET;
            }
            catch { }
            //Waiting(true, "Please wait while scanning Studio set names and initiating form...", StudioSetEditor_StackLayout);
            studioSetEditor_State = StudioSetEditor_State.INIT;
            ResetComboBoxes();
            hex2Midi = new Hex2Midi();
            //commonState.midi.midiInPort.MessageReceived += EditStudioSet_MidiInPort_MessageReceived;
            if (commonState.StudioSetNames == null)
            {
                //grid_MainStudioSet.IsVisible = false;
                //commonState.midi.midiInPort.MessageReceived += EditStudioSet_MidiInPort_MessageReceived;
                // Get a list of all studio set names. Start by storing the current studio set number.
                // Note that consequent queries will be sent from MidiInPort_MessageReceived and Timer_Tick.
                commonState.StudioSetNames = new List<String>();
                QueryCurrentStudioSetNumber(true); // And scan all studio set names.

            }
            else
            {
                //grid_PleaseWaitWhileScanning.IsVisible = true;
                StudioSetEditor_StackLayout.IsVisible = false;
                // Re-populate studio set names list:
                //PopulateStudioSetSelector();

                // Tell Timer_Tick that we have the studio set names:
                //QueryCurrentStudioSetNumber(false);
                //studioSetEditor_State = StudioSetEditor_State.NONE;
                //initDone = true;

                // Ask for system common settings:
                //QuerySystemCommon(); // This will be caught in MidiInPort_MessageReceived

                QueryCurrentStudioSetNumber(false); // But don't scan all studio set names.
            }
            btnStudioSetSave.IsEnabled = !commonState.VenderDriverIsInstalled;
            btnStudioSetDelete.IsEnabled = !commonState.VenderDriverIsInstalled;
            StudioSetEditor_SearchResult = new ObservableCollection<String>();
            lvSearchResults.ItemsSource = StudioSetEditor_SearchResult;
        }

        private void PopulateComboBoxes()
        {
            PopulateTonControl(cbToneControl1);
            PopulateTonControl(cbToneControl2);
            PopulateTonControl(cbToneControl3);
            PopulateTonControl(cbToneControl4);
            PopulateSoloPart(); // Off, Part 1- Part 16
            PopulateDrumCompEQPart();
            populateOutputAssign(cbDrumCompEQ1OutputAssign); // Part, A - 8
            populateOutputAssign(cbDrumCompEQ2OutputAssign);
            populateOutputAssign(cbDrumCompEQ3OutputAssign);
            populateOutputAssign(cbDrumCompEQ4OutputAssign);
            populateOutputAssign(cbDrumCompEQ5OutputAssign);
            populateOutputAssign(cbDrumCompEQ6OutputAssign);
            populateColumn2Selector();
            populateStudioSetControlChannel(cbSystemCommonStudioSetControlChannel); // Ch 1 - Ch 16, Off
            populateStudioSetControlChannel(cbStudioSetMotionalSurroundExtPartControl);
            PopulateTonControl(cbSystemCommonSystemControlSource1);
            PopulateTonControl(cbSystemCommonSystemControlSource2);
            PopulateTonControl(cbSystemCommonSystemControlSource3);
            PopulateTonControl(cbSystemCommonSystemControlSource4);
            PopulateSystemSource(cbSystemCommonControlSource); // System, Studio Set
            PopulateSystemSource(cbSystemCommonTempoAssignSource);
            PopulateSystemCommonClockSource(); // MIDI, USB
            SystemCommonStereoOutputMode();
            StudioSetChorusType();
            StudioSetOutputAssign(cbChorusOutputAssign);
            StudioSetOutputAssign(cbStudioSetReverbOutputAssign);
            ChorusOutputSelect();
            ChorusChorusFilterType();
            StudioSetFreq(cbChorusChorusFilterCutoffFrequency);
            StudioSetFreq(cbChorusDelayHFDamp);
            cbChorusDelayHFDamp.Items.Add("ByPass");
            StudioSetFreq(cbStudioSetMasterEqMidFreq);
            StudioSetFreq(cbStudioSetPartEQMidFreq);
            ChorusChorusRateHzNote();
            StudioSetMsNote(cbChorusDelayLeftMsNote);
            StudioSetMsNote(cbChorusDelayRightMsNote);
            StudioSetMsNote(cbChorusDelayCenterMsNote);
            StudioSetReverbType();
            StudioSetMotionalSurroundRoomType();
            StudioSetMotionalSurroundRoomSize();
            StudioSetMasterEqLowFreq();
            StudioSetMasterEqMidQ();
            StudioSetMasterEqHighFreq();
            StudioSetPartSelector();
            StudioSetPartSubSelector();
            StudioSetPartSettings1ReceiveChannel();
            StudioSetPartSettings1MonoPoly();
            StudioSetPartSettings1Legato();
            StudioSetPartSettings1Portamento();
            StudioSetPartEffectsOutputAssign();
            StudioSetPartKeyboardVelocityCurveType();
            StudioSetPartScaleTuneType();
            StudioSetPartScaleTuneKey();
            StudioSetPartEQLoqFreq();
            StudioSetPartEQMidQ();
            StudioSetPartEQHighFreq();

        }

        private void PopulateTonControl(ComboBox toneControl)
        {
            toneControl.Items.Clear();
            toneControl.Items.Add("Off");
            toneControl.Items.Add("CC01: Modulation");
            toneControl.Items.Add("CC02: Breath");
            toneControl.Items.Add("CC03");
            toneControl.Items.Add("CC04: Foot type");
            toneControl.Items.Add("CC05: Porta time");
            toneControl.Items.Add("CC06: Data entry");
            toneControl.Items.Add("CC07: Volume");
            toneControl.Items.Add("CC08: Balance");
            toneControl.Items.Add("CC09");
            toneControl.Items.Add("CC10: Panpot");
            toneControl.Items.Add("CC11: Expression");
            toneControl.Items.Add("CC12");
            toneControl.Items.Add("CC13");
            toneControl.Items.Add("CC14");
            toneControl.Items.Add("CC15");
            toneControl.Items.Add("cc16: General 1");
            toneControl.Items.Add("CC17: General 2");
            toneControl.Items.Add("CC18: General 3");
            toneControl.Items.Add("CC19: General 4");
            toneControl.Items.Add("CC20");
            toneControl.Items.Add("CC21");
            toneControl.Items.Add("CC22");
            toneControl.Items.Add("CC23");
            toneControl.Items.Add("CC24");
            toneControl.Items.Add("CC25");
            toneControl.Items.Add("CC26");
            toneControl.Items.Add("CC27");
            toneControl.Items.Add("CC28");
            toneControl.Items.Add("CC29");
            toneControl.Items.Add("CC30");
            toneControl.Items.Add("CC31");
            toneControl.Items.Add("N/A");
            toneControl.Items.Add("CC33");
            toneControl.Items.Add("CC34");
            toneControl.Items.Add("CC35");
            toneControl.Items.Add("CC36");
            toneControl.Items.Add("CC37");
            toneControl.Items.Add("CC38: Data entry");
            toneControl.Items.Add("CC39");
            toneControl.Items.Add("CC40");
            toneControl.Items.Add("CC41");
            toneControl.Items.Add("CC42");
            toneControl.Items.Add("CC43");
            toneControl.Items.Add("CC44");
            toneControl.Items.Add("CC45");
            toneControl.Items.Add("CC46");
            toneControl.Items.Add("CC47");
            toneControl.Items.Add("CC48");
            toneControl.Items.Add("CC49");
            toneControl.Items.Add("CC50");
            toneControl.Items.Add("CC51");
            toneControl.Items.Add("CC52");
            toneControl.Items.Add("CC53");
            toneControl.Items.Add("CC54");
            toneControl.Items.Add("CC55");
            toneControl.Items.Add("CC56");
            toneControl.Items.Add("CC57");
            toneControl.Items.Add("CC58");
            toneControl.Items.Add("CC59");
            toneControl.Items.Add("CC60");
            toneControl.Items.Add("CC61");
            toneControl.Items.Add("CC62");
            toneControl.Items.Add("CC63");
            toneControl.Items.Add("CC64: Hold 1");
            toneControl.Items.Add("CC65: Portamento");
            toneControl.Items.Add("CC66: Sostenuto");
            toneControl.Items.Add("CC67: Soft");
            toneControl.Items.Add("CC68: Legato switch");
            toneControl.Items.Add("CC69: Hold 2");
            toneControl.Items.Add("CC70");
            toneControl.Items.Add("CC71: Resonance");
            toneControl.Items.Add("CC72: Release time");
            toneControl.Items.Add("CC73: Attack time");
            toneControl.Items.Add("CC74: Cutoff");
            toneControl.Items.Add("CC75: Decay time");
            toneControl.Items.Add("CC76: Vib. rate");
            toneControl.Items.Add("CC77: Vib. depth");
            toneControl.Items.Add("CC78: Vib. delay");
            toneControl.Items.Add("CC79");
            toneControl.Items.Add("CC80: General 5");
            toneControl.Items.Add("CC81: General 6");
            toneControl.Items.Add("CC82: General 7");
            toneControl.Items.Add("CC83: General 8");
            toneControl.Items.Add("CC84");
            toneControl.Items.Add("CC85");
            toneControl.Items.Add("CC86");
            toneControl.Items.Add("CC87");
            toneControl.Items.Add("CC88");
            toneControl.Items.Add("CC89");
            toneControl.Items.Add("CC90");
            toneControl.Items.Add("CC91: Reverb");
            toneControl.Items.Add("CC92: Tremolo");
            toneControl.Items.Add("CC93: Chorus");
            toneControl.Items.Add("CC94: Celeste");
            toneControl.Items.Add("CC95: Phaser");
            toneControl.Items.Add("Pitch bend");
            toneControl.Items.Add("Aftertouch");
        }

        private void PopulateSoloPart()
        {
            cbSoloPart.Items.Add("Off");
            cbSoloPart.Items.Add("Part 1");
            cbSoloPart.Items.Add("Part 2");
            cbSoloPart.Items.Add("Part 3");
            cbSoloPart.Items.Add("Part 4");
            cbSoloPart.Items.Add("Part 5");
            cbSoloPart.Items.Add("Part 6");
            cbSoloPart.Items.Add("Part 7");
            cbSoloPart.Items.Add("Part 8");
            cbSoloPart.Items.Add("Part 9");
            cbSoloPart.Items.Add("Part 10");
            cbSoloPart.Items.Add("Part 11");
            cbSoloPart.Items.Add("Part 12");
            cbSoloPart.Items.Add("Part 13");
            cbSoloPart.Items.Add("Part 14");
            cbSoloPart.Items.Add("Part 15");
            cbSoloPart.Items.Add("Part 16");
        }

        private void PopulateDrumCompEQPart()
        {
            cbDrumCompEQPart.Items.Add("Part 1");
            cbDrumCompEQPart.Items.Add("Part 2");
            cbDrumCompEQPart.Items.Add("Part 3");
            cbDrumCompEQPart.Items.Add("Part 4");
            cbDrumCompEQPart.Items.Add("Part 5");
            cbDrumCompEQPart.Items.Add("Part 6");
            cbDrumCompEQPart.Items.Add("Part 7");
            cbDrumCompEQPart.Items.Add("Part 8");
            cbDrumCompEQPart.Items.Add("Part 9");
            cbDrumCompEQPart.Items.Add("Part 10");
            cbDrumCompEQPart.Items.Add("Part 11");
            cbDrumCompEQPart.Items.Add("Part 12");
            cbDrumCompEQPart.Items.Add("Part 13");
            cbDrumCompEQPart.Items.Add("Part 14");
            cbDrumCompEQPart.Items.Add("Part 15");
            cbDrumCompEQPart.Items.Add("Part 16");
        }

        private void populateOutputAssign(ComboBox outputAssignBox)
        {
            outputAssignBox.Items.Add("Part");
            outputAssignBox.Items.Add("A");
            outputAssignBox.Items.Add("B");
            outputAssignBox.Items.Add("C");
            outputAssignBox.Items.Add("D");
            outputAssignBox.Items.Add("1");
            outputAssignBox.Items.Add("2");
            outputAssignBox.Items.Add("3");
            outputAssignBox.Items.Add("4");
            outputAssignBox.Items.Add("5");
            outputAssignBox.Items.Add("6");
            outputAssignBox.Items.Add("7");
            outputAssignBox.Items.Add("8");
        }

        private void populateColumn2Selector()
        {
            cbColumn1Selector.Items.Add("System common settings");
            cbColumn1Selector.Items.Add("Voice reserve");
            cbColumn1Selector.Items.Add("Chorus");
            cbColumn1Selector.Items.Add("Reverb");
            cbColumn1Selector.Items.Add("Motional Surround");
            cbColumn1Selector.Items.Add("Master EQ");
        }

        private void populateStudioSetControlChannel(ComboBox StudioSetControlChannel)
        {
            StudioSetControlChannel.Items.Add("Channel 1");
            StudioSetControlChannel.Items.Add("Channel 2");
            StudioSetControlChannel.Items.Add("Channel 3");
            StudioSetControlChannel.Items.Add("Channel 4");
            StudioSetControlChannel.Items.Add("Channel 5");
            StudioSetControlChannel.Items.Add("Channel 6");
            StudioSetControlChannel.Items.Add("Channel 7");
            StudioSetControlChannel.Items.Add("Channel 8");
            StudioSetControlChannel.Items.Add("Channel 9");
            StudioSetControlChannel.Items.Add("Channel 10");
            StudioSetControlChannel.Items.Add("Channel 11");
            StudioSetControlChannel.Items.Add("Channel 12");
            StudioSetControlChannel.Items.Add("Channel 13");
            StudioSetControlChannel.Items.Add("Channel 14");
            StudioSetControlChannel.Items.Add("Channel 15");
            StudioSetControlChannel.Items.Add("Channel 16");
            StudioSetControlChannel.Items.Add("Off");
        }

        private void PopulateSystemSource(ComboBox cbSystemCommon)
        {
            cbSystemCommon.Items.Add("System");
            cbSystemCommon.Items.Add("Studio set");
        }

        private void PopulateSystemCommonClockSource()
        {
            cbSystemCommonSystemClockSource.Items.Add("MIDI");
            cbSystemCommonSystemClockSource.Items.Add("USB");
        }

        private void SystemCommonStereoOutputMode()
        {
            cbSystemCommonStereoOutputMode.Items.Add("Speaker");
            cbSystemCommonStereoOutputMode.Items.Add("Headphones");
        }

        private void StudioSetChorusType()
        {
            cbStudioSetChorusType.Items.Add("Off");
            cbStudioSetChorusType.Items.Add("Chorus");
            cbStudioSetChorusType.Items.Add("Delay");
            cbStudioSetChorusType.Items.Add("GM2 Chorus");
        }

        private void StudioSetOutputAssign(ComboBox comboBox)
        {
            comboBox.Items.Add("A");
            comboBox.Items.Add("B");
            comboBox.Items.Add("C");
            comboBox.Items.Add("D");
        }

        private void ChorusOutputSelect()
        {
            cbChorusOutputSelect.Items.Add("Main");
            cbChorusOutputSelect.Items.Add("Reverb");
            cbChorusOutputSelect.Items.Add("Main + reverb");
        }

        private void ChorusChorusFilterType()
        {
            cbChorusChorusFilterType.Items.Add("Off");
            cbChorusChorusFilterType.Items.Add("Low pass filter");
            cbChorusChorusFilterType.Items.Add("High pass filter");
        }

        private void StudioSetFreq(ComboBox comboBox)
        {
            comboBox.Items.Add("200");
            comboBox.Items.Add("250");
            comboBox.Items.Add("315");
            comboBox.Items.Add("400");
            comboBox.Items.Add("500");
            comboBox.Items.Add("630");
            comboBox.Items.Add("800");
            comboBox.Items.Add("1000");
            comboBox.Items.Add("1250");
            comboBox.Items.Add("1600");
            comboBox.Items.Add("2000");
            comboBox.Items.Add("2500");
            comboBox.Items.Add("3150");
            comboBox.Items.Add("4000");
            comboBox.Items.Add("5000");
            comboBox.Items.Add("6300");
            comboBox.Items.Add("8000");
        }

        private void ChorusChorusRateHzNote()
        {
            cbChorusChorusRateHzNote.Items.Add("Hz");
            cbChorusChorusRateHzNote.Items.Add("Note");
        }

        private void StudioSetMsNote(ComboBox comboBox)
        {
            comboBox.Items.Add("Milliseconds");
            comboBox.Items.Add("Note");
        }

        private void StudioSetReverbType()
        {
            cbStudioSetReverbType.Items.Add("Off");
            cbStudioSetReverbType.Items.Add("Room 1");
            cbStudioSetReverbType.Items.Add("Room 2");
            cbStudioSetReverbType.Items.Add("Hall 1");
            cbStudioSetReverbType.Items.Add("Hall 2");
            cbStudioSetReverbType.Items.Add("RPlate");
            cbStudioSetReverbType.Items.Add("GM2 reverb");
        }

        private void StudioSetMotionalSurroundRoomType()
        {
            cbStudioSetMotionalSurroundRoomType.Items.Add("Room1");
            cbStudioSetMotionalSurroundRoomType.Items.Add("Room2");
            cbStudioSetMotionalSurroundRoomType.Items.Add("Hall1");
            cbStudioSetMotionalSurroundRoomType.Items.Add("Hall2");
        }

        private void StudioSetMotionalSurroundRoomSize()
        {
            cbStudioSetMotionalSurroundRoomSize.Items.Add("Small");
            cbStudioSetMotionalSurroundRoomSize.Items.Add("Medium");
            cbStudioSetMotionalSurroundRoomSize.Items.Add("Large");
        }

        private void StudioSetMasterEqLowFreq()
        {
            cbStudioSetMasterEqLowFreq.Items.Add("200 Hz");
            cbStudioSetMasterEqLowFreq.Items.Add("400 Hz");
        }

        private void StudioSetMasterEqMidQ()
        {
            cbStudioSetMasterEqMidQ.Items.Add("0.5");
            cbStudioSetMasterEqMidQ.Items.Add("1.0");
            cbStudioSetMasterEqMidQ.Items.Add("2.0");
            cbStudioSetMasterEqMidQ.Items.Add("4.0");
            cbStudioSetMasterEqMidQ.Items.Add("8.0");
        }

        private void StudioSetMasterEqHighFreq()
        {
            cbStudioSetMasterEqHighFreq.Items.Add("2000 Hz");
            cbStudioSetMasterEqHighFreq.Items.Add("4000 Hz");
            cbStudioSetMasterEqHighFreq.Items.Add("8000 Hz");
        }

        private void StudioSetPartSelector()
        {
            cbStudioSetPartSelector.Items.Add("Studio set part 1 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 2 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 3 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 4 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 5 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 6 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 7 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 8 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 9 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 10 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 11 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 12 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 13 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 14 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 15 settings");
            cbStudioSetPartSelector.Items.Add("Studio set part 16 settings");
            cbStudioSetPartSelector.SelectedIndex = 0;
        }

        private void StudioSetPartSubSelector()
        {
            cbStudioSetPartSubSelector.Items.Add("Part settings page 1");
            cbStudioSetPartSubSelector.Items.Add("Part settings page 2");
            cbStudioSetPartSubSelector.Items.Add("Keyboard parameters");
            cbStudioSetPartSubSelector.Items.Add("Scale tune parameters");
            cbStudioSetPartSubSelector.Items.Add("Midi");
            cbStudioSetPartSubSelector.Items.Add("Motional surround");
            cbStudioSetPartSubSelector.Items.Add("Studio Set Part EQ");
            cbStudioSetPartSubSelector.SelectedIndex = 0;
        }

        private void StudioSetPartSettings1ReceiveChannel()
        {
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 1");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 2");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 3");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 4");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 5");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 6");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 7");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 8");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 9");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 10");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 11");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 12");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 13");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 14");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 15");
            cbStudioSetPartSettings1ReceiveChannel.Items.Add("Channel 16");
        }

        private void StudioSetPartSettings1MonoPoly()
        {
            cbStudioSetPartSettings1MonoPoly.Items.Add("Mono");
            cbStudioSetPartSettings1MonoPoly.Items.Add("Poly");
            cbStudioSetPartSettings1MonoPoly.Items.Add("Tone default");
        }

        private void StudioSetPartSettings1Legato()
        {
            cbStudioSetPartSettings1Legato.Items.Add("Off");
            cbStudioSetPartSettings1Legato.Items.Add("On");
            cbStudioSetPartSettings1Legato.Items.Add("Tone default");
        }

        private void StudioSetPartSettings1Portamento()
        {
            cbStudioSetPartSettings1Portamento.Items.Add("Off");
            cbStudioSetPartSettings1Portamento.Items.Add("On");
            cbStudioSetPartSettings1Portamento.Items.Add("Tone default");
        }

        private void StudioSetPartEffectsOutputAssign()
        {
            cbStudioSetPartEffectsOutputAssign.Items.Add("A");
            cbStudioSetPartEffectsOutputAssign.Items.Add("B");
            cbStudioSetPartEffectsOutputAssign.Items.Add("C");
            cbStudioSetPartEffectsOutputAssign.Items.Add("D");
            cbStudioSetPartEffectsOutputAssign.Items.Add("1");
            cbStudioSetPartEffectsOutputAssign.Items.Add("2");
            cbStudioSetPartEffectsOutputAssign.Items.Add("3");
            cbStudioSetPartEffectsOutputAssign.Items.Add("4");
            cbStudioSetPartEffectsOutputAssign.Items.Add("5");
            cbStudioSetPartEffectsOutputAssign.Items.Add("6");
            cbStudioSetPartEffectsOutputAssign.Items.Add("7");
            cbStudioSetPartEffectsOutputAssign.Items.Add("8");
        }

        private void StudioSetPartKeyboardVelocityCurveType()
        {
            cbStudioSetPartKeyboardVelocityCurveType.Items.Add("Off");
            cbStudioSetPartKeyboardVelocityCurveType.Items.Add("Type 1");
            cbStudioSetPartKeyboardVelocityCurveType.Items.Add("Type 2");
            cbStudioSetPartKeyboardVelocityCurveType.Items.Add("Type 3");
            cbStudioSetPartKeyboardVelocityCurveType.Items.Add("Type 4");
        }

        private void StudioSetPartScaleTuneType()
        {
            cbStudioSetPartScaleTuneType.Items.Add("Custom");
            cbStudioSetPartScaleTuneType.Items.Add("Equal");
            cbStudioSetPartScaleTuneType.Items.Add("Just-maj");
            cbStudioSetPartScaleTuneType.Items.Add("Just-min");
            cbStudioSetPartScaleTuneType.Items.Add("Pythagore");
            cbStudioSetPartScaleTuneType.Items.Add("Kirnberge");
            cbStudioSetPartScaleTuneType.Items.Add("Meantone");
            cbStudioSetPartScaleTuneType.Items.Add("Werckmeis");
            cbStudioSetPartScaleTuneType.Items.Add("Arabic");
        }

        private void StudioSetPartScaleTuneKey()
        {
            cbStudioSetPartScaleTuneKey.Items.Add("C");
            cbStudioSetPartScaleTuneKey.Items.Add("C#");
            cbStudioSetPartScaleTuneKey.Items.Add("D");
            cbStudioSetPartScaleTuneKey.Items.Add("D#");
            cbStudioSetPartScaleTuneKey.Items.Add("E");
            cbStudioSetPartScaleTuneKey.Items.Add("F");
            cbStudioSetPartScaleTuneKey.Items.Add("F#");
            cbStudioSetPartScaleTuneKey.Items.Add("G");
            cbStudioSetPartScaleTuneKey.Items.Add("G#");
            cbStudioSetPartScaleTuneKey.Items.Add("A");
            cbStudioSetPartScaleTuneKey.Items.Add("A#");
            cbStudioSetPartScaleTuneKey.Items.Add("B");
        }

        private void StudioSetPartEQLoqFreq()
        {
            cbStudioSetPartEQLowFreq.Items.Add("200 Hz");
            cbStudioSetPartEQLowFreq.Items.Add("400 Hz");
        }

        private void StudioSetPartEQMidQ()
        {
            cbStudioSetPartEQMidQ.Items.Add("0.5");
            cbStudioSetPartEQMidQ.Items.Add("1.0");
            cbStudioSetPartEQMidQ.Items.Add("2.0");
            cbStudioSetPartEQMidQ.Items.Add("4.0");
        }

        private void StudioSetPartEQHighFreq()
        {
            cbStudioSetPartEQHighFreq.Items.Add("2000 Hz");
            cbStudioSetPartEQHighFreq.Items.Add("4000 Hz");
            cbStudioSetPartEQHighFreq.Items.Add("8000 Hz");
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Communication handlers
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void EditStudioSet_Timer_Tick()
        {
            if (setVisibility)
            {
                setVisibility = false;

                gEditStudioSetSearchResult.IsVisible = false;
                StudioSetPartSettings1.IsVisible = true;
                StudioSetPartSettings2.IsVisible = false;
                StudioSetPartKeyboard.IsVisible = false;
                StudioSetPartScaleTune.IsVisible = false;
                StudioSetPartMidi.IsVisible = false;
                imgVelocityCurve1On.IsVisible = false;
                imgVelocityCurve1Off.IsVisible = true;
                imgVelocityCurve2On.IsVisible = false;
                imgVelocityCurve2Off.IsVisible = true;
                imgVelocityCurve3On.IsVisible = false;
                imgVelocityCurve3Off.IsVisible = true;
                imgVelocityCurve4On.IsVisible = false;
                imgVelocityCurve4Off.IsVisible = true;
                StudioSetPartMotionalSurround.IsVisible = false;
                StudioSetPartEQ.IsVisible = false;
                grid_Buttons.IsVisible = true;
                VoiceReserve.IsVisible = false;
                Chorus.IsVisible = false;
                ChorusChorus.IsVisible = false;
                Reverb.IsVisible = false;
                StudioSetMotionalSurround.IsVisible = false;
                StudioSetMasterEQ.IsVisible = false;
                gEditStudioSetSearchResult.IsVisible = false;
                StudioSetPartSettings1.IsVisible = true;
                StudioSetPartSettings2.IsVisible = false;
                StudioSetPartEffects.IsVisible = false;
                StudioSetPartKeyboard.IsVisible = false;
                StudioSetPartScaleTune.IsVisible = false;
                StudioSetPartMidi.IsVisible = false;
                StudioSetPartMotionalSurround.IsVisible = false;
                StudioSetPartEQ.IsVisible = false;
                grid_Buttons.IsVisible = true;
                StudioSetEditor_StackLayout.IsVisible = true;
            }

            if (studioSetEditor_State != StudioSetEditor_State.DONE && studioSetEditor_State != StudioSetEditor_State.NONE)
                // && waitingForResponseFromIntegra7 > 0)
            {
                t.Trace("private void Timer_Tick waiting for response");
                waitingForResponseFromIntegra7--;
                if (waitingForResponseFromIntegra7 < 1)
                {
                    // There was a timeout. Handle it:
                    //timer.Stop();
                    Waiting(false, "", StudioSetEditor_StackLayout);
                    if (studioSetEditor_State == StudioSetEditor_State.INIT)
                    {
                        //commonState.midi.midiInPort.MessageReceived -= EditStudioSet_MidiInPort_MessageReceived;
                        //this.Frame.Navigate(typeof(CommunicationError), commonState);
                    }
                }
            }

            if (!initDone)
            {
                if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest ==
                    StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_TITLES)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_TITLES");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // All studio set names has been received and stored in studioSetNames,
                    // populate the combobox:
                    PopulateStudioSetSelector();

                    // Set studio set back according to currentStudioSetNumber:
                    //commonState.midi.ProgramChange((byte)15, (byte)85, (byte)0, (byte)(currentStudioSetNumberAsBytes[2] + 1));
                    commonState.Midi.ProgramChange((byte)15, (byte)85, (byte)0, (byte)(commonState.CurrentStudioSet + 1));

                    // Set selector accordingly:
                    PushHandleControlEvents();
                    cbStudioSetSelector.SelectedIndex = commonState.CurrentStudioSet;// currentStudioSetNumberAsBytes[2] + currentStudioSetNumberAsBytes[1] * 128;
                    PopHandleControlEvents();

                    // Ask for system common settings:
                    QuerySystemCommon(); // This will be caught in MidiInPort_MessageReceived
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest ==
                    StudioSetEditor_currentStudioSetEditorMidiRequest.SYSTEM_COMMON)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == " +
                        "StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == " +
                        "StudioSetEditor_currentStudioSetEditorMidiRequest.SYSTEM_COMMON");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack system common settings:
                    ReadSystemCommon();

                    // Ask for current studio set common:
                    QueryStudioSetCommon(); // This will be caught in MidiInPort_MessageReceived

                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest ==
                    StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_COMMON)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == " +
                        "StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == " +
                        "StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_COMMON");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack studio set common and update controls:
                    ReadSelectedStudioSet();

                    // Ask for studio set chorus:
                    QueryStudioSetChorus();
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest ==
                    StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == " +
                        "StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == " +
                        "StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack studio set chours and update controls:
                    ReadStudioSetChorus();

                    // Ask for studio set reverb:
                    QueryStudioSetReverb();
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest ==
                    StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack studio set reverb and update controls:
                    ReadStudioSetReverb();

                    // Ask for studio set motional surround:
                    QueryStudioSetMotionalSurround();
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MOTIONAL_SURROUND)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MOTIONAL_SURROUND");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack studio set reverb and update controls:
                    ReadMotionalSurround();

                    // Ask for studio set reverb:
                    QueryStudioSetMasterEQ();
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest ==
                    StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MASTER_EQ)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MASTER_EQ");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack studio set master equalizer:
                    ReadStudioSetMasterEQ();

                    // Ask for studio set part:
                    cbStudioSetPartSelector.SelectedIndex = commonState.CurrentPart;
                    studioSetEditor_PartToRead = 0;
                    QueryStudioSetPart(studioSetEditor_PartToRead);
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest ==
                    StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack studio set part:
                    ReadStudioSetPart(studioSetEditor_PartToRead);
                    studioSetEditor_PartToRead++;
                    if (studioSetEditor_PartToRead < 16)
                    {
                        QueryStudioSetPart(studioSetEditor_PartToRead);
                    }
                    else
                    {
                        // Ask for part tone name:
                        QueryStudioSetPartToneName();
                    }
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest ==
                    StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_TONE_NAME)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == " +
                        "StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == " +
                        "StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_TONE_NAME");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack tone name and update controls:
                    ReadStudioSetPartToneName();

                    // Get current tone:
                    commonState.CurrentTone = new Tone(commonState.ToneList.Tones[commonState.ToneList.Get(
                        commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectMSB,
                        commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB,
                        commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneProgramNumber)]);
                    StudioSetCurrentToneName.Text = commonState.CurrentTone.Name;

                    // Get the current tone from commonState:
                    currentToneNumberAsBytes = new byte[3];
                    currentToneNumberAsBytes[0] = (byte)(UInt16.Parse(commonState.ToneList.Tones[commonState.CurrentTone.Index][4]));
                    currentToneNumberAsBytes[1] = (byte)(UInt16.Parse(commonState.ToneList.Tones[commonState.CurrentTone.Index][5]));
                    currentToneNumberAsBytes[2] = (byte)(UInt16.Parse(commonState.ToneList.Tones[commonState.CurrentTone.Index][7]));

                    // Now we have MSB, LSB and CB, set comboboxes:
                    PushHandleControlEvents();
                    PopulateCbStudioSetPartSettings1Group();
                    cbStudioSetPartSettings1Group.SelectedItem = commonState.CurrentTone.Group;
                    PopulateCbStudioSetPartSettings1Category();
                    cbStudioSetPartSettings1Category.SelectedItem = commonState.CurrentTone.Category;
                    PopulateCbStudioSetPartSettings1Program();
                    cbStudioSetPartSettings1Program.SelectedItem = commonState.CurrentTone.Name;
                    PopHandleControlEvents();

                    // Set current tone on I-7:
                    //commonState.midi.ProgramChange((byte)cbStudioSetPartSettings1ReceiveChannel.SelectedIndex, currentToneNumberAsBytes[0], currentToneNumberAsBytes[1], currentToneNumberAsBytes[2]);

                    // Ask for studio set midi phaselock:
                    QueryStudioSetPartMidiPhaselock();
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_MIDI_PHASELOCK)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == " +
                        "StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == " +
                        "StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_MIDI_PHASELOCK");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack studio set midi phaselock:
                    ReadStudioSetPartMidiPhaseLock();

                    // Ask for studio set part eq
                    studioSetEditor_PartToRead = 0;
                    QueryStudioSetPartEQ(studioSetEditor_PartToRead);
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest ==
                    StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_EQ)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == " +
                        "StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == " +
                        "StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_EQ");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack studio set part equalizer:
                    ReadStudioSetPartEQ(studioSetEditor_PartToRead);

                    studioSetEditor_PartToRead++;
                    if (studioSetEditor_PartToRead < 16)
                    {
                        QueryStudioSetPartEQ(studioSetEditor_PartToRead);
                    }
                    else
                    {
                        currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.NONE;
                        studioSetEditor_State = StudioSetEditor_State.INIT_DONE;
                        PopHandleControlEvents();
                    }

                    // Restore studio set selector selection:
                    //commonState.CurrentStudioSet = initialStudioSetNumber;
                    cbStudioSetSelector.SelectedIndex = commonState.CurrentStudioSet; //currentStudioSetNumberAsBytes[2];
                    //SetStudioSet(commonState.CurrentStudioSet);
                }
                // After initialization:
                else if (studioSetEditor_State == StudioSetEditor_State.INIT_DONE && currentStudioSetEditorMidiRequest ==
                    StudioSetEditor_currentStudioSetEditorMidiRequest.NONE)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == " +
                        "StudioSetEditor_State.INIT_DONE && currentStudioSetEditorMidiRequest == " +
                        "StudioSetEditor_currentStudioSetEditorMidiRequest.NONE");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    if (!initialGuiDone)
                    {
                        // Any initiations that requires all initial MIDI communications to be done goes here:
                        // ...
                        //Progress.IsVisible = false;
                        SystemCommonSettings.IsVisible = true;
                        VoiceReserve.IsVisible = false;
                        Chorus.IsVisible = false;
                        ChorusChorus.IsVisible = false;
                        ChorusDelay.IsVisible = false;
                        ChorusGM2Chorus.IsVisible = false;
                        switch (commonState.StudioSet.CommonChorus.Type)
                        {
                            case 1:
                                ChorusChorus.IsVisible = true;
                                break;
                            case 2:
                                ChorusDelay.IsVisible = true;
                                break;
                            case 3:
                                ChorusGM2Chorus.IsVisible = true;
                                break;
                        }
                        //grid_PleaseWaitWhileScanning.IsVisible = false;
                        //grid_MainStudioSet.IsVisible = true;
                        cbColumn1Selector.SelectedIndex = 0;

                        // All Studio Set Names was previously read in, so just copy them to the selector:
                        PopulateStudioSetSelector();

                        // Set Studio Set selector accordingly:
                        PushHandleControlEvents();
                        if (commonState != null && commonState.CurrentStudioSet < 64)
                        {
                            cbStudioSetSelector.SelectedIndex = commonState.CurrentStudioSet;
                        }
                        PopHandleControlEvents();
                        initialGuiDone = true;
                    }
                    initDone = true;
                    Waiting(false, "", StudioSetEditor_StackLayout);
                    //PopHandleControlEvents();
                    handleControlEvents = true;
                    studioSetEditor_State = StudioSetEditor_State.DONE;
                }
            }
            else
            {
                // Responses to user changing selectors _after_ initiation is done
                if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.SYSTEM_COMMON)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.SYSTEM_COMMON");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack system common settings:
                    ReadSystemCommon();

                    // Ask for current studio set common:
                    QueryStudioSetCommon(); // This will be caught in MidiInPort_MessageReceived

                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_COMMON)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_COMMON");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack studio set common and update controls:
                    ReadSelectedStudioSet();

                    // Ask for studio set chorus:
                    QueryStudioSetChorus();
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS)
                    //&& currentStudioSetEditorMidiRequest <= StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_GM2_CHORUS)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest >= " +
                        "StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS && currentStudioSetEditorMidiRequest <= StudioSetEditor_currentStudioSetEditorMidiRequest." +
                        "STUDIO_SET_CHORUS_GM2_CHORUS");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    ReadStudioSetChorus();
                    UpdateChorusChorusSubwindow();
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.NONE;
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_OFF)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest >= " +
                        "StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS && currentStudioSetEditorMidiRequest <= StudioSetEditor_currentStudioSetEditorMidiRequest." +
                        "STUDIO_SET_CHORUS_GM2_CHORUS");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;
                    UpdateStudioSetChorusOffControls();
                    UpdateChorusChorusSubwindow();
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.NONE;
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_CHORUS)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest >= " +
                        "StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS && currentStudioSetEditorMidiRequest <= StudioSetEditor_currentStudioSetEditorMidiRequest." +
                        "STUDIO_SET_CHORUS_GM2_CHORUS");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;
                    UpdateStudioSetChorusChorusControls();
                    UpdateChorusChorusSubwindow();
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.NONE;
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_DELAY)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest >= " +
                        "StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS && currentStudioSetEditorMidiRequest <= StudioSetEditor_currentStudioSetEditorMidiRequest." +
                        "STUDIO_SET_CHORUS_GM2_CHORUS");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;
                    UpdateStudioSetChorusDelayControls();
                    UpdateChorusChorusSubwindow();
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.NONE;
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_GM2_CHORUS)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest >= " +
                        "StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS && currentStudioSetEditorMidiRequest <= StudioSetEditor_currentStudioSetEditorMidiRequest." +
                        "STUDIO_SET_CHORUS_GM2_CHORUS");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;
                    UpdateStudioSetChorusGM2ChorusControls();
                    UpdateChorusChorusSubwindow();
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.NONE;
                }

                // These happens if another part is selected in 3:rd column:
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest >= StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB
                    && currentStudioSetEditorMidiRequest <= StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_GM2_REVERB)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest >= StudioSetEditor_currentStudioSetEditorMidiRequest." +
                        "STUDIO_SET_REVERB_ROOM_HALL_PLATE && currentStudioSetEditorMidiRequest <= StudioSetEditor_currentStudioSetEditorMidiRequest." +
                        "STUDIO_SET_REVERB_GM2_REVERB");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    UpdateChorusReverbSubwindow();
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.NONE;
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack studio set part and update controls:
                    ReadStudioSetPart();

                    // Ask for part tone name:
                    QueryStudioSetPartToneName();
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_TONE_NAME)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_TONE_NAME");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack tone name and update controls:
                    ReadStudioSetPartToneName();

                    // Get current tone:
                    StudioSetCurrentToneName.Text = commonState.CurrentTone.Name;

                    Int32 toneNumber = commonState.ToneList.Get(commonState.CurrentTone.BankMSB, commonState.CurrentTone.BankLSB, commonState.CurrentTone.Program);

                    // Now we have MSB, LSB and CB, fix comboboxes:
                    PushHandleControlEvents();
                    PopulateCbStudioSetPartSettings1Group();
                    cbStudioSetPartSettings1Group.SelectedItem = commonState.ToneList.Tones[toneNumber][0];// commonState.currentTone.Group;
                    PopulateCbStudioSetPartSettings1Category();
                    cbStudioSetPartSettings1Category.SelectedItem = commonState.ToneList.Tones[toneNumber][1];//commonState.currentTone.Category;
                    PopulateCbStudioSetPartSettings1Program();
                    cbStudioSetPartSettings1Program.SelectedItem = commonState.ToneList.Tones[toneNumber][3];//commonState.currentTone.Name;
                    PopHandleControlEvents();

                    // Set current tone on I-7:
                    //commonState.midi.ProgramChange((byte)cbStudioSetPartSettings1ReceiveChannel.SelectedIndex,
                    //    currentToneNumberAsBytes[0], currentToneNumberAsBytes[1], currentToneNumberAsBytes[2]);

                    // Ask for studio set midi phaselock:
                    QueryStudioSetPartMidiPhaselock();
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_MIDI_PHASELOCK)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_MIDI_PHASELOCK");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack studio set phase lock and update controls:
                    ReadStudioSetPartMidiPhaseLock();

                    // Ask for studio set part eq
                    QueryStudioSetPartEQ();
                }
                else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_EQ)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_EQ");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    // Unpack studio set part equalizer and update controls:
                    ReadStudioSetPartEQ();

                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.NONE;
                    studioSetEditor_State = StudioSetEditor_State.INIT_DONE;
                    PopHandleControlEvents();
                }
                else if (studioSetEditor_State == StudioSetEditor_State.INIT_DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.NONE)
                {
                    t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.INIT_DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.NONE");
                    // This will be handled, so...
                    studioSetEditor_State = StudioSetEditor_State.NONE;

                    switch (commonState.StudioSet.CommonChorus.Type)
                    {
                        case 1:
                            ChorusChorus.IsVisible = true;
                            break;
                        case 2:
                            ChorusDelay.IsVisible = true;
                            break;
                        case 3:
                            ChorusGM2Chorus.IsVisible = true;
                            break;
                    }
                }
            }
        }
        private void EditStudioSet_MidiInPort_MessageReceived(/*Windows.Devices.Midi.MidiInPort sender, Windows.Devices.Midi.MidiMessageReceivedEventArgs args*/)
        {
            t.Trace("private void EditStudioSet_MidiInPort_MessageReceived (" + "Windows.Devices.Midi.MidiInPort + sender + , Windows.Devices.Midi.MidiMessageReceivedEventArgs + args + , " + ")");
            {
                if ((currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER
                    || currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER_AND_SCAN)
                    && studioSetEditor_State == StudioSetEditor_State.QUERYING_CURRENT_STUDIO_SET_NUMBER)
                {
                    commonState.CurrentStudioSet = rawData[13];
                    studioSetNumberTemp = 0;

                    if (currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER_AND_SCAN)
                    {
                        currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_TITLES;
                        studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_NAMES;
                        ScanForStudioSetNames();
                    }
                    else
                    {
                        // Ask for system common settings:
                        //QuerySystemCommon(); // This will be caught in MidiInPort_MessageReceived
                        currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_TITLES;
                        studioSetEditor_State = StudioSetEditor_State.DONE;
                        //currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.NONE;
                    }
                }
                else if (currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_TITLES)// && receivedMidiMessage.Type == MidiMessageType.SystemExclusive)
                {
                    // We have asked for the first/next studio set:
                    String text = "";
                    for (Int32 i = 0x0b; i < rawData.Length - 2; i++)
                    {
                        text += (char)rawData[i];
                    }
                    commonState.StudioSetNames.Add(text);
                    studioSetNumberTemp++;
                    pb_WaitingProgress.Progress = (Double)studioSetNumberTemp / (Double) 64;

                    // Query next studio set if this was not last one:
                    if (studioSetNumberTemp < 64)
                    {
                        // Ask for it:
                        ScanForStudioSetNames(); // Answer will be caught here.
                    }
                    else
                    {
                        // All titles received, set a status that will be caught in Timer_Tick:
                        studioSetEditor_State = StudioSetEditor_State.DONE;

                        // PleaseWait has had the control so far, so take it and let Studio set editor be visible.
                        currentPage = CurrentPage.EDIT_STUDIO_SET;
                        PleaseWait_StackLayout.IsVisible = false;
                        StudioSetEditor_StackLayout.IsVisible = true;
                    }
                }
                else
                {
                    // Tell Timer_Tick that System Common has been read:
                    studioSetEditor_State = StudioSetEditor_State.DONE; // This will be caught in Timer_Tick
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Event handlers
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Column 0
        private void cbStudioSetSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (initDone)
            {
                t.Trace("private void cbStudioSetSelector_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
                if (handleControlEvents && studioSetEditor_State != StudioSetEditor_State.INIT && cbStudioSetSelector.SelectedIndex > -1)
                {
                    commonState.CurrentStudioSet = (byte)cbStudioSetSelector.SelectedIndex;
                    SetStudioSet(commonState.CurrentStudioSet);

                    // Get the values:
                    initDone = false;
                    QueryStudioSetCommon();
                }
            }
        }

        private void SetStudioSetCommon()
        {
        }

        private void cbToneControl1_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbToneControl1_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                if ((String)cbToneControl1.SelectedItem == "N/A")
                {
                    cbToneControl1.SelectedIndex++;
                }
                SetStudioSetCommonToneControl1(cbToneControl1.SelectedIndex);
            }
        }

        private void SetStudioSetCommonToneControl1(Int32 p)
        {
            commonState.StudioSet.Common.ToneControlSource[0] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x39 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbToneControl2_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbToneControl2_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                if ((String)cbToneControl2.SelectedItem == "N/A")
                {
                    cbToneControl2.SelectedIndex++;
                }
                SetStudioSetCommonToneControl2(cbToneControl2.SelectedIndex);
            }
        }

        private void SetStudioSetCommonToneControl2(Int32 p)
        {
            commonState.StudioSet.Common.ToneControlSource[1] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x3a };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbToneControl3_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbToneControl3_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                if ((String)cbToneControl3.SelectedItem == "N/A")
                {
                    cbToneControl3.SelectedIndex++;
                }
                SetStudioSetCommonToneControl3(cbToneControl3.SelectedIndex);
            }
        }

        private void SetStudioSetCommonToneControl3(Int32 p)
        {
            commonState.StudioSet.Common.ToneControlSource[2] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x3b };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbToneControl4_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbToneControl4_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                if ((String)cbToneControl4.SelectedItem == "N/A")
                {
                    cbToneControl4.SelectedIndex++;
                }
                SetStudioSetCommonToneControl4(cbToneControl4.SelectedIndex);
            }
        }

        private void SetStudioSetCommonToneControl4(Int32 p)
        {
            commonState.StudioSet.Common.ToneControlSource[3] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x3c };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slTempo_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slTempo_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCommonTempo(slTempo.Value);
            }
        }

        private void SetStudioSetCommonTempo(Double p)
        {
            commonState.StudioSet.Common.Tempo = (byte)p;
            tbTempo.Text = "Tempo " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x00, 0x3d };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, Int32ToTwoByteArray((byte)p));
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSoloPart_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbSoloPart_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCommonSoloPart(cbSoloPart.SelectedIndex);
            }
        }

        private void SetStudioSetCommonSoloPart(Int32 p)
        {
            commonState.StudioSet.Common.SoloPart = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x3f };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbReverb_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbReverb_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCommonReverb((Boolean)cbReverb.IsChecked);
            }
        }

        private void SetStudioSetCommonReverb(Boolean p)
        {
            commonState.StudioSet.Common.Reverb = p;
            byte[] address = { 0x18, 0x00, 0x00, 0x40 };
            byte[] value = { (byte)((p) ? 0x01 : 0x00) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbChorus_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbChorus_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCommonChorus((Boolean)cbChorus.IsChecked);
            }
        }

        private void SetStudioSetCommonChorus(Boolean p)
        {
            commonState.StudioSet.Common.Chorus = p;
            byte[] address = { 0x18, 0x00, 0x00, 0x41 };
            byte[] value = { (byte)(p ? 0x01 : 0x00) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbMasterEQ_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbMasterEQ_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCommonMasterEQ((Boolean)cbMasterEQ.IsChecked);
            }
        }

        private void SetStudioSetCommonMasterEQ(Boolean p)
        {
            commonState.StudioSet.Common.MasterEqualizer = p;
            byte[] address = { 0x18, 0x00, 0x00, 0x42 };
            byte[] value = { (byte)(p ? 0x01 : 0x00) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbDrumCompEQPart_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbDrumCompEQPart_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCommonEQPart(cbDrumCompEQPart.SelectedIndex);
            }
        }

        private void SetStudioSetCommonEQPart(Int32 p)
        {
            commonState.StudioSet.Common.DrumCompressorAndEqualizerPart = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x44 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbDrumCompEQ1OutputAssign_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbDrumCompEQ1OutputAssign_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCommonEQ1OutputAssign(cbDrumCompEQ1OutputAssign.SelectedIndex);
            }
        }

        private void SetStudioSetCommonEQ1OutputAssign(Int32 p)
        {
            commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[0] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x45 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbDrumCompEQ2OutputAssign_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbDrumCompEQ2OutputAssign_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCommonEQ2OutputAssign(cbDrumCompEQ2OutputAssign.SelectedIndex);
            }
        }

        private void SetStudioSetCommonEQ2OutputAssign(Int32 p)
        {
            commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[1] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x46 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbDrumCompEQ3OutputAssign_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbDrumCompEQ3OutputAssign_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCommonEQ3OutputAssign(cbDrumCompEQ3OutputAssign.SelectedIndex);
            }
        }

        private void SetStudioSetCommonEQ3OutputAssign(Int32 p)
        {
            commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[2] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x47 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbDrumCompEQ4OutputAssign_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbDrumCompEQ4OutputAssign_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCommonEQ4OutputAssign(cbDrumCompEQ4OutputAssign.SelectedIndex);
            }
        }

        private void SetStudioSetCommonEQ4OutputAssign(Int32 p)
        {
            commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[3] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x48 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbDrumCompEQ5OutputAssign_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbDrumCompEQ5OutputAssign_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCommonEQ5OutputAssign(cbDrumCompEQ5OutputAssign.SelectedIndex);
            }
        }

        private void SetStudioSetCommonEQ5OutputAssign(Int32 p)
        {
            commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[4] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x49 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbDrumCompEQ6OutputAssign_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbDrumCompEQ6OutputAssign_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCommonEQ6OutputAssign(cbDrumCompEQ6OutputAssign.SelectedIndex);
            }
        }

        private void SetStudioSetCommonEQ6OutputAssign(Int32 p)
        {
            commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[5] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x4a };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbDrumCompEQ_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbDrumCompEQ_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetCompEQ_Click((Boolean)cbDrumCompEQ.IsChecked);
            }
        }

        private void SetStudioSetCompEQ_Click(Boolean p)
        {
            commonState.StudioSet.Common.DrumCompressorAndEqualizer = p;
            byte[] address = { 0x18, 0x00, 0x00, 0x43 };
            byte[] value = { (byte)(p ? 0x01 : 0x00) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbExtPartMute_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbExtPartMute_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetExtPartMute((Boolean)cbExtPartMute.IsChecked);
            }
        }

        private void SetStudioSetExtPartMute(Boolean p)
        {
            commonState.StudioSet.Common.ExternalPartMute = p;
            byte[] address = { 0x18, 0x00, 0x00, 0x4f };
            byte[] value = { (byte)(p ? 0x01 : 0x00) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slExtPartLevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slExtPartLevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetExtPartLevel(slExtPartLevel.Value);
            }
        }

        private void SetStudioSetExtPartLevel(Double p)
        {
            commonState.StudioSet.Common.ExternalPartLevel = (byte)p;
            tbExtPartLevel.Text = "Ext part level " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x00, 0x4c };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slExtPartChorusSend_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slExtPartChorusSend_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetExtPartChorusSend(slExtPartChorusSend.Value);
            }
        }

        private void SetStudioSetExtPartChorusSend(Double p)
        {
            commonState.StudioSet.Common.ExternalPartChorusSendLevel = (byte)p;
            tbExtPartChorusSend.Text = "Ext part chorus send level " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x00, 0x4d };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slExtPartReverbSend_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slExtPartReverbSend_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetExtPartReverbSend(slExtPartReverbSend.Value);
            }
        }

        private void SetStudioSetExtPartReverbSend(Double p)
        {
            commonState.StudioSet.Common.ExternalPartReverbSendLevel = (byte)p;
            tbExtPartReverbSend.Text = "Ext part reverb send level " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x00, 0x4e };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        // Column 1
        private void cbColumn1Selector_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbColumn2Selector_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetColumn1Selector(cbColumn1Selector.SelectedIndex);
            }
        }

        private void SetStudioSetColumn1Selector(Int32 p)
        {
            SystemCommonSettings.IsVisible = false;
            VoiceReserve.IsVisible = false;
            Chorus.IsVisible = false;
            ChorusChorus.IsVisible = false;
            Reverb.IsVisible = false;
            StudioSetMotionalSurround.IsVisible = false;
            StudioSetMasterEQ.IsVisible = false;

            switch (p)
            {
                case 0:
                    SystemCommonSettings.IsVisible = true;
                    break;
                case 1:
                    VoiceReserve.IsVisible = true;
                    break;
                case 2:
                    Chorus.IsVisible = true;
                    cbStudioSetChorusType_SelectionChanged(null, null);
                    break;
                case 3:
                    Reverb.IsVisible = true;
                    cbStudioSetReverbType_SelectionChanged(null, null);
                    break;
                case 4:
                    StudioSetMotionalSurround.IsVisible = true;
                    break;
                case 5:
                    StudioSetMasterEQ.IsVisible = true;
                    break;
            }

            //UpdateControlsAndIntegra7FromClasses(cbStudioSetPartSelector.SelectedIndex);
        }

        private void slSystemCommonMasterTune_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slSystemCommonMasterTune_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonMasterTune((Int32)(slSystemCommonMasterTune.Value));
            }
        }

        private void SetStudioSetSystemCommonMasterTune(Int32 p)
        {
            commonState.StudioSet.SystemCommon.MasterTune = p + 1024;
            tbSystemCommonMasterTune.Text = "Master tune " + (slSystemCommonMasterTune.Value / 10).ToString() + " cent";
            byte[] address = { 0x02, 0x00, 0x00, 0x00 };
            byte[] value = new byte[4];
            value[0] = (byte)((commonState.StudioSet.SystemCommon.MasterTune & 0xf000) >> 12);
            value[1] = (byte)((commonState.StudioSet.SystemCommon.MasterTune & 0x0f00) >> 8);
            value[2] = (byte)((commonState.StudioSet.SystemCommon.MasterTune & 0x00f0) >> 4);
            value[3] = (byte)(commonState.StudioSet.SystemCommon.MasterTune & 0x000f);
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slSystemCommonMasterKeyShift_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slSystemCommonMasterKeyShift_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonMasterKeyShift((Int32)slSystemCommonMasterKeyShift.Value);
            }
        }

        private void SetStudioSetSystemCommonMasterKeyShift(Int32 p)
        {
            commonState.StudioSet.SystemCommon.MasterKeyShift = (byte)(p + 64); // Translates 40 - 88 into -24 - +24
            tbSystemCommonMasterKeyShift.Text = "Master key shift " + slSystemCommonMasterKeyShift.Value.ToString() + " keys";
            byte[] address = { 0x02, 0x00, 0x00, 0x04 };
            byte[] value = { commonState.StudioSet.SystemCommon.MasterKeyShift };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slSystemCommonMasterLevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slSystemCommonMasterLevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonMasterLevel((Int32)slSystemCommonMasterLevel.Value);
            }
        }

        private void SetStudioSetSystemCommonMasterLevel(Int32 p)
        {
            commonState.StudioSet.SystemCommon.MasterLevel = (byte)p;
            tbSystemCommonMasterLevel.Text = "Master level " + slSystemCommonMasterLevel.Value.ToString();
            byte[] address = { 0x02, 0x00, 0x00, 0x05 };
            byte[] value = { (byte)commonState.StudioSet.SystemCommon.MasterLevel };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonScaleTuneSwitch_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonScaleTuneSwitch_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonScaleTuneSwitch((Boolean)cbSystemCommonScaleTuneSwitch.IsChecked);
            }
        }

        private void SetStudioSetSystemCommonScaleTuneSwitch(Boolean p)
        {
            commonState.StudioSet.SystemCommon.ScaleTuneSwitch = p;
            byte[] address = { 0x02, 0x00, 0x00, 0x06 };
            byte[] value = { (byte)(commonState.StudioSet.SystemCommon.ScaleTuneSwitch ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonStudioSetControlChannel_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonStudioSetControlChannel_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonStudioSetControlChannel(cbSystemCommonStudioSetControlChannel.SelectedIndex);
            }
        }

        private void SetStudioSetSystemCommonStudioSetControlChannel(Int32 p)
        {
            commonState.StudioSet.SystemCommon.StudioSetControlChannel = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x11 };
            byte[] value = { (byte)commonState.StudioSet.SystemCommon.StudioSetControlChannel };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonSystemControlSource1_SelectedIndexChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonSystemControlSource1_SelectedIndexChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonSystemControlSource1(cbSystemCommonSystemControlSource1.SelectedIndex);
            }
        }

        private void SetStudioSetSystemCommonSystemControlSource1(Int32 p)
        {
            commonState.StudioSet.SystemCommon.SystemControl1Source = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x20 };
            byte[] value = { (byte)commonState.StudioSet.SystemCommon.SystemControl1Source };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonSystemControlSource2_SelectedIndexChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonSystemControlSource2_SelectedIndexChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonSystemControlSource2(cbSystemCommonSystemControlSource2.SelectedIndex);
            }
        }

        private void SetStudioSetSystemCommonSystemControlSource2(Int32 p)
        {
            commonState.StudioSet.SystemCommon.SystemControl2Source = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x21 };
            byte[] value = { (byte)commonState.StudioSet.SystemCommon.SystemControl2Source };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonSystemControlSource3_SelectedIndexChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonSystemControlSource3_SelectedIndexChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonSystemControlSource3(cbSystemCommonSystemControlSource3.SelectedIndex);
            }
        }

        private void SetStudioSetSystemCommonSystemControlSource3(Int32 p)
        {
            commonState.StudioSet.SystemCommon.SystemControl3Source = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x22 };
            byte[] value = { (byte)commonState.StudioSet.SystemCommon.SystemControl3Source };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonSystemControlSource4_SelectedIndexChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonSystemControlSource4_SelectedIndexChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonSystemControlSource4(cbSystemCommonSystemControlSource4.SelectedIndex);
            }
        }

        private void SetStudioSetSystemCommonSystemControlSource4(Int32 p)
        {
            commonState.StudioSet.SystemCommon.SystemControl4Source = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x23 };
            byte[] value = { (byte)commonState.StudioSet.SystemCommon.SystemControl4Source };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonControlSource_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonControlSource_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonControlSource(cbSystemCommonControlSource.SelectedIndex);
            }
        }

        private void SetStudioSetSystemCommonControlSource(Int32 p)
        {
            commonState.StudioSet.SystemCommon.ControlSource = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x24 };
            byte[] value = { (byte)commonState.StudioSet.SystemCommon.ControlSource };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonSystemClockSource_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonSystemClockSource_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonSystemClockSource(cbSystemCommonSystemClockSource.SelectedIndex);
            }
        }

        private void SetStudioSetSystemCommonSystemClockSource(Int32 p)
        {
            commonState.StudioSet.SystemCommon.SystemClockSource = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x25 };
            byte[] value = { (byte)commonState.StudioSet.SystemCommon.SystemClockSource };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slSystemCommonSystemTempo_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slSystemCommonSystemTempo_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonSystemTempo((Int32)slSystemCommonSystemTempo.Value);
            }
        }

        private void SetStudioSetSystemCommonSystemTempo(Int32 p)
        {
            tbSystemCommonSystemTempo.Text = "System tempo " + p.ToString();
            commonState.StudioSet.SystemCommon.SystemTempo = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x26 };
            byte[] value = { (byte)(commonState.StudioSet.SystemCommon.SystemTempo / 16), (byte)(commonState.StudioSet.SystemCommon.SystemTempo % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonTempoAssignSource_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonTempoAssignSource_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonTempoAssignSource(cbSystemCommonTempoAssignSource.SelectedIndex);
            }
        }

        private void SetStudioSetSystemCommonTempoAssignSource(Int32 p)
        {
            commonState.StudioSet.SystemCommon.TempoAssignSource = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x28 };
            byte[] value = { (byte)commonState.StudioSet.SystemCommon.TempoAssignSource };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonReceiveProgramChange_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonReceiveProgramChange_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonReceiveProgramChange((Boolean)cbSystemCommonReceiveProgramChange.IsChecked);
            }
        }

        private void SetStudioSetSystemCommonReceiveProgramChange(Boolean p)
        {
            commonState.StudioSet.SystemCommon.ReceiveProgramChange = p;
            byte[] address = { 0x02, 0x00, 0x00, 0x29 };
            byte[] value = { (byte)(commonState.StudioSet.SystemCommon.ReceiveProgramChange ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonReceiveBankSelect_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonReceiveBankSelect_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonReceiveBankSelect((Boolean)cbSystemCommonReceiveBankSelect.IsChecked);
            }
        }

        private void SetStudioSetSystemCommonReceiveBankSelect(Boolean p)
        {
            commonState.StudioSet.SystemCommon.ReceiveBankSelect = p;
            byte[] address = { 0x02, 0x00, 0x00, 0x2a };
            byte[] value = { (byte)(commonState.StudioSet.SystemCommon.ReceiveBankSelect ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonSurroundCenterSpeakerSwitch_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonSurroundCenterSpeakerSwitch_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonSurroundCenterSpeakerSwitch((Boolean)cbSystemCommonSurroundCenterSpeakerSwitch.IsChecked);
            }
        }

        private void SetStudioSetSystemCommonSurroundCenterSpeakerSwitch(Boolean p)
        {
            commonState.StudioSet.SystemCommon.SurroundCenterSpeakerSwitch = p;
            byte[] address = { 0x02, 0x00, 0x00, 0x2b };
            byte[] value = { (byte)(commonState.StudioSet.SystemCommon.SurroundCenterSpeakerSwitch ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonSurroundSubWooferSwitch_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonSurroundSubWooferSwitch_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonSurroundSubWooferSwitch((Boolean)cbSystemCommonSurroundSubWooferSwitch.IsChecked);
            }
        }

        private void SetStudioSetSystemCommonSurroundSubWooferSwitch(Boolean p)
        {
            commonState.StudioSet.SystemCommon.SurroundSubWooferSwitch = p;
            byte[] address = { 0x02, 0x00, 0x00, 0x2c };
            byte[] value = { (byte)(commonState.StudioSet.SystemCommon.SurroundSubWooferSwitch ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbSystemCommonStereoOutputMode_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbSystemCommonStereoOutputMode_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetSystemCommonStereoOutputMode(cbSystemCommonStereoOutputMode.SelectedIndex);
            }
        }

        // Voice reserve events
        private void SetStudioSetSystemCommonStereoOutputMode(Int32 p)
        {
            commonState.StudioSet.SystemCommon.StereoOutputMode = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x2d };
            byte[] value = { (byte)commonState.StudioSet.SystemCommon.StereoOutputMode };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve01_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve01_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve01((Int32)slVoiceReserve01.Value);
            }
        }

        private void SetStudioSetVoiceReserve01(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[0] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x18 };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[0] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve02_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve02_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve02((Int32)slVoiceReserve02.Value);
            }
        }

        private void SetStudioSetVoiceReserve02(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[1] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x19 };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[1] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve03_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve03_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve03((Int32)slVoiceReserve03.Value);
            }
        }

        private void SetStudioSetVoiceReserve03(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[2] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x1a };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[2] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve04_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve04_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve04((Int32)slVoiceReserve04.Value);
            }
        }

        private void SetStudioSetVoiceReserve04(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[3] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x1b };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[3] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve05_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve05_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve05((Int32)slVoiceReserve05.Value);
            }
        }

        private void SetStudioSetVoiceReserve05(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[4] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x1c };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[4] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve06_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve06_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve06((Int32)slVoiceReserve06.Value);
            }
        }

        private void SetStudioSetVoiceReserve06(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[5] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x1d };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[5] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve07_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve07_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve07((Int32)slVoiceReserve07.Value);
            }
        }

        private void SetStudioSetVoiceReserve07(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[6] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x1e };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[6] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve08_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve08_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve08((Int32)slVoiceReserve08.Value);
            }
        }

        private void SetStudioSetVoiceReserve08(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[7] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x1f };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[7] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve09_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve09_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve09((Int32)slVoiceReserve09.Value);
            }
        }

        private void SetStudioSetVoiceReserve09(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[8] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x20 };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[8] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve10_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve10_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve10((Int32)slVoiceReserve10.Value);
            }
        }

        private void SetStudioSetVoiceReserve10(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[9] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x21 };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[9] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve11_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve11_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve11((Int32)slVoiceReserve11.Value);
            }
        }

        private void SetStudioSetVoiceReserve11(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[10] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x22 };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[10] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve12_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve12_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve12((Int32)slVoiceReserve12.Value);
            }
        }

        private void SetStudioSetVoiceReserve12(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[11] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x23 };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[11] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve13_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve13_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve13((Int32)slVoiceReserve13.Value);
            }
        }

        private void SetStudioSetVoiceReserve13(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[12] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x24 };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[12] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve14_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve14_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve14((Int32)slVoiceReserve14.Value);
            }
        }

        private void SetStudioSetVoiceReserve14(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[13] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x25 };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[13] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve15_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve15_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve15((Int32)slVoiceReserve15.Value);
            }
        }

        private void SetStudioSetVoiceReserve15(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[14] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x26 };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[14] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slVoiceReserve16_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slVoiceReserve16_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetVoiceReserve16((Int32)slVoiceReserve16.Value);
            }
        }

        private void SetStudioSetVoiceReserve16(Int32 p)
        {
            commonState.StudioSet.Common.VoiceReserve[15] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x27 };
            byte[] value = { commonState.StudioSet.Common.VoiceReserve[15] };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        // Chorus events
        private void cbStudioSetChorusType_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetChorusType_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                // Switch type page:
                //ChorusChorus.IsVisible = false;
                //ChorusDelay.IsVisible = false;
                //ChorusGM2Chorus.IsVisible = false;
                byte p = (byte)cbStudioSetChorusType.SelectedIndex;
                byte[] address = { 0x18, 0x00, 0x04, 0x00 };
                byte[] value = { p, commonState.StudioSet.CommonChorus.Level, 0x00, commonState.StudioSet.CommonChorus.OutputSelect };
                byte[] bytes = new byte[0];

                switch (p)
                {
                    case 0:
                        break;
                    case 1:
                        //ChorusChorus.IsVisible = true;
                        value = value.Concat(commonState.StudioSet.CommonChorus.Chorus.GetData()).ToArray();
                        bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
                        commonState.Midi.SendSystemExclusive(bytes);
                        break;
                    case 2:
                        //ChorusDelay.IsVisible = true;
                        value = value.Concat(commonState.StudioSet.CommonChorus.Delay.GetData()).ToArray();
                        bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
                        commonState.Midi.SendSystemExclusive(bytes);
                        break;
                    case 3:
                        //ChorusGM2Chorus.IsVisible = true;
                        value = value.Concat(commonState.StudioSet.CommonChorus.Gm2Chorus.GetData()).ToArray();
                        bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
                        commonState.Midi.SendSystemExclusive(bytes);
                        break;
                }
                SetStudioSetStudioSetChorusType(p);
            }
        }

        private void SetStudioSetStudioSetChorusType(Int32 p)
        {
            ChorusChorus.IsVisible = false;
            ChorusDelay.IsVisible = false;
            ChorusGM2Chorus.IsVisible = false;

            switch (p)
            {
                case 1:
                    ChorusChorus.IsVisible = true;
                    cbChorusChorusFilterType.SelectedIndex = commonState.StudioSet.CommonChorus.Chorus.FilterType;
                    tbChorusChorusFilterType.Text = "Filter Type: " + cbChorusChorusFilterType.SelectedItem;
                    cbChorusChorusFilterCutoffFrequency.SelectedIndex = commonState.StudioSet.CommonChorus.Chorus.FilterCutoffFrequency;
                    tbChorusChorusFilterCutoffFrequency.Text = "Cutoff Freq: " + cbChorusChorusFilterCutoffFrequency.SelectedItem;
                    slChorusChorusPreDelay.Value = commonState.StudioSet.CommonChorus.Chorus.PreDelay;
                    tbChorusChorusPreDelay.Text = "Pre Delay: " + ((Double)commonState.StudioSet.CommonChorus.Chorus.PreDelay / (Double)10).ToString();
                    cbChorusChorusRateHzNote.SelectedIndex = commonState.StudioSet.CommonChorus.Chorus.RateHzNote;
                    tbChorusChorusRateHz.Text = "Rate in " + cbChorusChorusRateHzNote.SelectedItem;
                    commonState.StudioSet.CommonChorus.Chorus.RateHzNote = (byte)cbChorusChorusRateHzNote.SelectedIndex;
                    slChorusChorusRateHz.Value = commonState.StudioSet.CommonChorus.Chorus.RateHz;
                    tbChorusChorusRateHz.Text = "Rate : " + commonState.StudioSet.CommonChorus.Chorus.RateHz.ToString();
                    slChorusChorusRateNote.Value = commonState.StudioSet.CommonChorus.Chorus.RateHz;
                    tbChorusChorusRateNote.Text = "Rate : " + StudioSet.NoteString[commonState.StudioSet.CommonChorus.Chorus.RateNote];
                    if (commonState.StudioSet.CommonChorus.Chorus.RateHzNote == 0)
                    {
                        tbChorusChorusRateNote.IsVisible = false;
                        slChorusChorusRateNote.IsVisible = false;
                        tbChorusChorusRateHz.IsVisible = true;
                        slChorusChorusRateHz.IsVisible = true;
                    }
                    else
                    {
                        tbChorusChorusRateHz.IsVisible = false;
                        slChorusChorusRateNote.IsVisible = true;
                        tbChorusChorusRateHz.IsVisible = false;
                        slChorusChorusRateNote.IsVisible = true;
                    }
                    slChorusChorusDepth.Value = commonState.StudioSet.CommonChorus.Chorus.Depth;
                    tbChorusChorusDepth.Text = "Depth: " + commonState.StudioSet.CommonChorus.Chorus.Depth.ToString();
                    slChorusChorusPhase.Value = commonState.StudioSet.CommonChorus.Chorus.Phase;
                    tbChorusChorusPhase.Text = "Phase: " + (commonState.StudioSet.CommonChorus.Chorus.Phase * 2).ToString();
                    slChorusChorusFeedback.Value = commonState.StudioSet.CommonChorus.Chorus.Feedback;
                    tbChorusChorusFeedback.Text = "Feedback: " + commonState.StudioSet.CommonChorus.Chorus.Feedback.ToString();
                    break;
                case 2:
                    ChorusDelay.IsVisible = true;
                    commonState.StudioSet.CommonChorus.Delay.LeftMsNote = (byte)cbChorusDelayLeftMsNote.SelectedIndex;
                    slChorusDelayLeftLevel.Value = commonState.StudioSet.CommonChorus.Delay.LeftMs;
                    tbChorusDelayLeftLevel.Text = "Left Level: " + commonState.StudioSet.CommonChorus.Delay.LeftLevel.ToString();
                    slChorusDelayRightLevel.Value = commonState.StudioSet.CommonChorus.Delay.RightMs;
                    tbChorusDelayRightLevel.Text = "Right Level: " + commonState.StudioSet.CommonChorus.Delay.RightLevel.ToString();
                    slChorusDelayCenterLevel.Value = commonState.StudioSet.CommonChorus.Delay.CenterMs;
                    tbChorusDelayCenterLevel.Text = "Center Level: " + commonState.StudioSet.CommonChorus.Delay.CenterLevel.ToString();
                    if (commonState.StudioSet.CommonChorus.Delay.LeftMsNote == 0)
                    {
                        tbChorusDelayLeftNote.IsVisible = false;
                        slChorusDelayLeftNote.IsVisible = false;
                        tbChorusDelayLeftHz.IsVisible = true;
                        slChorusDelayLeftHz.IsVisible = true;
                    }
                    else
                    {
                        tbChorusDelayLeftHz.IsVisible = false;
                        slChorusDelayLeftHz.IsVisible = false;
                        tbChorusDelayLeftNote.IsVisible = true;
                        slChorusDelayLeftNote.IsVisible = true;
                    }
                    if (commonState.StudioSet.CommonChorus.Delay.RightMsNote == 0)
                    {
                        tbChorusDelayRightNote.IsVisible = false;
                        slChorusDelayRightNote.IsVisible = false;
                        tbChorusDelayRightHz.IsVisible = true;
                        slChorusDelayRightHz.IsVisible = true;
                    }
                    else
                    {
                        tbChorusDelayRightHz.IsVisible = false;
                        slChorusDelayRightHz.IsVisible = false;
                        tbChorusDelayRightNote.IsVisible = true;
                        slChorusDelayRightNote.IsVisible = true;
                    }
                    if (commonState.StudioSet.CommonChorus.Delay.CenterMsNote == 0)
                    {
                        tbChorusDelayCenterNote.IsVisible = false;
                        slChorusDelayCenterNote.IsVisible = false;
                        tbChorusDelayCenterHz.IsVisible = true;
                        slChorusDelayCenterHz.IsVisible = true;
                    }
                    else
                    {
                        tbChorusDelayCenterHz.IsVisible = false;
                        slChorusDelayCenterHz.IsVisible = false;
                        tbChorusDelayCenterNote.IsVisible = true;
                        slChorusDelayCenterNote.IsVisible = true;
                    }
                    slChorusDelayCenterFeedback.Value = 2 * (commonState.StudioSet.CommonChorus.Delay.CenterFeedback - 49);
                    tbChorusDelayCenterFeedback.Text = "Center Feedback: " + commonState.StudioSet.CommonChorus.Delay.CenterFeedback.ToString() + "%";
                    cbChorusDelayHFDamp.SelectedIndex = commonState.StudioSet.CommonChorus.Delay.HFDamp;
                    tbChorusDelayHFDamp.Text = "HF Damp: " + cbChorusDelayHFDamp.SelectedItem;
                    //slChorusDelayLeftLevel.Value = commonState.studioSet.CommonChorus.Delay.LeftLevel;
                    //tbChorusDelayLeftLevel.Text = "Left Level: " + commonState.studioSet.CommonChorus.Delay.LeftLevel.ToString();
                    //slChorusDelayRightLevel.Value = commonState.studioSet.CommonChorus.Delay.RightLevel;
                    //tbChorusDelayRightLevel.Text = "Right Level: " + commonState.studioSet.CommonChorus.Delay.RightLevel.ToString();
                    //slChorusDelayCenterLevel.Value = commonState.studioSet.CommonChorus.Delay.CenterLevel;
                    //tbChorusDelayCenterLevel.Text = "Center Level: " + commonState.studioSet.CommonChorus.Delay.CenterLevel.ToString();
                    break;
                case 3:
                    ChorusGM2Chorus.IsVisible = true;
                    slChorusGM2ChorusPreLPF.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.PreLPF;
                    tbChorusGM2ChorusPreLPF.Text = "Pre LPF: " + commonState.StudioSet.CommonChorus.Gm2Chorus.PreLPF.ToString();
                    slChorusGM2ChorusLevel.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Level;
                    tbChorusGM2ChorusLevel.Text = "Level: " + commonState.StudioSet.CommonChorus.Gm2Chorus.Level.ToString();
                    slChorusGM2ChorusFeedback.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Feedback;
                    tbChorusGM2ChorusFeedback.Text = "Feedback: " + commonState.StudioSet.CommonChorus.Gm2Chorus.Feedback.ToString();
                    slChorusGM2ChorusDelay.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Delay;
                    tbChorusGM2ChorusDelay.Text = "Delay: " + commonState.StudioSet.CommonChorus.Gm2Chorus.Delay.ToString();
                    slChorusGM2ChorusRate.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Rate;
                    tbChorusGM2ChorusRate.Text = "Rate: " + commonState.StudioSet.CommonChorus.Gm2Chorus.Rate.ToString();
                    slChorusGM2ChorusDepth.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Depth;
                    tbChorusGM2ChorusDepth.Text = "Depth: " + commonState.StudioSet.CommonChorus.Gm2Chorus.Depth.ToString();
                    slChorusGM2ChorusSendLevelToReverb.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.SendLevelToReverb;
                    tbChorusGM2ChorusSendLevelToReverb.Text = "Send Level to Reverb: " + commonState.StudioSet.CommonChorus.Gm2Chorus.SendLevelToReverb.ToString();
                    break;
            }
            // Tell INTEGRA-7 to change chorus type:
            //byte[] address = { 0x18, 0x00, 0x04, 0x00 };
            //byte[] value = commonState.studioSet.CommonChorus.GetSwitchMessage((byte)p);
            //byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            //waitingForResponseFromIntegra7 = 500;
            //commonState.midi.SendSystemExclusive(bytes);
            //QueryStudioSetChorus(cbStudioSetChorusType.SelectedIndex);
        }

        private void slChorusLevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusLevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusLevel((Int32)slChorusLevel.Value);
            }
        }

        private void SetStudioSetChorusLevel(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x04, 0x01 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
            tbChorusLevel.Text = "Chorus level " + p.ToString();
        }

        private void cbChorusOutputAssign_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbChorusOutputAssign_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusOutputAssign(cbChorusOutputAssign.SelectedIndex);
            }
        }

        private void CbChorusOutputSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            t.Trace("private void CbChorusOutputSelect_SelectedIndexChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusOutputSelect(cbChorusOutputSelect.SelectedIndex);
            }
        }

        // Chorus chorus events
        private void SetStudioSetChorusOutputAssign(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x04, 0x02 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void SetStudioSetChorusOutputSelect(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x04, 0x03 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }


        private void cbChorusChorusFilterType_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbChorusChorusFilterType_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusChorusFilterType(cbChorusChorusFilterType.SelectedIndex);
            }
        }

        private void SetStudioSetChorusChorusFilterType(Int32 p)
        {
            commonState.StudioSet.CommonChorus.Chorus.FilterType = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x04 };
            byte[] value = { 0x08, 0x00, 0x00, (byte)(commonState.StudioSet.CommonChorus.Chorus.FilterType) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbChorusChorusFilterCutoffFrequency_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbChorusChorusFilterCutoffFrequency_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusChorusFilterCutoffFrequency(cbChorusChorusFilterCutoffFrequency.SelectedIndex);
            }
        }

        private void SetStudioSetChorusChorusFilterCutoffFrequency(Int32 p)
        {
            commonState.StudioSet.CommonChorus.Chorus.FilterCutoffFrequency = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x08 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Chorus.FilterCutoffFrequency / 16),
                    (byte)(commonState.StudioSet.CommonChorus.Chorus.FilterCutoffFrequency % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusChorusPreDelay_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusChorusPreDelay_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusChorusPreDelay((Int32)slChorusChorusPreDelay.Value);
            }
        }

        private void SetStudioSetChorusChorusPreDelay(Int32 p)
        {
            commonState.StudioSet.CommonChorus.Chorus.PreDelay = (byte)p;
            tbChorusChorusPreDelay.Text = CalculateChorusPreDelay((Int32)slChorusChorusPreDelay.Value);
            byte[] address = { 0x18, 0x00, 0x04, 0x0c };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Chorus.PreDelay / 16),
                    (byte)(commonState.StudioSet.CommonChorus.Chorus.PreDelay % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbChorusChorusRateHzNote_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbChorusChorusRate_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusChorusRate(cbChorusChorusRateHzNote.SelectedIndex);
            }
        }

        private void SetStudioSetChorusChorusRate(Int32 p)
        {
            PushHandleControlEvents();
            switch (p)
            {
                case 0:
                    tbChorusChorusRateNote.IsVisible = false;
                    slChorusChorusRateNote.IsVisible = false;
                    tbChorusChorusRateHz.IsVisible = true;
                    slChorusChorusRateHz.IsVisible = true;
                    cbChorusChorusRateHzNote.SelectedIndex = 0;
                    break;
                case 1:
                    tbChorusChorusRateHz.IsVisible = false;
                    slChorusChorusRateHz.IsVisible = false;
                    tbChorusChorusRateNote.IsVisible = true;
                    slChorusChorusRateNote.IsVisible = true;
                    cbChorusChorusRateHzNote.SelectedIndex = 1;
                    break;
            }
            PopHandleControlEvents();
            commonState.StudioSet.CommonChorus.Chorus.RateHzNote = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x10 };
            byte[] value = { 0x08, 0x00, 0x00, (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusChorusRateHz_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusChorusRateHz_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusChorusRateHz((Int32)(Math.Round(slChorusChorusRateHz.Value, 2) * 20));
            }
        }

        private void SetStudioSetChorusChorusRateHz(Int32 p)
        {
            tbChorusChorusRateHz.Text = "Rate " + CalculateTimeHz(p) + " Hz";
            commonState.StudioSet.CommonChorus.Chorus.RateHz = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x14 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Chorus.RateHz / 16),
                (byte)(commonState.StudioSet.CommonChorus.Chorus.RateHz % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusChorusRateNote_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusChorusRateNote_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusChorusRateNote((Int32)slChorusChorusRateNote.Value);
            }
        }

        private void SetStudioSetChorusChorusRateNote(Int32 p)
        {
            tbChorusChorusRateNote.Text = "Rate " + CalculateTimeNote(p) + " note";
            commonState.StudioSet.CommonChorus.Chorus.RateNote = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x18 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Chorus.RateNote / 16),
                (byte)(commonState.StudioSet.CommonChorus.Chorus.RateNote % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusChorusDepth_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusChorusDepth_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusChorusDepth((Int32)slChorusChorusDepth.Value);
            }
        }

        private void SetStudioSetChorusChorusDepth(Int32 p)
        {
            tbChorusChorusDepth.Text = "Depth " + p.ToString();
            commonState.StudioSet.CommonChorus.Chorus.Depth = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x1c };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Chorus.Depth / 16),
                (byte)(commonState.StudioSet.CommonChorus.Chorus.Depth % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusChorusPhase_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusChorusPhase_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusChorusPhase((Int32)slChorusChorusPhase.Value);
            }
        }

        private void SetStudioSetChorusChorusPhase(Int32 p)
        {
            tbChorusChorusPhase.Text = "Phase: " + (p * 2).ToString();
            commonState.StudioSet.CommonChorus.Chorus.Phase = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x20 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Chorus.Phase / 16),
                (byte)(commonState.StudioSet.CommonChorus.Chorus.Phase % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusChorusFeedback_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusChorusFeedback_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusChorusFeedback((Int32)slChorusChorusFeedback.Value);
            }
        }

        // Chorus Delay events
        private void SetStudioSetChorusChorusFeedback(Int32 p)
        {
            tbChorusChorusFeedback.Text = "Feedback " + p.ToString();
            commonState.StudioSet.CommonChorus.Chorus.Feedback = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x24 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Chorus.Feedback / 16),
                (byte)(commonState.StudioSet.CommonChorus.Chorus.Feedback % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbChorusDelayLeftMsNote_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbChorusDelayLeft_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayLeft(cbChorusDelayLeftMsNote.SelectedIndex);
            }
        }

        private void SetStudioSetChorusDelayLeft(Int32 p)
        {
            PushHandleControlEvents();
            switch (p)
            {
                case 0:
                    tbChorusDelayLeftHz.IsVisible = true;
                    slChorusDelayLeftHz.IsVisible = true;
                    tbChorusDelayLeftNote.IsVisible = false;
                    slChorusDelayLeftNote.IsVisible = false;
                    cbChorusDelayLeftMsNote.SelectedIndex = 0;
                    commonState.StudioSet.CommonChorus.Delay.LeftMs = (byte)p;
                    break;
                case 1:
                    tbChorusDelayLeftHz.IsVisible = false;
                    slChorusDelayLeftHz.IsVisible = false;
                    tbChorusDelayLeftNote.IsVisible = true;
                    slChorusDelayLeftNote.IsVisible = true;
                    cbChorusDelayLeftMsNote.SelectedIndex = 1;
                    break;
            }
            byte[] address = { 0x18, 0x00, 0x04, 0x04 };
            byte[] value = { 0x08, 0x00, 0x00, (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            commonState.Midi.SendSystemExclusive(bytes);
            PopHandleControlEvents();
        }

        private void slChorusDelayLeftHz_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusDelayLeftHz_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayLeftHz((Int32)slChorusDelayLeftHz.Value);
            }
        }

        private void SetStudioSetChorusDelayLeftHz(Int32 p)
        {
            tbChorusDelayLeftHz.Text = "Delay left " + p + " ms";
            commonState.StudioSet.CommonChorus.Delay.LeftMs = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x08 };
            byte[] value = { 0x08,
                (byte)(p / 256),
                (byte)((p - ((p / 256) * 256)) / 16),
                (byte)((p - ((p / 256) * 256)) % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusDelayLeftNote_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusDelayLeftNote_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayLeftNote((Int32)slChorusDelayLeftNote.Value);
            }
        }

        private void SetStudioSetChorusDelayLeftNote(Int32 p)
        {
            tbChorusDelayLeftNote.Text = "Delay left " + CalculateTimeNote(p) + " note";
            commonState.StudioSet.CommonChorus.Delay.LeftNote = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x0c };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Delay.LeftNote / 16),
                    (byte)(commonState.StudioSet.CommonChorus.Delay.LeftNote % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbChorusDelayRightMsNote_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbChorusDelayRight_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayRight(cbChorusDelayRightMsNote.SelectedIndex);
            }
        }

        private void SetStudioSetChorusDelayRight(Int32 p)
        {
            PushHandleControlEvents();
            switch (p)
            {
                case 0:
                    tbChorusDelayRightHz.IsVisible = true;
                    slChorusDelayRightHz.IsVisible = true;
                    tbChorusDelayRightNote.IsVisible = false;
                    slChorusDelayRightNote.IsVisible = false;
                    cbChorusDelayRightMsNote.SelectedIndex = 0;
                    break;
                case 1:
                    tbChorusDelayRightHz.IsVisible = false;
                    slChorusDelayRightHz.IsVisible = false;
                    tbChorusDelayRightNote.IsVisible = true;
                    slChorusDelayRightNote.IsVisible = true;
                    cbChorusDelayRightMsNote.SelectedIndex = 1;
                    break;
            }
            byte[] address = { 0x18, 0x00, 0x04, 0x10 };
            byte[] value = { 0x08, 0x00, 0x00, (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            commonState.Midi.SendSystemExclusive(bytes);
            PopHandleControlEvents();
        }

        private void slChorusDelayRightHz_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusDelayRightHz_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayRightHz((Int32)slChorusDelayRightHz.Value);
            }
        }

        private void SetStudioSetChorusDelayRightHz(Int32 p)
        {
            tbChorusDelayRightHz.Text = "Delay right " + p + " ms";
            commonState.StudioSet.CommonChorus.Delay.RightMs = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x14 };
            byte[] value = { 0x08,
                (byte)(p / 256),
                (byte)((p - ((p / 256) * 256)) / 16),
                (byte)((p - ((p / 256) * 256)) % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusDelayRightNote_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusDelayRightNote_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayRightNote((Int32)slChorusDelayRightNote.Value);
            }
        }

        private void SetStudioSetChorusDelayRightNote(Int32 p)
        {
            tbChorusDelayRightNote.Text = "Delay right " + CalculateTimeNote(p) + " note";
            commonState.StudioSet.CommonChorus.Delay.RightNote = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x18 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Delay.RightNote / 16),
                    (byte)(commonState.StudioSet.CommonChorus.Delay.RightNote % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbChorusDelayCenterMsNote_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbChorusDelayCenter_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayCenter(cbChorusDelayCenterMsNote.SelectedIndex);
            }
        }

        private void SetStudioSetChorusDelayCenter(Int32 p)
        {
            PushHandleControlEvents();
            switch (cbChorusDelayCenterMsNote.SelectedIndex)
            {
                case 0:
                    tbChorusDelayCenterHz.IsVisible = true;
                    slChorusDelayCenterHz.IsVisible = true;
                    tbChorusDelayCenterNote.IsVisible = false;
                    slChorusDelayCenterNote.IsVisible = false;
                    cbChorusDelayCenterMsNote.SelectedIndex = 0;
                    break;
                case 1:
                    tbChorusDelayCenterHz.IsVisible = false;
                    slChorusDelayCenterHz.IsVisible = false;
                    tbChorusDelayCenterNote.IsVisible = true;
                    slChorusDelayCenterNote.IsVisible = true;
                    cbChorusDelayCenterMsNote.SelectedIndex = 1;
                    break;
            }
            byte[] address = { 0x18, 0x00, 0x04, 0x1c };
            byte[] value = { 0x08, 0x00, 0x00, (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            commonState.Midi.SendSystemExclusive(bytes);
            PopHandleControlEvents();
        }

        private void slChorusDelayCenterHz_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusDelayCenterHz_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayCenterHz((Int32)slChorusDelayCenterHz.Value);
            }
        }

        private void SetStudioSetChorusDelayCenterHz(Int32 p)
        {
            tbChorusDelayCenterHz.Text = "Delay Center " + p + " ms";
            commonState.StudioSet.CommonChorus.Delay.CenterMs = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x20 };
            byte[] value = { 0x08,
                (byte)(p / 256),
                (byte)((p - ((p / 256) * 256)) / 16),
                (byte)((p - ((p / 256) * 256)) % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusDelayCenterNote_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusDelayCenterNote_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayCenterNote((Int32)slChorusDelayCenterNote.Value);
            }
        }

        private void SetStudioSetChorusDelayCenterNote(Int32 p)
        {
            tbChorusDelayCenterNote.Text = "Delay Center " + CalculateTimeNote(p) + " note";
            commonState.StudioSet.CommonChorus.Delay.CenterNote = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x24 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Delay.CenterNote / 16),
                        (byte)(commonState.StudioSet.CommonChorus.Delay.CenterNote % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusDelayCenterFeedback_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusDelayCenterFeedback_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayCenterFeedback((Int32)slChorusDelayCenterFeedback.Value);
            }
        }

        private void SetStudioSetChorusDelayCenterFeedback(Int32 p)
        {
            tbChorusDelayCenterFeedback.Text = "Center feedback " + p + "%";
            commonState.StudioSet.CommonChorus.Delay.CenterFeedback = (byte)((p / 2) + 49);
            byte[] address = { 0x18, 0x00, 0x04, 0x28 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Delay.CenterFeedback / 16),
                        (byte)(commonState.StudioSet.CommonChorus.Delay.CenterFeedback % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbChorusDelayHFDamp_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbChorusDelayHFDamp_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayHFDamp(cbChorusDelayHFDamp.SelectedIndex);
            }
        }

        private void SetStudioSetChorusDelayHFDamp(Int32 p)
        {
            commonState.StudioSet.CommonChorus.Delay.HFDamp = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x2c };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Delay.HFDamp / 16),
                        (byte)(commonState.StudioSet.CommonChorus.Delay.HFDamp % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusDelayLeftLevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusDelayLeftLevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayLeftLevel((Int32)slChorusDelayLeftLevel.Value);
            }
        }

        private void SetStudioSetChorusDelayLeftLevel(Int32 p)
        {
            tbChorusDelayLeftLevel.Text = "Left level " + p.ToString();
            commonState.StudioSet.CommonChorus.Delay.LeftLevel = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x30 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Delay.LeftLevel / 16),
                        (byte)(commonState.StudioSet.CommonChorus.Delay.LeftLevel % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusDelayRightLevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusDelayRightevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayRightLevel((Int32)slChorusDelayRightLevel.Value);
            }
        }

        private void SetStudioSetChorusDelayRightLevel(Int32 p)
        {
            tbChorusDelayRightLevel.Text = "Right level " + p.ToString();
            commonState.StudioSet.CommonChorus.Delay.RightLevel = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x34 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Delay.RightLevel / 16),
                        (byte)(commonState.StudioSet.CommonChorus.Delay.RightLevel % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slChorusDelayCenterLevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusDelayCenterLevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayCenterLevel((Int32)slChorusDelayCenterLevel.Value);
            }
        }

        private void SetStudioSetChorusDelayCenterLevel(Int32 p)
        {
            tbChorusDelayCenterLevel.Text = "Center level " + p.ToString();
            commonState.StudioSet.CommonChorus.Delay.CenterLevel = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x38 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.StudioSet.CommonChorus.Delay.CenterLevel / 16),
                        (byte)(commonState.StudioSet.CommonChorus.Delay.CenterLevel % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        // Chorus GM2 event handlsers
        private void slChorusGM2ChorusPreLPF_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusGM2ChorusPreLPF_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusGM2ChorusPreLPF((Int32)slChorusGM2ChorusPreLPF.Value);
            }
        }

        private void SetStudioSetChorusGM2ChorusPreLPF(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x04, 0x04 };
            SendHashMarkedMessage(address, p);
            tbChorusGM2ChorusPreLPF.Text = "Pre-LPF " + p.ToString();
        }

        private void slChorusGM2ChorusLevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusGM2ChorusLevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusGM2ChorusLevel((Int32)slChorusGM2ChorusLevel.Value);
            }
        }

        private void SetStudioSetChorusGM2ChorusLevel(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x04, 0x08 };
            SendHashMarkedMessage(address, p);
            tbChorusGM2ChorusLevel.Text = "Level " + p.ToString();
        }

        private void slChorusGM2ChorusFeedback_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusGM2ChorusFeedback_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusGM2ChorusFeedback((Int32)slChorusGM2ChorusFeedback.Value);
            }
        }

        private void SetStudioSetChorusGM2ChorusFeedback(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x04, 0x0c };
            SendHashMarkedMessage(address, p);
            tbChorusGM2ChorusFeedback.Text = "Feedback " + p.ToString();
        }

        private void slChorusGM2ChorusDelay_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusGM2ChorusDelay_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusGM2ChorusDelay((Int32)slChorusGM2ChorusDelay.Value);
            }
        }

        private void SetStudioSetChorusGM2ChorusDelay(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x04, 0x10 };
            SendHashMarkedMessage(address, p);
            tbChorusGM2ChorusDelay.Text = "Delay " + p.ToString();
        }

        private void slChorusGM2ChorusRate_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusGM2ChorusRate_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusGM2ChorusRate((Int32)slChorusGM2ChorusRate.Value);
            }
        }

        private void SetStudioSetChorusGM2ChorusRate(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x04, 0x14 };
            SendHashMarkedMessage(address, p);
            tbChorusGM2ChorusRate.Text = "Rate " + p.ToString();
        }

        private void slChorusGM2ChorusDepth_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusGM2ChorusDepth_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusGM2ChorusDepth((Int32)slChorusGM2ChorusDepth.Value);
            }
        }

        private void SetStudioSetChorusGM2ChorusDepth(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x04, 0x18 };
            SendHashMarkedMessage(address, p);
            tbChorusGM2ChorusDepth.Text = "Depth " + p.ToString();
        }

        private void slChorusGM2ChorusSendLevelToReverb_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusGM2ChorusSendLevelToReverb_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusGM2ChorusSendLevelToReverb((Int32)slChorusGM2ChorusLevel.Value);
            }
        }

        // Studio set Reverb event handlers
        private void SetStudioSetChorusGM2ChorusSendLevelToReverb(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x04, 0x1c };
            SendHashMarkedMessage(address, p);
            tbChorusGM2ChorusSendLevelToReverb.Text = "Send level to reverb " + p.ToString();
        }

        private void cbStudioSetReverbType_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetReverbType_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetReverbType(cbStudioSetReverbType.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetReverbType(Int32 p)
        {
            StudioSetReverbGM2.IsVisible = false;
            StudioSetReverbReverb.IsVisible = false;
            byte[] address = new byte[] { 0x18, 0x00, 0x06, 0x00 };
            byte[] value = new byte[] { (byte)cbStudioSetReverbType.SelectedIndex, commonState.StudioSet.CommonReverb.Level, commonState.StudioSet.CommonReverb.OutputAssign };
            byte[] bytes = new byte[0];

            switch (p)
            {
                case 1:
                    StudioSetReverbReverb.IsVisible = true;
                    value = value.Concat(commonState.StudioSet.CommonReverb.ReverbRoom1.GetData()).ToArray();
                    commonState.Midi.SendSystemExclusive(bytes);
                    break;
                case 2:
                    StudioSetReverbReverb.IsVisible = true;
                    value = value.Concat(commonState.StudioSet.CommonReverb.ReverbRoom2.GetData()).ToArray();
                    commonState.Midi.SendSystemExclusive(bytes);
                    break;
                case 3:
                    StudioSetReverbReverb.IsVisible = true;
                    value = value.Concat(commonState.StudioSet.CommonReverb.ReverbHall1.GetData()).ToArray();
                    commonState.Midi.SendSystemExclusive(bytes);
                    break;
                case 4:
                    StudioSetReverbReverb.IsVisible = true;
                    value = value.Concat(commonState.StudioSet.CommonReverb.ReverbHall2.GetData()).ToArray();
                    commonState.Midi.SendSystemExclusive(bytes);
                    break;
                case 5:
                    StudioSetReverbReverb.IsVisible = true;
                    value = value.Concat(commonState.StudioSet.CommonReverb.ReverbPlate.GetData()).ToArray();
                    commonState.Midi.SendSystemExclusive(bytes);
                    break;
                case 6:
                    StudioSetReverbGM2.IsVisible = true;
                    value = value.Concat(commonState.StudioSet.CommonReverb.GM2Reverb.GetData()).ToArray();
                    commonState.Midi.SendSystemExclusive(bytes);
                    break;
            }
            //SetStudioSetCommonReverbControls(p);
        }

        private void SetStudioSetCommonReverbControls(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x06, 0x00 };
            byte[] value = { (byte)p, commonState.StudioSet.CommonReverb.Level, commonState.StudioSet.CommonReverb.OutputAssign };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

            StudioSet_CommonReverbReverb reverb = null;
            switch (p)
            {
                // NOTE! The attributes was initially named after Hall1, but some names are incorrect
                // while using the same address as Hall1 but control a different setting.
                case 1:
                    reverb = commonState.StudioSet.CommonReverb.ReverbRoom1;
                    break;
                case 2:
                    reverb = commonState.StudioSet.CommonReverb.ReverbRoom2;
                    break;
                case 3:
                    reverb = commonState.StudioSet.CommonReverb.ReverbHall1;
                    break;
                case 4:
                    reverb = commonState.StudioSet.CommonReverb.ReverbHall2;
                    break;
                case 5:
                    reverb = commonState.StudioSet.CommonReverb.ReverbPlate;
                    break;
            }
            // When the controls are updated below, their handlers will also send 
            // the settings to I-7.
            if (p == 6)
            {
                slStudioSetReverbGM2Character.Value =
                    commonState.StudioSet.CommonReverb.GM2Reverb.Character;
                slStudioSetReverbGM2Time.Value =
                    commonState.StudioSet.CommonReverb.GM2Reverb.Time;
            }
            else if (p > 0)
            {
                slStudioSetReverbPreDelay.Value = reverb.PreDelay;
                slStudioSetReverbTime.Value = 10 * reverb.Time;
                slStudioSetReverbDensity.Value = reverb.Density;
                slStudioSetReverbDiffusion.Value = reverb.Diffusion;
                slStudioSetReverbLFDamp.Value = reverb.LFDamp;
                slStudioSetReverbHFDamp.Value = reverb.HFDamp;
                slStudioSetReverbSpread.Value = reverb.Spread;
                slStudioSetReverbTone.Value = reverb.Tone;
            }
        }

        private void slReverbLevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slReverbLevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetReverbLevel((Int32)slStudioSetReverbLevel.Value);
            }
        }

        private void SetStudioSetReverbLevel(Int32 p)
        {
            tbStudioSetReverbLevel.Text = "Reverb level " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x06, 0x01 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbStudioSetReverbOutputAssign_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetReverbOutputAssign_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetReverbOutputAssign(cbStudioSetReverbOutputAssign.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetReverbOutputAssign(Int32 p)
        {

            byte[] address = { 0x18, 0x00, 0x06, 0x02 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetReverbPreDelay_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetReverbPreDelay_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetReverbPreDelay((Int32)slStudioSetReverbPreDelay.Value);
            }
        }

        private void SetStudioSetStudioSetReverbPreDelay(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x06, 0x07 };
            SendHashMarkedMessage(address, p);
            tbStudioSetReverbPreDelay.Text = "Pre delay " + p.ToString() + " ms";
            switch (commonState.StudioSet.CommonReverb.Type)
            {
                case 1:
                    commonState.StudioSet.CommonReverb.ReverbRoom1.PreDelay = (byte)p;
                    break;
                case 2:
                    commonState.StudioSet.CommonReverb.ReverbRoom2.PreDelay = (byte)p;
                    break;
                case 3:
                    commonState.StudioSet.CommonReverb.ReverbHall1.PreDelay = (byte)p;
                    break;
                case 4:
                    commonState.StudioSet.CommonReverb.ReverbHall2.PreDelay = (byte)p;
                    break;
                case 5:
                    commonState.StudioSet.CommonReverb.ReverbPlate.PreDelay = (byte)p;
                    break;
            }
        }

        private void slStudioSetReverbTime_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetReverbTime_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetReverbTime((Int32)slStudioSetReverbTime.Value);
            }
        }

        private void SetStudioSetStudioSetReverbTime(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x06, 0x0b };
            SendHashMarkedMessage(address, p);
            Double value = slStudioSetReverbTime.Value / 10;
            String sValue = value.ToString();
            Int32 pos = sValue.IndexOf('.');
            if (pos > -1 && pos < sValue.Length - 2)
            {
                sValue = sValue.Remove(pos + 2);
            }
            tbStudioSetReverbTime.Text = "Time " + sValue + " s";

            switch (commonState.StudioSet.CommonReverb.Type)
            {
                case 1:
                    commonState.StudioSet.CommonReverb.ReverbRoom1.Time = (byte)p;
                    break;
                case 2:
                    commonState.StudioSet.CommonReverb.ReverbRoom2.Time = (byte)p;
                    break;
                case 3:
                    commonState.StudioSet.CommonReverb.ReverbHall1.Time = (byte)p;
                    break;
                case 4:
                    commonState.StudioSet.CommonReverb.ReverbHall2.Time = (byte)p;
                    break;
                case 5:
                    commonState.StudioSet.CommonReverb.ReverbPlate.Time = (byte)p;
                    break;
            }
        }

        private void slStudioSetReverbDensity_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetReverbDensity_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetReverbDensity((Int32)slStudioSetReverbDensity.Value);
            }
        }

        private void SetStudioSetStudioSetReverbDensity(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x06, 0x0f };
            SendHashMarkedMessage(address, p);
            tbStudioSetReverbDensity.Text = "Density " + p.ToString();
        }

        private void slStudioSetReverbDiffusion_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetReverbDiffusion_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetReverbDiffusion((Int32)slStudioSetReverbDiffusion.Value);
            }
        }

        private void SetStudioSetStudioSetReverbDiffusion(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x06, 0x13 };
            SendHashMarkedMessage(address, p);
            tbStudioSetReverbDiffusion.Text = "Diffusion " + p.ToString();
        }

        private void slStudioSetReverbLFDamp_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetReverbLFDamp_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetReverbLFDamp((Int32)slStudioSetReverbLFDamp.Value);
            }
        }

        private void SetStudioSetStudioSetReverbLFDamp(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x06, 0x17 };
            SendHashMarkedMessage(address, p);
            tbStudioSetReverbLFDamp.Text = "LF damp" + p.ToString();
        }

        private void slStudioSetReverbHFDamp_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetReverbHFDamp_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetReverbHFDamp((Int32)slStudioSetReverbHFDamp.Value);
            }
        }

        private void SetStudioSetStudioSetReverbHFDamp(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x06, 0x1b };
            SendHashMarkedMessage(address, p);
            tbStudioSetReverbHFDamp.Text = "HF damp" + p.ToString();
        }

        private void slStudioSetReverbSpread_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetReverbSpread_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetReverbSpread((Int32)slStudioSetReverbSpread.Value);
            }
        }

        private void SetStudioSetStudioSetReverbSpread(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x06, 0x1f };
            SendHashMarkedMessage(address, p);
            tbStudioSetReverbSpread.Text = "Spread " + p.ToString();
        }

        private void slStudioSetReverbTone_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetReverbTone_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetReverbTone((Int32)slStudioSetReverbTone.Value);
            }
        }

        private void SetStudioSetStudioSetReverbTone(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x06, 0x23 };
            SendHashMarkedMessage(address, p);
            tbStudioSetReverbTone.Text = "Tone " + p.ToString();
        }

        private void slStudioSetReverbGM2Character_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetReverbGM2Character_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetReverbGM2Character((Int32)slStudioSetReverbGM2Character.Value);
            }
        }

        private void SetStudioSetStudioSetReverbGM2Character(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x06, 0x03 };
            SendHashMarkedMessage(address, p);
            tbStudioSetReverbGM2Character.Text = "Character " + p.ToString();
        }

        private void slStudioSetReverbGM2Time_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetReverbGM2Time_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetReverbGM2Time((Int32)slStudioSetReverbGM2Time.Value);
            }
        }

        private void SetStudioSetStudioSetReverbGM2Time(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x06, 0x0f };
            SendHashMarkedMessage(address, p);

            //Double value = p / 10;
            //String sValue = value.ToString();
            //Int32 pos = sValue.IndexOf('.');
            //if (pos > -1 && pos < sValue.Length - 2)
            //{
            //    sValue = sValue.Remove(pos + 2);
            //}
            tbStudioSetReverbGM2Time.Text = "Time " + p + " s";
        }

        // Motional surround 18 00 08 00 
        private void cbStudioSetMotionalSurround_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetMotionalSurround_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurround((Boolean)cbStudioSetMotionalSurround.IsChecked);
            }
        }

        private void SetStudioSetStudioSetMotionalSurround(Boolean p)
        {
            commonState.StudioSet.MotionalSurround.MotionalSurroundSwitch = p;
            byte[] address = { 0x18, 0x00, 0x08, 0x00 };
            byte[] value = { (byte)(p ? 0x01 : 0x00) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbStudioSetMotionalSurroundRoomType_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetMotionalSurroundRoomType_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurroundRoomType((Int32)cbStudioSetMotionalSurroundRoomType.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetMotionalSurroundRoomType(Int32 p)
        {
            commonState.StudioSet.MotionalSurround.RoomType = (byte)p;
            byte[] address = { 0x18, 0x00, 0x08, 0x01 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetMotionalSurroundAmbienceLevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetMotionalSurroundAmbienceLevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurroundAmbienceLevel((Int32)slStudioSetMotionalSurroundAmbienceLevel.Value);
            }
        }

        private void SetStudioSetStudioSetMotionalSurroundAmbienceLevel(Int32 p)
        {
            commonState.StudioSet.MotionalSurround.AmbienceLevel = (byte)p;
            tbStudioSetMotionalSurroundAmbienceLevel.Text = "Ambience level " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x02 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbStudioSetMotionalSurroundRoomSize_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetMotionalSurroundRoomSize_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurroundRoomSize(cbStudioSetMotionalSurroundRoomSize.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetMotionalSurroundRoomSize(Int32 p)
        {
            commonState.StudioSet.MotionalSurround.RoomSize = (byte)p;
            byte[] address = { 0x18, 0x00, 0x08, 0x03 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetMotionalSurroundAmbienceTime_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetMotionalSurroundAmbienceTime_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurroundAmbienceTime((Int32)slStudioSetMotionalSurroundAmbienceTime.Value);
            }
        }

        private void SetStudioSetStudioSetMotionalSurroundAmbienceTime(Int32 p)
        {
            commonState.StudioSet.MotionalSurround.AmbienceTime = (byte)p;
            tbStudioSetMotionalSurroundAmbienceTime.Text = "Ambience time " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x04 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetMotionalSurroundAmbienceDensity_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetMotionalSurroundAmbienceDensity_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurroundAmbienceDensity((Int32)slStudioSetMotionalSurroundAmbienceDensity.Value);
            }
        }

        private void SetStudioSetStudioSetMotionalSurroundAmbienceDensity(Int32 p)
        {
            commonState.StudioSet.MotionalSurround.AmbienceDensity = (byte)p;
            tbStudioSetMotionalSurroundAmbienceDensity.Text = "Ambience density " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x05 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetMotionalSurroundAmbienceHFDamp_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetMotionalSurroundAmbienceHFDamp_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurroundAmbienceHFDamp((Int32)slStudioSetMotionalSurroundAmbienceHFDamp.Value);
            }
        }

        private void SetStudioSetStudioSetMotionalSurroundAmbienceHFDamp(Int32 p)
        {
            commonState.StudioSet.MotionalSurround.AmbienceHFDamp = (byte)p;
            tbStudioSetMotionalSurroundAmbienceHFDamp.Text = "Ambience HF damp " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x06 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetMotionalSurroundExternalPartLR_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetMotionalSurroundExternalPartLR_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurroundExternalPartLR((Int32)slStudioSetMotionalSurroundExternalPartLR.Value);
            }
        }

        private void SetStudioSetStudioSetMotionalSurroundExternalPartLR(Int32 p)
        {
            commonState.StudioSet.MotionalSurround.ExtPartLR = (byte)(p + 64);
            tbStudioSetMotionalSurroundExternalPartLR.Text = "External part L-R " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x07 };
            byte[] value = { (byte)commonState.StudioSet.MotionalSurround.ExtPartLR };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetMotionalSurroundExternalPartFB_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetMotionalSurroundExternalPartFB_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurroundExternalPartFB((Int32)slStudioSetMotionalSurroundExternalPartFB.Value);
            }
        }

        private void SetStudioSetStudioSetMotionalSurroundExternalPartFB(Int32 p)
        {
            commonState.StudioSet.MotionalSurround.ExtPartFB = (byte)(p + 64);
            tbStudioSetMotionalSurroundExternalPartFB.Text = "External part F-B " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x08 };
            byte[] value = { commonState.StudioSet.MotionalSurround.ExtPartFB };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetMotionalSurroundExtPartWidth_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetMotionalSurroundExtPartWidth_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurroundExtPartWidth((Int32)slStudioSetMotionalSurroundExtPartWidth.Value);
            }
        }

        private void SetStudioSetStudioSetMotionalSurroundExtPartWidth(Int32 p)
        {
            commonState.StudioSet.MotionalSurround.ExtPartWidth = (byte)p;
            tbStudioSetMotionalSurroundExtPartWidth.Text = "External part width " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x09 };
            byte[] value = { commonState.StudioSet.MotionalSurround.ExtPartWidth };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetMotionalSurroundExtpartAmbienceSend_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetMotionalSurroundExtpartAmbienceSend_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurroundExtpartAmbienceSend((Int32)slStudioSetMotionalSurroundExtpartAmbienceSend.Value);
            }
        }

        private void SetStudioSetStudioSetMotionalSurroundExtpartAmbienceSend(Int32 p)
        {
            commonState.StudioSet.MotionalSurround.ExtPartAmbienceSendLevel = (byte)p;
            tbStudioSetMotionalSurroundExtpartAmbienceSend.Text = "External part ambience send" + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x0a };
            byte[] value = { commonState.StudioSet.MotionalSurround.ExtPartAmbienceSendLevel };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbStudioSetMotionalSurroundExtPartControl_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetMotionalSurroundExtPartControl_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurroundExtPartControl(cbStudioSetMotionalSurroundExtPartControl.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetMotionalSurroundExtPartControl(Int32 p)
        {
            commonState.StudioSet.MotionalSurround.ExtPartControlChannel = (byte)p;
            byte[] address = { 0x18, 0x00, 0x08, 0x0b };
            byte[] value = { commonState.StudioSet.MotionalSurround.ExtPartControlChannel };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetMotionalSurroundDepth_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetMotionalSurroundDepth_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMotionalSurroundDepth((Int32)slStudioSetMotionalSurroundDepth.Value);
            }
        }

        private void SetStudioSetStudioSetMotionalSurroundDepth(Int32 p)
        {
            commonState.StudioSet.MotionalSurround.MotionalSurroundDepth = (byte)p;
            tbStudioSetMotionalSurroundDepth.Text = "Motional surround depth " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x0c };
            byte[] value = { commonState.StudioSet.MotionalSurround.MotionalSurroundDepth };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        // Master equalizer 18 00 09 00
        private void cbStudioSetMasterEqLowFreq_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetMasterEqLowFreq_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMasterEqLowFreq(cbStudioSetMasterEqLowFreq.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetMasterEqLowFreq(Int32 p)
        {
            commonState.StudioSet.MasterEQ.EQLowFreq = (byte)p;
            byte[] address = { 0x18, 0x00, 0x09, 0x00 };
            byte[] value = { commonState.StudioSet.MasterEQ.EQLowFreq };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetMasterEqLowGain_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetMasterEqLowGain_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMasterEqLowGain((Int32)slStudioSetMasterEqLowGain.Value);
            }
        }

        private void SetStudioSetStudioSetMasterEqLowGain(Int32 p)
        {
            commonState.StudioSet.MasterEQ.EQLowGain = (byte)(p + 15);
            tbStudioSetMasterEqLowGain.Text = "EQ low gain " + (p).ToString() + " dB";
            byte[] address = { 0x18, 0x00, 0x09, 0x01 };
            byte[] value = { commonState.StudioSet.MasterEQ.EQLowGain };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbStudioSetMasterEqMidFreq_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetMasterEqMidFreq_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMasterEqMidFreq(cbStudioSetMasterEqMidFreq.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetMasterEqMidFreq(Int32 p)
        {
            commonState.StudioSet.MasterEQ.EQMidFreq = (byte)p;
            byte[] address = { 0x18, 0x00, 0x09, 0x02 };
            byte[] value = { commonState.StudioSet.MasterEQ.EQMidFreq };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetMasterEqMidGain_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetMasterEqMidGain_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMasterEqMidGain((Int32)slStudioSetMasterEqMidGain.Value);
            }
        }

        private void SetStudioSetStudioSetMasterEqMidGain(Int32 p)
        {
            commonState.StudioSet.MasterEQ.EQMidGain = (byte)(p + 15);
            tbStudioSetMasterEqMidGain.Text = "EQ mid gain " + p.ToString() + " dB";
            byte[] address = { 0x18, 0x00, 0x09, 0x03 };
            byte[] value = { commonState.StudioSet.MasterEQ.EQMidGain };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbStudioSetMasterEqMidQ_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetMasterEqMidQ_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMasterEqMidQ(cbStudioSetMasterEqMidQ.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetMasterEqMidQ(Int32 p)
        {
            commonState.StudioSet.MasterEQ.EQMidQ = (byte)p;
            byte[] address = { 0x18, 0x00, 0x09, 0x04 };
            byte[] value = { commonState.StudioSet.MasterEQ.EQMidQ };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbStudioSetMasterEqHighFreq_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetMasterEqHighFreq_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMasterEqHighFreq(cbStudioSetMasterEqHighFreq.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetMasterEqHighFreq(Int32 p)
        {
            commonState.StudioSet.MasterEQ.EQHighFreq = (byte)p;
            byte[] address = { 0x18, 0x00, 0x09, 0x05 };
            byte[] value = { commonState.StudioSet.MasterEQ.EQHighFreq };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void slStudioSetMasterEqHighGain_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetMasterEqHighGain_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetMasterEqHighGain((Int32)slStudioSetMasterEqHighGain.Value);
            }
        }

        private void SetStudioSetStudioSetMasterEqHighGain(Int32 p)
        {
            commonState.StudioSet.MasterEQ.EQHighGain = (byte)(p + 15);
            tbStudioSetMasterEqHighGain.Text = "EQ high gain " + p.ToString() + " dB";
            byte[] address = { 0x18, 0x00, 0x09, 0x06 };
            byte[] value = { commonState.StudioSet.MasterEQ.EQHighGain };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        // Column 2
        private void cbStudioSetPartSelector_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartSelector_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSelector(cbStudioSetPartSelector.SelectedIndex);
            }
        }


        private void SetStudioSetStudioSetPartSelector(Int32 p)
        {
            commonState.Midi.SetMidiOutPortChannel((byte)p); // We are actually talking part here, not MIDI channel. MIDI channel migth be changed and is stored in commonState.PartChannels[cbStudioSetPartSelector.SelectedIndex]
            QueryStudioSetPart();
        }

        private void cbStudioSetPartSubSelector_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartSubSelector_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSubSelector(cbStudioSetPartSubSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSubSelector(Int32 p)
        {
            StudioSetPartSettings1.IsVisible = false;
            StudioSetPartSettings2.IsVisible = false;
            StudioSetPartKeyboard.IsVisible = false;
            StudioSetPartScaleTune.IsVisible = false;
            StudioSetPartMidi.IsVisible = false;
            StudioSetPartMotionalSurround.IsVisible = false;
            StudioSetPartEQ.IsVisible = false;
            switch (p)
            {
                case 0:
                    StudioSetPartSettings1.IsVisible = true;
                    break;
                case 1:
                    StudioSetPartSettings2.IsVisible = true;
                    break;
                case 2:
                    StudioSetPartKeyboard.IsVisible = true;
                    break;
                case 3:
                    StudioSetPartScaleTune.IsVisible = true;
                    break;
                case 4:
                    StudioSetPartMidi.IsVisible = true;
                    break;
                case 5:
                    StudioSetPartMotionalSurround.IsVisible = true;
                    break;
                case 6:
                    StudioSetPartEQ.IsVisible = true;
                    break;
            }
        }

        // Part settings 1
        private async void cbStudioSetPartSettings1ReceiveChannel_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartSettings1ReceiveChannel_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                if (await mainPage.DisplayAlert("Warning!", "INTEGRA-7 normally uses channels 1 thru 16 for parts 1 thru 16.\r\n\r\n" +
                    "If you change channel you might end up with two or more parts responding to the same channel.\r\n\r\n" +
                    "Also, if you select a part in the librarian, and that part has another channel assigned here, " +
                    "you will not hear it, or you will hear wrong tone.\r\n\r\n" +
                    "However, this is useful for splitting the keyboard to play different instruments on different parts of the keyboard.\r\n\r\n" +
                    "To do that, you set up two or more parts to use the same MIDI channel, and limit their use of the keyboard as you like it.\r\n\r\n" +
                    "NOTE! When MIDI channel is changed, making other changes may not work as you expect!\r\n\r\n" +
                    "Thus, if you want to change MIDI channel, make all other changes before you change the MIDI channel.\r\n\r\n" +
                    "Are you shure you want to do this?", "Yes", "No"))
                { 
                    SetStudioSetStudioSetPartSettings1ReceiveChannel(cbStudioSetPartSettings1ReceiveChannel.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
                }
            }
        }

        private void SetStudioSetStudioSetPartSettings1ReceiveChannel(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[part].ReceiveChannel = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + part), 0x00 };
            byte[] value = { commonState.StudioSet.PartMainSettings[part].ReceiveChannel };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void cbStudioSetPartSettings1Receive_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartSettings1Receive_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1Receive((Boolean)cbStudioSetPartSettings1Receive.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings1Receive(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ReceiveSwitch = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + cbStudioSetPartSelector.SelectedIndex), 0x01 };
            byte[] value = { (byte)(commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ReceiveSwitch ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartSettings1Group_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartSettings1Group_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1Group(cbStudioSetPartSettings1Group.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings1Group(Int32 p, Int32 part)
        {
            if (MidiChannelIsSameAsPart())
            {
                Boolean currentHandleControlEvents = handleControlEvents;
                PushHandleControlEvents();
                PopulateCbStudioSetPartSettings1Category();
                handleControlEvents = currentHandleControlEvents;
                cbStudioSetPartSettings1Category.SelectedIndex = 0;
            }

        }

        private void cbStudioSetPartSettings1Category_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartSettings1Category_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1Category(cbStudioSetPartSettings1Category.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings1Category(Int32 p, Int32 part)
        {
            if (MidiChannelIsSameAsPart())
            {
                Boolean currentHandleControlEvents = handleControlEvents;
                PushHandleControlEvents();
                PopulateCbStudioSetPartSettings1Program();
                handleControlEvents = currentHandleControlEvents;
                cbStudioSetPartSettings1Program.SelectedIndex = 0;
            }

        }

        private void cbStudioSetPartSettings1Program_SelectionChanged(object sender, EventArgs e)
        {
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1Program(cbStudioSetPartSettings1Program.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings1Program(Int32 p, Int32 part)
        {
            if (MidiChannelIsSameAsPart())
            {
                try
                {
                    UpdateToneFromControls();
                }
                catch { }
            }
        }


        private void PopulateStudioSetSelector()
        {
            // All studio set names has been received and stored in studioSetNames,
            // populate the combobox:
            cbStudioSetSelector.Items.Clear();
            cbStudioSetSlot.Items.Clear();
            UInt16 i = 1;
            foreach (String s in commonState.StudioSetNames)
            {
                String num = i.ToString();
                if (num.Length < 2)
                {
                    num = "0" + num;
                }
                cbStudioSetSelector.Items.Add(num + " " + s);
                //if (i > 16)
                //{
                    cbStudioSetSlot.Items.Add(s);
                //}
                i++;
            }
            cbStudioSetSelector.SelectedIndex = commonState.CurrentStudioSet;
            cbStudioSetSlot.SelectedIndex = commonState.CurrentStudioSet; // > 15 ? commonState.CurrentStudioSet - 16 : 0;
        }

        private void ScanForStudioSetNames()
        {
            t.Trace("private void ScanForStudioSetNames()");
            // Set studio set according to currentStudioSetNumber:
            SetStudioSet(studioSetNumberTemp);

            // Request studio set name:
            byte[] address = { 0x18, 0x00, 0x00, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x10 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // Answer will be caught in MidiInPort_MessageReceived.
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_TITLES;
        }

        private void QueryCurrentStudioSetNumber(Boolean scan = true)
        {
            t.Trace("private void QueryCurrentStudioSetNumber (" + "Boolean" + scan + ", " + ")");
            // If this is the first time (scan = true)
            // We must iterate all studio sets on the INTEGRA-7 in order to get the names.
            // Get the currently set studio set in order to restore it when done iterating,
            // or, if this is not first time, to set selector correct:
            if (scan)
            {
                currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER_AND_SCAN;
            }
            else
            {
                currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER;
            }
            studioSetEditor_State = StudioSetEditor_State.QUERYING_CURRENT_STUDIO_SET_NUMBER;
            byte[] address = { 0x01, 0x00, 0x00, 0x04 };
            byte[] length = { 0x00, 0x00, 0x00, 0x03 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // Answer will be caught in MidiInPort_MessageReceived.
        }

        private void QuerySystemCommon()
        {
            t.Trace("private void QuerySystemCommon()");
            // Ask for system common parameters:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_SYSTEM_COMMON;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.SYSTEM_COMMON;
            byte[] address = { 0x02, 0x00, 0x00, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x2f };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetCommon()
        {
            t.Trace("private void QueryStudioSetCommon()");
            // Ask for current studio set common:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_COMMON;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_COMMON;
            byte[] address = { 0x18, 0x00, 0x00, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x54 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        //private void QueryStudioSetChorus(/*Int32 Selection = 0*/)
        //{
        //    t.Trace("private void QueryStudioSetChorus ("/* + "Int32" + Selection + ", "*/ + ")");
        //    // Ask for current studio set common:
        //    studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS;
        //    //switch (Selection)
        //    //{
        //    //    case 0:
        //    //        studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS;
        //    //        currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS;
        //    //        break;
        //    //    case 1:
        //    //        studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS_CHORUS;
        //    //        currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_CHORUS;
        //    //        break;
        //    //    case 2:
        //    //        studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS_DELAY;
        //    //        currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_DELAY;
        //    //        break;
        //    //    case 3:
        //    //        studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS_GM2_CHORUS;
        //    //        currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_GM2_CHORUS;
        //    //        break;
        //    //}
        //    byte[] address = { 0x18, 0x00, 0x04, 0x00 };
        //    byte[] length = { 0x00, 0x00, 0x00, 0x54 };
        //    byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
        //    waitingForResponseFromIntegra7 = 500;
        //    commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        //}

        //private void QueryStudioSetReverb(Int32 Selection = 0)
        //{
        //    t.Trace("private void QueryStudioSetReverb (" + "Int32" + Selection + ", " + ")");
        //    // Ask for current studio set common:
        //    switch (Selection)
        //    {
        //        case 0:
        //            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_OFF;
        //            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_OFF;
        //            Reverb.IsVisible = false;
        //            StudioSetReverbGM2.IsVisible = false;
        //            break;
        //        case 1:
        //            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_ROOM1;
        //            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_ROOM1;
        //            Reverb.IsVisible = true;
        //            StudioSetReverbGM2.IsVisible = false;
        //            break;
        //        case 2:
        //            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_ROOM2;
        //            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_ROOM2;
        //            Reverb.IsVisible = true;
        //            StudioSetReverbGM2.IsVisible = false;
        //            break;
        //        case 3:
        //            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_HALL1;
        //            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_HALL1;
        //            Reverb.IsVisible = true;
        //            StudioSetReverbGM2.IsVisible = false;
        //            break;
        //        case 4:
        //            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_HALL2;
        //            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_HALL2;
        //            Reverb.IsVisible = true;
        //            StudioSetReverbGM2.IsVisible = false;
        //            break;
        //        case 5: // Intentional fall-through! Those types have the same parameters and use the same xaml controls, just values differs.
        //            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_PLATE;
        //            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_PLATE;
        //            Reverb.IsVisible = true;
        //            StudioSetReverbGM2.IsVisible = false;
        //            break;
        //        case 6:
        //            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_GM2_REVERB;
        //            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_GM2_REVERB;
        //            Reverb.IsVisible = false;
        //            StudioSetReverbGM2.IsVisible = true;
        //            break;
        //    }
        //    byte[] address = { 0x18, 0x00, 0x06, 0x00 };
        //    byte[] length = { 0x00, 0x00, 0x00, 0x63 };
        //    byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
        //    waitingForResponseFromIntegra7 = 500;
        //    commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        //}

        private void QueryStudioSetChorusChorus()
        {
            t.Trace("private void QueryStudioSetChorusChorus()");
            // Ask for current studio set common:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS_CHORUS;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_CHORUS;
            byte[] address = { 0x18, 0x00, 0x04, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x54 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetChorusDelay()
        {
            t.Trace("private void QueryStudioSetChorusChorus()");
            // Ask for current studio set common:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS_DELAY;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_DELAY;
            byte[] address = { 0x18, 0x00, 0x04, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x54 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetChorusGM2Chorus()
        {
            t.Trace("private void QueryStudioSetChorusChorus()");
            // Ask for current studio set common:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS_GM2_CHORUS;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_GM2_CHORUS;
            byte[] address = { 0x18, 0x00, 0x04, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x54 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetChorus()
        {
            t.Trace("private void QueryStudioSetChorus()");
            // Ask for current studio set common:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS;
            byte[] address = { 0x18, 0x00, 0x04, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x54 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetReverb()
        {
            t.Trace("private void QueryStudioSetReverb()");
            // Ask for current studio set reverb:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB;
            byte[] address = { 0x18, 0x00, 0x06, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x63 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetMotionalSurround()
        {
            t.Trace("private void QueryStudioSetMotionalSurround()");
            // Ask for current studio set reverb:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_MOTIONAL_SURROUND;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MOTIONAL_SURROUND;
            byte[] address = { 0x18, 0x00, 0x08, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x10 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetMasterEQ()
        {
            t.Trace("private void QueryStudioSetMasterEQ()");
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_MASTER_EQ;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MASTER_EQ;
            byte[] address = { 0x18, 0x00, 0x09, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x07 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetPart(Int32 partToRead = -1)
        {
            t.Trace("private void QueryStudioSetPart()");
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_PART;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART;
            byte[] address;
            if (partToRead > -1)
            {
                address = new byte[] { 0x18, 0x00, (byte)(0x20 + (byte)partToRead), 0x00 };
            }
            else
            {
                address = new byte[] { 0x18, 0x00, (byte)(0x20 + cbStudioSetPartSelector.SelectedIndex), 0x00 };
            }
            byte[] length = { 0x00, 0x00, 0x00, 0x4d };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetPartToneName()
        {
            t.Trace("private void QueryStudioSetPartToneName()");
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_PART_TONE_NAME;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_TONE_NAME;
            // Set tone on INTEGRA-7:
            byte msb = commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectMSB;
            byte lsb = commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB;
            byte pc = commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneProgramNumber;
            commonState.GetToneType(msb, lsb, pc);
            byte[] length = null;
            byte[] address = null;
            switch (commonState.SimpleToneType)
            {
                case CommonState.SimpleToneTypes.PCM_SYNTH_TONE:
                    address = hex2Midi.AddBytes128(new byte[] { 0x19, 0x00, 0x00, 0x00 },
                        hex2Midi.MultiplyBytes(0x20, (byte)cbStudioSetPartSelector.SelectedIndex, 2));
                    length = new byte[] { 0x00, 0x00, 0x00, 0x50 };
                    break;
                case CommonState.SimpleToneTypes.PCM_DRUM_KIT:
                    address = hex2Midi.AddBytes128(new byte[] { 0x19, 0x10, 0x00, 0x00 },
                        hex2Midi.MultiplyBytes(0x20, (byte)cbStudioSetPartSelector.SelectedIndex, 2));
                    length = new byte[] { 0x00, 0x00, 0x00, 0x12 };
                    break;
                case CommonState.SimpleToneTypes.SUPERNATURAL_ACOUSTIC_TONE:
                    address = hex2Midi.AddBytes128(new byte[] { 0x19, 0x02, 0x00, 0x00 },
                        hex2Midi.MultiplyBytes(0x20, (byte)cbStudioSetPartSelector.SelectedIndex, 2));
                    length = new byte[] { 0x00, 0x00, 0x00, 0x46 };
                    break;
                case CommonState.SimpleToneTypes.SUPERNATURAL_SYNTH_TONE:
                    address = hex2Midi.AddBytes128(new byte[] { 0x19, 0x01, 0x00, 0x00 },
                        hex2Midi.MultiplyBytes(0x20, (byte)cbStudioSetPartSelector.SelectedIndex, 2));
                    length = new byte[] { 0x00, 0x00, 0x00, 0x40 };
                    break;
                case CommonState.SimpleToneTypes.SUPERNATURAL_DRUM_KIT:
                    address = hex2Midi.AddBytes128(new byte[] { 0x19, 0x03, 0x00, 0x00 },
                        hex2Midi.MultiplyBytes(0x20, (byte)cbStudioSetPartSelector.SelectedIndex, 2));
                    length = new byte[] { 0x00, 0x00, 0x00, 0x14 };
                    break;
            }
            byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(message); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetPartMidiPhaselock()
        {
            t.Trace("private void QueryStudioSetPartMidiPhaselock()");
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_PART_MIDI_PHASELOCK;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_MIDI_PHASELOCK;
            byte[] address = { 0x18, 0x00, (byte)(0x10 + cbStudioSetPartSelector.SelectedIndex), 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x01 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetPartEQ(Int32 partToRead = -1)
        {
            t.Trace("private void QueryStudioSetPartEQ()");
            if (partToRead == -1)
            {
                partToRead = cbStudioSetPartSelector.SelectedIndex;
            }
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_PART_EQ;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_EQ;
            byte[] address = { 0x18, 0x00, (byte)(0x50 + (byte)partToRead), 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x08 };
            byte[] bytes = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void ReadSystemCommon()
        {
            t.Trace("private void ReadSystemCommon()");
            try
            {
                // Unpack system common parameters and update controls:
                commonState.StudioSet.SystemCommon = new StudioSet_SystemCommon(new ReceivedData(rawData));
                PushHandleControlEvents();
                slSystemCommonMasterTune.Value = commonState.StudioSet.SystemCommon.MasterTune;
                tbSystemCommonMasterTune.Text = "Master tune " + (slSystemCommonMasterTune.Value / 10).ToString() + " cent";
                slSystemCommonMasterKeyShift.Value = commonState.StudioSet.SystemCommon.MasterKeyShift;
                tbSystemCommonMasterKeyShift.Text = "Master key shift " + slSystemCommonMasterKeyShift.Value.ToString() + " keys";
                slSystemCommonMasterLevel.Value = commonState.StudioSet.SystemCommon.MasterLevel;
                tbSystemCommonMasterLevel.Text = "Master level " + slSystemCommonMasterLevel.Value.ToString();
                cbSystemCommonScaleTuneSwitch.IsChecked = commonState.StudioSet.SystemCommon.ScaleTuneSwitch;
                cbSystemCommonStudioSetControlChannel.SelectedIndex = commonState.StudioSet.SystemCommon.StudioSetControlChannel;
                cbSystemCommonSystemControlSource1.SelectedIndex = commonState.StudioSet.SystemCommon.SystemControl1Source;
                cbSystemCommonSystemControlSource2.SelectedIndex = commonState.StudioSet.SystemCommon.SystemControl2Source;
                cbSystemCommonSystemControlSource3.SelectedIndex = commonState.StudioSet.SystemCommon.SystemControl3Source;
                cbSystemCommonSystemControlSource4.SelectedIndex = commonState.StudioSet.SystemCommon.SystemControl4Source;
                cbSystemCommonControlSource.SelectedIndex = commonState.StudioSet.SystemCommon.ControlSource;
                cbSystemCommonSystemClockSource.SelectedIndex = commonState.StudioSet.SystemCommon.SystemClockSource;
                slSystemCommonSystemTempo.Value = commonState.StudioSet.SystemCommon.SystemTempo;
                cbSystemCommonTempoAssignSource.SelectedIndex = commonState.StudioSet.SystemCommon.TempoAssignSource;
                cbSystemCommonReceiveProgramChange.IsChecked = commonState.StudioSet.SystemCommon.ReceiveProgramChange;
                cbSystemCommonReceiveBankSelect.IsChecked = commonState.StudioSet.SystemCommon.ReceiveBankSelect;
                cbSystemCommonSurroundCenterSpeakerSwitch.IsChecked = commonState.StudioSet.SystemCommon.SurroundCenterSpeakerSwitch;
                cbSystemCommonSurroundSubWooferSwitch.IsChecked = commonState.StudioSet.SystemCommon.SurroundSubWooferSwitch;
                cbSystemCommonStereoOutputMode.SelectedIndex = commonState.StudioSet.SystemCommon.StereoOutputMode;
                PopHandleControlEvents();
            }
            catch (Exception e)
            {

            }
        }

        private void ReadSelectedStudioSet()
        {
            t.Trace("private void ReadSelectedStudioSet()");
            PushHandleControlEvents();
            try
            {
                // Unpack studio set common and update controls:
                commonState.StudioSet.Common = new StudioSet_Common(new ReceivedData(rawData));
                cbStudioSetSelector.SelectedItem = commonState.StudioSet.Common.Name;
                slVoiceReserve01.Value = commonState.StudioSet.Common.VoiceReserve[0];
                slVoiceReserve02.Value = commonState.StudioSet.Common.VoiceReserve[1];
                slVoiceReserve03.Value = commonState.StudioSet.Common.VoiceReserve[2];
                slVoiceReserve04.Value = commonState.StudioSet.Common.VoiceReserve[3];
                slVoiceReserve05.Value = commonState.StudioSet.Common.VoiceReserve[4];
                slVoiceReserve06.Value = commonState.StudioSet.Common.VoiceReserve[5];
                slVoiceReserve07.Value = commonState.StudioSet.Common.VoiceReserve[6];
                slVoiceReserve08.Value = commonState.StudioSet.Common.VoiceReserve[7];
                slVoiceReserve09.Value = commonState.StudioSet.Common.VoiceReserve[8];
                slVoiceReserve10.Value = commonState.StudioSet.Common.VoiceReserve[9];
                slVoiceReserve11.Value = commonState.StudioSet.Common.VoiceReserve[10];
                slVoiceReserve12.Value = commonState.StudioSet.Common.VoiceReserve[11];
                slVoiceReserve13.Value = commonState.StudioSet.Common.VoiceReserve[12];
                slVoiceReserve14.Value = commonState.StudioSet.Common.VoiceReserve[13];
                slVoiceReserve15.Value = commonState.StudioSet.Common.VoiceReserve[14];
                slVoiceReserve16.Value = commonState.StudioSet.Common.VoiceReserve[15];
                cbToneControl1.SelectedIndex = commonState.StudioSet.Common.ToneControlSource[0];
                cbToneControl2.SelectedIndex = commonState.StudioSet.Common.ToneControlSource[1];
                cbToneControl3.SelectedIndex = commonState.StudioSet.Common.ToneControlSource[2];
                cbToneControl4.SelectedIndex = commonState.StudioSet.Common.ToneControlSource[3];
                slTempo.Value = commonState.StudioSet.Common.Tempo;
                cbSoloPart.SelectedIndex = commonState.StudioSet.Common.SoloPart;
                cbReverb.IsChecked = commonState.StudioSet.Common.Reverb;
                cbChorus.IsChecked = commonState.StudioSet.Common.Chorus;
                cbMasterEQ.IsChecked = commonState.StudioSet.Common.MasterEqualizer;
                cbDrumCompEQ.IsChecked = commonState.StudioSet.Common.DrumCompressorAndEqualizer;
                cbDrumCompEQPart.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerPart;
                cbDrumCompEQ1OutputAssign.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[0];
                cbDrumCompEQ2OutputAssign.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[1];
                cbDrumCompEQ3OutputAssign.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[2];
                cbDrumCompEQ4OutputAssign.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[3];
                cbDrumCompEQ5OutputAssign.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[4];
                cbDrumCompEQ6OutputAssign.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[5];
                slExtPartLevel.Value = commonState.StudioSet.Common.ExternalPartLevel;
                slExtPartChorusSend.Value = commonState.StudioSet.Common.ExternalPartChorusSendLevel;
                slExtPartReverbSend.Value = commonState.StudioSet.Common.ExternalPartReverbSendLevel;
                cbExtPartMute.IsChecked = commonState.StudioSet.Common.ExternalPartMute;
            }
            catch (Exception e)
            {

            }
            PopHandleControlEvents();
        }

        private void ReadStudioSetChorus()
        {
            t.Trace("private void ReadStudioSetChorus()");

            //// I-7 does not reveal its internal storage, so parameters displayed might not be 
            //// the same as we got in rawData.
            //// Try resetting data in I-7 to have it in sync:
            //commonState.midi.SendSystemExclusive(rawData);
            
            ////commonState.studioSet.CommonChorus = new StudioSet_CommonChorus(new ReceivedData(rawData));
            ////cbStudioSetChorusType.SelectedIndex = -1;
            ////cbStudioSetChorusType.SelectedIndex = commonState.studioSet.CommonChorus.Type;
            ////ReadStudioSetChorus(commonState.studioSet.CommonChorus.Type);
            ReadStudioSetChorus(rawData[11]);
        }

        private void UpdateStudioSetChorusOffControls()
        {
            UpdateStudioSetChorusControls(0);
        }

        private void UpdateStudioSetChorusChorusControls()
        {
            UpdateStudioSetChorusControls(3);
        }

        private void UpdateStudioSetChorusDelayControls()
        {
            UpdateStudioSetChorusControls(3);
        }

        private void UpdateStudioSetChorusGM2ChorusControls()
        {
            UpdateStudioSetChorusControls(3);
        }

        private void ReadStudioSetChorus(byte Selection)
        {
            t.Trace("private void ReadStudioSetChorus (" + "byte" + Selection + ", " + ")");
            commonState.StudioSet.CommonChorus = new StudioSet_CommonChorus(new ReceivedData(rawData));
            UpdateStudioSetChorusControls(rawData[11]);
        }

        private void UpdateStudioSetChorusControls(byte Selection)
        {
            PushHandleControlEvents();
            cbStudioSetChorusType.SelectedIndex = Selection;
            slChorusLevel.Value = commonState.StudioSet.CommonChorus.Level;
            cbChorusOutputAssign.SelectedIndex = commonState.StudioSet.CommonChorus.OutputAssign;
            cbChorusOutputSelect.SelectedIndex = commonState.StudioSet.CommonChorus.OutputSelect;
            try
            {
                switch (Selection)
                {
                    case 1:
                        // Type is chorus
                        //cbChorusChorusFilterType.SelectedIndex = -1;
                        cbChorusChorusFilterType.SelectedIndex = commonState.StudioSet.CommonChorus.Chorus.FilterType;
                        //cbChorusChorusFilterCutoffFrequency.SelectedIndex = -1;
                        cbChorusChorusFilterCutoffFrequency.SelectedIndex = commonState.StudioSet.CommonChorus.Chorus.FilterCutoffFrequency;
                        tbChorusChorusPreDelay.Text = CalculateChorusPreDelay(commonState.StudioSet.CommonChorus.Chorus.PreDelay);
                        slChorusChorusPreDelay.Value = commonState.StudioSet.CommonChorus.Chorus.PreDelay;
                        //cbChorusChorusRateHzNote.SelectedIndex = -1;
                        cbChorusChorusRateHzNote.SelectedIndex = commonState.StudioSet.CommonChorus.Chorus.RateHzNote;
                        ChorusChorus.IsVisible = true;
                        tbChorusChorusRateHz.Text = "Rate " + CalculateTimeHz(commonState.StudioSet.CommonChorus.Chorus.RateHz) + " Hz";
                        slChorusChorusRateHz.Value = (Double)(16 * rawData[33] + rawData[34]) / 20.0;
                        tbChorusChorusRateNote.Text = "Rate " + CalculateTimeNote(commonState.StudioSet.CommonChorus.Chorus.RateNote) + " note";
                        slChorusChorusRateNote.Value = 16 * rawData[33] + rawData[34];
                        switch (commonState.StudioSet.CommonChorus.Chorus.RateHzNote)
                        {
                            case 0:
                                slChorusChorusRateNote.IsVisible = false;
                                tbChorusChorusRateNote.IsVisible = false;
                                slChorusChorusRateHz.IsVisible = true;
                                tbChorusChorusRateHz.IsVisible = true;
                                break;
                            case 1:
                                slChorusChorusRateHz.IsVisible = false;
                                tbChorusChorusRateHz.IsVisible = false;
                                slChorusChorusRateNote.IsVisible = true;
                                tbChorusChorusRateNote.IsVisible = true;
                                break;
                        }
                        slChorusChorusDepth.Value = commonState.StudioSet.CommonChorus.Chorus.Depth;
                        tbChorusChorusDepth.Text = "Depth: " + commonState.StudioSet.CommonChorus.Chorus.Depth.ToString();
                        slChorusChorusPhase.Value = commonState.StudioSet.CommonChorus.Chorus.Phase;
                        tbChorusChorusPhase.Text = "Phase: " + (commonState.StudioSet.CommonChorus.Chorus.Phase * 2).ToString();
                        slChorusChorusFeedback.Value = commonState.StudioSet.CommonChorus.Chorus.Feedback;
                        tbChorusChorusFeedback.Text = "Feedback: " + commonState.StudioSet.CommonChorus.Chorus.Feedback.ToString();
                        break;
                    case 2:
                        // Type is delay
                        //cbChorusDelayLeftMsNote.SelectedIndex = -1;
                        cbChorusDelayLeftMsNote.SelectedIndex = commonState.StudioSet.CommonChorus.Delay.LeftMsNote;
                        slChorusDelayLeftHz.Value = commonState.StudioSet.CommonChorus.Delay.LeftMs;
                        slChorusDelayLeftNote.Value = commonState.StudioSet.CommonChorus.Delay.LeftNote;
                        switch (commonState.StudioSet.CommonChorus.Delay.LeftMsNote)
                        {
                            case 0:
                                tbChorusDelayLeftHz.IsVisible = true;
                                slChorusDelayLeftHz.IsVisible = true;
                                tbChorusDelayLeftNote.IsVisible = false;
                                slChorusDelayLeftNote.IsVisible = false;
                                break;
                            case 1:
                                tbChorusDelayLeftNote.IsVisible = true;
                                slChorusDelayLeftNote.IsVisible = true;
                                tbChorusDelayLeftHz.IsVisible = false;
                                slChorusDelayLeftHz.IsVisible = false;
                                break;
                        }
                        //cbChorusDelayRightMsNote.SelectedIndex = -1;
                        cbChorusDelayRightMsNote.SelectedIndex = commonState.StudioSet.CommonChorus.Delay.RightMsNote;
                        slChorusDelayRightHz.Value = commonState.StudioSet.CommonChorus.Delay.RightMs;
                        slChorusDelayRightNote.Value = commonState.StudioSet.CommonChorus.Delay.RightNote;
                        switch (commonState.StudioSet.CommonChorus.Delay.RightMsNote)
                        {
                            case 0:
                                tbChorusDelayRightNote.IsVisible = false;
                                slChorusDelayRightNote.IsVisible = false;
                                tbChorusDelayRightHz.IsVisible = true;
                                slChorusDelayRightHz.IsVisible = true;
                                break;
                            case 1:
                                tbChorusDelayRightHz.IsVisible = false;
                                slChorusDelayRightHz.IsVisible = false;
                                tbChorusDelayRightNote.IsVisible = true;
                                slChorusDelayRightNote.IsVisible = true;
                                break;
                        }
                        //cbChorusDelayCenterMsNote.SelectedIndex = -1;
                        cbChorusDelayCenterMsNote.SelectedIndex = commonState.StudioSet.CommonChorus.Delay.CenterMsNote;
                        slChorusDelayCenterHz.Value = commonState.StudioSet.CommonChorus.Delay.CenterMs;
                        slChorusDelayCenterNote.Value = commonState.StudioSet.CommonChorus.Delay.CenterNote;
                        switch (commonState.StudioSet.CommonChorus.Delay.CenterMsNote)
                        {
                            case 0:
                                tbChorusDelayCenterNote.IsVisible = false;
                                slChorusDelayCenterNote.IsVisible = false;
                                tbChorusDelayCenterHz.IsVisible = true;
                                slChorusDelayCenterHz.IsVisible = true;
                                break;
                            case 1:
                                tbChorusDelayCenterHz.IsVisible = false;
                                slChorusDelayCenterHz.IsVisible = false;
                                tbChorusDelayCenterNote.IsVisible = true;
                                slChorusDelayCenterNote.IsVisible = true;
                                break;
                        }
                        slChorusDelayCenterFeedback.Value = 2 * (commonState.StudioSet.CommonChorus.Delay.CenterFeedback - 49);
                        //cbChorusDelayHFDamp.SelectedIndex = -1;
                        cbChorusDelayHFDamp.SelectedIndex = commonState.StudioSet.CommonChorus.Delay.HFDamp;
                        slChorusDelayLeftLevel.Value = commonState.StudioSet.CommonChorus.Delay.LeftLevel;
                        slChorusDelayRightLevel.Value = commonState.StudioSet.CommonChorus.Delay.LeftLevel;
                        slChorusDelayCenterLevel.Value = commonState.StudioSet.CommonChorus.Delay.CenterLevel;
                        ChorusDelay.IsVisible = true;
                        break;
                    case 3:
                        // Type is GM2 chorus
                        slChorusGM2ChorusPreLPF.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.PreLPF;
                        tbChorusGM2ChorusPreLPF.Text = "Pre-LPF " + slChorusGM2ChorusPreLPF.Value.ToString();
                        slChorusGM2ChorusLevel.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Level;
                        tbChorusGM2ChorusLevel.Text = "Level " + slChorusGM2ChorusLevel.Value.ToString();
                        slChorusGM2ChorusFeedback.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Feedback;
                        tbChorusGM2ChorusFeedback.Text = "Feedback " + slChorusGM2ChorusFeedback.Value.ToString();
                        slChorusGM2ChorusDelay.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Delay;
                        tbChorusGM2ChorusDelay.Text = "Delay " + slChorusGM2ChorusDelay.Value.ToString();
                        slChorusGM2ChorusRate.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Rate;
                        tbChorusGM2ChorusRate.Text = "Rate " + slChorusGM2ChorusRate.Value.ToString();
                        slChorusGM2ChorusDepth.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Depth;
                        tbChorusGM2ChorusDepth.Text = "Depth " + slChorusGM2ChorusDepth.Value.ToString();
                        slChorusGM2ChorusSendLevelToReverb.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.SendLevelToReverb;
                        tbChorusGM2ChorusSendLevelToReverb.Text = "Send level to reverb " + slChorusGM2ChorusSendLevelToReverb.Value.ToString();
                        break;
                }
            }
            catch { }
            PopHandleControlEvents();
        }

        private void ReadStudioSetReverb()
        {
            t.Trace("private void ReadStudioSetReverb()");
            //commonState.studioSet.CommonReverb = new StudioSet_CommonReverb(new ReceivedData(rawData));
            //cbStudioSetReverbType.SelectedIndex = -1;
            //cbStudioSetReverbType.SelectedIndex = commonState.studioSet.CommonReverb.Type;
            //ReadStudioSetReverb(commonState.studioSet.CommonReverb.Type);
            ReadStudioSetReverb(rawData[11]);
        }

        private void ReadStudioSetReverb(byte Selection)
        {
            t.Trace("private void ReadStudioSetReverb (" + "byte" + Selection + ", " + ")");
            commonState.StudioSet.CommonReverb = new StudioSet_CommonReverb(new ReceivedData(rawData));
            PushHandleControlEvents();
            cbStudioSetReverbType.SelectedIndex = Selection;
            slStudioSetReverbLevel.Value = commonState.StudioSet.CommonReverb.Level;
            cbStudioSetReverbOutputAssign.SelectedIndex = commonState.StudioSet.CommonReverb.OutputAssign;

            switch (Selection)
            {
                case 1:
                    slStudioSetReverbPreDelay.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.PreDelay;
                    tbStudioSetReverbPreDelay.Text = "Pre delay " + commonState.StudioSet.CommonReverb.ReverbRoom1.PreDelay.ToString();
                    slStudioSetReverbTime.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.Time;
                    tbStudioSetReverbTime.Text = "Time " + commonState.StudioSet.CommonReverb.ReverbRoom1.Time.ToString();
                    slStudioSetReverbDensity.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.Density;
                    tbStudioSetReverbDensity.Text = "Density " + commonState.StudioSet.CommonReverb.ReverbRoom1.Density.ToString();
                    slStudioSetReverbDiffusion.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.Diffusion;
                    tbStudioSetReverbDiffusion.Text = "Diffusion " + commonState.StudioSet.CommonReverb.ReverbRoom1.Diffusion.ToString();
                    slStudioSetReverbLFDamp.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.LFDamp;
                    tbStudioSetReverbLFDamp.Text = "LF damp " + commonState.StudioSet.CommonReverb.ReverbRoom1.LFDamp.ToString();
                    slStudioSetReverbHFDamp.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.HFDamp;
                    tbStudioSetReverbHFDamp.Text = "HF damp " + commonState.StudioSet.CommonReverb.ReverbRoom1.HFDamp.ToString();
                    slStudioSetReverbSpread.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.Spread;
                    tbStudioSetReverbSpread.Text = "Spread " + commonState.StudioSet.CommonReverb.ReverbRoom1.Spread.ToString();
                    slStudioSetReverbTone.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.Tone;
                    tbStudioSetReverbTone.Text = "Tone " + commonState.StudioSet.CommonReverb.ReverbRoom1.Tone.ToString();
                    break;
                case 2:
                    slStudioSetReverbPreDelay.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.PreDelay;
                    tbStudioSetReverbPreDelay.Text = "Pre delay " + commonState.StudioSet.CommonReverb.ReverbRoom2.PreDelay.ToString();
                    slStudioSetReverbTime.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.Time;
                    tbStudioSetReverbTime.Text = "Time " + commonState.StudioSet.CommonReverb.ReverbRoom2.Time.ToString();
                    slStudioSetReverbDensity.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.Density;
                    tbStudioSetReverbDensity.Text = "Density " + commonState.StudioSet.CommonReverb.ReverbRoom2.Density.ToString();
                    slStudioSetReverbDiffusion.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.Diffusion;
                    tbStudioSetReverbDiffusion.Text = "Diffusion " + commonState.StudioSet.CommonReverb.ReverbRoom2.Diffusion.ToString();
                    slStudioSetReverbLFDamp.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.LFDamp;
                    tbStudioSetReverbLFDamp.Text = "LF damp " + commonState.StudioSet.CommonReverb.ReverbRoom2.LFDamp.ToString();
                    slStudioSetReverbHFDamp.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.HFDamp;
                    tbStudioSetReverbHFDamp.Text = "HF damp " + commonState.StudioSet.CommonReverb.ReverbRoom2.HFDamp.ToString();
                    slStudioSetReverbSpread.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.Spread;
                    tbStudioSetReverbSpread.Text = "Spread " + commonState.StudioSet.CommonReverb.ReverbRoom2.Spread.ToString();
                    slStudioSetReverbTone.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.Tone;
                    tbStudioSetReverbTone.Text = "Tone " + commonState.StudioSet.CommonReverb.ReverbRoom2.Tone.ToString();
                    break;
                case 3:
                    slStudioSetReverbPreDelay.Value = commonState.StudioSet.CommonReverb.ReverbHall1.PreDelay;
                    tbStudioSetReverbPreDelay.Text = "Pre delay " + commonState.StudioSet.CommonReverb.ReverbHall1.PreDelay.ToString();
                    slStudioSetReverbTime.Value = commonState.StudioSet.CommonReverb.ReverbHall1.Time;
                    tbStudioSetReverbTime.Text = "Time " + commonState.StudioSet.CommonReverb.ReverbHall1.Time.ToString();
                    slStudioSetReverbDensity.Value = commonState.StudioSet.CommonReverb.ReverbHall1.Density;
                    tbStudioSetReverbDensity.Text = "Density " + commonState.StudioSet.CommonReverb.ReverbHall1.Density.ToString();
                    slStudioSetReverbDiffusion.Value = commonState.StudioSet.CommonReverb.ReverbHall1.Diffusion;
                    tbStudioSetReverbDiffusion.Text = "Diffusion " + commonState.StudioSet.CommonReverb.ReverbHall1.Diffusion.ToString();
                    slStudioSetReverbLFDamp.Value = commonState.StudioSet.CommonReverb.ReverbHall1.LFDamp;
                    tbStudioSetReverbLFDamp.Text = "LF damp " + commonState.StudioSet.CommonReverb.ReverbHall1.LFDamp.ToString();
                    slStudioSetReverbHFDamp.Value = commonState.StudioSet.CommonReverb.ReverbHall1.HFDamp;
                    tbStudioSetReverbHFDamp.Text = "HF damp " + commonState.StudioSet.CommonReverb.ReverbHall1.HFDamp.ToString();
                    slStudioSetReverbSpread.Value = commonState.StudioSet.CommonReverb.ReverbHall1.Spread;
                    tbStudioSetReverbSpread.Text = "Spread " + commonState.StudioSet.CommonReverb.ReverbHall1.Spread.ToString();
                    slStudioSetReverbTone.Value = commonState.StudioSet.CommonReverb.ReverbHall1.Tone;
                    tbStudioSetReverbTone.Text = "Tone " + commonState.StudioSet.CommonReverb.ReverbHall1.Tone.ToString();
                    break;
                case 4:
                    slStudioSetReverbPreDelay.Value = commonState.StudioSet.CommonReverb.ReverbHall2.PreDelay;
                    tbStudioSetReverbPreDelay.Text = "Pre delay " + commonState.StudioSet.CommonReverb.ReverbHall2.PreDelay.ToString();
                    slStudioSetReverbTime.Value = commonState.StudioSet.CommonReverb.ReverbHall2.Time;
                    tbStudioSetReverbTime.Text = "Time " + commonState.StudioSet.CommonReverb.ReverbHall2.Time.ToString();
                    slStudioSetReverbDensity.Value = commonState.StudioSet.CommonReverb.ReverbHall2.Density;
                    tbStudioSetReverbDensity.Text = "Density " + commonState.StudioSet.CommonReverb.ReverbHall2.Density.ToString();
                    slStudioSetReverbDiffusion.Value = commonState.StudioSet.CommonReverb.ReverbHall2.Diffusion;
                    tbStudioSetReverbDiffusion.Text = "Diffusion " + commonState.StudioSet.CommonReverb.ReverbHall2.Diffusion.ToString();
                    slStudioSetReverbLFDamp.Value = commonState.StudioSet.CommonReverb.ReverbHall2.LFDamp;
                    tbStudioSetReverbLFDamp.Text = "LF damp " + commonState.StudioSet.CommonReverb.ReverbHall2.LFDamp.ToString();
                    slStudioSetReverbHFDamp.Value = commonState.StudioSet.CommonReverb.ReverbHall2.HFDamp;
                    tbStudioSetReverbHFDamp.Text = "HF damp " + commonState.StudioSet.CommonReverb.ReverbHall2.HFDamp.ToString();
                    slStudioSetReverbSpread.Value = commonState.StudioSet.CommonReverb.ReverbHall2.Spread;
                    tbStudioSetReverbSpread.Text = "Spread " + commonState.StudioSet.CommonReverb.ReverbHall2.Spread.ToString();
                    slStudioSetReverbTone.Value = commonState.StudioSet.CommonReverb.ReverbHall2.Tone;
                    tbStudioSetReverbTone.Text = "Tone " + commonState.StudioSet.CommonReverb.ReverbHall2.Tone.ToString();
                    break;
                case 5:
                    slStudioSetReverbPreDelay.Value = commonState.StudioSet.CommonReverb.ReverbPlate.PreDelay;
                    tbStudioSetReverbPreDelay.Text = "Pre delay " + commonState.StudioSet.CommonReverb.ReverbPlate.PreDelay.ToString();
                    slStudioSetReverbTime.Value = commonState.StudioSet.CommonReverb.ReverbPlate.Time;
                    tbStudioSetReverbTime.Text = "Time " + commonState.StudioSet.CommonReverb.ReverbPlate.Time.ToString();
                    slStudioSetReverbDensity.Value = commonState.StudioSet.CommonReverb.ReverbPlate.Density;
                    tbStudioSetReverbDensity.Text = "Density " + commonState.StudioSet.CommonReverb.ReverbPlate.Density.ToString();
                    slStudioSetReverbDiffusion.Value = commonState.StudioSet.CommonReverb.ReverbPlate.Diffusion;
                    tbStudioSetReverbDiffusion.Text = "Diffusion " + commonState.StudioSet.CommonReverb.ReverbPlate.Diffusion.ToString();
                    slStudioSetReverbLFDamp.Value = commonState.StudioSet.CommonReverb.ReverbPlate.LFDamp;
                    tbStudioSetReverbLFDamp.Text = "LF damp " + commonState.StudioSet.CommonReverb.ReverbPlate.LFDamp.ToString();
                    slStudioSetReverbHFDamp.Value = commonState.StudioSet.CommonReverb.ReverbPlate.HFDamp;
                    tbStudioSetReverbHFDamp.Text = "HF damp " + commonState.StudioSet.CommonReverb.ReverbPlate.HFDamp.ToString();
                    slStudioSetReverbSpread.Value = commonState.StudioSet.CommonReverb.ReverbPlate.Spread;
                    tbStudioSetReverbSpread.Text = "Spread " + commonState.StudioSet.CommonReverb.ReverbPlate.Spread.ToString();
                    slStudioSetReverbTone.Value = commonState.StudioSet.CommonReverb.ReverbPlate.Tone;
                    tbStudioSetReverbTone.Text = "Tone " + commonState.StudioSet.CommonReverb.ReverbPlate.Tone.ToString();
                    break;
                case 6:
                    slStudioSetReverbGM2Character.Value = commonState.StudioSet.CommonReverb.GM2Reverb.Character;
                    tbStudioSetReverbGM2Time.Text = "Character " + commonState.StudioSet.CommonReverb.GM2Reverb.Character.ToString();
                    slStudioSetReverbGM2Time.Value = commonState.StudioSet.CommonReverb.GM2Reverb.Time;
                    tbStudioSetReverbGM2Time.Text = "Time " + commonState.StudioSet.CommonReverb.GM2Reverb.Time.ToString();
                    break;
            }
            PopHandleControlEvents();
        }

        private void ReadMotionalSurround()
        {
            t.Trace("private void ReadMotionalSurround()");
            commonState.StudioSet.MotionalSurround = new StudioSet_MotionalSurround(new ReceivedData(rawData));
            PushHandleControlEvents();
            // Since this code is also called from the Motional surround editor, the controls below might not be
            // created. It's ok, since in that case we do not need them, so use try-catch:
            try
            {
                cbStudioSetMotionalSurround.IsChecked = commonState.StudioSet.MotionalSurround.MotionalSurroundSwitch;
                cbStudioSetMotionalSurroundRoomType.SelectedIndex = commonState.StudioSet.MotionalSurround.RoomType;
                slStudioSetMotionalSurroundAmbienceLevel.Value = commonState.StudioSet.MotionalSurround.AmbienceLevel;
                cbStudioSetMotionalSurroundRoomSize.SelectedIndex = commonState.StudioSet.MotionalSurround.RoomSize;
                slStudioSetMotionalSurroundAmbienceTime.Value = commonState.StudioSet.MotionalSurround.AmbienceTime;
                slStudioSetMotionalSurroundAmbienceDensity.Value = commonState.StudioSet.MotionalSurround.AmbienceDensity;
                slStudioSetMotionalSurroundAmbienceHFDamp.Value = commonState.StudioSet.MotionalSurround.AmbienceHFDamp;
                slStudioSetMotionalSurroundExternalPartLR.Value = commonState.StudioSet.MotionalSurround.ExtPartLR - 64;
                slStudioSetMotionalSurroundExternalPartFB.Value = commonState.StudioSet.MotionalSurround.ExtPartFB - 64;
                slStudioSetMotionalSurroundExtPartWidth.Value = commonState.StudioSet.MotionalSurround.ExtPartWidth;
                slStudioSetMotionalSurroundExtpartAmbienceSend.Value = commonState.StudioSet.MotionalSurround.ExtPartAmbienceSendLevel;
                cbStudioSetMotionalSurroundExtPartControl.SelectedIndex = commonState.StudioSet.MotionalSurround.ExtPartControlChannel;
                slStudioSetMotionalSurroundDepth.Value = commonState.StudioSet.MotionalSurround.MotionalSurroundDepth;
                tbStudioSetMotionalSurroundAmbienceLevel.Text = "Ambience level " + slStudioSetMotionalSurroundAmbienceLevel.Value.ToString();
                tbStudioSetMotionalSurroundAmbienceTime.Text = "Ambience time " + slStudioSetMotionalSurroundAmbienceTime.Value.ToString();
                tbStudioSetMotionalSurroundAmbienceDensity.Text = "Ambience density " + slStudioSetMotionalSurroundAmbienceDensity.Value.ToString();
                tbStudioSetMotionalSurroundAmbienceHFDamp.Text = "Ambience HF damp " + slStudioSetMotionalSurroundAmbienceHFDamp.Value.ToString();
                tbStudioSetMotionalSurroundExternalPartLR.Text = "External part L-R " + slStudioSetMotionalSurroundExternalPartLR.Value.ToString();
                tbStudioSetMotionalSurroundExternalPartFB.Text = "External part F-B " + slStudioSetMotionalSurroundExternalPartFB.Value.ToString();
                tbStudioSetMotionalSurroundExtPartWidth.Text = "External part width " + slStudioSetMotionalSurroundExtPartWidth.Value.ToString();
                tbStudioSetMotionalSurroundExtpartAmbienceSend.Text = "External part ambience send" + 
                    slStudioSetMotionalSurroundExtpartAmbienceSend.Value.ToString();
                tbStudioSetMotionalSurroundDepth.Text = "Motional surround depth " + slStudioSetMotionalSurroundDepth.Value.ToString();
            }
            catch { }
            PopHandleControlEvents();
        }

        private void ReadStudioSetMasterEQ()
        {
            t.Trace("private void ReadStudioSetMasterEQ()");
            commonState.StudioSet.MasterEQ = new StudioSet_MasterEQ(new ReceivedData(rawData));
            PushHandleControlEvents();
            cbStudioSetMasterEqLowFreq.SelectedIndex = commonState.StudioSet.MasterEQ.EQHighFreq;
            slStudioSetMasterEqLowGain.Value = commonState.StudioSet.MasterEQ.EQHighGain - 15;
            cbStudioSetMasterEqMidFreq.SelectedIndex = commonState.StudioSet.MasterEQ.EQMidFreq;
            slStudioSetMasterEqMidGain.Value = commonState.StudioSet.MasterEQ.EQMidGain - 15;
            cbStudioSetMasterEqMidQ.SelectedIndex = commonState.StudioSet.MasterEQ.EQMidQ;
            cbStudioSetMasterEqHighFreq.SelectedIndex = commonState.StudioSet.MasterEQ.EQHighFreq;
            slStudioSetMasterEqHighGain.Value = commonState.StudioSet.MasterEQ.EQHighGain - 15;
            tbStudioSetMasterEqLowGain.Text = "EQ low gain " + (slStudioSetMasterEqLowGain.Value).ToString() + " dB";
            tbStudioSetMasterEqMidGain.Text = "EQ mid gain " + (slStudioSetMasterEqMidGain.Value).ToString() + " dB";
            tbStudioSetMasterEqHighGain.Text = "EQ high gain " + (slStudioSetMasterEqHighGain.Value).ToString() + " dB";
            PopHandleControlEvents();
        }

        private void ReadStudioSetPart(Int32 partToRead = -1)
        {
            if (partToRead == -1)
            {
                partToRead = cbStudioSetPartSelector.SelectedIndex;
            }
            t.Trace("private void ReadStudioSetPart()");
            // This is a bit different since the read rawData is split into several classes.
            ReceivedData Data = new ReceivedData(rawData);
            commonState.StudioSet.PartMainSettings[(byte)partToRead] = new StudioSet_PartMainSettings(Data);
            commonState.StudioSet.PartKeyboard[(byte)partToRead] = new StudioSet_PartKeyboard(Data);
            commonState.StudioSet.PartScaleTune[(byte)partToRead] = new StudioSet_PartScaleTune(Data);
            commonState.StudioSet.PartMidi[(byte)partToRead] = new StudioSet_PartMidi(Data);
            commonState.StudioSet.PartMotionalSurround[(byte)partToRead] = new StudioSet_PartMotionalSurround(Data);
            PushHandleControlEvents();
            // Since this code is also called from the Motional surround editor, the controls below might not be
            // created. It's ok, since in that case we do not need them, so use try-catch:
            try
            {
                // Part settings 1 page
                if (cbStudioSetPartSelector.SelectedIndex > -1 && cbStudioSetPartSelector.SelectedIndex == partToRead)
                {
                    cbStudioSetPartSettings1ReceiveChannel.SelectedIndex = commonState.StudioSet.PartMainSettings[(byte)partToRead].ReceiveChannel;
                    cbStudioSetPartSettings1Receive.IsChecked = commonState.StudioSet.PartMainSettings[(byte)partToRead].ReceiveSwitch;
                    // These will be set after part is read (and the slider is now a ComboBox)
                    //cbStudioSetPartSettings1Group.SelectedIndex = MsbToCbIndex(commonState.studioSet.PartMainSettings[(byte)partToRead].ToneBankSelectMSB);
                    commonState.CurrentTone.BankMSB = commonState.StudioSet.PartMainSettings[(byte)partToRead].ToneBankSelectMSB;
                    //cbStudioSetPartSettings1Category.SelectedIndex = LsbToCbIndex(commonState.studioSet.PartMainSettings[(byte)partToRead].ToneBankSelectLSB);
                    commonState.CurrentTone.BankLSB = commonState.StudioSet.PartMainSettings[(byte)partToRead].ToneBankSelectLSB;
                    //cbStudioSetPartSettings1Program.SelectedIndex = commonState.studioSet.PartMainSettings[(byte)partToRead].ToneProgramNumber + 1;
                    commonState.CurrentTone.Program = commonState.StudioSet.PartMainSettings[(byte)partToRead].ToneProgramNumber;
                    slStudioSetPartSettings1Level.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].Level;
                    slStudioSetPartSettings1Pan.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].Pan - 64;
                    slStudioSetPartSettings1CoarseTune.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].CoarseTune - 64;
                    slStudioSetPartSettings1FineTune.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].FineTune - 64;
                    cbStudioSetPartSettings1MonoPoly.SelectedIndex = commonState.StudioSet.PartMainSettings[(byte)partToRead].MonoPoly;
                    cbStudioSetPartSettings1Legato.SelectedIndex = commonState.StudioSet.PartMainSettings[(byte)partToRead].Legato;
                    slStudioSetPartSettings1PitchBendRange.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].PitchBendRange;
                    cbStudioSetPartSettings1Portamento.SelectedIndex = commonState.StudioSet.PartMainSettings[(byte)partToRead].PortamentoSwitch;
                    slStudioSetPartSettings1PortamentoTime.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].PortamentoTime;
                    // Part settings 2 page
                    slStudioSetPartSettings2CutoffOffset.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].CutoffOffset - 64;
                    slStudioSetPartSettings2ResonanceOffset.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].ResonanceOffset - 64;
                    slStudioSetPartSettings2AttackTimeOffset.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].AttackTimeOffset - 64;
                    slStudioSetPartSettings2DecayTimeOffset.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].DecayTimeOffset - 64;
                    slStudioSetPartSettings2ResonanceOffset.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].ReleaseTimeOffset - 64;
                    slStudioSetPartSettings2VibratoRate.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].VibratoRate - 64;
                    slStudioSetPartSettings2VibratoDepth.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].VibratoDepth - 64;
                    slStudioSetPartSettings2VibratoDelay.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].VibratoDelay - 64;
                    // Part effects (continues previous page):
                    slStudioSetPartEffectsChorusSendLevel.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].ChorusSendLevel;
                    slStudioSetPartEffectsReverbSendLevel.Value = commonState.StudioSet.PartMainSettings[(byte)partToRead].ReverbSendLevel;
                    cbStudioSetPartEffectsOutputAssign.SelectedIndex = commonState.StudioSet.PartMainSettings[(byte)partToRead].OutputAssign;
                    // Part keyboard page
                    slStudioSetPartKeyboardOctaveShift.Value = commonState.StudioSet.PartKeyboard[(byte)partToRead].OctaveShift - 64;
                    slStudioSetPartKeyboardVelocitySenseOffset.Value = commonState.StudioSet.PartKeyboard[(byte)partToRead].VelocitySenseOffset - 64;
                    slStudioSetPartKeyboardRangeLower.Value = commonState.StudioSet.PartKeyboard[(byte)partToRead].RangeLower;
                    slStudioSetPartKeyboardRangeUpper.Value = commonState.StudioSet.PartKeyboard[(byte)partToRead].RangeUpper;
                    slStudioSetPartKeyboardFadeWidthLower.Value = commonState.StudioSet.PartKeyboard[(byte)partToRead].FadeWidthLower;
                    slStudioSetPartKeyboardFadeWidthUpper.Value = commonState.StudioSet.PartKeyboard[(byte)partToRead].FadeWidthUpper;
                    slStudioSetPartKeyboardVelocityRangeLower.Value = commonState.StudioSet.PartKeyboard[(byte)partToRead].VelocityRangeLower;
                    slStudioSetPartKeyboardVelocityRangeUpper.Value = commonState.StudioSet.PartKeyboard[(byte)partToRead].VelocityRangeUpper;
                    slStudioSetPartKeyboardVelocityFadeWidthLower.Value = commonState.StudioSet.PartKeyboard[(byte)partToRead].VelocityFadeWidthLower;
                    slStudioSetPartKeyboardVelocityFadeWidthUpper.Value = commonState.StudioSet.PartKeyboard[(byte)partToRead].VelocityFadeWidthUpper;
                    cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = commonState.StudioSet.PartKeyboard[(byte)partToRead].VelocityCurveType;
                    cbStudioSetPartKeyboardMute.IsChecked = commonState.StudioSet.PartKeyboard[(byte)partToRead].Mute;
                    // Scale tune page
                    cbStudioSetPartScaleTuneType.SelectedIndex = commonState.StudioSet.PartScaleTune[(byte)partToRead].Type;
                    cbStudioSetPartScaleTuneKey.SelectedIndex = commonState.StudioSet.PartScaleTune[(byte)partToRead].Key;
                    slStudioSetPartScaleTuneC.Value = commonState.StudioSet.PartScaleTune[(byte)partToRead].C - 64;
                    slStudioSetPartScaleTuneCi.Value = commonState.StudioSet.PartScaleTune[(byte)partToRead].Ci - 64;
                    slStudioSetPartScaleTuneD.Value = commonState.StudioSet.PartScaleTune[(byte)partToRead].D - 64;
                    slStudioSetPartScaleTuneDi.Value = commonState.StudioSet.PartScaleTune[(byte)partToRead].Di - 64;
                    slStudioSetPartScaleTuneE.Value = commonState.StudioSet.PartScaleTune[(byte)partToRead].E - 64;
                    slStudioSetPartScaleTuneF.Value = commonState.StudioSet.PartScaleTune[(byte)partToRead].F - 64;
                    slStudioSetPartScaleTuneFi.Value = commonState.StudioSet.PartScaleTune[(byte)partToRead].Fi - 64;
                    slStudioSetPartScaleTuneG.Value = commonState.StudioSet.PartScaleTune[(byte)partToRead].G - 64;
                    slStudioSetPartScaleTuneGi.Value = commonState.StudioSet.PartScaleTune[(byte)partToRead].Gi - 64;
                    slStudioSetPartScaleTuneA.Value = commonState.StudioSet.PartScaleTune[(byte)partToRead].A - 64;
                    slStudioSetPartScaleTuneAi.Value = commonState.StudioSet.PartScaleTune[(byte)partToRead].Ai - 64;
                    slStudioSetPartScaleTuneB.Value = commonState.StudioSet.PartScaleTune[(byte)partToRead].B - 64;
                    // Midi page (all but Phase lock)
                    cbStudioSetPartMidiReceiveProgramChange.IsChecked = commonState.StudioSet.PartMidi[(byte)partToRead].ReceiveProgramChange;
                    cbStudioSetPartMidiReceiveBankSelect.IsChecked = commonState.StudioSet.PartMidi[(byte)partToRead].ReceiveBankSelect;
                    cbStudioSetPartMidiReceivePitchBend.IsChecked = commonState.StudioSet.PartMidi[(byte)partToRead].ReceivePitchBend;
                    cbStudioSetPartMidiReceivePolyphonicKeyPressure.IsChecked = commonState.StudioSet.PartMidi[(byte)partToRead].ReceivePolyphonicKeyPressure;
                    cbStudioSetPartMidiReceiveChannelPressure.IsChecked = commonState.StudioSet.PartMidi[(byte)partToRead].ReceiveChannelPressure;
                    cbStudioSetPartMidiReceiveModulation.IsChecked = commonState.StudioSet.PartMidi[(byte)partToRead].ReceiveModulation;
                    cbStudioSetPartMidiReceiveVolume.IsChecked = commonState.StudioSet.PartMidi[(byte)partToRead].ReceiveVolume;
                    cbStudioSetPartMidiReceivePan.IsChecked = commonState.StudioSet.PartMidi[(byte)partToRead].ReceivePan;
                    cbStudioSetPartMidiReceiveExpression.IsChecked = commonState.StudioSet.PartMidi[(byte)partToRead].ReceiveExpression;
                    cbStudioSetPartMidiReceiveHold1.IsChecked = commonState.StudioSet.PartMidi[(byte)partToRead].ReceiveHold1;
                    // Motional surround page
                    slStudioSetPartMotionalSurroundLR.Value = commonState.StudioSet.PartMotionalSurround[(byte)partToRead].LR;
                    slStudioSetPartMotionalSurroundFB.Value = commonState.StudioSet.PartMotionalSurround[(byte)partToRead].FB;
                    slStudioSetPartMotionalSurroundWidth.Value = commonState.StudioSet.PartMotionalSurround[(byte)partToRead].Width;
                    slStudioSetPartMotionalSurroundAmbienceSendLevel.Value = commonState.StudioSet.PartMotionalSurround[(byte)partToRead].AmbienceSendLevel;
                    // Texts for sliders, Part settings 1
                    tbStudioSetPartSettings1Group.Text = "Bank select MSB " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].ToneBankSelectMSB).ToString();
                    tbStudioSetPartSettings1Category.Text = "Bank select LSB " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].ToneBankSelectLSB).ToString();
                    tbStudioSetPartSettings1Program.Text = "Program number " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].ToneProgramNumber + 1).ToString();
                    tbStudioSetPartSettings1Level.Text = "Level " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].Level).ToString();
                    tbStudioSetPartSettings1Pan.Text = "Pan " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].Pan - 64).ToString();
                    tbStudioSetPartSettings1CoarseTune.Text = "Coarse tune " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].CoarseTune - 64).ToString();
                    tbStudioSetPartSettings1FineTune.Text = "Fine tune " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].FineTune - 64).ToString();
                    tbStudioSetPartSettings1PitchBendRange.Text = "Pitch bend range " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].PitchBendRange).ToString();
                    tbStudioSetPartSettings1PortamentoTime.Text = "Portamento time " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].PortamentoTime).ToString();
                    // Part settings 2
                    tbStudioSetPartSettings2CutoffOffset.Text = "Cutoff offset " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].CutoffOffset - 64).ToString();
                    tbStudioSetPartSettings2ResonanceOffset.Text = "Resonance offset " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].ResonanceOffset - 64).ToString();
                    tbStudioSetPartSettings2AttackTimeOffset.Text = "Attack time offset " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].AttackTimeOffset - 64).ToString();
                    tbStudioSetPartSettings2DecayTimeOffset.Text = "Decay time offset " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].DecayTimeOffset - 64).ToString();
                    tbStudioSetPartSettings2ReleaseTimeOffset.Text = "Release time offset " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].ReleaseTimeOffset - 64).ToString();
                    tbStudioSetPartSettings2VibratoRate.Text = "Vibrato rate " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].VibratoRate - 64).ToString();
                    tbStudioSetPartSettings2VibratoDepth.Text = "Vibrato depth " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].VibratoDepth - 64).ToString();
                    tbStudioSetPartSettings2VibratoDelay.Text = "Vibrato delay " + (commonState.StudioSet.PartMainSettings[(byte)partToRead].VibratoDelay - 64).ToString();
                    // Part effects:
                    tbStudioSetPartEffectsChorusSendLevel.Text = "Chorus send level " + commonState.StudioSet.PartMainSettings[(byte)partToRead].ChorusSendLevel.ToString();
                    tbStudioSetPartEffectsReverbSendLevel.Text = "Reverb send level " + commonState.StudioSet.PartMainSettings[(byte)partToRead].ReverbSendLevel.ToString();
                    // Part keyboard:
                    tbStudioSetPartKeyboardOctaveShift.Text = "Octave shift " + (commonState.StudioSet.PartKeyboard[(byte)partToRead].OctaveShift - 64).ToString();
                    tbStudioSetPartKeyboardVelocitySenseOffset.Text = "Velocity sense offset " + (commonState.StudioSet.PartKeyboard[(byte)partToRead].VelocitySenseOffset - 63).ToString();
                    tbStudioSetPartKeyboardRangeLower.Text = "Range lower " + (commonState.StudioSet.PartKeyboard[(byte)partToRead].RangeLower).ToString();
                    tbStudioSetPartKeyboardRangeUpper.Text = "Range upper " + (commonState.StudioSet.PartKeyboard[(byte)partToRead].RangeUpper).ToString();
                    tbStudioSetPartKeyboardFadeWidthLower.Text = "Fade width lower " + (commonState.StudioSet.PartKeyboard[(byte)partToRead].FadeWidthLower).ToString();
                    tbStudioSetPartKeyboardFadeWidthUpper.Text = "Fade width uppper " + (commonState.StudioSet.PartKeyboard[(byte)partToRead].FadeWidthUpper).ToString();
                    tbStudioSetPartKeyboardVelocityRangeLower.Text = "Velocity range lower " + (commonState.StudioSet.PartKeyboard[(byte)partToRead].VelocityRangeLower).ToString();
                    tbStudioSetPartKeyboardVelocityRangeUpper.Text = "Velocity range upper " + (commonState.StudioSet.PartKeyboard[(byte)partToRead].VelocityRangeUpper).ToString();
                    tbStudioSetPartKeyboardVelocityFadeWidthLower.Text = "Vel. fade width lower " + (commonState.StudioSet.PartKeyboard[(byte)partToRead].VelocityFadeWidthLower).ToString();
                    tbStudioSetPartKeyboardVelocityFadeWidthUpper.Text = "Vel. fade width upper " + (commonState.StudioSet.PartKeyboard[(byte)partToRead].VelocityFadeWidthUpper).ToString();
                    // Scale tune:
                    tbStudioSetPartScaleTuneC.Text = "C " + (commonState.StudioSet.PartScaleTune[(byte)partToRead].C).ToString();
                    tbStudioSetPartScaleTuneCi.Text = "C# " + (commonState.StudioSet.PartScaleTune[(byte)partToRead].Ci).ToString();
                    tbStudioSetPartScaleTuneD.Text = "D " + (commonState.StudioSet.PartScaleTune[(byte)partToRead].D).ToString();
                    tbStudioSetPartScaleTuneDi.Text = "D# " + (commonState.StudioSet.PartScaleTune[(byte)partToRead].Di).ToString();
                    tbStudioSetPartScaleTuneE.Text = "E " + (commonState.StudioSet.PartScaleTune[(byte)partToRead].E).ToString();
                    tbStudioSetPartScaleTuneF.Text = "F " + (commonState.StudioSet.PartScaleTune[(byte)partToRead].F).ToString();
                    tbStudioSetPartScaleTuneFi.Text = "F# " + (commonState.StudioSet.PartScaleTune[(byte)partToRead].Fi).ToString();
                    tbStudioSetPartScaleTuneG.Text = "G " + (commonState.StudioSet.PartScaleTune[(byte)partToRead].G).ToString();
                    tbStudioSetPartScaleTuneGi.Text = "G# " + (commonState.StudioSet.PartScaleTune[(byte)partToRead].Gi).ToString();
                    tbStudioSetPartScaleTuneA.Text = "A " + (commonState.StudioSet.PartScaleTune[(byte)partToRead].A).ToString();
                    tbStudioSetPartScaleTuneAi.Text = "A# " + (commonState.StudioSet.PartScaleTune[(byte)partToRead].Ai).ToString();
                    tbStudioSetPartScaleTuneB.Text = "B " + (commonState.StudioSet.PartScaleTune[(byte)partToRead].B).ToString();
                    // Motional surround:
                    tbStudioSetPartMotionalSurroundLR.Text = "L-R " + (commonState.StudioSet.PartMotionalSurround[(byte)partToRead].LR).ToString();
                    tbStudioSetPartMotionalSurroundFB.Text = "F-B " + (commonState.StudioSet.PartMotionalSurround[(byte)partToRead].FB).ToString();
                    tbStudioSetPartMotionalSurroundWidth.Text = "Width " + (commonState.StudioSet.PartMotionalSurround[(byte)partToRead].Width).ToString();
                    tbStudioSetPartMotionalSurroundAmbienceSendLevel.Text = "Ambience send level " + (commonState.StudioSet.PartMotionalSurround[(byte)partToRead].AmbienceSendLevel).ToString();
                }
            }
            catch { }
            PopHandleControlEvents();
        }

        private void ReadStudioSetPartToneName()
        {
            t.Trace("private void ReadStudioSetPartToneName()");
            ReceivedData Data = new ReceivedData(rawData);
            PushHandleControlEvents();
            switch (commonState.SimpleToneType)
            {
                case CommonState.SimpleToneTypes.PCM_SYNTH_TONE:
                    pCMSynthTone = new PCMSynthTone(Data);
                    StudioSetCurrentToneName.Text = commonState.ToneSource +  " PCMS: " + pCMSynthTone.pCMSynthToneCommon.Name;
                    commonState.CurrentTone.Name = pCMSynthTone.pCMSynthToneCommon.Name;
                    break;
                case CommonState.SimpleToneTypes.PCM_DRUM_KIT:
                    pCMDrumKit = new PCMDrumKit(Data);
                    StudioSetCurrentToneName.Text = commonState.ToneSource + " PCMD: " + pCMDrumKit.pCMDrumKitCommon.Name;
                    commonState.CurrentTone.Name = pCMDrumKit.pCMDrumKitCommon.Name;
                    break;
                case CommonState.SimpleToneTypes.SUPERNATURAL_ACOUSTIC_TONE:
                    superNATURALAcousticTone = new SuperNATURALAcousticTone(Data);
                    StudioSetCurrentToneName.Text = commonState.ToneSource + " SN-A: " + superNATURALAcousticTone.superNATURALAcousticToneCommon.Name;
                    commonState.CurrentTone.Name = superNATURALAcousticTone.superNATURALAcousticToneCommon.Name;
                    break;
                case CommonState.SimpleToneTypes.SUPERNATURAL_SYNTH_TONE:
                    superNATURALSynthTone = new SuperNATURALSynthTone(Data);
                    StudioSetCurrentToneName.Text = commonState.ToneSource + " SN-S: " + superNATURALSynthTone.superNATURALSynthToneCommon.Name;
                    commonState.CurrentTone.Name = superNATURALSynthTone.superNATURALSynthToneCommon.Name;
                    break;
                case CommonState.SimpleToneTypes.SUPERNATURAL_DRUM_KIT:
                    superNATURALDrumKit = new SuperNATURALDrumKit(Data);
                    StudioSetCurrentToneName.Text = commonState.ToneSource + " SN-D: " + superNATURALDrumKit.superNATURALDrumKitCommon.Name;
                    commonState.CurrentTone.Name = superNATURALDrumKit.superNATURALDrumKitCommon.Name;
                    break;
            }
            PopHandleControlEvents();
        }

        private void ReadStudioSetPartMidiPhaseLock()
        {
            t.Trace("private void ReadStudioSetPartMidiPhaseLock()");
            // This is a bit special since we have put part MIDI together with MIDI switches (which must first have been read above!).
            ReceivedData Data = new ReceivedData(rawData);
            PushHandleControlEvents();
            cbStudioSetPartMidiPhaseLock.IsChecked = commonState.StudioSet.PartMidi[cbStudioSetPartSelector.SelectedIndex].PhaseLock;
            PopHandleControlEvents();
        }

        private void ReadStudioSetPartEQ(Int32 partToRead = -1)
        {
            t.Trace("private void ReadStudioSetPartEQ()");
            if (partToRead == -1)
            {
                partToRead = cbStudioSetPartSelector.SelectedIndex;
            }
            commonState.StudioSet.PartEQ[partToRead] = new StudioSet_PartEQ(new ReceivedData(rawData));
            PushHandleControlEvents();
            cbStudioSetPartEQSwitch.IsChecked = commonState.StudioSet.PartEQ[(byte)partToRead].EqSwitch;
            cbStudioSetPartEQLowFreq.SelectedIndex = commonState.StudioSet.PartEQ[(byte)partToRead].EqLowFreq;
            slStudioSetPartEQLowGain.Value = commonState.StudioSet.PartEQ[(byte)partToRead].EqLowGain;
            cbStudioSetPartEQMidFreq.SelectedIndex = commonState.StudioSet.PartEQ[(byte)partToRead].EqMidFreq;
            slStudioSetPartEQMidGain.Value = commonState.StudioSet.PartEQ[(byte)partToRead].EqMidGain;
            cbStudioSetPartEQMidQ.SelectedIndex = commonState.StudioSet.PartEQ[(byte)partToRead].EqMidQ;
            cbStudioSetPartEQHighFreq.SelectedIndex = commonState.StudioSet.PartEQ[(byte)partToRead].EqHighFreq;
            slStudioSetPartEQHighGain.Value = commonState.StudioSet.PartEQ[(byte)partToRead].EqHighGain;
            // Slider texts:
            tbStudioSetPartEQLowGain.Text = "Low gain " + (commonState.StudioSet.PartEQ[(byte)partToRead].EqLowGain).ToString();
            tbStudioSetPartEQMidGain.Text = "Mid gain " + (commonState.StudioSet.PartEQ[(byte)partToRead].EqMidGain).ToString();
            tbStudioSetPartEQHighGain.Text = "High gain " + (commonState.StudioSet.PartEQ[(byte)partToRead].EqHighGain).ToString();
            PopHandleControlEvents();
        }

        private void UpdateToneFromControls()
        {
            // Update StudioSetCurrentToneName:
            StudioSetCurrentToneName.Text = (String)cbStudioSetPartSettings1Program.SelectedItem;
            // Update commonState:
            commonState.CurrentTone.Group = (String)cbStudioSetPartSettings1Group.SelectedItem;
            commonState.CurrentTone.Category = (String)cbStudioSetPartSettings1Category.SelectedItem;
            commonState.CurrentTone.Name = (String)cbStudioSetPartSettings1Program.SelectedItem;
            commonState.CurrentTone.Index = commonState.ToneList.Get(commonState.CurrentTone);
            // Update  commonState.studioSet:
            commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectMSB =
                byte.Parse(commonState.ToneList.Tones[commonState.CurrentTone.Index][4]);
            commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB =
                byte.Parse(commonState.ToneList.Tones[commonState.CurrentTone.Index][5]);
            commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneProgramNumber =
                byte.Parse(commonState.ToneList.Tones[commonState.CurrentTone.Index][7]);
            // Update  I-7:
            commonState.Midi.ProgramChange((byte)cbStudioSetPartSelector.SelectedIndex,
                byte.Parse(commonState.ToneList.Tones[commonState.CurrentTone.Index][4]),
                byte.Parse(commonState.ToneList.Tones[commonState.CurrentTone.Index][5]),
                byte.Parse(commonState.ToneList.Tones[commonState.CurrentTone.Index][7]));
        }

        private void SendProgramChangeToI_7()
        {
            t.Trace("private void SendProgramChangeToI_7()");
            if (MidiChannelIsSameAsPart())
            {
                byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)cbStudioSetPartSelector.SelectedIndex), 0x06 };
                byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)cbStudioSetPartSelector.SelectedIndex].ToneBankSelectMSB,
                    (byte)commonState.StudioSet.PartMainSettings[(byte)cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB,
                    (byte)(commonState.StudioSet.PartMainSettings[(byte)cbStudioSetPartSelector.SelectedIndex].ToneProgramNumber % 128) };
                byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
                waitingForResponseFromIntegra7 = 500;
                commonState.Midi.SendSystemExclusive(bytes);
            }
        }

        private void slStudioSetPartSettings1Level_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings1Level_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1Level((Int32)slStudioSetPartSettings1Level.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings1Level(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].Level = (byte)p;
            tbStudioSetPartSettings1Level.Text = "Level " + (commonState.StudioSet.PartMainSettings[(byte)part].Level).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x09 };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].Level };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings1Pan_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings1Pan_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1Pan((Int32)slStudioSetPartSettings1Pan.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings1Pan(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].Pan = (byte)(p + 64);
            tbStudioSetPartSettings1Pan.Text = "Pan " + (commonState.StudioSet.PartMainSettings[(byte)part].Pan - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x0a };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].Pan };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings1CoarseTune_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings1CoarseTune_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1CoarseTune((Int32)slStudioSetPartSettings1CoarseTune.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings1CoarseTune(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].CoarseTune = (byte)(p + 64);
            tbStudioSetPartSettings1CoarseTune.Text = "Coarse tune " + (commonState.StudioSet.PartMainSettings[(byte)part].CoarseTune - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x0b };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].CoarseTune };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings1FineTune_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings1FineTune_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1FineTune((Int32)slStudioSetPartSettings1FineTune.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings1FineTune(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].FineTune = (byte)(p + 64);
            tbStudioSetPartSettings1FineTune.Text = "Fine tune " + (commonState.StudioSet.PartMainSettings[(byte)part].FineTune - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x0c };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].FineTune };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartSettings1Poly_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartSettings1Poly_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1Poly(cbStudioSetPartSettings1MonoPoly.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings1Poly(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].MonoPoly = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x0d };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].MonoPoly };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartSettings1Legato_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartSettings1Legato_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1Legato(cbStudioSetPartSettings1Legato.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings1Legato(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].Legato = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x0e };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].Legato };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings1BendRange_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings1BendRange_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1BendRange((Int32)slStudioSetPartSettings1PitchBendRange.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings1BendRange(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].PitchBendRange = (byte)p;
            tbStudioSetPartSettings1PitchBendRange.Text = "Pitch bend range " +
                (commonState.StudioSet.PartMainSettings[(byte)part].PitchBendRange > 24 ? "Tone" :
                (commonState.StudioSet.PartMainSettings[(byte)part].PitchBendRange).ToString());
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x0f };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].PitchBendRange };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartSettings1Portamento_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartSettings1Portamento_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1Portamento(cbStudioSetPartSettings1Portamento.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings1Portamento(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].PortamentoSwitch = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x10 };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].PortamentoSwitch };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings1PortamentoTime_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings1PortamentoTime_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings1PortamentoTime((Int32)slStudioSetPartSettings1PortamentoTime.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        //Part settings 2
        private void SetStudioSetStudioSetPartSettings1PortamentoTime(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].PortamentoTime = (byte)p;
            tbStudioSetPartSettings1PortamentoTime.Text = "Portamento time " +
                (commonState.StudioSet.PartMainSettings[(byte)part].PortamentoTime > 127 ? "Tone" :
                (commonState.StudioSet.PartMainSettings[(byte)part].PortamentoTime).ToString());
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x11 };
            byte[] value = { (byte)(commonState.StudioSet.PartMainSettings[(byte)part].PortamentoTime / 16), (byte)(commonState.StudioSet.PartMainSettings[(byte)part].PortamentoTime % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings2CutoffOffset_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings2CutoffOffset_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings2CutoffOffset((Int32)slStudioSetPartSettings2CutoffOffset.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings2CutoffOffset(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].CutoffOffset = (byte)(p + 64);
            tbStudioSetPartSettings2CutoffOffset.Text = "Cutoff offset " + (commonState.StudioSet.PartMainSettings[(byte)part].CutoffOffset - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x13 };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].CutoffOffset };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings2ResonanceOffset_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings2ResonanceOffset_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings2ResonanceOffset((Int32)slStudioSetPartSettings2ResonanceOffset.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings2ResonanceOffset(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].ResonanceOffset = (byte)(p + 64);
            tbStudioSetPartSettings2ResonanceOffset.Text = "Resonance offset " + (commonState.StudioSet.PartMainSettings[(byte)part].ResonanceOffset - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x14 };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].ResonanceOffset };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings2AttackTimeOffset_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings2AttackTimeOffset_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings2AttackTimeOffset((Int32)slStudioSetPartSettings2AttackTimeOffset.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings2AttackTimeOffset(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].AttackTimeOffset = (byte)(p + 64);
            tbStudioSetPartSettings2AttackTimeOffset.Text = "Attack time offset " + (commonState.StudioSet.PartMainSettings[(byte)part].AttackTimeOffset - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x15 };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].AttackTimeOffset };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings2DecayTimeOffset_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings2DecayTimeOffset_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings2DecayTimeOffset((Int32)slStudioSetPartSettings2DecayTimeOffset.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings2DecayTimeOffset(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].DecayTimeOffset = (byte)(p + 64);
            tbStudioSetPartSettings2DecayTimeOffset.Text = "Decay time offset " + (commonState.StudioSet.PartMainSettings[(byte)part].DecayTimeOffset - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x16 };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].DecayTimeOffset };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings2ReleaseTimeOffset_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings2ReleaseTimeOffset_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings2ReleaseTimeOffset((Int32)slStudioSetPartSettings2ReleaseTimeOffset.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings2ReleaseTimeOffset(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].ReleaseTimeOffset = (byte)(p + 64);
            tbStudioSetPartSettings2ReleaseTimeOffset.Text = "Release time offset " + (commonState.StudioSet.PartMainSettings[(byte)part].ReleaseTimeOffset - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x17 };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].ReleaseTimeOffset };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings2VibratoRate_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings2VibratoRate_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings2VibratoRate((Int32)slStudioSetPartSettings2VibratoRate.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings2VibratoRate(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].VibratoRate = (byte)(p + 64);
            tbStudioSetPartSettings2VibratoRate.Text = "Vibrato rate " + (commonState.StudioSet.PartMainSettings[(byte)part].VibratoRate - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x18 };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].VibratoRate };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings2VibratoDepth_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings2VibratoDepth_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings2VibratoDepth((Int32)slStudioSetPartSettings2VibratoDepth.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings2VibratoDepth(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].VibratoDepth = (byte)(p + 64);
            tbStudioSetPartSettings2VibratoDepth.Text = "Vibrato depth " + (commonState.StudioSet.PartMainSettings[(byte)part].VibratoDepth - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x19 };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].VibratoDepth };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartSettings2VibratoDelay_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartSettings2VibratoDelay_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartSettings2VibratoDelay((Int32)slStudioSetPartSettings2VibratoDelay.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartSettings2VibratoDelay(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].VibratoDelay = (byte)(p + 64);
            tbStudioSetPartSettings2VibratoDelay.Text = "Vibrato delay " + (commonState.StudioSet.PartMainSettings[(byte)part].VibratoDelay - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x1a };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].VibratoDelay };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartEffectsChorusSendLevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartEffectsChorusSendLevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartEffectsChorusSendLevel((Int32)slStudioSetPartEffectsChorusSendLevel.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartEffectsChorusSendLevel(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].ChorusSendLevel = (byte)p;
            tbStudioSetPartEffectsChorusSendLevel.Text = "Chorus send level " + commonState.StudioSet.PartMainSettings[(byte)part].ChorusSendLevel.ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x27 };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].ChorusSendLevel };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartEffectsReverbSendLevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartEffectsReverbSendLevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents) // Gör färdig även denna!
            {
                SetStudioSetStudioSetPartEffectsReverbSendLevel((Int32)slStudioSetPartEffectsReverbSendLevel.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartEffectsReverbSendLevel(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].ReverbSendLevel = (byte)(p);
            tbStudioSetPartEffectsReverbSendLevel.Text = "Reverb send level " + (commonState.StudioSet.PartMainSettings[(byte)part].ReverbSendLevel).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x28 };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].ReverbSendLevel };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartEffectsOutputAssign_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartEffectsOutputAssign_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartEffectsOutputAssign(cbStudioSetPartEffectsOutputAssign.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        // Keyboard parameters
        private void SetStudioSetStudioSetPartEffectsOutputAssign(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMainSettings[(byte)part].OutputAssign = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x29 };
            byte[] value = { (byte)commonState.StudioSet.PartMainSettings[(byte)part].OutputAssign };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartKeyboardOctaveShift_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartKeyboardOctaveShift_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboardOctaveShift((Int32)slStudioSetPartKeyboardOctaveShift.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartKeyboardOctaveShift(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartKeyboard[(byte)part].OctaveShift = (byte)(p + 64);
            tbStudioSetPartKeyboardOctaveShift.Text = "Octave shift " + (commonState.StudioSet.PartKeyboard[(byte)part].OctaveShift - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x1b };
            byte[] value = { (byte)commonState.StudioSet.PartKeyboard[(byte)part].OctaveShift };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartKeyboardVelocitySenseOffset_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartKeyboardVelocitySenseOffset_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboardVelocitySenseOffset((Int32)slStudioSetPartKeyboardVelocitySenseOffset.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartKeyboardVelocitySenseOffset(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartKeyboard[(byte)part].VelocitySenseOffset = (byte)(p + 63);
            tbStudioSetPartKeyboardVelocitySenseOffset.Text = "Velocity sense offset " + (commonState.StudioSet.PartKeyboard[(byte)part].VelocitySenseOffset - 63).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x1c };
            byte[] value = { (byte)commonState.StudioSet.PartKeyboard[(byte)part].VelocitySenseOffset };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartKeyboardRangeLower_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartKeyboardRangeLower_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboardRangeLower((Int32)slStudioSetPartKeyboardRangeLower.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartKeyboardRangeLower(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartKeyboard[(byte)part].RangeLower = (byte)p;
            tbStudioSetPartKeyboardRangeLower.Text = "Range lower " + (commonState.StudioSet.PartKeyboard[(byte)part].RangeLower).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x1d };
            byte[] value = { (byte)commonState.StudioSet.PartKeyboard[(byte)part].RangeLower };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartKeyboardRangeUpper_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartKeyboardRangeUpper_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboardRangeUpper((Int32)slStudioSetPartKeyboardRangeUpper.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartKeyboardRangeUpper(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartKeyboard[(byte)part].RangeUpper = (byte)p;
            tbStudioSetPartKeyboardRangeUpper.Text = "Range upper " + (commonState.StudioSet.PartKeyboard[(byte)part].RangeUpper).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x1e };
            byte[] value = { (byte)commonState.StudioSet.PartKeyboard[(byte)part].RangeUpper };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartKeyboardFadeWidthLower_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartKeyboardFadeWidthLower_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboardFadeWidthLower((Int32)slStudioSetPartKeyboardFadeWidthLower.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartKeyboardFadeWidthLower(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartKeyboard[(byte)part].FadeWidthLower = (byte)p;
            tbStudioSetPartKeyboardFadeWidthLower.Text = "Fade width lower " + (commonState.StudioSet.PartKeyboard[(byte)part].FadeWidthLower).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x1f };
            byte[] value = { (byte)commonState.StudioSet.PartKeyboard[(byte)part].FadeWidthLower };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartKeyboardFadeWidthUpper_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartKeyboardFadeWidthUpper_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboardFadeWidthUpper((Int32)slStudioSetPartKeyboardFadeWidthUpper.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartKeyboardFadeWidthUpper(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartKeyboard[(byte)part].FadeWidthUpper = (byte)p;
            tbStudioSetPartKeyboardFadeWidthUpper.Text = "Fade width uppper " + (commonState.StudioSet.PartKeyboard[(byte)part].FadeWidthUpper).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x20 };
            byte[] value = { (byte)commonState.StudioSet.PartKeyboard[(byte)part].FadeWidthUpper };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartKeyboardVelocityRangeLower_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartKeyboardVelocityRangeLower_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboardVelocityRangeLower((Int32)slStudioSetPartKeyboardVelocityRangeLower.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartKeyboardVelocityRangeLower(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartKeyboard[(byte)part].VelocityRangeLower = (byte)p;
            tbStudioSetPartKeyboardVelocityRangeLower.Text = "Velocity range lower " + (commonState.StudioSet.PartKeyboard[(byte)part].VelocityRangeLower).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x21 };
            byte[] value = { (byte)commonState.StudioSet.PartKeyboard[(byte)part].VelocityRangeLower };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartKeyboardVelocityRangeUpper_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartKeyboardVelocityRangeUpper_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboardVelocityRangeUpper((Int32)slStudioSetPartKeyboardVelocityRangeUpper.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartKeyboardVelocityRangeUpper(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartKeyboard[(byte)part].VelocityRangeUpper = (byte)p;
            tbStudioSetPartKeyboardVelocityRangeUpper.Text = "Velocity range upper " + (commonState.StudioSet.PartKeyboard[(byte)part].VelocityRangeUpper).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x22 };
            byte[] value = { (byte)commonState.StudioSet.PartKeyboard[(byte)part].VelocityRangeUpper };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartKeyboardVelocityFadeWidthLower_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartKeyboardVelocityFadeWidthLower_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboardVelocityFadeWidthLower((Int32)slStudioSetPartKeyboardVelocityFadeWidthLower.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartKeyboardVelocityFadeWidthLower(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartKeyboard[(byte)part].VelocityFadeWidthLower = (byte)p;
            tbStudioSetPartKeyboardVelocityFadeWidthLower.Text = "Vel. fade width lower " + (commonState.StudioSet.PartKeyboard[(byte)part].VelocityFadeWidthLower).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x23 };
            byte[] value = { (byte)commonState.StudioSet.PartKeyboard[(byte)part].VelocityFadeWidthLower };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartKeyboardVelocityFadeWidthUpper_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartKeyboardVelocityFadeWidthUpper_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboardVelocityFadeWidthUpper((Int32)slStudioSetPartKeyboardVelocityFadeWidthUpper.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartKeyboardVelocityFadeWidthUpper(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartKeyboard[(byte)part].VelocityFadeWidthUpper = (byte)p;
            tbStudioSetPartKeyboardVelocityFadeWidthUpper.Text = "Vel. fade width upper " + (commonState.StudioSet.PartKeyboard[(byte)part].VelocityFadeWidthUpper).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x24 };
            byte[] value = { (byte)commonState.StudioSet.PartKeyboard[(byte)part].VelocityFadeWidthUpper };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartKeyboardVelocityCurveType_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartKeyboardVelocityCurveType_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboardVelocityCurveType(cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void BtnVelocity1Curve_Clicked(object sender, EventArgs e)
        {
            if (commonState.StudioSet.PartKeyboard[cbStudioSetPartSelector.SelectedIndex].VelocityCurveType == 1)
            {
                // This one is selected, turn it off:
                cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = 0;
            }
            else
            {
                cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = 1;
            }
        }

        private void BtnVelocity2Curve_Clicked(object sender, EventArgs e)
        {
            if (commonState.StudioSet.PartKeyboard[cbStudioSetPartSelector.SelectedIndex].VelocityCurveType == 2)
            {
                // This one is selected, turn it off:
                cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = 0;
            }
            else
            {
                cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = 2;
            }
        }

        private void BtnVelocity3Curve_Clicked(object sender, EventArgs e)
        {
            if (commonState.StudioSet.PartKeyboard[cbStudioSetPartSelector.SelectedIndex].VelocityCurveType == 3)
            {
                // This one is selected, turn it off:
                cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = 0;
            }
            else
            {
                cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = 3;
            }
        }

        private void BtnVelocity4Curve_Clicked(object sender, EventArgs e)
        {
            if (commonState.StudioSet.PartKeyboard[cbStudioSetPartSelector.SelectedIndex].VelocityCurveType == 4)
            {
                // This one is selected, turn it off:
                cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = 0;
            }
            else
            {
                cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = 4;
            }
        }

        private void SetStudioSetStudioSetPartKeyboardVelocityCurveType(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartKeyboard[(byte)part].VelocityCurveType = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x43 };
            byte[] value = { (byte)commonState.StudioSet.PartKeyboard[(byte)part].VelocityCurveType };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
            switch (p)
            {
                case 0:
                    imgVelocityCurve1On.IsVisible = false;
                    imgVelocityCurve2On.IsVisible = false;
                    imgVelocityCurve3On.IsVisible = false;
                    imgVelocityCurve4On.IsVisible = false;
                    imgVelocityCurve1Off.IsVisible = true;
                    imgVelocityCurve2Off.IsVisible = true;
                    imgVelocityCurve3Off.IsVisible = true;
                    imgVelocityCurve4Off.IsVisible = true;
                    break;
                case 1:
                    imgVelocityCurve1On.IsVisible = true;
                    imgVelocityCurve2On.IsVisible = false;
                    imgVelocityCurve3On.IsVisible = false;
                    imgVelocityCurve4On.IsVisible = false;
                    imgVelocityCurve1Off.IsVisible = false;
                    imgVelocityCurve2Off.IsVisible = true;
                    imgVelocityCurve3Off.IsVisible = true;
                    imgVelocityCurve4Off.IsVisible = true;
                    break;
                case 2:
                    imgVelocityCurve1On.IsVisible = false;
                    imgVelocityCurve2On.IsVisible = true;
                    imgVelocityCurve3On.IsVisible = false;
                    imgVelocityCurve4On.IsVisible = false;
                    imgVelocityCurve1Off.IsVisible = true;
                    imgVelocityCurve2Off.IsVisible = false;
                    imgVelocityCurve3Off.IsVisible = true;
                    imgVelocityCurve4Off.IsVisible = true;
                    break;
                case 3:
                    imgVelocityCurve1On.IsVisible = false;
                    imgVelocityCurve2On.IsVisible = false;
                    imgVelocityCurve3On.IsVisible = true;
                    imgVelocityCurve4On.IsVisible = false;
                    imgVelocityCurve1Off.IsVisible = true;
                    imgVelocityCurve2Off.IsVisible = true;
                    imgVelocityCurve3Off.IsVisible = false;
                    imgVelocityCurve4Off.IsVisible = true;
                    break;
                case 4:
                    imgVelocityCurve1On.IsVisible = false;
                    imgVelocityCurve2On.IsVisible = false;
                    imgVelocityCurve3On.IsVisible = false;
                    imgVelocityCurve4On.IsVisible = true;
                    imgVelocityCurve1Off.IsVisible = true;
                    imgVelocityCurve2Off.IsVisible = true;
                    imgVelocityCurve3Off.IsVisible = true;
                    imgVelocityCurve4Off.IsVisible = false;
                    break;
            }
        }

        private void cbStudioSetPartKeyboard_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartKeyboard_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboard((Boolean)cbStudioSetPartKeyboardMute.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartKeyboard(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartKeyboard[(byte)part].Mute = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x25 };
            byte[] value = { (byte)(commonState.StudioSet.PartKeyboard[(byte)part].Mute ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        // Scale tune parameters
        private void cbStudioSetPartScaleTuneType_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartScaleTuneType_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneType(cbStudioSetPartScaleTuneType.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneType(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].Type = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x2b };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].Type };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartScaleTune_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartScaleTune_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTune(cbStudioSetPartScaleTuneKey.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTune(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].Key = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x2c };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].Key };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneC_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneC_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneC((Int32)slStudioSetPartScaleTuneC.Value + 64, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneC(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].C = (byte)p;
            tbStudioSetPartScaleTuneC.Text = "C " + (commonState.StudioSet.PartScaleTune[(byte)part].C).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x2d };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].C };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneCi_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneCi_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneCi((Int32)slStudioSetPartScaleTuneCi.Value + 64, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneCi(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].Ci = (byte)p;
            tbStudioSetPartScaleTuneCi.Text = "C# " + (commonState.StudioSet.PartScaleTune[(byte)part].Ci).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x2e };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].Ci };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneD_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneD_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneD((Int32)slStudioSetPartScaleTuneD.Value + 64, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneD(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].D = (byte)p;
            tbStudioSetPartScaleTuneD.Text = "D " + (commonState.StudioSet.PartScaleTune[(byte)part].D).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x2f };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].D };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneDi_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneDi_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneDi((Int32)slStudioSetPartScaleTuneDi.Value + 64, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneDi(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].Di = (byte)p;
            tbStudioSetPartScaleTuneDi.Text = "D# " + (commonState.StudioSet.PartScaleTune[(byte)part].Di).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x30 };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].Di };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneE_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneE_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneE((Int32)slStudioSetPartScaleTuneE.Value + 64, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneE(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].E = (byte)p;
            tbStudioSetPartScaleTuneE.Text = "E " + (commonState.StudioSet.PartScaleTune[(byte)part].E).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x31 };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].E };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneF_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneF_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneF((Int32)slStudioSetPartScaleTuneF.Value + 64, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneF(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].F = (byte)p;
            tbStudioSetPartScaleTuneF.Text = "F " + (commonState.StudioSet.PartScaleTune[(byte)part].F).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x32 };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].F };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneFi_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneFi_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneFi((Int32)slStudioSetPartScaleTuneFi.Value + 64, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneFi(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].Fi = (byte)p;
            tbStudioSetPartScaleTuneFi.Text = "F# " + (commonState.StudioSet.PartScaleTune[(byte)part].Fi).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x33 };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].Fi };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneG_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneG_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneG((Int32)slStudioSetPartScaleTuneG.Value + 64, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneG(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].G = (byte)p;
            tbStudioSetPartScaleTuneG.Text = "G " + (commonState.StudioSet.PartScaleTune[(byte)part].G).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x34 };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].G };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneGi_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneGi_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneGi((Int32)slStudioSetPartScaleTuneGi.Value + 64, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneGi(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].Gi = (byte)p;
            tbStudioSetPartScaleTuneGi.Text = "G# " + (commonState.StudioSet.PartScaleTune[(byte)part].Gi).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x35 };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].Gi };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneA_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneA_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneA((Int32)slStudioSetPartScaleTuneA.Value + 64, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneA(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].A = (byte)p;
            tbStudioSetPartScaleTuneA.Text = "A " + (commonState.StudioSet.PartScaleTune[(byte)part].A).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x36 };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].A };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneAi_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneAi_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneAi((Int32)slStudioSetPartScaleTuneAi.Value + 64, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneAi(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].Ai = (byte)p;
            tbStudioSetPartScaleTuneAi.Text = "A# " + (commonState.StudioSet.PartScaleTune[(byte)part].Ai).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x37 };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].Ai };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneB_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneB_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneB((Int32)slStudioSetPartScaleTuneB.Value + 64, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneB(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartScaleTune[(byte)part].B = (byte)p;
            tbStudioSetPartScaleTuneB.Text = "B " + (commonState.StudioSet.PartScaleTune[(byte)part].B).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x38 };
            byte[] value = { (byte)commonState.StudioSet.PartScaleTune[(byte)part].B };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        // Midi
        private void cbStudioSetPartMidiReceiveProgramChange_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartMidiReceiveProgramChange_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMidiReceiveProgramChange((Boolean)cbStudioSetPartMidiReceiveProgramChange.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartMidiReceiveProgramChange(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartMidi[(byte)part].ReceiveProgramChange = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x39 };
            byte[] value = { (byte)(commonState.StudioSet.PartMidi[(byte)part].ReceiveProgramChange ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartMidiReceiveBankSelect_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartMidiReceiveBankSelect_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMidiReceiveBankSelect((Boolean)cbStudioSetPartMidiReceiveBankSelect.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartMidiReceiveBankSelect(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartMidi[(byte)part].ReceiveBankSelect = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x3a };
            byte[] value = { (byte)(commonState.StudioSet.PartMidi[(byte)part].ReceiveBankSelect ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartMidiReceivePitchBend_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartMidiReceivePitchBend_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMidiReceivePitchBend((Boolean)cbStudioSetPartMidiReceivePitchBend.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartMidiReceivePitchBend(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartMidi[(byte)part].ReceivePitchBend = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x3b };
            byte[] value = { (byte)(commonState.StudioSet.PartMidi[(byte)part].ReceivePitchBend ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartMidiReceivePolyphonicKeyPressure_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartMidiReceivePolyphonicKeyPressure_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMidiReceivePolyphonicKeyPressure((Boolean)cbStudioSetPartMidiReceivePolyphonicKeyPressure.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartMidiReceivePolyphonicKeyPressure(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartMidi[(byte)part].ReceivePolyphonicKeyPressure = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x3c };
            byte[] value = { (byte)(commonState.StudioSet.PartMidi[(byte)part].ReceivePolyphonicKeyPressure ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartMidiReceiveChannelPressure_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartMidiReceiveChannelPressure_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetPartMidiReceiveChannelPressure((Boolean)cbStudioSetPartMidiReceiveChannelPressure.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetPartMidiReceiveChannelPressure(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartMidi[(byte)part].ReceiveChannelPressure = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x3d };
            byte[] value = { (byte)(commonState.StudioSet.PartMidi[(byte)part].ReceiveChannelPressure ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartMidiReceiveModulation_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartMidiReceiveModulation_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMidiReceiveModulation((Boolean)cbStudioSetPartMidiReceiveModulation.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartMidiReceiveModulation(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartMidi[(byte)part].ReceiveModulation = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x3e };
            byte[] value = { (byte)(commonState.StudioSet.PartMidi[(byte)part].ReceiveModulation ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartMidiReceiveVolume_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartMidiReceiveVolume_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMidiReceiveVolume((Boolean)cbStudioSetPartMidiReceiveVolume.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartMidiReceiveVolume(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartMidi[(byte)part].ReceiveVolume = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x3f };
            byte[] value = { (byte)(commonState.StudioSet.PartMidi[(byte)part].ReceiveVolume ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartMidiReceivePan_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartMidiReceivePan_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMidiReceivePan((Boolean)cbStudioSetPartMidiReceivePan.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartMidiReceivePan(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartMidi[(byte)part].ReceivePan = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x40 };
            byte[] value = { (byte)(commonState.StudioSet.PartMidi[(byte)part].ReceivePan ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartMidiReceiveExpression_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartMidiReceiveExpression_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMidiReceiveExpression((Boolean)cbStudioSetPartMidiReceiveExpression.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartMidiReceiveExpression(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartMidi[(byte)part].ReceiveExpression = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x41 };
            byte[] value = { (byte)(commonState.StudioSet.PartMidi[(byte)part].ReceiveExpression ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartMidiReceiveHold1_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartMidiReceiveHold1_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMidiReceiveHold1((Boolean)cbStudioSetPartMidiReceiveHold1.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartMidiReceiveHold1(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartMidi[(byte)part].ReceiveHold1 = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x42 };
            byte[] value = { (byte)(commonState.StudioSet.PartMidi[(byte)part].ReceiveHold1 ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartMidiPhaseLock_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartMidiPhaseLock_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMidiPhaseLock((Boolean)cbStudioSetPartMidiPhaseLock.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        // Motional surround
        private void SetStudioSetStudioSetPartMidiPhaseLock(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartMidi[(byte)part].PhaseLock = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x10 + (byte)part), 0x00 };
            byte[] value = { (byte)(commonState.StudioSet.PartMidi[(byte)part].PhaseLock ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartMotionalSurroundLR_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartMotionalSurroundLR_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMotionalSurroundLR((Int32)slStudioSetPartMotionalSurroundLR.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartMotionalSurroundLR(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMotionalSurround[(byte)part].LR = (byte)p;
            tbStudioSetPartMotionalSurroundLR.Text = "L-R " + (commonState.StudioSet.PartMotionalSurround[(byte)part].LR).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x44 };
            byte[] value = { (byte)commonState.StudioSet.PartMotionalSurround[(byte)part].LR };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartMotionalSurroundFB_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartMotionalSurroundFB_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMotionalSurroundFB((Int32)slStudioSetPartMotionalSurroundFB.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartMotionalSurroundFB(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMotionalSurround[(byte)part].FB = (byte)p;
            tbStudioSetPartMotionalSurroundFB.Text = "F-B " + (commonState.StudioSet.PartMotionalSurround[(byte)part].FB).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x46 };
            byte[] value = { (byte)commonState.StudioSet.PartMotionalSurround[(byte)part].FB };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartMotionalSurroundWidth_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartMotionalSurroundWidth_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMotionalSurroundWidth((Int32)slStudioSetPartMotionalSurroundWidth.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartMotionalSurroundWidth(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMotionalSurround[(byte)part].Width = (byte)p;
            tbStudioSetPartMotionalSurroundWidth.Text = "Width " + (commonState.StudioSet.PartMotionalSurround[(byte)part].Width).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x48 };
            byte[] value = { (byte)commonState.StudioSet.PartMotionalSurround[(byte)part].Width };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartMotionalSurroundAmbienceSendLevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartMotionalSurroundAmbienceSendLevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartMotionalSurroundAmbienceSendLevel((Int32)slStudioSetPartMotionalSurroundAmbienceSendLevel.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        // Equalizer settings
        private void SetStudioSetStudioSetPartMotionalSurroundAmbienceSendLevel(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartMotionalSurround[(byte)part].AmbienceSendLevel = (byte)p;
            tbStudioSetPartMotionalSurroundAmbienceSendLevel.Text = "Ambience send level " + (commonState.StudioSet.PartMotionalSurround[(byte)part].AmbienceSendLevel).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x49 };
            byte[] value = { (byte)commonState.StudioSet.PartMotionalSurround[(byte)part].AmbienceSendLevel };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartEQ_Click(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartEQ_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartEQ((Boolean)cbStudioSetPartEQSwitch.IsChecked, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartEQ(Boolean p, Int32 part)
        {
            commonState.StudioSet.PartEQ[(byte)part].EqSwitch = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x50 + (byte)part), 0x00 };
            byte[] value = { (byte)(commonState.StudioSet.PartEQ[(byte)part].EqSwitch ? 1 : 0) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartEQLoqFreq_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartEQLoqFreq_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartEQLoqFreq(cbStudioSetPartEQLowFreq.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartEQLoqFreq(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartEQ[(byte)part].EqLowFreq = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x50 + (byte)part), 0x01 };
            byte[] value = { (byte)commonState.StudioSet.PartEQ[(byte)part].EqLowFreq };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartEQLowGain_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartEQLowGain_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartEQLowGain((Int32)slStudioSetPartEQLowGain.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartEQLowGain(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqLowGain = (byte)(p + 15);
            tbStudioSetPartEQLowGain.Text = "Low gain " + (commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqLowGain - 15).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x50 + cbStudioSetPartSelector.SelectedIndex), 0x02 };
            byte[] value = { (byte)(commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqLowGain) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartEQMidFreq_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartEQMidFreq_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartEQMidFreq(cbStudioSetPartEQMidFreq.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartEQMidFreq(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidFreq = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x50 + cbStudioSetPartSelector.SelectedIndex), 0x03 };
            byte[] value = { (byte)commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidFreq };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartEQMidGain_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartEQMidGain_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartEQMidGain((Int32)slStudioSetPartEQMidGain.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartEQMidGain(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidGain = (byte)(p + 15);
            tbStudioSetPartEQMidGain.Text = "Mid gain " + (commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidGain - 15).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x50 + cbStudioSetPartSelector.SelectedIndex), 0x04 };
            byte[] value = { (byte)(commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidGain) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartEQMidQ_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartEQMidQ_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartEQMidQ(cbStudioSetPartEQMidQ.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartEQMidQ(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidQ = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x50 + cbStudioSetPartSelector.SelectedIndex), 0x05 };
            byte[] value = { (byte)commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidQ };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartEQHighFreq_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartEQHighFreq_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartEQHighFreq(cbStudioSetPartEQHighFreq.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartEQHighFreq(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqHighFreq = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x50 + cbStudioSetPartSelector.SelectedIndex), 0x06 };
            byte[] value = { (byte)commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqHighFreq };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartEQHighGain_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartEQHighGain_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartEQHighGain((Int32)slStudioSetPartEQHighGain.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartEQHighGain(Int32 p, Int32 part)
        {
            commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqHighGain = (byte)((Int32)p + 15);
            tbStudioSetPartEQHighGain.Text = "High gain " + (commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqHighGain - 15).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x50 + cbStudioSetPartSelector.SelectedIndex), 0x07 };
            byte[] value = { (byte)(commonState.StudioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqHighGain) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetSlot_SelectionChanged(object sender, EventArgs e)
        {
            if (cbStudioSetSlot.SelectedItem != null
                && (!((String)cbStudioSetSlot.SelectedItem).StartsWith("INIT STUDIO")))
            {
                tbStudioSetName.Text = ((String)cbStudioSetSlot.SelectedItem).TrimEnd();
            }
        }

        private void SetStudioSetStudioSetSlot(Int32 p)
        {

        }

        private void tbStudioSetName_KeyUp(object sender, EventArgs e)
        {
            t.Trace("private void tbStudioSetName_KeyUp (" + "object" + sender + ", " + "KeyEventArgs" + e + ", " + ")");
            if (tbStudioSetName.Text.Length > 16)
            {
                tbStudioSetName.Text = tbStudioSetName.Text.Remove(12);
                //tbStudioSetName.SelectionStart = tbStudioSetName.Text.Length;
                //tbStudioSetName.SelectionLength = 0;
            }
        }

        private async void btnStudioSetSave_Click(object sender, EventArgs e)
        {
            if (tbStudioSetName.Text.Length > 0)
            {
                if (tbStudioSetName.Text.StartsWith("INIT STUDIO"))
                {
                    //MessageDialog warning = new MessageDialog("Name of studio set should not be \'INIT STUDIO\'. Please use another name.");
                    //warning.Title = "Note!";
                    //warning.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                    //var response = await warning.ShowAsync();
                    String response = await mainPage.DisplayActionSheet("Name of studio set should not be \'INIT STUDIO\'. Please use another name.",
                        null, null, new string[] { "Ok" });
                }
                else
                {
                    Boolean write = true;
                    if (!((String)cbStudioSetSlot.SelectedItem).StartsWith("INIT STUDIO"))
                    {
                        //MessageDialog warning = new MessageDialog("This slot contains another Studio Set. Are you sure you want to overwrite it?");
                        //warning.Title = "Note!";
                        //warning.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
                        //warning.Commands.Add(new UICommand { Label = "No", Id = 1 });
                        //var response = await warning.ShowAsync();
                        //if ((Int32)response.Id == 1)
                        //{
                        //    write = false;
                        //}
                        String response = await mainPage.DisplayActionSheet("This slot contains another Studio Set. Are you sure you want to overwrite it?",
                            null, null, new String[] { "Yes,", "No" });
                        if (response == "No")
                        {
                            write = false;
                        }
                    }
                    if (write)
                    {
                        // Store the new Studio Set name:
                        String name = tbStudioSetName.Text;
                        while (name.Length < 16)
                        {
                            name = name + " ";
                        }
                        byte[] address = { 0x18, 0x00, 0x00, 0x00 };
                        byte[] data = Encoding.UTF8.GetBytes(name);
                        byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, data);
                        waitingForResponseFromIntegra7 = 500;
                        commonState.Midi.SendSystemExclusive(bytes);
                        // Save all partials:

                        // Make INTEGRA-7 save the Studio Set:
                        address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                        data = new byte[] { 0x55, 0x00, (byte)(cbStudioSetSlot.SelectedIndex), 0x7f };
                        bytes = commonState.Midi.SystemExclusiveRQ1Message(address, data);
                        waitingForResponseFromIntegra7 = 500;
                        commonState.Midi.SendSystemExclusive(bytes);

                        // Add the new studio set name to the studio set selector:
                        cbStudioSetSelector.Items[cbStudioSetSlot.SelectedIndex] = cbStudioSetSlot.SelectedIndex.ToString() + " " + tbStudioSetName.Text;

                        // Add the new studio set name to commonState.studioSetNames.
                        commonState.StudioSetNames[cbStudioSetSlot.SelectedIndex] = tbStudioSetName.Text;

                        // Select the new studio set:
                        PushHandleControlEvents();
                        cbStudioSetSelector.SelectedIndex = cbStudioSetSlot.SelectedIndex;
                        PopHandleControlEvents();
                    }
                }
            }
        }

        private async void btnStudioSetDelete_Click(object sender, EventArgs e)
        {
            t.Trace("private void btnStudioSetDelete_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (!tbStudioSetName.Text.StartsWith("INIT STUDIO"))
            {
                Boolean write = true;
                if (!((String)cbStudioSetSlot.SelectedItem).StartsWith("INIT STUDIO"))
                {
                    //MessageDialog warning = new MessageDialog("Are you sure wyou want to delete this Studio Set?");
                    //warning.Title = "Note!";
                    //warning.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
                    //warning.Commands.Add(new UICommand { Label = "No", Id = 1 });
                    //var response = await warning.ShowAsync();
                    //if ((Int32)response.Id == 1)
                    //{
                    //    write = false;
                    //}
                    String response = await mainPage.DisplayActionSheet("Are you sure you want to delete this Studio Set?",
                        null, null, new String[] { "Yes,", "No" });
                    if (response == "Yes")
                    {
                        write = false;
                    }
                }
                if (write)
                {
                    // Change the name:
                    byte[] address = { 0x18, 0x00, 0x00, 0x00 };
                    byte[] data = Encoding.UTF8.GetBytes("INIT STUDIO     ");
                    byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(address, data);
                    waitingForResponseFromIntegra7 = 500;
                    commonState.Midi.SendSystemExclusive(bytes);
                    // InitializeComponent the studio set:
                    address = new byte[] { 0x0f, 0x00, (byte)(cbStudioSetSlot.SelectedIndex), 0x00 };
                    data = new byte[] { 0x7f, 0x7f, 0x00, 0x00 };
                    bytes = commonState.Midi.SystemExclusiveRQ1Message(address, data);
                    waitingForResponseFromIntegra7 = 500;
                    commonState.Midi.SendSystemExclusive(bytes);
                    // Save the studio set:
                    address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                    data = new byte[] { 0x55, 0x00, (byte)(cbStudioSetSlot.SelectedIndex + 16), 0x7f };
                    bytes = commonState.Midi.SystemExclusiveRQ1Message(address, data);
                    waitingForResponseFromIntegra7 = 500;
                    commonState.Midi.SendSystemExclusive(bytes);

                    // Remove the new studio set name from the studio set selector:
                    cbStudioSetSelector.Items[cbStudioSetSlot.SelectedIndex] = "INIT STUDIO";

                    // Remove the new studio set name from commonState.studioSetNames.
                    commonState.StudioSetNames[cbStudioSetSlot.SelectedIndex] = "INIT STUDIO";

                    // Now, get the freshly initiated part:
                    QueryStudioSetPart();
                }
            }
        }

        private void cbStudioSetPartSettings1Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            t.Trace("private void cbStudioSetPartSettings1Search_TextChanged (" + "object" + sender + ", " + "TextChangedEventArgs" + e + ", " + ")");
            if (String.IsNullOrEmpty(cbStudioSetPartSettings1Search.Text) || cbStudioSetPartSettings1Search.Text.Length < 3)
            {
                gEditStudioSetSearchResult.IsVisible = false;
                grid_StudioSet_Column1.IsVisible = true;
            }
            else
            {
                if (MidiChannelIsSameAsPart())
                {
                    gEditStudioSetSearchResult.IsVisible = true;
                    grid_StudioSet_Column1.IsVisible = false;
                    try
                    {
                        StudioSetEditor_SearchResult.Clear();
                    }
                    catch { }
                    String searchString = cbStudioSetPartSettings1Search.Text.ToLower();
                    // Search voices:
                    foreach (List<String> tone in commonState.ToneList.Tones)
                    {
                        if (tone[3].ToLower().Contains(searchString))
                        {
                            StudioSetEditor_SearchResult.Add(tone[3] + ", " + tone[0] + ", " + tone[1]);
                        }
                    }
                }
            }
        }

        private void lvSearchResults_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void lvSearchResults_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            String soundName = (String)((ListView)sender).SelectedItem;
            Boolean drumMap = false;
            if (!String.IsNullOrEmpty(soundName))
            {
                commonState.CurrentTone.Name = soundName;
            }
            if (!String.IsNullOrEmpty(cbStudioSetPartSettings1Search.Text))
            {
                if (commonState.CurrentTone.Name.EndsWith("\t"))
                {
                    drumMap = true;
                    commonState.CurrentTone.Name = commonState.CurrentTone.Name.TrimEnd('\t');
                }
                String[] parts = commonState.CurrentTone.Name.Split(',');
                if (parts.Length == 3)
                {
                    if (drumMap)
                    {
                        commonState.CurrentTone.Group = parts[1].TrimStart();
                        commonState.CurrentTone.Category = "Drum";
                        commonState.CurrentTone.Name = parts[2].TrimStart();
                    }
                    else
                    {
                        commonState.CurrentTone.Group = parts[1].TrimStart();
                        commonState.CurrentTone.Category = parts[2].TrimStart();
                        commonState.CurrentTone.Name = parts[0].TrimStart();
                    }
                    Boolean currentHandleControlEvents = handleControlEvents;
                    PushHandleControlEvents();
                    cbStudioSetPartSettings1Group.SelectedItem = commonState.CurrentTone.Group;
                    PopulateCbStudioSetPartSettings1Category();
                    cbStudioSetPartSettings1Category.SelectedItem = commonState.CurrentTone.Category;
                    PopulateCbStudioSetPartSettings1Program();
                    cbStudioSetPartSettings1Program.SelectedItem = commonState.CurrentTone.Name;
                    PopHandleControlEvents();
                    cbStudioSetPartSettings1Search.Text = "";
                    commonState.CurrentTone.Index = 
                        commonState.ToneList.Get(cbStudioSetPartSettings1Group.SelectedItem.ToString(),
                        cbStudioSetPartSettings1Category.SelectedItem.ToString(),
                        commonState.CurrentTone.Name);
                    UpdateToneFromControls();
                }
            }
        }

        private async void btnFileSave_Click(object sender, EventArgs e)
        {
            await StudioSet.Serialize<StudioSet>(commonState.StudioSet);
        }

        private async void btnFileLoad_Click(object sender, EventArgs e)
        {
            PushHandleControlEvents();
            commonState.StudioSet = await StudioSet.Deserialize<StudioSet>(commonState.StudioSet);
            UpdateControlsAndIntegra7FromClasses(cbStudioSetPartSelector.SelectedIndex);
            PopHandleControlEvents();
            ;
        }

        private void btnStudioSetReturn_Click(object sender, EventArgs e)
        {
            StudioSetEditor_StackLayout.IsVisible = false;
            stopEditTimer = true;
            ShowLibrarianPage();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private Boolean MidiChannelIsSameAsPart()
        {
            if (commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ReceiveChannel == (byte)cbStudioSetPartSelector.SelectedIndex)
            {
                return true;
            }
            else
            {
                ShowMessage("Current part has a MIDI receive channel that differs from the part number!\r\n" +
                "You can only change tone when the MIDI receive channel is the same as the selected part number.");
                return false;
            }
        }

        //private async void ShowMessage(String message)
        //{
        //    MessageDialog warning = new MessageDialog(message);
        //    warning.Title = "Warning!";
        //    await warning.ShowAsync();
        //}

        private void PopulateCbStudioSetPartSettings1Group()
        {
            t.Trace("private void PopulateCbStudioSetPartSettings1Group()");
            //byte[] tags = { 86, 87, 88, 89, 92, 93, 95, 96, 97, 120, 121 };
            //String[] groups = { "PCM Drum Kit", "PCM Synth Tone", "SN Drum Kit", "SN Acoustic Tone", "Exp PCM Drum Kit", "Exp PCM Tone", "SN Synth Tone", "ExPCM Drum Kit", "ExPCM Tone", "Exp GM2 Drum", "Exp GM2 Tons" };
            //ComboBoxItem item = null;
            //for (Int16 i = 0; i < tags.Length; i++)
            //{
            //    item = new ComboBoxItem();
            //    item.Tag = tags[i];
            //    //String s = item.Tag.ToString();
            //    //while (s.Length < 3)
            //    //{
            //    //    s = "0" + s;
            //    //}
            //    //item.Content = s + " (" + ByteToHexString((byte)item.Tag) + "H)";
            //    item.Content = groups[i];
            //    cbStudioSetPartSettings1Group.Items.Add(item);
            //}
            // Populate lvGroups:
            PushHandleControlEvents();
            cbStudioSetPartSettings1Group.Items.Clear();
            foreach (List<String> tone in commonState.ToneList.Tones)
            {
                if (!cbStudioSetPartSettings1Group.Items.Contains(tone[0]))
                {
                    cbStudioSetPartSettings1Group.Items.Add(tone[0]);
                }
            }
            PopHandleControlEvents();
        }

        private void PopulateCbStudioSetPartSettings1Category()
        {
            t.Trace("private void PopulateCbStudioSetPartSettings1Category()");
            //byte[] tags = new byte[1];
            //String[] categories = null;
            //switch ((byte)((ComboBoxItem)cbStudioSetPartSettings1Group.SelectedItem).Tag)
            //{
            //    case 86:
            //        tags = new byte[] { 0, 64 };
            //        categories = new String[] { "User", "Preset" };
            //        break;
            //    case 87:
            //        tags = new byte[] { 0, 1, 64, 65, 66, 67, 68, 69, 70 };
            //        categories = new String[] { "User 1", "User 2", "Preset 1", "Preset 2", "Preset 3", "Preset 4", "Preset 5", "Preset 6", "Preset 7" };
            //        break;
            //    case 88:
            //        tags = new byte[] { 0, 64, 101 };
            //        categories = new String[] { "User", "", "" };
            //        break;
            //    case 89:
            //        tags = new byte[] { 0, 1, 64, 65, 96, 97, 98, 99, 100 };
            //        categories = new String[] { "User 1", "User 2", "", "", "", "", "", "", "" };
            //        break;
            //    case 92:
            //        tags = new byte[] { 0, 2, 4, 7, 11, 15, 19 };
            //        categories = new String[] { "", "", "", "", "", "", "" };
            //        break;
            //    case 93:
            //        tags = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 26 };
            //        categories = new String[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            //        break;
            //    case 95:
            //        tags = new byte[] { 0, 1, 2, 3, 64, 65, 66, 67, 68, 69, 70, 71, 72 };
            //        categories = new String[] { "User 1", "User 2", "User 3", "User 4", "", "", "", "", "", "", "", "", "" };
            //        break;
            //    case 96:
            //        tags = new byte[] { 0 };
            //        categories = new String[] { "" };
            //        break;
            //    case 97:
            //        tags = new byte[] { 0, 1, 2, 3};
            //        categories = new String[] { "", "", "", "" };
            //        break;
            //    case 120:
            //        tags = new byte[] { 0 };
            //        categories = new String[] { "" };
            //        break;
            //    case 121:
            //        tags = new byte[] { 0 };
            //        categories = new String[] { "" };
            //        break;
            //}
            //Boolean currentHandleControlEvents = handleControlEvents;
            //PushHandleControlEvents();
            //cbStudioSetPartSettings1Category.Items.Clear();
            //ComboBoxItem item = null;
            //for (Int16 i = 0; i < tags.Length; i++)
            //{
            //    item = new ComboBoxItem();
            //    item.Tag = tags[i];
            //    String s = item.Tag.ToString();
            //    while (s.Length < 3)
            //    {
            //        s = "0" + s;
            //    }
            //    item.Content = s + " (" + ByteToHexString((byte)item.Tag) + "H)";
            //    cbStudioSetPartSettings1Category.Items.Add(item);
            //}
            //handleControlEvents = currentHandleControlEvents;
            String lastCategory = "";
            //CategoriesSource.Clear();
            PushHandleControlEvents();
            cbStudioSetPartSettings1Category.Items.Clear();
            foreach (List<String> line in commonState.ToneList.Tones.OrderBy(o => o[1]))
            {
                //if (line[0] == group && line[1] != lastCategory && !CategoriesSource.Contains(line[1]))
                if (line[0] == (String)cbStudioSetPartSettings1Group.SelectedItem && line[1] != lastCategory && !cbStudioSetPartSettings1Category.Items.Contains(line[1]))
                {
                    cbStudioSetPartSettings1Category.Items.Add(line[1]);
                    //CategoriesSource.Add(line[1]);
                    lastCategory = line[1];
                }
            }
            PopHandleControlEvents();
        }

        private void PopulateCbStudioSetPartSettings1Program()
        {
            t.Trace("private void PopulateCbStudioSetPartSettings1Program ()");
            PushHandleControlEvents();
            try
            {
                if (cbStudioSetPartSettings1Program.Items.Count() > 0)
                {
                    try
                    {
                        cbStudioSetPartSettings1Program.Items.Clear();
                    }
                    catch (Exception e)
                    {

                    }
                }
                foreach (List<String> tone in commonState.ToneList.Tones.OrderBy(o => o[3]))
                {
                    if (tone[0] == (String)cbStudioSetPartSettings1Group.SelectedItem && tone[1] == (String)cbStudioSetPartSettings1Category.SelectedItem)
                    {
                        cbStudioSetPartSettings1Program.Items.Add(tone[3]);
                    }
                }
            }
            catch (Exception e)
            {

            }
            PopHandleControlEvents();
        }

        //private Int32 MsbToCbIndex(Int32 msb)
        //{
        //    t.Trace("private Int32 MsbToCbIndex()");
        //    Int32 index = 0;
        //    foreach (ComboBoxItem item in cbStudioSetPartSettings1Group.Items)
        //    {
        //        if ((byte)item.Tag == msb)
        //        {
        //            return index;
        //        }
        //        index++;
        //    }
        //    return -1;
        //}

        //private Int32 LsbToCbIndex(Int32 lsb)
        //{
        //    t.Trace("private Int32 LsbToCbIndex()");
        //    Int32 index = 0;
        //    foreach (ComboBoxItem item in cbStudioSetPartSettings1Category.Items)
        //    {
        //        if ((byte)item.Tag == lsb)
        //        {
        //            return index;
        //        }
        //        index++;
        //    }
        //    return -1;
        //}

        private Int32 MaxValidPc()
        {
            t.Trace("private Int32 MaxValidPc()");
            Int32 max = 1;
            switch (commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectMSB)
            {
                case 85:
                    max = 64;
                    break;
                case 86:
                    switch (commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
                    {
                        case 0:
                            max = 32;
                            break;
                        case 64:
                            max = 14;
                            break;
                    }
                    break;
                case 87:
                    switch (commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
                    {
                        case 0:
                        case 1:
                            max = 128;
                            break;
                        case 64:
                        case 65:
                        case 66:
                        case 67:
                        case 68:
                        case 69:
                        case 70:
                            max = 128;
                            break;
                    }
                    break;
                case 88:
                    switch (commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
                    {
                        case 0:
                            max = 64;
                            break;
                        case 64:
                            max = 26;
                            break;
                        case 101:
                            max = 7;
                            break;
                    }
                    break;
                case 89:
                    switch (commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
                    {
                        case 0:
                        case 1:
                            max = 128;
                            break;
                        case 64:
                        case 65:
                            max = 128;
                            break;
                        case 96:
                            max = 17;
                            break;
                    }
                    break;
                case 92:
                    switch (commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
                    {
                        case 0:
                            max = 79;
                            break;
                        case 2:
                            max = 12;
                            break;
                        case 4:
                            max = 34;
                            break;
                        case 7:
                            max = 5;
                            break;
                        case 11:
                            max = 11;
                            break;
                        case 15:
                            max = 21;
                            break;
                        case 19:
                            max = 12;
                            break;
                    }
                    break;
                case 93:
                    switch (commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
                    {
                        case 0:
                            max = 41;
                            break;
                        case 1:
                            max = 50;
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            max = 128;
                            break;
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            max = 128;
                            break;
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                            max = 128;
                            break;
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                            max = 128;
                            break;
                        case 19:
                        case 20:
                        case 21:
                            max = 128;
                            break;
                        case 22:
                            max = 30;
                            break;
                        case 23:
                            max = 100;
                            break;
                        case 24:
                            max = 42;
                            break;
                        case 26:
                            max = 50;
                            break;
                    }
                    break;
                case 95:
                    switch (commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 64:
                        case 65:
                        case 66:
                        case 67:
                        case 68:
                        case 69:
                        case 70:
                        case 71:
                            max = 128;
                            break;
                        case 72:
                            max = 85;
                            break;
                    }
                    break;
                case 96:
                    max = 19;
                    break;
                case 97:
                    switch (commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            max = 128;
                            break;
                    }
                    break;
                case 120:
                    max = 9;
                    break;
                case 121:
                    switch (commonState.StudioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
                    {
                        case 0:
                        case 1:
                            max = 128;
                            break;
                    }
                    break;
            }
            return max;
        }

        private String ByteToHexString(byte b)
        {
            t.Trace("private String ByteToHexString (" + "byte" + b + ", " + ")");
            String chars = "0123456789ABCDEF";
            return (chars.ToCharArray()[b / 16].ToString() + chars.ToCharArray()[b % 16].ToString());
        }

        public Int32 FourByteRightNibbleToInt32(Int32 i)
        {
            t.Trace("public Int32 FourByteRightNibbleToInt32 (" + "Int32" + i + ", " + ")");
            return (rawData[i++] & 0x0f) * 16 * 16 * 16 +
                (rawData[i++] & 0x0f) * 16 * 16 +
                (rawData[i++] & 0x0f) * 16 +
                (rawData[i++] & 0x0f) - 32768;
        }

        private byte[] Int32ToTwoByteArray(Int32 value)
        {
            t.Trace("private byte[] Int32ToTwoByteArray (" + "Int32" + value + ", " + ")");
            byte[] result = new byte[2];
            result[0] = (byte)(value / 16);
            result[1] = (byte)(value % 16);
            return result;
        }

        private String CalculateChorusPreDelay(Int32 Value)
        {
            t.Trace("private String CalculateChorusPreDelay (" + "Int32" + Value + ", " + ")");
            Double stringValue;
            if (Value < 50)
            {
                stringValue = (Double)Value / 10;
            }
            else if (Value < 60)
            {
                stringValue = (Double)Value / 2 - 20;
            }
            else if (Value < 100)
            {
                stringValue = Value - 50;
            }
            else
            {
                stringValue = Value * 2 - 150;
            }
            return "Pre delay " + stringValue.ToString() + " ms";
        }

        private String CalculateTimeHz(Int32 Value)
        {
            t.Trace("private String CalculateTimeHz (" + "Int32" + Value + ", " + ")");
            // 0.05 - 10 in 200 0.05 steps 
            Double stringValue;
            stringValue = Math.Round((Value / 20.0), 2);
            return stringValue.ToString();
        }

        private String CalculateTimeNote(Int32 Value)
        {
            t.Trace("private String CalculateTimeNote (" + "Int32" + Value + ", " + ")");
            // Different note length values in 22 steps denoted in list:
            String[] noteLengthValues = {
                "1/64T", "1/64", "1/32T", "1/32", "1/16T", "1/32.",
                "1/16", "1/8T", "1/16.", "1/8", "1/4T", "1/8.",
                "1/4", "1/2T", "1/4.", "1/2", "1/1T", "1/2.",
                "1/1", "2/1T", "1/1.", "2/1" };
            if (Value > -1 && Value < 22)
            {
                return noteLengthValues[Value];
            }
            else
            {
                return "Rate ??? note";
            }
        }

        private void ResetComboBoxes()
        {
            t.Trace("private void ResetComboBoxes()");
            Boolean currentHandleControlEvents = handleControlEvents;
            PushHandleControlEvents();
            cbChorusChorusFilterCutoffFrequency.SelectedIndex = -1;
            cbChorusChorusFilterType.SelectedIndex = -1;
            cbChorusChorusRateHzNote.SelectedIndex = -1;
            cbChorusDelayCenterMsNote.SelectedIndex = -1;
            cbChorusDelayHFDamp.SelectedIndex = -1;
            cbChorusDelayLeftMsNote.SelectedIndex = -1;
            cbChorusDelayRightMsNote.SelectedIndex = -1;
            cbChorusOutputAssign.SelectedIndex = -1;
            cbStudioSetChorusType.SelectedIndex = -1;
            cbColumn1Selector.SelectedIndex = -1;
            cbDrumCompEQ1OutputAssign.SelectedIndex = -1;
            cbDrumCompEQ2OutputAssign.SelectedIndex = -1;
            cbDrumCompEQ3OutputAssign.SelectedIndex = -1;
            cbDrumCompEQ4OutputAssign.SelectedIndex = -1;
            cbDrumCompEQ5OutputAssign.SelectedIndex = -1;
            cbDrumCompEQ6OutputAssign.SelectedIndex = -1;
            cbDrumCompEQPart.SelectedIndex = -1;
            cbStudioSetMotionalSurroundExtPartControl.SelectedIndex = -1;
            cbStudioSetPartSelector.SelectedIndex = -1;
            cbStudioSetReverbOutputAssign.SelectedIndex = -1;
            cbStudioSetReverbType.SelectedIndex = -1;
            cbStudioSetMotionalSurroundRoomSize.SelectedIndex = -1;
            cbStudioSetMotionalSurroundRoomType.SelectedIndex = -1;
            cbSoloPart.SelectedIndex = -1;
            cbStudioSetSelector.SelectedIndex = -1;
            cbToneControl1.SelectedIndex = -1;
            cbToneControl2.SelectedIndex = -1;
            cbToneControl3.SelectedIndex = -1;
            cbToneControl4.SelectedIndex = -1;
            handleControlEvents = currentHandleControlEvents;
        }

        private void SendHashMarkedMessage(byte[] Address, Double Value)
        {
            t.Trace("private void SendHashMarkedMessage (" + "byte[]" + Address + ", " + "Double" + Value + ", " + ")");
            // Since there are no values that takes more than 2 bytes, set first two to 0x08 and 0x00
            // as they are already set in the INTEGRA-7. Split Value into MSB and LSB and send.
            byte[] value = { 0x08, 0x00, (byte)((Value) / 16), (byte)((Value) % 16) };
            byte[] bytes = commonState.Midi.SystemExclusiveDT1Message(Address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.Midi.SendSystemExclusive(bytes);
        }

        private void UpdateChorusChorusSubwindow()
        {
            t.Trace("private void UpdateChorusChorusSubwindow()");

            switch (commonState.StudioSet.CommonChorus.Type)
            {
                case 0:
                    // Off:
                    ChorusChorus.IsVisible = false;
                    ChorusDelay.IsVisible = false;
                    ChorusGM2Chorus.IsVisible = false;
                    break;
                case 1:
                    // Chorus:
                    //ReadStudioSetChorus(1);
                    ChorusChorus.IsVisible = true;
                    ChorusDelay.IsVisible = false;
                    ChorusGM2Chorus.IsVisible = false;
                    break;
                case 2:
                    // Delay:
                    //ReadStudioSetChorus(2);
                    ChorusChorus.IsVisible = false;
                    ChorusDelay.IsVisible = true;
                    ChorusGM2Chorus.IsVisible = false;
                    break;
                case 3:
                    // GM2 chorus:
                    //ReadStudioSetChorus(3);
                    ChorusChorus.IsVisible = false;
                    ChorusDelay.IsVisible = false;
                    ChorusGM2Chorus.IsVisible = true;
                    break;
            }
        }

        private void UpdateChorusReverbSubwindow()
        {
            t.Trace("private void UpdateChorusReverbSubwindow()");
            // Off:
            StudioSetReverbReverb.IsVisible = false;
            StudioSetReverbGM2.IsVisible = false;

            if (commonState.StudioSet.CommonReverb.Type == 5)
            {
                StudioSetReverbGM2.IsVisible = true;
            }
            else if (commonState.StudioSet.CommonReverb.Type > 0)
            {
                StudioSetReverbReverb.IsVisible = true;
            }
            ReadStudioSetReverb((byte)cbStudioSetReverbType.SelectedIndex);
        }

        private void UpdateControlsAndIntegra7FromClasses(Int32 part)
        {
            if (cbToneControl1.SelectedIndex != commonState.StudioSet.Common.ToneControlSource[0])
            {
                cbToneControl1.SelectedIndex = commonState.StudioSet.Common.ToneControlSource[0];
                SetStudioSetCommonToneControl1(commonState.StudioSet.Common.ToneControlSource[0]);
            }
            if (cbToneControl2.SelectedIndex != commonState.StudioSet.Common.ToneControlSource[1])
            {
                cbToneControl2.SelectedIndex = commonState.StudioSet.Common.ToneControlSource[1];
                SetStudioSetCommonToneControl2(commonState.StudioSet.Common.ToneControlSource[1]);
            }
            if (cbToneControl3.SelectedIndex != commonState.StudioSet.Common.ToneControlSource[2])
            {
                cbToneControl3.SelectedIndex = commonState.StudioSet.Common.ToneControlSource[2];
                SetStudioSetCommonToneControl3(commonState.StudioSet.Common.ToneControlSource[2]);
            }
            if (cbToneControl4.SelectedIndex != commonState.StudioSet.Common.ToneControlSource[3])
            {
                cbToneControl4.SelectedIndex = commonState.StudioSet.Common.ToneControlSource[3];
                SetStudioSetCommonToneControl4(commonState.StudioSet.Common.ToneControlSource[3]);
            }
            if (slTempo.Value != commonState.StudioSet.Common.Tempo)
            {
                slTempo.Value = commonState.StudioSet.Common.Tempo;
                SetStudioSetCommonTempo(commonState.StudioSet.Common.Tempo);
            }
            if (cbSoloPart.SelectedIndex != commonState.StudioSet.Common.SoloPart)
            {
                cbSoloPart.SelectedIndex = commonState.StudioSet.Common.SoloPart;
                SetStudioSetCommonSoloPart(commonState.StudioSet.Common.SoloPart);
            }
            if (cbReverb.IsChecked != commonState.StudioSet.Common.Reverb)
            {
                cbReverb.IsChecked = commonState.StudioSet.Common.Reverb;
                SetStudioSetCommonReverb((Boolean)commonState.StudioSet.Common.Reverb);
            }
            if (cbChorus.IsChecked != commonState.StudioSet.Common.Chorus)
            {
                cbChorus.IsChecked = commonState.StudioSet.Common.Chorus;
                SetStudioSetCommonChorus((Boolean)commonState.StudioSet.Common.Chorus);
            }
            if (cbMasterEQ.IsChecked != commonState.StudioSet.Common.MasterEqualizer)
            {
                cbMasterEQ.IsChecked = commonState.StudioSet.Common.MasterEqualizer;
                SetStudioSetCommonMasterEQ((Boolean)commonState.StudioSet.Common.MasterEqualizer);
            }
            if (cbDrumCompEQPart.SelectedIndex != commonState.StudioSet.Common.DrumCompressorAndEqualizerPart)
            {
                cbDrumCompEQPart.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerPart;
                SetStudioSetCommonEQPart(commonState.StudioSet.Common.DrumCompressorAndEqualizerPart);
            }
            if (cbDrumCompEQ1OutputAssign.SelectedIndex != commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[0])
            {
                cbDrumCompEQ1OutputAssign.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[0];
                SetStudioSetCommonEQ1OutputAssign(commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[0]);
            }
            if (cbDrumCompEQ2OutputAssign.SelectedIndex != commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[1])
            {
                cbDrumCompEQ2OutputAssign.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[1];
                SetStudioSetCommonEQ2OutputAssign(commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[1]);
            }
            if (cbDrumCompEQ3OutputAssign.SelectedIndex != commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[2])
            {
                cbDrumCompEQ3OutputAssign.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[2];
                SetStudioSetCommonEQ3OutputAssign(commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[2]);
            }
            if (cbDrumCompEQ4OutputAssign.SelectedIndex != commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[3])
            {
                cbDrumCompEQ4OutputAssign.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[3];
                SetStudioSetCommonEQ4OutputAssign(commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[3]);
            }
            if (cbDrumCompEQ5OutputAssign.SelectedIndex != commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[4])
            {
                cbDrumCompEQ5OutputAssign.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[4];
                SetStudioSetCommonEQ5OutputAssign(commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[4]);
            }
            if (cbDrumCompEQ6OutputAssign.SelectedIndex != commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[5])
            {
                cbDrumCompEQ6OutputAssign.SelectedIndex = commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[5];
                SetStudioSetCommonEQ6OutputAssign(commonState.StudioSet.Common.DrumCompressorAndEqualizerOutputAssign[5]);
            }
            if (cbDrumCompEQ.IsChecked != commonState.StudioSet.Common.DrumCompressorAndEqualizer)
            {
                cbDrumCompEQ.IsChecked = commonState.StudioSet.Common.DrumCompressorAndEqualizer;
                SetStudioSetCompEQ_Click((Boolean)commonState.StudioSet.Common.DrumCompressorAndEqualizer);
            }
            if (cbExtPartMute.IsChecked != commonState.StudioSet.Common.ExternalPartMute)
            {
                cbExtPartMute.IsChecked = commonState.StudioSet.Common.ExternalPartMute;
                SetStudioSetExtPartMute((Boolean)commonState.StudioSet.Common.ExternalPartMute);
            }
            if (slExtPartLevel.Value != commonState.StudioSet.Common.ExternalPartLevel)
            {
                slExtPartLevel.Value = commonState.StudioSet.Common.ExternalPartLevel;
                SetStudioSetExtPartLevel(commonState.StudioSet.Common.ExternalPartLevel);
            }
            if (slExtPartChorusSend.Value != commonState.StudioSet.Common.ExternalPartChorusSendLevel)
            {
                slExtPartChorusSend.Value = commonState.StudioSet.Common.ExternalPartChorusSendLevel;
                SetStudioSetExtPartChorusSend(commonState.StudioSet.Common.ExternalPartChorusSendLevel);
            }
            if (slExtPartReverbSend.Value != commonState.StudioSet.Common.ExternalPartReverbSendLevel)
            {
                slExtPartReverbSend.Value = commonState.StudioSet.Common.ExternalPartReverbSendLevel;
                SetStudioSetExtPartReverbSend(commonState.StudioSet.Common.ExternalPartReverbSendLevel);
            }
            if (slSystemCommonMasterTune.Value != commonState.StudioSet.SystemCommon.MasterTune)
            {
                slSystemCommonMasterTune.Value = commonState.StudioSet.SystemCommon.MasterTune;
                SetStudioSetSystemCommonMasterTune((Int32)slSystemCommonMasterTune.Value);
            }
            if (slSystemCommonMasterKeyShift.Value != commonState.StudioSet.SystemCommon.MasterKeyShift)
            {
                slSystemCommonMasterKeyShift.Value = commonState.StudioSet.SystemCommon.MasterKeyShift;
                SetStudioSetSystemCommonMasterKeyShift((Int32)slSystemCommonMasterKeyShift.Value);
            }
            if (slSystemCommonMasterLevel.Value != commonState.StudioSet.SystemCommon.MasterLevel)
            {
                slSystemCommonMasterLevel.Value = commonState.StudioSet.SystemCommon.MasterLevel;
                SetStudioSetSystemCommonMasterLevel((Int32)slSystemCommonMasterLevel.Value);
            }
            if (cbSystemCommonScaleTuneSwitch.IsChecked != commonState.StudioSet.SystemCommon.ScaleTuneSwitch)
            {
                cbSystemCommonScaleTuneSwitch.IsChecked = commonState.StudioSet.SystemCommon.ScaleTuneSwitch;
                SetStudioSetSystemCommonScaleTuneSwitch((Boolean)cbSystemCommonScaleTuneSwitch.IsChecked);
            }
            if (cbSystemCommonStudioSetControlChannel.SelectedIndex != commonState.StudioSet.SystemCommon.StudioSetControlChannel)
            {
                cbSystemCommonStudioSetControlChannel.SelectedIndex = commonState.StudioSet.SystemCommon.StudioSetControlChannel;
                SetStudioSetSystemCommonStudioSetControlChannel(cbSystemCommonStudioSetControlChannel.SelectedIndex);
            }
            if (cbSystemCommonSystemControlSource1.SelectedIndex != commonState.StudioSet.SystemCommon.SystemControl1Source)
            {
                cbSystemCommonSystemControlSource1.SelectedIndex = commonState.StudioSet.SystemCommon.SystemControl1Source;
                SetStudioSetSystemCommonSystemControlSource1(cbSystemCommonSystemControlSource1.SelectedIndex);
            }
            if (cbSystemCommonSystemControlSource2.SelectedIndex != commonState.StudioSet.SystemCommon.SystemControl2Source)
            {
                cbSystemCommonSystemControlSource2.SelectedIndex = commonState.StudioSet.SystemCommon.SystemControl2Source;
                SetStudioSetSystemCommonSystemControlSource2(cbSystemCommonSystemControlSource2.SelectedIndex);
            }
            if (cbSystemCommonSystemControlSource3.SelectedIndex != commonState.StudioSet.SystemCommon.SystemControl3Source)
            {
                cbSystemCommonSystemControlSource3.SelectedIndex = commonState.StudioSet.SystemCommon.SystemControl3Source;
                SetStudioSetSystemCommonSystemControlSource3(cbSystemCommonSystemControlSource3.SelectedIndex);
            }
            if (cbSystemCommonSystemControlSource4.SelectedIndex != commonState.StudioSet.SystemCommon.SystemControl4Source)
            {
                cbSystemCommonSystemControlSource4.SelectedIndex = commonState.StudioSet.SystemCommon.SystemControl4Source;
                SetStudioSetSystemCommonSystemControlSource4(cbSystemCommonSystemControlSource4.SelectedIndex);
            }
            if (cbSystemCommonControlSource.SelectedIndex != commonState.StudioSet.SystemCommon.ControlSource)
            {
                cbSystemCommonControlSource.SelectedIndex = commonState.StudioSet.SystemCommon.ControlSource;
                SetStudioSetSystemCommonControlSource(cbSystemCommonControlSource.SelectedIndex);
            }
            if (cbSystemCommonSystemClockSource.SelectedIndex != commonState.StudioSet.SystemCommon.SystemClockSource)
            {
                cbSystemCommonSystemClockSource.SelectedIndex = commonState.StudioSet.SystemCommon.SystemClockSource;
                SetStudioSetSystemCommonSystemClockSource(cbSystemCommonSystemClockSource.SelectedIndex);
            }
            if (slSystemCommonSystemTempo.Value != commonState.StudioSet.SystemCommon.SystemTempo)
            {
                slSystemCommonSystemTempo.Value = commonState.StudioSet.SystemCommon.SystemTempo;
                SetStudioSetSystemCommonSystemTempo((Int32)slSystemCommonSystemTempo.Value);
            }
            if (cbSystemCommonTempoAssignSource.SelectedIndex != commonState.StudioSet.SystemCommon.TempoAssignSource)
            {
                cbSystemCommonTempoAssignSource.SelectedIndex = commonState.StudioSet.SystemCommon.TempoAssignSource;
                SetStudioSetSystemCommonTempoAssignSource(cbSystemCommonTempoAssignSource.SelectedIndex);
            }
            if (cbSystemCommonReceiveProgramChange.IsChecked != commonState.StudioSet.SystemCommon.ReceiveProgramChange)
            {
                cbSystemCommonReceiveProgramChange.IsChecked = commonState.StudioSet.SystemCommon.ReceiveProgramChange;
                SetStudioSetSystemCommonReceiveProgramChange((Boolean)cbSystemCommonReceiveProgramChange.IsChecked);
            }
            if (cbSystemCommonReceiveBankSelect.IsChecked != commonState.StudioSet.SystemCommon.ReceiveBankSelect)
            {
                cbSystemCommonReceiveBankSelect.IsChecked = commonState.StudioSet.SystemCommon.ReceiveBankSelect;
                SetStudioSetSystemCommonReceiveBankSelect((Boolean)cbSystemCommonReceiveBankSelect.IsChecked);
            }
            if (cbSystemCommonSurroundCenterSpeakerSwitch.IsChecked != commonState.StudioSet.SystemCommon.SurroundCenterSpeakerSwitch)
            {
                cbSystemCommonSurroundCenterSpeakerSwitch.IsChecked = commonState.StudioSet.SystemCommon.SurroundCenterSpeakerSwitch;
                SetStudioSetSystemCommonSurroundCenterSpeakerSwitch((Boolean)cbSystemCommonSurroundCenterSpeakerSwitch.IsChecked);
            }
            if (cbSystemCommonSurroundSubWooferSwitch.IsChecked != commonState.StudioSet.SystemCommon.SurroundSubWooferSwitch)
            {
                cbSystemCommonSurroundSubWooferSwitch.IsChecked = commonState.StudioSet.SystemCommon.SurroundSubWooferSwitch;
                SetStudioSetSystemCommonSurroundSubWooferSwitch((Boolean)cbSystemCommonSurroundSubWooferSwitch.IsChecked);
            }
            if (cbSystemCommonStereoOutputMode.SelectedIndex != commonState.StudioSet.SystemCommon.StereoOutputMode)
            {
                cbSystemCommonStereoOutputMode.SelectedIndex = commonState.StudioSet.SystemCommon.StereoOutputMode;
                SetStudioSetSystemCommonStereoOutputMode(cbSystemCommonStereoOutputMode.SelectedIndex);
            }
            if (slVoiceReserve01.Value != commonState.StudioSet.Common.VoiceReserve[0])
            {
                slVoiceReserve01.Value = commonState.StudioSet.Common.VoiceReserve[0];
                SetStudioSetVoiceReserve01((Int32)slVoiceReserve01.Value);
            }
            if (slVoiceReserve02.Value != commonState.StudioSet.Common.VoiceReserve[1])
            {
                slVoiceReserve02.Value = commonState.StudioSet.Common.VoiceReserve[1];
                SetStudioSetVoiceReserve02((Int32)slVoiceReserve02.Value);
            }
            if (slVoiceReserve03.Value != commonState.StudioSet.Common.VoiceReserve[2])
            {
                slVoiceReserve03.Value = commonState.StudioSet.Common.VoiceReserve[2];
                SetStudioSetVoiceReserve03((Int32)slVoiceReserve03.Value);
            }
            if (slVoiceReserve04.Value != commonState.StudioSet.Common.VoiceReserve[3])
            {
                slVoiceReserve04.Value = commonState.StudioSet.Common.VoiceReserve[3];
                SetStudioSetVoiceReserve04((Int32)slVoiceReserve04.Value);
            }
            if (slVoiceReserve05.Value != commonState.StudioSet.Common.VoiceReserve[4])
            {
                slVoiceReserve05.Value = commonState.StudioSet.Common.VoiceReserve[4];
                SetStudioSetVoiceReserve05((Int32)slVoiceReserve05.Value);
            }
            if (slVoiceReserve06.Value != commonState.StudioSet.Common.VoiceReserve[5])
            {
                slVoiceReserve06.Value = commonState.StudioSet.Common.VoiceReserve[5];
                SetStudioSetVoiceReserve06((Int32)slVoiceReserve06.Value);
            }
            if (slVoiceReserve07.Value != commonState.StudioSet.Common.VoiceReserve[6])
            {
                slVoiceReserve07.Value = commonState.StudioSet.Common.VoiceReserve[6];
                SetStudioSetVoiceReserve07((Int32)slVoiceReserve07.Value);
            }
            if (slVoiceReserve08.Value != commonState.StudioSet.Common.VoiceReserve[7])
            {
                slVoiceReserve08.Value = commonState.StudioSet.Common.VoiceReserve[7];
                SetStudioSetVoiceReserve08((Int32)slVoiceReserve08.Value);
            }
            if (slVoiceReserve09.Value != commonState.StudioSet.Common.VoiceReserve[8])
            {
                slVoiceReserve09.Value = commonState.StudioSet.Common.VoiceReserve[8];
                SetStudioSetVoiceReserve09((Int32)slVoiceReserve09.Value);
            }
            if (slVoiceReserve10.Value != commonState.StudioSet.Common.VoiceReserve[9])
            {
                slVoiceReserve10.Value = commonState.StudioSet.Common.VoiceReserve[9];
                SetStudioSetVoiceReserve10((Int32)slVoiceReserve10.Value);
            }
            if (slVoiceReserve11.Value != commonState.StudioSet.Common.VoiceReserve[10])
            {
                slVoiceReserve11.Value = commonState.StudioSet.Common.VoiceReserve[10];
                SetStudioSetVoiceReserve11((Int32)slVoiceReserve11.Value);
            }
            if (slVoiceReserve12.Value != commonState.StudioSet.Common.VoiceReserve[11])
            {
                slVoiceReserve12.Value = commonState.StudioSet.Common.VoiceReserve[11];
                SetStudioSetVoiceReserve12((Int32)slVoiceReserve12.Value);
            }
            if (slVoiceReserve13.Value != commonState.StudioSet.Common.VoiceReserve[12])
            {
                slVoiceReserve13.Value = commonState.StudioSet.Common.VoiceReserve[12];
                SetStudioSetVoiceReserve13((Int32)slVoiceReserve13.Value);
            }
            if (slVoiceReserve14.Value != commonState.StudioSet.Common.VoiceReserve[13])
            {
                slVoiceReserve14.Value = commonState.StudioSet.Common.VoiceReserve[13];
                SetStudioSetVoiceReserve14((Int32)slVoiceReserve14.Value);
            }
            if (slVoiceReserve15.Value != commonState.StudioSet.Common.VoiceReserve[14])
            {
                slVoiceReserve15.Value = commonState.StudioSet.Common.VoiceReserve[14];
                SetStudioSetVoiceReserve15((Int32)slVoiceReserve15.Value);
            }
            if (slVoiceReserve16.Value != commonState.StudioSet.Common.VoiceReserve[15])
            {
                slVoiceReserve16.Value = commonState.StudioSet.Common.VoiceReserve[15];
                SetStudioSetVoiceReserve16((Int32)slVoiceReserve16.Value);
            }
            //if (cbStudioSetChorusType.SelectedIndex != commonState.studioSet.CommonChorus.Type)
            {
                cbStudioSetChorusType.SelectedIndex = commonState.StudioSet.CommonChorus.Type;
                SetStudioSetStudioSetChorusType(cbStudioSetChorusType.SelectedIndex);
            }
            if (slChorusLevel.Value != commonState.StudioSet.CommonChorus.Level)
            {
                slChorusLevel.Value = commonState.StudioSet.CommonChorus.Level;
                SetStudioSetChorusLevel((Int32)slChorusLevel.Value);
            }
            if (cbChorusOutputAssign.SelectedIndex != commonState.StudioSet.CommonChorus.OutputAssign)
            {
                cbChorusOutputAssign.SelectedIndex = commonState.StudioSet.CommonChorus.OutputAssign;
                SetStudioSetChorusOutputAssign(cbChorusOutputAssign.SelectedIndex);
            }
            if (cbChorusChorusFilterType.SelectedIndex != commonState.StudioSet.CommonChorus.Chorus.FilterType)
            {
                cbChorusChorusFilterType.SelectedIndex = commonState.StudioSet.CommonChorus.Chorus.FilterType;
                SetStudioSetChorusChorusFilterType(cbChorusChorusFilterType.SelectedIndex);
            }
            if (cbChorusChorusFilterCutoffFrequency.SelectedIndex != commonState.StudioSet.CommonChorus.Chorus.FilterCutoffFrequency)
            {
                cbChorusChorusFilterCutoffFrequency.SelectedIndex = commonState.StudioSet.CommonChorus.Chorus.FilterCutoffFrequency;
                SetStudioSetChorusChorusFilterCutoffFrequency(cbChorusChorusFilterCutoffFrequency.SelectedIndex);
            }
            if (slChorusChorusPreDelay.Value != commonState.StudioSet.CommonChorus.Chorus.PreDelay)
            {
                slChorusChorusPreDelay.Value = commonState.StudioSet.CommonChorus.Chorus.PreDelay;
                SetStudioSetChorusChorusPreDelay((Int32)slChorusChorusPreDelay.Value);
            }
            if (cbChorusChorusRateHzNote.SelectedIndex != commonState.StudioSet.CommonChorus.Chorus.RateHzNote)
            {
                try
                {
                    cbChorusChorusRateHzNote.SelectedIndex = commonState.StudioSet.CommonChorus.Chorus.RateHzNote;
                    SetStudioSetChorusChorusRate(cbChorusChorusRateHzNote.SelectedIndex);
                }
                catch (Exception e)
                {
                    t.Trace(e.Message + " setting cbChorusChorusRateHzNote");
                }
            }
            if (slChorusChorusRateHz.Value != commonState.StudioSet.CommonChorus.Chorus.RateHz)
            {
                slChorusChorusRateHz.Value = commonState.StudioSet.CommonChorus.Chorus.RateHz;
                SetStudioSetChorusChorusRateHz((Int32)slChorusChorusRateHz.Value);
            }
            if (slChorusChorusRateNote.Value != commonState.StudioSet.CommonChorus.Chorus.RateNote)
            {
                slChorusChorusRateNote.Value = commonState.StudioSet.CommonChorus.Chorus.RateNote;
                SetStudioSetChorusChorusRateNote((Int32)slChorusChorusRateNote.Value);
            }
            if (slChorusChorusDepth.Value != commonState.StudioSet.CommonChorus.Chorus.Depth)
            {
                slChorusChorusDepth.Value = commonState.StudioSet.CommonChorus.Chorus.Depth;
                SetStudioSetChorusChorusDepth((Int32)slChorusChorusDepth.Value);
            }
            if (slChorusChorusPhase.Value != commonState.StudioSet.CommonChorus.Chorus.Phase)
            {
                slChorusChorusPhase.Value = commonState.StudioSet.CommonChorus.Chorus.Phase;
                SetStudioSetChorusChorusPhase((Int32)slChorusChorusPhase.Value);
            }
            if (slChorusChorusFeedback.Value != commonState.StudioSet.CommonChorus.Chorus.Feedback)
            {
                slChorusChorusFeedback.Value = commonState.StudioSet.CommonChorus.Chorus.Feedback;
                SetStudioSetChorusChorusFeedback((Int32)slChorusChorusFeedback.Value);
            }
            if (cbChorusDelayLeftMsNote.SelectedIndex != commonState.StudioSet.CommonChorus.Delay.LeftMsNote)
            {
                try
                {
                    cbChorusDelayLeftMsNote.SelectedIndex = commonState.StudioSet.CommonChorus.Delay.LeftMsNote;
                    SetStudioSetChorusDelayLeft(cbChorusDelayLeftMsNote.SelectedIndex);
                }
                catch (Exception e)
                {
                    t.Trace(e.Message + " setting cbChorusDelayLeftMsNote");
                }
            }
            if (slChorusDelayLeftHz.Value != commonState.StudioSet.CommonChorus.Delay.LeftMs)
            {
                slChorusDelayLeftHz.Value = commonState.StudioSet.CommonChorus.Delay.LeftMs;
                SetStudioSetChorusDelayLeftHz((Int32)slChorusDelayLeftHz.Value);
            }
            if (slChorusDelayLeftNote.Value != commonState.StudioSet.CommonChorus.Delay.LeftNote)
            {
                slChorusDelayLeftNote.Value = commonState.StudioSet.CommonChorus.Delay.LeftNote;
                SetStudioSetChorusDelayLeftNote((Int32)slChorusDelayLeftNote.Value);
            }
            if (cbChorusDelayRightMsNote.SelectedIndex != commonState.StudioSet.CommonChorus.Delay.RightMsNote)
            {
                try
                {
                    cbChorusDelayRightMsNote.SelectedIndex = commonState.StudioSet.CommonChorus.Delay.RightMsNote;
                    SetStudioSetChorusDelayRight(cbChorusDelayRightMsNote.SelectedIndex);
                }
                catch (Exception e)
                {
                    t.Trace(e.Message + " setting cbChorusDelayRightMsNote");
                }
            }
            if (slChorusDelayRightHz.Value != commonState.StudioSet.CommonChorus.Delay.RightMs)
            {
                slChorusDelayRightHz.Value = commonState.StudioSet.CommonChorus.Delay.RightMs;
                SetStudioSetChorusDelayRightHz((Int32)slChorusDelayRightHz.Value);
            }
            if (slChorusDelayRightNote.Value != commonState.StudioSet.CommonChorus.Delay.RightNote)
            {
                slChorusDelayRightNote.Value = commonState.StudioSet.CommonChorus.Delay.RightNote;
                SetStudioSetChorusDelayRightNote((Int32)slChorusDelayRightNote.Value);
            }
            if (cbChorusDelayCenterMsNote.SelectedIndex != commonState.StudioSet.CommonChorus.Delay.CenterMsNote)
            {
                try
                {
                    cbChorusDelayCenterMsNote.SelectedIndex = commonState.StudioSet.CommonChorus.Delay.CenterMsNote;
                    SetStudioSetChorusDelayCenter(cbChorusDelayCenterMsNote.SelectedIndex);
                }
                catch (Exception e)
                {
                    t.Trace(e.Message + " setting cbChorusDelayCenterMsNote");
                }
            }
            if (slChorusDelayCenterHz.Value != commonState.StudioSet.CommonChorus.Delay.CenterMs)
            {
                slChorusDelayCenterHz.Value = commonState.StudioSet.CommonChorus.Delay.CenterMs;
                SetStudioSetChorusDelayCenterHz((Int32)slChorusDelayCenterHz.Value);
            }
            if (slChorusDelayCenterNote.Value != commonState.StudioSet.CommonChorus.Delay.CenterNote)
            {
                slChorusDelayCenterNote.Value = commonState.StudioSet.CommonChorus.Delay.CenterNote;
                SetStudioSetChorusDelayCenterNote((Int32)slChorusDelayCenterNote.Value);
            }
            if (slChorusDelayCenterFeedback.Value != commonState.StudioSet.CommonChorus.Delay.CenterFeedback)
            {
                slChorusDelayCenterFeedback.Value = 2 * (commonState.StudioSet.CommonChorus.Delay.CenterFeedback - 49);
                SetStudioSetChorusDelayCenterFeedback((Int32)slChorusDelayCenterFeedback.Value);
            }
            if (cbChorusDelayHFDamp.SelectedIndex != commonState.StudioSet.CommonChorus.Delay.HFDamp)
            {
                cbChorusDelayHFDamp.SelectedIndex = commonState.StudioSet.CommonChorus.Delay.HFDamp;
                SetStudioSetChorusDelayHFDamp(cbChorusDelayHFDamp.SelectedIndex);
            }
            if (slChorusDelayLeftLevel.Value != commonState.StudioSet.CommonChorus.Delay.LeftLevel)
            {
                slChorusDelayLeftLevel.Value = commonState.StudioSet.CommonChorus.Delay.LeftLevel;
                SetStudioSetChorusDelayLeftLevel((Int32)slChorusDelayLeftLevel.Value);
            }
            if (slChorusDelayRightLevel.Value != commonState.StudioSet.CommonChorus.Delay.RightLevel)
            {
                slChorusDelayRightLevel.Value = commonState.StudioSet.CommonChorus.Delay.RightLevel;
                SetStudioSetChorusDelayRightLevel((Int32)slChorusDelayRightLevel.Value);
            }
            if (slChorusDelayCenterLevel.Value != commonState.StudioSet.CommonChorus.Delay.CenterLevel)
            {
                slChorusDelayCenterLevel.Value = commonState.StudioSet.CommonChorus.Delay.CenterLevel;
                SetStudioSetChorusDelayCenterLevel((Int32)slChorusDelayCenterLevel.Value);
            }
            if (slChorusGM2ChorusPreLPF.Value != commonState.StudioSet.CommonChorus.Gm2Chorus.PreLPF)
            {
                slChorusGM2ChorusPreLPF.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.PreLPF;
                SetStudioSetChorusGM2ChorusPreLPF((Int32)slChorusGM2ChorusPreLPF.Value);
            }
            if (slChorusGM2ChorusLevel.Value != commonState.StudioSet.CommonChorus.Gm2Chorus.Level)
            {
                slChorusGM2ChorusLevel.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Level;
                SetStudioSetChorusGM2ChorusLevel((Int32)slChorusGM2ChorusLevel.Value);
            }
            if (slChorusGM2ChorusFeedback.Value != commonState.StudioSet.CommonChorus.Gm2Chorus.Feedback)
            {
                slChorusGM2ChorusFeedback.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Feedback;
                SetStudioSetChorusGM2ChorusFeedback((Int32)slChorusGM2ChorusFeedback.Value);
            }
            if (slChorusGM2ChorusDelay.Value != commonState.StudioSet.CommonChorus.Gm2Chorus.Delay)
            {
                slChorusGM2ChorusDelay.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Delay;
                SetStudioSetChorusGM2ChorusDelay((Int32)slChorusGM2ChorusDelay.Value);
            }
            if (slChorusGM2ChorusRate.Value != commonState.StudioSet.CommonChorus.Gm2Chorus.Rate)
            {
                slChorusGM2ChorusRate.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Rate;
                SetStudioSetChorusGM2ChorusRate((Int32)slChorusGM2ChorusRate.Value);
            }
            if (slChorusGM2ChorusDepth.Value != commonState.StudioSet.CommonChorus.Gm2Chorus.Depth)
            {
                slChorusGM2ChorusDepth.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Depth;
                SetStudioSetChorusGM2ChorusDepth((Int32)slChorusGM2ChorusDepth.Value);
            }
            if (slChorusGM2ChorusLevel.Value != commonState.StudioSet.CommonChorus.Gm2Chorus.Level)
            {
                slChorusGM2ChorusLevel.Value = commonState.StudioSet.CommonChorus.Gm2Chorus.Level;
                SetStudioSetChorusGM2ChorusSendLevelToReverb((Int32)slChorusGM2ChorusLevel.Value);
            }
            //if (cbStudioSetReverbType.SelectedIndex != commonState.studioSet.CommonReverb.Type)
            {
                cbStudioSetReverbType.SelectedIndex = commonState.StudioSet.CommonReverb.Type;
                SetStudioSetStudioSetReverbType((Int32)cbStudioSetReverbType.SelectedIndex);
            }
            if (slStudioSetReverbLevel.Value != commonState.StudioSet.CommonReverb.Level)
            {
                slStudioSetReverbLevel.Value = commonState.StudioSet.CommonReverb.Level;
                SetStudioSetReverbLevel((Int32)slStudioSetReverbLevel.Value);
            }
            if (cbStudioSetReverbOutputAssign.SelectedIndex != commonState.StudioSet.CommonReverb.OutputAssign)
            {
                cbStudioSetReverbOutputAssign.SelectedIndex = commonState.StudioSet.CommonReverb.OutputAssign;
                SetStudioSetStudioSetReverbOutputAssign(cbStudioSetReverbOutputAssign.SelectedIndex);
            }
            switch (commonState.StudioSet.CommonReverb.Type)
            {
                case 1:
                    if (slStudioSetReverbPreDelay.Value != commonState.StudioSet.CommonReverb.ReverbRoom1.PreDelay)
                    {
                        slStudioSetReverbPreDelay.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.PreDelay;
                        SetStudioSetStudioSetReverbPreDelay((Int32)slStudioSetReverbPreDelay.Value);
                    }
                    if (slStudioSetReverbTime.Value != commonState.StudioSet.CommonReverb.ReverbRoom1.Time)
                    {
                        slStudioSetReverbTime.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.Time;
                        SetStudioSetStudioSetReverbTime((Int32)slStudioSetReverbTime.Value);
                    }
                    if (slStudioSetReverbDensity.Value != commonState.StudioSet.CommonReverb.ReverbRoom1.Density)
                    {
                        slStudioSetReverbDensity.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.Density;
                        SetStudioSetStudioSetReverbDensity((Int32)slStudioSetReverbDensity.Value);
                    }
                    if (slStudioSetReverbDiffusion.Value != commonState.StudioSet.CommonReverb.ReverbRoom1.Diffusion)
                    {
                        slStudioSetReverbDiffusion.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.Diffusion;
                        SetStudioSetStudioSetReverbDiffusion((Int32)slStudioSetReverbDiffusion.Value);
                    }
                    if (slStudioSetReverbLFDamp.Value != commonState.StudioSet.CommonReverb.ReverbRoom1.LFDamp)
                    {
                        slStudioSetReverbLFDamp.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.LFDamp;
                        SetStudioSetStudioSetReverbLFDamp((Int32)slStudioSetReverbLFDamp.Value);
                    }
                    if (slStudioSetReverbHFDamp.Value != commonState.StudioSet.CommonReverb.ReverbRoom1.HFDamp)
                    {
                        slStudioSetReverbHFDamp.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.HFDamp;
                        SetStudioSetStudioSetReverbHFDamp((Int32)slStudioSetReverbHFDamp.Value);
                    }
                    if (slStudioSetReverbSpread.Value != commonState.StudioSet.CommonReverb.ReverbRoom1.Spread)
                    {
                        slStudioSetReverbSpread.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.Spread;
                        SetStudioSetStudioSetReverbSpread((Int32)slStudioSetReverbSpread.Value);
                    }
                    if (slStudioSetReverbTone.Value != commonState.StudioSet.CommonReverb.ReverbRoom1.Tone)
                    {
                        slStudioSetReverbTone.Value = commonState.StudioSet.CommonReverb.ReverbRoom1.Tone;
                        SetStudioSetStudioSetReverbTone((Int32)slStudioSetReverbTone.Value);
                    }
                    break;
                case 2:
                    if (slStudioSetReverbPreDelay.Value != commonState.StudioSet.CommonReverb.ReverbRoom2.PreDelay)
                    {
                        slStudioSetReverbPreDelay.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.PreDelay;
                        SetStudioSetStudioSetReverbPreDelay((Int32)slStudioSetReverbPreDelay.Value);
                    }
                    if (slStudioSetReverbTime.Value != commonState.StudioSet.CommonReverb.ReverbRoom2.Time)
                    {
                        slStudioSetReverbTime.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.Time;
                        SetStudioSetStudioSetReverbTime((Int32)slStudioSetReverbTime.Value);
                    }
                    if (slStudioSetReverbDensity.Value != commonState.StudioSet.CommonReverb.ReverbRoom2.Density)
                    {
                        slStudioSetReverbDensity.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.Density;
                        SetStudioSetStudioSetReverbDensity((Int32)slStudioSetReverbDensity.Value);
                    }
                    if (slStudioSetReverbDiffusion.Value != commonState.StudioSet.CommonReverb.ReverbRoom2.Diffusion)
                    {
                        slStudioSetReverbDiffusion.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.Diffusion;
                        SetStudioSetStudioSetReverbDiffusion((Int32)slStudioSetReverbDiffusion.Value);
                    }
                    if (slStudioSetReverbLFDamp.Value != commonState.StudioSet.CommonReverb.ReverbRoom2.LFDamp)
                    {
                        slStudioSetReverbLFDamp.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.LFDamp;
                        SetStudioSetStudioSetReverbLFDamp((Int32)slStudioSetReverbLFDamp.Value);
                    }
                    if (slStudioSetReverbHFDamp.Value != commonState.StudioSet.CommonReverb.ReverbRoom2.HFDamp)
                    {
                        slStudioSetReverbHFDamp.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.HFDamp;
                        SetStudioSetStudioSetReverbHFDamp((Int32)slStudioSetReverbHFDamp.Value);
                    }
                    if (slStudioSetReverbSpread.Value != commonState.StudioSet.CommonReverb.ReverbRoom2.Spread)
                    {
                        slStudioSetReverbSpread.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.Spread;
                        SetStudioSetStudioSetReverbSpread((Int32)slStudioSetReverbSpread.Value);
                    }
                    if (slStudioSetReverbTone.Value != commonState.StudioSet.CommonReverb.ReverbRoom2.Tone)
                    {
                        slStudioSetReverbTone.Value = commonState.StudioSet.CommonReverb.ReverbRoom2.Tone;
                        SetStudioSetStudioSetReverbTone((Int32)slStudioSetReverbTone.Value);
                    }
                    break;
                case 3:
                    if (slStudioSetReverbPreDelay.Value != commonState.StudioSet.CommonReverb.ReverbHall1.PreDelay)
                    {
                        slStudioSetReverbPreDelay.Value = commonState.StudioSet.CommonReverb.ReverbHall1.PreDelay;
                        SetStudioSetStudioSetReverbPreDelay((Int32)slStudioSetReverbPreDelay.Value);
                    }
                    if (slStudioSetReverbTime.Value != commonState.StudioSet.CommonReverb.ReverbHall1.Time)
                    {
                        slStudioSetReverbTime.Value = commonState.StudioSet.CommonReverb.ReverbHall1.Time;
                        SetStudioSetStudioSetReverbTime((Int32)slStudioSetReverbTime.Value);
                    }
                    if (slStudioSetReverbDensity.Value != commonState.StudioSet.CommonReverb.ReverbHall1.Density)
                    {
                        slStudioSetReverbDensity.Value = commonState.StudioSet.CommonReverb.ReverbHall1.Density;
                        SetStudioSetStudioSetReverbDensity((Int32)slStudioSetReverbDensity.Value);
                    }
                    if (slStudioSetReverbDiffusion.Value != commonState.StudioSet.CommonReverb.ReverbHall1.Diffusion)
                    {
                        slStudioSetReverbDiffusion.Value = commonState.StudioSet.CommonReverb.ReverbHall1.Diffusion;
                        SetStudioSetStudioSetReverbDiffusion((Int32)slStudioSetReverbDiffusion.Value);
                    }
                    if (slStudioSetReverbLFDamp.Value != commonState.StudioSet.CommonReverb.ReverbHall1.LFDamp)
                    {
                        slStudioSetReverbLFDamp.Value = commonState.StudioSet.CommonReverb.ReverbHall1.LFDamp;
                        SetStudioSetStudioSetReverbLFDamp((Int32)slStudioSetReverbLFDamp.Value);
                    }
                    if (slStudioSetReverbHFDamp.Value != commonState.StudioSet.CommonReverb.ReverbHall1.HFDamp)
                    {
                        slStudioSetReverbHFDamp.Value = commonState.StudioSet.CommonReverb.ReverbHall1.HFDamp;
                        SetStudioSetStudioSetReverbHFDamp((Int32)slStudioSetReverbHFDamp.Value);
                    }
                    if (slStudioSetReverbSpread.Value != commonState.StudioSet.CommonReverb.ReverbHall1.Spread)
                    {
                        slStudioSetReverbSpread.Value = commonState.StudioSet.CommonReverb.ReverbHall1.Spread;
                        SetStudioSetStudioSetReverbSpread((Int32)slStudioSetReverbSpread.Value);
                    }
                    if (slStudioSetReverbTone.Value != commonState.StudioSet.CommonReverb.ReverbHall1.Tone)
                    {
                        slStudioSetReverbTone.Value = commonState.StudioSet.CommonReverb.ReverbHall1.Tone;
                        SetStudioSetStudioSetReverbTone((Int32)slStudioSetReverbTone.Value);
                    }
                    break;
                case 4:
                    if (slStudioSetReverbPreDelay.Value != commonState.StudioSet.CommonReverb.ReverbHall2.PreDelay)
                    {
                        slStudioSetReverbPreDelay.Value = commonState.StudioSet.CommonReverb.ReverbHall2.PreDelay;
                        SetStudioSetStudioSetReverbPreDelay((Int32)slStudioSetReverbPreDelay.Value);
                    }
                    if (slStudioSetReverbTime.Value != commonState.StudioSet.CommonReverb.ReverbHall2.Time)
                    {
                        slStudioSetReverbTime.Value = commonState.StudioSet.CommonReverb.ReverbHall2.Time;
                        SetStudioSetStudioSetReverbTime((Int32)slStudioSetReverbTime.Value);
                    }
                    if (slStudioSetReverbDensity.Value != commonState.StudioSet.CommonReverb.ReverbHall2.Density)
                    {
                        slStudioSetReverbDensity.Value = commonState.StudioSet.CommonReverb.ReverbHall2.Density;
                        SetStudioSetStudioSetReverbDensity((Int32)slStudioSetReverbDensity.Value);
                    }
                    if (slStudioSetReverbDiffusion.Value != commonState.StudioSet.CommonReverb.ReverbHall2.Diffusion)
                    {
                        slStudioSetReverbDiffusion.Value = commonState.StudioSet.CommonReverb.ReverbHall2.Diffusion;
                        SetStudioSetStudioSetReverbDiffusion((Int32)slStudioSetReverbDiffusion.Value);
                    }
                    if (slStudioSetReverbLFDamp.Value != commonState.StudioSet.CommonReverb.ReverbHall2.LFDamp)
                    {
                        slStudioSetReverbLFDamp.Value = commonState.StudioSet.CommonReverb.ReverbHall2.LFDamp;
                        SetStudioSetStudioSetReverbLFDamp((Int32)slStudioSetReverbLFDamp.Value);
                    }
                    if (slStudioSetReverbHFDamp.Value != commonState.StudioSet.CommonReverb.ReverbHall2.HFDamp)
                    {
                        slStudioSetReverbHFDamp.Value = commonState.StudioSet.CommonReverb.ReverbHall2.HFDamp;
                        SetStudioSetStudioSetReverbHFDamp((Int32)slStudioSetReverbHFDamp.Value);
                    }
                    if (slStudioSetReverbSpread.Value != commonState.StudioSet.CommonReverb.ReverbHall2.Spread)
                    {
                        slStudioSetReverbSpread.Value = commonState.StudioSet.CommonReverb.ReverbHall2.Spread;
                        SetStudioSetStudioSetReverbSpread((Int32)slStudioSetReverbSpread.Value);
                    }
                    if (slStudioSetReverbTone.Value != commonState.StudioSet.CommonReverb.ReverbHall2.Tone)
                    {
                        slStudioSetReverbTone.Value = commonState.StudioSet.CommonReverb.ReverbHall2.Tone;
                        SetStudioSetStudioSetReverbTone((Int32)slStudioSetReverbTone.Value);
                    }
                    break;
                case 5:
                    if (slStudioSetReverbPreDelay.Value != commonState.StudioSet.CommonReverb.ReverbPlate.PreDelay)
                    {
                        slStudioSetReverbPreDelay.Value = commonState.StudioSet.CommonReverb.ReverbPlate.PreDelay;
                        SetStudioSetStudioSetReverbPreDelay((Int32)slStudioSetReverbPreDelay.Value);
                    }
                    if (slStudioSetReverbTime.Value != commonState.StudioSet.CommonReverb.ReverbPlate.Time)
                    {
                        slStudioSetReverbTime.Value = commonState.StudioSet.CommonReverb.ReverbPlate.Time;
                        SetStudioSetStudioSetReverbTime((Int32)slStudioSetReverbTime.Value);
                    }
                    if (slStudioSetReverbDensity.Value != commonState.StudioSet.CommonReverb.ReverbPlate.Density)
                    {
                        slStudioSetReverbDensity.Value = commonState.StudioSet.CommonReverb.ReverbPlate.Density;
                        SetStudioSetStudioSetReverbDensity((Int32)slStudioSetReverbDensity.Value);
                    }
                    if (slStudioSetReverbDiffusion.Value != commonState.StudioSet.CommonReverb.ReverbPlate.Diffusion)
                    {
                        slStudioSetReverbDiffusion.Value = commonState.StudioSet.CommonReverb.ReverbPlate.Diffusion;
                        SetStudioSetStudioSetReverbDiffusion((Int32)slStudioSetReverbDiffusion.Value);
                    }
                    if (slStudioSetReverbLFDamp.Value != commonState.StudioSet.CommonReverb.ReverbPlate.LFDamp)
                    {
                        slStudioSetReverbLFDamp.Value = commonState.StudioSet.CommonReverb.ReverbPlate.LFDamp;
                        SetStudioSetStudioSetReverbLFDamp((Int32)slStudioSetReverbLFDamp.Value);
                    }
                    if (slStudioSetReverbHFDamp.Value != commonState.StudioSet.CommonReverb.ReverbPlate.HFDamp)
                    {
                        slStudioSetReverbHFDamp.Value = commonState.StudioSet.CommonReverb.ReverbPlate.HFDamp;
                        SetStudioSetStudioSetReverbHFDamp((Int32)slStudioSetReverbHFDamp.Value);
                    }
                    if (slStudioSetReverbSpread.Value != commonState.StudioSet.CommonReverb.ReverbPlate.Spread)
                    {
                        slStudioSetReverbSpread.Value = commonState.StudioSet.CommonReverb.ReverbPlate.Spread;
                        SetStudioSetStudioSetReverbSpread((Int32)slStudioSetReverbSpread.Value);
                    }
                    if (slStudioSetReverbTone.Value != commonState.StudioSet.CommonReverb.ReverbPlate.Tone)
                    {
                        slStudioSetReverbTone.Value = commonState.StudioSet.CommonReverb.ReverbPlate.Tone;
                        SetStudioSetStudioSetReverbTone((Int32)slStudioSetReverbTone.Value);
                    }
                    break;
            }
            if (slStudioSetReverbGM2Character.Value != commonState.StudioSet.CommonReverb.GM2Reverb.Character)
            {
                slStudioSetReverbGM2Character.Value = commonState.StudioSet.CommonReverb.GM2Reverb.Character;
                SetStudioSetStudioSetReverbGM2Character((Int32)slStudioSetReverbGM2Character.Value);
            }
            if (slStudioSetReverbGM2Time.Value != commonState.StudioSet.CommonReverb.GM2Reverb.Time)
            {
                slStudioSetReverbGM2Time.Value = commonState.StudioSet.CommonReverb.GM2Reverb.Time;
                SetStudioSetStudioSetReverbGM2Time((Int32)slStudioSetReverbGM2Time.Value);
            }
            if (cbStudioSetMotionalSurround.IsChecked != commonState.StudioSet.MotionalSurround.MotionalSurroundSwitch)
            {
                cbStudioSetMotionalSurround.IsChecked = commonState.StudioSet.MotionalSurround.MotionalSurroundSwitch;
                SetStudioSetStudioSetMotionalSurround((Boolean)cbStudioSetMotionalSurround.IsChecked);
            }
            if (cbStudioSetMotionalSurroundRoomType.SelectedIndex != commonState.StudioSet.MotionalSurround.RoomType)
            {
                cbStudioSetMotionalSurroundRoomType.SelectedIndex = commonState.StudioSet.MotionalSurround.RoomType;
                SetStudioSetStudioSetMotionalSurroundRoomType((Int32)cbStudioSetMotionalSurroundRoomType.SelectedIndex);
            }
            if (slStudioSetMotionalSurroundAmbienceLevel.Value != commonState.StudioSet.MotionalSurround.AmbienceLevel)
            {
                slStudioSetMotionalSurroundAmbienceLevel.Value = commonState.StudioSet.MotionalSurround.AmbienceLevel;
                SetStudioSetStudioSetMotionalSurroundAmbienceLevel((Int32)slStudioSetMotionalSurroundAmbienceLevel.Value);
            }
            if (cbStudioSetMotionalSurroundRoomSize.SelectedIndex != commonState.StudioSet.MotionalSurround.RoomSize)
            {
                cbStudioSetMotionalSurroundRoomSize.SelectedIndex = commonState.StudioSet.MotionalSurround.RoomSize;
                SetStudioSetStudioSetMotionalSurroundRoomSize(cbStudioSetMotionalSurroundRoomSize.SelectedIndex);
            }
            if (slStudioSetMotionalSurroundAmbienceTime.Value != commonState.StudioSet.MotionalSurround.AmbienceTime)
            {
                slStudioSetMotionalSurroundAmbienceTime.Value = commonState.StudioSet.MotionalSurround.AmbienceTime;
                SetStudioSetStudioSetMotionalSurroundAmbienceTime((Int32)slStudioSetMotionalSurroundAmbienceTime.Value);
            }
            if (slStudioSetMotionalSurroundAmbienceDensity.Value != commonState.StudioSet.MotionalSurround.AmbienceDensity)
            {
                slStudioSetMotionalSurroundAmbienceDensity.Value = commonState.StudioSet.MotionalSurround.AmbienceDensity;
                SetStudioSetStudioSetMotionalSurroundAmbienceDensity((Int32)slStudioSetMotionalSurroundAmbienceDensity.Value);
            }
            if (slStudioSetMotionalSurroundAmbienceHFDamp.Value != commonState.StudioSet.MotionalSurround.AmbienceHFDamp)
            {
                slStudioSetMotionalSurroundAmbienceHFDamp.Value = commonState.StudioSet.MotionalSurround.AmbienceHFDamp;
                SetStudioSetStudioSetMotionalSurroundAmbienceHFDamp((Int32)slStudioSetMotionalSurroundAmbienceHFDamp.Value);
            }
            if (slStudioSetMotionalSurroundExternalPartLR.Value != commonState.StudioSet.MotionalSurround.ExtPartLR)
            {
                slStudioSetMotionalSurroundExternalPartLR.Value = commonState.StudioSet.MotionalSurround.ExtPartLR;
                SetStudioSetStudioSetMotionalSurroundExternalPartLR((Int32)slStudioSetMotionalSurroundExternalPartLR.Value);
            }
            if (slStudioSetMotionalSurroundExternalPartFB.Value != commonState.StudioSet.MotionalSurround.ExtPartFB)
            {
                slStudioSetMotionalSurroundExternalPartFB.Value = commonState.StudioSet.MotionalSurround.ExtPartFB;
                SetStudioSetStudioSetMotionalSurroundExternalPartFB((Int32)slStudioSetMotionalSurroundExternalPartFB.Value);
            }
            if (slStudioSetMotionalSurroundExtPartWidth.Value != commonState.StudioSet.MotionalSurround.ExtPartWidth)
            {
                slStudioSetMotionalSurroundExtPartWidth.Value = commonState.StudioSet.MotionalSurround.ExtPartWidth;
                SetStudioSetStudioSetMotionalSurroundExtPartWidth((Int32)slStudioSetMotionalSurroundExtPartWidth.Value);
            }
            if (slStudioSetMotionalSurroundExtpartAmbienceSend.Value != commonState.StudioSet.MotionalSurround.ExtPartAmbienceSendLevel)
            {
                slStudioSetMotionalSurroundExtpartAmbienceSend.Value = commonState.StudioSet.MotionalSurround.ExtPartAmbienceSendLevel;
                SetStudioSetStudioSetMotionalSurroundExtpartAmbienceSend((Int32)slStudioSetMotionalSurroundExtpartAmbienceSend.Value);
            }
            if (cbStudioSetMotionalSurroundExtPartControl.SelectedIndex != commonState.StudioSet.MotionalSurround.ExtPartControlChannel)
            {
                cbStudioSetMotionalSurroundExtPartControl.SelectedIndex = commonState.StudioSet.MotionalSurround.ExtPartControlChannel;
                SetStudioSetStudioSetMotionalSurroundExtPartControl(cbStudioSetMotionalSurroundExtPartControl.SelectedIndex);
            }
            if (slStudioSetMotionalSurroundDepth.Value != commonState.StudioSet.MotionalSurround.MotionalSurroundDepth)
            {
                slStudioSetMotionalSurroundDepth.Value = commonState.StudioSet.MotionalSurround.MotionalSurroundDepth;
                SetStudioSetStudioSetMotionalSurroundDepth((Int32)slStudioSetMotionalSurroundDepth.Value);
            }
            if (commonState.StudioSet.MasterEQ.EQLowFreq != cbStudioSetMasterEqLowFreq.SelectedIndex)
            {
                SetStudioSetStudioSetMasterEqLowFreq(cbStudioSetMasterEqLowFreq.SelectedIndex);
            }
            if (slStudioSetMasterEqLowGain.Value != commonState.StudioSet.MasterEQ.EQLowGain)
            {
                slStudioSetMasterEqLowGain.Value = commonState.StudioSet.MasterEQ.EQLowGain;
                SetStudioSetStudioSetMasterEqLowGain((Int32)slStudioSetMasterEqLowGain.Value);
            }
            if (cbStudioSetMasterEqMidFreq.SelectedIndex != commonState.StudioSet.MasterEQ.EQMidFreq)
            {
                cbStudioSetMasterEqMidFreq.SelectedIndex = commonState.StudioSet.MasterEQ.EQMidFreq;
                SetStudioSetStudioSetMasterEqMidFreq(cbStudioSetMasterEqMidFreq.SelectedIndex);
            }
            if (slStudioSetMasterEqMidGain.Value != commonState.StudioSet.MasterEQ.EQMidGain)
            {
                slStudioSetMasterEqMidGain.Value = commonState.StudioSet.MasterEQ.EQMidGain;
                SetStudioSetStudioSetMasterEqMidGain((Int32)slStudioSetMasterEqMidGain.Value);
            }
            if (cbStudioSetMasterEqMidQ.SelectedIndex != commonState.StudioSet.MasterEQ.EQMidQ)
            {
                cbStudioSetMasterEqMidQ.SelectedIndex = commonState.StudioSet.MasterEQ.EQMidQ;
                SetStudioSetStudioSetMasterEqMidQ(cbStudioSetMasterEqMidQ.SelectedIndex);
            }
            if (cbStudioSetMasterEqHighFreq.SelectedIndex != commonState.StudioSet.MasterEQ.EQHighFreq)
            {
                cbStudioSetMasterEqHighFreq.SelectedIndex = commonState.StudioSet.MasterEQ.EQHighFreq;
                SetStudioSetStudioSetMasterEqHighFreq(cbStudioSetMasterEqHighFreq.SelectedIndex);
            }
            if (slStudioSetMasterEqHighGain.Value != commonState.StudioSet.MasterEQ.EQHighGain)
            {
                slStudioSetMasterEqHighGain.Value = commonState.StudioSet.MasterEQ.EQHighGain;
                SetStudioSetStudioSetMasterEqHighGain(commonState.StudioSet.MasterEQ.EQHighGain);
            }
            if (cbStudioSetPartSettings1ReceiveChannel.SelectedIndex != commonState.StudioSet.PartMainSettings[part].ReceiveChannel)
            {
                cbStudioSetPartSettings1ReceiveChannel.SelectedIndex = commonState.StudioSet.PartMainSettings[part].ReceiveChannel;
                SetStudioSetStudioSetPartSettings1ReceiveChannel(commonState.StudioSet.PartMainSettings[part].ReceiveChannel, (byte)part);
            }
            if (cbStudioSetPartSettings1Receive.IsChecked != commonState.StudioSet.PartMainSettings[part].ReceiveSwitch)
            {
                cbStudioSetPartSettings1Receive.IsChecked = commonState.StudioSet.PartMainSettings[part].ReceiveSwitch;
                SetStudioSetStudioSetPartSettings1Receive(commonState.StudioSet.PartMainSettings[part].ReceiveSwitch, (byte)part);
            }
            //if (cbStudioSetPartSettings1Group.SelectedIndex != commonState.studioSet.PartMainSettings[part].Group)
            //{
            //    cbStudioSetPartSettings1Group.SelectedIndex = commonState.studioSet.PartMainSettings[part].Group;
            //    SetStudioSetStudioSetPartSettings1Group(commonState.studioSet.PartMainSettings[part].Group, (byte)part);
            //}
            //if (cbStudioSetPartSettings1Category.SelectedIndex != commonState.studioSet.PartMainSettings[part].Category)
            //{
            //    cbStudioSetPartSettings1Category.SelectedIndex = commonState.studioSet.PartMainSettings[part].Category;
            //    SetStudioSetStudioSetPartSettings1Category(commonState.studioSet.PartMainSettings[part].Category, (byte)part);
            //}
            //if (cbStudioSetPartSettings1Program.SelectedIndex != commonState.studioSet.PartMainSettings[part].Program)
            //{
            //    cbStudioSetPartSettings1Program.SelectedIndex = commonState.studioSet.PartMainSettings[part].Program;
            //    SetStudioSetStudioSetPartSettings1Program(commonState.studioSet.PartMainSettings[part].Program, (byte)part);
            //}
            if (slStudioSetPartSettings1Level.Value != commonState.StudioSet.PartMainSettings[part].Level)
            {
                slStudioSetPartSettings1Level.Value = commonState.StudioSet.PartMainSettings[part].Level;
                SetStudioSetStudioSetPartSettings1Level(commonState.StudioSet.PartMainSettings[part].Level, (byte)part);
            }
            if (slStudioSetPartSettings1Pan.Value != commonState.StudioSet.PartMainSettings[part].Pan)
            {
                slStudioSetPartSettings1Pan.Value = commonState.StudioSet.PartMainSettings[part].Pan;
                SetStudioSetStudioSetPartSettings1Pan(commonState.StudioSet.PartMainSettings[part].Pan, (byte)part);
            }
            if (slStudioSetPartSettings1CoarseTune.Value != commonState.StudioSet.PartMainSettings[part].CoarseTune)
            {
                slStudioSetPartSettings1CoarseTune.Value = commonState.StudioSet.PartMainSettings[part].CoarseTune;
                SetStudioSetStudioSetPartSettings1CoarseTune(commonState.StudioSet.PartMainSettings[part].CoarseTune, (byte)part);
            }
            if (slStudioSetPartSettings1FineTune.Value != commonState.StudioSet.PartMainSettings[part].FineTune)
            {
                slStudioSetPartSettings1FineTune.Value = commonState.StudioSet.PartMainSettings[part].FineTune;
                SetStudioSetStudioSetPartSettings1FineTune(commonState.StudioSet.PartMainSettings[part].FineTune, (byte)part);
            }
            if (cbStudioSetPartSettings1MonoPoly.SelectedIndex != commonState.StudioSet.PartMainSettings[part].MonoPoly)
            {
                cbStudioSetPartSettings1MonoPoly.SelectedIndex = commonState.StudioSet.PartMainSettings[part].MonoPoly;
                SetStudioSetStudioSetPartSettings1Poly(commonState.StudioSet.PartMainSettings[part].MonoPoly, (byte)part);
            }
            if (cbStudioSetPartSettings1Legato.SelectedIndex != commonState.StudioSet.PartMainSettings[part].Legato)
            {
                cbStudioSetPartSettings1Legato.SelectedIndex = commonState.StudioSet.PartMainSettings[part].Legato;
                SetStudioSetStudioSetPartSettings1Legato(commonState.StudioSet.PartMainSettings[part].Legato, (byte)part);
            }
            if (slStudioSetPartSettings1PitchBendRange.Value != commonState.StudioSet.PartMainSettings[part].PitchBendRange)
            {
                slStudioSetPartSettings1PitchBendRange.Value = commonState.StudioSet.PartMainSettings[part].PitchBendRange;
                SetStudioSetStudioSetPartSettings1BendRange(commonState.StudioSet.PartMainSettings[part].PitchBendRange, (byte)part);
            }
            if (cbStudioSetPartSettings1Portamento.SelectedIndex != commonState.StudioSet.PartMainSettings[part].PortamentoSwitch)
            {
                cbStudioSetPartSettings1Portamento.SelectedIndex = commonState.StudioSet.PartMainSettings[part].PortamentoSwitch;
                SetStudioSetStudioSetPartSettings1Portamento(commonState.StudioSet.PartMainSettings[part].PortamentoSwitch, (byte)part);
            }
            if (slStudioSetPartSettings1PortamentoTime.Value != commonState.StudioSet.PartMainSettings[part].PortamentoTime)
            {
                slStudioSetPartSettings1PortamentoTime.Value = commonState.StudioSet.PartMainSettings[part].PortamentoTime;
                SetStudioSetStudioSetPartSettings1PortamentoTime(commonState.StudioSet.PartMainSettings[part].PortamentoTime, (byte)part);
            }
            if (slStudioSetPartSettings2CutoffOffset.Value != commonState.StudioSet.PartMainSettings[part].CutoffOffset)
            {
                slStudioSetPartSettings2CutoffOffset.Value = commonState.StudioSet.PartMainSettings[part].CutoffOffset;
                SetStudioSetStudioSetPartSettings2CutoffOffset(commonState.StudioSet.PartMainSettings[part].CutoffOffset, (byte)part);
            }
            if (slStudioSetPartSettings2ResonanceOffset.Value != commonState.StudioSet.PartMainSettings[part].ResonanceOffset)
            {
                slStudioSetPartSettings2ResonanceOffset.Value = commonState.StudioSet.PartMainSettings[part].ResonanceOffset;
                SetStudioSetStudioSetPartSettings2ResonanceOffset(commonState.StudioSet.PartMainSettings[part].ResonanceOffset, (byte)part);
            }
            if (slStudioSetPartSettings2AttackTimeOffset.Value != commonState.StudioSet.PartMainSettings[part].AttackTimeOffset)
            {
                slStudioSetPartSettings2AttackTimeOffset.Value = commonState.StudioSet.PartMainSettings[part].AttackTimeOffset;
                SetStudioSetStudioSetPartSettings2AttackTimeOffset(commonState.StudioSet.PartMainSettings[part].AttackTimeOffset, (byte)part);
            }
            if (slStudioSetPartSettings2DecayTimeOffset.Value != commonState.StudioSet.PartMainSettings[part].DecayTimeOffset)
            {
                slStudioSetPartSettings2DecayTimeOffset.Value = commonState.StudioSet.PartMainSettings[part].DecayTimeOffset;
                SetStudioSetStudioSetPartSettings2DecayTimeOffset(commonState.StudioSet.PartMainSettings[part].DecayTimeOffset, (byte)part);
            }
            if (slStudioSetPartSettings2ReleaseTimeOffset.Value != commonState.StudioSet.PartMainSettings[part].ReleaseTimeOffset)
            {
                slStudioSetPartSettings2ReleaseTimeOffset.Value = commonState.StudioSet.PartMainSettings[part].ReleaseTimeOffset;
                SetStudioSetStudioSetPartSettings2ReleaseTimeOffset(commonState.StudioSet.PartMainSettings[part].ReleaseTimeOffset, (byte)part);
            }
            if (slStudioSetPartSettings2VibratoRate.Value != commonState.StudioSet.PartMainSettings[part].VibratoRate)
            {
                slStudioSetPartSettings2VibratoRate.Value = commonState.StudioSet.PartMainSettings[part].VibratoRate;
                SetStudioSetStudioSetPartSettings2VibratoRate(commonState.StudioSet.PartMainSettings[part].VibratoRate, (byte)part);
            }
            if (slStudioSetPartSettings2VibratoDepth.Value != commonState.StudioSet.PartMainSettings[part].VibratoDepth)
            {
                slStudioSetPartSettings2VibratoDepth.Value = commonState.StudioSet.PartMainSettings[part].VibratoDepth;
                SetStudioSetStudioSetPartSettings2VibratoDepth(commonState.StudioSet.PartMainSettings[part].VibratoDepth, (byte)part);
            }
            if (slStudioSetPartSettings2VibratoDelay.Value != commonState.StudioSet.PartMainSettings[part].VibratoDelay)
            {
                slStudioSetPartSettings2VibratoDelay.Value = commonState.StudioSet.PartMainSettings[part].VibratoDelay;
                SetStudioSetStudioSetPartSettings2VibratoDelay(commonState.StudioSet.PartMainSettings[part].VibratoDelay, (byte)part);
            }
            if (slStudioSetPartEffectsChorusSendLevel.Value != commonState.StudioSet.PartMainSettings[part].ChorusSendLevel)
            {
                slStudioSetPartEffectsChorusSendLevel.Value = commonState.StudioSet.PartMainSettings[part].ChorusSendLevel;
                SetStudioSetStudioSetPartEffectsChorusSendLevel(commonState.StudioSet.PartMainSettings[part].ChorusSendLevel, (byte)part);
            }
            if (slStudioSetPartEffectsReverbSendLevel.Value != commonState.StudioSet.PartMainSettings[part].ReverbSendLevel)
            {
                slStudioSetPartEffectsReverbSendLevel.Value = commonState.StudioSet.PartMainSettings[part].ReverbSendLevel;
                SetStudioSetStudioSetPartEffectsReverbSendLevel(commonState.StudioSet.PartMainSettings[part].ReverbSendLevel, (byte)part);
            }
            if (cbStudioSetPartEffectsOutputAssign.SelectedIndex != commonState.StudioSet.PartMainSettings[part].OutputAssign)
            {
                cbStudioSetPartEffectsOutputAssign.SelectedIndex = commonState.StudioSet.PartMainSettings[part].OutputAssign;
                SetStudioSetStudioSetPartEffectsOutputAssign(commonState.StudioSet.PartMainSettings[part].OutputAssign, (byte)part);
            }
            if (slStudioSetPartKeyboardOctaveShift.Value != commonState.StudioSet.PartKeyboard[part].OctaveShift)
            {
                slStudioSetPartKeyboardOctaveShift.Value = commonState.StudioSet.PartKeyboard[part].OctaveShift;
                SetStudioSetStudioSetPartKeyboardOctaveShift(commonState.StudioSet.PartKeyboard[part].OctaveShift, (byte)part);
            }
            if (slStudioSetPartKeyboardVelocitySenseOffset.Value != commonState.StudioSet.PartKeyboard[part].VelocitySenseOffset)
            {
                slStudioSetPartKeyboardVelocitySenseOffset.Value = commonState.StudioSet.PartKeyboard[part].VelocitySenseOffset;
                SetStudioSetStudioSetPartKeyboardVelocitySenseOffset(commonState.StudioSet.PartKeyboard[part].VelocitySenseOffset, (byte)part);
            }
            if (slStudioSetPartKeyboardRangeLower.Value != commonState.StudioSet.PartKeyboard[part].RangeLower)
            {
                slStudioSetPartKeyboardRangeLower.Value = commonState.StudioSet.PartKeyboard[part].RangeLower;
                SetStudioSetStudioSetPartKeyboardRangeLower(commonState.StudioSet.PartKeyboard[part].RangeLower, (byte)part);
            }
            if (slStudioSetPartKeyboardRangeUpper.Value != commonState.StudioSet.PartKeyboard[part].RangeUpper)
            {
                slStudioSetPartKeyboardRangeUpper.Value = commonState.StudioSet.PartKeyboard[part].RangeUpper;
                SetStudioSetStudioSetPartKeyboardRangeUpper(commonState.StudioSet.PartKeyboard[part].RangeUpper, (byte)part);
            }
            if (slStudioSetPartKeyboardFadeWidthLower.Value != commonState.StudioSet.PartKeyboard[part].FadeWidthLower)
            {
                slStudioSetPartKeyboardFadeWidthLower.Value = commonState.StudioSet.PartKeyboard[part].FadeWidthLower;
                SetStudioSetStudioSetPartKeyboardFadeWidthLower(commonState.StudioSet.PartKeyboard[part].FadeWidthLower, (byte)part);
            }
            if (slStudioSetPartKeyboardFadeWidthUpper.Value != commonState.StudioSet.PartKeyboard[part].FadeWidthUpper)
            {
                slStudioSetPartKeyboardFadeWidthUpper.Value = commonState.StudioSet.PartKeyboard[part].FadeWidthUpper;
                SetStudioSetStudioSetPartKeyboardFadeWidthUpper(commonState.StudioSet.PartKeyboard[part].FadeWidthUpper, (byte)part);
            }
            if (slStudioSetPartKeyboardVelocityRangeLower.Value != commonState.StudioSet.PartKeyboard[part].VelocityRangeLower)
            {
                slStudioSetPartKeyboardVelocityRangeLower.Value = commonState.StudioSet.PartKeyboard[part].VelocityRangeLower;
                SetStudioSetStudioSetPartKeyboardVelocityRangeLower(commonState.StudioSet.PartKeyboard[part].VelocityRangeLower, (byte)part);
            }
            if (slStudioSetPartKeyboardVelocityRangeUpper.Value != commonState.StudioSet.PartKeyboard[part].VelocityRangeUpper)
            {
                slStudioSetPartKeyboardVelocityRangeUpper.Value = commonState.StudioSet.PartKeyboard[part].VelocityRangeUpper;
                SetStudioSetStudioSetPartKeyboardVelocityRangeUpper(commonState.StudioSet.PartKeyboard[part].VelocityRangeUpper, (byte)part);
            }
            if (slStudioSetPartKeyboardVelocityFadeWidthLower.Value != commonState.StudioSet.PartKeyboard[part].VelocityFadeWidthLower)
            {
                slStudioSetPartKeyboardVelocityFadeWidthLower.Value = commonState.StudioSet.PartKeyboard[part].VelocityFadeWidthLower;
                SetStudioSetStudioSetPartKeyboardVelocityFadeWidthLower(commonState.StudioSet.PartKeyboard[part].VelocityFadeWidthLower, (byte)part);
            }
            if (slStudioSetPartKeyboardVelocityFadeWidthUpper.Value != commonState.StudioSet.PartKeyboard[part].VelocityFadeWidthUpper)
            {
                slStudioSetPartKeyboardVelocityFadeWidthUpper.Value = commonState.StudioSet.PartKeyboard[part].VelocityFadeWidthUpper;
                SetStudioSetStudioSetPartKeyboardVelocityFadeWidthUpper(commonState.StudioSet.PartKeyboard[part].VelocityFadeWidthUpper, (byte)part);
            }
            if (cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex != commonState.StudioSet.PartKeyboard[part].VelocityCurveType)
            {
                cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = commonState.StudioSet.PartKeyboard[part].VelocityCurveType;
                SetStudioSetStudioSetPartKeyboardVelocityCurveType(commonState.StudioSet.PartKeyboard[part].VelocityCurveType, (byte)part);
            }
            if (cbStudioSetPartKeyboardMute.IsChecked != commonState.StudioSet.PartKeyboard[part].Mute)
            {
                cbStudioSetPartKeyboardMute.IsChecked = commonState.StudioSet.PartKeyboard[part].Mute;
                SetStudioSetStudioSetPartKeyboard(commonState.StudioSet.PartKeyboard[part].Mute, (byte)part);
            }
            if (cbStudioSetPartScaleTuneType.SelectedIndex != commonState.StudioSet.PartScaleTune[part].Type)
            {
                cbStudioSetPartScaleTuneType.SelectedIndex = commonState.StudioSet.PartScaleTune[part].Type;
                SetStudioSetStudioSetPartScaleTuneType(commonState.StudioSet.PartScaleTune[part].Type, (byte)part);
            }
            if (cbStudioSetPartScaleTuneKey.SelectedIndex != commonState.StudioSet.PartScaleTune[part].Key)
            {
                cbStudioSetPartScaleTuneKey.SelectedIndex = commonState.StudioSet.PartScaleTune[part].Key;
                SetStudioSetStudioSetPartScaleTune(commonState.StudioSet.PartScaleTune[part].Key, (byte)part);
            }
            if (slStudioSetPartScaleTuneC.Value != commonState.StudioSet.PartScaleTune[part].C)
            {
                slStudioSetPartScaleTuneC.Value = commonState.StudioSet.PartScaleTune[part].C;
                SetStudioSetStudioSetPartScaleTuneC(commonState.StudioSet.PartScaleTune[part].C, (byte)part);
            }
            if (slStudioSetPartScaleTuneCi.Value != commonState.StudioSet.PartScaleTune[part].Ci)
            {
                slStudioSetPartScaleTuneCi.Value = commonState.StudioSet.PartScaleTune[part].Ci;
                SetStudioSetStudioSetPartScaleTuneCi(commonState.StudioSet.PartScaleTune[part].Ci, (byte)part);
            }
            if (slStudioSetPartScaleTuneD.Value != commonState.StudioSet.PartScaleTune[part].D)
            {
                slStudioSetPartScaleTuneD.Value = commonState.StudioSet.PartScaleTune[part].D;
                SetStudioSetStudioSetPartScaleTuneD(commonState.StudioSet.PartScaleTune[part].D, (byte)part);
            }
            if (slStudioSetPartScaleTuneDi.Value != commonState.StudioSet.PartScaleTune[part].Di)
            {
                slStudioSetPartScaleTuneDi.Value = commonState.StudioSet.PartScaleTune[part].Di;
                SetStudioSetStudioSetPartScaleTuneDi(commonState.StudioSet.PartScaleTune[part].Di, (byte)part);
            }
            if (slStudioSetPartScaleTuneE.Value != commonState.StudioSet.PartScaleTune[part].E)
            {
                slStudioSetPartScaleTuneE.Value = commonState.StudioSet.PartScaleTune[part].E;
                SetStudioSetStudioSetPartScaleTuneE(commonState.StudioSet.PartScaleTune[part].E, (byte)part);
            }
            if (slStudioSetPartScaleTuneF.Value != commonState.StudioSet.PartScaleTune[part].F)
            {
                slStudioSetPartScaleTuneF.Value = commonState.StudioSet.PartScaleTune[part].F;
                SetStudioSetStudioSetPartScaleTuneF(commonState.StudioSet.PartScaleTune[part].F, (byte)part);
            }
            if (slStudioSetPartScaleTuneFi.Value != commonState.StudioSet.PartScaleTune[part].Fi)
            {
                slStudioSetPartScaleTuneFi.Value = commonState.StudioSet.PartScaleTune[part].Fi;
                SetStudioSetStudioSetPartScaleTuneFi(commonState.StudioSet.PartScaleTune[part].Fi, (byte)part);
            }
            if (slStudioSetPartScaleTuneG.Value != commonState.StudioSet.PartScaleTune[part].G)
            {
                slStudioSetPartScaleTuneG.Value = commonState.StudioSet.PartScaleTune[part].G;
                SetStudioSetStudioSetPartScaleTuneG(commonState.StudioSet.PartScaleTune[part].G, (byte)part);
            }
            if (slStudioSetPartScaleTuneGi.Value != commonState.StudioSet.PartScaleTune[part].Gi)
            {
                slStudioSetPartScaleTuneGi.Value = commonState.StudioSet.PartScaleTune[part].Gi;
                SetStudioSetStudioSetPartScaleTuneGi(commonState.StudioSet.PartScaleTune[part].Gi, (byte)part);
            }
            if (slStudioSetPartScaleTuneA.Value != commonState.StudioSet.PartScaleTune[part].A)
            {
                slStudioSetPartScaleTuneA.Value = commonState.StudioSet.PartScaleTune[part].A;
                SetStudioSetStudioSetPartScaleTuneA(commonState.StudioSet.PartScaleTune[part].A, (byte)part);
            }
            if (slStudioSetPartScaleTuneAi.Value != commonState.StudioSet.PartScaleTune[part].Ai)
            {
                slStudioSetPartScaleTuneAi.Value = commonState.StudioSet.PartScaleTune[part].Ai;
                SetStudioSetStudioSetPartScaleTuneAi(commonState.StudioSet.PartScaleTune[part].Ai, (byte)part);
            }
            if (slStudioSetPartScaleTuneB.Value != commonState.StudioSet.PartScaleTune[part].B)
            {
                slStudioSetPartScaleTuneB.Value = commonState.StudioSet.PartScaleTune[part].B;
                SetStudioSetStudioSetPartScaleTuneB(commonState.StudioSet.PartScaleTune[part].B, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveProgramChange.IsChecked != commonState.StudioSet.PartMidi[part].ReceiveProgramChange)
            {
                cbStudioSetPartMidiReceiveProgramChange.IsChecked = commonState.StudioSet.PartMidi[part].ReceiveProgramChange;
                SetStudioSetStudioSetPartMidiReceiveProgramChange(commonState.StudioSet.PartMidi[part].ReceiveProgramChange, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveBankSelect.IsChecked != commonState.StudioSet.PartMidi[part].ReceiveBankSelect)
            {
                cbStudioSetPartMidiReceiveBankSelect.IsChecked = commonState.StudioSet.PartMidi[part].ReceiveBankSelect;
                SetStudioSetStudioSetPartMidiReceiveBankSelect(commonState.StudioSet.PartMidi[part].ReceiveBankSelect, (byte)part);
            }
            if (cbStudioSetPartMidiReceivePitchBend.IsChecked != commonState.StudioSet.PartMidi[part].ReceivePitchBend)
            {
                cbStudioSetPartMidiReceivePitchBend.IsChecked = commonState.StudioSet.PartMidi[part].ReceivePitchBend;
                SetStudioSetStudioSetPartMidiReceivePitchBend(commonState.StudioSet.PartMidi[part].ReceivePitchBend, (byte)part);
            }
            if (cbStudioSetPartMidiReceivePolyphonicKeyPressure.IsChecked != commonState.StudioSet.PartMidi[part].ReceivePolyphonicKeyPressure)
            {
                cbStudioSetPartMidiReceivePolyphonicKeyPressure.IsChecked = commonState.StudioSet.PartMidi[part].ReceivePolyphonicKeyPressure;
                SetStudioSetStudioSetPartMidiReceivePolyphonicKeyPressure(commonState.StudioSet.PartMidi[part].ReceivePolyphonicKeyPressure, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveChannelPressure.IsChecked != commonState.StudioSet.PartMidi[part].ReceiveChannelPressure)
            {
                cbStudioSetPartMidiReceiveChannelPressure.IsChecked = commonState.StudioSet.PartMidi[part].ReceiveChannelPressure;
                SetStudioSetPartMidiReceiveChannelPressure(commonState.StudioSet.PartMidi[part].ReceiveChannelPressure, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveModulation.IsChecked != commonState.StudioSet.PartMidi[part].ReceiveModulation)
            {
                cbStudioSetPartMidiReceiveModulation.IsChecked = commonState.StudioSet.PartMidi[part].ReceiveModulation;
                SetStudioSetStudioSetPartMidiReceiveModulation(commonState.StudioSet.PartMidi[part].ReceiveModulation, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveVolume.IsChecked != commonState.StudioSet.PartMidi[part].ReceiveVolume)
            {
                cbStudioSetPartMidiReceiveVolume.IsChecked = commonState.StudioSet.PartMidi[part].ReceiveVolume;
                SetStudioSetStudioSetPartMidiReceiveVolume(commonState.StudioSet.PartMidi[part].ReceiveVolume, (byte)part);
            }
            if (cbStudioSetPartMidiReceivePan.IsChecked != commonState.StudioSet.PartMidi[part].ReceivePan)
            {
                cbStudioSetPartMidiReceivePan.IsChecked = commonState.StudioSet.PartMidi[part].ReceivePan;
                SetStudioSetStudioSetPartMidiReceivePan(commonState.StudioSet.PartMidi[part].ReceivePan, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveExpression.IsChecked != commonState.StudioSet.PartMidi[part].ReceiveExpression)
            {
                cbStudioSetPartMidiReceiveExpression.IsChecked = commonState.StudioSet.PartMidi[part].ReceiveExpression;
                SetStudioSetStudioSetPartMidiReceiveExpression(commonState.StudioSet.PartMidi[part].ReceiveExpression, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveHold1.IsChecked != commonState.StudioSet.PartMidi[part].ReceiveHold1)
            {
                cbStudioSetPartMidiReceiveHold1.IsChecked = commonState.StudioSet.PartMidi[part].ReceiveHold1;
                SetStudioSetStudioSetPartMidiReceiveHold1(commonState.StudioSet.PartMidi[part].ReceiveHold1, (byte)part);
            }
            if (cbStudioSetPartMidiPhaseLock.IsChecked != commonState.StudioSet.PartMidi[part].PhaseLock)
            {
                cbStudioSetPartMidiPhaseLock.IsChecked = commonState.StudioSet.PartMidi[part].PhaseLock;
                SetStudioSetStudioSetPartMidiPhaseLock(commonState.StudioSet.PartMidi[part].PhaseLock, (byte)part);
            }
            if (slStudioSetPartMotionalSurroundLR.Value != commonState.StudioSet.PartMotionalSurround[part].LR)
            {
                slStudioSetPartMotionalSurroundLR.Value = commonState.StudioSet.PartMotionalSurround[part].LR;
                SetStudioSetStudioSetPartMotionalSurroundLR(commonState.StudioSet.PartMotionalSurround[part].LR, (byte)part);
            }
            if (slStudioSetPartMotionalSurroundFB.Value != commonState.StudioSet.PartMotionalSurround[part].FB)
            {
                slStudioSetPartMotionalSurroundFB.Value = commonState.StudioSet.PartMotionalSurround[part].FB;
                SetStudioSetStudioSetPartMotionalSurroundFB(commonState.StudioSet.PartMotionalSurround[part].FB, (byte)part);
            }
            if (slStudioSetPartMotionalSurroundWidth.Value != commonState.StudioSet.PartMotionalSurround[part].Width)
            {
                slStudioSetPartMotionalSurroundWidth.Value = commonState.StudioSet.PartMotionalSurround[part].Width;
                SetStudioSetStudioSetPartMotionalSurroundWidth(commonState.StudioSet.PartMotionalSurround[part].Width, (byte)part);
            }
            if (slStudioSetPartMotionalSurroundAmbienceSendLevel.Value != commonState.StudioSet.PartMotionalSurround[part].AmbienceSendLevel)
            {
                slStudioSetPartMotionalSurroundAmbienceSendLevel.Value = commonState.StudioSet.PartMotionalSurround[part].AmbienceSendLevel;
                SetStudioSetStudioSetPartMotionalSurroundAmbienceSendLevel(commonState.StudioSet.PartMotionalSurround[part].AmbienceSendLevel, (byte)part);
            }
            if (cbStudioSetPartEQSwitch.IsChecked != commonState.StudioSet.PartEQ[part].EqSwitch)
            {
                cbStudioSetPartEQSwitch.IsChecked = commonState.StudioSet.PartEQ[part].EqSwitch;
                SetStudioSetStudioSetPartEQ(commonState.StudioSet.PartEQ[part].EqSwitch, (byte)part);
            }
            if (cbStudioSetPartEQLowFreq.SelectedIndex != commonState.StudioSet.PartEQ[part].EqLowFreq)
            {
                cbStudioSetPartEQLowFreq.SelectedIndex = commonState.StudioSet.PartEQ[part].EqLowFreq;
                SetStudioSetStudioSetPartEQLoqFreq(commonState.StudioSet.PartEQ[part].EqLowFreq, (byte)part);
            }
            if (slStudioSetPartEQLowGain.Value != commonState.StudioSet.PartEQ[part].EqLowGain)
            {
                slStudioSetPartEQLowGain.Value = commonState.StudioSet.PartEQ[part].EqLowGain;
                SetStudioSetStudioSetPartEQLowGain(commonState.StudioSet.PartEQ[part].EqLowGain, (byte)part);
            }
            if (cbStudioSetPartEQMidFreq.SelectedIndex != commonState.StudioSet.PartEQ[part].EqMidFreq)
            {
                cbStudioSetPartEQMidFreq.SelectedIndex = commonState.StudioSet.PartEQ[part].EqMidFreq;
                SetStudioSetStudioSetPartEQMidFreq(commonState.StudioSet.PartEQ[part].EqMidFreq, (byte)part);
            }
            if (slStudioSetPartEQMidGain.Value != commonState.StudioSet.PartEQ[part].EqMidGain)
            {
                slStudioSetPartEQMidGain.Value = commonState.StudioSet.PartEQ[part].EqMidGain;
                SetStudioSetStudioSetPartEQMidGain(commonState.StudioSet.PartEQ[part].EqMidGain, (byte)part);
            }
            if (cbStudioSetPartEQMidQ.SelectedIndex != commonState.StudioSet.PartEQ[part].EqMidQ)
            {
                cbStudioSetPartEQMidQ.SelectedIndex = commonState.StudioSet.PartEQ[part].EqMidQ;
                SetStudioSetStudioSetPartEQMidQ(commonState.StudioSet.PartEQ[part].EqMidQ, (byte)part);
            }
            if (cbStudioSetPartEQHighFreq.SelectedIndex != commonState.StudioSet.PartEQ[part].EqHighFreq)
            {
                cbStudioSetPartEQHighFreq.SelectedIndex = commonState.StudioSet.PartEQ[part].EqHighFreq;
                SetStudioSetStudioSetPartEQHighFreq(commonState.StudioSet.PartEQ[part].EqHighFreq, (byte)part);
            }
            if (slStudioSetPartEQHighGain.Value != commonState.StudioSet.PartEQ[part].EqHighGain)
            {
                slStudioSetPartEQHighGain.Value = commonState.StudioSet.PartEQ[part].EqHighGain;
                SetStudioSetStudioSetPartEQHighGain(commonState.StudioSet.PartEQ[part].EqHighGain, (byte)part);
            }
        }
    }
}
