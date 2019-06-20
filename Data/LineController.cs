using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data {
    public class LineController {

        //data
        List<Wire> wires;
        public List<Wire> Wires {
            get => wires;
            set => wires = value;
        }

        //constructor
        public LineController() => wires = new List<Wire>();

        // public void colorLineByValues(Line l, bool? value) {
        //     if (value == true)
        //         l.Stroke = SchemeCreator.Constants.brushes[Constants.AccentEnum.light1];
        //     else if (value == false)
        //         l.Stroke = SchemeCreator.Constants.brushes[Constants.AccentEnum.dark1];
        //     l.UpdateLayout();
        // }

        // public void reloadLines() {
        //     foreach (Wire w in Wires)
        //         Wires.Add(new Wire {
        //             start = w.start,
        //             end = w.end });
        // }
    }
}