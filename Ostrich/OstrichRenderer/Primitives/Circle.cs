﻿using OstrichRenderer.Materials;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;
using System;
using System.Collections.Generic;

namespace OstrichRenderer.Primitives
{
    public class Circle : Hitable
    {
        public Vector2 Center;
        public float R, R2;
        public ushort Material;

        public Circle(Vector2 cen, float r, ushort material)
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

        public override bool IsOnBoundary(Vector2 point) => (Center - point).Magnitude() <= float.Epsilon;

        public override List<LineSeg> GetLineSegs() => new List<LineSeg>();
        
    }
}
