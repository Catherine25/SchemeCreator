using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Models;
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data.Controllers
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
            wires.Find(x => x.Start == line.GetStartPoint() && x.End == line.GetEndPoint());
    }
}