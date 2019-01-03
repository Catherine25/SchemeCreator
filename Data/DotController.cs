using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data
{
    public static class DotController
    {
        private static SolidColorBrush brush;
        private static int netSize, dotSize;
        private static double width, heigh;

        public static IList<Ellipse> dots;

        static DotController()
        {
            netSize = Scheme.netSize;
            dotSize = Scheme.dotSize;
            brush = Scheme.darkBrush;
            dots = new List<Ellipse>();
        }

        public static void InitNet(double actW, double actH)
        {
            if (width == 0 || heigh == 0)
            {
                width = actW;
                heigh = actH;
            }                
            double stepW = width / (netSize + 2);
            double stepH = heigh / (netSize + 2);
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
