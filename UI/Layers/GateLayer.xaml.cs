using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class GateLayer : UserControl, ILayer<GateView>
    {
        public event Action<GatePortView, GateView> GatePortTapped;
        public event Action<GateView> RemoveWiresByGateRequest;

        public IEnumerable<GateView> Items => Grid.Children.Select(e => e as GateView);

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
        }

        private void DeleteGate(GateBodyView gateBody, GateView gate)
        {
            Grid.Children.Remove(gate);
            RemoveWiresByGateRequest(gate);
        }

        public void Clear() => Grid.Children.Clear();
    }
}
