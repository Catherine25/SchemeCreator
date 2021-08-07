using SchemeCreator.UI.Dynamic;
using Windows.Foundation;

namespace SchemeCreator.Data.Services.Serialization
{
    public class WireDto
    {
        public Point Start;
        public Point End;

        public WireDto() {}

        public WireDto(WireView view)
        {
            Start = view.Start;
            End = view.End;
        }
    }
}
