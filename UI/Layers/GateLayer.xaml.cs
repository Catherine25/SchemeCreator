using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class GateLayer : UserControl
    {
        public event Action<GatePortView, GateView> GatePortTapped;
        //public event Action<GateBodyView, GateView> GateBodyTapped;
        public event Action<GateView> RemoveWiresByGateRequest;

        public IList<GateView> Gates { get => Grid.Children.Select(e => e as GateView).ToList(); }
        private void SetGates(List<GateView> gates) => gates.ForEach(g => Grid.Children.Add(g));

        public GateLayer()
        {
            InitializeComponent();
            Grid.InitGridColumnsAndRows(Constants.GridSize);
        }

        public void AddToView(GateView gate)
        {
            gate.GateBodyTapped += (gateBody, gate) => DeleteGate(gateBody, gate);
            gate.GatePortTapped += (gatePort, gate) => GatePortTapped(gatePort, gate);
            Grid.Children.Add(gate);
            Gates.Add(gate);
        }

        private void DeleteGate(GateBodyView gateBody, GateView gate)
        {
            Grid.Children.Remove(gate);
            RemoveWiresByGateRequest(gate);
        }
    }
}
