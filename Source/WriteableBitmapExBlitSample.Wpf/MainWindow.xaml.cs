using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WriteableBitmapExBlitSample.Wpf
{
    public partial class MainWindow
    {
        #region Fields

        WriteableBitmap bmp;
        WriteableBitmap circleBmp;
        WriteableBitmap particleBmp;
        Rect particleSourceRect;
        ParticleEmitter emitter = new ParticleEmitter();
        DateTime lastUpdate = DateTime.Now;
        private Stopwatch _stopwatch = Stopwatch.StartNew();
        private double _lastTime;
        private double _lowestFrameTime;

        #endregion

        #region Contructors

        public MainWindow()
        {
            InitializeComponent();

            particleBmp = LoadBitmap("/WriteableBitmapExBlitSample.Wpf;component/Data/FlowerBurst.jpg");
            circleBmp = LoadBitmap("/WriteableBitmapExBlitSample.Wpf;component/Data/circle.png");

            particleSourceRect = new Rect(0, 0, 64, 64);
            bmp = BitmapFactory.New(640, 480);
            bmp.Clear(Colors.Black);
            image.Source = bmp;
            emitter = new ParticleEmitter();
            emitter.TargetBitmap = bmp;
            emitter.ParticleBitmap = particleBmp;
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            this.MouseMove += new MouseEventHandler(MainPage_MouseMove);
        }

        #endregion

        #region Methods

        static WriteableBitmap LoadBitmap(string path)
        {
            using (var s = Application.GetResourceStream(new Uri(path, UriKind.Relative)).Stream)
            {
                var wb = BitmapFactory.FromStream(s);
                return BitmapFactory.ConvertToPbgra32Format(wb);
            }
        }

        #endregion

        #region Eventhandler

        void MainPage_MouseMove(object sender, MouseEventArgs e)
        {
            emitter.Center = e.GetPosition(image);
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
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

        #endregion
    }
}
