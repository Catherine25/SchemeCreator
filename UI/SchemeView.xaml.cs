using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services.History;
using SchemeCreator.Data.Services.Serialization;
using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
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
            
            ExternalPortsLayer.Tapped += ExternalPortTapped;
            ExternalPortsLayer.RemoveConnectedWires += RemoveConnectedWires;
            
            DotLayer.Tapped += DotTappedEventAsync;
            DotLayer.RightTapped += DotRightTappedEventAsync;
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
            foreach (ExternalPortView port in ExternalOutputs)
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
                port.Type != ConnectionTypeEnum.Input,
                port.GetCenterRelativeTo(XSchemeGrid),
                gate.MatrixLocation,
                port.Index);

        private async void DotTappedEventAsync(DotView dot)
        {
            var msg = new NewGateDialog();
            var result = await msg.ShowAsync();

            if(result != ContentDialogResult.Primary)
                return;

            GateView? gate = msg.Gate;
            // todo fix
            // if (gate.Type == GateEnum.Custom)
            // {
            //     var msg2 = new CustomGateDialog(gate.Inputs.Count());
            //     result = await msg2.ShowAsync();
            //
            //     if (result != ContentDialogResult.Primary)
            //         return;
            //
            //     gate.ConfigureCustomWorkFunction(msg2.exceptionsData);
            // }

            gate.MatrixLocation = dot.MatrixLocation;
            GateLayer.Add(gate);
        }

        private async void DotRightTappedEventAsync(DotView dot)
        {
            var msg = new NewExternalPortDialog();
            var result = await msg.ShowAsync();

            if (result != ContentDialogResult.Primary)
                return;

            ExternalPortView port = msg.ExternalPortView;
            port.MatrixLocation = dot.MatrixLocation;
            
            ExternalPortsLayer.Add(port);
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
