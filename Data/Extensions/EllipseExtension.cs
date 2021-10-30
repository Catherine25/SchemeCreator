using SchemeCreator.Data.Services;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data.Extensions
{
    public static class EllipseExtension
    {
        public static Size GetSize(this Ellipse ellipse) => new(ellipse.Width, ellipse.Height);

        public static void SetSize(this Ellipse ellipse, Size size)
        {
            ellipse.Width = size.Width;
            ellipse.Height = size.Height;
        }

        public static void Activate(this Ellipse ellipse) => ellipse.Stroke = Colorer.ActivatedColor;
        public static void Deactivate(this Ellipse ellipse) => ellipse.Stroke = Colorer.DeactivatedColor;
        

        public static void SetStandartAlingment(this Ellipse ellipse)
        {
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.VerticalAlignment = VerticalAlignment.Top;
        }
    }
}
