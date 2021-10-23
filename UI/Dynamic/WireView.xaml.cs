using SchemeCreator.Data.Interfaces;
using SchemeCreator.Data.Services;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using SchemeCreator.Data.Extensions;

namespace SchemeCreator.UI.Dynamic
{
    public class WireConnection
    {
        public Vector2 MatrixStart { get; set; }
        public int? StartPort { get; set; }

        public Vector2 MatrixEnd { get; set; }
        public int? EndPort { get; set; }

        public Point StartPoint;
        public Point EndPoint;
    }

    public sealed partial class WireView : UserControl, IValueHolder
    {
        private const double WireThickness = 5.0;
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
            XLine.SetLeftTopAlignment();
            XLine.MakeCellIndependent(SchemeView.GridSize);

            XLine.Fill = Colorer.GetBrushByValue(null);
            XLine.StrokeThickness = WireThickness;
            XLine.Tapped += (sender, e) => Tapped(this);

            XLine.PointerEntered += (sender, e) => XLine.StrokeThickness *= 2;
            XLine.PointerExited += (sender, e) => XLine.StrokeThickness /= 2;

            XLine.Stroke = new SolidColorBrush(Colors.Wheat);
            XLine.Fill = new SolidColorBrush(Colors.Wheat);
        }
    }
}
