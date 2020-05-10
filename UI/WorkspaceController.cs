﻿using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data.ConstantsNamespace;
using SchemeCreator.Data.Extensions;
using System.Collections.Generic;
using SchemeCreator.Data.Model;

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
            Button body = gate.DrawBody();
            grid.Children.Add(body);

            if (Constants.external.Contains(gate.type))
            {
                if (gate.type == Constants.GateEnum.IN)
                    body.Tapped += GateINBodyTapped;
                else
                    body.Tapped += GateOUTBodyTapped;
                return;
            }
            else
                body.Tapped += LogicGateBodyTapped;

            gate.DrawGateInPorts().ForEach(e =>
            {
                e.Tapped += GateInTapped;
                grid.Children.Add(e);
            });

            gate.DrawGateOutPorts().ForEach(e =>
            {
                e.Tapped += GateOutTapped;
                grid.Children.Add(e);
            });
        }
        public void RemoveLine(Line line) => grid.Children.Remove(line);
        public void SetParentGrid(Grid parentGrid) => parentGrid.Children.Add(grid);
        public void Hide() => grid.Children.Clear();
        public void Update(Rect rect) => grid.SetRect(rect);

        public void ShowDots(ref Data.DotController dotController)
        {
            dotController.InitNet(grid.GetActualSize());

            dotController.Dots.ForEach(dot =>
            {
                grid.Children.Add(dot);
                dot.Tapped += DotTapped;
                dot.PointerEntered += E_PointerEntered;
                dot.PointerExited += E_PointerExited;
            });
        }

        public void ShowGates(ref Data.GateController gateController)
        {
            IEnumerable<Data.Gate> logicGates = gateController.GetLogicGates();

            foreach (Data.Gate gate in logicGates)
            {
                Button rect = gate.DrawBody();
                rect.Tapped += LogicGateBodyTapped;
                grid.Children.Add(rect);

                foreach (Ellipse e in gate.DrawGateInPorts())
                {
                    grid.Children.Add(e);
                    e.Tapped += GateInTapped;
                }

                foreach (Ellipse e in gate.DrawGateOutPorts())
                {
                    grid.Children.Add(e);
                    e.Tapped += GateOutTapped;
                }
            }

            List<Data.Gate> externalGates = new List<Data.Gate>(gateController.GetExternalGates());
            foreach(Data.Gate gate in externalGates)
            {
                Button body = gate.DrawBody();

                if (gate.type == Constants.GateEnum.IN)
                    body.Tapped += GateINBodyTapped;
                else
                    body.Tapped += GateOUTBodyTapped;

                grid.Children.Add(body);
            }
        }

        public void ShowLines(ref Data.LineController lineController)
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

        public void ShowWireTraceIndexes(int[] tracedWireIndexes, Data.LineController lc)
        {
            int wireCount = lc.Wires.Count;

            for (int i = 0; i < wireCount; i++)
            {
                Data.Wire wire = lc.Wires[i];

                Button tb = new Button()
                {
                    Content = tracedWireIndexes[i].ToString()
                };

                tb.SetStandartAlignment();
                tb.SetSizeAndCenter(Constants.traceTextSize, wire.Center);

                grid.Children.Add(tb);
            };
        }

        public void ShowAll(ref Data.Scheme scheme)
        {
            ShowDots(ref scheme.dotController);
            ShowGates(ref scheme.gateController);
            ShowLines(ref scheme.lineController);
        }

        #region Button event handlers 
        private void LogicGateBodyTapped(object sender, TappedRoutedEventArgs e) =>
            LogicGateTappedEvent(sender as Button);
        private void GateINBodyTapped(object sender, TappedRoutedEventArgs e) =>
            GateINTapped(sender as Button);
        private void GateOUTBodyTapped(object sender, TappedRoutedEventArgs e) =>
            GateOUTTapped(sender as Button);
        #endregion

        #region Ellipse event handlers
        public void DotTapped(object sender, TappedRoutedEventArgs e) =>
            DotTappedEvent(sender as Ellipse);
        private void GateOutTapped(object sender, TappedRoutedEventArgs e) =>
            LogicGateOutTapped(sender as Ellipse);
        private void GateInTapped(object sender, TappedRoutedEventArgs e) =>
            LogicGateInTapped(sender as Ellipse);
        /*   -----   Ellipse event handlers   -----   */
        #endregion

        #region Events

        public event Action<Button> LogicGateTappedEvent,
            GateINTapped, GateOUTTapped;

        public event Action<Ellipse> DotTappedEvent, LogicGateInTapped,
            LogicGateOutTapped;

        public event Action<Line> LineTappedEvent;

        #endregion

        readonly Grid grid;
    }
}