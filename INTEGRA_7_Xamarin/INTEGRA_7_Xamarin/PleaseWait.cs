﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integra_7_Xamarin
{
    public enum WaitingFor
    {
        MIDI,
        INTEGRA_7,
        EDIT,
        READING_STUDIO_SET_NAMES,
        IDLE
    }

    public partial class UIHandler
    {
        //public ProgressBar ProgressBar { get; set; }

        private WaitingFor waitingFor;
        private CurrentPage continueTo;
        private Int32 waitCount;
        private Int32 studioSetNumber;
        private Object deviceSpecifics; 

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
                    PleaseWait_ReadStudioSetNames();
                    break;
            }
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
                            commonState.Midi.Init(mainPage, "INTEGRA-7", Librarian_midiOutputDevice, Librarian_midiInputDevice, deviceSpecifics, 0, 0);
                        }
                    }
                    else
                    {
                        commonState.Midi.Init(mainPage, "INTEGRA-7", Librarian_midiOutputDevice, Librarian_midiInputDevice, 0, 0);
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
                            commonState.Midi.Init(mainPage, MidiInterfaceName, Librarian_midiOutputDevice, Librarian_midiInputDevice, 0, 0);
                            if (commonState.Midi.MidiIsReady())
                            {
                                MidiState = MIDIState. INITIALIZED;
                            }
                            pb_WaitingProgress.Progress = pb_WaitingProgress.Progress + ((1 - pb_WaitingProgress.Progress) / 100);
                        }
                        else
                        {
                            // Hold this timer loop (MIDIState.INITIALIZING does not do anything) and ask the user
                            // for a MIDI interface to use:
                            MidiState = MIDIState.INITIALIZING;
                            mainPage.SaveLocalValue("MidiDevice", null);
                            commonState.Midi.MakeMidiDeviceList();
                            MidiInterfaceName = await mainPage.DisplayActionSheet("Please select MIDI interface:",
                                "Close", null, commonState.Midi.GetMidiDeviceList().ToArray());
                            mainPage.SaveLocalValue("MidiDevice", MidiInterfaceName);
                            commonState.Midi.Init(mainPage, MidiInterfaceName, Librarian_midiOutputDevice, Librarian_midiInputDevice, 0, 0);
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
                    else
                    {
                        // No MIDI devices found!
                    }
                }
            }
            else if (MidiState == MIDIState.INITIALIZING)
            {
            }

            else if (MidiState == MIDIState.INITIALIZED)
            {
                waitingFor = WaitingFor.INTEGRA_7;
                CheckForIntegra_7_readiness();
            }
        }

        private void PleaseWait_WaitForIntegra_7()
        {
            waitCount--;
            if (integra_7Ready)
            {
                PleaseWait_StackLayout.IsVisible = false;
                ShowLibrarianPage();
            }
            else if (waitCount > 0)
            {
                CheckForIntegra_7_readiness();
            }
            else
            {
                mainPage.SaveLocalValue("MidiDevice", null);
                MidiState = MIDIState.NOT_INITIALIZED;
                waitingFor = WaitingFor.MIDI;
            }
        }

        private void PleaseWait_InitializeEditorForm()
        {
        }

        private void PleaseWait_ReadStudioSetNames()
        {
            waitingFor = WaitingFor.IDLE;
            queryType = QueryType.STUDIO_SET_NAMES;
            ShowStudioSetEditorPage();
            pb_WaitingProgress.Progress = 0;
        }

        public void PleaseWait_MidiInPort_MessageReceived()
        {
            if (queryType == QueryType.CHECKING_I_7_READINESS)
            {
                integra_7Ready = true;
            }
            else if (queryType == QueryType.STUDIO_SET_NAMES)
            {
                pb_WaitingProgress.Progress += 1F / 64F;
                EditStudioSet_MidiInPort_MessageReceived();

                if (studioSetEditor_State == StudioSetEditor_State.DONE)
                {
                    if (continueTo == CurrentPage.LIBRARIAN)
                    {
                        currentPage = CurrentPage.LIBRARIAN;
                        PleaseWait_StackLayout.IsVisible = false;
                        Librarian_StackLayout.IsVisible = true;
                        studioSetNamesJustRead = StudioSetNames.READ_BUT_NOT_LISTED;
                        initDone = true;
                    }
                    else
                    {
                        // PleaseWait has had the control so far, so take it and let Studio set editor be visible.
                        currentPage = CurrentPage.EDIT_STUDIO_SET;
                        PleaseWait_StackLayout.IsVisible = false;
                        initDone = true;
                        StudioSetEditor_StackLayout.IsVisible = true;
                        commonState.StudioSetNames = new List<String>();
                        QueryCurrentStudioSetNumber();
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
