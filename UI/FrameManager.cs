using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

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
        }
        private void NewSchemeEvent(object sender, LastClickedButtonEventArgs e) =>
            workspaceController.ShowDots(ref scheme);
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
            menuController.InActivateModeButtons();
            e.button.BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
            scheme.frameManager.modeManager.CurrentMode = Constants.ModeEnum.addLineStartMode;
        }         
        private void RemoveLineEvent(object sender, LastClickedButtonEventArgs e) {
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
            }
            else {
                newGateMenuController.Hide();
                menuController.Show();
            }
        }
        public void UpdateView(double width, double height) {

            System.Diagnostics.Debug.Assert(Grid.Width != 0);
            System.Diagnostics.Debug.Assert(Grid.Height != 0);

            if (currentFrame == Constants.FrameEnum.newGate)
                newGateMenuController.Update(width, height);
            else {
                menuController.Update(width, height);
                workspaceController.Update(width, height);
            }
        } 
    }
}
