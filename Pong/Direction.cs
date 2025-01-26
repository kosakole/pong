using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Direction
    {
        private readonly int numDeg = 1;
        public int Vertical {  get; set; }
        public int Horizontal { get; set; }

        public Direction() { }
        public Direction(int vertical, int horizontal) {  Vertical = vertical; Horizontal = horizontal; }

        public void ChangeHorizontal()
        {
            Horizontal *= -1;
        }

        public void ChangeVertical()
        {
            Vertical *= -1;
        }

        public void ChangeDirectionLeft()
        {
            if (Horizontal >= 0)
                Horizontal -= numDeg;
        }

        public void ChangeDirectionRight()
        {
            if (Horizontal <= 0)
                Horizontal += numDeg;
        }
    }
}
