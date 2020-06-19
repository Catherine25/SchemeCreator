using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data.Extensions
{
    public static class EllipseExtension
    {
        private static Thickness CalculateMargin(Point center, Size size) => new Thickness
        {
            Left = center.X - size.Width / 2,
            Top = center.Y - size.Height / 2
        };

        public static Point GetCenterPoint(this Ellipse ellipse) => new Point
        {
            X = ellipse.Margin.Left + ellipse.Width / 2,
            Y = ellipse.Margin.Top + ellipse.Height / 2
        };

        public static Size GetSize(this Ellipse ellipse) => new Size(ellipse.Width, ellipse.Height);

        public static void SetSize(this Ellipse ellipse, Size size)
        {
            ellipse.Width = size.Width;
            ellipse.Height = size.Height;
        }

        public static void SetSizeAndCenterPoint(this Ellipse ellipse, Size? size, Point? center)
        {
            if (size == null)
                size = GetSize(ellipse);

            if (center == null)
                center = GetCenterPoint(ellipse);

            ellipse.SetSize(size.Value);

            ellipse.Margin = CalculateMargin(center.Value, size.Value);
        }

        public static void IncreaseSize(this Ellipse ellipse)
        {
            Point center = ellipse.GetCenterPoint();

            ellipse.Width *= 2;
            ellipse.Height *= 2;

            ellipse.Margin = CalculateMargin(center, GetSize(ellipse));
        }

        public static void DecreaseSize(this Ellipse ellipse)
        {
            Point center = ellipse.GetCenterPoint();

            ellipse.Width /= 2;
            ellipse.Height /= 2;

            ellipse.Margin = CalculateMargin(center, GetSize(ellipse));
        }

        public static void SetStandartAlingment(this Ellipse ellipse)
        {
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.VerticalAlignment = VerticalAlignment.Top;
        }
    }
}
