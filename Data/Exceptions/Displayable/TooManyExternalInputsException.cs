using SchemeCreator.UI;

namespace SchemeCreator.Data.Exceptions.Displayable
{
    public class TooManyExternalInputsException : DisplayableException
    {
        public TooManyExternalInputsException() : base(
            "Too many external inputs!",
            $"External inputs amount cannot exceed {SchemeView.GridSize.Height}.") { }
    };
}