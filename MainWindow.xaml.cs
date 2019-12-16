﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Path = System.IO.Path;

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

        private void CheckObst(object sender, RoutedEventArgs e) {
            door.IsChecked = false;
            people.IsChecked = false;
        }

        private void CheckDoor(object sender, RoutedEventArgs e) {
            obst.IsChecked = false;
            people.IsChecked = false;
        }

        private void CheckPeople(object sender, RoutedEventArgs e) {
            door.IsChecked = false;
            obst.IsChecked = false;
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
            Console.WriteLine("Application has been closed");
        }

        private bool CheckConvertion(string name) {
            int number;

            bool success = Int32.TryParse(name, out number);
            if (success) {
                Console.WriteLine("Converted correctly", name, number);
                return true;
            } else {
                Console.WriteLine("Attempted conversion of '{0}' failed.",
                                    name ?? "<null>");
                return false;
            }
        }


        private void GenerateFloorField(object sender, RoutedEventArgs e) {
            if (CheckConvertion(rowTb.Text) && CheckConvertion(colTb.Text) == true) {

                MessageBoxResult result = MessageBox.Show("Are you sure you want to generate the floor field?"
                + '\n' + "You will be not allowed to add more stuff to this miserable world you just created.", "Confirm", MessageBoxButton.YesNo);
                switch (result) {
                    case MessageBoxResult.Yes:
                        Cell.GenerateField(cells);

                        obst.IsEnabled = false;
                        door.IsEnabled = false;
                        people.IsEnabled = false;

                        Label floorValueLabel;
                        for (int i = 0; i < cols; i++) {
                            for (int j = 0; j < rows; j++) {

                                floorValueLabel = new Label {
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
                        }

                        Cell.FindHoomans(cells);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }


        }

        private void SaveStaticField(object sender, RoutedEventArgs e) {

            String fileName = DateTime.Now.ToString("h/mm/ss_tt");
            System.Windows.Forms.FolderBrowserDialog openfiledalog = new System.Windows.Forms.FolderBrowserDialog();
            if (openfiledalog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

                var dest = Path.Combine(openfiledalog.SelectedPath, "floor_" + fileName + ".txt");
                using (StreamWriter file = new StreamWriter(dest, false)) {
                    for (int j = 0; j < rows; j++) {
                        for (int i = 0; i < cols; i++) {
                            file.Write(cells[i][j].floorValue.ToString() + '\t');
                        }
                        file.Write("\n");
                    }
                    dest = Path.Combine(openfiledalog.SelectedPath, "hoomans_" + fileName + ".txt");
                    using (StreamWriter file2 = new StreamWriter(dest, false)) {
                        for (int j = 0; j < rows; j++) {
                            for (int i = 0; i < cols; i++) {
                                if (cells[i][j].isAPerson)
                                    file2.Write("1" + '\t');
                                else
                                    file2.Write("0" + '\t');
                            }
                            file2.Write("\n");
                        }
                    }
                }
            }
        }

        private void EvacuateHoomans(object sender, RoutedEventArgs e) {
            List<Cell> listOfHoomans = Cell.FindHoomans(cells);
            Cell evacuateTo;
            foreach (Cell cell in listOfHoomans) {
                evacuateTo = Cell.FindNeighbour(cell, cells);       //znajdujemy pozycje gdzie ma sie ewakuowac
                cells[cell.i][cell.j].isAPerson = false;            //likwidujemy ludzika z miejsca gdzie stal
                cells[evacuateTo.i][evacuateTo.j].isAPerson = true; //i wstawiamy go tam gdzie ma sie ewakuowac
            }

            Random rng = new Random();  //szuffle
            int n = listOfHoomans.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                Cell value = listOfHoomans[k];
                listOfHoomans[k] = listOfHoomans[n];
                listOfHoomans[n] = value;
            }

            for (int i = 1; i < cols - 1; i++) {
                for (int j = 1; j < rows - 1; j++) {
                    if (!cells[i][j].isAWall) {
                        Rectangle rect = new Rectangle();
                        rect.Width = step - 2;
                        rect.Height = step - 2;
                        rect.Fill = Brushes.White;
                        Canvas.SetLeft(rect, i * step + offsetX + 1);
                        Canvas.SetTop(rect, j * step + offsetY + 1);
                        canvas.Children.Add(rect);
                    }

                    Label floorValueLabel;

                    floorValueLabel = new Label {
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

                    if (cells[i][j].isAPerson) {
                        Ellipse ellipse = new Ellipse();
                        ellipse.Width = step - 4;
                        ellipse.Height = step - 4;
                        ellipse.Stroke = Brushes.Black;
                        ellipse.StrokeThickness = 2;

                        Canvas.SetLeft(ellipse, i * step + offsetX + 2);
                        Canvas.SetTop(ellipse, j * step + offsetY + 2);
                        canvas.Children.Add(ellipse);
                    }
                }
            }
        }

        private void Draw(object sender, RoutedEventArgs e) {

            obst.IsEnabled = true;
            door.IsEnabled = true;
            people.IsEnabled = true;

            canvas.Children.Clear();

            if (CheckConvertion(rowTb.Text) && CheckConvertion(colTb.Text) == true) {
                rows = Convert.ToInt32(rowTb.Text);
                cols = Convert.ToInt32(colTb.Text);

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
                        cells[i][j] = new Cell(offsetX + i * step, offsetY + j * step);
                        cells[i][j].i = i;
                        cells[i][j].j = j;
                    }
                }

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

                DrawWalls(); //zrobilem z tego odrebna funkcje bo sie przyda przy wstawianiu drzwi tak zeby byly jedne

            } else {
                MessageBox.Show("Invalid inputs in rows and columns!", "Convertion Error");
            }

        }

        private void DrawWalls() {
            Rectangle rect;
            //wstawianie scian na brzegach
            for (int j = 0; j < rows; j++) {        //lewy i prawy brzeg
                rect = new Rectangle {
                    Width = step - 4,
                    Height = step - 4,
                    Fill = Brushes.DarkGray
                };
                Canvas.SetTop(rect, offsetY + j * step + 2);
                Canvas.SetLeft(rect, offsetX + 2);
                canvas.Children.Add(rect);
                cells[0][j].isADoor = false;
                cells[0][j].isAWall = true;

                rect = new Rectangle {
                    Width = step - 4,
                    Height = step - 4,
                    Fill = Brushes.DarkGray
                };
                Canvas.SetTop(rect, offsetY + j * step + 2);
                Canvas.SetLeft(rect, offsetX + (cols - 1) * step + 2);
                canvas.Children.Add(rect);
                cells[cols - 1][j].isADoor = false;
                cells[cols - 1][j].isAWall = true;
            }


            for (int k = 0; k < cols; k++) {        //gorny i dolny brzeg
                rect = new Rectangle {
                    Width = step - 4,
                    Height = step - 4,
                    Fill = Brushes.DarkGray
                };
                Canvas.SetTop(rect, offsetY + 2);
                Canvas.SetLeft(rect, offsetX + k * step + 2);
                canvas.Children.Add(rect);
                cells[k][0].isADoor = false;
                cells[k][0].isAWall = true;

                rect = new Rectangle {
                    Width = step - 4,
                    Height = step - 4,
                    Fill = Brushes.DarkGray
                };
                Canvas.SetTop(rect, offsetY + (rows - 1) * step + 2);
                Canvas.SetLeft(rect, offsetX + k * step + 2);
                canvas.Children.Add(rect);
                cells[k][rows - 1].isADoor = false;
                cells[k][rows - 1].isAWall = true;
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

                Rectangle rect = new Rectangle();

                if (startPoint.X > offsetX + step && startPoint.X < cols * step + offsetX - step &&
                    startPoint.Y > offsetY + step && startPoint.Y < rows * step + offsetY - step) { //jesli kliknie sie w wewnetrzne kwadraty

                    if (obst.IsChecked == true) {
                        rect.Fill = Brushes.DarkGray;
                        if (cells[c][r].isAWall) {          //jesli juz jest sciana, to czysci sciane
                            cells[c][r].isAWall = false;
                            rect.Width = step - 2;
                            rect.Height = step - 2;
                            rect.Fill = Brushes.White;
                            Canvas.SetLeft(rect, c * step + offsetX + 1);
                            Canvas.SetTop(rect, r * step + offsetY + 1);
                        } else {                            //jesli jeszcze nie jest sciana, to czysci ewentualnego ludzika i wstawia sciane
                            cells[c][r].isAPerson = false;
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
                        if (cells[c][r].isAPerson) {        //jesli jest juz ludzik, to zabija go
                            cells[c][r].isAPerson = false;
                            rect.Width = step - 2;
                            rect.Height = step - 2;
                            rect.Fill = Brushes.White;
                            Canvas.SetLeft(rect, c * step + offsetX + 1);
                            Canvas.SetTop(rect, r * step + offsetY + 1);
                            canvas.Children.Add(rect);
                        } else {                            //jesli jeszcze nie ma ludzika to czysci ewentualna sciane i wstawia ludzika
                            cells[c][r].isAPerson = true;
                            cells[c][r].isAWall = false;
                            ellipse.Width = step - 4;
                            ellipse.Height = step - 4;
                            ellipse.Stroke = Brushes.Black;
                            ellipse.StrokeThickness = 2;
                            rect.Width = step - 2;
                            rect.Height = step - 2;
                            rect.Fill = Brushes.White;
                            Canvas.SetLeft(rect, c * step + offsetX + 1);
                            Canvas.SetTop(rect, r * step + offsetY + 1);
                            canvas.Children.Add(rect);
                            Canvas.SetLeft(ellipse, c * step + offsetX + 2);
                            Canvas.SetTop(ellipse, r * step + offsetY + 2);
                            canvas.Children.Add(ellipse);
                        }
                    }
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
                        rect.Fill = Brushes.Red;
                        if (cells[c][r].isADoor) {          //jesli sa juz drzwi, to wywala drzwi, czysci komorke i wstawia sciane
                            cells[c][r].isADoor = false;
                            cells[c][r].isAWall = true;
                            rect.Width = step - 2;
                            rect.Height = step - 2;
                            rect.Fill = Brushes.White;
                            Canvas.SetLeft(rect, c * step + offsetX + 1);
                            Canvas.SetTop(rect, r * step + offsetY + 1);
                            canvas.Children.Add(rect);
                            rect = new Rectangle {
                                Width = step - 4,
                                Height = step - 4,
                                Fill = Brushes.DarkGray
                            };
                            Canvas.SetLeft(rect, c * step + offsetX + 2);
                            Canvas.SetTop(rect, r * step + offsetY + 2);
                        } else {                            //jesli nie ma jeszcze drzwi, to wywala sciane, czysci komorke i wstawia drzwi
                            DrawWalls();                    //natomiast jesli sa juz drzwi to moga byc tylko jedne
                            cells[c][r].isADoor = true;
                            cells[c][r].isAWall = false;
                            rect.Width = step - 2;
                            rect.Height = step - 2;
                            rect.Fill = Brushes.White;
                            Canvas.SetLeft(rect, c * step + offsetX + 1);
                            Canvas.SetTop(rect, r * step + offsetY + 1);
                            canvas.Children.Add(rect);
                            rect = new Rectangle {
                                Width = step - 4,
                                Height = step - 4,
                                Fill = Brushes.Red
                            };
                            Canvas.SetLeft(rect, c * step + offsetX + 2);
                            Canvas.SetTop(rect, r * step + offsetY + 2);
                        }
                        canvas.Children.Add(rect);
                    }
                }
            }
        }
    }
}