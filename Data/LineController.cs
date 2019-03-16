using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data {
    public class LineController {
        //data
        public List<Line> lines;
        public IList<LineInfo> lineInfo;

        //constructor
        public LineController() {
            lines = new List<Line>();
            lineInfo = new List<LineInfo>();
        }

        public Line CreateLine(Point p1, Point p2) {
            Line line = new Line {
                X1 = p1.X,
                Y1 = p1.Y,
                X2 = p2.X,
                Y2 = p2.Y,
                StrokeThickness = SchemeCreator.Constants.lineStartOffset,
                Stroke = SchemeCreator.Constants.brushes[Constants.AccentEnum.dark1],
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round
            };

            lines.Add(line);
            return line;
        }

        public void ColorLine(Line l, bool? value) {
            if (value == true)
                l.Stroke = SchemeCreator.Constants.brushes[Constants.AccentEnum.light1];
            else if (value == false)
                l.Stroke = SchemeCreator.Constants.brushes[Constants.AccentEnum.dark1];
            l.UpdateLayout();
        }

        public void ReloadLines() {
            foreach (LineInfo lInfo in lineInfo)
                CreateLine(lInfo.point1, lInfo.point2);
        }
    }
}