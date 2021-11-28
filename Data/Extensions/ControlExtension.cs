using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Extensions
{
    public static class ControlExtension
    {
        /// <summary>
        /// Gets element center relative to its parent element.
        /// </summary>
        public static Point GetCenterRelativeTo(this UserControl childElement, UIElement parentElement)
        {
            var point = GetLeftTopRelativeTo(childElement, parentElement);

            point.X += childElement.ActualWidth / 2;
            point.Y += childElement.ActualHeight / 2;

            return point;
        }
        
        private static Point GetLeftTopRelativeTo(this UIElement childElement, UIElement parentElement) =>
            childElement.TransformToVisual(parentElement).TransformPoint(new Point(0, 0));
        
        public static bool IsInitialized(this Point point) => point.X != 0 && point.Y != 0;

        public static void SetSize(this UserControl control, Size size)
        {
            control.Width = size.Width;
            control.Height = size.Height;
        }
        
        public static void SetLeftTopAlignment(this FrameworkElement control)
        {
            control.HorizontalAlignment = HorizontalAlignment.Left;
            control.VerticalAlignment = VerticalAlignment.Top;
        }
        
        public static void SetCenterAlignment(this FrameworkElement control)
        {
            control.HorizontalAlignment = HorizontalAlignment.Center;
            control.VerticalAlignment = VerticalAlignment.Center;
        }
        
        public static void MakeCellIndependent(this FrameworkElement control, Size gridSize)
        {
            Grid.SetColumnSpan(control, (int)gridSize.Width);
            Grid.SetRowSpan(control, (int)gridSize.Height);
        }
    }
}
