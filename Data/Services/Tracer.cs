using System;
using System.Collections.Generic;
using System.Linq;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services.History;
using SchemeCreator.UI;
using SchemeCreator.UI.Dialogs;
using SchemeCreator.UI.Dynamic;
using PortType = SchemeCreator.UI.Dynamic.PortType;

namespace SchemeCreator.Data.Services
{
    /// <summary>
    /// Exception to throw in during tracing.
    /// </summary>
    public class TracingErrorException : Exception { }
    
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
        private HistoryService? service;

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

        public HistoryService? Run()
        {
            this.Log("Running...");

            (int exPorts, int gates, int wires) total = (externalPorts.Count(), gates.Count(), wires.Count());

            // check if there is nothing to trace
            if (total.exPorts == 0 || total.wires == 0)
            {
                new Message(Messages.NothingToTrace).ShowAsync();
                return null;
            }

            // trace external inputs first
            this.Log("Tracing external inputs...");
            service.AddToHistory(TraceExternalPorts(PortType.Input));

            while (!AllGatesAndWiresTraced((total.gates, total.wires)))
            {
                // flag used to handle logic errors in the scheme
                bool anyTraced = false;

                // tracing wires
                this.Log("Tracing wires...");
                var tracedWires = GetWiresToTrace().ToList();
                bool wiresTraced = tracedWires.Any();
                service.AddToHistory(tracedWires);

                // tracing gates
                this.Log("Tracing gates...");
                var tracedGates = GetGatesToTrace().ToList();
                bool gatesTraced = tracedGates.Any();
                service.AddToHistory(tracedGates);

                anyTraced = wiresTraced || gatesTraced;

                if (!anyTraced)
                    throw new TracingErrorException();
            }

            // external outputs should be traced last
            this.Log("Tracing external outputs...");
            service.AddToHistory(TraceExternalPorts(PortType.Output));

            this.Log("Done");

            return service;
        }

        /// <summary>
        /// Checks all gates and wires are traced.
        /// </summary>
        /// <param name="total">count if gates and wires traced</param>
        /// <returns>true if all components are traced</returns>
        private bool AllGatesAndWiresTraced((int gates, int wires) total) =>
            total.gates == service.Count(nameof(GateView)) && total.wires == service.Count(nameof(WireView));

        private IEnumerable<ExternalPortView> TraceExternalPorts(PortType type) => externalPorts.Where(x => x.Type == type);

        /// <summary>
        /// Gets gates to trace.
        /// Algorithm:
        /// Select all traced wires and gates.
        /// Return all not traces gates that connect to traced wires.
        /// </summary>
        /// <returns>List of gates to trace</returns>
        private IEnumerable<GateView> GetGatesToTrace()
        {
            var tracedWires = service.GetAll(nameof(WireView))
                .Select(x => x.TracedObject as WireView);

            var tracedGates = service.GetAll(nameof(GateView))
                .Select(x => x.TracedObject as GateView);

            var notTracedGates = gates.Except(tracedGates);

            // select only gates connected to traced wires
            return notTracedGates
                .Where(gate => tracedWires
                    .Any(wire => gate.WireEndConnects(wire)));
        }

        /// <summary>
        /// Gets wires to trace.
        /// Algorithm:
        /// Select all already traced components of the scheme.
        /// Return all not traced wires that connect to the components. 
        /// </summary>
        /// <returns>List of wires to trace</returns>
        private IEnumerable<WireView> GetWiresToTrace()
        {
            var exPorts = externalPorts
                    .Where(p => p.Type == PortType.Input);

            var tracedGates = service.GetAll(nameof(GateView))
                .Select(x => x.TracedObject as GateView);

            var tracedWires = service.GetAll(nameof(WireView))
                .Select(x => x.TracedObject as WireView);

            // check by point OR by matrix location or external point
            return wires.Except(tracedWires)
                .Where(w => tracedGates.Any(g => g.WireStartConnects(w))
                    || exPorts.Any(p => p.MatrixLocation == w.Connection.MatrixStart));
        }
    }
}