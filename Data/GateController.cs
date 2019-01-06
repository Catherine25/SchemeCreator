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
        public static void CreateGate(Point p, int id, int newGateInputs, bool[] isInputsReserved) => gates.Add(new Gate(p, newGateInputs, id, isInputsReserved));

        public static void CreateInOut(Point p, int id, bool isReserved) => gates.Add(new Gate(p, id, isReserved));

        //sync from gateInfo
        public static void ReloadGates()
        {
            foreach (GateInfo gInfo in gateInfo)
                if (gInfo.id == (int)Scheme.GateId.IN || gInfo.id == (int)Scheme.GateId.OUT)
                    CreateInOut(gInfo.point, gInfo.id, gInfo.isInputsReserved[0]);
                else
                    CreateGate(gInfo.point, gInfo.id, gInfo.newGateInputs, gInfo.isInputsReserved);
        }
    }
}
