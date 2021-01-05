using System;
using System.Collections.Generic;
using System.Text;

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
