namespace ConsoleTetris
{
    struct Shape
    {
        public ShapeKind Kind;
        public Point2D Position;
        public Point2D[] Points;

        public Shape(Point2D position, ShapeKind kind)
        {
            Kind = kind;
            Position = position;

            switch(kind)
            {
                case ShapeKind.GLeft:
                    Points = new Point2D[] { new Point2D(0, 0), new Point2D(-1, 0), new Point2D(0, 1), new Point2D(0, 2) };
                    break;
                case ShapeKind.GRight:
                    Points = new Point2D[] { new Point2D(0, 0), new Point2D(1, 0), new Point2D(0, 1), new Point2D(0, 2) };
                    break;
                case ShapeKind.SnakeLeft:
                    Points = new Point2D[] { new Point2D(0, 0), new Point2D(1, 0), new Point2D(0, 1), new Point2D(0, 2) };
                    break;
                case ShapeKind.SnakeRight:
                    Points = new Point2D[] { new Point2D(0, 0), new Point2D(1, 0), new Point2D(0, 1), new Point2D(0, 2) };
                    break;
                case ShapeKind.Square:
                    Points = new Point2D[] { new Point2D(0, 0), new Point2D(1, 0), new Point2D(0, 1), new Point2D(0, 2) };
                    break;
                case ShapeKind.Strait:
                    Points = new Point2D[] { new Point2D(0, 0), new Point2D(1, 0), new Point2D(0, 1), new Point2D(0, 2) };
                    break;
                case ShapeKind.TLetter:
                    Points = new Point2D[] { new Point2D(0, 0), new Point2D(1, 0), new Point2D(0, 1), new Point2D(0, 2) };
                    break;
                default:
                    Points = new Point2D[0];
                    break;
            }
        }
    }
}