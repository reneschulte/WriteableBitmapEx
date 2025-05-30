﻿#region Header
//
//   Project:           WriteableBitmapEx - WriteableBitmap extensions
//   Description:       Collection of extension methods for the WriteableBitmap class.
//
//   Changed by:        $Author: unknown $
//   Changed on:        $Date: 2015-03-05 18:18:24 +0100 (Do, 05 Mrz 2015) $
//   Changed in:        $Revision: 113191 $
//   Project:           $URL: https://writeablebitmapex.svn.codeplex.com/svn/trunk/Source/WriteableBitmapEx/WriteableBitmapBaseExtensions.cs $
//   Id:                $Id: WriteableBitmapBaseExtensions.cs 113191 2015-03-05 17:18:24Z unknown $
//
//
//   Copyright © 2009-2015 Rene Schulte and WriteableBitmapEx Contributors
//
//   This code is open source. Please read the License.txt for details. No worries, we won't sue you! ;)
//
#endregion

using System;
using System.Runtime.CompilerServices;

#if NETFX_CORE
namespace Windows.UI.Xaml.Media.Imaging
#else
namespace System.Windows.Media.Imaging
#endif
{
    /// <summary>
    /// Collection of extension methods for the WriteableBitmap class.
    /// </summary>
    public
#if WPF 
    unsafe 
#endif
 static partial class WriteableBitmapExtensions
    {
        #region Fields

        internal const int SizeOfArgb = 4;

        #endregion

        #region Methods

        #region General

        public static int ConvertColor(double opacity, Color color)
        {
            if (opacity < 0.0 || opacity > 1.0)
            {
                throw new ArgumentOutOfRangeException("opacity", "Opacity must be between 0.0 and 1.0");
            }

            color.A = (byte)(color.A * opacity);

            return ConvertColor(color);
        }

        public static int ConvertColor(Color color)
        {
            var col = 0;

            if (color.A != 0)
            {
                var a = color.A + 1;
                col = (color.A << 24)
                  | ((byte)((color.R * a) >> 8) << 16)
                  | ((byte)((color.G * a) >> 8) << 8)
                  | ((byte)((color.B * a) >> 8));
            }

            return col;
        }

        // same as ConvertColor() but takes care of the transparency
        public static int ConvertColorT(Color color)
        {
            int col = 0;

            col = color.A << 24 | color.R << 16 | color.G << 8 | color.B;

            return col;
        }

        /// <summary>
        /// Blends two pixels regarding their alpha
        /// </summary>
        /// <returns>blended</returns>
        [MethodImpl(256)]
        public static int AlphaBlendArgbPixels(byte a1, byte r1, byte g1, byte b1, byte a2, byte r2, byte g2, byte b2)
        {
            //inlined
            //blends two pixel and return ARGB result as int
            //[MethodImpl(256)] equals to MethodImplOptions.AggressiveInlining, add support for net40, tested on net40 working
            //https://stackoverflow.com/a/8746128
            //https://stackoverflow.com/a/43060488
            //s:1
            //d:2

            var a1not = (byte)(0xff - a1);

            var ad = (byte)((a1 * a1 + a2 * a1not) >> 8);
            var rd = (byte)((r1 * a1 + r2 * a1not) >> 8);
            var gd = (byte)((g1 * a1 + g2 * a1not) >> 8);
            var bd = (byte)((b1 * a1 + b2 * a1not) >> 8);

            return ad << 24 | rd << 16 | gd << 8 | bd;
        }

        /// <summary>
        /// Fills the whole WriteableBitmap with a color.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="color">The color used for filling.</param>
        public static void Clear(this WriteableBitmap bmp, Color color)
        {
            var col = ConvertColor(color);
            using (var context = bmp.GetBitmapContext())
            {
                var pixels = context.Pixels;
                var w = context.Width;
                var h = context.Height;
                var len = w * SizeOfArgb;

                // Fill first line
                for (var x = 0; x < w; x++)
                {
                    pixels[x] = col;
                }

                // Copy first line
                var blockHeight = 1;
                var y = 1;
                while (y < h)
                {
                    BitmapContext.BlockCopy(context, 0, context, y * len, blockHeight * len);
                    y += blockHeight;
                    blockHeight = Math.Min(2 * blockHeight, h - y);
                }
            }
        }

        /// <summary>
        /// Fills the whole WriteableBitmap with an empty color (0).
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        public static void Clear(this WriteableBitmap bmp)
        {
            using (var context = bmp.GetBitmapContext())
            {
                context.Clear();
            }
        }

        /// <summary>
        /// Clones the specified WriteableBitmap.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <returns>A copy of the WriteableBitmap.</returns>
        public static WriteableBitmap Clone(this WriteableBitmap bmp)
        {
            using (var srcContext = bmp.GetBitmapContext(ReadWriteMode.ReadOnly))
            {
                var result = BitmapFactory.New(srcContext.Width, srcContext.Height);
                using (var destContext = result.GetBitmapContext())
                {
                    BitmapContext.BlockCopy(srcContext, 0, destContext, 0, srcContext.Length * SizeOfArgb);
                }
                return result;
            }
        }

        #endregion

        #region ForEach

        /// <summary>
        /// Applies the given function to all the pixels of the bitmap in 
        /// order to set their color.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="func">The function to apply. With parameters x, y and a color as a result</param>
        public static void ForEach(this WriteableBitmap bmp, Func<int, int, Color> func)
        {
            using (var context = bmp.GetBitmapContext())
            {
                var pixels = context.Pixels;
                int w = context.Width;
                int h = context.Height;
                int index = 0;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        var color = func(x, y);
                        pixels[index++] = ConvertColor(color);
                    }
                }
            }
        }

        /// <summary>
        /// Applies the given function to all the pixels of the bitmap in 
        /// order to set their color.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="func">The function to apply. With parameters x, y, source color and a color as a result</param>
        public static void ForEach(this WriteableBitmap bmp, Func<int, int, Color, Color> func)
        {
            using (var context = bmp.GetBitmapContext())
            {
                var pixels = context.Pixels;
                var w = context.Width;
                var h = context.Height;
                var index = 0;

                for (var y = 0; y < h; y++)
                {
                    for (var x = 0; x < w; x++)
                    {
                        var c = pixels[index];

                        // Premultiplied Alpha!
                        var a = (byte)(c >> 24);
                        // Prevent division by zero
                        int ai = a;
                        if (ai == 0)
                        {
                            ai = 1;
                        }
                        // Scale inverse alpha to use cheap integer mul bit shift
                        ai = ((255 << 8) / ai);
                        var srcColor = Color.FromArgb(a,
                                                      (byte)((((c >> 16) & 0xFF) * ai) >> 8),
                                                      (byte)((((c >> 8) & 0xFF) * ai) >> 8),
                                                      (byte)((((c & 0xFF) * ai) >> 8)));

                        var color = func(x, y, srcColor);
                        pixels[index++] = ConvertColor(color);
                    }
                }
            }
        }

        #endregion

        #region Get Pixel / Brightness

        /// <summary>
        /// Gets the color of the pixel at the x, y coordinate as integer.  
        /// For best performance this method should not be used in iterative real-time scenarios. Implement the code directly inside a loop.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="x">The x coordinate of the pixel.</param>
        /// <param name="y">The y coordinate of the pixel.</param>
        /// <returns>The color of the pixel at x, y.</returns>
        public static int GetPixeli(this WriteableBitmap bmp, int x, int y)
        {
            using (var context = bmp.GetBitmapContext(ReadWriteMode.ReadOnly))
            {
                return context.Pixels[y * context.Width + x];
            }
        }

        /// <summary>
        /// Gets the color of the pixel at the x, y coordinate as a Color struct.  
        /// For best performance this method should not be used in iterative real-time scenarios. Implement the code directly inside a loop.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="x">The x coordinate of the pixel.</param>
        /// <param name="y">The y coordinate of the pixel.</param>
        /// <returns>The color of the pixel at x, y as a Color struct.</returns>
        public static Color GetPixel(this WriteableBitmap bmp, int x, int y)
        {
            using (var context = bmp.GetBitmapContext(ReadWriteMode.ReadOnly))
            {
                var c = context.Pixels[y * context.Width + x];
                var a = (byte)(c >> 24);

                // Prevent division by zero
                int ai = a;
                if (ai == 0)
                {
                    ai = 1;
                }

                // Scale inverse alpha to use cheap integer mul bit shift
                ai = ((255 << 8) / ai);
                return Color.FromArgb(a,
                                     (byte)((((c >> 16) & 0xFF) * ai) >> 8),
                                     (byte)((((c >> 8) & 0xFF) * ai) >> 8),
                                     (byte)((((c & 0xFF) * ai) >> 8)));
            }
        }

        /// <summary>
        /// Gets the brightness / luminance of the pixel at the x, y coordinate as byte.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="x">The x coordinate of the pixel.</param>
        /// <param name="y">The y coordinate of the pixel.</param>
        /// <returns>The brightness of the pixel at x, y.</returns>
        public static byte GetBrightness(this WriteableBitmap bmp, int x, int y)
        {
            using (var context = bmp.GetBitmapContext(ReadWriteMode.ReadOnly))
            {
                // Extract color components
                var c = context.Pixels[y * context.Width + x];
                var r = (byte)(c >> 16);
                var g = (byte)(c >> 8);
                var b = (byte)(c);

                // Convert to gray with constant factors 0.2126, 0.7152, 0.0722
                return (byte)((r * 6966 + g * 23436 + b * 2366) >> 15);
            }
        }

        #endregion

        #region SetPixel

        #region Without alpha

        /// <summary>
        /// Sets the color of the pixel using a precalculated index (faster). 
        /// For best performance this method should not be used in iterative real-time scenarios. Implement the code directly inside a loop.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="index">The coordinate index.</param>
        /// <param name="r">The red value of the color.</param>
        /// <param name="g">The green value of the color.</param>
        /// <param name="b">The blue value of the color.</param>
        public static void SetPixeli(this WriteableBitmap bmp, int index, byte r, byte g, byte b)
        {
            using (var context = bmp.GetBitmapContext())
            {
                context.Pixels[index] = (255 << 24) | (r << 16) | (g << 8) | b;
            }
        }

        /// <summary>
        /// Sets the color of the pixel. 
        /// For best performance this method should not be used in iterative real-time scenarios. Implement the code directly inside a loop.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="x">The x coordinate (row).</param>
        /// <param name="y">The y coordinate (column).</param>
        /// <param name="r">The red value of the color.</param>
        /// <param name="g">The green value of the color.</param>
        /// <param name="b">The blue value of the color.</param>
        public static void SetPixel(this WriteableBitmap bmp, int x, int y, byte r, byte g, byte b)
        {
            using (var context = bmp.GetBitmapContext())
            {
                context.Pixels[y * context.Width + x] = (255 << 24) | (r << 16) | (g << 8) | b;
            }
        }

        #endregion

        #region With alpha

        /// <summary>
        /// Sets the color of the pixel including the alpha value and using a precalculated index (faster). 
        /// For best performance this method should not be used in iterative real-time scenarios. Implement the code directly inside a loop.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="index">The coordinate index.</param>
        /// <param name="a">The alpha value of the color.</param>
        /// <param name="r">The red value of the color.</param>
        /// <param name="g">The green value of the color.</param>
        /// <param name="b">The blue value of the color.</param>
        public static void SetPixeli(this WriteableBitmap bmp, int index, byte a, byte r, byte g, byte b)
        {
            using (var context = bmp.GetBitmapContext())
            {
                context.Pixels[index] = (a << 24) | (r << 16) | (g << 8) | b;
            }
        }

        /// <summary>
        /// Sets the color of the pixel including the alpha value. 
        /// For best performance this method should not be used in iterative real-time scenarios. Implement the code directly inside a loop.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="x">The x coordinate (row).</param>
        /// <param name="y">The y coordinate (column).</param>
        /// <param name="a">The alpha value of the color.</param>
        /// <param name="r">The red value of the color.</param>
        /// <param name="g">The green value of the color.</param>
        /// <param name="b">The blue value of the color.</param>
        public static void SetPixel(this WriteableBitmap bmp, int x, int y, byte a, byte r, byte g, byte b)
        {
            using (var context = bmp.GetBitmapContext())
            {
                context.Pixels[y * context.Width + x] = (a << 24) | (r << 16) | (g << 8) | b;
            }
        }

        #endregion

        #region With System.Windows.Media.Color

        /// <summary>
        /// Sets the color of the pixel using a precalculated index (faster). 
        /// For best performance this method should not be used in iterative real-time scenarios. Implement the code directly inside a loop.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="index">The coordinate index.</param>
        /// <param name="color">The color.</param>
        public static void SetPixeli(this WriteableBitmap bmp, int index, Color color)
        {
            using (var context = bmp.GetBitmapContext())
            {
                context.Pixels[index] = ConvertColor(color);
            }
        }

        /// <summary>
        /// Sets the color of the pixel. 
        /// For best performance this method should not be used in iterative real-time scenarios. Implement the code directly inside a loop.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="x">The x coordinate (row).</param>
        /// <param name="y">The y coordinate (column).</param>
        /// <param name="color">The color.</param>
        public static void SetPixel(this WriteableBitmap bmp, int x, int y, Color color)
        {
            using (var context = bmp.GetBitmapContext())
            {
                context.Pixels[y * context.Width + x] = ConvertColor(color);
            }
        }

        /// <summary>
        /// Sets the color of the pixel using an extra alpha value and a precalculated index (faster). 
        /// For best performance this method should not be used in iterative real-time scenarios. Implement the code directly inside a loop.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="index">The coordinate index.</param>
        /// <param name="a">The alpha value of the color.</param>
        /// <param name="color">The color.</param>
        public static void SetPixeli(this WriteableBitmap bmp, int index, byte a, Color color)
        {
            using (var context = bmp.GetBitmapContext())
            {
                // Add one to use mul and cheap bit shift for multiplicaltion
                var ai = a + 1;
                context.Pixels[index] = (a << 24)
                           | ((byte)((color.R * ai) >> 8) << 16)
                           | ((byte)((color.G * ai) >> 8) << 8)
                           | ((byte)((color.B * ai) >> 8));
            }
        }

        /// <summary>
        /// Sets the color of the pixel using an extra alpha value. 
        /// For best performance this method should not be used in iterative real-time scenarios. Implement the code directly inside a loop.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="x">The x coordinate (row).</param>
        /// <param name="y">The y coordinate (column).</param>
        /// <param name="a">The alpha value of the color.</param>
        /// <param name="color">The color.</param>
        public static void SetPixel(this WriteableBitmap bmp, int x, int y, byte a, Color color)
        {
            using (var context = bmp.GetBitmapContext())
            {
                // Add one to use mul and cheap bit shift for multiplicaltion
                var ai = a + 1;
                context.Pixels[y * context.Width + x] = (a << 24)
                                             | ((byte)((color.R * ai) >> 8) << 16)
                                             | ((byte)((color.G * ai) >> 8) << 8)
                                             | ((byte)((color.B * ai) >> 8));
            }
        }

        /// <summary>
        /// Sets the color of the pixel using a precalculated index (faster).  
        /// For best performance this method should not be used in iterative real-time scenarios. Implement the code directly inside a loop.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="index">The coordinate index.</param>
        /// <param name="color">The color.</param>
        public static void SetPixeli(this WriteableBitmap bmp, int index, int color)
        {
            using (var context = bmp.GetBitmapContext())
            {
                context.Pixels[index] = color;
            }
        }

        /// <summary>
        /// Sets the color of the pixel. 
        /// For best performance this method should not be used in iterative real-time scenarios. Implement the code directly inside a loop.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="x">The x coordinate (row).</param>
        /// <param name="y">The y coordinate (column).</param>
        /// <param name="color">The color.</param>
        public static void SetPixel(this WriteableBitmap bmp, int x, int y, int color)
        {
            using (var context = bmp.GetBitmapContext())
            {
                context.Pixels[y * context.Width + x] = color;
            }
        }

        #endregion

        #endregion

        #endregion
    }
}