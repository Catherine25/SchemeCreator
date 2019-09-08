using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using SchemeCreator.Data.ConstantsNamespace;
using SchemeCreator.Data.Extensions;

namespace SchemeCreator.UI
{
    class WorkspaceController : IFrameInterface
    {
        /*      methods      */
        public void SetParentGrid(Grid parentGrid) =>
            parentGrid.Children.Add(grid);

        public void Hide()
        {
            grid.Children.Clear();
            isActive = false;
        }

        public void Update(Size size) => grid.SetSize(size);

        public void ShowDots(ref Data.DotController dotController)
        {
            dotController.InitNet(grid.GetActualSize());

            int dotCount = dotController.Dots.Count;

            for (int i = 0; i < dotCount; i++)
            {
                Ellipse e = dotController.Dots[i];
                grid.Children.Add(e);

                e.Tapped += DotTapped;
                e.PointerEntered += E_PointerEntered;
                e.PointerExited += E_PointerExited;
            }

            isActive = true;
        }

        public void ShowGates(ref Data.GateController gateController)
        {
            var logicGates = gateController.GetLogicGates();

            foreach (Data.Gate gate in logicGates)
            {
                var rect = gate.DrawBody();
                rect.Tapped += LogicGateBodyTapped;
                grid.Children.Add(rect);

                foreach (Ellipse e in gate.DrawGateInPorts())
                {
                    grid.Children.Add(e);
                    e.Tapped += GateInTapped;
                    e.PointerEntered += E_PointerEntered;
                    e.PointerExited += E_PointerExited;
                }


                foreach (Ellipse e in gate.DrawGateOutPorts())
                {
                    grid.Children.Add(e);
                    e.Tapped += GateOutTapped;
                    e.PointerEntered += E_PointerEntered;
                    e.PointerExited += E_PointerExited;
                }
            }

            var externalGates = gateController.GetExternalGates();

            foreach (Data.Gate gate in externalGates)
            {
                var gateBody = gate.DrawBody();

                if (gate.type == Constants.GateEnum.IN)
                    gateBody.Tapped += GateINBodyTapped;
                else gateBody.Tapped += GateOUTBodyTapped;

                grid.Children.Add(gateBody);
            }
        }

        private void E_PointerExited(object sender, PointerRoutedEventArgs e) => (sender as Ellipse).DecreaseSize();

        private void E_PointerEntered(object sender, PointerRoutedEventArgs e) => (sender as Ellipse).IncreaseSize();

        public void ShowLines(ref Data.LineController lineController)
        {
            for (int i = 0; i < lineController.Wires.Count; i++)
                grid.Children.Add(lineController.Wires[i].
                CreateLine(lineController.Wires[i].isActive));
        }

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

        /*  -----   Button event handlers   -----   */
        //private void ExternalGateBodyTapped(object sender, TappedRoutedEventArgs e) =>
        //    ExternalGateTapped(sender as Button);
        private void LogicGateBodyTapped(object sender, TappedRoutedEventArgs e) =>
            LogicGateTappedEvent(sender as Button);
        private void GateINBodyTapped(object sender, TappedRoutedEventArgs e) =>
            GateINTapped(sender as Button);
        private void GateOUTBodyTapped(object sender, TappedRoutedEventArgs e) =>
            GateOUTTapped(sender as Button);
        /*  -----   Button event handlers   -----   */


        /*   -----   Ellipse event handlers   -----   */
        public void DotTapped(object sender, TappedRoutedEventArgs e) =>
            DotTappedEvent(sender as Ellipse);
        private void GateOutTapped(object sender, TappedRoutedEventArgs e) =>
            LogicGateOutTapped(sender as Ellipse);
        private void GateInTapped(object sender, TappedRoutedEventArgs e) =>
            LogicGateInTapped(sender as Ellipse);
        /*   -----   Ellipse event handlers   -----   */

        /*   -----   events   -----   */

        public event Action<Button> LogicGateTappedEvent, ExternalGateTapped,
            GateINTapped, GateOUTTapped;

        public event Action<Ellipse> DotTappedEvent, LogicGateInTapped,
            LogicGateOutTapped;

        /*   -----   events   -----   */

        /*      data        */
        public bool IsActive { get => isActive; }

        bool isActive;
        readonly Grid grid = new Grid()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 60, 0, 0)
        };
    }
}