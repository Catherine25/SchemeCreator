using System;
using System.Collections.Generic;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Services
{
    /// <summary>
    /// Exception to throw in case the Gate has Custom type.
    /// </summary>
    public class CustomGateException : Exception { }
    
    public static class StandardGateWorkPatterns
    {
        public static readonly Dictionary<GateEnum, Func<List<bool?>, List<bool?>>> ActionByType = new()
        {
            { GateEnum.Buffer, BufferWork },
            { GateEnum.Not, NotWork },
            { GateEnum.And, AndWork },
            { GateEnum.Nand, NandWork },
            { GateEnum.Or, OrWork },
            { GateEnum.Nor, NorWork },
            { GateEnum.Xor, XorWork },
            { GateEnum.Xnor, XnorWork },
            { GateEnum.Custom, CustomWork }
        };

        public static List<bool?> BufferWork(List<bool?> inputs) => inputs;
        
        public static List<bool?> NotWork(List<bool?> inputs)
        {
            inputs[0] = !inputs[0];

            return inputs;
        }
        
        public static List<bool?> AndWork(List<bool?> inputs)
        {
            for (int i = 1; i < inputs.Count; i++)
                inputs[0] &= inputs[i];

            return inputs;
        }
        
        private static List<bool?> NandWork(List<bool?> inputs)
        {
            for (int i = 1; i < inputs.Count; i++)
                inputs[0] &= inputs[i];

            inputs[0] = !inputs[0];

            return inputs;
        }

        private static List<bool?> OrWork(List<bool?> inputs)
        {
            for (int i = 1; i < inputs.Count; i++)
                inputs[0] |= inputs[i];

            return inputs;
        }

        private static List<bool?> NorWork(List<bool?> inputs)
        {
            for (int i = 1; i < inputs.Count; i++)
                inputs[0] |= inputs[i];
                
            inputs[0] = !inputs[0];

            return inputs;
        }

        private static List<bool?> XorWork(List<bool?> inputs)
        {
            for (int i = 1; i < inputs.Count; i++)
                inputs[0] ^= inputs[i];

            return inputs;
        }

        private static List<bool?> XnorWork(List<bool?> inputs)
        {
            for (int i = 1; i < inputs.Count; i++)
                inputs[0] ^= inputs[i];
                
            inputs[0] = !inputs[0];

            return inputs;
        }

        private static List<bool?> CustomWork(List<bool?> inputs) => throw new CustomGateException();
    }
}
