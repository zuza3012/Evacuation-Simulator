using System.Collections.Generic;

namespace Ha {
    class Cell {
        public double x, y, floorValue = 666;
        public bool isAWall = false, isAPerson = false, isADoor = false;
        public int i, j;
        public int howManyHoomansWereThere = 0;
        public Cell(double x, double y) {
            this.x = x;
            this.y = y;
        }

        public Cell() { }

        internal static List<Cell> FindHoomans(Cell[][] cells) {
            List<Cell> listOfHoomans = new List<Cell>();
            for(int j = 0; j < cells[0].Length; j++){
                for(int i = 0; i < cells.Length; i++) {
                    if (cells[i][j].isAPerson && !(cells[i][j].floorValue == 0)) {
                        listOfHoomans.Add(cells[i][j]);
                        //System.Console.WriteLine("(" + cells[i][j].i + ", " + cells[i][j].j + ")");
                    }
                }
            }
            return listOfHoomans;
        }


        internal static Cell FindNeighbour(Cell cell, Cell[][] cells, double panicParameter) {
            System.Random r = new System.Random();
            double panic = r.NextDouble();

            if (panic > panicParameter) {
                double minimumFloorValue = 666;

                List<Cell> listOfNeighbours = new List<Cell> {
                cells[cell.i - 1][cell.j],                  //sasiad z lewej
                cells[cell.i + 1][cell.j],                  //sasiad z prawej
                cells[cell.i][cell.j - 1],                  //sasiad z gory
                cells[cell.i][cell.j + 1],                  //sasiad z dolu
                cells[cell.i - 1][cell.j - 1],              //sasiad z lewej gory
                cells[cell.i + 1][cell.j - 1],              //sasiad z prawej gory
                cells[cell.i - 1][cell.j + 1],              //sasiad z lewego dolu
                cells[cell.i + 1][cell.j + 1]               //sasiad z prawego dolu
            };

                List<Cell> listOfNearestNeighbours = new List<Cell>();
                foreach (Cell neighbour in listOfNeighbours) {
                    if (neighbour.isADoor) {
                        cell.isAPerson = false;
                        return neighbour;
                    }

                    if (neighbour.floorValue < cell.floorValue && !neighbour.isAPerson && neighbour.floorValue < minimumFloorValue) {//szukamy najmniejszej wartosci pola w ogole
                        minimumFloorValue = neighbour.floorValue;
                        System.Console.WriteLine(minimumFloorValue.ToString() + "kupa");
                    }
                }
                System.Console.WriteLine(minimumFloorValue.ToString());
                foreach (Cell neighbour in listOfNeighbours) {                          //szukamy pola lub pol ktore maja najmniejsza wartosc i nie sa ludziami
                    if (neighbour.floorValue == minimumFloorValue) {
                        listOfNearestNeighbours.Add(neighbour);
                    }
                }
                if (listOfNearestNeighbours.Count == 0)
                    return cell;
                else {
                    System.Random rand = new System.Random();
                    int randomNeighbourIndex = rand.Next(listOfNearestNeighbours.Count); //losujemy komorke jesli jest ich wiecej niz 1 (w praktyce wiecej niz 0)
                    System.Console.WriteLine("(" + cell.i + ", " + cell.j + ") - " +
                        "(" + listOfNearestNeighbours[randomNeighbourIndex].i + ", " + listOfNearestNeighbours[randomNeighbourIndex].j + ")");

                    return listOfNearestNeighbours[randomNeighbourIndex];
                }
            } else {
                return cell;
            }
        }

