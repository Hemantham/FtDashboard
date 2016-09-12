using System;
using System.Collections.Generic;
using Dashboard.API.API;
using Dashboard.API.Domain;
using Dashboard.API.Models;
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
            //    var time1 = DateTime.Now;
            //    var charts = _chartDataService.GetChartValues(
            //            new ChartSearchCriteria
            //            {
            //                Filters =  new List<ValueFilter>
            //                {
            //                    new ValueFilter
            //                    {
            //                        Code = "GROUPS",
            //                        Answer = "CONSUMER"
            //                    },
            //                    new ValueFilter
            //                    {
            //                         Code = "CHURNER_FLAG",
            //                         Answer = "CHURNER"
            //                    },
            //                    new ValueFilter
            //                    {
            //                         Code = "OLDPRODUCT",
            //                         Answer = "Overall Fixed"
            //                    },

            //                }
            //            }
            //        );
            //    var time2 = DateTime.Now;
            //    Console.WriteLine((time2 - time1).TotalSeconds);
            //    Assert.IsNotNull(charts);
            //
        }
    }
}


