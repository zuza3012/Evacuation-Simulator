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
        Cell[][] cells;

        private void checkObst(object sender, RoutedEventArgs e) {
            door.IsChecked = false;
            people.IsChecked = false;
        }

        private void checkDoor(object sender, RoutedEventArgs e) {
            obst.IsChecked = false;
            people.IsChecked = false;
        }

        private void checkPeople(object sender, RoutedEventArgs e) {
            door.IsChecked = false;
            obst.IsChecked = false;
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
            Console.WriteLine("Application has been closed");
        }

        private void Draw(object sender, RoutedEventArgs e) {
            canvas.Children.Clear();

            rows = Convert.ToInt32(rowTb.Text);
            cols = Convert.ToInt32(colTb.Text);


            if (canvas.Width / cols < canvas.Height / rows) {
                step = (int)canvas.Width / cols;
            } else {
                step = (int)canvas.Height / rows;
            }
            Console.WriteLine("Step: " + step);
            offsetX = (canvas.Width - step * cols) / 2;
            offsetY = (canvas.Height - step * rows) / 2;

            cells = new Cell[cols][];

            for (int i = 0; i < cols; i++) {
                cells[i] = new Cell[rows];
                for (int j = 0; j < rows; j++) {
                    cells[i][j] = new Cell(offsetX + i * step, offsetY + j * step);
                }
            }

            for (int i = 0; i < cols + 1; i++) {
                Line lineX = new Line {
                    Stroke = Brushes.Black,

                    X1 = i * step + offsetX,
                    Y1 = 0 + offsetY,

                    X2 = i * step + offsetX,
                    Y2 = rows * step + offsetY
                };
                Console.WriteLine("(" + lineX.X1 + "," + lineX.Y1 + ")");
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

            Point converted = new Point {
                X = startPoint.X - offsetX,
                Y = startPoint.Y - offsetY
            };
            int c, r;

            c = (int)converted.X / step;
            r = (int)converted.Y / step;

            Rectangle rect = new Rectangle();

            if (startPoint.X > offsetX + step && startPoint.X < cols * step + offsetX - step &&
                startPoint.Y > offsetY + step && startPoint.Y < rows * step + offsetY - step) {

                if (obst.IsChecked == true) {
                    rect.Fill = Brushes.DarkGray;
                    if (cells[c][r].isAWall) {
                        cells[c][r].isAWall = false;
                        rect.Width = step - 2;
                        rect.Height = step - 2;
                        rect.Fill = Brushes.White;
                        Canvas.SetLeft(rect, c * step + offsetX + 1);
                        Canvas.SetTop(rect, r * step + offsetY + 1);
                    } else {
                        cells[c][r].isAWall = true;
                        rect.Width = step - 4;
                        rect.Height = step - 4;
                        rect.Fill = Brushes.DarkGray;
                        Canvas.SetLeft(rect, c * step + offsetX + 2);
                        Canvas.SetTop(rect, r * step + offsetY + 2);
                    }
                    canvas.Children.Add(rect);

                } else if (people.IsChecked == true) {
                    Ellipse ellipse = new Ellipse();
                    if (cells[c][r].isAPerson) {
                        cells[c][r].isAPerson = false;
                        ellipse.Width = step - 2;
                        ellipse.Height = step - 2;
                        ellipse.Stroke = Brushes.White;
                        ellipse.StrokeThickness = 4;
                        Canvas.SetLeft(ellipse, c * step + offsetX + 1);
                        Canvas.SetTop(ellipse, r * step + offsetY + 1);
                    } else {
                        cells[c][r].isAPerson = true;
                        ellipse.Width = step - 4;
                        ellipse.Height = step - 4;
                        ellipse.Stroke = Brushes.Black;
                        ellipse.StrokeThickness = 2;
                        Canvas.SetLeft(ellipse, c * step + offsetX + 2);
                        Canvas.SetTop(ellipse, r * step + offsetY + 2);
                    }

                    canvas.Children.Add(ellipse);
                }
            }
            if ((startPoint.X > offsetX && startPoint.X < offsetX + cols * step && startPoint.Y > offsetY && startPoint.Y < offsetY + cols * step) 
                && !(startPoint.X > offsetX + step && startPoint.X < cols * step + offsetX - step &&
                startPoint.Y > offsetY + step && startPoint.Y < rows * step + offsetY - step)) {

                if (door.IsChecked == true) {
                    rect.Fill = Brushes.Red;
                    if (cells[c][r].isADoor) {
                        cells[c][r].isADoor = false;
                        rect.Width = step - 4;
                        rect.Height = step - 4;
                        rect.Fill = Brushes.DarkGray;
                        Canvas.SetLeft(rect, c * step + offsetX + 2);
                        Canvas.SetTop(rect, r * step + offsetY + 2);
                    } else {
                        cells[c][r].isADoor = true;
                        rect.Width = step - 4;
                        rect.Height = step - 4;
                        rect.Fill = Brushes.Red;
                        Canvas.SetLeft(rect, c * step + offsetX + 2);
                        Canvas.SetTop(rect, r * step + offsetY + 2);
                    }
                    canvas.Children.Add(rect);
                }
            }
        }
    }
}

