using System.Runtime.Serialization;
using Windows.Foundation;

namespace SchemeCreator.Data {
    [DataContract] public class GateInfo {
        [DataMember] public Point point;
        [DataMember] public Constants.GateEnum type;
        [DataMember] public int newGateInputs;
        [DataMember] public bool[] isInputsReserved;

        public GateInfo() { }

        public GateInfo(Point p, Constants.GateEnum _type, int inputs) {
            point = p;
            type = _type;
            newGateInputs = inputs;
            isInputsReserved = new bool[inputs];
        }
    }
}
