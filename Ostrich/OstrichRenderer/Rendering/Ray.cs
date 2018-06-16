using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Rendering
{
    public struct Ray
    {
        public readonly Vector2 Origin, Direction, NormalDirection;

        public Ray(Vector2 o, Vector2 d)
        {
            Origin = o;
            Direction = d;
            NormalDirection = d.Normalize();
        }
    }
}
