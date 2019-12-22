using Windows.Foundation;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data.Extensions
{
    public static class LineExtension
    {
        public static Point GetStartPoint(this Line line) =>
            new Point(line.X1, line.Y1);
        public static Point GetEndPoint(this Line line) =>
            new Point(line.X2, line.Y2);
    }
}
