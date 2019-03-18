using System;
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
        READING_STUDIO_SET_NAMES
    }

    public partial class UIHandler
    {
        public ProgressBar ProgressBar { get; set; }

        private WaitingFor waitingFor;
        private Object o;

        /// <summary>
        /// Call to show a wait page with a progress bar
        /// </summary>
        /// <param name="waitingFor">Controls text and behaviour</param>
        /// <param name="o">Pass any object needed if applicable</param>
        public void ShowPleaseWaitPage(WaitingFor waitingFor, Object o = null)
        {
            if (!PleaseWait_IsCreated)
            {
                DrawPleaseWaitPage();
                PleaseWait_StackLayout.MinimumWidthRequest = 1;
                mainStackLayout.Children.Add(PleaseWait_StackLayout);
                PleaseWait_Init();
                PleaseWait_IsCreated = true;
            }
            Page = _page.PLEASE_WAIT;
            PleaseWait_StackLayout.IsVisible = true;
            this.waitingFor = waitingFor;
            this.o = o;
        }

        private void PleaseWait_Init()
        {
            switch (waitingFor)
            {
                case WaitingFor.MIDI:
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
                    tbPleaseWait.Text = "Please wait while scanning Studio set names and initiating studio set editor form...";
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
                    PleaseWait_FindMidiInterfaces();
                    break;
                case WaitingFor.INTEGRA_7:
                    PleaseWait_FindIntegra_7();
                    break;
                case WaitingFor.EDIT:
                    PleaseWait_InitializeEditorForm();
                    break;
                case WaitingFor.READING_STUDIO_SET_NAMES:
                    PleaseWait_ReadStudioSetNames();
                    break;
            }
        }

        private void PleaseWait_FindMidiInterfaces()
        {
            if (commonState.midi == null)
            {
                // This should have been done in UIHandler.Init() but sometimes fails.
                commonState.midi = DependencyService.Get<IMidi>();
                MidiState = MIDIState.NOT_INITIALIZED;
            }
            else if (MidiState == MIDIState.NOT_INITIALIZED)
            {
                commonState.midi.Init(mainPage, "INTEGRA-7", Librarian_midiOutputDevice, Librarian_midiInputDevice, 0, 0);
                pb_WaitingProgress.Progress = pb_WaitingProgress.Progress + ((1 - pb_WaitingProgress.Progress) / 2);
                if (commonState.midi.MidiIsReady())
                {
                    pb_WaitingProgress.Progress = 1;
                    MidiState = MIDIState.INITIALIZED;
                }
            }
            else if (MidiState == MIDIState.INITIALIZED)
            {
                PleaseWait_StackLayout.IsVisible = false;
                ShowLibrarianPage();
            }
            //else if (!midiIsInitiated && midi.MIDIState == MIDIState.INITIALIZING_FAILED && getMidiTries > 100)
            //{
            //    // There does not seem to be any MIDI here
            //    midi.MIDIState = MIDIState.MIDI_NOT_AVAILABLE;
            //}
            //else if (!midiIsInitiated && midi.MIDIState == MIDIState.INITIALIZED)
            //{
            //    midiIsInitiated = true;
            //    midi.midiInPort.MessageReceived += MidiInPort_MessageReceived;
            //    timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            //}
        }

        private void PleaseWait_FindIntegra_7()
        {
        }

        private void PleaseWait_InitializeEditorForm()
        {
        }

        private void PleaseWait_ReadStudioSetNames()
        {
        }

        public void PleaseWait_MidiInPort_MessageReceived()
        {

        }

    }
}
