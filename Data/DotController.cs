using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data {
    public class DotController {
        private double width, heigh;
        public IList<Ellipse> dots = new List<Ellipse>();

        public void InitNet(double actW, double actH) {
            width = actW;
            heigh = actH;

            double stepW = width / (Constants.netSize+1),
                stepH = heigh / (Constants.netSize+1);
            Thickness margin;

            for (int i = 1; i <= Constants.netSize; i++)
                for (int j = 1; j <= Constants.netSize; j++) {
                    margin.Left = stepW * i;
                    margin.Top = (stepH*0.9) * j;

                    Ellipse ellipse = new Ellipse {
                        Margin = margin,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Height = Constants.dotSize,
                        Width = Constants.dotSize,
                        Fill = Constants.brushes[Constants.AccentEnum.dark1]
                    };

                    dots.Add(ellipse);
                }
        }
    }
}
