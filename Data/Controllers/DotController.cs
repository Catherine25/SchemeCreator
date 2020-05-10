using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data.Extensions;
using Windows.Foundation;
using static SchemeCreator.Data.Constants;
using Windows.UI.Xaml.Media;
using SchemeCreator.Data.Services;

namespace SchemeCreator.Data.Controllers
{
    public class DotController
    {
        private List<Ellipse> dots;

        public List<Ellipse> Dots
        {
            get => dots;
            set => dots = value;
        }

        public DotController() => dots = new List<Ellipse>();

        public void InitNet(Size actSize)
        {
            double stepW = actSize.Width / (netSize + 1),
                stepH = actSize.Height / (netSize + 1);

            for (int i = 1; i <= netSize; i++)
                for (int j = 1; j <= netSize; j++)
                {
                    Ellipse ellipse = new Ellipse();

                    Colorer.SetFillByValue(ellipse, false);

                    ellipse.SetSizeAndCenterPoint(dotSize, new Point(stepW * i, stepH * j));

                    ellipse.SetStandartAlingment();
                 
                    Dots.Add(ellipse);
                }
        }
    }
}