using Integra_7_Xamarin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integra_7_Xamarin
{
    public partial class MainPage : ContentPage
    {
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
            //StackLayout mainStackLayout = this.FindByName<StackLayout>("MainStackLayout");
            StackLayout mainStackLayout = this.FindByName<StackLayout>("MainStackLayout");
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
