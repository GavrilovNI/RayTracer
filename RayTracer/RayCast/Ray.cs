using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.RayCast
{
    public struct Ray
    {
        private Vector3 _direction;

        public Vector3 Origin;
        public Vector3 Direction {
            get { return this._direction; }
            set
            {
                this._direction = value.Normalized;
                if (_direction == Vector3.Zero)
                    throw new ArgumentException("Ray direction must be non-zero vector.");
            }
        }

        public Ray(Vector3 origin, Vector3 direction)
        {
            this.Origin = origin;
            this._direction = direction.Normalized;
            if (_direction == Vector3.Zero)
                throw new ArgumentException("Ray direction must be non-zero vector.");
        }

        public Vector3 Position(double distance)
        {
            return Origin + Direction * distance;
        }
    }
}
