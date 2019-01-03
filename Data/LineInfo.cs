using Windows.Foundation;

namespace SchemeCreator.Data
{
    public class LineInfo
    {
        public Point point1, point2;

        public LineInfo(Point p1, Point p2)
        {
            point1 = p1;
            point2 = p2;
        }
    }
}
