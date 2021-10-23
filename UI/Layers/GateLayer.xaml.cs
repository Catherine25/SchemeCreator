using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class GateLayer : UserControl, ILayer<GateView>
    {
        public event Action<GatePortView, GateView> GatePortTapped;
        public event Action<GateView> RemoveConnectedWires;

        public GateLayer()
        {
            InitializeComponent();
            Grid.InitGridColumnsAndRows(SchemeView.GridSize);
        }

        private void DeleteGate(GateBodyView gateBody, GateView gate)
        {
            Grid.Remove(gate);
            RemoveConnectedWires(gate);
        }

        #region ILayer

        public void Add(GateView gate)
        {
            gate.GateBodyTapped += (body, g) => DeleteGate(body, g);
            gate.GatePortTapped += (port, g) => GatePortTapped(port, g);
            Grid.Add(gate);
        }

        public IEnumerable<GateView> Items => Grid.GetItems<GateView>();

        public void Clear() => Grid.Clear();

        #endregion
    }
}
