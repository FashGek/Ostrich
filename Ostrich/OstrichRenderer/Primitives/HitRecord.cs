using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public struct HitRecord
    {
        /// 距离
        public double T;
        /// 交点
        public Vector2 P;
        /// 法线
        public Vector2 Normal;

        public bool IsInside;
        /// 材质
        public ushort Material;
    }
}