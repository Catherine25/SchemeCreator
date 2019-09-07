using System.Collections.Generic;
using System.Diagnostics;
using SchemeCreator.Data.ConstantsNamespace;

namespace SchemeCreator.Data
{
    static class WorkAlgorithm
    {
        public static Constants.WorkAlgorithmResult Visualize(Scheme scheme)
        {
            Debug.WriteLine("\n" + "[Method] Ver5");

            if (!scheme.IsExternalInputsInited())
                return Constants.WorkAlgorithmResult.exInsNotInited;
            else if (!scheme.IsAllConnected())
                return Constants.WorkAlgorithmResult.gatesNotConnected;

            Queue<Wire> wires = new Queue<Wire>(scheme.lineController.Wires);

            while (wires.Count != 0)
            {
                Wire wire = wires.Dequeue();
                Debug.WriteLine("Dequed a wire");

                Gate startGate = scheme.gateController.GetGateByWireStart(wire.start);

                if(startGate.type == Constants.GateEnum.IN)
                {
                    Debug.WriteLine("it's exIN");

                    wire.isActive = startGate.values[0];
                }
                else
                {
                    Debug.WriteLine("it's a logic gate");

                    if(startGate.FirstFreeValueBoxIndex() == -1)
                    {
                        Debug.WriteLine("it's valid");

                        wire.isActive = startGate.values[0];
                    }
                    else
                    {
                        Debug.WriteLine("it's not valid yet");

                        wires.Enqueue(wire);

                        continue;
                    }
                }

                Gate endGate = scheme.gateController.GetGateByWireEnd(wire.end);

                if (endGate.type == Constants.GateEnum.OUT)
                {
                    Debug.WriteLine("it's exOUT");

                    endGate.values[0] = wire.isActive;
                }
                else
                {
                    Debug.WriteLine("it's a logic gate");

                    int box = endGate.FirstFreeValueBoxIndex();

                    endGate.values[box] = wire.isActive;

                    if (box == endGate.inputs - 1)
                        endGate.Work();
                }
            }

            return Constants.WorkAlgorithmResult.correct;
        }
    }
}
