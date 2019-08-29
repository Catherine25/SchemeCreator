using System.Collections.Generic;
using System.Diagnostics;
using SchemeCreator.Data.ConstantsNamespace;

namespace SchemeCreator.Data
{
    static class WorkAlgorithm
    {
        public static int FindMaxNumber(int[] array)
        {
            int maxNumber = 0;
            int length = array.Length;

            for (int i = 0; i < length; i++)
                if (maxNumber < array[i])
                    maxNumber = array[i];

            return maxNumber;
        }

        public static bool IsExternalInputsInited(Scheme scheme)
        {
            IList<Gate> exINs = scheme.gateController.GetExternalInputs();

            //check is all exINs are inited
            for (int i = 0; i < exINs.Count; i++)
                if (exINs[i].values[0] == null)
                    return false;

            return true;
        }

        public static bool IsAllConnected(Scheme scheme)
        {
            int wLength = scheme.lineController.Wires.Count;
            int gLength = scheme.gateController.Gates.Count;
            int inputsCount = 0;

            for (int g = 0; g < gLength; g++)
            {
                inputsCount += scheme.gateController.Gates[g].inputs;
                inputsCount += scheme.gateController.Gates[g].outputs;
            }

            int counter = 0;

            for (int i = 0; i < wLength; i++)
            {
                for (int j = 0; j < gLength; j++)
                {
                    Gate g = scheme.gateController.Gates[j];

                    Wire w = scheme.lineController.Wires[i];

                    if (g.WireConnects(w.start) || g.WireConnects(w.end))
                        counter++;
                }
            }

            if (inputsCount == counter)
                return true;
            else
                return false;

        }

        public static void Ver2(List<HistoryComponent> tracedComponentsHistory, Scheme scheme)
        {
            int length = tracedComponentsHistory.Count;
            bool? value = false;

            for (int i = 0; i < length; i++)
            {
                int historyIndex = tracedComponentsHistory[i].index;
                Debug.Write("\nhistoryIndex = " + historyIndex);

                //if it's gate
                Debug.Write("\ncomponent is: ");
                if (tracedComponentsHistory[i].component == Constants.ComponentTypeEnum.gate)
                {
                    Debug.Write("gate;");
                    Gate gate = scheme.gateController.Gates[historyIndex];

                    Debug.Write(" type:");
                    if (gate.type == Constants.GateEnum.IN)
                    {
                        Debug.Write(" IN");
                        //load value
                        value = gate.values[0];
                        Debug.Write("\n" + value + " value loaded");
                    }
                    else if (gate.type == Constants.GateEnum.OUT)
                    {
                        Debug.Write(" OUT");
                        //set value
                        gate.values[0] = value;
                        Debug.Write("\n" + value + " set");
                    }
                    else
                    {
                        Debug.Write(" " + gate.type);
                        //logic gate
                        int valueLength = gate.values.Count;
                        //bool found = false;

                        int index = gate.firstFreeValueBoxIndex();
                        Debug.Write("\nfirstFreeValueBoxIndex() returned " + index);


                        //TODO CHECK CODE

                        if (index == gate.values.Count - 1)
                        {
                            gate.values[index] = value;
                            Debug.Write("\n" + value + " set");
                            gate.Work();
                            value = gate.values[0];
                            Debug.Write("\n" + value + " loaded");
                        }
                        else
                        {
                            gate.values[index] = value;
                            Debug.Write("\n" + value + " set");
                        }
                    }
                }
                //if it's wire
                else
                {
                    Debug.Write("wire");
                    //REDRAW
                    Wire w = scheme.lineController.Wires[historyIndex];
                    w.isActive = value;
                    Debug.Write("\n" + value + " set");
                }
            }
        }

