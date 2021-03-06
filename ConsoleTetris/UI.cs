﻿using System;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal class UI
    {
        const int MENU_LEFT = 10;
        const int MENU_TOP = 10;

        const int POINT_WIDTH = 4; // ширина одной ячейки фигурки/ одной ячейки игрового поля
        const int POINT_HEIGHT = 2; // высота одной ячейки фигурки/ одной ячейки игрового поля
        //const string POINT = "\u2588\u2588"; // залитый квадрат для отображения одной ячейки игрового поля / фигуры // (char)0x + (char)0x2588; // 
        const string POINT = "\u2588\u2588\u2588\u2588"; // залитый квадрат для отображения одной ячейки игрового поля / фигуры // (char)0x + (char)0x2588; // 

        const int SCORE_DIGITS = 6; // число знаков при выводе счёта
        const int LEVEL_DIGITS = 2; // число знаков при выводе уровня
        static TimeSpan MAX_DELAY = new TimeSpan(0, 0, 10);

        /// <summary>
        /// Инициализировать приложение
        /// </summary>
        /// <param name="screenWidth">Ширина экрана консоли</param>
        /// <param name="screenHeight">Высота экрана консоли</param>
        public static void Initialize(int screenWidth, int screenHeight)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(screenWidth, screenHeight);
        }

        /// <summary>
        /// Если появился пользовательский ввод, то вернуть истину и команду, которую ввёл пользователь
        /// </summary>
        /// <param name="command">Проинтерпретированный ввод команды пользователя</param>
        /// <returns>Признак, что получен пользовательский ввод</returns>
        public static bool HasUserInput(out GameCommand command)
        {
            if(Console.KeyAvailable)
            {
                command = TranslateCommand(Console.ReadKey(true).Key);
            }
            else {
                command = GameCommand.NoCommand;
            }

            return (command != GameCommand.NoCommand);
        }

        /// <summary>
        /// Преобразовать ввод пользователя в команду игры
        /// </summary>
        /// <param name="key">Нажатая клавиша</param>
        /// <returns>Команда, соответствующая заданной клавише</returns>
        private static GameCommand TranslateCommand(ConsoleKey key)
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
                case ConsoleKey.UpArrow:
                    command = GameCommand.Rotate;
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

        /// <summary>
        /// Показать заставку
        /// </summary>
        /// <param name="delaySeconds">Время показа заставки в секундах. Если равно ноль, то показывать до тех пор, пока пользователь не нажмёт любую клавишу</param>
        public static void ShowSplashScreen(int delaySeconds = 0)
        {
            Console.Clear();

            DateTime d = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 0, 1);
            Console.ForegroundColor = ConsoleColor.White;

            // Показываем стартовый экран на протяжении заданного интервала либо до нажатия клавиши
            // Если передали нулевой интервал, то выходим только по нажатию клавиши
            do
            {

                Console.WriteLine("ConsoleTetris Game SplashScreen");
                if(Console.ForegroundColor == ConsoleColor.DarkBlue)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(0, 0);
                    Console.Clear();
                }
                else
                {
                    Console.ForegroundColor--;
                }
                System.Threading.Thread.Sleep(ts);
            } while((delaySeconds == 0 || d.AddSeconds(delaySeconds) > DateTime.Now)
                    && !Console.KeyAvailable);

            if(Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Вывести на экран справку игры
        /// </summary>
        public static void ShowHelp()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("ПРАВИЛА");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Случайные фигурки тетрамино падают сверху в прямоугольный стакан шириной 10");
            Console.WriteLine("и высотой 20 клеток. В полёте игрок может поворачивать фигурку на 90°");
            Console.WriteLine("и двигать её по горизонтали. Также можно «сбрасывать» фигурку, то есть");
            Console.WriteLine("ускорять её падение, когда уже решено, куда фигурка должна упасть.");
            Console.WriteLine("Фигурка летит до тех пор, пока не наткнётся на другую фигурку либо");
            Console.WriteLine("на дно стакана. Если при этом заполнился горизонтальный ряд из 10 клеток,");
            Console.WriteLine("он пропадает и всё, что выше него, опускается на одну клетку.");
            Console.WriteLine("Дополнительно показывается фигурка, которая будет следовать после текущей —");
            Console.WriteLine("это подсказка, которая позволяет игроку планировать действия.");
            Console.WriteLine("Темп игры постепенно увеличивается. Игра заканчивается, когда новая фигурка");
            Console.WriteLine("не может поместиться в стакан. Игрок получает очки за каждый заполненный ряд,");
            Console.WriteLine("поэтому его задача — заполнять ряды, не заполняя сам стакан (по вертикали)");
            Console.WriteLine("как можно дольше, чтобы таким образом получить как можно больше очков.");

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nУПРАВЛЕНИЕ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Стрелки влево и вправо - двигать фигуру влево/вправо");
            Console.WriteLine("Стрелка вверх - повернуть фигуру на 90 градусов по часовой стрелке");
            Console.WriteLine("Стрелка вниз - ускорение падения фигуры");
            Console.WriteLine("Пробел - опустить фигуру до предела");
            Console.WriteLine("Esc - поставить игру на паузу и выйти в основное меню");

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n\nНажмите любую клавишу, чтобы вернуться в основное меню");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey(true);
        }

        /// <summary>
        /// Отобразить окно "О программе"
        /// </summary>
        public static void ShowCredits()
        {
            ShowSplashScreen();
        }

        /// <summary>
        /// Вывести на экран меню и обработать пользовательский выбор пункта меню
        /// </summary>
        /// <param name="menu">Массив пунктов меню</param>
        /// <returns>Выбранный пункт меню</returns>
        public static GameMenu GetMenuSelection(MenuItem[] menu, ref int currentItem)
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
        /// Вывести меню на экран
        /// </summary>
        /// <param name="menu">Массив пунктов меню</param>
        /// <param name="currentItem">Выбранный пункт меню</param>
        private static void ShowMenu(MenuItem[] menu, int currentItem)
        {

            Console.SetCursorPosition(MENU_LEFT, MENU_TOP);

            for (int i = 0; i < menu.Length; i++)
            {
                if (i == currentItem)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    if (menu[i].Enabled)
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
        /// Сделать текущим следующий не запрещённый пункт меню. Меню цикличное
        /// </summary>
        /// <param name="menu">Массив пунктов меню</param>
        /// <param name="currentItem">Выбранный пункт меню</param>
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
        /// <param name="menu">Массив пунктов меню</param>
        /// <param name="currentItem">Выбранный пункт меню</param>
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

        /// <summary>
        /// Отобразить сообщение "Игра окончена"
        /// </summary>
        /// <param name="view">Свойства пользовательского интерфейса</param>
        /// <param name="game">Свойства игры</param>
        public static void ShowGameOver(GameView view, Game game)
        {
            string s1 = "G A M E";
            string s2 = "O V E R";

            int left = view.GameFieldLeft + game.Field.Width * POINT_WIDTH / 2 - s1.Length / 2;
            int top = view.GameFieldTop + game.Field.Height * POINT_HEIGHT / 2;
            //DrawBorder(view.GameFieldLeft, view.GameFieldTop + game.Field.Height * POINT_HEIGHT / 2, s1.Length + 2, 4, ConsoleColor.Red);
            DrawBorder(left, top, s1.Length + 2, 4, ConsoleColor.Red);
            Console.SetCursorPosition(left + 1, top + 1);
            Console.Write(s1);
            Console.SetCursorPosition(left + 1, top + 2);
            Console.Write(s2);

            DateTime start = DateTime.Now;

            do
            {
                System.Threading.Thread.Sleep(1);
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    break;
                }
            } while (DateTime.Now < (start + MAX_DELAY));
        }

        /// <summary>
        /// Вывести на экран Поле, отображающее следующую фигуру
        /// </summary>
        /// <param name="view">Свойства пользовательского интерфейса</param>
        /// <param name="g">Свойства игры</param>
        public static void DrawNextShapeField(GameView view, Game g)
        {
            DrawField(view.NextShapeFieldLeft, view.NextShapeFieldTop, g.NextShapeField, view);
        }

        /// <summary>
        /// Вывести на экран Игровое поле
        /// </summary>
        /// <param name="view">Свойства пользовательского интерфейса</param>
        /// <param name="g">Свойства игры</param>
        public static void DrawPlayField(GameView view, Game g)
        {
            DrawField(view.GameFieldLeft, view.GameFieldTop, g.Field, view);
        }

        /// <summary>
        /// Вывести на экран Поле текущего счёта игры
        /// </summary>
        /// <param name="view"></param>
        /// <param name="game"></param>
        public static void DrawScore(GameView view, Game game)
        {
            DrawBorder(view.ScoreLeft, view.ScoreTop, 14, 2, ConsoleColor.Cyan);
            Console.SetCursorPosition(view.ScoreLeft, view.ScoreTop);
            Console.Write("SCORE:{0}", game.Score.Score);
            Console.SetCursorPosition(view.LevelLeft, view.LevelTop);
            Console.Write("LEVEL:{0}", game.Score.Level);
        }

        /// <summary>
        /// Вывести массив точек на игровом поле
        /// </summary>
        /// <param name="points">Массив точек для отрисовки</param>
        /// <param name="kind">Тип фигуры. Нужен для определения цвета точек</param>
        /// <param name="view">Свойства пользовательского интерфейса</param>
        public static void DrawPoints(Point2D[] points, ShapeKind kind, GameView view)
        {
            for (int i = 0; i < points.Length; i++)
            {
                DrawPoint(view.GameFieldLeft + points[i].Left * POINT_WIDTH, view.GameFieldTop + points[i].Top * POINT_HEIGHT, view.ShapeColors[(int)kind]);
            }
        }

        public static void DrawRows(int firstRow, int lastRow, GameField field, GameView view)
        {
            for (int row = lastRow; row >= firstRow; row--)
            {
                for (int column = 0; column < field.Width; column++)
                {
                    DrawPoint(view.GameFieldLeft + column * POINT_WIDTH, view.GameFieldTop + row * POINT_HEIGHT, view.ShapeColors[(int)field.Points[column, row]]);
                }
            }
        }

        public static void DrawCurrentShape(GameView view, Game g)
        {
            DrawShape(view, view.GameFieldLeft, view.GameFieldTop, g.CurrentShape);
        }

        public static void DrawNextShape(GameView view, Game g)
        {
            DrawShape(view, view.NextShapeFieldLeft, view.NextShapeFieldTop, g.NextShape);
        }

        public static void ClearNextShape(GameView view, Game g)
        {
            DrawShape(view, view.NextShapeFieldLeft, view.NextShapeFieldTop, g.NextShape, true);
        }

        /// <summary>
        ///  Вывести заданное поле и его содержимое на экран
        /// </summary>
        /// <param name="left">Левая координата экрана</param>
        /// <param name="top">Верхняя координата экрана</param>
        /// <param name="field">Поле для вывода</param>
        /// <param name="view">Свойства пользовательского интерфейса</param>
        private static void DrawField(int left, int top, GameField field, GameView view)
        {
            DrawBorder(left, top, field.Width * POINT_WIDTH, field.Height * POINT_HEIGHT, ConsoleColor.White);

            for (int deltaX = 0; deltaX < field.Points.GetLength(0); deltaX++)
            {
                for (int deltaY = 0; deltaY < field.Points.GetLength(1); deltaY++)
                {
                    DrawPoint(left + deltaX * POINT_WIDTH, top + deltaY * POINT_HEIGHT, view.ShapeColors[(int)field.Points[deltaX, deltaY]]);
                }
            }
        }

        /// <summary>
        /// Вывести на экран одну "точку" игрового поля
        /// </summary>
        /// <param name="x">Левая координата точки</param>
        /// <param name="y">Верхняя координата точки</param>
        /// <param name="color">Цвет точки</param>
        private static void DrawPoint(int x, int y, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            for (int i = 0; i < POINT_HEIGHT; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(POINT);
            }
        }

        /// <summary>
        /// Нарисовать рамку вокруг заданной области экрана
        /// </summary>
        /// <param name="left">Левая координата поля</param>
        /// <param name="top">Верхняя координата поля</param>
        /// <param name="width">Ширина поля</param>
        /// <param name="height">Высота поля</param>
        /// <param name="borderColor">Цвет рамки</param>
        private static void DrawBorder(int left, int top, int width, int height, ConsoleColor borderColor)
        {
            int padWidth = 1 + width; // один символ левого угла рамки плюс width символов горизонтальной рамки
            string up = "\u2554".PadRight(padWidth, '\u2550') + '\u2557';
            string middle = "\u2551".PadRight(padWidth) + '\u2551';
            string bottom = "\u255A".PadRight(padWidth, '\u2550') + '\u255D';

            Console.ForegroundColor = borderColor;

            Console.SetCursorPosition(left - 1, top - 1);
            Console.WriteLine(up);

            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(left - 1, Console.CursorTop);
                Console.WriteLine(middle);
            }

            Console.SetCursorPosition(left - 1, Console.CursorTop);
            Console.WriteLine(bottom);
        }

        private static void DrawShape(GameView view, int fieldLeft, int fieldTop, Shape shape, bool clear = false)
        {
            ShapeKind kind = clear ? ShapeKind.Empty : shape.Kind;
            ConsoleColor color = view.ShapeColors[(int) kind];

            for (int i = 0; i < shape.Points.Length; i++)
            {
                int left = fieldLeft + (shape.Position.Left + shape.Points[i].Left) * POINT_WIDTH;
                int top = fieldTop + (shape.Position.Top + shape.Points[i].Top) * POINT_HEIGHT;
                DrawPoint(left, top, color);
            }
        }
    }
}
