using System;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using System.Linq;
using System.Numerics;
using static SchemeCreator.Data.Constants;
using System.Collections.Generic;
using System.Diagnostics;

namespace SchemeCreator.Test
{
    public class TestSamples
    {
        private PortBuilder portBuilder;
        private GateBuilder gateBuilder;
        private HashSet<Vector2> occupiedPlaces;
        private Random random;

        public TestSamples()
        {
            portBuilder = new PortBuilder();
            gateBuilder = new GateBuilder();
            occupiedPlaces = new HashSet<Vector2>();
            random = new Random();
        }

        public void Test1(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var output = BuildOutput(scheme);

            Tap(input);
            Tap(output);

            Debug.Assert(scheme.Wires.Count() == 1);
        }

        public void Test2(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var output = BuildOutput(scheme);

            var gate = BuildGate(scheme);

            Tap(input);
            Tap(gate.Inputs.Single());

            Tap(gate.Outputs.Single());
            Tap(output);

            Debug.Assert(scheme.Wires.Count() == 2);
        }

        public void Test3(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var input2 = BuildInput(scheme);
            var output = BuildOutput(scheme);

            var gate = BuildGate(scheme, GateEnum.AND, 2);

            Tap(input);
            Tap(gate.Inputs.ElementAt(0));

            Tap(input2);
            Tap(gate.Inputs.ElementAt(1));

            Tap(gate.Outputs.Single());
            Tap(output);

            Debug.Assert(scheme.Wires.Count() == 3);
        }

        public void Test4(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var input2 = BuildInput(scheme);
            var input3 = BuildInput(scheme);
            var output = BuildOutput(scheme);

            var gate = BuildGate(scheme, GateEnum.AND, 3);

            Tap(input);
            Tap(gate.Inputs.ElementAt(0));

            Tap(input2);
            Tap(gate.Inputs.ElementAt(1));

            Tap(input3);
            Tap(gate.Inputs.ElementAt(2));

            Tap(gate.Outputs.Single());
            Tap(output);

            Debug.Assert(scheme.Wires.Count() == 4);
        }

        public void Test5(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var input2 = BuildInput(scheme);
            var input3 = BuildInput(scheme);
            var input4 = BuildInput(scheme);
            var output = BuildOutput(scheme);

            var gate = BuildGate(scheme, GateEnum.AND, 4);

            Tap(input);
            Tap(gate.Inputs.ElementAt(0));

            Tap(input2);
            Tap(gate.Inputs.ElementAt(1));

            Tap(input3);
            Tap(gate.Inputs.ElementAt(2));

            Tap(input4);
            Tap(gate.Inputs.ElementAt(3));

            Tap(gate.Outputs.Single());
            Tap(output);

            Debug.Assert(scheme.Wires.Count() == 5);
        }

        public void Test6(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var output = BuildOutput(scheme);

            var gate = BuildGate(scheme);
            var gate2 = BuildGate(scheme);

            Tap(input);
            Tap(gate.Inputs.Single());

            Tap(gate.Outputs.Single());
            Tap(gate2.Inputs.Single());

            Tap(gate2.Outputs.Single());
            Tap(output);

            Debug.Assert(scheme.Wires.Count() == 3);
        }

        public void Test7(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var input2 = BuildInput(scheme);
            var input3 = BuildInput(scheme);
            var output = BuildOutput(scheme);

            var gate = BuildGate(scheme, GateEnum.AND, 2);
            var gate2 = BuildGate(scheme, GateEnum.NAND, 2);

            Tap(input);
            Tap(gate.Inputs.ElementAt(0));

            Tap(input2);
            Tap(gate.Inputs.ElementAt(1));

            Tap(gate.Outputs.Single());
            Tap(gate2.Inputs.ElementAt(0));

            Tap(input3);
            Tap(gate2.Inputs.ElementAt(1));

            Tap(gate2.Outputs.Single());
            Tap(output);

            Debug.Assert(scheme.Wires.Count() == 5);
        }

