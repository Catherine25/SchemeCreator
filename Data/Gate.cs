using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data
{
    public class Gate
    {
        //this element data
        public Rectangle gateRect;
        public TextBlock gateName;
        //inputs data
        public Ellipse[] inputEllipse;
        public bool[] inputReserved;
        //outputs data
        public bool? outputValue;
        public Ellipse outputEllipse;

        //default constructor
        public Gate() { }

        /// <summary>
        /// Specified constructor for IN and OUT
        /// </summary>
        public Gate(Point p, string name)
        {
            //creating thickness
            Thickness t = new Thickness()
            {
                Left = p.X -  Scheme.offset,
                Top = p.Y - Scheme.offset
            };

            //initializing gate body
            gateRect = new Rectangle()
            {
                Width = Scheme.dotSize * 2,
                Height = Scheme.dotSize * 2,
                Fill = Scheme.darkBrush,
                Margin = t,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            //initializing gate name
            gateName = new TextBlock()
            {
                Width = Scheme.dotSize * 2,
                Height = Scheme.dotSize * 2,
                Text = name,
                FontSize = 10,
                Margin = t,
                TextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            //setting flag to false
            outputValue = false;
        }

        /// <summary>
        /// Specified constructor for elements extcluding IN and OUT
        /// </summary>
        public Gate(Point p, string name, int newGateInputs)
        {
            //setting initial margin
            Thickness t = new Thickness
            {
                Top = p.Y - (GateController.gateHeight / 2),
                Left = p.X - (GateController.gateWidth / 2)
            };

            //gate name
            gateName = new TextBlock()
            {
                Text = name,
                TextAlignment = TextAlignment.Center,
                Height = GateController.gateHeight,
                Width = GateController.gateWidth,
                Margin = t,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            //body of gate
            gateRect = new Rectangle()
            {
                Height = GateController.gateHeight,
                Width = GateController.gateWidth,
                Fill = Scheme.darkBrush,
                Margin = t,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            inputEllipse = new Ellipse[newGateInputs];
            inputReserved = new bool[newGateInputs];

            //inputs
            for (int i = 0; i < inputEllipse.Length; i++)
            {
                //setting margin
                t.Top += gateRect.Height / (inputEllipse.Length + 1);

                if (i == 0)
                    t.Top -= Scheme.lineStartOffset;

                t.Left = gateRect.Margin.Left - Scheme.lineStartOffset;

                //creatinf inputs array
                inputEllipse[i] = new Ellipse()
                {
                    Height = Scheme.dotSize,
                    Width = Scheme.dotSize,
                    Fill = Scheme.lightBrush,
                    Margin = t,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                inputReserved[i] = false;
            }

            //output
            //setting margin
            t.Left += gateRect.Width;
            t.Top = gateRect.Margin.Top + (GateController.gateHeight / 2) - Scheme.lineStartOffset;

            outputEllipse = new Ellipse()
            {
                Height = Scheme.offset,
                Width = Scheme.offset,
                Fill = Scheme.lightBrush,
                Margin = t,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
        }

        //methods
        public void Work(bool? value)
        {
            if (outputValue == null)
            {
                outputValue = value;
                return;
            }
            switch (gateName.Text)
            {
                case "Buffer":
                    outputValue = value;
                    break;
                case "NOT":
                    outputValue = !value;
                    break;
                case "AND":
                    outputValue &= value;
                    break;
                case "NAND":
                    outputValue &= !value;
                    break;
                case "OR":
                    outputValue |= value;
                    break;
                case "NOR":
                    outputValue |= !value;
                    break;
                case "XOR":
                    outputValue ^= value;
                    break;
                case "XNOR":
                    outputValue ^= !value;
                    break;
                default:
                    break;
            }
            ColorByValue();
        }
        public void ColorByValue()
        {
            if (outputValue == true)
                gateRect.Fill = Scheme.lightBrush;
            else if (outputValue == false)
                gateRect.Fill = Scheme.darkBrush;
        }
    }
}