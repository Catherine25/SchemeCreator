using SchemeCreator.Data;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.UI {
    public class FrameManager {

        /*      constructor     */
        public FrameManager(Scheme _scheme) {
            
            scheme = _scheme;

            workspaceController = new WorkspaceController();
            menuController = new MenuController();
            newGateMenuController = new NewGateMenuController();

            menuController.SetParentGrid(Grid);
            workspaceController.SetParentGrid(Grid);
            newGateMenuController.SetParentGrid(Grid);

            SwitchToFrame(Constants.FrameEnum.workspace, Grid);
            
            EventSubscribe();
        }

        /*      methods     */
        
        private void EventSubscribe() {

            menuController.NewSchemeBtClickEvent += NewSchemeEvent;
            menuController.LoadSchemeBtClickEvent += LoadSchemeEvent;
            menuController.SaveSchemeBtClickEvent += SaveSchemeEvent;
            menuController.TraceSchemeBtClickEvent += TraceSchemeEvent;
            menuController.WorkSchemeBtClickEvent += WorkSchemeEvent;
            menuController.AddGateBtClickEvent += AddGateEvent;
            menuController.AddLineBtClickEvent += AddLineEvent;
            menuController.RemoveLineBtClickEvent += RemoveLineEvent;
            menuController.ChangeValueBtClickEvent += ChangeValueEvent;
            
            workspaceController.DotTappedEvent += DotTappedEvent;
            workspaceController.LogicGateInTapped += LogicGateInTapped;
            workspaceController.LogicGateOutTapped += LogicGateOutTapped;
            workspaceController.LogicGateTappedEvent += LogicGateTappedEvent;
            workspaceController.GateINTapped += GateINTappedEvent;
            workspaceController.GateOUTTapped += GateOUTTappedEvent;
            
            newGateMenuController.NewGateBtClickedEvent += NewGateBtClickedEvent;
        }

        private void LogicGateOutTapped(Ellipse e) {

            if(modeManager.CurrentMode == Constants.ModeEnum.addLineStartMode) {

                modeManager.CurrentMode = Constants.ModeEnum.addLineEndMode;

                newWire = new Wire {
                    start = new Point(
                    e.Margin.Left + Constants.lineStartOffset,
                    e.Margin.Top + Constants.lineStartOffset) };

                Gate gate = scheme.gateController.GetGateByInOut(e, false);

                newWire.isActive = gate.values[gate.GetIndexOfInOutByMargin(e.Margin, false)];
            }
        }

        private void LogicGateInTapped(Ellipse e) {

            if(modeManager.CurrentMode == Constants.ModeEnum.addLineEndMode) {

                modeManager.CurrentMode = Constants.ModeEnum.addLineStartMode;

                Point point = new Point {
                    X = e.Margin.Left + Constants.lineStartOffset,
                    Y = e.Margin.Top + Constants.lineStartOffset
                };

                newWire.end = point;

                scheme.lineController.Wires.Add(newWire);

                workspaceController.ShowLines(ref scheme.lineController);
            }
        }

        private void DotTappedEvent(Ellipse e) {

            if(modeManager.CurrentMode == Constants.ModeEnum.addGateMode)
                SwitchToFrame(Constants.FrameEnum.newGate, Grid);

            scheme.dotController.lastTapped = e;
        }

        private void LogicGateTappedEvent(Button b) => 
            throw new NotImplementedException();

        private void GateINTappedEvent(Button b) {

            Constants.ModeEnum? curMode = modeManager.CurrentMode;

            if (curMode == Constants.ModeEnum.addLineStartMode) {

                modeManager.CurrentMode = Constants.ModeEnum.addLineEndMode;

                Point point = new Point {
                    X = b.Margin.Left + Constants.externalGateWidth / 2,
                    Y = b.Margin.Top + Constants.externalGateHeight / 2
                };

                newWire = new Wire {
                    start = point,
                    isActive = scheme.gateController.GetGateByBody(b).values[0]
                };
            }
            else if (curMode == Constants.ModeEnum.changeValueMode) {

                Gate gate = scheme.gateController.GetGateByBody(b);
                gate.values[0] = !gate.values[0];

                if(gate.values[0]) {
                    b.Background = Constants.brushes[Constants.AccentEnum.light1];
                    b.Foreground = Constants.brushes[Constants.AccentEnum.dark1];
                }
                else {
                    b.Background = Constants.brushes[Constants.AccentEnum.dark1];
                    b.Foreground = Constants.brushes[Constants.AccentEnum.light1];
                }
            }
        }

        private void GateOUTTappedEvent(Button b) {

            if(modeManager.CurrentMode == Constants.ModeEnum.addLineEndMode) {

                modeManager.CurrentMode = Constants.ModeEnum.addLineStartMode;

                Point point = new Point() {
                    X = b.Margin.Left + (Constants.externalGateWidth / 2),
                    Y = b.Margin.Top + (Constants.externalGateHeight / 2)
                };

                newWire.end = point;
                
                scheme.lineController.Wires.Add(newWire);

                workspaceController.ShowLines(ref scheme.lineController);
            }
        }

        public Constants.FrameEnum GetActiveFrame() => currentFrame;

        /*      data        */
        WorkspaceController workspaceController;

        MenuController menuController;

        NewGateMenuController newGateMenuController;

        ModeManager modeManager = new ModeManager();

        Scheme scheme;

        Constants.FrameEnum currentFrame;

        Wire newWire;

        public Grid Grid { get; } = new Grid {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };

        /*      events      */
        private void NewGateBtClickedEvent(object sender, NewGateBtClickedEventArgs e) {

            if (Constants.external.Contains(e.type)) {

                Point point = new Point {

                    X = scheme.dotController.lastTapped.Margin.Left
                    + Constants.dotSize,

                    Y = scheme.dotController.lastTapped.Margin.Top
                    + Constants.dotSize
                };

                Gate gate = new Gate(
                    e.type,
                    e.isExternal,
                    e.inputs,
                    e.outputs,
                    point.X,
                    point.Y);

                scheme.gateController.Gates.Add(gate);
            }
            else {

                Point point = new Point {

                    X = scheme.dotController.lastTapped.Margin.Left
                    + (Constants.dotSize / 2)
                    - (Constants.gateWidth / 2),

                    Y = scheme.dotController.lastTapped.Margin.Top
                    + (Constants.dotSize / 2)
                    - (Constants.gateHeight / 2)
                };

                Gate gate = new Gate(e.type,
                e.isExternal,
                e.inputs,
                e.outputs,
                point.X,
                point.Y);

                scheme.gateController.Gates.Add(gate);
            }

            SwitchToFrame(Constants.FrameEnum.workspace, Grid);
            workspaceController.ShowAll(ref scheme);
        }

        private void NewSchemeEvent(object sender, LastClickedBtEventArgs e) =>
            workspaceController.ShowAll(ref scheme);

        private void LoadSchemeEvent(object sender, LastClickedBtEventArgs e) =>
            Serializer.DeserializeAll(scheme);

        private void SaveSchemeEvent(object sender, LastClickedBtEventArgs e) =>
            Serializer.SerializeAll(scheme);

        private void TraceSchemeEvent(object sender, LastClickedBtEventArgs e) {
            
            int gateCount = scheme.gateController.Gates.Count;
            System.Diagnostics.Debug.WriteLine(gateCount);

            int wireCount = scheme.lineController.Wires.Count;
            System.Diagnostics.Debug.WriteLine(wireCount);

            Tracer tracer = new Tracer(gateCount, wireCount);

            tracer.Trace(scheme.gateController, scheme.lineController);

            int[] tracedWireIndexes = tracer.GetWireIndexes();

            workspaceController.ShowWireTraceIndexes(tracedWireIndexes,
            scheme.lineController);            
        }

        private void WorkSchemeEvent(object sender, LastClickedBtEventArgs e) {
            // // Clearing the elements data
            // foreach (var gate in from Data.Gate gate in scheme.gateController.gates
            //                      where gate.type != Constants.GateEnum.IN
            //                      select gate) { gate.outputValue = null; }

            // // Sends signals from outputs to inputs
            // foreach (Line l in scheme.lineController.lines)
            //     scheme.SendValue(scheme.GetValue(l), l);
        }
        
        private void AddGateEvent(object sender, LastClickedBtEventArgs e) {
            
            menuController.InActivateModeButtons();
            e.button.BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
            scheme.frameManager.modeManager.CurrentMode = Constants.ModeEnum.addGateMode;
        }
        
        private void AddLineEvent(object sender, LastClickedBtEventArgs e) {
            
            menuController.InActivateModeButtons();
            e.button.BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
            scheme.frameManager.modeManager.CurrentMode = Constants.ModeEnum.addLineStartMode;
        }

        private void RemoveLineEvent(object sender, LastClickedBtEventArgs e) {
            
            menuController.InActivateModeButtons();
            e.button.BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
            scheme.frameManager.modeManager.CurrentMode = Constants.ModeEnum.removeLineMode;
        }

        private void ChangeValueEvent(object sender, LastClickedBtEventArgs e) {

            menuController.InActivateModeButtons();
            e.button.BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
            scheme.frameManager.modeManager.CurrentMode = Constants.ModeEnum.changeValueMode;
        }

        public void SwitchToFrame(Constants.FrameEnum frame, Grid grid) {

            currentFrame = frame;

            Size size = new Size(Grid.Width, Grid.Height);

            if(frame == Constants.FrameEnum.newGate) {

                menuController.Hide();
                workspaceController.Hide();
                newGateMenuController.Show();
                newGateMenuController.Update(size);
            }
            else {

                newGateMenuController.Hide();
                menuController.Show();
                menuController.Update(size);
            }
        }

        public void UpdateView() {

            System.Diagnostics.Debug.Assert(Grid.Width != 0);
            System.Diagnostics.Debug.Assert(Grid.Height != 0);
            
            Size size = new Size(Grid.Width, Grid.Height);

            if (currentFrame == Constants.FrameEnum.newGate)
                newGateMenuController.Update(size);
            else {

                menuController.Update(size);
                workspaceController.Update(size);
            }
        }

        public void SizeChanged(Size size) {
            
            Grid.Width = size.Width;
            Grid.Height = size.Height;   

            UpdateView();
        } 
    }
}
