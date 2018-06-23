using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;
using System;
using System.Collections.Generic;

namespace OstrichRenderer.Primitives
{
    [Obsolete]
    public class Circle : Hitable
    {
        public Vector2 Center;
        public double R, R2;
        public ushort Material;

        public Circle(Vector2 cen, double r, ushort material)
        {
            Center = cen;
            Material = material;
            R = r;
            R2 = r * r;
        }

        public override bool IsInside(Vector2 point) => (Center - point).Magnitude() <= R;
        public override bool IsInside(Vector2 point, out Point p)
        {
            p = new Point();
            if (!((Center - point).Magnitude() <= R)) return false;
            p = new Point(Material, point);
            return true;
        }

        public override bool IsOnBoundary(Vector2 point) => (Center - point).Magnitude() <= double.Epsilon;

        public override List<LineSeg> GetLineSegs() => new List<LineSeg>();
        
    }
}
