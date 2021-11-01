using System;
using SchemeCreator.UI.Dialogs;

namespace SchemeCreator.Data.Exceptions.Displayable
{
    /// <summary>
    /// Exception that can be displayed by <see cref="Message"/>.
    /// </summary>
    public class DisplayableException : Exception
    {
        protected DisplayableException(string message, string description) : base(message)
        {
            Description = description;
        }

        public string Description { get; }
    }
}