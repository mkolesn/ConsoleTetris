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

        static void Main(string[] args)
        {
            MenuItem[] menu;

            BL.InitializeApplication(out menu);
            UI.Initialize();
            UI.ShowSplashScreen(SPLASH_SCREEN_DELAY);

            GameMenu menuItemSelected;
            int currentItem = 0;

            do
            {
                menuItemSelected = UI.GetMenuSelection(menu, ref currentItem);

                switch (menuItemSelected)
                {
                    case GameMenu.StartGame:
                        Game.StartGame();
                        break;
                    case GameMenu.ResumeGame:
                        Game.ResumeGame();
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
