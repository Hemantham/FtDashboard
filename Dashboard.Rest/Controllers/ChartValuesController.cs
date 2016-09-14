using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dashboard.API.API;
using Dashboard.API.Domain;
using Dashboard.API.Models;
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

        //// GET api/values
        //[HttpGet]
        //[Route("api/chartvalues")]
        //public IEnumerable<DataChart> Get()
        //{
        //    return GetChartEntries(null);
        //}

        [HttpGet]
        [Route("api/questions/answers")]
        public IEnumerable<Response> GetFieldValues(FieldSearchCriteria criteria)
        {
            return _chartDataService.GetFieldValues(criteria);
        }

        
        [HttpPost]
        [Route("api/charts/data")]
        public IEnumerable<DataChart> GetChartEntries(ChartSearchCriteria criteria)
        {
            return _chartDataService.GetChartValues(criteria);

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
