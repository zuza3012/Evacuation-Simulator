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
                //      if (((int)canvas.ActualWidth / cols) % 2 == 0) {
                step = (int)canvas.ActualWidth / cols;
                //     } else {
                //        step = (int)canvas.ActualWidth / cols - 1;
                //   }
            } else {
                //   if (((int)canvas.ActualHeight / rows) % 2 == 0) {
                step = (int)canvas.ActualHeight / rows;
                //   } else {
                //       step = (int)canvas.ActualHeight / rows - 1;
                //   }
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

            Point clickPoint = e.GetPosition(canvas);
            if (clickPoint.X > offsetX && clickPoint.X < canvas.Width - offsetX &&
                clickPoint.Y > offsetY && clickPoint.Y < canvas.Height - offsetY) {
                Rectangle rect = new Rectangle {
                    Fill = Brushes.DarkGray,
                    Height = step,
                    Width = step
                };
                //Console.WriteLine("("+((int)startPoint.X / step * step + offsetX)+","+ ((int)startPoint.Y / step * step + offsetY)+")");
                Canvas.SetLeft(rect, (int)(clickPoint.X) / step * step + offsetX % step);
                Canvas.SetTop(rect, (int)(clickPoint.Y) / step * step + offsetY % step);
                canvas.Children.Add(rect);
            }
        }
    }
}
