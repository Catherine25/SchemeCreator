using SchemeCreator.Data;
using System;
using System.Collections.Generic;
using SchemeCreator.Data.ConstantsNamespace;

namespace SchemeCreator.UI {
    class ModeManager {
        //constructors
        public ModeManager() {}
        //public methods
        public Constants.ModeEnum CurrentMode {get; set;}
    }
}