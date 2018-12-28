using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    struct Point2D
    {
        public int Left;
        public int Top;

        public Point2D(int left, int top)
        {
            Left = left;
            Top = top;
        }
    }
}
