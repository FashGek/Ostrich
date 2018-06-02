using System.Collections.Generic;
using System.Linq;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    /// 由于在堆上的内存分配较频繁，GC消耗大
    public class Substract : Hitable
    {
        private readonly Hitable Minuend;
        private readonly List<Hitable> Substraction;

        public Substract(Hitable minuend, params Hitable[] substraction)
        {
            Minuend = minuend;
            Substraction = substraction.ToList();
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            HitRecord mrecord = new HitRecord();
            if (!Minuend.Hit(ray, tMin, tMax, ref mrecord)) return false;
            List<HitRecord> records = new List<HitRecord>();
            foreach (Hitable hitable in Substraction)
                records.AddRange(hitable.GetAllCross(ray, tMin, tMax));

            List<HitRecord> validRecords = new List<HitRecord>();
            for (int i = 0; i < records.Count; i++)
            {
                bool inside = false;
                for (int a = 0; a < Substraction.Count; a++)
                {
                    if (records[i].Object == Substraction[a]) continue;
                    if (!Substraction[a].IsInside(records[i].P)) continue;
                    inside = true;
                    break;
                }
                if (!inside)
                    validRecords.Add(records[i]);
            }
            validRecords = validRecords.Where(hitRecord => Minuend.IsInside(hitRecord.P)).ToList();
            if (!Substraction.All(hit => hit.IsInside(mrecord.P)))
                validRecords.Add(mrecord);
            if (validRecords.Count == 0) return false;

            double mint = validRecords[0].T;
            rec = validRecords[0];
            foreach (HitRecord validRecord in validRecords)
                if (validRecord.T <= mint)
                {
                    mint = validRecord.T;
                    rec = validRecord;
                }
            rec.IsInside = IsInside(ray.Origin);
            rec.Object = this;
            return true;
        }

        public override bool IsInside(Vector2 point)
        {
            if (!Minuend.IsInside(point)) return false;
            return !Substraction.Any(hitable => hitable.IsInside(point));
        }

        public override HitRecord[] GetAllCross(Ray ray, double tMin, double tMax)
        {
            List<HitRecord> records = new List<HitRecord>();
            foreach (Hitable hitable in Substraction)
            {
                records.AddRange(hitable.GetAllCross(ray, tMin, tMax));
            }
            records.AddRange(Minuend.GetAllCross(ray, tMin, tMax));
            return records.ToArray();
        }

        private bool IsInSubstraction(Vector2 point) => Substraction.Any(hitable => hitable.IsInside(point));

        public void AddSubstraction(Hitable hitable)
        {
            Substraction.Add(hitable);
        }
    }
}