using System;
using System.Collections.Generic;
using System.Numerics;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Test.Builder
{
    public class ComponentBuilder
    {
        private readonly SchemeView scheme;
        private readonly PortBuilder portBuilder;
        private readonly GateBuilder gateBuilder;
        private readonly HashSet<Vector2> occupiedPlaces;
        private readonly Random random;

        public ComponentBuilder(SchemeView scheme)
        {
            this.scheme = scheme;

            portBuilder = new PortBuilder();
            gateBuilder = new GateBuilder();

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
            var place = GeneratePlace();

            while (occupiedPlaces.Contains(place))
                place = GeneratePlace();

            occupiedPlaces.Add(place);

            return place;
        }

        private Vector2 GeneratePlace()
        {
            var x = random.Next((int)SchemeView.GridSize.Width);
            var y = random.Next((int)SchemeView.GridSize.Height);
            return new Vector2(x, y);
        }
    }
}
