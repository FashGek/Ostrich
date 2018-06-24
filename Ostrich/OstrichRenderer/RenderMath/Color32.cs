using System.Drawing;

namespace OstrichRenderer.RenderMath
{
    public class Color32
    {
        public double R, B, G/*, A*/;

        public Color32(double r, double g, double b/*, double a = 1*/)
        {
            R = r;
            G = g;
            B = b;
            //A = a;
        }

        public Color32(Color c)
        {
            R = Mathd.Range((double)c.R / 255, 0, 1);
            G = Mathd.Range((double)c.G / 255, 0, 1);
            B = Mathd.Range((double)c.B / 255, 0, 1);
            //A = Mathd.Range((double)c.A / 255, 0, 1);
        }

        public override string ToString() => "<" + R + "," + G + "," + B + ">";

        public static Color32 operator +(Color32 a, Color32 b) =>
            new Color32(a.R + b.R, a.G + b.G, a.B + b.B/*, a.A + b.A*/);

        public static Color32 operator -(Color32 a, Color32 b) =>
            new Color32(a.R - b.R, a.G - b.G, a.B - b.B/*, a.A - b.A*/);

        public static Color32 operator *(Color32 a, Color32 b) =>
            new Color32(a.R * b.R, a.G * b.G, a.B * b.B/*, a.A * b.A*/);

        public static Color32 operator *(Color32 a, double b) => new Color32(a.R * b, a.G * b, a.B * b/*, a.A * b*/);

        public static Color32 operator *(double b, Color32 a) => new Color32(a.R * b, a.G * b, a.B * b/*, a.A * b*/);

        public static Color32 operator /(Color32 a, double b) => new Color32(a.R / b, a.G / b, a.B / b/*, a.A / b*/);

        public static Color32 Black = new Color32(0, 0, 0), White = new Color32(1, 1, 1);
    }
}
