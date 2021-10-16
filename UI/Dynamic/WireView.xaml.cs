using SchemeCreator.Data;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.Data.Services;
using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using SchemeCreator.Data.Models;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class WireView : UserControl, IValueHolder
    {
        public WireConnection Connection => connection;
        private WireConnection connection;

        public void SetConnection(WireConnection con)
        {
            connection = con;

            XLine.X1 = con.StartPoint.X;
            XLine.Y1 = con.StartPoint.Y;
            XLine.X2 = con.EndPoint.X;
            XLine.Y2 = con.EndPoint.Y;

            //must be called to update coordinates immediately
            UpdateLayout();
        }

        public new Action<WireView> Tapped;

        public bool? Value
        {
            get => _value;
            set
            {
                _value = value;
                XLine.Stroke = Colorer.GetBrushByValue(_value);
                XLine.Fill = Colorer.GetBrushByValue(_value);
            }
        }
        private bool? _value;
        public Action<bool?> ValueChanged { get; set; }

        public WireView()
        {
            InitializeComponent();

            InitLine();

            SetConnection(new());
        }

        public WireView(WireConnection connection)
        {
            InitializeComponent();

            InitLine();

            this.connection = connection;
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
