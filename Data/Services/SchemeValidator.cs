using System.Linq;
using SchemeCreator.UI;

namespace SchemeCreator.Data.Services
{
    public static class SchemeValidator
    {
        public static bool ValidateAsync(SchemeView scheme)
        {
            if(!scheme.ExternalInputs.Any())
            {
                new Message(Messages.NoExternalInputs).ShowAsync();
                return false;
            }

            if (!scheme.ExternalOutputs.Any())
            {
                new Message(Messages.NoExternalOutputs).ShowAsync();
                return false;
            }

            if(!ExternalInputsInited(scheme))
            {
                new Message(Messages.ExternalInputsNotInited).ShowAsync();
                return false;
            }

            if(!scheme.Wires.Any())
            {
               new Message(Messages.NoWires).ShowAsync();
               return false;
            }

            return true;
        }

        private static bool ExternalInputsInited(SchemeView scheme) =>
            scheme.GetFirstNotInitedExternalPort() == null;
    }
}
