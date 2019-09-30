using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WriteableBitmapExBlitSample.Uwp
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            particleBmp = await LoadBitmap("///Assets/FlowerBurst.jpg");
            circleBmp = await LoadBitmap("///Assets/circle.png");

            particleSourceRect = new Rect(0, 0, 64, 64);
            var w = (int)image.Width;
            var h = (int)image.Height;
            bmp = BitmapFactory.New(w, h);
            bmp.Clear(Colors.Black);
            image.Source = bmp;
            emitter = new ParticleEmitter
            {
                TargetBitmap = bmp,
                ParticleBitmap = particleBmp
            };
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            PointerMoved += MainPage_PointerMoved;
        }

        async Task<WriteableBitmap> LoadBitmap(string path)
        {
            Uri imageUri = new Uri(BaseUri, path);
            var bmp = await BitmapFactory.FromContent(imageUri);
            return bmp;
        }

        private void MainPage_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            emitter.Center = e.GetCurrentPoint(image).Position;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            // Wrap updates in a GetContext call, to prevent invalidation and nested locking/unlocking during this block
            // NOTE: This is not strictly necessary for the UWP version as this is a WPF feature, however we include it here for completeness and to show
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
