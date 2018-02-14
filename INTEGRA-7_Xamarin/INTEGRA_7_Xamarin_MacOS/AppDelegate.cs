﻿using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;
using AppKit;
using Foundation;
using INTEGRA_7_Xamarin;
using CoreMidi;

[assembly: Dependency(typeof(IMidi))]
namespace INTEGRA_7_Xamarin_MacOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        NSWindow mainPage_MacOS;
        private Picker OutputSelector;
        private Picker InputSelector;
        public MIDI midi;
        public INTEGRA_7_Xamarin.MainPage mainPage = null;

        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;
            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
            mainPage_MacOS = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            mainPage_MacOS.Title = "the title";
            mainPage_MacOS.TitleVisibility = NSWindowTitleVisibility.Hidden;
        }

        public override NSWindow MainWindow
        {
            get { return mainPage_MacOS; }
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
            Forms.Init();
            LoadApplication(new INTEGRA_7_Xamarin.App());
            mainPage = INTEGRA_7_Xamarin.MainPage.GetMainPage();
            mainPage.uIHandler.DrawPage();
 
            // We need invisible ComboBoxes to hold settings from the
            // corresponding Pickers in the Xamarin code.
            OutputSelector = mainPage.uIHandler.Librarian_midiOutputDevice;
            InputSelector = mainPage.uIHandler.Librarian_midiInputDevice;
            //midi = new MIDI(mainPage, OutputSelector, InputSelector, /*Dispatcher,*/ 0, 0);
            //midi.Init("INTEGRA-7");

            base.DidFinishLaunching(notification);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
