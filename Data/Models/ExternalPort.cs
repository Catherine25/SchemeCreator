namespace SchemeCreator.Data.Models
{
    class ExternalPort : SmartEllipse
    {
        public ExternalPort()
        {
            //SetCenterAndSize(null, Constants.externalPortSize);
            SetSize(Constants.ExternalPortSize);
            BooleanValue = null;
        }
    }
}
