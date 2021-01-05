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

    public abstract class Object3D : IRayCasted, IHaveBounds
    {
        public Vector3 position;
        public Material material;

        protected Object3D(Vector3 position, Material material)
        {
            this.position = position;
            this.material = material;
        }

        public abstract Bounds GetBounds();

        public abstract bool RayCast(Ray ray, double maxDistance, out HitInfo hitInfo);
        public abstract List<HitInfo> RayCastAll(Ray ray, double maxDistance);

        public abstract PointLocation GetPointLocation(Vector3 point);

        
    }
}
