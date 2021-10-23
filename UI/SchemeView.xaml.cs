using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services.History;
using SchemeCreator.Data.Services.Serialization;
using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Interfaces;

namespace SchemeCreator.UI
{
    public sealed partial class SchemeView : UserControl
    {
        public static readonly Size GridSize = new(8, 8);

        public SchemeView()
        {
            InitializeComponent();

            GateLayer.GatePortTapped += GatePortTapped;
            GateLayer.RemoveConnectedWires += RemoveConnectedWires;
            
            ExternalPortsLayer.Tapped += Tapped;
            ExternalPortsLayer.RemoveConnectedWires += RemoveConnectedWires;
            
            DotLayer.Tapped += TappedEventAsync;
            DotLayer.InitGrid(GridSize);
        }

        private void RemoveConnectedWires(ISchemeComponent schemeComponent) => WireLayer.RemoveConnectedWires(schemeComponent);
        
        public ExternalPortView GetFirstNotInitedExternalPort() => ExternalPortsLayer.GetFirstNotInitedExternalPort();

        public IEnumerable<GateView> Gates => GateLayer.Items;
        public IEnumerable<WireView> Wires => WireLayer.Items;
        public IEnumerable<ExternalPortView> ExternalPorts => ExternalPortsLayer.Items;
        public IEnumerable<ExternalPortView> ExternalInputs => ExternalPortsLayer.Items.Where(x => x.Type == PortType.Input);
        public IEnumerable<ExternalPortView> ExternalOutputs => ExternalPortsLayer.Items.Where(x => x.Type == PortType.Output);

        public void AddToView(ExternalPortView p) => ExternalPortsLayer.Add(p);

        public void AddToView(GateView g) => GateLayer.Add(g);
        public void AddToView(WireView w) => WireLayer.Add(w);

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

        private void Tapped(ExternalPortView externalPort) =>
            WireLayer.WireBuilder.SetPoint(
                externalPort.Type == PortType.Input,
                externalPort.GetCenterRelativeTo(XSchemeGrid),
                externalPort.MatrixLocation);

        private void GatePortTapped(GatePortView port, GateView gate) =>
            WireLayer.WireBuilder.SetPoint(
                port.Type != ConnectionTypeEnum.Input,
                port.GetCenterRelativeTo(XSchemeGrid),
                gate.MatrixLocation,
                port.Index);

        private async void TappedEventAsync(DotView e)
        {
            var msg = new NewGateDialog();

            await msg.ShowAsync();

            if (msg.gateType != null)
            {
                var location = new Vector2(Grid.GetColumn(e), Grid.GetRow(e));

                if(msg.Gate != null)
                {
                    msg.Gate.MatrixLocation = location;
                    GateLayer.Add(msg.Gate);
                }
                else if(msg.ExternalPort != null)
                {
                    msg.ExternalPort.MatrixLocation = location;
                    ExternalPortsLayer.Add(msg.ExternalPort);
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
