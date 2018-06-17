using System;

namespace OstrichRenderer.RenderMath
{
    public struct Vector2
    {
        public readonly double X, Y;

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => "<" + X + "," + Y + ">";

        public double Magnitude() => Math.Sqrt(X * X + Y * Y);

        public Vector2 Normalize()
        {
            var magnitude = Magnitude();
            return new Vector2(X / magnitude, Y / magnitude);
        }
        public static Vector2 Normalize(Vector2 v)
        {
            var magnitude = v.Magnitude();
            return new Vector2(v.X / magnitude, v.Y / magnitude);
        }

        public static double Angle(Vector2 from, Vector2 to) =>
            /*Math.Tan(*/Math.Acos(Mathd.Range(Dot(from.Normalize(), to.Normalize()), -1d, 1d))/* * 57.29578)*/;

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs) => new Vector2(lhs.X + rhs.X, lhs.Y + rhs.Y);

        public static Vector2 operator *(Vector2 lhs, double v) => new Vector2(lhs.X * v, lhs.Y * v);

        public static Vector2 operator *(double v, Vector2 rhs) => new Vector2(rhs.X * v, rhs.Y * v);

        public static double operator *(Vector2 lhs, Vector2 rhs) => lhs.X * rhs.X + lhs.Y * rhs.Y;

        public static Vector2 operator /(Vector2 lhs, double v) => new Vector2(lhs.X / v, lhs.Y / v);

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs) => new Vector2(lhs.X - rhs.X, lhs.Y - rhs.Y);

        public bool Equals(Vector2 other) => X.Equals(other.X) && Y.Equals(other.Y);

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj.GetType() == GetType() && Equals((Vector2)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static Vector2 operator -(Vector2 a) => new Vector2(-a.X, -a.Y);

        public static bool operator ==(Vector2 lhs, Vector2 rhs) => SqrMagnitude(lhs - rhs) < 9.99999944E-11f;

        public static bool operator !=(Vector2 lhs, Vector2 rhs) => !(lhs == rhs);

        public static double SqrMagnitude(Vector2 vector) => vector.X * vector.X + vector.Y * vector.Y;

        public static double Dot(Vector2 lhs, Vector2 rhs) => lhs.X * rhs.X + lhs.Y * rhs.Y;

        //public static Vector2 Cross(Vector2 lhs, Vector2 rhs) => new Vector2(lhs.y * rhs.z - lhs.z * rhs.y,
        //    lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);

        public static Vector2 Zero = new Vector2(0, 0);
        public static Vector2 One = new Vector2(1, 1);
    }
}