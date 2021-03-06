//using SchemeCreator.Data.Extensions;
//using SchemeCreator.Data.Interfaces;
//using SchemeCreator.Data.Services;
//using SchemeCreator.UI.Dynamic;
//using System;
//using Windows.Foundation;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Input;
//using static SchemeCreator.Data.Constants;

//namespace SchemeCreator.Data.Models
//{
//    public class GateBody : IGridChild
//    {
//        private Button button;
//        private Size Size;

//        public Action<GateView, Button> Tapped;

//        public GateBody(GateView gate)
//        {
//            button = new Button()
//            {
//                FontSize = 10,
//                HorizontalAlignment = HorizontalAlignment.Left,
//                VerticalAlignment = VerticalAlignment.Top,
//                HorizontalContentAlignment = HorizontalAlignment.Center,
//                VerticalContentAlignment = VerticalAlignment.Center

//            };

//            //if (external.Contains(gate.Type))
//            //    button.SetSizeAndCenter(externalGateSize, gate.MatrixIndex);
//            //else
//            //    button.SetSizeAndCenter(logicGateSize, gate.MatrixIndex);

//            button.Content = TextController.BuildButtonBodyText(gate.Type, gate.InputCount, gate.OutputCount);

//            Colorer.SetFillByValue(button, gate.Values[0]);

//            button.Tapped += (object sender, TappedRoutedEventArgs e) => Tapped(gate, button);
//        }

//        public bool ContainsBodyByMargin(Thickness t) => button.Margin == t;

//        public void AddToParent(SmartGrid parent) => parent.Add(button);
//    }
//}
