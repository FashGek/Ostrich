namespace OstrichRenderer.RenderMath
{
    class Random
    {
        public static long Seed = 1;

        public static float Get()
        {
            Seed = (0x5DEECE66DL * Seed + 0xB16) & 0xFFFFFFFFFFFFL;
            return (Seed >> 16) / (float)0x100000000L;
        }
    }
}
