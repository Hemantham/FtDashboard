using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dashboard.API.API;
using Dashboard.Models;

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
        public IEnumerable<ChartEntry> Get()
        {
            var chartvalues = _chartDataService.GetChartEntries(new ChartSearchCriteria
            {
                Filters = new List<ChartFilter>
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
            });

            return chartvalues;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
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
