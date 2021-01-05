using System;
using System.Collections.Generic;
using System.Text;
using RayTracer.RayCast;
using RayTracer.Structs;

namespace RayTracer.Objects
{
    public class Sphere : Object3D
    {
        public double radius;

        public Sphere(Vector3 position, double radius) : base(position)
        {
            if (radius < 0)
                throw new ArgumentException("Radius must be not negative.");
            this.radius = radius;
        }

        public override Bounds GetBounds()
        {
            Vector3 maxBound = radius * Vector3.One;
            return new Bounds(-maxBound, maxBound);
        }

        public override PointLocation GetPointLocation(Vector3 point)
        {
            double sqrDist = Vector3.SqrDistance(this.position, point);
            double sqrRadius = radius * radius;
            if (sqrDist == sqrRadius)
                return PointLocation.On;
            if (sqrDist < sqrRadius)
                return PointLocation.Inside;
            return PointLocation.OutSide;
        }

        public override List<HitInfo> RayCastAll(Ray ray, double distance)
        {
            List<HitInfo> result = new List<HitInfo>();

            Vector3 sphereToRay = ray.Origin - this.position;
            double a = Vector3.Dot(ray.Direction, ray.Direction);
            double b = 2.0 * Vector3.Dot(ray.Direction, sphereToRay);
            double c = Vector3.Dot(sphereToRay, sphereToRay) - radius * radius;

            double discriminant = b * b - 4.0 * a * c;

            if (discriminant < 0)
                return result;

            double discriminantSqrt = Math.Sqrt(discriminant);
            double dist1 = (-b - discriminantSqrt) / (2.0 * a);
            if (dist1 >= 0 && dist1 <= distance)
            {
                Vector3 pos1 = ray.Position(dist1);
                bool isRayOriginInside = this.GetPointLocation(ray.Origin) == PointLocation.Inside;
                Vector3 normal1 = isRayOriginInside ? this.position - pos1 : pos1 - this.position;
                result.Add(new HitInfo(pos1, normal1));
            }
            if(discriminant != 0)
            {
                double dist2 = (-b + discriminantSqrt) / (2.0 * a);
                if (dist2 >= 0 && dist2 <= distance)
                {
                    Vector3 pos2 = ray.Position(dist2);
                    Vector3 normal2 = this.position - pos2;
                    result.Add(new HitInfo(pos2, normal2));
                }
            }

            return result;
        }
    }
}
