using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Integra_7_Xamarin
{
    public partial class UIHandler
    {
        public enum FavoritesAction
        {
            SHOW,
            ADD,
            REMOVE
        }

        public FavoritesAction favoritesAction = FavoritesAction.SHOW;
        public Player player;
        Int32 Favorites_CurrentFolder = -1;

        Grid Favorites_grLeftColumn = null;
        Button Favorites_lblFolders = null;
        ListView Favorites_lvFolderList = null;
        public ObservableCollection<String> Favorites_ocFolderList = null;
        Grid Favorites_grMiddleColumn = null;
        Button Favorites_lblFavorites = null;
        ListView Favorites_lvFavoriteList = null;
        public ObservableCollection<String> Favorites_ocFavoriteList = null;
        List<FavoriteTone> Favorites_CurrentlyInFavoriteList = null;
        Grid gNewFolder = null;
        Editor Favorites_edNewFolderName = null;
        Image imgOk = null;
        Image imgNok = null;
        Button btnOk = null;
        Button btnNok = null;
        Button Favorites_btnAddFolder = null;
        Button Favorites_btnDeleteFolder = null;
        TextBlock Favorites_tbHelp = null;
        Button Favorites_btnAddFavorite = null;
        Button Favorites_btnDeleteFavorite = null;
        Button Favorites_btnSelectFavorite = null;
        Button Favorites_btnPlay = null;
        Button Favorites_btnBackup = null;
        Button Favorites_btnRestore = null;
        Button Favorites_btnReturn = null;
        Grid Favorites_grRightColumn = null;

        public void DrawFavoritesPage()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Favorites
            // --------------------------------------------------------------------------------------------
            // | Folderlist         | Tonelist                                     | Foldername editor    |
            // |--------------------|----------------------------------------------|----------------------|
            // |                    |                                              | Add folder button    |
            // |                    |                                              |----------------------|
            // |                    |                                              | Delete folder button |
            // |                    |                                              |----------------------|
            // |                    |                                              | Help text area       |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |----------------------|
            // |                    |                                              | Context button       |
            // |                    |                                              |----------------------|
            // |                    |                                              | Play button          |
            // |                    |                                              |----------------------|
            // |                    |                                              | Backup buttton       |
            // |                    |                                              |----------------------|
            // |                    |                                              | Restore button       |
            // |                    |                                              |----------------------|
            // |                    |                                              | Return button        |
            // --------------------------------------------------------------------------------------------

            // Create all controls ------------------------------------------------------------------------
            Favorites_lblFolders = new Button();
            Favorites_lblFolders.Text = "Folders";
            Favorites_lblFolders.IsEnabled = false;

            Favorites_lvFolderList = new ListView();
            Favorites_ocFolderList = new ObservableCollection<String>();
            Favorites_lvFolderList.ItemsSource = Favorites_ocFolderList;

            Favorites_lblFavorites = new Button();
            Favorites_lblFavorites.Text = "Tones";
            Favorites_lblFavorites.IsEnabled = false;

            Favorites_lvFavoriteList = new ListView();
            Favorites_ocFavoriteList = new ObservableCollection<String>();
            Favorites_lvFavoriteList.ItemsSource = Favorites_ocFavoriteList;
            Favorites_CurrentlyInFavoriteList = new List<FavoriteTone>();

            Favorites_edNewFolderName = new Editor();
            Favorites_edNewFolderName.BackgroundColor = colorSettings.ControlBackground;
            Favorites_edNewFolderName.VerticalOptions = LayoutOptions.FillAndExpand;

            gNewFolder = new Grid();
            Favorites_btnAddFolder = new Button();
            Favorites_btnAddFolder.Text = "Add folder";
            btnOk = new Button();
            btnNok = new Button();
            btnOk.BackgroundColor = colorSettings.Transparent;
            btnNok.BackgroundColor = colorSettings.Transparent;
            imgOk = new Image();
            imgNok = new Image();
            imgOk.Source = ImageSource.FromFile("Ok.png");
            imgNok.Source = ImageSource.FromFile("Nok.png");

            Favorites_btnDeleteFolder = new Button();
            Favorites_btnDeleteFolder.Text = "Delete selected folder";

            Favorites_tbHelp = new TextBlock();
            Favorites_tbHelp.HorizontalTextAlignment = TextAlignment.Center;
            Favorites_tbHelp.VerticalTextAlignment = TextAlignment.Center;
            Favorites_tbHelp.BackgroundColor = colorSettings.ControlBackground;

            //Favorites_btnCopyFavorite = new Button();
            //Favorites_btnCopyFavorite.Text = "Copy";

            Favorites_btnAddFavorite = new Button();
            Favorites_btnAddFavorite.Text = "Delete favorite";

            Favorites_btnDeleteFavorite = new Button();
            Favorites_btnDeleteFavorite.Text = "Delete favorite";

            Favorites_btnSelectFavorite = new Button();
            Favorites_btnSelectFavorite.Text = "Select favorite";

            Favorites_btnPlay = new Button();
            Favorites_btnPlay.Text = "Play";

            Favorites_btnBackup = new Button();
            Favorites_btnBackup.Text = "Backup to file";

            Favorites_btnRestore = new Button();
            Favorites_btnRestore.Text = "Restore from file";

            Favorites_btnReturn = new Button();
            Favorites_btnReturn.Text = "Return";

            // Add handlers -------------------------------------------------------------------------------

            // Folderlist handlers:
            Favorites_lvFolderList.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    Favorites_lvFolderList_ItemSelected(Favorites_lvFolderList.SelectedItem, null);
                }),
                NumberOfTapsRequired = 1
            });
            Favorites_lvFolderList.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    lvFolders_DoubleTapped(null);
                }),
                NumberOfTapsRequired = 2
            });

            // FavoriteList handlers:
            Favorites_lvFavoriteList.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    Favorites_lvFavoriteList_ItemSelected(Favorites_lvFavoriteList.SelectedItem, null);
                }),
                NumberOfTapsRequired = 1
            });
            Favorites_lvFavoriteList.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    lvFavorites_DoubleTapped(null);
                }),
                NumberOfTapsRequired = 2
            });

            // Other handlers:
            Favorites_edNewFolderName.TextChanged += Favorites_edNewFolderName_TextChanged;
            btnOk.Clicked += BtnOk_Clicked;
            btnNok.Clicked += BtnNok_Clicked;
            Favorites_btnAddFolder.Clicked += Favorites_btnAddFolder_Clicked;
            //Favorites_btnCopyFavorite.Clicked += Favorites_btnCopyFavorite_Clicked;
            Favorites_btnDeleteFolder.Clicked += Favorites_btnDeleteFolder_Clicked;
            Favorites_btnAddFavorite.Clicked += Favorites_btnAddFavorite_Clicked;
            Favorites_btnDeleteFavorite.Clicked += Favorites_btnDeleteFavorite_Clicked;
            Favorites_btnSelectFavorite.Clicked += Favorites_btnSelectFavorite_Clicked;
            Favorites_btnPlay.Clicked += Librarian_btnPlay_Clicked; //  Favorites_btnPlay_Clicked; All in same class now, UIHandler.
            Favorites_btnBackup.Clicked += Favorites_btnBackup_Clicked;
            Favorites_btnRestore.Clicked += Favorites_btnRestore_Clicked;
            Favorites_btnReturn.Clicked += Favorites_btnReturn_Clicked;

            // Assemble grids with controls ---------------------------------------------------------------

            // A grid for the left column:
            Favorites_grLeftColumn = new Grid();
            Favorites_grLeftColumn.Children.Add(new GridRow(0, new View[] { Favorites_lblFolders }, null, false, false).Row);
            Favorites_grLeftColumn.Children.Add(new GridRow(1, new View[] { Favorites_lvFolderList }, null, false, false).Row);

            RowDefinitionCollection Favorites_rdcLeft = new RowDefinitionCollection();
            Favorites_rdcLeft.Add(new RowDefinition());
            Favorites_rdcLeft.Add(new RowDefinition());
            Favorites_rdcLeft[0].Height = new GridLength(rowHeight, GridUnitType.Absolute);
            Favorites_rdcLeft[1].Height = new GridLength(0, GridUnitType.Auto);

            Favorites_grLeftColumn.RowDefinitions.Add(Favorites_rdcLeft[0]);
            Favorites_grLeftColumn.RowDefinitions.Add(Favorites_rdcLeft[1]);

            // A grid for the middle column:
            Favorites_grMiddleColumn = new Grid();
            Favorites_grMiddleColumn.Children.Add(new GridRow(0, new View[] { Favorites_lblFavorites }, null, false, false).Row);
            Favorites_grMiddleColumn.Children.Add(new GridRow(1, new View[] { Favorites_lvFavoriteList }, null, false, false).Row);

            RowDefinitionCollection Favorites_rdcMiddle = new RowDefinitionCollection();
            Favorites_rdcMiddle.Add(new RowDefinition());
            Favorites_rdcMiddle.Add(new RowDefinition());
            Favorites_rdcMiddle[0].Height = new GridLength(rowHeight, GridUnitType.Absolute);
            Favorites_rdcMiddle[1].Height = new GridLength(0, GridUnitType.Auto);

            Favorites_grMiddleColumn.RowDefinitions.Add(Favorites_rdcMiddle[0]);
            Favorites_grMiddleColumn.RowDefinitions.Add(Favorites_rdcMiddle[1]);

            // Create the new folder grid:
            gNewFolder.Children.Add(new GridRow(0, new View[] { Favorites_edNewFolderName }).Row);
            gNewFolder.Children.Add(new GridRow(1, new View[] { btnOk, btnNok }).Row);
            gNewFolder.Children.Add(new GridRow(1, new View[] { imgOk, imgNok }).Row);
            gNewFolder.Children.Add(new GridRow(0, new View[] { Favorites_btnAddFolder }, null, false, false, 2).Row);
            Favorites_edNewFolderName.IsVisible = false;
            imgOk.IsVisible = false;
            imgNok.IsVisible = false;
            imgOk.InputTransparent = true;
            imgNok.InputTransparent = true;
            btnOk.IsVisible = false;
            btnNok.IsVisible = false;

            // A grid for the right column:

            Favorites_grRightColumn = new Grid();
            RowDefinitionCollection Favorites_rdcRight = new RowDefinitionCollection();

            for (Int32 i = 0; i < 10; i++)
            {
                Favorites_rdcRight.Add(new RowDefinition());
                if (i == 2)
                {
                    Favorites_rdcRight[i].Height = new GridLength(5, GridUnitType.Star);
                }
                else
                {
                    Favorites_rdcRight[i].Height = new GridLength(1, GridUnitType.Star);
                }
                Favorites_grRightColumn.RowDefinitions.Add(Favorites_rdcRight[i]);
            }

            Favorites_grRightColumn.Children.Add(new GridRow(0, new View[] { gNewFolder }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(1, new View[] { Favorites_btnDeleteFolder }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(2, new View[] { Favorites_tbHelp }, null, false, false).Row);
            //Favorites_grRightColumn.Children.Add(new GridRow(3, new View[] { Favorites_btnCopyFavorite }, null, false, false).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(3, new View[] { Favorites_btnAddFavorite }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(4, new View[] { Favorites_btnDeleteFavorite }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(5, new View[] { Favorites_btnSelectFavorite }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(6, new View[] { Favorites_btnPlay }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(7, new View[] { Favorites_btnBackup }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(8, new View[] { Favorites_btnRestore }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(9, new View[] { Favorites_btnReturn }).Row);

            // Assemble FavoritesStackLayout --------------------------------------------------------------

            Favorites_StackLayout = new StackLayout();
            Favorites_StackLayout.Children.Add((new GridRow(0, new View[] { Favorites_grLeftColumn, Favorites_grMiddleColumn, Favorites_grRightColumn })).Row);
            Favorites_StackLayout.BackgroundColor = Color.Black;
            Favorites_UpdateFoldersList();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Handlers
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Folder listview handlers -------------------------------------------------------------------------------------
        private void Favorites_lvFolderList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PushHandleControlEvents();
            UpdateFavoritesListFromCommonstate((String)sender);
            PopHandleControlEvents();
            Favorites_btnDeleteFolder.IsEnabled = true;
            Favorites_CurrentFolder = Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem);
            if (Favorites_lvFolderList.SelectedItem != null)
            {
                Favorites_btnDeleteFolder.IsEnabled = true;
            }
            if (favoritesAction == FavoritesAction.REMOVE)
            {
                // Since we came here to delete a favorite and are now
                // selecting a folder: If the folder name starts with a star,
                // select the item to delete in the favorites list:
                if (((String)Favorites_lvFolderList.SelectedItem).StartsWith("*"))
                {
                    Favorites_lvFavoriteList.SelectedItem = Favorites_ocFavoriteList
                        .IndexOf(commonState.CurrentTone.Name);
                }
            }
        }

        private void lvFolders_DoubleTapped(object sender/*, DoubleTappedRoutedEventArgs e*/)
        {
            if (commonState.CurrentTone != null && Favorites_lvFolderList.SelectedItem != null)
            {
                //t.Trace("private void lvFolders_DoubleTapped(" + ((ListView)sender).SelectedIndex.ToString() + ")");
                //ListViewItem item = (ListViewItem)Favorites_lvFolderList.ContainerFromItem(Favorites_lvFolderList.SelectedItem);
                if (favoritesAction == FavoritesAction.ADD 
                    && ((String)Favorites_lvFolderList.SelectedItem).StartsWith("*"))
                {
                    commonState.FavoritesList.FavoritesFolders[Favorites_ocFolderList
                        .IndexOf(Favorites_lvFolderList.SelectedItem)]
                        .FavoriteTones.Add(new FavoriteTone(
                            commonState.CurrentTone.Group,
                            commonState.CurrentTone.Category,
                            commonState.CurrentTone.Name));
                    Favorites_lvFolderList.SelectedItem = 
                        ((String)Favorites_lvFolderList.SelectedItem).TrimStart('*');
                    if (Favorites_lvFolderList.SelectedItem != null)
                    {
                        UpdateFavoritesListFromCommonstate((String)Favorites_lvFolderList.SelectedItem);
                        //Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem));
                        SaveToLocalSettings();
                    }
                }
                else if (favoritesAction == FavoritesAction.REMOVE && ((String)Favorites_lvFolderList.SelectedItem).StartsWith("*"))
                {
                    FavoriteTone tone = commonState.FavoritesList.FavoritesFolders[Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem)].ByToneName(commonState.CurrentTone.Name);
                    commonState.FavoritesList.FavoritesFolders[Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem)].FavoriteTones.Remove(tone);
                    Favorites_lvFolderList.SelectedItem = ((String)Favorites_lvFolderList.SelectedItem).TrimStart('*');
                    UpdateFavoritesListFromCommonstate((String)Favorites_lvFolderList.SelectedItem);
                    SaveToLocalSettings();
                }
            }
        }

        // Favorites listview handlers ----------------------------------------------------------------------------------
        private void Favorites_lvFavoriteList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (handleControlEvents)
            {
                Favorites_btnDeleteFolder.IsEnabled = true;
                //Favorites_btnCopyFavorite.IsEnabled = true;
                Favorites_btnAddFavorite.IsEnabled = true;
                Favorites_btnDeleteFavorite.IsEnabled = true;
                Favorites_btnSelectFavorite.IsEnabled = true;
                Favorites_btnPlay.IsEnabled = true;
                FavoriteTone favoriteTone;
                try
                {
                    // The user came here to browse favorites, and has now selected one.
                    // The favorite should be selectable via the 'Select <name>' button.
                    if (commonState.CurrentTone != null && favoritesAction == FavoritesAction.SHOW)
                    {
                        // List only contains the name, lookup the currentTone by name and folder:
                        favoriteTone = Favorites_CurrentlyInFavoriteList
                            .Find(f => f.Name == (String)Favorites_lvFavoriteList.SelectedItem);
                        commonState.CurrentTone = new Tone(-1, -1, -1, favoriteTone.Group, favoriteTone.Category, favoriteTone.Name);

                        if (commonState.CurrentTone.Index > -1)
                        {
                            List<String> tone = commonState.ToneList.Tones[commonState.CurrentTone.Index];
                            commonState.Midi.ProgramChange(commonState.Midi.GetMidiInPortChannel(), tone[4], tone[5], tone[7]);
                        }
                        else
                        {
                            Int32 index = commonState.ToneList.Get(commonState.CurrentTone);
                            if (index > -1)
                            {
                                List<String> tone = commonState.ToneList.Tones[commonState.ToneList.Get(commonState.CurrentTone)];
                                commonState.Midi.ProgramChange(commonState.Midi.GetMidiOutPortChannel(), tone[4], tone[5], tone[7]);
                            }
                        }
                    }
                    // The user came here to add a favorite, and now selects another favorite.
                    // The favorite should be allowed to be deleted, but we must keep the one to store
                    else if (favoritesAction == FavoritesAction.ADD)
                    {
                        Favorites_btnDeleteFavorite.Text = "Delete " + Favorites_lvFavoriteList.SelectedItem;
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Right tapping or double-clicking a favorite should take immediate action
        /// without the need to click the context button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvFavorites_DoubleTapped(object sender/*, DoubleTappedRoutedEventArgs e*/)
        {
            if (handleControlEvents)
            {
                try
                {
                    // List only contains the name, lookup the currentTone by name and folder.
                    // Important note! Assigning the selected tone (fromFavorites_CurrentlyInFavoriteList)
                    // to commonState.currentTone will only create a reference, which will change the
                    // content of the favorite when changing tone in the Librarian. Creating a new
                    // object with value arguments (NOT the object, which is a reference object)
                    // will keep the objects separate.
                    // Find the Tone object from Favorites_CurrentlyInFavoriteList:
                    FavoriteTone tone = Favorites_CurrentlyInFavoriteList
                        .Find(f => f.Name == (String)Favorites_lvFavoriteList.SelectedItem);
                    // Use the variables in the found Tone object to create a new object and
                    // assign it to commonState.currentTone:
                    commonState.CurrentTone = 
                        new Tone(-1, -1, -1, tone.Group, 
                        tone.Category, tone.Name);
                    Librarian_SynchronizeListviews();
                    Favorites_StackLayout.IsVisible = false;
                    ShowLibrarianPage();
                }
                catch { }
            }
        }

        // Add/delete folder controls handlers --------------------------------------------------------------------------
        private void Favorites_edNewFolderName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //t.Trace("private void tbNewFolder_KeyUp (" + "object" + sender + ", " + "KeyRoutedEventArgs" + e + ", " + ")");
            if (!String.IsNullOrEmpty(Favorites_edNewFolderName.Text))
            {
                Boolean found = false;
                if (commonState.FavoritesList != null && commonState.FavoritesList.FavoritesFolders != null && commonState.FavoritesList.FavoritesFolders.Count() > 0)
                {
                    foreach (FavoritesFolder folder in commonState.FavoritesList.FavoritesFolders)
                    {
                        if (folder.Name == Favorites_edNewFolderName.Text.Trim())
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Favorites_btnAddFolder.IsEnabled = true;
                        if (((String)e.NewTextValue).Contains("\r"))
                        {
                            commonState.FavoritesList.FavoritesFolders.Add(new FavoritesFolder(Favorites_edNewFolderName.Text.Trim().Replace("\r", "")));
                            Favorites_edNewFolderName.Text = "";
                            Favorites_UpdateFoldersList();
                        }
                    }
                    else
                    {
                        Favorites_btnAddFolder.IsEnabled = false;
                    }
                }
            }
            else
            {
                Favorites_btnAddFolder.IsEnabled = false;
            }
        }

        private void BtnNok_Clicked(object sender, EventArgs e)
        {
            imgOk.IsVisible = false;
            imgNok.IsVisible = false;
            btnOk.IsVisible = false;
            btnNok.IsVisible = false;
            Favorites_edNewFolderName.IsVisible = false;
            Favorites_btnAddFolder.IsVisible = true;
        }

        private void BtnOk_Clicked(object sender, EventArgs e)
        {
            imgOk.IsVisible = false;
            imgNok.IsVisible = false;
            btnOk.IsVisible = false;
            btnNok.IsVisible = false;
            Favorites_edNewFolderName.IsVisible = false;
            Favorites_btnAddFolder.IsVisible = true;
            if (String.IsNullOrEmpty(Favorites_edNewFolderName.Text)
                || Favorites_ocFolderList.Contains(Favorites_edNewFolderName.Text))
            {
                ShowMessage("Please type a unique name for the new folder.");
            }
            else
            {
                commonState.FavoritesList.FavoritesFolders.Add(new FavoritesFolder(Favorites_edNewFolderName.Text.Trim()));
                Favorites_edNewFolderName.Text = "";
                Favorites_UpdateFoldersList();
            }
        }

        private void Favorites_btnAddFolder_Clicked(object sender, EventArgs e)
        {
            imgOk.IsVisible = true;
            imgNok.IsVisible = true;
            btnOk.IsVisible = true;
            btnNok.IsVisible = true;
            Favorites_edNewFolderName.IsVisible = true;
            Favorites_btnAddFolder.IsVisible = false;
        }

        private void Favorites_btnDeleteFolder_Clicked(object sender, EventArgs e)
        {
            DeleteFolder((String)Favorites_lvFolderList.SelectedItem);
        }

        // Favorite buttons handlers ------------------------------------------------------------------------------------

        private void Favorites_btnAddFavorite_Clicked(object sender, EventArgs e)
        {
            if (commonState.CurrentTone != null && Favorites_lvFolderList.SelectedItem != null)
            {
                //t.Trace("private void btnContext_Click (" + "object" + sender + ", " + "RoutedEventArgs" + e + ", " + ")");
                //ListViewItem item = (ListViewItem)lvFolders.ContainerFromItem(lvFolders.Items[lvFolders.SelectedIndex]);
                if (favoritesAction == FavoritesAction.ADD/* && ((String)Favorites_lvFolderList.SelectedItem).StartsWith("*")*/)
                {
                    Int32 index = Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem);
                    if (index > -1)
                    {
                        commonState.FavoritesList.FavoritesFolders[index].FavoriteTones
                            .Add(new FavoriteTone(
                                commonState.CurrentTone.Group,
                                commonState.CurrentTone.Category,
                                commonState.CurrentTone.Name));
                        Favorites_ocFolderList[Favorites_CurrentFolder] = ((String)Favorites_lvFolderList.SelectedItem).TrimStart('*');
                        Favorites_lvFolderList.SelectedItem = Favorites_ocFolderList[Favorites_CurrentFolder];
                        Favorites_UpdateFoldersList(Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem));
                        UpdateFavoritesListFromCommonstate((String)Favorites_lvFolderList.SelectedItem);
                        //Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem));
                        SaveToLocalSettings();
                    }
                }
            }
        }

        private void Favorites_btnDeleteFavorite_Clicked(object sender, EventArgs e)
        {
            PushHandleControlEvents();
            if (commonState.CurrentTone != null && Favorites_lvFolderList.SelectedItem != null)
            {
                try
                {
                    FavoriteTone tone = Favorites_CurrentlyInFavoriteList.
                        Find(f => f.Name == (String)Favorites_lvFavoriteList.SelectedItem);
                    if (tone != null)
                    {
                        DeleteFavorite(tone);
                        UpdateFavoritesListFromCommonstate((String)Favorites_lvFolderList.SelectedItem);
                    }
                }
                catch { }
            }
            PopHandleControlEvents();
        }

        private void Favorites_btnSelectFavorite_Clicked(object sender, EventArgs e)
        {
            Librarian_SynchronizeListviews();
            Favorites_StackLayout.IsVisible = false;
            ShowLibrarianPage();
        }

        // Backup/restore button handlers -------------------------------------------------------------------------------
        private async void Favorites_btnRestore_Clicked(object sender, EventArgs e)
        {
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();
                if (fileData != null)
                {
                    if (fileData.FileName.ToLower().EndsWith(".fav"))
                    {
                        byte[] data = fileData.DataArray;
                        String favorites = "";
                        for (Int32 i = 0; i < data.Length; i++)
                        {
                            favorites += (char)data[i];
                        }
                        Favorites_Restore(favorites);
                    }
                    else if (fileData.FileName.ToLower().EndsWith(".fav_xml"))
                    {
                        byte[] data = fileData.DataArray;
                        await Task.Run(() =>
                        {
                            MemoryStream memoryStream = new MemoryStream(data);
                            memoryStream.Position = 0;
                            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(FavoritesList));
                            XmlReader xmlReader = XmlReader.Create(memoryStream);
                            XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(xmlReader);
                            commonState.FavoritesList = (FavoritesList)dataContractSerializer.ReadObject(xmlDictionaryReader);
                            xmlReader.Dispose();
                        });
                        Favorites_UpdateFoldersList();
                        UpdateFavoritesListFromCommonstate((String)Favorites_lvFolderList.SelectedItem);
                        SaveToLocalSettings();
                    }
                    else
                    {
                        await mainPage.DisplayAlert("Integra-7 Librarian", "You can only restore favorites from a file with " +
                            "a file extension '.fav_xml' (or '.fav' if you have saved from the old version of the program)!", "Cancel");
                    }
                }
            }
            catch { }
        }

        private async void Favorites_btnBackup_Clicked(object sender, EventArgs e)
        {
            t.Trace("private void btnSave_Click (" + "object" + sender + ", " + "RoutedEventArgs" + e + ", " + ")");
            try
            {
                String xmlText = "";
                await Task.Run(() =>
                {
                    MemoryStream memoryStream = new MemoryStream();
                    XmlWriter xmlWriter = XmlWriter.Create(memoryStream);
                    using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(xmlWriter))
                    {
                        var dataContractSerializer = new DataContractSerializer(typeof(FavoritesList));
                        dataContractSerializer.WriteObject(memoryStream, commonState.FavoritesList);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        for (Int32 i = 0; i < memoryStream.Length; i++)
                        {
                            xmlText += (char)memoryStream.ReadByte();
                        }
                        xmlWriter.Flush();
                        xmlDictionaryWriter.Dispose();
                    }
                    xmlWriter.Dispose();
                    memoryStream.Dispose();
                });
                xmlText = xmlText.Replace("><", ">\n<");
                String[] lines = xmlText.Split('\n');
                String indent = "";
                xmlText = "";
                Boolean firstLine = true;
                foreach (String line in lines)
                {
                    if (firstLine)
                    {
                        xmlText = line + "\r\n";
                        firstLine = false;
                    }
                    else if (line.StartsWith("</"))
                    {
                        if (indent.Length > 0)
                        {
                            indent = indent.Remove(0, 1);
                        }
                        xmlText += indent + line + "\r\n";
                    }
                    else if (line.Contains("</"))
                    {
                        xmlText += indent + line + "\r\n";
                    }
                    else
                    {
                        xmlText += indent + line + "\r\n";
                        indent += "\t";
                    }
                }
                myFileIO.SaveFileAsync(xmlText, ".fav_xml");
            }
            catch { }
        }

        // Return to Librarian button handler ---------------------------------------------------------------------------
        private void Favorites_btnReturn_Clicked(object sender, EventArgs e)
        {
            Favorites_StackLayout.IsVisible = false;
            ShowLibrarianPage();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Public functions to be called from code in other files
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ShowFavoritesPage(FavoritesAction favoriteAction)
        {
            this.favoritesAction = favoriteAction;
            Favorites_StackLayout.IsVisible = true;
            commonState.Player.btnPlayStop = Favorites_btnPlay;

            if (commonState.Player.Playing)
            {
                Favorites_btnPlay.Content = "Stop";
            }
            else
            {
                Favorites_btnPlay.IsEnabled = false;
            }

            switch (favoritesAction)
            {
                case FavoritesAction.SHOW:
                Favorites_UpdateFoldersList();
                    break;
                case FavoritesAction.ADD:
                    Favorites_tbHelp.Text = "The folder(s) that does not already contain the Tone " + commonState.CurrentTone.Name +
                        " has been marked with a \'*\'. Doubletap the folders you wish to add the Tone \'" +
                        commonState.CurrentTone.Name + "\' to (or select the folder name and click 'Add " +
                        commonState.CurrentTone.Name + "').";
                    Favorites_btnAddFavorite.Content = "Add " + commonState.CurrentTone.Name;
                    Favorites_UpdateFoldersList();
                    break;
                case FavoritesAction.REMOVE:
                Favorites_tbHelp.Text = "The folder(s) that contains the Tone " + commonState.CurrentTone.Name +
                    " has been marked with a \'*\'. Doubletap the folders you wish to remove the Tone \'" +
                    commonState.CurrentTone.Name + "\' from (or select the folder name and click 'Delete " +
                    commonState.CurrentTone.Name + "').";
                Favorites_btnDeleteFavorite.Content = "Delete " + commonState.CurrentTone.Name;
                Favorites_UpdateFoldersList();
                    break;
            }
        }

        public void Favorites_Restore(String linesToUnpack)
        {
            if (linesToUnpack != "Error" && linesToUnpack != "Empty")
            {
                UnpackFoldersWithFavoritesString(linesToUnpack);
                SaveToLocalSettings();
                Favorites_UpdateFoldersList();
            }
        }

        private void Favorites_ReadFavoritesFromLocalSettings()
        {
            String favorites = "";
            try
            {
                favorites = (String)mainPage.LoadLocalValue("Favorites");
                if (!String.IsNullOrEmpty(favorites))
                {
                    UnpackFoldersWithFavoritesString(favorites);
                    Favorites_UpdateFoldersList();
                }
            }
            catch { }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Local helpers
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private async void DeleteFolder(String folder)
        {
            //MessageDialog warning = new MessageDialog("Warning: Deleting a folder will also delete all containing favorites.\r\n\r\n" +
            //    "Are you sure you want to do this?");
            Boolean response = await mainPage.DisplayAlert("INTEGRA_7 Librarian", 
                "Warning: Deleting a folder will also delete all containing favorites.\r\n\r\n" +
                "Are you sure you want to do this?", "Yes", "No");
            if (response)
            {
                commonState.FavoritesList.FavoritesFolders.RemoveAt(Favorites_ocFolderList.IndexOf(folder));
                SaveToLocalSettings();
                Favorites_UpdateFoldersList();
            }
        }

        private void DeleteFavorite(FavoriteTone Tone)
        {
            t.Trace("private void DeleteFavorite (" + Tone.Name + ")");
            UInt16 i = 0;
            Int32 index = 0;
            Int32 folderIndex = Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem);
            Boolean found = false;
            foreach (FavoriteTone tone in commonState.FavoritesList.FavoritesFolders[folderIndex].FavoriteTones)
            {
                if (tone.Name == Tone.Name)
                {
                    index = i;
                    found = true;
                }
                i++;
            }
            if (found)
            {
                commonState.FavoritesList.FavoritesFolders[folderIndex].FavoriteTones.RemoveAt(index);
                UpdateFavoritesListFromCommonstate((String)Favorites_lvFolderList.SelectedItem);//, Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem));
                Favorites_UpdateFoldersList(folderIndex);
            }
        }

        private void SelectFolder(String folderName)
        {
            t.Trace("private void SelectFolder (" + "String" + folderName + ", " + ")");
            try
            {
                foreach (String item in Favorites_ocFolderList.AsQueryable())
                {
                    if (item.TrimStart('*') == folderName)
                    {
                        Favorites_lvFolderList.SelectedItem = item;
                        return;
                    }
                }
            }
            catch (Exception e) { }
        }

        private void UnpackFoldersWithFavoritesString(String foldersWithFavorites)
        {
            t.Trace("private void UnpackFoldersWithFavoritesString (" + "String" + foldersWithFavorites + ", " + ")");
            // Format: [Folder name\v[Group index\tCategory index\tTone index\tGroup\tCategory\tTone\b]\f...]...
            // I.e. Split all by \f to get all folders with content.
            // Split each folder by \v to get folder name and all favorites together.
            // Split favorites by \b to get all favorites one by one.
            // Split each favorite by \t to get the 6 parts (3 indexes, 3 names).
            if (foldersWithFavorites != null)
            {
                FavoritesFolder folder = null;
                commonState.FavoritesList.FavoritesFolders.Clear();
                foreach (String foldersWithFavoritePart in foldersWithFavorites.Split('\f'))
                {
                    String[] folderWithFavorites = foldersWithFavoritePart.Split('\v');
                    // Folder name:
                    folder = new FavoritesFolder(folderWithFavorites[0]);
                    commonState.FavoritesList.FavoritesFolders.Add(folder);
                    if (folderWithFavorites.Length > 1)
                    {
                        String[] favorites = folderWithFavorites[1].Split('\b');
                        foreach (String favorite in favorites)
                        {
                            String[] favoriteParts = favorite.Split('\t');
                            try
                            {
                                if (favoriteParts.Length == 6)
                                {
                                    folder.FavoriteTones.Add(new FavoriteTone(favoriteParts[3], favoriteParts[4], favoriteParts[5]));
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        private void SaveToLocalSettings()
        {
            t.Trace("private void SaveToLocalSettings()");
            // Format: [Folder name\v[Group index\tCategory index\tTone index\tGroup\tCategory\tTone\b]\f...]...
            // I.e. Loop all folders, loop all favorites.
            // Pack the 6 parts of the favorite as strings separated by \t.
            // Concatenate the parts separated by \b.
            // Concatenate the folder name and tthe parts separated by a \v.
            // Concatenate all folders separated by a \b.
            String toSave = "";
            try
            {
                foreach (FavoritesFolder folder in commonState.FavoritesList.FavoritesFolders)
                {
                    toSave += folder.Name + '\v';
                    foreach (FavoriteTone favorite in folder.FavoriteTones)
                    {
                        toSave += "-1\t-1\t-1\t" + favorite.Group + "\t" +
                            favorite.Category + "\t" + favorite.Name + "\b";
                    }
                    toSave = toSave.TrimEnd('\b') + "\f";
                }
                toSave = toSave.TrimEnd('\f');
            }
            catch { }
            mainPage.SaveLocalValue("Favorites", toSave);
        }

        private void Favorites_UpdateFoldersList(Int32 SelectedFolderIndex = -1)
        {
            if (handleControlEvents)
            {
                t.Trace("private void UpdateFoldersList(SelectedIndex = " + SelectedFolderIndex.ToString() + ")");
                PushHandleControlEvents();
                if (Favorites_ocFavoriteList != null)
                {
                    try
                    {
                        Int32 count = 0;
                        Favorites_ocFavoriteList.Clear(); // Since we will not have a selected folder, do not show favorites!
                        Favorites_CurrentlyInFavoriteList.Clear();
                        Favorites_ocFolderList.Clear();
                        UInt16 i = 0;
                        foreach (FavoritesFolder folder in commonState.FavoritesList.FavoritesFolders)
                        {
                            Boolean mark = false;
                            if (favoritesAction == FavoritesAction.ADD || favoritesAction == FavoritesAction.REMOVE)
                            {
                                foreach (FavoriteTone fav in folder.FavoriteTones)
                                {
                                    if (fav.Group == commonState.CurrentTone.Group
                                        && fav.Category == commonState.CurrentTone.Category
                                        && fav.Name == commonState.CurrentTone.Name)
                                    {
                                        mark = true;
                                        SelectedFolderIndex = i;
                                        count++;
                                    }
                                }
                            }
                            if (favoritesAction == FavoritesAction.ADD)
                            {
                                mark = !mark;
                            }

                            if (mark)
                            {
                                Favorites_ocFolderList.Add("*" + folder.Name);
                            }
                            else
                            {
                                Favorites_ocFolderList.Add(folder.Name);
                            }
                            i++;
                        }
                        if ((SelectedFolderIndex > -1 && favoritesAction == FavoritesAction.SHOW) || (count > 0 && favoritesAction == FavoritesAction.REMOVE) || (count == 0 && favoritesAction == FavoritesAction.ADD))
                        {
                            // There are still items to delete or still room for more items, 
                            // stay in marked mode:
                            if (Favorites_ocFolderList.Count() > 0)
                            {
                                if (SelectedFolderIndex > -1 && SelectedFolderIndex < Favorites_ocFolderList.Count)
                                {
                                    Favorites_lvFolderList.SelectedItem = Favorites_ocFolderList[SelectedFolderIndex];
                                }
                                else
                                {
                                    Favorites_lvFolderList.SelectedItem = Favorites_ocFolderList[0];
                                    Favorites_CurrentFolder = 0;
                                }
                            }
                        }
                        else
                        {
                            // There are no more items to delete, or no folders that 
                            // does not have the item to add, go to normal mode:
                            if (Favorites_CurrentFolder > -1 && Favorites_CurrentFolder < Favorites_ocFolderList.Count())
                            {
                                Favorites_lvFolderList.SelectedItem = Favorites_ocFolderList[Favorites_CurrentFolder];
                            }
                            else if (Favorites_ocFolderList.Count() > 0)
                            {
                                Favorites_lvFolderList.SelectedItem = Favorites_ocFolderList[0];
                                Favorites_CurrentFolder = 0;
                            }
                        }
                        if (Favorites_lvFolderList.SelectedItem == null)
                        {
                            if (Favorites_ocFolderList.Count > 0)
                            {
                                Favorites_lvFolderList.SelectedItem = Favorites_ocFolderList[0];
                                Favorites_CurrentFolder = 0;
                            }
                        }
                        UpdateFavoritesListFromCommonstate(Favorites_lvFolderList.SelectedItem.ToString());
                    }
                    catch { }
                    if (Favorites_ocFolderList.Count() > 0 && SelectedFolderIndex > -1)
                    {
                        Favorites_lvFolderList.SelectedItem = Favorites_ocFolderList[SelectedFolderIndex];
                        Favorites_CurrentFolder = SelectedFolderIndex;
                    }
                    else
                    {
                        Favorites_lvFolderList.SelectedItem = Favorites_ocFolderList[0];
                        Favorites_CurrentFolder = 0;
                    }
                }
                PopHandleControlEvents();
            }
        }

        private void UpdateFavoritesListFromCommonstate(String folderName/*, Int32 selectIndex = -1*/)
        {
            // Find the folder by name:
            FavoritesFolder favoritesFolder = null;
            for (Int32 i = 0; i < commonState.FavoritesList.FavoritesFolders.Count() && favoritesFolder == null; i++)
            {
                if (commonState.FavoritesList.FavoritesFolders[i].Name == folderName.TrimStart('*'))
                {
                    favoritesFolder = commonState.FavoritesList.FavoritesFolders[i];
                }
            }
            if (favoritesFolder != null)
            {
                Favorites_ocFavoriteList.Clear();
                Favorites_CurrentlyInFavoriteList.Clear();
                foreach (FavoriteTone fav in commonState.FavoritesList.FavoritesFolders[commonState.FavoritesList.FavoritesFolders.IndexOf(favoritesFolder)].FavoriteTones)
                {
                    Favorites_ocFavoriteList.Add(fav.Name);
                    Favorites_CurrentlyInFavoriteList.Add(fav);
                }
            }
        }

        private Tone FindFavoriteByNameAndFolder(String Name, String FolderName)
        {
            foreach (FavoritesFolder folder in commonState.FavoritesList.FavoritesFolders)
            {
                if (folder.Name == FolderName || ("* " + folder.Name) == FolderName)
                {
                    foreach (FavoriteTone favorite in folder.FavoriteTones)
                    {
                        if (favorite.Name == Name)
                        {
                            return new Tone(-1, -1, -1, favorite.Group, favorite.Category, favorite.Name);
                        }
                    }
                }
            }
            return null;
        }
    }
}
