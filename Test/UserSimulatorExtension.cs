using System.Linq;
using System.Diagnostics;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Test
{
    public static class UserSimulatorExtension
    {
        public static void Tap(this ExternalPortView port) => port.Tapped(port);
        public static void Tap(this GatePortView gatePort) => gatePort.Tapped(gatePort);

        public static void Connect(this SchemeView scheme, ExternalPortView source, ExternalPortView destination)
        {
            source.Tap();
            destination.Tap();

            var lastWire = scheme.Wires.Last();

            Debug.Assert(source.WireStartConnects(lastWire));
            Debug.Assert(destination.WireEndConnects(lastWire));
        }

        public static void Connect(this SchemeView scheme, ExternalPortView source, GateView destination, int index = 0)
        {
            source.Tap();

            var destPort = destination.Inputs.ToList().ElementAt(index);
            destPort.Tap();

            var lastWire = scheme.Wires.Last();

            Debug.Assert(source.WireStartConnects(lastWire));
            Debug.Assert(destination.WireEndConnects(lastWire));
            Debug.Assert(lastWire.Connection.EndPort == index);
        }

        public static void Connect(this SchemeView scheme, GateView source, ExternalPortView destination, int index = 0)
        {
            var sourcePort = source.Outputs.ElementAt(index);
            sourcePort.Tap();

            destination.Tap();

            var lastWire = scheme.Wires.Last();

            Debug.Assert(source.WireStartConnects(lastWire));
            Debug.Assert(destination.WireEndConnects(lastWire));
            Debug.Assert(lastWire.Connection.StartPort == index);
        }

        public static void Connect(this SchemeView scheme, GateView source, GateView destination, int srcIndex = 0, int dstIndex = 0)
        {
            var sourcePort = source.Outputs.ElementAt(srcIndex);
            sourcePort.Tap();

            var destinationPort = destination.Inputs.ElementAt(dstIndex);
            destinationPort.Tap();

            var lastWire = scheme.Wires.Last();

            Debug.Assert(source.WireStartConnects(lastWire));
            Debug.Assert(destination.WireEndConnects(lastWire));
            Debug.Assert(lastWire.Connection.StartPort == srcIndex);
            Debug.Assert(lastWire.Connection.EndPort == dstIndex);
        }
    }
}
