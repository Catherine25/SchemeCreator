namespace SchemeCreator.Data.Exceptions.Displayable
{
    public class NoExternalInputsException : DisplayableException
    {
        public NoExternalInputsException() : base(
            "No external inputs!",
            $"Scheme has to have at least 1 external input to be aligned.") { }
    }
}