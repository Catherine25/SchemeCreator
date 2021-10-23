using System.Numerics;
using SchemeCreator.Data.Extensions;
using SchemeCreator.UI.Dynamic;
using System;
using Windows.Foundation;

namespace SchemeCreator.Data.Services
{
    public class WireBuilder
    {
        public Action<WireView> WireReady;

        public void SetPoint(bool isStart, Point point, Vector2 location, int? port = null)
        {
            WireConnection con = _wire.Connection;

            if(isStart)
            {
                con.StartPoint = point;
                con.MatrixStart = location;
                con.StartPort = port;
                _wire.SetConnection(con);
            }
            else
            {
                con.EndPoint = point;
                con.MatrixEnd = location;
                con.EndPort = port;
                _wire.SetConnection(con);
            }

            if (_wire.Connection.StartPoint.IsInited() && _wire.Connection.EndPoint.IsInited())
            {
                WireReady(_wire);
                _wire = new WireView();
            }
        }

        private WireView _wire = new();
    }
}
