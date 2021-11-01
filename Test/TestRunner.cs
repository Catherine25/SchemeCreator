using SchemeCreator.UI;
using System;

namespace SchemeCreator.Test
{
    public class TestRunner
    {
        public TestRunner(SchemeView scheme) => this.scheme = scheme;

        private readonly SchemeView scheme;

        public bool Run(Action<SchemeView> test)
        {
            try
            {
                test(scheme);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
