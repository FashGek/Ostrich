using OstrichRenderer.Materials;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;
using System.Collections.Generic;
using System.Linq;

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
        public Material Material;

        public Hitable Object;
    }


    public abstract class Hitable
    {
        public string Name;
        public abstract bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec);
        public abstract bool IsInside(Vector2 point);
        public abstract HitRecord[] GetAllCross(Ray ray, double tMin, double tMax);

        public override string ToString()
        {
            return Name;
        }

        public static Union operator +(Hitable lhs, Hitable rhs) => new Union(lhs, rhs);

        public static Substract operator -(Hitable lhs, Hitable rhs) => new Substract(lhs, rhs);
        public static Substract operator -(Substract lhs, Hitable rhs)
        {
            lhs.AddSubstraction(rhs);
            return lhs;
        }
    }

    public class HitableList : Hitable
    {
        public readonly List<Hitable> List = new List<Hitable>();

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            HitRecord tempRecord = new HitRecord();
            bool hitAnything = false;
            double closest = tMax;
            foreach (Hitable h in List)
            {
                if (!h.Hit(ray, tMin, closest, ref tempRecord)) continue;
                hitAnything = true;
                closest = tempRecord.T;
                rec = tempRecord;
                if (rec.IsInside) return true;
            }

            return hitAnything;
        }

        public override bool IsInside(Vector2 point) => List.Any(hitable => hitable.IsInside(point));
        public override HitRecord[] GetAllCross(Ray ray, double tMin, double tMax)
        {
            List<HitRecord> records = new List<HitRecord>();
            foreach (Hitable hitable in List)
            {
                records.AddRange(hitable.GetAllCross(ray, tMin, tMax));
            }
            return records.ToArray();
        }

        public void Add(Hitable item) => List.Add(item);
    }
}
