using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;
using AppKit;
using Foundation;
using Integra_7_Xamarin;
using Integra_7_Xamarin_MacOS;
//using CoreMidi;

[assembly: Dependency(typeof(MIDI))]

namespace Integra_7_Xamarin_MacOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        NSWindow mainPage_MacOS;
        private Picker OutputSelector;
        private Picker InputSelector;
        public Integra_7_Xamarin.MainPage MainPage_Portable { get; set; }

        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;
            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
            mainPage_MacOS = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            mainPage_MacOS.Title = "Roland INTEGRA-7 Librarian and Editor";
        }

        public override NSWindow MainWindow
        {
            get { return mainPage_MacOS; }
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
            Forms.Init();
            LoadApplication(new Integra_7_Xamarin.App());

            UIHandler.appType = UIHandler._appType.MacOS;
            MainPage_Portable = Integra_7_Xamarin.MainPage.GetMainPage();
            MainPage_Portable.uIHandler.DrawLibrarianPage();
 
            // We need invisible ComboBoxes to hold settings from the
            // corresponding Pickers in the Xamarin code.
            OutputSelector = MainPage_Portable.uIHandler.Librarian_midiOutputDevice;
            InputSelector = MainPage_Portable.uIHandler.Librarian_midiInputDevice;

            MainPage_Portable.SetDeviceSpecificMainPage(this);

            MainPage_Portable.uIHandler.commonState.midi.Init(MainPage_Portable, "INTEGRA-7", OutputSelector, InputSelector, null, 0, 0);

            MainPage_Portable.uIHandler.ShowLibrarianPage();

            base.DidFinishLaunching(notification);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