        public void Test8(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var input2 = BuildInput(scheme);
            var output = BuildOutput(scheme);
            var output2 = BuildOutput(scheme);

            var gate = BuildGate(scheme, GateEnum.AND, 2);
            var gate2 = BuildGate(scheme);

            Tap(input);
            Tap(gate.Inputs.ElementAt(0));

            Tap(input2);
            Tap(gate.Inputs.ElementAt(1));

            Tap(gate.Outputs.Single());
            Tap(gate2.Inputs.Single());

            Tap(gate2.Outputs.Single());
            Tap(output);

            Tap(gate2.Outputs.Single());
            Tap(output2);

            Debug.Assert(scheme.Wires.Count() == 5);
        }

        public void Test9(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var input2 = BuildInput(scheme);
            var output = BuildOutput(scheme);

            var gate = BuildGate(scheme);
            var gate2 = BuildGate(scheme, GateEnum.AND, 2);
            var gate3 = BuildGate(scheme);

            Tap(input);
            Tap(gate.Inputs.Single());

            Tap(gate.Outputs.Single());
            Tap(gate2.Inputs.ElementAt(0));

            Tap(input2);
            Tap(gate2.Inputs.ElementAt(1));

            Tap(gate2.Outputs.Single());
            Tap(gate3.Inputs.Single());

            Tap(gate3.Outputs.Single());
            Tap(output);

            Debug.Assert(scheme.Wires.Count() == 5);
        }

        public void Test10(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var input2 = BuildInput(scheme);
            var output = BuildOutput(scheme);
            var output2 = BuildOutput(scheme);

            Tap(input);
            Tap(output2);

            Tap(input2);
            Tap(output);

            Debug.Assert(scheme.Wires.Count() == 2);
        }

        public void Test11(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var input2 = BuildInput(scheme);
            var output = BuildOutput(scheme);
            var output2 = BuildOutput(scheme);

            var gate = BuildGate(scheme);
            var gate2 = BuildGate(scheme);

            Tap(input);
            Tap(gate2.Inputs.Single());

            Tap(input2);
            Tap(gate.Inputs.Single());

            Tap(gate2.Outputs.Single());
            Tap(output);

            Tap(gate.Outputs.Single());
            Tap(output2);

            Debug.Assert(scheme.Wires.Count() == 4);
        }

        public void Test12(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var input2 = BuildInput(scheme);
            var output = BuildOutput(scheme);
            var output2 = BuildOutput(scheme);

            var gate = BuildGate(scheme);

            Tap(input);
            Tap(gate.Inputs.Single());

            Tap(gate.Outputs.Single());
            Tap(output2);

            Tap(input2);
            Tap(output);

            Debug.Assert(scheme.Wires.Count() == 3);
        }

        public void Test13(SchemeView scheme)
        {
            var input = BuildInput(scheme);
            var output = BuildOutput(scheme);
            var output2 = BuildOutput(scheme);

            Tap(input);
            Tap(output);

            Tap(input);
            Tap(output2);

            Debug.Assert(scheme.Wires.Count() == 2);
        }

        private void Tap(ExternalPortView port) => port.Tapped(port);
        private void Tap(GatePortView gatePort) => gatePort.Tapped(gatePort);

        public ExternalPortView BuildInput(SchemeView scheme)
        {
            var input = portBuilder.Build(PortType.Input, GetPlace());
            scheme.AddToView(input);
            return input;
        }

        public ExternalPortView BuildOutput(SchemeView scheme)
        {
            var output = portBuilder.Build(PortType.Output, GetPlace());
            scheme.AddToView(output);
            return output;
        }

        public GateView BuildGate(SchemeView scheme, GateEnum type = GateEnum.Buffer, int inputs = 1, int outputs = 1)
        {
            var gate = gateBuilder.Build(type, GetPlace(), inputs, outputs);
            scheme.AddToView(gate);
            return gate;
        }

        private Vector2 GetPlace()
        {
            Vector2 place = GeneratePlace();

            while (occupiedPlaces.Contains(place))
                place = GeneratePlace();

            return place;
        }

        private Vector2 GeneratePlace()
        {
            var x = random.Next(NetSize);
            var y = random.Next(NetSize);
            return new Vector2(x, y);
        }
    }
}
