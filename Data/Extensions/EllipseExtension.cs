using SchemeCreator.Data.Services;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data.Extensions
{
    public static class EllipseExtension
    {
        public static void Activate(this Ellipse ellipse) => ellipse.Stroke = Colorer.ActivatedColor;
        public static void Deactivate(this Ellipse ellipse) => ellipse.Stroke = Colorer.DeactivatedColor;
    }
}
