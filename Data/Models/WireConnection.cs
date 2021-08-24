using System.Numerics;
using Windows.Foundation;

namespace SchemeCreator.Data.Models
{
    public class WireConnection
    {
        public Vector2 MatrixStart { get; set; }
        public int? StartPort { get; set; }

        public Vector2 MatrixEnd { get; set; }
        public int? EndPort { get; set; }

        public Point StartPoint;
        public Point EndPoint;
    }
}
