using System;
using System.Linq;
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
                e.Tapped += DotTapped;
            }

            isActive = true;
        }

        public void ShowGates(ref Data.GateController gateController) {

            var logicGates = gateController.getLogicGates();
            
            foreach(Data.Gate gate in logicGates) {
                grid.Children.Add(gate.DrawBody());

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

            foreach(Data.Gate gate in externalGates)
                grid.Children.Add(gate.DrawBody());

        }

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

        public delegate void DotTappedHandler(Ellipse sender, DotTappedEventArgs e);
        public delegate void GateInTappedHandler(Ellipse sender, GateInTappedEventArgs e);
        public delegate void GateOutTappedHandler(Ellipse sender, GateOutTappedEventArgs e);
        public event DotTappedHandler DotTappedEvent;
        public event GateInTappedHandler gateInTappedEvent;
        public event GateOutTappedHandler gateOutTappedEvent;

        public void DotTapped(object sender, TappedRoutedEventArgs e) =>
            DotTappedEvent(sender as Ellipse, new DotTappedEventArgs(sender as Ellipse));
        private void gateOutTapped(object sender, TappedRoutedEventArgs e) =>
            gateOutTappedEvent(sender as Ellipse, new GateOutTappedEventArgs(sender as Ellipse));
        private void gateInTapped(object sender, TappedRoutedEventArgs e) =>
            gateInTappedEvent(sender as Ellipse, new GateInTappedEventArgs(sender as Ellipse));

        /*      data        */

        public bool IsActive { get => isActive; }

        bool isActive;
        
        Grid grid = new Grid() {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Windows.UI.Xaml.Thickness(0, 60, 0, 0) };
    }

    class DotTappedEventArgs {
        public DotTappedEventArgs(Ellipse e) => dot = e;
        public Ellipse dot;
    }
    class GateInTappedEventArgs {
        public GateInTappedEventArgs(Ellipse e) => gateInput = e;
        public Ellipse gateInput;
    }
    class GateOutTappedEventArgs {
        public GateOutTappedEventArgs(Ellipse e) => gateOutput = e;
        public Ellipse gateOutput;
    }
}