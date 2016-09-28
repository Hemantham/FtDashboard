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
    public class DashboardController : ApiController
    {
        private readonly IDashboardService _service;
        private readonly IChartDataService _chartService;

        public DashboardController(IDashboardService service, IChartDataService chartService)
        {
            _service = service;
            _chartService = chartService;
        }

        // GET api/values
        [HttpGet]
        [Route("api/products")]
        public IEnumerable<Product> GetProducts()
        {
            return _service.GetProducts();
        }

        [HttpGet]
        [Route("api/products/{id:int}")]
        public Product GetProduct(int id)
        {
            return _service.GetProduct(id);
        }

        [HttpGet]
        [Route("api/products/{id:int}/views")]
        public IEnumerable<ProductViewModel> GetViews(int id)
        {
            return _service.GetProductViewModels(id);
        }

        [HttpGet]
        [Route("api/dashboardviews")]
        public IEnumerable<DashboardView> GetDashboradViews()
        {
            return _service.GetDashboardViews();
        }


        [HttpGet]
        [Route("api/products/views/{id:int}/splitfilters")]
        public IEnumerable<FieldValueModel> GetFieldValues(int id)
        {
            return _chartService.GetFieldValues(id);
        }

        [HttpGet]
        [Route("api/products/views/{id:int}")]
        public ProductView GetView(int id)
        {
            return _service.GetProductView(id);
        }
    }
}
