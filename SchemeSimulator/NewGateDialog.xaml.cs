using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SchemeSimulator
{
    public partial class NewGateDialog : Window
    {
        public NewGateDialog()
        {
            InitializeComponent();

            ItemsList.SelectionChanged += (sender, e) => ValidateAndAssign();
            Inputs.SelectionChanged += (sender, e) => ValidateAndAssign();
            Outputs.SelectionChanged += (sender, e) => ValidateAndAssign();

            OkBt.Click += OkBt_Click;
            CloseBt.Click += CloseBt_Click;
            Closing += NewGateDialog_Closing;

            possibleCombinations = new List<(string, int, int)?>
            {
                ("In", 1, 1),
                ("Out", 1, 1),
                ("Buffer", 1, 1),
                ("Not", 1, 1)
            };

            for (int i = 2; i <= 8; i++)
            {
                possibleCombinations.Add(("And", i, 1));
                possibleCombinations.Add(("Nand", i, 1));
                possibleCombinations.Add(("Or", i, 1));
                possibleCombinations.Add(("Nor", i, 1));
                possibleCombinations.Add(("Xor", i, 1));
                possibleCombinations.Add(("Xnor", i, 1));
            }

            ValidateAndAssign();
        }

        public ExternalPort ExternalPort;
        public GateView GateView;
        private string _selectedItem;
        private int _inputs;
        private int _outputs;
        private List<(string, int, int)?> possibleCombinations;

        private void NewGateDialog_Closing(object sender, CancelEventArgs e)
        {
            if (_selectedItem == "In")
                ExternalPort = new ExternalPort(true);
            else if (_selectedItem == "Out")
                ExternalPort = new ExternalPort(false);
            else
                GateView = new GateView((GateType)Enum.Parse(typeof(GateType), _selectedItem), _inputs, _outputs);
        }

        private void CloseBt_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OkBt_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private (string, int, int)? Validate()
        {
            string type = GetStringFromComboBox(ItemsList);
            int inputs = int.Parse(GetStringFromComboBox(Inputs));
            int outputs = int.Parse(GetStringFromComboBox(Outputs));
            
            var gates = possibleCombinations.Where(x => x.Value.Item1 == type);
            
            var gatesWithPorts = gates.Where(x => x.Value.Item2 == inputs || x.Value.Item3 == outputs);
            if (gatesWithPorts.Count() == 0)
                return gates.ToList()[0];
            
            var gate = gatesWithPorts.FirstOrDefault(x => x.Value.Item1 == type && x.Value.Item2 == inputs || x.Value.Item3 == outputs);

            if (gate == null)
                return gatesWithPorts.First();
            else return gate;
        }

        private void ValidateAndAssign()
        {
            var gate = Validate();

            _selectedItem = gate.Value.Item1;
            _inputs = gate.Value.Item2;
            _outputs = gate.Value.Item3;
            
            ItemsList.SelectedItem = _selectedItem;
            Inputs.SelectedItem = _inputs;
            Outputs.SelectedItem = _outputs;
        }

        private string GetStringFromComboBox(ComboBox s) => s.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
    }
}
