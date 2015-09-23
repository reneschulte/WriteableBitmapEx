#region Header
//
//   Project:           WriteableBitmapEx - Silverlight WriteableBitmap extensions
//   Description:       Blit Sample for the WriteableBitmap extension methods.
//
//   Changed by:        $Author: unknown $
//   Changed on:        $Date: 2015-02-24 20:36:41 +0100 (Di, 24 Feb 2015) $
//   Changed in:        $Revision: 112951 $
//   Project:           $URL: https://writeablebitmapex.svn.codeplex.com/svn/trunk/Source/WriteableBitmapExBlitSample/Particle.cs $
//   Id:                $Id: Particle.cs 112951 2015-02-24 19:36:41Z unknown $
//
//
//   Copyright © 2009-2015 Bill Reiss, Rene Schulte and WriteableBitmapEx Contributors
//
//   This code is open source. Please read the License.txt for details. No worries, we won't sue you! ;)
//
#endregion

#if NETFX_CORE
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using System.Collections.Generic;
using Windows.UI;
#else
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif

namespace WriteableBitmapExBlitSample
{
   public class Particle
   {
      #region Fields

      public Point Position;
      public Point Velocity;
      public Color Color;
      public double Lifespan;
      public double Elapsed;

      #endregion

      #region Methods

      public void Initiailize()
      {
         Elapsed = 0;
      }

      public void Update(double elapsedSeconds)
      {
         Elapsed += elapsedSeconds;
         if (Elapsed > Lifespan)
         {
            Color.A = 0;
            return;
         }
         Color.A = (byte)(255 - ((255 * Elapsed)) / Lifespan);
         Position.X += Velocity.X * elapsedSeconds;
         Position.Y += Velocity.Y * elapsedSeconds;
      }

      #endregion
   }
}
