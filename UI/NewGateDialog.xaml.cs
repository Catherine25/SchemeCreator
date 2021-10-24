using SchemeCreator.UI.Dynamic;
using System;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI
{
    public sealed partial class NewGateDialog : ContentDialog
    {
        public GateView? Gate;
        private GateEnum gateType = GateEnum.Buffer;
        private int inputs = 1;
        private int outputs = 1;
        
        public NewGateDialog()
        {
            InitializeComponent();

            // add gate types
            GateComboBox.Items.Add(GateEnum.And);
            GateComboBox.Items.Add(GateEnum.Buffer);
            GateComboBox.Items.Add(GateEnum.Nand);
            GateComboBox.Items.Add(GateEnum.Nor);
            GateComboBox.Items.Add(GateEnum.Not);
            GateComboBox.Items.Add(GateEnum.Or);
            GateComboBox.Items.Add(GateEnum.Xnor);
            GateComboBox.Items.Add(GateEnum.Xor);
            // todo fix
            // GateComboBox.Items.Add(GateEnum.Custom);

            // add inputs count
            InputsComboBox.Items.Add(1);
            InputsComboBox.Items.Add(2);
            InputsComboBox.Items.Add(3);
            InputsComboBox.Items.Add(4);
            
            // add outputs count
            OutputsComboBox.Items.Add(1);
            
            // preselect gate on view
            Preselect();
            
            // subscribe
            GateComboBox.SelectionChanged += SelectionChanged;
            InputsComboBox.SelectionChanged += SelectionChanged;
            OutputsComboBox.SelectionChanged += SelectionChanged;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inputs = (int)InputsComboBox.SelectedValue;
            outputs = (int) OutputsComboBox.SelectedValue;
            gateType = Enum.Parse<GateEnum>(GateComboBox.SelectedValue.ToString());

            AdjustGate();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) =>
            Gate = new GateView(gateType, new Vector2(), inputs, outputs);

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) =>
            Gate = null;
        
        /// <summary>
        /// Preselects Buffer to have gate always selected.
        /// </summary>
        private void Preselect()
        {
            GateComboBox.SelectedIndex = 1;
            InputsComboBox.SelectedIndex = 0;
            OutputsComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Adjusts gate to match standard gates configurations.
        /// </summary>
        private void AdjustGate()
        {
            // don't validate custom gate
            if (gateType == GateEnum.Custom)
                return;
            
            // restrict gate input ports count
            if (gateType == GateEnum.Buffer || gateType == GateEnum.Not)
                InputsComboBox.SelectedValue = 1;
            else if ((int)InputsComboBox.SelectedValue == 1)
                InputsComboBox.SelectedValue = 2;
        }
    }
}
