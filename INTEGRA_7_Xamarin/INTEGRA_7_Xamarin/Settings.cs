using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public partial class UIHandler
    {
        private Grid Settings_gridMain;
        private Grid Settings_gridTop;
        private Grid Settings_gridLeftFiller;
        private Grid Settings_gridRightFiller;
        private Grid Settings_gridControls;
        private Grid Settings_gridSamples;

        private LabeledSwitch Settings_lsBankNumberToClipboard;
        private Label Settings_lblColorSelector;
        private Picker Settings_pColorSelector;
        private Label Settings_lblColorTypeSelector;
        private Picker Settings_pColorTypeSelector;
        private Slider Settings_slRed;
        private Slider Settings_slGreen;
        private Slider Settings_slBlue;
        private Slider Settings_slAlpha;

        private Button Settings_btnSave;
        private Button Settings_btnRestore;
        private Button Settings_btnReturn;

        private Label Settings_lblRed;
        private Label Settings_lblGreen;
        private Label Settings_lblBlue;
        private Label Settings_lblAlpha;

        private Switch Settings_sSwitch;
        private Button Settings_btnButton;
        private Label Settings_lblLabel;
        private Button Settings_btnFavorites;
        private Label Settings_lblSlider;
        private Slider Settings_slSlider;
        private ListView Settings_lvListView;
        private DataTemplate Settings_ListsTextColor { get; set; }
        private ObservableCollection<String> Settings_ocListView;
        private Picker Settings_pPicker;
        private Button Settings_btnWhitePianoKey;
        private Button Settings_btnBlackPianoKey;

        private ColorElements ColorElements;
        private Boolean Settings_updateColors;

        private Color[] Settings_Color;

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
                ColorElements = new ColorElements();
                Settings_Color = new Color[14];
            }
            Settings_GetColors();
            Settings_SetSliders();
            Settings_Color_Changed(null, null);
            Settings_updateColors = false;
        }

        public void DrawSettingsPage()
        {
            Settings_gridMain = new Grid();
            Settings_gridTop = new Grid();
            Settings_gridLeftFiller = new Grid();
            Settings_gridRightFiller = new Grid();
            Settings_gridControls = new Grid();
            Settings_gridSamples = new Grid();

            Settings_lsBankNumberToClipboard = new LabeledSwitch("Put combined bank number in clipboard");
            Settings_lblColorSelector = new Label();
            Settings_lblColorSelector.Text = "Color scheme: ";
            Settings_pColorSelector = new Picker();
            Settings_pColorSelector.Items.Add("Light colors");
            Settings_pColorSelector.Items.Add("Dark colors");
            Settings_pColorSelector.Items.Add("Custom colors");
            Settings_pColorSelector.SelectedIndex = 0;
            Settings_pColorSelector.SelectedIndexChanged += Settings_pColorSelector_SelectedIndexChanged;
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
            Settings_pColorTypeSelector.Items.Add("Piano key cover color");
            Settings_pColorTypeSelector.Items.Add("Motional surround part label text color");
            Settings_pColorTypeSelector.Items.Add("Motional surround part label focused color");
            Settings_pColorTypeSelector.Items.Add("Motional surround part label unfocused color");
            Settings_pColorTypeSelector.Items.Add("Lists text color");
            Settings_pColorTypeSelector.SelectedIndex = 0;
            Settings_pColorTypeSelector.SelectedIndexChanged += Settings_ColorTypeSelector_SelectedIndexChanged;

            Settings_sSwitch = new Switch();
            Settings_btnButton = new Button();
            Settings_lblLabel = new Label();
            Settings_btnFavorites = new Button();
            Settings_lblSlider = new Label();
            Settings_slSlider = new Slider();
            Settings_ocListView = new ObservableCollection<String>();
            Settings_lvListView = new ListView();
            Settings_pPicker = new Picker();
            Settings_btnWhitePianoKey = new Button();
            Settings_btnBlackPianoKey = new Button();
            Settings_btnSave = new Button();
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
            Settings_btnSave.Text = "Save";
            Settings_btnRestore.Text = "Restore";
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

            Settings_slRed.ValueChanged += Settings_Color_Changed;
            Settings_slGreen.ValueChanged += Settings_Color_Changed;
            Settings_slBlue.ValueChanged += Settings_Color_Changed;
            Settings_slAlpha.ValueChanged += Settings_Color_Changed;

            Settings_btnSave.Clicked += Settings_btnSave_Clicked;
            Settings_btnRestore.Clicked += Settings_btnRestore_Clicked;
            Settings_btnReturn.Clicked += Settings_btnReturn_Clicked;

            Settings_lblRed.Text = "Red: ";
            Settings_lblGreen.Text = "Green: ";
            Settings_lblBlue.Text = "Blue: ";
            Settings_lblAlpha.Text = "Alpha: ";

            for (Int32 i = 0; i < 18; i++)
            {
                Settings_gridControls.RowDefinitions.Add(new RowDefinition());
                Settings_gridSamples.RowDefinitions.Add(new RowDefinition());
            }

            Settings_gridControls.Children.Add(new GridRow(0, new View[] { Settings_gridTop }, null, false, false, 6).Row);
            byte rowOffset = 5;
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] { Settings_lsBankNumberToClipboard }).Row);
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] { Settings_lblColorSelector, Settings_pColorSelector }).Row);
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] { Settings_lblColorTypeSelector, Settings_pColorTypeSelector }).Row);
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] { Settings_lblRed, Settings_slRed }, new byte[] { 1, 10 }).Row);
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] { Settings_lblGreen, Settings_slGreen }, new byte[] { 1, 10 }).Row);
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] { Settings_lblBlue, Settings_slBlue }, new byte[] { 1, 10 }).Row);
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] { Settings_lblAlpha, Settings_slAlpha }, new byte[] { 1, 10 }).Row);
            Settings_gridControls.Children.Add(new GridRow(rowOffset++, new View[] { Settings_btnSave, Settings_btnRestore, Settings_btnReturn }, new byte[] { 1, 1, 1 }).Row);

            rowOffset = 5;
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[] { Settings_lblLabel, Settings_sSwitch, Settings_btnButton }).Row);
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[] { Settings_lblSlider, Settings_slSlider }, new byte[] { 1, 2 }).Row);
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[] { Settings_lvListView }, null, false, false, 3).Row);
            rowOffset++;
            rowOffset++;
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[] { Settings_pPicker }).Row);
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[] { Settings_btnWhitePianoKey, Settings_btnBlackPianoKey }).Row);
            Settings_gridSamples.Children.Add(new GridRow(rowOffset++, new View[] { Settings_btnFavorites }).Row);

            Settings_gridMain.Children.Add(new GridRow(0, new View[]
                { Settings_gridLeftFiller, Settings_gridControls, Settings_gridSamples, Settings_gridRightFiller },
                new byte[] { 2, 4, 2, 2 }, false, false, 1).Row);
            Settings_gridSamples.Margin = new Thickness(2, 0, 2, 0);
            Settings_gridSamples.Padding = new Thickness(2, 0, 2, 0);

            Settings_StackLayout = new StackLayout();

            Settings_StackLayout.Children.Add(Settings_gridMain);
        }

        private void Settings_btnSave_Clicked(object sender, EventArgs e)
        {
            colorSettings.ControlBorder = new Color(
                Settings_Color[0].R,
                Settings_Color[0].G,
                Settings_Color[0].B,
                Settings_Color[0].A);
            colorSettings.FrameBorder = new Color(
                Settings_Color[1].R,
                Settings_Color[1].G,
                Settings_Color[1].B,
                Settings_Color[1].A);
            colorSettings.Background = new Color(
                Settings_Color[2].R,
                Settings_Color[2].G,
                Settings_Color[2].B,
                Settings_Color[2].A);
            colorSettings.Text = new Color(
                Settings_Color[3].R,
                Settings_Color[3].G,
                Settings_Color[3].B,
                Settings_Color[3].A);
            colorSettings.IsFavorite = new Color(
                Settings_Color[4].R,
                Settings_Color[4].G,
                Settings_Color[4].B,
                Settings_Color[4].A);
            colorSettings.WhitePianoKey = new Color(
                Settings_Color[5].R,
                Settings_Color[5].G,
                Settings_Color[5].B,
                Settings_Color[5].A);
            colorSettings.BlackPianoKey = new Color(
                Settings_Color[6].R,
                Settings_Color[6].G,
                Settings_Color[6].B,
                Settings_Color[6].A);
            colorSettings.WhitePianoKeyText = new Color(
                Settings_Color[7].R,
                Settings_Color[7].G,
                Settings_Color[7].B,
                Settings_Color[7].A);
            colorSettings.BlackPianoKeyText = new Color(
                Settings_Color[8].R,
                Settings_Color[8].G,
                Settings_Color[8].B,
                Settings_Color[8].A);
            colorSettings.PianoKeyCover = new Color(
                Settings_Color[9].R,
                Settings_Color[9].G,
                Settings_Color[9].B,
                Settings_Color[9].A);
            colorSettings.MotionalSurroundPartLabelText = new Color(
                Settings_Color[10].R,
                Settings_Color[10].G,
                Settings_Color[10].B,
                Settings_Color[10].A);
            colorSettings.MotionalSurroundPartLabelFocused = new Color(
                Settings_Color[11].R,
                Settings_Color[11].G,
                Settings_Color[11].B,
                Settings_Color[11].A);
            colorSettings.MotionalSurroundPartLabelUnfocused = new Color(
                Settings_Color[12].R,
                Settings_Color[12].G,
                Settings_Color[12].B,
                Settings_Color[12].A);
            Color temp = (Color)colorSettings.ListViewTextColor.Values[TextCell.TextColorProperty];
            Settings_Color[13] = new Color(temp.R, temp.G, temp.B, temp.A);
        }

        private void Settings_btnRestore_Clicked(object sender, EventArgs e)
        {
            Settings_GetColors();
            Settings_updateColors = false;
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
                    SetStackLayoutColors(Edit_StackLayout);
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

        private void Settings_pColorSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings_updateColors = true;
            switch (Settings_pColorSelector.SelectedIndex)
            {
                case 0:
                    colorSettings.ControlBorder = Color.Black;
                    colorSettings.FrameBorder = Color.Black;
                    colorSettings.Background = Color.White;
                    colorSettings.Text = Color.Black;
                    colorSettings.IsFavorite = Color.LightGreen;
                    colorSettings.Transparent = new Color(0, 0, 0, 0);
                    colorSettings.WhitePianoKey = Color.FloralWhite;
                    colorSettings.BlackPianoKey = Color.Black;
                    colorSettings.WhitePianoKeyText = Color.Black;
                    colorSettings.BlackPianoKeyText = Color.FloralWhite;
                    colorSettings.PianoKeyCover = Color.DarkGreen;
                    colorSettings.MotionalSurroundPartLabelText = new Color(1, 1, 0.5, 1);
                    colorSettings.MotionalSurroundPartLabelFocused = new Color(0, 0.5, 0, 0.25);
                    colorSettings.MotionalSurroundPartLabelUnfocused = new Color(0.5, 0.5, 0, 0.25);
                    break;
                case 1:
                    colorSettings.ControlBorder = new Color(0.35, 0.1, 0, 1);
                    colorSettings.FrameBorder = new Color(0.35, 0.1, 0, 1);
                    colorSettings.Background = new Color(1, 1, 0.95, 1);
                    colorSettings.Text = new Color(0.35, 0.1, 0, 1);
                    colorSettings.ListViewTextColor.SetValue(TextCell.TextColorProperty, new Color(0.5, 0.1, 0, 1));
                    colorSettings.IsFavorite = Color.LightGreen;
                    colorSettings.Transparent = new Color(0, 0, 0, 0);
                    colorSettings.WhitePianoKey = Color.FloralWhite;
                    colorSettings.BlackPianoKey = new Color(0.35, 0.1, 0, 1);
                    colorSettings.WhitePianoKeyText = new Color(0.35, 0.1, 0, 1);
                    colorSettings.BlackPianoKeyText = Color.FloralWhite;
                    colorSettings.PianoKeyCover = Color.DarkOrange;
                    colorSettings.MotionalSurroundPartLabelText = new Color(1, 1, 0.5, 1);
                    colorSettings.MotionalSurroundPartLabelFocused = new Color(0, 0.5, 0, 0.25);
                    colorSettings.MotionalSurroundPartLabelUnfocused = new Color(0.5, 0.5, 0, 0.25);
                    break;
                case 2:
                    break;
            }
        }

        private void Settings_ColorTypeSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings_SetSliders();
        }

        private void Settings_Color_Changed(object sender, ValueChangedEventArgs e)
        {
            if (handleControlEvents)
            {
                Settings_updateColors = true; ;

                Settings_Color[Settings_pColorTypeSelector.SelectedIndex] = new Color(
                    Settings_slRed.Value,
                    Settings_slGreen.Value,
                    Settings_slBlue.Value,
                    Settings_slAlpha.Value);

                Settings_gridTop.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];

                //Settings_StackLayout.BackgroundColor = Settings_Color[ColorElements.FRAME_BORDER];

                Settings_sSwitch.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_sSwitch.OnColor = Settings_Color[ColorElements.TEXT];

                Settings_btnButton.BorderColor = Settings_Color[ColorElements.CONTROL_BORDER];
                Settings_btnButton.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_btnButton.TextColor = Settings_Color[ColorElements.TEXT];

                Settings_btnFavorites.BackgroundColor = Settings_Color[ColorElements.IS_FAVORITE];
                Settings_btnFavorites.TextColor = Settings_Color[ColorElements.TEXT];

                Settings_slSlider.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                //Settings_slSlider.Opacity = Settings_Color[ColorElements.TRANSPARENT].A;

                Settings_pPicker.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_pPicker.TextColor = Settings_Color[ColorElements.TEXT];

                Settings_lvListView.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_ListsTextColor.SetValue(TextCell.TextColorProperty, Settings_Color[ColorElements.TEXT]);

                Settings_btnWhitePianoKey.BackgroundColor = Settings_Color[ColorElements.WHITE_PIANO_KEY];
                Settings_btnWhitePianoKey.TextColor = Settings_Color[ColorElements.WHITE_PIANO_KEY_TEXT];

                Settings_btnBlackPianoKey.BackgroundColor = Settings_Color[ColorElements.BLACK_PIANO_KEY];
                Settings_btnBlackPianoKey.TextColor = Settings_Color[ColorElements.BLACK_PIANO_KEY_TEXT];

                Settings_lblLabel.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_lblLabel.TextColor = Settings_Color[ColorElements.TEXT];

                Settings_gridSamples.BackgroundColor = Settings_Color[ColorElements.FRAME_BORDER];
                Settings_gridMain.BackgroundColor = Settings_Color[ColorElements.FRAME_BORDER];
                Settings_gridLeftFiller.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_gridRightFiller.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_gridControls.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_gridSamples.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];

                Settings_lsBankNumberToClipboard.LSLabel.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_lsBankNumberToClipboard.LSLabel.TextColor = Settings_Color[ColorElements.TEXT];
                Settings_lsBankNumberToClipboard.LSSwitch.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];

                Settings_lblColorSelector.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_lblColorSelector.TextColor = Settings_Color[ColorElements.TEXT];

                Settings_pColorSelector.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_lblColorTypeSelector.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_lblColorTypeSelector.TextColor = Settings_Color[ColorElements.TEXT];
                Settings_pColorTypeSelector.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];

                Settings_slRed.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_slGreen.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_slBlue.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_slAlpha.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];

                Settings_lblRed.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_lblGreen.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_lblBlue.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];
                Settings_lblAlpha.BackgroundColor = Settings_Color[ColorElements.BACKGROUND];

                Settings_lblRed.TextColor = Settings_Color[ColorElements.TEXT];
                Settings_lblGreen.TextColor = Settings_Color[ColorElements.TEXT];
                Settings_lblBlue.TextColor = Settings_Color[ColorElements.TEXT];
                Settings_lblAlpha.TextColor = Settings_Color[ColorElements.TEXT];
            }
        }

        private void Settings_GetColors()
        {
            Settings_Color[0] = new Color(
                colorSettings.ControlBorder.R,
                colorSettings.ControlBorder.G,
                colorSettings.ControlBorder.B,
                colorSettings.ControlBorder.A);
            Settings_Color[1] = new Color(
                colorSettings.FrameBorder.R,
                colorSettings.FrameBorder.G,
                colorSettings.FrameBorder.B,
                colorSettings.FrameBorder.A);
            Settings_Color[2] = new Color(
                colorSettings.Background.R,
                colorSettings.Background.G,
                colorSettings.Background.B,
                colorSettings.Background.A);
            Settings_Color[3] = new Color(
                colorSettings.Text.R,
                colorSettings.Text.G,
                colorSettings.Text.B,
                colorSettings.Text.A);
            Settings_Color[4] = new Color(
                colorSettings.IsFavorite.R,
                colorSettings.IsFavorite.G,
                colorSettings.IsFavorite.B,
                colorSettings.IsFavorite.A);
            Settings_Color[5] = new Color(
                colorSettings.WhitePianoKey.R,
                colorSettings.WhitePianoKey.G,
                colorSettings.WhitePianoKey.B,
                colorSettings.WhitePianoKey.A);
            Settings_Color[6] = new Color(
                colorSettings.BlackPianoKey.R,
                colorSettings.BlackPianoKey.G,
                colorSettings.BlackPianoKey.B,
                colorSettings.BlackPianoKey.A);
            Settings_Color[7] = new Color(
                colorSettings.WhitePianoKeyText.R,
                colorSettings.WhitePianoKeyText.G,
                colorSettings.WhitePianoKeyText.B,
                colorSettings.WhitePianoKeyText.A);
            Settings_Color[8] = new Color(
                colorSettings.BlackPianoKeyText.R,
                colorSettings.BlackPianoKeyText.G,
                colorSettings.BlackPianoKeyText.B,
                colorSettings.BlackPianoKeyText.A);
            Settings_Color[9] = new Color(
                colorSettings.PianoKeyCover.R,
                colorSettings.PianoKeyCover.G,
                colorSettings.PianoKeyCover.B,
                colorSettings.PianoKeyCover.A);
            Settings_Color[10] = new Color(
                colorSettings.MotionalSurroundPartLabelText.R,
                colorSettings.MotionalSurroundPartLabelText.G,
                colorSettings.MotionalSurroundPartLabelText.B,
                colorSettings.MotionalSurroundPartLabelText.A);
            Settings_Color[11] = new Color(
                colorSettings.MotionalSurroundPartLabelFocused.R,
                colorSettings.MotionalSurroundPartLabelFocused.G,
                colorSettings.MotionalSurroundPartLabelFocused.B,
                colorSettings.MotionalSurroundPartLabelFocused.A);
            Settings_Color[12] = new Color(
                colorSettings.MotionalSurroundPartLabelUnfocused.R,
                colorSettings.MotionalSurroundPartLabelUnfocused.G,
                colorSettings.MotionalSurroundPartLabelUnfocused.B,
                colorSettings.MotionalSurroundPartLabelUnfocused.A);
            Color temp = (Color)colorSettings.ListViewTextColor.Values[TextCell.TextColorProperty];
            Settings_Color[13] = new Color(temp.R, temp.G, temp.B, temp.A);
        }

        private void Settings_SetSliders()
        {
            PushHandleControlEvents();
            Settings_slRed.Value =
                Settings_Color[Settings_pColorTypeSelector.SelectedIndex].R;
            Settings_slGreen.Value =
                Settings_Color[Settings_pColorTypeSelector.SelectedIndex].G;
            Settings_slBlue.Value =
                Settings_Color[Settings_pColorTypeSelector.SelectedIndex].B;
            Settings_slAlpha.Value =
                Settings_Color[Settings_pColorTypeSelector.SelectedIndex].A;
            PopHandleControlEvents();
        }

        private void SetStackLayoutColors(StackLayout stackLayout)
        {
            //for (Int32 i = 0; i < stackLayout.Children.Count; i++)
            //{
            //    SetControlColors(stackLayout.Children[i].child);
            //}
            foreach (Object child in stackLayout.Children)
            {
                SetControlColors(child);
            }
        }

        private void SetControlColors(Object control)
        {
            Type type = control.GetType();

            if (type == typeof(Button))
            {
                ((Button)control).TextColor = colorSettings.Text;
                ((Button)control).BackgroundColor = colorSettings.Background;
            }
            else if (type == typeof(Label))
            {
                ((Label)control).TextColor = colorSettings.Text;
                ((Label)control).BackgroundColor = colorSettings.Background;
            }
            else if (type == typeof(Switch))
            {
                ((Switch)control).OnColor = colorSettings.Text;
                ((Switch)control).BackgroundColor = colorSettings.Background;
            }
            else if (type == typeof(LabeledSwitch))
            {
                //((Switch)control).OnColor = colorSettings.Text;
                ((LabeledSwitch)control).LSLabel.BackgroundColor = colorSettings.Background;
                ((LabeledSwitch)control).LSSwitch.BackgroundColor = colorSettings.Background;
                ((LabeledSwitch)control).LSLabel.TextColor = colorSettings.Text;
                //((LabeledSwitch)control).LSSwitch.BackgroundColor = colorSettings.Background;
            }
            else if (type == typeof(ListView))
            {
                //((ListView)control).TextColor = colorSettings.Text;
                ((ListView)control).BackgroundColor = colorSettings.Background;
            }
            else if (type == typeof(Slider))
            {
                ((Slider)control).BackgroundColor = colorSettings.Background;
            }
            //else if (type == typeof(View))
            //{
            //    ((View)control).BackgroundColor = colorSettings.FrameBorder;
            //    foreach (Object child in ((Grid)control).Children)
            //    {
            //        SetControlColors(control);
            //    }
            //}
            else if (type == typeof(Grid))
            {
                ((View)control).BackgroundColor = colorSettings.FrameBorder;
                if (((Grid)control).Children.Count > 0)
                {
                    for (Int32 i = 0; i < ((Grid)control).Children.Count; i++)
                    {
                        if (((Grid)control).Children[i].GetType() == typeof(Slider))
                        {
                            ((View)control).BackgroundColor = colorSettings.Background;
                        }
                    }
                }

                foreach (Object child in ((Grid)control).Children)
                {
                    SetControlColors(child);
                }
            }
        }
    }

    public class ColorElements
    {
        public const Int32 CONTROL_BORDER = 0;
        public const Int32 FRAME_BORDER = 1;
        public const Int32 BACKGROUND = 2;
        public const Int32 TEXT = 3;
        public const Int32 IS_FAVORITE = 4;
        public const Int32 WHITE_PIANO_KEY = 5;
        public const Int32 BLACK_PIANO_KEY = 6;
        public const Int32 WHITE_PIANO_KEY_TEXT = 7;
        public const Int32 BLACK_PIANO_KEY_TEXT = 8;
        public const Int32 PIANO_KEY_COVER = 9;
        public const Int32 MOTIONAL_SURROUND_PART_LABEL_TEXT = 10;
        public const Int32 MOTIONAL_SURROUND_PART_LABEL_FOCUSED = 11;
        public const Int32 MOTIONAL_SURROUND_PART_LABEL_UNFOCUSED = 12;
        public const Int32 LISTINGS_TEXT = 13;
    }
}
