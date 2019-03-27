using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace SchemeCreator.UI {
    class NewGateMenuController : IFrameInterface {

        /*      constructor     */
        public NewGateMenuController() {

            string[] inputs = new string[] {  "1", "2", "3", "4", "6", "8" };

            for (int i = 0; i < inputs.Length; i++)
                inputCount[i] = new RadioButton {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = inputs[i] + " output",
                Foreground = Constants.brushes[Constants.AccentEnum.dark1]
            };

            foreach(RadioButton rb in inputCount) {
                rb.Checked += rbChecked;
                rb.Unchecked += rbUnchecked;
            }
                
            //The cast to (GateId[]) is not strictly necessary, but does make the code faster
            foreach (Constants.GateEnum gate in (Constants.GateEnum[])Enum.GetValues(typeof(Constants.GateEnum))) {
                if(Constants.external.Contains(gate))
                    buttons.Add(
                        new Button { Content = Constants.gateNames[gate] },
                        new Data.NewGateBt(gate, 0, 0, true));
                else {
                    if (Constants.singleOutput.Contains(gate))
                        buttons.Add(
                            new Button { Content = Constants.gateNames[gate] },
                            new Data.NewGateBt(gate, 1, 1, false));
                    else buttons.Add(
                            new Button { Content = Constants.gateNames[gate] },
                            new Data.NewGateBt(gate, 2, 1, false));
                }

                foreach (Button b in buttons.Keys) {
                    b.HorizontalAlignment = HorizontalAlignment.Left;
                    b.VerticalAlignment = VerticalAlignment.Top;
                    b.Background = Constants.brushes[Constants.AccentEnum.dark1];
                    b.Foreground = Constants.brushes[Constants.AccentEnum.light1];
                    b.Click += btClick;
                }
            }
            inputCount[0].IsChecked = true;
        }

        private void btClick(object sender, RoutedEventArgs e) =>
            new NewGateBtClickedEventArgs(
                buttons[sender as Button].inputCount,
                buttons[sender as Button].outputCount,
                buttons[sender as Button].type,
                buttons[sender as Button].isExternal);

        private void rbChecked(object sender, RoutedEventArgs e) {
            (sender as RadioButton).Foreground = Constants.brushes[Constants.AccentEnum.light1];
            
            if((sender as RadioButton).Content.ToString() == "1 output")
                foreach(Button b in buttons.Keys) {
                    if(Constants.external.Contains(buttons[b].type) ||
                        Constants.singleOutput.Contains(buttons[b].type))
                            b.IsEnabled = true;
                    else b.IsEnabled = false;
            }
            else foreach(Button b in buttons.Keys) {
                if(Constants.external.Contains(buttons[b].type) ||
                        Constants.singleOutput.Contains(buttons[b].type) )
                            b.IsEnabled = false;
                    else b.IsEnabled = true;

                if((sender as RadioButton).Content.ToString().StartsWith("2"))
                    buttons[b].inputCount = 2;
                if((sender as RadioButton).Content.ToString().StartsWith("3"))
                    buttons[b].inputCount = 3;
                if((sender as RadioButton).Content.ToString().StartsWith("4"))
                    buttons[b].inputCount = 4;
                if((sender as RadioButton).Content.ToString().StartsWith("6"))
                    buttons[b].inputCount = 6;
                if((sender as RadioButton).Content.ToString().StartsWith("8"))
                    buttons[b].inputCount = 8;
            }
        }
            
        private void rbUnchecked(object sender, RoutedEventArgs e) =>
            (sender as RadioButton).Foreground = Constants.brushes[Constants.AccentEnum.dark1];

        /*      data        */

        Dictionary<Button, Data.NewGateBt> buttons = new Dictionary<Button, Data.NewGateBt>();
        Grid grid = new Grid {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        RadioButton[] inputCount = new RadioButton[6];
        public bool IsActive { get; private set; }

        /*      events      */

        public delegate void NewGateBtClickedEventHandler(object sender, NewGateBtClickedEventArgs e);
        public event NewGateBtClickedEventHandler NewGateBtClickedEvent;
        
        /*      methods     */

        public void Update(double width, double height) {
            
            System.Diagnostics.Debug.Assert(width != 0);
            System.Diagnostics.Debug.Assert(height != 0);
            
            grid.Width = width;
            grid.Height = height;

            for(int j = 0; j < 6; j++) {
                inputCount[j].Margin = new Thickness(j * (width / 6), 0, 0, 0);
                grid.Children.Add(inputCount[j]);
            }
                
            int i = 1;
            foreach(Button button in buttons.Keys) {
                button.Width = width;
                button.Height = 50;
                button.Margin = new Thickness(0, 50 * i, 0, 0);
                i++;
            }        
        }

        public void SetParentGrid(Grid parentGrid) {
            parentGrid.Children.Add(grid);

            foreach (Button button in buttons.Keys) {
                grid.Children.Add(button);
                button.Click += btClicked;
            }
        }

        private void btClicked(object sender, RoutedEventArgs e) =>
            NewGateBtClickedEvent(sender, new NewGateBtClickedEventArgs(
                buttons[sender as Button].inputCount,
                buttons[sender as Button].outputCount,
                buttons[sender as Button].type,
                buttons[sender as Button].isExternal));

        public void Show() {
            foreach(Button button in buttons.Keys)
                grid.Children.Add(button);
            
            IsActive = true;
        }

        public void Hide() {
            grid.Children.Clear();
            IsActive = false;
        }
    }

    class NewGateBtClickedEventArgs {
        public NewGateBtClickedEventArgs(
            int _inputs,
            int _outputs,
            Constants.GateEnum _type,
            bool _isExternal) {
            
            inputs = _inputs;
            outputs = _outputs;
            type = _type;
            isExternal = _isExternal;
        }

        public int inputs, outputs;
        public Constants.GateEnum type;
        public bool isExternal;
    }
}