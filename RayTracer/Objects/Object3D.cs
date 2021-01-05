using System;
using System.Collections.Generic;
using System.Text;

using RayTracer.RayCast;
using RayTracer.Structs;

namespace RayTracer.Objects
{
    public enum PointLocation
    {
        Inside,
        On,
        OutSide
    }

    public abstract class Object3D : IRayCasted
    {
        public Vector3 position;

        protected Object3D(Vector3 position)
        {
            this.position = position;
        }

        public abstract Bounds GetBounds();
        public abstract List<HitInfo> RayCastAll(Ray ray, double length);

        public abstract PointLocation GetPointLocation(Vector3 point);
    }
}
