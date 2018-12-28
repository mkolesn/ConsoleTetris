namespace ConsoleTetris
{
    /// <summary>
    /// Тип фигуры тетрис
    /// </summary>
    enum ShapeKind
    {
        Empty,
        GLeft = 1,      // Г правая
        GRight = 2,     // Г левая
        SnakeLeft = 3,  // змейка вправо
        SnakeRight = 4, // змейка влево
        Square = 5,     // квадрат
        Strait = 6,     // прямая
        TLetter = 7     // буква Т
    }
}