using System.Diagnostics;
using System.Linq;
using System.Numerics;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services.Navigation;
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
            this.Log("Running...");

            // get external outputs of the scheme.
            var externalOutputs = scheme.ExternalOutputs.ToList();
            this.Log($"Got {externalOutputs.Count()} external outputs.");

            foreach (var item in externalOutputs)
            {
                Vector2? place = NavigationHelper.GetNotOccupiedLocationOnColumn(scheme, Constants.NetSize - 1);
                Debug.Assert(place != null); // todo handle no-place error
                MoveExternalOutput(item, place.Value);
            }

            this.Log("Done");
        }

        private void MoveExternalOutput(ExternalPortView port, Vector2 newLocation)
        {
            // get connected wires to the port
            var connectedWires = Navigation.NavigationHelper.ConnectedWires(scheme, port).ToList();

            // update location
            port.MatrixLocation = newLocation;

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
