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

            Assert(scheme.dotController.dots != null);
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
    }
}
