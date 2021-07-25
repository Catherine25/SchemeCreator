using SchemeCreator.UI.Dynamic;
using System.Numerics;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Services.Serialization
{
    public class GateDto
    {
        public Vector2 Location;
        public GateEnum Type;
        public int Inputs;
        public int Outputs;

        public GateDto() { }
        public GateDto(GateView view)
        {
            Location = view.MatrixLocation;
            Type = view.Type;
            Inputs = view.InputCount;
            Outputs = view.OutputCount;
        }
    }
}