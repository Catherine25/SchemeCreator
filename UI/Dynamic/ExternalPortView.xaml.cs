using System;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Services;
using SchemeCreator.Data;
using System.Numerics;
using Windows.Foundation;
using static SchemeCreator.Data.Extensions.ControlExtension;
using SchemeCreator.Data.Interfaces;

namespace SchemeCreator.UI.Dynamic
{
    public enum PortType
    {
        Input,
        Output
    }

    public sealed partial class ExternalPortView : UserControl, IValueHolder, ISchemeComponent
    {
        public new Action<ExternalPortView> Tapped;

        public PortType Type;

        public bool? Value
        {
            get => _value;
            set
            {
                _value = value;
                XEllipse.Fill = Colorer.GetBrushByValue(value);
                ValueChanged?.Invoke(_value);
            }
        }
        private bool? _value;
        public Action<bool?> ValueChanged { get; set; }

        public Vector2 MatrixLocation
        {
            get => this.GetMatrixLocation();
            set => this.SetMatrixLocation(value);
        }

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
            XEllipse.RightTapped += (sender, args) => SwitchMode();
            PortName.RightTapped += (sender, args) => SwitchMode();
        }

        public void SwitchMode() => Value = Value == true ? false : Value == false ? null : true;

        public bool WireStartConnects(WireView wire) => wire.Connection.MatrixStart == MatrixLocation;
        public bool WireEndConnects(WireView wire) => wire.Connection.MatrixEnd == MatrixLocation;
    }
}
