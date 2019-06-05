using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data {

    class Tracer {

        //data
        bool[] isGateTraced, isWireTraced;
        int[] tracedGateIndexes, tracedWireIndexes;
        int gateCount, wireCount;
        int traceGateCounter = 1, traceWireCounter = 1;

        //constructor
        public Tracer(int gateCount, int wireCount) {

                //get gate and wire count
                this.gateCount = gateCount;
                this.wireCount = wireCount;

                //set arrays size
                tracedGateIndexes = new int[gateCount];
                tracedWireIndexes = new int[wireCount];
        }

        //methods

        ///<summary> Traces gates and wires </summary>
        public void trace(GateController gc, LineController lc) {

            zeroWaveGateIndexes(gc);

                int curStepToTrace = 0;

                while(curStepToTrace != componentsToTrace()) {

                    curStepToTrace = componentsToTrace();

                    wireIndexesWave(gc, lc);

                    //  now we have traced wires, that connect to exINs
                    //  now we need to get first Wave Gate Indexes: 
                    //      1) find the 'first wave' gates
                    //      2) check that all their inputs have wires,
                    //          that have been traced (they aren't 0)
                    gateIndexesWave(gc, lc);
                }
        }

        ///<summary> Returns the traced indexes </summary>
        public int[] getWireIndexes() => tracedWireIndexes;

        ///<summary> Returns not traced components count </summary>
        private int componentsToTrace() {

            int counter = 0;

            for (int g = 0; g < gateCount; g++)
                if(tracedGateIndexes[g] == 0)
                    counter++;
            for (int w = 0; w < wireCount; w++)
                if(tracedWireIndexes[w] == 0)
                    counter++;

            return counter;
        }

        ///<summary> Sets traceCounter values in tracedGateIndexes
        ///by the exINs' index </summary>
        private void zeroWaveGateIndexes(GateController gc) {

            for (int i = 0; i < gateCount; i++) {

                    //get type
                    var type = gc.getGateByIndex(i).type;

                    //if type is IN
                    if(type == Constants.GateEnum.IN) {
                        
                        //save traceCounter' value in found gate' index
                        tracedGateIndexes[i] = traceGateCounter;

                        //increase traceCounter
                        traceGateCounter++;
                    }
            }
        }

        ///<summary> Finds and traces the wires,
        ///that connect to gates found before </summary>
        private void wireIndexesWave(GateController gc, LineController lc) {

            for (int i = 0; i < gateCount; i++) {

                    //get only already traced gates
                    if(tracedGateIndexes[i] == 0)
                        continue;

                    for (int j = 0; j < wireCount; j++) {

                        //get the connected wire
                        if(gc.getGateByIndex(i).WireStartConnects(
                        lc.getWireByIndex(j))) {

                            //save the wire's indexes in tracedWireIndexes
                            tracedWireIndexes[j] = traceWireCounter;

                            //increase counter
                            traceWireCounter++;
                        }
                    }
                }
        }

        ///<summary> Finds the next 'wave' of gates,
        ///and checks that all their inputs have wires,
        ///that have been traced (they aren't 0) </summary>
        private void gateIndexesWave(GateController gc, LineController lc) {

            for (int i = 0; i < wireCount; i++) {

                    //take only already traced wires
                    if(tracedWireIndexes[i] == 0)
                        continue;

                    //take the wire
                    Wire curWire = lc.getWireByIndex(i);

                    for (int j = 0; j < gateCount; j++) {

                        // take the gate
                        Gate currentGate = gc.getGateByIndex(j);

                        //check if the wire' end connects to the current gate
                        if(currentGate.WireEndConnects(curWire)) {

                            //check is the gate' inputs have wires
                            //that have been traced

                            //get inputs count
                            int inputsCount = currentGate.inputs;

                            for (int w2 = 0; w2 < wireCount; w2++) {
                                
                                //get w2Wire
                                var w2Wire = lc.getWireByIndex(w2);

                                //check if connects
                                if(currentGate.WireEndConnects(w2Wire))

                                    //check if traced
                                    if(tracedWireIndexes[w2] != 0)
                                        inputsCount--;
                            }

                            //if inputsCount = 0
                            //then all the input wires have been traced
                            if(inputsCount == 0) {

                                //now we can trace the gate
                                tracedGateIndexes[j] = traceGateCounter;

                                //and increase the counter
                                traceGateCounter++;
                            }
                        }
                    }
            }
        }
    }
}