using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Structs
{
    public struct Vector3 : IFormattable
    {
        public const double kEpsilon = 0.00001;
        public const double kEpsilonNormalSqrt = 1e-15;

        public double x, y, z;
        public double SqrMagnitude { get { return x * x + y * y + z * z; } }
        public double Magnitude { get { return (double)Math.Sqrt(this.SqrMagnitude); } }

        public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, double t)
        {
            return a + (b - a) * t;
        }
        public static Vector3 Lerp(Vector3 a, Vector3 b, double t)
        {
            t = Math.Clamp(t, 0, 1);
            return LerpUnclamped(a, b, t);
        }
        
        public static Vector3 MoveTowards(Vector3 current, Vector3 target, double maxDistanceDelta)
        {
            Vector3 toVector = target - current;

            double sqdist = toVector.SqrMagnitude;

            if (sqdist == 0 || (maxDistanceDelta >= 0 && sqdist <= maxDistanceDelta * maxDistanceDelta))
                return target;
            var dist = (double)Math.Sqrt(sqdist);

            return current + toVector / dist * maxDistanceDelta;
        }



        public Vector3(double x = 0, double y = 0, double z = 0) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 Scale(Vector3 a, Vector3 scale) { return new Vector3(a.x * scale.x, a.y * scale.y, a.z * scale.z); }
        public void Scale(Vector3 scale) { this = Vector3.Scale(this, scale); }
        public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(
                lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.z * rhs.x - lhs.x * rhs.z,
                lhs.x * rhs.y - lhs.y * rhs.x
            );
        }

        public override int GetHashCode() { return HashCode.Combine(x, y, z); }

        public override bool Equals(object obj)
        {
            return obj is Vector3 vector &&
                   x == vector.x &&
                   y == vector.y &&
                   z == vector.z;
        }

        public static Vector3 Reflect(Vector3 inDirection, Vector3 inNormal)
        {
            double factor = -2 * Vector3.Dot(inNormal, inDirection);
            return factor * inNormal + inDirection;
        }

        public void Normalize()
        {
            double mag = this.Magnitude;
            if (mag > kEpsilon)
                this = this / mag;
            else
                this = Zero;
        }

        public Vector3 Normalized {
            get {
                double mag = this.Magnitude;
                if (mag > kEpsilon)
                    return this / mag;
                else
                    return Zero;
            }
        }

        public static double Dot(Vector3 a, Vector3 b) { return a.x * b.x + a.y * b.y + a.z * b.z; }

        public static Vector3 ProjectOnVector(Vector3 vector, Vector3 vectorToProjectOn)
        {
            double sqrMag = Vector3.Dot(vectorToProjectOn, vectorToProjectOn);
            if (sqrMag < kEpsilon)
                return Vector3.Zero;
            else
            {
                var dot = Vector3.Dot(vector, vectorToProjectOn);
                return vectorToProjectOn * dot / sqrMag;
            }
        }
        public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal)
        {
            return vector - Vector3.ProjectOnVector(vector, planeNormal);
        }

        public static double Angle(Vector3 from, Vector3 to)
        {
            double denominator = (double)Math.Sqrt(from.SqrMagnitude * to.SqrMagnitude);
            if (denominator < kEpsilonNormalSqrt)
                return 0;

            double dot = Math.Clamp(Vector3.Dot(from, to) / denominator, -1, 1);
            return ((double)Math.Acos(dot)) * MathUtils.Rad2Deg;
        }

        public static double SignedAngle(Vector3 from, Vector3 to, Vector3 axis)
        {
            double unsignedAngle = Vector3.Angle(from, to);
            Vector3 cross = Vector3.Cross(from, to);
            double sign = Math.Sign(Vector3.Dot(axis, cross));
            return unsignedAngle * sign;
        }

        public static double Distance(Vector3 a, Vector3 b)
        {
            return (a - b).Magnitude;
        }
        public static double SqrDistance(Vector3 a, Vector3 b)
        {
            return (a - b).SqrMagnitude;
        }

        public static Vector3 ClampMagnitude(Vector3 vector, double maxLength)
        {
            double sqrmag = vector.SqrMagnitude;
            if (sqrmag > maxLength * maxLength)
            {
                double mag = (double)Math.Sqrt(sqrmag);
                return vector / mag * maxLength;
            }
            return vector;
        }
        public static Vector3 Min(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
        }
        public static Vector3 Max(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
        }

        public static Vector3 Zero => new Vector3(0, 0, 0);
        public static Vector3 One => new Vector3(1, 1, 1);
        public static Vector3 Up => new Vector3(0, 1, 0);
        public static Vector3 Down => new Vector3(0, -1, 0);
        public static Vector3 Right => new Vector3(1, 0, 0);
        public static Vector3 Left => new Vector3(-1, 0, 0);
        public static Vector3 Forward => new Vector3(0, 0, 1);
        public static Vector3 Backward => new Vector3(0, 0, -1);
        public static Vector3 PositiveInfinity => new Vector3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        public static Vector3 NegativeInfinity => new Vector3(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);


        public static Vector3 operator +(Vector3 a, Vector3 b) { return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z); }
        public static Vector3 operator -(Vector3 a, Vector3 b) { return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z); }
        public static Vector3 operator -(Vector3 a) { return new Vector3(-a.x, -a.y, -a.z); }
        public static Vector3 operator *(Vector3 a, double value) { return new Vector3(a.x * value, a.y * value, a.z * value); }
        public static Vector3 operator *(double value, Vector3 a) { return a * value; }
        public static Vector3 operator /(Vector3 a, double d) { return new Vector3(a.x / d, a.y / d, a.z / d); }
        public static bool operator ==(Vector3 a, Vector3 b) { return (a - b).SqrMagnitude < kEpsilon * kEpsilon; }
        public static bool operator !=(Vector3 a, Vector3 b) { return !(a == b); }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return String.Join(String.Empty, new String[] {
                "Vector3(",
                String.Join(", ", new String[] {
                            x.ToString(format, formatProvider),
                            y.ToString(format, formatProvider),
                            z.ToString(format, formatProvider)
                            }),
                ")" });
        }

    }
}
