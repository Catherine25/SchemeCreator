using System;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Services;
using SchemeCreator.Data;
using System.Numerics;
using Windows.Foundation;
using static SchemeCreator.Data.Extensions.ControlExtension;

namespace SchemeCreator.UI.Dynamic
{
    public enum PortType
    {
        Input,
        Output
    }

    public sealed partial class ExternalPortView : UserControl
    {
        public PortType Type;

        public bool? Value
        {
            get => _value;
            set
            {
                _value = value;
                XEllipse.Fill = Colorer.GetBrushByValue(value);
            }
        }

        private bool? _value;
        public Vector2 MatrixLocation
        {
            get
            {
                return _matrixIndex;
            }
            set
            {
                _matrixIndex = value;
                Grid.SetColumn(this, (int)_matrixIndex.X);
                Grid.SetRow(this, (int)_matrixIndex.Y);
            }
        }

        private Vector2 _matrixIndex;

        public new Action<ExternalPortView> Tapped;

        public ExternalPortView() => InitializeComponent();

        public Point Center => new(CenterPoint.X, CenterPoint.Y);

        public ExternalPortView(PortType type, Vector2 point)
        {
            InitializeComponent();

            Type = type;

            PortName.Text = type == PortType.Input ? "In" : "Out";

            this.SetSize(Constants.ExternalPortSize);

            Value = null;
            MatrixLocation = point;

            XEllipse.Tapped += (sender, args) => Tapped(this);
            PortName.Tapped += (sender, args) => Tapped(this);
        }

        public void SwitchMode() => Value = Value == true ? false : Value == false ? null : true;
    }
}
