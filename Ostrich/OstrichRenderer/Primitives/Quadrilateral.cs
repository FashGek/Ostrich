using OstrichRenderer.Materials;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public class Quadrilateral : Hitable
    {
        private readonly Intersect Intersect;

        /// <summary>
        /// 点按逆时针顺序传递
        /// </summary>
        public Quadrilateral(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Material material)
        {
            Intersect i1 = new Intersect(new Line(p1, p2, material), new Line(p2, p3, material));
            Intersect i2 = new Intersect(new Line(p3, p4, material), new Line(p4, p1, material));
            Intersect = new Intersect(i1, i2);
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec) => Intersect.Hit(ray, tMin, tMax, ref rec);

        public override bool IsInside(Vector2 point) => Intersect.IsInside(point);

        public override HitRecord[] GetAllCross(Ray ray, double tMin, double tMax)
        {
            HitRecord[] hitRecords = Intersect.GetAllCross(ray, tMin, tMax);
            for (int hit = 0; hit < hitRecords.Length; hit++)
                hitRecords[hit].Object = this;
            return hitRecords;
        }
    }
}