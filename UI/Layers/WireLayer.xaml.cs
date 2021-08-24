using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services;
using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class WireLayer : UserControl
    {

        public IEnumerable<WireView> Wires { get => Grid.Children.Select(c => c as WireView); }
        public WireBuilder WireBuilder;

        public WireLayer()
        {
            InitializeComponent();
            Grid.InitGridColumnsAndRows(Constants.GridSize);
            WireBuilder = new WireBuilder();
            WireBuilder.WireReady += WireReady;
        }

        /// <summary>
        /// Adds wire to the view and requests scheme validating.
        /// </summary>
        /// <param name="wire"></param>
        private void WireReady(WireView wire) => AddToView(wire);

        private void Wire_Tapped(WireView wire)
        {
            Grid.Children.Remove(wire);
        }

        public void RemoveWiresByGate(GateView gate)
        {
            // ToList() is needed to remove wires correctly
            var wiresToRemove = Wires.Where(wire => gate.WireConnects(wire)).ToList();

            foreach (var wire in wiresToRemove)
                Grid.Children.Remove(wire);
        }

        public void AddToView(WireView wire)
        {
            Grid.SetColumnSpan(wire, Constants.NetSize);
            Grid.SetRowSpan(wire, Constants.NetSize);

            Grid.Children.Add(wire);
            wire.Tapped += Wire_Tapped;
        }
    }
}
