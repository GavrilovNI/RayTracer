using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Objects
{
    public interface IHaveBounds
    {
        Bounds GetBounds();
    }
}
