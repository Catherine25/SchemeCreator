using Windows.Foundation;

namespace SchemeCreator.Data
{
    public class GateInfo
    {
        public Point point;
        public string gateName;
        public int newGateInputs;

        public GateInfo(Point p, string name, int inputs)
        {
            point = p;
            gateName = name;
            newGateInputs = inputs;
        }
    }
}
