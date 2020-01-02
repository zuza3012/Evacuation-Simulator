using System.Collections.Generic;
using System.Linq;

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
        public Cell(int i, int j, int floorValue) {
            this.i = i;
            this.j = j;
            this.floorValue = floorValue;
        }

        public Cell() { }

        internal static List<Cell> FindHoomans(Cell[][] cells) {
            List<Cell> listOfHoomans = new List<Cell>();
            for (int j = 0; j < cells[0].Length; j++) {
                for (int i = 0; i < cells.Length; i++) {
                    if (cells[i][j].isAPerson && !(cells[i][j].floorValue == 0)) {
                        listOfHoomans.Add(cells[i][j]);
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
            } else if (cells[i - 1][j].isAWall) {
                cells[i - 1][j].floorValue = 500;
            } else {
                cells[i - 1][j].floorValue = 0;
            }

            if (!cells[i + 1][j].isAWall && !cells[i + 1][j].isADoor) {         //sasiad z prawej
                if (cells[i][j].floorValue + 1 < cells[i + 1][j].floorValue)
                    cells[i + 1][j].floorValue = cells[i][j].floorValue + 1;
            } else if (cells[i + 1][j].isAWall) {
                cells[i + 1][j].floorValue = 500;
            } else {
                cells[i + 1][j].floorValue = 0;
            }

            if (!cells[i][j - 1].isAWall && !cells[i][j - 1].isADoor) {         //sasiad z gory
                if (cells[i][j].floorValue + 1 < cells[i][j - 1].floorValue)
                    cells[i][j - 1].floorValue = cells[i][j].floorValue + 1;
            } else if (cells[i][j - 1].isAWall) {
                cells[i][j - 1].floorValue = 500;
            } else {
                cells[i][j - 1].floorValue = 0;
            }

            if (!cells[i - 1][j - 1].isAWall && !cells[i - 1][j - 1].isADoor) { //sasiad z lewej gory
                if (cells[i][j].floorValue + 1.5 < cells[i - 1][j - 1].floorValue)
                    cells[i - 1][j - 1].floorValue = cells[i][j].floorValue + 1.5;
            } else if (cells[i - 1][j - 1].isAWall) {
                cells[i - 1][j - 1].floorValue = 500;
            } else {
                cells[i - 1][j - 1].floorValue = 0;
            }

            if (!cells[i + 1][j - 1].isAWall && !cells[i + 1][j - 1].isADoor) { //sasiad z prawej gory
                if (cells[i][j].floorValue + 1.5 < cells[i + 1][j - 1].floorValue)
                    cells[i + 1][j - 1].floorValue = cells[i][j].floorValue + 1.5;
            } else if (cells[i + 1][j - 1].isAWall) {
                cells[i + 1][j - 1].floorValue = 500;
            } else {
                cells[i + 1][j - 1].floorValue = 0;
            }

            if (!cells[i + 1][j + 1].isAWall && !cells[i + 1][j + 1].isADoor) { //sasiad z prawego dolu
                if (cells[i][j].floorValue + 1.5 < cells[i + 1][j + 1].floorValue)
                    cells[i + 1][j + 1].floorValue = cells[i][j].floorValue + 1.5;
            } else if (cells[i + 1][j + 1].isAWall) {
                cells[i + 1][j + 1].floorValue = 500;
            } else {
                cells[i + 1][j + 1].floorValue = 0;
            }

            if (!cells[i - 1][j + 1].isAWall && !cells[i - 1][j + 1].isADoor) { //sasiad z lewego dolu
                if (cells[i][j].floorValue + 1.5 < cells[i - 1][j + 1].floorValue)
                    cells[i - 1][j + 1].floorValue = cells[i][j].floorValue + 1.5;
            } else if (cells[i - 1][j + 1].isAWall) {
                cells[i - 1][j + 1].floorValue = 500;
            } else {
                cells[i - 1][j + 1].floorValue = 0;
            }

            if (!cells[i][j + 1].isAWall && !cells[i][j + 1].isADoor) {         //sasiad z dolu
                if (cells[i][j].floorValue + 1 < cells[i][j + 1].floorValue)
                    cells[i][j + 1].floorValue = cells[i][j].floorValue + 1;
            } else if (cells[i][j + 1].isAWall) {
                cells[i][j + 1].floorValue = 500;
            } else {
                cells[i][j + 1].floorValue = 0;
            }
        }

        internal static void GenerateField(Cell[][] cells) {
            int cols = cells.Length;
            int rows = cells[0].Length;

            List<Cell> listOfDoors = new List<Cell>();
            for (int i = 0; i < cols; i++) {
                for (int j = 0; j < rows; j++) {
                    if (cells[i][j].isADoor) {
                        cells[i][j].floorValue = 500;
                        listOfDoors.Add(cells[i][j]);
                    }
                }
            }
            List<Cell[][]> listOfFloorFieldValuesForSpecificDoorJustLikeTheyWereTheOnlyDoor = new List<Cell[][]>();
            int iD = 0, jD = 0;

            foreach (Cell door in listOfDoors) {
                Cell[][] copyOfCells = new Cell[cols][];

                for (int i = 0; i < cols; i++) {        //stwarzamy tablice 2D 
                    copyOfCells[i] = new Cell[rows];
                    for (int j = 0; j < rows; j++) {
                        copyOfCells[i][j] = new Cell(i, j, 666); //stworzylam nowy konstruktor Cell() 
                        if (cells[i][j].isAWall)
                            copyOfCells[i][j].isAWall = true;
                    }
                }

                for (int i = 0; i < cols; i++) {            //ustawiamy wartosc scian
                    copyOfCells[i][0].floorValue = 500;
                    copyOfCells[i][0].isAWall = true;
                    copyOfCells[i][rows - 1].floorValue = 500;
                    copyOfCells[i][rows - 1].isAWall = true;
                }

                for (int j = 0; j < rows; j++) {            // ustawiamy wartosc scian
                    copyOfCells[0][j].floorValue = 500;
                    copyOfCells[0][j].isAWall = true;
                    copyOfCells[cols - 1][j].floorValue = 500;
                    copyOfCells[cols - 1][j].isAWall = true;
                }

                jD = door.j;
                iD = door.i;
                copyOfCells[iD][jD].isADoor = true;
                copyOfCells[door.i][door.j].floorValue = 0;

                listOfFloorFieldValuesForSpecificDoorJustLikeTheyWereTheOnlyDoor.Add(copyOfCells);

            }

            if (listOfFloorFieldValuesForSpecificDoorJustLikeTheyWereTheOnlyDoor.Count != 0) {

                foreach (Cell[][] copyOfCells in listOfFloorFieldValuesForSpecificDoorJustLikeTheyWereTheOnlyDoor) {

                    int pos = listOfFloorFieldValuesForSpecificDoorJustLikeTheyWereTheOnlyDoor.IndexOf(copyOfCells);
                    iD = listOfDoors.ElementAt(pos).i;
                    jD = listOfDoors.ElementAt(pos).j;
                    System.Console.WriteLine("Drzwi:" + "(" + iD + ", " + jD + ")");


                    List<Cell> listOfCells = new List<Cell>();
                    //tu bendzie jagiź algorydm
                    if (jD == rows - 1) {                       //jesli drzwi na dole
                        copyOfCells[iD][jD - 1].floorValue = 1;
                        listOfCells.Add(copyOfCells[iD][jD - 1]);

                        if (!copyOfCells[iD - 1][jD - 1].isAWall && !(copyOfCells[iD - 1][jD - 1].floorValue < 1.5)) {
                            copyOfCells[iD - 1][jD - 1].floorValue = 1.5;
                            listOfCells.Add(copyOfCells[iD - 1][jD - 1]);
                        }

                        if (!copyOfCells[iD + 1][jD - 1].isAWall && !(copyOfCells[iD + 1][jD - 1].floorValue < 1.5)) {
                            copyOfCells[iD + 1][jD - 1].floorValue = 1.5;
                            listOfCells.Add(copyOfCells[iD + 1][jD - 1]);
                        }

                    } else if (jD == 0) {                       //jesli drzwi na gorze
                        copyOfCells[iD][1].floorValue = 1;
                        listOfCells.Add(copyOfCells[iD][1]);

                        if (!copyOfCells[iD - 1][1].isAWall && !(copyOfCells[iD - 1][1].floorValue < 1.5)) {
                            copyOfCells[iD - 1][1].floorValue = 1.5;
                            listOfCells.Add(copyOfCells[iD - 1][1]);
                        }
                        if (!copyOfCells[iD + 1][1].isAWall && !(copyOfCells[iD + 1][1].floorValue < 1.5)) {
                            copyOfCells[iD + 1][1].floorValue = 1.5;
                            listOfCells.Add(copyOfCells[iD + 1][1]);
                        }
                    } else if (iD == 0) {                       //jesli drzwi po lewej 
                        copyOfCells[1][jD].floorValue = 1;
                        listOfCells.Add(copyOfCells[1][jD]);

                        if (!copyOfCells[1][jD + 1].isAWall && !(copyOfCells[1][jD + 1].floorValue < 1.5)) {
                            copyOfCells[1][jD + 1].floorValue = 1.5;
                            listOfCells.Add(copyOfCells[1][jD + 1]);
                        }

                        if (!copyOfCells[1][jD - 1].isAWall && !(copyOfCells[1][jD - 1].floorValue < 1.5)) {
                            copyOfCells[1][jD - 1].floorValue = 1.5;
                            listOfCells.Add(copyOfCells[1][jD - 1]);
                        }
                    } else if (iD == cols - 1) {                //jesli drzwi po prawej
                        copyOfCells[iD - 1][jD].floorValue = 1;
                        listOfCells.Add(copyOfCells[iD - 1][jD]);

                        if (!copyOfCells[iD - 1][jD - 1].isAWall && !(copyOfCells[iD - 1][jD - 1].floorValue < 1.5)) {
                            copyOfCells[iD - 1][jD - 1].floorValue = 1.5;
                            listOfCells.Add(copyOfCells[iD - 1][jD - 1]);
                        }

                        if (!copyOfCells[iD - 1][jD + 1].isAWall && !(copyOfCells[iD - 1][jD + 1].floorValue < 1.5)) {
                            copyOfCells[iD - 1][jD + 1].floorValue = 1.5;
                            listOfCells.Add(copyOfCells[iD - 1][jD + 1]);
                        }
                    }

                    while (listOfCells.Count < (rows - 2) * (cols - 2)) {

                        foreach (Cell cell in listOfCells) {
                            if (!cell.isAWall) {
                                CheckCells(cell.i, cell.j, copyOfCells);
                            }
                        }

                        for (int i = 1; i < cols - 1; i++)
                            for (int j = 1; j < rows - 1; j++)
                                if (copyOfCells[i][j].floorValue != 666 && !listOfCells.Contains(copyOfCells[i][j])) {
                                    listOfCells.Add(copyOfCells[i][j]);
                                }
                    }
                    copyOfCells[iD][jD].floorValue = 0;
                }

                foreach (Cell[][] copyOfCells in listOfFloorFieldValuesForSpecificDoorJustLikeTheyWereTheOnlyDoor) {

                    for (int i = 0; i < cols; i++) {
                        for (int j = 0; j < rows; j++) {
                            if (copyOfCells[i][j].floorValue <= cells[i][j].floorValue) {
                                cells[i][j].floorValue = copyOfCells[i][j].floorValue;
                            }
                        }
                    }
                }
            } else {
                System.Windows.MessageBox.Show("You didn't put any door here! We are going to die!", "Where is the door?");
            }
        }
    }
}

