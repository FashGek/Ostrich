using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public class HitRecord
    {
        public double u, v;
        public double t;
        public Vector2 p;
        public Vector2 normal;
        //public Material material;
        public HitRecord() { }

        public HitRecord(HitRecord copy)
        {
            u = copy.u;
            v = copy.v;
            p = copy.p;
            normal = copy.normal;
            //material = copy.material;
        }

        public HitRecord(Vector2 p)
        {
            this.p = p;
        }
    }


    public abstract class Hitable
    {
        public string name;
        public abstract bool Hit(Ray ray, double t_min, double t_max, ref HitRecord rec);

        public override string ToString()
        {
            return name;
        }
    }

    public class HitableList : Hitable
    {
        public readonly List<Hitable> list = new List<Hitable>();

        public override bool Hit(Ray ray, double t_min, double t_max, ref HitRecord rec)
        {
            var temp_record = new HitRecord();
            var hit_anything = false;
            var closest = t_max;
            foreach (var h in list)
            {
                if (!h.Hit(ray, t_min, closest, ref temp_record)) continue;
                hit_anything = true;
                closest = temp_record.t;
                rec = temp_record;
                if (rec.t == 0) return true;
            }

            return hit_anything;
        }
    }
}
