using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integra_7_Xamarin.UWP
{
    public class Keyboard
    {
        //public Xamarin.Forms.Image Librarian_Keyboard { get; set; }
        //private Xamarin.Forms.IPlatformElementConfiguration<Xamarin.Forms.PlatformConfiguration.Windows, Integra7Random_Xamarin.MainPage> librarian_Keyboard;

        public Keyboard(UIHandler uiHandler)
        {

            //Librarian_Keyboard = new Image { Aspect = Aspect.Fill };
            //Librarian_Keyboard.Source = ImageSource.FromFile("Keyboard.jpg");
            //Librarian_Keyboard.HeightRequest = 330;
            //Librarian_Keyboard.VerticalOptions = LayoutOptions.StartAndExpand;
            //Librarian_Keyboard.HorizontalOptions = LayoutOptions.CenterAndExpand;
            //Librarian_Keyboard = new Xamarin.Forms.Image();
            //librarian_Keyboard = Librarian_Keyboard.On<Image>();
            //uiHandler.mainStackLayout.Children.Add(Librarian_Keyboard);

            //uiHandler.mainStackLayout//
            //keyboard.PointerPressed += keyboard_PointerPressed;
            //keyboard.PointerReleased += keyboard_PointerReleased;
            //keyboard.PointerMoved += keyboard_PointerMoved;
        }

        //private void keyboard_PointerPressed(object sender, object e)
        //{
        //    //t.Trace("private void keyboard_PointerPressed (" + "object" + sender + ", " + "PointerRoutedEventArgs" + e + ", " + ")");
        //    PointerPoint mousePosition = e.GetCurrentPoint(keyboard);
        //    Note note = NoteFromMousePosition(mousePosition.Position.X, mousePosition.Position.Y);
        //    if (note.NoteNumber < 128)
        //    {
        //        currentNote = note.NoteNumber;
        //        commonState.midi.NoteOn(commonState.CurrentPart, note.NoteNumber, note.Velocity);
        //    }
        //}

        //private void keyboard_PointerReleased(object sender, object e)
        //{
        //    //t.Trace("private void keyboard_PointerReleased (" + "object" + sender + ", " + "PointerRoutedEventArgs" + e + ", " + ")");
        //    if (currentNote < 128)
        //    {
        //        commonState.midi.NoteOff(commonState.CurrentPart, currentNote);
        //        currentNote = 255;
        //    }
        //}

        //private void keyboard_PointerMoved(object sender, object e)
        //{
        //    //t.Trace("private void keyboard_PointerMoved (" + "object" + sender + ", " + "PointerRoutedEventArgs" + e + ", " + ")");
        //    if (currentNote < 128) // Do this only when a note is currently playing.
        //    {
        //        PointerPoint mousePosition = e.GetCurrentPoint(keyboard);
        //        Note note = NoteFromMousePosition(mousePosition.Position.X, mousePosition.Position.Y);
        //        if (note.NoteNumber != currentNote)
        //        {
        //            // Kill sounding note:
        //            commonState.midi.NoteOff(commonState.CurrentPart, currentNote);

        //            // Play next note:
        //            currentNote = note.NoteNumber;
        //            commonState.midi.NoteOn(commonState.CurrentPart, currentNote, note.Velocity);
        //        }
        //    }
        //}
    }
}
