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

        public IEnumerable<ExternalPortView> GetExternalPorts(PortType type) =>
            ExternalPorts.Where(x => x.Type == type);

        public ExternalPortView GetFirstNotInitedExternalPort() =>
            GetExternalPorts(PortType.Input)
                .FirstOrDefault(x => x.Value == null);
    }
}