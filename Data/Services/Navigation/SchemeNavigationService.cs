using System.Diagnostics;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using System.Collections.Generic;
using System.Linq;

namespace SchemeCreator.Data.Services.Navigation
{
    public class SchemeNavigationService
    {
        private readonly SchemeView scheme;

        public SchemeNavigationService(SchemeView scheme)
        {
            this.scheme = scheme;
        }

        public IEnumerable<WireView> GetGateInputWires(GateView gate) => scheme.Wires.Where(w => gate.WireEndConnects(w));

        public IEnumerable<WireView> GetGateOutputWires(GateView gate) => scheme.Wires.Where(w => gate.WireStartConnects(w));

        public Source GetWireSourceControl(WireView wire)
        {
            var gate = scheme.Gates.FirstOrDefault(g => g.WireStartConnects(wire));
            var port = scheme.ExternalPorts.FirstOrDefault(p => p.WireStartConnects(wire));
            return new Source(gate, port);
        }

        public IEnumerable<Source> GetGateSources(GateView gate)
        {
            var inputWires = GetGateInputWires(gate);
            Debug.WriteLine($"Getting for {gate.Type} sources...");
            IEnumerable<Source> sources = inputWires.Select(i => GetWireSourceControl(i));
            Debug.WriteLine(string.Join(", ", sources));
            return sources;
        }

        public IEnumerable<ExternalPortView> GetGateExternalPortInputs(GateView gate)
        {
            var sources = GetGateSources(gate).ToList();

            var gates = sources.Where(s => s.Gate != null);

            while (gates.Count() != 0)
            {
                gates = sources.Where(s => s.Gate != null);

                sources = sources.Except(gates).ToList();

                sources.AddRange(gates.SelectMany(g => GetGateSources(g.Gate)));
            }

            var externalPorts = sources.Select(x => x.Port);

            Debug.Assert(externalPorts.All(x => x != null));

            return externalPorts;
        }
    }
}
