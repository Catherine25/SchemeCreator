using SchemeCreator.Data.Models;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator
{
    public sealed partial class MainPage : Page
    {
        private Scheme scheme;

        public MainPage()
        {
            InitializeComponent();

            scheme = new Scheme();

            scheme.frameManager.Grid.AddToParent(this);
            SizeChanged += MainPageSizeChanged;
        }

        private void MainPageSizeChanged(object sender, SizeChangedEventArgs e) =>
            scheme.frameManager.SizeChanged(new Rect(new Point(0,0), e.NewSize));
    }
}