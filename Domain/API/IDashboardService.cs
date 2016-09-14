using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Domain;

namespace Dashboard.API.API
{
    public interface IDashboardService
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(long productId);
        IEnumerable<ProductView> GetProductViews(long productId);
        ProductView GetProductView(long productViewId);
        ViewSplit GetViewSplit(long id);
    }
}
