using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.Data.Interfaces
{
    public interface ILayer<T> where T : UserControl
    {
        public IEnumerable<T> Items { get; }

        public void Add(T item);
        public void Clear();
    }
}
