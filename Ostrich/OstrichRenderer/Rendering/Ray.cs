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
