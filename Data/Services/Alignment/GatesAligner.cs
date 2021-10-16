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
        private SchemeNavigationService navigationService;

        public GatesAligner(SchemeView scheme, HistoryService historyService, SchemeNavigationService navigationService)
        {
            this.scheme = scheme;
            this.historyService = historyService;
            this.navigationService = navigationService;
        }

        public void MoveGates()
        {
            if(!scheme.Gates.Any())
                return;

            // 2.1 Get logic gates of the scheme.
            var gates = scheme.Gates.ToList();
            Debug.WriteLine($"Got {gates.Count} to move");

            // 2.2 Get gates ranges.
            var sameTypeComponents = historyService.GetComponentsWithSameType();
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

            for (int i = 0; i < tuplesOrdered.Count(); i++)
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
    }
}
