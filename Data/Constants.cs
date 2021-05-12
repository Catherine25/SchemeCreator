using System.Collections.Generic;
using Windows.Foundation;

namespace SchemeCreator.Data
{
    static public class Constants
    {
        public static Size ExternalPortSize = new(25, 25);
        public static Size LogicGateSize = new(50, 70);
        public static Size TraceTextSize = new(50, 30);
        public static Size DotSize = new(15, 15);
        public static Size GatePortSize = new(10, 10);
        public static double MarginBetweenPorts = LogicGateSize.Width - GatePortSize.Width;

        public const int NetSize = 8;

        public const double Offset = 10.0;
        public const double LineStartOffset = 10.0;
        public const double WireThickness = 10.0;

        public static SortedSet<GateEnum> SingleInput = new() {
            GateEnum.Buffer,
            GateEnum.NOT,
            //GateEnum.IN,
            //GateEnum.OUT
        };

        public static SortedSet<GateEnum> SingleOutput = new() {
            GateEnum.AND,
            GateEnum.Buffer,
            GateEnum.NAND,
            GateEnum.NOR,
            GateEnum.NOT,
            GateEnum.OR,
            GateEnum.XNOR,
            GateEnum.XOR,
            //GateEnum.IN,
            //GateEnum.OUT
        };

        //public static SortedSet<GateEnum> external = new SortedSet<GateEnum> {
        //    GateEnum.IN,
        //    GateEnum.OUT
        //};

        public static Dictionary<GateEnum, string> GateNames = new() {
            { GateEnum.AND, "AND" },
            { GateEnum.Buffer, "Buffer" },
            //{ GateEnum.IN, "Input" },
            { GateEnum.NAND, "NAND" },
            { GateEnum.NOR, "NOR" },
            { GateEnum.NOT, "NOT" },
            { GateEnum.OR, "OR" },
            //{ GateEnum.OUT, "Output" },
            { GateEnum.XNOR, "XNOR" },
            { GateEnum.XOR, "XOR" }
        };

        public enum GateEnum { /*IN, OUT,*/ Buffer, NOT, AND, NAND, OR, NOR, XOR, XNOR };
        public enum ModeEnum { AddGateMode, AddLineMode, ChangeValueMode }
        public enum FrameEnum { Workspace, NewGate }
        public enum ComponentTypeEnum { ExternalPort, Gate, Wire }
        public enum MessageTypes { ExInsNotInited, GatesNotConnected, FunctionIsNotSupported, ModeChanged, NewSchemeButtonClicked, DetailedView, CreateGate, VisualizingFailed }
        public enum MessageAttribute { Title, Text, Button1, Button2 }
        public enum WorkAlgorithmResult { Correct, ExInsNotInited, GatesNotConnected, SchemeIsntCorrect }
    }
}
