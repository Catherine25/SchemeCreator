using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Interfaces;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class PortConfigurationView : UserControl, IGridComponent
    {
        public PortConfigurationView() => InitializeComponent();

        public bool Value
        {
            get => Switch.IsEnabled;
            set => Switch.IsEnabled = value;
        }

        public Vector2 MatrixLocation
        {
            get => this.GetMatrixLocation();
            set => this.SetMatrixLocation(value);
        }

        public void SetIsInput(bool isInput) =>
            Switch.FlowDirection = isInput ? FlowDirection.LeftToRight : FlowDirection.RightToLeft;
    }
}
