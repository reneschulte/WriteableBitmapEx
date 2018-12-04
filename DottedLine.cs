namespace System.Windows.Media.Imaging {
    /// <summary>
    /// Class for drawing a dotted line
    /// </summary>
    public static class DottedLine {
        private static int ConvertColor(Color color) {
            
            var col = 0;

            if (color.A != 0) {
                var a = color.A + 1;
                col = (color.A << 24)
                  | ((byte)((color.R * a) >> 8) << 16)
                  | ((byte)((color.G * a) >> 8) << 8)
                  | ((byte)((color.B * a) >> 8));
            }

            return col;
        }
    
        public static void DrawLineDotted(this WriteableBitmap bmp, int x1, int y1, int x2, int y2, int dotSpace, int dotLength, Color color) {
        var c = ConvertColor(color);
            DrawLineDotted(bmp, x1, y1, x2, y2, dotSpace, dotLength, c);
        }
        /// <summary>
        /// Draws a dotted line
        /// </summary>
        /// <param name="bmp">The WriteableBitmap</param>
        /// <param name="x1">Startpoint.X</param>
        /// <param name="y1">Startpoint.Y</param>
        /// <param name="x2">Endpoint.X</param>
        /// <param name="y2">Endpoint.Y</param>
        /// <param name="dotSpace">length of space between each line segment</param>
        /// <param name="dotLength">length of each line segment</param>
        /// <param name="color"></param>
        public static void DrawLineDotted(this WriteableBitmap bmp, int x1, int y1, int x2, int y2, int dotSpace, int dotLength, int color) {
            // vertically?
            if (x1 == x2) {
                SwapHorV(ref y1, ref y2);
                DrawVertically(bmp, x1, y1, y2, dotSpace, dotLength, color);
            }
            // horizontally?
            else if (y1 == y2) {
                SwapHorV(ref x1, ref x2);
                DrawHorizontally(bmp, x1, x2, y1, dotSpace, dotLength, color);
            } else {
                Draw(bmp, x1, y1, x2, y2, dotSpace, dotLength, color);
            }
        }

        private static void DrawVertically(WriteableBitmap bmp, int x, int y1, int y2, int dotSpace, int dotLength, int color) {            
            bool on = true;
            int spaceCnt = 0;
            for(int i=y1; i <= y2; i++) {
                if (i < 1) {
                    continue;
                }
                if (i >= bmp.PixelHeight) {
                    break;
                }  
                if(x >= bmp.PixelWidth)
                {
                    break;
                }

                if (on) {                    
                    bmp.SetPixel(x, i, color);                      
                    on = i % dotLength != 0;
                    spaceCnt = 0;
                } else {
                    spaceCnt++;
                    on = spaceCnt % dotSpace == 0;
                }
                //System.Diagnostics.Debug.WriteLine($"{x},{i}, on = {on}");
            }
        }

        private static void DrawHorizontally(WriteableBitmap bmp, int x1, int x2, int y, int dotSpace, int dotLength, int color) {
            bool on = true;
            int spaceCnt = 0;
            for (int i = x1; i <= x2; i++) {
                if (i < 1) {
                    continue;
                }
                if (i >= bmp.PixelWidth) {
                    break;
                }
                if (y >= bmp.PixelHeight) {
                    break;
                }

                if (on) {
                    bmp.SetPixel(i, y, color);
                    on = i % dotLength != 0;
                    spaceCnt = 0;
                } else {
                    spaceCnt++;
                    on = spaceCnt % dotSpace == 0;
                }
                //System.Diagnostics.Debug.WriteLine($"{i},{y}, on = {on}");
            }
        }

        private static void Draw(WriteableBitmap bmp, int x1, int y1, int x2, int y2,int dotSpace, int dotLength, int color) {
            // y = m * x + n
            // y - m * x = n
            Swap(ref x1, ref x2, ref y1, ref y2);
            double m = (y2 - y1) / (double)(x2 - x1);            
            double n = y1 - m * x1;

            bool on = true;
            int spaceCnt = 0;
            for(int i = x1; i <= bmp.PixelWidth; i++) {
                if (i == 0) {
                    continue;
                }
                int y = (int)Math.Round(m * i + n,0, MidpointRounding.ToEven);
                if(y <= 0) {
                    continue;
                }
                if (y >= bmp.PixelHeight || i >= x2) {
                    break;
                }
                if (on) {
                    bmp.SetPixel(i, y, color);
                    spaceCnt = 0;
                    on = i % dotLength != 0;
                } else {
                    spaceCnt++;
                    on = spaceCnt % dotSpace == 0;
                }
            }
        }

        private static void Swap(ref int x1,ref int x2, ref int y1, ref int y2) {
            // always draw from left to right
            // or from top to bottom
            if(x2 < x1) {
                int tmpx1 = x1;
                int tmpx2 = x2;
                int tmpy1 = y1;
                int tmpy2 = y2;
                x1 = tmpx2;
                y1 = tmpy2;
                x2 = tmpx1;
                y2 = tmpy1;
            }
        }

        private static void SwapHorV(ref int a1, ref int a2) {
            int x1 = 0; // dummy
            int x2 = -1; // dummy
            if (a2 < a1) {
                Swap(ref x1, ref x2, ref a1, ref a2);
            }
        }        
    }
}
