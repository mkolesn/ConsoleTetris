using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    /// <summary>
    ////Данные для отрисовки игры
    /// </summary>
    struct GameView
    {
        public int GameFieldLeft;
        public int GameFieldTop;
        public int NextShapeFieldLeft;
        public int NextShapeFieldTop;
        public int ScoreLeft;
        public int ScoreTop;
        public int LevelLeft;
        public int LevelTop;
        public int GameQuantum;
        public int[] LevelDelay;

        public GameView(int gameFieldLeft, int gameFieldTop, int nextShapeFieldLeft, int nextShapeFieldTop,
            int scoreLeft, int scoreTop, int levelLeft, int levelTop, int gameQuantum, int[] levelDelay)
        {
            GameFieldLeft = gameFieldLeft;
            GameFieldTop = gameFieldTop;
            NextShapeFieldLeft = nextShapeFieldLeft;
            NextShapeFieldTop = nextShapeFieldTop;
            ScoreLeft = scoreLeft;
            ScoreTop = scoreTop;
            LevelLeft = levelLeft;
            LevelTop = levelTop;
            GameQuantum = gameQuantum;
            LevelDelay = (int[])levelDelay.Clone();
        }
    }
}
