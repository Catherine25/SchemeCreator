using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data.Extensions
{
    public static class EllipseExtension
    {
        public static Point GetCenterPoint(this Ellipse ellipse) => new Point
        {
            X = ellipse.Margin.Left + ellipse.Width / 2,
            Y = ellipse.Margin.Top + ellipse.Height / 2
        };

        public static void SetSize(this Ellipse ellipse, Size size)
        {
            ellipse.Width = size.Width;
            ellipse.Height = size.Height;
        }

        public static void SetSizeAndCenterPoint(this Ellipse ellipse, Size size, Point center)
        {
            ellipse.SetSize(size);

            ellipse.Margin = new Thickness
            {
                Left = center.X - size.Width / 2,
                Top = center.Y - size.Height / 2
            };
        }

        public static void IncreaseSize(this Ellipse ellipse)
        {
            Point center = ellipse.GetCenterPoint();

            ellipse.Width *= 2;
            ellipse.Height *= 2;

            ellipse.Margin = new Thickness
            {
                Left = center.X - ellipse.Width / 2,
                Top = center.Y - ellipse.Height / 2
            };
        }

        public static void DecreaseSize(this Ellipse ellipse)
        {
            Point center = ellipse.GetCenterPoint();

            ellipse.Width /= 2;
            ellipse.Height /= 2;

            ellipse.Margin = new Thickness
            {
                Left = center.X - ellipse.Width / 2,
                Top = center.Y - ellipse.Height / 2
            };
        }

        public static void SetStandartAlingment(this Ellipse ellipse)
        {
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.VerticalAlignment = VerticalAlignment.Top;
        }

        public static void SetFillByValue(this Ellipse ellipse, bool? value)
        {
            if (value == true)
                ellipse.Fill = ConstantsNamespace.Constants.brushes[ConstantsNamespace.Constants.AccentEnum.light1];
            else if (value == false)
                ellipse.Fill = ConstantsNamespace.Constants.brushes[ConstantsNamespace.Constants.AccentEnum.dark1];
            else
                ellipse.Fill = ConstantsNamespace.Constants.brushes[ConstantsNamespace.Constants.AccentEnum.accent2];
        }
    }
}
