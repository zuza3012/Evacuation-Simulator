using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ha {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        int rows, cols, step;
        double offsetX, offsetY;

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
            Console.WriteLine("Application has been closed");
        }

        private void Draw(object sender, RoutedEventArgs e) {
            canvas.Children.Clear();

            rows = Convert.ToInt32(rowTb.Text);
            cols = Convert.ToInt32(colTb.Text);


            if (canvas.Width / cols < canvas.ActualHeight / rows) {
                if ((int)canvas.Width / cols % 2 == 0) {
                    step = (int)canvas.Width / cols;
                } else {
                    step = (int)canvas.Width / cols - 1;
                }
            } else {
                if ((int)canvas.Height / rows % 2 == 0) {
                    step = (int)canvas.Height / rows;
                } else {
                    step = (int)canvas.Height / rows - 1;
                }
            }

            offsetX = (canvas.Width - step * cols) / 2;
            offsetY = (canvas.Height - step * rows) / 2;

            for (int i = 0; i < cols + 1; i++) {
                Line lineX = new Line {
                    Stroke = Brushes.Black,

                    X1 = i * step + offsetX,
                    Y1 = 0 + offsetY,
                    
                    X2 = i * step + offsetX,
                    Y2 = rows * step + offsetY
                };
                Console.WriteLine("("+lineX.X1+","+lineX.Y1+")");
                canvas.Children.Add(lineX);

            }

            for (int j = 0; j < rows + 1; j++) {
                Line lineY = new Line {
                    Stroke = Brushes.Black,

                    X1 = 0 + offsetX,
                    Y1 = j * step + offsetY,

                    X2 = cols * step + offsetX,
                    Y2 = j * step + offsetY
                };

                canvas.Children.Add(lineY);

            }
        }

        private void DrawRect(object sender, MouseButtonEventArgs e) {

            Point startPoint = e.GetPosition(canvas);
            if (startPoint.X > offsetX && startPoint.X < cols * step + offsetX &&
                startPoint.Y > offsetY && startPoint.Y < rows * step + offsetY) {

                Console.WriteLine((int)(startPoint.X) / step * step+offsetX);
                Console.WriteLine((int)(startPoint.Y) / step * step+offsetY);

                Rectangle rect = new Rectangle {
                    Fill = Brushes.Black,
                    Height = step,
                    Width = step
                };
                Canvas.SetLeft(rect, (int)(startPoint.X) / step * step+offsetX);
                Canvas.SetTop(rect, (int)(startPoint.Y) / step * step+offsetY);
                canvas.Children.Add(rect);
            }
        }
    }
}
