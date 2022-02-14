using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
}

namespace UnitTestStore

{
    public abstract class UTBase
    {
        private AppsContext _context;

        public AppsContext GetStoreContext()
        {
            return _context;
        }
    }

    [TestClass]
    public class UnitTest1 
    {
        private readonly AppsContext _context;
        

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
