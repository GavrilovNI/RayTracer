using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer
{
    public static class MathUtils
    {
        public const double Epsilon = 1e-15;
        public const double Deg2Rad = (System.Math.PI * 2) / 360;
        public const double Rad2Deg = 360 / (System.Math.PI * 2);

        public static bool Equal(double a, double b)
        {
            return Math.Abs(a - b) < Epsilon;
        }
    }
}
