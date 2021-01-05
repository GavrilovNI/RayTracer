using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.RayCast
{
    public interface IRayCasted
    {

        public bool RayCast(Ray ray, double maxDistance, out HitInfo hitInfo); // raycast only to first intersection
        public List<HitInfo> RayCastAll(Ray ray, double maxDistance); // full raycast - return all intesection

    }
}
