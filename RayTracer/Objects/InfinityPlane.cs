using RayTracer.RayCast;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Objects
{
    public class InfinityPlane : Object3D
    {


        public InfinityPlane(Vector3 position, Material material) :base(position, material)
        {

        }

        public override Bounds GetBounds()
        {
            return new Bounds(new Vector3(double.NegativeInfinity, position.y, double.NegativeInfinity),
                new Vector3(double.PositiveInfinity, position.y, double.PositiveInfinity));
        }

        public override PointLocation GetPointLocation(Vector3 point)
        {
            if (MathUtils.Equal(point.y, position.y))
                return PointLocation.On;
            return PointLocation.OutSide;
        }

        public override bool RayCast(Ray ray, double maxDistance, out HitInfo hitInfo)
        {
            Vector3 normal = Vector3.Up;
            double denom = Vector3.Dot(normal, ray.Direction);
            if (Math.Abs(denom) > 0.0001f)
            {
                double t = Vector3.Dot((Vector3.Up*position.y - ray.Origin), normal) / denom;
                if (t >= 0 && t <= maxDistance)
                {
                    hitInfo = new HitInfo(ray, t, normal, this);
                    return true;
                }
            }
            hitInfo = new HitInfo();
            return false;
        }

        public override List<HitInfo> RayCastAll(Ray ray, double maxDistance)
        {
            var result = new List<HitInfo>();
            if (RayCast(ray, maxDistance, out HitInfo hitInfo))
                result.Add(hitInfo);
            return result;
        }
    }
}
