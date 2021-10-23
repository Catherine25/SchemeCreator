using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Extensions;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class TraceLabel : UserControl
    {
        private readonly Size traceLabelSize = new(20, 20);
        private readonly Size traceLabelOffset = new(0, 5);
        public TraceLabel() => InitializeComponent();

        public TraceLabel(int index, double x, double y)
        {
            InitializeComponent();

            this.SetSize(traceLabelSize);
            this.SetLeftTopAlignment();
            this.MakeCellIndependent(SchemeView.GridSize);

            TextBlock.Text = index.ToString();
            Margin = new Thickness(x + traceLabelOffset.Width, y + traceLabelOffset.Height, 0, 0);
        }
    }
}
