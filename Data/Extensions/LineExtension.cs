using System.Numerics;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data.Extensions
{
    public static class LineExtension
    {
        public static Vector3 GetStartPoint(this Line line) =>
            new Vector3((float)line.X1, (float)line.Y1, 0);
        public static Vector3 GetEndPoint(this Line line) =>
            new Vector3((float)line.X2, (float)line.Y2, 0);
    }
}
