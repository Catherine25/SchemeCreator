using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI {
    class FrameManager {

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
            newGateMenuController.NewGateBtClickedEvent += NewGateBtClickedEvent;
        }

        public Constants.FrameEnum GetActiveFrame() => currentFrame;

        /*      data        */
        WorkspaceController workspaceController;

        MenuController menuController;

        NewGateMenuController newGateMenuController;

        ModeManager modeManager = new ModeManager();

        Data.Scheme scheme;

        Constants.FrameEnum currentFrame;

        public Grid Grid { get; } = new Grid {

            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };

        /*      events      */
        private void DotTappedEvent(object sender, DotTappedEventArgs e) {
            if(modeManager.CurrentMode == Constants.ModeEnum.addGateMode)
                SwitchToFrame(Constants.FrameEnum.newGate, Grid);
            scheme.dotController.lastTapped = e.dot;
        }

        private void NewGateBtClickedEvent(object sender, NewGateBtClickedEventArgs e) {

            if(Constants.external.Contains(e.type))
                scheme.gateController.gates.Add(new Data.Gate(
                    e.type,
                    e.isExternal,
                    e.inputs,
                    e.outputs,
                    scheme.dotController.lastTapped.Margin.Left + Constants.dotSize,
                    scheme.dotController.lastTapped.Margin.Top + Constants.dotSize
                ));
            else scheme.gateController.gates.Add(new Data.Gate(
                e.type,
                e.isExternal,
                e.inputs,
                e.outputs,
                scheme.dotController.lastTapped.Margin.Left + Constants.dotSize / 2 - Constants.gateWidth / 2,
                scheme.dotController.lastTapped.Margin.Top + Constants.dotSize / 2 - Constants.gateHeight / 2));

            SwitchToFrame(Constants.FrameEnum.workspace, Grid);
            workspaceController.ShowAll(ref scheme);
        }

        private void NewSchemeEvent(object sender, LastClickedBtEventArgs e) =>
            workspaceController.ShowAll(ref scheme);

        private void LoadSchemeEvent(object sender, LastClickedBtEventArgs e) => Data.Serializer.DeserializeAll(scheme);

        private void SaveSchemeEvent(object sender, LastClickedBtEventArgs e) => Data.Serializer.SerializeAll(scheme);

        private void TraceSchemeEvent(object sender, LastClickedBtEventArgs e) => Data.Tracing.ShowTracing(scheme);

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
            
            if(frame == Constants.FrameEnum.newGate) {
                menuController.Hide();
                workspaceController.Hide();
                newGateMenuController.Show();
                newGateMenuController.Update(Grid.Width, Grid.Height);
            }
            else {
                newGateMenuController.Hide();
                menuController.Show();
                menuController.Update(Grid.Width, Grid.Height);
            }
        }

        public void UpdateView() {

            System.Diagnostics.Debug.Assert(Grid.Width != 0);
            System.Diagnostics.Debug.Assert(Grid.Height != 0);
            
            if (currentFrame == Constants.FrameEnum.newGate)
                newGateMenuController.Update(Grid.Width, Grid.Height);
            else {
                menuController.Update(Grid.Width, Grid.Height);
                workspaceController.Update(Grid.Width, Grid.Height);
            }
        }

        public void SizeChanged(double width, double height) {
            Grid.Width = width;
            Grid.Height = height;   

            UpdateView();         
        } 
    }
}
