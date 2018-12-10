using System;

namespace ConsoleTetris
{
    internal class Game
    {
        static Shape currentShape;
        static Shape nextShape;

        static Random rnd = new Random();

        internal static void StartGame()
        {
            InitializeGame();
            PlayGame();
        }

        private static void PlayGame()
        {
            throw new NotImplementedException();
        }

        private static void InitializeGame()
        {
            nextShape.Kind = GenerateShapeKind();
        }

        /// <summary>
        /// Сгенерировать новую фигуру
        /// </summary>
        /// <returns></returns>
        private static ShapeKind GenerateShapeKind()
        {
            return (ShapeKind)((rnd.Next() % (int)ShapeKind.TLetter) + 1);
        }

        internal static void ResumeGame()
        {
            PlayGame();
        }

        static void DrawShape(Shape shape)
        {
            throw new NotImplementedException();
        }

        static void MoveShape(Shape shape, int dx, int dy)
        {
            throw new NotImplementedException();
        }
    }
}