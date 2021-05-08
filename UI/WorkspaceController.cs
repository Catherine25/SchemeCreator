using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data.Controllers;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Models;
using SchemeCreator.UI.Dynamic;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.UI
{
    class WorkspaceController
    {
        public WorkspaceController() => _grid = new Grid();
        private WireBuilder WireBuilder;

        /*      methods      */
        public void AddGateToGrid(GateView gate)
        {
            _grid.Children.Add(gate);
            
            gate.GateBodyTapped += LogicGateTappedEvent;

            //var body = gate.DrawBody();
            //body.AddToParent(_grid);

            //if (external.Contains(gate.Type))
            //{
            //    if (gate.Type == GateEnum.IN)
            //        body.Tapped += (GateView g, Button button) => GateINTapped(gate, button);
            //    else
            //        body.Tapped += (GateView g, Button button) => GateOUTTapped(gate, button);
            //    return;
            //}

            //body.Tapped += (GateView g, Button button) => LogicGateTappedEvent(gate, button);

            //gate.DrawPorts(ConnectionTypeEnum.Both).ForEach(p =>
            //{
            //    p.Tapped += (GatePortView port) => PortTapped(port);
            //    p.AddToParent(_grid);
            //});
        }

        public void AddExternalPortToGrid(ExternalPortView portView)
        {
            portView.Tapped += view => ExternalPortViewTappedEvent(view);
            _grid.Children.Add(portView); 
        }

        public void RemoveLine(Line line) => _grid.Children.Remove(line);
        public void SetParentGrid(Grid parentGrid) => _grid.Children.Add(parentGrid);
        public void Hide() => _grid.Children.Clear();
        public void Update(Rect rect) => _grid.SetRect(rect);

        //public void ShowDots(ref DotController dotController)
        //{
        //    dotController.InitNet();

        //    AddMatrix(ref _grid, netSize);

        //    dotController.Dots.ForEach(dot =>
        //    {
        //        var (ellipse, vector2) = dot;

        //        _grid.Children.Add(ellipse);
        //        ellipse.Tapped += (sender, e) => DotTapped(ellipse, vector2);
        //        ellipse.PointerEntered += (sender, args) => ellipse.IncreaseSize();
        //        ellipse.PointerExited += (sender, args) => ellipse.DecreaseSize();
        //    });
        //}

        public void ShowGates(ref GateController gateController)
        {
            foreach (GateView gate in gateController.Gates)
            {
                gate.GateBodyTapped += (view, gateView) => LogicGateTappedEvent(view, gateView);
                gate.GatePortTapped += (view, gateView) => PortTapped(view, gateView);

                _grid.Children.Add(gate);
            }

            foreach (ExternalPortView externalPort in gateController.ExternalPorts)
            {
                externalPort.Tapped += ExternalPortViewTappedEvent;
                _grid.Children.Add(externalPort);
            }

            //IEnumerable<GateView> logicGates = gateController.GetLogicGates();

            //foreach (GateView gate in logicGates)
            //{
            //    var body = gate.DrawBody();
            //    body.AddToParent(_grid);
            //    body.Tapped += (GateView g, Button button) => LogicGateTappedEvent(gate, button); ;

            //    var items = gate.DrawPorts(ConnectionTypeEnum.Both);
            //    foreach (var item in items)
            //    {
            //        item.AddToParent(_grid);
            //        item.Tapped += (GatePortView port) => PortTapped(port);
            //    }
            //}

            //List<GateView> externalGates = new List<GateView>(gateController.GetExternalGates());
            //foreach(GateView gate in externalGates)
            //{
            //    var body = gate.DrawBody();
            //    body.AddToParent(_grid);

            //    if (gate.Type == GateEnum.IN)
            //        body.Tapped += (GateView g, Button button) => GateINTapped(gate, button);
            //    else
            //        body.Tapped += (GateView g, Button button) => GateOUTTapped(gate, button);
            //}
        }

        public void ShowLines(SchemeView scheme)
        {
            for (int i = 0; i < scheme.Wires.Count; i++)
            {
                WireView line = scheme.Wires[i];
                line.PointerEntered += Line_PointerEntered;
                line.PointerExited += Line_PointerExited;
                _grid.Children.Add(line);
            }
        }

        private void Line_PointerExited(object sender, PointerRoutedEventArgs e) =>
            (sender as Line).StrokeThickness /= 2;

        private void Line_PointerEntered(object sender, PointerRoutedEventArgs e) =>
            (sender as Line).StrokeThickness *= 2;

        public void ShowWireTraceIndexes(int[] tracedWireIndexes, SchemeView scheme)
        {
            int wireCount = scheme.Wires.Count;

            for (int i = 0; i < wireCount; i++)
            {
                WireView wire = scheme.Wires[i];

                Button tb = new Button
                {
                    Content = tracedWireIndexes[i].ToString()
                };

                tb.SetStandartAlignment();
                tb.SetSizeAndCenter(TraceTextSize, wire.CenterPoint);

                _grid.Children.Add(tb);
            };
        }

        //public void ShowAll(ref Scheme scheme)
        //{
        //    ShowDots(ref scheme.dotController);
        //    ShowGates(ref scheme.gateController);
        //    ShowLines(ref scheme.lineController);
        //}

        public void AddMatrix(ref Grid grid, int netSize)
        {
            for (int i = 0; i < netSize; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        public void DotTapped(Ellipse ellipse, Vector2 position) =>
            DotTappedEvent(ellipse, position);

        #region Events

        public event Action<GateBodyView, GateView> LogicGateTappedEvent;
            //GateINTapped, GateOUTTapped;

        public event Action<ExternalPortView> ExternalPortViewTappedEvent;

        public event Action<Ellipse, Vector2> DotTappedEvent;
        public event Action<GatePortView, GateView> PortTapped;

        #endregion

        private Grid _grid;
    }
}