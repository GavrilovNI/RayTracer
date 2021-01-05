using RayTracer.Objects;
using RayTracer.RayCast;
using RayTracer.RayCast.Lightning;
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


            Vector3 a = new Vector3(0, 0, 1);
            Vector3 b = new Vector3(0, 1, -0.1);

            double x = Vector3.Dot(a, b);

            Scene scene = new Scene();

            Material red = new Material(System.Drawing.Color.Red);
            Material green = new Material(System.Drawing.Color.Green);
            Material blue = new Material(System.Drawing.Color.Blue);
            scene.AddObject(new Sphere(new Vector3(0, 0, 3), red, 1));
            scene.AddObject(new Sphere(new Vector3(1.5, 1, 4), green, 0.7));
            scene.AddObject(new InfinityPlane(new Vector3(0,-1,0), blue));

            scene.AddLight(new PointLight(5, new Vector3(2, 1, 0)));
            scene.AddLight(new PointLight(5, new Vector3(-2, 1, 0)));

            Camera camera = new Camera(new Ray(new Vector3(0, 0, 0), Vector3.Forward), 60, Vector3.Up);

            int width = 1600;
            int height = 900;


            Bitmap bmp = scene.Draw(camera, width, height, 100);

            image.Source = Imaging.Utils.Bitmap2Source(bmp);
        }
    }
}
