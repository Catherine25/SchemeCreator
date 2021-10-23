using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.Data.Services;
using SchemeCreator.UI.Dynamic;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class WireLayer : UserControl, ILayer<WireView>
    {
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
        private void WireReady(WireView wire) => Add(wire);

        private void Wire_Tapped(WireView wire) => Grid.Remove(wire);

        public void RemoveConnectedWires(ISchemeComponent component)
        {
            // ToList() is needed to remove wires correctly
            var wiresToRemove = Items.Where(wire => component.WireConnects(wire)).ToList();

            foreach (var wire in wiresToRemove)
                Grid.Remove(wire);
        }

        #region ILayer

        public void Add(WireView wire)
        {
            Grid.SetColumnSpan(wire, Constants.NetSize);
            Grid.SetRowSpan(wire, Constants.NetSize);

            Grid.Add(wire);
            wire.Tapped += Wire_Tapped;
        }
        
        public IEnumerable<WireView> Items => Grid.GetItems<WireView>();

        public void Clear() => Grid.Clear();

        #endregion
    }
}
