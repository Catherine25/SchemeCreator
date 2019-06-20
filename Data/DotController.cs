using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data {
    public class DotController {

        IList<Ellipse> dots;
        public Ellipse lastTapped;

        public IList<Ellipse> Dots {
            get => dots;
            set => dots = value;
        }

        public DotController() => dots = new List<Ellipse>();

        public void InitNet(double actW, double actH) {
            
            double width = actW;
            double heigh = actH;

            double stepW = width / (Constants.netSize+1),
                stepH = heigh / (Constants.netSize+1);

            Thickness margin;

            for (int i = 1; i <= Constants.netSize; i++)
                for (int j = 1; j <= Constants.netSize; j++) {
                    margin.Left = stepW * i;
                    margin.Top = stepH * j;

                    Ellipse ellipse = new Ellipse {
                        Margin = margin,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Height = Constants.dotSize,
                        Width = Constants.dotSize,
                        Fill = Constants.brushes[Constants.AccentEnum.dark1]
                    };

                    Dots.Add(ellipse);
            }
        }
    }
}
