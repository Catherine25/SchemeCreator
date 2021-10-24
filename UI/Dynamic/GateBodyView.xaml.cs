using System.Collections.Generic;
using Windows.Foundation;
using SchemeCreator.Data.Services;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Extensions;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class GateBodyView : UserControl
    {
        public static readonly Size LogicGateSize = new(50, 75);

        public GateBodyView()
        {
            InitializeComponent();

            this.SetSize(LogicGateSize);

            Button.Foreground = Colorer.GetGateForegroundBrush();
            Button.Background = Colorer.GetGateBackgroundBrush();
        }

        public GateEnum GateType
        {
            set => Button.Content = GateNames[value];
        }
        
        public static readonly Dictionary<GateEnum, string> GateNames = new()
        {
            { GateEnum.And, "AND" },
            { GateEnum.Buffer, "Buffer" },
            { GateEnum.Nand, "NAND" },
            { GateEnum.Nor, "NOR" },
            { GateEnum.Not, "NOT" },
            { GateEnum.Or, "OR" },
            { GateEnum.Xnor, "XNOR" },
            { GateEnum.Xor, "XOR" },
            { GateEnum.Custom, "Custom" },
        };
    }
}
