using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Dialogs
{
    public sealed partial class AddElement : Page
    {
        public AddElement()
        {
            InitializeComponent();

            Buffer.Click += Buffer_Click;
            NOT.Click += NOT_Click;
            AND.Click += AND_Click;
            NAND.Click += NAND_Click;
            OR.Click += OR_Click;
            NOR.Click += NOR_Click;
            XOR.Click += XOR_Click;
            XNOR.Click += XNOR_Click;
            Input.Click += Input_Click;
            Output.Click += Output_Click;
            OK.Click += OK_Click;

            Input1.IsEnabled = false;
            Input2.IsEnabled = false;
            Input3.IsEnabled = false;
            Input4.IsEnabled = false;
            Input6.IsEnabled = false;
            Input8.IsEnabled = false;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (Input2.IsChecked == true)
                Data.Scheme.NewGateInputs = 2;
            if (Input3.IsChecked == true)
                Data.Scheme.NewGateInputs = 3;
            if (Input4.IsChecked == true)
                Data.Scheme.NewGateInputs = 4;
            if (Input6.IsChecked == true)
                Data.Scheme.NewGateInputs = 6;
            if (Input8.IsChecked == true)
                Data.Scheme.NewGateInputs = 8;

            if (Input2.IsChecked == true || Input3.IsChecked == true || Input4.IsChecked == true || Input6.IsChecked == true || Input8.IsChecked == true)
                Frame.Navigate(typeof(MainPage));
        }

        private void Buffer_Click(object sender, RoutedEventArgs e)
        {
            Data.Scheme.NewGateInputs = 1;
            Data.Scheme.NewElementName = "Buffer";
            Frame.Navigate(typeof(MainPage));
        }

        private void NOT_Click(object sender, RoutedEventArgs e)
        {
            Data.Scheme.NewElementName = "NOT";
            Data.Scheme.NewGateInputs = 1;
            Frame.Navigate(typeof(MainPage));
        }

        private void AND_Click(object sender, RoutedEventArgs e)
        {
            Input1.IsEnabled = false;
            Input2.IsEnabled = true;
            Input3.IsEnabled = true;
            Input4.IsEnabled = true;
            Input6.IsEnabled = true;
            Input8.IsEnabled = true;
            Data.Scheme.NewElementName = "AND";
        }

        private void NAND_Click(object sender, RoutedEventArgs e)
        {
            Input1.IsEnabled = false;
            Input2.IsEnabled = true;
            Input3.IsEnabled = true;
            Input4.IsEnabled = true;
            Input6.IsEnabled = true;
            Input8.IsEnabled = true;
            Data.Scheme.NewElementName = "NAND";
        }

        private void OR_Click(object sender, RoutedEventArgs e)
        {
            Input1.IsEnabled = false;
            Input2.IsEnabled = true;
            Input3.IsEnabled = true;
            Input4.IsEnabled = true;
            Input6.IsEnabled = true;
            Input8.IsEnabled = true;
            Data.Scheme.NewElementName = "OR";
        }

        private void NOR_Click(object sender, RoutedEventArgs e)
        {
            Input1.IsEnabled = false;
            Input2.IsEnabled = true;
            Input3.IsEnabled = true;
            Input4.IsEnabled = true;
            Input6.IsEnabled = true;
            Input8.IsEnabled = true;
            Data.Scheme.NewElementName = "NOR";
        }

        private void XOR_Click(object sender, RoutedEventArgs e)
        {
            Input1.IsEnabled = false;
            Input2.IsEnabled = true;
            Input3.IsEnabled = true;
            Input4.IsEnabled = true;
            Input6.IsEnabled = true;
            Input8.IsEnabled = true;
            Data.Scheme.NewElementName = "XOR";
        }

        private void XNOR_Click(object sender, RoutedEventArgs e)
        {
            Input1.IsEnabled = false;
            Input2.IsEnabled = true;
            Input3.IsEnabled = true;
            Input4.IsEnabled = true;
            Input6.IsEnabled = true;
            Input8.IsEnabled = true;
            Data.Scheme.NewElementName = "XNOR";
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {
            Data.Scheme.NewElementName = "IN";
            Data.Scheme.NewGateInputs = 1;
            Frame.Navigate(typeof(MainPage));
        }

        private void Output_Click(object sender, RoutedEventArgs e)
        {
            Data.Scheme.NewElementName = "OUT";
            Data.Scheme.NewGateInputs = 1;
            Frame.Navigate(typeof(MainPage));
        }
    }
}
