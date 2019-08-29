using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator
{
    public sealed partial class MainPage : Page
    {
        private Data.Scheme scheme = new Data.Scheme();

        public MainPage()
        {
            InitializeComponent();

            Content = scheme.frameManager.Grid;
            SizeChanged += MainPageSizeChanged;
        }

        private void MainPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            scheme.frameManager.Grid.Height = ActualHeight;
            scheme.frameManager.Grid.Width = ActualWidth;

            Size size = new Size
            {
                Width = ActualWidth,
                Height = ActualHeight
            };

            scheme.frameManager.SizeChanged(size);
        }
    }
}