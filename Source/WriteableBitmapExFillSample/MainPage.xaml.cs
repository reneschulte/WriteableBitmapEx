#region Header
//
//   Project:           WriteableBitmapEx - Silverlight WriteableBitmap extensions
//   Description:       Sample for the WriteableBitmap extension methods.
//
//   Changed by:        $Author: unknown $
//   Changed on:        $Date: 2015-02-24 20:36:41 +0100 (Di, 24 Feb 2015) $
//   Changed in:        $Revision: 112951 $
//   Project:           $URL: https://writeablebitmapex.svn.codeplex.com/svn/trunk/Source/WriteableBitmapExFillSample/MainPage.xaml.cs $
//   Id:                $Id: MainPage.xaml.cs 112951 2015-02-24 19:36:41Z unknown $
//
//
//   Copyright © 2009-2015 Rene Schulte and WriteableBitmapEx Contributors
//
//   This code is open source. Please read the License.txt for details. No worries, we won't sue you! ;)
//
#endregion

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace WriteableBitmapExFillSample
{
   public partial class MainPage : UserControl
   {
      #region Inner class

      private class Circle
      {
         public int Color {get; set;}
         public int X {get; set;}
         public int Y {get; set;}
         public float Radius { get; set; }
         public float Velocity { get; set; }

         public void Update()
         {
            Radius += Velocity;
         }
      }

      #endregion

      #region Fields

      private WriteableBitmap writeableBmp;
      private int shapeCount;
      private static Random rand = new Random();
      private List<Circle> circles;
      private float time;
      private const float timeStep = 0.01f;

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
         Reset();
         CompositionTarget.Rendering += (s, e) => Draw();
      }

      private void Reset()
      {
         // Init WriteableBitmap
         writeableBmp = new WriteableBitmap((int)ViewPortContainer.Width, (int)ViewPortContainer.Height);
         ImageViewport.Source = writeableBmp;

         // Init vars
         time = 0f;
         circles = new List<Circle>();
         TxtBoxShapeCount_TextChanged(this, null);

         // Start render loop
         DrawStaticShapes();
      }

      private void Draw()
      {
         // What to draw?
         if (!RBFillShapes.IsChecked.Value)
         {
            TxtBlockShapeCount.Visibility = Visibility.Visible;
            TxtBoxShapeCount.Visibility = Visibility.Visible;
            if (RBFillShapesAnim.IsChecked.Value)
            {
               DrawShapes();
            }
            else
            {
               HideShapeCountText();
               DrawFillDemo();
            }
         }
      }

      /// <summary>
      /// Draws the different types of shapes.
      /// </summary>
      private void DrawStaticShapes()
      {
         HideShapeCountText();
         if (writeableBmp != null)
         {
            // Init some size vars
            int w = this.writeableBmp.PixelWidth;
            int h = this.writeableBmp.PixelHeight;
            int w3 = w / 3;
            int h3 = h / 3;
            int w6 = w3 >> 1;
            int h6 = h3 >> 1;
            int w12 = w6 >> 1;
            int h12 = h6 >> 1;

            // Clear 
            writeableBmp.Clear();

            // Fill closed concave polygon
            var p = new int[]
            {
               w12 >> 1, h12,
               w6, h3 - (h12 >> 1),
               w3 - (w12 >> 1), h12,
               w6 + w12, h12,
               w6, h6 + h12,
               w12, h12,
               w12 >> 1, h12,
            };
            writeableBmp.FillPolygon(p, GetRandomColor());

            // Fill closed convex polygon
            p = new int[]
            {
               w3 + w6, h12 >> 1,
               w3 + w6 + w12, h12,
               w3 + w6 + w12, h6 + h12,
               w3 + w6, h6 + h12 + (h12 >> 1),
               w3 + w12, h6 + h12,
               w3 + w12, h12,
               w3 + w6, h12 >> 1,
            };
            writeableBmp.FillPolygon(p, GetRandomColor()); 
           
            // Fill Triangle + Quad
            writeableBmp.FillTriangle(2 * w3 + w6, h12 >> 1, 2 * w3 + w6 + w12, h6 + h12, 2 * w3 + w12, h6 + h12, GetRandomColor());
            writeableBmp.FillQuad(w6, h3 + (h12 >> 1), w6 + w12, h3 + h6, w6, h3 + h6 + h12 + (h12 >> 1), w12, h3 + h6, GetRandomColor());

            // Fill Ellipses
            writeableBmp.FillEllipse(rand.Next(w3, w3 + w6), rand.Next(h3, h3 + h6), rand.Next(w3 + w6, 2 * w3), rand.Next(h3 + h6, 2 * h3), GetRandomColor());
            writeableBmp.FillEllipseCentered(2 * w3 + w6, h3 + h6, w12, h12, GetRandomColor());

            // Fill closed Cardinal Spline curve
            p = new int[]
            {
               w12 >> 1, 2 * h3 + h12,
               w6, h - (h12 >> 1),
               w3 - (w12 >> 1), 2 * h3 + h12,
               w6 + w12, 2 * h3 + h12,
               w6, 2 * h3 + (h12 >> 1),
               w12, 2 * h3 + h12,
            };
            writeableBmp.FillCurveClosed(p, 0.5f, GetRandomColor());

            // Fill closed Beziér curve
            p = new int[]
            {
               w3 + w12, 2 * h3 + h6 + h12,
               w3 + w6 + (w12 >> 1), 2 * h3,
               w3 + w6 + w12 + (w12 >> 1), 2 * h3,
               w3 + w6 + w12, 2 * h3 + h6 + h12,
            };
            writeableBmp.FillBeziers(p, GetRandomColor());

              // Fill Rectangle
            writeableBmp.FillRectangle(rand.Next(2 * w3, 2 * w3 + w6), rand.Next(2 * h3, 2 * h3 + h6), rand.Next(2 * w3 + w6, w), rand.Next(2 * h3 + h6, h), GetRandomColor());
          
            // Draw Grid
            writeableBmp.DrawLine(0, h3, w, h3, Colors.Black);
            writeableBmp.DrawLine(0, 2 * h3, w, 2 * h3, Colors.Black);
            writeableBmp.DrawLine(w3, 0, w3, h, Colors.Black);
            writeableBmp.DrawLine(2 * w3, 0, 2 * w3, h, Colors.Black);

            // Invalidate
            writeableBmp.Invalidate();
         }
      }

      /// <summary>
      /// Draws random shapes.
      /// </summary>
      private void DrawShapes()
      {
         // Init some size vars
         int w = this.writeableBmp.PixelWidth - 2;
         int h = this.writeableBmp.PixelHeight - 2;
         int w2 = w >> 1;
         int h2 = h >> 1;

         // Clear 
         writeableBmp.Clear();

         // Fill Shapes
         for (int i = 0; i < shapeCount; i++)
         {
            // Random polygon
            int[] p = new int[rand.Next(5, 10) * 2];
            for (int j = 0; j < p.Length; j += 2)
            {
               p[j] = rand.Next(w);
               p[j + 1] = rand.Next(h);
            }
            writeableBmp.FillPolygon(p, GetRandomColor());
         }

         // Invalidate
         writeableBmp.Invalidate();
      }

      private void DrawFillDemo()
      {
         // Init some size vars
         int w = this.writeableBmp.PixelWidth - 2;
         int h = this.writeableBmp.PixelHeight - 2;
         int w2 = w >> 1;
         int h2 = h >> 1;
         int w4 = w2 >> 1;
         int h4 = h2 >> 1;
         int w8 = w4 >> 1;
         int h8 = h4 >> 1;

         // Clear 
         writeableBmp.Clear();

         // Add circles
         const float startTimeFixed = 1;
         const float endTimeFixed = startTimeFixed + timeStep;
         const float startTimeRandom = 3;
         const float endTimeCurve = 9.7f;
         const int intervalRandom = 2;
         const int maxCircles = 30;

         // Spread fixed position and color circles
         if (time > startTimeFixed && time < endTimeFixed)
         {
            unchecked
            {
               circles.Add(new Circle {X = w8, Y = h8, Radius = 10f, Velocity = 1, Color = (int) 0xFFC88717});
               circles.Add(new Circle {X = w8, Y = h - h8, Radius = 10f, Velocity = 1, Color = (int) 0xFFFB522B});
               circles.Add(new Circle {X = w - w8, Y = h8, Radius = 10f, Velocity = 1, Color = (int) 0xFFDB6126});
               circles.Add(new Circle {X = w - w8, Y = h - h8, Radius = 10f, Velocity = 1, Color = (int) 0xFFFFCE25});
            }
         }

         // Spread random position and color circles
         if (time > startTimeRandom && (int) time % intervalRandom == 0)
         {
            unchecked
            {
               circles.Add(new Circle
                              {
                                 X = rand.Next(w),
                                 Y = rand.Next(h),
                                 Radius = 1f,
                                 Velocity = rand.Next(1, 5),
                                 Color = rand.Next((int) 0xFFFF0000, (int) 0xFFFFFFFF),
                              });
            }
         }

         // Render and update circles
         foreach (var circle in circles)
         {
            var r = (int) circle.Radius;
            writeableBmp.FillEllipseCentered(circle.X, circle.Y, r, r, circle.Color);
            circle.Update();
         }

         if (circles.Count > maxCircles)
         {
            circles.RemoveAt(0);
         }

         // Fill closed Cardinal Spline curve
         if (time < endTimeCurve)
         {
            var p = new int[]
                       {
                          w4, h2,
                          w2, h2 + h4,
                          w2 + w4, h2,
                          w2, h4,
                       };
            writeableBmp.FillCurveClosed(p, (float) Math.Sin(time) * 7, Colors.Purple);
         }

         // Invalidate
         writeableBmp.Invalidate();

         // Update time
         time += timeStep;
      }

      /// <summary>
      /// Random color fully opaque
      /// </summary>
      /// <returns></returns>
      private static int GetRandomColor()
      {
         return (int)(0xFF000000 | (uint)rand.Next(0xFFFFFF));
      }

      private void HideShapeCountText()
      {
         if (TxtBoxShapeCount != null)
         {
            this.TxtBoxShapeCount.Visibility = Visibility.Collapsed;
         }
         if (TxtBlockShapeCount != null)
         {
            this.TxtBlockShapeCount.Visibility = Visibility.Collapsed;
         }
      }

      #endregion

      #region Eventhandler

      private void UserControl_Loaded(object sender, RoutedEventArgs e)
      {
         Init();
      }

      private void TxtBoxShapeCount_TextChanged(object sender, TextChangedEventArgs e)
      {
         int v = 1;
         if (int.TryParse(TxtBoxShapeCount.Text, out v))
         {
            this.shapeCount = v;
            TxtBoxShapeCount.Background = null;
            Draw();
         }
         else
         {
            TxtBoxShapeCount.Background = new SolidColorBrush(Colors.Red);
         }
      }

      private void RadioButton_Checked(object sender, RoutedEventArgs e)
      {
         Reset();
      }

      #endregion
   }
}
