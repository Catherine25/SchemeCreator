using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Interfaces
{
    public interface ILayer<T> where T : UserControl
    {
        public IEnumerable<T> Items { get; }

        public void Add(T item);
        public void Clear();
    }
}
