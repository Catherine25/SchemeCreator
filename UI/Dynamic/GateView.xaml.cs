using SchemeCreator.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using SchemeCreator.Data.Models.Enums;
using static SchemeCreator.Data.Constants;
using Windows.UI.Xaml;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Models;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class GateView : UserControl
    {
        public readonly GateEnum Type;
        public readonly string Text;
        public readonly Size GateBodySize;
        public readonly GridLength PortsMargin;
        public readonly Size PortSize;
        private GridLength PortWidth => new GridLength(PortSize.Width);

        public Vector2 MatrixLocation
        {
            get => _matrixIndex;
            set
            {
                _matrixIndex = value;
                Grid.SetColumn(this, (int)_matrixIndex.X);
                Grid.SetRow(this, (int)_matrixIndex.Y);

                //must be called to update coordinates immediately
                UpdateLayout();
            }
        }
        private Vector2 _matrixIndex;

        public readonly Brush ForegroundBrush;
        public readonly Brush BackgroundBrush;

        public Action<GateBodyView, GateView> GateBodyTapped;
        public Action<GatePortView, GateView> GatePortTapped;

        public IEnumerable<GatePortView> Inputs { get => XInputs.Children.Select(p => p as GatePortView); }
        public IEnumerable<GatePortView> Outputs { get => XOutputs.Children.Select(p => p as GatePortView); }

        public bool AreOutputsReady => Outputs.Any(p => p.Value != null);

        public GateView(GateEnum type, Vector2 point, int inputs = 1, int outputs = 1)
        {
            Type = type;
            MatrixLocation = point;
            Text = GateNames[type];

            ForegroundBrush = Colorer.GetGateForegroundBrush();
            BackgroundBrush = Colorer.GetGateBackgroundBrush();

            GateBodySize = LogicGateSize;
            PortSize = GatePortSize;

            PortsMargin = new GridLength(MarginBetweenPorts);

            InitializeComponent();

            xBody.Tapped += (sender, args) => GateBodyTapped(FindName("xBody") as GateBodyView, this);
            xBody.Width = LogicGateSize.Width;
            xBody.Height = LogicGateSize.Height;

            XInputs.SetSize(GatePortSize.Width, LogicGateSize.Height);

            CreatePorts(inputs, outputs);
        }

        public void Work()
        {
            List<bool?> initialValues = Inputs.Select(p => p.Value).ToList();
            List<bool?> resultValues = GateWorkPatterns.ActionByType[Type](initialValues);

            var outPorts = Outputs.ToList();

            for (int i = 0; i < outPorts.Count; i++)
                outPorts[i].Value = resultValues[i];
        }

        private void CreatePorts(int inputs, int outputs)
        {
            CreateInputs(inputs);
            CreateOutputs(outputs);
        }

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

        public void SetInputValueFromWire(WireView wire) =>
            Inputs.First(i => i.Index == wire.Connection.EndPort).Value = wire.Value;

        public void SetOutputValueToWire(WireView wire) =>
            wire.Value = Outputs.First(o => o.Index == wire.Connection.StartPort).Value;

        public void Reset()
        {
            foreach (var child in Inputs)
                child.Value = null;
        }

        public bool WireStartConnects(WireView wire) =>
            (MatrixLocation == wire.Connection.MatrixStart) && (wire.Connection.StartPort == null || Outputs.Any(i => i.Index == wire.Connection.StartPort));

        public bool WireEndConnects(WireView wire) =>
            (MatrixLocation == wire.Connection.MatrixEnd) && (wire.Connection.EndPort == null || Inputs.Any(i => i.Index == wire.Connection.EndPort));

        public bool WireConnects(WireView wire) =>
            (MatrixLocation == wire.Connection.MatrixStart) || (MatrixLocation == wire.Connection.MatrixEnd);
    }
}
