using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    /// <summary>
    /// Свойства и состояние игрового поля
    /// </summary>
    struct GameField
    {
        public int Width;
        public int Height;
        public ShapeKind[,] Points;

        public GameField(int width, int height)
        {
            Width = width;
            Height = height;
            Points = new ShapeKind[Width, Height];
        }
    }
}
