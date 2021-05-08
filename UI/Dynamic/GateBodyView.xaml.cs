using Windows.UI.Xaml.Controls;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class GateBodyView : UserControl
    {
        public readonly string Text;

        public GateBodyView() => InitializeComponent();

        public GateBodyView(GateEnum gateEnum)
        {
            InitializeComponent();
            Text = GateNames[gateEnum];
        }
    }
}
