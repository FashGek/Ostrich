using OstrichRenderer.Materials;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public class Line : Hitable
    {
        private readonly double A, B, C;
        private readonly Vector2 Normal;
        private readonly Material Material;

        public Line(Vector2 p1, Vector2 p2, Material material)
        {
            GetABC(p1, p2, out A, out B, out C);
            Normal = new Vector2(-A, -B).Normalize();
            Material = material;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            bool isInside = IsInside(ray.Origin);
            Vector2 nowNormal = isInside ? -Normal : Normal;
            bool isOpposite = ray.Direction * nowNormal > 0;
            if (isOpposite && !isInside) return false;
            Vector2 inter = CalcIntersect(ray);
            double t = (inter - ray.Origin).Magnitude();
            if (t > tMax && !double.IsInfinity(t) && !isInside) return false;
            rec.P = inter;
            rec.Material = Material;
            rec.Normal = nowNormal;
            rec.T = isOpposite ? double.MaxValue : t;
            rec.IsInside = isInside;
            rec.Object = this;
            return true;
        }

        public override bool IsInside(Vector2 point) => A * point.X + B * point.Y + C >= 0;

        public override HitRecord[] GetAllCross(Ray ray, double tMin, double tMax)
        {
            HitRecord record = new HitRecord();
            return Hit(ray, tMin, tMax, ref record) ?  new[] {record} : new HitRecord[0];
        }

        private static void GetABC(Vector2 p1, Vector2 p2, out double a, out double b, out double c)
        {
            a = -(p1.Y - p2.Y);
            b = p1.X - p2.X;
            c = -b * p2.Y + -a * p2.X;
        }

        private Vector2 CalcIntersect(Ray ray)
        {
            double interx =
                (B * ray.Direction.Y * ray.Origin.X - B * ray.Direction.X * ray.Origin.Y - C * ray.Direction.X) /
                (A * ray.Direction.X + B * ray.Direction.Y);
            double intery;
            if (ray.Direction.X > 0)
                intery = (interx - ray.Origin.X) * ray.Direction.Y / ray.Direction.X + ray.Origin.Y;
            else intery = (-C - A * interx) / B;
            return new Vector2(interx, intery);
        }
    }
}