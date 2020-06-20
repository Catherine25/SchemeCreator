namespace SchemeCreator.Data.Models
{
    class ExternalPort : SmartEllipse
    {
        public ExternalPort()
        {
            SetCenterAndSize(null, Constants.externalPortSize);
            BooleanValue = null;
        }
    }
}
