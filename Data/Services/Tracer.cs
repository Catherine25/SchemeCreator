﻿using System;
using System.Collections.Generic;
using System.Linq;
using SchemeCreator.Data.Controllers;
using SchemeCreator.Data.Models;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using PortType = SchemeCreator.UI.Dynamic.PortType;

namespace SchemeCreator.Data.Services
{
    public class Tracer
    {
        public List<HistoryComponent> TraceHistory;
     
        //public readonly int[] tracedGateIndexes, tracedWireIndexes, tracedExternalPortsIndexes;

        //private readonly int gateCount, wireCount, externalPortCount;

        //private int traceGateCounter = 1, traceWireCounter = 1, tracedExternalPortCounter;

        //constructor
        public Tracer(/*int gateCount, int wireCount*/)
        {
            System.Diagnostics.Debug.WriteLine("Tracer init");

            //get gate and wire count
            //this.gateCount = gateCount;
            //this.wireCount = wireCount;

            //set arrays size
            //tracedGateIndexes = new int[gateCount];
            //tracedWireIndexes = new int[wireCount];

            TraceHistory = new List<HistoryComponent>();
        }

        //methods

        ///<summary> Traces gates and wires </summary>
        //public void Trace(GateController gc, LineController lc)
        //{
        //    System.Diagnostics.Debug.Write("Trace() running...");

        //    ZeroWaveGateIndexes(gc);

        //    int curStepToTrace = 0;

        //    while (curStepToTrace != ComponentsToTrace())
        //    {
        //        curStepToTrace = ComponentsToTrace();

        //        WireIndexesWave(gc, lc);

        //        //  now we have traced wires, that connect to exINs
        //        //  now we need to get first Wave Gate Indexes:
        //        //      1) find the 'first wave' gates
        //        //      2) check that all their inputs have wires,
        //        //          that have been traced (they aren't 0)
        //        GateIndexesWave(gc, lc);
        //    }
        //}

        public void Trace(SchemeView scheme)
        {
            TraceExternalInputs(scheme);

            while (scheme.Gates.Count != TraceHistory.Count(x => x.Type == Constants.ComponentTypeEnum.gate)
                   || scheme.Wires.Count != TraceHistory.Count(x => x.Type == Constants.ComponentTypeEnum.wire))
            {
                bool somethingTraced = false;

                somethingTraced = TraceWires(scheme) || TraceGates(scheme);
                
                if(!somethingTraced)
                    throw new Exception();
            }

            TraceExternalOutputs(scheme);
        }

        ///<summary> Returns not traced components count </summary>
        //private int ComponentsToTrace()
        //{
        //    System.Diagnostics.Debug.Write("\nRunning ComponentsToTrace()... ");

        //    int counter = 0;

        //    for (int g = 0; g < gateCount; g++)
        //        if (tracedGateIndexes[g] == 0)
        //            counter++;
        //    for (int w = 0; w < wireCount; w++)
        //        if (tracedWireIndexes[w] == 0)
        //            counter++;

        //    System.Diagnostics.Debug.Write("Result " + counter);

        //    return counter;
        //}

        ///<summary> Sets traceCounter values in tracedGateIndexes
        ///by the exINs' index </summary>
        //private void ZeroWaveGateIndexes(GateController gc)
        //{
        //    System.Diagnostics.Debug.Write("\nRunning ZeroWaveGateIndexes()... ");

        //    for (int i = 0; i < gateCount; i++)
        //    {
        //        //get type
        //        var type = gc.Gates[i].Type;

        //        //if type is IN
        //        if (type == Constants.GateEnum.IN)
        //        {
        //            //save traceCounter' value in found gate' index
        //            tracedGateIndexes[i] = traceGateCounter;

        //            //increase traceCounter
        //            traceGateCounter++;

        //            //add data in history
        //            HistoryComponent Type = new HistoryComponent
        //            {
        //                Type = Constants.ComponentTypeEnum.gate,
        //                index = i
        //            };

        //            TraceHistory.Add(Type);
        //        }
        //    }

        //    System.Diagnostics.Debug.Write("Result:");

        //    for (int i = 0; i < gateCount; i++)
        //        System.Diagnostics.Debug.Write(" " + tracedGateIndexes[i]);
        //}

        ///<summary> Finds and traces the wires,
        ///that connect to gates found before </summary>
        //private void WireIndexesWave(GateController gc, LineController lc)
        //{
        //    System.Diagnostics.Debug.Write("\nRunning wireIndexesWave()...");

        //    for (int i = 0; i < gateCount; i++)
        //    {
        //        //get only already traced gates
        //        if (tracedGateIndexes[i] == 0)
        //            continue;

        //        for (int j = 0; j < wireCount; j++)
        //        {
        //            //do not rewrite indexes
        //            if (tracedWireIndexes[j] != 0)
        //                continue;

        //            //get the connected wire
        //            if (gc.Gates[i].WireConnects(lc.Wires[j].Start))
        //            {
        //                //save the wire's indexes in tracedWireIndexes
        //                tracedWireIndexes[j] = traceWireCounter;

        //                //increase counter
        //                traceWireCounter++;

