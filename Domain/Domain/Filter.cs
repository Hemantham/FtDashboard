using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Domain
{
    public class Filter : DataEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<FilteredDashboardView> ProductViews { get; set; }
        public string FilterString { get; set; }
        public string Group { get; set; }

    }
}
