using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class DotLayer : UserControl
    {
        public event Action<Ellipse> DotTapped;

        public List<Ellipse> Dots
        {
            get => Grid.Children.Select(e => e as Ellipse).ToList();
            private set => value.ForEach(e => Grid.Children.Add(e));
        }

        public DotLayer() => InitializeComponent();

        public void InitGrid(Size size)
        {
            Grid.InitGridColumnsAndRows(size);

            for (int i = 1; i <= size.Width; i++)
                for (int j = 1; j <= size.Height; j++)
                {
                    Ellipse ellipse = new Ellipse
                    {
                        HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                        VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
                        Width = Constants.DotSize.Width,
                        Height = Constants.DotSize.Height
                    };

                    Grid.SetRow(ellipse, i - 1);
                    Grid.SetColumn(ellipse, j - 1);

                    ellipse.PointerEntered += (sender, args) => ellipse.Activate();
                    ellipse.PointerExited += (sender, args) => ellipse.Deactivate();
                    ellipse.Tapped += (sender, args) => DotTapped(sender as Ellipse);

                    Colorer.SetFillByValue(ellipse, false);

                    Dots.Add(ellipse);
                    Grid.Children.Add(ellipse);
                }
        }
    }
}
