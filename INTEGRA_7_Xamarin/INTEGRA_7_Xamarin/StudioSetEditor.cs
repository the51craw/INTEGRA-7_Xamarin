using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integra_7_Xamarin
{
    public partial class UIHandler
    {
        public void DrawStudioSetEditorPage()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Edit studio set
            // ____________________________________________________________________________________________
            // |                                                                                          |
            // |__________________________________________________________________________________________|

            // Create all controls ------------------------------------------------------------------------

            Button StudioSetEditor_NotYetImplemented = new Button();
            StudioSetEditor_NotYetImplemented.HorizontalOptions = LayoutOptions.FillAndExpand;
            StudioSetEditor_NotYetImplemented.VerticalOptions = LayoutOptions.FillAndExpand;
            StudioSetEditor_NotYetImplemented.Text = "Not yet implemented";
            StudioSetEditor_NotYetImplemented.Clicked += StudioSetEditor_NotYetImplemented_Clicked;
            Image image = new Image { Source = "MissingImage.png" };
            image.BackgroundColor = Color.Cyan;// += StudioSetEditor_NotYetImplemented_Clicked;
            image.MinimumHeightRequest = 100;
            image.MinimumWidthRequest = 20;
            image.VerticalOptions = LayoutOptions.FillAndExpand;
            image.HorizontalOptions = LayoutOptions.FillAndExpand;

            // Add handlers -------------------------------------------------------------------------------

            // Assemble grids with controls ---------------------------------------------------------------

            // Assemble StudioSetEditorStackLayout -----------------------------------------------------------------

            StudioSetEditorStackLayout = new StackLayout();
            StudioSetEditorStackLayout.Children.Add((new GridRow(0, new View[] { StudioSetEditor_NotYetImplemented, image })).Row);
            //Grid imgGrid = new Grid();
            //imgGrid.HorizontalOptions = LayoutOptions.FillAndExpand;
            //imgGrid.VerticalOptions = LayoutOptions.FillAndExpand;
            //image.Margin = new Thickness(5, 5, 5, 5);
            //imgGrid.Children.Add(image);
            //StudioSetEditorStackLayout.Children.Add(imgGrid);
        }

        private void StudioSetEditor_NotYetImplemented_Clicked(object sender, EventArgs e)
        {
            //mainStackLayout.Children.RemoveAt(0);
            StudioSetEditorStackLayout.IsVisible = false;
            ShowLibrarianPage();
        }

        public void ShowStudioSetEditorPage()
        {
            if (!StudioSetEditorIsCreated)
            {
                DrawStudioSetEditorPage();
                mainStackLayout.Children.Add(StudioSetEditorStackLayout);
                StudioSetEditorIsCreated = true;
            }
            //mainStackLayout.Children.Add(StudioSetEditorStackLayout);
            StudioSetEditorStackLayout.IsVisible = true;
        }
    }
 }
