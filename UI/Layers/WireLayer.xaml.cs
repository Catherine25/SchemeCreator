using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services;
using SchemeCreator.UI.Dynamic;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class WireLayer : UserControl
    {
        public List<WireView> Wires { get; private set; }
        public WireBuilder WireBuilder;

        public WireLayer()
        {
            InitializeComponent();
            Grid.InitGridColumnsAndRows(Constants.GridSize);
            Wires = new List<WireView>();
            WireBuilder = new WireBuilder();
            WireBuilder.WireReady = (wire) => AddToView(wire);
        }

        private void Wire_Tapped(WireView wire)
        {
            Wires.Remove(wire);
            Grid.Children.Remove(wire);
        }

        public void AddToView(WireView wire)
        {
            System.Diagnostics.Debug.WriteLine("AddToView");
            
            Grid.SetColumnSpan(wire, Constants.NetSize);
            Grid.SetRowSpan(wire, Constants.NetSize);

            Wires.Add(wire);
            Grid.Children.Add(wire);
            wire.Tapped += Wire_Tapped;
        }
    }
}
