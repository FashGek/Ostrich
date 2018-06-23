using System;
using System.Collections.Generic;
using OstrichRenderer.Materials;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public class Line : Hitable
    {
        private readonly double A, B, C;
        private readonly Vector2 Normal, P1, P2;
        private readonly ushort Material;

        public Line(Vector2 p1, Vector2 p2, ushort material)
        {
            P1 = p1;
            P2 = p2;
            GetABC(p1, p2, out A, out B, out C);
            Normal = new Vector2(-A, -B).Normalize();
            Material = material;
        }

        public override bool IsInside(Vector2 point) => A * point.X + B * point.Y + C >= 0;
        public override bool IsInside(Vector2 point, out Point p)
        {
            if (A * point.X + B * point.Y + C >= 0)
            {
                p = new Point(Material, point);
                return true;
            }
            p = new Point();
            return false;
        }

        public override bool IsOnBoundary(Vector2 point) => Math.Abs(A * point.X + B * point.Y + C) >= double.Epsilon;

        public override List<LineSeg> GetLineSegs() => new List<LineSeg> {new LineSeg(P1, P2, Normal, Material)};

        private static void GetABC(Vector2 p1, Vector2 p2, out double a, out double b, out double c)
        {
            a = -(p1.Y - p2.Y);
            b = p1.X - p2.X;
            c = -b * p2.Y + -a * p2.X;
        }

        private Vector2 CalcIntersect(Ray ray)
        {
            double interx =
                (B * ray.Direction.Y * ray.Origin.X - B * ray.Direction.X * ray.Origin.Y - C * ray.Direction.X) /
                (A * ray.Direction.X + B * ray.Direction.Y);
            double intery;
            if (ray.Direction.X > 0)
                intery = (interx - ray.Origin.X) * ray.Direction.Y / ray.Direction.X + ray.Origin.Y;
            else intery = (-C - A * interx) / B;
            return new Vector2(interx, intery);
        }
    }
}