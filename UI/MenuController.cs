using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Extensions;
using static SchemeCreator.Data.Constants;
using SchemeCreator.Data.Services;
using System.Linq;

namespace SchemeCreator.UI
{
    class MenuController
    {
        Dictionary<BtId, Button> buttons = new Dictionary<BtId, Button>();
        Grid grid = new Grid();

        #region Events

        public event Action NewSchemeBtClickEvent;
        public event Action LoadSchemeBtClickEvent;
        public event Action SaveSchemeBtClickEvent;
        public event Action TraceSchemeBtClickEvent;
        public event Action WorkSchemeBtClickEvent;

        public event Action<BtId> ChangeModeEvent;

        #endregion

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
        
        private void EventSubscribe()
        {
            buttons[BtId.newSchemeBt].Click += (object o, RoutedEventArgs e) => NewSchemeBtClickEvent();
            buttons[BtId.loadSchemeBt].Click += (object o, RoutedEventArgs e) => LoadSchemeBtClickEvent();
            buttons[BtId.saveSchemeBt].Click += (object o, RoutedEventArgs e) => SaveSchemeBtClickEvent();
            buttons[BtId.traceSchemeBt].Click += (object o, RoutedEventArgs e) => TraceSchemeBtClickEvent();
            buttons[BtId.workSchemeBt].Click += (object o, RoutedEventArgs e) => WorkSchemeBtClickEvent();
            buttons[BtId.addLineBt].Click += (object o, RoutedEventArgs e) => ChangeModeEvent(BtId.addLineBt);

            buttons[BtId.changeValueBt].Click += (object o, RoutedEventArgs e) => {
                ChangeModeEvent(BtId.changeValueBt);
                Colorer.ColorMenuButtonBorderByValue(o as Button, true); };
        }

        #region Public methods

        public void Update(Rect rect)
        {
            int i = 0;
            foreach (Button button in buttons.Values)
            {
                button.Height = rect.Height;
                button.Width = rect.Width / buttons.Count;
                button.Margin = new Thickness(i * button.Width, 0, 0, 0);
                i++;
            }
        }

        public void SetParentGrid(Grid parentGrid) => parentGrid.Children.Add(grid);

        public void Show() => buttons.Values.ToList().ForEach(b => grid.Children.Add(b));

        public void Hide() => grid.Children.Clear();
        
        public void InActivateModeButtons()
        {
            Colorer.ColorMenuButtonBorderByValue(buttons[BtId.addLineBt] as Button, false);
            Colorer.ColorMenuButtonBorderByValue(buttons[BtId.changeValueBt] as Button, false);
        }

        #endregion
    }
}
