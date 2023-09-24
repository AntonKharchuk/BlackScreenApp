using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BlackScreenApp
{
    public partial class Window2 : Window
    {

        public Window2()
        {
            InitializeComponent();
        }

        private async void Canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {           
            var  mousePosition = e.GetPosition(canvas);
            DrawCircle(mousePosition);
        }

        private async void DrawCircle(Point mousePosition)
        {
            List<Ellipse> dropCircles = new List<Ellipse>();

            double diametr = 40;
            double addPixelsToCicle = 100;
            int awaitMiliseconds = 200;
            double startThickness = 8;
            int circleLifeTime = awaitMiliseconds * 4;

            double centerX = mousePosition.X;
            double centerY = mousePosition.Y;

            while (diametr < canvas.ActualWidth)
            {
                Ellipse circle = new Ellipse()
                {
                    Width = diametr,
                    Height = diametr,
                    Stroke = Brushes.White,
                    StrokeThickness = startThickness/=2,
                };
                // Calculate circle size
                double radius = diametr / 2;

                // Set the circle's position and size
                Canvas.SetLeft(circle, centerX - radius);
                Canvas.SetTop(circle, centerY - radius);

                canvas.Children.Add(circle);
                dropCircles.Add(circle);    

                diametr += addPixelsToCicle;
                AutoDestroyCircle(circle);

                await Task.Delay(awaitMiliseconds);
            }
            async void AutoDestroyCircle(Ellipse circle)
            {
                await Task.Delay(circleLifeTime);
                canvas.Children.Remove(circle);
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Escape key (Key.Escape) was pressed
            if (e.Key == Key.Escape)
            {
                // Close the window
                Close();
            }
        }
    }
}
