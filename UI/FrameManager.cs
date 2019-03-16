using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.UI {
    class FrameManager {
        public FrameManager(Data.Scheme _scheme, Grid _grid) {
            scheme = _scheme;
            workspaceController = new WorkspaceController(_grid);
            menuController = new MenuController(_grid);
            newGateMenuController = new NewGateMenuController(_grid);

            workspaceController.DotTappedEvent += DotTappedEvent;
            menuController.NewSchemeBtClickEvent += NewSchemeEvent;
            menuController.LoadSchemeBtClickEvent += LoadSchemeEvent;
            menuController.SaveSchemeBtClickEvent += SaveSchemeEvent;
            menuController.TraceSchemeBtClickEvent += TraceSchemeEvent;
            menuController.WorkSchemeBtClickEvent += WorkSchemeEvent;
            menuController.AddGateBtClickEvent += AddGateEvent;
            menuController.AddLineBtClickEvent += AddLineEvent;
            menuController.RemoveLineBtClickEvent += RemoveLineEvent;
        }
        WorkspaceController workspaceController;
        MenuController menuController;
        NewGateMenuController newGateMenuController;
        ModeManager modeManager = new ModeManager();
        Data.Scheme scheme;
        Constants.FrameEnum currentFrame;
        //public Point[] userPoints = new Point[3];
        private void DotTappedEvent(object sender, DotTappedEventArgs e) {
            if(modeManager.CurrentMode == Constants.ModeEnum.addGateMode) {
                    //disable current button's border (automaticly)
                    //close this frame
                    //go to another frame
                }
        }
        private void NewSchemeEvent(object sender, LastClickedButtonEventArgs e) =>
            workspaceController.ReloadDots(scheme);
        private void LoadSchemeEvent(object sender, LastClickedButtonEventArgs e) => Data.Serializer.DeserializeAll(scheme);
        private void SaveSchemeEvent(object sender, LastClickedButtonEventArgs e) => Data.Serializer.SerializeAll(scheme);
        private void TraceSchemeEvent(object sender, LastClickedButtonEventArgs e) => Data.Tracing.ShowTracing(scheme);
        private void WorkSchemeEvent(object sender, LastClickedButtonEventArgs e) {
            // Clearing the elements data
            foreach (var gate in from Data.Gate gate in scheme.gateController.gates
                                 where gate.id != (byte)Constants.GateEnum.IN
                                 select gate) { gate.outputValue = null; }

            // Sends signals from outputs to inputs
            foreach (Line l in scheme.lineController.lines)
                scheme.SendValue(scheme.GetValue(l), l);
        }
        private void AddGateEvent(object sender, LastClickedButtonEventArgs e) {
            menuController.InActivateModeButtons();
            e.button.BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
            scheme.frameManager.modeManager.CurrentMode = Constants.ModeEnum.addGateMode;
        }
        private void AddLineEvent(object sender, LastClickedButtonEventArgs e) {
            e.button.BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
            scheme.frameManager.modeManager.CurrentMode = Constants.ModeEnum.addLineStartMode;
        }
            
        private void RemoveLineEvent(object sender, LastClickedButtonEventArgs e) {
            e.button.BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
            scheme.frameManager.modeManager.CurrentMode = Constants.ModeEnum.removeLineMode;
        }
        public Constants.FrameEnum GetActiveFrame() => currentFrame;
        public void SwitchToFrame(Constants.FrameEnum frame, Grid grid) {
            if(frame!=currentFrame) {
                currentFrame = frame;
                DisposeAll();
                if(frame == Constants.FrameEnum.newGate)
                    newGateMenuController = new NewGateMenuController(grid);
                else {
                    menuController = new MenuController(grid);
                    workspaceController = new WorkspaceController(grid);
                }
            }   
        }
        public void DisposeAll() {
            workspaceController.Dispose();
            menuController.Dispose();
            newGateMenuController.Dispose();
        }
        public void UpdateView(double width, double height) {
            menuController.UpdateView(width, height);
        }
    }
}
