using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using RayTracer.RayCast.Lightning;

namespace RayTracer.RayCast
{
    public class Scene : IRayCasted
    {
        private List<IRayCasted> objects = new List<IRayCasted>();
        private List<Light> lights = new List<Light>();

        public Scene()
        {

        }

        public void AddObject(IRayCasted obj)
        {
            if (obj == this)
                throw new Exception("Can't add itself to objects");

            if(objects.Contains(obj))
                throw new Exception("Object already in scene.");

            objects.Add(obj);
        }

        public void AddLight(Light light)
        {
            if (lights.Contains(light))
                throw new Exception("Light already in scene.");

            lights.Add(light);
        }

        public bool RayCast(Ray ray, double distance, out HitInfo hitInfo)
        {
            hitInfo = new HitInfo();
            if (objects.Count == 0)
            {
                return false;
            }

            bool found = false;

            int i = 0;
            for (; i < objects.Count; i++) // look for first intersection
            {
                if(objects[i].RayCast(ray, distance,out hitInfo))
                {
                    found = true;
                    break;
                }
            }


            for (; i < objects.Count; i++) // look for closest intersection
            {
                HitInfo currHitInfo;
                if (objects[i].RayCast(ray, distance, out currHitInfo))
                {
                    if (currHitInfo.Distance < hitInfo.Distance)
                    {
                        hitInfo = currHitInfo;
                    }
                }
            }

            return found;
        }

        public List<HitInfo> RayCastAll(Ray ray, double length)
        {
            var result = new List<HitInfo>();
            if (objects.Count == 0)
                return result;

            for (int i = 0; i < objects.Count; i++)
            {
                result.AddRange(objects[i].RayCastAll(ray, length));
            }

            result.Sort((HitInfo a, HitInfo b) =>
            {
                if (a.Distance == b.Distance)
                    return 0;
                if (a.Distance < b.Distance)
                    return -1;
                return 1;
            });

            return result;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public Bitmap Draw(Camera camera, int width, int height, double maxDistance)
        {
            const double addNormal = 1e-10;

            if (width < 0)
                throw new ArgumentException("Width must be not negative");
            if (height < 0)
                throw new ArgumentException("Height must be not negative");

            Ray[,] rays = camera.GetRays(width, height);

            Bitmap bmp = new Bitmap(width, height);

            List<HitInfo> hits = new List<HitInfo>();

            for (int y = 0; y < rays.GetLength(0); y++)
            {
                for (int x = 0; x < rays.GetLength(1); x++)
                {
                    System.Drawing.Color color;
                    if (RayCast(rays[y,x], maxDistance, out HitInfo hitInfo))
                    {
                        hits.Add(hitInfo);
                        color = hitInfo.Object3D.material.Color;
                        if (lights.Count > 0)
                        {
                            double light = lights[0].GetLight(hitInfo.Point+hitInfo.Normal * addNormal, hitInfo.Normal, this);
                            color = ColorFromHSV(color.GetHue(), color.GetSaturation(), light);
                        }
                        
                    }
                    else
                    {
                        color = System.Drawing.Color.White;
                    }
                    bmp.SetPixel(x, y, color);

                }
            }

            return bmp;
        }
    }
}
