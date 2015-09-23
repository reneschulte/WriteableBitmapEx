using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Windows.Storage;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Resources;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var unmodifiedBmp = await LoadFromUri("Assets/Overlays/19.jpg");
            var sticker = await LoadFromUri("Assets/Overlays/nEW.png");
            ImgOrg.Source = unmodifiedBmp;
            ImgOrgOverlay.Source = sticker;

            var modifiedBmp = Overlay(unmodifiedBmp, sticker, new Point(10, 10));

            ImgMod.Source = modifiedBmp;
        }

        public static WriteableBitmap Overlay(WriteableBitmap bmp, WriteableBitmap overlay, Point location)
        {
            var result = bmp.Clone();
            var size = new Size(overlay.PixelWidth, overlay.PixelHeight);
            result.Blit(new Rect(location, size), overlay, new Rect(new Point(0, 0), size), WriteableBitmapExtensions.BlendMode.Alpha);
            return result;
        }

        public static async Task<WriteableBitmap> LoadFromUri(string path)
        {
            return new WriteableBitmap(1, 1).FromContent(path);
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}