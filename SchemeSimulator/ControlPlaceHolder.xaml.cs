using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SchemeSimulator
{
    public partial class ControlPlaceHolder : UserControl
    {
        public const int ControlHeight = 10;
        public const int ControlWidth = 10;

        public ControlPlaceHolder()
        {
            InitializeComponent();

            Ellipse.Height = ControlHeight;
            Ellipse.Width = ControlWidth;

            Ellipse.MouseEnter += Ellipse_MouseEnter;
            Ellipse.MouseLeave += Ellipse_MouseLeave;
        }

        private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            Ellipse.Width /= 2;
            Ellipse.Height /= 2;
        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            Ellipse.Width *= 2;
            Ellipse.Height *= 2;
        }
    }
}
