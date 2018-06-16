using OstrichRenderer.Materials;
using OstrichRenderer.Primitives;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using Point = OstrichRenderer.Rendering.Point;

namespace OstrichRenderer
{
    public class Renderer
    {
        public static int Width, Height;
        private static HitableList World = new HitableList();
        private static Material[] Materials;
        private static LineSeg[] LineSegs;

        private static double[] Buff;
        private static PointBitmap PointBitmap;

        public static void Init(int width, int height)
        {
            Width = width;
            Height = height;
            Buff = new double[width * height * 4];
            PointBitmap = new PointBitmap(new Bitmap(width, height));
        }

        public static void InitScene()
        {
            //World.Add(
            //    new Quadrilateral(new Vector2(200, 100), new Vector2(100, 100), new Vector2(99, 200),
            //        new Vector2(199, 200), 1) +
            //    new Quadrilateral(new Vector2(300, 100), new Vector2(200, 100), new Vector2(199, 200),
            //        new Vector2(299, 200), 1) +
            //    new Quadrilateral(new Vector2(200, 200), new Vector2(100, 200), new Vector2(99, 300),
            //        new Vector2(199, 300), 1));


            //World.Add(
            //    new Quadrilateral(new Vector2(300, 100), new Vector2(100, 100), new Vector2(99, 300),
            //        new Vector2(299, 300), 0) -
            //    new Quadrilateral(new Vector2(400, 200), new Vector2(200, 200), new Vector2(199, 400),
            //        new Vector2(399, 400), 0));

            World.Add(new Quadrilateral(new Vector2(150, 100), new Vector2(67, 84), new Vector2(32, 576),
                new Vector2(118, 580), 0));
            World.Add(new Quadrilateral(new Vector2(322, 81), new Vector2(161, 103), new Vector2(169, 176),
                new Vector2(343, 157), 0));
            World.Add(new Quadrilateral(new Vector2(323, 291), new Vector2(155, 300), new Vector2(149, 379),
                new Vector2(320, 375), 0));

            World.Add(new Circle(new Vector2(416, 98), 55, 1));

            World.Add(
                new Quadrilateral(new Vector2(516, 129), new Vector2(448, 172), new Vector2(647, 569),
                    new Vector2(721, 534), 2) + new Quadrilateral(new Vector2(746, 153), new Vector2(635, 100),
                    new Vector2(412, 536), new Vector2(455, 581), 2));

            World.Add(new Circle(new Vector2(738, 628), 55, 3));

            World.Add(new Quadrilateral(new Vector2(927, 128), new Vector2(862, 118), new Vector2(758, 540),
                new Vector2(826, 569), 4));

            World.Add(new Circle(new Vector2(931, 73), 49, 4));

            World.Add(new Quadrilateral(new Vector2(993, 112), new Vector2(943, 129), new Vector2(1045, 585),
                new Vector2(1120, 572), 4));

            World.Add(new Circle(new Vector2(1122, 635), 47, 4));

            World.Add(new Quadrilateral(new Vector2(1278, 123), new Vector2(1208, 120), new Vector2(1130, 571),
                new Vector2(1187, 592), 4));

            World.Add(new Circle(new Vector2(1284, 73), 44, 4));

            World.Add(new Quadrilateral(new Vector2(1358, 115), new Vector2(1286, 121), new Vector2(1389, 619),
                new Vector2(1465, 604), 4));

            Materials = new Material[]
            {
                new Light(new Color32(1, 0.5, 0.5), 1, 0.3),
                new Light(new Color32(0.75, 0.75, 0.5), 1, 0.3),
                new Light(new Color32(0.5, 1, 0.5), 1, 0.3),
                new Light(new Color32(0.5, 0.75, 0.75), 1, 0.3),
                new Light(new Color32(0.5, 0.5, 1), 1, 0.3)
            };
        }

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
            //RayCount = 0;
            Console.WriteLine();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            InitScene();
            PrepareScene();
            Console.WriteLine("场景加载耗时: " + stopwatch.ElapsedMilliseconds + "ms");

