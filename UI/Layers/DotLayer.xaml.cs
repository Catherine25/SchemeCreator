using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class DotLayer : UserControl, ILayer<DotView>
    {
        public event Action<DotView> Tapped;
        public event Action<DotView> RightTapped;

        public DotLayer() => InitializeComponent();

        public void InitGrid(Size size)
        {
            Grid.InitGridColumnsAndRows(size);

            for (int i = 1; i <= size.Width; i++)
                for (int j = 1; j <= size.Height; j++)
                {
                    DotView dot = new();
                    dot.MatrixLocation = new(i - 1, j - 1);
                    dot.Tapped += (d) => Tapped(d);
                    dot.RightTapped += (d) => RightTapped(d);
                    Add(dot);
                }
        }

        #region ILayer
        
        public void Add(DotView e) => Grid.Add(e);
        
        public IEnumerable<DotView> Items => Grid.GetItems<DotView>();
        
        public void Clear() => Grid.Clear();

        
        #endregion
    }
}
