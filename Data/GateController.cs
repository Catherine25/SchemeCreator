using System.Collections.Generic;
using Windows.Foundation;

namespace SchemeCreator.Data {
    public class GateController {
        //constans
        public const int gateHeight = 70, gateWidth = 50;

        //data
        public IList<Gate> gates;

        //constructor
        public GateController() =>
            gates = new List<Gate>();
    }
}
