using System;
using System.Collections.Generic;
using System.Linq;
using SchemeCreator.Data.Services.History;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using PortType = SchemeCreator.UI.Dynamic.PortType;

namespace SchemeCreator.Data.Services
{
    public class Tracer
    {
        private HistoryService service;

        public Tracer()
        {
            service = new HistoryService();
        }

        public void Trace(SchemeView scheme, out IEnumerable<HistoryComponent> result)
        {
            (int gates, int wires) total = (scheme.Gates.Count(), scheme.Wires.Count());

            if (!AnyComponentsToTrace(scheme))
                throw new NotImplementedException();

            TraceExternalInputs(scheme);

            while (!AllGatesAndWiresTraced(total))
            {
                bool somethingTraced = false;

                somethingTraced = TraceWires(scheme) || TraceGates(scheme);

                if (!somethingTraced)
                    throw new Exception();
            }

            TraceExternalOutputs(scheme);

            result = service.TraceHistory;
        }

        private bool AllGatesAndWiresTraced((int gates, int wires) total) =>
            total.gates == service.Count(nameof(GateView)) && total.wires == service.Count(nameof(WireView));

        private bool AnyComponentsToTrace(SchemeView scheme) =>
            scheme.ExternalPorts.Any() && scheme.Gates.Any() && scheme.Wires.Any();

        private void TraceExternalInputs(SchemeView scheme)
        {
            var externalPortViews = scheme.ExternalPorts.Where(x => x.Type == PortType.Input);
            foreach (ExternalPortView externalPortView in externalPortViews)
                service.TraceHistory.Add(new HistoryComponent(nameof(ExternalPortView)));
        }

        private bool TraceGates(SchemeView scheme)
        {
            var tracedWires = service.GetAll(nameof(WireView))
                .Select(x => x.TracedObject as WireView);

            var tracedGates = service.GetAll(nameof(GateView))
                .Select(x => x.TracedObject as GateView);

            var notTracedGates = scheme.Gates.Except(tracedGates);

            var gatesToTrace = notTracedGates
                .Where(gate => tracedWires
                    .Any(wire => gate.WirePartConnects(wire.Connection.EndPoint)));

            if (gatesToTrace.Count() != 0)
                return false;

            foreach (GateView gateView in gatesToTrace)
                service.TraceHistory.Add(new HistoryComponent(gateView));

            return true;
        }

        private bool TraceWires(SchemeView scheme)
        {
            var exPorts = scheme.ExternalPorts.Where(p => p.Type == PortType.Input);

            var tracedGates = service.GetAll(nameof(GateView))
                .Select(x => x.TracedObject as GateView);

            var tracedWires = service.GetAll(nameof(WireView))
                .Select(x => x.TracedObject as WireView);

            //check by point OR by matrix location or external point
            var wiresToTrace = scheme.Wires
                .Except(tracedWires)
                .Where(w => tracedGates.Any(g => g.WirePartConnects(w.Connection.StartPoint))
                || exPorts.Any(p => p.MatrixLocation == w.Connection.MatrixStart));

            if (wiresToTrace.Count() == 0)
                return false;

            foreach (WireView wire in wiresToTrace)
                service.TraceHistory.Add(new HistoryComponent(wire));

            return true;
        }

        private void TraceExternalOutputs(SchemeView scheme)
        {
            var externalPortViews = scheme.ExternalPorts.Where(x => x.Type == PortType.Output);
            foreach (ExternalPortView externalPortView in externalPortViews)
                service.TraceHistory.Add(new HistoryComponent(externalPortView));
        }
    }
}