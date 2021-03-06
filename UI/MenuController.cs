//using System;
//using System.Collections.Generic;
//using Windows.Foundation;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using static SchemeCreator.Data.Constants;
//using SchemeCreator.Data.Services;
//using System.Linq;
//using System.Diagnostics;
//using SchemeCreator.Data.Models;

//namespace SchemeCreator.UI
//{
//    class MenuController
//    {
//        Dictionary<BtId, Button> buttons = new Dictionary<BtId, Button>();
//        StackPanel panel = new StackPanel();

//        #region Events

//        public event Action NewSchemeBtClickEvent;
//        public event Action LoadSchemeBtClickEvent;
//        public event Action SaveSchemeBtClickEvent;
//        public event Action TraceSchemeBtClickEvent;
//        public event Action WorkSchemeBtClickEvent;

//        public event Action<BtId> ChangeModeEvent;

//        #endregion

//        public MenuController()
//        {
//            panel.Orientation = Orientation.Horizontal;
//            panel.HorizontalAlignment = HorizontalAlignment.Left;
//            panel.VerticalAlignment = VerticalAlignment.Top;
//            Colorer.ColorGrid(panel);

//            int counter = 0;

//            CreateButton(BtId.newSchemeBt, ref counter);
//            buttons[BtId.newSchemeBt].Click += (object o, RoutedEventArgs e) => NewSchemeBtClickEvent();

//            CreateButton(BtId.loadSchemeBt, ref counter);
//            buttons[BtId.loadSchemeBt].Click += (object o, RoutedEventArgs e) => LoadSchemeBtClickEvent();

//            CreateButton(BtId.saveSchemeBt, ref counter);
//            buttons[BtId.saveSchemeBt].Click += (object o, RoutedEventArgs e) => SaveSchemeBtClickEvent();

//            CreateButton(BtId.traceSchemeBt, ref counter);
//            buttons[BtId.traceSchemeBt].Click += (object o, RoutedEventArgs e) => TraceSchemeBtClickEvent();

//            CreateButton(BtId.workSchemeBt, ref counter);
//            buttons[BtId.workSchemeBt].Click += (object o, RoutedEventArgs e) => WorkSchemeBtClickEvent();

//            CreateButton(BtId.addLineBt, ref counter);
//            buttons[BtId.addLineBt].Click += (object o, RoutedEventArgs e) => ChangeModeEvent(BtId.addLineBt);

//            CreateButton(BtId.changeValueBt, ref counter);
//            buttons[BtId.changeValueBt].Click += (object o, RoutedEventArgs e) => {
//                ChangeModeEvent(BtId.changeValueBt);
//                Colorer.ColorMenuButtonBorderByValue(o as Button, true); };
//        }

        

//        #region Public methods

//        public void Update(Rect rect)
//        {
//            panel.Width = rect.Width;
//            panel.Height = rect.Height;

//            Debug.WriteLine($"Grid Width = {panel.Width}, Height = {panel.Height}");
//        }

//        public void SetParentGrid(Grid parentGrid) => parentGrid.Children.Add(panel);

//        public void Show() => buttons.Values.ToList().ForEach(b => panel.Children.Add(b));

//        public void Hide() => panel.Children.Clear();
        
//        public void InActivateModeButtons()
//        {
//            Colorer.ColorMenuButtonBorderByValue(buttons[BtId.addLineBt] as Button, false);
//            Colorer.ColorMenuButtonBorderByValue(buttons[BtId.changeValueBt] as Button, false);
//        }

//        #endregion
//    }
//}
