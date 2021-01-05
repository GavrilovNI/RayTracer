using RayTracer.Objects;
using RayTracer.RayCast;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RayTracer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            Camera camera = new Camera(new Ray(Vector3.Zero, Vector3.Forward), 60, Vector3.Up);
            Sphere sphere = new Sphere(Vector3.Forward*3, 1);

            int width = 160;
            int height = 90;

            Ray[,] rays = camera.GetRays(width, height);

            Bitmap bmp = new Bitmap(width, height);

            for (int y = 0; y < rays.GetLength(0); y++)
            {
                for (int x = 0; x < rays.GetLength(1); x++)
                {
                    if(sphere.RayCastAll(rays[y, x], 100).Count>0)
                    {
                        bmp.SetPixel(x, y, System.Drawing.Color.Red);
                    }
                    else
                    {
                        bmp.SetPixel(x, y, System.Drawing.Color.White);
                    }
                }
            }

            image.Source = Imaging.Utils.Bitmap2Source(bmp);
        }
    }
}
