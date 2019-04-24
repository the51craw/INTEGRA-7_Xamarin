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
using INTEGRA_7_Xamarin.UWP;
using Xamarin.Forms;
using INTEGRA_7_Xamarin;
using System.ServiceModel.Dispatcher;

[assembly: Dependency(typeof(MIDI))]

namespace INTEGRA_7_Xamarin.UWP
{
    public sealed partial class MainPage// : IDeviceDependent
    {
        // For accessing INTEGRA_7_Xamarin.MainPage from UWP:
        public INTEGRA_7_Xamarin.MainPage MainPage_Portable { get; set; }
        public MainPage MainPage_UWP { get; set; }

        // Invisible comboboxes used by MIDI class (will always have INTEGRA-7 selected):
        public MIDI midi;
        public Keyboard keyboard;
        //private Double x, y;

        public Windows.UI.Core.CoreDispatcher Dispatcher_UWP { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new INTEGRA_7_Xamarin.App());
            Init();
        }
        
        private async void Init()
        {
            // Get dispatcher:
            //Dispatcher_UWP = Dispatcher;

            // Get INTEGRA_7_Xamarin.MainPage:
            MainPage_Portable = INTEGRA_7_Xamarin.MainPage.GetMainPage();
            UIHandler.appType = UIHandler._appType.UWP;

            // Let portable know this MainPage:
            MainPage_Portable.MainPage_Device = this;

            // Create UI (function is in mainPage.uIHandler):
            //MainPage_Portable.uIHandler.DrawLibrarianPage();

            MainPage_Portable.uIHandler.ShowPleaseWaitPage(WaitingFor.CONNECTION, UIHandler.CurrentPage.LIBRARIAN, null);

            //MainPage_Portable.SetDeviceSpecificMainPage(this);

            //MainPage_Portable.uIHandler.commonState.midi.Init(MainPage_Portable, "INTEGRA-7", OutputSelector, InputSelector, (object)Dispatcher_UWP, 0, 0);
            //await MainPage_Portable.uIHandler.commonState.midi.CheckForVenderDriver();

            //// Create a MyFileIO object:
            ////MyFileIO fileIO = new MyFileIO();
            ////MainPage_Portable.uIHandler.myFileIO.SetMainPagePortable(MainPage_Portable);

            //// Always start by showing librarian:
            //MainPage_Portable.uIHandler.ShowLibrarianPage();
        }

        //public void InitMidi()
        //{
        //    //MainPage_Portable.uIHandler.commonState.Midi.Init(MainPage_Portable, "INTEGRA-7", (object)Dispatcher_UWP, 0, 0);
        //    MainPage_Portable.uIHandler.commonState.Midi.Init(MainPage_Portable, "INTEGRA-7", (object)Dispatcher_UWP, 0, 0);
        //}

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
