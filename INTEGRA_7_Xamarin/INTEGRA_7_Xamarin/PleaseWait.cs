using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public enum WaitingFor
    {
        MIDI,
        INTEGRA_7,
        EDIT,
        READING_STUDIO_SET,
        READING_STUDIO_SET_NAMES,
        IDLE
    }

    public enum WaitingState
    {
        INITIALIZING,
        WAITING,
        PROCESSING,
        DONE
    }

    public enum UIAction
    {
        NONE,
        PROGRESS,
        GOTO_LIBRARIAN,
        GOTO_MOTIONAL_SURROUND,
        GOTO_EDIT_STUDIO_SET
    }

    public partial class UIHandler
    {

        // Please Wait controls:
        public Grid grid_PleaseWait;
        public Grid grid_PleaseWaitTop;
        public Grid grid_PleaseWaitBottom;
        public ProgressBar pb_WaitingProgress;
        public Button btnPleaseWait;

        private WaitingFor waitingFor;
        private WaitingState waitingState;
        private CurrentPage continueTo;
        private Int32 waitCount;
        private Int32 studioSetNumber;
        private Object deviceSpecifics;
        private UIAction uiAction;
        private Double progressStep;

        /// <summary>
        /// Call to show a wait page with a progress bar
        /// </summary>
        /// <param name="waitingFor">Controls text and behaviour</param>
        /// <param name="o">Pass any object needed if applicable</param>
        public void ShowPleaseWaitPage(WaitingFor waitingFor, CurrentPage continueTo, Object deviceSpecifics)
        {
            currentPage = CurrentPage.PLEASE_WAIT;
            this.continueTo = continueTo;
            this.deviceSpecifics = deviceSpecifics;
            if (!PleaseWait_IsCreated)
            {
                DrawPleaseWaitPage();
                PleaseWait_StackLayout.MinimumWidthRequest = 1;
                mainStackLayout.Children.Add(PleaseWait_StackLayout);
                PleaseWait_IsCreated = true;
            }
            SetStackLayoutColors(PleaseWait_StackLayout);
            PleaseWait_StackLayout.IsVisible = true;
            this.waitingFor = waitingFor;
            waitingState = WaitingState.INITIALIZING;
            uiAction = UIAction.NONE;
            grid_PleaseWaitTop.BackgroundColor = colorSettings.Background;
            grid_PleaseWaitBottom.BackgroundColor = colorSettings.Background;
            PleaseWait_Init();
            //this.o = o;
        }

        private void PleaseWait_Init()
        {
            switch (waitingFor)
            {
                case WaitingFor.MIDI:
                    waitCount = 15;
                    btnPleaseWait.Text = "Please wait while looking for MIDI devices...";
                    pb_WaitingProgress.Progress = 0;
                    break;
                case WaitingFor.INTEGRA_7:
                    btnPleaseWait.Text = "Please wait while finding INTEGRA-7 module(s)...";
                    break;
                case WaitingFor.EDIT:
                    btnPleaseWait.Text = "Please wait while initiating editor form...";
                    break;
                case WaitingFor.READING_STUDIO_SET:
                    btnPleaseWait.Text = "Please wait while reading studio set.";
                    pb_WaitingProgress.Progress = 0;
                    break;
                case WaitingFor.READING_STUDIO_SET_NAMES:
                    if (continueTo == CurrentPage.EDIT_STUDIO_SET)
                    {
                        btnPleaseWait.Text = "Please wait while scanning Studio set names and initiating studio set editor form...";
                    }
                    else
                    {
                        btnPleaseWait.Text = "Please wait while scanning Studio set names...";
                    }
                    pb_WaitingProgress.Progress = 0;
                    studioSetNumber = 0;
                    break;
            }
        }

        public void DrawPleaseWaitPage()
        {
            // Create all controls ------------------------------------------------------------------------
            grid_PleaseWait = new Grid();
            grid_PleaseWait.VerticalOptions = LayoutOptions.FillAndExpand;
            for (Int32 i = 0; i < 18; i++)
            {
                grid_PleaseWait.RowDefinitions.Add(new RowDefinition());
            }
            grid_PleaseWaitTop = new Grid();
            grid_PleaseWaitTop.VerticalOptions = LayoutOptions.FillAndExpand;
            grid_PleaseWaitBottom = new Grid();
            grid_PleaseWaitBottom.VerticalOptions = LayoutOptions.FillAndExpand;
            pb_WaitingProgress = new ProgressBar();
            pb_WaitingProgress.IsEnabled = true;
            btnPleaseWait = new Button();
            PleaseWait_StackLayout = new StackLayout();
            PleaseWait_StackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            PleaseWait_StackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            PleaseWait_StackLayout.IsVisible = false;

            // Assemble grids with controls ---------------------------------------------------------------
            grid_PleaseWait.Children.Add(new GridRow(0, new View[] { grid_PleaseWaitTop }, null, false, false, 8));
            grid_PleaseWait.Children.Add(new GridRow(8, new View[] { btnPleaseWait }));
            grid_PleaseWait.Children.Add(new GridRow(9, new View[] { pb_WaitingProgress }));
            grid_PleaseWait.Children.Add(new GridRow(10, new View[] { grid_PleaseWaitBottom }, null, false, false, 8));
            btnPleaseWait.HorizontalOptions = LayoutOptions.FillAndExpand;
            btnPleaseWait.VerticalOptions = LayoutOptions.FillAndExpand;
            btnPleaseWait.Margin = new Thickness(0);
            grid_PleaseWait.Margin = new Thickness(2);

            // Assemble PleaseWait_StackLayout --------------------------------------------------------------
            PleaseWait_StackLayout.Children.Add(new GridRow(0, new View[] { grid_PleaseWait }));
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Handlers
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void PleaseWait_Timer_Tick()
        {
            switch (waitingFor)
            {
                case WaitingFor.MIDI:
                    divider--;
                    if (divider < 1)
                    {
                        divider = 10;
                    }
                    if (MidiState != MIDIState.WAITING_FOR_INITIONALIZATION)
                    {
                        // There are await calls in some MIDI implementations, so this thread
                        // must not call again before the await calls finishes, thus the
                        // test above.
                        if (divider % 10 == 0)
                        {
                            PleaseWait_FindMidiInterfaces();
                        }
                    }
                    break;
                case WaitingFor.INTEGRA_7:
                    if (waitingState == WaitingState.DONE)
                    {
                        ContinueToPage();
                    }
                    divider--;
                    if (divider < 1)
                    {
                        divider = 10;
                    }
                    if (divider % 10 == 0)
                    {
                        PleaseWait_WaitForIntegra_7();
                    }
                    break;
                case WaitingFor.EDIT:
                    PleaseWait_InitializeEditorForm();
                    break;
                case WaitingFor.READING_STUDIO_SET:
                    if (waitingState == WaitingState.INITIALIZING)
                    {
                        PleaseWait_ReadStudioSet();
                    }
                    else if (waitingState == WaitingState.PROCESSING)
                    {
                        EditStudioSet_Timer_Tick();
                    }
                    else if (waitingState == WaitingState.DONE)
                    {
                        ContinueToPage();
                        //ShowMotionalSurroundPage();
                    }
                    break;
                case WaitingFor.READING_STUDIO_SET_NAMES:
                    if (waitingState == WaitingState.DONE)
                    {
                        ContinueToPage();
                    }
                    else
                    {
                        PleaseWait_ReadStudioSetNames();
                    }
                    break;
            }

            switch (uiAction)
            {
                case UIAction.PROGRESS:
                    pb_WaitingProgress.Progress += progressStep; // 1F / 64F;
                    uiAction = UIAction.NONE;
                    break;
                case UIAction.GOTO_LIBRARIAN:
                    PleaseWait_StackLayout.IsVisible = false;
                    Librarian_StackLayout.IsVisible = true;
                    currentPage = CurrentPage.LIBRARIAN;
                    studioSetNamesJustRead = StudioSetNames.READ_BUT_NOT_LISTED;
                    initDone = true;
                    break;
                case UIAction.GOTO_MOTIONAL_SURROUND:
                    PleaseWait_StackLayout.IsVisible = false;
                    MotionalSurround_StackLayout.IsVisible = true;
                    currentPage = CurrentPage.MOTIONAL_SURROUND;
                    break;
                case UIAction.GOTO_EDIT_STUDIO_SET:
                    PleaseWait_StackLayout.IsVisible = false;
                    StudioSetEditor_StackLayout.IsVisible = true;
                    currentPage = CurrentPage.EDIT_STUDIO_SET;
                    PopulateStudioSetSelector();
                    break;
            }
            uiAction = UIAction.NONE;
        }

        private async void PleaseWait_FindMidiInterfaces()
        {
            if (commonState.Midi == null)
            {
                // This should have been done in UIHandler.Init() but sometimes fails.
                commonState.Midi = DependencyService.Get<IMidi>();
                MidiState = MIDIState.NOT_INITIALIZED;
            }
            else if (MidiState == MIDIState.NOT_INITIALIZED)
            {
                waitCount--;
                pb_WaitingProgress.Progress += ((1 - pb_WaitingProgress.Progress) / 10000);
                if (waitCount > 10)
                {
                    // Try to find I-7 via USB interface:
                    if (appType == _appType.ANDROID)
                    {
                        if (!commonState.Midi.MidiIsReady())
                        {
                            MidiState = MIDIState.WAITING_FOR_INITIONALIZATION;
                            commonState.Midi.Init(mainPage, "INTEGRA-7", deviceSpecifics, 0, 0);
                            MidiState = MIDIState.INITIALIZING;
                        }
                    }
                    else
                    {
                        MidiState = MIDIState.WAITING_FOR_INITIONALIZATION;
                        commonState.Midi.Init(mainPage, "INTEGRA-7", 0, 0);
                        MidiState = MIDIState.INITIALIZING;
                    }
                    pb_WaitingProgress.Progress += ((1 - pb_WaitingProgress.Progress) / 10000);
                    if (commonState.Midi.MidiIsReady())
                    {
                        pb_WaitingProgress.Progress = 1;
                        MidiState = MIDIState.INITIALIZED;
                    }
                }
                else
                {
                    commonState.Midi.MakeMidiDeviceList();
                    if (commonState.Midi.GetMidiDeviceList().Count > 0)
                    {
                        String MidiInterfaceName;
                        if (mainPage.LoadLocalValue("MidiDevice") != null && waitCount > 5)
                        {
                            // See if a connection via 5-pin connector and another MIDI device has worked before
                            MidiInterfaceName = (String)mainPage.LoadLocalValue("MidiDevice");
                            commonState.Midi.Init(mainPage, MidiInterfaceName, 0, 0);
                            if (commonState.Midi.MidiIsReady())
                            {
                                MidiState = MIDIState. INITIALIZED;
                            }
                            pb_WaitingProgress.Progress += ((1 - pb_WaitingProgress.Progress) / 10000);
                        }
                        else
                        {
                            // Hold this timer loop (MIDIState.WAITING_FOR_I7 does not do anything) and ask the user
                            // for a MIDI interface to use:
                            MidiState = MIDIState.WAITING_FOR_I7;
                            mainPage.SaveLocalValue("MidiDevice", null);
                            commonState.Midi.MakeMidiDeviceList();
                            MidiInterfaceName = await mainPage.DisplayActionSheet("Please select MIDI interface:",
                                "Close", null, commonState.Midi.GetMidiDeviceList().ToArray());
                            if (MidiInterfaceName == "Close")
                            {
                                MidiState = MIDIState.NO_MIDI_INTERFACE_AVAILABLE;
                            }
                            else
                            {
                                mainPage.SaveLocalValue("MidiDevice", MidiInterfaceName);
                                commonState.Midi.Init(mainPage, MidiInterfaceName, 0, 0);
                                pb_WaitingProgress.Progress += ((1 - pb_WaitingProgress.Progress) / 10000);
                                if (commonState.Midi.MidiIsReady())
                                {
                                    MidiState = MIDIState.INITIALIZED;
                                }
                                else
                                {
                                    MidiState = MIDIState.NOT_INITIALIZED;
                                }
                            }
                        }
                    }
                    else
                    {
                        // No MIDI devices found!
                    }
                }
            }
            else if (MidiState == MIDIState.INITIALIZING)
            {
                if (commonState.Midi.MidiIsReady())
                {
                    pb_WaitingProgress.Progress = 1;
                    MidiState = MIDIState.INITIALIZED;
                }
                else
                {
                    waitCount--;
                    if (waitCount < 5)
                    {
                        MidiState = MIDIState.NO_MIDI_INTERFACE_AVAILABLE;
                    }
                    else
                    {
                        waitCount = 15;
                        MidiState = MIDIState.NOT_INITIALIZED;
                    }
                }
            }

            else if (MidiState == MIDIState.WAITING_FOR_I7)
            {
                pb_WaitingProgress.Progress += ((1 - pb_WaitingProgress.Progress) / 10000);
            }

            else if (MidiState == MIDIState.INITIALIZED)
            {
                waitingFor = WaitingFor.INTEGRA_7;
                CheckForIntegra_7_readiness();
            }

            else if (MidiState == MIDIState.NO_MIDI_INTERFACE_AVAILABLE)
            {
                PleaseWait_StackLayout.IsVisible = false;
                waitingState = WaitingState.DONE;
                currentPage = CurrentPage.LIBRARIAN;
            }
        }

        private void PleaseWait_WaitForIntegra_7()
        {
            waitCount--;
            if (integra_7Ready)
            {
                // We got a response from I-7, so we are ready to show the Librarian page now:
                PleaseWait_StackLayout.IsVisible = false;
                ShowLibrarianPage();
            }
            else if (waitCount > 0)
            {
                // No response, ask again and wait bit more:
                CheckForIntegra_7_readiness();
            }
            else
            {
                // We got no response. In case we have a saved MIDI
                // interface name, it is no longer valid, erase it...
                mainPage.SaveLocalValue("MidiDevice", null);
                // ...and try again:
                MidiState = MIDIState.NOT_INITIALIZED;
                waitingFor = WaitingFor.MIDI;
            }
        }

        private void PleaseWait_InitializeEditorForm()
        {
        }

        /// <summary>
        /// We need to read the studio set for one of two reasons:
        /// 1) We wish to enter Motional Surround and need to synchronize
        ///     data with actual data from the I-7
        /// 2) We wish to enter Studio Set Editor and need to synchronize
        ///     data with actual data from the I-7
        /// </summary>
        private void PleaseWait_ReadStudioSet()
        {
            progressStep = 1.0F / 22.0F;
            if (waitingState == WaitingState.INITIALIZING)
            {
                waitingState = WaitingState.PROCESSING;
                pb_WaitingProgress.Progress = 0;
                initDone = false;
                commonState.StudioSet = new StudioSet();
                QueryCurrentStudioSetNumber(false);
            }
        }

        /// <summary>
        /// We need to read the studio set names for one of two reasons:
        /// 1) Studio sets has been selected in Librarian_lvGroups and
        ///     we should list groups in the Librarian
        /// 2) The button Librarian_btnEditStudioSet has been pressed and
        ///     we need to list the groups in the EditStudioSet group selector
        ///     and in the free slots selector of the Studio Set Editor.
        /// If this has already been done, commonState.StudioSetNames is
        /// filled out, and we should not come here at all.
        /// I.e. we should come here zero or one time per session, never more.
        /// The enumerated variable continueTo will be used to navigate to
        /// the correct page after names are read.
        /// </summary>
        private void PleaseWait_ReadStudioSetNames()
        {
            progressStep = 1.0F / 63.0F;
            if (waitingState == WaitingState.INITIALIZING)
            {
                waitingState = WaitingState.PROCESSING;
                pb_WaitingProgress.Progress = 0;
                initDone = false;
                commonState.StudioSetNames = new List<String>();
                studioSetNumberTemp = 0;
                ScanForStudioSetNames();
            }


            //queryType = QueryType.STUDIO_SET_NAMES;
            //ShowStudioSetEditorPage();
            //pb_WaitingProgress.Progress = 0;
        }

        /// <summary>
        /// MIDI received messages are currently sent here, to the PleaseWait page, 
        /// where is is handled in accordance to what we are waiting for.
        /// </summary>
        public void PleaseWait_MidiInPort_MessageReceived()
        {
            if (queryType == QueryType.CHECKING_I_7_READINESS)
            {
                // Got response from the I-7, just set the integra_7Ready flag:
                integra_7Ready = true;
            }
            else if (waitingFor == WaitingFor.READING_STUDIO_SET_NAMES
                && waitingState == WaitingState.PROCESSING)
            {
                uiAction = UIAction.PROGRESS;
                String text = "";
                for (Int32 i = 0x0b; i < rawData.Length - 2; i++)
                {
                    text += (char)rawData[i];
                }
                commonState.StudioSetNames.Add(text);
                studioSetNumberTemp++;

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
                    studioSetNamesJustRead = StudioSetNames.READ_BUT_NOT_LISTED;
                    SetStudioSet(commonState.CurrentStudioSet);
                    waitingState = WaitingState.DONE;
                }


                // StudioSetEditor_currentStudioSetEditorMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER_AND_SCAN

                //uiAction = UIAction.PROGRESS;
                //EditStudioSet_MidiInPort_MessageReceived();

                //if (studioSetEditor_State == StudioSetEditor_State.DONE)
                //{
                //    // Studio set names has been read. Go to the Librarian
                //    // or the studio set editor depending on what is in 
                //    // the enumertor continueTo:
                //    if (continueTo == CurrentPage.LIBRARIAN)
                //    {
                //        uiAction = UIAction.GOTO_LIBRARIAN;
                //    }
                //    else if (continueTo == CurrentPage.EDIT_STUDIO_SET)
                //    {
                //        // PleaseWait has had the control so far, so take
                //        // it and let Studio set editor be visible.
                //        uiAction = UIAction.GOTO_EDIT_STUDIO_SET;
                //    }
                //}
            }
            else if (waitingFor == WaitingFor.READING_STUDIO_SET
                && waitingState == WaitingState.PROCESSING)
            {
                //EditStudioSet_MidiInPort_MessageReceived();
                uiAction = UIAction.PROGRESS;
                switch (currentStudioSetEditorMidiRequest)
                {
                    case StudioSetEditor_currentStudioSetEditorMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER:
                        commonState.CurrentSoundMode = rawData[11];
                        commonState.CurrentStudioSet = rawData[17];
                        studioSetNumberTemp = 0;
                        QuerySystemCommon();
                        break;
                    case StudioSetEditor_currentStudioSetEditorMidiRequest.SYSTEM_COMMON:
                        ReadSystemCommon(false);
                        QueryStudioSetCommon();
                        break;
                    case StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_COMMON:
                        ReadSelectedStudioSet(false);
                        QueryStudioSetChorus();
                        break;
                    case StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_CHORUS:
                        ReadStudioSetChorus(false);
                        QueryStudioSetReverb();
                        break;
                    case StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_REVERB:
                        ReadStudioSetReverb(false);
                        QueryStudioSetMotionalSurround();
                        break;
                    case StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MOTIONAL_SURROUND:
                        ReadMotionalSurround(false);
                        QueryStudioSetMasterEQ();
                        break;
                    case StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MASTER_EQ:
                        ReadStudioSetMasterEQ(false);
                        studioSetEditor_PartToRead = 0;
                        QueryStudioSetPart(studioSetEditor_PartToRead);
                        break;
                    case StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART:
                        ReadStudioSetPart(studioSetEditor_PartToRead, false);
                        studioSetEditor_PartToRead++;
                        if (studioSetEditor_PartToRead < 16)
                        {
                            QueryStudioSetPart(studioSetEditor_PartToRead);
                        }
                        else
                        {
                            QueryStudioSetPartToneName();
                        }
                        break;
                    case StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART_TONE_NAME:
                        ReadStudioSetPartToneName(false);
                        initDone = true;
                        uiAction = UIAction.NONE;
                        waitingState = WaitingState.DONE;
                        break;

                }
                
            }
        }

        private void ContinueToPage()
        {
            switch (continueTo)
            {
                case CurrentPage.EDIT_STUDIO_SET:
                    // If we are going to the studio set editor, it might be that
                    // also studio set names must be read:
                    if (commonState.StudioSetNames == null || commonState.StudioSetNames.Count < 1)
                    {
                        waitingFor = WaitingFor.READING_STUDIO_SET_NAMES;
                        waitingState = WaitingState.INITIALIZING;
                        btnPleaseWait.Text = "Please wait while scanning Studio set names and initiating studio set editor form...";
                        PleaseWait_ReadStudioSetNames();
                    }
                    else
                    {
                        PleaseWait_StackLayout.IsVisible = false;
                        ShowStudioSetEditorPage();
                    }
                    break;
                case CurrentPage.EDIT_TONE:
                    PleaseWait_StackLayout.IsVisible = false;
                    ShowToneEditorPage();
                    break;
                case CurrentPage.FAVORITES:
                    currentPage = CurrentPage.FAVORITES;
                    PleaseWait_StackLayout.IsVisible = false;
                    Favorites_StackLayout.IsVisible = true;
                    break;
                case CurrentPage.LIBRARIAN:
                    currentPage = CurrentPage.LIBRARIAN;
                    PleaseWait_StackLayout.IsVisible = false;
                    Librarian_StackLayout.IsVisible = true;
                    if (waitingFor == WaitingFor.READING_STUDIO_SET_NAMES)
                    {
                        Librarian_PopulateStudioSetListview();
                    }
                    break;
                case CurrentPage.MOTIONAL_SURROUND:
                    PleaseWait_StackLayout.IsVisible = false;
                    ShowMotionalSurroundPage();
                    break;
            }
        }

        private void CheckForIntegra_7_readiness()
        {
            queryType = QueryType.CHECKING_I_7_READINESS;
            byte[] address = new byte[] { 0x01, 0x00, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x01 };
            byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            commonState.Midi.SendSystemExclusive(message);
        }
    }
}
