//using SchemeCreator.Data.Interfaces;
//using SchemeCreator.Data.Services;
//using System.Numerics;
//using System.Runtime.Serialization;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Media;
//using Windows.UI.Xaml.Shapes;
//using static SchemeCreator.Data.Constants;

//namespace SchemeCreator.Data.Models
//{
//    [DataContract]
//    public class Wire : IIdHolder
//    {
//        [DataMember]
//        public Vector3 Start;

//        [DataMember]
//        public Vector3 End;

//        [DataMember]
//        public bool? isActive;

//        public Line CreateLine()
//        {
//            Line l = new Line
//            {
//                X1 = Start.X,
//                X2 = End.X,
//                Y1 = Start.Y,
//                Y2 = End.Y,
//                StrokeThickness = wireThickness,
//                StrokeEndLineCap = PenLineCap.Round,
//                StrokeStartLineCap = PenLineCap.Round,
//                HorizontalAlignment = HorizontalAlignment.Left,
//                VerticalAlignment = VerticalAlignment.Top
//            };

//            Colorer.SetFillByValue(l, isActive);

//            return l;
//        }

//        public Vector3 Center => new Vector3
//        {
//            X = (Start.X + End.X) / 2,
//            Y = (Start.Y + End.Y) / 2,
//            Z = 0
//        };

//        public int Id => GetHashCode();
//    }
//}
