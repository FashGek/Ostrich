using System;

namespace OstrichRenderer.RenderMath
{
    class Mathd
    {
        public static double Range(double v, double min, double max) => v <= min ? min :
            v >= max ? max : v;

        public static double GetK(double a) => Math.Tan(TwoPi * a);

        public static double Min(double d1, double d2) => d1 > d2 ? d2 : d1;

        public static double Max(double d1, double d2) => d1 > d2 ? d1 : d2;

        public const double Pi = 3.1415926535897932384626433832795;
        public const double TwoPi = 6.283185307179586476925286766559;
    }
}
