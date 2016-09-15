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
        IEnumerable<Product> GetProducts();
        Product GetProduct(long productId);
        IEnumerable<ProductView> GetProductViews(long productId);
        ProductView GetProductView(long productViewId);
        IEnumerable<ProductViewModel> GetProductViewModels(long productId);
        ViewSplit GetViewSplit(long id);
    }
}
