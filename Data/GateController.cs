using System.Collections.Generic;
using Windows.Foundation;
using System.Linq;

namespace SchemeCreator.Data {
    public class GateController {
        IList<Gate> gates = new List<Gate>();
        public void addGate(Gate gate) => gates.Add(gate);
        public int getGateCount() => gates.Count;
        public int getIndexOf(Gate gate) => gates.IndexOf(gate);
        public Gate getGateByIndex(int index) => gates[index];
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
