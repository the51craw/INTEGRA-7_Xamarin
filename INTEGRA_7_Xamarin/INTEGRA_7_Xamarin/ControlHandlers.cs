using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integra_7_Xamarin
{
    public sealed partial class UIHandler
    {
        private async void cbEditTone_SynthesizerType_SelectionChanged(object sender, EventArgs e)
        {
            //t.Trace("private void cbEditTone_SynthesizerType_SelectionChanged (" + "object" + sender + ", " + "SelectionChangedEventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                if (cbEditTone_SynthesizerType.SelectedIndex != currentProgramIndex)
                {
                    // Changing program type means that we have incorrect data read into the controls, and that some classes are not filled out.
                    // Doing this will mean that we will have to fetch default data for all classes used by the new program type.
                    //MessageDialog warning = new MessageDialog("Changing Tone Type results in all settings to be reset. Are you sure this is what you want to do?");
                    //warning.Title = "Warning!";
                    //warning.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                    //warning.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
                    String response = await mainPage.DisplayActionSheet("Changing Tone Type results in all settings to be reset. Are you sure this is what you want to do?", null, null, new String[] { "No", "Yes" });
                    //var response = await warning.ShowAsync();
                    //if ((Int32)response.Id == 0)
                    if (response == "Yes")
                    {
                        Reset();
                    }
                    else
                    {
                        cbEditTone_SynthesizerType.SelectedIndex = currentProgramIndex;
                    }
                }
            }
        }

        private void cbEditTone_PartSelector_SelectionChanged(object sender, EventArgs e)
        {
            //t.Trace("private void cbEditTone_PartSelector_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                Waiting(true);
                commonState.CurrentPart = (byte)cbEditTone_PartSelector.SelectedIndex;
                initDone = false;
                handleControlEvents = false;
                currentParameterPageIndex = 0;
                currentPartial = 0;
                QueryToneType();
            }
        }

        // When changing partial, partial needs to be re-read
        private void cbEditTone_PartialSelector_SelectionChanged(object sender, EventArgs e)
        {
            //t.Trace("private void cbEditTone_PartialSelector_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");

            if (initDone && handleControlEvents)
            {
                Waiting(true);
                switch (currentProgramType)
                {
                    case ProgramType.PCM_SYNTH_TONE:
                        handleControlEvents = false;
                        currentPartial = (byte)cbEditTone_PartialSelector.SelectedIndex;
                        //QueryPCMSynthTonePartial(); No need, all 4 are now read in initially, just update controls:
                        EditTone_UpdateControls();
                        handleControlEvents = true;
                        break;
                    case ProgramType.PCM_DRUM_KIT:
                        handleControlEvents = false;
                        currentPartial = (byte)cbEditTone_PartialSelector.SelectedIndex;
                        //QueryPCMDrumKitPartial();
                        UpdatePCMDrumKitControls();
                        handleControlEvents = true;
                        break;
                    case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                        handleControlEvents = false;
                        currentPartial = (byte)cbEditTone_PartialSelector.SelectedIndex;
                        UpdateSuperNATURALAcousticToneControls();
                        handleControlEvents = true;
                        break;
                    case ProgramType.SUPERNATURAL_SYNTH_TONE:
                        handleControlEvents = false;
                        currentPartial = (byte)cbEditTone_PartialSelector.SelectedIndex;
                        //QuerySuperNATURALSynthTonePartial();
                        UpdateSuperNATURALSynthToneControls();
                        handleControlEvents = true;
                        break;
                    case ProgramType.SUPERNATURAL_DRUM_KIT:
                        handleControlEvents = false;
                        currentPartial = (byte)cbEditTone_PartialSelector.SelectedIndex;
                        //QuerySuperNATURALDrumKit();
                        UpdateSuperNATURALDrumKitControls();
                        handleControlEvents = true;
                        break;
                }
            }
            //handleControlEvents = true;
        }

        //private void tbEditTone_KeyName_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (initDone && cbEditTone_KeySelector.SelectedIndex > -1)
        //    {
        //        commonState.keyNames[cbEditTone_KeySelector.SelectedIndex] = tbEditTone_KeyName.Text;
        //    }
        //}

        // When changin key for a drum kit, we actually change partial, partial needs to be re-read
        // NO! All keys are now initially read in!
        private void cbEditTone_KeySelector_SelectionChanged(object sender, EventArgs e)
        {
            //t.Trace("private void cbEditTone_KeySelector_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                Waiting(true);
                switch (currentProgramType)
                {
                    case ProgramType.PCM_DRUM_KIT:
                        handleControlEvents = false;
                        currentKey = (byte)(cbEditTone_KeySelector.SelectedIndex);
                        //QueryPCMDrumKitPartial();
                        UpdatePCMDrumKitControls();
                        handleControlEvents = true;
                        break;
                    case ProgramType.SUPERNATURAL_DRUM_KIT:
                        handleControlEvents = false;
                        currentKey = (byte)(cbEditTone_KeySelector.SelectedIndex);
                        //QuerySuperNATURALDrumKitPartial();
                        UpdateSuperNATURALDrumKitControls();
                        handleControlEvents = true;
                        break;
                }
            }
        }

        private void cbEditTone_ParameterPages_SelectionChanged(object sender, EventArgs e)
        {
            //t.Trace("private void cbEditTone_ParameterPages_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                Waiting(true);
                currentHandleControlEvents = handleControlEvents;
                handleControlEvents = false;
                currentParameterPageIndex = (byte)cbEditTone_ParameterPages.SelectedIndex;
                currentParameterPage = (String)cbEditTone_ParameterPages.SelectedItem;
                //tbEditTone_KeyName.Visibility = Visibility.Collapsed;
                cbEditTone_PartialSelector.Visibility = Visibility.Visible; // We want that as default.
                switch (currentProgramType)
                {
                    case ProgramType.PCM_SYNTH_TONE:
/*                        if (currentParameterPage == "Save file")
                        {
                            PCMData pcmData = new PCMData();
                            pcmData.ToneData = pCMSynthTone;
                            pcmData.MFX = commonMFX;
                            await PCMData.Serialize(pcmData);
                            Waiting(false);
                        }
                        else if (currentParameterPage == "Load file")
                        {
                            PCMData pcmData = new PCMData();
                            pcmData.ToneData = pCMSynthTone;
                            pcmData = await PCMData.Deserialize(pcmData);
                            try
                            {
                                pCMSynthTone = pcmData.ToneData;
                                commonMFX = pcmData.MFX;
                                UpdateIntegra7FromPCM();
                                currentParameterPageIndex = 0;
                                UpdatePCMSynthToneControls();
                            }
                            catch { }
                            Waiting(false);
                        }
                        else*/ if (currentParameterPageIndex == 15)
                        {
                            SetMFXOffsetValues(commonMFX.MFXType);
                            if (commonMFX.MFXType == mfxPageReadFromIntegra7)
                            {
                                useMFXParameterDataFromIntegra_7 = true;
                            }
                            else
                            {
                                useMFXParameterDataFromIntegra_7 = false;
                            }
                            QueryPCMSynthToneCommonMFX();
                        }
                        else
                        {
                            MakePCMSynthToneControls(currentParameterPageIndex);
                        }
                        break;
                    case ProgramType.PCM_DRUM_KIT:
                        /*if (currentParameterPage == "Save file")
                        {
                            PCMDData pcmdData = new PCMDData();
                            pcmdData.ToneData = pCMDrumKit;
                            pcmdData.MFX = commonMFX;
                            await PCMDData.Serialize(pcmdData);
                            Waiting(false);
                        }
                        else if (currentParameterPage == "Load file")
                        {
                            PCMDData pcmdData = new PCMDData();
                            pcmdData.ToneData = pCMDrumKit;
                            pcmdData = await PCMDData.Deserialize(pcmdData);
                            try
                            {
                                pCMDrumKit = pcmdData.ToneData;
                                commonMFX = pcmdData.MFX;
                                UpdateIntegra7FromPCMD();
                                currentParameterPageIndex = 0;
                                UpdatePCMDrumKitControls();
                            }
                            catch { }
                            Waiting(false);
                        }
                        else*/ if (currentParameterPageIndex == 12)
                        {
                            SetMFXOffsetValues(commonMFX.MFXType);
                            if (commonMFX.MFXType == mfxPageReadFromIntegra7)
                            {
                                useMFXParameterDataFromIntegra_7 = true;
                            }
                            else
                            {
                                useMFXParameterDataFromIntegra_7 = false;
                            }
                            QueryPCMDrumKitCommonMFX();
                        }
                        else
                        {
                            if (currentParameterPageIndex == 0)
                            {
                                //cbEditTone_PartialSelector.Visibility = Visibility.Collapsed;
                                //tbEditTone_KeyName.Visibility = Visibility.Visible;
                            }
                            UpdatePCMDrumKitControls();
                        }
                        break;
                    case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                        /*if (currentParameterPage == "Save file")
                        {
                            SNAData snaData = new SNAData();
                            snaData.ToneData = superNATURALAcousticTone;
                            snaData.MFX = commonMFX;
                            await SNAData.Serialize(snaData);
                            Waiting(false);
                        }
                        else if (currentParameterPage == "Load file")
                        {
                            SNAData snaData = new SNAData();
                            snaData.ToneData = superNATURALAcousticTone;
                            snaData = await SNAData.Deserialize(snaData);
                            try
                            {
                                superNATURALAcousticTone = snaData.ToneData;
                                commonMFX = snaData.MFX;
                                UpdateIntegra7FromSNA();
                                currentParameterPageIndex = 0;
                            }
                            catch { }
                            UpdateSuperNATURALAcousticToneControls();
                            Waiting(false);
                        }
                        else*/ if (currentParameterPageIndex == 2)
                        {
                            SetMFXOffsetValues(commonMFX.MFXType);
                            if (commonMFX.MFXType == mfxPageReadFromIntegra7)
                            {
                                useMFXParameterDataFromIntegra_7 = true;
                            }
                            else
                            {
                                useMFXParameterDataFromIntegra_7 = false;
                            }
                            QuerySuperNATURALAcousticToneCommonMFX();
                        }
                        else
                        {
                            UpdateSuperNATURALAcousticToneControls();
                        }
                        break;
                    case ProgramType.SUPERNATURAL_SYNTH_TONE:
                        /*if (currentParameterPage == "Save file")
                        {
                            SNSData snsData = new SNSData();
                            snsData.ToneData = superNATURALSynthTone;
                            snsData.MFX = commonMFX;
                            await SNSData.Serialize(snsData);
                            Waiting(false);
                        }
                        else if (currentParameterPage == "Load file")
                        {
                            SNSData snsData = new SNSData();
                            snsData.ToneData = superNATURALSynthTone;
                            snsData = await SNSData.Deserialize(snsData);
                            try
                            {
                                superNATURALSynthTone = snsData.ToneData;
                                commonMFX = snsData.MFX;
                                UpdateIntegra7FromSNS();
                                currentParameterPageIndex = 0;
                                UpdateSuperNATURALSynthToneControls();
                            } catch { }
                            Waiting(false);
                        }
                        else*/ if (currentParameterPageIndex == 9)
                        {
                            SetMFXOffsetValues(commonMFX.MFXType);
                            if (commonMFX.MFXType == mfxPageReadFromIntegra7)
                            {
                                useMFXParameterDataFromIntegra_7 = true;
                            }
                            else
                            {
                                useMFXParameterDataFromIntegra_7 = false;
                            }
                            QuerySuperNATURALSynthToneCommonMFX();
                        }
                        else
                        {
                            UpdateSuperNATURALSynthToneControls();
                        }
                        break;
                    case ProgramType.SUPERNATURAL_DRUM_KIT:
                        /*if (currentParameterPage == "Save file")
                        {
                            SNDData sndData = new SNDData();
                            sndData.ToneData = superNATURALDrumKit;
                            sndData.MFX = commonMFX;
                            await SNDData.Serialize(sndData);
                            Waiting(false);
                        }
                        else if (currentParameterPage == "Load file")
                        {
                            SNDData sndData = new SNDData();
                            sndData.ToneData = superNATURALDrumKit;
                            sndData = await SNDData.Deserialize(sndData);
                            try
                            {
                                superNATURALDrumKit = sndData.ToneData;
                                handleControlEvents = false;
                                commonMFX = sndData.MFX;
                                UpdateIntegra7FromSND();
                                currentParameterPageIndex = 0;
                                UpdateSuperNATURALDrumKitControls();
                            }
                            catch { }
                            Waiting(false);
                        }
                        else*/ if (currentParameterPageIndex == 4)
                        {
                            SetMFXOffsetValues(commonMFX.MFXType);
                            if (commonMFX.MFXType == mfxPageReadFromIntegra7)
                            {
                                useMFXParameterDataFromIntegra_7 = true;
                            }
                            else
                            {
                                useMFXParameterDataFromIntegra_7 = false;
                            }
                            QuerySuperNATURALDrumKitCommonMFX();
                        }
                        else
                        {
                            if (currentParameterPageIndex == 0)
                            {
                                //cbEditTone_PartialSelector.Visibility = Visibility.Collapsed;
                                //tbEditTone_KeyName.IsVisible = true;
                            }
                            UpdateSuperNATURALDrumKitControls();
                        }
                        break;
                }
                handleControlEvents = currentHandleControlEvents;
            }
        }

        private void cbEditTone_InstrumentCategorySelector_SelectionChanged(object sender, EventArgs e)
        {
            //t.Trace("private void cbEditTone_InstrumentCategorySelector_SelectionChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone)
            {
                switch (currentProgramType)
                {
                    case ProgramType.PCM_SYNTH_TONE:
                        pCMSynthTone.pCMSynthToneCommon2.ToneCategory = toneCategories.pcmToneCategoryIndex[(byte)cbEditTone_InstrumentCategorySelector.SelectedIndex];
                        commonState.currentTone.Category = toneCategories.pcmToneCategoryNames[toneCategories.pcmToneCategoryIndex[(byte)cbEditTone_InstrumentCategorySelector.SelectedIndex]];
                        byte[] address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON2, new byte[] { 0x10 });
                        byte[] value = new byte[] { (byte)(pCMSynthTone.pCMSynthToneCommon2.ToneCategory) };
                        SendParameter(address, value);
                        break;
                    case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                        superNATURALAcousticTone.superNATURALAcousticToneCommon.Category = toneCategories.snaToneCategoryIndex[(byte)cbEditTone_InstrumentCategorySelector.SelectedIndex];
                        commonState.currentTone.Category = toneCategories.snaToneCategoryNames[toneCategories.snaToneCategoryIndex[(byte)cbEditTone_InstrumentCategorySelector.SelectedIndex]];
                        address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x1b });
                        value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.Category) };
                        SendParameter(address, value);
                        break;
                    case ProgramType.SUPERNATURAL_SYNTH_TONE:
                        superNATURALSynthTone.superNATURALSynthToneCommon.Category = toneCategories.snsToneCategoryIndex[(byte)cbEditTone_InstrumentCategorySelector.SelectedIndex];
                        commonState.currentTone.Category = toneCategories.snsToneCategoryNames[toneCategories.snsToneCategoryIndex[(byte)cbEditTone_InstrumentCategorySelector.SelectedIndex]];
                        address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x36 });
                        value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthToneCommon.Category) };
                        SendParameter(address, value);
                        break;
                }
                // Todo:
                //MessageDialog question = new MessageDialog("Do you also wish to reset edits to defaults for this type?");
                //question.Title = "Question";
                //question.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
                //question.Commands.Add(new UICommand { Label = "No", Id = 1 });
                //var response = await question.ShowAsync();
                //if ((Int32)response.Id == 0)
                //{
                //}
            }
        }

        // When typing a name, duplicates are not allowed:
        private void TbEditTone_Save_TitleText_KeyUp(object sender, EventArgs e)
        {
            Boolean found = false;
            foreach (String name in cbEditTone_SaveTone_SlotNumber.Items)
            {
                if (name.Remove(0, 5).Trim() == tbEditTone_SaveTone_TitleText.Text.Trim())
                {
                    found = true;
                    break;
                }
            }
            btnEditTone_SaveTone.IsEnabled = !found;
            // But if we type the same as edited, allow save:
            if (((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5).Trim() == tbEditTone_SaveTone_TitleText.Text.Trim())
            {
                btnEditTone_SaveTone.IsEnabled = true;
            }
        }

        // When selecting a slot, same title as edited is allowed as well as INIT... of course:
        private void CbEditTone_Save_SlotNumber_SelectionChanged(object sender, EventArgs e)
        {
            if (handleControlEvents&& cbEditTone_SaveTone_SlotNumber.SelectedItem != null)
            {
                btnEditTone_SaveTone.IsEnabled = false;
                // Enable save button if slot is free or if the name is the same as typed:
                if (((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5) == "INIT TONE"
                    || ((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5) == "INIT KIT"
                    || ((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5).Trim() == tbEditTone_SaveTone_TitleText.Text.Trim())
                {
                    btnEditTone_SaveTone.IsEnabled = true;
                }
                // If the user selects a slot that is not free, update the name in title text:
                if (((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5) != "INIT TONE"
                    && ((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5) != "INIT KIT")
                {
                    tbEditTone_SaveTone_TitleText.Text = ((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5).Trim();
                }
            }
        }

        private async void btnEditTone_SaveTone_Click(object sender, EventArgs e)
        {
            Boolean save = true;
            byte[] address;
            byte[] value;
            byte[] data;
            // Are we trying tooverwrite an existing name?
            if (((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5) != "INIT TONE"
                && ((String)cbEditTone_SaveTone_SlotNumber.SelectedItem).Remove(0, 5) != "INIT KIT")
            {
                //MessageDialog warning = new MessageDialog("Do you wish to overwrite the selected tone with your edits?");
                //warning.Title = "Warning!";
                //warning.Commands.Add(new UICommand { Label = "Yes, overwrite selected sound.", Id = 0 });
                //warning.Commands.Add(new UICommand { Label = "Eh, overwrite? Nope! Don\'t touch it!", Id = 1 });
                //var response = await warning.ShowAsync();
                String response = await mainPage.DisplayActionSheet("Do you wish to overwrite the selected tone with your edits?",
                    null, null, new String[] { "Yes, overwrite selected sound.", "Eh, overwrite? Nope! Don\'t touch it!" });
                initDone = true;
                //if ((Int32)response.Id == 1)
                if (response == "Eh, overwrite? Nope! Don\'t touch it!")
                {
                    save = false;
                }
            }
            else
            {
                // If we add a new name, ensure we do not create a duplicate:
                for (Int32 i = 0; i < commonState.toneList.Tones.Count(); i++)
                {
                    if (commonState.toneList.Tones[i][3] == tbEditTone_SaveTone_TitleText.Text)
                    {
                        //MessageDialog warning = new MessageDialog("Sorry, that name already exists. Try another name.");
                        //warning.Title = "Warning!";
                        //warning.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                        //warning.Commands.Add(new UICommand { Label = "Eh, overwrite? Nope! Don\'t touch it!", Id = 1 });
                        //var response = await warning.ShowAsync();
                        String response = await mainPage.DisplayActionSheet("Sorry, that name already exists. Try another name.",
                            null, null, new String[] { "Ok" });
                        initDone = true;
                        save = false;
                    }
                }
            }
            if (save)
            {
                Waiting(true);
                if (tbEditTone_SaveTone_TitleText.Text.Length > 12)
                {
                    tbEditTone_SaveTone_TitleText.Text = tbEditTone_SaveTone_TitleText.Text.Remove(12);
                }
                while (tbEditTone_SaveTone_TitleText.Text.Length < 12)
                {
                    tbEditTone_SaveTone_TitleText.Text = tbEditTone_SaveTone_TitleText.Text += " ";
                }
                switch (currentProgramType)
                {
                    // Saving name is straight forward, but saving tone has to be explained.
                    // Regarding address. Address is always 0f 00 10 00
                    // Regarding value. There are four bytes. MIDI-Spy gave us this:
                    // First depends on tone type. It is the msb for the user tone of current set.
                    // Second and third makes up the slot number from lsb and pc.
                    // Fourth is the command: 0 = save, 3 = copy from slot 0.
                    case ProgramType.PCM_SYNTH_TONE:
                        // Update I-7 with the new name:
                        String name = tbEditTone_SaveTone_TitleText.Text;
                        address = new byte[] { (byte)(0x19 + ((0x20 * commonState.CurrentPart) / 0x80)),
                            (byte)((0x20 * commonState.CurrentPart) % 0x80), 0x00, 0x00 };
                        value = Encoding.UTF8.GetBytes(name);
                        data = commonState.midi.SystemExclusiveDT1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
  
                        // Store tone in slot:
                        address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                        value = new byte[] { 0x57, 0x00,
                            (byte)cbEditTone_SaveTone_SlotNumber.SelectedIndex,
                            commonState.CurrentPart};
                        data = commonState.midi.SystemExclusiveRQ1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);

                        // Also update toneList and toneNames.
                        List<String> tone = new List<String>();
                        tone.Add("PCM Synth Tone");
                        tone.Add(cbEditTone_InstrumentCategorySelector.SelectedItem.ToString());
                        tone.Add(cbEditTone_SaveTone_SlotNumber.SelectedIndex.ToString());
                        tone.Add(name.Trim());
                        tone.Add("87");
                        tone.Add((cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128).ToString());
                        tone.Add((87 * 128 + (cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128)).ToString());
                        tone.Add(((cbEditTone_SaveTone_SlotNumber.SelectedIndex + 1) % 128).ToString());
                        tone.Add("(User)");
                        if (commonState.toneNames[0][cbEditTone_SaveTone_SlotNumber.SelectedIndex] == "INIT TONE")
                        {
                            // This will be a new one in toneList:
                            tone.Add(commonState.toneList.Tones.Count().ToString());
                            tone.Add("-1");
                            commonState.toneList.Tones.Add(tone);
                        }
                        else
                        {
                            Int32 i = commonState.toneList.PresetsCount; // Start looking for it after the last tone from soundlist pdf
                            Boolean found = false;
                            while (!found && i < commonState.toneList.Tones.Count())
                            {
                                if (commonState.toneList.Tones[i][0] == "PCM Synth Tone"
                                    //&& commonState.toneList.Tones[i][1] == cbEditTone_InstrumentCategorySelector.SelectedItem.ToString()
                                    && commonState.toneList.Tones[i][3].Trim() == name.Trim())
                                {
                                    found = true;
                                }
                                else
                                {
                                    i++;
                                }
                            }
                            if (found)
                            {
                                tone.Add(i.ToString());
                                tone.Add("-1");
                                commonState.toneList.Tones[i] = tone;
                            }
                            else
                            {
                                // This should never happen!
                                tone.Add(commonState.toneList.Tones.Count().ToString());
                                tone.Add("-1");
                                commonState.toneList.Tones.Add(tone);
                                //t.Trace("Save PCM Synth Tone did not find expected tone! Tone was added instead.");
                            }
                        }
                        
                        // Also update in toneNames:
                        commonState.toneNames[0][cbEditTone_SaveTone_SlotNumber.SelectedIndex] = name.Trim();
                        
                        // Update 
                        commonState.currentTone = new Tone(tone);
 
                        // Finally, re-draw the tab to update controls:
                        UpdatePCMSynthToneControls();
                        break;
                    case ProgramType.PCM_DRUM_KIT:
                        // All the key names (the name of the instrument associated with each key) are in commonState.keyNames.
                        // We need to create a new record, or update the existing one, in the DrumKeyAssignList.
                        // We also need to add them to all partials so they will ba stored in the INTEGRA-7 as well.
                        byte keyIndex = 0;
                        // Look for a record in DrumKeyAssignList, starting after the last preset:
                        Int32 listIndex = commonState.PresetDrumKeyAssignListsCount;
                        while (listIndex < commonState.drumKeyAssignLists.ToneNames.Count())
                        {
                            if (commonState.drumKeyAssignLists.ToneNames[listIndex][1] == tbEditTone_SaveTone_TitleText.Text.Trim())
                            {
                                break;
                            }
                            listIndex++;
                        }
                        if (listIndex >= commonState.drumKeyAssignLists.ToneNames.Count())
                        {
                            // This is a new list. Add it:
                            commonState.drumKeyAssignLists.ToneNames.Add(new List<String>());
                            listIndex = commonState.drumKeyAssignLists.ToneNames.Count() - 1;
                            // Fill out the header strings
                            commonState.drumKeyAssignLists.ToneNames[listIndex].Add("PCM Drum Kit");
                            commonState.drumKeyAssignLists.ToneNames[listIndex].Add(tbEditTone_SaveTone_TitleText.Text.Trim());
                            // Fill out the names:
                            for (keyIndex = 0; keyIndex < commonState.keyNames.Count(); keyIndex++)
                            {
                                if (String.IsNullOrEmpty(commonState.keyNames[keyIndex]))
                                {
                                    commonState.drumKeyAssignLists.ToneNames[listIndex].Add("");
                                }
                                else
                                {
                                    commonState.drumKeyAssignLists.ToneNames[listIndex].Add(commonState.keyNames[keyIndex]);
                                }
                            }
                        }
                        else
                        {
                            // Fill out the names (first two lines are already filled out):
                            for (keyIndex = 0; keyIndex < commonState.keyNames.Count(); keyIndex++)
                            {
                                commonState.drumKeyAssignLists.ToneNames[listIndex][keyIndex + 2] = commonState.keyNames[keyIndex];
                            }
                        }
                        for (keyIndex = 2; keyIndex < commonState.drumKeyAssignLists.ToneNames[listIndex].Count(); keyIndex++)
                        {
                            // Update partial in INTEGRA-7:
                            byte partOffset = (byte)((keyIndex - 2) * 2);
                            name = commonState.drumKeyAssignLists.ToneNames[listIndex][keyIndex];
                            while (name.Length < 12)
                            {
                                name += " ";
                            }
                            address = new byte[] { 0x19, (byte)(0x10 + (0x10 + partOffset) / 128), (byte)((0x10 + partOffset) % 128), 0x00 };
                            value = Encoding.UTF8.GetBytes(name);
                            data = commonState.midi.SystemExclusiveDT1Message(address, value);
                            commonState.midi.SendSystemExclusive(data);
                        }
                        // The rest is handled like for e.g. PCM Synth Tone:
                        name = tbEditTone_SaveTone_TitleText.Text;
                        address = new byte[] { (byte)(0x19 + ((0x20 * commonState.CurrentPart) / 0x80)),
                            (byte)(0x10 + ((0x20 * commonState.CurrentPart) % 0x80)), 0x00, 0x00 };
                        value = Encoding.UTF8.GetBytes(name);
                        data = commonState.midi.SystemExclusiveDT1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);

                        // Store tone in slot:
                        address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                        value = new byte[] { 0x56, 0x00,
                            (byte)cbEditTone_SaveTone_SlotNumber.SelectedIndex,
                            commonState.CurrentPart};
                        data = commonState.midi.SystemExclusiveRQ1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);

                        // Also update toneList and toneNames:
                        tone = new List<String>();
                        tone.Add("PCM Drum Kit");
                        tone.Add("Drum");
                        tone.Add(cbEditTone_SaveTone_SlotNumber.SelectedIndex.ToString());
                        tone.Add(name.Trim());
                        tone.Add("86");
                        tone.Add((cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128).ToString());
                        tone.Add((86 * 128 + (cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128)).ToString());
                        tone.Add(((cbEditTone_SaveTone_SlotNumber.SelectedIndex + 1) % 128).ToString());
                        tone.Add("(User)");
                        if (commonState.toneNames[1] != null && commonState.toneNames[1][cbEditTone_SaveTone_SlotNumber.SelectedIndex] == "INIT KIT")
                        {
                            // This will be a new one in toneList:
                            tone.Add(commonState.toneList.Tones.Count().ToString());
                            tone.Add("-1");
                            commonState.toneList.Tones.Add(tone);
                        }
                        else
                        {
                            Int32 i = commonState.toneList.PresetsCount; // Start looking for it after the last tone from soundlist pdf
                            Boolean found = false;
                            while (!found && i < commonState.toneList.Tones.Count())
                            {
                                if (commonState.toneList.Tones[i][0] == "PCM Drum Kit"
                                    && commonState.toneList.Tones[i][1] == "Drum"
                                    && commonState.toneList.Tones[i][3].Trim() == name.Trim())
                                {
                                    found = true;
                                }
                                else
                                {
                                    i++;
                                }
                            }
                            if (found)
                            {
                                tone.Add(i.ToString());
                                tone.Add("-1");
                                commonState.toneList.Tones[i] = tone;
                            }
                            else
                            {
                                // This should never happen!
                                tone.Add(commonState.toneList.Tones.Count().ToString());
                                tone.Add("-1");
                                commonState.toneList.Tones.Add(tone);
                                //t.Trace("Save PCM Drum Kit did not find expected tone! Tone was added instead.");
                            }
                        }
                        
                        // Also update in toneNames:
                        commonState.toneNames[1][cbEditTone_SaveTone_SlotNumber.SelectedIndex] = name.Trim();

                        // Finally, re-draw the tab to update controls:
                        UpdatePCMDrumKitControls();
                        break;
                    case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                        // Store tone name:
                        name = tbEditTone_SaveTone_TitleText.Text;
                        address = new byte[] { (byte)(0x19 + ((0x20 * commonState.CurrentPart) / 0x80)),
                            (byte)(0x02 + ((0x20 * commonState.CurrentPart) % 0x80)), 0x00, 0x00 };
                        value = Encoding.UTF8.GetBytes(name);
                        data = commonState.midi.SystemExclusiveDT1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);

                        // Store tone in slot:
                        address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                        value = new byte[] { 0x59, 0x00,
                            (byte)cbEditTone_SaveTone_SlotNumber.SelectedIndex,
                            commonState.CurrentPart};
                        data = commonState.midi.SystemExclusiveRQ1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
                        // Also update toneList and toneNames:
                        tone = new List<String>();
                        tone.Add("SuperNATURAL Acoustic Tone");
                        tone.Add(cbEditTone_InstrumentCategorySelector.SelectedItem.ToString());
                        tone.Add(cbEditTone_SaveTone_SlotNumber.SelectedIndex.ToString());
                        tone.Add(name.Trim());
                        tone.Add("89");
                        tone.Add((cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128).ToString());
                        tone.Add((89 * 128 + (cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128)).ToString());
                        tone.Add(((cbEditTone_SaveTone_SlotNumber.SelectedIndex + 1) % 128).ToString());
                        tone.Add("(User)");
                        if (commonState.toneNames[2][cbEditTone_SaveTone_SlotNumber.SelectedIndex] == "INIT TONE")
                        {
                            // This will be a new one in toneList:
                            tone.Add(commonState.toneList.Tones.Count().ToString());
                            tone.Add(cbEditTone_superNATURALDrumKit_Druminstrument_Variation.SelectedIndex.ToString());
                            tone.Add("-1");
                            commonState.toneList.Tones.Add(tone);
                        }
                        else
                        {
                            Int32 i = commonState.toneList.PresetsCount; // Start looking for it after the last tone from soundlist pdf
                            Boolean found = false;
                            while (!found && i < commonState.toneList.Tones.Count())
                            {
                                if (commonState.toneList.Tones[i][0] == "SuperNATURAL Acoustic Tone"
                                    && commonState.toneList.Tones[i][1] == cbEditTone_InstrumentCategorySelector.SelectedItem.ToString()
                                    && commonState.toneList.Tones[i][3].Trim() == name.Trim())
                                {
                                    found = true;
                                }
                                else
                                {
                                    i++;
                                }
                            }
                            if (found)
                            {
                                tone.Add(i.ToString());
                                tone.Add(cbEditTone_superNATURALDrumKit_Druminstrument_Variation.SelectedIndex.ToString());
                                tone.Add("-1");
                                commonState.toneList.Tones[i] = tone;
                            }
                            else
                            {
                                // This should never happen!
                                tone.Add(commonState.toneList.Tones.Count().ToString());
                                tone.Add(cbEditTone_superNATURALDrumKit_Druminstrument_Variation.SelectedIndex.ToString());
                                tone.Add("-1");
                                commonState.toneList.Tones.Add(tone);
                                //t.Trace("Save SuperNATURAL Acoustic Tone did not find expected tone! Tone was added instead.");
                            }
                        }
                        
                        // Also update in toneNames:
                        commonState.toneNames[2][cbEditTone_SaveTone_SlotNumber.SelectedIndex] = name.Trim();

                        // Finally, re-draw the tab to update controls:
                        UpdateSuperNATURALAcousticToneControls();
                        break;
                    case ProgramType.SUPERNATURAL_SYNTH_TONE:
                        // Store the name:
                        name = tbEditTone_SaveTone_TitleText.Text;
                        address = new byte[] { (byte)(0x19 + ((0x20 * commonState.CurrentPart) / 0x80)),
                            (byte)(0x01 + ((0x20 * commonState.CurrentPart) % 0x80)), 0x00, 0x00 };
                        value = Encoding.UTF8.GetBytes(name);
                        data = commonState.midi.SystemExclusiveDT1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);

                        // Store tone in slot:
                        address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                        value = new byte[] { 0x5f, 0x00,
                            (byte)cbEditTone_SaveTone_SlotNumber.SelectedIndex,
                            commonState.CurrentPart};
                        data = commonState.midi.SystemExclusiveRQ1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);

                        // Also update toneList and toneNames:
                        tone = new List<String>();
                        tone.Add("SuperNATURAL Synth Tone");
                        tone.Add(cbEditTone_InstrumentCategorySelector.SelectedItem.ToString());
                        tone.Add(cbEditTone_SaveTone_SlotNumber.SelectedIndex.ToString());
                        tone.Add(name.Trim());
                        tone.Add("95");
                        tone.Add((cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128).ToString());
                        tone.Add((95 * 128 + (cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128)).ToString());
                        tone.Add(((cbEditTone_SaveTone_SlotNumber.SelectedIndex + 1) % 128).ToString());
                        tone.Add("(User)");
                        if (commonState.toneNames[3][cbEditTone_SaveTone_SlotNumber.SelectedIndex] == "INIT TONE")
                        {
                            // This will be a new one in toneList:
                            tone.Add(commonState.toneList.Tones.Count().ToString());
                            tone.Add("-1");
                            commonState.toneList.Tones.Add(tone);
                        }
                        else
                        {
                            Int32 i = commonState.toneList.PresetsCount; // Start looking for it after the last tone from soundlist pdf
                            Boolean found = false;
                            while (!found && i < commonState.toneList.Tones.Count())
                            {
                                if (commonState.toneList.Tones[i][0] == "SuperNATURAL Synth Tone"
                                    && commonState.toneList.Tones[i][1] == cbEditTone_InstrumentCategorySelector.SelectedItem.ToString()
                                    && commonState.toneList.Tones[i][3].Trim() == name.Trim())
                                {
                                    found = true;
                                }
                                else
                                {
                                    i++;
                                }
                            }
                            if (found)
                            {
                                tone.Add(i.ToString());
                                tone.Add("-1");
                                commonState.toneList.Tones[i] = tone;
                            }
                            else
                            {
                                // This should never happen!
                                tone.Add(commonState.toneList.Tones.Count().ToString());
                                tone.Add("-1");
                                commonState.toneList.Tones.Add(tone);
                                //t.Trace("Save SuperNATURAL Synth Tone did not find expected tone! Tone was added instead.");
                            }
                        }
                        // Also update in toneNames:
                        commonState.toneNames[3][cbEditTone_SaveTone_SlotNumber.SelectedIndex] = name.Trim();
                        // Finally, re-draw the tab to update controls:
                        UpdateSuperNATURALSynthToneControls();
                        break;
                    case ProgramType.SUPERNATURAL_DRUM_KIT:
                        // All the key names (the name of the instrument associated with each key) are in commonState.keyNames.
                        // We need to create a new record, or update the existing one, in the DrumKeyAssignList.
                        // Note that SuperNATURAL Drum Kits uses the names of the selected waves for the key naming.
                        keyIndex = 0;

                        // Look for a record in DrumKeyAssignList, starting after the last preset:
                        listIndex = commonState.PresetDrumKeyAssignListsCount;
                        while (listIndex < commonState.drumKeyAssignLists.ToneNames.Count())
                        {
                            if (commonState.drumKeyAssignLists.ToneNames[listIndex][1] == // Name is in second string in list.
                                tbEditTone_SaveTone_TitleText.Text.Trim())
                            {
                                break;
                            }
                            listIndex++;
                        }
                        if (listIndex >= commonState.drumKeyAssignLists.ToneNames.Count())
                        {
                            // This is a new list. Add it:
                            commonState.drumKeyAssignLists.ToneNames.Add(new List<String>());
                            listIndex = commonState.drumKeyAssignLists.ToneNames.Count() - 1;

                            // Fill out the header strings
                            commonState.drumKeyAssignLists.ToneNames[listIndex].Add("SuperNATURAL Drum Kit");
                            commonState.drumKeyAssignLists.ToneNames[listIndex].Add(tbEditTone_SaveTone_TitleText.Text.Trim());

                            // Fill out the names:
                            for (keyIndex = 0; keyIndex < commonState.keyNames.Count(); keyIndex++)
                            {
                                if (String.IsNullOrEmpty(commonState.keyNames[keyIndex]))
                                {
                                    commonState.drumKeyAssignLists.ToneNames[listIndex].Add("");
                                }
                                else
                                {
                                    commonState.drumKeyAssignLists.ToneNames[listIndex].Add(commonState.keyNames[keyIndex]);
                                }
                            }
                        }
                        else
                        {
                            // Fill out the names (first two lines are already filled out):
                            for (keyIndex = 0; keyIndex < commonState.keyNames.Count(); keyIndex++)
                            {
                                commonState.drumKeyAssignLists.ToneNames[listIndex][keyIndex + 2] = commonState.keyNames[keyIndex];
                            }
                        }

                        name = tbEditTone_SaveTone_TitleText.Text;
                        address = new byte[] { (byte)(0x19 + ((0x20 * commonState.CurrentPart) / 0x80)),
                            (byte)(0x03 + ((0x20 * commonState.CurrentPart) % 0x80)), 0x00, 0x00 };
                        value = Encoding.UTF8.GetBytes(name);
                        data = commonState.midi.SystemExclusiveDT1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
                        address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                        value = new byte[] { 0x58, 0x00,
                            (byte)cbEditTone_SaveTone_SlotNumber.SelectedIndex,
                            commonState.CurrentPart};
                        data = commonState.midi.SystemExclusiveRQ1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
                        // Also update toneList and toneNames:
                        tone = new List<String>();
                        tone.Add("SuperNATURAL Drum Kit");
                        tone.Add("Drum");
                        tone.Add(cbEditTone_SaveTone_SlotNumber.SelectedIndex.ToString());
                        tone.Add(name.Trim());
                        tone.Add("88");
                        tone.Add((cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128).ToString());
                        tone.Add((88 * 128 + (cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128)).ToString());
                        tone.Add(((cbEditTone_SaveTone_SlotNumber.SelectedIndex + 1) % 128).ToString());
                        tone.Add("(User)");
                        if (commonState.toneNames[4][cbEditTone_SaveTone_SlotNumber.SelectedIndex] == "INIT KIT")
                        {
                            // This will be a new one in toneList:
                            tone.Add(commonState.toneList.Tones.Count().ToString());
                            tone.Add("-1");
                            commonState.toneList.Tones.Add(tone);
                        }
                        else
                        {
                            Int32 i = commonState.toneList.PresetsCount; // Start looking for it after the last tone from soundlist pdf
                            Boolean found = false;
                            while (!found && i < commonState.toneList.Tones.Count())
                            {
                                if (commonState.toneList.Tones[i][0] == "SuperNATURAL Drum Kit"
                                    && commonState.toneList.Tones[i][1] == "Drum"
                                    && commonState.toneList.Tones[i][3].Trim() == name.Trim())
                                {
                                    found = true;
                                }
                                else
                                {
                                    i++;
                                }
                            }
                            if (found)
                            {
                                tone.Add(i.ToString());
                                tone.Add("-1");
                                commonState.toneList.Tones[i] = tone;
                            }
                            else
                            {
                                // This should never happen!
                                tone.Add(commonState.toneList.Tones.Count().ToString());
                                tone.Add("-1");
                                commonState.toneList.Tones.Add(tone);
                                //t.Trace("Save SuperNATURAL Drum Kit did not find expected tone! Tone was added instead.");
                            }
                        }
                        // Also update in toneNames:
                        commonState.toneNames[4][cbEditTone_SaveTone_SlotNumber.SelectedIndex] = name.Trim();
                        // Finally, re-draw the tab to update controls:
                        UpdateSuperNATURALDrumKitControls();
                        break;
                }
                Waiting(false);
            }
        }

        private async void btnEditTone_DeleteTone_Click(object sender, EventArgs e)
        {
            //MessageDialog warning = new MessageDialog("Do you wish to delete the selected tone?");
            //warning.Title = "Warning!";
            //warning.Commands.Add(new UICommand { Label = "Yes, delete selected sound.", Id = 0 });
            //warning.Commands.Add(new UICommand { Label = "Oh-oh, NOOO! Please don\'t!", Id = 1 });
            //var response = await warning.ShowAsync();
            String response = await mainPage.DisplayActionSheet("Do you wish to delete the selected tone?",
                null, null, new String[] { "Yes, delete selected sound.", "Oh-oh, NOOO! Please don\'t!" });
            initDone = true;
            //if ((Int32)response.Id == 0)
            if (response == "Yes, delete selected sound.")
            {
                switch (currentProgramType)
                {
                    case ProgramType.PCM_SYNTH_TONE:
                        // Delete in I-7:
                        String name = tbEditTone_SaveTone_TitleText.Text;
                        byte[] address = new byte[] { 0x19, 0x00, 0x00, 0x00 };
                        byte[] value = Encoding.UTF8.GetBytes("INIT TONE   ");
                        byte[] data = commonState.midi.SystemExclusiveDT1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
                        //for (byte c = 1; c < 0x80; c++)
                        {
                            //MessageDialog test = new MessageDialog("Trying c = " + c.ToString());
                            //test.Title = "Test!";
                            //test.Commands.Add(new UICommand { Label = "Go", Id = 0 });
                            //await test.ShowAsync();
                            address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                            value = new byte[] { 0x57, (byte)(cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128),
                            (byte)(cbEditTone_SaveTone_SlotNumber.SelectedIndex % 128), 0x00 }; // 0 initiated name, 3 copied the first name
                            data = commonState.midi.SystemExclusiveRQ1Message(address, value);
                            commonState.midi.SendSystemExclusive(data);
                        }
                        // Remove from ToneList:
                        Int32 i = commonState.toneList.PresetsCount; // Start looking for it after the last tone from soundlist pdf
                        Boolean found = false;
                        while (!found && i < commonState.toneList.Tones.Count())
                        {
                            if (commonState.toneList.Tones[i][0] == "PCM Synth Tone"
                                && commonState.toneList.Tones[i][1] == cbEditTone_InstrumentCategorySelector.SelectedItem.ToString()
                                && commonState.toneList.Tones[i][3].Trim() == name.Trim())
                            {
                                found = true;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        if (found)
                        {
                            commonState.toneList.Tones.RemoveAt(i);
                            while (i < commonState.toneList.Tones.Count - 1)
                            {
                                commonState.toneList.Tones[i][9] = i.ToString();
                                i++;
                            }
                        }
                        else
                        {
                            // This should never happen!
                            //t.Trace("Save PCM Synth Tone did not find expected tone! Tone was not deleted.");
                        }
                        // Also clear in toneNames and back-up current tone:
                        commonState.toneNames[0][cbEditTone_SaveTone_SlotNumber.SelectedIndex] = "INIT TONE";
                        commonState.currentTone = null;
                        // Finally, re-draw the tab to update controls:
                        UpdatePCMSynthToneControls();
                        break;
                    case ProgramType.PCM_DRUM_KIT:
                        // Delete in I-7:
                        name = tbEditTone_SaveTone_TitleText.Text;
                        address = new byte[] { 0x19, 0x10, 0x00, 0x00 };
                        value = Encoding.UTF8.GetBytes("INIT KIT    ");
                        data = commonState.midi.SystemExclusiveDT1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
                        address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                        value = new byte[] { 0x56, (byte)(cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128),
                        (byte)(cbEditTone_SaveTone_SlotNumber.SelectedIndex % 128), 0x00 };
                        data = commonState.midi.SystemExclusiveRQ1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
                        // Remove from ToneList:
                        i = commonState.toneList.PresetsCount; // Start looking for it after the last tone from soundlist pdf
                        found = false;
                        while (!found && i < commonState.toneList.Tones.Count())
                        {
                            if (commonState.toneList.Tones[i][0] == "PCM Drum Kit"
                                && commonState.toneList.Tones[i][1] == "Drum"
                                && commonState.toneList.Tones[i][3].Trim() == name.Trim())
                            {
                                found = true;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        if (found)
                        {
                            commonState.toneList.Tones.RemoveAt(i);
                            while (i < commonState.toneList.Tones.Count - 1)
                            {
                                commonState.toneList.Tones[i][9] = i.ToString();
                                i++;
                            }
                        }
                        else
                        {
                            // This should never happen!
                            //t.Trace("Save PCM Synth Tone did not find expected tone! Tone was not deleted.");
                        }
                        // Also clear in toneNames and back-up current tone:
                        commonState.toneNames[1][cbEditTone_SaveTone_SlotNumber.SelectedIndex] = "INIT KIT";
                        commonState.currentTone = null;
                        // Remove from keynames:
                        commonState.keyNames.Remove(name.Trim());
                        // Remove from drumKeyAssignLists:
                        found = false;
                        i = 0;
                        while (!found && i < commonState.drumKeyAssignLists.ToneNames.Count())
                        {
                            if (commonState.drumKeyAssignLists.ToneNames[i][1] == name.Trim())
                            {
                                found = true;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        if (found)
                        {
                            commonState.drumKeyAssignLists.ToneNames.RemoveAt(i);
                        }
                        // Finally, re-draw the tab to update controls:
                        UpdatePCMDrumKitControls();
                    break;
                    case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                        // Delete in I-7:
                        name = tbEditTone_SaveTone_TitleText.Text;
                        address = new byte[] { 0x19, 0x02, 0x00, 0x00 };
                        value = Encoding.UTF8.GetBytes("INIT TONE   ");
                        data = commonState.midi.SystemExclusiveDT1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
                        address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                        value = new byte[] { 0x59, (byte)(cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128),
                        (byte)(cbEditTone_SaveTone_SlotNumber.SelectedIndex % 128), 0x00 };
                        data = commonState.midi.SystemExclusiveRQ1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
                        // Remove from ToneList:
                        i = commonState.toneList.PresetsCount; // Start looking for it after the last tone from soundlist pdf
                        found = false;
                        while (!found && i < commonState.toneList.Tones.Count())
                        {
                            if (commonState.toneList.Tones[i][0] == "SuperNATURAL Acoustic Tone"
                                && commonState.toneList.Tones[i][1] == cbEditTone_InstrumentCategorySelector.SelectedItem.ToString()
                                && commonState.toneList.Tones[i][3].Trim() == name.Trim())
                            {
                                found = true;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        if (found)
                        {
                            commonState.toneList.Tones.RemoveAt(i);
                            while (i < commonState.toneList.Tones.Count - 1)
                            {
                                commonState.toneList.Tones[i][9] = i.ToString();
                                i++;
                            }
                        }
                        else
                        {
                            // This should never happen!
                            //t.Trace("Save SuperNATURAL Acoustic Tone did not find expected tone! Tone was not deleted.");
                        }
                        // Also clear in toneNames and back-up current tone:
                        commonState.toneNames[2][cbEditTone_SaveTone_SlotNumber.SelectedIndex] = "INIT TONE";
                        commonState.currentTone = null;
                        // Finally, re-draw the tab to update controls:
                        UpdateSuperNATURALAcousticToneControls();
                        break;
                    case ProgramType.SUPERNATURAL_SYNTH_TONE:
                        // Delete in I-7:
                        name = tbEditTone_SaveTone_TitleText.Text;
                        address = new byte[] { 0x19, 0x01, 0x00, 0x00 };
                        value = Encoding.UTF8.GetBytes("INIT TONE   ");
                        data = commonState.midi.SystemExclusiveDT1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
                        address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                        value = new byte[] { 0x5f, (byte)(cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128),
                        (byte)(cbEditTone_SaveTone_SlotNumber.SelectedIndex % 128), 0x00 };
                        data = commonState.midi.SystemExclusiveRQ1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
                        // Remove from ToneList:
                        i = commonState.toneList.PresetsCount; // Start looking for it after the last tone from soundlist pdf
                        found = false;
                        while (!found && i < commonState.toneList.Tones.Count())
                        {
                            if (commonState.toneList.Tones[i][0] == "SuperNATURAL Synth Tone"
                                && commonState.toneList.Tones[i][1] == cbEditTone_InstrumentCategorySelector.SelectedItem.ToString()
                                && commonState.toneList.Tones[i][3].Trim() == name.Trim())
                            {
                                found = true;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        if (found)
                        {
                            commonState.toneList.Tones.RemoveAt(i);
                            while (i < commonState.toneList.Tones.Count - 1)
                            {
                                commonState.toneList.Tones[i][9] = i.ToString();
                                i++;
                            }
                        }
                        else
                        {
                            // This should never happen!
                            //t.Trace("Save SuperNATURAL Synth Tone did not find expected tone! Tone was not deleted.");
                        }
                        // Also clear in toneNames and back-up current tone:
                        commonState.toneNames[3][cbEditTone_SaveTone_SlotNumber.SelectedIndex] = "INIT TONE";
                        commonState.currentTone = null;
                        // Finally, re-draw the tab to update controls:
                        UpdateSuperNATURALSynthToneControls();
                        break;
                    case ProgramType.SUPERNATURAL_DRUM_KIT:
                        // Delete in I-7:
                        name = tbEditTone_SaveTone_TitleText.Text;
                        address = new byte[] { 0x19, 0x03, 0x00, 0x00 };
                        value = Encoding.UTF8.GetBytes("INIT KIT    ");
                        data = commonState.midi.SystemExclusiveDT1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
                        address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                        value = new byte[] { 0x58, (byte)(cbEditTone_SaveTone_SlotNumber.SelectedIndex / 128),
                        (byte)(cbEditTone_SaveTone_SlotNumber.SelectedIndex % 128), 0x00 };
                        data = commonState.midi.SystemExclusiveRQ1Message(address, value);
                        commonState.midi.SendSystemExclusive(data);
                        // Remove from ToneList:
                        i = commonState.toneList.PresetsCount; // Start looking for it after the last tone from soundlist pdf
                        found = false;
                        while (!found && i < commonState.toneList.Tones.Count())
                        {
                            if (commonState.toneList.Tones[i][0] == "SuperNATURAL Drum Kit"
                                && commonState.toneList.Tones[i][1] == "Drum"
                                && commonState.toneList.Tones[i][3].Trim() == name.Trim())
                            {
                                found = true;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        if (found)
                        {
                            commonState.toneList.Tones.RemoveAt(i);
                            while (i < commonState.toneList.Tones.Count - 1)
                            {
                                commonState.toneList.Tones[i][9] = i.ToString();
                                i++;
                            }
                        }
                        else
                        {
                            // This should never happen!
                            //t.Trace("Save SuperNATURAL Drum Kit did not find expected tone! Tone was not deleted.");
                        }
                        // Also clear in toneNames and back-up current tone:
                        commonState.toneNames[4][cbEditTone_SaveTone_SlotNumber.SelectedIndex] = "INIT KIT";
                        commonState.currentTone = null;
                        // Finally, re-draw the tab to update controls:
                        UpdateSuperNATURALDrumKitControls();
                        break;
                }
            }
        }

        private void btnEditTone_Play_Click(object sender, EventArgs e)
        {
            if (initDone && handleControlEvents)
            {
                DrumKit_WaveOff();
                commonState.player.Play();
                //byte[] address = new byte[] { 0x0f, 0x00, 0x20, 0x00 };
                //byte[] data = new byte[1];
                //if (commonState.player.Playing)
                //{
                //    data = new byte[] { 0x00 };
                //    btnEditTone_Play.Content = "Play";
                //}
                //else
                //{
                //    data = new byte[] { (byte)(currentPart + 1) };
                //    btnEditTone_Play.Content = "Stop";
                //}
                //byte[] package = commonState.midi.SystemExclusiveDT1Message(address, data);
                //commonState.midi.SendSystemExclusive(package);
                //commonState.player.Playing = !commonState.player.Playing;
            }
        }

        private async void btnEditTone_Reset_Click(object sender, EventArgs e)
        {
            DrumKit_WaveOff();
            if (initDone && handleControlEvents)
            {
                //MessageDialog warning = new MessageDialog("You are about to reset all settings. Are you sure?");
                //warning.Title = "Warning!";
                //warning.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                //warning.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
                //var response = await warning.ShowAsync();
                String response = await mainPage.DisplayActionSheet("You are about to reset all settings. Are you sure?",
                    null, null, new String[] { "Ok", "Cancel" });
                //if ((Int32)response.Id == 0)
                if (response == "Ok")
                {
                    Reset();
                }
            }
        }

        private async void Reset()
        {
            //t.Trace("Reset()");
            Waiting(true);
            DrumKit_WaveOff();
            // Set the I-7 to a default sound. It always does that when initialized.
            // Read all from I-7.
            handleControlEvents = false;
            currentProgramIndex = (byte)(cbEditTone_SynthesizerType.SelectedIndex);
            currentProgramType = (ProgramType)cbEditTone_SynthesizerType.SelectedIndex;
            byte msb = 0;
            byte lsb = 64;
            byte pc = 1;
            switch (currentProgramType)
            {
                case ProgramType.PCM_SYNTH_TONE:
                    msb = 87;
                    break;
                case ProgramType.PCM_DRUM_KIT:
                    msb = 86;
                    break;
                case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                    msb = 89;
                    break;
                case ProgramType.SUPERNATURAL_SYNTH_TONE:
                    msb = 95;
                    break;
                case ProgramType.SUPERNATURAL_DRUM_KIT:
                    msb = 88;
                    break;
            }
            commonState.midi.ProgramChange(commonState.CurrentPart, msb, lsb, pc);
            initDone = false;
            Waiting(true);
            await Task.Delay(TimeSpan.FromMilliseconds(1000));
            QueryToneType();
        }

        //private void btnEditTone_Return_Click(object sender, EventArgs e)
        //{
        //    //t.Trace("private void btnEditTone_Return_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
        //    if (initDone && handleControlEvents)
        //    {
        //        RemoveControls(ControlsGrid);
        //        commonState.midi. midiInPort.MessageReceived -= Edit_MidiInPort_MessageReceived;
        //        try
        //        {
        //            this.Frame.Navigate(typeof(MainPage), commonState);
        //        }
        //        catch (Exception e2)
        //        {

        //        }
        //    }
        //}

        private void GenericCombobox_SelectionChanged(object sender, EventArgs e)
        {
            //t.Trace("private void GenericCombobox_SelectionChanged ()");
            if (initDone && handleControlEvents)
            {
                Waiting(true);
                ComboBox cb = (ComboBox)sender;
                //t.Trace("cb.Name = " + cb.Name);
                if (cb.Tag != null && cb.Tag.GetType() == typeof(Buddy))
                {
                    UInt16 tempAddress = (UInt16)(((Buddy)cb.Tag).Offset + 4 * ((Buddy)cb.Tag).ParameterNumber);
                    byte addressHi = (byte)(tempAddress / 128);
                    byte addressLo = (byte)(tempAddress % 128);
                    byte[] address = MakeAddress(currentProgramType, ParameterPage.COMMONMFX, 0x00);
                    address = AddBytes128(address, new byte[] { addressHi, addressLo });
                    byte[] pageOffset = new byte[] { 0x00, 0x00, 0x00, currentMFXTypePageAddressOffset };
                    address = AddBytes128(address, pageOffset);
                    byte[] value = new byte[] { 0x08, 0x00, (byte)((cb.SelectedIndex) / 16), (byte)((cb.SelectedIndex) % 16) };
                    byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
                    commonState.midi.SendSystemExclusive(bytes);
                    if (cb.Name.EndsWith("Hz/Note") || cb.Name.EndsWith("Ms/Note")) // Content is "# Ms/Note" where # is line number, which is needed and parsed out as lineNumber 3 lines down below.
                    {
                        // This type of control must also toggle visibility of the two sets of controls occupying next line!
                        UInt16 lineNumber = UInt16.Parse(cb.Name.Remove(cb.Name.IndexOf(' ')));
                        switch (cb.SelectedIndex)
                        {
                            case 1:
                                ((Grid)((Grid)cb.Parent.Parent).Children[lineNumber]).Children[0].IsVisible = false;
                                ((Grid)((Grid)cb.Parent.Parent).Children[lineNumber]).Children[1].IsVisible = false;
                                ((Grid)((Grid)cb.Parent.Parent).Children[lineNumber + 1]).Children[0].IsVisible = true;
                                ((Grid)((Grid)cb.Parent.Parent).Children[lineNumber + 1]).Children[1].IsVisible = true;
                                break;
                            case 0:
                                ((Grid)((Grid)cb.Parent.Parent).Children[lineNumber]).Children[0].IsVisible = true;
                                ((Grid)((Grid)cb.Parent.Parent).Children[lineNumber]).Children[1].IsVisible = true;
                                ((Grid)((Grid)cb.Parent.Parent).Children[lineNumber + 1]).Children[0].IsVisible = false;
                                ((Grid)((Grid)cb.Parent.Parent).Children[lineNumber + 1]).Children[1].IsVisible = false;
                                break;
                        }
                    }

                    HelpTag tag = ((Buddy)cb.Tag).Tag;
                    Help.Show(5, (byte)(currentMFXType + currentMFXTypeOffset), (byte)(((HelpTag)tag).ItemIndex), (byte)(((HelpTag)tag).SubItemIndex + cb.SelectedIndex));
                }
                else
                {
                    GenericHandler(cb);
                }
                Waiting(false);
            }
        }

        private void slEditToneChorusSendLevel_ValueChanged(object sender, EventArgs e)
        {
            //t.Trace("slEditToneChorusSendLevel_ValueChanged()");
            if (initDone && handleControlEvents)
            {
                //byte[] address = MakeAddress(currentProgramType, ParameterPage.COMMONMFX, new byte[] { 0x02 });
                byte[] address = new byte[] { 0x18, 0x00, (byte)(0x20 + commonState.CurrentPart), 0x27};
                tbEditToneChorusSendLevel.Text = "Chorus send level: " + slEditToneChorusSendLevel.Value.ToString();
                byte[] value = new byte[] { (byte)slEditToneChorusSendLevel.Value };
                SendParameter(address, value);
            }
        }

        private void slEditToneReverbSendLevel_ValueChanged(object sender, EventArgs e)
        {
            //t.Trace("slEditToneReverbSendLevel_ValueChanged()");
            if (initDone && handleControlEvents)
            {
                //byte[] address = MakeAddress(currentProgramType, ParameterPage.COMMONMFX, new byte[] { 0x03 });
                byte[] address = new byte[] { 0x18, 0x00, (byte)(0x20 + commonState.CurrentPart), 0x28 };
                tbEditToneReverbSendLevel.Text = "Reverb send level: " + slEditToneReverbSendLevel.Value.ToString();
                byte[] value = new byte[] { (byte)slEditToneReverbSendLevel.Value };
                SendParameter(address, value);
            }
        }

        private void GenericSlider_ValueChanged(object sender, EventArgs e)
        {
            //t.Trace("private void GenericSlider_ValueChanged (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
            if (initDone && handleControlEvents)
            {
                Slider sl = (Slider)sender;
                if (sl.Tag != null && sl.Tag.GetType() == typeof(Buddy))
                {
                    if (((Buddy)sl.Tag).TextBox != null)
                    {
                        ((Buddy)sl.Tag).TextBox.Text = ((Buddy)sl.Tag).TextBox.Text.Remove(((Buddy)sl.Tag).TextBox.Text.IndexOf(':')) + ": " + sl.Value.ToString();
                    }
                    else if (((Buddy)sl.Tag).CheckBox != null)
                    {
                        ((Buddy)sl.Tag).CheckBox.Content = ((String)((Buddy)sl.Tag).CheckBox.Content)
                            .Remove(((String)((Buddy)sl.Tag).CheckBox.Content).IndexOf(':')) + ": " + sl.Value.ToString();
                    }
                    UInt16 tempAddress = (UInt16)(((Buddy)sl.Tag).Offset + 4 * ((Buddy)sl.Tag).ParameterNumber);
                    byte addressHi = (byte)(tempAddress / 128);
                    byte addressLo = (byte)(tempAddress % 128);
                    byte[] address = MakeAddress(currentProgramType, ParameterPage.COMMONMFX, 0x00);
                    address = AddBytes128(address, new byte[] { addressHi, addressLo });
                    byte[] pageOffset = new byte[] { 0x00, 0x00, 0x00, currentMFXTypePageAddressOffset };
                    address = AddBytes128(address, pageOffset);
                    UInt16 dValue = (UInt16)(((sl.Value + ((Buddy)sl.Tag).ValueOffset)) * ((Buddy)sl.Tag).ValueMultiplier);
                    byte[] value = new byte[] { 0x08, (byte)((dValue & 0xf00) >> 8), (byte)((dValue & 0x0f0) >> 4), (byte)(dValue & 0x00f) };
                    byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
                    commonState.midi.SendSystemExclusive(bytes);
                }
                else
                {
                    GenericHandler(sl);
                }
            }
        }

        private void GenericCheckBox_Click(object sender, EventArgs e)
        {
            if (initDone && handleControlEvents)
            {
                //t.Trace("private void GenericCheckBox_Click (" + "object" + sender + ", " + "EventArgs" + e + ", " + ")");
                GenericCheckBox_Handler(sender);
            }
        }

        private void GenericCheckBox_Handler(object sender)
        {
            //t.Trace("private void GenericCheckBox_Handler (" + "object" + sender + ", " + ")");
            if (initDone && handleControlEvents)
            {
                CheckBox cb = (CheckBox)sender;
                if (cb.Tag != null && cb.Tag.GetType() == typeof(Buddy))
                {
                    UInt16 tempAddress = (UInt16)(((Buddy)cb.Tag).Offset + 4 * ((Buddy)cb.Tag).ParameterNumber);
                    byte addressHi = (byte)(tempAddress / 128);
                    byte addressLo = (byte)(tempAddress % 128);
                    byte[] address = MakeAddress(currentProgramType, ParameterPage.COMMONMFX, 0x00);
                    address = AddBytes128(address, new byte[] { addressHi, addressLo });
                    byte[] pageOffset = new byte[] { 0x00, 0x00, 0x00, currentMFXTypePageAddressOffset };
                    address = AddBytes128(address, pageOffset);
                    UInt16 dValue = (UInt16)((Boolean)(cb.IsChecked) ? 1 : 0);
                    byte[] value = new byte[] { 0x08, 0x00, 0x00, (byte)(dValue) };
                    byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
                    commonState.midi.SendSystemExclusive(bytes);
                    //SendCommonMFX();
                }
                else
                {
                    GenericHandler(cb);
                }
            }
        }

        /// <summary>
        /// Integra-7 sends MFX parameters from first to the altered one. If we need to mimic that behaviour,
        /// or send all parameters at once, use this function.
        /// </summary>
        private void SendCommonMFX()
        {
            byte[] address = MakeAddress(currentProgramType, ParameterPage.COMMONMFX, 0x11);
            byte[] data = new byte[] { };
            for (byte i = 0; i < commonMFX.MFXNumberedParameters.MFXLength; i++)
            {
                if (commonMFX.MFXNumberedParameters.Parameters.Parameters[i].Type >= PARAMETER_TYPE.COMBOBOX_BEND_AFT_SYS1_TO_SYS4
                    && commonMFX.MFXNumberedParameters.Parameters.Parameters[i].Type <= PARAMETER_TYPE.COMBOBOX_WAVE_SHAPE)
                {
                    byte[] value = new byte[] { 0x08, 0x00, (byte)((commonMFX.MFXNumberedParameters.Parameters.Parameters[i].Value.Value) / 16),
                        (byte)((commonMFX.MFXNumberedParameters.Parameters.Parameters[i].Value.Value) % 16) };
                    data = concatenateByteArrays(data, value);
                }
                else if (commonMFX.MFXNumberedParameters.Parameters.Parameters[i].Type >= PARAMETER_TYPE.SLIDER_0_05_TO_10_00_STEP_0_05
                    && commonMFX.MFXNumberedParameters.Parameters.Parameters[i].Type <= PARAMETER_TYPE.SLIDER_MINUS_W100_TO_D100_STEP_2)
                {
                    UInt16 dValue = commonMFX.MFXNumberedParameters.Parameters.Parameters[i].Value.Value;
                    byte[] value = new byte[] { 0x08, (byte)((dValue & 0xf00) >> 8), (byte)((dValue & 0x0f0) >> 4), (byte)(dValue & 0x00f) };
                    data = concatenateByteArrays(data, value);
                }
                else if (commonMFX.MFXNumberedParameters.Parameters.Parameters[i].Type >= PARAMETER_TYPE.CHECKBOX
                    && commonMFX.MFXNumberedParameters.Parameters.Parameters[i].Type <= PARAMETER_TYPE.CHECKBOX_4)
                {
                    byte[] value = new byte[] { 0x08, 0x00, 0x00, (byte)(commonMFX.MFXNumberedParameters.Parameters.Parameters[i].Value.Value) };
                }
            }
            byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, data);
            commonState.midi.SendSystemExclusive(bytes);
        }

        private byte[] concatenateByteArrays(byte[] a1, byte[] a2)
        {
            byte[] result = new byte[a1.Length + a2.Length];
            UInt16 i = 0;
            while (i < a1.Length)
            {
                result[i] = a1[i];
                i++;
            }
            while (i < a1.Length + a2.Length)
            {
                result[i] = a2[i - a1.Length];
                i++;
            }
            return result;
        }

        private void GenericHandler(object sender)
        {
            //t.Trace("private void GenericHandler (" + "object" + sender + ", " + ")");
            if (initDone && handleControlEvents)
            {
                ComboBox comboBox = null;
                Slider slider = null;
                CheckBox checkBox = null;
                String name = "";
                if (sender.GetType() == typeof(ComboBox))
                {
                    comboBox = (ComboBox)sender;
                    name = comboBox.Name;
                }
                else if (sender.GetType() == typeof(Slider))
                {
                    slider = (Slider)sender;
                    name = slider.Name;
                }
                else if (sender.GetType() == typeof(CheckBox))
                {
                    checkBox = (CheckBox)sender;
                    name = checkBox.Name;
                }
                //Control control = (Control)sender;

                //t.Trace("switch (currentProgramType = " + currentProgramType.ToString() + ")");
                switch (currentProgramType)
                {
                    case ProgramType.PCM_SYNTH_TONE:
                        switch (name)
                        {
                            // PCM Synth Tone ************************************************************************************
                            // Partial switches (used in all PCM Synth Tone pages):
                            case "cbEditTone_PCMSynthTone_Partial1Switch":
                                byte[] address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x05 });
                                pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0] = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMSynthTone_Partial2Switch":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x0e });
                                pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1] = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMSynthTone_Partial3Switch":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x17 });
                                pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2] = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMSynthTone_Partial4Switch":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x20 });
                                pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3] = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            // Common *****
                            case "cbEditTone_ToneTypeSelector":
                                // When changing tone type we have no valid parameters. 
                                // Tell Integra-7 to change to a corresponding sound, and then
                                // re-initialize classes:
                                currentParameterPageIndex = (byte)comboBox.SelectedIndex;
                                SetFirstSoundOfCurrentType();
                                EditTone_UpdateControls();
                                break;
                            case "cbEditTone_PCMSynthTone_Legato":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x17);
                                pCMSynthTone.pCMSynthToneCommon.LegatoSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.LegatoSwitch ? 1 : 0));
                                break;
                            case "cbEditTone_PCMSynthTone_LegatoTrigger":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x18);
                                pCMSynthTone.pCMSynthToneCommon.LegatoRetrigger = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.LegatoRetrigger ? 1 : 0));
                                break;
                            case "cbEditTone_PCMSynthTone_Portamento":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x19);
                                pCMSynthTone.pCMSynthToneCommon.PortamentoSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.PortamentoSwitch ? 1 : 0));
                                break;
                            case "cbEditTone_PCMSynthTone_ToneCategory":
                                pCMSynthTone.pCMSynthToneCommon2.ToneCategory = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON2, new byte[] { 0x10 });
                                byte[] value = new byte[] { (byte)(pCMSynthTone.pCMSynthToneCommon2.ToneCategory) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_PhraseNumber":
                                pCMSynthTone.pCMSynthToneCommon2.PhraseNumber = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON2, new byte[] { 0x38 });
                                value = new byte[] { 0x00, 0x00, (byte)(pCMSynthTone.pCMSynthToneCommon2.PhraseNumber / 16), (byte)(pCMSynthTone.pCMSynthToneCommon2.PhraseNumber % 16) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_PhraseOctaveShift":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON2, 0x13);
                                pCMSynthTone.pCMSynthToneCommon2.PhraseOctaveShift = (byte)(comboBox.SelectedIndex + 61);
                                SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon2.PhraseOctaveShift + 61));
                                break;
                            case "cbEditTone_PCMSynthTone_TonePriority":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x10);
                                pCMSynthTone.pCMSynthToneCommon.Priority = (byte)(comboBox.SelectedIndex);
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_OctaveShift":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x13);
                                pCMSynthTone.pCMSynthToneCommon.OctaveShift = (byte)(comboBox.SelectedIndex + 61);
                                SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.OctaveShift + 61));
                                break;
                            case "cbEditTone_PCMSynthTone_StretchTuneDepth":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x14);
                                pCMSynthTone.pCMSynthToneCommon.TuneDepth = (byte)(comboBox.SelectedIndex);
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_MonoPoly":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x16);
                                pCMSynthTone.pCMSynthToneCommon.MonoPoly = (byte)(comboBox.SelectedIndex);
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PortamentoMode":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x1a);
                                pCMSynthTone.pCMSynthToneCommon.PortamentoMode = (byte)(comboBox.SelectedIndex);
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PortamentoType":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x1b);
                                pCMSynthTone.pCMSynthToneCommon.PortamentoType = (byte)(comboBox.SelectedIndex);
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PortamentoStart":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x1c);
                                pCMSynthTone.pCMSynthToneCommon.PortamentoStart = (byte)(comboBox.SelectedIndex);
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "slEditTone_PCMSynthTone_ToneLevel":
                                pCMSynthTone.pCMSynthToneCommon.Level = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_ToneLevel.Text = "Tone Level: " + pCMSynthTone.pCMSynthToneCommon.Level.ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x0e);
                                SendParameter(address, pCMSynthTone.pCMSynthToneCommon.Level);
                                break;
                            case "slEditTone_PCMSynthTone_TonePan":
                                pCMSynthTone.pCMSynthToneCommon.Pan = (byte)(slider.Value + 64);
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
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x0f);
                                SendParameter(address, pCMSynthTone.pCMSynthToneCommon.Pan);
                                break;
                            case "slEditTone_PCMSynthTone_CoarseTune":
                                pCMSynthTone.pCMSynthToneCommon.CoarseTune = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_CoarseTune.Text = "Coarse tune: " + slider.Value.ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x11);
                                SendParameter(address, (byte)pCMSynthTone.pCMSynthToneCommon.CoarseTune);
                                break;
                            case "slEditTone_PCMSynthTone_FineTune":
                                pCMSynthTone.pCMSynthToneCommon.FineTune = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_FineTune.Text = "Fine tune: " + pCMSynthTone.pCMSynthToneCommon.FineTune.ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x12);
                                SendParameter(address, (byte)pCMSynthTone.pCMSynthToneCommon.FineTune);
                                break;
                            case "slEditTone_PCMSynthTone_AnalogFeel":
                                pCMSynthTone.pCMSynthToneCommon.AnalogFeel = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_AnalogFeel.Text = "Analog feel: " + pCMSynthTone.pCMSynthToneCommon.AnalogFeel.ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x15);
                                SendParameter(address, (byte)pCMSynthTone.pCMSynthToneCommon.AnalogFeel);
                                break;
                            case "slEditTone_PCMSynthTone_CutoffOffset":
                                pCMSynthTone.pCMSynthToneCommon.CutoffOffset = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_CutoffOffset.Text = "Cutoff offset: " + pCMSynthTone.pCMSynthToneCommon.CutoffOffset.ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x22);
                                SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.CutoffOffset + 64));
                                break;
                            case "slEditTone_PCMSynthTone_ResonanceOffset":
                                pCMSynthTone.pCMSynthToneCommon.ResonanceOffset = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_ResonanceOffset.Text = "Resonance offset: " + pCMSynthTone.pCMSynthToneCommon.ResonanceOffset.ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x23);
                                SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.ResonanceOffset + 64));
                                break;
                            case "slEditTone_PCMSynthTone_AttackTimeOffset":
                                pCMSynthTone.pCMSynthToneCommon.AttackTimeOffset = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_AttackTimeOffset.Text = "Attack time: " + (byte)slider.Value;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x24);
                                SendParameter(address, (byte)pCMSynthTone.pCMSynthToneCommon.AttackTimeOffset);
                                break;
                            case "slEditTone_PCMSynthTone_ReleaseTimeOffset":
                                pCMSynthTone.pCMSynthToneCommon.ReleaseTimeOffset = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_ReleaseTimeOffset.Text = "Release time offset: " + (byte)slider.Value;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x25);
                                SendParameter(address, (byte)pCMSynthTone.pCMSynthToneCommon.ReleaseTimeOffset);
                                break;
                            case "slEditTone_PCMSynthTone_VelocitySenseOffset":
                                pCMSynthTone.pCMSynthToneCommon.VelocitySenseOffset = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_VelocitySenseOffset.Text = "Velocity sense offset: " + (byte)slider.Value;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x26);
                                SendParameter(address, (byte)pCMSynthTone.pCMSynthToneCommon.VelocitySenseOffset);
                                break;
                            case "slEditTone_PCMSynthTone_PortamentoTime":
                                pCMSynthTone.pCMSynthToneCommon.PortamentoTime = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_PortamentoTime.Text = "Portamento time: " + slider.Value.ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x1d);
                                SendParameter(address, (byte)pCMSynthTone.pCMSynthToneCommon.PortamentoTime);
                                break;
                            // PCM Synth Tone Wave tab *****
                            case "cbEditTone_PCMSynthTone_Wave_WaveGroupType":
                                if (comboBox.SelectedIndex == 0)
                                {
                                    pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveGroupType = 0x00;
                                    pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveGroupID = 0;
                                    value = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 };
                                }
                                else
                                {
                                    pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveGroupType = 0x01;
                                    pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveGroupID = (UInt16)comboBox.SelectedIndex;
                                    value = new byte[] { 0x01, 0x00, 0x00, 0x00, (byte)pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveGroupID };
                                }
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x27 });
                                SendParameter(address, value, true);
                                PopulatePCMSynthToneWaveLists(cbEditTone_PCMSynthTone_WaveGroupType.SelectedIndex);
                                // When changing module to one that has fewer items, check count and adjust to last if needed:
                                if (pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveNumberL > cbEditTone_PCMSynthTone_Wave_WaveNumberL.Items.Count())
                                {
                                    pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveNumberL = (UInt16)(cbEditTone_PCMSynthTone_Wave_WaveNumberL.Items.Count() - 1);
                                }
                                if (pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveNumberR > cbEditTone_PCMSynthTone_Wave_WaveNumberR.Items.Count())
                                {
                                    pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveNumberR = (UInt16)(cbEditTone_PCMSynthTone_Wave_WaveNumberR.Items.Count() - 1);
                                }
                                cbEditTone_PCMSynthTone_Wave_WaveNumberL.SelectedIndex = pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveNumberL;
                                cbEditTone_PCMSynthTone_Wave_WaveNumberR.SelectedIndex = pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveNumberR;
                                break;
                            case "cbEditTone_PCMSynthTone_Wave_WaveGain":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveGain = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x34 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveGain) };
                                SendParameter(address, value, true);
                                break;
                            case "cbEditTone_PCMSynthTone_Wave_FXMColor":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveFXMColor = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x36 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveFXMColor) };
                                SendParameter(address, value, true);
                                break;
                            case "cbEditTone_PCMSynthTone_Wave_PartialDelayMode":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialDelayMode = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x09 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialDelayMode) };
                                SendParameter(address, value, true);
                                break;
                            case "cbEditTone_PCMSynthTone_Wave_WaveNumberL":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveNumberL = (UInt16)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x2c });
                                SendParameter(address, (UInt16)(pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveNumberL), true);
                                break;
                            case "cbEditTone_PCMSynthTone_Wave_WaveNumberR":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveNumberR = (UInt16)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x30 });
                                SendParameter(address, (UInt16)(pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveNumberR), true);
                                break;
                            case "slEditTone_PCMSynthTone_Wave_PartialDelayTime":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialDelayTime = (byte)slider.Value;
                                if (pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialDelayTime < 128)
                                {
                                    tbEditTone_PCMSynthTone_Wave_PartialDelayTime.Text = "Partial Delay Time: " + 
                                        pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialDelayTime.ToString();
                                }
                                else
                                {
                                    tbEditTone_PCMSynthTone_Wave_PartialDelayTime.Text = "Partial Delay Time: " + 
                                        toneLengths[pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialDelayTime - 128];
                                }
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x0a });
                                byte[] data = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialDelayTime / 16),
                                    (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialDelayTime % 16) };
                                SendParameter(address, data, true);
                                break;
                            case "cbEditTone_PCMSynthTone_Wave_WaveTempoSync":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x38 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveTempoSync = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0), true);
                                break;
                            case "cbEditTone_PCMSynthTone_Wave_WaveFXMSwitch":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x35 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveFXMSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0), true);
                                break;
                            case "slEditTone_pCMSynthTone_Wave_WaveFXMDepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveFXMDepth = (byte)((slider.Value));
                                tbEditTone_pCMSynthTone_Wave_WaveFXMDepth.Text = "Wave MFX Depth: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveFXMDepth)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x37 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveFXMDepth)), true);
                                break;
                            // PCM Synth Tone PMT tab *****
                            case "cbEditTone_PCMSynthTone_PMT":
                                currentPMT = (byte)comboBox.SelectedIndex;
                                handleControlEvents = false;
                                UpdatePCMSynthToneControls();
                                handleControlEvents = true;
                                break;
                            case "cbEditTone_PCMSynthTone_PMT_PMTVelocityControl":
                                pCMSynthTone.pCMSynthTonePMT.PMTVelocityControl = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x04 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePMT.PMTVelocityControl) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_PMT_StructureType1_2":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthTonePMT.StructureType1_2 = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x00 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePMT.StructureType1_2) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_PMT_Booster1_2":
                                pCMSynthTone.pCMSynthTonePMT.Booster1_2 = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x01 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePMT.Booster1_2) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_PMT_StructureType3_4":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthTonePMT.StructureType3_4 = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x02 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePMT.StructureType3_4) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_PMT_Booster3_4":
                                pCMSynthTone.pCMSynthTonePMT.Booster3_4 = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x03 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePMT.Booster3_4) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_Pitch_PartialRandomPitchDepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialRandomPitchDepth = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x03 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialRandomPitchDepth) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper":
                                pCMSynthTone.pCMSynthTonePMT.PMTKeyboardFadeWidthUpper[currentPMT] = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Fade Width Upper: " + pCMSynthTone.pCMSynthTonePMT.PMTKeyboardFadeWidthUpper[currentPMT].ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x09 + 0x09 * currentPMT) });
                                SendParameter(address, (byte)slider.Value);
                                break;
                            case "slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper":
                                pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeUpper[currentPMT] = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Range Upper: " + keyNames[pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeUpper[currentPMT]];
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x07 + 0x09 * currentPMT) });
                                SendParameter(address, (byte)slider.Value);
                                if (pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeUpper[currentPMT] < pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT])
                                {
                                    pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT] = pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeUpper[currentPMT];
                                    address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x06 + 0x09 * currentPMT) });
                                    SendParameter(address, (byte)pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT]);
                                    slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower.Value = pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT];
                                    tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Range Lower: " + keyNames[pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT]];
                                }
                                break;
                            case "slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower":
                                pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT] = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeLower.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Range Lower: " + keyNames[pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT]];
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x06 + 0x09 * currentPMT) });
                                SendParameter(address, (byte)slider.Value);
                                if (pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT] > pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeUpper[currentPMT])
                                {
                                    pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeUpper[currentPMT] = pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT];
                                    address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x07 + 0x09 * currentPMT) });
                                    SendParameter(address, (byte)pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT]);
                                    tbEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Range Upper: " + keyNames[pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeUpper[currentPMT]];
                                    slEditTone_PCMSynthTone_PMT_PMTKeyboardRangeUpper.Value = pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeUpper[currentPMT];
                                }
                                break;
                            case "slEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower":
                                pCMSynthTone.pCMSynthTonePMT.PMTKeyboardFadeWidthLower[currentPMT] = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_PMT_PMTKeyboardFadeWidthLower.Text = "PMT" + (currentPMT + 1).ToString() + " Keyboard Fade Width Lower: " + pCMSynthTone.pCMSynthTonePMT.PMTKeyboardFadeWidthLower[currentPMT].ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x08 + 0x09 * currentPMT) });
                                SendParameter(address, (byte)slider.Value);
                                break;
                            case "slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper":
                                pCMSynthTone.pCMSynthTonePMT.PMTVelocityFadeWidthUpper[currentPMT] = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Fade Width Upper: " + pCMSynthTone.pCMSynthTonePMT.PMTVelocityFadeWidthUpper[currentPMT].ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x0d + 0x09 * currentPMT) });
                                SendParameter(address, (byte)slider.Value);
                                break;
                            case "slEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower":
                                pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT] = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Range Lower: " + pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT].ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x0a + 0x09 * currentPMT) });
                                SendParameter(address, (byte)slider.Value);
                                if (pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT] > pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeUpper[currentPMT])
                                {
                                    pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeUpper[currentPMT] = pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT];
                                    address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x07 + 0x09 * currentPMT) });
                                    SendParameter(address, (byte)pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT]);
                                    tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Range Upper: " + keyNames[pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeUpper[currentPMT]];
                                    slEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper.Value = pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeUpper[currentPMT];
                                }
                                break;
                            case "slEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper":
                                pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeUpper[currentPMT] = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeUpper.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Range Upper: " + pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeUpper[currentPMT].ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x0b + 0x09 * currentPMT) });
                                SendParameter(address, (byte)slider.Value);
                                if (pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeUpper[currentPMT] < pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT])
                                {
                                    pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT] = pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeUpper[currentPMT];
                                    address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x06 + 0x09 * currentPMT) });
                                    SendParameter(address, (byte)pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT]);
                                    slEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower.Value = pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT];
                                    tbEditTone_PCMSynthTone_PMT_PMTVelocityRangeLower.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Range Lower: " + keyNames[pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT]];
                                }
                                break;
                            case "slEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower":
                                pCMSynthTone.pCMSynthTonePMT.PMTVelocityFadeWidthLower[currentPMT] = (byte)slider.Value;
                                tbEditTone_PCMSynthTone_PMT_PMTVelocityFadeWidthLower.Text = "PMT" + (currentPMT + 1).ToString() + " Velocity Fade Width Lower: " + pCMSynthTone.pCMSynthTonePMT.PMTVelocityFadeWidthLower[currentPMT].ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x0c + 0x09 * currentPMT) });
                                SendParameter(address, (byte)slider.Value);
                                break;
                            case "cbEditTone_PCMSynthTone_PMTControlSwitch":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x28 });
                                pCMSynthTone.pCMSynthToneCommon.PMTControlSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            // PCM Synth Tone Pitch tab *****
                            case "slEditTone_PCMSynthTone_Pitch_PartialFineTune":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialFineTune = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_Pitch_PartialFineTune.Text = "Partial " + (currentPartial + 1).ToString() + " Fine Tune: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialFineTune - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x02 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialFineTune)), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitch_PartialCoarseTune":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialCoarseTune = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_Pitch_PartialCoarseTune.Text = "Partial " + (currentPartial + 1).ToString() + " Coarse Tune: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialCoarseTune - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialCoarseTune)), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow":
                                slider.Value = AdjustSliderStep(slider.Value, 10); // TODO: Check how to handle a 10 step slider (this is the only case it is not 1 step)
                                if (slider.Value % 10 == 1)
                                {
                                    slider.Value = slider.Value + 9;
                                }
                                else if (slider.Value % 10 == 9)
                                {
                                    slider.Value = slider.Value - 9;
                                }
                                else if (slider.Value % 10 < 5)
                                {
                                    slider.Value = slider.Value - slider.Value % 10;
                                }
                                else
                                {
                                    slider.Value = slider.Value - slider.Value % 10 + 10;
                                }
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].WavePitchKeyfollow = (byte)(slider.Value / 10 + 64);
                                tbEditTone_PCMSynthTone_Pitch_WavePitchKeyfollow.Text = "Wave Pitch Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].WavePitchKeyfollow - 64) * 10).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x39 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].WavePitchKeyfollow)), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitch_PitchBendRangeUp":
                                pCMSynthTone.pCMSynthToneCommon.PitchBendRangeUp = (byte)(slider.Value);
                                tbEditTone_PCMSynthTone_Pitch_PitchBendRangeUp.Text = "Pitch Bend Range Up: " + (pCMSynthTone.pCMSynthToneCommon.PitchBendRangeUp).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x29 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthToneCommon.PitchBendRangeUp)));
                                break;
                            case "slEditTone_PCMSynthTone_Pitch_PitchBendRangeDown":
                                pCMSynthTone.pCMSynthToneCommon.PitchBendRangeDown = (byte)(0 - slider.Value);
                                tbEditTone_PCMSynthTone_Pitch_PitchBendRangeDown.Text = "Pitch Bend Range Down: " + (0 - pCMSynthTone.pCMSynthToneCommon.PitchBendRangeDown).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x2a });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthToneCommon.PitchBendRangeDown)));
                                break;
                            // PCM Synth Tone Pitch envelope tab *****
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvDepth = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvDepth.Text = "Depth: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvDepth - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3a });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvDepth)), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvVelocitySens = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvVelocitySens.Text = "Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvVelocitySens - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3b });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvVelocitySens)), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime1VelocitySens = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1VelocitySens.Text = "Time 1 Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime1VelocitySens - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3c });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime1VelocitySens)), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime4VelocitySens = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4VelocitySens.Text = "Time 4 Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime4VelocitySens - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3d });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime4VelocitySens)), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTimeKeyfollow = (byte)(slider.Value / 10 + 64);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTimeKeyfollow.Text = "Time Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTimeKeyfollow - 64) * 10).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3e });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTimeKeyfollow)), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime1":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[0] = (byte)(slider.Value);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[0].Text = "Time 1: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[0]).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3f });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[0])), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime2":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[1] = (byte)(slider.Value);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[1].Text = "Time 2: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[1]).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x40 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[1])), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime3":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[2] = (byte)(slider.Value);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[2].Text = "Time 3: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[2]).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x41 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[2])), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime4":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[3] = (byte)(slider.Value);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvTime[3].Text = "Time 4: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[3]).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x42 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[3])), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel0":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[0] = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[0].Text = "Level 0: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[0] - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x43 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[0])), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel1":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[1] = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[1].Text = "Level 1: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[1] - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x44 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[1])), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel2":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[2] = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[2].Text = "Level 2: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[2] - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x45 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[2])), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel3":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[3] = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[3].Text = "Level 3: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[3] - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x46 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[3])), true);
                                break;
                            case "slEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel4":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[4] = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_Pitchenvelope_PitchEnvLevel[4].Text = "Level 4: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[4] - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x47 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[4])), true);
                                break;
                            // PCM Synth Tone TVF tab *****
                            case "cbEditTone_PCMSynthTone_TVF_TVFFilterType":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFFilterType = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x48 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFFilterType) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_PCMSynthTone_TVF_TVFCutoffFrequency":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffFrequency = (byte)(slider.Value);
                                tbEditTone_PCMSynthTone_TVF_TVFCutoffFrequency.Text = "TVF Cutoff Frequency: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffFrequency).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x49 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffFrequency)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVF_TVFResonance":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFResonance = (byte)(slider.Value);
                                tbEditTone_PCMSynthTone_TVF_TVFResonance.Text = "TVF Resonance: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFResonance).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x4d });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFResonance)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffKeyfollow = (byte)(slider.Value / 10 + 64);
                                tbEditTone_PCMSynthTone_TVF_TVFCutoffKeyfollow.Text = "TVF Cutoff Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffKeyfollow - 64) * 10).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x4a });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffKeyfollow)), true);
                                break;
                            case "cbEditTone_PCMSynthTone_TVF_TVFCutoffVelocityCurve":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffVelocityCurve = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x4b });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffVelocityCurve) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffVelocitySens = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_TVF_TVFCutoffVelocitySens.Text = "TVF Cutoff Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffVelocitySens - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x4c });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffVelocitySens)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFResonanceVelocitySens = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_TVF_TVFResonanceVelocitySens.Text = "TVF Resonance Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFResonanceVelocitySens - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x4e });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFResonanceVelocitySens)), true);
                                break;
                            // PCM Synth Tone TVF Envelope tab *****
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvDepth = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvDepth.Text = "Depth: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvDepth - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x4f });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvDepth)), true);
                                break;
                            case "cbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocityCurve":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvVelocityCurve = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x50 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvVelocityCurve) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvVelocitySens = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvVelocitySens.Text = "Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvVelocitySens - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x51 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvVelocitySens)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime1VelocitySens = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1VelocitySens.Text = "Time 1 Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime1VelocitySens - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x52 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime1VelocitySens)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime4VelocitySens = (byte)(slider.Value + 64);
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime4VelocitySens.Text = "Time 4 Velocity Sens: " + (pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime4VelocitySens - 64).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x53 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime4VelocitySens)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTimeKeyfollow = (byte)((slider.Value / 10) + 64);
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTimeKeyfollow.Text = "Time Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTimeKeyfollow - 64) * 10).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x54 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTimeKeyfollow)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime0":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[0] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[0].Text = "Time 1: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[0])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x55 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[0])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime1":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[1] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[1].Text = "Time 2: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[1])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x56 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[1])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime2":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[2] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[2].Text = "Time 3: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[2])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x57 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[2])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime3":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[3] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvTime[3].Text = "Time 4: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[3])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x58 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[3])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel0":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[0] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[0].Text = "Level 0: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[0])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x59 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[0])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel1":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[1] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[1].Text = "Level 1: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[1])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x5a });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[1])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel2":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[2] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[2].Text = "Level 2: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[2])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x5b });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[2])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel3":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[3] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[3].Text = "Level 3: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[3])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x5c });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[3])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel4":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[4] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVFEnvelope_TVFEnvLevel[4].Text = "Level 4: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[4])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x5d });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[4])), true);
                                break;
                            // PCM Synth Tone TVA tab *****
                            case "slEditTone_PCMSynthTone_TVA_PartialOutputLevel":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialOutputLevel = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVA_PartialOutputLevel.Text = "Partial Output Level: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialOutputLevel)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x00 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialOutputLevel)), true);
                                break;
                            case "cbEditTone_PCMSynthTone_TVA_TVALevelVelocityCurve":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVALevelVelocityCurve = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x61 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVALevelVelocityCurve) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_PCMSynthTone_TVA_TVALevelVelocitySens":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVALevelVelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_TVA_TVALevelVelocitySens.Text = "TVA Level Velocity Sens: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVALevelVelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x62 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVALevelVelocitySens)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVA_BiasLevel":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].BiasLevel = (byte)((slider.Value / 10) + 64);
                                tbEditTone_PCMSynthTone_TVA_BiasLevel.Text = "Bias Level: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].BiasLevel - 64) * 10).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x5e });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].BiasLevel)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVA_BiasPosition":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].BiasPosition = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVA_BiasPosition.Text = "Bias Position: " + keyNames[((pCMSynthTone.pCMSynthTonePartial[currentPartial].BiasPosition))];
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x5f });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].BiasPosition)), true);
                                break;
                            case "cbEditTone_PCMSynthTone_TVA_BiasDirection":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].BiasDirection = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x60 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].BiasDirection) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_PCMSynthTone_TVA_PartialPan":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialPan = (byte)((slider.Value + 64));
                                if (pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialPan < 64)
                                {
                                    tbEditTone_PCMSynthTone_TVA_PartialPan.Text = "Partial Pan: L" + (Math.Abs(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialPan - 64)).ToString();
                                }
                                else if (pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialPan > 64)
                                {
                                    tbEditTone_PCMSynthTone_TVA_PartialPan.Text = "Partial Pan: R" + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialPan - 64)).ToString();
                                }
                                else
                                {
                                    tbEditTone_PCMSynthTone_TVA_PartialPan.Text = "Partial Pan: Center";
                                }
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x04 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialPan)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVA_PartialPanKeyfollow":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialPanKeyfollow = (byte)((slider.Value / 10) + 64);
                                tbEditTone_PCMSynthTone_TVA_PartialPanKeyfollow.Text = "Partial Pan Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialPanKeyfollow - 64) * 10).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x05 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialPanKeyfollow)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVA_PartialRandomPanDepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialRandomPanDepth = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVA_PartialRandomPanDepth.Text = "Partial Random Pan Depth: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialRandomPanDepth)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x06 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialRandomPanDepth)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialAlternatePanDepth = (byte)((slider.Value) + 64);
                                if (pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialAlternatePanDepth < 64)
                                {
                                    tbEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.Text = "Alternate Pan Depth: L" + (Math.Abs(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialAlternatePanDepth - 64)).ToString();
                                }
                                else if (pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialAlternatePanDepth > 64)
                                {
                                    tbEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.Text = "Alternate Pan Depth: R" + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialAlternatePanDepth - 64)).ToString();
                                }
                                else
                                {
                                    tbEditTone_PCMSynthTone_TVA_PartialAlternatePanDepth.Text = "Alternate Pan Depth: Center";
                                }
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x07 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialAlternatePanDepth)), true);
                                break;
                            // PCM Synth Tone TVA Envelope tab *****
                            case "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime1VelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1VelocitySens.Text = "Time 1 Velocity Sens: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime1VelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x63 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime1VelocitySens)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime4VelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4VelocitySens.Text = "Time 4 Velocity Sens: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime4VelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x64 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime4VelocitySens)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTimeKeyfollow = (byte)((slider.Value / 10) + 64);
                                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTimeKeyfollow.Text = "Time Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTimeKeyfollow - 64) * 10).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x65 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTimeKeyfollow)), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime1":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[0] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[0].Text = "TVA Env Time 1: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[0])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x66 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[0])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime2":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[1] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[1].Text = "TVA Env Time 2: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[1])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x67 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[1])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime3":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[2] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[2].Text = "TVA Env Time 3: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[2])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x68 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[2])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime4":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[3] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvTime[3].Text = "TVA Env Time 4: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[3])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x69 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[3])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel1":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvLevel[0] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[0].Text = "TVA Env Level 1: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvLevel[0])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x6a });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvLevel[0])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel2":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvLevel[1] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[1].Text = "TVA Env Level 2: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvLevel[1])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x6b });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvLevel[1])), true);
                                break;
                            case "slEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel3":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvLevel[2] = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_TVAEnvelope_TVAEnvLevel[2].Text = "TVA Env Level 3: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvLevel[2])).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x6c });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvLevel[2])), true);
                                break;
                            // PCM Synth Tone Output tab *****
                            case "slEditTone_PCMSynthTone_Output_PartialOutputLevel":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialOutputLevel = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_Output_PartialOutputLevel.Text = "Partial Output Level: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialOutputLevel)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0X0c });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialOutputLevel)), true);
                                break;
                            case "slEditTone_PCMSynthTone_Output_PartialChorusSendLevel":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialChorusSendLevel = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_Output_PartialChorusSendLevel.Text = "Partial Chorus Send: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialChorusSendLevel)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0X0f });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialChorusSendLevel)), true);
                                break;
                            case "slEditTone_PCMSynthTone_Output_PartialReverbSendLevel":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialReverbSendLevel = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_Output_PartialReverbSendLevel.Text = "Partial Reverb Send: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialReverbSendLevel)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0X10 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialReverbSendLevel)), true);
                                break;
                            // PCM Synth Tone LFO1 tab *****
                            case "cbEditTone_PCMSynthTone_LFO1_LFO1Waveform":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOWaveform = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x6d });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOWaveform) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO1_LFORate":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFORate = (byte)((slider.Value));
                                if (pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFORate < 128)
                                {
                                    tbEditTone_PCMSynthTone_LFO1_LFORate.Text = "LFO Rate: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFORate)).ToString();
                                }
                                else
                                {
                                    tbEditTone_PCMSynthTone_LFO1_LFORate.Text = "LFO Rate: " + toneLengths[pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFORate - 128];
                                }
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x6e });
                                data = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFORate / 16), (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFORate % 16) };
                                SendParameter(address, data, true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO1_LFORateDetune":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFORateDetune = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_LFO1_LFORateDetune.Text = "LFO Rate Detune: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFORateDetune)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x71 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFORateDetune)), true);
                                break;
                            case "cbEditTone_PCMSynthTone_LFO1_LFOOffset":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOOffset = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x70 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOOffset) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO1_LFODelayTime":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFODelayTime = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_LFO1_LFODelayTime.Text = "Delay Time: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFODelayTime)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x72 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFODelayTime)), true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFODelayTimeKeyfollow = (byte)((slider.Value / 10) + 64);
                                tbEditTone_PCMSynthTone_LFO1_LFODelayTimeKeyfollow.Text = "Del-T Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFODelayTimeKeyfollow - 64) * 10).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x73 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFODelayTimeKeyfollow)), true);
                                break;
                            case "cbEditTone_PCMSynthTone_LFO1_LFOFadeMode":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOFadeMode = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x74 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOFadeMode) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO1_LFOFadeTime":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOFadeTime = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_LFO1_LFOFadeTime.Text = "LFO Fade Time: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOFadeTime)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x75 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOFadeTime)), true);
                                break;
                            case "cbEditTone_PCMSynthTone_LFO1KeyTrigger":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x76 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOKeyTrigger = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0), true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO1_LFOPitchDepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOPitchDepth = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_LFO1_LFOPitchDepth.Text = "LFO Pitch Depth: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOPitchDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x77 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOPitchDepth)), true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO1_LFOTVFDepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOTVFDepth = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_LFO1_LFOTVFDepth.Text = "LFO TVF Depth: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOTVFDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x78 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOTVFDepth)), true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO1_LFOTVADepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOTVADepth = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_LFO1_LFOTVADepth.Text = "LFO TVA Depth: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOTVADepth - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x79 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOTVADepth)), true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO1_LFOPanDepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOPanDepth = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_LFO1_LFOPanDepth.Text = "LFO Pan Depth: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOPanDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x7a });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOPanDepth)), true);
                                break;
                            // PCM Synth Tone LFO2 tab *****
                            case "cbEditTone_PCMSynthTone_LFO2_LFO2Waveform":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOWaveform = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x7b });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOWaveform) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO2_LFORate":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFORate = (byte)((slider.Value));
                                if (pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFORate < 128)
                                {
                                    tbEditTone_PCMSynthTone_LFO2_LFORate.Text = "LFO Rate: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFORate)).ToString();
                                }
                                else
                                {
                                    tbEditTone_PCMSynthTone_LFO2_LFORate.Text = "LFO Rate: " + toneLengths[pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFORate - 128];
                                }
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x7c });
                                data = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFORate / 16), (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFORate % 16) };
                                SendParameter(address, data, true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO2_LFORateDetune":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFORateDetune = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_LFO2_LFORateDetune.Text = "LFO Rate Detune: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFORateDetune)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x7f });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFORateDetune)), true);
                                break;
                            case "cbEditTone_PCMSynthTone_LFO2_LFOOffset":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOOffset = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x7e });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOOffset) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO2_LFODelayTime":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFODelayTime = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_LFO2_LFODelayTime.Text = "Delay Time: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFODelayTime)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x00 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFODelayTime)), true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFODelayTimeKeyfollow = (byte)((slider.Value / 10) + 64);
                                tbEditTone_PCMSynthTone_LFO2_LFODelayTimeKeyfollow.Text = "Del-T Keyfollow: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFODelayTimeKeyfollow - 64) * 10).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x01 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFODelayTimeKeyfollow)), true);
                                break;
                            case "cbEditTone_PCMSynthTone_LFO2_LFOFadeMode":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOFadeMode = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x02 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOFadeMode) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO2_LFOFadeTime":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOFadeTime = (byte)((slider.Value));
                                tbEditTone_PCMSynthTone_LFO2_LFOFadeTime.Text = "LFO Fade Time: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOFadeTime)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x03 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOFadeTime)), true);
                                break;
                            case "cbEditTone_PCMSynthTone_LFO2KeyTrigger":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x04 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOKeyTrigger = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0), true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO2_LFOPitchDepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOPitchDepth = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_LFO2_LFOPitchDepth.Text = "LFO Pitch Depth: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOPitchDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x05 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOPitchDepth)), true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO2_LFOTVFDepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOTVFDepth = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_LFO2_LFOTVFDepth.Text = "LFO TVF Depth: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOTVFDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x06 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOTVFDepth)), true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO2_LFOTVADepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOTVADepth = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_LFO2_LFOTVADepth.Text = "LFO TVA Depth: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOTVADepth - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x07 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOTVADepth)), true);
                                break;
                            case "slEditTone_PCMSynthTone_LFO2_LFOPanDepth":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOPanDepth = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_LFO2_LFOPanDepth.Text = "LFO Pan Depth: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOPanDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x08 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOPanDepth)), true);
                                break;
                            // PCM Synth Tone Step LFO tab *****
                            case "cbEditTone_PCMSynthTone_StepLFO_LFOStepType":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStepType = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x09 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStepType) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep0":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[0] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[0].Text = "Step 1: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[0] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x0a });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[0])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep1":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[1] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[1].Text = "Step 2: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[1] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x0b });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[1])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep2":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[2] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[2].Text = "Step 3: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[2] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x0c });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[2])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep3":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[3] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[3].Text = "Step 4: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[3] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x0d });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[3])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep4":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[4] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[4].Text = "Step 5: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[4] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x0e });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[4])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep5":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[5] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[5].Text = "Step 6: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[5] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x0f });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[5])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep6":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[6] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[6].Text = "Step 7: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[6] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x10 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[6])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep7":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[7] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[7].Text = "Step 8: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[7] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x11 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[7])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep8":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[8] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[8].Text = "Step 9: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[8] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x12 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[8])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep9":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[9] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[9].Text = "Step 10: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[9] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x13 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[9])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep10":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[10] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[10].Text = "Step 11: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[10] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x14 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[10])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep11":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[11] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[11].Text = "Step 12: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[11] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x15 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[11])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep12":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[12] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[12].Text = "Step 13: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[12] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x16 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[12])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep13":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[13] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[13].Text = "Step 14: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[13] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x17 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[13])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep14":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[14] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[14].Text = "Step 15: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[14] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x18 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[14])), true);
                                break;
                            case "slEditTone_PCMSynthTone_StepLFO_LFOStep15":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[15] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_StepLFO_LFOStep[15].Text = "Step 16: " + ((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[15] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x19 });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[15])), true);
                                break;
                            // PCM Synth Tone Partial tab *****
                            case "cbEditTone_PCMSynthTone_Control_PartialEnvMode":
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialEnvMode = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x08 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialEnvMode) };
                                SendParameter(address, value, true);
                                break;
                            case "cbEditTone_PCMSynthTone_PartialReceiveBender":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x12 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialReceiveBender = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0), true);
                                break;
                            case "cbEditTone_PCMSynthTone_PartialReceiveExpression":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x13 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialReceiveExpression = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0), true);
                                break;
                            case "cbEditTone_PCMSynthTone_PartialReceiveHold_1":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x14 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialReceiveHold_1 = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0), true);
                                break;
                            case "cbEditTone_PCMSynthTone_PartialRedamperSwitch":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x16 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialRedamperSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0), true);
                                break;
                            // PCM Synth Tone Matrix control tab *****
                            case "cbEditTone_PCMSynthTone_MatrixControl_Page":
                                if (handleControlEvents)
                                {
                                    RemoveControls(ControlsGrid);
                                    handleControlEvents = false;
                                    currentMatrixControlPage = (byte)comboBox.SelectedIndex;
                                    AddPCMSynthToneMatrixControlControls();
                                }
                                break;
                            case "cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSource":
                                pCMSynthTone.pCMSynthToneCommon.MatrixControlSource[currentMatrixControlPage] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { (byte)(0x2b + 9 * currentMatrixControlPage) });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlSource[currentMatrixControlPage]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination0":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][0] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x2c });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][0]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination1":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][1] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x2e });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][1]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination2":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][2] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x30 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][2]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_MatrixControl1_MatrixControlDestination3":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][3] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x32 });
                                value = new byte[] { (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][3]) };
                                SendParameter(address, value);
                                break;
                            // PCM Synth Tone Partial tab *****
                            // We cannot use automatic partial addressing for the following 16 controls since matrix control shows switches from all 4 partials at the same time!
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch00":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x17 + 4 * currentMatrixControlPage) });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][0] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch01":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x18 + 4 * currentMatrixControlPage) });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][1] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch02":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x19 + 4 * currentMatrixControlPage) });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][2] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch03":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x1a + 4 * currentMatrixControlPage) });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][3] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch10":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x17 + 4 * currentMatrixControlPage) });
                                address = AddBytes128(address, new byte[] { 0x02, 0x00 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][0] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch11":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x18 + 4 * currentMatrixControlPage) });
                                address = AddBytes128(address, new byte[] { 0x02, 0x00 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][1] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch12":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x19 + 4 * currentMatrixControlPage) });
                                address = AddBytes128(address, new byte[] { 0x02, 0x00 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][2] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch13":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x1a + 4 * currentMatrixControlPage) });
                                address = AddBytes128(address, new byte[] { 0x02, 0x00 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][3] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch20":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x17 + 4 * currentMatrixControlPage) });
                                address = AddBytes128(address, new byte[] { 0x04, 0x00 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][0] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch21":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x18 + 4 * currentMatrixControlPage) });
                                address = AddBytes128(address, new byte[] { 0x04, 0x00 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][1] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch22":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x19 + 4 * currentMatrixControlPage) });
                                address = AddBytes128(address, new byte[] { 0x04, 0x00 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][2] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch23":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x1a + 4 * currentMatrixControlPage) });
                                address = AddBytes128(address, new byte[] { 0x04, 0x00 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][3] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch30":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x17 + 4 * currentMatrixControlPage) });
                                address = AddBytes128(address, new byte[] { 0x06, 0x00 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][0] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch31":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x18 + 4 * currentMatrixControlPage) });
                                address = AddBytes128(address, new byte[] { 0x06, 0x00 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][1] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch32":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x19 + 4 * currentMatrixControlPage) });
                                address = AddBytes128(address, new byte[] { 0x06, 0x00 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][2] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            case "cbEditTone_PCMSynthTone_PartialControlSwitch33":
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x1a + 4 * currentMatrixControlPage) });
                                address = AddBytes128(address, new byte[] { 0x06, 0x00 });
                                pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][3] = (byte)comboBox.SelectedIndex;
                                SendParameter(address, (byte)(comboBox.SelectedIndex));
                                break;
                            // PCM Synth Tone Matrix tab *****
                            case "slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens0":
                                pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][0] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][0].Text = "Matrix Control 1 Sens: " + ((pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][0] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { (byte)(0x2d + 9 * currentMatrixControlPage) });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][0])));
                                break;
                            case "slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens1":
                                pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][1] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][1].Text = "Matrix Control 2 Sens: " + ((pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][1] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { (byte)(0x2f + 9 * currentMatrixControlPage) });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][1])));
                                break;
                            case "slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens2":
                                pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][2] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][2].Text = "Matrix Control 3 Sens: " + ((pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][2] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { (byte)(0x31 + 9 * currentMatrixControlPage) });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][2])));
                                break;
                            case "slEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens3":
                                pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][3] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_MatrixControl1_MatrixControlSens[currentMatrixControlPage][3].Text = "Matrix Control 4 Sens: " + ((pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][3] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { (byte)(0x33 + 9 * currentMatrixControlPage) });
                                SendParameter(address, (byte)((pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][3])));
                                break;
                            // PCM Synth Tone MFX *****
                            case "cbEditTone_MFXType":
                                HandleCommonMFX(comboBox);
                                break;
                            // PCM Synth Tone MFX control *****
                            case "cbEditTone_PCMSynthTone_MFXControl_MFXControlSource0":
                                commonMFX.MFXControlSource[0] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x05 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[0]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_MFXControl_MFXControlSource1":
                                commonMFX.MFXControlSource[1] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x07 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[1]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_MFXControl_MFXControlSource2":
                                commonMFX.MFXControlSource[2] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x09 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[2]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_MFXControl_MFXControlSource3":
                                commonMFX.MFXControlSource[3] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0b });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[3]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_MFXControl_MFXControlDestination0":
                                commonMFX.MFXControlAssign[0] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0d });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[0]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_MFXControl_MFXControlDestination1":
                                commonMFX.MFXControlAssign[1] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0e });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[1]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_MFXControl_MFXControlDestination2":
                                commonMFX.MFXControlAssign[2] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0f });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[2]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMSynthTone_MFXControl_MFXControlDestination3":
                                commonMFX.MFXControlAssign[3] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x10 });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[3]) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMSynthTone_MFXControl_MFXControlSense0":
                                commonMFX.MFXControlSens[0] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_MFXControl_MFXControlSens[0].Text = "MFX Control 1 Sense: " + ((commonMFX.MFXControlSens[0] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x06 });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[0])));
                                break;
                            case "slEditTone_PCMSynthTone_MFXControl_MFXControlSense1":
                                commonMFX.MFXControlSens[1] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_MFXControl_MFXControlSens[1].Text = "MFX Control 2 Sense: " + ((commonMFX.MFXControlSens[1] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x08 });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[1])));
                                break;
                            case "slEditTone_PCMSynthTone_MFXControl_MFXControlSense2":
                                commonMFX.MFXControlSens[2] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_MFXControl_MFXControlSens[2].Text = "MFX Control 3 Sense: " + ((commonMFX.MFXControlSens[2] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0a });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[2])));
                                break;
                            case "slEditTone_PCMSynthTone_MFXControl_MFXControlSense3":
                                commonMFX.MFXControlSens[3] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMSynthTone_MFXControl_MFXControlSens[3].Text = "MFX Control 4 Sense: " + ((commonMFX.MFXControlSens[3] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0c });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[3])));
                                break;
                            case "cbEditTone_pCMSynthTone_MFXControl_MFXControlAssign0":
                                commonMFX.MFXControlAssign[0] = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0d });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[0]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_pCMSynthTone_MFXControl_MFXControlAssign1":
                                commonMFX.MFXControlAssign[1] = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0e });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[1]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_pCMSynthTone_MFXControl_MFXControlAssign2":
                                commonMFX.MFXControlAssign[2] = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0f });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[2]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_pCMSynthTone_MFXControl_MFXControlAssign3":
                                commonMFX.MFXControlAssign[3] = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x10 });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[3]) };
                                SendParameter(address, value);
                                break;
                            default:
                                t.Trace("Missing case for \"" + name + "\"!");
                                break;
                        }
                        break;
                    // PCM Drum Kit ************************************************************************************
                    case ProgramType.PCM_DRUM_KIT:
                        switch (name)
                        {
                            // PCM Drum Kit Common *****
                            case "cbEditTone_PCMDrumKit_Common_PhraseNumber":
                                pCMDrumKit.pCMDrumKitCommon2.PhraseNumber = (byte)comboBox.SelectedIndex;
                                byte[] address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMON2, new byte[] { 0x10 });
                                byte[] value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommon2.PhraseNumber / 16), (byte)(pCMDrumKit.pCMDrumKitCommon2.PhraseNumber % 16) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_Common_DrumKitLevel":
                                pCMDrumKit.pCMDrumKitCommon.DrumKitLevel = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Common_DrumKitLevel.Text = "Drum Kit Level: " + ((pCMDrumKit.pCMDrumKitCommon.DrumKitLevel)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMON, new byte[] { 0x0c });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommon.DrumKitLevel)));
                                break;
                            case "cbEditTone_PCMDrumKit_Common_AssignType":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].AssignType = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0c });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].AssignType) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Common_MuteGroup":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].MuteGroup = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0d });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].MuteGroup) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Common_PartialEnvMode":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialEnvMode = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x15 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialEnvMode) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_Common_PartialPitchBendRange":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialPitchBendRange = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Common_PartialPitchBendRange.Text = "Partial Pitch Bend Range: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialPitchBendRange)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x1c });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialPitchBendRange)));
                                break;
                            case "cbEditTone_PCMDrumKit_PartialReceiveExpression":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x1d });
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialReceiveExpression = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_PartialReceiveHold_1":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x1e });
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialReceiveHold_1 = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_OneShotMode":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x41 });
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].OneShotMode = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            // Partial switches (used in pages Wave and WMT): *****
                            case "cbEditTone_PCMDrumKit_Partial1Switch":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x21 }, false);
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[0].WMTWaveSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_Partial2Switch":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x3e }, false);
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[1].WMTWaveSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_Partial3Switch":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x5b }, false);
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[2].WMTWaveSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_Partial4Switch":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x78 }, false);
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[3].WMTWaveSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            // PCM Drum Kit Wave tab *****
                            case "cbEditTone_PCMDrumKit_Wave_WMTWaveGroupType":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveGroupType = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x22 }, true);
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveGroupType) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Wave_WMTWaveNumberL":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveNumberL = (UInt16)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x27 }, true);
                                SendParameter(address, (UInt16)(pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveNumberL));
                                DrumKit_WaveOn(0);
                                // Save the key name:
                                commonState.keyNames[currentPartial] = ((String)comboBox.SelectedItem);
                                commonState.keyNames[currentPartial] = commonState.keyNames[currentPartial].Remove(0,
                                    commonState.keyNames[currentPartial].IndexOf(':') + 1).Trim();
                                break;
                            case "cbEditTone_PCMDrumKit_Wave_WMTWaveNumberR":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveNumberR = (UInt16)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x2b }, true);
                                SendParameter(address, (UInt16)(pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveNumberR));
                                DrumKit_WaveOn(1);
                                break;
                            case "cbEditTone_PCMDrumKit_Wave_WMTWaveGain":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveGain = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x2f }, true);
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveGain) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_WMTWaveTempoSync":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x33 }, true);
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveTempoSync = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_WMTWaveFXMSwitch":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x30 }, true);
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveFXMSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_Wave_WMTWaveFXMColor":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveFXMColor = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x31 }, true);
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveFXMColor) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveFXMDepth = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Wave_WMTWaveFXMDepth.Text = "WMT Wave MFX Depth: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveFXMDepth)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x32 }, true);
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveFXMDepth)));
                                break;
                            case "slEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveCoarseTune = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_Wave_WMTWaveCoarseTune.Text = "WMT Wave Coarse Tune: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveCoarseTune - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x34 }, true);
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveCoarseTune)));
                                break;
                            case "slEditTone_PCMDrumKit_Wave_WMTWaveFineTune":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveFineTune = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_Wave_WMTWaveFineTune.Text = "WMT Wave Fine Tune: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveFineTune - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x35 }, true);
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveFineTune)));
                                break;
                            case "slEditTone_PCMDrumKit_Wave_WMTWaveLevel":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveLevel = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Wave_WMTWaveLevel.Text = "WMT Wave Level: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveLevel)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x39 }, true);
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveLevel)));
                                break;
                            case "slEditTone_PCMDrumKit_Wave_WMTWavePan":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWavePan = (byte)((slider.Value + 64));
                                if (slider.Value > 0)
                                {
                                    tbEditTone_PCMDrumKit_Wave_WMTWavePan.Text = "WMT Wave Pan: R" + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWavePan - 64)).ToString();
                                }
                                else if (slider.Value < 0)
                                {
                                    tbEditTone_PCMDrumKit_Wave_WMTWavePan.Text = "WMT Wave Pan: L" + (0 - (pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWavePan - 64)).ToString();
                                }
                                else
                                {
                                    tbEditTone_PCMDrumKit_Wave_WMTWavePan.Text = "WMT Wave Pan: 0";
                                }
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x36 }, true);
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWavePan)));
                                break;
                            case "cbEditTone_PCMDrumKit_WMTWaveRandomPanSw":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x37 }, true);
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveRandomPanSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_Wave_WMTWaveAlterPanSwitch":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveAlternatePanSwitch = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x38 }, true);
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTWaveAlternatePanSwitch) };
                                SendParameter(address, value);
                                break;
                            // PCM Drum Kit WMT tab
                            case "cbEditTone_PCMDrumKit_WMT_WMTVelocityControl":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMTVelocityControl = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x20 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].WMTVelocityControl) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTVelocityFadeWidthUpper = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthUpper.Text = "Velocity Fade Upper: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTVelocityFadeWidthUpper)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x3d }, true);
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTVelocityFadeWidthUpper)));
                                break;
                            case "slEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTVelocityRangeUpper = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_WMT_WMTVelocityRangeUpper.Text = "Velocity Range Upper: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTVelocityRangeUpper)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x3b }, true);
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTVelocityRangeUpper)));
                                break;
                            case "slEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTVelocityRangeLower = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_WMT_WMTVelocityRangeLower.Text = "Velocity Range Lower: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTVelocityRangeLower)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x3a }, true);
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTVelocityRangeLower)));
                                break;
                            case "slEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTVelocityFadeWidthLower = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_WMT_WMTVelocityFadeWidthLower.Text = "WMT Velocity Fade Width Lower: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTVelocityFadeWidthLower)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x3c }, true);
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].WMT[currentPartial].WMTVelocityFadeWidthLower)));
                                break;
                            // PMC Drum Kit Pitch tab *****
                            case "slEditTone_PCMDrumKit_Pitch_PartialCoarseTune":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialCoarseTune = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Pitch_PartialCoarseTune.Text = "Partial Coarse Tune: " + keyNames[pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialCoarseTune];
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x0f });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialCoarseTune)));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PartialFineTune":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialFineTune = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_Pitch_PartialFineTune.Text = "Partial Fine Tune: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialFineTune - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x10 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialFineTune)));
                                break;
                            case "cbEditTone_PCMDrumKit_Pitch_PartialRandomPitchDepth":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialRandomPitchDepth = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x11 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialRandomPitchDepth) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvDepth":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvDepth = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvDepth.Text = "Pitch Env Depth: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x15 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvDepth)));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvVelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvVelocitySens.Text = "Pitch Env Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvVelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x16 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvVelocitySens)));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime1VelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvTime1VelocitySens.Text = "Pitch Env Time 1 Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime1VelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x17 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime1VelocitySens)));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime4VelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvTime4VelocitySens.Text = "Pitch Env Time 4 Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime4VelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x18 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime4VelocitySens)));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvTime0":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime[0] = (byte)((slider.Value / 1));
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvTime[0].Text = "Pitch Env Time 1: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime[0])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x19 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime[0])));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvTime1":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime[1] = (byte)((slider.Value / 1));
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvTime[1].Text = "Pitch Env Time 2: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime[1])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x1a });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime[1])));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvTime2":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime[2] = (byte)((slider.Value / 1));
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvTime[2].Text = "Pitch Env Time 3: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime[2])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x1b });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime[2])));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvTime3":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime[3] = (byte)((slider.Value / 1));
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvTime[3].Text = "Pitch Env Time 4: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime[3])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x1c });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvTime[3])));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvLevel0":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[0] = (byte)((slider.Value / 1) + 64);
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvLevel[0].Text = "Pitch Env Level 0: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[0] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x1d });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[0])));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvLevel1":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[1] = (byte)((slider.Value / 1) + 64);
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvLevel[1].Text = "Pitch Env Level 1: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[1] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x1e });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[1])));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvLevel2":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[2] = (byte)((slider.Value / 1) + 64);
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvLevel[2].Text = "Pitch Env Level 2: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[2] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x1f });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[2])));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvLevel3":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[3] = (byte)((slider.Value / 1) + 64);
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvLevel[3].Text = "Pitch Env Level 3: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[3] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x20 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[3])));
                                break;
                            case "slEditTone_PCMDrumKit_Pitch_PitchEnvLevel4":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[4] = (byte)((slider.Value / 1) + 64);
                                tbEditTone_PCMDrumKit_Pitch_PitchEnvLevel[4].Text = "Pitch Env Level 4: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[4] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x21 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PitchEnv.PitchEnvLevel[4])));
                                break;
                            // PCM Drum Kit TVF controls
                            case "cbEditTone_PCMDrumKit_TVF_TVFFilterType":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFFilterType = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x22 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFFilterType) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_TVF_TVFCutoffFrequency":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFCutoffFrequency = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVF_TVFCutoffFrequency.Text = "TVF Cutoff Frequency: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFCutoffFrequency)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x23 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFCutoffFrequency)));
                                break;
                            case "slEditTone_PCMDrumKit_TVF_TVFResonance":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFResonance = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVF_TVFResonance.Text = "TVF Resonance: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFResonance)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x26 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFResonance)));
                                break;
                            case "cbEditTone_PCMDrumKit_TVF_TVFCutoffVelocityCurve":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFCutoffVelocityCurve = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x24 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFCutoffVelocityCurve) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFCutoffVelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_TVF_TVFCutoffVelocitySens.Text = "TVF Cutoff Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFCutoffVelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x25 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFCutoffVelocitySens)));
                                break;
                            case "slEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFResonanceVelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_TVF_TVFResonanceVelocitySens.Text = "TVF Resonance Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFResonanceVelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x27 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFResonanceVelocitySens)));
                                break;
                            case "cbEditTone_PCMDrumKit_TVF_TVFEnvVelocityCurveType":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvVelocityCurve = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x29 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvVelocityCurve) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvVelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_TVF_TVFEnvVelocitySens.Text = "TVF Env Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvVelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x2a });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvVelocitySens)));
                                break;
                            case "slEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime1VelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_TVF_TVFEnvTime1VelocitySens.Text = "TVF Env Time 1 Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime1VelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x2b });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime1VelocitySens)));
                                break;
                            case "slEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime4VelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_TVF_TVFEnvTime4VelocitySens.Text = "TVF Env Time 4 Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime4VelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x2c });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime4VelocitySens)));
                                break;
                            // PCM Drum Kit TVF ENV tab
                            case "slEditTone_PCMDrumKit_TVFenv_TVFEnvDepth":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvDepth = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_TVFenv_TVFEnvDepth.Text = "TVF Env Depth: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x28 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvDepth)));
                                break;
                            case "slEditTone_PCMDrumKit_TVFenv_TVFEnvTime0":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime[0] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVFenv_TVFEnvTime[0].Text = "TVF Env Time 1: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime[0])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x2d });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime[0])));
                                break;
                            case "slEditTone_PCMDrumKit_TVFenv_TVFEnvTime1":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime[1] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVFenv_TVFEnvTime[1].Text = "TVF Env Time 2: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime[1])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x2e });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime[1])));
                                break;
                            case "slEditTone_PCMDrumKit_TVFenv_TVFEnvTime2":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime[2] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVFenv_TVFEnvTime[2].Text = "TVF Env Time 3: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime[2])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x2f });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime[2])));
                                break;
                            case "slEditTone_PCMDrumKit_TVFenv_TVFEnvTime3":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime[3] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVFenv_TVFEnvTime[3].Text = "TVF Env Time 4: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime[3])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x30 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvTime[3])));
                                break;
                            case "slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel0":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[0] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[0].Text = "TVF Env Level 0: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[0])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x31 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[0])));
                                break;
                            case "slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel1":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[1] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[1].Text = "TVF Env Level 1: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[1])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x32 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[1])));
                                break;
                            case "slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel2":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[2] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[2].Text = "TVF Env Level 2: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[2])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x33 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[2])));
                                break;
                            case "slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel3":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[3] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[3].Text = "TVF Env Level 3: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[3])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x34 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[3])));
                                break;
                            case "slEditTone_PCMDrumKit_TVFenv_TVFEnvLevel4":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[4] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVFenv_TVFEnvLevel[4].Text = "TVF Env Level 4: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[4])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x35 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVF.TVFEnvLevel[4])));
                                break;
                            // PCM Drum Kit TVA tab
                            case "slEditTone_PCMDrumKit_TVA_PartialLevel":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialLevel = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVA_PartialLevel.Text = "Partial Level: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialLevel)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0e });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialLevel)));
                                break;
                            case "cbEditTone_PCMDrumKit_TVA_TVALevelVelocityCurve":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVALevelVelocityCurve = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x36 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVALevelVelocityCurve) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_TVA_TVALevelVelocitySens":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVALevelVelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_TVA_TVALevelVelocitySens.Text = "TVA Level Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVALevelVelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x37 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVALevelVelocitySens)));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_PartialPan":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialPan = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_TVA_PartialPan.Text = "Partial Pan: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialPan - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x12 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialPan)));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_PartialRandomPanDepth":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialRandomPanDepth = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVA_PartialRandomPanDepth.Text = "Partial Random Pan Depth: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialRandomPanDepth)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x13 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialRandomPanDepth)));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialAlternatePanDepth = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_TVA_PartialAlternatePanDepth.Text = "Partial Alternate Pan Depth: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialAlternatePanDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x14 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialAlternatePanDepth)));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_RelativeLevel":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].RelativeLevel = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_TVA_RelativeLevel.Text = "Relative Level: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].RelativeLevel - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x42 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].RelativeLevel)));
                                break;
                            // PCM Drum Kit TVA ENV tab
                            case "slEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime1VelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_TVA_TVAEnvTime1VelocitySens.Text = "TVA Env Time 1 Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime1VelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x38 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime1VelocitySens)));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_TVAEnvTime2VelocitySens":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime4VelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_TVA_TVAEnvTime4VelocitySens.Text = "TVA Env Time 2 Velocity Sens: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime4VelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x39 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime4VelocitySens)));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_TVAEnvTime0":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime[0] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVA_TVAEnvTime[0].Text = "TVA Env Time 1: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime[0])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x3a });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime[0])));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_TVAEnvTime1":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime[1] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVA_TVAEnvTime[1].Text = "TVA Env Time 2: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime[1])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x3b });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime[1])));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_TVAEnvTime2":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime[2] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVA_TVAEnvTime[2].Text = "TVA Env Time 3: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime[2])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x3c });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime[2])));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_TVAEnvTime3":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime[3] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVA_TVAEnvTime[3].Text = "TVA Env Time 4: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime[3])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x3d });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvTime[3])));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_TVAEnvLevel0":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvLevel[0] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVA_TVAEnvLevel[0].Text = "TVA Env Level 1: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvLevel[0])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x3e });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvLevel[0])));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_TVAEnvLevel1":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvLevel[1] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVA_TVAEnvLevel[1].Text = "TVA Env Level 2: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvLevel[1])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x3f });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvLevel[1])));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_TVAEnvLevel2":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvLevel[2] = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVA_TVAEnvLevel[2].Text = "TVA Env Level 3: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvLevel[2])).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x40 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].TVA.TVAEnvLevel[2])));
                                break;
                            // PCM Drum Kit Output tab
                            case "cbEditTone_PCMDrumKit_TVA_PartialOutputAssign":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialOutputAssign = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x1b });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialOutputAssign) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_TVA_PartialOutputLevel":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialOutputLevel = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVA_PartialOutputLevel.Text = "Partial Output Level: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialOutputLevel)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x16 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialOutputLevel)));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_PartialChorusSendLevel":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialChorusSendLevel = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVA_PartialChorusSendLevel.Text = "Partial Chorus Send Level: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialChorusSendLevel)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x19 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialChorusSendLevel)));
                                break;
                            case "slEditTone_PCMDrumKit_TVA_PartialReverbSendLevel":
                                pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialReverbSendLevel = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_TVA_PartialReverbSendLevel.Text = "Partial Reverb Send Level: " + ((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialReverbSendLevel)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x1a });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitPartial[currentPartial].PartialReverbSendLevel)));
                                break;
                            // PCM Drum Kit Common CompEq
                            case "cbEditTone_PCMDrumKit_CompSwitch0":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x00 });
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_CompSwitch1":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0e });
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_CompSwitch2":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1c });
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_CompSwitch3":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2a });
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_CompSwitch4":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x38 });
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_CompSwitch5":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x46 });
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;

                            case "cbEditTone_PCMDrumKit_Compressor_CompAttackTime0":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompAttackTime = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x01 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompAttackTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompAttackTime1":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompAttackTime = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0f });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompAttackTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompAttackTime2":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompAttackTime = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0d });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompAttackTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompAttackTime3":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompAttackTime = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2b });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompAttackTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompAttackTime4":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompAttackTime = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x39 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompAttackTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompAttackTime5":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompAttackTime = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x47 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompAttackTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompReleaseTime0":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompReleaseTime = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x02 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompReleaseTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompReleaseTime1":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompReleaseTime = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x10 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompReleaseTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompReleaseTime2":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompReleaseTime = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1e });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompReleaseTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompReleaseTime3":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompReleaseTime = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2c });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompReleaseTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompReleaseTime4":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompReleaseTime = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3a });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompReleaseTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompReleaseTime5":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompReleaseTime = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x48 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompReleaseTime) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_Compressor_CompThreshold0":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompThreshold = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Compressor_CompThreshold[0].Text = "Thresh: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompThreshold)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x03 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompThreshold)));
                                break;
                            case "slEditTone_PCMDrumKit_Compressor_CompThreshold1":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompThreshold = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Compressor_CompThreshold[1].Text = "Thresh: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompThreshold)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x11 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompThreshold)));
                                break;
                            case "slEditTone_PCMDrumKit_Compressor_CompThreshold2":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompThreshold = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Compressor_CompThreshold[2].Text = "Thresh: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompThreshold)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1f });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompThreshold)));
                                break;
                            case "slEditTone_PCMDrumKit_Compressor_CompThreshold3":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompThreshold = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Compressor_CompThreshold[3].Text = "Thresh: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompThreshold)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2d });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompThreshold)));
                                break;
                            case "slEditTone_PCMDrumKit_Compressor_CompThreshold4":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompThreshold = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Compressor_CompThreshold[4].Text = "Thresh: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompThreshold)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3b });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompThreshold)));
                                break;
                            case "slEditTone_PCMDrumKit_Compressor_CompThreshold5":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompThreshold = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Compressor_CompThreshold[5].Text = "Thresh: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompThreshold)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4a });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompThreshold)));
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompRatio0":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompRatio = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x04 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompRatio) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompRatio1":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompRatio = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x12 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompRatio) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompRatio2":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompRatio = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x20 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompRatio) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompRatio3":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompRatio = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2e });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompRatio) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompRatio4":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompRatio = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3c });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompRatio) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Compressor_CompRatio5":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompRatio = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4a });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompRatio) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_Compressor_CompOutputGain0":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompOutputGain = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Compressor_CompOutputGain[0].Text = "Gain: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompOutputGain)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x05 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompOutputGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Compressor_CompOutputGain1":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompOutputGain = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Compressor_CompOutputGain[1].Text = "Gain: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompOutputGain)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x13 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompOutputGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Compressor_CompOutputGain2":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompOutputGain = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Compressor_CompOutputGain[2].Text = "Gain: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompOutputGain)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x21 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompOutputGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Compressor_CompOutputGain3":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompOutputGain = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Compressor_CompOutputGain[3].Text = "Gain: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompOutputGain)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2f });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompOutputGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Compressor_CompOutputGain4":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompOutputGain = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Compressor_CompOutputGain[4].Text = "Gain: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompOutputGain)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3d });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompOutputGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Compressor_CompOutputGain5":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompOutputGain = (byte)((slider.Value));
                                tbEditTone_PCMDrumKit_Compressor_CompOutputGain[5].Text = "Gain: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompOutputGain)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4b });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompOutputGain)));
                                break;
                            // PCM Drum Kit Eq tab
                            case "cbEditTone_PCMDrumKit_EQSwitch0":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x06 });
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_EQSwitch1":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x14 });
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_EQSwitch2":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x22 });
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_EQSwitch3":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x30 });
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_EQSwitch4":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3e });
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_EQSwitch5":
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4c });
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQLowFreq0":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQLowFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x07 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQLowFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQLowFreq1":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQLowFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x15 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQLowFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQLowFreq2":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQLowFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x23 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQLowFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQLowFreq3":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQLowFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x31 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQLowFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQLowFreq4":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQLowFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3f });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQLowFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQLowFreq5":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQLowFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4d });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQLowFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQMidFreq0":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQMidFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x09 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQMidFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQMidFreq1":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQMidFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x17 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQMidFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQMidFreq2":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQMidFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x25 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQMidFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQMidFreq3":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQMidFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x33 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQMidFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQMidFreq4":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQMidFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x41 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQMidFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQMidFreq5":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQMidFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4f });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQMidFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQMidQ0":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQMidQ = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0b });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQMidQ) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQMidQ1":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQMidQ = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x19 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQMidQ) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQMidQ2":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQMidQ = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x27 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQMidQ) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQMidQ3":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQMidQ = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x35 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQMidQ) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQMidQ4":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQMidQ = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x43 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQMidQ) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQMidQ5":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQMidQ = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x51 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQMidQ) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQHighFreq0":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQHighFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0c });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQHighFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQHighFreq1":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQHighFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1a });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQHighFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQHighFreq2":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQHighFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x28 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQHighFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQHighFreq3":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQHighFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x36 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQHighFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQHighFreq4":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQHighFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x44 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQHighFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_Equalizer_EQHighFreq5":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQHighFreq = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x52 });
                                value = new byte[] { (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQHighFreq) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQLowGain0":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQLowGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQLowGain[0].Text = "Low 1: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQLowGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x08 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQLowGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQLowGain1":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQLowGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQLowGain[1].Text = "Low 2: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQLowGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x16 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQLowGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQLowGain2":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQLowGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQLowGain[2].Text = "Low 3: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQLowGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x24 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQLowGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQLowGain3":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQLowGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQLowGain[3].Text = "Low 4: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQLowGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x32 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQLowGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQLowGain4":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQLowGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQLowGain[4].Text = "Low 5: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQLowGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x40 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQLowGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQLowGain5":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQLowGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQLowGain[5].Text = "Low 6: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQLowGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1e });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQLowGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQMidGain0":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQMidGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQMidGain[0].Text = "Mid 1: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQMidGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0a });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQMidGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQMidGain1":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQMidGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQMidGain[1].Text = "Mid 2: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQMidGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x18 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQMidGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQMidGain2":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQMidGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQMidGain[2].Text = "Mid 3: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQMidGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x26 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQMidGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQMidGain3":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQMidGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQMidGain[3].Text = "Mid 4: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQMidGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x34 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQMidGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQMidGain4":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQMidGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQMidGain[4].Text = "Mid 5: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQMidGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x42 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQMidGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQMidGain5":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQMidGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQMidGain[5].Text = "Mid 6: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQMidGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x50 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQMidGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQHighGain0":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQHighGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQHighGain[0].Text = "High 1: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQHighGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0d });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQHighGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQHighGain1":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQHighGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQHighGain[1].Text = "High 2: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQHighGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1b });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQHighGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQHighGain2":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQHighGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQHighGain[2].Text = "High 3: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQHighGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x29 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQHighGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQHighGain3":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQHighGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQHighGain[3].Text = "High 4: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQHighGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x37 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQHighGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQHighGain4":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQHighGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQHighGain[4].Text = "High 5: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQHighGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x45 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQHighGain)));
                                break;
                            case "slEditTone_PCMDrumKit_Equalizer_EQHighGain5":
                                pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQHighGain = (byte)((slider.Value) + 15);
                                tbEditTone_PCMDrumKit_Equalizer_EQHighGain[5].Text = "High 6: " + ((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQHighGain - 15)).ToString() + " dB";
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x53 });
                                SendParameter(address, (byte)((pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQHighGain)));
                                break;
                            // PCM Drum Kit MFX *****
                            case "cbEditTone_MFXType":
                                HandleCommonMFX(comboBox);
                                break;
                            // PCM Drum Kit MFX control *****
                            case "cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource0":
                                commonMFX.MFXControlSource[0] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x05 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[0]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource1":
                                commonMFX.MFXControlSource[1] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x07 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource)[1] };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource2":
                                commonMFX.MFXControlSource[2] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x09 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource)[2] };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_MFXcontrol_MFXControlSource3":
                                commonMFX.MFXControlSource[3] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0b });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource)[3] };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign0":
                                commonMFX.MFXControlAssign[0] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0d });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[0]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign1":
                                commonMFX.MFXControlAssign[1] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0e });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[1]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign2":
                                commonMFX.MFXControlAssign[2] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0f });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[2]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_PCMDrumKit_MFXcontrol_MFXControlAssign3":
                                commonMFX.MFXControlAssign[3] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x10 });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[3]) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens0":
                                commonMFX.MFXControlSens[0] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[0].Text = "MFX Control Sens: " + ((commonMFX.MFXControlSens[0] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x06 });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[0])));
                                break;
                            case "slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens1":
                                commonMFX.MFXControlSens[1] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[1].Text = "MFX Control Sens: " + ((commonMFX.MFXControlSens[1] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x08 });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[1])));
                                break;
                            case "slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens2":
                                commonMFX.MFXControlSens[2] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[2].Text = "MFX Control Sens: " + ((commonMFX.MFXControlSens[2] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0a });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[2])));
                                break;
                            case "slEditTone_PCMDrumKit_MFXcontrol_MFXControlSens3":
                                commonMFX.MFXControlSens[3] = (byte)((slider.Value) + 64);
                                tbEditTone_PCMDrumKit_MFXcontrol_MFXControlSens[3].Text = "MFX Control Sens: " + ((commonMFX.MFXControlSens[3] - 64)).ToString();
                                address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0c });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[3])));
                                break;
                            default:
                                t.Trace("Missing case for \"" + name + "\"!");
                                break;
                        }
                        break;
                    // SuperNATURAL Acoustic Tone **********************************************************************
                    case ProgramType.SUPERNATURAL_ACOUSTIC_TONE:
                        // SuperNATURAL Acoustic Tone Common tab
                        switch (name)
                        {
                            case "cbEditTone_SuperNATURALAcousticTone_Common_Category":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.Category = (byte)comboBox.SelectedIndex;
                                byte[] address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x1b });
                                byte[] value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.Category) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Common_PhraseNumber":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.PhraseNumber = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x1c });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.PhraseNumber / 16),
                                (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.PhraseNumber % 16) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Common_PhraseOctaveShift":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.PhraseOctaveShift = (byte)(comboBox.SelectedIndex + 61);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x1e });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.PhraseOctaveShift) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Common_ToneLevel":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ToneLevel = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Common_ToneLevel.Text = "Tone Level: " +
                                        ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ToneLevel)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x10 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ToneLevel)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Common_MonoPoly":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.MonoPoly = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x11 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.MonoPoly) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Common_OctaveShift":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.OctaveShift = (byte)(comboBox.SelectedIndex + 61);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x1a });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.OctaveShift) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Common_CutoffOffset":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.CutoffOffset = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Common_CutoffOffset.Text = "Cutoff Offset: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.CutoffOffset - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x13 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.CutoffOffset)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ResonanceOffset = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Common_ResonanceOffset.Text = "Resonance Offset: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ResonanceOffset - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x14 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ResonanceOffset)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.AttackTimeOffset = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Common_AttackTimeOffset.Text = "Attack Time Offset: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.AttackTimeOffset - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x15 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.AttackTimeOffset)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ReleaseTimeOffset = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Common_ReleaseTimeOffset.Text = "Release Time Offset: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ReleaseTimeOffset - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x16 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ReleaseTimeOffset)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.PortamentoTimeOffset = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Common_PortamentoTimeOffset.Text = "Portamento Time Offset: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.PortamentoTimeOffset - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x12 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.PortamentoTimeOffset)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Common_VibratoRate":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoRate = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Common_VibratoRate.Text = "Vibrato Rate: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoRate - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x17 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoRate)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Common_VibratoDepth":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoDepth = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Common_VibratoDepth.Text = "Vibrato Depth: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x18 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoDepth)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Common_VibratoDelay":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoDelay = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Common_VibratoDelay.Text = "Vibrato Delay: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoDelay - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x19 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoDelay)));
                                break;
                            // SuperNATURAL Acoustic Tone Instrument tab
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_Bank":
                                currentHandleControlEvents = handleControlEvents;
                                handleControlEvents = false;
                                List<Instrument> instruments = superNaturalAcousticInstrumentList.ListInstruments((String)cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.SelectedItem);
                                try
                                {
                                    cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.Items.Clear();
                                } catch { }
                                Int32 index = 0;
                                foreach (Instrument instrument in superNaturalAcousticInstrumentList.Tones)
                                {
                                    if (instrument.InstrumentBank == cbEditTone_SuperNATURALAcousticTone_Instrument_Bank.SelectedItem.ToString())
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
                                handleControlEvents = currentHandleControlEvents;
                                if (cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.Items.Count > index)
                                {
                                    cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.SelectedIndex = index;
                                }
                                else if (cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.Items.Count() > 0)
                                {
                                    cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.SelectedIndex = 0;
                                }
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber":
                                // We cannot handle this here, because changing instrument number might also affect tone type.
                                // Just call AddSupernaturalAcousticToneInstrumentParametersControls() which in turm finds out
                                // what tone type it is, and sends tone type together with tone number.
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x20 });
                                SendParameter(address, InstrumentSettings.Tone[23][cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.SelectedIndex]);
                                //SendParameter(address, InstrumentSettings.Tone[commonState.currentTone.GroupIndex][cbEditTone_SuperNATURALAcousticTone_Instrument_InstNumber.SelectedIndex]);
                                AddSupernaturalAcousticToneInstrumentParametersControls();
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_StringResonance":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_StringResonance.Text = "String Resonance: " +
                                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x22 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOffResonance.Text = "Key Off Resonance: " +
                                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_HammerNoise":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3 = (byte)(comboBox.SelectedIndex + 62);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x24 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_StereoWidth.Text = "Stereo Width: " +
                                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x25 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_Nuance":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5 = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x26 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_ToneCharacter":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6 = (byte)(comboBox.SelectedIndex + 59);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x27 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_NoiseLevelCC16.Text = "Noise Level (CC16): " +
                                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x22 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar16":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x22 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar513":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar8":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x24 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar4":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x25 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar223":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x26 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar2":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x27 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar135":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter7 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x28 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter7) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar113":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter8 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x29 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter8) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_HarmonicBar1":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter9 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2a });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter9) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter22 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_LeakageLevel.Text = "Leakage Level: " +
                                    ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter22)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x37 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter22)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_PercussionSwitch":
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2b });
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter10 = (byte)((Boolean)checkBox.IsChecked ? 1 : 0);
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoft":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter21 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x36 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter21) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter15 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSoftLevel.Text = "Percussion Soft Level: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x30 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter15)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter16 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionNormalLevel.Text = "Percussion Normal Level: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter16)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x31 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter16)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlow":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter12 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2d });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter12) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter17 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionSlowTime.Text = "Percussion Slow Time: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter17)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x32 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter17)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter18 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionFastTime.Text = "Percussion Fast Time: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter18)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x33 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter18)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonic":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter11 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2c });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter11) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter19 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionRechargeTime.Text = "Percussion Recharge Time: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter19)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x34 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter19)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter20 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_PercussionHarmonicBarLevel.Text = "Percussion Harmonic Bar Level: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter20)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x35 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter20)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter13 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOnClickLevel.Text = "Key On Click Level: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter13)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2e });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter13)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter14 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_KeyOffClickLevel.Text = "Key Off Click Level: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter14)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2f });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter14)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3 = (byte)((slider.Value));
                                tbEditTone_SuperNATURALAcousticTone_Instrument_GrowlSensCC18.Text = "Growl Sens (CC18): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x24 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_MalletHardnessCC16.Text = "Mallet Hardness (CC16): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x22 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_RollSpeedCC17.Text = "Roll Speed (CC17): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_InstVariation":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter10 = comboBox.SelectedIndex == 0 ? (byte)(comboBox.SelectedIndex) :
                                    (byte)(comboBox.SelectedIndex);// + superNATURALAcousticToneVariation.ComboBoxOffset);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2b });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter10) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_StrumSpeedCC17.Text = "Strum Speed (CC17): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_StrumModeCC19":
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x25 });
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4 = (byte)((Boolean)checkBox.IsChecked ? 1 : 0);
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter13 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_SubStringTune.Text = "Sub String Tune: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter13 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2e });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter13)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_CrescendoDepthCC17.Text = "Crescendo Depth (CC17): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_TremoloSpeedCC17.Text = "Tremolo Speed (CC17): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_PickingHarmonics":
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2c });
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter11 = (byte)((Boolean)checkBox.IsChecked ? 1 : 0);
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_ResonanceLevelCC16.Text = "Resonance Level (CC16): " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x22 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_GlissandoModeCC19":
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x25 });
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4 = (byte)((Boolean)checkBox.IsChecked ? 1 : 0);
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_PlayScale":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter7 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x28 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter7) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter8 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x29 });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter8) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_ScaleKey":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_BendDepth.Text = "Scale Key: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_BuzzKeySwitch":
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2D });
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter12 = (byte)((Boolean)checkBox.IsChecked ? 1 : 0);
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_TamburaLevel.Text = "Tambura lEVEL: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x26 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_TamburaPitch.Text = "Tambura Pitch: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x27 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_HoldLegatoModeCC19":
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x25 });
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4 = (byte)((Boolean)checkBox.IsChecked ? 1 : 0);
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_DroneLevel.Text = "Drone Level: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x26 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5)));
                                break;
                            case "slEditTone_SuperNATURALAcousticTone_Instrument_DronePitch":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6 = (byte)((slider.Value) + 64);
                                tbEditTone_SuperNATURALAcousticTone_Instrument_DronePitch.Text = "Drone Pitch: " + ((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6 - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x27 });
                                SendParameter(address, (byte)((superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6)));
                                break;
                            case "cbEditTone_SuperNATURALAcousticTone_Instrument_Glide":
                                superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter9 = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2a });
                                value = new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter9) };
                                SendParameter(address, value);
                                break;
                            // SuperNATURAL Acoustic Tone MFX *****
                            case "cbEditTone_MFXType":
                                HandleCommonMFX(comboBox);
                                break;
                            // SuperNATURAL Acoustic Tone MFX control *****
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlSource0":
                                commonMFX.MFXControlSource[0] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x05 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[0]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlSource1":
                                commonMFX.MFXControlSource[1] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x07 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[1]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlSource2":
                                commonMFX.MFXControlSource[2] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x09 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[2]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlSource3":
                                commonMFX.MFXControlSource[3] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0b });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[3]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControlDestination0":
                                commonMFX.MFXControlAssign[0] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0d });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[0]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControlDestination1":
                                commonMFX.MFXControlAssign[1] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0e });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[1]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControlDestination2":
                                commonMFX.MFXControlAssign[2] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0f });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[2]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControlDestination3":
                                commonMFX.MFXControlAssign[3] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x10 });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[3]) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_CommonMFXControl_MFXControlSense0":
                                commonMFX.MFXControlSens[0] = (byte)((slider.Value) + 64);
                                tbEditTone_CommonMFX_Control_MFXControlSens[0].Text = "MFX Control 1 Sense: " + ((commonMFX.MFXControlSens[0] - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x06 });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[0])));
                                break;
                            case "slEditTone_CommonMFXControl_MFXControlSense1":
                                commonMFX.MFXControlSens[1] = (byte)((slider.Value) + 64);
                                tbEditTone_CommonMFX_Control_MFXControlSens[1].Text = "MFX Control 2 Sense: " + ((commonMFX.MFXControlSens[1] - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x08 });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[1])));
                                break;
                            case "slEditTone_CommonMFXControl_MFXControlSense2":
                                commonMFX.MFXControlSens[2] = (byte)((slider.Value) + 64);
                                tbEditTone_CommonMFX_Control_MFXControlSens[2].Text = "MFX Control 3 Sense: " + ((commonMFX.MFXControlSens[2] - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0a });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[2])));
                                break;
                            case "slEditTone_CommonMFXControl_MFXControlSense3":
                                commonMFX.MFXControlSens[3] = (byte)((slider.Value) + 64);
                                tbEditTone_CommonMFX_Control_MFXControlSens[3].Text = "MFX Control 4 Sense: " + ((commonMFX.MFXControlSens[3] - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0c });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[3])));
                                break;
                            default:
                                t.Trace("Missing case for \"" + name + "\"!");
                                break;
                        }
                        break;
                    // SuperNATURAL Synth Tone *************************************************************************
                    case ProgramType.SUPERNATURAL_SYNTH_TONE:
                        switch (name)
                        {
                            // SuperNATURAL Synth Tone Common ***
                            case "cbEditTone_SuperNATURALSynthTone_Common_PhraseNumber":
                                superNATURALSynthTone.superNATURALSynthToneCommon.PhraseNumber = (byte)(comboBox.SelectedIndex);
                                byte[] address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x37 });
                                byte[] value = new byte[] { 0x00, 0x00, (byte)(superNATURALSynthTone.superNATURALSynthToneCommon.PhraseNumber / 16),
                                (byte)(superNATURALSynthTone.superNATURALSynthToneCommon.PhraseNumber % 16)};
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALSynthTone_Common_PhraseOctaveShift":
                                superNATURALSynthTone.superNATURALSynthToneCommon.PhraseOctaveShift = (byte)(comboBox.SelectedIndex + 61);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x3b });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthToneCommon.PhraseOctaveShift) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_superNATURALSynthTone_Common_ToneLevel":
                                superNATURALSynthTone.superNATURALSynthToneCommon.ToneLevel = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Common_ToneLevel.Text = "Tone Level: " +
                                    ((superNATURALSynthTone.superNATURALSynthToneCommon.ToneLevel)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x0c });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthToneCommon.ToneLevel)));
                                break;
                            case "cbEditTone_superNATURALSynthTone_RINGSwitch":
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x1f });
                                superNATURALSynthTone.superNATURALSynthToneCommon.RINGSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 2 : 0));
                                break;
                            case "slEditTone_superNATURALSynthTone_Common_WaveShape":
                                superNATURALSynthTone.superNATURALSynthToneCommon.WaveShape = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Common_WaveShape.Text = "Wave Shape: " +
                                    ((superNATURALSynthTone.superNATURALSynthToneCommon.WaveShape)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x35 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthToneCommon.WaveShape)));
                                break;
                            case "slEditTone_superNATURALSynthTone_Common_AnalogFeel":
                                superNATURALSynthTone.superNATURALSynthToneCommon.AnalogFeel = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Common_AnalogFeel.Text = "Analog Feel: " +
                                    ((superNATURALSynthTone.superNATURALSynthToneCommon.AnalogFeel)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x34 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthToneCommon.AnalogFeel)));
                                break;
                            case "cbEditTone_superNATURALSynthTone_UnisonSwitch":
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x2e });
                                superNATURALSynthTone.superNATURALSynthToneCommon.UnisonSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALSynthTone_Common_UnisonSize":
                                superNATURALSynthTone.superNATURALSynthToneCommon.UnisonSize = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x3c });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthToneCommon.UnisonSize) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALSynthTone_Common_MonoPoly":
                                superNATURALSynthTone.superNATURALSynthToneCommon.MonoPoly = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x14 });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthToneCommon.MonoPoly) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALSynthTone_LegatoSwitch":
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x32 });
                                superNATURALSynthTone.superNATURALSynthToneCommon.LegatoSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALSynthTone_PortamentoSwitch":
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x12 });
                                superNATURALSynthTone.superNATURALSynthToneCommon.PortamentoSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "slEditTone_superNATURALSynthTone_Common_PortamentoTime":
                                superNATURALSynthTone.superNATURALSynthToneCommon.PortamentoTime = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Common_PortamentoTime.Text = "Portamento Time: " +
                                    ((superNATURALSynthTone.superNATURALSynthToneCommon.PortamentoTime)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x13 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthToneCommon.PortamentoTime)));
                                break;
                            case "cbEditTone_superNATURALSynthTone_Common_PortamentoMode":
                                superNATURALSynthTone.superNATURALSynthToneCommon.PortamentoMode = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x31 });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthToneCommon.PortamentoMode) };
                                SendParameter(address, value);
                                break;
                            // SuperNATURAL Synth Tone Osc ***
                            case "cbEditTone_superNATURALSynthTone_Partial1Switch":
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x19 });
                                superNATURALSynthTone.superNATURALSynthToneCommon.Partial1Switch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALSynthTone_Partial2Switch":
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x1b });
                                superNATURALSynthTone.superNATURALSynthToneCommon.Partial2Switch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALSynthTone_Partial3Switch":
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x1d });
                                superNATURALSynthTone.superNATURALSynthToneCommon.Partial3Switch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALSynthTone_Partial_OscWave":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                superNATURALSynthTone.superNATURALSynthToneCommon.WaveShape = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x00 });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthToneCommon.WaveShape) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALSynthTone_Osc_WaveVariation":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCWaveVariation = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01 });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCWaveVariation) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALSynthTone_Osc_WaveNumber":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].WaveNumber = (UInt16)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x35 });
                                SendParameter(address, superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].WaveNumber);
                                break;
                            case "cbEditTone_superNATURALSynthTone_Osc_WaveGain":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].WaveGain = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x34 });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].WaveGain) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPulseWidthModDepth = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Osc_PulseWidthModDepth.Text = "Pulse Width Mod Depth: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPulseWidthModDepth)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x05 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPulseWidthModDepth)));
                                break;
                            case "slEditTone_superNATURALSynthTone_Osc_PulseWidthShift":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPulseWidthShift = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Osc_PulseWidthShift.Text = "Pulse Width Shift: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPulseWidthShift)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x2a });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPulseWidthShift)));
                                break;
                            case "slEditTone_superNATURALSynthTone_Osc_SuperSawDetune":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].SuperSawDetune = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Osc_SuperSawDetune.Text = "Super Saw Detune: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].SuperSawDetune)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3a });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].SuperSawDetune)));
                                break;
                            // SuperNATURAL Synth Tone Pitch tab ***
                            case "slEditTone_superNATURALSynthTone_Pitch_OSCPitch":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPitch = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_Pitch_OSCPitch.Text = "OSC Pitch: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPitch - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x03 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPitch)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_OSCDetune":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCDetune = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_Pitch_OSCDetune.Text = "OSC Detune: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCDetune - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x04 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCDetune)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPitchEnvAttackTime = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Pitch_PitchEnvAttackTime.Text = "Pitch Env Attack Time: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPitchEnvAttackTime)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x07 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPitchEnvAttackTime)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPitchEnvDecay = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDecay.Text = "OSC Pitch Env Decay: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPitchEnvDecay)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x08 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPitchEnvDecay)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPitchEnvDepth = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_Pitch_OSCPitchEnvDepth.Text = "OSC Pitch Env Depth: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPitchEnvDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x09 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].OSCPitchEnvDepth)), true);
                                break;
                            case "cbEditTone_superNATURALSynthTone_Pitch_OSCOctaveShift":
                                superNATURALSynthTone.superNATURALSynthToneCommon.OctaveShift = (byte)(comboBox.SelectedIndex + 61);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x15 });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthToneCommon.OctaveShift) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp":
                                superNATURALSynthTone.superNATURALSynthToneCommon.PitchBendRangeUp = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Pitch_PitchBendRangeUp.Text = "Pitch Bend Range Up: " +
                                    ((superNATURALSynthTone.superNATURALSynthToneCommon.PitchBendRangeUp)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x16 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthToneCommon.PitchBendRangeUp)));
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown":
                                superNATURALSynthTone.superNATURALSynthToneCommon.PitchBendRangeDown = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Pitch_PitchBendRangeDown.Text = "Pitch Bend Range Down: " +
                                    ((superNATURALSynthTone.superNATURALSynthToneCommon.PitchBendRangeDown)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x17 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthToneCommon.PitchBendRangeDown)));
                                break;
                            // SuperNATURAL Synth Tone Filter tab ***
                            case "cbEditTone_superNATURALSynthTone_Pitch_FILTERMode":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERMode = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x0a });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERMode) };
                                SendParameter(address, value, true);
                                break;
                            case "cbEditTone_superNATURALSynthTone_Pitch_FILTERSlope":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERSlope = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x0b });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERSlope) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_FILTERCutoff":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERCutoff = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Pitch_FILTERCutoff.Text = "FILTER Cutoff: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERCutoff)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x0c });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERCutoff)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERCutoffKeyfollow = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_Pitch_FILTERCutoffKeyfollow.Text = "FILTER Cutoff Keyfollow: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERCutoffKeyfollow - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x0d });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERCutoffKeyfollow)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvVelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvVelocitySens.Text = "FILTER Env Velocity Sens: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvVelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x0e });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvVelocitySens)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_FILTERResonance":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERResonance = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Pitch_FILTERResonance.Text = "FILTER Resonance: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERResonance)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x0f });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTERResonance)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvAttackTime = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvAttackTime.Text = "FILTER Env Attack Time: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvAttackTime)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x10 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvAttackTime)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvDecayTime = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvDecayTime.Text = "FILTER Env Decay Time: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvDecayTime)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x11 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvDecayTime)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvSustainLevel = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvSustainLevel.Text = "FILTER Env Sustain Level: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvSustainLevel)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x12 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvSustainLevel)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvReleaseTime = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvReleaseTime.Text = "FILTER Env Release Time: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvReleaseTime)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x13 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvReleaseTime)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvDepth = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_Pitch_FILTEREnvDepth.Text = "FILTER Env Depth: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x14 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].FILTEREnvDepth)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Pitch_HPFCutoff":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].HPFCutoff = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Pitch_HPFCutoff.Text = "HPF Cutoff: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].HPFCutoff)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x39 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].HPFCutoff)), true);
                                break;
                            // SuperNATURAL Synth Tone AMP tab ***
                            case "slEditTone_superNATURALSynthTone_Amp_AMPLevel":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPLevel = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Amp_AMPLevel.Text = "AMP Level: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPLevel)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x15 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPLevel)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPLevelVelocitySens = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_Amp_AMPLevelVelocitySens.Text = "AMP Level Velocity Sens: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPLevelVelocitySens - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x16 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPLevelVelocitySens)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Amp_AMPPan":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPPan = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_Amp_AMPPan.Text = "AMP Pan: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPPan - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x1b });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPPan)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPLevelKeyfollow = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_Amp_AMPLevelKeyfollow.Text = "AMP Level Keyfollow: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPLevelKeyfollow - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3c });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPLevelKeyfollow)));
                                break;
                            case "slEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPEnvAttackTime = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Amp_AMPEnvAttackTime.Text = "AMP Env Attack Time: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPEnvAttackTime)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x17 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPEnvAttackTime)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPEnvDecayTime = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Amp_AMPEnvDecayTime.Text = "AMP Env Decay Time: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPEnvDecayTime)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x18 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPEnvDecayTime)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPEnvSustainLevel = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Amp_AMPEnvSustainLevel.Text = "AMP Env Sustain Level: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPEnvSustainLevel)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x19 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPEnvSustainLevel)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPEnvReleaseTime = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Amp_AMPEnvReleaseTime.Text = "AMP Env Release Time: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPEnvReleaseTime)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x1a });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].AMPEnvReleaseTime)), true);
                                break;
                            // SuperNATURALSynthTone LFO controls ***
                            case "cbEditTone_superNATURALSynthTone_LFO_LFOShape":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOShape = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x1c });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOShape) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_superNATURALSynthTone_LFO_LFORate":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFORate = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_LFO_LFORate.Text = "LFO Rate: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFORate)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x1d });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFORate)), true);
                                break;
                            case "cbEditTone_superNATURALSynthTone_LFOTempoSyncSwitch":
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x1e });
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOTempoSyncSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0), true);
                                break;
                            case "cbEditTone_superNATURALSynthTone_LFO_LFOTempoSyncNote":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOTempoSyncNote = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x1f });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOTempoSyncNote) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_superNATURALSynthTone_LFO_LFOFadeTime":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOFadeTime = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_LFO_LFOFadeTime.Text = "LFO Fade Time: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOFadeTime)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x20 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOFadeTime)), true);
                                break;
                            case "cbEditTone_superNATURALSynthTone_LFOKeyTrigger":
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x21 });
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOKeyTrigger = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_LFO_LFOPitchDepth":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOPitchDepth = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_LFO_LFOPitchDepth.Text = "LFO Pitch Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOPitchDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x22 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOPitchDepth)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_LFO_LFOFilterDepth":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOFilterDepth = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_LFO_LFOFilterDepth.Text = "LFO Filter Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOFilterDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x23 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOFilterDepth)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_LFO_LFOAMPDepth":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOAmpDepth = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_LFO_LFOAMPDepth.Text = "LFO AMP Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOAmpDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x24 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOAmpDepth)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_LFO_LFOPanDepth":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOPanDepth = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_LFO_LFOPanDepth.Text = "LFO Pan Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOPanDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x25 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LFOPanDepth)), true);
                                break;
                            // SuperNATURALSynthTone Modulation LFO controls ***
                            case "cbEditTone_superNATURALSynthTone_LFO_ModulationLFOShape":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOShape = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x26 });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOShape) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_superNATURALSynthTone_LFO_ModulationLFORate":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFORate = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_LFO_ModulationLFORate.Text = "Modulation LFO Rate: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFORate)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x27 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFORate)), true);
                                break;
                            case "cbEditTone_superNATURALSynthTone_ModulationLFOTempoSyncSwitch":
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x28 });
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOTempoSyncSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0), true);
                                break;
                            case "cbEditTone_superNATURALSynthTone_LFO_ModulationLFOTempoSyncNote":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOTempoSyncNote = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x29 });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOTempoSyncNote) };
                                SendParameter(address, value, true);
                                break;
                            case "slEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOPitchDepth = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_LFO_ModulationLFOPitchDepth.Text = "Modulation LFO Pitch Depth: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOPitchDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x2c });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOPitchDepth)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOFilterDepth = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_LFO_ModulationLFOFilterDepth.Text = "Modulation LFO Filter Depth: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOFilterDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x2d });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOFilterDepth)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOAmpDepth = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_LFO_ModulationLFOAmpDepth.Text = "Modulation LFO Amp Depth: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOAmpDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x2e });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOAmpDepth)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Amp_ModulationLFOPanDepth":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOPanDepth = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_LFO_ModulationLFOPanDepth.Text = "Modulation LFO Pan Depth: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOPanDepth - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x2f });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFOPanDepth)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFORateControl = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_LFO_ModulationLFORateControl.Text = "Modulation LFO Rate Control: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFORateControl - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3b });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].ModulationLFORateControl)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_Amp_LevelAftertouchSens":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LevelAftertouchSens = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_LFO_LevelAftertouchSens.Text = "Level Aftertouch Sens: " + ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LevelAftertouchSens - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x31 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].LevelAftertouchSens)), true);
                                break;
                            case "slEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens":
                                superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].CutoffAftertouchSens = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALSynthTone_LFO_CutoffAftertouchSens.Text = "Cutoff Aftertouch Sens: " +
                                    ((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].CutoffAftertouchSens - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x30 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthTonePartial[currentPartial].CutoffAftertouchSens)), true);
                                break;
                            // SuperNATURALSynthTone Misc controls ***
                            case "slEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens":
                                superNATURALSynthTone.superNATURALSynthToneMisc.AttackTimeIntervalSens = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Misc_AttackTimeIntervalSens.Text = "Attack Time Interval Sens: " +
                                    ((superNATURALSynthTone.superNATURALSynthToneMisc.AttackTimeIntervalSens)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.MISC, new byte[] { 0x01 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthToneMisc.AttackTimeIntervalSens)));
                                break;
                            case "slEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens":
                                superNATURALSynthTone.superNATURALSynthToneMisc.ReleaseTimeIntervalSens = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Misc_ReleaseTimeIntervalSens.Text = "Release Time Interval Sens: " +
                                    ((superNATURALSynthTone.superNATURALSynthToneMisc.ReleaseTimeIntervalSens)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.MISC, new byte[] { 0x02 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthToneMisc.ReleaseTimeIntervalSens)));
                                break;
                            case "slEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens":
                                superNATURALSynthTone.superNATURALSynthToneMisc.PortamentoTimeIntervalSens = (byte)((slider.Value));
                                tbEditTone_superNATURALSynthTone_Misc_PortamentoTimeIntervalSens.Text = "Portamento Time Interval Sens: " +
                                    ((superNATURALSynthTone.superNATURALSynthToneMisc.PortamentoTimeIntervalSens)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.MISC, new byte[] { 0x03 });
                                SendParameter(address, (byte)((superNATURALSynthTone.superNATURALSynthToneMisc.PortamentoTimeIntervalSens)));
                                break;
                            case "cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopMode":
                                Generic_GotFocus(comboBox, new FocusEventArgs(comboBox, true)); // This trig is to make selection change reflect in help
                                superNATURALSynthTone.superNATURALSynthToneMisc.EnvelopeLoopMode = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.MISC, new byte[] { 0x04 });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthToneMisc.EnvelopeLoopMode) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALSynthTone_Misc_EnvelopeLoopSyncNote":
                                superNATURALSynthTone.superNATURALSynthToneMisc.EnvelopeLoopSyncNote = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.MISC, new byte[] { 0x05 });
                                value = new byte[] { (byte)(superNATURALSynthTone.superNATURALSynthToneMisc.EnvelopeLoopSyncNote) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALSynthTone_ChromaticPortamento":
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.MISC, new byte[] { 0x05 });
                                superNATURALSynthTone.superNATURALSynthToneMisc.ChromaticPortamento = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            // SuperNATURAL Synth Tone MFX *****
                            case "cbEditTone_MFXType":
                                HandleCommonMFX(comboBox);
                                break;
                            // SuperNATURAL Synth Tone MFX Control *****
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlSource0":
                                commonMFX.MFXControlSource[0] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x05 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[0]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlSource1":
                                commonMFX.MFXControlSource[1] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x07 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[1]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlSource2":
                                commonMFX.MFXControlSource[2] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x09 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[2]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlSource3":
                                commonMFX.MFXControlSource[3] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0b });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[3]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlAssign0":
                                commonMFX.MFXControlAssign[0] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0d });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[0]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlAssign1":
                                commonMFX.MFXControlAssign[1] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0e });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[1]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlAssign2":
                                commonMFX.MFXControlAssign[2] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0f });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[2]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlAssign3":
                                commonMFX.MFXControlAssign[3] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x10 });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[3]) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_CommonMFX_MFXControl_MFXControlSense0":
                                commonMFX.MFXControlSens[0] = (byte)((slider.Value) + 64);
                                tbEditTone_CommonMFX_MFXControl_MFXControlSens[0].Text = "MFX Control 1 Sense: " + ((commonMFX.MFXControlSens[0] - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x06 });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[0])));
                                break;
                            case "slEditTone_CommonMFX_MFXControl_MFXControlSense1":
                                commonMFX.MFXControlSens[1] = (byte)((slider.Value) + 64);
                                tbEditTone_CommonMFX_MFXControl_MFXControlSens[1].Text = "MFX Control 2 Sense: " + ((commonMFX.MFXControlSens[1] - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x08 });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[1])));
                                break;
                            case "slEditTone_CommonMFX_MFXControl_MFXControlSense2":
                                commonMFX.MFXControlSens[2] = (byte)((slider.Value) + 64);
                                tbEditTone_CommonMFX_MFXControl_MFXControlSens[2].Text = "MFX Control 3 Sense: " + ((commonMFX.MFXControlSens[2] - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0a });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[2])));
                                break;
                            case "slEditTone_CommonMFX_MFXControl_MFXControlSense3":
                                commonMFX.MFXControlSens[3] = (byte)((slider.Value) + 64);
                                tbEditTone_CommonMFX_MFXControl_MFXControlSens[3].Text = "MFX Control 4 Sense: " + ((commonMFX.MFXControlSens[3] - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0c });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[3])));
                                break;
                            default:
                                t.Trace("Missing case for \"" + name + "\"!");
                                break;
                        }
                        break;
                    // SuperNATURAL Drum Set ***************************************************************************
                    case ProgramType.SUPERNATURAL_DRUM_KIT:
                        switch (name)
                        {
                            // SuperNATURAL Drum Kit Common *****
                            case "cbEditTone_superNATURALDrumKit_Common_PhraseNumber":
                                superNATURALDrumKit.superNATURALDrumKitCommon.PhraseNumber = (byte)(comboBox.SelectedIndex);
                                byte[] address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMON, new byte[] { 0x12 });
                                byte[] value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommon.PhraseNumber) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_superNATURALDrumKit_Common_KitLevel":
                                superNATURALDrumKit.superNATURALDrumKitCommon.KitLevel = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Common_KitLevel.Text = "Kit Level: " + ((superNATURALDrumKit.superNATURALDrumKitCommon.KitLevel)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMON, new byte[] { 0x10 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommon.KitLevel)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Common_AmbienceLevel":
                                superNATURALDrumKit.superNATURALDrumKitCommon.AmbienceLevel = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Common_AmbienceLevel.Text = "Ambience Level: " +
                                    ((superNATURALDrumKit.superNATURALDrumKitCommon.AmbienceLevel)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMON, new byte[] { 0x11 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommon.AmbienceLevel)));
                                break;
                            // SuperNATURAL Drum Kit Instrument *****
                            case "cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber":
                                handleControlEvents = false;
                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].BankNumber = (byte)(comboBox.SelectedIndex);
                                //commonState.keyNames[currentPartial] = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
                                // Bank number is 0 for Internal and 1 for ExSN6.Read more in MakeDynamicControls.cs AddSupernaturalDrumKitDruminstrumentControls()
                                switch (superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].BankNumber)
                                {
                                    case 0:
                                        cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.Visibility = Visibility.Visible;
                                        cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.Visibility = Visibility.Collapsed;
                                        cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.IsEnabled = true;
                                        cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.IsEnabled = false;
                                        try
                                        {
                                            cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.SelectedIndex =
                                            superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].InstNumber;
                                        }
                                        catch
                                        {
                                            cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.SelectedIndex = 0;
                                        }
                                        break;
                                    case 1:
                                        cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.Visibility = Visibility.Collapsed;
                                        cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.Visibility = Visibility.Visible;
                                        cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.IsEnabled = false;
                                        cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.IsEnabled = true;
                                        try
                                        {
                                            cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.SelectedIndex =
                                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].InstNumber - 168;
                                        }
                                        catch
                                        {
                                            cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.SelectedIndex = 0;
                                        }
                                        break;
                                }
                                SuperNaturalDrumKitSetVariation();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x00 });
                                value = new byte[] { 0x00, 0x00, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].BankNumber * 0x0a),
                                (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].BankNumber * 0x09)};
                                //value = AddBytes16(value, SplitToBytes16((UInt16)(superNATURALDrumKit
                                //    .superNATURALDrumKitKey[currentPartial].InstNumber[superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].BankNumber])));
                                value = AddBytes16(value, SplitToBytes16(superNATURALDrumKit
                                    .superNATURALDrumKitKey[currentPartial].InstNumber));
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber":
                                switch (cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber.SelectedIndex)
                                {
                                    case 0:
                                        {
                                            try
                                            {
                                                // When changing wave for a SN-D key, also update in commonState.keyNames so it can later be saved in 
                                                commonState.keyNames[currentPartial + 6] = ((String)comboBox.SelectedItem);
                                                commonState.keyNames[currentPartial + 6] = commonState.keyNames[currentPartial + 6]
                                                    .Remove(0, commonState.keyNames[currentPartial + 6].IndexOf(':') + 1).Trim();
                                            }
                                            catch { }
                                            superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].InstNumber = (byte)(comboBox.SelectedIndex);
                                            SuperNaturalDrumKitSetVariation();
                                            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x00 });
                                            value = new byte[] { 0x00, 0x00, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].BankNumber * 0x0a),
                                            (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].BankNumber * 0x09)};
                                            value = AddBytes16(value, SplitToBytes16((UInt16)(superNATURALDrumKit
                                                .superNATURALDrumKitKey[currentPartial].InstNumber)));
                                            SendParameter(address, value);
                                            DrumKit_WaveOn(0);
                                        }
                                        break;
                                    case 1:
                                        {
                                            try
                                            {
                                                // When changing wave for a SN-D key, also update in commonState.keyNames so it can later be saved in 
                                                commonState.keyNames[currentPartial + 6] = ((String)comboBox.SelectedItem);
                                                commonState.keyNames[currentPartial + 6] = commonState.keyNames[currentPartial + 6]
                                                        .Remove(0, commonState.keyNames[currentPartial + 6].IndexOf(':') + 1).Trim();
                                            }
                                            catch { }
                                            superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].InstNumber =
                                                (byte)(comboBox.SelectedIndex + 168); // I-7 does not have bank number for this, bank 1 is all over 168
                                            SuperNaturalDrumKitSetVariation();
                                            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x00 });
                                            value = new byte[] { 0x00, 0x00, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].BankNumber * 0x0a),
                                            (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].BankNumber * 0x09)};
                                            value = AddBytes16(value, SplitToBytes16((UInt16)(superNATURALDrumKit
                                                .superNATURALDrumKitKey[currentPartial].InstNumber)));
                                            SendParameter(address, value);
                                        }
                                        break;
                                }
                                break;
                            case "slEditTone_superNATURALDrumKit_Druminstrument_Level":
                                     superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Level = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Druminstrument_Level.Text = "Level: " + ((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Level)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x04 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Level)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Druminstrument_Pan":
                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Pan = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALDrumKit_Druminstrument_Pan.Text = "Pan: " + ((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Pan - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x05 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Pan)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel":
                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].ChorusSendLevel = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Druminstrument_ChorusSendLevel.Text = "Chorus Send Level: " + ((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].ChorusSendLevel)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x06 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].ChorusSendLevel)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel":
                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].ReverbSendLevel = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Druminstrument_ReverbSendLevel.Text = "Reverb Send Level: " + ((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].ReverbSendLevel)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x07 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].ReverbSendLevel)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Druminstrument_Tune":
                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Tune = (byte)(slider.Value + 128);
                                tbEditTone_superNATURALDrumKit_Druminstrument_Tune.Text = "Tune: " + (superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Tune - 128).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x08 });
                                SendParameter(address, new byte[] { 0x00, 0x00, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Tune / 16),
                                    (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Tune % 16) } );
                                break;
                            case "slEditTone_superNATURALDrumKit_Druminstrument_Attack":
                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Attack = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Druminstrument_Attack.Text = "Attack: " + ((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Attack)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0c });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Attack)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Druminstrument_Decay":
                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Decay = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALDrumKit_Druminstrument_Decay.Text = "Decay: " + ((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Decay - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0d });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Decay)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Druminstrument_Brilliance":
                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Brilliance = (byte)((slider.Value) + 64);
                                tbEditTone_superNATURALDrumKit_Druminstrument_Brilliance.Text = "Brilliance: " + ((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Brilliance - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0e });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Brilliance)));
                                break;
                            case "cbEditTone_superNATURALDrumKit_Druminstrument_Variation":
                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Variation = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0f });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].Variation) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Druminstrument_OutputAssign":
                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].OutputAssign = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x12 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].OutputAssign) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_superNATURALDrumKit_Druminstrument_DynamicRange":
                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].DynamicRange = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Druminstrument_DynamicRange.Text = "Dynamic Range: " + ((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].DynamicRange)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x10 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].DynamicRange)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Druminstrument_StereoWidth":
                                superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].StereoWidth = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Druminstrument_StereoWidth.Text = "Stereo Width: " + ((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].StereoWidth)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x11 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitKey[currentPartial].StereoWidth)));
                                break;
                            // SuperNATURAL Drum Kit Compressor *****
                            case "cbEditTone_superNATURALDrumKit_CompSwitch0":
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x00 });
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALDrumKit_CompSwitch1":
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0e });
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALDrumKit_CompSwitch2":
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1c });
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALDrumKit_CompSwitch3":
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2a });
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALDrumKit_CompSwitch4":
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x38 });
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALDrumKit_CompSwitch5":
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x46 });
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime0":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompAttackTime = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x01 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompAttackTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime1":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompAttackTime = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0f });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompAttackTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime2":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompAttackTime = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1d });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompAttackTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime3":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompAttackTime = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2b });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompAttackTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime4":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompAttackTime = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x39 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompAttackTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompAttackTime5":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompAttackTime = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x47 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompAttackTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompReleaseTime0":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompReleaseTime = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x02 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompReleaseTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompReleaseTime1":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompReleaseTime = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x10 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompReleaseTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompReleaseTime2":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompReleaseTime = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1e });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompReleaseTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompReleaseTime3":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompReleaseTime = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2c });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompReleaseTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompReleaseTime4":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompReleaseTime = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3a });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompReleaseTime) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompReleaseTime5":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompReleaseTime = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x48 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompReleaseTime) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_superNATURALDrumKit_Compressor_CompThreshold0":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompThreshold = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[0].Text = "Thresh.1: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompThreshold)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x03 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompThreshold)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Compressor_CompThreshold1":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompThreshold = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[1].Text = "Thresh.2: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompThreshold)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x11 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompThreshold)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Compressor_CompThreshold2":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompThreshold = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[2].Text = "Thresh.3: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompThreshold)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1f });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompThreshold)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Compressor_CompThreshold3":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompThreshold = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[3].Text = "Thresh.4: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompThreshold)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2d });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompThreshold)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Compressor_CompThreshold4":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompThreshold = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[4].Text = "Thresh.5: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompThreshold)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3b });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompThreshold)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Compressor_CompThreshold5":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompThreshold = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Compressor_CompThreshold[5].Text = "Thresh.6: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompThreshold)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x49 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompThreshold)));
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompRatio0":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompRatio = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x04 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompRatio) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompRatio1":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompRatio = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x12 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompRatio) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompRatio2":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompRatio = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x20 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompRatio) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompRatio3":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompRatio = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2e });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompRatio) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompRatio4":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompRatio = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3c });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompRatio) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Compressor_CompRatio5":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompRatio = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4a });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompRatio) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_superNATURALDrumKit_Compressor_CompOutputGain0":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompOutputGain = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[0].Text = "Outp.1 gain: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompOutputGain)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x05 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompOutputGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Compressor_CompOutputGain1":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompOutputGain = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[1].Text = "Outp.2 gain: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompOutputGain)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x13 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompOutputGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Compressor_CompOutputGain2":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompOutputGain = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[2].Text = "Outp.3 gain: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompOutputGain)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x21 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompOutputGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Compressor_CompOutputGain3":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompOutputGain = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[3].Text = "Outp.4 gain: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompOutputGain)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2f });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompOutputGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Compressor_CompOutputGain4":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompOutputGain = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[4].Text = "Outp.5 gain: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompOutputGain)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3d });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompOutputGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Compressor_CompOutputGain5":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompOutputGain = (byte)((slider.Value));
                                tbEditTone_superNATURALDrumKit_Compressor_CompOutputGain[5].Text = "Outp.6 gain: " + ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompOutputGain)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4b });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompOutputGain)));
                                break;
                            // SuperNATURAL Drum Kit Equalizer *****
                            case "cbEditTone_superNATURALDrumKit_EQSwitch0":
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x06 });
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALDrumKit_EQSwitch1":
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x14 });
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALDrumKit_EQSwitch2":
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x22 });
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALDrumKit_EQSwitch3":
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x30 });
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALDrumKit_EQSwitch4":
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4e });
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALDrumKit_EQSwitch5":
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x5c });
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQSwitch = (Boolean)checkBox.IsChecked;
                                SendParameter(address, (byte)((Boolean)checkBox.IsChecked ? 1 : 0));
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq0":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQLowFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x07 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQLowFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq1":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQLowFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x15 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQLowFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq2":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQLowFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x23 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQLowFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq3":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQLowFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x31 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQLowFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq4":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQLowFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3f });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQLowFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQLowFreq5":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQLowFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4d });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQLowFreq) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQLowGain0":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQLowGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[0].Text = "Low1 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQLowGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x08 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQLowGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQLowGain1":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQLowGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[1].Text = "Low2 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQLowGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x16 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQLowGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQLowGain2":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQLowGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[2].Text = "Low3 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQLowGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x24 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQLowGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQLowGain3":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQLowGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[3].Text = "Low4 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQLowGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x32 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQLowGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQLowGain4":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQLowGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[4].Text = "Low5 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQLowGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x40 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQLowGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQLowGain5":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQLowGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQLowGain[5].Text = "Low6 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQLowGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4e });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQLowGain)));
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq0":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQMidFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x09 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQMidFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq1":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQMidFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x17 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQMidFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq2":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQMidFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x25 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQMidFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq3":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQMidFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x33 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQMidFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq4":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQMidFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x41 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQMidFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQMidFreq5":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQMidFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4f });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQMidFreq) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQMidGain0":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQMidGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[0].Text = "Mid1 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQMidGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0a });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQMidGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQMidGain1":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQMidGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[1].Text = "Mid2 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQMidGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x18 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQMidGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQMidGain2":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQMidGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[2].Text = "Mid3 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQMidGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x26 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQMidGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQMidGain3":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQMidGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[3].Text = "Mid4 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQMidGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x34 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQMidGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQMidGain4":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQMidGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[4].Text = "Mid5 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQMidGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x42 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQMidGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQMidGain5":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQMidGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQMidGain[5].Text = "Mid6 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQMidGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x50 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQMidGain)));
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ0":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQMidQ = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0b });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQMidQ) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ1":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQMidQ = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x19 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQMidQ) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ2":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQMidQ = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x27 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQMidQ) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ3":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQMidQ = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x35 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQMidQ) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ4":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQMidQ = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x43 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQMidQ) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQMidQ5":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQMidQ = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x51 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQMidQ) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq0":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQHighFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0c });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQHighFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq1":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQHighFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1a });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQHighFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq2":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQHighFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x28 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQHighFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq3":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQHighFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x36 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQHighFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq4":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQHighFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x44 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQHighFreq) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_superNATURALDrumKit_Equalizer_EQHighFreq5":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQHighFreq = (byte)(comboBox.SelectedIndex);
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x52 });
                                value = new byte[] { (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQHighFreq) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQHighGain0":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQHighGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[0].Text = "High1 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQHighGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0d });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQHighGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQHighGain1":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQHighGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[1].Text = "High2 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQHighGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1b });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQHighGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQHighGain2":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQHighGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[2].Text = "High3 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQHighGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x29 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQHighGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQHighGain3":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQHighGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[3].Text = "High4 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQHighGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x37 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQHighGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQHighGain4":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQHighGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[4].Text = "High5 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQHighGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x45 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQHighGain)));
                                break;
                            case "slEditTone_superNATURALDrumKit_Equalizer_EQHighGain5":
                                superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQHighGain = (byte)((slider.Value) + 15);
                                tbEditTone_superNATURALDrumKit_Equalizer_EQHighGain[5].Text = "High6 gain: " + 
                                    ((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQHighGain - 15)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x53 });
                                SendParameter(address, (byte)((superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQHighGain)));
                                break;
                            // SuperNATURAL Drum Kit MFX *****
                            case "cbEditTone_MFXType":
                                HandleCommonMFX(comboBox);
                                break;
                            // SuperNATURAL Drum Kit MFX Control *****
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlSource0":
                                commonMFX.MFXControlSource[0] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x05 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[0]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlSource1":
                                commonMFX.MFXControlSource[1] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x07 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[1]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlSource2":
                                commonMFX.MFXControlSource[2] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x09 });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[2]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlSource3":
                                commonMFX.MFXControlSource[3] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0b });
                                value = new byte[] { (byte)(commonMFX.MFXControlSource[3]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlAssign0":
                                commonMFX.MFXControlAssign[0] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0d });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[0]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlAssign1":
                                commonMFX.MFXControlAssign[1] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0e });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[1]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlAssign2":
                                commonMFX.MFXControlAssign[2] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0f });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[2]) };
                                SendParameter(address, value);
                                break;
                            case "cbEditTone_CommonMFX_MFXControl_MFXControlAssign3":
                                commonMFX.MFXControlAssign[3] = (byte)comboBox.SelectedIndex;
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x10 });
                                value = new byte[] { (byte)(commonMFX.MFXControlAssign[3]) };
                                SendParameter(address, value);
                                break;
                            case "slEditTone_CommonMFX_MFXControl_MFXControlSense0":
                                commonMFX.MFXControlSens[0] = (byte)((slider.Value) + 64);
                                tbEditTone_CommonMFX_MFXControl_MFXControlSens[0].Text = "MFX Control 1 Sense: " + ((commonMFX.MFXControlSens[0] - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x06 });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[0])));
                                break;
                            case "slEditTone_CommonMFX_MFXControl_MFXControlSense1":
                                commonMFX.MFXControlSens[1] = (byte)((slider.Value) + 64);
                                tbEditTone_CommonMFX_MFXControl_MFXControlSens[1].Text = "MFX Control 2 Sense: " + ((commonMFX.MFXControlSens[1] - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x08 });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[1])));
                                break;
                            case "slEditTone_CommonMFX_MFXControl_MFXControlSense2":
                                commonMFX.MFXControlSens[2] = (byte)((slider.Value) + 64);
                                tbEditTone_CommonMFX_MFXControl_MFXControlSens[2].Text = "MFX Control 3 Sense: " + ((commonMFX.MFXControlSens[2] - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0a });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[2])));
                                break;
                            case "slEditTone_CommonMFX_MFXControl_MFXControlSense3":
                                commonMFX.MFXControlSens[3] = (byte)((slider.Value) + 64);
                                tbEditTone_CommonMFX_MFXControl_MFXControlSens[3].Text = "MFX Control 4 Sense: " + ((commonMFX.MFXControlSens[3] - 64)).ToString();
                                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0c });
                                SendParameter(address, (byte)((commonMFX.MFXControlSens[3])));
                                break;
                            default:
                                t.Trace("Missing case for \"" + name + "\"!");
                                break;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// This is the common MFX handler that is called from all instrument types MFX handlers above.
        /// </summary>
        /// <param name="comboBox"></param>
        private void HandleCommonMFX(ComboBox comboBox)
        {
            if (handleControlEvents)
            {
                handleControlEvents = false;
                byte selectedIndex = (byte)comboBox.SelectedIndex;
                commonMFX.MFXType = SetMFXTypeAndOffset((byte)comboBox.SelectedIndex);
                // Is this read from Integra-7, or should we use default parameters?
                //if (commonMFX.MFXType != mfxPageReadFromIntegra7)
                {
                    // We have changed type of MFX. We cannot ask Integra-7 for parameters, so use our saved ones, modified or not:
                    useMFXParameterDataFromIntegra_7 = false;
                    //byte parameterCount = (byte)numberedParametersContent.ParameterTypes[currentMFXType + currentMFXTypeOffset].Length;
                    // Replace the parameters list wint a new blank one:
                    //commonMFX.MFXNumberedParameters.Parameters.Parameters = new NumberedParameter[parameterCount];
                    commonMFX.MFXNumberedParameters.Parameters.Parameters = new NumberedParameter[32];
                    // Make a list of all parameters:
                    List<byte> values = new List<byte>();
                    // Fill out laned parameters:
                    values.Add(commonMFX.MFXType);
                    values.Add(commonMFX.Reserve1);
                    values.Add(commonMFX.MFXChorusSendLevel);
                    values.Add(commonMFX.MFXReverbSendLevel);
                    values.Add(commonMFX.Reserve2);
                    for (byte i = 0; i < 4; i++)
                    {
                        values.Add(commonMFX.MFXControlSource[i]);
                        values.Add(commonMFX.MFXControlSens[i]);
                    }
                    for (byte i = 0; i < 4; i++)
                    {
                        values.Add(commonMFX.MFXControlAssign[i]);
                    }
                    // We need to know the number of actual pages this MFX occupies:
                    byte pageCount = numberedParametersContent.MFXPageCount[currentMFXType + currentMFXTypeOffset];
                    byte page = 0;
                    byte parameterReadIndex = 0;
                    // Fill out all numbered parameters (also those that are not in use, in order to send complete):
                    for (byte parameterWriteIndex = 0; parameterWriteIndex < 32; parameterWriteIndex++)
                    {
                        values.Add(0x08);
                        values.Add((byte)((numberedParameterValues.Parameters[currentMFXType][parameterWriteIndex] & 0x0f00) >> 8));
                        values.Add((byte)((numberedParameterValues.Parameters[currentMFXType][parameterWriteIndex] & 0x00f0) >> 4));
                        values.Add((byte)(numberedParameterValues.Parameters[currentMFXType][parameterWriteIndex] & 0x000f));
                        // Update the blank list of parameters that we will use to display controls (for all pages if more than one):
                        commonMFX.MFXNumberedParameters.Parameters.Parameters[parameterWriteIndex] = new NumberedParameter();
                        try
                        {
                            if (parameterReadIndex >= numberedParametersContent.ParameterNames[numberedParametersContent.MFXIndexFromType[currentMFXType] + page].Length)
                            {
                                parameterReadIndex = 0;
                                page++;
                            }
                        }
                        catch { }
                        if (page < pageCount)
                        {
                            // Fetch MFX name from numberedParametersContent:
                            commonMFX.MFXNumberedParameters.Parameters.Parameters[parameterWriteIndex].Name =
                                numberedParametersContent.TypeNames[numberedParametersContent.MFXIndexFromType[currentMFXType] + page];
                            // Fetch control name from numberedParametersContent:
                            commonMFX.MFXNumberedParameters.Parameters.Parameters[parameterWriteIndex].ControlName =
                                numberedParametersContent.ParameterNames[numberedParametersContent.MFXIndexFromType[currentMFXType] + page][parameterReadIndex];
                            // Fetch control type from numberedParametersContent:
                            commonMFX.MFXNumberedParameters.Parameters.Parameters[parameterWriteIndex].Type =
                                numberedParametersContent.ParameterTypes[numberedParametersContent.MFXIndexFromType[currentMFXType] + page][parameterReadIndex];
                            // Fetch default control value from numberedParameterValues:
                            commonMFX.MFXNumberedParameters.Parameters.Parameters[parameterWriteIndex].Value.Value =
                                numberedParameterValues.Parameters[currentMFXType][parameterWriteIndex];
                                //numberedParameterValues.Parameters[currentMFXType][parameterWriteIndex]; // This is correct!
                            // If this is a combo box, fetch combobox strings from commonMFX.MFXNumberedParameters:
                            if (commonMFX.MFXNumberedParameters.Parameters.Parameters[parameterWriteIndex].Type >= PARAMETER_TYPE.COMBOBOX_0_TO_100_STEP_0_1_TO_2
                                && commonMFX.MFXNumberedParameters.Parameters.Parameters[parameterWriteIndex].Type <= PARAMETER_TYPE.COMBOBOX_WAVE_SHAPE)
                            {
                                commonMFX.MFXNumberedParameters.Parameters.Parameters[parameterWriteIndex].Value.Text =
                                    parameterSets.GetNumberedParameter(commonMFX.MFXNumberedParameters.Parameters.Parameters[parameterWriteIndex].Type);
                            }
                            parameterReadIndex++;
                        }
                    }
                    // Here we tell Integra-7 to change MFX type:
                    byte[] address = MakeAddress(currentProgramType, ParameterPage.COMMONMFX, new byte[] { 0x00 });
                    byte[] value = values.ToArray();
                    SendParameter(address, value);
                    // Update the controls:
                    //RemoveControls(ControlsGrid);
                    //AddMFXControls();
                }
                //else
                //{
                //    mfxPageReadFromIntegra7 = 0xfe;
                //    // We are still using the same MFX, probably another page of a splitted type. We can use the current
                //    // parameter settings in CommonMFX.
                //    useMFXParameterDataFromIntegra_7 = true;
                //    QueryCommonMFX();
                //}
                // Update the controls:
                RemoveControls(ControlsGrid);
                AddMFXControls();
            }
        }

        private void SuperNaturalDrumKitSetVariation()
        {
            currentHandleControlEvents = handleControlEvents;
            handleControlEvents = false;
            DrumInstrument drumInstrumentForVariations =
                superNATURALDrumKitInstrumentList.DrumInstruments[superNATURALDrumKit
                .superNATURALDrumKitKey[currentPartial].InstNumber];
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
            handleControlEvents = currentHandleControlEvents;
        }

        private void DrumKit_WaveOn(byte side)
        {
            byte key = currentKey;
            switch (selectedSound.ProgramType)
            {
                case ProgramType.PCM_DRUM_KIT:
                    key += 21;
                    break;
                case ProgramType.SUPERNATURAL_DRUM_KIT:
                    key += 27;
                    break;
            }
            DrumKit_WaveOff();
            switch (side)
            {
                case 0:
                    commonState.midi.NoteOn(commonState.CurrentPart, key, 0x70);
                    KeySamplePlayingL = key;
                    break;
                case 1:
                    commonState.midi.NoteOn(commonState.CurrentPart, key, 0x70);
                    KeySamplePlayingR = key;
                    break;
            }
        }

        private void DrumKit_WaveOff()
        {
            if (KeySamplePlayingL > -1)
            {
                commonState.midi.NoteOff(commonState.CurrentPart, (byte)KeySamplePlayingL);
                KeySamplePlayingL = -1;
            }
            else if (KeySamplePlayingR > -1)
            {
                commonState.midi.NoteOff(commonState.CurrentPart, (byte)KeySamplePlayingR);
                KeySamplePlayingR = -1;
            }
        }

        //private void UpdateDrumNames()
        //{
        //    t.Trace("private void UpdateDrumNames()");
        //    //if (commonState.currentTone.Category == "Drum" && commonState.drumKeyAssignLists.KeyboardNameList(commonState.currentTone.Group, commonState.currentTone.Name) != null)
        //    Int32 drumSetIndex = commonState.drumKeyAssignLists.Drumset(commonState.currentTone.Group, commonState.currentTone.Name);
        //    commonState.keyNames = new List<String>();
        //    foreach (String keyName in commonState.drumKeyAssignLists.KeyboardNameList(commonState.currentTone.Group, commonState.currentTone.Name))
        //    {
        //        commonState.keyNames.Add(keyName);
        //    }
        //}
    }
}
