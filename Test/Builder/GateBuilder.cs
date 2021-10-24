using SchemeCreator.UI.Dynamic;
using System.Numerics;

namespace SchemeCreator.Test.Builder
{
    public class GateBuilder
    {
        public GateView Build(GateEnum type, Vector2 point, int inputs = 1, int outputs = 1) => new(type, point, inputs, outputs); 
    }
}
