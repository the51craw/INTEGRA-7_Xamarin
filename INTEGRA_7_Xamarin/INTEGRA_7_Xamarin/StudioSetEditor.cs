using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        byte[] currentToneNumberAsBytes = new byte[3];
        Boolean initialGuiDone = false;
        ObservableCollection<String> StudioSetEditor_SearchResult;

//        Grid grid_StudioSet_And_Scanning = new Grid();
        Grid grid_PleaseWaitWhileScanning = new Grid();
        ProgressBar Progress = new ProgressBar();
        TextBlock tbPleaseWaitWhileScanning = new TextBlock();
//        Grid grid_MainStudioSet = new Grid();
        ColumnDefinition columndefinition_0004 = new ColumnDefinition();
        ColumnDefinition columndefinition_0005 = new ColumnDefinition();
        ColumnDefinition columndefinition_0006 = new ColumnDefinition();
        Grid grid_StudioSet_Column0 = new Grid();
        RowDefinition rowdefinition_0009 = new RowDefinition();
        RowDefinition rowdefinition_0010 = new RowDefinition();
        RowDefinition rowdefinition_0011 = new RowDefinition();
        RowDefinition rowdefinition_0012 = new RowDefinition();
        RowDefinition rowdefinition_0013 = new RowDefinition();
        RowDefinition rowdefinition_0014 = new RowDefinition();
        RowDefinition rowdefinition_0015 = new RowDefinition();
        RowDefinition rowdefinition_0016 = new RowDefinition();
        RowDefinition rowdefinition_0017 = new RowDefinition();
        RowDefinition rowdefinition_0018 = new RowDefinition();
        RowDefinition rowdefinition_0019 = new RowDefinition();
        RowDefinition rowdefinition_0020 = new RowDefinition();
        RowDefinition rowdefinition_0021 = new RowDefinition();
        RowDefinition rowdefinition_0022 = new RowDefinition();
        RowDefinition rowdefinition_0023 = new RowDefinition();
        RowDefinition rowdefinition_0024 = new RowDefinition();
        RowDefinition rowdefinition_0025 = new RowDefinition();
        RowDefinition rowdefinition_0026 = new RowDefinition();
        RowDefinition rowdefinition_0027 = new RowDefinition();
        Grid grid_StudioSetSelector = new Grid();
        ComboBox cbStudioSetSelector = new ComboBox();
        Grid grid_ToneControl1 = new Grid();
        ColumnDefinition columndefinition_0032 = new ColumnDefinition();
        ColumnDefinition columndefinition_0033 = new ColumnDefinition();
        TextBlock textblock_ToneControl1 = new TextBlock();
        ComboBox cbToneControl1 = new ComboBox();
        Grid grid_ToneControl2 = new Grid();
        ColumnDefinition columndefinition_0038 = new ColumnDefinition();
        ColumnDefinition columndefinition_0039 = new ColumnDefinition();
        TextBlock textblock_ToneControl2 = new TextBlock();
        ComboBox cbToneControl2 = new ComboBox();
        Grid grid_ToneControl3 = new Grid();
        ColumnDefinition columndefinition_0044 = new ColumnDefinition();
        ColumnDefinition columndefinition_0045 = new ColumnDefinition();
        TextBlock textblock_ToneControl3 = new TextBlock();
        ComboBox cbToneControl3 = new ComboBox();
        Grid grid_ToneControl4 = new Grid();
        ColumnDefinition columndefinition_0050 = new ColumnDefinition();
        ColumnDefinition columndefinition_0051 = new ColumnDefinition();
        TextBlock textblock_ToneControl4 = new TextBlock();
        ComboBox cbToneControl4 = new ComboBox();
        Grid grid_Tempo = new Grid();
        ColumnDefinition columndefinition_0056 = new ColumnDefinition();
        ColumnDefinition columndefinition_0057 = new ColumnDefinition();
        TextBlock tbTempo = new TextBlock();
        Slider slTempo = new Slider();
        Grid grid_SoloPart = new Grid();
        ColumnDefinition columndefinition_0061 = new ColumnDefinition();
        ColumnDefinition columndefinition_0062 = new ColumnDefinition();
        TextBlock tbSoloPart = new TextBlock();
        ComboBox cbSoloPart = new ComboBox();
        Grid grid_0064 = new Grid();
        Grid grid_0065 = new Grid();
        ColumnDefinition columndefinition_0067 = new ColumnDefinition();
        ColumnDefinition columndefinition_0068 = new ColumnDefinition();
        ColumnDefinition columndefinition_0069 = new ColumnDefinition();
        LabeledSwitch cbReverb = new LabeledSwitch("Reverb");
        LabeledSwitch cbChorus = new LabeledSwitch("Chorus");
        LabeledSwitch cbMasterEQ = new LabeledSwitch("Master EQ");
        //CheckBox cbReverb = new CheckBox();
        //CheckBox cbChorus = new CheckBox();
        //CheckBox cbMasterEQ = new CheckBox();
        //TextBlock tbReverb = new TextBlock();
        //TextBlock tbChorus = new TextBlock();
        //TextBlock tbMasterEQ = new TextBlock();
        Grid grid_0070 = new Grid();
        Grid grid_0071 = new Grid();
        ColumnDefinition columndefinition_0073 = new ColumnDefinition();
        ColumnDefinition columndefinition_0074 = new ColumnDefinition();
        TextBlock tbDrumCompEQPart = new TextBlock();
        ComboBox cbDrumCompEQPart = new ComboBox();
        Grid grid_0076 = new Grid();
        Grid grid_0077 = new Grid();
        ColumnDefinition columndefinition_0079 = new ColumnDefinition();
        ColumnDefinition columndefinition_0080 = new ColumnDefinition();
        TextBlock textblock_0081 = new TextBlock();
        ComboBox cbDrumCompEQ1OutputAssign = new ComboBox();
        Grid grid_0082 = new Grid();
        Grid grid_0083 = new Grid();
        ColumnDefinition columndefinition_0085 = new ColumnDefinition();
        ColumnDefinition columndefinition_0086 = new ColumnDefinition();
        TextBlock textblock_0087 = new TextBlock();
        ComboBox cbDrumCompEQ2OutputAssign = new ComboBox();
        Grid grid_0088 = new Grid();
        Grid grid_0089 = new Grid();
        ColumnDefinition columndefinition_0091 = new ColumnDefinition();
        ColumnDefinition columndefinition_0092 = new ColumnDefinition();
        TextBlock textblock_0093 = new TextBlock();
        ComboBox cbDrumCompEQ3OutputAssign = new ComboBox();
        Grid grid_0094 = new Grid();
        Grid grid_0095 = new Grid();
        ColumnDefinition columndefinition_0097 = new ColumnDefinition();
        ColumnDefinition columndefinition_0098 = new ColumnDefinition();
        TextBlock textblock_0099 = new TextBlock();
        ComboBox cbDrumCompEQ4OutputAssign = new ComboBox();
        Grid grid_0100 = new Grid();
        Grid grid_0101 = new Grid();
        ColumnDefinition columndefinition_0103 = new ColumnDefinition();
        ColumnDefinition columndefinition_0104 = new ColumnDefinition();
        TextBlock textblock_0105 = new TextBlock();
        ComboBox cbDrumCompEQ5OutputAssign = new ComboBox();
        Grid grid_0106 = new Grid();
        Grid grid_0107 = new Grid();
        ColumnDefinition columndefinition_0109 = new ColumnDefinition();
        ColumnDefinition columndefinition_0110 = new ColumnDefinition();
        TextBlock textblock_0111 = new TextBlock();
        ComboBox cbDrumCompEQ6OutputAssign = new ComboBox();
        Grid grid_0112 = new Grid();
        ColumnDefinition columndefinition_0114 = new ColumnDefinition();
        ColumnDefinition columndefinition_0115 = new ColumnDefinition();
        LabeledSwitch cbDrumCompEQ = new LabeledSwitch("Drum Comp/Equalizer");
        LabeledSwitch cbExtPartMute = new LabeledSwitch("Ext mute");
        Grid grid_0116 = new Grid();
        ColumnDefinition columndefinition_0118 = new ColumnDefinition();
        ColumnDefinition columndefinition_0119 = new ColumnDefinition();
        TextBlock tbExtPartLevel = new TextBlock();
        Slider slExtPartLevel = new Slider();
        Grid grid_0120 = new Grid();
        ColumnDefinition columndefinition_0122 = new ColumnDefinition();
        ColumnDefinition columndefinition_0123 = new ColumnDefinition();
        TextBlock tbExtPartChorusSend = new TextBlock();
        Slider slExtPartChorusSend = new Slider();
        Grid grid_0124 = new Grid();
        ColumnDefinition columndefinition_0126 = new ColumnDefinition();
        ColumnDefinition columndefinition_0127 = new ColumnDefinition();
        TextBlock tbExtPartReverbSend = new TextBlock();
        Slider slExtPartReverbSend = new Slider();
        Grid gEditStudioSetColumn1 = new Grid();
        RowDefinition rowdefinition_0129 = new RowDefinition();
        RowDefinition rowdefinition_0130 = new RowDefinition();
        Grid grid_Column2Selector = new Grid();
        ComboBox cbColumn2Selector = new ComboBox();
        Grid SystemCommonSettings = new Grid();
        RowDefinition rowdefinition_0133 = new RowDefinition();
        RowDefinition rowdefinition_0134 = new RowDefinition();
        RowDefinition rowdefinition_0135 = new RowDefinition();
        RowDefinition rowdefinition_0136 = new RowDefinition();
        RowDefinition rowdefinition_0137 = new RowDefinition();
        RowDefinition rowdefinition_0138 = new RowDefinition();
        RowDefinition rowdefinition_0139 = new RowDefinition();
        RowDefinition rowdefinition_0140 = new RowDefinition();
        RowDefinition rowdefinition_0141 = new RowDefinition();
        RowDefinition rowdefinition_0142 = new RowDefinition();
        RowDefinition rowdefinition_0143 = new RowDefinition();
        RowDefinition rowdefinition_0144 = new RowDefinition();
        RowDefinition rowdefinition_0145 = new RowDefinition();
        RowDefinition rowdefinition_0146 = new RowDefinition();
        RowDefinition rowdefinition_0147 = new RowDefinition();
        RowDefinition rowdefinition_0148 = new RowDefinition();
        RowDefinition rowdefinition_0149 = new RowDefinition();
        RowDefinition rowdefinition_0150 = new RowDefinition();
        Grid grid_0151 = new Grid();
        ColumnDefinition columndefinition_0153 = new ColumnDefinition();
        ColumnDefinition columndefinition_0154 = new ColumnDefinition();
        TextBlock tbSystemCommonMasterTune = new TextBlock();
        Slider slSystemCommonMasterTune = new Slider();
        Grid grid_0155 = new Grid();
        ColumnDefinition columndefinition_0157 = new ColumnDefinition();
        ColumnDefinition columndefinition_0158 = new ColumnDefinition();
        TextBlock tbSystemCommonMasterKeyShift = new TextBlock();
        Slider slSystemCommonMasterKeyShift = new Slider();
        Grid grid_0159 = new Grid();
        ColumnDefinition columndefinition_0161 = new ColumnDefinition();
        ColumnDefinition columndefinition_0162 = new ColumnDefinition();
        TextBlock tbSystemCommonMasterLevel = new TextBlock();
        Slider slSystemCommonMasterLevel = new Slider();
        Grid grid_0163 = new Grid();
        CheckBox cbSystemCommonScaleTuneSwitch = new CheckBox();
        Grid grid_0164 = new Grid();
        ColumnDefinition columndefinition_0166 = new ColumnDefinition();
        ColumnDefinition columndefinition_0167 = new ColumnDefinition();
        TextBlock textblock_0168 = new TextBlock();
        ComboBox cbSystemCommonStudioSetControlChannel = new ComboBox();
        Grid grid_0169 = new Grid();
        ColumnDefinition columndefinition_0171 = new ColumnDefinition();
        ColumnDefinition columndefinition_0172 = new ColumnDefinition();
        TextBlock tbSystemCommonSystemControlSource1 = new TextBlock();
        ComboBox cbSystemCommonSystemControlSource1 = new ComboBox();
        Grid grid_0173 = new Grid();
        ColumnDefinition columndefinition_0175 = new ColumnDefinition();
        ColumnDefinition columndefinition_0176 = new ColumnDefinition();
        TextBlock tbSystemCommonSystemControlSource2 = new TextBlock();
        ComboBox cbSystemCommonSystemControlSource2 = new ComboBox();
        Grid grid_0177 = new Grid();
        ColumnDefinition columndefinition_0179 = new ColumnDefinition();
        ColumnDefinition columndefinition_0180 = new ColumnDefinition();
        TextBlock tbSystemCommonSystemControlSource3 = new TextBlock();
        ComboBox cbSystemCommonSystemControlSource3 = new ComboBox();
        Grid grid_0181 = new Grid();
        ColumnDefinition columndefinition_0183 = new ColumnDefinition();
        ColumnDefinition columndefinition_0184 = new ColumnDefinition();
        TextBlock tbSystemCommonSystemControlSource4 = new TextBlock();
        ComboBox cbSystemCommonSystemControlSource4 = new ComboBox();
        Grid grid_0185 = new Grid();
        ColumnDefinition columndefinition_0187 = new ColumnDefinition();
        ColumnDefinition columndefinition_0188 = new ColumnDefinition();
        TextBlock textblock_0189 = new TextBlock();
        ComboBox cbSystemCommonControlSource = new ComboBox();
        Grid grid_0190 = new Grid();
        ColumnDefinition columndefinition_0192 = new ColumnDefinition();
        ColumnDefinition columndefinition_0193 = new ColumnDefinition();
        TextBlock textblock_0194 = new TextBlock();
        ComboBox cbSystemCommonSystemClockSource = new ComboBox();
        Grid grid_0195 = new Grid();
        ColumnDefinition columndefinition_0197 = new ColumnDefinition();
        ColumnDefinition columndefinition_0198 = new ColumnDefinition();
        TextBlock textblock_0199 = new TextBlock();
        Slider slSystemCommonSystemTempo = new Slider();
        Grid grid_0200 = new Grid();
        ColumnDefinition columndefinition_0202 = new ColumnDefinition();
        ColumnDefinition columndefinition_0203 = new ColumnDefinition();
        TextBlock textblock_0204 = new TextBlock();
        ComboBox cbSystemCommonTempoAssignSource = new ComboBox();
        Grid grid_0205 = new Grid();
        CheckBox cbSystemCommonReceiveProgramChange = new CheckBox();
        Grid grid_0206 = new Grid();
        CheckBox cbSystemCommonReceiveBankSelect = new CheckBox();
        Grid grid_0207 = new Grid();
        CheckBox cbSystemCommonSurroundCenterSpeakerSwitch = new CheckBox();
        Grid grid_0208 = new Grid();
        CheckBox cbSystemCommonSurroundSubWooferSwitch = new CheckBox();
        Grid grid_0209 = new Grid();
        ColumnDefinition columndefinition_0211 = new ColumnDefinition();
        ColumnDefinition columndefinition_0212 = new ColumnDefinition();
        TextBlock textblock_0213 = new TextBlock();
        ComboBox cbSystemCommonStereoOutputMode = new ComboBox();
        Grid VoiceReserve = new Grid();
        RowDefinition rowdefinition_0215 = new RowDefinition();
        RowDefinition rowdefinition_0216 = new RowDefinition();
        RowDefinition rowdefinition_0217 = new RowDefinition();
        RowDefinition rowdefinition_0218 = new RowDefinition();
        RowDefinition rowdefinition_0219 = new RowDefinition();
        RowDefinition rowdefinition_0220 = new RowDefinition();
        RowDefinition rowdefinition_0221 = new RowDefinition();
        RowDefinition rowdefinition_0222 = new RowDefinition();
        RowDefinition rowdefinition_0223 = new RowDefinition();
        RowDefinition rowdefinition_0224 = new RowDefinition();
        RowDefinition rowdefinition_0225 = new RowDefinition();
        RowDefinition rowdefinition_0226 = new RowDefinition();
        RowDefinition rowdefinition_0227 = new RowDefinition();
        RowDefinition rowdefinition_0228 = new RowDefinition();
        RowDefinition rowdefinition_0229 = new RowDefinition();
        RowDefinition rowdefinition_0230 = new RowDefinition();
        Grid grid_0231 = new Grid();
        ColumnDefinition columndefinition_0233 = new ColumnDefinition();
        ColumnDefinition columndefinition_0234 = new ColumnDefinition();
        TextBlock textblock_0235 = new TextBlock();
        Slider slVoiceReserve01 = new Slider();
        Grid grid_0236 = new Grid();
        ColumnDefinition columndefinition_0238 = new ColumnDefinition();
        ColumnDefinition columndefinition_0239 = new ColumnDefinition();
        TextBlock textblock_0240 = new TextBlock();
        Slider slVoiceReserve02 = new Slider();
        Grid grid_0241 = new Grid();
        ColumnDefinition columndefinition_0243 = new ColumnDefinition();
        ColumnDefinition columndefinition_0244 = new ColumnDefinition();
        TextBlock textblock_0245 = new TextBlock();
        Slider slVoiceReserve03 = new Slider();
        Grid grid_0246 = new Grid();
        ColumnDefinition columndefinition_0248 = new ColumnDefinition();
        ColumnDefinition columndefinition_0249 = new ColumnDefinition();
        TextBlock textblock_0250 = new TextBlock();
        Slider slVoiceReserve04 = new Slider();
        Grid grid_0251 = new Grid();
        ColumnDefinition columndefinition_0253 = new ColumnDefinition();
        ColumnDefinition columndefinition_0254 = new ColumnDefinition();
        TextBlock textblock_0255 = new TextBlock();
        Slider slVoiceReserve05 = new Slider();
        Grid grid_0256 = new Grid();
        ColumnDefinition columndefinition_0258 = new ColumnDefinition();
        ColumnDefinition columndefinition_0259 = new ColumnDefinition();
        TextBlock textblock_0260 = new TextBlock();
        Slider slVoiceReserve06 = new Slider();
        Grid grid_0261 = new Grid();
        ColumnDefinition columndefinition_0263 = new ColumnDefinition();
        ColumnDefinition columndefinition_0264 = new ColumnDefinition();
        TextBlock textblock_0265 = new TextBlock();
        Slider slVoiceReserve07 = new Slider();
        Grid grid_0266 = new Grid();
        ColumnDefinition columndefinition_0268 = new ColumnDefinition();
        ColumnDefinition columndefinition_0269 = new ColumnDefinition();
        TextBlock textblock_0270 = new TextBlock();
        Slider slVoiceReserve08 = new Slider();
        Grid grid_0271 = new Grid();
        ColumnDefinition columndefinition_0273 = new ColumnDefinition();
        ColumnDefinition columndefinition_0274 = new ColumnDefinition();
        TextBlock textblock_0275 = new TextBlock();
        Slider slVoiceReserve09 = new Slider();
        Grid grid_0276 = new Grid();
        ColumnDefinition columndefinition_0278 = new ColumnDefinition();
        ColumnDefinition columndefinition_0279 = new ColumnDefinition();
        TextBlock textblock_0280 = new TextBlock();
        Slider slVoiceReserve10 = new Slider();
        Grid grid_0281 = new Grid();
        ColumnDefinition columndefinition_0283 = new ColumnDefinition();
        ColumnDefinition columndefinition_0284 = new ColumnDefinition();
        TextBlock textblock_0285 = new TextBlock();
        Slider slVoiceReserve11 = new Slider();
        Grid grid_0286 = new Grid();
        ColumnDefinition columndefinition_0288 = new ColumnDefinition();
        ColumnDefinition columndefinition_0289 = new ColumnDefinition();
        TextBlock textblock_0290 = new TextBlock();
        Slider slVoiceReserve12 = new Slider();
        Grid grid_0291 = new Grid();
        ColumnDefinition columndefinition_0293 = new ColumnDefinition();
        ColumnDefinition columndefinition_0294 = new ColumnDefinition();
        TextBlock textblock_0295 = new TextBlock();
        Slider slVoiceReserve13 = new Slider();
        Grid grid_0296 = new Grid();
        ColumnDefinition columndefinition_0298 = new ColumnDefinition();
        ColumnDefinition columndefinition_0299 = new ColumnDefinition();
        TextBlock textblock_0300 = new TextBlock();
        Slider slVoiceReserve14 = new Slider();
        Grid grid_0301 = new Grid();
        ColumnDefinition columndefinition_0303 = new ColumnDefinition();
        ColumnDefinition columndefinition_0304 = new ColumnDefinition();
        TextBlock textblock_0305 = new TextBlock();
        Slider slVoiceReserve15 = new Slider();
        Grid grid_0306 = new Grid();
        ColumnDefinition columndefinition_0308 = new ColumnDefinition();
        ColumnDefinition columndefinition_0309 = new ColumnDefinition();
        TextBlock textblock_0310 = new TextBlock();
        Slider slVoiceReserve16 = new Slider();
        Grid Chorus = new Grid();
        RowDefinition rowdefinition_0312 = new RowDefinition();
        RowDefinition rowdefinition_0313 = new RowDefinition();
        RowDefinition rowdefinition_0314 = new RowDefinition();
        RowDefinition rowdefinition_0315 = new RowDefinition();
        RowDefinition rowdefinition_0316 = new RowDefinition();
        Grid grid_0317 = new Grid();
        ColumnDefinition columndefinition_0319 = new ColumnDefinition();
        ColumnDefinition columndefinition_0320 = new ColumnDefinition();
        TextBlock textblock_0321 = new TextBlock();
        ComboBox cbStudioSetChorusType = new ComboBox();
        Grid grid_0322 = new Grid();
        ColumnDefinition columndefinition_0324 = new ColumnDefinition();
        ColumnDefinition columndefinition_0325 = new ColumnDefinition();
        TextBlock tbChorusLevel = new TextBlock();
        Slider slChorusLevel = new Slider();
        Grid grid_0326 = new Grid();
        ColumnDefinition columndefinition_0328 = new ColumnDefinition();
        ColumnDefinition columndefinition_0329 = new ColumnDefinition();
        TextBlock textblock_0330 = new TextBlock();
        ComboBox cbChorusOutputAssign = new ComboBox();
        Grid grid_0331 = new Grid();
        ColumnDefinition columndefinition_0333 = new ColumnDefinition();
        ColumnDefinition columndefinition_0334 = new ColumnDefinition();
        TextBlock textblock_0335 = new TextBlock();
        ComboBox cbChorusOutputSelect = new ComboBox();
        Grid ChorusChorus = new Grid();
        RowDefinition rowdefinition_0337 = new RowDefinition();
        RowDefinition rowdefinition_0338 = new RowDefinition();
        RowDefinition rowdefinition_0339 = new RowDefinition();
        RowDefinition rowdefinition_0340 = new RowDefinition();
        RowDefinition rowdefinition_0341 = new RowDefinition();
        RowDefinition rowdefinition_0342 = new RowDefinition();
        RowDefinition rowdefinition_0343 = new RowDefinition();
        RowDefinition rowdefinition_0344 = new RowDefinition();
        RowDefinition rowdefinition_0345 = new RowDefinition();
        Grid grid_0346 = new Grid();
        ColumnDefinition columndefinition_0348 = new ColumnDefinition();
        ColumnDefinition columndefinition_0349 = new ColumnDefinition();
        TextBlock tbChorusChorusFilterType = new TextBlock();
        ComboBox cbChorusChorusFilterType = new ComboBox();
        Grid grid_0350 = new Grid();
        ColumnDefinition columndefinition_0352 = new ColumnDefinition();
        ColumnDefinition columndefinition_0353 = new ColumnDefinition();
        TextBlock tbChorusChorusFilterCutoffFrequency = new TextBlock();
        ComboBox cbChorusChorusFilterCutoffFrequency = new ComboBox();
        Grid grid_0354 = new Grid();
        ColumnDefinition columndefinition_0356 = new ColumnDefinition();
        ColumnDefinition columndefinition_0357 = new ColumnDefinition();
        TextBlock tbChorusChorusPreDelay = new TextBlock();
        Slider slChorusChorusPreDelay = new Slider();
        Grid grid_0358 = new Grid();
        ColumnDefinition columndefinition_0360 = new ColumnDefinition();
        ColumnDefinition columndefinition_0361 = new ColumnDefinition();
        TextBlock tbChorusChorusRateHzNote = new TextBlock();
        ComboBox cbChorusChorusRateHzNote = new ComboBox();
        Grid ChorusRateHz = new Grid();
        ColumnDefinition columndefinition_0363 = new ColumnDefinition();
        ColumnDefinition columndefinition_0364 = new ColumnDefinition();
        TextBlock tbChorusChorusRateHz = new TextBlock();
        Slider slChorusChorusRateHz = new Slider();
        Grid ChorusRateNote = new Grid();
        ColumnDefinition columndefinition_0366 = new ColumnDefinition();
        ColumnDefinition columndefinition_0367 = new ColumnDefinition();
        TextBlock tbChorusChorusRateNote = new TextBlock();
        Slider slChorusChorusRateNote = new Slider();
        Grid ChorusDepth = new Grid();
        ColumnDefinition columndefinition_0369 = new ColumnDefinition();
        ColumnDefinition columndefinition_0370 = new ColumnDefinition();
        TextBlock tbChorusChorusDepth = new TextBlock();
        Slider slChorusChorusDepth = new Slider();
        Grid grid_0371 = new Grid();
        ColumnDefinition columndefinition_0373 = new ColumnDefinition();
        ColumnDefinition columndefinition_0374 = new ColumnDefinition();
        TextBlock tbChorusChorusPhase = new TextBlock();
        Slider slChorusChorusPhase = new Slider();
        Grid grid_0375 = new Grid();
        ColumnDefinition columndefinition_0377 = new ColumnDefinition();
        ColumnDefinition columndefinition_0378 = new ColumnDefinition();
        TextBlock tbChorusChorusFeedback = new TextBlock();
        Slider slChorusChorusFeedback = new Slider();
        Grid ChorusDelay = new Grid();
        RowDefinition rowdefinition_0380 = new RowDefinition();
        RowDefinition rowdefinition_0381 = new RowDefinition();
        RowDefinition rowdefinition_0382 = new RowDefinition();
        RowDefinition rowdefinition_0383 = new RowDefinition();
        RowDefinition rowdefinition_0384 = new RowDefinition();
        RowDefinition rowdefinition_0385 = new RowDefinition();
        RowDefinition rowdefinition_0386 = new RowDefinition();
        RowDefinition rowdefinition_0387 = new RowDefinition();
        RowDefinition rowdefinition_0388 = new RowDefinition();
        RowDefinition rowdefinition_0389 = new RowDefinition();
        RowDefinition rowdefinition_0390 = new RowDefinition();
        RowDefinition rowdefinition_0391 = new RowDefinition();
        Grid grid_0392 = new Grid();
        ColumnDefinition columndefinition_0394 = new ColumnDefinition();
        ColumnDefinition columndefinition_0395 = new ColumnDefinition();
        TextBlock textblock_0396 = new TextBlock();
        ComboBox cbChorusDelayLeftMsNote = new ComboBox();
        Grid ChorusDelayLeftHz = new Grid();
        ColumnDefinition columndefinition_0398 = new ColumnDefinition();
        ColumnDefinition columndefinition_0399 = new ColumnDefinition();
        TextBlock tbChorusDelayLeftHz = new TextBlock();
        Slider slChorusDelayLeftHz = new Slider();
        Grid ChorusDelayLeftNote = new Grid();
        ColumnDefinition columndefinition_0401 = new ColumnDefinition();
        ColumnDefinition columndefinition_0402 = new ColumnDefinition();
        TextBlock tbChorusDelayLeftNote = new TextBlock();
        Slider slChorusDelayLeftNote = new Slider();
        Grid grid_0403 = new Grid();
        ColumnDefinition columndefinition_0405 = new ColumnDefinition();
        ColumnDefinition columndefinition_0406 = new ColumnDefinition();
        TextBlock textblock_0407 = new TextBlock();
        ComboBox cbChorusDelayRightMsNote = new ComboBox();
        Grid ChorusDelayRightHz = new Grid();
        ColumnDefinition columndefinition_0409 = new ColumnDefinition();
        ColumnDefinition columndefinition_0410 = new ColumnDefinition();
        TextBlock tbChorusDelayRightHz = new TextBlock();
        Slider slChorusDelayRightHz = new Slider();
        Grid ChorusDelayRightNote = new Grid();
        ColumnDefinition columndefinition_0412 = new ColumnDefinition();
        ColumnDefinition columndefinition_0413 = new ColumnDefinition();
        TextBlock tbChorusDelayRightNote = new TextBlock();
        Slider slChorusDelayRightNote = new Slider();
        Grid grid_0414 = new Grid();
        ColumnDefinition columndefinition_0416 = new ColumnDefinition();
        ColumnDefinition columndefinition_0417 = new ColumnDefinition();
        TextBlock textblock_0418 = new TextBlock();
        ComboBox cbChorusDelayCenterMsNote = new ComboBox();
        Grid ChorusDelayCenterHz = new Grid();
        ColumnDefinition columndefinition_0420 = new ColumnDefinition();
        ColumnDefinition columndefinition_0421 = new ColumnDefinition();
        TextBlock tbChorusDelayCenterHz = new TextBlock();
        Slider slChorusDelayCenterHz = new Slider();
        Grid ChorusDelayCenterNote = new Grid();
        ColumnDefinition columndefinition_0423 = new ColumnDefinition();
        ColumnDefinition columndefinition_0424 = new ColumnDefinition();
        TextBlock tbChorusDelayCenterNote = new TextBlock();
        Slider slChorusDelayCenterNote = new Slider();
        Grid grid_0425 = new Grid();
        ColumnDefinition columndefinition_0427 = new ColumnDefinition();
        ColumnDefinition columndefinition_0428 = new ColumnDefinition();
        TextBlock tbChorusDelayCenterFeedback = new TextBlock();
        Slider slChorusDelayCenterFeedback = new Slider();
        Grid grid_0429 = new Grid();
        ColumnDefinition columndefinition_0431 = new ColumnDefinition();
        ColumnDefinition columndefinition_0432 = new ColumnDefinition();
        TextBlock tbChorusDelayHFDamp = new TextBlock();
        ComboBox cbChorusDelayHFDamp = new ComboBox();
        Grid grid_0433 = new Grid();
        ColumnDefinition columndefinition_0435 = new ColumnDefinition();
        ColumnDefinition columndefinition_0436 = new ColumnDefinition();
        TextBlock tbChorusDelayLeftLevel = new TextBlock();
        Slider slChorusDelayLeftLevel = new Slider();
        Grid grid_0437 = new Grid();
        ColumnDefinition columndefinition_0439 = new ColumnDefinition();
        ColumnDefinition columndefinition_0440 = new ColumnDefinition();
        TextBlock tbChorusDelayRightLevel = new TextBlock();
        Slider slChorusDelayRightLevel = new Slider();
        Grid grid_0441 = new Grid();
        ColumnDefinition columndefinition_0443 = new ColumnDefinition();
        ColumnDefinition columndefinition_0444 = new ColumnDefinition();
        TextBlock tbChorusDelayCenterLevel = new TextBlock();
        Slider slChorusDelayCenterLevel = new Slider();
        Grid ChorusGM2Chorus = new Grid();
        RowDefinition rowdefinition_0446 = new RowDefinition();
        RowDefinition rowdefinition_0447 = new RowDefinition();
        RowDefinition rowdefinition_0448 = new RowDefinition();
        RowDefinition rowdefinition_0449 = new RowDefinition();
        RowDefinition rowdefinition_0450 = new RowDefinition();
        RowDefinition rowdefinition_0451 = new RowDefinition();
        RowDefinition rowdefinition_0452 = new RowDefinition();
        RowDefinition rowdefinition_0453 = new RowDefinition();
        Grid ChorusGM2ChorusPreLPF = new Grid();
        ColumnDefinition columndefinition_0455 = new ColumnDefinition();
        ColumnDefinition columndefinition_0456 = new ColumnDefinition();
        TextBlock tbChorusGM2ChorusPreLPF = new TextBlock();
        Slider slChorusGM2ChorusPreLPF = new Slider();
        Grid ChorusGM2ChorusLevel = new Grid();
        ColumnDefinition columndefinition_0458 = new ColumnDefinition();
        ColumnDefinition columndefinition_0459 = new ColumnDefinition();
        TextBlock tbChorusGM2ChorusLevel = new TextBlock();
        Slider slChorusGM2ChorusLevel = new Slider();
        Grid ChorusGM2ChorusFeedback = new Grid();
        ColumnDefinition columndefinition_0461 = new ColumnDefinition();
        ColumnDefinition columndefinition_0462 = new ColumnDefinition();
        TextBlock tbChorusGM2ChorusFeedback = new TextBlock();
        Slider slChorusGM2ChorusFeedback = new Slider();
        Grid ChorusGM2ChorusDelay = new Grid();
        ColumnDefinition columndefinition_0464 = new ColumnDefinition();
        ColumnDefinition columndefinition_0465 = new ColumnDefinition();
        TextBlock tbChorusGM2ChorusDelay = new TextBlock();
        Slider slChorusGM2ChorusDelay = new Slider();
        Grid ChorusGM2ChorusRate = new Grid();
        ColumnDefinition columndefinition_0467 = new ColumnDefinition();
        ColumnDefinition columndefinition_0468 = new ColumnDefinition();
        TextBlock tbChorusGM2ChorusRate = new TextBlock();
        Slider slChorusGM2ChorusRate = new Slider();
        Grid ChorusGM2ChorusDepth = new Grid();
        ColumnDefinition columndefinition_0470 = new ColumnDefinition();
        ColumnDefinition columndefinition_0471 = new ColumnDefinition();
        TextBlock tbChorusGM2ChorusDepth = new TextBlock();
        Slider slChorusGM2ChorusDepth = new Slider();
        Grid ChorusGM2ChorusSendLevelToReverb = new Grid();
        ColumnDefinition columndefinition_0473 = new ColumnDefinition();
        ColumnDefinition columndefinition_0474 = new ColumnDefinition();
        TextBlock tbChorusGM2ChorusSendLevelToReverb = new TextBlock();
        Slider slChorusGM2ChorusSendLevelToReverb = new Slider();
        Grid ChorusReverb = new Grid();
        RowDefinition rowdefinition_0476 = new RowDefinition();
        RowDefinition rowdefinition_0477 = new RowDefinition();
        RowDefinition rowdefinition_0478 = new RowDefinition();
        RowDefinition rowdefinition_0479 = new RowDefinition();
        Grid grid_0480 = new Grid();
        ColumnDefinition columndefinition_0482 = new ColumnDefinition();
        ColumnDefinition columndefinition_0483 = new ColumnDefinition();
        TextBlock textblock_0484 = new TextBlock();
        ComboBox cbStudioSetReverbType = new ComboBox();
        Grid grid_0485 = new Grid();
        ColumnDefinition columndefinition_0487 = new ColumnDefinition();
        ColumnDefinition columndefinition_0488 = new ColumnDefinition();
        TextBlock tbStudioSetReverbLevel = new TextBlock();
        Slider slStudioSetReverbLevel = new Slider();
        Grid grid_0489 = new Grid();
        ColumnDefinition columndefinition_0491 = new ColumnDefinition();
        ColumnDefinition columndefinition_0492 = new ColumnDefinition();
        TextBlock tbStudioSetReverbOutputAssign = new TextBlock();
        ComboBox cbStudioSetReverbOutputAssign = new ComboBox();
        Grid StudioSetReverb = new Grid();
        RowDefinition rowdefinition_0494 = new RowDefinition();
        RowDefinition rowdefinition_0495 = new RowDefinition();
        RowDefinition rowdefinition_0496 = new RowDefinition();
        RowDefinition rowdefinition_0497 = new RowDefinition();
        RowDefinition rowdefinition_0498 = new RowDefinition();
        RowDefinition rowdefinition_0499 = new RowDefinition();
        RowDefinition rowdefinition_0500 = new RowDefinition();
        RowDefinition rowdefinition_0501 = new RowDefinition();
        RowDefinition rowdefinition_0502 = new RowDefinition();
        Grid grid_0503 = new Grid();
        ColumnDefinition columndefinition_0505 = new ColumnDefinition();
        ColumnDefinition columndefinition_0506 = new ColumnDefinition();
        TextBlock tbStudioSetReverbPreDelay = new TextBlock();
        Slider slStudioSetReverbPreDelay = new Slider();
        Grid grid_0507 = new Grid();
        ColumnDefinition columndefinition_0509 = new ColumnDefinition();
        ColumnDefinition columndefinition_0510 = new ColumnDefinition();
        TextBlock tbStudioSetReverbTime = new TextBlock();
        Slider slStudioSetReverbTime = new Slider();
        Grid grid_0511 = new Grid();
        ColumnDefinition columndefinition_0513 = new ColumnDefinition();
        ColumnDefinition columndefinition_0514 = new ColumnDefinition();
        TextBlock tbStudioSetReverbDensity = new TextBlock();
        Slider slStudioSetReverbDensity = new Slider();
        Grid grid_0515 = new Grid();
        ColumnDefinition columndefinition_0517 = new ColumnDefinition();
        ColumnDefinition columndefinition_0518 = new ColumnDefinition();
        TextBlock tbStudioSetReverbDiffusion = new TextBlock();
        Slider slStudioSetReverbDiffusion = new Slider();
        Grid grid_0519 = new Grid();
        ColumnDefinition columndefinition_0521 = new ColumnDefinition();
        ColumnDefinition columndefinition_0522 = new ColumnDefinition();
        TextBlock tbStudioSetReverbLFDamp = new TextBlock();
        Slider slStudioSetReverbLFDamp = new Slider();
        Grid grid_0523 = new Grid();
        ColumnDefinition columndefinition_0525 = new ColumnDefinition();
        ColumnDefinition columndefinition_0526 = new ColumnDefinition();
        TextBlock tbStudioSetReverbHFDamp = new TextBlock();
        Slider slStudioSetReverbHFDamp = new Slider();
        Grid grid_0527 = new Grid();
        ColumnDefinition columndefinition_0529 = new ColumnDefinition();
        ColumnDefinition columndefinition_0530 = new ColumnDefinition();
        TextBlock tbStudioSetReverbSpread = new TextBlock();
        Slider slStudioSetReverbSpread = new Slider();
        Grid grid_0531 = new Grid();
        ColumnDefinition columndefinition_0533 = new ColumnDefinition();
        ColumnDefinition columndefinition_0534 = new ColumnDefinition();
        TextBlock tbStudioSetReverbTone = new TextBlock();
        Slider slStudioSetReverbTone = new Slider();
        Grid StudioSetReverbGM2 = new Grid();
        RowDefinition rowdefinition_0536 = new RowDefinition();
        RowDefinition rowdefinition_0537 = new RowDefinition();
        RowDefinition rowdefinition_0538 = new RowDefinition();
        Grid grid_0539 = new Grid();
        ColumnDefinition columndefinition_0541 = new ColumnDefinition();
        ColumnDefinition columndefinition_0542 = new ColumnDefinition();
        TextBlock tbStudioSetReverbGM2Character = new TextBlock();
        Slider slStudioSetReverbGM2Character = new Slider();
        Grid grid_0543 = new Grid();
        ColumnDefinition columndefinition_0545 = new ColumnDefinition();
        ColumnDefinition columndefinition_0546 = new ColumnDefinition();
        TextBlock tbStudioSetReverbGM2Time = new TextBlock();
        Slider slStudioSetReverbGM2Time = new Slider();
        Grid StudioSetMotionalSurround = new Grid();
        RowDefinition rowdefinition_0548 = new RowDefinition();
        RowDefinition rowdefinition_0549 = new RowDefinition();
        RowDefinition rowdefinition_0550 = new RowDefinition();
        RowDefinition rowdefinition_0551 = new RowDefinition();
        RowDefinition rowdefinition_0552 = new RowDefinition();
        RowDefinition rowdefinition_0553 = new RowDefinition();
        RowDefinition rowdefinition_0554 = new RowDefinition();
        RowDefinition rowdefinition_0555 = new RowDefinition();
        RowDefinition rowdefinition_0556 = new RowDefinition();
        RowDefinition rowdefinition_0557 = new RowDefinition();
        RowDefinition rowdefinition_0558 = new RowDefinition();
        RowDefinition rowdefinition_0559 = new RowDefinition();
        RowDefinition rowdefinition_0560 = new RowDefinition();
        RowDefinition rowdefinition_0561 = new RowDefinition();
        RowDefinition rowdefinition_0562 = new RowDefinition();
        RowDefinition rowdefinition_0563 = new RowDefinition();
        RowDefinition rowdefinition_0564 = new RowDefinition();
        RowDefinition rowdefinition_0565 = new RowDefinition();
        Grid grid_0566 = new Grid();
        CheckBox cbStudioSetMotionalSurround = new CheckBox();
        Grid grid_0567 = new Grid();
        ColumnDefinition columndefinition_0569 = new ColumnDefinition();
        ColumnDefinition columndefinition_0570 = new ColumnDefinition();
        TextBlock textblock_0571 = new TextBlock();
        ComboBox cbStudioSetMotionalSurroundRoomType = new ComboBox();
        Grid grid_0572 = new Grid();
        ColumnDefinition columndefinition_0574 = new ColumnDefinition();
        ColumnDefinition columndefinition_0575 = new ColumnDefinition();
        TextBlock tbStudioSetMotionalSurroundAmbienceLevel = new TextBlock();
        Slider slStudioSetMotionalSurroundAmbienceLevel = new Slider();
        Grid grid_0576 = new Grid();
        ColumnDefinition columndefinition_0578 = new ColumnDefinition();
        ColumnDefinition columndefinition_0579 = new ColumnDefinition();
        TextBlock textblock_0580 = new TextBlock();
        ComboBox cbStudioSetMotionalSurroundRoomSize = new ComboBox();
        Grid grid_0581 = new Grid();
        ColumnDefinition columndefinition_0583 = new ColumnDefinition();
        ColumnDefinition columndefinition_0584 = new ColumnDefinition();
        TextBlock tbStudioSetMotionalSurroundAmbienceTime = new TextBlock();
        Slider slStudioSetMotionalSurroundAmbienceTime = new Slider();
        Grid grid_0585 = new Grid();
        ColumnDefinition columndefinition_0587 = new ColumnDefinition();
        ColumnDefinition columndefinition_0588 = new ColumnDefinition();
        TextBlock tbStudioSetMotionalSurroundAmbienceDensity = new TextBlock();
        Slider slStudioSetMotionalSurroundAmbienceDensity = new Slider();
        Grid grid_0589 = new Grid();
        ColumnDefinition columndefinition_0591 = new ColumnDefinition();
        ColumnDefinition columndefinition_0592 = new ColumnDefinition();
        TextBlock tbStudioSetMotionalSurroundAmbienceHFDamp = new TextBlock();
        Slider slStudioSetMotionalSurroundAmbienceHFDamp = new Slider();
        Grid grid_0593 = new Grid();
        ColumnDefinition columndefinition_0595 = new ColumnDefinition();
        ColumnDefinition columndefinition_0596 = new ColumnDefinition();
        TextBlock tbStudioSetMotionalSurroundExternalPartLR = new TextBlock();
        Slider slStudioSetMotionalSurroundExternalPartLR = new Slider();
        Grid grid_0597 = new Grid();
        ColumnDefinition columndefinition_0599 = new ColumnDefinition();
        ColumnDefinition columndefinition_0600 = new ColumnDefinition();
        TextBlock tbStudioSetMotionalSurroundExternalPartFB = new TextBlock();
        Slider slStudioSetMotionalSurroundExternalPartFB = new Slider();
        Grid grid_0601 = new Grid();
        ColumnDefinition columndefinition_0603 = new ColumnDefinition();
        ColumnDefinition columndefinition_0604 = new ColumnDefinition();
        TextBlock tbStudioSetMotionalSurroundExtPartWidth = new TextBlock();
        Slider slStudioSetMotionalSurroundExtPartWidth = new Slider();
        Grid grid_0605 = new Grid();
        ColumnDefinition columndefinition_0607 = new ColumnDefinition();
        ColumnDefinition columndefinition_0608 = new ColumnDefinition();
        TextBlock tbStudioSetMotionalSurroundExtpartAmbienceSend = new TextBlock();
        Slider slStudioSetMotionalSurroundExtpartAmbienceSend = new Slider();
        Grid grid_0609 = new Grid();
        ColumnDefinition columndefinition_0611 = new ColumnDefinition();
        ColumnDefinition columndefinition_0612 = new ColumnDefinition();
        TextBlock textblock_0613 = new TextBlock();
        ComboBox cbStudioSetMotionalSurroundExtPartControl = new ComboBox();
        Grid grid_0614 = new Grid();
        ColumnDefinition columndefinition_0616 = new ColumnDefinition();
        ColumnDefinition columndefinition_0617 = new ColumnDefinition();
        TextBlock tbStudioSetMotionalSurroundDepth = new TextBlock();
        Slider slStudioSetMotionalSurroundDepth = new Slider();
        Grid StudioSetMasterEQ = new Grid();
        RowDefinition rowdefinition_0619 = new RowDefinition();
        RowDefinition rowdefinition_0620 = new RowDefinition();
        RowDefinition rowdefinition_0621 = new RowDefinition();
        RowDefinition rowdefinition_0622 = new RowDefinition();
        RowDefinition rowdefinition_0623 = new RowDefinition();
        RowDefinition rowdefinition_0624 = new RowDefinition();
        RowDefinition rowdefinition_0625 = new RowDefinition();
        RowDefinition rowdefinition_0626 = new RowDefinition();
        RowDefinition rowdefinition_0627 = new RowDefinition();
        RowDefinition rowdefinition_0628 = new RowDefinition();
        RowDefinition rowdefinition_0629 = new RowDefinition();
        RowDefinition rowdefinition_0630 = new RowDefinition();
        RowDefinition rowdefinition_0631 = new RowDefinition();
        RowDefinition rowdefinition_0632 = new RowDefinition();
        RowDefinition rowdefinition_0633 = new RowDefinition();
        RowDefinition rowdefinition_0634 = new RowDefinition();
        RowDefinition rowdefinition_0635 = new RowDefinition();
        RowDefinition rowdefinition_0636 = new RowDefinition();
        RowDefinition rowdefinition_0637 = new RowDefinition();
        RowDefinition rowdefinition_0638 = new RowDefinition();
        RowDefinition rowdefinition_0639 = new RowDefinition();
        RowDefinition rowdefinition_0640 = new RowDefinition();
        RowDefinition rowdefinition_0641 = new RowDefinition();
        RowDefinition rowdefinition_0642 = new RowDefinition();
        Grid grid_0643 = new Grid();
        ColumnDefinition columndefinition_0645 = new ColumnDefinition();
        ColumnDefinition columndefinition_0646 = new ColumnDefinition();
        TextBlock tbStudioSetMasterEqLowFreq = new TextBlock();
        ComboBox cbStudioSetMasterEqLowFreq = new ComboBox();
        Grid grid_0647 = new Grid();
        ColumnDefinition columndefinition_0649 = new ColumnDefinition();
        ColumnDefinition columndefinition_0650 = new ColumnDefinition();
        TextBlock tbStudioSetMasterEqLowGain = new TextBlock();
        Slider slStudioSetMasterEqLowGain = new Slider();
        Grid grid_0651 = new Grid();
        ColumnDefinition columndefinition_0653 = new ColumnDefinition();
        ColumnDefinition columndefinition_0654 = new ColumnDefinition();
        TextBlock tbStudioSetMasterEqMidFreq = new TextBlock();
        ComboBox cbStudioSetMasterEqMidFreq = new ComboBox();
        Grid grid_0655 = new Grid();
        ColumnDefinition columndefinition_0657 = new ColumnDefinition();
        ColumnDefinition columndefinition_0658 = new ColumnDefinition();
        TextBlock tbStudioSetMasterEqMidGain = new TextBlock();
        Slider slStudioSetMasterEqMidGain = new Slider();
        Grid grid_0659 = new Grid();
        ColumnDefinition columndefinition_0661 = new ColumnDefinition();
        ColumnDefinition columndefinition_0662 = new ColumnDefinition();
        TextBlock tbStudioSetMasterEqMidQ = new TextBlock();
        ComboBox cbStudioSetMasterEqMidQ = new ComboBox();
        Grid grid_0663 = new Grid();
        ColumnDefinition columndefinition_0665 = new ColumnDefinition();
        ColumnDefinition columndefinition_0666 = new ColumnDefinition();
        TextBlock tbStudioSetMasterEqHighFreq = new TextBlock();
        ComboBox cbStudioSetMasterEqHighFreq = new ComboBox();
        Grid grid_0667 = new Grid();
        ColumnDefinition columndefinition_0669 = new ColumnDefinition();
        ColumnDefinition columndefinition_0670 = new ColumnDefinition();
        TextBlock tbStudioSetMasterEqHighGain = new TextBlock();
        Slider slStudioSetMasterEqHighGain = new Slider();
        Grid gEditStudioSetSearchResult = new Grid();
        ListView lvSearchResults = new ListView();
        Grid gEditStudioSetColumn2 = new Grid();
        RowDefinition rowdefinition_0673 = new RowDefinition();
        RowDefinition rowdefinition_0674 = new RowDefinition();
        RowDefinition rowdefinition_0675 = new RowDefinition();
        RowDefinition rowdefinition_0676 = new RowDefinition();
        RowDefinition rowdefinition_0677 = new RowDefinition();
        Grid grid_PartSelector = new Grid();
        ComboBox cbStudioSetPartSelector = new ComboBox();
        Grid grid_PartSettings = new Grid();
        ComboBox cbStudioSetPartSubSelector = new ComboBox();
        Grid grid_StudioSetPartSubSelector = new Grid();
        TextBlock StudioSetCurrentToneName = new TextBlock();
        Grid StudioSetPartSettings1 = new Grid();
        RowDefinition rowdefinition_0681 = new RowDefinition();
        RowDefinition rowdefinition_0682 = new RowDefinition();
        RowDefinition rowdefinition_0683 = new RowDefinition();
        RowDefinition rowdefinition_0684 = new RowDefinition();
        RowDefinition rowdefinition_0685 = new RowDefinition();
        RowDefinition rowdefinition_0686 = new RowDefinition();
        RowDefinition rowdefinition_0687 = new RowDefinition();
        RowDefinition rowdefinition_0688 = new RowDefinition();
        RowDefinition rowdefinition_0689 = new RowDefinition();
        RowDefinition rowdefinition_0690 = new RowDefinition();
        RowDefinition rowdefinition_0691 = new RowDefinition();
        RowDefinition rowdefinition_0692 = new RowDefinition();
        RowDefinition rowdefinition_0693 = new RowDefinition();
        RowDefinition rowdefinition_0694 = new RowDefinition();
        Grid grid_0695 = new Grid();
        ColumnDefinition columndefinition_0697 = new ColumnDefinition();
        ColumnDefinition columndefinition_0698 = new ColumnDefinition();
        CheckBox cbStudioSetPartSettings1Receive = new CheckBox();
        ComboBox cbStudioSetPartSettings1ReceiveChannel = new ComboBox();
        Grid grid_0699 = new Grid();
        ColumnDefinition columndefinition_0701 = new ColumnDefinition();
        ColumnDefinition columndefinition_0702 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings1Group = new TextBlock();
        ComboBox cbStudioSetPartSettings1Group = new ComboBox();
        Grid grid_0703 = new Grid();
        ColumnDefinition columndefinition_0705 = new ColumnDefinition();
        ColumnDefinition columndefinition_0706 = new ColumnDefinition();
        ColumnDefinition columndefinition_0707 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings1Category = new TextBlock();
        ComboBox cbStudioSetPartSettings1Category = new ComboBox();
        Grid grid_0708 = new Grid();
        ColumnDefinition columndefinition_0710 = new ColumnDefinition();
        ColumnDefinition columndefinition_0711 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings1Program = new TextBlock();
        ComboBox cbStudioSetPartSettings1Program = new ComboBox();
        Grid grid_0712 = new Grid();
        ColumnDefinition columndefinition_0714 = new ColumnDefinition();
        ColumnDefinition columndefinition_0715 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings1Search = new TextBlock();
        TextBox cbStudioSetPartSettings1Search = new TextBox();
        Grid grid_0716 = new Grid();
        ColumnDefinition columndefinition_0718 = new ColumnDefinition();
        ColumnDefinition columndefinition_0719 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings1Level = new TextBlock();
        Slider slStudioSetPartSettings1Level = new Slider();
        Grid grid_0720 = new Grid();
        ColumnDefinition columndefinition_0722 = new ColumnDefinition();
        ColumnDefinition columndefinition_0723 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings1Pan = new TextBlock();
        Slider slStudioSetPartSettings1Pan = new Slider();
        Grid grid_0724 = new Grid();
        ColumnDefinition columndefinition_0726 = new ColumnDefinition();
        ColumnDefinition columndefinition_0727 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings1CoarseTune = new TextBlock();
        Slider slStudioSetPartSettings1CoarseTune = new Slider();
        Grid grid_0728 = new Grid();
        ColumnDefinition columndefinition_0730 = new ColumnDefinition();
        ColumnDefinition columndefinition_0731 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings1FineTune = new TextBlock();
        Slider slStudioSetPartSettings1FineTune = new Slider();
        Grid grid_0732 = new Grid();
        ColumnDefinition columndefinition_0734 = new ColumnDefinition();
        ColumnDefinition columndefinition_0735 = new ColumnDefinition();
        TextBlock textblock_0736 = new TextBlock();
        ComboBox cbStudioSetPartSettings1MonoPoly = new ComboBox();
        Grid grid_0737 = new Grid();
        ColumnDefinition columndefinition_0739 = new ColumnDefinition();
        ColumnDefinition columndefinition_0740 = new ColumnDefinition();
        TextBlock textblock_0741 = new TextBlock();
        ComboBox cbStudioSetPartSettings1Legato = new ComboBox();
        Grid grid_0742 = new Grid();
        ColumnDefinition columndefinition_0744 = new ColumnDefinition();
        ColumnDefinition columndefinition_0745 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings1PitchBendRange = new TextBlock();
        Slider slStudioSetPartSettings1PitchBendRange = new Slider();
        Grid grid_0746 = new Grid();
        ColumnDefinition columndefinition_0748 = new ColumnDefinition();
        ColumnDefinition columndefinition_0749 = new ColumnDefinition();
        TextBlock textblock_0750 = new TextBlock();
        ComboBox cbStudioSetPartSettings1Portamento = new ComboBox();
        Grid grid_0751 = new Grid();
        ColumnDefinition columndefinition_0753 = new ColumnDefinition();
        ColumnDefinition columndefinition_0754 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings1PortamentoTime = new TextBlock();
        Slider slStudioSetPartSettings1PortamentoTime = new Slider();
        Grid StudioSetPartSettings2 = new Grid();
        RowDefinition rowdefinition_0756 = new RowDefinition();
        RowDefinition rowdefinition_0757 = new RowDefinition();
        RowDefinition rowdefinition_0758 = new RowDefinition();
        RowDefinition rowdefinition_0759 = new RowDefinition();
        RowDefinition rowdefinition_0760 = new RowDefinition();
        RowDefinition rowdefinition_0761 = new RowDefinition();
        RowDefinition rowdefinition_0762 = new RowDefinition();
        RowDefinition rowdefinition_0763 = new RowDefinition();
        RowDefinition rowdefinition_0764 = new RowDefinition();
        RowDefinition rowdefinition_0765 = new RowDefinition();
        RowDefinition rowdefinition_0766 = new RowDefinition();
        RowDefinition rowdefinition_0767 = new RowDefinition();
        RowDefinition rowdefinition_0768 = new RowDefinition();
        RowDefinition rowdefinition_0769 = new RowDefinition();
        RowDefinition rowdefinition_0770 = new RowDefinition();
        RowDefinition rowdefinition_0771 = new RowDefinition();
        RowDefinition rowdefinition_0772 = new RowDefinition();
        Grid grid_0773 = new Grid();
        ColumnDefinition columndefinition_0775 = new ColumnDefinition();
        ColumnDefinition columndefinition_0776 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings2CutoffOffset = new TextBlock();
        Slider slStudioSetPartSettings2CutoffOffset = new Slider();
        Grid grid_0777 = new Grid();
        ColumnDefinition columndefinition_0779 = new ColumnDefinition();
        ColumnDefinition columndefinition_0780 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings2ResonanceOffset = new TextBlock();
        Slider slStudioSetPartSettings2ResonanceOffset = new Slider();
        Grid grid_0781 = new Grid();
        ColumnDefinition columndefinition_0783 = new ColumnDefinition();
        ColumnDefinition columndefinition_0784 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings2AttackTimeOffset = new TextBlock();
        Slider slStudioSetPartSettings2AttackTimeOffset = new Slider();
        Grid grid_0785 = new Grid();
        ColumnDefinition columndefinition_0787 = new ColumnDefinition();
        ColumnDefinition columndefinition_0788 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings2DecayTimeOffset = new TextBlock();
        Slider slStudioSetPartSettings2DecayTimeOffset = new Slider();
        Grid grid_0789 = new Grid();
        ColumnDefinition columndefinition_0791 = new ColumnDefinition();
        ColumnDefinition columndefinition_0792 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings2ReleaseTimeOffset = new TextBlock();
        Slider slStudioSetPartSettings2ReleaseTimeOffset = new Slider();
        Grid grid_0793 = new Grid();
        ColumnDefinition columndefinition_0795 = new ColumnDefinition();
        ColumnDefinition columndefinition_0796 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings2VibratoRate = new TextBlock();
        Slider slStudioSetPartSettings2VibratoRate = new Slider();
        Grid grid_0797 = new Grid();
        ColumnDefinition columndefinition_0799 = new ColumnDefinition();
        ColumnDefinition columndefinition_0800 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings2VibratoDepth = new TextBlock();
        Slider slStudioSetPartSettings2VibratoDepth = new Slider();
        Grid grid_0801 = new Grid();
        ColumnDefinition columndefinition_0803 = new ColumnDefinition();
        ColumnDefinition columndefinition_0804 = new ColumnDefinition();
        TextBlock tbStudioSetPartSettings2VibratoDelay = new TextBlock();
        Slider slStudioSetPartSettings2VibratoDelay = new Slider();
        Grid grid_0805 = new Grid();
        ColumnDefinition columndefinition_0807 = new ColumnDefinition();
        ColumnDefinition columndefinition_0808 = new ColumnDefinition();
        TextBlock tbStudioSetPartEffectsChorusSendLevel = new TextBlock();
        Slider slStudioSetPartEffectsChorusSendLevel = new Slider();
        Grid grid_0809 = new Grid();
        ColumnDefinition columndefinition_0811 = new ColumnDefinition();
        ColumnDefinition columndefinition_0812 = new ColumnDefinition();
        TextBlock tbStudioSetPartEffectsReverbSendLevel = new TextBlock();
        Slider slStudioSetPartEffectsReverbSendLevel = new Slider();
        Grid grid_0813 = new Grid();
        ColumnDefinition columndefinition_0815 = new ColumnDefinition();
        ColumnDefinition columndefinition_0816 = new ColumnDefinition();
        TextBlock textblock_0817 = new TextBlock();
        ComboBox cbStudioSetPartEffectsOutputAssign = new ComboBox();
        Grid StudioSetPartKeyboard = new Grid();
        RowDefinition rowdefinition_0819 = new RowDefinition();
        RowDefinition rowdefinition_0820 = new RowDefinition();
        RowDefinition rowdefinition_0821 = new RowDefinition();
        RowDefinition rowdefinition_0822 = new RowDefinition();
        RowDefinition rowdefinition_0823 = new RowDefinition();
        RowDefinition rowdefinition_0824 = new RowDefinition();
        RowDefinition rowdefinition_0825 = new RowDefinition();
        RowDefinition rowdefinition_0826 = new RowDefinition();
        RowDefinition rowdefinition_0827 = new RowDefinition();
        RowDefinition rowdefinition_0828 = new RowDefinition();
        RowDefinition rowdefinition_0829 = new RowDefinition();
        RowDefinition rowdefinition_0830 = new RowDefinition();
        Grid grid_0831 = new Grid();
        ColumnDefinition columndefinition_0833 = new ColumnDefinition();
        ColumnDefinition columndefinition_0834 = new ColumnDefinition();
        TextBlock tbStudioSetPartKeyboardOctaveShift = new TextBlock();
        Slider slStudioSetPartKeyboardOctaveShift = new Slider();
        Grid grid_0835 = new Grid();
        ColumnDefinition columndefinition_0837 = new ColumnDefinition();
        ColumnDefinition columndefinition_0838 = new ColumnDefinition();
        TextBlock tbStudioSetPartKeyboardVelocitySenseOffset = new TextBlock();
        Slider slStudioSetPartKeyboardVelocitySenseOffset = new Slider();
        Grid grid_0839 = new Grid();
        ColumnDefinition columndefinition_0841 = new ColumnDefinition();
        ColumnDefinition columndefinition_0842 = new ColumnDefinition();
        TextBlock tbStudioSetPartKeyboardRangeLower = new TextBlock();
        Slider slStudioSetPartKeyboardRangeLower = new Slider();
        Grid grid_0843 = new Grid();
        ColumnDefinition columndefinition_0845 = new ColumnDefinition();
        ColumnDefinition columndefinition_0846 = new ColumnDefinition();
        TextBlock tbStudioSetPartKeyboardRangeUpper = new TextBlock();
        Slider slStudioSetPartKeyboardRangeUpper = new Slider();
        Grid grid_0847 = new Grid();
        ColumnDefinition columndefinition_0849 = new ColumnDefinition();
        ColumnDefinition columndefinition_0850 = new ColumnDefinition();
        TextBlock tbStudioSetPartKeyboardFadeWidthLower = new TextBlock();
        Slider slStudioSetPartKeyboardFadeWidthLower = new Slider();
        Grid grid_0851 = new Grid();
        ColumnDefinition columndefinition_0853 = new ColumnDefinition();
        ColumnDefinition columndefinition_0854 = new ColumnDefinition();
        TextBlock tbStudioSetPartKeyboardFadeWidthUpper = new TextBlock();
        Slider slStudioSetPartKeyboardFadeWidthUpper = new Slider();
        Grid grid_0855 = new Grid();
        ColumnDefinition columndefinition_0857 = new ColumnDefinition();
        ColumnDefinition columndefinition_0858 = new ColumnDefinition();
        TextBlock tbStudioSetPartKeyboardVelocityRangeLower = new TextBlock();
        Slider slStudioSetPartKeyboardVelocityRangeLower = new Slider();
        Grid grid_0859 = new Grid();
        ColumnDefinition columndefinition_0861 = new ColumnDefinition();
        ColumnDefinition columndefinition_0862 = new ColumnDefinition();
        TextBlock tbStudioSetPartKeyboardVelocityRangeUpper = new TextBlock();
        Slider slStudioSetPartKeyboardVelocityRangeUpper = new Slider();
        Grid grid_0863 = new Grid();
        ColumnDefinition columndefinition_0865 = new ColumnDefinition();
        ColumnDefinition columndefinition_0866 = new ColumnDefinition();
        TextBlock tbStudioSetPartKeyboardVelocityFadeWidthLower = new TextBlock();
        Slider slStudioSetPartKeyboardVelocityFadeWidthLower = new Slider();
        Grid grid_0867 = new Grid();
        ColumnDefinition columndefinition_0869 = new ColumnDefinition();
        ColumnDefinition columndefinition_0870 = new ColumnDefinition();
        TextBlock tbStudioSetPartKeyboardVelocityFadeWidthUpper = new TextBlock();
        Slider slStudioSetPartKeyboardVelocityFadeWidthUpper = new Slider();
        Grid grid_0871 = new Grid();
        CheckBox cbStudioSetPartKeyboardMute = new CheckBox();
        Grid grid_0872 = new Grid();
        ColumnDefinition columndefinition_0874 = new ColumnDefinition();
        ColumnDefinition columndefinition_0875 = new ColumnDefinition();
        TextBlock textblock_0876 = new TextBlock();
        ComboBox cbStudioSetPartKeyboardVelocityCurveType = new ComboBox();
        Grid StudioSetPartScaleTune = new Grid();
        RowDefinition rowdefinition_0878 = new RowDefinition();
        RowDefinition rowdefinition_0879 = new RowDefinition();
        RowDefinition rowdefinition_0880 = new RowDefinition();
        RowDefinition rowdefinition_0881 = new RowDefinition();
        RowDefinition rowdefinition_0882 = new RowDefinition();
        RowDefinition rowdefinition_0883 = new RowDefinition();
        RowDefinition rowdefinition_0884 = new RowDefinition();
        RowDefinition rowdefinition_0885 = new RowDefinition();
        RowDefinition rowdefinition_0886 = new RowDefinition();
        RowDefinition rowdefinition_0887 = new RowDefinition();
        RowDefinition rowdefinition_0888 = new RowDefinition();
        RowDefinition rowdefinition_0889 = new RowDefinition();
        RowDefinition rowdefinition_0890 = new RowDefinition();
        RowDefinition rowdefinition_0891 = new RowDefinition();
        Grid grid_0892 = new Grid();
        ColumnDefinition columndefinition_0894 = new ColumnDefinition();
        ColumnDefinition columndefinition_0895 = new ColumnDefinition();
        TextBlock textblock_0896 = new TextBlock();
        ComboBox cbStudioSetPartScaleTuneType = new ComboBox();
        Grid grid_0897 = new Grid();
        ColumnDefinition columndefinition_0899 = new ColumnDefinition();
        ColumnDefinition columndefinition_0900 = new ColumnDefinition();
        TextBlock textblock_0901 = new TextBlock();
        ComboBox cbStudioSetPartScaleTuneKey = new ComboBox();
        Grid grid_0902 = new Grid();
        ColumnDefinition columndefinition_0904 = new ColumnDefinition();
        ColumnDefinition columndefinition_0905 = new ColumnDefinition();
        TextBlock tbStudioSetPartScaleTuneC = new TextBlock();
        Slider slStudioSetPartScaleTuneC = new Slider();
        Grid grid_0906 = new Grid();
        ColumnDefinition columndefinition_0908 = new ColumnDefinition();
        ColumnDefinition columndefinition_0909 = new ColumnDefinition();
        TextBlock tbStudioSetPartScaleTuneCi = new TextBlock();
        Slider slStudioSetPartScaleTuneCi = new Slider();
        Grid grid_0910 = new Grid();
        ColumnDefinition columndefinition_0912 = new ColumnDefinition();
        ColumnDefinition columndefinition_0913 = new ColumnDefinition();
        TextBlock tbStudioSetPartScaleTuneD = new TextBlock();
        Slider slStudioSetPartScaleTuneD = new Slider();
        Grid grid_0914 = new Grid();
        ColumnDefinition columndefinition_0916 = new ColumnDefinition();
        ColumnDefinition columndefinition_0917 = new ColumnDefinition();
        TextBlock tbStudioSetPartScaleTuneDi = new TextBlock();
        Slider slStudioSetPartScaleTuneDi = new Slider();
        Grid grid_0918 = new Grid();
        ColumnDefinition columndefinition_0920 = new ColumnDefinition();
        ColumnDefinition columndefinition_0921 = new ColumnDefinition();
        TextBlock tbStudioSetPartScaleTuneE = new TextBlock();
        Slider slStudioSetPartScaleTuneE = new Slider();
        Grid grid_0922 = new Grid();
        ColumnDefinition columndefinition_0924 = new ColumnDefinition();
        ColumnDefinition columndefinition_0925 = new ColumnDefinition();
        TextBlock tbStudioSetPartScaleTuneF = new TextBlock();
        Slider slStudioSetPartScaleTuneF = new Slider();
        Grid grid_0926 = new Grid();
        ColumnDefinition columndefinition_0928 = new ColumnDefinition();
        ColumnDefinition columndefinition_0929 = new ColumnDefinition();
        TextBlock tbStudioSetPartScaleTuneFi = new TextBlock();
        Slider slStudioSetPartScaleTuneFi = new Slider();
        Grid grid_0930 = new Grid();
        ColumnDefinition columndefinition_0932 = new ColumnDefinition();
        ColumnDefinition columndefinition_0933 = new ColumnDefinition();
        TextBlock tbStudioSetPartScaleTuneG = new TextBlock();
        Slider slStudioSetPartScaleTuneG = new Slider();
        Grid grid_0934 = new Grid();
        ColumnDefinition columndefinition_0936 = new ColumnDefinition();
        ColumnDefinition columndefinition_0937 = new ColumnDefinition();
        TextBlock tbStudioSetPartScaleTuneGi = new TextBlock();
        Slider slStudioSetPartScaleTuneGi = new Slider();
        Grid grid_0938 = new Grid();
        ColumnDefinition columndefinition_0940 = new ColumnDefinition();
        ColumnDefinition columndefinition_0941 = new ColumnDefinition();
        TextBlock tbStudioSetPartScaleTuneA = new TextBlock();
        Slider slStudioSetPartScaleTuneA = new Slider();
        Grid grid_0942 = new Grid();
        ColumnDefinition columndefinition_0944 = new ColumnDefinition();
        ColumnDefinition columndefinition_0945 = new ColumnDefinition();
        TextBlock tbStudioSetPartScaleTuneAi = new TextBlock();
        Slider slStudioSetPartScaleTuneAi = new Slider();
        Grid grid_0946 = new Grid();
        ColumnDefinition columndefinition_0948 = new ColumnDefinition();
        ColumnDefinition columndefinition_0949 = new ColumnDefinition();
        TextBlock tbStudioSetPartScaleTuneB = new TextBlock();
        Slider slStudioSetPartScaleTuneB = new Slider();
        Grid StudioSetPartMidi = new Grid();
        RowDefinition rowdefinition_0951 = new RowDefinition();
        RowDefinition rowdefinition_0952 = new RowDefinition();
        RowDefinition rowdefinition_0953 = new RowDefinition();
        RowDefinition rowdefinition_0954 = new RowDefinition();
        RowDefinition rowdefinition_0955 = new RowDefinition();
        RowDefinition rowdefinition_0956 = new RowDefinition();
        RowDefinition rowdefinition_0957 = new RowDefinition();
        RowDefinition rowdefinition_0958 = new RowDefinition();
        RowDefinition rowdefinition_0959 = new RowDefinition();
        RowDefinition rowdefinition_0960 = new RowDefinition();
        RowDefinition rowdefinition_0961 = new RowDefinition();
        Grid grid_0962 = new Grid();
        CheckBox cbStudioSetPartMidiPhaseLock = new CheckBox();
        Grid grid_0963 = new Grid();
        CheckBox cbStudioSetPartMidiReceiveProgramChange = new CheckBox();
        Grid grid_0964 = new Grid();
        CheckBox cbStudioSetPartMidiReceiveBankSelect = new CheckBox();
        Grid grid_0965 = new Grid();
        CheckBox cbStudioSetPartMidiReceivePitchBend = new CheckBox();
        Grid grid_0966 = new Grid();
        CheckBox cbStudioSetPartMidiReceivePolyphonicKeyPressure = new CheckBox();
        Grid grid_0967 = new Grid();
        CheckBox cbStudioSetPartMidiReceiveChannelPressure = new CheckBox();
        Grid grid_0968 = new Grid();
        CheckBox cbStudioSetPartMidiReceiveModulation = new CheckBox();
        Grid grid_0969 = new Grid();
        CheckBox cbStudioSetPartMidiReceiveVolume = new CheckBox();
        Grid grid_0970 = new Grid();
        CheckBox cbStudioSetPartMidiReceivePan = new CheckBox();
        Grid grid_0971 = new Grid();
        CheckBox cbStudioSetPartMidiReceiveExpression = new CheckBox();
        Grid grid_0972 = new Grid();
        CheckBox cbStudioSetPartMidiReceiveHold1 = new CheckBox();
        Grid StudioSetPartMotionalSurround = new Grid();
        RowDefinition rowdefinition_0974 = new RowDefinition();
        RowDefinition rowdefinition_0975 = new RowDefinition();
        RowDefinition rowdefinition_0976 = new RowDefinition();
        RowDefinition rowdefinition_0977 = new RowDefinition();
        RowDefinition rowdefinition_0978 = new RowDefinition();
        Grid grid_0979 = new Grid();
        ColumnDefinition columndefinition_0981 = new ColumnDefinition();
        ColumnDefinition columndefinition_0982 = new ColumnDefinition();
        TextBlock tbStudioSetPartMotionalSurroundLR = new TextBlock();
        Slider slStudioSetPartMotionalSurroundLR = new Slider();
        Grid grid_0983 = new Grid();
        ColumnDefinition columndefinition_0985 = new ColumnDefinition();
        ColumnDefinition columndefinition_0986 = new ColumnDefinition();
        TextBlock tbStudioSetPartMotionalSurroundFB = new TextBlock();
        Slider slStudioSetPartMotionalSurroundFB = new Slider();
        Grid grid_0987 = new Grid();
        ColumnDefinition columndefinition_0989 = new ColumnDefinition();
        ColumnDefinition columndefinition_0990 = new ColumnDefinition();
        TextBlock tbStudioSetPartMotionalSurroundWidth = new TextBlock();
        Slider slStudioSetPartMotionalSurroundWidth = new Slider();
        Grid grid_0991 = new Grid();
        ColumnDefinition columndefinition_0993 = new ColumnDefinition();
        ColumnDefinition columndefinition_0994 = new ColumnDefinition();
        TextBlock tbStudioSetPartMotionalSurroundAmbienceSendLevel = new TextBlock();
        Slider slStudioSetPartMotionalSurroundAmbienceSendLevel = new Slider();
        Grid StudioSetPartEQ = new Grid();
        RowDefinition rowdefinition_0996 = new RowDefinition();
        RowDefinition rowdefinition_0997 = new RowDefinition();
        RowDefinition rowdefinition_0998 = new RowDefinition();
        RowDefinition rowdefinition_0999 = new RowDefinition();
        RowDefinition rowdefinition_1000 = new RowDefinition();
        RowDefinition rowdefinition_1001 = new RowDefinition();
        RowDefinition rowdefinition_1002 = new RowDefinition();
        RowDefinition rowdefinition_1003 = new RowDefinition();
        RowDefinition rowdefinition_1004 = new RowDefinition();
        Grid grid_1005 = new Grid();
        CheckBox cbStudioSetPartEQSwitch = new CheckBox();
        Grid grid_1006 = new Grid();
        ColumnDefinition columndefinition_1008 = new ColumnDefinition();
        ColumnDefinition columndefinition_1009 = new ColumnDefinition();
        TextBlock textblock_1010 = new TextBlock();
        ComboBox cbStudioSetPartEQLoqFreq = new ComboBox();
        Grid grid_1011 = new Grid();
        ColumnDefinition columndefinition_1013 = new ColumnDefinition();
        ColumnDefinition columndefinition_1014 = new ColumnDefinition();
        TextBlock tbStudioSetPartEQLowGain = new TextBlock();
        Slider slStudioSetPartEQLowGain = new Slider();
        Grid grid_1015 = new Grid();
        ColumnDefinition columndefinition_1017 = new ColumnDefinition();
        ColumnDefinition columndefinition_1018 = new ColumnDefinition();
        TextBlock tbStudioSetPartEQMidFreq = new TextBlock();
        ComboBox cbStudioSetPartEQMidFreq = new ComboBox();
        Grid grid_1019 = new Grid();
        ColumnDefinition columndefinition_1021 = new ColumnDefinition();
        ColumnDefinition columndefinition_1022 = new ColumnDefinition();
        TextBlock tbStudioSetPartEQMidGain = new TextBlock();
        Slider slStudioSetPartEQMidGain = new Slider();
        Grid grid_1023 = new Grid();
        ColumnDefinition columndefinition_1025 = new ColumnDefinition();
        ColumnDefinition columndefinition_1026 = new ColumnDefinition();
        TextBlock tbStudioSetPartEQMidQ = new TextBlock();
        ComboBox cbStudioSetPartEQMidQ = new ComboBox();
        Grid grid_1027 = new Grid();
        ColumnDefinition columndefinition_1029 = new ColumnDefinition();
        ColumnDefinition columndefinition_1030 = new ColumnDefinition();
        TextBlock tbStudioSetPartEQHighFreq = new TextBlock();
        ComboBox cbStudioSetPartEQHighFreq = new ComboBox();
        Grid grid_1031 = new Grid();
        ColumnDefinition columndefinition_1033 = new ColumnDefinition();
        ColumnDefinition columndefinition_1034 = new ColumnDefinition();
        TextBlock tbStudioSetPartEQHighGain = new TextBlock();
        Slider slStudioSetPartEQHighGain = new Slider();
        Grid Buttons = new Grid();
        RowDefinition rowdefinition_1036 = new RowDefinition();
        RowDefinition rowdefinition_1037 = new RowDefinition();
        Grid grid_1038 = new Grid();
        ColumnDefinition columndefinition_1040 = new ColumnDefinition();
        ColumnDefinition columndefinition_1041 = new ColumnDefinition();
        ColumnDefinition columndefinition_1042 = new ColumnDefinition();
        ComboBox cbStudioSetSlot = new ComboBox();
        TextBlock textblock_1043 = new TextBlock();
        TextBox tbStudioSetName = new TextBox();
        Grid grid_Buttons = new Grid();
        ColumnDefinition columndefinition_1046 = new ColumnDefinition();
        ColumnDefinition columndefinition_1047 = new ColumnDefinition();
        ColumnDefinition columndefinition_1048 = new ColumnDefinition();
        ColumnDefinition columndefinition_1049 = new ColumnDefinition();
        ColumnDefinition columndefinition_1050 = new ColumnDefinition();
        Button btnFileSave = new Button();
        Button btnFileLoad = new Button();
        Button btnStudioSetSave = new Button();
        Button btnStudioSetDelete = new Button();
        Button btnStudioSetReturn = new Button();

        public void ShowStudioSetEditorPage()
        {
            page = _page.EDIT_STUDIO_SET;
            if (!EditStudioSet_IsCreated)
            {
                initDone = false;
                handleControlEvents = false;
                DrawStudioSetEditorPage();
                mainStackLayout.Children.Add(StudioSetEditor_StackLayout);
                EditStudioSet_IsCreated = true;
                handleControlEvents = true;
                commonState.studioSet = new StudioSet();
                StudioSetEditor_Init();
                StudioSetEditor_StartTimer();
                PopulateComboBoxes();
            }
            //ShowToneTypeDependentControls();
            StudioSetEditor_StackLayout.IsVisible = true;
        }

        public void DrawStudioSetEditorPage()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Edit studio set
            // ____________________________________________________________________________________________
            // |                                                                                          |
            // |__________________________________________________________________________________________|

            // Create all controls ------------------------------------------------------------------------



            // Set properties -----------------------------------------------------------------------------

            ////grid_0001.Background = "{ThemeResource ApplicationPageBackgroundThemeBrush}";
            //grid_PleaseWaitWhileScanning.IsVisible = false;
            ////Progress.Name = "Progress";
            //Progress.Margin = new Thickness(10, 25, 10, 0);
            ////Progress.IsIndeterminate = true;
            //textblock_0002.Text = "Please wait while reading Studio Set names and settings from your INTEGRA-7...";
            //grid_MainStudioSet.IsVisible = true;
            //grid_StudioSet_Column0.Margin = new Thickness(2, 2, 2, 2);
            ////grid_0007.BorderThickness = new Thickness(1);
            //rowdefinition_0009.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0010.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0011.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0012.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0013.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0014.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0015.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0016.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0017.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0018.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0019.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0020.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0021.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0022.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0023.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0024.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0025.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0026.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0027.Height = new GridLength(1, GridUnitType.Star);
            cbStudioSetSelector.Margin = new Thickness(2, 2, 2, 2);
            cbStudioSetSelector.SelectedIndexChanged += cbStudioSetSelector_SelectionChanged;
            grid_ToneControl1.Margin = new Thickness(2, 0, 2, 2);
            //grid_0029.BorderThickness = new Thickness(1);
            grid_ToneControl1.Padding = new Thickness(4, 0, 0, 0);
            textblock_ToneControl1.Text = "Tone control 1 source";
            cbToneControl1.SelectedIndex = 0;
            cbToneControl1.SelectedIndexChanged += cbToneControl1_SelectionChanged;
            grid_ToneControl2.Margin = new Thickness(2, 0, 2, 2);
            //grid_0035.BorderThickness = new Thickness(1);
            grid_ToneControl2.Padding = new Thickness(4, 0, 0, 0);
            textblock_ToneControl2.Text = "Tone control 2 source";
            cbToneControl2.SelectedIndex = 0;
            cbToneControl2.SelectedIndexChanged += cbToneControl2_SelectionChanged;
            grid_ToneControl3.Margin = new Thickness(2, 0, 2, 2);
            //grid_0041.BorderThickness = new Thickness(1);
            grid_ToneControl3.Padding = new Thickness(4, 0, 0, 0);
            textblock_ToneControl3.Text = "Tone control 3 source";
            cbToneControl3.SelectedIndex = 0;
            cbToneControl3.SelectedIndexChanged += cbToneControl3_SelectionChanged;
            grid_ToneControl4.Margin = new Thickness(2, 0, 2, 2);
            //grid_0047.BorderThickness = new Thickness(1);
            grid_ToneControl4.Padding = new Thickness(4, 0, 0, 0);
            textblock_ToneControl4.Text = "Tone control 4 source";
            cbToneControl4.SelectedIndex = 0;
            cbToneControl4.SelectedIndexChanged += cbToneControl4_SelectionChanged;
            grid_Tempo.Margin = new Thickness(2, 0, 2, 2);
            //grid_0053.BorderThickness = new Thickness(1);
            grid_Tempo.Padding = new Thickness(4, 0, 0, 0);
            columndefinition_0056.Width = new GridLength(1, GridUnitType.Star);
            columndefinition_0057.Width = new GridLength(1, GridUnitType.Star);
            tbTempo.Text = "Tempo";
            //slTempo.Padding = new Thickness(2, 0, 2, 0);
            slTempo.Maximum = 250;
            slTempo.Minimum = 20;
            slTempo.ValueChanged += slTempo_ValueChanged;
            grid_SoloPart.Margin = new Thickness(2, 0, 2, 2);
            //grid_0058.BorderThickness = new Thickness(1);
            grid_SoloPart.Padding = new Thickness(4, 0, 0, 0);
            columndefinition_0061.Width = new GridLength(2, GridUnitType.Star);
            columndefinition_0062.Width = new GridLength(1, GridUnitType.Star);
            tbSoloPart.Text = "Solo part";
            cbSoloPart.SelectedIndex = 0;
            cbSoloPart.SelectedIndexChanged += cbSoloPart_SelectionChanged;
            //grid_0064.Margin = new Thickness(2, 0, 2, 2);
            ////grid_0064.BorderThickness = new Thickness(1);
            //grid_0065.Padding = new Thickness(4, 0, 0, 0);
            //columndefinition_0067.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0068.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0069.Width = new GridLength(1, GridUnitType.Star);
            //cbReverb.Content = "Reverb";
            //cbReverb.Toggled += cbReverb_Click;
            //cbChorus.Content = "Chorus";
            //cbChorus.Toggled += cbChorus_Click;
            //cbMasterEQ.Content = "Master EQ";
            //cbMasterEQ.Toggled += cbMasterEQ_Click;
            //tbReverb.Text = "Reverb";
            //tbChorus.Text = "Chorus";
            //tbMasterEQ.Text = "Master EQ";
            //grid_0070.Margin = new Thickness(2, 0, 2, 2);
            ////grid_0070.BorderThickness = new Thickness(1);
            //grid_0071.Padding = new Thickness(4, 0, 0, 0);
            //columndefinition_0073.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0074.Width = new GridLength(1, GridUnitType.Star);
            tbDrumCompEQPart.Text = "Drum Compression/Equalizer";
            //cbDrumCompEQPart.SelectedIndex = 0;
            //cbDrumCompEQPart.SelectedIndexChanged += cbDrumCompEQPart_SelectionChanged;
            //grid_0076.Margin = new Thickness(2, 0, 2, 2);
            ////grid_0076.BorderThickness = new Thickness(1);
            //grid_0077.Padding = new Thickness(4, 0, 0, 0);
            //columndefinition_0079.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0080.Width = new GridLength(1, GridUnitType.Star);
            textblock_0081.Text = "Drum Comp/EQ 1 Output assign";
            //cbDrumCompEQ1OutputAssign.SelectedIndex = 0;
            //cbDrumCompEQ1OutputAssign.SelectedIndexChanged += cbDrumCompEQ1OutputAssign_SelectionChanged;
            //grid_0082.Margin = new Thickness(2, 0, 2, 2);
            ////grid_0082.BorderThickness = new Thickness(1);
            //grid_0083.Padding = new Thickness(4, 0, 0, 0);
            //columndefinition_0085.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0086.Width = new GridLength(1, GridUnitType.Star);
            textblock_0087.Text = "Drum Comp/EQ 2 Output assign";
            //cbDrumCompEQ2OutputAssign.SelectedIndex = 0;
            //cbDrumCompEQ2OutputAssign.SelectedIndexChanged += cbDrumCompEQ2OutputAssign_SelectionChanged;
            //grid_0088.Margin = new Thickness(2, 0, 2, 2);
            ////grid_0088.BorderThickness = new Thickness(1);
            //grid_0089.Padding = new Thickness(4, 0, 0, 0);
            //columndefinition_0091.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0092.Width = new GridLength(1, GridUnitType.Star);
            textblock_0093.Text = "Drum Comp/EQ 3 Output assign";
            //cbDrumCompEQ3OutputAssign.SelectedIndex = 0;
            //cbDrumCompEQ3OutputAssign.SelectedIndexChanged += cbDrumCompEQ3OutputAssign_SelectionChanged;
            //grid_0094.Margin = new Thickness(2, 0, 2, 2);
            ////grid_0094.BorderThickness = new Thickness(1);
            //grid_0095.Padding = new Thickness(4, 0, 0, 0);
            //columndefinition_0097.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0098.Width = new GridLength(1, GridUnitType.Star);
            textblock_0099.Text = "Drum Comp/EQ 4 Output assign";
            //cbDrumCompEQ4OutputAssign.SelectedIndex = 0;
            //cbDrumCompEQ4OutputAssign.SelectedIndexChanged += cbDrumCompEQ4OutputAssign_SelectionChanged;
            //grid_0100.Margin = new Thickness(2, 0, 2, 2);
            ////grid_0100.BorderThickness = new Thickness(1);
            //grid_0101.Padding = new Thickness(4, 0, 0, 0);
            //columndefinition_0103.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0104.Width = new GridLength(1, GridUnitType.Star);
            textblock_0105.Text = "Drum Comp/EQ 5 Output assign";
            //cbDrumCompEQ5OutputAssign.SelectedIndex = 0;
            //cbDrumCompEQ5OutputAssign.SelectedIndexChanged += cbDrumCompEQ5OutputAssign_SelectionChanged;
            //grid_0106.Margin = new Thickness(2, 0, 2, 2);
            ////grid_0106.BorderThickness = new Thickness(1);
            //grid_0107.Padding = new Thickness(4, 0, 0, 0);
            //columndefinition_0109.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0110.Width = new GridLength(1, GridUnitType.Star);
            textblock_0111.Text = "Drum Comp/EQ 6 Output assign";
            //cbDrumCompEQ6OutputAssign.SelectedIndex = 0;
            //cbDrumCompEQ6OutputAssign.SelectedIndexChanged += cbDrumCompEQ6OutputAssign_SelectionChanged;
            //grid_0112.Margin = new Thickness(2, 0, 2, 2);
            //grid_0112.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0112.BorderThickness = new Thickness(1);
            //columndefinition_0114.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0115.Width = new GridLength(1, GridUnitType.Star);
            ////cbDrumCompEQ.Padding = new Thickness(4, 5, 0, 0);
            //cbDrumCompEQ.Content = "Drum Comp/Equalizer";
            //cbDrumCompEQ.Toggled += cbDrumCompEQ_Click;
            ////cbExtPartMute.Padding = new Thickness(4, 5, 0, 0);
            //cbExtPartMute.Content = "Ext mute";
            //cbExtPartMute.Toggled += cbExtPartMute_Click;
            //grid_0116.Margin = new Thickness(2, 0, 2, 2);
            //grid_0116.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0116.BorderThickness = new Thickness(1);
            //columndefinition_0118.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0119.Width = new GridLength(1, GridUnitType.Star);
            ////tbExtPartLevel.Padding = new Thickness(4, 0, 0, 0);
            tbExtPartLevel.Text = "Ext Part Level";
            ////slExtPartLevel.Padding = new Thickness(4, 0, 0, 0);
            //slExtPartLevel.Minimum = 0;
            //slExtPartLevel.Maximum = 127;
            //slExtPartLevel.ValueChanged += slExtPartLevel_ValueChanged;
            //grid_0120.Margin = new Thickness(2, 0, 2, 2);
            //grid_0120.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0120.BorderThickness = new Thickness(1);
            //columndefinition_0122.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0123.Width = new GridLength(1, GridUnitType.Star);
            ////tbExtPartChorusSend.Padding = new Thickness(4, 0, 0, 0);
            tbExtPartChorusSend.Text = "Ext part chorus level";
            ////slExtPartChorusSend.Padding = new Thickness(4, 0, 0, 0);
            //slExtPartChorusSend.Minimum = 0;
            //slExtPartChorusSend.Maximum = 127;
            //slExtPartChorusSend.ValueChanged += slExtPartChorusSend_ValueChanged;
            //grid_0124.Margin = new Thickness(2, 0, 2, 2);
            //grid_0124.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0124.BorderThickness = new Thickness(1);
            //columndefinition_0126.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0127.Width = new GridLength(1, GridUnitType.Star);
            ////tbExtPartReverbSend.Padding = new Thickness(4, 0, 0, 0);
            tbExtPartReverbSend.Text = "Ext part reverb level";
            ////slExtPartReverbSend.Padding = new Thickness(4, 0, 0, 0);
            //slExtPartReverbSend.Minimum = 0;
            //slExtPartReverbSend.Maximum = 127;
            //slExtPartReverbSend.ValueChanged += slExtPartReverbSend_ValueChanged;
            //gEditStudioSetColumn1.Margin = new Thickness(0, 2, 0, 2);
            ////gEditStudioSetColumn1.BorderThickness = new Thickness(1);
            //rowdefinition_0129.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0130.Height = new GridLength(17, GridUnitType.Star);
            //cbColumn2Selector.Margin = new Thickness(2, 2, 2, 2);
            //cbColumn2Selector.SelectedIndexChanged += cbColumn2Selector_SelectionChanged;
            //cbColumn2Selector.SelectedIndex = 0;
            //SystemCommonSettings.Margin = new Thickness(2, 0, 2, 2);
            ////SystemCommonSettings.BorderThickness = new Thickness(1);
            //SystemCommonSettings.IsVisible = true;
            //grid_0151.Margin = new Thickness(2, 2, 2, 2);
            //grid_0151.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0151.BorderThickness = new Thickness(1);
            //columndefinition_0153.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0154.Width = new GridLength(1, GridUnitType.Star);
            //tbSystemCommonMasterTune.Text = "Master tune 0 cent";
            //slSystemCommonMasterTune.Minimum = -1000;
            //slSystemCommonMasterTune.Maximum = 1000;
            //slSystemCommonMasterTune.ValueChanged += slSystemCommonMasterTune_ValueChanged;
            //grid_0155.Margin = new Thickness(2, 0, 2, 2);
            //grid_0155.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0155.BorderThickness = new Thickness(1);
            //columndefinition_0157.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0158.Width = new GridLength(1, GridUnitType.Star);
            //tbSystemCommonMasterKeyShift.Text = "Master Key Shift";
            //slSystemCommonMasterKeyShift.Minimum = -24;
            //slSystemCommonMasterKeyShift.Maximum = 24;
            //slSystemCommonMasterKeyShift.ValueChanged += slSystemCommonMasterKeyShift_ValueChanged;
            //grid_0159.Margin = new Thickness(2, 0, 2, 2);
            //grid_0159.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0159.BorderThickness = new Thickness(1);
            //columndefinition_0161.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0162.Width = new GridLength(1, GridUnitType.Star);
            //tbSystemCommonMasterLevel.Text = "Master level";
            //slSystemCommonMasterLevel.Minimum = 0;
            //slSystemCommonMasterLevel.Maximum = 127;
            //slSystemCommonMasterLevel.ValueChanged += slSystemCommonMasterLevel_ValueChanged;
            //grid_0163.Margin = new Thickness(2, 0, 2, 2);
            //grid_0163.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0163.BorderThickness = new Thickness(1);
            ////cbSystemCommonScaleTuneSwitch.Padding = new Thickness(4, 5, 0, 0);
            //cbSystemCommonScaleTuneSwitch.Content = "Use scale tune (as set in parts)";
            //cbSystemCommonScaleTuneSwitch.Toggled += cbSystemCommonScaleTuneSwitch_Click;
            //grid_0164.Margin = new Thickness(2, 0, 2, 2);
            //grid_0164.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0164.BorderThickness = new Thickness(1);
            //columndefinition_0166.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0167.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0168.Text = "Studio set control channel";
            //cbSystemCommonStudioSetControlChannel.SelectedIndexChanged += cbSystemCommonStudioSetControlChannel_SelectionChanged;
            //cbSystemCommonStudioSetControlChannel.IsEnabled = true;
            //grid_0169.Margin = new Thickness(2, 0, 2, 2);
            //grid_0169.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0169.BorderThickness = new Thickness(1);
            //columndefinition_0171.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0172.Width = new GridLength(1, GridUnitType.Star);
            //tbSystemCommonSystemControlSource1.Text = "System control source 1";
            //cbSystemCommonSystemControlSource1.SelectedIndexChanged += cbSystemCommonSystemControlSource1_SelectedIndexChanged;
            //grid_0173.Margin = new Thickness(2, 0, 2, 2);
            //grid_0173.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0173.BorderThickness = new Thickness(1);
            //columndefinition_0175.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0176.Width = new GridLength(1, GridUnitType.Star);
            //tbSystemCommonSystemControlSource2.Text = "System control source 2";
            //cbSystemCommonSystemControlSource2.SelectedIndexChanged += cbSystemCommonSystemControlSource2_SelectedIndexChanged;
            //grid_0177.Margin = new Thickness(2, 0, 2, 2);
            //grid_0177.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0177.BorderThickness = new Thickness(1);
            //columndefinition_0179.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0180.Width = new GridLength(1, GridUnitType.Star);
            //tbSystemCommonSystemControlSource3.Text = "System control source 3";
            //cbSystemCommonSystemControlSource3.SelectedIndexChanged += cbSystemCommonSystemControlSource3_SelectedIndexChanged;
            //grid_0181.Margin = new Thickness(2, 0, 2, 2);
            //grid_0181.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0181.BorderThickness = new Thickness(1);
            //columndefinition_0183.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0184.Width = new GridLength(1, GridUnitType.Star);
            //tbSystemCommonSystemControlSource4.Text = "System control source 4";
            //cbSystemCommonSystemControlSource4.SelectedIndexChanged += cbSystemCommonSystemControlSource4_SelectedIndexChanged;
            //grid_0185.Margin = new Thickness(2, 0, 2, 2);
            //grid_0185.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0185.BorderThickness = new Thickness(1);
            //columndefinition_0187.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0188.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0189.Text = "Control source";
            //cbSystemCommonControlSource.SelectedIndexChanged += cbSystemCommonControlSource_SelectionChanged;
            //grid_0190.Margin = new Thickness(2, 0, 2, 2);
            //grid_0190.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0190.BorderThickness = new Thickness(1);
            //columndefinition_0192.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0193.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0194.Text = "System clock source";
            //cbSystemCommonSystemClockSource.SelectedIndexChanged += cbSystemCommonSystemClockSource_SelectionChanged;
            //grid_0195.Margin = new Thickness(2, 0, 2, 2);
            //grid_0195.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0195.BorderThickness = new Thickness(1);
            //columndefinition_0197.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0198.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0199.Text = "System tempo";
            //slSystemCommonSystemTempo.Maximum = 250;
            //slSystemCommonSystemTempo.Minimum = 20;
            //slSystemCommonSystemTempo.ValueChanged += slSystemCommonSystemTempo_ValueChanged;
            //grid_0200.Margin = new Thickness(2, 0, 2, 2);
            //grid_0200.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0200.BorderThickness = new Thickness(1);
            //columndefinition_0202.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0203.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0204.Text = "Tempo assign source";
            //cbSystemCommonTempoAssignSource.SelectedIndexChanged += cbSystemCommonTempoAssignSource_SelectionChanged;
            //grid_0205.Margin = new Thickness(2, 0, 2, 2);
            //grid_0205.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0205.BorderThickness = new Thickness(1);
            ////cbSystemCommonReceiveProgramChange.Padding = new Thickness(4, 5, 0, 0);
            //cbSystemCommonReceiveProgramChange.Content = "Receive program change";
            //cbSystemCommonReceiveProgramChange.Toggled += cbSystemCommonReceiveProgramChange_Click;
            //grid_0206.Margin = new Thickness(2, 0, 2, 2);
            ////grid_0206.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0206.BorderThickness = new Thickness(1);
            ////cbSystemCommonReceiveBankSelect.Padding = new Thickness(4, 5, 0, 0);
            //cbSystemCommonReceiveBankSelect.Content = "Receive bank select";
            //cbSystemCommonReceiveBankSelect.Toggled += cbSystemCommonReceiveBankSelect_Click;
            //grid_0207.Margin = new Thickness(2, 0, 2, 2);
            //grid_0207.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0207.BorderThickness = new Thickness(1);
            ////cbSystemCommonSurroundCenterSpeakerSwitch.Padding = new Thickness(4, 5, 0, 0);
            //cbSystemCommonSurroundCenterSpeakerSwitch.Content = "5.1Ch center speaker";
            //cbSystemCommonSurroundCenterSpeakerSwitch.Toggled += cbSystemCommonSurroundCenterSpeakerSwitch_Click;
            //grid_0208.Margin = new Thickness(2, 0, 2, 2);
            //grid_0208.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0208.BorderThickness = new Thickness(1);
            ////cbSystemCommonSurroundSubWooferSwitch.Padding = new Thickness(4, 5, 0, 0);
            //cbSystemCommonSurroundSubWooferSwitch.Content = "5.1Ch sub woofer";
            //cbSystemCommonSurroundSubWooferSwitch.Toggled += cbSystemCommonSurroundSubWooferSwitch_Click;
            //grid_0209.Margin = new Thickness(2, 0, 2, 2);
            //grid_0209.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0209.BorderThickness = new Thickness(1);
            //columndefinition_0211.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0212.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0213.Text = "Output mode";
            //cbSystemCommonStereoOutputMode.SelectedIndexChanged += cbSystemCommonStereoOutputMode_SelectionChanged;
            //VoiceReserve.Margin = new Thickness(0, 2, 2, 2);
            ////VoiceReserve.BorderThickness = new Thickness(1);
            //VoiceReserve.IsVisible = false;
            //columndefinition_0233.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0234.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0235.Text = "VoiceReserve 1";
            ////textblock_0235.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve01.Maximum = 64;
            //slVoiceReserve01.Minimum = 0;
            ////slVoiceReserve01.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve01.ValueChanged += slVoiceReserve01_ValueChanged;
            //columndefinition_0238.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0239.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0240.Text = "VoiceReserve 2";
            ////textblock_0240.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve02.Maximum = 64;
            //slVoiceReserve02.Minimum = 0;
            ////slVoiceReserve02.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve02.ValueChanged += slVoiceReserve02_ValueChanged;
            //columndefinition_0243.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0244.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0245.Text = "VoiceReserve 3";
            ////textblock_0245.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve03.Maximum = 64;
            //slVoiceReserve03.Minimum = 0;
            ////slVoiceReserve03.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve03.ValueChanged += slVoiceReserve03_ValueChanged;
            //columndefinition_0248.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0249.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0250.Text = "VoiceReserve 4";
            ////textblock_0250.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve04.Maximum = 64;
            //slVoiceReserve04.Minimum = 0;
            ////slVoiceReserve04.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve04.ValueChanged += slVoiceReserve04_ValueChanged;
            //columndefinition_0253.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0254.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0255.Text = "VoiceReserve 5";
            ////textblock_0255.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve05.Minimum = 0;
            //slVoiceReserve05.Maximum = 64;
            ////slVoiceReserve05.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve05.ValueChanged += slVoiceReserve05_ValueChanged;
            //columndefinition_0258.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0259.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0260.Text = "VoiceReserve 6";
            ////textblock_0260.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve06.Minimum = 0;
            //slVoiceReserve06.Maximum = 64;
            ////slVoiceReserve06.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve06.ValueChanged += slVoiceReserve06_ValueChanged;
            //columndefinition_0263.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0264.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0265.Text = "VoiceReserve 7";
            ////textblock_0265.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve07.Minimum = 0;
            //slVoiceReserve07.Maximum = 64;
            ////slVoiceReserve07.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve07.ValueChanged += slVoiceReserve07_ValueChanged;
            //columndefinition_0268.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0269.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0270.Text = "VoiceReserve 8";
            ////textblock_0270.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve08.Minimum = 0;
            //slVoiceReserve08.Maximum = 64;
            ////slVoiceReserve08.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve08.ValueChanged += slVoiceReserve08_ValueChanged;
            //columndefinition_0273.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0274.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0275.Text = "VoiceReserve 9";
            ////textblock_0275.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve09.Minimum = 0;
            //slVoiceReserve09.Maximum = 64;
            ////slVoiceReserve09.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve09.ValueChanged += slVoiceReserve09_ValueChanged;
            //columndefinition_0278.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0279.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0280.Text = "VoiceReserve 10";
            ////textblock_0280.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve10.Minimum = 0;
            //slVoiceReserve10.Maximum = 64;
            ////slVoiceReserve10.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve10.ValueChanged += slVoiceReserve10_ValueChanged;
            //columndefinition_0283.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0284.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0285.Text = "VoiceReserve 11";
            ////textblock_0285.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve11.Minimum = 0;
            //slVoiceReserve11.Maximum = 64;
            ////slVoiceReserve11.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve11.ValueChanged += slVoiceReserve11_ValueChanged;
            //columndefinition_0288.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0289.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0290.Text = "VoiceReserve 12";
            ////textblock_0290.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve12.Minimum = 0;
            //slVoiceReserve12.Maximum = 64;
            ////slVoiceReserve12.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve12.ValueChanged += slVoiceReserve12_ValueChanged;
            //columndefinition_0293.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0294.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0295.Text = "VoiceReserve 13";
            ////textblock_0295.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve13.Minimum = 0;
            //slVoiceReserve13.Maximum = 64;
            ////slVoiceReserve13.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve13.ValueChanged += slVoiceReserve13_ValueChanged;
            //columndefinition_0298.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0299.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0300.Text = "VoiceReserve 14";
            ////textblock_0300.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve14.Minimum = 0;
            //slVoiceReserve14.Maximum = 64;
            ////slVoiceReserve14.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve14.ValueChanged += slVoiceReserve14_ValueChanged;
            //columndefinition_0303.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0304.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0305.Text = "VoiceReserve 15";
            ////textblock_0305.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve15.Minimum = 0;
            //slVoiceReserve15.Maximum = 64;
            ////slVoiceReserve15.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve15.ValueChanged += slVoiceReserve15_ValueChanged;
            //columndefinition_0308.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0309.Width = new GridLength(2, GridUnitType.Star);
            //textblock_0310.Text = "VoiceReserve 16";
            ////textblock_0310.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve16.Minimum = 0;
            //slVoiceReserve16.Maximum = 64;
            ////slVoiceReserve16.Padding = new Thickness(4, 0, 4, 0);
            //slVoiceReserve16.ValueChanged += slVoiceReserve16_ValueChanged;
            //Chorus.Margin = new Thickness(2, 2, 2, 2);
            ////Chorus.BorderThickness = new Thickness(1);
            //Chorus.IsVisible = false;
            //rowdefinition_0312.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0313.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0314.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0315.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0316.Height = new GridLength(15, GridUnitType.Star);
            //columndefinition_0319.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0320.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0321.Text = "Type";
            ////textblock_0321.Padding = new Thickness(4, 0, 4, 0);
            ////cbStudioSetChorusType.Padding = new Thickness(4, 0, 4, 0);
            //cbStudioSetChorusType.SelectedIndex = 0;
            //cbStudioSetChorusType.SelectedIndexChanged += cbStudioSetChorusType_SelectionChanged;
            //columndefinition_0324.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0325.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusLevel.Text = "Level";
            ////tbChorusLevel.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusLevel.Padding = new Thickness(4, 0, 4, 0);
            //slChorusLevel.Minimum = 0;
            //slChorusLevel.Maximum = 127;
            //slChorusLevel.ValueChanged += slChorusLevel_ValueChanged;
            //columndefinition_0328.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0329.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0330.Text = "Output assign";
            ////textblock_0330.Padding = new Thickness(4, 0, 4, 0);
            ////cbChorusOutputAssign.Padding = new Thickness(4, 0, 4, 0);
            //cbChorusOutputAssign.SelectedIndexChanged += cbChorusOutputAssign_SelectionChanged;
            //columndefinition_0333.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0334.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0335.Text = "Output select";
            ////textblock_0335.Padding = new Thickness(4, 0, 4, 0);
            ////cbChorusOutputSelect.Padding = new Thickness(4, 0, 4, 0);
            //cbChorusOutputSelect.SelectedIndexChanged += cbChorusOutputAssign_SelectionChanged;
            //ChorusChorus.Margin = new Thickness(2, 2, 2, 2);
            ////ChorusChorus.BorderThickness = new Thickness(1);
            //rowdefinition_0337.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0338.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0339.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0340.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0341.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0342.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0343.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0344.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0345.Height = new GridLength(6, GridUnitType.Star);
            //columndefinition_0348.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0349.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusChorusFilterType.Text = "Filter type";
            ////tbChorusChorusFilterType.Padding = new Thickness(4, 0, 4, 0);
            ////cbChorusChorusFilterType.Padding = new Thickness(4, 0, 4, 0);
            //cbChorusChorusFilterType.SelectedIndexChanged += cbChorusChorusFilterType_SelectionChanged;
            //columndefinition_0352.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0353.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusChorusFilterCutoffFrequency.Text = "Filter cutoff frequency";
            ////tbChorusChorusFilterCutoffFrequency.Padding = new Thickness(4, 0, 4, 0);
            ////cbChorusChorusFilterCutoffFrequency.Padding = new Thickness(4, 0, 4, 0);
            //cbChorusChorusFilterCutoffFrequency.SelectedIndexChanged += cbChorusChorusFilterCutoffFrequency_SelectionChanged;
            //columndefinition_0356.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0357.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusChorusPreDelay.Text = "Pre delay";
            ////tbChorusChorusPreDelay.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusChorusPreDelay.Padding = new Thickness(4, 0, 4, 0);
            //slChorusChorusPreDelay.Minimum = 0;
            //slChorusChorusPreDelay.Maximum = 125;
            //slChorusChorusPreDelay.ValueChanged += slChorusChorusPreDelay_ValueChanged;
            //columndefinition_0360.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0361.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusChorusRateHzNote.Text = "Rate (num/note sw)";
            ////tbChorusChorusRateHzNote.Padding = new Thickness(4, 0, 4, 0);
            ////cbChorusChorusRateHzNote.Padding = new Thickness(4, 0, 4, 0);
            //cbChorusChorusRateHzNote.SelectedIndexChanged += cbChorusChorusRateHzNote_SelectionChanged;
            //columndefinition_0363.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0364.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusChorusRateHz.Text = "Rate";
            ////tbChorusChorusRateHz.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusChorusRateHz.Padding = new Thickness(4, 0, 4, 0);
            //slChorusChorusRateHz.Minimum = 0;
            //slChorusChorusRateHz.Maximum = 200;
            //slChorusChorusRateHz.ValueChanged += slChorusChorusRateHz_ValueChanged;
            //columndefinition_0366.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0367.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusChorusRateNote.Text = "Rate";
            ////tbChorusChorusRateNote.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusChorusRateNote.Padding = new Thickness(4, 0, 4, 0);
            //slChorusChorusRateNote.Minimum = 0;
            //slChorusChorusRateNote.Maximum = 21;
            //slChorusChorusRateNote.ValueChanged += slChorusChorusRateNote_ValueChanged;
            //columndefinition_0369.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0370.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusChorusDepth.Text = "Depth";
            ////tbChorusChorusDepth.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusChorusDepth.Padding = new Thickness(4, 0, 4, 0);
            //slChorusChorusDepth.Minimum = 0;
            //slChorusChorusDepth.Maximum = 127;
            //slChorusChorusDepth.ValueChanged += slChorusChorusDepth_ValueChanged;
            //columndefinition_0373.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0374.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusChorusPhase.Text = "Phase";
            ////tbChorusChorusPhase.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusChorusPhase.Padding = new Thickness(4, 0, 4, 0);
            //slChorusChorusPhase.Minimum = 0;
            //slChorusChorusPhase.Maximum = 180;
            //// FIX!!! slChorusChorusPhase.StepFrequency = 2;
            //slChorusChorusPhase.ValueChanged += slChorusChorusPhase_ValueChanged;
            //tbChorusChorusFeedback.Text = "Feedback";
            ////tbChorusChorusFeedback.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusChorusFeedback.Padding = new Thickness(4, 0, 4, 0);
            //slChorusChorusFeedback.Minimum = 0;
            //slChorusChorusFeedback.Maximum = 127;
            //slChorusChorusFeedback.ValueChanged += slChorusChorusFeedback_ValueChanged;
            //ChorusDelay.Margin = new Thickness(2, 2, 2, 2);
            ////ChorusDelay.BorderThickness = new Thickness(1);
            //rowdefinition_0380.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0381.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0382.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0383.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0384.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0385.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0386.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0387.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0388.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0389.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0390.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0391.Height = new GridLength(3, GridUnitType.Star);
            //columndefinition_0394.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0395.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0396.Text = "Delay left (num/note sw)";
            ////textblock_0396.Padding = new Thickness(4, 0, 4, 0);
            ////cbChorusDelayLeftMsNote.Padding = new Thickness(4, 0, 4, 0);
            //cbChorusDelayLeftMsNote.SelectedIndexChanged += cbChorusDelayLeftMsNote_SelectionChanged;
            //columndefinition_0398.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0399.Width = new GridLength(2, GridUnitType.Star);
            //tbChorusDelayLeftHz.Text = "Delay left";
            ////tbChorusDelayLeftHz.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusDelayLeftHz.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayLeftHz.Minimum = 0;
            //slChorusDelayLeftHz.Maximum = 200;
            //slChorusDelayLeftHz.ValueChanged += slChorusDelayLeftHz_ValueChanged;
            //columndefinition_0401.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0402.Width = new GridLength(2, GridUnitType.Star);
            //tbChorusDelayLeftNote.Text = "Delay left";
            ////tbChorusDelayLeftNote.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusDelayLeftNote.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayLeftNote.Minimum = 0;
            //slChorusDelayLeftNote.Maximum = 21;
            //slChorusDelayLeftNote.ValueChanged += slChorusDelayLeftNote_ValueChanged;
            //columndefinition_0405.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0406.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0407.Text = "Delay right (num/note sw)";
            ////textblock_0407.Padding = new Thickness(4, 0, 4, 0);
            ////cbChorusDelayRightMsNote.Padding = new Thickness(4, 0, 4, 0);
            //cbChorusDelayRightMsNote.SelectedIndexChanged += cbChorusDelayRightMsNote_SelectionChanged;
            //columndefinition_0409.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0410.Width = new GridLength(2, GridUnitType.Star);
            //tbChorusDelayRightHz.Text = "Delay right";
            ////tbChorusDelayRightHz.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusDelayRightHz.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayRightHz.Minimum = 0;
            //slChorusDelayRightHz.Maximum = 200;
            //slChorusDelayRightHz.ValueChanged += slChorusDelayRightHz_ValueChanged;
            //columndefinition_0412.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0413.Width = new GridLength(2, GridUnitType.Star);
            //tbChorusDelayRightNote.Text = "Delay right";
            ////tbChorusDelayRightNote.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusDelayRightNote.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayRightNote.Minimum = 0;
            //slChorusDelayRightNote.Maximum = 21;
            //slChorusDelayRightNote.ValueChanged += slChorusDelayRightNote_ValueChanged;
            //columndefinition_0416.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0417.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0418.Text = "Delay center (num/note sw)";
            ////textblock_0418.Padding = new Thickness(4, 0, 4, 0);
            ////cbChorusDelayCenterMsNote.Padding = new Thickness(4, 0, 4, 0);
            //cbChorusDelayCenterMsNote.SelectedIndexChanged += cbChorusDelayCenterMsNote_SelectionChanged;
            //columndefinition_0420.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0421.Width = new GridLength(2, GridUnitType.Star);
            //tbChorusDelayCenterHz.Text = "Delay center";
            ////tbChorusDelayCenterHz.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusDelayCenterHz.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayCenterHz.Minimum = 0;
            //slChorusDelayCenterHz.Maximum = 200;
            //slChorusDelayCenterHz.ValueChanged += slChorusDelayCenterHz_ValueChanged;
            //columndefinition_0423.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0424.Width = new GridLength(2, GridUnitType.Star);
            //tbChorusDelayCenterNote.Text = "Delay center";
            ////tbChorusDelayCenterNote.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusDelayCenterNote.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayCenterNote.Minimum = 0;
            //slChorusDelayCenterNote.Maximum = 21;
            //slChorusDelayCenterNote.ValueChanged += slChorusDelayCenterNote_ValueChanged;
            //columndefinition_0427.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0428.Width = new GridLength(2, GridUnitType.Star);
            //tbChorusDelayCenterFeedback.Text = "Center feedback";
            ////tbChorusDelayCenterFeedback.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayCenterFeedback.Minimum = -98;
            //slChorusDelayCenterFeedback.Maximum = 98;
            //// FIX!!! slChorusDelayCenterFeedback.StepFrequency = 2;
            ////slChorusDelayCenterFeedback.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayCenterFeedback.ValueChanged += slChorusDelayCenterFeedback_ValueChanged;
            //columndefinition_0431.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0432.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusDelayHFDamp.Text = "HF damp";
            ////tbChorusDelayHFDamp.Padding = new Thickness(4, 0, 4, 0);
            ////cbChorusDelayHFDamp.Padding = new Thickness(4, 0, 4, 0);
            //cbChorusDelayHFDamp.SelectedIndexChanged += cbChorusDelayHFDamp_SelectionChanged;
            //columndefinition_0435.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0436.Width = new GridLength(2, GridUnitType.Star);
            //tbChorusDelayLeftLevel.Text = "Left level";
            ////tbChorusDelayLeftLevel.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayLeftLevel.Minimum = 0;
            //slChorusDelayLeftLevel.Maximum = 127;
            ////slChorusDelayLeftLevel.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayLeftLevel.ValueChanged += slChorusDelayLeftLevel_ValueChanged;
            //columndefinition_0439.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0440.Width = new GridLength(2, GridUnitType.Star);
            //tbChorusDelayRightLevel.Text = "Right level";
            ////tbChorusDelayRightLevel.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayRightLevel.Minimum = 0;
            //slChorusDelayRightLevel.Maximum = 127;
            ////slChorusDelayRightLevel.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayRightLevel.ValueChanged += slChorusDelayRightevel_ValueChanged;
            //columndefinition_0443.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0444.Width = new GridLength(2, GridUnitType.Star);
            //tbChorusDelayCenterLevel.Text = "Center level";
            ////tbChorusDelayCenterLevel.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayCenterLevel.Minimum = 0;
            //slChorusDelayCenterLevel.Maximum = 127;
            ////slChorusDelayCenterLevel.Padding = new Thickness(4, 0, 4, 0);
            //slChorusDelayCenterLevel.ValueChanged += slChorusDelayCenterLevel_ValueChanged;
            //ChorusGM2Chorus.Margin = new Thickness(2, 2, 2, 2);
            ////ChorusGM2Chorus.BorderThickness = new Thickness(1);
            //rowdefinition_0446.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0447.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0448.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0449.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0450.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0451.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0452.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0453.Height = new GridLength(7, GridUnitType.Star);
            //columndefinition_0455.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0456.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusGM2ChorusPreLPF.Text = "Pre-LPF";
            ////tbChorusGM2ChorusPreLPF.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusGM2ChorusPreLPF.Padding = new Thickness(4, 0, 4, 0);
            //slChorusGM2ChorusPreLPF.Minimum = 0;
            //slChorusGM2ChorusPreLPF.Maximum = 7;
            //slChorusGM2ChorusPreLPF.ValueChanged += slChorusGM2ChorusPreLPF_ValueChanged;
            //columndefinition_0458.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0459.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusGM2ChorusLevel.Text = "Level";
            ////tbChorusGM2ChorusLevel.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusGM2ChorusLevel.Padding = new Thickness(4, 0, 4, 0);
            //slChorusGM2ChorusLevel.Minimum = 0;
            //slChorusGM2ChorusLevel.Maximum = 127;
            //slChorusGM2ChorusLevel.ValueChanged += slChorusGM2ChorusLevel_ValueChanged;
            //columndefinition_0461.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0462.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusGM2ChorusFeedback.Text = "Feedback";
            ////tbChorusGM2ChorusFeedback.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusGM2ChorusFeedback.Padding = new Thickness(4, 0, 4, 0);
            //slChorusGM2ChorusFeedback.Minimum = 0;
            //slChorusGM2ChorusFeedback.Maximum = 127;
            //slChorusGM2ChorusFeedback.ValueChanged += slChorusGM2ChorusFeedback_ValueChanged;
            //columndefinition_0464.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0465.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusGM2ChorusDelay.Text = "Delay";
            ////tbChorusGM2ChorusDelay.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusGM2ChorusDelay.Padding = new Thickness(4, 0, 4, 0);
            //slChorusGM2ChorusDelay.Minimum = 0;
            //slChorusGM2ChorusDelay.Maximum = 127;
            //slChorusGM2ChorusDelay.ValueChanged += slChorusGM2ChorusDelay_ValueChanged;
            //columndefinition_0467.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0468.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusGM2ChorusRate.Text = "Rate";
            ////tbChorusGM2ChorusRate.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusGM2ChorusRate.Padding = new Thickness(4, 0, 4, 0);
            //slChorusGM2ChorusRate.Minimum = 0;
            //slChorusGM2ChorusRate.Maximum = 127;
            //slChorusGM2ChorusRate.ValueChanged += slChorusGM2ChorusRate_ValueChanged;
            //columndefinition_0470.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0471.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusGM2ChorusDepth.Text = "Depth";
            ////tbChorusGM2ChorusDepth.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusGM2ChorusDepth.Padding = new Thickness(4, 0, 4, 0);
            //slChorusGM2ChorusDepth.Minimum = 0;
            //slChorusGM2ChorusDepth.Maximum = 127;
            //slChorusGM2ChorusDepth.ValueChanged += slChorusGM2ChorusDepth_ValueChanged;
            //columndefinition_0473.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0474.Width = new GridLength(1, GridUnitType.Star);
            //tbChorusGM2ChorusSendLevelToReverb.Text = "Send lvl to rev.";
            ////tbChorusGM2ChorusSendLevelToReverb.Padding = new Thickness(4, 0, 4, 0);
            ////slChorusGM2ChorusSendLevelToReverb.Padding = new Thickness(4, 0, 4, 0);
            //slChorusGM2ChorusSendLevelToReverb.Minimum = 0;
            //slChorusGM2ChorusSendLevelToReverb.Maximum = 127;
            //slChorusGM2ChorusSendLevelToReverb.ValueChanged += slChorusGM2ChorusSendLevelToReverb_ValueChanged;
            //ChorusReverb.Margin = new Thickness(0, 2, 2, 2);
            ////ChorusReverb.BorderThickness = new Thickness(1);
            //ChorusReverb.IsVisible = false;
            //rowdefinition_0476.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0477.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0478.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0479.Height = new GridLength(15, GridUnitType.Star);
            //columndefinition_0482.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0483.Width = new GridLength(3, GridUnitType.Star);
            //textblock_0484.Text = "Type";
            ////textblock_0484.Padding = new Thickness(4, 0, 4, 0);
            ////cbStudioSetReverbType.Padding = new Thickness(4, 0, 4, 0);
            //cbStudioSetReverbType.SelectedIndexChanged += cbStudioSetReverbType_SelectionChanged;
            //columndefinition_0487.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0488.Width = new GridLength(3, GridUnitType.Star);
            //tbStudioSetReverbLevel.Text = "Level";
            ////tbStudioSetReverbLevel.Padding = new Thickness(4, 0, 4, 0);
            ////slStudioSetReverbLevel.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbLevel.Minimum = 0;
            //slStudioSetReverbLevel.Maximum = 127;
            //slStudioSetReverbLevel.ValueChanged += slReverbLevel_ValueChanged;
            //columndefinition_0491.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_0492.Width = new GridLength(3, GridUnitType.Star);
            //tbStudioSetReverbOutputAssign.Text = "Output assign";
            ////tbStudioSetReverbOutputAssign.Padding = new Thickness(4, 0, 4, 0);
            ////cbStudioSetReverbOutputAssign.Padding = new Thickness(4, 0, 4, 0);
            //cbStudioSetReverbOutputAssign.SelectedIndexChanged += cbStudioSetReverbOutputAssign_SelectionChanged;
            //StudioSetReverb.Margin = new Thickness(0, 2, 2, 2);
            ////StudioSetReverb.BorderThickness = new Thickness(1);
            //rowdefinition_0494.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0495.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0496.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0497.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0498.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0499.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0500.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0501.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0502.Height = new GridLength(7, GridUnitType.Star);
            //columndefinition_0505.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0506.Width = new GridLength(2, GridUnitType.Star);
            //tbStudioSetReverbPreDelay.Text = "Pre delay";
            ////tbStudioSetReverbPreDelay.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbPreDelay.Minimum = 0;
            //slStudioSetReverbPreDelay.Maximum = 100;
            ////slStudioSetReverbPreDelay.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbPreDelay.ValueChanged += slStudioSetReverbPreDelay_ValueChanged;
            //columndefinition_0509.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0510.Width = new GridLength(2, GridUnitType.Star);
            //tbStudioSetReverbTime.Text = "Time";
            ////tbStudioSetReverbTime.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbTime.Maximum = 100;
            //slStudioSetReverbTime.Minimum = 1;
            ////slStudioSetReverbTime.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbTime.ValueChanged += slStudioSetReverbTime_ValueChanged;
            //columndefinition_0513.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0514.Width = new GridLength(2, GridUnitType.Star);
            //tbStudioSetReverbDensity.Text = "Density";
            ////tbStudioSetReverbDensity.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbDensity.Minimum = 0;
            //slStudioSetReverbDensity.Maximum = 127;
            ////slStudioSetReverbDensity.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbDensity.ValueChanged += slStudioSetReverbDensity_ValueChanged;
            //columndefinition_0517.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0518.Width = new GridLength(2, GridUnitType.Star);
            //tbStudioSetReverbDiffusion.Text = "Diffusion";
            ////tbStudioSetReverbDiffusion.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbDiffusion.Minimum = 0;
            //slStudioSetReverbDiffusion.Maximum = 127;
            ////slStudioSetReverbDiffusion.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbDiffusion.ValueChanged += slStudioSetReverbDiffusion_ValueChanged;
            //columndefinition_0521.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0522.Width = new GridLength(2, GridUnitType.Star);
            //tbStudioSetReverbLFDamp.Text = "LF damp";
            ////tbStudioSetReverbLFDamp.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbLFDamp.Minimum = 0;
            //slStudioSetReverbLFDamp.Maximum = 100;
            ////slStudioSetReverbLFDamp.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbLFDamp.ValueChanged += slStudioSetReverbLFDamp_ValueChanged;
            //columndefinition_0525.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0526.Width = new GridLength(2, GridUnitType.Star);
            //tbStudioSetReverbHFDamp.Text = "HF damp";
            ////tbStudioSetReverbHFDamp.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbHFDamp.Minimum = 0;
            //slStudioSetReverbHFDamp.Maximum = 100;
            ////slStudioSetReverbHFDamp.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbHFDamp.ValueChanged += slStudioSetReverbHFDamp_ValueChanged;
            //columndefinition_0529.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0530.Width = new GridLength(2, GridUnitType.Star);
            //tbStudioSetReverbSpread.Text = "Spread";
            ////tbStudioSetReverbSpread.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbSpread.Minimum = 0;
            //slStudioSetReverbSpread.Maximum = 127;
            ////slStudioSetReverbSpread.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbSpread.ValueChanged += slStudioSetReverbSpread_ValueChanged;
            //columndefinition_0533.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0534.Width = new GridLength(2, GridUnitType.Star);
            //tbStudioSetReverbTone.Text = "Tone";
            ////tbStudioSetReverbTone.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbTone.Minimum = 0;
            //slStudioSetReverbTone.Maximum = 127;
            ////slStudioSetReverbTone.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbTone.ValueChanged += slStudioSetReverbTone_ValueChanged;
            //StudioSetReverbGM2.Margin = new Thickness(0, 2, 2, 2);
            ////StudioSetReverbGM2.BorderThickness = new Thickness(1);
            //rowdefinition_0536.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0537.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0538.Height = new GridLength(15, GridUnitType.Star);
            //columndefinition_0541.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0542.Width = new GridLength(2, GridUnitType.Star);
            //tbStudioSetReverbGM2Character.Text = "Character";
            ////tbStudioSetReverbGM2Character.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbGM2Character.Minimum = 0;
            //slStudioSetReverbGM2Character.Maximum = 5;
            ////slStudioSetReverbGM2Character.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbGM2Character.ValueChanged += slStudioSetReverbGM2Character_ValueChanged;
            //columndefinition_0545.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0546.Width = new GridLength(2, GridUnitType.Star);
            //tbStudioSetReverbGM2Time.Text = "Time";
            ////tbStudioSetReverbGM2Time.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbGM2Time.Minimum = 0;
            //slStudioSetReverbGM2Time.Maximum = 127;
            //// FIX!!! slStudioSetReverbGM2Time.StepFrequency = "0.1";
            ////slStudioSetReverbGM2Time.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetReverbGM2Time.ValueChanged += slStudioSetReverbGM2Time_ValueChanged;
            //StudioSetMotionalSurround.Margin = new Thickness(0, 2, 2, 2);
            ////StudioSetMotionalSurround.BorderThickness = new Thickness(1);
            //StudioSetMotionalSurround.IsVisible = false;
            //grid_0566.Margin = new Thickness(2, 0, 2, 2);
            //grid_0566.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0566.BorderThickness = new Thickness(1);
            ////cbStudioSetMotionalSurround.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetMotionalSurround.Content = "Motional surround";
            //cbStudioSetMotionalSurround.Toggled += cbStudioSetMotionalSurround_Click;
            //grid_0567.Padding = new Thickness(4, 0, 0, 0);
            //columndefinition_0569.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0570.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0571.Text = "Room type";
            //cbStudioSetMotionalSurroundRoomType.SelectedIndexChanged += cbStudioSetMotionalSurroundRoomType_SelectionChanged;
            //columndefinition_0574.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0575.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMotionalSurroundAmbienceLevel.Text = "Ambience level";
            ////tbStudioSetMotionalSurroundAmbienceLevel.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundAmbienceLevel.Minimum = 0;
            //slStudioSetMotionalSurroundAmbienceLevel.Maximum = 127;
            ////slStudioSetMotionalSurroundAmbienceLevel.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundAmbienceLevel.ValueChanged += slStudioSetMotionalSurroundAmbienceLevel_ValueChanged;
            //columndefinition_0578.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0579.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0580.Text = "Room size";
            ////textblock_0580.Padding = new Thickness(4, 0, 4, 0);
            //cbStudioSetMotionalSurroundRoomSize.SelectedIndexChanged += cbStudioSetMotionalSurroundRoomSize_SelectionChanged;
            //columndefinition_0583.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0584.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMotionalSurroundAmbienceTime.Text = "Ambience time";
            ////tbStudioSetMotionalSurroundAmbienceTime.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundAmbienceTime.Minimum = 0;
            //slStudioSetMotionalSurroundAmbienceTime.Maximum = 100;
            ////slStudioSetMotionalSurroundAmbienceTime.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundAmbienceTime.ValueChanged += slStudioSetMotionalSurroundAmbienceTime_ValueChanged;
            //columndefinition_0587.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0588.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMotionalSurroundAmbienceDensity.Text = "Ambience density";
            ////tbStudioSetMotionalSurroundAmbienceDensity.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundAmbienceDensity.Minimum = 0;
            //slStudioSetMotionalSurroundAmbienceDensity.Maximum = 100;
            ////slStudioSetMotionalSurroundAmbienceDensity.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundAmbienceDensity.ValueChanged += slStudioSetMotionalSurroundAmbienceDensity_ValueChanged;
            //columndefinition_0591.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0592.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMotionalSurroundAmbienceHFDamp.Text = "Ambience HF damp";
            ////tbStudioSetMotionalSurroundAmbienceHFDamp.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundAmbienceHFDamp.Minimum = 0;
            //slStudioSetMotionalSurroundAmbienceHFDamp.Maximum = 100;
            ////slStudioSetMotionalSurroundAmbienceHFDamp.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundAmbienceHFDamp.ValueChanged += slStudioSetMotionalSurroundAmbienceHFDamp_ValueChanged;
            //columndefinition_0595.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0596.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMotionalSurroundExternalPartLR.Text = "External part L-R";
            ////tbStudioSetMotionalSurroundExternalPartLR.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundExternalPartLR.Minimum = -64;
            //slStudioSetMotionalSurroundExternalPartLR.Maximum = 63;
            ////slStudioSetMotionalSurroundExternalPartLR.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundExternalPartLR.ValueChanged += slStudioSetMotionalSurroundExternalPartLR_ValueChanged;
            //columndefinition_0599.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0600.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMotionalSurroundExternalPartFB.Text = "External part F-B";
            ////tbStudioSetMotionalSurroundExternalPartFB.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundExternalPartFB.Minimum = -64;
            //slStudioSetMotionalSurroundExternalPartFB.Maximum = 63;
            ////slStudioSetMotionalSurroundExternalPartFB.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundExternalPartFB.ValueChanged += slStudioSetMotionalSurroundExternalPartFB_ValueChanged;
            //columndefinition_0603.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0604.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMotionalSurroundExtPartWidth.Text = "Ext part width";
            ////tbStudioSetMotionalSurroundExtPartWidth.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundExtPartWidth.Minimum = 0;
            //slStudioSetMotionalSurroundExtPartWidth.Maximum = 32;
            ////slStudioSetMotionalSurroundExtPartWidth.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundExtPartWidth.ValueChanged += slStudioSetMotionalSurroundExtPartWidth_ValueChanged;
            //columndefinition_0607.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0608.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMotionalSurroundExtpartAmbienceSend.Text = "Ext part ambience send";
            ////tbStudioSetMotionalSurroundExtpartAmbienceSend.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundExtpartAmbienceSend.Minimum = 0;
            //slStudioSetMotionalSurroundExtpartAmbienceSend.Maximum = 127;
            ////slStudioSetMotionalSurroundExtpartAmbienceSend.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundExtpartAmbienceSend.ValueChanged += slStudioSetMotionalSurroundExtpartAmbienceSend_ValueChanged;
            //grid_0609.Padding = new Thickness(4, 0, 0, 0);
            //columndefinition_0611.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0612.Width = new GridLength(1, GridUnitType.Star);
            //textblock_0613.Text = "Ext part control";
            //cbStudioSetMotionalSurroundExtPartControl.SelectedIndexChanged += cbStudioSetMotionalSurroundExtPartControl_SelectionChanged;
            //columndefinition_0616.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0617.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMotionalSurroundDepth.Text = "Motional surround depth";
            ////tbStudioSetMotionalSurroundDepth.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundDepth.Minimum = 0;
            //slStudioSetMotionalSurroundDepth.Maximum = 100;
            ////slStudioSetMotionalSurroundDepth.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMotionalSurroundDepth.ValueChanged += slStudioSetMotionalSurroundDepth_ValueChanged;
            //StudioSetMasterEQ.Margin = new Thickness(0, 2, 2, 2);
            ////StudioSetMasterEQ.BorderThickness = new Thickness(1);
            //StudioSetMasterEQ.IsVisible = false;
            //columndefinition_0645.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0646.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMasterEqLowFreq.Text = "EQ low freq";
            ////tbStudioSetMasterEqLowFreq.Padding = new Thickness(4, 0, 4, 0);
            //cbStudioSetMasterEqLowFreq.SelectedIndexChanged += cbStudioSetMasterEqLowFreq_SelectionChanged;
            //columndefinition_0649.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0650.Width = new GridLength(1, GridUnitType.Star);
            ////tbStudioSetMasterEqLowGain.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMasterEqLowGain.Minimum = -15;
            //slStudioSetMasterEqLowGain.Maximum = 15;
            //slStudioSetMasterEqLowGain.ValueChanged += slStudioSetMasterEqLowGain_ValueChanged;
            //columndefinition_0653.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0654.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMasterEqMidFreq.Text = "EQ mid freq";
            ////tbStudioSetMasterEqMidFreq.Padding = new Thickness(4, 0, 4, 0);
            //cbStudioSetMasterEqMidFreq.SelectedIndexChanged += cbStudioSetMasterEqMidFreq_SelectionChanged;
            //columndefinition_0657.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0658.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMasterEqMidGain.Text = "Parameter 2";
            ////tbStudioSetMasterEqMidGain.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMasterEqMidGain.Minimum = -15;
            //slStudioSetMasterEqMidGain.Maximum = 15;
            //slStudioSetMasterEqMidGain.ValueChanged += slStudioSetMasterEqMidGain_ValueChanged;
            //columndefinition_0661.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0662.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMasterEqMidQ.Text = "EQ mid Q";
            ////tbStudioSetMasterEqMidQ.Padding = new Thickness(4, 0, 4, 0);
            //cbStudioSetMasterEqMidQ.SelectedIndexChanged += cbStudioSetMasterEqMidQ_SelectionChanged;
            //columndefinition_0665.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0666.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMasterEqHighFreq.Text = "EQ high freq";
            ////tbStudioSetMasterEqHighFreq.Padding = new Thickness(4, 0, 4, 0);
            //cbStudioSetMasterEqHighFreq.SelectedIndexChanged += cbStudioSetMasterEqHighFreq_SelectionChanged;
            //columndefinition_0669.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0670.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetMasterEqHighGain.Text = "Parameter 2";
            ////tbStudioSetMasterEqHighGain.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetMasterEqHighGain.Minimum = -15;
            //slStudioSetMasterEqHighGain.Maximum = 15;
            //slStudioSetMasterEqHighGain.ValueChanged += slStudioSetMasterEqHighGain_ValueChanged;
            //gEditStudioSetSearchResult.Margin = new Thickness(0, 2, 0, 2);
            ////gEditStudioSetSearchResult.BorderThickness = new Thickness(1);
            //gEditStudioSetSearchResult.IsVisible = false;
            //lvSearchResults.Margin = new Thickness(2, 2, 0, 2);
            ////lvSearchResults.BorderThickness = new Thickness(1);
            ////lvSearchResults.RequestedTheme = "Light";
            //lvSearchResults.ItemSelected += lvSearchResults_SelectionChanged;
            //gEditStudioSetColumn2.Margin = new Thickness(2, 2, 2, 2);
            ////grid_0671.BorderThickness = new Thickness(1);
            //rowdefinition_0673.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0674.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0675.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0676.Height = new GridLength(14, GridUnitType.Star);
            //rowdefinition_0677.Height = new GridLength(2, GridUnitType.Star);
            //cbStudioSetPartSelector.Margin = new Thickness(2, 2, 2, 2);
            //cbStudioSetPartSelector.SelectedIndex = 0;
            //cbStudioSetPartSelector.SelectedIndexChanged += cbStudioSetPartSelector_SelectionChanged;
            //grid_PartSettings.Margin = new Thickness(2, 0, 2, 2);
            ////PartSettings.BorderThickness = new Thickness(1);
            //cbStudioSetPartSubSelector.SelectedIndex = 0;
            //cbStudioSetPartSubSelector.SelectedIndexChanged += cbStudioSetPartSubSelector_SelectionChanged;
            //grid_StudioSetPartSubSelector.Margin = new Thickness(2, 0, 2, 2);
            //grid_StudioSetPartSubSelector.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0679.BorderThickness = new Thickness(1);
            ////StudioSetPartSettings1.Name = "StudioSetPartSettings1";
            //StudioSetPartSettings1.Margin = new Thickness(2, 0, 2, 2);
            ////StudioSetPartSettings1.BorderThickness = new Thickness(1);
            //StudioSetPartSettings1.IsVisible = true;
            //rowdefinition_0681.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0682.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0683.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0684.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0685.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0686.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0687.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0688.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0689.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0690.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0691.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0692.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0693.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0694.Height = new GridLength(1, GridUnitType.Star);
            //grid_0695.Margin = new Thickness(2, 2, 2, 2);
            //grid_0695.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0695.BorderThickness = new Thickness(1);
            ////cbStudioSetPartSettings1Receive.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartSettings1Receive.Content = "Receive";
            //cbStudioSetPartSettings1Receive.Toggled += cbStudioSetPartSettings1Receive_Click;
            //cbStudioSetPartSettings1ReceiveChannel.SelectedIndex = 0;
            //cbStudioSetPartSettings1ReceiveChannel.SelectedIndexChanged += cbStudioSetPartSettings1ReceiveChannel_SelectionChanged;
            //grid_0699.Margin = new Thickness(2, 0, 2, 2);
            //grid_0699.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0699.BorderThickness = new Thickness(1);
            //columndefinition_0701.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0702.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings1Group.Text = "Group ";
            //cbStudioSetPartSettings1Group.SelectedIndexChanged += cbStudioSetPartSettings1Group_ValueChanged;
            //grid_0703.Margin = new Thickness(2, 0, 2, 2);
            //grid_0703.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0703.BorderThickness = new Thickness(1);
            //columndefinition_0705.Width = new GridLength(96, GridUnitType.Star);
            //columndefinition_0706.Width = new GridLength(71, GridUnitType.Star);
            //columndefinition_0707.Width = new GridLength(25, GridUnitType.Star);
            //tbStudioSetPartSettings1Category.Text = "Category ";
            ////tbStudioSetPartSettings1Category.Grid.ColumnSpan = 2;
            //tbStudioSetPartSettings1Category.Margin = new Thickness(0, 0, 0, 4);
            //cbStudioSetPartSettings1Category.SelectedIndexChanged += cbStudioSetPartSettings1Category_ValueChanged;
            ////cbStudioSetPartSettings1Category.Grid.ColumnSpan = 2;
            //grid_0708.Margin = new Thickness(2, 0, 2, 2);
            //grid_0708.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0708.BorderThickness = new Thickness(1);
            //columndefinition_0710.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0711.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings1Program.Text = "Tone";
            //cbStudioSetPartSettings1Program.SelectedIndexChanged += cbStudioSetPartSettings1Program_SelectionChanged;
            //grid_0712.Margin = new Thickness(2, 0, 2, 2);
            //grid_0712.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0712.BorderThickness = new Thickness(1);
            //columndefinition_0714.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0715.Width = new GridLength(4, GridUnitType.Star);
            //tbStudioSetPartSettings1Search.Text = "Search";
            //cbStudioSetPartSettings1Search.TextChanged += cbStudioSetPartSettings1Search_TextChanged;
            //grid_0716.Margin = new Thickness(2, 0, 2, 2);
            //grid_0716.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0716.BorderThickness = new Thickness(1);
            //columndefinition_0718.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0719.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings1Level.Text = "Part level";
            //slStudioSetPartSettings1Level.Minimum = 0;
            //slStudioSetPartSettings1Level.Maximum = 127;
            //slStudioSetPartSettings1Level.ValueChanged += slStudioSetPartSettings1Level_ValueChanged;
            //grid_0720.Margin = new Thickness(2, 0, 2, 2);
            //grid_0720.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0720.BorderThickness = new Thickness(1);
            //columndefinition_0722.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0723.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings1Pan.Text = "Part pan";
            //slStudioSetPartSettings1Pan.Minimum = -64;
            //slStudioSetPartSettings1Pan.Maximum = 63;
            //slStudioSetPartSettings1Pan.ValueChanged += slStudioSetPartSettings1Pan_ValueChanged;
            //grid_0724.Margin = new Thickness(2, 0, 2, 2);
            //grid_0724.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0724.BorderThickness = new Thickness(1);
            //columndefinition_0726.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0727.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings1CoarseTune.Text = "Coarse tune";
            //slStudioSetPartSettings1CoarseTune.Minimum = -48;
            //slStudioSetPartSettings1CoarseTune.Maximum = 48;
            //slStudioSetPartSettings1CoarseTune.ValueChanged += slStudioSetPartSettings1CoarseTune_ValueChanged;
            //grid_0728.Margin = new Thickness(2, 0, 2, 2);
            //grid_0728.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0728.BorderThickness = new Thickness(1);
            //columndefinition_0730.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0731.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings1FineTune.Text = "Fine tune";
            //slStudioSetPartSettings1FineTune.Minimum = -50;
            //slStudioSetPartSettings1FineTune.Maximum = 50;
            //slStudioSetPartSettings1FineTune.ValueChanged += slStudioSetPartSettings1FineTune_ValueChanged;
            //grid_0732.Margin = new Thickness(2, 0, 2, 2);
            //grid_0732.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0732.BorderThickness = new Thickness(1);
            //textblock_0736.Text = "Mono/Poly";
            //cbStudioSetPartSettings1MonoPoly.SelectedIndex = 0;
            //cbStudioSetPartSettings1MonoPoly.SelectedIndexChanged += cbStudioSetPartSettings1Poly_SelectionChanged;
            //grid_0737.Margin = new Thickness(2, 0, 2, 2);
            //grid_0737.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0737.BorderThickness = new Thickness(1);
            //textblock_0741.Text = "Legato";
            //cbStudioSetPartSettings1Legato.SelectedIndex = 0;
            //cbStudioSetPartSettings1Legato.SelectedIndexChanged += cbStudioSetPartSettings1Legato_SelectionChanged;
            //grid_0742.Margin = new Thickness(2, 0, 2, 2);
            //grid_0742.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0742.BorderThickness = new Thickness(1);
            //columndefinition_0744.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0745.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings1PitchBendRange.Text = "Bend range";
            //slStudioSetPartSettings1PitchBendRange.Minimum = 0;
            //slStudioSetPartSettings1PitchBendRange.Maximum = 25;
            //slStudioSetPartSettings1PitchBendRange.ValueChanged += slStudioSetPartSettings1BendRange_ValueChanged;
            //grid_0746.Margin = new Thickness(2, 0, 2, 2);
            //grid_0746.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0746.BorderThickness = new Thickness(1);
            //textblock_0750.Text = "Portamento";
            //cbStudioSetPartSettings1Portamento.SelectedIndex = 0;
            //cbStudioSetPartSettings1Portamento.SelectedIndexChanged += cbStudioSetPartSettings1Portamento_SelectionChanged;
            //grid_0751.Margin = new Thickness(2, 0, 2, 2);
            //grid_0751.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0751.BorderThickness = new Thickness(1);
            //columndefinition_0753.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0754.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings1PortamentoTime.Text = "Portamento time";
            //slStudioSetPartSettings1PortamentoTime.Minimum = 0;
            //slStudioSetPartSettings1PortamentoTime.Maximum = 128;
            //slStudioSetPartSettings1PortamentoTime.ValueChanged += slStudioSetPartSettings1PortamentoTime_ValueChanged;
            //StudioSetPartSettings2.Margin = new Thickness(2, 0, 2, 2);
            ////StudioSetPartSettings2.BorderThickness = new Thickness(1);
            //StudioSetPartSettings2.IsVisible = false;
            //rowdefinition_0756.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0757.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0758.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0759.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0760.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0761.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0762.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0763.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0764.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0765.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0766.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0767.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0768.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0769.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0770.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0771.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0772.Height = new GridLength(1, GridUnitType.Star);
            //grid_0773.Margin = new Thickness(2, 2, 2, 2);
            //grid_0773.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0773.BorderThickness = new Thickness(1);
            //columndefinition_0775.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0776.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings2CutoffOffset.Text = "Cutoff offset";
            //slStudioSetPartSettings2CutoffOffset.Minimum = -64;
            //slStudioSetPartSettings2CutoffOffset.Maximum = 63;
            //slStudioSetPartSettings2CutoffOffset.ValueChanged += slStudioSetPartSettings2CutoffOffset_ValueChanged;
            //grid_0777.Margin = new Thickness(2, 0, 2, 2);
            //grid_0777.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0777.BorderThickness = new Thickness(1);
            //columndefinition_0779.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0780.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings2ResonanceOffset.Text = "Resonance offset";
            //slStudioSetPartSettings2ResonanceOffset.Minimum = -64;
            //slStudioSetPartSettings2ResonanceOffset.Maximum = 63;
            //slStudioSetPartSettings2ResonanceOffset.ValueChanged += slStudioSetPartSettings2ResonanceOffset_ValueChanged;
            //grid_0781.Margin = new Thickness(2, 0, 2, 2);
            //grid_0781.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0781.BorderThickness = new Thickness(1);
            //columndefinition_0783.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0784.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings2AttackTimeOffset.Text = "Attack time offset";
            //slStudioSetPartSettings2AttackTimeOffset.Minimum = -64;
            //slStudioSetPartSettings2AttackTimeOffset.Maximum = 63;
            //slStudioSetPartSettings2AttackTimeOffset.ValueChanged += slStudioSetPartSettings2AttackTimeOffset_ValueChanged;
            //grid_0785.Margin = new Thickness(2, 0, 2, 2);
            //grid_0785.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0785.BorderThickness = new Thickness(1);
            //columndefinition_0787.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0788.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings2DecayTimeOffset.Text = "Decay time offset";
            //slStudioSetPartSettings2DecayTimeOffset.Minimum = -64;
            //slStudioSetPartSettings2DecayTimeOffset.Maximum = 63;
            //slStudioSetPartSettings2DecayTimeOffset.ValueChanged += slStudioSetPartSettings2DecayTimeOffset_ValueChanged;
            //grid_0789.Margin = new Thickness(2, 0, 2, 2);
            //grid_0789.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0789.BorderThickness = new Thickness(1);
            //columndefinition_0791.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0792.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings2ReleaseTimeOffset.Text = "Release time offset";
            //slStudioSetPartSettings2ReleaseTimeOffset.Minimum = -64;
            //slStudioSetPartSettings2ReleaseTimeOffset.Maximum = 63;
            //slStudioSetPartSettings2ReleaseTimeOffset.ValueChanged += slStudioSetPartSettings2ReleaseTimeOffset_ValueChanged;
            //grid_0793.Margin = new Thickness(2, 0, 2, 2);
            //grid_0793.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0793.BorderThickness = new Thickness(1);
            //columndefinition_0795.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0796.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings2VibratoRate.Text = "Vibrato rate";
            //slStudioSetPartSettings2VibratoRate.Minimum = -64;
            //slStudioSetPartSettings2VibratoRate.Maximum = 63;
            //slStudioSetPartSettings2VibratoRate.ValueChanged += slStudioSetPartSettings2VibratoRate_ValueChanged;
            //grid_0797.Margin = new Thickness(2, 0, 2, 2);
            //grid_0797.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0797.BorderThickness = new Thickness(1);
            //columndefinition_0799.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0800.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings2VibratoDepth.Text = "Vibrato depth";
            //slStudioSetPartSettings2VibratoDepth.Minimum = -64;
            //slStudioSetPartSettings2VibratoDepth.Maximum = 63;
            //slStudioSetPartSettings2VibratoDepth.ValueChanged += slStudioSetPartSettings2VibratoDepth_ValueChanged;
            //grid_0801.Margin = new Thickness(2, 0, 2, 2);
            //grid_0801.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0801.BorderThickness = new Thickness(1);
            //columndefinition_0803.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0804.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartSettings2VibratoDelay.Text = "Vibrato delay";
            //slStudioSetPartSettings2VibratoDelay.Minimum = -64;
            //slStudioSetPartSettings2VibratoDelay.Maximum = 63;
            //slStudioSetPartSettings2VibratoDelay.ValueChanged += slStudioSetPartSettings2VibratoDelay_ValueChanged;
            //grid_0805.Margin = new Thickness(2, 0, 2, 2);
            //grid_0805.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0805.BorderThickness = new Thickness(1);
            //columndefinition_0807.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0808.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartEffectsChorusSendLevel.Text = "Chorus send level";
            //slStudioSetPartEffectsChorusSendLevel.Minimum = 0;
            //slStudioSetPartEffectsChorusSendLevel.Maximum = 127;
            //slStudioSetPartEffectsChorusSendLevel.ValueChanged += slStudioSetPartEffectsChorusSendLevel_ValueChanged;
            //grid_0809.Margin = new Thickness(2, 0, 2, 2);
            //grid_0809.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0809.BorderThickness = new Thickness(1);
            //columndefinition_0811.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0812.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartEffectsReverbSendLevel.Text = "Reverb send level";
            //slStudioSetPartEffectsReverbSendLevel.Minimum = 0;
            //slStudioSetPartEffectsReverbSendLevel.Maximum = 127;
            //slStudioSetPartEffectsReverbSendLevel.ValueChanged += slStudioSetPartEffectsReverbSendLevel_ValueChanged;
            //grid_0813.Margin = new Thickness(2, 0, 2, 2);
            //grid_0813.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0813.BorderThickness = new Thickness(1);
            //textblock_0817.Text = "Output assign";
            //cbStudioSetPartEffectsOutputAssign.SelectedIndex = 0;
            //cbStudioSetPartEffectsOutputAssign.SelectedIndexChanged += cbStudioSetPartEffectsOutputAssign_SelectionChanged;
            //StudioSetPartKeyboard.Margin = new Thickness(2, 0, 2, 2);
            ////StudioSetPartKeyboard.BorderThickness = new Thickness(1);
            //StudioSetPartKeyboard.IsVisible = false;
            //rowdefinition_0819.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0820.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0821.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0822.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0823.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0824.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0825.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0826.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0827.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0828.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0829.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0830.Height = new GridLength(1, GridUnitType.Star);
            //grid_0831.Margin = new Thickness(2, 2, 2, 2);
            //grid_0831.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0831.BorderThickness = new Thickness(1);
            //columndefinition_0833.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0834.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartKeyboardOctaveShift.Text = "Octave shift";
            //slStudioSetPartKeyboardOctaveShift.Minimum = -3;
            //slStudioSetPartKeyboardOctaveShift.Maximum = 3;
            //slStudioSetPartKeyboardOctaveShift.ValueChanged += slStudioSetPartKeyboardOctaveShift_ValueChanged;
            //grid_0835.Margin = new Thickness(2, 0, 2, 2);
            //grid_0835.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0835.BorderThickness = new Thickness(1);
            //columndefinition_0837.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0838.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartKeyboardVelocitySenseOffset.Text = "Velocity sense offset";
            //slStudioSetPartKeyboardVelocitySenseOffset.Minimum = -63;
            //slStudioSetPartKeyboardVelocitySenseOffset.Maximum = 63;
            //slStudioSetPartKeyboardVelocitySenseOffset.ValueChanged += slStudioSetPartKeyboardVelocitySenseOffset_ValueChanged;
            //grid_0839.Margin = new Thickness(2, 0, 2, 2);
            //grid_0839.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0839.BorderThickness = new Thickness(1);
            //columndefinition_0841.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0842.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartKeyboardRangeLower.Text = "Range lower";
            //slStudioSetPartKeyboardRangeLower.Minimum = 0;
            //slStudioSetPartKeyboardRangeLower.Maximum = 127;
            //slStudioSetPartKeyboardRangeLower.ValueChanged += slStudioSetPartKeyboardRangeLower_ValueChanged;
            //grid_0843.Margin = new Thickness(2, 0, 2, 2);
            //grid_0843.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0843.BorderThickness = new Thickness(1);
            //columndefinition_0845.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0846.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartKeyboardRangeUpper.Text = "Range upper";
            //slStudioSetPartKeyboardRangeUpper.Minimum = 0;
            //slStudioSetPartKeyboardRangeUpper.Maximum = 127;
            //slStudioSetPartKeyboardRangeUpper.ValueChanged += slStudioSetPartKeyboardRangeUpper_ValueChanged;
            //grid_0847.Margin = new Thickness(2, 0, 2, 2);
            //grid_0847.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0847.BorderThickness = new Thickness(1);
            //columndefinition_0849.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0850.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartKeyboardFadeWidthLower.Text = "Fade width lower";
            //slStudioSetPartKeyboardFadeWidthLower.Minimum = 0;
            //slStudioSetPartKeyboardFadeWidthLower.Maximum = 127;
            //slStudioSetPartKeyboardFadeWidthLower.ValueChanged += slStudioSetPartKeyboardFadeWidthLower_ValueChanged;
            //grid_0851.Margin = new Thickness(2, 0, 2, 2);
            //grid_0851.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0851.BorderThickness = new Thickness(1);
            //columndefinition_0853.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0854.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartKeyboardFadeWidthUpper.Text = "Fade width upper";
            //slStudioSetPartKeyboardFadeWidthUpper.Minimum = 0;
            //slStudioSetPartKeyboardFadeWidthUpper.Maximum = 127;
            //slStudioSetPartKeyboardFadeWidthUpper.ValueChanged += slStudioSetPartKeyboardFadeWidthUpper_ValueChanged;
            //grid_0855.Margin = new Thickness(2, 0, 2, 2);
            //grid_0855.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0855.BorderThickness = new Thickness(1);
            //columndefinition_0857.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0858.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartKeyboardVelocityRangeLower.Text = "Velocity range lower";
            //slStudioSetPartKeyboardVelocityRangeLower.Minimum = 0;
            //slStudioSetPartKeyboardVelocityRangeLower.Maximum = 127;
            //slStudioSetPartKeyboardVelocityRangeLower.ValueChanged += slStudioSetPartKeyboardVelocityRangeLower_ValueChanged;
            //grid_0859.Margin = new Thickness(2, 0, 2, 2);
            //grid_0859.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0859.BorderThickness = new Thickness(1);
            //columndefinition_0861.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0862.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartKeyboardVelocityRangeUpper.Text = "Velocity range upper";
            //slStudioSetPartKeyboardVelocityRangeUpper.Minimum = 0;
            //slStudioSetPartKeyboardVelocityRangeUpper.Maximum = 127;
            //slStudioSetPartKeyboardVelocityRangeUpper.ValueChanged += slStudioSetPartKeyboardVelocityRangeUpper_ValueChanged;
            //grid_0863.Margin = new Thickness(2, 0, 2, 2);
            //grid_0863.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0863.BorderThickness = new Thickness(1);
            //columndefinition_0865.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0866.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartKeyboardVelocityFadeWidthLower.Text = "Velocity fade width lower";
            //slStudioSetPartKeyboardVelocityFadeWidthLower.Minimum = 0;
            //slStudioSetPartKeyboardVelocityFadeWidthLower.Maximum = 127;
            //slStudioSetPartKeyboardVelocityFadeWidthLower.ValueChanged += slStudioSetPartKeyboardVelocityFadeWidthLower_ValueChanged;
            //grid_0867.Margin = new Thickness(2, 0, 2, 2);
            //grid_0867.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0867.BorderThickness = new Thickness(1);
            //columndefinition_0869.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0870.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartKeyboardVelocityFadeWidthUpper.Text = "Velocity fade width upper";
            //slStudioSetPartKeyboardVelocityFadeWidthUpper.Minimum = 0;
            //slStudioSetPartKeyboardVelocityFadeWidthUpper.Maximum = 127;
            //slStudioSetPartKeyboardVelocityFadeWidthUpper.ValueChanged += slStudioSetPartKeyboardVelocityFadeWidthUpper_ValueChanged;
            //grid_0871.Margin = new Thickness(2, 0, 2, 2);
            //grid_0871.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0871.BorderThickness = new Thickness(1);
            ////cbStudioSetPartKeyboardMute.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartKeyboardMute.Content = "Mute";
            //cbStudioSetPartKeyboardMute.Toggled += cbStudioSetPartKeyboard_Click;
            //grid_0872.Margin = new Thickness(2, 0, 2, 2);
            //grid_0872.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0872.BorderThickness = new Thickness(1);
            //textblock_0876.Text = "Velocity Curve Type";
            //cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = 0;
            //cbStudioSetPartKeyboardVelocityCurveType.SelectedIndexChanged += cbStudioSetPartKeyboardVelocityCurveType_SelectionChanged;
            //StudioSetPartScaleTune.Margin = new Thickness(2, 0, 2, 2);
            ////StudioSetPartScaleTune.BorderThickness = new Thickness(1);
            //StudioSetPartScaleTune.IsVisible = false;
            //rowdefinition_0878.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0879.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0880.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0881.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0882.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0883.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0884.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0885.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0886.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0887.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0888.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0889.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0890.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0891.Height = new GridLength(1, GridUnitType.Star);
            //grid_0892.Margin = new Thickness(2, 2, 2, 2);
            //grid_0892.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0892.BorderThickness = new Thickness(1);
            //textblock_0896.Text = "Type";
            //cbStudioSetPartScaleTuneType.SelectedIndex = 0;
            //cbStudioSetPartScaleTuneType.SelectedIndexChanged += cbStudioSetPartScaleTuneType_SelectionChanged;
            //grid_0897.Margin = new Thickness(2, 0, 2, 2);
            //grid_0897.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0897.BorderThickness = new Thickness(1);
            //textblock_0901.Text = "Key";
            //cbStudioSetPartScaleTuneKey.SelectedIndex = 0;
            //cbStudioSetPartScaleTuneKey.SelectedIndexChanged += cbStudioSetPartScaleTune_SelectionChanged;
            //grid_0902.Margin = new Thickness(2, 0, 2, 2);
            //grid_0902.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0902.BorderThickness = new Thickness(1);
            //columndefinition_0904.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0905.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartScaleTuneC.Text = "Tune for C";
            //slStudioSetPartScaleTuneC.Minimum = -64;
            //slStudioSetPartScaleTuneC.Maximum = 63;
            //slStudioSetPartScaleTuneC.ValueChanged += slStudioSetPartScaleTuneC_ValueChanged;
            //grid_0906.Margin = new Thickness(2, 0, 2, 2);
            //grid_0906.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0906.BorderThickness = new Thickness(1);
            //columndefinition_0908.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0909.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartScaleTuneCi.Text = "Tune for C#";
            //slStudioSetPartScaleTuneCi.Minimum = -64;
            //slStudioSetPartScaleTuneCi.Maximum = 63;
            //slStudioSetPartScaleTuneCi.ValueChanged += slStudioSetPartScaleTuneCi_ValueChanged;
            //grid_0910.Margin = new Thickness(2, 0, 2, 2);
            //grid_0910.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0910.BorderThickness = new Thickness(1);
            //columndefinition_0912.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0913.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartScaleTuneD.Text = "Tune for D";
            //slStudioSetPartScaleTuneD.Minimum = -64;
            //slStudioSetPartScaleTuneD.Maximum = 63;
            //slStudioSetPartScaleTuneD.ValueChanged += slStudioSetPartScaleTuneD_ValueChanged;
            //grid_0914.Margin = new Thickness(2, 0, 2, 2);
            //grid_0914.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0914.BorderThickness = new Thickness(1);
            //columndefinition_0916.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0917.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartScaleTuneDi.Text = "Tune for D#";
            //slStudioSetPartScaleTuneDi.Minimum = -64;
            //slStudioSetPartScaleTuneDi.Maximum = 63;
            //slStudioSetPartScaleTuneDi.ValueChanged += slStudioSetPartScaleTuneDi_ValueChanged;
            //grid_0918.Margin = new Thickness(2, 0, 2, 2);
            //grid_0918.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0918.BorderThickness = new Thickness(1);
            //columndefinition_0920.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0921.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartScaleTuneE.Text = "Tune for E";
            //slStudioSetPartScaleTuneE.Minimum = -64;
            //slStudioSetPartScaleTuneE.Maximum = 63;
            //slStudioSetPartScaleTuneE.ValueChanged += slStudioSetPartScaleTuneE_ValueChanged;
            //grid_0922.Margin = new Thickness(2, 0, 2, 2);
            //grid_0922.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0922.BorderThickness = new Thickness(1);
            //columndefinition_0924.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0925.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartScaleTuneF.Text = "Tune for F";
            //slStudioSetPartScaleTuneF.Minimum = -64;
            //slStudioSetPartScaleTuneF.Maximum = 63;
            //slStudioSetPartScaleTuneF.ValueChanged += slStudioSetPartScaleTuneF_ValueChanged;
            //grid_0926.Margin = new Thickness(2, 0, 2, 2);
            //grid_0926.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0926.BorderThickness = new Thickness(1);
            //columndefinition_0928.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0929.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartScaleTuneFi.Text = "Tune for F#";
            //slStudioSetPartScaleTuneFi.Minimum = -64;
            //slStudioSetPartScaleTuneFi.Maximum = 63;
            //slStudioSetPartScaleTuneFi.ValueChanged += slStudioSetPartScaleTuneFi_ValueChanged;
            //grid_0930.Margin = new Thickness(2, 0, 2, 2);
            //grid_0930.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0930.BorderThickness = new Thickness(1);
            //columndefinition_0932.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0933.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartScaleTuneG.Text = "Tune for G";
            //slStudioSetPartScaleTuneG.Minimum = -64;
            //slStudioSetPartScaleTuneG.Maximum = 63;
            //slStudioSetPartScaleTuneG.ValueChanged += slStudioSetPartScaleTuneG_ValueChanged;
            //grid_0934.Margin = new Thickness(2, 0, 2, 2);
            //grid_0934.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0934.BorderThickness = new Thickness(1);
            //columndefinition_0936.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0937.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartScaleTuneGi.Text = "Tune for G#";
            //slStudioSetPartScaleTuneGi.Minimum = -64;
            //slStudioSetPartScaleTuneGi.Maximum = 63;
            //slStudioSetPartScaleTuneGi.ValueChanged += slStudioSetPartScaleTuneGi_ValueChanged;
            //grid_0938.Margin = new Thickness(2, 0, 2, 2);
            //grid_0938.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0938.BorderThickness = new Thickness(1);
            //columndefinition_0940.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0941.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartScaleTuneA.Text = "Tune for A";
            //slStudioSetPartScaleTuneA.Minimum = -64;
            //slStudioSetPartScaleTuneA.Maximum = 63;
            //slStudioSetPartScaleTuneA.ValueChanged += slStudioSetPartScaleTuneA_ValueChanged;
            //grid_0942.Margin = new Thickness(2, 0, 2, 2);
            //grid_0942.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0942.BorderThickness = new Thickness(1);
            //columndefinition_0944.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0945.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartScaleTuneAi.Text = "Tune for A#";
            //slStudioSetPartScaleTuneAi.Minimum = -64;
            //slStudioSetPartScaleTuneAi.Maximum = 63;
            //slStudioSetPartScaleTuneAi.ValueChanged += slStudioSetPartScaleTuneAi_ValueChanged;
            //grid_0946.Margin = new Thickness(2, 0, 2, 2);
            //grid_0946.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0946.BorderThickness = new Thickness(1);
            //columndefinition_0948.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0949.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartScaleTuneB.Text = "Tune for B";
            //slStudioSetPartScaleTuneB.Minimum = -64;
            //slStudioSetPartScaleTuneB.Maximum = 63;
            //slStudioSetPartScaleTuneB.ValueChanged += slStudioSetPartScaleTuneB_ValueChanged;
            //StudioSetPartMidi.Margin = new Thickness(2, 0, 2, 2);
            ////StudioSetPartMidi.BorderThickness = new Thickness(1);
            //StudioSetPartMidi.IsVisible = false;
            //rowdefinition_0951.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0952.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0953.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0954.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0955.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0956.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0957.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0958.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0959.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0960.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0961.Height = new GridLength(1, GridUnitType.Star);
            //grid_0962.Margin = new Thickness(2, 2, 2, 2);
            //grid_0962.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0962.BorderThickness = new Thickness(1);
            ////cbStudioSetPartMidiPhaseLock.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartMidiPhaseLock.Content = "Phase lock";
            //cbStudioSetPartMidiPhaseLock.Toggled += cbStudioSetPartMidiPhaseLock_Click;
            //grid_0963.Margin = new Thickness(2, 0, 2, 2);
            //grid_0963.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0963.BorderThickness = new Thickness(1);
            ////cbStudioSetPartMidiReceiveProgramChange.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartMidiReceiveProgramChange.Content = "Receive program change";
            //cbStudioSetPartMidiReceiveProgramChange.Toggled += cbStudioSetPartMidiReceiveProgramChange_Click;
            //grid_0964.Margin = new Thickness(2, 0, 2, 2);
            //grid_0964.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0964.BorderThickness = new Thickness(1);
            ////cbStudioSetPartMidiReceiveBankSelect.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartMidiReceiveBankSelect.Content = "Receive bank select";
            //cbStudioSetPartMidiReceiveBankSelect.Toggled += cbStudioSetPartMidiReceiveBankSelect_Click;
            //grid_0965.Margin = new Thickness(2, 0, 2, 2);
            //grid_0965.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0965.BorderThickness = new Thickness(1);
            ////cbStudioSetPartMidiReceivePitchBend.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartMidiReceivePitchBend.Content = "Receive pitch bend";
            //cbStudioSetPartMidiReceivePitchBend.Toggled += cbStudioSetPartMidiReceivePitchBend_Click;
            //grid_0966.Margin = new Thickness(2, 0, 2, 2);
            //grid_0966.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0966.BorderThickness = new Thickness(1);
            ////cbStudioSetPartMidiReceivePolyphonicKeyPressure.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartMidiReceivePolyphonicKeyPressure.Content = "Receive polyphonic key pressure";
            //cbStudioSetPartMidiReceivePolyphonicKeyPressure.Toggled += cbStudioSetPartMidiReceivePolyphonicKeyPressure_Click;
            //grid_0967.Margin = new Thickness(2, 0, 2, 2);
            //grid_0967.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0967.BorderThickness = new Thickness(1);
            ////cbStudioSetPartMidiReceiveChannelPressure.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartMidiReceiveChannelPressure.Content = "Receive channel pressure";
            //cbStudioSetPartMidiReceiveChannelPressure.Toggled += cbStudioSetPartMidiReceiveChannelPressure_Click;
            //grid_0968.Margin = new Thickness(2, 0, 2, 2);
            //grid_0968.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0968.BorderThickness = new Thickness(1);
            ////cbStudioSetPartMidiReceiveModulation.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartMidiReceiveModulation.Content = "Receive modulation";
            //cbStudioSetPartMidiReceiveModulation.Toggled += cbStudioSetPartMidiReceiveModulation_Click;
            //grid_0969.Margin = new Thickness(2, 0, 2, 2);
            //grid_0969.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0969.BorderThickness = new Thickness(1);
            ////cbStudioSetPartMidiReceiveVolume.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartMidiReceiveVolume.Content = "Receive volume";
            //cbStudioSetPartMidiReceiveVolume.Toggled += cbStudioSetPartMidiReceiveVolume_Click;
            //grid_0970.Margin = new Thickness(2, 0, 2, 2);
            //grid_0970.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0970.BorderThickness = new Thickness(1);
            ////cbStudioSetPartMidiReceivePan.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartMidiReceivePan.Content = "Receive pan";
            //cbStudioSetPartMidiReceivePan.Toggled += cbStudioSetPartMidiReceivePan_Click;
            //grid_0971.Margin = new Thickness(2, 0, 2, 2);
            //grid_0971.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0971.BorderThickness = new Thickness(1);
            ////cbStudioSetPartMidiReceiveExpression.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartMidiReceiveExpression.Content = "Receive expression";
            //cbStudioSetPartMidiReceiveExpression.Toggled += cbStudioSetPartMidiReceiveExpression_Click;
            //grid_0972.Margin = new Thickness(2, 0, 2, 2);
            //grid_0972.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0972.BorderThickness = new Thickness(1);
            ////cbStudioSetPartMidiReceiveHold1.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartMidiReceiveHold1.Content = "Receive hold-1";
            //cbStudioSetPartMidiReceiveHold1.Toggled += cbStudioSetPartMidiReceiveHold1_Click;
            //StudioSetPartMotionalSurround.Margin = new Thickness(2, 0, 2, 2);
            ////StudioSetPartMotionalSurround.BorderThickness = new Thickness(1);
            //StudioSetPartMotionalSurround.IsVisible = false;
            //rowdefinition_0974.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0975.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0976.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0977.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0978.Height = new GridLength(10, GridUnitType.Star);
            //grid_0979.Margin = new Thickness(2, 2, 2, 2);
            //grid_0979.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0979.BorderThickness = new Thickness(1);
            //columndefinition_0981.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0982.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartMotionalSurroundLR.Text = "Motional surround L-R";
            //slStudioSetPartMotionalSurroundLR.Minimum = -64;
            //slStudioSetPartMotionalSurroundLR.Maximum = 63;
            //slStudioSetPartMotionalSurroundLR.ValueChanged += slStudioSetPartMotionalSurroundLR_ValueChanged;
            //grid_0983.Margin = new Thickness(2, 0, 2, 2);
            //grid_0983.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0983.BorderThickness = new Thickness(1);
            //columndefinition_0985.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0986.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartMotionalSurroundFB.Text = "Motional surround F-B";
            //slStudioSetPartMotionalSurroundFB.Minimum = -64;
            //slStudioSetPartMotionalSurroundFB.Maximum = 63;
            //slStudioSetPartMotionalSurroundFB.ValueChanged += slStudioSetPartMotionalSurroundFB_ValueChanged;
            //grid_0987.Margin = new Thickness(2, 0, 2, 2);
            //grid_0987.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0987.BorderThickness = new Thickness(1);
            //columndefinition_0989.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0990.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartMotionalSurroundWidth.Text = "Width";
            //slStudioSetPartMotionalSurroundWidth.Minimum = 0;
            //slStudioSetPartMotionalSurroundWidth.Maximum = 32;
            //slStudioSetPartMotionalSurroundWidth.ValueChanged += slStudioSetPartMotionalSurroundWidth_ValueChanged;
            //grid_0991.Margin = new Thickness(2, 0, 2, 2);
            //grid_0991.Padding = new Thickness(4, 0, 0, 0);
            ////grid_0991.BorderThickness = new Thickness(1);
            //columndefinition_0993.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_0994.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartMotionalSurroundAmbienceSendLevel.Text = "Ambience send level";
            //slStudioSetPartMotionalSurroundAmbienceSendLevel.Minimum = 0;
            //slStudioSetPartMotionalSurroundAmbienceSendLevel.Maximum = 127;
            //slStudioSetPartMotionalSurroundAmbienceSendLevel.ValueChanged += slStudioSetPartMotionalSurroundAmbienceSendLevel_ValueChanged;
            //StudioSetPartEQ.Margin = new Thickness(2, 0, 2, 2);
            ////StudioSetPartEQ.BorderThickness = new Thickness(1);
            //StudioSetPartEQ.IsVisible = false;
            //rowdefinition_0996.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0997.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0998.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_0999.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_1000.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_1001.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_1002.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_1003.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_1004.Height = new GridLength(6, GridUnitType.Star);
            //grid_1005.Margin = new Thickness(2, 2, 2, 2);
            //grid_1005.Padding = new Thickness(4, 0, 0, 0);
            ////grid_1005.BorderThickness = new Thickness(1);
            ////cbStudioSetPartEQSwitch.Padding = new Thickness(4, 5, 0, 0);
            //cbStudioSetPartEQSwitch.Content = "Equalizer on/off";
            //cbStudioSetPartEQSwitch.Toggled += cbStudioSetPartEQ_Click;
            //grid_1006.Margin = new Thickness(2, 0, 2, 2);
            //grid_1006.Padding = new Thickness(4, 0, 0, 0);
            ////grid_1006.BorderThickness = new Thickness(1);
            //textblock_1010.Text = "EQ low freq";
            //cbStudioSetPartEQLoqFreq.SelectedIndex = 0;
            //cbStudioSetPartEQLoqFreq.SelectedIndexChanged += cbStudioSetPartEQLoqFreq_SelectionChanged;
            //grid_1011.Margin = new Thickness(2, 0, 2, 2);
            //grid_1011.Padding = new Thickness(4, 0, 0, 0);
            ////grid_1011.BorderThickness = new Thickness(1);
            //columndefinition_1013.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_1014.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartEQLowGain.Text = "EQ low gain";
            //slStudioSetPartEQLowGain.Minimum = -15;
            //slStudioSetPartEQLowGain.Maximum = 15;
            //slStudioSetPartEQLowGain.ValueChanged += slStudioSetPartEQLowGain_ValueChanged;
            //columndefinition_1017.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_1018.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartEQMidFreq.Text = "EQ mid freq";
            ////tbStudioSetPartEQMidFreq.Padding = new Thickness(4, 0, 4, 0);
            ////tbStudioSetPartEQMidFreq.Grid.ColumnSpan = 2;
            //tbStudioSetPartEQMidFreq.Margin = new Thickness(0, 6, 110.2, 7);
            ////cbStudioSetPartEQMidFreq.Padding = new Thickness(4, 0, 4, 0);
            //cbStudioSetPartEQMidFreq.SelectedIndexChanged += cbStudioSetPartEQMidFreq_SelectionChanged;
            //grid_1019.Margin = new Thickness(2, 0, 2, 2);
            //grid_1019.Padding = new Thickness(4, 0, 0, 0);
            ////grid_1019.BorderThickness = new Thickness(1);
            //columndefinition_1021.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_1022.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartEQMidGain.Text = "EQ mid gain";
            //slStudioSetPartEQMidGain.Minimum = -15;
            //slStudioSetPartEQMidGain.Maximum = 15;
            //slStudioSetPartEQMidGain.ValueChanged += slStudioSetPartEQMidGain_ValueChanged;
            //columndefinition_1025.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_1026.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartEQMidQ.Text = "EQ mid Q";
            ////tbStudioSetPartEQMidQ.Padding = new Thickness(4, 0, 4, 0);
            //cbStudioSetPartEQMidQ.SelectedIndexChanged += cbStudioSetPartEQMidQ_SelectionChanged;
            //columndefinition_1029.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_1030.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartEQHighFreq.Text = "EQ high freq";
            ////tbStudioSetPartEQHighFreq.Padding = new Thickness(4, 0, 4, 0);
            //cbStudioSetPartEQHighFreq.SelectedIndexChanged += cbStudioSetPartEQHighFreq_SelectionChanged;
            //columndefinition_1033.Width = new GridLength(1, GridUnitType.Star);
            //columndefinition_1034.Width = new GridLength(1, GridUnitType.Star);
            //tbStudioSetPartEQHighGain.Text = "EQ high gain";
            ////tbStudioSetPartEQHighGain.Padding = new Thickness(4, 0, 4, 0);
            //slStudioSetPartEQHighGain.Minimum = -15;
            //slStudioSetPartEQHighGain.Maximum = 15;
            //slStudioSetPartEQHighGain.ValueChanged += slStudioSetPartEQHighGain_ValueChanged;
            //Buttons.Margin = new Thickness(2, 0, 2, 2);
            ////Buttons.BorderThickness = new Thickness(1);
            //Buttons.IsVisible = true;
            //rowdefinition_1036.Height = new GridLength(1, GridUnitType.Star);
            //rowdefinition_1037.Height = new GridLength(1, GridUnitType.Star);
            //columndefinition_1040.Width = new GridLength(5, GridUnitType.Star);
            //columndefinition_1041.Width = new GridLength(3, GridUnitType.Star);
            //columndefinition_1042.Width = new GridLength(8, GridUnitType.Star);
            //cbStudioSetSlot.SelectedIndexChanged += cbStudioSetSlot_SelectionChanged;
            //textblock_1043.Text = "Name:";
            //textblock_1043.Margin = new Thickness(2, 0, 2, 2);
            ////textblock_1043.Padding = new Thickness(4, 0, 0, 0);
            //tbStudioSetName.TextChanged += tbStudioSetName_KeyUp;
            //columndefinition_1046.Width = new GridLength(3, GridUnitType.Star);
            //columndefinition_1047.Width = new GridLength(3, GridUnitType.Star);
            //columndefinition_1048.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_1049.Width = new GridLength(2, GridUnitType.Star);
            //columndefinition_1050.Width = new GridLength(2, GridUnitType.Star);
            btnFileSave.Content = "Save file";
            //btnFileSave.Clicked += btnFileSave_Click;
            btnFileLoad.Content = "Load file";
            //btnFileLoad.Clicked += btnFileLoad_Click;
            //btnStudioSetSave.Content = "Save";
            //btnStudioSetSave.Clicked += btnStudioSetSave_Click;
            btnStudioSetDelete.Content = "Delete";
            //btnStudioSetDelete.Clicked += btnStudioSetDelete_Click;
            btnStudioSetReturn.Content = "Return";
            //btnStudioSetReturn.Clicked += btnStudioSetReturn_Click;
            ////btnStudioSetReturn.Grid.ColumnSpan = 3;
            //btnStudioSetReturn.Margin = new Thickness(0, 0, 0.333, 0);
            //grid_MainStudioSet.ColumnDefinitions.Add(columndefinition_0004);
            //grid_MainStudioSet.ColumnDefinitions.Add(columndefinition_0005);
            //grid_MainStudioSet.ColumnDefinitions.Add(columndefinition_0006);
            //grid_ToneControl1.ColumnDefinitions.Add(columndefinition_0032);
            //grid_ToneControl1.ColumnDefinitions.Add(columndefinition_0033);
            //grid_ToneControl2.ColumnDefinitions.Add(columndefinition_0038);
            //grid_ToneControl2.ColumnDefinitions.Add(columndefinition_0039);
            //grid_ToneControl3.ColumnDefinitions.Add(columndefinition_0044);
            //grid_ToneControl3.ColumnDefinitions.Add(columndefinition_0045);
            //grid_ToneControl4.ColumnDefinitions.Add(columndefinition_0050);
            //grid_ToneControl4.ColumnDefinitions.Add(columndefinition_0051);
            //grid_Tempo.ColumnDefinitions.Add(columndefinition_0056);
            //grid_Tempo.ColumnDefinitions.Add(columndefinition_0057);
            //grid_SoloPart.ColumnDefinitions.Add(columndefinition_0061);
            //grid_SoloPart.ColumnDefinitions.Add(columndefinition_0062);
            //grid_0065.ColumnDefinitions.Add(columndefinition_0067);
            //grid_0065.ColumnDefinitions.Add(columndefinition_0068);
            //grid_0065.ColumnDefinitions.Add(columndefinition_0069);
            //grid_0071.ColumnDefinitions.Add(columndefinition_0073);
            //grid_0071.ColumnDefinitions.Add(columndefinition_0074);
            //grid_0077.ColumnDefinitions.Add(columndefinition_0079);
            //grid_0077.ColumnDefinitions.Add(columndefinition_0080);
            //grid_0083.ColumnDefinitions.Add(columndefinition_0085);
            //grid_0083.ColumnDefinitions.Add(columndefinition_0086);
            //grid_0089.ColumnDefinitions.Add(columndefinition_0091);
            //grid_0089.ColumnDefinitions.Add(columndefinition_0092);
            //grid_0095.ColumnDefinitions.Add(columndefinition_0097);
            //grid_0095.ColumnDefinitions.Add(columndefinition_0098);
            //grid_0101.ColumnDefinitions.Add(columndefinition_0103);
            //grid_0101.ColumnDefinitions.Add(columndefinition_0104);
            //grid_0107.ColumnDefinitions.Add(columndefinition_0109);
            //grid_0107.ColumnDefinitions.Add(columndefinition_0110);
            //grid_0112.ColumnDefinitions.Add(columndefinition_0114);
            //grid_0112.ColumnDefinitions.Add(columndefinition_0115);
            //grid_0116.ColumnDefinitions.Add(columndefinition_0118);
            //grid_0116.ColumnDefinitions.Add(columndefinition_0119);
            //grid_0120.ColumnDefinitions.Add(columndefinition_0122);
            //grid_0120.ColumnDefinitions.Add(columndefinition_0123);
            //grid_0124.ColumnDefinitions.Add(columndefinition_0126);
            //grid_0124.ColumnDefinitions.Add(columndefinition_0127);
            //grid_0151.ColumnDefinitions.Add(columndefinition_0153);
            //grid_0151.ColumnDefinitions.Add(columndefinition_0154);
            //grid_0155.ColumnDefinitions.Add(columndefinition_0157);
            //grid_0155.ColumnDefinitions.Add(columndefinition_0158);
            //grid_0159.ColumnDefinitions.Add(columndefinition_0161);
            //grid_0159.ColumnDefinitions.Add(columndefinition_0162);
            //grid_0164.ColumnDefinitions.Add(columndefinition_0166);
            //grid_0164.ColumnDefinitions.Add(columndefinition_0167);
            //grid_0169.ColumnDefinitions.Add(columndefinition_0171);
            //grid_0169.ColumnDefinitions.Add(columndefinition_0172);
            //grid_0173.ColumnDefinitions.Add(columndefinition_0175);
            //grid_0173.ColumnDefinitions.Add(columndefinition_0176);
            //grid_0177.ColumnDefinitions.Add(columndefinition_0179);
            //grid_0177.ColumnDefinitions.Add(columndefinition_0180);
            //grid_0181.ColumnDefinitions.Add(columndefinition_0183);
            //grid_0181.ColumnDefinitions.Add(columndefinition_0184);
            //grid_0185.ColumnDefinitions.Add(columndefinition_0187);
            //grid_0185.ColumnDefinitions.Add(columndefinition_0188);
            //grid_0190.ColumnDefinitions.Add(columndefinition_0192);
            //grid_0190.ColumnDefinitions.Add(columndefinition_0193);
            //grid_0195.ColumnDefinitions.Add(columndefinition_0197);
            //grid_0195.ColumnDefinitions.Add(columndefinition_0198);
            //grid_0200.ColumnDefinitions.Add(columndefinition_0202);
            //grid_0200.ColumnDefinitions.Add(columndefinition_0203);
            //grid_0209.ColumnDefinitions.Add(columndefinition_0211);
            //grid_0209.ColumnDefinitions.Add(columndefinition_0212);
            //grid_0231.ColumnDefinitions.Add(columndefinition_0233);
            //grid_0231.ColumnDefinitions.Add(columndefinition_0234);
            //grid_0236.ColumnDefinitions.Add(columndefinition_0238);
            //grid_0236.ColumnDefinitions.Add(columndefinition_0239);
            //grid_0241.ColumnDefinitions.Add(columndefinition_0243);
            //grid_0241.ColumnDefinitions.Add(columndefinition_0244);
            //grid_0246.ColumnDefinitions.Add(columndefinition_0248);
            //grid_0246.ColumnDefinitions.Add(columndefinition_0249);
            //grid_0251.ColumnDefinitions.Add(columndefinition_0253);
            //grid_0251.ColumnDefinitions.Add(columndefinition_0254);
            //grid_0256.ColumnDefinitions.Add(columndefinition_0258);
            //grid_0256.ColumnDefinitions.Add(columndefinition_0259);
            //grid_0261.ColumnDefinitions.Add(columndefinition_0263);
            //grid_0261.ColumnDefinitions.Add(columndefinition_0264);
            //grid_0266.ColumnDefinitions.Add(columndefinition_0268);
            //grid_0266.ColumnDefinitions.Add(columndefinition_0269);
            //grid_0271.ColumnDefinitions.Add(columndefinition_0273);
            //grid_0271.ColumnDefinitions.Add(columndefinition_0274);
            //grid_0276.ColumnDefinitions.Add(columndefinition_0278);
            //grid_0276.ColumnDefinitions.Add(columndefinition_0279);
            //grid_0281.ColumnDefinitions.Add(columndefinition_0283);
            //grid_0281.ColumnDefinitions.Add(columndefinition_0284);
            //grid_0286.ColumnDefinitions.Add(columndefinition_0288);
            //grid_0286.ColumnDefinitions.Add(columndefinition_0289);
            //grid_0291.ColumnDefinitions.Add(columndefinition_0293);
            //grid_0291.ColumnDefinitions.Add(columndefinition_0294);
            //grid_0296.ColumnDefinitions.Add(columndefinition_0298);
            //grid_0296.ColumnDefinitions.Add(columndefinition_0299);
            //grid_0301.ColumnDefinitions.Add(columndefinition_0303);
            //grid_0301.ColumnDefinitions.Add(columndefinition_0304);
            //grid_0306.ColumnDefinitions.Add(columndefinition_0308);
            //grid_0306.ColumnDefinitions.Add(columndefinition_0309);
            //grid_0317.ColumnDefinitions.Add(columndefinition_0319);
            //grid_0317.ColumnDefinitions.Add(columndefinition_0320);
            //grid_0322.ColumnDefinitions.Add(columndefinition_0324);
            //grid_0322.ColumnDefinitions.Add(columndefinition_0325);
            //grid_0326.ColumnDefinitions.Add(columndefinition_0328);
            //grid_0326.ColumnDefinitions.Add(columndefinition_0329);
            //grid_0331.ColumnDefinitions.Add(columndefinition_0333);
            //grid_0331.ColumnDefinitions.Add(columndefinition_0334);
            //grid_0346.ColumnDefinitions.Add(columndefinition_0348);
            //grid_0346.ColumnDefinitions.Add(columndefinition_0349);
            //grid_0350.ColumnDefinitions.Add(columndefinition_0352);
            //grid_0350.ColumnDefinitions.Add(columndefinition_0353);
            //grid_0354.ColumnDefinitions.Add(columndefinition_0356);
            //grid_0354.ColumnDefinitions.Add(columndefinition_0357);
            //grid_0358.ColumnDefinitions.Add(columndefinition_0360);
            //grid_0358.ColumnDefinitions.Add(columndefinition_0361);
            //ChorusRateHz.ColumnDefinitions.Add(columndefinition_0363);
            //ChorusRateHz.ColumnDefinitions.Add(columndefinition_0364);
            //ChorusRateNote.ColumnDefinitions.Add(columndefinition_0366);
            //ChorusRateNote.ColumnDefinitions.Add(columndefinition_0367);
            //ChorusDepth.ColumnDefinitions.Add(columndefinition_0369);
            //ChorusDepth.ColumnDefinitions.Add(columndefinition_0370);
            //grid_0371.ColumnDefinitions.Add(columndefinition_0373);
            //grid_0371.ColumnDefinitions.Add(columndefinition_0374);
            //grid_0375.ColumnDefinitions.Add(columndefinition_0377);
            //grid_0375.ColumnDefinitions.Add(columndefinition_0378);
            //grid_0392.ColumnDefinitions.Add(columndefinition_0394);
            //grid_0392.ColumnDefinitions.Add(columndefinition_0395);
            //ChorusDelayLeftHz.ColumnDefinitions.Add(columndefinition_0398);
            //ChorusDelayLeftHz.ColumnDefinitions.Add(columndefinition_0399);
            //ChorusDelayLeftNote.ColumnDefinitions.Add(columndefinition_0401);
            //ChorusDelayLeftNote.ColumnDefinitions.Add(columndefinition_0402);
            //grid_0403.ColumnDefinitions.Add(columndefinition_0405);
            //grid_0403.ColumnDefinitions.Add(columndefinition_0406);
            //ChorusDelayRightHz.ColumnDefinitions.Add(columndefinition_0409);
            //ChorusDelayRightHz.ColumnDefinitions.Add(columndefinition_0410);
            //ChorusDelayRightNote.ColumnDefinitions.Add(columndefinition_0412);
            //ChorusDelayRightNote.ColumnDefinitions.Add(columndefinition_0413);
            //grid_0414.ColumnDefinitions.Add(columndefinition_0416);
            //grid_0414.ColumnDefinitions.Add(columndefinition_0417);
            //ChorusDelayCenterHz.ColumnDefinitions.Add(columndefinition_0420);
            //ChorusDelayCenterHz.ColumnDefinitions.Add(columndefinition_0421);
            //ChorusDelayCenterNote.ColumnDefinitions.Add(columndefinition_0423);
            //ChorusDelayCenterNote.ColumnDefinitions.Add(columndefinition_0424);
            //grid_0425.ColumnDefinitions.Add(columndefinition_0427);
            //grid_0425.ColumnDefinitions.Add(columndefinition_0428);
            //grid_0429.ColumnDefinitions.Add(columndefinition_0431);
            //grid_0429.ColumnDefinitions.Add(columndefinition_0432);
            //grid_0433.ColumnDefinitions.Add(columndefinition_0435);
            //grid_0433.ColumnDefinitions.Add(columndefinition_0436);
            //grid_0437.ColumnDefinitions.Add(columndefinition_0439);
            //grid_0437.ColumnDefinitions.Add(columndefinition_0440);
            //grid_0441.ColumnDefinitions.Add(columndefinition_0443);
            //grid_0441.ColumnDefinitions.Add(columndefinition_0444);
            //ChorusGM2ChorusPreLPF.ColumnDefinitions.Add(columndefinition_0455);
            //ChorusGM2ChorusPreLPF.ColumnDefinitions.Add(columndefinition_0456);
            //ChorusGM2ChorusLevel.ColumnDefinitions.Add(columndefinition_0458);
            //ChorusGM2ChorusLevel.ColumnDefinitions.Add(columndefinition_0459);
            //ChorusGM2ChorusFeedback.ColumnDefinitions.Add(columndefinition_0461);
            //ChorusGM2ChorusFeedback.ColumnDefinitions.Add(columndefinition_0462);
            //ChorusGM2ChorusDelay.ColumnDefinitions.Add(columndefinition_0464);
            //ChorusGM2ChorusDelay.ColumnDefinitions.Add(columndefinition_0465);
            //ChorusGM2ChorusRate.ColumnDefinitions.Add(columndefinition_0467);
            //ChorusGM2ChorusRate.ColumnDefinitions.Add(columndefinition_0468);
            //ChorusGM2ChorusDepth.ColumnDefinitions.Add(columndefinition_0470);
            //ChorusGM2ChorusDepth.ColumnDefinitions.Add(columndefinition_0471);
            //ChorusGM2ChorusSendLevelToReverb.ColumnDefinitions.Add(columndefinition_0473);
            //ChorusGM2ChorusSendLevelToReverb.ColumnDefinitions.Add(columndefinition_0474);
            //grid_0480.ColumnDefinitions.Add(columndefinition_0482);
            //grid_0480.ColumnDefinitions.Add(columndefinition_0483);
            //grid_0485.ColumnDefinitions.Add(columndefinition_0487);
            //grid_0485.ColumnDefinitions.Add(columndefinition_0488);
            //grid_0489.ColumnDefinitions.Add(columndefinition_0491);
            //grid_0489.ColumnDefinitions.Add(columndefinition_0492);
            //grid_0503.ColumnDefinitions.Add(columndefinition_0505);
            //grid_0503.ColumnDefinitions.Add(columndefinition_0506);
            //grid_0507.ColumnDefinitions.Add(columndefinition_0509);
            //grid_0507.ColumnDefinitions.Add(columndefinition_0510);
            //grid_0511.ColumnDefinitions.Add(columndefinition_0513);
            //grid_0511.ColumnDefinitions.Add(columndefinition_0514);
            //grid_0515.ColumnDefinitions.Add(columndefinition_0517);
            //grid_0515.ColumnDefinitions.Add(columndefinition_0518);
            //grid_0519.ColumnDefinitions.Add(columndefinition_0521);
            //grid_0519.ColumnDefinitions.Add(columndefinition_0522);
            //grid_0523.ColumnDefinitions.Add(columndefinition_0525);
            //grid_0523.ColumnDefinitions.Add(columndefinition_0526);
            //grid_0527.ColumnDefinitions.Add(columndefinition_0529);
            //grid_0527.ColumnDefinitions.Add(columndefinition_0530);
            //grid_0531.ColumnDefinitions.Add(columndefinition_0533);
            //grid_0531.ColumnDefinitions.Add(columndefinition_0534);
            //grid_0539.ColumnDefinitions.Add(columndefinition_0541);
            //grid_0539.ColumnDefinitions.Add(columndefinition_0542);
            //grid_0543.ColumnDefinitions.Add(columndefinition_0545);
            //grid_0543.ColumnDefinitions.Add(columndefinition_0546);
            //grid_0567.ColumnDefinitions.Add(columndefinition_0569);
            //grid_0567.ColumnDefinitions.Add(columndefinition_0570);
            //grid_0572.ColumnDefinitions.Add(columndefinition_0574);
            //grid_0572.ColumnDefinitions.Add(columndefinition_0575);
            //grid_0576.ColumnDefinitions.Add(columndefinition_0578);
            //grid_0576.ColumnDefinitions.Add(columndefinition_0579);
            //grid_0581.ColumnDefinitions.Add(columndefinition_0583);
            //grid_0581.ColumnDefinitions.Add(columndefinition_0584);
            //grid_0585.ColumnDefinitions.Add(columndefinition_0587);
            //grid_0585.ColumnDefinitions.Add(columndefinition_0588);
            //grid_0589.ColumnDefinitions.Add(columndefinition_0591);
            //grid_0589.ColumnDefinitions.Add(columndefinition_0592);
            //grid_0593.ColumnDefinitions.Add(columndefinition_0595);
            //grid_0593.ColumnDefinitions.Add(columndefinition_0596);
            //grid_0597.ColumnDefinitions.Add(columndefinition_0599);
            //grid_0597.ColumnDefinitions.Add(columndefinition_0600);
            //grid_0601.ColumnDefinitions.Add(columndefinition_0603);
            //grid_0601.ColumnDefinitions.Add(columndefinition_0604);
            //grid_0605.ColumnDefinitions.Add(columndefinition_0607);
            //grid_0605.ColumnDefinitions.Add(columndefinition_0608);
            //grid_0609.ColumnDefinitions.Add(columndefinition_0611);
            //grid_0609.ColumnDefinitions.Add(columndefinition_0612);
            //grid_0614.ColumnDefinitions.Add(columndefinition_0616);
            //grid_0614.ColumnDefinitions.Add(columndefinition_0617);
            //grid_0643.ColumnDefinitions.Add(columndefinition_0645);
            //grid_0643.ColumnDefinitions.Add(columndefinition_0646);
            //grid_0647.ColumnDefinitions.Add(columndefinition_0649);
            //grid_0647.ColumnDefinitions.Add(columndefinition_0650);
            //grid_0651.ColumnDefinitions.Add(columndefinition_0653);
            //grid_0651.ColumnDefinitions.Add(columndefinition_0654);
            //grid_0655.ColumnDefinitions.Add(columndefinition_0657);
            //grid_0655.ColumnDefinitions.Add(columndefinition_0658);
            //grid_0659.ColumnDefinitions.Add(columndefinition_0661);
            //grid_0659.ColumnDefinitions.Add(columndefinition_0662);
            //grid_0663.ColumnDefinitions.Add(columndefinition_0665);
            //grid_0663.ColumnDefinitions.Add(columndefinition_0666);
            //grid_0667.ColumnDefinitions.Add(columndefinition_0669);
            //grid_0667.ColumnDefinitions.Add(columndefinition_0670);
            //grid_0695.ColumnDefinitions.Add(columndefinition_0697);
            //grid_0695.ColumnDefinitions.Add(columndefinition_0698);
            //grid_0699.ColumnDefinitions.Add(columndefinition_0701);
            //grid_0699.ColumnDefinitions.Add(columndefinition_0702);
            //grid_0703.ColumnDefinitions.Add(columndefinition_0705);
            //grid_0703.ColumnDefinitions.Add(columndefinition_0706);
            //grid_0703.ColumnDefinitions.Add(columndefinition_0707);
            //grid_0708.ColumnDefinitions.Add(columndefinition_0710);
            //grid_0708.ColumnDefinitions.Add(columndefinition_0711);
            //grid_0712.ColumnDefinitions.Add(columndefinition_0714);
            //grid_0712.ColumnDefinitions.Add(columndefinition_0715);
            //grid_0716.ColumnDefinitions.Add(columndefinition_0718);
            //grid_0716.ColumnDefinitions.Add(columndefinition_0719);
            //grid_0720.ColumnDefinitions.Add(columndefinition_0722);
            //grid_0720.ColumnDefinitions.Add(columndefinition_0723);
            //grid_0724.ColumnDefinitions.Add(columndefinition_0726);
            //grid_0724.ColumnDefinitions.Add(columndefinition_0727);
            //grid_0728.ColumnDefinitions.Add(columndefinition_0730);
            //grid_0728.ColumnDefinitions.Add(columndefinition_0731);
            //grid_0732.ColumnDefinitions.Add(columndefinition_0734);
            //grid_0732.ColumnDefinitions.Add(columndefinition_0735);
            //grid_0737.ColumnDefinitions.Add(columndefinition_0739);
            //grid_0737.ColumnDefinitions.Add(columndefinition_0740);
            //grid_0742.ColumnDefinitions.Add(columndefinition_0744);
            //grid_0742.ColumnDefinitions.Add(columndefinition_0745);
            //grid_0746.ColumnDefinitions.Add(columndefinition_0748);
            //grid_0746.ColumnDefinitions.Add(columndefinition_0749);
            //grid_0751.ColumnDefinitions.Add(columndefinition_0753);
            //grid_0751.ColumnDefinitions.Add(columndefinition_0754);
            //grid_0773.ColumnDefinitions.Add(columndefinition_0775);
            //grid_0773.ColumnDefinitions.Add(columndefinition_0776);
            //grid_0777.ColumnDefinitions.Add(columndefinition_0779);
            //grid_0777.ColumnDefinitions.Add(columndefinition_0780);
            //grid_0781.ColumnDefinitions.Add(columndefinition_0783);
            //grid_0781.ColumnDefinitions.Add(columndefinition_0784);
            //grid_0785.ColumnDefinitions.Add(columndefinition_0787);
            //grid_0785.ColumnDefinitions.Add(columndefinition_0788);
            //grid_0789.ColumnDefinitions.Add(columndefinition_0791);
            //grid_0789.ColumnDefinitions.Add(columndefinition_0792);
            //grid_0793.ColumnDefinitions.Add(columndefinition_0795);
            //grid_0793.ColumnDefinitions.Add(columndefinition_0796);
            //grid_0797.ColumnDefinitions.Add(columndefinition_0799);
            //grid_0797.ColumnDefinitions.Add(columndefinition_0800);
            //grid_0801.ColumnDefinitions.Add(columndefinition_0803);
            //grid_0801.ColumnDefinitions.Add(columndefinition_0804);
            //grid_0805.ColumnDefinitions.Add(columndefinition_0807);
            //grid_0805.ColumnDefinitions.Add(columndefinition_0808);
            //grid_0809.ColumnDefinitions.Add(columndefinition_0811);
            //grid_0809.ColumnDefinitions.Add(columndefinition_0812);
            //grid_0813.ColumnDefinitions.Add(columndefinition_0815);
            //grid_0813.ColumnDefinitions.Add(columndefinition_0816);
            //grid_0831.ColumnDefinitions.Add(columndefinition_0833);
            //grid_0831.ColumnDefinitions.Add(columndefinition_0834);
            //grid_0835.ColumnDefinitions.Add(columndefinition_0837);
            //grid_0835.ColumnDefinitions.Add(columndefinition_0838);
            //grid_0839.ColumnDefinitions.Add(columndefinition_0841);
            //grid_0839.ColumnDefinitions.Add(columndefinition_0842);
            //grid_0843.ColumnDefinitions.Add(columndefinition_0845);
            //grid_0843.ColumnDefinitions.Add(columndefinition_0846);
            //grid_0847.ColumnDefinitions.Add(columndefinition_0849);
            //grid_0847.ColumnDefinitions.Add(columndefinition_0850);
            //grid_0851.ColumnDefinitions.Add(columndefinition_0853);
            //grid_0851.ColumnDefinitions.Add(columndefinition_0854);
            //grid_0855.ColumnDefinitions.Add(columndefinition_0857);
            //grid_0855.ColumnDefinitions.Add(columndefinition_0858);
            //grid_0859.ColumnDefinitions.Add(columndefinition_0861);
            //grid_0859.ColumnDefinitions.Add(columndefinition_0862);
            //grid_0863.ColumnDefinitions.Add(columndefinition_0865);
            //grid_0863.ColumnDefinitions.Add(columndefinition_0866);
            //grid_0867.ColumnDefinitions.Add(columndefinition_0869);
            //grid_0867.ColumnDefinitions.Add(columndefinition_0870);
            //grid_0872.ColumnDefinitions.Add(columndefinition_0874);
            //grid_0872.ColumnDefinitions.Add(columndefinition_0875);
            //grid_0892.ColumnDefinitions.Add(columndefinition_0894);
            //grid_0892.ColumnDefinitions.Add(columndefinition_0895);
            //grid_0897.ColumnDefinitions.Add(columndefinition_0899);
            //grid_0897.ColumnDefinitions.Add(columndefinition_0900);
            //grid_0902.ColumnDefinitions.Add(columndefinition_0904);
            //grid_0902.ColumnDefinitions.Add(columndefinition_0905);
            //grid_0906.ColumnDefinitions.Add(columndefinition_0908);
            //grid_0906.ColumnDefinitions.Add(columndefinition_0909);
            //grid_0910.ColumnDefinitions.Add(columndefinition_0912);
            //grid_0910.ColumnDefinitions.Add(columndefinition_0913);
            //grid_0914.ColumnDefinitions.Add(columndefinition_0916);
            //grid_0914.ColumnDefinitions.Add(columndefinition_0917);
            //grid_0918.ColumnDefinitions.Add(columndefinition_0920);
            //grid_0918.ColumnDefinitions.Add(columndefinition_0921);
            //grid_0922.ColumnDefinitions.Add(columndefinition_0924);
            //grid_0922.ColumnDefinitions.Add(columndefinition_0925);
            //grid_0926.ColumnDefinitions.Add(columndefinition_0928);
            //grid_0926.ColumnDefinitions.Add(columndefinition_0929);
            //grid_0930.ColumnDefinitions.Add(columndefinition_0932);
            //grid_0930.ColumnDefinitions.Add(columndefinition_0933);
            //grid_0934.ColumnDefinitions.Add(columndefinition_0936);
            //grid_0934.ColumnDefinitions.Add(columndefinition_0937);
            //grid_0938.ColumnDefinitions.Add(columndefinition_0940);
            //grid_0938.ColumnDefinitions.Add(columndefinition_0941);
            //grid_0942.ColumnDefinitions.Add(columndefinition_0944);
            //grid_0942.ColumnDefinitions.Add(columndefinition_0945);
            //grid_0946.ColumnDefinitions.Add(columndefinition_0948);
            //grid_0946.ColumnDefinitions.Add(columndefinition_0949);
            //grid_0979.ColumnDefinitions.Add(columndefinition_0981);
            //grid_0979.ColumnDefinitions.Add(columndefinition_0982);
            //grid_0983.ColumnDefinitions.Add(columndefinition_0985);
            //grid_0983.ColumnDefinitions.Add(columndefinition_0986);
            //grid_0987.ColumnDefinitions.Add(columndefinition_0989);
            //grid_0987.ColumnDefinitions.Add(columndefinition_0990);
            //grid_0991.ColumnDefinitions.Add(columndefinition_0993);
            //grid_0991.ColumnDefinitions.Add(columndefinition_0994);
            //grid_1006.ColumnDefinitions.Add(columndefinition_1008);
            //grid_1006.ColumnDefinitions.Add(columndefinition_1009);
            //grid_1011.ColumnDefinitions.Add(columndefinition_1013);
            //grid_1011.ColumnDefinitions.Add(columndefinition_1014);
            //grid_1015.ColumnDefinitions.Add(columndefinition_1017);
            //grid_1015.ColumnDefinitions.Add(columndefinition_1018);
            //grid_1019.ColumnDefinitions.Add(columndefinition_1021);
            //grid_1019.ColumnDefinitions.Add(columndefinition_1022);
            //grid_1023.ColumnDefinitions.Add(columndefinition_1025);
            //grid_1023.ColumnDefinitions.Add(columndefinition_1026);
            //grid_1027.ColumnDefinitions.Add(columndefinition_1029);
            //grid_1027.ColumnDefinitions.Add(columndefinition_1030);
            //grid_1031.ColumnDefinitions.Add(columndefinition_1033);
            //grid_1031.ColumnDefinitions.Add(columndefinition_1034);
            //grid_1038.ColumnDefinitions.Add(columndefinition_1040);
            //grid_1038.ColumnDefinitions.Add(columndefinition_1041);
            //grid_1038.ColumnDefinitions.Add(columndefinition_1042);
            //grid_Buttons.ColumnDefinitions.Add(columndefinition_1046);
            //grid_Buttons.ColumnDefinitions.Add(columndefinition_1047);
            //grid_Buttons.ColumnDefinitions.Add(columndefinition_1048);
            //grid_Buttons.ColumnDefinitions.Add(columndefinition_1049);
            //grid_Buttons.ColumnDefinitions.Add(columndefinition_1050);

            // Assemble grids with controls ---------------------------------------------------------------

            grid_0151.Children.Add((new GridRow(0, new View[] { tbSystemCommonMasterTune, slSystemCommonMasterTune }, new byte[] { 1, 1 })).Row);
            grid_0155.Children.Add((new GridRow(0, new View[] { tbSystemCommonMasterKeyShift, slSystemCommonMasterKeyShift }, new byte[] { 1, 1 })).Row);
            grid_0159.Children.Add((new GridRow(0, new View[] { tbSystemCommonMasterLevel, slSystemCommonMasterLevel }, new byte[] { 1, 1 })).Row);
            grid_0164.Children.Add((new GridRow(0, new View[] { textblock_0168, cbSystemCommonStudioSetControlChannel }, new byte[] { 1, 1 })).Row);
            grid_0169.Children.Add((new GridRow(0, new View[] { tbSystemCommonSystemControlSource1, cbSystemCommonSystemControlSource1 }, new byte[] { 1, 1 })).Row);
            grid_0173.Children.Add((new GridRow(0, new View[] { tbSystemCommonSystemControlSource2, cbSystemCommonSystemControlSource2 }, new byte[] { 1, 1 })).Row);
            grid_0177.Children.Add((new GridRow(0, new View[] { tbSystemCommonSystemControlSource3, cbSystemCommonSystemControlSource3 }, new byte[] { 1, 1 })).Row);
            grid_0181.Children.Add((new GridRow(0, new View[] { tbSystemCommonSystemControlSource4, cbSystemCommonSystemControlSource4 }, new byte[] { 1, 1 })).Row);
            grid_0185.Children.Add((new GridRow(0, new View[] { textblock_0189, cbSystemCommonControlSource }, new byte[] { 1, 1 })).Row);
            grid_0190.Children.Add((new GridRow(0, new View[] { textblock_0194, cbSystemCommonSystemClockSource }, new byte[] { 1, 1 })).Row);
            grid_0195.Children.Add((new GridRow(0, new View[] { textblock_0199, slSystemCommonSystemTempo }, new byte[] { 1, 1 })).Row);
            grid_0200.Children.Add((new GridRow(0, new View[] { textblock_0204, cbSystemCommonTempoAssignSource }, new byte[] { 1, 1 })).Row);
            grid_0209.Children.Add((new GridRow(0, new View[] { textblock_0213, cbSystemCommonStereoOutputMode }, new byte[] { 1, 1 })).Row);
            grid_0231.Children.Add((new GridRow(0, new View[] { textblock_0235, slVoiceReserve01 }, new byte[] { 1, 2 })).Row);
            grid_0236.Children.Add((new GridRow(0, new View[] { textblock_0240, slVoiceReserve02 }, new byte[] { 1, 2 })).Row);
            grid_0241.Children.Add((new GridRow(0, new View[] { textblock_0245, slVoiceReserve03 }, new byte[] { 1, 2 })).Row);
            grid_0246.Children.Add((new GridRow(0, new View[] { textblock_0250, slVoiceReserve04 }, new byte[] { 1, 2 })).Row);
            grid_0251.Children.Add((new GridRow(0, new View[] { textblock_0255, slVoiceReserve05 }, new byte[] { 1, 2 })).Row);
            grid_0256.Children.Add((new GridRow(0, new View[] { textblock_0260, slVoiceReserve06 }, new byte[] { 1, 2 })).Row);
            grid_0261.Children.Add((new GridRow(0, new View[] { textblock_0265, slVoiceReserve07 }, new byte[] { 1, 2 })).Row);
            grid_0266.Children.Add((new GridRow(0, new View[] { textblock_0270, slVoiceReserve08 }, new byte[] { 1, 2 })).Row);
            grid_0271.Children.Add((new GridRow(0, new View[] { textblock_0275, slVoiceReserve09 }, new byte[] { 1, 2 })).Row);
            grid_0276.Children.Add((new GridRow(0, new View[] { textblock_0280, slVoiceReserve10 }, new byte[] { 1, 2 })).Row);
            grid_0281.Children.Add((new GridRow(0, new View[] { textblock_0285, slVoiceReserve11 }, new byte[] { 1, 2 })).Row);
            grid_0286.Children.Add((new GridRow(0, new View[] { textblock_0290, slVoiceReserve12 }, new byte[] { 1, 2 })).Row);
            grid_0291.Children.Add((new GridRow(0, new View[] { textblock_0295, slVoiceReserve13 }, new byte[] { 1, 2 })).Row);
            grid_0296.Children.Add((new GridRow(0, new View[] { textblock_0300, slVoiceReserve14 }, new byte[] { 1, 2 })).Row);
            grid_0301.Children.Add((new GridRow(0, new View[] { textblock_0305, slVoiceReserve15 }, new byte[] { 1, 2 })).Row);
            grid_0306.Children.Add((new GridRow(0, new View[] { textblock_0310, slVoiceReserve16 }, new byte[] { 1, 2 })).Row);
            grid_0317.Children.Add((new GridRow(0, new View[] { textblock_0321, cbStudioSetChorusType }, new byte[] { 2, 1 })).Row);
            grid_0322.Children.Add((new GridRow(0, new View[] { tbChorusLevel, slChorusLevel }, new byte[] { 1, 1 })).Row);
            grid_0326.Children.Add((new GridRow(0, new View[] { textblock_0330, cbChorusOutputAssign }, new byte[] { 2, 1 })).Row);
            grid_0331.Children.Add((new GridRow(0, new View[] { textblock_0335, cbChorusOutputSelect }, new byte[] { 2, 1 })).Row);
            grid_0346.Children.Add((new GridRow(0, new View[] { tbChorusChorusFilterType, cbChorusChorusFilterType }, new byte[] { 2, 1 })).Row);
            grid_0350.Children.Add((new GridRow(0, new View[] { tbChorusChorusFilterCutoffFrequency, cbChorusChorusFilterCutoffFrequency }, new byte[] { 2, 1 })).Row);
            grid_0354.Children.Add((new GridRow(0, new View[] { tbChorusChorusPreDelay, slChorusChorusPreDelay }, new byte[] { 1, 1 })).Row);
            grid_0358.Children.Add((new GridRow(0, new View[] { tbChorusChorusRateHzNote, cbChorusChorusRateHzNote }, new byte[] { 2, 1 })).Row);
            ChorusRateHz.Children.Add((new GridRow(0, new View[] { tbChorusChorusRateHz, slChorusChorusRateHz }, new byte[] { 1, 1 })).Row);
            ChorusRateNote.Children.Add((new GridRow(0, new View[] { tbChorusChorusRateNote, slChorusChorusRateNote }, new byte[] { 1, 1 })).Row);
            ChorusDepth.Children.Add((new GridRow(0, new View[] { tbChorusChorusDepth, slChorusChorusDepth }, new byte[] { 1, 1 })).Row);
            grid_0371.Children.Add((new GridRow(0, new View[] { tbChorusChorusPhase, slChorusChorusPhase }, new byte[] { 1, 1 })).Row);
            grid_0375.Children.Add((new GridRow(0, new View[] { tbChorusChorusFeedback, slChorusChorusFeedback }, new byte[] { 1, 1 })).Row);
            grid_0392.Children.Add((new GridRow(0, new View[] { textblock_0396, cbChorusDelayLeftMsNote }, new byte[] { 2, 1 })).Row);
            ChorusDelayLeftHz.Children.Add((new GridRow(0, new View[] { tbChorusDelayLeftHz, slChorusDelayLeftHz }, new byte[] { 1, 2 })).Row);
            ChorusDelayLeftNote.Children.Add((new GridRow(0, new View[] { tbChorusDelayLeftNote, slChorusDelayLeftNote }, new byte[] { 1, 2 })).Row);
            grid_0403.Children.Add((new GridRow(0, new View[] { textblock_0407, cbChorusDelayRightMsNote }, new byte[] { 2, 1 })).Row);
            ChorusDelayRightHz.Children.Add((new GridRow(0, new View[] { tbChorusDelayRightHz, slChorusDelayRightHz }, new byte[] { 1, 2 })).Row);
            ChorusDelayRightNote.Children.Add((new GridRow(0, new View[] { tbChorusDelayRightNote, slChorusDelayRightNote }, new byte[] { 1, 2 })).Row);
            grid_0414.Children.Add((new GridRow(0, new View[] { textblock_0418, cbChorusDelayCenterMsNote }, new byte[] { 2, 1 })).Row);
            ChorusDelayCenterHz.Children.Add((new GridRow(0, new View[] { tbChorusDelayCenterHz, slChorusDelayCenterHz }, new byte[] { 1, 2 })).Row);
            ChorusDelayCenterNote.Children.Add((new GridRow(0, new View[] { tbChorusDelayCenterNote, slChorusDelayCenterNote }, new byte[] { 1, 2 })).Row);
            grid_0425.Children.Add((new GridRow(0, new View[] { tbChorusDelayCenterFeedback, slChorusDelayCenterFeedback }, new byte[] { 1, 2 })).Row);
            grid_0429.Children.Add((new GridRow(0, new View[] { tbChorusDelayHFDamp, cbChorusDelayHFDamp }, new byte[] { 2, 1 })).Row);
            grid_0433.Children.Add((new GridRow(0, new View[] { tbChorusDelayLeftLevel, slChorusDelayLeftLevel }, new byte[] { 1, 2 })).Row);
            grid_0437.Children.Add((new GridRow(0, new View[] { tbChorusDelayRightLevel, slChorusDelayRightLevel }, new byte[] { 1, 2 })).Row);
            grid_0441.Children.Add((new GridRow(0, new View[] { tbChorusDelayCenterLevel, slChorusDelayCenterLevel }, new byte[] { 1, 2 })).Row);
            ChorusGM2ChorusPreLPF.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusPreLPF, slChorusGM2ChorusPreLPF }, new byte[] { 1, 1 })).Row);
            ChorusGM2ChorusLevel.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusLevel, slChorusGM2ChorusLevel }, new byte[] { 1, 1 })).Row);
            ChorusGM2ChorusFeedback.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusFeedback, slChorusGM2ChorusFeedback }, new byte[] { 1, 1 })).Row);
            ChorusGM2ChorusDelay.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusDelay, slChorusGM2ChorusDelay }, new byte[] { 1, 1 })).Row);
            ChorusGM2ChorusRate.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusRate, slChorusGM2ChorusRate }, new byte[] { 1, 1 })).Row);
            ChorusGM2ChorusDepth.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusDepth, slChorusGM2ChorusDepth }, new byte[] { 1, 1 })).Row);
            ChorusGM2ChorusSendLevelToReverb.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusSendLevelToReverb, slChorusGM2ChorusSendLevelToReverb }, new byte[] { 1, 1 })).Row);
            grid_0480.Children.Add((new GridRow(0, new View[] { textblock_0484, cbStudioSetReverbType }, new byte[] { 2, 3 })).Row);
            grid_0485.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbLevel, slStudioSetReverbLevel }, new byte[] { 2, 3 })).Row);
            grid_0489.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbOutputAssign, cbStudioSetReverbOutputAssign }, new byte[] { 2, 3 })).Row);
            grid_0503.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbPreDelay, slStudioSetReverbPreDelay }, new byte[] { 1, 2 })).Row);
            grid_0507.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbTime, slStudioSetReverbTime }, new byte[] { 1, 2 })).Row);
            grid_0511.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbDensity, slStudioSetReverbDensity }, new byte[] { 1, 2 })).Row);
            grid_0515.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbDiffusion, slStudioSetReverbDiffusion }, new byte[] { 1, 2 })).Row);
            grid_0519.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbLFDamp, slStudioSetReverbLFDamp }, new byte[] { 1, 2 })).Row);
            grid_0523.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbHFDamp, slStudioSetReverbHFDamp }, new byte[] { 1, 2 })).Row);
            grid_0527.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbSpread, slStudioSetReverbSpread }, new byte[] { 1, 2 })).Row);
            grid_0531.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbTone, slStudioSetReverbTone }, new byte[] { 1, 2 })).Row);
            grid_0539.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbGM2Character, slStudioSetReverbGM2Character }, new byte[] { 1, 2 })).Row);
            grid_0543.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbGM2Time, slStudioSetReverbGM2Time }, new byte[] { 1, 2 })).Row);
            grid_0567.Children.Add((new GridRow(0, new View[] { textblock_0571, cbStudioSetMotionalSurroundRoomType }, new byte[] { 1, 1 })).Row);
            grid_0572.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundAmbienceLevel, slStudioSetMotionalSurroundAmbienceLevel }, new byte[] { 1, 1 })).Row);
            grid_0576.Children.Add((new GridRow(0, new View[] { textblock_0580, cbStudioSetMotionalSurroundRoomSize }, new byte[] { 1, 1 })).Row);
            grid_0581.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundAmbienceTime, slStudioSetMotionalSurroundAmbienceTime }, new byte[] { 1, 1 })).Row);
            grid_0585.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundAmbienceDensity, slStudioSetMotionalSurroundAmbienceDensity }, new byte[] { 1, 1 })).Row);
            grid_0589.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundAmbienceHFDamp, slStudioSetMotionalSurroundAmbienceHFDamp }, new byte[] { 1, 1 })).Row);
            grid_0593.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundExternalPartLR, slStudioSetMotionalSurroundExternalPartLR }, new byte[] { 1, 1 })).Row);
            grid_0597.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundExternalPartFB, slStudioSetMotionalSurroundExternalPartFB }, new byte[] { 1, 1 })).Row);
            grid_0601.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundExtPartWidth, slStudioSetMotionalSurroundExtPartWidth }, new byte[] { 1, 1 })).Row);
            grid_0605.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundExtpartAmbienceSend, slStudioSetMotionalSurroundExtpartAmbienceSend }, new byte[] { 1, 1 })).Row);
            grid_0609.Children.Add((new GridRow(0, new View[] { textblock_0613, cbStudioSetMotionalSurroundExtPartControl }, new byte[] { 1, 1 })).Row);
            grid_0614.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundDepth, slStudioSetMotionalSurroundDepth }, new byte[] { 1, 1 })).Row);
            grid_0643.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqLowFreq, cbStudioSetMasterEqLowFreq }, new byte[] { 1, 1 })).Row);
            grid_0647.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqLowGain, slStudioSetMasterEqLowGain }, new byte[] { 1, 1 })).Row);
            grid_0651.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqMidFreq, cbStudioSetMasterEqMidFreq }, new byte[] { 1, 1 })).Row);
            grid_0655.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqMidGain, slStudioSetMasterEqMidGain }, new byte[] { 1, 1 })).Row);
            grid_0659.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqMidQ, cbStudioSetMasterEqMidQ }, new byte[] { 1, 1 })).Row);
            grid_0663.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqHighFreq, cbStudioSetMasterEqHighFreq }, new byte[] { 1, 1 })).Row);
            grid_0667.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqHighGain, slStudioSetMasterEqHighGain }, new byte[] { 1, 1 })).Row);
            grid_0695.Children.Add((new GridRow(0, new View[] { cbStudioSetPartSettings1Receive, cbStudioSetPartSettings1ReceiveChannel }, new byte[] { 1, 1 })).Row);
            grid_0699.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1Group, cbStudioSetPartSettings1Group }, new byte[] { 1, 1 })).Row);
            grid_0703.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1Category, cbStudioSetPartSettings1Category }, new byte[] { 96, 71 })).Row);
            grid_0708.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1Program, cbStudioSetPartSettings1Program }, new byte[] { 1, 1 })).Row);
            grid_0712.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1Search, cbStudioSetPartSettings1Search }, new byte[] { 1, 4 })).Row);
            grid_0716.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1Level, slStudioSetPartSettings1Level }, new byte[] { 1, 1 })).Row);
            grid_0720.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1Pan, slStudioSetPartSettings1Pan }, new byte[] { 1, 1 })).Row);
            grid_0724.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1CoarseTune, slStudioSetPartSettings1CoarseTune }, new byte[] { 1, 1 })).Row);
            grid_0728.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1FineTune, slStudioSetPartSettings1FineTune }, new byte[] { 1, 1 })).Row);
            grid_0732.Children.Add((new GridRow(0, new View[] { textblock_0736, cbStudioSetPartSettings1MonoPoly }, new byte[] { 1, 1 })).Row);
            grid_0737.Children.Add((new GridRow(0, new View[] { textblock_0741, cbStudioSetPartSettings1Legato }, new byte[] { 1, 1 })).Row);
            grid_0742.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1PitchBendRange, slStudioSetPartSettings1PitchBendRange }, new byte[] { 1, 1 })).Row);
            grid_0746.Children.Add((new GridRow(0, new View[] { textblock_0750, cbStudioSetPartSettings1Portamento }, new byte[] { 1, 1 })).Row);
            grid_0751.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1PortamentoTime, slStudioSetPartSettings1PortamentoTime }, new byte[] { 1, 1 })).Row);
            grid_0773.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2CutoffOffset, slStudioSetPartSettings2CutoffOffset }, new byte[] { 1, 1 })).Row);
            grid_0777.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2ResonanceOffset, slStudioSetPartSettings2ResonanceOffset }, new byte[] { 1, 1 })).Row);
            grid_0781.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2AttackTimeOffset, slStudioSetPartSettings2AttackTimeOffset }, new byte[] { 1, 1 })).Row);
            grid_0785.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2DecayTimeOffset, slStudioSetPartSettings2DecayTimeOffset }, new byte[] { 1, 1 })).Row);
            grid_0789.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2ReleaseTimeOffset, slStudioSetPartSettings2ReleaseTimeOffset }, new byte[] { 1, 1 })).Row);
            grid_0793.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2VibratoRate, slStudioSetPartSettings2VibratoRate }, new byte[] { 1, 1 })).Row);
            grid_0797.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2VibratoDepth, slStudioSetPartSettings2VibratoDepth }, new byte[] { 1, 1 })).Row);
            grid_0801.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2VibratoDelay, slStudioSetPartSettings2VibratoDelay }, new byte[] { 1, 1 })).Row);
            grid_0805.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEffectsChorusSendLevel, slStudioSetPartEffectsChorusSendLevel }, new byte[] { 1, 1 })).Row);
            grid_0809.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEffectsReverbSendLevel, slStudioSetPartEffectsReverbSendLevel }, new byte[] { 1, 1 })).Row);
            grid_0813.Children.Add((new GridRow(0, new View[] { textblock_0817, cbStudioSetPartEffectsOutputAssign }, new byte[] { 1, 1 })).Row);
            grid_0831.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardOctaveShift, slStudioSetPartKeyboardOctaveShift }, new byte[] { 1, 1 })).Row);
            grid_0835.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardVelocitySenseOffset, slStudioSetPartKeyboardVelocitySenseOffset }, new byte[] { 1, 1 })).Row);
            grid_0839.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardRangeLower, slStudioSetPartKeyboardRangeLower }, new byte[] { 1, 1 })).Row);
            grid_0843.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardRangeUpper, slStudioSetPartKeyboardRangeUpper }, new byte[] { 1, 1 })).Row);
            grid_0847.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardFadeWidthLower, slStudioSetPartKeyboardFadeWidthLower }, new byte[] { 1, 1 })).Row);
            grid_0851.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardFadeWidthUpper, slStudioSetPartKeyboardFadeWidthUpper }, new byte[] { 1, 1 })).Row);
            grid_0855.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardVelocityRangeLower, slStudioSetPartKeyboardVelocityRangeLower }, new byte[] { 1, 1 })).Row);
            grid_0859.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardVelocityRangeUpper, slStudioSetPartKeyboardVelocityRangeUpper }, new byte[] { 1, 1 })).Row);
            grid_0863.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardVelocityFadeWidthLower, slStudioSetPartKeyboardVelocityFadeWidthLower }, new byte[] { 1, 1 })).Row);
            grid_0867.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardVelocityFadeWidthUpper, slStudioSetPartKeyboardVelocityFadeWidthUpper }, new byte[] { 1, 1 })).Row);
            grid_0872.Children.Add((new GridRow(0, new View[] { textblock_0876, cbStudioSetPartKeyboardVelocityCurveType }, new byte[] { 1, 1 })).Row);
            grid_0892.Children.Add((new GridRow(0, new View[] { textblock_0896, cbStudioSetPartScaleTuneType }, new byte[] { 1, 1 })).Row);
            grid_0897.Children.Add((new GridRow(0, new View[] { textblock_0901, cbStudioSetPartScaleTuneKey }, new byte[] { 1, 1 })).Row);
            grid_0902.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneC, slStudioSetPartScaleTuneC }, new byte[] { 1, 1 })).Row);
            grid_0906.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneCi, slStudioSetPartScaleTuneCi }, new byte[] { 1, 1 })).Row);
            grid_0910.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneD, slStudioSetPartScaleTuneD }, new byte[] { 1, 1 })).Row);
            grid_0914.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneDi, slStudioSetPartScaleTuneDi }, new byte[] { 1, 1 })).Row);
            grid_0918.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneE, slStudioSetPartScaleTuneE }, new byte[] { 1, 1 })).Row);
            grid_0922.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneF, slStudioSetPartScaleTuneF }, new byte[] { 1, 1 })).Row);
            grid_0926.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneFi, slStudioSetPartScaleTuneFi }, new byte[] { 1, 1 })).Row);
            grid_0930.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneG, slStudioSetPartScaleTuneG }, new byte[] { 1, 1 })).Row);
            grid_0934.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneGi, slStudioSetPartScaleTuneGi }, new byte[] { 1, 1 })).Row);
            grid_0938.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneA, slStudioSetPartScaleTuneA }, new byte[] { 1, 1 })).Row);
            grid_0942.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneAi, slStudioSetPartScaleTuneAi }, new byte[] { 1, 1 })).Row);
            grid_0946.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneB, slStudioSetPartScaleTuneB }, new byte[] { 1, 1 })).Row);
            grid_0979.Children.Add((new GridRow(0, new View[] { tbStudioSetPartMotionalSurroundLR, slStudioSetPartMotionalSurroundLR }, new byte[] { 1, 1 })).Row);
            grid_0983.Children.Add((new GridRow(0, new View[] { tbStudioSetPartMotionalSurroundFB, slStudioSetPartMotionalSurroundFB }, new byte[] { 1, 1 })).Row);
            grid_0987.Children.Add((new GridRow(0, new View[] { tbStudioSetPartMotionalSurroundWidth, slStudioSetPartMotionalSurroundWidth }, new byte[] { 1, 1 })).Row);
            grid_0991.Children.Add((new GridRow(0, new View[] { tbStudioSetPartMotionalSurroundAmbienceSendLevel, slStudioSetPartMotionalSurroundAmbienceSendLevel }, new byte[] { 1, 1 })).Row);
            grid_1006.Children.Add((new GridRow(0, new View[] { textblock_1010, cbStudioSetPartEQLoqFreq }, new byte[] { 1, 1 })).Row);
            grid_1011.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEQLowGain, slStudioSetPartEQLowGain }, new byte[] { 1, 1 })).Row);
            grid_1015.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEQMidFreq, cbStudioSetPartEQMidFreq }, new byte[] { 1, 1 })).Row);
            grid_1019.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEQMidGain, slStudioSetPartEQMidGain }, new byte[] { 1, 1 })).Row);
            grid_1023.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEQMidQ, cbStudioSetPartEQMidQ }, new byte[] { 1, 1 })).Row);
            grid_1027.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEQHighFreq, cbStudioSetPartEQHighFreq }, new byte[] { 1, 1 })).Row);
            grid_1031.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEQHighGain, slStudioSetPartEQHighGain }, new byte[] { 1, 1 })).Row);
            grid_1038.Children.Add((new GridRow(0, new View[] { cbStudioSetSlot, textblock_1043, tbStudioSetName }, new byte[] { 5, 3, 8 })).Row);
            grid_Buttons.Children.Add((new GridRow(0, new View[] { btnFileSave, btnFileLoad, btnStudioSetSave, btnStudioSetDelete, btnStudioSetReturn }, new byte[] { 3, 3, 2, 2, 2 })).Row);

            //grid_StudioSet_And_Scanning.Children.Add((new GridRow(0, new View[] { grid_PleaseWaitWhileScanning, grid_MainStudioSet })).Row);
            grid_PleaseWaitWhileScanning.Children.Add((new GridRow(0, new View[] { Progress, tbPleaseWaitWhileScanning })).Row);
            grid_StudioSetSelector.Children.Add((new GridRow(0, new View[] { cbStudioSetSelector })).Row);
            grid_ToneControl1.Children.Add((new GridRow(0, new View[] { textblock_ToneControl1, cbToneControl1 })).Row);
            grid_ToneControl2.Children.Add((new GridRow(0, new View[] { textblock_ToneControl2, cbToneControl2 })).Row);
            grid_ToneControl3.Children.Add((new GridRow(0, new View[] { textblock_ToneControl3, cbToneControl3 })).Row);
            grid_ToneControl4.Children.Add((new GridRow(0, new View[] { textblock_ToneControl4, cbToneControl4 })).Row);
            grid_Tempo.Children.Add((new GridRow(0, new View[] { tbTempo, slTempo })).Row);
            grid_SoloPart.Children.Add((new GridRow(0, new View[] { tbSoloPart, cbSoloPart })).Row);
            //grid_0064.Children.Add((new GridRow(0, new View[] { grid_0065 })).Row);
            //grid_0065.Children.Add((new GridRow(0, new View[] { cbReverb, cbChorus, cbMasterEQ })).Row);
            //grid_0070.Children.Add((new GridRow(0, new View[] { grid_0071 })).Row);
            //grid_0071.Children.Add((new GridRow(0, new View[] { textblock_0075, cbDrumCompEQPart })).Row);
            //grid_0076.Children.Add((new GridRow(0, new View[] { grid_0077 })).Row);
            //grid_0077.Children.Add((new GridRow(0, new View[] { textblock_0081, cbDrumCompEQ1OutputAssign })).Row);
            //grid_0082.Children.Add((new GridRow(0, new View[] { grid_0083 })).Row);
            //grid_0083.Children.Add((new GridRow(0, new View[] { textblock_0087, cbDrumCompEQ2OutputAssign })).Row);
            //grid_0088.Children.Add((new GridRow(0, new View[] { grid_0089 })).Row);
            //grid_0089.Children.Add((new GridRow(0, new View[] { textblock_0093, cbDrumCompEQ3OutputAssign })).Row);
            //grid_0094.Children.Add((new GridRow(0, new View[] { grid_0095 })).Row);
            //grid_0095.Children.Add((new GridRow(0, new View[] { textblock_0099, cbDrumCompEQ4OutputAssign })).Row);
            //grid_0100.Children.Add((new GridRow(0, new View[] { grid_0101 })).Row);
            //grid_0101.Children.Add((new GridRow(0, new View[] { textblock_0105, cbDrumCompEQ5OutputAssign })).Row);
            //grid_0106.Children.Add((new GridRow(0, new View[] { grid_0107 })).Row);
            //grid_0107.Children.Add((new GridRow(0, new View[] { textblock_0111, cbDrumCompEQ6OutputAssign })).Row);
            //grid_0112.Children.Add((new GridRow(0, new View[] { cbDrumCompEQ, cbExtPartMute })).Row);
            //grid_0116.Children.Add((new GridRow(0, new View[] { tbExtPartLevel, slExtPartLevel })).Row);
            //grid_0120.Children.Add((new GridRow(0, new View[] { tbExtPartChorusSend, slExtPartChorusSend })).Row);
            //grid_0124.Children.Add((new GridRow(0, new View[] { tbExtPartReverbSend, slExtPartReverbSend })).Row);
            //grid_Column2Selector.Children.Add((new GridRow(0, new View[] { cbColumn2Selector })).Row);
            //SystemCommonSettings.Children.Add((new GridRow(0, new View[] { grid_0151, grid_0155, grid_0159, grid_0163, grid_0164, grid_0169, grid_0173, grid_0177, grid_0181, grid_0185, grid_0190, grid_0195, grid_0200, grid_0205, grid_0206, grid_0207, grid_0208, grid_0209 })).Row);
            //grid_0151.Children.Add((new GridRow(0, new View[] { tbSystemCommonMasterTune, slSystemCommonMasterTune })).Row);
            //grid_0155.Children.Add((new GridRow(0, new View[] { tbSystemCommonMasterKeyShift, slSystemCommonMasterKeyShift })).Row);
            //grid_0159.Children.Add((new GridRow(0, new View[] { tbSystemCommonMasterLevel, slSystemCommonMasterLevel })).Row);
            //grid_0163.Children.Add((new GridRow(0, new View[] { cbSystemCommonScaleTuneSwitch })).Row);
            //grid_0164.Children.Add((new GridRow(0, new View[] { textblock_0168, cbSystemCommonStudioSetControlChannel })).Row);
            //grid_0169.Children.Add((new GridRow(0, new View[] { tbSystemCommonSystemControlSource1, cbSystemCommonSystemControlSource1 })).Row);
            //grid_0173.Children.Add((new GridRow(0, new View[] { tbSystemCommonSystemControlSource2, cbSystemCommonSystemControlSource2 })).Row);
            //grid_0177.Children.Add((new GridRow(0, new View[] { tbSystemCommonSystemControlSource3, cbSystemCommonSystemControlSource3 })).Row);
            //grid_0181.Children.Add((new GridRow(0, new View[] { tbSystemCommonSystemControlSource4, cbSystemCommonSystemControlSource4 })).Row);
            //grid_0185.Children.Add((new GridRow(0, new View[] { textblock_0189, cbSystemCommonControlSource })).Row);
            //grid_0190.Children.Add((new GridRow(0, new View[] { textblock_0194, cbSystemCommonSystemClockSource })).Row);
            //grid_0195.Children.Add((new GridRow(0, new View[] { textblock_0199, slSystemCommonSystemTempo })).Row);
            //grid_0200.Children.Add((new GridRow(0, new View[] { textblock_0204, cbSystemCommonTempoAssignSource })).Row);
            //grid_0205.Children.Add((new GridRow(0, new View[] { cbSystemCommonReceiveProgramChange })).Row);
            //grid_0206.Children.Add((new GridRow(0, new View[] { cbSystemCommonReceiveBankSelect })).Row);
            //grid_0207.Children.Add((new GridRow(0, new View[] { cbSystemCommonSurroundCenterSpeakerSwitch })).Row);
            //grid_0208.Children.Add((new GridRow(0, new View[] { cbSystemCommonSurroundSubWooferSwitch })).Row);
            //grid_0209.Children.Add((new GridRow(0, new View[] { textblock_0213, cbSystemCommonStereoOutputMode })).Row);
            //VoiceReserve.Children.Add((new GridRow(0, new View[] { grid_0231, grid_0236, grid_0241, grid_0246, grid_0251, grid_0256, grid_0261, grid_0266, grid_0271, grid_0276, grid_0281, grid_0286, grid_0291, grid_0296, grid_0301, grid_0306 })).Row);
            //grid_0231.Children.Add((new GridRow(0, new View[] { textblock_0235, slVoiceReserve01 })).Row);
            //grid_0236.Children.Add((new GridRow(0, new View[] { textblock_0240, slVoiceReserve02 })).Row);
            //grid_0241.Children.Add((new GridRow(0, new View[] { textblock_0245, slVoiceReserve03 })).Row);
            //grid_0246.Children.Add((new GridRow(0, new View[] { textblock_0250, slVoiceReserve04 })).Row);
            //grid_0251.Children.Add((new GridRow(0, new View[] { textblock_0255, slVoiceReserve05 })).Row);
            //grid_0256.Children.Add((new GridRow(0, new View[] { textblock_0260, slVoiceReserve06 })).Row);
            //grid_0261.Children.Add((new GridRow(0, new View[] { textblock_0265, slVoiceReserve07 })).Row);
            //grid_0266.Children.Add((new GridRow(0, new View[] { textblock_0270, slVoiceReserve08 })).Row);
            //grid_0271.Children.Add((new GridRow(0, new View[] { textblock_0275, slVoiceReserve09 })).Row);
            //grid_0276.Children.Add((new GridRow(0, new View[] { textblock_0280, slVoiceReserve10 })).Row);
            //grid_0281.Children.Add((new GridRow(0, new View[] { textblock_0285, slVoiceReserve11 })).Row);
            //grid_0286.Children.Add((new GridRow(0, new View[] { textblock_0290, slVoiceReserve12 })).Row);
            //grid_0291.Children.Add((new GridRow(0, new View[] { textblock_0295, slVoiceReserve13 })).Row);
            //grid_0296.Children.Add((new GridRow(0, new View[] { textblock_0300, slVoiceReserve14 })).Row);
            //grid_0301.Children.Add((new GridRow(0, new View[] { textblock_0305, slVoiceReserve15 })).Row);
            //grid_0306.Children.Add((new GridRow(0, new View[] { textblock_0310, slVoiceReserve16 })).Row);
            //Chorus.Children.Add((new GridRow(0, new View[] { grid_0317, grid_0322, grid_0326, grid_0331, ChorusChorus, ChorusDelay, ChorusGM2Chorus })).Row);
            //grid_0317.Children.Add((new GridRow(0, new View[] { textblock_0321, cbStudioSetChorusType })).Row);
            //grid_0322.Children.Add((new GridRow(0, new View[] { tbChorusLevel, slChorusLevel })).Row);
            //grid_0326.Children.Add((new GridRow(0, new View[] { textblock_0330, cbChorusOutputAssign })).Row);
            //grid_0331.Children.Add((new GridRow(0, new View[] { textblock_0335, cbChorusOutputSelect })).Row);
            //ChorusChorus.Children.Add((new GridRow(0, new View[] { grid_0346, grid_0350, grid_0354, grid_0358, ChorusRateHz, ChorusRateNote, ChorusDepth, grid_0371, grid_0375 })).Row);
            //grid_0346.Children.Add((new GridRow(0, new View[] { tbChorusChorusFilterType, cbChorusChorusFilterType })).Row);
            //grid_0350.Children.Add((new GridRow(0, new View[] { tbChorusChorusFilterCutoffFrequency, cbChorusChorusFilterCutoffFrequency })).Row);
            //grid_0354.Children.Add((new GridRow(0, new View[] { tbChorusChorusPreDelay, slChorusChorusPreDelay })).Row);
            //grid_0358.Children.Add((new GridRow(0, new View[] { tbChorusChorusRateHzNote, cbChorusChorusRateHzNote })).Row);
            //ChorusRateHz.Children.Add((new GridRow(0, new View[] { tbChorusChorusRateHz, slChorusChorusRateHz })).Row);
            //ChorusRateNote.Children.Add((new GridRow(0, new View[] { tbChorusChorusRateNote, slChorusChorusRateNote })).Row);
            //ChorusDepth.Children.Add((new GridRow(0, new View[] { tbChorusChorusDepth, slChorusChorusDepth })).Row);
            //grid_0371.Children.Add((new GridRow(0, new View[] { tbChorusChorusPhase, slChorusChorusPhase })).Row);
            //grid_0375.Children.Add((new GridRow(0, new View[] { tbChorusChorusFeedback, slChorusChorusFeedback })).Row);
            //ChorusDelay.Children.Add((new GridRow(0, new View[] { grid_0392, ChorusDelayLeftHz, ChorusDelayLeftNote, grid_0403, ChorusDelayRightHz, ChorusDelayRightNote, grid_0414, ChorusDelayCenterHz, ChorusDelayCenterNote, grid_0425, grid_0429, grid_0433, grid_0437, grid_0441 })).Row);
            //grid_0392.Children.Add((new GridRow(0, new View[] { textblock_0396, cbChorusDelayLeftMsNote })).Row);
            //ChorusDelayLeftHz.Children.Add((new GridRow(0, new View[] { tbChorusDelayLeftHz, slChorusDelayLeftHz })).Row);
            //ChorusDelayLeftNote.Children.Add((new GridRow(0, new View[] { tbChorusDelayLeftNote, slChorusDelayLeftNote })).Row);
            //grid_0403.Children.Add((new GridRow(0, new View[] { textblock_0407, cbChorusDelayRightMsNote })).Row);
            //ChorusDelayRightHz.Children.Add((new GridRow(0, new View[] { tbChorusDelayRightHz, slChorusDelayRightHz })).Row);
            //ChorusDelayRightNote.Children.Add((new GridRow(0, new View[] { tbChorusDelayRightNote, slChorusDelayRightNote })).Row);
            //grid_0414.Children.Add((new GridRow(0, new View[] { textblock_0418, cbChorusDelayCenterMsNote })).Row);
            //ChorusDelayCenterHz.Children.Add((new GridRow(0, new View[] { tbChorusDelayCenterHz, slChorusDelayCenterHz })).Row);
            //ChorusDelayCenterNote.Children.Add((new GridRow(0, new View[] { tbChorusDelayCenterNote, slChorusDelayCenterNote })).Row);
            //grid_0425.Children.Add((new GridRow(0, new View[] { tbChorusDelayCenterFeedback, slChorusDelayCenterFeedback })).Row);
            //grid_0429.Children.Add((new GridRow(0, new View[] { tbChorusDelayHFDamp, cbChorusDelayHFDamp })).Row);
            //grid_0433.Children.Add((new GridRow(0, new View[] { tbChorusDelayLeftLevel, slChorusDelayLeftLevel })).Row);
            //grid_0437.Children.Add((new GridRow(0, new View[] { tbChorusDelayRightLevel, slChorusDelayRightLevel })).Row);
            //grid_0441.Children.Add((new GridRow(0, new View[] { tbChorusDelayCenterLevel, slChorusDelayCenterLevel })).Row);
            //ChorusGM2Chorus.Children.Add((new GridRow(0, new View[] { ChorusGM2ChorusPreLPF, ChorusGM2ChorusLevel, ChorusGM2ChorusFeedback, ChorusGM2ChorusDelay, ChorusGM2ChorusRate, ChorusGM2ChorusDepth, ChorusGM2ChorusSendLevelToReverb })).Row);
            //ChorusGM2ChorusPreLPF.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusPreLPF, slChorusGM2ChorusPreLPF })).Row);
            //ChorusGM2ChorusLevel.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusLevel, slChorusGM2ChorusLevel })).Row);
            //ChorusGM2ChorusFeedback.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusFeedback, slChorusGM2ChorusFeedback })).Row);
            //ChorusGM2ChorusDelay.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusDelay, slChorusGM2ChorusDelay })).Row);
            //ChorusGM2ChorusRate.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusRate, slChorusGM2ChorusRate })).Row);
            //ChorusGM2ChorusDepth.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusDepth, slChorusGM2ChorusDepth })).Row);
            //ChorusGM2ChorusSendLevelToReverb.Children.Add((new GridRow(0, new View[] { tbChorusGM2ChorusSendLevelToReverb, slChorusGM2ChorusSendLevelToReverb })).Row);
            //ChorusReverb.Children.Add((new GridRow(0, new View[] { grid_0480, grid_0485, grid_0489, StudioSetReverb, StudioSetReverbGM2 })).Row);
            //grid_0480.Children.Add((new GridRow(0, new View[] { textblock_0484, cbStudioSetReverbType })).Row);
            //grid_0485.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbLevel, slStudioSetReverbLevel })).Row);
            //grid_0489.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbOutputAssign, cbStudioSetReverbOutputAssign })).Row);
            //StudioSetReverb.Children.Add((new GridRow(0, new View[] { grid_0503, grid_0507, grid_0511, grid_0515, grid_0519, grid_0523, grid_0527, grid_0531 })).Row);
            //grid_0503.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbPreDelay, slStudioSetReverbPreDelay })).Row);
            //grid_0507.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbTime, slStudioSetReverbTime })).Row);
            //grid_0511.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbDensity, slStudioSetReverbDensity })).Row);
            //grid_0515.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbDiffusion, slStudioSetReverbDiffusion })).Row);
            //grid_0519.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbLFDamp, slStudioSetReverbLFDamp })).Row);
            //grid_0523.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbHFDamp, slStudioSetReverbHFDamp })).Row);
            //grid_0527.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbSpread, slStudioSetReverbSpread })).Row);
            //grid_0531.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbTone, slStudioSetReverbTone })).Row);
            //StudioSetReverbGM2.Children.Add((new GridRow(0, new View[] { grid_0539, grid_0543 })).Row);
            //grid_0539.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbGM2Character, slStudioSetReverbGM2Character })).Row);
            //grid_0543.Children.Add((new GridRow(0, new View[] { tbStudioSetReverbGM2Time, slStudioSetReverbGM2Time })).Row);
            //StudioSetMotionalSurround.Children.Add((new GridRow(0, new View[] { grid_0566, grid_0567, grid_0572, grid_0576, grid_0581, grid_0585, grid_0589, grid_0593, grid_0597, grid_0601, grid_0605, grid_0609, grid_0614 })).Row);
            //grid_0566.Children.Add((new GridRow(0, new View[] { cbStudioSetMotionalSurround })).Row);
            //grid_0567.Children.Add((new GridRow(0, new View[] { textblock_0571, cbStudioSetMotionalSurroundRoomType })).Row);
            //grid_0572.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundAmbienceLevel, slStudioSetMotionalSurroundAmbienceLevel })).Row);
            //grid_0576.Children.Add((new GridRow(0, new View[] { textblock_0580, cbStudioSetMotionalSurroundRoomSize })).Row);
            //grid_0581.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundAmbienceTime, slStudioSetMotionalSurroundAmbienceTime })).Row);
            //grid_0585.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundAmbienceDensity, slStudioSetMotionalSurroundAmbienceDensity })).Row);
            //grid_0589.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundAmbienceHFDamp, slStudioSetMotionalSurroundAmbienceHFDamp })).Row);
            //grid_0593.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundExternalPartLR, slStudioSetMotionalSurroundExternalPartLR })).Row);
            //grid_0597.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundExternalPartFB, slStudioSetMotionalSurroundExternalPartFB })).Row);
            //grid_0601.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundExtPartWidth, slStudioSetMotionalSurroundExtPartWidth })).Row);
            //grid_0605.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundExtpartAmbienceSend, slStudioSetMotionalSurroundExtpartAmbienceSend })).Row);
            //grid_0609.Children.Add((new GridRow(0, new View[] { textblock_0613, cbStudioSetMotionalSurroundExtPartControl })).Row);
            //grid_0614.Children.Add((new GridRow(0, new View[] { tbStudioSetMotionalSurroundDepth, slStudioSetMotionalSurroundDepth })).Row);
            //StudioSetMasterEQ.Children.Add((new GridRow(0, new View[] { grid_0643, grid_0647, grid_0651, grid_0655, grid_0659, grid_0663, grid_0667 })).Row);
            //grid_0643.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqLowFreq, cbStudioSetMasterEqLowFreq })).Row);
            //grid_0647.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqLowGain, slStudioSetMasterEqLowGain })).Row);
            //grid_0651.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqMidFreq, cbStudioSetMasterEqMidFreq })).Row);
            //grid_0655.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqMidGain, slStudioSetMasterEqMidGain })).Row);
            //grid_0659.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqMidQ, cbStudioSetMasterEqMidQ })).Row);
            //grid_0663.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqHighFreq, cbStudioSetMasterEqHighFreq })).Row);
            //grid_0667.Children.Add((new GridRow(0, new View[] { tbStudioSetMasterEqHighGain, slStudioSetMasterEqHighGain })).Row);
            //grid_PartSelector.Children.Add((new GridRow(0, new View[] { cbStudioSetPartSelector })).Row);
            //grid_PartSettings.Children.Add((new GridRow(0, new View[] { cbStudioSetPartSubSelector })).Row);
            //grid_StudioSetPartSubSelector.Children.Add((new GridRow(0, new View[] { StudioSetCurrentToneName })).Row);
            //StudioSetPartSettings1.Children.Add((new GridRow(0, new View[] { grid_0695, grid_0699, grid_0703, grid_0708, grid_0712, grid_0716, grid_0720, grid_0724, grid_0728, grid_0732, grid_0737, grid_0742, grid_0746, grid_0751 })).Row);
            //grid_0695.Children.Add((new GridRow(0, new View[] { cbStudioSetPartSettings1Receive, cbStudioSetPartSettings1ReceiveChannel })).Row);
            //grid_0699.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1Group, cbStudioSetPartSettings1Group })).Row);
            //grid_0703.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1Category, cbStudioSetPartSettings1Category })).Row);
            //grid_0708.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1Program, cbStudioSetPartSettings1Program })).Row);
            //grid_0712.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1Search, cbStudioSetPartSettings1Search })).Row);
            //grid_0716.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1Level, slStudioSetPartSettings1Level })).Row);
            //grid_0720.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1Pan, slStudioSetPartSettings1Pan })).Row);
            //grid_0724.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1CoarseTune, slStudioSetPartSettings1CoarseTune })).Row);
            //grid_0728.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1FineTune, slStudioSetPartSettings1FineTune })).Row);
            //grid_0732.Children.Add((new GridRow(0, new View[] { textblock_0736, cbStudioSetPartSettings1MonoPoly })).Row);
            //grid_0737.Children.Add((new GridRow(0, new View[] { textblock_0741, cbStudioSetPartSettings1Legato })).Row);
            //grid_0742.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1PitchBendRange, slStudioSetPartSettings1PitchBendRange })).Row);
            //grid_0746.Children.Add((new GridRow(0, new View[] { textblock_0750, cbStudioSetPartSettings1Portamento })).Row);
            //grid_0751.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings1PortamentoTime, slStudioSetPartSettings1PortamentoTime })).Row);
            //StudioSetPartSettings2.Children.Add((new GridRow(0, new View[] { grid_0773, grid_0777, grid_0781, grid_0785, grid_0789, grid_0793, grid_0797, grid_0801, grid_0805, grid_0809, grid_0813 })).Row);
            //grid_0773.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2CutoffOffset, slStudioSetPartSettings2CutoffOffset })).Row);
            //grid_0777.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2ResonanceOffset, slStudioSetPartSettings2ResonanceOffset })).Row);
            //grid_0781.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2AttackTimeOffset, slStudioSetPartSettings2AttackTimeOffset })).Row);
            //grid_0785.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2DecayTimeOffset, slStudioSetPartSettings2DecayTimeOffset })).Row);
            //grid_0789.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2ReleaseTimeOffset, slStudioSetPartSettings2ReleaseTimeOffset })).Row);
            //grid_0793.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2VibratoRate, slStudioSetPartSettings2VibratoRate })).Row);
            //grid_0797.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2VibratoDepth, slStudioSetPartSettings2VibratoDepth })).Row);
            //grid_0801.Children.Add((new GridRow(0, new View[] { tbStudioSetPartSettings2VibratoDelay, slStudioSetPartSettings2VibratoDelay })).Row);
            //grid_0805.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEffectsChorusSendLevel, slStudioSetPartEffectsChorusSendLevel })).Row);
            //grid_0809.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEffectsReverbSendLevel, slStudioSetPartEffectsReverbSendLevel })).Row);
            //grid_0813.Children.Add((new GridRow(0, new View[] { textblock_0817, cbStudioSetPartEffectsOutputAssign })).Row);
            //StudioSetPartKeyboard.Children.Add((new GridRow(0, new View[] { grid_0831, grid_0835, grid_0839, grid_0843, grid_0847, grid_0851, grid_0855, grid_0859, grid_0863, grid_0867, grid_0871, grid_0872 })).Row);
            //grid_0831.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardOctaveShift, slStudioSetPartKeyboardOctaveShift })).Row);
            //grid_0835.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardVelocitySenseOffset, slStudioSetPartKeyboardVelocitySenseOffset })).Row);
            //grid_0839.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardRangeLower, slStudioSetPartKeyboardRangeLower })).Row);
            //grid_0843.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardRangeUpper, slStudioSetPartKeyboardRangeUpper })).Row);
            //grid_0847.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardFadeWidthLower, slStudioSetPartKeyboardFadeWidthLower })).Row);
            //grid_0851.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardFadeWidthUpper, slStudioSetPartKeyboardFadeWidthUpper })).Row);
            //grid_0855.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardVelocityRangeLower, slStudioSetPartKeyboardVelocityRangeLower })).Row);
            //grid_0859.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardVelocityRangeUpper, slStudioSetPartKeyboardVelocityRangeUpper })).Row);
            //grid_0863.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardVelocityFadeWidthLower, slStudioSetPartKeyboardVelocityFadeWidthLower })).Row);
            //grid_0867.Children.Add((new GridRow(0, new View[] { tbStudioSetPartKeyboardVelocityFadeWidthUpper, slStudioSetPartKeyboardVelocityFadeWidthUpper })).Row);
            //grid_0871.Children.Add((new GridRow(0, new View[] { cbStudioSetPartKeyboardMute })).Row);
            //grid_0872.Children.Add((new GridRow(0, new View[] { textblock_0876, cbStudioSetPartKeyboardVelocityCurveType })).Row);
            //StudioSetPartScaleTune.Children.Add((new GridRow(0, new View[] { grid_0892, grid_0897, grid_0902, grid_0906, grid_0910, grid_0914, grid_0918, grid_0922, grid_0926, grid_0930, grid_0934, grid_0938, grid_0942, grid_0946 })).Row);
            //grid_0892.Children.Add((new GridRow(0, new View[] { textblock_0896, cbStudioSetPartScaleTuneType })).Row);
            //grid_0897.Children.Add((new GridRow(0, new View[] { textblock_0901, cbStudioSetPartScaleTuneKey })).Row);
            //grid_0902.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneC, slStudioSetPartScaleTuneC })).Row);
            //grid_0906.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneCi, slStudioSetPartScaleTuneCi })).Row);
            //grid_0910.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneD, slStudioSetPartScaleTuneD })).Row);
            //grid_0914.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneDi, slStudioSetPartScaleTuneDi })).Row);
            //grid_0918.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneE, slStudioSetPartScaleTuneE })).Row);
            //grid_0922.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneF, slStudioSetPartScaleTuneF })).Row);
            //grid_0926.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneFi, slStudioSetPartScaleTuneFi })).Row);
            //grid_0930.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneG, slStudioSetPartScaleTuneG })).Row);
            //grid_0934.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneGi, slStudioSetPartScaleTuneGi })).Row);
            //grid_0938.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneA, slStudioSetPartScaleTuneA })).Row);
            //grid_0942.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneAi, slStudioSetPartScaleTuneAi })).Row);
            //grid_0946.Children.Add((new GridRow(0, new View[] { tbStudioSetPartScaleTuneB, slStudioSetPartScaleTuneB })).Row);
            //StudioSetPartMidi.Children.Add((new GridRow(0, new View[] { grid_0962, grid_0963, grid_0964, grid_0965, grid_0966, grid_0967, grid_0968, grid_0969, grid_0970, grid_0971, grid_0972 })).Row);
            //grid_0962.Children.Add((new GridRow(0, new View[] { cbStudioSetPartMidiPhaseLock })).Row);
            //grid_0963.Children.Add((new GridRow(0, new View[] { cbStudioSetPartMidiReceiveProgramChange })).Row);
            //grid_0964.Children.Add((new GridRow(0, new View[] { cbStudioSetPartMidiReceiveBankSelect })).Row);
            //grid_0965.Children.Add((new GridRow(0, new View[] { cbStudioSetPartMidiReceivePitchBend })).Row);
            //grid_0966.Children.Add((new GridRow(0, new View[] { cbStudioSetPartMidiReceivePolyphonicKeyPressure })).Row);
            //grid_0967.Children.Add((new GridRow(0, new View[] { cbStudioSetPartMidiReceiveChannelPressure })).Row);
            //grid_0968.Children.Add((new GridRow(0, new View[] { cbStudioSetPartMidiReceiveModulation })).Row);
            //grid_0969.Children.Add((new GridRow(0, new View[] { cbStudioSetPartMidiReceiveVolume })).Row);
            //grid_0970.Children.Add((new GridRow(0, new View[] { cbStudioSetPartMidiReceivePan })).Row);
            //grid_0971.Children.Add((new GridRow(0, new View[] { cbStudioSetPartMidiReceiveExpression })).Row);
            //grid_0972.Children.Add((new GridRow(0, new View[] { cbStudioSetPartMidiReceiveHold1 })).Row);
            //StudioSetPartMotionalSurround.Children.Add((new GridRow(0, new View[] { grid_0979, grid_0983, grid_0987, grid_0991 })).Row);
            //grid_0979.Children.Add((new GridRow(0, new View[] { tbStudioSetPartMotionalSurroundLR, slStudioSetPartMotionalSurroundLR })).Row);
            //grid_0983.Children.Add((new GridRow(0, new View[] { tbStudioSetPartMotionalSurroundFB, slStudioSetPartMotionalSurroundFB })).Row);
            //grid_0987.Children.Add((new GridRow(0, new View[] { tbStudioSetPartMotionalSurroundWidth, slStudioSetPartMotionalSurroundWidth })).Row);
            //grid_0991.Children.Add((new GridRow(0, new View[] { tbStudioSetPartMotionalSurroundAmbienceSendLevel, slStudioSetPartMotionalSurroundAmbienceSendLevel })).Row);
            //StudioSetPartEQ.Children.Add((new GridRow(0, new View[] { grid_1005, grid_1006, grid_1011, grid_1015, grid_1019, grid_1023, grid_1027, grid_1031 })).Row);
            //grid_1005.Children.Add((new GridRow(0, new View[] { cbStudioSetPartEQSwitch })).Row);
            //grid_1006.Children.Add((new GridRow(0, new View[] { textblock_1010, cbStudioSetPartEQLoqFreq })).Row);
            //grid_1011.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEQLowGain, slStudioSetPartEQLowGain })).Row);
            //grid_1015.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEQMidFreq, cbStudioSetPartEQMidFreq })).Row);
            //grid_1019.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEQMidGain, slStudioSetPartEQMidGain })).Row);
            //grid_1023.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEQMidQ, cbStudioSetPartEQMidQ })).Row);
            //grid_1027.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEQHighFreq, cbStudioSetPartEQHighFreq })).Row);
            //grid_1031.Children.Add((new GridRow(0, new View[] { tbStudioSetPartEQHighGain, slStudioSetPartEQHighGain })).Row);
            ////Buttons.Children.Add((new GridRow(0, new View[] { grid_1038, grid_1044 })).Row);
            //grid_1038.Children.Add((new GridRow(0, new View[] { cbStudioSetSlot, textblock_1043, tbStudioSetName })).Row);
            ////grid_1044.Children.Add((new GridRow(0, new View[] { btnFileSave, btnFileLoad, btnStudioSetSave, btnStudioSetDelete, btnStudioSetReturn })).Row);

            //grid_StudioSet_Column0.Children.Add((new GridRow(0, new View[] {
            //    grid_StudioSetSelector, grid_ToneControl1, grid_ToneControl2, grid_ToneControl3, grid_ToneControl4, grid_Tempo, grid_SoloPart/*,
            //    grid_0064, grid_0070, grid_0076, grid_0082, grid_0088, grid_0094, grid_0100, grid_0106, grid_0112, grid_0116, grid_0120, grid_0124*/ })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(0, new View[] { grid_StudioSetSelector })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(1, new View[] { grid_ToneControl1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(2, new View[] { grid_ToneControl2 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(3, new View[] { grid_ToneControl3 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(4, new View[] { grid_ToneControl4 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(5, new View[] { tbTempo, slTempo })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(6, new View[] { tbSoloPart, cbSoloPart })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(7, new View[] { cbReverb, cbChorus, cbMasterEQ }, new byte[] { 1, 1, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(8, new View[] { tbDrumCompEQPart, cbDrumCompEQPart }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(9, new View[] { textblock_0081, cbDrumCompEQ1OutputAssign }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(10, new View[] { textblock_0087, cbDrumCompEQ2OutputAssign }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(11, new View[] { textblock_0093, cbDrumCompEQ3OutputAssign }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(12, new View[] { textblock_0099, cbDrumCompEQ4OutputAssign }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(13, new View[] { textblock_0105, cbDrumCompEQ5OutputAssign }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(14, new View[] { textblock_0111, cbDrumCompEQ6OutputAssign }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(15, new View[] { cbDrumCompEQ, cbExtPartMute }, new byte[] { 2, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(16, new View[] { tbExtPartLevel, slExtPartLevel }, new byte[] { 1, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(17, new View[] { tbExtPartChorusSend, slExtPartChorusSend }, new byte[] { 1, 1 })).Row);
            grid_StudioSet_Column0.Children.Add((new GridRow(18, new View[] { tbExtPartReverbSend, slExtPartReverbSend }, new byte[] { 1, 1 })).Row);

            gEditStudioSetColumn1.Children.Add((new GridRow(0, new View[] {
                grid_Column2Selector, SystemCommonSettings, VoiceReserve, Chorus, ChorusReverb, StudioSetMotionalSurround, StudioSetMasterEQ })).Row);

            gEditStudioSetSearchResult.Children.Add((new GridRow(0, new View[] { lvSearchResults })).Row);

            gEditStudioSetColumn2.Children.Add((new GridRow(0, new View[] {
                grid_PartSelector, grid_PartSettings, grid_StudioSetPartSubSelector, StudioSetPartSettings1, StudioSetPartSettings2,
                StudioSetPartKeyboard, StudioSetPartScaleTune, StudioSetPartMidi, StudioSetPartMotionalSurround, StudioSetPartEQ, Buttons })).Row);

            // Assemble StudioSetEditorStackLayout -----------------------------------------------------------------

            StudioSetEditor_StackLayout = new StackLayout();
            StudioSetEditor_StackLayout.Children.Add((new GridRow(0, new View[] { grid_StudioSet_Column0, gEditStudioSetColumn1, gEditStudioSetSearchResult, gEditStudioSetColumn2 })).Row);
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
                commonState.player.btnPlayStop = btnEditTone_Play;
                if (commonState.player.Playing)
                {
                    btnEditTone_Play.Content = "Stop";
                }
                commonState.reactToMidiIn = CommonState.ReactToMidiIn.EDIT_STUDIO_SET;
            }
            catch { }
            Waiting(true);
            studioSetEditor_State = StudioSetEditor_State.INIT;
            ResetComboBoxes();
            hex2Midi = new Hex2Midi();
            //commonState.midi.midiInPort.MessageReceived += EditStudioSet_MidiInPort_MessageReceived;
            if (commonState.studioSetNames == null)
            {
                grid_PleaseWaitWhileScanning.IsVisible = true;
                //grid_MainStudioSet.IsVisible = false;
                //commonState.midi.midiInPort.MessageReceived += EditStudioSet_MidiInPort_MessageReceived;
                // Get a list of all studio set names. Start by storing the current studio set number.
                // Note that consequent queries will be sent from MidiInPort_MessageReceived and Timer_Tick.
                commonState.studioSetNames = new List<String>();
                QueryCurrentStudioSetNumber(true); // And scan all studio set names.
            }
            else
            {
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
            cbColumn2Selector.Items.Add("System common settings");
            cbColumn2Selector.Items.Add("Voice reserve");
            cbColumn2Selector.Items.Add("Chorus");
            cbColumn2Selector.Items.Add("Reverb");
            cbColumn2Selector.Items.Add("Motional Surround");
            cbColumn2Selector.Items.Add("Master EQ");
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
            cbStudioSetPartEQLoqFreq.Items.Add("200 Hz");
            cbStudioSetPartEQLoqFreq.Items.Add("400 Hz");
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

        private void StudioSetEditor_StartTimer()
        {
            stopEditTimer = false;
            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
            {
                if (stopEditTimer)
                {
                    return false;
                }
                if (commonState.reactToMidiIn != CommonState.ReactToMidiIn.EDIT_STUDIO_SET)
                {
                    return true;
                }
                // When waiting for a response, countdown the time counter:
                if (studioSetEditor_State != StudioSetEditor_State.DONE && studioSetEditor_State != StudioSetEditor_State.NONE)// && waitingForResponseFromIntegra7 > 0)
                {
                    t.Trace("private void Timer_Tick waiting for response");
                    waitingForResponseFromIntegra7--;
                    if (waitingForResponseFromIntegra7 < 1)
                    {
                        // There was a timeout. Handle it:
                        //timer.Stop();
                        Waiting(false);
                        if (studioSetEditor_State == StudioSetEditor_State.INIT)
                        {
                            //commonState.midi.midiInPort.MessageReceived -= EditStudioSet_MidiInPort_MessageReceived;
                            //this.Frame.Navigate(typeof(CommunicationError), commonState);
                        }
                    }
                }

                if (!initDone)
                {
                    if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_TITLES)
                    {
                        t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_TITLES");
                        // This will be handled, so...
                        studioSetEditor_State = StudioSetEditor_State.NONE;

                        // All studio set names has been received and stored in studioSetNames,
                        // populate the combobox:
                        PopulateStudioSetSelector();

                        // Set studio set back according to currentStudioSetNumber:
                        commonState.midi.ProgramChange((byte)15, (byte)85, (byte)0, (byte)(currentStudioSetNumberAsBytes[2] + 1));

                        // Set selector accordingly:
                        previousHandleControlEvents = handleControlEvents;
                        handleControlEvents = false;
                        cbStudioSetSelector.SelectedIndex = currentStudioSetNumberAsBytes[2] + currentStudioSetNumberAsBytes[1] * 128;
                        handleControlEvents = previousHandleControlEvents;

                        // Ask for system common settings:
                        QuerySystemCommon(); // This will be caught in MidiInPort_MessageReceived
                    }
                    else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.SYSTEM_COMMON)
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
                    {
                        t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS");
                        // This will be handled, so...
                        studioSetEditor_State = StudioSetEditor_State.NONE;

                        // Unpack studio set chours and update controls:
                        ReadStudioSetChorus();

                        // Ask for studio set reverb:
                        QueryStudioSetReverb();
                    }
                    else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB)
                    {
                        t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB");
                        // This will be handled, so...
                        studioSetEditor_State = StudioSetEditor_State.NONE;

                        // Unpack studio set reverb and update controls:
                        ReadStudioSetReverb();

                        // Ask for studio set reverb:
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
                    else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MASTER_EQ)
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
                    else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART)
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
                    else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_TONE_NAME)
                    {
                        t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_TONE_NAME");
                        // This will be handled, so...
                        studioSetEditor_State = StudioSetEditor_State.NONE;

                        // Unpack tone name and update controls:
                        ReadStudioSetPartToneName();

                        // Get current tone:
                        commonState.currentTone = new Tone(commonState.toneList.Tones[commonState.toneList.Get(
                            commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectMSB,
                            commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB,
                            commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneProgramNumber)]);
                        StudioSetCurrentToneName.Text = commonState.currentTone.Name;

                        // Get the current tone from commonState:
                        currentToneNumberAsBytes[0] = (byte)(UInt16.Parse(commonState.toneList.Tones[commonState.currentTone.Index][4]));
                        currentToneNumberAsBytes[1] = (byte)(UInt16.Parse(commonState.toneList.Tones[commonState.currentTone.Index][5]));
                        currentToneNumberAsBytes[2] = (byte)(UInt16.Parse(commonState.toneList.Tones[commonState.currentTone.Index][7]));

                        // Now we have MSB, LSB and CB, set comboboxes:
                        previousHandleControlEvents = handleControlEvents;
                        handleControlEvents = false;
                        PopulateCbStudioSetPartSettings1Group();
                        cbStudioSetPartSettings1Group.SelectedItem = commonState.currentTone.Group;
                        PopulateCbStudioSetPartSettings1Category();
                        cbStudioSetPartSettings1Category.SelectedItem = commonState.currentTone.Category;
                        PopulateCbStudioSetPartSettings1Program();
                        cbStudioSetPartSettings1Program.SelectedItem = commonState.currentTone.Name;
                        handleControlEvents = previousHandleControlEvents;

                        // Set current tone on I-7:
                        //commonState.midi.ProgramChange((byte)cbStudioSetPartSettings1ReceiveChannel.SelectedIndex, currentToneNumberAsBytes[0], currentToneNumberAsBytes[1], currentToneNumberAsBytes[2]);

                        // Ask for studio set midi phaselock:
                        QueryStudioSetPartMidiPhaselock();
                    }
                    else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_MIDI_PHASELOCK)
                    {
                        t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_MIDI_PHASELOCK");
                        // This will be handled, so...
                        studioSetEditor_State = StudioSetEditor_State.NONE;

                        // Unpack studio set midi phaselock:
                        ReadStudioSetPartMidiPhaseLock();

                        // Ask for studio set part eq
                        studioSetEditor_PartToRead = 0;
                        QueryStudioSetPartEQ(studioSetEditor_PartToRead);
                    }
                    else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_EQ)
                    {
                        t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_EQ");
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
                            handleControlEvents = true;
                            previousHandleControlEvents = true;
                        }
                    }
                    // After initialization:
                    else if (studioSetEditor_State == StudioSetEditor_State.INIT_DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.NONE)
                    {
                        t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.INIT_DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.NONE");
                        // This will be handled, so...
                        studioSetEditor_State = StudioSetEditor_State.NONE;

                        if (!initialGuiDone)
                        {
                            // Any initiations that requires all initial MIDI communications to be done goes here:
                            // ...
                            Progress.IsVisible = false;
                            SystemCommonSettings.IsVisible = true;
                            VoiceReserve.IsVisible = false;
                            Chorus.IsVisible = false;
                            ChorusChorus.IsVisible = false;
                            ChorusDelay.IsVisible = false;
                            ChorusGM2Chorus.IsVisible = false;
                            switch (commonState.studioSet.CommonChorus.Type)
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
                            grid_PleaseWaitWhileScanning.IsVisible = false;
                            //grid_MainStudioSet.IsVisible = true;
                            cbColumn2Selector.SelectedIndex = 0;

                            // All Studio Set Names was previously read in, so just copy them to the selector:
                            PopulateStudioSetSelector();

                            // Set Studio Set selector accordingly:
                            previousHandleControlEvents = handleControlEvents;
                            handleControlEvents = false;
                            if (commonState != null && commonState.CurrentStudioSet < 64)
                            {
                                cbStudioSetSelector.SelectedIndex = commonState.CurrentStudioSet;
                            }
                            handleControlEvents = previousHandleControlEvents;
                            initialGuiDone = true;
                        }
                        initDone = true;
                        handleControlEvents = true;
                        previousHandleControlEvents = true;
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
                    else if (studioSetEditor_State == StudioSetEditor_State.DONE && currentStudioSetEditorMidiRequest >= StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS
                        && currentStudioSetEditorMidiRequest <= StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_GM2_CHORUS)
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
                        StudioSetCurrentToneName.Text = commonState.currentTone.Name;

                        Int32 toneNumber = commonState.toneList.Get(commonState.currentTone.BankMSB, commonState.currentTone.BankLSB, commonState.currentTone.Program);

                        // Now we have MSB, LSB and CB, fix comboboxes:
                        previousHandleControlEvents = handleControlEvents;
                        handleControlEvents = false;
                        PopulateCbStudioSetPartSettings1Group();
                        cbStudioSetPartSettings1Group.SelectedItem = commonState.toneList.Tones[toneNumber][0];// commonState.currentTone.Group;
                        PopulateCbStudioSetPartSettings1Category();
                        cbStudioSetPartSettings1Category.SelectedItem = commonState.toneList.Tones[toneNumber][1];//commonState.currentTone.Category;
                        PopulateCbStudioSetPartSettings1Program();
                        cbStudioSetPartSettings1Program.SelectedItem = commonState.toneList.Tones[toneNumber][3];//commonState.currentTone.Name;
                        handleControlEvents = previousHandleControlEvents;

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
                        handleControlEvents = true;
                        previousHandleControlEvents = true;
                    }
                    else if (studioSetEditor_State == StudioSetEditor_State.INIT_DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.NONE)
                    {
                        t.Trace("private void Timer_Tick studioSetEditor_State == StudioSetEditor_State.INIT_DONE && currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.NONE");
                        // This will be handled, so...
                        studioSetEditor_State = StudioSetEditor_State.NONE;

                        switch (commonState.studioSet.CommonChorus.Type)
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
                return true;
            });
        }

        private void EditStudioSet_MidiInPort_MessageReceived(/*Windows.Devices.Midi.MidiInPort sender, Windows.Devices.Midi.MidiMessageReceivedEventArgs args*/)
        {
            if (commonState.reactToMidiIn != CommonState.ReactToMidiIn.EDIT_STUDIO_SET)
            {
                return;
            }
            t.Trace("private void EditStudioSet_MidiInPort_MessageReceived (" + "Windows.Devices.Midi.MidiInPort + sender + , Windows.Devices.Midi.MidiMessageReceivedEventArgs + args + , " + ")");
            //IMidiMessage receivedMidiMessage = args.Message;
            //if (receivedMidiMessage.Type == MidiMessageType.SystemExclusive)
            {
                
                //rawData = commonState.midi.raw receivedMidiMessage.RawData.ToArray();
                if ((currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER
                    || currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER_AND_SCAN)
                    && studioSetEditor_State == StudioSetEditor_State.QUERYING_CURRENT_STUDIO_SET_NUMBER)
                {
                    // We have asked for the current studio set number in order to restore it when we have iterated all names:
                    currentStudioSetNumberAsBytes[0] = 0x55;
                    currentStudioSetNumberAsBytes[1] = 0x00;
                    currentStudioSetNumberAsBytes[2] = rawData[13];

                    // Also store in commonState:
                    commonState.CurrentStudioSet = currentStudioSetNumberAsBytes[2];

                    if (currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER_AND_SCAN)
                    {
                        // Start querying all studio sets:
                        currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_TITLES;
                        studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_NAMES;
                        studioSetNumberIndexAsBytes[0] = 0x55;
                        studioSetNumberIndexAsBytes[1] = 0x00;
                        studioSetNumberIndexAsBytes[2] = 0x00;
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
                    commonState.studioSetNames.Add(text);
                    studioSetNumberIndexAsBytes[2]++;

                    // Query next studio set if this was not last one:
                    if (studioSetNumberIndexAsBytes[2] < 64)
                    {
                        // Ask for it:
                        ScanForStudioSetNames(); // Answer will be caught here.
                    }
                    else
                    {
                        // All titles received, set a status that will be caught in Timer_Tick:
                        studioSetEditor_State = StudioSetEditor_State.DONE;
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
        // Communication handlers
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Column 0
        private void cbStudioSetSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (initDone)
            {
                t.Trace("private void cbStudioSetSelector_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
                if (handleControlEvents && studioSetEditor_State != StudioSetEditor_State.INIT && cbStudioSetSelector.SelectedIndex > -1)
                {
                    // Set Studio set according to selected item:
                    currentStudioSetNumberAsBytes[0] = 0x55;
                    currentStudioSetNumberAsBytes[1] = 0x00; // (byte)(cbStudioSetSelector.SelectedIndex / 128);
                    currentStudioSetNumberAsBytes[2] = (byte)cbStudioSetSelector.SelectedIndex; // % 128);
                    SetStudioSet(currentStudioSetNumberAsBytes);

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
            commonState.studioSet.Common.ToneControlSource[0] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x39 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.ToneControlSource[1] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x3a };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.ToneControlSource[2] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x3b };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.ToneControlSource[3] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x3c };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.Tempo = (byte)p;
            tbTempo.Text = "Tempo " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x00, 0x3d };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, Int32ToTwoByteArray((byte)p));
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.SoloPart = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x3f };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.Reverb = p;
            byte[] address = { 0x18, 0x00, 0x00, 0x40 };
            byte[] value = { (byte)((p) ? 0x01 : 0x00) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.Chorus = p;
            byte[] address = { 0x18, 0x00, 0x00, 0x41 };
            byte[] value = { (byte)(p ? 0x01 : 0x00) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.MasterEqualizer = p;
            byte[] address = { 0x18, 0x00, 0x00, 0x42 };
            byte[] value = { (byte)(p ? 0x01 : 0x00) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.DrumCompressorAndEqualizerPart = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x44 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[0] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x45 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[1] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x46 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[2] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x47 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[3] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x48 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[4] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x49 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[5] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x4a };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.DrumCompressorAndEqualizer = p;
            byte[] address = { 0x18, 0x00, 0x00, 0x43 };
            byte[] value = { (byte)(p ? 0x01 : 0x00) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.ExternalPartMute = p;
            byte[] address = { 0x18, 0x00, 0x00, 0x4f };
            byte[] value = { (byte)(p ? 0x01 : 0x00) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.ExternalPartLevel = (byte)p;
            tbTempo.Text = "Ext part level " + slExtPartLevel.Value.ToString();
            byte[] address = { 0x18, 0x00, 0x00, 0x4c };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.ExternalPartChorusSendLevel = (byte)p;
            tbTempo.Text = "Ext part chorus send level " + slExtPartChorusSend.Value.ToString();
            byte[] address = { 0x18, 0x00, 0x00, 0x4d };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.ExternalPartReverbSendLevel = (byte)p;
            tbTempo.Text = "Ext part reverb send level " + slExtPartReverbSend.Value.ToString();
            byte[] address = { 0x18, 0x00, 0x00, 0x4e };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
        }

        // Column 1
        private void cbColumn2Selector_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbColumn2Selector_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetColumn2Selector(cbColumn2Selector.SelectedIndex);
            }
        }

        private void SetStudioSetColumn2Selector(Int32 p)
        {
            SystemCommonSettings.IsVisible = false;
            VoiceReserve.IsVisible = false;
            Chorus.IsVisible = false;
            ChorusReverb.IsVisible = false;
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
                    //cbStudioSetChorusType_SelectionChanged(null, null);
                    break;
                case 3:
                    ChorusReverb.IsVisible = true;
                    //cbStudioSetReverbType_SelectionChanged(null, null);
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
            commonState.studioSet.SystemCommon.MasterTune = p + 1024;
            tbSystemCommonMasterTune.Text = "Master tune " + (slSystemCommonMasterTune.Value / 10).ToString() + " cent";
            byte[] address = { 0x02, 0x00, 0x00, 0x00 };
            byte[] value = new byte[4];
            value[0] = (byte)((commonState.studioSet.SystemCommon.MasterTune & 0xf000) >> 12);
            value[1] = (byte)((commonState.studioSet.SystemCommon.MasterTune & 0x0f00) >> 8);
            value[2] = (byte)((commonState.studioSet.SystemCommon.MasterTune & 0x00f0) >> 4);
            value[3] = (byte)(commonState.studioSet.SystemCommon.MasterTune & 0x000f);
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.MasterKeyShift = (byte)(p + 64); // Translates 40 - 88 into -24 - +24
            tbSystemCommonMasterKeyShift.Text = "Master key shift " + slSystemCommonMasterKeyShift.Value.ToString() + " keys";
            byte[] address = { 0x02, 0x00, 0x00, 0x04 };
            byte[] value = { commonState.studioSet.SystemCommon.MasterKeyShift };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.MasterLevel = (byte)p;
            tbSystemCommonMasterLevel.Text = "Master level " + slSystemCommonMasterLevel.Value.ToString();
            byte[] address = { 0x02, 0x00, 0x00, 0x05 };
            byte[] value = { (byte)commonState.studioSet.SystemCommon.MasterLevel };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.ScaleTuneSwitch = p;
            byte[] address = { 0x02, 0x00, 0x00, 0x06 };
            byte[] value = { (byte)(commonState.studioSet.SystemCommon.ScaleTuneSwitch ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.StudioSetControlChannel = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x11 };
            byte[] value = { (byte)commonState.studioSet.SystemCommon.StudioSetControlChannel };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.SystemControl1Source = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x20 };
            byte[] value = { (byte)commonState.studioSet.SystemCommon.SystemControl1Source };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.SystemControl2Source = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x21 };
            byte[] value = { (byte)commonState.studioSet.SystemCommon.SystemControl2Source };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.SystemControl3Source = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x22 };
            byte[] value = { (byte)commonState.studioSet.SystemCommon.SystemControl3Source };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.SystemControl4Source = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x23 };
            byte[] value = { (byte)commonState.studioSet.SystemCommon.SystemControl4Source };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.ControlSource = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x24 };
            byte[] value = { (byte)commonState.studioSet.SystemCommon.ControlSource };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.SystemClockSource = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x25 };
            byte[] value = { (byte)commonState.studioSet.SystemCommon.SystemClockSource };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.SystemTempo = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x26 };
            byte[] value = { (byte)(commonState.studioSet.SystemCommon.SystemTempo / 16), (byte)(commonState.studioSet.SystemCommon.SystemTempo % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.TempoAssignSource = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x28 };
            byte[] value = { (byte)commonState.studioSet.SystemCommon.TempoAssignSource };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.ReceiveProgramChange = p;
            byte[] address = { 0x02, 0x00, 0x00, 0x29 };
            byte[] value = { (byte)(commonState.studioSet.SystemCommon.ReceiveProgramChange ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.ReceiveBankSelect = p;
            byte[] address = { 0x02, 0x00, 0x00, 0x2a };
            byte[] value = { (byte)(commonState.studioSet.SystemCommon.ReceiveBankSelect ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.SurroundCenterSpeakerSwitch = p;
            byte[] address = { 0x02, 0x00, 0x00, 0x2b };
            byte[] value = { (byte)(commonState.studioSet.SystemCommon.SurroundCenterSpeakerSwitch ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.SurroundSubWooferSwitch = p;
            byte[] address = { 0x02, 0x00, 0x00, 0x2c };
            byte[] value = { (byte)(commonState.studioSet.SystemCommon.SurroundSubWooferSwitch ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.SystemCommon.StereoOutputMode = (byte)p;
            byte[] address = { 0x02, 0x00, 0x00, 0x2d };
            byte[] value = { (byte)commonState.studioSet.SystemCommon.StereoOutputMode };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[0] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x18 };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[0] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[1] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x19 };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[1] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[2] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x1a };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[2] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[3] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x1b };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[3] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[4] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x1c };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[4] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[5] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x1d };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[5] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[6] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x1e };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[6] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[7] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x1f };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[7] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[8] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x20 };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[8] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[9] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x21 };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[9] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[10] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x22 };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[10] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[11] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x23 };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[11] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[12] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x24 };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[12] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[13] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x25 };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[13] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[14] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x26 };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[14] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.Common.VoiceReserve[15] = (byte)p;
            byte[] address = { 0x18, 0x00, 0x00, 0x27 };
            byte[] value = { commonState.studioSet.Common.VoiceReserve[15] };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
        }

        // Chorus events
        private void cbStudioSetChorusType_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetChorusType_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                //MessageDialog warning = new MessageDialog("The INTEGRA-7 does not reveal current settings for the selected chorus type.\r\n" +
                //"If you are content with getting the factory reset standard values, click 'Ok'.\r\n" +
                //"Else, click 'Cancel', write down the settings by using the INTEGRA-7 front panel EFFECTS menu.\r\n" +
                //"Now you may select the type you want to edit, and press 'Ok'.\r\n" +
                //"Edit the values according to your notes, and then make your changes.\r\n\r\n" +
                //"Changin type also causes you to loose any changes done to current type, unless you saved them to the INTEGRA-7 first.\r\n" +
                //"If you wish to save, press 'Cancel' and then use the 'Save' button." );
                //warning.Title = "Warning!";
                //warning.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                //warning.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
                //var response = await warning.ShowAsync();
                //if ((Int32)response.Id == 0)
                //{
                //    SetStudioSetStudioSetChorusType(cbStudioSetChorusType.SelectedIndex);
                //}
                //else
                //{
                //    previousHandleControlEvents = handleControlEvents;
                //    handleControlEvents = false;
                //    cbStudioSetChorusType.SelectedIndex = commonState.studioSet.CommonChorus.Type;
                //    handleControlEvents = previousHandleControlEvents;
                //}
                // Switch type on I-7:
                byte[] address = new byte[] { 0x18, 0x00, 0x04, 0x00 };
                byte[] value = new byte[] { (byte)cbStudioSetChorusType.SelectedIndex };
                byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
                waitingForResponseFromIntegra7 = 500;
                commonState.midi.SendSystemExclusive(bytes);

                // Query parameters of the selected type:
                QueryStudioSetChorus(); // Will eventually be catched in Timer_Tick
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
                    cbChorusChorusFilterType.SelectedIndex = commonState.studioSet.CommonChorus.Chorus.FilterType;
                    tbChorusChorusFilterType.Text = "Filter Type: " + cbChorusChorusFilterType.SelectedItem;
                    cbChorusChorusFilterCutoffFrequency.SelectedIndex = commonState.studioSet.CommonChorus.Chorus.FilterCutoffFrequency;
                    tbChorusChorusFilterCutoffFrequency.Text = "Cutoff Freq: " + cbChorusChorusFilterCutoffFrequency.SelectedItem;
                    slChorusChorusPreDelay.Value = commonState.studioSet.CommonChorus.Chorus.PreDelay;
                    tbChorusChorusPreDelay.Text = "Pre Delay: " + ((Double)commonState.studioSet.CommonChorus.Chorus.PreDelay / (Double)10).ToString();
                    cbChorusChorusRateHzNote.SelectedIndex = commonState.studioSet.CommonChorus.Chorus.RateHzNote;
                    tbChorusChorusRateHz.Text = "Rate in " + cbChorusChorusRateHzNote.SelectedItem;
                    commonState.studioSet.CommonChorus.Chorus.RateHzNote = (byte)cbChorusChorusRateHzNote.SelectedIndex;
                    if (commonState.studioSet.CommonChorus.Chorus.RateHzNote == 0)
                    {
                        slChorusChorusRateHz.Value = commonState.studioSet.CommonChorus.Chorus.RateHz;
                        tbChorusChorusRateHz.Text = "Rate : " + commonState.studioSet.CommonChorus.Chorus.RateHz.ToString();
                        ChorusRateNote.IsVisible = false;
                        ChorusRateHz.IsVisible = true;
                    }
                    else
                    {
                        slChorusChorusRateHz.Value = commonState.studioSet.CommonChorus.Chorus.RateHz;
                        tbChorusChorusRateHz.Text = "Rate : " + StudioSet.NoteString[commonState.studioSet.CommonChorus.Chorus.RateNote];
                        ChorusRateHz.IsVisible = false;
                        ChorusRateNote.IsVisible = true;
                    }
                    slChorusChorusDepth.Value = commonState.studioSet.CommonChorus.Chorus.Depth;
                    tbChorusChorusDepth.Text = "Depth : " + commonState.studioSet.CommonChorus.Chorus.Depth.ToString();
                    slChorusChorusPhase.Value = commonState.studioSet.CommonChorus.Chorus.Phase;
                    tbChorusChorusPhase.Text = "Phase : " + commonState.studioSet.CommonChorus.Chorus.Phase.ToString(); ;
                    slChorusChorusFeedback.Value = commonState.studioSet.CommonChorus.Chorus.Feedback;
                    tbChorusChorusFeedback.Text = "Feedback: " + commonState.studioSet.CommonChorus.Chorus.Feedback.ToString();
                    break;
                case 2:
                    ChorusDelay.IsVisible = true;
                    commonState.studioSet.CommonChorus.Delay.LeftMsNote = (byte)cbChorusDelayLeftMsNote.SelectedIndex;
                    if (commonState.studioSet.CommonChorus.Delay.LeftMsNote == 0)
                    {
                        slChorusDelayLeftLevel.Value = commonState.studioSet.CommonChorus.Delay.LeftMs;
                        tbChorusDelayLeftLevel.Text = "Left Level: " + commonState.studioSet.CommonChorus.Delay.LeftLevel.ToString();
                        ChorusDelayLeftNote.IsVisible = false;
                        ChorusDelayLeftHz.IsVisible = true;
                    }
                    else
                    {
                        slChorusDelayLeftLevel.Value = commonState.studioSet.CommonChorus.Delay.LeftNote;
                        tbChorusDelayLeftLevel.Text = "Left Level: " + StudioSet.NoteString[commonState.studioSet.CommonChorus.Delay.LeftNote];
                        ChorusDelayLeftHz.IsVisible = false;
                        ChorusDelayLeftNote.IsVisible = true;
                    }
                    if (commonState.studioSet.CommonChorus.Delay.RightMsNote == 0)
                    {
                        slChorusDelayRightLevel.Value = commonState.studioSet.CommonChorus.Delay.RightMs;
                        tbChorusDelayRightLevel.Text = "Right Level: " + commonState.studioSet.CommonChorus.Delay.RightLevel.ToString();
                        ChorusDelayRightNote.IsVisible = false;
                        ChorusDelayRightHz.IsVisible = true;
                    }
                    else
                    {
                        slChorusDelayRightLevel.Value = commonState.studioSet.CommonChorus.Delay.RightNote;
                        tbChorusDelayRightLevel.Text = "Right Level: " + StudioSet.NoteString[commonState.studioSet.CommonChorus.Delay.RightNote];
                        ChorusDelayRightHz.IsVisible = false;
                        ChorusDelayRightNote.IsVisible = true;
                    }
                    if (commonState.studioSet.CommonChorus.Delay.CenterMsNote == 0)
                    {
                        slChorusDelayCenterLevel.Value = commonState.studioSet.CommonChorus.Delay.CenterMs;
                        tbChorusDelayCenterLevel.Text = "Center Level: " + commonState.studioSet.CommonChorus.Delay.CenterLevel.ToString();
                        ChorusDelayCenterNote.IsVisible = false;
                        ChorusDelayCenterHz.IsVisible = true;
                    }
                    else
                    {
                        slChorusDelayCenterLevel.Value = commonState.studioSet.CommonChorus.Delay.CenterNote;
                        tbChorusDelayCenterLevel.Text = "Center Level: " + StudioSet.NoteString[commonState.studioSet.CommonChorus.Delay.CenterNote];
                        ChorusDelayCenterHz.IsVisible = false;
                        ChorusDelayCenterNote.IsVisible = true;
                    }
                    slChorusDelayCenterFeedback.Value = commonState.studioSet.CommonChorus.Delay.CenterFeedback;
                    tbChorusDelayCenterFeedback.Text = "Center Feedback: " + commonState.studioSet.CommonChorus.Delay.CenterFeedback.ToString() + "%";
                    cbChorusDelayHFDamp.SelectedIndex = commonState.studioSet.CommonChorus.Delay.HFDamp;
                    tbChorusDelayHFDamp.Text = "HF Damp: " + cbChorusDelayHFDamp.SelectedItem;
                    slChorusDelayLeftLevel.Value = commonState.studioSet.CommonChorus.Delay.LeftLevel;
                    tbChorusDelayLeftLevel.Text = "Left Level: " + commonState.studioSet.CommonChorus.Delay.LeftLevel.ToString();
                    slChorusDelayRightLevel.Value = commonState.studioSet.CommonChorus.Delay.RightLevel;
                    tbChorusDelayRightLevel.Text = "Right Level: " + commonState.studioSet.CommonChorus.Delay.RightLevel.ToString();
                    slChorusDelayCenterLevel.Value = commonState.studioSet.CommonChorus.Delay.CenterLevel;
                    tbChorusDelayCenterLevel.Text = "Center Level: " + commonState.studioSet.CommonChorus.Delay.CenterLevel.ToString();
                    break;
                case 3:
                    ChorusGM2Chorus.IsVisible = true;
                    slChorusGM2ChorusPreLPF.Value = commonState.studioSet.CommonChorus.Gm2Chorus.PreLPF;
                    tbChorusGM2ChorusPreLPF.Text = "Pre LPF: " + commonState.studioSet.CommonChorus.Gm2Chorus.PreLPF.ToString();
                    slChorusGM2ChorusLevel.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Level;
                    tbChorusGM2ChorusLevel.Text = "Level: " + commonState.studioSet.CommonChorus.Gm2Chorus.Level.ToString();
                    slChorusGM2ChorusFeedback.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Feedback;
                    tbChorusGM2ChorusFeedback.Text = "Feedback: " + commonState.studioSet.CommonChorus.Gm2Chorus.Feedback.ToString();
                    slChorusGM2ChorusDelay.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Delay;
                    tbChorusGM2ChorusDelay.Text = "Delay: " + commonState.studioSet.CommonChorus.Gm2Chorus.Delay.ToString();
                    slChorusGM2ChorusRate.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Rate;
                    tbChorusGM2ChorusRate.Text = "Rate: " + commonState.studioSet.CommonChorus.Gm2Chorus.Rate.ToString();
                    slChorusGM2ChorusDepth.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Depth;
                    tbChorusGM2ChorusDepth.Text = "Depth: " + commonState.studioSet.CommonChorus.Gm2Chorus.Depth.ToString();
                    slChorusGM2ChorusSendLevelToReverb.Value = commonState.studioSet.CommonChorus.Gm2Chorus.SendLevelToReverb;
                    tbChorusGM2ChorusSendLevelToReverb.Text = "Send Level to Reverb: " + commonState.studioSet.CommonChorus.Gm2Chorus.SendLevelToReverb.ToString();
                    break;
            }
            // Tell INTEGRA-7 to change chorus type:
            byte[] address = { 0x18, 0x00, 0x04, 0x00 };
            byte[] value = commonState.studioSet.CommonChorus.GetSwitchMessage((byte)p);
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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

        // Chorus chorus events
        private void SetStudioSetChorusOutputAssign(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x04, 0x02 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Chorus.FilterType = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x04 };
            byte[] value = { 0x08, 0x00, 0x00, (byte)(commonState.studioSet.CommonChorus.Chorus.FilterType) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Chorus.FilterCutoffFrequency = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x08 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Chorus.FilterCutoffFrequency / 16),
                    (byte)(commonState.studioSet.CommonChorus.Chorus.FilterCutoffFrequency % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Chorus.PreDelay = (byte)p;
            tbChorusChorusPreDelay.Text = CalculateChorusPreDelay((Int32)slChorusChorusPreDelay.Value);
            byte[] address = { 0x18, 0x00, 0x04, 0x0c };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Chorus.PreDelay / 16),
                    (byte)(commonState.studioSet.CommonChorus.Chorus.PreDelay % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            switch (p)
            {
                case 0:
                    ChorusRateNote.IsVisible = false;
                    ChorusRateHz.IsVisible = true;
                    cbChorusChorusRateHzNote.SelectedIndex = 0;
                    break;
                case 1:
                    ChorusRateHz.IsVisible = false;
                    ChorusRateNote.IsVisible = true;
                    cbChorusChorusRateHzNote.SelectedIndex = 1;
                    break;
            }
            handleControlEvents = previousHandleControlEvents;
            commonState.studioSet.CommonChorus.Chorus.RateHzNote = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x10 };
            byte[] value = { 0x08, 0x00, 0x00, (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
        }

        private void slChorusChorusRateHz_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusChorusRateHz_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusChorusRateHz((Int32)slChorusChorusRateHz.Value);
            }
        }

        private void SetStudioSetChorusChorusRateHz(Int32 p)
        {
            tbChorusChorusRateHz.Text = "Rate " + CalculateTimeHz(p) + " Hz";
            commonState.studioSet.CommonChorus.Chorus.RateHz = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x14 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Chorus.RateHz / 16),
                (byte)(commonState.studioSet.CommonChorus.Chorus.RateHz % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Chorus.RateNote = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x18 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Chorus.RateNote / 16),
                (byte)(commonState.studioSet.CommonChorus.Chorus.RateNote % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Chorus.Depth = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x1c };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Chorus.Depth / 16),
                (byte)(commonState.studioSet.CommonChorus.Chorus.Depth % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            tbChorusChorusPhase.Text = "Phase " + p.ToString();
            commonState.studioSet.CommonChorus.Chorus.Phase = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x20 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Chorus.Phase / 16),
                (byte)(commonState.studioSet.CommonChorus.Chorus.Phase % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Chorus.Feedback = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x24 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Chorus.Feedback / 16),
                (byte)(commonState.studioSet.CommonChorus.Chorus.Feedback % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            switch (p)
            {
                case 0:
                    ChorusDelayLeftHz.IsVisible = true;
                    ChorusDelayLeftNote.IsVisible = false;
                    cbChorusDelayLeftMsNote.SelectedIndex = 0;
                    break;
                case 1:
                    ChorusDelayLeftHz.IsVisible = false;
                    ChorusDelayLeftNote.IsVisible = true;
                    cbChorusDelayLeftMsNote.SelectedIndex = 1;
                    break;
            }
            handleControlEvents = previousHandleControlEvents;
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
            tbChorusDelayLeftHz.Text = "Delay left " + CalculateTimeHz(p) + " ms";
            commonState.studioSet.CommonChorus.Delay.LeftMs = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x08 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Delay.LeftMs / 16),
                    (byte)(commonState.studioSet.CommonChorus.Delay.LeftMs % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Delay.LeftNote = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x0c };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Delay.LeftNote / 16),
                    (byte)(commonState.studioSet.CommonChorus.Delay.LeftNote % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            switch (p)
            {
                case 0:
                    ChorusDelayRightHz.IsVisible = true;
                    ChorusDelayRightNote.IsVisible = false;
                    cbChorusDelayRightMsNote.SelectedIndex = 0;
                    break;
                case 1:
                    ChorusDelayRightHz.IsVisible = false;
                    ChorusDelayRightNote.IsVisible = true;
                    cbChorusDelayRightMsNote.SelectedIndex = 1;
                    break;
            }
            handleControlEvents = previousHandleControlEvents;
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
            tbChorusDelayRightHz.Text = "Delay right " + CalculateTimeHz(p) + " ms";
            commonState.studioSet.CommonChorus.Delay.RightMs = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x14 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Delay.RightMs / 16),
                    (byte)(commonState.studioSet.CommonChorus.Delay.RightMs % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Delay.RightNote = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x18 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Delay.RightNote / 16),
                    (byte)(commonState.studioSet.CommonChorus.Delay.RightNote % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            switch (cbChorusDelayCenterMsNote.SelectedIndex)
            {
                case 0:
                    ChorusDelayCenterHz.IsVisible = true;
                    ChorusDelayCenterNote.IsVisible = false;
                    cbChorusDelayCenterMsNote.SelectedIndex = 0;
                    break;
                case 1:
                    ChorusDelayCenterHz.IsVisible = false;
                    ChorusDelayCenterNote.IsVisible = true;
                    cbChorusDelayCenterMsNote.SelectedIndex = 1;
                    break;
            }
            handleControlEvents = previousHandleControlEvents;
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
            tbChorusDelayCenterHz.Text = "Delay Center " + CalculateTimeHz(p) + " ms";
            commonState.studioSet.CommonChorus.Delay.CenterMs = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x20 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Delay.CenterMs / 16),
                    (byte)(commonState.studioSet.CommonChorus.Delay.CenterMs % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Delay.CenterNote = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x24 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Delay.CenterNote / 16),
                        (byte)(commonState.studioSet.CommonChorus.Delay.CenterNote % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Delay.CenterFeedback = (byte)((p / 2) + 49);
            byte[] address = { 0x18, 0x00, 0x04, 0x28 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Delay.CenterFeedback / 16),
                        (byte)(commonState.studioSet.CommonChorus.Delay.CenterFeedback % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Delay.HFDamp = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x2c };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Delay.HFDamp / 16),
                        (byte)(commonState.studioSet.CommonChorus.Delay.HFDamp % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Delay.LeftLevel = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x30 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Delay.LeftLevel / 16),
                        (byte)(commonState.studioSet.CommonChorus.Delay.LeftLevel % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
        }

        private void slChorusDelayRightevel_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slChorusDelayRightevel_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetChorusDelayRightevel((Int32)slChorusDelayRightLevel.Value);
            }
        }

        private void SetStudioSetChorusDelayRightevel(Int32 p)
        {
            tbChorusDelayRightLevel.Text = "Right level " + p.ToString();
            commonState.studioSet.CommonChorus.Delay.RightLevel = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x34 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Delay.RightLevel / 16),
                        (byte)(commonState.studioSet.CommonChorus.Delay.RightLevel % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.CommonChorus.Delay.CenterLevel = (byte)p;
            byte[] address = { 0x18, 0x00, 0x04, 0x38 };
            byte[] value = { 0x08, 0x00, (byte)(commonState.studioSet.CommonChorus.Delay.CenterLevel / 16),
                        (byte)(commonState.studioSet.CommonChorus.Delay.CenterLevel % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
                //MessageDialog warning = new MessageDialog("The INTEGRA-7 does not reveal current settings for the selected reverb type.\r\n" +
                //"If you are content with getting the factory reset standard values, click 'Ok'.\r\n" +
                //"Else, click 'Cancel', write down the settings by using the INTEGRA-7 front panel EFFECTS menu.\r\n" +
                //"Now you may select the type you want to edit, and press 'Ok'.\r\n" +
                //"Edit the values according to your notes, and then make your changes.\r\n\r\n" +
                //"Changin type also causes you to loose any changes done to current type, unless you saved them to the INTEGRA-7 first.\r\n" +
                //"If you wish to save, press 'Cancel' and then use the 'Save' button.");
                //warning.Title = "Warning!";
                //warning.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                //warning.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
                //var response = await warning.ShowAsync();
                //if ((Int32)response.Id == 0)
                //{
                //    // It is necessary to send the entire content with all parameters when changing type:
                //    SetStudioSetStudioSetReverbType((Int32)cbStudioSetReverbType.SelectedIndex);
                //}
                //else
                //{
                //    previousHandleControlEvents = handleControlEvents;
                //    handleControlEvents = false;
                //    cbStudioSetReverbType.SelectedIndex = commonState.studioSet.CommonReverb.Type;
                //    handleControlEvents = previousHandleControlEvents;
                //}
                byte[] address = new byte[] { 0x18, 0x00, 0x06, 0x00 };
                byte[] value = new byte[] { (byte)cbStudioSetReverbType.SelectedIndex };
                byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
                waitingForResponseFromIntegra7 = 500;
                commonState.midi.SendSystemExclusive(bytes);

                // Query parameters of the selected type:
                QueryStudioSetReverb(); // Will eventually be catched in Timer_Tick
            }
        }

        private void SetStudioSetStudioSetReverbType(Int32 p)
        {
            if (p == 6)
            {
                StudioSetReverbGM2.IsVisible = true;
                StudioSetReverb.IsVisible = false;
            }
            else
            {
                StudioSetReverbGM2.IsVisible = false;
                StudioSetReverb.IsVisible = true;
            }
            SetStudioSetCommonReverbControls(p);
        }

        private void SetStudioSetCommonReverbControls(Int32 p)
        {
            byte[] address = { 0x18, 0x00, 0x06, 0x00 };
            byte[] value = { (byte)p, commonState.studioSet.CommonReverb.Level, commonState.studioSet.CommonReverb.OutputAssign };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

            StudioSet_CommonReverbReverb reverb = null;
            switch (cbStudioSetReverbType.SelectedIndex)
            {
                // NOTE! The attributes was initially named after Hall1, but some names are incorrect
                // while using the same address as Hall1 but control a different setting.
                case 1:
                    reverb = commonState.studioSet.CommonReverb.ReverbRoom1;
                    break;
                case 2:
                    reverb = commonState.studioSet.CommonReverb.ReverbRoom2;
                    break;
                case 3:
                    reverb = commonState.studioSet.CommonReverb.ReverbHall1;
                    break;
                case 4:
                    reverb = commonState.studioSet.CommonReverb.ReverbHall2;
                    break;
                case 5:
                    reverb = commonState.studioSet.CommonReverb.ReverbPlate;
                    break;
            }
            // When the controls are updated below, their handlers will also send 
            // the settings to I-7.
            if (cbStudioSetReverbType.SelectedIndex == 6)
            {
                slStudioSetReverbGM2Character.Value =
                    commonState.studioSet.CommonReverb.GM2Reverb.Character;
                slStudioSetReverbGM2Time.Value =
                    commonState.studioSet.CommonReverb.GM2Reverb.Time;
            }
            else
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
            tbStudioSetReverbLevel.Text = "Reverb level" + p.ToString();
            byte[] address = { 0x18, 0x00, 0x06, 0x01 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            switch (commonState.studioSet.CommonReverb.Type)
            {
                case 1:
                    commonState.studioSet.CommonReverb.ReverbRoom1.PreDelay = (byte)p;
                    break;
                case 2:
                    commonState.studioSet.CommonReverb.ReverbRoom2.PreDelay = (byte)p;
                    break;
                case 3:
                    commonState.studioSet.CommonReverb.ReverbHall1.PreDelay = (byte)p;
                    break;
                case 4:
                    commonState.studioSet.CommonReverb.ReverbHall2.PreDelay = (byte)p;
                    break;
                case 5:
                    commonState.studioSet.CommonReverb.ReverbPlate.PreDelay = (byte)p;
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

            switch (commonState.studioSet.CommonReverb.Type)
            {
                case 1:
                    commonState.studioSet.CommonReverb.ReverbRoom1.Time = (byte)p;
                    break;
                case 2:
                    commonState.studioSet.CommonReverb.ReverbRoom2.Time = (byte)p;
                    break;
                case 3:
                    commonState.studioSet.CommonReverb.ReverbHall1.Time = (byte)p;
                    break;
                case 4:
                    commonState.studioSet.CommonReverb.ReverbHall2.Time = (byte)p;
                    break;
                case 5:
                    commonState.studioSet.CommonReverb.ReverbPlate.Time = (byte)p;
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

            Double value = p / 10;
            String sValue = value.ToString();
            Int32 pos = sValue.IndexOf('.');
            if (pos > -1 && pos < sValue.Length - 2)
            {
                sValue = sValue.Remove(pos + 2);
            }
            tbStudioSetReverbGM2Time.Text = "Time " + sValue + " s";
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
            commonState.studioSet.MotionalSurround.MotionalSurroundSwitch = p;
            byte[] address = { 0x18, 0x00, 0x08, 0x00 };
            byte[] value = { (byte)(p ? 0x01 : 0x00) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MotionalSurround.RoomType = (byte)p;
            byte[] address = { 0x18, 0x00, 0x08, 0x01 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MotionalSurround.AmbienceLevel = (byte)p;
            tbStudioSetMotionalSurroundAmbienceLevel.Text = "Ambience level " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x02 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MotionalSurround.RoomSize = (byte)p;
            byte[] address = { 0x18, 0x00, 0x08, 0x03 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MotionalSurround.AmbienceTime = (byte)p;
            tbStudioSetMotionalSurroundAmbienceTime.Text = "Ambience time " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x04 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MotionalSurround.AmbienceDensity = (byte)p;
            tbStudioSetMotionalSurroundAmbienceDensity.Text = "Ambience density " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x05 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MotionalSurround.AmbienceHFDamp = (byte)p;
            tbStudioSetMotionalSurroundAmbienceHFDamp.Text = "Ambience HF damp " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x06 };
            byte[] value = { (byte)p };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MotionalSurround.ExtPartLR = (byte)(p + 64);
            tbStudioSetMotionalSurroundExternalPartLR.Text = "External part L-R " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x07 };
            byte[] value = { (byte)commonState.studioSet.MotionalSurround.ExtPartLR };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MotionalSurround.ExtPartFB = (byte)(p + 64);
            tbStudioSetMotionalSurroundExternalPartFB.Text = "External part F-B " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x08 };
            byte[] value = { commonState.studioSet.MotionalSurround.ExtPartFB };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MotionalSurround.ExtPartWidth = (byte)p;
            tbStudioSetMotionalSurroundExtPartWidth.Text = "External part width " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x09 };
            byte[] value = { commonState.studioSet.MotionalSurround.ExtPartWidth };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MotionalSurround.ExtPartAmbienceSendLevel = (byte)p;
            tbStudioSetMotionalSurroundExtpartAmbienceSend.Text = "External part ambience send" + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x0a };
            byte[] value = { commonState.studioSet.MotionalSurround.ExtPartAmbienceSendLevel };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MotionalSurround.ExtPartControlChannel = (byte)p;
            byte[] address = { 0x18, 0x00, 0x08, 0x0b };
            byte[] value = { commonState.studioSet.MotionalSurround.ExtPartControlChannel };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MotionalSurround.MotionalSurroundDepth = (byte)p;
            tbStudioSetMotionalSurroundDepth.Text = "Motional surround depth " + p.ToString();
            byte[] address = { 0x18, 0x00, 0x08, 0x0c };
            byte[] value = { commonState.studioSet.MotionalSurround.MotionalSurroundDepth };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MasterEQ.EQLowFreq = (byte)p;
            byte[] address = { 0x18, 0x00, 0x09, 0x00 };
            byte[] value = { commonState.studioSet.MasterEQ.EQLowFreq };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MasterEQ.EQLowGain = (byte)(p + 15);
            tbStudioSetMasterEqLowGain.Text = "EQ low gain " + (p).ToString() + " dB";
            byte[] address = { 0x18, 0x00, 0x09, 0x01 };
            byte[] value = { commonState.studioSet.MasterEQ.EQLowGain };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MasterEQ.EQMidFreq = (byte)p;
            byte[] address = { 0x18, 0x00, 0x09, 0x02 };
            byte[] value = { commonState.studioSet.MasterEQ.EQMidFreq };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MasterEQ.EQMidGain = (byte)(p + 15);
            tbStudioSetMasterEqMidGain.Text = "EQ mid gain " + p.ToString() + " dB";
            byte[] address = { 0x18, 0x00, 0x09, 0x03 };
            byte[] value = { commonState.studioSet.MasterEQ.EQMidGain };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MasterEQ.EQMidQ = (byte)p;
            byte[] address = { 0x18, 0x00, 0x09, 0x04 };
            byte[] value = { commonState.studioSet.MasterEQ.EQMidQ };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MasterEQ.EQHighFreq = (byte)p;
            byte[] address = { 0x18, 0x00, 0x09, 0x05 };
            byte[] value = { commonState.studioSet.MasterEQ.EQHighFreq };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.MasterEQ.EQHighGain = (byte)(p + 15);
            tbStudioSetMasterEqHighGain.Text = "EQ high gain " + p.ToString() + " dB";
            byte[] address = { 0x18, 0x00, 0x09, 0x06 };
            byte[] value = { commonState.studioSet.MasterEQ.EQHighGain };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.midi.SetMidiOutPortChannel((byte)p); // We are actually talking part here, not MIDI channel. MIDI channel migth be changed and is stored in commonState.PartChannels[cbStudioSetPartSelector.SelectedIndex]
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
                Int32 ok = 0; // Assume ok, since we do not always ask

                // Ask only when changing to a channel that is not consistent with selected part:
                if (cbStudioSetPartSettings1ReceiveChannel.SelectedIndex != cbStudioSetPartSelector.SelectedIndex)
                {
                    //    MessageDialog warning = new MessageDialog("INTEGRA-7 normally uses channels 1 thru 16 for parts 1 thru 16.\r\n\r\n" +
                    //    "If you change channel you might end up with two or more parts responding to the same channel.\r\n\r\n" +
                    //    "Also, if you select a part in the librarian, and that part has another channel assigned here, " +
                    //    "you will not hear it, or you will hear wrong tone.\r\n\r\n" +
                    //    "However, this is useful for splitting the keyboard to play different instruments on different parts of the keyboard.\r\n\r\n" +
                    //    "To do that, you set up two or more parts to use the same MIDI channel, and limit their use of the keyboard as you like it.\r\n\r\n" +
                    //    "NOTE! When MIDI channel is changed, making other changes may not work as you expect!\r\n\r\n" +
                    //    "Are you shure you want to do this?");
                    //    warning.Title = "Warning!";
                    //    warning.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
                    //    warning.Commands.Add(new UICommand { Label = "No", Id = 1 });
                    //    IUICommand response = await warning.ShowAsync();
                    //    ok = (Int32)response.Id;
                    //}
                    //if (ok == 0)
                    //{
                    //    SetStudioSetStudioSetPartSettings1ReceiveChannel(cbStudioSetPartSettings1ReceiveChannel.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
                    //}
                    String response = await mainPage.DisplayActionSheet("INTEGRA-7 normally uses channels 1 thru 16 for parts 1 thru 16.\r\n\r\n" +
                        "If you change channel you might end up with two or more parts responding to the same channel.\r\n\r\n" +
                        "Also, if you select a part in the librarian, and that part has another channel assigned here, " +
                        "you will not hear it, or you will hear wrong tone.\r\n\r\n" +
                        "However, this is useful for splitting the keyboard to play different instruments on different parts of the keyboard.\r\n\r\n" +
                        "To do that, you set up two or more parts to use the same MIDI channel, and limit their use of the keyboard as you like it.\r\n\r\n" +
                        "NOTE! When MIDI channel is changed, making other changes may not work as you expect!\r\n\r\n" +
                        "Are you shure you want to do this?",
                        null, null, new String[] { "Yes,", "No" });
                    initDone = true;
                    //if ((Int32)response.Id == 1)
                    if (response == "Yes")
                    {
                        SetStudioSetStudioSetPartSettings1ReceiveChannel(cbStudioSetPartSettings1ReceiveChannel.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
                    }
                }
            }
        }

        private void SetStudioSetStudioSetPartSettings1ReceiveChannel(Int32 p, Int32 part)
        {
            commonState.studioSet.PartMainSettings[part].ReceiveChannel = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + part), 0x00 };
            byte[] value = { commonState.studioSet.PartMainSettings[part].ReceiveChannel };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ReceiveSwitch = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + cbStudioSetPartSelector.SelectedIndex), 0x01 };
            byte[] value = { (byte)(commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ReceiveSwitch ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
                handleControlEvents = false;
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
                handleControlEvents = false;
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
            foreach (String s in commonState.studioSetNames)
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
            SetStudioSet(studioSetNumberIndexAsBytes);

            // Request studio set name:
            byte[] address = { 0x18, 0x00, 0x00, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x10 };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // Answer will be caught in MidiInPort_MessageReceived.
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
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // Answer will be caught in MidiInPort_MessageReceived.
        }

        private void QuerySystemCommon()
        {
            t.Trace("private void QuerySystemCommon()");
            // Ask for system common parameters:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_SYSTEM_COMMON;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.SYSTEM_COMMON;
            byte[] address = { 0x02, 0x00, 0x00, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x2f };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetCommon()
        {
            t.Trace("private void QueryStudioSetCommon()");
            // Ask for current studio set common:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_COMMON;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_COMMON;
            byte[] address = { 0x18, 0x00, 0x00, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x54 };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetChorus(Int32 Selection = 0)
        {
            t.Trace("private void QueryStudioSetChorus (" + "Int32" + Selection + ", " + ")");
            // Ask for current studio set common:
            switch (Selection)
            {
                case 0:
                    studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS;
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS;
                    break;
                case 1:
                    studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS_CHORUS;
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_CHORUS;
                    break;
                case 2:
                    studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS_DELAY;
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_DELAY;
                    break;
                case 3:
                    studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS_GM2_CHORUS;
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_GM2_CHORUS;
                    break;
            }
            byte[] address = { 0x18, 0x00, 0x04, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x54 };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetReverb(Int32 Selection = 0)
        {
            t.Trace("private void QueryStudioSetReverb (" + "Int32" + Selection + ", " + ")");
            // Ask for current studio set common:
            switch (Selection)
            {
                case 0:
                    studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_OFF;
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_OFF;
                    break;
                case 1:
                    studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_ROOM1;
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_ROOM1;
                    break;
                case 2:
                    studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_ROOM2;
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_ROOM2;
                    break;
                case 3:
                    studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_HALL1;
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_HALL1;
                    break;
                case 4:
                    studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_HALL2;
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_HALL2;
                    break;
                case 5: // Intentional fall-through! Those types have the same parameters and use the same xaml controls, just values differs.
                    studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_PLATE;
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_PLATE;
                    break;
                case 6:
                    studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB_GM2_REVERB;
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB_GM2_REVERB;
                    break;
            }
            byte[] address = { 0x18, 0x00, 0x06, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x63 };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetChorusChorus()
        {
            t.Trace("private void QueryStudioSetChorusChorus()");
            // Ask for current studio set common:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS_CHORUS;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS_CHORUS;
            byte[] address = { 0x18, 0x00, 0x04, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x54 };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetChorus()
        {
            t.Trace("private void QueryStudioSetChorus()");
            // Ask for current studio set common:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_CHORUS;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS;
            byte[] address = { 0x18, 0x00, 0x04, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x54 };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetReverb()
        {
            t.Trace("private void QueryStudioSetReverb()");
            // Ask for current studio set reverb:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_REVERB;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB;
            byte[] address = { 0x18, 0x00, 0x06, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x63 };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetMotionalSurround()
        {
            t.Trace("private void QueryStudioSetMotionalSurround()");
            // Ask for current studio set reverb:
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_MOTIONAL_SURROUND;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MOTIONAL_SURROUND;
            byte[] address = { 0x18, 0x00, 0x08, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x10 };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetMasterEQ()
        {
            t.Trace("private void QueryStudioSetMasterEQ()");
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_MASTER_EQ;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MASTER_EQ;
            byte[] address = { 0x18, 0x00, 0x09, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x07 };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
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
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetPartToneName()
        {
            t.Trace("private void QueryStudioSetPartToneName()");
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_PART_TONE_NAME;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_TONE_NAME;
            // Set tone on INTEGRA-7:
            byte msb = commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectMSB;
            byte lsb = commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB;
            byte pc = commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneProgramNumber;
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
            byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(message); // This will be caught in MidiInPort_MessageReceived
        }

        private void QueryStudioSetPartMidiPhaselock()
        {
            t.Trace("private void QueryStudioSetPartMidiPhaselock()");
            studioSetEditor_State = StudioSetEditor_State.QUERYING_STUDIO_SET_PART_MIDI_PHASELOCK;
            currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_MIDI_PHASELOCK;
            byte[] address = { 0x18, 0x00, (byte)(0x10 + cbStudioSetPartSelector.SelectedIndex), 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x01 };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
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
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes); // This will be caught in MidiInPort_MessageReceived
        }

        private void ReadSystemCommon()
        {
            t.Trace("private void ReadSystemCommon()");
            try
            {
                // Unpack system common parameters and update controls:
                commonState.studioSet.SystemCommon = new StudioSet_SystemCommon(new ReceivedData(rawData));
                previousHandleControlEvents = handleControlEvents;
                handleControlEvents = false;
                slSystemCommonMasterTune.Value = commonState.studioSet.SystemCommon.MasterTune;
                tbSystemCommonMasterTune.Text = "Master tune " + (slSystemCommonMasterTune.Value / 10).ToString() + " cent";
                slSystemCommonMasterKeyShift.Value = commonState.studioSet.SystemCommon.MasterKeyShift;
                tbSystemCommonMasterKeyShift.Text = "Master key shift " + slSystemCommonMasterKeyShift.Value.ToString() + " keys";
                slSystemCommonMasterLevel.Value = commonState.studioSet.SystemCommon.MasterLevel;
                tbSystemCommonMasterLevel.Text = "Master level " + slSystemCommonMasterLevel.Value.ToString();
                cbSystemCommonScaleTuneSwitch.IsChecked = commonState.studioSet.SystemCommon.ScaleTuneSwitch;
                cbSystemCommonStudioSetControlChannel.SelectedIndex = commonState.studioSet.SystemCommon.StudioSetControlChannel;
                cbSystemCommonSystemControlSource1.SelectedIndex = commonState.studioSet.SystemCommon.SystemControl1Source;
                cbSystemCommonSystemControlSource2.SelectedIndex = commonState.studioSet.SystemCommon.SystemControl2Source;
                cbSystemCommonSystemControlSource3.SelectedIndex = commonState.studioSet.SystemCommon.SystemControl3Source;
                cbSystemCommonSystemControlSource4.SelectedIndex = commonState.studioSet.SystemCommon.SystemControl4Source;
                cbSystemCommonControlSource.SelectedIndex = commonState.studioSet.SystemCommon.ControlSource;
                cbSystemCommonSystemClockSource.SelectedIndex = commonState.studioSet.SystemCommon.SystemClockSource;
                slSystemCommonSystemTempo.Value = commonState.studioSet.SystemCommon.SystemTempo;
                cbSystemCommonTempoAssignSource.SelectedIndex = commonState.studioSet.SystemCommon.TempoAssignSource;
                cbSystemCommonReceiveProgramChange.IsChecked = commonState.studioSet.SystemCommon.ReceiveProgramChange;
                cbSystemCommonReceiveBankSelect.IsChecked = commonState.studioSet.SystemCommon.ReceiveBankSelect;
                cbSystemCommonSurroundCenterSpeakerSwitch.IsChecked = commonState.studioSet.SystemCommon.SurroundCenterSpeakerSwitch;
                cbSystemCommonSurroundSubWooferSwitch.IsChecked = commonState.studioSet.SystemCommon.SurroundSubWooferSwitch;
                cbSystemCommonStereoOutputMode.SelectedIndex = commonState.studioSet.SystemCommon.StereoOutputMode;
                handleControlEvents = previousHandleControlEvents;
            }
            catch (Exception e)
            {

            }
        }

        private void ReadSelectedStudioSet()
        {
            t.Trace("private void ReadSelectedStudioSet()");
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            try
            {
                // Unpack studio set common and update controls:
                commonState.studioSet.Common = new StudioSet_Common(new ReceivedData(rawData));
                cbStudioSetSelector.SelectedItem = commonState.studioSet.Common.Name;
                slVoiceReserve01.Value = commonState.studioSet.Common.VoiceReserve[0];
                slVoiceReserve02.Value = commonState.studioSet.Common.VoiceReserve[1];
                slVoiceReserve03.Value = commonState.studioSet.Common.VoiceReserve[2];
                slVoiceReserve04.Value = commonState.studioSet.Common.VoiceReserve[3];
                slVoiceReserve05.Value = commonState.studioSet.Common.VoiceReserve[4];
                slVoiceReserve06.Value = commonState.studioSet.Common.VoiceReserve[5];
                slVoiceReserve07.Value = commonState.studioSet.Common.VoiceReserve[6];
                slVoiceReserve08.Value = commonState.studioSet.Common.VoiceReserve[7];
                slVoiceReserve09.Value = commonState.studioSet.Common.VoiceReserve[8];
                slVoiceReserve10.Value = commonState.studioSet.Common.VoiceReserve[9];
                slVoiceReserve11.Value = commonState.studioSet.Common.VoiceReserve[10];
                slVoiceReserve12.Value = commonState.studioSet.Common.VoiceReserve[11];
                slVoiceReserve13.Value = commonState.studioSet.Common.VoiceReserve[12];
                slVoiceReserve14.Value = commonState.studioSet.Common.VoiceReserve[13];
                slVoiceReserve15.Value = commonState.studioSet.Common.VoiceReserve[14];
                slVoiceReserve16.Value = commonState.studioSet.Common.VoiceReserve[15];
                cbToneControl1.SelectedIndex = commonState.studioSet.Common.ToneControlSource[0];
                cbToneControl2.SelectedIndex = commonState.studioSet.Common.ToneControlSource[1];
                cbToneControl3.SelectedIndex = commonState.studioSet.Common.ToneControlSource[2];
                cbToneControl4.SelectedIndex = commonState.studioSet.Common.ToneControlSource[3];
                slTempo.Value = commonState.studioSet.Common.Tempo;
                cbSoloPart.SelectedIndex = commonState.studioSet.Common.SoloPart;
                cbReverb.IsChecked = commonState.studioSet.Common.Reverb;
                cbChorus.IsChecked = commonState.studioSet.Common.Chorus;
                cbMasterEQ.IsChecked = commonState.studioSet.Common.MasterEqualizer;
                cbDrumCompEQ.IsChecked = commonState.studioSet.Common.DrumCompressorAndEqualizer;
                cbDrumCompEQPart.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerPart;
                cbDrumCompEQ1OutputAssign.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[0];
                cbDrumCompEQ2OutputAssign.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[1];
                cbDrumCompEQ3OutputAssign.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[2];
                cbDrumCompEQ4OutputAssign.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[3];
                cbDrumCompEQ5OutputAssign.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[4];
                cbDrumCompEQ6OutputAssign.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[5];
                slExtPartLevel.Value = commonState.studioSet.Common.ExternalPartLevel;
                slExtPartChorusSend.Value = commonState.studioSet.Common.ExternalPartChorusSendLevel;
                slExtPartReverbSend.Value = commonState.studioSet.Common.ExternalPartReverbSendLevel;
                cbExtPartMute.IsChecked = commonState.studioSet.Common.ExternalPartMute;
            }
            catch (Exception e)
            {

            }
            handleControlEvents = previousHandleControlEvents;
        }

        private void ReadStudioSetChorus()
        {
            t.Trace("private void ReadStudioSetChorus()");
            //commonState.studioSet.CommonChorus = new StudioSet_CommonChorus(new ReceivedData(rawData));
            //cbStudioSetChorusType.SelectedIndex = -1;
            //cbStudioSetChorusType.SelectedIndex = commonState.studioSet.CommonChorus.Type;
            //ReadStudioSetChorus(commonState.studioSet.CommonChorus.Type);
            ReadStudioSetChorus(rawData[11]);
        }

        private void ReadStudioSetChorus(byte Selection)
        {
            t.Trace("private void ReadStudioSetChorus (" + "byte" + Selection + ", " + ")");
            commonState.studioSet.CommonChorus = new StudioSet_CommonChorus(new ReceivedData(rawData));
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            cbStudioSetChorusType.SelectedIndex = Selection;
            slChorusLevel.Value = commonState.studioSet.CommonChorus.Level;
            cbChorusOutputAssign.SelectedIndex = commonState.studioSet.CommonChorus.OutputAssign;
            cbChorusOutputSelect.SelectedIndex = commonState.studioSet.CommonChorus.OutputSelect;
            try
            {
                switch (Selection)
                {
                    case 1:
                        // Type is chorus
                        //cbChorusChorusFilterType.SelectedIndex = -1;
                        cbChorusChorusFilterType.SelectedIndex = commonState.studioSet.CommonChorus.Type;
                        //cbChorusChorusFilterCutoffFrequency.SelectedIndex = -1;
                        cbChorusChorusFilterCutoffFrequency.SelectedIndex = commonState.studioSet.CommonChorus.Chorus.FilterCutoffFrequency;
                        tbChorusChorusPreDelay.Text = CalculateChorusPreDelay(commonState.studioSet.CommonChorus.Chorus.PreDelay);
                        slChorusChorusPreDelay.Value = commonState.studioSet.CommonChorus.Chorus.PreDelay;
                        //cbChorusChorusRateHzNote.SelectedIndex = -1;
                        cbChorusChorusRateHzNote.SelectedIndex = commonState.studioSet.CommonChorus.Chorus.RateHzNote;
                        ChorusChorus.IsVisible = true;
                        switch (commonState.studioSet.CommonChorus.Chorus.RateHzNote)
                        {
                            case 0:
                                tbChorusChorusRateHz.Text = "Rate " + CalculateTimeHz(commonState.studioSet.CommonChorus.Chorus.RateHz) + " Hz";
                                slChorusChorusRateHz.Value = 16 * rawData[33] + rawData[34];
                                slChorusChorusRateNote.IsVisible = false;
                                slChorusChorusRateNote.IsVisible = false;
                                tbChorusChorusRateNote.IsVisible = false;
                                slChorusChorusRateHz.IsVisible = true;
                                tbChorusChorusRateHz.IsVisible = true;
                                break;
                            case 1:
                                tbChorusChorusRateNote.Text = "Rate " + CalculateTimeNote(commonState.studioSet.CommonChorus.Chorus.RateNote) + " note";
                                slChorusChorusRateNote.Value = 16 * rawData[33] + rawData[34];
                                slChorusChorusRateHz.IsVisible = false;
                                tbChorusChorusRateHz.IsVisible = false;
                                slChorusChorusRateNote.IsVisible = true;
                                tbChorusChorusRateNote.IsVisible = true;
                                break;
                        }
                        slChorusChorusDepth.Value = commonState.studioSet.CommonChorus.Chorus.Depth;
                        break;
                    case 2:
                        // Type is delay
                        //cbChorusDelayLeftMsNote.SelectedIndex = -1;
                        cbChorusDelayLeftMsNote.SelectedIndex = commonState.studioSet.CommonChorus.Delay.LeftMsNote;
                        switch (commonState.studioSet.CommonChorus.Delay.LeftMsNote)
                        {
                            case 0:
                                slChorusDelayLeftHz.Value = commonState.studioSet.CommonChorus.Delay.LeftMs;
                                ChorusDelayLeftHz.IsVisible = true;
                                ChorusDelayLeftNote.IsVisible = false;
                                break;
                            case 1:
                                slChorusDelayLeftNote.Value = commonState.studioSet.CommonChorus.Delay.LeftNote;
                                ChorusDelayLeftNote.IsVisible = true;
                                ChorusDelayLeftHz.IsVisible = false;
                                break;
                        }
                        //cbChorusDelayRightMsNote.SelectedIndex = -1;
                        cbChorusDelayRightMsNote.SelectedIndex = commonState.studioSet.CommonChorus.Delay.RightMsNote;
                        switch (commonState.studioSet.CommonChorus.Delay.RightMsNote)
                        {
                            case 0:
                                slChorusDelayRightHz.Value = commonState.studioSet.CommonChorus.Delay.RightMs;
                                ChorusDelayRightNote.IsVisible = false;
                                ChorusDelayRightHz.IsVisible = true;
                                break;
                            case 1:
                                slChorusDelayRightNote.Value = commonState.studioSet.CommonChorus.Delay.RightNote;
                                ChorusDelayRightHz.IsVisible = false;
                                ChorusDelayRightNote.IsVisible = true;
                                break;
                        }
                        //cbChorusDelayCenterMsNote.SelectedIndex = -1;
                        cbChorusDelayCenterMsNote.SelectedIndex = commonState.studioSet.CommonChorus.Delay.CenterMsNote;
                        switch (commonState.studioSet.CommonChorus.Delay.CenterMsNote)
                        {
                            case 0:
                                slChorusDelayCenterHz.Value = commonState.studioSet.CommonChorus.Delay.CenterMs;
                                ChorusDelayCenterNote.IsVisible = false;
                                ChorusDelayCenterHz.IsVisible = true;
                                break;
                            case 1:
                                slChorusDelayCenterNote.Value = commonState.studioSet.CommonChorus.Delay.CenterNote;
                                ChorusDelayCenterHz.IsVisible = false;
                                ChorusDelayCenterNote.IsVisible = true;
                                break;
                        }
                        slChorusDelayCenterFeedback.Value = commonState.studioSet.CommonChorus.Delay.CenterFeedback;
                        //cbChorusDelayHFDamp.SelectedIndex = -1;
                        cbChorusDelayHFDamp.SelectedIndex = commonState.studioSet.CommonChorus.Delay.HFDamp;
                        slChorusDelayLeftLevel.Value = commonState.studioSet.CommonChorus.Delay.LeftLevel;
                        slChorusDelayRightLevel.Value = commonState.studioSet.CommonChorus.Delay.LeftLevel;
                        slChorusDelayCenterLevel.Value = commonState.studioSet.CommonChorus.Delay.CenterLevel;
                        ChorusDelay.IsVisible = true;
                        break;
                    case 3:
                        // Type is GM2 chorus
                        slChorusGM2ChorusPreLPF.Value = commonState.studioSet.CommonChorus.Gm2Chorus.PreLPF;
                        tbChorusGM2ChorusPreLPF.Text = "Pre-LPF " + slChorusGM2ChorusPreLPF.Value.ToString();
                        slChorusGM2ChorusLevel.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Level;
                        tbChorusGM2ChorusLevel.Text = "Level " + slChorusGM2ChorusLevel.Value.ToString();
                        slChorusGM2ChorusFeedback.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Feedback;
                        tbChorusGM2ChorusFeedback.Text = "Feedback " + slChorusGM2ChorusFeedback.Value.ToString();
                        slChorusGM2ChorusDelay.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Delay;
                        tbChorusGM2ChorusDelay.Text = "Delay " + slChorusGM2ChorusDelay.Value.ToString();
                        slChorusGM2ChorusRate.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Rate;
                        tbChorusGM2ChorusRate.Text = "Rate " + slChorusGM2ChorusRate.Value.ToString();
                        slChorusGM2ChorusDepth.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Depth;
                        tbChorusGM2ChorusDepth.Text = "Depth " + slChorusGM2ChorusDepth.Value.ToString();
                        slChorusGM2ChorusSendLevelToReverb.Value = commonState.studioSet.CommonChorus.Gm2Chorus.SendLevelToReverb;
                        tbChorusGM2ChorusSendLevelToReverb.Text = "Send level to reverb " + slChorusGM2ChorusSendLevelToReverb.Value.ToString();
                        break;
                }
            }
            catch { }
            handleControlEvents = previousHandleControlEvents;
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
            commonState.studioSet.CommonReverb = new StudioSet_CommonReverb(new ReceivedData(rawData));
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            cbStudioSetReverbType.SelectedIndex = Selection;
            slStudioSetReverbLevel.Value = commonState.studioSet.CommonReverb.Level;
            cbStudioSetReverbOutputAssign.SelectedIndex = commonState.studioSet.CommonReverb.OutputAssign;

            switch (Selection)
            {
                case 1:
                    slStudioSetReverbPreDelay.Value = commonState.studioSet.CommonReverb.ReverbRoom1.PreDelay;
                    tbStudioSetReverbPreDelay.Text = "Pre delay " + commonState.studioSet.CommonReverb.ReverbRoom1.PreDelay.ToString();
                    slStudioSetReverbTime.Value = commonState.studioSet.CommonReverb.ReverbRoom1.Time;
                    tbStudioSetReverbTime.Text = "Time " + commonState.studioSet.CommonReverb.ReverbRoom1.Time.ToString();
                    slStudioSetReverbDensity.Value = commonState.studioSet.CommonReverb.ReverbRoom1.Density;
                    tbStudioSetReverbDensity.Text = "Density " + commonState.studioSet.CommonReverb.ReverbRoom1.Density.ToString();
                    slStudioSetReverbDiffusion.Value = commonState.studioSet.CommonReverb.ReverbRoom1.Diffusion;
                    tbStudioSetReverbDiffusion.Text = "Diffusion " + commonState.studioSet.CommonReverb.ReverbRoom1.Diffusion.ToString();
                    slStudioSetReverbLFDamp.Value = commonState.studioSet.CommonReverb.ReverbRoom1.LFDamp;
                    tbStudioSetReverbLFDamp.Text = "LF damp " + commonState.studioSet.CommonReverb.ReverbRoom1.LFDamp.ToString();
                    slStudioSetReverbHFDamp.Value = commonState.studioSet.CommonReverb.ReverbRoom1.HFDamp;
                    tbStudioSetReverbHFDamp.Text = "HF damp " + commonState.studioSet.CommonReverb.ReverbRoom1.HFDamp.ToString();
                    slStudioSetReverbSpread.Value = commonState.studioSet.CommonReverb.ReverbRoom1.Spread;
                    tbStudioSetReverbSpread.Text = "Spread " + commonState.studioSet.CommonReverb.ReverbRoom1.Spread.ToString();
                    slStudioSetReverbTone.Value = commonState.studioSet.CommonReverb.ReverbRoom1.Tone;
                    tbStudioSetReverbTone.Text = "Tone " + commonState.studioSet.CommonReverb.ReverbRoom1.Tone.ToString();
                    break;
                case 2:
                    slStudioSetReverbPreDelay.Value = commonState.studioSet.CommonReverb.ReverbRoom2.PreDelay;
                    tbStudioSetReverbPreDelay.Text = "Pre delay " + commonState.studioSet.CommonReverb.ReverbRoom2.PreDelay.ToString();
                    slStudioSetReverbTime.Value = commonState.studioSet.CommonReverb.ReverbRoom2.Time;
                    tbStudioSetReverbTime.Text = "Time " + commonState.studioSet.CommonReverb.ReverbRoom2.Time.ToString();
                    slStudioSetReverbDensity.Value = commonState.studioSet.CommonReverb.ReverbRoom2.Density;
                    tbStudioSetReverbDensity.Text = "Density " + commonState.studioSet.CommonReverb.ReverbRoom2.Density.ToString();
                    slStudioSetReverbDiffusion.Value = commonState.studioSet.CommonReverb.ReverbRoom2.Diffusion;
                    tbStudioSetReverbDiffusion.Text = "Diffusion " + commonState.studioSet.CommonReverb.ReverbRoom2.Diffusion.ToString();
                    slStudioSetReverbLFDamp.Value = commonState.studioSet.CommonReverb.ReverbRoom2.LFDamp;
                    tbStudioSetReverbLFDamp.Text = "LF damp " + commonState.studioSet.CommonReverb.ReverbRoom2.LFDamp.ToString();
                    slStudioSetReverbHFDamp.Value = commonState.studioSet.CommonReverb.ReverbRoom2.HFDamp;
                    tbStudioSetReverbHFDamp.Text = "HF damp " + commonState.studioSet.CommonReverb.ReverbRoom2.HFDamp.ToString();
                    slStudioSetReverbSpread.Value = commonState.studioSet.CommonReverb.ReverbRoom2.Spread;
                    tbStudioSetReverbSpread.Text = "Spread " + commonState.studioSet.CommonReverb.ReverbRoom2.Spread.ToString();
                    slStudioSetReverbTone.Value = commonState.studioSet.CommonReverb.ReverbRoom2.Tone;
                    tbStudioSetReverbTone.Text = "Tone " + commonState.studioSet.CommonReverb.ReverbRoom2.Tone.ToString();
                    break;
                case 3:
                    slStudioSetReverbPreDelay.Value = commonState.studioSet.CommonReverb.ReverbHall1.PreDelay;
                    tbStudioSetReverbPreDelay.Text = "Pre delay " + commonState.studioSet.CommonReverb.ReverbHall1.PreDelay.ToString();
                    slStudioSetReverbTime.Value = commonState.studioSet.CommonReverb.ReverbHall1.Time;
                    tbStudioSetReverbTime.Text = "Time " + commonState.studioSet.CommonReverb.ReverbHall1.Time.ToString();
                    slStudioSetReverbDensity.Value = commonState.studioSet.CommonReverb.ReverbHall1.Density;
                    tbStudioSetReverbDensity.Text = "Density " + commonState.studioSet.CommonReverb.ReverbHall1.Density.ToString();
                    slStudioSetReverbDiffusion.Value = commonState.studioSet.CommonReverb.ReverbHall1.Diffusion;
                    tbStudioSetReverbDiffusion.Text = "Diffusion " + commonState.studioSet.CommonReverb.ReverbHall1.Diffusion.ToString();
                    slStudioSetReverbLFDamp.Value = commonState.studioSet.CommonReverb.ReverbHall1.LFDamp;
                    tbStudioSetReverbLFDamp.Text = "LF damp " + commonState.studioSet.CommonReverb.ReverbHall1.LFDamp.ToString();
                    slStudioSetReverbHFDamp.Value = commonState.studioSet.CommonReverb.ReverbHall1.HFDamp;
                    tbStudioSetReverbHFDamp.Text = "HF damp " + commonState.studioSet.CommonReverb.ReverbHall1.HFDamp.ToString();
                    slStudioSetReverbSpread.Value = commonState.studioSet.CommonReverb.ReverbHall1.Spread;
                    tbStudioSetReverbSpread.Text = "Spread " + commonState.studioSet.CommonReverb.ReverbHall1.Spread.ToString();
                    slStudioSetReverbTone.Value = commonState.studioSet.CommonReverb.ReverbHall1.Tone;
                    tbStudioSetReverbTone.Text = "Tone " + commonState.studioSet.CommonReverb.ReverbHall1.Tone.ToString();
                    break;
                case 4:
                    slStudioSetReverbPreDelay.Value = commonState.studioSet.CommonReverb.ReverbHall2.PreDelay;
                    tbStudioSetReverbPreDelay.Text = "Pre delay " + commonState.studioSet.CommonReverb.ReverbHall2.PreDelay.ToString();
                    slStudioSetReverbTime.Value = commonState.studioSet.CommonReverb.ReverbHall2.Time;
                    tbStudioSetReverbTime.Text = "Time " + commonState.studioSet.CommonReverb.ReverbHall2.Time.ToString();
                    slStudioSetReverbDensity.Value = commonState.studioSet.CommonReverb.ReverbHall2.Density;
                    tbStudioSetReverbDensity.Text = "Density " + commonState.studioSet.CommonReverb.ReverbHall2.Density.ToString();
                    slStudioSetReverbDiffusion.Value = commonState.studioSet.CommonReverb.ReverbHall2.Diffusion;
                    tbStudioSetReverbDiffusion.Text = "Diffusion " + commonState.studioSet.CommonReverb.ReverbHall2.Diffusion.ToString();
                    slStudioSetReverbLFDamp.Value = commonState.studioSet.CommonReverb.ReverbHall2.LFDamp;
                    tbStudioSetReverbLFDamp.Text = "LF damp " + commonState.studioSet.CommonReverb.ReverbHall2.LFDamp.ToString();
                    slStudioSetReverbHFDamp.Value = commonState.studioSet.CommonReverb.ReverbHall2.HFDamp;
                    tbStudioSetReverbHFDamp.Text = "HF damp " + commonState.studioSet.CommonReverb.ReverbHall2.HFDamp.ToString();
                    slStudioSetReverbSpread.Value = commonState.studioSet.CommonReverb.ReverbHall2.Spread;
                    tbStudioSetReverbSpread.Text = "Spread " + commonState.studioSet.CommonReverb.ReverbHall2.Spread.ToString();
                    slStudioSetReverbTone.Value = commonState.studioSet.CommonReverb.ReverbHall2.Tone;
                    tbStudioSetReverbTone.Text = "Tone " + commonState.studioSet.CommonReverb.ReverbHall2.Tone.ToString();
                    break;
                case 5:
                    slStudioSetReverbPreDelay.Value = commonState.studioSet.CommonReverb.ReverbPlate.PreDelay;
                    tbStudioSetReverbPreDelay.Text = "Pre delay " + commonState.studioSet.CommonReverb.ReverbPlate.PreDelay.ToString();
                    slStudioSetReverbTime.Value = commonState.studioSet.CommonReverb.ReverbPlate.Time;
                    tbStudioSetReverbTime.Text = "Time " + commonState.studioSet.CommonReverb.ReverbPlate.Time.ToString();
                    slStudioSetReverbDensity.Value = commonState.studioSet.CommonReverb.ReverbPlate.Density;
                    tbStudioSetReverbDensity.Text = "Density " + commonState.studioSet.CommonReverb.ReverbPlate.Density.ToString();
                    slStudioSetReverbDiffusion.Value = commonState.studioSet.CommonReverb.ReverbPlate.Diffusion;
                    tbStudioSetReverbDiffusion.Text = "Diffusion " + commonState.studioSet.CommonReverb.ReverbPlate.Diffusion.ToString();
                    slStudioSetReverbLFDamp.Value = commonState.studioSet.CommonReverb.ReverbPlate.LFDamp;
                    tbStudioSetReverbLFDamp.Text = "LF damp " + commonState.studioSet.CommonReverb.ReverbPlate.LFDamp.ToString();
                    slStudioSetReverbHFDamp.Value = commonState.studioSet.CommonReverb.ReverbPlate.HFDamp;
                    tbStudioSetReverbHFDamp.Text = "HF damp " + commonState.studioSet.CommonReverb.ReverbPlate.HFDamp.ToString();
                    slStudioSetReverbSpread.Value = commonState.studioSet.CommonReverb.ReverbPlate.Spread;
                    tbStudioSetReverbSpread.Text = "Spread " + commonState.studioSet.CommonReverb.ReverbPlate.Spread.ToString();
                    slStudioSetReverbTone.Value = commonState.studioSet.CommonReverb.ReverbPlate.Tone;
                    tbStudioSetReverbTone.Text = "Tone " + commonState.studioSet.CommonReverb.ReverbPlate.Tone.ToString();
                    break;
                case 6:
                    slStudioSetReverbGM2Character.Value = commonState.studioSet.CommonReverb.GM2Reverb.Character;
                    tbStudioSetReverbGM2Time.Text = "Character " + commonState.studioSet.CommonReverb.GM2Reverb.Character.ToString();
                    slStudioSetReverbGM2Time.Value = commonState.studioSet.CommonReverb.GM2Reverb.Time;
                    tbStudioSetReverbGM2Time.Text = "Time " + commonState.studioSet.CommonReverb.GM2Reverb.Time.ToString();
                    break;
            }
            handleControlEvents = previousHandleControlEvents;
        }

        private void ReadMotionalSurround()
        {
            t.Trace("private void ReadMotionalSurround()");
            commonState.studioSet.MotionalSurround = new StudioSet_MotionalSurround(new ReceivedData(rawData));
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            cbStudioSetMotionalSurround.IsChecked = commonState.studioSet.MotionalSurround.MotionalSurroundSwitch;
            cbStudioSetMotionalSurroundRoomType.SelectedIndex = commonState.studioSet.MotionalSurround.RoomType;
            slStudioSetMotionalSurroundAmbienceLevel.Value = commonState.studioSet.MotionalSurround.AmbienceLevel;
            cbStudioSetMotionalSurroundRoomSize.SelectedIndex = commonState.studioSet.MotionalSurround.RoomSize;
            slStudioSetMotionalSurroundAmbienceTime.Value = commonState.studioSet.MotionalSurround.AmbienceTime;
            slStudioSetMotionalSurroundAmbienceDensity.Value = commonState.studioSet.MotionalSurround.AmbienceDensity;
            slStudioSetMotionalSurroundAmbienceHFDamp.Value = commonState.studioSet.MotionalSurround.AmbienceHFDamp;
            slStudioSetMotionalSurroundExternalPartLR.Value = commonState.studioSet.MotionalSurround.ExtPartLR - 64;
            slStudioSetMotionalSurroundExternalPartFB.Value = commonState.studioSet.MotionalSurround.ExtPartFB - 64;
            slStudioSetMotionalSurroundExtPartWidth.Value = commonState.studioSet.MotionalSurround.ExtPartWidth;
            slStudioSetMotionalSurroundExtpartAmbienceSend.Value = commonState.studioSet.MotionalSurround.ExtPartAmbienceSendLevel;
            cbStudioSetMotionalSurroundExtPartControl.SelectedIndex = commonState.studioSet.MotionalSurround.ExtPartControlChannel;
            slStudioSetMotionalSurroundDepth.Value = commonState.studioSet.MotionalSurround.MotionalSurroundDepth;
            tbStudioSetMotionalSurroundAmbienceLevel.Text = "Ambience level " + slStudioSetMotionalSurroundAmbienceLevel.Value.ToString();
            tbStudioSetMotionalSurroundAmbienceTime.Text = "Ambience time " + slStudioSetMotionalSurroundAmbienceTime.Value.ToString();
            tbStudioSetMotionalSurroundAmbienceDensity.Text = "Ambience density " + slStudioSetMotionalSurroundAmbienceDensity.Value.ToString();
            tbStudioSetMotionalSurroundAmbienceHFDamp.Text = "Ambience HF damp " + slStudioSetMotionalSurroundAmbienceHFDamp.Value.ToString();
            tbStudioSetMotionalSurroundExternalPartLR.Text = "External part L-R " + slStudioSetMotionalSurroundExternalPartLR.Value.ToString();
            tbStudioSetMotionalSurroundExternalPartFB.Text = "External part F-B " + slStudioSetMotionalSurroundExternalPartFB.Value.ToString();
            tbStudioSetMotionalSurroundExtPartWidth.Text = "External part width " + slStudioSetMotionalSurroundExtPartWidth.Value.ToString();
            tbStudioSetMotionalSurroundExtpartAmbienceSend.Text = "External part ambience send" + slStudioSetMotionalSurroundExtpartAmbienceSend.Value.ToString();
            tbStudioSetMotionalSurroundDepth.Text = "Motional surround depth " + slStudioSetMotionalSurroundDepth.Value.ToString();
            handleControlEvents = previousHandleControlEvents;
        }

        private void ReadStudioSetMasterEQ()
        {
            t.Trace("private void ReadStudioSetMasterEQ()");
            commonState.studioSet.MasterEQ = new StudioSet_MasterEQ(new ReceivedData(rawData));
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            cbStudioSetMasterEqLowFreq.SelectedIndex = commonState.studioSet.MasterEQ.EQHighFreq;
            slStudioSetMasterEqLowGain.Value = commonState.studioSet.MasterEQ.EQHighGain - 15;
            cbStudioSetMasterEqMidFreq.SelectedIndex = commonState.studioSet.MasterEQ.EQMidFreq;
            slStudioSetMasterEqMidGain.Value = commonState.studioSet.MasterEQ.EQMidGain - 15;
            cbStudioSetMasterEqMidQ.SelectedIndex = commonState.studioSet.MasterEQ.EQMidQ;
            cbStudioSetMasterEqHighFreq.SelectedIndex = commonState.studioSet.MasterEQ.EQHighFreq;
            slStudioSetMasterEqHighGain.Value = commonState.studioSet.MasterEQ.EQHighGain - 15;
            tbStudioSetMasterEqLowGain.Text = "EQ low gain " + (slStudioSetMasterEqLowGain.Value).ToString() + " dB";
            tbStudioSetMasterEqMidGain.Text = "EQ mid gain " + (slStudioSetMasterEqMidGain.Value).ToString() + " dB";
            tbStudioSetMasterEqHighGain.Text = "EQ high gain " + (slStudioSetMasterEqHighGain.Value).ToString() + " dB";
            handleControlEvents = previousHandleControlEvents;
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
            commonState.studioSet.PartMainSettings[(byte)partToRead] = new StudioSet_PartMainSettings(Data);
            commonState.studioSet.PartKeyboard[(byte)partToRead] = new StudioSet_PartKeyboard(Data);
            commonState.studioSet.PartScaleTune[(byte)partToRead] = new StudioSet_PartScaleTune(Data);
            commonState.studioSet.PartMidi[(byte)partToRead] = new StudioSet_PartMidi(Data);
            commonState.studioSet.PartMotionalSurround[(byte)partToRead] = new StudioSet_PartMotionalSurround(Data);
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            // Part settings 1 page
            if (cbStudioSetPartSelector.SelectedIndex > -1 && cbStudioSetPartSelector.SelectedIndex == partToRead)
            {
                cbStudioSetPartSettings1ReceiveChannel.SelectedIndex = commonState.studioSet.PartMainSettings[(byte)partToRead].ReceiveChannel;
                cbStudioSetPartSettings1Receive.IsChecked = commonState.studioSet.PartMainSettings[(byte)partToRead].ReceiveSwitch;
                // These will be set after part is read (and the slider is now a ComboBox)
                //cbStudioSetPartSettings1Group.SelectedIndex = MsbToCbIndex(commonState.studioSet.PartMainSettings[(byte)partToRead].ToneBankSelectMSB);
                commonState.currentTone.BankMSB = commonState.studioSet.PartMainSettings[(byte)partToRead].ToneBankSelectMSB;
                //cbStudioSetPartSettings1Category.SelectedIndex = LsbToCbIndex(commonState.studioSet.PartMainSettings[(byte)partToRead].ToneBankSelectLSB);
                commonState.currentTone.BankLSB = commonState.studioSet.PartMainSettings[(byte)partToRead].ToneBankSelectLSB;
                //cbStudioSetPartSettings1Program.SelectedIndex = commonState.studioSet.PartMainSettings[(byte)partToRead].ToneProgramNumber + 1;
                commonState.currentTone.Program = commonState.studioSet.PartMainSettings[(byte)partToRead].ToneProgramNumber;
                slStudioSetPartSettings1Level.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].Level;
                slStudioSetPartSettings1Pan.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].Pan - 64;
                slStudioSetPartSettings1CoarseTune.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].CoarseTune - 64;
                slStudioSetPartSettings1FineTune.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].FineTune - 64;
                cbStudioSetPartSettings1MonoPoly.SelectedIndex = commonState.studioSet.PartMainSettings[(byte)partToRead].MonoPoly;
                cbStudioSetPartSettings1Legato.SelectedIndex = commonState.studioSet.PartMainSettings[(byte)partToRead].Legato;
                slStudioSetPartSettings1PitchBendRange.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].PitchBendRange;
                cbStudioSetPartSettings1Portamento.SelectedIndex = commonState.studioSet.PartMainSettings[(byte)partToRead].PortamentoSwitch;
                slStudioSetPartSettings1PortamentoTime.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].PortamentoTime;
                // Part settings 2 page
                slStudioSetPartSettings2CutoffOffset.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].CutoffOffset - 64;
                slStudioSetPartSettings2ResonanceOffset.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].ResonanceOffset - 64;
                slStudioSetPartSettings2AttackTimeOffset.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].AttackTimeOffset - 64;
                slStudioSetPartSettings2DecayTimeOffset.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].DecayTimeOffset - 64;
                slStudioSetPartSettings2ResonanceOffset.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].ReleaseTimeOffset - 64;
                slStudioSetPartSettings2VibratoRate.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].VibratoRate - 64;
                slStudioSetPartSettings2VibratoDepth.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].VibratoDepth - 64;
                slStudioSetPartSettings2VibratoDelay.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].VibratoDelay - 64;
                // Part effects (continues previous page):
                slStudioSetPartEffectsChorusSendLevel.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].ChorusSendLevel;
                slStudioSetPartEffectsReverbSendLevel.Value = commonState.studioSet.PartMainSettings[(byte)partToRead].ReverbSendLevel;
                cbStudioSetPartEffectsOutputAssign.SelectedIndex = commonState.studioSet.PartMainSettings[(byte)partToRead].OutputAssign;
                // Part keyboard page
                slStudioSetPartKeyboardOctaveShift.Value = commonState.studioSet.PartKeyboard[(byte)partToRead].OctaveShift - 64;
                slStudioSetPartKeyboardVelocitySenseOffset.Value = commonState.studioSet.PartKeyboard[(byte)partToRead].VelocitySenseOffset - 64;
                slStudioSetPartKeyboardRangeLower.Value = commonState.studioSet.PartKeyboard[(byte)partToRead].RangeLower;
                slStudioSetPartKeyboardRangeUpper.Value = commonState.studioSet.PartKeyboard[(byte)partToRead].RangeUpper;
                slStudioSetPartKeyboardFadeWidthLower.Value = commonState.studioSet.PartKeyboard[(byte)partToRead].FadeWidthLower;
                slStudioSetPartKeyboardFadeWidthUpper.Value = commonState.studioSet.PartKeyboard[(byte)partToRead].FadeWidthUpper;
                slStudioSetPartKeyboardVelocityRangeLower.Value = commonState.studioSet.PartKeyboard[(byte)partToRead].VelocityRangeLower;
                slStudioSetPartKeyboardVelocityRangeUpper.Value = commonState.studioSet.PartKeyboard[(byte)partToRead].VelocityRangeUpper;
                slStudioSetPartKeyboardVelocityFadeWidthLower.Value = commonState.studioSet.PartKeyboard[(byte)partToRead].VelocityFadeWidthLower;
                slStudioSetPartKeyboardVelocityFadeWidthUpper.Value = commonState.studioSet.PartKeyboard[(byte)partToRead].VelocityFadeWidthUpper;
                cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = commonState.studioSet.PartKeyboard[(byte)partToRead].VelocityCurveType;
                cbStudioSetPartKeyboardMute.IsChecked = commonState.studioSet.PartKeyboard[(byte)partToRead].Mute;
                // Scale tune page
                cbStudioSetPartScaleTuneType.SelectedIndex = commonState.studioSet.PartScaleTune[(byte)partToRead].Type;
                cbStudioSetPartScaleTuneKey.SelectedIndex = commonState.studioSet.PartScaleTune[(byte)partToRead].Key;
                slStudioSetPartScaleTuneC.Value = commonState.studioSet.PartScaleTune[(byte)partToRead].C;
                slStudioSetPartScaleTuneCi.Value = commonState.studioSet.PartScaleTune[(byte)partToRead].Ci;
                slStudioSetPartScaleTuneD.Value = commonState.studioSet.PartScaleTune[(byte)partToRead].D;
                slStudioSetPartScaleTuneDi.Value = commonState.studioSet.PartScaleTune[(byte)partToRead].Di;
                slStudioSetPartScaleTuneE.Value = commonState.studioSet.PartScaleTune[(byte)partToRead].E;
                slStudioSetPartScaleTuneF.Value = commonState.studioSet.PartScaleTune[(byte)partToRead].F;
                slStudioSetPartScaleTuneFi.Value = commonState.studioSet.PartScaleTune[(byte)partToRead].Fi;
                slStudioSetPartScaleTuneG.Value = commonState.studioSet.PartScaleTune[(byte)partToRead].G;
                slStudioSetPartScaleTuneGi.Value = commonState.studioSet.PartScaleTune[(byte)partToRead].Gi;
                slStudioSetPartScaleTuneA.Value = commonState.studioSet.PartScaleTune[(byte)partToRead].A;
                slStudioSetPartScaleTuneAi.Value = commonState.studioSet.PartScaleTune[(byte)partToRead].Ai;
                slStudioSetPartScaleTuneB.Value = commonState.studioSet.PartScaleTune[(byte)partToRead].B;
                // Midi page (all but Phase lock)
                cbStudioSetPartMidiReceiveProgramChange.IsChecked = commonState.studioSet.PartMidi[(byte)partToRead].ReceiveProgramChange;
                cbStudioSetPartMidiReceiveBankSelect.IsChecked = commonState.studioSet.PartMidi[(byte)partToRead].ReceiveBankSelect;
                cbStudioSetPartMidiReceivePitchBend.IsChecked = commonState.studioSet.PartMidi[(byte)partToRead].ReceivePitchBend;
                cbStudioSetPartMidiReceivePolyphonicKeyPressure.IsChecked = commonState.studioSet.PartMidi[(byte)partToRead].ReceivePolyphonicKeyPressure;
                cbStudioSetPartMidiReceiveChannelPressure.IsChecked = commonState.studioSet.PartMidi[(byte)partToRead].ReceiveChannelPressure;
                cbStudioSetPartMidiReceiveModulation.IsChecked = commonState.studioSet.PartMidi[(byte)partToRead].ReceiveModulation;
                cbStudioSetPartMidiReceiveVolume.IsChecked = commonState.studioSet.PartMidi[(byte)partToRead].ReceiveVolume;
                cbStudioSetPartMidiReceivePan.IsChecked = commonState.studioSet.PartMidi[(byte)partToRead].ReceivePan;
                cbStudioSetPartMidiReceiveExpression.IsChecked = commonState.studioSet.PartMidi[(byte)partToRead].ReceiveExpression;
                cbStudioSetPartMidiReceiveHold1.IsChecked = commonState.studioSet.PartMidi[(byte)partToRead].ReceiveHold1;
                // Motional surround page
                slStudioSetPartMotionalSurroundLR.Value = commonState.studioSet.PartMotionalSurround[(byte)partToRead].LR;
                slStudioSetPartMotionalSurroundFB.Value = commonState.studioSet.PartMotionalSurround[(byte)partToRead].FB;
                slStudioSetPartMotionalSurroundWidth.Value = commonState.studioSet.PartMotionalSurround[(byte)partToRead].Width;
                slStudioSetPartMotionalSurroundAmbienceSendLevel.Value = commonState.studioSet.PartMotionalSurround[(byte)partToRead].AmbienceSendLevel;
                // Texts for sliders, Part settings 1
                tbStudioSetPartSettings1Group.Text = "Bank select MSB " + (commonState.studioSet.PartMainSettings[(byte)partToRead].ToneBankSelectMSB).ToString();
                tbStudioSetPartSettings1Category.Text = "Bank select LSB " + (commonState.studioSet.PartMainSettings[(byte)partToRead].ToneBankSelectLSB).ToString();
                tbStudioSetPartSettings1Program.Text = "Program number " + (commonState.studioSet.PartMainSettings[(byte)partToRead].ToneProgramNumber + 1).ToString();
                tbStudioSetPartSettings1Level.Text = "Level " + (commonState.studioSet.PartMainSettings[(byte)partToRead].Level).ToString();
                tbStudioSetPartSettings1Pan.Text = "Pan " + (commonState.studioSet.PartMainSettings[(byte)partToRead].Pan - 64).ToString();
                tbStudioSetPartSettings1CoarseTune.Text = "Coarse tune " + (commonState.studioSet.PartMainSettings[(byte)partToRead].CoarseTune - 64).ToString();
                tbStudioSetPartSettings1FineTune.Text = "Fine tune " + (commonState.studioSet.PartMainSettings[(byte)partToRead].FineTune - 64).ToString();
                tbStudioSetPartSettings1PitchBendRange.Text = "Pitch bend range " + (commonState.studioSet.PartMainSettings[(byte)partToRead].PitchBendRange).ToString();
                tbStudioSetPartSettings1PortamentoTime.Text = "Portamento time " + (commonState.studioSet.PartMainSettings[(byte)partToRead].PortamentoTime).ToString();
                // Part settings 2
                tbStudioSetPartSettings2CutoffOffset.Text = "Cutoff offset " + (commonState.studioSet.PartMainSettings[(byte)partToRead].CutoffOffset - 64).ToString();
                tbStudioSetPartSettings2ResonanceOffset.Text = "Resonance offset " + (commonState.studioSet.PartMainSettings[(byte)partToRead].ResonanceOffset - 64).ToString();
                tbStudioSetPartSettings2AttackTimeOffset.Text = "Attack time offset " + (commonState.studioSet.PartMainSettings[(byte)partToRead].AttackTimeOffset - 64).ToString();
                tbStudioSetPartSettings2DecayTimeOffset.Text = "Decay time offset " + (commonState.studioSet.PartMainSettings[(byte)partToRead].DecayTimeOffset - 64).ToString();
                tbStudioSetPartSettings2ReleaseTimeOffset.Text = "Release time offset " + (commonState.studioSet.PartMainSettings[(byte)partToRead].ReleaseTimeOffset - 64).ToString();
                tbStudioSetPartSettings2VibratoRate.Text = "Vibrato rate " + (commonState.studioSet.PartMainSettings[(byte)partToRead].VibratoRate - 64).ToString();
                tbStudioSetPartSettings2VibratoDepth.Text = "Vibrato depth " + (commonState.studioSet.PartMainSettings[(byte)partToRead].VibratoDepth - 64).ToString();
                tbStudioSetPartSettings2VibratoDelay.Text = "Vibrato delay " + (commonState.studioSet.PartMainSettings[(byte)partToRead].VibratoDelay - 64).ToString();
                // Part effects:
                tbStudioSetPartEffectsChorusSendLevel.Text = "Chorus send level " + commonState.studioSet.PartMainSettings[(byte)partToRead].ChorusSendLevel.ToString();
                tbStudioSetPartEffectsReverbSendLevel.Text = "Reverb send level " + commonState.studioSet.PartMainSettings[(byte)partToRead].ReverbSendLevel.ToString();
                // Part keyboard:
                tbStudioSetPartKeyboardOctaveShift.Text = "Octave shift " + (commonState.studioSet.PartKeyboard[(byte)partToRead].OctaveShift - 64).ToString();
                tbStudioSetPartKeyboardVelocitySenseOffset.Text = "Velocity sense offset " + (commonState.studioSet.PartKeyboard[(byte)partToRead].VelocitySenseOffset - 63).ToString();
                tbStudioSetPartKeyboardRangeLower.Text = "Range lower " + (commonState.studioSet.PartKeyboard[(byte)partToRead].RangeLower).ToString();
                tbStudioSetPartKeyboardRangeUpper.Text = "Range upper " + (commonState.studioSet.PartKeyboard[(byte)partToRead].RangeUpper).ToString();
                tbStudioSetPartKeyboardFadeWidthLower.Text = "Fade width lower " + (commonState.studioSet.PartKeyboard[(byte)partToRead].FadeWidthLower).ToString();
                tbStudioSetPartKeyboardFadeWidthUpper.Text = "Fade width uppper " + (commonState.studioSet.PartKeyboard[(byte)partToRead].FadeWidthUpper).ToString();
                tbStudioSetPartKeyboardVelocityRangeLower.Text = "Velocity range lower " + (commonState.studioSet.PartKeyboard[(byte)partToRead].VelocityRangeLower).ToString();
                tbStudioSetPartKeyboardVelocityRangeUpper.Text = "Velocity range upper " + (commonState.studioSet.PartKeyboard[(byte)partToRead].VelocityRangeUpper).ToString();
                tbStudioSetPartKeyboardVelocityFadeWidthLower.Text = "Vel. fade width lower " + (commonState.studioSet.PartKeyboard[(byte)partToRead].VelocityFadeWidthLower).ToString();
                tbStudioSetPartKeyboardVelocityFadeWidthUpper.Text = "Vel. fade width upper " + (commonState.studioSet.PartKeyboard[(byte)partToRead].VelocityFadeWidthUpper).ToString();
                // Scale tune:
                tbStudioSetPartScaleTuneC.Text = "C " + (commonState.studioSet.PartScaleTune[(byte)partToRead].C).ToString();
                tbStudioSetPartScaleTuneCi.Text = "C# " + (commonState.studioSet.PartScaleTune[(byte)partToRead].Ci).ToString();
                tbStudioSetPartScaleTuneD.Text = "D " + (commonState.studioSet.PartScaleTune[(byte)partToRead].D).ToString();
                tbStudioSetPartScaleTuneDi.Text = "D# " + (commonState.studioSet.PartScaleTune[(byte)partToRead].Di).ToString();
                tbStudioSetPartScaleTuneE.Text = "E " + (commonState.studioSet.PartScaleTune[(byte)partToRead].E).ToString();
                tbStudioSetPartScaleTuneF.Text = "F " + (commonState.studioSet.PartScaleTune[(byte)partToRead].F).ToString();
                tbStudioSetPartScaleTuneFi.Text = "F# " + (commonState.studioSet.PartScaleTune[(byte)partToRead].Fi).ToString();
                tbStudioSetPartScaleTuneG.Text = "G " + (commonState.studioSet.PartScaleTune[(byte)partToRead].G).ToString();
                tbStudioSetPartScaleTuneGi.Text = "G# " + (commonState.studioSet.PartScaleTune[(byte)partToRead].Gi).ToString();
                tbStudioSetPartScaleTuneA.Text = "A " + (commonState.studioSet.PartScaleTune[(byte)partToRead].A).ToString();
                tbStudioSetPartScaleTuneAi.Text = "A# " + (commonState.studioSet.PartScaleTune[(byte)partToRead].Ai).ToString();
                tbStudioSetPartScaleTuneB.Text = "B " + (commonState.studioSet.PartScaleTune[(byte)partToRead].B).ToString();
                // Motional surround:
                tbStudioSetPartMotionalSurroundLR.Text = "L-R " + (commonState.studioSet.PartMotionalSurround[(byte)partToRead].LR).ToString();
                tbStudioSetPartMotionalSurroundFB.Text = "F-B " + (commonState.studioSet.PartMotionalSurround[(byte)partToRead].FB).ToString();
                tbStudioSetPartMotionalSurroundWidth.Text = "Width " + (commonState.studioSet.PartMotionalSurround[(byte)partToRead].Width).ToString();
                tbStudioSetPartMotionalSurroundAmbienceSendLevel.Text = "Ambience send level " + (commonState.studioSet.PartMotionalSurround[(byte)partToRead].AmbienceSendLevel).ToString();
                handleControlEvents = previousHandleControlEvents;
            }
        }

        private void ReadStudioSetPartToneName()
        {
            t.Trace("private void ReadStudioSetPartToneName()");
            ReceivedData Data = new ReceivedData(rawData);
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            switch (commonState.SimpleToneType)
            {
                case CommonState.SimpleToneTypes.PCM_SYNTH_TONE:
                    pCMSynthTone = new PCMSynthTone(Data);
                    StudioSetCurrentToneName.Text = commonState.ToneSource +  " PCMS: " + pCMSynthTone.pCMSynthToneCommon.Name;
                    commonState.currentTone.Name = pCMSynthTone.pCMSynthToneCommon.Name;
                    break;
                case CommonState.SimpleToneTypes.PCM_DRUM_KIT:
                    pCMDrumKit = new PCMDrumKit(Data);
                    StudioSetCurrentToneName.Text = commonState.ToneSource + " PCMD: " + pCMDrumKit.pCMDrumKitCommon.Name;
                    commonState.currentTone.Name = pCMDrumKit.pCMDrumKitCommon.Name;
                    break;
                case CommonState.SimpleToneTypes.SUPERNATURAL_ACOUSTIC_TONE:
                    superNATURALAcousticTone = new SuperNATURALAcousticTone(Data);
                    StudioSetCurrentToneName.Text = commonState.ToneSource + " SN-A: " + superNATURALAcousticTone.superNATURALAcousticToneCommon.Name;
                    commonState.currentTone.Name = superNATURALAcousticTone.superNATURALAcousticToneCommon.Name;
                    break;
                case CommonState.SimpleToneTypes.SUPERNATURAL_SYNTH_TONE:
                    superNATURALSynthTone = new SuperNATURALSynthTone(Data);
                    StudioSetCurrentToneName.Text = commonState.ToneSource + " SN-S: " + superNATURALSynthTone.superNATURALSynthToneCommon.Name;
                    commonState.currentTone.Name = superNATURALSynthTone.superNATURALSynthToneCommon.Name;
                    break;
                case CommonState.SimpleToneTypes.SUPERNATURAL_DRUM_KIT:
                    superNATURALDrumKit = new SuperNATURALDrumKit(Data);
                    StudioSetCurrentToneName.Text = commonState.ToneSource + " SN-D: " + superNATURALDrumKit.superNATURALDrumKitCommon.Name;
                    commonState.currentTone.Name = superNATURALDrumKit.superNATURALDrumKitCommon.Name;
                    break;
            }
            handleControlEvents = previousHandleControlEvents;
        }

        private void ReadStudioSetPartMidiPhaseLock()
        {
            t.Trace("private void ReadStudioSetPartMidiPhaseLock()");
            // This is a bit special since we have put part MIDI together with MIDI switches (which must first have been read above!).
            ReceivedData Data = new ReceivedData(rawData);
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            cbStudioSetPartMidiPhaseLock.IsChecked = commonState.studioSet.PartMidi[cbStudioSetPartSelector.SelectedIndex].PhaseLock;
            handleControlEvents = previousHandleControlEvents;
        }

        private void ReadStudioSetPartEQ(Int32 partToRead = -1)
        {
            t.Trace("private void ReadStudioSetPartEQ()");
            if (partToRead == -1)
            {
                partToRead = cbStudioSetPartSelector.SelectedIndex;
            }
            commonState.studioSet.PartEQ[partToRead] = new StudioSet_PartEQ(new ReceivedData(rawData));
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            cbStudioSetPartEQSwitch.IsChecked = commonState.studioSet.PartEQ[(byte)partToRead].EqSwitch;
            cbStudioSetPartEQLoqFreq.SelectedIndex = commonState.studioSet.PartEQ[(byte)partToRead].EqLowFreq;
            slStudioSetPartEQLowGain.Value = commonState.studioSet.PartEQ[(byte)partToRead].EqLowGain;
            cbStudioSetPartEQMidFreq.SelectedIndex = commonState.studioSet.PartEQ[(byte)partToRead].EqMidFreq;
            slStudioSetPartEQMidGain.Value = commonState.studioSet.PartEQ[(byte)partToRead].EqMidGain;
            cbStudioSetPartEQMidQ.SelectedIndex = commonState.studioSet.PartEQ[(byte)partToRead].EqMidQ;
            cbStudioSetPartEQHighFreq.SelectedIndex = commonState.studioSet.PartEQ[(byte)partToRead].EqHighFreq;
            slStudioSetPartEQHighGain.Value = commonState.studioSet.PartEQ[(byte)partToRead].EqHighGain;
            // Slider texts:
            tbStudioSetPartEQLowGain.Text = "Low gain " + (commonState.studioSet.PartEQ[(byte)partToRead].EqLowGain).ToString();
            tbStudioSetPartEQMidGain.Text = "Mid gain " + (commonState.studioSet.PartEQ[(byte)partToRead].EqMidGain).ToString();
            tbStudioSetPartEQHighGain.Text = "High gain " + (commonState.studioSet.PartEQ[(byte)partToRead].EqHighGain).ToString();
            handleControlEvents = previousHandleControlEvents;
        }

        private void UpdateToneFromControls()
        {
            // Update StudioSetCurrentToneName:
            StudioSetCurrentToneName.Text = (String)cbStudioSetPartSettings1Program.SelectedItem;
            // Update commonState:
            commonState.currentTone.Group = (String)cbStudioSetPartSettings1Group.SelectedItem;
            commonState.currentTone.Category = (String)cbStudioSetPartSettings1Category.SelectedItem;
            commonState.currentTone.Name = (String)cbStudioSetPartSettings1Program.SelectedItem;
            commonState.currentTone.Index = commonState.toneList.Get(commonState.currentTone);
            // Update  commonState.studioSet:
            commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectMSB =
                byte.Parse(commonState.toneList.Tones[commonState.currentTone.Index][4]);
            commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB =
                byte.Parse(commonState.toneList.Tones[commonState.currentTone.Index][5]);
            commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneProgramNumber =
                byte.Parse(commonState.toneList.Tones[commonState.currentTone.Index][7]);
            // Update  I-7:
            commonState.midi.ProgramChange((byte)cbStudioSetPartSelector.SelectedIndex,
                byte.Parse(commonState.toneList.Tones[commonState.currentTone.Index][4]),
                byte.Parse(commonState.toneList.Tones[commonState.currentTone.Index][5]),
                byte.Parse(commonState.toneList.Tones[commonState.currentTone.Index][7]));
        }

        private void SendProgramChangeToI_7()
        {
            t.Trace("private void SendProgramChangeToI_7()");
            if (MidiChannelIsSameAsPart())
            {
                byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)cbStudioSetPartSelector.SelectedIndex), 0x06 };
                byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)cbStudioSetPartSelector.SelectedIndex].ToneBankSelectMSB,
                    (byte)commonState.studioSet.PartMainSettings[(byte)cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB,
                    (byte)(commonState.studioSet.PartMainSettings[(byte)cbStudioSetPartSelector.SelectedIndex].ToneProgramNumber % 128) };
                byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
                waitingForResponseFromIntegra7 = 500;
                commonState.midi.SendSystemExclusive(bytes);
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
            commonState.studioSet.PartMainSettings[(byte)part].Level = (byte)p;
            tbStudioSetPartSettings1Level.Text = "Level " + (commonState.studioSet.PartMainSettings[(byte)part].Level).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x09 };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].Level };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].Pan = (byte)(p + 64);
            tbStudioSetPartSettings1Pan.Text = "Pan " + (commonState.studioSet.PartMainSettings[(byte)part].Pan - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x0a };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].Pan };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].CoarseTune = (byte)(p + 64);
            tbStudioSetPartSettings1CoarseTune.Text = "Coarse tune " + (commonState.studioSet.PartMainSettings[(byte)part].CoarseTune - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x0b };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].CoarseTune };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].FineTune = (byte)(p + 64);
            tbStudioSetPartSettings1FineTune.Text = "Fine tune " + (commonState.studioSet.PartMainSettings[(byte)part].FineTune - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x0c };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].FineTune };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].MonoPoly = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x0d };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].MonoPoly };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].Legato = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x0e };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].Legato };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].PitchBendRange = (byte)p;
            tbStudioSetPartSettings1PitchBendRange.Text = "Pitch bend range " +
                (commonState.studioSet.PartMainSettings[(byte)part].PitchBendRange > 24 ? "Tone" :
                (commonState.studioSet.PartMainSettings[(byte)part].PitchBendRange).ToString());
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x0f };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].PitchBendRange };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].PortamentoSwitch = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x10 };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].PortamentoSwitch };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].PortamentoTime = (byte)p;
            tbStudioSetPartSettings1PortamentoTime.Text = "Portamento time " +
                (commonState.studioSet.PartMainSettings[(byte)part].PortamentoTime > 127 ? "Tone" :
                (commonState.studioSet.PartMainSettings[(byte)part].PortamentoTime).ToString());
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x11 };
            byte[] value = { (byte)(commonState.studioSet.PartMainSettings[(byte)part].PortamentoTime / 16), (byte)(commonState.studioSet.PartMainSettings[(byte)part].PortamentoTime % 16) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].CutoffOffset = (byte)(p + 64);
            tbStudioSetPartSettings2CutoffOffset.Text = "Cutoff offset " + (commonState.studioSet.PartMainSettings[(byte)part].CutoffOffset - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x13 };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].CutoffOffset };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].ResonanceOffset = (byte)(p + 64);
            tbStudioSetPartSettings2ResonanceOffset.Text = "Resonance offset " + (commonState.studioSet.PartMainSettings[(byte)part].ResonanceOffset - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x14 };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].ResonanceOffset };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].AttackTimeOffset = (byte)(p + 64);
            tbStudioSetPartSettings2AttackTimeOffset.Text = "Attack time offset " + (commonState.studioSet.PartMainSettings[(byte)part].AttackTimeOffset - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x15 };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].AttackTimeOffset };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].DecayTimeOffset = (byte)(p + 64);
            tbStudioSetPartSettings2DecayTimeOffset.Text = "Decay time offset " + (commonState.studioSet.PartMainSettings[(byte)part].DecayTimeOffset - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x16 };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].DecayTimeOffset };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].ReleaseTimeOffset = (byte)(p + 64);
            tbStudioSetPartSettings2ReleaseTimeOffset.Text = "Release time offset " + (commonState.studioSet.PartMainSettings[(byte)part].ReleaseTimeOffset - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x17 };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].ReleaseTimeOffset };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].VibratoRate = (byte)(p + 64);
            tbStudioSetPartSettings2VibratoRate.Text = "Vibrato rate " + (commonState.studioSet.PartMainSettings[(byte)part].VibratoRate - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x18 };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].VibratoRate };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].VibratoDepth = (byte)(p + 64);
            tbStudioSetPartSettings2VibratoDepth.Text = "Vibrato depth " + (commonState.studioSet.PartMainSettings[(byte)part].VibratoDepth - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x19 };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].VibratoDepth };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].VibratoDelay = (byte)(p + 64);
            tbStudioSetPartSettings2VibratoDelay.Text = "Vibrato delay " + (commonState.studioSet.PartMainSettings[(byte)part].VibratoDelay - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x1a };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].VibratoDelay };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].ChorusSendLevel = (byte)p;
            tbStudioSetPartEffectsChorusSendLevel.Text = "Chorus send level " + commonState.studioSet.PartMainSettings[(byte)part].ChorusSendLevel.ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x27 };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].ChorusSendLevel };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].ReverbSendLevel = (byte)(p);
            tbStudioSetPartEffectsReverbSendLevel.Text = "Reverb send level " + (commonState.studioSet.PartMainSettings[(byte)part].ReverbSendLevel).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x28 };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].ReverbSendLevel };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMainSettings[(byte)part].OutputAssign = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x29 };
            byte[] value = { (byte)commonState.studioSet.PartMainSettings[(byte)part].OutputAssign };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartKeyboard[(byte)part].OctaveShift = (byte)(p + 64);
            tbStudioSetPartKeyboardOctaveShift.Text = "Octave shift " + (commonState.studioSet.PartKeyboard[(byte)part].OctaveShift - 64).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x1b };
            byte[] value = { (byte)commonState.studioSet.PartKeyboard[(byte)part].OctaveShift };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartKeyboard[(byte)part].VelocitySenseOffset = (byte)(p + 63);
            tbStudioSetPartKeyboardVelocitySenseOffset.Text = "Velocity sense offset " + (commonState.studioSet.PartKeyboard[(byte)part].VelocitySenseOffset - 63).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x1c };
            byte[] value = { (byte)commonState.studioSet.PartKeyboard[(byte)part].VelocitySenseOffset };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartKeyboard[(byte)part].RangeLower = (byte)p;
            tbStudioSetPartKeyboardRangeLower.Text = "Range lower " + (commonState.studioSet.PartKeyboard[(byte)part].RangeLower).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x1d };
            byte[] value = { (byte)commonState.studioSet.PartKeyboard[(byte)part].RangeLower };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartKeyboard[(byte)part].RangeUpper = (byte)p;
            tbStudioSetPartKeyboardRangeUpper.Text = "Range upper " + (commonState.studioSet.PartKeyboard[(byte)part].RangeUpper).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x1e };
            byte[] value = { (byte)commonState.studioSet.PartKeyboard[(byte)part].RangeUpper };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartKeyboard[(byte)part].FadeWidthLower = (byte)p;
            tbStudioSetPartKeyboardFadeWidthLower.Text = "Fade width lower " + (commonState.studioSet.PartKeyboard[(byte)part].FadeWidthLower).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x1f };
            byte[] value = { (byte)commonState.studioSet.PartKeyboard[(byte)part].FadeWidthLower };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartKeyboard[(byte)part].FadeWidthUpper = (byte)p;
            tbStudioSetPartKeyboardFadeWidthUpper.Text = "Fade width uppper " + (commonState.studioSet.PartKeyboard[(byte)part].FadeWidthUpper).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x20 };
            byte[] value = { (byte)commonState.studioSet.PartKeyboard[(byte)part].FadeWidthUpper };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartKeyboard[(byte)part].VelocityRangeLower = (byte)p;
            tbStudioSetPartKeyboardVelocityRangeLower.Text = "Velocity range lower " + (commonState.studioSet.PartKeyboard[(byte)part].VelocityRangeLower).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x21 };
            byte[] value = { (byte)commonState.studioSet.PartKeyboard[(byte)part].VelocityRangeLower };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartKeyboard[(byte)part].VelocityRangeUpper = (byte)p;
            tbStudioSetPartKeyboardVelocityRangeUpper.Text = "Velocity range upper " + (commonState.studioSet.PartKeyboard[(byte)part].VelocityRangeUpper).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x22 };
            byte[] value = { (byte)commonState.studioSet.PartKeyboard[(byte)part].VelocityRangeUpper };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartKeyboard[(byte)part].VelocityFadeWidthLower = (byte)p;
            tbStudioSetPartKeyboardVelocityFadeWidthLower.Text = "Vel. fade width lower " + (commonState.studioSet.PartKeyboard[(byte)part].VelocityFadeWidthLower).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x23 };
            byte[] value = { (byte)commonState.studioSet.PartKeyboard[(byte)part].VelocityFadeWidthLower };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartKeyboard[(byte)part].VelocityFadeWidthUpper = (byte)p;
            tbStudioSetPartKeyboardVelocityFadeWidthUpper.Text = "Vel. fade width upper " + (commonState.studioSet.PartKeyboard[(byte)part].VelocityFadeWidthUpper).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x24 };
            byte[] value = { (byte)commonState.studioSet.PartKeyboard[(byte)part].VelocityFadeWidthUpper };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartKeyboardVelocityCurveType_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartKeyboardVelocityCurveType_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartKeyboardVelocityCurveType(cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartKeyboardVelocityCurveType(Int32 p, Int32 part)
        {
            commonState.studioSet.PartKeyboard[(byte)part].VelocityCurveType = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x43 };
            byte[] value = { (byte)commonState.studioSet.PartKeyboard[(byte)part].VelocityCurveType };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartKeyboard[(byte)part].Mute = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x25 };
            byte[] value = { (byte)(commonState.studioSet.PartKeyboard[(byte)part].Mute ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartScaleTune[(byte)part].Type = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x2b };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].Type };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartScaleTune[(byte)part].Key = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x2c };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].Key };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneC_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneC_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneC((Int32)slStudioSetPartScaleTuneC.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneC(Int32 p, Int32 part)
        {
            commonState.studioSet.PartScaleTune[(byte)part].C = (byte)p;
            tbStudioSetPartScaleTuneC.Text = "C " + (commonState.studioSet.PartScaleTune[(byte)part].C).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x2d };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].C };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneCi_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneCi_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneCi((Int32)slStudioSetPartScaleTuneCi.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneCi(Int32 p, Int32 part)
        {
            commonState.studioSet.PartScaleTune[(byte)part].Ci = (byte)p;
            tbStudioSetPartScaleTuneCi.Text = "C# " + (commonState.studioSet.PartScaleTune[(byte)part].Ci).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x2e };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].Ci };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneD_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneD_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneD((Int32)slStudioSetPartScaleTuneD.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneD(Int32 p, Int32 part)
        {
            commonState.studioSet.PartScaleTune[(byte)part].D = (byte)p;
            tbStudioSetPartScaleTuneD.Text = "D " + (commonState.studioSet.PartScaleTune[(byte)part].D).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x2f };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].D };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneDi_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneDi_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneDi((Int32)slStudioSetPartScaleTuneDi.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneDi(Int32 p, Int32 part)
        {
            commonState.studioSet.PartScaleTune[(byte)part].Di = (byte)p;
            tbStudioSetPartScaleTuneDi.Text = "D# " + (commonState.studioSet.PartScaleTune[(byte)part].Di).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x30 };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].Di };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneE_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneE_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneE((Int32)slStudioSetPartScaleTuneE.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneE(Int32 p, Int32 part)
        {
            commonState.studioSet.PartScaleTune[(byte)part].E = (byte)p;
            tbStudioSetPartScaleTuneE.Text = "E " + (commonState.studioSet.PartScaleTune[(byte)part].E).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x31 };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].E };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneF_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneF_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneF((Int32)slStudioSetPartScaleTuneF.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneF(Int32 p, Int32 part)
        {
            commonState.studioSet.PartScaleTune[(byte)part].F = (byte)p;
            tbStudioSetPartScaleTuneF.Text = "F " + (commonState.studioSet.PartScaleTune[(byte)part].F).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x32 };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].F };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneFi_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneFi_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneFi((Int32)slStudioSetPartScaleTuneFi.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneFi(Int32 p, Int32 part)
        {
            commonState.studioSet.PartScaleTune[(byte)part].Fi = (byte)p;
            tbStudioSetPartScaleTuneFi.Text = "F# " + (commonState.studioSet.PartScaleTune[(byte)part].Fi).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x33 };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].Fi };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneG_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneG_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneG((Int32)slStudioSetPartScaleTuneG.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneG(Int32 p, Int32 part)
        {
            commonState.studioSet.PartScaleTune[(byte)part].G = (byte)p;
            tbStudioSetPartScaleTuneG.Text = "G " + (commonState.studioSet.PartScaleTune[(byte)part].G).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x34 };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].G };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneGi_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneGi_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneGi((Int32)slStudioSetPartScaleTuneGi.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneGi(Int32 p, Int32 part)
        {
            commonState.studioSet.PartScaleTune[(byte)part].Gi = (byte)p;
            tbStudioSetPartScaleTuneGi.Text = "G# " + (commonState.studioSet.PartScaleTune[(byte)part].Gi).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x35 };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].Gi };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneA_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneA_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneA((Int32)slStudioSetPartScaleTuneA.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneA(Int32 p, Int32 part)
        {
            commonState.studioSet.PartScaleTune[(byte)part].A = (byte)p;
            tbStudioSetPartScaleTuneA.Text = "A " + (commonState.studioSet.PartScaleTune[(byte)part].A).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x36 };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].A };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneAi_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneAi_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneAi((Int32)slStudioSetPartScaleTuneAi.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneAi(Int32 p, Int32 part)
        {
            commonState.studioSet.PartScaleTune[(byte)part].Ai = (byte)p;
            tbStudioSetPartScaleTuneAi.Text = "A# " + (commonState.studioSet.PartScaleTune[(byte)part].Ai).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x37 };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].Ai };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void slStudioSetPartScaleTuneB_ValueChanged(object sender, EventArgs e)
        {
            t.Trace("private void slStudioSetPartScaleTuneB_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartScaleTuneB((Int32)slStudioSetPartScaleTuneB.Value, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartScaleTuneB(Int32 p, Int32 part)
        {
            commonState.studioSet.PartScaleTune[(byte)part].B = (byte)p;
            tbStudioSetPartScaleTuneB.Text = "B " + (commonState.studioSet.PartScaleTune[(byte)part].B).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x38 };
            byte[] value = { (byte)commonState.studioSet.PartScaleTune[(byte)part].B };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMidi[(byte)part].ReceiveProgramChange = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x39 };
            byte[] value = { (byte)(commonState.studioSet.PartMidi[(byte)part].ReceiveProgramChange ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMidi[(byte)part].ReceiveBankSelect = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x3a };
            byte[] value = { (byte)(commonState.studioSet.PartMidi[(byte)part].ReceiveBankSelect ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMidi[(byte)part].ReceivePitchBend = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x3b };
            byte[] value = { (byte)(commonState.studioSet.PartMidi[(byte)part].ReceivePitchBend ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMidi[(byte)part].ReceivePolyphonicKeyPressure = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x3c };
            byte[] value = { (byte)(commonState.studioSet.PartMidi[(byte)part].ReceivePolyphonicKeyPressure ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMidi[(byte)part].ReceiveChannelPressure = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x3d };
            byte[] value = { (byte)(commonState.studioSet.PartMidi[(byte)part].ReceiveChannelPressure ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMidi[(byte)part].ReceiveModulation = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x3e };
            byte[] value = { (byte)(commonState.studioSet.PartMidi[(byte)part].ReceiveModulation ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMidi[(byte)part].ReceiveVolume = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x3f };
            byte[] value = { (byte)(commonState.studioSet.PartMidi[(byte)part].ReceiveVolume ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMidi[(byte)part].ReceivePan = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x40 };
            byte[] value = { (byte)(commonState.studioSet.PartMidi[(byte)part].ReceivePan ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMidi[(byte)part].ReceiveExpression = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x41 };
            byte[] value = { (byte)(commonState.studioSet.PartMidi[(byte)part].ReceiveExpression ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMidi[(byte)part].ReceiveHold1 = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x42 };
            byte[] value = { (byte)(commonState.studioSet.PartMidi[(byte)part].ReceiveHold1 ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMidi[(byte)part].PhaseLock = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x10 + (byte)part), 0x00 };
            byte[] value = { (byte)(commonState.studioSet.PartMidi[(byte)part].PhaseLock ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMotionalSurround[(byte)part].LR = (byte)p;
            tbStudioSetPartMotionalSurroundLR.Text = "L-R " + (commonState.studioSet.PartMotionalSurround[(byte)part].LR).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x44 };
            byte[] value = { (byte)commonState.studioSet.PartMotionalSurround[(byte)part].LR };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMotionalSurround[(byte)part].FB = (byte)p;
            tbStudioSetPartMotionalSurroundFB.Text = "F-B " + (commonState.studioSet.PartMotionalSurround[(byte)part].FB).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x46 };
            byte[] value = { (byte)commonState.studioSet.PartMotionalSurround[(byte)part].FB };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMotionalSurround[(byte)part].Width = (byte)p;
            tbStudioSetPartMotionalSurroundWidth.Text = "Width " + (commonState.studioSet.PartMotionalSurround[(byte)part].Width).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x48 };
            byte[] value = { (byte)commonState.studioSet.PartMotionalSurround[(byte)part].Width };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartMotionalSurround[(byte)part].AmbienceSendLevel = (byte)p;
            tbStudioSetPartMotionalSurroundAmbienceSendLevel.Text = "Ambience send level " + (commonState.studioSet.PartMotionalSurround[(byte)part].AmbienceSendLevel).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x20 + (byte)part), 0x49 };
            byte[] value = { (byte)commonState.studioSet.PartMotionalSurround[(byte)part].AmbienceSendLevel };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartEQ[(byte)part].EqSwitch = (Boolean)p;
            byte[] address = { 0x18, 0x00, (byte)(0x50 + (byte)part), 0x00 };
            byte[] value = { (byte)(commonState.studioSet.PartEQ[(byte)part].EqSwitch ? 1 : 0) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

        }

        private void cbStudioSetPartEQLoqFreq_SelectionChanged(object sender, EventArgs e)
        {
            t.Trace("private void cbStudioSetPartEQLoqFreq_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                SetStudioSetStudioSetPartEQLoqFreq(cbStudioSetPartEQLoqFreq.SelectedIndex, cbStudioSetPartSelector.SelectedIndex);
            }
        }

        private void SetStudioSetStudioSetPartEQLoqFreq(Int32 p, Int32 part)
        {
            commonState.studioSet.PartEQ[(byte)part].EqLowFreq = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x50 + (byte)part), 0x01 };
            byte[] value = { (byte)commonState.studioSet.PartEQ[(byte)part].EqLowFreq };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqLowGain = (byte)(p + 15);
            tbStudioSetPartEQLowGain.Text = "Low gain " + (commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqLowGain - 15).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x50 + cbStudioSetPartSelector.SelectedIndex), 0x02 };
            byte[] value = { (byte)(commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqLowGain) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidFreq = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x50 + cbStudioSetPartSelector.SelectedIndex), 0x03 };
            byte[] value = { (byte)commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidFreq };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidGain = (byte)(p + 15);
            tbStudioSetPartEQMidGain.Text = "Mid gain " + (commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidGain - 15).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x50 + cbStudioSetPartSelector.SelectedIndex), 0x04 };
            byte[] value = { (byte)(commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidGain) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidQ = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x50 + cbStudioSetPartSelector.SelectedIndex), 0x05 };
            byte[] value = { (byte)commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqMidQ };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqHighFreq = (byte)p;
            byte[] address = { 0x18, 0x00, (byte)(0x50 + cbStudioSetPartSelector.SelectedIndex), 0x06 };
            byte[] value = { (byte)commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqHighFreq };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
            commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqHighGain = (byte)((Int32)p + 15);
            tbStudioSetPartEQHighGain.Text = "High gain " + (commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqHighGain - 15).ToString();
            byte[] address = { 0x18, 0x00, (byte)(0x50 + cbStudioSetPartSelector.SelectedIndex), 0x07 };
            byte[] value = { (byte)(commonState.studioSet.PartEQ[cbStudioSetPartSelector.SelectedIndex].EqHighGain) };
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);

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
                        byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, data);
                        waitingForResponseFromIntegra7 = 500;
                        commonState.midi.SendSystemExclusive(bytes);
                        // Save all partials:

                        // Make INTEGRA-7 save the Studio Set:
                        address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                        data = new byte[] { 0x55, 0x00, (byte)(cbStudioSetSlot.SelectedIndex), 0x7f };
                        bytes = commonState.midi.SystemExclusiveRQ1Message(address, data);
                        waitingForResponseFromIntegra7 = 500;
                        commonState.midi.SendSystemExclusive(bytes);

                        // Add the new studio set name to the studio set selector:
                        cbStudioSetSelector.Items[cbStudioSetSlot.SelectedIndex] = cbStudioSetSlot.SelectedIndex.ToString() + " " + tbStudioSetName.Text;

                        // Add the new studio set name to commonState.studioSetNames.
                        commonState.studioSetNames[cbStudioSetSlot.SelectedIndex] = tbStudioSetName.Text;

                        // Select the new studio set:
                        previousHandleControlEvents = handleControlEvents;
                        handleControlEvents = false;
                        cbStudioSetSelector.SelectedIndex = cbStudioSetSlot.SelectedIndex;
                        handleControlEvents = previousHandleControlEvents;
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
                    String response = await mainPage.DisplayActionSheet("Are you sure wyou want to delete this Studio Set?",
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
                    byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, data);
                    waitingForResponseFromIntegra7 = 500;
                    commonState.midi.SendSystemExclusive(bytes);
                    // InitializeComponent the studio set:
                    address = new byte[] { 0x0f, 0x00, (byte)(cbStudioSetSlot.SelectedIndex), 0x00 };
                    data = new byte[] { 0x7f, 0x7f, 0x00, 0x00 };
                    bytes = commonState.midi.SystemExclusiveRQ1Message(address, data);
                    waitingForResponseFromIntegra7 = 500;
                    commonState.midi.SendSystemExclusive(bytes);
                    // Save the studio set:
                    address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                    data = new byte[] { 0x55, 0x00, (byte)(cbStudioSetSlot.SelectedIndex + 16), 0x7f };
                    bytes = commonState.midi.SystemExclusiveRQ1Message(address, data);
                    waitingForResponseFromIntegra7 = 500;
                    commonState.midi.SendSystemExclusive(bytes);

                    // Remove the new studio set name from the studio set selector:
                    cbStudioSetSelector.Items[cbStudioSetSlot.SelectedIndex] = "INIT STUDIO";

                    // Remove the new studio set name from commonState.studioSetNames.
                    commonState.studioSetNames[cbStudioSetSlot.SelectedIndex] = "INIT STUDIO";

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
                gEditStudioSetColumn1.IsVisible = true;
            }
            else
            {
                if (MidiChannelIsSameAsPart())
                {
                    gEditStudioSetSearchResult.IsVisible = true;
                    gEditStudioSetColumn1.IsVisible = false;
                    try
                    {
                        StudioSetEditor_SearchResult.Clear();
                    }
                    catch { }
                    String searchString = cbStudioSetPartSettings1Search.Text.ToLower();
                    // Search voices:
                    foreach (List<String> tone in commonState.toneList.Tones)
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
                commonState.currentTone.Name = soundName;
            }
            if (!String.IsNullOrEmpty(cbStudioSetPartSettings1Search.Text))
            {
                if (commonState.currentTone.Name.EndsWith("\t"))
                {
                    drumMap = true;
                    commonState.currentTone.Name = commonState.currentTone.Name.TrimEnd('\t');
                }
                String[] parts = commonState.currentTone.Name.Split(',');
                if (parts.Length == 3)
                {
                    if (drumMap)
                    {
                        commonState.currentTone.Group = parts[1].TrimStart();
                        commonState.currentTone.Category = "Drum";
                        commonState.currentTone.Name = parts[2].TrimStart();
                    }
                    else
                    {
                        commonState.currentTone.Group = parts[1].TrimStart();
                        commonState.currentTone.Category = parts[2].TrimStart();
                        commonState.currentTone.Name = parts[0].TrimStart();
                    }
                    Boolean currentHandleControlEvents = handleControlEvents;
                    handleControlEvents = false;
                    cbStudioSetPartSettings1Group.SelectedItem = commonState.currentTone.Group;
                    PopulateCbStudioSetPartSettings1Category();
                    cbStudioSetPartSettings1Category.SelectedItem = commonState.currentTone.Category;
                    PopulateCbStudioSetPartSettings1Program();
                    cbStudioSetPartSettings1Program.SelectedItem = commonState.currentTone.Name;
                    handleControlEvents = true;
                    commonState.currentTone.Index = commonState.toneList.Get(cbStudioSetPartSettings1Group.SelectedItem.ToString(), cbStudioSetPartSettings1Category.SelectedItem.ToString(), commonState.currentTone.Name);
                }
            }
        }

        private async void btnFileSave_Click(object sender, EventArgs e)
        {
            await StudioSet.Serialize<StudioSet>(commonState.studioSet);
        }

        private async void btnFileLoad_Click(object sender, EventArgs e)
        {
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            commonState.studioSet = await StudioSet.Deserialize<StudioSet>(commonState.studioSet);
            UpdateControlsAndIntegra7FromClasses(cbStudioSetPartSelector.SelectedIndex);
            handleControlEvents = previousHandleControlEvents = handleControlEvents;
            ;
        }

        private void btnStudioSetReturn_Click(object sender, EventArgs e)
        {
            StudioSetEditor_StackLayout.IsVisible = false;
            ShowLibrarianPage();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private Boolean MidiChannelIsSameAsPart()
        {
            if (commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ReceiveChannel == (byte)cbStudioSetPartSelector.SelectedIndex)
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
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            cbStudioSetPartSettings1Group.Items.Clear();
            foreach (List<String> tone in commonState.toneList.Tones)
            {
                if (!cbStudioSetPartSettings1Group.Items.Contains(tone[0]))
                {
                    cbStudioSetPartSettings1Group.Items.Add(tone[0]);
                }
            }
            handleControlEvents = previousHandleControlEvents;
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
            //handleControlEvents = false;
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
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            cbStudioSetPartSettings1Category.Items.Clear();
            foreach (List<String> line in commonState.toneList.Tones.OrderBy(o => o[1]))
            {
                //if (line[0] == group && line[1] != lastCategory && !CategoriesSource.Contains(line[1]))
                if (line[0] == (String)cbStudioSetPartSettings1Group.SelectedItem && line[1] != lastCategory && !cbStudioSetPartSettings1Category.Items.Contains(line[1]))
                {
                    cbStudioSetPartSettings1Category.Items.Add(line[1]);
                    //CategoriesSource.Add(line[1]);
                    lastCategory = line[1];
                }
            }
            handleControlEvents = previousHandleControlEvents;
        }

        private void PopulateCbStudioSetPartSettings1Program()
        {
            t.Trace("private void PopulateCbStudioSetPartSettings1Program ()");
            previousHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
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
                foreach (List<String> tone in commonState.toneList.Tones.OrderBy(o => o[3]))
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
            handleControlEvents = previousHandleControlEvents;
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
            switch (commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectMSB)
            {
                case 85:
                    max = 64;
                    break;
                case 86:
                    switch (commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
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
                    switch (commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
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
                    switch (commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
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
                    switch (commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
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
                    switch (commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
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
                    switch (commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
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
                    switch (commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
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
                    switch (commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
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
                    switch (commonState.studioSet.PartMainSettings[cbStudioSetPartSelector.SelectedIndex].ToneBankSelectLSB)
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
            stringValue = (Double)Value / 20;
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
            if (Value > -1 && Value < 23)
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
            handleControlEvents = false;
            cbChorusChorusFilterCutoffFrequency.SelectedIndex = -1;
            cbChorusChorusFilterType.SelectedIndex = -1;
            cbChorusChorusRateHzNote.SelectedIndex = -1;
            cbChorusDelayCenterMsNote.SelectedIndex = -1;
            cbChorusDelayHFDamp.SelectedIndex = -1;
            cbChorusDelayLeftMsNote.SelectedIndex = -1;
            cbChorusDelayRightMsNote.SelectedIndex = -1;
            cbChorusOutputAssign.SelectedIndex = -1;
            cbStudioSetChorusType.SelectedIndex = -1;
            cbColumn2Selector.SelectedIndex = -1;
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
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(Address, value);
            waitingForResponseFromIntegra7 = 500;
            commonState.midi.SendSystemExclusive(bytes);
        }

        private void UpdateChorusChorusSubwindow()
        {
            t.Trace("private void UpdateChorusChorusSubwindow()");

            switch (commonState.studioSet.CommonChorus.Type)
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
            StudioSetReverb.IsVisible = false;
            StudioSetReverbGM2.IsVisible = false;

            if (commonState.studioSet.CommonReverb.Type == 5)
            {
                StudioSetReverbGM2.IsVisible = true;
            }
            else if (commonState.studioSet.CommonReverb.Type > 0)
            {
                StudioSetReverb.IsVisible = true;
            }
            ReadStudioSetReverb((byte)cbStudioSetReverbType.SelectedIndex);
        }

        private void UpdateControlsAndIntegra7FromClasses(Int32 part)
        {
            if (cbToneControl1.SelectedIndex != commonState.studioSet.Common.ToneControlSource[0])
            {
                cbToneControl1.SelectedIndex = commonState.studioSet.Common.ToneControlSource[0];
                SetStudioSetCommonToneControl1(commonState.studioSet.Common.ToneControlSource[0]);
            }
            if (cbToneControl2.SelectedIndex != commonState.studioSet.Common.ToneControlSource[1])
            {
                cbToneControl2.SelectedIndex = commonState.studioSet.Common.ToneControlSource[1];
                SetStudioSetCommonToneControl2(commonState.studioSet.Common.ToneControlSource[1]);
            }
            if (cbToneControl3.SelectedIndex != commonState.studioSet.Common.ToneControlSource[2])
            {
                cbToneControl3.SelectedIndex = commonState.studioSet.Common.ToneControlSource[2];
                SetStudioSetCommonToneControl3(commonState.studioSet.Common.ToneControlSource[2]);
            }
            if (cbToneControl4.SelectedIndex != commonState.studioSet.Common.ToneControlSource[3])
            {
                cbToneControl4.SelectedIndex = commonState.studioSet.Common.ToneControlSource[3];
                SetStudioSetCommonToneControl4(commonState.studioSet.Common.ToneControlSource[3]);
            }
            if (slTempo.Value != commonState.studioSet.Common.Tempo)
            {
                slTempo.Value = commonState.studioSet.Common.Tempo;
                SetStudioSetCommonTempo(commonState.studioSet.Common.Tempo);
            }
            if (cbSoloPart.SelectedIndex != commonState.studioSet.Common.SoloPart)
            {
                cbSoloPart.SelectedIndex = commonState.studioSet.Common.SoloPart;
                SetStudioSetCommonSoloPart(commonState.studioSet.Common.SoloPart);
            }
            if (cbReverb.IsChecked != commonState.studioSet.Common.Reverb)
            {
                cbReverb.IsChecked = commonState.studioSet.Common.Reverb;
                SetStudioSetCommonReverb((Boolean)commonState.studioSet.Common.Reverb);
            }
            if (cbChorus.IsChecked != commonState.studioSet.Common.Chorus)
            {
                cbChorus.IsChecked = commonState.studioSet.Common.Chorus;
                SetStudioSetCommonChorus((Boolean)commonState.studioSet.Common.Chorus);
            }
            if (cbMasterEQ.IsChecked != commonState.studioSet.Common.MasterEqualizer)
            {
                cbMasterEQ.IsChecked = commonState.studioSet.Common.MasterEqualizer;
                SetStudioSetCommonMasterEQ((Boolean)commonState.studioSet.Common.MasterEqualizer);
            }
            if (cbDrumCompEQPart.SelectedIndex != commonState.studioSet.Common.DrumCompressorAndEqualizerPart)
            {
                cbDrumCompEQPart.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerPart;
                SetStudioSetCommonEQPart(commonState.studioSet.Common.DrumCompressorAndEqualizerPart);
            }
            if (cbDrumCompEQ1OutputAssign.SelectedIndex != commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[0])
            {
                cbDrumCompEQ1OutputAssign.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[0];
                SetStudioSetCommonEQ1OutputAssign(commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[0]);
            }
            if (cbDrumCompEQ2OutputAssign.SelectedIndex != commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[1])
            {
                cbDrumCompEQ2OutputAssign.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[1];
                SetStudioSetCommonEQ2OutputAssign(commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[1]);
            }
            if (cbDrumCompEQ3OutputAssign.SelectedIndex != commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[2])
            {
                cbDrumCompEQ3OutputAssign.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[2];
                SetStudioSetCommonEQ3OutputAssign(commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[2]);
            }
            if (cbDrumCompEQ4OutputAssign.SelectedIndex != commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[3])
            {
                cbDrumCompEQ4OutputAssign.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[3];
                SetStudioSetCommonEQ4OutputAssign(commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[3]);
            }
            if (cbDrumCompEQ5OutputAssign.SelectedIndex != commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[4])
            {
                cbDrumCompEQ5OutputAssign.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[4];
                SetStudioSetCommonEQ5OutputAssign(commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[4]);
            }
            if (cbDrumCompEQ6OutputAssign.SelectedIndex != commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[5])
            {
                cbDrumCompEQ6OutputAssign.SelectedIndex = commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[5];
                SetStudioSetCommonEQ6OutputAssign(commonState.studioSet.Common.DrumCompressorAndEqualizerOutputAssign[5]);
            }
            if (cbDrumCompEQ.IsChecked != commonState.studioSet.Common.DrumCompressorAndEqualizer)
            {
                cbDrumCompEQ.IsChecked = commonState.studioSet.Common.DrumCompressorAndEqualizer;
                SetStudioSetCompEQ_Click((Boolean)commonState.studioSet.Common.DrumCompressorAndEqualizer);
            }
            if (cbExtPartMute.IsChecked != commonState.studioSet.Common.ExternalPartMute)
            {
                cbExtPartMute.IsChecked = commonState.studioSet.Common.ExternalPartMute;
                SetStudioSetExtPartMute((Boolean)commonState.studioSet.Common.ExternalPartMute);
            }
            if (slExtPartLevel.Value != commonState.studioSet.Common.ExternalPartLevel)
            {
                slExtPartLevel.Value = commonState.studioSet.Common.ExternalPartLevel;
                SetStudioSetExtPartLevel(commonState.studioSet.Common.ExternalPartLevel);
            }
            if (slExtPartChorusSend.Value != commonState.studioSet.Common.ExternalPartChorusSendLevel)
            {
                slExtPartChorusSend.Value = commonState.studioSet.Common.ExternalPartChorusSendLevel;
                SetStudioSetExtPartChorusSend(commonState.studioSet.Common.ExternalPartChorusSendLevel);
            }
            if (slExtPartReverbSend.Value != commonState.studioSet.Common.ExternalPartReverbSendLevel)
            {
                slExtPartReverbSend.Value = commonState.studioSet.Common.ExternalPartReverbSendLevel;
                SetStudioSetExtPartReverbSend(commonState.studioSet.Common.ExternalPartReverbSendLevel);
            }
            if (slSystemCommonMasterTune.Value != commonState.studioSet.SystemCommon.MasterTune)
            {
                slSystemCommonMasterTune.Value = commonState.studioSet.SystemCommon.MasterTune;
                SetStudioSetSystemCommonMasterTune((Int32)slSystemCommonMasterTune.Value);
            }
            if (slSystemCommonMasterKeyShift.Value != commonState.studioSet.SystemCommon.MasterKeyShift)
            {
                slSystemCommonMasterKeyShift.Value = commonState.studioSet.SystemCommon.MasterKeyShift;
                SetStudioSetSystemCommonMasterKeyShift((Int32)slSystemCommonMasterKeyShift.Value);
            }
            if (slSystemCommonMasterLevel.Value != commonState.studioSet.SystemCommon.MasterLevel)
            {
                slSystemCommonMasterLevel.Value = commonState.studioSet.SystemCommon.MasterLevel;
                SetStudioSetSystemCommonMasterLevel((Int32)slSystemCommonMasterLevel.Value);
            }
            if (cbSystemCommonScaleTuneSwitch.IsChecked != commonState.studioSet.SystemCommon.ScaleTuneSwitch)
            {
                cbSystemCommonScaleTuneSwitch.IsChecked = commonState.studioSet.SystemCommon.ScaleTuneSwitch;
                SetStudioSetSystemCommonScaleTuneSwitch((Boolean)cbSystemCommonScaleTuneSwitch.IsChecked);
            }
            if (cbSystemCommonStudioSetControlChannel.SelectedIndex != commonState.studioSet.SystemCommon.StudioSetControlChannel)
            {
                cbSystemCommonStudioSetControlChannel.SelectedIndex = commonState.studioSet.SystemCommon.StudioSetControlChannel;
                SetStudioSetSystemCommonStudioSetControlChannel(cbSystemCommonStudioSetControlChannel.SelectedIndex);
            }
            if (cbSystemCommonSystemControlSource1.SelectedIndex != commonState.studioSet.SystemCommon.SystemControl1Source)
            {
                cbSystemCommonSystemControlSource1.SelectedIndex = commonState.studioSet.SystemCommon.SystemControl1Source;
                SetStudioSetSystemCommonSystemControlSource1(cbSystemCommonSystemControlSource1.SelectedIndex);
            }
            if (cbSystemCommonSystemControlSource2.SelectedIndex != commonState.studioSet.SystemCommon.SystemControl2Source)
            {
                cbSystemCommonSystemControlSource2.SelectedIndex = commonState.studioSet.SystemCommon.SystemControl2Source;
                SetStudioSetSystemCommonSystemControlSource2(cbSystemCommonSystemControlSource2.SelectedIndex);
            }
            if (cbSystemCommonSystemControlSource3.SelectedIndex != commonState.studioSet.SystemCommon.SystemControl3Source)
            {
                cbSystemCommonSystemControlSource3.SelectedIndex = commonState.studioSet.SystemCommon.SystemControl3Source;
                SetStudioSetSystemCommonSystemControlSource3(cbSystemCommonSystemControlSource3.SelectedIndex);
            }
            if (cbSystemCommonSystemControlSource4.SelectedIndex != commonState.studioSet.SystemCommon.SystemControl4Source)
            {
                cbSystemCommonSystemControlSource4.SelectedIndex = commonState.studioSet.SystemCommon.SystemControl4Source;
                SetStudioSetSystemCommonSystemControlSource4(cbSystemCommonSystemControlSource4.SelectedIndex);
            }
            if (cbSystemCommonControlSource.SelectedIndex != commonState.studioSet.SystemCommon.ControlSource)
            {
                cbSystemCommonControlSource.SelectedIndex = commonState.studioSet.SystemCommon.ControlSource;
                SetStudioSetSystemCommonControlSource(cbSystemCommonControlSource.SelectedIndex);
            }
            if (cbSystemCommonSystemClockSource.SelectedIndex != commonState.studioSet.SystemCommon.SystemClockSource)
            {
                cbSystemCommonSystemClockSource.SelectedIndex = commonState.studioSet.SystemCommon.SystemClockSource;
                SetStudioSetSystemCommonSystemClockSource(cbSystemCommonSystemClockSource.SelectedIndex);
            }
            if (slSystemCommonSystemTempo.Value != commonState.studioSet.SystemCommon.SystemTempo)
            {
                slSystemCommonSystemTempo.Value = commonState.studioSet.SystemCommon.SystemTempo;
                SetStudioSetSystemCommonSystemTempo((Int32)slSystemCommonSystemTempo.Value);
            }
            if (cbSystemCommonTempoAssignSource.SelectedIndex != commonState.studioSet.SystemCommon.TempoAssignSource)
            {
                cbSystemCommonTempoAssignSource.SelectedIndex = commonState.studioSet.SystemCommon.TempoAssignSource;
                SetStudioSetSystemCommonTempoAssignSource(cbSystemCommonTempoAssignSource.SelectedIndex);
            }
            if (cbSystemCommonReceiveProgramChange.IsChecked != commonState.studioSet.SystemCommon.ReceiveProgramChange)
            {
                cbSystemCommonReceiveProgramChange.IsChecked = commonState.studioSet.SystemCommon.ReceiveProgramChange;
                SetStudioSetSystemCommonReceiveProgramChange((Boolean)cbSystemCommonReceiveProgramChange.IsChecked);
            }
            if (cbSystemCommonReceiveBankSelect.IsChecked != commonState.studioSet.SystemCommon.ReceiveBankSelect)
            {
                cbSystemCommonReceiveBankSelect.IsChecked = commonState.studioSet.SystemCommon.ReceiveBankSelect;
                SetStudioSetSystemCommonReceiveBankSelect((Boolean)cbSystemCommonReceiveBankSelect.IsChecked);
            }
            if (cbSystemCommonSurroundCenterSpeakerSwitch.IsChecked != commonState.studioSet.SystemCommon.SurroundCenterSpeakerSwitch)
            {
                cbSystemCommonSurroundCenterSpeakerSwitch.IsChecked = commonState.studioSet.SystemCommon.SurroundCenterSpeakerSwitch;
                SetStudioSetSystemCommonSurroundCenterSpeakerSwitch((Boolean)cbSystemCommonSurroundCenterSpeakerSwitch.IsChecked);
            }
            if (cbSystemCommonSurroundSubWooferSwitch.IsChecked != commonState.studioSet.SystemCommon.SurroundSubWooferSwitch)
            {
                cbSystemCommonSurroundSubWooferSwitch.IsChecked = commonState.studioSet.SystemCommon.SurroundSubWooferSwitch;
                SetStudioSetSystemCommonSurroundSubWooferSwitch((Boolean)cbSystemCommonSurroundSubWooferSwitch.IsChecked);
            }
            if (cbSystemCommonStereoOutputMode.SelectedIndex != commonState.studioSet.SystemCommon.StereoOutputMode)
            {
                cbSystemCommonStereoOutputMode.SelectedIndex = commonState.studioSet.SystemCommon.StereoOutputMode;
                SetStudioSetSystemCommonStereoOutputMode(cbSystemCommonStereoOutputMode.SelectedIndex);
            }
            if (slVoiceReserve01.Value != commonState.studioSet.Common.VoiceReserve[0])
            {
                slVoiceReserve01.Value = commonState.studioSet.Common.VoiceReserve[0];
                SetStudioSetVoiceReserve01((Int32)slVoiceReserve01.Value);
            }
            if (slVoiceReserve02.Value != commonState.studioSet.Common.VoiceReserve[1])
            {
                slVoiceReserve02.Value = commonState.studioSet.Common.VoiceReserve[1];
                SetStudioSetVoiceReserve02((Int32)slVoiceReserve02.Value);
            }
            if (slVoiceReserve03.Value != commonState.studioSet.Common.VoiceReserve[2])
            {
                slVoiceReserve03.Value = commonState.studioSet.Common.VoiceReserve[2];
                SetStudioSetVoiceReserve03((Int32)slVoiceReserve03.Value);
            }
            if (slVoiceReserve04.Value != commonState.studioSet.Common.VoiceReserve[3])
            {
                slVoiceReserve04.Value = commonState.studioSet.Common.VoiceReserve[3];
                SetStudioSetVoiceReserve04((Int32)slVoiceReserve04.Value);
            }
            if (slVoiceReserve05.Value != commonState.studioSet.Common.VoiceReserve[4])
            {
                slVoiceReserve05.Value = commonState.studioSet.Common.VoiceReserve[4];
                SetStudioSetVoiceReserve05((Int32)slVoiceReserve05.Value);
            }
            if (slVoiceReserve06.Value != commonState.studioSet.Common.VoiceReserve[5])
            {
                slVoiceReserve06.Value = commonState.studioSet.Common.VoiceReserve[5];
                SetStudioSetVoiceReserve06((Int32)slVoiceReserve06.Value);
            }
            if (slVoiceReserve07.Value != commonState.studioSet.Common.VoiceReserve[6])
            {
                slVoiceReserve07.Value = commonState.studioSet.Common.VoiceReserve[6];
                SetStudioSetVoiceReserve07((Int32)slVoiceReserve07.Value);
            }
            if (slVoiceReserve08.Value != commonState.studioSet.Common.VoiceReserve[7])
            {
                slVoiceReserve08.Value = commonState.studioSet.Common.VoiceReserve[7];
                SetStudioSetVoiceReserve08((Int32)slVoiceReserve08.Value);
            }
            if (slVoiceReserve09.Value != commonState.studioSet.Common.VoiceReserve[8])
            {
                slVoiceReserve09.Value = commonState.studioSet.Common.VoiceReserve[8];
                SetStudioSetVoiceReserve09((Int32)slVoiceReserve09.Value);
            }
            if (slVoiceReserve10.Value != commonState.studioSet.Common.VoiceReserve[9])
            {
                slVoiceReserve10.Value = commonState.studioSet.Common.VoiceReserve[9];
                SetStudioSetVoiceReserve10((Int32)slVoiceReserve10.Value);
            }
            if (slVoiceReserve11.Value != commonState.studioSet.Common.VoiceReserve[10])
            {
                slVoiceReserve11.Value = commonState.studioSet.Common.VoiceReserve[10];
                SetStudioSetVoiceReserve11((Int32)slVoiceReserve11.Value);
            }
            if (slVoiceReserve12.Value != commonState.studioSet.Common.VoiceReserve[11])
            {
                slVoiceReserve12.Value = commonState.studioSet.Common.VoiceReserve[11];
                SetStudioSetVoiceReserve12((Int32)slVoiceReserve12.Value);
            }
            if (slVoiceReserve13.Value != commonState.studioSet.Common.VoiceReserve[12])
            {
                slVoiceReserve13.Value = commonState.studioSet.Common.VoiceReserve[12];
                SetStudioSetVoiceReserve13((Int32)slVoiceReserve13.Value);
            }
            if (slVoiceReserve14.Value != commonState.studioSet.Common.VoiceReserve[13])
            {
                slVoiceReserve14.Value = commonState.studioSet.Common.VoiceReserve[13];
                SetStudioSetVoiceReserve14((Int32)slVoiceReserve14.Value);
            }
            if (slVoiceReserve15.Value != commonState.studioSet.Common.VoiceReserve[14])
            {
                slVoiceReserve15.Value = commonState.studioSet.Common.VoiceReserve[14];
                SetStudioSetVoiceReserve15((Int32)slVoiceReserve15.Value);
            }
            if (slVoiceReserve16.Value != commonState.studioSet.Common.VoiceReserve[15])
            {
                slVoiceReserve16.Value = commonState.studioSet.Common.VoiceReserve[15];
                SetStudioSetVoiceReserve16((Int32)slVoiceReserve16.Value);
            }
            //if (cbStudioSetChorusType.SelectedIndex != commonState.studioSet.CommonChorus.Type)
            {
                cbStudioSetChorusType.SelectedIndex = commonState.studioSet.CommonChorus.Type;
                SetStudioSetStudioSetChorusType(cbStudioSetChorusType.SelectedIndex);
            }
            if (slChorusLevel.Value != commonState.studioSet.CommonChorus.Level)
            {
                slChorusLevel.Value = commonState.studioSet.CommonChorus.Level;
                SetStudioSetChorusLevel((Int32)slChorusLevel.Value);
            }
            if (cbChorusOutputAssign.SelectedIndex != commonState.studioSet.CommonChorus.OutputAssign)
            {
                cbChorusOutputAssign.SelectedIndex = commonState.studioSet.CommonChorus.OutputAssign;
                SetStudioSetChorusOutputAssign(cbChorusOutputAssign.SelectedIndex);
            }
            if (cbChorusChorusFilterType.SelectedIndex != commonState.studioSet.CommonChorus.Chorus.FilterType)
            {
                cbChorusChorusFilterType.SelectedIndex = commonState.studioSet.CommonChorus.Chorus.FilterType;
                SetStudioSetChorusChorusFilterType(cbChorusChorusFilterType.SelectedIndex);
            }
            if (cbChorusChorusFilterCutoffFrequency.SelectedIndex != commonState.studioSet.CommonChorus.Chorus.FilterCutoffFrequency)
            {
                cbChorusChorusFilterCutoffFrequency.SelectedIndex = commonState.studioSet.CommonChorus.Chorus.FilterCutoffFrequency;
                SetStudioSetChorusChorusFilterCutoffFrequency(cbChorusChorusFilterCutoffFrequency.SelectedIndex);
            }
            if (slChorusChorusPreDelay.Value != commonState.studioSet.CommonChorus.Chorus.PreDelay)
            {
                slChorusChorusPreDelay.Value = commonState.studioSet.CommonChorus.Chorus.PreDelay;
                SetStudioSetChorusChorusPreDelay((Int32)slChorusChorusPreDelay.Value);
            }
            if (cbChorusChorusRateHzNote.SelectedIndex != commonState.studioSet.CommonChorus.Chorus.RateHzNote)
            {
                try
                {
                    cbChorusChorusRateHzNote.SelectedIndex = commonState.studioSet.CommonChorus.Chorus.RateHzNote;
                    SetStudioSetChorusChorusRate(cbChorusChorusRateHzNote.SelectedIndex);
                }
                catch (Exception e)
                {
                    t.Trace(e.Message + " setting cbChorusChorusRateHzNote");
                }
            }
            if (slChorusChorusRateHz.Value != commonState.studioSet.CommonChorus.Chorus.RateHz)
            {
                slChorusChorusRateHz.Value = commonState.studioSet.CommonChorus.Chorus.RateHz;
                SetStudioSetChorusChorusRateHz((Int32)slChorusChorusRateHz.Value);
            }
            if (slChorusChorusRateNote.Value != commonState.studioSet.CommonChorus.Chorus.RateNote)
            {
                slChorusChorusRateNote.Value = commonState.studioSet.CommonChorus.Chorus.RateNote;
                SetStudioSetChorusChorusRateNote((Int32)slChorusChorusRateNote.Value);
            }
            if (slChorusChorusDepth.Value != commonState.studioSet.CommonChorus.Chorus.Depth)
            {
                slChorusChorusDepth.Value = commonState.studioSet.CommonChorus.Chorus.Depth;
                SetStudioSetChorusChorusDepth((Int32)slChorusChorusDepth.Value);
            }
            if (slChorusChorusPhase.Value != commonState.studioSet.CommonChorus.Chorus.Phase)
            {
                slChorusChorusPhase.Value = commonState.studioSet.CommonChorus.Chorus.Phase;
                SetStudioSetChorusChorusPhase((Int32)slChorusChorusPhase.Value);
            }
            if (slChorusChorusFeedback.Value != commonState.studioSet.CommonChorus.Chorus.Feedback)
            {
                slChorusChorusFeedback.Value = commonState.studioSet.CommonChorus.Chorus.Feedback;
                SetStudioSetChorusChorusFeedback((Int32)slChorusChorusFeedback.Value);
            }
            if (cbChorusDelayLeftMsNote.SelectedIndex != commonState.studioSet.CommonChorus.Delay.LeftMsNote)
            {
                try
                {
                    cbChorusDelayLeftMsNote.SelectedIndex = commonState.studioSet.CommonChorus.Delay.LeftMsNote;
                    SetStudioSetChorusDelayLeft(cbChorusDelayLeftMsNote.SelectedIndex);
                }
                catch (Exception e)
                {
                    t.Trace(e.Message + " setting cbChorusDelayLeftMsNote");
                }
            }
            if (slChorusDelayLeftHz.Value != commonState.studioSet.CommonChorus.Delay.LeftMs)
            {
                slChorusDelayLeftHz.Value = commonState.studioSet.CommonChorus.Delay.LeftMs;
                SetStudioSetChorusDelayLeftHz((Int32)slChorusDelayLeftHz.Value);
            }
            if (slChorusDelayLeftNote.Value != commonState.studioSet.CommonChorus.Delay.LeftNote)
            {
                slChorusDelayLeftNote.Value = commonState.studioSet.CommonChorus.Delay.LeftNote;
                SetStudioSetChorusDelayLeftNote((Int32)slChorusDelayLeftNote.Value);
            }
            if (cbChorusDelayRightMsNote.SelectedIndex != commonState.studioSet.CommonChorus.Delay.RightMsNote)
            {
                try
                {
                    cbChorusDelayRightMsNote.SelectedIndex = commonState.studioSet.CommonChorus.Delay.RightMsNote;
                    SetStudioSetChorusDelayRight(cbChorusDelayRightMsNote.SelectedIndex);
                }
                catch (Exception e)
                {
                    t.Trace(e.Message + " setting cbChorusDelayRightMsNote");
                }
            }
            if (slChorusDelayRightHz.Value != commonState.studioSet.CommonChorus.Delay.RightMs)
            {
                slChorusDelayRightHz.Value = commonState.studioSet.CommonChorus.Delay.RightMs;
                SetStudioSetChorusDelayRightHz((Int32)slChorusDelayRightHz.Value);
            }
            if (slChorusDelayRightNote.Value != commonState.studioSet.CommonChorus.Delay.RightNote)
            {
                slChorusDelayRightNote.Value = commonState.studioSet.CommonChorus.Delay.RightNote;
                SetStudioSetChorusDelayRightNote((Int32)slChorusDelayRightNote.Value);
            }
            if (cbChorusDelayCenterMsNote.SelectedIndex != commonState.studioSet.CommonChorus.Delay.CenterMsNote)
            {
                try
                {
                    cbChorusDelayCenterMsNote.SelectedIndex = commonState.studioSet.CommonChorus.Delay.CenterMsNote;
                    SetStudioSetChorusDelayCenter(cbChorusDelayCenterMsNote.SelectedIndex);
                }
                catch (Exception e)
                {
                    t.Trace(e.Message + " setting cbChorusDelayCenterMsNote");
                }
            }
            if (slChorusDelayCenterHz.Value != commonState.studioSet.CommonChorus.Delay.CenterMs)
            {
                slChorusDelayCenterHz.Value = commonState.studioSet.CommonChorus.Delay.CenterMs;
                SetStudioSetChorusDelayCenterHz((Int32)slChorusDelayCenterHz.Value);
            }
            if (slChorusDelayCenterNote.Value != commonState.studioSet.CommonChorus.Delay.CenterNote)
            {
                slChorusDelayCenterNote.Value = commonState.studioSet.CommonChorus.Delay.CenterNote;
                SetStudioSetChorusDelayCenterNote((Int32)slChorusDelayCenterNote.Value);
            }
            if (slChorusDelayCenterFeedback.Value != commonState.studioSet.CommonChorus.Delay.CenterFeedback)
            {
                slChorusDelayCenterFeedback.Value = commonState.studioSet.CommonChorus.Delay.CenterFeedback;
                SetStudioSetChorusDelayCenterFeedback((Int32)slChorusDelayCenterFeedback.Value);
            }
            if (cbChorusDelayHFDamp.SelectedIndex != commonState.studioSet.CommonChorus.Delay.HFDamp)
            {
                cbChorusDelayHFDamp.SelectedIndex = commonState.studioSet.CommonChorus.Delay.HFDamp;
                SetStudioSetChorusDelayHFDamp(cbChorusDelayHFDamp.SelectedIndex);
            }
            if (slChorusDelayLeftLevel.Value != commonState.studioSet.CommonChorus.Delay.LeftLevel)
            {
                slChorusDelayLeftLevel.Value = commonState.studioSet.CommonChorus.Delay.LeftLevel;
                SetStudioSetChorusDelayLeftLevel((Int32)slChorusDelayLeftLevel.Value);
            }
            if (slChorusDelayRightLevel.Value != commonState.studioSet.CommonChorus.Delay.RightLevel)
            {
                slChorusDelayRightLevel.Value = commonState.studioSet.CommonChorus.Delay.RightLevel;
                SetStudioSetChorusDelayRightevel((Int32)slChorusDelayRightLevel.Value);
            }
            if (slChorusDelayCenterLevel.Value != commonState.studioSet.CommonChorus.Delay.CenterLevel)
            {
                slChorusDelayCenterLevel.Value = commonState.studioSet.CommonChorus.Delay.CenterLevel;
                SetStudioSetChorusDelayCenterLevel((Int32)slChorusDelayCenterLevel.Value);
            }
            if (slChorusGM2ChorusPreLPF.Value != commonState.studioSet.CommonChorus.Gm2Chorus.PreLPF)
            {
                slChorusGM2ChorusPreLPF.Value = commonState.studioSet.CommonChorus.Gm2Chorus.PreLPF;
                SetStudioSetChorusGM2ChorusPreLPF((Int32)slChorusGM2ChorusPreLPF.Value);
            }
            if (slChorusGM2ChorusLevel.Value != commonState.studioSet.CommonChorus.Gm2Chorus.Level)
            {
                slChorusGM2ChorusLevel.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Level;
                SetStudioSetChorusGM2ChorusLevel((Int32)slChorusGM2ChorusLevel.Value);
            }
            if (slChorusGM2ChorusFeedback.Value != commonState.studioSet.CommonChorus.Gm2Chorus.Feedback)
            {
                slChorusGM2ChorusFeedback.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Feedback;
                SetStudioSetChorusGM2ChorusFeedback((Int32)slChorusGM2ChorusFeedback.Value);
            }
            if (slChorusGM2ChorusDelay.Value != commonState.studioSet.CommonChorus.Gm2Chorus.Delay)
            {
                slChorusGM2ChorusDelay.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Delay;
                SetStudioSetChorusGM2ChorusDelay((Int32)slChorusGM2ChorusDelay.Value);
            }
            if (slChorusGM2ChorusRate.Value != commonState.studioSet.CommonChorus.Gm2Chorus.Rate)
            {
                slChorusGM2ChorusRate.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Rate;
                SetStudioSetChorusGM2ChorusRate((Int32)slChorusGM2ChorusRate.Value);
            }
            if (slChorusGM2ChorusDepth.Value != commonState.studioSet.CommonChorus.Gm2Chorus.Depth)
            {
                slChorusGM2ChorusDepth.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Depth;
                SetStudioSetChorusGM2ChorusDepth((Int32)slChorusGM2ChorusDepth.Value);
            }
            if (slChorusGM2ChorusLevel.Value != commonState.studioSet.CommonChorus.Gm2Chorus.Level)
            {
                slChorusGM2ChorusLevel.Value = commonState.studioSet.CommonChorus.Gm2Chorus.Level;
                SetStudioSetChorusGM2ChorusSendLevelToReverb((Int32)slChorusGM2ChorusLevel.Value);
            }
            //if (cbStudioSetReverbType.SelectedIndex != commonState.studioSet.CommonReverb.Type)
            {
                cbStudioSetReverbType.SelectedIndex = commonState.studioSet.CommonReverb.Type;
                SetStudioSetStudioSetReverbType((Int32)cbStudioSetReverbType.SelectedIndex);
            }
            if (slStudioSetReverbLevel.Value != commonState.studioSet.CommonReverb.Level)
            {
                slStudioSetReverbLevel.Value = commonState.studioSet.CommonReverb.Level;
                SetStudioSetReverbLevel((Int32)slStudioSetReverbLevel.Value);
            }
            if (cbStudioSetReverbOutputAssign.SelectedIndex != commonState.studioSet.CommonReverb.OutputAssign)
            {
                cbStudioSetReverbOutputAssign.SelectedIndex = commonState.studioSet.CommonReverb.OutputAssign;
                SetStudioSetStudioSetReverbOutputAssign(cbStudioSetReverbOutputAssign.SelectedIndex);
            }
            switch (commonState.studioSet.CommonReverb.Type)
            {
                case 1:
                    if (slStudioSetReverbPreDelay.Value != commonState.studioSet.CommonReverb.ReverbRoom1.PreDelay)
                    {
                        slStudioSetReverbPreDelay.Value = commonState.studioSet.CommonReverb.ReverbRoom1.PreDelay;
                        SetStudioSetStudioSetReverbPreDelay((Int32)slStudioSetReverbPreDelay.Value);
                    }
                    if (slStudioSetReverbTime.Value != commonState.studioSet.CommonReverb.ReverbRoom1.Time)
                    {
                        slStudioSetReverbTime.Value = commonState.studioSet.CommonReverb.ReverbRoom1.Time;
                        SetStudioSetStudioSetReverbTime((Int32)slStudioSetReverbTime.Value);
                    }
                    if (slStudioSetReverbDensity.Value != commonState.studioSet.CommonReverb.ReverbRoom1.Density)
                    {
                        slStudioSetReverbDensity.Value = commonState.studioSet.CommonReverb.ReverbRoom1.Density;
                        SetStudioSetStudioSetReverbDensity((Int32)slStudioSetReverbDensity.Value);
                    }
                    if (slStudioSetReverbDiffusion.Value != commonState.studioSet.CommonReverb.ReverbRoom1.Diffusion)
                    {
                        slStudioSetReverbDiffusion.Value = commonState.studioSet.CommonReverb.ReverbRoom1.Diffusion;
                        SetStudioSetStudioSetReverbDiffusion((Int32)slStudioSetReverbDiffusion.Value);
                    }
                    if (slStudioSetReverbLFDamp.Value != commonState.studioSet.CommonReverb.ReverbRoom1.LFDamp)
                    {
                        slStudioSetReverbLFDamp.Value = commonState.studioSet.CommonReverb.ReverbRoom1.LFDamp;
                        SetStudioSetStudioSetReverbLFDamp((Int32)slStudioSetReverbLFDamp.Value);
                    }
                    if (slStudioSetReverbHFDamp.Value != commonState.studioSet.CommonReverb.ReverbRoom1.HFDamp)
                    {
                        slStudioSetReverbHFDamp.Value = commonState.studioSet.CommonReverb.ReverbRoom1.HFDamp;
                        SetStudioSetStudioSetReverbHFDamp((Int32)slStudioSetReverbHFDamp.Value);
                    }
                    if (slStudioSetReverbSpread.Value != commonState.studioSet.CommonReverb.ReverbRoom1.Spread)
                    {
                        slStudioSetReverbSpread.Value = commonState.studioSet.CommonReverb.ReverbRoom1.Spread;
                        SetStudioSetStudioSetReverbSpread((Int32)slStudioSetReverbSpread.Value);
                    }
                    if (slStudioSetReverbTone.Value != commonState.studioSet.CommonReverb.ReverbRoom1.Tone)
                    {
                        slStudioSetReverbTone.Value = commonState.studioSet.CommonReverb.ReverbRoom1.Tone;
                        SetStudioSetStudioSetReverbTone((Int32)slStudioSetReverbTone.Value);
                    }
                    break;
                case 2:
                    if (slStudioSetReverbPreDelay.Value != commonState.studioSet.CommonReverb.ReverbRoom2.PreDelay)
                    {
                        slStudioSetReverbPreDelay.Value = commonState.studioSet.CommonReverb.ReverbRoom2.PreDelay;
                        SetStudioSetStudioSetReverbPreDelay((Int32)slStudioSetReverbPreDelay.Value);
                    }
                    if (slStudioSetReverbTime.Value != commonState.studioSet.CommonReverb.ReverbRoom2.Time)
                    {
                        slStudioSetReverbTime.Value = commonState.studioSet.CommonReverb.ReverbRoom2.Time;
                        SetStudioSetStudioSetReverbTime((Int32)slStudioSetReverbTime.Value);
                    }
                    if (slStudioSetReverbDensity.Value != commonState.studioSet.CommonReverb.ReverbRoom2.Density)
                    {
                        slStudioSetReverbDensity.Value = commonState.studioSet.CommonReverb.ReverbRoom2.Density;
                        SetStudioSetStudioSetReverbDensity((Int32)slStudioSetReverbDensity.Value);
                    }
                    if (slStudioSetReverbDiffusion.Value != commonState.studioSet.CommonReverb.ReverbRoom2.Diffusion)
                    {
                        slStudioSetReverbDiffusion.Value = commonState.studioSet.CommonReverb.ReverbRoom2.Diffusion;
                        SetStudioSetStudioSetReverbDiffusion((Int32)slStudioSetReverbDiffusion.Value);
                    }
                    if (slStudioSetReverbLFDamp.Value != commonState.studioSet.CommonReverb.ReverbRoom2.LFDamp)
                    {
                        slStudioSetReverbLFDamp.Value = commonState.studioSet.CommonReverb.ReverbRoom2.LFDamp;
                        SetStudioSetStudioSetReverbLFDamp((Int32)slStudioSetReverbLFDamp.Value);
                    }
                    if (slStudioSetReverbHFDamp.Value != commonState.studioSet.CommonReverb.ReverbRoom2.HFDamp)
                    {
                        slStudioSetReverbHFDamp.Value = commonState.studioSet.CommonReverb.ReverbRoom2.HFDamp;
                        SetStudioSetStudioSetReverbHFDamp((Int32)slStudioSetReverbHFDamp.Value);
                    }
                    if (slStudioSetReverbSpread.Value != commonState.studioSet.CommonReverb.ReverbRoom2.Spread)
                    {
                        slStudioSetReverbSpread.Value = commonState.studioSet.CommonReverb.ReverbRoom2.Spread;
                        SetStudioSetStudioSetReverbSpread((Int32)slStudioSetReverbSpread.Value);
                    }
                    if (slStudioSetReverbTone.Value != commonState.studioSet.CommonReverb.ReverbRoom2.Tone)
                    {
                        slStudioSetReverbTone.Value = commonState.studioSet.CommonReverb.ReverbRoom2.Tone;
                        SetStudioSetStudioSetReverbTone((Int32)slStudioSetReverbTone.Value);
                    }
                    break;
                case 3:
                    if (slStudioSetReverbPreDelay.Value != commonState.studioSet.CommonReverb.ReverbHall1.PreDelay)
                    {
                        slStudioSetReverbPreDelay.Value = commonState.studioSet.CommonReverb.ReverbHall1.PreDelay;
                        SetStudioSetStudioSetReverbPreDelay((Int32)slStudioSetReverbPreDelay.Value);
                    }
                    if (slStudioSetReverbTime.Value != commonState.studioSet.CommonReverb.ReverbHall1.Time)
                    {
                        slStudioSetReverbTime.Value = commonState.studioSet.CommonReverb.ReverbHall1.Time;
                        SetStudioSetStudioSetReverbTime((Int32)slStudioSetReverbTime.Value);
                    }
                    if (slStudioSetReverbDensity.Value != commonState.studioSet.CommonReverb.ReverbHall1.Density)
                    {
                        slStudioSetReverbDensity.Value = commonState.studioSet.CommonReverb.ReverbHall1.Density;
                        SetStudioSetStudioSetReverbDensity((Int32)slStudioSetReverbDensity.Value);
                    }
                    if (slStudioSetReverbDiffusion.Value != commonState.studioSet.CommonReverb.ReverbHall1.Diffusion)
                    {
                        slStudioSetReverbDiffusion.Value = commonState.studioSet.CommonReverb.ReverbHall1.Diffusion;
                        SetStudioSetStudioSetReverbDiffusion((Int32)slStudioSetReverbDiffusion.Value);
                    }
                    if (slStudioSetReverbLFDamp.Value != commonState.studioSet.CommonReverb.ReverbHall1.LFDamp)
                    {
                        slStudioSetReverbLFDamp.Value = commonState.studioSet.CommonReverb.ReverbHall1.LFDamp;
                        SetStudioSetStudioSetReverbLFDamp((Int32)slStudioSetReverbLFDamp.Value);
                    }
                    if (slStudioSetReverbHFDamp.Value != commonState.studioSet.CommonReverb.ReverbHall1.HFDamp)
                    {
                        slStudioSetReverbHFDamp.Value = commonState.studioSet.CommonReverb.ReverbHall1.HFDamp;
                        SetStudioSetStudioSetReverbHFDamp((Int32)slStudioSetReverbHFDamp.Value);
                    }
                    if (slStudioSetReverbSpread.Value != commonState.studioSet.CommonReverb.ReverbHall1.Spread)
                    {
                        slStudioSetReverbSpread.Value = commonState.studioSet.CommonReverb.ReverbHall1.Spread;
                        SetStudioSetStudioSetReverbSpread((Int32)slStudioSetReverbSpread.Value);
                    }
                    if (slStudioSetReverbTone.Value != commonState.studioSet.CommonReverb.ReverbHall1.Tone)
                    {
                        slStudioSetReverbTone.Value = commonState.studioSet.CommonReverb.ReverbHall1.Tone;
                        SetStudioSetStudioSetReverbTone((Int32)slStudioSetReverbTone.Value);
                    }
                    break;
                case 4:
                    if (slStudioSetReverbPreDelay.Value != commonState.studioSet.CommonReverb.ReverbHall2.PreDelay)
                    {
                        slStudioSetReverbPreDelay.Value = commonState.studioSet.CommonReverb.ReverbHall2.PreDelay;
                        SetStudioSetStudioSetReverbPreDelay((Int32)slStudioSetReverbPreDelay.Value);
                    }
                    if (slStudioSetReverbTime.Value != commonState.studioSet.CommonReverb.ReverbHall2.Time)
                    {
                        slStudioSetReverbTime.Value = commonState.studioSet.CommonReverb.ReverbHall2.Time;
                        SetStudioSetStudioSetReverbTime((Int32)slStudioSetReverbTime.Value);
                    }
                    if (slStudioSetReverbDensity.Value != commonState.studioSet.CommonReverb.ReverbHall2.Density)
                    {
                        slStudioSetReverbDensity.Value = commonState.studioSet.CommonReverb.ReverbHall2.Density;
                        SetStudioSetStudioSetReverbDensity((Int32)slStudioSetReverbDensity.Value);
                    }
                    if (slStudioSetReverbDiffusion.Value != commonState.studioSet.CommonReverb.ReverbHall2.Diffusion)
                    {
                        slStudioSetReverbDiffusion.Value = commonState.studioSet.CommonReverb.ReverbHall2.Diffusion;
                        SetStudioSetStudioSetReverbDiffusion((Int32)slStudioSetReverbDiffusion.Value);
                    }
                    if (slStudioSetReverbLFDamp.Value != commonState.studioSet.CommonReverb.ReverbHall2.LFDamp)
                    {
                        slStudioSetReverbLFDamp.Value = commonState.studioSet.CommonReverb.ReverbHall2.LFDamp;
                        SetStudioSetStudioSetReverbLFDamp((Int32)slStudioSetReverbLFDamp.Value);
                    }
                    if (slStudioSetReverbHFDamp.Value != commonState.studioSet.CommonReverb.ReverbHall2.HFDamp)
                    {
                        slStudioSetReverbHFDamp.Value = commonState.studioSet.CommonReverb.ReverbHall2.HFDamp;
                        SetStudioSetStudioSetReverbHFDamp((Int32)slStudioSetReverbHFDamp.Value);
                    }
                    if (slStudioSetReverbSpread.Value != commonState.studioSet.CommonReverb.ReverbHall2.Spread)
                    {
                        slStudioSetReverbSpread.Value = commonState.studioSet.CommonReverb.ReverbHall2.Spread;
                        SetStudioSetStudioSetReverbSpread((Int32)slStudioSetReverbSpread.Value);
                    }
                    if (slStudioSetReverbTone.Value != commonState.studioSet.CommonReverb.ReverbHall2.Tone)
                    {
                        slStudioSetReverbTone.Value = commonState.studioSet.CommonReverb.ReverbHall2.Tone;
                        SetStudioSetStudioSetReverbTone((Int32)slStudioSetReverbTone.Value);
                    }
                    break;
                case 5:
                    if (slStudioSetReverbPreDelay.Value != commonState.studioSet.CommonReverb.ReverbPlate.PreDelay)
                    {
                        slStudioSetReverbPreDelay.Value = commonState.studioSet.CommonReverb.ReverbPlate.PreDelay;
                        SetStudioSetStudioSetReverbPreDelay((Int32)slStudioSetReverbPreDelay.Value);
                    }
                    if (slStudioSetReverbTime.Value != commonState.studioSet.CommonReverb.ReverbPlate.Time)
                    {
                        slStudioSetReverbTime.Value = commonState.studioSet.CommonReverb.ReverbPlate.Time;
                        SetStudioSetStudioSetReverbTime((Int32)slStudioSetReverbTime.Value);
                    }
                    if (slStudioSetReverbDensity.Value != commonState.studioSet.CommonReverb.ReverbPlate.Density)
                    {
                        slStudioSetReverbDensity.Value = commonState.studioSet.CommonReverb.ReverbPlate.Density;
                        SetStudioSetStudioSetReverbDensity((Int32)slStudioSetReverbDensity.Value);
                    }
                    if (slStudioSetReverbDiffusion.Value != commonState.studioSet.CommonReverb.ReverbPlate.Diffusion)
                    {
                        slStudioSetReverbDiffusion.Value = commonState.studioSet.CommonReverb.ReverbPlate.Diffusion;
                        SetStudioSetStudioSetReverbDiffusion((Int32)slStudioSetReverbDiffusion.Value);
                    }
                    if (slStudioSetReverbLFDamp.Value != commonState.studioSet.CommonReverb.ReverbPlate.LFDamp)
                    {
                        slStudioSetReverbLFDamp.Value = commonState.studioSet.CommonReverb.ReverbPlate.LFDamp;
                        SetStudioSetStudioSetReverbLFDamp((Int32)slStudioSetReverbLFDamp.Value);
                    }
                    if (slStudioSetReverbHFDamp.Value != commonState.studioSet.CommonReverb.ReverbPlate.HFDamp)
                    {
                        slStudioSetReverbHFDamp.Value = commonState.studioSet.CommonReverb.ReverbPlate.HFDamp;
                        SetStudioSetStudioSetReverbHFDamp((Int32)slStudioSetReverbHFDamp.Value);
                    }
                    if (slStudioSetReverbSpread.Value != commonState.studioSet.CommonReverb.ReverbPlate.Spread)
                    {
                        slStudioSetReverbSpread.Value = commonState.studioSet.CommonReverb.ReverbPlate.Spread;
                        SetStudioSetStudioSetReverbSpread((Int32)slStudioSetReverbSpread.Value);
                    }
                    if (slStudioSetReverbTone.Value != commonState.studioSet.CommonReverb.ReverbPlate.Tone)
                    {
                        slStudioSetReverbTone.Value = commonState.studioSet.CommonReverb.ReverbPlate.Tone;
                        SetStudioSetStudioSetReverbTone((Int32)slStudioSetReverbTone.Value);
                    }
                    break;
            }
            if (slStudioSetReverbGM2Character.Value != commonState.studioSet.CommonReverb.GM2Reverb.Character)
            {
                slStudioSetReverbGM2Character.Value = commonState.studioSet.CommonReverb.GM2Reverb.Character;
                SetStudioSetStudioSetReverbGM2Character((Int32)slStudioSetReverbGM2Character.Value);
            }
            if (slStudioSetReverbGM2Time.Value != commonState.studioSet.CommonReverb.GM2Reverb.Time)
            {
                slStudioSetReverbGM2Time.Value = commonState.studioSet.CommonReverb.GM2Reverb.Time;
                SetStudioSetStudioSetReverbGM2Time((Int32)slStudioSetReverbGM2Time.Value);
            }
            if (cbStudioSetMotionalSurround.IsChecked != commonState.studioSet.MotionalSurround.MotionalSurroundSwitch)
            {
                cbStudioSetMotionalSurround.IsChecked = commonState.studioSet.MotionalSurround.MotionalSurroundSwitch;
                SetStudioSetStudioSetMotionalSurround((Boolean)cbStudioSetMotionalSurround.IsChecked);
            }
            if (cbStudioSetMotionalSurroundRoomType.SelectedIndex != commonState.studioSet.MotionalSurround.RoomType)
            {
                cbStudioSetMotionalSurroundRoomType.SelectedIndex = commonState.studioSet.MotionalSurround.RoomType;
                SetStudioSetStudioSetMotionalSurroundRoomType((Int32)cbStudioSetMotionalSurroundRoomType.SelectedIndex);
            }
            if (slStudioSetMotionalSurroundAmbienceLevel.Value != commonState.studioSet.MotionalSurround.AmbienceLevel)
            {
                slStudioSetMotionalSurroundAmbienceLevel.Value = commonState.studioSet.MotionalSurround.AmbienceLevel;
                SetStudioSetStudioSetMotionalSurroundAmbienceLevel((Int32)slStudioSetMotionalSurroundAmbienceLevel.Value);
            }
            if (cbStudioSetMotionalSurroundRoomSize.SelectedIndex != commonState.studioSet.MotionalSurround.RoomSize)
            {
                cbStudioSetMotionalSurroundRoomSize.SelectedIndex = commonState.studioSet.MotionalSurround.RoomSize;
                SetStudioSetStudioSetMotionalSurroundRoomSize(cbStudioSetMotionalSurroundRoomSize.SelectedIndex);
            }
            if (slStudioSetMotionalSurroundAmbienceTime.Value != commonState.studioSet.MotionalSurround.AmbienceTime)
            {
                slStudioSetMotionalSurroundAmbienceTime.Value = commonState.studioSet.MotionalSurround.AmbienceTime;
                SetStudioSetStudioSetMotionalSurroundAmbienceTime((Int32)slStudioSetMotionalSurroundAmbienceTime.Value);
            }
            if (slStudioSetMotionalSurroundAmbienceDensity.Value != commonState.studioSet.MotionalSurround.AmbienceDensity)
            {
                slStudioSetMotionalSurroundAmbienceDensity.Value = commonState.studioSet.MotionalSurround.AmbienceDensity;
                SetStudioSetStudioSetMotionalSurroundAmbienceDensity((Int32)slStudioSetMotionalSurroundAmbienceDensity.Value);
            }
            if (slStudioSetMotionalSurroundAmbienceHFDamp.Value != commonState.studioSet.MotionalSurround.AmbienceHFDamp)
            {
                slStudioSetMotionalSurroundAmbienceHFDamp.Value = commonState.studioSet.MotionalSurround.AmbienceHFDamp;
                SetStudioSetStudioSetMotionalSurroundAmbienceHFDamp((Int32)slStudioSetMotionalSurroundAmbienceHFDamp.Value);
            }
            if (slStudioSetMotionalSurroundExternalPartLR.Value != commonState.studioSet.MotionalSurround.ExtPartLR)
            {
                slStudioSetMotionalSurroundExternalPartLR.Value = commonState.studioSet.MotionalSurround.ExtPartLR;
                SetStudioSetStudioSetMotionalSurroundExternalPartLR((Int32)slStudioSetMotionalSurroundExternalPartLR.Value);
            }
            if (slStudioSetMotionalSurroundExternalPartFB.Value != commonState.studioSet.MotionalSurround.ExtPartFB)
            {
                slStudioSetMotionalSurroundExternalPartFB.Value = commonState.studioSet.MotionalSurround.ExtPartFB;
                SetStudioSetStudioSetMotionalSurroundExternalPartFB((Int32)slStudioSetMotionalSurroundExternalPartFB.Value);
            }
            if (slStudioSetMotionalSurroundExtPartWidth.Value != commonState.studioSet.MotionalSurround.ExtPartWidth)
            {
                slStudioSetMotionalSurroundExtPartWidth.Value = commonState.studioSet.MotionalSurround.ExtPartWidth;
                SetStudioSetStudioSetMotionalSurroundExtPartWidth((Int32)slStudioSetMotionalSurroundExtPartWidth.Value);
            }
            if (slStudioSetMotionalSurroundExtpartAmbienceSend.Value != commonState.studioSet.MotionalSurround.ExtPartAmbienceSendLevel)
            {
                slStudioSetMotionalSurroundExtpartAmbienceSend.Value = commonState.studioSet.MotionalSurround.ExtPartAmbienceSendLevel;
                SetStudioSetStudioSetMotionalSurroundExtpartAmbienceSend((Int32)slStudioSetMotionalSurroundExtpartAmbienceSend.Value);
            }
            if (cbStudioSetMotionalSurroundExtPartControl.SelectedIndex != commonState.studioSet.MotionalSurround.ExtPartControlChannel)
            {
                cbStudioSetMotionalSurroundExtPartControl.SelectedIndex = commonState.studioSet.MotionalSurround.ExtPartControlChannel;
                SetStudioSetStudioSetMotionalSurroundExtPartControl(cbStudioSetMotionalSurroundExtPartControl.SelectedIndex);
            }
            if (slStudioSetMotionalSurroundDepth.Value != commonState.studioSet.MotionalSurround.MotionalSurroundDepth)
            {
                slStudioSetMotionalSurroundDepth.Value = commonState.studioSet.MotionalSurround.MotionalSurroundDepth;
                SetStudioSetStudioSetMotionalSurroundDepth((Int32)slStudioSetMotionalSurroundDepth.Value);
            }
            if (commonState.studioSet.MasterEQ.EQLowFreq != cbStudioSetMasterEqLowFreq.SelectedIndex)
            {
                SetStudioSetStudioSetMasterEqLowFreq(cbStudioSetMasterEqLowFreq.SelectedIndex);
            }
            if (slStudioSetMasterEqLowGain.Value != commonState.studioSet.MasterEQ.EQLowGain)
            {
                slStudioSetMasterEqLowGain.Value = commonState.studioSet.MasterEQ.EQLowGain;
                SetStudioSetStudioSetMasterEqLowGain((Int32)slStudioSetMasterEqLowGain.Value);
            }
            if (cbStudioSetMasterEqMidFreq.SelectedIndex != commonState.studioSet.MasterEQ.EQMidFreq)
            {
                cbStudioSetMasterEqMidFreq.SelectedIndex = commonState.studioSet.MasterEQ.EQMidFreq;
                SetStudioSetStudioSetMasterEqMidFreq(cbStudioSetMasterEqMidFreq.SelectedIndex);
            }
            if (slStudioSetMasterEqMidGain.Value != commonState.studioSet.MasterEQ.EQMidGain)
            {
                slStudioSetMasterEqMidGain.Value = commonState.studioSet.MasterEQ.EQMidGain;
                SetStudioSetStudioSetMasterEqMidGain((Int32)slStudioSetMasterEqMidGain.Value);
            }
            if (cbStudioSetMasterEqMidQ.SelectedIndex != commonState.studioSet.MasterEQ.EQMidQ)
            {
                cbStudioSetMasterEqMidQ.SelectedIndex = commonState.studioSet.MasterEQ.EQMidQ;
                SetStudioSetStudioSetMasterEqMidQ(cbStudioSetMasterEqMidQ.SelectedIndex);
            }
            if (cbStudioSetMasterEqHighFreq.SelectedIndex != commonState.studioSet.MasterEQ.EQHighFreq)
            {
                cbStudioSetMasterEqHighFreq.SelectedIndex = commonState.studioSet.MasterEQ.EQHighFreq;
                SetStudioSetStudioSetMasterEqHighFreq(cbStudioSetMasterEqHighFreq.SelectedIndex);
            }
            if (slStudioSetMasterEqHighGain.Value != commonState.studioSet.MasterEQ.EQHighGain)
            {
                slStudioSetMasterEqHighGain.Value = commonState.studioSet.MasterEQ.EQHighGain;
                SetStudioSetStudioSetMasterEqHighGain(commonState.studioSet.MasterEQ.EQHighGain);
            }
            if (cbStudioSetPartSettings1ReceiveChannel.SelectedIndex != commonState.studioSet.PartMainSettings[part].ReceiveChannel)
            {
                cbStudioSetPartSettings1ReceiveChannel.SelectedIndex = commonState.studioSet.PartMainSettings[part].ReceiveChannel;
                SetStudioSetStudioSetPartSettings1ReceiveChannel(commonState.studioSet.PartMainSettings[part].ReceiveChannel, (byte)part);
            }
            if (cbStudioSetPartSettings1Receive.IsChecked != commonState.studioSet.PartMainSettings[part].ReceiveSwitch)
            {
                cbStudioSetPartSettings1Receive.IsChecked = commonState.studioSet.PartMainSettings[part].ReceiveSwitch;
                SetStudioSetStudioSetPartSettings1Receive(commonState.studioSet.PartMainSettings[part].ReceiveSwitch, (byte)part);
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
            if (slStudioSetPartSettings1Level.Value != commonState.studioSet.PartMainSettings[part].Level)
            {
                slStudioSetPartSettings1Level.Value = commonState.studioSet.PartMainSettings[part].Level;
                SetStudioSetStudioSetPartSettings1Level(commonState.studioSet.PartMainSettings[part].Level, (byte)part);
            }
            if (slStudioSetPartSettings1Pan.Value != commonState.studioSet.PartMainSettings[part].Pan)
            {
                slStudioSetPartSettings1Pan.Value = commonState.studioSet.PartMainSettings[part].Pan;
                SetStudioSetStudioSetPartSettings1Pan(commonState.studioSet.PartMainSettings[part].Pan, (byte)part);
            }
            if (slStudioSetPartSettings1CoarseTune.Value != commonState.studioSet.PartMainSettings[part].CoarseTune)
            {
                slStudioSetPartSettings1CoarseTune.Value = commonState.studioSet.PartMainSettings[part].CoarseTune;
                SetStudioSetStudioSetPartSettings1CoarseTune(commonState.studioSet.PartMainSettings[part].CoarseTune, (byte)part);
            }
            if (slStudioSetPartSettings1FineTune.Value != commonState.studioSet.PartMainSettings[part].FineTune)
            {
                slStudioSetPartSettings1FineTune.Value = commonState.studioSet.PartMainSettings[part].FineTune;
                SetStudioSetStudioSetPartSettings1FineTune(commonState.studioSet.PartMainSettings[part].FineTune, (byte)part);
            }
            if (cbStudioSetPartSettings1MonoPoly.SelectedIndex != commonState.studioSet.PartMainSettings[part].MonoPoly)
            {
                cbStudioSetPartSettings1MonoPoly.SelectedIndex = commonState.studioSet.PartMainSettings[part].MonoPoly;
                SetStudioSetStudioSetPartSettings1Poly(commonState.studioSet.PartMainSettings[part].MonoPoly, (byte)part);
            }
            if (cbStudioSetPartSettings1Legato.SelectedIndex != commonState.studioSet.PartMainSettings[part].Legato)
            {
                cbStudioSetPartSettings1Legato.SelectedIndex = commonState.studioSet.PartMainSettings[part].Legato;
                SetStudioSetStudioSetPartSettings1Legato(commonState.studioSet.PartMainSettings[part].Legato, (byte)part);
            }
            if (slStudioSetPartSettings1PitchBendRange.Value != commonState.studioSet.PartMainSettings[part].PitchBendRange)
            {
                slStudioSetPartSettings1PitchBendRange.Value = commonState.studioSet.PartMainSettings[part].PitchBendRange;
                SetStudioSetStudioSetPartSettings1BendRange(commonState.studioSet.PartMainSettings[part].PitchBendRange, (byte)part);
            }
            if (cbStudioSetPartSettings1Portamento.SelectedIndex != commonState.studioSet.PartMainSettings[part].PortamentoSwitch)
            {
                cbStudioSetPartSettings1Portamento.SelectedIndex = commonState.studioSet.PartMainSettings[part].PortamentoSwitch;
                SetStudioSetStudioSetPartSettings1Portamento(commonState.studioSet.PartMainSettings[part].PortamentoSwitch, (byte)part);
            }
            if (slStudioSetPartSettings1PortamentoTime.Value != commonState.studioSet.PartMainSettings[part].PortamentoTime)
            {
                slStudioSetPartSettings1PortamentoTime.Value = commonState.studioSet.PartMainSettings[part].PortamentoTime;
                SetStudioSetStudioSetPartSettings1PortamentoTime(commonState.studioSet.PartMainSettings[part].PortamentoTime, (byte)part);
            }
            if (slStudioSetPartSettings2CutoffOffset.Value != commonState.studioSet.PartMainSettings[part].CutoffOffset)
            {
                slStudioSetPartSettings2CutoffOffset.Value = commonState.studioSet.PartMainSettings[part].CutoffOffset;
                SetStudioSetStudioSetPartSettings2CutoffOffset(commonState.studioSet.PartMainSettings[part].CutoffOffset, (byte)part);
            }
            if (slStudioSetPartSettings2ResonanceOffset.Value != commonState.studioSet.PartMainSettings[part].ResonanceOffset)
            {
                slStudioSetPartSettings2ResonanceOffset.Value = commonState.studioSet.PartMainSettings[part].ResonanceOffset;
                SetStudioSetStudioSetPartSettings2ResonanceOffset(commonState.studioSet.PartMainSettings[part].ResonanceOffset, (byte)part);
            }
            if (slStudioSetPartSettings2AttackTimeOffset.Value != commonState.studioSet.PartMainSettings[part].AttackTimeOffset)
            {
                slStudioSetPartSettings2AttackTimeOffset.Value = commonState.studioSet.PartMainSettings[part].AttackTimeOffset;
                SetStudioSetStudioSetPartSettings2AttackTimeOffset(commonState.studioSet.PartMainSettings[part].AttackTimeOffset, (byte)part);
            }
            if (slStudioSetPartSettings2DecayTimeOffset.Value != commonState.studioSet.PartMainSettings[part].DecayTimeOffset)
            {
                slStudioSetPartSettings2DecayTimeOffset.Value = commonState.studioSet.PartMainSettings[part].DecayTimeOffset;
                SetStudioSetStudioSetPartSettings2DecayTimeOffset(commonState.studioSet.PartMainSettings[part].DecayTimeOffset, (byte)part);
            }
            if (slStudioSetPartSettings2ReleaseTimeOffset.Value != commonState.studioSet.PartMainSettings[part].ReleaseTimeOffset)
            {
                slStudioSetPartSettings2ReleaseTimeOffset.Value = commonState.studioSet.PartMainSettings[part].ReleaseTimeOffset;
                SetStudioSetStudioSetPartSettings2ReleaseTimeOffset(commonState.studioSet.PartMainSettings[part].ReleaseTimeOffset, (byte)part);
            }
            if (slStudioSetPartSettings2VibratoRate.Value != commonState.studioSet.PartMainSettings[part].VibratoRate)
            {
                slStudioSetPartSettings2VibratoRate.Value = commonState.studioSet.PartMainSettings[part].VibratoRate;
                SetStudioSetStudioSetPartSettings2VibratoRate(commonState.studioSet.PartMainSettings[part].VibratoRate, (byte)part);
            }
            if (slStudioSetPartSettings2VibratoDepth.Value != commonState.studioSet.PartMainSettings[part].VibratoDepth)
            {
                slStudioSetPartSettings2VibratoDepth.Value = commonState.studioSet.PartMainSettings[part].VibratoDepth;
                SetStudioSetStudioSetPartSettings2VibratoDepth(commonState.studioSet.PartMainSettings[part].VibratoDepth, (byte)part);
            }
            if (slStudioSetPartSettings2VibratoDelay.Value != commonState.studioSet.PartMainSettings[part].VibratoDelay)
            {
                slStudioSetPartSettings2VibratoDelay.Value = commonState.studioSet.PartMainSettings[part].VibratoDelay;
                SetStudioSetStudioSetPartSettings2VibratoDelay(commonState.studioSet.PartMainSettings[part].VibratoDelay, (byte)part);
            }
            if (slStudioSetPartEffectsChorusSendLevel.Value != commonState.studioSet.PartMainSettings[part].ChorusSendLevel)
            {
                slStudioSetPartEffectsChorusSendLevel.Value = commonState.studioSet.PartMainSettings[part].ChorusSendLevel;
                SetStudioSetStudioSetPartEffectsChorusSendLevel(commonState.studioSet.PartMainSettings[part].ChorusSendLevel, (byte)part);
            }
            if (slStudioSetPartEffectsReverbSendLevel.Value != commonState.studioSet.PartMainSettings[part].ReverbSendLevel)
            {
                slStudioSetPartEffectsReverbSendLevel.Value = commonState.studioSet.PartMainSettings[part].ReverbSendLevel;
                SetStudioSetStudioSetPartEffectsReverbSendLevel(commonState.studioSet.PartMainSettings[part].ReverbSendLevel, (byte)part);
            }
            if (cbStudioSetPartEffectsOutputAssign.SelectedIndex != commonState.studioSet.PartMainSettings[part].OutputAssign)
            {
                cbStudioSetPartEffectsOutputAssign.SelectedIndex = commonState.studioSet.PartMainSettings[part].OutputAssign;
                SetStudioSetStudioSetPartEffectsOutputAssign(commonState.studioSet.PartMainSettings[part].OutputAssign, (byte)part);
            }
            if (slStudioSetPartKeyboardOctaveShift.Value != commonState.studioSet.PartKeyboard[part].OctaveShift)
            {
                slStudioSetPartKeyboardOctaveShift.Value = commonState.studioSet.PartKeyboard[part].OctaveShift;
                SetStudioSetStudioSetPartKeyboardOctaveShift(commonState.studioSet.PartKeyboard[part].OctaveShift, (byte)part);
            }
            if (slStudioSetPartKeyboardVelocitySenseOffset.Value != commonState.studioSet.PartKeyboard[part].VelocitySenseOffset)
            {
                slStudioSetPartKeyboardVelocitySenseOffset.Value = commonState.studioSet.PartKeyboard[part].VelocitySenseOffset;
                SetStudioSetStudioSetPartKeyboardVelocitySenseOffset(commonState.studioSet.PartKeyboard[part].VelocitySenseOffset, (byte)part);
            }
            if (slStudioSetPartKeyboardRangeLower.Value != commonState.studioSet.PartKeyboard[part].RangeLower)
            {
                slStudioSetPartKeyboardRangeLower.Value = commonState.studioSet.PartKeyboard[part].RangeLower;
                SetStudioSetStudioSetPartKeyboardRangeLower(commonState.studioSet.PartKeyboard[part].RangeLower, (byte)part);
            }
            if (slStudioSetPartKeyboardRangeUpper.Value != commonState.studioSet.PartKeyboard[part].RangeUpper)
            {
                slStudioSetPartKeyboardRangeUpper.Value = commonState.studioSet.PartKeyboard[part].RangeUpper;
                SetStudioSetStudioSetPartKeyboardRangeUpper(commonState.studioSet.PartKeyboard[part].RangeUpper, (byte)part);
            }
            if (slStudioSetPartKeyboardFadeWidthLower.Value != commonState.studioSet.PartKeyboard[part].FadeWidthLower)
            {
                slStudioSetPartKeyboardFadeWidthLower.Value = commonState.studioSet.PartKeyboard[part].FadeWidthLower;
                SetStudioSetStudioSetPartKeyboardFadeWidthLower(commonState.studioSet.PartKeyboard[part].FadeWidthLower, (byte)part);
            }
            if (slStudioSetPartKeyboardFadeWidthUpper.Value != commonState.studioSet.PartKeyboard[part].FadeWidthUpper)
            {
                slStudioSetPartKeyboardFadeWidthUpper.Value = commonState.studioSet.PartKeyboard[part].FadeWidthUpper;
                SetStudioSetStudioSetPartKeyboardFadeWidthUpper(commonState.studioSet.PartKeyboard[part].FadeWidthUpper, (byte)part);
            }
            if (slStudioSetPartKeyboardVelocityRangeLower.Value != commonState.studioSet.PartKeyboard[part].VelocityRangeLower)
            {
                slStudioSetPartKeyboardVelocityRangeLower.Value = commonState.studioSet.PartKeyboard[part].VelocityRangeLower;
                SetStudioSetStudioSetPartKeyboardVelocityRangeLower(commonState.studioSet.PartKeyboard[part].VelocityRangeLower, (byte)part);
            }
            if (slStudioSetPartKeyboardVelocityRangeUpper.Value != commonState.studioSet.PartKeyboard[part].VelocityRangeUpper)
            {
                slStudioSetPartKeyboardVelocityRangeUpper.Value = commonState.studioSet.PartKeyboard[part].VelocityRangeUpper;
                SetStudioSetStudioSetPartKeyboardVelocityRangeUpper(commonState.studioSet.PartKeyboard[part].VelocityRangeUpper, (byte)part);
            }
            if (slStudioSetPartKeyboardVelocityFadeWidthLower.Value != commonState.studioSet.PartKeyboard[part].VelocityFadeWidthLower)
            {
                slStudioSetPartKeyboardVelocityFadeWidthLower.Value = commonState.studioSet.PartKeyboard[part].VelocityFadeWidthLower;
                SetStudioSetStudioSetPartKeyboardVelocityFadeWidthLower(commonState.studioSet.PartKeyboard[part].VelocityFadeWidthLower, (byte)part);
            }
            if (slStudioSetPartKeyboardVelocityFadeWidthUpper.Value != commonState.studioSet.PartKeyboard[part].VelocityFadeWidthUpper)
            {
                slStudioSetPartKeyboardVelocityFadeWidthUpper.Value = commonState.studioSet.PartKeyboard[part].VelocityFadeWidthUpper;
                SetStudioSetStudioSetPartKeyboardVelocityFadeWidthUpper(commonState.studioSet.PartKeyboard[part].VelocityFadeWidthUpper, (byte)part);
            }
            if (cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex != commonState.studioSet.PartKeyboard[part].VelocityCurveType)
            {
                cbStudioSetPartKeyboardVelocityCurveType.SelectedIndex = commonState.studioSet.PartKeyboard[part].VelocityCurveType;
                SetStudioSetStudioSetPartKeyboardVelocityCurveType(commonState.studioSet.PartKeyboard[part].VelocityCurveType, (byte)part);
            }
            if (cbStudioSetPartKeyboardMute.IsChecked != commonState.studioSet.PartKeyboard[part].Mute)
            {
                cbStudioSetPartKeyboardMute.IsChecked = commonState.studioSet.PartKeyboard[part].Mute;
                SetStudioSetStudioSetPartKeyboard(commonState.studioSet.PartKeyboard[part].Mute, (byte)part);
            }
            if (cbStudioSetPartScaleTuneType.SelectedIndex != commonState.studioSet.PartScaleTune[part].Type)
            {
                cbStudioSetPartScaleTuneType.SelectedIndex = commonState.studioSet.PartScaleTune[part].Type;
                SetStudioSetStudioSetPartScaleTuneType(commonState.studioSet.PartScaleTune[part].Type, (byte)part);
            }
            if (cbStudioSetPartScaleTuneKey.SelectedIndex != commonState.studioSet.PartScaleTune[part].Key)
            {
                cbStudioSetPartScaleTuneKey.SelectedIndex = commonState.studioSet.PartScaleTune[part].Key;
                SetStudioSetStudioSetPartScaleTune(commonState.studioSet.PartScaleTune[part].Key, (byte)part);
            }
            if (slStudioSetPartScaleTuneC.Value != commonState.studioSet.PartScaleTune[part].C)
            {
                slStudioSetPartScaleTuneC.Value = commonState.studioSet.PartScaleTune[part].C;
                SetStudioSetStudioSetPartScaleTuneC(commonState.studioSet.PartScaleTune[part].C, (byte)part);
            }
            if (slStudioSetPartScaleTuneCi.Value != commonState.studioSet.PartScaleTune[part].Ci)
            {
                slStudioSetPartScaleTuneCi.Value = commonState.studioSet.PartScaleTune[part].Ci;
                SetStudioSetStudioSetPartScaleTuneCi(commonState.studioSet.PartScaleTune[part].Ci, (byte)part);
            }
            if (slStudioSetPartScaleTuneD.Value != commonState.studioSet.PartScaleTune[part].D)
            {
                slStudioSetPartScaleTuneD.Value = commonState.studioSet.PartScaleTune[part].D;
                SetStudioSetStudioSetPartScaleTuneD(commonState.studioSet.PartScaleTune[part].D, (byte)part);
            }
            if (slStudioSetPartScaleTuneDi.Value != commonState.studioSet.PartScaleTune[part].Di)
            {
                slStudioSetPartScaleTuneDi.Value = commonState.studioSet.PartScaleTune[part].Di;
                SetStudioSetStudioSetPartScaleTuneDi(commonState.studioSet.PartScaleTune[part].Di, (byte)part);
            }
            if (slStudioSetPartScaleTuneE.Value != commonState.studioSet.PartScaleTune[part].E)
            {
                slStudioSetPartScaleTuneE.Value = commonState.studioSet.PartScaleTune[part].E;
                SetStudioSetStudioSetPartScaleTuneE(commonState.studioSet.PartScaleTune[part].E, (byte)part);
            }
            if (slStudioSetPartScaleTuneF.Value != commonState.studioSet.PartScaleTune[part].F)
            {
                slStudioSetPartScaleTuneF.Value = commonState.studioSet.PartScaleTune[part].F;
                SetStudioSetStudioSetPartScaleTuneF(commonState.studioSet.PartScaleTune[part].F, (byte)part);
            }
            if (slStudioSetPartScaleTuneFi.Value != commonState.studioSet.PartScaleTune[part].Fi)
            {
                slStudioSetPartScaleTuneFi.Value = commonState.studioSet.PartScaleTune[part].Fi;
                SetStudioSetStudioSetPartScaleTuneFi(commonState.studioSet.PartScaleTune[part].Fi, (byte)part);
            }
            if (slStudioSetPartScaleTuneG.Value != commonState.studioSet.PartScaleTune[part].G)
            {
                slStudioSetPartScaleTuneG.Value = commonState.studioSet.PartScaleTune[part].G;
                SetStudioSetStudioSetPartScaleTuneG(commonState.studioSet.PartScaleTune[part].G, (byte)part);
            }
            if (slStudioSetPartScaleTuneGi.Value != commonState.studioSet.PartScaleTune[part].Gi)
            {
                slStudioSetPartScaleTuneGi.Value = commonState.studioSet.PartScaleTune[part].Gi;
                SetStudioSetStudioSetPartScaleTuneGi(commonState.studioSet.PartScaleTune[part].Gi, (byte)part);
            }
            if (slStudioSetPartScaleTuneA.Value != commonState.studioSet.PartScaleTune[part].A)
            {
                slStudioSetPartScaleTuneA.Value = commonState.studioSet.PartScaleTune[part].A;
                SetStudioSetStudioSetPartScaleTuneA(commonState.studioSet.PartScaleTune[part].A, (byte)part);
            }
            if (slStudioSetPartScaleTuneAi.Value != commonState.studioSet.PartScaleTune[part].Ai)
            {
                slStudioSetPartScaleTuneAi.Value = commonState.studioSet.PartScaleTune[part].Ai;
                SetStudioSetStudioSetPartScaleTuneAi(commonState.studioSet.PartScaleTune[part].Ai, (byte)part);
            }
            if (slStudioSetPartScaleTuneB.Value != commonState.studioSet.PartScaleTune[part].B)
            {
                slStudioSetPartScaleTuneB.Value = commonState.studioSet.PartScaleTune[part].B;
                SetStudioSetStudioSetPartScaleTuneB(commonState.studioSet.PartScaleTune[part].B, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveProgramChange.IsChecked != commonState.studioSet.PartMidi[part].ReceiveProgramChange)
            {
                cbStudioSetPartMidiReceiveProgramChange.IsChecked = commonState.studioSet.PartMidi[part].ReceiveProgramChange;
                SetStudioSetStudioSetPartMidiReceiveProgramChange(commonState.studioSet.PartMidi[part].ReceiveProgramChange, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveBankSelect.IsChecked != commonState.studioSet.PartMidi[part].ReceiveBankSelect)
            {
                cbStudioSetPartMidiReceiveBankSelect.IsChecked = commonState.studioSet.PartMidi[part].ReceiveBankSelect;
                SetStudioSetStudioSetPartMidiReceiveBankSelect(commonState.studioSet.PartMidi[part].ReceiveBankSelect, (byte)part);
            }
            if (cbStudioSetPartMidiReceivePitchBend.IsChecked != commonState.studioSet.PartMidi[part].ReceivePitchBend)
            {
                cbStudioSetPartMidiReceivePitchBend.IsChecked = commonState.studioSet.PartMidi[part].ReceivePitchBend;
                SetStudioSetStudioSetPartMidiReceivePitchBend(commonState.studioSet.PartMidi[part].ReceivePitchBend, (byte)part);
            }
            if (cbStudioSetPartMidiReceivePolyphonicKeyPressure.IsChecked != commonState.studioSet.PartMidi[part].ReceivePolyphonicKeyPressure)
            {
                cbStudioSetPartMidiReceivePolyphonicKeyPressure.IsChecked = commonState.studioSet.PartMidi[part].ReceivePolyphonicKeyPressure;
                SetStudioSetStudioSetPartMidiReceivePolyphonicKeyPressure(commonState.studioSet.PartMidi[part].ReceivePolyphonicKeyPressure, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveChannelPressure.IsChecked != commonState.studioSet.PartMidi[part].ReceiveChannelPressure)
            {
                cbStudioSetPartMidiReceiveChannelPressure.IsChecked = commonState.studioSet.PartMidi[part].ReceiveChannelPressure;
                SetStudioSetPartMidiReceiveChannelPressure(commonState.studioSet.PartMidi[part].ReceiveChannelPressure, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveModulation.IsChecked != commonState.studioSet.PartMidi[part].ReceiveModulation)
            {
                cbStudioSetPartMidiReceiveModulation.IsChecked = commonState.studioSet.PartMidi[part].ReceiveModulation;
                SetStudioSetStudioSetPartMidiReceiveModulation(commonState.studioSet.PartMidi[part].ReceiveModulation, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveVolume.IsChecked != commonState.studioSet.PartMidi[part].ReceiveVolume)
            {
                cbStudioSetPartMidiReceiveVolume.IsChecked = commonState.studioSet.PartMidi[part].ReceiveVolume;
                SetStudioSetStudioSetPartMidiReceiveVolume(commonState.studioSet.PartMidi[part].ReceiveVolume, (byte)part);
            }
            if (cbStudioSetPartMidiReceivePan.IsChecked != commonState.studioSet.PartMidi[part].ReceivePan)
            {
                cbStudioSetPartMidiReceivePan.IsChecked = commonState.studioSet.PartMidi[part].ReceivePan;
                SetStudioSetStudioSetPartMidiReceivePan(commonState.studioSet.PartMidi[part].ReceivePan, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveExpression.IsChecked != commonState.studioSet.PartMidi[part].ReceiveExpression)
            {
                cbStudioSetPartMidiReceiveExpression.IsChecked = commonState.studioSet.PartMidi[part].ReceiveExpression;
                SetStudioSetStudioSetPartMidiReceiveExpression(commonState.studioSet.PartMidi[part].ReceiveExpression, (byte)part);
            }
            if (cbStudioSetPartMidiReceiveHold1.IsChecked != commonState.studioSet.PartMidi[part].ReceiveHold1)
            {
                cbStudioSetPartMidiReceiveHold1.IsChecked = commonState.studioSet.PartMidi[part].ReceiveHold1;
                SetStudioSetStudioSetPartMidiReceiveHold1(commonState.studioSet.PartMidi[part].ReceiveHold1, (byte)part);
            }
            if (cbStudioSetPartMidiPhaseLock.IsChecked != commonState.studioSet.PartMidi[part].PhaseLock)
            {
                cbStudioSetPartMidiPhaseLock.IsChecked = commonState.studioSet.PartMidi[part].PhaseLock;
                SetStudioSetStudioSetPartMidiPhaseLock(commonState.studioSet.PartMidi[part].PhaseLock, (byte)part);
            }
            if (slStudioSetPartMotionalSurroundLR.Value != commonState.studioSet.PartMotionalSurround[part].LR)
            {
                slStudioSetPartMotionalSurroundLR.Value = commonState.studioSet.PartMotionalSurround[part].LR;
                SetStudioSetStudioSetPartMotionalSurroundLR(commonState.studioSet.PartMotionalSurround[part].LR, (byte)part);
            }
            if (slStudioSetPartMotionalSurroundFB.Value != commonState.studioSet.PartMotionalSurround[part].FB)
            {
                slStudioSetPartMotionalSurroundFB.Value = commonState.studioSet.PartMotionalSurround[part].FB;
                SetStudioSetStudioSetPartMotionalSurroundFB(commonState.studioSet.PartMotionalSurround[part].FB, (byte)part);
            }
            if (slStudioSetPartMotionalSurroundWidth.Value != commonState.studioSet.PartMotionalSurround[part].Width)
            {
                slStudioSetPartMotionalSurroundWidth.Value = commonState.studioSet.PartMotionalSurround[part].Width;
                SetStudioSetStudioSetPartMotionalSurroundWidth(commonState.studioSet.PartMotionalSurround[part].Width, (byte)part);
            }
            if (slStudioSetPartMotionalSurroundAmbienceSendLevel.Value != commonState.studioSet.PartMotionalSurround[part].AmbienceSendLevel)
            {
                slStudioSetPartMotionalSurroundAmbienceSendLevel.Value = commonState.studioSet.PartMotionalSurround[part].AmbienceSendLevel;
                SetStudioSetStudioSetPartMotionalSurroundAmbienceSendLevel(commonState.studioSet.PartMotionalSurround[part].AmbienceSendLevel, (byte)part);
            }
            if (cbStudioSetPartEQSwitch.IsChecked != commonState.studioSet.PartEQ[part].EqSwitch)
            {
                cbStudioSetPartEQSwitch.IsChecked = commonState.studioSet.PartEQ[part].EqSwitch;
                SetStudioSetStudioSetPartEQ(commonState.studioSet.PartEQ[part].EqSwitch, (byte)part);
            }
            if (cbStudioSetPartEQLoqFreq.SelectedIndex != commonState.studioSet.PartEQ[part].EqLowFreq)
            {
                cbStudioSetPartEQLoqFreq.SelectedIndex = commonState.studioSet.PartEQ[part].EqLowFreq;
                SetStudioSetStudioSetPartEQLoqFreq(commonState.studioSet.PartEQ[part].EqLowFreq, (byte)part);
            }
            if (slStudioSetPartEQLowGain.Value != commonState.studioSet.PartEQ[part].EqLowGain)
            {
                slStudioSetPartEQLowGain.Value = commonState.studioSet.PartEQ[part].EqLowGain;
                SetStudioSetStudioSetPartEQLowGain(commonState.studioSet.PartEQ[part].EqLowGain, (byte)part);
            }
            if (cbStudioSetPartEQMidFreq.SelectedIndex != commonState.studioSet.PartEQ[part].EqMidFreq)
            {
                cbStudioSetPartEQMidFreq.SelectedIndex = commonState.studioSet.PartEQ[part].EqMidFreq;
                SetStudioSetStudioSetPartEQMidFreq(commonState.studioSet.PartEQ[part].EqMidFreq, (byte)part);
            }
            if (slStudioSetPartEQMidGain.Value != commonState.studioSet.PartEQ[part].EqMidGain)
            {
                slStudioSetPartEQMidGain.Value = commonState.studioSet.PartEQ[part].EqMidGain;
                SetStudioSetStudioSetPartEQMidGain(commonState.studioSet.PartEQ[part].EqMidGain, (byte)part);
            }
            if (cbStudioSetPartEQMidQ.SelectedIndex != commonState.studioSet.PartEQ[part].EqMidQ)
            {
                cbStudioSetPartEQMidQ.SelectedIndex = commonState.studioSet.PartEQ[part].EqMidQ;
                SetStudioSetStudioSetPartEQMidQ(commonState.studioSet.PartEQ[part].EqMidQ, (byte)part);
            }
            if (cbStudioSetPartEQHighFreq.SelectedIndex != commonState.studioSet.PartEQ[part].EqHighFreq)
            {
                cbStudioSetPartEQHighFreq.SelectedIndex = commonState.studioSet.PartEQ[part].EqHighFreq;
                SetStudioSetStudioSetPartEQHighFreq(commonState.studioSet.PartEQ[part].EqHighFreq, (byte)part);
            }
            if (slStudioSetPartEQHighGain.Value != commonState.studioSet.PartEQ[part].EqHighGain)
            {
                slStudioSetPartEQHighGain.Value = commonState.studioSet.PartEQ[part].EqHighGain;
                SetStudioSetStudioSetPartEQHighGain(commonState.studioSet.PartEQ[part].EqHighGain, (byte)part);
            }
        }
    }
}
