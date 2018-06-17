using System;

namespace OstrichRenderer.RenderMath
{
    class Mathf
    {
        public static float Range(float v, float min, float max) => v <= min ? min :
            v >= max ? max : v;

        public static float GetK(float a) => (float)Math.Tan(TwoPi * a);

        public static float Min(float d1, float d2) => d1 > d2 ? d2 : d1;

        public static float Max(float d1, float d2) => d1 > d2 ? d1 : d2;

        public const float Pi = 3.1415926535897932384626433832795f;
        public const float TwoPi = 6.283185307179586476925286766559f;
    }
}
