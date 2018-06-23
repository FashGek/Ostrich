using OstrichRenderer.RenderMath;
using System.Collections.Generic;

namespace OstrichRenderer.Rendering
{
    public struct LineSeg
    {
        public Vector2 P1, P2, Position, Normal;
        public uint ID;
        public ushort Material;

        public LineSeg(Vector2 p1, Vector2 p2, Vector2 normal, ushort material)
        {
            P1 = p1;
            P2 = p2;
            Position = (p1 + p2) / 2;
            Normal = normal;
            Material = material;
            ID = 0;
        }

        public bool IsInside(Vector2 point) => point.X >= Mathf.Min(P1.X, P2.X) && point.X <= Mathf.Max(P1.X, P2.X) &&
                                               point.Y >= Mathf.Min(P1.Y, P2.Y) && point.Y <= Mathf.Max(P1.Y, P2.Y);

        public void Cut(Vector2 point, out LineSeg l1, out LineSeg l2)
        {
            l1 = new LineSeg(P1, point, Normal, Material);
            l2 = new LineSeg(P2, point, Normal, Material);
        }

        public static List<LineSeg> Cut(LineSeg l1, LineSeg l2, Vector2 i)
        {
            List<LineSeg> line = new List<LineSeg>();
            l1.Cut(i, out LineSeg i1, out LineSeg i2);
            line.Add(i1);
            line.Add(i2);
            l2.Cut(i, out i1, out i2);
            line.Add(i1);
            line.Add(i2);
            return line;
        }

        public List<LineSeg> Cut(List<Vector2> point)
        {
            List<LineSeg> lineSegs = new List<LineSeg> {this};
            foreach (Vector2 vector2 in point)
            {
                for (int i = 0; i < lineSegs.Count; i++)
                {
                    if (!lineSegs[i].IsInside(vector2)) continue;
                    lineSegs[i].Cut(vector2, out LineSeg l1, out LineSeg l2);
                    lineSegs.RemoveAt(i);
                    lineSegs.Add(l1);
                    lineSegs.Add(l2);
                    break;
                }
            }
            return lineSegs;
        }

        public static List<LineSeg> DivideByIntersection(List<LineSeg> lineSegs)
        {
            List<LineSeg> lineSeg = new List<LineSeg>();
            for (int i = 0; i < lineSegs.Count; i++)
            {
                List<Vector2> vector2s = new List<Vector2>();
                for (int a = 0; a < lineSegs.Count; a++)
                {
                    if (i == a) continue;
                    if (!IsIntersect(lineSegs[i], lineSegs[a], out Vector2 inter)) continue;
                    vector2s.Add(inter);
                }
                lineSeg.AddRange(lineSegs[i].Cut(vector2s));
            }
            return lineSeg;
        }

        public static bool IsIntersect(LineSeg l1, LineSeg l2, out Vector2 i)
        {
            //if (l1.IsConnectedWith(l2))
            //{
            //    i = new Vector2();
            //    return false;
            //}

            Vector2 s1 = l1.P2 - l1.P1;
            Vector2 s2 = l2.P2 - l2.P1;

            double s = (-s1.Y * (l1.P1.X - l2.P1.X) + s1.X * (l1.P1.Y - l2.P1.Y)) / (-s2.X * s1.Y + s1.X * s2.Y);
            double t = (s2.X * (l1.P1.Y - l2.P1.Y) - s2.Y * (l1.P1.X - l2.P1.X)) / (-s2.X * s1.Y + s1.X * s2.Y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                i = new Vector2(l1.P1.X + t * s1.X, l1.P1.Y + t * s1.Y);
                return i != l2.P1 && i != l2.P2;
            }
            i = new Vector2();
            return false;
        }

        //public bool IsConnectedWith(LineSeg line) => P1 == line.P1 || P1 == line.P2 || P2 == line.P1 || line.P2 == P2;

        public double Length() => (P1 - P2).Magnitude();
    }
}