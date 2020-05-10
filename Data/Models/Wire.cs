using SchemeCreator.Data.Services;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Models
{
    [DataContract]
    public class Wire
    {
        [DataMember]
        public Point start, end;
        [DataMember]
        public bool? isActive;

        public Line CreateLine()
        {

            Line l = new Line
            {
                X1 = start.X,
                X2 = end.X,
                Y1 = start.Y,
                Y2 = end.Y,
                StrokeThickness = wireThickness,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            Colorer.SetFillByValue(l, isActive);

            return l;
        }

        public Point Center => new Point
        {
            X = (start.X + end.X) / 2,
            Y = (start.Y + end.Y) / 2
        };
    }
}
