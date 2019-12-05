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


            if (canvas.ActualWidth / cols < canvas.ActualHeight / rows) {
                if (((int)canvas.ActualWidth / cols) % 2 == 0) {
                    step = (int)canvas.ActualWidth / cols;
                } else {
                    step = (int)canvas.ActualWidth / cols - 1;
                }
            } else {
                if (((int)canvas.ActualHeight / rows) % 2 == 0) {
                    step = (int)canvas.ActualHeight / rows;
                } else {
                    step = (int)canvas.ActualHeight / rows - 1;
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
            if (startPoint.X > offsetX && startPoint.X < canvas.Width - offsetX &&
                startPoint.Y > offsetY && startPoint.Y < canvas.Height - offsetY) {
                Rectangle rect = new Rectangle {
                    Fill = Brushes.Black,
                    Height = step,
                    Width = step
                };
                Canvas.SetLeft(rect, (int)startPoint.X / step * step);
                Canvas.SetTop(rect, (int)startPoint.Y / step * step);
                canvas.Children.Add(rect);
            }
        }
    }
}
