using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WriteableBitmapEx.TextExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Draw();
        }

        WriteableBitmap bmp;

        private void Draw()
        {
            var unmodifiedBmp = bmp = LoadFromFile("Assets/Overlays/19.jpg");

            FormattedText formattedText;

            {
                System.Windows.FontStyle fontStyle = FontStyles.Normal;
                FontWeight fontWeight = FontWeights.Medium;

                var Text = "Now supports text!";

                var FontSize = 80;

                var Font = new FontFamily("Sans MS");

                // Create the formatted text based on the properties set.
                formattedText = new FormattedText(
                    Text,
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface(
                        Font,
                        fontStyle,
                        fontWeight,
                        FontStretches.Normal),
                    FontSize,
                    System.Windows.Media.Brushes.Black // This brush does not matter since we use the geometry of the text.
                    );
            }

            
            bmp.DrawTextAa(formattedText, 0, 100, Colors.Black,3);
            bmp.FillText(formattedText, 0, 100, Colors.Orange);


            imgMain.Source = bmp;
            //bmp.DrawTextAa()
        }


        public static WriteableBitmap LoadFromFile(string fileName)
        {
            using (var fileStream = File.OpenRead(fileName))
            {
                var wb = BitmapFactory.FromStream(fileStream);
                return wb;
            }
        }
    }
}
