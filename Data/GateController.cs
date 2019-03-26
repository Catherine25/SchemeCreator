using System.Collections.Generic;
using Windows.Foundation;

namespace SchemeCreator.Data {
    public class GateController {
        //constans
        public const int gateHeight = 70, gateWidth = 50;

        //data
        public IList<Gate> gates;
        public IList<GateInfo> gateInfo;

        //constructor
        public GateController() {
            gates = new List<Gate>();
            gateInfo = new List<GateInfo>();
        }

        //function for creating IN, OUT and gate
        //public void CreateGate(Point p, Constants.GateEnum type, int newGateInputs) =>
        //    gates.Add(new Gate(p, newGateInputs, type));

        //public void CreateInOut(Point p, Constants.GateEnum type) =>
        //    gates.Add(new Gate(p, type));
    }
}
