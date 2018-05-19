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

        public Ray(Vector2 o, Vector2 d)
        {
            Origin = o;
            Direction = d.Normalize();
            NormalDirection = Direction;
        }
    }
}
