using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EllipseAlphaTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private WriteableBitmap _bitmap;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PreviewImage_OnLoaded(object sender, RoutedEventArgs e)
        {
            _bitmap = BitmapFactory.New(500, 500);
            for (var y = 0; y < 500; ++y)
            {
                for (var x1 = 0; x1 < 250; ++x1)
                {
                    _bitmap.SetPixel(x1,y,WriteableBitmapExtensions.ConvertColor(Colors.DodgerBlue));
                    _bitmap.SetPixel(x1+250, y, WriteableBitmapExtensions.ConvertColor(Colors.SeaGreen));
                }
            }
            _bitmap.FillEllipseCentered(225, 225, 50, 50, WriteableBitmapExtensions.ConvertColor(0.5, Colors.Red), true);
            //_bitmap.FillRectangle(200, 200, 250, 250, WriteableBitmapExtensions.ConvertColor(0.5, Colors.Red), true);
            PreviewImage.Source = _bitmap;
        }
    }
}
