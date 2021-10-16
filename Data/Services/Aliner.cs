using System.Diagnostics;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services.History;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SchemeCreator.Data.Services.Navigation;

namespace SchemeCreator.Data.Services
{
    public class Aliner
    {
        private int range = Constants.NetSize;
        private SchemeView scheme;
        private HistoryService service;
        private List<HistoryComponent> history;
        private Queue<HistoryComponent> historyQ;
        private SchemeNavigationService navigationService;

        public Aliner(SchemeView scheme, HistoryService history)
        {
            this.scheme = scheme;
            this.service = history;
            this.historyQ = new Queue<HistoryComponent>(history.TraceHistory);
            this.history = history.TraceHistory.ToList();
            navigationService = new(scheme);
        }

        public void Run()
        {
            Debug.WriteLine("Running Liner...");
            Debug.WriteLine("Moving external inputs...");
            MoveExternalInputs();
            Debug.WriteLine("Moving gates...");
            if(scheme.Gates.Any())
                MoveGates();
            Debug.WriteLine("Moving external outputs...");
            MoveExternalOutputs();
            Debug.WriteLine("Finished running Liner");
        }

        public void MoveExternalInputs()
        {
            // 1.1. Get external inputs of the scheme.
            var externalInputs = scheme.ExternalPorts.Where(p => p.Type == PortType.Input);
            Debug.WriteLine($"Got {externalInputs.Count()} external inputs.");

            // 1.2. Create a tuple list of their location matrixes and themselves.
            var tuples = externalInputs.Select(p => (port: p, loc: p.MatrixLocation));
            Debug.WriteLine($"Created {tuples.Count()} tuples.");

            // 1.3. Order them ascending by their location first by row and then by column.
            var tuplesOrdered = tuples.OrderBy(x => x.loc.X).ThenBy(x => x.loc.Y).Select(t => t.port).ToList();
            Debug.WriteLine($"Ordered {tuplesOrdered.Count()} tuples.");

            for (int i = 0; i < tuplesOrdered.Count(); i++)
            {
                var port = tuplesOrdered[i];

                MoveExternalInput(port, new Vector2(0, i));
            }
        }

        private void MoveExternalInput(ExternalPortView port, Vector2 newPosition)
        {
            var oldLocation = port.MatrixLocation;
            var newLocation = newPosition;
            port.MatrixLocation = newLocation;

            // get connected wires to the port by its old location
            var connectedWires = scheme.Wires.Where(w => w.Connection.MatrixStart == oldLocation).ToList();

            // adjust connected wires location
            foreach (var w in connectedWires)
            {
                var c2 = w.Connection;

                c2.MatrixStart = newLocation;
                c2.StartPoint = port.GetCenterRelativeTo(scheme);

                w.SetConnection(c2);
            }
        }

        public void MoveGates()
        {
            // 2.1 Get logic gates of the scheme.
            var gates = scheme.Gates.ToList();
            Debug.WriteLine($"Got {gates.Count} to move");

            // 2.2 Get gates ranges.
            var sameTypeComponents = service.GetComponentsWithSameType();
            Debug.WriteLine($"GetComponentsWithSameType() returned {sameTypeComponents.Count()}");

            var historizedGates = sameTypeComponents.Where(x => x.Item1 == nameof(GateView));
            Debug.WriteLine($"historizedGates = {historizedGates.Count()}");
            Debug.WriteLine($"historizedGates = {historizedGates.First()}");

            var gatesWithRanges = historizedGates.SelectMany(x => x.Item2.Select(y => (y.Item1, y.Item2.TracedObject as GateView))).ToList();
            Debug.WriteLine($"gatesWithRanges = {gatesWithRanges.Count()}");

            //! maybe it's X, check
            var gatesWithRangesAndExInputsYs = gatesWithRanges.Select(x => (x.Item1, x.Item2,
                navigationService.GetGateExternalPortInputs(x.Item2)
                    .Max(y => y.MatrixLocation.Y)));
            Debug.WriteLine($"gatesWithRangesAndExInputs = {gatesWithRangesAndExInputsYs.Count()}");

            var tuplesOrdered = gatesWithRangesAndExInputsYs.OrderBy(x => x.Item1).ThenBy(x => x.Item3).ToList();
            Debug.WriteLine($"tuplesOrdered = {tuplesOrdered.Count()}");

            for (int i = 0; i < tuplesOrdered.Count; i++)
            {
                (int, GateView, float) g = tuplesOrdered[i];
                MoveGate(g.Item2, g.Item1 + 1, i);
            }
        }

        private void MoveGate(GateView g, int range, int index)
        {
            var oldLocation = g.MatrixLocation;

            var inputWires = navigationService.GetGateInputWires(g).ToList();
            Debug.WriteLine($"inputWires = {inputWires.Count()}");
            var outputWires = navigationService.GetGateOutputWires(g).ToList();
            Debug.WriteLine($"outputWires = {outputWires.Count()}");

            // todo adjust range to skip wires
            Debug.WriteLine($"range = {range}, index = {index}");
            var newLocation = new Vector2(range, index);
            g.MatrixLocation = newLocation;

            // adjust connected wires location
            foreach (var w in inputWires)
            {
                var c2 = w.Connection;

                c2.MatrixEnd = newLocation;

                if (c2.EndPort != null)
                {
                    // todo investigate
                    var port = g.Inputs.ElementAt(c2.EndPort.Value);
                    c2.EndPoint = port.GetCenterRelativeTo(scheme);
                }
                w.SetConnection(c2);
            }

            // adjust connected wires location
            foreach (var w in outputWires)
            {
                var c2 = w.Connection;

                c2.MatrixStart = newLocation;

                if (c2.StartPort != null)
                {
                    // todo investigate
                    var port = g.Outputs.ElementAt(c2.StartPort.Value);
                    c2.StartPoint = port.GetCenterRelativeTo(scheme);
                }

                w.SetConnection(c2);
            }
        }

        public void MoveExternalOutputs()
        {
            // 1.1. Get external outputs of the scheme.
            var externalOutputs = scheme.ExternalPorts.Where(p => p.Type == PortType.Output).ToList();
            Debug.WriteLine($"Got {externalOutputs.Count()} external inputs.");

            // 3.2. Get new locations' columns of the current scheme's components (except for external outputs).
            var componentsLocations = scheme.ExternalPorts.Where(x => x.Type != PortType.Output).Select(x => x.MatrixLocation.Y).ToList();
            componentsLocations.AddRange(scheme.Gates.Select(x => x.MatrixLocation.X));

            // todo finish writing docs
            int maxXLocation = componentsLocations.Max(x => (int)x);

            for (int i = 0; i < externalOutputs.Count(); i++)
                MoveExternalOutput(externalOutputs[i], new Vector2(maxXLocation + 1, i));
        }

        private void MoveExternalOutput(ExternalPortView port, Vector2 newPosition)
        {
            var oldLocation = port.MatrixLocation;
            var newLocation = newPosition;
            port.MatrixLocation = newLocation;

            // get connected wires to the port by its old location
            var connectedWires = scheme.Wires.Where(w => w.Connection.MatrixEnd == oldLocation).ToList();

            // adjust connected wires location
            foreach (var w in connectedWires)
            {
                var c2 = w.Connection;

                c2.MatrixEnd = newLocation;
                c2.EndPoint = port.GetCenterRelativeTo(scheme);

                w.SetConnection(c2);
            }
        }
    }
}
