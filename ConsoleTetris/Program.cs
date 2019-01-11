using System;

namespace ConsoleTetris
{
    /// <summary>
    /// ConsoleTetris - a Tetris game
    /// https://github.com/mkolesn/ConsoleTetris
    /// </summary>
    class Program
    {
        // TODO Добавлять ко всем типам и методам xml-комментарии
        // TODO Добавить английское и украинское меню и выбор языка при запуске программы (добавил enum InterfaceLanguage)

        const int FIELD_WIDTH = 10;
        const int FIELD_HEIGHT = 20;
        const int NEXT_SHAPE_FIELD_WIDTH = 7;
        const int NEXT_SHAPE_FIELD_HEIGHT = 7;

        const int SCREEN_WIDTH = 80;
        const int SCREEN_HEIGHT = 50;
        const int GAME_FIELD_LEFT = 1;
        const int GAME_FIELD_TOP = 2;
        const int NEXT_SHAPE_FIELD_LEFT = 48;
        const int NEXT_SHAPE_FIELD_TOP = GAME_FIELD_TOP;
        const int SCORE_LEFT = NEXT_SHAPE_FIELD_LEFT;
        const int SCORE_TOP = 22;
        const int LEVEL_LEFT = NEXT_SHAPE_FIELD_LEFT;
        const int LEVEL_TOP = 23;

        const int SPLASH_SCREEN_DELAY = 5; // сколько времени показывать стартовый экран
        const int GAME_QUANTUM = 5; // минимальный квант времени в игре. в миллисекундах
        const int FALL_ANIMATE_QUANTUM = 300;
        static int[] LEVEL_SCORE = { 0, 2000, 4000, 7000, 11000, 15000 }; // количество очков, при котором происходит переход на новый уровень
        static TimeSpan[] LEVEL_DELAY = { new TimeSpan(0, 0, 0, 1, 100),
                                           new TimeSpan(0, 0, 0, 0, 900),
                                           new TimeSpan(0, 0, 0, 0, 700),
                                           new TimeSpan(0, 0, 0, 0, 500),
                                           new TimeSpan(0, 0, 0, 0, 300),
                                           new TimeSpan(0, 0, 0, 0, 200)}; // величина задержки между автоматическими падениями фигуры (миллисекунды)


