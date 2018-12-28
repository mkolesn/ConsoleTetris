using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    class Program
    {
        const int SPLASH_SCREEN_DELAY = 5; // сколько времени показывать стартовый экран
        const int FIELD_WIDTH = 10;
        const int FIELD_HEIGHT = 20;
        const int NEXT_SHAPE_FIELD_WIDTH = 6;
        const int NEXT_SHAPE_FIELD_HEIGHT = 6;

        const int SCREEN_WIDTH = 80;
        const int SCREEN_HEIGHT = 50;

        const int GAME_FIELD_LEFT = 1;
        const int GAME_FIELD_TOP = 1;
        const int NEXT_SHAPE_FIELD_LEFT = 24;
        const int NEXT_SHAPE_FIELD_TOP = 4;
        const int SCORE_LEFT = 24;
        const int SCORE_TOP = 15;
        const int LEVEL_LEFT = 24;
        const int LEVEL_TOP = 16;
        const int GAME_QUANTUM = 5; // минимальный квант времени в игре. в миллисекундах

        static void Main(string[] args)
        {
            MenuItem[] menu;

            BL.InitializeApplication(out menu);
            UI.Initialize(SCREEN_WIDTH, SCREEN_HEIGHT);
            UI.ShowSplashScreen(SPLASH_SCREEN_DELAY);

            int[] levelScore = {0, 1000, 3000 }; // количество очков, при котором происходит переход на новый уровень
            int[] levelDelay = {60, 40, 25}; // величина задержки между автоматическими падениями фигуры

            Game game = BL.InitializeGame(FIELD_WIDTH, FIELD_HEIGHT, NEXT_SHAPE_FIELD_WIDTH, NEXT_SHAPE_FIELD_HEIGHT, levelScore);

            GameView view = new GameView(GAME_FIELD_LEFT, GAME_FIELD_TOP, NEXT_SHAPE_FIELD_LEFT, NEXT_SHAPE_FIELD_TOP,
                SCORE_LEFT, SCORE_TOP, LEVEL_LEFT, LEVEL_TOP, GAME_QUANTUM, levelDelay);

            GameMenu menuItemSelected;
            int currentItem = 0;

            do
            {
                menuItemSelected = UI.GetMenuSelection(menu, ref currentItem);

                switch (menuItemSelected)
                {
                    case GameMenu.StartGame:
                        game = BL.InitializeGame(FIELD_WIDTH, FIELD_HEIGHT, NEXT_SHAPE_FIELD_WIDTH, NEXT_SHAPE_FIELD_HEIGHT, levelScore);
                        UI.StartGame(ref game, view);
                        break;
                    case GameMenu.ResumeGame:
                        UI.ResumeGame(ref game, view);
                        break;
                    case GameMenu.ShowHelp:
                        UI.ShowHelp();
                        break;
                    case GameMenu.ShowCredits:
                        UI.ShowCredits();
                        break;
                    default:
                        break;
                }
            } while (menuItemSelected != GameMenu.QuitApplication);

        }
    }
}
