using Windows.Foundation;
using SchemeCreator.Data.Services;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Extensions;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class GateBodyView : UserControl
    {
        public static readonly Size LogicGateSize = new(50, 70);

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
    }
}
