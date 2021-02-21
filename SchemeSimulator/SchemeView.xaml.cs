using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchemeSimulator
{
    public partial class SchemeView : UserControl
    {
        public const int ColumnsCount = 8;
        public const int RowsCount = 8;

        public SchemeView()
        {
            InitializeComponent();

            WireBuilder.WireReady += WireReady;

            for (int i = 0; i < ColumnsCount; i++)
                Grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int j = 0; j < RowsCount; j++)
                Grid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < ColumnsCount; i++)
                for (int j = 0; j < RowsCount; j++)
                {
                    var controlPlaceHolder = new ControlPlaceHolder();
                    Grid.SetColumn(controlPlaceHolder, i);
                    Grid.SetRow(controlPlaceHolder, j);
                    controlPlaceHolder.MouseDown += ControlPlaceHolder_MouseDown;
                    Grid.Children.Add(controlPlaceHolder);
                }
        }

        private void WireReady(WireView wireView)
        {
            Grid.SetColumnSpan(wireView, ColumnsCount);
            Grid.SetRowSpan(wireView, RowsCount);
            Grid.Children.Add(wireView);
        }

        private void ControlPlaceHolder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var control = sender as ControlPlaceHolder;

            var window = new NewGateDialog();
            if (window.ShowDialog() == true)
            {
                var port = window.ExternalPort;
                Grid.SetColumn(port, Grid.GetColumn(control));
                Grid.SetRow(port, Grid.GetRow(control));
                port.MouseDown += Port_MouseDown;
                Grid.Children.Add(port);
            }
        }

        private void Port_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var port = sender as ExternalPort;
            
            // use half of a column as margin before the line to get line centered
            double width = Grid.ColumnDefinitions[0].ActualWidth;
            double height = Grid.RowDefinitions[0].ActualHeight;
            Point point = port.TranslatePoint(new Point(width / 2, height / 2), Grid);
            
            WireBuilder.AddPoint(point, port.IsInput);
        }
    }
}
