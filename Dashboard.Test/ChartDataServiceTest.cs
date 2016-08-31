using System;
using System.Collections.Generic;
using Dashboard.API.API;
using Dashboard.API.Domain;
using Dashboard.Models;
using Dashboard.Models.Constants;
using Dashboard.Services;
using DataEf.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Dashboard.Test
{
    [TestClass]
    public class ChartDataServiceTest
    {
        private readonly IChartDataService _chartDataService;
        public ChartDataServiceTest()
        {
            Bootstrapper.CommonBootstrapper.LoadFromCurrentAssembly();
            _chartDataService = Common.Container.Get<IChartDataService>();
        }

        [TestMethod]
        public void TestGetChartEntries()
        {
            var time1 = DateTime.Now;
            var charts = _chartDataService.GetChartEntries(
                    new ChartSearchCriteria
                    {
                        Filters =  new List<ChartFilter>
                        {
                            new ChartFilter
                            {
                                Code = "GROUPS",
                                Answer = "CONSUMER"
                            },
                            new ChartFilter
                            {
                                 Code = "CHURNER_FLAG",
                                 Answer = "CHURNER"
                            },
                            new ChartFilter
                            {
                                 Code = "OLDPRODUCT",
                                 Answer = "Overall Fixed"
                            },
                          
                        }
                    }
                );
            var time2 = DateTime.Now;
            Console.WriteLine((time2 - time1).TotalSeconds);
            Assert.IsNotNull(charts);
        }
    }
}


