using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OstrichRenderer.Primitives;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;
using Random = OstrichRenderer.RenderMath.Random;

namespace OstrichRenderer.Materials
{
    public abstract class Material
    {
        public double Reflectivity;

        public abstract Color32 GetColor();
    }

    public class Light : Material
    {
        /// 强度
        public double Intensity;

        public Color32 Color;

        public Light(Color32 color, double intensity, double reflectivity)
        {
            Color = color;
            Intensity = intensity;
            Reflectivity = reflectivity;
        }

        public override Color32 GetColor() => Color * Intensity;
    }
}
