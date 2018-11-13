using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integra_7_Xamarin
{
    /**
     * All pages are dynamically created here.
     * They all exists simultaneously, but shown one at a time by assigning it to mainStackLayout.
     * Some pages has subpages that are generated in MakeDynamicControls.cs, Help.cs and
     * ControlHandlers.cs
     */
    public partial class UIHandler
    {
        //private HBTrace t = new HBTrace("UIHandler public sealed partial class MainPage : Page");
        Boolean handleControlEvents = true;            // Some control events re-creates the control, and that will cause a loop. Use handleControlEvents to prevent that.
        Boolean previousHandleControlEvents = true;

        public enum _appType
        {
            UWP,
            IOS,
            MacOS,
            ANDROID,
        }

        enum _page
        {
            LIBRARIAN,
            MOTIONAL_SURROUND,
            FAVORITES,
            EDIT_TONE,
            EDIT_STUDIO_SET,
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
        }

        public object MainPage_Device { get; set; }
        Boolean scanAll = false;
        UInt16 emptySlots = 10;

        SuperNATURALDrumKitInstrumentList superNATURALDrumKitInstrumentList = new SuperNATURALDrumKitInstrumentList();

        //ApplicationDataContainer localSettings = null;
        public CommonState commonState = null;
        //public IMyFileIO myFileIO = null;
        public System.Collections.Generic.List<System.Collections.Generic.List<String>> Lists = null;
        private Boolean AutoUpdateChildLists = true;
        private Int32 currentGroupIndex = -1;
        private Int32 currentCategoryIndex = -1;
        private Int32 currentToneNameIndex = -1;
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
        String toneName;
        String category;
        byte toneCategory;
        Int32 userToneIndex;
        Boolean integra_7Ready = false;
        byte integra_7ReadyCounter = 100;
        //SolidColorBrush green = null;
        //SolidColorBrush gray = null;
        Boolean initMidi = false;
        UInt16[] userToneNumbers;
        public QueryType queryType { get; set; }
        Boolean updateToneNames = false;
        ToneCategories toneCategories = new ToneCategories();
        Hex2Midi hex2Midi = new Hex2Midi();
        //SuperNATURALDrumKitInstrumentList superNATURALDrumKitInstrumentList = new SuperNATURALDrumKitInstrumentList();
        public byte[] rawData;
        Int32 lastfontSize = 15;

        Integra_7_Xamarin.MainPage mainPage;
        public StackLayout mainStackLayout { get; set; }
        public StackLayout LibrarianStackLayout = null;
        public Boolean LibrarianIsCreated = false;
        public StackLayout EditorStackLayout = null;
        public StackLayout StudioSetEditorStackLayout = null;
        public StackLayout FavoritesStackLayout = null;
        public Boolean EditorIsCreated = false;
        public Boolean StudioSetEditorIsCreated = false;
        public Boolean FavoritesIsCreated = false;


        public static _appType appType;
        public static ColorSettings colorSettings { get; set; }
        public static BorderThicknesSettings borderThicknesSettings { get; set; }
        _page page;

        // Edit tone controls:
        //...

        // Constructor
        public UIHandler(StackLayout mainStackLayout, Integra_7_Xamarin.MainPage mainPage)
        {
            this.mainStackLayout = mainStackLayout;
            this.mainPage = mainPage;
            MainPage_Device = mainPage.MainPage_Device;
            Init();
        }

        public void Init()
        {
            page = _page.LIBRARIAN;
            colorSettings = new ColorSettings(_colorSettings.LIGHT);
            borderThicknesSettings = new BorderThicknesSettings(2);
            commonState = new CommonState(ref Librarian_btnPlay);
            commonState.midi = DependencyService.Get<IMidi>();
            rawData = new byte[0];
            IDeviceDependent deviceDependent;
            initDone = true;
        }

        /// <summary>
        /// Device-specific classes fills out rawData and then calls MidiInPort_MessageRecceived().
        /// </summary>
        public void MidiInPort_MessageRecceived()
        {
            if (rawData.Length > 0)
            {
                switch (page)
                {
                    case _page.LIBRARIAN:
                        Librarian_MidiInPort_MessageReceived();
                        break;
                    case _page.EDIT_TONE:
                        Edit_MidiInPort_MessageReceived();
                        break;
                }
                rawData = new byte[0];
            }
        }

        public void SetFontSizes(StackLayout stackLayout)
        {
            if (stackLayout.Children != null && stackLayout.Children.Count > 0)
            {
                Int32 size = (Int32)(stackLayout.Width < stackLayout.Height * 1.25 ? stackLayout.Width / 80 : stackLayout.Height / 70);
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

        public void SetFontSize(View view, Int32 size)
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
                ((LabeledSwitch)view).Label.FontSize = size;
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
        }

        //private void Waiting(Boolean on)
        //{
        //    switch (appType)
        //    {
        //        case _appType.ANDROID:
        //            break;
        //        case _appType.IOS:
        //            break;
        //        case _appType.MacOS:
        //            break;
        //        case _appType.UWP:
        //            Waiting(on);
        //            break;
        //    }
        //    t.Trace("private void Waiting (" + on + ")");
        //    if (on)
        //    {
        //        EditTonesGrid.Cursor
        //        Window.Current.CoreWindow.PointerCursor =
        //            new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 1);
        //    }
        //    else
        //    {
        //        Window.Current.CoreWindow.PointerCursor =
        //            new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        //    }
        //}

        private async void ShowMessage(String message)
        {
            await mainPage.DisplayAlert("INTEGRA_7 Librarian", message, "Ok");
        }
    }
}
