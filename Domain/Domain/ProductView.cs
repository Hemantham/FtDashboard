using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Domain
{
    public class ProductView : DataEntity
    {
        public ICollection<ViewSplit> ViewSplits { get; set; }
        public DashboardView DashboardView { get; set; }
        public Product Product { get; set; }

    }
}
