using RayTracer.Objects;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.RayCast
{
    public struct HitInfo : IFormattable
    {
        private Vector3 _hitNormal;

        public Vector3 HitPoint;
        public Vector3 HitNormal {
            get { return _hitNormal; }
            set
            {
                _hitNormal = value.Normalized;
            }
        }

        public HitInfo(Vector3 hitPoint, Vector3 hitNormal)
        {
            this.HitPoint = hitPoint;
            this._hitNormal = hitNormal.Normalized;
        }


        public string ToString(string format, IFormatProvider formatProvider)
        {
            return String.Join(String.Empty, new String[] {
                "HitInfo(",
                String.Join(", ", new String[] {
                            HitPoint.ToString(format, formatProvider),
                            HitNormal.ToString(format, formatProvider)
                            }),
                ")" });
        }
    }
}
