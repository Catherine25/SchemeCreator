using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data {
    [DataContract] public class Wire {
    [DataMember] public Point start, end;
    [DataMember] public bool isActive;

    public Line createWire(bool isActive) {

        Line l = new Line {
            X1 = start.X,
            X2 = end.Y,
            Y1 = start.X,
            Y2 = end.Y,
            StrokeThickness =
                SchemeCreator.Constants.lineStartOffset,
            StrokeEndLineCap = PenLineCap.Round,
            StrokeStartLineCap = PenLineCap.Round
        };
        
        if(isActive)
            l.Stroke =
                Constants.brushes[Constants.AccentEnum.light1];
        else l.Stroke =
            Constants.brushes[Constants.AccentEnum.dark1];
        
        return l;
        }
    }
}
