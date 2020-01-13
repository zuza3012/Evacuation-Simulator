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

        private void DrawLines() {
            for (int i = 0; i < cols + 1; i++) {    //rysowanie linii pionowych
                Line lineX = new Line {
                    Stroke = Brushes.Black,

                    X1 = i * step + offsetX,
                    Y1 = 0 + offsetY,

                    X2 = i * step + offsetX,
                    Y2 = rows * step + offsetY
                };
                canvas.Children.Add(lineX);
            }

            for (int j = 0; j < rows + 1; j++) {    //rysowanie linii poziomych
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

        private void DrawSomething(int i, int j) {

            Rectangle rect = new Rectangle {
                Width = step - 2,
                Height = step - 2,
                Fill = Brushes.White
            };                                                      //czyszczenie
            Canvas.SetLeft(rect, i * step + offsetX + 1);
            Canvas.SetTop(rect, j * step + offsetY + 1);
            canvas.Children.Add(rect);

            if (cells[i][j].isAWall) {                              //sciana
                rect = new Rectangle {
                    Width = step - 4,
                    Height = step - 4,
                    Fill = Brushes.DarkGray
                };

                Canvas.SetLeft(rect, i * step + offsetX + 2);
                Canvas.SetTop(rect, j * step + offsetY + 2);
                canvas.Children.Add(rect);

            } else if (cells[i][j].isAPerson) {                      //ludź
                Ellipse ellipse = new Ellipse {
                    Width = step - 4,
                    Height = step - 4,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                Canvas.SetLeft(ellipse, i * step + offsetX + 2);
                Canvas.SetTop(ellipse, j * step + offsetY + 2);
                canvas.Children.Add(ellipse);

            } else if (cells[i][j].isADoor) {                       //drzwi
                rect = new Rectangle {
                    Width = step - 4,
                    Height = step - 4,
                    Fill = Brushes.Red
                };

                Canvas.SetLeft(rect, i * step + offsetX + 2);
                Canvas.SetTop(rect, j * step + offsetY + 2);
                canvas.Children.Add(rect);
            }
        }

        private void DrawLabel(int i, int j) {
            Label floorValueLabel;

            floorValueLabel = new Label {                           //label z wartoscia pola
                Content = cells[i][j].floorValue,
                Width = step,
                Height = step
            };

            if (step <= 100) {
                floorValueLabel.FontSize = step * 3 / 12;
            } else {
                floorValueLabel.FontSize = step * 3 / 16;
            }
            Canvas.SetLeft(floorValueLabel, cells[i][j].x);
            Canvas.SetTop(floorValueLabel, cells[i][j].y);
            canvas.Children.Add(floorValueLabel);
        }

        private void Draw(object sender, RoutedEventArgs e) {

            obst.IsEnabled = true;
            door.IsEnabled = true;
            people.IsEnabled = true;
            calculate.IsEnabled = true;
            evacuateHoomansBtn.IsEnabled = false;
            evacuateHoomansNTimesBtn.IsEnabled = false;
            multiplePanicParametersButton.IsEnabled = false;
            canvas.Children.Clear();

            if (CheckConvertion(rowTb.Text) && CheckConvertion(colTb.Text) == true) {
                rows = Convert.ToInt32(rowTb.Text);
                cols = Convert.ToInt32(colTb.Text);

                if (rows < 3 || cols < 3) {
                    MessageBox.Show("Number of columns or rows can't be less than 3.", "Fatal error");
                } else {
                    if (canvas.Width / cols < canvas.Height / rows) { //nadawanie wartosci stepu
                        step = (int)canvas.Width / cols;
                    } else {
                        step = (int)canvas.Height / rows;
                    }

                    offsetX = (canvas.Width - step * cols) / 2;       //nadawanie wartosci offsetom
                    offsetY = (canvas.Height - step * rows) / 2;

                    cells = new Cell[cols][];

                    for (int i = 0; i < cols; i++) {        //nadawanie komorkom wartosci x i y
                        cells[i] = new Cell[rows];
                        for (int j = 0; j < rows; j++) {
                            cells[i][j] = new Cell(offsetX + i * step, offsetY + j * step) {
                                i = i,
                                j = j
                            };
                        }
                    }

                    DrawLines();
                    DrawWalls(); //zrobilem z tego odrebna funkcje bo sie przyda przy wstawianiu drzwi tak zeby byly jedne
                }
            } else {
                MessageBox.Show("Invalid inputs in rows and columns!", "Convertion Error");
            }
        }

        private void DrawWalls() {                  //wstawianie scian na brzegach
            for (int j = 0; j < rows; j++) {        //lewy i prawy brzeg
                cells[0][j].isAWall = true;
                cells[0][j].isADoor = false;
                cells[0][j].isAWall = true;

                cells[cols - 1][j].isAWall = true;
                cells[cols - 1][j].isADoor = false;
                cells[cols - 1][j].isAWall = true;

                DrawSomething(0, j);
                DrawSomething(cols - 1, j);
            }

            for (int k = 0; k < cols; k++) {        //gorny i dolny brzeg
                cells[k][0].isAWall = true;
                cells[k][0].isADoor = false;
                cells[k][0].isAWall = true;

                cells[k][rows - 1].isAWall = true;
                cells[k][rows - 1].isADoor = false;
                cells[k][rows - 1].isAWall = true;

                DrawSomething(k, 0);
                DrawSomething(k, rows - 1);
            }
        }

        private void DrawRect(object sender, MouseButtonEventArgs e) { //rysowanie obiektu w kwadraciku na ktory sie kliknie
            if (CheckConvertion(rowTb.Text) && CheckConvertion(colTb.Text) == true) {
                Point startPoint = e.GetPosition(canvas);

                Point converted = new Point {
                    X = startPoint.X - offsetX,
                    Y = startPoint.Y - offsetY
                };
                int c, r;

                c = (int)converted.X / step;
                r = (int)converted.Y / step;

                if (startPoint.X > offsetX + step && startPoint.X < cols * step + offsetX - step &&
                    startPoint.Y > offsetY + step && startPoint.Y < rows * step + offsetY - step) { //jesli kliknie sie w wewnetrzne kwadraty

                    if (obst.IsChecked == true) {
                        if (cells[c][r].isAWall) {          //jesli juz jest sciana, to czysci sciane
                            cells[c][r].isAWall = false;
                        } else {                            //jesli jeszcze nie jest sciana, to czysci ewentualnego ludzika i wstawia sciane
                            cells[c][r].isAPerson = false;
                            cells[c][r].isAWall = true;
                        }

                    } else if (people.IsChecked == true) {
                        if (cells[c][r].isAPerson) {        //jesli jest juz ludzik, to zabija go (brutal)
                            cells[c][r].isAPerson = false;
                        } else {                            //jesli jeszcze nie ma ludzika to czysci ewentualna sciane i wstawia ludzika
                            cells[c][r].isAPerson = true;
                            cells[c][r].isAWall = false;
                        }
                    }
                    DrawSomething(c, r);
                }

                if ((startPoint.X > offsetX && startPoint.X < offsetX + cols * step && startPoint.Y > offsetY && startPoint.Y < offsetY + rows * step)
                    && !(startPoint.X > offsetX + step && startPoint.X < cols * step + offsetX - step &&
                    startPoint.Y > offsetY + step && startPoint.Y < rows * step + offsetY - step)
                    && !((startPoint.X < offsetX + step) && (startPoint.Y < offsetY + step))
                    && !((startPoint.X > offsetX + (cols - 1) * step) && (startPoint.Y < offsetY + step))
                    && !((startPoint.X < offsetX + step) && (startPoint.Y > offsetY + (rows - 1) * step))
                    && !((startPoint.X > offsetX + (cols - 1) * step) && (startPoint.Y > offsetY + (rows - 1) * step))) {
                    //jesli kliknie sie w zewnetrzne kwadraty (bez rogow)

                    if (door.IsChecked == true) {
                        if (cells[c][r].isADoor) {          //jesli sa juz drzwi, to wywala drzwi, czysci komorke i wstawia sciane
                            cells[c][r].isADoor = false;
                            cells[c][r].isAWall = true;

                        } else {                            //jesli nie ma jeszcze drzwi, to wywala sciane, czysci komorke i wstawia drzwi
                                                            //DrawWalls();                    //natomiast jesli sa juz drzwi to moga byc tylko jedne
                            cells[c][r].isADoor = true;
                            cells[c][r].isAWall = false;
                        }
                        DrawSomething(c, r);
                    }
                }
            }
        }
    }
}
