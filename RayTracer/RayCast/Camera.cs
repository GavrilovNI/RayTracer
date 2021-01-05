using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.RayCast
{
    public class Camera
    {
        private double _fov;

        public Ray ray;
        public double FOV { get { return _fov; } set{ _fov = Math.Max(0, value); } }
        public Vector3 up;

        public Camera(Ray ray, double fov, Vector3 up)
        {
            this.ray = ray;
            this.FOV = fov;
            this.up = up;
        }

        public Ray[,] GetRays(int width, int height)
        {
            Ray[,] result = new Ray[height, width];

            double ratio = 1.0f * width / height;
            double tan = Math.Tan(FOV / 2 * MathUtils.Deg2Rad); 
            double one = tan * 2 / height;

            Vector3 right = Quaternion.AngleAxis(90, up) * ray.Direction;

            Vector3 dirTopLeft = ray.Direction + (-right) * (tan * ratio - one / 2) + up * (tan - one / 2);


            Vector3 oneRight = right * one;
            Vector3 oneDown = -up * one;

            for (int y = 0; y < height; y++)
            {
                Vector3 currDir = dirTopLeft;
                for (int x = 0; x < width; x++)
                {
                    result[y, x] = new Ray(ray.Origin, currDir);
                    currDir += oneRight;
                }
                dirTopLeft += oneDown;
            }

            return result;
        }
    }
}
