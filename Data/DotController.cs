using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data
{
    public static class DotController
    {
        public static List<Ellipse> dots;
        private static SolidColorBrush brush;
        private static int netSize;
        private static int dotSize;
        static DotController()
        {
            netSize = Scheme.netSize;
            dotSize = Scheme.dotSize;
            brush = Scheme.darkBrush;
            dots = new List<Ellipse>();
        }
        public static void InitNet(double actW, double actH)
        {
            double stepW = actW / (netSize + 2);
            double stepH = actH / (netSize + 2);
            Thickness margin;

            for (int i = 0; i < netSize; i++)
                for (int j = 0; j < netSize; j++)
                {
                    margin.Left = stepW * i + stepW;
                    margin.Top = stepH * j + stepH;

                    Ellipse ellipse = new Ellipse
                    {
                        Margin = margin,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Height = dotSize,
                        Width = dotSize,
                        Fill = brush
                    };
                    dots.Add(ellipse);
                }
        }
    }
}
