#region Header
//
//   Project:           Silverlight procedural Plant
//
//   Changed by:        $Author: unknown $
//   Changed on:        $Date: 2015-02-24 20:36:41 +0100 (Di, 24 Feb 2015) $
//   Changed in:        $Revision: 112951 $
//   Project:           $URL: https://writeablebitmapex.svn.codeplex.com/svn/trunk/Source/WriteableBitmapExCurveSample/Plant/BranchPoint.cs $
//   Id:                $Id: BranchPoint.cs 112951 2015-02-24 19:36:41Z unknown $
//
//
//   Copyright © 2010-2015 Rene Schulte and WriteableBitmapEx Contributors
//
//   This code is open source. Please read the License.txt for details. No worries, we won't sue you! ;)
//
#endregion

using System.Collections.Generic;
using System;

namespace Schulte.Silverlight
{
   /// <summary>
   /// A branching point of a plant.
   /// </summary>
   public struct BranchPoint
   {
      public float Time;
      public int Angle;

      public BranchPoint(float time, int angle)
      {
         this.Time = time;
         this.Angle = angle;
      }
   }
}
