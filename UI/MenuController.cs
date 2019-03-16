using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SchemeCreator.UI {
    class MenuController {
        public delegate void LastClickedButtonHandler(object sender, LastClickedButtonEventArgs e);
        public event LastClickedButtonHandler NewSchemeBtClickEvent,
            LoadSchemeBtClickEvent,
            SaveSchemeBtClickEvent,
            TraceSchemeBtClickEvent,
            WorkSchemeBtClickEvent,
            AddGateBtClickEvent,
            AddLineBtClickEvent,
            RemoveLineBtClickEvent;
        //data
        Dictionary<Constants.BtId, Button> buttons = new Dictionary<Constants.BtId, Button>();

        Grid grid = new Grid {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };

        //constructor
        public MenuController(Grid _grid) {
            _grid.Children.Add(grid);
            //The cast to (BtId[]) is not strictly necessary, but does make the code faster
            foreach (Constants.BtId id in (Constants.BtId[])Enum.GetValues(typeof(Constants.BtId))) {
                Button button = CreateButton(Constants.btText[id]);
                grid.Children.Add(button);
                buttons.Add(id, button);
            }
            buttons[Constants.BtId.newSchemeBt].Click += NewSchemeBtClick;
            buttons[Constants.BtId.loadSchemeBt].Click += LoadSchemeBtClick;
            buttons[Constants.BtId.saveSchemeBt].Click += SaveSchemeBtClick;
            buttons[Constants.BtId.traceSchemeBt].Click += TraceSchemeBtClick;
            buttons[Constants.BtId.workSchemeBt].Click += WorkSchemeBtClick;
            buttons[Constants.BtId.addGateBt].Click += AddGateBtClick;
            buttons[Constants.BtId.addLineBt].Click += AddLineBtClick;
            buttons[Constants.BtId.removeLineBt].Click += RemoveLineBtClick;
        }

        //handlers
        private void NewSchemeBtClick(object sender, RoutedEventArgs e) {
            if(NewSchemeBtClickEvent == null)
                NewSchemeBtClickEvent(this, new LastClickedButtonEventArgs(sender as Button, Constants.BtId.newSchemeBt));
        }
        private void LoadSchemeBtClick(object sender, RoutedEventArgs e) {
            if(LoadSchemeBtClickEvent == null)
                LoadSchemeBtClickEvent(this, new LastClickedButtonEventArgs(sender as Button, Constants.BtId.loadSchemeBt));
        }
        private void SaveSchemeBtClick(object sender, RoutedEventArgs e) {
            if(SaveSchemeBtClickEvent == null)
                SaveSchemeBtClickEvent(this, new LastClickedButtonEventArgs(sender as Button, Constants.BtId.saveSchemeBt));
        }
        private void TraceSchemeBtClick(object sender, RoutedEventArgs e) {
            if(TraceSchemeBtClickEvent == null)
                TraceSchemeBtClickEvent(this, new LastClickedButtonEventArgs(sender as Button, Constants.BtId.traceSchemeBt));
        }
            
        private void WorkSchemeBtClick(object sender, RoutedEventArgs e) {
            if(WorkSchemeBtClickEvent == null)
                WorkSchemeBtClickEvent(this, new LastClickedButtonEventArgs(sender as Button, Constants.BtId.workSchemeBt));
        }
        private void AddGateBtClick(object sender, RoutedEventArgs e) {
            if(AddGateBtClickEvent == null)
                AddGateBtClickEvent(this, new LastClickedButtonEventArgs(sender as Button, Constants.BtId.addGateBt));
            (sender as Button).BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
        }
        private void AddLineBtClick(object sender, RoutedEventArgs e) {
            if(AddGateBtClickEvent == null)
                AddLineBtClickEvent(this, new LastClickedButtonEventArgs(sender as Button, Constants.BtId.addLineBt));
        }
            
        private void RemoveLineBtClick (object sender, RoutedEventArgs e) {
            if(RemoveLineBtClickEvent == null) 
                RemoveLineBtClickEvent(this, new LastClickedButtonEventArgs(sender as Button, Constants.BtId.removeLineBt));
        }

        //methods
        Button CreateButton(string text) {
            Button bt = new Button {
                Content = text,
                Background = Constants.brushes[Constants.AccentEnum.dark1],
                Foreground = Constants.brushes[Constants.AccentEnum.light1]
            };
            return bt;
        }
        public void UpdateView(double width, double height) {
            int i = 0;
            foreach (Button button in buttons.Values) {
                button.Height = height/20;
                button.Width = width / (buttons.Count);
                button.Margin = new Thickness((i*button.Width), 10, 0, 0); 
                i++;
            }
        }
        public void Dispose() => grid.Children.Clear();
        public void InActivateModeButtons() {
            buttons[Constants.BtId.addGateBt].BorderBrush = Constants.brushes[Constants.AccentEnum.dark1];
            buttons[Constants.BtId.addLineBt].BorderBrush = Constants.brushes[Constants.AccentEnum.dark1];
            buttons[Constants.BtId.removeLineBt].BorderBrush = Constants.brushes[Constants.AccentEnum.dark1];
        }
    }
    class LastClickedButtonEventArgs {
        public Constants.BtId btId;
        public Button button;
        public LastClickedButtonEventArgs(Button _b, Constants.BtId _btId) {
            button = _b;
            btId = _btId;
        }
    }
}
