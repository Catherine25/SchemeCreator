using SchemeCreator.Data;
using SchemeCreator.Data.Services;
using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class WireView : UserControl
    {
        public Point Start
        {
            get => new Point(XLine.X1, XLine.Y1);
            set
            {
                XLine.X1 = value.X;
                XLine.Y1 = value.Y;
            }
        }

        public Point End
        {
            get => new Point(XLine.X2, XLine.Y2);
            set
            {
                XLine.X2 = value.X;
                XLine.Y2 = value.Y;
            }
        }

        public new Action<WireView> Tapped;

        public bool? IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                XLine.Fill = Colorer.GetBrushByValue(_isActive);
            }
        }
        private bool? _isActive;

        public WireView()
        {
            InitializeComponent();

            Grid.SetColumnSpan(XLine, Constants.netSize);
            Grid.SetRowSpan(XLine, Constants.netSize);
            
            XLine.Fill = Colorer.GetBrushByValue(null);
            XLine.StrokeThickness = Constants.wireThickness;
            XLine.Tapped += (sender, e) => Tapped(this);

            XLine.PointerEntered += (sender, e) => XLine.StrokeThickness *= 2;
            XLine.PointerExited += (sender, e) => XLine.StrokeThickness /= 2;

            XLine.Stroke = new SolidColorBrush(Colors.Wheat);
            XLine.Fill = new SolidColorBrush(Colors.Wheat);
            XLine.StrokeThickness = 5;
            XLine.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            XLine.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
        }
    }
}
