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
            Points = BL.AllShapes[(int)kind];
        }
    }
}