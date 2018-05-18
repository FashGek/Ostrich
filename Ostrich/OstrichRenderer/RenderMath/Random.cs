namespace OstrichRenderer.RenderMath
{
    class Random
    {
        private static long seed = 1;

        public static double Get()
        {
            seed = (0x5DEECE66DL * seed + 0xB16) & 0xFFFFFFFFFFFFL;
            return (seed >> 16) / (float)0x100000000L;
        }
    }
}
