namespace SchemeCreator.Data.Services.History
{
    public class HistoryComponent
    {
        public HistoryComponent(object obj) => TracedObject = obj;

        public string TypeName { get => TracedObject.GetType().Name; }
        public object TracedObject;
    }
}
