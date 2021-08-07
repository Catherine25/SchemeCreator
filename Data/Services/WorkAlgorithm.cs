﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Services
{
    static class WorkAlgorithm
    {
        public static WorkAlgorithmResult Visualize(SchemeView scheme, List<HistoryComponent> traceHistory)
        {
            Debug.WriteLine("\n" + "[Method] Ver6");

            if (scheme.GetFirstNotInitedExternalPort() != null)
                return WorkAlgorithmResult.ExInsNotInited;

            // todo
            //if (!scheme.IsAllConnected())
                //return WorkAlgorithmResult.GatesNotConnected;

            // transfer from input ports to wires
            foreach (ExternalPortView port in scheme.ExternalPorts.Where(p => p.Type == PortType.Input))
            foreach (WireView wire in scheme.Wires.Where(w => w.Start == port.Center))
                wire.Value = port.Value;

            bool traced = true;
            while (scheme.Gates.Any(g => !g.AreOutputsReady) && traced)
            {
                traced = false;
                // transfer from wires to gates
                foreach (WireView wire in scheme.Wires.Where(w => w.Value != null)) //TODO exclude transfered
                foreach (GateView gate in scheme.Gates.Where(g => g.WireConnects(wire.End)))
                {
                    traced = true;
                    gate.SetInputValueFromWire(wire);
                }

                // transfer from gates that are ready to wires
                foreach (WireView wire in scheme.Wires.Where(w => w.Value != null))
                foreach (GateView gate in scheme.Gates.Where(g =>
                    g.WireConnects(wire.Start) && g.AreOutputsReady))
                {
                    gate.SetInputValueFromWire(wire);
                    traced = true;
                }
            }

            if (!traced)
                return WorkAlgorithmResult.SchemeIsntCorrect;

            // transfer from wires to output ports
            foreach (ExternalPortView port in scheme.ExternalPorts.Where(p => p.Type == PortType.Output))
            foreach (WireView wire in scheme.Wires.Where(w => w.End == port.Center))
                wire.Value = port.Value;

            return WorkAlgorithmResult.Correct;
        }

        public static WorkAlgorithmResult Visualize_7(SchemeView scheme)
        {
            Debug.WriteLine("[Method] Ver7");



            if (scheme.GetFirstNotInitedExternalPort() != null)
                return WorkAlgorithmResult.ExInsNotInited;

            // todo
            //if (!scheme.IsAllConnected())
                //return WorkAlgorithmResult.GatesNotConnected;

            // transfer from input ports to wires
            foreach (ExternalPortView port in scheme.ExternalPorts.Where(p => p.Type == PortType.Input))
            foreach (WireView wire in scheme.Wires.Where(w => w.Start == port.Center))
                wire.Value = port.Value;

            bool traced = true;
            while (scheme.Gates.Any(g => !g.AreOutputsReady) && traced)
            {
                traced = false;
                // transfer from wires to gates
                foreach (WireView wire in scheme.Wires.Where(w => w.Value != null)) //TODO exclude transfered
                foreach (GateView gate in scheme.Gates.Where(g => g.WireConnects(wire.End)))
                {
                    traced = true;
                    gate.SetInputValueFromWire(wire);
                }

                // transfer from gates that are ready to wires
                foreach (WireView wire in scheme.Wires.Where(w => w.Value != null))
                foreach (GateView gate in scheme.Gates.Where(g =>
                    g.WireConnects(wire.Start) && g.AreOutputsReady))
                {
                    gate.SetInputValueFromWire(wire);
                    traced = true;
                }
            }

            if (!traced)
                return WorkAlgorithmResult.SchemeIsntCorrect;

            // transfer from wires to output ports
            foreach (ExternalPortView port in scheme.ExternalPorts.Where(p => p.Type == PortType.Output))
            foreach (WireView wire in scheme.Wires.Where(w => w.End == port.Center))
                wire.Value = port.Value;

            return WorkAlgorithmResult.Correct;
        }

        //public static WorkAlgorithmResult Visualize(Scheme scheme)
        //{
        //    //Debug.WriteLine("\n" + "[Method] Ver5");

        //    if (scheme.gateController.GetFirstNotInitedExternalPort() != null)
        //        return WorkAlgorithmResult.exInsNotInited;
        //    else if (!scheme.IsAllConnected())
        //        return WorkAlgorithmResult.gatesNotConnected;

        //    Queue<Wire> wires = new Queue<Wire>(scheme.lineController.Wires);

        //    int connectionNotFound = 0;
        //    while (wires.Count != 0)
        //    {
        //        Wire wire = wires.Dequeue();
        //        //Debug.WriteLine("Dequed a wire");

        //        GateView startGate = scheme.gateController.GetGateByWireStart(wire.Start);

        //        if(startGate.Type == GateEnum.IN)
        //        {
        //            //Debug.WriteLine("it's exIN");

        //            connectionNotFound = 0;

        //            wire.isActive = startGate.Values[0];
        //        }
        //        else
        //        {
        //            //Debug.WriteLine("it's a logic gate");

        //            if(startGate.FirstFreeValueBoxIndex() == -1)
        //            {
        //                //Debug.WriteLine("it's valid");

        //                connectionNotFound = 0;

        //                wire.isActive = startGate.Values[0];
        //            }
        //            else
        //            {
        //                //Debug.WriteLine("it's not valid yet");

        //                connectionNotFound++;

        //                wires.Enqueue(wire);

        //                if (connectionNotFound > wires.Count)
        //                    return WorkAlgorithmResult.schemeIsntCorrect;

        //                continue;
        //            }
        //        }

        //        GateView endGate = scheme.gateController.GetGateByWireEnd(wire.End);

        //        if (endGate.Type == GateEnum.OUT)
        //        {
        //            //Debug.WriteLine("it's exOUT");

        //            connectionNotFound = 0;

        //            endGate.Values[0] = wire.isActive;
        //        }
        //        else
        //        {
        //            //Debug.WriteLine("it's a logic gate");

        //            int box = endGate.FirstFreeValueBoxIndex();

        //            connectionNotFound = 0;

        //            endGate.Values[box] = wire.isActive;

        //            if (box == endGate.InputCount - 1)
        //                endGate.Work();
        //        }
        //    }

        //    return WorkAlgorithmResult.correct;
        //}
    }

    public class Connector
    {
        private SchemeView schemeView;
        
        public Connector(SchemeView schemeView)
        {
            this.schemeView = schemeView;
        }

        public void ConnectExternalInputsToWires()
        {
            var inputs = schemeView.ExternalPorts.Where(ports => ports.Type == PortType.Input);
            var wires = schemeView.Wires;
            var connections = new List<Connection>();

            foreach (var input in inputs) 
                foreach (var wire in wires)
                    if (Connects(input, wire))        
                        connections.Add(new Connection(input, wire));
        }
        
        public bool Connects(ExternalPortView port, WireView w)
        {
            return port.Center == w.Start || port.Center == w.End;
        }

        public bool Connects(GatePortView port, WireView w)
        {
            return port.Center == w.Start || port.Center == w.End;
        }
    }

    public struct Connection
    {
        public Connection(IValueHolder source, IValueHolder destination)
        {
            Source = source;
            Destination = destination;

            source.ValueChanged += ValueChanged;
        }

        private void ValueChanged(bool? obj) => Destination = Source;

        public IValueHolder Source;
        public IValueHolder Destination;
    }
}
