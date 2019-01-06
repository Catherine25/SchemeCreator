using Windows.Foundation;

namespace SchemeCreator.Data
{
    public class GateInfo
    {
        public Point point;
        public int id;
        public int newGateInputs;
        public bool[] isInputsReserved;

        public GateInfo(Point p, int _id, int inputs)
        {
            point = p;
            id = _id;
            newGateInputs = inputs;
            isInputsReserved = new bool[inputs];
        }
    }
}
