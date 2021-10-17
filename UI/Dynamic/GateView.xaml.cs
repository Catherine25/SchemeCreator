using SchemeCreator.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Models.Enums;
using static SchemeCreator.Data.Constants;
using Windows.UI.Xaml;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class GateView : UserControl, ISchemeComponent
    {
        public Action<GateBodyView, GateView> GateBodyTapped;
        public Action<GatePortView, GateView> GatePortTapped;

        public readonly GateEnum Type;
        public readonly Size GateBodySize;
        public readonly GridLength PortsMargin;
        public readonly Size PortSize;

        public Vector2 MatrixLocation
        {
            get => this.GetMatrixLocation();
            set => this.SetMatrixLocation(value);
        }

        public IEnumerable<GatePortView> Inputs { get => XInputs.Children.Select(p => p as GatePortView); }
        public IEnumerable<GatePortView> Outputs { get => XOutputs.Children.Select(p => p as GatePortView); }

        public bool AreOutputsReady => Outputs.Any(p => p.Value != null);

        public GateView(GateEnum type, Vector2 point, int inputs = 1, int outputs = 1)
        {
            Type = type;
            MatrixLocation = point;

            GateBodySize = LogicGateSize;
            PortSize = GatePortSize;

            PortsMargin = new GridLength(MarginBetweenPorts);

            InitializeComponent();

            xBody.Tapped += (sender, args) => GateBodyTapped(FindName("xBody") as GateBodyView, this);
            xBody.Content = GateNames[type];
            xBody.Width = LogicGateSize.Width;
            xBody.Height = LogicGateSize.Height;
            xBody.Foreground = Colorer.GetGateForegroundBrush();
            xBody.Background = Colorer.GetGateBackgroundBrush();

            XInputs.SetSize(GatePortSize.Width, LogicGateSize.Height);

            CreateInputs(inputs);
            CreateOutputs(outputs);
        }

        public void Work()
        {
            List<bool?> initialValues = Inputs.Select(p => p.Value).ToList();
            List<bool?> resultValues = GateWorkPatterns.ActionByType[Type](initialValues);

            var outPorts = Outputs.ToList();

            for (int i = 0; i < outPorts.Count; i++)
                outPorts[i].Value = resultValues[i];
        }

        public void Reset() => Inputs.ToList().ForEach(x => x.Value = null);

        #region Ports creation

        private void CreateInputs(int count)
        {
            for (int i = 0; i < count; i++)
                XInputs.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < count; i++)
            {
                GatePortView port = new(ConnectionTypeEnum.Input, i);
                port.Tapped += (port) => GatePortTapped(port, this);
                port.ValueChanged += newValue => { Work(); };
                XInputs.Children.Add(port);
            }
        }

        private void CreateOutputs(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GatePortView port = new(ConnectionTypeEnum.Output, i);
                port.Tapped += (port) => GatePortTapped(port, this);
                XOutputs.Children.Add(port);
            }
        }

        #endregion

        #region Interaction with Wires

        public void SetInputValueFromWire(WireView wire) => Inputs.First(i => i.Index == wire.Connection.EndPort).Value = wire.Value;
        public void SetOutputValueToWire(WireView wire) => wire.Value = Outputs.First(o => o.Index == wire.Connection.StartPort).Value;
        public bool WireStartConnects(WireView wire) =>
            (MatrixLocation == wire.Connection.MatrixStart) && (wire.Connection.StartPort == null || Outputs.Any(i => i.Index == wire.Connection.StartPort));

        public bool WireEndConnects(WireView wire) =>
            (MatrixLocation == wire.Connection.MatrixEnd) && (wire.Connection.EndPort == null || Inputs.Any(i => i.Index == wire.Connection.EndPort));

        public bool WireConnects(WireView wire) =>
            (MatrixLocation == wire.Connection.MatrixStart) || (MatrixLocation == wire.Connection.MatrixEnd);

        #endregion
    }
}
