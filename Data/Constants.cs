using Windows.UI.Xaml.Media;
using Windows.UI.ViewManagement;
using System.Collections.Generic;

namespace SchemeCreator {
    static public class Constants {
        static Constants() { }
        public const int netSize = 8, dotSize = 10, gateWidth = 50, gateHeight = 70, externalGateSize = 50;
        public const double offset = 10.0, lineStartOffset = 5.0;
        public static SortedSet<GateEnum> singleInput = new SortedSet<GateEnum> {
            GateEnum.AND,
            GateEnum.Buffer,
            GateEnum.NAND,
            GateEnum.NOR,
            GateEnum.NOT,
            GateEnum.OR,
            GateEnum.XNOR,
            GateEnum.XOR
        };
        public static SortedSet<GateEnum> singleOutput = new SortedSet<GateEnum> {
            GateEnum.Buffer,
            GateEnum.NOT
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
            { BtId.addGateBt, "Add Gate" },
            { BtId.addLineBt, "Add Line" },
            { BtId.removeLineBt, "Remove Line" }
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
            { AccentEnum.light3, new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentLight3)) }
        };
        public enum BtId { newSchemeBt, loadSchemeBt, saveSchemeBt, traceSchemeBt, workSchemeBt, addGateBt, addLineBt, removeLineBt }
        public enum GateEnum { IN, OUT, Buffer, NOT, AND, NAND, OR, NOR, XOR, XNOR };
        public enum ModeEnum { addGateMode, addLineEndMode, addLineStartMode, removeLineMode, changeValueMode }
        public enum FrameEnum { workspace, newGate }
        public enum AccentEnum { accent, dark1, dark2, dark3, light1, light2, light3, background, foreground }
    }
}