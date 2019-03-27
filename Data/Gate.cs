using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.Data {
    public class Gate {
        /*      data        */
        public Constants.GateEnum type;
        public bool isExternal;
        public int inputs, outputs;
        public double x, y;
        public bool[] values;

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
            if(isExternal)
                return new Button {
                    Margin = new Thickness(x, y, 0, 0),
                    Height = Constants.dotSize * 2,
                    Width = Constants.dotSize * 2,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = Constants.brushes[Constants.AccentEnum.dark1],
                    Foreground = Constants.brushes[Constants.AccentEnum.light1],
                    Content = type.ToString() };
            else {
                if (Constants.singleInput.Contains(type)) {
                    return new Button {
                        Margin = new Thickness(x, y, 0, 0),
                        Width = Constants.gateWidth,
                        Height = Constants.gateHeight,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Background = Constants.brushes[Constants.AccentEnum.dark1],
                        Foreground = Constants.brushes[Constants.AccentEnum.light1],
                        Content = type.ToString() }; }
                else {
                    return new Button {
                        Margin = new Thickness(x, y, 0, 0),
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Background = Constants.brushes[Constants.AccentEnum.dark1],
                        Foreground = Constants.brushes[Constants.AccentEnum.light1],
                        Content = type.ToString() + inputs.ToString() + " in " + outputs.ToString() };
                }
            }
        }

        public Ellipse[] DrawGateInputs() {

            Ellipse[] ellipses = new Ellipse[inputs];
            Thickness t = new Thickness{
                Left = x - SchemeCreator.Constants.offset,
                Top = y - SchemeCreator.Constants.offset
            };                 

            for (int i = 0; i < inputs; i++) {                
                //margin set
                t.Top += Constants.gateHeight / (inputs + 1);

                if (i == 0)
                    t.Top -= SchemeCreator.Constants.lineStartOffset;

                t.Left = x - SchemeCreator.Constants.lineStartOffset;

                //create inputs array
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

        public Ellipse[] DrawGateOutputs() {
            Ellipse[] ellipses = new Ellipse[outputs];
            Thickness t = new Thickness();
            
            t.Left = x + Constants.gateWidth + Constants.lineStartOffset;
            t.Top = x + (GateController.gateHeight / 2) - SchemeCreator.Constants.lineStartOffset;
            
            for (int i = 0; i<outputs; i++) {
                t.Top += Constants.gateHeight / (inputs + 1);

                if(i == 0)
                    t.Top -= SchemeCreator.Constants.lineStartOffset;
                t.Left = x - SchemeCreator.Constants.lineStartOffset;
                
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