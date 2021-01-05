using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.RayCast.Lightning
{
    public class PointLight : Light
    {
        public Vector3 position;
        public double radius;

        public PointLight(double intensity, Vector3 position) : base(intensity)
        {
            this.position = position;
        }

        public override double GetLight(Vector3 point, Vector3 normal, Scene scene)
        {
            if(scene.RayCast(new Ray(point, this.position - point), Vector3.Distance(point, position)+MathUtils.Epsilon, out HitInfo hitInfo))
            {
                return 0;
            }

            double dot = Vector3.Dot(normal, (this.position - point).Normalized);
            if(dot<0)
            {
                return 0;
            }


            return dot;
        }
    }
}
