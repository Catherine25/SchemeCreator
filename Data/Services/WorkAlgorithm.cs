﻿using System.Linq;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Services
{
    static class WorkAlgorithm
    {
        public static WorkAlgorithmResult Visualize(SchemeView scheme)
        {
            TransferFromExternalInputsToWires(scheme);

            bool traced = true;
            while (scheme.Gates.Any(g => !g.AreOutputsReady) && traced)
            {
                traced = false;

                // transfer from wires to gates, then make gates work
                traced = TransferFromWiresToGates(scheme);

                // transfer from gates that are ready to wires
                traced = TransferFromGatesToWires(scheme);

                // handle scheme errors
                if (!traced)
                    return WorkAlgorithmResult.SchemeIsntCorrect;
            }

            // transfer from wires to output ports
            TransferFromWiresToExternalOutputs(scheme);

            return WorkAlgorithmResult.Correct;
        }

        private static void TransferFromExternalInputsToWires(SchemeView scheme)
        {
            var externalInputs = scheme.ExternalPorts.Where(p => p.Type == PortType.Input);

            foreach (ExternalPortView port in externalInputs)
            {
                var connectedWires = scheme.Wires.Where(w => port.WireStartConnects(w));

                foreach (WireView wire in connectedWires)
                    wire.Value = port.Value;
            }
        }

        private static bool TransferFromWiresToGates(SchemeView scheme)
        {
            bool traced = false;

            var initedWires = scheme.Wires.Where(w => w.Value != null);

            foreach (WireView wire in initedWires)
            {
                var connectedGates = scheme.Gates.Where(g => g.WireEndConnects(wire.Connection));

                foreach (GateView gate in connectedGates)
                {
                    traced = true;
                    gate.SetInputValueFromWire(wire);
                    gate.Work();
                }
            }

            return traced;
        }

        private static bool TransferFromGatesToWires(SchemeView scheme)
        {
            bool traced = false;

            var notInitedWires = scheme.Wires.Where(w => w.Value == null);

            foreach (WireView wire in notInitedWires)
            {
                var connectedGates = scheme.Gates.Where(g => g.WireStartConnects(wire.Connection) && g.AreOutputsReady);

                foreach (GateView gate in connectedGates)
                {
                    gate.SetOutputValueToWire(wire);
                    traced = true;
                }
            }

            return traced;
        }

        private static void TransferFromWiresToExternalOutputs(SchemeView scheme)
        {
            var externalOutputs = scheme.ExternalPorts.Where(p => p.Type == PortType.Output);

            foreach (ExternalPortView port in externalOutputs)
            {
                var connectedWires = scheme.Wires.Where(w => w.Connection.MatrixEnd == port.MatrixLocation);

                foreach (WireView wire in connectedWires)
                    port.Value = wire.Value;
            }
        }
    }
}
