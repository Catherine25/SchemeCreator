using SchemeCreator.UI.Dynamic;
using System.Numerics;

namespace SchemeCreator.Test
{
    public class PortBuilder
    {
        public ExternalPortView Build(PortType type, Vector2 point) => new ExternalPortView(type, point);
    }
}
