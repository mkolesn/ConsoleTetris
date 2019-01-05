using System;

namespace ConsoleTetris
{
    struct Game
    {
        public GameField Field;
        public GameField NextShapeField;
        public Shape NextShape;
        public Shape CurrentShape;
        public GameScore Score;
        public Point2D shapeStart;     // координата игрового поля, в которой появляется текущая фигура
    }
}