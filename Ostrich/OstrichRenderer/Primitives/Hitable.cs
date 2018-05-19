using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OstrichRenderer.Materials;
using OstrichRenderer.Rendering;
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
        /// 材质
        public Material Material;
    }


    public abstract class Hitable
    {
        public string Name;
        public abstract bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec);

        public override string ToString()
        {
            return Name;
        }
    }

    public class HitableList : Hitable
    {
        public readonly List<Hitable> List = new List<Hitable>();

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            var tempRecord = new HitRecord();
            var hitAnything = false;
            var closest = tMax;
            foreach (var h in List)
            {
                if (!h.Hit(ray, tMin, closest, ref tempRecord)) continue;
                hitAnything = true;
                closest = tempRecord.T;
                rec = tempRecord;
                if (rec.T == 0) return true;//这意味着在某个物体内
            }

            return hitAnything;
        }
    }
}
