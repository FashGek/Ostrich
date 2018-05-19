using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using OstrichRenderer.Materials;
using OstrichRenderer.RenderMath;
using OstrichRenderer.Primitives;
using OstrichRenderer.Rendering;
using Random = OstrichRenderer.RenderMath.Random;

namespace OstrichRenderer
{
    public class Renderer
    {
        public static int Width, Height, Sample, MaxDepth;
        private static HitableList World = new HitableList();

        private static double[] Buff;
        private static int[] ChangeTimes;
        private static PointBitmap pointBitmap;

        public static void Init(int width, int height, int sample, int maxDepth = 4)
        {
            Width = width;
            Height = height;
            Sample = sample;
            MaxDepth = maxDepth;
            Buff = new double[width * height * 4];
            pointBitmap = new PointBitmap(new Bitmap(width, height));
        }

        public static void InitScene()
        {
            World.List.Add(new Circle(new Vector2(250, 300), 70, new Light(new Color32(1, 0.5, 0), 1)));
            World.List.Add(new Circle(new Vector2(150, 150), 50, new Light(new Color32(1, 1, 0), 1)));
            World.List.Add(new Circle(new Vector2(350, 150), 50, new Light(new Color32(0.5, 1, 0), 1)));
        }

        public static void Start()
        {
            InitScene();
            new Task(Render).Start();
        }

        private static void Render()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Sampling(x, y);
                }
            }
        }
            
        private static void Sampling(int x, int y)
        {
            Color32 sum = Color32.Black;
            for (int i = 0; i < Sample; i++)
            {
                double a = Mathd.TwoPi * (i + Random.Get()) / Sample;
                sum += Trace(new Ray(new Vector2(x, y),
                    new Vector2(Math.Cos(a), Math.Sin(a))), 0);
            }
            sum = sum / Sample;
            Buff[y * Width * 4 + x * 4] = sum.B;
            Buff[y * Width * 4 + x * 4 + 1] = sum.G;
            Buff[y * Width * 4 + x * 4 + 2] = sum.R;
            Buff[y * Width * 4 + x * 4 + 3] = sum.A;
        }

        private static Color32 Trace(Ray ray, int depth)
        {
            HitRecord hit = new HitRecord();
            if (World.Hit(ray, 0, double.MaxValue, ref hit))
            {
                Color32 attenuation = Color32.Black;
                Ray r = new Ray(Vector2.Zero, Vector2.Zero);
                if (depth < MaxDepth && hit.Material.Scatter(ray, hit, ref attenuation, ref r))
                    return attenuation + Trace(r, depth + 1);
                return attenuation;
            }

            return Color32.Black;
        }

        public static Bitmap GetBitmap()
        {
            pointBitmap.LockBits();
            for (int i = 0; i < Buff.Length; i++)
                pointBitmap.SetColor(i, (byte) Mathd.Range(Buff[i] * 255 + 0.5, 0, 255));

            return pointBitmap.UnlockBits();
        }
    }
}
