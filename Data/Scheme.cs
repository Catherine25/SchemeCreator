using System.Collections.Generic;
using static SchemeCreator.Data.ConstantsNamespace.Constants;

namespace SchemeCreator.Data
{
    public class Scheme
    {
        public UI.FrameManager frameManager;
        public LineController lineController = new LineController();
        public GateController gateController = new GateController();
        public DotController dotController = new DotController();

        //constructors
        public Scheme() => frameManager = new UI.FrameManager(this);

        public bool IsExternalInputsInited()
        {
            IList<Gate> exINs = gateController.GetExternalInputs();

            //check is all exINs are inited
            for (int i = 0; i < exINs.Count; i++)
                if (exINs[i].values[0] == null)
                    return false;

            return true;
        }

        public bool IsAllConnected()
        {
            Stack<Gate> gates = new Stack<Gate>(gateController.Gates);
            int wireCount = lineController.Wires.Count;
            bool found = true;

            while(gates.Count != 0 && found)
            {
                Gate gate = gates.Pop();
                found = false;

                if (gate.type == GateEnum.IN)
                {
                    for (int i = 0; i < wireCount; i++)
                    {
                        if (gate.WireConnects(lineController.Wires[i].start))
                        {
                            found = true;
                            break;
                        }
                    }
                }
                else if (gate.type == GateEnum.OUT)
                {
                    for (int i = 0; i < wireCount; i++)
                    {
                        if (gate.WireConnects(lineController.Wires[i].end))
                        {
                            found = true;
                            break;
                        }
                    }
                }
                else
                {
                    int connections = gate.inputs + gate.outputs;
                    for (int i = 0; i < wireCount; i++)
                    {
                        if (gate.WireConnects(lineController.Wires[i].start))
                            connections--;
                        else if (gate.WireConnects(lineController.Wires[i].end))
                            connections--;
                    }
                    if (connections == 0)
                        found = true;
                }
            }

            if (gates.Count == 0)
                return true;
            else return false;
        }
    }
}