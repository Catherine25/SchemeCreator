using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Extensions;
using static SchemeCreator.Data.Constants;
using SchemeCreator.Data.Services;

namespace SchemeCreator.UI
{
    class MenuController
    {
        Dictionary<BtId, Button> buttons = new Dictionary<BtId, Button>();
        Grid grid = new Grid();

        /*      events      */
        public event Action NewSchemeBtClickEvent,
            LoadSchemeBtClickEvent,
            SaveSchemeBtClickEvent,
            TraceSchemeBtClickEvent,
            WorkSchemeBtClickEvent;
        public event Action<BtId> ChangeModeEvent;

        /*      constructor     */
        public MenuController()
        {
            grid.SetStandartAlighnment();
            //The cast to (BtId[]) is not strictly necessary, but does make the code faster
            foreach (BtId id in (BtId[])Enum.GetValues(typeof(BtId)))
            {
                Button button = new Button { Content = btText[id] };

                Colorer.ColorMenuButton(button);

                buttons.Add(id, button);
            }
            EventSubscribe();
        }
        
        /*      events      */
        private void EventSubscribe() {
            buttons[BtId.newSchemeBt].Click += NewSchemeBtClick;
            buttons[BtId.loadSchemeBt].Click += LoadSchemeBtClick;
            buttons[BtId.saveSchemeBt].Click += SaveSchemeBtClick;
            buttons[BtId.traceSchemeBt].Click += TraceSchemeBtClick;
            buttons[BtId.workSchemeBt].Click += WorkSchemeBtClick;
            buttons[BtId.addLineBt].Click += AddLineBtClick;
            buttons[BtId.changeValueBt].Click += ChangeValueBtClick;
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
            ChangeModeEvent(BtId.addLineBt);
            Colorer.ColorMenuButtonBorderByValue(sender as Button, true);
        }       
        private void ChangeValueBtClick (object sender, RoutedEventArgs e) {
            ChangeModeEvent(BtId.changeValueBt);
            Colorer.ColorMenuButtonBorderByValue(sender as Button, true);
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
            Colorer.ColorMenuButtonBorderByValue(buttons[BtId.addLineBt] as Button, false);
            Colorer.ColorMenuButtonBorderByValue(buttons[BtId.changeValueBt] as Button, false);
        }
    }
}
