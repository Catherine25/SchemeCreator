using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Interfaces
{
    public interface ISchemeComponent : IGridComponent
    {
        public bool WireConnects(WireView wire);
    }
}
