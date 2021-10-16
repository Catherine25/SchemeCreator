using System.Diagnostics;
using System.Linq;
using System.Numerics;
using SchemeCreator.Data.Extensions;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Services.Alignment
{
    public class ExternalOutputsAligner
    {
        private SchemeView scheme;

        public ExternalOutputsAligner(SchemeView scheme)
        {
            this.scheme = scheme;
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
