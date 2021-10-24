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
            { GateEnum.And, "&" },
            { GateEnum.Buffer, "1" },
            { GateEnum.Nand, "&" },
            { GateEnum.Nor, "1" },
            { GateEnum.Not, "1" },
            { GateEnum.Or, "1" },
            { GateEnum.Xnor, "=1" },
            { GateEnum.Xor, "=1" },
            { GateEnum.Custom, "Custom" },
        };
    }
}
