using System.Collections.Generic;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Models
{
    [DataContract]
    public class Gate : IGridChild
    {
        /*      data        */
        [DataMember] public GateEnum type;
        [DataMember] public int inputs, outputs;
        [DataMember] public Point center;
        [DataMember] public List<bool?> values;

        public Gate(GateEnum type, int inputs, int outputs, Point point)
        {
            this.type = type;
            this.inputs = inputs;
            this.outputs = outputs;
            center = point;

            values = new List<bool?>(inputs);

            for (int i = 0; i < inputs; i++)
                values.Add(null);
        }

        public Point GetLeftTop()
        {
            if(external.Contains(type))
            {
                return new Point
                {
                    X = center.X - externalGateSize.Width / 2,
                    Y = center.Y - externalGateSize.Height / 2
                };
            }
            else
            {
                return new Point
                {
                    X = center.X - logicGateSize.Width / 2,
                    Y = center.Y - logicGateSize.Height / 2
                };
            }
            
        }

        public void Work()
        {
            int length = values.Count;

            switch (type)
            {
                case GateEnum.NOT:
                    values[0] = !values[0];
                    break;

                case GateEnum.AND:
                    for (int i = 0; i < length; i++)
                        values[0] &= values[i];
                    break;

                case GateEnum.NAND:
                    {
                        for (int i = 0; i < length; i++)
                            values[0] &= values[i];

                        values[0] = !values[0];
                    }
                    break;

                case GateEnum.OR:
                    {
                        for (int i = 0; i < length; i++)
                            values[0] |= values[i];
                    }
                    break;

                case GateEnum.NOR:
                    {
                        for (int i = 0; i < length; i++)
                            values[0] |= values[i];

                        values[0] = !values[0];
                    }
                    break;

                case GateEnum.XOR:
                    {
                        for (int i = 0; i < length; i++)
                            values[0] ^= values[i];
                    }
                    break;

                case GateEnum.XNOR:
                    {
                        for (int i = 0; i < length; i++)
                            values[0] ^= values[i];

                        values[0] = !values[0];
                    }
                    break;

                default: return;
            }
        }

        public Button DrawBody()
        {
            Button button = new Button()
            {
                FontSize = 10,
                Content = type.ToString()
            };

            button.SetStandartAlignment();

            if (Constants.external.Contains(type))
            {
                button.SetSizeAndCenter(Constants.externalGateSize, center);

                button.SetFillByValue(values[0]);
            }
            else
            {
                button.SetSizeAndCenter(Constants.logicGateSize, center);
                button.Background = Constants.brushes[Constants.AccentEnum.dark1];
                button.Foreground = Constants.brushes[Constants.AccentEnum.light1];

                if (!Constants.singleOutput.Contains(type))
                    button.Content += "\n" + inputs.ToString() + " in " + outputs.ToString();
            }

            return button;
        }

        public List<Port> DrawGateInPorts()
        {
            List<Port> ports = new List<Port>();
            int length = inputs;

            for (int i = 0; i < length; i++)
            {
                Port port = new Port(ConnectionType.input);

                Point center = new Point(GetLeftTop().X, GetLeftTop().Y + ((logicGateSize.Height / (length + 1)) * (i + 1)));

                port.CenterPoint = center;

                port.Size = gatePortSize;

                port.BooleanValue = values[i];

                ports.Add(port);
            }

            return ports;
        }

        public List<Port> DrawGateOutPorts()
        {
            List<Port> ports = new List<Port>();
            int length = outputs;

            for (int i = 0; i < length; i++)
            {
                Port port = new Port(ConnectionType.output);

                Point center = new Point(GetLeftTop().X + logicGateSize.Width, GetLeftTop().Y + ((logicGateSize.Height / (length + 1)) * (i + 1)));

                port.CenterPoint = center;

                port.Size = gatePortSize;

                port.BooleanValue = values[i];

                ports.Add(port);
            }

            return ports;
        }

        public bool ContainsInOutByCenter(Point center, ConnectionType type)
        {
            if(type == ConnectionType.input)
                return DrawGateInPorts().Exists(x => x.CenterPoint == center);
            else
                return DrawGateOutPorts().Exists(x => x.CenterPoint == center);
        }

        public bool ContainsBodyByMargin(Thickness t) =>
            DrawBody().Margin == t;

        public int GetIndexOfInOutByCenter(Point center, ConnectionType type) {
            if (type == ConnectionType.input)
                return DrawGateInPorts().FindIndex(x => x.CenterPoint == center);
            else
                return DrawGateOutPorts().FindIndex(x => x.CenterPoint == center);
        }
        
        public Button GetBodyByWirePart(Point p)
        {
            if (type == GateEnum.IN || type == GateEnum.OUT)
                if (p == center)
                    return DrawBody();

            return null;
        }

        public Port GetInOutByWirePart(Point p)
        {
            List<Port> inputs = DrawGateInPorts();
            List<Port> outputs = DrawGateOutPorts();

            for (int i = 0; i < inputs.Count; i++)
            {
                Point point = inputs[i].CenterPoint;

                if (point == p)
                    return inputs[i];
            }

            for (int i = 0; i < outputs.Count; i++)
            {
                Point point = outputs[i].CenterPoint;

                if (point == p)
                    return outputs[i];
            }
               
            return null;
        }

        public bool WireConnects(Point point)
        {
            if (external.Contains(type))
                return center == point;
            else
            {
                var inputs = DrawGateInPorts();

                for (int i = 0; i < inputs.Count; i++)
                    if (point == inputs[i].CenterPoint)
                        return true;

                var outputs = DrawGateOutPorts();

                for (int i = 0; i < outputs.Count; i++)
                    if (point == outputs[i].CenterPoint)
                        return true;
            }

            return false;
        }

        public int FirstFreeValueBoxIndex()
        {
            for (int i = 0; i < inputs; i++)
                if (values[i] == null)
                    return i;

            return -1;
        }

        public void AddToParent(Grid parent)
        {
            var body = DrawBody();
            parent.Children.Add(body);
        }
    }
}