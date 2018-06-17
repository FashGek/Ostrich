using System;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Rendering
{
    public struct Point : IComparable<Point>
    {
        public ushort Materail;
        public uint Line;
        public Vector2 Position;
        public float Distance;

        public Point(ushort materail, uint line, Vector2 position)
        {
            Materail = materail;
            Line = line;
            Position = position;
            Distance = 0;
        }

        public Point(ushort materail, Vector2 position)
        {
            Materail = materail;
            Line = 0;
            Position = position;
            Distance = 0;
        }

        public int CompareTo(Point other) => Distance.CompareTo(other.Distance);

        public override string ToString() => Position.ToString();
    }
}