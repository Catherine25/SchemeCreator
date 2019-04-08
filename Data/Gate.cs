using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using System.Runtime.Serialization;
using Windows.Foundation;

namespace SchemeCreator.Data {
    [DataContract] public class Gate {
        /*      data        */
        [DataMember] public Constants.GateEnum type;
        [DataMember] public bool isExternal;
        [DataMember] public int inputs, outputs;
        [DataMember] public double x, y;
        [DataMember] public bool[] values;

        public Gate(Constants.GateEnum type,
            bool isExternal,
            int inputs,
            int outputs,
            double x,
            double y) {
                
                this.type = type;
                this.isExternal = isExternal;
                this.inputs = inputs;
                this.outputs = outputs;
                this.x = x;
                this.y = y;
                this.values = new bool[inputs];
        }

        public Button DrawBody() {

            Button button = new Button() {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Background = Constants.brushes[Constants.AccentEnum.dark1],
                Foreground = Constants.brushes[Constants.AccentEnum.light1],
                Margin = new Thickness(x, y, 0, 0 ),
                FontSize = 10,
                Content = type.ToString()
            };

            if(isExternal) {
                button.Height = Constants.dotSize * 2;
                button.Width = Constants.dotSize * 2;
            }
            else {
                if (Constants.singleInput.Contains(type)) {
                    button.Width = Constants.gateWidth;
                    button.Height = Constants.gateHeight;
                }
                else
                    button.Content += " " + inputs.ToString() + " in " + outputs.ToString();
                }

                button.Name = button.Margin.ToString();
                return button;
            }

        public Ellipse[] DrawGateInOut(bool isInput) {
            
            Thickness t = new Thickness {
                Left = x - Constants.lineStartOffset,
                Top = y
            };

            int newInOutCount;

            if(isInput)
                newInOutCount = inputs;
            else {
                newInOutCount = outputs;
                t.Left += Constants.gateWidth;
            }

            Ellipse[] ellipses = new Ellipse[newInOutCount];

            for (int i = 0; i < newInOutCount; i++) {

                t.Top += Constants.gateHeight / (newInOutCount + 1);

                if (i == 0)
                    t.Top -= Constants.lineStartOffset;

                ellipses[i] = new Ellipse() {
                    Height = SchemeCreator.Constants.dotSize,
                    Width = SchemeCreator.Constants.dotSize,
                    Fill = SchemeCreator.Constants.brushes[Constants.AccentEnum.light1],
                    Margin = t,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left };
            }
            return ellipses;
        }
    }
}