using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Integra_7_Xamarin.UWP;
using Xamarin.Forms;
using Integra_7_Xamarin;
using System.ServiceModel.Dispatcher;

[assembly: Xamarin.Forms.Dependency(typeof(GenericHandlerInterface))]

[assembly: Dependency(typeof(MIDI))]

namespace Integra_7_Xamarin.UWP
{
    public class GenericHandlerInterface: IGenericHandler
    {
        public MainPage mainPage { get; set; }

        public void GenericHandler(object sender, object e)
        {
            if (mainPage.midi.midiOutPort == null)
            {
                mainPage.midi.Init("INTEGRA-7");
            }
        }
    }

    public sealed partial class MainPage : IDeviceDependent
    {
        // For accessing Integra_7_Xamarin.MainPage from UWP:
        public Integra_7_Xamarin.MainPage MainPage_Portable { get; set; }
        public MainPage MainPage_UWP { get; set; }

        // Invisible comboboxes used by MIDI class (will always have INTEGRA-7 selected):
        private Picker OutputSelector;
        private Picker InputSelector;
        public MIDI midi;
        public Keyboard keyboard;
        //private Double x, y;

        // For accessing the genericHandlerInterface:
        GenericHandlerInterface genericHandlerInterface;
        public Windows.UI.Core.CoreDispatcher Dispatcher_UWP { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new Integra_7_Xamarin.App());
            Init();
        }
        
        private async void Init()
        {
            // Get dispatcher:
            //Dispatcher_UWP = Dispatcher;

            // Get Integra_7_Xamarin.MainPage:
            MainPage_Portable = Integra_7_Xamarin.MainPage.GetMainPage();
            UIHandler.appType = UIHandler._appType.UWP;

            // Get the generic handler (same way as done in Integra_7_Xamarin.UIHandler):
            genericHandlerInterface = (GenericHandlerInterface)DependencyService.Get<IGenericHandler>();

            // Let genericHandlerInterface know this MainPage:
            genericHandlerInterface.mainPage = this;

            // Let portable know this MainPage:
            MainPage_Portable.MainPage_Device = this;

            // Create UI (function is in mainPage.uIHandler):
            MainPage_Portable.uIHandler.DrawLibrarianPage();

            // We need ComboBoxes to hold settings from the
            // corresponding Pickers in the Xamarin code.
            OutputSelector = MainPage_Portable.uIHandler.Librarian_midiOutputDevice;
            // The input selector is never shown. When changing output, input follows.
            InputSelector = MainPage_Portable.uIHandler.Librarian_midiInputDevice;

            MainPage_Portable.uIHandler.ShowPleaseWaitPage(WaitingFor.MIDI);

            //MainPage_Portable.SetDeviceSpecificMainPage(this);

            //MainPage_Portable.uIHandler.commonState.midi.Init(MainPage_Portable, "INTEGRA-7", OutputSelector, InputSelector, (object)Dispatcher_UWP, 0, 0);
            //await MainPage_Portable.uIHandler.commonState.midi.CheckForVenderDriver();

            //// Create a MyFileIO object:
            ////MyFileIO fileIO = new MyFileIO();
            ////MainPage_Portable.uIHandler.myFileIO.SetMainPagePortable(MainPage_Portable);

            //// Always start by showing librarian:
            //MainPage_Portable.uIHandler.ShowLibrarianPage();
        }

        public void InitMidi()
        {
            MainPage_Portable.uIHandler.commonState.Midi.Init(MainPage_Portable, "INTEGRA-7", OutputSelector, InputSelector, (object)Dispatcher_UWP, 0, 0);
        }

        public void Waiting(Boolean on)
        {
            if (on)
            {
                Window.Current.CoreWindow.PointerCursor =
                    new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 1);
            }
            else
            {
                Window.Current.CoreWindow.PointerCursor =
                    new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
            }
        }

        public Windows.UI.Core.CoreDispatcher GetDispatcher()
        {
            return Dispatcher_UWP;
        }

        //public Windows.UI.Xaml.Controls.Image GetDeviceDependentImageUWP()
        //{
        //    return new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///MotionalSurround.png"));
        //}
    }
}
