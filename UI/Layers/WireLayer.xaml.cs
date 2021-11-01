using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.UI.Dynamic;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Services;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class WireLayer : UserControl, ILayer<WireView>
    {
        public WireBuilder WireBuilder
        {
            get => wireBuilder;
            set
            {
                wireBuilder = value;
                wireBuilder.WireReady += wire => Add(wire);
            }
        }
        private WireBuilder wireBuilder;

        public WireLayer()
        {
            InitializeComponent();
            Grid.InitGridColumnsAndRows(SchemeView.GridSize);
        }

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
            wire.MakeCellIndependent(SchemeView.GridSize);

            Grid.Add(wire);
            wire.Tapped += Wire_Tapped;
        }
        
        public IEnumerable<WireView> Items => Grid.GetItems<WireView>();

        public void Clear() => Grid.Clear();

        #endregion
    }
}
