#region Header
//
//   Project:           WriteableBitmapEx - Silverlight WriteableBitmap extensions
//   Description:       Collection of draw spline extension methods for the Silverlight WriteableBitmap class.
//
//   Changed by:        $Author: unknown $
//   Changed on:        $Date: 2015-02-24 20:36:41 +0100 (Di, 24 Feb 2015) $
//   Changed in:        $Revision: 112951 $
//   Project:           $URL: https://writeablebitmapex.svn.codeplex.com/svn/trunk/Source/WriteableBitmapExWinPhoneXnaDependant/WriteableBitmapWindowsPhoneXnaExtensions.cs $
//   Id:                $Id: WriteableBitmapWindowsPhoneXnaExtensions.cs 112951 2015-02-24 19:36:41Z unknown $
//
//
//   Copyright © 2009-2015 Rene Schulte and WriteableBitmapEx Contributors
//
//   This code is open source. Please read the License.txt for details. No worries, we won't sue you! ;)
//
#endregion

using System.IO;
using Microsoft.Xna.Framework.Media;

namespace System.Windows.Media.Imaging
{
   /// <summary>
   /// Collection of draw spline extension methods for the Silverlight WriteableBitmap class.
   /// </summary>
   public static class WriteableBitmapExtensionsXna
   {
      #region Methods

      /// <summary>
      /// Saves the WriteableBitmap encoded as JPEG to the Media library using the best quality of 100.
      /// </summary>
      /// <param name="bitmap">The WriteableBitmap to save.</param>
      /// <param name="name">The name of the destination file.</param>
      /// <param name="saveToCameraRoll">If true the bitmap will be saved to the camera roll, otherwise it will be written to the default saved album.</param>
      public static Picture SaveToMediaLibrary(this WriteableBitmap bitmap, string name, bool saveToCameraRoll = false)
      {
         return SaveToMediaLibrary(bitmap, name, 100, saveToCameraRoll);
      }

      /// <summary>
      /// Saves the WriteableBitmap encoded as JPEG to the Media library.
      /// </summary>
      /// <param name="bitmap">The WriteableBitmap to save.</param>
      /// <param name="name">The name of the destination file.</param>
      /// <param name="quality">The quality for JPEG encoding has to be in the range 0-100, 
      /// where 100 is the best quality with the largest size.</param>
      /// <param name="saveToCameraRoll">If true the bitmap will be saved to the camera roll, otherwise it will be written to the default saved album.</param>
      public static Picture SaveToMediaLibrary(this WriteableBitmap bitmap, string name, int quality, bool saveToCameraRoll = false)
      {
         using (var stream = new MemoryStream())
         {
            // Save the picture to the WP media library
            bitmap.SaveJpeg(stream, bitmap.PixelWidth, bitmap.PixelHeight, 0, quality);
            stream.Seek(0, SeekOrigin.Begin);
            var mediaLibrary = new MediaLibrary();
            return saveToCameraRoll ? mediaLibrary.SavePictureToCameraRoll(name, stream) : mediaLibrary.SavePicture(name, stream);
         }
      }
      
      #endregion
   }
}