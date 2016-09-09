using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class DataChart
    {
        public string ChartName { get; set; }
        public IEnumerable<ChartEntry> ChartValues { get; set; }
    }
}
