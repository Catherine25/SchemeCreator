using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class ExceptionsLayer : UserControl, ILayer<PortConfigurationLayer>, IGridComponent
    {
        public ExceptionsLayer() => InitializeComponent();
        
        public Vector2 MatrixLocation
        {
            get => this.GetMatrixLocation();
            set => this.SetMatrixLocation(value);
        }

        #region ILayer
        
        public IEnumerable<PortConfigurationLayer> Items => Grid.GetItems<PortConfigurationLayer>();

        public void Add(PortConfigurationLayer item)
        {
            int rowCount = Grid.RowDefinitions.Count;
            item.MatrixLocation = new Vector2(0, rowCount);
            item.LayerTapped += (l) =>
            {
                var y = l.MatrixLocation.Y;
                Grid.RowDefinitions.Remove(Grid.RowDefinitions.Last());
                Grid.Remove(l);
            };
            Grid.RowDefinitions.Add(new RowDefinition());
            Grid.Add(item);
        }

        public void Clear()
        {
            Grid.InitGridColumnsAndRows(new(0, 0));
            Grid.Clear();
        }

        #endregion
    }
}
