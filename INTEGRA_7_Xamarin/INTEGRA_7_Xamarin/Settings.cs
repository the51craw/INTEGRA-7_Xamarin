using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public partial class UIHandler
    {
        private Grid Settings_gridMain;
        private Grid Settings_gridTopLeft;
        private Grid Settings_gridTopRight;
        private Grid Settings_gridBottomLeft;
        private Grid Settings_gridBottomRight;
        private Grid Settings_gridLeftFiller;
        private Grid Settings_gridRightFiller;
        private Grid Settings_gridControls;
        private Grid Settings_gridSamples;

        private LabeledSwitch Settings_lsAutomaticSelectConnection;
        private LabeledSwitch Settings_lsBankNumberToClipboard;
        private Label Settings_lblColorSchemeSelector;
        private Picker Settings_pColorTypeSelector;
        private Label Settings_lblColorTypeSelector;
        private Picker Settings_pColorSchemeSelector;
        private Slider Settings_slRed;
        private Slider Settings_slGreen;
        private Slider Settings_slBlue;
        private Slider Settings_slAlpha;

        private Button Settings_btnSave;
        private Button Settings_btnRestore;
        private Button Settings_btnRandomColors;
        private Button Settings_btnReturn;

        private Label Settings_lblRed;
        private Label Settings_lblGreen;
        private Label Settings_lblBlue;
        private Label Settings_lblAlpha;

        private Switch Settings_sSwitch;
        private Button Settings_btnButton;
        private Label Settings_lblLabel;
        private FavoritesButton Settings_btnFavorites;
        private ProgressBar Settings_pProgress;
        private MotionalSurroundPartLabel Settings_mslMotionalSurroundPartLabel;
        private Label Settings_lblSlider;
        private Slider Settings_slSlider;
        private ListView Settings_lvListView;
        private DataTemplate Settings_ListsTextColor { get; set; }
        private ObservableCollection<String> Settings_ocListView;
        private Picker Settings_pPicker;
        private PianoKey Settings_btnWhitePianoKey;
        private PianoKey Settings_btnBlackPianoKey;
        private Boolean Settings_mslMotionalSurroundPartLabelHasFocus = false;

        private ColorSettings Settings_UserColorSettings;
        public Int32 CurrentColorScheme { get; set; }

        public void ShowSettingsPage()
        {
            currentPage = CurrentPage.SETTINGS;

            if (!Settings_IsCreated)
            {
                DrawSettingsPage();
                Settings_StackLayout.MinimumWidthRequest = 1;
                mainStackLayout.Children.Add(Settings_StackLayout);
                Settings_IsCreated = true;
                PopHandleControlEvents();
                needsToSetFontSizes = NeedsToSetFontSizes.MOTIONAL_SURROUND;
                //ColorElements = new ColorElements();
                if (CurrentColorScheme == 0)
                {
                    Settings_UserColorSettings = new ColorSettings(_colorSettings.LIGHT);
                }
                else if (CurrentColorScheme == 1)
                {
                    Settings_UserColorSettings = new ColorSettings(_colorSettings.DARK);
                }
                // Else, leave it as read in.
            }
            Settings_pColorSchemeSelector.SelectedIndex = CurrentColorScheme;
            Settings_GetColors();
            Settings_SetSliders();
            //Settings_Color_Changed(null, null);
            SetStackLayoutColors(Settings_StackLayout);
            Settings_gridTopLeft.BackgroundColor = colorSettings.Background;
            Settings_gridTopRight.BackgroundColor = colorSettings.Background;
            Settings_gridLeftFiller.BackgroundColor = colorSettings.Background;
            Settings_gridRightFiller.BackgroundColor = colorSettings.Background;
            Settings_gridBottomLeft.BackgroundColor = colorSettings.Background;
            Settings_gridBottomRight.BackgroundColor = colorSettings.Background;
            //Settings_updateColors = false;
            Settings_StackLayout.IsVisible = true;
        }

        public void DrawSettingsPage()
        {
            Settings_gridMain = new Grid();
            Settings_gridTopLeft = new Grid();
            Settings_gridTopRight = new Grid();
            Settings_gridBottomLeft = new Grid();
            Settings_gridBottomRight = new Grid();
            Settings_gridLeftFiller = new Grid();
            Settings_gridRightFiller = new Grid();
            Settings_gridControls = new Grid();
            Settings_gridSamples = new Grid();

            Settings_gridMain.HorizontalOptions = LayoutOptions.FillAndExpand;
            Settings_gridTopLeft.HorizontalOptions = LayoutOptions.FillAndExpand;
            Settings_gridTopRight.HorizontalOptions = LayoutOptions.FillAndExpand;
            Settings_gridBottomLeft.HorizontalOptions = LayoutOptions.FillAndExpand;
            Settings_gridBottomRight.HorizontalOptions = LayoutOptions.FillAndExpand;
            Settings_gridLeftFiller.HorizontalOptions = LayoutOptions.FillAndExpand;
            Settings_gridRightFiller.HorizontalOptions = LayoutOptions.FillAndExpand;
            Settings_gridControls.HorizontalOptions = LayoutOptions.FillAndExpand;
            Settings_gridSamples.HorizontalOptions = LayoutOptions.FillAndExpand;

            Settings_gridMain.VerticalOptions = LayoutOptions.FillAndExpand;
            Settings_gridTopLeft.VerticalOptions = LayoutOptions.FillAndExpand;
            Settings_gridTopRight.VerticalOptions = LayoutOptions.FillAndExpand;
            Settings_gridBottomLeft.VerticalOptions = LayoutOptions.FillAndExpand;
            Settings_gridBottomRight.VerticalOptions = LayoutOptions.FillAndExpand;
            Settings_gridLeftFiller.VerticalOptions = LayoutOptions.FillAndExpand;
            Settings_gridRightFiller.VerticalOptions = LayoutOptions.FillAndExpand;
            Settings_gridControls.VerticalOptions = LayoutOptions.FillAndExpand;
            Settings_gridSamples.VerticalOptions = LayoutOptions.FillAndExpand;

            if (!String.IsNullOrEmpty(preferredConnection))
            {
                Settings_lsAutomaticSelectConnection = new LabeledSwitch("Automatically select " + preferredConnection, null, new byte[] { 3, 1 });
            }
            else
            {
                Settings_lsAutomaticSelectConnection = new LabeledSwitch("Automatically select connection", null, new byte[] { 3, 1 });
            }
            Settings_lsAutomaticSelectConnection.LSSwitch.IsToggled = automaticSelectConnection;
            Settings_lsAutomaticSelectConnection.LSSwitch.Toggled += AutomaticSelectConnection_Toggled;
            Settings_lsBankNumberToClipboard = new LabeledSwitch("Put combined bank number in clipboard", null, new byte[] { 3, 1 });
            Settings_lsBankNumberToClipboard.LSSwitch.IsToggled = putBankInClipboard;
            Settings_lsBankNumberToClipboard.LSSwitch.Toggled += BankNumberToClipboard_Toggled; ;
            Settings_lblColorTypeSelector = new Label();
            Settings_lblColorTypeSelector.Text = "Element to set: ";
            Settings_pColorTypeSelector = new Picker();
            Settings_pColorTypeSelector.Items.Add("Control border color");
            Settings_pColorTypeSelector.Items.Add("Frame border color");
            Settings_pColorTypeSelector.Items.Add("Background color");
            Settings_pColorTypeSelector.Items.Add("Text color");
            Settings_pColorTypeSelector.Items.Add("Is favorite color");
            Settings_pColorTypeSelector.Items.Add("White piano key color");
            Settings_pColorTypeSelector.Items.Add("Black piano key color");
            Settings_pColorTypeSelector.Items.Add("White piano key text color");
            Settings_pColorTypeSelector.Items.Add("Black piano key text color");
            Settings_pColorTypeSelector.Items.Add("Progress bar color");
            Settings_pColorTypeSelector.Items.Add("Motional surround part label text color");
            Settings_pColorTypeSelector.Items.Add("Motional surround part label focused color");
            Settings_pColorTypeSelector.Items.Add("Motional surround part label unfocused color");
            Settings_pColorTypeSelector.Items.Add("Lists text color");
            Settings_pColorTypeSelector.SelectedIndex = 0;
            Settings_pColorTypeSelector.SelectedIndexChanged += Settings_ColorTypeSelector_SelectedIndexChanged;
            Settings_lblColorSchemeSelector = new Label();
            Settings_lblColorSchemeSelector.Text = "Color scheme: ";
            Settings_pColorSchemeSelector = new Picker();
            Settings_pColorSchemeSelector.Items.Add("Light colors");
            Settings_pColorSchemeSelector.Items.Add("Dark colors");
            Settings_pColorSchemeSelector.Items.Add("Custom colors");
            Settings_pColorSchemeSelector.SelectedIndex = 0;
            Settings_pColorSchemeSelector.SelectedIndexChanged += Settings_pColorSchemeSelector_SelectedIndexChanged;

            Settings_sSwitch = new Switch();
            Settings_btnButton = new Button();
            Settings_lblLabel = new Label();
            Settings_btnFavorites = new FavoritesButton();
            Settings_pProgress = new ProgressBar();
            Settings_pProgress.Progress = 0.5F;
            Settings_mslMotionalSurroundPartLabel = new MotionalSurroundPartLabel(0);
            Settings_lblSlider = new Label();
            Settings_slSlider = new Slider();
            Settings_ocListView = new ObservableCollection<String>();
            Settings_lvListView = new ListView();
            Settings_pPicker = new Picker();
            Settings_btnWhitePianoKey = new PianoKey(true);
            Settings_btnBlackPianoKey = new PianoKey(false);
            Settings_btnSave = new Button();
            Settings_btnRandomColors = new Button();
            Settings_btnRestore = new Button();
            Settings_btnReturn = new Button();

            Settings_btnButton.Text = "Button";
            Settings_lblLabel.Text = "Label";
            Settings_lblSlider.Text = "Slider";
            Settings_ocListView.Add("Line 1");
            Settings_ocListView.Add("Line 2");
            Settings_ocListView.Add("Line 3");
            Settings_lvListView.ItemsSource = Settings_ocListView;
            Settings_ListsTextColor = new DataTemplate(typeof(TextCell));
            Settings_ListsTextColor.SetBinding(TextCell.TextProperty, ".");
            Settings_lvListView.ItemTemplate = Settings_ListsTextColor;
            Settings_pPicker.Items.Add("Line 1");
            Settings_pPicker.Items.Add("Line 2");
            Settings_pPicker.Items.Add("Line 3");
            Settings_pPicker.SelectedIndex = 0;
            Settings_btnWhitePianoKey.Text = "White piano key";
            Settings_btnBlackPianoKey.Text = "Black piano key";
            Settings_btnFavorites.Text = "Favorites";
            Settings_mslMotionalSurroundPartLabel.Text = "Motional surround part";
            Settings_btnSave.Text = "Save";
            Settings_btnRestore.Text = "Restore";
            Settings_btnRandomColors.Text = "Random colors";
            Settings_btnReturn.Text = "Return";

            Settings_lblRed = new Label();
            Settings_lblGreen = new Label();
            Settings_lblBlue = new Label();
            Settings_lblAlpha = new Label();

            Settings_slRed = new Slider();
            Settings_slGreen = new Slider();
            Settings_slBlue = new Slider();
            Settings_slAlpha = new Slider();

            Settings_slRed.Minimum = 0.0F;
            Settings_slGreen.Minimum = 0.0F;
            Settings_slBlue.Minimum = 0.0F;
            Settings_slAlpha.Minimum = 0.0F;

            Settings_slRed.Maximum = 1.0F;
            Settings_slGreen.Maximum = 1.0F;
            Settings_slBlue.Maximum = 1.0F;
            Settings_slAlpha.Maximum = 1.0F;

            Settings_slRed.StepFrequency = 0.01F;
            Settings_slGreen.StepFrequency = 0.01F;
            Settings_slBlue.StepFrequency = 0.01F;
            Settings_slAlpha.StepFrequency = 0.01F;

            Settings_mslMotionalSurroundPartLabel.Clicked += Settings_mslMotionalSurroundPartLabel_Clicked;

            Settings_slRed.ValueChanged += Settings_Color_Changed;
            Settings_slGreen.ValueChanged += Settings_Color_Changed;
            Settings_slBlue.ValueChanged += Settings_Color_Changed;
            Settings_slAlpha.ValueChanged += Settings_Color_Changed;

            Settings_btnSave.Clicked += Settings_btnSave_Clicked;
            Settings_btnRestore.Clicked += Settings_btnRestore_Clicked;
            Settings_btnRandomColors.Clicked += Settings_btnRandomColors_Clicked;
            Settings_btnReturn.Clicked += Settings_btnReturn_Clicked;

            Settings_lblRed.Text = "Red: ";
            Settings_lblGreen.Text = "Green: ";
            Settings_lblBlue.Text = "Blue: ";
            Settings_lblAlpha.Text = "Alpha: ";

            for (Int32 i = 0; i < 18; i++)
            {
                Settings_gridLeftFiller.RowDefinitions.Add(new RowDefinition());
                Settings_gridControls.RowDefinitions.Add(new RowDefinition());
                Settings_gridSamples.RowDefinitions.Add(new RowDefinition());
                Settings_gridRightFiller.RowDefinitions.Add(new RowDefinition());
            }

            //Settings_gridControls.RowSpacing = 2;

            Settings_gridControls.Children.Add(new GridRow(0, new View[] 
                { Settings_gridTopLeft }, null, false, false, 4));
            byte rowOffset = 4;
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[]
                { Settings_lsAutomaticSelectConnection }));
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[]
                { Settings_lsBankNumberToClipboard }));
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_lblColorTypeSelector, Settings_pColorSchemeSelector }));
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_lblColorSchemeSelector, Settings_pColorTypeSelector }));
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_lblRed, Settings_slRed }, new byte[] { 1, 10 }));
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_lblGreen, Settings_slGreen }, new byte[] { 1, 10 }));
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_lblBlue, Settings_slBlue }, new byte[] { 1, 10 }));
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_lblAlpha, Settings_slAlpha }, new byte[] { 1, 10 }));
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_btnSave, Settings_btnRestore, Settings_btnRandomColors,
                Settings_btnReturn }, new byte[] { 1, 1, 1, 1 }));

            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_gridBottomLeft }, null, false, false, 5));

            //Settings_gridSamples.RowSpacing = 2;

            Settings_gridSamples.Children.Add(new GridRow(0, new View[] 
                { Settings_gridTopRight }, null, false, false, 4));

            rowOffset = 4;
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_lblLabel, Settings_sSwitch, Settings_btnButton }));
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_lblSlider, Settings_slSlider }, new byte[] { 1, 2 }));
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_lvListView }, null, false, false, 3));
            rowOffset++;
            rowOffset++;
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_pPicker }));
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[] 
                { Settings_btnWhitePianoKey, Settings_btnBlackPianoKey }));
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[]
                { Settings_btnFavorites, Settings_pProgress }));
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[]
                { Settings_mslMotionalSurroundPartLabel }));

            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[] 
            { Settings_gridBottomRight }, null, false, false, 5));

            Settings_gridMain.Children.Add(new GridRow(0, new View[]
                { Settings_gridLeftFiller, Settings_gridControls,
                    Settings_gridSamples, Settings_gridRightFiller },
                new byte[] { 2, 4, 2, 2 }, false, false, 1));

            Settings_slRed.IsEnabled = false;
            Settings_slGreen.IsEnabled = false;
            Settings_slBlue.IsEnabled = false;
            Settings_slAlpha.IsEnabled = false;
            Settings_btnSave.IsEnabled = false;
            Settings_btnRandomColors.IsEnabled = false;
            Settings_btnRestore.IsEnabled = false;

            Settings_gridControls.RowSpacing = Margins;
            Settings_gridSamples.RowSpacing = Margins;
            Settings_gridMain.ColumnSpacing = Margins;

            Settings_gridControls.Margin = new Thickness(0, 0, 0, 0);
            Settings_gridSamples.Margin = new Thickness(0, 0, 0, 0);
            Settings_gridTopLeft.Margin = new Thickness(-10, -2, -10, 0);
            Settings_gridTopRight.Margin = new Thickness(-10, -2, -14, 0);
            Settings_gridBottomLeft.Margin = new Thickness(-10, 0, -10, -2);
            Settings_gridBottomRight.Margin = new Thickness(-10, 0, -14, -2);

            Settings_StackLayout = new StackLayout();
            Settings_StackLayout.Margin = new Thickness(2);
            Settings_StackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            Settings_StackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            Settings_StackLayout.Children.Add(Settings_gridMain);
        }

        private void BankNumberToClipboard_Toggled(object sender, ToggledEventArgs e)
        {
            putBankInClipboard = Settings_lsBankNumberToClipboard.LSSwitch.IsToggled;
            mainPage.SaveLocalValue("PutBankInClipboard", putBankInClipboard);
        }

        private void AutomaticSelectConnection_Toggled(object sender, ToggledEventArgs e)
        {
            automaticSelectConnection = Settings_lsAutomaticSelectConnection.LSSwitch.IsToggled;
            mainPage.SaveLocalValue("AutomaticSelectConnection", automaticSelectConnection ? "True" : "False");
        }

        private void Settings_mslMotionalSurroundPartLabel_Clicked(object sender, EventArgs e)
        {
            Settings_mslMotionalSurroundPartLabelHasFocus = !Settings_mslMotionalSurroundPartLabelHasFocus;
            if (Settings_mslMotionalSurroundPartLabelHasFocus)
            {
                Settings_mslMotionalSurroundPartLabel.BackgroundColor = Settings_UserColorSettings.MotionalSurroundPartLabelFocused;
                Settings_mslMotionalSurroundPartLabel.Text = "Motional surround part focused";
            }
            else
            {
                Settings_mslMotionalSurroundPartLabel.BackgroundColor = Settings_UserColorSettings.MotionalSurroundPartLabelUnfocused;
                Settings_mslMotionalSurroundPartLabel.Text = "Motional surround part unfocused";
            }
        }

        private void Settings_btnSave_Clicked(object sender, EventArgs e)
        {
            colorSettings = Settings_UserColorSettings;
            Settings_SaveUserColorSettings();
        }

        private async void Settings_btnRestore_Clicked(object sender, EventArgs e)
        {
            String result = await mainPage.DisplayActionSheet("Restore from:", "Cancel", null, new string[] { "Light theme", "Dark theme", "Custom colors" });
            if (result == "Light theme")
            {
                Settings_UserColorSettings = new ColorSettings(_colorSettings.LIGHT);
            }
            else if (result == "Dark theme")
            {
                Settings_UserColorSettings = new ColorSettings(_colorSettings.DARK);
            }
            else if (result == "Custom colors")
            {
                Settings_ReadUserColorSettings();
                SetStackLayoutColors(Settings_StackLayout, Settings_UserColorSettings);
            }
            Settings_SetSliders();
            SetStackLayoutColors(Settings_StackLayout, Settings_UserColorSettings);
        }

        private void Settings_btnRandomColors_Clicked(object sender, EventArgs e)
        {
            Settings_RandomUserColors();
            SetStackLayoutColors(Settings_StackLayout, Settings_UserColorSettings);
        }

        private void Settings_btnReturn_Clicked(object sender, EventArgs e)
        {
            if (Settings_StackLayout.IsVisible)
            {
                if (Librarian_StackLayout != null)
                {
                    SetStackLayoutColors(Librarian_StackLayout);
                }
                if (StudioSetEditor_StackLayout != null)
                {
                    SetStackLayoutColors(StudioSetEditor_StackLayout);
                }
                if (Edit_StackLayout != null)
                {
                    //SetStackLayoutColors(Edit_StackLayout);
                }
                if (MotionalSurround_StackLayout != null)
                {
                    SetStackLayoutColors(MotionalSurround_StackLayout);
                }
            }
            Settings_StackLayout.IsVisible = false;
            ShowLibrarianPage();
        }

        public void Settings_Timer_Tick()
        {
        }

        private void Settings_pColorSchemeSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings_slRed.IsEnabled = false;
            Settings_slGreen.IsEnabled = false;
            Settings_slBlue.IsEnabled = false;
            Settings_slAlpha.IsEnabled = false;
            Settings_btnSave.IsEnabled = false;
            Settings_btnRandomColors.IsEnabled = false;
            Settings_btnRestore.IsEnabled = false;

            switch (Settings_pColorSchemeSelector.SelectedIndex)
            {
                case 0:
                    colorSettings = new ColorSettings(_colorSettings.LIGHT);
                    break;
                case 1:
                    colorSettings = new ColorSettings(_colorSettings.DARK);
                    break;
                case 2:
                    colorSettings = new ColorSettings(Settings_UserColorSettings);
                    Settings_slRed.IsEnabled = true;
                    Settings_slGreen.IsEnabled = true;
                    Settings_slBlue.IsEnabled = true;
                    Settings_slAlpha.IsEnabled = true;
                    Settings_btnSave.IsEnabled = true;
                    Settings_btnRandomColors.IsEnabled = true;
                    Settings_btnRestore.IsEnabled = true;
                    break;
            }
            //Settings_GetColors();
            SetStackLayoutColors(Settings_StackLayout);
            Settings_gridTopLeft.BackgroundColor = colorSettings.Background;
            Settings_gridTopRight.BackgroundColor = colorSettings.Background;
            Settings_gridLeftFiller.BackgroundColor = colorSettings.Background;
            Settings_gridRightFiller.BackgroundColor = colorSettings.Background;
            Settings_gridBottomLeft.BackgroundColor = colorSettings.Background;
            Settings_gridBottomRight.BackgroundColor = colorSettings.Background;
            Settings_SaveUserColorSettings();
        }

        private void Settings_ReadUserColorSettings()
        {
            if (mainPage.LoadLocalValue("UserColorSettings") != null)
            {
                try
                {
                    String temp = (String)mainPage.LoadLocalValue("UserColorSettings");
                    Settings_UserColorSettings = Settings_DeserializeColor(temp);
                }
                catch
                {
                    Settings_UserColorSettings = new ColorSettings(_colorSettings.LIGHT);
                    Settings_SaveUserColorSettings();
                }
            }
            else
            {
                Settings_UserColorSettings = new ColorSettings(_colorSettings.LIGHT);
                Settings_SaveUserColorSettings();
            }

            if (mainPage.LoadLocalValue("ColorScheme") != null)
            {
                try
                {
                    CurrentColorScheme = (Int32)mainPage.LoadLocalValue("ColorScheme");
                }
                catch
                {
                    CurrentColorScheme = 0;
                    mainPage.SaveLocalValue("ColorScheme", CurrentColorScheme);
                }
            }
            else
            {
                CurrentColorScheme = 0;
                mainPage.SaveLocalValue("ColorScheme", CurrentColorScheme);
            }
        }

        private void Settings_SaveUserColorSettings()
        {
            String serializationText = "";
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.ControlBorder);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.FrameBorder);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.Background);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.Text);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.IsFavorite);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.Transparent);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.WhitePianoKey);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.BlackPianoKey);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.WhitePianoKeyText);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.BlackPianoKeyText);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.Progressbar);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.MotionalSurroundPartLabelText);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.MotionalSurroundPartLabelFocused);
            serializationText += Settings_SerializeColor(Settings_UserColorSettings.MotionalSurroundPartLabelUnfocused).TrimEnd(';');
            mainPage.SaveLocalValue("UserColorSettings", serializationText);
            if (Settings_pColorSchemeSelector != null)
            {
                mainPage.SaveLocalValue("ColorScheme", Settings_pColorSchemeSelector.SelectedIndex);
            }
        }

        private String Settings_SerializeColor(Color color)
        {
            return (color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString() + "," + color.A.ToString() + ";");
        }

        private ColorSettings Settings_DeserializeColor(String setting)
        {
            String[] colors = setting.Split(';');
            ColorSettings userColorSetting = new ColorSettings(_colorSettings.LIGHT);
            Int32 i = 0;
            String[] color = colors[i++].Split(',');
            userColorSetting.ControlBorder = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.FrameBorder = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.Background = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.Text = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.IsFavorite = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.Transparent = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.WhitePianoKey = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.BlackPianoKey = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.WhitePianoKeyText = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.BlackPianoKeyText = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.Progressbar = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.MotionalSurroundPartLabelText = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.MotionalSurroundPartLabelFocused = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            color = colors[i++].Split(',');
            userColorSetting.MotionalSurroundPartLabelUnfocused = new Color(
                Double.Parse(color[0]), Double.Parse(color[1]), Double.Parse(color[2]), Double.Parse(color[3]));
            userColorSetting.ListViewTextColor.SetValue(
                TextCell.TextColorProperty, userColorSetting.Text);
            return userColorSetting;
        }

        private void Settings_ColorTypeSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings_SetSliders();
        }

        private void Settings_Color_Changed(object sender, ValueChangedEventArgs e)
        {
            if (handleControlEvents)
            {
                PushHandleControlEvents();
                Color color = Color.Red;
                switch (Settings_pColorTypeSelector.SelectedIndex)
                {
                    case 0:
                        Settings_UserColorSettings.ControlBorder = Settings_GetColorFromSliders();
                        break;
                    case 1:
                        Settings_UserColorSettings.FrameBorder = Settings_GetColorFromSliders();
                        break;
                    case 2:
                        Settings_UserColorSettings.Background = Settings_GetColorFromSliders();
                        break;
                    case 3:
                        Settings_UserColorSettings.Text = Settings_GetColorFromSliders();
                        break;
                    case 4:
                        Settings_UserColorSettings.IsFavorite = Settings_GetColorFromSliders();
                        break;
                    case 5:
                        Settings_UserColorSettings.WhitePianoKey = Settings_GetColorFromSliders();
                        break;
                    case 6:
                        Settings_UserColorSettings.BlackPianoKey = Settings_GetColorFromSliders();
                        break;
                    case 7:
                        Settings_UserColorSettings.WhitePianoKeyText = Settings_GetColorFromSliders();
                        break;
                    case 8:
                        Settings_UserColorSettings.BlackPianoKeyText = Settings_GetColorFromSliders();
                        break;
                    case 9:
                        Settings_UserColorSettings.Progressbar = Settings_GetColorFromSliders();
                        break;
                    case 10:
                        Settings_UserColorSettings.MotionalSurroundPartLabelText = Settings_GetColorFromSliders();
                        break;
                    case 11:
                        Settings_UserColorSettings.MotionalSurroundPartLabelFocused = Settings_GetColorFromSliders();
                        break;
                    case 12:
                        Settings_UserColorSettings.MotionalSurroundPartLabelUnfocused = Settings_GetColorFromSliders();
                        break;
                    case 13:
                        //Settings_UserColorSettings.ListViewTextColor = Settings_GetColorFromSliders();
                        break;
                }
                SetStackLayoutColors(Settings_StackLayout, Settings_UserColorSettings);
                PopHandleControlEvents();
            }
        }

        private void Settings_GetColors()
        {
            Settings_UserColorSettings = new ColorSettings(_colorSettings.LIGHT);
        }

        private void Settings_SetSliders()
        {
            PushHandleControlEvents();
            switch (Settings_pColorTypeSelector.SelectedIndex)
            {
                case 0:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.ControlBorder);
                    break;
                case 1:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.FrameBorder);
                    break;
                case 2:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.Background);
                    break;
                case 3:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.Text);
                    break;
                case 4:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.IsFavorite);
                    break;
                case 5:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.WhitePianoKey);
                    break;
                case 6:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.BlackPianoKey);
                    break;
                case 7:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.WhitePianoKeyText);
                    break;
                case 8:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.BlackPianoKeyText);
                    break;
                case 9:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.Progressbar);
                    break;
                case 10:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.MotionalSurroundPartLabelText);
                    break;
                case 11:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.MotionalSurroundPartLabelFocused);
                    break;
                case 12:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.MotionalSurroundPartLabelUnfocused);
                    break;
                case 13:
                    Settings_SetSlidersFromColor(Settings_UserColorSettings.Text);
                    break;
            }
            PopHandleControlEvents();
        }

        private void Settings_SetSlidersFromColor(Color color)
        {
            Settings_slRed.Value = color.R;
            Settings_slGreen.Value = color.G;
            Settings_slBlue.Value = color.B;
            Settings_slAlpha.Value = color.A;
        }

        private Color Settings_GetColorFromSliders()
        {
            return new Color(
                Settings_slRed.Value,
                Settings_slGreen.Value,
                Settings_slBlue.Value,
                Settings_slAlpha.Value);
        }

        private void SetStackLayoutColors(StackLayout stackLayout, ColorSettings settings = null)
        {
            if (settings == null)
            {
                settings = colorSettings;
            }
            settings.ListViewTextColor.SetValue(TextCell.TextColorProperty, colorSettings.Text);
            stackLayout.BackgroundColor = colorSettings.FrameBorder;
            foreach (Object child in stackLayout.Children)
            {
                SetControlColors(child, settings);
            }
        }

        private void SetControlColors(Object control, ColorSettings settings)
        {
            Type type = control.GetType();

            if (type == typeof(Button))
            {
                if (((String)((Button)control).Tag) == "PianoKeyCover")
                {
                    ((Button)control).BackgroundColor = settings.Progressbar;
                }
                else
                {
                    ((Button)control).TextColor = settings.Text;
                    ((Button)control).BackgroundColor = settings.Background;
                }
            }
            else if (type == typeof(Xamarin.Forms.Button))
            {
                ((Xamarin.Forms.Button)control).TextColor = settings.Text;
                ((Xamarin.Forms.Button)control).BackgroundColor = settings.Background;
            }
            else if (type == typeof(PianoKey))
            {
                if ((Boolean)((PianoKey)control).WhiteKey)
                {
                    ((PianoKey)control).TextColor = settings.WhitePianoKeyText;
                    ((PianoKey)control).BackgroundColor = settings.WhitePianoKey;
                }
                else
                {
                    ((PianoKey)control).TextColor = settings.BlackPianoKeyText;
                    ((PianoKey)control).BackgroundColor = settings.BlackPianoKey;
                }
            }
            else if (type == typeof(FavoritesButton))
            {
                ((FavoritesButton)control).TextColor = settings.Text;
                ((FavoritesButton)control).BackgroundColor = settings.IsFavorite;
            }
            else if (type == typeof(Label))
            {
                ((Label)control).TextColor = settings.Text;
                ((Label)control).BackgroundColor = settings.Background;
            }
            else if (type == typeof(LabeledText))
            {
                ((LabeledText)control).Label.TextColor = settings.Text;
                ((LabeledText)control).Label.BackgroundColor = settings.Background;
                ((LabeledText)control).text.TextColor = settings.Text;
                ((LabeledText)control).text.BackgroundColor = settings.Background;
                ((LabeledText)control).BackgroundColor = settings.Background;
                //if (((LabeledText)control).Children.Count > 0)
                //{
                //    for (Int32 i = 0; i < ((LabeledText)control).Children.Count; i++)
                //    {
                //        foreach (Object child in ((LabeledText)control).Children)
                //        {
                //            SetControlColors(child, settings);
                //        }
                //    }
                //}
            }
            else if (type == typeof(Switch))
            {
                ((Switch)control).OnColor = settings.Text;
                ((Switch)control).BackgroundColor = settings.Background;
            }
            else if (type == typeof(CheckBox)) // This is actually INTEGRA_7_Xamarin.CheckBox
            {
                ((CheckBox)control).BackgroundColor = settings.Background;
                ((CheckBox)control).CBLabel.BackgroundColor = settings.Background;
                ((CheckBox)control).CBLabel.TextColor = settings.Text;
                ((CheckBox)control).CBSwitch.BackgroundColor = settings.Background;
            }
            else if (type == typeof(LabeledSwitch))
            {
                ((LabeledSwitch)control).LSLabel.BackgroundColor = settings.Background;
                ((LabeledSwitch)control).LSSwitch.BackgroundColor = settings.Background;
                ((LabeledSwitch)control).LSLabel.TextColor = settings.Text;
            }
            else if (type == typeof(ListView))
            {
                String temp = (String)((ListView)control).SelectedItem;
                ((ListView)control).ItemTemplate = settings.ListViewTextColor;
                ((ListView)control).BackgroundColor = settings.Background;
                ((ListView)control).SelectedItem = temp;
            }
            else if (type == typeof(Slider))
            {
                ((Slider)control).BackgroundColor = settings.Background;
            }
            else if (type == typeof(Picker))
            {
                ((Picker)control).TextColor = settings.Text;
                ((Picker)control).BackgroundColor = settings.Background;
            }
            else if (type == typeof(ComboBox))
            {
                ((ComboBox)control).TextColor = settings.Text;
                ((ComboBox)control).BackgroundColor = settings.Background;
            }
            else if (type == typeof(Editor))
            {
                ((Editor)control).TextColor = settings.Text;
                ((Editor)control).BackgroundColor = settings.Background;
            }
            else if (type == typeof(TextBox))
            {
                ((TextBox)control).TextColor = settings.Text;
                ((TextBox)control).BackgroundColor = settings.Background;
                //((TextBox)control).Editor.TextColor = settings.Text;
                //((TextBox)control).Editor.BackgroundColor = settings.Background;
            }
            else if (type == typeof(TextBlock))
            {
                ((TextBlock)control).TextColor = settings.Text;
                ((TextBlock)control).BackgroundColor = settings.Background;
            }
            else if (type == typeof(MotionalSurroundPartLabel))
            {
                ((MotionalSurroundPartLabel)control).TextColor = settings.MotionalSurroundPartLabelText;
                if (((MotionalSurroundPartLabel)control).IsFocused == true)
                {
                    ((MotionalSurroundPartLabel)control).BackgroundColor = settings.MotionalSurroundPartLabelFocused;
                }
                else
                {
                    ((MotionalSurroundPartLabel)control).BackgroundColor = settings.MotionalSurroundPartLabelUnfocused;
                }
            }
            else if (type == typeof(MotionalSurroundPartEditor))
            {
                ((MotionalSurroundPartEditor)control).Editor.BackgroundColor = settings.Background;
                ((MotionalSurroundPartEditor)control).Editor.TextColor = settings.Text;
                ((MotionalSurroundPartEditor)control).Switch.LSLabel.BackgroundColor = settings.Background;
                ((MotionalSurroundPartEditor)control).Switch.LSLabel.TextColor = settings.Text;
                ((MotionalSurroundPartEditor)control).Switch.LSSwitch.BackgroundColor = settings.Background;
                //((MotionalSurroundPartEditor)control).Switch.LSSwitch.TextColor = settings.Text;
                ((MotionalSurroundPartEditor)control).BackgroundColor = settings.Background;
            }
            else if (type == typeof(Image))
            {
                ((Image)control).BackgroundColor = settings.Background;
            }
            else if (type == typeof(TouchableImage))
            {
                ((TouchableImage)control).BackgroundColor = settings.Background;
            }
            else if (type == typeof(ProgressBar))
            {
                ((ProgressBar)control).BackgroundColor = settings.Background;
                ((ProgressBar)control).ProgressColor = settings.Progressbar;
            }
            else if (type == typeof(Grid))
            {
                if (((Grid)control).IsPianoGrid == 1)
                {
                    ((View)control).BackgroundColor = settings.BlackPianoKey;
                }
                else if (((Grid)control).IsPianoGrid == 2)
                {
                    ((View)control).BackgroundColor = settings.WhitePianoKey;
                }
                else
                {
                    ((View)control).BackgroundColor = settings.Transparent;
                }
                if (((Grid)control).Children.Count > 0)
                {
                    for (Int32 i = 0; i < ((Grid)control).Children.Count; i++)
                    {
                        if (((Grid)control).Children[i].GetType() == typeof(Slider))
                        {
                            ((View)control).BackgroundColor = settings.Background;
                        }
                    }
                }
                foreach (Object child in ((Grid)control).Children)
                {
                    SetControlColors(child, settings);
                }
            }
            else if (type == typeof(GridRow))
            {
                ((Grid)((GridRow)control)).BackgroundColor = settings.FrameBorder;
                if (((Grid)((GridRow)control)).Children.Count > 0)
                {
                    for (Int32 i = 0; i < ((Grid)((GridRow)control)).Children.Count; i++)
                    {
                        if (((Grid)((GridRow)control)).Children[i].GetType() == typeof(Slider))
                        {
                            ((Grid)((GridRow)control)).BackgroundColor = settings.Background;
                        }
                    }
                }
                foreach (Object child in ((Grid)((GridRow)control)).Children)
                {
                    SetControlColors(child, settings);
                }
            }
            else
            {
                throw new Exception("Missing type in SetControlColors(): " + type.ToString());
            }
        }

        private void Settings_RandomUserColors()
        {
            Random random = new Random();
            Settings_UserColorSettings.Background = new Color(
                (Double)random.Next(101) / (Double)100,
                (Double)random.Next(101) / (Double)100,
                (Double)random.Next(101) / (Double)100, 1);

            Settings_UserColorSettings.FrameBorder = new Color(
                1 - Settings_UserColorSettings.Background.R,
                1 - Settings_UserColorSettings.Background.G,
                1 - Settings_UserColorSettings.Background.B, 1);

            Settings_UserColorSettings.Text = new Color(
                1 - Settings_UserColorSettings.Background.R,
                1 - Settings_UserColorSettings.Background.G,
                1 - Settings_UserColorSettings.Background.B, 1);

            Settings_UserColorSettings.ControlBorder = new Color(
                (Double)random.Next(101) / (Double)100,
                (Double)random.Next(101) / (Double)100,
                (Double)random.Next(101) / (Double)100, 1);

            Settings_UserColorSettings.IsFavorite = new Color(
                (Double)random.Next(101) / (Double)100,
                (Double)random.Next(101) / (Double)100,
                (Double)random.Next(101) / (Double)100, 1);

            Settings_UserColorSettings.WhitePianoKey = new Color(
                (Double)random.Next(101) / (Double)100,
                (Double)random.Next(101) / (Double)100,
                (Double)random.Next(101) / (Double)100, 1);

            Settings_UserColorSettings.BlackPianoKey = new Color(
                1 - Settings_UserColorSettings.WhitePianoKey.R,
                1 - Settings_UserColorSettings.WhitePianoKey.G,
                1 - Settings_UserColorSettings.WhitePianoKey.B, 1);

            Settings_UserColorSettings.WhitePianoKeyText = new Color(
                1 - Settings_UserColorSettings.WhitePianoKey.R,
                1 - Settings_UserColorSettings.WhitePianoKey.G,
                1 - Settings_UserColorSettings.WhitePianoKey.B, 1);

            Settings_UserColorSettings.BlackPianoKeyText = new Color(
                Settings_UserColorSettings.WhitePianoKey.R,
                Settings_UserColorSettings.WhitePianoKey.G,
                Settings_UserColorSettings.WhitePianoKey.B, 1);

            Settings_UserColorSettings.Progressbar = new Color(
                (Double)random.Next(101) / (Double)100,
                (Double)random.Next(101) / (Double)100,
                (Double)random.Next(101) / (Double)100, 1);
        }
    }
}
