using System.Linq;
using Windows.UI.Xaml.Controls;
using static SchemeCreator.Data.Constants;
// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SchemeCreator.UI
{
    public sealed partial class NewGateDialog : ContentDialog
    {
        public GateEnum? gateType = GateEnum.IN;
        public int inputs = 1;
        public int outputs = 1;

        public NewGateDialog()
        {
            InitializeComponent();

            gateComboBox.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            gateComboBox.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            
            gateComboBox.Items.Add(GateEnum.AND);
            gateComboBox.Items.Add(GateEnum.Buffer);
            gateComboBox.Items.Add(GateEnum.IN);
            gateComboBox.Items.Add(GateEnum.NAND);
            gateComboBox.Items.Add(GateEnum.NOR);
            gateComboBox.Items.Add(GateEnum.NOT);
            gateComboBox.Items.Add(GateEnum.OR);
            gateComboBox.Items.Add(GateEnum.OUT);
            gateComboBox.Items.Add(GateEnum.XNOR);
            gateComboBox.Items.Add(GateEnum.XOR);

            gateComboBox.SelectedItem = GateEnum.IN;

            gateComboBox.SelectionChanged += Gate_SelectionChanged;

            inputsComboBox.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            inputsComboBox.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;

            inputsComboBox.Items.Add(1);
            inputsComboBox.Items.Add(2);
            inputsComboBox.Items.Add(3);
            inputsComboBox.Items.Add(4);
            inputsComboBox.Items.Add(6);
            inputsComboBox.Items.Add(8);

            inputsComboBox.SelectedItem = 1;

            inputsComboBox.SelectionChanged += Inputs_SelectionChanged;

            outputsComboBox.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
            outputsComboBox.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;

            outputsComboBox.Items.Add(1);
            outputsComboBox.SelectedItem = 1;
            outputsComboBox.SelectionChanged += Outputs_SelectionChanged;
        }

        private void Outputs_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            outputs = (int)outputsComboBox.SelectedValue;

        private void Inputs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inputs = (int)inputsComboBox.SelectedValue;

            if (singleInput.Contains(gateType.Value))
                inputsComboBox.SelectedValue = 1;
            else if ((int)inputsComboBox.SelectedValue == 1)
                inputsComboBox.SelectedValue = 2;
        }

        private void Gate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gateType = (GateEnum)gateComboBox.SelectedValue;

            if (singleInput.Contains(gateType.Value))
                inputsComboBox.SelectedValue = 1;
            else if ((int)inputsComboBox.SelectedValue == 1)
                inputsComboBox.SelectedValue = 2;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) =>
            gateType = null;
    }
}
