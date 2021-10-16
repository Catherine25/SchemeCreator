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

        public IEnumerable<(string, IEnumerable<(int, HistoryComponent)>)> GetComponentsWithSameType()
        {
            var history = new Queue<HistoryComponent>(TraceHistory);
            var result = new List<(string, IEnumerable<(int, HistoryComponent)>)>();
            int iterator = 0;

            while (history.TryDequeue(out var first))
            {
                var elements = history.TakeWhile(x => x.TypeName == first.TypeName).ToList();
                elements.Add(first);
                var res = (first.TypeName, elements);
                result.Add((res.TypeName, elements.Select(x => (iterator++, x))));
            }

            return result;
        }

        public IEnumerable<HistoryComponent> GetAll(string typeName) => TraceHistory.Where(c => c.TypeName == typeName);

        public void AddToHistory<T>(IEnumerable<T> components) where T : UserControl =>
            components.ToList().ForEach(c => AddToHistory(c));

        public void AddToHistory<T>(T component) where T : UserControl =>
            TraceHistory.Add(new HistoryComponent(component));

        public int Count(string typeName) => TraceHistory.Count(c => c.TypeName == typeName);
    }
}