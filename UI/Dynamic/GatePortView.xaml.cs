using SchemeCreator.Data.Services;
using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;

namespace SchemeCreator.UI.Dynamic
{
    public enum ConnectionTypeEnum
    {
        Input,
        Output
    }
    
    public sealed partial class GatePortView : UserControl, IValueHolder
    {
        public new Action<GatePortView> Tapped;
     
        public static readonly Size GatePortSize = new(10, 10);

        /// <summary>
        /// Defines the Port's type - Input or Output
        /// </summary>
        public readonly ConnectionTypeEnum Type;

        public readonly int Index;

        private readonly bool extended;

        public GatePortView() => InitializeComponent();

        /// <summary>
        /// Creates <see cref="GatePortView"/> with chosen <see cref="ConnectionTypeEnum"/>, sets <see cref="Grid.RowProperty"/>.
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="index"></param>
        public GatePortView(ConnectionTypeEnum connectionType, int index, bool extended = false)
        {
            InitializeComponent();
            
            Type = connectionType;
            Index = index;
            this.extended = extended;

            this.SetSize(GatePortSize);
            Grid.SetRow(this, index);

            if (extended)
            {
                Ellipse.Fill = Colorer.DeactivatedColor;
                Ellipse.Stroke = Colorer.ErrorBrush;
                Ellipse.StrokeThickness = 2;
            }
            else
            {
                Ellipse.Fill = Colorer.ErrorBrush;
                Ellipse.Stroke = Colorer.ErrorBrush;
            }

            Ellipse.Tapped += (_, _) => Tapped(this);
            Ellipse.PointerEntered += (_, _) => Ellipse.Stroke = Colorer.ActivatedColor;
            Ellipse.PointerExited += (_, _) => Ellipse.Stroke = Colorer.GetBrushByValue(Value);
        }

        #region ValueHolder

        public Action<bool?> ValueChanged { get; set; }

        public bool? Value
        {
            get => value;
            set
            {
                this.value = value;
                Ellipse.Fill = extended ? Colorer.DeactivatedColor : Colorer.GetBrushByValue(value);
                Ellipse.Stroke = Colorer.GetBrushByValue(value);
                ValueChanged(this.value);
                UpdateLayout();
            }
        }
        private bool? value;
        
        public void SwitchValue() => this.SwitchControlValue();

        public void Reset() => this.ResetControlValue();

        #endregion
    }
}
