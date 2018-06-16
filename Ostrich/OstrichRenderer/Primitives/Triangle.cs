using System.Collections.Generic;
using OstrichRenderer.Materials;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public class Triangle : Hitable
    {
        private readonly Intersect Intersect;

        /// <summary>
        /// 点按逆时针顺序传递
        /// </summary>
        public Triangle(Vector2 p1, Vector2 p2, Vector2 p3, ushort material)
        {
            Intersect = new Intersect(new Line(p1, p2, material), new Line(p2, p3, material));
            Intersect = new Intersect(Intersect, new Line(p3, p1, material));
        }

        public override bool IsInside(Vector2 point) => Intersect.IsInside(point);
        public override bool IsInside(Vector2 point, out Point p) => Intersect.IsInside(point, out p);

        public override bool IsOnBoundary(Vector2 point) => Intersect.IsOnBoundary(point);

        public override List<LineSeg> GetLineSegs() => Intersect.GetLineSegs();
    }
}