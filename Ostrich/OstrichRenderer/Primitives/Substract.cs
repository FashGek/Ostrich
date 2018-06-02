using System.Collections.Generic;
using System.Linq;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    ///相交判断没写好，有bug
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
            if (!Minuend.Hit(ray, tMin, tMax, ref mrecord)) return false;//获得射线与被减物体的交点
            List<HitRecord> records = new List<HitRecord>();
            foreach (Hitable hitable in Substraction)
                records.AddRange(hitable.GetAllCross(ray, tMin, tMax));//获得射线与所有减（数）的交点

            //判断所有交点是否在减（数）内，如果否，就是有效交点
            List<HitRecord> validRecords = new List<HitRecord>();
            for (int i = 0; i < records.Count; i++)
            {
                bool inside = false;
                for (int a = 0; a < Substraction.Count; a++)
                {
                    if (records[i].Object == Substraction[a]) continue;//如果是相同的物体就不必判断了
                    if (!Substraction[a].IsInside(records[i].P)) continue;//如果交点在物体内则为无效交点
                    inside = true;
                    break;
                }
                //判断交点是不是在被减物体内，如果是，就是有效交点
                if (!inside && Minuend.IsInside(records[i].P))
                    validRecords.Add(records[i]);
            }
            //判断射线与被减物体的交点是否在减（数）内，如果否，把此交点加入列表中
            if (!Substraction.Any(hit => hit.IsInside(mrecord.P)))
                validRecords.Add(mrecord);
            if (validRecords.Count == 0) return false;

            //循环找出路径最短的点
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