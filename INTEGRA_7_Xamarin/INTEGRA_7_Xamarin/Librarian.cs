using Plugin.Clipboard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
//using INTEGRA_7_Xamarin.Helpers;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public partial class UIHandler
    {
        enum ToneNamesFilter
        {
            ALL = 0,
            PRESET = 1,
            USER = 2,
            INIT = 3,
        }

        enum StudioSetNames
        {
            NOT_READ,
            READ_BUT_NOT_LISTED,
            READ_AND_LISTED
        }

        ToneNamesFilter toneNamesFilter = ToneNamesFilter.INIT;
        Boolean toneNamesRead = false;
        StudioSetNames studioSetNamesJustRead = StudioSetNames.NOT_READ;
        Boolean putBankInClipboard = false;
        Boolean updateIntegra7 = false;
        Boolean needToSetFontSize = true;
        //public Double x { get; set; }
        //public Double y { get; set; }

        Dictionary<String, String> localSettings = new Dictionary<String, String>();

        // Librarian controls:
        //public Picker Librarian_midiOutputDevice { get; set; }
        //public Picker Librarian_midiInputDevice { get; set; }
        public Picker Librarian_midiOutputChannel { get; set; }
        public Picker Librarian_midiInputChannel { get; set; }
        //public Image Librarian_Keyboard { get; set; }

        Grid Librarian_gridMain;
        Grid Librarian_gridGroups;
        Button Librarian_lblGroups;
        ListView Librarian_lvGroups;
        ObservableCollection<String> Librarian_ocGroups;
        Grid Librarian_gridCategories;
        Button Librarian_lblCategories;
        ListView Librarian_lvCategories;
        ObservableCollection<String> Librarian_ocCategories;
        //Button Librarian_lblStudioSets;
        //ListView Librarian_lvStudioSets;
        //ObservableCollection<String> Librarian_ocStudioSets;
        Grid Librarian_gridTones;
        Button Librarian_filterPresetAndUser;
        ListView Librarian_lvToneNames;
        ObservableCollection<String> Librarian_ocToneNames;
        //ListView Librarian_lvSearchResult;
        //ObservableCollection<String> Librarian_SearchResult;
        //Grid Librarian_gridToneData;
        Grid Librarian_gridKeyboard;
        Button Librarian_btnSearch;
        Editor Librarian_tbSearch;
        LabeledText Librarian_ltToneName;
        LabeledText Librarian_ltType;
        LabeledText Librarian_ltToneNumber;
        LabeledText Librarian_ltBankAddress;
        LabeledText Librarian_ltPatchMSB;
        LabeledText Librarian_ltPatchLSB;
        LabeledText Librarian_ltProgramNumber;
        Button Librarian_btnSettings;
        Button Librarian_btnEditTone;
        Button Librarian_btnEditStudioSet;
        Button Librarian_btnResetVolume;
        Button Librarian_btnMotionalSurround;
        Button Librarian_btnAddFavorite;
        Button Librarian_btnRemoveFavorite;
        Button Librarian_btnPlay;
        Button Librarian_btnShowFavorites;
        Button Librarian_btnResetHangingNotes;
        Button Librarian_btnPlus12keys;
        Button Librarian_btnMinus12keys;
        Button Librarian_btnKeys;
        Boolean usingSearchResults = false;
        Int32 rowHeight;
        byte listingHeight;
        byte keyTranspose = 0;
        private Int32 lowKey = 36;
        byte currentStudioSet;

        // Buttons for the keyboard:
        PianoKey[] Librarian_btnWhiteKeys;
        PianoKey[] Librarian_btnBlackKeys;

        private void Librarian_Init()
        {
            t.Trace("private void Librarian_Init()");
            try
            {
                // ToDo: Is this really needed? It is taken care of in OnNavigatedTo, or...
                //if (commonState == null)
                //{
                //    commonState = new CommonState(ref Librarian_btnPlay);
                //}
                //else
                //{
                //    commonState.Player.btnPlayStop = Librarian_btnPlay;
                //    if (commonState.Player.Playing)
                //    {
                //        Librarian_btnPlay.Content = "Stop";
                //    }
                //}

                CheckForVenderDriver();

                // Get index of selected tone:
                //if (commonState.currentTone == null)
                //{
                //    commonState.currentTone = new Tone();
                //    if (commonState.toneList.Tones.Count() > 925)
                //    {
                //        // This will be PCM Synth Tone, Ac.Piano, 128voicePno (normal init for PCM Synth Tone)
                //        commonState.currentTone.Group = commonState.toneList.Tones[925][0];
                //        commonState.currentTone.Category = commonState.toneList.Tones[925][1];
                //        commonState.currentTone.Name = commonState.toneList.Tones[925][3];
                //        commonState.currentTone.Index = 925;
                //    }
                //    else
                //    {
                //        commonState.currentTone.Category = commonState.toneList.Tones[0][1];
                //        commonState.currentTone.Name = commonState.toneList.Tones[0][3];
                //        commonState.currentTone.Index = 0;
                //    }
                //}

                //ReadSettings(); // Already done in UIHandler

                // Populate lvGroups:
                foreach (List<String> tone in commonState.ToneList.Tones)
                {
                    if (!Librarian_ocGroups.Contains(tone[0]))
                    {
                        Librarian_ocGroups.Add(tone[0]);
                    }
                }
                Librarian_ocGroups.Add("Studio sets");

                // Populate lvCategories and tone names:
                //PopulateCategories();
                //PopulateToneNames();

                // Fill out form:
                //PopulateToneData(commonState.currentTone.Index);
                //commonState.midi.SetVolume(commonState.CurrentPart, 127);
                //UpdateDrumNames();
                //Librarian_lvGroups.SelectedItem = commonState.currentTone.Group;
                //commonState.currentTone.GroupIndex = Librarian_Groups.IndexOf((String)Librarian_lvGroups.SelectedItem);
                //if (commonState.favoritesList == null)
                //{
                //    //ReadFavorites();
                //}
            }
            catch { }

            initDone = true;
            //cbChannel.SelectedIndex = commonState.CurrentPart;
            EnableOrDisableEditButton();

            waitingForMidiResponse = true;
            QuerySelectedStudioSet();
        }

        private void EnableOrDisableEditButton()
        {
            try
            {
                //Librarian_btnEditTone.IsEnabled = cbChannel.SelectedIndex == 0 && InstrumentSettings.Editable[Librarian_lvGroups.SelectedIndex];
                if (Librarian_lvGroups.SelectedItem != null && Librarian_ocGroups.IndexOf((String)Librarian_lvGroups.SelectedItem) > -1)
                {
                    Librarian_btnEditTone.IsEnabled = InstrumentSettings.Editable[Librarian_ocGroups.IndexOf((String)Librarian_lvGroups.SelectedItem)];
                }
            }
            catch { }
        }

        private async void CheckForVenderDriver()
        {
            commonState.VenderDriverIsInstalled = commonState.Midi.VenderDriverDetected();
            if (commonState.VenderDriverIsInstalled)
            {
                Librarian_btnPlay.IsEnabled = false;
                await mainPage.DisplayAlert("Vender driver detected.",
                    "\r\nPlay (Preview), Save and Delete functionality is not available while using Vender driver." +
                    "\r\n(Vender driver enables USB MIDI + AUDIO.)" +
                    "\r\nIf you do not use audio over USB, you can temporarily disable Vender driver like this:" +
                    "\r\nOn INTEGRA-7 press 'Menu', select 'System', select 'Setup' tab and set USB driver to 'GENERIC'." +
                    "\r\nRestart INTEGRA-7 and then restart this app." +
                    "\r\nYou might consider temporarily disabling the Vender driver while editing the INTERA-7 for full functionality.",
                     "Ok");
                //initDone = true;
            }
        }

        private void ReadSettings()
        {
            t.Trace("private void ReadSettings()");
        //private String PreferredConnection; // Preffered midi device name or "USB" to use to connect to I-7
        //private Boolean AutomaticSelectConnection; // Allows user to select connection when multiple connections are available.
            if (mainPage.LoadLocalValue("PreferredConnection") != null)
            {
                try
                {
                    String temp = (String)mainPage.LoadLocalValue("PreferredConnection");
                    preferredConnection = temp;
                }
                catch
                {
                    preferredConnection = "";
                    mainPage.SaveLocalValue("PreferredConnection", preferredConnection);
                }
            }
            if (mainPage.LoadLocalValue("AutomaticSelectConnection") != null)
            {
                try
                {
                    String temp = (String)mainPage.LoadLocalValue("AutomaticSelectConnection");
                    automaticSelectConnection = temp == "True";
                }
                catch
                {
                    automaticSelectConnection = true;
                    mainPage.SaveLocalValue("AutomaticSelectConnection", "False");
                }
            }
            else
            {
                automaticSelectConnection = true;
                mainPage.SaveLocalValue("AutomaticSelectConnection", "False");
            }
            if (mainPage.LoadLocalValue("MidiChannel") != null)
            {
                try
                {
                    Int32 temp = (Int32)mainPage.LoadLocalValue("MidiChannel");
                    commonState.CurrentPart = (byte)temp;
                }
                catch
                {
                    commonState.CurrentPart = 0;
                    mainPage.SaveLocalValue("MidiChannel", 0);
                }
            }
            else
            {
                commonState.CurrentPart = 0;
                mainPage.SaveLocalValue("MidiChannel", 0);
            }
            if (mainPage.LoadLocalValue("Transpose") != null)
            {
                try
                {
                    transpose = ((Int32)mainPage.LoadLocalValue("Transpose"));
                    lowKey = transpose + 36;
                    //Librarian_btnKeys.Text = "Keys " + lowKey.ToString() + " - " + (lowKey + 36).ToString();
                }
                catch
                {
                    transpose = 0;
                    mainPage.SaveLocalValue("Transpose", 0);
                    lowKey = transpose + 36;
                    //Librarian_btnKeys.Text = "Keys " + lowKey.ToString() + " - " + (lowKey + 36).ToString();
                }
            }
            else
            {
                transpose = 0;
                mainPage.SaveLocalValue("Transpose", 0);
                lowKey = transpose + 36;
                //Librarian_btnKeys.Text = "Keys " + lowKey.ToString() + " - " + (lowKey + 36).ToString();
            }
            if (mainPage.LoadLocalValue("PutBankInClipboard") != null)
            {
                try
                {
                    putBankInClipboard = (Boolean)mainPage.LoadLocalValue("PutBankInClipboard");
                }
                catch
                {
                    mainPage.SaveLocalValue("PutBankInClipboard", false);
                    putBankInClipboard = false;
                }
            }
            else
            {
                mainPage.SaveLocalValue("PutBankInClipboard", false);
                putBankInClipboard = false;
            }
            Settings_ReadUserColorSettings();
            switch (CurrentColorScheme)
            {
                case 0:
                    colorSettings = new ColorSettings(_colorSettings.LIGHT);
                    break;
                case 1:
                    colorSettings = new ColorSettings(_colorSettings.DARK);
                    break;
                case 2:
                    colorSettings = Settings_UserColorSettings;
                    break;
            }
            SetStackLayoutColors(Librarian_StackLayout);
        }

        public void DrawLibrarianPage()
        {
            HBTrace t = new HBTrace("DrawLibrarianPage");

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Librarian 
            // New suggested layout since we cannot write vertical text on keyboard:
            // |-------------------------------------------------------------------------------------------| 
            // | lblGroups            | lblCategories        | filterPresetAndUser  | Keyboard             |
            // |----------------------|----------------------|----------------------|                      |
            // | lvGroups             | lvCategories         | lvToneNames or       |                      |
            // |                      |                      | lvSearchResult       |                      |
            // |                      |                      |                      |                      |
            // |                      |                      |                      |                      |
            // |                      |                      |                      |                      |
            // |                      |                      |                      |                      |
            // |                      |                      |                      |                      |
            // |                      |                      |                      |                      |
            // |                      |                      |                      |                      |
            // |                      |                      |                      |                      |
            // |                      |                      |                      |                      |
            // |                      |                      |                      |                      |  
            // |                      |                      |                      |                      |
            // |                      |                      |                      |                      |
            // |----------------------|----------------------|----------------------|                      |
            // |midiOutputDevice/part |ltBankNumber          |Edit tone| studioset  |                      |
            // |----------------------|----------------------|----------------------|                      |
            // |tbSearch              |ltBankMSB             |Motion.sur.           |                      |
            // |----------------------|----------------------|----------------------|                      |
            // |ltToneName            |ltBankLSB             |Favorites|Add  |Remove|                      |
            // |----------------------|----------------------|----------------------|                      |
            // |ltType                |ltProgramNumber       |Reset hanging  |volume|                      |
            // |----------------------|----------------------|----------------------|                      |
            // |ltToneNumber          | play                 | keyrange |+12k | -12k|                      |
            // |-------------------------------------------------------------------------------------------| 

            // Create all controls ------------------------------------------------------------------------

            // Make a listview lvGroups for column 0:
            Librarian_lblGroups = new Button();
            Librarian_lblGroups.Text = "Synth types & Expansion slots";
            Librarian_lblGroups.IsEnabled = false;
            Librarian_lvGroups = new ListView();
            Librarian_ocGroups = new ObservableCollection<String>();
            Librarian_lvGroups.ItemsSource = Librarian_ocGroups;
            Librarian_lvGroups.ItemTemplate = colorSettings.ListViewTextColor;

            // Make a listview lvCategories for column 1:
            Librarian_lblCategories = new Button();
            Librarian_lblCategories.Text = "Sound categories";
            Librarian_lvCategories = new ListView();
            Librarian_ocCategories = new ObservableCollection<String>();
            Librarian_lvCategories.ItemsSource = Librarian_ocCategories;
            Librarian_lvCategories.ItemTemplate = colorSettings.ListViewTextColor;

            // Make a listview for studio sets:
            //Librarian_lblStudioSets = new Button();
            //Librarian_lblStudioSets.Text = "Studio sets";
            //Librarian_lblStudioSets.IsVisible = false;
            //Librarian_lvStudioSets = new ListView();
            //Librarian_lvStudioSets.IsVisible = false;
            //Librarian_ocStudioSets = new ObservableCollection<String>();
            //Librarian_lvStudioSets.ItemsSource = Librarian_ocStudioSets;

            // Make Grids for column 0 - 2:
            Librarian_gridMain = new Grid();
            Librarian_gridMain.VerticalOptions = LayoutOptions.FillAndExpand;
            Librarian_gridGroups = new Grid();
            Librarian_gridGroups.VerticalOptions = LayoutOptions.FillAndExpand;
            Librarian_gridCategories = new Grid();
            Librarian_gridCategories.VerticalOptions = LayoutOptions.FillAndExpand;
            Librarian_gridTones = new Grid();
            Librarian_gridTones.VerticalOptions = LayoutOptions.FillAndExpand;

            // Make a filter button for column 2:
            Librarian_filterPresetAndUser = new Button();
            Librarian_filterPresetAndUser.Text = "Load user tones";
            //Librarian_filterPresetAndUser.BackgroundColor = colorSettings.ControlBackground;

            // Make listviews lvToneNames and lvSearchResult for column 2:
            Librarian_lvToneNames = new ListView();
            Librarian_lvToneNames.ItemTemplate = colorSettings.ListViewTextColor;
            //Librarian_lvToneNames.BackgroundColor = colorSettings.ControlBackground;
            //Librarian_ocCategories = new ObservableCollection<String>();
            //Librarian_lvToneNames.ItemsSource = Librarian_ocToneNames;
            //Librarian_lvSearchResult = new ListView();
            //Librarian_lvSearchResult.BackgroundColor = colorSettings.Background;
            //Librarian_SearchResult = new ObservableCollection<String>();
            //Librarian_lvSearchResult.ItemsSource = Librarian_ToneNames;
            //Librarian_lvSearchResult.IsVisible = false;

            // Make a Grid for column 3:
            Librarian_gridKeyboard = new Grid(1);

            // Make pickers for MIDI:
            //Librarian_midiOutputDevice = new Picker();
            //Librarian_midiInputDevice = new Picker();
            Librarian_midiOutputChannel = new Picker();
            for (Int32 i = 0; i < 16; i++)
            {
                String temp = "Part " + (i + 1).ToString();
                Librarian_midiOutputChannel.Items.Add(temp);
            }
            Librarian_midiOutputChannel.SelectedIndex = 0;
            Librarian_midiInputChannel = new Picker();
            for (Int32 i = 0; i < 16; i++)
            {
                String temp = "Part " + (i + 1).ToString();
                Librarian_midiInputChannel.Items.Add(temp);
            }
            Librarian_midiInputChannel.SelectedIndex = 0;
            //Librarian_midiInputDevice.IsVisible = false;
            Librarian_midiInputChannel.IsVisible = false;

            // Make labeled editor fields:
            Librarian_btnSearch = new Button();
            Librarian_btnSearch.Text = "Search:";
            Librarian_tbSearch = new Editor();
            Librarian_ltToneName = new LabeledText("Tone Name:", "", new byte[] { 1, 2 });
            Librarian_ltType = new LabeledText("Type:", "", new byte[] { 1, 2 });
            Librarian_ltToneNumber = new LabeledText("Tone #:", "", new byte[] { 1, 2 });
            Librarian_ltBankAddress = new LabeledText("Bank @:", "", new byte[] { 1, 2 });
            Librarian_ltPatchMSB = new LabeledText("Bank MSB:", "", new byte[] { 1, 2 });
            Librarian_ltPatchLSB = new LabeledText("Bank LSB:", "", new byte[] { 1, 2 });
            Librarian_ltProgramNumber = new LabeledText("Program #:", "", new byte[] { 1, 2 });

            // Make the keyboard grid (We cannot use GridRow here due to limitation of byte
            // and we do not need to run a lot of setting we will change here anyway):
            Librarian_gridKeyboard.ColumnDefinitions = new ColumnDefinitionCollection();
            ColumnDefinition cdHide = new ColumnDefinition(); // Spanning the cover over left key roundings,
            ColumnDefinition cdLength = new ColumnDefinition(); // Spanning first black key size,
            ColumnDefinition cdMargin = new ColumnDefinition(); // Spanning both to get white key size
            cdHide.Width = new GridLength(9, GridUnitType.Absolute);
            cdLength.Width = new GridLength(30, GridUnitType.Star);
            cdMargin.Width = new GridLength(15, GridUnitType.Star);
            Librarian_gridKeyboard.ColumnDefinitions.Add(cdHide);
            Librarian_gridKeyboard.ColumnDefinitions.Add(cdLength);
            Librarian_gridKeyboard.ColumnDefinitions.Add(cdMargin);

            Librarian_gridKeyboard.RowDefinitions = new RowDefinitionCollection();
            for (Int32 i = 0; i < 22 * 16; i++)
            {
                Librarian_gridKeyboard.RowDefinitions.Add(new RowDefinition());
            }

            // Make the white keyboard keys:
            Librarian_btnWhiteKeys = new PianoKey[22];
            for (byte i = 0; i < 22; i++)
            {
                Librarian_btnWhiteKeys[i] = new PianoKey(true);
                Grid grid = new Grid(1);
                grid.Margin = new Thickness(0);
                grid.BackgroundColor = colorSettings.FrameBorder;
                Grid.SetRowSpan(grid, 16);
                Grid.SetRow(grid, i * 16);
                Grid.SetColumnSpan(grid, 3);
                Grid.SetColumn(grid, 0);
                grid.Children.Add(Librarian_btnWhiteKeys[i]);
                Librarian_gridKeyboard.Children.Add(grid);
                Librarian_btnWhiteKeys[i].BackgroundColor = colorSettings.WhitePianoKey;
                Librarian_btnWhiteKeys[i].TextColor = colorSettings.BlackPianoKey;
                Librarian_btnWhiteKeys[i].Text = "";
                Librarian_btnWhiteKeys[i].StyleId = i.ToString();
                Librarian_btnWhiteKeys[i].Pressed += Librarian_btnWhiteKey_Pressed;
                Librarian_btnWhiteKeys[i].Released += Librarian_btnWhiteKey_Released;
                Librarian_btnWhiteKeys[i].BorderWidth = 0;
                if (i == 0)
                {
                    Librarian_btnWhiteKeys[i].Margin = new Thickness(2, 2, 2, 2);
                }
                else
                {
                    Librarian_btnWhiteKeys[i].Margin = new Thickness(2, 0, 2, 2);
                }
            }

            // Make the black keyboard keys:
            Librarian_btnBlackKeys = new PianoKey[15];
            byte[] need6fillers = { 3, 5, 8, 10, 13 };
            byte numberOfFillers = 0;
            Int32 position = 0;
            for (byte i = 0; i < 15; i++)
            {
                if (i == 0)
                {
                    numberOfFillers = 27;
                }
                else if (need6fillers.Contains(i))
                {
                    numberOfFillers = 24;
                }
                else
                {
                    numberOfFillers = 8;
                }
                Librarian_btnBlackKeys[i] = new PianoKey(false);
                position += numberOfFillers;
                Grid grid = new Grid(2);
                grid.Margin = new Thickness(2, 0, 0, 0);
                grid.BackgroundColor = colorSettings.Background;
                Grid.SetRowSpan(grid, 10);
                Grid.SetRow(grid, position);
                Grid.SetColumnSpan(grid, 2);
                Grid.SetColumn(grid, 0);
                grid.Children.Add(Librarian_btnBlackKeys[i]);
                Librarian_gridKeyboard.Children.Add(grid);
                position += 8;
                Librarian_btnBlackKeys[i].Text = "";
                Librarian_btnBlackKeys[i].StyleId = i.ToString();
                Librarian_btnBlackKeys[i].BackgroundColor = colorSettings.BlackPianoKey;
                Librarian_btnBlackKeys[i].TextColor = colorSettings.WhitePianoKey;
                Librarian_btnBlackKeys[i].Margin = new Thickness(2, 0, 0, 0);
                Librarian_btnBlackKeys[i].Pressed += Librarian_btnBlackKey_Pressed;
                Librarian_btnBlackKeys[i].Released += Librarian_btnBlackKey_Released;
                Librarian_btnBlackKeys[i].BorderWidth = 0;
            }

            //Librarian_gridKeyboard.RowSpacing = 0;
            //Librarian_gridKeyboard.ColumnSpacing = 0;

            // Put an unused white key that spans and covers the leftmost rounded
            // corners of the white keys:
            Button cover = new Button();
            Grid.SetRowSpan(cover, 22 * 16);
            Grid.SetColumn(cover, 0);
            cover.Tag = "PianoKeyCover";
            cover.BackgroundColor = colorSettings.Progressbar;
            Librarian_gridKeyboard.Children.Add(cover);

            // Add the buttons
            Librarian_btnEditTone = new Button();
            Librarian_btnEditStudioSet = new Button();
            Librarian_btnResetVolume = new Button();
            Librarian_btnMotionalSurround = new Button();
            Librarian_btnSettings = new Button();
            Librarian_btnSettings.Text = "Settings";
            Librarian_btnAddFavorite = new Button();
            Librarian_btnRemoveFavorite = new Button();
            Librarian_btnPlay = new Button();
            Librarian_btnShowFavorites = new Button();
            Librarian_btnResetHangingNotes = new Button();
            Librarian_btnPlus12keys = new Button();
            Librarian_btnMinus12keys = new Button();
            Librarian_btnKeys = new Button();

            Librarian_btnEditTone.Text = "Edit tone";
            Librarian_btnEditStudioSet.Text = "Edit studio set";
            Librarian_btnResetVolume.Text = "Res. vol.";
            Librarian_btnMotionalSurround.Text = "Motional surround";
            Librarian_btnAddFavorite.Text = "Add";
            Librarian_btnRemoveFavorite.Text = "Remove";
            Librarian_btnPlay.Text = "Play";
            Librarian_btnShowFavorites.Text = "Favorites";
            Librarian_btnResetHangingNotes.Text = "Reset";
            Librarian_btnPlus12keys.Text = "+12";
            Librarian_btnMinus12keys.Text = "-12";
            //ShowKeyNumbering();

            // Add handlers -------------------------------------------------------------------------------

            Librarian_btnSettings.Clicked += Librarian_btnSettings_Clicked;
            Librarian_lvGroups.ItemSelected += Librarian_LvGroups_ItemSelected;
            Librarian_lvCategories.ItemSelected += Librarian_LvCategories_ItemSelected;
            Librarian_filterPresetAndUser.Clicked += Librarian_FilterPresetAndUser_Clicked;
            Librarian_lvToneNames.ItemSelected += Librarian_LvToneNames_ItemSelected;
            Librarian_midiOutputChannel.SelectedIndexChanged += Librarian_MidiOutputChannel_SelectedIndexChanged;
            Librarian_tbSearch.TextChanged += Librarian_tbSearch_TextChanged;
            Librarian_btnEditTone.Clicked += Librarian_BtnEditTone_Clicked;
            Librarian_btnEditStudioSet.Clicked += Librarian_btnEditStudioSet_Clicked;
            Librarian_btnResetVolume.Clicked += Librarian_btnResetVolume_Clicked;
            Librarian_btnMotionalSurround.Clicked += Librarian_btnMotionalSurround_Clicked;
            Librarian_btnAddFavorite.Clicked += Librarian_btnAddFavorite_Clicked;
            Librarian_btnRemoveFavorite.Clicked += Librarian_btnRemoveFavorite_Clicked;
            Librarian_btnPlay.Clicked += Librarian_btnPlay_Clicked;
            Librarian_btnShowFavorites.Clicked += Librarian_btnFavorites_Clicked;
            Librarian_btnResetHangingNotes.Clicked += Librarian_btnResetHangingNotes_Clicked;
            Librarian_btnKeys.Clicked += Librarian_btnKeys_Clicked;
            Librarian_btnPlus12keys.Clicked += Librarian_btnPlus12keys_Clicked;
            Librarian_btnMinus12keys.Clicked += Librarian_btnMinus12keys_Clicked;

            // Assemble grids with controls ---------------------------------------------------------------

            rowHeight = 1;
            if (appType == _appType.ANDROID)
            {
                rowHeight = 2;
            }

            listingHeight = 12;

            // Assemble column 0:
            Librarian_gridGroups.Children.Add((new GridRow(
                0, new View[] { Librarian_lblGroups })));
            Librarian_gridGroups.Children.Add((new GridRow(
                1, new View[] { Librarian_lvGroups }, null, false, false, listingHeight)));
            Librarian_gridGroups.Children.Add((new GridRow(
                (byte)(listingHeight + 1), new View[] { Librarian_midiOutputChannel })));
            Librarian_gridGroups.Children.Add((new GridRow(
                (byte)(listingHeight + 2), new View[] { Librarian_btnSearch, Librarian_tbSearch }, new byte[] { 1, 2 })));
            Librarian_gridGroups.Children.Add((new GridRow(
                (byte)(listingHeight + 3), new View[] { Librarian_ltToneName })));
            Librarian_gridGroups.Children.Add((new GridRow(
                (byte)(listingHeight + 4), new View[] { Librarian_ltType })));
            Librarian_gridGroups.Children.Add((new GridRow(
                (byte)(listingHeight + 5), new View[] { Librarian_ltToneNumber })));

            // Assemble column 1:
            Librarian_gridCategories.Children.Add((new GridRow(
                0, new View[] { Librarian_lblCategories })));
            Librarian_gridCategories.Children.Add((new GridRow(
                1, new View[] { Librarian_lvCategories }, null, false, false, listingHeight)));
            Librarian_gridCategories.Children.Add((new GridRow(
                (byte)(listingHeight + 1), new View[] { Librarian_ltBankAddress })));
            Librarian_gridCategories.Children.Add((new GridRow(
                (byte)(listingHeight + 2), new View[] { Librarian_ltPatchMSB })));
            Librarian_gridCategories.Children.Add((new GridRow(
                (byte)(listingHeight + 3), new View[] { Librarian_ltPatchLSB })));
            Librarian_gridCategories.Children.Add((new GridRow(
                (byte)(listingHeight + 4), new View[] { Librarian_ltProgramNumber })));
            Librarian_gridCategories.Children.Add((new GridRow(
                (byte)(listingHeight + 5), new View[] { Librarian_btnPlay })));

            // Assemble column 2:
            Librarian_gridTones.Children.Add((new GridRow(
                0, new View[] { Librarian_filterPresetAndUser })));
            Librarian_gridTones.Children.Add((new GridRow(
                1, new View[] { Librarian_lvToneNames }, null, false, false, listingHeight)));



            // Temporary removed settings to get project going
            Librarian_gridTones.Children.Add((new GridRow(
                (byte)(listingHeight + 1),
                new View[] { Librarian_btnMotionalSurround, Librarian_btnSettings }, new byte[] { 2, 1 })));
            //Librarian_gridTones.Children.Add((new GridRow(
            //    (byte)(listingHeight + 1),
            //    new View[] { Librarian_btnMotionalSurround } )));



            Librarian_gridTones.Children.Add((new GridRow(
                (byte)(listingHeight + 2),
                new View[] { Librarian_btnShowFavorites, Librarian_btnAddFavorite, Librarian_btnRemoveFavorite }, new byte[] { 1, 1, 1 })));
            Librarian_gridTones.Children.Add((new GridRow(
                (byte)(listingHeight + 3),
                new View[] { Librarian_btnEditTone, Librarian_btnEditStudioSet }, new byte[] { 1, 2 })));
            Librarian_gridTones.Children.Add((new GridRow(
                (byte)(listingHeight + 4),
                new View[] { Librarian_btnResetHangingNotes, Librarian_btnResetVolume }, new byte[] { 2, 1 })));
            Librarian_gridTones.Children.Add((new GridRow(
                (byte)(listingHeight + 5),
                new View[] { Librarian_btnKeys, Librarian_btnMinus12keys, Librarian_btnPlus12keys }, new byte[] { 3, 1, 1 })));
            Librarian_btnKeys.Margin = new Thickness(0);

            // Assemble LibrarianStackLayout --------------------------------------------------------------

            Librarian_StackLayout = new StackLayout();
            Librarian_StackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            Librarian_StackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            Librarian_gridGroups.BackgroundColor = colorSettings.FrameBorder;
            Librarian_gridCategories.BackgroundColor = colorSettings.FrameBorder;
            Librarian_gridTones.BackgroundColor = colorSettings.FrameBorder;
            Librarian_gridKeyboard.BackgroundColor = colorSettings.FrameBorder;
            Librarian_gridMain.Children.Add((new GridRow(
                0, new View[]
                { Librarian_gridGroups, Librarian_gridCategories, Librarian_gridTones, Librarian_gridKeyboard },
                new byte[] { 5, 5, 5, 3 }, false, true)));
            Librarian_StackLayout.Children.Add(Librarian_gridMain);

            Librarian_gridMain.RowSpacing = Margins;
            Librarian_gridMain.ColumnSpacing = Margins;
            Librarian_gridMain.Margin = new Thickness(0);
            Librarian_gridKeyboard.RowSpacing = 0;
            Librarian_gridKeyboard.ColumnSpacing = 0;
            Librarian_gridGroups.RowSpacing = Margins;
            Librarian_gridGroups.ColumnSpacing = Margins;
            Librarian_gridCategories.RowSpacing = Margins;
            Librarian_gridCategories.ColumnSpacing = Margins;
            Librarian_gridTones.RowSpacing = Margins;
            Librarian_gridTones.ColumnSpacing = Margins;

            SetStackLayoutColors(Librarian_StackLayout);
            t.Trace("Librarian created ");
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Librarian handlers
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Librarian_Timer_Tick()
        {
            if (updateToneName)
            {
                updateToneName = false;
                PushHandleControlEvents();
                if (commonState.CurrentTone == null)
                {
                    mainPage.DisplayAlert("INTEGRA-7 Librarian and Editor",
                         "It seems like you have selected a Studio Set with " +
                         "some User Tone that has not been read in from your INTEGRA-7. " +
                         "Click on 'Load user tones' in the Librarian.",
                         "Ok");
                    // We must have something in current tone:
                    commonState.CurrentTone = new Tone(-1, -1, -1, "SuperNATURAL Acoustic Tone", "Ac.Piano", "Full Grand 1");
                    commonState.CurrentTone.BankMSB = 0x59;
                    commonState.CurrentTone.BankLSB = 0x40;
                    commonState.CurrentTone.Program = 0x00;
                }
                try // In case I7 holds a user sound, which are not loaded into app, surround this with a try:
                {
                    Librarian_lvGroups.SelectedItem = commonState.CurrentTone.Group;
                    PopulateCategories();
                    Librarian_lvCategories.SelectedItem = commonState.CurrentTone.Category;
                    PopulateToneNames();
                    Librarian_lvToneNames.SelectedItem = commonState.CurrentTone.Name;
                    PopulateToneData();
                    UpdateDrumNames();
                }
                catch
                {
                    commonState.CurrentTone = new Tone();
                }
                PopHandleControlEvents();
            }
            if (showCurrentToneReadFromI7)
            {
                showCurrentToneReadFromI7 = false;
                if (commonState.CurrentTone.ToneIndex > -1)
                {
                    updateIntegra7 = false;
                    PushHandleControlEvents();
                    Librarian_lvGroups.SelectedItem = commonState.CurrentTone.Group;
                    PopulateCategories();
                    Librarian_lvCategories.SelectedItem = commonState.CurrentTone.Category;
                    PopulateToneNames();
                    Librarian_lvToneNames.SelectedItem = commonState.CurrentTone.Name;
                    PopulateToneData();
                    updateIntegra7 = true;
                    PopHandleControlEvents();
                }
            }
            if (toneNamesRead)
            {
                toneNamesRead = false;
            }
            if (studioSetNamesJustRead == StudioSetNames.READ_BUT_NOT_LISTED)
            {
                // All studio set names has been received and stored in studioSetNames,
                // populate the combobox:
                updateIntegra7 = false;
                Librarian_PopulateStudioSetListview();
                studioSetNamesJustRead = StudioSetNames.READ_AND_LISTED;
                PleaseWait_StackLayout.IsVisible = false;
                Librarian_StackLayout.IsVisible = true;
                initDone = true;
            }
            if (initDone)
            {
                // From now we allow selection changes in lvToneNames to update the I-7:
                updateIntegra7 = true;
            }
        }

        public void Librarian_MidiInPort_MessageReceived()
        {
            // Set font size:
            if (needToSetFontSize)
            {
                SetFontSizes(Librarian_StackLayout);
                needToSetFontSize = false;
            }

            //t.Trace("private void MainPage_MidiInPort_MessageReceived");
            if (initDone || scanning)
            {
                try
                {
                    if (rawData[0] == 0xf0) // handle system exclusive messages only
                    {
                        byte[] data = rawData;
                        switch (queryType)
                        {
                            case QueryType.PCM_SYNTH_TONE_COMMON:
                                // This is the first user tone.
                                // Its index should be set to continue after the preset tones:
                                userToneIndex = commonState.ToneList.PresetsCount;
                                if (!IsInitTone(data))
                                {
                                    commonState.CurrentTone.Name = "";
                                    for (byte i = 0; i < 12; i++)
                                    {
                                        commonState.CurrentTone.Name += (char)data[i + 11];
                                    }
                                    commonState.CurrentTone.Name = commonState.CurrentTone.Name.Trim();
                                    commonState.ToneNames[0].Add(commonState.CurrentTone.Name);
                                    // Also read common2 to get tone category:
                                    byte[] address = new byte[] { 0x1c, 0x60, 0x30, 0x00 };
                                    byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x3c };
                                    byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
                                    commonState.Midi.SendSystemExclusive(message);
                                    queryType = QueryType.PCM_SYNTH_TONE_COMMON2;
                                }
                                else
                                {
                                    commonState.ToneNames[0].Add("INIT TONE");
                                    emptySlots++;
                                    pc++;
                                    if (pc > 128 || (!scanAll && emptySlots > 10))
                                    {
                                        lsb++;
                                        pc = 1;
                                        if (lsb > 1)
                                        {
                                            // No more patches to test!
                                            while (commonState.ToneNames[0].Count() < 256)
                                            {
                                                commonState.ToneNames[0].Add("INIT TONE");
                                            }
                                            msb = 86;
                                            lsb = 0;
                                            pc = 1;
                                            emptySlots = 0;
                                            QueryUserPCMDrumKitTones();
                                            break;
                                        }
                                    }
                                    // Check next:
                                    QueryUserPCMSyntTones();
                                }
                                break;
                            case QueryType.PCM_SYNTH_TONE_COMMON2:
                                toneCategory = data[0x1b];
                                List<String> tone = new List<String>();
                                tone.Add("PCM Synth Tone");
                                tone.Add(toneCategories.pcmToneCategoryNames[toneCategory]);
                                tone.Add((userToneNumbers[toneCategory]++).ToString());
                                tone.Add(commonState.CurrentTone.Name);
                                tone.Add(msb.ToString());
                                tone.Add(lsb.ToString());
                                tone.Add((msb * 128 + lsb).ToString());
                                tone.Add(pc.ToString());
                                tone.Add("(User)");
                                tone.Add((userToneIndex++).ToString());
                                commonState.ToneList.Tones.Add(tone);
                                pc++;
                                if (pc > 128 || (!scanAll && emptySlots > 10))
                                {
                                    lsb++;
                                    pc = 1;
                                    if (lsb > 1 || (!scanAll && emptySlots > 10))
                                    {
                                        // No more patches to test!
                                        while (commonState.ToneNames[0].Count() < 256)
                                        {
                                            commonState.ToneNames[0].Add("INIT TONE");
                                        }
                                        msb = 86;
                                        lsb = 0;
                                        pc = 1;
                                        emptySlots = 10;
                                        for (byte i = 0; i < 128; i++)
                                        {
                                            userToneNumbers[i] = 0;
                                        }
                                        QueryUserPCMDrumKitTones();
                                        break;
                                    }
                                }
                                // Check next:
                                QueryUserPCMSyntTones();
                                break;
                            case QueryType.PCM_DRUM_KIT_COMMON:
                                if (!IsInitKit(data))
                                {
                                    commonState.CurrentTone.Name = "";
                                    for (byte i = 0; i < 12; i++)
                                    {
                                        commonState.CurrentTone.Name += (char)data[i + 11];
                                    }
                                    commonState.CurrentTone.Name = commonState.CurrentTone.Name.Trim();
                                    commonState.ToneNames[1].Add(commonState.CurrentTone.Name);
                                    tone = new List<String>();
                                    tone.Add("PCM Drum Kit");
                                    tone.Add("Drum");
                                    tone.Add((userToneNumbers[toneCategory]++).ToString());
                                    tone.Add(commonState.CurrentTone.Name);
                                    tone.Add(msb.ToString());
                                    tone.Add(lsb.ToString());
                                    tone.Add((msb * 128 + lsb).ToString());
                                    tone.Add(pc.ToString());
                                    tone.Add("(User)");
                                    tone.Add((userToneIndex++).ToString());
                                    commonState.ToneList.Tones.Add(tone);
                                    // Create a list for the key names:
                                    commonState.DrumKeyAssignLists.ToneNames.Add(new List<String>());
                                    commonState.DrumKeyAssignLists.ToneNames[commonState.DrumKeyAssignLists.ToneNames.Count - 1].Add("PCM Drum Kit");
                                    commonState.DrumKeyAssignLists.ToneNames[commonState.DrumKeyAssignLists.ToneNames.Count - 1].Add(commonState.CurrentTone.Name);
                                    // Read all key names:
                                    key = 0;
                                    QueryPcmDrumKitKeyName(key);
                                    break;
                                }
                                else
                                {
                                    commonState.ToneNames[1].Add("INIT KIT");
                                    emptySlots++;
                                }
                                pc++;
                                if (pc > 32 || (!scanAll && emptySlots > 10))
                                {
                                    // No more patches to test!
                                    while (commonState.ToneNames[1].Count() < 32)
                                    {
                                        commonState.ToneNames[1].Add("INIT KIT");
                                    }
                                    msb = 89;
                                    lsb = 0;
                                    pc = 1;
                                    emptySlots = 10;
                                    for (byte i = 0; i < 128; i++)
                                    {
                                        userToneNumbers[i] = 0;
                                    }
                                    emptySlots = 0;
                                    QueryUserSuperNaturalAcousticTones();
                                    break;
                                }
                                // Check next:
                                QueryUserPCMDrumKitTones();
                                break;
                            case QueryType.PCM_KEY_NAME:
                                // Put the name into the list:
                                String name = "";
                                for (byte i = 0; i < 12; i++)
                                {
                                    name += (char)data[i + 11];
                                }
                                commonState.DrumKeyAssignLists.ToneNames[commonState.DrumKeyAssignLists.ToneNames.Count - 1].Add(name);
                                // Query next if more is expected:
                                key++;
                                if (key < 88)
                                {
                                    // Query next key:
                                    QueryPcmDrumKitKeyName(key);
                                }
                                else
                                {
                                    // Query next PCM Drum Kit:
                                    pc++;
                                    if (pc > 32 || (!scanAll && emptySlots > 10))
                                    {
                                        // No more patches to test!
                                        while (commonState.ToneNames[1].Count() < 32)
                                        {
                                            commonState.ToneNames[1].Add("INIT KIT");
                                        }
                                        msb = 89;
                                        lsb = 0;
                                        pc = 1;
                                        emptySlots = 10;
                                        for (byte i = 0; i < 128; i++)
                                        {
                                            userToneNumbers[i] = 0;
                                        }
                                        emptySlots = 0;
                                        QueryUserSuperNaturalAcousticTones();
                                        break;
                                    }
                                    // Check next:
                                    QueryUserPCMDrumKitTones();
                                }
                                break;
                            case QueryType.SUPERNATURAL_ACOUSTIC_TONE_COMMON:
                                if (!IsInitTone(data))
                                {
                                    toneCategory = data[0x26];
                                    commonState.CurrentTone.Name = "";
                                    for (byte i = 0; i < 12; i++)
                                    {
                                        commonState.CurrentTone.Name += (char)data[i + 11];
                                    }
                                    commonState.CurrentTone.Name = commonState.CurrentTone.Name.Trim();
                                    commonState.ToneNames[2].Add(commonState.CurrentTone.Name);
                                    tone = new List<String>();
                                    tone.Add("SuperNATURAL Acoustic Tone");
                                    tone.Add(toneCategories.snaToneCategoryNames[toneCategory]);
                                    tone.Add((userToneNumbers[toneCategory]++).ToString());
                                    tone.Add(commonState.CurrentTone.Name);
                                    tone.Add(msb.ToString());
                                    tone.Add(lsb.ToString());
                                    tone.Add((msb * 128 + lsb).ToString());
                                    tone.Add(pc.ToString());
                                    tone.Add("(User)");
                                    tone.Add((userToneIndex++).ToString());
                                    commonState.ToneList.Tones.Add(tone);
                                }
                                else
                                {
                                    commonState.ToneNames[2].Add("INIT TONE");
                                    emptySlots++;
                                }
                                pc++;
                                if (pc > 128 || (!scanAll && emptySlots > 10))
                                {
                                    lsb++;
                                    pc = 1;
                                    if (lsb > 1 || (!scanAll && emptySlots > 10))
                                    {
                                        // No more patches to test!
                                        while (commonState.ToneNames[2].Count() < 256)
                                        {
                                            commonState.ToneNames[2].Add("INIT TONE");
                                        }
                                        msb = 95;
                                        lsb = 0;
                                        pc = 1;
                                        emptySlots = 10;
                                        for (byte i = 0; i < 128; i++)
                                        {
                                            userToneNumbers[i] = 0;
                                        }
                                        emptySlots = 0;
                                        QueryUserSuperNaturalSynthTones();
                                        break;
                                    }
                                }
                                // Check next:
                                QueryUserSuperNaturalAcousticTones();
                                break;
                            case QueryType.SUPERNATURAL_SYNTH_TONE_COMMON:
                                if (!IsInitTone(data))
                                {
                                    toneCategory = data[0x41];
                                    commonState.CurrentTone.Name = "";
                                    for (byte i = 0; i < 12; i++)
                                    {
                                        commonState.CurrentTone.Name += (char)data[i + 11];
                                    }
                                    commonState.CurrentTone.Name = commonState.CurrentTone.Name.Trim();
                                    commonState.ToneNames[3].Add(commonState.CurrentTone.Name);
                                    tone = new List<String>();
                                    tone.Add("SuperNATURAL Synth Tone");
                                    tone.Add(toneCategories.snsToneCategoryNames[toneCategory]);
                                    tone.Add((userToneNumbers[toneCategory]++).ToString());
                                    tone.Add(commonState.CurrentTone.Name);
                                    tone.Add(msb.ToString());
                                    tone.Add(lsb.ToString());
                                    tone.Add((msb * 128 + lsb).ToString());
                                    tone.Add(pc.ToString());
                                    tone.Add("(User)");
                                    tone.Add((userToneIndex++).ToString());
                                    commonState.ToneList.Tones.Add(tone);
                                }
                                else
                                {
                                    commonState.ToneNames[3].Add("INIT TONE");
                                    emptySlots++;
                                }
                                pc++;
                                if (pc > 128 || (!scanAll && emptySlots > 10))
                                {
                                    lsb++;
                                    pc = 1;
                                    if (lsb > 3 || (!scanAll && emptySlots > 10))
                                    {
                                        // No more patches to test!
                                        while (commonState.ToneNames[3].Count() < 512)
                                        {
                                            commonState.ToneNames[3].Add("INIT TONE");
                                        }
                                        msb = 88;
                                        lsb = 0;
                                        pc = 1;
                                        emptySlots = 10;
                                        for (byte i = 0; i < 128; i++)
                                        {
                                            userToneNumbers[i] = 0;
                                        }
                                        emptySlots = 0;
                                        QueryUserSuperNaturalDrumKitTones();
                                        break;
                                    }
                                }
                                // Check next:
                                QueryUserSuperNaturalSynthTones();
                                break;
                            case QueryType.SUPERNATURAL_DRUM_KIT_COMMON:
                                if (!IsInitKit(data))
                                {
                                    commonState.CurrentTone.Name = "";
                                    for (byte i = 0; i < 12; i++)
                                    {
                                        commonState.CurrentTone.Name += (char)data[i + 11];
                                    }
                                    commonState.CurrentTone.Name = commonState.CurrentTone.Name.Trim();
                                    commonState.ToneNames[4].Add(commonState.CurrentTone.Name);
                                    tone = new List<String>();
                                    tone.Add("SuperNATURAL Drum Kit");
                                    tone.Add("Drum");
                                    tone.Add((userToneNumbers[toneCategory]++).ToString());
                                    tone.Add(commonState.CurrentTone.Name);
                                    tone.Add(msb.ToString());
                                    tone.Add(lsb.ToString());
                                    tone.Add((msb * 128 + lsb).ToString());
                                    tone.Add(pc.ToString());
                                    tone.Add("(User)");
                                    tone.Add((userToneIndex++).ToString());
                                    commonState.ToneList.Tones.Add(tone);
                                    // Create a list for the key names:
                                    commonState.DrumKeyAssignLists.ToneNames.Add(new List<String>());
                                    commonState.DrumKeyAssignLists.ToneNames[commonState.DrumKeyAssignLists.ToneNames.Count - 1].Add("SuperNATURAL Drum Kit");
                                    commonState.DrumKeyAssignLists.ToneNames[commonState.DrumKeyAssignLists.ToneNames.Count - 1].Add(commonState.CurrentTone.Name);
                                    // SN-D keys does not have keys 22 - 26, fill with empth slots:
                                    commonState.DrumKeyAssignLists.ToneNames[commonState.DrumKeyAssignLists.ToneNames.Count - 1].Add("-----");
                                    commonState.DrumKeyAssignLists.ToneNames[commonState.DrumKeyAssignLists.ToneNames.Count - 1].Add("-----");
                                    commonState.DrumKeyAssignLists.ToneNames[commonState.DrumKeyAssignLists.ToneNames.Count - 1].Add("-----");
                                    commonState.DrumKeyAssignLists.ToneNames[commonState.DrumKeyAssignLists.ToneNames.Count - 1].Add("-----");
                                    commonState.DrumKeyAssignLists.ToneNames[commonState.DrumKeyAssignLists.ToneNames.Count - 1].Add("-----");
                                    // Read all key names:
                                    key = 0;
                                    QuerySnDrumKitKeyName(key);
                                    break;
                                }
                                else
                                {
                                    commonState.ToneNames[4].Add("INIT KIT");
                                    emptySlots++;
                                }
                                pc++;
                                if (pc > 64 || (!scanAll && emptySlots > 10))
                                {
                                    // No more patches to test!
                                    while (commonState.ToneNames[4].Count() < 64)
                                    {
                                        commonState.ToneNames[4].Add("INIT KIT");
                                    }
                                    QuerySelectedStudioSet();
                                    break;
                                }
                                // Check next:
                                QueryUserSuperNaturalDrumKitTones();
                                break;
                            case QueryType.SND_KEY_NAME:
                                // Put the name into the list:
                                try
                                {
                                    commonState.DrumKeyAssignLists.ToneNames[commonState.DrumKeyAssignLists.ToneNames.Count - 1]
                                        .Add(superNATURALDrumKitInstrumentList.DrumInstruments[data[12] * 256 + data[13] * 16 + data[14]].Name);
                                }
                                catch { }
                                // Query next if more is expected:
                                key++;
                                if (key < 61)
                                {
                                    // Query next key:
                                    QuerySnDrumKitKeyName(key);
                                }
                                else
                                {
                                    // Query next SN Drum Kit:
                                    pc++;
                                    if (pc > 64 || (!scanAll && emptySlots > 10))
                                    {
                                        // No more patches to test!
                                        while (commonState.ToneNames[1].Count() < 32)
                                        {
                                            commonState.ToneNames[1].Add("INIT KIT");
                                        }
                                        msb = 85;
                                        lsb = 0;
                                        pc = 1;
                                        emptySlots = 10;
                                        for (byte i = 0; i < 128; i++)
                                        {
                                            userToneNumbers[i] = 0;
                                        }
                                        // Part 16 has been used to get user tone names.
                                        // Therefore, the studio set has been changed (tone at part 16 changed but restored).
                                        // In order to turn the change state off and restore it, just select the studio set:
                                        SetStudioSet(commonState.CurrentStudioSet);
                                        QuerySelectedStudioSet();
                                        break;
                                    }
                                    // Check next:
                                    QueryUserSuperNaturalDrumKitTones();
                                }
                                break;
                            case QueryType.CURRENT_SELECTED_STUDIO_SET:
                                //commonState.CurrentStudioSet = receivedMidiMessage.RawData.ToArray()[0x11];
                                commonState.CurrentStudioSet = rawData[0x11];
                                QuerySelectedTone();
                                break;
                            case QueryType.CURRENT_SELECTED_TONE:
                                Int32 index = commonState.ToneList.Get(
                                    rawData[0x11],
                                    rawData[0x12],
                                    (byte)(rawData[0x13]));
                                try
                                {
                                    commonState.CurrentTone = new Tone(commonState.ToneList.Tones[index]);
                                }
                                catch { }
                                queryType = QueryType.NONE;
                                scanning = false;
                                updateToneName = true;
                                break;
                            case QueryType.READ_TONE_FROM_I7:
                                try
                                {
                                    Int32 toneIndex = commonState.ToneList.Get(data[17], data[18], data[19]);
                                    commonState.CurrentTone = new Tone(commonState.ToneList.Tones[toneIndex]);
                                    commonState.CurrentTone.ToneIndex = toneIndex;
                                }
                                catch
                                {
                                    // If the user created a user tone, and that is the one selected,
                                    // it must have been read in, or else we end up here. So, let's
                                    // just pick number 0:
                                    commonState.CurrentTone = new Tone(commonState.ToneList.Tones[0]);
                                    commonState.CurrentTone.ToneIndex = 0;
                                }
                                showCurrentToneReadFromI7 = true;
                                queryType = QueryType.NONE;
                                break;
                            case QueryType.STUDIO_SET_NAMES:
                                break;
                        }
                    }
                }
                catch { }
            }
        }

        private void Librarian_LvGroups_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (initDone && handleControlEvents)
            {
                if ((String)Librarian_lvGroups.SelectedItem == "Studio sets")
                {
                    if (Librarian_ocToneNames == null)
                    {
                        try
                        {
                            Librarian_ocToneNames = new ObservableCollection<String>();
                        }
                        catch { }
                    }
                    currentStudioSet = commonState.CurrentStudioSet;
                    Librarian_lblCategories.Content = "Studio sets";
                    Librarian_lvToneNames.IsEnabled = false;
                    //ControlRows.Visibility = Visibility.Collapsed;
                    Librarian_ocToneNames.Clear();
                    //PleaseWaitWhileScanning.Visibility = Visibility.Visible;
                    //txtScanning.Text = "Please wait while reading Studio Set names from your INTEGRA-7...";
                    //gridMainPage.Visibility = Visibility.Collapsed;

                    if (commonState.StudioSetNames == null || commonState.StudioSetNames.Count() < 1)
                    {
                        // Get a list of all studio set names. Start by storing the current studio set number.
                        // Note that consequent queries will be sent from MidiInPort_MessageReceived and Timer_Tick.
                        Librarian_StackLayout.IsVisible = false;
                        ShowPleaseWaitPage(WaitingFor.READING_STUDIO_SET_NAMES, CurrentPage.LIBRARIAN, null);
                    }
                    else
                    {
                        // Tell Timer_Tick that we have the studio set names:
                        studioSetNamesJustRead = StudioSetNames.READ_BUT_NOT_LISTED;
                        //QueryCurrentStudioSetNumber(false);
                    }
                }
                else
                {
                    Librarian_lblCategories.Content = "Sound categories";
                    commonState.CurrentTone.Group = (String)Librarian_lvGroups.SelectedItem;
                    PushHandleControlEvents();
                    PopulateCategories();
                    if (Librarian_ocCategories.Count > 0)
                    {
                        Librarian_lvCategories.SelectedItem = Librarian_ocCategories[0];
                        commonState.CurrentTone.Category = Librarian_ocCategories[0];
                        PopulateToneNames();
                    }
                    PopHandleControlEvents();
                }
            }
        }

        private void Librarian_LvCategories_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (initDone && handleControlEvents && !scanning)
            {
                if ((String)Librarian_lvGroups.SelectedItem != "Studio sets")
                {
                    commonState.CurrentTone.Category = (String)Librarian_lvCategories.SelectedItem;
                    // Synthesizer type or extension pack selected.
                    //currentCategoryIndex = Librarian_Categories.IndexOf(commonState.currentTone.Category);

                    //if (AutoUpdateChildLists)
                    //{
                    //UpdateLvToneNames();
                    //}
                    PopulateToneNames();

                    //Librarian_lvToneNames.SelectedItem = Librarian_ToneNames[0];
                    //PopulateToneData();
                }
                else
                {
                    // Studio set selected.
                    // Note that we must not do this when new names was just fetched since
                    // this is called also when populating the group list:
                    if (studioSetNamesJustRead == StudioSetNames.NOT_READ)
                    {
                        commonState.CurrentStudioSet = (byte)(Librarian_ocCategories.IndexOf((String)Librarian_lvCategories.SelectedItem));
                        SetStudioSet((byte)Librarian_ocCategories.IndexOf((String)Librarian_lvCategories.SelectedItem));
                        Librarian_lvToneNames.IsVisible = true;
                        commonState.CurrentPart = (byte)Librarian_midiOutputChannel.SelectedIndex;
                        PartChanged();
                    }
                    else if (studioSetNamesJustRead == StudioSetNames.READ_AND_LISTED)
                    {
                        commonState.CurrentStudioSet = (byte)(Librarian_ocCategories.IndexOf((String)Librarian_lvCategories.SelectedItem));
                        SetStudioSet(commonState.CurrentStudioSet);
                        commonState.CurrentPart = (byte)Librarian_midiOutputChannel.SelectedIndex;
                        GetToneFromI7(); // This will be catched in Librarian_MidiInPort_MessageReceived, and displayed in Timer_Tick.
                        //Librarian_lvGroups.SelectedItem = commonState.CurrentTone.Group;
                        ////Librarian_lvCategories.IsVisible = true;
                        ////Librarian_lvStudioSets.IsVisible = false;
                        //PopulateCategories();
                        Librarian_lvToneNames.IsEnabled = true;
                    }
                }
            }
        }

        //private void Librarian_lvStudioSets_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    if (initDone && handleControlEvents)
        //    {
        //        // Studio set selected.
        //        // Note that we must not do this when new names was just fetched since
        //        // this is called also when populating the group list:
        //        if (!studioSetNamesJustRead)
        //        {
        //            commonState.CurrentStudioSet = (byte)(Librarian_ocStudioSets.IndexOf((String)Librarian_lvStudioSets.SelectedItem));
        //            SetStudioSet(commonState.CurrentStudioSet);
        //            commonState.CurrentPart = (byte)Librarian_midiOutputChannel.SelectedIndex;
        //            Librarian_lvCategories.IsVisible = true;
        //            Librarian_lvStudioSets.IsVisible = false;
        //            Librarian_lvToneNames.IsEnabled = true;
        //            PartChanged();
        //        }
        //    }
        //}

        //private void UpdateLvToneNames()
        //{
        //    t.Trace("private void UpdateLvToneNames()");
        //    if (initDone && !scanning)
        //    {
        //        if (!String.IsNullOrEmpty(commonState.currentTone.Category))
        //        {
        //            // The user selected a category, save and populate tone names:
        //            commonState.currentTone.Category = commonState.currentTone.Category;
        //            PopulateToneNames();
        //        }
        //        else
        //        {
        //            // We came here for 1 of 2 reasons:
        //            // 1 lvGroups_SelectionChanged populates this list
        //            // 2 lvGroups_SelectionChanged changes selected item in this list
        //            if (!String.IsNullOrEmpty(commonState.currentTone.Category))
        //            {
        //                try
        //                {
        //                    PopulateToneNames();
        //                }
        //                catch { }
        //            }
        //            if (!String.IsNullOrEmpty(commonState.currentTone.Name))
        //            {
        //                try
        //                {
        //                    Librarian_lvToneNames.SelectedItem = commonState.currentTone.Name;
        //                    if (IsFavorite())
        //                    {
        //                        Librarian_btnShowFavorites.BackgroundColor = colorSettings.IsFavorite;
        //                    }
        //                    else
        //                    {
        //                        Librarian_btnShowFavorites.BackgroundColor = colorSettings.LabelBackground;
        //                    }
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //}

        private void Librarian_LvToneNames_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (initDone && handleControlEvents)
            {
                if (usingSearchResults)
                {
                    //t.Trace("private void lvSearchResults_SelectionChanged (" + "object" + sender + ", " + "SelectionChangedEventArgs" + e + ", " + ")");
                    String soundName = (String)((ListView)sender).SelectedItem;
                    Boolean drumMap = false;
                    if (!String.IsNullOrEmpty(soundName))
                    {
                        commonState.CurrentTone.Name = soundName;
                    }
                    if (commonState.CurrentTone.Name.EndsWith("\t"))
                    {
                        drumMap = true;
                        commonState.CurrentTone.Name = commonState.CurrentTone.Name.TrimEnd('\t');
                    }
                    String[] parts = commonState.CurrentTone.Name.Split(',');
                    if (parts.Length == 3)
                    {
                        if (drumMap)
                        {
                            commonState.CurrentTone.Group = parts[1].TrimStart();
                            commonState.CurrentTone.Category = "Drum";
                            commonState.CurrentTone.Name = parts[2].TrimStart();
                        }
                        else
                        {
                            commonState.CurrentTone.Group = parts[1].TrimStart();
                            commonState.CurrentTone.Category = parts[2].TrimStart();
                            commonState.CurrentTone.Name = parts[0].TrimStart();
                        }
                        //Librarian_lvGroups.SelectedItem = commonState.currentTone.Group;
                        //Librarian_lvCategories.SelectedItem = commonState.currentTone.Category;
                        //Librarian_lvToneNames.SelectedItem = commonState.currentTone.Name;
                        commonState.CurrentTone.Index = commonState.ToneList.Get(commonState.CurrentTone.Group, commonState.CurrentTone.Category, commonState.CurrentTone.Name);
                    }
                    if (commonState.CurrentTone.Index > -1)
                    {
                        PushHandleControlEvents();
                        try
                        {
                            Librarian_lvGroups.IsEnabled = true;
                            Librarian_lvCategories.IsEnabled = true;
                            Librarian_tbSearch.Text = "";
                            PopulateToneData();
                            PopulateToneNames();
                            Librarian_lvGroups.SelectedItem = commonState.CurrentTone.Group;
                            Librarian_lvCategories.SelectedItem = commonState.CurrentTone.Category;
                            Librarian_lvToneNames.SelectedItem = commonState.CurrentTone.Name;
                        }
                        catch { }
                        commonState.Midi.SetVolume(commonState.CurrentPart, 127);
                        UpdateDrumNames();
                        if (commonState.Player.Playing)
                        {
                            commonState.Player.StopPlaying();
                            commonState.Player.StartPlaying();
                            commonState.Player.WasPlaying = true;
                        }
                        PopHandleControlEvents();
                    }
                }
                else
                {
                    //t.Trace("private void lvToneNames_SelectionChanged (" + "object" + sender + ", " + "SelectionChangedEventArgs" + e + ", " + ")");
                    if (initDone && !scanning)
                    {
                        if (Librarian_lvToneNames.SelectedItem != null && Librarian_lvToneNames.SelectedItem.ToString() != "")
                        {
                            //localSettings.Values["Tone"] = lvToneNames.SelectedIndex;
                            //currentToneNameIndex = Librarian_ToneNames.IndexOf(Librarian_lvToneNames.SelectedItem.ToString());
                            commonState.CurrentTone.Name = (String)Librarian_lvToneNames.SelectedItem;
                            commonState.CurrentTone = new Tone(
                                Librarian_ocGroups.IndexOf(Librarian_lvGroups.SelectedItem.ToString()),
                                Librarian_ocCategories.IndexOf(Librarian_lvCategories.SelectedItem.ToString()),
                                Librarian_ocToneNames.IndexOf(Librarian_lvToneNames.SelectedItem.ToString()),
                                Librarian_lvGroups.SelectedItem.ToString(),
                                Librarian_lvCategories.SelectedItem.ToString(), commonState.CurrentTone.Name);
                            if (!String.IsNullOrEmpty(commonState.CurrentTone.Name))
                            {
                                commonState.CurrentTone.Name = commonState.CurrentTone.Name;
                            }
                            if (!String.IsNullOrEmpty(commonState.CurrentTone.Name))
                            {
                                try
                                {
                                    if (commonState.CurrentTone.Index < 0)
                                    {
                                        commonState.CurrentTone.Index = commonState.ToneList.Get(commonState.CurrentTone);
                                    }
                                    PopulateToneData();
                                    //PushHandleControlEvents();
                                    //try
                                    //{
                                    //    Librarian_lvGroups.IsEnabled = true;
                                    //    Librarian_lvCategories.IsEnabled = true;
                                    //    Librarian_tbSearch.Editor.Text = "";
                                    //    //PopulateToneData();
                                    //    //PopulateToneNames();
                                    //    //Librarian_lvGroups.SelectedItem = commonState.currentTone.Group;
                                    //    //Librarian_lvCategories.SelectedItem = commonState.currentTone.Category;
                                    //    //Librarian_lvToneNames.SelectedItem = commonState.currentTone.Name;
                                    //}
                                    //catch { }
                                    //PopHandleControlEvents();
                                }
                                catch { }
                                commonState.Midi.SetVolume(commonState.CurrentPart, 127);
                                UpdateDrumNames();
                                if (commonState.Player.Playing)
                                {
                                    commonState.Player.StopPlaying();
                                    commonState.Player.StartPlaying();
                                    commonState.Player.WasPlaying = true;
                                }
                            }
                            //Librarian_lvGroups.SelectedItem = commonState.CurrentTone.Group;
                        }
                    }
                }
            }
        }

        //private void Librarian_lvSearchResult_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    if (initDone)
        //    {
        //        //t.Trace("private void lvSearchResults_SelectionChanged (" + "object" + sender + ", " + "SelectionChangedEventArgs" + e + ", " + ")");
        //        String soundName = (String)((ListView)sender).SelectedItem;
        //        Boolean drumMap = false;
        //        if (!String.IsNullOrEmpty(soundName))
        //        {
        //            commonState.currentTone.Name = soundName;
        //        }
        //        if (!String.IsNullOrEmpty(Librarian_tbSearch.Editor.Text))
        //        {
        //            if (commonState.currentTone.Name.EndsWith("\t"))
        //            {
        //                drumMap = true;
        //                commonState.currentTone.Name = commonState.currentTone.Name.TrimEnd('\t');
        //            }
        //            String[] parts = commonState.currentTone.Name.Split(',');
        //            if (parts.Length == 3)
        //            {
        //                if (drumMap)
        //                {
        //                    commonState.currentTone.Group = parts[1].TrimStart();
        //                    commonState.currentTone.Category = "Drum";
        //                    commonState.currentTone.Name = parts[2].TrimStart();
        //                }
        //                else
        //                {
        //                    commonState.currentTone.Group = parts[1].TrimStart();
        //                    commonState.currentTone.Category = parts[2].TrimStart();
        //                    commonState.currentTone.Name = parts[0].TrimStart();
        //                }
        //                Librarian_lvGroups.SelectedItem = commonState.currentTone.Group;
        //                Librarian_lvCategories.SelectedItem = commonState.currentTone.Category;
        //                Librarian_lvToneNames.SelectedItem = commonState.currentTone.Name;
        //                commonState.currentTone.Index = commonState.toneList.Get(Librarian_lvGroups.SelectedItem.ToString(), Librarian_lvCategories.SelectedItem.ToString(), toneName);
        //            }
        //        }
        //        if (!String.IsNullOrEmpty(commonState.currentTone.Name))
        //        {
        //            try
        //            {
        //                PopulateToneData(commonState.toneList.Get(commonState.currentTone.Group, commonState.currentTone.Category, commonState.currentTone.Name));
        //            }
        //            catch { }
        //            commonState.midi.SetVolume(commonState.CurrentPart, 127);
        //            UpdateDrumNames();
        //            if (commonState.player.Playing)
        //            {
        //                commonState.player.StopPlaying();
        //                commonState.player.StartPlaying();
        //                commonState.player.WasPlaying = true;
        //            }
        //        }
        //        Librarian_lvGroups.SelectedItem = commonState.currentTone.Group;
        //    }
        //}

        private void Librarian_FilterPresetAndUser_Clicked(object sender, EventArgs e)
        {
            switch (toneNamesFilter)
            {
                case ToneNamesFilter.INIT:
                    QueryUserTones();
                    break;
                case ToneNamesFilter.ALL:
                    toneNamesFilter = ToneNamesFilter.PRESET;
                    Librarian_filterPresetAndUser.Text = "Preset tones only";
                    break;
                case ToneNamesFilter.PRESET:
                    toneNamesFilter = ToneNamesFilter.USER;
                    Librarian_filterPresetAndUser.Text = "User tones only";
                    break;
                case ToneNamesFilter.USER:
                    toneNamesFilter = ToneNamesFilter.ALL;
                    Librarian_filterPresetAndUser.Text = "Preset and user tones";
                    break;
            }
            commonState.CurrentTone.Category = (String)Librarian_lvCategories.SelectedItem;
            PopulateToneNames();
        }

        private void Librarian_MidiOutputChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initDone)
            {
                commonState.CurrentPart = (byte)Librarian_midiOutputChannel.SelectedIndex;
                PartChanged();
            }

        }

        private void Librarian_btnSettings_Clicked(object sender, EventArgs e)
        {
            Librarian_StackLayout.IsVisible = false;
            ShowSettingsPage();
        }

        private void Librarian_tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (initDone && handleControlEvents)
            {
                //t.Trace("private void tbSearch_TextChanged (" + "object" + sender + ", " + "TextChangedEventArgs" + e + ", " + ")");
                PushHandleControlEvents();
                if (!String.IsNullOrEmpty(Librarian_tbSearch.Text) && Librarian_tbSearch.Text.Length > 2)
                {
                    Librarian_lvGroups.IsEnabled = false;
                    Librarian_lvCategories.IsEnabled = false;
                    usingSearchResults = true;
                    Librarian_PopulateSearchResults();
                }
                else if (String.IsNullOrEmpty(Librarian_tbSearch.Text))
                {
                    Librarian_lvGroups.IsEnabled = true;
                    Librarian_lvCategories.IsEnabled = true;
                    usingSearchResults = false;
                    PopulateToneNames();
                }
                PopHandleControlEvents();
            }
        }

        private void Librarian_btnMinus12keys_Clicked(object sender, EventArgs e)
        {
            if (lowKey >= 12)
            {
                lowKey -= 12;
                ShowKeyNumbering();
            }
        }

        private void Librarian_btnPlus12keys_Clicked(object sender, EventArgs e)
        {
            if (lowKey < 92)
            {
                lowKey += 12;
                ShowKeyNumbering();
            }
        }

        private void Librarian_btnKeys_Clicked(object sender, EventArgs e)
        {
            lowKey = 36;
            ShowKeyNumbering();
        }

        private void ShowKeyNumbering()
        {
            if (lowKey + 3 * 12 > 127)
            {
                Librarian_btnKeys.Text = "Keys " + (lowKey).ToString() + " - 128";
                Librarian_btnWhiteKeys[0].IsVisible = false;
                Librarian_btnWhiteKeys[1].IsVisible = false;
                Librarian_btnWhiteKeys[2].IsVisible = false;
                Librarian_btnWhiteKeys[3].IsVisible = false;
                Librarian_btnBlackKeys[0].IsVisible = false;
                Librarian_btnBlackKeys[1].IsVisible = false;
                //Librarian_btnBlackKeys[2].IsVisible = false;
            }
            else
            {
                Librarian_btnKeys.Text = "Keys " + (lowKey).ToString() + " - " + (lowKey + 36).ToString();
                Librarian_btnWhiteKeys[0].IsVisible = true;
                Librarian_btnWhiteKeys[1].IsVisible = true;
                Librarian_btnWhiteKeys[2].IsVisible = true;
                Librarian_btnWhiteKeys[3].IsVisible = true;
                Librarian_btnBlackKeys[0].IsVisible = true;
                Librarian_btnBlackKeys[1].IsVisible = true;
                //Librarian_btnBlackKeys[2].IsVisible = true;
            }
            UpdateDrumNames();
        }

        private void Librarian_BtnEditTone_Clicked(object sender, EventArgs e)
        {
            //mainStackLayout.Children.RemoveAt(0);
            currentPage = CurrentPage.EDIT_TONE;
            Librarian_StackLayout.IsVisible = false;
            ShowToneEditorPage();
        }

        private void Librarian_btnEditStudioSet_Clicked(object sender, EventArgs e)
        {
            Librarian_StackLayout.IsVisible = false;

            // Studio set editor starts assuming we use part 1, and if some other
            // part has been selected in the Librarian, it will try obtaining 
            // data from possibly wrong class. Therefore, first read in the tone
            // in part 1:

            if (EditStudioSet_IsCreated)
            {
                // If studio set editor is created, studio set and studio set must already
                // have been read, and it is only to go there:
                //QueryCurrentStudioSetNumber(false);
                ShowStudioSetEditorPage();
                //StudioSetEditor_StackLayout.IsVisible = true;
            }
            else if (commonState.StudioSet == null)
            {
                // If studio set has not been read, we must first read that.
                // Let PleaseWait do it, and also check if studio set names are read:
                ShowPleaseWaitPage(WaitingFor.READING_STUDIO_SET, CurrentPage.EDIT_STUDIO_SET, null);
            }
            else if (commonState.StudioSetNames == null || commonState.StudioSetNames.Count() < 1)
            {
                // Get a list of all studio set names. Start by storing the current studio set number.
                // Note that consequent queries will be sent from MidiInPort_MessageReceived and Timer_Tick.
                Librarian_StackLayout.IsVisible = false;
                ShowPleaseWaitPage(WaitingFor.READING_STUDIO_SET_NAMES, CurrentPage.EDIT_STUDIO_SET, null);
            }
            else
            {
                // Studio set and studio set names are read, but studio set page is not created:
                Librarian_StackLayout.IsVisible = false;
                ShowStudioSetEditorPage();
            }
        }

        private void Librarian_btnResetVolume_Clicked(object sender, EventArgs e)
        {
            for (byte i = 0; i < 16; i++)
            {
                commonState.Midi.SetVolume(i, 100);
            }
        }

        private void Librarian_btnMotionalSurround_Clicked(object sender, EventArgs e)
        {
            //Waiting(true, "Please wait while making sensor grid...", MotionalSurround_StackLayout);
            Librarian_StackLayout.IsVisible = false;
            //if (commonState.StudioSet == null || currentStudioSet != commonState.CurrentStudioSet)
            //{
            currentStudioSet = commonState.CurrentStudioSet;
            ShowPleaseWaitPage(WaitingFor.READING_STUDIO_SET, CurrentPage.MOTIONAL_SURROUND, null);
            //}
            //else
            //{
            //    ShowMotionalSurroundPage();
            //}
            //ShowMotionalSurroundPage();
        }

        //private async void Settings_Tapped(object sender, EventArgs e)
        //{
        //    Librarian_StackLayout.IsVisible = false;
        //    ShowSettingsPage();
        //Boolean rebootNeeded = false;
        //String response = await mainPage.DisplayActionSheet(
        //    "INTEGRA-7 Librarian and Editor",
        //    "Cancel",
        //    null, new String[] {
        //        "Always put bank number in clipboard",
        //        "Do not put bank number in clipboard",
        //        "Light colors",
        //        "Forrest colors",
        //        "Ocean colors",
        //        "Rose colors",
        //        "Sunny colors",
        //        "Brown colors"
        //});

        //if (response == "Always put bank number in clipboard")
        //{
        //    mainPage.SaveLocalValue("PutBankInClipboard", true);
        //    putBankInClipboard = true;
        //}
        //else if (response == "Do not put bank number in clipboard")
        //{
        //    mainPage.SaveLocalValue("PutBankInClipboard", false);
        //    putBankInClipboard = false;
        //}
        //else if (response == "Light colors")
        //{
        //    mainPage.SaveLocalValue("ColorSettings", "LIGHT");
        //    colorSettings = new ColorSettings(_colorSettings.LIGHT);
        //    rebootNeeded = true;
        //}
        //else if (response == "Forrest colors")
        //{
        //    mainPage.SaveLocalValue("ColorSettings", "FOREST");
        //    colorSettings = new ColorSettings(_colorSettings.FOREST);
        //    rebootNeeded = true;
        //}
        //else if (response == "Ocean colors")
        //{
        //    mainPage.SaveLocalValue("ColorSettings", "OCEAN");
        //    colorSettings = new ColorSettings(_colorSettings.OCEAN);
        //    rebootNeeded = true;
        //}
        //else if (response == "Rose colors")
        //{
        //    mainPage.SaveLocalValue("ColorSettings", "ROSE");
        //    colorSettings = new ColorSettings(_colorSettings.ROSE);
        //    rebootNeeded = true;
        //}
        //else if (response == "Sunny colors")
        //{
        //    mainPage.SaveLocalValue("ColorSettings", "SUNNY");
        //    colorSettings = new ColorSettings(_colorSettings.SUNNY);
        //    rebootNeeded = true;
        //}
        //else if (response == "Brown colors")
        //{
        //    mainPage.SaveLocalValue("ColorSettings", "BROWN");
        //    colorSettings = new ColorSettings(_colorSettings.BROWN);
        //    rebootNeeded = true;
        //}

        //if (rebootNeeded)
        //{
        //    await mainPage.DisplayAlert("INTEGRA-7 Librarian and Editor", 
        //        "The new colors will show when you restart the app!", "Got it");
        //}
        //}

        private void Librarian_btnFavorites_Clicked(object sender, EventArgs e)
        {
            Librarian_StackLayout.IsVisible = false;
            ShowFavoritesPage(FavoritesAction.SHOW);
        }

        private void Librarian_btnAddFavorite_Clicked(object sender, EventArgs e)
        {
            Librarian_StackLayout.IsVisible = false;
            ShowFavoritesPage(FavoritesAction.ADD);
        }

        private void Librarian_btnRemoveFavorite_Clicked(object sender, EventArgs e)
        {
            Librarian_StackLayout.IsVisible = false;
            ShowFavoritesPage(FavoritesAction.REMOVE);
        }

        private void Librarian_btnResetHangingNotes_Clicked(object sender, EventArgs e)
        {
            for (byte i = 0; i < 16; i++)
            {
                commonState.Midi.AllNotesOff(i);
            }
        }

        private void PartChanged()
        {
            if (commonState.CurrentPart < 16 && localSettings != null)
            {
                mainPage.SaveLocalValue("MidiChannel", commonState.CurrentPart);
                commonState.Midi.SetMidiInPortChannel((byte)Librarian_midiOutputChannel.SelectedIndex);
                commonState.Midi.SetMidiOutPortChannel((byte)Librarian_midiOutputChannel.SelectedIndex);
                GetToneFromI7();
            }
            //if (cbChannel.SelectedIndex > -1 && cbChannel.SelectedIndex < 16 && commonState != null && commonState.midi != null)
            //{
            //    commonState.midi.MidiInPortChannel = (byte)cbChannel.SelectedIndex;
            //    commonState.midi.MidiOutPortChannel = (byte)cbChannel.SelectedIndex;
            //    //EditButton.IsEnabled = cbChannel.SelectedIndex == 0;
            //    EnableOrDisableEditButton();
            //}
        }

        private void Librarian_btnPlay_Clicked(object sender, EventArgs e)
        {
            if (Librarian_btnPlay.Text == "Stop")
            {
                byte[] address = new byte[] { 0x0f, 0x00, 0x20, 0x00 };
                byte[] data = new byte[] { 0x00 };
                byte[] package = commonState.Midi.SystemExclusiveDT1Message(address, data);
                commonState.Midi.SendSystemExclusive(package);
                for (byte i = 0; i < 16; i++)
                {
                    Librarian_btnPlay.Text = "Play";
                    if (btnEditTone_Play != null)
                    {
                        btnEditTone_Play.Text = "Play";
                    }
                    if (Favorites_btnPlay != null)
                    {
                        Favorites_btnPlay.Text = "Play";
                    }
                }
            }
            else
            {
                byte[] address = new byte[] { 0x0f, 0x00, 0x20, 0x00 };
                byte[] data = new byte[] { (byte)(commonState.CurrentPart + 1) };
                byte[] package = commonState.Midi.SystemExclusiveDT1Message(address, data);
                commonState.Midi.SendSystemExclusive(package);
                Librarian_btnPlay.Text = "Stop";
                if (btnEditTone_Play != null)
                {
                    btnEditTone_Play.Text = "Stop";
                }
                if (Favorites_btnPlay != null)
                {
                    Favorites_btnPlay.Text = "Stop";
                }
            }
        }

        private void Librarian_btnWhiteKey_Pressed(object sender, EventArgs e)
        {
            byte[] keyNumbers = new byte[] { 36, 35, 33, 31, 30, 28, 26, 24, 23, 21, 19, 17, 16, 14, 12, 11, 9, 7, 5, 4, 2, 0 };
            byte noteNumber = (byte)(keyNumbers[Int32.Parse(((PianoKey)sender).StyleId)] + lowKey);
            if (noteNumber == currentNote)
            {
                commonState.Midi.NoteOff(commonState.CurrentPart, currentNote);
                currentNote = 255;
            }
            else if (noteNumber < 128)
            {
                if (currentNote < 128)
                {
                    commonState.Midi.NoteOff(commonState.CurrentPart, currentNote);
                }
                currentNote = noteNumber;
                commonState.Midi.NoteOn(commonState.CurrentPart, noteNumber, 64);
            }
        }

        private void Librarian_btnWhiteKey_Released(object sender, EventArgs e)
        {
            byte[] keyNumbers = new byte[] { 36, 35, 33, 31, 30, 28, 26, 24, 23, 21, 19, 17, 16, 14, 12, 11, 9, 7, 5, 4, 2, 0 };
            byte noteNumber = (byte)(keyNumbers[Int32.Parse(((PianoKey)sender).StyleId)] + lowKey);
            if (noteNumber == currentNote)
            {
                commonState.Midi.NoteOff(commonState.CurrentPart, currentNote);
                currentNote = 255;
            }
            else if (noteNumber < 128)
            {
                if (currentNote < 128)
                {
                    commonState.Midi.NoteOff(commonState.CurrentPart, currentNote);
                }
                currentNote = noteNumber;
                commonState.Midi.NoteOn(commonState.CurrentPart, noteNumber, 64);
            }
        }

        private void Librarian_btnBlackKey_Pressed(object sender, EventArgs e)
        {
            byte[] keyNumbers = new byte[] { 34, 32, 29, 27, 25, 22, 20, 18, 15, 13, 10, 8, 6, 3, 1 };
            byte noteNumber = (byte)(keyNumbers[Int32.Parse(((PianoKey)sender).StyleId)] + lowKey);
            if (noteNumber == currentNote)
            {
                commonState.Midi.NoteOff(commonState.CurrentPart, currentNote);
                currentNote = 255;
            }
            else if (noteNumber < 128)
            {
                if (currentNote < 128)
                {
                    commonState.Midi.NoteOff(commonState.CurrentPart, currentNote);
                }
                currentNote = noteNumber;
                commonState.Midi.NoteOn(commonState.CurrentPart, noteNumber, 64);
            }
        }

        private void Librarian_btnBlackKey_Released(object sender, EventArgs e)
        {
            byte[] keyNumbers = new byte[] { 34, 32, 29, 27, 25, 22, 20, 18, 15, 13, 10, 8, 6, 3, 1 };
            byte noteNumber = (byte)(keyNumbers[Int32.Parse(((PianoKey)sender).StyleId)] + lowKey);
            if (noteNumber == currentNote)
            {
                commonState.Midi.NoteOff(commonState.CurrentPart, currentNote);
                currentNote = 255;
            }
            else if (noteNumber < 128)
            {
                if (currentNote < 128)
                {
                    commonState.Midi.NoteOff(commonState.CurrentPart, currentNote);
                }
                currentNote = noteNumber;
                commonState.Midi.NoteOn(commonState.CurrentPart, noteNumber, 64);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Librarian functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private Boolean IsInitTone(byte[] data)
        {
            //t.Trace("private Boolean IsInitTone (" + "byte[]" + data + ", " + ")");
            char[] init = "INIT TONE   ".ToCharArray();
            Boolean initTone = true;
            for (byte i = 0; i < 12; i++)
            {
                if (init[i] != data[i + 11])
                {
                    initTone = false;
                    break;
                }
            }
            return initTone;
        }

        private Boolean IsInitKit(byte[] data)
        {
            //t.Trace("private Boolean IsInitKit (" + "byte[]" + data + ", " + ")");
            char[] init = "INIT KIT    ".ToCharArray();
            Boolean initTone = true;
            for (byte i = 0; i < 12; i++)
            {
                if (init[i] != data[i + 11])
                {
                    initTone = false;
                    break;
                }
            }
            return initTone;
        }

        /// <summary>
        /// Queries I-7 for user tones to add to the voicelist
        /// </summary>
        private async void QueryUserTones()
        {
            //// Start with PCM Synth Tone, MainPage_MidiInPort_MessageReceived and Timer_Tick will handle the rest:
            //Boolean response = await mainPage.DisplayAlert("INTEGRA_7 Librarian", "Do you want the librarian to scan " +
            //    "your INTEGRA-7 for user tones, or will you only use the INTEGRA-7 preset tones?\r\n\r\n" +
            //    "Note: Scanning will change Tone on your INTEGRA-7, part 16.", "Yes", "No");

            //if (response)
            {
                toneNamesFilter = ToneNamesFilter.ALL;
                Librarian_filterPresetAndUser.Text = "Preset and user tones";
                scanAll = await mainPage.DisplayAlert("INTEGRA_7 Librarian", "This could take a while, so please select " +
                    "scanning option below:", "Scan all user tone slots", "Scan only until 10 empty slots are found in row");
                msb = 87;
                lsb = 0;
                pc = 1;
                emptySlots = 10;
                scanning = true;
                userToneNumbers = new ushort[128];
                for (byte i = 0; i < 128; i++)
                {
                    userToneNumbers[i] = 0;
                }
                emptySlots = 0;
                QueryUserPCMSyntTones();
            }
        }

        // Use GetToneFromI7() to read current tone from INTEGRA-7.
        private void GetToneFromI7()
        {
            if (initDone)// && AutoUpdateChildLists)
            {
                t.Trace("private void GetToneFromI7()");
                // Read MSB, LSB and PC from Studio set at current part:
                byte[] address = new byte[] { 0x18, 0x00, (byte)(0x20 + commonState.CurrentPart), 0x00 };
                byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x09 };
                byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
                queryType = QueryType.READ_TONE_FROM_I7;
                commonState.Midi.SendSystemExclusive(message);
            }
        }

        // These 5 functions will change program on channel 16 and query to get the name.
        private void QueryUserPCMSyntTones()
        {
            //t.Trace("private void QueryUserPCMSyntTones()");
            commonState.Midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x1c, 0x60, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x0c };
            byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.PCM_SYNTH_TONE_COMMON;
            commonState.Midi.SendSystemExclusive(message);
        }
        private void QueryUserPCMDrumKitTones()
        {
            //t.Trace("private void QueryUserPCMDrumKitTones()");
            commonState.Midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x1c, 0x70, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x0c };
            byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.PCM_DRUM_KIT_COMMON;
            commonState.Midi.SendSystemExclusive(message);
        }
        private void QueryPcmDrumKitKeyName(byte Key)
        {
            //t.Trace("private void QueryPcmDrumKitKeyName()");
            byte[] address = new byte[] { 0x1c, 0x70, 0x10, 0x00 };
            address = hex2Midi.AddBytes128(address, new byte[] { 0x00, 0x00, Key, 0x00 });
            address = hex2Midi.AddBytes128(address, new byte[] { 0x00, 0x00, Key, 0x00 });
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x0c };
            byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.PCM_KEY_NAME;
            commonState.Midi.SendSystemExclusive(message);
        }
        private void QueryUserSuperNaturalAcousticTones()
        {
            //t.Trace("private void QueryUserSuperNaturalAcousticTones()");
            commonState.Midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x1c, 0x62, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x46 };
            byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.SUPERNATURAL_ACOUSTIC_TONE_COMMON;
            commonState.Midi.SendSystemExclusive(message);
        }
        private void QueryUserSuperNaturalSynthTones()
        {
            //t.Trace("private void QueryUserSuperNaturalSynthTones()");
            commonState.Midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x1c, 0x61, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x40 };
            byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.SUPERNATURAL_SYNTH_TONE_COMMON;
            commonState.Midi.SendSystemExclusive(message);
        }
        private void QueryUserSuperNaturalDrumKitTones()
        {
            //t.Trace("private void QueryUserSuperNaturalDrumKitTones()");
            commonState.Midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x1c, 0x63, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x0e };
            byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.SUPERNATURAL_DRUM_KIT_COMMON;
            commonState.Midi.SendSystemExclusive(message);
        }
        private void QuerySnDrumKitKeyName(byte Key)
        {
            //t.Trace("private void QuerySnDrumKitKeyName()");
            byte[] address = new byte[] { 0x1c, 0x63, 0x10, 0x00 };
            address = hex2Midi.AddBytes128(address, new byte[] { 0x00, 0x00, Key, 0x00 });
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x04 };
            byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.SND_KEY_NAME;
            commonState.Midi.SendSystemExclusive(message);
        }
        public void QuerySelectedStudioSet()
        {
            //t.Trace("private void QuerySelectedStudioSet()");
            //commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x01, 0x00, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x07 };
            byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.CURRENT_SELECTED_STUDIO_SET;
            commonState.Midi.SendSystemExclusive(message);
        }

        private void QuerySelectedTone()
        {
            //t.Trace("private void QuerySelectedTone()");
            //commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x18, 0x00, (byte)(0x20 + commonState.CurrentPart), 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x09 };
            byte[] message = commonState.Midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.CURRENT_SELECTED_TONE;
            commonState.Midi.SendSystemExclusive(message);
        }

        public void ShowLibrarianPage()
        {
            currentPage = CurrentPage.LIBRARIAN;
            if (!Librarian_IsCreated)
            {
                //DrawLibrarianPage(); // Librarian page is created in advace because the MIDI pickers must be present
                // before creating the MIDI object, and the MIDI object is needed before showing the librarian page.
                mainStackLayout.Children.Add(Librarian_StackLayout);
                //DrawPleaseWaitPage(); // Librarian is always first to start, but this page is used by other pages.
                //mainStackLayout.Children.Add(PleaseWait_StackLayout);
                Librarian_IsCreated = true;

                // We need favorites already now, so let's create it:
                commonState.FavoritesList = new FavoritesList();
                Favorites_ReadFavoritesFromLocalSettings();
                DrawFavoritesPage();
                mainStackLayout.Children.Add(Favorites_StackLayout);
                Favorites_StackLayout.IsVisible = false;
                Favorites_IsCreated = true;
                Librarian_Init();
                Favorites_UpdateFoldersList();
                needsToSetFontSizes = NeedsToSetFontSizes.FAVORITES;
            }
            //else if(MidiState == MIDIState.INITIALIZING)
            //{
            //    // I-7 was not found via USB, and the user has indicated a MIDI interface
            //    // to use to contact I-7 via 5-pin connectors. Try to connect to open it:
            //    ShowPleaseWaitPage(WaitingFor.INTEGRA_7, null);
            //}
            PopHandleControlEvents();
            //Favorites_UpdateFoldersList();
            //mainStackLayout.Children.Add(LibrarianStackLayout);
            Librarian_StackLayout.IsVisible = true;
            if (Librarian_ocGroups.Count == 0)
            {
                PopulateGroups();
            }

            if (commonState != null && commonState.CurrentTone != null)
            {
                Librarian_lvGroups.SelectedItem = commonState.CurrentTone.Group;
                Librarian_lvCategories.SelectedItem = commonState.CurrentTone.Category;
                Librarian_lvToneNames.SelectedItem = commonState.CurrentTone.Name;
            }

            //// Set font size:
            //SetFontSizes(Librarian_StackLayout);
        }

        private void PopulateGroups()
        {
            PushHandleControlEvents();
            foreach (List<String> tone in commonState.ToneList.Tones)
            {
                if (!Librarian_ocGroups.Contains(tone[0]))
                {
                    Librarian_ocGroups.Add(tone[0]);
                }
            }
            Librarian_lvGroups.SelectedItem = "SuperNATURAL Acoustic Tone";
            PopHandleControlEvents();
        }

        private void PopulateCategories()
        {
            //t.Trace("private void PopulateCategories (" + "String" + group + ", " + ")");
            String lastCategory = "";
            PushHandleControlEvents();
            Librarian_ocCategories.Clear();
            foreach (List<String> line in commonState.ToneList.Tones.OrderBy(o => o[1]))
            {
                if (line[0] == commonState.CurrentTone.Group && line[1] != lastCategory && !Librarian_ocCategories.Contains(line[1]))
                {
                    Librarian_ocCategories.Add(line[1]);
                    lastCategory = line[1];
                }
            }
            Librarian_lvCategories.ItemsSource = Librarian_ocCategories;
            PopHandleControlEvents();
            //Librarian_lvCategories.SelectedItem = Librarian_Categories[0];
        }

        private void Librarian_PopulateStudioSetListview()
        {
            // All studio set names has been received and stored in studioSetNames,
            // populate the combobox:
            PushHandleControlEvents();
            Librarian_ocCategories.Clear();
            UInt16 i = 1;
            foreach (String s in commonState.StudioSetNames)
            {
                String num = i.ToString();
                if (num.Length < 2)
                {
                    num = "0" + num;
                }
                Librarian_ocCategories.Add(num + " " + s);
                i++;
            }
            commonState.CurrentStudioSet = currentStudioSet;
            Librarian_lvCategories.SelectedItem = commonState.CurrentStudioSet;
            PopHandleControlEvents();
            SetStudioSet(commonState.CurrentStudioSet);
        }

        private void PopulateToneNames()
        {
            //t.Trace("private void PopulateToneNames (" + "String" + category + ", " + ")");
            if (initDone || !scanning)
            {
                try
                {
                    if (Librarian_ocToneNames == null)
                    {
                        try
                        {
                            Librarian_ocToneNames = new ObservableCollection<String>();
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    if (Librarian_ocToneNames.Count() > 0)
                    {
                        try
                        {
                            Librarian_ocToneNames.Clear();
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    String group = Librarian_lvGroups.SelectedItem.ToString();
                    foreach (List<String> tone in commonState.ToneList.Tones.OrderBy(o => o[3]))
                    {
                        if (tone[0] == group && tone[1] == commonState.CurrentTone.Category
                            && (toneNamesFilter == ToneNamesFilter.USER && tone[8] == "(User)"
                            || toneNamesFilter == ToneNamesFilter.PRESET && tone[8] != "(User)"
                            || toneNamesFilter == ToneNamesFilter.INIT
                            || toneNamesFilter == ToneNamesFilter.ALL))
                        {
                            Librarian_ocToneNames.Add(tone[3]);
                        }
                    }
                    //Librarian_lvToneNames.SelectedItem = Librarian_ToneNames[0];
                    if (commonState.CurrentTone == null)
                    {
                        List<String> toneData = new List<String>();
                        toneData.Add(Librarian_lvGroups.SelectedItem.ToString());
                        toneData.Add(Librarian_lvCategories.SelectedItem.ToString());
                        toneData.Add("");
                        toneData.Add(Librarian_ocToneNames[0].ToString());
                        toneData.Add("");
                        toneData.Add("");
                        toneData.Add("");
                        toneData.Add("");
                        toneData.Add("");
                        toneData.Add("-1");
                        commonState.CurrentTone = new Tone(toneData);
                    }
                    if (!String.IsNullOrEmpty(commonState.CurrentTone.Name))
                    {
                        try
                        {
                            Librarian_lvToneNames.SelectedItem = commonState.CurrentTone.Name;
                            if (IsFavorite())
                            {
                                Librarian_btnShowFavorites.BackgroundColor = colorSettings.IsFavorite;
                                Librarian_btnShowFavorites.Text = "Favorite";
                                Librarian_btnRemoveFavorite.IsEnabled = true;
                            }
                            else
                            {
                                Librarian_btnShowFavorites.BackgroundColor = colorSettings.Background;
                                Librarian_btnShowFavorites.Text = "Favorites";
                                Librarian_btnRemoveFavorite.IsEnabled = false;
                            }
                        }
                        catch
                        {
                            Librarian_lvToneNames.SelectedItem = "";
                        }
                    }
                    //else if (Librarian_ToneNames.Count > 0)
                    //{
                    //    Librarian_lvToneNames.SelectedItem = Librarian_ToneNames[0];
                    //}
                }
                catch (Exception e)
                {

                }
                //SetFavorite();
                Librarian_lvToneNames.ItemsSource = Librarian_ocToneNames;
            }
        }

        private void Librarian_PopulateSearchResults()
        {
            try
            {
                //SearchResultSource.Clear();
                Librarian_ocToneNames.Clear();
            }
            catch { }
            Librarian_ocToneNames.Add("=== Tones =============");
            String searchString = Librarian_tbSearch.Text.ToLower();
            // Search voices:
            foreach (List<String> tone in commonState.ToneList.Tones)
            {
                if (tone[3].ToLower().Contains(searchString)
                    && (toneNamesFilter == ToneNamesFilter.USER && tone[8] == "(User)"
                    || toneNamesFilter == ToneNamesFilter.PRESET && tone[8] != "(User)"
                    || toneNamesFilter == ToneNamesFilter.INIT
                    || toneNamesFilter == ToneNamesFilter.ALL))
                {
                    Librarian_ocToneNames.Add(tone[3] + ", " + tone[0] + ", " + tone[1]);
                }
            }
            // Search drum sounds:
            Librarian_ocToneNames.Add("=== Drums ==============");
            Boolean first = true; // To skip the key number column
            foreach (List<String> toneNames in commonState.DrumKeyAssignLists.ToneNames)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    int skip = 2; // To skip the Group and Category names
                    foreach (String toneName in toneNames)
                    {
                        if (skip > 0)
                        {
                            skip--;
                        }
                        else
                        {
                            if (toneName.ToLower().Contains(searchString))
                            {
                                Librarian_ocToneNames.Add(toneName + ", " + toneNames[0] + ", " + toneNames[1] + "\t");
                            }
                        }
                    }
                }
            }
            Librarian_lvToneNames.ItemsSource = Librarian_ocToneNames;
        }

        private void PopulateToneData()
        {
            //t.Trace("private void PopulateToneData (" + "Int32" + Index + ", " + ")");
            if (commonState.CurrentTone.Index > -1)
            {
                List<String> tone = commonState.ToneList.Tones[commonState.CurrentTone.Index];
                Librarian_ltToneName.Text = tone[3];
                Librarian_ltType.Text = tone[8];
                Librarian_ltToneNumber.Text = tone[2];
                Librarian_ltBankAddress.Text = tone[6];
                Librarian_ltPatchMSB.Text = tone[4];
                Librarian_ltPatchLSB.Text = tone[5];
                Librarian_ltProgramNumber.Text = tone[7];
                //commonState.Midi.ProgramChange(commonState.Midi.GetMidiOutPortChannel(), tone[4], tone[5], tone[7]);
                if (putBankInClipboard)
                {
                    CrossClipboard.Current.SetText(Librarian_ltBankAddress.Text);
                }
                if (updateIntegra7)
                {
                    commonState.Midi.ProgramChange(commonState.CurrentPart, tone[4], tone[5], tone[7]);
                }
                UpdateDrumNames();
                if (IsFavorite())
                {
                    Librarian_btnShowFavorites.BackgroundColor = colorSettings.IsFavorite;
                    Librarian_btnRemoveFavorite.IsEnabled = true;
                }
                else
                {
                    Librarian_btnShowFavorites.BackgroundColor = colorSettings.Background;
                    Librarian_btnRemoveFavorite.IsEnabled = false;
                }
                Librarian_btnAddFavorite.IsEnabled = true;
                try
                {
                    commonState.GetToneType((byte)Int32.Parse(tone[4]), (byte)Int32.Parse(tone[5]), (byte)Int32.Parse(tone[7]));
                }
                catch { }
            }
        }

        private void UpdateDrumNames()
        {
            //t.Trace("private void UpdateDrumNames()");
            ClearKeyNames();
            if (commonState.CurrentTone != null && commonState.CurrentTone.Category == "Drum"
                && commonState.DrumKeyAssignLists.KeyboardNameList(commonState.CurrentTone.Group, commonState.CurrentTone.Name) != null)
            {
                commonState.KeyNames = new List<String>();
                foreach (String keyName in commonState.DrumKeyAssignLists.KeyboardNameList(commonState.CurrentTone.Group, commonState.CurrentTone.Name))
                {
                    commonState.KeyNames.Add(keyName);
                }

                if (commonState.KeyNames != null && commonState.KeyNames.Count() > 0)
                {
                    for (Int32 i = 0; i < 37; i++)
                    {
                        if (i + lowKey - 21 > -1 && i + lowKey - 21 < commonState.KeyNames.Count())
                        {
                            SetKeyText(i, commonState.KeyNames[i + lowKey - 21]);
                        }
                    }
                }
            }
        }

        private Boolean IsFavorite()
        {
            //t.Trace("private Boolean IsFavorite()");
            if (commonState.FavoritesList != null)
            {
                foreach (FavoritesFolder folder in commonState.FavoritesList.FavoritesFolders)
                {
                    foreach (FavoriteTone favorite in folder.FavoriteTones)
                    {
                        if (favorite.Group == commonState.CurrentTone.Group
                            && favorite.Category == commonState.CurrentTone.Category
                            && favorite.Name == commonState.CurrentTone.Name)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void Librarian_SynchronizeListviews()
        {
            Librarian_lvGroups.SelectedItem = commonState.CurrentTone.Group;
            Librarian_lvCategories.SelectedItem = commonState.CurrentTone.Category;
            Librarian_lvToneNames.SelectedItem = commonState.CurrentTone.Name;
        }

        //private void ReadFavorites()
        //{
        //    try
        //    {
        //        if (localSettings.Values.ContainsKey("Favorites"))
        //        {
        //            if (commonState.favoritesList == null)
        //            {
        //                commonState.favoritesList = new FavoritesList();
        //                commonState.favoritesList.folders = new List<FavoritesFolder>();
        //            }
        //            commonState.favoritesList.folders.Clear();
        //            String foldersWithFavorites = ((String)localSettings.Values["Favorites"]).Trim('\n');
        //            // Format: [Folder name\v[Group index\tCategory index\tTone index\tGroup\tCategory\tTone\b]\f...]...
        //            // I.e. Split all by \f to get all folders with content.
        //            // Split each folder by \v to get folder name and all favorites together.
        //            // Split favorites by \b to get all favorites one by one.
        //            // Split each favorite by \t to get the 6 parts (3 indexes, 3 names).
        //            FavoritesFolder folder = null;
        //            foreach (String foldersWithFavoritePart in foldersWithFavorites.Split('\f'))
        //            {
        //                String[] folderWithFavorites = foldersWithFavoritePart.Split('\v');
        //                // Folder name:
        //                folder = new FavoritesFolder(folderWithFavorites[0]);
        //                commonState.favoritesList.folders.Add(folder);
        //                if (folderWithFavorites.Length > 1)
        //                {
        //                    String[] favoritesList = folderWithFavorites[1].Split('\b');
        //                    foreach (String favorite in favoritesList)
        //                    {
        //                        String[] favoriteParts = favorite.Split('\t');
        //                        try
        //                        {
        //                            if (favoriteParts.Length == 6)
        //                            {
        //                                // Add strings for Group, Category and Tone name:
        //                                // commonState.favoritesList.folders.Add(new String[] { favoriteParts[3], favoriteParts[4], favoriteParts[5] });
        //                                folder.FavoritesTones.Add(new Tone(Int32.Parse(favoriteParts[0]), Int32.Parse(favoriteParts[1]),
        //                                    Int32.Parse(favoriteParts[2]), favoriteParts[3], favoriteParts[4], favoriteParts[5]));
        //                            }
        //                        }
        //                        catch { }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //}

        //private Note NoteFromMousePosition(Double x, Double y)
        //{
        //    Note key = new Note();
        //    key.NoteNumber = 255;
        //    key.Velocity = 255;

        //    Double keyBoardWidth = Librarian_Keyboard.Width;

        //    // y is based on entire screen. Remove diff to image top:
        //    y -= (mainPage.Height - Librarian_Keyboard.Height);

        //    Double keyWidthAll = Librarian_Keyboard.Width / 61.7;
        //    Double keyWidthWhite = Librarian_Keyboard.Width / 36;
        //    Int32 keyNumber = 0;
        //    if (y > (0.56 * Librarian_Keyboard.Height))
        //    {
        //        // White key area
        //        keyNumber = (byte)((double)x / keyWidthWhite);
        //        if (keyNumber > 35)
        //        {
        //            keyNumber = 35;
        //        }
        //        key.NoteNumber = (byte)(whiteKeys[keyNumber] + transpose);
        //        if (currentNote > 127)
        //        {
        //            currentNote = 127;
        //        }
        //        key.Velocity = (byte)(127 * (y - (0.56 * Librarian_Keyboard.Height)) / (0.44 * Librarian_Keyboard.Height));
        //        if (key.Velocity > 127)
        //        {
        //            key.Velocity = 127;
        //        }
        //    }
        //    else
        //    {
        //        // All keys area
        //        keyNumber = (byte)((double)x / keyWidthAll);
        //        if (keyNumber > 60)
        //        {
        //            keyNumber = 60;
        //        }
        //        key.NoteNumber = (byte)(keyNumber + 36 + transpose);
        //        if (currentNote > 127)
        //        {
        //            currentNote = 127;
        //        }
        //        key.Velocity = (byte)(127 * (y / (0.56 * Librarian_Keyboard.Height)));
        //        if (key.Velocity > 127)
        //        {
        //            key.Velocity = 127;
        //        }
        //    }
        //    return key;
        //}

        private void ClearKeyNames()
        {
            //t.Trace("private void ClearKeyNames()");
            for (Int32 key = 0; key < 37; key++)
            {
                SetKeyText(key, "");
            }
        }

        private void SetKeyText(Int32 Key, String Text)
        {
            //t.Trace("private void SetKeyText (" + "Int32" + Key + ", " + "String" + Text + ", " + ")");
            Int32 tempKeyNumber; // Derived from tone and lowKey, but then transformed to indicate actual key button.
            // keyIndexes to find key buttons. If < 200 use as index in whiteKeys, else subtract 200 and use to index blackKeys
            Int32[] keyIndexes = new Int32[] { 21, 214, 20, 213, 19, 18, 212, 17, 211, 16, 210, 15,
                                               14, 209, 13, 208, 12, 11, 207, 10, 206, 09, 205, 08,
                                               07, 203, 06, 204, 05, 04, 202, 03, 201, 02, 200, 01, 00 };
            tempKeyNumber = Key;// - lowKey;
            if (keyIndexes[tempKeyNumber] < 200)
            {
                Librarian_btnWhiteKeys[keyIndexes[tempKeyNumber]].Text = Text;
            }
            else
            {
                Librarian_btnBlackKeys[keyIndexes[tempKeyNumber] - 200].Text = Text;
            }
        }

        //private void PlayNote(byte note, Int32 length)
        //{
        //    //t.Trace("private void PlayNote (" + "byte" + note + ", " + "Int32" + length + ", " + ")");
        //    commonState.midi.NoteOn(commonState.CurrentPart, note, 92);
        //    Task.Delay(length).Wait();
        //    commonState.midi.NoteOff(commonState.CurrentPart, note);
        //}

        //private void SetFavorite()
        //{
        //    //t.Trace("private void SetFavorite()");
        //    btnAddFavorite.IsEnabled = true;
        //    btnRemoveFavorite.IsEnabled = true;
        //}
    }
    class Note
    {
        HBTrace t = new HBTrace("class Note");
        public byte NoteNumber { get; set; }
        public byte Velocity { get; set; }
    }
}
