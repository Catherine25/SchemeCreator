using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data
{
    static class Scheme
    {
        //constants
        public const int netSize = 8;
        public const int dotSize = 10;
        public const double offset = 10.0;
        public const double lineStartOffset = 5.0;

        //flags
        public static bool SchemeCreated;
        public static bool AddLineStartMode = false;
        public static bool AddLineEndMode = false;
        public static bool AddGateMode = false;
        public static bool ChangeValueMode = false;
        public static bool Subscribed = false;

        //data
        public static SolidColorBrush lightBrush, darkBrush;
        public static Point[] userPoints;
        public static int NewGateInputs;
        public static string NewElementName;

        //public enum GateId
        //{
        //    IN, OUT,
        //    Buffer, NOT,
        //    AND, NAND,
        //    OR, NOR,
        //    XOR, XNOR
        //};

        //constructors
        static Scheme()
        {
            lightBrush = new SolidColorBrush(Windows.UI.Colors.Cyan);
            darkBrush = new SolidColorBrush(Windows.UI.Colors.DarkCyan);
            userPoints = new Point[3];
            //GateId gateId = new GateId();
        }

        public static bool? GetValue(Line l)
        {
            foreach (Gate g in GateController.gates)
                if (g.gateName.Text == "IN")
                {
                    if (l.X1 == g.gateName.Margin.Left + dotSize)
                        if (l.Y1 == g.gateName.Margin.Top + dotSize)
                        {
                            LineController.ColorLine(l, g.outputValue);
                            return g.outputValue;
                        }
                }
                else if (g.gateName.Text != "OUT")
                {
                    if (l.X1 == g.outputEllipse.Margin.Left + lineStartOffset)
                        if (l.Y1 == g.outputEllipse.Margin.Top + lineStartOffset)
                        {
                            LineController.ColorLine(l, g.outputValue);
                            return g.outputValue;
                        }
                }
            return null;
        }

        public static void SendValue(bool? value, Line l)
        {
            foreach (Gate g in GateController.gates)
            {
                if(g.gateName.Text == "OUT")
                {
                    if (l.X2 == g.gateName.Margin.Left + dotSize)
                        if (l.Y2 == g.gateName.Margin.Top + dotSize)
                        {
                            g.outputValue = value;
                            g.ColorByValue();
                        }
                            
                }
                else if (g.gateName.Text != "IN")
                {
                    foreach (Ellipse e in g.inputEllipse)
                    {
                        if (l.X2 == e.Margin.Left + lineStartOffset)
                            if (l.Y2 == e.Margin.Top + lineStartOffset)
                            {
                                g.Work(value);
                                g.ColorByValue();
                                break;
                            }
                    }
                }
            }
        }

        public static bool LineConnects(LineInfo li, Gate gate)
        {
            if (gate.gateName.Text == "IN" || gate.gateName.Text == "OUT")
            {
                if (gate.gateName.Margin.Left + (lineStartOffset * 2) == li.point1.X)
                    if (gate.gateName.Margin.Top + (lineStartOffset * 2) == li.point1.Y)
                        return true;
            }
            else
            {
                if (gate.outputEllipse.Margin.Left + lineStartOffset == li.point1.X)
                    if (gate.outputEllipse.Margin.Top + lineStartOffset == li.point1.Y)
                        return true;

                return LineConnectsToGateIn(li, gate);
            }
            return false;
        }
        public static bool LineConnectsToGateIn(LineInfo li, Gate gate)
        {
            foreach (Ellipse ellipse in gate.inputEllipse)
                if (ellipse.Margin.Left + lineStartOffset == li.point1.X)
                    if (ellipse.Margin.Top + lineStartOffset == li.point1.Y)
                        return true;
            return false;
        }
    }
}