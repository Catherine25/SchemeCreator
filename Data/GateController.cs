using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data
{
    public static class GateController
    {
        public static List<Gate> gates;
        public const int gateHeight = 70;
        public const int gateWidth = 50;
        //
        private static int dotSize;
        private static double lineStartOffset, offset;
        private static SolidColorBrush lightBrush, darkBrush;
        static GateController()
        {
            gates = new List<Gate>();
            lineStartOffset = Scheme.lineStartOffset;
            dotSize = Scheme.dotSize;
            lightBrush = Scheme.lightBrush;
            darkBrush = Scheme.darkBrush;
            offset = Scheme.offset;
        }
        public static void CreateGate(Point p, string gateName, int newGateInputs)
        {
            //setting margin
            Thickness t = new Thickness
            {
                Top = p.Y - gateHeight / 2,
                Left = p.X - gateWidth / 2
            };
            //initialization
            Gate gate = new Gate
            {
                //gate name
                gateName = new TextBlock()
                {
                    Text = gateName,
                    TextAlignment = TextAlignment.Center,
                    Height = gateHeight,
                    Width = gateWidth,
                    Margin = t,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                //body of gate
                gateRect = new Rectangle()
                {
                    Height = gateHeight,
                    Width = gateWidth,
                    Fill = darkBrush,
                    Margin = t,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                inputEllipse = new Ellipse[newGateInputs],
                inputReserved = new bool[newGateInputs]
            };
            //inputs
            for (int i = 0; i < gate.inputEllipse.Length; i++)
            {
                t.Top += gate.gateRect.Height / (gate.inputEllipse.Length + 1);
                if (i == 0)
                    t.Top -= lineStartOffset;
                t.Left = gate.gateRect.Margin.Left - lineStartOffset;
                gate.inputEllipse[i] = new Ellipse()
                {
                    Height = dotSize,
                    Width = dotSize,
                    Fill = lightBrush,
                    Margin = t,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                gate.inputReserved[i] = false;
            }
            //output
            t.Left += gate.gateRect.Width;
            t.Top = gate.gateRect.Margin.Top + gateHeight / 2 - lineStartOffset;
            gate.outputEllipse = new Ellipse()
            {
                Height = offset,
                Width = offset,
                Fill = lightBrush,
                Margin = t,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            gates.Add(gate);
        }
        public static void CreateInOut(Point p, string gateName)
        {
            Thickness t = new Thickness()
            {
                Left = p.X - offset,
                Top = p.Y - offset
            };
            Gate gate = new Gate()
            {
                gateRect = new Rectangle()
                {
                    Width = dotSize * 2,
                    Height = dotSize * 2,
                    Fill = darkBrush,
                    Margin = t,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                gateName = new TextBlock()
                {
                    Width = dotSize * 2,
                    Height = dotSize * 2,
                    Text = gateName,
                    FontSize = 10,
                    Margin = t,
                    TextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                outputValue = false
            };
            gates.Add(gate);
        }
    }
}
