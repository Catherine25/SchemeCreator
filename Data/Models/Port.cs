using SchemeCreator.Data.Models.Enums;
using System;

namespace SchemeCreator.Data.Models
{
    public class Port : SmartEllipse
    {
        public readonly ConnectionTypeEnum Type;

        public Port(ConnectionTypeEnum connectionType)
        {
            Type = connectionType;
            SetCenterAndSize(null, Constants.gatePortSize);
            base.Tapped += (SmartEllipse e) => Tapped(this);
        }

        public new Action<Port> Tapped; 
    }
}