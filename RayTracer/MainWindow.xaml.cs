using RayTracer.Objects;
using RayTracer.RayCast;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
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

            Sphere sphere = new Sphere(Vector3.Zero, 1);
            Ray ray = new Ray(Vector3.Backward * 0 + Vector3.Up*0, Vector3.Forward);
            var hits = sphere.RayCastAll(ray, 1);

            Vector3 up = Vector3.Up;
            Vector3 forward = Vector3.Forward;

            Vector3 right = Quaternion.AngleAxis(90, up) * forward;

            Vector3 q = new Vector3(-1, 1, 0);

            Vector3 x = Quaternion.AngleAxis(-45, up) * Quaternion.AngleAxis(-45, right) * forward;

            hits = hits;
        }
    }
}
