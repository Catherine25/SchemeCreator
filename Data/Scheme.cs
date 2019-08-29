namespace SchemeCreator.Data
{
    public class Scheme
    {
        public UI.FrameManager frameManager;
        public LineController lineController = new LineController();
        public GateController gateController = new GateController();
        public DotController dotController = new DotController();

        //constructors
        public Scheme() => frameManager = new UI.FrameManager(this);
    }
}