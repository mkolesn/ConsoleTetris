using System;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal class UI
    {
        const int MENU_LEFT = 10;
        const int MENU_TOP = 10;
        const int POINT_WIDTH = 2; // ширина одной ячейки фигурки/ одной ячейки игрового поля
        const int POINT_HEIGHT = 1; // высота одной ячейки фигурки/ одной ячейки игрового поля
        const string POINT = "\u2588\u2588"; // залитый квадрат для отображения одной ячейки игрового поля / фигуры // (char)0x + (char)0x2588; // 

        internal static void Initialize(int screenWidth, int screenHeight)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(screenWidth, screenHeight);
        }

        internal static void StartGame(ref Game game, GameView view)
        {
            //BL.InitializeGame(ref game);
            PlayGame(ref game, view);
        }

        private static void PlayGame(ref Game game, GameView view)
        {
            Console.Clear();
            UI.DrawScore(game, view);
            UI.DrawField(view.GameFieldLeft, view.GameFieldTop, game.Field);

            // Game Loop
            GameCommand command;
            bool quit = false;

            do
            {
                if (Console.KeyAvailable)
                {
                    command = TranslateCommand(Console.ReadKey(true).Key);

                    switch (command)
                    {
                        case GameCommand.NoCommand:
                            break;
                        case GameCommand.MoveLeft:
                            break;
                        case GameCommand.MoveRight:
                            break;
                        case GameCommand.MoveDown:
                            break;
                        case GameCommand.Land:
                            break;
                        case GameCommand.QuitToMenu:
                            quit = true;
                            break;
                        default:
                            break;
                    }
                }

                /*
                if (TimeToFall())
                {
                    Shape newShape = BL.MoveShape();
                }
                */

                System.Threading.Thread.Sleep(view.GameQuantum);

            } while (!quit);
            throw new NotImplementedException();
        }

        internal static void ResumeGame(ref Game game, GameView view)
        {
            PlayGame(ref game, view);
        }

        static GameCommand TranslateCommand(ConsoleKey key)
        {
            GameCommand command;

            switch (key)
            {
                case ConsoleKey.Escape:
                    command = GameCommand.QuitToMenu;
                    break;
                case ConsoleKey.Spacebar:
                    command = GameCommand.Land;
                    break;
                case ConsoleKey.LeftArrow:
                    command = GameCommand.MoveLeft;
                    break;
                case ConsoleKey.RightArrow:
                    command = GameCommand.MoveRight;
                    break;
                case ConsoleKey.DownArrow:
                    command = GameCommand.MoveDown;
                    break;
                default:
                    command = GameCommand.NoCommand;
                    break;
            }

            return command;
        }

        internal static void ShowSplashScreen(int delaySeconds = 0)
        {
            Console.Clear();

            DateTime d = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 0, 1);
            Console.ForegroundColor = ConsoleColor.Gray;

            // Показываем стартовый экран на протяжении заданного интервала либо до нажатия клавиши
            // Если передали нулевой интервал, то выходим только по нажатию клавиши
            do
            {

                Console.WriteLine("ConsoleTetris Game SplashScreen");
                if(Console.ForegroundColor == ConsoleColor.White)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.SetCursorPosition(0, 0);
                    Console.Clear();
                }
                else
                {
                    Console.ForegroundColor++;
                }
                System.Threading.Thread.Sleep(ts);
            } while((delaySeconds == 0 || d.AddSeconds(delaySeconds) > DateTime.Now)
                    && !Console.KeyAvailable);

            if(Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        internal static void ShowHelp()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("ПРАВИЛА");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Случайные фигурки тетрамино падают сверху в прямоугольный стакан шириной 10 и высотой 20 клеток. В полёте игрок может поворачивать фигурку на 90° и двигать её по горизонтали. Также можно «сбрасывать» фигурку, то есть ускорять её падение, когда уже решено, куда фигурка должна упасть. Фигурка летит до тех пор, пока не наткнётся на другую фигурку либо на дно стакана. Если при этом заполнился горизонтальный ряд из 10 клеток, он пропадает и всё, что выше него, опускается на одну клетку. Дополнительно показывается фигурка, которая будет следовать после текущей — это подсказка, которая позволяет игроку планировать действия. Темп игры постепенно увеличивается. Игра заканчивается, когда новая фигурка не может поместиться в стакан. Игрок получает очки за каждый заполненный ряд, поэтому его задача — заполнять ряды, не заполняя сам стакан (по вертикали) как можно дольше, чтобы таким образом получить как можно больше очков.");

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nУПРАВЛЕНИЕ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Стрелки влево и вправо - двигать фигуру влево/вправо");
            Console.WriteLine("Стрелка вниз - ускорение падения фигуры");
            Console.WriteLine("Пробел - опустить фигуру до предела");
            Console.WriteLine("Esc - поставить игру на паузу и выйти в основное меню");

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n\nНажмите любую клавишу, чтобы вернуться в основное меню");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey(true);
        }

        internal static void ShowCredits()
        {
            ShowSplashScreen();
        }

        internal static void ShowMenu(MenuItem[] menu, int currentItem)
        {

            Console.SetCursorPosition(MENU_LEFT, MENU_TOP);

            for(int i = 0; i < menu.Length; i++)
            {
                if(i == currentItem)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    if(menu[i].Enabled)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                }

                Console.CursorLeft = MENU_LEFT;
                Console.WriteLine(menu[i].Text);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Вывести на экран меню и обработать пользовательский выбор пункта меню
        /// </summary>
        /// <param name="menu"></param>
        /// <returns>выбранный пункт меню</returns>
        internal static GameMenu GetMenuSelection(MenuItem[] menu, ref int currentItem)
        {
            bool itemConfirm = false;

            Console.Clear();

            do
            {
                UI.ShowMenu(menu, currentItem);

                // Ожидаем нажатия клавиши
                do
                {
                    System.Threading.Thread.Sleep(1);
                } while(!Console.KeyAvailable);

                // Обрабатываем пользовательский ввод
                ConsoleKey key = Console.ReadKey(true).Key;
                switch(key)
                {
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        itemConfirm = true;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.Tab:
                        SelectNextMenuItem(menu, ref currentItem);
                        break;
                    case ConsoleKey.UpArrow:
                        SelectPreviousMenuItem(menu, ref currentItem);
                        break;
                    default:
                        break;
                }

            } while(!itemConfirm);

            return menu[currentItem].Value;
        }

        /// <summary>
        /// Сделать текущим следующий не запрещённый пункт меню. Меню цикличное
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="currentItem"></param>
        private static void SelectPreviousMenuItem(MenuItem[] menu, ref int currentItem)
        {
            int newCurrent = currentItem;

            do
            {
                if(newCurrent == 0)
                {
                    newCurrent = menu.Length - 1;
                }
                else
                {
                    newCurrent--;
                }
            } while(!menu[newCurrent].Enabled && newCurrent != currentItem); // если нашли разрешённый пункт меню или дошли по кругу до исходного пункта, то выходим из цикла

            currentItem = newCurrent;
        }

        /// <summary>
        /// Сделать текущим предыдующий не запрещённый пункт меню. Меню цикличное
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="currentItem"></param>
        private static void SelectNextMenuItem(MenuItem[] menu, ref int currentItem)
        {
            int newCurrent = currentItem;

            do
            {
                if(newCurrent == menu.Length - 1)
                {
                    newCurrent = 0;
                }
                else
                {
                    newCurrent++;
                }
            } while(!menu[newCurrent].Enabled && newCurrent != currentItem); // если нашли разрешённый пункт меню или дошли по кругу до исходного пункта, то выходим из цикла

            currentItem = newCurrent;
        }

        public static void DrawField(int left, int top, GameField field)
        {
            DrawBorder(left, top, field.Width * POINT_WIDTH, field.Height * POINT_HEIGHT, ConsoleColor.White);

            for(int deltaX = 0; deltaX < field.Points.GetLength(0); deltaX++)
            {
                for(int deltaY = 0; deltaY < field.Points.GetLength(1); deltaY++)
                {
                    DrawPoint(left + deltaX, top + deltaY, field.Points[deltaX, deltaY]);
                }
            }
        }

        private static void DrawPoint(int x, int y, ShapeKind shapeKind)
        {
            Console.SetCursorPosition(x * POINT_WIDTH, y * POINT_HEIGHT);
            switch(shapeKind)
            {
                case ShapeKind.Empty:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case ShapeKind.GLeft:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ShapeKind.GRight:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ShapeKind.SnakeLeft:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case ShapeKind.SnakeRight:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case ShapeKind.Square:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case ShapeKind.Strait:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case ShapeKind.TLetter:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
            }
            Console.Write(POINT);
        }

        /// <summary>
        ////Нарисовать рамку вокруг заданной области экрана
        /// </summary>
        /// <param name="point2D"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="consoleColor"></param>
        public static void DrawBorder(int left, int top, int width, int height, ConsoleColor consoleColor)
        {
            throw new NotImplementedException();
        }

        public static void DrawScore(Game game, GameView view)
        {
            throw new NotImplementedException();
        }


        static void DrawShape(Shape shape)
        {
            for(int i = 0; i < shape.Points.Length; i++)
            {
                DrawPoint(shape.Position.Left + shape.Points[i].Left, shape.Position.Top + shape.Points[i].Top, shape.Kind);
            }
        }

        static void DrawModifiedShape(Shape oldShape, Shape newShape)
        {
            Point2D[] oldPoints = (Point2D[])oldShape.Points.Clone();
            Point2D[] newPoints = (Point2D[])newShape.Points.Clone();

            // переводим относительные координаты в координаты игрового поля
            for (int i = 0; i < oldPoints.Length; i++)
			{
                oldPoints[i].Left += newShape.Position.Left;
                oldPoints[i].Top += newShape.Position.Top;
			}

            // переводим относительные координаты в координаты игрового поля
            for (int i = 0; i < newPoints.Length; i++)
			{
                newPoints[i].Left += oldShape.Position.Left;
                newPoints[i].Top += oldShape.Position.Top;
			}

            // если точки старой фигуры отсутствуют в составе новой, то закрасить их пустым цветом
            for(int old = 0; old < oldPoints.Length; old++)
            {
                bool pointChanged = true;

                for(int new1 = 0; new1 < newPoints.Length; new1++)
                {
                    if(oldPoints[old].Left == newPoints[new1].Left
                        && oldPoints[old].Top == newPoints[new1].Top)
                    {
                        pointChanged = false;
                        break;
                    }
                }

                if(pointChanged)
                {
                    UI.DrawPoint(oldPoints[old].Left, oldPoints[old].Top, ShapeKind.Empty);
                }
            }

            // если точки новой фигуры отсутствуют в составе старой, то закрасить их пустым цветом
            for(int new2 = 0; new2 < oldPoints.Length; new2++)
            {
                bool pointChanged = true;

                for(int old1 = 0; old1 < oldPoints.Length; old1++)
                {
                    if(newPoints[new2].Left == oldPoints[old1].Left
                        && newPoints[new2].Top == oldPoints[old1].Top)
                    {
                        pointChanged = false;
                        break;
                    }
                }

                if(pointChanged)
                {
                    UI.DrawPoint(newPoints[new2].Left, newPoints[new2].Top, newShape.Kind);
                }
            }

        }

    }
}
