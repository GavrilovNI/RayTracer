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

        public Sphere(Vector3 position, Material material, double radius) : base(position, material)
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

        public override bool RayCast(Ray ray, double maxDistance, out HitInfo hitInfo)
        {
            Vector3 sphereToRay = ray.Origin - this.position;
            double a = Vector3.Dot(ray.Direction, ray.Direction);
            double b = 2.0 * Vector3.Dot(ray.Direction, sphereToRay);
            double c = Vector3.Dot(sphereToRay, sphereToRay) - radius * radius;

            double discriminant = b * b - 4.0 * a * c;

            if (discriminant < 0)
            {
                hitInfo = new HitInfo();
                return false;
            }

            bool isRayOriginInside = this.GetPointLocation(ray.Origin) == PointLocation.Inside;

            double discriminantSqrt = Math.Sqrt(discriminant);
            double dist = isRayOriginInside ? (-b + discriminantSqrt) / (2.0 * a) : (-b - discriminantSqrt) / (2.0 * a);
            if (dist >= 0 && dist <= maxDistance)
            {
                Vector3 pos = ray.Position(dist);
                Vector3 normal1 = isRayOriginInside ? this.position - pos : pos - this.position;
                hitInfo = new HitInfo(pos, dist, normal1, this);
                return true;
            }

            hitInfo = new HitInfo();
            return false;
        }

        public override List<HitInfo> RayCastAll(Ray ray, double maxDistance)
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

            List<double> dists = new List<double>();
            dists.Add((-b - discriminantSqrt) / (2.0 * a));
            if (discriminant != 0)
                dists.Add((-b + discriminantSqrt) / (2.0 * a));

            for (int i = 0; i < dists.Count; i++)
            {
                if(dists[i]>=0 && dists[i]<=maxDistance)
                {
                    Vector3 pos = ray.Position(dists[i]);
                    Vector3 normal = this.position - pos;
                    if (Vector3.Dot(normal, ray.Direction) > 0)
                        normal = -normal;
                    result.Add(new HitInfo(pos, dists[i], normal, this));
                }
            }

            return result;
        }
    }
}
