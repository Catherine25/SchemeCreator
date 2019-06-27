using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data {
    [DataContract] public class Wire {
    [DataMember] public Point start, end;
    [DataMember] public bool isActive;

    public Line CreateLine(bool isActive) {

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
        
        if(isActive) {
            l.Stroke =
                Constants.brushes[Constants.AccentEnum.light1];
            l.Fill = Constants.brushes[Constants.AccentEnum.dark1];
        }
        else {
            l.Stroke =
            Constants.brushes[Constants.AccentEnum.dark1];
            l.Fill = Constants.brushes[Constants.AccentEnum.light1];
        }
        
        return l;
        }
    }
}
