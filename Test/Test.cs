using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeCreator.Test {
    class Test {
        public void init_test_1() {
            var scheme = new SchemeCreator.Data.Scheme();
            System.Diagnostics.Debug.Assert(scheme != null);
        }
        
    }
}
