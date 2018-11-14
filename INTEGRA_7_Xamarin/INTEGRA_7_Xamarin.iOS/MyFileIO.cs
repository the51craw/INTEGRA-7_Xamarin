using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integra_7_Xamarin.iOS;
using Integra_7_Xamarin;
using Xamarin.Forms;
using System.Collections.ObjectModel;

//[assembly: Xamarin.Forms.Dependency(typeof(GenericHandlerInterface))]

[assembly: Dependency(typeof(MyFileIO))]

namespace Integra_7_Xamarin.iOS
{
    class MyFileIO : IMyFileIO, IGenericHandler
    {
        public Integra_7_Xamarin.MainPage MainPage_Portable { get; set; }

        public void GenericHandler(object sender, object e)
        {
            //if (mainPage.uIHandler.commonState.midi. midiOutPort == null)
            //{
            //    mainPage.midi.Init("INTEGRA-7");
            //}
        }

        public void SetMainPagePortable(Integra_7_Xamarin.MainPage mainPage)
        {
            MainPage_Portable = mainPage;
        }

        public String ReadFile(String filename)
        {
            try
            {
                using (StreamReader sr = File.OpenText(filename))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void LoadFavorites()
        {
            LoadFavoritesAsync();
        }

        private async void LoadFavoritesAsync()
        {
            String linesToUnpack = "";
            try
            {
                //FileOpenPicker openPicker = new FileOpenPicker();
                //openPicker.FileTypeFilter.Add(".fav");
                //StorageFile file = await openPicker.PickSingleFileAsync();
                //IList<String> lines = await FileIO.ReadLinesAsync(file);
                //if (lines != null && lines.Count() > 0)
                //{
                //    MainPage_Portable.uIHandler.Favorites_ocFolderList = new ObservableCollection<String>();
                //    foreach (String line in lines)
                //    {
                //        linesToUnpack += line + '\n';
                //    }
                //    linesToUnpack = linesToUnpack.TrimEnd('\n');
                //    if (linesToUnpack == "")
                //    {
                //        linesToUnpack = "Empty";
                //    }
                //    //    localSettings.Values["Favorites"] = linesToUnpack;
                //    //    UnpackFoldersWithFavoritesString(linesToUnpack);
                //    //    UpdateFoldersList();
                //}
            }
            catch
            {
                linesToUnpack = "Error";
            } //(Exception e) { }
            MainPage_Portable.uIHandler.Favorites_Restore(linesToUnpack);
        }
    }
}
