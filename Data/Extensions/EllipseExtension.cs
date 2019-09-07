using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data.Extensions
{
    public static class EllipseExtension
    {
        public static Point CenterPoint(this Ellipse ellipse) => new Point
        {
            X = ellipse.Margin.Left + ellipse.Width / 2,
            Y = ellipse.Margin.Top + ellipse.Height / 2
        };

        public static void IncreaseSize(this Ellipse ellipse)
        {
            Point center = ellipse.CenterPoint();

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
            Point center = ellipse.CenterPoint();

            ellipse.Width /= 2;
            ellipse.Height /= 2;

            ellipse.Margin = new Thickness
            {
                Left = center.X - ellipse.Width / 2,
                Top = center.Y - ellipse.Height / 2
            };
        }
    }
}
