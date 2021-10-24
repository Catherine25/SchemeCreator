using SchemeCreator.UI.Dynamic;
using System;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI
{
    public sealed partial class NewExternalPortDialog : ContentDialog
    {
        public ExternalPortView? ExternalPortView;
        private PortType SelectedPortType;

        public NewExternalPortDialog()
        {
            InitializeComponent();

            // add external ports
            PortComboBox.Items.Add(PortType.Input);
            PortComboBox.Items.Add(PortType.Output);
            
            Preselect();
        }

        private void Preselect() => PortComboBox.SelectedIndex = 0;

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            SelectedPortType = Enum.Parse<PortType>(PortComboBox.SelectedValue.ToString());
            ExternalPortView = new ExternalPortView(SelectedPortType, new Vector2(3, 3));
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) =>
            ExternalPortView = null;
    }
}