        //                HistoryComponent Type = new HistoryComponent
        //                {
        //                    Type = Constants.ComponentTypeEnum.wire,
        //                    index = j
        //                };

        //                TraceHistory.Add(Type);
        //            }
        //        }
        //    }

        //    System.Diagnostics.Debug.Write("\nResult:");
        //    for (int i = 0; i < gateCount; i++)
        //        System.Diagnostics.Debug.Write(" " + tracedGateIndexes[i]);
        //}

        ///<summary> Finds the next 'wave' of gates,
        ///and checks that all their inputs have wires,
        ///that have been traced (they aren't 0) </summary>
        //private void GateIndexesWave(GateController gc, LineController lc)
        //{
        //    System.Diagnostics.Debug.Write("\nRunnung GateIndexesWave()...");

        //    for (int i = 0; i < wireCount; i++)
        //    {
        //        //take only already traced wires
        //        if (tracedWireIndexes[i] == 0)
        //            continue;

        //        //take the wire
        //        Wire curWire = lc.Wires[i];

        //        for (int j = 0; j < gateCount; j++)
        //        {
        //            // take the gate
        //            GateView currentGate = gc.Gates[j];

        //            //do not trace already traced gates
        //            if (tracedGateIndexes[j] != 0)
        //                continue;

        //            //check if the wire' end connects to the current gate
        //            if (currentGate.WireConnects(curWire.End))
        //            {
        //                //check is the gate' inputs have wires
        //                //that have been traced

        //                //get inputs count
        //                int inputsCount = currentGate.InputCount;

        //                for (int w2 = 0; w2 < wireCount; w2++)
        //                {
        //                    //get w2Wire
        //                    var w2Wire = lc.Wires[w2];

        //                    //check if connects
        //                    if (currentGate.WireConnects(w2Wire.End))

        //                        //check if traced
        //                        if (tracedWireIndexes[w2] != 0)
        //                            inputsCount--;
        //                }

        //                //if inputsCount = 0
        //                //then all the input wires have been traced
        //                if (inputsCount == 0)
        //                {
        //                    //now we can trace the gate
        //                    tracedGateIndexes[j] = traceGateCounter;

        //                    //and increase the counter
        //                    traceGateCounter++;

        //                    //add data in history

        //                    HistoryComponent Type = new HistoryComponent
        //                    {
        //                        Type = Constants.ComponentTypeEnum.gate,
        //                        index = j
        //                    };

        //                    TraceHistory.Add(Type);
        //                }
        //            }
        //        }
        //    }

        //    System.Diagnostics.Debug.Write("\nResult:");
        //    for (int i = 0; i < tracedGateIndexes.Length; i++)
        //        System.Diagnostics.Debug.Write(" " + tracedGateIndexes[i]);
        //}

        private void TraceExternalInputs(SchemeView scheme)
        {
            var externalPortViews = scheme.ExternalPorts.Where(x => x.Type == PortType.Input);
            foreach (ExternalPortView externalPortView in externalPortViews)
                TraceHistory.Add(new HistoryComponent
                {
                    Type = Constants.ComponentTypeEnum.externalPort,
                    TracedObject = externalPortView
                });
        }

        private bool TraceGates(SchemeView scheme)
        {
            var tracedWires = TraceHistory
                .Where(x => x.Type == Constants.ComponentTypeEnum.wire)
                .Select(x => x.TracedObject as WireView);

            var tracedGates = TraceHistory
                .Where(x => x.Type == Constants.ComponentTypeEnum.gate)
                .Select(x => x.TracedObject as GateView);

            var notTracedGates = scheme.Gates
                .Where(x => tracedGates
                    .Any(t => t.CenterPoint != x.CenterPoint));

            var gatesToTrace = notTracedGates
                .Where(gate => tracedWires
                    .Any(wire => gate.WireConnects(wire.End)));

            if (gatesToTrace.Count() != 0)
                return false;

            foreach (GateView gateView in gatesToTrace)
                TraceHistory.Add(new HistoryComponent
                {
                    Type = Constants.ComponentTypeEnum.gate,
                    TracedObject = gateView
                });

            return true;
        }

        private bool TraceWires(SchemeView scheme)
        {
            var tracedGates = TraceHistory
                .Where(x => x.Type == Constants.ComponentTypeEnum.gate)
                .Select(x => x.TracedObject as GateView);

            var wiresToTrace = scheme.Wires
                .Where(w => tracedGates
                    .Any(g => g.WireConnects(w.Start)));

            if(wiresToTrace.Count() == 0)
                return false;

            foreach (WireView wire in wiresToTrace)
                TraceHistory.Add(new HistoryComponent
                {
                    Type = Constants.ComponentTypeEnum.wire,
                    TracedObject = wire
                });

            return true;
        }

        private void TraceExternalOutputs(SchemeView scheme)
        {
            var externalPortViews = scheme.ExternalPorts.Where(x => x.Type == PortType.Output);
            foreach (ExternalPortView externalPortView in externalPortViews)
                TraceHistory.Add(new HistoryComponent
                {
                    Type = Constants.ComponentTypeEnum.externalPort,
                    TracedObject = externalPortView
                });
        }
    }

    public class HistoryComponent
    {
        public Constants.ComponentTypeEnum Type;
        public object TracedObject;
    }
}