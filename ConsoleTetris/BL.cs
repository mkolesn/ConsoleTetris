using System;

namespace ConsoleTetris
{
    internal class BL
    {
        static Random rnd = new Random();

        internal static void InitializeApplication(out MenuItem[] menu)
        {
            menu = new MenuItem[5];

            menu[0] = new MenuItem() { Value = GameMenu.StartGame, Enabled = true, Text = "Новая игра" };
            menu[1] = new MenuItem() { Value = GameMenu.ResumeGame, Enabled = false, Text = "Возобновить игру" };
            menu[2] = new MenuItem() { Value = GameMenu.ShowHelp, Enabled = true, Text = "Помощь" };
            menu[3] = new MenuItem() { Value = GameMenu.ShowCredits, Enabled = true, Text = "О программе" };
            menu[4] = new MenuItem() { Value = GameMenu.QuitApplication, Enabled = true, Text = "Выход" };
        }

        internal static Game InitializeGame(int fieldWidth, int fieldHeight, int nextShapeFieldWidth, int nextShapeFieldHeight, int[] levelScore)
        {
            Game game;
            game.Field = new GameField(fieldWidth, fieldHeight);
            game.NextShapeField = new GameField(nextShapeFieldWidth, nextShapeFieldWidth);
            game.NextShape = new Shape(new Point2D(nextShapeFieldWidth / 2, nextShapeFieldHeight / 2), GenerateShapeKind());
            game.CurrentShape = new Shape(new Point2D(fieldWidth / 2, 2), GenerateShapeKind());
            game.Score = new GameScore() { Score = 0, Level = 0 };
            game.LevelScore = (int[])levelScore.Clone();

            return game;
        }

        /// <summary>
        /// Сгенерировать новую фигуру
        /// </summary>
        /// <returns></returns>
        private static ShapeKind GenerateShapeKind()
        {
            return (ShapeKind)((rnd.Next() % (int)ShapeKind.TLetter) + 1);
        }

        public static Shape MoveShape(Shape shape, int deltaX, int deltaY)
        {
            Shape moved = CloneShape(shape);
            moved.Position.Left += deltaX;
            moved.Position.Top += deltaY;
            return moved;
        }

        static Shape CloneShape(Shape shape)
        {
            Shape clone = shape;
            clone.Points = (Point2D[])shape.Points.Clone();
            return clone;
        }

        public static Shape RotateShape(Shape shape)
        {
            Shape rotated = CloneShape(shape);

            // квадратная фигура одинаковая при любых поворотах
            if(rotated.Kind != ShapeKind.Square)
            {
                for(int i = 0; i < rotated.Points.Length; i++)
                {
                    // Вращаем по часовой стрелке
                    int temp = rotated.Points[i].Left;
                    rotated.Points[i].Left = -rotated.Points[i].Top;
                    rotated.Points[i].Top = temp;
                }

            }

            return rotated;
        }

        public static bool IsPositionPossible(GameField field, Shape shape)
        {
            bool positionPossible = true;

            for(int i = 0; i < shape.Points.Length; i++)
            {
                Point2D point = shape.Points[i];
                if(point.Left >= field.Width
                    || point.Left < 0
                    || point.Top >= field.Height
                    || field.Points[point.Left, point.Top] != ShapeKind.Empty)
                {
                    positionPossible = false;
                    break;
                }
            }

            return positionPossible;
        }

    }
}