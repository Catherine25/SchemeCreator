using SchemeCreator.Data.ConstantsNamespace;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Extensions
{
    public static class ButtonExtension
    {
        public static Point GetCenter(this Button button) => new Point
        {
            X = button.Margin.Left + button.Width / 2,
            Y = button.Margin.Top + button.Height / 2
        };

        public static void SetSizeAndCenter(this Button button, Size size, Point center)
        {
            button.SetSize(size);

            button.Margin = new Thickness
            {
                Left = center.X - size.Width / 2,
                Top = center.Y - size.Height / 2
            };
        }

        public static void SetStandartAlignment(this Button button)
        {
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.VerticalAlignment = VerticalAlignment.Top;
            button.HorizontalContentAlignment = HorizontalAlignment.Center;
            button.VerticalContentAlignment = VerticalAlignment.Center;
        }

        public static void SetSize(this Button button, Size size)
        {
            button.Width = size.Width;
            button.Height = size.Height;
        }

        public static void SetFillByValue(this Button button, bool? value)
        {
            if (value == true)
            {
                button.Foreground = Constants.brushes[Constants.AccentEnum.dark1];
                button.Background = Constants.brushes[Constants.AccentEnum.light1];
            }
            else if (value == false)
            {
                button.Foreground = Constants.brushes[Constants.AccentEnum.light1];
                button.Background = Constants.brushes[Constants.AccentEnum.dark1];
            }
            else
            {
                button.Foreground = Constants.brushes[Constants.AccentEnum.background];
                button.Background = Constants.brushes[Constants.AccentEnum.accent2];
            }
        }
    }
}
