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
        IEnumerable<Filter> GetFilters();
        Filter GetFilter(long productId);
        IEnumerable<FilteredDashboardView> GetFilteredViews(long productId);
        FilteredDashboardView GetFilteredView(long productViewId);
        IEnumerable<ProductViewModel> GetProductViewModels(long productId);
        IEnumerable<DashboardView> GetDashboardViews();
        ViewSplit GetViewSplit(long id);
    }
}
