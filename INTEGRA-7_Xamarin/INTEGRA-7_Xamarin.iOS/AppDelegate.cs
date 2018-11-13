using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
//using AppKit;
using Integra7Random_Xamarin.iOS;

[assembly: Dependency(typeof(MIDI))]
namespace Integra7Random_Xamarin.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        // For accessing Integra7Random_Xamarin.MainPage from IOS:
        private static Integra7Random_Xamarin.MainPage mainPage;
        private Picker OutputSelector;
        private Picker InputSelector;
        public MIDI midi;

        // Main window:
        //static UIWindow window;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            // Get Integra7Random_Xamarin.MainPage:
            mainPage = Integra7Random_Xamarin.MainPage.GetMainPage();
            UIHandler.appType = UIHandler._appType.IOS;
            mainPage.uIHandler.DrawLibrarianPage();
			
            // We need invisible ComboBoxes to hold settings from the
            // corresponding Pickers in the Xamarin code.
            OutputSelector = mainPage.uIHandler.Librarian_midiOutputDevice;
            InputSelector = mainPage.uIHandler.Librarian_midiInputDevice;
            //midi = new MIDI(this, OutputSelector, InputSelector, Dispatcher, 0, 0);
            //midi.Init("INTEGRA-7");
            mainPage.uIHandler.ShowLibrarianPage();
            return base.FinishedLaunching(app, options);
        }
    }
}
