using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public class ComboBox : Picker
    {
        public Boolean Visibility { get { return (Boolean)IsVisible; } set { IsVisible = value; } }
        public Object Tag { get; set; }
        public String Name { get; set; }
    }

    public static class Visibility
    {
        public static Boolean Visible { get { return true; } }
        public static Boolean Collapsed { get { return false; } }
    }

    public class ComboBoxItem : Object
    {
        public String Content { get; set; } = "";
        public static implicit operator String(ComboBoxItem v) { return v.Content; }
        public override String ToString()
        {
            return Content;
        }
    }

    // Mapping controls for compatibility with Xamarin:
    public class TextBlock : Label { }

    // My conveniance controls:
    public enum _orientation
    {
        HORIZONTAL,
        VERTICAL,
    }

    public enum _labelPosition
    {
        BEFORE,
        AFTER,
    }

    public class BorderThicknesSettings
    {
        public Int32 Size { get; set; }

        public BorderThicknesSettings()
        {
            this.Size = 1;
        }

        public BorderThicknesSettings(Int32 Size)
        {
            this.Size = Size;
        }
    }

    public class MyLabel : Grid
    {
        public String Text { get { return Label.Text; } set { Label.Text = value; } }
        public Button Label;

        public MyLabel()
        {
            myLabel("");
        }

        public MyLabel(String LabelText)
        {
            myLabel(LabelText);
        }

        private void myLabel(String text)
        {
            this.Label = new Button();
            this.Label.Text = text;
            this.Label.Margin = new Thickness(0, 0, 0, 0);
            this.Label.BorderWidth = 0;
            this.Label.BackgroundColor = UIHandler.colorSettings.Background;
            this.Label.BorderWidth = 0;
            this.Children.Add((new GridRow(0, new View[] { this.Label })));

        }
    }

    public class TextBox : Editor
    {
        public Object Tag { get; set; }
        public String Name { get; set; }
        //public Editor Editor = new Editor();

        //public new Boolean IsEnabled { get { return Editor.IsEnabled; } set { Editor.IsEnabled = value; } }
    }

    public class CheckBox : Xamarin.Forms.Grid
    {
        public Boolean IsChecked { get { return CBSwitch.IsToggled; } set { CBSwitch.IsToggled = value; } }
        public String Name { get; set; }
        public Object Tag { get; set; }
        public String Content { get { return CBLabel.Text; } set { CBLabel.Text = value; } }

        public Switch CBSwitch { get; set; }
        public Label CBLabel { get; set; }

        public CheckBox()
        {
            MinimumHeightRequest = UIHandler.minimumHeightRequest;
            CBSwitch = new Switch();
            CBLabel = new Label();
            CBSwitch.VerticalOptions = LayoutOptions.FillAndExpand;
            CBSwitch.MinimumWidthRequest = 100;
            CBSwitch.MinimumHeightRequest = 14;
            CBLabel.WidthRequest = 1;
            CBLabel.HeightRequest = 1;

            CBLabel.BackgroundColor = UIHandler.colorSettings.Background;
            CBLabel.TextColor = UIHandler.colorSettings.Text;
            Children.Add(new GridRow(0, new View[] { CBLabel, CBSwitch }));
        }

        //public CheckBox(Grid g)
        //{
        //    MinimumHeightRequest = UIHandler.minimumHeightRequest;
        //    CBSwitch = new Switch();
        //    CBLabel = new Label();
        //    CBSwitch.VerticalOptions = LayoutOptions.FillAndExpand;
        //    CBSwitch.MinimumWidthRequest = 100;
        //    CBSwitch.MinimumHeightRequest = 14;
        //    CBLabel.WidthRequest = 1;
        //    CBLabel.HeightRequest = 1;

        //    CBLabel.BackgroundColor = UIHandler.colorSettings.Background;
        //    CBLabel.TextColor = UIHandler.colorSettings.Text;
        //    GridRow.CreateRow(this, 0, new View[] { CBLabel, CBSwitch });
        //}

        //public static implicit operator CheckBox(Grid g)
        //{
        //    return new CheckBox(g);
        //}

        //public static implicit operator Grid(CheckBox v)
        //{
        //    return new Grid();
        //}
    }

    public class Button : Xamarin.Forms.Button
    {
        public String Content { get { return Text; } set { Text = value; } }
        public Object Tag { get; set; }

        public Button()
        {
            this.BorderWidth = 0;
            this.CornerRadius = 6;
        }
    }

    public class PianoKey : Xamarin.Forms.Button
    {
        public String Content { get { return Text; } set { Text = value; } }
        public Boolean WhiteKey { get; set; }

        public PianoKey(Boolean KeyColor)
        {
            this.WhiteKey = KeyColor;
            this.BorderWidth = 0;
            this.CornerRadius = 6;
        }
    }

    public class FavoritesButton : Xamarin.Forms.Button
    {
        public String Content { get { return Text; } set { Text = value; } }

        public FavoritesButton()
        {
            this.BorderWidth = 0;
            this.CornerRadius = 6;
        }
    }

    public class Grid : Xamarin.Forms.Grid 
    {
        public byte IsPianoGrid { get; set; }

        public Grid(byte IsPianoGrid = 0)
        {
            this.IsPianoGrid = IsPianoGrid;
        }
    }

    public class LabeledText : Grid
    {
        //public Grid TheGrid { get; set; }
        public _orientation Orientation { get; set; }
        public _labelPosition LabelPosition { get; set; }
        public Button Label { get; set; }
        public Button text { get; set; }
        public String Text { get { return text.Text; } set { text.Text = value; } }

        public LabeledText(String LabelText, String Text, byte[] Sizes = null)
        {
            labeledText(LabelText, Text, _orientation.HORIZONTAL, _labelPosition.BEFORE, Sizes);
        }

        public LabeledText(String LabelText, String Text, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            labeledText(LabelText, Text, Orientation, LabelPosition, Sizes);
        }

        private void labeledText(String LabelText, String Text, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            this.Orientation = Orientation;
            this.LabelPosition = LabelPosition;
            this.Label = new Button();
            //this.Label.IsEnabled = false;
            this.Label.Text = LabelText;
            this.Label.Margin = new Thickness(0, 0, 0, 0);
            this.Label.BackgroundColor = UIHandler.colorSettings.Background;
            this.Label.BorderWidth = 0;
            this.text = new Button();
            //this.Text.IsEnabled = false;
            this.text.Text = Text;
            this.Text = Text;
            this.text.Margin = new Thickness(0, 0, 0, 0);
            this.text.BackgroundColor = UIHandler.colorSettings.Background;
            this.text.BorderWidth = 0;
            byte[] sizes;
            if (Sizes == null || Sizes.Count() != 2)
            {
                sizes = new byte[] { 1, 1 };
            }
            else
            {
                sizes = Sizes;
            }

            this.text.VerticalOptions = LayoutOptions.FillAndExpand;
            this.Label.VerticalOptions = LayoutOptions.FillAndExpand;
            this.VerticalOptions = LayoutOptions.FillAndExpand;
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.Margin = new Thickness(0);
            this.Padding = new Thickness(0);
            if (Orientation == _orientation.HORIZONTAL)
            {

                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.text.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Label, this.text }, sizes, true)));
                }
                else
                {
                    this.Label.HorizontalOptions = LayoutOptions.Start;
                    this.text.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.text, this.Label }, sizes, true)));
                }
            }
            else
            {
                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.Start;
                    this.text.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label }, sizes, true)));
                    this.Children.Add((new GridRow(1, new View[] { this.text }, sizes, true)));
                }
                else
                {
                    this.text.HorizontalOptions = LayoutOptions.Start;
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.text }, sizes, true)));
                    this.Children.Add((new GridRow(1, new View[] { this.Label }, sizes, true)));
                }
            }
            this.Children[0].Margin = new Thickness(0);
            text.Margin = new Thickness(0);
            Label.Margin = new Thickness(0);
        }
    }

    public class LabeledPicker : Grid
    {
        //public Grid TheGrid { get; set; }
        public _orientation Orientation { get; set; }
        public _labelPosition LabelPosition { get; set; }
        public Label Label { get; set; }
        public Picker Picker { get; set; }

        public LabeledPicker(String LabelText)
        {
            labeledPicker(LabelText, null, 0, _orientation.HORIZONTAL, _labelPosition.BEFORE, null);
        }

        public LabeledPicker(String LabelText, Picker Picker = null, byte[] Sizes = null)
        {
            labeledPicker(LabelText, Picker, 0, _orientation.HORIZONTAL, _labelPosition.BEFORE, Sizes);
        }

        public LabeledPicker(String LabelText, Picker Picker = null, 
            Int32 SelectedIndex = 0, _orientation Orientation = _orientation.HORIZONTAL, 
            _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            labeledPicker(LabelText, Picker, SelectedIndex, Orientation, LabelPosition, Sizes);
        }

        private void labeledPicker(String LabelText, Picker Picker = null, 
            Int32 SelectedIndex = 0, _orientation Orientation = 
            _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            this.Orientation = Orientation;
            this.LabelPosition = LabelPosition;
            this.Label = new Label();
            this.Label.Text = LabelText;
            if (Picker == null)
            {
                this.Picker = new Picker();
            }
            else
            {
                this.Picker = Picker;
            }
            byte[] sizes;
            if (Sizes == null || Sizes.Count() != 2)
            {
                sizes = new byte[] { 1, 1 };
            }
            else
            {
                sizes = Sizes;
            }

            this.Picker.VerticalOptions = LayoutOptions.FillAndExpand;
            this.Label.VerticalOptions = LayoutOptions.FillAndExpand;
            if (Orientation == _orientation.HORIZONTAL)
            {

                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label, this.Picker }, sizes, true)));
                }
                else
                {
                    this.Picker.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Picker, this.Label }, sizes, true)));
                }
            }
            else
            {
                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.Start;
                    this.Picker.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label }, null, true)));
                    this.Children.Add((new GridRow(1, new View[] { this.Picker }, null, true)));
                }
                else
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Picker.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Picker }, null, true)));
                    this.Children.Add((new GridRow(1, new View[] { this.Label }, null, true)));
                }
            }
            this.Picker.SelectedIndex = SelectedIndex;
        }
    }

    public class LabeledSwitch : Grid
    {
        public _orientation Orientation { get; set; }
        public _labelPosition LabelPosition { get; set; }
        public Label LSLabel { get; set; }
        public Switch LSSwitch { get; set; }
        public Boolean IsChecked { get { return LSSwitch.IsToggled; } set { LSSwitch.IsToggled = value; } }

        public LabeledSwitch(String LabelText)
        {
            labeledSwitch(LabelText, null, false, _orientation.HORIZONTAL, _labelPosition.BEFORE, null);
        }

        public LabeledSwitch(String LabelText, Switch Switch = null, byte[] Sizes = null)
        {
            labeledSwitch(LabelText, Switch, false, _orientation.HORIZONTAL, _labelPosition.BEFORE, Sizes);
        }

        public LabeledSwitch(String LabelText, Switch Switch = null, 
            Boolean IsSelected = false, _orientation Orientation = _orientation.HORIZONTAL, 
            _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            labeledSwitch(LabelText, Switch, IsSelected, Orientation, LabelPosition, Sizes);
        }

        private void labeledSwitch(String LabelText, Switch Switch = null, 
            Boolean IsSelected = false, _orientation Orientation = _orientation.HORIZONTAL, 
            _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            this.Orientation = Orientation;
            this.LabelPosition = LabelPosition;
            LSLabel = new Label();
            LSLabel.Text = LabelText;
            if (Switch == null)
            {
                this.LSSwitch = new Switch();
            }
            else
            {
                this.LSSwitch = Switch;
                throw new Exception("Switchen fanns visst! *************************************************************************");
            }
            this.LSSwitch.MinimumWidthRequest = 1;
            this.LSSwitch.MinimumHeightRequest = 1;
            byte[] sizes;
            if (Sizes == null || Sizes.Count() != 2)
            {
                sizes = new byte[] { 1, 1 };
            }
            else
            {
                sizes = Sizes;
            }

            this.LSSwitch.VerticalOptions = LayoutOptions.FillAndExpand;
            this.LSLabel.VerticalOptions = LayoutOptions.FillAndExpand;
            this.LSSwitch.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.LSLabel.HorizontalOptions = LayoutOptions.FillAndExpand;

            if (Orientation == _orientation.HORIZONTAL)
            {

                if (LabelPosition == _labelPosition.BEFORE)
                {
                    //LSLabel.HorizontalOptions = LayoutOptions.End;
                    Children.Add((new GridRow(0, new View[] { LSLabel, this.LSSwitch }, sizes, false)));
                }
                else
                {
                    //this.LSSwitch.HorizontalOptions = LayoutOptions.Start;
                    Children.Add((new GridRow(0, new View[] { this.LSSwitch, LSLabel }, sizes, false)));
                }
            }
            else
            {
                if (LabelPosition == _labelPosition.BEFORE)
                {
                    LSLabel.HorizontalOptions = LayoutOptions.Start;
                    this.LSSwitch.HorizontalOptions = LayoutOptions.End;
                    Children.Add((new GridRow(0, new View[] { LSLabel }, null, false)));
                    Children.Add((new GridRow(1, new View[] { this.LSSwitch }, null, false)));
                }
                else
                {
                    LSLabel.HorizontalOptions = LayoutOptions.End;
                    this.LSSwitch.HorizontalOptions = LayoutOptions.Start;
                    Children.Add((new GridRow(0, new View[] { this.LSSwitch }, null, false)));
                    Children.Add((new GridRow(1, new View[] { LSLabel }, null, false)));
                }
                LSLabel.HorizontalOptions = LayoutOptions.Center;
                LSLabel.Margin = new Thickness(0);
                this.LSSwitch.HorizontalOptions = LayoutOptions.Center;
                LSSwitch.Margin = new Thickness(0);
                this.Margin = new Thickness(0);
            }
            //this.Switch.IsToggled = IsSelected;
        }
    }

    public class Slider : Xamarin.Forms.Slider
    {
        public String Name { get; set; }
        public Object Tag { get; set; }
        public Double StepFrequency { get; set; }
        public new Double Value { get { return AdjustForStepFrequency(); } set { SetValue(value); } }

        //private Grid gridContainer;
        private Double currentValue = 0;
        private Boolean lockIt;

        public Slider()
        {
            MinimumWidthRequest = 1;
            MinimumHeightRequest = UIHandler.minimumHeightRequest;
            HeightRequest = UIHandler.minimumHeightRequest;
            WidthRequest = 10;
            StepFrequency = 1;
            Value = 0;
            lockIt = false;
            //gridContainer = new Grid();
            //gridContainer.BackgroundColor = Color.WhiteSmoke;
            //gridContainer.Parent = this.Parent;
            //gridContainer.Children.Add(this);
        }

        private void SetValue(Double value)
        {
            currentValue = value;
            SetValue(ValueProperty, currentValue);
        }

        private Double AdjustForStepFrequency()
        {
            if (!lockIt)
            {
                Double value = (Double)GetValue(ValueProperty);
                if (value > currentValue + StepFrequency)
                {
                    currentValue += StepFrequency;
                    lockIt = true;
                    SetValue(ValueProperty, currentValue);
                }
                else if (value < currentValue - StepFrequency)
                {
                    currentValue -= StepFrequency;
                    lockIt = true;
                    SetValue(ValueProperty, currentValue);
                }
            }
            else
            {
                lockIt = false;
            }
            return currentValue;
        }
    }

    public partial class Picker : Xamarin.Forms.Picker
    {
        //public Color TextColor { get; set; }

        //protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Picker> e)
        //{
        //    base.OnElementChanged(e);

        //    this.Control.TextAlignment = MonoTouch.UIKit.UITextAlignment.Center;

        //    this.Control.TextColor = UIColor.White;

        //    this.Control.BackgroundColor = UIColor.Clear;
        //    this.Control.BorderStyle = UITextBorderStyle.RoundedRect;
        //    this.Layer.BorderWidth = 1.0f;
        //    this.Layer.CornerRadius = 4.0f;
        //    this.Layer.MasksToBounds = true;
        //    this.Layer.BorderColor = UIColor.White.CGColor;
        //}
    }

    public class TaggedGrid : Xamarin.Forms.Grid
    {
        public Int32 Row { get; set; }
        public Int32 Column { get; set; }
    }

    public class TaggedImage: Xamarin.Forms.Image
    {
        public Int32 Row { get; set; }
        public Int32 Column { get; set; }
    }

    public class TouchableImage : Xamarin.Forms.Image
    {
        public object Tag { get; set; }
        
        public TouchableImage(EventHandler Handler, String ImageFile = null, object Tag = null, EventArgs e = null)
        {
            this.WidthRequest = 1000;
            this.HeightRequest = 1000;
            this.Tag = Tag;

            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Handler(this, e);
                }),
                NumberOfTapsRequired = 1
            });

            if (!String.IsNullOrEmpty(ImageFile))
            {
                this.Source = ImageFile;
            }
        }
    }

