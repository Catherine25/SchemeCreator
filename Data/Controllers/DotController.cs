using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data.ConstantsNamespace;
using SchemeCreator.Data.Extensions;
using Windows.Foundation;

namespace SchemeCreator.Data
{
    public class DotController
    {
        private IList<Ellipse> dots;
        public Ellipse lastTapped;

        public IList<Ellipse> Dots
        {
            get => dots;
            set => dots = value;
        }

        public DotController() => dots = new List<Ellipse>();

        public void InitNet(Size actSize)
        {
            double stepW = actSize.Width / (Constants.netSize + 1),
                stepH = actSize.Height / (Constants.netSize + 1);

            Thickness margin;

            for (int i = 1; i <= Constants.netSize; i++)
                for (int j = 1; j <= Constants.netSize; j++)
                {
                    margin.Left = stepW * i;
                    margin.Top = stepH * j;

                    Ellipse ellipse = new Ellipse
                    {
                        Margin = margin,
                        Fill = Constants.brushes[Constants.AccentEnum.dark1]
                    };

                    ellipse.SetStandartAlingment();
                    ellipse.SetSize(Constants.dotSize);

                    Dots.Add(ellipse);
                }
        }
    }
}