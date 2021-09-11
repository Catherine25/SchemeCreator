using SchemeCreator.Data.Models.Enums;
using SchemeCreator.Data.Services;
using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data;
using SchemeCreator.Data.Interfaces;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class GatePortView : UserControl, IValueHolder
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
                XEllipse.Fill = Colorer.GetBrushByValue(value);
                XEllipse.Stroke = Colorer.GetBrushByValue(value);
                ValueChanged?.Invoke(_value);
            }
        }
        private bool? _value;
        public Action<bool?> ValueChanged { get; set; }

        public readonly int Index;

        public Point Center { get => CenterPoint.TransformToPoint(); }

        public GatePortView() => InitializeComponent();

        /// <summary>
        /// Creates <see cref="GatePortView"/> with chosen <see cref="ConnectionTypeEnum"/>, sets <see cref="Grid.RowProperty"/>.
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="index"></param>
        public GatePortView(ConnectionTypeEnum connectionType, int index)
        {
            Type = connectionType;
            Index = index;

            InitializeComponent();

            XEllipse.Width = Constants.GatePortSize.Width;
            XEllipse.Height = Constants.GatePortSize.Height;
            Width = Constants.GatePortSize.Width;
            Height = Constants.GatePortSize.Height;

            Grid.SetRow(this, index);

            Colorer.SetFillByValue(this.XEllipse, null);

            XEllipse.Tapped += (sender, e) => Tapped(this);
            XEllipse.PointerEntered += (sender, e) => XEllipse.Activate();
            XEllipse.PointerExited += (sender, e) => XEllipse.Deactivate();
        }

        public new Action<GatePortView> Tapped;
    }
}
