using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data.ConstantsNamespace;

namespace SchemeCreator.Data.Models
{
    [DataContract] public class Wire {
    [DataMember] public Point start, end;
    [DataMember] public bool? isActive;

    public Line CreateLine() {

        Line l = new Line {
            X1 = start.X,
            X2 = end.X,
            Y1 = start.Y,
            Y2 = end.Y,
            StrokeThickness =
                Constants.wireThickness,
            StrokeEndLineCap = PenLineCap.Round,
            StrokeStartLineCap = PenLineCap.Round,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };

            if (isActive == true)
            {
                l.Stroke =
                    Constants.brushes[Constants.AccentEnum.light1];
                l.Fill = Constants.brushes[Constants.AccentEnum.dark1];
            }
            else if (isActive == false)
            {
                l.Stroke =
                Constants.brushes[Constants.AccentEnum.dark1];
                l.Fill = Constants.brushes[Constants.AccentEnum.light1];
            }
            else
            {
                l.Stroke = Constants.brushes[Constants.AccentEnum.accent2];
                l.Fill = Constants.brushes[Constants.AccentEnum.accent2];
            }
        
        return l;
        }

        public Point Center => new Point
        {
            X = (start.X + end.X) / 2,
            Y = (start.Y + end.Y) / 2
        };
    }
}
