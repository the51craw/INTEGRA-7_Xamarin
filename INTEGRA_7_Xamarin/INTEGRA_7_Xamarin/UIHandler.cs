using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;

namespace INTEGRA_7_Xamarin
{
    /**
     * All pages are dynamically created here.
     * They all exists simultaneously, but shown one at a time by assigning it to mainStackLayout.
     * Some pages has subpages that are generated in MakeDynamicControls.cs, Help.cs and
     * ControlHandlers.cs
     */
    public partial class UIHandler
    {
        private HBTrace t = new HBTrace("UIHandler public sealed partial class MainPage : Page");
        Boolean handleControlEvents = true;            // Some control events re-creates the control, and that will cause a loop. Use handleControlEvents to prevent that.
        List<Boolean> previousHandleControlEvents = new List<Boolean>();

        public enum _appType
        {
            UWP,
            IOS,
            MacOS,
            ANDROID,
        }

        public enum CurrentPage
        {
            PLEASE_WAIT,
            LIBRARIAN,
            MOTIONAL_SURROUND,
            FAVORITES,
            EDIT_TONE,
            EDIT_STUDIO_SET,
        }

        public enum MIDIState
        {
            NOT_INITIALIZED,
            WAITING_FOR_INITIONALIZATION,
            INITIALIZING,
            WAITING_FOR_I7,
            INITIALIZING_FAILED,
            MIDI_NOT_AVAILABLE,
            INITIALIZED,
            NO_MIDI_INTERFACE_AVAILABLE
        }

        public enum QueryType
        {
            NONE,
            CHECKING_I_7_READINESS,
            PCM_SYNTH_TONE_COMMON,
            PCM_SYNTH_TONE_COMMON2,
            PCM_DRUM_KIT_COMMON,
            SUPERNATURAL_ACOUSTIC_TONE_COMMON,
            SUPERNATURAL_SYNTH_TONE_COMMON,
            SUPERNATURAL_DRUM_KIT_COMMON,
            PCM_KEY_NAME,
            SND_KEY_NAME,
            CURRENT_SELECTED_STUDIO_SET,
            CURRENT_SELECTED_TONE,
            READ_TONE_FROM_I7,
            GET_CURRENT_STUDIO_SET_NUMBER_AND_SCAN,
            GET_CURRENT_STUDIO_SET_NUMBER,
            STUDIO_SET_NAMES,
        }

        public enum NeedsToSetFontSizes
        {
            NONE,
            LIBRARIAN,
            FAVORITES,
            EDIT,
            EDIT_STUDIO_SET,
            MOTIONAL_SURROUND
        }

        //public object MainPage_Device { get; set; }
        Boolean scanAll = false;
        UInt16 emptySlots = 10;

        SuperNATURALDrumKitInstrumentList superNATURALDrumKitInstrumentList = new SuperNATURALDrumKitInstrumentList();
        Boolean showCurrentToneReadFromI7 = false;

