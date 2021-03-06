using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchemeSimulator
{
    public enum GateType
    {
        Buffer,
        Not,
        And,
        Nand,
        Or,
        Nor,
        Xor,
        Xnor
    }

    public partial class GateView : UserControl
    {
        public GateView() => InitializeComponent();

        public GateView(GateType type, int inputs, int outputs)
        {
            InitializeComponent();

            GateType = type;
            Rectangle.Text = type.ToString();

            for (int i = 0; i < inputs; i++)
                CreatePort(Inputs, true);

            for (int o = 0; o < outputs; o++)
                CreatePort(Outputs, false);
        }

        public readonly GateType GateType;
        public Action<GateView, GatePortView> PortClicked;

        private void CreatePort(Grid grid, bool isInput)
        {
            var port = new GatePortView(isInput);
            port.MouseDown += Port_MouseDown;
            Grid.SetRow(port, grid.RowDefinitions.Count);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.Children.Add(port);
        }

        private void Port_MouseDown(object sender, MouseButtonEventArgs e) => PortClicked(this, sender as GatePortView);
    }
}
