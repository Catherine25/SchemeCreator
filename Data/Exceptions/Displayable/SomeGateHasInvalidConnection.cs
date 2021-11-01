namespace SchemeCreator.Data.Exceptions.Displayable
{
    public class SomeGateHasInvalidConnection : DisplayableException
    {
        public SomeGateHasInvalidConnection() : base(
            "Some gate has invalid connection!",
            "Some gate has not enough input or output wires.\n" +
            "Please add missing wires.")
        {
        }
    }
}