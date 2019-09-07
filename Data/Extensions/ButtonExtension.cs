using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Extensions
{
    public static class ButtonExtension
    {
        public static Point CenterPoint(this Button button) => new Point
        {
            X = button.Margin.Left + button.Width / 2,
            Y = button.Margin.Top + button.Height / 2
        };
    }
}
