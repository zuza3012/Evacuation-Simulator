using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ha {
    class Cell {
        public double x, y, floorValue;
        public bool isAWall = false, isAPerson = false, isADoor = false;

        public Cell(double x, double y) {
            this.x = x;
            this.y = y;
        }

        public Cell() { }

        internal static void GenerateField(Cell[][] cells) {
            throw new NotImplementedException();
        }
    }

    
}
