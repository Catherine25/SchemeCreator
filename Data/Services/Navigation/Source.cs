using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Services.Navigation
{
    public class Source
    {
        public GateView Gate;
        public ExternalPortView Port;

        public Source(GateView gate, ExternalPortView port)
        {
            Gate = gate;
            Port = port;
        }

        public override string ToString()
        {
            return Gate == null ? $"{Port.Type} port, at [{Port.MatrixLocation.X},{Port.MatrixLocation.Y}]" : $"{Gate.Type} gate, at [{Gate.MatrixLocation.X}, {Gate.MatrixLocation.Y}]";
        }
    }
}
