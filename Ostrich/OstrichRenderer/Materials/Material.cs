using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Materials
{
    public struct Material
    {
        public Color32 Color;

        /// 强度
        public double Intensity;

        public Material(Color32 color, double intensity = 0)
        {
            Color = color;
            Intensity = intensity;
        }

        public Color32 GetColor() => Intensity * Color;
    }
}
