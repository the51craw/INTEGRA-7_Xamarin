﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Midi;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Integra_7_uwp
{
    public sealed partial class Edit
    {
        /// <summary>
        /// Populates parameter pages selector and instrument selector according to tone type.
        /// Builds all controls for PCM Synth Tone after having removed any previous ones.
        /// </summary>

        #region Controls common to all types

        private void UpdateParameterPagesSelector()
        {
            t.Trace("private void UpdateParameterPagesSelector()");

            EditToneParameterPageItems parameters = new EditToneParameterPageItems();
            cbEditTone_ParameterPages.Items.Clear();
            ComboBoxItem item;
            foreach (String s in parameters.ParameterPageItems(currentProgramType))
            {
                item = new ComboBoxItem();
                item.Content = s;
                cbEditTone_ParameterPages.Items.Add(item);
            }
            item = new ComboBoxItem();
            item.Content = "Save/Delete";
            cbEditTone_ParameterPages.Items.Add(item);

            // When selecting another part, that part might have another tone type, thus the items we just
            // read in to the combobox does not correspond to the previous ones, and currentParameterPageIndex is invalid.
            // Try this: Get the text of the current parameter page before updating the combobox, and then try
            // to find one after updating, that has the same name. If not, set selector to 0.

            Boolean found = false;
            for (byte i = 0; !found && i < cbEditTone_ParameterPages.Items.Count(); i++)
            {
                if ((String)((ComboBoxItem)cbEditTone_ParameterPages.Items[i]).Content == currentParameterPage)
                {
                    found = true;
                    cbEditTone_ParameterPages.SelectedIndex = i;
                    currentParameterPageIndex = i;
                }
            }
            if (!found && cbEditTone_ParameterPages.SelectedIndex < 0 && cbEditTone_ParameterPages.Items.Count > 0)
            {
                cbEditTone_ParameterPages.SelectedIndex = 0;
            }
        }

        private void Update_PartialSelector()
        {
            t.Trace("Update_PartialSelector()");
            cbEditTone_PartialSelector.Items.Clear();
            cbEditTone_PartialSelector.Items.Add("Partial 1");
            cbEditTone_PartialSelector.Items.Add("Partial 2");
            cbEditTone_PartialSelector.Items.Add("Partial 3");
            if (currentProgramType != ProgramType.SUPERNATURAL_SYNTH_TONE)
            {
                cbEditTone_PartialSelector.Items.Add("Partial 4");
            }
            if (currentPartial < cbEditTone_PartialSelector.Items.Count())
            {
                cbEditTone_PartialSelector.SelectedIndex = currentPartial;
            }
            else
            {
                cbEditTone_PartialSelector.SelectedIndex = 0;
            }
        }

        private void UpdateInstrumentSelector()
        {
            t.Trace("private void UpdateInstrumentSelector()");
            cbEditTone_InstrumentCategorySelector.Items.Clear();
            switch (currentProgramType)
            {
                case ProgramType.PCM_SYNTH_TONE:
                    cbEditTone_InstrumentCategorySelector.Visibility = Visibility.Visible;
                    cbEditTone_KeySelector.Visibility = Visibility.Collapsed;
                    foreach (String s in toneCategories.pcmToneCategoryNames)
                    {
                        if (!String.IsNullOrEmpty(s))
                        {
                            ComboBoxItem item = new ComboBoxItem();
                            item.Content = s;
                            cbEditTone_InstrumentCategorySelector.Items.Add(item);
                        }
                    }
                    cbEditTone_InstrumentCategorySelector.SelectedIndex = toneCategories.pcmToneCategoryNameIndex[pCMSynthTone.pCMSynthToneCommon2.ToneCategory];
                    break;
                case ProgramType.PCM_DRUM_KIT:
                    cbEditTone_InstrumentCategorySelector.Visibility = Visibility.Collapsed;
                    cbEditTone_KeySelector.Visibility = Visibility.Visible;
                    break;
                case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                    cbEditTone_InstrumentCategorySelector.Visibility = Visibility.Visible;
                    cbEditTone_KeySelector.Visibility = Visibility.Collapsed;
                    foreach (String s in toneCategories.snaToneCategoryNames)
                    {
                        if (!String.IsNullOrEmpty(s))
                        {
                            ComboBoxItem item = new ComboBoxItem();
                            item.Content = s;
                            cbEditTone_InstrumentCategorySelector.Items.Add(item);
                        }
                    }
                    cbEditTone_InstrumentCategorySelector.SelectedIndex = toneCategories.snaToneCategoryNameIndex[superNATURALAcousticTone.superNATURALAcousticToneCommon.Category];
                    break;
                case ProgramType.SUPERNATURAL_SYNTH_TONE:
                    cbEditTone_InstrumentCategorySelector.Visibility = Visibility.Visible;
                    cbEditTone_KeySelector.Visibility = Visibility.Collapsed;
                    foreach (String s in toneCategories.snsToneCategoryNames)
                    {
                        if (!String.IsNullOrEmpty(s))
                        {
                            ComboBoxItem item = new ComboBoxItem();
                            item.Content = s;
                            cbEditTone_InstrumentCategorySelector.Items.Add(item);
                        }
                    }
                    cbEditTone_InstrumentCategorySelector.SelectedIndex = toneCategories.snsToneCategoryNameIndex[superNATURALSynthTone.superNATURALSynthToneCommon.Category];
                    break;
                case ProgramType.SUPERNATURAL_DRUM_KIT:
                    cbEditTone_InstrumentCategorySelector.Visibility = Visibility.Collapsed;
                    cbEditTone_KeySelector.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void UpdatePCMDrumKitKeySelector()
        {
            cbEditTone_InstrumentCategorySelector.Items.Clear();
            for (byte i = 0; i < 88; i++)
            {
                cbEditTone_KeySelector.Items.Add("Key " + (i + 21).ToString() + " (" + keyNames[i + 21] + ")");
            }
            if (currentKey >= 0 && currentKey < cbEditTone_KeySelector.Items.Count())
            {
                cbEditTone_KeySelector.SelectedIndex = currentKey;
            }
            else
            {
                cbEditTone_KeySelector.SelectedIndex = 0;
            }
        }

        private void UpdateSuperNaturalDrumKitKeySelector()
        {
            cbEditTone_InstrumentCategorySelector.Items.Clear();
            for (byte i = 0; i < 62; i++)
            {
                cbEditTone_KeySelector.Items.Add("Key " + (i + 27).ToString() + " (" + keyNames[i + 27] + ")");
            }
            if (currentKey >= 0 && currentKey < cbEditTone_KeySelector.Items.Count())
            {
                cbEditTone_KeySelector.SelectedIndex = currentKey;
            }
            else
            {
                cbEditTone_KeySelector.SelectedIndex = 0;
            }
        }

        #endregion

        #region Main entry points

        private void MakePCMSynthToneControls(byte SelectedIndex = 0)
        {
            // Bug tracing! Do we need to use SelectedIndex? Because it is written into currentPartial causing havoc!
            t.Trace("private void MakePCMSynthToneControls (" + "byte " + SelectedIndex.ToString() + ", " + ")");
            UpdatePCMSynthToneControls(SelectedIndex);
        }

        private void UpdatePCMSynthToneControls(byte SelectedIndex = 0)
        {
            RemoveControls(ControlsGrid); // How can this function set currentMFXTypeOffset and currentMFXTypePageParameterOffset to zero by removing a control?!?
            switch (currentParameterPageIndex)
            {
                case 0:
                    AddPCMSynthToneCommonControls();
                    break;
                case 1:
                    AddPCMSynthToneWaveControls(SelectedIndex);
                    break;
                case 2:
                    AddPCMSynthTonePMTControls(SelectedIndex);
                    break;
                case 3:
                    AddPCMSynthTonePitchControls(SelectedIndex);
                    break;
                case 4:
                    AddPCMSynthTonePitchEnvControls(SelectedIndex);
                    break;
                case 5:
                    AddPCMSynthToneTVFControls(SelectedIndex);
                    break;
                case 6:
                    AddPCMSynthToneTVFEnvControls(SelectedIndex);
                    break;
                case 7:
                    AddPCMSynthToneTVAControls(SelectedIndex);
                    break;
                case 8:
                    AddPCMSynthToneTVAEnvControls(SelectedIndex);
                    break;
                case 9:
                    AddPCMSynthToneOutputControls(SelectedIndex);
                    break;
                case 10:
                    AddPCMSynthToneLFO01Controls(SelectedIndex);
                    break;
                case 11:
                    AddPCMSynthToneLFO02Controls(SelectedIndex);
                    break;
                case 12:
                    AddPCMSynthToneStepLFOControls(SelectedIndex);
                    break;
                case 13:
                    AddPCMSynthToneControlControls(SelectedIndex);
                    break;
                case 14:
                    AddPCMSynthToneMatrixControlControls();
                    break;
                case 15:
                    AddMFXControls();
                    break;
                case 16:
                    AddPCMSynthToneMFXControlControls();
                    break;
                case 17:
                    AddSaveDeleteControls();
                    break;
            }
            Waiting(false);
        }

        private void MakePCMDrumKitControls(byte SelectedIndex = 0)
        {
            t.Trace("private void MakePCMDrumKitControls (" + "byte" + SelectedIndex + ", " + ")");
            //RemoveControls(ControlsGrid);
            UpdatePCMDrumKitKeySelector();
            cbEditTone_PartialSelector.SelectedIndex = currentPartial;
            cbEditTone_KeySelector.SelectedIndex = currentKey;
            UpdatePCMDrumKitControls();
        }

        private void UpdatePCMDrumKitControls()
        {
            RemoveControls(ControlsGrid);
            switch (currentParameterPageIndex)
            {
                case 0:
                    cbEditTone_PartialSelector.Visibility = Visibility.Collapsed;
                    tbEditTone_KeyName.Visibility = Visibility.Visible;
                    if (commonState.keyNames != null && cbEditTone_KeySelector.SelectedIndex > -1)
                    {
                        tbEditTone_KeyName.Text = commonState.keyNames[cbEditTone_KeySelector.SelectedIndex];
                    }
                    else
                    {
                        tbEditTone_KeyName.Text = pCMDrumKit.pCMDrumKitPartial.Name;
                    }
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitCommonControls();
                    break;
                case 1:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitWaveControls();
                    break;
                case 2:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitWMTControls(0);
                    break;
                case 3:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitPitchControls(0);
                    break;
                case 4:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitPitchEnvControls(0);
                    break;
                case 5:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitTVFControls(0);
                    break;
                case 6:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitTVFEnvControls(0);
                    break;
                case 7:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitTVAControls(0);
                    break;
                case 8:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitTVAEnvControls(0);
                    break;
                case 9:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitOutputControls(0);
                    break;
                case 10:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitCompressorControls(0);
                    break;
                case 11:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitEqualizerControls(0);
                    break;
                case 12:
                    RemoveControls(ControlsGrid);
                    AddMFXControls();
                    break;
                case 13:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitMFXControlControls();
                    break;
                case 14:
                    RemoveControls(ControlsGrid);
                    AddPCMDrumKitSaveControls();
                    break;
            }
            Waiting(false);
        }

        private void UpdateSuperNATURALAcousticToneControls(byte SelectedIndex = 0)
        {
            t.Trace("private void UpdateSuperNATURALAcousticToneControls (" + "byte" + SelectedIndex + ", " + ")");
            RemoveControls(ControlsGrid);
            switch (currentParameterPageIndex)
            {
                case 0:
                    AddSupernaturalAcousticToneCommonControls(SelectedIndex);
                    break;
                case 1:
                    AddSupernaturalAcousticToneInstrumentControls(SelectedIndex);
                    break;
                case 2:
                    AddMFXControls();
                    //AddSupernaturalAcousticToneMFXControls(SelectedIndex);
                    break;
                case 3:
                    AddSupernaturalAcousticToneMFXcontrolControls(SelectedIndex);
                    break;
                case 4:
                    AddSuperNaturalAcousticToneSaveControls();
                    break;
            }
            Waiting(false);
        }

        private void UpdateSuperNATURALSynthToneControls(byte SelectedIndex = 0)
        {
            t.Trace("private void UpdateSuperNATURALSynthToneControls (" + "byte " + SelectedIndex.ToString() + ", " + ")");
            RemoveControls(ControlsGrid);
            switch (currentParameterPageIndex)
            {
                case 0:
                    AddSupernaturalSynthToneCommonControls(SelectedIndex);
                    break;
                case 1:
                    AddSupernaturalSynthToneOscControls(SelectedIndex);
                    break;
                case 2:
                    AddSupernaturalSynthTonePitchControls(SelectedIndex);
                    break;
                case 3:
                    AddSupernaturalSynthToneFilterControls(SelectedIndex);
                    break;
                case 4:
                    AddSupernaturalSynthToneAMPControls(SelectedIndex);
                    break;
                case 5:
                    AddSupernaturalSynthToneLFOControls(SelectedIndex);
                    break;
                case 6:
                    AddSupernaturalSynthToneModLFOControls(SelectedIndex);
                    break;
                case 7:
                    AddSupernaturalSynthToneAftertouchControls(SelectedIndex);
                    break;
                case 8:
                    AddSupernaturalSynthToneMiscControls(SelectedIndex);
                    break;
                case 9:
                    AddMFXControls();
                    break;
                case 10:
                    AddSuperNaturalSynthToneMFXControlControls();
                    break;
                case 11:
                    AddSuperNaturalSynthToneSaveControls();
                    break;
            }
            Waiting(false);
        }

        private void UpdateSuperNATURALDrumKitControls(byte SelectedIndex = 0)
        {
            t.Trace("private void UpdateSuperNATURALDrumKitControls (" + "byte" + SelectedIndex + ", " + ")");
            UpdateSuperNaturalDrumKitKeySelector();
            RemoveControls(ControlsGrid);
            switch (currentParameterPageIndex)
            {
                case 0:
                    cbEditTone_PartialSelector.Visibility = Visibility.Collapsed;
                    tbEditTone_KeyName.Visibility = Visibility.Visible;
                    //tbEditTone_KeyName.Text = superNATURALDrumKit.superNATURALDrumKitKey.na
                    AddSupernaturalDrumKitCommonControls(SelectedIndex);
                    break;
                case 1:
                    AddSupernaturalDrumKitDruminstrumentControls(SelectedIndex);
                    break;
                case 2:
                    AddSupernaturalDrumKitCompressorControls(SelectedIndex);
                    break;
                case 3:
                    AddSupernaturalDrumKitEqualizerControls(SelectedIndex);
                    break;
                case 4:
                    AddMFXControls();
                    break;
                case 5:
                    AddSuperNaturalDrumKitMFXControlControls();
                    break;
                case 6:
                    AddSuperNaturalDrumKitSaveControls();
                    break;
            }
            Waiting(false);
        }

        #endregion

        #region PCM Synth Tone

        private void AddPCMSynthToneCommonControls()
        {
            t.Trace("private void AddPCMSynthToneCommonControls()");
            controlsIndex = 0;
            // Create all controls and put into lines:
            // Phrase number:
            ComboBox cbEditTone_PCMSynthTone_PhraseNumber = new ComboBox();
            cbEditTone_PCMSynthTone_PhraseNumber.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_PhraseNumber.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_PhraseNumber.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_PhraseNumber.Name = "cbEditTone_PCMSynthTone_PhraseNumber";
            UInt16 i = 0;
            foreach (String item in phrases.Names)
            {
                cbEditTone_PCMSynthTone_PhraseNumber.Items.Add("Phrase" + i.ToString() + ":" + item);
                i++;
            }

            // Phrase octave shift:
            ComboBox cbEditTone_PCMSynthTone_PhraseOctaveShift = new ComboBox();
            cbEditTone_PCMSynthTone_PhraseOctaveShift.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_PhraseOctaveShift.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_PhraseOctaveShift.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_PhraseOctaveShift.Name = "cbEditTone_PCMSynthTone_PhraseOctaveShift";
            for (i = 0; i < 7; i++)
            {
                cbEditTone_PCMSynthTone_PhraseOctaveShift.Items.Add("Phrase octave shift " + (i - 3).ToString());
            }

            // Synth tone level:
            tbEditTone_PCMSynthTone_ToneLevel.Text = "Tone level: " + pCMSynthTone.pCMSynthToneCommon.Level.ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_ToneLevel);
            Slider slEditTone_PCMSynthTone_ToneLevel = new Slider();
            slEditTone_PCMSynthTone_ToneLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_ToneLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_ToneLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_ToneLevel.Name = "slEditTone_PCMSynthTone_ToneLevel";
            slEditTone_PCMSynthTone_ToneLevel.Minimum = 0;
            slEditTone_PCMSynthTone_ToneLevel.Maximum = 127;

            // Synth tone pan:
            if (pCMSynthTone.pCMSynthToneCommon.Pan < 64)
            {
                tbEditTone_PCMSynthTone_TonePan.Text = "Pan: L" + (Math.Abs(pCMSynthTone.pCMSynthToneCommon.Pan - 64)).ToString();
            }
            else if (pCMSynthTone.pCMSynthToneCommon.Pan == 64)
            {
                tbEditTone_PCMSynthTone_TonePan.Text = "Pan: Center";
            }
            else
            {
                tbEditTone_PCMSynthTone_TonePan.Text = "Pan: R" + (pCMSynthTone.pCMSynthToneCommon.Pan - 64).ToString();
            }
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TonePan);
            Slider slEditTone_PCMSynthTone_TonePan = new Slider();
            slEditTone_PCMSynthTone_TonePan.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TonePan.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TonePan.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TonePan.Name = "slEditTone_PCMSynthTone_TonePan";
            slEditTone_PCMSynthTone_TonePan.Minimum = -64;
            slEditTone_PCMSynthTone_TonePan.Maximum = 63;

            // Tone proirity:
            ComboBox cbEditTone_PCMSynthTone_TonePriority = new ComboBox();
            cbEditTone_PCMSynthTone_TonePriority.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_TonePriority.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_TonePriority.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_TonePriority.Name = "cbEditTone_PCMSynthTone_TonePriority";
            cbEditTone_PCMSynthTone_TonePriority.Items.Add("Tone priority: last");
            cbEditTone_PCMSynthTone_TonePriority.Items.Add("Tone priority: loudest");

            // Octave shift -3 - +3
            ComboBox cbEditTone_PCMSynthTone_OctaveShift = new ComboBox();
            cbEditTone_PCMSynthTone_OctaveShift.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_OctaveShift.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_OctaveShift.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_OctaveShift.Name = "cbEditTone_PCMSynthTone_OctaveShift";
            for (i = 0; i < 7; i++)
            {
                cbEditTone_PCMSynthTone_OctaveShift.Items.Add("Octave shift " + (i - 3).ToString());
            }

            // Stretch tune depth Off, 1 - 3
            ComboBox cbEditTone_PCMSynthTone_StretchTuneDepth = new ComboBox();
            cbEditTone_PCMSynthTone_StretchTuneDepth.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_StretchTuneDepth.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_StretchTuneDepth.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_StretchTuneDepth.Name = "cbEditTone_PCMSynthTone_StretchTuneDepth";
            cbEditTone_PCMSynthTone_StretchTuneDepth.Items.Add("Tune depth off");
            cbEditTone_PCMSynthTone_StretchTuneDepth.Items.Add("Tune depth 1");
            cbEditTone_PCMSynthTone_StretchTuneDepth.Items.Add("Tune depth 2");
            cbEditTone_PCMSynthTone_StretchTuneDepth.Items.Add("Tune depth 3");

            // Coarse tune -48 -- +48
            tbEditTone_PCMSynthTone_CoarseTune.Text = "Coarse tune: " + (pCMSynthTone.pCMSynthToneCommon.CoarseTune - 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_CoarseTune);
            Slider slEditTone_PCMSynthTone_CoarseTune = new Slider();
            slEditTone_PCMSynthTone_CoarseTune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_CoarseTune.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_CoarseTune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_CoarseTune.Name = "slEditTone_PCMSynthTone_CoarseTune";
            slEditTone_PCMSynthTone_CoarseTune.Minimum = -48;
            slEditTone_PCMSynthTone_CoarseTune.Maximum = 48;

            // Fine tune -50 -- +50
            tbEditTone_PCMSynthTone_FineTune.Text = "Fine tune: " + (pCMSynthTone.pCMSynthToneCommon.FineTune - 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_FineTune);
            Slider slEditTone_PCMSynthTone_FineTune = new Slider();
            slEditTone_PCMSynthTone_FineTune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_FineTune.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_FineTune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_FineTune.Name = "slEditTone_PCMSynthTone_FineTune";
            slEditTone_PCMSynthTone_FineTune.Minimum = -50;
            slEditTone_PCMSynthTone_FineTune.Maximum = 50;

            // Analog feel 0 - 127
            tbEditTone_PCMSynthTone_AnalogFeel.Text = "Analog feel: " + pCMSynthTone.pCMSynthToneCommon.AnalogFeel.ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_AnalogFeel);
            Slider slEditTone_PCMSynthTone_AnalogFeel = new Slider();
            slEditTone_PCMSynthTone_AnalogFeel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_AnalogFeel.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_AnalogFeel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_AnalogFeel.Name = "slEditTone_PCMSynthTone_AnalogFeel";
            slEditTone_PCMSynthTone_AnalogFeel.Minimum = 0;
            slEditTone_PCMSynthTone_AnalogFeel.Maximum = 127;

            // Cutoff offset -63 -- +63
            tbEditTone_PCMSynthTone_CutoffOffset.Text = "Cutoff offset: " + (pCMSynthTone.pCMSynthToneCommon.CutoffOffset - 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_CutoffOffset);
            Slider slEditTone_PCMSynthTone_CutoffOffset = new Slider();
            slEditTone_PCMSynthTone_CutoffOffset.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_CutoffOffset.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_CutoffOffset.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_CutoffOffset.Name = "slEditTone_PCMSynthTone_CutoffOffset";
            slEditTone_PCMSynthTone_CutoffOffset.Minimum = -63;
            slEditTone_PCMSynthTone_CutoffOffset.Maximum = 63;

            // Resonance offset -63 -- +63
            tbEditTone_PCMSynthTone_ResonanceOffset.Text = "Resonance offset: " + (pCMSynthTone.pCMSynthToneCommon.ResonanceOffset - 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_ResonanceOffset);
            Slider slEditTone_PCMSynthTone_ResonanceOffset = new Slider();
            slEditTone_PCMSynthTone_ResonanceOffset.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_ResonanceOffset.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_ResonanceOffset.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_ResonanceOffset.Name = "slEditTone_PCMSynthTone_ResonanceOffset";
            slEditTone_PCMSynthTone_ResonanceOffset.Minimum = -63;
            slEditTone_PCMSynthTone_ResonanceOffset.Maximum = 63;

            // Attack time offset -63 -- +63
            tbEditTone_PCMSynthTone_AttackTimeOffset.Text = "Attack time offset: " + (pCMSynthTone.pCMSynthToneCommon.AttackTimeOffset - 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_AttackTimeOffset);
            Slider slEditTone_PCMSynthTone_AttackTimeOffset = new Slider();
            slEditTone_PCMSynthTone_AttackTimeOffset.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_AttackTimeOffset.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_AttackTimeOffset.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_AttackTimeOffset.Name = "slEditTone_PCMSynthTone_AttackTimeOffset";
            slEditTone_PCMSynthTone_AttackTimeOffset.Minimum = -63;
            slEditTone_PCMSynthTone_AttackTimeOffset.Maximum = 63;

            // Release time offset -63 -- +63
            tbEditTone_PCMSynthTone_ReleaseTimeOffset.Text = "Release time offset: " + (pCMSynthTone.pCMSynthToneCommon.ReleaseTimeOffset - 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_ReleaseTimeOffset);
            Slider slEditTone_PCMSynthTone_ReleaseTimeOffset = new Slider();
            slEditTone_PCMSynthTone_ReleaseTimeOffset.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_ReleaseTimeOffset.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_ReleaseTimeOffset.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_ReleaseTimeOffset.Name = "slEditTone_PCMSynthTone_ReleaseTimeOffset";
            slEditTone_PCMSynthTone_ReleaseTimeOffset.Minimum = -63;
            slEditTone_PCMSynthTone_ReleaseTimeOffset.Maximum = 63;

            // Velocity sense offset -63 -- +63
            tbEditTone_PCMSynthTone_VelocitySenseOffset.Text = "Velocity sense offset: " + (pCMSynthTone.pCMSynthToneCommon.VelocitySenseOffset - 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_VelocitySenseOffset);
            Slider slEditTone_PCMSynthTone_VelocitySenseOffset = new Slider();
            slEditTone_PCMSynthTone_VelocitySenseOffset.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_VelocitySenseOffset.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_VelocitySenseOffset.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_VelocitySenseOffset.Name = "slEditTone_PCMSynthTone_VelocitySenseOffset";
            slEditTone_PCMSynthTone_VelocitySenseOffset.Minimum = -63;
            slEditTone_PCMSynthTone_VelocitySenseOffset.Maximum = 63;

            // Mono/Poly 0,1:
            ComboBox cbEditTone_PCMSynthTone_MonoPoly = new ComboBox();
            cbEditTone_PCMSynthTone_MonoPoly.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_MonoPoly.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_MonoPoly.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_MonoPoly.Name = "cbEditTone_PCMSynthTone_MonoPoly";
            cbEditTone_PCMSynthTone_MonoPoly.Items.Add("Mono");
            cbEditTone_PCMSynthTone_MonoPoly.Items.Add("Poly");

            // Legato switch:
            CheckBox cbEditTone_PCMSynthTone_Legato = new CheckBox();
            cbEditTone_PCMSynthTone_Legato.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Legato.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Legato.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Legato.Content = "Legato";
            cbEditTone_PCMSynthTone_Legato.Name = "cbEditTone_PCMSynthTone_Legato";

            // Legato trigger:
            CheckBox cbEditTone_PCMSynthTone_LegatoTrigger = new CheckBox();
            cbEditTone_PCMSynthTone_LegatoTrigger.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_LegatoTrigger.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_LegatoTrigger.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_LegatoTrigger.Name = "cbEditTone_PCMSynthTone_LegatoTrigger";
            cbEditTone_PCMSynthTone_LegatoTrigger.Content = "Legato trigger";

            // Portamento switch:
            CheckBox cbEditTone_PCMSynthTone_Portamento = new CheckBox();
            cbEditTone_PCMSynthTone_Portamento.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Portamento.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Portamento.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Portamento.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Portamento.Name = "cbEditTone_PCMSynthTone_Portamento";
            cbEditTone_PCMSynthTone_Portamento.Content = "Portamento";

            // Portamento mode:
            ComboBox cbEditTone_PCMSynthTone_PortamentoMode = new ComboBox();
            cbEditTone_PCMSynthTone_PortamentoMode.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_PortamentoMode.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_PortamentoMode.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_PortamentoMode.Name = "cbEditTone_PCMSynthTone_PortamentoMode";
            cbEditTone_PCMSynthTone_PortamentoMode.Items.Add("Normal");
            cbEditTone_PCMSynthTone_PortamentoMode.Items.Add("Legato");

            // Portamento type:
            ComboBox cbEditTone_PCMSynthTone_PortamentoType = new ComboBox();
            cbEditTone_PCMSynthTone_PortamentoType.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_PortamentoType.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_PortamentoType.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_PortamentoType.Name = "cbEditTone_PCMSynthTone_PortamentoType";
            cbEditTone_PCMSynthTone_PortamentoType.Items.Add("Rate");
            cbEditTone_PCMSynthTone_PortamentoType.Items.Add("Time");

            // Portamento start:
            ComboBox cbEditTone_PCMSynthTone_PortamentoStart = new ComboBox();
            cbEditTone_PCMSynthTone_PortamentoStart.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_PortamentoStart.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_PortamentoStart.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_PortamentoStart.Name = "cbEditTone_PCMSynthTone_PortamentoStart";
            cbEditTone_PCMSynthTone_PortamentoStart.Items.Add("Pitch");
            cbEditTone_PCMSynthTone_PortamentoStart.Items.Add("Note");

            // Portamento time 0 - 127
            tbEditTone_PCMSynthTone_PortamentoTime.Text = "Portamento time: " + (pCMSynthTone.pCMSynthToneCommon.PortamentoTime).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_PortamentoTime);
            Slider slEditTone_PCMSynthTone_PortamentoTime = new Slider();
            slEditTone_PCMSynthTone_PortamentoTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_PortamentoTime.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_PortamentoTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_PortamentoTime.Name = "slEditTone_PCMSynthTone_PortamentoTime";
            slEditTone_PCMSynthTone_PortamentoTime.Minimum = 0;
            slEditTone_PCMSynthTone_PortamentoTime.Maximum = 127;

            // Put Phrase number and octave shift on line 0:
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_PhraseNumber, cbEditTone_PCMSynthTone_PhraseOctaveShift })).Row);
            // Put Level with label on line 1:
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMSynthTone_ToneLevel, slEditTone_PCMSynthTone_ToneLevel }, new byte[] { 1, 2 })).Row);

            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMSynthTone_TonePan, slEditTone_PCMSynthTone_TonePan }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { cbEditTone_PCMSynthTone_TonePriority, cbEditTone_PCMSynthTone_OctaveShift, cbEditTone_PCMSynthTone_StretchTuneDepth }, new byte[] { 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_PCMSynthTone_CoarseTune, slEditTone_PCMSynthTone_CoarseTune }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_PCMSynthTone_FineTune, slEditTone_PCMSynthTone_FineTune }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_PCMSynthTone_AnalogFeel, slEditTone_PCMSynthTone_AnalogFeel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_PCMSynthTone_CutoffOffset, slEditTone_PCMSynthTone_CutoffOffset }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_PCMSynthTone_ResonanceOffset, slEditTone_PCMSynthTone_ResonanceOffset }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { tbEditTone_PCMSynthTone_AttackTimeOffset, slEditTone_PCMSynthTone_AttackTimeOffset }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { tbEditTone_PCMSynthTone_ReleaseTimeOffset, slEditTone_PCMSynthTone_ReleaseTimeOffset }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(11, new Control[] { tbEditTone_PCMSynthTone_VelocitySenseOffset, slEditTone_PCMSynthTone_VelocitySenseOffset }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(12, new Control[] { cbEditTone_PCMSynthTone_MonoPoly, cbEditTone_PCMSynthTone_Legato, cbEditTone_PCMSynthTone_LegatoTrigger }, new byte[] { 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(13, new Control[] { cbEditTone_PCMSynthTone_Portamento, cbEditTone_PCMSynthTone_PortamentoMode, cbEditTone_PCMSynthTone_PortamentoType, cbEditTone_PCMSynthTone_PortamentoStart }, new byte[] { 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(14, new Control[] { tbEditTone_PCMSynthTone_PortamentoTime, slEditTone_PCMSynthTone_PortamentoTime }, new byte[] { 1, 2 })).Row);

            // Finally, set selected values (must be done after the controls has been given their parents):
            slEditTone_PCMSynthTone_ToneLevel.Value = pCMSynthTone.pCMSynthToneCommon.Level;
            cbEditTone_PCMSynthTone_PhraseNumber.SelectedIndex = pCMSynthTone.pCMSynthToneCommon2.PhraseNumber;
            cbEditTone_PCMSynthTone_PhraseOctaveShift.SelectedIndex = pCMSynthTone.pCMSynthToneCommon2.PhraseOctaveShift - 61;
            cbEditTone_PCMSynthTone_TonePriority.SelectedIndex = pCMSynthTone.pCMSynthToneCommon2.MissingInDocs;
            cbEditTone_PCMSynthTone_OctaveShift.SelectedIndex = pCMSynthTone.pCMSynthToneCommon.OctaveShift - 61;
            slEditTone_PCMSynthTone_TonePan.Value = pCMSynthTone.pCMSynthToneCommon.Pan - 64;
            slEditTone_PCMSynthTone_CoarseTune.Value = pCMSynthTone.pCMSynthToneCommon.CoarseTune - 64;
            slEditTone_PCMSynthTone_FineTune.Value = pCMSynthTone.pCMSynthToneCommon.FineTune - 64;
            cbEditTone_PCMSynthTone_StretchTuneDepth.SelectedIndex = pCMSynthTone.pCMSynthToneCommon.TuneDepth;
            slEditTone_PCMSynthTone_AnalogFeel.Value = pCMSynthTone.pCMSynthToneCommon.AnalogFeel;
            slEditTone_PCMSynthTone_CutoffOffset.Value = pCMSynthTone.pCMSynthToneCommon.CutoffOffset - 64;
            slEditTone_PCMSynthTone_ResonanceOffset.Value = pCMSynthTone.pCMSynthToneCommon.ResonanceOffset - 64;
            slEditTone_PCMSynthTone_AttackTimeOffset.Value = pCMSynthTone.pCMSynthToneCommon.AttackTimeOffset - 64;
            slEditTone_PCMSynthTone_ReleaseTimeOffset.Value = pCMSynthTone.pCMSynthToneCommon.ReleaseTimeOffset - 64;
            slEditTone_PCMSynthTone_VelocitySenseOffset.Value = pCMSynthTone.pCMSynthToneCommon.VelocitySenseOffset - 64;
            cbEditTone_PCMSynthTone_MonoPoly.SelectedIndex = pCMSynthTone.pCMSynthToneCommon.MonoPoly;
            cbEditTone_PCMSynthTone_Legato.IsChecked = pCMSynthTone.pCMSynthToneCommon.LegatoSwitch;
            cbEditTone_PCMSynthTone_LegatoTrigger.IsChecked = pCMSynthTone.pCMSynthToneCommon.LegatoRetrigger;
            cbEditTone_PCMSynthTone_Portamento.IsChecked = pCMSynthTone.pCMSynthToneCommon.PortamentoSwitch;
            cbEditTone_PCMSynthTone_PortamentoMode.SelectedIndex = pCMSynthTone.pCMSynthToneCommon.PortamentoMode;
            cbEditTone_PCMSynthTone_PortamentoType.SelectedIndex = pCMSynthTone.pCMSynthToneCommon.PortamentoType;
            cbEditTone_PCMSynthTone_PortamentoStart.SelectedIndex = pCMSynthTone.pCMSynthToneCommon.PortamentoStart;
        }

        private void AddPCMSynthToneWaveControls(byte Partial)
        {
            t.Trace("private void AddPCMSynthToneWaveControls (" + "byte" + Partial + ", " + ")");
            controlsIndex = 0;
            //currentPartial = Partial; // The event handlers are dependent on correct partial reference

            // CheckBox for PMTPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PMTPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PMTPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PMTPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // ComboBox for Wave Group Type
            cbEditTone_PCMSynthTone_WaveGroupType.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_WaveGroupType.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_WaveGroupType.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_WaveGroupType.Name = "cbEditTone_PCMSynthTone_Wave_WaveGroupType";
            cbEditTone_PCMSynthTone_WaveGroupType.Items.Clear();
            cbEditTone_PCMSynthTone_WaveGroupType.Items.Add("Wave Group: INT");
            cbEditTone_PCMSynthTone_WaveGroupType.Items.Add("Wave Group: SRX");
            cbEditTone_PCMSynthTone_WaveGroupType.Items.Add("Wave Group: ---");
            cbEditTone_PCMSynthTone_WaveGroupType.Items.Add("Wave Group: ---");

            // ComboBox for Wave Gain
            ComboBox cbEditTone_PCMSynthTone_WaveGain = new ComboBox();
            cbEditTone_PCMSynthTone_WaveGain.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_WaveGain.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_WaveGain.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_WaveGain.Name = "cbEditTone_PCMSynthTone_Wave_WaveGain";
            cbEditTone_PCMSynthTone_WaveGain.Items.Add("Gain: -6 Db");
            cbEditTone_PCMSynthTone_WaveGain.Items.Add("Gain: 0 Db");
            cbEditTone_PCMSynthTone_WaveGain.Items.Add("Gain: +6 Db");
            cbEditTone_PCMSynthTone_WaveGain.Items.Add("Gain: +12 Db");

            // Label for WaveNumberL:
            SetLabelProperties(ref tbEditTone_Wave_WaveNumberL);

            // ComboBox for Wave Number L
            ComboBox cbEditTone_PCMSynthTone_Wave_WaveNumberL = new ComboBox();
            cbEditTone_PCMSynthTone_Wave_WaveNumberL.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_Wave_WaveNumberL.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_WaveNumberL.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_WaveNumberL.Name = "cbEditTone_PCMSynthTone_Wave_WaveNumberL";

            // Label for WaveNumberR:
            SetLabelProperties(ref tbEditTone_Wave_WaveNumberR);

            // ComboBox for Wave Number R
            ComboBox cbEditTone_PCMSynthTone_Wave_WaveNumberR = new ComboBox();
            cbEditTone_PCMSynthTone_Wave_WaveNumberR.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_Wave_WaveNumberR.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_WaveNumberR.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_WaveNumberR.Name = "cbEditTone_PCMSynthTone_Wave_WaveNumberR";

            // CheckBox for Wave Tempo Sync
            CheckBox cbEditTone_PCMSynthTone_Wave_WaveTempoSync = new CheckBox();
            cbEditTone_PCMSynthTone_Wave_WaveTempoSync.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_WaveTempoSync.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_WaveTempoSync.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_WaveTempoSync.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_WaveTempoSync.Content = "Wave Tempo Sync";
            cbEditTone_PCMSynthTone_Wave_WaveTempoSync.Name = "cbEditTone_PCMSynthTone_Wave_WaveTempoSync";

            // CheckBox for Wave FXM Switch
            CheckBox cbEditTone_PCMSynthTone_Wave_WaveFXMSwitch = new CheckBox();
            cbEditTone_PCMSynthTone_Wave_WaveFXMSwitch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_WaveFXMSwitch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_WaveFXMSwitch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_WaveFXMSwitch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_WaveFXMSwitch.Content = "Wave FXM Switch";
            cbEditTone_PCMSynthTone_Wave_WaveFXMSwitch.Name = "cbEditTone_PCMSynthTone_Wave_WaveFXMSwitch";

            // ComboBox for FXM Color
            ComboBox cbEditTone_PCMSynthTone_FXMColor = new ComboBox();
            cbEditTone_PCMSynthTone_FXMColor.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_FXMColor.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_FXMColor.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_FXMColor.Name = "cbEditTone_PCMSynthTone_Wave_FXMColor";
            cbEditTone_PCMSynthTone_FXMColor.Items.Add("FXM Color 1");
            cbEditTone_PCMSynthTone_FXMColor.Items.Add("FXM Color 2");
            cbEditTone_PCMSynthTone_FXMColor.Items.Add("FXM Color 3");
            cbEditTone_PCMSynthTone_FXMColor.Items.Add("FXM Color 4");

            // Slider for Wave FXM Depth:
            SetLabelProperties(ref tbEditTone_pCMSynthTone_Wave_WaveFXMDepth);
            Slider slEditTone_pCMSynthTone_Wave_WaveFXMDepth = new Slider();
            slEditTone_pCMSynthTone_Wave_WaveFXMDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_pCMSynthTone_Wave_WaveFXMDepth.GotFocus += Generic_GotFocus;
            slEditTone_pCMSynthTone_Wave_WaveFXMDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_pCMSynthTone_Wave_WaveFXMDepth.Name = "slEditTone_pCMSynthTone_Wave_WaveFXMDepth";
            slEditTone_pCMSynthTone_Wave_WaveFXMDepth.Minimum = 0;
            slEditTone_pCMSynthTone_Wave_WaveFXMDepth.Maximum = 16;

            // ComboBox for Partial Delay Mode
            ComboBox cbEditTone_PCMSynthTone_Wave_PartialDelayMode = new ComboBox();
            cbEditTone_PCMSynthTone_Wave_PartialDelayMode.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_Wave_PartialDelayMode.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_PartialDelayMode.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_PartialDelayMode.Name = "cbEditTone_PCMSynthTone_Wave_PartialDelayMode";
            cbEditTone_PCMSynthTone_Wave_PartialDelayMode.Items.Add("Partial Delay Mode NORMAL");
            cbEditTone_PCMSynthTone_Wave_PartialDelayMode.Items.Add("Partial Delay Mode HOLD");
            cbEditTone_PCMSynthTone_Wave_PartialDelayMode.Items.Add("Partial Delay Mode KEY-OFF-NORMAL");
            cbEditTone_PCMSynthTone_Wave_PartialDelayMode.Items.Add("Partial Delay Mode KEY-OFF-DELAY");

            // Slider for Partial Delay Time:
            tbEditTone_PCMSynthTone_Wave_PartialDelayTime.Text = "Partial Delay Time: " + (pCMSynthTone.pCMSynthTonePartial.PartialDelayTime).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Wave_PartialDelayTime);
            Slider slEditTone_PCMSynthTone_Wave_PartialDelayTime = new Slider();
            slEditTone_PCMSynthTone_Wave_PartialDelayTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Wave_PartialDelayTime.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Wave_PartialDelayTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Wave_PartialDelayTime.Name = "slEditTone_PCMSynthTone_Wave_PartialDelayTime";
            slEditTone_PCMSynthTone_Wave_PartialDelayTime.Minimum = 0;
            slEditTone_PCMSynthTone_Wave_PartialDelayTime.Maximum = 149;

            // Insert controls in rows:
            ControlsGrid.Children.Add((new GridRow(0, new Control[] {
                cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch, cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch,
                cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch, cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch})).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_PCMSynthTone_WaveGroupType, cbEditTone_PCMSynthTone_WaveGain }, new byte[] { 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_Wave_WaveNumberL, cbEditTone_PCMSynthTone_Wave_WaveNumberL }, new byte[] { 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_Wave_WaveNumberR, cbEditTone_PCMSynthTone_Wave_WaveNumberR }, new byte[] { 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { cbEditTone_PCMSynthTone_Wave_WaveTempoSync, cbEditTone_PCMSynthTone_Wave_WaveFXMSwitch, cbEditTone_PCMSynthTone_FXMColor })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_pCMSynthTone_Wave_WaveFXMDepth, slEditTone_pCMSynthTone_Wave_WaveFXMDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { cbEditTone_PCMSynthTone_Wave_PartialDelayMode })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_PCMSynthTone_Wave_PartialDelayTime, slEditTone_PCMSynthTone_Wave_PartialDelayTime }, new byte[] { 1, 2 })).Row);

            // Set values:
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            if (pCMSynthTone.pCMSynthTonePartial.WaveGroupType< cbEditTone_PCMSynthTone_WaveGroupType.Items.Count())
            {
                cbEditTone_PCMSynthTone_WaveGroupType.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.WaveGroupType;
            }
            else if (cbEditTone_PCMSynthTone_WaveGroupType.Items.Count() > 0)
            {
                cbEditTone_PCMSynthTone_WaveGroupType.SelectedIndex = 0;
            }
            tbEditTone_Wave_WaveNumberL.Text = "Left wave (mono), Partial " + (currentPartial + 1).ToString() + ":";
            tbEditTone_Wave_WaveNumberR.Text = "Right wave, Partial " + (currentPartial + 1).ToString() + ":";
            foreach (String waveName in waveNames.Names)
            {
                cbEditTone_PCMSynthTone_Wave_WaveNumberL.Items.Add(waveName);
                cbEditTone_PCMSynthTone_Wave_WaveNumberR.Items.Add(waveName);
            }
            cbEditTone_PCMSynthTone_Wave_WaveNumberL.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.WaveNumberL;
            cbEditTone_PCMSynthTone_Wave_WaveNumberR.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.WaveNumberR;
            cbEditTone_PCMSynthTone_WaveGain.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.WaveGain;
            cbEditTone_PCMSynthTone_Wave_WaveTempoSync.IsChecked = pCMSynthTone.pCMSynthTonePartial.WaveTempoSync;
            cbEditTone_PCMSynthTone_Wave_WaveFXMSwitch.IsChecked = pCMSynthTone.pCMSynthTonePartial.WaveFXMSwitch;
            cbEditTone_PCMSynthTone_FXMColor.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.WaveFXMColor;
            slEditTone_pCMSynthTone_Wave_WaveFXMDepth.Value = (pCMSynthTone.pCMSynthTonePartial.WaveFXMDepth);
            tbEditTone_pCMSynthTone_Wave_WaveFXMDepth.Text = "Wave FXM Depth: " + ((pCMSynthTone.pCMSynthTonePartial.WaveFXMDepth)).ToString();
            cbEditTone_PCMSynthTone_Wave_PartialDelayMode.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.PartialDelayMode;
            slEditTone_PCMSynthTone_Wave_PartialDelayTime.Value = (double)pCMSynthTone.pCMSynthTonePartial.PartialDelayTime;
            if (pCMSynthTone.pCMSynthTonePartial.PartialDelayTime < 128)
            {
                tbEditTone_PCMSynthTone_Wave_PartialDelayTime.Text = "Partial Delay Time: " + pCMSynthTone.pCMSynthTonePartial.PartialDelayTime.ToString();
            }
            else
            {
                tbEditTone_PCMSynthTone_Wave_PartialDelayTime.Text = "Partial Delay Time: " + toneLengths[pCMSynthTone.pCMSynthTonePartial.PartialDelayTime - 127];
            }
        }

        private void AddPCMSynthTonePMTControls(byte PMTIndex)
        {
            t.Trace("private void AddPCMSynthTonePMTControls (" + "byte" + PMTIndex + ", " + ")");
            controlsIndex = 0;
            //currentPMT = PMTIndex; // The event handlers are dependent on correct partial reference

            // CheckBox for PMTPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PMTPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PMTPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PMTPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // Combobox for selecting partial mapping table
            ComboBox cbEditTone_PCMSynthTone_Partial = new ComboBox();
            cbEditTone_PCMSynthTone_Partial.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_Partial.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Partial.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Partial.Name = "cbEditTone_PCMSynthTone_PMT";
            cbEditTone_PCMSynthTone_Partial.Items.Add("PMT 1");
            cbEditTone_PCMSynthTone_Partial.Items.Add("PMT 2");
            cbEditTone_PCMSynthTone_Partial.Items.Add("PMT 3");
            cbEditTone_PCMSynthTone_Partial.Items.Add("PMT 4");

            // ComboBox for PMT Velocity Control
            ComboBox cbEditTone_PCMSynthTone_PMT_PMTVelocityControl = new ComboBox();
            cbEditTone_PCMSynthTone_PMT_PMTVelocityControl.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_PMT_PMTVelocityControl.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_PMT_PMTVelocityControl.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_PMT_PMTVelocityControl.Name = "cbEditTone_PCMSynthTone_PMT_PMTVelocityControl";
            cbEditTone_PCMSynthTone_PMT_PMTVelocityControl.Items.Add("PMT Velocity Off");
            cbEditTone_PCMSynthTone_PMT_PMTVelocityControl.Items.Add("PMT Velocity On");
            cbEditTone_PCMSynthTone_PMT_PMTVelocityControl.Items.Add("PMT Velocity Random");
            cbEditTone_PCMSynthTone_PMT_PMTVelocityControl.Items.Add("PMT Velocity Cycle");

            // CheckBox for PMT Control Switch
            CheckBox cbEditTone_PCMSynthTone_PMT_PMTControlSwitch = new CheckBox();
            cbEditTone_PCMSynthTone_PMT_PMTControlSwitch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_PMT_PMTControlSwitch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_PMT_PMTControlSwitch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_PMT_PMTControlSwitch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_PMT_PMTControlSwitch.Content = "PMT Control Switch";
            cbEditTone_PCMSynthTone_PMT_PMTControlSwitch.Name = "cbEditTone_PCMSynthTone_PMTControlSwitch";

            // ComboBox for Structure Type 1_2
            ComboBox cbEditTone_PCMSynthTone_PMT_StructureType1_2 = new ComboBox();
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.Name = "cbEditTone_PCMSynthTone_PMT_StructureType1_2";
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.Items.Add("Structure 1 & 2 Type: 1");
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.Items.Add("Structure 1 & 2 Type: 2");
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.Items.Add("Structure 1 & 2 Type: 3");
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.Items.Add("Structure 1 & 2 Type: 4");
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.Items.Add("Structure 1 & 2 Type: 5");
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.Items.Add("Structure 1 & 2 Type: 6");
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.Items.Add("Structure 1 & 2 Type: 7");
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.Items.Add("Structure 1 & 2 Type: 8");
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.Items.Add("Structure 1 & 2 Type: 9");
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.Items.Add("Structure 1 & 2 Type: 10");

            // ComboBox for Booster 1_2
            ComboBox cbEditTone_PCMSynthTone_PMT_Booster1_2 = new ComboBox();
            cbEditTone_PCMSynthTone_PMT_Booster1_2.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_PMT_Booster1_2.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_PMT_Booster1_2.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_PMT_Booster1_2.Name = "cbEditTone_PCMSynthTone_PMT_Booster1_2";
            cbEditTone_PCMSynthTone_PMT_Booster1_2.Items.Add("No boost");
            cbEditTone_PCMSynthTone_PMT_Booster1_2.Items.Add("Boost +6 Db");
            cbEditTone_PCMSynthTone_PMT_Booster1_2.Items.Add("Boost +12 Db");
            cbEditTone_PCMSynthTone_PMT_Booster1_2.Items.Add("Boost +18 Db");

            // ComboBox for Structure Type 3_4
            ComboBox cbEditTone_PCMSynthTone_PMT_StructureType3_4 = new ComboBox();
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.Name = "cbEditTone_PCMSynthTone_PMT_StructureType3_4";
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.Items.Add("Structure 3 & 4 Type: 1");
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.Items.Add("Structure 3 & 4 Type: 2");
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.Items.Add("Structure 3 & 4 Type: 3");
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.Items.Add("Structure 3 & 4 Type: 4");
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.Items.Add("Structure 3 & 4 Type: 5");
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.Items.Add("Structure 3 & 4 Type: 6");
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.Items.Add("Structure 3 & 4 Type: 7");
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.Items.Add("Structure 3 & 4 Type: 8");
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.Items.Add("Structure 3 & 4 Type: 9");
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.Items.Add("Structure 3 & 4 Type: 10");

            // ComboBox for Booster 3_4
            ComboBox cbEditTone_PCMSynthTone_PMT_Booster3_4 = new ComboBox();
            cbEditTone_PCMSynthTone_PMT_Booster3_4.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_PMT_Booster3_4.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_PMT_Booster3_4.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_PMT_Booster3_4.Name = "cbEditTone_PCMSynthTone_PMT_Booster3_4";
            cbEditTone_PCMSynthTone_PMT_Booster3_4.Items.Add("No boost");
            cbEditTone_PCMSynthTone_PMT_Booster3_4.Items.Add("Boost +6 Db");
            cbEditTone_PCMSynthTone_PMT_Booster3_4.Items.Add("Boost +12 Db");
            cbEditTone_PCMSynthTone_PMT_Booster3_4.Items.Add("Boost +18 Db");

            // Slider for PMT Keyboard Fade Width Upper:
            tbEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Fade Width Upper: " + (pCMSynthTone.pCMSynthTonePMT.PMTKeyboardFadeWidthUpper[currentPMT]).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper);
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper.Name = "slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper";
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper.Minimum = 0;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper.Maximum = 127;

            // Slider for PMT Keyboard Range Upper:
            tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Range Upper: " + (pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeUpper[currentPMT]).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper);
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper.Name = "slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper";
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper.Minimum = 0;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper.Maximum = 127;

            // Slider for PMT Keyboard Range Lower:
            tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Range Lower: " + (pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT]).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower);
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower.Name = "slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower";
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower.Minimum = 0;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower.Maximum = 127;

            // Slider for PMT Keyboard Fade Width Lower:
            tbEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Fade Width Lower: " + (pCMSynthTone.pCMSynthTonePMT.PMTKeyboardFadeWidthLower[currentPMT]).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower);
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower.Name = "slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower";
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower.Minimum = 0;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower.Maximum = 127;

            // Slider for PMT1 Velocity Fade Width Upper:
            tbEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Fade Width Upper: " + (pCMSynthTone.pCMSynthTonePMT.PMTVelocityFadeWidthUpper[currentPMT]).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper);
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper.Name = "slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper";
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper.Minimum = 0;
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper.Maximum = 127;

            // Slider for PMT1 Velocity Range Upper:
            tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Range Upper: " + (pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeUpper[currentPMT]).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper);
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper.Name = "slEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper";
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper.Minimum = 0;
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper.Maximum = 127;

            // Slider for PMPMT1 Velocity Range Lower:
            tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Range Lower: " + (pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT]).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower);
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower.Name = "slEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower";
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower.Minimum = 0;
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower.Maximum = 127;

            // Slider for PMT1 Velocity Fade Width Lower:
            tbEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Fade Width Lower: " + (pCMSynthTone.pCMSynthTonePMT.PMTVelocityFadeWidthLower[currentPMT]).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower);
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower.Name = "slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower";
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower.Minimum = 0;
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower.Maximum = 127;

            // Put controls on form
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch, cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch,
                cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch, cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch})).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_PCMSynthTone_Partial, cbEditTone_PCMSynthTone_PMT_PMTVelocityControl, cbEditTone_PCMSynthTone_PMT_PMTControlSwitch }, new byte[] { 2, 4, 3 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { cbEditTone_PCMSynthTone_PMT_StructureType1_2, cbEditTone_PCMSynthTone_PMT_Booster1_2, }, new byte[] { 2, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { cbEditTone_PCMSynthTone_PMT_StructureType3_4, cbEditTone_PCMSynthTone_PMT_Booster3_4 }, new byte[] { 2, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper, slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper }, new byte[] { 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper, slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper }, new byte[] { 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower, slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower }, new byte[] { 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower, slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower }, new byte[] { 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper, slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper, slEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper })).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower, slEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower })).Row);
            ControlsGrid.Children.Add((new GridRow(11, new Control[] { tbEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower, slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower })).Row);

            // Set controls values
            cbEditTone_PCMSynthTone_Wave_PMTPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Wave_PMTPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Wave_PMTPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Wave_PMTPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            cbEditTone_PCMSynthTone_Partial.SelectedIndex = currentPMT;
            cbEditTone_PCMSynthTone_PMT_PMTVelocityControl.SelectedIndex = pCMSynthTone.pCMSynthTonePMT.PMTVelocityControl;
            cbEditTone_PCMSynthTone_PMT_PMTControlSwitch.IsChecked = pCMSynthTone.pCMSynthToneCommon.PMTControlSwitch;
            cbEditTone_PCMSynthTone_PMT_StructureType1_2.SelectedIndex = pCMSynthTone.pCMSynthTonePMT.StructureType1_2;
            cbEditTone_PCMSynthTone_PMT_Booster1_2.SelectedIndex = pCMSynthTone.pCMSynthTonePMT.Booster1_2;
            cbEditTone_PCMSynthTone_PMT_StructureType3_4.SelectedIndex = pCMSynthTone.pCMSynthTonePMT.StructureType3_4;
            cbEditTone_PCMSynthTone_PMT_Booster3_4.SelectedIndex = pCMSynthTone.pCMSynthTonePMT.Booster3_4;
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper.Value = pCMSynthTone.pCMSynthTonePMT.PMTKeyboardFadeWidthUpper[currentPMT];
            tbEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Fade Width Upper: " + pCMSynthTone.pCMSynthTonePMT.PMTKeyboardFadeWidthUpper[currentPMT].ToString();
            tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Range Upper: " + keyNames[pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeUpper[currentPMT]];
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower.Value = pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT];
            slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper.Value = pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeUpper[currentPMT];
            tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Range Lower: " + keyNames[pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT]];
            slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower.Value = pCMSynthTone.pCMSynthTonePMT.PMTKeyboardFadeWidthLower[currentPMT];
            tbEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Fade Width Lower: " + pCMSynthTone.pCMSynthTonePMT.PMTKeyboardFadeWidthLower[currentPMT].ToString();
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper.Value = pCMSynthTone.pCMSynthTonePMT.PMTVelocityFadeWidthUpper[currentPMT];
            tbEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper.Text = "PMT" + currentPMT.ToString() + " Velocity Fade Width Upper: " + pCMSynthTone.pCMSynthTonePMT.PMTVelocityFadeWidthUpper[currentPMT].ToString();
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower.Value = pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT];
            tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Range Lower: " + pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT].ToString();
            slEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper.Value = pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeUpper[currentPMT];
            tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Range Upper: " + pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeUpper[currentPMT].ToString();
            slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower.Value = pCMSynthTone.pCMSynthTonePMT.PMTVelocityFadeWidthLower[currentPMT];
            tbEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Fade Width Lower: " + pCMSynthTone.pCMSynthTonePMT.PMTVelocityFadeWidthLower[currentPMT].ToString();

        }

        private void AddPCMSynthTonePitchControls(byte Partial)
        {
            t.Trace("private void AddPCMSynthTonePitchControls (" + "byte" + Partial + ", " + ")");
            controlsIndex = 0;

            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // Slider for Partial Coarse Tune:
            tbEditTone_PCMSynthTone_Pitch_PartialCoarseTune.Text = "Partial " + (currentPartial + 1).ToString() + " Coarse Tune: " + (pCMSynthTone.pCMSynthTonePartial.PartialCoarseTune - 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Pitch_PartialCoarseTune);
            Slider slEditTone_PCMSynthTone_Pitch_PartialCoarseTune = new Slider();
            slEditTone_PCMSynthTone_Pitch_PartialCoarseTune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Pitch_PartialCoarseTune.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Pitch_PartialCoarseTune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Pitch_PartialCoarseTune.Name = "slEditTone_PCMSynthTone_Pitch_PartialCoarseTune";
            slEditTone_PCMSynthTone_Pitch_PartialCoarseTune.Minimum = -48;
            slEditTone_PCMSynthTone_Pitch_PartialCoarseTune.Maximum = 48;

            // Slider for Partial Fine Tune:
            tbEditTone_PCMSynthTone_Pitch_PartialFineTune.Text = "Partial " + (currentPartial + 1).ToString() + " Fine Tune: " + (pCMSynthTone.pCMSynthTonePartial.PartialFineTune + 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Pitch_PartialFineTune);
            Slider slEditTone_PCMSynthTone_Pitch_PartialFineTune = new Slider();
            slEditTone_PCMSynthTone_Pitch_PartialFineTune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Pitch_PartialFineTune.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Pitch_PartialFineTune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Pitch_PartialFineTune.Name = "slEditTone_PCMSynthTone_Pitch_PartialFineTune";
            slEditTone_PCMSynthTone_Pitch_PartialFineTune.Minimum = -48;
            slEditTone_PCMSynthTone_Pitch_PartialFineTune.Maximum = 48;

            // ComboBox for Partial Random Pitch Depth
            ComboBox cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth = new ComboBox();
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Name = "cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth";
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("0");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("1");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("2");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("3");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("4");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("5");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("6");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("7");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("8");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("9");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("10");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("20");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("30");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("40");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("50");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("60");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("70");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("80");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("90");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("100");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("200");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("300");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("400");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("500");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("600");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("700");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("800");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("900");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("1000");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("1100");
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.Items.Add("1200");

            // Slider for Wave Pitch Keyfollow:
            tbEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow.Text = "Wave Pitch Keyfollow: " + (pCMSynthTone.pCMSynthTonePartial.WavePitchKeyfollow + 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow);
            Slider slEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow = new Slider();
            slEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow.Name = "slEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow";
            slEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow.StepFrequency = 10;
            slEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow.Minimum = -200;
            slEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow.Maximum = 200;

            // Slider for Pitch Bend Range Up:
            tbEditTone_PCMSynthTone_Pitch_PitchBendRangeUp.Text = "Pitch Bend Range Up: " + (pCMSynthTone.pCMSynthToneCommon.PitchBendRangeUp).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Pitch_PitchBendRangeUp);
            Slider slEditTone_PCMSynthTone_Pitch_PitchBendRangeUp = new Slider();
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeUp.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeUp.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeUp.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeUp.Name = "slEditTone_PCMSynthTone_Pitch_PitchBendRangeUp";
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeUp.Minimum = 0;
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeUp.Maximum = 24;

            // Slider for Pitch Bend Range Down:
            tbEditTone_PCMSynthTone_Pitch_PitchBendRangeDown.Text = "Pitch Bend Range Down: " + (pCMSynthTone.pCMSynthToneCommon.PitchBendRangeDown).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Pitch_PitchBendRangeDown);
            Slider slEditTone_PCMSynthTone_Pitch_PitchBendRangeDown = new Slider();
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeDown.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeDown.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeDown.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeDown.Name = "slEditTone_PCMSynthTone_Pitch_PitchBendRangeDown";
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeDown.Minimum = -48;
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeDown.Maximum = 0;

            // Put controls in rows 
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch
                , cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch})).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMSynthTone_Pitch_PartialCoarseTune, slEditTone_PCMSynthTone_Pitch_PartialCoarseTune }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMSynthTone_Pitch_PartialFineTune, slEditTone_PCMSynthTone_Pitch_PartialFineTune }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow, slEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_PCMSynthTone_Pitch_PitchBendRangeUp, slEditTone_PCMSynthTone_Pitch_PitchBendRangeUp }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_PCMSynthTone_Pitch_PitchBendRangeDown, slEditTone_PCMSynthTone_Pitch_PitchBendRangeDown }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            tbEditTone_PCMSynthTone_Pitch_PartialCoarseTune.Text = "Partial" + currentPartial.ToString() + " Coarse Tune: " + (pCMSynthTone.pCMSynthTonePartial.PartialCoarseTune - 64).ToString();
            slEditTone_PCMSynthTone_Pitch_PartialCoarseTune.Value = pCMSynthTone.pCMSynthTonePartial.PartialCoarseTune - 64;
            tbEditTone_PCMSynthTone_Pitch_PartialFineTune.Text = "Partial" + currentPartial.ToString() + " Fine Tune: " + (pCMSynthTone.pCMSynthTonePartial.PartialFineTune - 64).ToString();
            slEditTone_PCMSynthTone_Pitch_PartialFineTune.Value = pCMSynthTone.pCMSynthTonePartial.PartialFineTune - 64;
            cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.PartialRandomPitchDepth;
            slEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow.Value = (pCMSynthTone.pCMSynthTonePartial.WavePitchKeyfollow - 64) * 10;
            tbEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow.Text = "Wave Pitch Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial.WavePitchKeyfollow - 64) * 10).ToString();
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeUp.Value = pCMSynthTone.pCMSynthToneCommon.PitchBendRangeUp;
            tbEditTone_PCMSynthTone_Pitch_PitchBendRangeUp.Text = "Pitch Bend Range Up: " + (pCMSynthTone.pCMSynthToneCommon.PitchBendRangeUp).ToString();
            slEditTone_PCMSynthTone_Pitch_PitchBendRangeDown.Value = 0 - pCMSynthTone.pCMSynthToneCommon.PitchBendRangeDown;
            tbEditTone_PCMSynthTone_Pitch_PitchBendRangeDown.Text = "Pitch Bend Range Down: " + (0 - pCMSynthTone.pCMSynthToneCommon.PitchBendRangeDown).ToString();
        }

        private void AddPCMSynthTonePitchEnvControls(byte Partial)
        {
            t.Trace("private void AddPCMSynthTonePitchEnvControls (" + "byte" + Partial + ", " + ")");
            controlsIndex = 0;

            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // Slider for Pitch Env Depth:
            tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth.Text = "Pitch Env Depth: " + (pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvDepth + 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth);
            Slider slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth = new Slider();
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth.Name = "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth";
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth.Minimum = -12;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth.Maximum = 12;

            // Slider for Pitch Env Velocity Sens:
            tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens.Text = "Pitch Env Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvVelocitySens + 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens);
            Slider slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens = new Slider();
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens.Name = "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens";
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens.Minimum = -63;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens.Maximum = 63;

            // Slider for Pitch Env Time 1 Velocity Sens:
            tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens.Text = "Pitch Env Time 1 Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvTime1VelocitySens + 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens);
            Slider slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens = new Slider();
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens.Name = "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens";
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens.Minimum = -63;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens.Maximum = 63;

            // Slider for Pitch Env Time 4 Velocity Sens:
            tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens.Text = "Pitch Env Time 4 Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvTime4VelocitySens + 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens);
            Slider slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens = new Slider();
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens.Name = "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens";
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens.Minimum = -63;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens.Maximum = 63;

            // Slider for Pitch Env Time Keyfollow:
            tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow.Text = "Pitch Env Time Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvTimeKeyfollow - 64) * 10).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow);
            Slider slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow = new Slider();
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow.Name = "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow";
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow.Minimum = -100;
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow.Maximum = 100;

            // Slider for Pitch Env Time:
            Slider[] slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime = new Slider[4];
            for (byte i = 0; i < 4; i++)
            {
                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i] = new TextBox();
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i] = new Slider();
                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i].Text = "Pitch Env Time " + (i + 1).ToString() + ": " + (pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvTime[i]).ToString();
                SetLabelProperties(ref tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i]);
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i].Name = "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime" + (i + 1).ToString();
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i].Minimum = 0;
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i].Maximum = 127;
            }

            // Slider for Pitch Env Level:
            Slider[] slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel = new Slider[5];
            for (byte i = 0; i < 5; i++)
            {
                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i] = new TextBox();
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i] = new Slider();
                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i].Text = "Pitch Env Level: " + (pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvLevel[i] + 64).ToString();
                SetLabelProperties(ref tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i]);
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i].Name = "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel" + i.ToString();
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i].Minimum = -63;
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i].Maximum = 63;
            }

            // Put controls in rows 
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch
                , cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch})).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth, slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens, slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens, slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens, slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow, slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow }, new byte[] { 1, 2 })).Row);
            for (byte i = 0; i < 4; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(6 + i), new Control[] { tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i], slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i] }, new byte[] { 1, 2 })).Row);
            }
            for (byte i = 0; i < 5; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(10 + i), new Control[] { tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i], slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i] }, new byte[] { 1, 2 })).Row);
            }

            // Set control values
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth.Value = pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvDepth - 64;
            tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth.Text = "Depth: " + (pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvDepth - 64).ToString();
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens.Value = pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvVelocitySens - 64;
            tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens.Text = "Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvVelocitySens - 64).ToString();
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens.Value = pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvTime1VelocitySens - 64;
            tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens.Text = "Time 1 Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvTime1VelocitySens - 64).ToString();
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens.Value = pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvTime4VelocitySens - 64;
            tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens.Text = "Time 4 Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvTime4VelocitySens - 64).ToString();
            slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow.Value = pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvTimeKeyfollow - 64;
            tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow.Text = "Time Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvTimeKeyfollow - 64) * 10).ToString();
            for (byte i = 0; i < 4; i++)
            {
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i].Value = pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvTime[i];
                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[i].Text = "Time " + (i + 1).ToString() + ": " + (pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvTime[i]).ToString();
            }
            for (byte i = 0; i < 5; i++)
            {
                slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i].Value = pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvLevel[i] - 64;
                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[i].Text = "Level " + i.ToString() + ": " + (pCMSynthTone.pCMSynthTonePartial.PitchEnv.PitchEnvLevel[i] - 64).ToString();
            }
        }

        private void AddPCMSynthToneTVFControls(byte Partial)
        {
            t.Trace("private void AddPCMSynthToneTVFControls (" + "byte" + Partial + ", " + ")");
            controlsIndex = 0;

            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // ComboBox for TVF Filter Type
            ComboBox cbEditTone_PCMSynthTone_TVF_TVFFilterType = new ComboBox();
            cbEditTone_PCMSynthTone_TVF_TVFFilterType.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_TVF_TVFFilterType.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_TVF_TVFFilterType.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_TVF_TVFFilterType.Name = "cbEditTone_PCMSynthTone_TVF_TVFFilterType";
            cbEditTone_PCMSynthTone_TVF_TVFFilterType.Items.Add("TVF Filter Type Off");
            cbEditTone_PCMSynthTone_TVF_TVFFilterType.Items.Add("TVF Filter Type LPF");
            cbEditTone_PCMSynthTone_TVF_TVFFilterType.Items.Add("TVF Filter Type BPF");
            cbEditTone_PCMSynthTone_TVF_TVFFilterType.Items.Add("TVF Filter Type HPF");
            cbEditTone_PCMSynthTone_TVF_TVFFilterType.Items.Add("TVF Filter Type PKG");
            cbEditTone_PCMSynthTone_TVF_TVFFilterType.Items.Add("TVF Filter Type LPF2");
            cbEditTone_PCMSynthTone_TVF_TVFFilterType.Items.Add("TVF Filter Type LPF3");


            // Slider for TVF Cutoff Frequency:
            tbEditTone_PCMSynthTone_TVF_TVFCutoffFrequency.Text = "TVF Cutoff Frequency: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFCutoffFrequency).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVF_TVFCutoffFrequency);
            Slider slEditTone_PCMSynthTone_TVF_TVFCutoffFrequency = new Slider();
            slEditTone_PCMSynthTone_TVF_TVFCutoffFrequency.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVF_TVFCutoffFrequency.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVF_TVFCutoffFrequency.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVF_TVFCutoffFrequency.Name = "slEditTone_PCMSynthTone_TVF_TVFCutoffFrequency";
            slEditTone_PCMSynthTone_TVF_TVFCutoffFrequency.Minimum = 0;
            slEditTone_PCMSynthTone_TVF_TVFCutoffFrequency.Maximum = 127;

            // Slider for TVF Resonance:
            tbEditTone_PCMSynthTone_TVF_TVFResonance.Text = "TVF Resonance: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFResonance).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVF_TVFResonance);
            Slider slEditTone_PCMSynthTone_TVF_TVFResonance = new Slider();
            slEditTone_PCMSynthTone_TVF_TVFResonance.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVF_TVFResonance.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVF_TVFResonance.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVF_TVFResonance.Name = "slEditTone_PCMSynthTone_TVF_TVFResonance";
            slEditTone_PCMSynthTone_TVF_TVFResonance.Minimum = 0;
            slEditTone_PCMSynthTone_TVF_TVFResonance.Maximum = 127;

            // Slider for TVF Cutoff Keyfollow:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow);
            Slider slEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow = new Slider();
            slEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow.Name = "slEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow";
            slEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow.Minimum = -200;
            slEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow.Maximum = 200;

            // ComboBox for TVF Cutoff Velocity Curve
            ComboBox cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve = new ComboBox();
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.Name = "cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve";
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.Items.Add("Cutoff Velocity Curve FIXED");
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.Items.Add("Cutoff Velocity Curve 1");
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.Items.Add("Cutoff Velocity Curve 2");
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.Items.Add("Cutoff Velocity Curve 3");
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.Items.Add("Cutoff Velocity Curve 4");
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.Items.Add("Cutoff Velocity Curve 5");
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.Items.Add("Cutoff Velocity Curve 6");
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.Items.Add("Cutoff Velocity Curve 7");


            // Slider for TVF Cutoff Velocity Sens:
            tbEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens.Text = "TVF Cutoff Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFCutoffVelocitySens + 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens);
            Slider slEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens = new Slider();
            slEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens.Name = "slEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens";
            slEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens.Minimum = -63;
            slEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens.Maximum = 63;


            // Slider for TVF Resonance Velocity Sens:
            tbEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens.Text = "TVF Resonance Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFResonanceVelocitySens + 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens);
            Slider slEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens = new Slider();
            slEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens.Name = "slEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens";
            slEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens.Minimum = -63;
            slEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens.Maximum = 63;

            // Put controls in rows 
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch
                , cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch})).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_PCMSynthTone_TVF_TVFFilterType })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMSynthTone_TVF_TVFCutoffFrequency, slEditTone_PCMSynthTone_TVF_TVFCutoffFrequency }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMSynthTone_TVF_TVFResonance, slEditTone_PCMSynthTone_TVF_TVFResonance }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow, slEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens, slEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens, slEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            cbEditTone_PCMSynthTone_TVF_TVFFilterType.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.TVF.TVFFilterType;
            slEditTone_PCMSynthTone_TVF_TVFCutoffFrequency.Value = pCMSynthTone.pCMSynthTonePartial.TVF.TVFCutoffFrequency;
            tbEditTone_PCMSynthTone_TVF_TVFCutoffFrequency.Text = "TVF Cutoff Frequency: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFCutoffFrequency).ToString();
            slEditTone_PCMSynthTone_TVF_TVFResonance.Value = pCMSynthTone.pCMSynthTonePartial.TVF.TVFResonance;
            tbEditTone_PCMSynthTone_TVF_TVFResonance.Text = "TVF Resonance: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFResonance).ToString();
            slEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow.Value = (pCMSynthTone.pCMSynthTonePartial.TVF.TVFCutoffKeyfollow - 64) * 10;
            tbEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow.Text = "TVF Cutoff Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial.TVF.TVFCutoffKeyfollow - 64) * 10).ToString();
            cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.TVF.TVFCutoffVelocityCurve;
            slEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens.Value = pCMSynthTone.pCMSynthTonePartial.TVF.TVFCutoffVelocitySens - 64;
            tbEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens.Text = "TVF Cutoff Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFCutoffVelocitySens - 64).ToString();
            slEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens.Value = pCMSynthTone.pCMSynthTonePartial.TVF.TVFResonanceVelocitySens - 64;
            tbEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens.Text = "TVF Resonance Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFResonanceVelocitySens - 64).ToString();
        }

        private void AddPCMSynthToneTVFEnvControls(byte Partial)
        {
            t.Trace("private void AddPCMSynthToneTVFEnvControls (" + "byte" + Partial + ", " + ")");
            controlsIndex = 0;

            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // Slider for TVF Env Depth:
            tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth.Text = "TVF Env Depth: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvDepth + 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth);
            Slider slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth = new Slider();
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth.Name = "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth";
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth.Minimum = -63;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth.Maximum = 63;

            // ComboBox for TVF Env Velocity Curve
            ComboBox cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve = new ComboBox();
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.Name = "cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve";
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.Items.Add("V-Curve FIXED");
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.Items.Add("V-Curve 1");
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.Items.Add("V-Curve 2");
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.Items.Add("V-Curve 3");
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.Items.Add("V-Curve 4");
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.Items.Add("V-Curve 5");
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.Items.Add("V-Curve 6");
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.Items.Add("V-Curve 7");


            // Slider for TVF Env Velocity Sens:
            tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens.Text = "TVF Env Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvVelocitySens + 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens);
            Slider slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens = new Slider();
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens.Name = "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens";
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens.Minimum = -63;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens.Maximum = 63;

            // Slider for TVF Env Time 1 Velocity Sens:
            tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens.Text = "TVF Env Time 1 Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvTime1VelocitySens + 64).ToString();
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens);
            Slider slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens = new Slider();
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens.Name = "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens";
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens.Minimum = -63;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens.Maximum = 63;

            // Slider for TVF Env Time 4 Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens);
            Slider slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens = new Slider();
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens.Name = "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens";
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens.Minimum = -63;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens.Maximum = 63;

            // Slider for TVF Env Time Keyfollow:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow);
            Slider slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow = new Slider();
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow.Name = "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow";
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow.Minimum = -100;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow.Maximum = 100;

            Slider[] slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime = new Slider[4];
            for (byte i = 0; i < 4; i++)
            {
                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i] = new TextBox();

                // Slider for TVF Env Time:
                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i].Text = "TVF Env Time: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvTime[i]).ToString();
                SetLabelProperties(ref tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i]);
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i] = new Slider();
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i].Name = "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime" + i.ToString();
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i].Minimum = 0;
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i].Maximum = 127;
            }

            Slider[] slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel = new Slider[5];
            for (byte i = 0; i < 5; i++)
            {
                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i] = new TextBox();

                // Slider for TVF Env Level:
                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i].Text = "TVF Env Level: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvLevel[i]).ToString();
                SetLabelProperties(ref tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i]);
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i] = new Slider();
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i].Name = "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel" + i.ToString();
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i].Minimum = 0;
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i].Maximum = 127;
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve, cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch
                , cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch}, new byte[] { 1, 2, 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth, slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens, slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens, slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens, slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow, slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow }, new byte[] { 1, 2 })).Row);
            for (byte i = 0; i < 4; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(6 + i), new Control[] { tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i], slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i] }, new byte[] { 1, 2 })).Row);
            }
            for (byte i = 0; i < 5; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(10 + i), new Control[] { tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i], slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i] }, new byte[] { 1, 2 })).Row);
            }

            // Set values
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth.Value = pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvDepth - 64;
            tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth.Text = "TVF Env Depth: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvDepth - 64).ToString();
            cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvVelocityCurve;
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens.Value = pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvVelocitySens - 64;
            tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens.Text = "TVF Env Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvVelocitySens - 64).ToString();
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens.Value = pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvTime1VelocitySens - 64;
            tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens.Text = "Time 1 Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvTime1VelocitySens - 64).ToString();
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens.Value = pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvTime4VelocitySens - 64;
            tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens.Text = "Time 4 Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvTime4VelocitySens - 64).ToString();
            slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow.Value = ((pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvTimeKeyfollow - 64) * 10);
            tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow.Text = "Time Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvTimeKeyfollow - 64) * 10).ToString();
            for (byte i = 0; i < 4; i++)
            {
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i].Value = (pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvTime[i]);
                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[i].Text = "Time " + (i + 1).ToString() + ": " + ((pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvTime[i])).ToString();
            }
            for (byte i = 0; i < 5; i++)
            {
                slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i].Value = (pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvLevel[i]);
                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[i].Text = "Level " + i.ToString() + ": " + ((pCMSynthTone.pCMSynthTonePartial.TVF.TVFEnvLevel[i])).ToString();
            }
        }

        private void AddPCMSynthToneTVAControls(byte Partial)
        {
            t.Trace("private void AddPCMSynthToneTVAControls (" + "byte" + Partial + ", " + ")");
            controlsIndex = 0;

            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // Slider for Partial Output Level:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVA_PartialOutputLevel);
            Slider slEditTone_PCMSynthTone_TVA_PartialOutputLevel = new Slider();
            slEditTone_PCMSynthTone_TVA_PartialOutputLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVA_PartialOutputLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVA_PartialOutputLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVA_PartialOutputLevel.Name = "slEditTone_PCMSynthTone_TVA_PartialOutputLevel";
            slEditTone_PCMSynthTone_TVA_PartialOutputLevel.Minimum = 0;
            slEditTone_PCMSynthTone_TVA_PartialOutputLevel.Maximum = 127;

            // ComboBox for TVA Level Velocity Curve
            ComboBox cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve = new ComboBox();
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.Name = "cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve";
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.Items.Add("Lvl V-Curve FIXED");
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.Items.Add("Lvl V-Curve 1");
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.Items.Add("Lvl V-Curve 2");
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.Items.Add("Lvl V-Curve 3");
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.Items.Add("Lvl V-Curve 4");
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.Items.Add("Lvl V-Curve 5");
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.Items.Add("Lvl V-Curve 6");
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.Items.Add("Lvl V-Curve 7");


            // Slider for TVA Level Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVA_TVALevelVelocitySens);
            Slider slEditTone_PCMSynthTone_TVA_TVALevelVelocitySens = new Slider();
            slEditTone_PCMSynthTone_TVA_TVALevelVelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVA_TVALevelVelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVA_TVALevelVelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVA_TVALevelVelocitySens.Name = "slEditTone_PCMSynthTone_TVA_TVALevelVelocitySens";
            slEditTone_PCMSynthTone_TVA_TVALevelVelocitySens.Minimum = -63;
            slEditTone_PCMSynthTone_TVA_TVALevelVelocitySens.Maximum = 63;


            // Slider for Bias Level:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVA_BiasLevel);
            Slider slEditTone_PCMSynthTone_TVA_BiasLevel = new Slider();
            slEditTone_PCMSynthTone_TVA_BiasLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVA_BiasLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVA_BiasLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVA_BiasLevel.Name = "slEditTone_PCMSynthTone_TVA_BiasLevel";
            slEditTone_PCMSynthTone_TVA_BiasLevel.Minimum = -100;
            slEditTone_PCMSynthTone_TVA_BiasLevel.Maximum = 100;


            // Slider for Bias Position:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVA_BiasPosition);
            Slider slEditTone_PCMSynthTone_TVA_BiasPosition = new Slider();
            slEditTone_PCMSynthTone_TVA_BiasPosition.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVA_BiasPosition.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVA_BiasPosition.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVA_BiasPosition.Name = "slEditTone_PCMSynthTone_TVA_BiasPosition";
            slEditTone_PCMSynthTone_TVA_BiasPosition.Minimum = 0;
            slEditTone_PCMSynthTone_TVA_BiasPosition.Maximum = 127;

            // ComboBox for Bias Direction
            ComboBox cbEditTone_PCMSynthTone_TVA_BiasDirection = new ComboBox();
            cbEditTone_PCMSynthTone_TVA_BiasDirection.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_TVA_BiasDirection.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_TVA_BiasDirection.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_TVA_BiasDirection.Name = "cbEditTone_PCMSynthTone_TVA_BiasDirection";
            cbEditTone_PCMSynthTone_TVA_BiasDirection.Items.Add("Direction: LOWER");
            cbEditTone_PCMSynthTone_TVA_BiasDirection.Items.Add("Direction: UPPER");
            cbEditTone_PCMSynthTone_TVA_BiasDirection.Items.Add("Direction: LOWER&UPPER");
            cbEditTone_PCMSynthTone_TVA_BiasDirection.Items.Add("Direction: ALL");


            // Slider for Partial Pan:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVA_PartialPan);
            Slider slEditTone_PCMSynthTone_TVA_PartialPan = new Slider();
            slEditTone_PCMSynthTone_TVA_PartialPan.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVA_PartialPan.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVA_PartialPan.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVA_PartialPan.Name = "slEditTone_PCMSynthTone_TVA_PartialPan";
            slEditTone_PCMSynthTone_TVA_PartialPan.Minimum = -64;
            slEditTone_PCMSynthTone_TVA_PartialPan.Maximum = 63;


            // Slider for Partial Pan Keyfollow:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVA_PartialPanKeyfollow);
            Slider slEditTone_PCMSynthTone_TVA_PartialPanKeyfollow = new Slider();
            slEditTone_PCMSynthTone_TVA_PartialPanKeyfollow.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVA_PartialPanKeyfollow.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVA_PartialPanKeyfollow.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVA_PartialPanKeyfollow.Name = "slEditTone_PCMSynthTone_TVA_PartialPanKeyfollow";
            slEditTone_PCMSynthTone_TVA_PartialPanKeyfollow.Minimum = -100;
            slEditTone_PCMSynthTone_TVA_PartialPanKeyfollow.Maximum = 100;


            // Slider for Partial Random Pan Depth:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVA_PartialRandomPanDepth);
            Slider slEditTone_PCMSynthTone_TVA_PartialRandomPanDepth = new Slider();
            slEditTone_PCMSynthTone_TVA_PartialRandomPanDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVA_PartialRandomPanDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVA_PartialRandomPanDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVA_PartialRandomPanDepth.Name = "slEditTone_PCMSynthTone_TVA_PartialRandomPanDepth";
            slEditTone_PCMSynthTone_TVA_PartialRandomPanDepth.Minimum = 0;
            slEditTone_PCMSynthTone_TVA_PartialRandomPanDepth.Maximum = 63;


            // Slider for Partial Alternate Pan Depth:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth);
            Slider slEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth = new Slider();
            slEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.Name = "slEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth";
            slEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.Minimum = -63;
            slEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.Maximum = 63;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch,
                cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch}, new byte[] { 1, 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMSynthTone_TVA_PartialOutputLevel, slEditTone_PCMSynthTone_TVA_PartialOutputLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMSynthTone_TVA_TVALevelVelocitySens, slEditTone_PCMSynthTone_TVA_TVALevelVelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_PCMSynthTone_TVA_BiasLevel, slEditTone_PCMSynthTone_TVA_BiasLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_PCMSynthTone_TVA_BiasPosition, slEditTone_PCMSynthTone_TVA_BiasPosition }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { cbEditTone_PCMSynthTone_TVA_BiasDirection })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_PCMSynthTone_TVA_PartialPan, slEditTone_PCMSynthTone_TVA_PartialPan }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_PCMSynthTone_TVA_PartialPanKeyfollow, slEditTone_PCMSynthTone_TVA_PartialPanKeyfollow }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { tbEditTone_PCMSynthTone_TVA_PartialRandomPanDepth, slEditTone_PCMSynthTone_TVA_PartialRandomPanDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { tbEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth, slEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth }, new byte[] { 1, 2 })).Row);

            // Set values
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            slEditTone_PCMSynthTone_TVA_PartialOutputLevel.Value = (pCMSynthTone.pCMSynthTonePartial.PartialOutputLevel);
            tbEditTone_PCMSynthTone_TVA_PartialOutputLevel.Text = "Partial Output Level: " + ((pCMSynthTone.pCMSynthTonePartial.PartialOutputLevel)).ToString();
            cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.TVA.TVALevelVelocityCurve;
            slEditTone_PCMSynthTone_TVA_TVALevelVelocitySens.Value = (pCMSynthTone.pCMSynthTonePartial.TVA.TVALevelVelocitySens - 64);
            tbEditTone_PCMSynthTone_TVA_TVALevelVelocitySens.Text = "TVA Level Velocity Sens: " + ((pCMSynthTone.pCMSynthTonePartial.TVA.TVALevelVelocitySens - 64)).ToString();
            slEditTone_PCMSynthTone_TVA_BiasLevel.Value = (pCMSynthTone.pCMSynthTonePartial.BiasLevel - 64) * 10;
            tbEditTone_PCMSynthTone_TVA_BiasLevel.Text = "Bias Level: " + ((pCMSynthTone.pCMSynthTonePartial.BiasLevel - 64) * 10).ToString();
            slEditTone_PCMSynthTone_TVA_BiasPosition.Value = (pCMSynthTone.pCMSynthTonePartial.BiasPosition);
            tbEditTone_PCMSynthTone_TVA_BiasPosition.Text = "Bias Position: " + keyNames[((pCMSynthTone.pCMSynthTonePartial.BiasPosition))];
            cbEditTone_PCMSynthTone_TVA_BiasDirection.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.BiasDirection;
            slEditTone_PCMSynthTone_TVA_PartialPan.Value = (pCMSynthTone.pCMSynthTonePartial.PartialPan - 64);
            if (pCMSynthTone.pCMSynthTonePartial.PartialPan < 64)
            {
                tbEditTone_PCMSynthTone_TVA_PartialPan.Text = "Partial Pan: L" + (Math.Abs(pCMSynthTone.pCMSynthTonePartial.PartialPan - 64)).ToString();
            }
            else if (pCMSynthTone.pCMSynthTonePartial.PartialPan > 64)
            {
                tbEditTone_PCMSynthTone_TVA_PartialPan.Text = "Partial Pan: R" + ((pCMSynthTone.pCMSynthTonePartial.PartialPan - 64)).ToString();
            }
            else
            {
                tbEditTone_PCMSynthTone_TVA_PartialPan.Text = "Partial Pan: Center";
            }
            slEditTone_PCMSynthTone_TVA_PartialPanKeyfollow.Value = (pCMSynthTone.pCMSynthTonePartial.PartialPanKeyfollow - 64) * 10;
            tbEditTone_PCMSynthTone_TVA_PartialPanKeyfollow.Text = "Pan Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial.PartialPanKeyfollow - 64) * 10).ToString();
            slEditTone_PCMSynthTone_TVA_PartialRandomPanDepth.Value = (pCMSynthTone.pCMSynthTonePartial.PartialRandomPanDepth);
            tbEditTone_PCMSynthTone_TVA_PartialRandomPanDepth.Text = "Random Pan Depth: " + ((pCMSynthTone.pCMSynthTonePartial.PartialRandomPanDepth)).ToString();

            slEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.Value = (pCMSynthTone.pCMSynthTonePartial.PartialAlternatePanDepth - 64);
            if (pCMSynthTone.pCMSynthTonePartial.PartialAlternatePanDepth < 64)
            {
                tbEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.Text = "Alt Pan Depth: L" + (Math.Abs(pCMSynthTone.pCMSynthTonePartial.PartialAlternatePanDepth - 64)).ToString();
            }
            else if (pCMSynthTone.pCMSynthTonePartial.PartialAlternatePanDepth > 64)
            {
                tbEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.Text = "Alt Pan Depth: R" + ((pCMSynthTone.pCMSynthTonePartial.PartialAlternatePanDepth - 64)).ToString();
            }
            else
            {
                tbEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.Text = "Alternate Pan Depth: Center";
            }
        }

        private void AddPCMSynthToneTVAEnvControls(byte Partial)
        {
            t.Trace("private void AddPCMSynthToneTVAEnvControls (" + "byte" + Partial + ", " + ")");
            controlsIndex = 0;

            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // Slider for TVA Env Time 1 Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens);
            Slider slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens = new Slider();
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens.Name = "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens";
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens.Minimum = -63;
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens.Maximum = 63;

            // Slider for TVA Env Time 4 Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens);
            Slider slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens = new Slider();
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens.Name = "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens";
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens.Minimum = -63;
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens.Maximum = 63;

            // Slider for TVA Env Time Keyfollow:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow);
            Slider slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow = new Slider();
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow.Name = "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow";
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow.Minimum = -100;
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow.Maximum = 100;

            Slider[] slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime = new Slider[4];
            for (byte i = 0; i < 4; i++)
            {

                // Slider for TVA Env Time:
                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i]);
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i] = new Slider();
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i].Name = "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime" + (i + 1).ToString();
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i].Minimum = 0;
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i].Maximum = 127;
            }

            // Slider for TVA Env Level:
            Slider[] slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel = new Slider[3];
            for (byte i = 0; i < 3; i++)
            {
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i] = new Slider();
                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i]);
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i].Name = "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel" + (i + 1).ToString();
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i].Minimum = 0;
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i].Maximum = 127;
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch,
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch}, new byte[] { 1, 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens, slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens, slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow, slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow }, new byte[] { 1, 2 })).Row);
            for (byte i = 0; i < 4; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(4 + i), new Control[] { tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i], slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i] }, new byte[] { 1, 2 })).Row);
            }
            for (byte i = 0; i < 3; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(8 + i), new Control[] { tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i], slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i] }, new byte[] { 1, 2 })).Row);
            }

            // Set values
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens.Value = (pCMSynthTone.pCMSynthTonePartial.TVA.TVAEnvTime1VelocitySens - 64);
            tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens.Text = "TVA Env Time 1 Velocity Sens: " + ((pCMSynthTone.pCMSynthTonePartial.TVA.TVAEnvTime1VelocitySens - 64)).ToString();
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens.Value = (pCMSynthTone.pCMSynthTonePartial.TVA.TVAEnvTime4VelocitySens - 64);
            tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens.Text = "TVA Env Time 4 Velocity Sens: " + ((pCMSynthTone.pCMSynthTonePartial.TVA.TVAEnvTime4VelocitySens - 64)).ToString();
            slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow.Value = (pCMSynthTone.pCMSynthTonePartial.TVA.TVAEnvTimeKeyfollow - 64) * 10;
            tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow.Text = "TVA Env Time Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial.TVA.TVAEnvTimeKeyfollow - 64) * 10).ToString();
            for (byte i = 0; i < 4; i++)
            {
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i].Value = (pCMSynthTone.pCMSynthTonePartial.TVA.TVAEnvTime[i]);
                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[i].Text = "TVA Env Time " + (i + 1).ToString() + ": " + ((pCMSynthTone.pCMSynthTonePartial.TVA.TVAEnvTime[i])).ToString();
            }
            for (byte i = 0; i < 3; i++)
            {
                slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i].Value = (pCMSynthTone.pCMSynthTonePartial.TVA.TVAEnvLevel[i]);
                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[i].Text = "TVA Env Level " + (i + 1).ToString() + ": " + ((pCMSynthTone.pCMSynthTonePartial.TVA.TVAEnvLevel[i])).ToString();
            }
        }

        private void AddPCMSynthToneOutputControls(byte Partial)
        {
            t.Trace("private void AddPCMSynthToneOutputControls (" + "byte" + Partial + ", " + ")");
            controlsIndex = 0;

            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";


            // Slider for Partial Output Level:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Output_PartialOutputLevel);
            Slider slEditTone_PCMSynthTone_Output_PartialOutputLevel = new Slider();
            slEditTone_PCMSynthTone_Output_PartialOutputLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Output_PartialOutputLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Output_PartialOutputLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Output_PartialOutputLevel.Name = "slEditTone_PCMSynthTone_Output_PartialOutputLevel";
            slEditTone_PCMSynthTone_Output_PartialOutputLevel.Minimum = 0;
            slEditTone_PCMSynthTone_Output_PartialOutputLevel.Maximum = 127;


            // Slider for Partial Chorus Send Level:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Output_PartialChorusSendLevel);
            Slider slEditTone_PCMSynthTone_Output_PartialChorusSendLevel = new Slider();
            slEditTone_PCMSynthTone_Output_PartialChorusSendLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Output_PartialChorusSendLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Output_PartialChorusSendLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Output_PartialChorusSendLevel.Name = "slEditTone_PCMSynthTone_Output_PartialChorusSendLevel";
            slEditTone_PCMSynthTone_Output_PartialChorusSendLevel.Minimum = 0;
            slEditTone_PCMSynthTone_Output_PartialChorusSendLevel.Maximum = 127;

            // Slider for Partial Reverb Send Level:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_Output_PartialReverbSendLevel);
            Slider slEditTone_PCMSynthTone_Output_PartialReverbSendLevel = new Slider();
            slEditTone_PCMSynthTone_Output_PartialReverbSendLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_Output_PartialReverbSendLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_Output_PartialReverbSendLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_Output_PartialReverbSendLevel.Name = "slEditTone_PCMSynthTone_Output_PartialReverbSendLevel";
            slEditTone_PCMSynthTone_Output_PartialReverbSendLevel.Minimum = 0;
            slEditTone_PCMSynthTone_Output_PartialReverbSendLevel.Maximum = 127;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch,
                cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch}, new byte[] { 1, 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMSynthTone_Output_PartialOutputLevel, slEditTone_PCMSynthTone_Output_PartialOutputLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMSynthTone_Output_PartialChorusSendLevel, slEditTone_PCMSynthTone_Output_PartialChorusSendLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMSynthTone_Output_PartialReverbSendLevel, slEditTone_PCMSynthTone_Output_PartialReverbSendLevel }, new byte[] { 1, 2 })).Row);

            // Set values
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            slEditTone_PCMSynthTone_Output_PartialOutputLevel.Value = (pCMSynthTone.pCMSynthTonePartial.PartialOutputLevel);
            tbEditTone_PCMSynthTone_Output_PartialOutputLevel.Text = "Partial Output Level: " + ((pCMSynthTone.pCMSynthTonePartial.PartialOutputLevel)).ToString();
            slEditTone_PCMSynthTone_Output_PartialChorusSendLevel.Value = (pCMSynthTone.pCMSynthTonePartial.PartialChorusSendLevel);
            tbEditTone_PCMSynthTone_Output_PartialChorusSendLevel.Text = "Partial Chorus Send: " + ((pCMSynthTone.pCMSynthTonePartial.PartialChorusSendLevel)).ToString();
            slEditTone_PCMSynthTone_Output_PartialReverbSendLevel.Value = (pCMSynthTone.pCMSynthTonePartial.PartialReverbSendLevel);
            tbEditTone_PCMSynthTone_Output_PartialReverbSendLevel.Text = "Partial Reverb Send: " + ((pCMSynthTone.pCMSynthTonePartial.PartialReverbSendLevel)).ToString();
        }

        private void AddPCMSynthToneLFO01Controls(byte Partial)
        {
            t.Trace("private void AddPCMSynthToneLFO01Controls (" + "byte" + Partial + ", " + ")");
            controlsIndex = 0;

            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // ComboBox for LFO1 Waveform
            ComboBox cbEditTone_PCMSynthTone_LFO1_LFO1Waveform = new ComboBox();
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Name = "cbEditTone_PCMSynthTone_LFO1_LFO1Waveform";
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Sinus");
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Triangle");
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Saw up");
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Saw down");
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Square");
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Random");
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Bend up");
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Bend down");
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Trapezoidal");
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Sample & hold");
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Chaos");
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Varying sinus");
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.Items.Add("Step");

            // Slider for LFO Rate:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO1_LFORate);
            Slider slEditTone_PCMSynthTone_LFO1_LFORate = new Slider();
            slEditTone_PCMSynthTone_LFO1_LFORate.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO1_LFORate.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO1_LFORate.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO1_LFORate.Name = "slEditTone_PCMSynthTone_LFO1_LFORate";
            slEditTone_PCMSynthTone_LFO1_LFORate.Minimum = 0;
            slEditTone_PCMSynthTone_LFO1_LFORate.Maximum = 149;

            // Slider for LFO Rate Detune:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO1_LFORateDetune);
            Slider slEditTone_PCMSynthTone_LFO1_LFORateDetune = new Slider();
            slEditTone_PCMSynthTone_LFO1_LFORateDetune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO1_LFORateDetune.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO1_LFORateDetune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO1_LFORateDetune.Name = "slEditTone_PCMSynthTone_LFO1_LFORateDetune";
            slEditTone_PCMSynthTone_LFO1_LFORateDetune.Minimum = 0;
            slEditTone_PCMSynthTone_LFO1_LFORateDetune.Maximum = 127;

            // ComboBox for LFO Offset
            ComboBox cbEditTone_PCMSynthTone_LFO1_LFOOffset = new ComboBox();
            cbEditTone_PCMSynthTone_LFO1_LFOOffset.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_LFO1_LFOOffset.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_LFO1_LFOOffset.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_LFO1_LFOOffset.Name = "cbEditTone_PCMSynthTone_LFO1_LFOOffset";
            cbEditTone_PCMSynthTone_LFO1_LFOOffset.Items.Add("Offset -100");
            cbEditTone_PCMSynthTone_LFO1_LFOOffset.Items.Add("Offset -50");
            cbEditTone_PCMSynthTone_LFO1_LFOOffset.Items.Add("Offset 0");
            cbEditTone_PCMSynthTone_LFO1_LFOOffset.Items.Add("Offset 50");
            cbEditTone_PCMSynthTone_LFO1_LFOOffset.Items.Add("Offset 100");


            // Slider for LFO Delay Time:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO1_LFODelayTime);
            Slider slEditTone_PCMSynthTone_LFO1_LFODelayTime = new Slider();
            slEditTone_PCMSynthTone_LFO1_LFODelayTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO1_LFODelayTime.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO1_LFODelayTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO1_LFODelayTime.Name = "slEditTone_PCMSynthTone_LFO1_LFODelayTime";
            slEditTone_PCMSynthTone_LFO1_LFODelayTime.Minimum = 0;
            slEditTone_PCMSynthTone_LFO1_LFODelayTime.Maximum = 127;


            // Slider for LFO Delay Time Keyfollow:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow);
            Slider slEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow = new Slider();
            slEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow.Name = "slEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow";
            slEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow.Minimum = -100;
            slEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow.Maximum = 100;

            // ComboBox for LFO Fade Mode
            ComboBox cbEditTone_PCMSynthTone_LFO1_LFOFadeMode = new ComboBox();
            cbEditTone_PCMSynthTone_LFO1_LFOFadeMode.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_LFO1_LFOFadeMode.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_LFO1_LFOFadeMode.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_LFO1_LFOFadeMode.Name = "cbEditTone_PCMSynthTone_LFO1_LFOFadeMode";
            cbEditTone_PCMSynthTone_LFO1_LFOFadeMode.Items.Add("Fade mode: On in");
            cbEditTone_PCMSynthTone_LFO1_LFOFadeMode.Items.Add("Fade mode: On out");
            cbEditTone_PCMSynthTone_LFO1_LFOFadeMode.Items.Add("Fade mode: Off in");
            cbEditTone_PCMSynthTone_LFO1_LFOFadeMode.Items.Add("Fade mode: Off out");


            // Slider for LFO Fade Time:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO1_LFOFadeTime);
            Slider slEditTone_PCMSynthTone_LFO1_LFOFadeTime = new Slider();
            slEditTone_PCMSynthTone_LFO1_LFOFadeTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO1_LFOFadeTime.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO1_LFOFadeTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO1_LFOFadeTime.Name = "slEditTone_PCMSynthTone_LFO1_LFOFadeTime";
            slEditTone_PCMSynthTone_LFO1_LFOFadeTime.Minimum = 0;
            slEditTone_PCMSynthTone_LFO1_LFOFadeTime.Maximum = 127;

            // CheckBox for LFO Key Trigger
            CheckBox cbEditTone_PCMSynthTone_LFO1_LFOKeyTrigger = new CheckBox();
            cbEditTone_PCMSynthTone_LFO1_LFOKeyTrigger.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_LFO1_LFOKeyTrigger.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_LFO1_LFOKeyTrigger.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_LFO1_LFOKeyTrigger.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_LFO1_LFOKeyTrigger.Content = "LFO Key Trigger";
            cbEditTone_PCMSynthTone_LFO1_LFOKeyTrigger.Name = "cbEditTone_PCMSynthTone_LFO1KeyTrigger";

            // Slider for LFO Pitch Depth:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO1_LFOPitchDepth);
            Slider slEditTone_PCMSynthTone_LFO1_LFOPitchDepth = new Slider();
            slEditTone_PCMSynthTone_LFO1_LFOPitchDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO1_LFOPitchDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO1_LFOPitchDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO1_LFOPitchDepth.Name = "slEditTone_PCMSynthTone_LFO1_LFOPitchDepth";
            slEditTone_PCMSynthTone_LFO1_LFOPitchDepth.Minimum = -63;
            slEditTone_PCMSynthTone_LFO1_LFOPitchDepth.Maximum = 63;

            // Slider for LFO TVF Depth:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO1_LFOTVFDepth);
            Slider slEditTone_PCMSynthTone_LFO1_LFOTVFDepth = new Slider();
            slEditTone_PCMSynthTone_LFO1_LFOTVFDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO1_LFOTVFDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO1_LFOTVFDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO1_LFOTVFDepth.Name = "slEditTone_PCMSynthTone_LFO1_LFOTVFDepth";
            slEditTone_PCMSynthTone_LFO1_LFOTVFDepth.Minimum = 0;
            slEditTone_PCMSynthTone_LFO1_LFOTVFDepth.Maximum = 127;

            // Slider for LFO TVA Depth:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO1_LFOTVADepth);
            Slider slEditTone_PCMSynthTone_LFO1_LFOTVADepth = new Slider();
            slEditTone_PCMSynthTone_LFO1_LFOTVADepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO1_LFOTVADepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO1_LFOTVADepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO1_LFOTVADepth.Name = "slEditTone_PCMSynthTone_LFO1_LFOTVADepth";
            slEditTone_PCMSynthTone_LFO1_LFOTVADepth.Minimum = 0;
            slEditTone_PCMSynthTone_LFO1_LFOTVADepth.Maximum = 127;

            // Slider for LFO Pan Depth:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO1_LFOPanDepth);
            Slider slEditTone_PCMSynthTone_LFO1_LFOPanDepth = new Slider();
            slEditTone_PCMSynthTone_LFO1_LFOPanDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO1_LFOPanDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO1_LFOPanDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO1_LFOPanDepth.Name = "slEditTone_PCMSynthTone_LFO1_LFOPanDepth";
            slEditTone_PCMSynthTone_LFO1_LFOPanDepth.Minimum = 0;
            slEditTone_PCMSynthTone_LFO1_LFOPanDepth.Maximum = 127;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch,
                cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch}, new byte[] { 1, 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_PCMSynthTone_LFO1_LFO1Waveform })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMSynthTone_LFO1_LFORate, slEditTone_PCMSynthTone_LFO1_LFORate }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMSynthTone_LFO1_LFORateDetune, slEditTone_PCMSynthTone_LFO1_LFORateDetune }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { cbEditTone_PCMSynthTone_LFO1_LFOOffset })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_PCMSynthTone_LFO1_LFODelayTime, slEditTone_PCMSynthTone_LFO1_LFODelayTime }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow, slEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { cbEditTone_PCMSynthTone_LFO1_LFOFadeMode })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_PCMSynthTone_LFO1_LFOFadeTime, slEditTone_PCMSynthTone_LFO1_LFOFadeTime }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { cbEditTone_PCMSynthTone_LFO1_LFOKeyTrigger })).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { tbEditTone_PCMSynthTone_LFO1_LFOPitchDepth, slEditTone_PCMSynthTone_LFO1_LFOPitchDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(11, new Control[] { tbEditTone_PCMSynthTone_LFO1_LFOTVFDepth, slEditTone_PCMSynthTone_LFO1_LFOTVFDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(12, new Control[] { tbEditTone_PCMSynthTone_LFO1_LFOTVADepth, slEditTone_PCMSynthTone_LFO1_LFOTVADepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(13, new Control[] { tbEditTone_PCMSynthTone_LFO1_LFOPanDepth, slEditTone_PCMSynthTone_LFO1_LFOPanDepth }, new byte[] { 1, 2 })).Row);

            // Set values
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            cbEditTone_PCMSynthTone_LFO1_LFO1Waveform.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.LFO1.LFOWaveform;
            slEditTone_PCMSynthTone_LFO1_LFORate.Value = (pCMSynthTone.pCMSynthTonePartial.LFO1.LFORate);
            if (pCMSynthTone.pCMSynthTonePartial.LFO1.LFORate < 128)
            {
                tbEditTone_PCMSynthTone_LFO1_LFORate.Text = "LFO Rate: " + ((pCMSynthTone.pCMSynthTonePartial.LFO1.LFORate)).ToString();
            }
            else
            {
                tbEditTone_PCMSynthTone_LFO1_LFORate.Text = "LFO Rate: " + toneLengths[pCMSynthTone.pCMSynthTonePartial.LFO1.LFORate - 128];
            }
            slEditTone_PCMSynthTone_LFO1_LFORateDetune.Value = (pCMSynthTone.pCMSynthTonePartial.LFO1.LFORateDetune);
            tbEditTone_PCMSynthTone_LFO1_LFORateDetune.Text = "Rate Detune: " + ((pCMSynthTone.pCMSynthTonePartial.LFO1.LFORateDetune)).ToString();
            cbEditTone_PCMSynthTone_LFO1_LFOOffset.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.LFO1.LFOOffset;
            slEditTone_PCMSynthTone_LFO1_LFODelayTime.Value = (pCMSynthTone.pCMSynthTonePartial.LFO1.LFODelayTime);
            tbEditTone_PCMSynthTone_LFO1_LFODelayTime.Text = "Delay Time: " + ((pCMSynthTone.pCMSynthTonePartial.LFO1.LFODelayTime)).ToString();
            slEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow.Value = (pCMSynthTone.pCMSynthTonePartial.LFO1.LFODelayTimeKeyfollow - 64) * 10;
            tbEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow.Text = "Del-T Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial.LFO1.LFODelayTimeKeyfollow - 64) * 10).ToString();
            cbEditTone_PCMSynthTone_LFO1_LFOFadeMode.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.LFO1.LFOFadeMode;
            slEditTone_PCMSynthTone_LFO1_LFOFadeTime.Value = (pCMSynthTone.pCMSynthTonePartial.LFO1.LFOFadeTime);
            tbEditTone_PCMSynthTone_LFO1_LFOFadeTime.Text = "LFO Fade Time: " + ((pCMSynthTone.pCMSynthTonePartial.LFO1.LFOFadeTime)).ToString();
            cbEditTone_PCMSynthTone_LFO1_LFOKeyTrigger.IsChecked = pCMSynthTone.pCMSynthTonePartial.LFO1.LFOKeyTrigger;
            slEditTone_PCMSynthTone_LFO1_LFOTVFDepth.Value = (pCMSynthTone.pCMSynthTonePartial.LFO1.LFOTVFDepth - 64);
            tbEditTone_PCMSynthTone_LFO1_LFOTVFDepth.Text = "LFO TVF Depth: " + ((pCMSynthTone.pCMSynthTonePartial.LFO1.LFOTVFDepth - 64)).ToString();
            slEditTone_PCMSynthTone_LFO1_LFOTVADepth.Value = (pCMSynthTone.pCMSynthTonePartial.LFO1.LFOTVADepth - 64);
            tbEditTone_PCMSynthTone_LFO1_LFOTVADepth.Text = "LFO TVA Depth: " + ((pCMSynthTone.pCMSynthTonePartial.LFO1.LFOTVADepth - 64)).ToString();
            slEditTone_PCMSynthTone_LFO1_LFOPanDepth.Value = (pCMSynthTone.pCMSynthTonePartial.LFO1.LFOPanDepth - 64);
            tbEditTone_PCMSynthTone_LFO1_LFOPanDepth.Text = "LFO Pan Depth: " + ((pCMSynthTone.pCMSynthTonePartial.LFO1.LFOPanDepth - 64)).ToString();
            slEditTone_PCMSynthTone_LFO1_LFOPitchDepth.Value = (pCMSynthTone.pCMSynthTonePartial.LFO1.LFOPitchDepth - 64);
            tbEditTone_PCMSynthTone_LFO1_LFOPitchDepth.Text = "LFO Pitch Depth: " + ((pCMSynthTone.pCMSynthTonePartial.LFO1.LFOPitchDepth - 64)).ToString();
        }

        private void AddPCMSynthToneLFO02Controls(byte Partial)
        {
            t.Trace("private void AddPCMSynthToneLFO02Controls (" + "byte" + Partial + ", " + ")");
            controlsIndex = 0;

            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // ComboBox for LFO2 Waveform
            ComboBox cbEditTone_PCMSynthTone_LFO2_LFO2Waveform = new ComboBox();
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Name = "cbEditTone_PCMSynthTone_LFO2_LFO2Waveform";
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Sinus");
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Triangle");
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Saw up");
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Saw down");
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Square");
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Random");
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Bend up");
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Bend down");
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Trapezoidal");
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Sample & hold");
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Chaos");
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Varying sinus");
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.Items.Add("Step");

            // Slider for LFO Rate:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO2_LFORate);
            Slider slEditTone_PCMSynthTone_LFO2_LFORate = new Slider();
            slEditTone_PCMSynthTone_LFO2_LFORate.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO2_LFORate.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO2_LFORate.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO2_LFORate.Name = "slEditTone_PCMSynthTone_LFO2_LFORate";
            slEditTone_PCMSynthTone_LFO2_LFORate.Minimum = 0;
            slEditTone_PCMSynthTone_LFO2_LFORate.Maximum = 149;

            // Slider for LFO Rate Detune:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO2_LFORateDetune);
            Slider slEditTone_PCMSynthTone_LFO2_LFORateDetune = new Slider();
            slEditTone_PCMSynthTone_LFO2_LFORateDetune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO2_LFORateDetune.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO2_LFORateDetune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO2_LFORateDetune.Name = "slEditTone_PCMSynthTone_LFO2_LFORateDetune";
            slEditTone_PCMSynthTone_LFO2_LFORateDetune.Minimum = 0;
            slEditTone_PCMSynthTone_LFO2_LFORateDetune.Maximum = 127;

            // ComboBox for LFO Offset
            ComboBox cbEditTone_PCMSynthTone_LFO2_LFOOffset = new ComboBox();
            cbEditTone_PCMSynthTone_LFO2_LFOOffset.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_LFO2_LFOOffset.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_LFO2_LFOOffset.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_LFO2_LFOOffset.Name = "cbEditTone_PCMSynthTone_LFO2_LFOOffset";
            cbEditTone_PCMSynthTone_LFO2_LFOOffset.Items.Add("Offset -100");
            cbEditTone_PCMSynthTone_LFO2_LFOOffset.Items.Add("Offset -50");
            cbEditTone_PCMSynthTone_LFO2_LFOOffset.Items.Add("Offset 0");
            cbEditTone_PCMSynthTone_LFO2_LFOOffset.Items.Add("Offset 50");
            cbEditTone_PCMSynthTone_LFO2_LFOOffset.Items.Add("Offset 100");

            // Slider for LFO Delay Time:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO2_LFODelayTime);
            Slider slEditTone_PCMSynthTone_LFO2_LFODelayTime = new Slider();
            slEditTone_PCMSynthTone_LFO2_LFODelayTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO2_LFODelayTime.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO2_LFODelayTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO2_LFODelayTime.Name = "slEditTone_PCMSynthTone_LFO2_LFODelayTime";
            slEditTone_PCMSynthTone_LFO2_LFODelayTime.Minimum = 0;
            slEditTone_PCMSynthTone_LFO2_LFODelayTime.Maximum = 127;

            // Slider for LFO Delay Time Keyfollow:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow);
            Slider slEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow = new Slider();
            slEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow.Name = "slEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow";
            slEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow.Minimum = -100;
            slEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow.Maximum = 100;

            // ComboBox for LFO Fade Mode
            ComboBox cbEditTone_PCMSynthTone_LFO2_LFOFadeMode = new ComboBox();
            cbEditTone_PCMSynthTone_LFO2_LFOFadeMode.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_LFO2_LFOFadeMode.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_LFO2_LFOFadeMode.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_LFO2_LFOFadeMode.Name = "cbEditTone_PCMSynthTone_LFO2_LFOFadeMode";
            cbEditTone_PCMSynthTone_LFO2_LFOFadeMode.Items.Add("Fade mode: On in");
            cbEditTone_PCMSynthTone_LFO2_LFOFadeMode.Items.Add("Fade mode: On out");
            cbEditTone_PCMSynthTone_LFO2_LFOFadeMode.Items.Add("Fade mode: Off in");
            cbEditTone_PCMSynthTone_LFO2_LFOFadeMode.Items.Add("Fade mode: Off out");

            // Slider for LFO Fade Time:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO2_LFOFadeTime);
            Slider slEditTone_PCMSynthTone_LFO2_LFOFadeTime = new Slider();
            slEditTone_PCMSynthTone_LFO2_LFOFadeTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO2_LFOFadeTime.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO2_LFOFadeTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO2_LFOFadeTime.Name = "slEditTone_PCMSynthTone_LFO2_LFOFadeTime";
            slEditTone_PCMSynthTone_LFO2_LFOFadeTime.Minimum = 0;
            slEditTone_PCMSynthTone_LFO2_LFOFadeTime.Maximum = 127;

            // CheckBox for LFO Key Trigger
            CheckBox cbEditTone_PCMSynthTone_LFO2_LFOKeyTrigger = new CheckBox();
            cbEditTone_PCMSynthTone_LFO2_LFOKeyTrigger.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_LFO2_LFOKeyTrigger.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_LFO2_LFOKeyTrigger.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_LFO2_LFOKeyTrigger.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_LFO2_LFOKeyTrigger.Content = "LFO Key Trigger";
            cbEditTone_PCMSynthTone_LFO2_LFOKeyTrigger.Name = "cbEditTone_PCMSynthTone_LFO2KeyTrigger";

            // Slider for LFO Pitch Depth:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO2_LFOPitchDepth);
            Slider slEditTone_PCMSynthTone_LFO2_LFOPitchDepth = new Slider();
            slEditTone_PCMSynthTone_LFO2_LFOPitchDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO2_LFOPitchDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO2_LFOPitchDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO2_LFOPitchDepth.Name = "slEditTone_PCMSynthTone_LFO2_LFOPitchDepth";
            slEditTone_PCMSynthTone_LFO2_LFOPitchDepth.Minimum = -63;
            slEditTone_PCMSynthTone_LFO2_LFOPitchDepth.Maximum = 63;

            // Slider for LFO TVF Depth:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO2_LFOTVFDepth);
            Slider slEditTone_PCMSynthTone_LFO2_LFOTVFDepth = new Slider();
            slEditTone_PCMSynthTone_LFO2_LFOTVFDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO2_LFOTVFDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO2_LFOTVFDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO2_LFOTVFDepth.Name = "slEditTone_PCMSynthTone_LFO2_LFOTVFDepth";
            slEditTone_PCMSynthTone_LFO2_LFOTVFDepth.Minimum = 0;
            slEditTone_PCMSynthTone_LFO2_LFOTVFDepth.Maximum = 127;

            // Slider for LFO TVA Depth:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO2_LFOTVADepth);
            Slider slEditTone_PCMSynthTone_LFO2_LFOTVADepth = new Slider();
            slEditTone_PCMSynthTone_LFO2_LFOTVADepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO2_LFOTVADepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO2_LFOTVADepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO2_LFOTVADepth.Name = "slEditTone_PCMSynthTone_LFO2_LFOTVADepth";
            slEditTone_PCMSynthTone_LFO2_LFOTVADepth.Minimum = 0;
            slEditTone_PCMSynthTone_LFO2_LFOTVADepth.Maximum = 127;

            // Slider for LFO Pan Depth:
            SetLabelProperties(ref tbEditTone_PCMSynthTone_LFO2_LFOPanDepth);
            Slider slEditTone_PCMSynthTone_LFO2_LFOPanDepth = new Slider();
            slEditTone_PCMSynthTone_LFO2_LFOPanDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMSynthTone_LFO2_LFOPanDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMSynthTone_LFO2_LFOPanDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMSynthTone_LFO2_LFOPanDepth.Name = "slEditTone_PCMSynthTone_LFO2_LFOPanDepth";
            slEditTone_PCMSynthTone_LFO2_LFOPanDepth.Minimum = 0;
            slEditTone_PCMSynthTone_LFO2_LFOPanDepth.Maximum = 127;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch,
                cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch}, new byte[] { 1, 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_PCMSynthTone_LFO2_LFO2Waveform })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMSynthTone_LFO2_LFORate, slEditTone_PCMSynthTone_LFO2_LFORate }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMSynthTone_LFO2_LFORateDetune, slEditTone_PCMSynthTone_LFO2_LFORateDetune }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { cbEditTone_PCMSynthTone_LFO2_LFOOffset })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_PCMSynthTone_LFO2_LFODelayTime, slEditTone_PCMSynthTone_LFO2_LFODelayTime }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow, slEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { cbEditTone_PCMSynthTone_LFO2_LFOFadeMode })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_PCMSynthTone_LFO2_LFOFadeTime, slEditTone_PCMSynthTone_LFO2_LFOFadeTime }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { cbEditTone_PCMSynthTone_LFO2_LFOKeyTrigger })).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { tbEditTone_PCMSynthTone_LFO2_LFOPitchDepth, slEditTone_PCMSynthTone_LFO2_LFOPitchDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(11, new Control[] { tbEditTone_PCMSynthTone_LFO2_LFOTVFDepth, slEditTone_PCMSynthTone_LFO2_LFOTVFDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(12, new Control[] { tbEditTone_PCMSynthTone_LFO2_LFOTVADepth, slEditTone_PCMSynthTone_LFO2_LFOTVADepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(13, new Control[] { tbEditTone_PCMSynthTone_LFO2_LFOPanDepth, slEditTone_PCMSynthTone_LFO2_LFOPanDepth }, new byte[] { 1, 2 })).Row);

            // Set values
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            cbEditTone_PCMSynthTone_LFO2_LFO2Waveform.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.LFO2.LFOWaveform;
            slEditTone_PCMSynthTone_LFO2_LFORate.Value = (pCMSynthTone.pCMSynthTonePartial.LFO2.LFORate);
            if (pCMSynthTone.pCMSynthTonePartial.LFO2.LFORate < 128)
            {
                tbEditTone_PCMSynthTone_LFO2_LFORate.Text = "LFO Rate: " + ((pCMSynthTone.pCMSynthTonePartial.LFO2.LFORate)).ToString();
            }
            else
            {
                tbEditTone_PCMSynthTone_LFO2_LFORate.Text = "LFO Rate: " + toneLengths[pCMSynthTone.pCMSynthTonePartial.LFO2.LFORate - 128];
            }
            slEditTone_PCMSynthTone_LFO2_LFORateDetune.Value = (pCMSynthTone.pCMSynthTonePartial.LFO2.LFORateDetune);
            tbEditTone_PCMSynthTone_LFO2_LFORateDetune.Text = "Rate Detune: " + ((pCMSynthTone.pCMSynthTonePartial.LFO2.LFORateDetune)).ToString();
            cbEditTone_PCMSynthTone_LFO2_LFOOffset.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.LFO2.LFOOffset;
            slEditTone_PCMSynthTone_LFO2_LFODelayTime.Value = (pCMSynthTone.pCMSynthTonePartial.LFO2.LFODelayTime);
            tbEditTone_PCMSynthTone_LFO2_LFODelayTime.Text = "Delay Time: " + ((pCMSynthTone.pCMSynthTonePartial.LFO2.LFODelayTime)).ToString();
            slEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow.Value = (pCMSynthTone.pCMSynthTonePartial.LFO2.LFODelayTimeKeyfollow - 64) * 10;
            tbEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow.Text = "Del-T Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial.LFO2.LFODelayTimeKeyfollow - 64) * 10).ToString();
            cbEditTone_PCMSynthTone_LFO2_LFOFadeMode.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.LFO2.LFOFadeMode;
            slEditTone_PCMSynthTone_LFO2_LFOFadeTime.Value = (pCMSynthTone.pCMSynthTonePartial.LFO2.LFOFadeTime);
            tbEditTone_PCMSynthTone_LFO2_LFOFadeTime.Text = "LFO Fade Time: " + ((pCMSynthTone.pCMSynthTonePartial.LFO2.LFOFadeTime)).ToString();
            cbEditTone_PCMSynthTone_LFO2_LFOKeyTrigger.IsChecked = pCMSynthTone.pCMSynthTonePartial.LFO2.LFOKeyTrigger;
            slEditTone_PCMSynthTone_LFO2_LFOTVFDepth.Value = (pCMSynthTone.pCMSynthTonePartial.LFO2.LFOTVFDepth - 64);
            tbEditTone_PCMSynthTone_LFO2_LFOTVFDepth.Text = "LFO TVF Depth: " + ((pCMSynthTone.pCMSynthTonePartial.LFO2.LFOTVFDepth - 64)).ToString();
            slEditTone_PCMSynthTone_LFO2_LFOTVADepth.Value = (pCMSynthTone.pCMSynthTonePartial.LFO2.LFOTVADepth - 64);
            tbEditTone_PCMSynthTone_LFO2_LFOTVADepth.Text = "LFO TVA Depth: " + ((pCMSynthTone.pCMSynthTonePartial.LFO2.LFOTVADepth - 64)).ToString();
            slEditTone_PCMSynthTone_LFO2_LFOPanDepth.Value = (pCMSynthTone.pCMSynthTonePartial.LFO2.LFOPanDepth - 64);
            tbEditTone_PCMSynthTone_LFO2_LFOPanDepth.Text = "LFO Pan Depth: " + ((pCMSynthTone.pCMSynthTonePartial.LFO2.LFOPanDepth - 64)).ToString();
            slEditTone_PCMSynthTone_LFO2_LFOPitchDepth.Value = (pCMSynthTone.pCMSynthTonePartial.LFO2.LFOPitchDepth - 64);
            tbEditTone_PCMSynthTone_LFO2_LFOPitchDepth.Text = "LFO Pitch Depth: " + ((pCMSynthTone.pCMSynthTonePartial.LFO2.LFOPitchDepth - 64)).ToString();
        }

        private void AddPCMSynthToneStepLFOControls(byte Partial)
        {
            t.Trace("private void AddPCMSynthToneStepLFOControls (" + "byte" + Partial + ", " + ")");
            controlsIndex = 0;

            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // ComboBox for LFO Step Type
            ComboBox cbEditTone_PCMSynthTone_StepLFO_LFOStepType = new ComboBox();
            cbEditTone_PCMSynthTone_StepLFO_LFOStepType.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_StepLFO_LFOStepType.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_StepLFO_LFOStepType.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_StepLFO_LFOStepType.Name = "cbEditTone_PCMSynthTone_StepLFO_LFOStepType";
            cbEditTone_PCMSynthTone_StepLFO_LFOStepType.Items.Add("Type 0");
            cbEditTone_PCMSynthTone_StepLFO_LFOStepType.Items.Add("Type 1");

            // Slider for LFO Step:
            Slider[] slEditTone_PCMSynthTone_StepLFO_LFOStep = new Slider[16];
            for (byte i = 0; i < 16; i++)
            {
                tbEditTone_PCMSynthTone_StepLFO_LFOStep[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMSynthTone_StepLFO_LFOStep[i]);
                slEditTone_PCMSynthTone_StepLFO_LFOStep[i] = new Slider();
                slEditTone_PCMSynthTone_StepLFO_LFOStep[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMSynthTone_StepLFO_LFOStep[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMSynthTone_StepLFO_LFOStep[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMSynthTone_StepLFO_LFOStep[i].Name = "slEditTone_PCMSynthTone_StepLFO_LFOStep" + i.ToString();
                slEditTone_PCMSynthTone_StepLFO_LFOStep[i].Minimum = -36;
                slEditTone_PCMSynthTone_StepLFO_LFOStep[i].Maximum = 36;
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_StepLFO_LFOStepType, cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch,
                cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch}, new byte[] { 1, 1, 1, 1, 1, 1 })).Row);
            for (byte i = 0; i < 8; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(1 + i), new Control[] { tbEditTone_PCMSynthTone_StepLFO_LFOStep[i * 2], slEditTone_PCMSynthTone_StepLFO_LFOStep[i * 2],
                    tbEditTone_PCMSynthTone_StepLFO_LFOStep[i * 2 + 1], slEditTone_PCMSynthTone_StepLFO_LFOStep[i * 2 + 1]}, new byte[] { 1, 2, 1, 2 })).Row);
            }

            // Set values
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            cbEditTone_PCMSynthTone_StepLFO_LFOStepType.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.LFOStepType;
            for (byte i = 0; i < 16; i++)
            {
                slEditTone_PCMSynthTone_StepLFO_LFOStep[i].Value = (pCMSynthTone.pCMSynthTonePartial.LFOStep[i] - 64);
                tbEditTone_PCMSynthTone_StepLFO_LFOStep[i].Text = "Step " + (i + 1).ToString() + ": " + ((pCMSynthTone.pCMSynthTonePartial.LFOStep[i] - 64)).ToString();
            }
        }

        private void AddPCMSynthToneControlControls(byte Partial)
        {
            t.Trace("private void AddPCMSynthToneControlControls (" + "byte" + Partial + ", " + ")");
            controlsIndex = 0;

            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // ComboBox for Partial Env Mode
            ComboBox cbEditTone_PCMSynthTone_Control_PartialEnvMode = new ComboBox();
            cbEditTone_PCMSynthTone_Control_PartialEnvMode.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_Control_PartialEnvMode.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Control_PartialEnvMode.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Control_PartialEnvMode.Name = "cbEditTone_PCMSynthTone_Control_PartialEnvMode";
            cbEditTone_PCMSynthTone_Control_PartialEnvMode.Items.Add("No sustain");
            cbEditTone_PCMSynthTone_Control_PartialEnvMode.Items.Add("Sustain");

            // CheckBox for Partial Receive Bender
            CheckBox cbEditTone_PCMSynthTone_Control_PartialReceiveBender = new CheckBox();
            cbEditTone_PCMSynthTone_Control_PartialReceiveBender.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Control_PartialReceiveBender.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Control_PartialReceiveBender.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Control_PartialReceiveBender.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Control_PartialReceiveBender.Content = "Partial Receive Bender";
            cbEditTone_PCMSynthTone_Control_PartialReceiveBender.Name = "cbEditTone_PCMSynthTone_PartialReceiveBender";

            // CheckBox for Partial Receive Expression
            CheckBox cbEditTone_PCMSynthTone_Control_PartialReceiveExpression = new CheckBox();
            cbEditTone_PCMSynthTone_Control_PartialReceiveExpression.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Control_PartialReceiveExpression.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Control_PartialReceiveExpression.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Control_PartialReceiveExpression.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Control_PartialReceiveExpression.Content = "Partial Receive Expression";
            cbEditTone_PCMSynthTone_Control_PartialReceiveExpression.Name = "cbEditTone_PCMSynthTone_PartialReceiveExpression";

            // CheckBox for Partial Receive Hold_1
            CheckBox cbEditTone_PCMSynthTone_Control_PartialReceiveHold_1 = new CheckBox();
            cbEditTone_PCMSynthTone_Control_PartialReceiveHold_1.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Control_PartialReceiveHold_1.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Control_PartialReceiveHold_1.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Control_PartialReceiveHold_1.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Control_PartialReceiveHold_1.Content = "Partial Receive Hold_1";
            cbEditTone_PCMSynthTone_Control_PartialReceiveHold_1.Name = "cbEditTone_PCMSynthTone_PartialReceiveHold_1";

            // CheckBox for Partial Redamper Switch
            CheckBox cbEditTone_PCMSynthTone_Control_PartialRedamperSwitch = new CheckBox();
            cbEditTone_PCMSynthTone_Control_PartialRedamperSwitch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Control_PartialRedamperSwitch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Control_PartialRedamperSwitch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Control_PartialRedamperSwitch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Control_PartialRedamperSwitch.Content = "Partial Redamper Switch";
            cbEditTone_PCMSynthTone_Control_PartialRedamperSwitch.Name = "cbEditTone_PCMSynthTone_PartialRedamperSwitch";

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch,
                cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch}, new byte[] { 1, 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_PCMSynthTone_Control_PartialEnvMode })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { cbEditTone_PCMSynthTone_Control_PartialReceiveBender })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { cbEditTone_PCMSynthTone_Control_PartialReceiveExpression })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { cbEditTone_PCMSynthTone_Control_PartialReceiveHold_1 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { cbEditTone_PCMSynthTone_Control_PartialRedamperSwitch })).Row);

            // Set values
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            cbEditTone_PCMSynthTone_Control_PartialEnvMode.SelectedIndex = pCMSynthTone.pCMSynthTonePartial.PartialEnvMode;
            cbEditTone_PCMSynthTone_Control_PartialReceiveBender.IsChecked = pCMSynthTone.pCMSynthTonePartial.PartialReceiveBender;
            cbEditTone_PCMSynthTone_Control_PartialReceiveExpression.IsChecked = pCMSynthTone.pCMSynthTonePartial.PartialReceiveExpression;
            cbEditTone_PCMSynthTone_Control_PartialReceiveHold_1.IsChecked = pCMSynthTone.pCMSynthTonePartial.PartialReceiveHold_1;
            cbEditTone_PCMSynthTone_Control_PartialRedamperSwitch.IsChecked = pCMSynthTone.pCMSynthTonePartial.PartialRedamperSwitch;
        }

        private void AddPCMSynthToneMatrixControlControls()
        {
            t.Trace("private void AddPCMSynthToneMatrixControlControls ()");
            controlsIndex = 0;

            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Content = "Partial 1";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.Name = "cbEditTone_PCMSynthTone_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Content = "Partial 2";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.Name = "cbEditTone_PCMSynthTone_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Content = "Partial 3";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.Name = "cbEditTone_PCMSynthTone_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch = new CheckBox();
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Content = "Partial 4";
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.Name = "cbEditTone_PCMSynthTone_Partial4Switch";

            // ComboBox for selecting matrix control page:
            ComboBox cbEditTone_PCMSynthTone_MatrixControl_Page = new ComboBox();
            cbEditTone_PCMSynthTone_MatrixControl_Page.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_MatrixControl_Page.GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_MatrixControl_Page.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_MatrixControl_Page.Name = "cbEditTone_PCMSynthTone_MatrixControl_Page";
            cbEditTone_PCMSynthTone_MatrixControl_Page.Items.Add("Matrix control 1");
            cbEditTone_PCMSynthTone_MatrixControl_Page.Items.Add("Matrix control 2");
            cbEditTone_PCMSynthTone_MatrixControl_Page.Items.Add("Matrix control 3");
            cbEditTone_PCMSynthTone_MatrixControl_Page.Items.Add("Matrix control 4");
            handleControlEvents = false;
            cbEditTone_PCMSynthTone_MatrixControl_Page.SelectedIndex = currentMatrixControlPage;

            // ComboBox for Matrix Control Source
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage] = new ComboBox();
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].GotFocus += Generic_GotFocus;
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Name = "cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource";
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: Off");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC01");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC02");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC03");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC04");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC05");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC06");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC07");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC08");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC09");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC10");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC11");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC12");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC13");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC14");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC15");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC16");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC17");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC18");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC19");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC20");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC21");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC22");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC23");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC24");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC25");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC26");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC27");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC28");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC29");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC30");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC31");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC32");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC33");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC34");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC35");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC36");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC37");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC38");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC39");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC40");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC41");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC42");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC43");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC44");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC45");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC46");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC47");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC48");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC49");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC50");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC51");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC52");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC53");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC54");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC55");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC56");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC57");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC58");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC59");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC60");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC61");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC62");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC63");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC64");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC65");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC66");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC67");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC68");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC69");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC70");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC71");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC72");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC73");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC74");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC75");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC76");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC77");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC78");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC79");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC80");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC81");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC82");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC83");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC84");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC85");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC86");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC87");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC88");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC89");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC90");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC91");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC92");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC93");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC94");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: CC95");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: Pitch bend");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: After touch");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: Control 1");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: Control 2");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: Control 3");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: Control 4");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: Velocity");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: Key follow");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: Tempo");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: LF01");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: LF02");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: Pitch env");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: TVF env");
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].Items.Add("Source: TVA env");

            // ComboBox for Matrix Control Destination
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage] = new ComboBox[4];
            // CheckBox for PartialControlSwitch
            cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage] = new ComboBox[4];
            // Slider for Matrix Control Sens:
            tbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage] = new TextBox[4];
            slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage] = new Slider[4];

            for (byte i = 0; i < 4; i++)
            {
                // ComboBox for Matrix Control Destination
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i] = new ComboBox();
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Name = "cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination" + i.ToString();
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: OFF");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: PCH");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: CUT");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: RES");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: LEV");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: PAN");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: DRY");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: CHO");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: REV");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: PIT-LFO1");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: PIT-LFO2");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: TVF-LFO1");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: TVF-LFO2");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: TVA-LFO1");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: TVA-LFO2");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: PAN-LFO1");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: PAN-LFO2");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: LFO1-RATE");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: LFO2-RATE");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: PIT-ATK");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: PIT-DCY");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: PIT-REL");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: TVF-ATK");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: TVF-DCY");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: TVF-REL");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: TVA-ATK");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: TVA-DCY");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: TVA-REL");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: PMT");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: FXM");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: ---");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: ---");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: ---");
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].Items.Add("Destination: ---");

                // CheckBox for PartialControlSwitch
//                cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i] = new ComboBox();
//                for (byte partial = 0; partial < 4; partial++)
                {
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i] = new ComboBox();
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i].SelectionChanged += GenericCombobox_SelectionChanged;
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i].GotFocus += Generic_GotFocus;
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i].Tag = new HelpTag(controlsIndex++, 0);
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i].Name = "cbEditTone_PCMSynthTone_PartialControlSwitch" + i.ToString();// + partial.ToString() + i.ToString();
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i].Items.Add("Off");
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i].Items.Add("On");
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i].Items.Add("Reverse");
                }

                // Slider for Matrix Control Sens:
                tbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i]);
                slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i] = new Slider();
                slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i].GotFocus += Generic_GotFocus;
                slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i].Name =
                    "slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens" + i.ToString();
                slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i].Minimum = -63;
                slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i].Maximum = 63;
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch,
                cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch, cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch,
                cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch }, new byte[] { 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_PCMSynthTone_MatrixControl_Page,
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage] })).Row);
            for (byte i = 0; i < 4; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(2 + 2 * i), new Control[] {
                    cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i],
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i],
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i],
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i],
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i],
                }, new byte[] { 2, 1, 1, 1, 1 })).Row);
                ControlsGrid.Children.Add((new GridRow((byte)(3 + 2 * i), new Control[] {
                    tbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i],
                    slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i] },
                    new byte[] { 1, 2 })).Row);
            }

            // Set control values
            cbEditTone_PCMSynthTone_Pitch_PitchPartial1Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial2Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial3Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2];
            cbEditTone_PCMSynthTone_Pitch_PitchPartial4Switch.IsChecked = pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3];
            cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource[currentMatrixControlPage].SelectedIndex =
                pCMSynthTone.pCMSynthToneCommon.MatrixControlSource[currentMatrixControlPage];
            for (byte i = 0; i < 4; i++)
            {
                cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination[currentMatrixControlPage][i].SelectedIndex =
                    pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][i];
            }
            for (byte i = 0; i < 4; i++)
            {
                slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i].Value =
                    (pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][i] - 64);
                tbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][i].Text =
                    "Matrix Control " + (i + 1).ToString() + " Sens: " +
                    ((pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][i] - 64)).ToString();
                    cbEditTone_PCMSynthTone_MatrixControl1_PartialControlSwitch[currentMatrixControlPage][i].SelectedIndex =
                        pCMSynthTone.pCMSynthTonePartial.PartialControlSwitch[currentMatrixControlPage][i];
            }
        }

        private void AddPCMSynthToneMFXControlControls()
        {
            t.Trace("private void AddPCMSynthToneMFXControlControls()");
            controlsIndex = 0;
            // Create controls

            ComboBox[] cbEditTone_PCMSynthTone_MFXControl_MFXControlSource = new ComboBox[4];
            ComboBox[] cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign = new ComboBox[4];
            Slider[] slEditTone_PCMSynthTone_MFXControl_MFXControlSens = new Slider[4];
            for (byte i = 0; i < 4; i++)
            {
                // ComboBox for MFX Control Source
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i] = new ComboBox();
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Name = "cbEditTone_PCMSynthTone_MFXControl_MFXControlSource" + i.ToString();
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Off");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC01");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC02");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC03");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC04");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC05");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC06");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC07");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC08");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC09");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC10");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC11");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC12");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC13");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC14");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC15");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC16");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC17");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC18");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC19");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC20");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC21");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC22");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC23");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC24");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC25");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC26");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC27");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC28");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC29");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC30");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC31");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC32");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC33");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC34");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC35");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC36");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC37");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC38");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC39");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC40");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC41");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC42");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC43");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC44");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC45");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC46");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC47");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC48");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC49");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC50");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC51");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC52");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC53");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC54");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC55");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC56");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC57");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC58");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC59");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC60");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC61");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC62");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC63");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC64");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC65");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC66");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC67");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC68");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC69");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC70");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC71");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC72");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC73");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC74");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC75");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC76");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC77");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC78");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC79");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC80");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC81");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC82");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC83");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC84");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC85");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC86");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC87");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC88");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC89");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC90");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC91");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC92");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC93");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC94");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC95");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Pitch bend");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " After touch");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 1");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 2");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 3");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 4");

                // ComboBox for MFX Control Destination
                cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign[i] = new ComboBox();
                cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign[i].Name = "cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign" + i.ToString();
                cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign[i].Items.Add("Destination : Off");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign[i].Items.Add("Destination : Low gain");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign[i].Items.Add("Destination : High gain");
                cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign[i].Items.Add("Destination : Level");

                // Slider for MFX Control Sense:
                tbEditTone_PCMSynthTone_MFXControl_MFXControlSens[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMSynthTone_MFXControl_MFXControlSens[i]);
                slEditTone_PCMSynthTone_MFXControl_MFXControlSens[i] = new Slider();
                slEditTone_PCMSynthTone_MFXControl_MFXControlSens[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMSynthTone_MFXControl_MFXControlSens[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMSynthTone_MFXControl_MFXControlSens[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMSynthTone_MFXControl_MFXControlSens[i].Name = "slEditTone_PCMSynthTone_MFXControl_MFXControlSense" + i.ToString();
                slEditTone_PCMSynthTone_MFXControl_MFXControlSens[i].Minimum = -63;
                slEditTone_PCMSynthTone_MFXControl_MFXControlSens[i].Maximum = 63;
            }

            // Put in rows
            for (byte i = 0; i < 4; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(0 + 3 * i), new Control[] { cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i] })).Row);
                ControlsGrid.Children.Add((new GridRow((byte)(1 + 3 * i), new Control[] { cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign[i] })).Row);
                ControlsGrid.Children.Add((new GridRow((byte)(2 + 3 * i), new Control[] { tbEditTone_PCMSynthTone_MFXControl_MFXControlSens[i], slEditTone_PCMSynthTone_MFXControl_MFXControlSens[i] }, new byte[] { 1, 2 })).Row);
            }

            // Set values
            //handleControlEvents = false;
            for (byte i = 0; i < 4; i++)
            {
                cbEditTone_PCMSynthTone_MFXControl_MFXControlSource[i].SelectedIndex = commonMFX.MFXControlSource[i];
                cbEditTone_PCMSynthTone_MFXControl_MFXControlAssign[i].SelectedIndex = commonMFX.MFXControlAssign[i];
                slEditTone_PCMSynthTone_MFXControl_MFXControlSens[i].Value = (commonMFX.MFXControlSens[i] - 64);
                tbEditTone_PCMSynthTone_MFXControl_MFXControlSens[i].Text = "MFX Control " + (byte)(i + 1) + " Sense: " + ((commonMFX.MFXControlSens[i] - 64)).ToString();
            }
        }

        // Save controls
        private void AddSaveDeleteControls()
        {
            t.Trace("private void AddSaveDeleteControls()");
            controlsIndex = 0;

            // Create controls
            SetLabelProperties(ref tbEditTone_SaveTone_Title);
            btnEditTone_SaveTone.Content = "Save";
            btnEditTone_SaveTone.Click += btnEditTone_SaveTone_Click;
            Button btnEditTone_DeleteTone = new Button();
            btnEditTone_DeleteTone.Content = "Delete";
            btnEditTone_DeleteTone.Click += btnEditTone_DeleteTone_Click;

            // Hook tbEditTone_Save_TitleText to a validator:
            btnEditTone_SaveTone.IsEnabled = false;
            tbEditTone_SaveTone_TitleText.KeyUp += TbEditTone_Save_TitleText_KeyUp;

            // Hook to help:
            tbEditTone_SaveTone_TitleText.GotFocus += Generic_GotFocus;
            tbEditTone_SaveTone_TitleText.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SaveTone_SlotNumber.GotFocus += Generic_GotFocus;
            cbEditTone_SaveTone_SlotNumber.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SaveTone_SlotNumber.SelectionChanged += CbEditTone_Save_SlotNumber_SelectionChanged;
            btnEditTone_SaveTone.GotFocus += Generic_GotFocus;
            btnEditTone_SaveTone.Tag = new HelpTag(controlsIndex++, 0);
            btnEditTone_DeleteTone.GotFocus += Generic_GotFocus;
            btnEditTone_DeleteTone.Tag = new HelpTag(controlsIndex++, 0);

            String numString;
            cbEditTone_SaveTone_SlotNumber.Items.Clear();
            if (commonState.toneNames[0] != null && commonState.toneNames[0].Count() == 256)
            {
                for (UInt16 i = 0; i < 256; i++)
                {
                    numString = (i + 1).ToString();
                    while (numString.Length < 3) numString = "0" + numString;
                    cbEditTone_SaveTone_SlotNumber.Items.Add(numString + ": " + commonState.toneNames[0][i]);
                }
            }
            else
            {
                for (UInt16 i = 0; i < 256; i++)
                {
                    numString = (i + 1).ToString();
                    while (numString.Length < 3) numString = "0" + numString;
                    cbEditTone_SaveTone_SlotNumber.Items.Add(numString + ": INIT TONE");
                }
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow((byte)(0), new Control[] { tbEditTone_SaveTone_Title,
                tbEditTone_SaveTone_TitleText, cbEditTone_SaveTone_SlotNumber, btnEditTone_SaveTone,
                btnEditTone_DeleteTone}, new byte[] { 4, 3, 3, 2, 2 })).Row);

            // Set values
            tbEditTone_SaveTone_Title.Text = "Name (max 12 chars):";
            try
            {
                tbEditTone_SaveTone_TitleText.Text = commonState.currentTone.Name;
            }
            catch
            {
                tbEditTone_SaveTone_TitleText.Text = "";
            }
            SetSaveSlotToFirstFreeOrSameName();
        }

        #endregion

        #region PCM Drum Kit

        // PCM Drum Kit controls
        private void AddPCMDrumKitCommonControls()
        {
            t.Trace("private void AddPCMDrumKitCommonControls()");
            controlsIndex = 0;

            // There are 88 partials in PCM Drum Kit, corresponding to keys.
            // Combobox for selecting key number.
            // ComboBox for currentPartial
            //ComboBox cbEditTone_PCMDrumKit_Common_currentPartial = new ComboBox();
            //cbEditTone_PCMDrumKit_Common_currentPartial.SelectionChanged += GenericCombobox_SelectionChanged;
            //cbEditTone_PCMDrumKit_Common_currentPartial.GotFocus += Generic_GotFocus;
            //cbEditTone_PCMDrumKit_Common_currentPartial.Tag = new HelpTag(controlsIndex++, 0);
            //cbEditTone_PCMDrumKit_Common_currentPartial.Name = "cbEditTone_PCMDrumKit_Common_currentPartial";
            //cbEditTone_PCMDrumKit_Common_currentPartial.Items.Add("");

            //ComboBox cbEditTone_PCMSynthTone_Partial = new ComboBox();
            //cbEditTone_PCMSynthTone_Partial.SelectionChanged += GenericCombobox_SelectionChanged;
            //cbEditTone_PCMSynthTone_Partial.GotFocus += Generic_GotFocus;
            //cbEditTone_PCMSynthTone_Partial.Tag = new HelpTag(controlsIndex++, 0);
            //cbEditTone_PCMSynthTone_Partial.Name = "cbEditTone_PCMSynthTone_Partial";
            //cbEditTone_PCMSynthTone_Partial.Items.Add("Partial 1");
            //cbEditTone_PCMSynthTone_Partial.Items.Add("Partial 2");
            //cbEditTone_PCMSynthTone_Partial.Items.Add("Partial 3");
            //cbEditTone_PCMSynthTone_Partial.Items.Add("Partial 4");

               // ComboBox for Phrase Number
            ComboBox cbEditTone_PCMDrumKit_Common_PhraseNumber = new ComboBox();
            cbEditTone_PCMDrumKit_Common_PhraseNumber.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_Common_PhraseNumber.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Name = "cbEditTone_PCMDrumKit_Common_PhraseNumber";
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 0: No assign");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 1: Standard 1");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 2: Standard 2");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 3: Room 1");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 4: Room 2");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 5: Power 1");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 6: Power 2");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 7: Electric 1");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 8: Electric 2");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 9: Analog 1");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 10: Analog 2");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 11: Jazz 1");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 12: Jazz 2");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 13: Brush 1");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 14: Brush 2");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 15: Orchestra 1");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 16: Orchestra 2");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 17: SFX 1");
            cbEditTone_PCMDrumKit_Common_PhraseNumber.Items.Add("Phrase number 18: SFX 2");

            // ComboBox for Assign Type
            ComboBox cbEditTone_PCMDrumKit_Common_AssignType = new ComboBox();
            cbEditTone_PCMDrumKit_Common_AssignType.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_Common_AssignType.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Common_AssignType.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Common_AssignType.Name = "cbEditTone_PCMDrumKit_Common_AssignType";
            cbEditTone_PCMDrumKit_Common_AssignType.Items.Add("Assign type: Multi");
            cbEditTone_PCMDrumKit_Common_AssignType.Items.Add("Assign type: Single");

            // Slider for Drum Kit Level:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Common_DrumKitLevel);
            Slider slEditTone_PCMDrumKit_Common_DrumKitLevel = new Slider();
            slEditTone_PCMDrumKit_Common_DrumKitLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Common_DrumKitLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Common_DrumKitLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Common_DrumKitLevel.Name = "slEditTone_PCMDrumKit_Common_DrumKitLevel";
            slEditTone_PCMDrumKit_Common_DrumKitLevel.Minimum = 0;
            slEditTone_PCMDrumKit_Common_DrumKitLevel.Maximum = 127;

            // ComboBox for Mute Group
            ComboBox cbEditTone_PCMDrumKit_Common_MuteGroup = new ComboBox();
            cbEditTone_PCMDrumKit_Common_MuteGroup.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_Common_MuteGroup.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Common_MuteGroup.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Common_MuteGroup.Name = "cbEditTone_PCMDrumKit_Common_MuteGroup";
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: Off");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 1");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 2");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 3");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 4");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 5");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 6");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 7");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 8");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 9");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 10");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 11");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 12");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 13");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 14");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 15");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 16");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 17");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 18");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 19");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 20");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 21");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 22");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 23");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 24");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 25");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 26");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 27");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 28");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 29");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 30");
            cbEditTone_PCMDrumKit_Common_MuteGroup.Items.Add("Mute group: 31");

            // ComboBox for Partial Env Mode
            ComboBox cbEditTone_PCMDrumKit_Common_PartialEnvMode = new ComboBox();
            cbEditTone_PCMDrumKit_Common_PartialEnvMode.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_Common_PartialEnvMode.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Common_PartialEnvMode.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Common_PartialEnvMode.Name = "cbEditTone_PCMDrumKit_Common_PartialEnvMode";
            cbEditTone_PCMDrumKit_Common_PartialEnvMode.Items.Add("No sustain");
            cbEditTone_PCMDrumKit_Common_PartialEnvMode.Items.Add("Sustain");

            // Slider for Partial Pitch Bend Range:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Common_PartialPitchBendRange);
            Slider slEditTone_PCMDrumKit_Common_PartialPitchBendRange = new Slider();
            slEditTone_PCMDrumKit_Common_PartialPitchBendRange.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Common_PartialPitchBendRange.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Common_PartialPitchBendRange.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Common_PartialPitchBendRange.Name = "slEditTone_PCMDrumKit_Common_PartialPitchBendRange";
            slEditTone_PCMDrumKit_Common_PartialPitchBendRange.Minimum = 0;
            slEditTone_PCMDrumKit_Common_PartialPitchBendRange.Maximum = 48;

            // CheckBox for Partial Receive Expression
            CheckBox cbEditTone_PCMDrumKit_Common_PartialReceiveExpression = new CheckBox();
            cbEditTone_PCMDrumKit_Common_PartialReceiveExpression.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Common_PartialReceiveExpression.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Common_PartialReceiveExpression.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Common_PartialReceiveExpression.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Common_PartialReceiveExpression.Content = "Partial Receive Expression";
            cbEditTone_PCMDrumKit_Common_PartialReceiveExpression.Name = "cbEditTone_PCMDrumKit_PartialReceiveExpression";

            // CheckBox for Partial Receive Hold_1
            CheckBox cbEditTone_PCMDrumKit_Common_PartialReceiveHold_1 = new CheckBox();
            cbEditTone_PCMDrumKit_Common_PartialReceiveHold_1.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Common_PartialReceiveHold_1.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Common_PartialReceiveHold_1.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Common_PartialReceiveHold_1.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Common_PartialReceiveHold_1.Content = "Partial Receive Hold_1";
            cbEditTone_PCMDrumKit_Common_PartialReceiveHold_1.Name = "cbEditTone_PCMDrumKit_PartialReceiveHold_1";

            // CheckBox for One Shot Mode
            CheckBox cbEditTone_PCMDrumKit_Common_OneShotMode = new CheckBox();
            cbEditTone_PCMDrumKit_Common_OneShotMode.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Common_OneShotMode.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Common_OneShotMode.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Common_OneShotMode.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Common_OneShotMode.Content = "One Shot Mode";
            cbEditTone_PCMDrumKit_Common_OneShotMode.Name = "cbEditTone_PCMDrumKit_OneShotMode";

            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMDrumKit_Common_PhraseNumber,
                cbEditTone_PCMDrumKit_Common_AssignType,  })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMDrumKit_Common_DrumKitLevel,
                slEditTone_PCMDrumKit_Common_DrumKitLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { cbEditTone_PCMDrumKit_Common_MuteGroup,
                cbEditTone_PCMDrumKit_Common_PartialEnvMode})).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMDrumKit_Common_PartialPitchBendRange,
                slEditTone_PCMDrumKit_Common_PartialPitchBendRange }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { cbEditTone_PCMDrumKit_Common_PartialReceiveExpression,
                cbEditTone_PCMDrumKit_Common_PartialReceiveHold_1, cbEditTone_PCMDrumKit_Common_OneShotMode})).Row);

            cbEditTone_PCMDrumKit_Common_PhraseNumber.SelectedIndex = pCMDrumKit.pCMDrumKitCommon2.PhraseNumber;
            slEditTone_PCMDrumKit_Common_DrumKitLevel.Value = (pCMDrumKit.pCMDrumKitCommon.DrumKitLevel);
            tbEditTone_PCMDrumKit_Common_DrumKitLevel.Text = "Drum Kit Level: " + ((pCMDrumKit.pCMDrumKitCommon.DrumKitLevel)).ToString();
            cbEditTone_PCMDrumKit_Common_AssignType.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.AssignType;
            cbEditTone_PCMDrumKit_Common_MuteGroup.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.MuteGroup;
            cbEditTone_PCMDrumKit_Common_PartialEnvMode.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.PartialEnvMode;
            slEditTone_PCMDrumKit_Common_PartialPitchBendRange.Value = (pCMDrumKit.pCMDrumKitPartial.PartialPitchBendRange);
            tbEditTone_PCMDrumKit_Common_PartialPitchBendRange.Text = "Partial Pitch Bend Range: " + ((pCMDrumKit.pCMDrumKitPartial.PartialPitchBendRange)).ToString();
            cbEditTone_PCMDrumKit_Common_PartialReceiveExpression.IsChecked = pCMDrumKit.pCMDrumKitPartial.PartialReceiveExpression;
            cbEditTone_PCMDrumKit_Common_PartialReceiveHold_1.IsChecked = pCMDrumKit.pCMDrumKitPartial.PartialReceiveHold_1;
            cbEditTone_PCMDrumKit_Common_OneShotMode.IsChecked = pCMDrumKit.pCMDrumKitPartial.OneShotMode;
        }

        private void AddPCMDrumKitWaveControls() //byte SelectedIndex)
        {
            t.Trace("private void AddPCMDrumKitWaveControls (" + "byte)"); // + SelectedIndex + ", " + ")");
            controlsIndex = 0;
            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMDrumKit_Partial1Switch = new CheckBox();
            cbEditTone_PCMDrumKit_Partial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Partial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Partial1Switch.Content = "Partial 1";
            cbEditTone_PCMDrumKit_Partial1Switch.Name = "cbEditTone_PCMDrumKit_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMDrumKit_Partial2Switch = new CheckBox();
            cbEditTone_PCMDrumKit_Partial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Partial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Partial2Switch.Content = "Partial 2";
            cbEditTone_PCMDrumKit_Partial2Switch.Name = "cbEditTone_PCMDrumKit_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMDrumKit_Partial3Switch = new CheckBox();
            cbEditTone_PCMDrumKit_Partial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Partial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Partial3Switch.Content = "Partial 3";
            cbEditTone_PCMDrumKit_Partial3Switch.Name = "cbEditTone_PCMDrumKit_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMDrumKit_Partial4Switch = new CheckBox();
            cbEditTone_PCMDrumKit_Partial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Partial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Partial4Switch.Content = "Partial 4";
            cbEditTone_PCMDrumKit_Partial4Switch.Name = "cbEditTone_PCMDrumKit_Partial4Switch";

            // ComboBox for WMT Wave Group Type
            ComboBox cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType = new ComboBox();
            cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Name = "cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType";
            cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("Internal");
            cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX");
            cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("---");
            cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("---");
            //cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX-01: Dynamic Drum Kits");
            //cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX-02: Concert Piano");
            //cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX-03: Studio SRX");
            //cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX-04: Symphonique Strings");
            //cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX-05: Supreme Dance");
            //cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX-06: Complete Orchestra");
            //cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX-07: Ultimate Keys");
            //cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX-08: Platinum Trax");
            //cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX-09: World Collection");
            //cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX-10: Big Brass Ensemble");
            //cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX-11: Complete Piano");
            //cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.Items.Add("SRX-12: Classic EPs");

            // ComboBox for WMT Wave Number L
            tbEditTone_Wave_WaveNumberL.Text = "Left wave (mono), Partial " + (currentPartial + 1).ToString() + ":";
            SetLabelProperties(ref tbEditTone_Wave_WaveNumberL);
            ComboBox cbEditTone_PCMDrumKit_Wave_WMTWaveNumberL = new ComboBox();
            cbEditTone_PCMDrumKit_Wave_WMTWaveNumberL.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_Wave_WMTWaveNumberL.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Wave_WMTWaveNumberL.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Wave_WMTWaveNumberL.Name = "cbEditTone_PCMDrumKit_Wave_WMTWaveNumberL";

            // ComboBox for WMT Wave Number R
            tbEditTone_Wave_WaveNumberR.Text = "Right wave, Partial " + (currentPartial + 1).ToString() + ":";
            ComboBox cbEditTone_PCMDrumKit_Wave_WMTWaveNumberR = new ComboBox();
            SetLabelProperties(ref tbEditTone_Wave_WaveNumberR);
            cbEditTone_PCMDrumKit_Wave_WMTWaveNumberR.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_Wave_WMTWaveNumberR.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Wave_WMTWaveNumberR.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Wave_WMTWaveNumberR.Name = "cbEditTone_PCMDrumKit_Wave_WMTWaveNumberR";

            // ComboBox for Wave Gain
            ComboBox cbEditTone_PCMDrumKit_Wave_WaveGain = new ComboBox();
            cbEditTone_PCMDrumKit_Wave_WaveGain.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_Wave_WaveGain.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Wave_WaveGain.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Wave_WaveGain.Name = "cbEditTone_PCMDrumKit_Wave_WMTWaveGain";
            cbEditTone_PCMDrumKit_Wave_WaveGain.Items.Add("Wave gain: -6 dB");
            cbEditTone_PCMDrumKit_Wave_WaveGain.Items.Add("Wave gain: 0 dB");
            cbEditTone_PCMDrumKit_Wave_WaveGain.Items.Add("Wave gain: +6 dB");
            cbEditTone_PCMDrumKit_Wave_WaveGain.Items.Add("Wave gain: +12 dB");

            // CheckBox for WMT Wave Tempo Sync
            CheckBox cbEditTone_PCMDrumKit_Wave_WMTWaveTempoSync = new CheckBox();
            cbEditTone_PCMDrumKit_Wave_WMTWaveTempoSync.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Wave_WMTWaveTempoSync.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Wave_WMTWaveTempoSync.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Wave_WMTWaveTempoSync.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Wave_WMTWaveTempoSync.Content = "WMT Wave Tempo Sync";
            cbEditTone_PCMDrumKit_Wave_WMTWaveTempoSync.Name = "cbEditTone_PCMDrumKit_WMTWaveTempoSync";

            // CheckBox for WMT Wave FXM Switch
            CheckBox cbEditTone_PCMDrumKit_Wave_WMTWaveFXMSwitch = new CheckBox();
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMSwitch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMSwitch.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMSwitch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMSwitch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMSwitch.Content = "WMT Wave FXM";
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMSwitch.Name = "cbEditTone_PCMDrumKit_WMTWaveFXMSwitch";

            // ComboBox for WMT Wave FXM Color
            ComboBox cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor = new ComboBox();
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor.Name = "cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor";
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor.Items.Add("Wave FXM Color: 1");
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor.Items.Add("Wave FXM Color: 2");
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor.Items.Add("Wave FXM Color: 3");
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor.Items.Add("Wave FXM Color: 4");

            // Slider for WMT Wave FXM Depth:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth);
            Slider slEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth = new Slider();
            slEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth.Name = "slEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth";
            slEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth.Minimum = 0;
            slEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth.Maximum = 16;

            // Slider for WMT Wave Coarse Tune:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune);
            Slider slEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune = new Slider();
            slEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune.Name = "slEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune";
            slEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune.Minimum = -48;
            slEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune.Maximum = 48;

            // Slider for WMT Wave Fine Tune:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Wave_WMTWaveFineTune);
            Slider slEditTone_PCMDrumKit_Wave_WMTWaveFineTune = new Slider();
            slEditTone_PCMDrumKit_Wave_WMTWaveFineTune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Wave_WMTWaveFineTune.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Wave_WMTWaveFineTune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Wave_WMTWaveFineTune.Name = "slEditTone_PCMDrumKit_Wave_WMTWaveFineTune";
            slEditTone_PCMDrumKit_Wave_WMTWaveFineTune.Minimum = -50;
            slEditTone_PCMDrumKit_Wave_WMTWaveFineTune.Maximum = 50;


            // Slider for WMT Wave Level:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Wave_WMTWaveLevel);
            Slider slEditTone_PCMDrumKit_Wave_WMTWaveLevel = new Slider();
            slEditTone_PCMDrumKit_Wave_WMTWaveLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Wave_WMTWaveLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Wave_WMTWaveLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Wave_WMTWaveLevel.Name = "slEditTone_PCMDrumKit_Wave_WMTWaveLevel";
            slEditTone_PCMDrumKit_Wave_WMTWaveLevel.Minimum = 0;
            slEditTone_PCMDrumKit_Wave_WMTWaveLevel.Maximum = 127;

            // Slider for WMT Wave Pan:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Wave_WMTWavePan);
            Slider slEditTone_PCMDrumKit_Wave_WMTWavePan = new Slider();
            slEditTone_PCMDrumKit_Wave_WMTWavePan.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Wave_WMTWavePan.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Wave_WMTWavePan.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Wave_WMTWavePan.Name = "slEditTone_PCMDrumKit_Wave_WMTWavePan";
            slEditTone_PCMDrumKit_Wave_WMTWavePan.Minimum = -64;
            slEditTone_PCMDrumKit_Wave_WMTWavePan.Maximum = 63;

            // CheckBox for WMT Wave Random Pan Sw
            CheckBox cbEditTone_PCMDrumKit_Wave_WMTWaveRandomPanSw = new CheckBox();
            cbEditTone_PCMDrumKit_Wave_WMTWaveRandomPanSw.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Wave_WMTWaveRandomPanSw.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Wave_WMTWaveRandomPanSw.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Wave_WMTWaveRandomPanSw.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Wave_WMTWaveRandomPanSw.Content = "WMT Wave Random Pan";
            cbEditTone_PCMDrumKit_Wave_WMTWaveRandomPanSw.Name = "cbEditTone_PCMDrumKit_WMTWaveRandomPanSw";

            // ComboBox for WMT Wave Alter Pan Switch
            ComboBox cbEditTone_PCMDrumKit_Wave_WMTWaveAlterPanSwitch = new ComboBox();
            cbEditTone_PCMDrumKit_Wave_WMTWaveAlterPanSwitch.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_Wave_WMTWaveAlterPanSwitch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Wave_WMTWaveAlterPanSwitch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Wave_WMTWaveAlterPanSwitch.Name = "cbEditTone_PCMDrumKit_Wave_WMTWaveAlterPanSwitch";
            cbEditTone_PCMDrumKit_Wave_WMTWaveAlterPanSwitch.Items.Add("Alternate pan: Off");
            cbEditTone_PCMDrumKit_Wave_WMTWaveAlterPanSwitch.Items.Add("Alternate pan: On");
            cbEditTone_PCMDrumKit_Wave_WMTWaveAlterPanSwitch.Items.Add("Alternate pan: Reverse");

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMDrumKit_Partial1Switch,
                cbEditTone_PCMDrumKit_Partial2Switch, cbEditTone_PCMDrumKit_Partial3Switch,
                cbEditTone_PCMDrumKit_Partial4Switch }, new byte[] { 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_Wave_WaveNumberL, cbEditTone_PCMDrumKit_Wave_WMTWaveNumberL })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_Wave_WaveNumberR, cbEditTone_PCMDrumKit_Wave_WMTWaveNumberR })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { cbEditTone_PCMDrumKit_Wave_WaveGain })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { cbEditTone_PCMDrumKit_Wave_WMTWaveTempoSync,
                cbEditTone_PCMDrumKit_Wave_WMTWaveFXMSwitch })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth, slEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune, slEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { tbEditTone_PCMDrumKit_Wave_WMTWaveFineTune, slEditTone_PCMDrumKit_Wave_WMTWaveFineTune }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { tbEditTone_PCMDrumKit_Wave_WMTWaveLevel, slEditTone_PCMDrumKit_Wave_WMTWaveLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(11, new Control[] { tbEditTone_PCMDrumKit_Wave_WMTWavePan, slEditTone_PCMDrumKit_Wave_WMTWavePan }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(12, new Control[] { cbEditTone_PCMDrumKit_Wave_WMTWaveRandomPanSw })).Row);
            ControlsGrid.Children.Add((new GridRow(13, new Control[] { cbEditTone_PCMDrumKit_Wave_WMTWaveAlterPanSwitch })).Row);

            // Set control values
            cbEditTone_PCMDrumKit_Partial1Switch.IsChecked = pCMDrumKit.pCMDrumKitPartial.WMT[0].WMTWaveSwitch;
            cbEditTone_PCMDrumKit_Partial2Switch.IsChecked = pCMDrumKit.pCMDrumKitPartial.WMT[1].WMTWaveSwitch;
            cbEditTone_PCMDrumKit_Partial3Switch.IsChecked = pCMDrumKit.pCMDrumKitPartial.WMT[2].WMTWaveSwitch;
            cbEditTone_PCMDrumKit_Partial4Switch.IsChecked = pCMDrumKit.pCMDrumKitPartial.WMT[3].WMTWaveSwitch;
            cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveGroupType;
            foreach (String waveName in waveNames.Names)
            {
                cbEditTone_PCMDrumKit_Wave_WMTWaveNumberL.Items.Add(waveName);
                cbEditTone_PCMDrumKit_Wave_WMTWaveNumberR.Items.Add(waveName);
            }
            cbEditTone_PCMDrumKit_Wave_WMTWaveNumberL.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveNumberL;
            cbEditTone_PCMDrumKit_Wave_WMTWaveNumberR.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveNumberR;
            cbEditTone_PCMDrumKit_Wave_WaveGain.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveGain;
            cbEditTone_PCMDrumKit_Wave_WMTWaveTempoSync.IsChecked = pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveTempoSync;
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMSwitch.IsChecked = pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveFXMSwitch;
            cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveFXMColor;
            slEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth.Value = (pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveFXMDepth);
            tbEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth.Text = "WMT Wave FXM Depth: " + ((pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveFXMDepth)).ToString();
            slEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune.Value = (pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveCoarseTune - 64);
            tbEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune.Text = "WMT Wave Coarse Tune: " + ((pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveCoarseTune - 64)).ToString();
            slEditTone_PCMDrumKit_Wave_WMTWaveFineTune.Value = (pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveFineTune - 64);
            tbEditTone_PCMDrumKit_Wave_WMTWaveFineTune.Text = "WMT Wave Fine Tune: " + ((pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveFineTune - 64)).ToString();
            slEditTone_PCMDrumKit_Wave_WMTWaveLevel.Value = (pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveLevel);
            tbEditTone_PCMDrumKit_Wave_WMTWaveLevel.Text = "WMT Wave Level: " + ((pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveLevel)).ToString();
            slEditTone_PCMDrumKit_Wave_WMTWavePan.Value = (pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWavePan - 64);
            tbEditTone_PCMDrumKit_Wave_WMTWavePan.Text = "WMT Wave Pan: " + ((pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWavePan - 64)).ToString();
            cbEditTone_PCMDrumKit_Wave_WMTWaveRandomPanSw.IsChecked = pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveRandomPanSwitch;
            cbEditTone_PCMDrumKit_Wave_WMTWaveAlterPanSwitch.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTWaveAlternatePanSwitch;
        }

        private void AddPCMDrumKitWMTControls(byte SelectedIndex)
        {
            t.Trace("private void AddPCMDrumKitWMTControls (" + "byte" + SelectedIndex + ", " + ")");
            t.Trace("private void AddPCMDrumKitWaveControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;
            // CheckBox for PitchPartial1Switch
            CheckBox cbEditTone_PCMDrumKit_Partial1Switch = new CheckBox();
            cbEditTone_PCMDrumKit_Partial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Partial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Partial1Switch.Content = "Partial 1";
            cbEditTone_PCMDrumKit_Partial1Switch.Name = "cbEditTone_PCMDrumKit_Partial1Switch";

            // CheckBox for PitchPartial2Switch
            CheckBox cbEditTone_PCMDrumKit_Partial2Switch = new CheckBox();
            cbEditTone_PCMDrumKit_Partial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Partial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Partial2Switch.Content = "Partial 2";
            cbEditTone_PCMDrumKit_Partial2Switch.Name = "cbEditTone_PCMDrumKit_Partial2Switch";

            // CheckBox for PitchPartial3Switch
            CheckBox cbEditTone_PCMDrumKit_Partial3Switch = new CheckBox();
            cbEditTone_PCMDrumKit_Partial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Partial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Partial3Switch.Content = "Partial 3";
            cbEditTone_PCMDrumKit_Partial3Switch.Name = "cbEditTone_PCMDrumKit_Partial3Switch";

            // CheckBox for PitchPartial4Switch
            CheckBox cbEditTone_PCMDrumKit_Partial4Switch = new CheckBox();
            cbEditTone_PCMDrumKit_Partial4Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial4Switch.Click += GenericCheckBox_Click;
            cbEditTone_PCMDrumKit_Partial4Switch.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Partial4Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Partial4Switch.Content = "Partial 4";
            cbEditTone_PCMDrumKit_Partial4Switch.Name = "cbEditTone_PCMDrumKit_Partial4Switch";

            // ComboBox for WMT Velocity Control
            ComboBox cbEditTone_PCMDrumKit_WMT_WMTVelocityControl = new ComboBox();
            cbEditTone_PCMDrumKit_WMT_WMTVelocityControl.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_WMT_WMTVelocityControl.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_WMT_WMTVelocityControl.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_WMT_WMTVelocityControl.Name = "cbEditTone_PCMDrumKit_WMT_WMTVelocityControl";
            cbEditTone_PCMDrumKit_WMT_WMTVelocityControl.Items.Add("Velocity control: Off");
            cbEditTone_PCMDrumKit_WMT_WMTVelocityControl.Items.Add("Velocity control: On");
            cbEditTone_PCMDrumKit_WMT_WMTVelocityControl.Items.Add("Velocity control: Random");

            // Slider for WMT Velocity Fade Width Upper:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper);
            Slider slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper = new Slider();
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper.Name = "slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper";
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper.Minimum = 0;
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper.Maximum = 127;

            // Slider for WMT Velocity Range Upper:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper);
            Slider slEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper = new Slider();
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper.Name = "slEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper";
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper.Minimum = 1;
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper.Maximum = 127;

            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper.Value = (pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTVelocityRangeUpper);

            tbEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper.Text = "WMT Velocity Range Upper: " + ((pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTVelocityRangeUpper)).ToString();

            // Slider for WMT Velocity Range Lower:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower);
            Slider slEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower = new Slider();
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower.Name = "slEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower";
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower.Minimum = 1;
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower.Maximum = 127;

            // Slider for WMT Velocity Fade Width Lower:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower);
            Slider slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower = new Slider();
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower.Name = "slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower";
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower.Minimum = 0;
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower.Maximum = 127;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMDrumKit_Partial1Switch,
                cbEditTone_PCMDrumKit_Partial2Switch, cbEditTone_PCMDrumKit_Partial3Switch,
                cbEditTone_PCMDrumKit_Partial4Switch }, new byte[] { 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_PCMDrumKit_WMT_WMTVelocityControl })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper,
                slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper, slEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower, slEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower, slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_PCMDrumKit_Partial1Switch.IsChecked = pCMDrumKit.pCMDrumKitPartial.WMT[0].WMTWaveSwitch;
            cbEditTone_PCMDrumKit_Partial2Switch.IsChecked = pCMDrumKit.pCMDrumKitPartial.WMT[1].WMTWaveSwitch;
            cbEditTone_PCMDrumKit_Partial3Switch.IsChecked = pCMDrumKit.pCMDrumKitPartial.WMT[2].WMTWaveSwitch;
            cbEditTone_PCMDrumKit_Partial4Switch.IsChecked = pCMDrumKit.pCMDrumKitPartial.WMT[3].WMTWaveSwitch;
            cbEditTone_PCMDrumKit_WMT_WMTVelocityControl.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.WMTVelocityControl;
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper.Value = (pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTVelocityFadeWidthUpper);
            tbEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper.Text = "Velocity Fade Upper: " + ((pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTVelocityFadeWidthUpper)).ToString();
            slEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower.Value = (pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTVelocityRangeLower);
            tbEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower.Text = "Velocity Range Lower: " + ((pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTVelocityRangeLower)).ToString();
            slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower.Value = (pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTVelocityFadeWidthLower);
            tbEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower.Text = "WMT Velocity Fade Width Lower: " + ((pCMDrumKit.pCMDrumKitPartial.WMT[currentPartial].WMTVelocityFadeWidthLower)).ToString();
        }

        private void AddPCMDrumKitPitchControls(byte SelectedIndex)
        {
            t.Trace("private void AddPCMDrumKitPitchControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // Slider for Partial Coarse Tune:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Pitch_PartialCoarseTune);
            Slider slEditTone_PCMDrumKit_Pitch_PartialCoarseTune = new Slider();
            slEditTone_PCMDrumKit_Pitch_PartialCoarseTune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Pitch_PartialCoarseTune.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Pitch_PartialCoarseTune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Pitch_PartialCoarseTune.Name = "slEditTone_PCMDrumKit_Pitch_PartialCoarseTune";
            slEditTone_PCMDrumKit_Pitch_PartialCoarseTune.Minimum = 0;
            slEditTone_PCMDrumKit_Pitch_PartialCoarseTune.Maximum = 127;

            // Slider for Partial Fine Tune:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Pitch_PartialFineTune);
            Slider slEditTone_PCMDrumKit_Pitch_PartialFineTune = new Slider();
            slEditTone_PCMDrumKit_Pitch_PartialFineTune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Pitch_PartialFineTune.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Pitch_PartialFineTune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Pitch_PartialFineTune.Name = "slEditTone_PCMDrumKit_Pitch_PartialFineTune";
            slEditTone_PCMDrumKit_Pitch_PartialFineTune.Minimum = -50;
            slEditTone_PCMDrumKit_Pitch_PartialFineTune.Maximum = 50;

            // ComboBox for Partial Random Pitch Depth
            ComboBox cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth = new ComboBox();
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Name = "cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth";
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("0");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("1");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("2");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("3");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("4");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("5");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("6");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("7");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("8");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("9");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("10");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("20");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("30");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("40");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("50");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("60");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("70");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("80");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("90");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("100");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("200");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("300");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("400");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("500");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("600");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("700");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("800");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("900");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("1000");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("1100");
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.Items.Add("1200");

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { tbEditTone_PCMDrumKit_Pitch_PartialCoarseTune, slEditTone_PCMDrumKit_Pitch_PartialCoarseTune }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMDrumKit_Pitch_PartialFineTune, slEditTone_PCMDrumKit_Pitch_PartialFineTune }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth })).Row);

            // Set control values
            slEditTone_PCMDrumKit_Pitch_PartialCoarseTune.Value = (pCMDrumKit.pCMDrumKitPartial.PartialCoarseTune);
            tbEditTone_PCMDrumKit_Pitch_PartialCoarseTune.Text = "Partial Coarse Tune: " + ((pCMDrumKit.pCMDrumKitPartial.PartialCoarseTune)).ToString();
            slEditTone_PCMDrumKit_Pitch_PartialFineTune.Value = (pCMDrumKit.pCMDrumKitPartial.PartialFineTune - 64);
            tbEditTone_PCMDrumKit_Pitch_PartialFineTune.Text = "Partial Fine Tune: " + ((pCMDrumKit.pCMDrumKitPartial.PartialFineTune - 64)).ToString();
            cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.PartialRandomPitchDepth;
        }

        private void AddPCMDrumKitPitchEnvControls(byte SelectedIndex)
        {
            t.Trace("private void AddPCMDrumKitPitchEnvControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // Slider for Pitch Env Depth:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Pitch_PitchEnvDepth);
            Slider slEditTone_PCMDrumKit_Pitch_PitchEnvDepth = new Slider();
            slEditTone_PCMDrumKit_Pitch_PitchEnvDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Pitch_PitchEnvDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Pitch_PitchEnvDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Pitch_PitchEnvDepth.Name = "slEditTone_PCMDrumKit_Pitch_PitchEnvDepth";
            slEditTone_PCMDrumKit_Pitch_PitchEnvDepth.Minimum = -12;
            slEditTone_PCMDrumKit_Pitch_PitchEnvDepth.Maximum = 12;

            // Slider for Pitch Env Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens);
            Slider slEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens = new Slider();
            slEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens.Name = "slEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens";
            slEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens.Minimum = -63;
            slEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens.Maximum = 63;

            // Slider for Pitch Env Time 1 Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens);
            Slider slEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens = new Slider();
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens.Name = "slEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens";
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens.Minimum = -63;
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens.Maximum = 63;

            // Slider for Pitch Env Time 4 Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens);
            Slider slEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens = new Slider();
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens.Name = "slEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens";
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens.Minimum = -63;
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens.Maximum = 63;

            // Slider for Pitch Env Time:
            Slider[] slEditTone_PCMDrumKit_Pitch_PitchEnvTime = new Slider[4];
            for (byte i = 0; i < 4; i++)
            {
                tbEditTone_PCMDrumKit_Pitch_PitchEnvTime[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMDrumKit_Pitch_PitchEnvTime[i]);
                slEditTone_PCMDrumKit_Pitch_PitchEnvTime[i] = new Slider();
                slEditTone_PCMDrumKit_Pitch_PitchEnvTime[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMDrumKit_Pitch_PitchEnvTime[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMDrumKit_Pitch_PitchEnvTime[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMDrumKit_Pitch_PitchEnvTime[i].Name = "slEditTone_PCMDrumKit_Pitch_PitchEnvTime" + i.ToString();
                slEditTone_PCMDrumKit_Pitch_PitchEnvTime[i].Minimum = 0;
                slEditTone_PCMDrumKit_Pitch_PitchEnvTime[i].Maximum = 127;
            }

            // Slider for Pitch Env Level:
            Slider[] slEditTone_PCMDrumKit_Pitch_PitchEnvLevel = new Slider[5];
            for (byte i = 0; i < 5; i++)
            {
                tbEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i]);
                slEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i] = new Slider();
                slEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i].Name = "slEditTone_PCMDrumKit_Pitch_PitchEnvLevel" + i.ToString();
                slEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i].Minimum = -63;
                slEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i].Maximum = 63;
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { tbEditTone_PCMDrumKit_Pitch_PitchEnvDepth, slEditTone_PCMDrumKit_Pitch_PitchEnvDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens, slEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens, slEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens, slEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens }, new byte[] { 1, 2 })).Row);
            for (byte i = 0; i < 4; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(4 + i), new Control[] { tbEditTone_PCMDrumKit_Pitch_PitchEnvTime[i], slEditTone_PCMDrumKit_Pitch_PitchEnvTime[i] }, new byte[] { 1, 2 })).Row);
            }
            for (byte i = 0; i < 5; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(8 + i), new Control[] { tbEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i], slEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i] }, new byte[] { 1, 2 })).Row);
            }

            // Set control values
            slEditTone_PCMDrumKit_Pitch_PitchEnvDepth.Value = (pCMDrumKit.pCMDrumKitPartial.PitchEnv.PitchEnvDepth - 64);
            tbEditTone_PCMDrumKit_Pitch_PitchEnvDepth.Text = "Pitch Env Depth: " + ((pCMDrumKit.pCMDrumKitPartial.PitchEnv.PitchEnvDepth - 64)).ToString();
            slEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens.Value = (pCMDrumKit.pCMDrumKitPartial.PitchEnv.PitchEnvVelocitySens - 64);
            tbEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens.Text = "Pitch Env Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial.PitchEnv.PitchEnvVelocitySens - 64)).ToString();
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens.Value = (pCMDrumKit.pCMDrumKitPartial.PitchEnv.PitchEnvTime1VelocitySens - 64);
            tbEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens.Text = "Pitch Env Time 1 Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial.PitchEnv.PitchEnvTime1VelocitySens - 64)).ToString();
            slEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens.Value = (pCMDrumKit.pCMDrumKitPartial.PitchEnv.PitchEnvTime4VelocitySens - 64);
            tbEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens.Text = "Pitch Env Time 4 Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial.PitchEnv.PitchEnvTime4VelocitySens - 64)).ToString();
            for (byte i = 0; i < 4; i++)
            {
                slEditTone_PCMDrumKit_Pitch_PitchEnvTime[i].Value = (pCMDrumKit.pCMDrumKitPartial.PitchEnv.PitchEnvTime[i]);
                tbEditTone_PCMDrumKit_Pitch_PitchEnvTime[i].Text = "Pitch Env Time " + (i + 1).ToString() + ": " + (pCMDrumKit.pCMDrumKitPartial.PitchEnv.PitchEnvTime[i]).ToString();
            }
            for (byte i = 0; i < 5; i++)
            {
                slEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i].Value = (pCMDrumKit.pCMDrumKitPartial.PitchEnv.PitchEnvLevel[i] - 64);
                tbEditTone_PCMDrumKit_Pitch_PitchEnvLevel[i].Text = "Pitch Env Level " + i.ToString() + ": " + (pCMDrumKit.pCMDrumKitPartial.PitchEnv.PitchEnvLevel[i] - 64).ToString();
            }
        }

        private void AddPCMDrumKitTVFControls(byte SelectedIndex)
        {
            t.Trace("private void AddPCMDrumKitTVFControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;
            // ComboBox for Filter type
            ComboBox cbEditTone_PCMDrumKit_TVF_TVFFilterType = new ComboBox();
            cbEditTone_PCMDrumKit_TVF_TVFFilterType.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_TVF_TVFFilterType.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_TVF_TVFFilterType.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_TVF_TVFFilterType.Name = "cbEditTone_PCMDrumKit_TVF_TVFFilterType";
            cbEditTone_PCMDrumKit_TVF_TVFFilterType.Items.Add("No filter");
            cbEditTone_PCMDrumKit_TVF_TVFFilterType.Items.Add("Low pass filter");
            cbEditTone_PCMDrumKit_TVF_TVFFilterType.Items.Add("Band pass filter");
            cbEditTone_PCMDrumKit_TVF_TVFFilterType.Items.Add("High pass filter");
            cbEditTone_PCMDrumKit_TVF_TVFFilterType.Items.Add("Peaking filter");
            cbEditTone_PCMDrumKit_TVF_TVFFilterType.Items.Add("Warm LPF");
            cbEditTone_PCMDrumKit_TVF_TVFFilterType.Items.Add("Cutoff sensitive LPF");

            // Slider for TVF Cutoff Frequency:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVF_TVFCutoffFrequency);
            Slider slEditTone_PCMDrumKit_TVF_TVFCutoffFrequency = new Slider();
            slEditTone_PCMDrumKit_TVF_TVFCutoffFrequency.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVF_TVFCutoffFrequency.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVF_TVFCutoffFrequency.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVF_TVFCutoffFrequency.Name = "slEditTone_PCMDrumKit_TVF_TVFCutoffFrequency";
            slEditTone_PCMDrumKit_TVF_TVFCutoffFrequency.Minimum = 0;
            slEditTone_PCMDrumKit_TVF_TVFCutoffFrequency.Maximum = 127;

            // Slider for TVF Resonance:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVF_TVFResonance);
            Slider slEditTone_PCMDrumKit_TVF_TVFResonance = new Slider();
            slEditTone_PCMDrumKit_TVF_TVFResonance.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVF_TVFResonance.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVF_TVFResonance.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVF_TVFResonance.Name = "slEditTone_PCMDrumKit_TVF_TVFResonance";
            slEditTone_PCMDrumKit_TVF_TVFResonance.Minimum = 0;
            slEditTone_PCMDrumKit_TVF_TVFResonance.Maximum = 127;

            // ComboBox for TVF Cutoff Velocity Curve
            ComboBox cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve = new ComboBox();
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.Name = "cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve";
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.Items.Add("Velocity curve: Fixed");
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.Items.Add("Velocity curve: 1");
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.Items.Add("Velocity curve: 2");
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.Items.Add("Velocity curve: 3");
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.Items.Add("Velocity curve: 4");
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.Items.Add("Velocity curve: 5");
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.Items.Add("Velocity curve: 6");
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.Items.Add("Velocity curve: 7");

            // Slider for TVF Cutoff Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens);
            Slider slEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens = new Slider();
            slEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens.Name = "slEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens";
            slEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens.Minimum = -63;
            slEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens.Maximum = 63;

            // Slider for TVF Resonance Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens);
            Slider slEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens = new Slider();
            slEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens.Name = "slEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens";
            slEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens.Minimum = -63;
            slEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens.Maximum = 63;
            // ComboBox for TVF Env Velocity Curve Type
            ComboBox cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType = new ComboBox();
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.Name = "cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType";
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.Items.Add("Env velocity curve: Fixed");
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.Items.Add("Env velocity curve: 1");
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.Items.Add("Env velocity curve: 2");
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.Items.Add("Env velocity curve: 3");
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.Items.Add("Env velocity curve: 4");
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.Items.Add("Env velocity curve: 5");
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.Items.Add("Env velocity curve: 6");
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.Items.Add("Env velocity curve: 7");

            // Slider for TVF Env Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens);
            Slider slEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens = new Slider();
            slEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens.Name = "slEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens";
            slEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens.Minimum = -63;
            slEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens.Maximum = 63;

            // Slider for TVF Env Time 1 Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens);
            Slider slEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens = new Slider();
            slEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens.Name = "slEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens";
            slEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens.Minimum = -63;
            slEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens.Maximum = 63;

            // Slider for TVF Env Time 4 Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens);
            Slider slEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens = new Slider();
            slEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens.Name = "slEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens";
            slEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens.Minimum = -63;
            slEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens.Maximum = 63;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMDrumKit_TVF_TVFFilterType })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMDrumKit_TVF_TVFCutoffFrequency, slEditTone_PCMDrumKit_TVF_TVFCutoffFrequency }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMDrumKit_TVF_TVFResonance, slEditTone_PCMDrumKit_TVF_TVFResonance }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens, slEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens, slEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens, slEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens, slEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { tbEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens, slEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_PCMDrumKit_TVF_TVFFilterType.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.TVF.TVFFilterType;
            slEditTone_PCMDrumKit_TVF_TVFCutoffFrequency.Value = (pCMDrumKit.pCMDrumKitPartial.TVF.TVFCutoffFrequency);
            tbEditTone_PCMDrumKit_TVF_TVFCutoffFrequency.Text = "TVF Cutoff Frequency: " + ((pCMDrumKit.pCMDrumKitPartial.TVF.TVFCutoffFrequency)).ToString();
            slEditTone_PCMDrumKit_TVF_TVFResonance.Value = (pCMDrumKit.pCMDrumKitPartial.TVF.TVFResonance);
            tbEditTone_PCMDrumKit_TVF_TVFResonance.Text = "TVF Resonance: " + ((pCMDrumKit.pCMDrumKitPartial.TVF.TVFResonance)).ToString();
            cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.TVF.TVFCutoffVelocityCurve;
            slEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens.Value = (pCMDrumKit.pCMDrumKitPartial.TVF.TVFCutoffVelocitySens - 64);
            tbEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens.Text = "TVF Cutoff Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial.TVF.TVFCutoffVelocitySens - 64)).ToString();
            slEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens.Value = (pCMDrumKit.pCMDrumKitPartial.TVF.TVFResonanceVelocitySens - 64);
            tbEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens.Text = "TVF Resonance Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial.TVF.TVFResonanceVelocitySens - 64)).ToString();
            cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvVelocityCurve;
            slEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens.Value = (pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvVelocitySens - 64);
            tbEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens.Text = "TVF Env Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvVelocitySens - 64)).ToString();
            slEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens.Value = (pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvTime1VelocitySens - 64);
            tbEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens.Text = "TVF Env Time 1 Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvTime1VelocitySens - 64)).ToString();
            slEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens.Value = (pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvTime4VelocitySens - 64);
            tbEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens.Text = "TVF Env Time 4 Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvTime4VelocitySens - 64)).ToString();
        }

        private void AddPCMDrumKitTVFEnvControls(byte SelectedIndex)
        {
            t.Trace("private void AddPCMDrumKitTVFEnvControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // Slider for TVF Env Depth:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVFenv_TVFEnvDepth);
            Slider slEditTone_PCMDrumKit_TVFenv_TVFEnvDepth = new Slider();
            slEditTone_PCMDrumKit_TVFenv_TVFEnvDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVFenv_TVFEnvDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVFenv_TVFEnvDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVFenv_TVFEnvDepth.Name = "slEditTone_PCMDrumKit_TVFenv_TVFEnvDepth";
            slEditTone_PCMDrumKit_TVFenv_TVFEnvDepth.Minimum = -63;
            slEditTone_PCMDrumKit_TVFenv_TVFEnvDepth.Maximum = 63;

            // Slider for TVF Env Time:
            Slider[] slEditTone_PCMDrumKit_TVFenv_TVFEnvTime = new Slider[4];
            for (byte i = 0; i < 4; i++)
            {
                tbEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i]);
                slEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i] = new Slider();
                slEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i].Name = "slEditTone_PCMDrumKit_TVFenv_TVFEnvTime" + i.ToString();
                slEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i].Minimum = 0;
                slEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i].Maximum = 127;
            }

            // Slider for TVF Env Level:
            Slider[] slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel = new Slider[5];
            for (byte i = 0; i < 5; i++)
            {
                tbEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i]);
                slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i] = new Slider();
                slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i].Name = "slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel" + i.ToString();
                slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i].Minimum = 0;
                slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i].Maximum = 127;
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { tbEditTone_PCMDrumKit_TVFenv_TVFEnvDepth, slEditTone_PCMDrumKit_TVFenv_TVFEnvDepth }, new byte[] { 1, 2 })).Row);
            for (byte i = 0; i < 4; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(1 + i), new Control[] { tbEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i], slEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i] }, new byte[] { 1, 2 })).Row);
            }
            for (byte i = 0; i < 5; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(5 + i), new Control[] { tbEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i], slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i] }, new byte[] { 1, 2 })).Row);
            }

            // Set control values
            slEditTone_PCMDrumKit_TVFenv_TVFEnvDepth.Value = (pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvDepth - 64);
            tbEditTone_PCMDrumKit_TVFenv_TVFEnvDepth.Text = "TVF Env Depth: " + ((pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvDepth - 64)).ToString();
            for (byte i = 0; i < 4; i++)
            {
                slEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i].Value = (pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvTime[i]);
                tbEditTone_PCMDrumKit_TVFenv_TVFEnvTime[i].Text = "TVF Env Time " + (i + 1).ToString() + ": " + ((pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvTime[i])).ToString();
            }
            for (byte i = 0; i < 5; i++)
            {
                slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i].Value = (pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvLevel[i]);
                tbEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[i].Text = "TVF Env Level " + i.ToString() + ": " + ((pCMDrumKit.pCMDrumKitPartial.TVF.TVFEnvLevel[i])).ToString();
            }
        }

        private void AddPCMDrumKitTVAControls(byte SelectedIndex)
        {
            t.Trace("private void AddPCMDrumKitTVAControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // Slider for Partial Level:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_PartialLevel);
            Slider slEditTone_PCMDrumKit_TVA_PartialLevel = new Slider();
            slEditTone_PCMDrumKit_TVA_PartialLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVA_PartialLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVA_PartialLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVA_PartialLevel.Name = "slEditTone_PCMDrumKit_TVA_PartialLevel";
            slEditTone_PCMDrumKit_TVA_PartialLevel.Minimum = 0;
            slEditTone_PCMDrumKit_TVA_PartialLevel.Maximum = 127;

            // ComboBox for TVA Level Velocity Curve
            ComboBox cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve = new ComboBox();
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.Name = "cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve";
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.Items.Add("Velocity curve: Fixed");
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.Items.Add("Velocity curve: 1");
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.Items.Add("Velocity curve: 2");
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.Items.Add("Velocity curve: 3");
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.Items.Add("Velocity curve: 4");
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.Items.Add("Velocity curve: 5");
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.Items.Add("Velocity curve: 6");
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.Items.Add("Velocity curve: 7");

            // Slider for TVA Level Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_TVALevelVelocitySens);
            Slider slEditTone_PCMDrumKit_TVA_TVALevelVelocitySens = new Slider();
            slEditTone_PCMDrumKit_TVA_TVALevelVelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVA_TVALevelVelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVA_TVALevelVelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVA_TVALevelVelocitySens.Name = "slEditTone_PCMDrumKit_TVA_TVALevelVelocitySens";
            slEditTone_PCMDrumKit_TVA_TVALevelVelocitySens.Minimum = -63;
            slEditTone_PCMDrumKit_TVA_TVALevelVelocitySens.Maximum = 63;

            // Slider for Partial Pan:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_PartialPan);
            Slider slEditTone_PCMDrumKit_TVA_PartialPan = new Slider();
            slEditTone_PCMDrumKit_TVA_PartialPan.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVA_PartialPan.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVA_PartialPan.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVA_PartialPan.Name = "slEditTone_PCMDrumKit_TVA_PartialPan";
            slEditTone_PCMDrumKit_TVA_PartialPan.Minimum = -64;
            slEditTone_PCMDrumKit_TVA_PartialPan.Maximum = 63;

            // Slider for Partial Random Pan Depth:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_PartialRandomPanDepth);
            Slider slEditTone_PCMDrumKit_TVA_PartialRandomPanDepth = new Slider();
            slEditTone_PCMDrumKit_TVA_PartialRandomPanDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVA_PartialRandomPanDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVA_PartialRandomPanDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVA_PartialRandomPanDepth.Name = "slEditTone_PCMDrumKit_TVA_PartialRandomPanDepth";
            slEditTone_PCMDrumKit_TVA_PartialRandomPanDepth.Minimum = 0;
            slEditTone_PCMDrumKit_TVA_PartialRandomPanDepth.Maximum = 63;

            // Slider for Partial Alternate Pan Depth:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth);
            Slider slEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth = new Slider();
            slEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth.Name = "slEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth";
            slEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth.Minimum = -63;
            slEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth.Maximum = 63;

            // Slider for Relative Level:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_RelativeLevel);
            Slider slEditTone_PCMDrumKit_TVA_RelativeLevel = new Slider();
            slEditTone_PCMDrumKit_TVA_RelativeLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVA_RelativeLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVA_RelativeLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVA_RelativeLevel.Name = "slEditTone_PCMDrumKit_TVA_RelativeLevel";
            slEditTone_PCMDrumKit_TVA_RelativeLevel.Minimum = -64;
            slEditTone_PCMDrumKit_TVA_RelativeLevel.Maximum = 63;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { tbEditTone_PCMDrumKit_TVA_PartialLevel, slEditTone_PCMDrumKit_TVA_PartialLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMDrumKit_TVA_TVALevelVelocitySens, slEditTone_PCMDrumKit_TVA_TVALevelVelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMDrumKit_TVA_PartialPan, slEditTone_PCMDrumKit_TVA_PartialPan }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_PCMDrumKit_TVA_PartialRandomPanDepth, slEditTone_PCMDrumKit_TVA_PartialRandomPanDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth, slEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_PCMDrumKit_TVA_RelativeLevel, slEditTone_PCMDrumKit_TVA_RelativeLevel }, new byte[] { 1, 2 })).Row);

            // Set control values
            slEditTone_PCMDrumKit_TVA_PartialLevel.Value = (pCMDrumKit.pCMDrumKitPartial.PartialLevel);
            tbEditTone_PCMDrumKit_TVA_PartialLevel.Text = "Partial Level: " + ((pCMDrumKit.pCMDrumKitPartial.PartialLevel)).ToString();
            cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.TVA.TVALevelVelocityCurve;
            slEditTone_PCMDrumKit_TVA_TVALevelVelocitySens.Value = (pCMDrumKit.pCMDrumKitPartial.TVA.TVALevelVelocitySens - 64);
            tbEditTone_PCMDrumKit_TVA_TVALevelVelocitySens.Text = "TVA Level Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial.TVA.TVALevelVelocitySens - 64)).ToString();
            slEditTone_PCMDrumKit_TVA_PartialPan.Value = (pCMDrumKit.pCMDrumKitPartial.PartialPan - 64);
            tbEditTone_PCMDrumKit_TVA_PartialPan.Text = "Partial Pan: " + ((pCMDrumKit.pCMDrumKitPartial.PartialPan - 64)).ToString();
            slEditTone_PCMDrumKit_TVA_PartialRandomPanDepth.Value = (pCMDrumKit.pCMDrumKitPartial.PartialRandomPanDepth);
            tbEditTone_PCMDrumKit_TVA_PartialRandomPanDepth.Text = "Partial Random Pan Depth: " + ((pCMDrumKit.pCMDrumKitPartial.PartialRandomPanDepth)).ToString();
            slEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth.Value = (pCMDrumKit.pCMDrumKitPartial.PartialAlternatePanDepth - 64);
            tbEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth.Text = "Partial Alternate Pan Depth: " + ((pCMDrumKit.pCMDrumKitPartial.PartialAlternatePanDepth - 64)).ToString();
            slEditTone_PCMDrumKit_TVA_RelativeLevel.Value = (pCMDrumKit.pCMDrumKitPartial.RelativeLevel - 64);
            tbEditTone_PCMDrumKit_TVA_RelativeLevel.Text = "Relative Level: " + ((pCMDrumKit.pCMDrumKitPartial.RelativeLevel - 64)).ToString();
        }

        private void AddPCMDrumKitTVAEnvControls(byte SelectedIndex)
        {
            t.Trace("private void AddPCMDrumKitTVAEnvControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // Slider for TVA Env Time 1 Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens);
            Slider slEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens = new Slider();
            slEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens.Name = "slEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens";
            slEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens.Minimum = -63;
            slEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens.Maximum = 63;

            // Slider for TVA Env Time 2 Velocity Sens:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens);
            Slider slEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens = new Slider();
            slEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens.Name = "slEditTone_PCMDrumKit_TVA_TVAEnvTime2VelocitySens";
            slEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens.Minimum = -63;
            slEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens.Maximum = 63;

            // Slider for TVA Env Time:
            Slider[] slEditTone_PCMDrumKit_TVA_TVAEnvTime = new Slider[4];
            for (byte i = 0; i < 4; i++)
            {
                tbEditTone_PCMDrumKit_TVA_TVAEnvTime[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_TVAEnvTime[i]);
                slEditTone_PCMDrumKit_TVA_TVAEnvTime[i] = new Slider();
                slEditTone_PCMDrumKit_TVA_TVAEnvTime[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMDrumKit_TVA_TVAEnvTime[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMDrumKit_TVA_TVAEnvTime[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMDrumKit_TVA_TVAEnvTime[i].Name = "slEditTone_PCMDrumKit_TVA_TVAEnvTime" + i.ToString();
                slEditTone_PCMDrumKit_TVA_TVAEnvTime[i].Minimum = 0;
                slEditTone_PCMDrumKit_TVA_TVAEnvTime[i].Maximum = 127;
            }

            // Slider for TVA Env Level:
            Slider[] slEditTone_PCMDrumKit_TVA_TVAEnvLevel = new Slider[5];
            for (byte i = 0; i < 5; i++)
            {
                tbEditTone_PCMDrumKit_TVA_TVAEnvLevel[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_TVAEnvLevel[i]);
                slEditTone_PCMDrumKit_TVA_TVAEnvLevel[i] = new Slider();
                slEditTone_PCMDrumKit_TVA_TVAEnvLevel[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMDrumKit_TVA_TVAEnvLevel[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMDrumKit_TVA_TVAEnvLevel[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMDrumKit_TVA_TVAEnvLevel[i].Name = "slEditTone_PCMDrumKit_TVA_TVAEnvLevel" + i.ToString();
                slEditTone_PCMDrumKit_TVA_TVAEnvLevel[i].Minimum = 0;
                slEditTone_PCMDrumKit_TVA_TVAEnvLevel[i].Maximum = 127;
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { tbEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens, slEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens, slEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens }, new byte[] { 1, 2 })).Row);
            for (byte i = 0; i < 4; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(2 + i), new Control[] { tbEditTone_PCMDrumKit_TVA_TVAEnvTime[i], slEditTone_PCMDrumKit_TVA_TVAEnvTime[i] }, new byte[] { 1, 2 })).Row);
            }
            for (byte i = 0; i < 3; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(6 + i), new Control[] { tbEditTone_PCMDrumKit_TVA_TVAEnvLevel[i], slEditTone_PCMDrumKit_TVA_TVAEnvLevel[i] }, new byte[] { 1, 2 })).Row);
            }

            // Set control values
            slEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens.Value = (pCMDrumKit.pCMDrumKitPartial.TVA.TVAEnvTime1VelocitySens - 64);
            tbEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens.Text = "TVA Env Time 1 Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial.TVA.TVAEnvTime1VelocitySens - 64)).ToString();
            slEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens.Value = (pCMDrumKit.pCMDrumKitPartial.TVA.TVAEnvTime4VelocitySens - 64);
            tbEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens.Text = "TVA Env Time 2 Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial.TVA.TVAEnvTime4VelocitySens - 64)).ToString();
            for (byte i = 0; i < 4; i++)
            {
                slEditTone_PCMDrumKit_TVA_TVAEnvTime[i].Value = (pCMDrumKit.pCMDrumKitPartial.TVA.TVAEnvTime[i]);
                tbEditTone_PCMDrumKit_TVA_TVAEnvTime[i].Text = "TVA Env Time " + (i + 1).ToString() + ": " + ((pCMDrumKit.pCMDrumKitPartial.TVA.TVAEnvTime[i])).ToString();
            }
            for (byte i = 0; i < 3; i++)
            {
                slEditTone_PCMDrumKit_TVA_TVAEnvLevel[i].Value = (pCMDrumKit.pCMDrumKitPartial.TVA.TVAEnvLevel[i]);
                tbEditTone_PCMDrumKit_TVA_TVAEnvLevel[i].Text = "TVA Env Level " + (i + 1).ToString() + ": " + ((pCMDrumKit.pCMDrumKitPartial.TVA.TVAEnvLevel[i])).ToString();
            }
        }

        private void AddPCMDrumKitOutputControls(byte SelectedIndex)
        {
            t.Trace("private void AddPCMDrumKitOutputControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // ComboBox for Partial Output Assign
            ComboBox cbEditTone_PCMDrumKit_TVA_PartialOutputAssign = new ComboBox();
            cbEditTone_PCMDrumKit_TVA_PartialOutputAssign.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_PCMDrumKit_TVA_PartialOutputAssign.GotFocus += Generic_GotFocus;
            cbEditTone_PCMDrumKit_TVA_PartialOutputAssign.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_PCMDrumKit_TVA_PartialOutputAssign.Name = "cbEditTone_PCMDrumKit_TVA_PartialOutputAssign";
            cbEditTone_PCMDrumKit_TVA_PartialOutputAssign.Items.Add("Output assign: Part");
            cbEditTone_PCMDrumKit_TVA_PartialOutputAssign.Items.Add("Output assign: Comp+Eq1");
            cbEditTone_PCMDrumKit_TVA_PartialOutputAssign.Items.Add("Output assign: Comp+Eq2");
            cbEditTone_PCMDrumKit_TVA_PartialOutputAssign.Items.Add("Output assign: Comp+Eq3");
            cbEditTone_PCMDrumKit_TVA_PartialOutputAssign.Items.Add("Output assign: Comp+Eq4");
            cbEditTone_PCMDrumKit_TVA_PartialOutputAssign.Items.Add("Output assign: Comp+Eq5");
            cbEditTone_PCMDrumKit_TVA_PartialOutputAssign.Items.Add("Output assign: Comp+Eq6");

            // Slider for Partial Output Level:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_PartialOutputLevel);
            Slider slEditTone_PCMDrumKit_TVA_PartialOutputLevel = new Slider();
            slEditTone_PCMDrumKit_TVA_PartialOutputLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVA_PartialOutputLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVA_PartialOutputLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVA_PartialOutputLevel.Name = "slEditTone_PCMDrumKit_TVA_PartialOutputLevel";
            slEditTone_PCMDrumKit_TVA_PartialOutputLevel.Minimum = 0;
            slEditTone_PCMDrumKit_TVA_PartialOutputLevel.Maximum = 127;

            // Slider for Partial Chorus Send Level:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_PartialChorusSendLevel);
            Slider slEditTone_PCMDrumKit_TVA_PartialChorusSendLevel = new Slider();
            slEditTone_PCMDrumKit_TVA_PartialChorusSendLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVA_PartialChorusSendLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVA_PartialChorusSendLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVA_PartialChorusSendLevel.Name = "slEditTone_PCMDrumKit_TVA_PartialChorusSendLevel";
            slEditTone_PCMDrumKit_TVA_PartialChorusSendLevel.Minimum = 0;
            slEditTone_PCMDrumKit_TVA_PartialChorusSendLevel.Maximum = 127;

            // Slider for Partial Reverb Send Level:
            SetLabelProperties(ref tbEditTone_PCMDrumKit_TVA_PartialReverbSendLevel);
            Slider slEditTone_PCMDrumKit_TVA_PartialReverbSendLevel = new Slider();
            slEditTone_PCMDrumKit_TVA_PartialReverbSendLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_PCMDrumKit_TVA_PartialReverbSendLevel.GotFocus += Generic_GotFocus;
            slEditTone_PCMDrumKit_TVA_PartialReverbSendLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_PCMDrumKit_TVA_PartialReverbSendLevel.Name = "slEditTone_PCMDrumKit_TVA_PartialReverbSendLevel";
            slEditTone_PCMDrumKit_TVA_PartialReverbSendLevel.Minimum = 0;
            slEditTone_PCMDrumKit_TVA_PartialReverbSendLevel.Maximum = 127;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_PCMDrumKit_TVA_PartialOutputAssign })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMDrumKit_TVA_PartialOutputLevel, slEditTone_PCMDrumKit_TVA_PartialOutputLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMDrumKit_TVA_PartialChorusSendLevel, slEditTone_PCMDrumKit_TVA_PartialChorusSendLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMDrumKit_TVA_PartialReverbSendLevel, slEditTone_PCMDrumKit_TVA_PartialReverbSendLevel }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_PCMDrumKit_TVA_PartialOutputAssign.SelectedIndex = pCMDrumKit.pCMDrumKitPartial.PartialOutputAssign;
            slEditTone_PCMDrumKit_TVA_PartialOutputLevel.Value = (pCMDrumKit.pCMDrumKitPartial.PartialOutputLevel);
            tbEditTone_PCMDrumKit_TVA_PartialOutputLevel.Text = "Partial Output Level: " + ((pCMDrumKit.pCMDrumKitPartial.PartialOutputLevel)).ToString();
            slEditTone_PCMDrumKit_TVA_PartialChorusSendLevel.Value = (pCMDrumKit.pCMDrumKitPartial.PartialChorusSendLevel);
            tbEditTone_PCMDrumKit_TVA_PartialChorusSendLevel.Text = "Partial Chorus Send Level: " + ((pCMDrumKit.pCMDrumKitPartial.PartialChorusSendLevel)).ToString();
            slEditTone_PCMDrumKit_TVA_PartialReverbSendLevel.Value = (pCMDrumKit.pCMDrumKitPartial.PartialReverbSendLevel);
            tbEditTone_PCMDrumKit_TVA_PartialReverbSendLevel.Text = "Partial Reverb Send Level: " + ((pCMDrumKit.pCMDrumKitPartial.PartialReverbSendLevel)).ToString();
        }

        private void AddPCMDrumKitCompressorControls(byte SelectedIndex)
        {
            t.Trace("private void AddPCMDrumKitCompressorControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;
            // CheckBox for Comp Switch
            CheckBox[] cbEditTone_PCMDrumKit_CompEq_CompSwitch = new CheckBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_CompEq_CompSwitch[i] = new CheckBox();
                cbEditTone_PCMDrumKit_CompEq_CompSwitch[i].Tapped += GenericCheckBox_Click;
                cbEditTone_PCMDrumKit_CompEq_CompSwitch[i].Click += GenericCheckBox_Click;
                cbEditTone_PCMDrumKit_CompEq_CompSwitch[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMDrumKit_CompEq_CompSwitch[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMDrumKit_CompEq_CompSwitch[i].Content = "Comp Switch" + (i + 1).ToString();
                cbEditTone_PCMDrumKit_CompEq_CompSwitch[i].Name = "cbEditTone_PCMDrumKit_CompSwitch" + i.ToString();

            }

            // ComboBox for Comp Attack Time
            ComboBox[] cbEditTone_PCMDrumKit_Compressor_CompAttackTime = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i] = new ComboBox();
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Name = "cbEditTone_PCMDrumKit_Compressor_CompAttackTime" + i.ToString();
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.05 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.06 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.07 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.08 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.09 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.1 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.2 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.3 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.4 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.5 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.6 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.7 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.8 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.9 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 1.0 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 2.0 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 3.0 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 4.0 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 5.0 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 6.0 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 7.0 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 8.0 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 9.0 ms");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 10.0 mS");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 15.0 mS");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 20.0 mS");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 25.0 mS");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 30.0 mS");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 35.0 mS");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 40.0 mS");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 45.0 mS");
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 50.0 mS");
            }

            // ComboBox for Comp Release Time
            ComboBox[] cbEditTone_PCMDrumKit_Compressor_CompReleaseTime = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i] = new ComboBox();
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Name = "cbEditTone_PCMDrumKit_Compressor_CompReleaseTime" + i.ToString() + "";
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 0.05 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 0.07 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 0.1 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 0.5 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 1 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 5 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 10 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 17 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 25 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 50 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 75 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 100 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 200 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 300 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 400 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 500 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 600 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 700 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 800 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 900 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 1000 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 1200 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 1500 ms");
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 2000 ms");
            }

            // Slider for Comp Threshold:
            Slider[] slEditTone_PCMDrumKit_Compressor_CompThreshold = new Slider[6];
            for (byte i = 0; i < 6; i++)
            {
                tbEditTone_PCMDrumKit_Compressor_CompThreshold[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMDrumKit_Compressor_CompThreshold[i]);
                slEditTone_PCMDrumKit_Compressor_CompThreshold[i] = new Slider();
                slEditTone_PCMDrumKit_Compressor_CompThreshold[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMDrumKit_Compressor_CompThreshold[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMDrumKit_Compressor_CompThreshold[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMDrumKit_Compressor_CompThreshold[i].Name = "slEditTone_PCMDrumKit_Compressor_CompThreshold" + i.ToString();
                slEditTone_PCMDrumKit_Compressor_CompThreshold[i].Minimum = 0;
                slEditTone_PCMDrumKit_Compressor_CompThreshold[i].Maximum = 127;
            }

            // ComboBox for Comp Ratio
            ComboBox[] cbEditTone_PCMDrumKit_Compressor_CompRatio = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i] = new ComboBox();
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Name = "cbEditTone_PCMDrumKit_Compressor_CompRatio" + i.ToString() + "";
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:1");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:2");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:3");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:4");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:5");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:6");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:7");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:8");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:9");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:10");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:20");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:30");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:40");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:50");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:60");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:70");
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].Items.Add("Ratio: 1:80");
            }


            // Slider for Comp Output Gain:
            Slider[] slEditTone_PCMDrumKit_Compressor_CompOutputGain = new Slider[6];
            for (byte i = 0; i < 6; i++)
            {
                tbEditTone_PCMDrumKit_Compressor_CompOutputGain[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMDrumKit_Compressor_CompOutputGain[i]);
                slEditTone_PCMDrumKit_Compressor_CompOutputGain[i] = new Slider();
                slEditTone_PCMDrumKit_Compressor_CompOutputGain[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMDrumKit_Compressor_CompOutputGain[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMDrumKit_Compressor_CompOutputGain[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMDrumKit_Compressor_CompOutputGain[i].Name = "slEditTone_PCMDrumKit_Compressor_CompOutputGain" + i.ToString();
                slEditTone_PCMDrumKit_Compressor_CompOutputGain[i].Minimum = 0;
                slEditTone_PCMDrumKit_Compressor_CompOutputGain[i].Maximum = 24;
            }

            // Put in rows
            for (byte i = 0; i < 6; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(0 + i / 3), new Control[] { cbEditTone_PCMDrumKit_CompEq_CompSwitch[i++],
                cbEditTone_PCMDrumKit_CompEq_CompSwitch[i++], cbEditTone_PCMDrumKit_CompEq_CompSwitch[i]})).Row);
            }
            for (byte i = 0; i < 6; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(2 + i / 3), new Control[] { cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i++],
                    cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i++], cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i] })).Row);
            }
            for (byte i = 0; i < 6; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(4 + i / 3), new Control[] { cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i++],
                    cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i++], cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i] })).Row);
            }
            for (byte i = 0; i < 6; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(6 + i / 2), new Control[] { tbEditTone_PCMDrumKit_Compressor_CompThreshold[i], slEditTone_PCMDrumKit_Compressor_CompThreshold[i++],
                    //tbEditTone_PCMDrumKit_Compressor_CompThreshold[i], slEditTone_PCMDrumKit_Compressor_CompThreshold[i++],
                    tbEditTone_PCMDrumKit_Compressor_CompThreshold[i], slEditTone_PCMDrumKit_Compressor_CompThreshold[i],}, new byte[] { 1, 2, 1, 2 })).Row);
            }
            for (byte i = 0; i < 6; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(9 + i / 3), new Control[] { cbEditTone_PCMDrumKit_Compressor_CompRatio[i++],
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i++], cbEditTone_PCMDrumKit_Compressor_CompRatio[i]})).Row);
            }
            for (byte i = 0; i < 6; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(11 + i / 2), new Control[] { tbEditTone_PCMDrumKit_Compressor_CompOutputGain[i], slEditTone_PCMDrumKit_Compressor_CompOutputGain[i++],
                 tbEditTone_PCMDrumKit_Compressor_CompOutputGain[i], slEditTone_PCMDrumKit_Compressor_CompOutputGain[i]}, new byte[] { 1, 2, 1, 2 })).Row);
            }

            // Set control values
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_CompEq_CompSwitch[i].IsChecked = pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].CompSwitch;
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Compressor_CompAttackTime[i].SelectedIndex = pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].CompAttackTime;
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Compressor_CompReleaseTime[i].SelectedIndex = pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].CompReleaseTime;
            }
            for (byte i = 0; i < 6; i++)
            {
                slEditTone_PCMDrumKit_Compressor_CompThreshold[i].Value = (pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].CompThreshold);
                tbEditTone_PCMDrumKit_Compressor_CompThreshold[i].Text = "Thresh: " + (i + 1).ToString() + ": " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].CompThreshold)).ToString();
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Compressor_CompRatio[i].SelectedIndex = pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].CompRatio;
            }
            for (byte i = 0; i < 6; i++)
            {
                slEditTone_PCMDrumKit_Compressor_CompOutputGain[i].Value = (pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].CompOutputGain);
                tbEditTone_PCMDrumKit_Compressor_CompOutputGain[i].Text = "Gain: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].CompOutputGain)).ToString() + " dB";
            }
        }

        private void AddPCMDrumKitEqualizerControls(byte SelectedIndex)
        {
            t.Trace("private void AddPCMDrumKitEqualizerControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;
            // CheckBox for Eq Switch
            CheckBox[] cbEditTone_PCMDrumKit_Equalizer_EqSwitch = new CheckBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Equalizer_EqSwitch[i] = new CheckBox();
                cbEditTone_PCMDrumKit_Equalizer_EqSwitch[i].Tapped += GenericCheckBox_Click;
                cbEditTone_PCMDrumKit_Equalizer_EqSwitch[i].Click += GenericCheckBox_Click;
                cbEditTone_PCMDrumKit_Equalizer_EqSwitch[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMDrumKit_Equalizer_EqSwitch[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMDrumKit_Equalizer_EqSwitch[i].Content = "Eq " + (i + 1).ToString();
                cbEditTone_PCMDrumKit_Equalizer_EqSwitch[i].Name = "cbEditTone_PCMDrumKit_EQSwitch" + i.ToString();
            }

            TextBox tbEditTone_PCMDrumKit_Equalizer_SwitchesHeader = new TextBox();
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Equalizer_SwitchesHeader);
            tbEditTone_PCMDrumKit_Equalizer_SwitchesHeader.Text = "Switches";
            TextBox tbEditTone_PCMDrumKit_Equalizer_FrequencyHeader = new TextBox();
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Equalizer_FrequencyHeader);
            tbEditTone_PCMDrumKit_Equalizer_FrequencyHeader.Text = "Equalizer frequency bands and mid Q";
            tbEditTone_PCMDrumKit_Equalizer_FrequencyHeader.TextAlignment = TextAlignment.Left;
            TextBox[] tbEditTone_PCMDrumKit_Equalizer_Frequencies = new TextBox[4];
            tbEditTone_PCMDrumKit_Equalizer_Frequencies[0] = new TextBox();
            tbEditTone_PCMDrumKit_Equalizer_Frequencies[1] = new TextBox();
            tbEditTone_PCMDrumKit_Equalizer_Frequencies[2] = new TextBox();
            tbEditTone_PCMDrumKit_Equalizer_Frequencies[3] = new TextBox();
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Equalizer_Frequencies[0]);
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Equalizer_Frequencies[1]);
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Equalizer_Frequencies[2]);
            SetLabelProperties(ref tbEditTone_PCMDrumKit_Equalizer_Frequencies[3]);
            tbEditTone_PCMDrumKit_Equalizer_Frequencies[0].Text = "Low";
            tbEditTone_PCMDrumKit_Equalizer_Frequencies[1].Text = "Mid";
            tbEditTone_PCMDrumKit_Equalizer_Frequencies[2].Text = "MidQ";
            tbEditTone_PCMDrumKit_Equalizer_Frequencies[3].Text = "High";

            String[] freqs;

            // ComboBox for EQ Low Freq
            ComboBox[] cbEditTone_PCMDrumKit_Equalizer_EQLowFreq = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[i] = new ComboBox();
                cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[i].Name = "cbEditTone_PCMDrumKit_Equalizer_EQLowFreq" + i.ToString() + "";
                freqs = parameterSets.GetNumberedParameter(PARAMETER_TYPE.COMBOBOX_LOW_FREQ);
                foreach (String freq in freqs)
                {
                    cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[i].Items.Add(freq);
                }
            }

            // ComboBox for EQ Mid Freq
            ComboBox[] cbEditTone_PCMDrumKit_Equalizer_EQMidFreq = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[i] = new ComboBox();
                cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[i].Name = "cbEditTone_PCMDrumKit_Equalizer_EQMidFreq" + i.ToString() + "";
                freqs = parameterSets.GetNumberedParameter(PARAMETER_TYPE.COMBOBOX_MID_FREQ);
                foreach (String freq in freqs)
                {
                    cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[i].Items.Add(freq);
                }
            }

            // ComboBox for EQ Mid Q
            ComboBox[] cbEditTone_PCMDrumKit_Equalizer_EQMidQ = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[i] = new ComboBox();
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[i].Name = "cbEditTone_PCMDrumKit_Equalizer_EQMidQ" + i.ToString() + "";
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[i].Items.Add("0.5");
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[i].Items.Add("1.0");
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[i].Items.Add("2.0");
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[i].Items.Add("4.0");
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[i].Items.Add("8.0");
            }

            // ComboBox for EQ High Freq
            ComboBox[] cbEditTone_PCMDrumKit_Equalizer_EQHighFreq = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[i] = new ComboBox();
                cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[i].Name = "cbEditTone_PCMDrumKit_Equalizer_EQHighFreq" + i.ToString() + "";
                freqs = parameterSets.GetNumberedParameter(PARAMETER_TYPE.COMBOBOX_HIGH_FREQ);
                foreach (String freq in freqs)
                {
                    cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[i].Items.Add(freq);
                }
            }

            // Slider for EQ Low Gain:
            Slider[] slEditTone_PCMDrumKit_Equalizer_EQLowGain = new Slider[6];
            for (byte i = 0; i < 6; i++)
            {
                tbEditTone_PCMDrumKit_Equalizer_EQLowGain[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMDrumKit_Equalizer_EQLowGain[i]);
                slEditTone_PCMDrumKit_Equalizer_EQLowGain[i] = new Slider();
                slEditTone_PCMDrumKit_Equalizer_EQLowGain[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMDrumKit_Equalizer_EQLowGain[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMDrumKit_Equalizer_EQLowGain[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMDrumKit_Equalizer_EQLowGain[i].Name = "slEditTone_PCMDrumKit_Equalizer_EQLowGain" + i.ToString();
                slEditTone_PCMDrumKit_Equalizer_EQLowGain[i].Minimum = -15;
                slEditTone_PCMDrumKit_Equalizer_EQLowGain[i].Maximum = 15;
            }

            // Slider for EQ Mid Gain:
            Slider[] slEditTone_PCMDrumKit_Equalizer_EQMidGain = new Slider[6];
            for (byte i = 0; i < 6; i++)
            {
                tbEditTone_PCMDrumKit_Equalizer_EQMidGain[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMDrumKit_Equalizer_EQMidGain[i]);
                slEditTone_PCMDrumKit_Equalizer_EQMidGain[i] = new Slider();
                slEditTone_PCMDrumKit_Equalizer_EQMidGain[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMDrumKit_Equalizer_EQMidGain[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMDrumKit_Equalizer_EQMidGain[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMDrumKit_Equalizer_EQMidGain[i].Name = "slEditTone_PCMDrumKit_Equalizer_EQMidGain" + i.ToString();
                slEditTone_PCMDrumKit_Equalizer_EQMidGain[i].Minimum = -15;
                slEditTone_PCMDrumKit_Equalizer_EQMidGain[i].Maximum = 15;
            }

            // Slider for EQ High Gain:
            Slider[] slEditTone_PCMDrumKit_Equalizer_EQHighGain = new Slider[6];
            for (byte i = 0; i < 6; i++)
            {
                tbEditTone_PCMDrumKit_Equalizer_EQHighGain[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMDrumKit_Equalizer_EQHighGain[i]);
                slEditTone_PCMDrumKit_Equalizer_EQHighGain[i] = new Slider();
                slEditTone_PCMDrumKit_Equalizer_EQHighGain[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMDrumKit_Equalizer_EQHighGain[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMDrumKit_Equalizer_EQHighGain[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMDrumKit_Equalizer_EQHighGain[i].Name = "slEditTone_PCMDrumKit_Equalizer_EQHighGain" + i.ToString();
                slEditTone_PCMDrumKit_Equalizer_EQHighGain[i].Minimum = -15;
                slEditTone_PCMDrumKit_Equalizer_EQHighGain[i].Maximum = 15;
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { tbEditTone_PCMDrumKit_Equalizer_SwitchesHeader,
                cbEditTone_PCMDrumKit_Equalizer_EqSwitch[0], cbEditTone_PCMDrumKit_Equalizer_EqSwitch[1],
                cbEditTone_PCMDrumKit_Equalizer_EqSwitch[2], cbEditTone_PCMDrumKit_Equalizer_EqSwitch[3],
                cbEditTone_PCMDrumKit_Equalizer_EqSwitch[4], cbEditTone_PCMDrumKit_Equalizer_EqSwitch[5] }, new byte[] { 1, 1, 1, 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_PCMDrumKit_Equalizer_FrequencyHeader })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_PCMDrumKit_Equalizer_Frequencies[0],
                cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[0], cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[1],
                cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[2], cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[3],
                cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[4], cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[5] }, new byte[] { 1, 1, 1, 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_PCMDrumKit_Equalizer_Frequencies[1],
                cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[0], cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[1],
                cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[2], cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[3],
                cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[4], cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[5] }, new byte[] { 1, 1, 1, 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_PCMDrumKit_Equalizer_Frequencies[2],
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[0], cbEditTone_PCMDrumKit_Equalizer_EQMidQ[1],
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[2], cbEditTone_PCMDrumKit_Equalizer_EQMidQ[3],
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[4], cbEditTone_PCMDrumKit_Equalizer_EQMidQ[5] }, new byte[] { 1, 1, 1, 1, 1, 1, 1 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_PCMDrumKit_Equalizer_Frequencies[3],
                cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[0], cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[1],
                cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[2], cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[3],
                cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[4], cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[5] }, new byte[] { 1, 1, 1, 1, 1, 1, 1 })).Row);
            for (byte i = 0; i < 6; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(6 + i / 2), new Control[] { tbEditTone_PCMDrumKit_Equalizer_EQLowGain[i], slEditTone_PCMDrumKit_Equalizer_EQLowGain[i++],
                    tbEditTone_PCMDrumKit_Equalizer_EQLowGain[i], slEditTone_PCMDrumKit_Equalizer_EQLowGain[i] }, new byte[] { 1, 2, 1, 2 })).Row);
            }
            for (byte i = 0; i < 6; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(9 + i / 2), new Control[] { tbEditTone_PCMDrumKit_Equalizer_EQMidGain[i], slEditTone_PCMDrumKit_Equalizer_EQMidGain[i++],
                    tbEditTone_PCMDrumKit_Equalizer_EQMidGain[i], slEditTone_PCMDrumKit_Equalizer_EQMidGain[i]}, new byte[] { 1, 2, 1, 2 })).Row);
            }
            for (byte i = 0; i < 6; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(12 + i / 2), new Control[] { tbEditTone_PCMDrumKit_Equalizer_EQHighGain[i], slEditTone_PCMDrumKit_Equalizer_EQHighGain[i++],
                    tbEditTone_PCMDrumKit_Equalizer_EQHighGain[i], slEditTone_PCMDrumKit_Equalizer_EQHighGain[i] }, new byte[] { 1, 2, 1, 2 })).Row);
            }

            // Set control values
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Equalizer_EqSwitch[i].IsChecked = pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].EQSwitch;
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Equalizer_EQLowFreq[i].SelectedIndex = pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].EQLowFreq;
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Equalizer_EQMidFreq[i].SelectedIndex = pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].EQMidFreq;
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Equalizer_EQHighFreq[i].SelectedIndex = pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].EQHighFreq;
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_PCMDrumKit_Equalizer_EQMidQ[i].SelectedIndex = pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].EQMidQ;
            }
            for (byte i = 0; i < 6; i++)
            {
                slEditTone_PCMDrumKit_Equalizer_EQLowGain[i].Value = (pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].EQLowGain - 15);
                tbEditTone_PCMDrumKit_Equalizer_EQLowGain[i].Text = "Low " + (i + 1).ToString() + ": " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].EQLowGain - 15)).ToString() + " dB";
            }
            for (byte i = 0; i < 6; i++)
            {
                slEditTone_PCMDrumKit_Equalizer_EQMidGain[i].Value = (pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].EQMidGain - 15);
                tbEditTone_PCMDrumKit_Equalizer_EQMidGain[i].Text = "Mid " + (i + 1).ToString() + ": " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].EQMidGain - 15)).ToString() + " dB";
            }
            for (byte i = 0; i < 6; i++)
            {
                slEditTone_PCMDrumKit_Equalizer_EQHighGain[i].Value = (pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].EQHighGain - 15);
                tbEditTone_PCMDrumKit_Equalizer_EQHighGain[i].Text = "High " + (i + 1).ToString() + ": " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[i].EQHighGain - 15)).ToString() + " dB";
            }
        }

        private void AddPCMDrumKitMFXControlControls()
        {
            t.Trace("private void AddPCMDrumKitMFXControlControls()");
            controlsIndex = 0;

            // ComboBox for MFX Control Source
            ComboBox[] cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource = new ComboBox[4];
            // ComboBox for MFX Control Assign
            ComboBox[] cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign = new ComboBox[4];
            // Slider for MFX Control Sens:
            Slider[] slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens = new Slider[4];
            for (byte i = 0; i < 4; i++)
            {
                // ComboBox for MFX Control Source
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i] = new ComboBox();
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Name = "cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource" + i.ToString() + "";
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": Off");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC01");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC02");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC03");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC04");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC05");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC06");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC07");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC08");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC09");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC10");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC11");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC12");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC13");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC14");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC15");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC16");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC17");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC18");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC19");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC20");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC21");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC22");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC23");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC24");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC25");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC26");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC27");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC28");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC29");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC30");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC31");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC32");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC33");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC34");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC35");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC36");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC37");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC38");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC39");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC40");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC41");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC42");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC43");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC44");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC45");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC46");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC47");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC48");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC49");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC50");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC51");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC52");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC53");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC54");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC55");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC56");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC57");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC58");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC59");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC60");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC61");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC62");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC63");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC64");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC65");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC66");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC67");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC68");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC69");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC70");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC71");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC72");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC73");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC74");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC75");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC76");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC77");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC78");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC79");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC80");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC81");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC82");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC83");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC84");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC85");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC86");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC87");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC88");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC89");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC90");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC91");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC92");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC93");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC94");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": CC95");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": Pitch bend");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": After touch");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": Control 1");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": Control 2");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": Control 3");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": Control 4");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": Velocity");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": Key follow");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": Tempo");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": LF01");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": LF02");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": Pitch env");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": TVF env");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].Items.Add("Source " + (i + 1).ToString() + ": TVA env");

                // ComboBox for MFX Control Assign
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i] = new ComboBox();
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].GotFocus += Generic_GotFocus;
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Name = "cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign" + i.ToString() + "";
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: Off");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 1");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 2");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 3 ");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 4 ");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 5");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 6");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 7");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 8");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 9");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 10");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 11");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 12");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 13");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 14");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 15");
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].Items.Add("Assign: 16");

                // Slider for MFX Control Sens:
                tbEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i]);
                slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i] = new Slider();
                slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i].GotFocus += Generic_GotFocus;
                slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i].Name = "slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens" + i.ToString();
                slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i].Minimum = -63;
                slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i].Maximum = 63;
            }

            // Put in rows
            for (byte i = 0; i < 4; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(0 + 3 * i), new Control[] { cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i] })).Row);
                ControlsGrid.Children.Add((new GridRow((byte)(1 + 3 * i), new Control[] { cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i] })).Row);
                ControlsGrid.Children.Add((new GridRow((byte)(2 + 3 * i), new Control[] { tbEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i],
                    slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i] }, new byte[] { 1, 2 })).Row);
            }

            // Set control values
            for (byte i = 0; i < 4; i++)
            {
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource[i].SelectedIndex = commonMFX.MFXControlSource[i];
            }
            for (byte i = 0; i < 4; i++)
            {
                cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign[i].SelectedIndex = commonMFX.MFXControlAssign[i];
            }
            for (byte i = 0; i < 4; i++)
            {
                slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i].Value = (commonMFX.MFXControlSens[i] - 64);
                tbEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[i].Text = "MFX Control Sens: " + ((commonMFX.MFXControlSens[i] - 64)).ToString();
            }
        }

        // PCM Drum Kit Save controls
        private void AddPCMDrumKitSaveControls()
        {
            t.Trace("private void AddPCMDrumKitSaveControls()");
            controlsIndex = 0;

            // Create controls
            SetLabelProperties(ref tbEditTone_SaveTone_Title);
            Button btnEditTone_PCMDrumKit_SaveTitle = new Button();
            btnEditTone_PCMDrumKit_SaveTitle.Content = "Save";
            btnEditTone_PCMDrumKit_SaveTitle.Click += btnEditTone_SaveTone_Click;
            Button btnEditTone_PCMSynthTone_DeleteTone = new Button();
            btnEditTone_PCMSynthTone_DeleteTone.Content = "Delete";
            btnEditTone_PCMSynthTone_DeleteTone.Click += btnEditTone_DeleteTone_Click;

            // Hook to help:
            tbEditTone_SaveTone_TitleText.GotFocus += Generic_GotFocus;
            tbEditTone_SaveTone_TitleText.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SaveTone_SlotNumber.GotFocus += Generic_GotFocus;
            cbEditTone_SaveTone_SlotNumber.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SaveTone_SlotNumber.SelectionChanged += CbEditTone_Save_SlotNumber_SelectionChanged;
            btnEditTone_PCMDrumKit_SaveTitle.GotFocus += Generic_GotFocus;
            btnEditTone_PCMDrumKit_SaveTitle.Tag = new HelpTag(controlsIndex++, 0);

            String numString;
            cbEditTone_SaveTone_SlotNumber.Items.Clear();
            if (commonState.toneNames[1] != null && commonState.toneNames[1].Count() == 32)
            {
                for (UInt16 i = 0; i < 32; i++)
                {
                    numString = (i + 1).ToString();
                    while (numString.Length < 3) numString = "0" + numString;
                    cbEditTone_SaveTone_SlotNumber.Items.Add(numString + ": " + commonState.toneNames[1][i]);
                }
            }
            else
            {
                for (UInt16 i = 0; i < 32; i++)
                {
                    numString = (i + 1).ToString();
                    while (numString.Length < 3) numString = "0" + numString;
                    cbEditTone_SaveTone_SlotNumber.Items.Add(numString + ": INIT KIT");
                }
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow((byte)(0), new Control[] { tbEditTone_SaveTone_Title,
                tbEditTone_SaveTone_TitleText, cbEditTone_SaveTone_SlotNumber, btnEditTone_PCMDrumKit_SaveTitle,
                btnEditTone_PCMSynthTone_DeleteTone}, new byte[] { 4, 3, 3, 2, 2 })).Row);

            // Set values
            tbEditTone_SaveTone_Title.Text = "Name (max 12 chars):";
            tbEditTone_SaveTone_TitleText.Text = pCMDrumKit.pCMDrumKitCommon.Name;
            SetSaveSlotToFirstFreeOrSameName();
        }

        #endregion

        #region SuperNATURAL Acoustic Tone

        // SuperNATURAL Acoustic Tone controls
        private void AddSupernaturalAcousticToneCommonControls(byte SelectedIndex)
        {
            controlsIndex = 0;

            // ComboBox for Phrase Number
            ComboBox cbEditTone_SuperNATURALAcousticTone_Common_PhraseNumber = new ComboBox();
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseNumber.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseNumber.GotFocus += Generic_GotFocus;
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseNumber.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseNumber.Name = "cbEditTone_SuperNATURALAcousticTone_Common_PhraseNumber";
            UInt16 i = 0;
            foreach (String item in phrases.Names)
            {
                cbEditTone_SuperNATURALAcousticTone_Common_PhraseNumber.Items.Add("Phrase" + i.ToString() + ":" + item);
                i++;
            }

            // ComboBox for Phrase Octave Shift
            ComboBox cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift = new ComboBox();
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift.GotFocus += Generic_GotFocus;
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift.Name = "cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift";
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift.Items.Add("Phrase octave shift -3");
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift.Items.Add("Phrase octave shift -2");
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift.Items.Add("Phrase octave shift -1");
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift.Items.Add("Phrase octave shift 0");
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift.Items.Add("Phrase octave shift 1");
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift.Items.Add("Phrase octave shift 2");
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift.Items.Add("Phrase octave shift 3");

            // Slider for Tone Level:
            SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Common_ToneLevel);
            Slider slEditTone_SuperNATURALAcousticTone_Common_ToneLevel = new Slider();
            slEditTone_SuperNATURALAcousticTone_Common_ToneLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_SuperNATURALAcousticTone_Common_ToneLevel.GotFocus += Generic_GotFocus;
            slEditTone_SuperNATURALAcousticTone_Common_ToneLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_SuperNATURALAcousticTone_Common_ToneLevel.Name = "slEditTone_SuperNATURALAcousticTone_Common_ToneLevel";
            slEditTone_SuperNATURALAcousticTone_Common_ToneLevel.Minimum = 0;
            slEditTone_SuperNATURALAcousticTone_Common_ToneLevel.Maximum = 127;

            // ComboBox for Mono Poly
            ComboBox cbEditTone_SuperNATURALAcousticTone_Common_MonoPoly = new ComboBox();
            cbEditTone_SuperNATURALAcousticTone_Common_MonoPoly.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_SuperNATURALAcousticTone_Common_MonoPoly.GotFocus += Generic_GotFocus;
            cbEditTone_SuperNATURALAcousticTone_Common_MonoPoly.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SuperNATURALAcousticTone_Common_MonoPoly.Name = "cbEditTone_SuperNATURALAcousticTone_Common_MonoPoly";
            cbEditTone_SuperNATURALAcousticTone_Common_MonoPoly.Items.Add("Mono");
            cbEditTone_SuperNATURALAcousticTone_Common_MonoPoly.Items.Add("Poly");

            // ComboBox for Octave Shift
            ComboBox cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift = new ComboBox();
            cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift.GotFocus += Generic_GotFocus;
            cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift.Name = "cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift";
            cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift.Items.Add("Octave shift -3");
            cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift.Items.Add("Octave shift -2");
            cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift.Items.Add("Octave shift -1");
            cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift.Items.Add("Octave shift 0");
            cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift.Items.Add("Octave shift 2");
            cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift.Items.Add("Octave shift 3");

            // Slider for Cutoff Offset:
            SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Common_CutoffOffset);
            Slider slEditTone_SuperNATURALAcousticTone_Common_CutoffOffset = new Slider();
            slEditTone_SuperNATURALAcousticTone_Common_CutoffOffset.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_SuperNATURALAcousticTone_Common_CutoffOffset.GotFocus += Generic_GotFocus;
            slEditTone_SuperNATURALAcousticTone_Common_CutoffOffset.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_SuperNATURALAcousticTone_Common_CutoffOffset.Name = "slEditTone_SuperNATURALAcousticTone_Common_CutoffOffset";
            slEditTone_SuperNATURALAcousticTone_Common_CutoffOffset.Minimum = -64;
            slEditTone_SuperNATURALAcousticTone_Common_CutoffOffset.Maximum = 63;

            // Slider for Resonance Offset:
            SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset);
            Slider slEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset = new Slider();
            slEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset.GotFocus += Generic_GotFocus;
            slEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset.Name = "slEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset";
            slEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset.Minimum = -64;
            slEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset.Maximum = 63;

            // Slider for Attack Time Offset:
            SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset);
            Slider slEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset = new Slider();
            slEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset.GotFocus += Generic_GotFocus;
            slEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset.Name = "slEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset";
            slEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset.Minimum = -64;
            slEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset.Maximum = 63;

            // Slider for Release Time Offset:
            SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset);
            Slider slEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset = new Slider();
            slEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset.GotFocus += Generic_GotFocus;
            slEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset.Name = "slEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset";
            slEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset.Minimum = -64;
            slEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset.Maximum = 63;

            // Slider for Portamento Time Offset:
            SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset);
            Slider slEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset = new Slider();
            slEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset.GotFocus += Generic_GotFocus;
            slEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset.Name = "slEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset";
            slEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset.Minimum = -64;
            slEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset.Maximum = 63;

            // Slider for Vibrato Rate:
            SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Common_VibratoRate);
            Slider slEditTone_SuperNATURALAcousticTone_Common_VibratoRate = new Slider();
            slEditTone_SuperNATURALAcousticTone_Common_VibratoRate.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_SuperNATURALAcousticTone_Common_VibratoRate.GotFocus += Generic_GotFocus;
            slEditTone_SuperNATURALAcousticTone_Common_VibratoRate.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_SuperNATURALAcousticTone_Common_VibratoRate.Name = "slEditTone_SuperNATURALAcousticTone_Common_VibratoRate";
            slEditTone_SuperNATURALAcousticTone_Common_VibratoRate.Minimum = -64;
            slEditTone_SuperNATURALAcousticTone_Common_VibratoRate.Maximum = 63;

            // Slider for Vibrato Depth:
            SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Common_VibratoDepth);
            Slider slEditTone_SuperNATURALAcousticTone_Common_VibratoDepth = new Slider();
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDepth.GotFocus += Generic_GotFocus;
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDepth.Name = "slEditTone_SuperNATURALAcousticTone_Common_VibratoDepth";
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDepth.Minimum = -64;
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDepth.Maximum = 63;

            // Slider for Vibrato Delay:
            SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Common_VibratoDelay);
            Slider slEditTone_SuperNATURALAcousticTone_Common_VibratoDelay = new Slider();
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDelay.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDelay.GotFocus += Generic_GotFocus;
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDelay.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDelay.Name = "slEditTone_SuperNATURALAcousticTone_Common_VibratoDelay";
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDelay.Minimum = -64;
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDelay.Maximum = 63;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_SuperNATURALAcousticTone_Common_PhraseNumber,
                cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_SuperNATURALAcousticTone_Common_ToneLevel,
                slEditTone_SuperNATURALAcousticTone_Common_ToneLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { cbEditTone_SuperNATURALAcousticTone_Common_MonoPoly,
                cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift})).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_SuperNATURALAcousticTone_Common_CutoffOffset,
                slEditTone_SuperNATURALAcousticTone_Common_CutoffOffset }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset,
                slEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset,
                slEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset,
                slEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset,
                slEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_SuperNATURALAcousticTone_Common_VibratoRate,
                slEditTone_SuperNATURALAcousticTone_Common_VibratoRate }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { tbEditTone_SuperNATURALAcousticTone_Common_VibratoDepth,
                slEditTone_SuperNATURALAcousticTone_Common_VibratoDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { tbEditTone_SuperNATURALAcousticTone_Common_VibratoDelay,
                slEditTone_SuperNATURALAcousticTone_Common_VibratoDelay }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseNumber.SelectedIndex = superNATURALAcousticTone.superNATURALAcousticToneCommon.PhraseNumber;
            cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift.SelectedIndex = superNATURALAcousticTone.superNATURALAcousticToneCommon.PhraseOctaveShift - 61;
            slEditTone_SuperNATURALAcousticTone_Common_ToneLevel.Value = (superNATURALAcousticTone.superNATURALAcousticToneCommon.ToneLevel);
            tbEditTone_SuperNATURALAcousticTone_Common_ToneLevel.Text = "Tone Level: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ToneLevel)).ToString();
            cbEditTone_SuperNATURALAcousticTone_Common_MonoPoly.SelectedIndex = superNATURALAcousticTone.superNATURALAcousticToneCommon.MonoPoly;
            cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift.SelectedIndex = superNATURALAcousticTone.superNATURALAcousticToneCommon.OctaveShift - 61;
            slEditTone_SuperNATURALAcousticTone_Common_CutoffOffset.Value = (superNATURALAcousticTone.superNATURALAcousticToneCommon.CutoffOffset - 64);
            tbEditTone_SuperNATURALAcousticTone_Common_CutoffOffset.Text = "Cutoff Offset: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.CutoffOffset - 64)).ToString();
            slEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset.Value = (superNATURALAcousticTone.superNATURALAcousticToneCommon.ResonanceOffset - 64);
            tbEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset.Text = "Resonance Offset: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ResonanceOffset - 64)).ToString();
            slEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset.Value = (superNATURALAcousticTone.superNATURALAcousticToneCommon.AttackTimeOffset - 64);
            tbEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset.Text = "Attack Time Offset: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.AttackTimeOffset - 64)).ToString();
            slEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset.Value = (superNATURALAcousticTone.superNATURALAcousticToneCommon.ReleaseTimeOffset - 64);
            tbEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset.Text = "Release Time Offset: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ReleaseTimeOffset - 64)).ToString();
            slEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset.Value = (superNATURALAcousticTone.superNATURALAcousticToneCommon.PortamentoTimeOffset - 64);
            tbEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset.Text = "Portamento Time Offset: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.PortamentoTimeOffset - 64)).ToString();
            slEditTone_SuperNATURALAcousticTone_Common_VibratoRate.Value = (superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoRate - 64);
            tbEditTone_SuperNATURALAcousticTone_Common_VibratoRate.Text = "Vibrato Rate: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoRate - 64)).ToString();
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDepth.Value = (superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoDepth - 64);
            tbEditTone_SuperNATURALAcousticTone_Common_VibratoDepth.Text = "Vibrato Depth: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoDepth - 64)).ToString();
            slEditTone_SuperNATURALAcousticTone_Common_VibratoDelay.Value = (superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoDelay - 64);
            tbEditTone_SuperNATURALAcousticTone_Common_VibratoDelay.Text = "Vibrato Delay: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoDelay - 64)).ToString();
        }

        private void AddSupernaturalAcousticToneInstrumentControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalAcousticToneInstrumentControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // Get the string identifying bank:
            String[] parts = selectedSound.ProgramBank.Split(' ');
            String bankName = parts[0].Replace("Preset", "INT");

            // ComboBox for Bank (not in I-7 memory!)
            cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.Items.Clear();
            cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.GotFocus += Generic_GotFocus;
            cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.Name = "cbEditTone_SuperNATURALAcousticTone_Instrument_Bank";
            foreach (String bank in superNaturalAcousticInstrumentList.Banks)
            {
                cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.Items.Add(bank);
            }
            try
            {
                if (cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.Items.Count() > 5)
                {
                    currentHandleControlEvents = handleControlEvents;
                    handleControlEvents = false;
                    cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.SelectedItem = bankName;
                    handleControlEvents = currentHandleControlEvents;
                }
            }
            catch { }

            // ComboBox for Inst Number
            cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.Items.Clear();
            cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.GotFocus += Generic_GotFocus;
            cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.Name = 
                "cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber";
            Int32 index = 0;
            foreach (Instrument instrument in superNaturalAcousticInstrumentList.Tones)
            {
                if (instrument.InstrumentBank.Replace("Preset", "INT") == bankName)
                {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.Items
                        .Add(instrument.InstrumentNumber.ToString() + ": " + instrument.InstrumentName);
                    if (instrument.InstrumentName == superNATURALAcousticTone.superNATURALAcousticToneCommon.Name.Trim())
                    {
                        index = cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.Items.Count() - 1;
                        currentInstrument = instrument;
                    }
                }
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_SuperNATURALAcousticTone_Instrument_Bank,
                cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber })).Row);
            currentHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.SelectedIndex = index;
            handleControlEvents = currentHandleControlEvents;

            // Add instrument parameters:
            AddSupernaturalAcousticToneInstrumentParametersControls();
        }


        private void AddSupernaturalAcousticToneInstrumentParametersControls()
        {
            t.Trace("private void AddSupernaturalAcousticToneInstrumentParametersControls()");
            controlsIndex = 2;

            // Remove any previous controls before adding new ones, but leave the first row:
            //for (byte i = 1; i < ControlsGrid.Children.Count(); i++)
            //{
            //    RemoveControls((Grid)ControlsGrid.Children[i]);
            //    ((Grid)ControlsGrid).Children.RemoveAt(0);
            //}
            while (ControlsGrid.Children.Count() > 1)
            {
                RemoveControls((Grid)ControlsGrid.Children[1]);
                ((Grid)ControlsGrid).Children.RemoveAt(1);
            }

            // Set of controls depend on ParameterMask for the instrument. Mask to select is indicated in instrument MaskIndex.
            // Controls are created here according to the selected ParameterMask.
            // Parametermasks are indicated by tone type (Ac.Piano etc.) but there are exceptions.
            // Start with tone type and then check the exceptions and if it is not an exception, tone type will be used:
            //byte bankMsb = 89;
            //byte bankLsb = 64;
            //if (superNaturalAcousticInstrumentList.Instruments[cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.SelectedIndex].InstrumentGroup != "USER") // Not yet implemented!
            //{
            //    bankLsb = 0;
            //}
            byte index = 255;
            //currentInstrument = 
            //    superNaturalAcousticInstrumentList.GetTone(cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.SelectedItem.ToString(),
            //    cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.SelectedItem.ToString());

            byte[] mask = new byte[0];
            switch (cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.SelectedItem.ToString())
            {
                case "INT":
                    mask = new byte[] {
                        1, 1, 1, 1, 1, 1, 1, 1, 1, // Ac.Piano
                        2, 2, 2, 2, 2, 2,          // E.Pinao
                        4, 4, 4, 4, 4, 4, 4, 4,    // Other keyboards
                        6, 6, 6, 6, 6,             // Bell/Mallet
                        3,                         // Organ
                        5, 5,                      // Accordeon
                        48,                        // Harmonica
                        5,                         // Accordeon
                        7, 7, 7,                   // Ac.Guitar
                        8, 8, 8, 8,                // E.Guitar
                        10,                        // Ac.Bass
                        11, 11, 11,                // E.Bass
                        37,                        // Sitar
                        14, 14, 14, 14, 14, 14,    // Strings
                        13,                        // Plucked/Stroke
                        34,                        // Percussion
                        43, 43,                    // Strings/Marcato Strings
                        20, 20,                    // Vox/Choir
                        15, 15, 15, 15, 15,        // Brass
                        18, 18, 18, 18,            // Sax
                        16, 16, 16,                // Wind
                        17, 17, 17,                // Flute
                        46,                        // Shakuhachi + Recorder
                        45, 45,                    // Pipes
                        42,                        // Erhu
                        47                         // Steel Drums
                    };
                    break;
                case "ExSN1":
                    mask = new byte[] {
                        6, 6,                      // Bell/Mallet
                        19,19,                     // Tin Whistle/Ryuteki
                        38, 38,                    // Plucked/Stroke
                        39,                        // Koto 
                        40,                        // Taishou Koto
                        41,                        // Kalimba
                        44,                        // Sarangi
                    };
                    break;
                case "ExSN2":
                    mask = new byte[] {
                        18, 18, 18, 18,            // Sax
                        16, 16,                    // Wind
                        17,                        // Flute
                        19, 19, 19, 19,
                        19, 19, 19, 19,            // Recorder/Occarina
                    };
                    break;
                case "ExSN3":
                    mask = new byte[] {
                        7, 7,                      // Ac.Guitar
                        8, 8, 8,                   // E.Guitar
                        10,                        // Ac.Bass
                        11, 11,                    // E.Bass
                    };
                    break;
                case "ExSN4":
                    mask = new byte[] {
                        7, 7, 7,                   // Ac.Guitar
                        36,                        // Mandolin
                        7, 7,                      // Ac.Guitar
                    };
                    break;
                case "ExSN5":
                    mask = new byte[] {
                        15, 15, 15, 15, 15,        // Brass
                        15, 15, 15, 15, 15, 15,    // Brass
                    };
                    break;
                case "ExSN6":
                    mask = new byte[] {
                    };
                    break;
            }
            index = mask[cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.SelectedIndex];

            //switch (currentInstrument.InstrumentGroup)
            //{
            //    case "Ac.Piano": index = 1; break;
            //    case "E.Piano": index = 2; break;
            //    case "Organ": index = 3; break;
            //    case "Other keyboards + Accordion": index = 4; break;
            //    case "Accordion": index = 5; break;
            //    case "Bell/Mallet": index = 6; break;
            //    case "Ac.Guitar": index = 7; break;
            //    case "E.Guitar": index = 8; break;
            //    case "Dist.Guitar": index = 9; break;
            //    case "Ac.Bass": index = 10; break;
            //    case "E.Bass": index = 11; break;
            //    case "Synth Bass": index = 12; break;
            //    case "Plucked/Stroke": index = 13; break;
            //    case "Strings": index = 14; break;
            //    case "Brass": index = 15; break;
            //    case "Wind": index = 16; break;
            //    case "Flute": index = 17; break;
            //    case "Sax": index = 18; break;
            //    case "Recorder": index = 19; break;
            //    case "Vox/Choir": index = 20; break;
            //    case "Synth Lead": index = 21; break;
            //    case "Synth Brass": index = 22; break;
            //    case "Synth Pad/Strings": index = 23; break;
            //    case "Synth Bellpad": index = 24; break;
            //    case "Synth PolyKey": index = 25; break;
            //    case "FX": index = 26; break;
            //    case "Synth Seq/Pop": index = 27; break;
            //    case "Phrase": index = 28; break;
            //    case "Pulsating": index = 29; break;
            //    case "Beat &Groove": index = 30; break;
            //    case "Hit": index = 31; break;
            //    case "Sound FX": index = 32; break;
            //    case "Drums": index = 33; break;
            //    case "Percussion": index = 34; break;
            //    case "Combination": index = 35; break;
            //    case "Mandolin": index = 36; break;
            //    case "Sitar": index = 37; break;
            //    case "Tsugaru/Sansin": index = 38; break;
            //    case "Tsugaru": index = 38; break;
            //    case "Sansin": index = 38; break;
            //    case "Koto": index = 39; break;
            //    case "Taishou Koto": index = 40; break;
            //    case "Kalimba": index = 41; break;
            //    case "Erhu": index = 42; break;
            //    case "Marcato Strings": index = 43; break;
            //    case "Sarangi": index = 44; break;
            //    case "Pipes": index = 45; break;
            //    case "Shakuhachi": index = 46; break;
            //    case "Steel Drums": index = 47; break;
            //    case "Harmonica": index = 48; break;
            //}

            //if (index == 255)
            //{
            //    switch (currentInstrument.InstrumentName)
            //    {
            //        case "Ac.Piano": index = 1; break;
            //        case "E.Piano": index = 2; break;
            //        case "Organ": index = 3; break;
            //        case "Other keyboards + Accordion": index = 4; break;
            //        case "Accordion": index = 5; break;
            //        case "Bell/Mallet": index = 6; break;
            //        case "Ac.Guitar": index = 7; break;
            //        case "E.Guitar": index = 8; break;
            //        case "Dist.Guitar": index = 9; break;
            //        case "Ac.Bass": index = 10; break;
            //        case "E.Bass": index = 11; break;
            //        case "Synth Bass": index = 12; break;
            //        case "Plucked/Stroke": index = 13; break;
            //        case "Strings": index = 14; break;
            //        case "Brass": index = 15; break;
            //        case "Wind": index = 16; break;
            //        case "Flute": index = 17; break;
            //        case "Sax": index = 18; break;
            //        case "Recorder": index = 19; break;
            //        case "Vox/Choir": index = 20; break;
            //        case "Synth Lead": index = 21; break;
            //        case "Synth Brass": index = 22; break;
            //        case "Synth Pad/Strings": index = 23; break;
            //        case "Synth Bellpad": index = 24; break;
            //        case "Synth PolyKey": index = 25; break;
            //        case "FX": index = 26; break;
            //        case "Synth Seq/Pop": index = 27; break;
            //        case "Phrase": index = 28; break;
            //        case "Pulsating": index = 29; break;
            //        case "Beat &Groove": index = 30; break;
            //        case "Hit": index = 31; break;
            //        case "Sound FX": index = 32; break;
            //        case "Drums": index = 33; break;
            //        case "Percussion": index = 34; break;
            //        case "Combination": index = 35; break;
            //        case "Mandolin": index = 36; break;
            //        case "Sitar": index = 37; break;
            //        case "Tsugaru/Sansin": index = 38; break;
            //        case "Tsugaru": index = 38; break;
            //        case "Sansin": index = 38; break;
            //        case "Koto": index = 39; break;
            //        case "Taishou Koto": index = 40; break;
            //        case "Kalimba": index = 41; break;
            //        case "Erhu": index = 42; break;
            //        case "Marcato Strings": index = 43; break;
            //        case "Sarangi": index = 44; break;
            //        case "Pipes": index = 45; break;
            //        case "Shakuhachi": index = 46; break;
            //        case "Steel Drums": index = 47; break;
            //        case "Harmonica": index = 48; break;
            //    }
            //}

            if (index == 255)
            {
                t.Trace("Missing instrument, group = " + currentInstrument.InstrumentGroup + " name = " + currentInstrument.InstrumentName);
                return;
            }

            // Handler did not know tone type and thus did not tell I-7 to change sound, but now we know:
            // First send basic parameters Mono/Poly - Octave Shift (because that is what the I-7 does):
            byte[] address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x11 });
            SendParameter(address, superNaturalAcousticInstrumentList.Parameterlist1
                [cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.SelectedIndex]
                [(byte)cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.SelectedIndex]);
            // Then send Inst Variation, Inst Number and all available Modify Parameters:
            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x20 });
            SendParameter(address, superNaturalAcousticInstrumentList.Parameterlist2
                [cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.SelectedIndex]
                [(byte)cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.SelectedIndex]);

            //byte index = superNATURALAcousticTone.superNATURALAcousticToneCommon.ToneCategoryNameIndex[superNATURALAcousticTone.superNATURALAcousticToneCommon.Category];
            //switch (selectedSound.Id)
            //{
            //    case 1470858:
            //    case 1470859:
            //        index = 36;
            //        break;
            //    case 1466530: // Harmonica
            //        index = 48;
            //        break;
            //}

            byte row = 1;
            // String Resonance 0–127
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for String Resonance:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_StringResonance);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_StringResonance = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_StringResonance.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_StringResonance.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_StringResonance.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_StringResonance.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_StringResonance";
                slEditTone_SuperNATURALAcousticTone_Instrument_StringResonance.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_StringResonance.Maximum = 127;

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_StringResonance,
                    slEditTone_SuperNATURALAcousticTone_Instrument_StringResonance }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_StringResonance.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1);
                tbEditTone_SuperNATURALAcousticTone_Instrument_StringResonance.Text = 
                    "String Resonance: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1)).ToString();
            }

            // Key Off Resonance 0–127
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {

                // Slider for Key Off Resonance:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance";
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance.Maximum = 127;

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance,
                    slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2);
                tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance.Text = 
                    "Key Off Resonance: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2)).ToString();
            }

            // Hammer Noise -2, -1, 0, +1, +2
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Hammer Noise
                ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise = new ComboBox();
                cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise";
                cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise.Items.Add("Hammer Noise -2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise.Items.Add("Hammer Noise -1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise.Items.Add("Hammer Noise 0");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise.Items.Add("Hammer Noise +1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise.Items.Add("Hammer Noise +2");

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise.SelectedIndex = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3 - 62;
            }

            // StereoWidth 0–63
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Stereo Width:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth";
                slEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth.Maximum = 63;

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth,
                    slEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4);
                tbEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth.Text = 
                    "Stereo Width: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4)).ToString();
            }

            // Nuance Type1, Type2, Type3
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Nuance
                ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_Nuance = new ComboBox();
                cbEditTone_SuperNATURALAcousticTone_Instrument_Nuance.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_Nuance.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_Nuance.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_Nuance.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_Nuance";
                cbEditTone_SuperNATURALAcousticTone_Instrument_Nuance.Items.Add("Nuance type 1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_Nuance.Items.Add("Nuance type 2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_Nuance.Items.Add("Nuance type 3");

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_Nuance })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_Nuance.SelectedIndex = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5;
            }

            // Tone Character -5, -4, -3, -2, -1, 0, +1, +2, +3, +4, +5
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Tone Character
                ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter = new ComboBox();
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter";
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Items.Add("Tone Character -5");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Items.Add("Tone Character -4");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Items.Add("Tone Character -3");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Items.Add("Tone Character -2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Items.Add("Tone Character -1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Items.Add("Tone Character 0");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Items.Add("Tone Character +1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Items.Add("Tone Character +2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Items.Add("Tone Character +3");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Items.Add("Tone Character +4");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.Items.Add("Tone Character +5");

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter.SelectedIndex = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6 - 59;
            }

            // Noise Level (CC16) - 64–+63
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {

                // Slider for Noise Level (CC16):
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16 = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16";
                slEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16.Minimum = -64;
                slEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16.Maximum = 63;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16,
                    slEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16 }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16.Text = 
                    "Noise Level (CC16): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 - 64)).ToString();
            }

            // Crescendo Depth(CC17) - 64–+63 (This applies only for ExSN5 004: Mariachi Tp)
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0 && 
                superNATURALAcousticTone.superNATURALAcousticToneCommon.Name == "Mariachi Tp")
            {
                // Slider for Crescendo Depth CC17:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17 = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17";
                slEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17.Minimum = -64;
                slEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17.Maximum = 63;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17, slEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17 }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17.Text = 
                    "Crescendo Depth (CC17): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64)).ToString();
            }

            // Tremolo Speed(CC17) - 64–+63
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Tremolo Speed CC17:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17 = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17";
                slEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17.Minimum = -64;
                slEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17.Maximum = 63;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17,
                    slEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17 }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17.Text = 
                    "Tremolo Speed CC17: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64)).ToString();
            }

            // Strum Speed(CC17) - 64–+63
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Strum Speed CC17:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17 = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17";
                slEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17.Minimum = -64;
                slEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17.Maximum = 63;

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17,
                    slEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17 }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17.Text = "Strum Speed CC17: " + 
                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64)).ToString();
            }

            // Strum Mode(CC19) OFF, ON
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // CheckBox for StrumModeCC19
                CheckBox cbEditTone_SuperNATURALAcousticTone_Instrument_StrumModeCC19 = new CheckBox();
                cbEditTone_SuperNATURALAcousticTone_Instrument_StrumModeCC19.Tapped += GenericCheckBox_Click;
                cbEditTone_SuperNATURALAcousticTone_Instrument_StrumModeCC19.Click += GenericCheckBox_Click;
                cbEditTone_SuperNATURALAcousticTone_Instrument_StrumModeCC19.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_StrumModeCC19.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_StrumModeCC19.Content = "StrumModeCC19";
                cbEditTone_SuperNATURALAcousticTone_Instrument_StrumModeCC19.Name =
                    "cbEditTone_SuperNATURALAcousticTone_StrumModeCC19";

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_StrumModeCC19 })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_StrumModeCC19.IsChecked =
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4 > 0;
            }

            // Picking Harmonics OFF, ON
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0) 
                // && SuperNATURALAcousticTone.SuperNATURALAcousticToneCommon.Name != "Jazz ")
            {
                // CheckBox for Picking Harmonics
                CheckBox cbEditTone_SuperNATURALAcousticTone_Instrument_PickingHarmonics = new CheckBox();
                cbEditTone_SuperNATURALAcousticTone_Instrument_PickingHarmonics.Tapped += GenericCheckBox_Click;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PickingHarmonics.Click += GenericCheckBox_Click;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PickingHarmonics.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PickingHarmonics.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_PickingHarmonics.Content = "Picking Harmonics";
                cbEditTone_SuperNATURALAcousticTone_Instrument_PickingHarmonics.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_PickingHarmonics";

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_PickingHarmonics })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_PickingHarmonics.IsChecked = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter11 > 0;
            }

            // Sub String Tune - 64–+63 (This is valid only for ExSN4 003: 12th Steel Gtr.)
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0 && 
                superNATURALAcousticTone.superNATURALAcousticToneCommon.Name == "12StringsGtr")
            {
                // Slider for Sub String Tune:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune";
                slEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune.Minimum = -64;
                slEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune.Maximum = 63;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune,
                    slEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune.Value =
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter13 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune.Text = 
                    "Sub String Tune: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter13 - 64)).ToString();
            }

            // Growl Sens(CC18) 0–127
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Growl Sens (CC18):
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18 = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18";
                slEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18.Maximum = 127;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18,
                    slEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18 }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18.Value =
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3);
                tbEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18.Text = 
                    "Growl Sens (CC18): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3)).ToString();
            }

            // Harmonic Bar 16' 0–8
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Harmonic Bar 16
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16";
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.Items.Add("Harmonic Bar 16' 0");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.Items.Add("Harmonic Bar 16' 1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.Items.Add("Harmonic Bar 16' 2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.Items.Add("Harmonic Bar 16' 3");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.Items.Add("Harmonic Bar 16' 4");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.Items.Add("Harmonic Bar 16' 5");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.Items.Add("Harmonic Bar 16' 6");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.Items.Add("Harmonic Bar 16' 7");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.Items.Add("Harmonic Bar 16' 8");
            }

            // Harmonic Bar 5 - 1 / 3' 0–8
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Harmonic Bar 513
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.Name =
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513";
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.Items.Add("Harmonic Bar 5-1/3' 0");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.Items.Add("Harmonic Bar 5-1/3' 1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.Items.Add("Harmonic Bar 5-1/3' 2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.Items.Add("Harmonic Bar 5-1/3' 3");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.Items.Add("Harmonic Bar 5-1/3' 4");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.Items.Add("Harmonic Bar 5-1/3' 5");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.Items.Add("Harmonic Bar 5-1/3' 6");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.Items.Add("Harmonic Bar 5-1/3' 7");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.Items.Add("Harmonic Bar 5-1/3' 8");
            }

            // Harmonic Bar 8' 0–8
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Harmonic Bar 8
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8";
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.Items.Add("Harmonic Bar 8' 0");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.Items.Add("Harmonic Bar 8' 1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.Items.Add("Harmonic Bar 8' 2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.Items.Add("Harmonic Bar 8' 3");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.Items.Add("Harmonic Bar 8' 4");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.Items.Add("Harmonic Bar 8' 5");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.Items.Add("Harmonic Bar 8' 6");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.Items.Add("Harmonic Bar 8' 7");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.Items.Add("Harmonic Bar 8' 8");

                // Put in rows
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16,
                    cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8,
                    cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513 })).Row);

                // Set control values
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16.SelectedIndex =
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513.SelectedIndex =
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8.SelectedIndex =
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3;
            }

            // Harmonic Bar 4' 0–8
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Harmonic Bar 4
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.Name =
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4";
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.Items.Add("Harmonic Bar 4' 0");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.Items.Add("Harmonic Bar 4' 1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.Items.Add("Harmonic Bar 4' 2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.Items.Add("Harmonic Bar 4' 3");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.Items.Add("Harmonic Bar 4' 4");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.Items.Add("Harmonic Bar 4' 5");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.Items.Add("Harmonic Bar 4' 6");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.Items.Add("Harmonic Bar 4' 7");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.Items.Add("Harmonic Bar 4' 8");
            }

            // Harmonic Bar 2 - 2 / 3' 0–8
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Harmonic Bar 223
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.Name =
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223";
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.Items.Add("Harmonic Bar 2-2/3' 0");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.Items.Add("Harmonic Bar 2-2/3' 1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.Items.Add("Harmonic Bar 2-2/3' 2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.Items.Add("Harmonic Bar 2-2/3' 3");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.Items.Add("Harmonic Bar 2-2/3' 4");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.Items.Add("Harmonic Bar 2-2/3' 5");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.Items.Add("Harmonic Bar 2-2/3' 6");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.Items.Add("Harmonic Bar 2-2/3' 7");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.Items.Add("Harmonic Bar 2-2/3' 8");
            }

            // Harmonic Bar 2' 0–8
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Harmonic Bar 2
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.Name =
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2";
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.Items.Add("Harmonic Bar 2' 0");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.Items.Add("Harmonic Bar 2' 1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.Items.Add("Harmonic Bar 2' 2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.Items.Add("Harmonic Bar 2' 3");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.Items.Add("Harmonic Bar 2' 4");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.Items.Add("Harmonic Bar 2' 5");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.Items.Add("Harmonic Bar 2' 6");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.Items.Add("Harmonic Bar 2' 7");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.Items.Add("Harmonic Bar 2' 8");

                // Put in rows
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4,
                    cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223,
                    cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2 })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4.SelectedIndex = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223.SelectedIndex = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2.SelectedIndex = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6;
            }

            // Harmonic Bar 1 - 3 / 5' 0–8
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Harmonic Bar 135
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135";
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.Items.Add("Harmonic Bar 1-3/5' 0");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.Items.Add("Harmonic Bar 1-3/5' 1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.Items.Add("Harmonic Bar 1-3/5' 2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.Items.Add("Harmonic Bar 1-3/5' 3");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.Items.Add("Harmonic Bar 1-3/5' 4");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.Items.Add("Harmonic Bar 1-3/5' 5");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.Items.Add("Harmonic Bar 1-3/5' 6");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.Items.Add("Harmonic Bar 1-3/5' 7");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.Items.Add("Harmonic Bar 1-3/5' 8");
            }

            // Harmonic Bar 1 - 1 / 3' 0–8
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Harmonic Bar 113
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113";
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.Items.Add("Harmonic Bar 1-1/3' 0");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.Items.Add("Harmonic Bar 1-1/3' 1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.Items.Add("Harmonic Bar 1-1/3' 2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.Items.Add("Harmonic Bar 1-1/3' 3");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.Items.Add("Harmonic Bar 1-1/3' 4");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.Items.Add("Harmonic Bar 1-1/3' 5");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.Items.Add("Harmonic Bar 1-1/3' 6");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.Items.Add("Harmonic Bar 1-1/3' 7");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.Items.Add("Harmonic Bar 1-1/3' 8");
            }

            // Harmonic Bar 1' 0–8
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Harmonic Bar 1
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1";
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.Items.Add("Harmonic Bar 1' 0");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.Items.Add("Harmonic Bar 1' 1");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.Items.Add("Harmonic Bar 1' 2");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.Items.Add("Harmonic Bar 1' 3");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.Items.Add("Harmonic Bar 1' 4");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.Items.Add("Harmonic Bar 1' 5");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.Items.Add("Harmonic Bar 1' 6");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.Items.Add("Harmonic Bar 1' 7");
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.Items.Add("Harmonic Bar 1' 8");

                // Put in rows
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135,
                    cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113,
                    cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1 })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1.SelectedIndex = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter9;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135.SelectedIndex = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter7;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113.SelectedIndex = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter8;
            }

            // Leakage Level 0–127
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {

                // Slider for Leakage Level:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel";
                slEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel.Maximum = 127;

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] { tbEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel,
                    slEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter22);
                tbEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel.Text = "Leakage Level: " + 
                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter22)).ToString();
            }

            // Percussion Switch OFF, ON
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // CheckBox for Percussion Switch
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSwitch.Tapped += GenericCheckBox_Click;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSwitch.Click += GenericCheckBox_Click;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSwitch.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSwitch.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSwitch.Content = "Percussion Switch";
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSwitch.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_PercussionSwitch";

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSwitch }, new byte[] { 1, 2 })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSwitch.IsChecked = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter10 > 0);
            }

            // Percussion Soft NORM, SOFT
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Percussion Soft
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoft.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoft.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoft.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoft.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoft";
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoft.Items.Add("Percussion Normal");
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoft.Items.Add("Percussion Soft");
            }

            // Percussion Soft Level 0–15
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Percussion Soft Level:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel";
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel.Maximum = 15;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] { tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel,
                    slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter15);
                tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel.Text = "Percussion Soft Level: " + 
                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter15)).ToString();
            }

            // Percussion Normal Level 0–15
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Percussion Normal Level:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel";
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel.Maximum = 15;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel,
                    slEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter16);
                tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel.Text = "Percussion Normal Level: " + 
                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter16)).ToString();
            }

            // Percussion Slow FAST, SLOW
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Percussion Slow
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlow.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlow.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlow.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlow.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlow";
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlow.Items.Add("Percussion Fast");
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlow.Items.Add("Percussion Slow");
            }

            // Percussion Slow Time 0–127
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Percussion Slow Time:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime.Name =
                    "slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime";
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime.Maximum = 127;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime,
                    slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter17);
                tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime.Text = "Percussion Slow Time: " + 
                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter17)).ToString();
            }

            // Percussion Fast Time 0–127
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Percussion Fast Time:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime);
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime.Name =
                    "slEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime";
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime.Maximum = 127;

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime,
                    slEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter18);
                tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime.Text = "Percussion Fast Time: " + 
                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter18)).ToString();
            }

            // Percussion Harmonic 2ND, 3RD
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Percussion Harmonic
                ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonic = new ComboBox();
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonic.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonic.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonic.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonic.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonic";
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonic.Items.Add("Percussion Harmonic 2nd");
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonic.Items.Add("Percussion Harmonic 3rd");

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlow,
                    cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoft,
                    cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonic })).Row);

                // Set control values
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoft.SelectedIndex = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter21;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlow.SelectedIndex = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter12;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonic.SelectedIndex = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter11;
            }

            // Percussion Recharge Time 0–15
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Percussion Recharge Time:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime";
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime.Maximum = 15;

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime,
                    slEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter19);
                tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime.Text = "Percussion Recharge Time: " + 
                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter19)).ToString();
            }

            // Percussion Harmonic Bar Level 0–127
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Percussion Harmonic Bar Level:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel";
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel.Maximum = 127;

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] { tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel,
                    slEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter20);
                tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel.Text = "Percussion Harmonic Bar Level: " + 
                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter20)).ToString();
            }

            // Key On Click Level 0–31
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Key On Click Level:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel";
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel.Maximum = 31;

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] { tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel,
                    slEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter13);
                tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel.Text = "Key On Click Level: " + 
                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter13)).ToString();
            }

            // Key Off Click Level 0–31
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Key Off Click Level:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel";
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel.Minimum = 0;
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel.Maximum = 31;

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] { tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel,
                    slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter14);
                tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel.Text = "Key Off Click Level: " + 
                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter14)).ToString();
            }

            // Mallet Hardness(CC16) - 64–+63
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Mallet Hardness CC16:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16 = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16";
                slEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16.Minimum = -64;
                slEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16.Maximum = 63;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] { tbEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16, slEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16 }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16.Text = 
                    "Mallet Hardness (CC16): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 - 64)).ToString();
            }

            // Resonance Level(CC16) - 64–+63
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Resonance Level CC16:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16 = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16";
                slEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16.Minimum = -64;
                slEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16.Maximum = 63;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16,
                    slEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16 }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16.Value =
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16.Text = 
                    "Resonance Level (CC16): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 - 64)).ToString();
            }

            // Roll Speed(CC17) - 64–+63
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Roll Speed CC17:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17 = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17.Name =
                    "slEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17";
                slEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17.Minimum = -64;
                slEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17.Maximum = 63;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17,
                    slEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17 }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17.Text = 
                    "Roll Speed (CC17): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64)).ToString();
            }

            // Glissando Mode(CC19) OFF, ON
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // CheckBox for Glissando Mode CC19
                CheckBox cbEditTone_SuperNATURALAcousticTone_Instrument_GlissandoModeCC19 = new CheckBox();
                cbEditTone_SuperNATURALAcousticTone_Instrument_GlissandoModeCC19.Tapped += GenericCheckBox_Click;
                cbEditTone_SuperNATURALAcousticTone_Instrument_GlissandoModeCC19.Click += GenericCheckBox_Click;
                cbEditTone_SuperNATURALAcousticTone_Instrument_GlissandoModeCC19.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_GlissandoModeCC19.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_GlissandoModeCC19.Content = "Glissando Mode (CC19)";
                cbEditTone_SuperNATURALAcousticTone_Instrument_GlissandoModeCC19.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_GlissandoModeCC19";

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_GlissandoModeCC19 })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_GlissandoModeCC19.IsChecked = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4 > 0;
            }

            // Play Scale
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Play Scale
                ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale = new ComboBox();
                cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale";
                cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale.Items.Add("Play Scale: 7th");
                cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale.Items.Add("Play Scale: Major");
                cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale.Items.Add("Play Scale: Minor");
                cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale.Items.Add("Play Scale: Harmonic Minor");
                cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale.Items.Add("Play Scale: Diminish");
                cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale.Items.Add("Play Scale: Whole Tone");

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale.SelectedIndex =
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter7;
            }

            // Scale Key C, Db, D, Eb, E, F, Gb, G, Ab, A, Bb, B
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Scale Key
                ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey = new ComboBox();
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Name =
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey";
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Items.Add("Scale Key: C");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Items.Add("Scale Key: Db");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Items.Add("Scale Key: D");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Items.Add("Scale Key: Eb");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Items.Add("Scale Key: E");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Items.Add("Scale Key: F");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Items.Add("Scale Key: Gb");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Items.Add("Scale Key: G");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Items.Add("Scale Key: Ab");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Items.Add("Scale Key: A");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Items.Add("Scale Key: Bb");
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.Items.Add("Scale Key: B");

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey.SelectedIndex =
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter8;
            }

            // Bend Depth(CC17) - 64–+63
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Scale Key:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_BendDepth);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_BendDepth = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_BendDepth.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_BendDepth.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_BendDepth.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_BendDepth.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey";
                slEditTone_SuperNATURALAcousticTone_Instrument_BendDepth.Minimum = -64;
                slEditTone_SuperNATURALAcousticTone_Instrument_BendDepth.Maximum = 63;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_BendDepth, slEditTone_SuperNATURALAcousticTone_Instrument_BendDepth }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_BendDepth.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_BendDepth.Text = 
                    "Scale Key: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64)).ToString();
            }

            // Buzz Key Switch OFF, ON
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // CheckBox for Buzz Key Switch
                CheckBox cbEditTone_SuperNATURALAcousticTone_Instrument_BuzzKeySwitch = new CheckBox();
                cbEditTone_SuperNATURALAcousticTone_Instrument_BuzzKeySwitch.Tapped += GenericCheckBox_Click;
                cbEditTone_SuperNATURALAcousticTone_Instrument_BuzzKeySwitch.Click += GenericCheckBox_Click;
                cbEditTone_SuperNATURALAcousticTone_Instrument_BuzzKeySwitch.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_BuzzKeySwitch.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_BuzzKeySwitch.Content = "Buzz Key Switch";
                cbEditTone_SuperNATURALAcousticTone_Instrument_BuzzKeySwitch.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_BuzzKeySwitch";

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_BuzzKeySwitch })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_BuzzKeySwitch.IsChecked =
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter12 > 0;
            }

            // Tambura Level - 64–+63
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Tambura Level:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel";
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel.Minimum = -64;
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel.Maximum = 63;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel,
                    slEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel.Text = 
                    "Tambura Level: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5 - 64)).ToString();
            }

            // Tambura Pitch - 12–+12
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Tambura Pitch:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch";
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch.Minimum = -12;
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch.Maximum = 12;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch, slEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch.Value =
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch.Text = 
                    "Tambura Pitch: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6 - 64)).ToString();
            }

            // Hold Legato Mode(CC19) OFF, ON
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // CheckBox for Hold Legato Mode CC19
                CheckBox cbEditTone_SuperNATURALAcousticTone_Instrument_HoldLegatoModeCC19 = new CheckBox();
                cbEditTone_SuperNATURALAcousticTone_Instrument_HoldLegatoModeCC19.Tapped += GenericCheckBox_Click;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HoldLegatoModeCC19.Click += GenericCheckBox_Click;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HoldLegatoModeCC19.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_HoldLegatoModeCC19.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_HoldLegatoModeCC19.Content = "Hold Legato Mode CC19";
                cbEditTone_SuperNATURALAcousticTone_Instrument_HoldLegatoModeCC19.Name =
                    "cbEditTone_SuperNATURALAcousticTone_HoldLegatoModeCC19";

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_HoldLegatoModeCC19 })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_HoldLegatoModeCC19.IsChecked = 
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4 > 0;
            }

            // Drone Level - 64–+63
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Drone Level:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel.Name =
                    "slEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel";
                slEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel.Minimum = -64;
                slEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel.Maximum = 63;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel,
                    slEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel.Value = 
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel.Text = "Drone Level: " +
                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5 - 64)).ToString();
            }

            // Drone Pitch - 12–+12
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // Slider for Drone Pitch:
                SetLabelProperties(ref tbEditTone_SuperNATURALAcousticTone_Instrument_DronePitch);
                Slider slEditTone_SuperNATURALAcousticTone_Instrument_DronePitch = new Slider();
                slEditTone_SuperNATURALAcousticTone_Instrument_DronePitch.ValueChanged += GenericSlider_ValueChanged;
                slEditTone_SuperNATURALAcousticTone_Instrument_DronePitch.GotFocus += Generic_GotFocus;
                slEditTone_SuperNATURALAcousticTone_Instrument_DronePitch.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                slEditTone_SuperNATURALAcousticTone_Instrument_DronePitch.Name = 
                    "slEditTone_SuperNATURALAcousticTone_Instrument_DronePitch";
                slEditTone_SuperNATURALAcousticTone_Instrument_DronePitch.Minimum = -12;
                slEditTone_SuperNATURALAcousticTone_Instrument_DronePitch.Maximum = 12;
                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    tbEditTone_SuperNATURALAcousticTone_Instrument_DronePitch,
                    slEditTone_SuperNATURALAcousticTone_Instrument_DronePitch }, new byte[] { 1, 2 })).Row);

                // Set control value
                slEditTone_SuperNATURALAcousticTone_Instrument_DronePitch.Value =
                    (superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6 - 64);
                tbEditTone_SuperNATURALAcousticTone_Instrument_DronePitch.Text = 
                    "Drone Pitch: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6 - 64)).ToString();
            }

            // Glide
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                // ComboBox for Glide
                ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_Glide = new ComboBox();
                cbEditTone_SuperNATURALAcousticTone_Instrument_Glide.SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_SuperNATURALAcousticTone_Instrument_Glide.GotFocus += Generic_GotFocus;
                cbEditTone_SuperNATURALAcousticTone_Instrument_Glide.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                cbEditTone_SuperNATURALAcousticTone_Instrument_Glide.Name = 
                    "cbEditTone_SuperNATURALAcousticTone_Instrument_Glide";
                cbEditTone_SuperNATURALAcousticTone_Instrument_Glide.Items.Add("Glide: Portamento");
                cbEditTone_SuperNATURALAcousticTone_Instrument_Glide.Items.Add("Glide: Glissando");

                // Put in row
                ControlsGrid.Children.Add((new GridRow(row++, new Control[] {
                    cbEditTone_SuperNATURALAcousticTone_Instrument_Glide })).Row);

                // Set control value
                cbEditTone_SuperNATURALAcousticTone_Instrument_Glide.SelectedIndex =
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter9;
            }

            // Variation Refer to p. 28.
            if (superNaturalAcousticInstrumentList.ParameterMask[index][((controlsIndex++) -2)] > 0)
            {
                superNATURALAcousticToneVariation = superNATURALAcousticToneVariations.Get(selectedSound.ProgramBank,
                    superNATURALAcousticTone.superNATURALAcousticToneCommon.Name.Trim());
                if (superNATURALAcousticToneVariation != null)
                {
                    // ComboBox for Inst Variation
                    ComboBox cbEditTone_SuperNATURALAcousticTone_Instrument_InstVariation = new ComboBox();
                    cbEditTone_SuperNATURALAcousticTone_Instrument_InstVariation.SelectionChanged += GenericCombobox_SelectionChanged;
                    cbEditTone_SuperNATURALAcousticTone_Instrument_InstVariation.GotFocus += Generic_GotFocus;
                    cbEditTone_SuperNATURALAcousticTone_Instrument_InstVariation.Tag = new HelpTag((byte)(controlsIndex - 1), 0);
                    cbEditTone_SuperNATURALAcousticTone_Instrument_InstVariation.Name =
                        "cbEditTone_SuperNATURALAcousticTone_Instrument_InstVariation";
                    cbEditTone_SuperNATURALAcousticTone_Instrument_InstVariation.Items.Add("Off");
                    foreach (String variation in superNATURALAcousticToneVariation.Variations)
                    {
                        cbEditTone_SuperNATURALAcousticTone_Instrument_InstVariation.Items.Add(variation);
                    }

                    // Put in row
                    ControlsGrid.Children.Add((new GridRow(row++, new Control[] { cbEditTone_SuperNATURALAcousticTone_Instrument_InstVariation })).Row);

                    // Set control value
                    cbEditTone_SuperNATURALAcousticTone_Instrument_InstVariation.SelectedIndex =
                        superNATURALAcousticTone.superNATURALAcousticToneCommon.InstVariation == 0 ? 
                        superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter10 :
                        superNATURALAcousticTone.superNATURALAcousticToneCommon.InstVariation -
                        superNATURALAcousticToneVariation.ComboBoxOffset;
                }
            }
        }

        private void AddSupernaturalAcousticToneMFXcontrolControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalAcousticToneMFXcontrolControls()");
            controlsIndex = 0;
            // Create controls

            ComboBox[] cbEditTone_CommonMFX_Control_MFXControlSource = new ComboBox[4];
            ComboBox[] cbEditTone_CommonMFX_Control_MFXControlAssign = new ComboBox[4];
            Slider[] slEditTone_CommonMFX_Control_MFXControlSens = new Slider[4];
            for (byte i = 0; i < 4; i++)
            {
                // ComboBox for MFX Control Source
                cbEditTone_CommonMFX_Control_MFXControlSource[i] = new ComboBox();
                cbEditTone_CommonMFX_Control_MFXControlSource[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_CommonMFX_Control_MFXControlSource[i].GotFocus += Generic_GotFocus;
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Name = 
                    "cbEditTone_CommonMFX_MFXControl_MFXControlSource" + i.ToString();
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Off");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC01");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC02");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC03");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC04");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC05");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC06");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC07");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC08");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC09");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC10");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC11");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC12");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC13");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC14");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC15");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC16");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC17");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC18");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC19");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC20");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC21");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC22");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC23");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC24");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC25");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC26");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC27");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC28");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC29");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC30");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC31");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC32");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC33");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC34");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC35");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC36");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC37");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC38");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC39");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC40");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC41");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC42");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC43");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC44");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC45");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC46");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC47");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC48");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC49");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC50");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC51");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC52");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC53");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC54");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC55");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC56");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC57");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC58");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC59");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC60");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC61");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC62");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC63");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC64");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC65");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC66");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC67");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC68");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC69");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC70");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC71");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC72");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC73");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC74");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC75");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC76");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC77");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC78");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC79");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC80");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC81");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC82");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC83");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC84");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC85");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC86");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC87");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC88");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC89");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC90");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC91");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC92");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC93");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC94");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC95");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Pitch bend");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " After touch");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 1");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 2");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 3");
                cbEditTone_CommonMFX_Control_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 4");

                // ComboBox for MFX Control Destination
                cbEditTone_CommonMFX_Control_MFXControlAssign[i] = new ComboBox();
                cbEditTone_CommonMFX_Control_MFXControlAssign[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_CommonMFX_Control_MFXControlAssign[i].GotFocus += Generic_GotFocus;
                cbEditTone_CommonMFX_Control_MFXControlAssign[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_CommonMFX_Control_MFXControlAssign[i].Name = 
                    "cbEditTone_CommonMFX_MFXControlAssign" + i.ToString();
                cbEditTone_CommonMFX_Control_MFXControlAssign[i].Items.Add("Destination : Off");
                cbEditTone_CommonMFX_Control_MFXControlAssign[i].Items.Add("Destination : Low gain");
                cbEditTone_CommonMFX_Control_MFXControlAssign[i].Items.Add("Destination : High gain");
                cbEditTone_CommonMFX_Control_MFXControlAssign[i].Items.Add("Destination : Level");

                // Slider for MFX Control Sense:
                tbEditTone_CommonMFX_Control_MFXControlSens[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_CommonMFX_Control_MFXControlSens[i]);
                slEditTone_CommonMFX_Control_MFXControlSens[i] = new Slider();
                slEditTone_CommonMFX_Control_MFXControlSens[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_CommonMFX_Control_MFXControlSens[i].GotFocus += Generic_GotFocus;
                slEditTone_CommonMFX_Control_MFXControlSens[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_CommonMFX_Control_MFXControlSens[i].Name = 
                    "slEditTone_CommonMFXControl_MFXControlSense" + i.ToString();
                slEditTone_CommonMFX_Control_MFXControlSens[i].Minimum = -63;
                slEditTone_CommonMFX_Control_MFXControlSens[i].Maximum = 63;
            }

            // Put in rows
            for (byte i = 0; i < 4; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(0 + 3 * i), new Control[] {
                    cbEditTone_CommonMFX_Control_MFXControlSource[i] })).Row);
                ControlsGrid.Children.Add((new GridRow((byte)(1 + 3 * i), new Control[] {
                    cbEditTone_CommonMFX_Control_MFXControlAssign[i] })).Row);
                ControlsGrid.Children.Add((new GridRow((byte)(2 + 3 * i), new Control[] {
                    tbEditTone_CommonMFX_Control_MFXControlSens[i],
                    slEditTone_CommonMFX_Control_MFXControlSens[i] }, new byte[] { 1, 2 })).Row);
            }

            // Set values
            handleControlEvents = false;
            for (byte i = 0; i < 4; i++)
            {
                cbEditTone_CommonMFX_Control_MFXControlSource[i].SelectedIndex = commonMFX.MFXControlSource[i];
                cbEditTone_CommonMFX_Control_MFXControlAssign[i].SelectedIndex = commonMFX.MFXControlAssign[i];
                slEditTone_CommonMFX_Control_MFXControlSens[i].Value = (commonMFX.MFXControlSens[i] - 64);
                tbEditTone_CommonMFX_Control_MFXControlSens[i].Text = 
                    "MFX Control " + (byte)(i + 1) + " Sense: " + ((commonMFX.MFXControlSens[i] - 64)).ToString();
            }
        }

        // SuperNatural Acoustic Tone Save controls
        private void AddSuperNaturalAcousticToneSaveControls()
        {
            t.Trace("private void AddSuperNaturalAcousticToneSaveControls()");
            controlsIndex = 0;

            // Create controls
            SetLabelProperties(ref tbEditTone_SaveTone_Title);
            Button btnEditTone_SuperNaturalAcousticTone_SaveTitle = new Button();
            btnEditTone_SuperNaturalAcousticTone_SaveTitle.Content = "Save";
            btnEditTone_SuperNaturalAcousticTone_SaveTitle.Click += btnEditTone_SaveTone_Click;
            Button btnEditTone_PCMSynthTone_DeleteTone = new Button();
            btnEditTone_PCMSynthTone_DeleteTone.Content = "Delete";
            btnEditTone_PCMSynthTone_DeleteTone.Click += btnEditTone_DeleteTone_Click;

            // Hook to help:
            tbEditTone_SaveTone_TitleText.GotFocus += Generic_GotFocus;
            tbEditTone_SaveTone_TitleText.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SaveTone_SlotNumber.GotFocus += Generic_GotFocus;
            cbEditTone_SaveTone_SlotNumber.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SaveTone_SlotNumber.SelectionChanged += CbEditTone_Save_SlotNumber_SelectionChanged;
            btnEditTone_SuperNaturalAcousticTone_SaveTitle.GotFocus += Generic_GotFocus;
            btnEditTone_SuperNaturalAcousticTone_SaveTitle.Tag = new HelpTag(controlsIndex++, 0);

            String numString;
            cbEditTone_SaveTone_SlotNumber.Items.Clear();
            if (commonState.toneNames[2] != null && commonState.toneNames[2].Count() == 256)
            {
                for (UInt16 i = 0; i < 256; i++)
                {
                    numString = (i + 1).ToString();
                    while (numString.Length < 3) numString = "0" + numString;
                    cbEditTone_SaveTone_SlotNumber.Items.Add(numString + ": " + commonState.toneNames[2][i]);
                }
            }
            else
            {
                for (UInt16 i = 0; i < 256; i++)
                {
                    numString = (i + 1).ToString();
                    while (numString.Length < 3) numString = "0" + numString;
                    cbEditTone_SaveTone_SlotNumber.Items.Add(numString + ": INIT TONE");
                }
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow((byte)(0), new Control[] { tbEditTone_SaveTone_Title,
                tbEditTone_SaveTone_TitleText, cbEditTone_SaveTone_SlotNumber, btnEditTone_SuperNaturalAcousticTone_SaveTitle,
                btnEditTone_PCMSynthTone_DeleteTone}, new byte[] { 4, 3, 3, 2, 2 })).Row);

            // Set values
            tbEditTone_SaveTone_Title.Text = "Name (max 12 chars):";
            tbEditTone_SaveTone_TitleText.Text = superNATURALAcousticTone.superNATURALAcousticToneCommon.Name;
            SetSaveSlotToFirstFreeOrSameName();
        }

        #endregion

        #region SuperNATURAL Synth Tone

        private void AddSupernaturalSynthToneCommonControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalSynthToneCommonControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // ComboBox for Phrase Number
            ComboBox cbEditTone_SuperNATURALSynthTone_Common_PhraseNumber = new ComboBox();
            cbEditTone_SuperNATURALSynthTone_Common_PhraseNumber.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_SuperNATURALSynthTone_Common_PhraseNumber.GotFocus += Generic_GotFocus;
            cbEditTone_SuperNATURALSynthTone_Common_PhraseNumber.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SuperNATURALSynthTone_Common_PhraseNumber.Name = "cbEditTone_SuperNATURALSynthTone_Common_PhraseNumber";
            UInt16 i = 1;
            foreach (String name in phrases.Names)
            {
                cbEditTone_SuperNATURALSynthTone_Common_PhraseNumber.Items.Add("Phrase " + (i++).ToString() + ": " + name);
            }

            // ComboBox for Phrase Octave Shift
            ComboBox cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift = new ComboBox();
            cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift.Name = "cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift";
            cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift.Items.Add("Phrase Octave Shift: -3");
            cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift.Items.Add("Phrase Octave Shift: -2");
            cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift.Items.Add("Phrase Octave Shift: -1");
            cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift.Items.Add("Phrase Octave Shift: 0");
            cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift.Items.Add("Phrase Octave Shift: +1");
            cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift.Items.Add("Phrase Octave Shift: +2");
            cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift.Items.Add("Phrase Octave Shift: +3");

            // Slider for Tone Level:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Common_ToneLevel);
            Slider slEditTone_superNATURALSynthTone_Common_ToneLevel = new Slider();
            slEditTone_superNATURALSynthTone_Common_ToneLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Common_ToneLevel.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Common_ToneLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Common_ToneLevel.Name = "slEditTone_superNATURALSynthTone_Common_ToneLevel";
            slEditTone_superNATURALSynthTone_Common_ToneLevel.Minimum = 0;
            slEditTone_superNATURALSynthTone_Common_ToneLevel.Maximum = 127;

            // CheckBox for RING Switch
            CheckBox cbEditTone_superNATURALSynthTone_Common_RINGSwitch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Common_RINGSwitch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Common_RINGSwitch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Common_RINGSwitch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Common_RINGSwitch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Common_RINGSwitch.Content = "RING Switch";
            cbEditTone_superNATURALSynthTone_Common_RINGSwitch.Name = "cbEditTone_superNATURALSynthTone_RINGSwitch";

            // Slider for Wave Shape:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Common_WaveShape);
            Slider slEditTone_superNATURALSynthTone_Common_WaveShape = new Slider();
            slEditTone_superNATURALSynthTone_Common_WaveShape.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Common_WaveShape.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Common_WaveShape.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Common_WaveShape.Name = "slEditTone_superNATURALSynthTone_Common_WaveShape";
            slEditTone_superNATURALSynthTone_Common_WaveShape.Minimum = 0;
            slEditTone_superNATURALSynthTone_Common_WaveShape.Maximum = 127;

            // Slider for Analog Feel:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Common_AnalogFeel);
            Slider slEditTone_superNATURALSynthTone_Common_AnalogFeel = new Slider();
            slEditTone_superNATURALSynthTone_Common_AnalogFeel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Common_AnalogFeel.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Common_AnalogFeel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Common_AnalogFeel.Name = "slEditTone_superNATURALSynthTone_Common_AnalogFeel";
            slEditTone_superNATURALSynthTone_Common_AnalogFeel.Minimum = 0;
            slEditTone_superNATURALSynthTone_Common_AnalogFeel.Maximum = 127;

            // CheckBox for Unison Switch
            CheckBox cbEditTone_superNATURALSynthTone_Common_UnisonSwitch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Common_UnisonSwitch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Common_UnisonSwitch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Common_UnisonSwitch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Common_UnisonSwitch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Common_UnisonSwitch.Content = "Unison Switch";
            cbEditTone_superNATURALSynthTone_Common_UnisonSwitch.Name = "cbEditTone_superNATURALSynthTone_UnisonSwitch";

            // ComboBox for Unison Size
            ComboBox cbEditTone_superNATURALSynthTone_Common_UnisonSize = new ComboBox();
            cbEditTone_superNATURALSynthTone_Common_UnisonSize.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Common_UnisonSize.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Common_UnisonSize.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Common_UnisonSize.Name = "cbEditTone_superNATURALSynthTone_Common_UnisonSize";
            cbEditTone_superNATURALSynthTone_Common_UnisonSize.Items.Add("Unison Size: 2");
            cbEditTone_superNATURALSynthTone_Common_UnisonSize.Items.Add("Unison Size: 4");
            cbEditTone_superNATURALSynthTone_Common_UnisonSize.Items.Add("Unison Size: 6");
            cbEditTone_superNATURALSynthTone_Common_UnisonSize.Items.Add("Unison Size: 8");

            // ComboBox for Mono Poly
            ComboBox cbEditTone_superNATURALSynthTone_Common_MonoPoly = new ComboBox();
            cbEditTone_superNATURALSynthTone_Common_MonoPoly.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Common_MonoPoly.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Common_MonoPoly.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Common_MonoPoly.Name = "cbEditTone_superNATURALSynthTone_Common_MonoPoly";
            cbEditTone_superNATURALSynthTone_Common_MonoPoly.Items.Add("Mono");
            cbEditTone_superNATURALSynthTone_Common_MonoPoly.Items.Add("Poly");

            // CheckBox for Legato Switch
            CheckBox cbEditTone_superNATURALSynthTone_Common_LegatoSwitch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Common_LegatoSwitch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Common_LegatoSwitch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Common_LegatoSwitch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Common_LegatoSwitch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Common_LegatoSwitch.Content = "Legato Switch";
            cbEditTone_superNATURALSynthTone_Common_LegatoSwitch.Name = "cbEditTone_superNATURALSynthTone_LegatoSwitch";

            // CheckBox for Portamento Switch
            CheckBox cbEditTone_superNATURALSynthTone_Common_PortamentoSwitch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Common_PortamentoSwitch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Common_PortamentoSwitch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Common_PortamentoSwitch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Common_PortamentoSwitch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Common_PortamentoSwitch.Content = "Portamento Switch";
            cbEditTone_superNATURALSynthTone_Common_PortamentoSwitch.Name = "cbEditTone_superNATURALSynthTone_PortamentoSwitch";

            // Slider for Portamento Time:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Common_PortamentoTime);
            Slider slEditTone_superNATURALSynthTone_Common_PortamentoTime = new Slider();
            slEditTone_superNATURALSynthTone_Common_PortamentoTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Common_PortamentoTime.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Common_PortamentoTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Common_PortamentoTime.Name = "slEditTone_superNATURALSynthTone_Common_PortamentoTime";
            slEditTone_superNATURALSynthTone_Common_PortamentoTime.Minimum = 0;
            slEditTone_superNATURALSynthTone_Common_PortamentoTime.Maximum = 127;

            // ComboBox for Portamento Mode
            ComboBox cbEditTone_superNATURALSynthTone_Common_PortamentoMode = new ComboBox();
            cbEditTone_superNATURALSynthTone_Common_PortamentoMode.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Common_PortamentoMode.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Common_PortamentoMode.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Common_PortamentoMode.Name = "cbEditTone_superNATURALSynthTone_Common_PortamentoMode";
            cbEditTone_superNATURALSynthTone_Common_PortamentoMode.Items.Add("Portamento Mode: Normal");
            cbEditTone_superNATURALSynthTone_Common_PortamentoMode.Items.Add("Portamento Mode: Legato");

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_SuperNATURALSynthTone_Common_PhraseNumber })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_superNATURALSynthTone_Common_ToneLevel,
                slEditTone_superNATURALSynthTone_Common_ToneLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { cbEditTone_superNATURALSynthTone_Common_RINGSwitch })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_superNATURALSynthTone_Common_WaveShape,
                slEditTone_superNATURALSynthTone_Common_WaveShape }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_superNATURALSynthTone_Common_AnalogFeel,
                slEditTone_superNATURALSynthTone_Common_AnalogFeel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { cbEditTone_superNATURALSynthTone_Common_UnisonSwitch })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { cbEditTone_superNATURALSynthTone_Common_UnisonSize })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { cbEditTone_superNATURALSynthTone_Common_MonoPoly })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { cbEditTone_superNATURALSynthTone_Common_LegatoSwitch })).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { cbEditTone_superNATURALSynthTone_Common_PortamentoSwitch })).Row);
            ControlsGrid.Children.Add((new GridRow(11, new Control[] { tbEditTone_superNATURALSynthTone_Common_PortamentoTime,
                slEditTone_superNATURALSynthTone_Common_PortamentoTime }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(12, new Control[] { cbEditTone_superNATURALSynthTone_Common_PortamentoMode })).Row);

            // Set control values
            cbEditTone_SuperNATURALSynthTone_Common_PhraseNumber.SelectedIndex = superNATURALSynthTone.superNATURALSynthToneCommon.PhraseNumber;
            cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift.SelectedIndex = superNATURALSynthTone.superNATURALSynthToneCommon.PhraseOctaveShift - 61;
            slEditTone_superNATURALSynthTone_Common_ToneLevel.Value = (superNATURALSynthTone.superNATURALSynthToneCommon.ToneLevel);
            tbEditTone_superNATURALSynthTone_Common_ToneLevel.Text = "Tone Level: " + ((superNATURALSynthTone.superNATURALSynthToneCommon.ToneLevel)).ToString();
            cbEditTone_superNATURALSynthTone_Common_RINGSwitch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.RINGSwitch;
            slEditTone_superNATURALSynthTone_Common_WaveShape.Value = (superNATURALSynthTone.superNATURALSynthToneCommon.WaveShape);
            tbEditTone_superNATURALSynthTone_Common_WaveShape.Text = "Wave Shape: " + ((superNATURALSynthTone.superNATURALSynthToneCommon.WaveShape)).ToString();
            slEditTone_superNATURALSynthTone_Common_AnalogFeel.Value = (superNATURALSynthTone.superNATURALSynthToneCommon.AnalogFeel);
            tbEditTone_superNATURALSynthTone_Common_AnalogFeel.Text = "Analog Feel: " + ((superNATURALSynthTone.superNATURALSynthToneCommon.AnalogFeel)).ToString();
            cbEditTone_superNATURALSynthTone_Common_UnisonSwitch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.UnisonSwitch;
            cbEditTone_superNATURALSynthTone_Common_UnisonSize.SelectedIndex = superNATURALSynthTone.superNATURALSynthToneCommon.UnisonSize;
            cbEditTone_superNATURALSynthTone_Common_MonoPoly.SelectedIndex = superNATURALSynthTone.superNATURALSynthToneCommon.MonoPoly;
            cbEditTone_superNATURALSynthTone_Common_LegatoSwitch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.LegatoSwitch;
            cbEditTone_superNATURALSynthTone_Common_PortamentoSwitch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.PortamentoSwitch;
            slEditTone_superNATURALSynthTone_Common_PortamentoTime.Value = (superNATURALSynthTone.superNATURALSynthToneCommon.PortamentoTime);
            tbEditTone_superNATURALSynthTone_Common_PortamentoTime.Text = "Portamento Time: " + ((superNATURALSynthTone.superNATURALSynthToneCommon.PortamentoTime)).ToString();
            cbEditTone_superNATURALSynthTone_Common_PortamentoMode.SelectedIndex = superNATURALSynthTone.superNATURALSynthToneCommon.PortamentoMode;
        }

        private void AddSupernaturalSynthToneOscControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalSynthToneWaveControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // CheckBox for Partial 1 Switch
            CheckBox cbEditTone_superNATURALSynthTone_Osc_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Osc_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Osc_Partial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Osc_Partial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Osc_Partial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Osc_Partial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Osc_Partial1Switch.Content = "Partial 1 Switch";
            cbEditTone_superNATURALSynthTone_Osc_Partial1Switch.Name = "cbEditTone_superNATURALSynthTone_Partial1Switch";

            // CheckBox for Partial 2 Switch
            CheckBox cbEditTone_superNATURALSynthTone_Osc_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Osc_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Osc_Partial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Osc_Partial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Osc_Partial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Osc_Partial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Osc_Partial2Switch.Content = "Partial 2 Switch";
            cbEditTone_superNATURALSynthTone_Osc_Partial2Switch.Name = "cbEditTone_superNATURALSynthTone_Partial2Switch";

            // CheckBox for Partial 3 Switch
            CheckBox cbEditTone_superNATURALSynthTone_Osc_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Osc_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Osc_Partial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Osc_Partial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Osc_Partial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Osc_Partial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Osc_Partial3Switch.Content = "Partial 3 Switch";
            cbEditTone_superNATURALSynthTone_Osc_Partial3Switch.Name = "cbEditTone_superNATURALSynthTone_Partial3Switch";

            // ComboBox for Wave Shape
            ComboBox cbEditTone_superNATURALSynthTone_Osc_WaveShape = new ComboBox();
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.Name = "cbEditTone_superNATURALSynthTone_Partial_OscWave";
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.Items.Add("Wave form: Sawtooth");
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.Items.Add("Wave form: Square");
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.Items.Add("Wave form: Phasewidth square");
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.Items.Add("Wave form: Triangle");
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.Items.Add("Wave form: Sine");
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.Items.Add("Wave form: Noise");
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.Items.Add("Wave form: Super saw");
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.Items.Add("Wave form: PCM waveform");

            // ComboBox for Wave Variation
            ComboBox cbEditTone_superNATURALSynthTone_Osc_WaveVariation = new ComboBox();
            cbEditTone_superNATURALSynthTone_Osc_WaveVariation.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Osc_WaveVariation.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Osc_WaveVariation.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Osc_WaveVariation.Name = "cbEditTone_superNATURALSynthTone_Osc_WaveVariation";
            cbEditTone_superNATURALSynthTone_Osc_WaveVariation.Items.Add("Wave Variation A");
            cbEditTone_superNATURALSynthTone_Osc_WaveVariation.Items.Add("Wave Variation B");
            cbEditTone_superNATURALSynthTone_Osc_WaveVariation.Items.Add("Wave Variation C");

            // ComboBox for Wave Number
            ComboBox cbEditTone_superNATURALSynthTone_Osc_WaveNumber = new ComboBox();
            cbEditTone_superNATURALSynthTone_Osc_WaveNumber.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Osc_WaveNumber.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Osc_WaveNumber.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Osc_WaveNumber.Name = "cbEditTone_superNATURALSynthTone_Osc_WaveNumber";
            UInt16 i = 1;
            foreach (String waveName in superNaturalSynthToneWaveNames.Names)
            {
                cbEditTone_superNATURALSynthTone_Osc_WaveNumber.Items.Add("Wave " + i.ToString() + ": " + waveName);
            }

            // ComboBox for Wave Gain
            ComboBox cbEditTone_superNATURALSynthTone_Osc_WaveGain = new ComboBox();
            cbEditTone_superNATURALSynthTone_Osc_WaveGain.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Osc_WaveGain.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Osc_WaveGain.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Osc_WaveGain.Name = "cbEditTone_superNATURALSynthTone_Osc_WaveGain";
            cbEditTone_superNATURALSynthTone_Osc_WaveGain.Items.Add("Wave Gain: -6 dB");
            cbEditTone_superNATURALSynthTone_Osc_WaveGain.Items.Add("Wave Gain: 0 dB");
            cbEditTone_superNATURALSynthTone_Osc_WaveGain.Items.Add("Wave Gain: +6 dB");
            cbEditTone_superNATURALSynthTone_Osc_WaveGain.Items.Add("Wave Gain: +12 dB");


            // Slider for Pulse Width Mod Depth:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth);
            Slider slEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth = new Slider();
            slEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth.Name = "slEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth";
            slEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth.Minimum = 0;
            slEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth.Maximum = 127;

            // Slider for Pulse Width Shift:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Osc_PulseWidthShift);
            Slider slEditTone_superNATURALSynthTone_Osc_PulseWidthShift = new Slider();
            slEditTone_superNATURALSynthTone_Osc_PulseWidthShift.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Osc_PulseWidthShift.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Osc_PulseWidthShift.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Osc_PulseWidthShift.Name = "slEditTone_superNATURALSynthTone_Osc_PulseWidthShift";
            slEditTone_superNATURALSynthTone_Osc_PulseWidthShift.Minimum = 0;
            slEditTone_superNATURALSynthTone_Osc_PulseWidthShift.Maximum = 127;

            // Slider for Super Saw Detune:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Osc_SuperSawDetune);
            Slider slEditTone_superNATURALSynthTone_Osc_SuperSawDetune = new Slider();
            slEditTone_superNATURALSynthTone_Osc_SuperSawDetune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Osc_SuperSawDetune.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Osc_SuperSawDetune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Osc_SuperSawDetune.Name = "slEditTone_superNATURALSynthTone_Osc_SuperSawDetune";
            slEditTone_superNATURALSynthTone_Osc_SuperSawDetune.Minimum = 0;
            slEditTone_superNATURALSynthTone_Osc_SuperSawDetune.Maximum = 127;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow((byte)(0), new Control[] { cbEditTone_superNATURALSynthTone_Osc_Partial1Switch,
            cbEditTone_superNATURALSynthTone_Osc_Partial2Switch, cbEditTone_superNATURALSynthTone_Osc_Partial3Switch})).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_superNATURALSynthTone_Osc_WaveShape })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { cbEditTone_superNATURALSynthTone_Osc_WaveVariation })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { cbEditTone_superNATURALSynthTone_Osc_WaveNumber })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { cbEditTone_superNATURALSynthTone_Osc_WaveGain })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth, slEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_superNATURALSynthTone_Osc_PulseWidthShift, slEditTone_superNATURALSynthTone_Osc_PulseWidthShift }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_superNATURALSynthTone_Osc_SuperSawDetune, slEditTone_superNATURALSynthTone_Osc_SuperSawDetune }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_superNATURALSynthTone_Osc_WaveShape.SelectedIndex = superNATURALSynthTone.superNATURALSynthTonePartial.OSCWave;
            cbEditTone_superNATURALSynthTone_Osc_Partial1Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial1Switch;
            cbEditTone_superNATURALSynthTone_Osc_Partial2Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial2Switch;
            cbEditTone_superNATURALSynthTone_Osc_Partial3Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial3Switch;
            cbEditTone_superNATURALSynthTone_Osc_WaveVariation.SelectedIndex = superNATURALSynthTone.superNATURALSynthTonePartial.OSCWaveVariation;
            cbEditTone_superNATURALSynthTone_Osc_WaveNumber.SelectedIndex = superNATURALSynthTone.superNATURALSynthTonePartial.WaveNumber;
            cbEditTone_superNATURALSynthTone_Osc_WaveGain.SelectedIndex = superNATURALSynthTone.superNATURALSynthTonePartial.WaveGain;
            slEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.OSCPulseWidthModDepth);
            tbEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth.Text = "Pulse Width Mod Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.OSCPulseWidthModDepth)).ToString();
            slEditTone_superNATURALSynthTone_Osc_PulseWidthShift.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.OSCPulseWidthShift);
            tbEditTone_superNATURALSynthTone_Osc_PulseWidthShift.Text = "Pulse Width Shift: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.OSCPulseWidthShift)).ToString();
            slEditTone_superNATURALSynthTone_Osc_SuperSawDetune.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.SuperSawDetune);
            tbEditTone_superNATURALSynthTone_Osc_SuperSawDetune.Text = "Super Saw Detune: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.SuperSawDetune)).ToString();
        }

        private void AddSupernaturalSynthTonePitchControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalSynthTonePitchControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // CheckBox for Partial 1 Switch
            CheckBox cbEditTone_superNATURALSynthTone_Pitch_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Pitch_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Pitch_Partial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Pitch_Partial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Pitch_Partial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Pitch_Partial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Pitch_Partial1Switch.Content = "Partial 1 Switch";
            cbEditTone_superNATURALSynthTone_Pitch_Partial1Switch.Name = "cbEditTone_superNATURALSynthTone_Partial1Switch";

            // CheckBox for Partial 2 Switch
            CheckBox cbEditTone_superNATURALSynthTone_Pitch_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Pitch_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Pitch_Partial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Pitch_Partial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Pitch_Partial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Pitch_Partial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Pitch_Partial2Switch.Content = "Partial 2 Switch";
            cbEditTone_superNATURALSynthTone_Pitch_Partial2Switch.Name = "cbEditTone_superNATURALSynthTone_Partial2Switch";

            // CheckBox for Partial 3 Switch
            CheckBox cbEditTone_superNATURALSynthTone_Pitch_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Pitch_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Pitch_Partial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Pitch_Partial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Pitch_Partial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Pitch_Partial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Pitch_Partial3Switch.Content = "Partial 3 Switch";
            cbEditTone_superNATURALSynthTone_Pitch_Partial3Switch.Name = "cbEditTone_superNATURALSynthTone_Partial3Switch";

            // Slider for OSC Pitch:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_OSCPitch);
            Slider slEditTone_superNATURALSynthTone_Pitch_OSCPitch = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_OSCPitch.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_OSCPitch.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_OSCPitch.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_OSCPitch.Name = "slEditTone_superNATURALSynthTone_Pitch_OSCPitch";
            slEditTone_superNATURALSynthTone_Pitch_OSCPitch.Minimum = -24;
            slEditTone_superNATURALSynthTone_Pitch_OSCPitch.Maximum = 24;

            // Slider for OSC Detune:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_OSCDetune);
            Slider slEditTone_superNATURALSynthTone_Pitch_OSCDetune = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_OSCDetune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_OSCDetune.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_OSCDetune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_OSCDetune.Name = "slEditTone_superNATURALSynthTone_Pitch_OSCDetune";
            slEditTone_superNATURALSynthTone_Pitch_OSCDetune.Minimum = -50;
            slEditTone_superNATURALSynthTone_Pitch_OSCDetune.Maximum = 50;

            // Slider for Pitch Env Attack Time:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime);
            Slider slEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime.Name = "slEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime";
            slEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime.Minimum = 0;
            slEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime.Maximum = 127;

            // Slider for OSC Pitch Env Decay:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay);
            Slider slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay.Name = "slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay";
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay.Minimum = 0;
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay.Maximum = 127;

            // Slider for OSC Pitch Env Depth:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth);
            Slider slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth.Name = "slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth";
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth.Minimum = -63;
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth.Maximum = 63;

            // ComboBox for OSC Octave Shift
            ComboBox cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift = new ComboBox();
            cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift.Name = "cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift";
            cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift.Items.Add("Octave Shift: -3");
            cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift.Items.Add("Octave Shift: -2");
            cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift.Items.Add("Octave Shift: -1");
            cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift.Items.Add("Octave Shift: 0");
            cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift.Items.Add("Octave Shift: +1");
            cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift.Items.Add("Octave Shift: +2");
            cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift.Items.Add("Octave Shift: +3");

            // Slider for Pitch Bend Range Up:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp);
            Slider slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp.Name = "slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp";
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp.Minimum = 0;
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp.Maximum = 24;

            // Slider for Pitch Bend Range Down:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown);
            Slider slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown.Name = "slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown";
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown.Minimum = 0;
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown.Maximum = 24;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow((byte)(0), new Control[] { cbEditTone_superNATURALSynthTone_Pitch_Partial1Switch,
            cbEditTone_superNATURALSynthTone_Pitch_Partial2Switch, cbEditTone_superNATURALSynthTone_Pitch_Partial3Switch})).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_OSCPitch, slEditTone_superNATURALSynthTone_Pitch_OSCPitch }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_OSCDetune, slEditTone_superNATURALSynthTone_Pitch_OSCDetune }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime, slEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay, slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth, slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp, slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown, slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_superNATURALSynthTone_Pitch_Partial1Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial1Switch;
            cbEditTone_superNATURALSynthTone_Pitch_Partial2Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial2Switch;
            cbEditTone_superNATURALSynthTone_Pitch_Partial3Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial3Switch;
            slEditTone_superNATURALSynthTone_Pitch_OSCPitch.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.OSCPitch - 64);
            tbEditTone_superNATURALSynthTone_Pitch_OSCPitch.Text = "OSC Pitch: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.OSCPitch - 64)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_OSCDetune.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.OSCDetune - 64);
            tbEditTone_superNATURALSynthTone_Pitch_OSCDetune.Text = "OSC Detune: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.OSCDetune - 64)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.OSCPitchEnvAttackTime);
            tbEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime.Text = "Pitch Env Attack Time: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.OSCPitchEnvAttackTime)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.OSCPitchEnvDecay);
            tbEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay.Text = "OSC Pitch Env Decay: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.OSCPitchEnvDecay)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.OSCPitchEnvDepth - 64);
            tbEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth.Text = "OSC Pitch Env Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.OSCPitchEnvDepth - 64)).ToString();
            cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift.SelectedIndex = superNATURALSynthTone.superNATURALSynthToneCommon.OctaveShift - 61;
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp.Value = (superNATURALSynthTone.superNATURALSynthToneCommon.PitchBendRangeUp);
            tbEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp.Text = "Pitch Bend Range Up: " + ((superNATURALSynthTone.superNATURALSynthToneCommon.PitchBendRangeUp)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown.Value = (superNATURALSynthTone.superNATURALSynthToneCommon.PitchBendRangeDown);
            tbEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown.Text = "Pitch Bend Range Down: " + ((superNATURALSynthTone.superNATURALSynthToneCommon.PitchBendRangeDown)).ToString();
        }

        private void AddSupernaturalSynthToneFilterControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalSynthTonePitchenvControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // CheckBox for Partial 1 Switch
            CheckBox cbEditTone_superNATURALSynthTone_Filter_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Filter_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Filter_Partial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Filter_Partial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Filter_Partial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Filter_Partial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Filter_Partial1Switch.Content = "Partial 1 Switch";
            cbEditTone_superNATURALSynthTone_Filter_Partial1Switch.Name = "cbEditTone_superNATURALSynthTone_Partial1Switch";

            // CheckBox for Partial 2 Switch
            CheckBox cbEditTone_superNATURALSynthTone_Filter_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Filter_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Filter_Partial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Filter_Partial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Filter_Partial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Filter_Partial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Filter_Partial2Switch.Content = "Partial 2 Switch";
            cbEditTone_superNATURALSynthTone_Filter_Partial2Switch.Name = "cbEditTone_superNATURALSynthTone_Partial2Switch";

            // CheckBox for Partial 3 Switch
            CheckBox cbEditTone_superNATURALSynthTone_Filter_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Filter_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_Filter_Partial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Filter_Partial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Filter_Partial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Filter_Partial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Filter_Partial3Switch.Content = "Partial 3 Switch";
            cbEditTone_superNATURALSynthTone_Filter_Partial3Switch.Name = "cbEditTone_superNATURALSynthTone_Partial3Switch";

            // ComboBox for FILTER Mode
            ComboBox cbEditTone_superNATURALSynthTone_Pitch_FILTERMode = new ComboBox();
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.Name = "cbEditTone_superNATURALSynthTone_Pitch_FILTERMode";
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.Items.Add("Filter mode: Bypass");
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.Items.Add("Filter mode: LPF1");
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.Items.Add("Filter mode: LPF2");
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.Items.Add("Filter mode: LPF3");
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.Items.Add("Filter mode: LPF4");
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.Items.Add("Filter mode: HPF");
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.Items.Add("Filter mode: BPF");
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.Items.Add("Filter mode: PKG");

            // ComboBox for FILTER Slope
            ComboBox cbEditTone_superNATURALSynthTone_Pitch_FILTERSlope = new ComboBox();
            cbEditTone_superNATURALSynthTone_Pitch_FILTERSlope.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Pitch_FILTERSlope.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Pitch_FILTERSlope.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Pitch_FILTERSlope.Name = "cbEditTone_superNATURALSynthTone_Pitch_FILTERSlope";
            cbEditTone_superNATURALSynthTone_Pitch_FILTERSlope.Items.Add("Filter slope: -12 dB");
            cbEditTone_superNATURALSynthTone_Pitch_FILTERSlope.Items.Add("Filter slope: -24 dB");

            // Slider for FILTER Cutoff:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_FILTERCutoff);
            Slider slEditTone_superNATURALSynthTone_Pitch_FILTERCutoff = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoff.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoff.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoff.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoff.Name = "slEditTone_superNATURALSynthTone_Pitch_FILTERCutoff";
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoff.Minimum = 0;
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoff.Maximum = 127;

            // Slider for FILTER Cutoff Keyfollow:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow);
            Slider slEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow.Name = "slEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow";
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow.Minimum = -10;
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow.Maximum = 10;

            // Slider for FILTER Env Velocity Sens:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens);
            Slider slEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens.Name = "slEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens";
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens.Minimum = -63;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens.Maximum = 63;

            // Slider for FILTER Resonance:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_FILTERResonance);
            Slider slEditTone_superNATURALSynthTone_Pitch_FILTERResonance = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_FILTERResonance.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_FILTERResonance.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_FILTERResonance.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_FILTERResonance.Name = "slEditTone_superNATURALSynthTone_Pitch_FILTERResonance";
            slEditTone_superNATURALSynthTone_Pitch_FILTERResonance.Minimum = 0;
            slEditTone_superNATURALSynthTone_Pitch_FILTERResonance.Maximum = 127;

            // Slider for FILTER Env Attack Time:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime);
            Slider slEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime.Name = "slEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime";
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime.Minimum = 0;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime.Maximum = 127;

            // Slider for FILTER Env Decay Time:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime);
            Slider slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime.Name = "slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime";
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime.Minimum = 0;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime.Maximum = 127;

            // Slider for FILTER Env Sustain Level:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel);
            Slider slEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel.Name = "slEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel";
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel.Minimum = 0;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel.Maximum = 127;

            // Slider for FILTER Env Release Time:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime);
            Slider slEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime.Name = "slEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime";
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime.Minimum = 0;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime.Maximum = 127;

            // Slider for FILTER Env Depth:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth);
            Slider slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth.Name = "slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth";
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth.Minimum = -63;
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth.Maximum = 63;

            // Slider for HPF Cutoff:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Pitch_HPFCutoff);
            Slider slEditTone_superNATURALSynthTone_Pitch_HPFCutoff = new Slider();
            slEditTone_superNATURALSynthTone_Pitch_HPFCutoff.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Pitch_HPFCutoff.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Pitch_HPFCutoff.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Pitch_HPFCutoff.Name = "slEditTone_superNATURALSynthTone_Pitch_HPFCutoff";
            slEditTone_superNATURALSynthTone_Pitch_HPFCutoff.Minimum = 0;
            slEditTone_superNATURALSynthTone_Pitch_HPFCutoff.Maximum = 127;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow((byte)(0), new Control[] { cbEditTone_superNATURALSynthTone_Filter_Partial1Switch,
            cbEditTone_superNATURALSynthTone_Filter_Partial2Switch, cbEditTone_superNATURALSynthTone_Filter_Partial3Switch})).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_superNATURALSynthTone_Pitch_FILTERMode })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { cbEditTone_superNATURALSynthTone_Pitch_FILTERSlope })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_FILTERCutoff, slEditTone_superNATURALSynthTone_Pitch_FILTERCutoff }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow, slEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens, slEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_FILTERResonance, slEditTone_superNATURALSynthTone_Pitch_FILTERResonance }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime, slEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime, slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel, slEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime, slEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(11, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth, slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(12, new Control[] { tbEditTone_superNATURALSynthTone_Pitch_HPFCutoff, slEditTone_superNATURALSynthTone_Pitch_HPFCutoff }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_superNATURALSynthTone_Filter_Partial1Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial1Switch;
            cbEditTone_superNATURALSynthTone_Filter_Partial2Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial2Switch;
            cbEditTone_superNATURALSynthTone_Filter_Partial3Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial3Switch;
            cbEditTone_superNATURALSynthTone_Pitch_FILTERMode.SelectedIndex = superNATURALSynthTone.superNATURALSynthTonePartial.FILTERMode;
            cbEditTone_superNATURALSynthTone_Pitch_FILTERSlope.SelectedIndex = superNATURALSynthTone.superNATURALSynthTonePartial.FILTERSlope;
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoff.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.FILTERCutoff);
            tbEditTone_superNATURALSynthTone_Pitch_FILTERCutoff.Text = "FILTER Cutoff: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.FILTERCutoff)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.FILTERCutoffKeyfollow - 64);
            tbEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow.Text = "FILTER Cutoff Keyfollow: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.FILTERCutoffKeyfollow - 64)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.FILTEREnvVelocitySens - 64);
            tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens.Text = "FILTER Env Velocity Sens: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.FILTEREnvVelocitySens - 64)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_FILTERResonance.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.FILTERResonance);
            tbEditTone_superNATURALSynthTone_Pitch_FILTERResonance.Text = "FILTER Resonance: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.FILTERResonance)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.FILTEREnvAttackTime);
            tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime.Text = "FILTER Env Attack Time: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.FILTEREnvAttackTime)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.FILTEREnvDecayTime);
            tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime.Text = "FILTER Env Decay Time: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.FILTEREnvDecayTime)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.FILTEREnvSustainLevel);
            tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel.Text = "FILTER Env Sustain Level: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.FILTEREnvSustainLevel)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.FILTEREnvReleaseTime);
            tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime.Text = "FILTER Env Release Time: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.FILTEREnvReleaseTime)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.FILTEREnvDepth - 64);
            tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth.Text = "FILTER Env Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.FILTEREnvDepth - 64)).ToString();
            slEditTone_superNATURALSynthTone_Pitch_HPFCutoff.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.HPFCutoff);
            tbEditTone_superNATURALSynthTone_Pitch_HPFCutoff.Text = "HPF Cutoff: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.HPFCutoff)).ToString();
        }

        private void AddSupernaturalSynthToneAMPControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalSynthToneTVFControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // CheckBox for Partial 1 Switch
            CheckBox cbEditTone_superNATURALSynthTone_AMP_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_AMP_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_AMP_Partial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_AMP_Partial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_AMP_Partial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_AMP_Partial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_AMP_Partial1Switch.Content = "Partial 1 Switch";
            cbEditTone_superNATURALSynthTone_AMP_Partial1Switch.Name = "cbEditTone_superNATURALSynthTone_Partial1Switch";

            // CheckBox for Partial 2 Switch
            CheckBox cbEditTone_superNATURALSynthTone_AMP_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_AMP_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_AMP_Partial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_AMP_Partial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_AMP_Partial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_AMP_Partial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_AMP_Partial2Switch.Content = "Partial 2 Switch";
            cbEditTone_superNATURALSynthTone_AMP_Partial2Switch.Name = "cbEditTone_superNATURALSynthTone_Partial2Switch";

            // CheckBox for Partial 3 Switch
            CheckBox cbEditTone_superNATURALSynthTone_AMP_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_AMP_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_AMP_Partial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_AMP_Partial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_AMP_Partial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_AMP_Partial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_AMP_Partial3Switch.Content = "Partial 3 Switch";
            cbEditTone_superNATURALSynthTone_AMP_Partial3Switch.Name = "cbEditTone_superNATURALSynthTone_Partial3Switch";

            // Slider for AMP Level:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Amp_AMPLevel);
            Slider slEditTone_superNATURALSynthTone_Amp_AMPLevel = new Slider();
            slEditTone_superNATURALSynthTone_Amp_AMPLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Amp_AMPLevel.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Amp_AMPLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Amp_AMPLevel.Name = "slEditTone_superNATURALSynthTone_Amp_AMPLevel";
            slEditTone_superNATURALSynthTone_Amp_AMPLevel.Minimum = 0;
            slEditTone_superNATURALSynthTone_Amp_AMPLevel.Maximum = 127;

            // Slider for AMP Level Velocity Sens:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens);
            Slider slEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens = new Slider();
            slEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens.Name = "slEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens";
            slEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens.Minimum = -63;
            slEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens.Maximum = 63;

            // Slider for AMP Pan:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Amp_AMPPan);
            Slider slEditTone_superNATURALSynthTone_Amp_AMPPan = new Slider();
            slEditTone_superNATURALSynthTone_Amp_AMPPan.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Amp_AMPPan.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Amp_AMPPan.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Amp_AMPPan.Name = "slEditTone_superNATURALSynthTone_Amp_AMPPan";
            slEditTone_superNATURALSynthTone_Amp_AMPPan.Minimum = -64;
            slEditTone_superNATURALSynthTone_Amp_AMPPan.Maximum = 63;

            // Slider for AMP Level Keyfollow:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow);
            Slider slEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow = new Slider();
            slEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow.Name = "slEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow";
            slEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow.Minimum = -10;
            slEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow.Maximum = 10;

            // Slider for AMP Env Attack Time:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime);
            Slider slEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime = new Slider();
            slEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime.Name = "slEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime";
            slEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime.Minimum = 0;
            slEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime.Maximum = 127;

            // Slider for AMP Env Decay Time:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime);
            Slider slEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime = new Slider();
            slEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime.Name = "slEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime";
            slEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime.Minimum = 0;
            slEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime.Maximum = 127;

            // Slider for AMP Env Sustain Level:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel);
            Slider slEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel = new Slider();
            slEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel.Name = "slEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel";
            slEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel.Minimum = 0;
            slEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel.Maximum = 127;

            // Slider for AMP Env Release Time:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime);
            Slider slEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime = new Slider();
            slEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime.Name = "slEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime";
            slEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime.Minimum = 0;
            slEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime.Maximum = 127;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow((byte)(0), new Control[] { cbEditTone_superNATURALSynthTone_AMP_Partial1Switch,
            cbEditTone_superNATURALSynthTone_AMP_Partial2Switch, cbEditTone_superNATURALSynthTone_AMP_Partial3Switch})).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_superNATURALSynthTone_Amp_AMPLevel, slEditTone_superNATURALSynthTone_Amp_AMPLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens, slEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_superNATURALSynthTone_Amp_AMPPan, slEditTone_superNATURALSynthTone_Amp_AMPPan }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow, slEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime, slEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime, slEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel, slEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime, slEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_superNATURALSynthTone_AMP_Partial1Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial1Switch;
            cbEditTone_superNATURALSynthTone_AMP_Partial2Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial2Switch;
            cbEditTone_superNATURALSynthTone_AMP_Partial3Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial3Switch;
            slEditTone_superNATURALSynthTone_Amp_AMPLevel.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.AMPLevel);
            tbEditTone_superNATURALSynthTone_Amp_AMPLevel.Text = "AMP Level: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.AMPLevel)).ToString();
            slEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.AMPLevelVelocitySens - 64);
            tbEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens.Text = "AMP Level Velocity Sens: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.AMPLevelVelocitySens - 64)).ToString();
            slEditTone_superNATURALSynthTone_Amp_AMPPan.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.AMPPan - 64);
            tbEditTone_superNATURALSynthTone_Amp_AMPPan.Text = "AMP Pan: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.AMPPan - 64)).ToString();
            slEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.AMPLevelKeyfollow - 64);
            tbEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow.Text = "AMP Level Keyfollow: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.AMPLevelKeyfollow - 64)).ToString();
            slEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.AMPEnvAttackTime);
            tbEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime.Text = "AMP Env Attack Time: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.AMPEnvAttackTime)).ToString();
            slEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.AMPEnvDecayTime);
            tbEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime.Text = "AMP Env Decay Time: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.AMPEnvDecayTime)).ToString();
            slEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.AMPEnvSustainLevel);
            tbEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel.Text = "AMP Env Sustain Level: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.AMPEnvSustainLevel)).ToString();
            slEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.AMPEnvReleaseTime);
            tbEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime.Text = "AMP Env Release Time: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.AMPEnvReleaseTime)).ToString();
        }

        private void AddSupernaturalSynthToneLFOControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalSynthToneTVFenvControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // CheckBox for Partial 1 Switch
            CheckBox cbEditTone_superNATURALSynthTone_LFO_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_LFO_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_LFO_Partial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_LFO_Partial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_LFO_Partial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_LFO_Partial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_LFO_Partial1Switch.Content = "Partial 1 Switch";
            cbEditTone_superNATURALSynthTone_LFO_Partial1Switch.Name = "cbEditTone_superNATURALSynthTone_Partial1Switch";

            // CheckBox for Partial 2 Switch
            CheckBox cbEditTone_superNATURALSynthTone_LFO_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_LFO_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_LFO_Partial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_LFO_Partial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_LFO_Partial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_LFO_Partial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_LFO_Partial2Switch.Content = "Partial 2 Switch";
            cbEditTone_superNATURALSynthTone_LFO_Partial2Switch.Name = "cbEditTone_superNATURALSynthTone_Partial2Switch";

            // CheckBox for Partial 3 Switch
            CheckBox cbEditTone_superNATURALSynthTone_LFO_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_LFO_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_LFO_Partial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_LFO_Partial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_LFO_Partial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_LFO_Partial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_LFO_Partial3Switch.Content = "Partial 3 Switch";
            cbEditTone_superNATURALSynthTone_LFO_Partial3Switch.Name = "cbEditTone_superNATURALSynthTone_Partial3Switch";

            // ComboBox for LFO Shape
            ComboBox cbEditTone_superNATURALSynthTone_LFO_LFOShape = new ComboBox();
            cbEditTone_superNATURALSynthTone_LFO_LFOShape.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_LFO_LFOShape.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_LFO_LFOShape.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_LFO_LFOShape.Name = "cbEditTone_superNATURALSynthTone_LFO_LFOShape";
            cbEditTone_superNATURALSynthTone_LFO_LFOShape.Items.Add("LFO Shape: Triangle");
            cbEditTone_superNATURALSynthTone_LFO_LFOShape.Items.Add("LFO Shape: Sine");
            cbEditTone_superNATURALSynthTone_LFO_LFOShape.Items.Add("LFO Shape: Sawtooth");
            cbEditTone_superNATURALSynthTone_LFO_LFOShape.Items.Add("LFO Shape: Square");
            cbEditTone_superNATURALSynthTone_LFO_LFOShape.Items.Add("LFO Shape: Sample & hold");
            cbEditTone_superNATURALSynthTone_LFO_LFOShape.Items.Add("LFO Shape: Random");

            // Slider for LFO Rate:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_LFORate);
            Slider slEditTone_superNATURALSynthTone_LFO_LFORate = new Slider();
            slEditTone_superNATURALSynthTone_LFO_LFORate.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_LFORate.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_LFORate.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_LFORate.Name = "slEditTone_superNATURALSynthTone_LFO_LFORate";
            slEditTone_superNATURALSynthTone_LFO_LFORate.Minimum = 0;
            slEditTone_superNATURALSynthTone_LFO_LFORate.Maximum = 127;

            // CheckBox for LFO Tempo Sync Switch
            CheckBox cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncSwitch = new CheckBox();
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncSwitch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncSwitch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncSwitch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncSwitch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncSwitch.Content = "LFO Tempo Sync Switch";
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncSwitch.Name = "cbEditTone_superNATURALSynthTone_LFOTempoSyncSwitch";

            // ComboBox for LFO Tempo Sync Note
            ComboBox cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote = new ComboBox();
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Name = "cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote";
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 16");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 12");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 8");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 4");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 2");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 3/4");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 2/3");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/2");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 3/8");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/3");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/4");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 3/16");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/6");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/8");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 3/32");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/12");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/16");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/24");
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/32");

            // Slider for LFO Fade Time:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_LFOFadeTime);
            Slider slEditTone_superNATURALSynthTone_LFO_LFOFadeTime = new Slider();
            slEditTone_superNATURALSynthTone_LFO_LFOFadeTime.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_LFOFadeTime.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_LFOFadeTime.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_LFOFadeTime.Name = "slEditTone_superNATURALSynthTone_LFO_LFOFadeTime";
            slEditTone_superNATURALSynthTone_LFO_LFOFadeTime.Minimum = 0;
            slEditTone_superNATURALSynthTone_LFO_LFOFadeTime.Maximum = 127;

            // CheckBox for LFO Key Trigger
            CheckBox cbEditTone_superNATURALSynthTone_LFO_LFOKeyTrigger = new CheckBox();
            cbEditTone_superNATURALSynthTone_LFO_LFOKeyTrigger.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_LFO_LFOKeyTrigger.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_LFO_LFOKeyTrigger.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_LFO_LFOKeyTrigger.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_LFO_LFOKeyTrigger.Content = "LFO Key Trigger";
            cbEditTone_superNATURALSynthTone_LFO_LFOKeyTrigger.Name = "cbEditTone_superNATURALSynthTone_LFOKeyTrigger";

            // Slider for LFO Pitch Depth:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_LFOPitchDepth);
            Slider slEditTone_superNATURALSynthTone_LFO_LFOPitchDepth = new Slider();
            slEditTone_superNATURALSynthTone_LFO_LFOPitchDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_LFOPitchDepth.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_LFOPitchDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_LFOPitchDepth.Name = "slEditTone_superNATURALSynthTone_LFO_LFOPitchDepth";
            slEditTone_superNATURALSynthTone_LFO_LFOPitchDepth.Minimum = -63;
            slEditTone_superNATURALSynthTone_LFO_LFOPitchDepth.Maximum = 63;

            // Slider for LFO Filter Depth:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_LFOFilterDepth);
            Slider slEditTone_superNATURALSynthTone_LFO_LFOFilterDepth = new Slider();
            slEditTone_superNATURALSynthTone_LFO_LFOFilterDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_LFOFilterDepth.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_LFOFilterDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_LFOFilterDepth.Name = "slEditTone_superNATURALSynthTone_LFO_LFOFilterDepth";
            slEditTone_superNATURALSynthTone_LFO_LFOFilterDepth.Minimum = -63;
            slEditTone_superNATURALSynthTone_LFO_LFOFilterDepth.Maximum = 63;

            // Slider for LFO AMP Depth:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_LFOAMPDepth);
            Slider slEditTone_superNATURALSynthTone_LFO_LFOAMPDepth = new Slider();
            slEditTone_superNATURALSynthTone_LFO_LFOAMPDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_LFOAMPDepth.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_LFOAMPDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_LFOAMPDepth.Name = "slEditTone_superNATURALSynthTone_LFO_LFOAMPDepth";
            slEditTone_superNATURALSynthTone_LFO_LFOAMPDepth.Minimum = -63;
            slEditTone_superNATURALSynthTone_LFO_LFOAMPDepth.Maximum = 63;

            // Slider for LFO Pan Depth:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_LFOPanDepth);
            Slider slEditTone_superNATURALSynthTone_LFO_LFOPanDepth = new Slider();
            slEditTone_superNATURALSynthTone_LFO_LFOPanDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_LFOPanDepth.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_LFOPanDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_LFOPanDepth.Name = "slEditTone_superNATURALSynthTone_LFO_LFOPanDepth";
            slEditTone_superNATURALSynthTone_LFO_LFOPanDepth.Minimum = -63;
            slEditTone_superNATURALSynthTone_LFO_LFOPanDepth.Maximum = 63;
            
            // Put in rows
            ControlsGrid.Children.Add((new GridRow((byte)(0), new Control[] { cbEditTone_superNATURALSynthTone_LFO_Partial1Switch,
            cbEditTone_superNATURALSynthTone_LFO_Partial2Switch, cbEditTone_superNATURALSynthTone_LFO_Partial3Switch})).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_superNATURALSynthTone_LFO_LFOShape })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_superNATURALSynthTone_LFO_LFORate, slEditTone_superNATURALSynthTone_LFO_LFORate }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncSwitch })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_superNATURALSynthTone_LFO_LFOFadeTime, slEditTone_superNATURALSynthTone_LFO_LFOFadeTime }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { cbEditTone_superNATURALSynthTone_LFO_LFOKeyTrigger })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_superNATURALSynthTone_LFO_LFOPitchDepth, slEditTone_superNATURALSynthTone_LFO_LFOPitchDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_superNATURALSynthTone_LFO_LFOFilterDepth, slEditTone_superNATURALSynthTone_LFO_LFOFilterDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { tbEditTone_superNATURALSynthTone_LFO_LFOAMPDepth, slEditTone_superNATURALSynthTone_LFO_LFOAMPDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { tbEditTone_superNATURALSynthTone_LFO_LFOPanDepth, slEditTone_superNATURALSynthTone_LFO_LFOPanDepth }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_superNATURALSynthTone_LFO_Partial1Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial1Switch;
            cbEditTone_superNATURALSynthTone_LFO_Partial2Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial2Switch;
            cbEditTone_superNATURALSynthTone_LFO_Partial3Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial3Switch;
            cbEditTone_superNATURALSynthTone_LFO_LFOShape.SelectedIndex = superNATURALSynthTone.superNATURALSynthTonePartial.LFOShape;
            slEditTone_superNATURALSynthTone_LFO_LFORate.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.LFORate);
            tbEditTone_superNATURALSynthTone_LFO_LFORate.Text = "LFO Rate: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.LFORate)).ToString();
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncSwitch.IsChecked = superNATURALSynthTone.superNATURALSynthTonePartial.LFOTempoSyncSwitch;
            cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote.SelectedIndex = superNATURALSynthTone.superNATURALSynthTonePartial.LFOTempoSyncNote;
            slEditTone_superNATURALSynthTone_LFO_LFOFadeTime.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.LFOFadeTime);
            tbEditTone_superNATURALSynthTone_LFO_LFOFadeTime.Text = "LFO Fade Time: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.LFOFadeTime)).ToString();
            cbEditTone_superNATURALSynthTone_LFO_LFOKeyTrigger.IsChecked = superNATURALSynthTone.superNATURALSynthTonePartial.LFOKeyTrigger;
            slEditTone_superNATURALSynthTone_LFO_LFOPitchDepth.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.LFOPitchDepth - 64);
            tbEditTone_superNATURALSynthTone_LFO_LFOPitchDepth.Text = "LFO Pitch Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.LFOPitchDepth - 64)).ToString();
            slEditTone_superNATURALSynthTone_LFO_LFOFilterDepth.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.LFOFilterDepth - 64);
            tbEditTone_superNATURALSynthTone_LFO_LFOFilterDepth.Text = "LFO Filter Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.LFOFilterDepth - 64)).ToString();
            slEditTone_superNATURALSynthTone_LFO_LFOAMPDepth.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.LFOAmpDepth - 64);
            tbEditTone_superNATURALSynthTone_LFO_LFOAMPDepth.Text = "LFO AMP Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.LFOAmpDepth - 64)).ToString();
            slEditTone_superNATURALSynthTone_LFO_LFOPanDepth.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.LFOPanDepth - 64);
            tbEditTone_superNATURALSynthTone_LFO_LFOPanDepth.Text = "LFO Pan Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.LFOPanDepth - 64)).ToString();
        }

        private void AddSupernaturalSynthToneModLFOControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalSynthToneTVAControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // CheckBox for Partial 1 Switch
            CheckBox cbEditTone_superNATURALSynthTone_ModLFO_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_ModLFO_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_ModLFO_Partial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_ModLFO_Partial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_ModLFO_Partial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_ModLFO_Partial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_ModLFO_Partial1Switch.Content = "Partial 1 Switch";
            cbEditTone_superNATURALSynthTone_ModLFO_Partial1Switch.Name = "cbEditTone_superNATURALSynthTone_Partial1Switch";

            // CheckBox for Partial 2 Switch
            CheckBox cbEditTone_superNATURALSynthTone_ModLFO_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_ModLFO_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_ModLFO_Partial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_ModLFO_Partial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_ModLFO_Partial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_ModLFO_Partial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_ModLFO_Partial2Switch.Content = "Partial 2 Switch";
            cbEditTone_superNATURALSynthTone_ModLFO_Partial2Switch.Name = "cbEditTone_superNATURALSynthTone_Partial2Switch";

            // CheckBox for Partial 3 Switch
            CheckBox cbEditTone_superNATURALSynthTone_ModLFO_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_ModLFO_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_ModLFO_Partial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_ModLFO_Partial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_ModLFO_Partial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_ModLFO_Partial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_ModLFO_Partial3Switch.Content = "Partial 3 Switch";
            cbEditTone_superNATURALSynthTone_ModLFO_Partial3Switch.Name = "cbEditTone_superNATURALSynthTone_Partial3Switch";

            // ComboBox for Modulation LFO Shape
            ComboBox cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape = new ComboBox();
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape.Name = "cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape";
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape.Items.Add("Modulation LFO Shape: Triangle");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape.Items.Add("Modulation LFO Shape: Sine");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape.Items.Add("Modulation LFO Shape: Sawtooth");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape.Items.Add("Modulation LFO Shape: Square");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape.Items.Add("Modulation LFO Shape: Sample & hold");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape.Items.Add("Modulation LFO Shape: Random");

            // Slider for Modulation LFO Rate:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_ModulationLFORate);
            Slider slEditTone_superNATURALSynthTone_LFO_ModulationLFORate = new Slider();
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORate.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORate.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORate.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORate.Name = "slEditTone_superNATURALSynthTone_LFO_ModulationLFORate";
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORate.Minimum = 0;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORate.Maximum = 127;

            // CheckBox for Modulation LFO Tempo Sync Switch
            CheckBox cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncSwitch = new CheckBox();
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncSwitch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncSwitch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncSwitch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncSwitch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncSwitch.Content = "Modulation LFO Tempo Sync Switch";
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncSwitch.Name = "cbEditTone_superNATURALSynthTone_ModulationLFOTempoSyncSwitch";

            // ComboBox for Modulation LFO Tempo Sync Note
            ComboBox cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote = new ComboBox();
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Name = "cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote";
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 16");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 12");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 8");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 4");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 2");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 3/4");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 2/3");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/2");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 3/8");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/3");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/4");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 3/16");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/6");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/8");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 3/32");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/12");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/16");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/24");
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.Items.Add("LFO Tempo Sync Note: 1/32");

            // Slider for Modulation LFO Pitch Depth:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth);
            Slider slEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth = new Slider();
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth.Name = "slEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth";
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth.Minimum = -63;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth.Maximum = 63;

            // Slider for Modulation LFO Filter Depth:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth);
            Slider slEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth = new Slider();
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth.Name = "slEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth";
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth.Minimum = -63;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth.Maximum = 63;

            // Slider for Modulation LFO Amp Depth:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth);
            Slider slEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth = new Slider();
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth.Name = "slEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth";
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth.Minimum = -63;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth.Maximum = 63;

            // Slider for Modulation LFO Pan Depth:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth);
            Slider slEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth = new Slider();
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth.Name = "slEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth";
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth.Minimum = -63;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth.Maximum = 63;

            // Slider for Modulation LFO Rate Control:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl);
            Slider slEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl = new Slider();
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl.Name = "slEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl";
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl.Minimum = -63;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl.Maximum = 63;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow((byte)(0), new Control[] { cbEditTone_superNATURALSynthTone_ModLFO_Partial1Switch,
            cbEditTone_superNATURALSynthTone_ModLFO_Partial2Switch, cbEditTone_superNATURALSynthTone_ModLFO_Partial3Switch})).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_superNATURALSynthTone_LFO_ModulationLFORate, slEditTone_superNATURALSynthTone_LFO_ModulationLFORate }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncSwitch })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth, slEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth, slEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth, slEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth, slEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { tbEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl, slEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_superNATURALSynthTone_ModLFO_Partial1Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial1Switch;
            cbEditTone_superNATURALSynthTone_ModLFO_Partial2Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial2Switch;
            cbEditTone_superNATURALSynthTone_ModLFO_Partial3Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial3Switch;
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape.SelectedIndex = superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFOShape;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORate.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFORate);
            tbEditTone_superNATURALSynthTone_LFO_ModulationLFORate.Text = "Modulation LFO Rate: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFORate)).ToString();
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncSwitch.IsChecked = superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFOTempoSyncSwitch;
            cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote.SelectedIndex = superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFOTempoSyncNote;
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFOPitchDepth - 64);
            tbEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth.Text = "Modulation LFO Pitch Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFOPitchDepth - 64)).ToString();
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFOFilterDepth - 64);
            tbEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth.Text = "Modulation LFO Filter Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFOFilterDepth - 64)).ToString();
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFOAmpDepth - 64);
            tbEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth.Text = "Modulation LFO Amp Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFOAmpDepth - 64)).ToString();
            slEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFOPanDepth - 64);
            tbEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth.Text = "Modulation LFO Pan Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFOPanDepth - 64)).ToString();
            slEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFORateControl - 64);
            tbEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl.Text = "Modulation LFO Rate Control: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.ModulationLFORateControl - 64)).ToString();
        }

        private void AddSupernaturalSynthToneAftertouchControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalSynthToneTVAenvControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // CheckBox for Partial 1 Switch
            CheckBox cbEditTone_superNATURALSynthTone_TVAenv_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_TVAenv_Partial1Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_TVAenv_Partial1Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_TVAenv_Partial1Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_TVAenv_Partial1Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_TVAenv_Partial1Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_TVAenv_Partial1Switch.Content = "Partial 1 Switch";
            cbEditTone_superNATURALSynthTone_TVAenv_Partial1Switch.Name = "cbEditTone_superNATURALSynthTone_Partial1Switch";

            // CheckBox for Partial 2 Switch
            CheckBox cbEditTone_superNATURALSynthTone_TVAenv_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_TVAenv_Partial2Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_TVAenv_Partial2Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_TVAenv_Partial2Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_TVAenv_Partial2Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_TVAenv_Partial2Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_TVAenv_Partial2Switch.Content = "Partial 2 Switch";
            cbEditTone_superNATURALSynthTone_TVAenv_Partial2Switch.Name = "cbEditTone_superNATURALSynthTone_Partial2Switch";

            // CheckBox for Partial 3 Switch
            CheckBox cbEditTone_superNATURALSynthTone_TVAenv_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_TVAenv_Partial3Switch = new CheckBox();
            cbEditTone_superNATURALSynthTone_TVAenv_Partial3Switch.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_TVAenv_Partial3Switch.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_TVAenv_Partial3Switch.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_TVAenv_Partial3Switch.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_TVAenv_Partial3Switch.Content = "Partial 3 Switch";
            cbEditTone_superNATURALSynthTone_TVAenv_Partial3Switch.Name = "cbEditTone_superNATURALSynthTone_Partial3Switch";

            // Slider for Cutoff Aftertouch Sens:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens);
            Slider slEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens = new Slider();
            slEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens.Name = "slEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens";
            slEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens.Minimum = -63;
            slEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens.Maximum = 63;

            // Slider for Level Aftertouch Sens:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens);
            Slider slEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens = new Slider();
            slEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens.Name = "slEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens";
            slEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens.Minimum = -63;
            slEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens.Maximum = 63;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow((byte)(0), new Control[] { cbEditTone_superNATURALSynthTone_TVAenv_Partial1Switch,
            cbEditTone_superNATURALSynthTone_TVAenv_Partial2Switch, cbEditTone_superNATURALSynthTone_TVAenv_Partial3Switch})).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens, slEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens, slEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_superNATURALSynthTone_TVAenv_Partial1Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial1Switch;
            cbEditTone_superNATURALSynthTone_TVAenv_Partial2Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial2Switch;
            cbEditTone_superNATURALSynthTone_TVAenv_Partial3Switch.IsChecked = superNATURALSynthTone.superNATURALSynthToneCommon.Partial3Switch;
            slEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.CutoffAftertouchSens - 64);
            tbEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens.Text = "Cutoff Aftertouch Sens: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.CutoffAftertouchSens - 64)).ToString();
            slEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens.Value = (superNATURALSynthTone.superNATURALSynthTonePartial.LevelAftertouchSens - 64);
            tbEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens.Text = "Level Aftertouch Sens: " + ((superNATURALSynthTone.superNATURALSynthTonePartial.LevelAftertouchSens - 64)).ToString();
        }

        private void AddSupernaturalSynthToneMiscControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalSynthToneOutputControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // Slider for Attack Time Interval Sens:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens);
            Slider slEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens = new Slider();
            slEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens.Name = "slEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens";
            slEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens.Minimum = 0;
            slEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens.Maximum = 127;

            // Slider for Release Time Interval Sens:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens);
            Slider slEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens = new Slider();
            slEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens.Name = "slEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens";
            slEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens.Minimum = 0;
            slEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens.Maximum = 127;

            // Slider for Portamento Time Interval Sens:
            SetLabelProperties(ref tbEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens);
            Slider slEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens = new Slider();
            slEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens.Name = "slEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens";
            slEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens.Minimum = 0;
            slEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens.Maximum = 127;

            // ComboBox for Envelope Loop Mode
            ComboBox cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopMode = new ComboBox();
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopMode.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopMode.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopMode.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopMode.Name = "cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopMode";
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopMode.Items.Add("Envelope Loop: Off");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopMode.Items.Add("Envelope Loop: Free-run");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopMode.Items.Add("Envelope Loop: Tempo-sync");

            // ComboBox for Envelope Loop Sync Note
            ComboBox cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote = new ComboBox();
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Name = "cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote";
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 16");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 12");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 8");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 4");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 2");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 1/1");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 3/4");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 2/3");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 1/2");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 3/8");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 1/3");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 1/4");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 3/16");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 1/6");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 1/8");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 3/32");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 1/12");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 1/16");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 1/24");
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.Items.Add("Envelope Loop Sync Note: 1/32");

            // CheckBox for Chromatic Portamento
            CheckBox cbEditTone_superNATURALSynthTone_Misc_ChromaticPortamento = new CheckBox();
            cbEditTone_superNATURALSynthTone_Misc_ChromaticPortamento.Tapped += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Misc_ChromaticPortamento.Click += GenericCheckBox_Click;
            cbEditTone_superNATURALSynthTone_Misc_ChromaticPortamento.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALSynthTone_Misc_ChromaticPortamento.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALSynthTone_Misc_ChromaticPortamento.Content = "Chromatic Portamento";
            cbEditTone_superNATURALSynthTone_Misc_ChromaticPortamento.Name = "cbEditTone_superNATURALSynthTone_ChromaticPortamento";

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { tbEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens, slEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens, slEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens, slEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopMode })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { cbEditTone_superNATURALSynthTone_Misc_ChromaticPortamento })).Row);

            // Set control values
            slEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens.Value = (superNATURALSynthTone.superNATURALSynthToneMisc.AttackTimeIntervalSens);
            tbEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens.Text = "Attack Time Interval Sens: " + ((superNATURALSynthTone.superNATURALSynthToneMisc.AttackTimeIntervalSens)).ToString();
            slEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens.Value = (superNATURALSynthTone.superNATURALSynthToneMisc.ReleaseTimeIntervalSens);
            tbEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens.Text = "Release Time Interval Sens: " + ((superNATURALSynthTone.superNATURALSynthToneMisc.ReleaseTimeIntervalSens)).ToString();
            slEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens.Value = (superNATURALSynthTone.superNATURALSynthToneMisc.PortamentoTimeIntervalSens);
            tbEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens.Text = "Portamento Time Interval Sens: " + ((superNATURALSynthTone.superNATURALSynthToneMisc.PortamentoTimeIntervalSens)).ToString();
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopMode.SelectedIndex = superNATURALSynthTone.superNATURALSynthToneMisc.EnvelopeLoopMode;
            cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote.SelectedIndex = superNATURALSynthTone.superNATURALSynthToneMisc.EnvelopeLoopSyncNote;
            cbEditTone_superNATURALSynthTone_Misc_ChromaticPortamento.IsChecked = superNATURALSynthTone.superNATURALSynthToneMisc.ChromaticPortamento;
        }

        private void AddSuperNaturalSynthToneMFXControlControls()
        {
            t.Trace("private void AddSuperNaturalSynthToneMFXControlControls()");
            controlsIndex = 0;
            // Create controls

            ComboBox[] cbEditTone_CommonMFX_MFXControl_MFXControlSource = new ComboBox[4];
            ComboBox[] cbEditTone_CommonMFX_MFXControl_MFXControlAssign = new ComboBox[4];
            Slider[] slEditTone_CommonMFX_MFXControl_MFXControlSens = new Slider[4];
            for (byte i = 0; i < 4; i++)
            {
                // ComboBox for MFX Control Source
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i] = new ComboBox();
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].GotFocus += Generic_GotFocus;
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Name = "cbEditTone_CommonMFX_MFXControl_MFXControlSource" + i.ToString();
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Off");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC01");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC02");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC03");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC04");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC05");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC06");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC07");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC08");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC09");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC10");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC11");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC12");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC13");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC14");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC15");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC16");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC17");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC18");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC19");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC20");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC21");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC22");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC23");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC24");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC25");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC26");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC27");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC28");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC29");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC30");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC31");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC32");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC33");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC34");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC35");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC36");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC37");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC38");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC39");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC40");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC41");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC42");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC43");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC44");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC45");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC46");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC47");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC48");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC49");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC50");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC51");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC52");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC53");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC54");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC55");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC56");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC57");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC58");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC59");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC60");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC61");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC62");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC63");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC64");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC65");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC66");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC67");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC68");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC69");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC70");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC71");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC72");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC73");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC74");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC75");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC76");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC77");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC78");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC79");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC80");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC81");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC82");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC83");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC84");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC85");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC86");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC87");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC88");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC89");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC90");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC91");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC92");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC93");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC94");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC95");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Pitch bend");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " After touch");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 1");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 2");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 3");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 4");

                // ComboBox for MFX Control Destination
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i] = new ComboBox();
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].GotFocus += Generic_GotFocus;
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].Name = "cbEditTone_CommonMFX_MFXControl_MFXControlAssign" + i.ToString();
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].Items.Add("Destination : Off");
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].Items.Add("Destination : Low gain");
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].Items.Add("Destination : High gain");
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].Items.Add("Destination : Level");

                // Slider for MFX Control Sense:
                tbEditTone_CommonMFX_MFXControl_MFXControlSens[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_CommonMFX_MFXControl_MFXControlSens[i]);
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i] = new Slider();
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].GotFocus += Generic_GotFocus;
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].Name = "slEditTone_CommonMFX_MFXControl_MFXControlSense" + i.ToString();
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].Minimum = -63;
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].Maximum = 63;
            }

            // Put in rows
            for (byte i = 0; i < 4; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(0 + 3 * i), new Control[] { cbEditTone_CommonMFX_MFXControl_MFXControlSource[i] })).Row);
                ControlsGrid.Children.Add((new GridRow((byte)(1 + 3 * i), new Control[] { cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i] })).Row);
                ControlsGrid.Children.Add((new GridRow((byte)(2 + 3 * i), new Control[] { tbEditTone_CommonMFX_MFXControl_MFXControlSens[i], slEditTone_CommonMFX_MFXControl_MFXControlSens[i] }, new byte[] { 1, 2 })).Row);
            }

            // Set values
            handleControlEvents = false;
            for (byte i = 0; i < 4; i++)
            {
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].SelectedIndex = commonMFX.MFXControlSource[i];
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].SelectedIndex = commonMFX.MFXControlAssign[i];
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].Value = (commonMFX.MFXControlSens[i] - 64);
                tbEditTone_CommonMFX_MFXControl_MFXControlSens[i].Text = "MFX Control " + (byte)(i + 1) + " Sense: " + ((commonMFX.MFXControlSens[i] - 64)).ToString();
            }
        }

        // SuperNATURAL Synth Tone Save controls
        private void AddSuperNaturalSynthToneSaveControls()
        {
            t.Trace("private void AddSuperNaturalSynthToneSaveControls()");
            controlsIndex = 0;

            // Create controls
            SetLabelProperties(ref tbEditTone_SaveTone_Title);
            Button btnEditTone_SuperNaturalSynthTone_SaveTitle = new Button();
            btnEditTone_SuperNaturalSynthTone_SaveTitle.Content = "Save";
            btnEditTone_SuperNaturalSynthTone_SaveTitle.Click += btnEditTone_SaveTone_Click;
            Button btnEditTone_PCMSynthTone_DeleteTone = new Button();
            btnEditTone_PCMSynthTone_DeleteTone.Content = "Delete";
            btnEditTone_PCMSynthTone_DeleteTone.Click += btnEditTone_DeleteTone_Click;

            // Hook to help:
            tbEditTone_SaveTone_TitleText.GotFocus += Generic_GotFocus;
            tbEditTone_SaveTone_TitleText.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SaveTone_SlotNumber.GotFocus += Generic_GotFocus;
            cbEditTone_SaveTone_SlotNumber.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SaveTone_SlotNumber.SelectionChanged += CbEditTone_Save_SlotNumber_SelectionChanged;
            btnEditTone_SuperNaturalSynthTone_SaveTitle.GotFocus += Generic_GotFocus;
            btnEditTone_SuperNaturalSynthTone_SaveTitle.Tag = new HelpTag(controlsIndex++, 0);

            String numString;
            cbEditTone_SaveTone_SlotNumber.Items.Clear();
            if (commonState.toneNames[3] != null && commonState.toneNames[3].Count() == 512)
            {
                for (UInt16 i = 0; i < 512; i++)
                {
                    numString = (i + 1).ToString();
                    while (numString.Length < 3) numString = "0" + numString;
                    cbEditTone_SaveTone_SlotNumber.Items.Add(numString + ": " + commonState.toneNames[3][i]);
                }
            }
            else
            {
                for (UInt16 i = 0; i < 512; i++)
                {
                    numString = (i + 1).ToString();
                    while (numString.Length < 3) numString = "0" + numString;
                    cbEditTone_SaveTone_SlotNumber.Items.Add(numString + ": INIT TONE");
                }
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow((byte)(0), new Control[] { tbEditTone_SaveTone_Title,
                tbEditTone_SaveTone_TitleText, cbEditTone_SaveTone_SlotNumber, btnEditTone_SuperNaturalSynthTone_SaveTitle,
                btnEditTone_PCMSynthTone_DeleteTone}, new byte[] { 4, 3, 3, 2, 2 })).Row);

            // Set values
            tbEditTone_SaveTone_Title.Text = "Name (max 12 chars):";
            tbEditTone_SaveTone_TitleText.Text = superNATURALSynthTone.superNATURALSynthToneCommon.Name;
            SetSaveSlotToFirstFreeOrSameName();
        }


        #endregion

        #region SuperNATURAL Drum Kit

        private void AddSupernaturalDrumKitCommonControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalDrumKitCommonControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // ComboBox for Phrase Number
            ComboBox cbEditTone_superNATURALDrumKit_Common_PhraseNumber = new ComboBox();
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Name = "cbEditTone_superNATURALDrumKit_Common_PhraseNumber";
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 0: No assign");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 1: Standard 1");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 2: Jazz 1");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 3: Brush 1");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 4: Orchestra 1");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 5: Standard 2");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 6: Standard 3");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 7: Standard 3 with percussion");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 8: Rock 1");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 9: Rock 1 with percussion");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 10: Rock 2");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 11: Rock 2 with percussion");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 12: Jazz 2");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 13: Jazz 2 with percussion");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 14: Brush 2");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 15: Brush 2 with percussion");
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.Items.Add("Phrase Number 16: Orchestra 2");

            // Slider for Kit Level:
            SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Common_KitLevel);
            Slider slEditTone_superNATURALDrumKit_Common_KitLevel = new Slider();
            slEditTone_superNATURALDrumKit_Common_KitLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALDrumKit_Common_KitLevel.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALDrumKit_Common_KitLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALDrumKit_Common_KitLevel.Name = "slEditTone_superNATURALDrumKit_Common_KitLevel";
            slEditTone_superNATURALDrumKit_Common_KitLevel.Minimum = 0;
            slEditTone_superNATURALDrumKit_Common_KitLevel.Maximum = 127;

            // Slider for Ambience Level:
            SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Common_AmbienceLevel);
            Slider slEditTone_superNATURALDrumKit_Common_AmbienceLevel = new Slider();
            slEditTone_superNATURALDrumKit_Common_AmbienceLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALDrumKit_Common_AmbienceLevel.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALDrumKit_Common_AmbienceLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALDrumKit_Common_AmbienceLevel.Name = "slEditTone_superNATURALDrumKit_Common_AmbienceLevel";
            slEditTone_superNATURALDrumKit_Common_AmbienceLevel.Minimum = 0;
            slEditTone_superNATURALDrumKit_Common_AmbienceLevel.Maximum = 127;

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_superNATURALDrumKit_Common_PhraseNumber })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { tbEditTone_superNATURALDrumKit_Common_KitLevel, slEditTone_superNATURALDrumKit_Common_KitLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_superNATURALDrumKit_Common_AmbienceLevel, slEditTone_superNATURALDrumKit_Common_AmbienceLevel }, new byte[] { 1, 2 })).Row);

            // Set control values
            cbEditTone_superNATURALDrumKit_Common_PhraseNumber.SelectedIndex = superNATURALDrumKit.superNATURALDrumKitCommon.PhraseNumber;
            slEditTone_superNATURALDrumKit_Common_KitLevel.Value = (superNATURALDrumKit.superNATURALDrumKitCommon.KitLevel);
            tbEditTone_superNATURALDrumKit_Common_KitLevel.Text = "Kit Level: " + ((superNATURALDrumKit.superNATURALDrumKitCommon.KitLevel)).ToString();
            slEditTone_superNATURALDrumKit_Common_AmbienceLevel.Value = (superNATURALDrumKit.superNATURALDrumKitCommon.AmbienceLevel);
            tbEditTone_superNATURALDrumKit_Common_AmbienceLevel.Text = "Ambience Level: " + ((superNATURALDrumKit.superNATURALDrumKitCommon.AmbienceLevel)).ToString();
        }

        private void AddSupernaturalDrumKitDruminstrumentControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalDrumKitDruminstrumentControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // Instrument bank is a bit special. Bank and sound number are put together at address 0 (4 nibbles!)
            // There is only one expansion bank that contains SuperNATURAL Drum Kits: ExSN6
            // Internal sounds start at 0x00 0x00 0x00 0x00
            // ExSN6 sounds start at    0x00 0x00 0x0a 0x09
            // Remember that the addressing is nibble only, i.e. ExSN6 starts at 0xa9 (169)
            // So, we simply make a combobox with the texts Internal and ExSN6
            // The selected index multiplied with 169 will be an offset to add (nibblewise) to the sound number.
            // When using the offset (and instrument number) remember to use a key offset address!
            // ComboBox for Bank Number
            cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber.Name = "cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber";
            if (cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber.Items.Count() == 0)
            {
                cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber.Items.Add("Instrument bank: Internal");
                cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber.Items.Add("Instrument bank: ExSN6 (only if loaded)");
            }

            // ComboBox for Inst Number INT
            cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.Tag = new HelpTag(controlsIndex, 0); // Next control uses the same help text! Only one of theese controls are visible at any one time.
            cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.Name = "cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber";
            if (cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.Items.Count() == 0)
            {
                foreach (DrumInstrument drumInstrument in superNATURALDrumKitInstrumentList.DrumInstruments)
                {
                    if (drumInstrument.Bank == "INT")
                    {
                        cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.Items.Add(drumInstrument.Group +
                            " " + drumInstrument.Number.ToString() + ": " + drumInstrument.Name);
                    }
                }
            }

            // ComboBox for Inst Number ExSN6
            cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.Name = "cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber";
            if (cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.Items.Count == 0)
            {
                foreach (DrumInstrument drumInstrument in superNATURALDrumKitInstrumentList.DrumInstruments)
                {
                    if (drumInstrument.Bank == "ExSN6")
                    {
                        cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.Items.Add(drumInstrument.Group +
                            " " + drumInstrument.Number.ToString() + ": " + drumInstrument.Name);
                    }
                }
            }

            // Slider for Level:
            SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Druminstrument_Level);
            Slider slEditTone_superNATURALDrumKit_Druminstrument_Level = new Slider();
            slEditTone_superNATURALDrumKit_Druminstrument_Level.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALDrumKit_Druminstrument_Level.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALDrumKit_Druminstrument_Level.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALDrumKit_Druminstrument_Level.Name = "slEditTone_superNATURALDrumKit_Druminstrument_Level";
            slEditTone_superNATURALDrumKit_Druminstrument_Level.Minimum = 0;
            slEditTone_superNATURALDrumKit_Druminstrument_Level.Maximum = 127;

            // Slider for Pan:
            SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Druminstrument_Pan);
            Slider slEditTone_superNATURALDrumKit_Druminstrument_Pan = new Slider();
            slEditTone_superNATURALDrumKit_Druminstrument_Pan.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALDrumKit_Druminstrument_Pan.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALDrumKit_Druminstrument_Pan.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALDrumKit_Druminstrument_Pan.Name = "slEditTone_superNATURALDrumKit_Druminstrument_Pan";
            slEditTone_superNATURALDrumKit_Druminstrument_Pan.Minimum = -64;
            slEditTone_superNATURALDrumKit_Druminstrument_Pan.Maximum = 63;

            // Slider for Chorus Send Level:
            SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel);
            Slider slEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel = new Slider();
            slEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel.Name = "slEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel";
            slEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel.Minimum = 0;
            slEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel.Maximum = 127;

            // Slider for Reverb Send Level:
            SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel);
            Slider slEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel = new Slider();
            slEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel.Name = "slEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel";
            slEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel.Minimum = 0;
            slEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel.Maximum = 127;

            // Slider for Tune:
            SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Druminstrument_Tune);
            Slider slEditTone_superNATURALDrumKit_Druminstrument_Tune = new Slider();
            slEditTone_superNATURALDrumKit_Druminstrument_Tune.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALDrumKit_Druminstrument_Tune.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALDrumKit_Druminstrument_Tune.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALDrumKit_Druminstrument_Tune.Name = "slEditTone_superNATURALDrumKit_Druminstrument_Tune";
            slEditTone_superNATURALDrumKit_Druminstrument_Tune.Minimum = -120;
            slEditTone_superNATURALDrumKit_Druminstrument_Tune.Maximum = 120;

            // Slider for Attack:
            SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Druminstrument_Attack);
            Slider slEditTone_superNATURALDrumKit_Druminstrument_Attack = new Slider();
            slEditTone_superNATURALDrumKit_Druminstrument_Attack.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALDrumKit_Druminstrument_Attack.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALDrumKit_Druminstrument_Attack.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALDrumKit_Druminstrument_Attack.Name = "slEditTone_superNATURALDrumKit_Druminstrument_Attack";
            slEditTone_superNATURALDrumKit_Druminstrument_Attack.Minimum = 0;
            slEditTone_superNATURALDrumKit_Druminstrument_Attack.Maximum = 100;

            // Slider for Decay:
            SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Druminstrument_Decay);
            Slider slEditTone_superNATURALDrumKit_Druminstrument_Decay = new Slider();
            slEditTone_superNATURALDrumKit_Druminstrument_Decay.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALDrumKit_Druminstrument_Decay.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALDrumKit_Druminstrument_Decay.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALDrumKit_Druminstrument_Decay.Name = "slEditTone_superNATURALDrumKit_Druminstrument_Decay";
            slEditTone_superNATURALDrumKit_Druminstrument_Decay.Minimum = -63;
            slEditTone_superNATURALDrumKit_Druminstrument_Decay.Maximum = 0;

            // Slider for Brilliance:
            SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Druminstrument_Brilliance);
            Slider slEditTone_superNATURALDrumKit_Druminstrument_Brilliance = new Slider();
            slEditTone_superNATURALDrumKit_Druminstrument_Brilliance.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALDrumKit_Druminstrument_Brilliance.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALDrumKit_Druminstrument_Brilliance.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALDrumKit_Druminstrument_Brilliance.Name = "slEditTone_superNATURALDrumKit_Druminstrument_Brilliance";
            slEditTone_superNATURALDrumKit_Druminstrument_Brilliance.Minimum = -15;
            slEditTone_superNATURALDrumKit_Druminstrument_Brilliance.Maximum = 12;

            // Slider for Dynamic Range:
            SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Druminstrument_DynamicRange);
            Slider slEditTone_superNATURALDrumKit_Druminstrument_DynamicRange = new Slider();
            slEditTone_superNATURALDrumKit_Druminstrument_DynamicRange.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALDrumKit_Druminstrument_DynamicRange.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALDrumKit_Druminstrument_DynamicRange.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALDrumKit_Druminstrument_DynamicRange.Name = "slEditTone_superNATURALDrumKit_Druminstrument_DynamicRange";
            slEditTone_superNATURALDrumKit_Druminstrument_DynamicRange.Minimum = 0;
            slEditTone_superNATURALDrumKit_Druminstrument_DynamicRange.Maximum = 63;

            // Slider for Stereo Width:
            SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Druminstrument_StereoWidth);
            Slider slEditTone_superNATURALDrumKit_Druminstrument_StereoWidth = new Slider();
            slEditTone_superNATURALDrumKit_Druminstrument_StereoWidth.ValueChanged += GenericSlider_ValueChanged;
            slEditTone_superNATURALDrumKit_Druminstrument_StereoWidth.GotFocus += Generic_GotFocus;
            slEditTone_superNATURALDrumKit_Druminstrument_StereoWidth.Tag = new HelpTag(controlsIndex++, 0);
            slEditTone_superNATURALDrumKit_Druminstrument_StereoWidth.Name = "slEditTone_superNATURALDrumKit_Druminstrument_StereoWidth";
            slEditTone_superNATURALDrumKit_Druminstrument_StereoWidth.Minimum = 0;
            slEditTone_superNATURALDrumKit_Druminstrument_StereoWidth.Maximum = 127;

            // ComboBox for Variation
            cbEditTone_superNATURALDrumKit_Druminstrument_Variation.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALDrumKit_Druminstrument_Variation.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALDrumKit_Druminstrument_Variation.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALDrumKit_Druminstrument_Variation.Name = "cbEditTone_superNATURALDrumKit_Druminstrument_Variation";
            // superNATURALDrumKitInstrumentList.DrumInstruments contains (probably all) SuperNATURAL Drum instruments
            // with variations in a '/'-separated list. Call Variations() on the instrument to get all available variations.
            // An instrument can be fetched by calling instrument.Get(String Bank, String Name).
            DrumInstrument drumInstrumentForVariations = 
                superNATURALDrumKitInstrumentList.DrumInstruments[superNATURALDrumKit
                .superNATURALDrumKitKey.InstNumber[superNATURALDrumKit.superNATURALDrumKitKey.BankNumber]];
            cbEditTone_superNATURALDrumKit_Druminstrument_Variation.Items.Clear();
            if (drumInstrumentForVariations != null)
            {
                List<String> variations = drumInstrumentForVariations.Variations();
                foreach (String variation in variations)
                {
                    cbEditTone_superNATURALDrumKit_Druminstrument_Variation.Items.Add("Variation: " + variation);
                }
            }
            else
            {
                cbEditTone_superNATURALDrumKit_Druminstrument_Variation.Items.Add("Variation: Off");
            }

            // ComboBox for Output Assign
            ComboBox cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign = new ComboBox();
            cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign.GotFocus += Generic_GotFocus;
            cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign.Name = "cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign";
            cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign.Items.Add("Output assign: Part");
            cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign.Items.Add("Output assign: Comp+Eq1");
            cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign.Items.Add("Output assign: Comp+Eq2");
            cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign.Items.Add("Output assign: Comp+Eq3");
            cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign.Items.Add("Output assign: Comp+Eq4");
            cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign.Items.Add("Output assign: Comp+Eq5");
            cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign.Items.Add("Output assign: Comp+Eq6");

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6 })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_superNATURALDrumKit_Druminstrument_Level, slEditTone_superNATURALDrumKit_Druminstrument_Level }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_superNATURALDrumKit_Druminstrument_Pan, slEditTone_superNATURALDrumKit_Druminstrument_Pan }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel, slEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { tbEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel, slEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_superNATURALDrumKit_Druminstrument_Tune, slEditTone_superNATURALDrumKit_Druminstrument_Tune }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_superNATURALDrumKit_Druminstrument_Attack, slEditTone_superNATURALDrumKit_Druminstrument_Attack }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_superNATURALDrumKit_Druminstrument_Decay, slEditTone_superNATURALDrumKit_Druminstrument_Decay }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { tbEditTone_superNATURALDrumKit_Druminstrument_Brilliance, slEditTone_superNATURALDrumKit_Druminstrument_Brilliance }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { tbEditTone_superNATURALDrumKit_Druminstrument_DynamicRange, slEditTone_superNATURALDrumKit_Druminstrument_DynamicRange }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(11, new Control[] { tbEditTone_superNATURALDrumKit_Druminstrument_StereoWidth, slEditTone_superNATURALDrumKit_Druminstrument_StereoWidth }, new byte[] { 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(12, new Control[] { cbEditTone_superNATURALDrumKit_Druminstrument_Variation, cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign })).Row);

            // Set control values
            cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber.SelectedIndex = superNATURALDrumKit.superNATURALDrumKitKey.BankNumber;
            switch (superNATURALDrumKit.superNATURALDrumKitKey.BankNumber)
            {
                case 0:
                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.Visibility = Visibility.Visible;
                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.Visibility = Visibility.Collapsed;
                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.IsEnabled = true;
                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.IsEnabled = false;
                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.SelectedIndex = superNATURALDrumKit.superNATURALDrumKitKey.InstNumber[0];
                    break;
                case 1:
                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.Visibility = Visibility.Collapsed;
                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.Visibility = Visibility.Visible;
                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.IsEnabled = false;
                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.IsEnabled = true;
                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.SelectedIndex = superNATURALDrumKit.superNATURALDrumKitKey.InstNumber[1];
                    break;
            }
            slEditTone_superNATURALDrumKit_Druminstrument_Level.Value = (superNATURALDrumKit.superNATURALDrumKitKey.Level);
            tbEditTone_superNATURALDrumKit_Druminstrument_Level.Text = "Level: " + ((superNATURALDrumKit.superNATURALDrumKitKey.Level)).ToString();
            slEditTone_superNATURALDrumKit_Druminstrument_Pan.Value = (superNATURALDrumKit.superNATURALDrumKitKey.Pan - 64);
            tbEditTone_superNATURALDrumKit_Druminstrument_Pan.Text = "Pan: " + ((superNATURALDrumKit.superNATURALDrumKitKey.Pan - 64)).ToString();
            slEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel.Value = (superNATURALDrumKit.superNATURALDrumKitKey.ChorusSendLevel);
            tbEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel.Text = "Chorus Send Level: " + ((superNATURALDrumKit.superNATURALDrumKitKey.ChorusSendLevel)).ToString();
            slEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel.Value = (superNATURALDrumKit.superNATURALDrumKitKey.ReverbSendLevel);
            tbEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel.Text = "Reverb Send Level: " + ((superNATURALDrumKit.superNATURALDrumKitKey.ReverbSendLevel)).ToString();
            slEditTone_superNATURALDrumKit_Druminstrument_Tune.Value = (superNATURALDrumKit.superNATURALDrumKitKey.Tune - 128);
            tbEditTone_superNATURALDrumKit_Druminstrument_Tune.Text = "Tune: " + (superNATURALDrumKit.superNATURALDrumKitKey.Tune - 128).ToString();
            slEditTone_superNATURALDrumKit_Druminstrument_Attack.Value = (superNATURALDrumKit.superNATURALDrumKitKey.Attack);
            tbEditTone_superNATURALDrumKit_Druminstrument_Attack.Text = "Attack: " + ((superNATURALDrumKit.superNATURALDrumKitKey.Attack)).ToString();
            slEditTone_superNATURALDrumKit_Druminstrument_Decay.Value = (superNATURALDrumKit.superNATURALDrumKitKey.Decay - 64);
            tbEditTone_superNATURALDrumKit_Druminstrument_Decay.Text = "Decay: " + ((superNATURALDrumKit.superNATURALDrumKitKey.Decay - 64)).ToString();
            slEditTone_superNATURALDrumKit_Druminstrument_Brilliance.Value = (superNATURALDrumKit.superNATURALDrumKitKey.Brilliance - 64);
            tbEditTone_superNATURALDrumKit_Druminstrument_Brilliance.Text = "Brilliance: " + ((superNATURALDrumKit.superNATURALDrumKitKey.Brilliance - 64)).ToString();
            slEditTone_superNATURALDrumKit_Druminstrument_DynamicRange.Value = (superNATURALDrumKit.superNATURALDrumKitKey.DynamicRange);
            tbEditTone_superNATURALDrumKit_Druminstrument_DynamicRange.Text = "Dynamic Range: " + ((superNATURALDrumKit.superNATURALDrumKitKey.DynamicRange)).ToString();
            slEditTone_superNATURALDrumKit_Druminstrument_StereoWidth.Value = (superNATURALDrumKit.superNATURALDrumKitKey.StereoWidth);
            tbEditTone_superNATURALDrumKit_Druminstrument_StereoWidth.Text = "Stereo Width: " + ((superNATURALDrumKit.superNATURALDrumKitKey.StereoWidth)).ToString();
            cbEditTone_superNATURALDrumKit_Druminstrument_Variation.SelectedIndex = superNATURALDrumKit.superNATURALDrumKitKey.Variation;
            cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign.SelectedIndex = superNATURALDrumKit.superNATURALDrumKitKey.OutputAssign;
        }

        private void AddSupernaturalDrumKitCompressorControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalDrumKitCompressorControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;

            // CheckBox for Comp Switch
            CheckBox[] cbEditTone_superNATURALDrumKit_Compressor_CompSwitch = new CheckBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[i] = new CheckBox();
                cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[i].Tapped += GenericCheckBox_Click;
                cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[i].Click += GenericCheckBox_Click;
                cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[i].GotFocus += Generic_GotFocus;
                cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[i].Content = "Comp " + (i + 1).ToString();
                cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[i].Name = "cbEditTone_superNATURALDrumKit_CompSwitch" + i.ToString();
            }

            // ComboBox for Comp Attack Time
            ComboBox[] cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i] = new ComboBox();
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].GotFocus += Generic_GotFocus;
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Name = "cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime" + i.ToString() + "";
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.05 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.06 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.07 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.08 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.09 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.1 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.2 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.3 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.4 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.5 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.6 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.7 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.8 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 0.9 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 1.0 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 2.0 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 3.0 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 4.0 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 5.0 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 6.0 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 7.0 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 8.0 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 9.0 ms");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 10.0 mS");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 15.0 mS");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 20.0 mS");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 25.0 mS");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 30.0 mS");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 35.0 mS");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 40.0 mS");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 45.0 mS");
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].Items.Add("Attack time " + (i + 1).ToString() + ": 50.0 mS");
            }

            // ComboBox for Release Time
            ComboBox[] cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i] = new ComboBox();
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].GotFocus += Generic_GotFocus;
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Name = "cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime" + i.ToString() + "";
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 0.05 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 0.07 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 0.1 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 0.5 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 1 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 5 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 10 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 17 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 25 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 50 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 75 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 100 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 200 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 300 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 400 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 500 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 600 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 700 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 800 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 900 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 1000 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 1200 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 1500 ms");
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].Items.Add("Release time " + (i + 1).ToString() + ": 2000 ms");
            }

            // Slider for Comp Threshold:
            Slider[] slEditTone_superNATURALDrumKit_Compressor_CompThreshold = new Slider[6];
            for (byte i = 0; i < 6; i++)
            {
                tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[i]);
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[i] = new Slider();
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[i].GotFocus += Generic_GotFocus;
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[i].Name = "slEditTone_superNATURALDrumKit_Compressor_CompThreshold" + i.ToString();
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[i].Minimum = 0;
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[i].Maximum = 127;
            }

            // ComboBox for Comp Ratio
            ComboBox[] cbEditTone_superNATURALDrumKit_Compressor_CompRatio = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i] = new ComboBox();
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].GotFocus += Generic_GotFocus;
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Name = "cbEditTone_superNATURALDrumKit_Compressor_CompRatio" + i.ToString() + "";
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:1");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:2");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:3");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:4");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:5");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:6");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:7");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:8");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:9");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:10");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:20");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:30");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:40");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:50");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:60");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:70");
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].Items.Add("Comp ratio: 1:80");
            }

            // Slider for Comp Output Gain:
            Slider[] slEditTone_superNATURALDrumKit_Compressor_CompOutputGain = new Slider[6];
            for (byte i = 0; i < 6; i++)
            {
                tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[i]);
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[i] = new Slider();
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[i].GotFocus += Generic_GotFocus;
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[i].Name = "slEditTone_superNATURALDrumKit_Compressor_CompOutputGain" + i.ToString();
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[i].Minimum = 0;
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[i].Maximum = 24;
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[0],
                cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[1], cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[2],
                cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[3], cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[4],
                cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[5] })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[0],
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[1], cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[2], })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[3],
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[4], cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[5]})).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[0],
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[1], cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[2]})).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[3],
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[4], cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[5]})).Row);
            ControlsGrid.Children.Add((new GridRow((byte)(5), new Control[] { tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[0],
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[0], tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[1],
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[1], }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow((byte)(6), new Control[] { tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[2],
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[2], tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[3],
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[3], }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow((byte)(7), new Control[] { tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[4],
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[4], tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[5],
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[5], }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { cbEditTone_superNATURALDrumKit_Compressor_CompRatio[0],
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[1], cbEditTone_superNATURALDrumKit_Compressor_CompRatio[2]})).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { cbEditTone_superNATURALDrumKit_Compressor_CompRatio[3],
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[4], cbEditTone_superNATURALDrumKit_Compressor_CompRatio[5]})).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[0],
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[0], tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[1],
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[1] }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(11, new Control[] { tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[2],
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[2], tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[3],
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[3] }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(12, new Control[] { tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[4],
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[4], tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[5],
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[5] }, new byte[] { 1, 2, 1, 2 })).Row);





            // Set control values
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Compressor_CompSwitch[i].IsChecked = superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].CompSwitch;
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime[i].SelectedIndex = superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].CompAttackTime;
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Compressor_ReleaseTime[i].SelectedIndex = superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].CompReleaseTime;
            }
            for (byte i = 0; i < 6; i++)
            {
                slEditTone_superNATURALDrumKit_Compressor_CompThreshold[i].Value = (superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].CompThreshold);
                tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[i].Text = "Thresh.: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].CompThreshold)).ToString();
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Compressor_CompRatio[i].SelectedIndex = superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].CompRatio;
            }
            for (byte i = 0; i < 6; i++)
            {
                slEditTone_superNATURALDrumKit_Compressor_CompOutputGain[i].Value = (superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].CompOutputGain);
                tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[i].Text = "Outp. gain: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].CompOutputGain)).ToString();
            }
        }

        private void AddSupernaturalDrumKitEqualizerControls(byte SelectedIndex)
        {
            t.Trace("private void AddSupernaturalDrumKitEqualizerControls (" + "byte" + SelectedIndex + ", " + ")");
            controlsIndex = 0;
            String[] freqs;

            // CheckBox for EQ Switch
            CheckBox[] cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch = new CheckBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[i] = new CheckBox();
                cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[i].Tapped += GenericCheckBox_Click;
                cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[i].Click += GenericCheckBox_Click;
                cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[i].GotFocus += Generic_GotFocus;
                cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[i].Content = "EQ Switch" + (i + 1).ToString();
                cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[i].Name = "cbEditTone_superNATURALDrumKit_EQSwitch" + i.ToString();
            }

            // ComboBox for EQ Low Freq
            ComboBox[] cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[i] = new ComboBox();
                cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[i].GotFocus += Generic_GotFocus;
                cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[i].Name = "cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq" + i.ToString() + "";
                cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[i].Items.Add("Low" + (i + 1).ToString() + " f: 200");
                cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[i].Items.Add("Low" + (i + 1).ToString() + " f: 400");

            }

            // Slider for EQ Low Gain:
            Slider[] slEditTone_superNATURALDrumKit_Equalizer_EQLowGain = new Slider[6];
            for (byte i = 0; i < 6; i++)
            {
                tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[i]);
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[i] = new Slider();
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[i].GotFocus += Generic_GotFocus;
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[i].Name = "slEditTone_superNATURALDrumKit_Equalizer_EQLowGain" + i.ToString();
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[i].Minimum = -15;
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[i].Maximum = 15;
            }

            // ComboBox for EQ Mid Freq
            freqs = parameterSets.GetNumberedParameter(PARAMETER_TYPE.COMBOBOX_MID_FREQ);
            ComboBox[] cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[i] = new ComboBox();
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[i].GotFocus += Generic_GotFocus;
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[i].Name = "cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq" + i.ToString() + "";
                foreach (String freq in freqs)
                {
                    cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[i].Items.Add("Mid" + (i + 1).ToString() + " f: " + freq);
                }
            }

            // Slider for EQ Mid Gain:
            Slider[] slEditTone_superNATURALDrumKit_Equalizer_EQMidGain = new Slider[6];
            for (byte i = 0; i < 6; i++)
            {
                tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[i]);
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[i] = new Slider();
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[i].GotFocus += Generic_GotFocus;
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[i].Name = "slEditTone_superNATURALDrumKit_Equalizer_EQMidGain" + i.ToString();
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[i].Minimum = -15;
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[i].Maximum = 15;
            }

            // ComboBox for EQ Mid Q
            ComboBox[] cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[i] = new ComboBox();
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[i].GotFocus += Generic_GotFocus;
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[i].Name = "cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ" + i.ToString() + "";
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[i].Items.Add("Mid" + (i + 1).ToString() + " Q: 0.5");
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[i].Items.Add("Mid" + (i + 1).ToString() + " Q: 1.0");
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[i].Items.Add("Mid" + (i + 1).ToString() + " Q: 2.0");
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[i].Items.Add("Mid" + (i + 1).ToString() + " Q: 4.0");
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[i].Items.Add("Mid" + (i + 1).ToString() + " Q: 8.0");
            }

            // ComboBox for EQ High Freq
            ComboBox[] cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq = new ComboBox[6];
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[i] = new ComboBox();
                cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[i].GotFocus += Generic_GotFocus;
                cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[i].Name = "cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq" + i.ToString() + "";
                cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[i].Items.Add("Hi" + (i + 1).ToString() + " f: 2000");
                cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[i].Items.Add("Hi" + (i + 1).ToString() + " f: 4000");
                cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[i].Items.Add("Hi" + (i + 1).ToString() + " f: 8000");

            }

            // Slider for EQ High Gain:
            Slider[] slEditTone_superNATURALDrumKit_Equalizer_EQHighGain = new Slider[6];
            for (byte i = 0; i < 6; i++)
            {
                tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[i]);
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[i] = new Slider();
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[i].GotFocus += Generic_GotFocus;
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[i].Name = "slEditTone_superNATURALDrumKit_Equalizer_EQHighGain" + i.ToString();
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[i].Minimum = -15;
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[i].Maximum = 15;
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[0],
                cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[1], cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[2],
                cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[3], cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[4],
                cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[5] })).Row);
            ControlsGrid.Children.Add((new GridRow(1, new Control[] { cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[0],
                cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[1], cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[2],
                cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[3], cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[4],
                cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[5], })).Row);
            ControlsGrid.Children.Add((new GridRow(2, new Control[] { tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[0],
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[0], tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[1],
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[1] }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(3, new Control[] { tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[2],
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[2], tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[3],
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[3] }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(4, new Control[] { tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[4],
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[4], tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[5],
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[5] }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(5, new Control[] { cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[0],
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[1], cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[2],
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[3], cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[4],
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[5], })).Row);
            ControlsGrid.Children.Add((new GridRow(6, new Control[] { tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[0],
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[0], tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[1],
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[1], }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(7, new Control[] { tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[2],
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[2], tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[3],
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[3], }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(8, new Control[] { tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[4],
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[4], tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[5],
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[5], }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(9, new Control[] { cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[0],
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[1], cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[2],
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[3], cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[4],
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[5], })).Row);
            ControlsGrid.Children.Add((new GridRow(10, new Control[] { cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[0],
                cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[1], cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[2],
                cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[3], cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[4],
                cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[5] })).Row);
            ControlsGrid.Children.Add((new GridRow(11, new Control[] { tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[0],
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[0], tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[1],
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[1] }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(12, new Control[] { tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[2],
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[2], tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[3],
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[3] }, new byte[] { 1, 2, 1, 2 })).Row);
            ControlsGrid.Children.Add((new GridRow(13, new Control[] { tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[4],
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[4], tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[5],
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[5] }, new byte[] { 1, 2, 1, 2 })).Row);

            // Set control values
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Equalizer_EQSwitch[i].IsChecked = superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].EQSwitch;
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq[i].SelectedIndex = superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].EQLowFreq;
            }
            for (byte i = 0; i < 6; i++)
            {
                slEditTone_superNATURALDrumKit_Equalizer_EQLowGain[i].Value = (superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].EQLowGain - 15);
                tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[i].Text = "Low" + (i + 1).ToString() + " gain: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].EQLowGain - 15)).ToString();
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq[i].SelectedIndex = superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].EQMidFreq;
            }
            for (byte i = 0; i < 6; i++)
            {
                slEditTone_superNATURALDrumKit_Equalizer_EQMidGain[i].Value = (superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].EQMidGain - 15);
                tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[i].Text = "Mid" + (i + 1).ToString() + " gain: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].EQMidGain - 15)).ToString();
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ[i].SelectedIndex = superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].EQMidQ;
            }
            for (byte i = 0; i < 6; i++)
            {
                cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq[i].SelectedIndex = superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].EQHighFreq;
            }
            for (byte i = 0; i < 6; i++)
            {
                slEditTone_superNATURALDrumKit_Equalizer_EQHighGain[i].Value = (superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].EQHighGain - 15);
                tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[i].Text = "High" + (i + 1).ToString() + " gain: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[i].EQHighGain - 15)).ToString();
            }
        }

        private void AddSuperNaturalDrumKitMFXControlControls()
        {
            t.Trace("private void AddSuperNaturalDrumKitMFXControlControls()");
            controlsIndex = 0;
            // Create controls

            ComboBox[] cbEditTone_CommonMFX_MFXControl_MFXControlSource = new ComboBox[4];
            ComboBox[] cbEditTone_CommonMFX_MFXControl_MFXControlAssign = new ComboBox[4];
            Slider[] slEditTone_CommonMFX_MFXControl_MFXControlSens = new Slider[4];
            for (byte i = 0; i < 4; i++)
            {
                // ComboBox for MFX Control Source
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i] = new ComboBox();
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].GotFocus += Generic_GotFocus;
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Name = "cbEditTone_CommonMFX_MFXControl_MFXControlSource" + i.ToString();
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Off");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC01");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC02");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC03");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC04");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC05");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC06");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC07");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC08");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC09");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC10");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC11");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC12");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC13");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC14");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC15");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC16");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC17");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC18");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC19");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC20");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC21");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC22");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC23");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC24");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC25");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC26");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC27");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC28");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC29");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC30");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC31");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC32");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC33");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC34");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC35");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC36");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC37");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC38");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC39");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC40");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC41");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC42");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC43");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC44");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC45");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC46");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC47");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC48");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC49");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC50");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC51");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC52");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC53");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC54");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC55");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC56");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC57");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC58");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC59");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC60");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC61");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC62");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC63");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC64");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC65");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC66");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC67");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC68");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC69");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC70");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC71");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC72");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC73");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC74");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC75");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC76");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC77");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC78");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC79");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC80");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC81");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC82");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC83");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC84");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC85");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC86");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC87");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC88");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC89");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC90");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC91");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC92");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC93");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC94");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " CC95");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Pitch bend");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " After touch");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 1");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 2");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 3");
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].Items.Add("Control " + (i + 1).ToString() + " Sys 4");

                // ComboBox for MFX Control Destination
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i] = new ComboBox();
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].SelectionChanged += GenericCombobox_SelectionChanged;
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].GotFocus += Generic_GotFocus;
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].Tag = new HelpTag(controlsIndex++, 0);
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].Name = "cbEditTone_CommonMFX_MFXControl_MFXControlAssign" + i.ToString();
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].Items.Add("Destination : Off");
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].Items.Add("Destination : Low gain");
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].Items.Add("Destination : High gain");
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].Items.Add("Destination : Level");

                // Slider for MFX Control Sense:
                tbEditTone_CommonMFX_MFXControl_MFXControlSens[i] = new TextBox();
                SetLabelProperties(ref tbEditTone_CommonMFX_MFXControl_MFXControlSens[i]);
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i] = new Slider();
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].ValueChanged += GenericSlider_ValueChanged;
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].GotFocus += Generic_GotFocus;
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].Tag = new HelpTag(controlsIndex++, 0);
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].Name = "slEditTone_CommonMFX_MFXControl_MFXControlSense" + i.ToString();
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].Minimum = -63;
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].Maximum = 63;
            }

            // Put in rows
            for (byte i = 0; i < 4; i++)
            {
                ControlsGrid.Children.Add((new GridRow((byte)(0 + 3 * i), new Control[] { cbEditTone_CommonMFX_MFXControl_MFXControlSource[i] })).Row);
                ControlsGrid.Children.Add((new GridRow((byte)(1 + 3 * i), new Control[] { cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i] })).Row);
                ControlsGrid.Children.Add((new GridRow((byte)(2 + 3 * i), new Control[] { tbEditTone_CommonMFX_MFXControl_MFXControlSens[i], slEditTone_CommonMFX_MFXControl_MFXControlSens[i] }, new byte[] { 1, 2 })).Row);
            }

            // Set values
            //handleControlEvents = false;
            for (byte i = 0; i < 4; i++)
            {
                cbEditTone_CommonMFX_MFXControl_MFXControlSource[i].SelectedIndex = commonMFX.MFXControlSource[i];
                cbEditTone_CommonMFX_MFXControl_MFXControlAssign[i].SelectedIndex = commonMFX.MFXControlAssign[i];
                slEditTone_CommonMFX_MFXControl_MFXControlSens[i].Value = (commonMFX.MFXControlSens[i] - 64);
                tbEditTone_CommonMFX_MFXControl_MFXControlSens[i].Text = "MFX Control " + (byte)(i + 1) + " Sense: " + ((commonMFX.MFXControlSens[i] - 64)).ToString();
            }
        }

        // SuperNATURAL Drum Kit Save controls
        private void AddSuperNaturalDrumKitSaveControls()
        {
            t.Trace("private void AddSuperNaturalDrumKitSaveControls()");
            controlsIndex = 0;

            // Create controls
            SetLabelProperties(ref tbEditTone_SaveTone_Title);
            Button btnEditTone_SuperNaturalDrumKit_SaveTitle = new Button();
            btnEditTone_SuperNaturalDrumKit_SaveTitle.Content = "Save";
            btnEditTone_SuperNaturalDrumKit_SaveTitle.Click += btnEditTone_SaveTone_Click;
            Button btnEditTone_PCMSynthTone_DeleteTone = new Button();
            btnEditTone_PCMSynthTone_DeleteTone.Content = "Delete";
            btnEditTone_PCMSynthTone_DeleteTone.Click += btnEditTone_DeleteTone_Click;

            // Hook to help:
            tbEditTone_SaveTone_TitleText.GotFocus += Generic_GotFocus;
            tbEditTone_SaveTone_TitleText.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_SaveTone_SlotNumber.GotFocus += Generic_GotFocus;
            cbEditTone_SaveTone_SlotNumber.SelectionChanged += CbEditTone_Save_SlotNumber_SelectionChanged;
            cbEditTone_SaveTone_SlotNumber.Tag = new HelpTag(controlsIndex++, 0);
            btnEditTone_SuperNaturalDrumKit_SaveTitle.GotFocus += Generic_GotFocus;
            btnEditTone_SuperNaturalDrumKit_SaveTitle.Tag = new HelpTag(controlsIndex++, 0);

            String numString;
            cbEditTone_SaveTone_SlotNumber.Items.Clear();
            if (commonState.toneNames[4] != null && commonState.toneNames[4].Count() == 64)
            {
                for (UInt16 i = 0; i < 64; i++)
                {
                    numString = (i + 1).ToString();
                    while (numString.Length < 3) numString = "0" + numString;
                    cbEditTone_SaveTone_SlotNumber.Items.Add(numString + ": " + commonState.toneNames[4][i]);
                }
            }
            else
            {
                for (UInt16 i = 0; i < 64; i++)
                {
                    numString = (i + 1).ToString();
                    while (numString.Length < 3) numString = "0" + numString;
                    cbEditTone_SaveTone_SlotNumber.Items.Add(numString + ": INIT KIT");
                }
            }

            // Put in rows
            ControlsGrid.Children.Add((new GridRow((byte)(0), new Control[] { tbEditTone_SaveTone_Title,
                tbEditTone_SaveTone_TitleText, cbEditTone_SaveTone_SlotNumber, btnEditTone_SuperNaturalDrumKit_SaveTitle,
                btnEditTone_PCMSynthTone_DeleteTone}, new byte[] { 4, 3, 3, 2, 2 })).Row);

            // Set values
            tbEditTone_SaveTone_Title.Text = "Name (max 12 chars):";
            tbEditTone_SaveTone_TitleText.Text = superNATURALDrumKit.superNATURALDrumKitCommon.Name;
            SetSaveSlotToFirstFreeOrSameName();
        }

        private void SetSaveSlotToFirstFreeOrSameName()
        {
            currentHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            cbEditTone_SaveTone_SlotNumber.SelectedIndex = 0;
            while (((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5) != "INIT TONE"
                && ((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5) != "INIT KIT"
                && ((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5).Trim() != tbEditTone_SaveTone_TitleText.Text.Trim()
                && cbEditTone_SaveTone_SlotNumber.SelectedIndex < cbEditTone_SaveTone_SlotNumber.Items.Count() - 1)
            {
                cbEditTone_SaveTone_SlotNumber.SelectedIndex++;
            }
            if (cbEditTone_SaveTone_SlotNumber.SelectedIndex == cbEditTone_SaveTone_SlotNumber.Items.Count() - 1
                && ((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5) != "INIT TONE"
                && ((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5) != "INIT KIT"
                && ((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5).Trim() != tbEditTone_SaveTone_TitleText.Text.Trim())
            {
                // No free slots, reset to first slot:
                cbEditTone_SaveTone_SlotNumber.SelectedIndex = 0;
            }
            // When _typing_ a name, duplicates are not allowed, but here we have selected intentionally:
            else if (((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5).Trim() == tbEditTone_SaveTone_TitleText.Text.Trim())
            {
                btnEditTone_SaveTone.IsEnabled = true;
            }
            handleControlEvents = currentHandleControlEvents;
        }

        #endregion

        #region MFX Controls

        private void AddMFXControls()
        {
            t.Trace("private void AddMFXControls()");
            controlsIndex = 0;
            // MFX type:
            ComboBox cbEditTone_MFXType = new ComboBox();
            cbEditTone_MFXType.SelectionChanged += GenericCombobox_SelectionChanged;
            cbEditTone_MFXType.GotFocus += cbEditTone_MFXType_GotFocus;
            cbEditTone_MFXType.Tag = new HelpTag(controlsIndex++, 0);
            cbEditTone_MFXType.Name = "cbEditTone_MFXType";

            for (byte i = 0; i < numberedParametersContent.TypeNames.Length; i++)
            {
                cbEditTone_MFXType.Items.Add(numberedParametersContent.TypeNames[i]);
            }

            // Numbered parameters:
            dynamicTextboxes = new List<TextBox>();
            dynamicSliders = new List<Slider>();
            dynamicComboboxes = new List<ComboBox>();
            dynamicCheckboxes = new List<CheckBox>();
            byte itemIndex = 0;
            for (byte i = 0; i < numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset].Length; i++)
            {
                try
                {
                    HelpTag tag;
                    switch (numberedParametersContent.ParameterTypes[commonMFX.MFXType + currentMFXTypeOffset][i])
                    {
                        case PARAMETER_TYPE.SLIDER_0_TO_127:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString());
                            AddDynamicSlider(0, 127, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_TO_100:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString());
                            AddDynamicSlider(0, 100, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_TO_100_HZ:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString() + " Hz");
                            AddDynamicSlider(0, 100, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_TO_100_MS:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString() + " ms");
                            AddDynamicSlider(0, 100, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_TO_10:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString());
                            AddDynamicSlider(0, 10, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 64).ToString());
                            AddDynamicSlider(-64, 63, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 64,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 64, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_TO_180_STEP_2:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString());
                            AddDynamicSlider(0, 180, 2, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, .5));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_TO_12:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString());
                            AddDynamicSlider(0, 12, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_TO_1300_MS:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString() + " ms");
                            AddDynamicSlider(0, 1300, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_TO_15:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString());
                            AddDynamicSlider(0, 15, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_TO_18_DB:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString() + " dB");
                            AddDynamicSlider(0, 18, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_TO_20:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString());
                            AddDynamicSlider(0, 20, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_TO_2600_MS:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString());
                            AddDynamicSlider(0, 2600, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_MINUS_10_TO_10:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 10).ToString());
                            AddDynamicSlider(-10, 10, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 10,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 10, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_MINUS_15_TO_15:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 15).ToString());
                            AddDynamicSlider(-15, 15, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 15,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 15, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_MINUS_20_TO_20:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 20).ToString());
                            AddDynamicSlider(-20, 20, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 20,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 20, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_MINUS_24_TO_24:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 24).ToString());
                            AddDynamicSlider(-24, 24, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 24,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 24, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_MINUS_50_TO_50:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 50).ToString());
                            AddDynamicSlider(-50, 50, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 50,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 50, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_MINUS_63_TO_63:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 63).ToString());
                            AddDynamicSlider(-63, 63, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 63,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 63, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_MINUS_64_TO_64:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 64).ToString());
                            AddDynamicSlider(-64, 64, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 64,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 64, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 98).ToString());
                            AddDynamicSlider(-98, 98, 2, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 98,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 98, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 0.5));
                            break;
                        case PARAMETER_TYPE.SLIDER_MINUS_100_TO_100_STEP_2:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 100).ToString());
                            AddDynamicSlider(-100, 100, 2, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 100,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 100, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 0.5));
                            break;
                        case PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 100).ToString());
                            AddDynamicSlider(-100, 100, 2, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value - 100,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 100, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 0.5));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString());
                            AddDynamicSlider(.05, 10, .05, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 20));
                            break;
                        case PARAMETER_TYPE.SLIDER_0_10_TO_20_00_STEP_0_10:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString());
                            AddDynamicSlider(.1, 20, .1, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 10));
                            break;
                        case PARAMETER_TYPE.COMBOBOX_0_TO_100_STEP_0_1_TO_2:
                        case PARAMETER_TYPE.COMBOBOX_AMPLIFIER_GAIN:
                        case PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_3:
                        case PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_4:
                        case PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_14:
                        case PARAMETER_TYPE.COMBOBOX_BEND_AFT_SYS1_TO_SYS4:
                        case PARAMETER_TYPE.COMBOBOX_DRIVE_TYPE:
                        case PARAMETER_TYPE.COMBOBOX_FILTER_SLOPE:
                        case PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_OFF_2:
                        case PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_4:
                        case PARAMETER_TYPE.COMBOBOX_GATE_MODE:
                        case PARAMETER_TYPE.COMBOBOX_HF_DAMP:
                        case PARAMETER_TYPE.COMBOBOX_HIGH_FREQ:
                        case PARAMETER_TYPE.COMBOBOX_LOW_BOOST_FREQUENCY:
                        case PARAMETER_TYPE.COMBOBOX_LOW_BOOST_WIDTH:
                        case PARAMETER_TYPE.COMBOBOX_LOW_FREQ:
                        case PARAMETER_TYPE.COMBOBOX_MID_FREQ:
                        case PARAMETER_TYPE.COMBOBOX_NORMAL_CROSS:
                        case PARAMETER_TYPE.COMBOBOX_NORMAL_INVERSE:
                        case PARAMETER_TYPE.COMBOBOX_POSTFILTER_TYPE:
                        case PARAMETER_TYPE.COMBOBOX_PREFILTER_TYPE:
                        case PARAMETER_TYPE.COMBOBOX_LOFI_TYPE:
                        case PARAMETER_TYPE.COMBOBOX_Q:
                        case PARAMETER_TYPE.COMBOBOX_TONE_NAMES:
                        case PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_2:
                        case PARAMETER_TYPE.COMBOBOX_POLARITY:
                        case PARAMETER_TYPE.COMBOBOX_VOWELS:
                        case PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES:
                        case PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES_5:
                        case PARAMETER_TYPE.COMBOBOX_MICROPHONE_DISTANCE:
                        case PARAMETER_TYPE.COMBOBOX_PHASER_MODE_3:
                        case PARAMETER_TYPE.COMBOBOX_PHASER_MODE_4:
                        case PARAMETER_TYPE.COMBOBOX_PHASER_MODE_6:
                        case PARAMETER_TYPE.COMBOBOX_PHASER_POLARITY:
                        case PARAMETER_TYPE.COMBOBOX_PHASER_COLOR:
                        case PARAMETER_TYPE.COMBOBOX_RATIO:
                        case PARAMETER_TYPE.COMBOBOX_WAVE_SHAPE:
                        case PARAMETER_TYPE.COMBOBOX_LEGATO_SLASH:
                        case PARAMETER_TYPE.COMBOBOX_ROTARY_SPEED:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i]);
                            AddDynamicComboBox(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Text,
                                commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            break;
                        case PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i]);
                            AddDynamicComboBox(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Text,
                                commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset),
                                (byte)(i), 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            if (i < numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset].Length - 2)
                            {
                                i++;
                                tag = new HelpTag(itemIndex++, 0);
                                AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                    ((Double)(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value) / 20.0).ToString() + " Hz");
                                dynamicTextboxes[dynamicTextboxes.Count() - 1].Name = "tbHz" + i.ToString();
                                switch (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Type)
                                {
                                    case PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05:
                                        AddDynamicSlider(0.05, 10.00, 0.05, (Double)(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value) / 20.0,
                                            new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset),
                                            (byte)(i), 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 20));
                                        break;
                                    case PARAMETER_TYPE.SLIDER_0_10_TO_20_00_STEP_0_10:
                                        AddDynamicSlider(0.1, 20.00, 0.1, (Double)(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value) / 10.0,
                                            new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset),
                                            (byte)(i), 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 10));
                                        break;
                                    case PARAMETER_TYPE.SLIDER_0_TO_100_HZ:
                                        AddDynamicSlider(0, 100, 1, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                            new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset),
                                            (byte)(i), 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 1));
                                        break;
                                }
                                dynamicSliders[dynamicSliders.Count() - 1].Name = "slHz" + i.ToString();
                                i++;
                                tag = new HelpTag(itemIndex++, 0);
                                AddDynamicTextbox("Note length: ");
                                dynamicTextboxes[dynamicTextboxes.Count() - 1].Name = "tbNote" + i.ToString();
                                AddDynamicComboBox(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Text,
                                    commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                    new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset),
                                    (byte)(i), 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                                dynamicComboboxes[dynamicComboboxes.Count() - 1].Name = "cbNote" + i.ToString();
                            }
                            break;
                        case PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i]);
                            AddDynamicComboBox(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Text,
                                commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset),
                                (byte)(i), 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                            if (i < numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset].Length - 2)
                            {
                                i++;
                                tag = new HelpTag(itemIndex++, 0);
                                AddDynamicTextbox(numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i] + ": " +
                                    ((Double)(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value) / 20.0).ToString() + " ms");
                                dynamicTextboxes[dynamicTextboxes.Count() - 1].Name = "tbHz" + i.ToString();
                                switch (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Type)
                                {
                                    case PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05:
                                        AddDynamicSlider(0.05, 10.00, 0.05, (Double)(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value) / 20.0,
                                            new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset),
                                            (byte)(i), 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 20));
                                        break;
                                    case PARAMETER_TYPE.SLIDER_0_10_TO_20_00_STEP_0_10:
                                        AddDynamicSlider(0.1, 20.00, 0.1, (Double)(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value) / 10.0,
                                            new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset),
                                            (byte)(i), 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 10));
                                        break;
                                    case PARAMETER_TYPE.SLIDER_0_TO_1300_MS:
                                        AddDynamicSlider(0, 1300, 1, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                            new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset),
                                            (byte)(i), 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 1));
                                        break;
                                    case PARAMETER_TYPE.SLIDER_0_TO_100_MS:
                                        AddDynamicSlider(0, 1300, 1, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                            new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset),
                                            (byte)(i), 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 1));
                                        break;
                                    case PARAMETER_TYPE.SLIDER_0_TO_2600_MS:
                                        AddDynamicSlider(0, 2600, 1, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                            new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset),
                                            (byte)(i), 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag, 1));
                                        break;
                                }
                                dynamicSliders[dynamicSliders.Count() - 1].Name = "slHz" + i.ToString();
                                i++;
                                tag = new HelpTag(itemIndex++, 0);
                                AddDynamicTextbox("Note length: ");
                                dynamicTextboxes[dynamicTextboxes.Count() - 1].Name = "tbNote" + i.ToString();
                                AddDynamicComboBox(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Text,
                                    commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                    new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset),
                                    (byte)(i), 0, dynamicTextboxes[dynamicTextboxes.Count - 1], tag));
                                dynamicComboboxes[dynamicComboboxes.Count() - 1].Name = "cbNote" + i.ToString();
                            }
                            break;
                        case PARAMETER_TYPE.CHECKBOX:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicCheckBox(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i],
                                new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, (CheckBox)null, tag));
                            break;
                        case PARAMETER_TYPE.CHECKBOX_1:
                            tag = new HelpTag(itemIndex++, 0);
                            AddDynamicCheckBox(commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                null, new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, (CheckBox)null, tag));
                            if (i < numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset].Length - 1)
                            {
                                // Add the following control:
                                switch (numberedParametersContent.ParameterTypes[commonMFX.MFXType + currentMFXTypeOffset][i + 1])
                                {
                                    case PARAMETER_TYPE.CHECKBOX:
                                    case PARAMETER_TYPE.CHECKBOX_1:
                                    case PARAMETER_TYPE.CHECKBOX_2:
                                    case PARAMETER_TYPE.CHECKBOX_3:
                                    case PARAMETER_TYPE.CHECKBOX_4:
                                        break;
                                    case PARAMETER_TYPE.SLIDER_0_TO_127_R:
                                        tag = new HelpTag(itemIndex++, 0);
                                        i++;
                                        dynamicCheckboxes[dynamicCheckboxes.Count - 1].Content =
                                            numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset][i - 1] + ": " +
                                            (commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value).ToString();
                                        AddDynamicSlider(0, 127, commonMFX.MFXNumberedParameters.Parameters.Parameters[i + currentMFXTypePageParameterOffset].Value.Value,
                                            new Buddy((byte)(commonMFX.MFXNumberedParameters.Parameters.Offset), i, 0, dynamicCheckboxes[dynamicCheckboxes.Count - 1], tag));
                                        break;
                                }
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    String message = "Error in AddMFXControls() while creating controls: " + e.Message;
                    if (e.InnerException != null && e.InnerException.Message != null)
                    {
                        message += " InnerException: " + e.InnerException.Message;
                    }
                    t.Trace(message);
                }
            }

            // Put MFX type in row 0:
            ControlsGrid.Children.Add((new GridRow(0, new Control[] { cbEditTone_MFXType })).Row);

            // Put the rest of the controls in row 1 and on:
            byte sliderIndex = 0;
            byte textboxIndex = 0;
            byte comboboxIndex = 0;
            byte checkboxIndex = 0;
            byte rowOffset = 0;
            try
            {
                // Note! MFX type is in row 0, so all parameters has to be put in to row number parameterNumber + 1 BUT:
                // Controls like Hz/Note takes one row for the Hz/Note selector and one more row where two sets of
                // controls reside: one set for the Hz label and slider, and one set for the Note label and combobox.
                // Therefore, after such a set, the row numbers must be offset to: parameterNumber + 1 - rowOffset.
                for (byte parameterNumber = 0; parameterNumber < numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset].Length; parameterNumber++)
                {
                    switch (numberedParametersContent.ParameterTypes[commonMFX.MFXType + currentMFXTypeOffset][parameterNumber])
                    {
                        case PARAMETER_TYPE.SLIDER_0_TO_127:
                        case PARAMETER_TYPE.SLIDER_0_TO_100:
                        case PARAMETER_TYPE.SLIDER_0_TO_10:
                        case PARAMETER_TYPE.SLIDER_0_TO_1300_MS:
                        case PARAMETER_TYPE.SLIDER_0_TO_100_HZ:
                        case PARAMETER_TYPE.SLIDER_0_TO_100_MS:
                        case PARAMETER_TYPE.SLIDER_0_TO_180_STEP_2:
                        case PARAMETER_TYPE.SLIDER_0_TO_12:
                        case PARAMETER_TYPE.SLIDER_0_TO_15:
                        case PARAMETER_TYPE.SLIDER_0_TO_18_DB:
                        case PARAMETER_TYPE.SLIDER_0_TO_20:
                        case PARAMETER_TYPE.SLIDER_0_TO_2600_MS:
                        case PARAMETER_TYPE.SLIDER_MINUS_10_TO_10:
                        case PARAMETER_TYPE.SLIDER_MINUS_15_TO_15:
                        case PARAMETER_TYPE.SLIDER_MINUS_20_TO_20:
                        case PARAMETER_TYPE.SLIDER_MINUS_24_TO_24:
                        case PARAMETER_TYPE.SLIDER_MINUS_50_TO_50:
                        case PARAMETER_TYPE.SLIDER_MINUS_63_TO_63:
                        case PARAMETER_TYPE.SLIDER_MINUS_64_TO_64:
                        case PARAMETER_TYPE.SLIDER_MINUS_L64_TO_R63:
                        case PARAMETER_TYPE.SLIDER_MINUS_98_TO_98_STEP_2:
                        case PARAMETER_TYPE.SLIDER_MINUS_100_TO_100_STEP_2:
                        case PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2:
                        case PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05:
                        case PARAMETER_TYPE.SLIDER_0_10_TO_20_00_STEP_0_10:
                            ControlsGrid.Children.Add((new GridRow((byte)(parameterNumber + 1 - rowOffset), new Control[] { dynamicTextboxes[textboxIndex++],
                            dynamicSliders[sliderIndex++] }, new byte[] { 1, 2 })).Row);
                            break;
                        case PARAMETER_TYPE.COMBOBOX_AMPLIFIER_GAIN:
                        case PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_3:
                        case PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_4:
                        case PARAMETER_TYPE.COMBOBOX_AMPLIFIER_TYPE_14:
                        case PARAMETER_TYPE.COMBOBOX_BEND_AFT_SYS1_TO_SYS4:
                        case PARAMETER_TYPE.COMBOBOX_DRIVE_TYPE:
                        case PARAMETER_TYPE.COMBOBOX_FILTER_SLOPE:
                        case PARAMETER_TYPE.COMBOBOX_0_TO_100_STEP_0_1_TO_2:
                        case PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_OFF_2:
                        case PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_4:
                        case PARAMETER_TYPE.COMBOBOX_GATE_MODE:
                        case PARAMETER_TYPE.COMBOBOX_HF_DAMP:
                        case PARAMETER_TYPE.COMBOBOX_HIGH_FREQ:
                        case PARAMETER_TYPE.COMBOBOX_LOW_BOOST_FREQUENCY:
                        case PARAMETER_TYPE.COMBOBOX_LOW_BOOST_WIDTH:
                        case PARAMETER_TYPE.COMBOBOX_LOW_FREQ:
                        case PARAMETER_TYPE.COMBOBOX_MID_FREQ:
                        case PARAMETER_TYPE.COMBOBOX_NORMAL_CROSS:
                        case PARAMETER_TYPE.COMBOBOX_NORMAL_INVERSE:
                        case PARAMETER_TYPE.COMBOBOX_POSTFILTER_TYPE:
                        case PARAMETER_TYPE.COMBOBOX_PREFILTER_TYPE:
                        case PARAMETER_TYPE.COMBOBOX_LOFI_TYPE:
                        case PARAMETER_TYPE.COMBOBOX_Q:
                        case PARAMETER_TYPE.COMBOBOX_TONE_NAMES:
                        case PARAMETER_TYPE.COMBOBOX_FILTER_TYPE_2:
                        case PARAMETER_TYPE.COMBOBOX_POLARITY:
                        case PARAMETER_TYPE.COMBOBOX_VOWELS:
                        case PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES:
                        case PARAMETER_TYPE.COMBOBOX_SPEAKER_TYPES_5:
                        case PARAMETER_TYPE.COMBOBOX_MICROPHONE_DISTANCE:
                        case PARAMETER_TYPE.COMBOBOX_PHASER_MODE_3:
                        case PARAMETER_TYPE.COMBOBOX_PHASER_MODE_4:
                        case PARAMETER_TYPE.COMBOBOX_PHASER_MODE_6:
                        case PARAMETER_TYPE.COMBOBOX_PHASER_POLARITY:
                        case PARAMETER_TYPE.COMBOBOX_PHASER_COLOR:
                        case PARAMETER_TYPE.COMBOBOX_WAVE_SHAPE:
                        case PARAMETER_TYPE.COMBOBOX_LEGATO_SLASH:
                        case PARAMETER_TYPE.COMBOBOX_RATIO:
                        case PARAMETER_TYPE.COMBOBOX_ROTARY_SPEED:
                            ControlsGrid.Children.Add((new GridRow((byte)(parameterNumber + 1 - rowOffset), new Control[] { dynamicTextboxes[textboxIndex++],
                                dynamicComboboxes[comboboxIndex++] }, new byte[] { 1, 2 })).Row);
                            break;
                        case PARAMETER_TYPE.COMBOBOX_HZ_AND_NOTE_LENGTHS:
                            ControlsGrid.Children.Add((new GridRow((byte)(parameterNumber + 1 - rowOffset), new Control[] { dynamicTextboxes[textboxIndex++],
                            dynamicComboboxes[comboboxIndex] }, new byte[] { 1, 2 })).Row);
                            dynamicComboboxes[comboboxIndex++].Name = (parameterNumber + 2).ToString() + " Hz/Note"; // Something to identify for special functionality (swapping controls on line below).
                            parameterNumber++;
                            ControlsGrid.Children.Add((new GridRow((byte)(parameterNumber + 1 - rowOffset), new Control[] { dynamicTextboxes[textboxIndex++],
                            dynamicSliders[sliderIndex++] }, new byte[] { 1, 2 })).Row);
                            ControlsGrid.Children.Add((new GridRow((byte)(parameterNumber + 1 - rowOffset), new Control[] { dynamicTextboxes[textboxIndex++],
                            dynamicComboboxes[comboboxIndex++]}, new byte[] { 1, 2 })).Row);
                            //parameterNumber++;
                            try
                            {
                                if ((String)((ComboBox)((Grid)((Grid)(ControlsGrid.Children[parameterNumber])).Children[1]).Children[0]).SelectedItem == "Hz")
                                {
                                    ((TextBox)((Grid)((Grid)(ControlsGrid.Children[parameterNumber + 2])).Children[0]).Children[0]).Visibility = Visibility.Collapsed;
                                    ((ComboBox)((Grid)((Grid)(ControlsGrid.Children[parameterNumber + 2])).Children[1]).Children[0]).Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    ((TextBox)((Grid)((Grid)(ControlsGrid.Children[parameterNumber + 1])).Children[0]).Children[0]).Visibility = Visibility.Collapsed;
                                    ((Slider)((Grid)((Grid)(ControlsGrid.Children[parameterNumber + 1])).Children[1]).Children[0]).Visibility = Visibility.Collapsed;
                                }
                            }
                            catch (Exception e)
                            {
                                String message = "Error in AddMFXControls() while hiding one of two on same row: " + e.Message;
                                if (e.InnerException != null && e.InnerException.Message != null)
                                {
                                    message += " InnerException: " + e.InnerException.Message;
                                }
                                t.Trace(message);
                            }
                            rowOffset++;
                            break;
                        case PARAMETER_TYPE.COMBOBOX_MS_AND_NOTE_LENGTHS:
                            ControlsGrid.Children.Add((new GridRow((byte)(parameterNumber + 1 - rowOffset), new Control[] { dynamicTextboxes[textboxIndex++],
                            dynamicComboboxes[comboboxIndex] }, new byte[] { 1, 2 })).Row);
                            dynamicComboboxes[comboboxIndex++].Name = (parameterNumber + 2).ToString() + " Ms/Note"; // Something to identify for special functionality (swapping controls on line below).
                            parameterNumber++;
                            ControlsGrid.Children.Add((new GridRow((byte)(parameterNumber + 1 - rowOffset), new Control[] { dynamicTextboxes[textboxIndex++],
                            dynamicSliders[sliderIndex++] }, new byte[] { 1, 2 })).Row);
                            ControlsGrid.Children.Add((new GridRow((byte)(parameterNumber + 1 - rowOffset), new Control[] { dynamicTextboxes[textboxIndex++],
                            dynamicComboboxes[comboboxIndex++]}, new byte[] { 1, 2 })).Row);
                            //parameterNumber++;
                            try
                            {
                                if ((String)((ComboBox)((Grid)((Grid)(ControlsGrid.Children[parameterNumber])).Children[1]).Children[0]).SelectedItem == "Hz")
                                {
                                    ((TextBox)((Grid)((Grid)(ControlsGrid.Children[parameterNumber + 2])).Children[0]).Children[0]).Visibility = Visibility.Collapsed;
                                    ((ComboBox)((Grid)((Grid)(ControlsGrid.Children[parameterNumber + 2])).Children[1]).Children[0]).Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    ((TextBox)((Grid)((Grid)(ControlsGrid.Children[parameterNumber + 1])).Children[0]).Children[0]).Visibility = Visibility.Collapsed;
                                    ((Slider)((Grid)((Grid)(ControlsGrid.Children[parameterNumber + 1])).Children[1]).Children[0]).Visibility = Visibility.Collapsed;
                                }
                            }
                            catch (Exception e)
                            {
                                String message = "Error in AddMFXControls() while hiding one of two on same row: " + e.Message;
                                if (e.InnerException != null && e.InnerException.Message != null)
                                {
                                    message += " InnerException: " + e.InnerException.Message;
                                }
                                t.Trace(message);
                            }
                            rowOffset++;
                            break;
                        case PARAMETER_TYPE.CHECKBOX:
                            ControlsGrid.Children.Add((new GridRow((byte)(parameterNumber + 1 - rowOffset), new Control[] { dynamicCheckboxes[checkboxIndex++] }, new byte[] { 1 })).Row);
                            break;
                        case PARAMETER_TYPE.CHECKBOX_1:
                        case PARAMETER_TYPE.CHECKBOX_2:
                        case PARAMETER_TYPE.CHECKBOX_3:
                        case PARAMETER_TYPE.CHECKBOX_4:
                            if (parameterNumber < numberedParametersContent.ParameterNames[commonMFX.MFXType + currentMFXTypeOffset].Length - 1)
                            {
                                switch (numberedParametersContent.ParameterTypes[commonMFX.MFXType + currentMFXTypeOffset][parameterNumber + 1])
                                {
                                    case PARAMETER_TYPE.SLIDER_0_TO_127_R:
                                        ControlsGrid.Children.Add((new GridRow((byte)(parameterNumber + 1 - rowOffset), new Control[] { dynamicCheckboxes[checkboxIndex++],
                                    dynamicSliders[sliderIndex++]}, new byte[] { 1, 2 })).Row);
                                        rowOffset++;
                                        parameterNumber++;
                                        break;
                                }
                            }
                            else
                            {
                                ControlsGrid.Children.Add((new GridRow((byte)(parameterNumber + 1 - rowOffset), new Control[] { dynamicCheckboxes[checkboxIndex++] }, new byte[] { 1 })).Row);
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                String message = "Error in AddMFXControls() while adding controls to grid: " + e.Message;
                if (e.InnerException != null && e.InnerException.Message != null)
                {
                    message += " InnerException: " + e.InnerException.Message;
                }
                t.Trace(message);
            }

            // Set control values (here it is only the Type selector):
            cbEditTone_MFXType.SelectedIndex = commonMFX.MFXType + currentMFXTypeOffset;
        }

        #endregion
        #region helpers

        private void AddDynamicTextbox(String text)
        {
            t.Trace("private void AddDynamicTextbox (" + "String" + text + ", " + ")");
            dynamicTextboxes.Add(new TextBox());
            dynamicTextboxes[dynamicTextboxes.Count() - 1].TextAlignment = TextAlignment.Right;
            dynamicTextboxes[dynamicTextboxes.Count() - 1].BorderThickness = new Thickness(0);
            dynamicTextboxes[dynamicTextboxes.Count() - 1].IsEnabled = false;
            dynamicTextboxes[dynamicTextboxes.Count() - 1].Text = text;
        }

        private void AddDynamicSlider(Int32 Minimum, Int32 Maximum, Int32 Value, Buddy Tag = null)
        {
            t.Trace("private void AddDynamicSlider (" + "Int32" + Minimum + ", " + "Int32" + Maximum + ", " + "Int32" + Value + ", " + "Buddy" + Tag + ", " + ")");
            handleControlEvents = false;
            dynamicSliders.Add(new Slider());
            dynamicSliders[dynamicSliders.Count() - 1].Minimum = Minimum;
            dynamicSliders[dynamicSliders.Count() - 1].Maximum = Maximum;
            dynamicSliders[dynamicSliders.Count() - 1].Value = Value;
            dynamicSliders[dynamicSliders.Count() - 1].ValueChanged += GenericSlider_ValueChanged;
            dynamicSliders[dynamicSliders.Count() - 1].GotFocus += Generic_GotFocus;
            dynamicSliders[dynamicSliders.Count() - 1].Tag = Tag;
        }

        private void AddDynamicSlider(Double Minimum, Double Maximum, Double Step, Double Value, Buddy Tag = null)
        {
            t.Trace("private void AddDynamicHzSlider (" + "Double" + Minimum + ", " + "Double" + Maximum + ", " + "Double" + Step + ", " + "Double" + Value + ", " + "Buddy" + Tag + ", " + ")");
            handleControlEvents = false;
            dynamicSliders.Add(new Slider());
            dynamicSliders[dynamicSliders.Count() - 1].Minimum = Minimum;
            dynamicSliders[dynamicSliders.Count() - 1].Maximum = Maximum;
            dynamicSliders[dynamicSliders.Count() - 1].StepFrequency = Step;
            dynamicSliders[dynamicSliders.Count() - 1].Value = Value;
            dynamicSliders[dynamicSliders.Count() - 1].ValueChanged += GenericSlider_ValueChanged;
            dynamicSliders[dynamicSliders.Count() - 1].GotFocus += Generic_GotFocus;
            dynamicSliders[dynamicSliders.Count() - 1].Tag = Tag;
        }

        private void AddDynamicComboBox(String[] Items, Int32 SelectedIndex, Buddy Tag = null)
        {
            t.Trace("private void AddDynamicComboBox (" + "String[]" + Items + ", " + "Int32" + SelectedIndex + ", " + "Buddy" + Tag + ", " + ")");
            handleControlEvents = false;
            dynamicComboboxes.Add(new ComboBox());
            if (Items != null)
            {
                for (byte i = 0; i < Items.Length; i++)
                {
                    dynamicComboboxes[dynamicComboboxes.Count - 1].Items.Add(Items[i]);
                }
                if (SelectedIndex < Items.Count())
                {
                    dynamicComboboxes[dynamicComboboxes.Count - 1].SelectedIndex = SelectedIndex;
                }
                else if (Items.Count() > 0)
                {
                    dynamicComboboxes[dynamicComboboxes.Count - 1].SelectedIndex = 0;
                }
            }
            else
            {
                //dynamicComboboxes[dynamicComboboxes.Count - 1].SelectedIndex = 0;
            }
            dynamicComboboxes[dynamicComboboxes.Count - 1].SelectionChanged += GenericCombobox_SelectionChanged;
            dynamicComboboxes[dynamicComboboxes.Count - 1].GotFocus += Generic_GotFocus;
            dynamicComboboxes[dynamicComboboxes.Count - 1].Tag = Tag;
        }

        private void AddDynamicCheckBox(UInt16 Checked, String Text, Buddy Tag = null)
        {
            t.Trace("private void AddDynamicCheckBox(Boolean Checked = " + Checked.ToString() + ", Buddy Tag = " + Tag + ")");
            dynamicCheckboxes.Add(new CheckBox());
            dynamicCheckboxes[dynamicCheckboxes.Count - 1].IsChecked = Checked > 0;
            dynamicCheckboxes[dynamicCheckboxes.Count - 1].Tapped += GenericCheckBox_Click;
            dynamicCheckboxes[dynamicCheckboxes.Count - 1].Click += GenericCheckBox_Click;
            dynamicCheckboxes[dynamicCheckboxes.Count - 1].GotFocus += Generic_GotFocus;
            dynamicCheckboxes[dynamicCheckboxes.Count() - 1].HorizontalContentAlignment = HorizontalAlignment.Left;
            dynamicCheckboxes[dynamicCheckboxes.Count() - 1].BorderThickness = new Thickness(0);
            dynamicCheckboxes[dynamicCheckboxes.Count() - 1].Content = Text;
            dynamicCheckboxes[dynamicCheckboxes.Count() - 1].Tag = Tag;
        }

        private void RemoveControls(Grid grid)
        {
            t.Trace("private void RemoveControls (" + "Grid" + grid + ", " + ")");
            if (grid != null && grid.Children != null && grid.Children.Count > 0)
            {
                try
                {
                    while (grid.Children.Count() > 0)
                    {
                        Type type = grid.Children[0].GetType();
                        if (type == typeof(Grid))
                        {
                            RemoveControls((Grid)grid.Children[0]);
                        }
                        else if (type == typeof(ComboBox))
                        {
                            ((ComboBox)grid.Children[0]).SelectionChanged -= CbEditTone_Save_SlotNumber_SelectionChanged;
                            ((ComboBox)grid.Children[0]).SelectionChanged -= GenericCombobox_SelectionChanged;
                            ((ComboBox)grid.Children[0]).GotFocus -= Generic_GotFocus;
                        }
                        else if (type == typeof(Slider))
                        {
                            ((Slider)grid.Children[0]).ValueChanged -= GenericSlider_ValueChanged;
                            ((Slider)grid.Children[0]).GotFocus -= Generic_GotFocus;
                        }
                        else if (type == typeof(CheckBox))
                        {
                            ((CheckBox)grid.Children[0]).Click -= GenericCheckBox_Click;
                            ((CheckBox)grid.Children[0]).Tapped -= GenericCheckBox_Click;
                            ((CheckBox)grid.Children[0]).GotFocus -= Generic_GotFocus;
                        }
                        else if (type == typeof(Button))
                        {
                            ((Button)grid.Children[0]).Click -= GenericCheckBox_Click;
                            ((Button)grid.Children[0]).Tapped -= GenericCheckBox_Click;
                            ((Button)grid.Children[0]).GotFocus -= Generic_GotFocus;
                        }
                        else if (type == typeof(TextBox))
                        {
                            ((TextBox)grid.Children[0]).KeyUp -= TbEditTone_Save_TitleText_KeyUp;
                            ((TextBox)grid.Children[0]).GotFocus -= Generic_GotFocus;
                        }
                        grid.Children.RemoveAt(0);
                    }
                    while (grid.ColumnDefinitions.Count() > 0)
                    {
                        grid.ColumnDefinitions.RemoveAt(0);
                    }
                }
                catch
                {
                }
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion
    }
}