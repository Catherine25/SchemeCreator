using System;
using System.Collections.Generic;
using System.Linq;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using PortType = SchemeCreator.UI.Dynamic.PortType;

namespace SchemeCreator.Data.Services
{
    public class Tracer
    {
        public List<HistoryComponent> TraceHistory = new List<HistoryComponent>();

        public void Trace(SchemeView scheme)
        {
            if(!AnyComponentsToTrace(scheme))
                throw new NotImplementedException();

            TraceExternalInputs(scheme);

            while (scheme.Gates.Count() != TraceHistory.Count(x => x.Type == Constants.ComponentTypeEnum.Gate)
                   || scheme.Wires.Count() != TraceHistory.Count(x => x.Type == Constants.ComponentTypeEnum.Wire))
            {
                bool somethingTraced = false;

                somethingTraced = TraceWires(scheme) || TraceGates(scheme);
                
                if(!somethingTraced)
                    throw new Exception();
            }

            TraceExternalOutputs(scheme);
        }

        private bool AnyComponentsToTrace(SchemeView scheme)
        {
            return scheme.ExternalPorts.Any(x => x.Type == PortType.Input)
                && scheme.ExternalPorts.Any(x => x.Type == PortType.Output)
                && scheme.Gates.Any()
                && scheme.Wires.Any();
        }

        private void TraceExternalInputs(SchemeView scheme)
        {
            var externalPortViews = scheme.ExternalPorts.Where(x => x.Type == PortType.Input);
            foreach (ExternalPortView externalPortView in externalPortViews)
                TraceHistory.Add(new HistoryComponent
                {
                    Type = Constants.ComponentTypeEnum.ExternalPort,
                    TracedObject = externalPortView
                });
        }

        private bool TraceGates(SchemeView scheme)
        {
            var tracedWires = TraceHistory
                .Where(x => x.Type == Constants.ComponentTypeEnum.Wire)
                .Select(x => x.TracedObject as WireView);

            var tracedGates = TraceHistory
                .Where(x => x.Type == Constants.ComponentTypeEnum.Gate)
                .Select(x => x.TracedObject as GateView);

            var notTracedGates = scheme.Gates
                .Where(x => tracedGates
                    .Any(t => t.CenterPoint != x.CenterPoint));

            var gatesToTrace = notTracedGates
                .Where(gate => tracedWires
                    .Any(wire => gate.WireConnects(wire.End)));

            if (gatesToTrace.Count() != 0)
                return false;

            foreach (GateView gateView in gatesToTrace)
                TraceHistory.Add(new HistoryComponent
                {
                    Type = Constants.ComponentTypeEnum.Gate,
                    TracedObject = gateView
                });

            return true;
        }

        private bool TraceWires(SchemeView scheme)
        {
            var tracedGates = TraceHistory
                .Where(x => x.Type == Constants.ComponentTypeEnum.Gate)
                .Select(x => x.TracedObject as GateView);

            var wiresToTrace = scheme.Wires
                .Where(w => tracedGates
                    .Any(g => g.WireConnects(w.Start)));

            if(wiresToTrace.Count() == 0)
                return false;

            foreach (WireView wire in wiresToTrace)
                TraceHistory.Add(new HistoryComponent
                {
                    Type = Constants.ComponentTypeEnum.Wire,
                    TracedObject = wire
                });

            return true;
        }

        private void TraceExternalOutputs(SchemeView scheme)
        {
            var externalPortViews = scheme.ExternalPorts.Where(x => x.Type == PortType.Output);
            foreach (ExternalPortView externalPortView in externalPortViews)
                TraceHistory.Add(new HistoryComponent
                {
                    Type = Constants.ComponentTypeEnum.ExternalPort,
                    TracedObject = externalPortView
                });
        }
    }

    public class HistoryComponent
    {
        public Constants.ComponentTypeEnum Type;
        public object TracedObject;
    }
}