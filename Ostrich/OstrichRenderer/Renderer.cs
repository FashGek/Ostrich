using OstrichRenderer.Materials;
using OstrichRenderer.Primitives;
using OstrichRenderer.RenderMath;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using Random = OstrichRenderer.RenderMath.Random;

namespace OstrichRenderer
{
    public class Renderer
    {
        public static int Width, Height, Sample, MaxDepth;
        private static List<Polygon> World = new List<Polygon>();
        private static Material[] Materials;

        private static double[] Buff;
        private static PointBitmap PointBitmap;

        private static long[,] Seeds;

        private static long RayCount;

        public static void Init(int width, int height, int sample, int maxDepth = 4)
        {
            Width = width;
            Height = height;
            Sample = sample;
            MaxDepth = maxDepth;
            Buff = new double[width * height * 4];
            PointBitmap = new PointBitmap(new Bitmap(width, height));
        }

        public static void InitScene()
        {
            World.Add(new Polygon(0, new Vector2(450, 300), new Vector2(300, 450), new Vector2(450, 600),
                new Vector2(600, 450)));

            World.Add(new Polygon(1, new Vector2(300, 100), new Vector2(100, 100), new Vector2(100, 300),
                new Vector2(300, 300)));
            World.Add(new Polygon(2, new Vector2(800, 100), new Vector2(600, 100), new Vector2(600, 300),
                new Vector2(800, 300)));
            World.Add(new Polygon(1, new Vector2(300, 600), new Vector2(100, 600), new Vector2(100, 800),
                new Vector2(300, 800)));
            World.Add(new Polygon(2, new Vector2(800, 600), new Vector2(600, 600), new Vector2(600, 800),
                new Vector2(800, 800)));

            World.Add(new Polygon(3, new Vector2(450, 150), new Vector2(400, 200), new Vector2(450, 250),
                new Vector2(500, 200)));
            World.Add(new Polygon(3, new Vector2(200, 400), new Vector2(150, 450), new Vector2(200, 500),
                new Vector2(250, 450)));
            World.Add(new Polygon(3, new Vector2(450, 650), new Vector2(400, 700), new Vector2(450, 750),
                new Vector2(500, 700)));
            World.Add(new Polygon(3, new Vector2(700, 400), new Vector2(650, 450), new Vector2(700, 500),
                new Vector2(750, 450)));

            Materials = new Material[]
            {
                new Dielectirc(1, 1),
                new Light(new Color32(1,0.5,0), 1, 0.3),
                new Light(new Color32(0,0.5,1), 1, 0.3),
                new Metal(0.7), 
            };
        }

        /*
         
            World.Add(new Polygon(0, new Vector2(150, 100), new Vector2(67, 84), new Vector2(32, 576),
                new Vector2(118, 580)));
            World.Add(new Polygon(0, new Vector2(322, 81), new Vector2(161, 103), new Vector2(169, 176),
                new Vector2(343, 157)));
            World.Add(new Polygon(0, new Vector2(323, 291), new Vector2(155, 300), new Vector2(149, 379),
                new Vector2(320, 375)));

            World.Add(new Polygon(1, new Vector2(516, 129), new Vector2(448, 172), new Vector2(647, 569),
                new Vector2(721, 534)));
            //World.Add(new Polygon(1, new Vector2(746, 153), new Vector2(635, 100), new Vector2(412, 536),
            //    new Vector2(455, 581)));

            World.Add(new Polygon(2, new Vector2(927, 128), new Vector2(862, 118), new Vector2(758, 540),
                new Vector2(826, 569)));

            World.Add(new Polygon(2, new Vector2(993, 112), new Vector2(943, 129), new Vector2(1045, 585),
                new Vector2(1120, 572)));

            World.Add(new Polygon(2, new Vector2(1278, 123), new Vector2(1208, 120), new Vector2(1130, 571),
                new Vector2(1187, 592)));

            World.Add(new Polygon(2, new Vector2(1358, 115), new Vector2(1286, 121), new Vector2(1389, 619),
                new Vector2(1465, 604)));

            Materials = new Material[]
            {
                new Light(new Color32(1, 0.5, 0.5), 1, 0.3), 
                new Dielectirc(0.6, 0.3), 
                new Light(new Color32(0.5, 0.5, 1), 1, 0.3)
            };
          
         */

