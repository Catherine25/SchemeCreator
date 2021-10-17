using System.Collections.Generic;
using System.Linq;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using SchemeCreator.Data.Extensions;

namespace SchemeCreator.Data.Services.Alignment
{
    public class RangeHelper
    {
        private SchemeView scheme;

        public RangeHelper(SchemeView scheme)
        {
            this.scheme = scheme;
        }

        public int GetRange(GateView gate)
        {
            // initial range for gates
            int range = 1;

            List<GateView> gates = GetSourceGates(gate).ToList();;
            while (gates.Any())
            {
                gates = gates.SelectMany(g => GetSourceGates(g)).ToList();
                range++;
            }

            return range;
        }

        private IEnumerable<GateView> GetSourceGates(GateView gate)
        {
            var inputWires = Navigation.NavigationHelper.ConnectedInputWires(scheme, gate);
            var sources = inputWires.Select(x => Navigation.NavigationHelper.GetSource(scheme, x));

            var gates = sources.Where(x => x is GateView)
                .Select(x => x as GateView);

            this.Log($"Found {gates.Count()} source gates");

            return gates;
        }
    }
}
