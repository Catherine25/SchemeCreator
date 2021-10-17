using System;
using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class DotView : UserControl
    {
        public new Action<DotView> Tapped;

        public DotView() => InitializeComponent();

        public DotView(int x, int y)
        {
            InitializeComponent();

            HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Width = Constants.DotSize.Width;
            Height = Constants.DotSize.Height;

            Grid.SetRow(this, x - 1);
            Grid.SetColumn(this, y - 1);

            Ellipse.PointerEntered += (sender, args) => Ellipse.Activate();
            Ellipse.PointerExited += (sender, args) => Ellipse.Deactivate();
            Ellipse.Tapped += (sender, args) => Tapped(this);

            Colorer.SetFillByValue(Ellipse, false);
        }
    }
}