            foreach (LineSeg lineSeg in LineSegs)
            {
                DrawLine(lineSeg.P1, lineSeg.P2, new Color32(1, 0, 0));
            }
            return;

            //#if  DEBUG
            //            Seeds = new long[Width, Height];
            //#endif
            new Task(Render).Start();
        }

        private static void Render()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int y = 0; y < Height; y++)
            {
                //多线程加速
                //Parallel.For(0, Height, y =>
                //{
                    for (int x = 0; x < Width; x++)
                    {
                        if (x == 219 && y == 210)
                        {

                        }
//#if DEBUG
//                        Seeds[x, y] = Random.Seed;
//#endif
                        Sampling(x, y);
                    }
                //});
        }
        stopwatch.Stop();
            Console.WriteLine("渲染耗时: " + stopwatch.ElapsedMilliseconds / 1000f + "s");
        }

        private static void Sampling(int x, int y)
        {
            Vector2 Origin = new Vector2(x, y);
            Color32 color = new Color32();
            if (World.IsInside(Origin, out Point p))
            {
                color = Materials[p.Materail].GetColor();
                Buff[y * Width * 4 + x * 4] = color.B;
                Buff[y * Width * 4 + x * 4 + 1] = color.G;
                Buff[y * Width * 4 + x * 4 + 2] = color.R;
                Buff[y * Width * 4 + x * 4 + 3] = 1;
                return;
            }
            List<Point>[] points = new List<Point>[LineSegs.Length];
            for (int i = 0; i < points.Length; i++)
                points[i] = new List<Point>();
            foreach (LineSeg lineSeg in LineSegs)
            {
                double l = lineSeg.Normal * (Origin - lineSeg.Position);
                bool isOnline = l == 0;
                if (l < 0) continue;
                HitLineSeg(new Ray(Origin, lineSeg.P1 - Origin), lineSeg.ID, out Point p1,
                    out Point p2, out bool b1, out bool b2);
                if (!isOnline)
                {
                    HitLineSeg(new Ray(Origin, lineSeg.P2 - Origin), lineSeg.ID, out Point p3,
                        out Point p4, out bool b3, out bool b4);
                        if (b3) points[p3.Line].Add(p3);
                        if (b4) points[p4.Line].Add(p4);
                }
                if (b1) points[p1.Line].Add(p1);
                if (b2) points[p2.Line].Add(p2);
            }

            foreach (List<Point> point in points)
            {
                //if (point.Count % 2 == 1)
                //{ continue;}
                if (point.Count < 2) continue;
                if (point.Count == 2)
                {
                    if (point[0].Position == point[1].Position) continue;
                    color += Math.Abs(Vector2.Angle(point[0].Position - Origin, point[1].Position - Origin)) /
                             Mathd.TwoPi * Materials[point[0].Materail].GetColor();
                    continue;
                }
                Point lastPoint = point[0];
                for (int i = 0; i < point.Count; i++)
                {
                    Point p1 = point[i];
                    if (p1.Position == lastPoint.Position && i != 0)
                    {
                        point.RemoveAt(i--);
                        continue;
                    }
                    lastPoint = p1;
                    p1.Distance = (LineSegs[p1.Line].P1 - p1.Position).Magnitude();
                    point[i] = p1;
                }
                if (point.Count % 2 == 1)
                { continue; }
                point.Sort();
                for (int i = 0; i < point.Count; i += 2)
                    color += Math.Abs(Vector2.Angle(point[i].Position - Origin, point[i + 1].Position - Origin)) /
                             Mathd.TwoPi * Materials[point[i].Materail].GetColor();
            }

            Buff[y * Width * 4 + x * 4] = color.B;
            Buff[y * Width * 4 + x * 4 + 1] = color.G;
            Buff[y * Width * 4 + x * 4 + 2] = color.R;
            Buff[y * Width * 4 + x * 4 + 3] = 1;

            #region ffff

            //foreach (List<Point> point in points)
            //{
            //    if (point.Count % 2 == 1) continue;
            //    if (point.Count < 2) continue;
            //    if (point.Count == 2)
            //    {
            //        if (point[0].Position == point[1].Position) continue;
            //        DrawLine(point[0].Position, point[01].Position, new Color32(0, 0, 1));
            //        //color += Math.Abs(Vector2.Angle(point[0].Position - Origin, point[1].Position - Origin)) /
            //        //         Mathd.TwoPi * Materials[point[0].Materail].GetColor();
            //        continue;
            //    }
            //    for (int i = 0; i < point.Count; i++)
            //    {
            //        Point poo = point[i];
            //        poo.Distance = (LineSegs[poo.Line].P1 - poo.Position).Magnitude();
            //        point[i] = poo;
            //    }
            //    //foreach (Point point1 in point)
            //    //    point1.SetDis((LineSegs[point1.Line].P1 - point1.Position).Magnitude());
            //    point.Sort();
            //    for (int i = 0; i < point.Count; i += 2)
            //        DrawLine(point[i].Position, point[i + 1].Position, new Color32(0, 0, 1));
            //    //color += Math.Abs(Vector2.Angle(point[i].Position - Origin, point[i + 1].Position - Origin)) /
            //    //         Mathd.TwoPi * Materials[point[i].Materail].GetColor();
            //}
            //if (lineSeg.Normal * (new Vector2(x, y) - lineSeg.Position) < 0)
            //{
            //    DrawLine(lineSeg.P1, lineSeg.P2, new Color32(0, 0, 1));
            //}
            //else
            //{
            //    DrawLine(lineSeg.P1, lineSeg.P2, new Color32(0, 1, 0));
            //}
            //}

            #endregion

            foreach (List<Point> point in points)
            {
                if (point.Count % 2 == 1 && point.Count != 1)
                {
                    Console.WriteLine(new Vector2(x, y));
                    foreach (Point point1 in point)
                    {
                        DrawLine(LineSegs[point1.Line].P1, LineSegs[point1.Line].P2, new Color32(0, 0, 1));
                    }
                    foreach (Point point1 in point)
                    {
                        DrawLine(Origin, point1.Position, new Color32(0, 0, 1));
                    }
                    continue;
                }
                foreach (Point point1 in point)
                {
                    DrawLine(Origin, point1.Position, new Color32(0, 1, 0));
                }
            }

            #region useless

            //Color32 sum = Color32.Black;
            //for (int i = 0; i < Sample; i++)
            //{
            //    if (x == 255 && y == 0)
            //    {

            //    }
            //    double a = Mathd.TwoPi * (i + Random.Get()) / Sample;//抖动采样
            //    sum += Trace(new Ray(new Vector2(x, y),
            //        new Vector2(Math.Cos(a), Math.Sin(a))), 0);
            //}
            //sum = sum / Sample;
            //Buff[y * Width * 4 + x * 4] = sum.B;
            //Buff[y * Width * 4 + x * 4 + 1] = sum.G;
            //Buff[y * Width * 4 + x * 4 + 2] = sum.R;
            //Buff[y * Width * 4 + x * 4 + 3] = sum.A;

            #endregion
        }

        //        private static Color32 Trace(Ray ray, int depth, bool debug = false)
        //        {
        //            HitRecord hit = new HitRecord();
        //            RayCount++;
        //            if (!World.Hit(ray, 0, double.MaxValue, ref hit)) return Color32.Black;
        //#if DEBUG
        //            if (debug) DrawLine(ray.Origin, hit.P, new Color32(1, 0, 0));
        //#endif
        //            Color32 attenuation = Color32.Black;
        //            Ray r = new Ray(Vector2.Zero, Vector2.Zero);

        //            if (depth < MaxDepth && Materials[hit.Material].Scatter(ray, hit, ref attenuation, ref r))
        //                //return attenuation + hit.Material.Reflectivity * Trace(r, depth + 1, debug);
        //                return attenuation;
        //            return attenuation;
        //#if DEBUG
        //            if (debug)
        //                DrawLine(ray.Origin, ray.Origin + ray.NormalDirection * Width, new Color32(1, 0, 0));
        //#endif
        //        }

        /// <summary>
        /// 如果端点在前，返回true，否则是false
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="p1">端点</param>
        /// <param name="p2">交点</param>
        private static void HitLineSeg(Ray ray, uint line, out Point p1, out Point p2, out bool b1, out bool b2)
        {
            LineSeg lineSeg = new LineSeg(ray.Origin, ray.NormalDirection * (double.MaxValue / 10000) + ray.Origin,
                LineSegs[line].Normal, 0);
            double p1l = ray.Direction.Magnitude();
            double p2l = double.MaxValue;
            p1 = new Point(LineSegs[line].Material, line, ray.Direction + ray.Origin);
            p2 = new Point();
            b1 = true;
            b2 = false;
            for (uint i = 0; i < LineSegs.Length; i++)
            {
                if (line == i) continue;
                if (LineSeg.IsIntersect(lineSeg, LineSegs[i], out Vector2 vector2))
                {
                    double l = (ray.Origin - vector2).Magnitude();
                    if (vector2 == p1.Position) continue;
                    if (l < p1l && b1)
                    {
                        p1 = new Point();
                        b1 = false;
                    }
                    if (l < p2l)
                    {
                        p2l = l;
                        if (l < p1l || LineSegs[i].Normal * (ray.Origin - LineSegs[i].Position) < 0)
                        {
                            b2 = false;
                            continue;
                        }
                        p2 = new Point(LineSegs[i].Material, i, vector2);
                        b2 = true;
                    }
                }
            }
            if (b2 && (p2.Position == LineSegs[p2.Line].P1 || p2.Position == LineSegs[p2.Line].P2)) b2 = false;
        }

        private static void PrepareScene()
        {
            List<LineSeg> lineSegs = new List<LineSeg>();
            foreach (Hitable hitable in World.List)
                lineSegs.AddRange(hitable.GetLineSegs());
            LineSegs = lineSegs.ToArray();
            for (uint i = 0; i < lineSegs.Count; i++)
                LineSegs[i].ID = i;
            GC.Collect();
        }

        #region Debug

        public static void DrawRoute(int x, int y)
        {
#if DEBUG
            Sampling(x, y);
            //Random.Seed = Seeds[x, y];
            //for (int i = 0; i < Sample; i++)
            //{
            //    double a = Mathd.TwoPi * (i + Random.Get()) / Sample;
            //    Trace(new Ray(new Vector2(x, y), new Vector2(Math.Cos(a), Math.Sin(a))), 0, true);
            //}
#endif
        }

        private static void DrawLine(Vector2 s, Vector2 e, Color32 color)
        {
            int x0 = (int)s.X, y0 = (int)s.Y, x1 = (int)e.X, y1 = (int)e.Y;
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
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
                Buff[y0 * Width * 4 + x0 * 4] = color.B;
                Buff[y0 * Width * 4 + x0 * 4 + 1] = color.G;
                Buff[y0 * Width * 4 + x0 * 4 + 2] = color.R;
                Buff[y0 * Width * 4 + x0 * 4 + 3] = color.A;
            }
        }

        #endregion

        public static Bitmap GetBitmap()
        {
            if (LineSegs != null)
            {
                DrawRoute(1010, 5);
                //DrawLine(LineSegs[tag].P1, LineSegs[tag].P2, new Color32(1, 0, 0));
                //tag++;
                //if (tag == LineSegs.Length) tag--;
            }
            PointBitmap.LockBits();
            for (int i = 0; i < Buff.Length; i++)
                PointBitmap.SetColor(i, (byte) Mathd.Range(Buff[i] * 255 + 0.5, 0, 255));

            return PointBitmap.UnlockBits();
        }
    }
}
