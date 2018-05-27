using OstrichRenderer.Materials;
using OstrichRenderer.Primitives;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;
using System;
using System.Drawing;
using System.Threading.Tasks;
using Random = OstrichRenderer.RenderMath.Random;

namespace OstrichRenderer
{
    public class Renderer
    {
        public static int Width, Height, Sample, MaxDepth;
        private static HitableList World = new HitableList();

        private static double[] Buff;
        private static PointBitmap PointBitmap;

        private static long[,] Seeds;
        private const bool Debug = true;

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
            //World.List.Add(new Circle(new Vector2(256, 256), 70, new Light(new Color32(1, 0.5, 0), 1)));
            //World.List.Add(new Circle(new Vector2(150, 150), 50, new Light(new Color32(1, 1, 0), 1)));
            //World.List.Add(new Circle(new Vector2(350, 150), 50, new Light(new Color32(0.5, 1, 0), 1)));
            //World.List.Add(new Line(new Vector2(0, 0), new Vector2(512, 512), new Light(new Color32(1, 1, 1), 1)));
            //World.List.Add(new Line(new Vector2(0, 512), new Vector2(512, 0), new Light(new Color32(1, 1, 1), 1)));
            //World.List.Add(new Intersect(
            //    new Line(new Vector2(0, 0), new Vector2(512, 512), new Light(new Color32(0.1, 0.5, 0.8), 1)),
            //    new Line(new Vector2(0, 512), new Vector2(512, 0), new Light(new Color32(0.1, 0.5, 0.8), 1)),
            //    new Line(new Vector2(512, 100), new Vector2(0, 100), new Light(new Color32(0.1, 0.5, 0.8), 1))));
            //World.List.Add(new Intersect(
            //    new Line(new Vector2(512, 256), new Vector2(0, 256), new Light(new Color32(0.2, 0.5, 0.8), 1)),
            //    new Circle(new Vector2(256, 256), 100, new Light(new Color32(0.2, 0.5, 0.8), 1))));
            //World.List.Add(new Circle(new Vector2(256, 256), 100, new Light(new Color32(1, 1, 1), 1)) -
            //               new Circle(new Vector2(256, 260), 100, new Light(new Color32(1, 1, 1), 1)));
            World.List.Add(new Line(new Vector2(512, 512), new Vector2(0, 512), new Light(Color32.Black, 1)) -
                           new Circle(new Vector2(512, 512), 400, new Light(Color32.Black, 1)));
            World.List.Add(new Circle(new Vector2(350, 200), 100, new Light(Color32.White, 1.5)));
        }

        public static void Start()
        {
            InitScene();
            new Task(Render).Start();
            if (Debug)
                Seeds = new long[Width, Height];
        }

        private static void Render()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (x == 586 && y == 919)
                    {
                        
                    }
                    if (Debug)
                        Seeds[x, y] = Random.Seed;
                    Sampling(x, y);
                }
            }
        }
            
        private static void Sampling(int x, int y)
        {
            Color32 sum = Color32.Black;
            for (int i = 0; i < Sample; i++)
            {
                if (x == 66 && y == 26)
                {
                    
                }
                if (x == 48 && y == 59)
                {
                    sum += Trace(new Ray(new Vector2(x, y),
                        new Vector2(Math.Cos(Mathd.Pi / 3 ), Math.Sin(Mathd.Pi / 3 ))), 0);
                }
                else
                {
                    double a = Mathd.TwoPi * (i + Random.Get()) / Sample;//抖动采样
                    sum += Trace(new Ray(new Vector2(x, y),
                        new Vector2(Math.Cos(a), Math.Sin(a))), 0);
                }
            }
            sum = sum / Sample;
            Buff[y * Width * 4 + x * 4] = sum.B;
            Buff[y * Width * 4 + x * 4 + 1] = sum.G;
            Buff[y * Width * 4 + x * 4 + 2] = sum.R;
            Buff[y * Width * 4 + x * 4 + 3] = sum.A;
        }

        private static Color32 Trace(Ray ray, int depth, bool debug = false)
        {
            HitRecord hit = new HitRecord();
            if (World.Hit(ray, 0, double.MaxValue, ref hit))
            {
                if (Debug && debug)
                    DrawLine(ray.Origin, hit.P);

                Color32 attenuation = Color32.Black;
                Ray r = new Ray(Vector2.Zero, Vector2.Zero);

                if (depth < MaxDepth && hit.Material.Scatter(ray, hit, ref attenuation, ref r))
                    return attenuation + Trace(r, depth + 1, debug);
                return attenuation;
                return attenuation;
            }
            if (Debug && debug)
                DrawLine(ray.Origin, ray.Origin + ray.NormalDirection * Width);

            return Color32.Black;
        }

        public static void DrawRoute(int x, int y)
        {
            if (!Debug) return;
            Random.Seed = Seeds[x, y];
            for (int i = 0; i < Sample; i++)
            {
                double a = Mathd.TwoPi * (i + Random.Get()) / Sample;
                Trace(new Ray(new Vector2(x, y), new Vector2(Math.Cos(a), Math.Sin(a))), 0, true);
            }
        }

        private static void DrawLine(Vector2 s, Vector2 e)
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
                if (y0 < -Width || y0 >= Width * 2) return;
                if (x0 < 0 || x0 >= Width) continue;
                if (y0 < 0 || y0 >= Width) continue;
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
