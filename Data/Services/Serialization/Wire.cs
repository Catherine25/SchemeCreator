using SchemeCreator.UI.Dynamic;
using Windows.Foundation;

namespace SchemeCreator.Data.Services.Serialization
{
    static partial class Serializer
    {
        public struct Wire
        {
            public Point Start;
            public Point End;

            public Wire(WireView view)
            {
                Start = view.Start;
                End = view.End;
            }
        }
    }
}