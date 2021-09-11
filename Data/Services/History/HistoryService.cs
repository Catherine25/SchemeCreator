using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Services.History
{
    /// <summary>
    /// Used by <see cref="Tracer"/> to store history components.
    /// </summary>
    public class HistoryService
    {
        public List<HistoryComponent> TraceHistory = new();

        public IEnumerable<HistoryComponent> GetAll(string typeName) => TraceHistory.Where(c => c.TypeName == typeName);

        public void AddToHistory<T>(IEnumerable<T> components) where T : UserControl =>
            components.ToList().ForEach(c => AddToHistory(c));

        public void AddToHistory<T>(T component) where T : UserControl =>
            TraceHistory.Add(new HistoryComponent(component));

        public int Count(string typeName) => TraceHistory.Count(c => c.TypeName == typeName);
    }
}