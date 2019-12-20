using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        double offsetX, offsetY, panicParameter = 0;
        Cell[][] cells;
        String buffer;

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

        private void SaveSimulatedData(object sender, RoutedEventArgs e) {

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
                        file2.Write(buffer);
                    }

                    dest = Path.Combine(openfiledalog.SelectedPath, "density_" + fileName + ".txt");
                    using (StreamWriter file3 = new StreamWriter(dest, false)) {
                        for (int j = 0; j < rows; j++) {
                            for (int i = 0; i < cols; i++) {
                                file3.Write(cells[i][j].howManyHoomansWereThere.ToString() + '\t');
                            }
                            file3.Write("\n");
                        }
                    }
                }
            }
        }


        private BackgroundWorker evacuationWorker = null;

        void evacuationWorker_DoWork(object sender, DoWorkEventArgs e) {
            int counter = 0;
            buffer += "Time:" + counter.ToString() + '\n';
            for (int j = 0; j < rows; j++) {
                for (int i = 0; i < cols; i++) {
                    if (cells[i][j].isAWall)
                        buffer += "#" + '\t';
                    else if (cells[i][j].isAPerson)
                        buffer += "1" + '\t';
                    else if (cells[i][j].isADoor)
                        buffer += "D" + '\t';
                    else
                        buffer += "0" + '\t';
                }
                buffer += '\n';
            }
            while (Cell.FindHoomans(cells).Count != 0) {
                counter++;
                buffer += "Time:" + counter.ToString() + '\n';

                List<Cell> listOfHoomans = Cell.FindHoomans(cells);
                Cell evacuateTo;
                foreach (Cell cell in listOfHoomans) {
                    cell.howManyHoomansWereThere += 1;
                    evacuateTo = Cell.FindNeighbour(cell, cells, panicParameter);       //znajdujemy pozycje gdzie ma sie ewakuowac
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

                for (int j = 0; j < rows; j++) {
                    for (int i = 0; i < cols; i++) {
                        if (cells[i][j].isAWall)
                            buffer += "#" + '\t';
                        else if (cells[i][j].isAPerson)
                            buffer += "1" + '\t';
                        else if (cells[i][j].isADoor)
                            buffer += "D" + '\t';
                        else
                            buffer += "0" + '\t';
                    }
                    buffer += '\n';
                }
                
                evacuationWorker.ReportProgress(1);
                System.Threading.Thread.Sleep(500);
            }
            System.Threading.Thread.Sleep(300);
            evacuationWorker.ReportProgress(100);
        }

        void evacuationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            for (int i = 1; i < cols - 1; i++) {
                for (int j = 1; j < rows - 1; j++) {
                    DrawSomething(i, j);
                    DrawLabel(i, j);
                }
            }
        }

        void evacuationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            MessageBox.Show("Evacuation completed succesfully.", "You are the real hero!");
        }

        private void EvacuateHoomans(object sender, RoutedEventArgs e) {
            if (null == evacuationWorker) {
                evacuationWorker = new BackgroundWorker();
                evacuationWorker.DoWork += new DoWorkEventHandler(evacuationWorker_DoWork);
                evacuationWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(evacuationWorker_RunWorkerCompleted);
                evacuationWorker.ProgressChanged += new ProgressChangedEventHandler(evacuationWorker_ProgressChanged);
                evacuationWorker.WorkerReportsProgress = true;
                evacuationWorker.WorkerSupportsCancellation = true;
            }
            evacuationWorker.RunWorkerAsync();
            saveItem.IsEnabled = true;
        }

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

        private void ParametersDialog(object sender, RoutedEventArgs e) {
            var dialog = new ParameterWindow();
            if(dialog.ShowDialog() == true) {
                panicParameter = Double.Parse(dialog.panicParameterTB.Text);
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
                        cells[i][j] = new Cell(offsetX + i * step, offsetY + j * step) {
                            i = i,
                            j = j
                        };
                    }
                }

                DrawLines();
                DrawWalls(); //zrobilem z tego odrebna funkcje bo sie przyda przy wstawianiu drzwi tak zeby byly jedne

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
                            DrawWalls();                    //natomiast jesli sa juz drzwi to moga byc tylko jedne
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