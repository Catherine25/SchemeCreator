using System;
using System.ComponentModel;
using System.Windows;

namespace SchemeSimulator
{
    public partial class NewGateDialog : Window
    {
        public NewGateDialog()
        {
            InitializeComponent();

            ItemsList.SelectionChanged += (sender, e) => _selectedItem = ItemsList.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");

            OkBt.Click += OkBt_Click;
            CloseBt.Click += CloseBt_Click;
            
            Closing += NewGateDialog_Closing;
        }

        public ExternalPort ExternalPort;
        private string _selectedItem;

        private void NewGateDialog_Closing(object sender, CancelEventArgs e)
        {
            if (_selectedItem == "In")
            {
                ExternalPort = new ExternalPort(true);
                DialogResult = true;
            }
            else if (_selectedItem == "Out")
            {
                ExternalPort = new ExternalPort(false);
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
                Close();
            }
        }

        private void CloseBt_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OkBt_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem == "In")
            {
                ExternalPort = new ExternalPort(true);
            }
            else if (_selectedItem == "Out")
            {
                ExternalPort = new ExternalPort(false);
            }

            DialogResult = true;
            Close();
        }
    }
}
