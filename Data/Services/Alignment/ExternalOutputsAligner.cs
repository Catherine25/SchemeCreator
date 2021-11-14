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
    public class ExternalOutputsAligner
    {
        private readonly SchemeView scheme;

        public ExternalOutputsAligner(SchemeView scheme)
        {
            this.scheme = scheme;
        }

        public HashSet<ISchemeComponent> MoveExternalOutputs(HashSet<ISchemeComponent> processed)
        {
            this.Log("Running...");

            // get external outputs of the scheme.
            var ports = scheme.ExternalOutputs.ToList();

            foreach (var item in ports)
            {
                var place = NavigationHelper.GetNotOccupiedLocationOnColumn(processed, (int)SchemeView.GridSize.Width - 1);
                MoveExternalOutput(item, place);
                processed.Add(item);
            }

            this.Log($"Processed {ports.Count()} external outputs");
            this.Log("Done");
            return processed;
        }

        private void MoveExternalOutput(ExternalPortView port, Vector2 newLocation)
        {
            // get connected wires to the port
            var connectedWires = NavigationHelper.ConnectedWires(scheme, port).ToList();

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
