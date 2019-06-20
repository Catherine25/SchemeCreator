using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                rb.Checked += RbChecked;
                rb.Unchecked += RbUnchecked;
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
                    b.Click += BtClick;
                }
            }
            inputCount[0].IsChecked = true;
        }

        private void BtClick(object sender, RoutedEventArgs e) =>
            new NewGateBtClickedEventArgs(
                buttons[sender as Button].inputCount,
                buttons[sender as Button].outputCount,
                buttons[sender as Button].type,
                buttons[sender as Button].isExternal);

        private void RbChecked(object sender, RoutedEventArgs e) {
            (sender as RadioButton).Foreground = Constants.brushes[Constants.AccentEnum.light1];
            
            if((sender as RadioButton).Content.ToString() == "1 output")
                foreach(Button b in buttons.Keys)
                    if(Constants.external.Contains(buttons[b].type) ||
                        Constants.singleOutput.Contains(buttons[b].type)) {
                            b.IsEnabled = true;
                            buttons[b].inputCount = 1;
                        }
                    else b.IsEnabled = false;

            else
                foreach(Button b in buttons.Keys) {
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
            
        private void RbUnchecked(object sender, RoutedEventArgs e) =>
            (sender as RadioButton).Foreground = Constants.brushes[Constants.AccentEnum.dark1];

        /*      data        */

        Dictionary<Button, Data.NewGateBt> buttons = new Dictionary<Button, Data.NewGateBt>();

        Grid grid = new Grid {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        
        private readonly RadioButton[] inputCount = new RadioButton[6];
        public bool IsActive { get; private set; }

        /*      events      */

        public delegate void NewGateBtClickedEventHandler(
            object sender, NewGateBtClickedEventArgs e);
        public event NewGateBtClickedEventHandler NewGateBtClickedEvent;
        
        /*      methods     */

        public void Update(Size size) {

            grid.Width = size.Width;
            grid.Height = size.Height;

            for(int j = 0; j < 6; j++) {
                inputCount[j].Margin = new Thickness(j * (size.Width / 6), 0, 0, 0);
                grid.Children.Add(inputCount[j]);
            }
                
            int i = 1;
            foreach(Button button in buttons.Keys) {
                button.Width = size.Width;
                button.Height = 50;
                button.Margin = new Thickness(0, 50 * i, 0, 0);
                i++;
            }        
        }

        public void SetParentGrid(Grid parentGrid) {
            parentGrid.Children.Add(grid);

            foreach (Button button in buttons.Keys) {
                grid.Children.Add(button);
                button.Click += BtClicked;
            }
        }

        private void BtClicked(object sender, RoutedEventArgs e) =>
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