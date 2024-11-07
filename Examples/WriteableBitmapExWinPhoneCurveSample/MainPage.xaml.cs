#region Header
//
//   Project:           WriteableBitmapEx - Silverlight WriteableBitmap extensions
//   Description:       Main Page of WinPhone Curve demo.
//
//   Changed by:        $Author: unknown $
//   Changed on:        $Date: 2015-02-24 20:36:41 +0100 (Di, 24 Feb 2015) $
//   Changed in:        $Revision: 112951 $
//   Project:           $URL: https://writeablebitmapex.svn.codeplex.com/svn/trunk/Source/WriteableBitmapExWinPhoneCurveSample/MainPage.xaml.cs $
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
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using WriteableBitmapExCurveSample;

namespace WriteableBitmapExWinPhoneCurveSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Consts

        // Minimum size according to the WP7 UI Design Guideline
        // http://go.microsoft.com/?linkid=9713252
        private const int PointHitZoneSize      = 34;
        private const int PointHitZoneSizeHalf  = PointHitZoneSize >> 1;
        private const int PointVisualSize       = 20;
        private const int PointVisualSizeHalf   = PointVisualSize >> 1;

        #endregion

        #region Fields

        private WriteableBitmap writeableBmp;
        private List<ControlPoint> points;
        private ControlPoint PickedPoint;

        #endregion

        #region Properties

        public float Tension { get; set; }

        #endregion

        #region Contructors

        public MainPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void Init()
        {
            // Show fps counter
            Application.Current.Host.Settings.EnableFrameRateCounter = true;

            // Init vars
            points = new List<ControlPoint>();
            Tension = 0.5f;

            this.DataContext = this;

            // Init WriteableBitmap
            writeableBmp = new WriteableBitmap((int)ViewPortContainer.Width, (int)ViewPortContainer.Height);
            Viewport.Source = writeableBmp;
        }

        private void Draw()
        {
            if (this.points != null && this.writeableBmp != null)
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
                writeableBmp.Invalidate();
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

        private ControlPoint GetMousePoint(MouseEventArgs e)
        {
            return new ControlPoint(e.GetPosition(Viewport));
        }

        #endregion

        #region Eventhandler

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Only add new control point is [DEL] wasn't pressed
            if (PickedPoint == null)
            {
                points.Add(GetMousePoint(e));
            }
            PickedPoint = null;
            Draw();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Pick control point
            var mp = GetMousePoint(e);
            PickedPoint = (from p in points
                           where p.X > mp.X - PointHitZoneSizeHalf && p.X < mp.X + PointHitZoneSizeHalf
                              && p.Y > mp.Y - PointHitZoneSizeHalf && p.Y < mp.Y + PointHitZoneSizeHalf
                           select p).FirstOrDefault();
            Draw();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
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

        private void Button_Click(object sender, RoutedEventArgs e)
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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Set tension text
            if (this.TxtTension != null)
            {
                this.TxtTension.Text = String.Format("Tension: {0:f2}", Tension);
                Draw();
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.points != null && points.Count > 0)
            {
                points.RemoveAt(points.Count - 1);
            }
            Draw();
            e.Cancel = true;
        }

        #endregion
    }
}