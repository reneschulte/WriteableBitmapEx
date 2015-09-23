using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WriteableBitmapExBlitAlphaRepro.WinRT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var unmodifiedBmp = await LoadFromUri(new Uri("ms-appx:///Assets/Overlays/19.jpg"));
            var sticker = await LoadFromUri(new Uri("ms-appx:///Assets/Overlays/nEW.png"));
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

        public static async Task<WriteableBitmap> LoadFromUri(Uri uri)
        {
            // Fix URI. Sometimes only one / is provided
            var us = uri.OriginalString;
            var match = Regex.Match(us, "(\\:\\/)[a-zA-Z0-9]+?");
            if (match.Success)
            {
                uri = new Uri(us.Replace(match.Groups[1].Value, ":///"));
            }
            var file = await StorageFile.GetFileFromApplicationUriAsync(uri);

            if (file == null)
            {
                return null;
            }

            using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                var wb = await new WriteableBitmap(1, 1).FromStream(fileStream);
                return wb;
            }
        }
    }
}
