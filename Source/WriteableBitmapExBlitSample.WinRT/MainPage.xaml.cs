#region Header
//
//   Project:           WriteableBitmapEx - WriteableBitmap extensions
//
//   Changed by:        $Author: unknown $
//   Changed on:        $Date: 2015-02-24 20:36:41 +0100 (Di, 24 Feb 2015) $
//   Changed in:        $Revision: 112951 $
//   Project:           $URL: https://writeablebitmapex.svn.codeplex.com/svn/trunk/Source/WriteableBitmapExBlitSample.WinRT/MainPage.xaml.cs $
//   Id:                $Id: MainPage.xaml.cs 112951 2015-02-24 19:36:41Z unknown $
//
//
//   Copyright © 2009-2015 Rene Schulte and WriteableBitmapEx Contributors
//
//   This code is open source. Please read the License.txt for details. No worries, we won't sue you! ;)
//
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WriteableBitmapExBlitSample.WinRT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        WriteableBitmap bmp;
        WriteableBitmap circleBmp;
        WriteableBitmap particleBmp;
        Rect particleSourceRect;
        ParticleEmitter emitter = new ParticleEmitter();
        DateTime lastUpdate = DateTime.Now;
        private Stopwatch _stopwatch = Stopwatch.StartNew();
        private double _lastTime;
        private double _lowestFrameTime;


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
            particleBmp = await LoadBitmap("///Assets/FlowerBurst.jpg");
            circleBmp = await LoadBitmap("///Assets/circle.png");

            particleSourceRect = new Rect(0, 0, 64, 64);
            bmp = BitmapFactory.New(640, 480);
            bmp.Clear(Colors.Black);
            image.Source = bmp;
            emitter = new ParticleEmitter();
            emitter.TargetBitmap = bmp;
            emitter.ParticleBitmap = particleBmp;
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            this.PointerMoved += MainPage_PointerMoved;
        }

        async Task<WriteableBitmap> LoadBitmap(string path)
        {
            Uri imageUri = new Uri(BaseUri, path);
            var bmp = await BitmapFactory.New(1, 1).FromContent(imageUri);
            return bmp;
        }

        private void MainPage_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            emitter.Center = e.GetCurrentPoint(image).Position;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            // Wrap updates in a GetContext call, to prevent invalidation and nested locking/unlocking during this block
            // NOTE: This is not strictly necessary for the SL version as this is a WPF feature, however we include it here for completeness and to show
            // a similar API to WPF
            using (bmp.GetBitmapContext())
            {
                bmp.Clear(Colors.Black);

                double elapsed = (DateTime.Now - lastUpdate).TotalSeconds;
                lastUpdate = DateTime.Now;
                emitter.Update(elapsed);
                //			bmp.Blit(new Point(100, 150), circleBmp, new Rect(0, 0, 200, 200), Colors.Red, BlendMode.Additive);
                //			bmp.Blit(new Point(160, 55), circleBmp, new Rect(0, 0, 200, 200), Color.FromArgb(255, 0, 255, 0), BlendMode.Additive);
                //			bmp.Blit(new Point(220, 150), circleBmp, new Rect(0, 0, 200, 200), Colors.Blue, BlendMode.Additive);

                double timeNow = _stopwatch.ElapsedMilliseconds;
                double elapsedMilliseconds = timeNow - _lastTime;
                _lowestFrameTime = Math.Min(_lowestFrameTime, elapsedMilliseconds);
                FpsCounter.Text = string.Format("FPS: {0:0.0} / Max: {1:0.0}", 1000.0 / elapsedMilliseconds, 1000.0 / _lowestFrameTime);
                _lastTime = timeNow;
            }
        }
    }
}
