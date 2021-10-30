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

        public DotView()
        {
            InitializeComponent();
            this.SetCenterAlignment();
            this.SetSize(dotSize);

            Ellipse.PointerEntered += (sender, args) => Ellipse.Activate();
            Ellipse.PointerExited += (sender, args) => Ellipse.Deactivate();
            Ellipse.Tapped += (sender, args) => Tapped(this);
            Ellipse.RightTapped += (sender, args) => RightTapped(this);

            Colorer.SetFillByValue(Ellipse, false);
        }

        public Vector2 MatrixLocation
        {
            get => this.GetMatrixLocation();
            set => this.SetMatrixLocation(value);
        }
    }
}
