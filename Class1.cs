using System;

namespace Ha {
    class Cell {
        public double x, y, floorValue = 666;
        public bool isAWall = false, isAPerson = false, isADoor = false;

        public Cell(double x, double y) {
            this.x = x;
            this.y = y;
        }

        public Cell() { }

        internal static void checkCells(int i, int j, Cell[][] cells) {
            if (!cells[i - 1][j].isAWall && !cells[i - 1][j].isADoor) { //sasiad z lewej
                if (cells[i][j].floorValue + 1 < cells[i - 1][j].floorValue)
                    cells[i - 1][j].floorValue = cells[i][j].floorValue + 1;
                Console.WriteLine((cells[i][j].floorValue + 1) + "\t" + (cells[i - 1][j].floorValue));
            } else {
                cells[i - 1][j].floorValue = 500;
            }

            if (!cells[i + 1][j].isAWall && !cells[i + 1][j].isADoor) { //sasiad z prawej
                if (cells[i][j].floorValue + 1 < cells[i + 1][j].floorValue)
                    cells[i + 1][j].floorValue = cells[i][j].floorValue + 1;
            } else {
                cells[i + 1][j].floorValue = 500;
            }

            if (!cells[i][j - 1].isAWall && !cells[i][j - 1].isADoor) { //sasiad z gory
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
                cells[i + 1][j - 1].floorValue = 500;
            }

            if (!cells[i - 1][j + 1].isAWall && !cells[i - 1][j + 1].isADoor) { //sasiad z lewego dolu
                if (cells[i][j].floorValue + 1.5 < cells[i + 1][j + 1].floorValue)
                    cells[i + 1][j + 1].floorValue = cells[i][j].floorValue + 1.5;
            } else {
                cells[i + 1][j - 1].floorValue = 500;
            }

            if (!cells[i][j + 1].isAWall && !cells[i][j + 1].isADoor) { //sasiad z dolu
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
                //tu bendzie jagiź algorydm
                if (jD == rows - 1) {
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
                        //tu cos bendzie
                    }
                }

            }




        }
    }


}
