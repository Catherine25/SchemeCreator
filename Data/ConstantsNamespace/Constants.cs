using Windows.UI.Xaml.Media;
using Windows.UI.ViewManagement;
using System.Collections.Generic;
using Windows.Foundation;

namespace SchemeCreator.Data.ConstantsNamespace {
    static public class Constants
    {
        public static Size externalGateSize = new Size { Width = 50, Height = 50 };
        public static Size logicGateSize = new Size { Width = 50, Height = 70 };
        public static Size traceTextSize = new Size { Width = 50, Height = 30 };
        public static Size dotSize = new Size { Width = 10, Height = 10 };
        public static Size gatePortSize = new Size { Width = 10, Height = 10 };

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

        public static Dictionary <AccentEnum, SolidColorBrush> brushes = new Dictionary<AccentEnum, SolidColorBrush> {
            { AccentEnum.accent, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.Accent)) },
            { AccentEnum.background, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.Background)) },
            { AccentEnum.foreground, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.Foreground)) },

            { AccentEnum.dark1, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentDark1)) },
            { AccentEnum.dark2, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentDark2)) },
            { AccentEnum.dark3, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentDark3)) },

            { AccentEnum.light1, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentLight1)) },
            { AccentEnum.light2, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentLight2)) },
            { AccentEnum.light3, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentLight3)) },

            { AccentEnum.accent2, new SolidColorBrush(Windows.UI.Colors.Maroon) }
        };

        public enum BtId { newSchemeBt, loadSchemeBt, saveSchemeBt, traceSchemeBt, workSchemeBt, addLineBt, changeValueBt }
        public enum GateEnum { IN, OUT, Buffer, NOT, AND, NAND, OR, NOR, XOR, XNOR };
        public enum ModeEnum { addGateMode, addLineMode, changeValueMode }
        public enum FrameEnum { workspace, newGate }
        public enum AccentEnum { accent, dark1, dark2, dark3, light1, light2, light3, background, foreground, accent2 }
        public enum ComponentTypeEnum { gate, wire }
        public enum MessageTypes { exInsNotInited, gatesNotConnected, functionIsNotSupported, modeChanged, newSchemeButtonClicked, detailedView, createGate, visualizingFailed }
        public enum MessageAttribute { title, text, button1, button2 }
        public enum WorkAlgorithmResult { correct, exInsNotInited, gatesNotConnected, schemeIsntCorrect }
        public enum ConnectionType { input, output }
    }
}