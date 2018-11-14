using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Integra_7_Xamarin
{
    public interface IGenericHandler
    {
        void GenericHandler(object sender, object e);
    }

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

			MainPage = new Integra_7_Xamarin.MainPage();
		}

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
