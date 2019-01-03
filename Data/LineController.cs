using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data
{
    public static class LineController
    {
        public static List<Line> lines;
        private static double lineStartOffset;
        private static SolidColorBrush darkBrush, lightBrush;
        static LineController()
        {
            lineStartOffset = Scheme.lineStartOffset;
            lines = new List<Line>();
            darkBrush = Scheme.darkBrush;
            lightBrush = Scheme.lightBrush;
        }
        public static void CreateLine(Point p1, Point p2)
        {
            Line line = new Line
            {
                X1 = p1.X,
                Y1 = p1.Y,
                X2 = p2.X,
                Y2 = p2.Y,
                StrokeThickness = lineStartOffset,
                Stroke = darkBrush
            };
            lines.Add(line);
        }
        public static void ColorLine(Line l, bool? value)
        {
            if (value == true)
                l.Stroke = lightBrush;
            else if (value == false)
                l.Stroke = darkBrush;
            l.UpdateLayout();
        }
    }
}