using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.RayCast
{
    public interface IRayCasted
    {
        
        public List<HitInfo> RayCastAll(Ray ray, double length);

    }
}
