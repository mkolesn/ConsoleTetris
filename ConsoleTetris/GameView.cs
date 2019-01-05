using System;

namespace ConsoleTetris
{
    /// <summary>
    ////Данные для отрисовки игры
    /// </summary>
    struct GameView
    {
        // TODO Выделить тип Window или RectangularArea, и переделать вызовы методов рисования полей на него
        public int GameFieldLeft;
        public int GameFieldTop;
        public int NextShapeFieldLeft;
        public int NextShapeFieldTop;
        public int ScoreLeft;
        public int ScoreTop;
        public int LevelLeft;
        public int LevelTop;
        public ConsoleColor[] ShapeColors;

        public GameView(int gameFieldLeft, int gameFieldTop, int nextShapeFieldLeft, int nextShapeFieldTop,
            int scoreLeft, int scoreTop, int levelLeft, int levelTop, ConsoleColor[] shapeColors)
        {
            GameFieldLeft = gameFieldLeft;
            GameFieldTop = gameFieldTop;
            NextShapeFieldLeft = nextShapeFieldLeft;
            NextShapeFieldTop = nextShapeFieldTop;
            ScoreLeft = scoreLeft;
            ScoreTop = scoreTop;
            LevelLeft = levelLeft;
            LevelTop = levelTop;
            ShapeColors = shapeColors;
        }
    }
}