        public static void Ver3(List<HistoryComponent> historyComponents, Scheme scheme)
        {
            Queue<HistoryComponent> components = new Queue<HistoryComponent>(historyComponents);

            bool? value = null;

            while(components.Count != 0)
            {
                HistoryComponent component = components.Dequeue();

                Debug.Write("\n");
                Debug.Write(" component: " + component.component + " index: " + component.index);

                if(component.component == Constants.ComponentTypeEnum.gate)
                {
                    Gate gate = scheme.gateController.Gates[component.index];

                    if (gate.type == Constants.GateEnum.IN)
                        value = gate.values[0];
                    else if (gate.type == Constants.GateEnum.OUT)
                        gate.values[0] = value;
                    else
                    {
                        int valueLength = gate.values.Count;

                        for (int i = 0; i < valueLength; i++)
                        {
                            int index = gate.firstFreeValueBoxIndex();

                            gate.values[index] = value;

                            if (valueLength == index + 1)
                            {
                                gate.Work();
                                value = gate.values[0];
                            }
                        }
                    }
                }
                else
                {
                    Wire wire = scheme.lineController.Wires[component.index];

                    if (wire.isActive == null)
                        wire.isActive = value;
                    else
                        value = wire.isActive;
                }
            }

            historyComponents = new List<HistoryComponent>(components);
        }

        public static bool Ver4(List<HistoryComponent> historyComponents, Scheme scheme)
        {
            Debug.WriteLine("\n" + "[Method] Ver4");

            IList<Gate> exINs = scheme.gateController.GetExternalInputs();
            
            //check is all exINs are inited
            for (int i = 0; i < exINs.Count; i++)
                if (exINs[i].values[0] == null)
                    return false;

            int length = historyComponents.Count;
            int gates = 0;

            for (int i = 0; i < length; i++)
                if (historyComponents[i].component == Constants.ComponentTypeEnum.gate)
                    gates++;
            Debug.WriteLine("there is " + gates + " gates of " + length + " components");

            Queue<HistoryComponent> hcs = new Queue<HistoryComponent>(historyComponents);

            while (hcs.Count != gates)
            {
                HistoryComponent hc = hcs.Dequeue();
                Debug.WriteLine("dequeued a component");

                if (hc.component == Constants.ComponentTypeEnum.wire)
                {
                    Debug.WriteLine("it's a wire");

                    Wire w = scheme.lineController.Wires[hc.index];

                    Gate startGate = scheme.gateController.GetGateByWireStart(w.start);
                    Debug.WriteLine("is startGate null? " + (startGate == null));

                    Gate endGate = scheme.gateController.GetGateByWireEnd(w.end);
                    Debug.WriteLine("is endGate null? " + (endGate == null));

                    if (startGate.type != Constants.GateEnum.IN)
                    {
                        Debug.WriteLine("it's a logic gate");

                        if (startGate.firstFreeValueBoxIndex() == -1)
                        {
                            Debug.WriteLine("it's valid");

                            w.isActive = startGate.values[0];
                        }
                        else
                        {
                            Debug.WriteLine("it's not valid yet");

                            hcs.Enqueue(hc);

                            Debug.WriteLine("enqueued back");

                            continue;
                        };
                    }
                    else
                    {
                        Debug.WriteLine("it's exIN");

                        w.isActive = startGate.values[0];
                    }

                    if (endGate.type != Constants.GateEnum.OUT)
                    {
                        Debug.WriteLine("it's a logic gate");

                        int box = endGate.firstFreeValueBoxIndex();

                        endGate.values[box] = w.isActive;

                        if (box == endGate.inputs - 1)
                            endGate.Work();
                    }
                    else
                    {
                        Debug.WriteLine("it's exOUT");

                        endGate.values[0] = w.isActive;
                    }
                }
                else
                {
                    Debug.WriteLine("it's a gate");

                    hcs.Enqueue(hc);

                    Debug.WriteLine("enqueued back");
                }
            }

            return true;
        }

        public static Constants.WorkAlgorithmResult Ver5(Scheme scheme)
        {
            Debug.WriteLine("\n" + "[Method] Ver5");

            if (!IsExternalInputsInited(scheme))
                return Constants.WorkAlgorithmResult.exInsNotInited;
            else if (!IsAllConnected(scheme))
                return Constants.WorkAlgorithmResult.gatesNotConnected;

            Queue<Wire> wires = new Queue<Wire>(scheme.lineController.Wires);

            int counter = 0;

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

                    if(startGate.firstFreeValueBoxIndex() == -1)
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

                    int box = endGate.firstFreeValueBoxIndex();

                    endGate.values[box] = wire.isActive;

                    if (box == endGate.inputs - 1)
                        endGate.Work();
                }
            }

            return Constants.WorkAlgorithmResult.correct;
        }
    }
}
