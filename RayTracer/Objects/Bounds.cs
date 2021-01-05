using System;
using System.Collections.Generic;
using System.Text;
using RayTracer.RayCast;
using RayTracer.Structs;

namespace RayTracer.Objects
{
    public class Bounds
    {
        Vector3 min, max;

        public Bounds() : this(Vector3.Zero, Vector3.Zero)
        {
        }
        public Bounds(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

        public PointLocation GetPointLocation(Vector3 point)
        {
            if (point.x < min.x || point.y < min.y || point.z < min.z)
                return PointLocation.OutSide;
            if (point.x > max.x || point.y > max.y || point.z > max.z)
                return PointLocation.OutSide;

            if (point.x == min.x || point.y == min.y || point.z == min.z)
                return PointLocation.On;
            if (point.x == max.x || point.y == max.y || point.z == max.z)
                return PointLocation.On;

            return PointLocation.Inside;
        }

        

        public static Bounds operator+ (Bounds a, Bounds b)
        {
            return new Bounds(Vector3.Min(a.min, b.min), Vector3.Max(a.max, b.max));
        }
        public static Bounds operator +(Bounds a, Vector3 b)
        {
            return new Bounds(a.min + b, a.max + b);
        }
        public static Bounds operator -(Bounds a, Vector3 b)
        {
            return a + -b;
        }
    }
}
