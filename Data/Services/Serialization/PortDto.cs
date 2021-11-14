using System.Numerics;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Services.Serialization
{
    public class PortDto
    {
        public PortType Type;
        public Vector2 Location;
        public PortDto() { }

        public PortDto(ExternalPortView port)
        {
            Type = port.Type;
            Location = port.MatrixLocation;
        }
    }
}
