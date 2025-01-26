using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Coordinates
    {
        public int X ; public int Y;

        public Coordinates()
        {

        }
        public Coordinates(int x, int y)
        {
                X = x;
                Y = y;
        }

        public Coordinates Move(Direction dir)
        {
            Coordinates ret = new Coordinates(); ;
            ret.X = X + dir.Horizontal;
            ret.Y = Y + dir.Vertical;
            return ret;
        }
    }
}
