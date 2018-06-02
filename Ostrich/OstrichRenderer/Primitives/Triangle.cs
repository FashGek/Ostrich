using OstrichRenderer.Materials;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public class Triangle : Hitable
    {
        private readonly Intersect Intersect;

        /// <summary>
        /// 点按逆时针顺序传递
        /// </summary>
        public Triangle(Vector2 p1, Vector2 p2, Vector2 p3, Material material)
        {
            Intersect = new Intersect(new Line(p1, p2, material), new Line(p2, p3, material));
            Intersect = new Intersect(Intersect, new Line(p3, p1, material));
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec) => Intersect.Hit(ray, tMin, tMax, ref rec);

        public override bool IsInside(Vector2 point) => Intersect.IsInside(point);

        public override HitRecord[] GetAllCross(Ray ray, double tMin, double tMax) => Intersect.GetAllCross(ray, tMin, tMax);
    }
}