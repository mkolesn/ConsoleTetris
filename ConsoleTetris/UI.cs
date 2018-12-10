using System;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal class UI
    {
        const int MENU_LEFT = 10;
        const int MENU_TOP = 10;

        internal static void Initialize()
        {
            Console.CursorVisible = false;
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
                if (Console.ForegroundColor == ConsoleColor.White)
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
            } while ((delaySeconds == 0 || d.AddSeconds(delaySeconds) > DateTime.Now)
                    && !Console.KeyAvailable);

            if (Console.KeyAvailable)
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
                } while (!Console.KeyAvailable);

                // Обрабатываем пользовательский ввод
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
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

            } while (!itemConfirm);

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
                if (newCurrent == 0)
                {
                    newCurrent = menu.Length - 1;
                }
                else
                {
                    newCurrent--;
                }
            } while (!menu[newCurrent].Enabled && newCurrent != currentItem); // если нашли разрешённый пункт меню или дошли по кругу до исходного пункта, то выходим из цикла

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
                if (newCurrent == menu.Length - 1)
                {
                    newCurrent = 0;
                }
                else
                {
                    newCurrent++;
                }
            } while (!menu[newCurrent].Enabled && newCurrent != currentItem); // если нашли разрешённый пункт меню или дошли по кругу до исходного пункта, то выходим из цикла

            currentItem = newCurrent;
        }
    }
}