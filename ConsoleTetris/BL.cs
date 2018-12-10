using System;

namespace ConsoleTetris
{
    internal class BL
    {
        internal static void InitializeApplication(out MenuItem[] menu)
        {
            menu = new MenuItem[5];

            menu[0] = new MenuItem() { Value = GameMenu.StartGame, Enabled = true, Text = "Новая игра" };
            menu[1] = new MenuItem() { Value = GameMenu.ResumeGame, Enabled = false, Text = "Возобновить игру" };
            menu[2] = new MenuItem() { Value = GameMenu.ShowHelp, Enabled = true, Text = "Помощь" };
            menu[3] = new MenuItem() { Value = GameMenu.ShowCredits, Enabled = true, Text = "О программе" };
            menu[4] = new MenuItem() { Value = GameMenu.QuitApplication, Enabled = true, Text = "Выход" };
        }
    }
}