public class MotionalSurroundPartLabel : Button
    {
        public byte Horizontal { get; set; } // 0 - 127 => L64 - R63
        public byte Vertical { get; set; }   // 0 - 127 => B64 - F63

        public MotionalSurroundPartLabel(Int32 partNumber)
        {
            Horizontal = 63;
            Vertical = 63;
            if (partNumber == 17)
            {
                Text = "Ext";
            }
            else
            {
                Text = "Part " + partNumber.ToString();
            }
            BackgroundColor = UIHandler.colorSettings.MotionalSurroundPartLabelUnfocused;
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;
            MinimumWidthRequest = 200;
            WidthRequest = 200;
            MinimumHeightRequest = 30;
            HeightRequest = 30;
            IsVisible = true;
        }

        public void Step(Int32 direction, Double width, Double height)
        {
            byte hsteps;
            byte vsteps;

            switch (direction)
            {
                case 0:
                    // Up left
                    hsteps = 10;
                    vsteps = 10;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 1:
                    // Up left
                    hsteps = 5;
                    vsteps = 10;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 2:
                    // Up
                    vsteps = 10;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 3:
                    // Up right
                    hsteps = 5;
                    vsteps = 10;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)127;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 4:
                    // Up right
                    hsteps = 10;
                    vsteps = 10;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)127;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 5:
                    // Up left
                    hsteps = 10;
                    vsteps = 5;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 6:
                    // Up left
                    hsteps = 1;
                    vsteps = 1;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 7:
                    // Up
                    vsteps = 1;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 8:
                    // Up right
                    hsteps = 1;
                    vsteps = 1;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)127;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 9:
                    // Up right
                    hsteps = 10;
                    vsteps = 5;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)127;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 10:
                    // Left
                    hsteps = 10;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    break;
                case 11:
                    // Left
                    hsteps = 1;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    break;
                case 12:
                    // Center
                    Horizontal = 63;
                    Vertical = 63;
                    break;
                case 13:
                    // Right
                    hsteps = 1;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)127;
                    break;
                case 14:
                    // Right
                    hsteps = 10;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)127;
                    break;
                case 15:
                    // Down left
                    hsteps = 10;
                    vsteps = 5;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 16:
                    // Down left
                    hsteps = 1;
                    vsteps = 1;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 17:
                    // Down
                    vsteps = 1;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 18:
                    // Down right
                    hsteps = 1;
                    vsteps = 1;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)127;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 19:
                    // Down right
                    hsteps = 10;
                    vsteps = 5;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)127;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 20:
                    // Down left
                    hsteps = 10;
                    vsteps = 10;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 21:
                    // Down left
                    hsteps = 5;
                    vsteps = 10;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 22:
                    // Down
                    vsteps = 10;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 23:
                    // Down right
                    hsteps = 5;
                    vsteps = 10;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)127;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 24:
                    // Down right
                    hsteps = 10;
                    vsteps = 10;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)127;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
            }
            Plot(width, height);
        }

        public void Plot(Double width, Double height)
        {
            // Offset from upper left corner to center:
            Double hOffset = (Double)(width / 2.0);
            Double vOffset = (Double)(height / 2.0);

            // Position in terms of grid coordinates adjusted for I-7 position:
            Double left = 0.8 * width * (Double)Horizontal / (Double)127; // Horizontal varying from 0 to 127 gives left varying from 0.0 to grid width
            Double top = 0.8 * height * (Double)Vertical / (Double)127;   // Vertical varying from 0 to 127 gives top varying from 0.0 to grid height
            Double right = 0.8 * width - left;                            // Horizontal varying from 0 to 127 gives right varying from grid width to 0.0
            Double bottom = 0.8 * height - top;                           // Vertical varying from 0 to 127 gives right varying from grid height to 0.0

            // Sum of margin values are now twice the available space, adjust it_
            left -= hOffset;
            top -= vOffset;
            right -= hOffset;
            bottom -= vOffset;
            Margin = new Thickness(left, top, right, bottom);
        }
    }

    public class MotionalSurroundPartEditor : Grid
    {
        public LabeledSwitch Switch { get; set; }
        public Editor Editor { get; set; }
        public Int32 Tag { get; set; }

        public MotionalSurroundPartEditor(Int32 PartNumber)
        {
            String label = "Ext";
            if (PartNumber < 17)
            {
                label = "Part " + PartNumber.ToString();
            }
            Switch = new LabeledSwitch(label, null, false, _orientation.HORIZONTAL, _labelPosition.AFTER);
            Editor = new Editor();
            Editor.HorizontalOptions = LayoutOptions.FillAndExpand;
            Editor.VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            Children.Add(new GridRow(0, new View[] { Switch, Editor }, new byte[] { 2, 3 }));
        }
    }
}