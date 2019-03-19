using System;
using Windows.UI.Input;
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
        /*      events        */
        public delegate void DotTappedHandler(Ellipse sender, DotTappedEventArgs e);
        public event DotTappedHandler DotTappedEvent;
        public void DotTapped(object sender, TappedRoutedEventArgs e) =>
            DotTappedEvent(sender as Ellipse, new DotTappedEventArgs(sender as Ellipse));

        /*      data        */
        public bool IsActive { get => isActive; }
        bool isActive;
        Grid grid = new Grid() { Margin = new Windows.UI.Xaml.Thickness(0, 60, 0, 0) };
    }

    class DotTappedEventArgs {
        public DotTappedEventArgs(Ellipse e) => dot = e;
        public Ellipse dot;
    }
}
