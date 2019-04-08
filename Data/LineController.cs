using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data {
    public class LineController {
        //data
        List<Wire> wires = new List<Wire>();

        public Wire getWireByIndex(int index) => wires[index];
        public int getWireCount() => wires.Count;
        public void addWire(Wire w) => wires.Add(w);

        public void createWireByPoints(Point p1, Point p2) =>
            wires.Add(new Wire { start = p1, end = p2 });

        public void colorLineByValues(Line l, bool? value) {
            if (value == true)
                l.Stroke = SchemeCreator.Constants.brushes[Constants.AccentEnum.light1];
            else if (value == false)
                l.Stroke = SchemeCreator.Constants.brushes[Constants.AccentEnum.dark1];
            l.UpdateLayout();
        }

        public void reloadLines() {
            foreach (Wire w in wires)
                createWireByPoints(w.start, w.end);
        }
    }
}