using SchemeCreator.Data.Models;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Services.Serialization
{
    public class WireDto
    {
        public WireConnection Connection;

        public WireDto() {}

        public WireDto(WireView view) => Connection = view.Connection;
    }
}