        //ApplicationDataContainer localSettings = null;
        public CommonState commonState = null;
        //public IMyFileIO myFileIO = null;
        public System.Collections.Generic.List<System.Collections.Generic.List<String>> Lists = null;
        //private Boolean AutoUpdateChildLists = true;
        //private Int32 currentGroupIndex = -1;
        //private Int32 currentCategoryIndex = -1;
        //private Int32 currentToneNameIndex = -1;
        private byte currentNote = 255; // > 127 when note is off
        public static String[] lines;
        // private DrumKeyAssignLists drumKeyAssignLists = null; Moved to commonState
        private Int32 transpose = 0;
        private byte[] notes = { 36, 40, 43, 48 };
        private byte[] drumNotes = { 36, 38, 42, 45, 43, 41,
            60, 61, 56, 69, 70, 54, 65, 76, 77,
            46, 44, 51, 53, 52, 67, 68, 69, 70 };
        private byte[] whiteKeys = { 36, 38, 40, 41, 43, 45,
            47, 48, 50, 52, 53, 55, 57, 59, 60, 62, 64, 65,
            67, 69, 71, 72, 74, 76, 77, 79, 81, 83, 84, 86,
            88, 89, 91, 93, 95, 96 };
        private Boolean initDone = false;
        private Boolean scanning = false;
        //private DispatcherTimer timer = null;
        private Boolean followOutputPort = false;
        private Boolean followingOutputPort = false;
        private Boolean waitingForInputPortChange = false;
        private String outputPortId;
        private Int32 savedInputPort = -1;
        private Int32 testingInputPortId;
        private byte msb;
        private byte lsb;
        private byte pc;
        private byte key;
        //String toneName;
        //String category;
        byte toneCategory;
        Int32 userToneIndex;
        Boolean integra_7Ready = false;
        Boolean waitingForMidiResponse = false;
        //byte integra_7ReadyCounter = 100;
        //SolidColorBrush green = null;
        //SolidColorBrush gray = null;
        //Boolean initMidi = false;
        UInt16[] userToneNumbers;
        public QueryType queryType { get; set; }
        Boolean updateToneName = false;
        ToneCategories toneCategories = new ToneCategories();
        Hex2Midi hex2Midi = new Hex2Midi();
        //SuperNATURALDrumKitInstrumentList superNATURALDrumKitInstrumentList = new SuperNATURALDrumKitInstrumentList();
        public byte[] rawData;
        Double lastfontSize = 15;
        public static Boolean StopTimer = false;
        public static Int32 minimumHeightRequest = 14;
        private NeedsToSetFontSizes needsToSetFontSizes = NeedsToSetFontSizes.NONE;

        MainPage mainPage;
        public StackLayout mainStackLayout { get; set; }
        public StackLayout PleaseWait_StackLayout = null;
        public Boolean PleaseWait_IsCreated = false;
        public StackLayout Librarian_StackLayout = null;
        public Boolean Librarian_IsCreated = false;
        public StackLayout Edit_StackLayout = null;
        public StackLayout StudioSetEditor_StackLayout = null;
        public StackLayout Favorites_StackLayout = null;
        public StackLayout MotionalSurround_StackLayout = null;
        public Boolean Edit_IsCreated = false;
        public Boolean EditStudioSet_IsCreated = false;
        public Boolean MotionalSurround_IsCreated = false;
        public Boolean Favorites_IsCreated = false;
        public MIDIState MidiState { get; set; }
        public IMyFileIO myFileIO { get; set; }


        public static _appType appType;
        //public static ColorSettings colorSettings { get; set; }
        public static ColorSettings colorSettings = new ColorSettings(_colorSettings.LIGHT);
        //public static BorderThicknesSettings borderThicknesSettings { get; set; }
        public static BorderThicknesSettings borderThicknesSettings = new BorderThicknesSettings(2);
        CurrentPage currentPage;
        Int32 divider = 1; // Used to make timer slower by skipping ticks

        // Edit tone controls:
        //...

        // Constructor
        public UIHandler(StackLayout mainStackLayout, INTEGRA_7_Xamarin.MainPage mainPage)
        {
            this.mainStackLayout = mainStackLayout;
            this.mainPage = mainPage;
            //MainPage_Device = mainPage.MainPage_Device;
            Init();
        }