        /* FXM 1500 * 700
        
            World.Add(new Quadrilateral(new Vector2(150, 100), new Vector2(67, 84), new Vector2(32, 576),
                new Vector2(118, 580), new Light(new Color32(1, 0.5, 0.5), 1, 0.3)));
            World.Add(new Quadrilateral(new Vector2(322, 81), new Vector2(161, 103), new Vector2(169, 176),
                new Vector2(343, 157), new Light(new Color32(1, 0.5, 0.5), 1, 0.3)));
            World.Add(new Quadrilateral(new Vector2(323, 291), new Vector2(155, 300), new Vector2(149, 379),
                new Vector2(320, 375), new Light(new Color32(1, 0.5, 0.5), 1, 0.3)));

            World.Add(new Circle(new Vector2(416, 98), 55, new Light(new Color32(0.75, 0.75, 0.5), 1, 0.3)));

            World.Add(new Quadrilateral(new Vector2(516, 129), new Vector2(448, 172), new Vector2(647, 569),
                          new Vector2(721, 534), new Light(new Color32(0.5, 1, 0.5), 1, 0.3)) + new Quadrilateral(
                          new Vector2(746, 153), new Vector2(635, 100), new Vector2(412, 536),
                          new Vector2(455, 581), new Light(new Color32(0.5, 1, 0.5), 1, 0.3)));

            World.Add(new Circle(new Vector2(738, 628), 55, new Light(new Color32(0.5, 0.75, 0.75), 1, 0.3)));

            World.Add(new Quadrilateral(new Vector2(927, 128), new Vector2(862, 118), new Vector2(758, 540),
                new Vector2(826, 569), new Light(new Color32(0.5, 0.5, 1), 1, 0.3)));

            World.Add(new Circle(new Vector2(931, 73), 49, new Light(new Color32(0.5, 0.5, 1), 1, 0.3)));

            World.Add(new Quadrilateral(new Vector2(993, 112), new Vector2(943, 129), new Vector2(1045, 585),
                new Vector2(1120, 572), new Light(new Color32(0.5, 0.5, 1), 1, 0.3)));

            World.Add(new Circle(new Vector2(1122, 635), 47, new Light(new Color32(0.5, 0.5, 1), 1, 0.3)));

            World.Add(new Quadrilateral(new Vector2(1278, 123), new Vector2(1208, 120), new Vector2(1130, 571),
                new Vector2(1187, 592), new Light(new Color32(0.5, 0.5, 1), 1, 0.3)));

            World.Add(new Circle(new Vector2(1284, 73), 44, new Light(new Color32(0.5, 0.5, 1), 1, 0.3)));

            World.Add(new Quadrilateral(new Vector2(1358, 115), new Vector2(1286, 121), new Vector2(1389, 619),
                new Vector2(1465, 604), new Light(new Color32(0.5, 0.5, 1), 1, 0.3)));

        */

        /*
         
            World.Add(new Circle(new Vector2(512, 512), 450, new Light(new Color32(0.9, 0.67, 0.3), 1, 0.7)) -
                      new Circle(new Vector2(512, 512), 440, new Light(new Color32(1, 0.85, 0.3), 1, 0.7)));

         */

        /*Milo 1024*1024
        
            World.List.Add(new Line(new Vector2(512, 512), new Vector2(0, 512), new Light(Color32.Black, 1, 0.4)) -
                           new Circle(new Vector2(512, 512), 400, new Light(Color32.Black, 1, 0.4)));
            World.List.Add(new Circle(new Vector2(350, 200), 100, new Light(Color32.White, 4, 0.4)));

        */

