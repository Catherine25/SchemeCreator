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
        public new Action<ExternalPortView> RightTapped;

        public readonly PortType Type;

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
        
        public Point Center => new(CenterPoint.X, CenterPoint.Y);
        
        public ExternalPortView() => InitializeComponent();

        public ExternalPortView(PortType type, Vector2 point)
        {
            InitializeComponent();

            Type = type;

            PortName.Text = type == PortType.Input ? "In" : "Out";

            this.SetSize(Constants.ExternalPortSize);

            Value = null;
            MatrixLocation = point;
            
            PortName.Tapped += (_, _) => Tapped(this);
            PortName.RightTapped += (_, _) => RightTapped(this);
            PortName.DoubleTapped += (_, _) => SwitchMode();
        }

        private void SwitchMode() => Value = Value == true ? false : Value == false ? null : true;

        #region Wire

        public bool WireConnects(WireView wire) => WireStartConnects(wire) || WireEndConnects(wire);
        
        public bool WireStartConnects(WireView wire) => wire.Connection.MatrixStart == MatrixLocation;
        
        public bool WireEndConnects(WireView wire) => wire.Connection.MatrixEnd == MatrixLocation;
        
        #endregion
    }
}
