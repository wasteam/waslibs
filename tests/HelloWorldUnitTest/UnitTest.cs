using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace HelloWorldUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(true, "it works!");
        }
    }
}