        /*
         
            World.Add(
                new Quadrilateral(new Vector2(200, 100), new Vector2(100, 100), new Vector2(99, 200),
                    new Vector2(199, 200), new Light(new Color32(1, 0.5, 0.5), 1, 0.3)) +
                new Quadrilateral(new Vector2(300, 100), new Vector2(200, 100), new Vector2(199, 200),
                    new Vector2(299, 200), new Light(new Color32(1, 0.5, 0.5), 1, 0.3)) +
                new Quadrilateral(new Vector2(200, 200), new Vector2(100, 200), new Vector2(99, 300),
                    new Vector2(199, 300), new Light(new Color32(1, 0.5, 0.5), 1, 0.3)));
            World.Add(
                new Quadrilateral(new Vector2(700, 100), new Vector2(600, 100), new Vector2(599, 200),
                    new Vector2(699, 200), new Light(new Color32(0.5, 1, 0.5), 1, 0.3)) +
                new Quadrilateral(new Vector2(800, 100), new Vector2(700, 100), new Vector2(699, 200),
                    new Vector2(799, 200), new Light(new Color32(0.5, 1, 0.5), 1, 0.3)) +
                new Quadrilateral(new Vector2(800, 200), new Vector2(700, 200), new Vector2(699, 300),
                    new Vector2(799, 300), new Light(new Color32(0.5, 1, 0.5), 1, 0.3)));
            World.Add(
                new Quadrilateral(new Vector2(200, 600), new Vector2(100, 600), new Vector2(99, 700),
                    new Vector2(199, 700), new Light(new Color32(0.5, 1, 0.5), 1, 0.3)) +
                new Quadrilateral(new Vector2(300, 700), new Vector2(200, 700), new Vector2(199, 800),
                    new Vector2(299, 800), new Light(new Color32(0.5, 1, 0.5), 1, 0.3)) +
                new Quadrilateral(new Vector2(200, 700), new Vector2(100, 700), new Vector2(99, 800),
                    new Vector2(199, 800), new Light(new Color32(0.5, 1, 0.5), 1, 0.3)));
            World.Add(
                new Quadrilateral(new Vector2(700, 700), new Vector2(600, 700), new Vector2(599, 800),
                    new Vector2(699, 800), new Light(new Color32(1, 0.5, 0.5), 1, 0.3)) +
                new Quadrilateral(new Vector2(800, 600), new Vector2(700, 600), new Vector2(699, 700),
                    new Vector2(799, 700), new Light(new Color32(1, 0.5, 0.5), 1, 0.3)) +
                new Quadrilateral(new Vector2(800, 700), new Vector2(700, 700), new Vector2(699, 800),
                    new Vector2(799, 800), new Light(new Color32(1, 0.5, 0.5), 1, 0.3)));

            World.Add(new Circle(new Vector2(450, 450), 200, new Light(new Color32(0.5, 0.5, 1), 1, 0.3)) -
                      new Circle(new Vector2(250, 450), 100, new Light(new Color32(0.5, 0.5, 1), 1, 0.3)) -
                      new Circle(new Vector2(650, 450), 100, new Light(new Color32(0.5, 0.5, 1), 1, 0.3)) -
                      new Circle(new Vector2(450, 250), 100, new Light(new Color32(0.5, 0.5, 1), 1, 0.3)) -
                      new Circle(new Vector2(450, 650), 100, new Light(new Color32(0.5, 0.5, 1), 1, 0.3)));

        */

        public static void Start()
        {
            RayCount = 0;
            Console.WriteLine();
            InitScene();
#if  DEBUG
            Seeds = new long[Width, Height];
#endif
            new Task(Render).Start();
        }

