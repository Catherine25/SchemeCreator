using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data {
    static class Tracing {
        //data
        public static SortedSet<int> startLinesId, middleLinesId, endLinesId;
        public static List<TextBlock> textBlocks;
        public static int counter;

        //constructor
        static Tracing() {
            startLinesId = new SortedSet<int>();
            middleLinesId = new SortedSet<int>();
            endLinesId = new SortedSet<int>();

            textBlocks = new List<TextBlock>();
        }

        //methods
        public static void ShowTracing(Scheme scheme) {
            
            NumerateInputLines(scheme);
            NumerateCenterLines(scheme);
            NumerateOutputLines(scheme);

            counter = 0;
            textBlocks.Clear();

            for (int i = 0; i < startLinesId.Count; i++) {
                counter++;

                TextBlock tb = new TextBlock() {
                    Margin = new Thickness {
                        Left = (scheme.lineController.getWireByIndex(startLinesId.ElementAt(i)).start.X +
                        scheme.lineController.getWireByIndex(startLinesId.ElementAt(i)).end.X) / 2,
                        Top = ((scheme.lineController.getWireByIndex(startLinesId.ElementAt(i)).start.Y +
                        scheme.lineController.getWireByIndex(startLinesId.ElementAt(i)).end.Y) / 2) + SchemeCreator.Constants.offset
                    },

                    Text = counter.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };

                textBlocks.Add(tb);
            }

            for (int i = 0; i < middleLinesId.Count; i++) {
                counter++;

                TextBlock tb = new TextBlock() {
                    Margin = new Thickness {
                        Left = (scheme.lineController.getWireByIndex(startLinesId.ElementAt(i)).start.X +
                        scheme.lineController.getWireByIndex(startLinesId.ElementAt(i)).end.X) / 2,

                        Top = ((scheme.lineController.getWireByIndex(startLinesId.ElementAt(i)).start.Y +
                        scheme.lineController.getWireByIndex(startLinesId.ElementAt(i)).end.Y) / 2) +
                        SchemeCreator.Constants.offset
                    },

                    Text = counter.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };

                textBlocks.Add(tb);
            }

            for (int i = 0; i < endLinesId.Count; i++) {
                counter++;

                TextBlock tb = new TextBlock() {
                    Margin = new Thickness {
                        Left = (scheme.lineController.getWireByIndex(startLinesId.ElementAt(i)).start.X +
                        scheme.lineController.getWireByIndex(startLinesId.ElementAt(i)).end.X) / 2,
                        Top = ((scheme.lineController.getWireByIndex(startLinesId.ElementAt(i)).start.Y +
                        scheme.lineController.getWireByIndex(startLinesId.ElementAt(i)).end.Y) / 2) + SchemeCreator.Constants.offset
                    },

                    Text = counter.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };

                textBlocks.Add(tb);
            }
        }

        //firstly numerating the input lines
        private static void NumerateInputLines(Scheme scheme) {
            // startLinesId.Clear();

            // for (int i = 0; i < scheme.lineController.getWireCount(); i++)
            
            //     foreach (var gate in from Gate gate in scheme.gateController.gates
            //                              //looking for inputs
            //                          where gate.type == Constants.GateEnum.IN
            //                          //comparing inputs coordinates with line coordinates
            //                          //where scheme.LineConnects(scheme.lineController.lineInfo[i], gate, false, 0)
            //                          select gate) {
            //         //saving index
            //         startLinesId.Add(i);
            //     }
        }

        //numerating lines without inputs and outputs
        private static void NumerateCenterLines(Scheme scheme) {
            // middleLinesId.Clear();

            // for (int i = 0; i < scheme.lineController.getWireCount(); i++)
            //     foreach (var gate in from Gate gate in scheme.gateController.gates
            //                              //comparing inputs coordinates with line coordinates
            //                          //where scheme.LineConnects(scheme.lineController.lineInfo[i], gate, false, 0)
            //                          select gate)
            //     {
            //         //looking for regular gates
            //         if (Constants.external.Contains(gate.type))
            //             break;

            //         //saving index
            //         middleLinesId.Add(i);
            //         break;
            //     }
        }
        //numerating the output lines
        private static void NumerateOutputLines(Scheme scheme) {
            // endLinesId.Clear();

            // for (int i = 0; i < scheme.lineController.getWireCount(); i++)
            //     foreach (var gate in from Gate gate in scheme.gateController.gates
            //                              //looking for for outputs
            //                          where gate.type == Constants.GateEnum.OUT
            //                          //comparing inputs coordinates with line coordinates
            //                          //where scheme.LineConnects(scheme.lineController.lineInfo[i], gate, false, 0)
            //                          select gate) {
            //         //saving index
            //         endLinesId.Add(i);
            //     }
        }
    }
}