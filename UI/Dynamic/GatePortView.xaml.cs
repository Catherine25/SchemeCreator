using SchemeCreator.Data.Models.Enums;
using SchemeCreator.Data.Services;
using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class GatePortView : UserControl
    {
        /// <summary>
        /// Defines the Port's type - Input or Output
        /// </summary>
        public readonly ConnectionTypeEnum Type;

        public bool? Value
        {
            get => _value;
            set
            {
                _value = value;
                XEllipse.Fill = Colorer.GetBrushByValue(_value);
                ValueChanged(_value);
            }
        }
        private bool? _value;

        public readonly int Index;

        public Point Center { get => CenterPoint.TransformToPoint(); }

        public Action<bool?> ValueChanged;

        public GatePortView() => InitializeComponent();

        public GatePortView(ConnectionTypeEnum connectionType, int index)
        {
            Type = connectionType;
            Index = index;

            InitializeComponent();

            XEllipse.Width = Constants.gatePortSize.Width;
            XEllipse.Height = Constants.gatePortSize.Height;
            Width = Constants.gatePortSize.Width;
            Height = Constants.gatePortSize.Height;

            Grid.SetRow(this, index);

            //SetCenterAndSize(null, Constants.gatePortSize);
            //base.Tapped += (SmartEllipse e) => Tapped(this);

            XEllipse.Tapped += (sender, e) => Tapped(this);
            XEllipse.PointerEntered += (sender, e) => XEllipse.IncreaseSize();
            XEllipse.PointerExited += (sender, e) => XEllipse.DecreaseSize();
        }

        public new Action<GatePortView> Tapped;

        //public void AddToParent(SmartGrid grid) => grid.Add(this);
    }
}
