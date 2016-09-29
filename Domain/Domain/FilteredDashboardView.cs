using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Domain
{
    public class FilteredDashboardView : DataEntity
    {
        public ICollection<ViewSplit> ViewSplits { get; set; }
        public DashboardView DashboardView { get; set; }
        public Filter Filter { get; set; }

    }
}
