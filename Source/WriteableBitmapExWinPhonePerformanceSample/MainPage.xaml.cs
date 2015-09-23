#region Header
//
//   Project:           WriteableBitmapEx - Silverlight WriteableBitmap extensions
//   Description:       Sample for the WriteableBitmap extension methods.
//
//   Changed by:        $Author: unknown $
//   Changed on:        $Date: 2015-02-24 20:36:41 +0100 (Di, 24 Feb 2015) $
//   Changed in:        $Revision: 112951 $
//   Project:           $URL: https://writeablebitmapex.svn.codeplex.com/svn/trunk/Source/WriteableBitmapExWinPhonePerformanceSample/MainPage.xaml.cs $
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
using Microsoft.Phone.Controls;

namespace WriteableBitmapExWinPhonePerformanceSample
{
   public partial class MainPage
   {
      #region Fields

      private WriteableBitmap writeableBmp;
      private int shapeCount;
      private static Random rand = new Random();
      private int frameCounter = 0;

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

         // Init WriteableBitmap
         writeableBmp = new WriteableBitmap((int)ViewPortContainer.Width, (int)ViewPortContainer.Height);
         ImageViewport.Source = writeableBmp;

         // Init vars
         TxtBoxShapeCount_TextChanged(this, null);

         // Start render loop
         CompositionTarget.Rendering += new EventHandler(CompositionTargetRendering);
      }

      private void Draw()
      {
         DrawShapes();
      }

      /// <summary>
      /// Draws the different types of shapes.
      /// </summary>
      private void DrawStaticShapes()
      {
         // Init some size vars
         int w = this.writeableBmp.PixelWidth - 2;
         int h = this.writeableBmp.PixelHeight - 2;
         int w3rd = w / 3;
         int h3rd = h / 3;
         int w6th = w3rd >> 1;
         int h6th = h3rd >> 1;

         // Clear 
         writeableBmp.Clear();

         // Draw some points
         for (int i = 0; i < 200; i++)
         {
            writeableBmp.SetPixel(rand.Next(w3rd), rand.Next(h3rd), GetRandomColor());
         }

         // Draw Standard shapes
         writeableBmp.DrawLine(rand.Next(w3rd, w3rd * 2), rand.Next(h3rd), rand.Next(w3rd, w3rd * 2), rand.Next(h3rd), GetRandomColor());
         writeableBmp.DrawTriangle(rand.Next(w3rd * 2, w - w6th), rand.Next(h6th), rand.Next(w3rd * 2, w), rand.Next(h6th, h3rd), rand.Next(w - w6th, w), rand.Next(h3rd), GetRandomColor());

         writeableBmp.DrawQuad(rand.Next(0, w6th), rand.Next(h3rd, h3rd + h6th), rand.Next(w6th, w3rd), rand.Next(h3rd, h3rd + h6th), rand.Next(w6th, w3rd), rand.Next(h3rd + h6th, 2 * h3rd), rand.Next(0, w6th), rand.Next(h3rd + h6th, 2 * h3rd), GetRandomColor());
         writeableBmp.DrawRectangle(rand.Next(w3rd, w3rd + w6th), rand.Next(h3rd, h3rd + h6th), rand.Next(w3rd + w6th, w3rd * 2), rand.Next(h3rd + h6th, 2 * h3rd), GetRandomColor());

         // Random polyline
         int[] p = new int[rand.Next(7, 10) * 2];
         for (int j = 0; j < p.Length; j += 2)
         {
            p[j] = rand.Next(w3rd * 2, w);
            p[j + 1] = rand.Next(h3rd, 2 * h3rd);
         }
         writeableBmp.DrawPolyline(p, GetRandomColor());

         // Random closed polyline
         p = new int[rand.Next(6, 9) * 2];
         for (int j = 0; j < p.Length - 2; j += 2)
         {
            p[j] = rand.Next(w3rd);
            p[j + 1] = rand.Next(2 * h3rd, h);
         }
         p[p.Length - 2] = p[0];
         p[p.Length - 1] = p[1];
         writeableBmp.DrawPolyline(p, GetRandomColor());

         // Ellipses
         writeableBmp.DrawEllipse(rand.Next(w3rd, w3rd + w6th), rand.Next(h3rd * 2, h - h6th), rand.Next(w3rd + w6th, w3rd * 2), rand.Next(h - h6th, h), GetRandomColor());
         writeableBmp.DrawEllipseCentered(w - w6th, h - h6th, w6th >> 1, h6th >> 1, GetRandomColor());

         // Draw Grid
         writeableBmp.DrawLine(0, h3rd, w, h3rd, Colors.Black);
         writeableBmp.DrawLine(0, 2 * h3rd, w, 2 * h3rd, Colors.Black);
         writeableBmp.DrawLine(w3rd, 0, w3rd, h, Colors.Black);
         writeableBmp.DrawLine(2 * w3rd, 0, 2 * w3rd, h, Colors.Black);

         // Invalidate
         writeableBmp.Invalidate();
      }

