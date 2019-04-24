using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public enum WaitingFor
    {
        CONNECTION,
        USB,
        MIDI,
        SEARCH_MIDI_DEVICES,
        WAITING_FOR_INTEGRA_7,
        INTEGRA_7_FOUND,
        INTEGRA_7_CONNECTION_FOUND,
        INTEGRA_7_NOT_FOUND,
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
        private Object deviceSpecifics;
        private UIAction uiAction;
        private Double progressStep;
        private Int32 connectionCurrentlyUnderTest;
        private List<String> possibleConnections;
        private Boolean gotResponseFromI7 = false;

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
                case WaitingFor.CONNECTION:
                    waitingFor = WaitingFor.IDLE; // To avoid that Timer_Tick messes with our initialization
                    btnPleaseWait.Text = "Please wait while connecting to your INTEGRA-7...";
                    ConnectToIntegra7();
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
                    break;
            }
        }

        /**
         * How to connect?
         * 
         * Start
         *   Preferred connection present in settings and activated? (automatic selection)
         *      Yes
         *          Can connect?
         *              Yes
         *                  Use connection
         *              No
         *                  Search connections
         *                  Number of connections possible:
         *                      0: Failed, inform user.
         *                      1: Use the available connection.
         *                     >1: Ask user
         *                          
         *      No
         *                  Search connections
         *                  Number of connections possible:
         *                      0: Failed, inform user.
         *                      1: Use the available connection.
         *                     >1: Ask user
         */
        private void ConnectToIntegra7()
        {
            if (!String.IsNullOrEmpty(preferredConnection) && automaticSelectConnection)
            {
                // Try using preferred connedtion:
                TryConnectViaPreferredConnection();
            }
            else
            {
                // Look for USB and MIDI devices that can connect:
                FindPossibleConnections();
            }
        }

        private async void TryConnectViaPreferredConnection()
        {
            //System.Diagnostics.Debug.WriteLine("Connecting... TryConnectViaPreferredConnection()");
            await commonState.Midi.Init(mainPage, preferredConnection, 0, 0);
            if (commonState.Midi.MidiIsReady())
            {
                //System.Diagnostics.Debug.WriteLine("Connecting... commonState.Midi.MidiIsReady");
                AskForIntegra_7_response();
                waitingFor = WaitingFor.WAITING_FOR_INTEGRA_7;
            }
        }

        private async void FindPossibleConnections()
        {
            //System.Diagnostics.Debug.WriteLine("Connecting... FindPossibleConnections()");
            // First we make a list of devices that possibly could be used to connect to I-7:
            await commonState.Midi.MakeMidiDeviceList();
            possibleConnections = commonState.Midi.GetMidiDeviceList();

            // If MIDI interfaces were found, test them one by one to see if I - 7 responds via respective connection:
            if (possibleConnections.Count > 0)
            {
                //System.Diagnostics.Debug.WriteLine("Connecting... possibleConnections.Count > 0");
                connectionCurrentlyUnderTest = 0;
                AskIntegra7ForResponseOnCurrentConnection();
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine("Connecting... else?");
                // 
            }
        }

        private async void AskIntegra7ForResponseOnCurrentConnection()
        {
            //System.Diagnostics.Debug.WriteLine("Connecting... AskIntegra7ForResponseOnCurrentConnection()");
            commonState.Midi.ResetMidi();
            await commonState.Midi.Init(mainPage, possibleConnections[connectionCurrentlyUnderTest], 0, 0);
            if (commonState.Midi.MidiIsReady())
            {
                //System.Diagnostics.Debug.WriteLine("Connecting... commonState.Midi.MidiIsReady");
                AskForIntegra_7_response();
            }
        }

        private async void PresentListToUser()
        {
            waitingFor = WaitingFor.IDLE;
            if (possibleConnections.Count > 0)
            {
                String response = await mainPage.DisplayActionSheet("Please select connection to use:", "Cancel", null, possibleConnections.ToArray());
                commonState.Midi.ResetMidi();
                pb_WaitingProgress.Progress = 1;
                if (response == "Cancel")
                {
                    btnPleaseWait.Text = "Please close the app.";
                }
                else
                {
                    preferredConnection = response;
                    mainPage.SaveLocalValue("PreferredConnection", preferredConnection);
                    await commonState.Midi.Init(mainPage, preferredConnection, 0, 0);
                    // The list only contains connections that are tested to work, so...
                    ContinueToPage();
                }
            }
            else
            {
                if (await mainPage.DisplayAlert("INTEGRA-7 Librarian and Editor",
                    "There was no way to contact INTEGRA-7. Please check your equipment. " +
                    "The INTEGRA-7 needs to have either a USB " +
                    "connection, or a connection via a MIDI interface with both MIDI IN " +
                    "and MIDI OUT connected.", "Try again", "Give up"))
                {
                    pb_WaitingProgress.Progress = 0;
                    commonState.Midi.ResetMidi();
                    pb_WaitingProgress.Progress = 0;
                    ConnectToIntegra7();
                }
                else
                {
                    btnPleaseWait.Text = "Please close the app.";
                    pb_WaitingProgress.Progress = 1;
                }
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

        public async void PleaseWait_Timer_Tick()
        {
            switch (waitingFor)
            {
                case WaitingFor.WAITING_FOR_INTEGRA_7:
                    //HoldTimer();
                    waitCount--;
                    pb_WaitingProgress.Progress += 0.005;
                    //System.Diagnostics.Debug.WriteLine("Connecting... case WaitingFor.WAITING_FOR_INTEGRA_7: waitCount = " + waitCount.ToString());
                    //AskForIntegra_7_response();
                    if (waitCount > 0)
                    {
                        if (gotResponseFromI7)
                        {
                            waitingFor = WaitingFor.IDLE;
                            gotResponseFromI7 = false;
                            if (automaticSelectConnection)
                            {
                                //System.Diagnostics.Debug.WriteLine("Connecting... ContinueToPage()");
                                pb_WaitingProgress.Progress = 1;
                                ContinueToPage();
                            }
                            else
                            {
                                //System.Diagnostics.Debug.WriteLine("Connecting... ");
                                connectionCurrentlyUnderTest++;
                                if (connectionCurrentlyUnderTest < possibleConnections.Count)
                                {
                                    //System.Diagnostics.Debug.WriteLine("Connecting... AskIntegra7ForResponseOnCurrentConnection()");
                                    AskIntegra7ForResponseOnCurrentConnection();
                                }
                                else
                                {
                                    //System.Diagnostics.Debug.WriteLine("Connecting... PresentListToUser()");
                                    PresentListToUser();
                                }
                            }
                        }
                    }
                    if (waitCount < 1)
                    {
                        waitingFor = WaitingFor.IDLE;
                        if (automaticSelectConnection)
                        {
                            waitingFor = WaitingFor.INTEGRA_7_NOT_FOUND;
                        }
                        else
                        {
                            possibleConnections.RemoveAt(connectionCurrentlyUnderTest);
                            if (connectionCurrentlyUnderTest < possibleConnections.Count)
                            {
                                commonState.Midi.ResetMidi();
                                AskIntegra7ForResponseOnCurrentConnection();
                            }
                            else
                            {
                                commonState.Midi.ResetMidi();
                                PresentListToUser();
                            }
                        }
                    }
                    //ReleaseTimer();
                    break;
                case WaitingFor.INTEGRA_7_NOT_FOUND:
                    //HoldTimer();
                    waitingFor = WaitingFor.IDLE;
                    if (await mainPage.DisplayAlert("INTEGRA-7 Librarian and Editor",
                        "There was no way to contact INTEGRA-7. Please check your equipment. " +
                        "The INTEGRA-7 needs to have either a USB " +
                        "connection, or a connection via a MIDI interface with both MIDI IN " +
                        "and MIDI OUT connected.", "Try again", "Give up"))
                    {
                        commonState.Midi.ResetMidi();
                        pb_WaitingProgress.Progress = 0;
                        ConnectToIntegra7();
                    }
                    else
                    {
                        btnPleaseWait.Text = "Please close the app.";
                        pb_WaitingProgress.Progress = 1;
                    }
                    //ReleaseTimer();
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

        private void PleaseWait_WaitForIntegra_7()
        {
            waitCount--;
            btnPleaseWait.Text = "Please wait while finding INTEGRA-7 module(s)...";
            if (waitingFor == WaitingFor.INTEGRA_7_FOUND)
            {
                // We got a response from I-7, so we are ready to show the Librarian page now:
                PleaseWait_StackLayout.IsVisible = false;
                ShowLibrarianPage();
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
            if (waitingFor == WaitingFor.WAITING_FOR_INTEGRA_7)
            {
                //System.Diagnostics.Debug.WriteLine("Connecting... gotResponseFromI7 = true");
                // Got response from the I-7, just set the integra_7Ready flag:
                gotResponseFromI7 = true;
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
                    //Librarian_StackLayout.IsVisible = true;
                    ShowLibrarianPage();
                    if (waitingFor == WaitingFor.READING_STUDIO_SET_NAMES)
                    {
                        Librarian_PopulateStudioSetListview();
                        Librarian_lvGroups.SelectedItem = "Studio sets";
                    }
                    break;
                case CurrentPage.MOTIONAL_SURROUND:
                    PleaseWait_StackLayout.IsVisible = false;
                    ShowMotionalSurroundPage();
                    break;
            }
        }

        private void AskForIntegra_7_response()
        {
            byte[] address = new byte[] { 0x01, 0x00, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x01 };
            byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            waitCount = 100;
            waitingFor = WaitingFor.WAITING_FOR_INTEGRA_7;
            commonState.Midi.SendSystemExclusive(message);
        }

        //private void HoldTimer()
        //{
        //    holdTimer = true;
        //}

        //private void ReleaseTimer()
        //{
        //    holdTimer = false;
        //}
    }
}
