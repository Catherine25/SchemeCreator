using System.Collections.Generic;
using Windows.Foundation;
using System.Linq;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data {
    public class GateController {
        IList<Gate> gates = new List<Gate>();
        public void addGate(Gate gate) => gates.Add(gate);
        public int getGateCount() => gates.Count;
        public int getIndexOf(Gate gate) => gates.IndexOf(gate);
        public Gate getGateByIndex(int index) => gates[index];
        public Gate getGateByBody(Button b) {
            foreach (Gate gate in gates)
                if(gate.containsBodyByMargin(b.Margin))
                    return gate;
            throw new System.Exception();
        }
        public Gate getGateByInOut(Ellipse e, bool isInput) {
            foreach (Gate gate in gates)
                if(gate.containsInOutByMargin(e, isInput))
                    return gate;
            throw new System.Exception();
        }
        public IList<Gate> getLogicGates() {

            var logicGates = new List<Gate>(); 

            foreach (var gate in from Gate g in gates
                where (!Constants.external.Contains(g.type))
                select g) {
                    logicGates.Add(gate); };

            return logicGates;
        }

        public IList<Gate> getExternalGates() {

            var externalGates = new List<Gate>(); 

            foreach (var gate in from Gate g in gates
                where (Constants.external.Contains(g.type))
                select g) {
                    externalGates.Add(gate); };

            return externalGates;
        }
    }
}
