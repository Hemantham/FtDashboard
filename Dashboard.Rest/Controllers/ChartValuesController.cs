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
        public ChartsContainerModel GetChartEntries(ChartSearchCriteria criteria)
        {
            if (criteria.FilteredDashboardViewId > 0)
            {
                return _chartDataService.GetChartsContainerModel(criteria);
            }
            else
            {
                return _chartDataService.GetChartsContainerModelForMultipleFilters(criteria);
            }
        }

        //[HttpPost]
        //[Route("api/charts/data/overall")]
        //public ChartsContainerModel GetChartEntrieOveralls(ComparisonChartSearchCriteria criteria)
        //{
        //    return _chartDataService.GetChartsContainerModelForMultipleProducts(criteria);
        //}

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
