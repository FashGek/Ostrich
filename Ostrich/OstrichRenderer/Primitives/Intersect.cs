using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;
using System.Collections.Generic;
using System.Linq;

namespace OstrichRenderer.Primitives
{
    public class Intersect : Hitable
    {
        private readonly Hitable O1, O2;

        public Intersect(Hitable o1, Hitable o2)
        {
            O1 = o1;
            O2 = o2;
        }

        public override bool IsInside(Vector2 point) => O1.IsInside(point) && O2.IsInside(point);
        public override bool IsInside(Vector2 point, out Point p) => O1.IsInside(point, out p) && O2.IsInside(point, out p);

        public override bool IsOnBoundary(Vector2 point) => O1.IsOnBoundary(point) && O2.IsOnBoundary(point);

        public override List<LineSeg> GetLineSegs()
        {
            List<LineSeg> lineSeg1 = new List<LineSeg>();
            List<LineSeg> lineSeg2 = new List<LineSeg>();
            List<LineSeg> lineSeg = new List<LineSeg>();
            lineSeg1.AddRange(O1.GetLineSegs());
            lineSeg1 = LineSeg.DivideByIntersection(lineSeg1);
            for (int i = 0; i < lineSeg1.Count; i++)
                if (O2.IsInside(lineSeg1[i].Position)) lineSeg.Add(lineSeg1[i]);
            lineSeg2.AddRange(O2.GetLineSegs());
            lineSeg2 = LineSeg.DivideByIntersection(lineSeg2);
            for (int i = 0; i < lineSeg2.Count; i++)
                if (O1.IsInside(lineSeg2[i].Position)) lineSeg.Add(lineSeg2[i]);
            return lineSeg;
        }

        /*
         
        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            HitRecord record1 = new HitRecord(), record2 = new HitRecord();
            if(!(O1.Hit(ray,tMin,tMax, ref record1)&& O2.Hit(ray, tMin, tMax, ref record2))) return false;

            if (O2.IsInside(record1.P) && O1.IsInside(record2.P))
                rec = (record1.P - ray.Origin).Magnitude() > (record2.P - ray.Origin).Magnitude() ? record2 : record1;
            else if (O2.IsInside(record1.P)) rec = record1;
            else if (O1.IsInside(record2.P)) rec = record2;
            else return false;
            return true;
        }

        public override bool IsInside(Vector2 point) => O1.IsInside(point) && O2.IsInside(point);

         */

        /*
         
                 public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            bool isInside = IsInside(ray.Origin);
            HitRecord[] records = new HitRecord[Hitables.Length];
            HitRecord record = new HitRecord();
            for (int i = 0; i < Hitables.Length; i++)
            {
                if (!Hitables[i].Hit(ray, tMin, tMax, ref record)) return false;
                records[i] = record;
            }

            List<HitRecord> validRecords = new List<HitRecord>();
            for (int i = 0; i < records.Length; i++)
            {
                bool inside = true;
                for (int a = 0; a < records.Length; a++)
                {
                    if (i == a) continue;
                    if (Hitables[a].IsInside(records[i].P)) continue;
                    inside = false;
                    break;
                }
                if (inside || isInside)
                    validRecords.Add(records[i]);
            }
            if (validRecords.Count == 0) return false;

            double mint = validRecords[0].T;
            rec = validRecords[0];
            foreach (HitRecord validRecord in validRecords)
                if (validRecord.T <= mint)
                {
                    mint = validRecord.T;
                    rec = validRecord;
                }
            rec.IsInside = isInside;
            rec.Object = this;
            return true;
        }

        public override bool IsInside(Vector2 point) => Hitables.All(hitable => hitable.IsInside(point));
         
         */
    }
}