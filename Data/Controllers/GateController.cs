﻿using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.ConstantsNamespace;
using SchemeCreator.Data.Models;

namespace SchemeCreator.Data.Controllers
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

        public Gate GetGateByInOut(Port p, Constants.ConnectionType type)
        {
            foreach (Gate gate in Gates)
                if (gate.ContainsInOutByCenter(p.CenterPoint, type))
                    return gate;

            throw new System.Exception();
        }

        public IEnumerable<Gate> GetLogicGates() =>
            gates.Where(gate => (!Constants.external.Contains(gate.type)));

        public IEnumerable<Gate> GetExternalGates() =>
            gates.Where(gate => Constants.external.Contains(gate.type));

        public IEnumerable<Gate> GetExternalInputs() =>
            gates.Where(x => x.type == Constants.GateEnum.IN);

        public Gate GetFirstNotInitedGate() =>
            GetExternalInputs().FirstOrDefault(x => x.values[0] == null);

        public Gate GetGateByWireStart(Point point) =>
            gates.FirstOrDefault(
                x => x.GetBodyByWirePart(point) != null
                || x.GetInOutByWirePart(point) != null);

        public Gate GetGateByWireEnd(Point point) =>
            gates.FirstOrDefault(
                gate => gate.GetBodyByWirePart(point) != null
                || gate.GetInOutByWirePart(point) != null);

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