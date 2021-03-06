using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SchemeSimulator
{
    public partial class WireView : UserControl
    {
        public bool? Value
        {
            get => _value;
            set
            {
                _value = value;

                if(_value == true)
                    Line.Fill = new SolidColorBrush(Colors.Cyan);
                else if (_value == false)
                    Line.Fill = new SolidColorBrush(Colors.DarkCyan);
                else
                    Line.Fill = new SolidColorBrush(Colors.Maroon);
            }
        }
        private bool? _value;

        public WireView() => InitializeComponent();

        public WireView(Point start, Point end)
        {
            InitializeComponent();

            Line.MouseEnter += Line_MouseEnter;
            Line.MouseLeave += Line_MouseLeave;

            Line.X1 = start.X;
            Line.Y1 = start.Y;
            Line.X2 = end.X;
            Line.Y2 = end.Y;

            _value = null;
        }

        private void Line_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Line.StrokeThickness /= 2;
        }

        private void Line_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Line.StrokeThickness *= 2;
        }
    }
}
