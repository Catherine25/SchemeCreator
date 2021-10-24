namespace SchemeCreator.Data.Services
{
    public static class Messages
    {
        public static readonly MessageData NothingToTrace = new()
        {
            Title = "Can't trace the scheme",
            Description = "At least one external port and at least one gate are needed.\n" + 
                          "Click on a dot to create a component.",
            SecondaryButton = OK
        };

        public static readonly MessageData NoExternalInputs = new()
        {
            Title = "No external inputs",
            Description = "Scheme has no external inputs.\n" + 
                          "Please add at least one.",
            SecondaryButton = OK
        };

        public static readonly MessageData NoExternalOutputs = new()
        {
            Title = "No external outputs",
            Description = "Scheme has no external outputs.\n" +
                          "Please add at least one.",
            SecondaryButton = OK
        };

        public static readonly MessageData ExternalInputsNotInited = new()
        {
            Title = "External input ports are not inited",
            Description = "External input ports of the scheme are not inited.\n" +
                          "All the ports must have a value (0 or 1).\n" +
                          "Please click on them to change their values.",
            SecondaryButton = OK
        };

        public static readonly MessageData NoWires = new()
        {
            Title = "No wires",
            Description = "The scheme has no wires added.\n" +
                          "Please add at least one.",
            SecondaryButton = OK
        };

        public static readonly MessageData CreateNew = new()
        {
            Title = "Create new scheme",
            Description = "Do you want to create new scheme?\n" +
                          "Current scheme will be lost.",
            PrimaryButton = "Create",
            SecondaryButton = "Cancel"
        };

        public static readonly MessageData ImpossibleToVisualize = new()
        {
            Title = "It's impossible to visualize scheme",
            Description = "You have some feedbacks in the scheme.",
            SecondaryButton = OK
        };
        
        public static readonly MessageData TracingError = new()
        {
            Title = "Can't trace the scheme",
            Description = "Scheme isn't correct.\n" + 
                          "Please check it for error.",
            SecondaryButton = OK
        };

        private const string OK = "OK";
    }
}
