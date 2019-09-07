using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data.ConstantsNamespace;

namespace SchemeCreator.Data
{
    public class GateController
    {
        public GateController() => Gates = new List<Gate>();

        private IList<Gate> gates;

        public IList<Gate> Gates
        {
            get => gates;
            set => gates = value;
        }

        public Gate GetGateByBody(Button b)
        {
            foreach (Gate gate in Gates)
                if (gate.ContainsBodyByMargin(b.Margin))
                    return gate;

            throw new System.Exception();
        }

        public Gate GetGateByWire(Wire w)
        {
            foreach (Gate gate in Gates)
                if (gate.GetBodyByWirePart(w.start) != null
                    || gate.GetBodyByWirePart(w.end) != null)
                    return gate;
                else if ((gate.GetInOutByWirePart(w.start) != null) ||
                    gate.GetInOutByWirePart(w.end) != null)
                    return gate;

            return null;
        }

        public Gate GetGateByInOut(Ellipse e, Constants.ConnectionType type)
        {
            foreach (Gate gate in Gates)
                if (gate.ContainsInOutByCenter(e, type))
                    return gate;

            throw new System.Exception();
        }

        public IList<Gate> GetLogicGates()
        {
            var logicGates = new List<Gate>();

            foreach (Gate gate in from Gate g in Gates
                                 where (!Constants.external.Contains(g.type))
                                 select g)
            {
                logicGates.Add(gate);
            }

            return logicGates;
        }

        public IList<Gate> GetExternalGates()
        {
            var externalGates = new List<Gate>();

            foreach (var gate in from Gate g in Gates
                                 where (Constants.external.Contains(g.type))
                                 select g)
            {
                externalGates.Add(gate);
            }

            return externalGates;
        }

        public IList<Gate> GetExternalInputs()
        {
            var externalInputs = new List<Gate>();

            foreach (var gate in from Gate g in Gates
                                 where (Constants.GateEnum.IN == g.type)
                                 select g)
            {
                externalInputs.Add(gate);
            }

            return externalInputs;
        }

        public Gate GetGateByWireStart(Point point)
        {
            foreach (Gate gate in Gates)
                if (gate.GetBodyByWirePart(point) != null)
                    return gate;
                else if (gate.GetInOutByWirePart(point) != null)
                    return gate;

            throw new System.Exception("No gate connected to the wire!");
        }

        public Gate GetGateByWireEnd(Point point)
        {
            foreach (Gate gate in Gates)
                if (gate.GetBodyByWirePart(point) != null)
                    return gate;
                else if (gate.GetInOutByWirePart(point) != null)
                    return gate;

            return null;
        }

        public void ClearValuesExcludingIN()
        {
            int gatesCount = gates.Count;

            for (int i = 0; i < gatesCount; i++)
                if (gates[i].type != Constants.GateEnum.IN)
                {
                    int gateValuesCount = gates[i].inputs;

                    for (int j = 0; j < gateValuesCount; j++)
                        gates[i].values[j] = null;
                }
        }
    }
}