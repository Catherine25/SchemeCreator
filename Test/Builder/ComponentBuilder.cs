using System;
using System.Collections.Generic;
using System.Numerics;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Test.Builder
{
    public class ComponentBuilder
    {
        private readonly SchemeView scheme;
        private readonly PortBuilder portBuilder;
        private readonly GateBuilder gateBuilder;
        private HashSet<Vector2> occupiedPlaces;
        private Random random;

        public ComponentBuilder(SchemeView scheme)
        {
            this.scheme = scheme;

            this.portBuilder = new PortBuilder();
            this.gateBuilder = new GateBuilder();

            occupiedPlaces = new HashSet<Vector2>();
            random = new Random();
        }

        public ExternalPortView BuildInput()
        {
            var input = portBuilder.Build(PortType.Input, GetPlace());
            scheme.AddToView(input);
            return input;
        }

        public ExternalPortView BuildOutput()
        {
            var output = portBuilder.Build(PortType.Output, GetPlace());
            scheme.AddToView(output);
            return output;
        }

        public GateView BuildGate(GateEnum type = GateEnum.Buffer, int inputs = 1, int outputs = 1)
        {
            var gate = gateBuilder.Build(type, GetPlace(), inputs, outputs);
            scheme.AddToView(gate);
            return gate;
        }

        private Vector2 GetPlace()
        {
            Vector2 place = GeneratePlace();

            while (occupiedPlaces.Contains(place))
                place = GeneratePlace();

            return place;
        }

        private Vector2 GeneratePlace()
        {
            var x = random.Next(NetSize);
            var y = random.Next(NetSize);
            return new Vector2(x, y);
        }
    }
}
