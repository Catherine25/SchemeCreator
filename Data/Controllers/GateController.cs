using System.Collections.Generic;
using System.Linq;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Controllers
{
    public class GateController
    {
        public GateController()
        {
            Gates = new List<GateView>();
            ExternalPorts = new List<ExternalPortView>();
        }

        public IList<GateView> Gates;
        public IList<ExternalPortView> ExternalPorts;


        //public GateView GetGateByBody(Button b)
        //{
        //    foreach (GateView gate in Gates)
        //        if (gate.ContainsBodyByMargin(b.Margin))
        //            return gate;

        //    throw new System.Exception();
        //}

        //public Gate GetGateByWire(Wire w)
        //{
        //    foreach (Gate gate in Gates)
        //        if (gate.GetBodyByWirePart(w.Start) != null
        //            || gate.GetBodyByWirePart(w.End) != null)
        //            return gate;
        //        else if ((gate.GetInOutByWirePart(w.Start) != null) ||
        //            gate.GetInOutByWirePart(w.End) != null)
        //            return gate;

        //    return null;
        //}

        //public Gate GetGateByInOut(GatePortView p, ConnectionTypeEnum type)
        //{
        //    foreach (Gate gate in Gates)
        //        if (gate.ContainsInOutByCenter(p.CenterPoint, type))
        //            return gate;

        //    throw new System.Exception();
        //}

        //public IEnumerable<Gate> GetLogicGates() =>
        //    gates.Where(gate => (!Constants.external.Contains(gate.Type)));

        //public IEnumerable<Gate> GetExternalGates() =>
        //    gates.Where(gate => Constants.external.Contains(gate.Type));

        public IEnumerable<ExternalPortView> GetExternalPorts(PortType type) =>
            ExternalPorts.Where(x => x.Type == type);

        public ExternalPortView GetFirstNotInitedExternalPort() =>
            GetExternalPorts(PortType.Input)
                .FirstOrDefault(x => x.Value == null);

        //public Gate GetGateByWireStart(Vector3 point) =>
        //    gates.FirstOrDefault(
        //        x => x.GetBodyByWirePart(point) != null
        //        || x.GetInOutByWirePart(point) != null);

        //public Gate GetGateByWireEnd(Vector3 point) =>
        //    gates.FirstOrDefault(
        //        gate => gate.GetBodyByWirePart(point) != null
        //        || gate.GetInOutByWirePart(point) != null);
    }
}