      /// <summary>
      /// Draws random shapes.
      /// </summary>
      private void DrawShapes()
      {
         using (var bitmapContext = writeableBmp.GetBitmapContext())
         {
            // Init some size vars
            int w = this.writeableBmp.PixelWidth - 2;
            int h = this.writeableBmp.PixelHeight - 2;
            int wh = w >> 1;
            int hh = h >> 1;

            // Clear 
            writeableBmp.Clear();

            // Draw Shapes and use refs for faster access which speeds up a lot.
            int wbmp = writeableBmp.PixelWidth;
            int hbmp = writeableBmp.PixelHeight;
            int[] pixels = writeableBmp.Pixels;
            for (int i = 0; i < shapeCount / 6; i++)
            {
               // Standard shapes
               WriteableBitmapExtensions.DrawLine(bitmapContext, wbmp, hbmp, rand.Next(w), rand.Next(h), rand.Next(w),
                                                  rand.Next(h), GetRandomColor());
               writeableBmp.DrawTriangle(rand.Next(w), rand.Next(h), rand.Next(w), rand.Next(h), rand.Next(w),
                                         rand.Next(h), GetRandomColor());
               writeableBmp.DrawQuad(rand.Next(w), rand.Next(h), rand.Next(w), rand.Next(h), rand.Next(w),
                                     rand.Next(h), rand.Next(w), rand.Next(h), GetRandomColor());
               writeableBmp.DrawRectangle(rand.Next(wh), rand.Next(hh), rand.Next(wh, w), rand.Next(hh, h),
                                          GetRandomColor());
               writeableBmp.DrawEllipse(rand.Next(wh), rand.Next(hh), rand.Next(wh, w), rand.Next(hh, h),
                                        GetRandomColor());

               // Random polyline
               int[] p = new int[rand.Next(5, 10) * 2];
               for (int j = 0; j < p.Length; j += 2)
               {
                  p[j] = rand.Next(w);
                  p[j + 1] = rand.Next(h);
               }
               writeableBmp.DrawPolyline(p, GetRandomColor());
            }

            // Invalidate
            writeableBmp.Invalidate();
         }
      }

      /// <summary>
      /// Draws random ellipses
      /// </summary>
      private void DrawEllipses()
      {
         // Init some size vars
         int w = this.writeableBmp.PixelWidth - 2;
         int h = this.writeableBmp.PixelHeight - 2;
         int wh = w >> 1;
         int hh = h >> 1;

         // Clear 
         writeableBmp.Clear();

         // Draw Ellipses
         for (int i = 0; i < shapeCount; i++)
         {
            writeableBmp.DrawEllipse(rand.Next(wh), rand.Next(hh), rand.Next(wh, w), rand.Next(hh, h), GetRandomColor());
         }

         // Invalidate
         writeableBmp.Invalidate();
      }

      /// <summary>
      /// Draws circles that decrease in size to build a flower that is animated
      /// </summary>
      private void DrawEllipsesFlower()
      {
         // Init some size vars
         int w = this.writeableBmp.PixelWidth - 2;
         int h = this.writeableBmp.PixelHeight - 2;

         // Increment frame counter
         if (++frameCounter >= int.MaxValue || frameCounter < 1)
         {
            frameCounter = 1;
         }
         double s = Math.Sin(frameCounter * 0.01);
         if (s < 0)
         {
            s *= -1;
         }

         // Clear 
         writeableBmp.Clear();

         // Draw center circle
         int xc = w >> 1;
         int yc = h >> 1;
         // Animate base size with sine
         int r0 = (int)((w + h) * 0.07 * s) + 10;
         writeableBmp.DrawEllipseCentered(xc, yc, r0, r0, Colors.Brown);

         // Draw outer circles
         int dec = (int)((w + h) * 0.0045f);
         int r = (int)((w + h) * 0.025f);
         int offset = r0 + r;
         for (int i = 1; i < 6 && r > 1; i++)
         {
            for (double f = 1; f < 7; f += 0.7)
            {
               // Calc postion based on unit circle
               int xc2 = (int)(Math.Sin(frameCounter * 0.002 * i + f) * offset + xc);
               int yc2 = (int)(Math.Cos(frameCounter * 0.002 * i + f) * offset + yc);
               int col = (int)(0xFFFF0000 | (uint)(0x1A * i) << 8 | (uint)(0x20 * f));
               writeableBmp.DrawEllipseCentered(xc2, yc2, r, r, col);
            }
            // Next ring
            offset += r;
            r -= dec;
            offset += r;
         }

         // Invalidate
         writeableBmp.Invalidate();
      }

      /// <summary>
      /// Random color fully opaque
      /// </summary>
      /// <returns></returns>
      private static int GetRandomColor()
      {
         return (int)(0xFF000000 | (uint)rand.Next(0xFFFFFF));
      }

      #endregion

      #region Eventhandler

      private void UserControlLoaded(object sender, RoutedEventArgs e)
      {
         Init();
      }

      private int f = 0;
      private TimeSpan all;
      private void CompositionTargetRendering(object sender, EventArgs e)
      {
         var now = DateTime.Now;
         Draw();
         var span = DateTime.Now - now;
         all += span;
         f++;
         TxtBlockPerf.Text = String.Format("{0:f2} ms / frame", all.TotalMilliseconds / f);

         if (f > 10)
         {
            f = 0;
            all = TimeSpan.FromTicks(0);
         }
      }

      private void TxtBoxShapeCount_TextChanged(object sender, TextChangedEventArgs e)
      {
         int v = 1;
         if (int.TryParse(TxtBoxShapeCount.Text, out v))
         {
            this.shapeCount = v;
            TxtBoxShapeCount.Background = null;
            frameCounter = 0;
            Draw();
         }
         else
         {
            TxtBoxShapeCount.Background = new SolidColorBrush(Colors.Red);
         }
      }

      #endregion
   }
}