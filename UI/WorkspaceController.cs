using System;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.UI {
    class WorkspaceController {
        /*      constructor     */
        public WorkspaceController(Grid _grid) => _grid.Children.Add(grid);
        public delegate void DotTappedHandler(Ellipse sender, DotTappedEventArgs e);
        public event DotTappedHandler DotTappedEvent;
        //static SchemeCreator.Data.Scheme scheme; 
        
        /*      public methods      */
        public void UpdateView(double height, double width) {
            grid.Height = height;
            grid.Width = width;
        }
        public void ReloadDots(SchemeCreator.Data.Scheme scheme) {
            scheme.dotController.InitNet(grid.Width, grid.Height);
            foreach(Ellipse e in scheme.dotController.dots) {
                grid.Children.Add(e);
                e.Tapped += DotTapped;
            }
        }
        /*      handlers        */
        public void DotTapped (object sender, TappedRoutedEventArgs e) {
            if(DotTappedEvent != null)
                DotTappedEvent(sender as Ellipse, new DotTappedEventArgs(sender as Ellipse));
        }
        public void Dispose() => grid.Children.Clear();
        /*      data        */
        Grid grid = new Grid() { Margin = new Windows.UI.Xaml.Thickness(0, 60, 0, 0) };
    }
    class DotTappedEventArgs {
        public DotTappedEventArgs(Ellipse e) {
            if(e != dot)
                dot = e;
        }
        public Ellipse dot;
    }
}
