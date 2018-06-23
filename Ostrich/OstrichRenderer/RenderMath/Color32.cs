using System.Drawing;

namespace OstrichRenderer.RenderMath
{
    public class Color32
    {
        public double R, B, G, A;

        public Color32(double r, double g, double b, double a = 1)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color32(Color c)
        {
            R = Mathf.Range((double)c.R / 255, 0, 1);
            G = Mathf.Range((double)c.G / 255, 0, 1);
            B = Mathf.Range((double)c.B / 255, 0, 1);
            A = Mathf.Range((double)c.A / 255, 0, 1);
        }

        public Color32()
        {
        }

        public override string ToString() => "<" + R + "," + G + "," + B + ">";

        public Color ToSystemColor()
        {
            if (double.IsNaN(R) || double.IsNaN(G) || double.IsNaN(B) || double.IsNaN(A)) return Color.DeepPink;
            return Color.FromArgb((int)(A * 255 + 0.5), (int)(R * 255 + 0.5), (int)(G * 255 + 0.5), (int)(B * 255 + 0.5));
        }

        public void Reset()
        {
            A = 0;
            R = 0;
            B = 0;
            G = 0;
        }

        public static Color32 operator +(Color32 a, Color32 b) =>
            new Color32(a.R + b.R, a.G + b.G, a.B + b.B, a.A + b.A);

        public static Color32 operator -(Color32 a, Color32 b) =>
            new Color32(a.R - b.R, a.G - b.G, a.B - b.B, a.A - b.A);

        public static Color32 operator *(Color32 a, Color32 b) =>
            new Color32(a.R * b.R, a.G * b.G, a.B * b.B, a.A * b.A);

        public static Color32 operator *(Color32 a, double b) => new Color32(a.R * b, a.G * b, a.B * b, a.A * b);

        public static Color32 operator *(double b, Color32 a) => new Color32(a.R * b, a.G * b, a.B * b, a.A * b);

        public static Color32 operator /(Color32 a, double b) => new Color32(a.R / b, a.G / b, a.B / b, a.A / b);

        public static Color32 Lerp(Color32 a, Color32 b, double t) => new Color32(a.R + (b.R - a.R) * t,
            a.G + (b.G - a.G) * t, a.B + (b.B - a.B) * t, a.A + (b.A - a.A) * t);

        public static Color32 Black = new Color32(0, 0, 0), White = new Color32(1, 1, 1);
    }
}
