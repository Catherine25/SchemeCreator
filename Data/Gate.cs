using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using System;
using System.Runtime.Serialization;
using Windows.Foundation;
using System.Collections.Generic;

namespace SchemeCreator.Data {
    [DataContract] public class Gate {
        /*      data        */
        [DataMember] public Constants.GateEnum type;
        [DataMember] public bool isExternal;
        [DataMember] public int inputs, outputs;
        [DataMember] public double x, y;
        [DataMember] public List<bool> values;

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
                this.values = new List<bool> (inputs);

                for(int i = 0; i < inputs; i++)
                    values.Add(false);
        }

        public Button DrawBody() {

            Button button = new Button() {

                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,

                Background = Constants.brushes[Constants.AccentEnum.dark1],
                Foreground = Constants.brushes[Constants.AccentEnum.light1],

                Margin = new Thickness(x, y, 0, 0),
                FontSize = 10,
                Content = type.ToString()
            };

            if(isExternal) {
                button.Height = Constants.externalGateSize;
                button.Width = Constants.externalGateSize;
                button.Margin = new Thickness(
                    button.Margin.Left - Constants.externalGateSize / 2 - Constants.dotSize / 2,
                    button.Margin.Top - Constants.externalGateSize / 2 - Constants.dotSize / 2,
                    0,
                    0 );
            }
            else {
                button.Width = Constants.gateWidth;
                button.Height = Constants.gateHeight;

                if (!Constants.singleOutput.Contains(type))
                    button.Content += "\n" + inputs.ToString() + " in " + outputs.ToString();
            }

                button.Name = button.Margin.ToString();
                return button;
            }

        public List<Ellipse> DrawGateInOut(bool isInput) {
            
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

            List<Ellipse> ellipses = new List<Ellipse>();

            for (int i = 0; i < newInOutCount; i++) {

                t.Top += Constants.gateHeight / (newInOutCount + 1);

                if (i == 0)
                    t.Top -= Constants.lineStartOffset;

                ellipses.Add (new Ellipse() {
                    Height = SchemeCreator.Constants.dotSize,
                    Width = SchemeCreator.Constants.dotSize,
                    Fill = SchemeCreator.Constants.brushes[Constants.AccentEnum.light1],
                    Margin = t,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left } );
            }

            return ellipses;
        }

        public bool containsInOutByMargin(Ellipse e, bool isInput) =>
            DrawGateInOut(isInput).Exists(x => x.Margin == e.Margin);
        public bool containsBodyByMargin(Thickness t) => DrawBody().Margin == t;

        public int getIndexOfInOutByMargin(Thickness t, bool isInput) =>
            DrawGateInOut(isInput).FindIndex(x =>  x.Margin == t);
    }
}