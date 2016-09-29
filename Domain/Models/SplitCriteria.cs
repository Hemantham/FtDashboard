using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class SplitCriteria
    {
        public long ViewSplitId { get; set; }
        public IEnumerable<object> SplitFilters { get; set; }
    }
}
