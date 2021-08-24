using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.Data.Services.History;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Services
{
    static class WorkAlgorithm
    {
        public static WorkAlgorithmResult Visualize(SchemeView scheme, IEnumerable<HistoryComponent> traceHistory)
        {
            Debug.WriteLine("\n" + "[Method] Ver6");

            if (scheme.GetFirstNotInitedExternalPort() != null)
                return WorkAlgorithmResult.ExInsNotInited;

            // todo
            //if (!scheme.IsAllConnected())
                //return WorkAlgorithmResult.GatesNotConnected;

            // transfer from input ports to wires
            foreach (ExternalPortView port in scheme.ExternalPorts.Where(p => p.Type == PortType.Input))
            foreach (WireView wire in scheme.Wires.Where(w => w.Connection.StartPoint == port.Center))
                wire.Value = port.Value;

            bool traced = true;
            while (scheme.Gates.Any(g => !g.AreOutputsReady) && traced)
            {
                traced = false;
                // transfer from wires to gates
                foreach (WireView wire in scheme.Wires.Where(w => w.Value != null)) //TODO exclude transfered
                foreach (GateView gate in scheme.Gates.Where(g => g.WirePartConnects(wire.Connection.EndPoint)))
                {
                    traced = true;
                    gate.SetInputValueFromWire(wire);
                }

                // transfer from gates that are ready to wires
                foreach (WireView wire in scheme.Wires.Where(w => w.Value != null))
                foreach (GateView gate in scheme.Gates.Where(g =>
                    g.WirePartConnects(wire.Connection.StartPoint) && g.AreOutputsReady))
                {
                    gate.SetInputValueFromWire(wire);
                    traced = true;
                }
            }

            if (!traced)
                return WorkAlgorithmResult.SchemeIsntCorrect;

            // transfer from wires to output ports
            foreach (ExternalPortView port in scheme.ExternalPorts.Where(p => p.Type == PortType.Output))
            foreach (WireView wire in scheme.Wires.Where(w => w.Connection.EndPoint == port.Center))
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
            foreach (WireView wire in scheme.Wires.Where(w => w.Connection.StartPoint == port.Center))
                wire.Value = port.Value;

            bool traced = true;
            while (scheme.Gates.Any(g => !g.AreOutputsReady) && traced)
            {
                traced = false;
                // transfer from wires to gates
                foreach (WireView wire in scheme.Wires.Where(w => w.Value != null)) //TODO exclude transfered
                foreach (GateView gate in scheme.Gates.Where(g => g.WirePartConnects(wire.Connection.EndPoint)))
                {
                    traced = true;
                    gate.SetInputValueFromWire(wire);
                }

                // transfer from gates that are ready to wires
                foreach (WireView wire in scheme.Wires.Where(w => w.Value != null))
                foreach (GateView gate in scheme.Gates.Where(g =>
                    g.WirePartConnects(wire.Connection.StartPoint) && g.AreOutputsReady))
                {
                    gate.SetInputValueFromWire(wire);
                    traced = true;
                }
            }

            if (!traced)
                return WorkAlgorithmResult.SchemeIsntCorrect;

            // transfer from wires to output ports
            foreach (ExternalPortView port in scheme.ExternalPorts.Where(p => p.Type == PortType.Output))
            foreach (WireView wire in scheme.Wires.Where(w => w.Connection.EndPoint == port.Center))
                wire.Value = port.Value;

            return WorkAlgorithmResult.Correct;
        }
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
            return port.Center == w.Connection.StartPoint || port.Center == w.Connection.EndPoint;
        }

        public bool Connects(GatePortView port, WireView w)
        {
            return port.Center == w.Connection.StartPoint || port.Center == w.Connection.EndPoint;
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
