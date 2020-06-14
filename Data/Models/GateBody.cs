using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.Data.Services;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using static SchemeCreator.Data.Constants;

namespace SchemeCreator.Data.Models
{
    public class GateBody : IGridChild
    {
        private Button button;

        public Action<Gate, Button> Tapped;

        public GateBody(Gate gate)
        {
            button = new Button()
            {
                FontSize = 10,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            if (external.Contains(gate.type))
                button.SetSizeAndCenter(externalGateSize, gate.center);
            else
                button.SetSizeAndCenter(logicGateSize, gate.center);

            button.Content = TextController.BuildButtonBodyText(gate.type, gate.inputs, gate.outputs);

            Colorer.SetFillByValue(button, gate.values[0]);

            button.Tapped += (object sender, TappedRoutedEventArgs e) => Tapped(gate, button);
        }

        public bool ContainsBodyByMargin(Thickness t) => button.Margin == t;

        public void AddToParent(Grid parent) => parent.Children.Add(button);
    }
}
