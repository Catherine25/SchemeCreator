using System;
using System.Collections.Generic;
using System.Linq;
using SchemeCreator.Data.Services.History;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using PortType = SchemeCreator.UI.Dynamic.PortType;

namespace SchemeCreator.Data.Services
{
    /// <summary>
    /// Used to trace the components of <see cref="SchemeView"/>.
    /// Builds a trace history as it's result.
    /// Shows a message in case of invalid scheme.
    /// Supports following types of components:
    /// - <see cref="ExternalPortView"/>;
    /// - <see cref="GateView"/>;
    /// - <see cref="WireView"/>;
    /// </summary>
    public class Tracer
    {
        private HistoryService service;

        private IEnumerable<ExternalPortView> externalPorts;
        private IEnumerable<GateView> gates;
        private IEnumerable<WireView> wires;

        public Tracer(SchemeView scheme)
        {
            service = new HistoryService();

            externalPorts = scheme.ExternalPorts;
            gates = scheme.Gates;
            wires = scheme.Wires;
        }

        public IEnumerable<HistoryComponent> Run()
        {
            (int exPorts, int gates, int wires) total = (externalPorts.Count(), gates.Count(), wires.Count());

            // check if there is nothing to trace
            if ((total.exPorts == 0 || total.gates == 0) && total.wires == 0)
            {
                new Message(Messages.NothingToTrace).ShowAsync();
                return null;
            }

            // trace external inputs first
            service.AddToHistory(TraceExternalPorts(PortType.Input));

            while (!AllGatesAndWiresTraced((total.gates, total.wires)))
            {
                // flag used to handle logic errors in the scheme
                bool anyTraced = false;

                var tracedWires = TraceWires();
                bool wiresTraced = tracedWires.Count() > 0;
                service.AddToHistory(tracedWires);

                var tracedGates = TraceGates();
                bool gatesTraced = tracedGates.Count() > 0;
                service.AddToHistory(tracedGates);

                anyTraced = wiresTraced || gatesTraced;

                if (!anyTraced)
                    // TODO handle
                    throw new Exception("Tracing error!");
            }

            // external outputs should be traced last
            service.AddToHistory(TraceExternalPorts(PortType.Output));

            return service.TraceHistory;
        }

        private bool AllGatesAndWiresTraced((int gates, int wires) total) =>
            total.gates == service.Count(nameof(GateView)) && total.wires == service.Count(nameof(WireView));

        private IEnumerable<ExternalPortView> TraceExternalPorts(PortType type) =>
            externalPorts.Where(x => x.Type == type);

        private IEnumerable<GateView> TraceGates()
        {
            var tracedWires = service.GetAll(nameof(WireView))
                .Select(x => x.TracedObject as WireView);

            var tracedGates = service.GetAll(nameof(GateView))
                .Select(x => x.TracedObject as GateView);

            var notTracedGates = gates.Except(tracedGates);

            // select only gates connected to traced wires
            return notTracedGates
                .Where(gate => tracedWires
                    .Any(wire => gate.WireEndConnects(wire.Connection)));
        }

        private IEnumerable<WireView> TraceWires()
        {
            var exPorts = externalPorts
                    .Where(p => p.Type == PortType.Input);

            var tracedGates = service.GetAll(nameof(GateView))
                .Select(x => x.TracedObject as GateView);

            var tracedWires = service.GetAll(nameof(WireView))
                .Select(x => x.TracedObject as WireView);

            // check by point OR by matrix location or external point
            return wires.Except(tracedWires)
                .Where(w => tracedGates.Any(g => g.WireStartConnects(w.Connection))
                    || exPorts.Any(p => p.MatrixLocation == w.Connection.MatrixStart));
        }
    }
}