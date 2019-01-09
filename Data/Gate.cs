using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data
{
    //class for UI
    public class Gate
    {
        //this element data
        public Rectangle body;
        public TextBlock title;
        public bool isReserved = false;
        public int id;
        //inputs data
        public Ellipse[] inputEllipse;
        public bool[] inputReserved;
        //outputs data
        public bool? outputValue;
        public Ellipse outputEllipse;

        //default constructor
        public Gate() { }

        // Specified constructor for IN and OUT
        public Gate(Point p, int _id, bool _isReserved)
        {
            //creating thickness
            Thickness t = new Thickness()
            {
                Left = p.X -  Scheme.offset,
                Top = p.Y - Scheme.offset
            };

            //initializing gate body
            body = new Rectangle()
            {
                Width = Scheme.dotSize * 2,
                Height = Scheme.dotSize * 2,
                Fill = Scheme.darkBrush,
                Margin = t,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            //initializing gate name
            title = new TextBlock()
            {
                Width = Scheme.dotSize * 2,
                Height = Scheme.dotSize * 2,
                Text = Scheme.gateNames[_id],
                FontSize = 10,
                Margin = t,
                TextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            outputValue = false;
            id = _id;
            isReserved = _isReserved;
        }

        // Specified constructor for elements excluding IN and OUT
        public Gate(Point p, int newGateInputs, int _id, bool[] isInputsReserved)
        {
            //setting initial margin
            Thickness t = new Thickness
            {
                Top = p.Y - (GateController.gateHeight / 2),
                Left = p.X - (GateController.gateWidth / 2)
            };

            //gate name
            title = new TextBlock()
            {
                Text = Scheme.gateNames[_id],
                TextAlignment = TextAlignment.Center,
                Height = GateController.gateHeight,
                Width = GateController.gateWidth,
                Margin = t,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            //body of gate
            body = new Rectangle()
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
                t.Top += body.Height / (inputEllipse.Length + 1);

                if (i == 0)
                    t.Top -= Scheme.lineStartOffset;

                t.Left = body.Margin.Left - Scheme.lineStartOffset;

                //creating inputs array
                inputEllipse[i] = new Ellipse()
                {
                    Height = Scheme.dotSize,
                    Width = Scheme.dotSize,
                    Fill = Scheme.lightBrush,
                    Margin = t,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                if(isInputsReserved == null)
                    inputReserved[i] = false;
                else
                    inputReserved[i] = isInputsReserved[i];
            }

            //output
            //setting margin
            t.Left += body.Width;
            t.Top = body.Margin.Top + (GateController.gateHeight / 2) - Scheme.lineStartOffset;

            outputEllipse = new Ellipse()
            {
                Height = Scheme.offset,
                Width = Scheme.offset,
                Fill = Scheme.lightBrush,
                Margin = t,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            id = _id;
        }

        //methods
        public void Work(bool? value)
        {
            if (outputValue == null)
            {
                if (id == (int)Scheme.GateId.NOT)
                {
                    outputValue = !value;
                    return;
                } 

                outputValue = value;
                return;
            }

            switch (id)
            {
                case 2: //Buffer
                    outputValue = value;
                    break;
                case 3: //NOT
                    outputValue = !value;
                    break;
                case 4: //AND
                    outputValue &= value;
                    break;
                case 5: //NAND
                    outputValue &= !value;
                    break;
                case 6: //OR
                    outputValue |= value;
                    break;
                case 7: //NOR
                    outputValue |= !value;
                    break;
                case 8: //XOR
                    outputValue ^= value;
                    break;
                case 9: //XNOR
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
                body.Fill = Scheme.lightBrush;
            else if (outputValue == false)
                body.Fill = Scheme.darkBrush;
        }
    }
}