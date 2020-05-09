using System.Collections.Generic;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data.ConstantsNamespace;
using SchemeCreator.Data.Extensions;

namespace SchemeCreator.Data.Model
{
    [DataContract]
    public class Gate
    {
        /*      data        */
        [DataMember] public Constants.GateEnum type;
        [DataMember] public int inputs, outputs;
        [DataMember] public Point center;
        [DataMember] public List<bool?> values;

        public Gate(Constants.GateEnum type, int inputs, int outputs, Point point)
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
            if(Constants.external.Contains(type))
            {
                return new Point
                {
                    X = center.X - Constants.externalGateSize.Width / 2,
                    Y = center.Y - Constants.externalGateSize.Height / 2
                };
            }
            else
            {
                return new Point
                {
                    X = center.X - Constants.logicGateSize.Width / 2,
                    Y = center.Y - Constants.logicGateSize.Height / 2
                };
            }
            
        }

        public void Work()
        {
            int length = values.Count;

            switch (type)
            {
                case Constants.GateEnum.NOT:
                    values[0] = !values[0];
                    break;

                case Constants.GateEnum.AND:
                    for (int i = 0; i < length; i++)
                        values[0] &= values[i];
                    break;

                case Constants.GateEnum.NAND:
                    {
                        for (int i = 0; i < length; i++)
                            values[0] &= values[i];

                        values[0] = !values[0];
                    }
                    break;

                case Constants.GateEnum.OR:
                    {
                        for (int i = 0; i < length; i++)
                            values[0] |= values[i];
                    }
                    break;

                case Constants.GateEnum.NOR:
                    {
                        for (int i = 0; i < length; i++)
                            values[0] |= values[i];

                        values[0] = !values[0];
                    }
                    break;

                case Constants.GateEnum.XOR:
                    {
                        for (int i = 0; i < length; i++)
                            values[0] ^= values[i];
                    }
                    break;

                case Constants.GateEnum.XNOR:
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
                Port port = new Port(Constants.ConnectionType.input);

                Point center = new Point(GetLeftTop().X, GetLeftTop().Y + ((Constants.logicGateSize.Height / (length + 1)) * (i + 1)));

                port.CenterPoint = center;

                port.Size = Constants.gatePortSize;

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
                Port port = new Port(Constants.ConnectionType.output);

                Point center = new Point(GetLeftTop().X + Constants.logicGateSize.Width, GetLeftTop().Y + ((Constants.logicGateSize.Height / (length + 1)) * (i + 1)));

                port.CenterPoint = center;

                port.Size = Constants.gatePortSize;

                port.BooleanValue = values[i];

                ports.Add(port);
            }

            return ports;
        }

        public bool ContainsInOutByCenter(Point center, Constants.ConnectionType type)
        {
            if(type == Constants.ConnectionType.input)
                return DrawGateInPorts().Exists(x => x.CenterPoint == center);
            else
                return DrawGateOutPorts().Exists(x => x.CenterPoint == center);
        }

        public bool ContainsBodyByMargin(Thickness t) =>
            DrawBody().Margin == t;

        public int GetIndexOfInOutByCenter(Point center, Constants.ConnectionType type) {
            if (type == Constants.ConnectionType.input)
                return DrawGateInPorts().FindIndex(x => x.GetCenterPoint() == center);
            else
                return DrawGateOutPorts().FindIndex(x => x.GetCenterPoint() == center);
        }
        
        public Button GetBodyByWirePart(Point p)
        {
            if (type == Constants.GateEnum.IN || type == Constants.GateEnum.OUT)
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
            System.Diagnostics.Debug.Write("\nRunning WireConnects()... ");

            if (Constants.external.Contains(type))
            {
                if (center == point)
                {
                    System.Diagnostics.Debug.Write("Result: true");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.Write("Result: false");
                    return false;
                }
            }
            else
            {
                var inputs = DrawGateInPorts();

                for (int i = 0; i < inputs.Count; i++)
                {
                    if (point == inputs[i].GetCenterPoint())
                    {
                        System.Diagnostics.Debug.Write("Result: true");
                        return true;
                    }
                }

                var outputs = DrawGateOutPorts();

                for (int i = 0; i < outputs.Count; i++)
                {
                    if (point == outputs[i].GetCenterPoint())
                    {
                        System.Diagnostics.Debug.Write("Result: true");
                        return true;
                    }
                }
            }

            System.Diagnostics.Debug.Write("Result: false");

            return false;
        }

        public int FirstFreeValueBoxIndex()
        {
            for (int i = 0; i < inputs; i++)
                if (values[i] == null)
                    return i;

            return -1;
        }
    }
}