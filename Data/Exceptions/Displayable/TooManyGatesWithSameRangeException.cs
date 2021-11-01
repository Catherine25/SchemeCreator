using SchemeCreator.UI;

namespace SchemeCreator.Data.Exceptions.Displayable
{
    public class TooManyGatesWithSameRangeException : DisplayableException
    {
        public TooManyGatesWithSameRangeException() : base(
            "Too many gates with same range!", 
            $"Scheme can't have more than {SchemeView.GridSize.Height} gates with same range.") { }
    }
}