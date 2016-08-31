using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public class ChartSearchCriteria
    {
        public IEnumerable<ChartFilter> Filters { get; set; }
        public string XAxislable { get; set; }
        public string XAxisId { get; set; }

    }
}
