using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.Storage.Streams;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Xamarin.Forms;
using INTEGRA_7_Xamarin.UWP;
using Windows.Storage.Pickers;
using Windows.Storage;

[assembly: Dependency(typeof(MyFileIO))]

namespace INTEGRA_7_Xamarin.UWP
{
    public class MyFileIO : IMyFileIO
    {
        public MyFileIO()
        {

        }

        public void GenericHandler(object sender, object e)
        {
            throw new NotImplementedException();
        }

        public String LoadFavorites()
        {
            return Load().Result;
        }

        public string ReadFile(string filename)
        {
            throw new NotImplementedException();
        }

        public async void SaveFileAsync(string content, string extension, string filename = "")
        {
            try
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.FileTypeChoices.Add("XML text", new List<string>() { extension });
                savePicker.SuggestedFileName = filename;
                StorageFile saveStudioSetFile = await savePicker.PickSaveFileAsync();
                await FileIO.WriteTextAsync(saveStudioSetFile, content);
            }
            catch { }
        }

        public void SetMainPagePortable(INTEGRA_7_Xamarin.MainPage mainPage)
        {
            throw new NotImplementedException();
        }

        private async Task<String> Load()
        {
            String linesToUnpack = "";
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.FileTypeFilter.Add(".xml");
                openPicker.FileTypeFilter.Add(".fav");
                StorageFile loadFavoritesFile = await openPicker.PickSingleFileAsync();

                if (loadFavoritesFile != null)
                {
                    if (loadFavoritesFile.Name.EndsWith(".fav"))
                    {
                        // Load old style .fav file:
                        IList<String> lines = await FileIO.ReadLinesAsync(loadFavoritesFile);
                        //String linesToUnpack = "";
                        if (lines != null && lines.Count() > 0)
                        {
                            //commonState.favoritesList.folders = new List<FavoritesFolder>();
                            foreach (String line in lines)
                            {
                                linesToUnpack += line + '\n';
                            }
                            linesToUnpack = linesToUnpack.TrimEnd('\n');
                            //localSettings.Values["Favorites"] = linesToUnpack;
                            //UnpackFoldersWithFavoritesString(linesToUnpack);
                        }
                    }
                    //else
                    //{
                        // Load new style .xml file
                        //Stream stream = await loadFavoritesFile.OpenStreamForReadAsync();
                        //await Task.Run(() =>
                        //{
                        //    MemoryStream memoryStream = new MemoryStream();
                        //    stream.CopyTo(memoryStream);
                        //    memoryStream.Position = 0;
                        //    DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(FavoritesList));
                        //    XmlReader xmlReader = XmlReader.Create(memoryStream);
                        //    XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(xmlReader);
                        //    commonState.favoritesList = (FavoritesList)dataContractSerializer.ReadObject(xmlDictionaryReader);
                        //    xmlReader.Dispose();
                        //});
                    //}
                    //UpdateFoldersList();
                }
            }
            catch (Exception e) { }
            return linesToUnpack;
        }

        //private void UnpackFoldersWithFavoritesString(String foldersWithFavorites)
        //{
        //    t.Trace("private void UnpackFoldersWithFavoritesString (" + "String" + foldersWithFavorites + ", " + ")");
        //    // Format: [Folder name\v[Group index\tCategory index\tTone index\tGroup\tCategory\tTone\b]\f...]...
        //    // I.e. Split all by \f to get all folders with content.
        //    // Split each folder by \v to get folder name and all favorites together.
        //    // Split favorites by \b to get all favorites one by one.
        //    // Split each favorite by \t to get the 6 parts (3 indexes, 3 names).
        //    FavoritesFolder folder = null;
        //    foreach (String foldersWithFavoritePart in foldersWithFavorites.Split('\f'))
        //    {
        //        String[] folderWithFavorites = foldersWithFavoritePart.Split('\v');
        //        // Folder name:
        //        folder = new FavoritesFolder(folderWithFavorites[0]);
        //        commonState.favoritesList.folders.Add(folder);
        //        if (folderWithFavorites.Length > 1)
        //        {
        //            String[] favorites = folderWithFavorites[1].Split('\b');
        //            foreach (String favorite in favorites)
        //            {
        //                String[] favoriteParts = favorite.Split('\t');
        //                try
        //                {
        //                    if (favoriteParts.Length == 6)
        //                    {
        //                        folder.FavoritesTones.Add(new Tone(Int32.Parse(favoriteParts[0]), Int32.Parse(favoriteParts[1]), 
        //                            Int32.Parse(favoriteParts[2]), favoriteParts[3], favoriteParts[4], favoriteParts[5]));
        //                    }
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //}
    }
}
