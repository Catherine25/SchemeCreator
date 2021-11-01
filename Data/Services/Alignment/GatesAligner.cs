using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SchemeCreator.Data.Exceptions.Displayable;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services.Navigation;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Services.Alignment
{
    public class GatesAligner
    {
        private readonly SchemeView scheme;

        public GatesAligner(SchemeView scheme) => this.scheme = scheme;

        public void MoveGates()
        {
            if(!scheme.Gates.Any())
                return;

            // get logic gates of the scheme
            var gates = scheme.Gates.ToList();
            this.Log($"Got {gates.Count} to move");

            var gatesWithRanges = gates.Select(x => (gate: x, range: new RangeHelper(scheme).GetRange(x))).ToList();
            this.Log($"gatesWithRanges = {string.Join(", ", gatesWithRanges.Select(x => $"{x.gate.Type} with range = {x.range}"))}");

            var gatesToProcess = new Queue<GateView>(gatesWithRanges.OrderByDescending(x => x.range).Select(g => g.gate));
            while(gatesToProcess.Any())
            {
                var gate = gatesToProcess.Dequeue();
                // will be used to set column
                int range = gatesWithRanges.Find(x => x.gate == gate).range;

                var outputWires = NavigationHelper.ConnectedOutputWires(scheme, gate);
                var destinations = outputWires.Select(w => NavigationHelper.GetDestination(scheme, w));

                var locations = destinations.Select(x => x.MatrixLocation);
                var minRow = locations.Min(l => l.Y);

                if (minRow >= SchemeView.GridSize.Height)
                    throw new TooManyGatesWithSameRangeException();

                MoveGate(gate, range, (int)minRow);
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
            this.Log($"inputWires = {inputWires.Count()}");
            var outputWires = NavigationHelper.ConnectedOutputWires(scheme, g).ToList();
            this.Log($"outputWires = {outputWires.Count()}");

            var newLocation = new Vector2(range, index);
            g.MatrixLocation = newLocation;

            this.Log($"Moving gate from [{oldLocation.X}, {oldLocation.Y}] to [{newLocation.X}, {newLocation.Y}]");

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
