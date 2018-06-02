using System.Collections.Generic;
using System.Linq;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public class Union : Hitable
    {
        private readonly Hitable O1, O2;

        public Union(Hitable o1, Hitable o2)
        {
            O1 = o1;
            O2 = o2;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            HitRecord record1 = new HitRecord();
            HitRecord record2 = new HitRecord();
            bool b1 = O1.Hit(ray, tMin, tMax, ref record1);
            bool b2 = O2.Hit(ray, tMin, tMax, ref record2);

            if (!(b1 || b2)) return false;//如果与两个物体都不相交才视为与整个物体无交点

            if (!b1) rec = record2;
            else if (!b2) rec = record1;
            else rec = record1.T > record2.T ? record2 : record1;//如果与两个物体都有交点，选最近的那个

            rec.IsInside = IsInside(ray.Origin);
            rec.Object = this;
            return true;
        }

        public override bool IsInside(Vector2 point) => O1.IsInside(point) || O2.IsInside(point);
        public override HitRecord[] GetAllCross(Ray ray, double tMin, double tMax)
        {
            List<HitRecord> records = new List<HitRecord>();
            records.AddRange(O1.GetAllCross(ray, tMin, tMax));
            records.AddRange(O2.GetAllCross(ray, tMin, tMax));
            return records.ToArray();
        }
    }
}