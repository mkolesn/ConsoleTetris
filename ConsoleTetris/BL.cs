using System;

namespace ConsoleTetris
{
    internal class BL
    {
        static Random rnd = new Random();
        public static Point2D[][] AllShapes = {
                 new Point2D[] { }, // Empty
                 new Point2D[] { new Point2D(-1, -1), new Point2D(0, -1), new Point2D(0, 0), new Point2D(0, 1) },   // GLeft
                 new Point2D[] { new Point2D(1, -1), new Point2D(0, -1), new Point2D(0, 0), new Point2D(0, 1) },    // GRight
                 new Point2D[] { new Point2D(-1, 0), new Point2D(0, 0), new Point2D(0, 1), new Point2D(1, 1) }, // SnakeLeft
                 new Point2D[] { new Point2D(-1, 1), new Point2D(0, 1), new Point2D(0, 0), new Point2D(1, 0) },  // SnakeRight
                 new Point2D[] { new Point2D(0, 0), new Point2D(1, 0), new Point2D(0, 1), new Point2D(1, 1) }, // Square
                 new Point2D[] { new Point2D(0, -1), new Point2D(0, 0), new Point2D(0, 1), new Point2D(0, 2) },  // Strait
                 new Point2D[] { new Point2D(-1, 0), new Point2D(0, 0), new Point2D(1, 0), new Point2D(0, -1) }  // TLetter
        };

        internal static void InitializeApplication(out MenuItem[] menu)
        {
            menu = new MenuItem[5];

            menu[0] = new MenuItem() { Value = GameMenu.StartGame, Enabled = true, Text = "Новая игра" };
            menu[1] = new MenuItem() { Value = GameMenu.ResumeGame, Enabled = false, Text = "Возобновить игру" };
            menu[2] = new MenuItem() { Value = GameMenu.ShowHelp, Enabled = true, Text = "Помощь" };
            menu[3] = new MenuItem() { Value = GameMenu.ShowCredits, Enabled = true, Text = "О программе" };
            menu[4] = new MenuItem() { Value = GameMenu.QuitApplication, Enabled = true, Text = "Выход" };
        }

