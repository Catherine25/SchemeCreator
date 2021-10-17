using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class DotLayer : UserControl, ILayer<DotView>
    {
        public event Action<DotView> DotTapped;

        public IEnumerable<DotView> Items => Grid.Children.Select(e => e as DotView);

        public DotLayer() => InitializeComponent();

        public void InitGrid(Size size)
        {
            Grid.InitGridColumnsAndRows(size);

            for (int i = 1; i <= size.Width; i++)
                for (int j = 1; j <= size.Height; j++)
                {
                    DotView dot = new(i, j);
                    dot.Tapped += (dot) => DotTapped(dot);
                    AddToView(dot);
                }
        }

        public void AddToView(DotView e) => Grid.Children.Add(e);

        public void Clear() => Grid.Children.Clear();
    }
}
