using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Domain;

namespace Dashboard.API.Models
{
    public class ProductViewModel
    {
        public long Id { get; set; }
        public DashboardView DashboardView { get; set; }
        public long ProductId { get; set; }
        public IEnumerable<ProductViewModel> Children { get; set; }
    }
}
