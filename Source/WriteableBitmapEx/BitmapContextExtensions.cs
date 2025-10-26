#region Header
//
//   Project:           WriteableBitmapEx - WriteableBitmap extensions
//   Description:       Collection of extension methods for the BitmapContext class.
//
//   Changed by:        $Author$
//   Changed on:        $Date$
//   Changed in:        $Revision$
//   Project:           $URL$
//   Id:                $Id$
//
//
//   Copyright © 2009-2015 Rene Schulte and WriteableBitmapEx Contributors
//
//   This code is open source. Please read the License.txt for details. No worries, we won't sue you! ;)
//
#endregion

using System;

#if NETFX_CORE
using Windows.Foundation;
using Windows.UI;

namespace Windows.UI.Xaml.Media.Imaging
#else
namespace System.Windows.Media.Imaging
#endif
{
    /// <summary>
    /// Collection of extension methods for the BitmapContext class.
    /// </summary>
    public
#if WPF
       unsafe 
#endif
    static partial class BitmapContextExtensions
    {
        #region Line Drawing Methods

        /// <summary> 
        /// Draws an anti-aliased line with a desired stroke thickness
        /// <param name="context">The BitmapContext containing the pixels.</param>
        /// <param name="x1">The x-coordinate of the start point.</param>
        /// <param name="y1">The y-coordinate of the start point.</param>
        /// <param name="x2">The x-coordinate of the end point.</param>
        /// <param name="y2">The y-coordinate of the end point.</param>
        /// <param name="color">The color for the line.</param>
        /// <param name="strokeThickness">The stroke thickness of the line.</param>
        /// </summary>
        public static void DrawLineAa(this BitmapContext context, int x1, int y1, int x2, int y2, int color, int strokeThickness, Rect? clipRect = null)
        {
            WriteableBitmapExtensions.AAWidthLine(context.Width, context.Height, context, x1, y1, x2, y2, strokeThickness, color, clipRect);
        }

        /// <summary> 
        /// Draws an anti-aliased line with a desired stroke thickness
        /// <param name="context">The BitmapContext containing the pixels.</param>
        /// <param name="x1">The x-coordinate of the start point.</param>
        /// <param name="y1">The y-coordinate of the start point.</param>
        /// <param name="x2">The x-coordinate of the end point.</param>
        /// <param name="y2">The y-coordinate of the end point.</param>
        /// <param name="color">The color for the line.</param>
        /// <param name="strokeThickness">The stroke thickness of the line.</param>
        /// </summary>
        public static void DrawLineAa(this BitmapContext context, int x1, int y1, int x2, int y2, Color color, int strokeThickness, Rect? clipRect = null)
        {
            var col = WriteableBitmapExtensions.ConvertColor(color);
            WriteableBitmapExtensions.AAWidthLine(context.Width, context.Height, context, x1, y1, x2, y2, strokeThickness, col, clipRect);
        }

        /// <summary> 
        /// Draws an anti-aliased line
        /// <param name="context">The BitmapContext containing the pixels.</param>
        /// <param name="x1">The x-coordinate of the start point.</param>
        /// <param name="y1">The y-coordinate of the start point.</param>
        /// <param name="x2">The x-coordinate of the end point.</param>
        /// <param name="y2">The y-coordinate of the end point.</param>
        /// <param name="color">The color for the line.</param>
        /// </summary> 
        public static void DrawLineAa(this BitmapContext context, int x1, int y1, int x2, int y2, int color, Rect? clipRect = null)
        {
            WriteableBitmapExtensions.DrawLineAa(context, context.Width, context.Height, x1, y1, x2, y2, color, clipRect);
        }

        /// <summary> 
        /// Draws an anti-aliased line
        /// <param name="context">The BitmapContext containing the pixels.</param>
        /// <param name="x1">The x-coordinate of the start point.</param>
        /// <param name="y1">The y-coordinate of the start point.</param>
        /// <param name="x2">The x-coordinate of the end point.</param>
        /// <param name="y2">The y-coordinate of the end point.</param>
        /// <param name="color">The color for the line.</param>
        /// </summary> 
        public static void DrawLineAa(this BitmapContext context, int x1, int y1, int x2, int y2, Color color, Rect? clipRect = null)
        {
            var col = WriteableBitmapExtensions.ConvertColor(color);
            WriteableBitmapExtensions.DrawLineAa(context, context.Width, context.Height, x1, y1, x2, y2, col, clipRect);
        }

        /// <summary>
        /// Draws a line with the specified color.
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="x1">The x-coordinate of the start point.</param>
        /// <param name="y1">The y-coordinate of the start point.</param>
        /// <param name="x2">The x-coordinate of the end point.</param>
        /// <param name="y2">The y-coordinate of the end point.</param>
        /// <param name="color">The color for the line.</param>
        /// <param name="clipRect">The region in the image to restrict drawing to.</param>
        public static void DrawLine(this BitmapContext context, int x1, int y1, int x2, int y2, int color, Rect? clipRect = null)
        {
            WriteableBitmapExtensions.DrawLine(context, context.Width, context.Height, x1, y1, x2, y2, color, clipRect);
        }

        /// <summary>
        /// Draws a line with the specified color.
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="x1">The x-coordinate of the start point.</param>
        /// <param name="y1">The y-coordinate of the start point.</param>
        /// <param name="x2">The x-coordinate of the end point.</param>
        /// <param name="y2">The y-coordinate of the end point.</param>
        /// <param name="color">The color for the line.</param>
        /// <param name="clipRect">The region in the image to restrict drawing to.</param>
        public static void DrawLine(this BitmapContext context, int x1, int y1, int x2, int y2, Color color, Rect? clipRect = null)
        {
            var col = WriteableBitmapExtensions.ConvertColor(color);
            WriteableBitmapExtensions.DrawLine(context, context.Width, context.Height, x1, y1, x2, y2, col, clipRect);
        }

        #endregion

        #region Shape Drawing Methods

        /// <summary>
        /// Draws a rectangle.
        /// x2 has to be greater than x1 and y2 has to be greater than y1.
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="x1">The x-coordinate of the bounding rectangle's left side.</param>
        /// <param name="y1">The y-coordinate of the bounding rectangle's top side.</param>
        /// <param name="x2">The x-coordinate of the bounding rectangle's right side.</param>
        /// <param name="y2">The y-coordinate of the bounding rectangle's bottom side.</param>
        /// <param name="color">The color.</param>
        public static void DrawRectangle(this BitmapContext context, int x1, int y1, int x2, int y2, int color)
        {
            // Use refs for faster access (really important!) speeds up a lot!
            var w = context.Width;
            var h = context.Height;

            // Check boundaries
            if ((x1 < 0 && x2 < 0) || (y1 < 0 && y2 < 0)
             || (x1 >= w && x2 >= w) || (y1 >= h && y2 >= h))
            {
                return;
            }

            // Clamp boundaries
            if (x1 < 0) { x1 = 0; }
            if (y1 < 0) { y1 = 0; }
            if (x2 < 0) { x2 = 0; }
            if (y2 < 0) { y2 = 0; }
            if (x1 >= w) { x1 = w - 1; }
            if (y1 >= h) { y1 = h - 1; }
            if (x2 >= w) { x2 = w - 1; }
            if (y2 >= h) { y2 = h - 1; }

            WriteableBitmapExtensions.DrawLine(context, w, h, x1, y1, x2, y1, color);
            WriteableBitmapExtensions.DrawLine(context, w, h, x2, y1, x2, y2, color);
            WriteableBitmapExtensions.DrawLine(context, w, h, x2, y2, x1, y2, color);
            WriteableBitmapExtensions.DrawLine(context, w, h, x1, y2, x1, y1, color);
        }

        /// <summary>
        /// Draws a rectangle.
        /// x2 has to be greater than x1 and y2 has to be greater than y1.
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="x1">The x-coordinate of the bounding rectangle's left side.</param>
        /// <param name="y1">The y-coordinate of the bounding rectangle's top side.</param>
        /// <param name="x2">The x-coordinate of the bounding rectangle's right side.</param>
        /// <param name="y2">The y-coordinate of the bounding rectangle's bottom side.</param>
        /// <param name="color">The color.</param>
        public static void DrawRectangle(this BitmapContext context, int x1, int y1, int x2, int y2, Color color)
        {
            var col = WriteableBitmapExtensions.ConvertColor(color);
            context.DrawRectangle(x1, y1, x2, y2, col);
        }

        /// <summary>
        /// Draws an ellipse.
        /// x2 has to be greater than x1 and y2 has to be greater than y1.
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="x1">The x-coordinate of the bounding rectangle's left side.</param>
        /// <param name="y1">The y-coordinate of the bounding rectangle's top side.</param>
        /// <param name="x2">The x-coordinate of the bounding rectangle's right side.</param>
        /// <param name="y2">The y-coordinate of the bounding rectangle's bottom side.</param>
        /// <param name="color">The color.</param>
        public static void DrawEllipse(this BitmapContext context, int x1, int y1, int x2, int y2, int color)
        {
            // Calc center and radius
            int xr = (x2 - x1) >> 1;
            int yr = (y2 - y1) >> 1;
            int xc = x1 + xr;
            int yc = y1 + yr;
            context.DrawEllipseCentered(xc, yc, xr, yr, color);
        }

        /// <summary>
        /// Draws an ellipse.
        /// x2 has to be greater than x1 and y2 has to be greater than y1.
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="x1">The x-coordinate of the bounding rectangle's left side.</param>
        /// <param name="y1">The y-coordinate of the bounding rectangle's top side.</param>
        /// <param name="x2">The x-coordinate of the bounding rectangle's right side.</param>
        /// <param name="y2">The y-coordinate of the bounding rectangle's bottom side.</param>
        /// <param name="color">The color.</param>
        public static void DrawEllipse(this BitmapContext context, int x1, int y1, int x2, int y2, Color color)
        {
            var col = WriteableBitmapExtensions.ConvertColor(color);
            context.DrawEllipse(x1, y1, x2, y2, col);
        }

        /// <summary>
        /// A Fast Bresenham Type Algorithm For Drawing Ellipses http://homepage.smc.edu/kennedy_john/belipse.pdf 
        /// Uses a different parameter representation than DrawEllipse().
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="xc">The x-coordinate of the ellipses center.</param>
        /// <param name="yc">The y-coordinate of the ellipses center.</param>
        /// <param name="xr">The radius of the ellipse in x-direction.</param>
        /// <param name="yr">The radius of the ellipse in y-direction.</param>
        /// <param name="color">The color for the line.</param>
        public static void DrawEllipseCentered(this BitmapContext context, int xc, int yc, int xr, int yr, int color)
        {
            // Use refs for faster access (really important!) speeds up a lot!
            var pixels = context.Pixels;
            var w = context.Width;
            var h = context.Height;

            // Avoid endless loop
            if (xr < 1 || yr < 1)
            {
                return;
            }

            // Init vars
            int uh, lh, uy, ly, lx, rx;
            int x = xr;
            int y = 0;
            int xrSqTwo = (xr * xr) << 1;
            int yrSqTwo = (yr * yr) << 1;
            int xChg = yr * yr * (1 - (xr << 1));
            int yChg = xr * xr;
            int err = 0;
            int xStopping = yrSqTwo * xr;
            int yStopping = 0;

            // Draw first set of points counter clockwise where tangent line slope > -1.
            while (xStopping > yStopping)
            {
                // Draw 4 quadrant points at once
                uy = yc + y;                  // Upper half
                ly = yc - y;                  // Lower half
                if (uy < 0) uy = 0;           // Clip
                if (uy >= h) uy = h - 1;      // ...
                if (ly < 0) ly = 0;
                if (ly >= h) ly = h - 1;
                uh = uy * w;                  // Upper half
                lh = ly * w;                  // Lower half

                rx = xc + x;
                lx = xc - x;
                if (rx < 0) rx = 0;           // Clip
                if (rx >= w) rx = w - 1;      // ...
                if (lx < 0) lx = 0;
                if (lx >= w) lx = w - 1;
                    
                // Draw line
                if (uy >= 0 && uy < h)
                {
                    pixels[rx + uh] = color;      // Quadrant I (Actually an octant)
                    pixels[lx + uh] = color;      // Quadrant II
                }
                if (ly >= 0 && ly < h)
                {
                    pixels[rx + lh] = color;      // Quadrant IV
                    pixels[lx + lh] = color;      // Quadrant III
                }

                y++;
                yStopping += xrSqTwo;
                err += yChg;
                yChg += xrSqTwo;
                if ((xChg + (err << 1)) > 0)
                {
                    x--;
                    xStopping -= yrSqTwo;
                    err += xChg;
                    xChg += yrSqTwo;
                }
            }

            // ReInit vars
            x = 0;
            y = yr;
            uy = yc + y;                  // Upper half
            ly = yc - y;                  // Lower half
            if (uy < 0) uy = 0;           // Clip
            if (uy >= h) uy = h - 1;      // ...
            if (ly < 0) ly = 0;
            if (ly >= h) ly = h - 1;
            uh = uy * w;                  // Upper half
            lh = ly * w;                  // Lower half
            xChg = yr * yr;
            yChg = xr * xr * (1 - (yr << 1));
            err = 0;
            xStopping = 0;
            yStopping = xrSqTwo * yr;

            // Draw second set of points clockwise where tangent line slope < -1.
            while (xStopping < yStopping)
            {
                // Draw 4 quadrant points at once
                rx = xc + x;
                lx = xc - x;
                if (rx < 0) rx = 0;           // Clip
                if (rx >= w) rx = w - 1;      // ...
                if (lx < 0) lx = 0;
                if (lx >= w) lx = w - 1;

                // Draw line
                if (uy >= 0 && uy < h)
                {
                    pixels[rx + uh] = color;      // Quadrant I (Actually an octant)
                    pixels[lx + uh] = color;      // Quadrant II
                }
                if (ly >= 0 && ly < h)
                {
                    pixels[rx + lh] = color;      // Quadrant IV
                    pixels[lx + lh] = color;      // Quadrant III
                }

                x++;
                xStopping += yrSqTwo;
                err += xChg;
                xChg += yrSqTwo;
                if ((yChg + (err << 1)) > 0)
                {
                    y--;
                    uy = yc + y;                  // Upper half
                    ly = yc - y;                  // Lower half
                    if (uy < 0) uy = 0;           // Clip
                    if (uy >= h) uy = h - 1;      // ...
                    if (ly < 0) ly = 0;
                    if (ly >= h) ly = h - 1;
                    uh = uy * w;                  // Upper half
                    lh = ly * w;                  // Lower half
                    yStopping -= xrSqTwo;
                    err += yChg;
                    yChg += xrSqTwo;
                }
            }
        }

        /// <summary>
        /// A Fast Bresenham Type Algorithm For Drawing Ellipses http://homepage.smc.edu/kennedy_john/belipse.pdf
        /// Uses a different parameter representation than DrawEllipse().
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="xc">The x-coordinate of the ellipses center.</param>
        /// <param name="yc">The y-coordinate of the ellipses center.</param>
        /// <param name="xr">The radius of the ellipse in x-direction.</param>
        /// <param name="yr">The radius of the ellipse in y-direction.</param>
        /// <param name="color">The color for the line.</param>
        }

        #endregion

        #region Fill Methods

        /// <summary>
        /// Fills a rectangle.
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="x1">The x-coordinate of the bounding rectangle's left side.</param>
        /// <param name="y1">The y-coordinate of the bounding rectangle's top side.</param>
        /// <param name="x2">The x-coordinate of the bounding rectangle's right side.</param>
        /// <param name="y2">The y-coordinate of the bounding rectangle's bottom side.</param>
        /// <param name="color">The color.</param>
        /// <param name="doAlphaBlend">True if alpha blending should be performed or false if not.</param>
        public static void FillRectangle(this BitmapContext context, int x1, int y1, int x2, int y2, int color, bool doAlphaBlend = false)
        {
            // Use refs for faster access (really important!) speeds up a lot!
            var w = context.Width;
            var h = context.Height;

            int sa = ((color >> 24) & 0xff);
            int sr = ((color >> 16) & 0xff);
            int sg = ((color >> 8) & 0xff);
            int sb = ((color) & 0xff);

            bool noBlending = !doAlphaBlend || sa == 255;

            var pixels = context.Pixels;

            // Check boundaries
            if ((x1 < 0 && x2 < 0) || (y1 < 0 && y2 < 0)
             || (x1 >= w && x2 >= w) || (y1 >= h && y2 >= h))
            {
                return;
            }

            // Clamp boundaries
            if (x1 < 0) { x1 = 0; }
            if (y1 < 0) { y1 = 0; }
            if (x2 < 0) { x2 = 0; }
            if (y2 < 0) { y2 = 0; }
            if (x1 > w) { x1 = w; }
            if (y1 > h) { y1 = h; }
            if (x2 > w) { x2 = w; }
            if (y2 > h) { y2 = h; }

            //swap values
            if (y1 > y2)
            {
                y2 -= y1;
                y1 += y2;
                y2 = (y1 - y2);
            }

            // Fill first line
            var startY = y1 * w;
            var startYPlusX1 = startY + x1;
            var endOffset = startY + x2;
            for (var idx = startYPlusX1; idx < endOffset; idx++)
            {
                pixels[idx] = noBlending ? color : AlphaBlendColors(pixels[idx], sa, sr, sg, sb);
            }

            // Copy first line
            var len = (x2 - x1);
            for (var y = y1 + 1; y < y2; y++)
            {
                var offset = y * w + x1;
                for (var i = 0; i < len; i++)
                {
                    pixels[offset + i] = noBlending ? color : AlphaBlendColors(pixels[offset + i], sa, sr, sg, sb);
                }
            }
        }

        /// <summary>
        /// Fills a rectangle.
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="x1">The x-coordinate of the bounding rectangle's left side.</param>
        /// <param name="y1">The y-coordinate of the bounding rectangle's top side.</param>
        /// <param name="x2">The x-coordinate of the bounding rectangle's right side.</param>
        /// <param name="y2">The y-coordinate of the bounding rectangle's bottom side.</param>
        /// <param name="color">The color.</param>
        /// <param name="doAlphaBlend">True if alpha blending should be performed or false if not.</param>
        public static void FillRectangle(this BitmapContext context, int x1, int y1, int x2, int y2, Color color, bool doAlphaBlend = false)
        {
            var col = WriteableBitmapExtensions.ConvertColor(color);
            context.FillRectangle(x1, y1, x2, y2, col, doAlphaBlend);
        }

        /// <summary>
        /// Fills an ellipse.
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="x1">The x-coordinate of the bounding rectangle's left side.</param>
        /// <param name="y1">The y-coordinate of the bounding rectangle's top side.</param>
        /// <param name="x2">The x-coordinate of the bounding rectangle's right side.</param>
        /// <param name="y2">The y-coordinate of the bounding rectangle's bottom side.</param>
        /// <param name="color">The color.</param>
        /// <param name="doAlphaBlend">True if alpha blending should be performed or false if not.</param>
        public static void FillEllipse(this BitmapContext context, int x1, int y1, int x2, int y2, int color, bool doAlphaBlend = false)
        {
            // Calc center and radius
            int xr = (x2 - x1) >> 1;
            int yr = (y2 - y1) >> 1;
            int xc = x1 + xr;
            int yc = y1 + yr;
            context.FillEllipseCentered(xc, yc, xr, yr, color, doAlphaBlend);
        }

        /// <summary>
        /// A Fast Bresenham Type Algorithm For Drawing filled ellipses
        /// Uses a different parameter representation than FillEllipse()
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="xc">The x-coordinate of the ellipses center.</param>
        /// <param name="yc">The y-coordinate of the ellipses center.</param>
        /// <param name="xr">The radius of the ellipse in x-direction.</param>
        /// <param name="yr">The radius of the ellipse in y-direction.</param>
        /// <param name="color">The color.</param>
        /// <param name="doAlphaBlend">True if alpha blending should be performed or false if not.</param>
        public static void FillEllipseCentered(this BitmapContext context, int xc, int yc, int xr, int yr, int color, bool doAlphaBlend = false)
        {
            // Use refs for faster access (really important!) speeds up a lot!
            var pixels = context.Pixels;
            int w = context.Width;
            int h = context.Height;

            // Avoid endless loop
            if (xr < 1 || yr < 1)
            {
                return;
            }

            // Skip completly outside objects
            if (xc - xr >= w || xc + xr < 0 || yc - yr >= h || yc + yr < 0)
            {
                return;
            }

            // Init vars
            int uh, lh, uy, ly, lx, rx;
            int x = xr;
            int y = 0;
            int xrSqTwo = (xr * xr) << 1;
            int yrSqTwo = (yr * yr) << 1;
            int xChg = yr * yr * (1 - (xr << 1));
            int yChg = xr * xr;
            int err = 0;
            int xStopping = yrSqTwo * xr;
            int yStopping = 0;

            int sa = ((color >> 24) & 0xff);
            int sr = ((color >> 16) & 0xff);
            int sg = ((color >> 8) & 0xff);
            int sb = ((color) & 0xff);

            bool noBlending = !doAlphaBlend || sa == 255;

            // Draw first set of points counter clockwise where tangent line slope > -1.
            while (xStopping >= yStopping)
            {
                // Draw 4 quadrant points at once
                // Upper half
                uy = yc + y;
                // Lower half
                ly = yc - y;

                // Clip
                if (uy < 0) uy = 0;
                if (uy >= h) uy = h - 1;
                if (ly < 0) ly = 0;
                if (ly >= h) ly = h - 1;

                // Upper half
                uh = uy * w;
                // Lower half
                lh = ly * w;

                rx = xc + x;
                lx = xc - x;

                // Clip
                if (rx < 0) rx = 0;
                if (rx >= w) rx = w - 1;
                if (lx < 0) lx = 0;
                if (lx >= w) lx = w - 1;

                // Draw line
                if (uy >= 0 && uy < h)
                {
                    // Draw horizontal line in upper half
                    for (var i = lx; i <= rx; i++)
                    {
                        pixels[i + uh] = noBlending ? color : AlphaBlendColors(pixels[i + uh], sa, sr, sg, sb);
                    }
                }
                if (ly >= 0 && ly < h && ly != uy) // Don't overdraw horizontl lines if uy == ly
                {
                    // Draw horizontal line in lower half
                    for (var i = lx; i <= rx; i++)
                    {
                        pixels[i + lh] = noBlending ? color : AlphaBlendColors(pixels[i + lh], sa, sr, sg, sb);
                    }
                }

                y++;
                yStopping += xrSqTwo;
                err += yChg;
                yChg += xrSqTwo;
                if ((xChg + (err << 1)) > 0)
                {
                    x--;
                    xStopping -= yrSqTwo;
                    err += xChg;
                    xChg += yrSqTwo;
                }
            }

            // ReInit vars
            x = 0;
            y = yr;

            // Upper half
            uy = yc + y;
            // Lower half
            ly = yc - y;

            // Clip
            if (uy < 0) uy = 0;
            if (uy >= h) uy = h - 1;
            if (ly < 0) ly = 0;
            if (ly >= h) ly = h - 1;

            // Upper half
            uh = uy * w;
            // Lower half
            lh = ly * w;

            xChg = yr * yr;
            yChg = xr * xr * (1 - (yr << 1));
            err = 0;
            xStopping = 0;
            yStopping = xrSqTwo * yr;

            // Draw second set of points clockwise where tangent line slope < -1.
            while (xStopping <= yStopping)
            {
                // Draw 4 quadrant points at once
                rx = xc + x;
                lx = xc - x;

                // Clip
                if (rx < 0) rx = 0;
                if (rx >= w) rx = w - 1;
                if (lx < 0) lx = 0;
                if (lx >= w) lx = w - 1;

                // Draw line
                if (uy >= 0 && uy < h)
                {
                    // Draw horizontal line in upper half
                    for (var i = lx; i <= rx; i++)
                    {
                        pixels[i + uh] = noBlending ? color : AlphaBlendColors(pixels[i + uh], sa, sr, sg, sb);
                    }
                }
                if (ly >= 0 && ly < h && ly != uy) // Don't overdraw horizontl lines if uy == ly
                {
                    // Draw horizontal line in lower half
                    for (var i = lx; i <= rx; i++)
                    {
                        pixels[i + lh] = noBlending ? color : AlphaBlendColors(pixels[i + lh], sa, sr, sg, sb);
                    }
                }

                x++;
                xStopping += yrSqTwo;
                err += xChg;
                xChg += yrSqTwo;
                if ((yChg + (err << 1)) > 0)
                {
                    y--;
                    // Upper half
                    uy = yc + y;
                    // Lower half
                    ly = yc - y;

                    // Clip
                    if (uy < 0) uy = 0;
                    if (uy >= h) uy = h - 1;
                    if (ly < 0) ly = 0;
                    if (ly >= h) ly = h - 1;

                    // Upper half
                    uh = uy * w;
                    // Lower half
                    lh = ly * w;
                    yStopping -= xrSqTwo;
                    err += yChg;
                    yChg += xrSqTwo;
                }
            }
        }

        /// <summary>
        /// A Fast Bresenham Type Algorithm For Drawing filled ellipses
        /// Uses a different parameter representation than FillEllipse()
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="xc">The x-coordinate of the ellipses center.</param>
        /// <param name="yc">The y-coordinate of the ellipses center.</param>
        /// <param name="xr">The radius of the ellipse in x-direction.</param>
        /// <param name="yr">The radius of the ellipse in y-direction.</param>
        /// <param name="color">The color.</param>
        /// <param name="doAlphaBlend">True if alpha blending should be performed or false if not.</param>
        public static void FillEllipseCentered(this BitmapContext context, int xc, int yc, int xr, int yr, Color color, bool doAlphaBlend = false)
        {
            var col = WriteableBitmapExtensions.ConvertColor(color);
            context.FillEllipseCentered(xc, yc, xr, yr, col, doAlphaBlend);
        }

        #endregion

        #region Common Methods
        
        /// <summary>
        /// Alpha blends the specified source color with the destination color.
        /// </summary>
        /// <param name="pixel">The destination color.</param>
        /// <param name="sa">The source alpha value.</param>
        /// <param name="sr">The source red value.</param>
        /// <param name="sg">The source green value.</param>
        /// <param name="sb">The source blue value.</param>
        /// <returns>The alpha blended color.</returns>
        private static int AlphaBlendColors(int pixel, int sa, int sr, int sg, int sb)
        {
            // Alpha blend
            int destPixel = pixel;
            int da = ((destPixel >> 24) & 0xff);
            int dr = ((destPixel >> 16) & 0xff);
            int dg = ((destPixel >> 8) & 0xff);
            int db = ((destPixel) & 0xff);

            destPixel = ((sa + (((da * (255 - sa)) * 0x8081) >> 23)) << 24) |
                                 ((sr + (((dr * (255 - sa)) * 0x8081) >> 23)) << 16) |
                                 ((sg + (((dg * (255 - sa)) * 0x8081) >> 23)) << 8) |
                                 ((sb + (((db * (255 - sa)) * 0x8081) >> 23)));

            return destPixel;
        }

        /// <summary>
        /// Copies the specified region from the source BitmapContext to the destination BitmapContext.
        /// </summary>
        /// <param name="destContext">The destination context.</param>
        /// <param name="destRect">The rectangle in the destination that should be filled with the source content.</param>
        /// <param name="srcContext">The source context.</param>
        /// <param name="sourceRect">The rectangle in the source that should be copied.</param>
        /// <param name="sourceWidth">The width of the source bitmap.</param>
        public static void Blit(this BitmapContext destContext, Rect destRect, BitmapContext srcContext, Rect sourceRect, int sourceWidth)
        {
            // Simple parameter validation
            int dw = (int)destRect.Width;
            int dh = (int)destRect.Height;
            int sw = (int)sourceRect.Width;
            int sh = (int)sourceRect.Height;
            int dpw = destContext.Width;
            int dph = destContext.Height;

            // Calculate actual widths to copy
            dw = Math.Min(dw, dpw - (int)destRect.X);
            dh = Math.Min(dh, dph - (int)destRect.Y);
            sw = Math.Min(sw, sourceWidth - (int)sourceRect.X);
            sh = Math.Min(sh, srcContext.Length / sourceWidth - (int)sourceRect.Y);

            // Check if nothing to do
            if (sw <= 0 || sh <= 0 || dw <= 0 || dh <= 0) return;

            // If width or height is different, use scaling
            if (sw != dw || sh != dh)
            {
                WriteableBitmapExtensions.BlitScaled(destContext, dpw, dph, destRect, srcContext, sourceRect, sourceWidth);
                return;
            }

            // Calculate start index in source and destination
            int xs0 = (int)sourceRect.X;
            int ys0 = (int)sourceRect.Y;
            int xd0 = (int)destRect.X;
            int yd0 = (int)destRect.Y;

            var srcPixels = srcContext.Pixels;
            var destPixels = destContext.Pixels;
            int sd = sourceWidth;
            int dd = dpw;

            // Copy identical sized regions
            for (int y = 0; y < dh; y++)
            {
                int srcIdx = ((ys0 + y) * sd) + xs0;
                int destIdx = ((yd0 + y) * dd) + xd0;

                for (int x = 0; x < dw; x++)
                {
                    destPixels[destIdx + x] = srcPixels[srcIdx + x];
                }
            }
        }

        /// <summary>
        /// Copies the specified region from the source BitmapContext to the destination BitmapContext.
        /// </summary>
        /// <param name="destContext">The destination context.</param>
        /// <param name="destPosition">The position in the destination context where the source should be copied to.</param>
        /// <param name="srcContext">The source context.</param>
        /// <param name="sourceRect">The rectangle in the source that should be copied.</param>
        /// <param name="sourceWidth">The width of the source bitmap.</param>
        public static void Blit(this BitmapContext destContext, Point destPosition, BitmapContext srcContext, Rect sourceRect, int sourceWidth)
        {
            Rect destRect = new Rect(destPosition, new Size(sourceRect.Width, sourceRect.Height));
            destContext.Blit(destRect, srcContext, sourceRect, sourceWidth);
        }

        #region Transform Methods

        /// <summary>
        /// Flips (mirrors) the BitmapContext.
        /// </summary>
        /// <param name="context">The BitmapContext.</param>
        /// <param name="flipMode">The flip mode.</param>
        /// <returns>A new BitmapContext containing the flipped image data.</returns>
        public static BitmapContext Flip(this BitmapContext context, FlipMode flipMode)
        {
            // Use refs for faster access (really important!) speeds up a lot!
            var w = context.Width;
            var h = context.Height;
            var p = context.Pixels;
            var result = new int[p.Length];
            var i = 0;

            if (flipMode == FlipMode.Horizontal)
            {
                for (var y = h - 1; y >= 0; y--)
                {
                    for (var x = 0; x < w; x++)
                    {
                        var srcInd = y * w + x;
                        result[i] = p[srcInd];
                        i++;
                    }
                }
            }
            else if (flipMode == FlipMode.Vertical)
            {
                for (var y = 0; y < h; y++)
                {
                    for (var x = w - 1; x >= 0; x--)
                    {
                        var srcInd = y * w + x;
                        result[i] = p[srcInd];
                        i++;
                    }
                }
            }

            return new BitmapContext(result, w, h, context.Format);
        }

        #endregion
    }
}