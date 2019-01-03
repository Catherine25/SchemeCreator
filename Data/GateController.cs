using System.Collections.Generic;
using Windows.Foundation;

namespace SchemeCreator.Data
{
    public static class GateController
    {
        //constans
        public const int gateHeight = 70;
        public const int gateWidth = 50;

        //data
        public static List<Gate> gates;

        //constructor
        static GateController() => gates = new List<Gate>();

        public static void CreateGate(Point p, string gateName, int newGateInputs)
        {
            Gate gate = new Gate(p, gateName, newGateInputs);
            gates.Add(gate);
        }
        public static void CreateInOut(Point p, string gateName)
        {
            Gate gate = new Gate(p, gateName);
            gates.Add(gate);
        }
    }
}