        internal static void CheckCells(int i, int j, Cell[][] cells) {

            if (!cells[i - 1][j].isAWall && !cells[i - 1][j].isADoor) {         //sasiad z lewej
                if (cells[i][j].floorValue + 1 < cells[i - 1][j].floorValue)
                    cells[i - 1][j].floorValue = cells[i][j].floorValue + 1;
            } else {
                cells[i - 1][j].floorValue = 500;
            }

            if (!cells[i + 1][j].isAWall && !cells[i + 1][j].isADoor) {         //sasiad z prawej
                if (cells[i][j].floorValue + 1 < cells[i + 1][j].floorValue)
                    cells[i + 1][j].floorValue = cells[i][j].floorValue + 1;
            } else {
                cells[i + 1][j].floorValue = 500;
            }

            if (!cells[i][j - 1].isAWall && !cells[i][j - 1].isADoor) {         //sasiad z gory
                if (cells[i][j].floorValue + 1 < cells[i][j - 1].floorValue)
                    cells[i][j - 1].floorValue = cells[i][j].floorValue + 1;
            } else {
                cells[i][j - 1].floorValue = 500;
            }

            if (!cells[i - 1][j - 1].isAWall && !cells[i - 1][j - 1].isADoor) { //sasiad z lewej gory
                if (cells[i][j].floorValue + 1.5 < cells[i - 1][j - 1].floorValue)
                    cells[i - 1][j - 1].floorValue = cells[i][j].floorValue + 1.5;
            } else {
                cells[i - 1][j - 1].floorValue = 500;
            }

            if (!cells[i + 1][j - 1].isAWall && !cells[i + 1][j - 1].isADoor) { //sasiad z prawej gory
                if (cells[i][j].floorValue + 1.5 < cells[i + 1][j - 1].floorValue)
                    cells[i + 1][j - 1].floorValue = cells[i][j].floorValue + 1.5;
            } else {
                cells[i + 1][j - 1].floorValue = 500;
            }

            if (!cells[i + 1][j + 1].isAWall && !cells[i + 1][j + 1].isADoor) { //sasiad z prawego dolu
                if (cells[i][j].floorValue + 1.5 < cells[i + 1][j + 1].floorValue)
                    cells[i + 1][j + 1].floorValue = cells[i][j].floorValue + 1.5;
            } else {
                cells[i + 1][j + 1].floorValue = 500;
            }

            if (!cells[i - 1][j + 1].isAWall && !cells[i - 1][j + 1].isADoor) { //sasiad z lewego dolu
                if (cells[i][j].floorValue + 1.5 < cells[i - 1][j + 1].floorValue)
                    cells[i - 1][j + 1].floorValue = cells[i][j].floorValue + 1.5;
            } else {
                cells[i - 1][j + 1].floorValue = 500;
            }

            if (!cells[i][j + 1].isAWall && !cells[i][j + 1].isADoor) {         //sasiad z dolu
                if (cells[i][j].floorValue + 1 < cells[i][j + 1].floorValue)
                    cells[i][j + 1].floorValue = cells[i][j].floorValue + 1;
            } else {
                cells[i][j + 1].floorValue = 500;
            }
        }

