using SchemeCreator.Data;
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
        public event Action<ExternalPortView> ExternalPortTapped;

        public IEnumerable<ExternalPortView> Items => Grid.Children.Select(x => x as ExternalPortView);

        public ExternalPortsLayer()
        {
            InitializeComponent();
            Grid.InitGridColumnsAndRows(Constants.GridSize);
        }

        public ExternalPortView GetFirstNotInitedExternalPort() => Items.Where(x => x.Type == PortType.Input).FirstOrDefault(x => x.Value == null);

        public void AddToView(ExternalPortView port)
        {
            port.Tapped += (port) => ExternalPortTapped(port);
            Grid.Children.Add(port);
        }

        public void Clear() => Grid.Children.Clear();
    }
}
