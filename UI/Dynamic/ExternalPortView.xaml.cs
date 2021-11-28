using System;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Services;
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
        private readonly Size externalPortSize = new(25, 25);

        public new Action<ExternalPortView> Tapped;
        public new Action<ExternalPortView> RightTapped;

        public readonly PortType Type;

        public Vector2 MatrixLocation
        {
            get => this.GetMatrixLocation();
            set => this.SetMatrixLocation(value);
        }

        public ExternalPortView() => InitializeComponent();

        public ExternalPortView(PortType type, Vector2 point)
        {
            InitializeComponent();

            this.SetSize(externalPortSize);
            
            Type = type;
            Value = null;
            MatrixLocation = point;

            PortName.Text = type == PortType.Input ? "In" : "Out";
            PortName.Tapped += (_, _) => Tapped(this);
            PortName.RightTapped += (_, _) => RightTapped(this);
            
            // only inputs support changing mode
            if (type == PortType.Input)
                PortName.DoubleTapped += (_, _) => SwitchValue();
        }

        #region ValueHolder
        
        public Action<bool?> ValueChanged { get; set; }

        public bool? Value
        {
            get => value;
            set
            {
                this.value = value;
                XEllipse.Fill = Colorer.GetBrushByValue(value);
                ValueChanged?.Invoke(this.value);
            }
        }
        private bool? value;
        
        public void SwitchValue() => this.SwitchControlValue();
        
        public void Reset() => this.ResetControlValue();
        
        #endregion

        #region Wire

        public bool WireConnects(WireView wire) => WireStartConnects(wire) || WireEndConnects(wire);
        
        public bool WireStartConnects(WireView wire) => wire.Connection.MatrixStart == MatrixLocation;
        
        public bool WireEndConnects(WireView wire) => wire.Connection.MatrixEnd == MatrixLocation;
        
        #endregion
    }
}
