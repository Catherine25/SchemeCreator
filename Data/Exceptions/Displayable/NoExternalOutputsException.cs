namespace SchemeCreator.Data.Exceptions.Displayable
{
    public class NoExternalOutputsException : DisplayableException
    {
        public NoExternalOutputsException() : base(
            "No external outputs!",
            $"Scheme has to have at least 1 external output to be aligned.") { }
    };
}