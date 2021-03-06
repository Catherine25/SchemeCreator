using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SchemeSimulator
{
    public partial class GatePortView : UserControl
    {
        public bool? Value
        {
            get
            {
                if (Ellipse.Fill == ActiveBrush)
                    return true;
                else if (Ellipse.Fill == InctiveBrush)
                    return false;
                else return null;
            }

            set
            {
                if (value == true)
                    Ellipse.Fill = ActiveBrush;
                else if (value == false)
                    Ellipse.Fill = InctiveBrush;
                else
                    Ellipse.Fill = UnknownBrush;
            }
        }

        private SolidColorBrush ActiveBrush;
        private SolidColorBrush InctiveBrush;
        private SolidColorBrush UnknownBrush;

        public readonly bool IsInput;

        public GatePortView() => InitializeComponent();
        public GatePortView(bool isInput)
        {
            InitializeComponent();
            
            ActiveBrush = new SolidColorBrush(Colors.LightGray);
            InctiveBrush = new SolidColorBrush(Colors.DarkGray);
            UnknownBrush = new SolidColorBrush(Colors.Maroon);
            
            Ellipse.MouseEnter += Ellipse_MouseEnter;
            Ellipse.MouseLeave += Ellipse_MouseLeave;

            IsInput = isInput;
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
