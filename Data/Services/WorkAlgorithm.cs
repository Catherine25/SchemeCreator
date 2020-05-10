using System.Collections.Generic;
using SchemeCreator.Data.ConstantsNamespace;
using SchemeCreator.Data.Models;

namespace SchemeCreator.Data.Services
{
    static class WorkAlgorithm
    {
        public static Constants.WorkAlgorithmResult Visualize(Scheme scheme)
        {
            //Debug.WriteLine("\n" + "[Method] Ver5");

            if (scheme.gateController.GetFirstNotInitedGate() != null)
                return Constants.WorkAlgorithmResult.exInsNotInited;
            else if (!scheme.IsAllConnected())
                return Constants.WorkAlgorithmResult.gatesNotConnected;

            Queue<Wire> wires = new Queue<Wire>(scheme.lineController.Wires);

            int connectionNotFound = 0;
            while (wires.Count != 0)
            {
                Wire wire = wires.Dequeue();
                //Debug.WriteLine("Dequed a wire");

                Gate startGate = scheme.gateController.GetGateByWireStart(wire.start);

                if(startGate.type == Constants.GateEnum.IN)
                {
                    //Debug.WriteLine("it's exIN");

                    connectionNotFound = 0;

                    wire.isActive = startGate.values[0];
                }
                else
                {
                    //Debug.WriteLine("it's a logic gate");

                    if(startGate.FirstFreeValueBoxIndex() == -1)
                    {
                        //Debug.WriteLine("it's valid");

                        connectionNotFound = 0;

                        wire.isActive = startGate.values[0];
                    }
                    else
                    {
                        //Debug.WriteLine("it's not valid yet");

                        connectionNotFound++;

                        wires.Enqueue(wire);

                        if (connectionNotFound > wires.Count)
                            return Constants.WorkAlgorithmResult.schemeIsntCorrect;

                        continue;
                    }
                }

                Gate endGate = scheme.gateController.GetGateByWireEnd(wire.end);

                if (endGate.type == Constants.GateEnum.OUT)
                {
                    //Debug.WriteLine("it's exOUT");

                    connectionNotFound = 0;

                    endGate.values[0] = wire.isActive;
                }
                else
                {
                    //Debug.WriteLine("it's a logic gate");

                    int box = endGate.FirstFreeValueBoxIndex();

                    connectionNotFound = 0;

                    endGate.values[box] = wire.isActive;

                    if (box == endGate.inputs - 1)
                        endGate.Work();
                }
            }

            return Constants.WorkAlgorithmResult.correct;
        }
    }
}
