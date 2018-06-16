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

        public override bool IsInside(Vector2 point) => O1.IsInside(point) || O2.IsInside(point);
        public override bool IsInside(Vector2 point, out Point p) => O1.IsInside(point, out p) || O2.IsInside(point, out p);

        public override bool IsOnBoundary(Vector2 point) => O1.IsOnBoundary(point) || O2.IsOnBoundary(point);

        public override List<LineSeg> GetLineSegs()
        {
            List<LineSeg> lineSeg = new List<LineSeg>();
            lineSeg.AddRange(O1.GetLineSegs());
            lineSeg.AddRange(O2.GetLineSegs());
            lineSeg = LineSeg.DivideByIntersection(lineSeg);
            for (int i = 0; i < lineSeg.Count; i++)
                if (O1.IsInside(lineSeg[i].Position) && O1.IsOnBoundary(lineSeg[i].Position) ||
                    O2.IsInside(lineSeg[i].Position) && O2.IsOnBoundary(lineSeg[i].Position)) lineSeg.RemoveAt(i);
            return lineSeg;
        }
    }
}