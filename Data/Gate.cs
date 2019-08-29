using System.Collections.Generic;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data.ConstantsNamespace;

namespace SchemeCreator.Data
{
    [DataContract]
    public class Gate
    {
        /*      data        */
        [DataMember] public Constants.GateEnum type;
        [DataMember] public bool isExternal;
        [DataMember] public int inputs, outputs;
        [DataMember] public double x, y;

        [DataMember] public List<bool?> values;

        public Gate(Constants.GateEnum type,
            int inputs,
            int outputs,
            double x,
            double y)
        {
            this.type = type;
            isExternal = Constants.external.Contains(type);
            this.inputs = inputs;
            this.outputs = outputs;
            this.x = x;
            this.y = y;
            values = new List<bool?>(inputs);

            for (int i = 0; i < inputs; i++)
                values.Add(null);
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
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,

                Background = Constants.brushes[Constants.AccentEnum.dark1],
                Foreground = Constants.brushes[Constants.AccentEnum.light1],

                Margin = new Thickness(x, y, 0, 0),
                FontSize = 10,
                Content = type.ToString()
            };

            if (isExternal)
            {
                button.Height = Constants.externalGateHeight;
                button.Width = Constants.externalGateWidth;
                button.Margin = new Thickness(
                    button.Margin.Left - Constants.externalGateWidth / 2 - Constants.dotSize / 2,
                    button.Margin.Top - Constants.externalGateHeight / 2 - Constants.dotSize / 2,
                    0,
                    0);

                if (values[0] == true)
                {
                    button.Foreground = Constants.brushes[Constants.AccentEnum.dark1];
                    button.Background = Constants.brushes[Constants.AccentEnum.light1];
                }
                else if (values[0] == false)
                {
                    button.Foreground = Constants.brushes[Constants.AccentEnum.light1];
                    button.Background = Constants.brushes[Constants.AccentEnum.dark1];
                }
                else
                {
                    button.Foreground = Constants.brushes[Constants.AccentEnum.background];
                    button.Background = Constants.brushes[Constants.AccentEnum.accent2];
                }
            }
            else
            {
                button.Width = Constants.gateWidth;
                button.Height = Constants.gateHeight;

                if (!Constants.singleOutput.Contains(type))
                    button.Content += "\n" + inputs.ToString() + " in " + outputs.ToString();
            }

            button.Name = button.Margin.ToString();

            return button;
        }

        public List<Ellipse> DrawGateInOut(bool isInput)
        {
            Thickness t = new Thickness
            {
                Left = x - Constants.lineStartOffset,
                Top = y
            };

            int newInOutCount;

            if (isInput)
                newInOutCount = inputs;
            else
            {
                newInOutCount = outputs;
                t.Left += Constants.gateWidth;
            }

            List<Ellipse> ellipses = new List<Ellipse>();

            for (int i = 0; i < newInOutCount; i++)
            {
                t.Top += Constants.gateHeight / (newInOutCount + 1);

                if (i == 0)
                    t.Top -= Constants.lineStartOffset;

                ellipses.Add(new Ellipse()
                {
                    Height = Constants.dotSize,
                    Width = Constants.dotSize,
                    Fill = Constants.brushes[Constants.AccentEnum.light1],
                    Margin = t,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                });
            }

            return ellipses;
        }

        public bool ContainsInOutByMargin(Ellipse e, bool isInput) =>
            DrawGateInOut(isInput).Exists(x => x.Margin == e.Margin);

        public bool ContainsBodyByMargin(Thickness t) =>
            DrawBody().Margin == t;

        public int GetIndexOfInOutByMargin(Thickness t, bool isInput) =>
            DrawGateInOut(isInput).FindIndex(x => x.Margin == t);

        public Button GetBodyByWirePart(Point p)
        {
            if (type == Constants.GateEnum.IN)
            {
                Point point = new Point
                {
                    X = x - Constants.lineStartOffset,
                    Y = y - Constants.lineStartOffset
                };

                if (p == point)
                    return DrawBody();
            }
            else if (type == Constants.GateEnum.OUT)
            {
                Point point = new Point
                {
                    X = x - Constants.lineStartOffset,
                    Y = y - Constants.lineStartOffset
                };

                if (p == point)
                    return DrawBody();
            }

            return null;
        }

        public Ellipse GetInOutByWirePart(Point p)
        {
            List<Ellipse> inputs = DrawGateInOut(true);
            List<Ellipse> outputs = DrawGateInOut(false);

            for (int i = 0; i < inputs.Count; i++)
            {
                Point point = new Point
                {
                    X = inputs[i].Margin.Left + Constants.lineStartOffset,
                    Y = inputs[i].Margin.Top + Constants.lineStartOffset
                };

                if (point == p)
                    return inputs[i];
            }

            for (int i = 0; i < outputs.Count; i++)
            {
                Point point = new Point
                {
                   X = outputs[i].Margin.Left + Constants.lineStartOffset,
                   Y = outputs[i].Margin.Top + Constants.lineStartOffset
                };

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
                Button body = DrawBody();

                double left = body.Margin.Left;
                double top = body.Margin.Top;

                Point gateCenter;

                gateCenter.X = left + (Constants.externalGateWidth / 2);
                gateCenter.Y = top + (Constants.externalGateHeight / 2);

                if (gateCenter == point)
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
                Point p = new Point();

                var inputs = DrawGateInOut(true);

                for (int i = 0; i < inputs.Count; i++)
                {
                    p.X = inputs[i].Margin.Left + (Constants.gateInOutSize / 2);
                    p.Y = inputs[i].Margin.Top + (Constants.gateInOutSize / 2);

                    if (p == point)
                    {
                        System.Diagnostics.Debug.Write("Result: true");
                        return true;
                    }
                }

                var outputs = DrawGateInOut(false);

                for (int i = 0; i < outputs.Count; i++)
                {
                    p.X = outputs[i].Margin.Left + (Constants.gateInOutSize / 2);
                    p.Y = outputs[i].Margin.Top + (Constants.gateInOutSize / 2);

                    if (p == point)
                    {
                        System.Diagnostics.Debug.Write("Result: true");
                        return true;
                    }
                }
            }

            System.Diagnostics.Debug.Write("Result: false");

            return false;
        }

        public int firstFreeValueBoxIndex()
        {
            for (int i = 0; i < inputs; i++)
                if (values[i] == null)
                    return i;

            return -1;
        }
    }
}