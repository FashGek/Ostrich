using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public struct LineSeg
    {
        public Vector2 P1, P2, Normal, NormalP2;

        public LineSeg(Vector2 p1, Vector2 p2)
        {
            P1 = p1;
            P2 = p2;
            Vector2 v = p1 - p2;
            Normal = new Vector2(v.Y, -v.X).Normalize();
            NormalP2 = p2.Normalize();
        }

        public bool IsInside(Vector2 point) => Normal * (point - P1) <= 0;

        public static bool IsIntersect(LineSeg l1, LineSeg l2, ref HitRecord rec)
        {
            Vector2 s1 = l1.P2 - l1.P1;
            Vector2 s2 = l2.P2 - l2.P1;

            double s = (-s1.Y * (l1.P1.X - l2.P1.X) + s1.X * (l1.P1.Y - l2.P1.Y)) / (-s2.X * s1.Y + s1.X * s2.Y);
            double t = (s2.X * (l1.P1.Y - l2.P1.Y) - s2.Y * (l1.P1.X - l2.P1.X)) / (-s2.X * s1.Y + s1.X * s2.Y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                Vector2 vector2 = new Vector2(l1.P1.X + t * s1.X, l1.P1.Y + t * s1.Y);
                double d = (vector2 - l1.P1).Magnitude();
                if (d >= rec.T) return false;
                rec.P = vector2;
                rec.Normal = l2.Normal;
                rec.T = d;
                return true;
            }
            return false;
        }
    }
}