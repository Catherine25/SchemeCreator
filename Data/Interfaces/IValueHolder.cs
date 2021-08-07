using System;

namespace SchemeCreator.Data.Interfaces
{
    public interface IValueHolder
    {
        public bool? Value { get; set; }
        public Action<bool?> ValueChanged { get; set; }
    }
}
