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

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class GateView : UserControl
    {
        public Guid Guid { get; set; } = new Guid();
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
            }
        } 
        private Vector2 _matrixIndex;

        public readonly Brush ForegroundBrush;
        public readonly Brush BackgroundBrush;

        public Action<GateBodyView, GateView> GateBodyTapped;
        public Action<GatePortView, GateView> GatePortTapped;

        public int InputCount;
        public int OutputCount;

        public bool AreOutputsReady => XOutputs.Children.Any(x => ((GatePortView) x).Value != null);

        public GateView(GateEnum type, Vector2 point, int inputs = 0, int outputs = 0)
        {
            Guid = new Guid();
            Type = type;
            InputCount = inputs;
            OutputCount = outputs;
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
            List<bool?> values =
                GateWorkPatterns.ActionByType[Type](XInputs.Children
                    .Select(x => ((GatePortView) x).Value)
                    .ToList());

            var outPorts = XOutputs.Children.Select(o => o as GatePortView).ToList();
            
            for (int i = 0; i < outPorts.Count; i++)
                outPorts[i].Value = values[i];
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

        public void SetInputValueFromWire(WireView wire)
        {
            XInputs.Children
                .Select(x => x as GatePortView)
                .FirstOrDefault(x => x.Center == wire.End)
                .Value = wire.Value;
        }

        public void Reset()
        {
            foreach (var child in XInputs.Children)
                (child as GatePortView).Value = null;
        }

        public bool WirePartConnects(Point point)
        {
            var inputs = XInputs.Children.Select(i => i as GatePortView);
            var outputs = XOutputs.Children.Select(i => i as GatePortView);

            return inputs.Any(i => i.Center == point) || outputs.Any(i => i.Center == point);
        }

        public bool WireConnects(WireView wire) => WirePartConnects(wire.Start) || WirePartConnects(wire.End);
    }
}
