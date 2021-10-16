using SchemeCreator.Data;
using SchemeCreator.Data.Extensions;
using SchemeCreator.UI.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.UI.Layers
{
    public sealed partial class ExternalPortsLayer : UserControl
    {
        public IList<ExternalPortView> ExternalPorts { get; private set; }
        public event Action<ExternalPortView> ExternalPortTapped;
        
        public ExternalPortsLayer()
        {
            InitializeComponent();
            Grid.InitGridColumnsAndRows(Constants.GridSize);
            ExternalPorts = new List<ExternalPortView>();
        }

        public IEnumerable<ExternalPortView> GetExternalPorts(PortType type) =>
            ExternalPorts.Where(x => x.Type == type);
        
        public ExternalPortView GetFirstNotInitedExternalPort() =>
            GetExternalPorts(PortType.Input)
                .FirstOrDefault(x => x.Value == null);

        public void AddToView(ExternalPortView port)
        {
            port.Tapped += (port) => ExternalPortTapped(port);
            Grid.Children.Add(port);
            ExternalPorts.Add(port);
        }

        public void Clear()
        {
            ExternalPorts.Clear();
            Grid.Children.Clear();
        }
    }
}
