# WriteableBitmapEx

The WriteableBitmapEx library is a collection of extension methods for the [WriteableBitmap](http://msdn.microsoft.com/en-us/library/system.windows.media.imaging.writeablebitmap%28VS.95%29.aspx). The WriteableBitmap class is available for all XAML flavors including WPF, Windows 10 UWP, Windows Phone, WinRT Windows Store XAML and Silverlight. It supports the .NET Framework and .NET Core 3 and was even ported to [Windows Embedded](http://wbexembedded.codeplex.com). WriteableBitmapEx allows the direct manipulation of a bitmap and can be used for image manipulation, to generate fast procedural images by drawing directly to a bitmap and more.  

The WriteableBitmap API is very minimalistic and there's only the raw [Pixels](http://msdn.microsoft.com/en-us/library/system.windows.media.imaging.writeablebitmap.pixels(VS.95).aspx) array for such operations. The WriteableBitmapEx library tries to compensate that with extensions methods that are easy to use like built in methods and offer [GDI+](http://msdn.microsoft.com/en-us/library/ms533797(v=VS.85).aspx) like functionality. The library extends the WriteableBitmap class with elementary and fast (2D drawing) functionality, conversion methods and functions to combine (blit) WriteableBitmaps.  

The extension methods are grouped into different C# files using a partial class approach. It is possible to include just a few methods by using the specific source code files directly or the full functionality via the built binaries.  

The latest binaries are available as [NuGet package](http://nuget.org/List/Packages/WriteableBitmapEx).

Please use the [GitHub Issues functionality](https://github.com/teichgraf/WriteableBitmapEx/issues) to add new issues which are not already reported. 

![wbx_announcement.png](https://4.bp.blogspot.com/-kGQh1bS1qlk/Sx1KnIEhZ3I/AAAAAAAAAI0/z3P5rYjXuk8/s1600/wbx_announcement.png)


# News 
* now supports text rendering (outline and fill)

# Features

[GDI+](http://msdn.microsoft.com/en-us/library/ms533797(v=VS.85).aspx) like drawing functionality for the WriteableBitmap.
Support for WPF, Windows 10 UWP (, Windows 8/8.1 WinRT XAML, Windows Phone Silverlight, Windows Phone WinRT and desktop Silverlight).

*   Base
    *   Support for the [Color structure](http://msdn.microsoft.com/en-us/library/system.windows.media.color(VS.95).aspx) (alpha premultiplication will be performed)
    *   Also overloads for faster int32 as color (assumed to be already alpha premultiplied)
    *   SetPixel method with various overloads
    *   GetPixel method to get the pixel color at a specified x, y coordinate
    *   Fast Clear methods
    *   Fast Clone method to copy a WriteableBitmap
    *   ForEach method to apply a given function to all pixels of the bitmap
*   Transformation
    *   Crop method to extract a defined region
    *   Resize method with support for bilinear interpolation and nearest neighbor
    *   Rotate in 90° steps clockwise and any arbitrary angle
    *   Flip vertical and horizontal
*   Shapes
    *   Fast line drawing algorithms including various anti-aliased algorithm
    *   Variable stroke thickness, dotted and penned / stamp lines
    *   Ellipse, polyline, quad, rectangle and triangle
    *   Cubic Beziér, Cardinal spline and closed curves
*   Filled shapes
    *   Fast ellipse and rectangle fill method
    *   Triangle, quad, simple and complex polygons
    *   Beziér and Cardinal spline curves
*	Text
	*	Fill and draw outline of text strings. text is highly flexible, it is instance of `FormattedText` thus any text and characted which is supported by wpf, can be rendered (options like `FlowDirection`, `FontWeight` and ... can be changed).
*   Blitting
    *   Different blend modes including alpha, additive, subtractive, multiply, mask and none
    *   Optimized fast path for non blended blitting
    *   Special BlitRender to apply affine transformation with bilinear interpolation
*   Filtering
    *   Convolution, Blur
    *   Brightness, contrast, gamma adjustments
    *   Gray/brightness, invert
*   Conversion
    *   Convert a WriteableBitmap to a byte array
    *   Create a WriteableBitmap from a byte array
    *   Create a WriteableBitmap easily from the application resource or content
    *   Create a WriteableBitmap from an any platform supported image stream
    *   Write a WriteableBitmap as a [TGA image](http://en.wikipedia.org/wiki/Truevision_TGA) to a stream
    *   Separate extension method to save as a [PNG image](http://en.wikipedia.org/wiki/Portable_Network_Graphics). Download [here](http://writeablebitmapex.codeplex.com/discussions/274445)
*   Windows Phone specific methods
    *   Save to media library and the camera roll


# Performance!

The WriteableBitmapEx methods are much faster than the XAML [Shape](http://msdn.microsoft.com/en-us/library/system.windows.shapes.shape(VS.95).aspx) subclasses. For example, the WriteableBitmapEx line drawing approach is more than 20-30 times faster than the Silverlight [Line](http://msdn.microsoft.com/en-us/library/system.windows.shapes.line(VS.95).aspx) element. If a lot of shapes need to be drawn, the WriteableBitmapEx methods are the right choice.

# Easy to use!
```cs
// Initialize the WriteableBitmap with size 512x512 and set it as source of an Image control
WriteableBitmap writeableBmp = BitmapFactory.New(512, 512);
ImageControl.Source = writeableBmp;
using(writeableBmp.GetBitmapContext())
{

   // Load an image from the calling Assembly's resources via the relative path
   writeableBmp = BitmapFactory.New(1, 1).FromResource("Data/flower2.png");

   // Clear the WriteableBitmap with white color
   writeableBmp.Clear(Colors.White);

   // Set the pixel at P(10, 13) to black
   writeableBmp.SetPixel(10, 13, Colors.Black);

   // Get the color of the pixel at P(30, 43)
   Color color = writeableBmp.GetPixel(30, 43);

   // Green line from P1(1, 2) to P2(30, 40)
   writeableBmp.DrawLine(1, 2, 30, 40, Colors.Green);

   // Line from P1(1, 2) to P2(30, 40) using the fastest draw line method 
   int[] pixels = writeableBmp.Pixels;
   int w = writeableBmp.PixelWidth;
   int h = writeableBmp.PixelHeight;
   WriteableBitmapExtensions.DrawLine(pixels, w, h, 1, 2, 30, 40, myIntColor);

   // Blue anti-aliased line from P1(10, 20) to P2(50, 70) with a stroke of 5
   writeableBmp.DrawLineAa(10, 20, 50, 70, Colors.Blue, 5);
   
   // Fills a text on the bitmap, Font, size, weight and almost any option is changable, all text supported with WPF is also supported here including Persian, Arabic, Chinese etc
   var formattedText = new FormattedText("Test String", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(new FontFamily("Sans MS"), FontStyles.Normal, FontWeights.Medium, FontStretches.Normal), 80.0, System.Windows.Media.Brushes.Black);
   writeableBmp.FillText(formattedText, 100, 100, Colors.Blue, 5);
   
   // Black triangle with the points P1(10, 5), P2(20, 40) and P3(30, 10)
   writeableBmp.DrawTriangle(10, 5, 20, 40, 30, 10, Colors.Black);

   // Red rectangle from the point P1(2, 4) that is 10px wide and 6px high
   writeableBmp.DrawRectangle(2, 4, 12, 10, Colors.Red);

   // Filled blue ellipse with the center point P1(2, 2) that is 8px wide and 5px high
   writeableBmp.FillEllipseCentered(2, 2, 8, 5, Colors.Blue);

   // Closed green polyline with P1(10, 5), P2(20, 40), P3(30, 30) and P4(7, 8)
   int[] p = new int[] { 10, 5, 20, 40, 30, 30, 7, 8, 10, 5 };
   writeableBmp.DrawPolyline(p, Colors.Green);

   // Cubic Beziér curve from P1(5, 5) to P4(20, 7) 
   // with the control points P2(10, 15) and P3(15, 0)
   writeableBmp.DrawBezier(5, 5, 10, 15, 15, 0, 20, 7,  Colors.Purple);

   // Cardinal spline with a tension of 0.5 
   // through the points P1(10, 5), P2(20, 40) and P3(30, 30)
   int[] pts = new int[] { 10, 5, 20, 40, 30, 30};
   writeableBmp.DrawCurve(pts, 0.5,  Colors.Yellow);

   // A filled Cardinal spline with a tension of 0.5 
   // through the points P1(10, 5), P2(20, 40) and P3(30, 30) 
   writeableBmp.FillCurveClosed(pts, 0.5,  Colors.Green);

   // Blit a bitmap using the additive blend mode at P1(10, 10)
   writeableBmp.Blit(new Point(10, 10), bitmap, sourceRect, Colors.White, WriteableBitmapExtensions.BlendMode.Additive);

   // Override all pixels with a function that changes the color based on the coordinate
   writeableBmp.ForEach((x, y, color) => Color.FromArgb(color.A, (byte)(color.R / 2), (byte)(x * y), 100));

} // Invalidate and present in the Dispose call

// Take snapshot
var clone = writeableBmp.Clone();

// Save to a TGA image stream (file for example)
writeableBmp.WriteTga(stream);

// Crops the WriteableBitmap to a region starting at P1(5, 8) and 10px wide and 10px high
var cropped = writeableBmp.Crop(5, 8, 10, 10);

// Rotates a copy of the WriteableBitmap 90 degress clockwise and returns the new copy
var rotated = writeableBmp.Rotate(90);

// Flips a copy of the WriteableBitmap around the horizontal axis and returns the new copy
var flipped = writeableBmp.Flip(FlipMode.Horizontal);

// Resizes the WriteableBitmap to 200px wide and 300px high using bilinear interpolation
var resized = writeableBmp.Resize(200, 300, WriteableBitmapExtensions.Interpolation.Bilinear);
```

# Additional Information

The WriteableBitmapEx library has its origin in several blog posts that also describe the implemenation and usage of some aspects in detail. The blog posts might be seen as the documentation:
* [WriteableBitmap Extension Methods](http://kodierer.blogspot.com/2009/07/writeablebitmap-extension-methods.html) introduced the SetPixel methods.  
* [Drawing Lines - Silverlight WriteableBitmap Extensions II](http://kodierer.blogspot.com/2009/10/drawing-lines-silverlight.html) provided the DrawLine methods.   
* [Drawing Shapes - Silverlight WriteableBitmap Extensions III](http://kodierer.blogspot.com/2009/11/drawing-shapes-silverlight.html) brought the shape functionality (ellipse, polyline, quad, rectangle, triangle).  
* [Convert, Encode And Decode Silverlight WriteableBitmap Data](http://kodierer.blogspot.com/2009/11/convert-encode-and-decode-silverlight.html) came with the byte array conversion methods and hows how to encode / decode a WriteableBitmap to JPEG.  
* [Blitting and Blending with Silverlight’s WriteableBitmap](http://blogs.silverarcade.com/silverlight-games-101/15/silverlight-blitting-and-blending-with-silverlights-writeablebitmap/) provided the Blit functions.  
* [WriteableBitmapEx - WriteableBitmap extensions now on CodePlex](http://kodierer.blogspot.com/2009/12/writeablebitmapex-writeablebitmap.html) announced this project.  
* [Quick and Dirty Output of WriteableBitmap as TGA Image](http://nokola.com/blog/post/2010/01/21/Quick-and-Dirty-Output-of-WriteableBitmap-as-TGA-Image.aspx) provided the original TgaWrite function.  
* [Rounder, Faster, Better - WriteableBitmapEx 0.9.0.0](http://kodierer.blogspot.com/2010/01/rounder-faster-better-writeablebitmapex.html) announced version 0.9.0.0 and gives some further information about the curve sample.  
* [Let it ring - WriteableBitmapEx for Windows Phone](http://kodierer.blogspot.com/2010/03/let-it-ring-writeablebitmapex-for.html) introtuced the WriteableBitmapEx version for the Windows Phone and a sample.  
* [Filled To The Bursting Point - WriteableBitmapEx 0.9.5.0](http://kodierer.blogspot.com/2010/06/filled-to-bursting-point.html) announced version 0.9.5.0, has some information about the new Fill methods and comes with a nice sample.  
* [One Bitmap to Rule Them All - WriteableBitmapEx for WinRT Metro Style](http://kodierer.blogspot.de/2012/05/one-bitmap-to-rule-them-all.html) announced version 1.0.0.0 and provides some background about the WinRT Metro Style version. 
* [Space Navigator](https://www.codeproject.com/Articles/1225848/Space-Navigator-A-Journey-into-WPFs-Display-Sub-Sy) is a great project on Code Project that compares the performance of the WriteableBitmapEx to other methods in WPF for visualizing large hierarchical data in a Tree Map.  

# Support it

[![Donate](https://www.paypal.com/en_US/i/btn/btn_donateCC_LG_global.gif "Donate")](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RPXX29MESX8A2)

# Credits

* [Rene Schulte](http://blog.rene-schulte.info) started this project, maintains it and provided most of the code.  
* [Dr. Andrew Burnett-Thompson](http://www.linkedin.com/profile/view?id=54694225)and his team proposed the portability refactoring, provided the WPF port and much more beneficial functions.  
* [Nikola Mihaylov (Nokola)](http://nokola.com) made some optimizations on the DrawLine and DrawRectangle methods, provided the original TgaWrite and the anti-aliased line drawing function.  
* [Bill Reiss](http://blogs.silverarcade.com/silverlight-games-101) wrote the Blit methods. 

And all the other amazing contributors you can see in the Contributors tab here on GitHub.
