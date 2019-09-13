using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.ConstantsNamespace;
using SchemeCreator.Data.Extensions;

namespace SchemeCreator.UI {
    class MenuController {

        /*      events      */
        public delegate void LastClickedBtEventHandler(object sender, LastClickedBtEventArgs e);
        public event LastClickedBtEventHandler NewSchemeBtClickEvent,
            LoadSchemeBtClickEvent,
            SaveSchemeBtClickEvent,
            TraceSchemeBtClickEvent,
            WorkSchemeBtClickEvent,
            AddGateBtClickEvent,
            AddLineBtClickEvent,
            RemoveLineBtClickEvent,
            ChangeValueBtClickEvent;

        /*      data        */
        Dictionary<Constants.BtId, Button> buttons = new Dictionary<Constants.BtId, Button>();
        Grid grid = new Grid();
        

        /*      constructor     */
        public MenuController() {
            grid.SetStandartAlighnment();
            //The cast to (BtId[]) is not strictly necessary, but does make the code faster
            foreach (Constants.BtId id in (Constants.BtId[])Enum.GetValues(typeof(Constants.BtId))) {
                Button button = new Button {
                    Content = Constants.btText[id],
                    Background = Constants.brushes[Constants.AccentEnum.dark1],
                    Foreground = Constants.brushes[Constants.AccentEnum.light1]
                };
                buttons.Add(id, button);
            }
            EventSubscribe();
        }
        
        /*      events      */
        private void EventSubscribe() {
            buttons[Constants.BtId.newSchemeBt].Click += NewSchemeBtClick;
            buttons[Constants.BtId.loadSchemeBt].Click += LoadSchemeBtClick;
            buttons[Constants.BtId.saveSchemeBt].Click += SaveSchemeBtClick;
            buttons[Constants.BtId.traceSchemeBt].Click += TraceSchemeBtClick;
            buttons[Constants.BtId.workSchemeBt].Click += WorkSchemeBtClick;
            buttons[Constants.BtId.addGateBt].Click += AddGateBtClick;
            buttons[Constants.BtId.addLineBt].Click += AddLineBtClick;
            buttons[Constants.BtId.removeLineBt].Click += RemoveLineBtClick;
            buttons[Constants.BtId.changeValueBt].Click += ChangeValueBtClick;
        }

        private void NewSchemeBtClick(object sender, RoutedEventArgs e) =>
            NewSchemeBtClickEvent(this, new LastClickedBtEventArgs(sender as Button, Constants.BtId.newSchemeBt));
        private void LoadSchemeBtClick(object sender, RoutedEventArgs e) =>
            LoadSchemeBtClickEvent(this, new LastClickedBtEventArgs(sender as Button, Constants.BtId.loadSchemeBt));
        private void SaveSchemeBtClick(object sender, RoutedEventArgs e) =>
            SaveSchemeBtClickEvent(this, new LastClickedBtEventArgs(sender as Button, Constants.BtId.saveSchemeBt));
        private void TraceSchemeBtClick(object sender, RoutedEventArgs e) =>
            TraceSchemeBtClickEvent(this, new LastClickedBtEventArgs(sender as Button, Constants.BtId.traceSchemeBt));
        private void WorkSchemeBtClick(object sender, RoutedEventArgs e) =>
            WorkSchemeBtClickEvent(this, new LastClickedBtEventArgs(sender as Button, Constants.BtId.workSchemeBt));
        private void AddGateBtClick(object sender, RoutedEventArgs e) {
            AddGateBtClickEvent(this, new LastClickedBtEventArgs(sender as Button, Constants.BtId.addGateBt));
            (sender as Button).BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
        }
        private void AddLineBtClick(object sender, RoutedEventArgs e) {
            AddLineBtClickEvent(this, new LastClickedBtEventArgs(sender as Button, Constants.BtId.addLineBt));
            (sender as Button).BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
        }       
        private void RemoveLineBtClick (object sender, RoutedEventArgs e) {
            RemoveLineBtClickEvent(this, new LastClickedBtEventArgs(sender as Button, Constants.BtId.removeLineBt));
            (sender as Button).BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
        }
        private void ChangeValueBtClick (object sender, RoutedEventArgs e) {
            ChangeValueBtClickEvent(this, new LastClickedBtEventArgs(sender as Button, Constants.BtId.changeValueBt));
            (sender as Button).BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
        }

        /*      methods     */
        public void Update(Rect rect) {
            int i = 0;
            foreach (Button button in buttons.Values) {
                button.Height = rect.Height;
                button.Width = rect.Width / (buttons.Count);
                button.Margin = new Thickness((i*button.Width), 0, 0, 0); 
                i++;
            }
        }

        public void SetParentGrid(Grid parentGrid) => parentGrid.Children.Add(grid);
        
        public void Show() {
            foreach (Button button in buttons.Values)
                grid.Children.Add(button);
        }
        
        public void Hide() => grid.Children.Clear();
        
        public void InActivateModeButtons() {
            buttons[Constants.BtId.addGateBt].BorderBrush = Constants.brushes[Constants.AccentEnum.dark1];
            buttons[Constants.BtId.addLineBt].BorderBrush = Constants.brushes[Constants.AccentEnum.dark1];
            buttons[Constants.BtId.removeLineBt].BorderBrush = Constants.brushes[Constants.AccentEnum.dark1];
            buttons[Constants.BtId.changeValueBt].BorderBrush = Constants.brushes[Constants.AccentEnum.dark1];
        }
    }

    class LastClickedBtEventArgs {
        public Constants.BtId btId;
        public Button button;
        public LastClickedBtEventArgs(Button _b, Constants.BtId _btId) {
            button = _b;
            btId = _btId;
        }
    }
}
