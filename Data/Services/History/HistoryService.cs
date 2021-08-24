using System;
using System.Collections.Generic;
using System.Linq;

namespace SchemeCreator.Data.Services.History
{
    public class HistoryService
    {
        public List<HistoryComponent> TraceHistory = new List<HistoryComponent>();

        public IEnumerable<HistoryComponent> GetAll(string typeName) => TraceHistory.Where(c => c.TypeName == typeName);
        public int Count(string typeName) => TraceHistory.Count(c => c.TypeName == typeName);
    }
}