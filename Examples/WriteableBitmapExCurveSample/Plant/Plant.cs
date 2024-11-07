#region Header
//
//   Project:           Silverlight procedural Plant
//
//   Changed by:        $Author: unknown $
//   Changed on:        $Date: 2015-02-24 20:36:41 +0100 (Di, 24 Feb 2015) $
//   Changed in:        $Revision: 112951 $
//   Project:           $URL: https://writeablebitmapex.svn.codeplex.com/svn/trunk/Source/WriteableBitmapExCurveSample/Plant/Plant.cs $
//   Id:                $Id: Plant.cs 112951 2015-02-24 19:36:41Z unknown $
//
//
//   Copyright © 2010-2015 Rene Schulte and WriteableBitmapEx Contributors
//
//   This code is open source. Please read the License.txt for details. No worries, we won't sue you! ;)
//
#endregion

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace Schulte.Silverlight
{
   /// <summary>
   /// A simple plant.
   /// </summary>
   public class Plant
   {
      private Random rand;
      private Dictionary<int, int> branchesPerGen;

      public Branch Root               { get; private set; }
      public float Tension             { get; set; }
      public int MaxWidth              { get; private set; }
      public int MaxHeight             { get; private set; }
      public int BranchLenMin          { get; set; }
      public int BranchLenMax          { get; set; }
      public int BranchAngleVariance   { get; set; }
      public float GrowthRate          { get; set; }
      public int MaxGenerations        { get; set; }
      public Color Color               { get; set; }
      public Vector Start              { get; private set; }
      public Vector Scale              { get; private set; }
      public List<BranchPoint> BranchPoints { get; private set; }
      //public int BranchDegression       { get; set; }
      public float BendingFactor       { get; set; }
      public int MaxBranchesPerGeneration { get; set; }

      public Plant()
      {
         this.Tension      = 1f;
         this.rand         = new Random();
         this.BranchLenMin = 150;
         this.BranchLenMax = 200;
         this.GrowthRate   = 0.007f;
         this.BranchPoints = new List<BranchPoint>();
         this.BranchAngleVariance = 10;
         this.MaxGenerations      = int.MaxValue;
         //this.BranchDegression    = 0;
         this.Color               = Color.FromArgb(255, 100, 150, 0);
         this.Start               = Vector.Zero;
         this.Scale               = Vector.One;
         this.BendingFactor       = 0.4f;
         this.MaxBranchesPerGeneration = int.MaxValue;
         this.branchesPerGen           = new Dictionary<int, int>();
      }

      public Plant(Vector start, Vector scale, int viewPortWidth, int viewPortHeight)
         : this()
      {
         this.Tension = 0.5f;
         this.Initialize(start, scale, viewPortWidth, viewPortHeight);
      }

      public void Initialize(Vector start, Vector scale, int viewPortWidth, int viewPortHeight)
      {
         this.Start = start;
         this.Scale = scale;
         this.MaxWidth = viewPortWidth;
         this.MaxHeight = viewPortHeight;
         var end = new Vector(Start.X, Start.Y + ((MaxHeight >> 4) * Scale.Y));
         this.Root = new Branch(Start, Start, end, 0.02f);
      }

      public void Clear()
      {
         branchesPerGen.Clear();
         this.Root.Clear();
      }

      public void Grow()
      {
         Grow(this.Root, 0);
      }

      private void Grow(Branch branch, int generation)
      {
         if (generation <= MaxGenerations)
         {
            if (branch.End.Y >= 0 && branch.End.Y <= MaxHeight
             && branch.End.X >= 0 && branch.End.X <= MaxWidth)
            {
               // Grow it
               branch.Grow();

               // Branch?
               foreach (var bp in BranchPoints)
               {
                  if (!branchesPerGen.ContainsKey(generation))
                  {
                     branchesPerGen.Add(generation, 0);
                  }
                  if (branchesPerGen[generation] < MaxBranchesPerGeneration)
                  {
                     if (branch.Life >= bp.Time && branch.Life <= bp.Time + branch.GrowthRate)
                     {
                        // Length and angle of the branch
                        var branchLen = rand.Next(BranchLenMin, BranchLenMax);
                        branchLen -= (int)(branchLen * 0.01f * generation);
                        // In radians
                        var angle = rand.Next(bp.Angle - BranchAngleVariance, bp.Angle + BranchAngleVariance) * 0.017453292519943295769236907684886;

                        // Desired end of new branch
                        var endTarget = new Vector(branch.End.X + ((int)(Math.Sin(angle) * branchLen) * Scale.X),
                                                   branch.End.Y + ((int)(Math.Cos(angle) * branchLen) * Scale.Y));

                        // Desired middle point
                        angle -= Math.Sign(bp.Angle) * BendingFactor;
                        var middleTarget = new Vector(endTarget.X - ((int)(Math.Sin(angle) * (branchLen >> 1)) * Scale.X),
                                                      endTarget.Y - ((int)(Math.Cos(angle) * (branchLen >> 1)) * Scale.Y));

                        // Add new branch
                        branch.Branches.Add(new Branch(branch.End, middleTarget, endTarget, GetRandomGrowthRate()));
                        branchesPerGen[generation]++;
                     }
                  }
               }
            }

            // Grow the child branches
            foreach (var b in branch.Branches)
            {
               Grow(b, generation+1);
            }
         }
      }

      private float GetRandomGrowthRate()
      {
          var r = (float)rand.NextDouble() * GrowthRate - GrowthRate * 0.5f;
          return GrowthRate + r;
      }

      public void Draw(WriteableBitmap writeableBmp)
      {
         if (writeableBmp != null)
         {
            // Wrap updates in a GetContext call, to prevent invalidation and nested locking/unlocking during this block
            using (writeableBmp.GetBitmapContext())
            {
               writeableBmp.Clear();
               Draw(writeableBmp, this.Root);
#if SILVERLIGHT
               writeableBmp.Invalidate();
#endif
            }
         }
      }

      private void Draw(WriteableBitmap writeableBmp, Branch branch)
      {
         int[] pts = new int[] 
         { 
            branch.Start.X,   branch.Start.Y,
            branch.Middle.X,  branch.Middle.Y,
            branch.End.X,     branch.End.Y,
         };

         // Draw with cardinal spline
         writeableBmp.DrawCurve(pts, Tension, this.Color);

         foreach (var b in branch.Branches)
         {
            Draw(writeableBmp, b);
         }
      }
   }
}
