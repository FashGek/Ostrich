using System.Collections.Generic;
using System.Linq;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    /// 相交判断没写好，有bug
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

        public override bool IsInside(Vector2 point)
        {
            if (!Minuend.IsInside(point)) return false;
            return !Substraction.Any(hitable => hitable.IsInside(point));
        }

        public override bool IsInside(Vector2 point, out Point p)
        {
            p = new Point();
            if (!Minuend.IsInside(point)) return false;
            foreach (Hitable hitable in Substraction)
                if (hitable.IsInside(point, out p))
                    return false;
            return true;
        }

        public override bool IsOnBoundary(Vector2 point)
        {
            return Minuend.IsOnBoundary(point) && !Substraction.Any(hitable => hitable.IsInside(point)) ||
                   Minuend.IsInside(point) && !Substraction.Any(hitable => hitable.IsOnBoundary(point));
        }

        public override List<LineSeg> GetLineSegs()
        {
            List<LineSeg> lineSeg = new List<LineSeg>();
            foreach (Hitable hitable in Substraction)
                lineSeg.AddRange(hitable.GetLineSegs());
            for (int i = 0; i < lineSeg.Count; i++)
            {
                LineSeg seg = lineSeg[i];
                seg.Normal = -lineSeg[i].Normal;
                lineSeg[i] = seg;
            }
            lineSeg.AddRange(Minuend.GetLineSegs());
            lineSeg = LineSeg.DivideByIntersection(lineSeg);
            for (int i = 0; i < lineSeg.Count; i++)
                if (!Minuend.IsInside(lineSeg[i].Position) || IsInSubstraction(lineSeg[i].Position))
                    lineSeg.RemoveAt(i--);
            return lineSeg;
        }

        private bool IsInSubstraction(Vector2 point) =>
            Substraction.Any(hitable => hitable.IsInside(point) && hitable.IsOnBoundary(point));

        public void AddSubstraction(Hitable hitable) => Substraction.Add(hitable);
    }
}