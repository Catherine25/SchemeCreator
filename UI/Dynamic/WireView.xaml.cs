using SchemeCreator.Data;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.Data.Services;
using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class WireView : UserControl, IValueHolder
    {
        public Point Start
        {
            get => new(XLine.X1, XLine.Y1);
            set
            {
                XLine.X1 = value.X;
                XLine.Y1 = value.Y;
            }
        }

        public Point End
        {
            get => new(XLine.X2, XLine.Y2);
            set
            {
                XLine.X2 = value.X;
                XLine.Y2 = value.Y;
            }
        }

        public new Action<WireView> Tapped;

        public bool? Value
        {
            get => _value;
            set
            {
                _value = value;
                XLine.Fill = Colorer.GetBrushByValue(_value);
            }
        }
        private bool? _value;
        public Action<bool?> ValueChanged { get; set; }

        public WireView()
        {
            InitializeComponent();

            InitLine();
        }

        public WireView(Point start, Point end)
        {
            InitializeComponent();

            InitLine();

            Start = start;
            End = end;
        }

        public void InitLine()
        {
            Grid.SetColumnSpan(XLine, Constants.NetSize);
            Grid.SetRowSpan(XLine, Constants.NetSize);

            XLine.Fill = Colorer.GetBrushByValue(null);
            XLine.StrokeThickness = Constants.WireThickness;
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
