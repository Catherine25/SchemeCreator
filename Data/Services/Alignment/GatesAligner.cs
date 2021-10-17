using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services.History;
using SchemeCreator.Data.Services.Navigation;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Services.Alignment
{
    public class GatesAligner
    {
        private SchemeView scheme;
        private HistoryService historyService;

        public GatesAligner(SchemeView scheme, HistoryService historyService)
        {
            this.scheme = scheme;
            this.historyService = historyService;
        }

        public void MoveGates()
        {
            if(!scheme.Gates.Any())
                return;

            // get logic gates of the scheme
            var gates = scheme.Gates.ToList();
            Debug.WriteLine($"Got {gates.Count} to move");

            // get components grouped by their ranges
            var sameTypeComponents = historyService.GroupComponentsWithSameType();
            Debug.WriteLine($"GetComponentsWithSameType() returned {sameTypeComponents.Count()}");
            Debug.WriteLine(string.Join(", ", sameTypeComponents.Select(x => x.Count())));

            // select only groups with gates
            var historizedGates = sameTypeComponents.Where(x => x.First().TypeName == nameof(GateView));
            Debug.WriteLine($"historizedGates = {historizedGates.Count()}");
            Debug.WriteLine($"historizedGates = {historizedGates.First()}");

            // select gates with ranges
            var gatesWithRanges = new List<(GateView gate, int range)>();
            for (int i = 0; i < historizedGates.Count(); i++)
            {
                var gatesInGroup = historizedGates.ElementAt(i).ToList();
                gatesInGroup.ForEach(x => gatesWithRanges.Add((x.TracedObject as GateView, i)));
            }
            Debug.WriteLine($"gatesWithRanges = {gatesWithRanges.Count()}");

            var gatesToProcess = new Queue<GateView>(gatesWithRanges.OrderByDescending(x => x.range).Select(g => g.gate));
            while(gatesToProcess.Any())
            {
                var gate = gatesToProcess.Dequeue();
                // will be used to set column
                int range = gatesWithRanges.Find(x => x.gate == gate).range;

                var outputWires = NavigationHelper.ConnectedOutputWires(scheme, gate);
                var destinations = outputWires.Select(w => NavigationHelper.GetDestination(scheme, w));

                var locations = destinations.Select(x => x.MatrixLocation);
                var minRow = locations.Max(l => l.Y);

                // adjust range to skip external ports
                MoveGate(gate, range + 1, (int)minRow);
            }
        }

        /// <summary>
        /// Moves gate to new position.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="range"> Specifies column </param>
        /// <param name="index"> Specifies row </param>
        private void MoveGate(GateView g, int range, int index)
        {
            var oldLocation = g.MatrixLocation;

            var inputWires = NavigationHelper.ConnectedInputWires(scheme, g).ToList();
            Debug.WriteLine($"inputWires = {inputWires.Count()}");
            var outputWires = NavigationHelper.ConnectedOutputWires(scheme, g).ToList();
            Debug.WriteLine($"outputWires = {outputWires.Count()}");

            var newLocation = new Vector2(range, index);
            g.MatrixLocation = newLocation;

            Debug.WriteLine($"Moving gate from [{oldLocation.X}, {oldLocation.Y}] to [{newLocation.X}, {newLocation.Y}]");

            // adjust connected wires location
            foreach (var w in inputWires)
            {
                var c2 = w.Connection;

                c2.MatrixEnd = newLocation;

                if (c2.EndPort != null)
                {
                    // todo investigate why it's null
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
                    // todo investigate why it's null
                    var port = g.Outputs.ElementAt(c2.StartPort.Value);
                    c2.StartPoint = port.GetCenterRelativeTo(scheme);
                }

                w.SetConnection(c2);
            }
        }
    }
}
