﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;

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
        Button Favorites_btnCopyFavorite = null;
        Button Favorites_btnAddOrDeleteFavorite = null;
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
            Favorites_tbHelp.BackgroundColor = colorSettings.ControlBackground;

            Favorites_btnCopyFavorite = new Button();
            Favorites_btnCopyFavorite.Text = "Copy";

            Favorites_btnAddOrDeleteFavorite = new Button();
            Favorites_btnAddOrDeleteFavorite.Text = "Delete favorite";

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
            Favorites_btnCopyFavorite.Clicked += Favorites_btnCopyFavorite_Clicked;
            Favorites_btnDeleteFolder.Clicked += Favorites_btnDeleteFolder_Clicked;
            Favorites_btnAddOrDeleteFavorite.Clicked += Favorites_btnAddOrDeleteFavorite_Clicked;
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
            Favorites_grRightColumn.Children.Add(new GridRow(0, new View[] { gNewFolder }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(1, new View[] { Favorites_btnDeleteFolder }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(2, new View[] { Favorites_tbHelp }, null, false, false).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(3, new View[] { Favorites_btnCopyFavorite }, null, false, false).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(4, new View[] { Favorites_btnAddOrDeleteFavorite }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(5, new View[] { Favorites_btnSelectFavorite }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(6, new View[] { Favorites_btnPlay }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(7, new View[] { Favorites_btnBackup }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(8, new View[] { Favorites_btnRestore }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(9, new View[] { Favorites_btnReturn }).Row);

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
            //t.Trace("private void lvFolders_SelectionChanged(" + ((ListView)sender).SelectedIndex.ToString() + ")");
            PushHandleControlEvents();
            Favorites_btnDeleteFolder.IsEnabled = true;
            Favorites_btnCopyFavorite.IsEnabled = false;
            Favorites_btnAddOrDeleteFavorite.IsEnabled = false;
            Favorites_btnSelectFavorite.IsEnabled = false;
            //UpdateFavoritesList((String)(((ListView)sender).SelectedItem));
            UpdateFavoritesList((String)sender);
            PopHandleControlEvents();
            Favorites_btnDeleteFolder.IsEnabled = true;
            Favorites_CurrentFolder = Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem);
            if (favoritesAction == FavoritesAction.ADD || favoritesAction == FavoritesAction.REMOVE)
            {
                if (Favorites_lvFolderList.SelectedItem.ToString().StartsWith("*"))
                {
                    Favorites_btnAddOrDeleteFavorite.IsEnabled = true;
                }
                else
                {
                    Favorites_btnAddOrDeleteFavorite.IsEnabled = false;
                }
            }
            if (Favorites_lvFolderList.SelectedItem != null)
            {
                Favorites_btnDeleteFolder.IsEnabled = true;
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
                    commonState.FavoritesList.folders[Favorites_ocFolderList
                        .IndexOf(Favorites_lvFolderList.SelectedItem)]
                        .FavoriteTones.Add(new FavoriteTone(
                            commonState.CurrentTone.Name,
                            commonState.CurrentTone.Group,
                            commonState.CurrentTone.Category));
                    Favorites_lvFolderList.SelectedItem = 
                        ((String)Favorites_lvFolderList.SelectedItem).TrimStart('*');
                    UpdateFavoritesList((String)Favorites_lvFolderList.SelectedItem);
                    SaveToLocalSettings();
                }
                if (favoritesAction == FavoritesAction.REMOVE && ((String)Favorites_lvFolderList.SelectedItem).StartsWith("*"))
                {
                    FavoriteTone tone = commonState.FavoritesList.folders[Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem)].ByToneName(commonState.CurrentTone.Name);
                    commonState.FavoritesList.folders[Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem)].FavoriteTones.Remove(tone);
                    Favorites_lvFolderList.SelectedItem = ((String)Favorites_lvFolderList.SelectedItem).TrimStart('*');
                    UpdateFavoritesList((String)Favorites_lvFolderList.SelectedItem);
                    SaveToLocalSettings();
                }
            }
        }

        // Favorites listview handlers ----------------------------------------------------------------------------------
        private void Favorites_lvFavoriteList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (handleControlEvents)
            {
                Favorites_btnDeleteFolder.IsEnabled = false;
                Favorites_btnCopyFavorite.IsEnabled = true;
                Favorites_btnAddOrDeleteFavorite.IsEnabled = true;
                Favorites_btnSelectFavorite.IsEnabled = true;
                Favorites_btnPlay.IsEnabled = true;
                FavoriteTone favoriteTone;
                try
                {
                    // List only contains the name, lookup the currentTone by name and folder:
                    favoriteTone = Favorites_CurrentlyInFavoriteList
                        .Find(f => f.Name == (String)Favorites_lvFavoriteList.SelectedItem);
                    commonState.CurrentTone = new Tone(-1, -1, -1, favoriteTone.Group, favoriteTone.Category, favoriteTone.Name);
                    // The user came here to browse favorites, and has now selected one.
                    // The favorite should be selectable via the 'Select <name>' button.
                    if (commonState.CurrentTone != null && favoritesAction == FavoritesAction.SHOW)
                    {
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
                    // The favorite should be allowed to be stored in another folder.
                    else if (favoritesAction == FavoritesAction.ADD)
                    {
                        Favorites_btnAddOrDeleteFavorite.Text = "Copy " + favoriteTone.Name;
                        Favorites_btnAddOrDeleteFavorite.IsVisible = true;
                        Favorites_btnAddOrDeleteFavorite.IsEnabled = true;
                        Favorites_tbHelp.Text = "You have selected a favorite and it is now possible to copy it to another " +
                            "folder. Select the destination folder and clic the \'Copy " + commonState.CurrentTone.Name + " button.";
                        //UpdateFoldersList();
                    }
                    // The user came here to delete a favorite, and now selects another favorite.
                    // The favorite should be allowed to be deleted.
                    else if (favoritesAction == FavoritesAction.REMOVE)
                    {
                        Favorites_btnAddOrDeleteFavorite.Text = "Delete " + favoriteTone.Name;
                        Favorites_btnAddOrDeleteFavorite.IsVisible = true;
                        Favorites_btnAddOrDeleteFavorite.IsEnabled = true;
                        Favorites_UpdateFoldersList();
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
                //        t.Trace("private void lvFavorites_DoubleTapped(" + ((ListView)sender).SelectedIndex.ToString() + ")");
                //        try
                //        {
                //            // List only contains the name, lookup the currentTone by name and folder:
                //            commonState.currentTone = (Tone)lvFavorites.Items[lvFavorites.SelectedIndex];
                //            commonState.currentTone.Index = commonState.toneList.Get(commonState.currentTone);
                //        }
                //        catch
                //        {
                //            commonState.currentTone.Index = -1;
                //        }
                //        // If the user came here to pick a favorite, it is now selected.
                //        // Just return to the main page:
                //        if (favoritesAction == FavoritesAction.SHOW)
                //        {
                //            this.Frame.Navigate(typeof(MainPage), commonState);
                //        }
                //        // If the user came here to add a favorite, selecting one would be to get it.
                //        // Just get it and return to the main page:
                //        else if (favoritesAction == FavoritesAction.ADD)
                //        {
                //            if (commonState.currentTone.Index < 0)
                //            {
                //                // List only contains the name, lookup the currentTone by name and folder:
                //                commonState.currentTone = FindFavoriteByNameAndFolder((String)lvFavorites.SelectedItem, ((String)lvFolders.SelectedItem).TrimStart('*'));
                //            }
                //            this.Frame.Navigate(typeof(MainPage), commonState);
                //        }
                //        // If the user came here to delete a favorite, selecting another would be for
                //        // deleting that one too (because of right tap or double click:
                //        else if (favoritesAction == FavoritesAction.REMOVE)
                //        {
                //            // List only contains the name, lookup the currentTone by name and folder:
                //            Tone tone = FindFavoriteByNameAndFolder((String)lvFavorites.SelectedItem, ((String)lvFolders.SelectedItem).TrimStart('*'));
                //            DeleteFavorite(tone);
                //            UpdateFoldersList();
                //        }
            }
        }

        // Add/delete folder controls handlers --------------------------------------------------------------------------
        private void Favorites_edNewFolderName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //t.Trace("private void tbNewFolder_KeyUp (" + "object" + sender + ", " + "KeyRoutedEventArgs" + e + ", " + ")");
            if (!String.IsNullOrEmpty(Favorites_edNewFolderName.Text))
            {
                Boolean found = false;
                if (commonState.FavoritesList != null && commonState.FavoritesList.folders != null && commonState.FavoritesList.folders.Count() > 0)
                {
                    foreach (FavoritesFolder folder in commonState.FavoritesList.folders)
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
                            commonState.FavoritesList.folders.Add(new FavoritesFolder(Favorites_edNewFolderName.Text.Trim().Replace("\r", "")));
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
                commonState.FavoritesList.folders.Add(new FavoritesFolder(Favorites_edNewFolderName.Text.Trim()));
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
        private void Favorites_btnCopyFavorite_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Favorites_btnAddOrDeleteFavorite_Clicked(object sender, EventArgs e)
        {
            if (commonState.CurrentTone != null && Favorites_lvFolderList.SelectedItem != null)
            {
                //t.Trace("private void btnContext_Click (" + "object" + sender + ", " + "RoutedEventArgs" + e + ", " + ")");
                //ListViewItem item = (ListViewItem)lvFolders.ContainerFromItem(lvFolders.Items[lvFolders.SelectedIndex]);
                if (favoritesAction == FavoritesAction.ADD && ((String)Favorites_lvFolderList.SelectedItem).StartsWith("*"))
                {
                    Int32 index = Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem);
                    if (index > -1)
                    {
                        commonState.FavoritesList.folders[index].FavoriteTones
                            .Add(new FavoriteTone(
                                commonState.CurrentTone.Group,
                                commonState.CurrentTone.Category,
                                commonState.CurrentTone.Name));
                        Favorites_ocFolderList[Favorites_CurrentFolder] = ((String)Favorites_lvFolderList.SelectedItem).TrimStart('*');
                        Favorites_lvFolderList.SelectedItem = Favorites_ocFolderList[Favorites_CurrentFolder];
                        Favorites_UpdateFoldersList();
                        UpdateFavoritesList((String)Favorites_lvFolderList.SelectedItem);
                        SaveToLocalSettings();
                    }
                }
                else if (favoritesAction == FavoritesAction.REMOVE)
                {
                    DeleteFavorite(commonState.CurrentTone);
                }
                else if (favoritesAction == FavoritesAction.SHOW)
                {
                    //FindFavoriteByNameAndFolder()
                    FavoriteTone favoriteTone = Favorites_CurrentlyInFavoriteList[Favorites_ocFavoriteList
                        .IndexOf(Favorites_lvFavoriteList.SelectedItem)];
                    commonState.CurrentTone = new Tone(-1, -1, -1, favoriteTone.Group, favoriteTone.Category, favoriteTone.Name);
                        
                    //commonState.currentTone = (Tone)Favorites_lvFavoriteList.SelectedItem;
                    //mainStackLayout.Children.RemoveAt(0);
                    Favorites_StackLayout.IsVisible = false;
                    ShowLibrarianPage();
                }
            }
        }

        private void Favorites_btnSelectFavorite_Clicked(object sender, EventArgs e)
        {
            Librarian_SynchronizeListviews();
            Favorites_StackLayout.IsVisible = false;
            ShowLibrarianPage();
        }

        private async void Favorites_btnRestore_Clicked(object sender, EventArgs e)
        {
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();
                byte[] data = fileData.DataArray;
                String favorites = "";
                for (Int32 i = 0; i < data.Length; i++)
                {
                    favorites += (char)data[i];
                }
                Favorites_Restore(favorites);
            }
            catch { }
        }

        // Backup/restore button handlers -------------------------------------------------------------------------------
        private void Favorites_btnBackup_Clicked(object sender, EventArgs e)
        {
            t.Trace("private void btnSave_Click (" + "object" + sender + ", " + "RoutedEventArgs" + e + ", " + ")");
            //Save();
        }

        //private void Favorites_btnPlay_Clicked(object sender, EventArgs e)
        //{
        //    commonState.player.Play();
        //    if (commonState.player.Playing)
        //    {
        //        Favorites_btnPlay.Text = "Stop";
        //    }
        //    else
        //    {
        //        Favorites_btnPlay.Text = "Play";
        //    }
        //}

        // Return to Librarian button handler ---------------------------------------------------------------------------
        private void Favorites_btnReturn_Clicked(object sender, EventArgs e)
        {
            //mainStackLayout.Children.RemoveAt(0);
            Favorites_StackLayout.IsVisible = false;
            ShowLibrarianPage();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Public functions to be called from code in other files
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ShowFavoritesPage(FavoritesAction favoriteAction)
        {
            this.favoritesAction = favoriteAction;
            //if (!Favorites_IsCreated)
            //{
            //    DrawFavoritesPage();
            //    mainStackLayout.Children.Add(Favorites_StackLayout);
            //    Favorites_IsCreated = true;
            //}
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

            Favorites_btnSelectFavorite.IsEnabled = false;
            Favorites_btnDeleteFolder.IsEnabled = false;
            Favorites_btnCopyFavorite.IsEnabled = false;
            Favorites_btnAddOrDeleteFavorite.IsEnabled = true;
            Favorites_btnSelectFavorite.IsEnabled = false;

            switch (favoritesAction)
            {
                case FavoritesAction.SHOW:
                Favorites_UpdateFoldersList();
                    //if (Favorites_ocFolderList.Count() > 0)
                    //{
                    //    Favorites_lvFolderList.SelectedItem = commonState.currentTone.
                    //    UpdateFavoritesList(lvFolders.SelectedIndex);
                    //}
                    break;
                case FavoritesAction.ADD:
                    Favorites_tbHelp.Text = "The folder(s) that does not already contain the Tone " + commonState.CurrentTone.Name +
                        " has been marked with a \'*\'. Doubletap the folders you wish to add the Tone \'" +
                        commonState.CurrentTone.Name + "\' to (or select the folder name and click 'Add " +
                        commonState.CurrentTone.Name + "').";
                    Favorites_btnAddOrDeleteFavorite.Content = "Add " + commonState.CurrentTone.Name;
                    Favorites_UpdateFoldersList();
                    break;
                case FavoritesAction.REMOVE:
                Favorites_tbHelp.Text = "The folder(s) that contains the Tone " + commonState.CurrentTone.Name +
                    " has been marked with a \'*\'. Doubletap the folders you wish to remove the Tone \'" +
                    commonState.CurrentTone.Name + "\' from (or select the folder name and click 'Delete " +
                    commonState.CurrentTone.Name + "').";
                Favorites_btnAddOrDeleteFavorite.Content = "Delete " + commonState.CurrentTone.Name;
                Favorites_UpdateFoldersList();
                    break;
            }
            //else if (favoritesAction == FavoritesAction.SHOW)
            //{
            //    tbContextInstructions.Text = "Please select a folder and double-click a favorite to select it.\r\n\r\n" +
            //        "You can also click it and then click the \'Select\'button.\r\n\\r\n." +
            //        "Press play if you want to hear a sample of the selected tone before you go back to the librarian.";
            //}
            //commonState.player = new Player(commonState, ref Favorites_btnPlay);
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
                commonState.FavoritesList.folders.RemoveAt(Favorites_ocFolderList.IndexOf(folder));
                SaveToLocalSettings();
                Favorites_UpdateFoldersList();
            }
        }

        private void DeleteFavorite(Tone Tone)
        {
            t.Trace("private void DeleteFavorite (" + Tone.Name + ")");
            UInt16 i = 0;
            UInt16 index = 0;
            Int32 folderIndex = Favorites_ocFolderList.IndexOf(Favorites_lvFolderList.SelectedItem);
            Boolean found = false;
            foreach (FavoriteTone tone in commonState.FavoritesList.folders[folderIndex].FavoriteTones)
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
                commonState.FavoritesList.folders[folderIndex].FavoriteTones.RemoveAt(index);
                UpdateFavoritesList((String)Favorites_lvFolderList.SelectedItem);
                Favorites_UpdateFoldersList(folderIndex);
            }
        }

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    try
        //    {
        //        commonState = (CommonState)e.Parameter;
        //        if (commonState.favoritesList == null)
        //        {
        //            commonState.favoritesList = new FavoritesList();
        //            commonState.favoritesList.folders = new List<FavoritesFolder>();
        //            try
        //            {
        //                if (localSettings.Values.ContainsKey("Favorites"))
        //                {
        //                    String foldersWithFavorites = ((String)localSettings.Values["Favorites"]);
        //                    UnpackFoldersWithFavoritesString(foldersWithFavorites);
        //                }
        //            }
        //            catch
        //            {
        //                localSettings.Values.Remove("Favorites");
        //            }
        //        }
        //        commonState.player.btnPlayStop = btnPlayStop;
        //        if (commonState.player.Playing)
        //        {
        //            btnPlayStop.Content = "Stop";
        //        }
        //        if (favoritesAction == FavoritesAction.SHOW)
        //        {
        //            UpdateFoldersList();
        //            if (lvFolders.Items.Count() > 0)
        //            {
        //                lvFolders.SelectedIndex = 0;
        //                UpdateFavoritesList(lvFolders.SelectedIndex);
        //            }
        //        }
        //        if (favoritesAction == FavoritesAction.ADD)
        //        {
        //            tbContextInstructions.Text = "The folder(s) that does not already contain the Tone " + commonState.currentTone.Name +
        //                " has been marked with a \'*\'. Doubletap the folders you wish to add the Tone \'" +
        //                commonState.currentTone.Name + "\' to (or select the folder name and click 'Add " +
        //                commonState.currentTone.Name + "').";
        //            Favorites_btnContext.Content = "Add " + commonState.currentTone.Name;
        //            Favorites_btnContext.IsEnabled = false;
        //            UpdateFoldersList();
        //        }
        //        else if (favoritesAction == FavoritesAction.REMOVE)
        //        {
        //            tbContextInstructions.Text = "The folder(s) that contains the Tone " + commonState.currentTone.Name +
        //                " has been marked with a \'*\'. Doubletap the folders you wish to remove the Tone \'" +
        //                commonState.currentTone.Name + "\' from (or select the folder name and click 'Delete " +
        //                commonState.currentTone.Name + "').";
        //            Favorites_btnContext.Content = "Delete " + commonState.currentTone.Name;
        //            Favorites_btnContext.IsEnabled = false;
        //            UpdateFoldersList();
        //        }
        //        else if (favoritesAction == FavoritesAction.SHOW)
        //        {
        //            tbContextInstructions.Text = "Please select a folder and double-click a favorite to select it.\r\n\r\n" +
        //                "You can also click it and then click the \'Select\'button.\r\n\\r\n." +
        //                "Press play if you want to hear a sample of the selected tone before you go back to the librarian.";
        //        }
        //        commonState.player = new Player(commonState, ref btnPlayStop);
        //    }
        //    catch { }
        //}

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
                commonState.FavoritesList.folders.Clear();
                foreach (String foldersWithFavoritePart in foldersWithFavorites.Split('\f'))
                {
                    String[] folderWithFavorites = foldersWithFavoritePart.Split('\v');
                    // Folder name:
                    folder = new FavoritesFolder(folderWithFavorites[0]);
                    commonState.FavoritesList.folders.Add(folder);
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
            foreach (FavoritesFolder folder in commonState.FavoritesList.folders)
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
            mainPage.SaveLocalValue("Favorites", toSave);
            //localSettings.Values["Favorites"] = toSave;
        }

        private void Favorites_UpdateFoldersList(Int32 SelectedFolderIndex = -1)
        {
            if (handleControlEvents)
            {
                t.Trace("private void UpdateFoldersList(SelectedIndex = " + SelectedFolderIndex.ToString() + ")");
                PushHandleControlEvents();
                try
                {
                    Int32 count = 0;
                    Favorites_ocFavoriteList.Clear(); // Since we will not have a selected folder, do not show favorites!
                    Favorites_CurrentlyInFavoriteList.Clear();
                    //Int32 selectedItemIndex = -1;
                    Favorites_btnDeleteFolder.IsEnabled = false;
                    Favorites_ocFolderList.Clear();
                    //Favorites_lvGroupList.ItemSelected -= Favorites_lvGroupList_ItemSelected;
                    //Favorites_lvGroupList.DoubleTapped -= Favorites_lvGroupList_DoubleTapped;
                    //Favorites_lvGroupList = new ListView();
                    //Favorites_lvGroupList.Name = "Favorites_lvGroupList";
                    //Favorites_lvGroupList.Margin = new Thickness(2, 2, 2, 2);
                    //Favorites_lvGroupList.BorderThickness = new Thickness(1);
                    //Favorites_lvGroupList.BorderBrush = black;
                    //Favorites_lvGroupList.Visibility = Visibility.Visible;
                    //Favorites_lvGroupList.SelectionChanged += Favorites_lvGroupList_SelectionChanged;
                    //Favorites_lvGroupList.DoubleTapped += Favorites_lvGroupList_DoubleTapped;
                    //Favorites_ocGroupList gcFolders.Children.Add(Favorites_lvGroupList);
                    UInt16 i = 0;
                    foreach (FavoritesFolder folder in commonState.FavoritesList.folders)
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
                    //PopHandleControlEvents();
                    if (Favorites_lvFolderList.SelectedItem == null)
                    {
                        if (Favorites_ocFolderList.Count > 0)
                        {
                            Favorites_lvFolderList.SelectedItem = Favorites_ocFolderList[0];
                            Favorites_CurrentFolder = 0;
                        }
                    }
                    UpdateFavoritesList(Favorites_lvFolderList.SelectedItem.ToString());
                    //Favorites_lvFolderList.ItemsSource = Favorites_ocFolderList;
                }
                catch { }
                if (Favorites_ocFolderList.Count() > 0)
                {
                    Favorites_lvFolderList.SelectedItem = Favorites_ocFolderList[0];
                    Favorites_CurrentFolder = 0;
                }
                PopHandleControlEvents();
            }
        }

        private void UpdateFavoritesList(String folderName)
        {
            //if (handleControlEvents)
            {
                //    t.Trace("private void UpdateFavoritesList(folderIndex = " + folderIndex.ToString() + ")");
                //    if (folderIndex > -1 && folderIndex < commonState.favoritesList.folders.Count())
                //    {
                // Find the folder by name:
                FavoritesFolder favoritesFolder = null;
                for (Int32 i = 0; i < commonState.FavoritesList.folders.Count() && favoritesFolder == null; i++)
                {
                    if (commonState.FavoritesList.folders[i].Name == folderName.TrimStart('*'))
                    {
                        favoritesFolder = commonState.FavoritesList.folders[i];
                    }
                }
                if (favoritesFolder != null)
                {
                    Favorites_ocFavoriteList.Clear();
                    Favorites_CurrentlyInFavoriteList.Clear();
                    foreach (FavoriteTone fav in commonState.FavoritesList.folders[commonState.FavoritesList.folders.IndexOf(favoritesFolder)].FavoriteTones)
                    {
                        Favorites_ocFavoriteList.Add(fav.Name);
                        Favorites_CurrentlyInFavoriteList.Add(fav);
                    }
                }
            }
        }

        private Tone FindFavoriteByNameAndFolder(String Name, String FolderName)
        {
            foreach (FavoritesFolder folder in commonState.FavoritesList.folders)
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
