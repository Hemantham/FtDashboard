using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class ChartsContainerModel
    {
        public IEnumerable<DataChart> Charts { get; set; }
        public IEnumerable<Recency> AvailableRecencies { get; set; }

    }
}
