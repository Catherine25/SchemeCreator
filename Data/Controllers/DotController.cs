using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data.ConstantsNamespace;
using SchemeCreator.Data.Extensions;
using Windows.Foundation;

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
            double stepW = actSize.Width / (Constants.netSize + 1),
                stepH = actSize.Height / (Constants.netSize + 1);

            //Thickness margin;

            for (int i = 1; i <= Constants.netSize; i++)
                for (int j = 1; j <= Constants.netSize; j++)
                {
                    Ellipse ellipse = new Ellipse { 
                        Fill = Constants.brushes[Constants.AccentEnum.dark1]
                    };

                    ellipse.SetSizeAndCenterPoint(Constants.dotSize, new Point(stepW * i, stepH * j));

                    ellipse.SetStandartAlingment();
                 
                    Dots.Add(ellipse);
                }
        }
    }
}