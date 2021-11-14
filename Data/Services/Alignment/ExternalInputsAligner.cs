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

            var externalInputs = scheme.ExternalInputs.ToList();
            var portsWithIndexes = new List<(ExternalPortView port, int destLoc, int portIdx)>();

            foreach (var i in externalInputs)
            {
                var wires = NavigationHelper.ConnectedWires(scheme, i).ToList();
                var destinations = wires.Select(x => NavigationHelper.GetDestination(scheme, x));
                var minLocDest = destinations.OrderBy(x => x.MatrixLocation.X).ThenBy(x => x.MatrixLocation.Y).First();
                var minLoc = minLocDest.MatrixLocation;

                int minPortIndex = 0;
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

            this.Log($"Processed {externalInputs.Count} external inputs.");
            this.Log("Done");
            return processed;
        }

        private void MoveExternalInput(ExternalPortView port, Vector2 newPosition)
        {
            // get connected wires to the port by its old location
            var connectedWires = NavigationHelper.ConnectedWires(scheme, port).ToList();
            
            // update location
            port.MatrixLocation = newPosition;
            
            // adjust connected wires location
            foreach (var w in connectedWires)
            {
                var c2 = w.Connection;

                c2.MatrixStart = newPosition;
                c2.StartPoint = port.GetCenterRelativeTo(scheme);

                w.SetConnection(c2);
            }
        }
    }
}
