#region Header
//
//   Project:           WriteableBitmapEx - Silverlight WriteableBitmap extensions
//   Description:       Sample for the WriteableBitmap extension methods.
//
//   Changed by:        $Author: unknown $
//   Changed on:        $Date: 2015-02-24 20:36:41 +0100 (Di, 24 Feb 2015) $
//   Changed in:        $Revision: 112951 $
//   Project:           $URL: https://writeablebitmapex.svn.codeplex.com/svn/trunk/Source/WriteableBitmapExCurveSample/MainPage.xaml.cs $
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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Schulte.Silverlight;

namespace WriteableBitmapExCurveSample
{
   public partial class MainPage : UserControl
   {
      #region Consts

      private const int PointSize      = 10;
      private const int PointSizeHalf  = PointSize >> 1;
      private const int PointCount     = 3000;

      #endregion

      #region Fields

      private WriteableBitmap writeableBmp;
      private List<ControlPoint> points;
      private ControlPoint PickedPoint;
      private Random rand;
      private bool isInDelete;
      private Plant plant;

      #endregion

      #region Properties

      public float Tension { get; set; }

      #endregion

      #region Contructors

      /// <summary>
      /// MainPage!
      /// </summary>
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
         rand = new Random();
         points = new List<ControlPoint>();
         isInDelete = false;
         Tension = 0.5f;

         // Init plant
         int vw = (int)ViewPortContainer.Width;
         int vh = (int)ViewPortContainer.Height;
         plant = new Plant(new Vector(vw >> 1, vh), new Vector(1, -1), vw, vh);
         plant.BranchLenMin = (int)(vw * 0.17f);
         plant.BranchLenMax = plant.BranchLenMin + (plant.BranchLenMin >> 1);
         plant.MaxGenerations = 6;
         plant.MaxBranchesPerGeneration = 80;
         plant.BranchPoints.AddRange(new List<BranchPoint>  
         { 
            new BranchPoint(1f,  40), // 40° Right at 100% of branch
            new BranchPoint(1f,   5), //  5° Right at 100% of branch
            new BranchPoint(1f,  -5), //  5° Left  at 100% of branch
            new BranchPoint(1f, -40), // 40° Left  at 100% of branch
         });

         ChkDemoPerf.Content = String.Format("Perf. Demo {0} points", PointCount);
         CheckDemoPlant_Checked(this, null);
         this.DataContext = this;

         // Init WriteableBitmap
         writeableBmp = new WriteableBitmap((int)ViewPortContainer.Width, (int)ViewPortContainer.Height);
         ImageViewport.Source = writeableBmp;

         // Start render loop
         CompositionTarget.Rendering += (s, e) =>
         {
            if (ChkDemoPlant.IsChecked.Value)
            {
               plant.Grow();
               plant.Draw(this.writeableBmp);
            }
            else if (ChkDemoPerf.IsChecked.Value)
            {
               AddRandomPoints();
               Draw();
            }
         };
      }

      private void AddRandomPoints()
      {
         int w = (int)ViewPortContainer.Width;
         int h = (int)ViewPortContainer.Height;

         points.Clear();
         for (int i = 0; i < PointCount; i++)
         {
            points.Add(new ControlPoint(rand.Next(0, w), rand.Next(0, h)));
         }
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
            DrawPoint(p, Colors.Blue);
         }
         if (PickedPoint != null)
         {
            DrawPoint(PickedPoint, Colors.Red);
         }
      }

      private void DrawPoint(ControlPoint p, Color color)
      { 
         var x1 = p.X - PointSizeHalf;
         var y1 = p.Y - PointSizeHalf;
         var x2 = p.X + PointSizeHalf;
         var y2 = p.Y + PointSizeHalf;
         writeableBmp.DrawRectangle(x1, y1, x2, y2, color);
      }

      private void DrawBeziers()
      {
         if (points.Count > 3)
         {
            writeableBmp.DrawBeziers(GetPointArray(), Colors.Purple);
         }
      }

      private void DrawCardinal()
      {
         if (points.Count > 2)
         {
            writeableBmp.DrawCurve(GetPointArray(), Tension, Colors.Purple);
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
         return new ControlPoint(e.GetPosition(ImageViewport));
      }

      private void RemovePickedPointPoint()
      {
         if (PickedPoint != null)
         {
            points.Remove(PickedPoint);
            PickedPoint = null;
            isInDelete = true;
            Draw();
         }
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
         if (!isInDelete && PickedPoint == null)
         {
            points.Add(GetMousePoint(e));
         }
         PickedPoint = null;
         isInDelete = false;
         Draw();
      }

      private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         // Pick control point
         var mp = GetMousePoint(e);
         PickedPoint = (from p in points
                        where p.X > mp.X - PointSizeHalf && p.X < mp.X + PointSizeHalf
                           && p.Y > mp.Y - PointSizeHalf && p.Y < mp.Y + PointSizeHalf
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

      protected override void OnKeyDown(KeyEventArgs e)
      {
         // Delete selected control point
         base.OnKeyDown(e);
         if (e.Key == Key.Delete)
         {
            RemovePickedPointPoint();
         }
      }

      private void Button_Click(object sender, RoutedEventArgs e)
      {
         // Restart plant
         if (plant != null && ChkDemoPlant.IsChecked.Value)
         {
            plant.Clear();
         }

         // Remove all comtrol points
         else if (this.points != null)
         {
            this.points.Clear();
            Draw();
         }
      }

      private void BtnSave_Click(object sender, RoutedEventArgs e)
      {
         // Take snapshot
         var clone = this.writeableBmp.Clone();

         // Save as TGA
         SaveFileDialog dialog = new SaveFileDialog { Filter = "TGA Image (*.tga)|*.tga" };
         if (dialog.ShowDialog().Value)
         {
            using (var fileStream = dialog.OpenFile())
            {
               clone.WriteTga(fileStream);
            }
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
         if(this.TxtTension != null)
         {
            this.TxtTension.Text = String.Format("Tension: {0:f2}", Tension);
            Draw();
         }

         // Update plant
         if (plant != null && ChkDemoPlant.IsChecked.Value)
         {
            plant.Tension = Tension;
            plant.Draw(this.writeableBmp);
         }
      }

      private void CheckDemoPlant_UnChecked(object sender, RoutedEventArgs e)
      {
         // Show irrelevant controls for plant growth demo
         if (SPCurveMode != null && ChkDemoPerf != null && ChkShowPoints != null)
         {
            SPCurveMode.Opacity = 1;
            ChkDemoPerf.Opacity = 1;
            ChkShowPoints.Opacity = 1;
            TxtUsage.Opacity = 1;
            BtnClear.Content = "Clear";
            Draw();
         }
      }

      private void CheckDemoPlant_Checked(object sender, RoutedEventArgs e)
      {
         // Hide irrelevant controls for plant growth demo
         if (SPCurveMode != null && ChkDemoPerf != null && ChkShowPoints != null)
         {
            SPCurveMode.Opacity = 0;
            ChkDemoPerf.Opacity = 0;
            ChkShowPoints.Opacity = 0;
            TxtUsage.Opacity = 0;
            BtnClear.Content = "Restart";
         }
      }

      #endregion
   }
}
