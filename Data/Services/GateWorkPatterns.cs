using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Services
{
    static class GateWorkPatterns
    {
        public static Dictionary<GateEnum, Func<List<bool?>, List<bool?>>> ActionByType =
            new Dictionary<GateEnum, Func<List<bool?>, List<bool?>>> {
                { GateEnum.Buffer, BufferWork },
                { GateEnum.NOT, NotWork },
                { GateEnum.AND, AndWork },
                { GateEnum.NAND, NandWork },
                { GateEnum.OR, OrWork },
                { GateEnum.NOR, NorWork },
                { GateEnum.XOR, XorWork },
                { GateEnum.XNOR, XnorWork }
            };

        public static List<bool?> BufferWork(List<bool?> inputs) => inputs;
        public static List<bool?> NotWork(List<bool?> inputs) => inputs;
        public static List<bool?> AndWork(List<bool?> inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
                inputs[0] &= inputs[i];
            return inputs;
        }
        private static List<bool?> NandWork(List<bool?> inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
                inputs[0] &= inputs[i];

            inputs[0] = !inputs[0];

            return inputs;
        }

        private static List<bool?> OrWork(List<bool?> inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
                inputs[0] |= inputs[i];

            return inputs;
        }

        private static List<bool?> NorWork(List<bool?> inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
                inputs[0] |= inputs[i];
                
            inputs[0] = !inputs[0];

            return inputs;
        }

        private static List<bool?> XorWork(List<bool?> inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
                inputs[0] ^= inputs[i];

            return inputs;
        }

        private static List<bool?> XnorWork(List<bool?> inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
                inputs[0] ^= inputs[i];
                
            inputs[0] = !inputs[0];

            return inputs;
        }
    }
}
