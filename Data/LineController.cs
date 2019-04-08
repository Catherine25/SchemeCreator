using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data {
    public class LineController {
        //data
        public List<Wire> wires = new List<Wire>();

        //constructor
        public LineController() { }

        public void CreateLine(Point p1, Point p2) =>
            wires.Add(new Wire { start = p1, end = p2 });

        public void ColorLine(Line l, bool? value) {
            if (value == true)
                l.Stroke = SchemeCreator.Constants.brushes[Constants.AccentEnum.light1];
            else if (value == false)
                l.Stroke = SchemeCreator.Constants.brushes[Constants.AccentEnum.dark1];
            l.UpdateLayout();
        }

        public void ReloadLines() {
            foreach (Wire w in wires)
                CreateLine(w.start, w.end);
        }
    }
}