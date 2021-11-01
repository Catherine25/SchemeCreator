﻿using System.Collections.Generic;
using System.Linq;
using SchemeCreator.Data.Exceptions.Displayable;
using SchemeCreator.UI;
using SchemeCreator.Data.Extensions;
using SchemeCreator.Data.Interfaces;

namespace SchemeCreator.Data.Services.Alignment
{
    public class Aligner
    {
        private readonly SchemeView scheme;
        private readonly ExternalInputsAligner inputsAligner;
        private readonly ExternalOutputsAligner outputsAligner;
        private readonly GatesAligner gatesAligner;
        private HashSet<ISchemeComponent> processed;

        public Aligner(SchemeView scheme)
        {
            this.scheme = scheme;
            processed = new HashSet<ISchemeComponent>();
            inputsAligner = new ExternalInputsAligner(scheme);
            outputsAligner = new ExternalOutputsAligner(scheme);
            gatesAligner = new GatesAligner(scheme);
        }

        public void Run()
        {
            this.Log("Running");
            
            this.Log("Validation...");
            Validate();
            this.Log("OK");

            this.Log("Moving external outputs...");
            processed = outputsAligner.MoveExternalOutputs(processed);

            this.Log("Moving gates...");
            gatesAligner.MoveGates();

            this.Log("Moving external inputs...");
            processed = inputsAligner.MoveExternalInputs(processed);

            this.Log("Done");
        }

        private void Validate()
        {
            if (scheme.ExternalOutputs.Count() > SchemeView.GridSize.Height)
                throw new TooManyExternalOutputsException();
            if(!scheme.ExternalOutputs.Any())
                throw new NoExternalOutputsException();
            
            if (scheme.ExternalInputs.Count() > SchemeView.GridSize.Height)
                throw new TooManyExternalInputsException();
            if(!scheme.ExternalInputs.Any())
                throw new NoExternalInputsException();
        }
    }
}
