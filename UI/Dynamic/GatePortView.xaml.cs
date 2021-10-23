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
        public new Action<GatePortView> Tapped;

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
                Ellipse.Fill = Colorer.GetBrushByValue(value);
                Ellipse.Stroke = Colorer.GetBrushByValue(value);
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

            Ellipse.Width = Constants.GatePortSize.Width;
            Ellipse.Height = Constants.GatePortSize.Height;
            Width = Constants.GatePortSize.Width;
            Height = Constants.GatePortSize.Height;

            Grid.SetRow(this, index);

            Colorer.SetFillByValue(Ellipse, null);

            Ellipse.Tapped += (sender, e) => Tapped(this);
            Ellipse.PointerEntered += (sender, e) => Ellipse.Activate();
            Ellipse.PointerExited += (sender, e) => Ellipse.Deactivate();
        }
    }
}
