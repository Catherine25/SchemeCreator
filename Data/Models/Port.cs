using SchemeCreator.Data.Interfaces;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using static SchemeCreator.Data.ConstantsNamespace.Constants;

namespace SchemeCreator.Data.Models
{
    public class Port : IGridChild, IResizable
    {
        #region Fields

        public readonly ConnectionType Type;

        public bool? BooleanValue
        {
            get { return booleanValue; }
            set
            {
                booleanValue = value;

                if (booleanValue == true)
                    ellipse.Fill = brushes[AccentEnum.light1];
                else if (booleanValue == false)
                    ellipse.Fill = brushes[AccentEnum.dark1];
                else
                    ellipse.Fill = brushes[AccentEnum.accent2];
            }
        }
        private bool? booleanValue;

        public Point CenterPoint { get; set; }
        
        public Size Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                ellipse.Width = size.Width;
                ellipse.Height = size.Height;
                ellipse.Margin = new Thickness
                {
                    Left = CenterPoint.X - size.Width / 2,
                    Top = CenterPoint.Y - size.Height / 2
                };
            }
        }
        private Size size;

        private Ellipse ellipse;

        #endregion

        #region Events

        public event Action<Port> Tapped;

        #endregion
        
        public Port(ConnectionType connectionType)
        {
            Type = connectionType;

            ellipse = new Ellipse
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            ellipse.Tapped += (object sender, TappedRoutedEventArgs e) => Tapped(this);
            ellipse.PointerEntered += (object sender, PointerRoutedEventArgs e) => ChangeSize(true);
            ellipse.PointerExited += (object sender, PointerRoutedEventArgs e) => ChangeSize(false);
        }

        public void ChangeSize(bool increase) =>
            Size = increase ? new Size(Size.Width * 2, Size.Height * 2) : new Size(Size.Width / 2, Size.Height / 2);

        public void AddToParent(Grid grid) => grid.Children.Add(ellipse);
    }
}