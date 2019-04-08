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

            dotController.InitNet(grid.ActualWidth, grid.ActualHeight);

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

                foreach(Ellipse e in gate.DrawGateInOut(true))
                    grid.Children.Add(e);

                foreach(Ellipse e in gate.DrawGateInOut(false))
                    grid.Children.Add(e);
            }

            var externalGates = gateController.getExternalGates();

            foreach(Data.Gate gate in externalGates) {
                grid.Children.Add(gate.DrawBody());
            }
        }

        public void ShowLines(ref Data.LineController scheme) { }

        public void ShowAll(ref Data.Scheme scheme) {

            ShowDots(ref scheme.dotController);
            ShowGates(ref scheme.gateController);
            ShowLines(ref scheme.lineController);
        }

        public void ReloadGates(Data.Scheme scheme) {

            for(int i = 0; i < scheme.gateController.getGateCount(); i++) {

                var gate = scheme.gateController.getGateByIndex(i);
                grid.Children.Add(gate.DrawBody());

                foreach (Ellipse ellipse in gate.DrawGateInOut(true))
                    grid.Children.Add(ellipse);

                foreach (Ellipse ellipse in gate.DrawGateInOut(false))
                    grid.Children.Add(ellipse);
            }
        }

        /*      events        */

        public delegate void DotTappedHandler(Ellipse sender, DotTappedEventArgs e);

        public event DotTappedHandler DotTappedEvent;

        public void DotTapped(object sender, TappedRoutedEventArgs e) =>
            DotTappedEvent(sender as Ellipse, new DotTappedEventArgs(sender as Ellipse));

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
}