using Windows.Foundation;
using Windows.UI.Xaml.Controls;
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
        public static bool schemeCreated = false;
        public static bool addLineStartMode = false;
        public static bool addLineEndMode = false;
        public static bool addGateMode = false;
        public static bool changeValueMode = false;
        public static bool subscribed = false;

        //data
        public static SolidColorBrush lightBrush, darkBrush;
        public static Point[] userPoints;

        public enum GateId
        {
            IN, OUT,
            Buffer, NOT,
            AND, NAND,
            OR, NOR,
            XOR, XNOR
        };
        public static string[] gateNames = { "IN", "OUT", "Buffer", "NOT", "AND", "NAND", "OR", "NOR", "XOR", "XNOR" };

        //constructors
        static Scheme()
        {
            lightBrush = new SolidColorBrush(Windows.UI.Colors.Cyan);
            darkBrush = new SolidColorBrush(Windows.UI.Colors.DarkCyan);
            userPoints = new Point[3];
        }

        //gets logic value from gate output or scheme input
        public static bool? GetValue(Line l)
        {
            foreach (Gate g in GateController.gates)
                if (g.id == (int)GateId.IN)
                {
                    if (l.X1 == g.title.Margin.Left + dotSize)
                        if (l.Y1 == g.title.Margin.Top + dotSize)
                        {
                            LineController.ColorLine(l, g.outputValue);
                            return g.outputValue;
                        }
                }
                else if (g.id != (int)GateId.OUT)
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

        //sends the value to the gate input or to scheme output
        public static void SendValue(bool? value, Line l)
        {
            foreach (Gate g in GateController.gates)
            {
                if(g.id == (int)GateId.OUT)
                {
                    if (l.X2 == g.title.Margin.Left + dotSize)
                        if (l.Y2 == g.title.Margin.Top + dotSize)
                        {
                            g.outputValue = value;
                            g.ColorByValue();
                        }
                            
                }
                else if (g.id != (int)GateId.IN)
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

        //determines line connection to the gate
        public static bool LineConnects(LineInfo li, Gate gate)
        {
            if (gate.id == (int)GateId.IN || gate.id == (int)GateId.OUT)
            {
                if (gate.title.Margin.Left + (lineStartOffset * 2) == li.point1.X)
                    if (gate.title.Margin.Top + (lineStartOffset * 2) == li.point1.Y)
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

        //specified function to determine line connection to the gate inputs
        private static bool LineConnectsToGateIn(LineInfo li, Gate gate)
        {
            foreach (Ellipse ellipse in gate.inputEllipse)
                if (ellipse.Margin.Left + lineStartOffset == li.point1.X)
                    if (ellipse.Margin.Top + lineStartOffset == li.point1.Y)
                        return true;
            return false;
        }

        //function to simplify saving points from Ellipse
        public static void SaveUserPointFromEllipse(int index, Ellipse e) =>
            userPoints[index] = new Point(e.Margin.Left + lineStartOffset,
                    e.Margin.Top + lineStartOffset);

        //function to simplify saving points from TextBlock
        public static void SaveUserPointFromTextBlock(int index, TextBlock tb) =>
            userPoints[index] = new Point(tb.Margin.Left + (lineStartOffset * 2),
                tb.Margin.Top + (lineStartOffset * 2));
    }
}