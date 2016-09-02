using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public class ChartEntry
    {
        public object Value { get; set; }
        public string XAxisLable { get; set; }
        public object XAxisValue { get; set; }
        public long XAxisId { get; set; }
        public string Series { get; set; }
      
    }


}
