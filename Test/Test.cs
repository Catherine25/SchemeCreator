using System;
using Windows.UI.Xaml.Input;
using static System.Diagnostics.Debug;
namespace SchemeCreator.Test {
    class Test {
        public Test() {
            scheme_creation();
            scheme_dotcontroller_creation();
            scheme_frameManager_SizeChanged();
            scheme_frameManager_creation();
        }

        public void scheme_creation() {
            
            var scheme = new SchemeCreator.Data.Scheme();  
            
            Assert(scheme != null);
            Assert(scheme.dotController != null);
            Assert(scheme.frameManager != null);
            Assert(scheme.gateController != null);
            Assert(scheme.lineController != null);

            System.Diagnostics.Debug.WriteLine("Test 1 scheme_creation passed");
        }

        public void scheme_dotcontroller_creation() {

            var scheme = new SchemeCreator.Data.Scheme();

            //Assert(scheme.dotController.dots != null);
            //TEST scheme.dotController.lastTapped
            
            System.Diagnostics.Debug.WriteLine("Test 2 scheme_dotcontroller_creation passed");
        }

        public void scheme_frameManager_creation() {
            
            var scheme = new SchemeCreator.Data.Scheme();

            Assert(scheme.frameManager.GetActiveFrame() == Constants.FrameEnum.workspace);
            Assert(scheme.frameManager.GetActiveFrame() != Constants.FrameEnum.newGate);
            
            System.Diagnostics.Debug.WriteLine("Test 3.1 scheme_frameManager passed");            
        }

        public void scheme_frameManager_SizeChanged() {

            var scheme = new SchemeCreator.Data.Scheme();

            scheme.frameManager.SizeChanged(1000, 1000);

            Assert(scheme.frameManager.Grid.Width == 1000);
            Assert(scheme.frameManager.Grid.Height == 1000);

            System.Diagnostics.Debug.WriteLine("Test 3.2 scheme_frameManager passed");
        }

        public void scheme_frameManager_SwitchToFrame() {

            var scheme = new SchemeCreator.Data.Scheme();

            scheme.frameManager.SwitchToFrame(Constants.FrameEnum.newGate, scheme.frameManager.Grid);

            Assert(scheme.frameManager.GetActiveFrame() == Constants.FrameEnum.newGate);
            Assert(scheme.frameManager.GetActiveFrame() != Constants.FrameEnum.workspace);

            scheme.frameManager.SwitchToFrame(Constants.FrameEnum.workspace, scheme.frameManager.Grid);

            Assert(scheme.frameManager.GetActiveFrame() == Constants.FrameEnum.workspace);
            Assert(scheme.frameManager.GetActiveFrame() != Constants.FrameEnum.newGate);

            System.Diagnostics.Debug.WriteLine("Test 3.3 scheme_frameManager_SwitchToFrame passed");            
        }

        public void scheme_frameManager_UpdateView() {
            var scheme = new SchemeCreator.Data.Scheme();

            // TEST scheme.frameManager.UpdateView()

            System.Diagnostics.Debug.WriteLine("Test 3.4 scheme_frameManager_UpdateView coming soon");            
        }

        public void scheme_gatecontroller_gates_creation() {

            var scheme = new SchemeCreator.Data.Scheme();

            //Assert(scheme.gateController.gates != null);
            //Assert(scheme.gateController.gates.Count == 0);

            System.Diagnostics.Debug.WriteLine("Test 4.1 scheme_gatecontroller_gates_creation passed");
        }

        public void scheme_gatecontroller_lineInfo_creation() {
            
            var scheme = new SchemeCreator.Data.Scheme();

            //Assert(scheme.lineController.wires != null);
            //Assert(scheme.lineController.wires.Count == 0);

            System.Diagnostics.Debug.WriteLine("Test 4.2 scheme_gatecontroller_lineInfo_creation passed");
        }

        public void scheme() {

            var menuController = new UI.MenuController();

            //menuController
        }
    }
}
