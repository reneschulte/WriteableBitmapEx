#region Header
//
//   Project:           WriteableBitmapEx - WriteableBitmap extensions
//
//   Changed by:        $Author: unknown $
//   Changed on:        $Date: 2015-02-24 20:36:41 +0100 (Di, 24 Feb 2015) $
//   Changed in:        $Revision: 112951 $
//   Project:           $URL: https://writeablebitmapex.svn.codeplex.com/svn/trunk/Source/WriteableBitmapExCurvesSample.WinRT/MainPage.xaml.cs $
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
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using Windows.Devices.Input;
using WriteableBitmapExCurveSample;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WriteableBitmapExCurvesSample.WinRT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int PointHitZoneSize = 34;
        private const int PointHitZoneSizeHalf = PointHitZoneSize >> 1;
        private const int PointVisualSize = 20;
        private const int PointVisualSizeHalf = PointVisualSize >> 1;

        private WriteableBitmap writeableBmp;
        private List<ControlPoint> points;
        private ControlPoint PickedPoint;

        public float Tension { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Init()
        {
            // Show fps counter
            Application.Current.DebugSettings.EnableFrameRateCounter = true;

            // Init vars
            points = new List<ControlPoint>();
            Tension = 0.5f;

            this.DataContext = this;

            // Init WriteableBitmap
            writeableBmp = BitmapFactory.New((int)(this.ActualWidth - ToolPanel.ActualWidth), (int)this.ActualHeight);
            Viewport.Source = writeableBmp;

            //// Test for FromContent
            //var wb = await BitmapFactory.New(1, 1).FromContent(new Uri(BaseUri, @"///assets/logo.png"));
            //wb.DrawLine(10, 10, 20, 20, Colors.Green);
            //Viewport.Source = wb;          
        }

        private void Draw()
        {
            if (this.points != null && this.writeableBmp != null)
            {
                // Wrap updates in a GetContext call, to prevent invalidation and nested locking/unlocking during this block
                using (writeableBmp.GetBitmapContext())
                {
                    writeableBmp.Clear();
                    if (ChkShowPoints.IsChecked.Value)
                    {
                        DrawPoints();
                    }

                    if (RBBezier.IsChecked.Value)
                    {
                        DrawBeziers();
                    }
                    else if (RBCardinal.IsChecked.Value)
                    {
                        DrawCardinal();
                    }

                    // Invalidates on exit of using block
                }
            }
        }

        private void DrawPoints()
        {
            foreach (var p in points)
            {
                DrawPoint(p, Colors.Green, PointVisualSizeHalf);
            }
            if (PickedPoint != null)
            {
                DrawPoint(PickedPoint, Colors.White, PointHitZoneSizeHalf);
            }
        }

        private void DrawPoint(ControlPoint p, Color color, int halfSizeOfPoint)
        {
            var x1 = p.X - halfSizeOfPoint;
            var y1 = p.Y - halfSizeOfPoint;
            var x2 = p.X + halfSizeOfPoint;
            var y2 = p.Y + halfSizeOfPoint;
            writeableBmp.DrawRectangle(x1, y1, x2, y2, color);
        }

        private void DrawBeziers()
        {
            if (points.Count > 3)
            {
                writeableBmp.DrawBeziers(GetPointArray(), Colors.Yellow);
            }
        }

        private void DrawCardinal()
        {
            if (points.Count > 2)
            {
                writeableBmp.DrawCurve(GetPointArray(), Tension, Colors.Yellow);
            }
        }

        private int[] GetPointArray()
        {
            int[] pts = new int[points.Count * 2];
            for (int i = 0; i < points.Count; i++)
            {
                pts[i * 2] = points[i].X;
                pts[i * 2 + 1] = points[i].Y;
            }
            return pts;
        }

        private ControlPoint GetMousePoint(PointerRoutedEventArgs e)
        {
            return new ControlPoint(e.GetCurrentPoint(Viewport).Position);
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Viewport_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            // Pick control point
            var mp = GetMousePoint(e);
            PickedPoint = (from p in points
                           where p.X > mp.X - PointHitZoneSizeHalf && p.X < mp.X + PointHitZoneSizeHalf
                              && p.Y > mp.Y - PointHitZoneSizeHalf && p.Y < mp.Y + PointHitZoneSizeHalf
                           select p).FirstOrDefault();
            Draw();
        }

        private void Viewport_PointerReleased_1(object sender, PointerRoutedEventArgs e)
        {
            // Only add new control point if [DEL] wasn't pressed
            if (PickedPoint == null)
            {
                points.Add(GetMousePoint(e));
            }
            PickedPoint = null;
            Draw();
        }

        private void Viewport_PointerMoved_1(object sender, PointerRoutedEventArgs e)
        {
            // Move control point
            if (PickedPoint != null)
            {
                var mp = GetMousePoint(e);
                PickedPoint.X = mp.X;
                PickedPoint.Y = mp.Y;
                Draw();
            }
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (this.points != null && points.Count > 0)
            {
                points.RemoveAt(points.Count - 1);
            }
            Draw();
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {

            // Remove all comtrol points
            if (this.points != null)
            {
                this.points.Clear();
                Draw();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Refresh
            Draw();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Tension only makes sense for cardinal splines
            if (RBCardinal != null)
            {
                if (RBCardinal.IsChecked.Value)
                {
                    SldTension.Opacity = 1;
                }
                else
                {
                    SldTension.Opacity = 0;
                }
            }
            Draw();
        }

        private void Slider_ValueChanged_1(object sender, RangeBaseValueChangedEventArgs e)
        {
            // Set tension text
            if (this.TxtTension != null)
            {
                this.TxtTension.Text = String.Format("Tension: {0:f2}", Tension);
                Draw();
            }
        }
    }
}