        internal static void GenerateField(Cell[][] cells) {

            int cols = cells.Length;
            int rows = cells[0].Length;
            int iD = 0, jD = 0;              //wspolrzedne drzwi w tablicy cells[][]
            for (int i = 0; i < cols; i++) { //szukamy drzwi na gornej i dolnej krawedzi i jednoczesnie ustawiamy wartosc scian
                cells[i][0].floorValue = 500;
                cells[i][rows - 1].floorValue = 500;
                if (cells[i][0].isADoor) {
                    cells[i][0].floorValue = 0;
                    iD = i;
                    jD = 0;
                } else if (cells[i][rows - 1].isADoor) {
                    cells[i][rows - 1].floorValue = 0;
                    iD = i;
                    jD = rows - 1;
                }
            }

            for (int j = 0; j < rows; j++) { //szukamy drzwi na lewej i prawej krawedzi i jednoczesnie ustawiamy wartosc scian
                cells[0][j].floorValue = 500;
                cells[cols - 1][j].floorValue = 500;
                if (cells[0][j].isADoor) {
                    cells[0][j].floorValue = 0;
                    iD = 0;
                    jD = j;
                } else if (cells[cols - 1][j].isADoor) {
                    cells[cols - 1][j].floorValue = 0;
                    iD = cols - 1;
                    jD = j;
                }
            }

            if (iD == 0 && jD == 0) {
                System.Windows.MessageBox.Show("You didn't put any door here! We are going to die!", "Where is the door?");
            } else {
                List<Cell> listOfCells = new List<Cell>();
                //tu bendzie jagiź algorydm
                if (jD == rows - 1) {                       //jesli drzwi na dole
                    cells[iD][jD - 1].floorValue = 1;
                    listOfCells.Add(cells[iD][jD - 1]);

                    if (!cells[iD - 1][jD - 1].isAWall) {
                        cells[iD - 1][jD - 1].floorValue = 1.5;
                        listOfCells.Add(cells[iD - 1][jD - 1]);
                    }

                    if (!cells[iD + 1][jD - 1].isAWall) {
                        cells[iD + 1][jD - 1].floorValue = 1.5;
                        listOfCells.Add(cells[iD + 1][jD - 1]);
                    }

                } else if (jD == 0) {                       //jesli drzwi na gorze
                    cells[iD][1].floorValue = 1;
                    listOfCells.Add(cells[iD][1]);

                    if (!cells[iD - 1][1].isAWall) {
                        cells[iD - 1][1].floorValue = 1.5;
                        listOfCells.Add(cells[iD - 1][1]);
                    }
                    if (!cells[iD + 1][1].isAWall) {
                        cells[iD + 1][1].floorValue = 1.5;
                        listOfCells.Add(cells[iD + 1][1]);
                    }
                } else if (iD == 0) {                       //jesli drzwi po lewej 
                    cells[1][jD].floorValue = 1;
                    listOfCells.Add(cells[1][jD]);

                    if (!cells[1][jD + 1].isAWall) {
                        cells[1][jD + 1].floorValue = 1.5;
                        listOfCells.Add(cells[1][jD + 1]);
                    }

                    if (!cells[1][jD - 1].isAWall) {
                        cells[1][jD - 1].floorValue = 1.5;
                        listOfCells.Add(cells[1][jD - 1]);
                    }
                } else if (iD == cols - 1) {                //jesli drzwi po prawej
                    cells[iD - 1][jD].floorValue = 1;
                    listOfCells.Add(cells[iD - 1][jD]);

                    if (!cells[iD - 1][jD - 1].isAWall) {
                        cells[iD - 1][jD - 1].floorValue = 1.5;
                        listOfCells.Add(cells[iD - 1][jD + 1]);
                    }

                    if (!cells[iD - 1][jD + 1].isAWall) {
                        cells[iD - 1][jD + 1].floorValue = 1.5;
                        listOfCells.Add(cells[iD - 1][jD - 1]);
                    }
                }

                while (listOfCells.Count < (rows - 2)*(cols - 2)) {
                    foreach (Cell cell in listOfCells) {
                        if (!cell.isAWall)
                            CheckCells(cell.i, cell.j, cells);
                    }


                    for (int i = 1; i < cols - 1; i++)
                        for (int j = 1; j < rows - 1; j++)
                            if (cells[i][j].floorValue != 666 && !listOfCells.Contains(cells[i][j])) {
                                listOfCells.Add(cells[i][j]);
                            }
                }

                cells[iD][jD].floorValue = 0;
                 

                /*
                if (jD == rows - 1) {                       //jesli drzwi na dole -> direction = 0
                    cells[iD][jD - 1].floorValue = 1;
                    cells[iD - 1][jD - 1].floorValue = 1.5;
                    cells[iD + 1][jD - 1].floorValue = 1.5;

                    for (int j = jD - 1; j > 0; j--) {
                        for (int i = iD; i < cols - 1; i++) { //od drzwi w prawo 
                            checkCells(i, j, cells);
                            cells[iD][jD].floorValue = 0;
                        }

                        for (int i = iD - 1; i > 0; i--) { //od obok drzwi w lewo
                            checkCells(i, j, cells);
                            cells[iD][jD].floorValue = 0;
                        }


                    }
                }

                if (jD == 0) {                       //jesli drzwi na gorze -> direction = 1
                    cells[iD][1].floorValue = 1;
                    cells[iD - 1][1].floorValue = 1.5;
                    cells[iD + 1][1].floorValue = 1.5;

                    for (int j = jD + 1; j < rows - 2; j++) {
                        for (int i = iD; i < cols - 1; i++) { //od drzwi w prawo 
                            checkCells(i, j, cells);
                            cells[iD][jD].floorValue = 0;
                        }

                        for (int i = iD - 1; i > 0; i--) { //od obok drzwi w lewo
                            checkCells(i, j, cells);
                            cells[iD][jD].floorValue = 0;
                        }
                    }
                }

                if (iD == 0) {                       //jesli drzwi po lewej -> direction = 2
                    cells[1][jD].floorValue = 1;
                    cells[1][jD + 1].floorValue = 1.5;
                    cells[1][jD - 1].floorValue = 1.5;

                    for (int i = 1; i < cols - 2; i++) {
                        for (int j = jD; j > 1; j--) { //od drzwi w gore 
                            checkCells(i, j, cells);
                            cells[iD][jD].floorValue = 0;
                        }

                        for (int j = jD + 1; j <rows - 2; j++) { //od drzwi w dol
                            checkCells(i, j, cells);
                            cells[iD][jD].floorValue = 0;
                        }
                    }
                }

                if (iD == cols - 1) {                       //jesli drzwi po prawej -> direction = 3
                    cells[iD - 1][jD].floorValue = 1;
                    cells[iD - 1][jD - 1].floorValue = 1.5;
                    cells[iD - 1][jD + 1].floorValue = 1.5;

                    for (int i = iD - 1; i > 1; i--) {
                        for (int j = jD; j > 1; j--) { //od drzwi w gore 
                            checkCells(i, j, cells);
                            cells[iD][jD].floorValue = 0;
                        }

                        for (int j = jD + 1; j < rows - 2; j++) { //od drzwi w dol
                            checkCells(i, j, cells);
                            cells[iD][jD].floorValue = 0;
                        }
                    }*/
            }
        }
    }
}

