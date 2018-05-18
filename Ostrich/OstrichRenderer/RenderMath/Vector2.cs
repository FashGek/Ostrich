using System;

namespace OstrichRenderer.RenderMath
{
    public struct Vector2
    {
        public readonly double x, y;
        public static Vector2 One = new Vector2(1, 1);
        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return "<" + x + "," + y + ">";
        }

        public double Magnitude() => Math.Sqrt(x * x + y * y);

        public Vector2 Normalize()
        {
            var magnitude = Magnitude();
            return new Vector2(x / magnitude, y / magnitude);
        }
        public static Vector2 Normalize(Vector2 v)
        {
            var magnitude = v.Magnitude();
            return new Vector2(v.x / magnitude, v.y / magnitude);
        }

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs) => new Vector2(lhs.x + rhs.x, lhs.y + rhs.y);

        public static Vector2 operator *(Vector2 lhs, double v) => new Vector2(lhs.x * v, lhs.y * v);

        public static Vector2 operator *(double v, Vector2 rhs) => new Vector2(rhs.x * v, rhs.y * v);

        public static double operator *(Vector2 lhs, Vector2 rhs) => lhs.x * rhs.x + lhs.y * rhs.y;

        public static Vector2 operator /(Vector2 lhs, double v) => new Vector2(lhs.x / v, lhs.y / v);

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs) => new Vector2(lhs.x - rhs.x, lhs.y - rhs.y);

        public bool Equals(Vector2 other)
        {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj.GetType() == GetType() && Equals((Vector2)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }

        public static Vector2 operator -(Vector2 a) => new Vector2(-a.x, -a.y);

        public static bool operator ==(Vector2 lhs, Vector2 rhs) => SqrMagnitude(lhs - rhs) < 9.99999944E-11f;

        public static bool operator !=(Vector2 lhs, Vector2 rhs) => !(lhs == rhs);

        public static double SqrMagnitude(Vector2 vector) => vector.x * vector.x + vector.y * vector.y;

        public static double Dot(Vector2 lhs, Vector2 rhs) => lhs.x * rhs.x + lhs.y * rhs.y;

        //public static Vector2 Cross(Vector2 lhs, Vector2 rhs) => new Vector2(lhs.y * rhs.z - lhs.z * rhs.y,
        //    lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);

        public static Vector2 zero = new Vector2(0, 0);
    }
}