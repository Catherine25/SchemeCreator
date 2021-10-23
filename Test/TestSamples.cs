using SchemeCreator.UI;
using System.Linq;
using System.Diagnostics;
using SchemeCreator.Test.Builder;

namespace SchemeCreator.Test
{
    public class TestSamples
    {
        private ComponentBuilder builder;

        public void Test1(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var output = builder.BuildOutput();

            scheme.Connect(input, output);

            Debug.Assert(scheme.Wires.Count() == 1);
        }

        public void Test2(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var output = builder.BuildOutput();

            var gate = builder.BuildGate();

            scheme.Connect(input, gate);
            scheme.Connect(gate, output);

            Debug.Assert(scheme.Wires.Count() == 2);
        }

        public void Test3(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var input2 = builder.BuildInput();
            var output = builder.BuildOutput();

            var gate = builder.BuildGate(GateEnum.And, 2);

            scheme.Connect(input, gate);
            scheme.Connect(input2, gate, 1);
            scheme.Connect(gate, output);

            Debug.Assert(scheme.Wires.Count() == 3);
        }

        public void Test4(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var input2 = builder.BuildInput();
            var input3 = builder.BuildInput();
            var output = builder.BuildOutput();

            var gate = builder.BuildGate(GateEnum.And, 3);

            scheme.Connect(input, gate);
            scheme.Connect(input2, gate, 1);
            scheme.Connect(input3, gate, 2);
            scheme.Connect(gate, output);

            Debug.Assert(scheme.Wires.Count() == 4);
        }

        public void Test5(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var input2 = builder.BuildInput();
            var input3 = builder.BuildInput();
            var input4 = builder.BuildInput();
            var output = builder.BuildOutput();

            var gate = builder.BuildGate(GateEnum.And, 4);

            scheme.Connect(input, gate);
            scheme.Connect(input2, gate, 1);
            scheme.Connect(input3, gate, 2);
            scheme.Connect(input4, gate, 3);
            scheme.Connect(gate, output);

            Debug.Assert(scheme.Wires.Count() == 5);
        }

        public void Test6(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var output = builder.BuildOutput();

            var gate = builder.BuildGate();
            var gate2 = builder.BuildGate();

            scheme.Connect(input, gate);
            scheme.Connect(gate, gate2);
            scheme.Connect(gate2, output);

            Debug.Assert(scheme.Wires.Count() == 3);
        }

        public void Test7(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var input2 = builder.BuildInput();
            var input3 = builder.BuildInput();
            var output = builder.BuildOutput();

            var gate = builder.BuildGate(GateEnum.And, 2);
            var gate2 = builder.BuildGate(GateEnum.Nand, 2);

            scheme.Connect(input, gate);
            scheme.Connect(input2, gate, 1);
            scheme.Connect(gate, gate2);
            scheme.Connect(input3, gate2, 1);
            scheme.Connect(gate2, output);

            Debug.Assert(scheme.Wires.Count() == 5);
        }

        public void Test8(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var input2 = builder.BuildInput();
            var output = builder.BuildOutput();
            var output2 = builder.BuildOutput();

            var gate = builder.BuildGate(GateEnum.And, 2);
            var gate2 = builder.BuildGate();

            scheme.Connect(input, gate);
            scheme.Connect(input2, gate, 1);
            scheme.Connect(gate, gate2);
            scheme.Connect(gate2, output);
            scheme.Connect(gate2, output2);

            Debug.Assert(scheme.Wires.Count() == 5);
        }

        public void Test9(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var input2 = builder.BuildInput();
            var output = builder.BuildOutput();

            var gate = builder.BuildGate();
            var gate2 = builder.BuildGate(GateEnum.And, 2);
            var gate3 = builder.BuildGate();

            scheme.Connect(input, gate);
            scheme.Connect(gate, gate2);
            scheme.Connect(input2, gate2, 1);
            scheme.Connect(gate2, gate3);
            scheme.Connect(gate3, output);

            Debug.Assert(scheme.Wires.Count() == 5);
        }

        public void Test10(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var input2 = builder.BuildInput();
            var output = builder.BuildOutput();
            var output2 = builder.BuildOutput();

            scheme.Connect(input, output2);
            scheme.Connect(input2, output);

            Debug.Assert(scheme.Wires.Count() == 2);
        }

        public void Test11(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var input2 = builder.BuildInput();
            var output = builder.BuildOutput();
            var output2 = builder.BuildOutput();

            var gate = builder.BuildGate();
            var gate2 = builder.BuildGate();

            scheme.Connect(input, gate2);
            scheme.Connect(input2, gate);
            scheme.Connect(gate2, output);
            scheme.Connect(gate, output2);

            Debug.Assert(scheme.Wires.Count() == 4);
        }

        public void Test12(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var input2 = builder.BuildInput();
            var output = builder.BuildOutput();
            var output2 = builder.BuildOutput();

            var gate = builder.BuildGate();

            scheme.Connect(input, gate);
            scheme.Connect(gate, output2);
            scheme.Connect(input2, output);

            Debug.Assert(scheme.Wires.Count() == 3);
        }

        public void Test13(SchemeView scheme)
        {
            builder = new ComponentBuilder(scheme);

            var input = builder.BuildInput();
            var output = builder.BuildOutput();
            var output2 = builder.BuildOutput();

            scheme.Connect(input, output);
            scheme.Connect(input, output2);

            Debug.Assert(scheme.Wires.Count() == 2);
        }
    }
}
