using System;

namespace Ha {
    class Cell {
        public double x, y, floorValue = 0;
        public bool isAWall = false, isAPerson = false, isADoor = false;

        public Cell(double x, double y) {
            this.x = x;
            this.y = y;
        }

        public Cell() { }

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
                    jD = rows;
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
                    iD = cols;
                    jD = j;
                }
            }

            if (iD == 0 && jD == 0) {
                System.Windows.MessageBox.Show("You didn't put any door here! We are going to die!", "Where is the door?");
            }
        }
    }


}
