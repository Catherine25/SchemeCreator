using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.UI {
    class WorkspaceController : IFrameInterface {

        /*      methods      */
        public void SetParentGrid(Grid parentGrid) => parentGrid.Children.Add(grid);

        public void Hide() {
            
            grid.Children.Clear();
            isActive = false;
        }

        public void Update(double width, double height) {

            grid.Height = height;
            grid.Width = width;
        }

        public void ShowDots(ref Data.DotController dotController) {

            dotController.initNet(grid.ActualWidth, grid.ActualHeight);

            for(int i = 0; i < dotController.getDotCount(); i++) {
                
                Ellipse e = dotController.getDotByIndex(i);
                grid.Children.Add(e);
                e.Tapped += dotTapped;
            }

            isActive = true;
        }

        public void ShowGates(ref Data.GateController gateController) {

            var logicGates = gateController.getLogicGates();
            
            foreach(Data.Gate gate in logicGates) {
                var rect = gate.DrawBody();
                rect.Tapped += logicGateBodyTapped;
                grid.Children.Add(rect);

                foreach(Ellipse e in gate.DrawGateInOut(true)) {
                    grid.Children.Add(e);
                    e.Tapped += gateInTapped;
                }
                    

                foreach(Ellipse e in gate.DrawGateInOut(false)) {
                    grid.Children.Add(e);
                    e.Tapped += gateOutTapped;
                }
            }

            var externalGates = gateController.getExternalGates();

            foreach(Data.Gate gate in externalGates) {

                var gateBody = gate.DrawBody();

                if(gate.type == Constants.GateEnum.IN)
                    gateBody.Tapped += gateINBodyTapped;
                else gateBody.Tapped += gateOUTBodyTapped;
                
                grid.Children.Add(gateBody);
            }
        }

        /* Button event handlers */
        private void externalGateBodyTapped(object sender, TappedRoutedEventArgs e) =>
            externalGateTapped(sender as Button);
        private void logicGateBodyTapped(object sender, TappedRoutedEventArgs e) => 
            logicGateTappedEvent(sender as Button);
        private void gateINBodyTapped(object sender, TappedRoutedEventArgs e) =>
            gateINTapped(sender as Button);
        private void gateOUTBodyTapped(object sender, TappedRoutedEventArgs e) =>
            gateOUTTapped(sender as Button);

        /* Ellipse event handlers */
        public void dotTapped(object sender, TappedRoutedEventArgs e) =>
            dotTappedEvent(sender as Ellipse);
        private void gateOutTapped(object sender, TappedRoutedEventArgs e) =>
            logicGateOutTapped(sender as Ellipse);
        private void gateInTapped(object sender, TappedRoutedEventArgs e) =>
            logicGateInTapped(sender as Ellipse);

        public void ShowLines(ref Data.LineController lineController) {

            for(int i = 0; i < lineController.getWireCount(); i++)
                grid.Children.Add(lineController.getWireByIndex(i).
                createLine(lineController.getWireByIndex(i).isActive));
        }

        public void ShowAll(ref Data.Scheme scheme) {

            ShowDots(ref scheme.dotController);
            ShowGates(ref scheme.gateController);
            ShowLines(ref scheme.lineController);
        }

        /*      events        */

        public event Action<Button> logicGateTappedEvent,
            externalGateTapped,
            gateINTapped,
            gateOUTTapped;
        public event Action<Ellipse> dotTappedEvent,
            logicGateInTapped,
            logicGateOutTapped;

        
        /*      data        */

        public bool IsActive { get => isActive; }

        bool isActive;
        
        Grid grid = new Grid() {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Windows.UI.Xaml.Thickness(0, 60, 0, 0) };
    }
}