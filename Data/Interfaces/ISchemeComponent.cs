using System.Numerics;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Interfaces
{
    public interface ISchemeComponent
    {
        public Vector2 MatrixLocation { get; set; }
        public bool WireConnects(WireView wire);
    }
}
