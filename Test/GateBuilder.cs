using SchemeCreator.Data;
using SchemeCreator.UI.Dynamic;
using System.Numerics;

namespace SchemeCreator.Test
{
    public class GateBuilder
    {
        public GateView Build(Constants.GateEnum type, Vector2 point, int inputs = 1, int outputs = 1) => new GateView(type, point, inputs, outputs); 
    }
}
