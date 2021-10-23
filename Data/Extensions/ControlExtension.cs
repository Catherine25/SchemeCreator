using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Extensions
{
    public static class ControlExtension
    {
        public static Point GetLeftTopRelativeTo(this UIElement childElement, UIElement parentElement) =>
            childElement.TransformToVisual(parentElement).TransformPoint(new Point(0, 0));

        public static Point GetCenterRelativeTo(this UserControl childElement, UIElement parentElement)
        {
            Point point = GetLeftTopRelativeTo(childElement, parentElement);

            point.X += childElement.ActualWidth / 2;
            point.Y += childElement.ActualHeight / 2;

            return point;
        }

        public static Vector3 TransformToVector3(this Point point) =>
            new Vector3((float)point.X, (float)point.Y, 1);

        public static Point TransformToPoint(this Vector3 vector3) =>
            new Point(vector3.X, vector3.Y);

        public static bool IsInited(this Point point) =>
            point.X != 0 && point.Y != 0;

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
        
        public static void MakeCellIndependent(this FrameworkElement control, Size gridSize)
        {
            Grid.SetColumnSpan(control, (int)gridSize.Width);
            Grid.SetRowSpan(control, (int)gridSize.Height);
        }
    }
}
