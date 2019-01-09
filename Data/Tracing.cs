using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data
{
    static class Tracing
    {
        //data
        public static SortedSet<int> startLinesId, middleLinesId, endLinesId;
        public static List<TextBlock> textBlocks;
        public static int counter;

        //constructor
        static Tracing()
        {
            startLinesId = new SortedSet<int>();
            middleLinesId = new SortedSet<int>();
            endLinesId = new SortedSet<int>();

            textBlocks = new List<TextBlock>();
        }

        //methods
        public static void ShowTracing()
        {
            NumerateInputLines();
            NumerateCenterLines();
            NumerateOutputLines();

            counter = 0;
            textBlocks.Clear();

            for (int i = 0; i < startLinesId.Count; i++)
            {
                counter++;

                TextBlock tb = new TextBlock()
                {
                    Margin = new Thickness
                    {
                        Left = (LineController.lineInfo[startLinesId.ElementAt(i)].point1.X + LineController.lineInfo[startLinesId.ElementAt(i)].point2.X) / 2,
                        Top = ((LineController.lineInfo[startLinesId.ElementAt(i)].point1.Y + LineController.lineInfo[startLinesId.ElementAt(i)].point2.Y) / 2) + Scheme.offset
                    },

                    Text = counter.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };

                textBlocks.Add(tb);
            }

            for (int i = 0; i < middleLinesId.Count; i++)
            {
                counter++;

                TextBlock tb = new TextBlock()
                {
                    Margin = new Thickness
                    {
                        Left = (LineController.lineInfo[middleLinesId.ElementAt(i)].point1.X + LineController.lineInfo[middleLinesId.ElementAt(i)].point2.X) / 2,
                        Top = ((LineController.lineInfo[middleLinesId.ElementAt(i)].point1.Y + LineController.lineInfo[middleLinesId.ElementAt(i)].point2.Y) / 2) + Scheme.offset
                    },

                    Text = counter.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };

                textBlocks.Add(tb);
            }

            for (int i = 0; i < endLinesId.Count; i++)
            {
                counter++;

                TextBlock tb = new TextBlock()
                {
                    Margin = new Thickness
                    {
                        Left = (LineController.lineInfo[endLinesId.ElementAt(i)].point1.X + LineController.lineInfo[endLinesId.ElementAt(i)].point2.X) / 2,
                        Top = ((LineController.lineInfo[endLinesId.ElementAt(i)].point1.Y + LineController.lineInfo[endLinesId.ElementAt(i)].point2.Y) / 2) + Scheme.offset
                    },

                    Text = counter.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };

                textBlocks.Add(tb);
            }
        }

        //firstly numerating the input lines
        private static void NumerateInputLines()
        {
            startLinesId.Clear();

            for (int i = 0; i < LineController.lineInfo.Count; i++)
                foreach (var gate in from Gate gate in GateController.gates
                                         //looking for inputs
                                     where gate.title.Text == "IN"
                                     //comparing inputs coordinates with line coordinates
                                     where Scheme.LineConnects(LineController.lineInfo[i], gate, false, 0)
                                     select gate)
                {
                    //saving index
                    startLinesId.Add(i);
                }
        }

        //numerating lines without inputs and outputs
        private static void NumerateCenterLines()
        {
            middleLinesId.Clear();

            for (int i = 0; i < LineController.lineInfo.Count; i++)
                foreach (var gate in from Gate gate in GateController.gates
                                         //comparing inputs coordinates with line coordinates
                                     where Scheme.LineConnects(LineController.lineInfo[i], gate, false, 0)
                                     select gate)
                {
                    //looking for regular gates
                    if (gate.title.Text == "IN" || gate.title.Text == "OUT")
                        break;

                    //saving index
                    middleLinesId.Add(i);
                    break;
                }
        }
        //numerating the output lines
        private static void NumerateOutputLines()
        {
            endLinesId.Clear();

            for (int i = 0; i < LineController.lineInfo.Count; i++)
                foreach (var gate in from Gate gate in GateController.gates
                                         //looking for for outputs
                                     where gate.title.Text == "OUT"
                                     //comparing inputs coordinates with line coordinates
                                     where Scheme.LineConnects(LineController.lineInfo[i], gate, false, 0)
                                     select gate)
                {
                    //saving index
                    endLinesId.Add(i);
                }
        }
    }
}