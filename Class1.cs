using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ha {
    class Cell {
        public double x, y;
        public bool isAWall = false, isAPerson = false, isADoor = false;

        public Cell(double x, double y) {
            this.x = x;
            this.y = y;
        }

        public Cell() { }
    }
}
