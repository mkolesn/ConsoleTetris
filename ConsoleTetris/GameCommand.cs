﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    enum GameCommand : sbyte
    {
        NoCommand,
        MoveLeft,
        MoveRight,
        Rotate,
        MoveDown,
        Land,
        QuitToMenu
    }
}
