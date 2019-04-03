using INTEGRA_7_Xamarin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// The mainStackLayout is dynamically created at startup, by Init() 
        /// and is the parent of the different pages, PleaseWait, Librarian,
        /// Favorites, Tone edit, Studio set edit and the Motional surround editor.
        /// All the pages are created at first use, except for the PleaseWait page,
        /// that is needed to be created at app startup and shown while finding
        /// a MIDI connection to the INTEGRA-7.
        /// </summary>
        StackLayout mainStackLayout { get; set; }
        public UIHandler uIHandler;
        private static MainPage MainPage_Portable;
        public object MainPage_Device { get; set; }
        public object[] platform_specific { get; set; }

        public interface IEventHandler
        {
            void GlobalHandler(object sender, EventArgs e);
        }
        
        public MainPage()
        {
            InitializeComponent();
            MainPage_Portable = this;
            Init();
        }

        public void Init()
        {
            mainStackLayout = new StackLayout();
            mainStackLayout.VerticalOptions = LayoutOptions.Fill;
            mainStackLayout.HorizontalOptions = LayoutOptions.Fill;
            mainStackLayout.Margin = 0;
            mainStackLayout.Padding = 0;
            mainStackLayout.Spacing = 0;
            mainStackLayout.MinimumWidthRequest = 1;
            mainStackLayout.MinimumHeightRequest = 1;
            mainStackLayout.SizeChanged += MainStackLayout_SizeChanged;
            mainStackLayout.Parent = this;
            mainStackLayout.IsVisible = true;
            this.SetValue(ContentProperty, mainStackLayout);
            uIHandler = new UIHandler(mainStackLayout, MainPage_Portable);
        }

        public static MainPage GetMainPage()
        {
            return MainPage_Portable;
        }

        public void SetDeviceSpecificMainPage(object mainPage)
        {
            MainPage_Device = mainPage;
        }

        private void MainStackLayout_SizeChanged(object sender, EventArgs e)
        {
            uIHandler.SetFontSizes(uIHandler.mainStackLayout);
        }

        public void SaveLocalValue(String Key, Object Value)
        {
            Object dummy;
            if (Application.Current.Properties.TryGetValue(Key, out dummy))
            {
                Application.Current.Properties.Remove(Key);
            }
            Application.Current.Properties.Add(new KeyValuePair<String, Object>(Key, Value));
        }

        public Object LoadLocalValue(String Key)
        {
            Object result = null;
            if (Application.Current.Properties.TryGetValue(Key, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
