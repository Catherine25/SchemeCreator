using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.UI.Dynamic;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class GatePortsLayer : UserControl, ILayer<GatePortView> 
    {
        public new Action<GatePortView> Tapped;
        public Action ValuesChanged;

        public GatePortsLayer()
        {
            InitializeComponent();
        }

        public void CreatePorts(ConnectionTypeEnum type, int count, bool extended = false)
        {
            Grid.SetSize(extended? GatePortView.ExtendedGatePortSize.Width : GatePortView.GatePortSize.Width, GateBodyView.LogicGateSize.Height);

            Grid.InitRows(count);

            for (int i = 0; i < count; i++)
            {
                GatePortView port = new(type, i, extended);
                port.Tapped += (p) => Tapped(p);
                port.ValueChanged += (newValue) => ValuesChanged?.Invoke();
                Add(port);
            }
        }

        #region ILayer

        public IEnumerable<GatePortView> Items => Grid.GetItems<GatePortView>();
        
        public void Add(GatePortView item) => Grid.Add(item);
        
        public void Clear() => Grid.Clear();

        #endregion
    }
}
