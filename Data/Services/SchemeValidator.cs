using System.Linq;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Services
{
    public static class SchemeValidator
    {
        public static bool ValidateAsync(SchemeView scheme)
        {
            if(!HasAnyExternalInput(scheme))
            {
                new Message(Messages.NoExternalInputs).ShowAsync();
                return false;
            }

            if (!HasAnyExternalOutput(scheme))
            {
                new Message(Messages.NoExternalOutputs).ShowAsync();
                return false;
            }

            if(!ExternalInputsInited(scheme))
            {
                new Message(Messages.ExternalInputsNotInited).ShowAsync();
                return false;
            }

            if(!AnyWires(scheme))
            {
               new Message(Messages.NoWires).ShowAsync();
               return false;
            }

            return true;
        }

        private static bool HasAnyExternalInput(SchemeView scheme) =>
            scheme.ExternalPorts.Any(p => p.Type == PortType.Input);

        private static bool HasAnyExternalOutput(SchemeView scheme) =>
            scheme.ExternalPorts.Any(p => p.Type == PortType.Output);

        private static bool ExternalInputsInited(SchemeView scheme) =>
            scheme.GetFirstNotInitedExternalPort() == null;

        private static bool AnyWires(SchemeView scheme) =>
            scheme.Wires.Count() > 0;
    }
}
