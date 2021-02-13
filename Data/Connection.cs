using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data
{
    public class Connection
    {
        public GateView gate;
        public GatePortView port;
        public ExternalPortView externalPort;

        public void Clear()
        {
            gate = null;
            port = null;
            externalPort = null;
        }
    }

    public class ConnectionPair
    {
        public Connection Start;
        public Connection End;
        public bool? Value;
    }
}
