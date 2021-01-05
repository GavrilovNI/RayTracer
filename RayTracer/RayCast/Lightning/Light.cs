using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.RayCast.Lightning
{
    public abstract class Light
    {
        public double intensity;

        public Light(double intensity)
        {
            this.intensity = intensity;
        }

        public abstract double GetLight(Vector3 point, Vector3 normal, Scene scene);
    }
}
