using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Structs
{
    public struct Quaternion
    {
        public const double kEpsilon = 0.000001;

        public double x, y, z, w;

        public Quaternion(double x = 0, double y = 0, double z = 0, double w = 1) { this.x = x; this.y = y; this.z = z; this.w = w; }
        public Quaternion(Vector3 xyz, double w = 1) { this.x = xyz.x; this.y = xyz.y; this.z = xyz.z; this.w = w; }


        public static Quaternion Identity => new Quaternion(0, 0, 0, 1);


        public Vector3 XYZ
        {
            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }
            get
            {
                return new Vector3(x, y, z);
            }
        }
        public Vector3 EulerAngles
        {
            get { return MakePositive(ToEulerRad(this) * MathUtils.Rad2Deg); }
            set { this = FromEulerRad(value * MathUtils.Deg2Rad); }
        }
        public double LengthSquared => this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w;
        public double Length => Math.Sqrt(LengthSquared);
        public Quaternion Normalized
        {
            get
            {
                double mag = Math.Sqrt(Dot(this, this));

                if (mag < MathUtils.Epsilon)
                    return Quaternion.Identity;

                return new Quaternion(this.x / mag, this.y / mag, this.z / mag, this.w / mag);
            }
        }

        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
            return new Quaternion(
                lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
                lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x,
                lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z
            );
        }
        public static Vector3 operator *(Quaternion rotation, Vector3 point)
        {
            double x = rotation.x * 2;
            double y = rotation.y * 2;
            double z = rotation.z * 2;
            double xx = rotation.x * x;
            double yy = rotation.y * y;
            double zz = rotation.z * z;
            double xy = rotation.x * y;
            double xz = rotation.x * z;
            double yz = rotation.y * z;
            double wx = rotation.w * x;
            double wy = rotation.w * y;
            double wz = rotation.w * z;

            Vector3 res;
            res.x = (1 - (yy + zz)) * point.x + (xy - wz) * point.y + (xz + wy) * point.z;
            res.y = (xy + wz) * point.x + (1 - (xx + zz)) * point.y + (yz - wx) * point.z;
            res.z = (xz - wy) * point.x + (yz + wx) * point.y + (1 - (xx + yy)) * point.z;
            return res;
        }
        public static Vector3 operator *(Vector3 point, Quaternion rotation)
        {
            return rotation * point;
        }
        public static bool operator ==(Quaternion a, Quaternion b)
        {
            return IsEqualUsingDot(Dot(a, b));
        }
        public static bool operator !=(Quaternion a, Quaternion b)
        {
            return !(a == b);
        }


        private static bool IsEqualUsingDot(double dot)
        {
            return dot > 1.0 - kEpsilon;
        }
        public static double Dot(Quaternion a, Quaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public static Quaternion LookRotation(Vector3 forward, Vector3 upwards)
        {
            return Quaternion.LookRotation(ref forward, ref upwards);
        }
        public static Quaternion LookRotation(Vector3 forward)
        {
            Vector3 up = Vector3.Up;
            return Quaternion.LookRotation(ref forward, ref up);
        }
        // from http://answers.unity3d.com/questions/467614/what-is-the-source-code-of-quaternionlookrotation.html
        private static Quaternion LookRotation(ref Vector3 forward, ref Vector3 up)
        {

            forward.Normalize();
            Vector3 right = Vector3.Cross(up, forward).Normalized;
            up = Vector3.Cross(forward, right);
            var m00 = right.x;
            var m01 = right.y;
            var m02 = right.z;
            var m10 = up.x;
            var m11 = up.y;
            var m12 = up.z;
            var m20 = forward.x;
            var m21 = forward.y;
            var m22 = forward.z;


            double num8 = (m00 + m11) + m22;
            var quaternion = new Quaternion();
            if (num8 > 0f)
            {
                var num = System.Math.Sqrt(num8 + 1f);
                quaternion.w = num * 0.5f;
                num = 0.5f / num;
                quaternion.x = (m12 - m21) * num;
                quaternion.y = (m20 - m02) * num;
                quaternion.z = (m01 - m10) * num;
                return quaternion;
            }
            if ((m00 >= m11) && (m00 >= m22))
            {
                var num7 = System.Math.Sqrt(((1f + m00) - m11) - m22);
                var num4 = 0.5f / num7;
                quaternion.x = 0.5f * num7;
                quaternion.y = (m01 + m10) * num4;
                quaternion.z = (m02 + m20) * num4;
                quaternion.w = (m12 - m21) * num4;
                return quaternion;
            }
            if (m11 > m22)
            {
                var num6 = System.Math.Sqrt(((1f + m11) - m00) - m22);
                var num3 = 0.5f / num6;
                quaternion.x = (m10 + m01) * num3;
                quaternion.y = 0.5f * num6;
                quaternion.z = (m21 + m12) * num3;
                quaternion.w = (m20 - m02) * num3;
                return quaternion;
            }
            var num5 = System.Math.Sqrt(((1f + m22) - m00) - m11);
            var num2 = 0.5f / num5;
            quaternion.x = (m20 + m02) * num2;
            quaternion.y = (m21 + m12) * num2;
            quaternion.z = 0.5f * num5;
            quaternion.w = (m01 - m10) * num2;
            return quaternion;
        }
        public void SetLookRotation(Vector3 view)
        {
            SetLookRotation(view, Vector3.Up);
        }
        public void SetLookRotation(Vector3 view, Vector3 up)
        {
            this = LookRotation(view, up);
        }

        public static double Angle(Quaternion a, Quaternion b)
        {
            double dot = Dot(a, b);
            return IsEqualUsingDot(dot) ? 0.0 : Math.Acos(Math.Min(Math.Abs(dot), 1.0)) * 2.0 * MathUtils.Rad2Deg;
        }
        private static double NormalizeAngle(double angle)
        {
            angle %= 360;
            if (angle < 0)
                angle += 360;
            return angle;
        }
        private static Vector3 NormalizeAngles(Vector3 angles)
        {
            angles.x = NormalizeAngle(angles.x);
            angles.y = NormalizeAngle(angles.y);
            angles.z = NormalizeAngle(angles.z);
            return angles;
        }

        private static Vector3 MakePositive(Vector3 euler)
        {
            double negativeFlip = -0.0001 * MathUtils.Rad2Deg;
            double positiveFlip = 360.0 + negativeFlip;

            if (euler.x < negativeFlip)
                euler.x += 360.0;
            else if (euler.x > positiveFlip)
                euler.x -= 360.0;

            if (euler.y < negativeFlip)
                euler.y += 360.0;
            else if (euler.y > positiveFlip)
                euler.y -= 360.0;

            if (euler.z < negativeFlip)
                euler.z += 360.0;
            else if (euler.z > positiveFlip)
                euler.z -= 360.0;

            return euler;
        }


        private static Vector3 ToEulerRad(Quaternion rotation)
        {
            double sqw = rotation.w * rotation.w;
            double sqx = rotation.x * rotation.x;
            double sqy = rotation.y * rotation.y;
            double sqz = rotation.z * rotation.z;
            double unit = sqx + sqy + sqz + sqw;
            double test = rotation.x * rotation.w - rotation.y * rotation.z;
            Vector3 v;

            if (test > 0.4995f * unit)
            {
                v.y = 2f * Math.Atan2(rotation.y, rotation.x);
                v.x = Math.PI / 2;
                v.z = 0;
                return NormalizeAngles(v * MathUtils.Rad2Deg);
            }
            if (test < -0.4995f * unit)
            {
                v.y = -2f * Math.Atan2(rotation.y, rotation.x);
                v.x = -Math.PI / 2;
                v.z = 0;
                return NormalizeAngles(v * MathUtils.Rad2Deg);
            }
            Quaternion q = new Quaternion(rotation.w, rotation.z, rotation.x, rotation.y);
            v.y = System.Math.Atan2(2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));
            v.x = System.Math.Asin(2f * (q.x * q.z - q.w * q.y));
            v.z = System.Math.Atan2(2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));
            return NormalizeAngles(v * MathUtils.Rad2Deg);
        }
        private static Quaternion FromEulerRad(Vector3 euler)
        {
            var yaw = euler.x;
            var pitch = euler.y;
            var roll = euler.z;
            double rollOver2 = roll * 0.5f;
            double sinRollOver2 = System.Math.Sin(rollOver2);
            double cosRollOver2 = System.Math.Cos(rollOver2);
            double pitchOver2 = pitch * 0.5f;
            double sinPitchOver2 = System.Math.Sin(pitchOver2);
            double cosPitchOver2 = System.Math.Cos(pitchOver2);
            double yawOver2 = yaw * 0.5f;
            double sinYawOver2 = System.Math.Sin(yawOver2);
            double cosYawOver2 = System.Math.Cos(yawOver2);
            Quaternion result;
            result.x = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
            result.y = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;
            result.z = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
            result.w = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
            return result;

        }
        private static void ToAxisAngleRad(Quaternion q, out Vector3 axis, out double angle)
        {
            if (System.Math.Abs(q.w) > 1.0f)
                q.Normalize();
            angle = 2.0f * System.Math.Acos(q.w);
            double den = System.Math.Sqrt(1.0 - q.w * q.w);
            if (den > 0.0001f)
            {
                axis = q.XYZ / den;
            }
            else
            {
                axis = new Vector3(1, 0, 0);
            }
        }

        
        public static Quaternion Euler(double x, double y, double z) { return FromEulerRad(new Vector3(x, y, z) * MathUtils.Deg2Rad); }
        public static Quaternion Euler(Vector3 euler) { return FromEulerRad(euler * MathUtils.Deg2Rad); }
        public static Quaternion AngleAxis(double angle, Vector3 axis)
        {
            return Quaternion.AngleAxis(angle, ref axis);
        }
        private static Quaternion AngleAxis(double degress, ref Vector3 axis)
        {
            if (axis.SqrMagnitude == 0.0f)
                return Identity;

            Quaternion result = Identity;
            var radians = degress * MathUtils.Deg2Rad;
            radians *= 0.5f;
            axis.Normalize();
            axis = axis * System.Math.Sin(radians);
            result.x = axis.x;
            result.y = axis.y;
            result.z = axis.z;
            result.w = System.Math.Cos(radians);

            return result.Normalized;
        }
        public void ToAngleAxis(out double angle, out Vector3 axis)
        {
            Quaternion.ToAxisAngleRad(this, out axis, out angle);
            angle *= MathUtils.Rad2Deg;
        }
        public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
        {
            return RotateTowards(LookRotation(fromDirection), LookRotation(toDirection), double.MaxValue);
        }
        public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection) { this = FromToRotation(fromDirection, toDirection); }

        public static Quaternion Slerp(Quaternion a, Quaternion b, double t)
        {
            return Quaternion.Slerp(ref a, ref b, t);
        }
        private static Quaternion Slerp(ref Quaternion a, ref Quaternion b, double t)
        {
            if (t > 1) t = 1;
            if (t < 0) t = 0;
            return SlerpUnclamped(ref a, ref b, t);
        }
        public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, double t)
        {
            return Quaternion.SlerpUnclamped(ref a, ref b, t);
        }
        private static Quaternion SlerpUnclamped(ref Quaternion a, ref Quaternion b, double t)
        {
            if (a.LengthSquared == 0.0f)
            {
                if (b.LengthSquared == 0.0f)
                {
                    return Identity;
                }
                return b;
            }
            else if (b.LengthSquared == 0.0f)
            {
                return a;
            }


            double cosHalfAngle = a.w * b.w + Vector3.Dot(a.XYZ, b.XYZ);

            if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
            {
                return a;
            }
            else if (cosHalfAngle < 0.0f)
            {
                b.XYZ = -b.XYZ;
                b.w = -b.w;
                cosHalfAngle = -cosHalfAngle;
            }

            double blendA;
            double blendB;
            if (cosHalfAngle < 0.99f)
            {
                double halfAngle = System.Math.Acos(cosHalfAngle);
                double sinHalfAngle = System.Math.Sin(halfAngle);
                double oneOverSinHalfAngle = 1.0f / sinHalfAngle;
                blendA = System.Math.Sin(halfAngle * (1.0f - t)) * oneOverSinHalfAngle;
                blendB = System.Math.Sin(halfAngle * t) * oneOverSinHalfAngle;
            }
            else
            {
                blendA = 1.0f - t;
                blendB = t;
            }

            Quaternion result = new Quaternion(blendA * a.XYZ + blendB * b.XYZ, blendA * a.w + blendB * b.w);
            if (result.LengthSquared > 0.0f)
                return result.Normalized;
            else
                return Identity;
        }
        public static Quaternion Lerp(Quaternion a, Quaternion b, double t)
        {
            if (t > 1) t = 1;
            if (t < 0) t = 0;
            return Slerp(ref a, ref b, t);
        }
        public static Quaternion LerpUnclamped(Quaternion a, Quaternion b, double t)
        {
            return Slerp(ref a, ref b, t);
        }
        public static Quaternion RotateTowards(Quaternion from, Quaternion to, double maxDegreesDelta)
        {
            double angle = Quaternion.Angle(from, to);
            if (angle == 0.0) return to;
            return SlerpUnclamped(from, to, Math.Min(1.0, maxDegreesDelta / angle));
        }

        public void Normalize()
        {
            this = this.Normalized;
        }


        public override int GetHashCode() { return HashCode.Combine(x, y, z, w); }

        public override bool Equals(object obj)
        {
            return obj is Quaternion quaternion &&
                   x == quaternion.x &&
                   y == quaternion.y &&
                   z == quaternion.z &&
                   w == quaternion.w;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return String.Join(String.Empty, new String[] {
                "Quaternion(",
                String.Join(", ", new String[] {
                            x.ToString(format, formatProvider),
                            y.ToString(format, formatProvider),
                            z.ToString(format, formatProvider),
                            w.ToString(format, formatProvider)
                            }),
                ")" });
        }
    }
}
