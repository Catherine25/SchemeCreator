using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI {
    class NewGateMenuController {

        //constructors
        public NewGateMenuController(Grid _grid) {
            _grid.Children.Add(grid);
            //The cast to (GateId[]) is not strictly necessary, but does make the code faster
            foreach (Constants.GateEnum id in (Constants.GateEnum[]) Enum.GetValues(typeof(Constants.GateEnum)))
                buttons.Add(id, CreateButton(btText[id]));
        }

        //public methods

        //private methods
        Button CreateButton(string text) {
            Button button = new Button() {
                Height = 25,
                Width = grid.Width,
                Content = text
            };
            return button;
        }

        //data
        Dictionary<Constants.GateEnum, Button> buttons = new Dictionary<Constants.GateEnum, Button>();
        Dictionary<Constants.GateEnum, string> btText = new Dictionary<Constants.GateEnum, string> {
            { Constants.GateEnum.AND, "AND" },
            { Constants.GateEnum.Buffer, "Buffer"},
            { Constants.GateEnum.IN, "Input"},
            { Constants.GateEnum.NAND, "NAND"},
            { Constants.GateEnum.NOR, "NOR"},
            { Constants.GateEnum.NOT, "NOT" },
            { Constants.GateEnum.OR, "OR"},
            { Constants.GateEnum.OUT, "Output"},
            { Constants.GateEnum.XNOR, "XNOR"},
            { Constants.GateEnum.XOR, "XOR"}
        };
        Grid grid = new Grid {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top
        };
        public void Dispose() => grid.Children.Clear();
        public void UpdateView(double width, double height) {
            grid.Width = width;
            grid.Height = height;
        }
    }
}