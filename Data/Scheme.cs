using System.Collections.Generic;

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

        public bool IsExternalInputsInited()
        {
            IList<Gate> exINs = gateController.GetExternalInputs();

            //check is all exINs are inited
            for (int i = 0; i < exINs.Count; i++)
                if (exINs[i].values[0] == null)
                    return false;

            return true;
        }

        public bool IsAllConnected()
        {
            int wLength = lineController.Wires.Count;
            int gLength = gateController.Gates.Count;
            int inputsCount = 0;

            for (int g = 0; g < gLength; g++)
            {
                inputsCount += gateController.Gates[g].inputs;
                inputsCount += gateController.Gates[g].outputs;
            }

            int counter = 0;

            for (int i = 0; i < wLength; i++)
            {
                for (int j = 0; j < gLength; j++)
                {
                    Gate g = gateController.Gates[j];

                    Wire w = lineController.Wires[i];

                    if (g.WireConnects(w.start) || g.WireConnects(w.end))
                        counter++;
                }
            }

            if (inputsCount == counter)
                return true;
            else
                return false;

        }
    }
}