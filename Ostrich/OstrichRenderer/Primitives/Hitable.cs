using System;
using OstrichRenderer.Materials;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;
using System.Collections.Generic;
using System.Linq;

namespace OstrichRenderer.Primitives
{
    public abstract class Hitable
    {
        public string Name;
        public abstract bool IsInside(Vector2 point);
        public abstract bool IsInside(Vector2 point, out Point p);
        public abstract bool IsOnBoundary(Vector2 point);
        public abstract List<LineSeg> GetLineSegs();

        public override string ToString() => Name;

        public static Union operator +(Hitable lhs, Hitable rhs) => new Union(lhs, rhs);

        //public static Substract operator -(Hitable lhs, Hitable rhs) => new Substract(lhs, rhs);
        //public static Substract operator -(Substract lhs, Hitable rhs)
        //{
        //    lhs.AddSubstraction(rhs);
        //    return lhs;
        //}
    }

    public class HitableList : Hitable
    {
        public readonly List<Hitable> List = new List<Hitable>();

        public override bool IsInside(Vector2 point) => List.Any(hitable => hitable.IsInside(point));
        public override bool IsInside(Vector2 point, out Point p)
        {
            foreach (Hitable hitable in List)
            {
                if (hitable.IsInside(point, out p)) return true;
            }
            p = new Point();
            return false;
        }

        public override bool IsOnBoundary(Vector2 point) => List.Any(hitable => hitable.IsOnBoundary(point));

        public override List<LineSeg> GetLineSegs()
        {
            List<LineSeg> line = new List<LineSeg>();
            foreach (Hitable hitable in List)
                line.AddRange(hitable.GetLineSegs());
            return line;
        }

        public void Add(Hitable item) => List.Add(item);
    }
}
