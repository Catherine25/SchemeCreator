using System.Runtime.Serialization;
using Windows.Foundation;

namespace SchemeCreator.Data {
    [DataContract] public class GateInfo {
        [DataMember] public Point point;
        [DataMember] public int id;
        [DataMember] public int newGateInputs;
        [DataMember] public bool[] isInputsReserved;

        public GateInfo() { }

        public GateInfo(Point p, int _id, int inputs) {
            point = p;
            id = _id;
            newGateInputs = inputs;
            isInputsReserved = new bool[inputs];
        }
    }
}
