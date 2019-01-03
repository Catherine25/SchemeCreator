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
        public static IList<Gate> gates;
        public static IList<GateInfo> gateInfo;

        //constructor
        static GateController()
        {
            gates = new List<Gate>();
            gateInfo = new List<GateInfo>();
        }

        //function for creating IN, OUT and gate
        public static void CreateGate(Point p, string gateName, int newGateInputs) => gates.Add(new Gate(p, gateName, newGateInputs));

        public static void CreateInOut(Point p, string gateName) => gates.Add(new Gate(p, gateName));

        //sync from gateInfo
        public static void ReloadGates()
        {
            foreach (GateInfo gInfo in gateInfo)
                if (gInfo.newGateInputs == 0)
                    CreateInOut(gInfo.point, gInfo.gateName);
                else
                    CreateGate(gInfo.point, gInfo.gateName, gInfo.newGateInputs);
        }
    }
}
