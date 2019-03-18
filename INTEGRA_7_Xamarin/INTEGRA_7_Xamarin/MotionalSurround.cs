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

    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        UPLEFT,
        UPRIGHT,
        DOWNLEFT,
        DOWNRIGHT,
        DOUBLE_UP,
        DOUBLE_DOWN,
        DOUBLE_LEFT,
        DOUBLE_RIGHT,
        DOUBLE_UPLEFT,
        DOUBLE_UPRIGHT,
        DOUBLE_DOWNLEFT,
        DOUBLE_DOWNRIGHT,
        CENTER
    }

    public partial class UIHandler
    {
        Grid gMotionalSurroundMainArea;
        Grid gMotionalSurroundBackground;
        Grid gMotionalSurroundLabelsArea;
        Grid gRightColumn;
        Grid gParts;
        Grid gArrows;
        TaggedImage imgMotionalSurroundBackground;
        TaggedImage imgCenter;
        TaggedImage imgUpArrow;
        TaggedImage imgDownArrow;
        TaggedImage imgLeftArrow;
        TaggedImage imgRightArrow;
        TaggedImage imgUpRightArrow;
        TaggedImage imgDownRightArrow;
        TaggedImage imgUpLeftArrow;
        TaggedImage imgDownLeftArrow;
        TaggedImage imgDoubleUpArrow;
        TaggedImage imgDoubleDownArrow;
        TaggedImage imgDoubleLeftArrow;
        TaggedImage imgDoubleRightArrow;
        TaggedImage imgDoubleUpRightArrow;
        TaggedImage imgDoubleDownRightArrow;
        TaggedImage imgDoubleUpLeftArrow;
        TaggedImage imgDoubleDownLeftArrow;
        Button btnCenter;
        Button btnUpArrow;
        Button btnDownArrow;
        Button btnLeftArrow;
        Button btnRightArrow;
        Button btnUpRightArrow;
        Button btnDownRightArrow;
        Button btnUpLeftArrow;
        Button btnDownLeftArrow;
        Button btnDoubleUpArrow;
        Button btnDoubleDownArrow;
        Button btnDoubleLeftArrow;
        Button btnDoubleRightArrow;
        Button btnDoubleUpRightArrow;
        Button btnDoubleDownRightArrow;
        Button btnDoubleUpLeftArrow;
        Button btnDoubleDownLeftArrow;
        //Button tbPart1;
        MotionalSurroundPartLabel[] mslPart;
        MotionalSurroundPartEditor[] msePart;
        Button btnMotionalSurroundReturn;
        Int32 currentMotionalSurroundPart = 0;
        //Boolean MotionalSurround_Initiating;
        MotionalSurroundInitializationState motionalSurroundInitializationState;

        public void ShowMotionalSurroundPage()
        {
            Page = _page.MOTIONAL_SURROUND;

            if (!MotionalSurround_IsCreated)
            {
                motionalSurroundInitializationState = MotionalSurroundInitializationState.NOT_INITIALIZED;
                DrawMotinalSurroundPage();
                MotionalSurround_StackLayout.MinimumWidthRequest = 1;
                mainStackLayout.Children.Add(MotionalSurround_StackLayout);
                MotionalSurround_Init();
                MotionalSurround_IsCreated = true;
                handleControlEvents = true;
            }
            MotionalSurround_StackLayout.IsVisible = true;
        }

        public void DrawMotinalSurroundPage()
        {
            gMotionalSurroundMainArea = new Grid();
            gMotionalSurroundMainArea.HorizontalOptions = LayoutOptions.CenterAndExpand;
            gMotionalSurroundMainArea.VerticalOptions = LayoutOptions.CenterAndExpand;
            gMotionalSurroundBackground = new Grid();
            gMotionalSurroundBackground.HorizontalOptions = LayoutOptions.FillAndExpand;
            gMotionalSurroundBackground.VerticalOptions = LayoutOptions.FillAndExpand;
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
            imgMotionalSurroundBackground = new TaggedImage();
            imgCenter = new TaggedImage();
            imgUpArrow = new TaggedImage();
            imgDownArrow = new TaggedImage();
            imgLeftArrow = new TaggedImage();
            imgRightArrow = new TaggedImage();
            imgUpRightArrow = new TaggedImage();
            imgDownRightArrow = new TaggedImage();
            imgUpLeftArrow = new TaggedImage();
            imgDownLeftArrow = new TaggedImage();
            imgDoubleUpArrow = new TaggedImage();
            imgDoubleDownArrow = new TaggedImage();
            imgDoubleLeftArrow = new TaggedImage();
            imgDoubleRightArrow = new TaggedImage();
            imgDoubleUpRightArrow = new TaggedImage();
            imgDoubleDownRightArrow = new TaggedImage();
            imgDoubleUpLeftArrow = new TaggedImage();
            imgDoubleDownLeftArrow = new TaggedImage();
            imgMotionalSurroundBackground.Source = ImageSource.FromFile("MotionalSurround.png");
            imgCenter.Source = ImageSource.FromFile("Center.png");
            imgUpArrow.Source = ImageSource.FromFile("UpArrow.png");
            imgDownArrow.Source = ImageSource.FromFile("DownArrow.png");
            imgLeftArrow.Source = ImageSource.FromFile("LeftArrow.png");
            imgRightArrow.Source = ImageSource.FromFile("RightArrow.png");
            imgUpRightArrow.Source = ImageSource.FromFile("UpRightArrow.png");
            imgDownRightArrow.Source = ImageSource.FromFile("DownRightArrow.png");
            imgUpLeftArrow.Source = ImageSource.FromFile("UpLeftArrow.png");
            imgDownLeftArrow.Source = ImageSource.FromFile("DownLeftArrow.png");
            imgDoubleUpArrow.Source = ImageSource.FromFile("DoubleUpArrow.png");
            imgDoubleDownArrow.Source = ImageSource.FromFile("DoubleDownArrow.png");
            imgDoubleLeftArrow.Source = ImageSource.FromFile("DoubleLeftArrow.png");
            imgDoubleRightArrow.Source = ImageSource.FromFile("DoubleRightArrow.png");
            imgDoubleUpRightArrow.Source = ImageSource.FromFile("DoubleUpRightArrow.png");
            imgDoubleDownRightArrow.Source = ImageSource.FromFile("DoubleDownRightArrow.png");
            imgDoubleUpLeftArrow.Source = ImageSource.FromFile("DoubleUpLeftArrow.png");
            imgDoubleDownLeftArrow.Source = ImageSource.FromFile("DoubleDownLeftArrow.png");
            Grid.SetRow(imgCenter, 2);
            Grid.SetRow(imgUpArrow, 1);
            Grid.SetRow(imgDownArrow, 3);
            Grid.SetRow(imgLeftArrow, 2);
            Grid.SetRow(imgRightArrow, 2);
            Grid.SetRow(imgUpRightArrow, 1);
            Grid.SetRow(imgDownRightArrow, 3);
            Grid.SetRow(imgUpLeftArrow, 1);
            Grid.SetRow(imgDownLeftArrow, 3);
            Grid.SetRow(imgDoubleUpArrow, 0);
            Grid.SetRow(imgDoubleDownArrow, 4);
            Grid.SetRow(imgDoubleLeftArrow, 2);
            Grid.SetRow(imgDoubleRightArrow, 2);
            Grid.SetRow(imgDoubleUpRightArrow, 0);
            Grid.SetRow(imgDoubleDownRightArrow, 4);
            Grid.SetRow(imgDoubleUpLeftArrow, 0);
            Grid.SetRow(imgDoubleDownLeftArrow, 4);
            Grid.SetColumn(imgCenter, 2);
            Grid.SetColumn(imgUpArrow, 2);
            Grid.SetColumn(imgDownArrow, 2);
            Grid.SetColumn(imgLeftArrow, 1);
            Grid.SetColumn(imgRightArrow, 3);
            Grid.SetColumn(imgUpRightArrow, 3);
            Grid.SetColumn(imgDownRightArrow, 3);
            Grid.SetColumn(imgUpLeftArrow, 1);
            Grid.SetColumn(imgDownLeftArrow, 1);
            Grid.SetColumn(imgDoubleUpArrow, 2);
            Grid.SetColumn(imgDoubleDownArrow, 2);
            Grid.SetColumn(imgDoubleLeftArrow, 0);
            Grid.SetColumn(imgDoubleRightArrow, 4);
            Grid.SetColumn(imgDoubleUpRightArrow, 4);
            Grid.SetColumn(imgDoubleDownRightArrow, 4);
            Grid.SetColumn(imgDoubleUpLeftArrow, 0);
            Grid.SetColumn(imgDoubleDownLeftArrow, 0);
            gArrows.Children.Add(imgCenter);
            gArrows.Children.Add(imgUpArrow);
            gArrows.Children.Add(imgDownArrow);
            gArrows.Children.Add(imgLeftArrow);
            gArrows.Children.Add(imgRightArrow);
            gArrows.Children.Add(imgUpRightArrow);
            gArrows.Children.Add(imgDownRightArrow);
            gArrows.Children.Add(imgUpLeftArrow);
            gArrows.Children.Add(imgDownLeftArrow);
            gArrows.Children.Add(imgDoubleUpArrow);
            gArrows.Children.Add(imgDoubleDownArrow);
            gArrows.Children.Add(imgDoubleLeftArrow);
            gArrows.Children.Add(imgDoubleRightArrow);
            gArrows.Children.Add(imgDoubleUpRightArrow);
            gArrows.Children.Add(imgDoubleDownRightArrow);
            gArrows.Children.Add(imgDoubleUpLeftArrow);
            gArrows.Children.Add(imgDoubleDownLeftArrow);
            imgMotionalSurroundBackground.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgCenter.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgUpArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgDownArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgLeftArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgRightArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgUpRightArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgDownRightArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgUpLeftArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgDownLeftArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgDoubleUpArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgDoubleDownArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgDoubleLeftArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgDoubleRightArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgDoubleUpRightArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgDoubleDownRightArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgDoubleUpLeftArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgDoubleDownLeftArrow.HorizontalOptions = LayoutOptions.FillAndExpand;
            imgMotionalSurroundBackground.VerticalOptions = LayoutOptions.FillAndExpand;
            imgCenter.VerticalOptions = LayoutOptions.FillAndExpand;
            imgUpArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgDownArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgLeftArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgRightArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgUpRightArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgDownRightArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgUpLeftArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgDownLeftArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgDoubleUpArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgDoubleDownArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgDoubleLeftArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgDoubleRightArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgDoubleUpRightArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgDoubleDownRightArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgDoubleUpLeftArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgDoubleDownLeftArrow.VerticalOptions = LayoutOptions.FillAndExpand;
            imgMotionalSurroundBackground.Aspect = Aspect.Fill;
            imgCenter.Aspect = Aspect.Fill;
            imgUpArrow.Aspect = Aspect.Fill;
            imgDownArrow.Aspect = Aspect.Fill;
            imgLeftArrow.Aspect = Aspect.Fill;
            imgRightArrow.Aspect = Aspect.Fill;
            imgUpRightArrow.Aspect = Aspect.Fill;
            imgDownRightArrow.Aspect = Aspect.Fill;
            imgUpLeftArrow.Aspect = Aspect.Fill;
            imgDownLeftArrow.Aspect = Aspect.Fill;
            imgDoubleUpArrow.Aspect = Aspect.Fill;
            imgDoubleDownArrow.Aspect = Aspect.Fill;
            imgDoubleLeftArrow.Aspect = Aspect.Fill;
            imgDoubleRightArrow.Aspect = Aspect.Fill;
            imgDoubleUpRightArrow.Aspect = Aspect.Fill;
            imgDoubleDownRightArrow.Aspect = Aspect.Fill;
            imgDoubleUpLeftArrow.Aspect = Aspect.Fill;
            imgDoubleDownLeftArrow.Aspect = Aspect.Fill;
            imgCenter.Row = 2;
            imgUpArrow.Row = 1;
            imgDownArrow.Row = 3;
            imgLeftArrow.Row = 2;
            imgRightArrow.Row = 2;
            imgUpRightArrow.Row = 1;
            imgDownRightArrow.Row = 3;
            imgUpLeftArrow.Row = 1;
            imgDownLeftArrow.Row = 3;
            imgDoubleUpArrow.Row = 0;
            imgDoubleDownArrow.Row = 4;
            imgDoubleLeftArrow.Row = 2;
            imgDoubleRightArrow.Row = 2;
            imgDoubleUpRightArrow.Row = 0;
            imgDoubleDownRightArrow.Row = 4;
            imgDoubleUpLeftArrow.Row = 0;
            imgDoubleDownLeftArrow.Row = 4;
            imgCenter.Column = 2;
            imgUpArrow.Column = 2;
            imgDownArrow.Column = 2;
            imgLeftArrow.Column = 1;
            imgRightArrow.Column = 3;
            imgUpRightArrow.Column = 3;
            imgDownRightArrow.Column = 3;
            imgUpLeftArrow.Column = 1;
            imgDownLeftArrow.Column = 1;
            imgDoubleUpArrow.Column = 2;
            imgDoubleDownArrow.Column = 2;
            imgDoubleLeftArrow.Column = 0;
            imgDoubleRightArrow.Column = 4;
            imgDoubleUpRightArrow.Column = 4;
            imgDoubleDownRightArrow.Column = 4;
            imgDoubleUpLeftArrow.Column = 0;
            imgDoubleDownLeftArrow.Column = 0;

            btnCenter = new Button();
            btnUpArrow = new Button();
            btnDownArrow = new Button();
            btnLeftArrow = new Button();
            btnRightArrow = new Button();
            btnUpRightArrow = new Button();
            btnDownRightArrow = new Button();
            btnUpLeftArrow = new Button();
            btnDownLeftArrow = new Button();
            btnDoubleUpArrow = new Button();
            btnDoubleDownArrow = new Button();
            btnDoubleLeftArrow = new Button();
            btnDoubleRightArrow = new Button();
            btnDoubleUpRightArrow = new Button();
            btnDoubleDownRightArrow = new Button();
            btnDoubleUpLeftArrow = new Button();
            btnDoubleDownLeftArrow = new Button();
            btnCenter.Tag = Direction.CENTER;
            btnUpArrow.Tag = Direction.UP;
            btnDownArrow.Tag = Direction.DOWN;
            btnLeftArrow.Tag = Direction.LEFT;
            btnRightArrow.Tag = Direction.RIGHT;
            btnUpRightArrow.Tag = Direction.UPRIGHT;
            btnDownRightArrow.Tag = Direction.DOWNRIGHT;
            btnUpLeftArrow.Tag = Direction.UPLEFT;
            btnDownLeftArrow.Tag = Direction.DOWNLEFT;
            btnDoubleUpArrow.Tag = Direction.DOUBLE_UP;
            btnDoubleDownArrow.Tag = Direction.DOUBLE_DOWN;
            btnDoubleLeftArrow.Tag = Direction.DOUBLE_LEFT;
            btnDoubleRightArrow.Tag = Direction.DOUBLE_RIGHT;
            btnDoubleUpRightArrow.Tag = Direction.DOUBLE_UPRIGHT;
            btnDoubleDownRightArrow.Tag = Direction.DOUBLE_DOWNRIGHT;
            btnDoubleUpLeftArrow.Tag = Direction.DOUBLE_UPLEFT;
            btnDoubleDownLeftArrow.Tag = Direction.DOUBLE_DOWNLEFT;
            Grid.SetRow(btnCenter, 2);
            Grid.SetRow(btnUpArrow, 1);
            Grid.SetRow(btnDownArrow, 3);
            Grid.SetRow(btnLeftArrow, 2);
            Grid.SetRow(btnRightArrow, 2);
            Grid.SetRow(btnUpRightArrow, 1);
            Grid.SetRow(btnDownRightArrow, 3);
            Grid.SetRow(btnUpLeftArrow, 1);
            Grid.SetRow(btnDownLeftArrow, 3);
            Grid.SetRow(btnDoubleUpArrow, 0);
            Grid.SetRow(btnDoubleDownArrow, 4);
            Grid.SetRow(btnDoubleLeftArrow, 2);
            Grid.SetRow(btnDoubleRightArrow, 2);
            Grid.SetRow(btnDoubleUpRightArrow, 0);
            Grid.SetRow(btnDoubleDownRightArrow, 4);
            Grid.SetRow(btnDoubleUpLeftArrow, 0);
            Grid.SetRow(btnDoubleDownLeftArrow, 4);
            Grid.SetColumn(btnCenter, 2);
            Grid.SetColumn(btnUpArrow, 2);
            Grid.SetColumn(btnDownArrow, 2);
            Grid.SetColumn(btnLeftArrow, 1);
            Grid.SetColumn(btnRightArrow, 3);
            Grid.SetColumn(btnUpRightArrow, 3);
            Grid.SetColumn(btnDownRightArrow, 3);
            Grid.SetColumn(btnUpLeftArrow, 1);
            Grid.SetColumn(btnDownLeftArrow, 1);
            Grid.SetColumn(btnDoubleUpArrow, 2);
            Grid.SetColumn(btnDoubleDownArrow, 2);
            Grid.SetColumn(btnDoubleLeftArrow, 0);
            Grid.SetColumn(btnDoubleRightArrow, 4);
            Grid.SetColumn(btnDoubleUpRightArrow, 4);
            Grid.SetColumn(btnDoubleDownRightArrow, 4);
            Grid.SetColumn(btnDoubleUpLeftArrow, 0);
            Grid.SetColumn(btnDoubleDownLeftArrow, 0);
            btnCenter.Clicked += BtnArrow_Clicked;
            btnUpArrow.Clicked += BtnArrow_Clicked;
            btnDownArrow.Clicked += BtnArrow_Clicked;
            btnLeftArrow.Clicked += BtnArrow_Clicked;
            btnRightArrow.Clicked += BtnArrow_Clicked;
            btnUpRightArrow.Clicked += BtnArrow_Clicked;
            btnDownRightArrow.Clicked += BtnArrow_Clicked;
            btnUpLeftArrow.Clicked += BtnArrow_Clicked;
            btnDownLeftArrow.Clicked += BtnArrow_Clicked;
            btnDoubleUpArrow.Clicked += BtnArrow_Clicked;
            btnDoubleDownArrow.Clicked += BtnArrow_Clicked;
            btnDoubleLeftArrow.Clicked += BtnArrow_Clicked;
            btnDoubleRightArrow.Clicked += BtnArrow_Clicked;
            btnDoubleUpRightArrow.Clicked += BtnArrow_Clicked;
            btnDoubleDownRightArrow.Clicked += BtnArrow_Clicked;
            btnDoubleUpLeftArrow.Clicked += BtnArrow_Clicked;
            btnDoubleDownLeftArrow.Clicked += BtnArrow_Clicked;
            gArrows.Children.Add(btnCenter);
            gArrows.Children.Add(btnUpArrow);
            gArrows.Children.Add(btnDownArrow);
            gArrows.Children.Add(btnLeftArrow);
            gArrows.Children.Add(btnRightArrow);
            gArrows.Children.Add(btnUpRightArrow);
            gArrows.Children.Add(btnDownRightArrow);
            gArrows.Children.Add(btnUpLeftArrow);
            gArrows.Children.Add(btnDownLeftArrow);
            gArrows.Children.Add(btnDoubleUpArrow);
            gArrows.Children.Add(btnDoubleDownArrow);
            gArrows.Children.Add(btnDoubleLeftArrow);
            gArrows.Children.Add(btnDoubleRightArrow);
            gArrows.Children.Add(btnDoubleUpRightArrow);
            gArrows.Children.Add(btnDoubleDownRightArrow);
            gArrows.Children.Add(btnDoubleUpLeftArrow);
            gArrows.Children.Add(btnDoubleDownLeftArrow);

            // Assemble MotionalSurroundStackLayout --------------------------------------------------------------

            Grid.SetRow(gMotionalSurroundBackground, 0);
            Grid.SetColumn(gMotionalSurroundBackground, 0);
            Grid.SetRow(gArrows, 0);
            Grid.SetColumn(gArrows, 0);
            Grid.SetRow(gMotionalSurroundLabelsArea, 0);
            Grid.SetColumn(gMotionalSurroundLabelsArea, 0);
            gMotionalSurroundBackground.Margin = new Thickness(0);
            gArrows.Margin = new Thickness(0);
            gMotionalSurroundLabelsArea.HorizontalOptions = LayoutOptions.FillAndExpand;
            gMotionalSurroundLabelsArea.VerticalOptions = LayoutOptions.FillAndExpand;
            gMotionalSurroundLabelsArea.Margin = new Thickness(50);
            gMotionalSurroundBackground.Children.Add(imgMotionalSurroundBackground);
            gMotionalSurroundMainArea.Children.Add(gMotionalSurroundBackground);
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

            //gParts.BackgroundColor = Color.Black;
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
            //commonState.reactToMidiInAndTimerTick = CommonState.ReactToMidiInAndTimerTick.SURROUND;

            if (commonState == null)
            {
                // If commonState is not initialized, we have no business here, go back!
                MotionalSurround_StackLayout.IsVisible = false;
                ShowLibrarianPage();
            }
            else if (commonState.studioSet == null)
            {
                // StudioSet set has not been read, thus we have no Motional Surround data. 
                // Start by creating the studioSet object:
                commonState.studioSet = new StudioSet();

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
                    mslPart[currentMotionalSurroundPart].Horizontal = commonState.studioSet.PartMotionalSurround[currentMotionalSurroundPart].LR;
                    mslPart[currentMotionalSurroundPart].Vertical = (byte)(127 - commonState.studioSet.PartMotionalSurround[currentMotionalSurroundPart].FB);
                    mslPart[currentMotionalSurroundPart].Plot(gMotionalSurroundLabelsArea.Width, gMotionalSurroundLabelsArea.Height);
                    mslPart[currentMotionalSurroundPart].IsVisible = msePart[currentMotionalSurroundPart].Switch.LSSwitch.IsToggled;
                    mslPart[currentMotionalSurroundPart].Text = msePart[currentMotionalSurroundPart].Editor.Text;
                    mslPart[currentMotionalSurroundPart].MinimumWidthRequest = 20 + mslPart[currentMotionalSurroundPart].Text.Length * 10;
                    mslPart[currentMotionalSurroundPart].WidthRequest = 20 + mslPart[currentMotionalSurroundPart].Text.Length * 10;
                }
                mslPart[currentMotionalSurroundPart].Horizontal = commonState.studioSet.MotionalSurround.ExtPartLR;
                mslPart[currentMotionalSurroundPart].Vertical = (byte)(127 -commonState.studioSet.MotionalSurround.ExtPartFB);
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
            //if (commonState.reactToMidiInAndTimerTick != CommonState.ReactToMidiInAndTimerTick.SURROUND)
            //{
            //    return;
            //}
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
                bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
                commonState.midi.SendSystemExclusive(bytes);
            }
            else
            {
                byte part = (byte)(0x20 + (byte)currentMotionalSurroundPart);
                address = new byte[] { 0x18, 0x00, part, 0x44 };
                value = new byte[] { mslPart[currentMotionalSurroundPart].Horizontal };
                bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
                commonState.midi.SendSystemExclusive(bytes);
                address = new byte[] { 0x18, 0x00, part, 0x46 };
                value = new byte[] { (byte)(0x7f - mslPart[currentMotionalSurroundPart].Vertical) };
                bytes = commonState.midi.SystemExclusiveDT1Message(address, value);
                commonState.midi.SendSystemExclusive(bytes);
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

        private void BtnArrow_Clicked(object sender, EventArgs e)
        {
            mslPart[currentMotionalSurroundPart].Step((Direction)((Button)sender).Tag,
                gMotionalSurroundLabelsArea.Width, gMotionalSurroundLabelsArea.Height);
            SendCoordinatesToIntegra7();
        }

        private void BtnMotionalSurroundReturn_Clicked(object sender, EventArgs e)
        {
            MotionalSurround_StackLayout.IsVisible = false;
            ShowLibrarianPage();
        }
    }
}
