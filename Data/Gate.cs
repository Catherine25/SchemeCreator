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