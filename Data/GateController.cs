using System.Collections.Generic;
using Windows.Foundation;
using System.Linq;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data {
    public class GateController {

        public GateController() => Gates = new List<Gate>();

        IList<Gate> gates;

        public IList<Gate> Gates {
            get => gates;
            set => gates = value;
        }

        public Gate GetGateByBody(Button b) {
            foreach (Gate gate in Gates)
                if(gate.ContainsBodyByMargin(b.Margin))
                    return gate;
            throw new System.Exception();
        }

        public Gate GetGateByWire(Wire w) {
            foreach (Gate gate in Gates)
                if(gate.GetBodyByWire(w) != null)
                    return gate;
                else if(gate.GetInOutByWire(w) != null)
                    return gate;
                return null;
        }

        public Gate GetGateByInOut(Ellipse e, bool isInput) {

            foreach (Gate gate in Gates)
                if(gate.ContainsInOutByMargin(e, isInput))
                    return gate;

            throw new System.Exception();
        }

        public IList<Gate> GetLogicGates() {

            var logicGates = new List<Gate>(); 

            foreach (var gate in from Gate g in Gates
                where (!Constants.external.Contains(g.type))
                select g) {
                    logicGates.Add(gate); }

            return logicGates;
        }

        public IList<Gate> GetExternalGates() {

            var externalGates = new List<Gate>(); 

            foreach (var gate in from Gate g in Gates
                where (Constants.external.Contains(g.type))
                select g) {
                    externalGates.Add(gate); }

            return externalGates;
        }

        public IList<Gate> GetExternalInputs() {
            
            var externalInputs = new List<Gate>();

            foreach(var gate in from Gate g in Gates
                where(Constants.GateEnum.IN == g.type)
                select g) {
                    externalInputs.Add(gate); }
            
            return externalInputs;
        }
    }
}
