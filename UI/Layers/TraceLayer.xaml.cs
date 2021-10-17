using System.Collections.Generic;
using System.Linq;
using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.Data.Services.History;
using SchemeCreator.UI.Dynamic;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class TraceLayer : UserControl, ILayer<TraceLabel>
    {
        public IEnumerable<TraceLabel> Items => Grid.Children.Select(x => x as TraceLabel);

        public TraceLayer()
        {
            InitializeComponent();
            Grid.InitGridColumnsAndRows(Constants.GridSize);
        }

        public void ShowTracings(HistoryService service)
        {
            Clear();

            var wires = service.GetAll(nameof(WireView)).ToList();

            for (int i = 0; i < wires.Count; i++)
            {
                var wireView = wires[i].TracedObject as WireView;

                var startPoint = wireView.Connection.StartPoint;
                var endPoint = wireView.Connection.EndPoint;

                var x = (startPoint.X + endPoint.X) / 2;
                var y = (startPoint.Y + endPoint.Y) / 2;

                var traceLabel = new TraceLabel(i, x, y);

                AddToView(traceLabel);
            }
        }

        public void AddToView(TraceLabel item) => Grid.Children.Add(item);
        public void Clear() => Grid.Children.Clear();
    }
}
