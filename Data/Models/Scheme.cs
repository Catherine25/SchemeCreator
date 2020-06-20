using SchemeCreator.Data.Controllers;
using SchemeCreator.UI;
using System.Collections.Generic;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Models
{
    public class Scheme
    {
        public FrameManager frameManager;
        public LineController lineController = new LineController();
        public GateController gateController = new GateController();
        public DotController dotController = new DotController();

        //constructors
        public Scheme() => frameManager = new FrameManager(this);
        public bool IsAllConnected()
        {
            Stack<Gate> gates = new Stack<Gate>(gateController.Gates);
            int wireCount = lineController.Wires.Count;
            bool found = true;

            while(gates.Count != 0 && found)
            {
                Gate gate = gates.Pop();
                found = false;

                if (gate.Type == GateEnum.IN)
                {
                    for (int i = 0; i < wireCount; i++)
                    {
                        if (gate.WireConnects(lineController.Wires[i].Start))
                        {
                            found = true;
                            break;
                        }
                    }
                }
                else if (gate.Type == GateEnum.OUT)
                {
                    for (int i = 0; i < wireCount; i++)
                    {
                        if (gate.WireConnects(lineController.Wires[i].End))
                        {
                            found = true;
                            break;
                        }
                    }
                }
                else
                {
                    int connections = gate.Inputs + gate.Outputs;
                    for (int i = 0; i < wireCount; i++)
                    {
                        if (gate.WireConnects(lineController.Wires[i].Start))
                            connections--;
                        else if (gate.WireConnects(lineController.Wires[i].End))
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