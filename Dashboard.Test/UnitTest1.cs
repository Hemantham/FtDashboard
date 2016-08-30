using System;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dashboard.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var charts = new Question().Get();
            Assert.IsNotNull(charts);
        }
    }
}
