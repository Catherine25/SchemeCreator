using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services.History;
using SchemeCreator.Data.Services.Serialization;
using SchemeCreator.UI.Dynamic;
using SchemeCreator.UI.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI
{
    public sealed partial class SchemeView : UserControl
    {
        public SchemeView()
        {
            InitializeComponent();
            GateLayer.GatePortTapped += GatePortTapped;
            GateLayer.RemoveWiresByGateRequest += RemoveWiresByGate;
            ExternalPortsLayer.ExternalPortTapped += ExternalPortTapped;
            DotLayer.DotTapped += DotTappedEventAsync;

            DotLayer.InitGrid(Constants.GridSize);
        }

        private void RemoveWiresByGate(GateView gate) => WireLayer.RemoveWiresByGate(gate);

        public ExternalPortView GetFirstNotInitedExternalPort() => ExternalPortsLayer.GetFirstNotInitedExternalPort();

        public IEnumerable<GateView> Gates { get => GateLayer.Items; }
        public IEnumerable<WireView> Wires { get => WireLayer.Items; }
        public IEnumerable<ExternalPortView> ExternalPorts { get => ExternalPortsLayer.Items; }
        public IEnumerable<ExternalPortView> ExternalInputs { get => ExternalPortsLayer.Items.Where(x => x.Type == PortType.Input); }
        public IEnumerable<ExternalPortView> ExternalOutputs { get => ExternalPortsLayer.Items.Where(x => x.Type == PortType.Output); }

        public void AddToView(ExternalPortView p) => ExternalPortsLayer.AddToView(p);

        public void AddToView(GateView g) => GateLayer.AddToView(g);
        public void AddToView(WireView w) => WireLayer.AddToView(w);

        private bool AllExternalPortsConnect(PortType type, List<WireView> wires)
        {
            IEnumerable<Point> wirePointsToCheck = type == PortType.Input
                ? wires.Select(x => x.Connection.StartPoint)
                : wires.Select(x => x.Connection.EndPoint);

            var allExternalPorts = ExternalPortsLayer.Items;
            var externalPorts = new Stack<ExternalPortView>(allExternalPorts.Where(x => x.Type == type));

            bool found = true;

            while (externalPorts.TryPop(out var externalPort) && found)
            {
                found = false;

                foreach (Point point in wirePointsToCheck)
                    if (externalPort.Center == point)
                    {
                        found = true;
                        break;
                    }
            }

            return found;
        }

        public void Reset()
        {
            foreach (ExternalPortView port in ExternalPorts.Where(p => p.Type == PortType.Output))
                port.Value = null;

            foreach (GateView gate in Gates)
                gate.Reset();

            foreach (WireView wire in Wires)
                wire.Value = null;
        }

        public void Clear()
        {
            ExternalPortsLayer.Clear();
            GateLayer.Clear();
            WireLayer.Clear();
            TraceLayer.Clear();
        }

        public void Recreate() => Clear();

        private void ExternalPortTapped(ExternalPortView externalPort) =>
            WireLayer.WireBuilder.SetPoint(
                externalPort.Type == PortType.Input,
                externalPort.GetCenterRelativeTo(XSchemeGrid),
                externalPort.MatrixLocation);

        private void GatePortTapped(GatePortView port, GateView gate) =>
            WireLayer.WireBuilder.SetPoint(
                port.Type != Data.Models.Enums.ConnectionTypeEnum.Input,
                port.GetCenterRelativeTo(XSchemeGrid),
                gate.MatrixLocation,
                port.Index);

        private async void DotTappedEventAsync(DotView e)
        {
            var msg = new NewGateDialog();

            await msg.ShowAsync();

            if (msg.gateType != null)
            {
                var location = new Vector2(Grid.GetColumn(e), Grid.GetRow(e));

                if(msg.Gate != null)
                {
                    msg.Gate.MatrixLocation = location;
                    GateLayer.AddToView(msg.Gate);
                }
                else if(msg.ExternalPort != null)
                {
                    msg.ExternalPort.MatrixLocation = location;
                    ExternalPortsLayer.AddToView(msg.ExternalPort);
                }
            }
        }

        public (IEnumerable<GateDto>, IEnumerable<WireDto>) PrepareForSerialization() 
        {
            var gateDtos = GateLayer.Items.Select(gate => new GateDto(gate));
            var wireDtos = WireLayer.Items.Select(wire => new WireDto(wire));

            return(gateDtos, wireDtos);
        }

        public void ShowTracings(HistoryService historyService) => TraceLayer.ShowTracings(historyService);
        public void ClearTracings() => TraceLayer.Clear();
    }
}
