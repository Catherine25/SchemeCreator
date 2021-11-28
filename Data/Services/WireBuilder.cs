using System.Numerics;
using SchemeCreator.Data.Extensions;
using SchemeCreator.UI.Dynamic;
using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data.Services
{
    public class WireBuilder
    {
        public Action<WireView> WireReady;
        
        public WireBuilder(Grid schemeGrid) => this.schemeGrid = schemeGrid;

        private readonly Grid schemeGrid;

        public void Connect(GatePortView port, GateView gate) => SetPoint(port.Type != ConnectionTypeEnum.Input, 
            port.GetCenterRelativeTo(schemeGrid), gate.MatrixLocation, port.Index);
        
        public void Connect(ExternalPortView externalPort) => SetPoint(externalPort.Type == PortType.Input, 
            externalPort.GetCenterRelativeTo(schemeGrid), externalPort.MatrixLocation);

        private void SetPoint(bool isStart, Point point, Vector2 location, int? port = null)
        {
            WireConnection con = wire.Connection;

            if(isStart)
            {
                con.StartPoint = point;
                con.MatrixStart = location;
                con.StartPort = port;
                wire.SetConnection(con);
            }
            else
            {
                con.EndPoint = point;
                con.MatrixEnd = location;
                con.EndPort = port;
                wire.SetConnection(con);
            }

            if (!wire.Connection.StartPoint.IsInitialized() || !wire.Connection.EndPoint.IsInitialized())
                return;
            
            WireReady(wire);
            wire = new WireView();
        }

        private WireView wire = new();
    }
}