        private static void Render()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.For(0, Height, y =>
            {
                for (int x = 0; x < Width; x++)
                {
#if DEBUG
                    Seeds[x, y] = Random.Seed;
#endif
                    Sampling(x, y);
                }
            });
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds / 1000d);
            Console.WriteLine((RayCount / 1000000d / (stopwatch.ElapsedMilliseconds / 1000d)).ToString("F1") + "MRays/s");
        }

        private static void Sampling(int x, int y)
        {
            Color32 sum = Color32.Black;
            for (int i = 0; i < Sample; i++)
            {
                double a = Mathd.TwoPi * (i + Random.Get()) / Sample;//抖动采样
                sum += Trace(new LineSeg(new Vector2(x, y),
                    new Vector2(Math.Cos(a), Math.Sin(a)) * 1e7), 0);
            }
            sum = sum / Sample;
            Buff[y * Width * 4 + x * 4] = sum.B;
            Buff[y * Width * 4 + x * 4 + 1] = sum.G;
            Buff[y * Width * 4 + x * 4 + 2] = sum.R;
            Buff[y * Width * 4 + x * 4 + 3] = 1;
        }

        private static Color32 Trace(LineSeg ray, int depth, bool debug = false)
        {
            HitRecord hit = new HitRecord {T = double.MaxValue};
            RayCount++;
            bool isHit = false;
            foreach (Polygon polygon in World)
                if (polygon.Hit(ray, ref hit)) isHit = true;
            if (isHit)
            {
#if DEBUG
                if(debug) DrawLine(ray.P1, hit.P);
#endif
                Color32 attenuation = Color32.Black;
                if (depth >= MaxDepth) return attenuation;
                LineSeg r = new LineSeg(Vector2.Zero, Vector2.Zero); //折射后的射线
                LineSeg l = new LineSeg(Vector2.Zero, Vector2.Zero); //反射后的射线
                if (Materials[hit.Material].Reflect(ray, hit, ref attenuation, ref r)) 
                    attenuation += Materials[hit.Material].Reflectivity * Trace(r, depth + 1, debug);
                if (Materials[hit.Material].Refract(ray, hit, ref l))
                    attenuation += Materials[hit.Material].Refractivity * Trace(l, depth + 1, debug);
                return attenuation;
            }
#if DEBUG
            if (debug)
                DrawLine(ray.P1, ray.P2);
#endif

            return Color32.Black;
        }

        public static void DrawRoute(int x, int y)
        {
#if DEBUG
            Random.Seed = Seeds[x, y];
            for (int i = 0; i < Sample; i++)
            {
                double a = Mathd.TwoPi * (i + Random.Get()) / Sample;
                Trace(new LineSeg(new Vector2(x, y), new Vector2(Math.Cos(a), Math.Sin(a)) * 1e7), 0, true);
            }
#endif
        }

        private static void DrawLine(Vector2 s, Vector2 e)
        {
            int x0 = (int)s.X, y0 = (int)s.Y, x1 = (int)e.X, y1 = (int)e.Y;
            int dx, dy;
            try
            {
                dx = Math.Abs(x1 - x0);
            }
            catch (Exception)
            {
                dx = int.MaxValue;
            }
            try
            {
                dy = Math.Abs(y1 - y0);
            }
            catch (Exception)
            {
                dy = int.MaxValue;
            }
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2;
            for (; x0 != x1 || y0 != y1;)
            {
                int e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
                if (x0 < -Width || x0 >= Width * 2) return;
                if (y0 < -Height || y0 >= Height * 2) return;
                if (x0 < 0 || x0 >= Width) continue;
                if (y0 < 0 || y0 >= Height) continue;
                Buff[y0 * Width * 4 + x0 * 4] = 0;
                Buff[y0 * Width * 4 + x0 * 4 + 1] = 0;
                Buff[y0 * Width * 4 + x0 * 4 + 2] = 255;
                Buff[y0 * Width * 4 + x0 * 4 + 3] = 255;
            }
        }

        public static Bitmap GetBitmap()
        {
            PointBitmap.LockBits();
            for (int i = 0; i < Buff.Length; i++)
                PointBitmap.SetColor(i, (byte) Mathd.Range(Buff[i] * 255 + 0.5, 0, 255));

            return PointBitmap.UnlockBits();
        }
    }
}
