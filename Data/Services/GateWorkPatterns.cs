using SchemeCreator.Data.Models;
using System;
using System.Collections.Generic;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Services
{
    static class GateWorkPatterns
    {
        public static Dictionary<GateEnum, Action<Gate>> ActionByType =
            new Dictionary<GateEnum, Action<Gate>> {
                { GateEnum.IN, BufferWork },
                { GateEnum.OUT, BufferWork },
                { GateEnum.Buffer, BufferWork },
                { GateEnum.NOT, NotWork },
                { GateEnum.AND, AndWork },
                { GateEnum.NAND, NandWork },
                { GateEnum.OR, OrWork },
                { GateEnum.NOR, NorWork },
                { GateEnum.XOR, XorWork },
                { GateEnum.XNOR, XnorWork }
            };

        private static void BufferWork(Gate gate) =>
            gate.values[0] = gate.values[0];
        
        private static void NotWork(Gate gate) =>
            gate.values[0] = !gate.values[0];

        private static void AndWork(Gate gate)
        {
            for (int i = 0; i < gate.values.Count; i++)
                gate.values[0] &= gate.values[i];
        }

        private static void NandWork(Gate gate)
        {
            for (int i = 0; i < gate.values.Count; i++)
                gate.values[0] &= gate.values[i];

            gate.values[0] = !gate.values[0];
        }

        private static void OrWork(Gate gate)
        {
            for (int i = 0; i < gate.values.Count; i++)
                gate.values[0] |= gate.values[i];
        }

        private static void NorWork(Gate gate)
        {
            for (int i = 0; i < gate.values.Count; i++)
                gate.values[0] |= gate.values[i];
                
            gate.values[0] = !gate.values[0];
        }

        private static void XorWork(Gate gate)
        {
            for (int i = 0; i < gate.values.Count; i++)
                gate.values[0] ^= gate.values[i];
        }

        private static void XnorWork(Gate gate)
        {
            for (int i = 0; i < gate.values.Count; i++)
                gate.values[0] ^= gate.values[i];
                
            gate.values[0] = !gate.values[0];
        }
    }
}
