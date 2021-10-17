using SchemeCreator.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class TraceLabel : UserControl
    {
        public TraceLabel() => InitializeComponent();

        public TraceLabel(int index, double x, double y)
        {
            InitializeComponent();

            TextBlock.Text = index.ToString();

            Width = 20;
            Height = 20;

            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            Margin = new Thickness(x, y + 5, 0, 0);

            Grid.SetColumnSpan(this, Constants.NetSize);
            Grid.SetRowSpan(this, Constants.NetSize);
        }
    }
}
