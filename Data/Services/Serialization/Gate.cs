using SchemeCreator.UI.Dynamic;
using System.Numerics;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Services.Serialization
{
    static partial class Serializer
    {
        public struct Gate
        {
            public Vector2 Location;
            public GateEnum Type;
            public int Inputs;
            public int Outputs;

            public Gate(GateView view)
            {
                Location = view.MatrixLocation;
                Type = view.Type;
                Inputs = view.InputCount;
                Outputs = view.OutputCount;
            }
        }
    }
}