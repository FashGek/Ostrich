using System.Drawing;

namespace OstrichRenderer.RenderMath
{
    class Color32
    {
        public double r, b, g, a;

        public Color32(double r, double g, double b, double a = 1)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Color32(Color c)
        {
            r = Mathd.Range((double)c.R / 255, 0, 1);
            g = Mathd.Range((double)c.G / 255, 0, 1);
            b = Mathd.Range((double)c.B / 255, 0, 1);
            a = Mathd.Range((double)c.A / 255, 0, 1);
        }

        public Color32()
        {
        }

        public override string ToString() => "<" + r + "," + g + "," + b + ">";

        public Color ToSystemColor()
        {
            if (double.IsNaN(r) || double.IsNaN(g) || double.IsNaN(b) || double.IsNaN(a)) return Color.DeepPink;
            return Color.FromArgb((int)(a * 255 + 0.5), (int)(r * 255 + 0.5), (int)(g * 255 + 0.5), (int)(b * 255 + 0.5));
        }

        public void Reset()
        {
            a = 0;
            r = 0;
            b = 0;
            g = 0;
        }

        public static Color32 operator +(Color32 a, Color32 b) =>
            new Color32(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);

        public static Color32 operator -(Color32 a, Color32 b) =>
            new Color32(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);

        public static Color32 operator *(Color32 a, Color32 b) =>
            new Color32(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);

        public static Color32 operator *(Color32 a, double b) => new Color32(a.r * b, a.g * b, a.b * b, a.a * b);

        public static Color32 operator *(double b, Color32 a) => new Color32(a.r * b, a.g * b, a.b * b, a.a * b);

        public static Color32 operator /(Color32 a, double b) => new Color32(a.r / b, a.g / b, a.b / b, a.a / b);

        public static Color32 Lerp(Color32 a, Color32 b, double t) => new Color32(a.r + (b.r - a.r) * t,
            a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);

        public static Color32 black = new Color32(0, 0, 0), white = new Color32(1, 1, 1);
    }
}
