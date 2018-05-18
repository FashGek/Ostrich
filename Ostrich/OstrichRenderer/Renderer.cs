using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using OstrichRenderer.RenderMath;
using OstrichRenderer.Primitives;
using OstrichRenderer.Rendering;
using Random = OstrichRenderer.RenderMath.Random;

namespace OstrichRenderer
{
    public class Renderer
    {
        public static int Width, Height, Sample;
        private static HitableList World = new HitableList();

        private static double[] Buff;
        private static int[] ChangeTimes;
        private static PointBitmap pointBitmap;

        public static void Init(int width, int height, int sample)
        {
            Width = width;
            Height = height;
            Sample = sample;
            Buff = new double[width * height * 4];
            pointBitmap = new PointBitmap(new Bitmap(width, height));
        }

        public static void InitScene()
        {
            World.list.Add(new Circle(new Vector2(256, 256), 50));
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
            double sum = 0;
            for (int i = 0; i < Sample; i++)
            {
                double a = Mathd.TwoPI * (i + Random.Get()) / Sample;
                sum += Trace(new Ray(new Vector2(x, y),
                    new Vector2(Math.Cos(a), Math.Sin(a))));
            }

            Buff[y * Width * 4 + x * 4] = sum / Sample;
            Buff[y * Width * 4 + x * 4 + 1] = sum / Sample;
            Buff[y * Width * 4 + x * 4 + 2] = sum / Sample;
            Buff[y * Width * 4 + x * 4 + 3] = 1;
        }

        private static double Trace(Ray ray)
        {
            HitRecord hit = new HitRecord();
            if (World.Hit(ray, 0, double.MaxValue, ref hit))
            {
                return 1;
            }

            return 0;
        }

        public static Bitmap GetBitmap()
        {
            pointBitmap.LockBits();
            for (int i = 0; i < Buff.Length; i++)
                pointBitmap.SetColor(i, (byte)(Buff[i] * 255 + 0.5));

            return pointBitmap.UnlockBits();
        }

        public static BitmapSource GetbBitmapSource() => Bitmap2BitmapImage(GetBitmap());

        private static BitmapSource Bitmap2BitmapImage(Bitmap bitmap) =>
            Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
    }
}
