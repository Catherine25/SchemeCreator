using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Extensions
{
    public static class GridExtension
    {
        public static void SetRect(this Grid grid, Rect rect)
        {
            grid.Margin = new Windows.UI.Xaml.Thickness
            {
                Left = rect.Left,
                Top = rect.Top
            };
            grid.Width = rect.Width;
            grid.Height = rect.Height;
        }
        public static Rect GetRect(this Grid grid) => new Rect
        {
            X = grid.Margin.Right,
            Y = grid.Margin.Top,
            Width = grid.Width,
            Height = grid.Height
        };

        public static Size GetActualSize(this Grid grid) => new Size(grid.ActualWidth, grid.ActualHeight);
        public static void SetStandartAlighnment(this Grid grid)
        {
            grid.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            grid.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
        }
        public static Point GetLeftTop(this Grid grid) =>
            new Point(grid.Margin.Left, grid.Margin.Top);

    }
}
