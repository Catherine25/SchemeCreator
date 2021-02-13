using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Numerics;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Controllers
{
    public class DotController
    {
        //private List<(Ellipse, Vector2)> dots;

        //public List<(Ellipse, Vector2)> Dots
        //{
        //    get => dots;
        //    set => dots = value;
        //}

        //public DotController() => dots = new List<(Ellipse, Vector2)>();

        //public void InitNet()
        //{
        //    for (int i = 1; i <= netSize; i++)
        //        for (int j = 1; j <= netSize; j++)
        //        {
        //            Ellipse ellipse = new Ellipse
        //            {
        //                HorizontalAlignment = HorizontalAlignment.Center,
        //                VerticalAlignment = VerticalAlignment.Center
        //            };

        //            Grid.SetRow(ellipse, i - 1);
        //            Grid.SetColumn(ellipse, j - 1);

        //            Colorer.SetFillByValue(ellipse, false);

        //            ellipse.SetSize(dotSize);

        //            Dots.Add((ellipse, new Vector2(i - 1, j - 1)));
        //        }
        //}
    }
}