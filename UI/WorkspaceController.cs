using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data.Extensions;
using System.Collections.Generic;
using SchemeCreator.Data.Models;
using SchemeCreator.Data.Controllers;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.UI
{
    class WorkspaceController
    {
        public WorkspaceController()
        {
            grid = new Grid();
            grid.SetStandartAlighnment();
        }

        /*      methods      */
        public void AddGateToGrid(Gate gate)
        {
            var body = gate.DrawBody();
            body.AddToParent(grid);

            if (external.Contains(gate.type))
            {
                if (gate.type == GateEnum.IN)
                    body.Tapped += GateINBodyTapped;
                else
                    body.Tapped += GateOUTBodyTapped;
                return;
            }
            else
                body.Tapped += LogicGateBodyTapped;

            gate.DrawGateInPorts().ForEach(p =>
            {
                p.Tapped += GateInTapped;
                p.AddToParent(grid);
            });

            gate.DrawGateOutPorts().ForEach(p =>
            {
                p.Tapped += GateOutTapped;
                p.AddToParent(grid);
            });
        }
        public void RemoveLine(Line line) => grid.Children.Remove(line);
        public void SetParentGrid(Grid parentGrid) => parentGrid.Children.Add(grid);
        public void Hide() => grid.Children.Clear();
        public void Update(Rect rect) => grid.SetRect(rect);

        public void ShowDots(ref DotController dotController)
        {
            dotController.InitNet(grid.GetActualSize());

            dotController.Dots.ForEach(dot =>
            {
                grid.Children.Add(dot);
                dot.Tapped += DotTapped;
                dot.PointerEntered += (object sender, PointerRoutedEventArgs args) => dot.IncreaseSize();
                dot.PointerExited += (object sender, PointerRoutedEventArgs args) => dot.DecreaseSize();
            });
        }

        public void ShowGates(ref GateController gateController)
        {
            IEnumerable<Gate> logicGates = gateController.GetLogicGates();

            foreach (Gate gate in logicGates)
            {
                var body = gate.DrawBody();
                body.AddToParent(grid);
                body.Tapped += LogicGateBodyTapped;

                foreach (Port p in gate.DrawGateInPorts())
                {
                    p.AddToParent(grid);
                    p.Tapped += GateInTapped;
                }

                foreach (Port p in gate.DrawGateOutPorts())
                {
                    p.AddToParent(grid);
                    p.Tapped += GateOutTapped;
                }
            }

            List<Gate> externalGates = new List<Gate>(gateController.GetExternalGates());
            foreach(Gate gate in externalGates)
            {
                var body = gate.DrawBody();
                body.AddToParent(grid);

                if (gate.type == GateEnum.IN)
                    body.Tapped += GateINBodyTapped;
                else
                    body.Tapped += GateOUTBodyTapped;
            }
        }

        public void ShowLines(ref LineController lineController)
        {
            for (int i = 0; i < lineController.Wires.Count; i++)
            {
                Line line = lineController.Wires[i].CreateLine();
                line.Tapped += Wire_Tapped;
                line.PointerEntered += Line_PointerEntered;
                line.PointerExited += Line_PointerExited;
                grid.Children.Add(line);
            }
        }

        private void Line_PointerExited(object sender, PointerRoutedEventArgs e) =>
            (sender as Line).StrokeThickness /= 2;

        private void Line_PointerEntered(object sender, PointerRoutedEventArgs e) =>
            (sender as Line).StrokeThickness *= 2;

        private void Wire_Tapped(object sender, TappedRoutedEventArgs e) =>
            LineTappedEvent(sender as Line);

        public void ShowWireTraceIndexes(int[] tracedWireIndexes, LineController lc)
        {
            int wireCount = lc.Wires.Count;

            for (int i = 0; i < wireCount; i++)
            {
                Wire wire = lc.Wires[i];

                Button tb = new Button()
                {
                    Content = tracedWireIndexes[i].ToString()
                };

                tb.SetStandartAlignment();
                tb.SetSizeAndCenter(traceTextSize, wire.Center);

                grid.Children.Add(tb);
            };
        }

        public void ShowAll(ref Scheme scheme)
        {
            ShowDots(ref scheme.dotController);
            ShowGates(ref scheme.gateController);
            ShowLines(ref scheme.lineController);
        }

        #region Button event handlers 

        private void LogicGateBodyTapped(Gate gate, Button button) => LogicGateTappedEvent(button);

        private void GateINBodyTapped(Gate gate, Button button) => GateINTapped(button);

        private void GateOUTBodyTapped(Gate gate, Button button) => GateOUTTapped(button);
        
        #endregion

        #region Ellipse event handlers
        public void DotTapped(object sender, TappedRoutedEventArgs e) =>
            DotTappedEvent(sender as Ellipse);
        private void GateOutTapped(Port port) => PortTapped(port);
        private void GateInTapped(Port port) => PortTapped(port);
        #endregion

        #region Events

        public event Action<Button> LogicGateTappedEvent,
            GateINTapped, GateOUTTapped;

        public event Action<Ellipse> DotTappedEvent;
        public event Action<Port> PortTapped;

        public event Action<Line> LineTappedEvent;

        #endregion

        readonly Grid grid;
    }
}