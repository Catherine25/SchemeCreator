using System.Diagnostics;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using System.Collections.Generic;
using System.Linq;
using SchemeCreator.Data.Interfaces;
using System.Numerics;

namespace SchemeCreator.Data.Services.Navigation
{
    public static class NavigationHelper
    {
        public static IEnumerable<WireView> ConnectedWires(SchemeView scheme, ExternalPortView port)
        {
            var wires = scheme.Wires;

            if (port.Type == PortType.Input)
                return wires.Where(w => port.WireStartConnects(w));
            else
                return wires.Where(w => port.WireEndConnects(w));
        }

        public static IEnumerable<WireView> ConnectedInputWires(SchemeView scheme, GateView gate)
        {
            return scheme.Wires.Where(w => gate.WireEndConnects(w));
        }

        public static IEnumerable<WireView> ConnectedOutputWires(SchemeView scheme, GateView gate)
        {
            return scheme.Wires.Where(w => gate.WireStartConnects(w));
        }

        public static ISchemeComponent GetSource(SchemeView scheme, WireView wire)
        {
            var port = scheme.ExternalInputs.FirstOrDefault(e => e.WireStartConnects(wire));
            var gate = scheme.Gates.FirstOrDefault(g => g.WireStartConnects(wire));

            // only one could be retrieved
            Debug.Assert(port == null ^ gate == null);
            Debug.Assert(port != null ^ gate != null);

            return gate == null ? port : gate;
        }

        public static ISchemeComponent GetDestination(SchemeView scheme, WireView wire)
        {
            var port = scheme.ExternalOutputs.FirstOrDefault(e => e.WireEndConnects(wire));
            var gate = scheme.Gates.FirstOrDefault(g => g.WireEndConnects(wire));

            // only one could be retrieved
            Debug.Assert(port == null ^ gate == null);
            Debug.Assert(port != null ^ gate != null);

            return gate == null ? port : gate;
        }

        public static Vector2? GetNotOccupiedLocationOnColumn(SchemeView scheme, int column)
        {
            var locs = scheme.ExternalPorts.Select(e => e.MatrixLocation).ToList();
            var locs2 = scheme.Gates.Select(e => e.MatrixLocation).ToList();
            locs.AddRange(locs2);

            var occupiedColumns = locs.Where(l => l.X == column).Select(x => x.Y);

            for (int i = 0; i < SchemeView.GridSize.Width; i++)
                if(!occupiedColumns.Contains(i))
                    return new Vector2(column, i);

            return null;
        }

        public static Vector2? GetNotOccupiedLocationOnRow(SchemeView scheme, int row)
        {
            var locs = scheme.ExternalPorts.Select(e => e.MatrixLocation).ToList();
            var locs2 = scheme.Gates.Select(e => e.MatrixLocation).ToList();
            locs.AddRange(locs2);

            var occupiedRows = locs.Where(l => l.Y == row).Select(x => x.X);

            for (int i = 0; i < SchemeView.GridSize.Height; i++)
                if(!occupiedRows.Contains(i))
                    return new Vector2(i, row);

            return null;
        }
    }
}
