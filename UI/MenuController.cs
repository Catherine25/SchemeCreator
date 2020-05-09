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
        public event Action NewSchemeBtClickEvent,
            LoadSchemeBtClickEvent,
            SaveSchemeBtClickEvent,
            TraceSchemeBtClickEvent,
            WorkSchemeBtClickEvent;
        public event Action<Constants.BtId>
            ChangeModeEvent;

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
            buttons[Constants.BtId.addLineBt].Click += AddLineBtClick;
            buttons[Constants.BtId.changeValueBt].Click += ChangeValueBtClick;
        }

        private void NewSchemeBtClick(object sender, RoutedEventArgs e) =>
            NewSchemeBtClickEvent();
        private void LoadSchemeBtClick(object sender, RoutedEventArgs e) =>
            LoadSchemeBtClickEvent();
        private void SaveSchemeBtClick(object sender, RoutedEventArgs e) =>
            SaveSchemeBtClickEvent();
        private void TraceSchemeBtClick(object sender, RoutedEventArgs e) =>
            TraceSchemeBtClickEvent();
        private void WorkSchemeBtClick(object sender, RoutedEventArgs e) =>
            WorkSchemeBtClickEvent();
        private void AddLineBtClick(object sender, RoutedEventArgs e) {
            ChangeModeEvent(Constants.BtId.addLineBt);
            (sender as Button).BorderBrush = Constants.brushes[Constants.AccentEnum.light1];
        }       
        private void ChangeValueBtClick (object sender, RoutedEventArgs e) {
            ChangeModeEvent(Constants.BtId.changeValueBt);
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
            buttons[Constants.BtId.addLineBt].BorderBrush = Constants.brushes[Constants.AccentEnum.dark1];
            buttons[Constants.BtId.changeValueBt].BorderBrush = Constants.brushes[Constants.AccentEnum.dark1];
        }
    }
}
