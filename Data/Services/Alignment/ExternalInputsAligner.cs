using System.Diagnostics;
using System.Linq;
using System.Numerics;
using SchemeCreator.Data.Extensions;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Services.Alignment
{
    public class ExternalInputsAligner
    {
        private SchemeView scheme;

        public ExternalInputsAligner(SchemeView scheme)
        {
            this.scheme = scheme;
        }

        public void MoveExternalInputs()
        {
            // get external inputs of the scheme.
            var externalInputs = scheme.ExternalInputs;
            Debug.WriteLine($"Got {externalInputs.Count()} external inputs.");

            // create a tuple list of their location matrixes and themselves.
            var tuples = externalInputs.Select(p => (port: p, loc: p.MatrixLocation));
            Debug.WriteLine($"Created {tuples.Count()} tuples.");

            // order them ascending by their location first by row and then by column.
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
    }
}
