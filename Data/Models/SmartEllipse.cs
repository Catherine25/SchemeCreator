using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.Data.Services;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data.Models
{
    public class SmartEllipse : IGridChild
    {
        public SmartEllipse()
        {
            _ellipse = new Ellipse
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            _ellipse.Tapped += (object o, TappedRoutedEventArgs e) => Tapped(this);

            _ellipse.PointerEntered += (object o, PointerRoutedEventArgs e) =>
                SetCenterAndSize(CenterPoint, new Size(Size.Width * 2, Size.Height * 2));

            _ellipse.PointerExited += (object o, PointerRoutedEventArgs e) =>
                SetCenterAndSize(CenterPoint, new Size(Size.Width / 2, Size.Height / 2));
        }

        #region Fields

        public Size Size => _ellipse.GetSize();

        public Point CenterPoint => _ellipse.GetCenterPoint();

        public bool? BooleanValue
        {
            get => booleanValue;
            set
            {
                booleanValue = value;
                Colorer.SetFillByValue(_ellipse, BooleanValue);
            }
        }
        private bool? booleanValue;

        private Ellipse _ellipse;

        #endregion

        public void AddToParent(SmartGrid grid) => grid.Add(_ellipse);

        public void SetCenterAndSize(Point? point = null, Size? size = null) => _ellipse.SetSizeAndCenterPoint(size, point);

        public Action<SmartEllipse> Tapped;
    }
}
