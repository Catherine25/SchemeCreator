using System.Diagnostics;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Services.History;
using SchemeCreator.UI;
using SchemeCreator.UI.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SchemeCreator.Data.Services.Navigation;

namespace SchemeCreator.Data.Services.Alignment
{
    public class Aliner
    {
        private SchemeView scheme;
        private SchemeNavigationService navigationService;
        private ExternalInputsAligner inputsAligner;
        private ExternalOutputsAligner outputsAligner;
        private GatesAligner gatesAligner;

        public Aliner(SchemeView scheme, HistoryService history)
        {
            navigationService = new(scheme);
            inputsAligner = new ExternalInputsAligner(scheme);
            outputsAligner = new ExternalOutputsAligner(scheme);
            gatesAligner = new GatesAligner(scheme, history, new(scheme));
        }

        public void Run()
        {
            Debug.WriteLine("Running Liner...");

            Debug.WriteLine("Moving external inputs...");
            inputsAligner.MoveExternalInputs();

            Debug.WriteLine("Moving gates...");
            gatesAligner.MoveGates();

            Debug.WriteLine("Moving external outputs...");
            outputsAligner.MoveExternalOutputs();

            Debug.WriteLine("Finished running Liner");
        }
    }
}
