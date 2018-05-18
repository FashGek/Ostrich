using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Rendering
{
    public class Ray
    {
        public readonly Vector2 Origin, Direction, NormalDirection;
        public readonly double K, C;

        public Ray(Vector2 o, Vector2 d)
        {
            Origin = o;
            Direction = d.Normalize();
            NormalDirection = Direction;
            K = d.y / d.x;
            C = o.y - K * o.x;
        }

        public Vector2 GetPoint(double x) => Origin + new Vector2(x, K * x);
    }
}
