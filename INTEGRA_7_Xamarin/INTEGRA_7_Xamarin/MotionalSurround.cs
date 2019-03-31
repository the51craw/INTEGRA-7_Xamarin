using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integra_7_Xamarin
{
    public enum MotionalSurroundInitializationState
    {
        NOT_INITIALIZED,
        INITIALIZING,
        INITIALIZED
    }

    public partial class UIHandler
    {
        Grid gMotionalSurroundMainArea;
        Grid gMotionalSurroundLabelsArea;
        Grid gRightColumn;
        Grid gParts;
        Grid gArrows;

        Image imgDoubleUpLeftArrow;
        Image imgDoubleUpLeftTopArrow;
        Image imgDoubleUpArrow;
        Image imgDoubleUpRightTopArrow;
        Image imgDoubleUpRightArrow;

        Image imgDoubleUpLeftLeftArrow;
        Image imgUpLeftArrow;
        Image imgUpArrow;
        Image imgUpRightArrow;
        Image imgDoubleUpRightRightArrow;

        Image imgDoubleLeftArrow;
        Image imgLeftArrow;
        Image imgCenter;
        Image imgRightArrow;
        Image imgDoubleRightArrow;

        Image imgDoubleDownLeftLeftArrow;
        Image imgDownLeftArrow;
        Image imgDownArrow;
        Image imgDownRightArrow;
        Image imgDoubleDownRightRightArrow;

        Image imgDoubleDownLeftArrow;
        Image imgDoubleDownLeftDownArrow;
        Image imgDoubleDownArrow;
        Image imgDoubleDownRightDownArrow;
        Image imgDoubleDownRightArrow;

        Button tbPart1;
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
        }

        public void DrawMotinalSurroundPage()
        {
            gMotionalSurroundMainArea = new Grid();
            gMotionalSurroundMainArea.HorizontalOptions = LayoutOptions.CenterAndExpand;
            gMotionalSurroundMainArea.VerticalOptions = LayoutOptions.CenterAndExpand;
            gMotionalSurroundLabelsArea = new Grid();
            gMotionalSurroundLabelsArea.HorizontalOptions = LayoutOptions.CenterAndExpand;
            gMotionalSurroundLabelsArea.VerticalOptions = LayoutOptions.CenterAndExpand;
            gArrows = new Grid();
            gArrows.HorizontalOptions = LayoutOptions.FillAndExpand;
            gArrows.VerticalOptions = LayoutOptions.FillAndExpand;
            for (byte i = 0; i < 5; i++)
            {
                gArrows.RowDefinitions.Add(new RowDefinition());
                gArrows.ColumnDefinitions.Add(new ColumnDefinition());
            }
            gRightColumn = new Grid();
            gParts = new Grid();

            imgDoubleUpLeftArrow = new Image();
            imgDoubleUpLeftTopArrow = new Image();
            imgDoubleUpArrow = new Image();
            imgDoubleUpRightTopArrow = new Image();
            imgDoubleUpRightArrow = new Image();

            imgDoubleUpLeftLeftArrow = new Image();
            imgUpLeftArrow = new Image();
            imgUpArrow = new Image();
            imgUpRightArrow = new Image();
            imgDoubleUpRightRightArrow = new Image();

            imgDoubleLeftArrow = new Image();
            imgLeftArrow = new Image();
            imgCenter = new Image();
            imgRightArrow = new Image();
            imgDoubleRightArrow = new Image();

            imgDoubleDownLeftLeftArrow = new Image();
            imgDownLeftArrow = new Image();
            imgDownArrow = new Image();
            imgDownRightArrow = new Image();
            imgDoubleDownRightRightArrow = new Image();

            imgDoubleDownLeftArrow = new Image();
            imgDoubleDownLeftDownArrow = new Image();
            imgDoubleDownArrow = new Image();
            imgDoubleDownRightDownArrow = new Image();
            imgDoubleDownRightArrow = new Image();

            imgDoubleUpLeftArrow.Source = ImageSource.FromFile("DoubleUpLeftArrow.png");
            imgDoubleUpLeftTopArrow.Source = ImageSource.FromFile("DoubleUpLeftTopArrow.png");
            imgDoubleUpArrow.Source = ImageSource.FromFile("DoubleUpArrow.png");
            imgDoubleUpRightTopArrow.Source = ImageSource.FromFile("DoubleUpRightTopArrow.png");
            imgDoubleUpRightArrow.Source = ImageSource.FromFile("DoubleUpRightArrow.png");

            imgDoubleUpLeftLeftArrow.Source = ImageSource.FromFile("DoubleUpLeftLeftArrow.png");
            imgUpLeftArrow.Source = ImageSource.FromFile("UpLeftArrow.png");
            imgUpArrow.Source = ImageSource.FromFile("UpArrow.png");
            imgUpRightArrow.Source = ImageSource.FromFile("UpRightArrow.png");
            imgDoubleUpRightRightArrow.Source = ImageSource.FromFile("DoubleUpRightRightArrow.png");

            imgDoubleLeftArrow.Source = ImageSource.FromFile("DoubleLeftArrow.png");
            imgLeftArrow.Source = ImageSource.FromFile("LeftArrow.png");
            imgCenter.Source = ImageSource.FromFile("Center.png");
            imgRightArrow.Source = ImageSource.FromFile("RightArrow.png");
            imgDoubleRightArrow.Source = ImageSource.FromFile("DoubleRightArrow.png");

            imgDoubleDownLeftLeftArrow.Source = ImageSource.FromFile("DoubleDownLeftLeftArrow.png");
            imgDownLeftArrow.Source = ImageSource.FromFile("DownLeftArrow.png");
            imgDownArrow.Source = ImageSource.FromFile("DownArrow.png");
            imgDownRightArrow.Source = ImageSource.FromFile("DownRightArrow.png");
            imgDoubleDownRightRightArrow.Source = ImageSource.FromFile("DoubleDownRightRightArrow.png");

            imgDoubleDownLeftArrow.Source = ImageSource.FromFile("DoubleDownLeftArrow.png");
            imgDoubleDownLeftDownArrow.Source = ImageSource.FromFile("DoubleDownLeftDownArrow.png");
            imgDoubleDownArrow.Source = ImageSource.FromFile("DoubleDownArrow.png");
            imgDoubleDownRightDownArrow.Source = ImageSource.FromFile("DoubleDownRightDownArrow.png");
            imgDoubleDownRightArrow.Source = ImageSource.FromFile("DoubleDownRightArrow.png");

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

            imgDoubleUpLeftArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(0);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleUpLeftTopArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(1);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleUpArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(2);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleUpRightTopArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(3);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleUpRightArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(4);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleUpLeftLeftArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(5);
                }),
                NumberOfTapsRequired = 1
            });

            imgUpLeftArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(6);
                }),
                NumberOfTapsRequired = 1
            });

            imgUpArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(7);
                }),
                NumberOfTapsRequired = 1
            });

            imgUpRightArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(8);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleUpRightRightArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(9);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleLeftArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(10);
                }),
                NumberOfTapsRequired = 1
            });

            imgLeftArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(11);
                }),
                NumberOfTapsRequired = 1
            });

            imgCenter.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(12);
                }),
                NumberOfTapsRequired = 1
            });

            imgRightArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(13);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleRightArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(14);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleDownLeftLeftArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(15);
                }),
                NumberOfTapsRequired = 1
            });

            imgDownLeftArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(16);
                }),
                NumberOfTapsRequired = 1
            });

            imgDownArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(17);
                }),
                NumberOfTapsRequired = 1
            });

            imgDownRightArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(18);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleDownRightRightArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(19);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleDownLeftArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(20);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleDownLeftDownArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(21);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleDownArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(22);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleDownRightDownArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(23);
                }),
                NumberOfTapsRequired = 1
            });

            imgDoubleDownRightArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    TapGestureRecognizer_Tapped(24);
                }),
                NumberOfTapsRequired = 1
            });

            // Assemble MotionalSurroundStackLayout --------------------------------------------------------------

            Grid.SetRow(gArrows, 0);
            Grid.SetColumn(gArrows, 0);
            Grid.SetRow(gMotionalSurroundLabelsArea, 0);
            Grid.SetColumn(gMotionalSurroundLabelsArea, 0);
            gArrows.Margin = new Thickness(0);
            gArrows.ColumnSpacing = 0;
            gArrows.RowSpacing = 0;
            gMotionalSurroundLabelsArea.HorizontalOptions = LayoutOptions.FillAndExpand;
            gMotionalSurroundLabelsArea.VerticalOptions = LayoutOptions.FillAndExpand;
            gMotionalSurroundLabelsArea.Margin = new Thickness(50);
            gMotionalSurroundMainArea.Children.Add(gArrows);
            gMotionalSurroundMainArea.Children.Add(gMotionalSurroundLabelsArea);

            mslPart = new MotionalSurroundPartLabel[17];
            for (Int32 i = 17; i > 0; i--)
            {
                mslPart[i - 1] = new MotionalSurroundPartLabel(i);
                //mslPart[i - 1].Step(Direction.CENTER, gMotionalSurroundLabelsArea.Width, gMotionalSurroundLabelsArea.Height);
                mslPart[i - 1].Tag = i - 1;
                mslPart[i - 1].Clicked += MsPart_Clicked;
                gMotionalSurroundLabelsArea.Children.Add(mslPart[i - 1]);
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
                    msePart[i].Editor.Text = "Ext ";
                }
                msePart[i].Switch.LSSwitch.Toggled += msePart_Toggled;
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
            MotionalSurround_StackLayout.Children.Add(new GridRow(0, new View[] { gMotionalSurroundMainArea, gParts }, new byte[] { 5, 2 }).Row);
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
                // Place part labels:
                for (currentMotionalSurroundPart = 0; currentMotionalSurroundPart < 16; currentMotionalSurroundPart++)
                {
                    mslPart[currentMotionalSurroundPart].Horizontal = commonState.StudioSet.PartMotionalSurround[currentMotionalSurroundPart].LR;
                    mslPart[currentMotionalSurroundPart].Vertical = (byte)(127 - commonState.StudioSet.PartMotionalSurround[currentMotionalSurroundPart].FB);
                    mslPart[currentMotionalSurroundPart].Plot(gMotionalSurroundLabelsArea.Width, gMotionalSurroundLabelsArea.Height);
                    mslPart[currentMotionalSurroundPart].IsVisible = msePart[currentMotionalSurroundPart].Switch.LSSwitch.IsToggled;
                    mslPart[currentMotionalSurroundPart].Text = msePart[currentMotionalSurroundPart].Editor.Text;
                    mslPart[currentMotionalSurroundPart].MinimumWidthRequest = 20 + mslPart[currentMotionalSurroundPart].Text.Length * 10;
                    mslPart[currentMotionalSurroundPart].WidthRequest = 20 + mslPart[currentMotionalSurroundPart].Text.Length * 10;
                }
                mslPart[currentMotionalSurroundPart].Horizontal = commonState.StudioSet.MotionalSurround.ExtPartLR;
                mslPart[currentMotionalSurroundPart].Vertical = (byte)(127 -commonState.StudioSet.MotionalSurround.ExtPartFB);
                mslPart[currentMotionalSurroundPart].Plot(gMotionalSurroundLabelsArea.Width, gMotionalSurroundLabelsArea.Height);
                mslPart[currentMotionalSurroundPart].IsVisible = msePart[currentMotionalSurroundPart].Switch.LSSwitch.IsToggled;
                mslPart[currentMotionalSurroundPart].Text = msePart[currentMotionalSurroundPart].Editor.Text;
                mslPart[currentMotionalSurroundPart].MinimumWidthRequest = 20 + mslPart[currentMotionalSurroundPart].Text.Length * 10;
                mslPart[currentMotionalSurroundPart].WidthRequest = 20 + mslPart[currentMotionalSurroundPart].Text.Length * 10;
                motionalSurroundInitializationState = MotionalSurroundInitializationState.INITIALIZED;
            }
        }

        private void MotionalSurrouns_MidiInPort_MessageReceived()
        {
            if (currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_MOTIONAL_SURROUND)
            {
                ReadMotionalSurround(); // We borrow this too from the Studio Set Editor
                currentMotionalSurroundPart = 0;
                QueryStudioSetPart(currentMotionalSurroundPart);
            }
            else if (currentStudioSetEditorMidiRequest == StudioSetEditor_currentStudioSetEditorMidiRequest.STUDIO_SET_PART)
            {
                // Unpack studio set part:
                ReadStudioSetPart(currentMotionalSurroundPart); // We borrow this from the Studio Set Editor
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

        private void TapGestureRecognizer_Tapped(Int32 direction)
        {
            mslPart[currentMotionalSurroundPart].Step(direction,
                gMotionalSurroundLabelsArea.Width, gMotionalSurroundLabelsArea.Height);
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
            Int32 Tag = ((Integra_7_Xamarin.MotionalSurroundPartEditor)((Grid)((Editor)sender).Parent).Parent).Tag;
            mslPart[Tag].Text = ((Editor)sender).Text;
            mslPart[Tag].MinimumWidthRequest = 20 + mslPart[Tag].Text.Length * 10;
            mslPart[Tag].WidthRequest = 20 + mslPart[Tag].Text.Length * 10;
        }

        private void msePart_Toggled(object sender, ToggledEventArgs e)
        {
            Int32 Tag = ((MotionalSurroundPartEditor)((Grid)((LabeledSwitch)((Grid)((Switch)sender).Parent).Parent).Parent).Parent).Tag;
            currentMotionalSurroundPart = Tag;
            mslPart[Tag].IsVisible = ((Switch)sender).IsToggled;

            // When turned on, also put it in right position:
            if (mslPart[Tag].IsVisible)
            {
                mslPart[Tag].Plot(gMotionalSurroundLabelsArea.Width, gMotionalSurroundLabelsArea.Height);
            }
        }

        private void MsPart_Clicked(object sender, EventArgs e)
        {
            currentMotionalSurroundPart = (Int32)((Button)sender).Tag;
        }

        private void BtnMotionalSurroundReturn_Clicked(object sender, EventArgs e)
        {
            MotionalSurround_StackLayout.IsVisible = false;
            ShowLibrarianPage();
        }
    }
}
