using System.Diagnostics;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using System.Collections.Generic;
using System.Linq;
using SchemeCreator.Data.Interfaces;
using System.Numerics;
using SchemeCreator.Data.Exceptions;

namespace SchemeCreator.Data.Services.Navigation
{
    public static class NavigationHelper
    {
        public static IEnumerable<WireView> ConnectedWires(SchemeView scheme, ExternalPortView port)
        {
            var wires = scheme.Wires;

            if (port.Type == PortType.Input)
                return wires.Where(port.WireStartConnects);
            else
                return wires.Where(port.WireEndConnects);
        }

        public static IEnumerable<WireView> ConnectedInputWires(SchemeView scheme, GateView gate)
        {
            return scheme.Wires.Where(gate.WireEndConnects);
        }

        public static IEnumerable<WireView> ConnectedOutputWires(SchemeView scheme, GateView gate)
        {
            return scheme.Wires.Where(gate.WireStartConnects);
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

        public static Vector2 GetNotOccupiedLocationOnColumn(HashSet<ISchemeComponent> processed, int column)
        {
            var locs = processed.Select(e => e.MatrixLocation).ToList();

            var occupiedColumns = locs.Where(l => (int)l.X == column).Select(x => x.Y).ToList();

            for (int i = 0; i < SchemeView.GridSize.Width; i++)
                if(!occupiedColumns.Contains(i))
                    return new Vector2(column, i);

            throw new NoPlaceForExternalPortException();
        }

        public static Vector2? GetNotOccupiedLocationOnRow(HashSet<ISchemeComponent> processed, int row)
        {
            var locs = processed.Select(e => e.MatrixLocation).ToList();

            var occupiedRows = locs.Where(l => (int)l.Y == row).Select(x => x.X).ToList();

            for (int i = 0; i < SchemeView.GridSize.Height; i++)
                if(!occupiedRows.Contains(i))
                    return new Vector2(i, row);

            return null;
        }
    }
}
