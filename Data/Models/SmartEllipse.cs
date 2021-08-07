using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.Data.Services;
using System;
using System.Numerics;
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
                _ellipse.CenterPoint = new Vector3(_ellipse.CenterPoint.X * 2, _ellipse.CenterPoint.Y *2, _ellipse.CenterPoint.Z);

            _ellipse.PointerExited += (object o, PointerRoutedEventArgs e) =>
                _ellipse.CenterPoint = new Vector3(_ellipse.CenterPoint.X / 2, _ellipse.CenterPoint.Y / 2, _ellipse.CenterPoint.Z);
        }

        #region Fields

        public Size Size => _ellipse.GetSize();

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

        public void SetSize(Size size) => _ellipse.SetSize(size);

        public Action<SmartEllipse> Tapped;
    }
}
