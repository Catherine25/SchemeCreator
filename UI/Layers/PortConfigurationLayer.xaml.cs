using System;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.UI.Dynamic;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class PortConfigurationLayer : UserControl, ILayer<PortConfigurationView>, IGridComponent
    {
        public Action<PortConfigurationLayer> LayerTapped;
        public PortConfigurationLayer()
        {
            InitializeComponent();
            DeleteBt.Click += DeleteBtOnClick;
        }

        public Vector2 MatrixLocation
        {
            get => this.GetMatrixLocation();
            set => this.SetMatrixLocation(value);
        }
        
        private void DeleteBtOnClick(object sender, RoutedEventArgs e) => LayerTapped(this);
        
        #region ILayer
        
        public IEnumerable<PortConfigurationView> Items => Grid.GetItems<PortConfigurationView>();

        public void Add(PortConfigurationView item)
        {
            int columns = Grid.ColumnDefinitions.Count;
            item.MatrixLocation = new Vector2(columns, 0);
            Grid.ColumnDefinitions.Add(new ColumnDefinition());
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
