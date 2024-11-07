#region Header
//
//   Project:           WriteableBitmapEx - Silverlight WriteableBitmap extensions
//   Description:       Blit Sample for the WriteableBitmap extension methods.
//
//   Changed by:        $Author: unknown $
//   Changed on:        $Date: 2015-02-24 20:36:41 +0100 (Di, 24 Feb 2015) $
//   Changed in:        $Revision: 112951 $
//   Project:           $URL: https://writeablebitmapex.svn.codeplex.com/svn/trunk/Source/WriteableBitmapExBlitSample/MainPage.xaml.cs $
//   Id:                $Id: MainPage.xaml.cs 112951 2015-02-24 19:36:41Z unknown $
//
//
//   Copyright © 2009-2015 Bill Reiss, Rene Schulte and WriteableBitmapEx Contributors
//
//   This code is open source. Please read the License.txt for details. No worries, we won't sue you! ;)
//
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO;

namespace WriteableBitmapExBlitSample
{
   public partial class MainPage : UserControl
   {
      #region Fields

      WriteableBitmap bmp;
      WriteableBitmap circleBmp;
      WriteableBitmap particleBmp;
      Rect particleSourceRect;
      ParticleEmitter emitter = new ParticleEmitter();
      DateTime lastUpdate = DateTime.Now;

      #endregion

      #region Contructors

      public MainPage()
      {
         InitializeComponent();
         particleBmp = LoadBitmap("/WriteableBitmapExBlitSample;component/Data/FlowerBurst.jpg");
         circleBmp = LoadBitmap("/WriteableBitmapExBlitSample;component/Data/circle.png");
         particleSourceRect = new Rect(0, 0, 64, 64);
         bmp = new WriteableBitmap(640, 480);
         bmp.Clear(Colors.Black);
         image.Source = bmp;
         emitter = new ParticleEmitter();
         emitter.TargetBitmap = bmp;
         emitter.ParticleBitmap = particleBmp;
         CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
         this.MouseMove += new MouseEventHandler(MainPage_MouseMove);
      }

      #endregion

      #region Methods

      WriteableBitmap LoadBitmap(string path)
      {
         BitmapImage img = new BitmapImage();
         img.CreateOptions = BitmapCreateOptions.None;
         Stream s = Application.GetResourceStream(new Uri(path, UriKind.Relative)).Stream;
         img.SetSource(s);
         return new WriteableBitmap(img);
      }

      #endregion

      #region Eventhandler

      void MainPage_MouseMove(object sender, MouseEventArgs e)
      {
         emitter.Center = e.GetPosition(image);
      }

      void CompositionTarget_Rendering(object sender, EventArgs e)
      {
         bmp.Clear(Colors.Black);

         double elapsed = (DateTime.Now - lastUpdate).TotalSeconds;
         lastUpdate = DateTime.Now;
         emitter.Update(elapsed);
         //			bmp.Blit(new Point(100, 150), circleBmp, new Rect(0, 0, 200, 200), Colors.Red, BlendMode.Additive);
         //			bmp.Blit(new Point(160, 55), circleBmp, new Rect(0, 0, 200, 200), Color.FromArgb(255, 0, 255, 0), BlendMode.Additive);
         //			bmp.Blit(new Point(220, 150), circleBmp, new Rect(0, 0, 200, 200), Colors.Blue, BlendMode.Additive);

         bmp.Invalidate();
      }

      #endregion
   }
}
