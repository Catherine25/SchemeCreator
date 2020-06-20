using System.Collections.Generic;
using Windows.Foundation;

namespace SchemeCreator.Data
{
    static public class Constants
    {
        public static Size externalGateSize = new Size { Width = 50, Height = 50 };
        public static Size logicGateSize = new Size { Width = 50, Height = 70 };
        public static Size traceTextSize = new Size { Width = 50, Height = 30 };
        public static Size dotSize = new Size { Width = 10, Height = 10 };
        public static Size gatePortSize = new Size { Width = 10, Height = 10 };
        public static Size externalPortSize = new Size { Width = 20, Height = 20 };

        public const int netSize = 8;

        public const double offset = 10.0,
            lineStartOffset = 5.0,
            wireThickness = 5.0;

        public static SortedSet<GateEnum> singleInput = new SortedSet<GateEnum> {
            GateEnum.Buffer,
            GateEnum.NOT,
            GateEnum.IN,
            GateEnum.OUT
        };

        public static SortedSet<GateEnum> singleOutput = new SortedSet<GateEnum> {
            GateEnum.AND,
            GateEnum.Buffer,
            GateEnum.NAND,
            GateEnum.NOR,
            GateEnum.NOT,
            GateEnum.OR,
            GateEnum.XNOR,
            GateEnum.XOR,
            GateEnum.IN,
            GateEnum.OUT
        };

        public static SortedSet<GateEnum> external = new SortedSet<GateEnum> {
            GateEnum.IN,
            GateEnum.OUT
        };

        public static Dictionary<GateEnum, string> gateNames = new Dictionary<GateEnum, string> {
            { GateEnum.AND, "AND" },
            { GateEnum.Buffer, "Buffer" },
            { GateEnum.IN, "Input" },
            { GateEnum.NAND, "NAND" },
            { GateEnum.NOR, "NOR" },
            { GateEnum.NOT, "NOT" },
            { GateEnum.OR, "OR" },
            { GateEnum.OUT, "Output" },
            { GateEnum.XNOR, "XNOR" },
            { GateEnum.XOR, "XOR" }
        };

        public static Dictionary<BtId, string> btText = new Dictionary<BtId, string> {
            { BtId.newSchemeBt, "New Scheme" },
            { BtId.loadSchemeBt, "Load Scheme" },
            { BtId.saveSchemeBt, "Save Scheme" },
            { BtId.traceSchemeBt, "Trace Scheme" },
            { BtId.workSchemeBt, "Work" },
            { BtId.addLineBt, "Add Line" },
            { BtId.changeValueBt, "Change Value" }
        };

        public enum BtId { newSchemeBt, loadSchemeBt, saveSchemeBt, traceSchemeBt, workSchemeBt, addLineBt, changeValueBt }
        public enum GateEnum { IN, OUT, Buffer, NOT, AND, NAND, OR, NOR, XOR, XNOR };
        public enum ModeEnum { addGateMode, addLineMode, changeValueMode }
        public enum FrameEnum { workspace, newGate }
        public enum ComponentTypeEnum { gate, wire }
        public enum MessageTypes { exInsNotInited, gatesNotConnected, functionIsNotSupported, modeChanged, newSchemeButtonClicked, detailedView, createGate, visualizingFailed }
        public enum MessageAttribute { title, text, button1, button2 }
        public enum WorkAlgorithmResult { correct, exInsNotInited, gatesNotConnected, schemeIsntCorrect }
    }
}
