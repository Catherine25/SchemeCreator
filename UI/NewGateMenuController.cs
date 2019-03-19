using System;
using System.Collections.Generic;
using SchemeCreator.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI {
    class NewGateMenuController : IFrameInterface {

        /*      constructor     */
        public NewGateMenuController() {
            //The cast to (GateId[]) is not strictly necessary, but does make the code faster
            foreach (Constants.GateEnum id in (Constants.GateEnum[]) Enum.GetValues(typeof(Constants.GateEnum))) {
                buttons.Add(id,
                    new Button {
                        Height = 50,
                        Width = grid.Width - 20,
                        Content = btText[id]
                    }
                );
            }
        }
        /*      data        */
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
        public bool IsActive { get => isActive; }
        bool isActive;

        /*      methods     */
        public void Update(double width, double height) {
            grid.Width = width;
            grid.Height = height;

            foreach(Button button in grid.Children)
                button.Width = grid.ActualWidth;
        }

        public void SetParentGrid(Grid parentGrid) {
            parentGrid.Children.Add(grid);
            grid.Width = parentGrid.ActualWidth;
            grid.Height = parentGrid.ActualHeight;

            foreach (Button button in buttons.Values)
                grid.Children.Add(button);
        }

        public void Show() {
            foreach(Button button in buttons.Values)
                grid.Children.Add(button);  
        }

        public void Hide() => grid.Children.Clear();
    }
}