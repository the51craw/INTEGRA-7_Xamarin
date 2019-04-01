using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integra_7_Xamarin
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
    public class TextBlock : Label
    {
        //public TextBlock()
        //{
        //    //this.IsEnabled = false;
        //}
    }

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
            //this.btn.IsEnabled = false;
            this.Label.Text = text;
            this.Label.Margin = new Thickness(0, 0, 0, 0);
            this.Label.BorderWidth = 0;
            this.Label.BackgroundColor = UIHandler.colorSettings.LabelBackground;
            this.Label.BorderWidth = 0;
            this.Children.Add((new GridRow(0, new View[] { this.Label })).Row);

        }
    }

    public class TextBox: Editor
    {
        public Object Tag { get; set; }
        public String Name { get; set; }
        Editor _editor = new Editor();

        public new Boolean IsEnabled { get { return _editor.IsEnabled; } set { _editor.IsEnabled = value; } }
    }

    public class CheckBox : Xamarin.Forms.Grid
    {
        //public new Boolean IsEnabled { get { return _switch.IsEnabled; } set { _switch.IsEnabled = value; } }
        public Boolean IsChecked { get { return CBSwitch.IsToggled; } set { CBSwitch.IsToggled = value; } }
        //public Boolean Toggled { get { return Switch.IsToggled; } set { Switch.IsToggled = value; } }
        public String Name { get; set; }
        public Object Tag { get; set; }
        public String Content { get { return CBLabel.Text; } set { CBLabel.Text = value; } }

        public Switch CBSwitch { get; set; }
        //public Grid Grid = new Grid();
        public Label CBLabel { get; set; }

        public CheckBox()
        {
            MinimumHeightRequest = UIHandler.minimumHeightRequest;
            CBSwitch = new Switch();
            CBLabel = new Label();
            CBSwitch.VerticalOptions = LayoutOptions.FillAndExpand;
            //Switch.BackgroundColor = new Color(255, 255, 255, 255);
            //Switch.OnColor = new Color(255, 255, 255, 255);
            //Switch.WidthRequest = 1;
            CBSwitch.MinimumWidthRequest = 100;
            //Switch.HeightRequest = 1;
            CBSwitch.MinimumHeightRequest = 14;
            CBLabel.WidthRequest = 1;
            CBLabel.HeightRequest = 1;

            CBLabel.BackgroundColor = UIHandler.colorSettings.Background;
            CBLabel.TextColor = UIHandler.colorSettings.Text;
            this.Children.Add(new GridRow(0, new View[] { CBLabel/*, Switch*/ }, null, true).Row);
        }


        //public static implicit operator Grid(CheckBox rhs)
        //{
        //    Grid grid = new Grid();
        //    grid.Children.Add(new GridRow(0, new View[] { rhs.Label, rhs }).Row);
        //    return grid;
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
            this.Label.Margin = new Thickness(0, 0, 2, 0);
            this.Label.BackgroundColor = UIHandler.colorSettings.LabelBackground;
            this.Label.BorderWidth = 0;
            this.text = new Button();
            //this.Text.IsEnabled = false;
            this.text.Text = Text;
            this.Text = Text;
            this.text.Margin = new Thickness(0, 0, 0, 0);
            this.text.BackgroundColor = UIHandler.colorSettings.LabelBackground;
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
                    this.Children.Add((new GridRow(0, new View[] { this.Label, this.text }, sizes, true)).Row);
                }
                else
                {
                    this.Label.HorizontalOptions = LayoutOptions.Start;
                    this.text.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.text, this.Label }, sizes, true)).Row);
                }
            }
            else
            {
                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.Start;
                    this.text.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label }, sizes, true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.text }, sizes, true)).Row);
                }
                else
                {
                    this.text.HorizontalOptions = LayoutOptions.Start;
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.text }, sizes, true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Label }, sizes, true)).Row);
                }
            }
        }
    }

    public class LabeledTextInput : Grid
    {
        //public Grid TheGrid { get; set; }
        public _orientation Orientation { get; set; }
        public _labelPosition LabelPosition { get; set; }
        public Button Label { get; set; }
        public Editor Editor { get; set; }

        public LabeledTextInput(String LabelText, byte[] Sizes = null)
        {
            labeledTextInput(LabelText, "", _orientation.HORIZONTAL, _labelPosition.BEFORE, Sizes);
        }

        public LabeledTextInput(String LabelText, String EditorText = "", _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            labeledTextInput(LabelText, EditorText, Orientation, LabelPosition, Sizes);
        }

        private void labeledTextInput(String LabelText, String EditorText = "", _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            this.Orientation = Orientation;
            this.LabelPosition = LabelPosition;
            this.Label = new Button();
            this.Label.Text = LabelText;
            //this.Label.IsEnabled = false;
            this.Label.Margin = new Thickness(0, 0, 2, 0);
            this.Label.BackgroundColor = UIHandler.colorSettings.LabelBackground;
            this.Label.BorderWidth = 0;
            this.Editor = new Editor();
            this.Editor.Text = EditorText;
            byte[] sizes;
            if (Sizes == null || Sizes.Count() != 2)
            {
                sizes = new byte[] { 1, 1 };
            }
            else
            {
                sizes = Sizes;
            }

            this.Editor.VerticalOptions = LayoutOptions.FillAndExpand;
            this.Label.VerticalOptions = LayoutOptions.FillAndExpand;
            if (Orientation == _orientation.HORIZONTAL)
            {

                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label, this.Editor }, sizes, true)).Row);
                }
                else
                {
                    this.Editor.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Editor, this.Label }, sizes, true)).Row);
                }
            }
            else
            {
                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.Start;
                    this.Editor.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label }, null,  true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Editor }, null, true)).Row);
                }
                else
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Editor.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Editor }, null, true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Label }, null, true)).Row);
                }
            }
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

        public LabeledPicker(String LabelText, Picker Picker = null, Int32 SelectedIndex = 0, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            labeledPicker(LabelText, Picker, SelectedIndex, Orientation, LabelPosition, Sizes);
        }

        private void labeledPicker(String LabelText, Picker Picker = null, Int32 SelectedIndex = 0, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
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
                    this.Children.Add((new GridRow(0, new View[] { this.Label, this.Picker }, sizes, true)).Row);
                }
                else
                {
                    this.Picker.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Picker, this.Label }, sizes, true)).Row);
                }
            }
            else
            {
                if (LabelPosition == _labelPosition.BEFORE)
                {
                    this.Label.HorizontalOptions = LayoutOptions.Start;
                    this.Picker.HorizontalOptions = LayoutOptions.End;
                    this.Children.Add((new GridRow(0, new View[] { this.Label }, null, true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Picker }, null, true)).Row);
                }
                else
                {
                    this.Label.HorizontalOptions = LayoutOptions.End;
                    this.Picker.HorizontalOptions = LayoutOptions.Start;
                    this.Children.Add((new GridRow(0, new View[] { this.Picker }, null, true)).Row);
                    this.Children.Add((new GridRow(1, new View[] { this.Label }, null, true)).Row);
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

        public LabeledSwitch(String LabelText, Switch Switch = null, Boolean IsSelected = false, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
        {
            labeledSwitch(LabelText, Switch, IsSelected, Orientation, LabelPosition, Sizes);
        }

        private void labeledSwitch(String LabelText, Switch Switch = null, Boolean IsSelected = false, _orientation Orientation = _orientation.HORIZONTAL, _labelPosition LabelPosition = _labelPosition.BEFORE, byte[] Sizes = null)
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

            if (Orientation == _orientation.HORIZONTAL)
            {

                if (LabelPosition == _labelPosition.BEFORE)
                {
                    LSLabel.HorizontalOptions = LayoutOptions.End;
                    Children.Add((new GridRow(0, new View[] { LSLabel, this.LSSwitch }, sizes, true)).Row);
                }
                else
                {
                    this.LSSwitch.HorizontalOptions = LayoutOptions.Start;
                    Children.Add((new GridRow(0, new View[] { this.LSSwitch, LSLabel }, sizes, true)).Row);
                }
                this.LSLabel.VerticalOptions = LayoutOptions.Center;
                this.LSSwitch.VerticalOptions = LayoutOptions.Center;
            }
            else
            {
                if (LabelPosition == _labelPosition.BEFORE)
                {
                    LSLabel.HorizontalOptions = LayoutOptions.Start;
                    this.LSSwitch.HorizontalOptions = LayoutOptions.End;
                    Children.Add((new GridRow(0, new View[] { LSLabel }, null, true)).Row);
                    Children.Add((new GridRow(1, new View[] { this.LSSwitch }, null, true)).Row);
                }
                else
                {
                    LSLabel.HorizontalOptions = LayoutOptions.End;
                    this.LSSwitch.HorizontalOptions = LayoutOptions.Start;
                    Children.Add((new GridRow(0, new View[] { this.LSSwitch }, null, true)).Row);
                    Children.Add((new GridRow(1, new View[] { LSLabel }, null, true)).Row);
                }
                LSLabel.HorizontalOptions = LayoutOptions.Center;
                this.LSSwitch.HorizontalOptions = LayoutOptions.Center;
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
                if (value > currentValue)
                {
                    currentValue += StepFrequency;
                    lockIt = true;
                    SetValue(ValueProperty, currentValue);
                }
                else if (value < currentValue)
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

public class MotionalSurroundPartLabel : /*Xamarin.Forms.*/Button
    {
        public byte Horizontal { get; set; } // 0 - 127 => L64 - R63
        public byte Vertical { get; set; }   // 0 - 127 => B64 - F63
        //public Int32 Tag { get; set; }

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
            BackgroundColor = Color.Yellow;
            //TextColor = Color.Yellow;
            //Focused += MotionalSurroundPartLabel_Focused;

            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;
            MinimumWidthRequest = 200;
            WidthRequest = 200;
            MinimumHeightRequest = 30;
            HeightRequest = 30;
            IsVisible = true;
        }

        private void MotionalSurroundPartLabel_Focused(object sender, FocusEventArgs e)
        {
            TextColor = Color.Black;
            IsEnabled = true;
        }

        public void Step(Int32 direction, Double width, Double height)
        {
            byte hsteps;
            byte vsteps;

            switch (direction)
            {
                case 0:
                    hsteps = 10;
                    vsteps = 10;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 1:
                    hsteps = 5;
                    vsteps = 10;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 2:
                    vsteps = 10;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 3:
                    hsteps = 5;
                    vsteps = 10;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)127;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 4:
                    hsteps = 10;
                    vsteps = 10;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)0;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 5:
                    hsteps = 10;
                    vsteps = 5;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 6:
                    hsteps = 1;
                    vsteps = 1;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 7:
                    vsteps = 1;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 8:
                    hsteps = 1;
                    vsteps = 1;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)0;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 9:
                    hsteps = 10;
                    vsteps = 5;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)0;
                    Vertical = Vertical > vsteps ? (byte)(Vertical - vsteps) : (byte)0;
                    break;
                case 10:
                    hsteps = 10;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    break;
                case 11:
                    hsteps = 1;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    break;
                case 12:
                    Horizontal = 63;
                    Vertical = 63;
                    break;
                case 13:
                    hsteps = 1;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)0;
                    break;
                case 14:
                    hsteps = 10;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)0;
                    break;
                case 15:
                    hsteps = 10;
                    vsteps = 5;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 16:
                    hsteps = 1;
                    vsteps = 1;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 17:
                    vsteps = 1;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 18:
                    hsteps = 1;
                    vsteps = 1;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 19:
                    hsteps = 10;
                    vsteps = 5;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 20:
                    hsteps = 10;
                    vsteps = 10;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 21:
                    hsteps = 5;
                    vsteps = 10;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 22:
                    hsteps = 1;
                    vsteps = 1;
                    Horizontal = Horizontal > hsteps ? (byte)(Horizontal - hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 23:
                    hsteps = 5;
                    vsteps = 10;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
                case 24:
                    hsteps = 10;
                    vsteps = 10;
                    Horizontal = Horizontal < 127 - hsteps ? (byte)(Horizontal + hsteps) : (byte)0;
                    Vertical = Vertical < 127 - vsteps ? (byte)(Vertical + vsteps) : (byte)127;
                    break;
            }
            Plot(width, height);
        }

        public void Plot(Double width, Double height)
        {
            byte hOffset = (byte)(Width / 2.0);
            byte vOffset = (byte)(Height / 2.0);

            Double left = width * Horizontal / 127;
            Double top = height * Vertical / 127;
            Double right = width - left;
            Double bottom = height - top;
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
            Children.Add(new GridRow(0, new View[] { Switch, Editor }, new byte[] { 2, 3 }).Row);
        }
    }
}