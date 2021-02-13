using SchemeCreator.Data.Models.Enums;
using SchemeCreator.Data.Services;
using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SchemeCreator.Data.Models;
using SchemeCreator.Data.Extensions;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class GatePortView : UserControl
    {
        /// <summary>
        /// Defines the Port's type - Input or Output
        /// </summary>
        public readonly ConnectionTypeEnum Type;

        public AppearanceSettings AppearanceSettings { get; private set; }

        public bool? Value
        {
            get => _value;
            set
            {
                _value = value;
                AppearanceSettings.Brush = Colorer.GetBrushByValue(_value);
                ValueChanged(_value);
            }
        }
        private bool? _value;

        public readonly int Index;

        public Point Center { get => CenterPoint.TransformToPoint(); }

        public Action<bool?> ValueChanged;

        public GatePortView() => InitializeComponent();

        public GatePortView(ConnectionTypeEnum connectionType, int index, AppearanceSettings appearanceSettings)
        {
            Type = connectionType;
            AppearanceSettings = appearanceSettings;
            Index = index;

            InitializeComponent();

            Grid.SetRow(this, index);

            //SetCenterAndSize(null, Constants.gatePortSize);
            //base.Tapped += (SmartEllipse e) => Tapped(this);

            XEllipse.Tapped += (sender, e) => Tapped(this);
            XEllipse.PointerEntered += XEllipse_PointerEntered;
            XEllipse.PointerExited += XEllipse_PointerExited;
        }

        private void XEllipse_PointerExited(object sender, PointerRoutedEventArgs e) =>
            AppearanceSettings.Size = new Size(AppearanceSettings.Size.Width / 2, AppearanceSettings.Size.Height / 2);

        private void XEllipse_PointerEntered(object sender, PointerRoutedEventArgs e) =>
            AppearanceSettings.Size = new Size(AppearanceSettings.Size.Width * 2, AppearanceSettings.Size.Height * 2);

        public new Action<GatePortView> Tapped;

        //public void AddToParent(SmartGrid grid) => grid.Add(this);
    }
}
