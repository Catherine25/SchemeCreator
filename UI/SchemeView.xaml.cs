using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services.Serialization;
using SchemeCreator.UI.Dynamic;
using SchemeCreator.UI.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.UI
{
    public sealed partial class SchemeView : UserControl
    {
        public SchemeView()
        {
            InitializeComponent();
            GateLayer.GatePortTapped += GatePortTapped;
            ExternalPortsLayer.ExternalPortTapped += ExternalPortTapped;
            DotLayer.DotTapped += DotTappedEventAsync;

            DotLayer.InitGrid(Constants.GridSize);
        }

        public ExternalPortView GetFirstNotInitedExternalPort() => ExternalPortsLayer.GetFirstNotInitedExternalPort();

        public bool ValidateScheme()
        {
            var gates = new Stack<GateView>(GateLayer.Gates);
            var wires = new List<WireView>(WireLayer.Wires);

            bool found = true;

            if (AllExternalPortsConnect(PortType.Input, wires))
                return false;

            if (AllExternalPortsConnect(PortType.Output, wires))
                return false;

            while (gates.Count != 0 && found)
            {
                GateView gate = gates.Pop();
                found = false;

                int connections = gate.InputCount + gate.OutputCount;

                foreach (var wire in wires)
                    if (gate.WireConnects(wire.Start) || gate.WireConnects(wire.End))
                        connections--;

                if (connections <= 0)
                    found = true;
            }

            return gates.Count == 0;
        }

        public IEnumerable<GateView> Gates { get => GateLayer.Gates; }
        public IEnumerable<WireView> Wires { get => WireLayer.Wires; }
        public IEnumerable<ExternalPortView> ExternalPorts { get => ExternalPortsLayer.ExternalPorts; }
        
        public void AddToView(GateView g) => GateLayer.AddToView(g);
        public void AddToView(WireView w) => WireLayer.AddToView(w);

        private bool AllExternalPortsConnect(PortType type, List<WireView> wires)
        {
            IEnumerable<Point> wirePointsToCheck = type == PortType.Input
                ? wires.Select(x => x.Start)
                : wires.Select(x => x.End);

            var allExternalPorts = ExternalPortsLayer.ExternalPorts;
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

        //public void ResetGates()
        //{
        //    foreach (GateView gate in Gates)
        //        gate.Reset();
        //}

        public void Clear()
        {
            var toRemove = XSchemeGrid.Children.Where(x => !(x is Ellipse));

            while(toRemove.Any())
                XSchemeGrid.Children.Remove(toRemove.Take(1).First());
        }

        private void ExternalPortTapped(ExternalPortView externalPort) =>
            WireLayer.WireBuilder.SetPoint(externalPort.Type == PortType.Input, externalPort.GetCenterRelativeTo(XSchemeGrid));

        private void GatePortTapped(GatePortView arg1, GateView arg2) =>
            WireLayer.WireBuilder.SetPoint(arg1.Type != Data.Models.Enums.ConnectionTypeEnum.Input, arg1.GetCenterRelativeTo(XSchemeGrid));

        private async void DotTappedEventAsync(Ellipse e)
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

        public void Recreate()
        {
            Clear();

            // WireBuilder = new();
            // WireBuilder.WireReady = (wire) => AddToView(wire);

            // Wires = new List<WireView>();
            // Gates = new List<GateView>();
            // ExternalPorts = new List<ExternalPortView>();
        }

        public (IEnumerable<GateDto>, IEnumerable<WireDto>) PrepareForSerialization() 
        {
            var gateDtos = GateLayer.Gates.Select(gate => new GateDto(gate));
            var wireDtos = WireLayer.Wires.Select(wire => new WireDto(wire));

            return(gateDtos, wireDtos);
        }
    }
}
