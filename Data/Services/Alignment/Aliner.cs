using SchemeCreator.UI;
using SchemeCreator.Data.Extensions;

namespace SchemeCreator.Data.Services.Alignment
{
    public class Aliner
    {
        private SchemeView scheme;
        private ExternalInputsAligner inputsAligner;
        private ExternalOutputsAligner outputsAligner;
        private GatesAligner gatesAligner;

        public Aliner(SchemeView scheme)
        {
            inputsAligner = new(scheme);
            outputsAligner = new(scheme);
            gatesAligner = new(scheme);
        }

        public void Run()
        {
            this.Log("Running");

            this.Log("Moving external outputs...");
            outputsAligner.MoveExternalOutputs();

            this.Log("Moving gates...");
            gatesAligner.MoveGates();

            this.Log("Moving external inputs...");
            inputsAligner.MoveExternalInputs();

            this.Log("Done");
        }
    }
}
