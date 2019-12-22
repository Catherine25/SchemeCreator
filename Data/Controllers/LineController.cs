using SchemeCreator.Data.Extensions;
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data
{
    public class LineController
    {
        //data
        private List<Wire> wires;

        public List<Wire> Wires
        {
            get => wires;
            set => wires = value;
        }

        //constructor
        public LineController() => wires = new List<Wire>();

        //methods
        public Wire FindWireByLine(Line line) =>
            wires.Find(x => x.start == line.GetStartPoint() && x.end == line.GetEndPoint());
    }
}