        static void Main(string[] args)
        {
            MenuItem[] menu;

            BL.InitializeApplication(out menu);
            UI.Initialize(SCREEN_WIDTH, SCREEN_HEIGHT);
#if !DEBUG
            UI.ShowSplashScreen(SPLASH_SCREEN_DELAY);
#endif
            ConsoleColor[] shapeColors = { ConsoleColor.Black, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Blue, ConsoleColor.Magenta, ConsoleColor.DarkYellow, ConsoleColor.Red }; // цвета фигур

            Game game = BL.InitializeGame(FIELD_WIDTH, FIELD_HEIGHT, NEXT_SHAPE_FIELD_WIDTH, NEXT_SHAPE_FIELD_HEIGHT);

            GameView view = new GameView(GAME_FIELD_LEFT, GAME_FIELD_TOP, NEXT_SHAPE_FIELD_LEFT, NEXT_SHAPE_FIELD_TOP,
                SCORE_LEFT, SCORE_TOP, LEVEL_LEFT, LEVEL_TOP, shapeColors);

            GameMenu menuItemSelected;
            int currentItem = 0;
            bool gameOver = false;

            do
            {
                menuItemSelected = UI.GetMenuSelection(menu, ref currentItem);

                switch (menuItemSelected)
                {
                    case GameMenu.StartGame:
                        game = BL.InitializeGame(FIELD_WIDTH, FIELD_HEIGHT, NEXT_SHAPE_FIELD_WIDTH, NEXT_SHAPE_FIELD_HEIGHT);
                        gameOver = PlayGame(ref game, view);
                        menu[(int)GameMenu.ResumeGame].Enabled = !gameOver;
                        if (gameOver)
                        {
                            UI.ShowGameOver(view, game);
                        }
                        else
                        {
                            currentItem = (int)GameMenu.ResumeGame; // если игра не завершена, то переключаемся на пункт GameMenu.ResumeGame
                        }
                        break;

                    case GameMenu.ResumeGame:
                        gameOver = PlayGame(ref game, view);
                        menu[(int)GameMenu.ResumeGame].Enabled = !gameOver;
                        if (gameOver)
                        {
                            currentItem = (int)GameMenu.StartGame; // в завершённую игру запрещено возвращаться. переключаемся на пункт GameMenu.StartGame
                            UI.ShowGameOver(view, game);
                        }
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

        private static bool PlayGame(ref Game game, GameView view)
        {
            bool gameOver = false;

            Console.Clear();
            UI.DrawScore(view, game);
            UI.DrawNextShapeField(view, game);
            UI.DrawPlayField(view, game);
            UI.DrawCurrentShape(view, game);
            UI.DrawNextShape(view, game);

            // Game Loop
            GameCommand command;
            DateTime previousFall = DateTime.Now; // время предыдущего автоматического опускания фигуры

            bool quit = false;

            do
            {
                if (BL.IsShapeLanded(game.CurrentShape, game.Field))
                {
                    int minShapeRow;
                    int maxShapeRow;
                    int[] filledRows;

                    BL.AppendShape(game.CurrentShape, game.Field, out minShapeRow, out maxShapeRow);

                    if (BL.HasFilledRows(minShapeRow, maxShapeRow, game.Field, out filledRows))
                    {
                        game.Score.Score += BL.CalcScore(filledRows.Length);
                        if(game.Score.Level < (LEVEL_SCORE.Length - 1) && game.Score.Score >= LEVEL_SCORE[game.Score.Level + 1])
                        {
                            ++game.Score.Level;
                        }

                        UI.DrawScore(view, game);

                        int firstRowToFall = BL.FirstEmptyRow(game.Field, filledRows[0] == 0 ? filledRows[0] : filledRows[0] - 1) + 1;

                        for (int i = 0; i < filledRows.Length; i++)
                        {
                            BL.RemoveFilledRow(firstRowToFall, filledRows[i], game.Field);
                            UI.DrawRows(firstRowToFall, filledRows[i], game.Field, view);

                            firstRowToFall++;
                            System.Threading.Thread.Sleep(FALL_ANIMATE_QUANTUM);
                        }
                    }

                    UI.ClearNextShape(view, game);
                    BL.SetNextShape(ref game);

                    if (BL.IsPositionPossible(game.Field, game.CurrentShape))
                    {
                        UI.DrawCurrentShape(view, game);
                        UI.DrawNextShape(view, game);
                    }
                    else
                    {
                        gameOver = true;
                    }
                }
                else
                {
                    if (IsTimeToFall(previousFall, LEVEL_DELAY[game.Score.Level]))
                    {
                        //автоматическое падение текущей фигуры
                        MoveShape(ref game, view, 0, 1);
                        previousFall = DateTime.Now;
                    }

                    if (UI.HasUserInput(out command))
                    {
                        if (command == GameCommand.QuitToMenu)
                        {
                            quit = true;
                        }
                        else
                        {
                            ExecuteCommand(command, ref game, view);
                        }
                    }
                }

                System.Threading.Thread.Sleep(GAME_QUANTUM);
            } while (!quit && !gameOver);

            return gameOver;
        }

        private static bool IsTimeToFall(DateTime previousFall, TimeSpan delay)
        {
            return (DateTime.Now >= (previousFall + delay));
        }

        private static void ExecuteCommand(GameCommand command, ref Game g, GameView view)
        {
            switch (command)
            {
                case GameCommand.MoveLeft:
                    TryMoveShape(ref g, view, -1, 0);
                    break;

                case GameCommand.MoveRight:
                    TryMoveShape(ref g, view, 1, 0);
                    break;

                case GameCommand.MoveDown:
                    TryMoveShape(ref g, view, 0, 1);
                    break;

                case GameCommand.Land:
                    while (TryMoveShape(ref g, view, 0, 1))
                    {
                        System.Threading.Thread.Sleep(GAME_QUANTUM);
                    }
                    break;

                case GameCommand.Rotate:
                    Shape rotated = BL.RotateShape(g.CurrentShape);
                    if (BL.IsPositionPossible(g.Field, rotated))
                    {
                        Point2D[] makeEmpty;
                        Point2D[] makeFilled;

                        BL.CalcChangedPoints(g.CurrentShape, rotated, out makeEmpty, out makeFilled);
                        g.CurrentShape = rotated;
                        UI.DrawPoints(makeEmpty, ShapeKind.Empty, view);
                        UI.DrawPoints(makeFilled, g.CurrentShape.Kind, view);
                    }
                    break;
            }
        }

        private static void MoveShape(ref Game game, GameView view, int horizontalShift, int verticalShift)
        {
            Point2D[] makeEmpty;
            Point2D[] makeFilled;

            BL.MoveShape(ref game.CurrentShape, horizontalShift, verticalShift, out makeEmpty, out makeFilled);
            UI.DrawPoints(makeEmpty, ShapeKind.Empty, view);
            UI.DrawPoints(makeFilled, game.CurrentShape.Kind, view);
        }

        private static bool TryMoveShape(ref Game g, GameView view, int horizontalShift, int verticalShift)
        {
            bool result = false;
            if (BL.IsPositionPossible(g.Field, g.CurrentShape, horizontalShift, verticalShift))
            {
                MoveShape(ref g, view, horizontalShift, verticalShift);
                result = true;
            }
            return result;
        }
    }
}
