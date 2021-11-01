using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.Data.Services.Navigation;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Services.Alignment
{
    public class ExternalInputsAligner
    {
        private readonly SchemeView scheme;

        public ExternalInputsAligner(SchemeView scheme)
        {
            this.scheme = scheme;
        }

        public HashSet<ISchemeComponent> MoveExternalInputs(HashSet<ISchemeComponent> processed)
        {
            this.Log("Running...");

            // get external inputs of the scheme.
            var externalInputs = scheme.ExternalInputs;
            this.Log($"Got {externalInputs.Count()} external inputs.");

            var portsWithIndexes = new List<(ExternalPortView port, int destLoc, int portIdx)>();

            foreach (var i in externalInputs)
            {
                var wires = NavigationHelper.ConnectedWires(scheme, i);
                var dests = wires.Select(x => NavigationHelper.GetDestination(scheme, x));
                var minLocDest = dests.OrderBy(x => x.MatrixLocation.X).ThenBy(x => x.MatrixLocation.Y).First();
                var minLoc = minLocDest.MatrixLocation;

                var minPortIndex = 0;
                if(minLocDest is GateView gate)
                {
                    var connectingWires = wires.Where(w => gate.WireEndConnects(w));
                    minPortIndex = connectingWires.Min(x => x.Connection.EndPort.Value);
                }

                portsWithIndexes.Add((i, (int)minLoc.Y, minPortIndex));
            }

            var portsToPlace = portsWithIndexes.OrderBy(x => x.destLoc).ThenBy(x => x.portIdx).Select(x => x.port);

            foreach (var port in portsToPlace)
            {
                var place = NavigationHelper.GetNotOccupiedLocationOnColumn(processed, 0);
                MoveExternalInput(port, place);
                processed.Add(port);
            }

            this.Log("Done");
            return processed;
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
    }
}
