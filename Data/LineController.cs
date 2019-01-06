using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data
{
    public static class LineController
    {
        //data
        public static List<Line> lines;
        public static IList<LineInfo> lineInfo;

        //constructor
        static LineController()
        {
            lines = new List<Line>();
            lineInfo = new List<LineInfo>();
        }

        public static Line CreateLine(Point p1, Point p2)
        {
            Line line = new Line
            {
                X1 = p1.X,
                Y1 = p1.Y,
                X2 = p2.X,
                Y2 = p2.Y,
                StrokeThickness = Scheme.lineStartOffset,
                Stroke = Scheme.darkBrush,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round
            };

            lines.Add(line);
            return line;
        }

        public static void ColorLine(Line l, bool? value)
        {
            if (value == true)
                l.Stroke = Scheme.lightBrush;
            else if (value == false)
                l.Stroke = Scheme.darkBrush;
            l.UpdateLayout();
        }

        public static void ReloadLines()
        {
            foreach (LineInfo lInfo in lineInfo)
                CreateLine(lInfo.point1, lInfo.point2);
        }
    }
}