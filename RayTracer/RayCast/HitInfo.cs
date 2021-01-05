using RayTracer.Objects;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.RayCast
{
    public struct HitInfo : IFormattable
    {
        private Vector3 _point;
        private double _distance;
        private Vector3 _normal;
        private Object3D _object3D;


        public Vector3 Point { get { return _point; } set { _point = value; } }

        public double Distance { get { return _distance; } set { _distance = value; } }
        public Vector3 Normal {
            get { return _normal; }
            set
            {
                _normal = value.Normalized;
            }
        }

        public Object3D Object3D
        {
            get { return _object3D; }
            set
            {
                _object3D = value;
            }
        }

        public HitInfo(Vector3 point, double distance, Vector3 normal, Object3D object3D)
        {
            this._point = point;
            this._distance = distance;
            this._normal = normal.Normalized;
            this._object3D = object3D;
        }

        public HitInfo(Ray ray, double distance, Vector3 normal, Object3D object3D)
        {
            this._point = ray.Position(distance);
            this._distance = distance;
            this._normal = normal.Normalized;
            if (Vector3.Dot(_normal, ray.Direction) > 0)
                _normal = -_normal;
            this._object3D = object3D;
        }


        public string ToString(string format, IFormatProvider formatProvider)
        {
            return String.Join(String.Empty, new String[] {
                "HitInfo(",
                String.Join(", ", new String[] {
                            Point.ToString(format, formatProvider),
                            Distance.ToString(format, formatProvider),
                            Normal.ToString(format, formatProvider)
                            }),
                ")" });
        }
    }
}
