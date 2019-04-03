using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public enum MotionalSurroundInitializationState
    {
        NOT_INITIALIZED,
        INITIALIZING,
        INITIALIZED
    }

    public partial class UIHandler
    {
        Grid gParts;
        Grid gArrows;

        TouchableImage imgDoubleUpLeftArrow;
        TouchableImage imgDoubleUpLeftTopArrow;
        TouchableImage imgDoubleUpArrow;
        TouchableImage imgDoubleUpRightTopArrow;
        TouchableImage imgDoubleUpRightArrow;

        TouchableImage imgDoubleUpLeftLeftArrow;
        TouchableImage imgUpLeftArrow;
        TouchableImage imgUpArrow;
        TouchableImage imgUpRightArrow;
        TouchableImage imgDoubleUpRightRightArrow;

        TouchableImage imgDoubleLeftArrow;
        TouchableImage imgLeftArrow;
        TouchableImage imgCenter;
        TouchableImage imgRightArrow;
        TouchableImage imgDoubleRightArrow;

        TouchableImage imgDoubleDownLeftLeftArrow;
        TouchableImage imgDownLeftArrow;
        TouchableImage imgDownArrow;
        TouchableImage imgDownRightArrow;
        TouchableImage imgDoubleDownRightRightArrow;

        TouchableImage imgDoubleDownLeftArrow;
        TouchableImage imgDoubleDownLeftDownArrow;
        TouchableImage imgDoubleDownArrow;
        TouchableImage imgDoubleDownRightDownArrow;
        TouchableImage imgDoubleDownRightArrow;

        MotionalSurroundPartLabel[] mslPart;
        MotionalSurroundPartEditor[] msePart;
        Button btnMotionalSurroundReturn;
        Int32 currentMotionalSurroundPart = 0;
        MotionalSurroundInitializationState motionalSurroundInitializationState;

        public void ShowMotionalSurroundPage()
        {
            currentPage = CurrentPage.MOTIONAL_SURROUND;

            if (!MotionalSurround_IsCreated)
            {
                motionalSurroundInitializationState = MotionalSurroundInitializationState.NOT_INITIALIZED;
                DrawMotinalSurroundPage();
                MotionalSurround_StackLayout.MinimumWidthRequest = 1;
                mainStackLayout.Children.Add(MotionalSurround_StackLayout);
                MotionalSurround_Init();
                MotionalSurround_IsCreated = true;
                handleControlEvents = true;
                needsToSetFontSizes = NeedsToSetFontSizes.MOTIONAL_SURROUND;
            }
            MotionalSurround_StackLayout.IsVisible = true;
            if (commonState.StudioSet.PartMotionalSurround != null
                && commonState.StudioSet.PartMotionalSurround[0] != null
                && commonState.StudioSet.PartMotionalSurround[1] != null
                && commonState.StudioSet.PartMotionalSurround[2] != null
                && commonState.StudioSet.PartMotionalSurround[3] != null
                && commonState.StudioSet.PartMotionalSurround[4] != null
                && commonState.StudioSet.PartMotionalSurround[5] != null
                && commonState.StudioSet.PartMotionalSurround[6] != null
                && commonState.StudioSet.PartMotionalSurround[7] != null
                && commonState.StudioSet.PartMotionalSurround[8] != null
                && commonState.StudioSet.PartMotionalSurround[9] != null
                && commonState.StudioSet.PartMotionalSurround[10] != null
                && commonState.StudioSet.PartMotionalSurround[11] != null
                && commonState.StudioSet.PartMotionalSurround[12] != null
                && commonState.StudioSet.PartMotionalSurround[13] != null
                && commonState.StudioSet.PartMotionalSurround[14] != null
                && commonState.StudioSet.PartMotionalSurround[15] != null
                && commonState.StudioSet.MotionalSurround != null)
            {
                // Studio set might have been changed, so classes has other data, tell TimerTick to update controls:
                motionalSurroundInitializationState = MotionalSurroundInitializationState.INITIALIZING;
            }
        }

        public void DrawMotinalSurroundPage()
        {
            gArrows = new Grid();
            gArrows.HorizontalOptions = LayoutOptions.FillAndExpand;
            gArrows.VerticalOptions = LayoutOptions.FillAndExpand;
            for (byte i = 0; i < 5; i++)
            {
                gArrows.RowDefinitions.Add(new RowDefinition());
                gArrows.ColumnDefinitions.Add(new ColumnDefinition());
            }
            gParts = new Grid();

            imgDoubleUpLeftArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleUpLeftArrow.png", 0);
            imgDoubleUpLeftTopArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleUpLeftTopArrow.png", 1);
            imgDoubleUpArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleUpArrow.png", 2);
            imgDoubleUpRightTopArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleUpRightTopArrow.png", 3);
            imgDoubleUpRightArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleUpRightArrow.png", 4);

            imgDoubleUpLeftLeftArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleUpLeftLeftArrow.png", 5);
            imgUpLeftArrow = new TouchableImage(TapGestureRecognizer_Tapped, "UpLeftArrow.png", 6);
            imgUpArrow = new TouchableImage(TapGestureRecognizer_Tapped, "UpArrow.png", 7);
            imgUpRightArrow = new TouchableImage(TapGestureRecognizer_Tapped, "UpRightArrow.png", 8);
            imgDoubleUpRightRightArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleUpRightRightArrow.png", 9);

            imgDoubleLeftArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleLeftArrow.png", 10);
            imgLeftArrow = new TouchableImage(TapGestureRecognizer_Tapped, "LeftArrow.png", 11);
            imgCenter = new TouchableImage(TapGestureRecognizer_Tapped, "Center.png", 12);
            imgRightArrow = new TouchableImage(TapGestureRecognizer_Tapped, "RightArrow.png", 13);
            imgDoubleRightArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleRightArrow.png", 14);

            imgDoubleDownLeftLeftArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleDownLeftLeftArrow.png", 15);
            imgDownLeftArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DownLeftArrow.png", 16);
            imgDownArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DownArrow.png", 17);
            imgDownRightArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DownRightArrow.png", 18);
            imgDoubleDownRightRightArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleDownRightRightArrow.png", 19);

            imgDoubleDownLeftArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleDownLeftArrow.png", 20);
            imgDoubleDownLeftDownArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleDownLeftDownArrow.png", 21);
            imgDoubleDownArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleDownArrow.png", 22);
            imgDoubleDownRightDownArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleDownRightDownArrow.png", 23);
            imgDoubleDownRightArrow = new TouchableImage(TapGestureRecognizer_Tapped, "DoubleDownRightArrow.png", 24);

            Grid.SetRow(imgDoubleUpLeftArrow, 0);
            Grid.SetRow(imgDoubleUpLeftTopArrow, 0);
            Grid.SetRow(imgDoubleUpArrow, 0);
            Grid.SetRow(imgDoubleUpRightTopArrow, 0);
            Grid.SetRow(imgDoubleUpRightArrow, 0);

            Grid.SetRow(imgDoubleUpLeftLeftArrow, 1);
            Grid.SetRow(imgUpLeftArrow, 1);
            Grid.SetRow(imgUpArrow, 1);
            Grid.SetRow(imgUpRightArrow, 1);
            Grid.SetRow(imgDoubleUpRightRightArrow, 1);

            Grid.SetRow(imgDoubleLeftArrow, 2);
            Grid.SetRow(imgLeftArrow, 2);
            Grid.SetRow(imgCenter, 2);
            Grid.SetRow(imgRightArrow, 2);
            Grid.SetRow(imgDoubleRightArrow, 2);

            Grid.SetRow(imgDoubleDownLeftLeftArrow, 3);
            Grid.SetRow(imgDownLeftArrow, 3);
            Grid.SetRow(imgDownArrow, 3);
            Grid.SetRow(imgDownRightArrow, 3);
            Grid.SetRow(imgDoubleDownRightRightArrow, 3);

            Grid.SetRow(imgDoubleDownLeftArrow, 4);
            Grid.SetRow(imgDoubleDownLeftDownArrow, 4);
            Grid.SetRow(imgDoubleDownArrow, 4);
            Grid.SetRow(imgDoubleDownRightDownArrow, 4);
            Grid.SetRow(imgDoubleDownRightArrow, 4);

            Grid.SetColumn(imgDoubleUpLeftArrow, 0);
            Grid.SetColumn(imgDoubleUpLeftTopArrow, 1);
            Grid.SetColumn(imgDoubleUpArrow, 2);
            Grid.SetColumn(imgDoubleUpRightTopArrow, 3);
            Grid.SetColumn(imgDoubleUpRightArrow, 4);

            Grid.SetColumn(imgDoubleUpLeftLeftArrow, 0);
            Grid.SetColumn(imgUpLeftArrow, 1);
            Grid.SetColumn(imgUpArrow, 2);
            Grid.SetColumn(imgUpRightArrow, 3);
            Grid.SetColumn(imgDoubleUpRightRightArrow, 4);

            Grid.SetColumn(imgDoubleLeftArrow, 0);
            Grid.SetColumn(imgLeftArrow, 1);
            Grid.SetColumn(imgCenter, 2);
            Grid.SetColumn(imgRightArrow, 3);
            Grid.SetColumn(imgDoubleRightArrow, 4);

            Grid.SetColumn(imgDoubleDownLeftLeftArrow, 0);
            Grid.SetColumn(imgDownLeftArrow, 1);
            Grid.SetColumn(imgDownArrow, 2);
            Grid.SetColumn(imgDownRightArrow, 3);
            Grid.SetColumn(imgDoubleDownRightRightArrow, 4);

            Grid.SetColumn(imgDoubleDownLeftArrow, 0);
            Grid.SetColumn(imgDoubleDownLeftDownArrow, 1);
            Grid.SetColumn(imgDoubleDownArrow, 2);
            Grid.SetColumn(imgDoubleDownRightDownArrow, 3);
            Grid.SetColumn(imgDoubleDownRightArrow, 4);

            imgDoubleUpLeftArrow.Aspect = Aspect.Fill;
            imgDoubleUpLeftTopArrow.Aspect = Aspect.Fill;
            imgDoubleUpArrow.Aspect = Aspect.Fill;
            imgDoubleUpRightTopArrow.Aspect = Aspect.Fill;
            imgDoubleUpRightArrow.Aspect = Aspect.Fill;

            imgDoubleUpLeftLeftArrow.Aspect = Aspect.Fill;
            imgUpLeftArrow.Aspect = Aspect.Fill;
            imgUpArrow.Aspect = Aspect.Fill;
            imgUpRightArrow.Aspect = Aspect.Fill;
            imgDoubleUpRightRightArrow.Aspect = Aspect.Fill;

            imgDoubleLeftArrow.Aspect = Aspect.Fill;
            imgLeftArrow.Aspect = Aspect.Fill;
            imgCenter.Aspect = Aspect.Fill;
            imgRightArrow.Aspect = Aspect.Fill;
            imgDoubleRightArrow.Aspect = Aspect.Fill;

            imgDoubleDownLeftLeftArrow.Aspect = Aspect.Fill;
            imgDownLeftArrow.Aspect = Aspect.Fill;
            imgDownArrow.Aspect = Aspect.Fill;
            imgDownRightArrow.Aspect = Aspect.Fill;
            imgDoubleDownRightRightArrow.Aspect = Aspect.Fill;

            imgDoubleDownLeftArrow.Aspect = Aspect.Fill;
            imgDoubleDownLeftDownArrow.Aspect = Aspect.Fill;
            imgDoubleDownArrow.Aspect = Aspect.Fill;
            imgDoubleDownRightDownArrow.Aspect = Aspect.Fill;
            imgDoubleDownRightArrow.Aspect = Aspect.Fill;

            gArrows.Children.Add(imgDoubleUpLeftArrow);
            gArrows.Children.Add(imgDoubleUpLeftTopArrow);
            gArrows.Children.Add(imgDoubleUpArrow);
            gArrows.Children.Add(imgDoubleUpRightTopArrow);
            gArrows.Children.Add(imgDoubleUpRightArrow);

            gArrows.Children.Add(imgDoubleUpLeftLeftArrow);
            gArrows.Children.Add(imgUpLeftArrow);
            gArrows.Children.Add(imgUpArrow);
            gArrows.Children.Add(imgUpRightArrow);
            gArrows.Children.Add(imgDoubleUpRightRightArrow);

            gArrows.Children.Add(imgDoubleLeftArrow);
            gArrows.Children.Add(imgLeftArrow);
            gArrows.Children.Add(imgCenter);
            gArrows.Children.Add(imgRightArrow);
            gArrows.Children.Add(imgDoubleRightArrow);

            gArrows.Children.Add(imgDoubleDownLeftLeftArrow);
            gArrows.Children.Add(imgDownLeftArrow);
            gArrows.Children.Add(imgDownArrow);
            gArrows.Children.Add(imgDownRightArrow);
            gArrows.Children.Add(imgDoubleDownRightRightArrow);

            gArrows.Children.Add(imgDoubleDownLeftArrow);
            gArrows.Children.Add(imgDoubleDownLeftDownArrow);
            gArrows.Children.Add(imgDoubleDownArrow);
            gArrows.Children.Add(imgDoubleDownRightDownArrow);
            gArrows.Children.Add(imgDoubleDownRightArrow);

            // Assemble MotionalSurroundStackLayout --------------------------------------------------------------

            Grid.SetRow(gArrows, 0);
            Grid.SetColumn(gArrows, 0);
            gArrows.Margin = new Thickness(0);
            gArrows.ColumnSpacing = 0;
            gArrows.RowSpacing = 0;

            mslPart = new MotionalSurroundPartLabel[17];
            for (Int32 i = 17; i > 0; i--)
            {
                mslPart[i - 1] = new MotionalSurroundPartLabel(i);
                Grid.SetRowSpan(mslPart[i - 1], 5);
                Grid.SetColumnSpan(mslPart[i - 1], 5);
                mslPart[i - 1].Tag = i - 1;
                mslPart[i - 1].TextColor = UIHandler.colorSettings.MotionalSurroundPartLabelText;
                if (i == 0)
                {
                    mslPart[i - 1].BackgroundColor = UIHandler.colorSettings.MotionalSurroundPartLabelUnfocused;
                }
                else
                {
                    mslPart[i - 1].BackgroundColor = UIHandler.colorSettings.MotionalSurroundPartLabelFocused;
                }
                gArrows.Children.Add(mslPart[i - 1]);
                mslPart[i - 1].Clicked += MsPart_Clicked;
                //mslPart[i - 1].Focused += MotionalSurroundPartLabel_Focused;
                //mslPart[i - 1].Unfocused += MotionalSurroundPartLabel_Unfocused;
            }

            msePart = new MotionalSurroundPartEditor[17];
            for (Int32 i = 0; i < 17; i++)
            {
                msePart[i] = new MotionalSurroundPartEditor(i + 1);
                msePart[i].Tag = i;
                if (i < 16)
                {
                    msePart[i].Editor.Text = "Part " + (i + 1).ToString();
                }
                else
                {
                    msePart[i].Editor.Text = "Ext";
                }
                msePart[i].Switch.LSSwitch.Toggled += msePart_Toggled;
                msePart[i].Switch.LSSwitch.IsToggled = true;
                msePart[i].Editor.TextChanged += msePartEditor_TextChanged;
                gParts.Children.Add(new GridRow((byte)i, new View[] { msePart[i] }).Row);
            }
            btnMotionalSurroundReturn = new Button();
            btnMotionalSurroundReturn.HorizontalOptions = LayoutOptions.FillAndExpand;
            btnMotionalSurroundReturn.VerticalOptions = LayoutOptions.FillAndExpand;
            btnMotionalSurroundReturn.Text = "Return to Librarian";
            btnMotionalSurroundReturn.Clicked += BtnMotionalSurroundReturn_Clicked;

            gParts.Children.Add(new GridRow(17, new View[] { btnMotionalSurroundReturn }).Row);

            MotionalSurround_StackLayout = new StackLayout();
            MotionalSurround_StackLayout.Children.Add(new GridRow(0, new View[] { gArrows, gParts }, new byte[] { 5, 2 }).Row);
        }

        private void MotionalSurround_Init()
        {
            if (commonState == null)
            {
                // If commonState is not initialized, we have no business here, go back!
                MotionalSurround_StackLayout.IsVisible = false;
                ShowLibrarianPage();
            }
            else if (commonState.StudioSet == null)
            {
                // StudioSet set has not been read, thus we have no Motional Surround data. 
                // Start by creating the studioSet object:
                commonState.StudioSet = new StudioSet();

                // Then get the Motional Lurround data by borrowing code from Studio set editor:
                QueryStudioSetMotionalSurround(); // This will be caught in MotionalSurrouns_MidiInPort_MessageReceived()
            }
        }

        public void MotionalSurround_Timer_Tick()
        {
            if (motionalSurroundInitializationState == MotionalSurroundInitializationState.INITIALIZING)
            {
                motionalSurroundInitializationState =
                    MotionalSurroundInitializationState.INITIALIZED;

                // Place part labels:
                for (currentMotionalSurroundPart = 0; currentMotionalSurroundPart < 16; currentMotionalSurroundPart++)
                {
                    mslPart[currentMotionalSurroundPart].Horizontal = 
                        commonState.StudioSet.PartMotionalSurround[currentMotionalSurroundPart].LR;
                    mslPart[currentMotionalSurroundPart].Vertical = 
                        (byte)(127 - commonState.StudioSet.PartMotionalSurround[currentMotionalSurroundPart].FB);
                    mslPart[currentMotionalSurroundPart].IsVisible = 
                        msePart[currentMotionalSurroundPart].Switch.LSSwitch.IsToggled;
                    Int32 index = commonState.ToneList.Get(commonState.StudioSet
                        .PartMainSettings[currentMotionalSurroundPart].ToneBankSelectMSB,
                        commonState.StudioSet.PartMainSettings[currentMotionalSurroundPart].ToneBankSelectLSB,
                        commonState.StudioSet.PartMainSettings[currentMotionalSurroundPart].ToneProgramNumber);
                    mslPart[currentMotionalSurroundPart].Text = 
                        commonState.ToneList.Tones[index][3];
                    msePart[currentMotionalSurroundPart].Editor.Text = 
                        mslPart[currentMotionalSurroundPart].Text;
                    //msePart[currentMotionalSurroundPart].Switch.IsChecked = 
                    //    commonState.StudioSet.PartMotionalSurround[currentMotionalSurroundPart].MotionalSurroundSwitch;
                    mslPart[currentMotionalSurroundPart].Plot(gArrows.Width, gArrows.Height);
                }
                mslPart[currentMotionalSurroundPart].Horizontal = 
                    commonState.StudioSet.MotionalSurround.ExtPartLR;
                mslPart[currentMotionalSurroundPart].Vertical = 
                    (byte)(127 -commonState.StudioSet.MotionalSurround.ExtPartFB);
                mslPart[currentMotionalSurroundPart].IsVisible = 
                    msePart[currentMotionalSurroundPart].Switch.LSSwitch.IsToggled;
                mslPart[currentMotionalSurroundPart].Text = 
                    msePart[currentMotionalSurroundPart].Editor.Text;
                //msePart[currentMotionalSurroundPart].Switch.IsChecked = 
                //    commonState.StudioSet.MotionalSurround.MotionalSurroundSwitch;
                mslPart[currentMotionalSurroundPart].Plot(gArrows.Width, gArrows.Height);
            }
        }

        private void MotionalSurrouns_MidiInPort_MessageReceived()
        {
            if (currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MOTIONAL_SURROUND)
            {
                ReadMotionalSurround(false); // We borrow this too from the Studio Set Editor
                currentMotionalSurroundPart = 0;
                QueryStudioSetPart(currentMotionalSurroundPart);
            }
            else if (currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART)
            {
                // Unpack studio set part:
                ReadStudioSetPart(currentMotionalSurroundPart, false); // We borrow this from the Studio Set Editor
                currentMotionalSurroundPart++;
                if (currentMotionalSurroundPart < 16)
                {
                    QueryStudioSetPart(currentMotionalSurroundPart); // We borrow this from the Studio Set Editor
                }
                else
                {
                    // If user has set I-7 to transmit MIDI edits, it will trigger MotionalSurrouns_MidiInPort_MessageReceived()
                    // and cause whatever havoc and crash. Better tell ourself not to react anymore:
                    currentStudioSetEditorMidiRequest = StudioSetEditor_currentStudioSetEditorMidiRequest.NONE;

                    // This was the last consequence of calling Init(), so tell Timer_Tick to do the rest:
                    // (The rest involves UI and can not be done from the current thread.)
                    motionalSurroundInitializationState = MotionalSurroundInitializationState.INITIALIZING;
                }
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Int32 index = (Int32)((TouchableImage)sender).Tag;
            mslPart[currentMotionalSurroundPart].Step(index,
                gArrows.Width, gArrows.Height);
            SendCoordinatesToIntegra7();
        }

        public void SendCoordinatesToIntegra7()
        {
            byte[] address;
            byte[] value;
            byte[] bytes;

            if (currentMotionalSurroundPart == 16)
            {
                address = new byte[] { 0x18, 0x00, 0x08, 0x07 };
                value = new byte[] { mslPart[currentMotionalSurroundPart].Horizontal,
                    (byte)(0x7f - mslPart[currentMotionalSurroundPart].Vertical)};
                bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
                commonState.Midi.SendSystemExclusive(bytes);
            }
            else
            {
                byte part = (byte)(0x20 + (byte)currentMotionalSurroundPart);
                address = new byte[] { 0x18, 0x00, part, 0x44 };
                value = new byte[] { mslPart[currentMotionalSurroundPart].Horizontal };
                bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
                commonState.Midi.SendSystemExclusive(bytes);
                address = new byte[] { 0x18, 0x00, part, 0x46 };
                value = new byte[] { (byte)(0x7f - mslPart[currentMotionalSurroundPart].Vertical) };
                bytes = commonState.Midi.SystemExclusiveDT1Message(address, value);
                commonState.Midi.SendSystemExclusive(bytes);
            }
        }

        private void msePartEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            Int32 Tag = ((INTEGRA_7_Xamarin.MotionalSurroundPartEditor)((Grid)((Editor)sender).Parent).Parent).Tag;
            mslPart[Tag].Text = ((Editor)sender).Text;
            //mslPart[Tag].MinimumWidthRequest = 20 + mslPart[Tag].Text.Length * 10;
            //mslPart[Tag].WidthRequest = 20 + mslPart[Tag].Text.Length * 10;
        }

        private void msePart_Toggled(object sender, ToggledEventArgs e)
        {
            Int32 Tag = ((MotionalSurroundPartEditor)((Grid)((LabeledSwitch)((Grid)((Switch)sender).Parent).Parent).Parent).Parent).Tag;
            currentMotionalSurroundPart = Tag;
            mslPart[Tag].IsVisible = ((Switch)sender).IsToggled;

            // When turned on, also put it in current position:
            //if (mslPart[Tag].IsVisible)
            //{
            //    mslPart[Tag].Plot(gArrows.Width, gArrows.Height);
            //}
        }

        private void MsPart_Clicked(object sender, EventArgs e)
        {
            currentMotionalSurroundPart = (Int32)((Button)sender).Tag;
            for (Int32 i = 0; i < 17; i++)
            {
                if (i == currentMotionalSurroundPart)
                {
                    mslPart[i].BackgroundColor = UIHandler.colorSettings.MotionalSurroundPartLabelFocused;
                }
                else
                {
                    mslPart[i].BackgroundColor = UIHandler.colorSettings.MotionalSurroundPartLabelUnfocused;
                }
            }
        }

        //private void MotionalSurroundPartLabel_Unfocused(object sender, FocusEventArgs e)
        //{
        //    ((MotionalSurroundPartLabel)sender).BackgroundColor = UIHandler.colorSettings.MotionalSurroundPartLabelUnfocused;
        //}

        //private void MotionalSurroundPartLabel_Focused(object sender, FocusEventArgs e)
        //{
        //    ((MotionalSurroundPartLabel)sender).BackgroundColor = UIHandler.colorSettings.MotionalSurroundPartLabelFocused;
        //}

        private void BtnMotionalSurroundReturn_Clicked(object sender, EventArgs e)
        {
            MotionalSurround_StackLayout.IsVisible = false;
            ShowLibrarianPage();
        }
    }
}
