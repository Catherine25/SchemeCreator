using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SchemeCreator.UI
{
    public sealed partial class NewGateDialog : ContentDialog
    {
        public GateEnum? gateType = GateEnum.And;
        public int inputs = 1;
        public int outputs = 1;
        public bool? isExternalInput;

        public GateView Gate;
        public ExternalPortView ExternalPort;

        public NewGateDialog()
        {
            InitializeComponent();

            // add external ports
            gateComboBox.Items.Add(PortType.Input);
            gateComboBox.Items.Add(PortType.Output);
            
            // add gate types
            gateComboBox.Items.Add(GateEnum.And);
            gateComboBox.Items.Add(GateEnum.Buffer);
            gateComboBox.Items.Add(GateEnum.Nand);
            gateComboBox.Items.Add(GateEnum.Nor);
            gateComboBox.Items.Add(GateEnum.Not);
            gateComboBox.Items.Add(GateEnum.Or);
            gateComboBox.Items.Add(GateEnum.Xnor);
            gateComboBox.Items.Add(GateEnum.Xor);

            // add inputs count
            inputsComboBox.Items.Add(1);
            inputsComboBox.Items.Add(2);
            inputsComboBox.Items.Add(3);
            inputsComboBox.Items.Add(4);
            inputsComboBox.Items.Add(6);
            inputsComboBox.Items.Add(8);
            
            // add outputs count
            outputsComboBox.Items.Add(1);

            PreselectGate();

            // subscribe
            gateComboBox.SelectionChanged += Gate_SelectionChanged;
            inputsComboBox.SelectionChanged += Inputs_SelectionChanged;
            outputsComboBox.SelectionChanged += Outputs_SelectionChanged;

            Closing += NewGateDialog_Closing;
        }

        private void NewGateDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (gateComboBox.SelectedValue == null)
                return;

            GateEnum? gate = GetSelectedGateType();
            PortType? port = GetSelectedPortType();

            if (gate != null)
                Gate = new GateView(gate.Value, new Vector2(), inputs, outputs);
            else if(port != null)
                ExternalPort = new ExternalPortView(port.Value, new Vector2());
        }

        private void PreselectGate()
        {
            gateComboBox.SelectedItem = PortType.Input;
            inputsComboBox.SelectedItem = 1;
            outputsComboBox.SelectedItem = 1;
        }

        private void Outputs_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            outputs = (int)outputsComboBox.SelectedValue;

        private void Inputs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inputs = (int)inputsComboBox.SelectedValue;

            if (SingleInput.Contains(gateType.Value))
                inputsComboBox.SelectedValue = 1;
            else if ((int)inputsComboBox.SelectedValue == 1)
                inputsComboBox.SelectedValue = 2;
        }

        private void Gate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Enum.TryParse(typeof(GateEnum), gateComboBox.SelectedValue.ToString(), out var type))
            {
                gateType = type as GateEnum?;
                isExternalInput = false;

                if (SingleInput.Contains(gateType.Value))
                    inputsComboBox.SelectedValue = 1;
                else if ((int)inputsComboBox.SelectedValue == 1)
                    inputsComboBox.SelectedValue = 2;
            }
            else
            {
                isExternalInput = gateComboBox.Text == "Input" ? true : false;

                inputsComboBox.SelectedValue = 1;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) =>
            gateType = null;

        public GateEnum? GetSelectedGateType()
        {
            return Enum.TryParse(typeof(GateEnum), gateComboBox.SelectedValue.ToString(), out var type)
                ? (GateEnum?)(GateEnum)type
                : null;
        }

        public PortType? GetSelectedPortType()
        {
            return Enum.TryParse(typeof(PortType), gateComboBox.SelectedValue.ToString(), out var type)
                ? (PortType?)(PortType)type
                : null;
        }
        
        public static readonly SortedSet<GateEnum> SingleInput = new()
        {
            GateEnum.Buffer,
            GateEnum.Not,
        };
        
        public static readonly Dictionary<GateEnum, string> GateNames = new()
        {
            { GateEnum.And, "AND" },
            { GateEnum.Buffer, "Buffer" },
            { GateEnum.Nand, "NAND" },
            { GateEnum.Nor, "NOR" },
            { GateEnum.Not, "NOT" },
            { GateEnum.Or, "OR" },
            { GateEnum.Xnor, "XNOR" },
            { GateEnum.Xor, "XOR" }
        };
    }
}
