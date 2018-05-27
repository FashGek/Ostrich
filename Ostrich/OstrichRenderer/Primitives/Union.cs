using System.Collections.Generic;
using System.Linq;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public class Union : Hitable
    {
        private readonly List<Hitable> Hitables;

        public Union(params Hitable[] hitables)
        {
            Hitables = hitables.ToList();
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            HitRecord record = new HitRecord();
            List<HitRecord> records = (from t in Hitables where t.Hit(ray, tMin, tMax, ref record) select record).ToList();
            if (records.Count == 0) return false;

            double mint = double.MaxValue;
            foreach (HitRecord hitRecord in records)
                if (hitRecord.T < mint)
                {
                    mint = hitRecord.T;
                    rec = hitRecord;
                }
            rec.IsInside = IsInside(ray.Origin);
            rec.Object = this;
            return true;
        }

        public override bool IsInside(Vector2 point) => Hitables.Any(hitable => hitable.IsInside(point));
        public override HitRecord[] GetAllCross(Ray ray, double tMin, double tMax)
        {
            List<HitRecord> records = new List<HitRecord>();
            foreach (Hitable hitable in Hitables)
            {
                records.AddRange(hitable.GetAllCross(ray, tMin, tMax));
            }
            return records.ToArray();
        }

        public void AddItem(Hitable hitable)
        {
            Hitables.Add(hitable);
        }
    }
}