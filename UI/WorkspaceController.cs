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

        public void ShowDots(ref Data.Scheme scheme) {
            scheme.dotController.InitNet(grid.ActualWidth, grid.ActualHeight);
            foreach(Ellipse e in scheme.dotController.dots) {
                grid.Children.Add(e);
                e.Tapped += DotTapped;
            }
            isActive = true;
        }

        public void ShowGates(ref Data.Scheme scheme) {

            foreach(Data.Gate gate in scheme.gateController.gates) {
                grid.Children.Add(gate.DrawBody());

                foreach(Ellipse e in gate.DrawGateInputs())
                    grid.Children.Add(e);

                foreach(Ellipse e in gate.DrawGateOutputs())
                    grid.Children.Add(e);
            }
        }

        public void ShowLines(ref Data.Scheme scheme) {

        }

        public void ShowAll(ref Data.Scheme scheme) {
            ShowDots(ref scheme);
            ShowGates(ref scheme);
            ShowLines(ref scheme);
        }

        public void ReloadGates(SchemeCreator.Data.Scheme scheme) {

            foreach(Data.Gate gate in scheme.gateController.gates) {
                grid.Children.Add(gate.DrawBody());
                
                foreach (Ellipse ellipse in gate.DrawGateInputs())
                    grid.Children.Add(ellipse);
                
                foreach (Ellipse ellipse in gate.DrawGateOutputs())
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