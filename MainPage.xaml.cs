using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator {
    public sealed partial class MainPage : Page {

        Data.Scheme scheme = new Data.Scheme();
        public MainPage() {

            InitializeComponent();

            var test = new Test.Test();

            Content = scheme.frameManager.Grid;
            SizeChanged += MainPageSizeChanged;
        }

        private void MainPageSizeChanged(object sender, SizeChangedEventArgs e) {
            
            scheme.frameManager.Grid.Height = ActualHeight;
            scheme.frameManager.Grid.Width = ActualWidth;
            
            scheme.frameManager.SizeChanged(ActualWidth, ActualHeight);
        }
    }
}