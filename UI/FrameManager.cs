using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.UI {
    public class FrameManager {

        /*      constructor     */
        public FrameManager(Data.Scheme _scheme) {
            
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

                newWire = new Data.Wire {
                    start = new Point(
                    e.Margin.Left + Constants.lineStartOffset,
                    e.Margin.Top + Constants.lineStartOffset) };

                Data.Gate gate = scheme.gateController.GetGateByInOut(e, false);

                newWire.isActive = gate.values[gate.GetIndexOfInOutByMargin(e.Margin, false)];
            }
        }

        private void LogicGateInTapped(Ellipse e) {

            if(modeManager.CurrentMode == Constants.ModeEnum.addLineEndMode) {

                modeManager.CurrentMode = Constants.ModeEnum.addLineStartMode;

                newWire.end = new Point(e.Margin.Left
                    + Constants.lineStartOffset,
                    e.Margin.Top + Constants.lineStartOffset);

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

            if(modeManager.CurrentMode == Constants.ModeEnum.addLineStartMode) {
                modeManager.CurrentMode = Constants.ModeEnum.addLineEndMode;

                newWire = new Data.Wire {
                    start = new Point( b.Margin.Left + Constants.externalGateWidth / 2,
                    b.Margin.Top + Constants.externalGateHeight / 2),

                    isActive = scheme.gateController.GetGateByBody(b).values[0] };

            }
        }

        private void GateOUTTappedEvent(Button b) {

            if(modeManager.CurrentMode == Constants.ModeEnum.addLineEndMode) {
                modeManager.CurrentMode = Constants.ModeEnum.addLineStartMode;
                
                newWire.end = new Point(b.Margin.Left + Constants.externalGateWidth / 2,
                    b.Margin.Top + Constants.externalGateHeight / 2);
                
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

        Data.Scheme scheme;

        Constants.FrameEnum currentFrame;

        Data.Wire newWire;

        public Grid Grid { get; } = new Grid {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };

        /*      events      */
        private void NewGateBtClickedEvent(object sender, NewGateBtClickedEventArgs e) {

            if(Constants.external.Contains(e.type))
                scheme.gateController.Gates.Add(new Data.Gate(
                    e.type,
                    e.isExternal,
                    e.inputs,
                    e.outputs,
                    scheme.dotController.lastTapped.Margin.Left + Constants.dotSize,
                    scheme.dotController.lastTapped.Margin.Top + Constants.dotSize
                ));
            else scheme.gateController.Gates.Add(new Data.Gate(
                e.type,
                e.isExternal,
                e.inputs,
                e.outputs,
                scheme.dotController.lastTapped.Margin.Left + Constants.dotSize / 2 - Constants.gateWidth / 2,
                scheme.dotController.lastTapped.Margin.Top + Constants.dotSize / 2 - Constants.gateHeight / 2));

            SwitchToFrame(Constants.FrameEnum.workspace, Grid);
            workspaceController.ShowAll(ref scheme);
        }

        private void NewSchemeEvent(object sender,
        LastClickedBtEventArgs e) =>
            workspaceController.ShowAll(ref scheme);

        private void LoadSchemeEvent(object sender,
        LastClickedBtEventArgs e) =>
            Data.Serializer.DeserializeAll(scheme);

        private void SaveSchemeEvent(object sender,
        LastClickedBtEventArgs e) =>
            Data.Serializer.SerializeAll(scheme);

        private void TraceSchemeEvent(object sender,
        LastClickedBtEventArgs e) {
            
            int gateCount = scheme.gateController.Gates.Count;
            System.Diagnostics.Debug.WriteLine(gateCount);

            int wireCount = scheme.lineController.Wires.Count;
            System.Diagnostics.Debug.WriteLine(wireCount);

            Data.Tracer tracer = new Data.Tracer(gateCount, wireCount);

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

            if (currentFrame == Constants.FrameEnum.newGate) {
                newGateMenuController.Update(size);
            }
            else {
                menuController.Update(size);
                workspaceController.Update(size);
            }
        }

        public void SizeChanged(double width, double height) {
            
            Grid.Width = width;
            Grid.Height = height;   

            UpdateView();
        } 
    }
}
