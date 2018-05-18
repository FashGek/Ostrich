using System;

namespace OstrichRenderer.RenderMath
{
    class Mathd
    {
        public static double Range(double v, double min, double max) => (v <= min) ? min :
            v >= max ? max : v;

        public static double Tan(double f) => Math.Tan(f);

        public static double Pow(double f, double p) => Math.Pow(f, p);

        public static double GetK(double a) => Math.Tan(TwoPI * a);

        public const double PI = 3.1415926535897932384626433832795;
        public const double TwoPI = 6.283185307179586476925286766559;
    }
}
