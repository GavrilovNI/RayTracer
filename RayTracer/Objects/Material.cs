using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RayTracer.Objects
{
    public struct Material
    {
        private Color _color;
        private double _ambient;
        private double _diffuse;
        private double _specular;
        private double _shinniness;

        public Color Color { get { return _color; } set { _color = value; } }
        public double Ambient { get { return _ambient; } set { _ambient = Math.Clamp(value, 0, 1); } }
        public double Diffuse { get { return _diffuse; } set { _diffuse = Math.Clamp(value, 0, 1); } }
        public double Specular { get { return _specular; } set { _specular = Math.Clamp(value, 0, 1); } }
        public double Shinniness { get { return _shinniness; } set { _shinniness = Math.Clamp(value, 10, 200); } }

        public Material(Color color, double ambient = 0.0, double diffuse = 1.0, double specular = 1.0, double shinniness = 100.0)
        {
            this._color = color;
            this._ambient = Math.Clamp(ambient, 0, 1);
            this._diffuse = Math.Clamp(diffuse, 0, 1);
            this._specular = Math.Clamp(specular, 0, 1);
            this._shinniness = Math.Clamp(shinniness, 10, 200);
        }

    }
}
