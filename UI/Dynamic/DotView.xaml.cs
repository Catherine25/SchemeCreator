using System;
using System.Numerics;
using Windows.Foundation;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Interfaces;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class DotView : UserControl, IGridComponent
    {
        public new Action<DotView> Tapped;
        public new Action<DotView> RightTapped;
        private readonly Size dotSize = new(15, 15);
        private readonly Size activeDotSize = new(25, 25);

        public DotView()
        {
            InitializeComponent();
            this.SetCenterAlignment();
            this.SetSize(dotSize);
            
            Ellipse.Fill = Colorer.InactiveBrush;
            Ellipse.Stroke = Colorer.ActiveBrush;

            Ellipse.PointerEntered += (_, _) => this.SetSize(activeDotSize);
            Ellipse.PointerExited += (_, _) => this.SetSize(dotSize);
            Ellipse.Tapped += (_, _) => Tapped(this);
            Ellipse.RightTapped += (_, _) => RightTapped(this);
        }

        public Vector2 MatrixLocation
        {
            get => this.GetMatrixLocation();
            set => this.SetMatrixLocation(value);
        }
    }
}
