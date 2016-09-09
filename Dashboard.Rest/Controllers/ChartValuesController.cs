using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dashboard.API.API;
using Dashboard.Models;
using Dashboard.Rest.Models.Charts;

namespace Dashboard.Rest.Controllers
{
   // [Authorize]
    public class ChartValuesController : ApiController
    {
        private IChartDataService _chartDataService;

        public ChartValuesController(IChartDataService chartDataService)
        {
            _chartDataService = chartDataService;
        }

        // GET api/values
        [HttpGet]
        [Route("api/chartvalues")]
        public IEnumerable<ChartEntry> Get()
        {
            return GetChartEntries(2);
        }

        [HttpGet]
        [Route("api/questions/{code}/answers")]
        public IEnumerable<ChartEntry> GetFieldValues(int productViewId)
        {
            return _chartDataService.GetChartValues(productViewId);
        }


        private IEnumerable<ChartEntry> GetChartEntries(int productViewId)
        {

            return _chartDataService.GetChartValues(productViewId);

            //chartCriteria ??
            //new ChartSearchCriteria
            //    {
            //        Filters = new List<ValueFilter>
            //    {
            //        new ValueFilter
            //        {
            //            Code = "GROUPS",
            //            Answer = "CONSUMER"
            //        },
            //        new ValueFilter
            //        {
            //            Code = "CHURNER_FLAG",
            //            Answer = "CHURNER"
            //        },
            //        new ValueFilter
            //        {
            //            Code = "OLDPRODUCT",
            //            Answer = "Overall Fixed"
            //        },
            //    },
            //        FieldOfInterest = "CHURN1",
            //        XAxisId = "ANALYSED_Week_#",
            //        XAxislable = "ANALYSED_Week",
            //    }
            //);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

       

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
