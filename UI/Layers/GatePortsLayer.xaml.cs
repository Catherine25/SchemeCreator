using System;
using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.Data.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Dynamic
{
    public sealed partial class GatePortsLayer : UserControl, ILayer<GatePortView> 
    {
        public new Action<GatePortView> Tapped;
        public Action ValuesChanged;

        public GatePortsLayer()
        {
            InitializeComponent();
            Grid.SetSize(Constants.GatePortSize.Width, Constants.LogicGateSize.Height);
        }

        public void CreatePorts(ConnectionTypeEnum type, int count)
        {
            Grid.InitGridColumnsAndRows(new Size(0, count));

            for (int i = 0; i < count; i++)
            {
                GatePortView port = new(type, i);
                port.Tapped += (port) => Tapped(port);
                port.ValueChanged += (newValue) => ValuesChanged?.Invoke();
                AddToView(port);
            }
        }

        public IEnumerable<GatePortView> Items => Grid.Children.Select(p => p as GatePortView);
        public void AddToView(GatePortView item) => Grid.Children.Add(item);
        public void Clear() => Grid.Children.Clear();
    }
}
