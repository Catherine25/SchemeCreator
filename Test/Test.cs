using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using static System.Diagnostics.Debug;
namespace SchemeCreator.Test {

    // DATA PART

    class DotControllerTest {

        public bool runTests() =>
            dotController_init_getDotCount() &
            dotController_init_net() &
            dotController_margin();

        bool dotController_init_getDotCount() =>
            new Data.DotController().getDotCount() == 0 ? true : false;
        bool dotController_init_net() {
            var dc = new Data.DotController();
            dc.initNet(100, 100);

            if(dc.getDotCount() == 64)
                return true;
            else return false;
        }
        bool dotController_margin() =>
            new Data.DotController().getDotByIndex(0).Margin ==
            new Thickness(10 - Constants.lineStartOffset,
            10 - Constants.lineStartOffset, 0, 0)
            ? true : false;
    }

    class GateTest {
        /* TODO:
        - drawBody()
        - drawGateInOut()
        - containsInOutByMargin()
        - getIndexOfInOutByMargin()
        - inputs
        - isExternal
        - outputs
        - type
        - values
        - x
        - y */
        public bool runTests() =>
            gate_containsInOut_input() &
            gate_containsInOut_output() &
            gate_getIndexOfInOut_input() &
            gate_getIndexOfInOut_output() &
            gate_init_input_count();

        bool gate_containsInOut_input() =>
            new Data.Gate(Constants.GateEnum.Buffer,
            false, 1, 1, 0.0, 0.0).containsInOutByMargin(
                new Ellipse() { Margin =
                new Thickness(0,0,0,0) }, true);
        bool gate_containsInOut_output() =>
            new Data.Gate(Constants.GateEnum.Buffer,
                false, 1, 1, 0.0, 0.0).containsInOutByMargin(
                    new Ellipse() { Margin = new Thickness(0,0,0,0) },
                    false);
        bool gate_getIndexOfInOut_input() =>
            new Data.Gate(Constants.GateEnum.Buffer,
                false, 1, 1, 0.0, 0.0).getIndexOfInOutByMargin(
                    new Ellipse() { Margin = new Thickness(0,0,0,0) },
                    true) == 0;
        bool gate_getIndexOfInOut_output() =>
            new Data.Gate(Constants.GateEnum.Buffer,
                false, 1, 1, 0.0, 0.0).getIndexOfInOutByMargin(
                    new Ellipse() { Margin = new Thickness(0,0,0,0) },
                    false) == 0;
        //TODO MORE TESTS
        bool gate_init_input_count() =>
            new Data.Gate(Constants.GateEnum.Buffer,
            false, 1, 1, 0.0, 0.0).values.Count == 1;

    }
    
    class GateControllerTest {
        /* TODO:
        - addGate() +
        - getExternalGates() +
        - getGateByInOut()
        - getGateByIndex()
        - getGateCount()
        - getIndexOf()
        - getLogicGates() */
        public bool runTests() => gateController_addGate() &
            gateController_getExternalGates();
        bool gateController_addGate() {
            
            var gc = new Data.GateController();
            
            gc.addGate(new Data.Gate(Constants.GateEnum.Buffer,
                false, 1, 1, 0.0, 0.0));

            if(gc.getGateCount() == 1)
                return true;
            else return false;
        }
        bool gateController_getExternalGates() {
            var gc = new Data.GateController();
            gc.addGate(new Data.Gate(Constants.GateEnum.Buffer, false, 1, 1, 0, 0));
            gc.addGate(new Data.Gate(Constants.GateEnum.IN, true, 1, 1, 0, 0));
            gc.addGate(new Data.Gate(Constants.GateEnum.OUT, false, 1, 1, 0, 0));

            if(gc.getExternalGates().Count == 2)
                return true;
            else return false;
        }
    }

    class LineControllerTest {
        /* TODO:
        - addWire()
        - colorLineByValues()
        - createWireByPoints()
        - getIndexOf()
        - getWireByIndex()
        - getWireCount()
        - reloadLines() */
        public bool runTests() => false;
    }

    class NewGateBtTest {
        /* TODO:
        - inputCount
        - isExternal
        - outputCount
        - type
        - x
        - y */
        public bool runTests() => false;
    }

    class SchemeTest {
        /* TODO: +
        - dotController +
        - frameManager +
        - gateController +
        - lineController */
        public bool runTests() =>
            scheme_init_dotController_init() &
            scheme_init_frameManager_init() &
            scheme_init_gateController_init() &
            scheme_init_lineController_init();
            
            bool scheme_init_dotController_init() =>
                new Data.Scheme().dotController != null ? true : false;
            bool scheme_init_frameManager_init() =>
                new Data.Scheme().frameManager != null ? true : false;
            bool scheme_init_gateController_init() =>
                new Data.Scheme().gateController != null ? true : false;
            bool scheme_init_lineController_init() =>
                new Data.Scheme().lineController != null ? true : false;
    }

    class SerializerTest {
        /* TODO:
        - DeserializeLine()
        - Load()
        - Save()
        - SerializeAll()
        - SerializeGate()
        - SerializeLine()
        - gatePath
        - linePath
        - serializeGateData
        - serializeLineData */
        public bool runTests() => false;
    }

    class TracingTest {
        /* TODO:
        - counter
        - endLinesId
        - middleLinesId
        - startLinesId
        - textBlocks */
        public bool runTests() => false;
    }

    class WireTest {
        /* TODO:
        - createWire()
        - end
        - isActive
        - start */
        public bool runTests() => false;
    }

    // UI PART

    class FrameManagerTest {
        public bool runTests() => false;
        public void scheme_frameManager_init() {
            
            var scheme = new SchemeCreator.Data.Scheme();

            Write("GetActiveFrame() init state");
            //WriteLineIf(scheme.frameManager.GetActiveFrame() == Constants.FrameEnum.workspace, OK);

            Assert(scheme.frameManager.GetActiveFrame() != Constants.FrameEnum.newGate);
            
            System.Diagnostics.Debug.WriteLine("Test 3.1 scheme_frameManager passed");            
        }
    }

    class Test {

        int testNumber = 0;

        string test = "test", progress = "", start = "start", end = "end", OK = "[OK]";
        
        void writeResult(string testName) => WriteLine("["+ test + testNumber.ToString() + "] "+ testName);

        public void runTests() {
            Write("SchemeTest ");
            WriteLineIf(new SchemeTest().runTests(), OK);

            Write("DotControllerTest ");
            WriteLineIf(new DotControllerTest().runTests(), OK);

            Write("GateTest ");
            WriteLineIf(new GateTest().runTests(), OK);

            Write("GateControllerTest ");
            WriteLineIf(new GateControllerTest().runTests(), OK);

            Write("LineControllerTest ");
            WriteLineIf(new LineControllerTest().runTests(), OK);

            Write("NewGateBtTest ");
            WriteLineIf(new NewGateBtTest().runTests(), OK);

            Write("SerializerTest ");
            WriteLineIf(new SerializerTest().runTests(), OK);

            Write("TracingTest ");
            WriteLineIf(new TracingTest().runTests(), OK);

            Write("WireTest ");
            WriteLineIf(new WireTest().runTests(), OK);
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