        internal static Game InitializeGame(int fieldWidth, int fieldHeight, int nextShapeFieldWidth, int nextShapeFieldHeight)
        {
            Game game;
            game.Field = new GameField(fieldWidth, fieldHeight);
            game.NextShapeField = new GameField(nextShapeFieldWidth, nextShapeFieldWidth);
            game.NextShape = new Shape(new Point2D(nextShapeFieldWidth / 2, nextShapeFieldHeight / 2), GenerateShapeKind());
            game.shapeStart = new Point2D(fieldWidth / 2, 1);
            game.CurrentShape = new Shape(game.shapeStart, GenerateShapeKind());
            game.Score = new GameScore() { Score = 0, Level = 0 };

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

        private static void SetShapeKind(ref Shape sh, ShapeKind kind)
        {
            sh.Kind = kind;
            sh.Points = AllShapes[(int)kind];
        }

        public static void SetNextShape(ref Game g)
        {
            g.CurrentShape.Position = g.shapeStart;
            SetShapeKind(ref g.CurrentShape, g.NextShape.Kind);
            SetShapeKind(ref g.NextShape, GenerateShapeKind());
        }

        public static void MoveShape(ref Shape sh, int deltaX, int deltaY, out Point2D[] makeEmpty, out Point2D[] makeFilled)
        {
            sbyte nextIsFoundMask = 0;
            sbyte previousIsFoundMask = 0;
            byte matchCounter = 0; // какое количество точек при перемещении попало на другую точку фигуры (не на пустую точку)

            for (int i = sh.Points.Length - 1; i >= 1; i--)
            {
                if (((1 << i) & nextIsFoundMask) == 0)
                {
                    var nextLeft = sh.Points[i].Left + deltaX;
                    var nextTop = sh.Points[i].Top + deltaY;
                    // ещё не обнаруживалось, чтобы для текущей точки какая-либо точка была следующей
                    // ищем, существует ли в фигуре точка в той координате, куда перемещается текущая точка
                    for (int j = 0; j < i; j++)
                    {
                        if (nextLeft == sh.Points[j].Left
                            && nextTop == sh.Points[j].Top)
                        {
                            // в фигуре существует точка в той координате, куда перемещается текущая точка
                            nextIsFoundMask |= (sbyte)(1 << i);
                            previousIsFoundMask |= (sbyte)(1 << j);
                            matchCounter++;
                            break;
                        }
                    }
                }

                if (((1 << i) & previousIsFoundMask) == 0)
                {
                    var previousLeft = sh.Points[i].Left - deltaX;
                    var previousTop = sh.Points[i].Top - deltaY;
                    // ещё не обнаруживалось, чтобы для текущей точки какая-либо точка была предыдущей
                    // ищем, существует ли в фигуре точка, которая переместится текущую точку
                    for (int j = 0; j < i; j++)
                    {
                        if (previousLeft == sh.Points[j].Left
                            && previousTop == sh.Points[j].Top)
                        {
                            // в фигуре существует точка, которая переместится в текущую точку
                            previousIsFoundMask |= (sbyte)(1 << i);
                            nextIsFoundMask |= (sbyte)(1 << j);
                            matchCounter++;
                            break;
                        }
                    }
                }
            }

            makeEmpty = new Point2D[sh.Points.Length - matchCounter];
            makeFilled = new Point2D[sh.Points.Length - matchCounter];

            // заполняем массив точек, которые стали пустыми
            for (int i = 0, index = 0; i < sh.Points.Length; i++)
            {
                if (((1 << i) & previousIsFoundMask) == 0)
                {
                    Point2D p = sh.Points[i];
                    makeEmpty[index++] = new Point2D(sh.Position.Left + p.Left, sh.Position.Top + p.Top);
                }
            }

            sh.Position.Left += deltaX;
            sh.Position.Top += deltaY;

            // заполняем массив точек, которые стали заполненными
            for (int i = 0, index = 0; i < sh.Points.Length; i++)
            {
                if (((1 << i) & nextIsFoundMask) == 0)
                {
                    Point2D p = sh.Points[i];
                    makeFilled[index++] = new Point2D(sh.Position.Left + p.Left, sh.Position.Top + p.Top);
                }
            }
        }

        internal static void CalcChangedPoints(Shape from, Shape to, out Point2D[] makeEmpty, out Point2D[] makeFilled)
        {
            Point2D[] fromPoints = ToFieldPoints(from);
            Point2D[] toPoints = ToFieldPoints(to);
            makeEmpty = SubtractPointSet(fromPoints, toPoints);
            makeFilled = SubtractPointSet(toPoints, fromPoints);
        }

        /// <summary>
        /// Преобразовать массив точек фигуры в массив точек с координатами игрового поля
        /// </summary>
        /// <param name="sh">Исходная фигура</param>
        /// <returns>Массив точек с координатами игрового поля</returns>
        static Point2D[] ToFieldPoints(Shape sh)
        {
            Point2D[] p = (Point2D[])sh.Points.Clone();

            for (int i = 0; i < p.Length; i++)
            {
                p[i].Top += sh.Position.Top;
                p[i].Left += sh.Position.Left;
            }

            return p;
        }


        /// <summary>
        /// Из одного множества точек вычесть второе
        /// </summary>
        /// <param name="original">Исходное множество точек</param>
        /// <param name="shapeToSubtract">Вычитаемое множество точек</param>
        /// <returns>Массив точек, относящихся к первому множеству, которые отсутствуют в вычитаемом множестве</returns>
        public static Point2D[] SubtractPointSet(Point2D[] original, Point2D[] setToSubtract)
        {
            Point2D[] result = new Point2D[original.Length];
            int found = 0;

            for (int i = 0; i < original.Length; i++)
            {
                bool pointExists = false;

                for (int j = 0; j < setToSubtract.Length; j++)
                {
                    if (original[i].Left == setToSubtract[j].Left
                        && original[i].Top == setToSubtract[j].Top)
                    {
                        pointExists = true;
                        break;
                    }
                }

                if (!pointExists)
                {
                    result[found++] = original[i];
                }
            }

            // Если результирующее множество точек меньше оригинального, то уменьшаем размер результирующего массива точек
            if (found < original.Length)
            {
                Array.Resize(ref result, found);
            }

            return result;
        }

        private static bool RowIsEmpty(GameField field, int row)
        {
            bool empty = true;

            for (int column = 0; column < field.Width; column++)
            {
                if (field.Points[column, row] != ShapeKind.Empty)
                {
                    empty = false;
                    break;
                }
            }

            return empty;
        }

        private static bool RowHasEmpty(GameField field, int row)
        {
            bool hasEmpty = false;

            for (int column = 0; column < field.Width; column++)
            {
                if (field.Points[column, row] == ShapeKind.Empty)
                {
                    hasEmpty = true;
                    break;
                }
            }

            return hasEmpty;
        }

        internal static bool HasFilledRows(int minShapeRow, int maxShapeRow, GameField field, out int[] filledRows)
        {
            int filledRowsCount = 0;
            filledRows = new int[maxShapeRow - minShapeRow + 1];

            for (int row = minShapeRow; row <= maxShapeRow; row++)
            {
                if (!RowHasEmpty(field, row))
                {
                    filledRows[filledRowsCount++] = row;
                }
            }

            if (filledRowsCount < filledRows.Length)
            {
                Array.Resize(ref filledRows, filledRowsCount);
            }

            return (filledRowsCount > 0);
        }

        internal static int FirstEmptyRow(GameField field, int startRow)
        {
            int firstRow = -1;

            for (int row = startRow; row >= 0; row--)
            {
                if (RowIsEmpty(field, row))
                {
                    firstRow = row;
                    break;
                }
            }

            return firstRow;
        }

        /// <summary>
        /// Вернуть, сколько очеов полагается за заданное количество заполненных линий
        /// </summary>
        /// <param name="removedRowsCount"></param>
        /// <returns></returns>
        internal static int CalcScore(int removedRowsCount)
        {
            int score = 0;

            //1 линия — 100 очков, 2 линии — 300 очков, 3 линии — 700 очков, 4 линии — 1500 очков
            switch (removedRowsCount)
            {
                case 1:
                    score = 100;
                    break;
                case 2:
                    score = 300;
                    break;
                case 3:
                    score = 700;
                    break;
                case 4:
                    score = 1500;
                    break;
            }

            return score;
        }

        internal static void RemoveRows(int[] filledRows, GameField field)
        {
            /*    int maxFilledRow = filledRows[filledRows.Length - 1];
                int min
                for (int column = 0; column < length; column++)
                {

                }*/
            throw new NotImplementedException();
        }

        private static Shape CloneShape(Shape shape)
        {
            Shape clone = shape;
            clone.Points = (Point2D[])shape.Points.Clone();
            return clone;
        }

        public static Shape RotateShape(Shape shape)
        {
            Shape rotated = CloneShape(shape);

            // квадратная фигура одинаковая при любых поворотах
            if (rotated.Kind != ShapeKind.Square)
            {
                for (int i = 0; i < rotated.Points.Length; i++)
                {
                    // Вращаем по часовой стрелке
                    int temp = rotated.Points[i].Left;
                    rotated.Points[i].Left = -rotated.Points[i].Top;
                    rotated.Points[i].Top = temp;
                }

            }

            return rotated;
        }

        public static bool IsPositionPossible(GameField field, Shape shape, int leftShift = 0, int topShift = 0)
        {
            bool positionPossible = true;

            for (int i = 0; i < shape.Points.Length; i++)
            {
                var left = shape.Position.Left + shape.Points[i].Left + leftShift;
                var top = shape.Position.Top + shape.Points[i].Top + topShift;

                if (left >= field.Width
                    || left < 0
                    || top >= field.Height
                    || field.Points[left, top] != ShapeKind.Empty)
                {
                    positionPossible = false;
                    break;
                }
            }

            return positionPossible;
        }



        /// <summary>
        /// Вычислить, приземлилась ли фигура
        /// </summary>
        /// <param name="sh">Анализируемая фигура</param>
        /// <param name="field">Игровое поле</param>
        /// <returns>Истина, если фигура приземлилась</returns>
        public static bool IsShapeLanded(Shape sh, GameField field)
        {
            bool isLanded = false;

            for (int i = 0; i < sh.Points.Length; i++)
            {
                int left = sh.Position.Left + sh.Points[i].Left;
                int top = sh.Position.Top + sh.Points[i].Top;
                if (top == field.Height - 1
                    || field.Points[left, top + 1] != ShapeKind.Empty)
                {
                    // если текущая точка фигуры достигла дна или под ней находится непустая точка поля,
                    // то это значит, что фигура "приземлилась"
                    isLanded = true;
                    break;
                }
            }

            return isLanded;
        }

        internal static void AppendShape(Shape sh, GameField field, out int minShapeRow, out int maxShapeRow)
        {
            maxShapeRow = 0;
            minShapeRow = field.Height - 1;

            for (int i = 0; i < sh.Points.Length; i++)
            {
                int row = sh.Position.Top + sh.Points[i].Top;
                int column = sh.Position.Left + sh.Points[i].Left;
                field.Points[column, row] = sh.Kind;

                if (row < minShapeRow)
                {
                    minShapeRow = row;
                }

                if (row > maxShapeRow)
                {
                    maxShapeRow = row;
                }
            }

        }

        internal static void RemoveFilledRow(int firstRow, int lastRow, GameField field)
        {
            for (int row = lastRow; row >= firstRow; row--)
            {
                for (int column = 0; column < field.Width; column++)
                {
                    field.Points[column, row] = field.Points[column, row - 1];
                }
            }
        }
    }
}