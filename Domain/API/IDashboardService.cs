using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Domain;
using Dashboard.API.Models;

namespace Dashboard.API.API
{
    public interface IDashboardService
    {
        IEnumerable<Filter> GetProducts();
        Filter GetProduct(long productId);
        IEnumerable<FilteredDashboardView> GetProductViews(long productId);
        FilteredDashboardView GetProductView(long productViewId);
        IEnumerable<ProductViewModel> GetProductViewModels(long productId);
        IEnumerable<DashboardView> GetDashboardViews();
        ViewSplit GetViewSplit(long id);
    }
}
