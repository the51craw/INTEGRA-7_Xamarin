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
        READING_STUDIO_SET_NAMES,
        IDLE
    }

    public enum UIAction
    {
        NONE,
        PROGRESS,
        GOTO_LIBRARIAN,
        GOTO_EDIT_STUDIO_SET
    }

    public partial class UIHandler
    {
        //public ProgressBar ProgressBar { get; set; }

        private WaitingFor waitingFor;
        private CurrentPage continueTo;
        private Int32 waitCount;
        private Int32 studioSetNumber;
        private Object deviceSpecifics;
        private UIAction uiAction;

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
            PleaseWait_StackLayout.IsVisible = true;
            this.waitingFor = waitingFor;
            uiAction = UIAction.NONE;
            PleaseWait_Init();
            //this.o = o;
        }

        private void PleaseWait_Init()
        {
            switch (waitingFor)
            {
                case WaitingFor.MIDI:
                    waitCount = 15;
                    tbPleaseWait.Text = "Please wait while looking for MIDI devices...";
                    pb_WaitingProgress.Progress = 0;
                    break;
                case WaitingFor.INTEGRA_7:
                    tbPleaseWait.Text = "Please wait while finding INTEGRA-7 module(s)...";
                    break;
                case WaitingFor.EDIT:
                    tbPleaseWait.Text = "Please wait while initiating editor form...";
                    break;
                case WaitingFor.READING_STUDIO_SET_NAMES:
                    if (continueTo == CurrentPage.EDIT_STUDIO_SET)
                    {
                        tbPleaseWait.Text = "Please wait while scanning Studio set names and initiating studio set editor form...";
                    }
                    else
                    {
                        tbPleaseWait.Text = "Please wait while scanning Studio set names...";
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
            pb_WaitingProgress = new ProgressBar();
            pb_WaitingProgress.IsEnabled = true;
            tbPleaseWait = new TextBlock();
            PleaseWait_StackLayout = new StackLayout();
            PleaseWait_StackLayout.IsVisible = false;

            // Assemble grids with controls ---------------------------------------------------------------
            grid_PleaseWait.Children.Add(new GridRow(0, new View[] { tbPleaseWait }).Row);
            grid_PleaseWait.Children.Add(new GridRow(1, new View[] { pb_WaitingProgress }).Row);

            // Assemble PleaseWait_StackLayout --------------------------------------------------------------
            PleaseWait_StackLayout.Children.Add(new GridRow(0, new View[] { grid_PleaseWait }).Row);
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
                    if (divider % 10 == 0)
                    {
                        PleaseWait_FindMidiInterfaces();
                    }
                    break;
                case WaitingFor.INTEGRA_7:
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
                case WaitingFor.READING_STUDIO_SET_NAMES:
                    waitingFor = WaitingFor.IDLE;
                    PleaseWait_ReadStudioSetNames();
                    break;
            }

            switch (uiAction)
            {
                case UIAction.PROGRESS:
                    pb_WaitingProgress.Progress += 1F / 64F;
                    break;
                case UIAction.GOTO_LIBRARIAN:
                    PleaseWait_StackLayout.IsVisible = false;
                    Librarian_StackLayout.IsVisible = true;
                    currentPage = CurrentPage.LIBRARIAN;
                    studioSetNamesJustRead = StudioSetNames.READ_BUT_NOT_LISTED;
                    initDone = true;
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
                pb_WaitingProgress.Progress = pb_WaitingProgress.Progress + ((1 - pb_WaitingProgress.Progress) / 100);
                if (waitCount > 10)
                {
                    // Try to find I-7 via USB interface:
                    if (appType == _appType.ANDROID)
                    {
                        if (!commonState.Midi.MidiIsReady())
                        {
                            commonState.Midi.Init(mainPage, "INTEGRA-7", deviceSpecifics, 0, 0);
                        }
                    }
                    else
                    {
                        MidiState = MIDIState.INITIALIZING;
                        commonState.Midi.Init(mainPage, "INTEGRA-7", 0, 0);
                    }
                    pb_WaitingProgress.Progress = pb_WaitingProgress.Progress + ((1 - pb_WaitingProgress.Progress) / 100);
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
                            pb_WaitingProgress.Progress = pb_WaitingProgress.Progress + ((1 - pb_WaitingProgress.Progress) / 100);
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
                                pb_WaitingProgress.Progress = pb_WaitingProgress.Progress + ((1 - pb_WaitingProgress.Progress) / 100);
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
            }

            else if (MidiState == MIDIState.WAITING_FOR_I7)
            {
            }

            else if (MidiState == MIDIState.INITIALIZED)
            {
                waitingFor = WaitingFor.INTEGRA_7;
                CheckForIntegra_7_readiness();
            }

            else if (MidiState == MIDIState.NO_MIDI_INTERFACE_AVAILABLE)
            {
                PleaseWait_StackLayout.IsVisible = false;
                ShowLibrarianPage();
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
            queryType = QueryType.STUDIO_SET_NAMES;
            ShowStudioSetEditorPage();
            pb_WaitingProgress.Progress = 0;
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
            else if (queryType == QueryType.STUDIO_SET_NAMES)
            {
                uiAction = UIAction.PROGRESS;
                EditStudioSet_MidiInPort_MessageReceived();

                if (studioSetEditor_State == StudioSetEditor_State.DONE)
                {
                    // Studio set names has been read. Go to the Librarian
                    // or the studio set editor depending on what is in 
                    // the enumertor continueTo:
                    if (continueTo == CurrentPage.LIBRARIAN)
                    {
                        uiAction = UIAction.GOTO_LIBRARIAN;
                    }
                    else if (continueTo == CurrentPage.EDIT_STUDIO_SET)
                    {
                        // PleaseWait has had the control so far, so take
                        // it and let Studio set editor be visible.
                        uiAction = UIAction.GOTO_EDIT_STUDIO_SET;
                    }
                }
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
