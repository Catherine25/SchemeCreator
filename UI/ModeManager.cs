using System;
using System.Collections.Generic;

namespace SchemeCreator.UI {
    class ModeManager {
        //constructors
        public ModeManager() {}
        //public methods
        public Constants.ModeEnum? CurrentMode {get; set;}
        //data
        Constants.ModeEnum currentMode;
    }
}