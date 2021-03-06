using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Extensions
{
    public static class ButtonExtension
    {
        public static void SetSizeAndCenter(this Button button, Size size, Vector3 center)
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
    }
}
