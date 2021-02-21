using System.Windows.Controls;

namespace SchemeSimulator
{
    public partial class ExternalPort : UserControl
    {
        public bool IsInput => Text.Content.ToString() == "In";

        public ExternalPort() => InitializeComponent();

        public ExternalPort(bool isInput)
        {
            InitializeComponent();

            Text.MouseEnter += Ellipse_MouseEnter;
            Text.MouseLeave += Ellipse_MouseLeave;
            Ellipse.MouseEnter += Ellipse_MouseEnter;
            Ellipse.MouseLeave += Ellipse_MouseLeave;

            Text.Content = isInput ? "In" : "Out";
        }

        private void Ellipse_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Ellipse.Width /= 1.5;
            Ellipse.Height /= 1.5;
        }

        private void Ellipse_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Ellipse.Width *= 1.5;
            Ellipse.Height *= 1.5;
        }
    }
}
