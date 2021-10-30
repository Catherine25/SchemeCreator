using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;
using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class ExternalPortsLayer : UserControl, ILayer<ExternalPortView>
    {
        public event Action<ExternalPortView> Tapped;
        public event Action<ExternalPortView> RemoveConnectedWires;

        public ExternalPortsLayer()
        {
            InitializeComponent();
            Grid.InitGridColumnsAndRows(SchemeView.GridSize);
        }

        public ExternalPortView GetFirstNotInitedExternalPort() => Items.Where(x => x.Type == PortType.Input).FirstOrDefault(x => x.Value == null);

        #region ILayer

        public void Add(ExternalPortView port)
        {
            port.RightTapped += (p) =>
            {
                RemoveConnectedWires(p);
                Grid.Remove(p);
            };
            port.Tapped += (p) => Tapped(p);
            Grid.Add(port);
        }
        
        public IEnumerable<ExternalPortView> Items => Grid.GetItems<ExternalPortView>();
        
        public void Clear() => Grid.Clear();

        #endregion
    }
}
