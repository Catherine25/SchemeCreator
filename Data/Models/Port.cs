using SchemeCreator.Data.Models.Enums;
using System;

namespace SchemeCreator.Data.Models
{
    public class Port : SmartEllipse
    {
        public readonly ConnectionType Type;

        public Port(ConnectionType connectionType)
        {
            Type = connectionType;
            SetCenterAndSize(null, Constants.gatePortSize);
            base.Tapped += (SmartEllipse e) => Tapped(this);
        }

        public new Action<Port> Tapped; 
    }
}