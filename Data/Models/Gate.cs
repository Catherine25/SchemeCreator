using System.Collections.Generic;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Interfaces;
using static SchemeCreator.Data.Constants;
using System.Linq;
using System;
using SchemeCreator.Data.Models.Enums;

namespace SchemeCreator.Data.Models
{
    [DataContract]
    public class Gate : IGridChild
    {
        [DataMember]
        public GateEnum Type;

        [DataMember]
        public int Inputs;

        [DataMember]
        public int Outputs;

        [DataMember]
        public Point Center;

        [DataMember]
        public List<bool?> Values;

        public Gate(GateEnum type, int inputs, int outputs, Point point)
        {
            Type = type;
            Inputs = inputs;
            Outputs = outputs;
            Center = point;

            Values = new List<bool?>(inputs);

            for (int i = 0; i < inputs; i++)
                Values.Add(null);

            ProcessData = Services.GateWorkPatterns.ActionByType[type];
        }

        public Point GetLeftTop()
        {
            if(external.Contains(Type))
            {
                return new Point
                {
                    X = Center.X - externalGateSize.Width / 2,
                    Y = Center.Y - externalGateSize.Height / 2
                };
            }
            else
            {
                return new Point
                {
                    X = Center.X - logicGateSize.Width / 2,
                    Y = Center.Y - logicGateSize.Height / 2
                };
            }
            
        }

        public void Work() { ProcessData(this); }
        private readonly Action<Gate> ProcessData;

        public GateBody DrawBody() => new GateBody(this);

        public List<Port> DrawPorts(ConnectionType type)
        {
            if(type == ConnectionType.both)
            {
                var items = DrawPorts(ConnectionType.input);
                items.AddRange(DrawPorts(ConnectionType.output));
                return items;
            }
            else
            {
                List<Port> ports = new List<Port>();
                int length = type == ConnectionType.input ? Inputs : Outputs;

                for (int i = 0; i < length; i++)
                {
                    Port port = new Port(type);

                    Point center =
                        type == ConnectionType.input
                        ? new Point(GetLeftTop().X, GetLeftTop().Y + (logicGateSize.Height / (length + 1) * (i + 1)))
                        : new Point(GetLeftTop().X + logicGateSize.Width, GetLeftTop().Y + (logicGateSize.Height / (length + 1) * (i + 1)));

                    port.SetCenterAndSize(center);

                    port.BooleanValue = Values[i];

                    ports.Add(port);
                }

                return ports;
            }            
        }

        public bool ContainsInOutByCenter(Point center, ConnectionType type) =>
            DrawPorts(type).Exists(x => x.CenterPoint == center);

        public bool ContainsBodyByMargin(Thickness t) =>
            DrawBody().ContainsBodyByMargin(t);

        public int GetIndexOfInOutByCenter(Point center, ConnectionType type) =>
            DrawPorts(type).FindIndex(x => x.CenterPoint == center);

        public GateBody GetBodyByWirePart(Point p)
        {
            if (Type == GateEnum.IN || Type == GateEnum.OUT)
                if (p == Center)
                    return DrawBody();

            return null;
        }

        public Port GetInOutByWirePart(Point p)
        {
            var items = DrawPorts(ConnectionType.both);
            return items.FirstOrDefault(i => i.CenterPoint == p);
        }

        public bool WireConnects(Point point)
        {
            if (external.Contains(Type))
                return Center == point;
            else
            {
                var items = DrawPorts(ConnectionType.both);
                return items.Any(i => i.CenterPoint == point);
            }
        }

        public int FirstFreeValueBoxIndex()
        {
            for (int i = 0; i < Inputs; i++)
                if (Values[i] == null)
                    return i;

            return -1;
        }

        public void AddToParent(Grid parent) =>
            DrawBody().AddToParent(parent);
    }
}