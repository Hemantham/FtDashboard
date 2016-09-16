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

        [HttpGet]
        [Route("api/charts/recencytypes")]
        public IEnumerable<RecencyType> GetRecencyTypes()
        {
            return _chartDataService.GetRecencyTypes();

        }

        [HttpPost]
        [Route("api/charts/data")]
        public IEnumerable<DataChart> GetChartEntries(ChartSearchCriteria criteria)
        {
            return _chartDataService.GetChartValues(criteria);

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
