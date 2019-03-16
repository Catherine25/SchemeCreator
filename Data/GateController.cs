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
        public void CreateGate(Point p, int id, int newGateInputs, bool[] isInputsReserved) => gates.Add(new Gate(p, newGateInputs, id, isInputsReserved));

        public void CreateInOut(Point p, int id, bool isReserved) => gates.Add(new Gate(p, id, isReserved));

        //sync from gateInfo
        public void ReloadGates() {
            if (gateInfo != null)
                foreach (GateInfo gInfo in gateInfo)
                    if (gInfo.id == (int)SchemeCreator.Constants.GateEnum.IN ||
                    gInfo.id == (int)SchemeCreator.Constants.GateEnum.OUT)
                        CreateInOut(gInfo.point, gInfo.id, gInfo.isInputsReserved[0]);
                    else
                        CreateGate(gInfo.point, gInfo.id, gInfo.newGateInputs, gInfo.isInputsReserved);
        }
    }
}
