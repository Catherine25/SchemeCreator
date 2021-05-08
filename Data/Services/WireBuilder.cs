using SchemeCreator.Data.Extensions;
using SchemeCreator.UI.Dynamic;
using System;
using Windows.Foundation;

namespace SchemeCreator.Data.Services
{
    public class WireBuilder
    {
        public Action<WireView> WireReady;

        public void SetPoint(bool isStart, Point point)
        {
            if(isStart)
                _wire.Start = point;
            else
                _wire.End = point;

            if (_wire.Start.IsInited() && _wire.End.IsInited())
            {
                WireReady(_wire);
                _wire = new WireView();
            }
        }

        private WireView _wire = new();
    }
}
