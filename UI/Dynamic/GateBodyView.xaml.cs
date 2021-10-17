using SchemeCreator.Data.Services;
using Windows.UI.Xaml.Controls;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class GateBodyView : UserControl
    {
        public GateBodyView()
        {
            InitializeComponent();

            Width = LogicGateSize.Width;
            Height = LogicGateSize.Height;

            Button.Foreground = Colorer.GetGateForegroundBrush();
            Button.Background = Colorer.GetGateBackgroundBrush();
        }

        public GateEnum GateType
        {
            set => Button.Content = GateNames[value];
        }
    }
}
