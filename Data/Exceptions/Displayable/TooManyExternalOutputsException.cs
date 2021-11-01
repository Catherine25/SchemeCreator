using SchemeCreator.UI;

namespace SchemeCreator.Data.Exceptions.Displayable
{
    public class TooManyExternalOutputsException : DisplayableException
    {
        public TooManyExternalOutputsException() : base(
            "Too many external outputs!",
            $"External outputs amount cannot exceed {SchemeView.GridSize.Height}.") { }
    };
}