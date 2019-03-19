using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data {
    class Scheme {
        public UI.FrameManager frameManager;
        public LineController lineController = new LineController();
        public GateController gateController = new GateController();
        public DotController dotController = new DotController();
        //constructors
        public Scheme() => frameManager = new UI.FrameManager(this);

        //gets logic value from gate output or scheme input
        public bool? GetValue(Line l) {
            foreach (Gate g in gateController.gates)
                if (g.id == (int)Constants.GateEnum.IN) {
                    if (l.X1 == g.title.Margin.Left + Constants.dotSize)
                        if (l.Y1 == g.title.Margin.Top + Constants.dotSize) {
                            lineController.ColorLine(l, g.outputValue);
                            return g.outputValue;
                        }
                }
                else if (g.id != (int)Constants.GateEnum.OUT) {
                    if (l.X1 == g.outputEllipse.Margin.Left + Constants.lineStartOffset)
                        if (l.Y1 == g.outputEllipse.Margin.Top + Constants.lineStartOffset) {
                            lineController.ColorLine(l, g.outputValue);
                            return g.outputValue;
                        }
                }
            return null;
        }

        //sends the value to the gate input or to scheme output
        public void SendValue(bool? value, Line l) {
            foreach (Gate g in gateController.gates) {
                if(g.id == (int)Constants.GateEnum.OUT) {
                    if (l.X2 == g.title.Margin.Left + Constants.dotSize)
                        if (l.Y2 == g.title.Margin.Top + Constants.dotSize) {
                            g.outputValue = value;
                            g.ColorByValue();
                        }
                }
                else if (g.id != (int)Constants.GateEnum.IN) {
                    foreach (Ellipse e in g.inputEllipse) {
                        if (l.X2 == e.Margin.Left + Constants.lineStartOffset)
                            if (l.Y2 == e.Margin.Top + Constants.lineStartOffset) {
                                g.Work(value);
                                g.ColorByValue();
                                break;
                            }
                    }
                }
            }
        }

        //determines line connection to the gate
        //changes the isReserved bit
        public bool LineConnects(LineInfo li, Gate gate, bool invertIsReserved, int index) {
            if (gate.id == (int)Constants.GateEnum.IN || gate.id == (int)Constants.GateEnum.OUT) {
                if (gate.title.Margin.Left + (Constants.lineStartOffset * 2) == li.point1.X)
                    if (gate.title.Margin.Top + (Constants.lineStartOffset * 2) == li.point1.Y)
                        return true;
            }
            else {
                if (gate.outputEllipse.Margin.Left + Constants.lineStartOffset == li.point1.X)
                    if (gate.outputEllipse.Margin.Top + Constants.lineStartOffset == li.point1.Y)
                        return true;
                return LineConnectsToGateIn(li, gate, invertIsReserved, index);
            }
            return false;
        }

        //specified function to determine line connection to the gate inputs
        //changes the isReserved bit
        public bool LineConnectsToGateIn(LineInfo li, Gate gate, bool invertIsReserved, int index) {
            if (gate.id != (int)Constants.GateEnum.IN && gate.id != (int)Constants.GateEnum.OUT) {
                for (int i = 0; i < gate.inputEllipse.Length; i++) {
                    if (gate.inputEllipse[i].Margin.Left + Constants.lineStartOffset != li.point2.X)
                        continue;
                    if (gate.inputEllipse[i].Margin.Top + Constants.lineStartOffset != li.point2.Y)
                        continue;

                    if (invertIsReserved)
                        gateController.gateInfo[index].isInputsReserved[i] = !gateController.gateInfo[index].isInputsReserved[i];
                    return true;
                }
            }
            return false;
        }
    }
}