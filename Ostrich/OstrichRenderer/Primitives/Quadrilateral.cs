using System.Collections.Generic;
using OstrichRenderer.Materials;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public class Quadrilateral : Hitable
    {
        private readonly Intersect Intersect;

        /// <summary>
        /// 点按逆时针顺序传递
        /// </summary>
        public Quadrilateral(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ushort material)
        {
            Intersect i1 = new Intersect(new Line(p1, p2, material), new Line(p2, p3, material));
            Intersect i2 = new Intersect(new Line(p3, p4, material), new Line(p4, p1, material));
            Intersect = new Intersect(i1, i2);
        }

        public override bool IsInside(Vector2 point) => Intersect.IsInside(point);
        public override bool IsInside(Vector2 point, out Point p) => Intersect.IsInside(point, out p);

        public override bool IsOnBoundary(Vector2 point) => Intersect.IsOnBoundary(point);

        public override List<LineSeg> GetLineSegs() => Intersect.GetLineSegs();
    }
}