        public void Init()
        {
            currentPage = CurrentPage.LIBRARIAN;
            MidiState = MIDIState.NOT_INITIALIZED;
            //colorSettings = new ColorSettings(_colorSettings.LIGHT);
            borderThicknesSettings = new BorderThicknesSettings(2);
            commonState = new CommonState(ref Librarian_btnPlay);
            commonState.Midi = DependencyService.Get<IMidi>();
            myFileIO = DependencyService.Get<IMyFileIO>();
            rawData = new byte[0];
            //IDeviceDependent deviceDependent;
            userToneNumbers = new UInt16[128];
            for (byte i = 0; i < 128; i++)
            {
                userToneNumbers[i] = 0;
            }
            StartTimer();
            initDone = true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Handlers common to all pages
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This timer is used in all pages.
        /// enum Page controls which page's timer routine is called.
        /// </summary>
        public void StartTimer()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(1), () =>
                {
                    if (StopTimer)
                    {
                        return false;
                    }
                    switch (currentPage)
                    {
                        case CurrentPage.PLEASE_WAIT:
                            PleaseWait_Timer_Tick();
                            break;
                        case CurrentPage.LIBRARIAN:
                            Librarian_Timer_Tick();
                            break;
                        case CurrentPage.EDIT_TONE:
                            Edit_Timer_Tick();
                            break;
                        case CurrentPage.EDIT_STUDIO_SET:
                            EditStudioSet_Timer_Tick();
                            break;
                        case CurrentPage.MOTIONAL_SURROUND:
                            MotionalSurround_Timer_Tick();
                            break;
                    }

                    switch (needsToSetFontSizes)
                    {
                        case NeedsToSetFontSizes.LIBRARIAN:
                            needsToSetFontSizes = NeedsToSetFontSizes.NONE;
                            SetFontSizes(Librarian_StackLayout);
                            break;
                        case NeedsToSetFontSizes.FAVORITES:
                            needsToSetFontSizes = NeedsToSetFontSizes.NONE;
                            SetFontSizes(Favorites_StackLayout);
                            break;
                        case NeedsToSetFontSizes.EDIT:
                            needsToSetFontSizes = NeedsToSetFontSizes.NONE;
                            SetFontSizes(Edit_StackLayout);
                            break;
                        case NeedsToSetFontSizes.EDIT_STUDIO_SET:
                            needsToSetFontSizes = NeedsToSetFontSizes.NONE;
                            SetFontSizes(StudioSetEditor_StackLayout);
                            break;
                        case NeedsToSetFontSizes.MOTIONAL_SURROUND:
                            needsToSetFontSizes = NeedsToSetFontSizes.NONE;
                            SetFontSizes(MotionalSurround_StackLayout);
                            break;
                    }

                    return true;
                });
            });
        }

        //public static void KillTimer()
        //{
        //    StopTimer = true;
        //}

        /// <summary>
        /// Device-specific classes fills out rawData and then calls MidiInPort_MessageRecceived().
        /// </summary>
        public void MidiInPort_MessageRecceived()
        {
            if (rawData.Length > 0)
            {
                waitingForMidiResponse = false;
                switch (currentPage)
                {
                    case CurrentPage.PLEASE_WAIT:
                        PleaseWait_MidiInPort_MessageReceived();
                        break;
                    case CurrentPage.LIBRARIAN:
                        Librarian_MidiInPort_MessageReceived();
                        break;
                    case CurrentPage.EDIT_TONE:
                        Edit_MidiInPort_MessageReceived();
                        break;
                    case CurrentPage.EDIT_STUDIO_SET:
                        EditStudioSet_MidiInPort_MessageReceived();
                        break;
                    case CurrentPage.MOTIONAL_SURROUND:
						MotionalSurround_MidiInPort_MessageReceived();
                        break;
                }
                //rawData = new byte[0];
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Functions common to all pages
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void PushHandleControlEvents(Boolean setHandleControlEvents = false)
        {
            previousHandleControlEvents.Add(handleControlEvents);
            handleControlEvents = setHandleControlEvents;
        }

        private void PopHandleControlEvents()
        {
            if (previousHandleControlEvents.Count > 0)
            {
                handleControlEvents = previousHandleControlEvents[previousHandleControlEvents.Count - 1];
                previousHandleControlEvents.RemoveAt(previousHandleControlEvents.Count - 1);
            }
            else
            {
                handleControlEvents = true;
            }
        }

        public void SetFontSizes(StackLayout stackLayout)
        {
            if (stackLayout == null)
            {
                switch (currentPage)
                {
                    case CurrentPage.PLEASE_WAIT:
                        needsToSetFontSizes = NeedsToSetFontSizes.NONE;
                        stackLayout = PleaseWait_StackLayout;
                        break;
                    case CurrentPage.LIBRARIAN:
                        needsToSetFontSizes = NeedsToSetFontSizes.NONE;
                        stackLayout = Librarian_StackLayout;
                        break;
                    case CurrentPage.FAVORITES:
                        needsToSetFontSizes = NeedsToSetFontSizes.NONE;
                        stackLayout = Favorites_StackLayout;
                        break;
                    case CurrentPage.EDIT_TONE:
                        needsToSetFontSizes = NeedsToSetFontSizes.NONE;
                        stackLayout = Edit_StackLayout;
                        break;
                    case CurrentPage.EDIT_STUDIO_SET:
                        needsToSetFontSizes = NeedsToSetFontSizes.NONE;
                        stackLayout = StudioSetEditor_StackLayout;
                        break;
                    case CurrentPage.MOTIONAL_SURROUND:
                        needsToSetFontSizes = NeedsToSetFontSizes.NONE;
                        stackLayout = MotionalSurround_StackLayout;
                        for (Int32 i = 17; i > 0; i--)
                        {
                            mslPart[i - 1].Plot(gArrows.Width, gArrows.Height);
                        }
                        break;
                }
            }

            if (stackLayout.Children != null && stackLayout.Children.Count > 0)
            {
                Double size = stackLayout.Height / 50F;
                if (stackLayout.Width < stackLayout.Height * 1.25)
                {
                    size = stackLayout.Width / 65F;
                }

                if (size > 0 && size != lastfontSize)
                {
                    lastfontSize = size;
                    foreach (View view in stackLayout.Children)
                    {
                        SetFontSize(view, size);
                    }
                }
            }
        }

        public void SetFontSize(View view, Double size)
        {
            if (view.GetType() == typeof(Button))
            {
                ((Button)view).FontSize = size;
            }
            else if (view.GetType() == typeof(Label))
            {
                ((Label)view).FontSize = size;
            }
            else if (view.GetType() == typeof(MyLabel))
            {
                ((MyLabel)view).Label.FontSize = size;
            }
            else if (view.GetType() == typeof(LabeledSwitch))
            {
                ((LabeledSwitch)view).LSLabel.FontSize = size;
            }
            else if (view.GetType() == typeof(LabeledPicker))
            {
                ((LabeledPicker)view).Label.FontSize = size;
            }
            else if (view.GetType() == typeof(LabeledText))
            {
                ((LabeledText)view).Label.FontSize = size;
                ((LabeledText)view).text.FontSize = size;
            }
            else if (view.GetType() == typeof(LabeledTextInput))
            {
                ((LabeledTextInput)view).Label.FontSize = size;
                ((LabeledTextInput)view).Editor.FontSize = size;
            }
            else if (view.GetType() == typeof(TextBlock))
            {
                ((TextBlock)view).FontSize = size;
                ((TextBlock)view).FontSize = size;
            }
            else if (view.GetType() == typeof(Image))
            {
                ((Image)view).WidthRequest = ((Grid)((Image)view).Parent).Width;
                ((Image)view).HeightRequest = ((Grid)((Image)view).Parent).Height;
            }
            else if (view.GetType() == typeof(TouchableImage))
            {
                ((TouchableImage)view).WidthRequest = ((Grid)((TouchableImage)view).Parent).Width;
                ((TouchableImage)view).HeightRequest = ((Grid)((TouchableImage)view).Parent).Height;
            }
            //else if (view.GetType() == typeof(ListView))
            //{
            //    ((ListView)view).SetValue(C ItemTemplate.f SetValue(Font.Default, size);
            //}
            //else if (view.GetType() == typeof(ComboBox))
            //{
            //    foreach (ComboBoxItem cbi in ((ComboBox)view).Items)
            //    {
            //        ((String)cbi.Content).
            //    }
            //    ((ComboBox)view).SetValue(C ItemTemplate.f SetValue(Font.Default, size);
            //}
            else if (view.GetType() == typeof(StackLayout))
            {
                foreach (View subView in ((StackLayout)view).Children)
                {
                    SetFontSize(subView, size);
                }
            }
            else if (view.GetType() == typeof(Grid))
            {
                foreach (View subView in ((Grid)view).Children)
                {
                    SetFontSize(subView, size);
                }
            }
            else
            {
                t.Trace("UIHandler line 309, missing type: " + view.GetType().ToString());
            }
        }

        private void SetStudioSet(byte number)
        {
            t.Trace("private void SetStudioSet (" + "byte[]" + number + ", " + ")");
            //commonState.CurrentStudioSet = number;
            commonState.Midi.ProgramChange((byte)15, (byte)85, (byte)0, (byte)(number + 1));
        }

        //public void DrawPleaseWaitPage()
        //{
        //    /*
        //    <Grid x:Name="PleaseWaitWhileScanning" Visibility="Collapsed">
        //        <ProgressBar Name="Progress" Margin="10,25,10,0" IsIndeterminate="true" Foreground="Green" HorizontalAlignment="Stretch" VerticalAlignment="Stretch" />
        //        <TextBlock Name="txtScanning"  HorizontalAlignment="Center" VerticalAlignment="Center" />
        //    </Grid>
        //    */
        //    grid_PleaseWait = new Grid();
        //    pb_WaitingProgress = new ProgressBar();
        //    pb_WaitingProgress.IsEnabled = true;
        //    tbPleaseWait = new TextBlock();
        //    tbPleaseWait.Text = "Please wait while scanning Studio set names and initiating form...";
        //    PleaseWait_StackLayout = new StackLayout();
        //    grid_PleaseWait.Children.Add(new GridRow(0, new View[] { tbPleaseWait }).Row);
        //    grid_PleaseWait.Children.Add(new GridRow(1, new View[] { pb_WaitingProgress }).Row);
        //    PleaseWait_StackLayout.Children.Add(new GridRow(0, new View[] { grid_PleaseWait }).Row);
        //    PleaseWait_StackLayout.IsVisible = false;
        //}

        // TODO: Think about this, Tablets and phones normally do not have a cursor, 
        // but can have when mouse is connected via OTG! Still has no waitcursor.
        // In those cases image UI might have some 'disabled' look and not respond to tapping.
        // This is platform dependent! UWP and MacOS definitely can show a waitcursor.
        private void Waiting(Boolean on, String WaitText = "", StackLayout stackLayout = null)
        {
            //t.Trace("private void Waiting(" + on.ToString() + ")");
            // Maybe also test for platform and use different methods?
            //if (stackLayout != null && PleaseWait_StackLayout != null)
            //{
            //    if (on)
            //    {
            //        stackLayout.IsVisible = false;
            //        ((TextBlock)((Grid)((Grid)((Grid)PleaseWait_StackLayout.Children[0]).
            //            Children[0]).Children[0]).Children[0]).Text = WaitText;
            //        PleaseWait_StackLayout.IsVisible = true;
            //        //Window.Current.CoreWindow.PointerCursor =
            //        //    new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 1);
            //    }
            //    else
            //    {
            //        stackLayout.IsVisible = true;
            //        PleaseWait_StackLayout.IsVisible = false;
            //        //Window.Current.CoreWindow.PointerCursor =
            //        //    new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
            //    }
            //}
        }

        private async void ShowMessage(String message)
        {
            await mainPage.DisplayAlert("INTEGRA_7 Librarian", message, "Ok");
        }
    }
}
