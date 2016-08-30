using System;
using Dashboard.API.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dashboard.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var time1 = DateTime.Now;
            var charts = new Question().Get();
            var time2 = DateTime.Now;
            Console.WriteLine((time2 - time1).TotalSeconds);
            Assert.IsNotNull(charts);
        }
    }
}
