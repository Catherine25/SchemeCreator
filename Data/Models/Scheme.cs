using SchemeCreator.Data.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Models
{
    public class Scheme
    {
        //public LineController lineController = new LineController();
        //public GateController gateController = new GateController();
        //public DotController dotController = new DotController();

        //public bool IsAllConnected()
        //{
        //    Stack<GateView> gates = new Stack<GateView>(gateController.Gates);
        //    List<Wire> wires = new List<Wire>(lineController.Wires);

        //    bool found = true;

        //    if (AllExternalPortsConnect(Type.Input, wires))
        //        return false;

        //    if (AllExternalPortsConnect(Type.Output, wires))
        //        return false;

        //    while (gates.Count != 0 && found)
        //    {
        //        GateView gate = gates.Pop();
        //        found = false;

        //        int connections = gate.InputCount + gate.OutputCount;
                
        //        foreach (var wire in wires)
        //            if (gate.WireConnects(wire.Start) || gate.WireConnects(wire.End))
        //                connections--;

        //        if (connections <= 0)
        //            found = true;
        //    }

        //    return gates.Count == 0;
        //}

        //private bool AllExternalPortsConnect(Type type, List<Wire> wires)
        //{
        //    IEnumerable<Vector3> wirePointsToCheck = type == Type.Input 
        //        ? wires.Select(x => x.Start)
        //        : wires.Select(x => x.End);
            
        //    Stack<ExternalPortView> externalPorts =
        //        new Stack<ExternalPortView>(gateController.ExternalPorts.Where(x => x.Type == type));

        //    bool found = true;

        //    while (externalPorts.TryPop(out var externalPort) && found)
        //    {
        //        found = false;

        //        foreach (Vector3 point in wirePointsToCheck)
        //            if (externalPort.CenterPoint == point)
        //            {
        //                found = true;
        //                break;
        //            }
        //    }

        //    return found;
        //}
    }
}